﻿Option Strict On

Imports System.Data.SqlClient
Imports Purchase.Common

Partial Public Class POIssue
    Inherits CommonPage

    Protected st_RFQLineNumber As String
    Protected st_ParPONumber As String
    Protected st_Action As String
    Protected st_LoginLocationCode As String

    ' 登録フォームを表示するか否か
    Protected bo_DisplayForm As Boolean

    Private Const ERR_RFQ_NOT_FOUND As String = "No requested enquiry record found."
    Private Const ERR_PAR_PO_NOT_FOUND As String = "No parent PO found."
    Private Const ERR_NO_QUOTATION_REPLY As String = "No quotation record found.<br />(You can not issue any order without quotation record.)"
    Private Const ERR_R3_SUPPLIER_DOES_NOT_EXIST As String = "No R3 Company code found.<br />(You can not issue any order without R3 company code.)"
    Private Const ERR_CHI_PO_ALREADY_EXISTS As String = "Other child PO has already issued."
    Private Const ERR_PAR_PO_NOT_ASSIGN As String = "The parent PO is assigned to nobody."
    Private Const EXP_PO_ISSUE_ERROR As String = "POIssue.Issue_Click: 発注番号が採番されませんでした。"
    Private Const EXP_RFQ_UNIT_CODE_UNUSABLE As String = "The Enq-Unit 'ZZ' is unusable for PO."

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        bo_DisplayForm = False

        If Request.RequestType = "POST" Then
            st_RFQLineNumber = Request.Form("RFQLineNumber")
            st_ParPONumber = Request.Form("ParPONumber")
            st_Action = Request.Form("Action")
        Else
            st_RFQLineNumber = Request.QueryString("RFQLineNumber")
            st_ParPONumber = Request.QueryString("ParPONumber")
            st_Action = Request.QueryString("Action")
        End If

        st_LoginLocationCode = Session("LocationCode").ToString

        If String.IsNullOrEmpty(st_RFQLineNumber) Or Not IsInteger(st_RFQLineNumber) Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        If Not IsPostBack Then
            If Not String.IsNullOrEmpty(st_ParPONumber) Then
                If Not IsInteger(st_ParPONumber) Then
                    Msg.Text = ERR_INVALID_PARAMETER
                    Exit Sub
                End If

                If Not ExistenceConfirmation("PO", "PONumber", st_ParPONumber) Then
                    Msg.Text = ERR_PAR_PO_NOT_FOUND
                    Exit Sub
                End If

                ' 同じ親を持つ子 PO が存在する場合はエラーとします
                If ExistenceConfirmation("PO", "ParPONumber", st_ParPONumber) Then
                    Msg.Text = ERR_CHI_PO_ALREADY_EXISTS
                    Exit Sub
                End If

                '親 PO に発注先担当者の設定がない場合はエラーとします
                If GetSOUserID(st_ParPONumber) = String.Empty Then
                    Msg.Text = ERR_PAR_PO_NOT_ASSIGN
                    Exit Sub
                End If
            End If

            'パラメータ RFQLineNumber に該当する RFQLine.EnqUnitCode が 'ZZ' の場合はエラーとします。
            If GetEnqUnitCode(st_RFQLineNumber) = "ZZ" Then
                Msg.Text = EXP_RFQ_UNIT_CODE_UNUSABLE
                Exit Sub
            End If

            If SetControl() = False Then
                Exit Sub
            End If
        End If

        bo_DisplayForm = True

    End Sub

    Protected Sub Issue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Issue.Click
        Dim i_PONumber As Integer = 0

        If st_Action <> "Issue" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        If Not String.IsNullOrEmpty(st_ParPONumber) Then
            ' 同じ親を持つ子 PO が存在する場合はエラーとします
            ' ※ブラウザの戻るボタン対策です
            If ExistenceConfirmation("PO", "ParPONumber", st_ParPONumber) Then
                Msg.Text = ERR_CHI_PO_ALREADY_EXISTS
                Exit Sub
            End If
        End If

        If ValidateField() = False Then
            Exit Sub
        End If

        i_PONumber = InsertPO()

        Response.Redirect(String.Format("POUpdate.aspx?PONumber={0}", i_PONumber.ToString))

    End Sub

    Private Function SetControl() As Boolean
        Dim de_OrderQuantity As Decimal = 0
        Dim de_PerQuantity As Decimal = 0
        Dim de_UnitPrice As Decimal = 0
        
        Dim sqlConn As New SqlConnection(DB_CONNECT_STRING)
        Dim sqlAdapter As New SqlDataAdapter
        Dim sqlCmd As New SqlCommand(CreateSql_SelectRFQLine(), sqlConn)
        Dim ds As New DataSet

        sqlAdapter.SelectCommand = sqlCmd
        sqlCmd.Parameters.AddWithValue("@RFQLineNumber", st_RFQLineNumber)
        sqlAdapter.Fill(ds, "RFQLine")

        If ds.Tables("RFQLine").Rows.Count = 0 Then
            Msg.Text = ERR_RFQ_NOT_FOUND
            Return False
        End If

        'HACK BugTrack-Purchase/227 R/3と連携まで'R3SupplierCode'の確認はしない
        'If IsDBNull(ds.Tables("RFQLine").Rows(0)("R3SupplierCode")) Then
        '    Msg.Text = ERR_R3_SUPPLIER_DOES_NOT_EXIST
        '    Return False
        'End If

        '権限ロールに従い極秘品はエラーとする
        If Session(SESSION_ROLE_CODE).ToString = ROLE_WRITE_P OrElse Session(SESSION_ROLE_CODE).ToString = ROLE_READ_P Then
            If IsConfidentialItem(ds.Tables("RFQLine").Rows(0)("RFQNumber").ToString) Then
                Response.Redirect("AuthError.html")
            End If
        End If

        If IsDBNull(ds.Tables("RFQLine").Rows(0)("EnqQuantity")) _
            Or IsDBNull(ds.Tables("RFQLine").Rows(0)("EnqPiece")) _
            Or IsDBNull(ds.Tables("RFQLine").Rows(0)("UnitPrice")) _
            Or IsDBNull(ds.Tables("RFQLine").Rows(0)("QuoPer")) Then

            Msg.Text = ERR_NO_QUOTATION_REPLY
            Return False
        End If

        RFQNumber.Text = ds.Tables("RFQLine").Rows(0)("RFQNumber").ToString
        ParPONumber_Label.Text = st_ParPONumber
        PODate.Text = GetLocalTime(Session("LocationCode").ToString, Now.Date, False, False)

        If Not CBool(Session("Purchase.isAdmin")) Then
            POUser.SelectedValue = Session("UserID").ToString
        End If

        '親POがある場合はその発注先担当者(SOUser)を設定し、選択固定とします。
        Dim st_ParentSOUserID As String = GetSOUserID(st_ParPONumber)
        If Not st_ParentSOUserID = String.Empty Then
            POUser.SelectedValue = st_ParentSOUserID
            POUser.Enabled = False
        End If

        POLocationName.Text = Session("LocationName").ToString
        ProductNumber.Text = ds.Tables("RFQLine").Rows(0)("ProductNumber").ToString
        ProductName.Text = CutShort(ds.Tables("RFQLine").Rows(0)("ProductName").ToString)
        Confidential.Text = IIf(CBool(ds.Tables("RFQLine").Rows(0)("isCONFIDENTIAL")), Common.CONFIDENTIAL, String.Empty).ToString
        de_OrderQuantity = CDec(ds.Tables("RFQLine").Rows(0)("EnqQuantity")) * CInt(ds.Tables("RFQLine").Rows(0)("EnqPiece"))
        OrderQuantity.Text = de_OrderQuantity.ToString("G29")
        OrderUnit.SelectedValue = ds.Tables("RFQLine").Rows(0)("EnqUnitCode").ToString
        CurrencyCode.Text = ds.Tables("RFQLine").Rows(0)("CurrencyCode").ToString
        de_UnitPrice = CDec(ds.Tables("RFQLine").Rows(0)("UnitPrice"))
        UnitPrice.Text = de_UnitPrice.ToString("G29")
        de_PerQuantity = CDec(ds.Tables("RFQLine").Rows(0)("QuoPer"))
        PerQuantity.Text = de_PerQuantity.ToString("G29")
        PerUnit.Text = ds.Tables("RFQLine").Rows(0)("QuoUnitCode").ToString
        R3MakerCode.Text = ds.Tables("RFQLine").Rows(0)("R3MakerCode").ToString
        R3MakerName.Text = ds.Tables("RFQLine").Rows(0)("R3MakerName").ToString
        PaymentTerm.Text = ds.Tables("RFQLine").Rows(0)("PaymentTerm").ToString
        Incoterms.Text = ds.Tables("RFQLine").Rows(0)("Incoterms").ToString
        DeliveryTerm.Text = ds.Tables("RFQLine").Rows(0)("DeliveryTerm").ToString
        SupplierItemNumber.Text = ds.Tables("RFQLine").Rows(0)("SupplierItemNumber").ToString


        ' HiddenField
        RFQLineNumber.Value = st_RFQLineNumber
        ParPONumber.Value = st_ParPONumber
        POLocationCode.Value = Session("LocationCode").ToString
        ProductID.Value = ds.Tables("RFQLine").Rows(0)("ProductID").ToString
        MakerCode.Value = ds.Tables("RFQLine").Rows(0)("MakerCode").ToString
        PaymentTermCode.Value = ds.Tables("RFQLine").Rows(0)("PaymentTermCode").ToString
        IncotermsCode.Value = ds.Tables("RFQLine").Rows(0)("IncotermsCode").ToString

        ' SqlDataSource
        SetControl_SrcUser(CBool(ds.Tables("RFQLine").Rows(0)("isCONFIDENTIAL")))
        SetUnitDropDownList(SrcUnit)
        SetSupplierDropDownList(SrcSupplier, ds.Tables("RFQLine").Rows(0)("SupplierCode").ToString, _
                               ds.Tables("RFQLine").Rows(0)("QuoLocationCode").ToString, Session("LocationCode").ToString)
        SetPurposeDropDownList(SrcPurpose)

        SetControl_Priority(st_ParPONumber)
        '親POがある場合はPriorityは編集不可。
        If Not String.IsNullOrEmpty(st_ParPONumber) Then
            Priority.Visible = False
            LabelPriority.Visible = True
        Else
            Priority.Visible = True
            LabelPriority.Visible = False
        End If

        'Purposeの表示
        If String.IsNullOrEmpty(st_ParPONumber) Then
            '親POの場合
            LabelPurpose.Visible = False
            Purpose.Visible = True
        Else
            '子POの場合
            Dim ParPOPurposeCode As String = String.Empty
            Dim ParPOPurposeText As String = String.Empty
            GetParPO_Purpose(st_ParPONumber, ParPOPurposeCode, ParPOPurposeText)
            ParPO_PurposeCode.Value = ParPOPurposeCode
            LabelPurpose.Text = ParPOPurposeText
            LabelPurpose.Visible = True
            Purpose.Visible = False
        End If

        Return True

    End Function

    Public Function GetSOUserID(ByVal PONumber As String) As String

        If PONumber = String.Empty Then
            Return String.Empty
        End If

        Dim sqlConn As SqlConnection = Nothing

        Try
            sqlConn = New SqlConnection(DB_CONNECT_STRING)
            Dim sqlCmd As New SqlCommand(GetSQL_SelectSOUserID(), sqlConn)
            sqlCmd.Parameters.AddWithValue("PONumber", PONumber)
            sqlConn.Open()

            Dim obj_Return As Object = sqlCmd.ExecuteScalar()

            If obj_Return Is Nothing Then
                Return String.Empty
            End If

            Return obj_Return.ToString()

        Finally
            If Not (sqlConn Is Nothing) Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If

        End Try

    End Function

    Public Function GetEnqUnitCode(ByVal RFQLineNumber As String) As String

        If RFQLineNumber = String.Empty Then
            Return String.Empty
        End If

        Dim sqlConn As SqlConnection = Nothing

        Try
            sqlConn = New SqlConnection(DB_CONNECT_STRING)
            Dim sqlCmd As New SqlCommand(GetSQL_SelectEnqUnitCode(), sqlConn)
            sqlCmd.Parameters.AddWithValue("RFQLineNumber", RFQLineNumber)
            sqlConn.Open()

            Dim obj_Return As Object = sqlCmd.ExecuteScalar()

            If obj_Return Is Nothing Then
                Return String.Empty
            End If

            Return obj_Return.ToString()

        Finally

            If Not (sqlConn Is Nothing) Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If

        End Try

    End Function

    Private Sub SetControl_SrcUser(ByVal IsConfidential As Boolean)

        If IsConfidential Then
            SrcUser.SelectCommand = "SELECT UserID, Name FROM v_User WHERE LocationCode = @LocationCode AND isDisabled = 0 AND RoleCode = 'WRITE' ORDER BY Name"
        Else
            SrcUser.SelectCommand = "SELECT UserID, Name FROM v_User WHERE LocationCode = @LocationCode AND isDisabled = 0 ORDER BY Name"
        End If

        SrcUser.SelectParameters.Clear()
        SrcUser.SelectParameters.Add("LocationCode", st_LoginLocationCode)

    End Sub

  
    Private Sub SetControl_Priority(ByVal ParPONumber As String)
        '親POのPriorityを取得する
        Dim st_ParPriority As String = GetParPOPriority(ParPONumber)

        SetPriorityDropDownList(Priority, PRIORITY_FOR_EDIT)
        Priority.SelectedValue = st_ParPriority
        LabelPriority.Text = st_ParPriority
    End Sub

    Private Function ValidateField() As Boolean

        ' R/3 PO Line Number
        R3PONumber.Text = R3PONumber.Text.Trim
        R3POLineNumber.Text = R3POLineNumber.Text.Trim
        If R3PONumber.Text <> String.Empty AndAlso R3POLineNumber.Text = String.Empty Then
            Msg.Text = "R/3 PO Line Number" & ERR_REQUIRED_FIELD
            Return False
        End If

        ' PO Date (必須)
        PODate.Text = PODate.Text.Trim
        If PODate.Text = String.Empty Then
            Msg.Text = "PO Date" & ERR_REQUIRED_FIELD
            Return False
        Else
            If Not Regex.IsMatch(PODate.Text, DATE_REGEX) Then
                Msg.Text = "PO Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(PODate.Text) Then
                Msg.Text = "PO Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        ' PO-User (必須)
        If POUser.SelectedValue = String.Empty Then
            Msg.Text = "PO-User" & ERR_REQUIRED_FIELD
            Return False
        End If

        ' Order Quantity (必須)
        OrderQuantity.Text = OrderQuantity.Text.Trim
        If OrderQuantity.Text = String.Empty Or OrderUnit.SelectedValue = String.Empty Then
            Msg.Text = "Order Quantity" & ERR_REQUIRED_FIELD
            Return False
        Else
            If Not Regex.IsMatch(OrderQuantity.Text, DECIMAL_7_3_REGEX) Then
                Msg.Text = "Order Quantity" & ERR_INVALID_NUMBER
                Return False
            End If
        End If

        ' Delivery Date
        DeliveryDate.Text = DeliveryDate.Text.Trim
        If DeliveryDate.Text <> String.Empty Then
            If Not Regex.IsMatch(DeliveryDate.Text, DATE_REGEX) Then
                Msg.Text = "Delivery Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(DeliveryDate.Text) Then
                Msg.Text = "Delivery Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        ' Due Date
        DueDate.Text = DueDate.Text.Trim
        If DueDate.Text <> String.Empty Then
            If Not Regex.IsMatch(DueDate.Text, DATE_REGEX) Then
                Msg.Text = "Due Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(DueDate.Text) Then
                Msg.Text = "Due Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        Return True

    End Function

    Private Function InsertPO() As Integer
        Dim st_SOLocationCode As String = String.Empty
        Dim obj_PONumber As Object = DBNull.Value
        Dim st_Priority As String = Priority.SelectedValue

        Dim sqlConn As New SqlConnection(DB_CONNECT_STRING)
        Dim sqlReader As SqlDataReader
        Dim sqlCmd As SqlCommand
        Dim ds As New DataSet
        Dim sqlAdapter As New SqlDataAdapter

        ' 現法に発注する場合は SOLocationCode を設定します
        ' (Supplier.LocationCode が設定されていたら、その仕入先は現法と判断)
        sqlCmd = New SqlCommand(CreateSql_SelectSupplier(), sqlConn)
        sqlAdapter.SelectCommand = sqlCmd
        sqlCmd.Parameters.Add("@SupplierCode", SqlDbType.VarChar).Value = Supplier.SelectedValue
        sqlAdapter.Fill(ds, "Supplier")
        st_SOLocationCode = ds.Tables("Supplier").Rows(0)("LocationCode").ToString

        sqlCmd = New SqlCommand(CreateSql_InsertPO(), sqlConn)
        sqlCmd.Parameters.AddWithValue("@R3PONumber", ConvertEmptyStringToNull(R3PONumber.Text))
        sqlCmd.Parameters.AddWithValue("@R3POLineNumber", ConvertEmptyStringToNull(R3POLineNumber.Text))
        sqlCmd.Parameters.AddWithValue("@PODate", GetDatabaseTime(st_LoginLocationCode, PODate.Text))
        sqlCmd.Parameters.AddWithValue("@POLocationCode", ConvertEmptyStringToNull(POLocationCode.Value))
        sqlCmd.Parameters.AddWithValue("@POUserID", ConvertStringToInt(POUser.SelectedValue))
        sqlCmd.Parameters.AddWithValue("@SOLocationCode", ConvertEmptyStringToNull(st_SOLocationCode))
        sqlCmd.Parameters.AddWithValue("@ProductID", ConvertStringToInt(ConvertEmptyStringToNull(ProductID.Value).ToString))
        sqlCmd.Parameters.AddWithValue("@SupplierCode", ConvertEmptyStringToNull(Supplier.SelectedValue))
        sqlCmd.Parameters.AddWithValue("@MakerCode", ConvertEmptyStringToNull(MakerCode.Value))
        sqlCmd.Parameters.AddWithValue("@OrderQuantity", ConvertStringToDec(OrderQuantity.Text))
        sqlCmd.Parameters.AddWithValue("@OrderUnitCode", ConvertEmptyStringToNull(OrderUnit.SelectedValue))
        sqlCmd.Parameters.AddWithValue("@DeliveryDate", GetDatabaseTime(st_LoginLocationCode, DeliveryDate.Text))
        sqlCmd.Parameters.AddWithValue("@CurrencyCode", ConvertEmptyStringToNull(CurrencyCode.Text))
        sqlCmd.Parameters.AddWithValue("@UnitPrice", ConvertStringToDec(UnitPrice.Text))
        sqlCmd.Parameters.AddWithValue("@PerQuantity", ConvertStringToDec(PerQuantity.Text))
        sqlCmd.Parameters.AddWithValue("@PerUnitCode", ConvertEmptyStringToNull(PerUnit.Text))
        sqlCmd.Parameters.AddWithValue("@PaymentTermCode", ConvertEmptyStringToNull(PaymentTermCode.Value))
        sqlCmd.Parameters.AddWithValue("@IncotermsCode", ConvertEmptyStringToNull(IncotermsCode.Value))
        sqlCmd.Parameters.AddWithValue("@DeliveryTerm", ConvertEmptyStringToNull(DeliveryTerm.Text))
        If String.IsNullOrEmpty(st_ParPONumber) Then
            sqlCmd.Parameters.AddWithValue("@PurposeCode", ConvertEmptyStringToNull(Purpose.SelectedValue))
        Else
            sqlCmd.Parameters.AddWithValue("@PurposeCode", ConvertEmptyStringToNull(ParPO_PurposeCode.Value))
        End If
        sqlCmd.Parameters.AddWithValue("@RawMaterialFor", ConvertEmptyStringToNull(RawMaterialFor.Text))
        sqlCmd.Parameters.AddWithValue("@RequestedBy", ConvertEmptyStringToNull(RequestedBy.Text))
        sqlCmd.Parameters.AddWithValue("@SupplierItemNumber", ConvertEmptyStringToNull(SupplierItemNumber.Text))
        sqlCmd.Parameters.AddWithValue("@SupplierLotNumber", ConvertEmptyStringToNull(SupplierLotNumber.Text))
        sqlCmd.Parameters.AddWithValue("@DueDate", GetDatabaseTime(st_LoginLocationCode, DueDate.Text))
        sqlCmd.Parameters.AddWithValue("@RFQLineNumber", ConvertStringToInt(RFQLineNumber.Value))
        sqlCmd.Parameters.AddWithValue("@ParPONumber", ConvertStringToInt(ParPONumber.Value))
        If Priority.Visible = True Then
            sqlCmd.Parameters.AddWithValue("@Priority", ConvertEmptyStringToNull(st_Priority))
        End If
        sqlCmd.Parameters.AddWithValue("@CreatedBy", CInt(Session("UserID")))
        sqlCmd.Parameters.AddWithValue("@UpdatedBy", CInt(Session("UserID")))

        sqlConn.Open()

        sqlReader = sqlCmd.ExecuteReader
        While sqlReader.Read
            obj_PONumber = sqlReader("PONumber")
        End While
        sqlReader.Close()
        sqlConn.Close()

        If IsDBNull(obj_PONumber) Then
            Throw New Exception(EXP_PO_ISSUE_ERROR)
        End If

        Return CInt(obj_PONumber)

    End Function

    
    Private Function CreateSql_SelectRFQLine() As String
        Dim sb_Sql As StringBuilder = New StringBuilder

        sb_Sql.Append("SELECT ")
        sb_Sql.Append("  RFQNumber, ")
        sb_Sql.Append("  QuoLocationCode,  ")
        sb_Sql.Append("  ProductID, ")
        sb_Sql.Append("  ProductNumber, ")
        sb_Sql.Append("  ProductName, ")
        sb_Sql.Append("  EnqQuantity, ")
        sb_Sql.Append("  EnqUnitCode, ")
        sb_Sql.Append("  EnqPiece, ")
        sb_Sql.Append("  CurrencyCode, ")
        sb_Sql.Append("  UnitPrice, ")
        sb_Sql.Append("  QuoPer, ")
        sb_Sql.Append("  QuoUnitCode, ")
        sb_Sql.Append("  SupplierCode, ")
        sb_Sql.Append("  R3SupplierCode, ")
        sb_Sql.Append("  R3SupplierName, ")
        sb_Sql.Append("  MakerCode, ")
        sb_Sql.Append("  R3MakerCode, ")
        sb_Sql.Append("  R3MakerName, ")
        sb_Sql.Append("  PaymentTermCode, ")
        sb_Sql.Append("  PaymentTerm, ")
        sb_Sql.Append("  IncotermsCode, ")
        sb_Sql.Append("  Incoterms, ")
        sb_Sql.Append("  DeliveryTerm, ")
        sb_Sql.Append("  PurposeCode, ")
        sb_Sql.Append("  SupplierItemNumber, ")
        sb_Sql.Append("  isCONFIDENTIAL ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append("  v_RFQLine ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append("  RFQLineNumber = @RFQLineNumber")

        Return sb_Sql.ToString

    End Function

    Private Function CreateSql_SelectSupplier() As String

        Return "SELECT LocationCode FROM Supplier WHERE SupplierCode = @SupplierCode"

    End Function

    Private Function CreateSql_InsertPO() As String
        Dim sb_Sql As StringBuilder = New StringBuilder

        sb_Sql.Append("INSERT INTO PO ( ")
        sb_Sql.Append("  R3PONumber, ")
        sb_Sql.Append("  R3POLineNumber, ")
        sb_Sql.Append("  PODate, ")
        sb_Sql.Append("  POLocationCode, ")
        sb_Sql.Append("  POUserID, ")
        sb_Sql.Append("  SOLocationCode, ")
        sb_Sql.Append("  ProductID, ")
        sb_Sql.Append("  SupplierCode, ")
        sb_Sql.Append("  MakerCode, ")
        sb_Sql.Append("  OrderQuantity, ")
        sb_Sql.Append("  OrderUnitCode, ")
        sb_Sql.Append("  DeliveryDate, ")
        sb_Sql.Append("  CurrencyCode, ")
        sb_Sql.Append("  UnitPrice, ")
        sb_Sql.Append("  PerQuantity, ")
        sb_Sql.Append("  PerUnitCode, ")
        sb_Sql.Append("  PaymentTermCode, ")
        sb_Sql.Append("  IncotermsCode, ")
        sb_Sql.Append("  DeliveryTerm, ")
        sb_Sql.Append("  PurposeCode, ")
        sb_Sql.Append("  RawMaterialFor, ")
        sb_Sql.Append("  RequestedBy, ")
        sb_Sql.Append("  SupplierItemNumber, ")
        sb_Sql.Append("  SupplierLotNumber, ")
        sb_Sql.Append("  DueDate, ")
        sb_Sql.Append("  RFQLineNumber, ")
        sb_Sql.Append("  ParPONumber, ")
        If Priority.Visible = True Then
            sb_Sql.Append("  Priority, ")
        End If
        sb_Sql.Append("  CreatedBy, ")
        sb_Sql.Append("  UpdatedBy ")
        sb_Sql.Append(") VALUES ( ")
        sb_Sql.Append("  @R3PONumber, ")
        sb_Sql.Append("  @R3POLineNumber, ")
        sb_Sql.Append("  @PODate, ")
        sb_Sql.Append("  @POLocationCode, ")
        sb_Sql.Append("  @POUserID, ")
        sb_Sql.Append("  @SOLocationCode, ")
        sb_Sql.Append("  @ProductID, ")
        sb_Sql.Append("  @SupplierCode, ")
        sb_Sql.Append("  @MakerCode, ")
        sb_Sql.Append("  @OrderQuantity, ")
        sb_Sql.Append("  @OrderUnitCode, ")
        sb_Sql.Append("  @DeliveryDate, ")
        sb_Sql.Append("  @CurrencyCode, ")
        sb_Sql.Append("  @UnitPrice, ")
        sb_Sql.Append("  @PerQuantity, ")
        sb_Sql.Append("  @PerUnitCode, ")
        sb_Sql.Append("  @PaymentTermCode, ")
        sb_Sql.Append("  @IncotermsCode, ")
        sb_Sql.Append("  @DeliveryTerm, ")
        sb_Sql.Append("  @PurposeCode, ")
        sb_Sql.Append("  @RawMaterialFor, ")
        sb_Sql.Append("  @RequestedBy, ")
        sb_Sql.Append("  @SupplierItemNumber, ")
        sb_Sql.Append("  @SupplierLotNumber, ")
        sb_Sql.Append("  @DueDate, ")
        sb_Sql.Append("  @RFQLineNumber, ")
        sb_Sql.Append("  @ParPONumber, ")
        If Priority.Visible = True Then
            sb_Sql.Append("  @Priority, ")
        End If
        sb_Sql.Append("  @CreatedBy, ")
        sb_Sql.Append("  @UpdatedBy ")
        sb_Sql.Append("); ")
        sb_Sql.Append("SELECT PONumber FROM PO WHERE PONumber = SCOPE_IDENTITY()")

        Return sb_Sql.ToString

    End Function

    Private Function GetSQL_SelectSOUserID() As String
        Dim sb_Sql As StringBuilder = New StringBuilder

        sb_Sql.Append("SELECT ")
        sb_Sql.Append("	SOUserID ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append("	PO ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append("	PONumber = @PONumber")

        Return sb_Sql.ToString()

    End Function

    Private Function GetSQL_SelectEnqUnitCode() As String
        Dim sb_Sql As StringBuilder = New StringBuilder

        sb_Sql.Append("SELECT ")
        sb_Sql.Append("	EnqUnitCode ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append("	RFQLine ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append("	RFQLineNumber = @RFQLineNumber")

        Return sb_Sql.ToString()

    End Function

    ''' <summary>
    ''' 親POのPurposeCode,PurposeTextを取得します。
    ''' </summary>
    ''' <param name="ParPONumber">親のPONumber</param>
    ''' <remarks></remarks>
    Private Sub GetParPO_Purpose(ByVal ParPONumber As String, ByRef PurposeCode As String, ByRef PurposeText As String)

        If ParPONumber Is Nothing Then
            Exit Sub
        End If

        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = New SqlCommand("SELECT PurposeCode,PurposeText FROM V_PO WHERE PONumber = @ParPONumber", conn)
            cmd.Parameters.AddWithValue("ParPONumber", ParPONumber)

            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.Read() Then
                If dr("PurposeCode").ToString <> String.Empty Then
                    PurposeCode = dr("PurposeCode").ToString
                    PurposeText = dr("PurposeText").ToString
                End If
            End If


        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try

    End Sub

End Class

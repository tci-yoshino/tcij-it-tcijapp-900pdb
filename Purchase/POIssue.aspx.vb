Option Strict On

Imports System.Data.SqlClient
Imports Purchase.Common

Partial Public Class POIssue
    Inherits CommonPage

    Protected st_RFQLineNumber As String
    Protected st_ParPONumber As String
    Protected st_Action As String
    Protected st_LoginLocationCode As String

    Private Const ERR_RFQ_NOT_FOUND As String = "該当する見積依頼は存在しません。"
    Private Const ERR_NO_QUOTATION_REPLY As String = "見積依頼に対する回答がないため発注できません。"
    Private Const ERR_R3_SUPPLIER_DOES_NOT_EXIST As String = "仕入先が R/3 に登録されていないため発注できません。"
    Private Const ERR_CHI_PO_ALREADY_EXISTS As String = "子発注データが既に存在します。"
    Private Const EXP_PO_ISSUE_ERROR As String = "POIssue.Issue_Click: 発注番号が採番されませんでした。"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        If String.IsNullOrEmpty(st_RFQLineNumber) Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        If Not IsPostBack Then
            If Not String.IsNullOrEmpty(st_ParPONumber) Then
                ' 同じ親を持つ子 PO が存在する場合はエラーとします
                If ExistenceConfirmation("PO", "ParPONumber", st_ParPONumber) Then
                    Msg.Text = ERR_CHI_PO_ALREADY_EXISTS
                    Exit Sub
                End If
            End If

            If SetControl() = False Then
                ' 登録フォームを表示させないための措置です
                st_RFQLineNumber = String.Empty
                Exit Sub
            End If
        End If

    End Sub

    Protected Sub Issue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Issue.Click
        Dim i_PONumber As Integer = 0

        If st_Action <> "Issue" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
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
        Dim st_Via As String = ""

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

        If IsDBNull(ds.Tables("RFQLine").Rows(0)("R3SupplierCode")) Then
            Msg.Text = ERR_R3_SUPPLIER_DOES_NOT_EXIST
            Return False
        End If

        If IsDBNull(ds.Tables("RFQLine").Rows(0)("UnitPrice")) Then
            Msg.Text = ERR_NO_QUOTATION_REPLY
            Return False
        End If

        RFQNumber.Text = ds.Tables("RFQLine").Rows(0)("RFQNumber").ToString
        ParPONumber.Text = st_ParPONumber
        PODate.Text = GetLocalTime(Session("LocationCode").ToString, Now)
        If Not CBool(Session("Purchase.isAdmin")) Then
            POUser.SelectedValue = Session("UserID").ToString
        End If
        POLocationName.Text = Session("LocationName").ToString
        ProductNumber.Text = ds.Tables("RFQLine").Rows(0)("ProductNumber").ToString
        ProductName.Text = CutShort(ds.Tables("RFQLine").Rows(0)("ProductName").ToString)
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
        Incoterms.Text = ds.Tables("RFQLine").Rows(0)("IncotermsCode").ToString
        DeliveryTerm.Text = ds.Tables("RFQLine").Rows(0)("DeliveryTerm").ToString
        SupplierItemNumber.Text = ds.Tables("RFQLine").Rows(0)("SupplierItemNumber").ToString

        ' HiddenField
        RFQLineNumber.Value = st_RFQLineNumber
        POLocationCode.Value = Session("LocationCode").ToString
        ProductID.Value = ds.Tables("RFQLine").Rows(0)("ProductID").ToString
        MakerCode.Value = ds.Tables("RFQLine").Rows(0)("MakerCode").ToString
        PaymentTermCode.Value = ds.Tables("RFQLine").Rows(0)("PaymentTermCode").ToString
        IncotermsCode.Value = ds.Tables("RFQLine").Rows(0)("IncotermsCode").ToString

        ' SqlDataSource
        SetControl_SrcUser(st_LoginLocationCode)
        SetControl_SrcUnit()
        SetControl_SrcSupplier(ds.Tables("RFQLine").Rows(0)("SupplierCode").ToString, ds.Tables("RFQLine").Rows(0)("QuoLocationCode").ToString)
        SetControl_SrcPurpose()

        Return True

    End Function

    Private Sub SetControl_SrcUser(ByVal LocationCode As String)

        SrcUser.SelectCommand = "SELECT UserID, Name FROM v_User WHERE LocationCode = @LocationCode ORDER BY Name"
        SrcUser.SelectParameters.Clear()
        SrcUser.SelectParameters.Add("LocationCode", LocationCode)

    End Sub

    Private Sub SetControl_SrcUnit()

        SrcUnit.SelectCommand = "SELECT UnitCode FROM PurchasingUnit ORDER BY UnitCode"

    End Sub

    Private Sub SetControl_SrcSupplier(ByVal SupplierCode As String, ByVal LocationCode As String)
        Dim sb_Sql As StringBuilder = New StringBuilder
        Dim st_Via As String = ""

        sb_Sql.Append("SELECT ")
        sb_Sql.Append("  SupplierCode, ")
        sb_Sql.Append("  LTRIM(RTRIM(ISNULL(Name1, '') + ' ' + ISNULL(Name2, ''))) AS Name ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append("  Supplier ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append("  SupplierCode = @SupplierCode ")
        sb_Sql.Append("UNION ")
        sb_Sql.Append("SELECT ")
        sb_Sql.Append("  SupplierCode, ")
        sb_Sql.Append("  LTRIM(RTRIM(ISNULL(Name1, '') + ' ' + ISNULL(Name2, ''))) AS Name ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append("  Supplier ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append("  LocationCode = @LocationCode")

        SrcSupplier.SelectCommand = sb_Sql.ToString
        SrcSupplier.SelectParameters.Clear()
        SrcSupplier.SelectParameters.Add("SupplierCode", SupplierCode)

        If (LocationCode = st_LoginLocationCode) Or (LocationCode = String.Empty) Then
            ' Direct 発注の場合に自拠点をリストアップしないための措置です
            SrcSupplier.SelectParameters.Add("LocationCode", "#%@$\")
        Else
            SrcSupplier.SelectParameters.Add("LocationCode", LocationCode)
        End If


    End Sub

    Private Sub SetControl_SrcPurpose()

        SrcPurpose.SelectCommand = "SELECT PurposeCode, Text FROM Purpose ORDER BY SortOrder"

    End Sub

    Private Function ValidateField() As Boolean

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

        ' Goods Arrived Date
        GoodsArrivedDate.Text = GoodsArrivedDate.Text.Trim
        If GoodsArrivedDate.Text <> String.Empty Then
            If Not Regex.IsMatch(GoodsArrivedDate.Text, DATE_REGEX) Then
                Msg.Text = "Goods Arrived Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(GoodsArrivedDate.Text) Then
                Msg.Text = "Goods Arrived Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        ' Commercial Invoice Received Date
        InvoiceReceivedDate.Text = InvoiceReceivedDate.Text.Trim
        If InvoiceReceivedDate.Text <> String.Empty Then
            If Not Regex.IsMatch(InvoiceReceivedDate.Text, DATE_REGEX) Then
                Msg.Text = "Commercial Invoice Received Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(InvoiceReceivedDate.Text) Then
                Msg.Text = "Commercial Invoice Received Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        ' Import Custom Clearance Date
        ImportCustomClearanceDate.Text = ImportCustomClearanceDate.Text.Trim
        If ImportCustomClearanceDate.Text <> String.Empty Then
            If Not Regex.IsMatch(ImportCustomClearanceDate.Text, DATE_REGEX) Then
                Msg.Text = "Import Custom Clearance Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(ImportCustomClearanceDate.Text) Then
                Msg.Text = "Import Custom Clearance Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        ' QM Starting Date
        QMStartingDate.Text = QMStartingDate.Text.Trim
        If QMStartingDate.Text <> String.Empty Then
            If Not Regex.IsMatch(QMStartingDate.Text, DATE_REGEX) Then
                Msg.Text = "QM Starting Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(QMStartingDate.Text) Then
                Msg.Text = "QM Starting Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        ' QM Finish Date
        QMFinishDate.Text = QMFinishDate.Text.Trim
        If QMFinishDate.Text <> String.Empty Then
            If Not Regex.IsMatch(QMFinishDate.Text, DATE_REGEX) Then
                Msg.Text = "QM Finish Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(QMFinishDate.Text) Then
                Msg.Text = "QM Finish Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        ' Scheduled Export Date
        ScheduledExportDate.Text = ScheduledExportDate.Text.Trim
        If ScheduledExportDate.Text <> String.Empty Then
            If Not Regex.IsMatch(ScheduledExportDate.Text, DATE_REGEX) Then
                Msg.Text = "Scheduled Export Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(ScheduledExportDate.Text) Then
                Msg.Text = "Scheduled Export Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        Return True

    End Function

    Private Function InsertPO() As Integer
        Dim st_SOLocationCode As String = ""
        Dim obj_PONumber As Object = DBNull.Value

        Dim sqlConn As New SqlConnection(DB_CONNECT_STRING)
        Dim sqlReader As SqlDataReader
        Dim sqlCmd As SqlCommand
        Dim ds As New DataSet
        Dim sqlAdapter As New SqlDataAdapter

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
        sqlCmd.Parameters.AddWithValue("@PurposeCode", ConvertEmptyStringToNull(Purpose.SelectedValue))
        sqlCmd.Parameters.AddWithValue("@RawMaterialFor", ConvertEmptyStringToNull(RawMaterialFor.Text))
        sqlCmd.Parameters.AddWithValue("@RequestedBy", ConvertEmptyStringToNull(RequestedBy.Text))
        sqlCmd.Parameters.AddWithValue("@SupplierItemNumber", ConvertEmptyStringToNull(SupplierItemNumber.Text))
        sqlCmd.Parameters.AddWithValue("@SupplierLotNumber", ConvertEmptyStringToNull(SupplierLotNumber.Text))
        sqlCmd.Parameters.AddWithValue("@DueDate", GetDatabaseTime(st_LoginLocationCode, DueDate.Text))
        sqlCmd.Parameters.AddWithValue("@GoodsArrivedDate", GetDatabaseTime(st_LoginLocationCode, GoodsArrivedDate.Text))
        sqlCmd.Parameters.AddWithValue("@LotNumber", ConvertEmptyStringToNull(LotNumber.Text))
        sqlCmd.Parameters.AddWithValue("@InvoiceReceivedDate", GetDatabaseTime(st_LoginLocationCode, InvoiceReceivedDate.Text))
        sqlCmd.Parameters.AddWithValue("@ImportCustomClearanceDate", GetDatabaseTime(st_LoginLocationCode, ImportCustomClearanceDate.Text))
        sqlCmd.Parameters.AddWithValue("@QMStartingDate", GetDatabaseTime(st_LoginLocationCode, QMStartingDate.Text))
        sqlCmd.Parameters.AddWithValue("@QMFinishDate", GetDatabaseTime(st_LoginLocationCode, QMFinishDate.Text))
        sqlCmd.Parameters.AddWithValue("@QMResult", ConvertEmptyStringToNull(QMResult.Text))
        sqlCmd.Parameters.AddWithValue("@RequestQuantity", ConvertEmptyStringToNull(RequestQuantity.Text))
        sqlCmd.Parameters.AddWithValue("@ScheduledExportDate", GetDatabaseTime(st_LoginLocationCode, ScheduledExportDate.Text))
        sqlCmd.Parameters.AddWithValue("@PurchasingRequisitionNumber", ConvertEmptyStringToNull(PurchasingRequisitionNumber.Text))
        sqlCmd.Parameters.AddWithValue("@RFQLineNumber", ConvertStringToInt(RFQLineNumber.Value))
        sqlCmd.Parameters.AddWithValue("@ParPONumber", ConvertStringToInt(ParPONumber.Text))
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
        sb_Sql.Append("  DeliveryTerm, ")
        sb_Sql.Append("  PurposeCode, ")
        sb_Sql.Append("  SupplierItemNumber ")
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
        sb_Sql.Append("  GoodsArrivedDate, ")
        sb_Sql.Append("  LotNumber, ")
        sb_Sql.Append("  InvoiceReceivedDate, ")
        sb_Sql.Append("  ImportCustomClearanceDate, ")
        sb_Sql.Append("  QMStartingDate, ")
        sb_Sql.Append("  QMFinishDate, ")
        sb_Sql.Append("  QMResult, ")
        sb_Sql.Append("  RequestQuantity, ")
        sb_Sql.Append("  ScheduledExportDate, ")
        sb_Sql.Append("  PurchasingRequisitionNumber, ")
        sb_Sql.Append("  RFQLineNumber, ")
        sb_Sql.Append("  ParPONumber, ")
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
        sb_Sql.Append("  @GoodsArrivedDate, ")
        sb_Sql.Append("  @LotNumber, ")
        sb_Sql.Append("  @InvoiceReceivedDate, ")
        sb_Sql.Append("  @ImportCustomClearanceDate, ")
        sb_Sql.Append("  @QMStartingDate, ")
        sb_Sql.Append("  @QMFinishDate, ")
        sb_Sql.Append("  @QMResult, ")
        sb_Sql.Append("  @RequestQuantity, ")
        sb_Sql.Append("  @ScheduledExportDate, ")
        sb_Sql.Append("  @PurchasingRequisitionNumber, ")
        sb_Sql.Append("  @RFQLineNumber, ")
        sb_Sql.Append("  @ParPONumber, ")
        sb_Sql.Append("  @CreatedBy, ")
        sb_Sql.Append("  @UpdatedBy ")
        sb_Sql.Append("); ")
        sb_Sql.Append("SELECT PONumber FROM PO WHERE PONumber = SCOPE_IDENTITY()")

        Return sb_Sql.ToString

    End Function

End Class

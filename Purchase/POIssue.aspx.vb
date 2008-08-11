Imports System.Data.SqlClient
Imports Purchase.Common

Partial Public Class POIssue
    Inherits CommonPage

    ' HTTP Query String
    Public st_RFQLineNumber As String
    Public st_ParPONumber As String
    Public st_Action As String

    ' Database Connection Information
    Public setting As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    Public sqlConn As SqlConnection

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim sqlAdapter As SqlDataAdapter
        Dim sqlCmd As SqlCommand
        Dim ds As DataSet = New DataSet
        Dim do_OrderQuantity As Double = 0
        Dim do_PerQuantity As Double = 0
        Dim do_UnitPrice As Double = 0
        Dim st_LocationCode As String = ""

        st_RFQLineNumber = IIf(Request.RequestType = "POST", Request.Form("RFQLineNumber"), Request.QueryString("RFQLineNumber"))
        st_ParPONumber = IIf(Request.RequestType = "POST", Request.Form("ParPONumber"), Request.QueryString("ParPONumber"))
        st_Action = IIf(Request.RequestType = "POST", Request.Form("Action"), Request.QueryString("Action"))

        If String.IsNullOrEmpty(st_RFQLineNumber) Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        sqlConn = New SqlConnection(setting.ConnectionString)

        If Not IsPostBack Then
            sqlAdapter = New SqlDataAdapter
            sqlCmd = New SqlCommand( _
"SELECT " & _
"  RFQNumber, " & _
"  QuoLocationCode,  " & _
"  ProductID, " & _
"  ProductNumber, " & _
"  ProductName, " & _
"  EnqQuantity, " & _
"  EnqUnitCode, " & _
"  EnqPiece, " & _
"  CurrencyCode, " & _
"  UnitPrice, " & _
"  QuoPer, " & _
"  QuoUnitCode, " & _
"  SupplierCode, " & _
"  R3SupplierCode, " & _
"  R3SupplierName, " & _
"  MakerCode, " & _
"  R3MakerCode, " & _
"  R3MakerName, " & _
"  PaymentTermCode, " & _
"  PaymentTerm, " & _
"  IncotermsCode, " & _
"  Incoterms, " & _
"  DeliveryTerm, " & _
"  PurposeCode, " & _
"  SupplierItemNumber " & _
"FROM " & _
"  v_RFQLine " & _
"WHERE " & _
"  RFQLineNumber = @RFQLineNumber", sqlConn)

            sqlAdapter.SelectCommand = sqlCmd
            sqlCmd.Parameters.Add("@RFQLineNumber", SqlDbType.Int).Value = st_RFQLineNumber
            sqlAdapter.Fill(ds, "RFQLine")

            If ds.Tables("RFQLine").Rows.Count = 0 Then
                Msg.Text = "RFQ record does not exist."
                Exit Sub
            End If

            If IsDBNull(ds.Tables("RFQLine").Rows(0)("UnitPrice")) Then
                Msg.Text = "Quotation reply does not exist."
                Exit Sub
            End If

            RFQNumber.Text = ds.Tables("RFQLine").Rows(0)("RFQNumber").ToString
            ParPONumber.Text = st_ParPONumber
            PODate.Text = GetLocalTime(Session("LocationCode"), Now)
            POUser.SelectedValue = Session("UserID")
            POLocation.Text = Session("LocationName")
            ProductNumber.Text = ds.Tables("RFQLine").Rows(0)("ProductNumber").ToString
            ProductName.Text = CutShort(ds.Tables("RFQLine").Rows(0)("ProductName").ToString)
            do_OrderQuantity = ds.Tables("RFQLine").Rows(0)("EnqQuantity") * ds.Tables("RFQLine").Rows(0)("EnqPiece")
            OrderQuantity.Text = do_OrderQuantity.ToString("G29")
            OrderUnit.SelectedValue = ds.Tables("RFQLine").Rows(0)("EnqUnitCode").ToString
            Currency.Text = ds.Tables("RFQLine").Rows(0)("CurrencyCode").ToString
            do_UnitPrice = ds.Tables("RFQLine").Rows(0)("UnitPrice")
            UnitPrice.Text = do_UnitPrice.ToString("G29")
            do_PerQuantity = ds.Tables("RFQLine").Rows(0)("QuoPer")
            PerQuantity.Text = do_PerQuantity.ToString("G29")
            PerUnit.Text = ds.Tables("RFQLine").Rows(0)("QuoUnitCode").ToString
            R3MakerCode.Text = ds.Tables("RFQLine").Rows(0)("R3MakerCode").ToString
            R3MakerName.Text = ds.Tables("RFQLine").Rows(0)("R3MakerName").ToString
            PaymentTerm.Text = ds.Tables("RFQLine").Rows(0)("PaymentTerm").ToString
            Incoterms.Text = ds.Tables("RFQLine").Rows(0)("Incoterms").ToString
            DeliveryTerm.Text = ds.Tables("RFQLine").Rows(0)("DeliveryTerm").ToString
            SupplierItemNumber.Text = ds.Tables("RFQLine").Rows(0)("SupplierItemNumber").ToString

            RFQLineNumber.Value = st_RFQLineNumber
            POLocationCode.Value = Session("LocationCode")
            ProductID.Value = ds.Tables("RFQLine").Rows(0)("ProductID").ToString
            MakerCode.Value = ds.Tables("RFQLine").Rows(0)("MakerCode").ToString
            PaymentTermCode.Value = ds.Tables("RFQLine").Rows(0)("PaymentTermCode").ToString
            IncotermsCode.Value = ds.Tables("RFQLine").Rows(0)("IncotermsCode").ToString

            SrcSupplier.SelectParameters.Clear()
            SrcSupplier.SelectParameters.Add("SupplierCode", ds.Tables("RFQLine").Rows(0)("SupplierCode").ToString)
            st_LocationCode = ds.Tables("RFQLine").Rows(0)("QuoLocationCode").ToString
            If (st_LocationCode = Session("LocationCode")) Or (st_LocationCode = "") Then
                st_LocationCode = "#%@$\"
            End If
            SrcSupplier.SelectParameters.Add("LocationCode", st_LocationCode)
        End If

    End Sub

    Protected Sub Issue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Issue.Click
        Dim sqlAdapter As SqlDataAdapter
        Dim ds As DataSet = New DataSet
        Dim sqlCmd As SqlCommand
        Dim sqlReader As SqlDataReader
        Dim st_SOLocationCode As String = ""
        Dim st_PONumber As String = ""

        If String.IsNullOrEmpty(st_Action) Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        If ValidateField() = False Then
            Exit Sub
        End If

        sqlAdapter = New SqlDataAdapter
        sqlCmd = New SqlCommand( _
"SELECT LocationCode FROM Supplier WHERE SupplierCode = @SupplierCode", sqlConn)

        sqlAdapter.SelectCommand = sqlCmd
        sqlCmd.Parameters.Add("@SupplierCode", SqlDbType.VarChar).Value = Supplier.SelectedValue
        sqlAdapter.Fill(ds, "Supplier")

        st_SOLocationCode = ds.Tables("Supplier").Rows(0)("LocationCode").ToString

        sqlCmd = New SqlCommand( _
"INSERT INTO PO ( " & _
"  R3PONumber, " & _
"  R3POLineNumber, " & _
"  PODate, " & _
"  POLocationCode, " & _
"  POUserID, " & _
"  SOLocationCode, " & _
"  ProductID, " & _
"  SupplierCode, " & _
"  MakerCode, " & _
"  OrderQuantity, " & _
"  OrderUnitCode, " & _
"  DeliveryDate, " & _
"  CurrencyCode, " & _
"  UnitPrice, " & _
"  PerQuantity, " & _
"  PerUnitCode, " & _
"  PaymentTermCode, " & _
"  IncotermsCode, " & _
"  DeliveryTerm, " & _
"  PurposeCode, " & _
"  RawMaterialFor, " & _
"  RequestedBy, " & _
"  SupplierItemNumber, " & _
"  SupplierLotNumber, " & _
"  DueDate, " & _
"  GoodsArrivedDate, " & _
"  LotNumber, " & _
"  InvoiceReceivedDate, " & _
"  ImportCustomClearanceDate, " & _
"  QMStartingDate, " & _
"  QMFinishDate, " & _
"  QMResult, " & _
"  RequestedQuantity, " & _
"  ScheduledExportDate, " & _
"  PurchasingRequisitionNumber, " & _
"  RFQLineNumber, " & _
"  ParPONumber, " & _
"  CreatedBy, " & _
"  UpdatedBy " & _
") VALUES ( " & _
"  @R3PONumber, " & _
"  @R3POLineNumber, " & _
"  @PODate, " & _
"  @POLocationCode, " & _
"  @POUserID, " & _
"  @SOLocationCode, " & _
"  @ProductID, " & _
"  @SupplierCode, " & _
"  @MakerCode, " & _
"  @OrderQuantity, " & _
"  @OrderUnitCode, " & _
"  @DeliveryDate, " & _
"  @CurrencyCode, " & _
"  @UnitPrice, " & _
"  @PerQuantity, " & _
"  @PerUnitCode, " & _
"  @PaymentTermCode, " & _
"  @IncotermsCode, " & _
"  @DeliveryTerm, " & _
"  @PurposeCode, " & _
"  @RawMaterialFor, " & _
"  @RequestedBy, " & _
"  @SupplierItemNumber, " & _
"  @SupplierLotNumber, " & _
"  @DueDate, " & _
"  @GoodsArrivedDate, " & _
"  @LotNumber, " & _
"  @InvoiceReceivedDate, " & _
"  @ImportCustomClearanceDate, " & _
"  @QMStartingDate, " & _
"  @QMFinishDate, " & _
"  @QMResult, " & _
"  @RequestedQuantity, " & _
"  @ScheduledExportDate, " & _
"  @PurchasingRequisitionNumber, " & _
"  @RFQLineNumber, " & _
"  @ParPONumber, " & _
"  @CreatedBy, " & _
"  @UpdatedBy " & _
"); " & _
"SELECT PONumber FROM PO WHERE PONumber = SCOPE_IDENTITY()", sqlConn)

        sqlCmd.Parameters.Add("@R3PONumber", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(R3PONumber.Text)
        sqlCmd.Parameters.Add("@R3POLineNumber", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(R3POLineNumber.Text)
        sqlCmd.Parameters.Add("@PODate", SqlDbType.DateTime).Value = GetDatabaseTime(Session("LocationCode"), PODate.Text)
        sqlCmd.Parameters.Add("@POLocationCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(POLocationCode.Value)
        sqlCmd.Parameters.Add("@POUserID", SqlDbType.Int).Value = ConvertStringToInt(POUser.SelectedValue)
        sqlCmd.Parameters.Add("@SOLocationCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(st_SOLocationCode)
        sqlCmd.Parameters.Add("@ProductID", SqlDbType.Int).Value = ConvertStringToInt(ConvertEmptyStringToNull(ProductID.Value))
        sqlCmd.Parameters.Add("@SupplierCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(Supplier.SelectedValue)
        sqlCmd.Parameters.Add("@MakerCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(MakerCode.Value)
        sqlCmd.Parameters.Add("@OrderQuantity", SqlDbType.Decimal).Value = ConvertStringToDec(OrderQuantity.Text)
        sqlCmd.Parameters.Add("@OrderUnitCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(OrderUnit.SelectedValue)
        sqlCmd.Parameters.Add("@DeliveryDate", SqlDbType.DateTime).Value = GetDatabaseTime(Session("LocationCode"), DeliveryDate.Text)
        sqlCmd.Parameters.Add("@CurrencyCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(Currency.Text)
        sqlCmd.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = ConvertStringToDec(UnitPrice.Text)
        sqlCmd.Parameters.Add("@PerQuantity", SqlDbType.Decimal).Value = ConvertStringToDec(PerQuantity.Text)
        sqlCmd.Parameters.Add("@PerUnitCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(PerUnit.Text)
        sqlCmd.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(PaymentTermCode.Value)
        sqlCmd.Parameters.Add("@IncotermsCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(IncotermsCode.Value)
        sqlCmd.Parameters.Add("@DeliveryTerm", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(DeliveryTerm.Text)
        sqlCmd.Parameters.Add("@PurposeCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(Purpose.SelectedValue)
        sqlCmd.Parameters.Add("@RawMaterialFor", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(RawMaterialFor.Text)
        sqlCmd.Parameters.Add("@RequestedBy", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(RequestedBy.Text)
        sqlCmd.Parameters.Add("@SupplierItemNumber", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(SupplierItemNumber.Text)
        sqlCmd.Parameters.Add("@SupplierLotNumber", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(SupplierLotNumber.Text)
        sqlCmd.Parameters.Add("@DueDate", SqlDbType.DateTime).Value = GetDatabaseTime(Session("LocationCode"), DueDate.Text)
        sqlCmd.Parameters.Add("@GoodsArrivedDate", SqlDbType.DateTime).Value = GetDatabaseTime(Session("LocationCode"), GoodsArrivedDate.Text)
        sqlCmd.Parameters.Add("@LotNumber", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(LotNumber.Text)
        sqlCmd.Parameters.Add("@InvoiceReceivedDate", SqlDbType.DateTime).Value = GetDatabaseTime(Session("LocationCode"), InvoceReceivedDate.Text)
        sqlCmd.Parameters.Add("@ImportCustomClearanceDate", SqlDbType.DateTime).Value = GetDatabaseTime(Session("LocationCode"), ImportCustomClearanceDate.Text)
        sqlCmd.Parameters.Add("@QMStartingDate", SqlDbType.DateTime).Value = GetDatabaseTime(Session("LocationCode"), QMStartingDate.Text)
        sqlCmd.Parameters.Add("@QMFinishDate", SqlDbType.DateTime).Value = GetDatabaseTime(Session("LocationCode"), QMFinishDate.Text)
        sqlCmd.Parameters.Add("@QMResult", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(QMResult.Text)
        sqlCmd.Parameters.Add("@RequestedQuantity", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(RequestQuantity.Text)
        sqlCmd.Parameters.Add("@ScheduledExportDate", SqlDbType.DateTime).Value = GetDatabaseTime(Session("LocationCode"), ScheduledExportDate.Text)
        sqlCmd.Parameters.Add("@PurchasingRequisitionNumber", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(PurchasingRequisitionNumber.Text)
        sqlCmd.Parameters.Add("@RFQLineNumber", SqlDbType.Int).Value = ConvertStringToInt(RFQLineNumber.Value)
        sqlCmd.Parameters.Add("@ParPONumber", SqlDbType.Int).Value = ConvertStringToInt(ParPONumber.Text)
        sqlCmd.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CInt(Session("UserID"))
        sqlCmd.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = CInt(Session("UserID"))

        sqlConn.Open()
        sqlReader = sqlCmd.ExecuteReader
        While sqlReader.Read
            st_PONumber = CType(sqlReader("PONumber"), String)
        End While
        sqlReader.Close()
        sqlConn.Close()

        If st_PONumber = "" Then
            Throw New Exception("POIssue.Issue_Click: 購買発注データの作成に失敗しましたが、エラーが検出されませんでした。")
        End If

        Response.Redirect(String.Format("POUpdate.aspx?PONumber={0}", st_PONumber))

    End Sub

    Private Function ValidateField() As Boolean

        ' PO Date
        If PODate.Text = "" Then
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

        ' PO-User
        If POUser.Text = "" Then
            Msg.Text = "PO-User" & ERR_REQUIRED_FIELD
            Return False
        End If

        ' Order Quantity
        If OrderQuantity.Text = "" Or OrderUnit.SelectedValue = "" Then
            Msg.Text = "Order Quantity" & ERR_REQUIRED_FIELD
            Return False
        Else
            If Not Regex.IsMatch(OrderQuantity.Text, DECIMAL_7_3_REGEX) Then
                Msg.Text = "Order Quantity" & ERR_INVALID_NUMBER
                Return False
            End If
        End If

        ' Delivery Date (Optional)
        If DeliveryDate.Text <> "" Then
            If Not Regex.IsMatch(DeliveryDate.Text, DATE_REGEX) Then
                Msg.Text = "Delivery Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(DeliveryDate.Text) Then
                Msg.Text = "Delivery Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        ' Due Date (Optional)
        If DueDate.Text <> "" Then
            If Not Regex.IsMatch(DueDate.Text, DATE_REGEX) Then
                Msg.Text = "Due Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(DueDate.Text) Then
                Msg.Text = "Due Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        ' Goods Arrived Date (Optional)
        If GoodsArrivedDate.Text <> "" Then
            If Not Regex.IsMatch(GoodsArrivedDate.Text, DATE_REGEX) Then
                Msg.Text = "Goods Arrived Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(GoodsArrivedDate.Text) Then
                Msg.Text = "Goods Arrived Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        ' Commercial Invoice Received Date (Optional)
        If InvoceReceivedDate.Text <> "" Then
            If Not Regex.IsMatch(InvoceReceivedDate.Text, DATE_REGEX) Then
                Msg.Text = "Commercial Invoice Received Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(InvoceReceivedDate.Text) Then
                Msg.Text = "Commercial Invoice Received Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        ' Import Custom Clearance Date (Optional)
        If ImportCustomClearanceDate.Text <> "" Then
            If Not Regex.IsMatch(ImportCustomClearanceDate.Text, DATE_REGEX) Then
                Msg.Text = "Import Custom Clearance Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(ImportCustomClearanceDate.Text) Then
                Msg.Text = "Import Custom Clearance Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        ' QM Starting Date (Optional)
        If QMStartingDate.Text <> "" Then
            If Not Regex.IsMatch(QMStartingDate.Text, DATE_REGEX) Then
                Msg.Text = "QM Starting Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(QMStartingDate.Text) Then
                Msg.Text = "QM Starting Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        ' QM Finish Date (Optional)
        If QMFinishDate.Text <> "" Then
            If Not Regex.IsMatch(QMFinishDate.Text, DATE_REGEX) Then
                Msg.Text = "QM Finish Date" & ERR_INCORRECT_FORMAT
                Return False
            End If

            If Not IsDate(QMFinishDate.Text) Then
                Msg.Text = "QM Finish Date" & ERR_INVALID_DATE
                Return False
            End If
        End If

        ' Scheduled Export Date (Optional)
        If ScheduledExportDate.Text <> "" Then
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

End Class

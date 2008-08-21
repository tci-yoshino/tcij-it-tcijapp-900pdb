Option Explicit On
Option Infer Off
Option Strict On

Imports Purchase.Common
Imports System.Data.SqlClient

''' <summary>
''' POUpdateフォームクラス
''' </summary>
''' <remarks></remarks>
Partial Public Class POUpdate
    Inherits CommonPage

    Protected st_PONumber As String
    Protected st_Action As String

    ''' <summary>
    ''' POデータを格納する構造体です。
    ''' </summary>
    ''' <remarks></remarks>
    Private Structure POInformationType

        Public PONumber As Nullable(Of Integer)
        Public R3PONumber As String
        Public R3POLineNumber As String
        Public PODate As DateTime?
        Public POLocationCode As String
        Public POLocationName As String   'ReadOnly
        Public POUserID As Integer?
        Public POUserName As String   'ReadOnly
        Public SOLocationCode As String
        Public SOLocationName As String   'ReadOnly
        Public SOUserID As Integer?
        Public SOUserName As String   'ReadOnly
        Public ProductID As Integer?
        Public ProductNumber As String   'ReadOnly
        Public ProductName As String   'ReadOnly
        Public SupplierCode As Integer?
        Public SupplierName As String   'ReadOnly
        Public R3SupplierCode As String   'ReadOnly
        Public R3SupplierName As String   'ReadOnly
        Public MakerCode As Integer?
        Public MakerName As String   'ReadOnly
        Public R3MakerCode As String   'ReadOnly
        Public R3MakerName As String   'ReadOnly
        Public OrderQuantity As Decimal?
        Public OrderUnitCode As String
        Public DeliveryDate As DateTime?
        Public CurrencyCode As String
        Public UnitPrice As Decimal?
        Public PerQuantity As Decimal?
        Public PerUnitCode As String
        Public PaymentTermCode As String
        Public PaymentTermText As String   'ReadOnly
        Public IncotermsCode As String
        Public IncotermsText As String   'ReadOnly
        Public DeliveryTerm As String
        Public PurposeCode As String
        Public PurposeText As String   'ReadOnly
        Public RawMaterialFor As String
        Public RequestedBy As String
        Public SupplierItemNumber As String
        Public SupplierLotNumber As String
        Public DueDate As DateTime?
        Public GoodsArrivedDate As DateTime?
        Public LotNumber As String
        Public InvoiceReceivedDate As DateTime?
        Public ImportCustomClearanceDate As DateTime?
        Public QMStartingDate As DateTime?
        Public QMFinishDate As DateTime?
        Public QMResult As String
        Public RequestQuantity As String
        Public ScheduledExportDate As DateTime?
        Public PurchasingRequisitionNumber As String
        Public isCancelled As Boolean?
        Public CancellationDate As DateTime?
        Public RFQNumber As Integer?
        Public RFQLineNumber As Integer?
        Public ParPONumber As Integer?
        Public StatusCode As String     'ReadOnly
        Public Status As String     'ReadOnly
        Public StatusChangeDate As DateTime?     'ReadOnly
        Public StatusSortOrder As Integer?   'ReadOnly
        Public CreatedBy As Integer?
        Public CreateDate As DateTime?
        Public UpdatedBy As Integer?
        Public UpdateDate As DateTime?

    End Structure


    ''' <summary>
    ''' このページのロードイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        st_Action = CStr(IIf(Request.RequestType = "POST", Request.Form("Action").ToString(), _
                             Request.QueryString("Action").ToString()))

        st_PONumber = CStr(IIf(Request.RequestType = "POST", Request.Form("PONumber").ToString(), _
                             Request.QueryString("PONumber").ToString()))
        st_PONumber = "1000000011"


        If IsPostBack = False Then
            If IsNumeric(st_PONumber) = False Then
                Msg.Text = ERR_INVALID_PARAMETER
                Exit Sub
            End If

            If ExistsPO(st_PONumber) = False Then
                Msg.Text = MSG_NO_DATA_FOUND
                Exit Sub
            End If


            ClearForm()
            ViewPOInformationToForm(CInt(st_PONumber))
        End If

    End Sub

    ''' <summary>
    ''' Updateボタンのクリックイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks></remarks>
    Protected Sub Update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Update.Click

        If st_Action <> "Update" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        If IsNumeric(PO.Value) = False Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        Dim i_PONumber As Integer = CInt(PO.Value)

        Dim POInformation As POInformationType = SelectPOInformation(i_PONumber)
        If CBool(Session("Purchase.isAdmin")) = False And POInformation.POLocationCode <> Session("LocationCode").ToString() Then
            Msg.Text = "拠点が一致しません。"
            Exit Sub
        End If

        If ExistsPO(i_PONumber.ToString()) = False Then
            Msg.Text = "このデータは他のユーザーによって削除されました。"
            Exit Sub
        End If

        'TODO 更新確認値の代入
        If isLatestData("PO", "PONumber", i_PONumber.ToString(), "") = False Then
            Msg.Text = "このデータは他のユーザーによって編集されました。その内容を確認し再度編集をお願いします"
            Exit Sub
        End If

        UpdatePOInfomationFromForm(i_PONumber)

    End Sub


    ''' <summary>
    ''' キャンセルボタンのクリックイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks></remarks>
    Protected Sub Cancell_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancell.Click

        '部分更新仕様


    End Sub




    ''' <summary>
    ''' フォームの表示・入力項目を初期化します。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearForm()
        'フォーム左段
        RFQNumber.Text = String.Empty
        R3PONumber.Text = String.Empty
        PODate.Text = String.Empty
        POUser.Text = String.Empty
        POLocation.Text = String.Empty
        ProductNumber.Text = String.Empty
        ProductName.Text = String.Empty
        OrderQuantity.Text = String.Empty
        OrderUnit.Text = String.Empty
        OrderPiece.Text = String.Empty
        DeliveryDate.Text = String.Empty
        Currency.Text = String.Empty
        UnitPrice.Text = String.Empty
        PerQuantity.Text = String.Empty
        PerUnit.Text = String.Empty
        R3SupplierCode.Text = String.Empty
        R3SupplierName.Text = String.Empty
        R3MakerCode.Text = String.Empty
        R3MakerName.Text = String.Empty
        PaymentTerm.Text = String.Empty
        Incoterms.Text = String.Empty
        DeliveryTerm.Text = String.Empty
        Purpose.Text = String.Empty
        RawMaterialFor.Text = String.Empty
        RequestedBy.Text = String.Empty
        SupplierItemNumber.Text = String.Empty
        SupplierLotNumber.Text = String.Empty
        'フォーム右段
        DueDate.Text = String.Empty
        GoodsArrivedDate.Text = String.Empty
        LotNumber.Text = String.Empty
        InvoceReceivedDate.Text = String.Empty
        ImportCustomClearanceDate.Text = String.Empty
        QMStartingDate.Text = String.Empty
        QMFinishDate.Text = String.Empty
        QMResult.Text = String.Empty
        RequestQuantity.Text = String.Empty
        ScheduledExportDate.Text = String.Empty
        PurchasingRequisitionNumber.Text = String.Empty
        CancellationDate.Text = String.Empty

    End Sub


    ''' <summary>
    ''' 指定されたPOのデータが存在するかを取得します。
    ''' </summary>
    ''' <param name="PONumber">POの一意ID</param>
    ''' <returns>存在するときはTure 存在しないときはFalse</returns>
    ''' <remarks></remarks>
    Private Function ExistsPO(ByVal PONumber As String) As Boolean
        Return ExistenceConfirmation("v_PO", "PONumber", ID)
    End Function

    ''' <summary>
    ''' 指定されたPOのデータの更新日付を取得します。
    ''' </summary>
    ''' <param name="ID">POの一意ID</param>
    ''' <returns>取得した更新日付 データが見つからないときはエラーが発生します。</returns>
    ''' <remarks></remarks>
    Private Function GetUpdateDate(ByVal ID As String) As DateTime
        Throw New ApplicationException("This method is'nt maked.")
    End Function

    ''' <summary>
    ''' 指定されたPOのデータの更新日付を文字列型で取得します。
    ''' </summary>
    ''' <param name="PONumber">POの一意ID</param>
    ''' <returns>取得した更新日付を示す文字列 データが見つからないときはエラーが発生します。</returns>
    ''' <remarks></remarks>
    Private Function GetUpdateDateString(ByVal PONumber As String) As String
        Throw New ApplicationException("This method is'nt maked.")
    End Function

    ''' <summary>
    ''' 指定されたPOデータを画面に表示します。
    ''' </summary>
    ''' <param name="PONumber"></param>
    ''' <remarks></remarks>
    Private Sub ViewPOInformationToForm(ByVal PONumber As Integer)

        PO.Value = PONumber.ToString()

        Dim POInformation As POInformationType = SelectPOInformation(PONumber)

        '画面に取得値を代入

        'フォーム左段
        RFQNumber.Text = POInformation.RFQNumber.ToString()
        R3PONumber.Text = POInformation.R3PONumber
        PODate.Text = NullableDateToString(POInformation.PODate, DATE_FORMAT)
        POUser.Text = POInformation.POUserName
        POLocation.Text = POInformation.POLocationName
        ProductNumber.Text = POInformation.ProductNumber
        ProductName.Text = POInformation.ProductName
        OrderQuantity.Text = POInformation.OrderQuantity.ToString()
        OrderUnit.Text = POInformation.OrderUnitCode
        'TODO 不明フィールド
        'OrderPiece.Text = POInfomation.Order
        DeliveryDate.Text = NullableDateToString(POInformation.DeliveryDate, DATE_FORMAT)
        Currency.Text = POInformation.CurrencyCode
        UnitPrice.Text = POInformation.UnitPrice.ToString()
        PerQuantity.Text = POInformation.PerQuantity.ToString()
        PerUnit.Text = POInformation.PerUnitCode
        R3SupplierCode.Text = POInformation.R3SupplierCode
        R3SupplierName.Text = POInformation.R3SupplierName
        R3MakerCode.Text = POInformation.R3MakerCode
        R3MakerName.Text = POInformation.R3MakerName
        PaymentTerm.Text = POInformation.PaymentTermText
        Incoterms.Text = POInformation.IncotermsText
        DeliveryTerm.Text = POInformation.DeliveryTerm
        Purpose.Text = POInformation.PurposeText
        RawMaterialFor.Text = POInformation.RawMaterialFor
        RequestedBy.Text = POInformation.RequestedBy
        SupplierItemNumber.Text = POInformation.SupplierItemNumber
        SupplierLotNumber.Text = POInformation.SupplierLotNumber
        'フォーム右段
        DueDate.Text = NullableDateToString(POInformation.DueDate, DATE_FORMAT)
        GoodsArrivedDate.Text = NullableDateToString(POInformation.GoodsArrivedDate, DATE_FORMAT)
        LotNumber.Text = POInformation.LotNumber
        InvoceReceivedDate.Text = NullableDateToString(POInformation.InvoiceReceivedDate, DATE_FORMAT)
        ImportCustomClearanceDate.Text = NullableDateToString(POInformation.ImportCustomClearanceDate, DATE_FORMAT)
        QMStartingDate.Text = NullableDateToString(POInformation.QMStartingDate, DATE_FORMAT)
        QMFinishDate.Text = NullableDateToString(POInformation.QMFinishDate, DATE_FORMAT)
        QMResult.Text = POInformation.QMResult
        RequestQuantity.Text = POInformation.RequestQuantity
        ScheduledExportDate.Text = NullableDateToString(POInformation.ScheduledExportDate, DATE_FORMAT)
        PurchasingRequisitionNumber.Text = POInformation.PurchasingRequisitionNumber
        CancellationDate.Text = NullableDateToString(POInformation.CancellationDate, DATE_FORMAT)

    End Sub

    ''' <summary>
    ''' 画面上のPOデータをDBへ保存します。
    ''' </summary>
    ''' <param name="PONumber"></param>
    ''' <remarks></remarks>
    Private Sub UpdatePOInfomationFromForm(ByVal PONumber As Integer)


        Dim POInformation As POInformationType = SelectPOInformation(PONumber)

        'フォーム左段
        POInformation.R3PONumber = StrToNullableString(R3PONumber.Text)
        POInformation.DeliveryDate = StrToNullableDateTime(DeliveryDate.Text)

        'フォーム右段
        POInformation.DueDate = StrToNullableDateTime(DueDate.Text)
        POInformation.GoodsArrivedDate = StrToNullableDateTime(GoodsArrivedDate.Text)
        POInformation.LotNumber = StrToNullableString(LotNumber.Text)
        POInformation.InvoiceReceivedDate = StrToNullableDateTime(InvoceReceivedDate.Text)
        POInformation.ImportCustomClearanceDate = StrToNullableDateTime(ImportCustomClearanceDate.Text)
        POInformation.QMStartingDate = StrToNullableDateTime(QMStartingDate.Text)
        POInformation.QMFinishDate = StrToNullableDateTime(QMFinishDate.Text)
        POInformation.QMResult = StrToNullableString(QMResult.Text)
        POInformation.RequestQuantity = StrToNullableString(RequestQuantity.Text)
        POInformation.ScheduledExportDate = StrToNullableDateTime(ScheduledExportDate.Text)
        POInformation.PurchasingRequisitionNumber = StrToNullableString(PurchasingRequisitionNumber.Text)
        POInformation.CancellationDate = StrToNullableDateTime(CancellationDate.Text)

        UpdatePOInfomation(POInformation)
    End Sub

    ''' <summary>
    ''' POテーブルのデータを取得します。
    ''' </summary>
    ''' <param name="PONumber">対象となるPONumber</param>
    ''' <returns>取得したPOInformationType型データ</returns>
    ''' <remarks></remarks>
    Private Function SelectPOInformation(ByVal PONumber As Integer) As POInformationType

        Dim PoInformation As POInformationType = New POInformationType()

        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()

            cmd.CommandText = CreateSQLForSelectPOInfomation()
            cmd.Parameters.AddWithValue("PONumber", PONumber)

            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.Read Then

                PoInformation.PONumber = DBObjToNullableInt(dr("PONumber"))
                PoInformation.R3PONumber = dr("R3PONumber").ToString()
                PoInformation.R3POLineNumber = dr("R3POLineNumber").ToString()
                PoInformation.PODate = DBObjToNullableDateTime(dr("PODate"))
                PoInformation.POLocationCode = dr("POLocationCode").ToString()
                PoInformation.POLocationName = dr("POLocationName").ToString()
                PoInformation.POUserID = DBObjToNullableInt(dr("POUserID"))
                PoInformation.POUserName = dr("POUserName").ToString()
                PoInformation.SOLocationCode = dr("SOLocationCode").ToString()
                PoInformation.SOLocationName = dr("SOLocationName").ToString()
                PoInformation.SOUserID = DBObjToNullableInt(dr("SOUserID"))
                PoInformation.SOUserName = dr("SOUserName").ToString()
                PoInformation.ProductID = DBObjToNullableInt(dr("ProductID"))
                PoInformation.ProductNumber = dr("ProductNumber").ToString()
                PoInformation.ProductName = dr("ProductName").ToString()
                PoInformation.SupplierCode = DBObjToNullableInt(dr("SupplierCode"))
                PoInformation.SupplierName = dr("SupplierName").ToString()
                PoInformation.R3SupplierCode = dr("R3SupplierCode").ToString()
                PoInformation.R3SupplierName = dr("R3SupplierName").ToString
                PoInformation.MakerCode = DBObjToNullableInt(dr("MakerCode"))
                PoInformation.MakerName = dr("MakerName").ToString()
                PoInformation.R3MakerCode = dr("R3MakerCode").ToString()
                PoInformation.R3MakerName = dr("R3MakerName").ToString()
                PoInformation.OrderQuantity = DBObjToNullableDecimal(dr("OrderQuantity"))
                PoInformation.OrderUnitCode = dr("OrderUnitCode").ToString()
                PoInformation.DeliveryDate = DBObjToNullableDateTime(dr("DeliveryDate"))
                PoInformation.CurrencyCode = dr("CurrencyCode").ToString()
                PoInformation.UnitPrice = DBObjToNullableDecimal(dr("UnitPrice"))
                PoInformation.PerQuantity = DBObjToNullableDecimal(dr("PerQuantity"))
                PoInformation.PerUnitCode = dr("PerUnitCode").ToString()
                PoInformation.PaymentTermCode = dr("PaymentTermCode").ToString()
                PoInformation.PaymentTermText = dr("PaymentTermText").ToString()
                PoInformation.IncotermsCode = dr("IncotermsCode").ToString()
                PoInformation.IncotermsText = dr("IncotermsText").ToString()
                PoInformation.DeliveryTerm = dr("DeliveryTerm").ToString()
                PoInformation.PurposeCode = dr("PurposeCode").ToString()
                PoInformation.PurposeText = dr("PurposeText").ToString()
                PoInformation.RawMaterialFor = dr("RawMaterialFor").ToString()
                PoInformation.RequestedBy = dr("RequestedBy").ToString()
                PoInformation.SupplierItemNumber = dr("SupplierItemNumber").ToString()
                PoInformation.SupplierLotNumber = dr("SupplierLotNumber").ToString()
                PoInformation.DueDate = DBObjToNullableDateTime(dr("DueDate"))
                PoInformation.GoodsArrivedDate = DBObjToNullableDateTime(dr("GoodsArrivedDate"))
                PoInformation.LotNumber = dr("LotNumber").ToString()
                PoInformation.InvoiceReceivedDate = DBObjToNullableDateTime(dr("InvoiceReceivedDate"))
                PoInformation.ImportCustomClearanceDate = DBObjToNullableDateTime(dr("ImportCustomClearanceDate"))
                PoInformation.QMStartingDate = DBObjToNullableDateTime(dr("QMStartingDate"))
                PoInformation.QMFinishDate = DBObjToNullableDateTime(dr("QMFinishDate"))
                PoInformation.QMResult = dr("QMResult").ToString()
                PoInformation.RequestQuantity = dr("RequestQuantity").ToString()
                PoInformation.ScheduledExportDate = DBObjToNullableDateTime(dr("ScheduledExportDate"))
                PoInformation.PurchasingRequisitionNumber = dr("PurchasingRequisitionNumber").ToString()
                PoInformation.isCancelled = DBObjToNullableBoolean(dr("isCancelled"))
                PoInformation.CancellationDate = DBObjToNullableDateTime(dr("CancellationDate"))
                PoInformation.RFQLineNumber = DBObjToNullableInt(dr("RFQLineNumber"))
                PoInformation.ParPONumber = DBObjToNullableInt(dr("ParPONumber"))
                PoInformation.StatusCode = dr("StatusCode").ToString()
                PoInformation.Status = dr("Status").ToString()
                PoInformation.StatusChangeDate = DBObjToNullableDateTime(dr("StatusChangeDate"))
                PoInformation.StatusSortOrder = DBObjToNullableInt(dr("StatusSortOrder"))
                PoInformation.CreatedBy = DBObjToNullableInt(dr("CreatedBy"))
                PoInformation.CreateDate = DBObjToNullableDateTime(dr("CreateDate"))
                PoInformation.UpdatedBy = DBObjToNullableInt(dr("UpdatedBy"))
                PoInformation.UpdateDate = DBObjToNullableDateTime(dr("UpdateDate"))

            End If

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try

        Return PoInformation

    End Function

    ''' <summary>
    ''' PO取得SQK文字列を生成します。
    ''' </summary>
    ''' <returns>生成した文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateSQLForSelectPOInfomation() As String

        Dim sb_SQL As StringBuilder = New StringBuilder()
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("	PONumber, ")
        sb_SQL.Append("	R3PONumber, ")
        sb_SQL.Append("	R3POLineNumber, ")
        sb_SQL.Append("	PODate, ")
        sb_SQL.Append("	POLocationCode, ")
        sb_SQL.Append("	POLocationName, ")
        sb_SQL.Append("	POUserID, ")
        sb_SQL.Append("	POUserName, ")
        sb_SQL.Append("	SOLocationCode, ")
        sb_SQL.Append("	SOLocationName, ")
        sb_SQL.Append("	SOUserID, ")
        sb_SQL.Append("	SOUserName, ")
        sb_SQL.Append("	ProductID, ")
        sb_SQL.Append("	ProductNumber, ")
        sb_SQL.Append("	ProductName, ")
        sb_SQL.Append("	SupplierCode, ")
        sb_SQL.Append("	SupplierName, ")
        sb_SQL.Append("	R3SupplierCode, ")
        sb_SQL.Append("	R3SupplierName, ")
        sb_SQL.Append("	MakerCode, ")
        sb_SQL.Append("	MakerName, ")
        sb_SQL.Append("	R3MakerCode, ")
        sb_SQL.Append("	R3MakerName, ")
        sb_SQL.Append("	OrderQuantity, ")
        sb_SQL.Append("	OrderUnitCode, ")
        sb_SQL.Append("	DeliveryDate, ")
        sb_SQL.Append("	CurrencyCode, ")
        sb_SQL.Append("	UnitPrice, ")
        sb_SQL.Append("	PerQuantity, ")
        sb_SQL.Append("	PerUnitCode, ")
        sb_SQL.Append("	PaymentTermCode, ")
        sb_SQL.Append("	PaymentTermText, ")
        sb_SQL.Append("	IncotermsCode, ")
        sb_SQL.Append("	IncotermsText, ")
        sb_SQL.Append("	DeliveryTerm, ")
        sb_SQL.Append("	PurposeCode, ")
        sb_SQL.Append("	PurposeText, ")
        sb_SQL.Append("	RawMaterialFor, ")
        sb_SQL.Append("	RequestedBy, ")
        sb_SQL.Append("	SupplierItemNumber, ")
        sb_SQL.Append("	SupplierLotNumber, ")
        sb_SQL.Append("	DueDate, ")
        sb_SQL.Append("	GoodsArrivedDate, ")
        sb_SQL.Append("	LotNumber, ")
        sb_SQL.Append("	InvoiceReceivedDate, ")
        sb_SQL.Append("	ImportCustomClearanceDate, ")
        sb_SQL.Append("	QMStartingDate, ")
        sb_SQL.Append("	QMFinishDate, ")
        sb_SQL.Append("	QMResult, ")
        sb_SQL.Append("	RequestQuantity, ")
        sb_SQL.Append("	ScheduledExportDate, ")
        sb_SQL.Append("	PurchasingRequisitionNumber, ")
        sb_SQL.Append("	isCancelled, ")
        sb_SQL.Append("	CancellationDate, ")
        sb_SQL.Append("	RFQNumber, ")
        sb_SQL.Append("	RFQLineNumber, ")
        sb_SQL.Append("	ParPONumber, ")
        sb_SQL.Append("	StatusCode, ")
        sb_SQL.Append("	Status, ")
        sb_SQL.Append("	StatusChangeDate, ")
        sb_SQL.Append("	StatusSortOrder, ")
        sb_SQL.Append("	CreatedBy, ")
        sb_SQL.Append("	CreateDate, ")
        sb_SQL.Append("	UpdatedBy, ")
        sb_SQL.Append("	UpdateDate ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("	v_PO ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("	PONumber = @PONumber")

        Return sb_SQL.ToString()
    End Function

    ''' <summary>
    ''' POテーブルのデータを更新します。
    ''' </summary>
    ''' <param name="POInfomation">更新するPOInformationType型データ</param>
    ''' <remarks></remarks>
    Private Sub UpdatePOInfomation(ByRef POInfomation As POInformationType)

        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()

            cmd.CommandText = CreateSQLForUpdatePOInfomation()

            cmd.Parameters.AddWithValue("R3PONumber", NullableStringToDBObject(POInfomation.R3PONumber))
            cmd.Parameters.AddWithValue("R3POLineNumber", NullableVariableToDBObject(POInfomation.R3POLineNumber))
            cmd.Parameters.AddWithValue("PODate", NullableVariableToDBObject(POInfomation.PODate))
            cmd.Parameters.AddWithValue("POLocationCode", NullableVariableToDBObject(POInfomation.POLocationCode))
            cmd.Parameters.AddWithValue("POUserID", NullableVariableToDBObject(POInfomation.POUserID))
            cmd.Parameters.AddWithValue("SOLocationCode", NullableVariableToDBObject(POInfomation.SOLocationCode))
            cmd.Parameters.AddWithValue("SOUserID", NullableVariableToDBObject(POInfomation.SOUserID))
            cmd.Parameters.AddWithValue("ProductID", NullableVariableToDBObject(POInfomation.ProductID))
            cmd.Parameters.AddWithValue("SupplierCode", NullableVariableToDBObject(POInfomation.SupplierCode))
            cmd.Parameters.AddWithValue("MakerCode", NullableVariableToDBObject(POInfomation.MakerCode))
            cmd.Parameters.AddWithValue("OrderQuantity", NullableVariableToDBObject(POInfomation.OrderQuantity))
            cmd.Parameters.AddWithValue("OrderUnitCode", NullableVariableToDBObject(POInfomation.OrderUnitCode))
            cmd.Parameters.AddWithValue("DeliveryDate", NullableVariableToDBObject(POInfomation.DeliveryDate))
            cmd.Parameters.AddWithValue("CurrencyCode", NullableVariableToDBObject(POInfomation.CurrencyCode))
            cmd.Parameters.AddWithValue("UnitPrice", NullableVariableToDBObject(POInfomation.UnitPrice))
            cmd.Parameters.AddWithValue("PerQuantity", NullableVariableToDBObject(POInfomation.PerQuantity))
            cmd.Parameters.AddWithValue("PerUnitCode", NullableVariableToDBObject(POInfomation.PerUnitCode))
            cmd.Parameters.AddWithValue("PaymentTermCode", NullableVariableToDBObject(POInfomation.PaymentTermCode))
            cmd.Parameters.AddWithValue("IncotermsCode", NullableVariableToDBObject(POInfomation.IncotermsCode))
            cmd.Parameters.AddWithValue("DeliveryTerm", NullableVariableToDBObject(POInfomation.DeliveryTerm))
            cmd.Parameters.AddWithValue("PurposeCode", NullableVariableToDBObject(POInfomation.PurposeCode))
            cmd.Parameters.AddWithValue("RawMaterialFor", NullableVariableToDBObject(POInfomation.RawMaterialFor))
            cmd.Parameters.AddWithValue("RequestedBy", NullableVariableToDBObject(POInfomation.RequestedBy))
            cmd.Parameters.AddWithValue("SupplierItemNumber", NullableVariableToDBObject(POInfomation.SupplierItemNumber))
            cmd.Parameters.AddWithValue("SupplierLotNumber", NullableVariableToDBObject(POInfomation.SupplierLotNumber))
            cmd.Parameters.AddWithValue("DueDate", NullableVariableToDBObject(POInfomation.DueDate))
            cmd.Parameters.AddWithValue("GoodsArrivedDate", NullableVariableToDBObject(POInfomation.GoodsArrivedDate))
            cmd.Parameters.AddWithValue("LotNumber", NullableVariableToDBObject(POInfomation.LotNumber))
            cmd.Parameters.AddWithValue("InvoiceReceivedDate", NullableVariableToDBObject(POInfomation.InvoiceReceivedDate))
            cmd.Parameters.AddWithValue("ImportCustomClearanceDate", NullableVariableToDBObject(POInfomation.ImportCustomClearanceDate))
            cmd.Parameters.AddWithValue("QMStartingDate", NullableVariableToDBObject(POInfomation.QMStartingDate))
            cmd.Parameters.AddWithValue("QMFinishDate", NullableVariableToDBObject(POInfomation.QMFinishDate))
            cmd.Parameters.AddWithValue("QMResult", NullableVariableToDBObject(POInfomation.QMResult))
            cmd.Parameters.AddWithValue("RequestQuantity", NullableVariableToDBObject(POInfomation.RequestQuantity))
            cmd.Parameters.AddWithValue("ScheduledExportDate", NullableVariableToDBObject(POInfomation.ScheduledExportDate))
            cmd.Parameters.AddWithValue("PurchasingRequisitionNumber", NullableVariableToDBObject(POInfomation.PurchasingRequisitionNumber))
            cmd.Parameters.AddWithValue("isCancelled", NullableVariableToDBObject(POInfomation.isCancelled))
            cmd.Parameters.AddWithValue("CancellationDate", NullableVariableToDBObject(POInfomation.CancellationDate))
            cmd.Parameters.AddWithValue("RFQLineNumber", NullableVariableToDBObject(POInfomation.RFQLineNumber))
            cmd.Parameters.AddWithValue("ParPONumber", NullableVariableToDBObject(POInfomation.ParPONumber))
            cmd.Parameters.AddWithValue("CreatedBy", NullableVariableToDBObject(POInfomation.CreatedBy))
            cmd.Parameters.AddWithValue("CreateDate", NullableVariableToDBObject(POInfomation.CreateDate))
            cmd.Parameters.AddWithValue("UpdatedBy", NullableVariableToDBObject(POInfomation.UpdatedBy))
            cmd.Parameters.AddWithValue("UpdateDate", NullableVariableToDBObject(POInfomation.UpdateDate))
            cmd.Parameters.AddWithValue("PONumber", NullableVariableToDBObject(POInfomation.PONumber))

            conn.Open()
            cmd.ExecuteNonQuery()

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try

    End Sub

    ''' <summary>
    ''' PO更新SQL文字列を生成します。
    ''' </summary>
    ''' <returns>生成した文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateSQLForUpdatePOInfomation() As String
        Dim sb_SQL As StringBuilder = New StringBuilder()

        sb_SQL.Append("UPDATE PO SET ")
        sb_SQL.Append("	R3PONumber = @R3PONumber, ")
        sb_SQL.Append("	R3POLineNumber = @R3POLineNumber, ")
        sb_SQL.Append("	PODate = @PODate, ")
        sb_SQL.Append("	POLocationCode = @POLocationCode, ")
        sb_SQL.Append("	POUserID = @POUserID, ")
        sb_SQL.Append("	SOLocationCode = @SOLocationCode, ")
        sb_SQL.Append("	SOUserID = @SOUserID, ")
        sb_SQL.Append("	ProductID = @ProductID, ")
        sb_SQL.Append("	SupplierCode = @SupplierCode, ")
        sb_SQL.Append("	MakerCode = @MakerCode, ")
        sb_SQL.Append("	OrderQuantity = @OrderQuantity, ")
        sb_SQL.Append("	OrderUnitCode = @OrderUnitCode, ")
        sb_SQL.Append("	DeliveryDate = @DeliveryDate, ")
        sb_SQL.Append("	CurrencyCode = @CurrencyCode, ")
        sb_SQL.Append("	UnitPrice = @UnitPrice, ")
        sb_SQL.Append("	PerQuantity = @PerQuantity, ")
        sb_SQL.Append("	PerUnitCode = @PerUnitCode, ")
        sb_SQL.Append("	PaymentTermCode = @PaymentTermCode, ")
        sb_SQL.Append("	IncotermsCode = @IncotermsCode, ")
        sb_SQL.Append("	DeliveryTerm = @DeliveryTerm, ")
        sb_SQL.Append("	PurposeCode = @PurposeCode, ")
        sb_SQL.Append("	RawMaterialFor = @RawMaterialFor, ")
        sb_SQL.Append("	RequestedBy = @RequestedBy, ")
        sb_SQL.Append("	SupplierItemNumber = @SupplierItemNumber, ")
        sb_SQL.Append("	SupplierLotNumber = @SupplierLotNumber, ")
        sb_SQL.Append("	DueDate = @DueDate, ")
        sb_SQL.Append("	GoodsArrivedDate = @GoodsArrivedDate, ")
        sb_SQL.Append("	LotNumber = @LotNumber, ")
        sb_SQL.Append("	InvoiceReceivedDate = @InvoiceReceivedDate, ")
        sb_SQL.Append("	ImportCustomClearanceDate = @ImportCustomClearanceDate, ")
        sb_SQL.Append("	QMStartingDate = @QMStartingDate, ")
        sb_SQL.Append("	QMFinishDate = @QMFinishDate, ")
        sb_SQL.Append("	QMResult = @QMResult, ")
        sb_SQL.Append("	RequestQuantity = @RequestQuantity, ")
        sb_SQL.Append("	ScheduledExportDate = @ScheduledExportDate, ")
        sb_SQL.Append("	PurchasingRequisitionNumber = @PurchasingRequisitionNumber, ")
        sb_SQL.Append("	isCancelled = @isCancelled, ")
        sb_SQL.Append("	CancellationDate = @CancellationDate, ")
        sb_SQL.Append("	RFQLineNumber = @RFQLineNumber, ")
        sb_SQL.Append("	ParPONumber = @ParPONumber, ")
        sb_SQL.Append("	UpdatedBy = @UpdatedBy, ")
        sb_SQL.Append("	UpdateDate = GETDATE() ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("	PONumber = @PONumber")

        Return sb_SQL.ToString()
    End Function

#Region "DB読み込み時変換関数"

    ''' <summary>
    ''' DBオブジェクトをSystem.stringに変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.object</param>
    ''' <returns>変換したSystem.string 。DBNullの場合はstring.Emptyを返します。</returns>
    Public Shared Function DBObjToString(ByVal value As Object) As String
        Return DBObjToString(value, String.Empty)
    End Function

    ''' <summary>
    ''' DBオブジェクトをSystem.stringに変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.object</param>
    ''' <returns>変換したSystem.string 。DBNullの場合はstring.Emptyを返します。</returns>
    Public Shared Function DBObjToObj(ByVal value As Object) As Object
        Return DBObjToString(value, String.Empty)
    End Function



    ''' <summary>
    ''' DBオブジェクトをSystem.stringに変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.object</param>
    ''' <param name="defaultValue">変換対象のObjectがDBNullの場合に返される値</param>
    ''' <returns>変換したSystem.string 。DBNullの場合は引数で指定されたSystem.stringを返します。</returns>
    Public Shared Function DBObjToString(ByVal value As Object, ByVal defaultValue As String) As String
        If Convert.IsDBNull(value) Then
            Return defaultValue
        Else
            Return value.ToString()
        End If
    End Function

    ''' <summary>
    ''' DBオブジェクトをSystem.Long(int64)に変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.object</param>
    ''' <param name="defaulfValue">変換対象のObjectがDBNullの場合に返される値</param>
    ''' <returns>変換したSystem.string 。DBNullの場合は引数で指定されたSystem.Long(System.Int64)を返します。</returns>
    Public Shared Function DBObjToLong(ByVal value As Object, ByVal defaulfValue As Long) As Long
        Return DBObjToInt64(value, defaulfValue)
    End Function

    ''' <summary>
    ''' DBオブジェクトをSystem.Int64に変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <param name="defaultValue">変換対象のObjectがDBNullの場合に返される値</param>
    ''' <returns>変換したSystem.Int64。DBNullの場合は引数で指定されたSystem.Int64を返します。</returns>
    Public Shared Function DBObjToInt64(ByVal value As Object, ByVal defaultValue As Long) As Long
        If Convert.IsDBNull(value) Then
            Return defaultValue
        Else
            Return Convert.ToInt64(value)
        End If
    End Function

    ''' <summary>
    ''' DBオブジェクトをSystem.Long(System.Int64)に変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したSystem.Long(System.Int64)。DBNullの場合は0を返します。</returns>
    Public Shared Function DBObjToLong(ByVal value As Object) As Long
        Return DBObjToInt64(value)
    End Function

    ''' <summary>
    ''' DBオブジェクトをSystem.Int64に変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したSystem.Int64。DBNullの場合は0を返します。</returns>
    Public Shared Function DBObjToInt64(ByVal value As Object) As Long
        Return DBObjToInt64(value, 0)
    End Function


    ''' <summary>
    ''' DBオブジェクトをSystem.Int32に変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <param name="defaultValue">変換対象のObjectがDBNullの場合に返される値</param>
    ''' <returns>変換したSystem.Int32。DBNullの場合は引数で指定されたSystem.Int32を返します。</returns>
    Public Shared Function DBObjToInt32(ByVal value As Object, ByVal defaultValue As Int32) As Int32
        If Convert.IsDBNull(value) Then
            Return defaultValue
        Else
            Return Convert.ToInt32(value)
        End If
    End Function


    ''' <summary>
    ''' DBオブジェクトをint(System.Int32)に変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したint(System.Int32)。DBNullの場合は0を返します。</returns>
    Public Shared Function DBObjToInt(ByVal value As Object, ByVal defaultValue As Integer) As Int32
        Return DBObjToInt32(value, defaultValue)
    End Function

    ''' <summary>
    ''' DBオブジェクトをSystem.Int32に変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したSystem.Int32。DBNullの場合は0を返します。</returns>
    Public Shared Function DBObjToInt32(ByVal value As Object) As Int32
        Return DBObjToInt32(value, 0)
    End Function

    ''' <summary>
    ''' DBオブジェクトをNullable System.Int32に変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したNullable System.Int32。</returns>
    Public Shared Function DBObjToNullableInt32(ByVal value As Object) As Int32?
        If Convert.IsDBNull(value) Then
            Return Nothing
        Else
            Return Convert.ToInt32(value)
        End If
    End Function

    ''' <summary>
    ''' DBオブジェクトをint(System.Int32)に変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したint(System.Int32)。DBNullの場合は0を返します。</returns>
    Public Shared Function DBObjToInt(ByVal value As Object) As Int32
        Return DBObjToInt32(value)
    End Function

    ''' <summary>
    ''' DBオブジェクトをNullable int(System.Int32)に変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したNullable int(System.Int32)。</returns>
    Public Shared Function DBObjToNullableInt(ByVal value As Object) As Int32?
        Return DBObjToNullableInt32(value)
    End Function


    ''' <summary>
    ''' DBオブジェクトをSystem.Decimalに変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <param name="defaultValue">変換対象のObjectがDBNullの場合に返される値</param>
    ''' <returns>変換したSystem.Decimal。DBNullの場合は引数で指定されたSystem.Decimalを返します。</returns>
    Public Shared Function DBObjToDecimal(ByVal value As Object, ByVal defaultValue As Decimal) As Decimal
        If Convert.IsDBNull(value) Then
            Return defaultValue
        Else
            Return Convert.ToDecimal(value)
        End If
    End Function

    ''' <summary>
    ''' DBオブジェクトをSystem.Decimalに変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したSystem.Decimal。DBNullの場合は0を返します。</returns>
    Public Shared Function DBObjToDecimal(ByVal value As Object) As Decimal
        Return DBObjToDecimal(value, 0)
    End Function

    ''' <summary>
    ''' DBオブジェクトをNullable System.Decimalに変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したNullable System.Decimal。</returns>
    Public Shared Function DBObjToNullableDecimal(ByVal value As Object) As Decimal?
        If Convert.IsDBNull(value) Then
            Return Nothing
        Else
            Return Convert.ToDecimal(value)
        End If
    End Function



    ''' <summary>
    ''' DBオブジェクトをSystem.Doubleに変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <param name="defaultValue">変換対象のObjectがDBNullの場合に返される値</param>
    ''' <returns>変換したSystem.Double。DBNullの場合は引数で指定されたSystem.Doubleを返します。</returns>
    Public Shared Function DBObjToDouble(ByVal value As Object, ByVal defaultValue As Double) As Double
        If Convert.IsDBNull(value) Then
            Return defaultValue
        Else
            Return Convert.ToDouble(value)
        End If
    End Function

    ''' <summary>
    ''' DBオブジェクトをSystem.Doubleに変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したSystem.Double。DBNullの場合は0を返します。</returns>
    Public Shared Function DBObjToDouble(ByVal value As Object) As Double
        Return DBObjToDouble(value, 0)
    End Function

    ''' <summary>
    ''' Nullable DateTimeオブジェクトを文字列に変換します。
    ''' </summary>
    ''' <param name="value">変換対象となる DateTime? オブジェクト</param>
    ''' <param name="format">書式指定文字列</param>
    ''' <returns>変換したstring 文字列</returns>
    Public Shared Function NullableDateToString(ByVal value As DateTime?, ByVal format As String) As String
        If value.HasValue Then
            Return (CType(value, DateTime).ToString(format))
        Else
            Return String.Empty
        End If
    End Function

    ''' <summary>
    ''' Nullable DateTimeオブジェクトを文字列に変換します。
    ''' </summary>
    ''' <param name="value">変換対象となる DateTime? オブジェクト</param>
    ''' <returns>変換したstring 文字列</returns>
    Public Shared Function NullableDateToString(ByVal value As DateTime?) As String
        Return NullableDateToString(value, "yyyy/MM/dd")
    End Function

    ''' <summary>
    ''' 整数に変換可能かを返します。
    ''' </summary>
    ''' <param name="value">検証対象となるsystem.string</param>
    ''' <returns>整数に変換が可能であればtrue、不可能ならfalse</returns>
    Public Shared Function IsNumeric(ByVal value As String) As Boolean
        Dim i As Integer
        Return Integer.TryParse(value, i)
    End Function

    ''' <summary>
    ''' 文字列をNullable DateTimeオブジェクトに変換します。
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Public Shared Function StrToNullableDateTime(ByVal value As String) As DateTime?
        Dim dt As DateTime = New DateTime()
        If DateTime.TryParse(value, dt) Then
            Return dt
        Else
            Return Nothing
        End If
    End Function


    ''' <summary>
    ''' 空白文字列をNothingオブジェクトに変換します。
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Public Shared Function StrToNullableString(ByVal value As String) As String
        If String.IsNullOrEmpty(value) Then
            Return Nothing
        Else
            Return value
        End If
    End Function



    ''' <summary>
    ''' Nullable DateTimeオブジェクトをDB Nullableを含んだオブジェクトに変換します。
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Public Shared Function NullableDateTimeToDBDate(ByVal value As DateTime?) As Object
        If value Is Nothing Then
            Return DBNull.Value
        Else
            Return value
        End If
    End Function

    ''' <summary>
    ''' Nullable System.StringをDBNullを含んだオブジェクトに変換します。
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Public Shared Function NullableStringToDBObject(ByVal value As String) As Object
        If String.IsNullOrEmpty(value) Then
            Return DBNull.Value
        Else
            Return value
        End If
    End Function



    ''' <summary>
    ''' Nullable 変数をDBNullを含んだオブジェクトに変換します。
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Public Shared Function NullableVariableToDBObject(ByVal value As Object) As Object
        If value Is Nothing Then
            Return DBNull.Value
        Else
            Return value
        End If
    End Function



    ''' <summary>
    ''' DBオブジェクトをNullable DateTimeオブジェクトに変換します。
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Public Shared Function DBObjToNullableDateTime(ByVal value As Object) As DateTime?
        Dim dt As DateTime = New DateTime()
        If DateTime.TryParse(value.ToString(), dt) Then
            Return dt
        Else
            Return Nothing
        End If
    End Function


    ''' <summary>
    ''' DBオブジェクトをNullable System.Booleanに変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したNullable System.Boolean。</returns>
    Public Shared Function DBObjToNullableBoolean(ByVal value As Object) As Boolean
        If Convert.IsDBNull(value) Then
            Return Nothing
        Else
            Return Boolean.Parse(value.ToString)
        End If
    End Function


    ''' <summary>
    ''' 文字列をDBNullを含んだintオブジェクトに変換します。
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Public Shared Function StringToDBInt(ByVal value As String) As Object
        Dim obRet As Object = DBNull.Value

        If Not String.IsNullOrEmpty(value) Then
            If IsNumeric(value) Then
                obRet = Integer.Parse(value)
            End If
        End If
        Return obRet
    End Function



#End Region


End Class
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

#Region "グローバル変数定義"

    Protected st_PONumber As String
    Protected st_Action As String
    Protected b_FormVisible As Boolean = True

#End Region

#Region "定数定義"
    ''' <summary>
    ''' エラー定数です。
    ''' </summary>
    ''' <remarks></remarks>
    Const ERR_LOCATION_INCONSITENT As String = "拠点が一致しません。"
    Const ERR_DATA_REMOVED_BY_OTHER As String = "このデータは他のユーザーによって削除されました。"
    Const ERR_DATA_CHAGED_BY_OTHER As String = "このデータは他のユーザーによって編集されました。その内容を確認し再度編集をお願いします"

    Const TABLE_NAME_PO As String = "PO"
    Const VIEW_NAME_PO As String = "v_PO"
    Const PK_NAME_PO As String = "PONumber"

    Const QUERY_KEY_ACTION As String = "Action"
    Const QUERY_KEY_PO_NUMBER As String = "PONumber"

    Const SESSION_KEY_ADMIN As String = "Purchase.isAdmin"
    Const SESSION_KEY_LOCATION As String = "LocationCode"


    Const ACTION_VALUE_UPDATE As String = "Update"
    Const ACTION_VALUE_CANCEL As String = "Cancel"

    Const FORMAT_DECIMAL As String = "G29"


#End Region

#Region "構造体定義"
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

#End Region

#Region "フォームイベント"

    ''' <summary>
    ''' このページのロードイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not (Request.QueryString(QUERY_KEY_ACTION) Is Nothing) Then
            st_Action = Request.QueryString(QUERY_KEY_ACTION).ToString()
        ElseIf Not (Request.Form(QUERY_KEY_ACTION) Is Nothing) Then
            st_Action = Request.Form(QUERY_KEY_ACTION).ToString()
        End If

        If Not (Request.QueryString(QUERY_KEY_PO_NUMBER) Is Nothing) Then
            st_PONumber = Request.QueryString(QUERY_KEY_PO_NUMBER).ToString()
        ElseIf Not (Request.Form(QUERY_KEY_PO_NUMBER) Is Nothing) Then
            st_PONumber = Request.Form(QUERY_KEY_PO_NUMBER).ToString()
        End If

        'TODO ダミーコードです。要削除
        'st_PONumber = "1000000011"
        'st_PONumber = "00011"

        If IsPostBack = False Then
            If IsNumeric(st_PONumber) = False Then
                Msg.Text = ERR_INVALID_PARAMETER
                b_FormVisible = False
                Exit Sub
            End If

            If ExistsPO(st_PONumber) = False Then
                Msg.Text = MSG_NO_DATA_FOUND
                b_FormVisible = False
                Exit Sub
            End If
            ClearForm()

            ViewPOInformationToForm(CInt(st_PONumber))

            POCorrespondence.OnClientClick = String.Format("popup('./POCorrespondence.aspx?PONumber={0}')", st_PONumber)
            ChiPOIssue.NavigateUrl = String.Format("./RFQSelect.aspx?ParPONumber={0}", st_PONumber)

        End If
    End Sub

    ''' <summary>
    ''' Updateボタンのクリックイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks></remarks>
    Protected Sub Update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Update.Click

        If ValidateForUpdate() = False Then
            Exit Sub
        End If

        If ValidateCommon() = False Then
            Exit Sub
        End If

        Dim i_PONumber As Integer = CInt(PO.Value)

        UpdatePOInfomationFromForm(i_PONumber)
        Msg.Text = String.Empty

        ViewPOInformationToForm(i_PONumber)

    End Sub

    ''' <summary>
    ''' Cancelボタンのクリックイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks></remarks>
    Protected Sub Cancell_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancell.Click

        If ValidateForUpdate() = False Then
            Exit Sub
        End If

        If Not ValidateDateTextBox(CancellationDate) Then
            Msg.Text = "Cancellation Date" & ERR_INCORRECT_FORMAT
            Exit Sub
        End If

        Dim i_PONumber As Integer = CInt(PO.Value)

        CancelPOInfomationFromForm(i_PONumber)
        Msg.Text = String.Empty

        ViewPOInformationToForm(i_PONumber)
    End Sub



#End Region

#Region "フォーム処理"
    ''' <summary>
    ''' フォームの表示・入力項目を初期化します。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearForm()
        'フォーム左段
        RFQNumber.Text = String.Empty
        R3PONumber.Text = String.Empty
        R3POLineNumber.Text = String.Empty
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
    ''' 指定されたPOデータを画面に表示します。
    ''' </summary>
    ''' <param name="PONumber">POテーブルの一意ID</param>
    ''' <remarks></remarks>
    Private Sub ViewPOInformationToForm(ByVal PONumber As Integer)

        PO.Value = PONumber.ToString()
        Dim POInformation As POInformationType = SelectPOInformation(PONumber)

        '関数戻り値が構造体でNothing判定できないため、主キーのPONumberがNothingかでデータ有無を判定
        If POInformation.PONumber Is Nothing Then
            Msg.Text = MSG_NO_DATA_FOUND
            b_FormVisible = False
            Exit Sub
        End If

        'フォーム左段
        RFQNumber.Text = POInformation.RFQNumber.ToString()
        R3PONumber.Text = POInformation.R3PONumber
        R3POLineNumber.Text = POInformation.R3POLineNumber
        PODate.Text = GetLocalTime(POInformation.PODate)
        POUser.Text = POInformation.POUserName
        POLocation.Text = POInformation.POLocationName
        ProductNumber.Text = POInformation.ProductNumber
        'ProductNameは表示時に40文字制限があります
        ProductName.Text = CutShort(POInformation.ProductName.ToString())

        OrderQuantity.Text = NullableDecimalToString(POInformation.OrderQuantity, FORMAT_DECIMAL)
        OrderUnit.Text = POInformation.OrderUnitCode
        OrderPiece.Text = NullableDecimalToString(POInformation.UnitPrice, FORMAT_DECIMAL)
        DeliveryDate.Text = GetLocalTime(POInformation.DeliveryDate)
        Currency.Text = POInformation.CurrencyCode
        UnitPrice.Text = NullableDecimalToString(POInformation.UnitPrice, FORMAT_DECIMAL)
        PerQuantity.Text = NullableDecimalToString(POInformation.PerQuantity, FORMAT_DECIMAL)
        PerUnit.Text = POInformation.PerUnitCode
        R3SupplierCode.Text = POInformation.R3SupplierCode
        R3SupplierName.Text = POInformation.R3SupplierName
        R3MakerCode.Text = POInformation.R3MakerCode
        R3MakerName.Text = POInformation.R3MakerName
        PaymentTerm.Text = POInformation.PaymentTermText
        Incoterms.Text = POInformation.IncotermsCode
        DeliveryTerm.Text = POInformation.DeliveryTerm
        Purpose.Text = POInformation.PurposeText
        RawMaterialFor.Text = POInformation.RawMaterialFor
        RequestedBy.Text = POInformation.RequestedBy
        SupplierItemNumber.Text = POInformation.SupplierItemNumber
        SupplierLotNumber.Text = POInformation.SupplierLotNumber
        'フォーム右段
        DueDate.Text = GetLocalTime(POInformation.DueDate)
        GoodsArrivedDate.Text = GetLocalTime(POInformation.GoodsArrivedDate)
        LotNumber.Text = POInformation.LotNumber
        InvoceReceivedDate.Text = GetLocalTime(POInformation.InvoiceReceivedDate)
        ImportCustomClearanceDate.Text = GetLocalTime(POInformation.ImportCustomClearanceDate)
        QMStartingDate.Text = GetLocalTime(POInformation.QMStartingDate)
        QMFinishDate.Text = GetLocalTime(POInformation.QMFinishDate)
        QMResult.Text = POInformation.QMResult
        RequestQuantity.Text = POInformation.RequestQuantity
        ScheduledExportDate.Text = GetLocalTime(POInformation.ScheduledExportDate)
        PurchasingRequisitionNumber.Text = POInformation.PurchasingRequisitionNumber
        CancellationDate.Text = GetLocalTime(POInformation.CancellationDate)

        UpdateDate.Value = GetUpdateDate(TABLE_NAME_PO, PK_NAME_PO, POInformation.PONumber.ToString())

    End Sub

    ''' <summary>
    ''' フォーム上のデータをPOテーブルに保存します。（Cancel）
    ''' </summary>
    ''' <param name="PONumber">POテーブルの一意ID</param>
    ''' <remarks></remarks>
    Private Sub CancelPOInfomationFromForm(ByVal PONumber As Integer)

        Dim POInformation As POInformationType = SelectPOInformation(PONumber)

        If CancellationDate.Text.Trim() = String.Empty Then
            CancellationDate.Text = GetLocalTime(GetCurrentTime())
        End If

        POInformation.CancellationDate = StrToNullableDateTime(CancellationDate.Text)
        POInformation.isCancelled = False

        UpdatePOInfomation(POInformation)

    End Sub

    ''' <summary>
    ''' フォーム上のデータをPOテーブルに保存します。(Update)
    ''' </summary>
    ''' <param name="PONumber">POテーブルの一意ID</param>
    ''' <remarks></remarks>
    Private Sub UpdatePOInfomationFromForm(ByVal PONumber As Integer)
        Dim POInformation As POInformationType = SelectPOInformation(PONumber)

        'フォーム左段
        POInformation.R3PONumber = StrToNullableString(R3PONumber.Text.Trim())
        POInformation.R3POLineNumber = StrToNullableString(R3POLineNumber.Text.Trim())
        POInformation.DeliveryDate = GetDatabaseTime(DeliveryDate.Text.Trim())

        'フォーム右段
        POInformation.DueDate = GetDatabaseTime(DueDate.Text.Trim())
        POInformation.GoodsArrivedDate = GetDatabaseTime(GoodsArrivedDate.Text.Trim())
        POInformation.LotNumber = StrToNullableString(LotNumber.Text.Trim())
        POInformation.InvoiceReceivedDate = GetDatabaseTime(InvoceReceivedDate.Text.Trim())
        POInformation.ImportCustomClearanceDate = GetDatabaseTime(ImportCustomClearanceDate.Text.Trim())
        POInformation.QMStartingDate = GetDatabaseTime(QMStartingDate.Text.Trim())
        POInformation.QMFinishDate = GetDatabaseTime(QMFinishDate.Text.Trim())
        POInformation.QMResult = StrToNullableString(QMResult.Text.Trim())
        POInformation.RequestQuantity = StrToNullableString(RequestQuantity.Text.Trim())
        POInformation.ScheduledExportDate = GetDatabaseTime(ScheduledExportDate.Text.Trim())
        POInformation.PurchasingRequisitionNumber = StrToNullableString(PurchasingRequisitionNumber.Text.Trim())
        POInformation.CancellationDate = GetDatabaseTime(CancellationDate.Text.Trim())

        UpdatePOInfomation(POInformation)
    End Sub


    ''' <summary>
    ''' Update,Cancel共通検証
    ''' </summary>
    ''' <returns>正当なときはTrue 不正なときはFalseを返します。</returns>
    ''' <remarks></remarks>
    Private Function ValidateCommon() As Boolean

        If Not ValidateDateTextBox(DeliveryDate) Then
            Msg.Text = "Delivery Date" & ERR_INCORRECT_FORMAT
            Return False
        End If

        If Not ValidateDateTextBox(DueDate) Then
            Msg.Text = "Due Date" & ERR_INCORRECT_FORMAT
            Return False
        End If

        If Not ValidateDateTextBox(GoodsArrivedDate) Then
            Msg.Text = "Goods ArrivedDate" & ERR_INCORRECT_FORMAT
            Return False
        End If

        If Not ValidateDateTextBox(InvoceReceivedDate) Then
            Msg.Text = "Invoice Received Date" & ERR_INCORRECT_FORMAT
            Return False
        End If

        If Not ValidateDateTextBox(ImportCustomClearanceDate) Then
            Msg.Text = "Import Custom Clearance Date" & ERR_INCORRECT_FORMAT
            Return False
        End If

        If Not ValidateDateTextBox(QMFinishDate) Then
            Msg.Text = "QM Finish Date" & ERR_INCORRECT_FORMAT
            Return False
        End If

        If Not ValidateDateTextBox(ScheduledExportDate) Then
            Msg.Text = "Scheduled Export Date" & ERR_INCORRECT_FORMAT
            Return False
        End If

        If Not ValidateDateTextBox(CancellationDate) Then
            Msg.Text = "CancellationDate" & ERR_INCORRECT_FORMAT
            Return False
        End If
        Return True

    End Function

    ''' <summary>
    ''' 日付型テキストボックスの正当性を評価します。
    ''' </summary>
    ''' <param name="TargetObject">対象となるTexrBoxオブジェクト</param>
    ''' <returns>正当なときはTrue 不正なときはFalseを返します</returns>
    ''' <remarks>評価対象の文字列が空のときはTrueと判定されます。</remarks>
    Private Function ValidateDateTextBox(ByVal TargetObject As TextBox) As Boolean

        Return ValidateDateTextBox(TargetObject, True)

    End Function

    ''' <summary>
    ''' 日付型テキストボックスの正当性を評価します。
    ''' </summary>
    ''' <param name="TargetObject">対象となるTexrBoxオブジェクト</param>
    ''' <param name="AllowEmpty">空の文字列を許すかを設定します。Trueは許可 Falseは不許可 </param>
    ''' <returns>正当なときはTrue 不正なときはFalseを返します</returns>
    ''' <remarks></remarks>
    Private Function ValidateDateTextBox(ByVal TargetObject As TextBox, ByVal AllowEmpty As Boolean) As Boolean

        If AllowEmpty And TargetObject.Text.Trim = String.Empty Then
            Return True
        End If

        If Not Regex.IsMatch(TargetObject.Text, DATE_REGEX_OPTIONAL) Then
            Return False
        End If

        If Not IsDate(TargetObject.Text.Trim) Then
            Return False
        End If
        Return True
    End Function


    ''' <summary>
    ''' 更新時のパラメータ、設定値の正当性チェックを行います。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateForUpdate() As Boolean

        If st_Action <> ACTION_VALUE_UPDATE Then
            Msg.Text = ERR_INVALID_PARAMETER
            Return False
        End If

        If IsNumeric(PO.Value) = False Then
            Msg.Text = ERR_INVALID_PARAMETER
            Return False
        End If

        Dim i_PONumber As Integer = CInt(PO.Value)

        Dim POInformation As POInformationType = SelectPOInformation(i_PONumber)
        If CBool(Session(SESSION_KEY_ADMIN)) = False And POInformation.POLocationCode <> Session(SESSION_KEY_LOCATION).ToString() Then
            Msg.Text = ERR_LOCATION_INCONSITENT
            Return False
        End If

        If ExistsPO(i_PONumber.ToString()) = False Then
            Msg.Text = ERR_DATA_REMOVED_BY_OTHER
            Return False
        End If

        If isLatestData(TABLE_NAME_PO, PK_NAME_PO, i_PONumber.ToString(), UpdateDate.Value) = False Then
            Msg.Text = ERR_DATA_CHAGED_BY_OTHER
            Return False
        End If
        Return True

    End Function

#End Region

#Region "クラス内共通処理"
    ''' <summary>
    ''' 指定されたPOのデータが存在するかを取得します。
    ''' </summary>
    ''' <param name="PONumber">POテーブルの一意ID</param>
    ''' <returns>存在するときはTure 存在しないときはFalse</returns>
    ''' <remarks></remarks>
    Private Function ExistsPO(ByVal PONumber As String) As Boolean

        'Return ExistenceConfirmation(TABLE_NAME_PO, PK_NAME_PO, PONumber)
        Return ExistenceConfirmation(VIEW_NAME_PO, PK_NAME_PO, PONumber)

    End Function


    ''' <summary>
    ''' ローカル時間を取得する
    ''' </summary>
    ''' <param name="DatabaseTime">データベース時間 (JST)</param>
    ''' <returns>ローカル時間</returns>
    ''' <remarks>Nullに対応したCommonのラッピング関数</remarks>
    Private Function GetLocalTime(ByVal DataBaseTime As Date?) As String
        If DataBaseTime Is Nothing Then
            Return String.Empty
        End If
        Return Common.GetLocalTime(Session(SESSION_KEY_LOCATION).ToString(), CType(DataBaseTime, Date))
    End Function


    ''' <summary>
    ''' データベース時間を取得する。
    ''' </summary>
    ''' <param name="LocalTime">ローカル時間</param>
    ''' <returns>データベース時間 (JST)</returns>
    ''' <remarks>Nullに対応したラッピング関数</remarks>
    Private Function GetDatabaseTime(ByVal LocalTime As Date?) As Date?

        If LocalTime Is Nothing Then
            Return Nothing
        End If

        Return CType(Common.GetDatabaseTime(Session(SESSION_KEY_LOCATION).ToString(), LocalTime.ToString()), Date?)

    End Function

    ''' <summary>
    ''' データベース時間を取得する。
    ''' </summary>
    ''' <param name="LocalTime">ローカル時間</param>
    ''' <returns>データベース時間 (JST)</returns>
    ''' <remarks>Nullに対応したラッピング関数</remarks>
    Private Function GetDatabaseTime(ByVal LocalTime As String) As Date?

        If LocalTime.Trim = String.Empty Then
            Return Nothing
        End If

        Return CType(Common.GetDatabaseTime(Session(SESSION_KEY_LOCATION).ToString(), LocalTime), Date?)

    End Function


    ''' <summary>
    ''' データベースから現在の時刻を取得します。
    ''' </summary>
    ''' <returns>取得した時刻</returns>
    ''' <remarks></remarks>
    Private Function GetCurrentTime() As DateTime

        Dim dt_Current As DateTime = New DateTime()
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()

            cmd.CommandText = "SELECT GETDATE() "

            conn.Open()
            dt_Current = CType(cmd.ExecuteScalar(), DateTime)

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
        Return dt_Current

    End Function

#End Region

#Region "DB登録処理"


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
                PoInformation.RFQNumber = DBObjToNullableInt(dr("RFQNumber"))
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
    ''' POデータ取得SQK文字列を生成します。
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
    ''' POテーブル更新SQL文字列を生成します。
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
#End Region

#Region "DBNull・.NET変数変換関数"
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
    ''' Nullable DateTimeオブジェクトを文字列に変換します。
    ''' </summary>
    ''' <param name="value">変換対象となる DateTime? オブジェクト</param>
    ''' <returns>変換したstring 文字列</returns>
    Public Shared Function NullableDateToString(ByVal value As DateTime?) As String
        Return NullableDateToString(value, "yyyy/MM/dd")
    End Function

    ''' <summary>
    ''' 文字列をNullable DateTimeオブジェクトに変換します。
    ''' </summary>
    ''' <param name="value">変換対象となるstring オブジェクト</param>
    ''' <returns>変換対象となる DateTime? オブジェクト</returns>
    Public Shared Function StrToNullableDateTime(ByVal value As String) As DateTime?
        Dim dt As DateTime = New DateTime()
        If DateTime.TryParse(value, dt) Then
            Return dt
        Else
            Return Nothing
        End If
    End Function

    ''' <summary>
    ''' Nullable DateTimeオブジェクト内をDBNullを含んだSystem.Objectオブジェクトに変換します。
    ''' </summary>
    ''' <param name="value">対象となるDateTime? オブジェクト</param>
    ''' <returns>DB Nullを含んだ System Object。</returns>
    Public Shared Function NullableDateTimeToDBDate(ByVal value As DateTime?) As Object
        If value Is Nothing Then
            Return DBNull.Value
        Else
            Return value
        End If
    End Function

    ''' <summary>
    ''' Nullable System.StringオブジェクトをDBNullを含んだSystem.Objectオブジェクトに変換します。
    ''' </summary>
    ''' <param name="value">対象となるSystem.String</param>
    ''' <returns>DB Nullを含んだ System Object。</returns>
    Public Shared Function NullableStringToDBObject(ByVal value As String) As Object
        If String.IsNullOrEmpty(value) Then
            Return DBNull.Value
        Else
            Return value
        End If
    End Function



    ''' <summary>
    ''' Nullable オブジェクトをDBNullを含んだSystem.Objectオブジェクトに変換します。
    ''' </summary>
    ''' <param name="value">対象となるSystem.Object</param>
    ''' <returns>DB Nullを含んだ System Object。</returns>
    Public Shared Function NullableVariableToDBObject(ByVal value As Object) As Object
        'TODO Stringで空文字列が来たときの対応
        If value Is Nothing Then
            Return DBNull.Value
        End If
        If String.IsNullOrEmpty(value.ToString()) Then
            Return DBNull.Value
        End If
        Return value
    End Function



    ''' <summary>
    ''' DBオブジェクトをNullable DateTimeオブジェクトに変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したNullable System.DateTime。</returns>
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
    ''' Systemn.StringをSystem.Intの値を持ったDBオブジェクトへ変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.String</param>
    ''' <returns>DB Nullを含んだ System Object。</returns>
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

#Region "Null・.NET変数変換関数"
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
    ''' Systemn.String内の空白をNothingに変換します。
    ''' </summary>
    ''' <param name="value">変換対象となるstring オブジェクト</param>
    ''' <returns>Nothingを含んだString文字列</returns>
    Public Shared Function StrToNullableString(ByVal value As String) As String
        If String.IsNullOrEmpty(value) Then
            Return Nothing
        Else
            Return value
        End If
    End Function

    ''' <summary>
    ''' Nullable Decimalオブジェクトを文字列に変換します。
    ''' </summary>
    ''' <param name="value">変換対象となる Decimal? オブジェクト</param>
    ''' <param name="format">書式指定文字列</param>
    ''' <returns>変換したstring 文字列</returns>
    Private Function NullableDecimalToString(ByVal value As Decimal?, ByVal format As String) As String
        If value Is Nothing Then
            Return String.Empty
        End If

        Return CType(value, Decimal).ToString(format)

    End Function

#End Region

End Class
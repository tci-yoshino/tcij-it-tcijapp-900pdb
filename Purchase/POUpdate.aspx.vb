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

    Protected st_PONumber As String = String.Empty
    Protected st_ParPONumber As String = String.Empty
    Protected st_Action As String = String.Empty
    Protected b_FormVisible As Boolean = True
    Protected b_ChildVisible As Boolean = False
    Protected b_ChiPOIssueVisible As Boolean = False
    Private i_OperatingUserID As Integer = -1

    ''' <summary>
    ''' エラー定数です。
    ''' </summary>
    ''' <remarks></remarks>
    Const ERR_LOCATION_INCONSISTENT As String = "You can not edit PO of other locations." '"拠点が一致しません。"
    Const ERR_LENGTH_OVER As String = "{0} には{1}文字以上登録することができません。"


    Const TABLE_NAME_PO As String = "PO"
    Const VIEW_NAME_PO As String = "v_PO"
    Const PK_NAME_PO As String = "PONumber"

    Const QUERY_KEY_ACTION As String = "Action"
    Const QUERY_KEY_PO_NUMBER As String = "PONumber"

    Const SESSION_KEY_ADMIN As String = "Purchase.isAdmin"
    Const SESSION_KEY_LOCATION As String = "LocationCode"
    Const SESSION_KEY_USER_ID As String = "UserID"


    Const ACTION_VALUE_UPDATE As String = "Update"
    Const ACTION_VALUE_CANCEL As String = "Cancell"

    Const FORMAT_DECIMAL As String = "G29"

    Const MAX_LENGTH_R3_PO_NUMBER As Integer = 10
    Const MAX_LENGTH_R3_PO_LINE_NUMBER As Integer = 5
    Const MAX_LENGTH_LOT_NUMBER As Integer = 10

    ''' <summary>
    ''' POデータを格納する構造体です。
    ''' </summary>
    ''' <remarks></remarks>
    Private Structure POInformationType

        Public PONumber As Integer?
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
        Public Confidential As String   'ReadOnly
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
        'Public isCancelled As Boolean?
        Public CancellationDate As DateTime?
        Public RFQNumber As Integer?
        Public RFQLineNumber As Integer?
        Public ParPONumber As Integer?
        Public Priority As String
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

        'Actionの取得
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

        '操作ユーザIDの取得
        If Common.IsInteger(Session(SESSION_KEY_USER_ID).ToString) Then
            i_OperatingUserID = Integer.Parse(Session(SESSION_KEY_USER_ID).ToString)
        Else
            Msg.Text = ERR_INVALID_PARAMETER
            b_FormVisible = False
            Exit Sub
        End If

        '初回呼び出し時のデータ読み出しとパラメータチェック
        If IsPostBack = False Then
            If IsInteger(st_PONumber) = False Then
                Msg.Text = ERR_INVALID_PARAMETER
                b_FormVisible = False
                Exit Sub
            End If

            If ExistsPO(st_PONumber) = False Then
                Msg.Text = MSG_NO_DATA_FOUND
                b_FormVisible = False
                Exit Sub
            End If
            b_FormVisible = True
            ClearForm()

            'プライオリティのプルダウンを設定する
            SetPriorityDropDownList(Priority, PRIORITY_FOR_EDIT)

            'プルダウンを設定する
            SetUnitDropDownList(SrcUnit)
            SetPurposeDropDownList(SrcPurpose)

            ViewPOInformationToForm(CInt(st_PONumber))
        Else
            If Purpose.Visible Then
                'プルダウンを設定する
                SetPurposeDropDownList(SrcPurpose)
            End If
        End If



            '親発注番号の取得
            'PO Correspondence / History リンクパラメータは 親発注番号(ParPONumber) 優先
            '親発注番号(ParPONumber) がない場合は、自身の 発注番号(PONumber) 
            Dim POInformation As POInformationType = SelectPOInformation(Integer.Parse(st_PONumber))
            If POInformation.ParPONumber Is Nothing Then
                st_ParPONumber = POInformation.PONumber.ToString()
            Else
                st_ParPONumber = POInformation.ParPONumber.ToString()
            End If



            ChiPOIssue.NavigateUrl = String.Format("./RFQSelect.aspx?ParPONumber={0}", st_PONumber)

            Update.PostBackUrl = String.Format("POUpdate.aspx?{0}={1}&{2}={3}", _
                                                QUERY_KEY_ACTION, ACTION_VALUE_UPDATE, _
                                                QUERY_KEY_PO_NUMBER, st_PONumber)

            Cancell.PostBackUrl = String.Format("POUpdate.aspx?{0}={1}&{2}={3}", _
                                                QUERY_KEY_ACTION, ACTION_VALUE_CANCEL, _
                                                QUERY_KEY_PO_NUMBER, st_PONumber)

    End Sub

    ''' <summary>
    ''' Updateボタンのクリックイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks></remarks>
    Protected Sub Update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Update.Click

        Msg.Text = String.Empty
        RunMsg.Text = String.Empty

        ChangeTextBoxValueToSingleByte()

        'Actionの確認
        If st_Action <> ACTION_VALUE_UPDATE Then
            Msg.Text = ERR_INVALID_PARAMETER
            Return
        End If

        If ValidateForUpdate() = False Then
            Exit Sub
        End If

        If ValidateCommon() = False Then
            Exit Sub
        End If

        Dim i_PONumber As Integer = CInt(st_PONumber)

        UpdatePOInfomationFromForm(i_PONumber)
        ViewPOInformationToForm(i_PONumber)
        RunMsg.Text = Common.MSG_DATA_UPDATED

    End Sub

    ''' <summary>
    ''' Cancelボタンのクリックイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks></remarks>
    Protected Sub Cancell_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Cancell.Click

        Msg.Text = String.Empty
        RunMsg.Text = String.Empty

        'Actionの確認
        If st_Action <> ACTION_VALUE_CANCEL Then
            Msg.Text = ERR_INVALID_PARAMETER
            Return
        End If

        If Not ValidateDateTextBox(CancellationDate) Then
            Msg.Text = "Cancellation Date" & ERR_INCORRECT_FORMAT
            Exit Sub
        End If

        If ValidateCommon() = False Then
            Exit Sub
        End If

        Dim i_PONumber As Integer = CInt(st_PONumber)

        CancelPOInfomationFromForm(i_PONumber)
        ViewPOInformationToForm(i_PONumber)
        RunMsg.Text = Common.MSG_DATA_UPDATED
    End Sub

    ''' <summary>
    ''' フォームの表示・入力項目を初期化します。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearForm()
        'フォーム左段
        Confidential.Text = String.Empty
        RFQNumber.Text = String.Empty
        R3PONumber.Text = String.Empty
        ParPONumber.Text = String.Empty
        R3POLineNumber.Text = String.Empty
        PODate.Text = String.Empty
        POLocationName.Text = String.Empty
        ProductNumber.Text = String.Empty
        ProductName.Text = String.Empty
        OrderQuantity.Text = String.Empty
        OrderUnit.Text = String.Empty
        'OrderPiece.Text = String.Empty
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
        LabelPurpose.Text = String.Empty
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

        'PO.Value = PONumber.ToString()
        Dim POInformation As POInformationType = SelectPOInformation(PONumber)

        '関数戻り値が構造体でNothing判定できないため、主キーのPONumberがNothingかでデータ有無を判定
        If POInformation.PONumber Is Nothing Then
            Msg.Text = MSG_NO_DATA_FOUND
            b_FormVisible = False
            Exit Sub
        End If

        '権限ロールに従い極秘品はエラーとする
        If Session(SESSION_ROLE_CODE).ToString = ROLE_WRITE_P OrElse Session(SESSION_ROLE_CODE).ToString = ROLE_READ_P Then
            If IsConfidentialItem(POInformation.ProductNumber) Then
                Response.Redirect("AuthError.html")
            End If
        End If

        'フォーム左段
        Confidential.Text = POInformation.Confidential
        RFQNumber.Text = POInformation.RFQNumber.ToString()
        R3PONumber.Text = POInformation.R3PONumber
        ParPONumber.Text = NullableIntToString(POInformation.ParPONumber)
        If String.IsNullOrEmpty(ParPONumber.Text) Then
            '表示データが親の場合
            Priority.SelectedValue = POInformation.Priority.ToString()
            LabelPriority.Text = POInformation.Priority.ToString()
        Else
            '表示データが子の場合、親の Priority を取得しセットする
            Dim st_Priority As String = GetParPOPriority(ParPONumber.Text)
            Priority.SelectedValue = st_Priority
            LabelPriority.Text = st_Priority
        End If


        R3POLineNumber.Text = POInformation.R3POLineNumber
        PODate.Text = GetLocalTime(POInformation.PODate)
        POUser.SelectedValue = POInformation.POUserID.ToString
        POLocationName.Text = POInformation.POLocationName
        ProductNumber.Text = POInformation.ProductNumber
        'ProductNameは表示時に40文字制限があります
        ProductName.Text = CutShort(POInformation.ProductName.ToString())

        OrderQuantity.Text = NullableDecimalToString(POInformation.OrderQuantity, FORMAT_DECIMAL)
        LabelOrderQuantity.Text = OrderQuantity.Text
        OrderUnit.SelectedValue = POInformation.OrderUnitCode.ToString
        LabelOrderUnit.Text = POInformation.OrderUnitCode
        'OrderPiece.Text = NullableDecimalToString(POInformation.UnitPrice, FORMAT_DECIMAL)
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
        LabelPurpose.Text = POInformation.PurposeText
        If Purpose.Visible = True Then Purpose.SelectedValue = POInformation.PurposeCode.ToString()
        LabelPurpose.Text = POInformation.PurposeText.ToString()
        HidPurposeCode.Value = POInformation.PurposeCode
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
        SOLocationCode.Value = POInformation.SOLocationCode.ToString
        SOUserID.Value = POInformation.SOUserID.ToString()

        'Par-PO Number と Chi-PO Request Quantity は、ParPONumber が設定されている (すなわち子 PO の) 場合のみ画面に表示する。
        b_ChildVisible = Not (POInformation.ParPONumber Is Nothing)

        'Chi-PO Issue リンクは 
        '1. "ParPONumber が設定されていない 
        '2. SupplierCode に該当するテーブル Supplier の Supplier.LocationCode が設定されている
        '3. その値がログインユーザの拠点 (セッション LocationCode) と一致する場合
        '画面に表示する。

        Dim st_SuppLiersLocation As String = GetSuppliers_Location(POInformation.SupplierCode)
        If (POInformation.ParPONumber Is Nothing) And (st_SuppLiersLocation <> String.Empty) And _
            (Session(SESSION_KEY_LOCATION).ToString = st_SuppLiersLocation) Then
            b_ChiPOIssueVisible = True
        Else
            b_ChiPOIssueVisible = False
        End If

        'ログインユーザ　＝ POUser　かつ　親 PONumber がない場合、Priority 編集可
        Dim st_POUser As String = String.Empty
        st_POUser = POUser.SelectedValue
        If String.IsNullOrEmpty(st_POUser) Then
            '画面初期表示時のみ SelectedValue で値が取得できないため、直接データ参照する
            st_POUser = POInformation.POUserID.ToString
        End If

        If ((String.IsNullOrEmpty(POInformation.ParPONumber.ToString)) AndAlso (i_OperatingUserID = Integer.Parse(st_POUser))) Then
            Priority.Enabled = True
            Priority.Visible = True
            LabelPriority.Visible = False
        Else
            Priority.Enabled = False
            Priority.Visible = False
            LabelPriority.Visible = True
        End If

        ' PO-User プルダウンの設定
        SetControl_SrcUser(POInformation.POLocationCode, CInt(POInformation.POUserID))

        ' SupplierName プルダウンの設定
        Dim SupplierCode As String = String.Empty
        Dim QuoLocationCode As String = String.Empty
        GetQuoLocationCode(POInformation.RFQLineNumber, QUOLocationCode, SupplierCode)
        SetSupplierDropDownList(SrcSupplier, SupplierCode.ToString, QuoLocationCode.ToString, Session(SESSION_KEY_LOCATION).ToString)

        If QuoLocationCode.ToString <> Session(SESSION_KEY_LOCATION).ToString Then Supplier.SelectedValue = POInformation.SupplierCode.ToString
        LabelSupplierCode.Value = POInformation.SupplierCode.ToString

        'OrderQuantity、OrderUnit、Purpose、SupplierName表示
        OrderQuantity.Visible = False
        LabelOrderQuantity.Visible = True
        OrderUnit.Visible = False
        LabelOrderUnit.Visible = True
        Purpose.Visible = False
        LabelPurpose.Visible = True
        Supplier.Visible = False
        R3SupplierName.Visible = True

        If POInformation.StatusCode <> STATUS_PAR_QM_FINISHED AndAlso _
             POInformation.StatusCode <> STATUS_PAR_PO_CANCELLED AndAlso POInformation.StatusCode <> STATUS_CHI_PO_CANCELLED Then
            OrderQuantity.Visible = True
            OrderUnit.Visible = True
            LabelOrderQuantity.Visible = False
            LabelOrderUnit.Visible = False
            If String.IsNullOrEmpty(ParPONumber.Text) Then
                '表示データが親の場合

                'Purposeプルダウン可視、ラベル不可視に設定
                Purpose.Visible = True
                LabelPurpose.Visible = False

                'SupplierNameプルダウン不可視、ラベル可視に設定
                If Not ExistenceConfirmation("V_PO", "ParPONumber", POInformation.PONumber.ToString) Then
                    '表示データが親PO、かつ子POのデータなしの場合、プルダウン可視、ラベル不可視に設定
                    Supplier.Visible = True
                    R3SupplierName.Visible = False
                End If
            Else
                '表示データが子の場合

                '親のPurposeを取得して表示　編集不可
                Dim st_PurposeText As String = String.Empty
                Dim st_PurposeCode As String = String.Empty
                Call GetParPOPurpose(ParPONumber.Text, st_PurposeCode, st_PurposeText)
                ParPurposeCode.Value = st_PurposeCode
                LabelPurpose.Text = st_PurposeText

                'SupplierName編集不可
                Supplier.Visible = False
                R3SupplierName.Visible = True

            End If
        End If

    End Sub


    ''' <summary>
    ''' フォーム上のデータをPOテーブルに保存します。（Cancel）
    ''' </summary>
    ''' <param name="PONumber">POテーブルの一意ID</param>
    ''' <remarks></remarks>
    Private Sub CancelPOInfomationFromForm(ByVal PONumber As Integer)

        Dim POInformation As POInformationType = SelectPOInformation(PONumber)

        If CancellationDate.Text.Trim() = String.Empty Then
            CancellationDate.Text = GetLocalTime(GetDBCurrentTime().Date)
        End If

        POInformation.CancellationDate = GetDatabaseTime(CancellationDate.Text.Trim())

        'POInformation.isCancelled = True

        UpdatePOInfomation(POInformation, POInformation)

    End Sub

    ''' <summary>
    ''' フォーム上のデータをPOテーブルに保存します。(Update)
    ''' </summary>
    ''' <param name="PONumber">POテーブルの一意ID</param>
    ''' <remarks></remarks>
    Private Sub UpdatePOInfomationFromForm(ByVal PONumber As Integer)
        Dim SrcPOInformation As POInformationType = SelectPOInformation(PONumber)
        Dim DstPOInformation As POInformationType = SrcPOInformation

        'フォーム左段

        Dim st_Priority As String = String.Empty
        If (Priority.Visible) Then
            'プルダウンが編集可能な場合はプルダウンから値を取得する
            st_Priority = Priority.SelectedValue
        Else
            st_Priority = LabelPriority.Text
        End If

        Dim st_PurposeCode As String = String.Empty
        If (Purpose.Visible) Then
            'プルダウンが編集可能な場合はプルダウンから値を取得する
            st_PurposeCode = Purpose.SelectedValue
        ElseIf String.IsNullOrEmpty(ParPONumber.Text) Then
            st_PurposeCode = HidPurposeCode.Value
        Else
            st_PurposeCode = ParPurposeCode.Value
        End If

        Dim st_OrderUnitCode As String = String.Empty
        If (OrderUnit.Visible) Then
            'プルダウンが編集可能な場合はプルダウンから値を取得する
            st_OrderUnitCode = OrderUnit.SelectedValue
        Else
            st_OrderUnitCode = LabelOrderUnit.Text
        End If

        Dim st_SupplierCode As String = String.Empty
        If (Supplier.Visible) Then
            'プルダウンが編集可能な場合はプルダウンから値を取得する
            st_SupplierCode = Supplier.SelectedValue
        Else
            st_SupplierCode = LabelSupplierCode.Value
        End If

        DstPOInformation.Priority = StrToNullableString(st_Priority)
        DstPOInformation.R3PONumber = StrToNullableString(R3PONumber.Text.Trim())
        DstPOInformation.R3POLineNumber = StrToNullableString(R3POLineNumber.Text.Trim())
        DstPOInformation.POUserID = CInt(POUser.SelectedValue)
        DstPOInformation.DeliveryDate = GetDatabaseTime(DeliveryDate.Text.Trim())
        DstPOInformation.PurposeCode = StrToNullableString(st_PurposeCode)
        DstPOInformation.OrderQuantity = CDec(OrderQuantity.Text)
        DstPOInformation.OrderUnitCode = StrToNullableString(st_OrderUnitCode)
        DstPOInformation.SupplierCode = CInt(st_SupplierCode)

        'フォーム右段
        DstPOInformation.DueDate = GetDatabaseTime(DueDate.Text.Trim())
        DstPOInformation.GoodsArrivedDate = GetDatabaseTime(GoodsArrivedDate.Text.Trim())
        DstPOInformation.LotNumber = StrToNullableString(LotNumber.Text.Trim())
        DstPOInformation.InvoiceReceivedDate = GetDatabaseTime(InvoceReceivedDate.Text.Trim())
        DstPOInformation.ImportCustomClearanceDate = GetDatabaseTime(ImportCustomClearanceDate.Text.Trim())
        DstPOInformation.QMStartingDate = GetDatabaseTime(QMStartingDate.Text.Trim())
        DstPOInformation.QMFinishDate = GetDatabaseTime(QMFinishDate.Text.Trim())
        DstPOInformation.QMResult = StrToNullableString(QMResult.Text.Trim())
        DstPOInformation.RequestQuantity = StrToNullableString(RequestQuantity.Text.Trim())
        DstPOInformation.ScheduledExportDate = GetDatabaseTime(ScheduledExportDate.Text.Trim())
        DstPOInformation.PurchasingRequisitionNumber = StrToNullableString(PurchasingRequisitionNumber.Text.Trim())
        DstPOInformation.CancellationDate = GetDatabaseTime(CancellationDate.Text.Trim())

        UpdatePOInfomation(SrcPOInformation, DstPOInformation)

    End Sub

    ''' <summary>
    ''' Update,Cancel時のパラメータ、設定値の正当性チェックを行います。
    ''' </summary>
    ''' <returns>正当なときはTrue 不正なときはFalseを返します。</returns>
    ''' <remarks></remarks>
    Private Function ValidateForUpdate() As Boolean

        If GetByteCount_SJIS(R3PONumber.Text) > MAX_LENGTH_R3_PO_NUMBER Then
            Msg.Text = String.Format(ERR_LENGTH_OVER, "R/3 PO Number", MAX_LENGTH_R3_PO_NUMBER)
            Return False
        End If

        If GetByteCount_SJIS(R3POLineNumber.Text) > MAX_LENGTH_R3_PO_LINE_NUMBER Then
            Msg.Text = String.Format(ERR_LENGTH_OVER, "R/3 PO Line Number", MAX_LENGTH_R3_PO_LINE_NUMBER)
            Return False
        End If

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

        If GetByteCount_SJIS(LotNumber.Text) > MAX_LENGTH_LOT_NUMBER Then
            Msg.Text = String.Format(ERR_LENGTH_OVER, "TCI Lot Number", MAX_LENGTH_LOT_NUMBER)
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

        If Not ValidateDateTextBox(QMStartingDate) Then
            Msg.Text = "QM Starting Date" & ERR_INCORRECT_FORMAT
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
    ''' 半角が期待されるフィールドの入力値を半角に変換します。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ChangeTextBoxValueToSingleByte()

        R3PONumber.Text = StrConv(R3PONumber.Text, VbStrConv.Narrow)
        R3POLineNumber.Text = StrConv(R3POLineNumber.Text, VbStrConv.Narrow)
        LotNumber.Text = StrConv(LotNumber.Text, VbStrConv.Narrow)

    End Sub

    ''' <summary>
    ''' Update,Cancel時のパラメータ、設定値の正当性チェックを行います。
    ''' </summary>
    ''' <returns>正当なときはTrue 不正なときはFalseを返します。</returns>
    ''' <remarks></remarks>
    Private Function ValidateCommon() As Boolean

        If IsInteger(st_PONumber) = False Then
            Msg.Text = ERR_INVALID_PARAMETER
            Return False
        End If

        Dim i_PONumber As Integer = Integer.Parse(st_PONumber)
        Dim i_ParPONumber As Integer = i_PONumber

        Dim POInformation As POInformationType = SelectPOInformation(i_PONumber)
        If POInformation.PONumber Is Nothing Then
            Msg.Text = ERR_DELETED_BY_ANOTHER_USER
            Return False
        End If

        If CBool(Session(SESSION_KEY_ADMIN)) = False And POInformation.POLocationCode <> Session(SESSION_KEY_LOCATION).ToString() Then
            Msg.Text = ERR_LOCATION_INCONSISTENT
            Return False
        End If

        If Not POInformation.ParPONumber Is Nothing Then
            i_ParPONumber = CInt(POInformation.ParPONumber)
        End If

        ' 変更前の PO-User 宛てに未処理のコレポンがある場合は、PO-User を変更できない
        If POInformation.POUserID <> CInt(POUser.SelectedValue) Then
            If UntreatedCorrespondence(i_ParPONumber, CInt(POInformation.POUserID)) = True Then
                Msg.Text = ERR_UNTREATED_CORRESPONDENCE
                Return False
            End If
        End If

        If IsLatestData(TABLE_NAME_PO, PK_NAME_PO, i_PONumber.ToString(), UpdateDate.Value) = False Then
            Msg.Text = ERR_UPDATED_BY_ANOTHER_USER
            Return False
        End If

        OrderQuantity.Text = OrderQuantity.Text.Trim
        If OrderQuantity.Text = String.Empty Then
            Msg.Text = "Order Quantity" & ERR_REQUIRED_FIELD
            Return False
        Else
            If Not Regex.IsMatch(OrderQuantity.Text, DECIMAL_7_3_REGEX) Then
                Msg.Text = "Order Quantity" & ERR_INVALID_NUMBER
                Return False
            End If
        End If

        Return True

    End Function

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
    ''' 指定したサプライヤーに設定されているLocationCodeを取得します。
    ''' </summary>
    ''' <param name="supplierCode">対象となるサプライヤーの一意コード</param>
    ''' <returns>LocationCodeを返します。存在しないときは空文字列を返します。</returns>
    ''' <remarks></remarks>
    Private Function GetSuppliers_Location(ByVal supplierCode As Integer?) As String

        If supplierCode Is Nothing Then
            Return String.Empty
        End If

        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = New SqlCommand("SELECT LocationCode FROM Supplier WHERE SupplierCode = @SupplierCode", conn)
            cmd.Parameters.AddWithValue("SupplierCode", supplierCode)

            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.Read() Then
                If DBObjToString(dr("LocationCode")) <> String.Empty Then
                    Return DBObjToString(dr("LocationCode"))
                End If
            End If
            Return String.Empty

        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try

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
        Return Common.GetLocalTime(Session(SESSION_KEY_LOCATION).ToString(), CType(DataBaseTime, Date), False, False)
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
    Private Function GetDBCurrentTime() As DateTime

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
                PoInformation.Confidential = IIf(CBool(dr("isCONFIDENTIAL")), Common.CONFIDENTIAL, String.Empty).ToString
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
                'PoInformation.isCancelled = DBObjToNullableBoolean(dr("isCancelled"))
                PoInformation.CancellationDate = DBObjToNullableDateTime(dr("CancellationDate"))
                PoInformation.RFQNumber = DBObjToNullableInt(dr("RFQNumber"))
                PoInformation.RFQLineNumber = DBObjToNullableInt(dr("RFQLineNumber"))
                PoInformation.ParPONumber = DBObjToNullableInt(dr("ParPONumber"))
                PoInformation.Priority = dr("Priority").ToString()
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
        'sb_SQL.Append("	isCancelled, ")
        sb_SQL.Append("	CancellationDate, ")
        sb_SQL.Append("	RFQNumber, ")
        sb_SQL.Append("	RFQLineNumber, ")
        sb_SQL.Append("	ParPONumber, ")
        sb_SQL.Append("	Priority, ")
        sb_SQL.Append("	StatusCode, ")
        sb_SQL.Append("	Status, ")
        sb_SQL.Append("	StatusChangeDate, ")
        sb_SQL.Append("	StatusSortOrder, ")
        sb_SQL.Append("	CreatedBy, ")
        sb_SQL.Append("	CreateDate, ")
        sb_SQL.Append("	UpdatedBy, ")
        sb_SQL.Append("	UpdateDate, ")
        sb_SQL.Append(" isCONFIDENTIAL ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("	v_PO ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("	PONumber = @PONumber")

        Return sb_SQL.ToString()
    End Function

    ''' <summary>
    ''' POテーブルのデータを更新します。
    ''' </summary>
    ''' <param name="SrcPOInformation">更新前の PO レコード</param>
    ''' <param name="DstPOInformation">更新後の PO レコード</param>
    ''' <remarks></remarks>
    Private Sub UpdatePOInfomation(ByRef SrcPOInformation As POInformationType, ByRef DstPOInformation As POInformationType)
        Dim st_Sql As String = String.Empty
        Dim conn As SqlConnection = Nothing
        Dim st_SOLocationCode As String = String.Empty
        Dim st_SOUserID As String = String.Empty
        Dim st_SupplierCode As String = String.Empty

        conn = New SqlConnection(DB_CONNECT_STRING)
        conn.Open()

        If Supplier.Visible Then
            'SupplierCodeからLocationCodeを取得する
            st_SupplierCode = Supplier.SelectedValue
            Dim sqlCmd As SqlCommand = conn.CreateCommand()
            Dim sqlAdapter As New SqlDataAdapter
            Dim ds As New DataSet
            sqlCmd = New SqlCommand(CreateSql_SelectSupplier(), conn)
            sqlAdapter.SelectCommand = sqlCmd
            sqlCmd.Parameters.Add("@SupplierCode", SqlDbType.VarChar).Value = st_SupplierCode
            sqlAdapter.Fill(ds, "Supplier")
            st_SOLocationCode = ds.Tables("Supplier").Rows(0)("LocationCode").ToString
            If st_SOLocationCode.Trim = String.Empty Then
                st_SOUserID = String.Empty
            End If
        Else
            st_SupplierCode = LabelSupplierCode.Value
            st_SOLocationCode = SOLocationCode.Value
            st_SOUserID = SOUserID.Value
        End If

        Dim trans As SqlTransaction = conn.BeginTransaction
        Try
            Dim cmd As SqlCommand = conn.CreateCommand()
            cmd.Transaction = trans

            cmd.CommandText = CreateSQLForUpdatePOInfomation()

            cmd.Parameters.AddWithValue("R3PONumber", NullableStringToDBObject(DstPOInformation.R3PONumber))
            cmd.Parameters.AddWithValue("R3POLineNumber", NullableVariableToDBObject(DstPOInformation.R3POLineNumber))
            cmd.Parameters.AddWithValue("PODate", NullableVariableToDBObject(DstPOInformation.PODate))
            cmd.Parameters.AddWithValue("POLocationCode", NullableVariableToDBObject(DstPOInformation.POLocationCode))
            cmd.Parameters.AddWithValue("POUserID", NullableVariableToDBObject(DstPOInformation.POUserID))
            cmd.Parameters.AddWithValue("SOLocationCode", NullableVariableToDBObject(st_SOLocationCode))
            cmd.Parameters.AddWithValue("SOUserID", NullableVariableToDBObject(st_SOUserID))
            cmd.Parameters.AddWithValue("ProductID", NullableVariableToDBObject(DstPOInformation.ProductID))
            cmd.Parameters.AddWithValue("SupplierCode", NullableVariableToDBObject(DstPOInformation.SupplierCode))
            cmd.Parameters.AddWithValue("MakerCode", NullableVariableToDBObject(DstPOInformation.MakerCode))
            cmd.Parameters.AddWithValue("OrderQuantity", NullableVariableToDBObject(DstPOInformation.OrderQuantity))
            cmd.Parameters.AddWithValue("OrderUnitCode", NullableVariableToDBObject(DstPOInformation.OrderUnitCode))
            cmd.Parameters.AddWithValue("DeliveryDate", NullableVariableToDBObject(DstPOInformation.DeliveryDate))
            cmd.Parameters.AddWithValue("CurrencyCode", NullableVariableToDBObject(DstPOInformation.CurrencyCode))
            cmd.Parameters.AddWithValue("UnitPrice", NullableVariableToDBObject(DstPOInformation.UnitPrice))
            cmd.Parameters.AddWithValue("PerQuantity", NullableVariableToDBObject(DstPOInformation.PerQuantity))
            cmd.Parameters.AddWithValue("PerUnitCode", NullableVariableToDBObject(DstPOInformation.PerUnitCode))
            cmd.Parameters.AddWithValue("PaymentTermCode", NullableVariableToDBObject(DstPOInformation.PaymentTermCode))
            cmd.Parameters.AddWithValue("IncotermsCode", NullableVariableToDBObject(DstPOInformation.IncotermsCode))
            cmd.Parameters.AddWithValue("DeliveryTerm", NullableVariableToDBObject(DstPOInformation.DeliveryTerm))
            cmd.Parameters.AddWithValue("PurposeCode", NullableVariableToDBObject(DstPOInformation.PurposeCode))
            cmd.Parameters.AddWithValue("RawMaterialFor", NullableVariableToDBObject(DstPOInformation.RawMaterialFor))
            cmd.Parameters.AddWithValue("RequestedBy", NullableVariableToDBObject(DstPOInformation.RequestedBy))
            cmd.Parameters.AddWithValue("SupplierItemNumber", NullableVariableToDBObject(DstPOInformation.SupplierItemNumber))
            cmd.Parameters.AddWithValue("SupplierLotNumber", NullableVariableToDBObject(DstPOInformation.SupplierLotNumber))
            cmd.Parameters.AddWithValue("DueDate", NullableVariableToDBObject(DstPOInformation.DueDate))
            cmd.Parameters.AddWithValue("GoodsArrivedDate", NullableVariableToDBObject(DstPOInformation.GoodsArrivedDate))
            cmd.Parameters.AddWithValue("LotNumber", NullableVariableToDBObject(DstPOInformation.LotNumber))
            cmd.Parameters.AddWithValue("InvoiceReceivedDate", NullableVariableToDBObject(DstPOInformation.InvoiceReceivedDate))
            cmd.Parameters.AddWithValue("ImportCustomClearanceDate", NullableVariableToDBObject(DstPOInformation.ImportCustomClearanceDate))
            cmd.Parameters.AddWithValue("QMStartingDate", NullableVariableToDBObject(DstPOInformation.QMStartingDate))
            cmd.Parameters.AddWithValue("QMFinishDate", NullableVariableToDBObject(DstPOInformation.QMFinishDate))
            cmd.Parameters.AddWithValue("QMResult", NullableVariableToDBObject(DstPOInformation.QMResult))
            cmd.Parameters.AddWithValue("RequestQuantity", NullableVariableToDBObject(DstPOInformation.RequestQuantity))
            cmd.Parameters.AddWithValue("ScheduledExportDate", NullableVariableToDBObject(DstPOInformation.ScheduledExportDate))
            cmd.Parameters.AddWithValue("PurchasingRequisitionNumber", NullableVariableToDBObject(DstPOInformation.PurchasingRequisitionNumber))
            'cmd.Parameters.AddWithValue("isCancelled", NullableVariableToDBObject(DstPOInformation.isCancelled))
            cmd.Parameters.AddWithValue("CancellationDate", NullableVariableToDBObject(DstPOInformation.CancellationDate))
            cmd.Parameters.AddWithValue("RFQLineNumber", NullableVariableToDBObject(DstPOInformation.RFQLineNumber))
            cmd.Parameters.AddWithValue("ParPONumber", NullableVariableToDBObject(DstPOInformation.ParPONumber))
            'Priority.visible = True （ログインユーザ＝POUser かつ 親 PO が存在しない）の場合のみ、Priority を更新する
            If Priority.Visible Then
                cmd.Parameters.AddWithValue("Priority", NullableVariableToDBObject(DstPOInformation.Priority))
            End If
            cmd.Parameters.AddWithValue("CreatedBy", NullableVariableToDBObject(DstPOInformation.CreatedBy))
            cmd.Parameters.AddWithValue("CreateDate", NullableVariableToDBObject(DstPOInformation.CreateDate))
            cmd.Parameters.AddWithValue("UpdatedBy", i_OperatingUserID)
            'UpdateDateフィールドはSQL内にGETDATE()を明示してあるため不要
            'cmd.Parameters.AddWithValue("UpdateDate", NullableVariableToDBObject(DstPOInformation.UpdateDate))
            cmd.Parameters.AddWithValue("PONumber", NullableVariableToDBObject(DstPOInformation.PONumber))
            cmd.ExecuteNonQuery()

            If (Not IsDBNull(DstPOInformation.ParPONumber)) And (SrcPOInformation.POUserID <> DstPOInformation.POUserID) Then
                cmd.Parameters.Clear()
                cmd.CommandText = "UPDATE PO SET SOUserID = @SOUserID, UpdatedBy = @UpdatedBy, UpdateDate = GETDATE() WHERE PONumber = @PONumber"
                cmd.Parameters.AddWithValue("SOUserID", NullableVariableToDBObject(DstPOInformation.POUserID))
                cmd.Parameters.AddWithValue("UpdatedBy", i_OperatingUserID)
                cmd.Parameters.AddWithValue("PONumber", NullableVariableToDBObject(DstPOInformation.ParPONumber))
                cmd.ExecuteNonQuery()
            End If

            If DstPOInformation.ParPONumber Is Nothing Then
                '親POの場合、子POのPurposeCodeを同じ値に更新する
                cmd.Parameters.Clear()
                cmd.CommandText = "UPDATE PO SET PurposeCode = @PurposeCode, UpdatedBy = @UpdatedBy, UpdateDate = GETDATE() WHERE ParPONumber = @PONumber"
                cmd.Parameters.AddWithValue("PurposeCode", NullableVariableToDBObject(DstPOInformation.PurposeCode))
                cmd.Parameters.AddWithValue("UpdatedBy", i_OperatingUserID)
                cmd.Parameters.AddWithValue("PONumber", NullableVariableToDBObject(DstPOInformation.PONumber))
                cmd.ExecuteNonQuery()
            End If

            trans.Commit()

        Catch exception As Exception
            trans.Rollback()
            Throw

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
        'sb_SQL.Append("	isCancelled = @isCancelled, ")
        sb_SQL.Append("	CancellationDate = @CancellationDate, ")
        sb_SQL.Append("	RFQLineNumber = @RFQLineNumber, ")
        sb_SQL.Append("	ParPONumber = @ParPONumber, ")
        'Priority.visible = True （ログインユーザ＝POUser かつ 親 PO が存在しない）の場合のみ、Priority を更新する
        If Priority.Visible Then
            sb_SQL.Append("	Priority = @Priority, ")
        End If
        sb_SQL.Append("	UpdatedBy = @UpdatedBy, ")
        sb_SQL.Append("	UpdateDate = GETDATE() ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("	PONumber = @PONumber")

        Return sb_SQL.ToString()
    End Function

    ''' <summary>
    ''' DBオブジェクトをSystem.stringに変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.object</param>
    ''' <returns>変換したSystem.string 。DBNullの場合はstring.Emptyを返します。</returns>
    Private Function DBObjToString(ByVal value As Object) As String
        Return DBObjToString(value, String.Empty)
    End Function

    ''' <summary>
    ''' DBオブジェクトをSystem.stringに変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.object</param>
    ''' <param name="defaultValue">変換対象のObjectがDBNullの場合に返される値</param>
    ''' <returns>変換したSystem.string 。DBNullの場合は引数で指定されたSystem.stringを返します。</returns>
    Private Function DBObjToString(ByVal value As Object, ByVal defaultValue As String) As String
        If Convert.IsDBNull(value) Then
            Return defaultValue
        Else
            Return value.ToString()
        End If
    End Function

    ''' <summary>
    ''' DBオブジェクトをNullable System.Int32に変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したNullable System.Int32。</returns>
    Private Shared Function DBObjToNullableInt32(ByVal value As Object) As Int32?
        If Convert.IsDBNull(value) Then
            Return Nothing
        Else
            Return Convert.ToInt32(value)
        End If
    End Function

    ''' <summary>
    ''' DBオブジェクトをNullable int(System.Int32)に変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したNullable int(System.Int32)。</returns>
    Private Function DBObjToNullableInt(ByVal value As Object) As Int32?
        Return DBObjToNullableInt32(value)
    End Function

    ''' <summary>
    ''' DBオブジェクトをNullable System.Decimalに変換します。
    ''' </summary>
    ''' <param name="value">対象となる System.Object</param>
    ''' <returns>変換したNullable System.Decimal。</returns>
    Private Function DBObjToNullableDecimal(ByVal value As Object) As Decimal?
        If Convert.IsDBNull(value) Then
            Return Nothing
        Else
            Return Convert.ToDecimal(value)
        End If
    End Function

    ''' <summary>
    ''' Nullable System.StringオブジェクトをDBNullを含んだSystem.Objectオブジェクトに変換します。
    ''' </summary>
    ''' <param name="value">対象となるSystem.String</param>
    ''' <returns>DB Nullを含んだ System Object。</returns>
    Private Function NullableStringToDBObject(ByVal value As String) As Object
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
    Private Function NullableVariableToDBObject(ByVal value As Object) As Object
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
    Private Function DBObjToNullableDateTime(ByVal value As Object) As DateTime?
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
    Private Function DBObjToNullableBoolean(ByVal value As Object) As Boolean
        If Convert.IsDBNull(value) Then
            Return Nothing
        Else
            Return Boolean.Parse(value.ToString)
        End If
    End Function

    ''' <summary>
    ''' Systemn.String内の空白をNothingに変換します。
    ''' </summary>
    ''' <param name="value">変換対象となるstring オブジェクト</param>
    ''' <returns>Nothingを含んだString文字列</returns>
    Private Function StrToNullableString(ByVal value As String) As String
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

    ''' <summary>
    ''' Nullable Integerオブジェクトを文字列に変換します。
    ''' </summary>
    ''' <param name="value">変換対象となる Integer? オブジェクト</param>
    ''' <returns>変換したstring 文字列</returns>
    Private Function NullableIntToString(ByVal value As Integer?) As String
        If value Is Nothing Then
            Return String.Empty
        End If

        Return CType(value, Integer).ToString()

    End Function

    Private Sub SetControl_SrcUser(ByVal LocationCode As String, ByVal UserID As Integer)
        Dim sb_Sql As StringBuilder = New StringBuilder

        sb_Sql.Append("SELECT ")
        sb_Sql.Append("  UserID, Name ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append("  v_User ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append("  LocationCode = @LocationCode ")
        sb_Sql.Append("  AND isDisabled = 0 ")
        sb_Sql.Append("UNION ")
        sb_Sql.Append("SELECT ")
        sb_Sql.Append("  UserID, Name ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append("  v_UserAll ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append("  UserID = @UserID ")
        sb_Sql.Append("ORDER BY ")
        sb_Sql.Append("  Name ")

        SrcUser.SelectCommand = sb_Sql.ToString
        SrcUser.SelectParameters.Clear()
        SrcUser.SelectParameters.Add("LocationCode", LocationCode)
        SrcUser.SelectParameters.Add("UserID", UserID.ToString)

    End Sub

    Private Function UntreatedCorrespondence(ByVal PONumber As Integer, ByVal UserID As Integer) As Boolean
        Dim sb_Sql As StringBuilder = New StringBuilder

        sb_Sql.Append("SELECT 1 ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append("  v_POReminder ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append("  PONumber = @PONumber ")
        sb_Sql.Append("  AND RcptUserID = @RcptUserID ")

        Dim sqlConn As SqlConnection = Nothing
        Try
            sqlConn = New SqlConnection(DB_CONNECT_STRING)
            Dim sqlCmd As New SqlCommand(sb_Sql.ToString, sqlConn)

            sqlCmd.Parameters.AddWithValue("PONumber", PONumber)
            sqlCmd.Parameters.AddWithValue("RcptUserID", UserID)
            sqlConn.Open()

            Dim obj_Return As Object = sqlCmd.ExecuteScalar()

            If obj_Return Is Nothing Then
                Return False
            End If

            Return True

        Finally
            If Not (sqlConn Is Nothing) Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If

        End Try

    End Function


    ''' <summary>
    ''' SupplierのLocationCodeを取得する
    ''' </summary>
    ''' <remarks></remarks>
    Private Function CreateSql_SelectSupplier() As String

        Return "SELECT LocationCode FROM Supplier WHERE SupplierCode = @SupplierCode"

    End Function

    ''' <summary>
    ''' 親POのPurposeを取得する。
    ''' </summary>
    ''' <param name="st_ParPONumber">親 PONumber</param>
    ''' <return>Priority</return>
    ''' <remarks></remarks>
    Private Sub GetParPOPurpose(ByVal st_ParPONumber As String, ByRef st_PurposeCode As String, ByRef st_PurposeText As String)

        'If String.IsNullOrEmpty(st_ParPONumber) Then
        '    Return String.Empty
        'End If

        Dim sqlConn As SqlConnection = Nothing

        Dim sb_Sql As StringBuilder = New StringBuilder

        sb_Sql.Append("SELECT ")
        sb_Sql.Append(" PurposeCode ")
        sb_Sql.Append(", PurposeText ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append(" v_PO ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append(" PONumber = @PONumber ")

        Try
            sqlConn = New SqlConnection(DB_CONNECT_STRING)

            Dim sqlCmd As New SqlCommand(sb_Sql.ToString(), sqlConn)
            sqlCmd.Parameters.AddWithValue("PONumber", st_ParPONumber)
            sqlConn.Open()

            'Dim obj_Return As Object = sqlCmd.ExecuteScalar()
            Dim dr As SqlDataReader = sqlCmd.ExecuteReader()
            If dr.Read() Then
                st_PurposeCode = DBObjToString(dr("PurposeCode"))
                st_PurposeText = DBObjToString(dr("PurposeText"))
            End If

            ''If obj_Return Is Nothing Then
            ''    Return String.Empty
            ''End If

            ''Return obj_Return.ToString()

        Finally

            If Not (sqlConn Is Nothing) Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If

        End Try

    End Sub


    ''' <summary>
    ''' v_RFQLineからQuoLocationCodeを取得します。
    ''' </summary>
    ''' <param name="RFQLineNumber">対象となるRFQLineNumber</param>
    ''' <param name="QUOLocationCode">取得したQUOLocationCode</param>
    ''' <param name="SupplierCode">取得したSupplierCode</param>
    ''' <remarks></remarks>
    Private Sub GetQuoLocationCode(ByVal RFQLineNumber As Integer?, ByRef QUOLocationCode As String, ByRef SupplierCode As String)

        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim sSQL As String = "SELECT QuoLocationCode,SupplierCode FROM v_RFQLine WHERE RFQLineNumber = @RFQLineNumber"
            Dim cmd As SqlCommand = New SqlCommand(sSQL, conn)
            cmd.Parameters.AddWithValue("RFQLineNumber", RFQLineNumber)
            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.Read() Then
                SupplierCode = DBObjToString(dr("SupplierCode"))
                QUOLocationCode = DBObjToString(dr("QuoLocationCode"))
            End If
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If

        End Try

    End Sub
End Class
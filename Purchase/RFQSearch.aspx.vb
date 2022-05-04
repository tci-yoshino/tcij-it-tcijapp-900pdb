Option Explicit On
Option Infer Off
Option Strict On
Imports System.Data.SqlClient
Imports Purchase.Common

Public Class RFQSearch
    Inherits CommonPage
    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Private DBCommand As SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader

    '数値型のチェックで使用するエラーメッセージ
    Private Const ERR_INCORRECT_RHQNUMMBER As String = "RFQ Reference Number" & ERR_INVALID_NUMBER
    Private Const ERR_INCORRECT_SUPPLIERCODE As String = "Supplier Code" & ERR_INVALID_NUMBER
    Private Const ERR_INCORRECT_SAPSUPPLIERCODE As String = "SAP Supplier Code" & ERR_INVALID_NUMBER

    '検索最小日付
    Const MinDate As String = "1900-01-01"

    ''' <summary> 他画面から戻った場合の遷移前の表示ページインデックス </summary>
    Private _CurrentPageIndexAtReturn As Integer = 0

    ' ViewStateの名称
    Private Const COLNAME_RFQSEARCH As String = "RFQSearch"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        
        If IsPostBack = False Then
            SearchMode.Value = "basic"
            
            '初期表示時は検索結果表示欄を非表示
            ResultArea.Visible = False

            'ページサイズ設定
            SearchResultList.PageSize = Common.LIST_ONEPAGE_ROW(Request.Url.ToString())

            'EnqLocationCode,QuoLocationCodeのプルダウンリストの設定
            SetLocationDropDownList(EnqLocationCode)
            SetLocationDropDownList(QuoLocationCode)

            'ドロップダウンリストの設定
            'StatusFrom
            SetRFQStatusDropDownList(StatusFrom)
            StatusFrom.SelectedValue = Common.RFQSTATUS_ALL
            'StatusTo
            SetRFQStatusDropDownList(StatusTo)
            StatusTo.SelectedValue = Common.RFQSTATUS_ALL
            'Priority
            SetPriorityDropDownList(Priority, "SEARCH")
            'SupplierCountryCode
            Dim v_CountryList As TCIDataAccess.Join.v_CountryList = New TCIDataAccess.Join.v_CountryList
            v_CountryList.Setv_CountryDropDownList(SupplierCountryCode)
            
            'Purpose
            Dim PL_PurposeList As TCIDataAccess.PurposeList = New TCIDataAccess.PurposeList
            PL_PurposeList.SetPurposeDropDownList(PurposeList)
            'Territory
            SetTerritoryDropDownList(TerritoryList)

            'ValidQuotation
            SetValidQuotationList(ValidQuotation, String.Empty)
            ValidQuotation.SelectedValue = String.Empty

        End If
    End Sub

    'EnqUserIDの値設定
    Protected Sub EnqLocationCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles EnqLocationCode.SelectedIndexChanged
        Dim User As TCIDataAccess.Join.v_UserAllList = New TCIDataAccess.Join.v_UserAllList
        User.SetEnqUserDropDownList(EnqUserID,EnqLocationCode.SelectedValue)
    End Sub

    'EnqStorageLocationの値設定
    Protected Sub EnqUserID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles EnqUserID.SelectedIndexChanged
        Dim StorageByPurchasingUserList As TCIDataAccess.StorageByPurchasingUserList = New TCIDataAccess.StorageByPurchasingUserList
        StorageByPurchasingUserList.SetStorageDropDownList(EnqStorageLocation,ConvertStringToInt(EnqUserID.SelectedValue))
    End Sub

    'QuoUserIDの値設定
    Protected Sub QuoLocationCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles QuoLocationCode.SelectedIndexChanged
        Dim User As TCIDataAccess.Join.v_UserAllList = New TCIDataAccess.Join.v_UserAllList
        User.SetQuoUserDropDownList(QuoUserID,QuoLocationCode.SelectedValue)
    End Sub

    'QuoStorageLocationの値設定
    Protected Sub QuoUserID_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles QuoUserID.SelectedIndexChanged
        Dim StorageByPurchasingUserList As TCIDataAccess.StorageByPurchasingUserList = New TCIDataAccess.StorageByPurchasingUserList
        StorageByPurchasingUserList.SetStorageDropDownList(QuoStorageLocation,ConvertStringToInt(QuoUserID.SelectedValue))
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        Msg.Text = String.Empty

        '[入力データを1Byte形式に変換する]------------------------------------------------------
        RFQNumber.Text = Trim(StrConv(RFQNumber.Text,VbStrConv.Narrow))
        ProductNumber.Text = Trim(StrConv(ProductNumber.Text,VbStrConv.Narrow))
        SupplierCode.Text = Trim(StrConv(SupplierCode.Text, VbStrConv.Narrow))
        S4SupplierCode.Text = Trim(StrConv(S4SupplierCode.Text, VbStrConv.Narrow))
        RFQCreatedDateFrom.Text = Trim(StrConv(RFQCreatedDateFrom.Text, VbStrConv.Narrow))
        RFQCreatedDateTo.Text = Trim(StrConv(RFQCreatedDateTo.Text, VbStrConv.Narrow))
        RFQQuotedDateFrom.Text = Trim(StrConv(RFQQuotedDateFrom.Text, VbStrConv.Narrow))
        RFQQuotedDateTo.Text = Trim(StrConv(RFQQuotedDateTo.Text, VbStrConv.Narrow))
        LastRFQStatusChangeDateFrom.Text = Trim(StrConv(LastRFQStatusChangeDateFrom.Text, VbStrConv.Narrow))
        LastRFQStatusChangeDateTo.Text = Trim(StrConv(LastRFQStatusChangeDateTo.Text, VbStrConv.Narrow))

        ' 検索条件が一つも設定されていない場合、処理を中断しエラーメッセージを表示する。
        If IsAllConditionsNotSet() Then
            Msg.Text = ERR_NO_MATCH_FOUND
            Exit Sub
        End If

        Msg.Text = CheckBeforeSearch()
        
        ' エラーメッセージが設定されている場合は処理を中断する
        IF Not String.IsNullOrEmpty(Msg.Text)
            Exit Sub
        End If

        '検索条件を格納
        Dim cond As New TCIDataAccess.Join.KeywordSearchConditionParameter
        SetCondition(cond)

        '検索条件を保持しておく
        SetConditionToViewState(cond)
        '----------------------------------------
        ' 一覧表示
        '----------------------------------------
        Msg.Text = SetListData(_CurrentPageIndexAtReturn, (SearchResultList.PageSize * _CurrentPageIndexAtReturn))
        ' エラーメッセージが設定されている場合は処理を中断する
        IF Not String.IsNullOrEmpty(Msg.Text)
            Exit Sub
        End If

        ResultArea.Visible = True

    End Sub
    ''' <summary>
    ''' ページャー クリック時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub SearchResultList_Paging(ByVal sender As System.Object, ByVal e As PagingEventArgs) Handles SearchResultList.Paging
        Msg.Text = SetListData(e.NewCurrentPageIndex, e.NewSkipRecord)
    End Sub
    'Clearボタン押下時、テキストボックス・ドロップダウンリスト・チェックボックスをクリアまたは未選択の状態にする
    Protected Sub Release_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Release.Click
        Msg.Text = String.Empty
        SupplierCountryCode.SelectedIndex = 0
        StatusFrom.SelectedValue = Common.RFQSTATUS_ALL
        StatusTo.SelectedValue = Common.RFQSTATUS_ALL
        EnqLocationCode.SelectedIndex = 0
        EnqUserID.Items.Clear()
        EnqStorageLocation.Items.Clear()
        QuoLocationCode.SelectedIndex = 0
        QuoUserID.Items.Clear()
        QuoStorageLocation.Items.Clear()
        PurposeList.SelectedIndex = -1
        TerritoryList.SelectedIndex = -1
        Priority.SelectedIndex = 0
        ValidQuotation.SelectedIndex = 0
        ResultArea.Visible = False
    End Sub

    ''' <summary>
    ''' 数値型の項目の入力値チェックを行います。
    ''' </summary>
    ''' <param name="MultipleItem">複数値項目のテキストボックス</param>
    ''' <returns>st_ErrMsg：空白の場合はエラーなし</returns>
    ''' <remarks></remarks>
    Private Function CheckValueType(Byval MultipleItem As System.Web.UI.WebControls.TextBox) As Boolean
        Dim st_MultipleItem() As String = Split(MultipleItem.Text, "|")
        Dim i_MultipleItemLength As Integer = st_MultipleItem.Length
        Dim i_Count As Integer = 0
        Dim i_CheckInteger As Integer
        Dim  bl_isErrg As Boolean = False

        '値が複数設定される可能性がある項目のIDのセット
        Dim st_RequestValue As String = st_MultipleItem(i_Count)  'DBに格納されているデータは半角のため、画面で全角文字列で入力されていた場合、文字列を半角文字列に変換

        'DBに数値型で登録されている値の入力チェック
        While Not String.IsNullOrEmpty(st_RequestValue)
            If Not Integer.TryParse(st_RequestValue, i_CheckInteger) And Not String.IsNullOrEmpty(st_RequestValue.Trim) Then
                bl_isErrg = True
                Exit While
            End If

            i_Count = i_Count + 1

            If i_MultipleItemLength = i_Count Then
                Exit While
            Else
                st_RequestValue = st_MultipleItem(i_Count)
            End If

        End While

        Return bl_isErrg

    End Function

    ''' <summary>
    ''' 日付のチェックを行います。
    ''' </summary>
    ''' <param name="ItemValueFrom">入力された日付(From)の値</param>
    ''' <param name="ItemValueTo">入力された日付(To)の値</param>
    ''' <param name="ItemName">入力された日付の項目名</param>
    ''' <returns>st_ErrMsg：空白の場合はエラーなし</returns>
    ''' <remarks></remarks>
    Private Function CheckDate(ByVal ItemValueFrom As String, ByVal ItemValueTo As String, ByVal ItemName As String) As String
        Dim st_ErrMsg As String = ""

        '日付妥当性チェック
        If Not String.IsNullOrEmpty(ItemValueFrom) And Not (IsDate(ItemValueFrom) And Regex.IsMatch(ItemValueFrom, DATE_REGEX_OPTIONAL)) Then
            st_ErrMsg = ItemName & " (from)" & ERR_INVALID_DATE
            Return st_ErrMsg
            Exit Function
        End If
        If Not String.IsNullOrEmpty(ItemValueTo) And Not (IsDate(ItemValueTo) And Regex.IsMatch(ItemValueTo, DATE_REGEX_OPTIONAL)) Then
            st_ErrMsg = ItemName & " (to)" & ERR_INVALID_DATE
            Return st_ErrMsg
            Exit Function
        End If

        Return st_ErrMsg

    End Function

    ''' <summary>
    ''' Exelダウンロードボタンクリック時
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub ExcelDownload_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Download.Click
        Msg.Text = String.Empty
        ' 検索条件が一つも設定されていない場合、処理を中断しエラーメッセージを表示する。
        If IsAllConditionsNotSet() Then
            Msg.Text = ERR_NO_MATCH_FOUND
            Exit Sub
        End If

        Dim rept As New Report_RFQSearch(Response)

        ResultArea.Visible = False
        Msg.Text = CheckBeforeSearch()
        
        ' エラーメッセージが設定されている場合は処理を中断する
        IF Not String.IsNullOrEmpty(Msg.Text)
            Exit Sub
        End If

        '検索時の条件を設定する
        Dim cond As New TCIDataAccess.Join.KeywordSearchConditionParameter
        setCondition(cond)

        rept.DownloadExcel(cond)

    End Sub
    
    ''' <summary>
    ''' 一覧表示
    ''' </summary>
    ''' <param name="CurrentPageIndex">表示するカレントページインデックス</param>
    ''' <param name="SkipRecord">スキップするレコード数</param>
    ''' <returns>st_ErrMsg：空白の場合はエラーなし</returns>
    ''' <remarks></remarks>
    Private Function SetListData(ByVal CurrentPageIndex As Integer, ByVal SkipRecord As Integer) As String
        Dim st_ErrMsg As String = String.Empty
        '検索時の条件を設定する
        Dim cond As New TCIDataAccess.Join.KeywordSearchConditionParameter
        GetConditionFromViewState(cond)

        Dim RFQHeaderList As New TCIDataAccess.Join.RFQHeaderList()
        Dim i_TotalDataCount As Integer = RFQHeaderList.Load(SkipRecord, SearchResultList.PageSize, cond)

        If i_TotalDataCount > 1000 Then
            st_ErrMsg = Common.MSG_RESULT_OVER_1000
            ResultArea.Visible = False
            Return st_ErrMsg
        End If

        If CurrentPageIndex > 0 AndAlso i_TotalDataCount > 0 AndAlso RFQHeaderList.Count = 0 Then
            '表示対象のページがない場合、1つ前のページを表示
            CurrentPageIndex -= 1
            SkipRecord -= SearchResultList.PageSize
            i_TotalDataCount = RFQHeaderList.Load(SkipRecord, SearchResultList.PageSize, cond)
        End If

        SearchResultList.SearchResultBind(RFQHeaderList, CurrentPageIndex, i_TotalDataCount)

        '検索条件を Session に格納
        cond.CurrentPageIndex = CurrentPageIndex     '現在のページをセット

        Return st_ErrMsg
    End Function

    ''' <summary>
    ''' 検索条件のViewStateへの取り出し
    ''' </summary>
    ''' <param name="cond">KeywordSearchConditionParameter</param>
    ''' <remarks></remarks>
    Private Sub GetConditionFromViewState(ByVal cond As TCIDataAccess.join.KeywordSearchConditionParameter)

        'KeywordSearchConditionParameter クラスのフィールド一覧を取得
        Dim ClassFields As System.Reflection.FieldInfo() = cond.GetType().GetFields()

        ' ViewState から取り出す  (DataTable → KeywordSearchConditionParameter  へ変換)
        Dim dt As DataTable = Me.SearchCondition
        If dt.Rows.Count > 0 Then
            Dim dr As DataRow = dt.Rows(0)
            For Each fld As System.Reflection.FieldInfo In ClassFields
                If Not fld.Name.Equals(COLNAME_RFQSEARCH) Then
                    fld.SetValue(cond, dr(fld.Name))
                End If
            Next
        End If
    End Sub
    ''' <summary>
    ''' 検索条件のViewStateへの保持
    ''' </summary>
    ''' <param name="cond">KeywordSearchConditionParameter</param>
    ''' <remarks></remarks>
    Private Sub SetConditionToViewState(ByVal cond As TCIDataAccess.join.KeywordSearchConditionParameter)

        'KeywordSearchConditionParameter クラスのフィールド一覧を取得
        Dim ClassFields As System.Reflection.FieldInfo() = cond.GetType().GetFields()

        ' ViewState へ保存 (KeywordSearchConditionParameter → DataTable へ変換)

        '格納用 DataTable 作成
        Dim dt As New DataTable
        For Each fld As System.Reflection.FieldInfo In ClassFields
            If Not fld.Name.Equals(COLNAME_RFQSEARCH) Then
                dt.Columns.Add(fld.Name, fld.FieldType)
            End If
        Next
        '値格納
        Dim dr As DataRow = dt.NewRow()
        For Each fld As System.Reflection.FieldInfo In ClassFields
            If Not fld.Name.Equals(COLNAME_RFQSEARCH) Then
                dr(fld.Name) = fld.GetValue(cond)
            End If
        Next
        dt.Rows.Add(dr)
        Me.SearchCondition = dt

    End Sub
     ''' <summary>
    ''' 検索条件の設定
    ''' </summary>
    ''' <remarks></remarks>
    ''' <returns>True:全ての検索条件未設定,False:いずれかの検索条件が設定済</returns>
    Private function IsAllConditionsNotSet() As Boolean
        Dim bl_IsAllConditionsNotSet As Boolean = True
        If Not String.IsNullOrEmpty(RFQNumber.Text) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(ProductNumber.Text)Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(ProductName.Text) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(SupplierCode.Text) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(S4SupplierCode.Text)Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(SupplierName.Text) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(SupplierCountryCode.SelectedValue)Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(SupplierItemName.Text) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(StatusFrom.SelectedValue) And StatusFrom.SelectedValue <> Common.RFQSTATUS_ALL Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(StatusTo.SelectedValue) And StatusTo.SelectedValue <> Common.RFQSTATUS_ALL Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(RFQCreatedDateFrom.Text) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(RFQCreatedDateTo.Text)Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(RFQQuotedDateFrom.Text) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(RFQQuotedDateTo.Text) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(LastRFQStatusChangeDateFrom.Text) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(LastRFQStatusChangeDateTo.Text) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(EnqLocationCode.SelectedValue)Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(EnqUserID.SelectedValue) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(QuoLocationCode.SelectedValue) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(QuoUserID.SelectedValue) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If IsCheckedMultipleSelectionItems(PurposeList.Items) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If IsCheckedMultipleSelectionItems(TerritoryList.Items) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(Priority.SelectedValue)Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        If Not String.IsNullOrEmpty(ValidQuotation.SelectedValue) Then
            bl_IsAllConditionsNotSet = False
            Return bl_IsAllConditionsNotSet
        End If
        Return bl_IsAllConditionsNotSet
    End function
    ''' <summary>
    ''' 複数選択可能なドロップダウンリストが選択されているかを判定します。
    ''' </summary>
    ''' <param name="ListItems">複数選択可能なドロップダウンリスト</param>
    ''' <returns>True:選択有り,False:選択無し</returns>
    ''' <remarks></remarks>
    Private Function IsCheckedMultipleSelectionItems(ByVal ListItems As ListItemCollection) As Boolean
        Dim bl_IsCheckedMultipleSelectionItems As Boolean =False
        For Each ListItem As ListItem In ListItems
            'CheckboxListのチェックON判定
            If ListItem.Selected Then
                bl_IsCheckedMultipleSelectionItems = True
                Exit For
            End If
        Next
        Return bl_IsCheckedMultipleSelectionItems
    End Function
    ''' <summary>
    ''' 検索条件の設定
    ''' </summary>
    ''' <param name="cond">KeywordSearchConditionParameter</param>
    ''' <remarks></remarks>
    Private Sub SetCondition(ByRef cond As TCIDataAccess.join.KeywordSearchConditionParameter)
        ' 検索条件を初期化
        cond = New TCIDataAccess.Join.KeywordSearchConditionParameter

        ' 各検索条件の設定
        Dim s_LocationCode As String = Session("LocationCode").ToString()
        cond.s_LocationCode = s_LocationCode
        Dim s_RoloCode As String = Session(SESSION_ROLE_CODE).ToString
        cond.s_RoleCode = s_RoloCode
        cond.RFQNumber = SplitMultipleListItems(RFQNumber)
        cond.ProductNumber = SplitMultipleListItems(ProductNumber)
        cond.ProductName = ProductName.Text
        cond.SupplierCode = SplitMultipleListItems(SupplierCode)
        cond.S4SupplierCode = SplitMultipleListItems(S4SupplierCode)
        cond.SupplierName = SupplierName.Text
        cond.SupplierCountryCode = SupplierCountryCode.SelectedValue
        cond.SupplierItemName = SupplierItemName.Text
        Dim RFQStatus As TCIDataAccess.RFQStatus = New TCIDataAccess.RFQStatus
        If Not String.IsNullOrEmpty(StatusFrom.SelectedValue) And StatusFrom.SelectedValue <> Common.RFQSTATUS_ALL Then
            RFQStatus.Load(StatusFrom.SelectedValue)
            cond.StatusFrom = RFQStatus.SortOrder.ToString
        End If
        If Not String.IsNullOrEmpty(StatusTo.SelectedValue) And StatusFrom.SelectedValue <> Common.RFQSTATUS_ALL Then
            RFQStatus.Load(StatusTo.SelectedValue.ToString())
            cond.StatusTo = RFQStatus.SortOrder.ToString
        End If
        If IsDate(RFQCreatedDateFrom.Text) Then
                cond.RFQCreatedDateFrom = RFQCreatedDateFrom.Text
        End If
        If IsDate(RFQCreatedDateTo.Text) Then
                cond.RFQCreatedDateTo = RFQCreatedDateTo.Text
        End If
        If IsDate(RFQQuotedDateFrom.Text) Then
                cond.RFQQuotedDateFrom = RFQQuotedDateFrom.Text
        End If
        If IsDate(RFQQuotedDateTo.Text) Then
                cond.RFQQuotedDateTo = RFQQuotedDateTo.Text
        End If
        If IsDate(LastRFQStatusChangeDateFrom.Text) Then
                cond.LastRFQStatusChangeDateFrom = LastRFQStatusChangeDateFrom.Text
        End If
        If IsDate(LastRFQStatusChangeDateTo.Text) Then
                cond.LastRFQStatusChangeDateTo = LastRFQStatusChangeDateTo.Text
        End If
        cond.EnqLocationCode = EnqLocationCode.SelectedValue
        cond.EnqUserID = EnqUserID.SelectedValue
        cond.EnqStorageLocation = EnqStorageLocation.SelectedValue
        cond.QuoLocationCode = QuoLocationCode.SelectedValue
        cond.QuoUserID = QuoUserID.SelectedValue
        cond.QuoStorageLocation = QuoStorageLocation.SelectedValue
        cond.Purpose = PurposeList.Items
        cond.Territory = TerritoryList.Items
        cond.Priority = Priority.SelectedValue
        cond.ValidQuotation = ValidQuotation.SelectedValue

    End Sub

    ''' <summary>
    ''' 検索条件　プロパティ
    ''' </summary>
    Public Property SearchCondition() As DataTable
        Get
            Dim cond As New DataTable
            If ViewState("SearchCondition") IsNot Nothing Then
                cond = DirectCast(ViewState("SearchCondition"), DataTable)
            End If
            Return cond
        End Get
        Set(ByVal value As DataTable)
            ViewState("SearchCondition") = value
        End Set
    End Property
    ''' <summary>
    ''' 複数値項目を配列に収める
    ''' </summary>
    ''' <param name="MultipleItem">複数値項目のテキストボックス</param>
    ''' <returns></returns>
    Private Function SplitMultipleListItems(Byval MultipleItem As System.Web.UI.WebControls.TextBox) As String()
        Dim ar_MultipleItem() As String = Split(MultipleItem.Text, "|")
        Dim i_Count As Integer = 0
        Dim ar_ResultMultipleItem(ar_MultipleItem.Length) As String

        For Each st_RFQNumber As String In ar_MultipleItem
            ar_ResultMultipleItem(i_Count) = st_RFQNumber
            i_Count = i_Count + 1
        Next
        Return ar_ResultMultipleItem
    End Function

    ''' <summary>
    ''' 検索前チェック
    ''' </summary>
    ''' <returns>エラーメッセージ</returns>
    Private function CheckBeforeSearch() As String
        Dim ErrMessage As String = String.Empty
        'ProductNumberのみDBに登録されている英単語が大文字のため、大文字に変換
        If (Not String.IsNullOrEmpty(ProductNumber.Text)) Then
            ProductNumber.Text = UCase(ProductNumber.Text)
        End If

        '画面に表示するエラーメッセージを設定
        'RFQNumber
        If CheckValueType(RFQNumber) Then
            ErrMessage = ERR_INCORRECT_RHQNUMMBER
            Return ErrMessage
        End If

        'SupplierCode
        If CheckValueType(SupplierCode) Then
            ErrMessage = ERR_INCORRECT_SUPPLIERCODE
            Return ErrMessage
        End If

        'S4Supplier
        If CheckValueType(S4SupplierCode) Then
            ErrMessage = ERR_INCORRECT_SAPSUPPLIERCODE
            Return ErrMessage
        End If

        '日付の入力値チェック
        'DBに格納されているデータは半角のため、画面で全角文字列で入力されていた場合、文字列を半角文字列に変換
        Dim RFQCreatedDate As String = CheckDate(RFQCreatedDateFrom.Text, RFQCreatedDateTo.Text, "RFQ Created Date")
        Dim RFQQuotedDate As String = CheckDate(RFQQuotedDateFrom.Text, RFQQuotedDateTo.Text, "RFQ Quoted Date")
        Dim LastRFQStatusChangeDate As String = CheckDate(LastRFQStatusChangeDateFrom.Text, LastRFQStatusChangeDateTo.Text, "Last RFQ Status Change Date")

        '日付の入力値チェック時にエラーとなった場合、画面に表示するエラーメッセージを設定
        'RFQ Created Date
        If (Not String.IsNullOrEmpty(RFQCreatedDate)) Then
            ErrMessage = RFQCreatedDate
            Return ErrMessage
        End If

        'RFQ Quoted Date
        If (Not String.IsNullOrEmpty(RFQQuotedDate)) Then
            ErrMessage = RFQQuotedDate
            Return ErrMessage
        End If

        'RFQ Quoted Date
        If (Not String.IsNullOrEmpty(LastRFQStatusChangeDate)) Then
            ErrMessage = LastRFQStatusChangeDate
            Return ErrMessage
        End If

        Return ErrMessage
    End function

End Class


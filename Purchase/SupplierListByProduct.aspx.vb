Imports Purchase.Common
Imports Purchase.TCIDataAccess

Partial Public Class SupplierListByProduct
    Inherits CommonPage

    '変数定義
    Public Url As String = ""
    Public AddUrl As String = ""

    ''' <summary>
    ''' ページロードイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim SrcSupplierProduct As TCIDataAccess.Join.SupplierListByProductDispList = New TCIDataAccess.Join.SupplierListByProductDispList
        Dim TerritoryChkList As TCIDataAccess.s_LocationList = New TCIDataAccess.s_LocationList
        Dim cmnProduct As TCIDataAccess.Product = New TCIDataAccess.Product

        '初期表示判定
        If IsPostBack = False Then                                          '--初期表示の場合
            'プロダクトID判定
            If String.IsNullOrEmpty(Request.QueryString("ProductID")) Then  '--空の場合
                SupplierProductList.DataSource = String.Empty
                SupplierProductList.DataBind()
                Msg.Text = Common.ERR_INVALID_PARAMETER
                Exit Sub
            End If

            Dim st_ProductID As String = Request.QueryString("ProductID").ToString

            '呼出元情報によるヘッダ情報取得・設定
            cmnProduct.Load(st_ProductID)
            ProductNumber.Text = cmnProduct.ProductNumber.ToString
            If Not String.IsNullOrEmpty(cmnProduct.Name.ToString) Then ProductName.Text = cmnProduct.Name.ToString
            If Not String.IsNullOrEmpty(cmnProduct.QuoName.ToString) Then ProductName.Text = cmnProduct.QuoName.ToString

            HiddenSortType.Value = "asc"
            HiddenSortField.Value = "SupplierCode"

            'Territoryコンボリスト情報取得・表示
            Common.SetTerritoryDropDownList(TerritoryList, True)
            'TerritoryChkList.Load()
            'TerritoryList.DataSource = TerritoryChkList
            'TerritoryList.DataBind()

            '初期表示時はProductNumberでソートを設定する
            HiddenSortType.Value = "asc"
            'Listview内のthにを付与するとASPXにて「ListViewのID_thに設定したID」を付与されるため、明示的にListViewのIDを付与する
            HiddenSortField.Value = SupplierProductList.ID + "_ProductNumHeader"

            'Hidden項目を設定する
            ProductID.Value = Request.QueryString("ProductID")
            SupplierCode.Value = String.Empty

            'リスト情報初期表示
            SrcSupplierProduct.Load(Request.QueryString("ProductID"), TerritoryList, UpdateDateFrom.Text, UpdateDateTo.Text, _
                                    HiddenSortField, HiddenSortType, SupplierProductList.ID)
            SupplierProductList.DataSource = SrcSupplierProduct
            SupplierProductList.DataBind()
        Else
            Msg.Text = String.Empty
            'Update Date バリデーションチェック処理
            If Not String.IsNullOrEmpty(UpdateDateFrom.Text) OrElse Not String.IsNullOrEmpty(UpdateDateTo.Text) Then
                If Not UpdateDateValidateCheck() Then
                    Exit Sub
                End If
            End If
            'リスト情報初期表示
            SrcSupplierProduct.Load(Request.Form("ProductID"), TerritoryList, UpdateDateFrom.Text, UpdateDateTo.Text, _
                                    HiddenSortField, HiddenSortType, SupplierProductList.ID)
            SupplierProductList.DataSource = SrcSupplierProduct
            SupplierProductList.DataBind()
        End If

    End Sub

    ''' <summary>
    ''' ページ事前レンダリング完了時イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete

        If Request.Form("Action") = "Delete" Then
            '[指定レコード削除]-----------------------------------------------------------------
            Dim FacadeSupplierListByProduct As FacadeSupplierListByProduct = New FacadeSupplierListByProduct
            FacadeSupplierListByProduct.SupplierCode = Request.Form("SupplierCode")
            FacadeSupplierListByProduct.ProductID = Request.QueryString("ProductID")
            FacadeSupplierListByProduct.Delete
            ' リダイレクト
            Url = "./SupplierListByProduct.aspx?ProductID=" & ProductID.Value.ToString
            Response.Redirect(Url)

        End If
        '[New Suppliers ProductのURL設定]------------------------------------------------------------
        AddUrl = "./SuppliersProductSetting.aspx?Product=" + Request.QueryString("ProductID") + "&Return=SP"
    End Sub

    ''' <summary>
    ''' Searchボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Search_Click(sender As Object, e As EventArgs) Handles Search.Click
        Dim SrcSupplierProduct As TCIDataAccess.Join.SupplierListByProductDispList = New TCIDataAccess.Join.SupplierListByProductDispList

        Msg.Text = String.Empty
        'Update Date バリデーションチェック処理
        If Not String.IsNullOrEmpty(UpdateDateFrom.Text) OrElse Not String.IsNullOrEmpty(UpdateDateTo.Text) Then
            If Not UpdateDateValidateCheck() Then
                Exit Sub
            End If
        End If
        'リスト情報取得
        SrcSupplierProduct.Load(Request.QueryString("ProductID"), TerritoryList, UpdateDateFrom.Text, UpdateDateTo.Text, _
                                HiddenSortField, HiddenSortType, SupplierProductList.ID)
        SupplierProductList.DataSource = SrcSupplierProduct
        SupplierProductList.DataBind()
    End Sub

    ''' <summary>
    ''' Releaseボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Release_Click(sender As Object, e As EventArgs) Handles Release.Click
        'メッセージクリア
        Msg.Text = String.Empty
        '条件して項目：Territoryコンボ内CheckBox全クリア
        For Each TerritoryItem As ListItem In TerritoryList.Items
            TerritoryItem.Selected = False
        Next
        '条件指定項目：UpdateDate(From,To)クリア
        UpdateDateFrom.Text = String.Empty
        UpdateDateTo.Text = String.Empty
        '初期表示時はProductNumberでソートを設定する
        HiddenSortType.Value = "asc"
        'Listview内のthにを付与するとASPXにて「ListViewのID_thに設定したID」を付与されるため、明示的にListViewのIDを付与する
        HiddenSortField.Value = SupplierProductList.ID + "_SupplierCodeHeader"

    End Sub

    ''' <summary>
    ''' UpdateDate のバリデートチェックする
    ''' </summary>
    ''' <remarks>
    ''' 
    ''' </remarks>
    ''' <returns>True：チェックOK(エラー無)/False：チェックNG(エラー有)</returns>
    Private Function UpdateDateValidateCheck() As Boolean
        '[入力データを1Byte形式に変換する]------------------------------------------------------
        UpdateDateFrom.Text = StrConv(UpdateDateFrom.Text, VbStrConv.Narrow)
        UpdateDateTo.Text = StrConv(UpdateDateTo.Text, VbStrConv.Narrow)
        '[日付妥当性チェック]-------------------------------------------------------------------
        If Not String.IsNullOrEmpty(UpdateDateFrom.Text) AndAlso _
            Not (IsDate(UpdateDateFrom.Text) AndAlso Regex.IsMatch(UpdateDateFrom.Text, DATE_REGEX_OPTIONAL)) Then
            Msg.Text = "Update Date (from)" & ERR_INVALID_DATE
            Return False
        End If
        If Not String.IsNullOrEmpty(UpdateDateTo.Text) AndAlso _
            Not (IsDate(UpdateDateTo.Text) AndAlso Regex.IsMatch(UpdateDateTo.Text, DATE_REGEX_OPTIONAL)) Then
            Msg.Text = "Update Date (to)" & ERR_INVALID_DATE
            Return False
        End If
        '[日付設定順序チェック]-----------------------------------------------------------------
        If Not String.IsNullOrEmpty(UpdateDateFrom.Text) And Not String.IsNullOrEmpty(UpdateDateTo.Text) Then
            If CInt(DateTime.Parse(UpdateDateFrom.Text).CompareTo(Date.Parse(UpdateDateTo.Text))) = 1 Then
                Msg.Text = "Update Date (from) or Update Date (to)" & ERR_REQUIRED_FIELD
                Return False
            End If
        End If

        Return True

    End Function

End Class
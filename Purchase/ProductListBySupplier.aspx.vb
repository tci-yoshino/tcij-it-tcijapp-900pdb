Imports Purchase.Common
Imports Purchase.TCIDataAccess
Imports Purchase.TCIDataAccess.Join

Partial Public Class ProductListBySupplier
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader
    Public Url As String = ""
    Public AddUrl As String = ""
    Public ImpUrl As String = ""

    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <remarks>
    ''' ページを読み込む
    ''' </remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            ' 初期表示の場合
            '[QueryString("Supplier")のチェック]----------------------------------------------
            If String.IsNullOrWhiteSpace(Request.QueryString("Supplier")) Then
                Msg.Text = Common.ERR_INVALID_PARAMETER
                Exit Sub
            End If

            Dim st_SupplierCode As String = Request.QueryString("Supplier").ToString

            ' 画面表示項目ををセット
            Dim productListBySupplierDispList As ProductListBySupplierDispList = New ProductListBySupplierDispList
            productListBySupplierDispList.Load(st_SupplierCode, Session(SESSION_ROLE_CODE).ToString, HiddenSelectedValidityFilter.Value, _
                                               SupplierProductList.ID, HiddenSortField.Value, HiddenSortType.Value)
            SupplierCode.Text = st_SupplierCode
            If Not String.IsNullOrEmpty(productListBySupplierDispList.SupplierName.ToString) Then SupplierName.Text = _
                productListBySupplierDispList.SupplierName.ToString
            If Not String.IsNullOrEmpty(productListBySupplierDispList.Territory.ToString) Then Territory.Text = _
                productListBySupplierDispList.Territory.ToString

            HiddenSortType.Value = "asc"
            HiddenSortField.Value = "ProductNumHeader"
            SupplierProductList.DataSource = productListBySupplierDispList.ProductListBySupplierList
            SupplierProductList.DataBind()

            ResetPagerIndex(True)
        Else
            Dim productListBySupplierDispList As ProductListBySupplierDispList = New ProductListBySupplierDispList
            SupplierProductList.DataSource = Nothing 
            productListBySupplierDispList.Load(SupplierCode.Text, Session(SESSION_ROLE_CODE).ToString, HiddenSelectedValidityFilter.Value, _
                                               SupplierProductList.ID, HiddenSortField.Value, HiddenSortType.Value)
            SupplierProductList.DataSource = productListBySupplierDispList.ProductListBySupplierList
            SupplierProductList.DataBind()

        End If
    End Sub

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        If Request.Form("Action") = "Delete" Then

            '[指定レコード削除]-----------------------------------------------------------------
            Dim facadeProductListBySupplier As FacadeProductListBySupplier = New FacadeProductListBySupplier
            facadeProductListBySupplier.SupplierCode = Request.QueryString("Supplier")
            facadeProductListBySupplier.ProductID = Request.Form("ProductID")
            facadeProductListBySupplier.Delete
            ' リダイレクト
            Url = "./ProductListBySupplier.aspx?Supplier=" & SupplierCode.Text.ToString
            Response.Redirect(Url)

        End If

        '[New Suppliers ProductのURL設定]------------------------------------------------------------
        AddUrl = "./SuppliersProductSetting.aspx?Supplier=" & SupplierCode.Text.ToString
        '[Excel ImportのURL設定]---------------------------------------------------------------------
        ImpUrl = "./SuppliersProductImport.aspx?Supplier=" & SupplierCode.Text.ToString

    End Sub

    ''' <summary>
    ''' ページアンロード
    ''' </summary>
    ''' <remarks>
    ''' ページアンロード
    ''' </remarks>
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
    End Sub

    ''' <summary>
    ''' SupplierProductList プロパティ変更時イベントハンドラ
    ''' </summary>
    ''' <remarks>
    ''' 
    ''' </remarks>
    Protected Sub SupplierProductList_PagePropertiesChanged(ByVal sender As Object, ByVal e As EventArgs) Handles SupplierProductList.PagePropertiesChanged
        Dim productListBySupplierDispList As ProductListBySupplierDispList = New ProductListBySupplierDispList
        productListBySupplierDispList.Load(SupplierCode.Text, Session(SESSION_ROLE_CODE).ToString, HiddenSelectedValidityFilter.Value, SupplierProductList.ID, HiddenSortField.Value, HiddenSortType.Value)
        SupplierProductList.DataSource = productListBySupplierDispList.ProductListBySupplierList
        SupplierProductList.DataBind()

    End Sub

    ''' <summary>
    ''' ExcelExportBtn ボタン押下時イベントハンドラ
    ''' </summary>
    ''' <remarks>
    ''' EXCEL形式で一覧を出力する。
    ''' </remarks>
    Protected Sub ExcelExportBtn_Click(sender As Object, e As EventArgs) Handles ExcelExportBtn.Click
        Dim rept As New Report_SupplierProduct(Response)
        rept.DownloadExcel(SupplierCode.Text, SupplierName.Text, Territory.Text, Session(SESSION_ROLE_CODE).ToString, HiddenSelectedValidityFilter.Value, _
                           SupplierProductList.ID, HiddenSortField.Value, HiddenSortType.Value)

    End Sub

    ''' <summary>
    ''' HiddenSortType HiddenSortField 値変更時イベントハンドラ
    ''' </summary>
    ''' <remarks>
    ''' ページャーを１ページ目に移動
    ''' </remarks>
    Protected Sub HiddenSortTypeAndSortField_ValueChanged(sender As Object, e As EventArgs) Handles HiddenSortType.ValueChanged, HiddenSortField.ValueChanged
        ResetPagerIndex(True)
    End Sub


    ''' <summary>
    ''' 画面表示しているDatapagerのページを1ページ目にセットする
    ''' </summary>
    ''' <remarks>
    ''' 画面上部のSupplierProductPagerCountTop、SupplierProductPagerLinkTop、
    ''' 画面下部のSupplierProductPagerCountBottom、SupplierProductPagerLinkBottomに対し最初のページを設定する
    ''' </remarks>
    Protected Sub ResetPagerIndex(Optional databind As Boolean = False)
        '各Pagerに1ページ目を設定する
        Dim PgrSupplierProductPagerCountTop As DataPager
        PgrSupplierProductPagerCountTop = SupplierProductList.FindControl("SupplierProductPagerCountTop")
        PgrSupplierProductPagerCountTop.SetPageProperties(0, PgrSupplierProductPagerCountTop.MaximumRows, databind)

        Dim PgrSupplierProductPagerLinkTop As DataPager
        PgrSupplierProductPagerLinkTop = SupplierProductList.FindControl("SupplierProductPagerLinkTop")
        PgrSupplierProductPagerLinkTop.SetPageProperties(0, PgrSupplierProductPagerLinkTop.MaximumRows, databind)

        Dim PgrSupplierProductPagerCountBottom As DataPager
        PgrSupplierProductPagerCountBottom = SupplierProductList.FindControl("SupplierProductPagerCountBottom")
        PgrSupplierProductPagerCountBottom.SetPageProperties(0, PgrSupplierProductPagerCountBottom.MaximumRows, databind)

        Dim PgrSupplierProductPagerLinkBottom As DataPager
        PgrSupplierProductPagerLinkBottom = SupplierProductList.FindControl("SupplierProductPagerLinkBottom")
        PgrSupplierProductPagerLinkBottom.SetPageProperties(0, PgrSupplierProductPagerLinkBottom.MaximumRows, databind)

    End Sub

End Class


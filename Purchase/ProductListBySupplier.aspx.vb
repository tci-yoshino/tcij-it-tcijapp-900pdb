Option Explicit On
Option Strict On
Option Infer Off

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
    Protected i_ListCount As Integer

    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <remarks>
    ''' ページを読み込む
    ''' </remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        SetPageSize()

        If Not IsPostBack Then
            ' 初期表示の場合
            '[QueryString("Supplier")のチェック]----------------------------------------------
            If String.IsNullOrWhiteSpace(Request.QueryString("Supplier")) Then
                Msg.Text = Common.ERR_INVALID_PARAMETER
                Exit Sub

            End If

            Dim st_SupplierCode As String = Request.QueryString("Supplier").ToString

            '初期表示時はProductNumberの降順でソートを設定する
            HiddenSortType.Value = "desc"
            HiddenSortField.Value = SupplierProductList.ID.ToString + "_" + "ProductNumHeader"

            ' 画面表示項目ををセット
            Dim productListBySupplierDispList As ProductListBySupplierDispList = New ProductListBySupplierDispList
            productListBySupplierDispList.Load(st_SupplierCode, Session(SESSION_ROLE_CODE).ToString, HiddenSelectedValidityFilter.Value, _
                                               SupplierProductList.ID, HiddenSortField.Value, HiddenSortType.Value)
            SupplierCode.Text = st_SupplierCode
            If Not String.IsNullOrEmpty(productListBySupplierDispList.SupplierName.ToString) Then SupplierName.Text = _
                productListBySupplierDispList.SupplierName.ToString
            If Not String.IsNullOrEmpty(productListBySupplierDispList.Territory.ToString) Then Territory.Text = _
                productListBySupplierDispList.Territory.ToString

            SupplierProductList.DataSource = productListBySupplierDispList.ProductListBySupplierList
            SupplierProductList.DataBind()

            i_ListCount = productListBySupplierDispList.ProductListBySupplierList.Count

        Else
            Dim productListBySupplierDispList As ProductListBySupplierDispList = New ProductListBySupplierDispList
            SupplierProductList.DataSource = Nothing 
            productListBySupplierDispList.Load(SupplierCode.Text, Session(SESSION_ROLE_CODE).ToString, HiddenSelectedValidityFilter.Value, _
                                               SupplierProductList.ID, HiddenSortField.Value, HiddenSortType.Value)
            SupplierProductList.DataSource = productListBySupplierDispList.ProductListBySupplierList
            SupplierProductList.DataBind()

            i_ListCount = productListBySupplierDispList.ProductListBySupplierList.Count

        End If

    End Sub

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        If Request.Form("Action") = "Delete" Then

            '[指定レコード削除]-----------------------------------------------------------------
            Dim facadeProductListBySupplier As FacadeProductListBySupplier = New FacadeProductListBySupplier
            facadeProductListBySupplier.SupplierCode = Integer.Parse(Request.QueryString("Supplier"))
            facadeProductListBySupplier.ProductID = Integer.Parse(Request.Form("ProductID"))
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
    ''' ExcelExportBtn ボタン押下時イベントハンドラ
    ''' </summary>
    ''' <remarks>
    ''' EXCEL形式で一覧を出力する。
    ''' </remarks>
    Protected Sub ExcelExportBtn_Click(sender As Object, e As EventArgs) Handles ExcelExportBtn.Click
        Dim rept As New Report_SupplierProduct(Response)
        rept.DownloadExcel(SupplierCode.Text, SupplierName.Text, Territory.Text, Session(SESSION_ROLE_CODE).ToString, HiddenSelectedValidityFilter.Value, 
                           SupplierProductList.ID, Session("LocationCode").ToString, HiddenSortField.Value, HiddenSortType.Value)

    End Sub

    ''' <summary>
    ''' SupplierProductList プロパティ変更時イベントハンドラ
    ''' </summary>
    ''' <remarks>
    ''' 
    ''' </remarks>
    Protected Sub SupplierProductList_PagePropertiesChanged(ByVal sender As Object, ByVal e As EventArgs) Handles SupplierProductList.PagePropertiesChanged
        if IsPostBack Then
            ShowList()
        End If
        SetPageSize()
    End Sub

    ''' <summary>
    ''' 一覧表示
    ''' </summary>
    Protected Sub ShowList()
        Dim productListBySupplierDispList As ProductListBySupplierDispList = New ProductListBySupplierDispList
        productListBySupplierDispList.Load(SupplierCode.Text, Session(SESSION_ROLE_CODE).ToString, HiddenSelectedValidityFilter.Value,
                                           SupplierProductList.ID, HiddenSortField.Value, HiddenSortType.Value)
        SupplierProductList.DataSource = productListBySupplierDispList.ProductListBySupplierList
        SupplierProductList.DataBind()

    End Sub

    ''' <summary>
    ''' HiddenSortType HiddenSortField 値変更時イベントハンドラ
    ''' </summary>
    ''' <remarks>
    ''' ページャーを１ページ目に移動
    ''' </remarks>
    Protected Sub HiddenSortTypeAndSortField_ValueChanged(sender As Object, e As EventArgs) Handles HiddenSortType.ValueChanged, HiddenSortField.ValueChanged
        if IsPostBack Then
            ShowList()
        End If
        SetPageSize()
        ReSetPager
    End Sub

    ''' <summary>
    ''' ページサイズ設定
    ''' </summary>
    Private Sub SetPageSize()

        SupplierProductPagerCountTop.PageSize = Common.LIST_ONEPAGE_ROW(Request.Url.ToString())
        SupplierProductPagerLinkTop.PageSize = Common.LIST_ONEPAGE_ROW(Request.Url.ToString())
        SupplierProductPagerLinkBottom.PageSize = Common.LIST_ONEPAGE_ROW(Request.Url.ToString())
        SupplierProductPagerCountBottom.PageSize = Common.LIST_ONEPAGE_ROW(Request.Url.ToString())

    End Sub

    ''' <summary>
    ''' ページリセット
    ''' </summary>
    Private Sub ReSetPager()

        ResetPageNumericPagerField(SupplierProductPagerLinkTop)
        ResetPageNumericPagerField(SupplierProductPagerLinkBottom)

    End Sub

    ''' <summary>
    ''' ページを初期化します。
    ''' </summary>
    private Sub ResetPageNumericPagerField(ByVal dp As DataPager)
        If Not IsNothing(dp) And Not dp.StartRowIndex = 0 Then
            Dim numericPF As NumericPagerField = Ctype(dp.Fields(0), NumericPagerField)
            If Not IsNothing(numericPF) Then
　　　　　　　　'' 引数に0をセット
                Dim args As CommandEventArgs = New CommandEventArgs("0", "")
　　　　　　　　'' イベント発生
                numericPF.HandleEvent(args)
            End If
        End If
    End Sub

End Class


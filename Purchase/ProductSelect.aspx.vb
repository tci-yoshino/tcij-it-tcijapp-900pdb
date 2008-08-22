Public Partial Class ProductSelect
    Inherits CommonPage

    Private DBConnectString As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Private st_ProductNumber As String = ""
    Private st_CASNumber As String = ""
    Private st_ProductName As String = ""
    Const SEARCH_ACTION As String = "Search"


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' コントロール初期化
        Msg.Text = ""

        ' パラメータを取得
        If Request.RequestType = "POST" Then
            st_ProductNumber = IIf(Request.Form("ProductNumber") = Nothing, "", Request.Form("ProductNumber"))
            st_CASNumber = IIf(Request.Form("CASNumber") = Nothing, "", Request.Form("CASNumber"))
            st_ProductName = IIf(Request.Form("ProductName") = Nothing, "", Request.Form("ProductName"))
        ElseIf Request.RequestType = "GET" Then
            st_ProductNumber = IIf(Request.QueryString("ProductNumber") = Nothing, "", Request.QueryString("ProductNumber"))
            st_CASNumber = IIf(Request.QueryString("CASNumber") = Nothing, "", Request.QueryString("CASNumber"))
            st_ProductName = IIf(Request.QueryString("ProductName") = Nothing, "", Request.QueryString("ProductName"))
        End If

        ' 空白除去
        st_ProductNumber = Trim(st_ProductNumber)
        st_CASNumber = Trim(st_CASNumber)
        st_ProductName = Trim(st_ProductName)

        ' URL デコード
        st_ProductNumber = HttpUtility.UrlDecode(st_ProductNumber)
        st_CASNumber = HttpUtility.UrlDecode(st_CASNumber)
        st_ProductName = HttpUtility.UrlDecode(st_ProductName)

        ' 全角を半角に変換
        st_ProductNumber = StrConv(st_ProductNumber, VbStrConv.Narrow)
        st_CASNumber = StrConv(st_CASNumber, VbStrConv.Narrow)

        ' 小文字を大文字に変換
        st_ProductNumber = StrConv(st_ProductNumber, VbStrConv.Uppercase)

        ' コントロール設定
        ProductNumber.Text = st_ProductNumber
        CASNumber.Text = st_CASNumber
        ProductName.Text = st_ProductName

        ' パラメータチェック
        If Not String.IsNullOrEmpty(st_ProductNumber) Then
            If (Not TCICommon.Func.IsProductNumber(st_ProductNumber)) And _
               (Not TCICommon.Func.IsNewProductNumber(st_ProductNumber)) And _
               (Not TCICommon.Func.IsCASNumber(st_ProductNumber)) Then
                st_ProductNumber = ""
                Msg.Text = "Product Number" & Common.ERR_REQUIRED_FIELD
                Exit Sub
            End If
        End If
        If Not String.IsNullOrEmpty(st_CASNumber) Then
            If Not TCICommon.Func.IsCASNumber(st_CASNumber) Then
                st_ProductNumber = ""
                Msg.Text = "CAS Number" & Common.ERR_REQUIRED_FIELD
                Exit Sub
            End If
        End If

        ' GET 且つ QueryString("st_ProductNumber") が送信されている場合は検索処理を実行
        If (Request.RequestType = "GET") And (Not String.IsNullOrEmpty(Request.QueryString("ProductNumber"))) Then
            SearchProductList()
        End If

    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        Dim st_Action As String = IIf(Request.QueryString("Action") = Nothing, "", Request.QueryString("Action"))

        If st_Action = SEARCH_ACTION Then
            SearchProductList()
        End If
    End Sub


    Private Sub SearchProductList()

        SrcProduct.SelectParameters.Clear()

        ' Where 句の生成
        Dim st_where As String = ""
        If Not String.IsNullOrEmpty(st_ProductNumber) Then
            SrcProduct.SelectParameters.Add("ProductNumber", Common.SafeSqlLiteral(st_ProductNumber))
            st_where = IIf(st_where.Length > 1, st_where & " AND ", "")
            st_where = st_where & " ProductNumber = @ProductNumber "
        End If
        If Not String.IsNullOrEmpty(st_CASNumber) Then
            SrcProduct.SelectParameters.Add("CASNumber", Common.SafeSqlLiteral(st_CASNumber))
            st_where = IIf(st_where.Length > 1, st_where & " AND ", "")
            st_where = st_where & " CASNumber = @CASNumber "
        End If
        If Not String.IsNullOrEmpty(st_ProductName) Then
            SrcProduct.SelectParameters.Add("ProductName", Common.SafeSqlLikeClauseLiteral(st_ProductName))
            st_where = IIf(st_where.Length > 1, st_where & " AND ", "")
            st_where = st_where & " (Name LIKE N'%' + @ProductName +'%' OR QuoName LIKE N'%' + @ProductName +'%') "
        End If

        ' Where 句が生成できなかった場合は検索処理を行わずに処理を終了する
        If String.IsNullOrEmpty(st_where) Then
            ProductList.DataBind()
            Exit Sub
        End If

        SrcProduct.SelectCommand = _
              " SELECT [ProductID], [ProductNumber], ISNULL([QuoName],[Name]) AS [ProductName], [CASNumber]" _
            & " FROM [Product] " _
            & " WHERE " & st_where

    End Sub
End Class
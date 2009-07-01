Partial Public Class SupplierSelect
    Inherits CommonPage

    ' 接続文字列
    Private st_Code As String = String.Empty
    Private st_Name As String = String.Empty
    Private st_Errorr_Meggage As String = String.Empty
    Const SEARCH_ACTION As String = "Search"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' コントロール初期化
        Msg.Text = ""

        ' パラメータを取得
        If Request.RequestType = "POST" Then
            st_Code = IIf(Request.Form("Code") = Nothing, "", Request.Form("Code"))
            st_Name = IIf(Request.Form("Name") = Nothing, "", Request.Form("Name"))
        ElseIf Request.RequestType = "GET" Then
            st_Code = IIf(Request.QueryString("Code") = Nothing, "", Request.QueryString("Code"))
            st_Name = IIf(Request.QueryString("Name") = Nothing, "", Request.QueryString("Name"))
        End If

        ' 空白除去
        st_Code = Trim(st_Code)
        st_Name = Trim(st_Name)

        ' URL デコード
        st_Code = HttpUtility.UrlDecode(st_Code)
        st_Name = HttpUtility.UrlDecode(st_Name)

        ' 全角を半角に変換
        st_Code = StrConv(st_Code, VbStrConv.Narrow)

        ' 検索ブロックの TextBox の値を書き換え
        Code.Text = st_Code
        Name.Text = st_Name

        ' GET 且つ QueryString("Code") が送信されている場合は検索処理を実行
        If (Request.RequestType = "GET") And (Request.QueryString("Code") <> Nothing) Then
            SetControl_SrcSupplier()
        End If

    End Sub

    ' Search ボタンクリック処理
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click

        Dim st_Action As String = String.Empty

        If Request.Form("Action") = Nothing Then
            st_Action = IIf(Request.QueryString("Action") = Nothing, String.Empty, Request.QueryString("Action")).ToString
        Else
            st_Action = Request.Form("Action").ToString
        End If

        If st_Action = SEARCH_ACTION Then
            SetControl_SrcSupplier()
        End If

    End Sub


    ' SQL データソースコントロールに SELECT 文を設定
    Private Sub SetControl_SrcSupplier()

        ' パラメータチェック
        If Not String.IsNullOrEmpty(st_Code) Then
            If Not Common.IsInteger(st_Code) Then
                st_Code = String.Empty
                SupplierList.DataBind()
                Exit Sub
            End If
        End If

        SrcSupplier.SelectParameters.Clear()
        SrcSupplier.SelectParameters.Add("Code", st_Code)
        SrcSupplier.SelectParameters.Add("Name", Common.SafeSqlLikeClauseLiteral(st_Name))
        SrcSupplier.SelectCommand = CreateSql_SelectSupplier()

    End Sub

    Private Function CreateSql_SelectSupplier() As String
        Dim sb_Sql As StringBuilder = New StringBuilder

        ' Where 句の生成
        Dim st_where As String = String.Empty
        If Not String.IsNullOrEmpty(st_Code) Then
            st_where = IIf(st_where.Length > 1, st_where & " AND ", "")
            st_where = st_where & " Supplier.SupplierCode = @Code "
        End If
        If Not String.IsNullOrEmpty(st_Name) Then
            st_where = IIf(st_where.Length > 1, st_where & " AND ", "")
            st_where = st_where & " ISNULL(Supplier.Name3,'') + ' ' + ISNULL(Supplier.Name4,'') LIKE N'%' + @Name + '%' "
        End If

        ' Where 句が生成できなかった場合は検索処理を行わずに処理を終了する
        If String.IsNullOrEmpty(st_where) Then
            SupplierList.DataBind()
            Return ""
        End If

        sb_Sql.Append(" SELECT ")
        sb_Sql.Append("   SupplierCode, ")
        sb_Sql.Append("   s_Country.[Name] AS CountryName, ")
        sb_Sql.Append("   LTRIM(RTRIM(ISNULL(Supplier.Name3, '') + ' ' + ISNULL(Supplier.Name4, ''))) AS Name ")
        sb_Sql.Append(" FROM ")
        sb_Sql.Append("   Supplier ")
        sb_Sql.Append("   LEFT OUTER JOIN s_Country ")
        sb_Sql.Append("    ON s_Country.CountryCode = Supplier.CountryCode ")
        sb_Sql.Append(" WHERE  ")
        sb_Sql.Append(st_where)
        sb_Sql.Append(" ORDER BY ")
        sb_Sql.Append("   SupplierCode, ")
        sb_Sql.Append("   Name3 ")

        Return sb_Sql.ToString
    End Function

End Class

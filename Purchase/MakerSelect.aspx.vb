Partial Public Class MakerSelect
    Inherits CommonPage

    ' 接続文字列
    Private st_Code As String = String.Empty
    Private st_Name As String = String.Empty
    Const SEARCH_ACTION As String = "Search"
    Private st_QuLocation As String = String.Empty


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' コントロール初期化
        Msg.Text = ""

        ' パラメータを取得
        If Request.RequestType = "POST" Then
            st_Code = IIf(Request.Form("Code") = Nothing, "", Request.Form("Code"))
            st_Name = IIf(Request.Form("Name") = Nothing, "", Request.Form("Name"))
            
            st_QuLocation = IIf(Request.Form("Name") = Nothing, "", Request.Form("Name"))
        ElseIf Request.RequestType = "GET" Then
            st_Code = IIf(Request.QueryString("Code") = Nothing, "", Request.QueryString("Code"))
            st_Name = IIf(Request.QueryString("Name") = Nothing, "", Request.QueryString("Name"))
           
            st_QuLocation = IIf(Request.QueryString("Name") = Nothing, "", Request.QueryString("Name"))
        End If

        ' 空白除去
        st_Code = Trim(st_Code)
        st_Name = Trim(st_Name)
        st_QuLocation = Trim(st_QuLocation)

        ' URL デコード
        st_Code = HttpUtility.UrlDecode(st_Code)
        st_Name = HttpUtility.UrlDecode(st_Name)
        st_QuLocation = HttpUtility.UrlDecode(st_QuLocation)

        ' 全角を半角に変換
        st_Code = StrConv(st_Code, VbStrConv.Narrow)

        ' 検索ブロックの TextBox の値を書き換え
        Code.Text = st_Code
        Name.Text = st_Name

        ' GET 且つ QueryString("Code") が送信されている場合は検索処理を実行
        If (Request.RequestType = "GET") And (Not String.IsNullOrEmpty(Request.QueryString("Code"))) Then
            SearchSupplierList()
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
            SearchSupplierList()
        End If
    End Sub

    ' 検索処理
    Private Sub SearchSupplierList()

        ' パラメータチェック
        If Not String.IsNullOrEmpty(st_Code) Then
            If Not Common.IsInteger(st_Code) Then
                st_Code = String.Empty
                SupplierList.DataBind()
                Exit Sub
            End If
        End If

        SrcMaker.SelectParameters.Clear()

        ' Where 句の生成
        Dim st_where As String = String.Empty
        If Not String.IsNullOrEmpty(st_Code) Then
            SrcMaker.SelectParameters.Add("Code", st_Code)
            st_where = IIf(st_where.Length > 1, st_where & " AND ", "")
            st_where = st_where & " Supplier.SupplierCode = @Code "
        End If
        If Not String.IsNullOrEmpty(st_Name) Then
            SrcMaker.SelectParameters.Add("Name", Common.SafeSqlLikeClauseLiteral(st_Name))
            st_where = IIf(st_where.Length > 1, st_where & " AND ", "")
            st_where = st_where & " ISNULL(Supplier.Name3,'') + ' ' + ISNULL(Supplier.Name4,'') LIKE N'%' + @Name + '%' "
        End If

        ' Where 句が生成できなかった場合は検索処理を行わずに処理を終了する
        If String.IsNullOrEmpty(st_where) Then
            SupplierList.DataBind()
            Exit Sub
        End If

        SrcMaker.SelectCommand = _
              " SELECT S4SupplierCode,SupplierCode,LocationCode, s_Country.[Name] AS CountryName, " _
            & "   LTRIM(RTRIM(ISNULL(Supplier.Name3, '') + ' ' + ISNULL(Supplier.Name4, ''))) AS Name " _
            & " FROM  Supplier " _
            & "   LEFT OUTER JOIN s_Country " _
            & "   ON s_Country.CountryCode = Supplier.CountryCode " _
            & " WHERE " & st_where _
            & " ORDER BY SupplierCode, Name3 "

    End Sub

End Class

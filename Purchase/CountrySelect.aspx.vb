Public Partial Class CountrySelect
    Inherits CommonPage

    Private DBConnectString As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Private st_Code As String = ""
    Private st_Name As String = ""
    Const SEARCH_ACTION As String = "Search"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' コントロール初期化
        Msg.Text = ""

        ' パラメータ取得
        If Request.RequestType = "POST" Then
            st_Code = IIf(Request.Form("Code") = Nothing, "", Request.Form("Code"))
            st_Name = IIf(Request.Form("Name") = Nothing, "", Request.Form("Name"))
        ElseIf Request.RequestType = "GET" Then
            st_Code = IIf(Request.QueryString("Code") = Nothing, "", Request.QueryString("Code"))
            st_Name = IIf(Request.QueryString("Name") = Nothing, "", Request.QueryString("Name"))
        End If

        ' 空白除去
        st_Code = st_Code.Trim
        st_Name = st_Name.Trim

        ' URL デコード
        st_Code = HttpUtility.UrlDecode(st_Code)
        st_Name = HttpUtility.UrlDecode(st_Name)

        ' 全角を半角に変換
        st_Code = StrConv(st_Code, VbStrConv.Narrow)
        st_Name = StrConv(st_Name, VbStrConv.Narrow)

        ' コントロール設定
        Code.Text = st_Code
        Name.Text = st_Name

        ' GET 且つ QueryString("Code") が空ではない場合は検索処理を実行
        If (Request.RequestType = "GET") And (Not String.IsNullOrEmpty(Request.QueryString("Code"))) Then
            GetCountryList()
        End If

    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Search.Click

        Dim st_Action As String = IIf(Request.QueryString("Action") = Nothing, "", Request.QueryString("Action"))

        If st_Action = SEARCH_ACTION Then
            GetCountryList()
        End If

    End Sub

    ' 検索処理
    Protected Sub GetCountryList()

        Dim st_Where As String = ""
        SrcCountry.SelectParameters.Clear()

        ' Where 句の生成
        If Not String.IsNullOrEmpty(st_Code) Then
            SrcCountry.SelectParameters.Add("CountryCode", Common.SafeSqlLiteral(st_Code))
            st_Where = IIf(st_Where.Length > 1, st_Where & " AND ", st_Where)
            st_Where = st_Where & " CountryCode = @CountryCode "
        End If

        If Not String.IsNullOrEmpty(st_Name) Then
            SrcCountry.SelectParameters.Add("CountryName", Common.SafeSqlLikeClauseLiteral(st_Name))
            st_Where = IIf(st_Where.Length > 1, st_Where & " AND ", st_Where)
            st_Where = st_Where & " [Name] LIKE N'%' + @CountryName + '%' "
        End If

        ' Where 句が生成できなかった場合は処理終了
        If String.IsNullOrEmpty(st_Where) Then
            Exit Sub
        End If

        SrcCountry.SelectCommand = _
              " SELECT [CountryCode], [Name] " _
            & " FROM [s_Country] " _
            & " WHERE " & st_Where _
            & " ORDER BY CountryCode, [Name] ASC"

    End Sub

End Class



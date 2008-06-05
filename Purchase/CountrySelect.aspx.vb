Public Partial Class CountrySelect
    Inherits CommonPage

    Public st_Code As String
    Public st_Name As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' パラメータCode, Name を取得。
        st_Code = IIf(Not (IsPostBack) And Not (String.IsNullOrEmpty(Request.QueryString("Code"))), Request.QueryString("Code"), Request.Form("Code"))
        st_Name = Request.Form("Name")

        ' 空白除去
        st_Code = Trim(st_Code)
        st_Name = Trim(st_Name)

        ' 検索ブロックの TextBox の値を書き換え
        Code.Text = st_Code
        Name.Text = st_Name

        ' ポストバックではない 且つ GET が 空, notiong, NULL 以外なら実行
        If Not (IsPostBack) And Not (String.IsNullOrEmpty(Request.QueryString("Code"))) Then
            ' SQL クエリをSelectCommand にセット
            SrcCountry.SelectCommand = "SELECT [CountryCode], [Name] FROM [s_Country] WHERE [CountryCode] = @Code ORDER BY CountryCode, Name"

            ' SelectParameters のデフォルト値に Code をセット
            SrcCountry.SelectParameters.Item("Code").DefaultValue = IIf(st_Code <> "", st_Code, "NULL")
        End If

    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Search.Click

        Dim st_Sql_Select As String = ""

        ' Code と Name の値によって SQL クエリを分岐
        If Not String.IsNullOrEmpty(st_Code) And String.IsNullOrEmpty(st_Name) Then
            st_Sql_Select = "WHERE [CountryCode] = @Code"
        ElseIf String.IsNullOrEmpty(st_Code) And Not String.IsNullOrEmpty(st_Name) Then
            st_Sql_Select = "WHERE [Name] LIKE '%' + @Name + '%'"
        End If

        ' SQL クエリをSelectCommand にセット
        If Not (String.IsNullOrEmpty(st_Sql_Select)) Then
            SrcCountry.SelectCommand = "SELECT [CountryCode], [Name] FROM [s_Country] " & st_Sql_Select & " ORDER BY CountryCode, Name"
        End If

        ' SelectParameters のデフォルト値に Code と Name をセット
        SrcCountry.SelectParameters.Item("Code").DefaultValue = IIf(String.IsNullOrEmpty(st_Code), "NULL", st_Code)
        SrcCountry.SelectParameters.Item("Name").DefaultValue = IIf(String.IsNullOrEmpty(st_Name), "NULL", st_Name)

    End Sub

End Class



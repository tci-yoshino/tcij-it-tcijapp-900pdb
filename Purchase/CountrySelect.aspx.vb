Public Partial Class CountrySelect
    Inherits CommonPage

    Public st_Code As String
    Public st_Name As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' パラメータCode, Name を取得。
        st_Code = IIf(Not (IsPostBack) And Request.QueryString("Code") <> "", Request.QueryString("Code"), Request.Form("Code"))
        st_Name = Request.Form("Name")

        ' 空白除去
        st_Code = Trim(st_Code)
        st_Name = Trim(st_Name)

        ' 検索ブロックの TextBox の値を書き換え。
        Code.Text = st_Code
        Name.Text = st_Name

        ' ポストバックではない 且つ クエリストリングがある場合に実行
        If Not (IsPostBack) And Request.QueryString("Code") <> "" Then
            SrcCountry.SelectCommand = "SELECT [CountryCode], [Name] FROM [s_Country] WHERE [CountryCode] = UPPER('" & st_Code & "') ORDER BY CountryCode, Name"
        End If

    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Search.Click
        Dim st_Sql_Select As String

        st_Sql_Select = "WHERE [CountryCode] = UPPER('" & st_Code & "') AND [Name] LIKE '%" & st_Name & "%'"
        If (st_Code <> "" And st_Name = "") Then st_Sql_Select = "WHERE [CountryCode] = UPPER('" & st_Code & "')"
        If (st_Code = "" And st_Name <> "") Then st_Sql_Select = "WHERE [Name] LIKE '%" & st_Name & "%'"

        SrcCountry.SelectCommand = IIf(st_Sql_Select = "", "", "SELECT [CountryCode], [Name] FROM [s_Country] " & st_Sql_Select & " ORDER BY CountryCode, Name")

        'SrcCountry.SelectParameters.Item("Code").DefaultValue = IIf(st_Code <> "", st_Code, "NULL")
        'SrcCountry.SelectParameters.Item("Name").DefaultValue = IIf(st_Name <> "", st_Name, "NULL")
    End Sub

End Class



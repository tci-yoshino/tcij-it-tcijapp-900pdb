Public Partial Class CountrySelect
    Inherits CommonPage

    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    Public st_Code As String
    Public st_Name As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' パラメータCode, Name を取得
        st_Code = IIf(Not (IsPostBack) And Not (String.IsNullOrEmpty(Request.QueryString("Code"))), Request.QueryString("Code"), Request.Form("Code"))
        st_Name = Request.Form("Name")

        ' 空白除去
        st_Code = Trim(st_Code)
        st_Name = Trim(st_Name)

        ' 検索ブロックの TextBox の値を書き換え
        Code.Text = st_Code
        Name.Text = st_Name

        ' Cuntry List のデータをクリア
        CountryList.Items.Clear()
        CountryList.DataBind()

        ' ポストバックではない 且つ GET が 空, notiong, NULL 以外なら実行
        If Not (IsPostBack) And Not (String.IsNullOrEmpty(Request.QueryString("Code"))) Then
            Get_Country_List(DBConnectString.ConnectionString)
        End If

    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Search.Click

        If Not String.IsNullOrEmpty(st_Code) Or Not String.IsNullOrEmpty(st_Name) Then
            Get_Country_List(DBConnectString.ConnectionString)
        End If

    End Sub


    Private Sub Get_Country_List(ByVal connectionString As String)

        Dim st_where As String = " WHERE [CountryCode] = @Code AND [Name] LIKE '%' + @Name + '%' "

        If Not String.IsNullOrEmpty(st_Code) And String.IsNullOrEmpty(st_Name) Then
            st_where = " WHERE [CountryCode] = @Code "
        ElseIf String.IsNullOrEmpty(st_Code) And Not String.IsNullOrEmpty(st_Name) Then
            st_where = " WHERE [Name] LIKE '%' + @Name + '%' "
        End If

        Dim st_query As String = "SELECT [CountryCode], [Name] FROM [s_Country] " & st_where & " ORDER BY CountryCode, Name"

        Try

            Using connection As New SqlClient.SqlConnection(connectionString)

                Dim command As New SqlClient.SqlCommand(st_query, connection)
                command.Parameters.AddWithValue("Code", st_Code)
                command.Parameters.AddWithValue("Name", st_Name)
                connection.Open()

                Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()

                CountryList.DataSource = reader
                CountryList.DataBind()
                reader.Close()

            End Using

        Catch ex As Exception
            'Exception をスローする
            Throw
        End Try

    End Sub

End Class



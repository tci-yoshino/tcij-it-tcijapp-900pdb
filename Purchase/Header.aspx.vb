Public Partial Class Header
    Inherits CommonPage
    ' 接続文字列
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim st_query As String = "SELECT [Name], [LocationName] FROM [v_User] WHERE [UserID] = @UserID"

        Try
            Using connection As New SqlClient.SqlConnection(DBConnectString.ConnectionString)

                Dim command As New SqlClient.SqlCommand(st_query, connection)
                command.Parameters.AddWithValue("UserID", Session("UserID"))
                connection.Open()

                Dim reader As SqlClient.SqlDataReader
                reader = command.ExecuteReader()

                If reader.HasRows Then
                    reader.Read()
                    UserName.Text = reader.GetString(0)
                    LocationName.Text = reader.GetString(1)
                End If

                reader.Close()

            End Using
        Catch ex As Exception
            'Exception をスローする
            Throw
        End Try

    End Sub

End Class
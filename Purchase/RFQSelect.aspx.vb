Public Partial Class RFQSelect
    Inherits CommonPage
    ' 変数宣言
    Private st_ParPONumber As String
    Private DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    ' 定数宣言
    Private Const MSG_REQUIED_ParPONumber = "購買発注番号が指定されていません。"
    Private Const MSG_REQUIED_ACTION = "データを処理できませんでした。"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' パラメータ取得
        If Request.RequestType = "POST" Then
            st_ParPONumber = IIf(Request.Form("ParPONumber") = Nothing, "", Request.Form("ParPONumber"))
        ElseIf Request.RequestType = "GET" Then
            st_ParPONumber = IIf(Request.QueryString("ParPONumber") = Nothing, "", Request.QueryString("ParPONumber"))
        End If

        ' 空白除去
        st_ParPONumber = st_ParPONumber.Trim

        ' パラメータエラー
        If String.IsNullOrEmpty(st_ParPONumber) Then
            Msg.Text = MSG_REQUIED_ParPONumber
            Exit Sub
        End If

        ' 変数宣言
        Dim st_ProductID As String = ""
        Dim st_SupplierCode As String = ""
        Dim st_MakerCode As String = ""

        ' 親POデータを取得。取得出来ない場合はエラー表示
        Try
            Using connection As New SqlClient.SqlConnection(DBConnectString.ConnectionString)
                Dim st_query As String = "SELECT ProductID, SupplierCode, MakerCode FROM PO WHERE PONumber = @PONumber"
                Dim command As New SqlClient.SqlCommand(st_query, connection)
                connection.Open()

                ' SQL SELECT パラメータの追加
                command.Parameters.AddWithValue("PONumber", st_ParPONumber)

                ' SqlDataReader を生成し、検索処理を実行。
                Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()

                ' データを変数にセット
                reader.Read()
                st_ProductID = IIf(IsDBNull(reader("ProductID")), "", reader("ProductID"))
                st_SupplierCode = IIf(IsDBNull(reader("SupplierCode")), "", reader("SupplierCode"))
                st_MakerCode = IIf(IsDBNull(reader("MakerCode")), "", reader("MakerCode"))

                reader.Close()
            End Using
        Catch ex As Exception
            Throw
        End Try

        ' 製品情報取得
        Try
            Using connection As New SqlClient.SqlConnection(DBConnectString.ConnectionString)
                Dim st_query As String = _
                      "SELECT " _
                    & "  ProductNumber, ISNULL(Name, QuoName) AS ProductName " _
                    & "FROM " _
                    & "  Product " _
                    & "WHERE " _
                    & "  (ProductID = @ProductID)"
                Dim command As New SqlClient.SqlCommand(st_query, connection)
                connection.Open()

                ' SQL SELECT パラメータの追加
                command.Parameters.AddWithValue("ProductID", st_ProductID)

                ' SqlDataReader を生成し、検索処理を実行。
                Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()

                ' データをHTMLのラベルにセット
                reader.Read()
                ProductNumber.Text = IIf(IsDBNull(reader("ProductNumber")), "", reader("ProductNumber"))
                ProductName.Text = IIf(IsDBNull(reader("ProductName")), "", reader("ProductName"))
                
                reader.Close()
            End Using
        Catch ex As Exception
            Throw
        End Try

        ' 仕入先情報取得
        Try
            Using connection As New SqlClient.SqlConnection(DBConnectString.ConnectionString)
                Dim st_query As String = _
                      "SELECT " _
                    & "  Name3, Name4, s_Country.[Name] AS CountryName " _
                    & "FROM " _
                    & "  Supplier, s_Country " _
                    & "WHERE " _
                    & "  (SupplierCode = @SupplierCode) " _
                    & "  AND (Supplier.CountryCode = s_Country.CountryCode)"

                Dim command As New SqlClient.SqlCommand(st_query, connection)
                connection.Open()

                ' SQL SELECT パラメータの追加
                command.Parameters.AddWithValue("SupplierCode", st_SupplierCode)

                ' SqlDataReader を生成し、検索処理を実行。
                Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()

                ' データをHTMLのラベルにセット
                reader.Read()
                Country.Text = IIf(IsDBNull(reader("CountryName")), "", reader("CountryName"))
                If IsDBNull(reader("Name3")) Then
                    SupplierName.Text = reader("Name4")
                Else
                    SupplierName.Text = reader("Name3") & " " & reader("Name4")
                End If

                reader.Close()
            End Using
        Catch ex As Exception
            Throw
        End Try

        ' 製品情報、仕入先情報をバインド

        ' RFQHeader 取得

        ' RFQHeader をバインド

    End Sub

    ' RFQLine を RFQHeader のレコードごとに取得し、バインドする
    Protected Sub Get_RFQLine(ByVal sender As Object, ByVal e As System.EventArgs) Handles RFQHeaderList.DataBound

    End Sub


    Protected Sub Next_Click(ByVal sender As Object, ByVal e As EventArgs) Handles NextPage.Click

        ' パラメータチェック。取得できない・"Next" で無い場合はエラー

        ' チェックされた RFQLine のデータを取得。取得できない場合はエラー

        ' Price の値が無い場合はエラー

        ' POIssue.aspx に遷移

    End Sub

End Class
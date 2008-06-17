Partial Public Class MakerSelect
    Inherits CommonPage

    ' 接続文字列
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    Public st_Code As String
    Public st_Name As String
    Public st_Location As String
    Public st_Action As String
    Public st_Errorr_Meggage As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' パラメータ Location を取得
        st_Location = IIf(Not (IsPostBack) And Not (String.IsNullOrEmpty(Request.QueryString("Location"))), Request.QueryString("Location"), Request.Form("Location"))
        st_Location = Trim(st_Location)  ' 空白除去

        If String.IsNullOrEmpty(st_Location) Then

            ' パラメータ Location が無い場合はエラーメッセージを表示して終了。
            st_Errorr_Meggage = st_Errorr_Meggage & "見積依頼拠点コードが設定されていません。"
            ErrorMessages.Text = st_Errorr_Meggage
            Exit Sub

        Else

            ' パラメータ Action を取得
            If Request.QueryString("Action") = "Search" Or Request.Form("Action") = "Search" Then
                st_Action = "Search"
            End If

            ' パラメータ Code, Name を取得
            st_Code = IIf(Not (IsPostBack) And Not (String.IsNullOrEmpty(Request.QueryString("Code"))), Request.QueryString("Code"), Request.Form("Code"))
            st_Name = Request.Form("Name")

            ' 空白除去
            st_Code = Trim(st_Code)
            st_Name = Trim(st_Name)

            ' 全角を半角に変換
            st_Code = StrConv(st_Code, VbStrConv.Narrow)

            ' 検索ブロックの TextBox の値を書き換え
            Code.Text = st_Code
            Name.Text = st_Name
            Location.Value = st_Location

            ' Supplier List のデータをクリア
            SupplierList.Items.Clear()
            SupplierList.DataBind()

            ' ポストバックではない 且つ GET が 空, notiong, NULL 以外なら実行
            If Not (IsPostBack) And Not (String.IsNullOrEmpty(Request.QueryString("Code"))) Then
                Dim dataSet As DataSet = New DataSet
                dataSet = Get_Supplier_Data(dataSet, DBConnectString.ConnectionString)
                SupplierList.DataSource = dataSet
                SupplierList.DataBind()
            End If

        End If
    End Sub

    ' Search ボタンクリック処理
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        If Not String.IsNullOrEmpty(st_Code) Or Not String.IsNullOrEmpty(st_Name) Then
            Dim dataSet As DataSet = New DataSet
            dataSet = Get_Supplier_Data(dataSet, DBConnectString.ConnectionString)
            SupplierList.DataSource = dataSet
            SupplierList.DataBind()
        End If
    End Sub

    ' Supplier リスト取得処理
    ' Public 変数の st_Code と st_Name を元に Supplier テーブルからデータを取得する。
    ' [パラメータ]
    ' dataSet As DataSet: 取得したデータをセットする DataSet オブジェクト
    ' connectionString As String) As String: 接続情報
    ' [戻り値]
    ' dataSet As DataSet : 取得したデータをセットした DataSet オブジェクト
    Public Function Get_Supplier_Data(ByVal dataSet As DataSet, ByVal connectionString As String) As DataSet
        Dim st_where As String = " WHERE(SupplierCode = @Code) AND ({ fn CONCAT(Name1, Name2) } LIKE N'%' + @Name + '%') "
        If Not String.IsNullOrEmpty(st_Code) And String.IsNullOrEmpty(st_Name) Then
            st_where = " WHERE(SupplierCode = @Code) "
        ElseIf String.IsNullOrEmpty(st_Code) And Not String.IsNullOrEmpty(st_Name) Then
            st_where = "  WHERE({ fn CONCAT(Name1, Name2) } LIKE N'%' + @Name + '%')  "
        End If


        Dim st_query As String = "SELECT SupplierCode, Name3, Name4, CountryCode FROM Supplier " & st_where & " ORDER BY SupplierCode, Name3 "

        Try
            Using connection As New SqlClient.SqlConnection(connectionString)

                Dim adapter As New SqlClient.SqlDataAdapter()
                Dim command As New SqlClient.SqlCommand(st_query, connection)

                command.Parameters.AddWithValue("Code", st_Code)
                command.Parameters.AddWithValue("Name", st_Name)
                command.Parameters.AddWithValue("Location", st_Location)

                adapter.SelectCommand = command
                adapter.Fill(dataSet)

                Dim dataTable As DataTable = New DataTable
                dataTable = dataSet.Tables(0)
                dataTable.Columns.Add("QuoLocationCode", Type.GetType("System.String"))

                Dim cnt As Integer = dataTable.Rows.Count - 1
                Dim i As Integer
                For i = 0 To cnt
                    Dim QuoLocationCode = Get_Quo_Location_Code(dataTable.Rows(i).Item("SupplierCode"), dataTable.Rows(i).Item("CountryCode"), DBConnectString.ConnectionString)
                    dataTable.Rows(i).Item("QuoLocationCode") = QuoLocationCode
                Next i

            End Using
        Catch ex As Exception
            st_Errorr_Meggage = "データベース接続時にエラーが発生しました。"
        End Try

        Return dataSet

    End Function

    ' 見積回答拠点コード取得関数
    ' 見積拠点例外テーブルを仕入先コードとロケーションで検索し、レコードが取得できれば見積回答拠点コードを返す。
    ' 取得できなかった場合、国テーブルを仕入先の国コードで検索し、レコードを取得し、見積依頼先デフォルトを返す。
    ' [パラメータ]
    ' supplierCode As String: 仕入先コード
    ' countryCode As String: 仕入先の国コード
    ' connectionString As String) As String: 接続情報
    '
    ' [戻り値]
    ' st_QuoLocationCode As String: 見積回答拠点コード（もしくは見積依頼先デフォルト）
    Private Function Get_Quo_Location_Code(ByVal supplierCode As String, ByVal countryCode As String, ByVal connectionString As String) As String

        Dim st_query As String
        Dim st_QuoLocationCode As String = ""

        Try

            Using connection As New SqlClient.SqlConnection(connectionString)

                st_query = "SELECT [QuoLocationCode] FROM [IrregularRFQLocation] WHERE [SupplierCode] = @Code AND [EnqLocationCode] = @Location"
                Dim command As New SqlClient.SqlCommand(st_query, connection)
                command.Parameters.AddWithValue("Code", supplierCode)
                command.Parameters.AddWithValue("Location", st_Location) ' Public 変数
                connection.Open()

                Dim reader As SqlClient.SqlDataReader

                reader = command.ExecuteReader()

                If reader.HasRows Then
                    reader.Read()
                    st_QuoLocationCode = IIf(IsDBNull(reader.Item("QuoLocationCode")), "Direct", reader.Item("QuoLocationCode"))
                End If
                reader.Close()
            End Using

            If st_QuoLocationCode = "" Then

                Using connection As New SqlClient.SqlConnection(connectionString)

                    st_query = "SELECT [DefaultQuoLocationCode] FROM [PurchasingCountry] WHERE [CountryCode] = @CountryCode"
                    Dim command As New SqlClient.SqlCommand(st_query, connection)
                    command.Parameters.AddWithValue("CountryCode", countryCode)
                    connection.Open()

                    Dim reader As SqlClient.SqlDataReader

                    reader = command.ExecuteReader()

                    If reader.HasRows Then
                        reader.Read()
                        st_QuoLocationCode = IIf(IsDBNull(reader.Item("DefaultQuoLocationCode")), "Direct", reader.Item("DefaultQuoLocationCode"))
                    Else
                        st_Errorr_Meggage = "見積依頼先デフォルトが取得できませんでした。"
                    End If
                    reader.Close()
                End Using
            End If

        Catch ex As Exception
            st_Errorr_Meggage = "データベース接続時にエラーが発生しました。"
        End Try

        Return st_QuoLocationCode

    End Function


End Class

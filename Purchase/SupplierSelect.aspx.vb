Public Partial Class SupplierSelect
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

            ' ポストバックではない 且つ GET が 空, notiong, NULL 以外なら実行
            If Not (IsPostBack) And Not (String.IsNullOrEmpty(Request.QueryString("Code"))) Then
                Get_Supplier_Data(DBConnectString.ConnectionString)
            End If

        End If



    End Sub

    ' Search ボタンクリック処理
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        Get_Supplier_Data(DBConnectString.ConnectionString)
    End Sub


    Private Sub Get_Supplier_Data(ByVal connectionString As String)

        Dim st_where As String = " WHERE (Supplier.SupplierCode = @Code) AND ({ fn CONCAT(Supplier.Name1, Supplier.Name2) } LIKE N'%' + @Name + '%') "

            If Not String.IsNullOrEmpty(st_Code) And String.IsNullOrEmpty(st_Name) Then
                st_where = " WHERE (Supplier.SupplierCode = @Code) "
            ElseIf String.IsNullOrEmpty(st_Code) And Not String.IsNullOrEmpty(st_Name) Then
                st_where = " WHERE ({ fn CONCAT(Supplier.Name1, Supplier.Name2) } LIKE N'%' + @Name + '%') "
            End If

        Dim st_query As String = _
  "SELECT Supplier.SupplierCode, Supplier.Name3, Supplier.Name4, Supplier.CountryCode, " _
& "  ISNULL(IrregularRFQLocation.QuoLocationCode,PurchasingCountry.DefaultQuoLocationCode) as QuoLocationCode " _
& "FROM Supplier " _
& "  INNER JOIN PurchasingCountry ON Supplier.CountryCode = PurchasingCountry.CountryCode" _
& "  LEFT OUTER JOIN IrregularRFQLocation ON Supplier.SupplierCode = IrregularRFQLocation.SupplierCode" _
& "      AND IrregularRFQLocation.EnqLocationCode = @Location " _
& st_where _
& "ORDER BY Supplier.SupplierCode, Supplier.Name3;"

        Try

            Using connection As New SqlClient.SqlConnection(connectionString)

                Dim command As New SqlClient.SqlCommand(st_query, connection)
                command.Parameters.AddWithValue("Code", st_Code)
                command.Parameters.AddWithValue("Name", st_Name)
                command.Parameters.AddWithValue("Location", st_Location)
                connection.Open()

                Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()

                SupplierList.DataSource = reader
                SupplierList.DataBind()
                reader.Close()

            End Using

        Catch ex As Exception
            SupplierList.Items.Clear()

        End Try

    End Sub

End Class

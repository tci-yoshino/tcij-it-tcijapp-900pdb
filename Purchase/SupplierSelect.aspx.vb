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

            ' Supplier List のデータをクリア
            SupplierList.Items.Clear()
            'SupplierList.DataBind()

            ' ポストバックではない 且つ GET が 空, notiong, NULL 以外なら実行
            If Not (IsPostBack) And Not (String.IsNullOrEmpty(Request.QueryString("Code"))) Then
                Dim dataSet As DataSet = New DataSet
                dataSet = Get_Supplier_Data(dataSet, DBConnectString.ConnectionString)
                Dim SupplierDataTable As DataTable = dataSet.Tables("Supplier")
                SupplierDataTable.Columns.Add("QuoLocationCode", Type.GetType("System.String"))
                SupplierDataTable.NewRow()
                SupplierDataTable.Rows(0)("QuoLocationCode") = "test"
                SupplierList.DataSource = SupplierDataTable
                SupplierList.DataBind()
            End If

        End If
    End Sub

    ' Search ボタンクリック処理
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        If Not String.IsNullOrEmpty(st_Code) Or Not String.IsNullOrEmpty(st_Name) Then
            Dim dataSet As DataSet = New DataSet
            dataSet = Get_Supplier_Data(dataSet, DBConnectString.ConnectionString)
            Dim SupplierDataTable As DataTable = dataSet.Tables("Supplier")
            SupplierDataTable.Columns.Add("QuoLocationCode", Type.GetType("System.String"), "QuoLocationCode")
            SupplierDataTable.Rows(0)("QuoLocationCode") = "test"
            SupplierList.DataSource = SupplierDataTable
            SupplierList.DataBind()
        End If
    End Sub

    Public Function Get_Supplier_Data(ByVal dataSet As DataSet, ByVal connectionString As String) As DataSet
        Dim st_query As String = _
            "SELECT Supplier.SupplierCode, Supplier.Name3, Supplier.Name4, Supplier.CountryCode, " _
            & "     IrregularRFQLocation.QuoLocationCode as IrregularQuoLocationCode,PurchasingCountry.DefaultQuoLocationCode " _
            & "FROM Supplier " _
            & "  INNER JOIN PurchasingCountry ON Supplier.CountryCode = PurchasingCountry.CountryCode" _
            & "  LEFT OUTER JOIN IrregularRFQLocation ON Supplier.SupplierCode = IrregularRFQLocation.SupplierCode" _
            & "      AND IrregularRFQLocation.EnqLocationCode = @Location " _
            & "ORDER BY Supplier.SupplierCode, Supplier.Name3;"

        Using connection As New SqlClient.SqlConnection(connectionString)

            Dim adapter As New SqlClient.SqlDataAdapter()
            Dim command As New SqlClient.SqlCommand(st_query, connection)

            command.Parameters.AddWithValue("Code", st_Code)
            command.Parameters.AddWithValue("Name", st_Name)
            command.Parameters.AddWithValue("Location", st_Location)

            adapter.SelectCommand = command
            adapter.Fill(dataSet)

            Return dataSet

        End Using
    End Function


End Class

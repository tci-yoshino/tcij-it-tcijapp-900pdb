Public Partial Class ProductSelect
    Inherits CommonPage
    ' 接続文字列
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    Public st_ProductNumber As String
    Public st_CASNumber As String
    Public st_ProductName As String


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' パラメータを取得
        If Request.RequestType = "POST" Then
            st_ProductNumber = Request.Form("ProductNumber")
            st_CASNumber = Request.Form("CASNumber")
            st_ProductName = Request.Form("ProductName")
        Else
            st_ProductNumber = Request.QueryString("ProductNumber")
            st_CASNumber = Request.QueryString("CASNumber")
            st_ProductName = Request.QueryString("ProductName")
        End If

        ' 空白除去
        st_ProductNumber = Trim(st_ProductNumber)
        st_CASNumber = Trim(st_CASNumber)
        st_ProductName = Trim(st_ProductName)

        ' 全角を半角に変換
        st_ProductNumber = StrConv(st_ProductNumber, VbStrConv.Narrow)
        st_CASNumber = StrConv(st_CASNumber, VbStrConv.Narrow)

        ' 検索ブロックの TextBox の値を書き換え
        ProductNumber.Text = st_ProductNumber
        CASNumber.Text = st_CASNumber
        ProductName.Text = st_ProductName

        ' Supplier List のデータをクリア
        ProductList.Items.Clear()
        ProductList.DataBind()

        ' ポストバックではない 且つ GET が 空 , notiong 以外なら実行
        If Not IsPostBack Then
            If Not String.IsNullOrEmpty(Request.QueryString("ProductNumber")) _
            Or Not String.IsNullOrEmpty(Request.QueryString("CASNumber")) _
            Or Not String.IsNullOrEmpty(Request.QueryString("ProductName")) Then
                Dim dataSet As DataSet = New DataSet("Product")
                dataSet.Tables.Add("ProductList")
                Get_Product_List(dataSet, DBConnectString.ConnectionString)
                ProductList.DataSource = dataSet.Tables("ProductList")
                ProductList.DataBind()
            End If
        End If

    End Sub



    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        If Not String.IsNullOrEmpty(st_ProductNumber) _
          Or Not String.IsNullOrEmpty(st_CASNumber) _
          Or Not String.IsNullOrEmpty(st_ProductName) Then
            Dim dataSet As DataSet = New DataSet("Product")
            dataSet.Tables.Add("ProductList")
            Get_Product_List(dataSet, DBConnectString.ConnectionString)
            ProductList.DataSource = dataSet.Tables("ProductList")
            ProductList.DataBind()
        End If
    End Sub


    Protected Function Get_Product_List(ByVal dataSet As DataSet, ByVal connectionString As String) As DataSet

        ' WHERE 分の分岐
        Dim st_where_arr As New ArrayList()
        Dim st_where As String = ""

        If Not String.IsNullOrEmpty(st_ProductNumber) Then st_where_arr.Add("ProductNumber = @ProductNumber ")
        If Not String.IsNullOrEmpty(st_CASNumber) Then st_where_arr.Add(" CASNumber = @CASNumber ")
        If Not String.IsNullOrEmpty(st_ProductName) Then st_where_arr.Add(" (Name LIKE N'%' + @ProductName +'%' OR QuoName LIKE N'%' + @ProductName +'%') ")

        st_where = st_where_arr.Item(0)
        If st_where_arr.Count >= 2 Then
            For i As Integer = 0 To st_where_arr.Count - 2
                st_where = st_where & " AND " & st_where_arr.Item(i)
            Next i
        End If

        ' 製品リスト取得
        Dim st_query As String = _
          " SELECT [ProductID], [ProductNumber], ISNULL([QuoName],[Name]) AS [ProductName], [CASNumber]" _
        & " FROM [Product] " _
        & " WHERE " & st_where

        Try
            Using connection As New SqlClient.SqlConnection(connectionString)

                ' 接続情報、アダプタ、SQLコマンド オブジェクトの生成
                Dim adapter As New SqlClient.SqlDataAdapter()
                Dim command As New SqlClient.SqlCommand(st_query, connection)

                ' SQL SELECT パラメータの追加
                command.Parameters.AddWithValue("ProductNumber", st_ProductNumber)
                command.Parameters.AddWithValue("CASNumber", st_CASNumber)
                command.Parameters.AddWithValue("ProductName", st_ProductName)

                ' データベースからデータを取得
                adapter.SelectCommand = command
                adapter.Fill(dataSet.Tables("ProductList"))
            End Using
        Catch ex As Exception
            'Exception をスローする
            Throw
        End Try

        Return dataSet

    End Function
End Class
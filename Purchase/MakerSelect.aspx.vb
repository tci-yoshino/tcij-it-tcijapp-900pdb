Partial Public Class MakerSelect
    Inherits CommonPage

    ' 接続文字列
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    Public st_Code As String
    Public st_Name As String
    Public st_Errorr_Meggage As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        ' Supplier List のデータをクリア
        SupplierList.Items.Clear()
        SupplierList.DataBind()

        ' ポストバックではない 且つ GET が 空, notiong, NULL 以外なら実行
        If Not (IsPostBack) And Not (String.IsNullOrEmpty(Request.QueryString("Code"))) Then
            Dim dataSet As DataSet = New DataSet("Supplier")
            dataSet = Get_Supplier_Data(dataSet, DBConnectString.ConnectionString)
            SupplierList.DataSource = dataSet.Tables("SupplierList")
            SupplierList.DataBind()
        End If

    End Sub

    ' Search ボタンクリック処理
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        ' st_Code と st_Name があれば実行
        If Not String.IsNullOrEmpty(st_Code) Or Not String.IsNullOrEmpty(st_Name) Then
            Dim dataSet As DataSet = New DataSet("Supplier")
            dataSet = Get_Supplier_Data(dataSet, DBConnectString.ConnectionString)
            SupplierList.DataSource = dataSet.Tables("SupplierList")
            SupplierList.DataBind()
        End If
    End Sub

    ' 仕入先リスト取得関数
    ' Public 変数の st_Code と st_Name を元に Supplier テーブルからデータを取得する。
    '
    ' [パラメータ]
    ' dataSet 取得したデータをセットする DataSet オブジェクト
    ' connectionString: 接続情報。文字列。
    '
    ' [戻り値]
    ' dataSet: 取得したデータをセットした DataSet オブジェクト

    Public Function Get_Supplier_Data(ByVal dataSet As DataSet, ByVal connectionString As String) As DataSet

        ' WHERE 分の分岐
        Dim st_where As String = " WHERE(SupplierCode = @Code) AND (ISNULL(Name1,'') + ' ' + ISNULL(Name2,'') LIKE N'%' + @Name + '%') "
        If Not String.IsNullOrEmpty(st_Code) And String.IsNullOrEmpty(st_Name) Then
            st_where = " WHERE(SupplierCode = @Code) "
        ElseIf String.IsNullOrEmpty(st_Code) And Not String.IsNullOrEmpty(st_Name) Then
            st_where = "  WHERE(ISNULL(Name1,'') + ' ' + ISNULL(Name2,'') LIKE N'%' + @Name + '%')  "
        End If

        ' 仕入先リスト取得
        Dim st_query As String = _
  " SELECT SupplierCode, Name3, Name4, CountryCode " _
& " FROM  Supplier " _
& st_where _
& " ORDER BY SupplierCode, Name3 "

        Try
            Using connection As New SqlClient.SqlConnection(connectionString)

                ' 接続情報、アダプタ、SQLコマンド オブジェクトの生成
                Dim adapter As New SqlClient.SqlDataAdapter()
                Dim command As New SqlClient.SqlCommand(st_query, connection)

                ' DataSet にテーブルとカラムを追加
                dataSet.Tables.Add("SupplierList")
                dataSet.Tables("SupplierList").Columns.Add("QuoLocationCode", Type.GetType("System.String"))

                ' SQL SELECT パラメータの追加
                command.Parameters.AddWithValue("Code", st_Code)
                command.Parameters.AddWithValue("Name", st_Name)

                ' データベースからデータを取得
                adapter.SelectCommand = command
                adapter.Fill(dataSet.Tables("SupplierList"))

            End Using
        Catch ex As Exception
            'Exception をスローする
            Throw
        End Try

        Return dataSet

    End Function

End Class

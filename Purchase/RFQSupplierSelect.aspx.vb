Partial Public Class RFQSupplierSelect
    Inherits CommonPage

    ' 接続文字列
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    Public st_Code As String
    Public st_Name As String
    Public st_Location As String
    Public st_Errorr_Meggage As String = ""
    Const DIRECT As String = "Direct"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' パラメータ Location を取得
        If Request.RequestType = "POST" Then
            st_Location = IIf(Request.Form("Location") = Nothing, "", Request.Form("Location"))
        ElseIf Request.RequestType = "GET" Then
            st_Location = IIf(Request.QueryString("Location") = Nothing, "", Request.QueryString("Location"))
        End If
        st_Location = Trim(st_Location)  ' 空白除去

        If String.IsNullOrEmpty(st_Location) Then
            ' パラメータ Location が無い場合はエラーメッセージを表示して終了。
            st_Errorr_Meggage = st_Errorr_Meggage & "見積依頼拠点コードが設定されていません。"
            ErrorMessages.Text = st_Errorr_Meggage
            Exit Sub
        Else

            ' パラメータ Code, Name を取得
            If Request.RequestType = "POST" Then
                st_Code = IIf(Request.Form("Code") = Nothing, "", Request.Form("Code"))
            ElseIf Request.RequestType = "GET" Then
                st_Code = IIf(Request.QueryString("Code") = Nothing, "", Request.QueryString("Code"))
            End If
            st_Name = Request.Form("Name")

            ' URL デコード
            st_Code = HttpUtility.UrlDecode(st_Code)
            st_Name = HttpUtility.UrlDecode(st_Name)

            ' 空白除去
            st_Code = Trim(st_Code)
            st_Name = Trim(st_Name)

            ' 検索ブロックの TextBox の値を書き換え
            Code.Text = st_Code
            Name.Text = st_Name
            Location.Value = st_Location

            ' 全角を半角に変換
            st_Code = StrConv(st_Code, VbStrConv.Narrow)

            ' 半角英数チェック
            If Not Regex.IsMatch(st_Code, "^[0-9]+$") Then
                st_Code = ""
            End If

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
        Dim st_where As String = " WHERE(Supplier.SupplierCode = @Code) AND (ISNULL(Supplier.Name1,'') + ' ' + ISNULL(Supplier.Name2,'') LIKE N'%' + @Name + '%') "
        If Not String.IsNullOrEmpty(st_Code) And String.IsNullOrEmpty(st_Name) Then
            st_where = " WHERE(Supplier.SupplierCode = @Code) "
        ElseIf String.IsNullOrEmpty(st_Code) And Not String.IsNullOrEmpty(st_Name) Then
            st_where = "  WHERE(ISNULL(Supplier.Name1,'') + ' ' + ISNULL(Supplier.Name2,'') LIKE N'%' + @Name + '%')  "
        End If

        ' 仕入先リスト取得
        ' [備考]
        ' LEFT OUTER JOIN で連結した際、IrregularRFQLocation.QuoLocationCode の値が
        ' レコードが存在していて NULL なのか、存在していなくて NULL なのかの判断ができないため、
        ' IrregularRFQLocation.SupplierCode を取得し、この値が NULL の場合はレコードが取得「できなかった」と判断する。
        Dim st_query As String = _
  "SELECT " _
& "  Supplier.SupplierCode, Supplier.R3SupplierCode, Supplier.Name3, Supplier.Name4, Supplier.CountryCode, " _
& "  IrregularRFQLocation.SupplierCode AS IrregularSupplierCode, " _
& " PurchasingCountry.DefaultQuoLocationCode, " _
& "  IrregularRFQLocation.QuoLocationCode AS IrregularQuoLocationCode " _
& "FROM  " _
& "  Supplier " _
& "    JOIN PurchasingCountry " _
& "      ON Supplier.CountryCode = PurchasingCountry.CountryCode " _
& "    LEFT OUTER JOIN IrregularRFQLocation  " _
& "      ON Supplier.SupplierCode = IrregularRFQLocation.SupplierCode  " _
& "         AND IrregularRFQLocation.EnqLocationCode = @Location " _
& st_where _
& "ORDER BY  " _
& "  Supplier.SupplierCode, Supplier.Name3 "

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
                command.Parameters.AddWithValue("Location", st_Location)

                ' データベースからデータを取得
                adapter.SelectCommand = command
                adapter.Fill(dataSet.Tables("SupplierList"))

                ' 見積回答拠点コード取得
                Dim i As Integer

                For i = 0 To dataSet.Tables("SupplierList").Rows.Count - 1
                    If IsDBNull(dataSet.Tables("SupplierList").Rows(i).Item("IrregularSupplierCode")) Then
                        dataSet.Tables("SupplierList").Rows(i).Item("QuoLocationCode") = dataSet.Tables("SupplierList").Rows(i).Item("DefaultQuoLocationCode")
                    Else
                        dataSet.Tables("SupplierList").Rows(i).Item("QuoLocationCode") = dataSet.Tables("SupplierList").Rows(i).Item("IrregularQuoLocationCode")
                    End If

                    If IsDBNull(dataSet.Tables("SupplierList").Rows(i).Item("QuoLocationCode")) Then
                        dataSet.Tables("SupplierList").Rows(i).Item("QuoLocationCode") = DIRECT
                    End If
                Next i
            End Using
        Catch ex As Exception
            'Exception をスローする
            Throw
        End Try

        Return dataSet

    End Function

End Class

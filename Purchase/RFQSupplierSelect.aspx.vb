Partial Public Class RFQSupplierSelect
    Inherits CommonPage

    Protected st_Code As String = String.Empty
    Protected st_Name As String = String.Empty
    Protected st_Location As String = String.Empty
    Protected st_js_postback = String.Empty ' do_Postback メソッドの取得

    Const MSG_REQUIED_EnqLocation = "見積依頼拠点コードが設定されていません。"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' コントロール初期化
        Msg.Text = ""

        ' パラメータを取得
        If Request.RequestType = "POST" Then
            st_Location = IIf(Request.Form("Location") = Nothing, "", Request.Form("Location"))
        ElseIf Request.RequestType = "GET" Then
            st_Location = IIf(Request.QueryString("Location") = Nothing, "", Request.QueryString("Location"))
        End If

        ' 空白除去
        st_Location = st_Location.Trim

        ' 見積依頼拠点が取得できない場合はエラーメッセージを表示して終了
        If String.IsNullOrEmpty(st_Location) Then
            Msg.Text = MSG_REQUIED_EnqLocation
            SearchForm.Visible = False
            Exit Sub
        End If

        ' パラメータを取得
        If Request.RequestType = "POST" Then
            st_Code = IIf(Request.Form("Code") = Nothing, "", Request.Form("Code"))
            st_Name = IIf(Request.Form("Name") = Nothing, "", Request.Form("Name"))
            ' 親画面から送信された ASP.NET が自動生成する JavaScript の関数を取得。
            ' この関数はポストバックを強制的に発生させる。
            ' 当プログラムでは、検索結果を親画面に渡した後に親画面の見積もり回答拠点のユーザ名プルダウンコントロールを更新するために用いている。
            If String.IsNullOrEmpty(Request.QueryString("Postback")) Then
                st_js_postback = "window.close();"
            Else
                st_js_postback = String.Format("window.opener.{0}; window.close(); return false;", HttpUtility.UrlDecode(Request.QueryString("Postback")))
            End If
        ElseIf Request.RequestType = "GET" Then
            st_Code = IIf(String.IsNullOrEmpty(Request.QueryString("Code")), "", Request.QueryString("Code"))
            If String.IsNullOrEmpty(Request.QueryString("Postback")) Then
                st_js_postback = "window.close();"
            Else
                st_js_postback = String.Format("window.opener.{0}; window.close(); return false;", HttpUtility.UrlDecode(Request.QueryString("Postback")))
            End If
        End If

        ' URL デコード
        st_Code = HttpUtility.UrlDecode(st_Code)
        st_Name = HttpUtility.UrlDecode(st_Name)

        ' 空白除去
        st_Code = st_Code.Trim
        st_Name = st_Name.Trim

        ' 全角を半角に変換
        st_Code = StrConv(st_Code, VbStrConv.Narrow)

        ' コントロール設定
        Code.Text = st_Code
        Name.Text = st_Name
        Location.Value = st_Location
        Postback.Value = Request.QueryString("Postback")

        ' GET 且つ QueryString("Code") が送信されている場合は検索処理を実行
        If (Request.RequestType = "GET") And (Not String.IsNullOrEmpty(Request.QueryString("Code"))) Then
            Dim dataSet As DataSet = New DataSet("Supplier")
            GetSupplierData(dataSet)
            SupplierList.DataSource = dataSet.Tables("SupplierList")
        End If
        SupplierList.DataBind()

    End Sub

    ' Search ボタンクリック処理
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        Dim dataSet As DataSet = New DataSet("Supplier")
        GetSupplierData(dataSet)
        SupplierList.DataSource = dataSet.Tables("SupplierList")
        SupplierList.DataBind()
    End Sub

    ' 仕入先リスト取得関数
    ' Public 変数の st_Code と st_Name を元に Supplier テーブルからデータを取得する。
    ' 
    '
    ' [パラメータ]
    ' ByRef dataSet: 取得したデータをセットする DataSet オブジェクト。
    '                SupplierList というデータテーブルが追加される。
    Private Sub GetSupplierData(ByRef ds As DataSet)

        ' パラメータチェック
        If Not String.IsNullOrEmpty(st_Code) Then
            If Not Regex.IsMatch(st_Code, "^[0-9]+$") Then
                st_Code = String.Empty
                SupplierList.DataBind()
                Exit Sub
            End If
        End If

        ' Where 句の生成
        Dim st_where As String = String.Empty
        If Not String.IsNullOrEmpty(st_Code) Then
            st_where = IIf(st_where.Length > 1, st_where & " AND ", "")
            st_where = st_where & " Supplier.SupplierCode = @Code "
        End If
        If Not String.IsNullOrEmpty(st_Name) Then
            st_where = IIf(st_where.Length > 1, st_where & " AND ", "")
            st_where = st_where & " ISNULL(Supplier.Name1,'') + ' ' + ISNULL(Supplier.Name2,'') LIKE N'%' + @Name + '%' "
        End If

        ' Where 句が生成できなかった場合は検索処理を行わずに処理を終了する
        If String.IsNullOrEmpty(st_where) Then
            SupplierList.DataBind()
            Exit Sub
        End If

        ' 仕入先リスト取得
        ' [備考]
        ' LEFT OUTER JOIN で連結した際、IrregularRFQLocation.QuoLocationCode の値が
        ' レコードが存在していて NULL なのか、存在していなくて NULL なのかの判断ができないため、
        ' IrregularRFQLocation.SupplierCode を取得し、この値が NULL の場合はレコードが取得「できなかった」と判断する。
        Dim st_query As String = _
               "SELECT " _
            & "  Supplier.SupplierCode, Supplier.R3SupplierCode, Supplier.CountryCode, " _
            & "  LTRIM(RTRIM(ISNULL(Supplier.Name3, '') + ' ' + ISNULL(Supplier.Name4, ''))) AS Name, " _
            & "  IrregularRFQLocation.SupplierCode AS IrregularSupplierCode, " _
            & "  PurchasingCountry.DefaultQuoLocationCode, " _
            & "  IrregularRFQLocation.QuoLocationCode AS IrregularQuoLocationCode, " _
            & "  s_Country.[Name] AS CountryName " _
            & "FROM " _
            & "  Supplier " _
            & "    INNER JOIN s_Country " _
            & "      ON Supplier.CountryCode = s_Country.CountryCode " _
            & "    INNER JOIN PurchasingCountry " _
            & "      ON Supplier.CountryCode = PurchasingCountry.CountryCode " _
            & "    LEFT OUTER JOIN IrregularRFQLocation " _
            & "      ON Supplier.SupplierCode = IrregularRFQLocation.SupplierCode " _
            & "         AND IrregularRFQLocation.EnqLocationCode = @Location " _
            & "WHERE " & st_where _
            & "ORDER BY " _
            & "  Supplier.SupplierCode, Supplier.Name3"

        Using connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)

            ' 接続情報、アダプタ、SQLコマンド オブジェクトの生成
            Dim adapter As New SqlClient.SqlDataAdapter()
            Dim command As New SqlClient.SqlCommand(st_query, connection)

            ' DataSet にテーブルとカラムを追加
            ds.Tables.Add("SupplierList")
            ds.Tables("SupplierList").Columns.Add("QuoLocationCode", Type.GetType("System.String"))

            ' SQL SELECT パラメータの追加
            command.Parameters.AddWithValue("Code", Common.SafeSqlLiteral(st_Code))
            command.Parameters.AddWithValue("Name", Common.SafeSqlLikeClauseLiteral(st_Name))
            command.Parameters.AddWithValue("Location", Common.SafeSqlLiteral(st_Location))

            ' データベースからデータを取得
            adapter.SelectCommand = command
            adapter.Fill(ds.Tables("SupplierList"))
        End Using

        ' 見積回答拠点コード取得
        For i As Integer = 0 To ds.Tables("SupplierList").Rows.Count - 1
            If IsDBNull(ds.Tables("SupplierList").Rows(i).Item("IrregularSupplierCode")) Then
                ds.Tables("SupplierList").Rows(i).Item("QuoLocationCode") = ds.Tables("SupplierList").Rows(i).Item("DefaultQuoLocationCode")
            Else
                ds.Tables("SupplierList").Rows(i).Item("QuoLocationCode") = ds.Tables("SupplierList").Rows(i).Item("IrregularQuoLocationCode")
            End If

            If IsDBNull(ds.Tables("SupplierList").Rows(i).Item("QuoLocationCode")) Then
                ds.Tables("SupplierList").Rows(i).Item("QuoLocationCode") = Common.DIRECT
            End If
        Next i
    End Sub

End Class

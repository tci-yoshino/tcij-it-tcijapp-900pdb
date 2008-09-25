Partial Public Class RFQSupplierSelect
    Inherits CommonPage

    Protected st_Code As String = String.Empty
    Protected st_Name As String = String.Empty
    Protected st_Location As String = String.Empty
    Protected st_js_postback = String.Empty ' do_Postback メソッドの取得

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
            Msg.Text = Common.ERR_INVALID_PARAMETER
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
            GetSupplierData()
        End If

    End Sub

    ' Search ボタンクリック処理
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        GetSupplierData()
    End Sub

    ' 仕入先リスト取得関数
    Private Sub GetSupplierData()

        ' パラメータチェック
        If Not String.IsNullOrEmpty(st_Code) Then
            If Not Regex.IsMatch(st_Code, "^[0-9]+$") Then
                st_Code = String.Empty
                SupplierList.DataBind()
                Exit Sub
            End If
        End If

        SrcSupplier.SelectParameters.Clear()
        SrcSupplier.SelectParameters.Add("Code", Common.SafeSqlLiteral(st_Code))
        SrcSupplier.SelectParameters.Add("Name", Common.SafeSqlLikeClauseLiteral(st_Name))
        SrcSupplier.SelectParameters.Add("Location", Common.SafeSqlLiteral(st_Location))
        SrcSupplier.SelectCommand = CreateSql_SelectSupplier()

    End Sub


    Private Function CreateSql_SelectSupplier() As String
        Dim sb_Sql As StringBuilder = New StringBuilder

        ' Where 句の生成
        Dim st_where As String = String.Empty
        If Not String.IsNullOrEmpty(st_Code) Then
            st_where = st_where & " AND S.SupplierCode = @Code "
        End If
        If Not String.IsNullOrEmpty(st_Name) Then
            st_where = st_where & " AND ISNULL(S.Name3,'') + ' ' + ISNULL(S.Name4,'') LIKE N'%' + @Name + '%' "
        End If

        ' Where 句が生成できなかった場合は検索処理を行わずに処理を終了する
        If String.IsNullOrEmpty(st_where) Then
            SupplierList.DataBind()
            Return ""
        End If

        sb_Sql.Append("SELECT ")
        sb_Sql.Append("  S.SupplierCode, ")
        sb_Sql.Append("  S.R3SupplierCode, ")
        sb_Sql.Append("  S.CountryCode, ")
        sb_Sql.Append("  LTRIM(RTRIM(ISNULL(S.Name3, '') + ' ' + ISNULL(S.Name4, ''))) AS [Name], ")
        sb_Sql.Append("  C.CountryName AS CountryName, ")
        sb_Sql.Append("  ISNULL(ISNULL(L.LocationCode, C.DefaultQuoLocationCode),'" & st_Location & "') AS QuoLocationCode, ")
        sb_Sql.Append("  ISNULL(L.[Name], C.DefaultQuoLocationName) AS QuoLocationName ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append("  Supplier AS S ")
        sb_Sql.Append("  LEFT OUTER JOIN IrregularRFQLocation AS IR ")
        sb_Sql.Append("    ON S.SupplierCode = IR.SupplierCode AND IR.EnqLocationCode = @Location ")
        sb_Sql.Append("  LEFT OUTER JOIN s_Location AS L ")
        sb_Sql.Append("    ON IR.QuoLocationCode = L.LocationCode, ")
        sb_Sql.Append("  v_Country AS C ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append("  S.CountryCode = C.CountryCode ")
        sb_Sql.Append(st_where)


        Return sb_Sql.ToString
    End Function

End Class

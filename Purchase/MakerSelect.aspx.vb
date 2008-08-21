Partial Public Class MakerSelect
    Inherits CommonPage

    ' 接続文字列
    Private DBConnectString As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Private st_Code As String = ""
    Private st_Name As String = ""
    Private st_Errorr_Meggage As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' パラメータを取得
        If Request.RequestType = "POST" Then
            st_Code = IIf(String.IsNullOrEmpty(Request.Form("Code")), "", Request.Form("Code"))
            st_Name = IIf(String.IsNullOrEmpty(Request.Form("Name")), "", Request.Form("Name"))
        ElseIf Request.RequestType = "GET" Then
            st_Code = IIf(String.IsNullOrEmpty(Request.QueryString("Code")), "", Request.QueryString("Code"))
            st_Name = IIf(String.IsNullOrEmpty(Request.Form("Name")), "", Request.Form("Name"))
        End If

        ' 空白除去
        st_Code = Trim(st_Code)
        st_Name = Trim(st_Name)

        ' URL デコード
        st_Code = HttpUtility.UrlDecode(st_Code)
        st_Name = HttpUtility.UrlDecode(st_Name)

        ' 検索ブロックの TextBox の値を書き換え
        Code.Text = st_Code
        Name.Text = st_Name

        ' 全角を半角に変換
        st_Code = StrConv(st_Code, VbStrConv.Narrow)

        ' 半角英数チェック
        If Not Regex.IsMatch(st_Code, "^[0-9]+$") Then
            st_Code = ""
        End If

        ' GET 且つ QueryString("Code") が送信されている場合は検索処理を実行
        If (Request.RequestType = "GET") And (Not String.IsNullOrEmpty(Request.QueryString("Code"))) Then
            Get_Supplier_Data()
        End If

    End Sub

    ' Search ボタンクリック処理
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        Get_Supplier_Data()
    End Sub

    ' 仕入先リスト取得関数
    Public Sub Get_Supplier_Data()

        SrcMaker.SelectParameters.Clear()

        ' Where 句の生成
        Dim st_where As String = ""
        If Not String.IsNullOrEmpty(st_Code) Then
            SrcMaker.SelectParameters.Add("Code", Common.SafeSqlLiteral(st_Code))
            st_where = IIf(st_where.Length > 1, st_where & " AND ", "")
            st_where = st_where & " Supplier.SupplierCode = @Code "
        End If
        If Not String.IsNullOrEmpty(st_Name) Then
            SrcMaker.SelectParameters.Add("Name", Common.SafeSqlLikeClauseLiteral(st_Name))
            st_where = IIf(st_where.Length > 1, st_where & " AND ", "")
            st_where = st_where & " ISNULL(Supplier.Name1,'') + ' ' + ISNULL(Supplier.Name2,'') LIKE N'%' + @Name + '%' "
        End If

        ' Where 句が生成できなかった場合は検索処理を行わずに処理を終了する
        If String.IsNullOrEmpty(st_where) Then
            SupplierList.DataBind()
            Exit Sub
        End If

        ' 仕入先リスト取得
        SrcMaker.SelectCommand = _
              " SELECT SupplierCode, Name3, Name4, s_Country.[Name] AS CountryName " _
            & " FROM  Supplier " _
            & "   LEFT OUTER JOIN s_Country " _
            & "   ON s_Country.CountryCode = Supplier.CountryCode " _
            & " WHERE " & st_where _
            & " ORDER BY SupplierCode, Name3 "

    End Sub

End Class

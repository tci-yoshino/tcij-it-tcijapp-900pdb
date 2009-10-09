Partial Public Class UserSelect
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader

    Private st_UserID As String = String.Empty
    'Private st_Name As String = String.Empty
    'Const SEARCH_ACTION As String = "Search"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '[DBの接続]-----------------------------------------------------------------------
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        '[Msgのクリア]---------------------------------------------------------------------
        Msg.Text = ""

        If IsPostBack = False Then
            '[パラメータ取得]------------------------------------------------------------------
            If Request.RequestType = "POST" Then
                st_UserID = IIf(Request.Form("UserID") = Nothing, "", Request.Form("UserID"))
            ElseIf Request.RequestType = "GET" Then
                st_UserID = IIf(Request.QueryString("UserID") = Nothing, "", Request.QueryString("UserID"))
            End If

            '[パラメータ正規化]--------------------------------------------------------------------
            st_UserID = st_UserID.Trim

            '[URL デコード]------------------------------------------------------------------------
            st_UserID = HttpUtility.UrlDecode(st_UserID)

            '[全角を半角に変換]--------------------------------------------------------------------
            st_UserID = StrConv(st_UserID, VbStrConv.Narrow)
        End If

        '[LocationCode 設定]-----------------------------------------------------------------------
        DBCommand.CommandText = "SELECT Name FROM s_Location ORDER BY Name"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        LocationName.Items.Clear()
        Do Until DBReader.Read = False
            LocationName.Items.Add(DBReader("Name"))
        Loop
        DBReader.Close()

        DBCommand.CommandText = "SELECT LocationName,AD_DeptName,Name,AD_DisplayName FROM v_UserAll WHERE UserID='" & st_UserID & "'"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.Read = True Then
            LocationName.SelectedValue = DBReader("LocationName")
            DeptName.Text = DBReader("AD_DeptName")
            UserName.Text = DBReader("Name")
        End If
        DBReader.Close()


        '' コントロール設定
        ''LocationName.Text = st_Code
        'UserName.Text = st_Name

        '' GET 且つ QueryString("Code") が空ではない場合は検索処理を実行
        'If (Request.RequestType = "GET") And (Not String.IsNullOrEmpty(Request.QueryString("Code"))) Then
        '    SearchCountryList()
        'End If

    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Search.Click

        'Dim st_Action As String = String.Empty

        'If Request.Form("Action") = Nothing Then
        '    st_Action = IIf(Request.QueryString("Action") = Nothing, String.Empty, Request.QueryString("Action")).ToString
        'Else
        '    st_Action = Request.Form("Action").ToString
        'End If

        'If st_Action = SEARCH_ACTION Then
        '    SearchCountryList()
        'End If

    End Sub

    ' 検索処理
    Protected Sub SearchCountryList()

        'Dim st_Where As String = String.Empty
        'SrcUser.SelectParameters.Clear()

        '' Where 句の生成
        'If Not String.IsNullOrEmpty(st_Code) Then
        '    SrcUser.SelectParameters.Add("CountryCode", st_Code)
        '    st_Where = IIf(st_Where.Length > 1, st_Where & " AND ", st_Where)
        '    st_Where = st_Where & " CountryCode = @CountryCode "
        'End If

        'If Not String.IsNullOrEmpty(st_Name) Then
        '    SrcUser.SelectParameters.Add("CountryName", Common.SafeSqlLikeClauseLiteral(st_Name))
        '    st_Where = IIf(st_Where.Length > 1, st_Where & " AND ", st_Where)
        '    st_Where = st_Where & " [Name] LIKE N'%' + @CountryName + '%' "
        'End If

        '' Where 句が生成できなかった場合は処理終了
        'If String.IsNullOrEmpty(st_Where) Then
        '    Exit Sub
        'End If

        'SrcUser.SelectCommand = _
        '      " SELECT [CountryCode], [Name] " _
        '    & " FROM [s_Country] " _
        '    & " WHERE " & st_Where _
        '    & " ORDER BY CountryCode, [Name] ASC"

    End Sub

End Class



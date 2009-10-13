Partial Public Class UserSelect
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader

    Private st_UserID As String = String.Empty
    Private st_LocationName As String = String.Empty
    'Const SEARCH_ACTION As String = "Search"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
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

            '[LocationCode 設定]-------------------------------------------------------------------
            DBCommand = DBConn.CreateCommand()
            DBCommand.CommandText = "SELECT Name FROM s_Location ORDER BY Name"
            DBConn.Open()
            DBReader = DBCommand.ExecuteReader()
            LocationName.Items.Clear()
            LocationName.Items.Add("")
            Do Until DBReader.Read = False
                LocationName.Items.Add(DBReader("Name"))
            Loop
            DBConn.Close()

            '[データ表示]--------------------------------------------------------------------------
            If st_UserID <> String.Empty Then
                DBCommand = DBConn.CreateCommand()
                DBCommand.CommandText = "SELECT LocationName,AD_DeptName,AD_DisplayName FROM v_UserAll WHERE UserID='" & st_UserID & "'"
                DBConn.Open()
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    LocationName.SelectedValue = DBReader("LocationName")
                    DeptName.Text = DBReader("AD_DeptName")
                    UserName.Text = DBReader("AD_DisplayName")
                End If
                DBReader.Close()
            End If

            '[検索データ表示]----------------------------------------------------------------------
            SearchCountryList()
        End If
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Search.Click
        SearchCountryList()
    End Sub

    Protected Sub SearchCountryList()
        Dim st_SQL As New Text.StringBuilder
        Dim st_WHERE As String = String.Empty
        SrcUser.SelectParameters.Clear()
        If LocationName.Text = String.Empty And DeptName.Text = String.Empty And UserName.Text = String.Empty Then
            Exit Sub
        Else
            st_SQL.Remove(0, st_SQL.Length)
            st_SQL.Append("SELECT")
            st_SQL.Append(" UserID, ")
            st_SQL.Append(" LocationName, ")
            st_SQL.Append(" AccountName, ")
            st_SQL.Append(" AD_DeptName, ")
            st_SQL.Append(" AD_DisplayName, ")
            st_SQL.Append(" Name ")
            st_SQL.Append("FROM ")
            st_SQL.Append(" v_UserAll ")
            st_SQL.Append("WHERE ")
            If LocationName.Text <> String.Empty Then
                st_WHERE = st_WHERE + "LocationName='" + LocationName.Text + "'"
            End If
            If DeptName.Text <> String.Empty Then
                If st_WHERE <> String.Empty Then st_WHERE = st_WHERE + " AND "
                st_WHERE = st_WHERE + "AD_DeptName LIKE '%" + DeptName.Text + "%'"
            End If
            If UserName.Text <> String.Empty Then
                If st_WHERE <> String.Empty Then st_WHERE = st_WHERE + " AND "
                st_WHERE = st_WHERE + "AD_DisplayName LIKE '%" + UserName.Text + "%'"
            End If
            st_SQL.Append("" + st_WHERE + "")
            SrcUser.SelectCommand = st_SQL.ToString
        End If
    End Sub

End Class



Imports Purchase.Common

Partial Public Class UserSelect
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader

    Private st_UserID As String = String.Empty
    Private st_LocationName As String = String.Empty
    Const SEARCH_ACTION As String = "Search"

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
                DBCommand.CommandText = "SELECT s_Location.Name AS LocationName,AD_DeptName,AD_DisplayName FROM s_User INNER JOIN s_Location ON s_User.LocationCode = s_Location.LocationCode WHERE CAST(UserID AS varchar)='" & st_UserID & "'"
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
            SearchUserList()
        End If
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Search.Click

        '[権限データ投入まで有効化しないこと（開発動作に支障）]
        'Dim st_Action As String = String.Empty

        'If Request.Form("Action") = Nothing Then
        '    st_Action = IIf(Request.QueryString("Action") = Nothing, String.Empty, Request.QueryString("Action")).ToString
        'Else
        '    st_Action = Request.Form("Action").ToString
        'End If

        'If st_Action = SEARCH_ACTION Then
        SearchUserList()
        'End If

    End Sub

    Protected Sub SearchUserList()
        Dim st_SQL As New Text.StringBuilder
        Dim st_WHERE As String = String.Empty
        SrcUser.SelectParameters.Clear()
        If LocationName.Text = String.Empty And DeptName.Text = String.Empty And UserName.Text = String.Empty Then
            Exit Sub
        Else
            st_SQL.Remove(0, st_SQL.Length)
            st_SQL.Append("SELECT")
            st_SQL.Append(" UserID, ")
            st_SQL.Append(" SL.Name AS LocationName, ")
            st_SQL.Append(" AD_AccountName, ")
            st_SQL.Append(" AD_DeptName, ")
            st_SQL.Append(" AD_DisplayName, ")
            st_SQL.Append(" LTRIM(RTRIM(ISNULL(SU.AD_GivenName, '') + ' ' + ISNULL(SU.AD_Surname, ''))) AS Name ")
            st_SQL.Append("FROM ")
            st_SQL.Append(" s_User AS SU ")
            st_SQL.Append(" INNER JOIN s_Location AS SL ON SU.LocationCode = SL.LocationCode ")
            st_SQL.Append("WHERE ")
            st_SQL.Append(" AD_AccountName<>'' ")
            If LocationName.Text <> String.Empty Then
                st_WHERE = st_WHERE + "SL.Name='" + LocationName.Text + "'"
            End If
            If DeptName.Text <> String.Empty Then
                If st_WHERE <> String.Empty Then st_WHERE = st_WHERE + " AND "
                st_WHERE = st_WHERE + "AD_DeptName LIKE '%" + DeptName.Text + "%'"
            End If
            If UserName.Text <> String.Empty Then
                If st_WHERE <> String.Empty Then st_WHERE = st_WHERE + " AND "
                st_WHERE = st_WHERE + "AD_DisplayName LIKE '%" + UserName.Text + "%'"
            End If
            If st_WHERE <> String.Empty Then
                st_SQL.Append(" AND " + st_WHERE + "")
            End If
            SrcUser.SelectCommand = st_SQL.ToString
        End If
    End Sub
End Class
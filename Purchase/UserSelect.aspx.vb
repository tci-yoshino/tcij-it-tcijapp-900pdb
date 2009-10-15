Option Explicit On
Option Infer Off
Option Strict On

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
        Msg.Text = String.Empty

        If IsPostBack = False Then
            '[パラメータ取得]----------------------------------------------------------------------
            st_UserID = Common.GetQuery(Request, "UserID")

            '[パラメータ正規化]--------------------------------------------------------------------
            st_UserID = st_UserID.Trim

            '[URL デコード]------------------------------------------------------------------------
            st_UserID = HttpUtility.UrlDecode(st_UserID)

            '[全角を半角に変換]--------------------------------------------------------------------
            st_UserID = StrConv(st_UserID, VbStrConv.Narrow)

            '[LocationCode 設定]-------------------------------------------------------------------
            Try
                DBCommand = DBConn.CreateCommand()
                DBCommand.CommandText = "SELECT Name FROM s_Location ORDER BY Name"
                DBConn.Open()
                DBReader = DBCommand.ExecuteReader()
                LocationName.Items.Clear()
                LocationName.Items.Add(String.Empty)
                Do Until DBReader.Read = False
                    LocationName.Items.Add(DBReader("Name").ToString)
                Loop
            Finally
                DBReader.Close()
                DBConn.Close()
            End Try

            '[データ表示]--------------------------------------------------------------------------
            If st_UserID <> String.Empty Then
                Try
                    DBCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = "SELECT s_Location.Name AS LocationName,AD_DeptName,AD_DisplayName FROM s_User INNER JOIN s_Location ON s_User.LocationCode = s_Location.LocationCode WHERE CAST(UserID AS varchar)='" & st_UserID & "'"
                    DBConn.Open()
                    DBReader = DBCommand.ExecuteReader()
                    If DBReader.Read = True Then
                        LocationName.SelectedValue = CStr(DBReader("LocationName"))
                        DeptName.Text = CStr(DBReader("AD_DeptName"))
                        UserName.Text = CStr(DBReader("AD_DisplayName"))
                    End If
                Finally
                    DBReader.Close()
                    DBConn.Close()
                End Try
            End If

            '[検索データ表示]----------------------------------------------------------------------
            SearchUserList()
        End If
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Search.Click
        '[Action取得]--------------------------------------------------------------------------
        If Common.GetAction(Request) = SEARCH_ACTION Then
            SearchUserList()
        End If
    End Sub

    Protected Sub SearchUserList()
        Dim st_SQL As New Text.StringBuilder
        SrcUser.SelectParameters.Clear()
        If LocationName.Text = String.Empty And DeptName.Text = String.Empty And UserName.Text = String.Empty Then
            Exit Sub
        End If
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
            st_SQL.Append("AND SL.Name='" & LocationName.Text & "' ")
        End If
        st_SQL.Append("AND AD_DeptName LIKE '%" & DeptName.Text & "%' AND AD_DisplayName LIKE '%" & UserName.Text & "%'")
        SrcUser.SelectCommand = st_SQL.ToString
    End Sub
End Class
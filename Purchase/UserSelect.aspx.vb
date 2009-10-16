Option Explicit On
Option Infer Off
Option Strict On

Imports System.Data.SqlClient

Partial Public Class UserSelect
    Inherits CommonPage

    Const SEARCH_ACTION As String = "Search"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim st_UserID As String = String.Empty

        '[Msgのクリア]---------------------------------------------------------------------
        Msg.Text = String.Empty

        If IsPostBack = False Then
            '[パラメータ取得]--------------------------------------------------------------
            st_UserID = Common.GetQuery(Request, "UserID")

            '[パラメータトリム]------------------------------------------------------------
            st_UserID = st_UserID.Trim

            '[URL デコード]----------------------------------------------------------------
            st_UserID = HttpUtility.UrlDecode(st_UserID)

            '[全角を半角に変換]------------------------------------------------------------
            st_UserID = StrConv(st_UserID, VbStrConv.Narrow)

            '[LocationCode 設定]-----------------------------------------------------------
            Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                Dim DBCommand As SqlCommand = DBConn.CreateCommand()
                DBCommand.CommandText = "SELECT Name FROM s_Location ORDER BY Name"
                DBConn.Open()
                Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
                LocationName.Items.Clear()
                LocationName.Items.Add(String.Empty)
                Do Until DBReader.Read = False
                    LocationName.Items.Add(DBReader("Name").ToString)
                Loop
                DBReader.Close()
            End Using

            '[テキストボックス等のデータ表示]----------------------------------------------
            If st_UserID <> String.Empty Then
                Dim st_SQL As String = String.Empty
                st_SQL = String.Empty
                st_SQL &= "SELECT "
                st_SQL &= " s_Location.Name AS LocationName, "
                st_SQL &= " AD_DeptName,AD_DisplayName "
                st_SQL &= "FROM "
                st_SQL &= " s_User INNER JOIN s_Location ON s_User.LocationCode = s_Location.LocationCode "
                st_SQL &= "WHERE "
                st_SQL &= " CAST(UserID AS varchar)='" & st_UserID & "'"

                Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                    Dim DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = st_SQL
                    DBConn.Open()
                    Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
                    If DBReader.Read = True Then
                        LocationName.SelectedValue = CStr(DBReader("LocationName"))
                        DeptName.Text = CStr(DBReader("AD_DeptName"))
                        UserName.Text = CStr(DBReader("AD_DisplayName"))
                    End If
                    DBReader.Close()
                End Using
            End If

            '[検索データ表示]--------------------------------------------------------------
            SearchUserList()
        End If
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Search.Click
        '[Action取得]----------------------------------------------------------------------
        If Common.GetAction(Request) = SEARCH_ACTION Then
            SearchUserList()
        End If
    End Sub

    Protected Sub SearchUserList()
        SrcUser.SelectParameters.Clear()
        If LocationName.Text = String.Empty And DeptName.Text = String.Empty And UserName.Text = String.Empty Then
            Exit Sub
        End If
        Dim st_SQL As String = String.Empty
        st_SQL &= "SELECT "
        st_SQL &= " UserID, "
        st_SQL &= " SL.Name AS LocationName, "
        st_SQL &= " AD_AccountName, "
        st_SQL &= " AD_DeptName, "
        st_SQL &= " AD_DisplayName, "
        st_SQL &= " LTRIM(RTRIM(ISNULL(SU.AD_GivenName, '') + ' ' + ISNULL(SU.AD_Surname, ''))) AS Name "
        st_SQL &= "FROM "
        st_SQL &= " s_User AS SU "
        st_SQL &= " INNER JOIN s_Location AS SL ON SU.LocationCode = SL.LocationCode "
        st_SQL &= "WHERE "
        st_SQL &= " AD_AccountName<>'' "
        If LocationName.Text <> String.Empty Then
            st_SQL &= "AND SL.Name='" & LocationName.Text & "' "
        End If
        st_SQL &= "AND AD_DeptName LIKE '%" & DeptName.Text & "%' AND AD_DisplayName LIKE '%" & UserName.Text & "%'"
        SrcUser.SelectCommand = st_SQL.ToString
    End Sub
End Class
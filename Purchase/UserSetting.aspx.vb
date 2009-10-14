Imports System.Data.SqlClient
Imports System.Text

Partial Public Class UserSetting
    Inherits CommonPage

    Dim DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As SqlCommand
    Dim DBReader As SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '[Msgのクリア]---------------------------------------------------------------------
        Msg.Text = ""

        If IsPostBack = False Then
            '[Role Code 設定]------------------------------------------------------------------
            DBCommand = DBConn.CreateCommand()
            DBCommand.CommandText = "SELECT RoleCode FROM Role ORDER BY RoleCode"
            DBConn.Open()
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            RoleCode.Items.Clear()
            Do Until DBReader.Read = False
                RoleCode.Items.Add(DBReader("RoleCode"))
            Loop
            DBReader.Close()
            DBConn.Close()
            RoleCode.SelectedValue = "WRITE"

            '[Privilege Label 設定]------------------------------------------------------------
            PrivilegeLevel.Items.Clear()
            PrivilegeLevel.Items.Add("P")
            PrivilegeLevel.Items.Add("A")

            '[処理(登録/修正)の判断]-------------------------------------------------------
            If Request.QueryString("Action") = "Edit" Then
                UserID.Text = Request.QueryString("UserID")
                Search.Visible = False
                DBCommand = DBConn.CreateCommand()
                DBCommand.CommandText = "SELECT UserID,LocationName,AccountName,""Name"",RoleCode,PrivilegeLevel,isAdmin,isDisabled, CONVERT(VARCHAR,UpdateDate,120) AS UpdateDate " & _
                                        "FROM v_UserAll WHERE UserID=" & UserID.Text
                DBConn.Open()
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    Location.Text = DBReader("LocationName")
                    AccountName.Text = DBReader("AccountName")
                    Name.Text = DBReader("Name")
                    RoleCode.SelectedValue = DBReader("RoleCode")
                    PrivilegeLevel.Text = DBReader("PrivilegeLevel")
                    isAdmin.Checked = DBReader("isAdmin")
                    isDisabled.Checked = DBReader("isDisAbled")
                    '[HiddenField設定]
                    UpdateDate.Value = DBReader("UpdateDate").ToString()
                End If
                DBReader.Close()
                DBConn.Close()
            Else
                UserID.CssClass = String.Empty
                UserID.ReadOnly = False
            End If
        End If
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        If Request.Form("Action") <> "Save" Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        If Common.IsInteger(UserID.Text) = False Then
            Msg.Text = "User ID" & Common.ERR_INVALID_NUMBER
            Exit Sub
        End If

        If Common.ExistenceConfirmation("s_User", "UserID", UserID.Text) = False Then
            Msg.Text = "User ID" & Common.ERR_DOES_NOT_EXIST
            Exit Sub
        End If

        Dim st_SQL As String = String.Empty
        DBCommand = DBConn.CreateCommand()
        DBCommand.CommandText = "SELECT UserID FROM PurchasingUser WHERE UserID='" + UserID.Text + "'"
        DBConn.Open()
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.Read = False Then
            st_SQL = "INSERT INTO PurchasingUser "
            st_SQL = st_SQL + "(UserID,"
            st_SQL = st_SQL + "RoleCode,"
            st_SQL = st_SQL + "PrivilegeLevel,"
            st_SQL = st_SQL + "isAdmin,"
            st_SQL = st_SQL + "isDisabled,"
            st_SQL = st_SQL + "CreatedBy,"
            st_SQL = st_SQL + "CreateDate,"
            st_SQL = st_SQL + "UpdatedBy,"
            st_SQL = st_SQL + "UpdateDate) "
            st_SQL = st_SQL + "VALUES "
            st_SQL = st_SQL + "('" + Common.SafeSqlLiteral(UCase(UserID.Text)) + "','"
            st_SQL = st_SQL + RoleCode.Text + "','"
            st_SQL = st_SQL + PrivilegeLevel.Text + "',"
            st_SQL = st_SQL + ConvertBoolToSQLString(isAdmin.Checked) + ","
            st_SQL = st_SQL + ConvertBoolToSQLString(isDisabled.Checked) + ","
            st_SQL = st_SQL + Session("UserID") + ","
            st_SQL = st_SQL + "GetDate(),"
            st_SQL = st_SQL + Session("UserID") + ","
            st_SQL = st_SQL + "GetDate())"
        Else
            If Common.IsLatestData("PurchasingUser", "UserID", Session("UserID").ToString(), UpdateDate.Value) = False Then
                Msg.Text = Common.ERR_UPDATED_BY_ANOTHER_USER
                Exit Sub
            End If
            st_SQL = "UPDATE PurchasingUser SET "
            st_SQL = st_SQL + "RoleCode='" + RoleCode.Text + "', "
            st_SQL = st_SQL + "PrivilegeLevel='" + PrivilegeLevel.Text + "', "
            st_SQL = st_SQL + "isAdmin=" + ConvertBoolToSQLString(isAdmin.Checked) + ", "
            st_SQL = st_SQL + "isDisAbled=" + ConvertBoolToSQLString(isDisabled.Checked) + ", "
            st_SQL = st_SQL + "UpdatedBy=" + Session("UserID") + ", "
            st_SQL = st_SQL + "UpdateDate=GetDate() "
            st_SQL = st_SQL + "WHERE UserID='" + UserID.Text + "'"
        End If
        DBReader.Close()
        DBConn.Close()

        DBCommand = DBConn.CreateCommand()
        DBCommand.CommandText = st_SQL
        DBConn.Open()
        DBCommand.ExecuteNonQuery()
        DBConn.Close()

        '[呼出元のフォームに戻る]----------------------------------------------------------
        If Msg.Text.ToString = "" Then
            Response.Redirect("UserList.aspx")
        End If
    End Sub

    Private Function ConvertBoolToSQLString(ByVal value As Boolean) As String
        If value = True Then
            Return "1"
        Else
            Return "0"
        End If
    End Function

End Class
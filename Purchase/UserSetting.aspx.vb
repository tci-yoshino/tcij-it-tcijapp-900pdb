Option Explicit On
Option Infer Off
Option Strict On

Imports System.Data.SqlClient

Partial Public Class UserSetting
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '[Msgのクリア]---------------------------------------------------------------------
        Msg.Text = String.Empty

        If IsPostBack = False Then
            '[Role Code 設定]------------------------------------------------------------------
            Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                Dim DBCommand As SqlCommand = DBConn.CreateCommand()
                DBCommand.CommandText = "SELECT RoleCode FROM Role ORDER BY RoleCode"
                DBConn.Open()
                Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
                RoleCode.Items.Clear()
                Do Until DBReader.Read = False
                    RoleCode.Items.Add(DBReader("RoleCode").ToString)
                Loop
            End Using
            RoleCode.SelectedValue = "WRITE"

            '[Privilege Label 設定]------------------------------------------------------------
            PrivilegeLevel.Items.Clear()
            PrivilegeLevel.Items.Add("P")
            PrivilegeLevel.Items.Add("A")

            '[Action=Edit時、選択データ表示]---------------------------------------------------
            If Request.QueryString("Action") = "Edit" Then
                UserID.Text = Request.QueryString("UserID")
                Search.Visible = False

                Dim st_SQL As String = String.Empty
                st_SQL &= "SELECT "
                st_SQL &= " UserID, "
                st_SQL &= " LocationName, "
                st_SQL &= " AccountName, "
                st_SQL &= """Name"", "
                st_SQL &= " RoleCode, "
                st_SQL &= " PrivilegeLevel, "
                st_SQL &= "isAdmin, "
                st_SQL &= " isDisabled, "
                st_SQL &= " CONVERT(VARCHAR,UpdateDate,120) AS UpdateDate "
                st_SQL &= "FROM "
                st_SQL &= " v_UserAll "
                st_SQL &= "WHERE "
                st_SQL &= "UserID = "
                st_SQL &= UserID.Text

                Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                    Dim DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = st_SQL
                    DBConn.Open()
                    Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
                    If DBReader.Read = True Then
                        Location.Text = CStr(DBReader("LocationName"))
                        AccountName.Text = CStr(DBReader("AccountName"))
                        Name.Text = CStr(DBReader("Name"))
                        RoleCode.SelectedValue = CStr(DBReader("RoleCode"))
                        PrivilegeLevel.Text = CStr(DBReader("PrivilegeLevel"))
                        isAdmin.Checked = CBool(DBReader("isAdmin"))
                        isDisabled.Checked = CBool(DBReader("isDisAbled"))
                        '[HiddenField設定]
                        UpdateDate.Value = DBReader("UpdateDate").ToString()
                    Else
                        Msg.Text = Common.MSG_NO_DATA_FOUND
                        Exit Sub
                    End If
                End Using
            Else
                UserID.CssClass = String.Empty
                UserID.ReadOnly = False
            End If
        End If
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        '[Actionのチェック]----------------------------------------------------------------
        If Request.Form("Action") <> "Save" Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        '[入力UserIDの数値チェック]--------------------------------------------------------
        If Common.IsInteger(UserID.Text) = False Then
            Msg.Text = "User ID" & Common.ERR_INVALID_NUMBER
            Exit Sub
        End If

        '[入力UserIDがs_Userに存在するかチェック]------------------------------------------
        If Common.ExistenceConfirmation("s_User", "UserID", UserID.Text) = False Then
            Msg.Text = "User ID" & Common.ERR_DOES_NOT_EXIST
            Exit Sub
        End If

        Dim st_SQL As String = String.Empty
        If Request.QueryString("Action") = "Edit" Then
            '[Action=Edit処理]-------------------------------------------------------------
            If Common.ExistenceConfirmation("PurchasingUser", "UserID", UserID.Text) = False Then    '[入力UserIDのPurchasingUser存在有無]
                Msg.Text = Common.ERR_DELETED_BY_ANOTHER_USER
                Exit Sub
            End If
            If Common.IsLatestData("PurchasingUser", "UserID", UserID.Text, UpdateDate.Value) = False Then
                Msg.Text = Common.ERR_UPDATED_BY_ANOTHER_USER
                Exit Sub
            End If
            st_SQL &= "UPDATE PurchasingUser SET "
            st_SQL &= "RoleCode='" & RoleCode.Text & "', "
            st_SQL &= "PrivilegeLevel='" & PrivilegeLevel.Text & "', "
            st_SQL &= "isAdmin=" & ConvertBoolToSQLString(isAdmin.Checked) & ", "
            st_SQL &= "isDisAbled=" & ConvertBoolToSQLString(isDisabled.Checked) & ", "
            st_SQL &= "UpdatedBy=" & Session("UserID").ToString & ", "
            st_SQL &= "UpdateDate=GetDate() "
            st_SQL &= "WHERE UserID='" & UserID.Text & "'"

        ElseIf Request.QueryString("Action") = Nothing Then
            '[Action=Nothing処理]----------------------------------------------------------
            If Common.ExistenceConfirmation("PurchasingUser", "UserID", UserID.Text) = True Then   '[入力UserIDのPurchasingUser存在有無]
                Msg.Text = "Your requested User ID already exist.<br />(Please check again to avoid duplication.)"
                Exit Sub
            End If
            st_SQL &= "INSERT INTO PurchasingUser "
            st_SQL &= "(UserID,"
            st_SQL &= "RoleCode,"
            st_SQL &= "PrivilegeLevel,"
            st_SQL &= "isAdmin,"
            st_SQL &= "isDisabled,"
            st_SQL &= "CreatedBy,"
            st_SQL &= "CreateDate,"
            st_SQL &= "UpdatedBy,"
            st_SQL &= "UpdateDate) "
            st_SQL &= "VALUES "
            st_SQL &= "(" & Common.SafeSqlLiteral(UserID.Text) & ",'"
            st_SQL &= RoleCode.Text & "','"
            st_SQL &= PrivilegeLevel.Text & "',"
            st_SQL &= ConvertBoolToSQLString(isAdmin.Checked) & ","
            st_SQL &= ConvertBoolToSQLString(isDisabled.Checked) & ","
            st_SQL &= Session("UserID").ToString & ","
            st_SQL &= "GetDate(),"
            st_SQL &= Session("UserID").ToString & ","
            st_SQL &= "GetDate())"
        Else
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        '[処理の実行]------------------------------------------------------------------
        Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
            Dim DBCommand As SqlCommand = DBConn.CreateCommand()
            DBCommand.CommandText = st_SQL
            DBConn.Open()
            DBCommand.ExecuteNonQuery()
        End Using

        '[呼出元のフォームに戻る]----------------------------------------------------------
        If Msg.Text.ToString = String.Empty Then
            Response.Redirect("UserList.aspx")
        End If
    End Sub

    Private Function ConvertBoolToSQLString(ByVal value As Boolean) As String
        '[isAdmin,isDisAbledの値True,Falseをそれぞれ1,0にする]-----------------------------
        If value = True Then
            Return "1"
        Else
            Return "0"
        End If
    End Function

End Class
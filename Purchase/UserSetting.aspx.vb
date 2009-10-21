Option Explicit On
Option Infer Off
Option Strict On

Imports System.Data.SqlClient

Partial Public Class UserSetting
    Inherits CommonPage

    Const SAVE_ACTION As String = "Save"
    Const EDIT_ACTION As String = "Edit"
    Const ALREADY_EXIST As String = "Your requested User ID already exist.<br />(Please check again to avoid duplication.)"

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
                DBReader.Close()
            End Using
            RoleCode.SelectedValue = "WRITE"

            '[Privilege Label 設定]------------------------------------------------------------
            PrivilegeLevel.Items.Clear()
            For i As Integer = 0 To Common.PRIVILEGE_LEVEL.Length - 1
                PrivilegeLevel.Items.Add(Common.PRIVILEGE_LEVEL(i))
            Next

            '[Actionの記憶]--------------------------------------------------------------------
            Mode.Value = Common.GetHttpAction(Request)

            '[Action=Edit時、選択データ表示]---------------------------------------------------
            If Common.GetHttpAction(Request) = EDIT_ACTION Then
                UserID.Text = Common.GetHttpQuery(Request, "UserID")
                Search.Visible = False

                '[UserID数値以外エラー]--------------------------------------------------------
                If Common.IsInteger(UserID.Text) = False Or UserID.Text.Length = 0 Then
                    Msg.Text = Common.ERR_INVALID_PARAMETER
                    Exit Sub
                End If

                Dim st_SQL As String = String.Empty
                st_SQL &= "SELECT "
                st_SQL &= " UserID, "
                st_SQL &= " LocationName, "
                st_SQL &= " AccountName, "
                st_SQL &= "[Name], "
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
                    DBReader.Close()
                End Using
            Else
                UserID.CssClass = String.Empty
                UserID.ReadOnly = False
            End If
        End If
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        '[Actionのチェック]----------------------------------------------------------------
        If Common.GetHttpAction(Request) <> SAVE_ACTION Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        '[UserIDの入力チェック]------------------------------------------------------------
        If UserID.Text.Length = 0 Then
            Msg.Text = "User ID " & Common.ERR_REQUIRED_FIELD
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
        If Common.GetHttpQuery(Request, "Mode") = EDIT_ACTION Then
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
            st_SQL &= "isAdmin=" & Common.ConvertBoolToInt(isAdmin.Checked) & ", "
            st_SQL &= "isDisAbled=" & Common.ConvertBoolToInt(isDisabled.Checked) & ", "
            st_SQL &= "UpdatedBy=" & Session("UserID").ToString & ", "
            st_SQL &= "UpdateDate=GetDate() "
            st_SQL &= "WHERE UserID='" & UserID.Text & "'"

        ElseIf Mode.Value = String.Empty Then
            '[Action=Nothing処理]----------------------------------------------------------
            If Common.ExistenceConfirmation("PurchasingUser", "UserID", UserID.Text) = True Then   '[入力UserIDのPurchasingUser存在有無]
                Msg.Text = ALREADY_EXIST
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
            st_SQL &= Common.ConvertBoolToInt(isAdmin.Checked) & ","
            st_SQL &= Common.ConvertBoolToInt(isDisabled.Checked) & ","
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
        Response.Redirect("UserList.aspx")
    End Sub
End Class
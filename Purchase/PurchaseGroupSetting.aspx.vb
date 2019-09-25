Option Explicit On
Option Infer Off
Option Strict On
Imports System.Data.SqlClient
Imports Purchase.Common
Partial Public Class PurchaseGroupSetting
    Inherits CommonPage
    Const SAVE_ACTION As String = "Save"
    Const EDIT_ACTION As String = "Edit"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Msg.Text = String.Empty
        If IsPostBack = False Then
            Mode.Value = Common.GetHttpAction(Request)
            UserID.Value = Common.GetHttpQuery(Request, "UserID")
            If Common.IsInteger(UserID.Value) = False Or UserID.Value.Length = 0 Then
                Msg.Text = Common.ERR_INVALID_PARAMETER
                Exit Sub
            End If
                Dim st_SQL As String = String.Empty
                st_SQL &= "SELECT "
                st_SQL &= " UserID, "
                st_SQL &= " LocationName, "
                st_SQL &= " AccountName, "
                st_SQL &= "[Name], "
            st_SQL &= "R3PurchasingGroup "
            st_SQL &= "FROM "
                st_SQL &= " v_UserAll "
                st_SQL &= "WHERE "
                st_SQL &= "UserID = "
            st_SQL &= UserID.Value
                Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                    Dim DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = st_SQL
                    DBConn.Open()
                    Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
                    If DBReader.Read = True Then
                        Location.Text = CStr(DBReader("LocationName"))
                        Name.Text = CStr(DBReader("Name"))
                    R3PurchasingGroup.Text = DBReader("R3PurchasingGroup").ToString()
                    Dim SLocationByPUser As DataTable = GetDataTable("select * from StorageByPurchasingUser where UserID=" + UserID.Value)
                    For i As Integer = 0 To SLocationByPUser.Rows.Count - 1
                        If SLocationByPUser.Rows(i)("Storage").ToString = "AL10" Then
                            AL10.Checked = True
                        End If
                        If SLocationByPUser.Rows(i)("Storage").ToString = "AL11" Then
                            AL11.Checked = True
                        End If
                        If SLocationByPUser.Rows(i)("Storage").ToString = "AL20" Then
                            AL20.Checked = True
                        End If
                        If SLocationByPUser.Rows(i)("Storage").ToString = "AL40" Then
                            AL40.Checked = True
                        End If
                        If SLocationByPUser.Rows(i)("Storage").ToString = "AL50" Then
                            AL50.Checked = True
                        End If
                        If SLocationByPUser.Rows(i)("Storage").ToString = "CL10" Then
                            CL10.Checked = True
                        End If
                        'If SLocationByPUser.Rows(i)("Storage").ToString = "CL20" Then
                        '    CL20.Checked = True
                        'End If
                        'If SLocationByPUser.Rows(i)("Storage").ToString = "CL30" Then
                        '    CL30.Checked = True
                        'End If
                        If SLocationByPUser.Rows(i)("Storage").ToString = "CL40" Then
                            CL40.Checked = True
                        End If
                        If SLocationByPUser.Rows(i)("Storage").ToString = "CL70" Then
                            CL70.Checked = True
                        End If
                        If SLocationByPUser.Rows(i)("Storage").ToString = "EL10" Then
                            EL10.Checked = True
                        End If
                        If SLocationByPUser.Rows(i)("Storage").ToString = "EL20" Then
                            EL20.Checked = True
                        End If
                        If SLocationByPUser.Rows(i)("Storage").ToString = "HL10" Then
                            HL10.Checked = True
                        End If
                        If SLocationByPUser.Rows(i)("Storage").ToString = "HL30" Then
                            HL30.Checked = True
                        End If
                        If SLocationByPUser.Rows(i)("Storage").ToString = "NL10" Then
                            NL10.Checked = True
                        End If
                        If SLocationByPUser.Rows(i)("Storage").ToString = "NL20" Then
                            NL20.Checked = True
                        End If
                    Next
                Else
                    Msg.Text = Common.MSG_NO_DATA_FOUND
                    Exit Sub
                End If
                DBReader.Close()
            End Using
        End If

    End Sub
    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        If Common.GetHttpAction(Request) <> SAVE_ACTION Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If
        If UserID.Value.Length = 0 Then
            Msg.Text = "User ID " & Common.ERR_REQUIRED_FIELD
            Exit Sub
        End If
        If Common.IsInteger(UserID.Value) = False Then
            Msg.Text = "User ID" & Common.ERR_INVALID_NUMBER
            Exit Sub
        End If
        If Common.ExistenceConfirmation("s_User", "UserID", UserID.Value) = False Then
            Msg.Text = "User ID" & Common.ERR_DOES_NOT_EXIST
            Exit Sub
        End If
        Dim st_SQL As String = String.Empty
        If Common.GetHttpQuery(Request, "Mode") = EDIT_ACTION Then
            If Common.ExistenceConfirmation("PurchasingUser", "UserID", UserID.Value) = False Then    '[入力UserIDのPurchasingUser存在有無]
                Msg.Text = Common.ERR_DELETED_BY_ANOTHER_USER
                Exit Sub
            End If
            st_SQL &= "UPDATE PurchasingUser SET "
            st_SQL &= "R3PurchasingGroup='" + R3PurchasingGroup.Text + "'"
            st_SQL &= "WHERE UserID='" & UserID.Value & "' "
            '1.先删除后添加
            st_SQL &= "delete StorageByPurchasingUser where UserID=" + UserID.Value + " "
            If AL10.Checked = True Then
                st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + AL10.ID.ToString + "') "
            End If
            If AL11.Checked = True Then
                st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + AL11.ID.ToString + "') "
            End If
            If AL20.Checked = True Then
                st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + AL20.ID.ToString + "') "
            End If
            If AL40.Checked = True Then
                st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + AL40.ID.ToString + "') "
            End If
            If AL50.Checked = True Then
                st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + AL50.ID.ToString + "') "
            End If
            If CL10.Checked = True Then
                st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + CL10.ID.ToString + "') "
            End If
            'If CL20.Checked = True Then
            '    st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + CL20.ID.ToString + "') "
            'End If
            'If CL30.Checked = True Then
            '    st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + CL30.ID.ToString + "') "
            'End If
            If CL40.Checked = True Then
                st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + CL40.ID.ToString + "') "
            End If
            If CL70.Checked = True Then
                st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + CL70.ID.ToString + "') "
            End If
            If EL10.Checked = True Then
                st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + EL10.ID.ToString + "') "
            End If
            If EL20.Checked = True Then
                st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + EL20.ID.ToString + "') "
            End If
            If HL10.Checked = True Then
                st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + HL10.ID.ToString + "') "
            End If
            If HL30.Checked = True Then
                st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + HL30.ID.ToString + "') "
            End If
            If NL10.Checked = True Then
                st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + NL10.ID.ToString + "') "
            End If
            If NL20.Checked = True Then
                st_SQL &= "insert into StorageByPurchasingUser (UserID,Storage) values(" + UserID.Value + ",'" + NL20.ID.ToString + "') "
            End If
        Else
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If
        Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
            Dim DBCommand As SqlCommand = DBConn.CreateCommand()
            DBCommand.CommandText = st_SQL
            DBConn.Open()
            DBCommand.ExecuteNonQuery()
        End Using
        Response.Redirect("PurchaseGroup.aspx")
    End Sub
End Class
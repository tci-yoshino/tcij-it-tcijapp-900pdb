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
                DBCommand.CommandText = "SELECT UserID,LocationName,AccountName,""Name"",RoleCode,PrivilegeLevel,isAdmin,isDisabled, UpdateDate " & _
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
            If Common.ExistenceConfirmation("v_UserAll", "UserID", Session("UserID").ToString()) = False Then
                '[エラーメッセージの表示]
                Return
            End If

            If Common.IsLatestData("v_userAll", "UserID", Session("UserID").ToString(), UpdateDate.Value) = False Then
                '[エラーメッセージの表示]
                Return
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

        'st_SQL.Remove(0, st_SQL.Length)
        'st_SQL.Append("UPDATE ")




        'st_SQL.Append(" UserID, ")
        'st_SQL.Append(" LocationName, ")
        'st_SQL.Append(" AccountName, ")
        'st_SQL.Append(" AD_DeptName, ")
        'st_SQL.Append(" AD_DisplayName, ")
        'st_SQL.Append(" Name ")
        'st_SQL.Append("FROM ")
        'st_SQL.Append(" v_UserAll ")
        'st_SQL.Append("WHERE ")
        'If LocationName.Text <> String.Empty Then
        '    st_WHERE = st_WHERE + "LocationName='" + LocationName.Text + "'"
        'End If
        'If DeptName.Text <> String.Empty Then
        '    If st_WHERE <> String.Empty Then st_WHERE = st_WHERE + " AND "
        '    st_WHERE = st_WHERE + "AD_DeptName LIKE '%" + DeptName.Text + "%'"
        'End If
        'If UserName.Text <> String.Empty Then
        '    If st_WHERE <> String.Empty Then st_WHERE = st_WHERE + " AND "
        '    st_WHERE = st_WHERE + "AD_DisplayName LIKE '%" + UserName.Text + "%'"
        'End If
        'st_SQL.Append("" + st_WHERE + "")
        'SrcUser.SelectCommand = st_SQL.ToString




        'Dim st_Location As String = ""
        'If Request.Form("Action") <> "Save" Then
        '    Msg.Text = Common.ERR_INVALID_PARAMETER
        '    Exit Sub
        'End If

        ''[CodeのCheck]---------------------------------------------------------------------
        'If Trim(UserID.Text) = "" Then
        '    Msg.Text = "CountryCode" + Common.ERR_REQUIRED_FIELD
        '    Exit Sub
        'Else
        '    '[s_Country check]-------------------------------------------------------------
        '    DBCommand.CommandText = "SELECT CountryCode FROM dbo.s_Country WHERE CountryCode = '" + Common.SafeSqlLiteral(UserID.Text) + "'"
        '    DBReader = DBCommand.ExecuteReader()
        '    DBCommand.Dispose()
        '    If DBReader.Read = False Then
        '        Msg.Text = "Country Code can not be found in R3 master table."  'CountryCodeが不正です。
        '        DBReader.Close()
        '        Exit Sub
        '    End If
        '    DBReader.Close()
        'End If

        ''[PurchasingCountryの追加又は更新]-------------------------------------------------
        'DBCommand.CommandText = "SELECT CountryCode FROM PurchasingCountry WHERE CountryCode = '" + Common.SafeSqlLiteral(UserID.Text) + "'"
        'DBReader = DBCommand.ExecuteReader()
        'DBCommand.Dispose()
        'If DBReader.Read = True Then
        '    DBReader.Close()

        '    If Request.QueryString("Action") <> "Edit" Then
        '        Msg.Text = "Your requested contry code already exist.<br />(Please check again to avoid duplication.)"   '"このデータはすでに登録済です。その内容を確認し再度処理をお願いします"
        '        Exit Sub
        '    End If

        '    '[PurchasingCountryのUpdateDateの値を取得する]---------------------------------
        '    DBCommand.CommandText = "SELECT UpdateDate FROM PurchasingCountry WHERE CountryCode = '" + Common.SafeSqlLiteral(UserID.Text) + "'"
        '    DBReader = DBCommand.ExecuteReader()
        '    DBCommand.Dispose()
        '    If DBReader.Read = False Then
        '        Msg.Text = Common.ERR_DELETED_BY_ANOTHER_USER  '"このデータは他のユーザーによって削除されています。"
        '        DBReader.Close()
        '        Exit Sub
        '    End If

        '    'TODO ToStringで臨時対応
        '    If DBReader("UpdateDate").ToString() <> UpdateDate.Value Then
        '        DBReader.Close()
        '        Msg.Text = Common.ERR_UPDATED_BY_ANOTHER_USER   '"データは他のユーザによって既に更新されています。ご確認ください。"
        '        Exit Sub
        '    End If
        '    DBReader.Close()

        '    If RoleCode.Text.ToString <> "Direct" Then
        '        DBCommand.CommandText = "SELECT LocationCode FROM dbo.s_Location WHERE Name = '" + Common.SafeSqlLiteral(RoleCode.Text) + "'"
        '        DBReader = DBCommand.ExecuteReader()
        '        DBCommand.Dispose()
        '        If DBReader.Read = True Then
        '            st_Location = DBReader("LocationCode")
        '            DBReader.Close()
        '            '[PurchasingCountryの更新処理]-----------------------------------------
        '            DBCommand.CommandText = "UPDATE [PurchasingCountry] SET DefaultQuoLocationCode='" + st_Location + "',UpdatedBy=" + Session("UserID") + ", UpdateDate='" + Now() + "'  WHERE CountryCode ='" + Common.SafeSqlLiteral(UserID.Text) + "'"
        '            DBCommand.ExecuteNonQuery()
        '        Else
        '            DBReader.Close()
        '        End If
        '    Else
        '        '[PurchasingCountryの更新処理]---------------------------------------------
        '        DBCommand.CommandText = "UPDATE [PurchasingCountry] SET DefaultQuoLocationCode=null,UpdatedBy=" + Session("UserID") + ", UpdateDate='" + Now() + "'  WHERE CountryCode ='" + Common.SafeSqlLiteral(UserID.Text) + "'"
        '        DBCommand.ExecuteNonQuery()
        '    End If
        'Else
        '    DBReader.Close()
        '    '[PurchasingCountryの追加処理]-------------------------------------------------
        '    If RoleCode.Text.ToString <> "Direct" Then
        '        DBCommand.CommandText = "SELECT LocationCode FROM dbo.s_Location WHERE Name = '" + Common.SafeSqlLiteral(RoleCode.Text) + "'"
        '        DBReader = DBCommand.ExecuteReader()
        '        DBCommand.Dispose()
        '        If DBReader.Read = True Then
        '            st_Location = DBReader("LocationCode")
        '            DBReader.Close()
        '            '[PurchasingCountryの追加処理]-----------------------------------------
        '            DBCommand.CommandText = "INSERT INTO PurchasingCountry (CountryCode,DefaultQuoLocationCode,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ('" + Common.SafeSqlLiteral(UCase(UserID.Text)) + "','" + st_Location + "','" + Session("UserID") + "','" + Now() + "','" + Session("UserID") + "','" + Now() + "')"
        '            DBCommand.ExecuteNonQuery()
        '        Else
        '            DBReader.Close()
        '        End If
        '    Else
        '        '[PurchasingCountryの追加処理]---------------------------------------------
        'DBCommand.CommandText = "INSERT INTO PurchasingCountry (CountryCode,DefaultQuoLocationCode,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ('" + Common.SafeSqlLiteral(UCase(UserID.Text)) + "',null,'" + Session("UserID") + "','" + Now() + "','" + Session("UserID") + "','" + Now() + "')"
        '        DBCommand.ExecuteNonQuery()
        '    End If
        'End If

        ''[最終的に更新するPurchasingCountryのUpdateDateの値をHidden(UpdateDate)にセット]
        'DBCommand.CommandText = "SELECT UpdateDate FROM PurchasingCountry WHERE CountryCode = '" + Common.SafeSqlLiteral(UserID.Text) + "'"
        'DBReader = DBCommand.ExecuteReader()
        'DBCommand.Dispose()
        'If DBReader.Read = True Then
        '    'TODO ToStringで臨時対応
        '    UpdateDate.Value = DBReader("UpdateDate").ToString()
        'End If
        'DBReader.Close()

        ''[呼出元のフォームに戻る]----------------------------------------------------------
        'If Msg.Text.ToString = "" Then
        '    Response.Redirect("CountryList.aspx")
        'End If
    End Sub

    Private Sub CountrySetting_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'DBConn.Close()
    End Sub

    Private Function ConvertBoolToSQLString(ByVal value As Boolean) As String
        If value = True Then
            Return "1"
        Else
            Return "0"
        End If
    End Function

End Class
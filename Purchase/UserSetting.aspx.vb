Partial Public Class UserSetting
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '[DBの接続]-----------------------------------------------------------------------
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        '[Msgのクリア]---------------------------------------------------------------------
        Msg.Text = ""

        '[Role Code 設定]------------------------------------------------------------------
        DBCommand.CommandText = "SELECT RoleCode FROM Role ORDER BY RoleCode"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        RoleCode.Items.Clear()
        Do Until DBReader.Read = False
            RoleCode.Items.Add(DBReader("RoleCode"))
        Loop
        DBReader.Close()
        RoleCode.SelectedValue = "WRITE"

        '[Privilege Label 設定]------------------------------------------------------------
        PrivilegeLavel.Items.Clear()
        PrivilegeLavel.Items.Add("P")
        PrivilegeLavel.Items.Add("A")

        If IsPostBack = False Then
            '[処理(登録/修正)の判断]-------------------------------------------------------
            If Request.QueryString("Action") = "Edit" Then
                UserID.Text = Request.QueryString("UserID")
                Search.Visible = False
                DBCommand.CommandText = "SELECT UserID,LocationName,AccountName,""Name"",RoleCode,PrivilegeLevel,R3PurchasingGroup,isAdmin,isDisabled " & _
                                        "FROM v_UserAll WHERE UserID=" & UserID.Text
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    Location.Text = DBReader("LocationName")
                    AccountName.Text = DBReader("AccountName")
                    Name.Text = DBReader("Name")
                    RoleCode.SelectedValue = DBReader("RoleCode")
                    PrivilegeLavel.Text = DBReader("PrivilegeLevel")
                End If
                DBReader.Close()
            Else
                UserID.CssClass = String.Empty
                UserID.ReadOnly = False

            End If












            '    '[最終的に更新するPurchasingCountryのUpdateDateの値をHidden(UpdateDate)にセット]
            '    DBCommand.CommandText = "SELECT UpdateDate FROM PurchasingCountry WHERE CountryCode = '" + Common.SafeSqlLiteral(UserID.Text) + "'"
            '    DBReader = DBCommand.ExecuteReader()
            '    DBCommand.Dispose()
            '    If DBReader.Read = True Then
            '        'TODO ToStringで臨時対応
            '        UpdateDate.Value = DBReader("UpdateDate").ToString()
            '    End If
            '    DBReader.Close()
            'Else
            '    '[ReadOnly項目の再設定]--------------------------------------------------------
            '    AccountName.Text = Request.Form("Name")
        End If
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
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
        '        DBCommand.CommandText = "INSERT INTO PurchasingCountry (CountryCode,DefaultQuoLocationCode,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ('" + Common.SafeSqlLiteral(UCase(UserID.Text)) + "',null,'" + Session("UserID") + "','" + Now() + "','" + Session("UserID") + "','" + Now() + "')"
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
        DBConn.Close()
    End Sub

End Class
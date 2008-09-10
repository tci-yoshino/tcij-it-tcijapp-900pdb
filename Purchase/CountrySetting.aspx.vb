Public Partial Class CountrySetting
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader
    Dim ActNai As String                                    '処理判断内容

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '[DBの接続]-----------------------------------------------------------------------
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        '[Msgのクリア]---------------------------------------------------------------------
        Msg.Text = ""

        If IsPostBack = False Then
            '[Location設定]----------------------------------------------------------------
            DBCommand.CommandText = "SELECT Name FROM dbo.s_Location ORDER BY Name"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            Location.Items.Clear()
            Location.Items.Add("Direct")
            Do Until DBReader.Read = False
                Location.Items.Add(DBReader("Name"))
            Loop
            DBReader.Close()

            '[処理(登録/修正)の判断]-------------------------------------------------------
            If Request.QueryString("Action") = "Edit" Then
                Code.Text = Request.QueryString("Code")
                Search.Visible = False
                DBCommand.CommandText = "SELECT CountryName,DefaultQuoLocationName FROM dbo.v_Country WHERE CountryCode = '" + Common.SafeSqlLiteral(Code.Text) + "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    Name.Text = DBReader("CountryName")
                    Location.Text = DBReader("DefaultQuoLocationName")
                End If
                DBReader.Close()
                '[最終的に更新するPurchasingCountryのUpdateDateの値をHidden(UpdateDate)にセット]
                DBCommand.CommandText = "SELECT UpdateDate FROM PurchasingCountry WHERE CountryCode = '" + Common.SafeSqlLiteral(Code.Text) + "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    'TODO ToStringで臨時対応
                    UpdateDate.Value = DBReader("UpdateDate").ToString()
                End If
            Else
                Code.CssClass = ""
                Code.ReadOnly = False
                UpdateDate.Value = ""
            End If
        Else
            '[ReadOnly項目の再設定]--------------------------------------------------------
            Name.Text = Request.Form("Name")
        End If
    End Sub

    Private Sub CountrySetting_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim wClient As String       'クライアントサイドの処理を格納する
        Dim Type2 As Type = Me.GetType
        wClient = Clientside()
        If wClient <> "" Then
            ClientScript.RegisterStartupScript(Type2, "startup", Chr(13) & Chr(10) & "<script language='JavaScript' type=text/javascript> " & wClient & " </script>")
        End If
    End Sub

    Private Function Clientside()
        Clientside = ""
        If ActNai = "CountrySelect.aspx_Open" Then
            Clientside = "popup('CountrySelect.aspx?code=" + Common.SafeSqlLiteral(Code.Text) + "')"
        End If
    End Function

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        Dim st_Location As String = ""
        If Request.Form("Action") <> "Save" Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        '[CodeのCheck]---------------------------------------------------------------------
        If Trim(Code.Text) = "" Then
            Msg.Text = "CountryCode" + Common.ERR_REQUIRED_FIELD
            Exit Sub
        Else
            '[s_Country check]-------------------------------------------------------------
            DBCommand.CommandText = "SELECT CountryCode FROM dbo.s_Country WHERE CountryCode = '" + Common.SafeSqlLiteral(Code.Text) + "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = False Then
                Msg.Text = "CountryCodeが不正です。"
                DBReader.Close()
                Exit Sub
            End If
            DBReader.Close()
        End If

        '[PurchasingCountryの追加又は更新]-------------------------------------------------
        DBCommand.CommandText = "SELECT CountryCode FROM PurchasingCountry WHERE CountryCode = '" + Common.SafeSqlLiteral(Code.Text) + "'"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.Read = True Then
            DBReader.Close()

            If Request.QueryString("Action") <> "Edit" Then
                Msg.Text = "このデータはすでに登録済です。その内容を確認し再度処理をお願いします"
                Exit Sub
            End If

            '[PurchasingCountryのUpdateDateの値を取得する]---------------------------------
            DBCommand.CommandText = "SELECT UpdateDate FROM PurchasingCountry WHERE CountryCode = '" + Common.SafeSqlLiteral(Code.Text) + "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = False Then
                Msg.Text = "このデータは他のユーザーによって削除されています。"
                DBReader.Close()
                Exit Sub
            End If

            'TODO ToStringで臨時対応
            If DBReader("UpdateDate").ToString() <> UpdateDate.Value Then
                DBReader.Close()
                Msg.Text = "データは他のユーザによって既に更新されています。ご確認ください。"
                Exit Sub
            End If
            DBReader.Close()

            If Location.Text.ToString <> "Direct" Then
                DBCommand.CommandText = "SELECT LocationCode FROM dbo.s_Location WHERE Name = '" + Common.SafeSqlLiteral(Location.Text) + "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    st_Location = DBReader("LocationCode")
                    DBReader.Close()
                    '[PurchasingCountryの更新処理]-----------------------------------------
                    DBCommand.CommandText = "UPDATE [PurchasingCountry] SET DefaultQuoLocationCode='" + st_Location + "',UpdatedBy=" + Session("UserID") + ", UpdateDate='" + Now() + "'  WHERE CountryCode ='" + Common.SafeSqlLiteral(Code.Text) + "'"
                    DBCommand.ExecuteNonQuery()
                Else
                    DBReader.Close()
                End If
            Else
                '[PurchasingCountryの更新処理]---------------------------------------------
                DBCommand.CommandText = "UPDATE [PurchasingCountry] SET DefaultQuoLocationCode=null,UpdatedBy=" + Session("UserID") + ", UpdateDate='" + Now() + "'  WHERE CountryCode ='" + Common.SafeSqlLiteral(Code.Text) + "'"
                DBCommand.ExecuteNonQuery()
            End If
        Else
            DBReader.Close()
            '[PurchasingCountryの追加処理]-------------------------------------------------
            If Location.Text.ToString <> "Direct" Then
                DBCommand.CommandText = "SELECT LocationCode FROM dbo.s_Location WHERE Name = '" + Common.SafeSqlLiteral(Location.Text) + "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    st_Location = DBReader("LocationCode")
                    DBReader.Close()
                    '[PurchasingCountryの追加処理]-----------------------------------------
                    DBCommand.CommandText = "INSERT INTO PurchasingCountry (CountryCode,DefaultQuoLocationCode,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ('" + Common.SafeSqlLiteral(UCase(Code.Text)) + "','" + st_Location + "','" + Session("UserID") + "','" + Now() + "','" + Session("UserID") + "','" + Now() + "')"
                    DBCommand.ExecuteNonQuery()
                Else
                    DBReader.Close()
                End If
            Else
                '[PurchasingCountryの追加処理]---------------------------------------------
                DBCommand.CommandText = "INSERT INTO PurchasingCountry (CountryCode,DefaultQuoLocationCode,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ('" + Common.SafeSqlLiteral(UCase(Code.Text)) + "',null,'" + Session("UserID") + "','" + Now() + "','" + Session("UserID") + "','" + Now() + "')"
                DBCommand.ExecuteNonQuery()
            End If
        End If

        '[呼出元のフォームに戻る]----------------------------------------------------------
        If Msg.Text.ToString = "" Then
            Response.Redirect("CountryList.aspx")
        End If
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Search.Click
        ActNai = "CountrySelect.aspx_Open"
    End Sub

    Private Sub CountrySetting_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
    End Sub

End Class
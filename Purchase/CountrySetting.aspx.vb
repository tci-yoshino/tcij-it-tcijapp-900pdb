Public Partial Class CountrySetting
    Inherits CommonPage

#Region " Web フォーム デザイナで生成されたコード "
    '*****（Region内は変更しないこと）*****
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection
        Me.SqlConnection1.FireInfoMessageEventOnUserErrors = False
    End Sub

    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub
#End Region

    '*****（DB接続用変数定義）*****
    Dim DBConnString As String                              '接続文字列	
    Dim DBConn As New System.Data.SqlClient.SqlConnection   'データベースコネクション	
    Dim DBCommand As System.Data.SqlClient.SqlCommand       'データベースコマンド	
    Dim DBReader As System.Data.SqlClient.SqlDataReader     'データリーダー	
    Dim ActNai As String                                    '処理判断内容

    Sub Set_DBConnectingString()
        Dim settings As ConnectionStringSettings
        '[接続文字列を設定ファイル(Web.config)から取得]---------------------------------------------
        settings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
        If Not settings Is Nothing Then
            '[接続文字列をイミディエイトに出力]-----------------------------------------------------
            Debug.Print(settings.ConnectionString)
        End If
        '[sqlConnectionに接続文字列を設定]----------------------------------------------------------
        Me.SqlConnection1.ConnectionString = settings.ConnectionString
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '[DBの接続]-----------------------------------------------------------------------
        Set_DBConnectingString()
        DBConn = Me.SqlConnection1
        DBConn.Open()

        '[Msgのクリア]--------------------------------------------------------------------
        Msg.Text = ""

        If IsPostBack = False Then
            '[Location設定]-------------------------------------------------------------------
            DBCommand = DBConn.CreateCommand()
            DBCommand.CommandText = "SELECT Name FROM dbo.s_Location"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            Location.Items.Clear()
            Location.Items.Add("Direct")
            Do Until DBReader.Read = False
                Location.Items.Add(DBReader("Name"))
            Loop
            DBReader.Close()

            '[処理(登録/修正)の判断]----------------------------------------------------------
            If Request.QueryString("Action") = "Edit" Then
                Code.Text = Request.QueryString("Code")
                Search.Visible = False
                DBCommand = DBConn.CreateCommand()
                DBCommand.CommandText = "SELECT CountryName,DefaultQuoLocationName FROM dbo.v_Country WHERE CountryCode = '" & Code.Text.ToString & "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    Name.Text = DBReader("CountryName")
                    Location.Text = DBReader("DefaultQuoLocationName")
                End If
                DBReader.Close()
            Else
                Code.CssClass = ""
                Code.ReadOnly = False
            End If
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
            Clientside = "popup('CountrySelect.aspx?code=" & Code.Text.ToString & "')"
        End If
    End Function

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        Dim memoLocation As String = ""

        '[データのチェックと保存]----------------------------------------------------------
        If Code.Text.ToString = "" Then
            Msg.Text = "Please Input Country_Code"
        Else
            '[s_Country check]----------------------------------------------------------
            DBCommand = DBConn.CreateCommand()
            DBCommand.CommandText = "SELECT CountryCode FROM dbo.s_Country WHERE CountryCode = '" & Code.Text.ToString & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                '[PurchasingCountryの追加又は更新]------------------------------------------
                DBReader.Close()
                DBCommand = DBConn.CreateCommand()
                DBCommand.CommandText = "SELECT CountryCode FROM dbo.PurchasingCountry WHERE CountryCode = '" & Code.Text.ToString & "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    DBReader.Close()
                    If Location.Text.ToString <> "Direct" Then
                        DBCommand = DBConn.CreateCommand()
                        DBCommand.CommandText = "SELECT LocationCode FROM dbo.s_Location WHERE Name = '" & Location.Text.ToString & "'"
                        DBReader = DBCommand.ExecuteReader()
                        DBCommand.Dispose()
                        If DBReader.Read = True Then
                            memoLocation = DBReader("LocationCode")
                            DBReader.Close()
                            '[PurchasingCountryの更新処理]------------------------------------------
                            DBCommand.CommandText = "UPDATE [PurchasingCountry] SET DefaultQuoLocationCode='" & memoLocation & "',UpdatedBy=" & Session("UserID") & ", UpdateDate='" & Now() & "'  WHERE CountryCode ='" & Code.Text.ToString & "'"
                            DBCommand.ExecuteNonQuery()
                        Else
                            DBReader.Close()
                        End If
                    Else
                        '[PurchasingCountryの更新処理]------------------------------------------
                        DBCommand.CommandText = "UPDATE [PurchasingCountry] SET DefaultQuoLocationCode=null,UpdatedBy=" & Session("UserID") & ", UpdateDate='" & Now() & "'  WHERE CountryCode ='" & Code.Text.ToString & "'"
                        DBCommand.ExecuteNonQuery()
                    End If
                Else
                    DBReader.Close()
                    '[PurchasingCountryの追加処理]------------------------------------------
                    If Location.Text.ToString <> "Direct" Then
                        DBCommand = DBConn.CreateCommand()
                        DBCommand.CommandText = "SELECT LocationCode FROM dbo.s_Location WHERE Name = '" & Location.Text.ToString & "'"
                        DBReader = DBCommand.ExecuteReader()
                        DBCommand.Dispose()
                        If DBReader.Read = True Then
                            memoLocation = DBReader("LocationCode")
                            DBReader.Close()
                            '[PurchasingCountryの追加処理]------------------------------------------
                            DBCommand.CommandText = "INSERT INTO PurchasingCountry (CountryCode,DefaultQuoLocationCode,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ('" & UCase(Code.Text.ToString) & "','" & memoLocation & "','" & Session("UserID") & "','" & Now() & "','" & Session("UserID") & "','" & Now() & "')"
                            DBCommand.ExecuteNonQuery()
                        Else
                            DBReader.Close()
                        End If
                    Else
                        '[PurchasingCountryの追加処理]------------------------------------------
                        DBCommand.CommandText = "INSERT INTO PurchasingCountry (CountryCode,DefaultQuoLocationCode,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ('" & UCase(Code.Text.ToString) & "',null,'" & Session("UserID") & "','" & Now() & "','" & Session("UserID") & "','" & Now() & "')"
                        DBCommand.ExecuteNonQuery()
                    End If
                End If
            Else
                DBReader.Close()
                Msg.Text = "Not found Country_Code"
            End If
        End If

        '[呼出元のフォームに戻る]------------------------------------------
        If Msg.Text.ToString = "" Then
            Response.Redirect("CountryList.aspx")
        End If
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Search.Click
        ActNai = "CountrySelect.aspx_Open"
    End Sub

End Class
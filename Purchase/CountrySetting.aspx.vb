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
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        If Code.Text.ToString = "" Then
            Msg.Text = "Please Input Country_Code"
        Else
            '[s_Country check]----------------------------------------------------------
            DBCommand = DBConn.CreateCommand()
            DBCommand.CommandText = "SELECT CountryCode FROM dbo.s_Country WHERE CountryCode = '" & Code.Text.ToString & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = False Then
                Msg.Text = "Not found Country_Code"
            End If
            DBReader.Close()
            '[PurchasingCountryの追加又は更新]------------------------------------------
            DBCommand = DBConn.CreateCommand()
            DBCommand.CommandText = "SELECT CountryCode FROM dbo.PurchasingCountry WHERE CountryCode = '" & Code.Text.ToString & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                'DBCommand.CommandText = "UPDATE [Zh_Action] SET UDateCata=null,TantCata='',UDate3=null,Tant3='',NYo1='',NPri1='',NCata1='',NYo2='',NPri2='',NCata2='',NYo3='',NPri3='',NCata3='',NChg=''  WHERE ([SCode]='" & TextBox1.Text.ToString & "')"
                'DBCommand.ExecuteNonQuery()
                'DBCommand.Dispose()
            Else

            End If
        End If
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles Search.Click
        Search.OnClientClick = "popup('CountrySelect.aspx?code=" & Code.Text.ToString & "')"
    End Sub
End Class
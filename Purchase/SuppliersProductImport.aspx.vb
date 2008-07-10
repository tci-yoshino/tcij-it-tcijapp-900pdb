Public Partial Class SuppliersProductImport
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
    Dim ImpExcel As String

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
        Set_DBConnectingString()
        DBConn = Me.SqlConnection1
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        If Request.QueryString("Supplier") <> "" Then
            If IsPostBack = False Then
                SupplierCode.Text = Request.QueryString("Supplier")
                DBCommand.CommandText = "SELECT Name3 FROM Supplier WHERE (SupplierCode = '" & SupplierCode.Text.ToString & "')"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    SupplierName.Text = DBReader("Name3")
                End If
                DBReader.Close()
            End If
        Else
            Msg.Text = "SupplierCodeが設定されていません"
        End If
    End Sub

    Protected Sub Preview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Preview.Click
        '[作成Table名の決定]------------------------------------------------------------
        ImpExcel = "D:\\temp\\G_System4\DT" & IO.Path.GetFileName(File.PostedFile.FileName)

        '[作成Tableがある場合削除する]--------------------------------------------------
        DBCommand.CommandText = "IF object_id('" & ImpExcel & "', 'U') IS NOT NULL DROP TABLE [" & ImpExcel & "]"
        DBCommand.ExecuteNonQuery()

        If File.PostedFile.FileName <> "" Then
            File.PostedFile.SaveAs(ImpExcel)
        End If


        ''[指定コードをDBに取り込む]-----------------------------------------------
        'Dim fileReader As System.IO.StreamReader
        'fileReader = My.Computer.FileSystem.OpenTextFileReader(ImpExcel, System.Text.Encoding.Default)
        'Dim stringReader As String = ""
        'Do Until fileReader.EndOfStream = True
        '    Dim aaa As String = stringReader
        'Loop
        'fileReader.Close()




    End Sub
End Class
Public Partial Class ProductSearch
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
        Set_DBConnectingString()
        DBConn = Me.SqlConnection1
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        SrcProduct.SelectCommand = "SELECT ProductNumber, QuoName, CASNumber FROM dbo.Product WHERE "
        If ProductNumber.Text.ToString <> "" Then SrcProduct.SelectCommand = SrcProduct.SelectCommand + "(ProductNumber = '" + ProductNumber.Text.ToString + "')"
        If CASNumber.Text.ToString <> "" Then
            If Right(SrcProduct.SelectCommand, 1) = ")" Then
                SrcProduct.SelectCommand = SrcProduct.SelectCommand + " AND (CASNumber = '" + CASNumber.Text.ToString + "')"
            Else
                SrcProduct.SelectCommand = SrcProduct.SelectCommand + "(CASNumber = '" + CASNumber.Text.ToString + "')"
            End If
        End If
        ProductList.DataBind()
    End Sub
End Class
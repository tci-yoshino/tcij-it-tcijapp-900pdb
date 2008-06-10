Public Partial Class SupplierSearch
    Inherits System.Web.UI.Page

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

        SrcSupplier.SelectCommand = ""
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        '[Supplier検索]-------------------------------------------------------------------
        If Not IsNumeric(Code.Text.ToString) Then
            Msg.Text = "コードには数字を入力して下さい"
        Else
            Msg.Text = ""
            Dim SQLStr As String = ""
            SrcSupplier.SelectCommand = "SELECT SupplierCode AS [Supplier Code], R3SupplierCode AS [R/3 Supplier Code], ISNULL(Name3, '') + N' ' + ISNULL(Name4, '') AS [Supplier Name] FROM dbo.Supplier "
            If Code.Text.ToString <> "" Then
                If SQLStr = "" Then SQLStr = "WHERE "
                SQLStr = SQLStr + "(SupplierCode = '" & Code.Text.ToString & "')"
            End If
            If R3Code.Text.ToString <> "" Then
                If SQLStr = "" Then SQLStr = "WHERE " Else SQLStr = SQLStr + " AND "
                SQLStr = SQLStr + "((R3SupplierCode) = " & (R3Code.Text.ToString) & ")"
            End If
            If Name.Text.ToString <> "" Then
                If SQLStr = "" Then SQLStr = "WHERE " Else SQLStr = SQLStr + " AND "
                SQLStr = SQLStr + "(Name1 + N' ' + Name2 LIKE '%" & Name.Text.ToString & "%')"
            End If
            SrcSupplier.SelectCommand = SrcSupplier.SelectCommand + SQLStr
        End If

    End Sub

End Class
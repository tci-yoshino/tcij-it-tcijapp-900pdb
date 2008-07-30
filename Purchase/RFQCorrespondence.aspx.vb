Public Partial Class RFQCorrespondence
    Inherits CommonPage

#Region " Region "
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection
        Me.SqlConnection1.FireInfoMessageEventOnUserErrors = False
    End Sub

    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub
#End Region

    Dim DBConnString As String                              '接続文字列	
    Dim DBConn As New System.Data.SqlClient.SqlConnection   'データベースコネクション	
    Dim DBCommand As System.Data.SqlClient.SqlCommand       'データベースコマンド	
    Dim DBReader As System.Data.SqlClient.SqlDataReader     'データリーダー	
    Public url As String = ""

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

        If IsPostBack = False Then
            '[CorresTitle設定]--------------------------------------------------------------------------
            DBCommand.CommandText = "SELECT RFQCorresCode, Text FROM dbo.RFQCorres Order BY SortOrder"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            CorresTitle.Items.Clear()
            Do Until DBReader.Read = False
                CorresTitle.Items.Add(New ListItem(DBReader("Text"), DBReader("RFQCorresCode")))
            Loop
            DBReader.Close()

            SrcRFQHistory.SelectCommand = "SELECT dbo.RFQStatus.Text AS Status, dbo.RFQHistory.StatusChangeDate AS Date, dbo.v_User.Name + '      (' + dbo.v_User.LocationName + ')' AS Sender, v_User_1.Name AS Addressee, dbo.RFQHistory.Note AS Notes " & _
                                          "FROM dbo.RFQHistory INNER JOIN dbo.RFQStatus ON dbo.RFQHistory.RFQStatusCode = dbo.RFQStatus.RFQStatusCode LEFT OUTER JOIN dbo.v_User AS v_User_1 ON dbo.RFQHistory.RcptUserID = v_User_1.UserID LEFT OUTER JOIN dbo.v_User ON dbo.RFQHistory.CreatedBy = dbo.v_User.UserID " & _
                                          "WHERE (dbo.RFQHistory.RFQNumber = '1000000001') " & _
                                          "ORDER BY dbo.RFQHistory.RFQHistoryNumber DESC"
            SrcRFQHistory.DataBind()
        End If
    End Sub

End Class
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
    Dim st_SqlStr As String = ""
    '***************************************************************************************************
    Dim RFQNumber As Integer = 1000000001
    '***************************************************************************************************

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
            '[DefaultでEnqUser.Checked設定]-------------------------------------------------------------
            EnqUser.Checked = True

            '[CorresTitle設定]--------------------------------------------------------------------------
            DBCommand.CommandText = "SELECT RFQCorresCode, Text FROM dbo.RFQCorres Order BY SortOrder"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            CorresTitle.Items.Clear()
            Do Until DBReader.Read = False
                CorresTitle.Items.Add(New ListItem(DBReader("Text"), DBReader("RFQCorresCode")))
            Loop
            DBReader.Close()

            '[EnqUser.Textの設定]-----------------------------------------------------------------------
            DBCommand.CommandText = "SELECT v_User.Name FROM RFQHeader LEFT OUTER JOIN v_User ON RFQHeader.EnqUserID = v_User.UserID WHERE (RFQHeader.RFQNumber = " & RFQNumber & ")"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                EnqUser.Text = DBReader("Name")
            End If
            DBReader.Close()

            '[EnqLocation.Textの設定]-------------------------------------------------------------------
            DBCommand.CommandText = "SELECT s_Location.Name FROM RFQHeader LEFT OUTER JOIN s_Location ON RFQHeader.EnqLocationCode = s_Location.LocationCode WHERE (RFQHeader.RFQNumber = " & RFQNumber & ")"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                EnqLocation.Text = "(" & DBReader("Name") & ")"
            End If
            DBReader.Close()

            '[QuoUser.Textの設定]-----------------------------------------------------------------------
            DBCommand.CommandText = "SELECT v_User.Name FROM RFQHeader LEFT OUTER JOIN v_User ON RFQHeader.QuoUserID = v_User.UserID WHERE (RFQHeader.RFQNumber = " & RFQNumber & ")"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                QuoUser.Text = DBReader("Name")
            End If
            DBReader.Close()

            '[QuoLocation.Textの設定]-------------------------------------------------------------------
            DBCommand.CommandText = "SELECT s_Location.Name FROM RFQHeader LEFT OUTER JOIN s_Location ON RFQHeader.QuoLocationCode = s_Location.LocationCode WHERE (RFQHeader.RFQNumber = " & RFQNumber & ")"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                QuoLocation.Text = "(" & DBReader("Name") & ")"
            End If
            DBReader.Close()
        End If
    End Sub

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        SrcRFQHistory.SelectCommand = "SELECT dbo.RFQStatus.Text AS Status, dbo.RFQHistory.CreateDate AS Date, dbo.v_User.Name + '          (' + dbo.v_User.LocationName + ')' AS Sender, v_User_1.Name + '          (' + dbo.v_User.LocationName + ')' AS Addressee, dbo.RFQHistory.Note AS Notes " & _
                                               "FROM dbo.RFQHistory INNER JOIN dbo.RFQStatus ON dbo.RFQHistory.RFQStatusCode = dbo.RFQStatus.RFQStatusCode LEFT OUTER JOIN dbo.v_User AS v_User_1 ON dbo.RFQHistory.RcptUserID = v_User_1.UserID LEFT OUTER JOIN dbo.v_User ON dbo.RFQHistory.CreatedBy = dbo.v_User.UserID " & _
                                               "WHERE (dbo.RFQHistory.RFQNumber = " & RFQNumber & ") " & _
                                               "ORDER BY dbo.RFQHistory.RFQHistoryNumber DESC"
        SrcRFQHistory.DataBind()
    End Sub

    Protected Sub Send_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Send.Click
        Dim st_RFQStatusCode As String = ""
        Dim st_StatusChangeDate As Date
        Dim st_LocationCode As String = ""
        Dim st_UserID As String = ""
        '[Send実行確認]---------------------------------------------------------------------------------
        If Action.Value <> "Send" Then
            Msg.Text = "Sendは拒否されました"
            Exit Sub
        End If

        '[パラメータRFQNumberと同一の最大RFQHistoryNumberのレコードを検索]------------------------------
        DBCommand.CommandText = "SELECT RFQStatusCode, StatusChangeDate FROM dbo.RFQHistory WHERE (RFQHistoryNumber = (SELECT MAX(RFQHistoryNumber) AS MaxNo FROM dbo.RFQHistory AS RFQHistory_1 WHERE (RFQNumber = 1000000001)))"
        DBReader = DBCommand.ExecuteReader()
        If DBReader.Read = True Then
            st_RFQStatusCode = DBReader("RFQStatusCode")
            st_StatusChangeDate = DBReader("StatusChangeDate")
        Else
            Msg.Text = "Sendは拒否されました"
            Exit Sub
        End If
        DBReader.Close()

        '[選択したUser,Locationを記憶する]--------------------------------------------------------------
        If EnqUser.Checked = True Then
            st_UserID = EnqUser.Text.ToString
            st_LocationCode = EnqLocation.Text.ToString
        Else
            st_UserID = QuoUser.Text.ToString
            st_LocationCode = QuoLocation.Text.ToString
        End If
        DBCommand.CommandText = "SELECT v_User.UserID FROM dbo.v_User WHERE (Name = '" & st_UserID & "')"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.Read = True Then
            st_UserID = DBReader("UserID")
        End If
        DBReader.Close()
        DBCommand.CommandText = "SELECT LocationCode FROM dbo.s_Location WHERE (Name = '" & Mid(st_LocationCode, 2, Len(st_LocationCode) - 2) & "')"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.Read = True Then
            st_LocationCode = DBReader("LocationCode")
        End If
        DBReader.Close()

        '[RFQHistory(を新規登録)]-----------------------------------------------------------------------
        st_SqlStr = "INSERT INTO RFQHistory (RFQNumber,RFQStatusCode,StatusChangeDate,RFQCorresCode,Note,SendLocationCode,SendUserID,RcptLocationCode,RcptUserID,isChecked,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ("
        st_SqlStr = st_SqlStr + "'" + Trim(Str(RFQNumber)) + "',"
        st_SqlStr = st_SqlStr + "'" + st_RFQStatusCode + "',"
        st_SqlStr = st_SqlStr + "'" + st_StatusChangeDate + "',"
        st_SqlStr = st_SqlStr + "'" + CorresTitle.SelectedValue + "',"
        st_SqlStr = st_SqlStr + "'" + CorresNote.Text + "',"
        st_SqlStr = st_SqlStr + "'" + Session("LocationCode") + "',"
        st_SqlStr = st_SqlStr + Session("UserID") + ","
        st_SqlStr = st_SqlStr + "'" + st_LocationCode + "',"
        st_SqlStr = st_SqlStr + st_UserID + ","
        st_SqlStr = st_SqlStr + "0,"
        st_SqlStr = st_SqlStr + Session("UserID") + ",'" + Now() + "'," + Session("UserID") + ",'" + Now() + "')"
        DBCommand.CommandText = st_SqlStr
        DBCommand.ExecuteNonQuery()
        Msg.Text = "表示データを登録しました"
    End Sub

End Class
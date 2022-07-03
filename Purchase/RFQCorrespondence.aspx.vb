Option Explicit On

Imports System.Data.SqlClient
Imports Purchase.Common

Partial Public Class RFQCorrespondence
    Inherits CommonPage

    Dim st_SqlStr As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            '[RFQNumberの取込]--------------------------------------------------------------------------
            If Request.QueryString("RFQNumber") <> "" Then
                RFQNumber.Value = Request.QueryString("RFQNumber")
            Else
                EnqUser.Enabled = False
                QuoUser.Enabled = False
                CorresTitle.Enabled = False
                CorresNote.Enabled = False
                Send.Enabled = False
                Msg.Text = Common.ERR_INVALID_PARAMETER
                Exit Sub
            End If

            '[Connectionの定義]-------------------------------------------------------------------------
            Dim conn As SqlConnection = Nothing

            '[DefaultでEnqUser.Checked設定]-------------------------------------------------------------
            EnqUser.Checked = True

            '権限ロールに従い極秘品はエラーとする
            If Session(SESSION_ROLE_CODE).ToString = ROLE_WRITE_P OrElse Session(SESSION_ROLE_CODE).ToString = ROLE_READ_P Then
                Using sqlConn As SqlConnection = New SqlConnection(DB_CONNECT_STRING)
                    Using sqlCmd As SqlCommand = sqlConn.CreateCommand()

                        sqlCmd.CommandText = "SELECT 1 FROM v_RFQHeader WHERE isCONFIDENTIAL = 1 AND RFQNumber = @RFQNumber"
                        sqlCmd.Parameters.AddWithValue("RFQNumber", RFQNumber.Value)
                        sqlConn.Open()
                        Dim dr As SqlDataReader = sqlCmd.ExecuteReader
                        If dr.Read = True Then
                            Response.Redirect("AuthError.html")
                        End If
                    End Using
                End Using
            End If

            '[CorresTitle設定]--------------------------------------------------------------------------
            Try
                conn = New SqlConnection(DB_CONNECT_STRING)
                Dim cmd As SqlCommand = conn.CreateCommand()
                cmd.CommandText = "SELECT RFQCorresCode, Text FROM dbo.RFQCorres WHERE isDisabled = 0 ORDER BY SortOrder"
                conn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader
                EnqUser.Text = ""
                Do Until dr.Read = False
                    CorresTitle.Items.Add(New ListItem(dr("Text"), dr("RFQCorresCode")))
                Loop
            Finally
                If Not conn Is Nothing Then conn.Close()
            End Try

            '[ProductNumber.Textの設定]-----------------------------------------------------------------------
            Try
                conn = New SqlConnection(DB_CONNECT_STRING)
                Dim cmd As SqlCommand = conn.CreateCommand()
                cmd.CommandText = "SELECT ProductNumber,ProductName FROM v_RFQHeader WHERE RFQNumber = @RFQNumber"
                cmd.Parameters.AddWithValue("RFQNumber", RFQNumber.Value)
                conn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader
                ProductNumber.Text = ""
                ProductName.Text = ""
                If dr.Read = True Then
                    If Not TypeOf dr("ProductNumber") Is DBNull Then ProductNumber.Text = dr("ProductNumber")
                    If Not TypeOf dr("ProductName") Is DBNull Then ProductName.Text = dr("ProductName")
                End If
            Finally
                If Not conn Is Nothing Then conn.Close()
            End Try

            '[EnqUser.Textの設定]-----------------------------------------------------------------------
            '[EnqLocation.Textの設定]-------------------------------------------------------------------
            EnqUserID.Value = String.Empty
            EnqUser.Text = String.Empty
            EnqLocationCode.Value = String.Empty
            EnqLocation.Text = String.Empty

            Using sqlConn As New SqlConnection(DB_CONNECT_STRING)
                sqlConn.Open()

                Dim sqlStr As New StringBuilder
                sqlStr.AppendLine("SELECT")
                sqlStr.AppendLine("    RH.EnqUserID")
                sqlStr.AppendLine("    ,U.[Name] AS EnqUserName")
                sqlStr.AppendLine("    ,RH.EnqLocationCode")
                sqlStr.AppendLine("    ,L.[Name] AS EnqLocationName")
                sqlStr.AppendLine("FROM")
                sqlStr.AppendLine("    RFQHeader AS RH")
                sqlStr.AppendLine("        LEFT OUTER JOIN v_User AS U ON U.UserID = RH.EnqUserID")
                sqlStr.AppendLine("        LEFT OUTER JOIN s_Location AS L ON L.LocationCode = RH.EnqLocationCode")
                sqlStr.AppendLine("WHERE")
                sqlStr.AppendLine("    RH.RFQNumber = @RFQNumber")

                Using sqlCmd As SqlCommand = sqlConn.CreateCommand
                    sqlCmd.CommandText = sqlStr.ToString
                    sqlCmd.Parameters.Add("RFQNumber", SqlDbType.Int)
                    sqlCmd.Parameters("RFQNumber").Value = RFQNumber.Value

                    Using sqlReader As SqlDataReader = sqlCmd.ExecuteReader
                        If sqlReader.Read = True Then
                            EnqUserID.Value = sqlReader("EnqUserID").ToString
                            EnqUser.Text = sqlReader("EnqUserName").ToString
                            EnqLocationCode.Value = sqlReader("EnqLocationCode").ToString
                            EnqLocation.Text = sqlReader("EnqLocationName").ToString
                        End If
                    End Using
                End Using
            End Using

            '[QuoUser.Textの設定]-----------------------------------------------------------------------
            '[QuoLocation.Textの設定]-------------------------------------------------------------------
            QuoUserID.Value = String.Empty
            QuoUser.Text = String.Empty
            QuoLocationCode.Value = String.Empty
            QuoLocation.Text = String.Empty

            Using sqlConn As New SqlConnection(DB_CONNECT_STRING)
                sqlConn.Open()

                Dim sqlStr As New StringBuilder
                sqlStr.AppendLine("SELECT")
                sqlStr.AppendLine("    RH.QuoUserID")
                sqlStr.AppendLine("    ,U.[Name] AS QuoUserName")
                sqlStr.AppendLine("    ,RH.QuoLocationCode")
                sqlStr.AppendLine("    ,L.[Name] AS QuoLocationName")
                sqlStr.AppendLine("FROM")
                sqlStr.AppendLine("    RFQHeader AS RH")
                sqlStr.AppendLine("        LEFT OUTER JOIN v_User AS U ON U.UserID = RH.QuoUserID")
                sqlStr.AppendLine("        LEFT OUTER JOIN s_Location AS L ON L.LocationCode = RH.QuoLocationCode")
                sqlStr.AppendLine("WHERE")
                sqlStr.AppendLine("    RH.RFQNumber = @RFQNumber")

                Using sqlCmd As SqlCommand = sqlConn.CreateCommand
                    sqlCmd.CommandText = sqlStr.ToString
                    sqlCmd.Parameters.Add("RFQNumber", SqlDbType.Int)
                    sqlCmd.Parameters("RFQNumber").Value = RFQNumber.Value

                    Using sqlReader As SqlDataReader = sqlCmd.ExecuteReader
                        If sqlReader.Read = True Then
                            QuoUserID.Value = sqlReader("QuoUserID").ToString
                            QuoUser.Text = sqlReader("QuoUserName").ToString
                            QuoLocationCode.Value = sqlReader("QuoLocationCode").ToString
                            QuoLocation.Text = sqlReader("QuoLocationName").ToString
                        End If
                    End Using
                End Using
            End Using

            '[QuoUserが設定されていない場合は選択できないようにする]------------------------------------
            If QuoUser.Text.ToString = "" Then QuoUser.Enabled = False : QuoLocation.Enabled = False

            If EnqUser.Checked Then
                SetDefaultCCUsers(CInt(EnqUserID.Value))
            ElseIf QuoUser.Checked Then
                SetDefaultCCUsers(CInt(QuoUserID.Value))
            End If

        End If

        ShowList()

    End Sub

    Private Sub ShowList()

        Dim history As New TCIDataAccess.Join.RFQHistoryDispList
        history.Load(CInt(RFQNumber.Value))
        RFQHistory.DataSource = history
        RFQHistory.DataBind()

    End Sub

    Protected Sub Send_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Send.Click
        Dim st_RFQStatusCode As String = ""
        Dim StatusChangeDate As String = String.Empty
        Dim st_LocationCode As String = ""
        Dim st_UserID As String = ""
        Msg.Text = String.Empty

        '[Send実行確認]---------------------------------------------------------------------------------
        If Request.QueryString("Action") <> "Send" Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        '[必須入力項目の入力確認]-----------------------------------------------------------------------
        CorresNote.Text = CorresNote.Text.Trim    '入力データ前後の改行コード、タブコードを除去
        If CorresNote.Text = "" Then
            Msg.Text = "Note" + Common.ERR_REQUIRED_FIELD
            Exit Sub
        ElseIf CorresNote.Text.Length > Common.INT_3000 Then
            Msg.Text = "Note" + Common.ERR_OVER_3000
            Exit Sub
        End If

        '[Connectionの定義]-----------------------------------------------------------------------------
        Dim conn As SqlConnection = Nothing

        '[パラメータRFQNumberと同一の最大RFQHistoryNumberのレコードを検索]------------------------------
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()
            cmd.CommandText = "SELECT RFQStatusCode, convert(varchar,StatusChangeDate,121) as StatusChangeDate FROM dbo.RFQHistory WHERE (RFQHistoryNumber = (SELECT MAX(RFQHistoryNumber) AS MaxNo FROM dbo.RFQHistory AS RFQHistory_1 WHERE (RFQNumber = @RFQNumber)))"
            cmd.Parameters.AddWithValue("RFQNumber", RFQNumber.Value)
            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader
            If dr.Read = True Then
                st_RFQStatusCode = dr("RFQStatusCode")
                StatusChangeDate = dr("StatusChangeDate")
            End If
        Finally
            If Not conn Is Nothing Then conn.Close()
        End Try

        '[選択したUser,Locationを記憶する]--------------------------------------------------------------
        If EnqUser.Checked = True Then
            st_UserID = EnqUserID.Value
            st_LocationCode = EnqLocationCode.Value
        Else
            st_UserID = QuoUserID.Value
            st_LocationCode = QuoLocationCode.Value
        End If

        Dim ccUserID1 As Integer?
        Dim ccLocationCode1 As String = String.Empty
        If String.IsNullOrEmpty(CCUser1.SelectedValue) = False Then
            ccUserID1 = CInt(CCUser1.SelectedValue)
            ccLocationCode1 = CCLocation1.SelectedValue
        End If

        Dim ccUserID2 As Integer?
        Dim ccLocationCode2 As String = String.Empty
        If String.IsNullOrEmpty(CCUser2.SelectedValue) = False Then
            ccUserID2 = CInt(CCUser2.SelectedValue)
            ccLocationCode2 = CCLocation2.SelectedValue
        End If

        '[RFQHistory(を新規登録)]-----------------------------------------------------------------------
        Dim addrHistory As New TCIDataAccess.RFQHistory
        With addrHistory
            .RFQNumber = CInt(RFQNumber.Value)
            .RFQStatusCode = st_RFQStatusCode
            .StatusChangeDate = Convert.ToDateTime(StatusChangeDate)
            .RFQCorresCode = CorresTitle.SelectedValue
            .Note = CorresNote.Text
            .SendLocationCode = Session("LocationCode").ToString
            .SendUserID = CInt(Session("UserID"))
            .RcptLocationCode = st_LocationCode
            .RcptUserID = CInt(st_UserID)
            .isChecked = 0
            .AddrLocationCode = st_LocationCode
            .AddrUserID = CInt(st_UserID)
            .CCLocationCode1 = ccLocationCode1
            .CCUserID1 = ccUserID1
            .CCLocationCode2 = ccLocationCode2
            .CCUserID2 = ccUserID2
            .CreatedBy = CInt(Session("UserID"))
            .UpdatedBy = CInt(Session("UserID"))
            .Save()
        End With

        If ccUserID1 IsNot Nothing Then
            Dim cc1History As New TCIDataAccess.RFQHistory
            With cc1History
                .RFQNumber = CInt(RFQNumber.Value)
                .RFQStatusCode = st_RFQStatusCode
                .StatusChangeDate = Convert.ToDateTime(StatusChangeDate)
                .RFQCorresCode = CorresTitle.SelectedValue
                .Note = CorresNote.Text
                .SendLocationCode = Session("LocationCode").ToString
                .SendUserID = CInt(Session("UserID"))
                .RcptLocationCode = ccLocationCode1
                .RcptUserID = ccUserID1
                .isChecked = 0
                .AddrLocationCode = st_LocationCode
                .AddrUserID = CInt(st_UserID)
                .CCLocationCode1 = ccLocationCode1
                .CCUserID1 = ccUserID1
                .CCLocationCode2 = ccLocationCode2
                .CCUserID2 = ccUserID2
                .CreatedBy = CInt(Session("UserID"))
                .UpdatedBy = CInt(Session("UserID"))
                .Save()
            End With
        End If

        If ccUserID2 IsNot Nothing Then
            Dim cc2History As New TCIDataAccess.RFQHistory
            With cc2History
                .RFQNumber = CInt(RFQNumber.Value)
                .RFQStatusCode = st_RFQStatusCode
                .StatusChangeDate = Convert.ToDateTime(StatusChangeDate)
                .RFQCorresCode = CorresTitle.SelectedValue
                .Note = CorresNote.Text
                .SendLocationCode = Session("LocationCode").ToString
                .SendUserID = CInt(Session("UserID"))
                .RcptLocationCode = ccLocationCode2
                .RcptUserID = ccUserID2
                .isChecked = 0
                .AddrLocationCode = st_LocationCode
                .AddrUserID = CInt(st_UserID)
                .CCLocationCode1 = ccLocationCode1
                .CCUserID1 = ccUserID1
                .CCLocationCode2 = ccLocationCode2
                .CCUserID2 = ccUserID2
                .CreatedBy = CInt(Session("UserID"))
                .UpdatedBy = CInt(Session("UserID"))
                .Save()
            End With
        End If

        '[CorresNoteのClear]-----------------------------------------------------------------------------
        CorresNote.Text = ""

        ShowList()

    End Sub

    Private Sub SetDefaultCCUsers(ByVal UserID As Integer)

        Dim purchasingUser As New TCIDataAccess.Join.PurchasingUserDisp
        purchasingUser.Load(UserID)

        Dim userList As New TCIDataAccess.Join.PurchasingUserDispList
        userList.LoadEditUsers(purchasingUser.LocationCode)

        Dim cc1 As Boolean = False
        Dim cc2 As Boolean = False

        CCUser1.Items.Clear()
        If purchasingUser.DefaultCCUserID1 IsNot Nothing Then

            CCUser1.Items.Add(New ListItem())
            For Each user As TCIDataAccess.Join.PurchasingUserDisp In userList
                If user.UserID = purchasingUser.DefaultCCUserID1 Then
                    cc1 = True
                End If
                CCUser1.Items.Add(New ListItem(user.UserName, user.UserID))
            Next
            If cc1 Then CCUser1.SelectedValue = purchasingUser.DefaultCCUserID1.ToString

        End If

        CCUser2.Items.Clear()
        If purchasingUser.DefaultCCUserID2 IsNot Nothing Then

            CCUser2.Items.Add(New ListItem())
            For Each user As TCIDataAccess.Join.PurchasingUserDisp In userList
                If user.UserID = purchasingUser.DefaultCCUserID2 Then
                    cc2 = True
                End If
                CCUser2.Items.Add(New ListItem(user.UserName, user.UserID))
            Next
            If cc2 Then CCUser2.SelectedValue = purchasingUser.DefaultCCUserID2.ToString

        End If

        Dim locationList As New TCIDataAccess.s_LocationList
        locationList.Load()

        CCLocation1.Items.Clear()
        CCLocation2.Items.Clear()

        CCLocation1.Items.Add(New ListItem())
        CCLocation2.Items.Add(New ListItem())
        For Each location As TCIDataAccess.s_Location In locationList
            CCLocation1.Items.Add(New ListItem(location.Name, location.LocationCode))
            CCLocation2.Items.Add(New ListItem(location.Name, location.LocationCode))
        Next
        If cc1 Then CCLocation1.SelectedValue = purchasingUser.LocationCode
        If cc2 Then CCLocation2.SelectedValue = purchasingUser.LocationCode

    End Sub

    Protected Sub CCLocation1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CCLocation1.SelectedIndexChanged

        Dim locationCode As String = CCLocation1.SelectedValue

        Dim userList As New TCIDataAccess.Join.PurchasingUserDispList
        userList.LoadEditUsers(locationCode)

        CCUser1.Items.Clear()
        CCUser1.Items.Add(New ListItem())
        For Each user As TCIDataAccess.Join.PurchasingUserDisp In userList
            CCUser1.Items.Add(New ListItem(user.UserName, user.UserID))
        Next

    End Sub

    Protected Sub CCLocation2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CCLocation2.SelectedIndexChanged

        Dim locationCode As String = CCLocation2.SelectedValue

        Dim userList As New TCIDataAccess.Join.PurchasingUserDispList
        userList.LoadEditUsers(locationCode)

        CCUser2.Items.Clear()
        CCUser2.Items.Add(New ListItem())
        For Each user As TCIDataAccess.Join.PurchasingUserDisp In userList
            CCUser2.Items.Add(New ListItem(user.UserName, user.UserID))
        Next

    End Sub

    Protected Sub Addressee_CheckedChanged(sender As Object, e As EventArgs) Handles EnqUser.CheckedChanged, QuoUser.CheckedChanged

        Dim uid As Integer = 0

        If EnqUser.Checked Then
            uid = CInt(EnqUserID.Value)
        ElseIf QuoUser.Checked Then
            uid = CInt(QuoUserID.Value)
        Else
            Exit Sub
        End If

        SetDefaultCCUsers(uid)

    End Sub

    Protected Sub RFQHistory_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles RFQHistory.ItemDataBound

        Dim checkLink As LinkButton = DirectCast(e.Item.FindControl("Check"), LinkButton)
        Dim isChecked As HiddenField = DirectCast(e.Item.FindControl("isChecked"), HiddenField)
        Dim rcptUserID As HiddenField = DirectCast(e.Item.FindControl("RcptUserID"), HiddenField)

        Dim locationCode As String = String.Empty
        Dim editable As Boolean = False

        If Not String.IsNullOrEmpty(rcptUserID.Value) Then
            Dim user As New TCIDataAccess.Join.PurchasingUserDisp
            user.Load(CInt(rcptUserID.Value))

            locationCode = user.LocationCode
            editable = user.RFQCorrespondenceEditable
        End If

        If isChecked.Value = "False" AndAlso rcptUserID.Value = Session("UserID").ToString Then
            checkLink.Visible = True
        ElseIf isChecked.Value = "False" AndAlso editable = True AndAlso locationCode = Session("LocationCode") Then
            checkLink.Visible = True
        Else
            checkLink.Visible = False
        End If

    End Sub

    Protected Sub RFQHistory_ItemCommand(sender As Object, e As ListViewCommandEventArgs) Handles RFQHistory.ItemCommand

        If Request.QueryString("Action").Equals("Check") = False Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        Dim historyNumber As HiddenField = DirectCast(e.Item.FindControl("RFQHistoryNumber"), HiddenField)

        Dim history As New TCIDataAccess.RFQHistory
        history.Load(CInt(historyNumber.Value))
        history.isChecked = True
        history.Save()

        ShowList()

    End Sub

End Class
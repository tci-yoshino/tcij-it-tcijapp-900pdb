Option Explicit On

Imports System.Data.SqlClient
Imports Purchase.Common
Imports TCICommon.Func

Partial Public Class POCorrespondence
    Inherits CommonPage

    Dim st_SqlStr As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            '[PONumberの取込]--------------------------------------------------------------------------
            If Request.QueryString("PONumber") <> "" Then
                hd_PONumber.Value = Request.QueryString("PONumber")
            Else
                POUser.Enabled = False
                SOUser.Enabled = False
                CorresTitle.Enabled = False
                CorresNote.Enabled = False
                Send.Enabled = False
                Msg.Text = Common.ERR_INVALID_PARAMETER
                Exit Sub
            End If

            '[Connectionの定義]-------------------------------------------------------------------------
            Dim conn As SqlConnection = Nothing

            '[DefaultでPOUser.Checked設定]-------------------------------------------------------------
            POUser.Checked = True

            '[CorresTitle設定]--------------------------------------------------------------------------
            Try
                conn = New SqlConnection(DB_CONNECT_STRING)
                Dim cmd As SqlCommand = conn.CreateCommand()
                cmd.CommandText = "SELECT POCorresCode, Text FROM dbo.POCorres Order BY SortOrder"
                conn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader
                POUser.Text = ""
                Do Until dr.Read = False
                    CorresTitle.Items.Add(New ListItem(dr("Text"), dr("POCorresCode")))
                Loop
            Finally
                If Not conn Is Nothing Then conn.Close()
            End Try

            '[POUser.Textの設定]-----------------------------------------------------------------------
            Try
                conn = New SqlConnection(DB_CONNECT_STRING)
                Dim cmd As SqlCommand = conn.CreateCommand()
                cmd.CommandText = "SELECT dbo.v_User.Name FROM dbo.PO LEFT OUTER JOIN dbo.v_User ON dbo.PO.POUserID = dbo.v_User.UserID WHERE (dbo.PO.PONumber = @PONumber)"
                cmd.Parameters.AddWithValue("PONumber", hd_PONumber.Value)
                conn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader
                POUser.Text = ""
                If dr.Read = True Then
                    If Not TypeOf dr("Name") Is DBNull Then POUser.Text = dr("Name")
                End If
            Finally
                If Not conn Is Nothing Then conn.Close()
            End Try

            '[POLocation.Textの設定]-------------------------------------------------------------------
            Try
                conn = New SqlConnection(DB_CONNECT_STRING)
                Dim cmd As SqlCommand = conn.CreateCommand()
                cmd.CommandText = "SELECT dbo.s_Location.Name FROM dbo.PO LEFT OUTER JOIN dbo.s_Location ON dbo.PO.POLocationCode = dbo.s_Location.LocationCode WHERE (dbo.PO.PONumber = @PONumber)"
                cmd.Parameters.AddWithValue("PONumber", hd_PONumber.Value)
                conn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader
                POLocation.Text = ""
                If dr.Read = True Then
                    If Not TypeOf dr("Name") Is DBNull Then POLocation.Text = "(" + dr("Name") + ")"
                End If
            Finally
                If Not conn Is Nothing Then conn.Close()
            End Try

            '[SOUser.Textの設定]-----------------------------------------------------------------------
            Try
                conn = New SqlConnection(DB_CONNECT_STRING)
                Dim cmd As SqlCommand = conn.CreateCommand()
                cmd.CommandText = "SELECT dbo.v_User.Name FROM dbo.PO LEFT OUTER JOIN dbo.v_User ON dbo.PO.SOUserID = dbo.v_User.UserID WHERE (dbo.PO.PONumber = @PONumber)"
                cmd.Parameters.AddWithValue("PONumber", hd_PONumber.Value)
                conn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader
                SOUser.Text = ""
                If dr.Read = True Then
                    If Not TypeOf dr("Name") Is DBNull Then SOUser.Text = dr("Name")
                End If
            Finally
                If Not conn Is Nothing Then conn.Close()
            End Try

            '[SOLocation.Textの設定]-------------------------------------------------------------------
            Try
                conn = New SqlConnection(DB_CONNECT_STRING)
                Dim cmd As SqlCommand = conn.CreateCommand()
                cmd.CommandText = "SELECT dbo.s_Location.Name FROM dbo.PO LEFT OUTER JOIN dbo.s_Location ON dbo.PO.SOLocationCode = dbo.s_Location.LocationCode WHERE (dbo.PO.PONumber = @PONumber)"
                cmd.Parameters.AddWithValue("PONumber", hd_PONumber.Value)
                conn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader
                SOLocation.Text = ""
                If dr.Read = True Then
                    If Not TypeOf dr("Name") Is DBNull Then SOLocation.Text = "(" + dr("Name") + ")"
                End If
            Finally
                If Not conn Is Nothing Then conn.Close()
            End Try

            '[SOUserが設定されていない場合は選択できないようにする]------------------------------------
            If SOUser.Text.ToString = "" Then SOUser.Enabled = False : SOLocation.Enabled = False
        End If

        '[SrcPOHistoryにSelectCommand設定]-------------------------------------------------------------
        SrcPOHistory.SelectCommand = "SELECT dbo.POStatus.Text AS Status, dbo.POHistory.CreateDate AS Date, dbo.v_User.Name AS Sender, '(' + dbo.s_Location.Name + ')' AS SenderLocation, v_User_1.Name AS Addressee, '(' + s_Location_1.Name + ')' AS AddresseeLocation, dbo.POCorres.Text AS Title, REPLACE(dbo.POHistory.Note,Char(10),'<br>') AS Notes, dbo.POHistory.isChecked, dbo.POHistory.RcptUserID, dbo.POHistory.POHistoryNumber " & _
                                     "FROM dbo.POHistory LEFT OUTER JOIN " & _
                                     "dbo.POCorres ON dbo.POHistory.POCorresCode = dbo.POCorres.POCorresCode LEFT OUTER JOIN " & _
                                     "dbo.s_Location AS s_Location_1 ON dbo.POHistory.RcptLocationCode = s_Location_1.LocationCode LEFT OUTER JOIN " & _
                                     "dbo.s_Location ON dbo.POHistory.SendLocationCode = dbo.s_Location.LocationCode LEFT OUTER JOIN " & _
                                     "dbo.v_User AS v_User_1 ON dbo.POHistory.RcptUserID = v_User_1.UserID LEFT OUTER JOIN " & _
                                     "dbo.v_User ON dbo.POHistory.SendUserID = dbo.v_User.UserID LEFT OUTER JOIN " & _
                                     "dbo.POStatus ON dbo.POHistory.POStatusCode = dbo.POStatus.POStatusCode " & _
                                     "WHERE (dbo.POHistory.PONumber = @PONumber) " & _
                                     "ORDER BY dbo.POHistory.POHistoryNumber DESC"

    End Sub

    Private Sub Set_isChecked(ByVal sender As Object, ByVal e As EventArgs) Handles POHistory.ItemDataBound
        '[POHistoryの行編集]--------------------------------------------------------------------
        Dim lb As LinkButton = DirectCast(DirectCast(DirectCast(e, ListViewItemEventArgs).Item.FindControl("Check"), System.Web.UI.Control), LinkButton)
        Dim isChecked As HiddenField = DirectCast(DirectCast(DirectCast(e, ListViewItemEventArgs).Item.FindControl("ischecked"), System.Web.UI.Control), HiddenField)
        Dim RcptUserID As HiddenField = DirectCast(DirectCast(DirectCast(e, ListViewItemEventArgs).Item.FindControl("RcptUserID"), System.Web.UI.Control), HiddenField)
        If isChecked.Value = "False" And RcptUserID.Value = Session("UserID") Then
            lb.Visible = True
        Else
            lb.Visible = False
        End If
    End Sub

    Private Sub UpdateChecked(ByVal sender As Object, ByVal e As EventArgs) Handles POHistory.ItemCommand
        If Request.QueryString("Action") = "Check" Then
            Dim POHistoryNumber As HiddenField = DirectCast(DirectCast(DirectCast(e, ListViewCommandEventArgs).Item.FindControl("POHistoryNumber"), System.Web.UI.Control), HiddenField)
            SrcPOHistory.UpdateCommand = "UPDATE POHistory SET isChecked=1 WHERE POHistoryNumber='" & POHistoryNumber.Value & "'"
            SrcPOHistory.Update()
        Else
            Msg.Text = Common.ERR_INVALID_PARAMETER
        End If
    End Sub

    Protected Sub Send_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Send.Click
        Dim st_POStatusCode As String = ""
        Dim st_StatusChangeDate As Date
        Dim st_LocationCode As String = ""
        Dim st_UserID As String = ""

        '[Send実行確認]---------------------------------------------------------------------------------
        If Request.QueryString("Action") <> "Send" Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
        End If

        '[CorresNoteのCheck]----------------------------------------------------------------------------
        If Trim(CorresNote.Text) = "" Then
            Msg.Text = "Note" + Common.ERR_REQUIRED_FIELD
            Exit Sub
        ElseIf CorresNote.Text.Length > 3000 Then
            Msg.Text = "Noteの文字数が3000を超えています。"
            Exit Sub
        End If

        '[Connectionの定義]-----------------------------------------------------------------------------
        Dim conn As SqlConnection = Nothing

        '[パラメータPONumberと同一の最大POHistoryNumberのレコードを検索]--------------------------------
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()
            cmd.CommandText = "SELECT POStatusCode, StatusChangeDate FROM dbo.POHistory WHERE (POHistoryNumber = (SELECT MAX(POHistoryNumber) AS MaxNo FROM dbo.POHistory AS POHistory_1 WHERE (PONumber = @PONumber)))"
            cmd.Parameters.AddWithValue("PONumber", hd_PONumber.Value)
            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader
            If dr.Read = True Then
                st_POStatusCode = dr("POStatusCode")
                st_StatusChangeDate = dr("StatusChangeDate")
            End If
        Finally
            If Not conn Is Nothing Then conn.Close()
        End Try

        '[選択したUser,Locationを記憶する]--------------------------------------------------------------
        If POUser.Checked = True Then
            st_UserID = POUser.Text.ToString
            st_LocationCode = POLocation.Text.ToString
        Else
            st_UserID = SOUser.Text.ToString
            st_LocationCode = SOLocation.Text.ToString
        End If

        '[選択したUserのUserIDを取得する]---------------------------------------------------------------
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()
            cmd.CommandText = "SELECT v_User.UserID FROM dbo.v_User WHERE (Name = @Name)"
            cmd.Parameters.AddWithValue("Name", st_UserID)
            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader
            If dr.Read = True Then
                st_UserID = dr("UserID")
            End If
        Finally
            If Not conn Is Nothing Then conn.Close()
        End Try

        '[選択したLocationのLocationCodeを取得する]-----------------------------------------------------
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()
            cmd.CommandText = "SELECT LocationCode FROM dbo.s_Location WHERE (Name = @Name)"
            cmd.Parameters.AddWithValue("Name", Mid(st_LocationCode, 2, Len(st_LocationCode) - 2))
            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader
            If dr.Read = True Then
                st_LocationCode = dr("LocationCode")
            End If
        Finally
            If Not conn Is Nothing Then conn.Close()
        End Try

        '[POHistory(を新規登録)]-----------------------------------------------------------------------
        st_SqlStr = "INSERT INTO POHistory (PONumber,POStatusCode,StatusChangeDate,POCorresCode,Note,SendLocationCode,SendUserID,RcptLocationCode,RcptUserID,isChecked,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ("
        st_SqlStr = st_SqlStr + "'" + Trim(Str(hd_PONumber.Value)) + "',"
        st_SqlStr = st_SqlStr + "'" + st_POStatusCode + "',"
        st_SqlStr = st_SqlStr + "'" + st_StatusChangeDate + "',"
        st_SqlStr = st_SqlStr + "'" + CorresTitle.SelectedValue + "',"
        st_SqlStr = st_SqlStr + " @Note , "
        st_SqlStr = st_SqlStr + "'" + Session("LocationCode") + "',"
        st_SqlStr = st_SqlStr + Session("UserID") + ","
        st_SqlStr = st_SqlStr + "'" + st_LocationCode + "',"
        st_SqlStr = st_SqlStr + st_UserID + ","
        st_SqlStr = st_SqlStr + "0,"
        st_SqlStr = st_SqlStr + Session("UserID") + ",'" + Now() + "'," + Session("UserID") + ",'" + Now() + "')"
        SrcPOHistory.InsertParameters.Add("Note", CorresNote.Text)
        SrcPOHistory.InsertCommand = st_SqlStr
        SrcPOHistory.Insert()

        '[CorresNoteのClear]-----------------------------------------------------------------------------
        CorresNote.Text = ""
    End Sub
End Class
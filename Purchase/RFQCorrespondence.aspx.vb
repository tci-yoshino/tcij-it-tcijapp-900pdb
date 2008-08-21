Option Explicit On

Imports System.Data.SqlClient
Imports Purchase.Common
Imports TCICommon.Func

Partial Public Class RFQCorrespondence
    Inherits CommonPage

    Dim st_SqlStr As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            '[RFQNumberの取込]--------------------------------------------------------------------------
            If Request.QueryString("RFQNumber") <> "" Then
                hd_RFQNumber.Value = Request.QueryString("RFQNumber")
            Else
                Msg.Text = Common.ERR_INVALID_PARAMETER
                Exit Sub
            End If

            '[Connectionの定義]-------------------------------------------------------------------------
            Dim conn As SqlConnection = Nothing

            '[DefaultでEnqUser.Checked設定]-------------------------------------------------------------
            EnqUser.Checked = True

            '[CorresTitle設定]--------------------------------------------------------------------------
            Try
                conn = New SqlConnection(DB_CONNECT_STRING)
                Dim cmd As SqlCommand = conn.CreateCommand()
                cmd.CommandText = "SELECT RFQCorresCode, Text FROM dbo.RFQCorres Order BY SortOrder"
                conn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader
                EnqUser.Text = ""
                Do Until dr.Read = False
                    CorresTitle.Items.Add(New ListItem(dr("Text"), dr("RFQCorresCode")))
                Loop
            Finally
                If Not conn Is Nothing Then conn.Close()
            End Try

            '[EnqUser.Textの設定]-----------------------------------------------------------------------
            Try
                conn = New SqlConnection(DB_CONNECT_STRING)
                Dim cmd As SqlCommand = conn.CreateCommand()
                cmd.CommandText = "SELECT v_User.Name FROM RFQHeader LEFT OUTER JOIN v_User ON RFQHeader.EnqUserID = v_User.UserID WHERE (RFQHeader.RFQNumber = @RFQNumber)"
                cmd.Parameters.AddWithValue("RFQNumber", hd_RFQNumber.Value)
                conn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader
                EnqUser.Text = ""
                If dr.Read = True Then
                    If Not TypeOf dr("Name") Is DBNull Then EnqUser.Text = dr("Name")
                End If
            Finally
                If Not conn Is Nothing Then conn.Close()
            End Try

            '[EnqLocation.Textの設定]-------------------------------------------------------------------
            Try
                conn = New SqlConnection(DB_CONNECT_STRING)
                Dim cmd As SqlCommand = conn.CreateCommand()
                cmd.CommandText = "SELECT s_Location.Name FROM RFQHeader LEFT OUTER JOIN s_Location ON RFQHeader.EnqLocationCode = s_Location.LocationCode WHERE (RFQHeader.RFQNumber = @RFQNumber)"
                cmd.Parameters.AddWithValue("RFQNumber", hd_RFQNumber.Value)
                conn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader
                EnqLocation.Text = ""
                If dr.Read = True Then
                    If Not TypeOf dr("Name") Is DBNull Then EnqLocation.Text = "(" + dr("Name") + ")"
                End If
            Finally
                If Not conn Is Nothing Then conn.Close()
            End Try

            '[QuoUser.Textの設定]-----------------------------------------------------------------------
            Try
                conn = New SqlConnection(DB_CONNECT_STRING)
                Dim cmd As SqlCommand = conn.CreateCommand()
                cmd.CommandText = "SELECT v_User.Name FROM RFQHeader LEFT OUTER JOIN v_User ON RFQHeader.QuoUserID = v_User.UserID WHERE (RFQHeader.RFQNumber = @RFQNumber)"
                cmd.Parameters.AddWithValue("RFQNumber", hd_RFQNumber.Value)
                conn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader
                QuoUser.Text = ""
                If dr.Read = True Then
                    If Not TypeOf dr("Name") Is DBNull Then QuoUser.Text = dr("Name")
                End If
            Finally
                If Not conn Is Nothing Then conn.Close()
            End Try

            '[QuoLocation.Textの設定]-------------------------------------------------------------------
            Try
                conn = New SqlConnection(DB_CONNECT_STRING)
                Dim cmd As SqlCommand = conn.CreateCommand()
                cmd.CommandText = "SELECT s_Location.Name FROM RFQHeader LEFT OUTER JOIN s_Location ON RFQHeader.QuoLocationCode = s_Location.LocationCode WHERE (RFQHeader.RFQNumber = @RFQNumber)"
                cmd.Parameters.AddWithValue("RFQNumber", hd_RFQNumber.Value)
                conn.Open()
                Dim dr As SqlDataReader = cmd.ExecuteReader
                QuoLocation.Text = ""
                If dr.Read = True Then
                    If Not TypeOf dr("Name") Is DBNull Then QuoLocation.Text = "(" + dr("Name") + ")"
                End If
            Finally
                If Not conn Is Nothing Then conn.Close()
            End Try

            '[QuoUserが設定されていない場合は選択できないようにする]------------------------------------
            If QuoUser.Text.ToString = "" Then QuoUser.Enabled = False : QuoLocation.Enabled = False
        End If

        '[SrcRFQHistoryにSelectCommand設定]-------------------------------------------------------------
        SrcRFQHistory.SelectCommand = "SELECT dbo.RFQStatus.Text AS Status, dbo.RFQHistory.CreateDate AS Date, dbo.v_User.Name + '          (' + dbo.v_User.LocationName + ')' AS Sender, v_User_1.Name + '          (' + dbo.v_User.LocationName + ')' AS Addressee, dbo.RFQHistory.Note AS Notes, dbo.RFQHistory.isChecked, dbo.RFQHistory.RcptUserID, dbo.RFQHistory.RFQHistoryNumber " & _
                                      "FROM dbo.RFQHistory LEFT OUTER JOIN dbo.RFQStatus ON dbo.RFQHistory.RFQStatusCode = dbo.RFQStatus.RFQStatusCode LEFT OUTER JOIN dbo.v_User AS v_User_1 ON dbo.RFQHistory.RcptUserID = v_User_1.UserID LEFT OUTER JOIN dbo.v_User ON dbo.RFQHistory.CreatedBy = dbo.v_User.UserID " & _
                                      "WHERE (dbo.RFQHistory.RFQNumber = " & hd_RFQNumber.Value & ") " & _
                                      "ORDER BY dbo.RFQHistory.RFQHistoryNumber DESC"
    End Sub

    Private Sub Set_isChecked(ByVal sender As Object, ByVal e As EventArgs) Handles RFQHistory.ItemDataBound
        '[RFQHistoryの行編集]-------------------------------------------------------------------
        Dim lb As LinkButton = DirectCast(DirectCast(DirectCast(e, ListViewItemEventArgs).Item.FindControl("Check"), System.Web.UI.Control), LinkButton)
        Dim isChecked As HiddenField = DirectCast(DirectCast(DirectCast(e, ListViewItemEventArgs).Item.FindControl("ischecked"), System.Web.UI.Control), HiddenField)
        Dim RcptUserID As HiddenField = DirectCast(DirectCast(DirectCast(e, ListViewItemEventArgs).Item.FindControl("RcptUserID"), System.Web.UI.Control), HiddenField)
        If isChecked.Value = "False" And RcptUserID.Value = Session("UserID") Then
            lb.Visible = True
        Else
            lb.Visible = False
        End If
    End Sub

    Private Sub UpdateChecked(ByVal sender As Object, ByVal e As EventArgs) Handles RFQHistory.ItemCommand
        If Request.QueryString("Action") = "Check" Then
            Dim RFQHistoryNumber As HiddenField = DirectCast(DirectCast(DirectCast(e, ListViewCommandEventArgs).Item.FindControl("RFQHistoryNumber"), System.Web.UI.Control), HiddenField)
            SrcRFQHistory.UpdateCommand = "UPDATE RFQHistory SET isChecked=1 WHERE RFQHistoryNumber='" & RFQHistoryNumber.Value & "'"
            SrcRFQHistory.Update()
        Else
            Msg.Text = Common.ERR_INVALID_PARAMETER
        End If
    End Sub

    Protected Sub Send_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Send.Click
        Dim st_RFQStatusCode As String = ""
        Dim st_StatusChangeDate As Date
        Dim st_LocationCode As String = ""
        Dim st_UserID As String = ""
        '[Send実行確認]---------------------------------------------------------------------------------
        If Request.QueryString("Action") <> "Send" Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
        End If

        '[Connectionの定義]-------------------------------------------------------------------------
        Dim conn As SqlConnection = Nothing

        '[パラメータRFQNumberと同一の最大RFQHistoryNumberのレコードを検索]------------------------------
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()
            cmd.CommandText = "SELECT RFQStatusCode, StatusChangeDate FROM dbo.RFQHistory WHERE (RFQHistoryNumber = (SELECT MAX(RFQHistoryNumber) AS MaxNo FROM dbo.RFQHistory AS RFQHistory_1 WHERE (RFQNumber = @RFQNumber)))"
            cmd.Parameters.AddWithValue("RFQNumber", hd_RFQNumber.Value)
            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader
            If dr.Read = True Then
                st_RFQStatusCode = dr("RFQStatusCode")
                st_StatusChangeDate = dr("StatusChangeDate")
            End If
        Finally
            If Not conn Is Nothing Then conn.Close()
        End Try

        '[選択したUser,Locationを記憶する]--------------------------------------------------------------
        If EnqUser.Checked = True Then
            st_UserID = EnqUser.Text.ToString
            st_LocationCode = EnqLocation.Text.ToString
        Else
            st_UserID = QuoUser.Text.ToString
            st_LocationCode = QuoLocation.Text.ToString
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

        '[RFQHistory(を新規登録)]-----------------------------------------------------------------------
        st_SqlStr = "INSERT INTO RFQHistory (RFQNumber,RFQStatusCode,StatusChangeDate,RFQCorresCode,Note,SendLocationCode,SendUserID,RcptLocationCode,RcptUserID,isChecked,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ("
        st_SqlStr = st_SqlStr + "'" + Trim(Str(hd_RFQNumber.Value)) + "',"
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
        SrcRFQHistory.InsertCommand = st_SqlStr
        SrcRFQHistory.Insert()
        Msg.Text = "表示データを登録しました"
    End Sub

End Class
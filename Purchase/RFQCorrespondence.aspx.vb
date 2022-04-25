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
                hd_RFQNumber.Value = Request.QueryString("RFQNumber")
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
                        sqlCmd.Parameters.AddWithValue("RFQNumber", hd_RFQNumber.Value)
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
                cmd.Parameters.AddWithValue("RFQNumber", hd_RFQNumber.Value)
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
                    sqlCmd.Parameters("RFQNumber").Value = hd_RFQNumber.Value

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
                    sqlCmd.Parameters("RFQNumber").Value = hd_RFQNumber.Value

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
        End If

        '[SrcRFQHistoryにSelectCommand設定]-------------------------------------------------------------
        SrcRFQHistory.SelectCommand = "SELECT dbo.RFQStatus.Text AS Status, dbo.RFQHistory.CreateDate AS Date, dbo.v_UserAll.Name AS Sender, '(' + dbo.s_Location.Name + ')' AS SenderLocation, v_UserAll_1.Name AS Addressee, '(' + s_Location_1.Name + ')' AS AddresseeLocation, dbo.RFQCorres.Text AS Title, REPLACE(dbo.RFQHistory.Note,Char(10),'<br>') AS Notes, dbo.RFQHistory.isChecked, dbo.RFQHistory.RcptUserID, dbo.RFQHistory.RFQHistoryNumber " & _
                                      "FROM dbo.RFQHistory LEFT OUTER JOIN " & _
                                      "dbo.RFQCorres ON dbo.RFQHistory.RFQCorresCode = dbo.RFQCorres.RFQCorresCode LEFT OUTER JOIN " & _
                                      "dbo.s_Location AS s_Location_1 ON dbo.RFQHistory.RcptLocationCode = s_Location_1.LocationCode LEFT OUTER JOIN " & _
                                      "dbo.s_Location ON dbo.RFQHistory.SendLocationCode = dbo.s_Location.LocationCode LEFT OUTER JOIN " & _
                                      "dbo.v_UserAll AS v_UserAll_1 ON dbo.RFQHistory.RcptUserID = v_UserAll_1.UserID LEFT OUTER JOIN " & _
                                      "dbo.v_UserAll ON dbo.RFQHistory.SendUserID = dbo.v_UserAll.UserID LEFT OUTER JOIN " & _
                                      "dbo.RFQStatus ON dbo.RFQHistory.RFQStatusCode = dbo.RFQStatus.RFQStatusCode " & _
                                      "WHERE (dbo.RFQHistory.RFQNumber = @RFQNumber) " & _
                                      "ORDER BY dbo.RFQHistory.RFQHistoryNumber DESC"

    End Sub

    Private Sub Set_isChecked(ByVal sender As Object, ByVal e As EventArgs) Handles RFQHistory.ItemDataBound
        '[RFQHistoryの行編集]-------------------------------------------------------------------
        Dim lb As LinkButton = DirectCast(DirectCast(DirectCast(e, ListViewItemEventArgs).Item.FindControl("Check"), System.Web.UI.Control), LinkButton)
        Dim isChecked As HiddenField = DirectCast(DirectCast(DirectCast(e, ListViewItemEventArgs).Item.FindControl("ischecked"), System.Web.UI.Control), HiddenField)
        Dim RcptUserID As HiddenField = DirectCast(DirectCast(DirectCast(e, ListViewItemEventArgs).Item.FindControl("RcptUserID"), System.Web.UI.Control), HiddenField)

        Dim locationCode As String = String.Empty
        Dim editable As Boolean = False

        If Not String.IsNullOrEmpty(RcptUserID.Value) Then

            Dim sqlStr As New StringBuilder
            sqlStr.AppendLine("SELECT")
            sqlStr.AppendLine("    LocationCode")
            sqlStr.AppendLine("    ,RFQCorrespondenceEditable")
            sqlStr.AppendLine("FROM")
            sqlStr.AppendLine("    v_UserAll")
            sqlStr.AppendLine("WHERE")
            sqlStr.AppendLine("    UserID = @UserID")

            Using sqlConn As New SqlConnection(DB_CONNECT_STRING)
                sqlConn.Open()

                Using sqlCmd As SqlCommand = sqlConn.CreateCommand
                    sqlCmd.CommandText = sqlStr.ToString
                    sqlCmd.Parameters.Add("UserID", SqlDbType.Int)
                    sqlCmd.Parameters("UserID").Value = RcptUserID.Value

                    Using sqlReader As SqlDataReader = sqlCmd.ExecuteReader
                        If sqlReader.Read = True Then
                            locationCode = sqlReader("LocationCode").ToString
                            editable = CBool(sqlReader("RFQCorrespondenceEditable"))
                        End If
                    End Using
                End Using
            End Using

        End If

        If isChecked.Value = "False" AndAlso RcptUserID.Value = Session("UserID") Then
            lb.Visible = True
        ElseIf isChecked.Value = "False" AndAlso editable = True AndAlso locationCode = Session("LocationCode") Then
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
            cmd.Parameters.AddWithValue("RFQNumber", hd_RFQNumber.Value)
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

        '[RFQHistory(を新規登録)]-----------------------------------------------------------------------
        st_SqlStr = "INSERT INTO RFQHistory (RFQNumber,RFQStatusCode,StatusChangeDate,RFQCorresCode,Note,SendLocationCode,SendUserID,RcptLocationCode,RcptUserID,isChecked,CreatedBy,UpdatedBy) values ("
        st_SqlStr = st_SqlStr + "'" + Trim(Str(hd_RFQNumber.Value)) + "',"
        st_SqlStr = st_SqlStr + "'" + st_RFQStatusCode + "',"
        st_SqlStr = st_SqlStr + "'" + StatusChangeDate + "',"
        st_SqlStr = st_SqlStr + "'" + CorresTitle.SelectedValue + "',"
        st_SqlStr = st_SqlStr + "@Note,"
        st_SqlStr = st_SqlStr + "'" + Session("LocationCode") + "',"
        st_SqlStr = st_SqlStr + Session("UserID") + ","
        st_SqlStr = st_SqlStr + "'" + st_LocationCode + "',"
        st_SqlStr = st_SqlStr + st_UserID + ","
        st_SqlStr = st_SqlStr + "0,"
        st_SqlStr = st_SqlStr + Session("UserID") + "," + Session("UserID") + ")"
        SrcRFQHistory.InsertParameters.Add("Note", CorresNote.Text)
        SrcRFQHistory.InsertCommand = st_SqlStr
        SrcRFQHistory.Insert()

        '[CorresNoteのClear]-----------------------------------------------------------------------------
        CorresNote.Text = ""
    End Sub

End Class
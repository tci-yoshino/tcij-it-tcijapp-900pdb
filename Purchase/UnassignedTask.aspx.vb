Imports Purchase.Common

Partial Public Class UnassignedTask
    Inherits CommonPage

    Private Const ASSIGN_ACTION As String = "Assign"
    Private ds_User As New DataSet
    Private ds_UserConfidential As New DataSet

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim st_Login_Location As String = Session("LocationCode").ToString()

        Msg.Text = ""

        SrcRFQ.SelectCommand = CreateRHQHeaderSelectSQL(st_Login_Location)
        SrcPO.SelectCommand = CreatePOSelectSQL(st_Login_Location)
        ds_User = GetUser(st_Login_Location, False)
        ds_UserConfidential = GetUser(st_Login_Location, True)

    End Sub

    Protected Sub RFQAssign_Click(ByVal source As Object, ByVal e As ListViewCommandEventArgs) Handles RFQList.ItemCommand

        Dim st_Action As String = String.Empty ' Action 格納変数
        Dim st_RFQNumber As String = CType(e.Item.FindControl("RFQNumber"), Label).Text
        Dim st_UpdateDate As String = CType(e.Item.FindControl("UpdateDate"), HiddenField).Value
        Dim st_QuoUserID As String = CType(e.Item.FindControl("QuoUser"), DropDownList).SelectedValue

        ' Action 取得
        If Request.RequestType = "POST" Then
            st_Action = IIf(Request.Form("Action") = Nothing, "", Request.Form("Action"))
        ElseIf Request.RequestType = "GET" Then
            st_Action = IIf(Request.QueryString("Action") = Nothing, "", Request.QueryString("Action"))
        End If

        ' Action が "Assign"でない場合はエラー
        If st_Action <> ASSIGN_ACTION Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            RFQList.DataBind()
            Exit Sub
        End If

        ' Update Chack
        If Not Common.IsLatestData("RFQHeader", "RFQNumber", st_RFQNumber, st_UpdateDate) Then
            Msg.Text = Common.ERR_UPDATED_BY_ANOTHER_USER
            RFQList.DataBind()
            Exit Sub
        End If

        ' SQLパラメータ設定
        SrcRFQ.UpdateParameters.Add("RFQNumber", st_RFQNumber)
        SrcRFQ.UpdateParameters.Add("QuoUserID", st_QuoUserID)
        SrcRFQ.UpdateParameters.Add("UpdatedBy", Session("UserID"))
        SrcRFQ.UpdateParameters.Add("UpdateDate", st_UpdateDate)

        ' Update 文作成
        SrcRFQ.UpdateCommand = CreateRFQQuoUserUpdateSQL()
        SrcRFQ.Update()

    End Sub

    Protected Sub POAssign_Click(ByVal source As Object, ByVal e As ListViewCommandEventArgs) Handles POList.ItemCommand

        Dim st_action As String = "" ' Action 格納変数
        Dim st_PONumber As String = CType(e.Item.FindControl("PONumber"), Label).Text
        Dim st_SOUserID As String = CType(e.Item.FindControl("SOUser"), DropDownList).SelectedValue
        Dim st_UpdateDate As String = CType(e.Item.FindControl("UpdateDate"), HiddenField).Value

        ' Action取得
        If Request.RequestType = "POST" Then
            st_action = IIf(Request.Form("Action") = Nothing, "", Request.Form("Action"))
        ElseIf Request.RequestType = "GET" Then
            st_action = IIf(Request.QueryString("Action") = Nothing, "", Request.QueryString("Action"))
        End If

        ' Action が "Assign"でない場合はエラー
        If st_action <> ASSIGN_ACTION Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            POList.DataBind()
            Exit Sub
        End If

        ' Update Chack
        If Not Common.IsLatestData("PO", "PONumber", st_PONumber, st_UpdateDate) Then
            Msg.Text = Common.ERR_UPDATED_BY_ANOTHER_USER
            POList.DataBind()
            Exit Sub
        End If

        ' SQLパラメータ設定
        SrcPO.UpdateParameters.Add("PONumber", st_PONumber)
        SrcPO.UpdateParameters.Add("SOUserID", st_SOUserID)
        SrcPO.UpdateParameters.Add("UpdatedBy", Session("UserID"))
        SrcPO.UpdateParameters.Add("UpdateDate", st_UpdateDate)

        ' Update 文作成
        SrcPO.UpdateCommand = CreatePOSOUserUpdateSQL()
        SrcPO.Update()

    End Sub

    Protected Sub SrcRFQ_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcRFQ.Selecting
        e.Command.CommandTimeout = 0
    End Sub

    Protected Sub SrcPO_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcPO.Selecting
        e.Command.CommandTimeout = 0
    End Sub

    ' ユーザ選択プルダウン用のユーザリストを取得する
    Private Function GetUser(ByVal LocationCode As String, ByVal IsConfidential As Boolean) As DataSet

        Dim ds As DataSet = New DataSet

        Dim sqlStr As StringBuilder = New StringBuilder
        sqlStr.AppendLine("SELECT")
        sqlStr.AppendLine("  UserID,")
        sqlStr.AppendLine("  Name")
        sqlStr.AppendLine("FROM ")
        sqlStr.AppendLine("  v_User")
        sqlStr.AppendLine("WHERE ")
        sqlStr.AppendLine("  isDisabled = 0")
        sqlStr.AppendLine("  AND LocationCode = @LocationCode")
        If IsConfidential Then
            sqlStr.AppendLine("  AND RoleCode = 'WRITE'")
        End If
        sqlStr.AppendLine("ORDER BY")
        sqlStr.AppendLine("  Name")

        Using sqlConn As New SqlClient.SqlConnection(DB_CONNECT_STRING)
            Using sqlCmd As SqlClient.SqlCommand = New SqlClient.SqlCommand(sqlStr.ToString, sqlConn)
                sqlCmd.Parameters.AddWithValue("LocationCode", LocationCode)
                sqlConn.Open()

                Dim sqlAdapter As New SqlClient.SqlDataAdapter
                sqlAdapter.SelectCommand = sqlCmd
                sqlAdapter.Fill(ds)
            End Using
        End Using

        Return ds

    End Function

    ' ユーザ選択プルダウンにユーザリストをセットする
    Private Sub SetAssinUser(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles RFQList.ItemDataBound, POList.ItemDataBound
        Dim st_UserType As String = CType(CType(e.Item, ListViewItem).BindingContainer, System.Web.UI.Control).ID

        If st_UserType = "RFQList" Then
            If IsConfidentialItem(DirectCast(e.Item.FindControl("ProductID"), HiddenField).Value) Then
                CType(e.Item.FindControl("QuoUser"), DropDownList).DataSource = ds_UserConfidential
            Else
                CType(e.Item.FindControl("QuoUser"), DropDownList).DataSource = ds_User
            End If
            CType(e.Item.FindControl("QuoUser"), DropDownList).DataBind()
        Else
            If IsConfidentialItem(DirectCast(e.Item.FindControl("ProductID"), HiddenField).Value) Then
                CType(e.Item.FindControl("SOUser"), DropDownList).DataSource = ds_UserConfidential
            Else
                CType(e.Item.FindControl("SOUser"), DropDownList).DataSource = ds_User
            End If
            CType(e.Item.FindControl("SOUser"), DropDownList).DataBind()
        End If
    End Sub

    Private Function CreateRHQHeaderSelectSQL(ByVal st_LocationCode As String) As String

        Dim sb_SQL As New Text.StringBuilder

        'SQL文字列の作成
        sb_SQL.Append("SELECT")
        sb_SQL.Append("  RH.RFQNumber, ")
        sb_SQL.Append("  ISNULL(RH.Priority, '') AS Priority, ")
        sb_SQL.Append("  CASE WHEN RH.Priority IS NULL THEN 1 ELSE 0  END AS PrioritySort,")
        sb_SQL.Append("  RH.StatusChangeDate, ")
        sb_SQL.Append("  RH.Status, ")
        sb_SQL.Append("  RH.ProductID, ")
        sb_SQL.Append("  RH.ProductNumber, ")
        sb_SQL.Append("  RH.ProductName, ")
        sb_SQL.Append("  RH.Purpose, ")
        sb_SQL.Append("  RH.EnqUserName, ")
        sb_SQL.Append("  RH.EnqLocationName, ")
        sb_SQL.Append("  RH.SupplierName, ")
        sb_SQL.Append("  RH.MakerName, ")
        sb_SQL.Append("  CONVERT(VARCHAR, RH.UpdateDate, 120) AS UpdateDate, ")
        sb_SQL.Append("  RH.isCONFIDENTIAL ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("  v_RFQHeader AS RH ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  RH.QuoLocationCode = '" & st_LocationCode & "' ")
        sb_SQL.Append("  AND RH.StatusCode = 'N' ")
        sb_SQL.Append("  AND RH.QuoUserID IS NULL ")
        '権限ロールに従い極秘品を除外する
        If Session(SESSION_ROLE_CODE).ToString = ROLE_WRITE_P OrElse Session(SESSION_ROLE_CODE).ToString = ROLE_READ_P Then
            sb_SQL.Append("  AND RH.isCONFIDENTIAL = 0 ")
        End If
        sb_SQL.Append("ORDER BY ")
        sb_SQL.Append("  PrioritySort, ")
        sb_SQL.Append("  Priority, ")
        sb_SQL.Append("  RH.RFQNumber ASC ")

        Return sb_SQL.ToString()

    End Function

    Private Function CreatePOSelectSQL(ByVal st_LocationCode As String) As String
        Dim sb_SQL As New Text.StringBuilder

        'SQL文字列の作成
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("  VP.PONumber, ")
        sb_SQL.Append("  ISNULL(VP.Priority, '') AS Priority, ")
        sb_SQL.Append("  CASE WHEN VP.Priority IS NULL THEN 1 ELSE 0  END AS PrioritySort,")
        sb_SQL.Append("  VP.PODate, ")
        sb_SQL.Append("  VP.POLocationCode, ")
        sb_SQL.Append("  VP.POLocationName, ")
        sb_SQL.Append("  VP.POUserID, ")
        sb_SQL.Append("  VP.POUserName, ")
        sb_SQL.Append("  VP.ProductID, ")
        sb_SQL.Append("  VP.ProductNumber, ")
        sb_SQL.Append("  VP.ProductName, ")
        sb_SQL.Append("  VP.SupplierCode, ")
        sb_SQL.Append("  VP.SupplierName, ")
        sb_SQL.Append("  VP.MakerCode, ")
        sb_SQL.Append("  VP.MakerName, ")
        sb_SQL.Append("  VP.ParPONumber, ")
        sb_SQL.Append("  VP.StatusCode, ")
        sb_SQL.Append("  VP.StatusChangeDate, ")
        sb_SQL.Append("  CONVERT(VARCHAR, VP.UpdateDate, 120) AS UpdateDate, ")
        sb_SQL.Append("  VP.isCONFIDENTIAL ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("  v_PO AS VP ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  VP.SOLocationCode = '" & st_LocationCode & "' ")
        sb_SQL.Append("  AND VP.StatusCode = 'PPI' ")
        sb_SQL.Append("  AND VP.SOUserID IS NULL ")
        '権限ロールに従い極秘品を除外する
        If Session(SESSION_ROLE_CODE).ToString = ROLE_WRITE_P OrElse Session(SESSION_ROLE_CODE).ToString = ROLE_READ_P Then
            sb_SQL.Append("  AND VP.isCONFIDENTIAL = 0 ")
        End If
        sb_SQL.Append("ORDER BY ")
        sb_SQL.Append("  PrioritySort, ")
        sb_SQL.Append("  Priority, ")
        sb_SQL.Append("  VP.PONumber ASC ")

        Return sb_SQL.ToString()
    End Function

    Private Function CreateRFQQuoUserUpdateSQL() As String
        Dim sb_SQL As New Text.StringBuilder

        'SQL文字列の作成
        sb_SQL.Append("UPDATE ")
        sb_SQL.Append("  RFQHeader ")
        sb_SQL.Append("SET ")
        sb_SQL.Append("  QuoUserID = @QuoUserID, ")
        sb_SQL.Append("  RFQStatusCode = 'A', ")
        sb_SQL.Append("  UpdateDate = GETDATE(), ")
        sb_SQL.Append("  UpdatedBy = @UpdatedBy ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  RFQNumber = @RFQNumber ")

        Return sb_SQL.ToString()
    End Function

    Private Function CreatePOSOUserUpdateSQL() As String
        Dim sb_SQL As New Text.StringBuilder

        'SQL文字列の作成
        sb_SQL.Append("UPDATE ")
        sb_SQL.Append("  PO ")
        sb_SQL.Append("SET ")
        sb_SQL.Append("  SOUserID = @SOUserID, ")
        sb_SQL.Append("  UpdateDate = GETDATE(), ")
        sb_SQL.Append("  UpdatedBy = @UpdatedBy ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  PONumber = @PONumber ")

        Return sb_SQL.ToString()
    End Function

End Class
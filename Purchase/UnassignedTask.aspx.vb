Partial Public Class UnassignedTask
    Inherits CommonPage

    Private Const ASSIGN_ACTION As String = "Assign"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim st_Login_Location As String = Session("LocationCode").ToString()

        Msg.Text = ""

        SrcRFQ.SelectCommand = _
              "SELECT VRH.RFQNumber, VRH.StatusChangeDate, VRH.Status, VRH.ProductNumber, VRH.ProductName, " _
            & "       VRH.Purpose, VRH.EnqUserName, VRH.EnqLocationName, VRH.SupplierName, VRH.MakerName, " _
            & "       CONVERT(VARCHAR, VRH.UpdateDate, 120) AS UpdateDate " _
            & "FROM v_RFQHeader AS VRH " _
            & "WHERE " _
            & "  VRH.QuoLocationCode = '" & st_Login_Location & "' " _
            & "  AND VRH.StatusCode = 'N' " _
            & "  AND VRH.QuoUserID IS NULL " _
            & "ORDER BY VRH.RFQNumber ASC "

        SrcPO.SelectCommand = _
              "SELECT VP.PONumber, VP.PODate, VP.POLocationCode, VP.POLocationName, VP.POUserID, VP.POUserName, " _
            & "       VP.ProductNumber, VP.ProductName, VP.SupplierCode, VP.SupplierName, " _
            & "       VP.MakerCode, VP.MakerName, VP.ParPONumber, VP.StatusCode, VP.StatusChangeDate, " _
            & "       CONVERT(VARCHAR, VP.UpdateDate, 120) AS UpdateDate " _
            & "FROM v_PO AS VP " _
            & "WHERE " _
            & "  VP.SOLocationCode = '" & st_Login_Location & "' " _
            & "  AND VP.SOUserID IS NULL " _
            & "ORDER BY VP.PONumber ASC "

        SrcUser.SelectCommand = "SELECT UserID, Name FROM v_User WHERE isDisabled = 0 AND LocationCode = '" & st_Login_Location & "' ORDER BY Name ASC "

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
        If Not Common.isLatestData("RFQHeader", "RFQNumber", st_RFQNumber, st_UpdateDate) Then
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
        SrcRFQ.UpdateCommand = _
              "UPDATE RFQHeader " _
            & "SET QuoUserID = @QuoUserID, RFQStatusCode = 'A', UpdateDate = GETDATE(), UpdatedBy = @UpdatedBy " _
            & "WHERE RFQNumber = @RFQNumber "
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
        If Not Common.isLatestData("PO", "PONumber", st_PONumber, st_UpdateDate) Then
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
        SrcPO.UpdateCommand = _
              "UPDATE PO " _
            & "SET SOUserID = @SOUserID, UpdateDate = GETDATE(), UpdatedBy = @UpdatedBy " _
            & "WHERE PONumber = @PONumber "
        SrcPO.Update()

    End Sub

    ' 更新日取得
    Private Sub SetRFQUpdateDate(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles RFQList.ItemDataBound
        Dim st_RFQNumber As String = CType(e.Item.FindControl("RFQNumber"), Label).Text
        CType(e.Item.FindControl("UpdateDate"), HiddenField).Value = Common.GetUpdateDate("RFQHeader", "RFQNumber", st_RFQNumber)
    End Sub

    ' 更新日取得
    Private Sub SetPOUpdateDate(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles POList.ItemDataBound
        Dim st_PONumber As String = CType(e.Item.FindControl("PONumber"), Label).Text
        CType(e.Item.FindControl("UpdateDate"), HiddenField).Value = Common.GetUpdateDate("PO", "PONumber", st_PONumber)
    End Sub

    Protected Sub SrcRFQ_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcRFQ.Selecting
        e.Command.CommandTimeout = 0
    End Sub

    Protected Sub SrcPO_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcPO.Selecting
        e.Command.CommandTimeout = 0
    End Sub

    Protected Sub SrcUser_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcUser.Selecting
        e.Command.CommandTimeout = 0
    End Sub
End Class
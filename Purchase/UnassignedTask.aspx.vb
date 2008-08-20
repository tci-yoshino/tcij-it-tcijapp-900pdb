Partial Public Class UnassignedTask
    Inherits CommonPage

    Private DBConnectString As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Const ERR_UPDATE As String = "既に他のユーザによって更新されました。"
    Const ASSIGN_ACTION As String = "Assign"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Msg.Text = ""

        SrcRFQ.SelectCommand = _
              "SELECT VRH.RFQNumber, VRH.StatusChangeDate, VRH.Status, VRH.ProductNumber, VRH.ProductName, " _
            & "       VRH.Purpose, VRH.EnqUserName, VRH.EnqLocationName, VRH.SupplierName, VRH.MakerName, " _
            & "       CONVERT(VARCHAR, VRH.UpdateDate, 120) AS UpdateDate " _
            & "FROM v_RFQHeader AS VRH " _
            & "WHERE " _
            & "  VRH.QuoLocationCode = '" & Session("LocationCode") & "' " _
            & "  AND VRH.QuoUserID IS NULL " _
            & "ORDER BY VRH.RFQNumber "

        SrcPO.SelectCommand = _
              "SELECT VP.PONumber, VP.PODate, VP.POLocationCode, VP.POLocationName, VP.POUserID, VP.POUserName, " _
            & "       VP.ProductNumber, VP.ProductName, VP.SupplierCode, VP.SupplierName, " _
            & "       VP.MakerCode, VP.MakerName, VP.ParPONumber, VP.Status, VP.StatusChangeDate, " _
            & "       CONVERT(VARCHAR, VP.UpdateDate, 120) AS UpdateDate " _
            & "FROM v_PO AS VP " _
            & "WHERE " _
            & "  VP.SOLocationCode = '" & Session("LocationCode") & "' " _
            & "  AND VP.SOUserID IS NULL " _
            & "ORDER BY VP.PONumber "


        SrcUser.SelectCommand = "SELECT UserID, Name FROM v_User WHERE isDisabled = 0 AND LocationCode = '" & Session("LocationCode") & "' "

    End Sub

    Protected Sub RFQAssign_Click(ByVal source As Object, ByVal e As ListViewCommandEventArgs) Handles RFQList.ItemCommand

        Dim st_action As String = "" ' Action 格納変数
        Dim st_RFQNumber As String = CType(e.Item.FindControl("RFQNumber"), Label).Text
        Dim st_UpdateDate As String = CType(e.Item.FindControl("UpdateDate"), HiddenField).Value
        Dim st_QuoUserID As String = CType(e.Item.FindControl("QuoUser"), DropDownList).SelectedValue

        ' Action 取得
        If Request.RequestType = "POST" Then
            st_action = IIf(Request.Form("Action") = Nothing, "", Request.Form("Action"))
        ElseIf Request.RequestType = "GET" Then
            st_action = IIf(Request.QueryString("Action") = Nothing, "", Request.QueryString("Action"))
        End If

        ' Action が "Assign"でない場合はエラー
        If st_action <> ASSIGN_ACTION Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            RFQList.DataBind()
            Exit Sub
        End If

        ' Update Chack
        If Not Common.isLatestData("RFQHeader", "RFQNumber", st_RFQNumber, st_UpdateDate) Then
            Msg.Text = ERR_UPDATE
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
        If Not Common.isLatestData("RFQHeader", "RFQNumber", st_PONumber, st_UpdateDate) Then
            Msg.Text = ERR_UPDATE
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

End Class
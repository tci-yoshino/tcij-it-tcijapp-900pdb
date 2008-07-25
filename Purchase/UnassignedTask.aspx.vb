Partial Public Class UnassignedTask
    Inherits CommonPage

    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ErrorMessages.Text = ""

        SrcRFQ.SelectCommand = _
              "SELECT VRH.RFQNumber, VRH.StatusChangeDate, VRH.Status, VRH.ProductNumber, VRH.ProductName, " _
            & "       VRH.Purpose, VRH.EnqUserName, VRH.EnqLocationName, VRH.SupplierName, VRH.MakerName, " _
            & "       CONVERT(VARCHAR, RH.UpdateDate, 120) AS UpdateDate " _
            & "FROM v_RFQHeader AS VRH, RFQHeader AS RH " _
            & "WHERE VRH.RFQNumber = RH.RFQNumber " _
            & "  AND VRH.QuoLocationCode = '" & Session("LocationCode") & "' " _
            & "  AND VRH.QuoUserID IS NULL " _
            & "ORDER BY VRH.RFQNumber "

        SrcPO.SelectCommand = _
              "SELECT VP.PONumber, VP.PODate, VP.POLocationCode, VP.POLocationName, VP.POUserID, VP.POUserName, " _
            & "       VP.ProductNumber, VP.ProductName, VP.SupplierCode, VP.SupplierName, " _
            & "       VP.MakerCode, VP.MakerName, VP.ParPONumber, VP.Status, VP.StatusChangeDate, " _
            & "       CONVERT(VARCHAR, P.UpdateDate, 120) AS UpdateDate " _
            & "FROM v_PO AS VP ,PO AS P " _
            & "WHERE VP.PONumber = P.PONumber " _
            & "  AND VP.SOLocationCode = '" & Session("LocationCode") & "' " _
            & "  AND VP.SOUserID IS NULL " _
            & "ORDER BY VP.PONumber "


        SrcQuo.SelectCommand = "SELECT UserID, Name FROM v_User WHERE LocationCode = '" & Session("LocationCode") & "' "
    End Sub

    Protected Sub Assign_Click(ByVal source As Object, ByVal e As ListViewCommandEventArgs) Handles RFQList.ItemCommand, POList.ItemCommand

        Dim st_action As String = "" ' Action 格納変数
        Dim i_updatelen As Integer ' Update が実行された行数を格納する変数

        ' Action取得
        If Request.RequestType = "POST" Then
            st_action = IIf(Request.Form("Action") = Nothing, "", Request.Form("Action"))
        ElseIf Request.RequestType = "GET" Then
            st_action = IIf(Request.QueryString("Action") = Nothing, "", Request.QueryString("Action"))
        End If

        ' Action が "Assign" なら実行
        If st_action = "Assign" Then
            ' 親コントローラの ID によって処理を分ける
            If e.Item.Parent.NamingContainer.ID = "RFQList" Then

                ' SQLパラメータ設定
                SrcRFQ.UpdateParameters.Add("RFQNumber", CType(e.Item.FindControl("RFQNumber"), Label).Text)
                SrcRFQ.UpdateParameters.Add("QuoUserID", CType(e.Item.FindControl("QuoUser"), DropDownList).SelectedValue)
                SrcRFQ.UpdateParameters.Add("UpdateBy", Session("UserID"))
                SrcRFQ.UpdateParameters.Add("UpdateDate", CType(e.Item.FindControl("UpdateDate"), HiddenField).Value)

                ' Update 文作成
                SrcRFQ.UpdateCommand = _
                      "UPDATE RFQHeader " _
                    & "SET QuoUserID = @QuoUserID, RFQStatusCode = 'A', UpdateDate = GETDATE(), UpdatedBy = @UpdateBy " _
                    & "WHERE RFQNumber = @RFQNumber " _
                    & "  AND CONVERT(VARCHAR, UpdateDate, 120) = @UpdateDate "

                ' Update 実行、実行行数取得
                i_updatelen = SrcRFQ.Update()
                ' 再バインド
                RFQList.DataBind()

            ElseIf e.Item.Parent.NamingContainer.ID = "POList" Then

                ' SQLパラメータ設定
                SrcPO.UpdateParameters.Add("PONumber", CType(e.Item.FindControl("PONumber"), Label).Text)
                SrcPO.UpdateParameters.Add("SOUserID", CType(e.Item.FindControl("SOUser"), DropDownList).SelectedValue)
                SrcPO.UpdateParameters.Add("UpdateBy", Session("UserID"))
                SrcPO.UpdateParameters.Add("UpdateDate", CType(e.Item.FindControl("UpdateDate"), HiddenField).Value)

                ' Update 文作成
                SrcPO.UpdateCommand = _
                      "UPDATE PO " _
                    & "SET SOUserID = @SOUserID, UpdateDate = GETDATE(), UpdatedBy = @UpdateBy " _
                    & "WHERE PONumber = @PONumber " _
                    & "  AND CONVERT(VARCHAR, UpdateDate, 120) = @UpdateDate "

                ' Update 実行、実行行数取得
                i_updatelen = SrcPO.Update()
                ' 再バインド
                POList.DataBind()
            End If

            ' Update された行数が 0 の場合はエラーメッセージ表示。
            If i_updatelen = 0 Then
                ErrorMessages.Text = "既に他のユーザによって更新されました。"
            End If
        End If



    End Sub

End Class
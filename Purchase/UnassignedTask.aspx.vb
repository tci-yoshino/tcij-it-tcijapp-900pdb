Partial Public Class UnassignedTask
    Inherits CommonPage

    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SrcRFQ.SelectCommand = _
  "SELECT VRH.RFQNumber, VRH.StatusChangeDate, VRH.Status, VRH.ProductNumber, VRH.ProductName, " _
& "       VRH.Purpose, VRH.EnqUserName, VRH.EnqLocationName, VRH.SupplierName, VRH.MakerName, " _
& "       RH.UpdateDate " _
& "FROM v_RFQHeader AS VRH, RFQHeader AS RH " _
& "WHERE VRH.RFQNumber = RH.RFQNumber " _
& "  AND VRH.QuoLocationCode = '" & Session("LocationCode") & "' " _
& "  AND VRH.QuoUserID IS NULL " _
& "ORDER BY VRH.StatusSortOrder, VRH.StatusChangeDate "

        SrcPO.SelectCommand = _
  "SELECT VP.PONumber, VP.PODate, VP.POLocationCode, VP.POLocationName, VP.POUserID, VP.POUserName, " _
& "       VP.ProductNumber, VP.ProductName, VP.SupplierCode, VP.SupplierName, " _
& "       VP.MakerCode, VP.MakerName, VP.ParPONumber, VP.Status, VP.StatusChangeDate, " _
& "       P.UpdateDate " _
& "FROM v_PO AS VP ,PO AS P " _
& "WHERE VP.PONumber = P.PONumber " _
& "  AND VP.SOLocationCode = '" & Session("LocationCode") & "' " _
& "  AND VP.SOUserID IS NULL "

        SrcQuo.SelectCommand = "SELECT UserID, Name FROM v_User WHERE LocationCode = '" & Session("LocationCode") & "' "
    End Sub

    Protected Sub Assign_Click(ByVal source As Object, ByVal e As ListViewCommandEventArgs) Handles RFQList.ItemCommand
        Dim lv As ListView = CType(source, ListView)
        Dim st_QuoUserID As String
        Dim st_Number As String
        st_QuoUserID = CType(e.Item.FindControl("QuoUser"), DropDownList).SelectedValue
        st_Number = lv.DataKeys(0).Value

    End Sub

End Class
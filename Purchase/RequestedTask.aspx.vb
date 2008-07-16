Public Partial Class RequestedTask
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SrcRFQ.SelectCommand = "SELECT RH.RFQNumber, RH.StatusChangeDate, RH.Status, RH.ProductNumber, RH.ProductName, RH.Purpose, RH.QuoUserName, RH.QuoLocationName, RH.SupplierName, RH.MakerName, RR.RFQCorres " _
& "FROM v_RFQHeader AS RH LEFT OUTER JOIN " _
& "     v_RFQReminder AS RR ON RH.RFQNumber = RR.RFQNumber AND RR.RcptUserID = '" & Session("UserID") & "' " _
& "WHERE EnqUserID = '" & Session("UserID") & "' " _
& "ORDER BY StatusSortOrder, StatusChangeDate "

    End Sub

End Class
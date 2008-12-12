Public Partial Class RequestedTask
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SrcRFQ.SelectCommand = _
              "SELECT RH.RFQNumber, RH.StatusChangeDate, RH.Status, RH.ProductNumber, RH.ProductName, RH.Purpose, " _
            & "       RH.QuoUserName, RH.QuoLocationName, RH.SupplierName, RH.MakerName, RR.RFQCorres AS RFQCorrespondence " _
            & "FROM v_RFQHeader AS RH LEFT OUTER JOIN " _
            & "     v_RFQReminder AS RR ON RH.RFQNumber = RR.RFQNumber AND RR.RcptUserID = '" & Session("UserID") & "' " _
            & "WHERE EnqUserID = '" & Session("UserID") & "' " _
            & "  AND NOT (RH.StatusCode = 'C' AND RR.RFQHistoryNumber IS NULL) " _
            & "ORDER BY StatusSortOrder, StatusChangeDate "

    End Sub

    Protected Sub SrcRFQ_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcRFQ.Selecting
        e.Command.CommandTimeout = 0
    End Sub

End Class
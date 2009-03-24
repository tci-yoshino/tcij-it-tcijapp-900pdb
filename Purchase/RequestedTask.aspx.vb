Public Partial Class RequestedTask
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SrcRFQ.SelectCommand = CreateRHQHeaderSelectSQL()
    End Sub

    Protected Sub SrcRFQ_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcRFQ.Selecting
        e.Command.CommandTimeout = 0
    End Sub

    Private Function CreateRHQHeaderSelectSQL() As String

        Dim sb_SQL As New Text.StringBuilder

        'SQL文字列の作成
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("  RH.RFQNumber, ")
        sb_SQL.Append("  RH.StatusChangeDate, ")
        sb_SQL.Append("  RH.Status, ")
        sb_SQL.Append("  RH.ProductNumber, ")
        sb_SQL.Append("  RH.ProductName, ")
        sb_SQL.Append("  RH.Purpose, ")
        sb_SQL.Append("  RH.QuoUserName, ")
        sb_SQL.Append("  RH.QuoLocationName, ")
        sb_SQL.Append("  RH.SupplierName, ")
        sb_SQL.Append("  RH.MakerName, ")
        sb_SQL.Append("  RR.RFQCorres AS RFQCorrespondence ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("  v_RFQHeader AS RH LEFT ")
        sb_SQL.Append("    OUTER JOIN v_RFQReminder AS RR ON RH.RFQNumber = RR.RFQNumber AND RR.RcptUserID = '" & Session("UserID") & "' ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  EnqUserID = '" & Session("UserID") & "' ")
        sb_SQL.Append("  AND NOT (RH.StatusCode = 'C' AND RR.RFQHistoryNumber IS NULL) ")
        sb_SQL.Append("ORDER BY ")
        sb_SQL.Append("  StatusSortOrder, StatusChangeDate ")

        Return sb_SQL.ToString()

    End Function

End Class
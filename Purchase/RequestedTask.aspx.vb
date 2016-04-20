Imports Purchase.Common

Partial Public Class RequestedTask
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not IsPostBack Then
            SetPriorityDropDownList(Priority, PRIORITY_FOR_SEARCH)
            Priority.SelectedValue = PRIORITY_ALL

            SrcRFQ.SelectCommand = CreateRHQHeaderSelectSQL()
        End If
    End Sub

    Protected Sub Switch_Click()
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
        sb_SQL.Append("  CASE WHEN RH.Priority IS NULL THEN 1 ELSE 0  END AS PrioritySort, ")
        sb_SQL.Append("  ISNULL(RH.Priority, '') AS Priority, ")
        sb_SQL.Append("  RH.StatusChangeDate, ")
        sb_SQL.Append("  RH.Status, ")
        sb_SQL.Append("  RH.ProductNumber, ")
        sb_SQL.Append("  RH.ProductName, ")
        sb_SQL.Append("  RH.Purpose, ")
        sb_SQL.Append("  RH.QuoUserName, ")
        sb_SQL.Append("  RH.QuoLocationName, ")
        sb_SQL.Append("  RH.SupplierName, ")
        sb_SQL.Append("  RH.MakerName, ")
        sb_SQL.Append("  RR.RFQCorres AS RFQCorrespondence, ")
        sb_SQL.Append("  RH.isCONFIDENTIAL ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("  v_RFQHeader AS RH LEFT ")
        sb_SQL.Append("    OUTER JOIN v_RFQReminder AS RR ON RH.RFQNumber = RR.RFQNumber AND RR.RcptUserID = '" & Session("UserID") & "' ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  EnqUserID = '" & Session("UserID") & "' ")
        sb_SQL.Append("  AND NOT (RH.StatusCode = 'C' AND RR.RFQHistoryNumber IS NULL) ")
        Select Case Priority.SelectedValue
            Case PRIORITY_A
                sb_SQL.Append("  AND RH.Priority = 'A' ")
            Case PRIORITY_B
                sb_SQL.Append("  AND RH.Priority = 'B' ")
            Case PRIORITY_AB
                sb_SQL.Append("  AND RH.Priority IN('A','B') ")
        End Select
        '権限ロールに従い極秘品を除外する
        If Session(SESSION_ROLE_CODE).ToString = ROLE_WRITE_P OrElse Session(SESSION_ROLE_CODE).ToString = ROLE_READ_P Then
            sb_SQL.Append("  AND RH.isCONFIDENTIAL = 0 ")
        End If
        sb_SQL.Append("ORDER BY ")
        sb_SQL.Append("  PrioritySort, Priority, StatusSortOrder, StatusChangeDate ")

        Return sb_SQL.ToString()

    End Function

End Class
Public Partial Class POIssue
    Inherits CommonPage

    Public st_rfqLineNumber As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Not IsPostBack Then
            st_rfqLineNumber = IIf(Request.RequestType = "POST", Request.Form("RFQLineNumber"), Request.QueryString("RFQLineNumber"))

            If String.IsNullOrEmpty(st_rfqLineNumber) Then
                Msg.Text = "Invalid parameter supplied."
                Exit Sub
            End If

            RFQLineNumber.Value = st_rfqLineNumber
        End If

    End Sub

End Class
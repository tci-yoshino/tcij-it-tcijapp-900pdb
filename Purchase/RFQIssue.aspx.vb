Public Partial Class RFQIssue
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim a As String = ""
        If IsPostBack = True Then

            If Request.QueryString("Action") = "Issue" Then

            Else

            End If
        Else

        End If
    End Sub

    Protected Sub EnqLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles EnqLocation.SelectedIndexChanged
        'ドロップダウンリストの項目を入れ替える。

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Issue.PostBackUrl = "~/RFQIssue.aspx?Action=Issue"
    End Sub
End Class
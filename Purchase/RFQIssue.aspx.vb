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
        If IsPostBack = False Then
            Issue.PostBackUrl = "~/RFQIssue.aspx?Action=Issue"

        End If
        If Request.QueryString("ProductID").Length > 0 Then

        End If
        'パラメータ ProductID を受け取った場合
        'テキストボックス ProductNumber，ProductName を ReadOnly="true" CssClass="readonly" ProductNumber 横の虫眼鏡は非表示にする。 
        'パラメータ SupplierCode が渡されたとき
        'テキストボックス SupplierCode，R3SupplierCode，SupplierName，SupplierCountry を ReadOnly="true" CssClass="readonly" SupplierCode 横の虫眼鏡は非表示にする。 
        'それぞれのパラメータが渡されない場合は ReadOnly CssClass の指定は変更しない。 

    End Sub
End Class
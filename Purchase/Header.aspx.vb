Public Partial Class Header
    Inherits CommonPage
    ' 接続文字列
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

End Class
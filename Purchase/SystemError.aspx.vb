Public Partial Class SystemError
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ex As Exception = HttpContext.Current.Server.GetLastError

        If TypeOf ex Is HttpUnhandledException AndAlso _
            ex.InnerException IsNot Nothing Then
            ex = ex.InnerException
        End If

        If ex IsNot Nothing Then
            Message.Text = ex.Message
            StackTrace.Text = ex.StackTrace
        End If
    End Sub

End Class
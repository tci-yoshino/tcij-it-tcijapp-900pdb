Option Strict On

Partial Public Class SystemError
    Inherits CommonPage

    Protected b_IsDebug As Boolean

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim ex As Exception = HttpContext.Current.Server.GetLastError
        Dim appSetting As New System.Configuration.AppSettingsReader()

        Try
            b_IsDebug = CBool(appSetting.GetValue("Debug", GetType(String)))
        Catch
            ' 例外を捕捉せず、デバッグモードを False に設定する
            b_IsDebug = False
        End Try

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
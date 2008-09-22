Option Strict On

Imports System.Web.SessionState
Imports System.IO
Imports System.Net.Mail

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' アプリケーションの起動時に呼び出されます。
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' セッションの開始時に呼び出されます。
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' 各要求の開始時に呼び出されます。
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' 使用の認証時に呼び出されます。
    End Sub

    ''' <summary>
    ''' エラーイベントを処理する。
    ''' </summary>
    ''' <param name="sender">オブジェクトデータ (既定パラメータ)</param>
    ''' <param name="e">イベントデータ (既定パラメータ)</param>
    ''' <remarks>
    ''' アプリケーションで発生した例外を、Web.config に定義した
    ''' 担当者のメールアドレス宛てに通知し、処理を SystemError.aspx に委譲する。
    ''' </remarks>
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' エラーの発生時に呼び出されます。
        Dim context As HttpContext = DirectCast(sender, HttpApplication).Context
        Dim ex As Exception = HttpContext.Current.Server.GetLastError
        Dim appSetting As New System.Configuration.AppSettingsReader()
        Dim b_IsDebug As Boolean

        If TypeOf ex Is HttpUnhandledException AndAlso _
            ex.InnerException IsNot Nothing Then
            ex = ex.InnerException
        End If

        Try
            b_IsDebug = CBool(appSetting.GetValue("Debug", GetType(String)))
        Catch
            ' 例外を捕捉せず、デバッグモードを False に設定する
            b_IsDebug = False
        End Try

        If ex IsNot Nothing And Not b_IsDebug Then
            Try
                Dim mail As New MailMessage

                mail.To.Add(New MailAddress(appSetting.GetValue("ErrorMailTo", GetType(String)).ToString))
                mail.Subject = "[Purchase] Internal System Error"
                mail.Body = String.Format("An unhandled exception occurred: {1}{0}{0}User: {2} ({3}){0}{0}Message: {4}{0}{0}Stack Trace:{0}{5}{0}{0}User Agent:{0}{6}{0}{0}", _
                    System.Environment.NewLine, context.Request.RawUrl, Session("UserName"), Session("UserID"), ex.Message, ex.StackTrace, context.Request.UserAgent)
                mail.IsBodyHtml = False

                Dim smtp As New SmtpClient
                smtp.Send(mail)
            Catch
                ' メール送信に失敗したときの例外は捕捉しない
            End Try
        End If

        If TypeOf ex Is Web.HttpRequestValidationException Then
            Response.Redirect("./FormError.html", True)
        Else
            Server.Transfer("./SystemError.aspx")
        End If

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' セッションの終了時に呼び出されます。
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' アプリケーションの終了時に呼び出されます。
    End Sub

End Class
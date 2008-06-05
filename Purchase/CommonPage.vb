Imports System
Imports System.Collections
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient

Public Class CommonPage
    Inherits Page

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        'Call the base class's OnLoad method
        MyBase.OnLoad(e)

        Dim st_action As String = ""
        Dim st_scriptName As String = ""
        Dim st_accountName As String = ""
        Dim st_buf() As String
        Dim settings As ConnectionStringSettings
        Dim dbConnection As New SqlConnection
        Dim dbCommand As SqlClient.SqlCommand
        Dim dbReader As SqlDataReader

        'User authorization process
        If Request.RequestType = "POST" Then
            st_action = IIf(Request.Form("Action") = Nothing, "", Request.Form("Action"))
        ElseIf Request.RequestType = "GET" Then
            st_action = IIf(Request.QueryString("Action") = Nothing, "", Request.QueryString("Action"))
        End If

        st_buf = Split(Request.FilePath, "/")
        st_scriptName = st_buf(st_buf.Length - 1)

        settings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
        dbConnection.ConnectionString = settings.ConnectionString

        dbConnection.Open()
        dbCommand = dbConnection.CreateCommand

        If Session("UserID") = Nothing Then
            st_buf = Split(Request.ServerVariables("LOGON_USER"))
            st_accountName = st_buf(st_buf.Length - 1)

            dbCommand.CommandText = "SELECT UserID, LocationCode, RoleCode, PrivilegeLevel FROM v_User WHERE AccountName = @AccountName"
            dbCommand.Parameters.Add("AccountName", SqlDbType.NVarChar).Value = st_accountName

            dbReader = dbCommand.ExecuteReader()
            If dbReader.Read = False Then
                'Authorization failed
                Response.Redirect("AuthError.html")
            End If

            Session("UserID") = dbReader("UserID").ToString
            Session("LocationCode") = dbReader("LocationCode").ToString
            Session("Purchase.RoleCode") = dbReader("RoleCode").ToString
            Session("Purchase.PrivilegeLevel") = dbReader("PrivilegeLevel").ToString

            dbCommand.Dispose()
            dbReader.Close()
        End If

        dbCommand.CommandText = "SELECT 1 FROM Privilege AS P, Role_Privilege AS RP WHERE RP.RoleCode = @RoleCode AND RP.PrivilegeCode = P.PrivilegeCode AND P.ScriptName = @ScriptName AND ISNULL(P.Action, '') = @Action"
        dbCommand.Parameters.Add("RoleCode", SqlDbType.VarChar).Value = IIf(Session("Purchase.RoleCode") = Nothing, "", Session("Purchase.RoleCode"))
        dbCommand.Parameters.Add("ScriptName", SqlDbType.VarChar).Value = st_scriptName
        dbCommand.Parameters.Add("Action", SqlDbType.VarChar).Value = st_action

        dbReader = dbCommand.ExecuteReader()
        If dbReader.Read = False Then
            'Authorization failed
            Response.Redirect("AuthError.html")
        End If

        dbCommand.Dispose()
        dbReader.Close()
        dbConnection.Close()
    End Sub
End Class

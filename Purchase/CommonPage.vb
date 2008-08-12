Imports System
Imports System.Collections
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data.SqlClient

Public Class CommonPage
    Inherits Page

    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        Dim st_Action As String = ""
        Dim st_ScriptName As String = ""
        Dim st_AccountName As String = ""
        Dim st_Buf() As String
        Dim setting As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
        Dim sqlConn As SqlConnection = New SqlConnection(setting.ConnectionString)
        Dim sqlAdapter As SqlDataAdapter
        Dim sqlCmd As SqlCommand
        Dim ds As DataSet = New DataSet

        ' User authorization process
        If Request.RequestType = "POST" Then
            st_Action = IIf(Request.Form("Action") = Nothing, "", Request.Form("Action"))
        ElseIf Request.RequestType = "GET" Then
            st_Action = IIf(Request.QueryString("Action") = Nothing, "", Request.QueryString("Action"))
        End If

        st_Buf = Split(Request.FilePath, "/")
        st_ScriptName = st_Buf(st_Buf.Length - 1)

        If Session("UserID") = Nothing Then
            st_Buf = Split(Request.ServerVariables("LOGON_USER"), "\")
            st_AccountName = st_Buf(st_Buf.Length - 1)

            sqlAdapter = New SqlDataAdapter
            sqlCmd = New SqlCommand( _
"SELECT " & _
"  PU.UserID, " & _
"  U.AD_GivenName + ' ' + U.AD_Surname AS UserName, " & _
"  U.LocationCode, " & _
"  L.Name AS LocationName, " & _
"  PU.RoleCode, " & _
"  PU.PrivilegeLevel, " & _
"  PU.isAdmin " & _
"FROM " & _
"  PurchasingUser AS PU, " & _
"  s_User AS U, " & _
"  s_Location AS L " & _
"WHERE " & _
"  PU.isDisabled = 0" & _
"  AND PU.UserID = U.UserID " & _
"  AND U.LocationCode = L.LocationCode " & _
"  AND U.AD_AccountName = @AccountName", sqlConn)
            sqlAdapter.SelectCommand = sqlCmd
            sqlCmd.Parameters.Add("AccountName", SqlDbType.NVarChar).Value = st_AccountName

            sqlAdapter.Fill(ds, "User")
            If ds.Tables("User").Rows.Count = 0 Then
                ' Authorization failed
                Response.Redirect("AuthError.html")
            End If

            Session("UserID") = ds.Tables("User").Rows(0)("UserID").ToString
            Session("UserName") = ds.Tables("User").Rows(0)("UserName").ToString
            Session("LocationCode") = ds.Tables("User").Rows(0)("LocationCode").ToString
            Session("LocationName") = ds.Tables("User").Rows(0)("LocationName").ToString
            Session("Purchase.RoleCode") = ds.Tables("User").Rows(0)("RoleCode").ToString
            Session("Purchase.PrivilegeLevel") = ds.Tables("User").Rows(0)("PrivilegeLevel").ToString
            Session("Purchase.isAdmin") = IIf(ds.Tables("User").Rows(0)("isAdmin").ToString = "True", True, False)
        End If

        If Session("Purchase.isAdmin") Then
            ' Nothing to do
        Else
            sqlAdapter = New SqlDataAdapter
            sqlCmd = New SqlCommand( _
"SELECT " & _
"  1 " & _
"FROM " & _
"  Privilege AS P, " & _
"  Role_Privilege AS RP " & _
"WHERE " & _
"  RP.RoleCode = @RoleCode " & _
"  AND RP.PrivilegeCode = P.PrivilegeCode " & _
"  AND P.ScriptName = @ScriptName " & _
"  AND ISNULL(P.Action, '') = @Action", sqlConn)
            sqlAdapter.SelectCommand = sqlCmd
            sqlCmd.Parameters.Add("RoleCode", SqlDbType.VarChar).Value = IIf(Session("Purchase.RoleCode") = Nothing, "", Session("Purchase.RoleCode"))
            sqlCmd.Parameters.Add("ScriptName", SqlDbType.VarChar).Value = st_ScriptName
            sqlCmd.Parameters.Add("Action", SqlDbType.VarChar).Value = st_Action

            sqlAdapter.Fill(ds, "Priv")
            If ds.Tables("Priv").Rows.Count = 0 Then
                ' Authorization failed
                Response.Redirect("AuthError.html")
            End If
        End If

        ' Call the base class's OnLoad method
        MyBase.OnLoad(e)
    End Sub
End Class

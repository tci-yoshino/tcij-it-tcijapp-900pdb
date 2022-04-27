Option Strict On

Imports System.Data.SqlClient
Imports Purchase.Common

''' <summary>
''' CommonPage クラス
''' </summary>
''' <remarks>各 Page クラスはこのクラスを継承しなければならない。</remarks>
Public Class CommonPage
    Inherits Page

    ''' <summary>
    ''' Page_Load イベントを処理する。
    ''' </summary>
    ''' <param name="e">イベントデータ (既定パラメータ)</param>
    ''' <remarks>
    ''' セッション UserID が格納されていない場合は、再認定を行う。
    ''' さらに、リクエストされた URL からスクリプト名を取得し、パラメータ Action の値と組み合わせて
    ''' 権限チェックを行う。管理者においては、このチェックをスルーする。
    ''' </remarks>
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        Dim st_Action As String = String.Empty
        Dim st_ScriptName As String = String.Empty
        Dim st_AccountName As String = String.Empty
        Dim st_Buf() As String

        Dim sqlConn As SqlConnection = New SqlConnection(DB_CONNECT_STRING)
        Dim sqlAdapter As SqlDataAdapter
        Dim sqlCmd As SqlCommand
        Dim ds As DataSet = New DataSet

        ' HTTP Request を処理します
        ' "OPTIONS" は IE で添付ファイルを直接開こうとした際に要求されます
        If Request.RequestType = "POST" Then
            If Request.Form("Action") = Nothing Then
                st_Action = IIf(Request.QueryString("Action") = Nothing, String.Empty, Request.QueryString("Action")).ToString
            Else
                st_Action = Request.Form("Action").ToString
            End If
        ElseIf (Request.RequestType = "GET" OrElse Request.RequestType = "HEAD") Then
            If Request.Form("Action") = Nothing Then
                st_Action = IIf(Request.QueryString("Action") = Nothing, String.Empty, Request.QueryString("Action")).ToString
            Else
                st_Action = Request.Form("Action").ToString
            End If
        ElseIf Request.RequestType = "OPTIONS" Then
            Exit Sub
        Else
            Throw New Exception("CommonPage.OnLoad: Bad Request Type.")
        End If

        st_ScriptName = System.IO.Path.GetFileName(Regex.Replace(Request.Url.ToString, "\.[aA][sS][pP][xX].*", ""))

        If Session("UserID") Is Nothing Then
            st_Buf = Split(Request.ServerVariables("LOGON_USER"), "\")
            st_AccountName = st_Buf(st_Buf.Length - 1)

            sqlAdapter = New SqlDataAdapter
            sqlCmd = New SqlCommand(CreateSql_SelectUser(), sqlConn)

            sqlAdapter.SelectCommand = sqlCmd
            sqlCmd.Parameters.Add("AccountName", SqlDbType.NVarChar).Value = st_AccountName

            sqlAdapter.Fill(ds, "User")
            If ds.Tables("User").Rows.Count = 0 Then
                Response.Redirect("AuthError.html")
            End If

            Session("UserID") = ds.Tables("User").Rows(0)("UserID").ToString
            Session("UserName") = ds.Tables("User").Rows(0)("UserName").ToString
            Session("LocationCode") = ds.Tables("User").Rows(0)("LocationCode").ToString
            Session("LocationName") = ds.Tables("User").Rows(0)("LocationName").ToString
            Session("Purchase.RoleCode") = ds.Tables("User").Rows(0)("RoleCode").ToString
            Session("Purchase.PrivilegeLevel") = ds.Tables("User").Rows(0)("PrivilegeLevel").ToString
            Session("Purchase.isAdmin") = IIf(ds.Tables("User").Rows(0)("isAdmin").ToString = "True", True, False)
            Session("Purchase.MMSTAInvalidationEditable") = ds.Tables("User").Rows(0)("MMSTAInvalidationEditable").ToString
        End If

        If CBool(Session("Purchase.isAdmin")) Then
            ' Nothing to do
        Else
            sqlAdapter = New SqlDataAdapter
            sqlCmd = New SqlCommand(CreateSql_SelectPrivilege(), sqlConn)

            sqlAdapter.SelectCommand = sqlCmd
            sqlCmd.Parameters.Add("RoleCode", SqlDbType.VarChar).Value = IIf(Session("Purchase.RoleCode") Is Nothing, String.Empty, Session("Purchase.RoleCode"))
            sqlCmd.Parameters.Add("ScriptName", SqlDbType.VarChar).Value = st_ScriptName
            sqlCmd.Parameters.Add("Action", SqlDbType.VarChar).Value = st_Action

            sqlAdapter.Fill(ds, "Privilege")
            If ds.Tables("Privilege").Rows.Count = 0 Then
                Response.Redirect("AuthError.html")
            End If
        End If

        ' 基底クラスの OnLoad を呼び出します
        MyBase.OnLoad(e)
    End Sub

    Private Function CreateSql_SelectUser() As String
        Dim sb_Sql As StringBuilder = New StringBuilder

        sb_Sql.Append("SELECT ")
        sb_Sql.Append("  PU.UserID, ")
        sb_Sql.Append("  U.AD_DisplayName AS UserName, ")
        sb_Sql.Append("  U.LocationCode, ")
        sb_Sql.Append("  L.Name AS LocationName, ")
        sb_Sql.Append("  PU.RoleCode, ")
        sb_Sql.Append("  PU.PrivilegeLevel, ")
        sb_Sql.Append("  PU.isAdmin, ")
        sb_Sql.Append("  CASE PU.MMSTAInvalidationEditable WHEN 1 THEN '1'  ")
        sb_Sql.Append("  ELSE '0' END AS MMSTAInvalidationEditable ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append("  PurchasingUser AS PU, ")
        sb_Sql.Append("  s_User AS U, ")
        sb_Sql.Append("  s_Location AS L ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append("  PU.isDisabled = 0 ")
        sb_Sql.Append("  AND PU.UserID = U.UserID ")
        sb_Sql.Append("  AND U.LocationCode = L.LocationCode ")
        sb_Sql.Append("  AND U.AD_AccountName = @AccountName ")

        Return sb_Sql.ToString

    End Function

    Private Function CreateSql_SelectPrivilege() As String
        Dim sb_Sql As StringBuilder = New StringBuilder

        sb_Sql.Append("SELECT ")
        sb_Sql.Append("  1 ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append("  Privilege AS P, ")
        sb_Sql.Append("  Role_Privilege AS RP ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append("  RP.RoleCode = @RoleCode ")
        sb_Sql.Append("  AND RP.PrivilegeCode = P.PrivilegeCode ")
        sb_Sql.Append("  AND P.ScriptName = @ScriptName ")
        sb_Sql.Append("  AND ISNULL(P.Action, '') = @Action ")

        Return sb_Sql.ToString

    End Function

End Class

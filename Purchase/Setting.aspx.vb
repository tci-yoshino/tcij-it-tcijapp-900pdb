Imports System.Data.SqlClient
Imports System.Web
'Imports System.Web.DynamicData
Imports Purchase.Common

Public Class Setting
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        CountryListli.Visible = False
        SupplierSearchli.Visible = False
        ProductSearchli.Visible = False
        PurchaseGroupli.Visible = False
        ReminderListli.Visible = False
        ProductInfoRegulationLi1.Visible = False

        Dim c As String
        Dim DataTable As System.Data.DataTable = GetDataTable("SELECT ScriptName from Privilege AS P,Role_Privilege AS RP 
WHERE RP.RoleCode = '" & Session("Purchase.RoleCode") & "'  AND RP.PrivilegeCode = P.PrivilegeCode And Action is null and 
ScriptName in('CountryList','SupplierSearch','ProductSearch','PurchaseGroup','ReminderList','UserList','ProductInfoRegulation')")
        If DataTable IsNot Nothing And DataTable.Rows.Count > 0 Then
            For i = 0 To DataTable.Rows.Count - 1
                c = DataTable.Rows(i).Item("ScriptName")
                If c = "CountryList" Then
                    CountryListli.Visible = True
                ElseIf c = "SupplierSearch" Then
                    SupplierSearchli.Visible = True
                ElseIf c = "ProductSearch" Then
                    ProductSearchli.Visible = True
                ElseIf c = "PurchaseGroup" Then
                    PurchaseGroupli.Visible = True
                ElseIf c = "ReminderList" Then
                    ReminderListli.Visible = True
                ElseIf c = "ProductInfoRegulation" Then
                    ProductInfoRegulationLi1.Visible = True
                End If
            Next i
        End If
    End Sub
End Class
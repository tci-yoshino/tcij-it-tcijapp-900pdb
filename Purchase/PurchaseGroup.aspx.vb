Option Explicit On
Option Infer Off
Option Strict On

Imports System.Data.SqlClient
Imports Purchase.Common

Partial Public Class PurchaseGroup
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'Dim purchaseGroupDisp As TCIDataAccess.Join.PurchaseGroupDispList = New TCIDataAccess.Join.PurchaseGroupDispList

        Dim locationCode As String = Session("LocationCode").ToString

        If IsPostBack = False Then
            'purchaseGroupDisp.Load(locationCode)
            'UserList.DataSource = purchaseGroupDisp
            'UserList.DataBind()

            Dim purchasingUserList As New TCIDataAccess.Join.PurchasingUserDispList
            purchasingUserList.LoadAllUsers(locationCode)
            UserList.DataSource = purchasingUserList
            UserList.DataBind()

        End If

    End Sub
    Public Function GetStorageLocations(ByVal userid As String) As String
        Dim ret As String = ""
        Dim SLocationByPUser As DataTable = GetDataTable("select * from StorageByPurchasingUser where UserID=" + userid)
        For i As Integer = 0 To SLocationByPUser.Rows.Count - 1
            ret += SLocationByPUser.Rows(i)("Storage").ToString + ", "
        Next
        If ret.Length > 0 Then
            ret = ret.Substring(0, ret.Length - 2)
        End If
        Return ret
    End Function

    Protected Sub UserList_ItemDataBound(sender As Object, e As ListViewItemEventArgs) Handles UserList.ItemDataBound

        If e.Item.ItemType <> ListViewItemType.DataItem Then
            Exit Sub
        End If

        Dim purchasingUser As TCIDataAccess.Join.PurchasingUserDisp = DirectCast(DirectCast(e.Item, ListViewDataItem).DataItem, TCIDataAccess.Join.PurchasingUserDisp)

        If TypeOf e.Item.FindControl("EditLink") Is HyperLink Then
            Dim editLink As HyperLink = DirectCast(e.Item.FindControl("EditLink"), HyperLink)
            editLink.NavigateUrl = String.Format("~/PurchaseGroupSetting.aspx?UserID={0}", purchasingUser.UserID)
        End If

    End Sub

End Class
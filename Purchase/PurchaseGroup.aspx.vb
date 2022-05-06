Option Explicit On
Option Infer Off
Option Strict On

Imports System.Data.SqlClient
Imports Purchase.Common

Partial Public Class PurchaseGroup
    Inherits CommonPage

    Private Structure ExcelLineType
        ''' <summary>Excel名</summary>
        Public ExlName As String
        ''' <summary>Header情報</summary>
        Public HeaderLine As String
        ''' <summary>Worksheet開始タグ</summary>
        Public StartWorksheetLine As String
        ''' <summary>Worksheet終了タグ</summary>
        Public EndWorksheetLine As String
        ''' <summary>Data情報</summary>
        Public DataLine As String
        ''' <summary>Row開始タグ</summary>
        Public StartRowLine As String
        ''' <summary>Row終了タグ</summary>
        Public EndRowLine As String
        ''' <summary>Table開始タグ</summary>
        Public StartTableLine As String
        ''' <summary>Table終了タグ</summary>
        Public EndTableLine As String
        ''' <summary>'Book終了タグ</summary>
        Public EndWorkBookLine As String
    End Structure

    Dim str_ExcelLine As ExcelLineType
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim purchaseGroupDisp As TCIDataAccess.Join.PurchaseGroupList = New TCIDataAccess.Join.PurchaseGroupList

        dim st_LocationCode As String = Session("LocationCode").ToString

        If IsPostBack = False Then
            purchaseGroupDisp.Load(st_LocationCode)
            UserList.DataSource = purchaseGroupDisp
            UserList.DataBind
        End If
    End Sub
    Public Function GetStorageLocations(ByVal userid As String) As String
        Dim ret As String = ""
        Dim SLocationByPUser As DataTable = GetDataTable("select * from StorageByPurchasingUser where UserID=" + userid)
        For i As Integer = 0 To SLocationByPUser.Rows.Count - 1
            ret += SLocationByPUser.Rows(i)("Storage").ToString + ","
        Next
        If ret.Length > 0 Then
            ret = ret.Substring(0, ret.Length - 1)
        End If
        Return ret
    End Function
End Class
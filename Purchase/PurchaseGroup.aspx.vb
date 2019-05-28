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
        If IsPostBack = False Then
            Dim st_SQL As String = String.Empty
            st_SQL &= "SELECT "
            st_SQL &= " UserID, "
            st_SQL &= " LocationName, "
            st_SQL &= " AccountName, "
            st_SQL &= " SurName, "
            st_SQL &= " GivenName, "
            st_SQL &= " R3PurchasingGroup, "
            st_SQL &= " PrivilegeLevel, "
            st_SQL &= " isAdmin, "
            st_SQL &= " isDisabled, "
            st_SQL &= " 'PurchaseGroupSetting.aspx?Action=Edit&UserID=' + Cast(UserID AS varchar) AS URL "
            st_SQL &= "FROM "
            st_SQL &= " v_UserAll "
            st_SQL &= "ORDER BY "
            st_SQL &= " LocationName, "
            st_SQL &= " isDisabled, "
            st_SQL &= " SurName, "
            st_SQL &= " GivenName"
            SrcUser.SelectCommand = st_SQL
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
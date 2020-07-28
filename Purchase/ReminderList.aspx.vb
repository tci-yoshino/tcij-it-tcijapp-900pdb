Option Explicit On
Option Infer Off
Option Strict On
Public Class ReminderList
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Dim st_SQL As String = String.Empty
            st_SQL &= "SELECT "
            st_SQL &= "SupplyingPlant, "
            st_SQL &= "case when ShowType=0 Then 'constant'else 'Formula' end as 'ShowType', "
            st_SQL &= "FirstRem, "
            st_SQL &= "SecondRem, "
            st_SQL &= "ThirdRem, "
            st_SQL &= " 'ReminderSetting.aspx?Action=Edit&SupplyingPlant=' + Cast(SupplyingPlant AS varchar) AS URL "
            st_SQL &= "FROM "
            st_SQL &= "Purchase.dbo.Reminder "
            SrcRemined.SelectCommand = st_SQL
        End If
    End Sub

End Class
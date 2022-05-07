Option Explicit On
Option Strict On
Option Infer Off

Imports System.Data.SqlClient

Namespace TCIDataAccess.Join

    Public Class PurchaseGroupSettingDisp

    End Class

    Public Class PurchaseGroupSettingDispList
        Inherits List(Of PurchaseGroupSettingDisp)
        Public Sub New()

        End Sub
        ''' <summary>
        ''' PurchaseGroupSetting 情報を取得する
        ''' </summary>
        ''' 
        Public Sub Load()
            Dim Value As StringBuilder = New StringBuilder

        End Sub

    End Class

End Namespace

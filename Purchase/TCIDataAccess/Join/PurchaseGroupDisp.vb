Option Explicit On
Option Strict On
Option Infer Off

Imports System.Data.SqlClient

Namespace TCIDataAccess.Join

    Public Class PurchaseGroupDisp

        Protected _UserID As String = String.Empty
        Protected _LocationName As String = String.Empty
        Protected _AccountName As String = String.Empty
        Protected _SurName As String = String.Empty
        Protected _GivenName As String = String.Empty
        Protected _R3PurchasingGroup As String = String.Empty
        Protected _PrivilegeLevel As String = String.Empty
        Protected _isAdmin As String = String.Empty
        Protected _isDisabled As String = String.Empty
        Protected _RFQCorrespondenceEditable As String = String.Empty
        Protected _MMSTAInvalidationEditable As String = String.Empty
        Protected _URL As String = String.Empty

        Public Property UserID() As String
            Get
                Return _UserID
            End Get
            Set(ByVal value As String)
                _UserID = value
            End Set
        End Property

        Public Property LocationName() As String
            Get
                Return _LocationName
            End Get
            Set(ByVal value As String)
                _LocationName = value
            End Set
        End Property

        Public Property AccountName() As String
            Get
                Return _AccountName
            End Get
            Set(ByVal value As String)
                _AccountName = value
            End Set
        End Property

        Public Property SurName() As String
            Get
                Return _SurName
            End Get
            Set(ByVal value As String)
                _SurName = value
            End Set
        End Property

        Public Property GivenName() As String
            Get
                Return _GivenName
            End Get
            Set(ByVal value As String)
                _GivenName = value
            End Set
        End Property

        Public Property R3PurchasingGroup() As String
            Get
                Return _R3PurchasingGroup
            End Get
            Set(ByVal value As String)
                _R3PurchasingGroup = value
            End Set
        End Property

        Public Property PrivilegeLevel() As String
            Get
                Return _PrivilegeLevel
            End Get
            Set(ByVal value As String)
                _PrivilegeLevel = value
            End Set
        End Property

        Public Property isAdmin() As String
            Get
                Return _isAdmin
            End Get
            Set(ByVal value As String)
                _isAdmin = value
            End Set
        End Property

        Public Property isDisabled() As String
            Get
                Return _isDisabled
            End Get
            Set(ByVal value As String)
                _isDisabled = value
            End Set
        End Property

        Public Property RFQCorrespondenceEditable() As String
            Get
                Return _RFQCorrespondenceEditable
            End Get
            Set(ByVal value As String)
                _RFQCorrespondenceEditable = value
            End Set
        End Property

        Public Property MMSTAInvalidationEditable() As String
            Get
                Return _MMSTAInvalidationEditable
            End Get
            Set(ByVal value As String)
                _MMSTAInvalidationEditable = value
            End Set
        End Property

        Public Property URL() As String
            Get
                Return _URL
            End Get
            Set(ByVal value As String)
                _URL = value
            End Set
        End Property

        Public Sub New()

        End Sub

    End Class

    Public Class PurchaseGroupList
        Inherits List(Of PurchaseGroupDisp)
        Public Sub New()

        End Sub
        ''' <summary>
        ''' PurchaseGroup 情報を取得する
        ''' </summary>
        ''' 
        Public Sub Load(ByVal st_LocationCode As String)
            Dim Value As StringBuilder = New StringBuilder

            'SQL文字列の作成
            Value.AppendLine("SELECT ")
            Value.AppendLine("  VU.[UserID], ")
            Value.AppendLine("  VU.[LocationName], ")
            Value.AppendLine("  VU.[AccountName], ")
            Value.AppendLine("  VU.[SurName], ")
            Value.AppendLine("  VU.[GivenName], ")
            Value.AppendLine("  VU.[R3PurchasingGroup], ")
            Value.AppendLine("  VU.[PrivilegeLevel], ")
            Value.AppendLine("  VU.[isAdmin], ")
            Value.AppendLine("  VU.[isDisabled], ")
            Value.AppendLine("  CASE VU.[RFQCorrespondenceEditable] ")
            Value.AppendLine("    WHEN 1 THEN 'Y' ")
            Value.AppendLine("    ELSE '' ")
            Value.AppendLine("  END AS RFQCorrespondenceEditable, ")
            Value.AppendLine("  CASE VU.[MMSTAInvalidationEditable] ")
            Value.AppendLine("    WHEN 1 THEN 'Y' ")
            Value.AppendLine("    ELSE '' ")
            Value.AppendLine("  END AS MMSTAInvalidationEditable, ")
            Value.AppendLine("  'PurchaseGroupSetting.aspx?Action=Edit&UserID=' + Cast(VU.[UserID] AS varchar) AS URL ")
            Value.AppendLine("FROM ")
            Value.AppendLine("  [v_UserAll] AS VU ")
            Value.AppendLine("WHERE ")
            Value.AppendLine("  VU.[isDisabled] = 0 ")
            Value.AppendLine("  And VU.[LocationCode] = @LocationCode ")
            Value.AppendLine("ORDER BY ")
            Value.AppendLine("  VU.[LocationName], VU.[isDisabled], VU.[SurName], VU.[GivenName] ")

            Dim DBConn As SqlConnection = New SqlConnection(DBCommon.DB_CONNECT_STRING)
            Dim DBCommand As SqlCommand = DBConn.CreateCommand
            DBCommand.CommandText = Value.ToString
            DBCommand.Parameters.Clear()
            DBCommand.Parameters.AddWithValue("LocationCode", st_LocationCode)

            DBConn.Open()
            Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()

            While DBReader.Read
                Dim dc_PurchaseGroup As PurchaseGroupDisp = New PurchaseGroupDisp
                DBCommon.SetProperty(DBReader("UserID"), dc_PurchaseGroup.UserID)
                DBCommon.SetProperty(DBReader("LocationName"), dc_PurchaseGroup.LocationName)
                DBCommon.SetProperty(DBReader("AccountName"), dc_PurchaseGroup.AccountName)
                DBCommon.SetProperty(DBReader("SurName"), dc_PurchaseGroup.SurName)
                DBCommon.SetProperty(DBReader("GivenName"), dc_PurchaseGroup.GivenName)
                DBCommon.SetProperty(DBReader("R3PurchasingGroup"), dc_PurchaseGroup.R3PurchasingGroup)
                DBCommon.SetProperty(DBReader("PrivilegeLevel"), dc_PurchaseGroup.PrivilegeLevel)
                DBCommon.SetProperty(DBReader("isAdmin"), dc_PurchaseGroup.isAdmin)
                DBCommon.SetProperty(DBReader("isDisabled"), dc_PurchaseGroup.isDisabled)
                DBCommon.SetProperty(DBReader("RFQCorrespondenceEditable"), dc_PurchaseGroup.RFQCorrespondenceEditable)
                DBCommon.SetProperty(DBReader("MMSTAInvalidationEditable"), dc_PurchaseGroup.MMSTAInvalidationEditable)
                DBCommon.SetProperty(DBReader("URL"), dc_PurchaseGroup.URL)
                Me.Add(dc_PurchaseGroup)
            End While
            DBReader.Close()
        End Sub

    End Class

End Namespace

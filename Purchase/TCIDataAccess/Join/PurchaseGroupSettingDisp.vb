Option Explicit On
Option Strict On
Option Infer Off

Imports System.Data.SqlClient

Namespace TCIDataAccess.Join

    Public Class PurchaseGroupSettingDisp

        Protected _UserID As String = String.Empty
        Protected _LocationName As String = String.Empty
        Protected _AccountName As String = String.Empty
        Protected _RFQCorrespondenceEditable As String = String.Empty
        Protected _MMSTAInvalidationEditable As String = String.Empty
        Protected _Name As String = String.Empty
        Protected _R3PurchasingGroup As String = String.Empty
        Protected _StorageByPurchasingUserList As List(Of StorageByPurchasingUser)

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

        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
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

        Public Property StorageByPurchasingUserList() As List(Of StorageByPurchasingUser)
            Get
                Return _StorageByPurchasingUserList
            End Get
            Set(ByVal value As List(Of StorageByPurchasingUser))
                _StorageByPurchasingUserList = value
            End Set
        End Property

        Public Sub New()

        End Sub

    End Class

    Public Class PurchaseGroupSettingDispList
        Inherits List(Of PurchaseGroupSettingDisp)
        Public Sub New()

        End Sub

        ''' <summary>
        ''' PurchaseGroup 情報を取得する
        ''' </summary>
        ''' 
        Public Sub Load(ByVal i_UserID As Integer)
            Dim Value As StringBuilder = New StringBuilder

            'SQL文字列の作成
            Value.AppendLine("SELECT ")
            Value.AppendLine("  VU.[UserID], ")
            Value.AppendLine("  VU.[LocationName], ")
            Value.AppendLine("  VU.[AccountName], ")
            Value.AppendLine("  CASE VU.[RFQCorrespondenceEditable] ")
            Value.AppendLine("    WHEN 1 THEN 'Y' ")
            Value.AppendLine("    ELSE '' ")
            Value.AppendLine("  END AS RFQCorrespondenceEditable, ")
            Value.AppendLine("  CASE VU.[MMSTAInvalidationEditable] ")
            Value.AppendLine("    WHEN 1 THEN 'Y' ")
            Value.AppendLine("    ELSE '' ")
            Value.AppendLine("  END AS MMSTAInvalidationEditable, ")
            Value.AppendLine("  VU.[Name], ")
            Value.AppendLine("  VU.[R3PurchasingGroup] ")
            Value.AppendLine("FROM ")
            Value.AppendLine("  [v_UserAll] AS VU ")
            Value.AppendLine("WHERE ")
            Value.AppendLine("  UserID = @UserID ")

            Dim DBConn As SqlConnection = New SqlConnection(DBCommon.DB_CONNECT_STRING)
            Dim DBCommand As SqlCommand = DBConn.CreateCommand
            DBCommand.CommandText = Value.ToString
            DBCommand.Parameters.Clear()
            DBCommand.Parameters.AddWithValue("UserID", i_UserID)

            DBConn.Open()
            Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()

            Dim dc_PurchaseGroupSettingDisp As PurchaseGroupSettingDisp = New PurchaseGroupSettingDisp
            While DBReader.Read
                DBCommon.SetProperty(DBReader("UserID"), dc_PurchaseGroupSettingDisp.UserID)
                DBCommon.SetProperty(DBReader("LocationName"), dc_PurchaseGroupSettingDisp.LocationName)
                DBCommon.SetProperty(DBReader("AccountName"), dc_PurchaseGroupSettingDisp.AccountName)
                DBCommon.SetProperty(DBReader("RFQCorrespondenceEditable"), dc_PurchaseGroupSettingDisp.RFQCorrespondenceEditable)
                DBCommon.SetProperty(DBReader("MMSTAInvalidationEditable"), dc_PurchaseGroupSettingDisp.MMSTAInvalidationEditable)
                DBCommon.SetProperty(DBReader("Name"), dc_PurchaseGroupSettingDisp.Name)
                DBCommon.SetProperty(DBReader("R3PurchasingGroup"), dc_PurchaseGroupSettingDisp.R3PurchasingGroup)
                Me.Add(dc_PurchaseGroupSettingDisp)
            End While

            dc_PurchaseGroupSettingDisp.StorageByPurchasingUserList = GetStorageByPurchasingUserList(i_UserID)

            DBReader.Close()
        End Sub

        ''' <summary>
        ''' PurchaseGroup 情報を取得する
        ''' </summary>
        ''' 
        Private Function GetStorageByPurchasingUserList(ByVal i_UserID As Integer) As List(Of StorageByPurchasingUser)
            Dim lst_StorageByPurchasingUserList As List(Of StorageByPurchasingUser) = New List(Of StorageByPurchasingUser)
            Dim dc_StorageByPurchasingUserList As TCIDataAccess.StorageByPurchasingUserList = New TCIDataAccess.StorageByPurchasingUserList
            dc_StorageByPurchasingUserList.LoadFromUserID(i_UserID)
            
            For Each storageByPurchasingUser As StorageByPurchasingUser In dc_StorageByPurchasingUserList
                lst_StorageByPurchasingUserList.add(storageByPurchasingUser)
            Next

            Return lst_StorageByPurchasingUserList
        End Function

    End Class

End Namespace

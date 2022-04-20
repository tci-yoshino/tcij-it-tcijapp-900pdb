Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Text
Imports System.Data.SqlClient
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess.Join
    Public Class MyTaskDisp
        Protected _RFQNumber As Integer = 0
        Protected _Priority As String = String.Empty
        Protected _CreateDate As DateTime
        Protected _StatusChangeDate As DateTime
        Protected _SupplierCode As Integer = 0
        Protected _Status As String = String.Empty
        Protected _StatusCode As String = String.Empty
        Protected _ProductNumber As String = String.Empty
        Protected _ProductName As String = String.Empty
        Protected _Purpose As String = String.Empty
        Protected _QuoUserName As String = String.Empty
        Protected _QuoLocationName As String = String.Empty
        Protected _EnqUserName As String = String.Empty
        Protected _EnqLocationName As String = String.Empty
        Protected _SupplierName As String = String.Empty
        Protected _RFQCorres As String = String.Empty
        Protected _MakerName As String = String.Empty
        Protected _isCONFIDENTIAL As String = String.Empty
        Protected _ProductID As String = String.Empty
        Public Property RFQNumber() As Integer
            Get
                Return _RFQNumber
            End Get
            Set(ByVal value As Integer)
                _RFQNumber = value
            End Set
        End Property
        Public Property CreateDate() As DateTime
            Get
                Return _CreateDate
            End Get
            Set(ByVal value As DateTime)
                _CreateDate = value
            End Set
        End Property
        Public Property StatusChangeDate() As DateTime
            Get
                Return _StatusChangeDate
            End Get
            Set(ByVal value As DateTime)
                _StatusChangeDate = value
            End Set
        End Property
        Public Property Priority() As String
            Get
                Return _Priority
            End Get
            Set(ByVal value As String)
                _Priority = value
            End Set
        End Property
        Public Property Status() As String
            Get
                Return _Status
            End Get
            Set(ByVal value As String)
                _Status = value
            End Set
        End Property
        Public Property StatusCode() As String
            Get
                Return _StatusCode
            End Get
            Set(ByVal value As String)
                _StatusCode = value
            End Set
        End Property
        Public Property ProductNumber() As String
            Get
                Return _ProductNumber
            End Get
            Set(ByVal value As String)
                _ProductNumber = value
            End Set
        End Property
        Public Property ProductName() As String
            Get
                Return _ProductName
            End Get
            Set(ByVal value As String)
                _ProductName = value
            End Set
        End Property

        Public Property Purpose() As String
            Get
                Return _Purpose
            End Get
            Set(ByVal value As String)
                _Purpose = value
            End Set
        End Property
        Public Property QuoUserName() As String
            Get
                Return _QuoUserName
            End Get
            Set(ByVal value As String)
                _QuoUserName = value
            End Set
        End Property
        Public Property QuoLocationName() As String
            Get
                Return _QuoLocationName
            End Get
            Set(ByVal value As String)
                _QuoLocationName = value
            End Set
        End Property
        Public Property EnqUserName() As String
            Get
                Return _EnqUserName
            End Get
            Set(ByVal value As String)
                _EnqUserName = value
            End Set
        End Property
        Public Property EnqLocationName() As String
            Get
                Return _EnqLocationName
            End Get
            Set(ByVal value As String)
                _EnqLocationName = value
            End Set
        End Property
        Public Property SupplierName() As String
            Get
                Return _SupplierName
            End Get
            Set(ByVal value As String)
                _SupplierName = value
            End Set
        End Property
        Public Property RFQCorrespondence() As String
            Get
                Return _RFQCorres
            End Get
            Set(ByVal value As String)
                _RFQCorres = value
            End Set
        End Property
        Public Property SupplierCode() As String
            Get
                Return _SupplierCode
            End Get
            Set(ByVal value As String)
                _SupplierCode = value
            End Set
        End Property
        Public Property MakerName() As String
            Get
                Return _MakerName
            End Get
            Set(ByVal value As String)
                _MakerName = value
            End Set
        End Property
        Public Property isCONFIDENTIAL() As String
            Get
                Return _isCONFIDENTIAL
            End Get
            Set(ByVal value As String)
                _isCONFIDENTIAL = value
            End Set
        End Property
        Public Property ProductID() As String
            Get
                Return _ProductID
            End Get
            Set(ByVal value As String)
                _ProductID = value
            End Set
        End Property
        
        Public Sub New()

        End Sub

    End Class
    Public Class MyTaskDispList
        Inherits List(Of MyTaskDisp)
        Public Sub New()

        End Sub
        ''' <summary>
        ''' MyTask情報を取得する
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <param name="RFQPriority"></param>
        ''' <param name="RFQStatus"></param>
        ''' <param name="SESSION_ROLE_CODE"></param>
        ''' 
        Public Sub Load(ByVal userID As Integer, ByVal RFQPriority As String, ByVal RFQStatus As String, ByVal StatusUpdate As String, ByVal SESSION_ROLE_CODE As String)
            Dim Value As StringBuilder = New StringBuilder
            'SQL文字列の作成
            Value.AppendLine("SELECT")
            Value.AppendLine("    RH.[RFQNumber],")
            Value.AppendLine("    CASE WHEN RH.[Priority] IS NULL THEN 1 ELSE 0  END AS PrioritySort,")
            Value.AppendLine("    ISNULL(RH.[Priority], '') AS Priority,")
            Value.AppendLine("    RH.[CreateDate],")
            Value.AppendLine("    RH.[StatusSortOrder],")
            Value.AppendLine("    RH.[StatusChangeDate], ")
            Value.AppendLine("    RH.[Status],")
            Value.AppendLine("    RH.[StatusCode],")
            Value.AppendLine("    RH.[ProductNumber],")
            Value.AppendLine("    RH.[ProductName] AS ProductName,")
            Value.AppendLine("    RH.[Purpose],")
            Value.AppendLine("    RH.[QuoUserName],")
            Value.AppendLine("    RH.[QuoLocationName],")
            Value.AppendLine("    RH.[EnqUserName],")
            Value.AppendLine("    RH.[EnqLocationName],")
            Value.AppendLine("    RH.[SupplierName],")
            Value.AppendLine("    RH.[MakerName],")
            Value.AppendLine("    RR.[RFQCorres] AS RFQCorrespondence,")
            Value.AppendLine("    RH.[isCONFIDENTIAL],")
            Value.AppendLine("    RH.[ProductID],")
            Value.AppendLine("    RH.[SupplierCode]")
            Value.AppendLine("FROM")
            Value.AppendLine("    [v_RFQHeader] AS RH")
            Value.AppendLine("LEFT OUTER JOIN")
            Value.AppendLine("    [v_RFQReminder] AS RR ON RH.[RFQNumber] = RR.[RFQNumber] AND @UserID = RR.[RcptUserID]")
            Value.AppendLine("WHERE")
            Value.AppendLine("    RH.[QuoUserID] = @UserID AND")
            Value.AppendLine("    RH.[EnqUserID] != @UserID AND")
            Value.AppendLine("    RH.[StatusCode] NOT IN('Q','C')")
            Select Case RFQPriority
                Case "A"
                    Value.AppendLine("  AND RH.[Priority] = 'A'")
                Case "B"
                    Value.AppendLine("  AND RH.[Priority] = 'B'")
                Case "AB"
                    Value.AppendLine("  AND RH.[Priority] IN('A','B')")
            End Select
            Select Case RFQStatus
                Case "N"
                    Value.AppendLine("  AND RH.[Status] = 'Create'")
                Case "A"
                    Value.AppendLine("  AND RH.[Status] = 'Assigned'")
                Case "E"
                    Value.AppendLine("  AND RH.[Status] = 'Enquired'")
                Case "PQ"
                    Value.AppendLine("  AND RH.[Status] = 'Partly-quoted'")
                Case "Q"
                    Value.AppendLine("  AND RH.[Status] = 'Quoted'")
                Case "II"
                    Value.AppendLine("  AND RH.[Status] = 'Interface Issued'")
                Case "C"
                    Value.AppendLine("  AND RH.[Status] = 'Closed'")
            End Select

            '権限ロールに従い極秘品を除外する
            If SESSION_ROLE_CODE = Common.ROLE_WRITE_P OrElse SESSION_ROLE_CODE = Common.ROLE_READ_P Then
                Value.AppendLine("  AND RH.[isCONFIDENTIAL] = 0")
            End If
            Value.AppendLine("UNION ALL ")
            Value.AppendLine("SELECT")
            Value.AppendLine("    RH.[RFQNumber],")
            Value.AppendLine("    CASE WHEN RH.[Priority] IS NULL THEN 1 ELSE 0  END AS PrioritySort,")
            Value.AppendLine("    ISNULL(RH.[Priority], '') AS Priority,")
            Value.AppendLine("    RH.[CreateDate],")
            Value.AppendLine("    RH.[StatusSortOrder],")
            Value.AppendLine("    RH.[StatusChangeDate],")
            Value.AppendLine("    RH.[Status],")
            Value.AppendLine("    RH.[StatusCode],")
            Value.AppendLine("    RH.[ProductNumber],")
            Value.AppendLine("    RH.[ProductName] AS ProductName,")
            Value.AppendLine("    RH.[Purpose],")
            Value.AppendLine("    RH.[QuoUserName],")
            Value.AppendLine("    RH.[QuoLocationName],")
            Value.AppendLine("    RH.[EnqUserName],")
            Value.AppendLine("    RH.[EnqLocationName],")
            Value.AppendLine("    RH.[SupplierName],")
            Value.AppendLine("    RH.[MakerName],")
            Value.AppendLine("    RR.[RFQCorres] AS RFQCorrespondence,")
            Value.AppendLine("    RH.[isCONFIDENTIAL],")
            Value.AppendLine("    RH.[ProductID],")
            Value.AppendLine("    RH.[SupplierCode]")
            Value.AppendLine("FROM")
            Value.AppendLine("    [v_RFQHeader] AS RH")
            Value.AppendLine("LEFT OUTER JOIN")
            Value.AppendLine("    [v_RFQReminder] AS RR ON RH.[RFQNumber] = RR.[RFQNumber] AND @UserID = RR.[RcptUserID]")
            Value.AppendLine("WHERE")
            Value.AppendLine("    RH.[QuoUserID] = @UserID AND")
            Value.AppendLine("    RH.[EnqUserID] != @UserID AND")
            Value.AppendLine("    RH.[StatusCode] IN('Q','C') AND")
            Value.AppendLine("    RR.[RFQHistoryNumber] IS NOT NULL")
            Select Case RFQPriority
                Case "A"
                    Value.AppendLine("    AND RH.[Priority] = 'A'")
                Case "B"
                    Value.AppendLine("    AND RH.[Priority] = 'B'")
                Case "AB"
                    Value.AppendLine("    AND RH.[Priority] IN('A','B')")
            End Select
            Select Case RFQStatus
                Case "N"
                    Value.AppendLine("    AND RH.[Status] = 'Create'")
                Case "A"
                    Value.AppendLine("    AND RH.[Status] = 'Assigned'")
                Case "E"
                    Value.AppendLine("    AND RH.[Status] = 'Enquired'")
                Case "PQ"
                    Value.AppendLine("    AND RH.[Status] = 'Partly-quoted'")
                Case "Q"
                    Value.AppendLine("    AND RH.[Status] = 'Quoted'")
                Case "II"
                    Value.AppendLine("    AND RH.[Status] = 'Interface Issued'")
                Case "C"
                    Value.AppendLine("    AND RH.[Status] = 'Closed'")
            End Select

            If SESSION_ROLE_CODE = Common.ROLE_WRITE_P OrElse SESSION_ROLE_CODE = Common.ROLE_READ_P Then
                Value.AppendLine("    AND RH.[isCONFIDENTIAL] = 0")
            End If
            Value.AppendLine("ORDER BY")

            Select Case StatusUpdate
                Case "REM"
                    Value.AppendLine("    [RFQCorres] DESC,")
                    Value.AppendLine("    [PrioritySort] ASC,")
                    Value.AppendLine("    [Priority] ASC,")
                Case "ASC"
                    Value.AppendLine("    RH.[StatusChangeDate] ASC,")
                Case "DESC"
                    Value.AppendLine("    RH.[StatusChangeDate] DESC,")
            End Select
            Value.AppendLine("    RH.[StatusSortOrder] ASC")
            Value.AppendLine("OPTION (RECOMPILE)")


            Dim DBConn As SqlConnection = New SqlConnection(DBCommon.DB_CONNECT_STRING)
            Dim DBCommand As SqlCommand = DBConn.CreateCommand
            DBCommand.CommandText = Value.ToString
            DBCommand.Parameters.Clear()
            DBCommand.Parameters.AddWithValue("UserID", userID)

            DBConn.Open()
            Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()

            While DBReader.Read
                Dim dc_Date As MyTaskDisp = New MyTaskDisp
                DBCommon.SetProperty(DBReader("RFQNumber"), dc_Date.RFQNumber)
                DBCommon.SetProperty(DBReader("Priority"), dc_Date.Priority)
                DBCommon.SetProperty(DBReader("CreateDate"), dc_Date.CreateDate)
                DBCommon.SetProperty(DBReader("StatusChangeDate"), dc_Date.StatusChangeDate)
                DBCommon.SetProperty(DBReader("Status"), dc_Date.Status)
                DBCommon.SetProperty(DBReader("StatusCode"), dc_Date.StatusCode)
                DBCommon.SetProperty(DBReader("ProductNumber"), dc_Date.ProductNumber)
                DBCommon.SetProperty(DBReader("ProductName"), dc_Date.ProductName)
                DBCommon.SetProperty(DBReader("Purpose"), dc_Date.Purpose)
                DBCommon.SetProperty(DBReader("QuoUserName"), dc_Date.QuoUserName)
                DBCommon.SetProperty(DBReader("QuoLocationName"), dc_Date.QuoLocationName)
                DBCommon.SetProperty(DBReader("EnqUserName"), dc_Date.EnqUserName)
                DBCommon.SetProperty(DBReader("EnqLocationName"), dc_Date.EnqLocationName)
                DBCommon.SetProperty(DBReader("SupplierName"), dc_Date.SupplierName)
                DBCommon.SetProperty(DBReader("RFQCorrespondence"), dc_Date.RFQCorrespondence)
                DBCommon.SetProperty(DBReader("MakerName"), dc_Date.MakerName)
                DBCommon.SetProperty(DBReader("isCONFIDENTIAL"), dc_Date.isCONFIDENTIAL)
                DBCommon.SetProperty(DBReader("ProductID"), dc_Date.ProductID)
                DBCommon.SetProperty(DBReader("SupplierCode"), dc_Date.SupplierCode)
                Me.Add(dc_Date)
            End While
            DBReader.Close()
        End Sub
    End Class
End Namespace
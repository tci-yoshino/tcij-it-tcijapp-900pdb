Imports System.Data.SqlClient
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess.Join

    ''' <summary>
    ''' CCReminderDisp データクラス
    ''' </summary>
    Public Class CCReminderDisp

        Protected _RFQNumber As Integer = 0
        Protected _Priority As String = String.Empty
        Protected _CreateDate As DateTime = New DateTime(0)
        Protected _StatusChangeDate As DateTime = New DateTime(0)
        Protected _StatusCode As String = String.Empty
        Protected _Status As String = String.Empty
        Protected _RFQCorres As String = String.Empty
        Protected _isCONFIDENTIAL As Boolean = False
        Protected _ProductID As Integer = 0
        Protected _ProductNumber As String = String.Empty
        Protected _ProductName As String = String.Empty
        Protected _Purpose As String = String.Empty
        Protected _QuoUserName As String = String.Empty
        Protected _QuoLocationName As String = String.Empty
        Protected _EnqUserName As String = String.Empty
        Protected _EnqLocationName As String = String.Empty
        Protected _SupplierCode As Integer = 0
        Protected _SupplierName As String = String.Empty
        Protected _MakerName As String = String.Empty

        ''' <summary>
        ''' RFQNumber を設定, または取得します
        ''' </summary>
        Public Property RFQNumber() As Integer
            Get
                Return _RFQNumber
            End Get
            Set(ByVal value As Integer)
                _RFQNumber = value
            End Set
        End Property

        ''' <summary>
        ''' Priority を設定, または取得します
        ''' </summary>
        Public Property Priority() As String
            Get
                Return _Priority
            End Get
            Set(ByVal value As String)
                _Priority = value
            End Set
        End Property

        ''' <summary>
        ''' CreateDate を設定, または取得します
        ''' </summary>
        Public Property CreateDate() As DateTime
            Get
                Return _CreateDate
            End Get
            Set(ByVal value As DateTime)
                _CreateDate = value
            End Set
        End Property

        ''' <summary>
        ''' StatusChangeDate を設定, または取得します
        ''' </summary>
        Public Property StatusChangeDate() As DateTime
            Get
                Return _StatusChangeDate
            End Get
            Set(ByVal value As DateTime)
                _StatusChangeDate = value
            End Set
        End Property

        ''' <summary>
        ''' StatusCode を設定, または取得します
        ''' </summary>
        Public Property StatusCode() As String
            Get
                Return _StatusCode
            End Get
            Set(ByVal value As String)
                _StatusCode = value
            End Set
        End Property

        ''' <summary>
        ''' Status を設定, または取得します
        ''' </summary>
        Public Property Status() As String
            Get
                Return _Status
            End Get
            Set(ByVal value As String)
                _Status = value
            End Set
        End Property

        ''' <summary>
        ''' RFQCorres を設定, または取得します
        ''' </summary>
        Public Property RFQCorres() As String
            Get
                Return _RFQCorres
            End Get
            Set(ByVal value As String)
                _RFQCorres = value
            End Set
        End Property

        ''' <summary>
        ''' isCONFIDENTIAL を設定, または取得します
        ''' </summary>
        Public Property isCONFIDENTIAL() As Boolean
            Get
                Return _isCONFIDENTIAL
            End Get
            Set(ByVal value As Boolean)
                _isCONFIDENTIAL = value
            End Set
        End Property

        ''' <summary>
        ''' ProductID を設定, または取得します
        ''' </summary>
        Public Property ProductID() As Integer
            Get
                Return _ProductID
            End Get
            Set(ByVal value As Integer)
                _ProductID = value
            End Set
        End Property

        ''' <summary>
        ''' ProductNumber を設定, または取得します
        ''' </summary>
        Public Property ProductNumber() As String
            Get
                Return _ProductNumber
            End Get
            Set(ByVal value As String)
                _ProductNumber = value
            End Set
        End Property

        ''' <summary>
        ''' ProductName を設定, または取得します
        ''' </summary>
        Public Property ProductName() As String
            Get
                Return _ProductName
            End Get
            Set(ByVal value As String)
                _ProductName = value
            End Set
        End Property

        ''' <summary>
        ''' Purpose を設定, または取得します
        ''' </summary>
        Public Property Purpose() As String
            Get
                Return _Purpose
            End Get
            Set(ByVal value As String)
                _Purpose = value
            End Set
        End Property

        ''' <summary>
        ''' QuoUserName を設定, または取得します
        ''' </summary>
        Public Property QuoUserName() As String
            Get
                Return _QuoUserName
            End Get
            Set(ByVal value As String)
                _QuoUserName = value
            End Set
        End Property

        ''' <summary>
        ''' QuoLocationName を設定, または取得します
        ''' </summary>
        Public Property QuoLocationName() As String
            Get
                Return _QuoLocationName
            End Get
            Set(ByVal value As String)
                _QuoLocationName = value
            End Set
        End Property

        ''' <summary>
        ''' EnqUserName を設定, または取得します
        ''' </summary>
        Public Property EnqUserName() As String
            Get
                Return _EnqUserName
            End Get
            Set(ByVal value As String)
                _EnqUserName = value
            End Set
        End Property

        ''' <summary>
        ''' EnqLocationName を設定, または取得します
        ''' </summary>
        Public Property EnqLocationName() As String
            Get
                Return _EnqLocationName
            End Get
            Set(ByVal value As String)
                _EnqLocationName = value
            End Set
        End Property

        ''' <summary>
        ''' SupplierCode を設定, または取得します
        ''' </summary>
        Public Property SupplierCode() As Integer
            Get
                Return _SupplierCode
            End Get
            Set(ByVal value As Integer)
                _SupplierCode = value
            End Set
        End Property

        ''' <summary>
        ''' SupplierName を設定, または取得します
        ''' </summary>
        Public Property SupplierName() As String
            Get
                Return _SupplierName
            End Get
            Set(ByVal value As String)
                _SupplierName = value
            End Set
        End Property

        ''' <summary>
        ''' MakerName を設定, または取得します
        ''' </summary>
        Public Property MakerName() As String
            Get
                Return _MakerName
            End Get
            Set(ByVal value As String)
                _MakerName = value
            End Set
        End Property

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' CC リマインダが存在するか否かを返します
        ''' </summary>
        ''' <param name="UserID">ユーザ ID</param>
        ''' <returns>存在する場合は True, 存在しない場合は False</returns>
        Public Shared Function IsExists(ByVal UserID As Integer) As Boolean

            Dim Value As New StringBuilder
            Value.AppendLine("SELECT")
            Value.AppendLine("    COUNT(*)")
            Value.AppendLine("FROM")
            Value.AppendLine("    RFQHistory")
            Value.AppendLine("WHERE")
            Value.AppendLine("    isChecked = 0")
            Value.AppendLine("    AND ISNULL(RFQCorresCode,'') != 'NS'")
            Value.AppendLine("    AND (RcptUserID = CCUserID1")
            Value.AppendLine("        OR RcptUserID = CCUserID2)")
            Value.AppendLine("    AND RcptUserID = @UserID")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                DBConn.Open()

                Using DBCommand As SqlCommand = DBConn.CreateCommand
                    DBCommand.CommandText = Value.ToString
                    DBCommand.Parameters.Add("UserID", SqlDbType.Int)
                    DBCommand.Parameters("UserID").Value = UserID

                    Dim count As Integer = Convert.ToInt32(DBCommand.ExecuteScalar())
                    Return count > 0
                End Using
            End Using

        End Function

    End Class

    ''' <summary>
    ''' CCReminderDisp データリストクラス
    ''' </summary>
    Public Class CCReminderDispList
        Inherits List(Of CCReminderDisp)

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' CC リマインダ一覧を読み込みます
        ''' </summary>
        ''' <param name="UserID">ユーザ ID</param>
        Public Sub Load(ByVal UserID As Integer)

            Dim Value As New StringBuilder
            Value.AppendLine("WITH OldestHistory AS (")
            Value.AppendLine("    SELECT")
            Value.AppendLine("        MIN(RH.RFQHistoryNumber) AS RFQHistoryNumber")
            Value.AppendLine("        ,RH.RFQNumber")
            Value.AppendLine("        ,RC.[Text] AS RFQCorres")
            Value.AppendLine("    FROM")
            Value.AppendLine("        RFQCorres AS RC")
            Value.AppendLine("        ,RFQHistory AS RH")
            Value.AppendLine("    WHERE")
            Value.AppendLine("        RH.isChecked = 0")
            Value.AppendLine("        AND ISNULL(RH.RFQCorresCode, '') != 'NS'")
            Value.AppendLine("        AND RH.RcptUserID = @UserID")
            Value.AppendLine("        AND (RH.RcptUserID = RH.CCUserID1")
            Value.AppendLine("            OR RH.RcptUserID = RH.CCUserID2)")
            Value.AppendLine("        AND RH.RFQCorresCode = RC.RFQCorresCode")
            Value.AppendLine("    GROUP BY")
            Value.AppendLine("        RH.RFQNumber")
            Value.AppendLine("        ,RH.RcptUserID")
            Value.AppendLine("        ,RC.[Text]")
            Value.AppendLine(")")
            Value.AppendLine("SELECT")
            Value.AppendLine("    RH.RFQNumber")
            Value.AppendLine("    ,RH.[Priority]")
            Value.AppendLine("    ,RH.CreateDate")
            Value.AppendLine("    ,RH.StatusChangeDate")
            Value.AppendLine("    ,RH.StatusCode")
            Value.AppendLine("    ,RH.[Status]")
            Value.AppendLine("    ,OH.RFQCorres")
            Value.AppendLine("    ,RH.isCONFIDENTIAL")
            Value.AppendLine("    ,RH.ProductID")
            Value.AppendLine("    ,RH.ProductNumber")
            Value.AppendLine("    ,RH.ProductName")
            Value.AppendLine("    ,RH.Purpose")
            Value.AppendLine("    ,RH.QuoUserName")
            Value.AppendLine("    ,RH.QuoLocationName")
            Value.AppendLine("    ,RH.EnqUserName")
            Value.AppendLine("    ,RH.EnqLocationName")
            Value.AppendLine("    ,RH.SupplierCode")
            Value.AppendLine("    ,RH.SupplierName")
            Value.AppendLine("    ,RH.MakerName")
            Value.AppendLine("FROM")
            Value.AppendLine("    OldestHistory AS OH")
            Value.AppendLine("    ,v_RFQHeader AS RH")
            Value.AppendLine("WHERE")
            Value.AppendLine("    RH.RFQNumber = OH.RFQNumber")
            Value.AppendLine("ORDER BY")
            Value.AppendLine("    OH.RFQHistoryNumber")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                DBConn.Open()

                Using DBCommand As SqlCommand = DBConn.CreateCommand
                    DBCommand.CommandText = Value.ToString
                    DBCommand.Parameters.Add("UserID", SqlDbType.Int)
                    DBCommand.Parameters("UserID").Value = UserID

                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader
                        While DBReader.Read
                            Dim ccr As New CCReminderDisp
                            SetProperty(DBReader("RFQNumber"), ccr.RFQNumber)
                            SetProperty(DBReader("Priority"), ccr.Priority)
                            SetProperty(DBReader("CreateDate"), ccr.CreateDate)
                            SetProperty(DBReader("StatusChangeDate"), ccr.StatusChangeDate)
                            SetProperty(DBReader("StatusCode"), ccr.StatusCode)
                            SetProperty(DBReader("Status"), ccr.Status)
                            SetProperty(DBReader("RFQCorres"), ccr.RFQCorres)
                            SetProperty(DBReader("isCONFIDENTIAL"), ccr.isCONFIDENTIAL)
                            SetProperty(DBReader("ProductID"), ccr.ProductID)
                            SetProperty(DBReader("ProductNumber"), ccr.ProductNumber)
                            SetProperty(DBReader("ProductName"), ccr.ProductName)
                            SetProperty(DBReader("Purpose"), ccr.Purpose)
                            SetProperty(DBReader("QuoUserName"), ccr.QuoUserName)
                            SetProperty(DBReader("QuoLocationName"), ccr.QuoLocationName)
                            SetProperty(DBReader("EnqUserName"), ccr.EnqUserName)
                            SetProperty(DBReader("EnqLocationName"), ccr.EnqLocationName)
                            SetProperty(DBReader("SupplierCode"), ccr.SupplierCode)
                            SetProperty(DBReader("SupplierName"), ccr.SupplierName)
                            SetProperty(DBReader("MakerName"), ccr.MakerName)
                            Me.Add(ccr)
                        End While
                    End Using
                End Using
            End Using

        End Sub

    End Class

End Namespace
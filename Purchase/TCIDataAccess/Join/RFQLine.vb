Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess.Join

    ''' <summary> 
    ''' RFQLine データクラス 
    ''' </summary> 
    ''' <remarks>
    ''' <para>Generated by データクラス自動生成ツール Ver 4.1</para>
    ''' </remarks>
    Public Class RFQLine


#Region "v_UserAll-Defined Constant"

#End Region 'v_UserAll-Defined Constant End

        Protected _RFQNumber As Integer = 0
        Protected _RFQLineNumber As Integer = 0
        Protected _EnqQuantity As Decimal = 0
        Protected _EnqUnitCode As String = String.Empty
        Protected _EnqPiece As Integer = 0
        Protected _CurrencyCode As String = String.Empty
        Protected _UnitPrice As Decimal? = Nothing
        Protected _QuoPer As Decimal? = Nothing
        Protected _QuoUnitCode As String = String.Empty
        Protected _LeadTime As String = String.Empty
        Protected _Packing As String = String.Empty
        Protected _Purity As String = String.Empty
        Protected _QMMethod As String = String.Empty
        Protected _SupplierOfferNo As String = String.Empty
        Protected _NoOfferReason As String = String.Empty
        Protected _PO As Integer? = Nothing
        Protected _Priority As String = String.Empty
        Protected _OutputStatusInterface As String = String.Empty
        Protected _SupplierItemNumber As String = String.Empty

        ''' <summary> 
        ''' RFQNumber を設定、または取得する 
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
        ''' RFQLineNumber を設定、または取得する 
        ''' </summary> 
        Public Property RFQLineNumber() As Integer
            Get
                Return _RFQLineNumber
            End Get
            Set(ByVal value As Integer)
                _RFQLineNumber = value
            End Set
        End Property
        ''' <summary> 
        ''' EnqQuantity を設定、または取得する 
        ''' </summary> 
        Public Property EnqQuantity() As Decimal
            Get
                Return _EnqQuantity
            End Get
            Set(ByVal value As Decimal)
                _EnqQuantity = value
            End Set
        End Property

        ''' <summary> 
        ''' EnqUnitCode を設定、または取得する 
        ''' </summary> 
        Public Property EnqUnitCode() As String
            Get
                Return _EnqUnitCode
            End Get
            Set(ByVal value As String)
                _EnqUnitCode = value
            End Set
        End Property

        ''' <summary> 
        ''' EnqPiece を設定、または取得する 
        ''' </summary> 
        Public Property EnqPiece() As Integer
            Get
                Return _EnqPiece
            End Get
            Set(ByVal value As Integer)
                _EnqPiece = value
            End Set
        End Property

        ''' <summary> 
        ''' CurrencyCode を設定、または取得する 
        ''' </summary> 
        Public Property CurrencyCode() As String
            Get
                Return _CurrencyCode
            End Get
            Set(ByVal value As String)
                _CurrencyCode = value
            End Set
        End Property

        ''' <summary> 
        ''' UnitPrice を設定、または取得する 
        ''' <para>
        ''' ※ Decimal 変数へ格納する場合は以下のようにすること。
        '''     Dim val As Decimal = IIf(UnitPrice.HasValue, UnitPrice, 0)
        '''     Dim val As Decimal = UnitPrice.GetValueOrDefault() 
        ''' </para>
        ''' </summary> 
        Public Property UnitPrice() As Decimal?
            Get
                Return _UnitPrice
            End Get
            Set(ByVal value As Decimal?)
                _UnitPrice = value
            End Set
        End Property

        ''' <summary> 
        ''' QuoPer を設定、または取得する 
        ''' <para>
        ''' ※ Decimal 変数へ格納する場合は以下のようにすること。
        '''     Dim val As Decimal = IIf(QuoPer.HasValue, QuoPer, 0)
        '''     Dim val As Decimal = QuoPer.GetValueOrDefault() 
        ''' </para>
        ''' </summary> 
        Public Property QuoPer() As Decimal?
            Get
                Return _QuoPer
            End Get
            Set(ByVal value As Decimal?)
                _QuoPer = value
            End Set
        End Property

        ''' <summary> 
        ''' QuoUnitCode を設定、または取得する 
        ''' </summary> 
        Public Property QuoUnitCode() As String
            Get
                Return _QuoUnitCode
            End Get
            Set(ByVal value As String)
                _QuoUnitCode = value
            End Set
        End Property

        ''' <summary> 
        ''' LeadTime を設定、または取得する 
        ''' </summary> 
        Public Property LeadTime() As String
            Get
                Return _LeadTime
            End Get
            Set(ByVal value As String)
                _LeadTime = value
            End Set
        End Property
        ''' <summary> 
        ''' Packing を設定、または取得する 
        ''' </summary> 
        Public Property Packing() As String
            Get
                Return _Packing
            End Get
            Set(ByVal value As String)
                _Packing = value
            End Set
        End Property
        ''' <summary> 
        ''' Purity を設定、または取得する 
        ''' </summary> 
        Public Property Purity() As String
            Get
                Return _Purity
            End Get
            Set(ByVal value As String)
                _Purity = value
            End Set
        End Property
        ''' <summary> 
        ''' QMMethod を設定、または取得する 
        ''' </summary> 
        Public Property QMMethod() As String
            Get
                Return _QMMethod
            End Get
            Set(ByVal value As String)
                _QMMethod = value
            End Set
        End Property
        ''' <summary> 
        ''' SupplierOfferNo を設定、または取得する 
        ''' </summary> 
        Public Property SupplierOfferNo() As String
            Get
                Return _SupplierOfferNo
            End Get
            Set(ByVal value As String)
                _SupplierOfferNo = value
            End Set
        End Property
        ''' <summary> 
        ''' NoOfferReasonCode を設定、または取得する 
        ''' </summary> 
        Public Property NoOfferReason() As String
            Get
                Return _NoOfferReason
            End Get
            Set(ByVal value As String)
                _NoOfferReason = value
            End Set
        End Property
        ''' <summary> 
        ''' PO を設定、または取得する 
        ''' </summary> 
        Public Property PO() As Integer?
            Get
                Return _PO
            End Get
            Set(ByVal value As Integer?)
                _PO = value
            End Set
        End Property
        ''' <summary> 
        ''' Priority を設定、または取得する 
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
        ''' OutputStatusInterface を設定、または取得する 
        ''' </summary> 
        Public Property OutputStatusInterface() As String
            Get
                Return _OutputStatusInterface
            End Get
            Set(ByVal value As String)
                _OutputStatusInterface = value
            End Set
        End Property
        ''' <summary> 
        ''' SupplierItemNumber を設定、または取得する 
        ''' </summary> 
        Public Property SupplierItemNumber() As String
            Get
                Return _SupplierItemNumber
            End Get
            Set(ByVal value As String)
                _SupplierItemNumber = value
            End Set
        End Property


        ''' <summary> 
        ''' コンストラクタ
        ''' </summary> 
        Public Sub New()

        End Sub



#Region "v_UserAll-Defined Methods"

       
#End Region 'v_UserAll-Defined Methods End

    End Class

    ''' <summary> 
    ''' RFQLine リストクラス 
    ''' </summary> 
    Public Class RFQLineList
        Inherits List(Of RFQLine)

#Region "v_UserAll-Defined Constant of List"

#End Region 'v_UserAll-Defined Constant of List End

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

#Region "v_UserAll-Defined Methods of List"
                ''' <summary>
        ''' データベースからデータを読み込む。
        ''' </summary>
        ''' <param name="RFQLineNumber">RFQLineNumber</param>
        Public Sub Load(ByVal RFQLineNumber As String)

            'データベースからデータを読み込む SQL 文字列を生成する。
            Dim Value As New StringBuilder()
            Value.AppendLine("SELECT")
            Value.AppendLine("    DISTINCT")
            Value.AppendLine("    RFQL.[RFQNumber],")
            Value.AppendLine("    RFQL.[RFQLineNumber],")
            Value.AppendLine("    RFQL.[EnqQuantity],")
            Value.AppendLine("    RFQL.[EnqUnitCode],")
            Value.AppendLine("    RFQL.[EnqPiece],")
            Value.AppendLine("    RFQL.[CurrencyCode],")
            Value.AppendLine("    RFQL.[UnitPrice],")
            Value.AppendLine("    RFQL.[QuoPer],")
            Value.AppendLine("    RFQL.[QuoUnitCode],")
            Value.AppendLine("    RFQL.[LeadTime],")
            Value.AppendLine("    RFQL.[Packing],")
            Value.AppendLine("    RFQL.[Purity],")
            Value.AppendLine("    RFQL.[QMMethod],")
            Value.AppendLine("    RFQL.[SupplierOfferNo],")
            Value.AppendLine("    RFQL.[SupplierItemNumber],")
            Value.AppendLine("    RFQL.[NoOfferReason],")
            Value.AppendLine("    POPriority.[RFQLineNumber] AS PO,")
            Value.AppendLine("    CASE ")
            Value.AppendLine("        WHEN POPriority.[Priority] = 'C' THEN '' ")
            Value.AppendLine("        ELSE POPriority.[Priority] ")
            Value.AppendLine("    END AS Priority,")
            Value.AppendLine("    CASE ")
            Value.AppendLine("        WHEN RFQL.[OutputStatus] = 1 THEN ' Interface issued' ")
            Value.AppendLine("        ELSE '' ")
            Value.AppendLine("    END AS OutputStatusInterface")
            Value.AppendLine("FROM")
            Value.AppendLine("    [v_RFQLine] RFQL")
            Value.AppendLine("    LEFT OUTER JOIN(")
            Value.AppendLine("        SELECT")
            Value.AppendLine("            [RFQLineNumber],")
            Value.AppendLine("            MIN(")
            Value.AppendLine("                CASE")
            Value.AppendLine("                    WHEN PO.[QMStartingDate] IS NOT NULL OR PO.[QMFinishDate] IS NOT NULL THEN 'C'")
            Value.AppendLine("                    ELSE ISNULL(PO.Priority, 'C')")
            Value.AppendLine("                END")
            Value.AppendLine("            ) AS Priority")
            Value.AppendLine("        FROM [PO]")
            Value.AppendLine("        GROUP BY [RFQLineNumber]")
            Value.AppendLine("        ) POPriority")
            Value.AppendLine("    ON")
            Value.AppendLine("    POPriority.[RFQLineNumber] = RFQL.[RFQLineNumber]")
            Value.AppendLine("WHERE")
            Value.AppendLine("    [RFQNumber] = @RFQNumber")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = Value.ToString()
                    DBCommand.Parameters.AddWithValue("RFQNumber", RFQLineNumber)
                    DBConn.Open()
                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader()
                        While DBReader.Read()
                            Dim dc_Data As New RFQLine
                            SetProperty(DBReader("RFQNumber"), dc_Data.RFQNumber)
                            SetProperty(DBReader("RFQLineNumber"), dc_Data.RFQLineNumber)
                            SetProperty(DBReader("EnqQuantity"), dc_Data.EnqQuantity)
                            SetProperty(DBReader("EnqUnitCode"), dc_Data.EnqUnitCode)
                            SetProperty(DBReader("EnqPiece"), dc_Data.EnqPiece)
                            SetProperty(DBReader("CurrencyCode"), dc_Data.CurrencyCode)
                            SetProperty(DBReader("UnitPrice"), dc_Data.UnitPrice)
                            SetProperty(DBReader("QuoPer"), dc_Data.QuoPer)
                            SetProperty(DBReader("QuoUnitCode"), dc_Data.QuoUnitCode)
                            SetProperty(DBReader("LeadTime"), dc_Data.LeadTime)
                            SetProperty(DBReader("Packing"), dc_Data.Packing)
                            SetProperty(DBReader("Purity"), dc_Data.Purity)
                            SetProperty(DBReader("QMMethod"), dc_Data.QMMethod)
                            SetProperty(DBReader("SupplierOfferNo"), dc_Data.SupplierOfferNo)
                            SetProperty(DBReader("SupplierItemNumber"), dc_Data.SupplierItemNumber)
                            SetProperty(DBReader("NoOfferReason"), dc_Data.NoOfferReason)
                            SetProperty(DBReader("PO"), dc_Data.PO)
                            SetProperty(DBReader("Priority"), dc_Data.Priority)
                            SetProperty(DBReader("OutputStatusInterface"), dc_Data.OutputStatusInterface)

                            Me.Add(dc_Data)
                        End While
                    End Using
                End Using
            End Using

        End Sub

#End Region 'v_UserAll-Defined Methods of List End

    End Class

End Namespace

Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports System.Data.SqlClient   
Imports System.Text.RegularExpressions
Imports Purchase.Common
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess.Join

    ''' <summary>
    ''' ReportOverviewRFQSearch データクラス
    ''' </summary>
    Public Class ReportOverviewRFQSearch

        'RFQHeader
        Protected _RFQNumber As Integer = 0
        Protected _Priority As String = String.Empty
        Protected _StatusChangeDate As DateTime = New DateTime(0)
        Protected _Status As String = String.Empty
        Protected _ProductNumber As String = String.Empty
        Protected _CASNumber As String = String.Empty
        Protected _ProductName As String = String.Empty
        Protected _SupplierCode As Integer = 0
        Protected _S4SupplierCode As String = String.Empty
        Protected _SupplierName As String = String.Empty
        Protected _SupplierCountryName As String = String.Empty
        Protected _Purpose As String = String.Empty
        Protected _MakerCode As Integer? = Nothing
        Protected _S4MakerCode As String = String.Empty
        Protected _MakerName As String = String.Empty
        Protected _MakerCountryName As String = String.Empty
        Protected _SupplierItemName As String = String.Empty
        Protected _ShippingHandlingCurrencyCode As String = String.Empty
        Protected _ShippingHandlingFee As Decimal? = Nothing
        Protected _EnqUserName As String = String.Empty
        Protected _EnqLocationName As String = String.Empty
        Protected _EnqStorageLocation As String = String.Empty
        Protected _QuoUserName As String = String.Empty
        Protected _QuoLocationName As String = String.Empty
        Protected _QuoStorageLocation As String = String.Empty
        Protected _Comment As String = String.Empty
        'RFQLine
        Protected _LineNo As Long = 0
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
        Protected _SupplierOfferNo As String = String.Empty
        Protected _SupplierItemNumber As String = String.Empty
        Protected _NoOfferReason As String = String.Empty
        Protected _PO As String = String.Empty

        Protected _StatusChangeDateN As DateTime = New DateTime(0)
        Protected _StatusChangeDateA As DateTime = New DateTime(0)
        Protected _StatusChangeDateE As DateTime = New DateTime(0)
        Protected _StatusChangeDatePQ As DateTime = New DateTime(0)
        Protected _StatusChangeDateQ As DateTime = New DateTime(0)
        Protected _StatusChangeDateII As DateTime = New DateTime(0)
        Protected _StatusChangeDateV As DateTime = New DateTime(0)

        ''' <summary> 
        ''' RFQNumber  を設定、または取得する 
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
        ''' Priority  を設定、または取得する 
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
        ''' StatusChangeDate  を設定、または取得する 
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
        ''' Status  を設定、または取得する 
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
        ''' _ProductNumber  を設定、または取得する 
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
        ''' CASNumber  を設定、または取得する 
        ''' </summary> 
        Public Property CASNumber() As String
            Get
                Return _CASNumber
            End Get
            Set(ByVal value As String)
                _CASNumber = value
            End Set
        End Property
        ''' <summary> 
        ''' ProductName  を設定、または取得する 
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
        ''' SupplierCode  を設定、または取得する 
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
        ''' S4SupplierCode  を設定、または取得する 
        ''' </summary> 
        Public Property S4SupplierCode() As String
            Get
                Return _S4SupplierCode
            End Get
            Set(ByVal value As String)
                _S4SupplierCode = value
            End Set
        End Property
        ''' <summary> 
        ''' SupplierName  を設定、または取得する 
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
        ''' SupplierCountryName  を設定、または取得する 
        ''' </summary> 
        Public Property SupplierCountryName() As String
            Get
                Return _SupplierCountryName
            End Get
            Set(ByVal value As String)
                _SupplierCountryName = value
            End Set
        End Property
        ''' <summary> 
        ''' Purpose  を設定、または取得する 
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
        ''' MakerCode を設定、または取得する 
        ''' <para>
        ''' ※ Integer 変数へ格納する場合は以下のようにすること。
        '''     Dim val As Integer = IIf(MakerCode.HasValue, MakerCode, 0)
        '''     Dim val As Integer = MakerCode.GetValueOrDefault() 
        ''' </para>
        ''' </summary> 
        Public Property MakerCode() As Integer?
            Get
                Return _MakerCode
            End Get
            Set(ByVal value As Integer?)
                _MakerCode = value
            End Set
        End Property
        ''' <summary> 
        ''' S4MakerCode  を設定、または取得する 
        ''' </summary> 
        Public Property S4MakerCode() As String
            Get
                Return _S4MakerCode
            End Get
            Set(ByVal value As String)
                _S4MakerCode = value
            End Set
        End Property
        ''' <summary> 
        ''' MakerName  を設定、または取得する 
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
        ''' MakerCountryName  を設定、または取得する 
        ''' </summary> 
        Public Property MakerCountryName() As String
            Get
                Return _MakerCountryName
            End Get
            Set(ByVal value As String)
                _MakerCountryName = value
            End Set
        End Property
        ''' <summary> 
        ''' SupplierItemName  を設定、または取得する 
        ''' </summary> 
        Public Property SupplierItemName() As String
            Get
                Return _SupplierItemName
            End Get
            Set(ByVal value As String)
                _SupplierItemName = value
            End Set
        End Property
        ''' <summary> 
        ''' ShippingHandlingCurrencyCode  を設定、または取得する 
        ''' </summary> 
        Public Property ShippingHandlingCurrencyCode() As String
            Get
                Return _ShippingHandlingCurrencyCode
            End Get
            Set(ByVal value As String)
                _ShippingHandlingCurrencyCode = value
            End Set
        End Property
        ''' <summary> 
        ''' ShippingHandlingFee を設定、または取得する 
        ''' <para>
        ''' ※ Decimal 変数へ格納する場合は以下のようにすること。
        '''     Dim val As Decimal = IIf(ShippingHandlingFee.HasValue, ShippingHandlingFee, 0)
        '''     Dim val As Decimal = ShippingHandlingFee.GetValueOrDefault() 
        ''' </para>
        ''' </summary> 
        Public Property ShippingHandlingFee() As Decimal?
            Get
                Return _ShippingHandlingFee
            End Get
            Set(ByVal value As Decimal?)
                _ShippingHandlingFee = value
            End Set
        End Property
        ''' <summary> 
        ''' EnqUserName  を設定、または取得する 
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
        ''' EnqLocationName  を設定、または取得する 
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
        ''' EnqStorageLocation  を設定、または取得する 
        ''' </summary> 
        Public Property EnqStorageLocation() As String
            Get
                Return _EnqStorageLocation
            End Get
            Set(ByVal value As String)
                _EnqStorageLocation = value
            End Set
        End Property
        ''' <summary> 
        ''' QuoUserName  を設定、または取得する 
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
        ''' QuoLocationName  を設定、または取得する 
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
        ''' QuoStorageLocation  を設定、または取得する 
        ''' </summary> 
        Public Property QuoStorageLocation() As String
            Get
                Return _QuoStorageLocation
            End Get
            Set(ByVal value As String)
                _QuoStorageLocation = value
            End Set
        End Property
        ''' <summary> 
        ''' Comment  を設定、または取得する 
        ''' </summary> 
        Public Property Comment() As String
            Get
                Return _Comment
            End Get
            Set(ByVal value As String)
                _Comment = value
            End Set
        End Property
        'RFQLine
        ''' <summary> 
        ''' LineNo  を設定、または取得する 
        ''' </summary> 
        Public Property LineNo() As Long
            Get
                Return _LineNo
            End Get
            Set(ByVal value As Long)
                _LineNo = value
            End Set
        End Property
        ''' <summary> 
        ''' EnqQuantity  を設定、または取得する 
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
        ''' EnqUnitCode  を設定、または取得する 
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
        ''' EnqPiece  を設定、または取得する 
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
        ''' CurrencyCode  を設定、または取得する 
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
        Public Property UnitPrice()  As Decimal?
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
        ''' QuoUnitCode  を設定、または取得する 
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
        ''' LeadTime  を設定、または取得する 
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
        ''' Packing  を設定、または取得する 
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
        ''' Purity  を設定、または取得する 
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
        ''' SupplierOfferNo  を設定、または取得する 
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
        ''' NoOfferReason  を設定、または取得する 
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
        ''' PO  を設定、または取得する 
        ''' </summary> 
        Public Property PO() As String
            Get
                Return _PO
            End Get
            Set(ByVal value As String)
                _PO = value
            End Set
        End Property
        ''' <summary> 
        ''' StatusChangeDateN  を設定、または取得する 
        ''' </summary> 
        Public Property StatusChangeDateN() As DateTime
            Get
                Return _StatusChangeDateN
            End Get
            Set(ByVal value As DateTime)
                _StatusChangeDateN = value
            End Set
        End Property
        ''' <summary> 
        ''' StatusChangeDateA  を設定、または取得する 
        ''' </summary> 
        Public Property StatusChangeDateA() As DateTime
            Get
                Return _StatusChangeDateA
            End Get
            Set(ByVal value As DateTime)
                _StatusChangeDateA = value
            End Set
        End Property
        ''' <summary> 
        ''' StatusChangeDateE  を設定、または取得する 
        ''' </summary> 
        Public Property StatusChangeDateE() As DateTime
            Get
                Return _StatusChangeDateE
            End Get
            Set(ByVal value As DateTime)
                _StatusChangeDateE = value
            End Set
        End Property
        ''' <summary> 
        ''' StatusChangeDatePQ  を設定、または取得する 
        ''' </summary> 
        Public Property StatusChangeDatePQ() As DateTime
            Get
                Return _StatusChangeDatePQ
            End Get
            Set(ByVal value As DateTime)
                _StatusChangeDatePQ = value
            End Set
        End Property
        ''' <summary> 
        ''' StatusChangeDateQ  を設定、または取得する 
        ''' </summary> 
        Public Property StatusChangeDateQ() As DateTime
            Get
                Return _StatusChangeDateQ
            End Get
            Set(ByVal value As DateTime)
                _StatusChangeDateQ = value
            End Set
        End Property
        ''' <summary> 
        ''' StatusChangeDateII  を設定、または取得する 
        ''' </summary> 
        Public Property StatusChangeDateII() As DateTime
            Get
                Return _StatusChangeDateII
            End Get
            Set(ByVal value As DateTime)
                _StatusChangeDateII = value
            End Set
        End Property
        ''' <summary> 
        ''' StatusChangeDateV  を設定、または取得する 
        ''' </summary> 
        Public Property StatusChangeDateV() As DateTime
            Get
                Return _StatusChangeDateV
            End Get
            Set(ByVal value As DateTime)
                _StatusChangeDateV = value
            End Set
        End Property

        ''' <summary> 
        ''' コンストラクタ
        ''' </summary> 
        Public Sub New()

        End Sub

    End Class

    ''' <summary>
    ''' ReportOverviewRFQSearchList データクラス
    ''' </summary>
    Public Class ReportOverviewRFQSearchList
        Inherits List(Of ReportOverviewRFQSearch)
        ''' <summary> SessionのLocationCode </summary>
        Private _s_LocationCode As String = String.Empty
        ''' <summary> 
        ''' s_LocationCode  を設定、または取得する 
        ''' </summary> 
        Public Property s_LocationCode() As String
            Get
                Return _s_LocationCode
            End Get
            Set(ByVal value As String)
                _s_LocationCode = value
            End Set
        End Property
        ''' <summary> 
        ''' コンストラクタ
        ''' </summary> 
        Public Sub New()

        End Sub

        ''' <summary>
        ''' 指定された条件の新製品データを提案元/指令・発注先ごとに、
        ''' 各ステータスごとの件数を集計したデータを読み込む
        ''' </summary>
        ''' <param name="Cond">検索条件</param>
        ''' <remarks></remarks>
        Public Sub Load(ByVal cond As TCIDataAccess.join.KeywordSearchConditionParameter)
            Dim Value As New StringBuilder()
            ' セッションのロケーションコードを設定
            s_LocationCode = cond.s_LocationCode

            '画面で入力された値のWhere句の生成
            Dim WhereClause As String = RFQHeaderList.CreateRFQHeaderWhereClauseSQL(cond)

            'SQL文字列の作成
            Value.AppendLine("SELECT")
            ' RFQヘッダ部 
            Value.AppendLine("    rfh.[RFQNumber],")
            Value.AppendLine("    ISNULL(rfh.[Priority], '') AS Priority,")
            Value.AppendLine("    rfh.[Status],")
            Value.AppendLine("    rfh.[StatusChangeDate],")
            Value.AppendLine("    rfh.[ProductNumber],")
            Value.AppendLine("    rfh.[CASNumber],")
            Value.AppendLine("    rfh.[ProductName],")
            Value.AppendLine("    rfh.[SupplierCode],")
            Value.AppendLine("    rfh.[S4SupplierCode],")
            Value.AppendLine("    rfh.[SupplierName],")
            Value.AppendLine("    scry.[Name] SupplierCountryName,")
            Value.AppendLine("    rfh.[Purpose],")
            Value.AppendLine("    rfh.[MakerCode],")
            Value.AppendLine("    rfh.[S4MakerCode],")
            Value.AppendLine("    rfh.[MakerName],")
            Value.AppendLine("    mcry.[Name] MakerCountryName,")
            Value.AppendLine("    rfh.[SupplierItemName],")
            Value.AppendLine("    rfh.[ShippingHandlingCurrencyCode],")
            Value.AppendLine("    rfh.[ShippingHandlingFee],")
            Value.AppendLine("    rfh.[EnqUserName],")
            Value.AppendLine("    rfh.[EnqLocationName],")
            Value.AppendLine("    rfh.[EnqStorageLocation],")
            Value.AppendLine("    rfh.[QuoUserName],")
            Value.AppendLine("    rfh.[QuoLocationName],")
            Value.AppendLine("    rfh.[QuoStorageLocation],")
            Value.AppendLine("    rfh.[Comment],")
            ' RFQ明細部
            Value.AppendLine("    ROW_NUMBER() OVER(PARTITION BY rfh.[RFQNumber] ORDER BY vRL.[RFQLineNumber] ASC) As 'LineNo',")
            Value.AppendLine("    vRL.[EnqQuantity],")
            Value.AppendLine("    vRL.[EnqUnitCode],")
            Value.AppendLine("    vRL.[EnqPiece],")
            Value.AppendLine("    vRL.[CurrencyCode],")
            Value.AppendLine("    vRL.[UnitPrice],")
            Value.AppendLine("    vRL.[QuoPer],")
            Value.AppendLine("    vRL.[QuoUnitCode],")
            Value.AppendLine("    vRL.[LeadTime],")
            Value.AppendLine("    vRL.[Packing],")
            Value.AppendLine("    vRL.[Purity],")
            Value.AppendLine("    vRL.[SupplierOfferNo],")
            Value.AppendLine("    vRL.[SupplierItemNumber],")
            Value.AppendLine("    vRL.[NoOfferReason],")
            Value.AppendLine("    PO.RFQLineNumber AS PO,")
            Value.AppendLine("    RHstryN.StatusChangeDate AS StatusChangeDateN,")
            Value.AppendLine("    RHstryA.StatusChangeDate AS StatusChangeDateA,")
            Value.AppendLine("    RHstryE.StatusChangeDate AS StatusChangeDateE,")
            Value.AppendLine("    RHstryPQ.StatusChangeDate AS StatusChangeDatePQ,")
            Value.AppendLine("    RHstryQ.StatusChangeDate AS StatusChangeDateQ,")
            Value.AppendLine("    RHstryII.StatusChangeDate AS StatusChangeDateII,")
            Value.AppendLine("    RHstryC.StatusChangeDate AS StatusChangeDateV")
            Value.AppendLine("FROM")
            Value.AppendLine("    [v_RFQHeader] rfh")
            Value.AppendLine("    LEFT JOIN")
            Value.AppendLine("        [s_Country] mcry")
            Value.AppendLine("        ON")
            Value.AppendLine("        rfh.[MakerCountryCode] = mcry.[CountryCode]")
            Value.AppendLine("    LEFT JOIN")
            Value.AppendLine("        s_Country scry")
            Value.AppendLine("        ON")
            Value.AppendLine("        rfh.SupplierCountryCode = scry.CountryCode")
            If Cond.ValidQuotation = "Valid Price" Then

                Value.AppendLine("    INNER JOIN ")
                Value.AppendLine("        (SELECT")
                Value.AppendLine("            [ProductID],")
                Value.AppendLine("            [UnitPrice],")
                Value.AppendLine("            [RFQNumber],")
                Value.AppendLine("        FROM")
                Value.AppendLine("            [v_RFQLine]")
                Value.AppendLine("        GROUP BY ")
                Value.AppendLine("            [ProductID],")
                Value.AppendLine("            [UnitPrice],")
                Value.AppendLine("            [RFQNumber]")
                Value.AppendLine("        HAVING")
                Value.AppendLine("            SUM([UnitPrice]) >= 0")
                Value.AppendLine("        ) vRL")
                Value.AppendLine("        ON")
                Value.AppendLine("        rfh.[RFQNumber] =  vRL.[RFQNumber]")
            End If
            If Cond.ValidQuotation = "Inalid Price" Then
                Value.AppendLine("    INNER JOIN ")
                Value.AppendLine("        (SELECT")
                Value.AppendLine("            [ProductID],")
                Value.AppendLine("            [UnitPrice],")
                Value.AppendLine("            [RFQNumber],")
                Value.AppendLine("        FROM")
                Value.AppendLine("            [v_RFQLine]")
                Value.AppendLine("        WHERE")
                Value.AppendLine("            [UnitPrice] IS NULL")
                Value.AppendLine("        GROUP BY")
                Value.AppendLine("            [ProductID],")
                Value.AppendLine("            [UnitPrice],")
                Value.AppendLine("            [RFQNumber]")
                Value.AppendLine("        ) vRL")
                Value.AppendLine("        ON")
                Value.AppendLine("        rfh.[RFQNumber] =  vRL.[RFQNumber]")
            End If
            If DBCommon.isTerritoryCheckd(Cond.Territory) Then
                Value.AppendLine("    LEFT JOIN")
                Value.AppendLine("        [v_Territory] vTertry")
                Value.AppendLine("        ON ")
                Value.AppendLine("        rfh.[SupplierCode] = vTertry.[SupplierCode]")
            End If
            Value.AppendLine("    LEFT JOIN")
            Value.AppendLine("        [v_RFQLine] vRL")
            Value.AppendLine("        ON")
            Value.AppendLine("        rfh.[RFQNumber] =  vRL.[RFQNumber]")
            Value.AppendLine("    LEFT OUTER JOIN")
            Value.AppendLine("        (SELECT")
            Value.AppendLine("            [RFQLineNumber],")
            Value.AppendLine("            MIN(")
            Value.AppendLine("                CASE WHEN")
            Value.AppendLine("                    PO.[QMStartingDate] IS NOT NULL")
            Value.AppendLine("                    OR")
            Value.AppendLine("                    PO.[QMFinishDate] IS NOT NULL")
            Value.AppendLine("                    THEN 'C'")
            Value.AppendLine("                ELSE")
            Value.AppendLine("                    ISNULL(PO.[Priority], 'C')")
            Value.AppendLine("                END")
            Value.AppendLine("            ) AS Priority")
            Value.AppendLine("        FROM PO")
            Value.AppendLine("        GROUP BY")
            Value.AppendLine("            [RFQLineNumber]")
            Value.AppendLine("        ) PO")
            Value.AppendLine("        ON")
            Value.AppendLine("            PO.[RFQLineNumber] = vRL.[RFQLineNumber]")
            ' Statusが ‘Create’ になった最新の日付
            Value.AppendLine("    LEFT OUTER JOIN")
            Value.AppendLine("        (SELECT")
            Value.AppendLine("            RHstry.RFQNumber,")
            Value.AppendLine("            MAX(RHstry.StatusChangeDate) StatusChangeDate")
            Value.AppendLine("        FROM")
            Value.AppendLine("            RFQHistory RHstry")
            Value.AppendLine("        WHERE")
            Value.AppendLine("            RHstry.RFQStatusCode = 'N'")
            Value.AppendLine("        GROUP BY RHstry.RFQNumber")
            Value.AppendLine("        ) RHstryN")
            Value.AppendLine("        ON")
            Value.AppendLine("        rfh.RFQNumber = RHstryN.RFQNumber")
            ' Statusが ‘Assigned’ になった最新の日付
            Value.AppendLine("    LEFT OUTER JOIN")
            Value.AppendLine("        (SELECT")
            Value.AppendLine("            RHstry.RFQNumber,")
            Value.AppendLine("            MAX(RHstry.StatusChangeDate) StatusChangeDate")
            Value.AppendLine("        FROM")
            Value.AppendLine("            RFQHistory RHstry")
            Value.AppendLine("        WHERE")
            Value.AppendLine("            RHstry.RFQStatusCode = 'A'")
            Value.AppendLine("        GROUP BY RHstry.RFQNumber")
            Value.AppendLine("        ) RHstryA")
            Value.AppendLine("        ON")
            Value.AppendLine("        rfh.RFQNumber = RHstryA.RFQNumber")
            ' Statusが ‘Enquired’ になった最新の日付
            Value.AppendLine("    LEFT OUTER JOIN")
            Value.AppendLine("        (SELECT")
            Value.AppendLine("            RHstry.RFQNumber,")
            Value.AppendLine("            MAX(RHstry.StatusChangeDate) StatusChangeDate")
            Value.AppendLine("        FROM")
            Value.AppendLine("            RFQHistory RHstry")
            Value.AppendLine("        WHERE")
            Value.AppendLine("            RHstry.RFQStatusCode = 'E'")
            Value.AppendLine("        GROUP BY RHstry.RFQNumber")
            Value.AppendLine("        ) RHstryE")
            Value.AppendLine("        ON")
            Value.AppendLine("        rfh.RFQNumber = RHstryE.RFQNumber")
            ' Statusが ‘Partly-Quoted’ になった最新の日付
            Value.AppendLine("    LEFT OUTER JOIN")
            Value.AppendLine("        (SELECT")
            Value.AppendLine("            RHstry.RFQNumber,")
            Value.AppendLine("            MAX(RHstry.StatusChangeDate) StatusChangeDate")
            Value.AppendLine("        FROM")
            Value.AppendLine("            RFQHistory RHstry")
            Value.AppendLine("        WHERE")
            Value.AppendLine("            RHstry.RFQStatusCode = 'PQ'")
            Value.AppendLine("        GROUP BY RHstry.RFQNumber")
            Value.AppendLine("        ) RHstryPQ")
            Value.AppendLine("        ON")
            Value.AppendLine("        rfh.RFQNumber = RHstryPQ.RFQNumber")
            ' Statusが ‘Quoted’ になった最新の日付
            Value.AppendLine("    LEFT OUTER JOIN")
            Value.AppendLine("        (SELECT")
            Value.AppendLine("            RHstry.RFQNumber,")
            Value.AppendLine("            MAX(RHstry.StatusChangeDate) StatusChangeDate")
            Value.AppendLine("        FROM")
            Value.AppendLine("            RFQHistory RHstry")
            Value.AppendLine("        WHERE")
            Value.AppendLine("            RHstry.RFQStatusCode = 'Q'")
            Value.AppendLine("        GROUP BY RHstry.RFQNumber")
            Value.AppendLine("        ) RHstryQ")
            Value.AppendLine("        ON")
            Value.AppendLine("        rfh.RFQNumber = RHstryQ.RFQNumber")
            ' Statusが ‘Interface Issued’ になった最新の日付
            Value.AppendLine("    LEFT OUTER JOIN")
            Value.AppendLine("        (SELECT")
            Value.AppendLine("            RHstry.RFQNumber,")
            Value.AppendLine("            MAX(RHstry.StatusChangeDate) StatusChangeDate")
            Value.AppendLine("        FROM")
            Value.AppendLine("            RFQHistory RHstry")
            Value.AppendLine("        WHERE")
            Value.AppendLine("            RHstry.RFQStatusCode = 'II'")
            Value.AppendLine("        GROUP BY RHstry.RFQNumber")
            Value.AppendLine("        ) RHstryII")
            Value.AppendLine("        ON")
            Value.AppendLine("        rfh.RFQNumber = RHstryII.RFQNumber")
            ' Statusが ‘Closed’ になった最新の日付
            Value.AppendLine("    LEFT OUTER JOIN")
            Value.AppendLine("        (SELECT")
            Value.AppendLine("            RHstry.RFQNumber,")
            Value.AppendLine("            MAX(RHstry.StatusChangeDate) StatusChangeDate")
            Value.AppendLine("        FROM")
            Value.AppendLine("            RFQHistory RHstry")
            Value.AppendLine("        WHERE")
            Value.AppendLine("            RHstry.RFQStatusCode = 'C'")
            Value.AppendLine("        GROUP BY RHstry.RFQNumber")
            Value.AppendLine("        ) RHstryC")
            Value.AppendLine("        ON")
            Value.AppendLine("        rfh.RFQNumber = RHstryC.RFQNumber")
            
            Value.AppendLine("WHERE ")
            Value.AppendLine(WhereClause)
            Value.AppendLine("ORDER BY")
            Value.AppendLine("    rfh.StatusSortOrder ASC,")
            Value.AppendLine("    rfh.QuotedDate DESC,")
            Value.AppendLine("    rfh.StatusChangeDate DESC,")
            Value.AppendLine("    rfh.RFQNumber ASC")

            Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = Value.ToString
                    DBCommand.Parameters.Clear()
                    '生成したSQL文に入力項目を反映
                    SetParamInClauseSQL(DBCommand, "RFQNumber", Cond.RFQNumber)
                    SetParamInClauseSQL(DBCommand, "ProductNumber", Cond.ProductNumber)
                    If Not String.IsNullOrEmpty(Cond.ProductName) Then
                        DBCommand.Parameters.AddWithValue("ProductName", StrConv(Cond.ProductName, VbStrConv.Narrow))
                    End If
                    SetParamInClauseSQL(DBCommand, "SupplierCode", Cond.SupplierCode)
                    SetParamInClauseSQL(DBCommand, "S4SupplierCode", Cond.S4SupplierCode)
                    If Not String.IsNullOrEmpty(Cond.SupplierName) Then
                        DBCommand.Parameters.AddWithValue("SupplierName", StrConv(Cond.SupplierName, VbStrConv.Narrow))
                    End If
                    If Not String.IsNullOrEmpty(Cond.SupplierItemName) Then
                        DBCommand.Parameters.AddWithValue("SupplierItemName", StrConv(Cond.SupplierItemName, VbStrConv.Narrow))
                    End If
                    If Not String.IsNullOrEmpty(Cond.SupplierCountryCode) Then
                        DBCommand.Parameters.AddWithValue("SupplierCountryCode", Cond.SupplierCountryCode)
                    End If
                    If Not String.IsNullOrEmpty(Cond.StatusFrom) Then
                        DBCommand.Parameters.AddWithValue("StatusFrom", Cond.StatusFrom)
                    End If
                    If Not String.IsNullOrEmpty(Cond.StatusTo) Then
                        DBCommand.Parameters.AddWithValue("StatusTo", Cond.StatusTo)
                    End If
                    If Not String.IsNullOrEmpty(Cond.RFQCreatedDateFrom) Then
                        DBCommand.Parameters.AddWithValue("RFQCreatedDateFrom", GetDatabaseTime(Cond.s_LocationCode, Cond.RFQCreatedDateFrom))
                    End If
                    If Not String.IsNullOrEmpty(Cond.RFQCreatedDateTo) Then
                        DBCommand.Parameters.AddWithValue("RFQCreatedDateTo", GetDatabaseTime(Cond.s_LocationCode, Cond.RFQCreatedDateTo))
                    End If
                    If Not String.IsNullOrEmpty(Cond.RFQQuotedDateFrom) Then
                        DBCommand.Parameters.AddWithValue("RFQQuotedDateFrom", GetDatabaseTime(Cond.s_LocationCode, Cond.RFQQuotedDateFrom))
                    End If
                    If Not String.IsNullOrEmpty(Cond.RFQQuotedDateTo) Then
                        DBCommand.Parameters.AddWithValue("RFQQuotedDateTo", GetDatabaseTime(Cond.s_LocationCode, Cond.RFQQuotedDateTo))
                    End If
                    If Not String.IsNullOrEmpty(Cond.LastRFQStatusChangeDateFrom) Then
                        DBCommand.Parameters.AddWithValue("LastRFQStatusChangeDateFrom", GetDatabaseTime(Cond.s_LocationCode, Cond.LastRFQStatusChangeDateFrom))
                    End If
                    If Not String.IsNullOrEmpty(Cond.LastRFQStatusChangeDateTo) Then
                        DBCommand.Parameters.AddWithValue("LastRFQStatusChangeDateTo", GetDatabaseTime(Cond.s_LocationCode, Cond.LastRFQStatusChangeDateTo))
                    End If
                    If Not String.IsNullOrEmpty(Cond.EnqLocationCode) Then
                        DBCommand.Parameters.AddWithValue("EnqLocationCode", Cond.EnqLocationCode)
                    End If
                    If Not String.IsNullOrEmpty(Cond.EnqUserID) Then
                        DBCommand.Parameters.AddWithValue("EnqUserID", Cond.EnqUserID)
                    End If
                    If Not String.IsNullOrEmpty(Cond.EnqStorageLocation) Then
                        DBCommand.Parameters.AddWithValue("EnqStorageLocation", Cond.EnqStorageLocation)
                    End If
                    If Not String.IsNullOrEmpty(Cond.QuoLocationCode) Then
                        DBCommand.Parameters.AddWithValue("QuoLocationCode", Cond.QuoLocationCode)
                    End If
                    If Not String.IsNullOrEmpty(Cond.QuoUserID) Then
                        DBCommand.Parameters.AddWithValue("QuoUserID", Cond.QuoUserID)
                    End If
                    If Not String.IsNullOrEmpty(Cond.QuoStorageLocation) Then
                        DBCommand.Parameters.AddWithValue("QuoStorageLocation", Cond.QuoStorageLocation)
                    End If
                    SetPramMultipleSelectionInClauseSQL(DBCommand, "Purpose", Cond.Purpose)
                    SetPramMultipleSelectionInClauseSQL(DBCommand, "EnqLocationName", Cond.Territory)
                    SetPramMultipleSelectionInClauseSQL(DBCommand, "QuoLocationName", Cond.Territory)
                    If Not String.IsNullOrEmpty(Cond.Priority) Then
                        DBCommand.Parameters.AddWithValue("Priority", Cond.Priority)
                    End If

                    DBConn.Open()
                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader()
                        While DBReader.Read()
                            Dim dc_Data As New ReportOverviewRFQSearch
                            'RFQHeader
                            SetProperty(DBReader("RFQNumber"), dc_Data.RFQNumber)
                            SetProperty(DBReader("Priority"), dc_Data.Priority)
                            SetProperty(DBReader("Status"), dc_Data.Status)
                            SetProperty(DBReader("StatusChangeDate"), dc_Data.StatusChangeDate)
                            SetProperty(DBReader("ProductNumber"), dc_Data.ProductNumber)
                            SetProperty(DBReader("CASNumber"), dc_Data.CASNumber)
                            SetProperty(DBReader("ProductName"), dc_Data.ProductName)
                            SetProperty(DBReader("SupplierCode"), dc_Data.SupplierCode)
                            SetProperty(DBReader("S4SupplierCode"), dc_Data.S4SupplierCode)
                            SetProperty(DBReader("SupplierName"), dc_Data.SupplierName)
                            SetProperty(DBReader("SupplierCountryName"), dc_Data.SupplierCountryName)
                            SetProperty(DBReader("Purpose"), dc_Data.Purpose)
                            SetProperty(DBReader("MakerCode"), dc_Data.MakerCode)
                            SetProperty(DBReader("S4MakerCode"), dc_Data.S4MakerCode)
                            SetProperty(DBReader("MakerName"), dc_Data.MakerName)
                            SetProperty(DBReader("MakerCountryName"), dc_Data.MakerCountryName)
                            SetProperty(DBReader("SupplierItemName"), dc_Data.SupplierItemName)
                            SetProperty(DBReader("ShippingHandlingCurrencyCode"), dc_Data.ShippingHandlingCurrencyCode)
                            SetProperty(DBReader("ShippingHandlingFee"), dc_Data.ShippingHandlingFee)
                            SetProperty(DBReader("EnqUserName"), dc_Data.EnqUserName)
                            SetProperty(DBReader("EnqLocationName"), dc_Data.EnqLocationName)
                            SetProperty(DBReader("EnqStorageLocation"), dc_Data.EnqStorageLocation)
                            SetProperty(DBReader("QuoUserName"), dc_Data.QuoUserName)
                            SetProperty(DBReader("QuoLocationName"), dc_Data.QuoLocationName)
                            SetProperty(DBReader("QuoStorageLocation"), dc_Data.QuoStorageLocation)
                            SetProperty(DBReader("Comment"), dc_Data.Comment)
                            'RFQLine
                            SetProperty(DBReader("LineNo"), dc_Data.LineNo)
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
                            SetProperty(DBReader("SupplierOfferNo"), dc_Data.SupplierOfferNo)
                            SetProperty(DBReader("SupplierItemNumber"), dc_Data.SupplierItemNumber)
                            SetProperty(DBReader("NoOfferReason"), dc_Data.NoOfferReason)
                            SetProperty(DBReader("PO"), dc_Data.PO)
                            SetProperty(DBReader("StatusChangeDateN"), dc_Data.StatusChangeDateN)
                            SetProperty(DBReader("StatusChangeDateA"), dc_Data.StatusChangeDateA)
                            SetProperty(DBReader("StatusChangeDateE"), dc_Data.StatusChangeDateE)
                            SetProperty(DBReader("StatusChangeDatePQ"), dc_Data.StatusChangeDatePQ)
                            SetProperty(DBReader("StatusChangeDateQ"), dc_Data.StatusChangeDateQ)
                            SetProperty(DBReader("StatusChangeDateII"), dc_Data.StatusChangeDateII)
                            SetProperty(DBReader("StatusChangeDateV"), dc_Data.StatusChangeDateV)
                            Me.Add(dc_Data)
                        End While
                    End Using
                End Using
            End Using

        End Sub
    End Class

End Namespace
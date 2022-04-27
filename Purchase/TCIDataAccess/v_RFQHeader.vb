Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess

    ''' <summary> 
    ''' v_RFQHeader データクラス 
    ''' </summary> 
    ''' <remarks>
    ''' <para>Generated by データクラス自動生成ツール Ver 4.1</para>
    ''' </remarks>
    Public Class v_RFQHeader


        Protected _RFQNumber As Integer = 0
        Protected _EnqLocationCode As String = String.Empty
        Protected _EnqLocationName As String = String.Empty
        Protected _EnqUserID As Integer = 0
        Protected _EnqUserName As String = String.Empty
        Protected _QuoLocationCode As String = String.Empty
        Protected _QuoLocationName As String = String.Empty
        Protected _QuoUserID As Integer? = Nothing
        Protected _QuoUserName As String = String.Empty
        Protected _ProductID As Integer = 0
        Protected _ProductNumber As String = String.Empty
        Protected _ProductName As String = String.Empty
        Protected _isCONFIDENTIAL As Boolean = False
        Protected _SupplierCode As Integer = 0
        Protected _SupplierName As String = String.Empty
        Protected _SupplierCountryCode As String = String.Empty
        Protected _SupplierInfo As String = String.Empty
        Protected _SupplierContactPerson As String = String.Empty
        Protected _R3SupplierCode As String = String.Empty
        Protected _S4SupplierCode As String = String.Empty
        Protected _R3SupplierName As String = String.Empty
        Protected _MakerCode As Integer? = Nothing
        Protected _MakerName As String = String.Empty
        Protected _MakerCountryCode As String = String.Empty
        Protected _MakerInfo As String = String.Empty
        Protected _R3MakerCode As String = String.Empty
        Protected _R3MakerName As String = String.Empty
        Protected _PaymentTermCode As String = String.Empty
        Protected _RequiredPurity As String = String.Empty
        Protected _RequiredQMMethod As String = String.Empty
        Protected _RequiredSpecification As String = String.Empty
        Protected _SpecSheet As Boolean = False
        Protected _Specification As String = String.Empty
        Protected _PurposeCode As String = String.Empty
        Protected _Purpose As String = String.Empty
        Protected _SupplierItemName As String = String.Empty
        Protected _ShippingHandlingFee As Decimal? = Nothing
        Protected _ShippingHandlingCurrencyCode As String = String.Empty
        Protected _Comment As String = String.Empty
        Protected _QuotedDate As DateTime = New DateTime(0)
        Protected _StatusCode As String = String.Empty
        Protected _Priority As String = String.Empty
        Protected _UpdateDate As DateTime = New DateTime(0)
        Protected _Status As String = String.Empty
        Protected _StatusSortOrder As Integer = 0
        Protected _StatusChangeDate As DateTime = New DateTime(0)
        Protected _CreateDate As DateTime = New DateTime(0)
        Protected _QuoStorageLocation As String = String.Empty
        Protected _EnqStorageLocation As String = String.Empty
        Protected _SupplierContactPersonSel As String = String.Empty
        Protected _SAPMakerCode As Integer? = Nothing
        Protected _ProductWarning As String = String.Empty
        Protected _BUoM As String = String.Empty
        Protected _SupplierWarning As String = String.Empty
        Protected _SupplierOfferValidTo As String = String.Empty
        Protected _CASNumber As String = String.Empty
        Protected _CodeExtensionCode As String = String.Empty
        Protected _MMSTAInvalidation As String = String.Empty

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
        ''' EnqLocationCode を設定、または取得する 
        ''' </summary> 
        Public Property EnqLocationCode() As String
            Get
                Return _EnqLocationCode
            End Get
            Set(ByVal value As String)
                _EnqLocationCode = value
            End Set
        End Property

        ''' <summary> 
        ''' EnqLocationName を設定、または取得する 
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
        ''' EnqUserID を設定、または取得する 
        ''' </summary> 
        Public Property EnqUserID() As Integer
            Get
                Return _EnqUserID
            End Get
            Set(ByVal value As Integer)
                _EnqUserID = value
            End Set
        End Property

        ''' <summary> 
        ''' EnqUserName を設定、または取得する 
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
        ''' QuoLocationCode を設定、または取得する 
        ''' </summary> 
        Public Property QuoLocationCode() As String
            Get
                Return _QuoLocationCode
            End Get
            Set(ByVal value As String)
                _QuoLocationCode = value
            End Set
        End Property

        ''' <summary> 
        ''' QuoLocationName を設定、または取得する 
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
        ''' QuoUserID を設定、または取得する 
        ''' <para>
        ''' ※ Integer 変数へ格納する場合は以下のようにすること。
        '''     Dim val As Integer = IIf(QuoUserID.HasValue, QuoUserID, 0)
        '''     Dim val As Integer = QuoUserID.GetValueOrDefault() 
        ''' </para>
        ''' </summary> 
        Public Property QuoUserID() As Integer?
            Get
                Return _QuoUserID
            End Get
            Set(ByVal value As Integer?)
                _QuoUserID = value
            End Set
        End Property

        ''' <summary> 
        ''' QuoUserName を設定、または取得する 
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
        ''' ProductID を設定、または取得する 
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
        ''' ProductNumber を設定、または取得する 
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
        ''' ProductName を設定、または取得する 
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
        ''' isCONFIDENTIAL を設定、または取得する 
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
        ''' SupplierCode を設定、または取得する 
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
        ''' SupplierName を設定、または取得する 
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
        ''' SupplierCountryCode を設定、または取得する 
        ''' </summary> 
        Public Property SupplierCountryCode() As String
            Get
                Return _SupplierCountryCode
            End Get
            Set(ByVal value As String)
                _SupplierCountryCode = value
            End Set
        End Property

        ''' <summary> 
        ''' SupplierInfo を設定、または取得する 
        ''' </summary> 
        Public Property SupplierInfo() As String
            Get
                Return _SupplierInfo
            End Get
            Set(ByVal value As String)
                _SupplierInfo = value
            End Set
        End Property

        ''' <summary> 
        ''' SupplierContactPerson を設定、または取得する 
        ''' </summary> 
        Public Property SupplierContactPerson() As String
            Get
                Return _SupplierContactPerson
            End Get
            Set(ByVal value As String)
                _SupplierContactPerson = value
            End Set
        End Property

        ''' <summary> 
        ''' R3SupplierCode を設定、または取得する 
        ''' </summary> 
        Public Property R3SupplierCode() As String
            Get
                Return _R3SupplierCode
            End Get
            Set(ByVal value As String)
                _R3SupplierCode = value
            End Set
        End Property

        ''' <summary> 
        ''' S4SupplierCode を設定、または取得する 
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
        ''' R3SupplierName を設定、または取得する 
        ''' </summary> 
        Public Property R3SupplierName() As String
            Get
                Return _R3SupplierName
            End Get
            Set(ByVal value As String)
                _R3SupplierName = value
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
        ''' MakerName を設定、または取得する 
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
        ''' MakerCountryCode を設定、または取得する 
        ''' </summary> 
        Public Property MakerCountryCode() As String
            Get
                Return _MakerCountryCode
            End Get
            Set(ByVal value As String)
                _MakerCountryCode = value
            End Set
        End Property

        ''' <summary> 
        ''' MakerInfo を設定、または取得する 
        ''' </summary> 
        Public Property MakerInfo() As String
            Get
                Return _MakerInfo
            End Get
            Set(ByVal value As String)
                _MakerInfo = value
            End Set
        End Property

        ''' <summary> 
        ''' R3MakerCode を設定、または取得する 
        ''' </summary> 
        Public Property R3MakerCode() As String
            Get
                Return _R3MakerCode
            End Get
            Set(ByVal value As String)
                _R3MakerCode = value
            End Set
        End Property

        ''' <summary> 
        ''' R3MakerName を設定、または取得する 
        ''' </summary> 
        Public Property R3MakerName() As String
            Get
                Return _R3MakerName
            End Get
            Set(ByVal value As String)
                _R3MakerName = value
            End Set
        End Property

        ''' <summary> 
        ''' PaymentTermCode を設定、または取得する 
        ''' </summary> 
        Public Property PaymentTermCode() As String
            Get
                Return _PaymentTermCode
            End Get
            Set(ByVal value As String)
                _PaymentTermCode = value
            End Set
        End Property

        ''' <summary> 
        ''' RequiredPurity を設定、または取得する 
        ''' </summary> 
        Public Property RequiredPurity() As String
            Get
                Return _RequiredPurity
            End Get
            Set(ByVal value As String)
                _RequiredPurity = value
            End Set
        End Property

        ''' <summary> 
        ''' RequiredQMMethod を設定、または取得する 
        ''' </summary> 
        Public Property RequiredQMMethod() As String
            Get
                Return _RequiredQMMethod
            End Get
            Set(ByVal value As String)
                _RequiredQMMethod = value
            End Set
        End Property

        ''' <summary> 
        ''' RequiredSpecification を設定、または取得する 
        ''' </summary> 
        Public Property RequiredSpecification() As String
            Get
                Return _RequiredSpecification
            End Get
            Set(ByVal value As String)
                _RequiredSpecification = value
            End Set
        End Property

        ''' <summary> 
        ''' SpecSheet を設定、または取得する 
        ''' </summary> 
        Public Property SpecSheet() As Boolean
            Get
                Return _SpecSheet
            End Get
            Set(ByVal value As Boolean)
                _SpecSheet = value
            End Set
        End Property

        ''' <summary> 
        ''' Specification を設定、または取得する 
        ''' </summary> 
        Public Property Specification() As String
            Get
                Return _Specification
            End Get
            Set(ByVal value As String)
                _Specification = value
            End Set
        End Property

        ''' <summary> 
        ''' PurposeCode を設定、または取得する 
        ''' </summary> 
        Public Property PurposeCode() As String
            Get
                Return _PurposeCode
            End Get
            Set(ByVal value As String)
                _PurposeCode = value
            End Set
        End Property

        ''' <summary> 
        ''' Purpose を設定、または取得する 
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
        ''' SupplierItemName を設定、または取得する 
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
        ''' ShippingHandlingCurrencyCode を設定、または取得する 
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
        ''' Comment を設定、または取得する 
        ''' </summary> 
        Public Property Comment() As String
            Get
                Return _Comment
            End Get
            Set(ByVal value As String)
                _Comment = value
            End Set
        End Property

        ''' <summary> 
        ''' QuotedDate を設定、または取得する 
        ''' </summary> 
        Public Property QuotedDate() As DateTime
            Get
                Return _QuotedDate
            End Get
            Set(ByVal value As DateTime)
                _QuotedDate = value
            End Set
        End Property

        ''' <summary> 
        ''' StatusCode を設定、または取得する 
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
        ''' UpdateDate を設定、または取得する 
        ''' </summary> 
        Public Property UpdateDate() As DateTime
            Get
                Return _UpdateDate
            End Get
            Set(ByVal value As DateTime)
                _UpdateDate = value
            End Set
        End Property

        ''' <summary> 
        ''' Status を設定、または取得する 
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
        ''' StatusSortOrder を設定、または取得する 
        ''' </summary> 
        Public Property StatusSortOrder() As Integer
            Get
                Return _StatusSortOrder
            End Get
            Set(ByVal value As Integer)
                _StatusSortOrder = value
            End Set
        End Property

        ''' <summary> 
        ''' StatusChangeDate を設定、または取得する 
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
        ''' CreateDate を設定、または取得する 
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
        ''' QuoStorageLocation を設定、または取得する 
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
        ''' EnqStorageLocation を設定、または取得する 
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
        ''' SupplierContactPersonSel を設定、または取得する 
        ''' </summary> 
        Public Property SupplierContactPersonSel() As String
            Get
                Return _SupplierContactPersonSel
            End Get
            Set(ByVal value As String)
                _SupplierContactPersonSel = value
            End Set
        End Property

        ''' <summary> 
        ''' SAPMakerCode を設定、または取得する 
        ''' <para>
        ''' ※ Integer 変数へ格納する場合は以下のようにすること。
        '''     Dim val As Integer = IIf(SAPMakerCode.HasValue, SAPMakerCode, 0)
        '''     Dim val As Integer = SAPMakerCode.GetValueOrDefault() 
        ''' </para>
        ''' </summary> 
        Public Property SAPMakerCode() As Integer?
            Get
                Return _SAPMakerCode
            End Get
            Set(ByVal value As Integer?)
                _SAPMakerCode = value
            End Set
        End Property

        ''' <summary> 
        ''' ProductWarning を設定、または取得する 
        ''' </summary> 
        Public Property ProductWarning() As String
            Get
                Return _ProductWarning
            End Get
            Set(ByVal value As String)
                _ProductWarning = value
            End Set
        End Property

        ''' <summary> 
        ''' BUoM を設定、または取得する 
        ''' </summary> 
        Public Property BUoM() As String
            Get
                Return _BUoM
            End Get
            Set(ByVal value As String)
                _BUoM = value
            End Set
        End Property

        ''' <summary> 
        ''' SupplierWarning を設定、または取得する 
        ''' </summary> 
        Public Property SupplierWarning() As String
            Get
                Return _SupplierWarning
            End Get
            Set(ByVal value As String)
                _SupplierWarning = value
            End Set
        End Property

        ''' <summary> 
        ''' SupplierOfferValidTo を設定、または取得する 
        ''' </summary> 
        Public Property SupplierOfferValidTo() As String
            Get
                Return _SupplierOfferValidTo
            End Get
            Set(ByVal value As String)
                _SupplierOfferValidTo = value
            End Set
        End Property

        ''' <summary> 
        ''' CASNumber を設定、または取得する 
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
        ''' CodeExtensionCode を設定、または取得する 
        ''' </summary> 
        Public Property CodeExtensionCode() As String
            Get
                Return _CodeExtensionCode
            End Get
            Set(ByVal value As String)
                _CodeExtensionCode = value
            End Set
        End Property

        ''' <summary> 
        ''' MMSTAInvalidation を設定、または取得する 
        ''' </summary> 
        Public Property MMSTAInvalidation() As String
            Get
                Return _MMSTAInvalidation
            End Get
            Set(ByVal value As String)
                _MMSTAInvalidation = value
            End Set
        End Property

        ''' <summary> 
        ''' コンストラクタ
        ''' </summary> 
        Public Sub New()

        End Sub

        ''' <summary>
        ''' データベースからデータを読み込む。
        ''' </summary>
        ''' <param name="_RFQNumber">RFQNumber</param>
        Public Sub Load(_RFQNumber As String)

            'データベースからデータを読み込む SQL 文字列を生成する。
            Dim Value As New StringBuilder()
            Value.AppendLine("SELECT")
            Value.AppendLine("    [RFQNumber],")
            Value.AppendLine("    [EnqLocationCode],")
            Value.AppendLine("    [EnqLocationName],")
            Value.AppendLine("    [EnqUserID],")
            Value.AppendLine("    [EnqUserName],")
            Value.AppendLine("    [QuoLocationCode],")
            Value.AppendLine("    [QuoLocationName],")
            Value.AppendLine("    [QuoUserID],")
            Value.AppendLine("    [QuoUserName],")
            Value.AppendLine("    [ProductID],")
            Value.AppendLine("    [ProductNumber],")
            Value.AppendLine("    [ProductName],")
            Value.AppendLine("    [isCONFIDENTIAL],")
            Value.AppendLine("    [SupplierCode],")
            Value.AppendLine("    [SupplierName],")
            Value.AppendLine("    [SupplierCountryCode],")
            Value.AppendLine("    [SupplierInfo],")
            Value.AppendLine("    [SupplierContactPerson],")
            Value.AppendLine("    [R3SupplierCode],")
            Value.AppendLine("    [S4SupplierCode],")
            Value.AppendLine("    [R3SupplierName],")
            Value.AppendLine("    [MakerCode],")
            Value.AppendLine("    [MakerName],")
            Value.AppendLine("    [MakerCountryCode],")
            Value.AppendLine("    [MakerInfo],")
            Value.AppendLine("    [R3MakerCode],")
            Value.AppendLine("    [R3MakerName],")
            Value.AppendLine("    [PaymentTermCode],")
            Value.AppendLine("    [RequiredPurity],")
            Value.AppendLine("    [RequiredQMMethod],")
            Value.AppendLine("    [RequiredSpecification],")
            Value.AppendLine("    [SpecSheet],")
            Value.AppendLine("    [Specification],")
            Value.AppendLine("    [PurposeCode],")
            Value.AppendLine("    [Purpose],")
            Value.AppendLine("    [SupplierItemName],")
            Value.AppendLine("    [ShippingHandlingFee],")
            Value.AppendLine("    [ShippingHandlingCurrencyCode],")
            Value.AppendLine("    [Comment],")
            Value.AppendLine("    [QuotedDate],")
            Value.AppendLine("    [StatusCode],")
            Value.AppendLine("    [Priority],")
            Value.AppendLine("    [UpdateDate],")
            Value.AppendLine("    [Status],")
            Value.AppendLine("    [StatusSortOrder],")
            Value.AppendLine("    [StatusChangeDate],")
            Value.AppendLine("    [CreateDate],")
            Value.AppendLine("    [QuoStorageLocation],")
            Value.AppendLine("    [EnqStorageLocation],")
            Value.AppendLine("    [SupplierContactPersonSel],")
            Value.AppendLine("    [SAPMakerCode],")
            Value.AppendLine("    [ProductWarning],")
            Value.AppendLine("    [BUoM],")
            Value.AppendLine("    [SupplierWarning],")
            Value.AppendLine("    [SupplierOfferValidTo],")
            Value.AppendLine("    [CASNumber],")
            Value.AppendLine("    [CodeExtensionCode],")
            Value.AppendLine("    [MMSTAInvalidation]")
            Value.AppendLine("FROM")
            Value.AppendLine("    [v_RFQHeader]")
            Value.AppendLine("WHERE")
            Value.AppendLine("    [RFQNumber] = @RFQNumber")
            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = Value.ToString()
                    DBCommand.Parameters.AddWithValue("RFQNumber", _RFQNumber)
                    DBConn.Open()
                    Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
                    If DBReader.Read() Then
                        SetProperty(DBReader("RFQNumber"), _RFQNumber)
                        SetProperty(DBReader("EnqLocationCode"), _EnqLocationCode)
                        SetProperty(DBReader("EnqLocationName"), _EnqLocationName)
                        SetProperty(DBReader("EnqUserID"), _EnqUserID)
                        SetProperty(DBReader("EnqUserName"), _EnqUserName)
                        SetProperty(DBReader("QuoLocationCode"), _QuoLocationCode)
                        SetProperty(DBReader("QuoLocationName"), _QuoLocationName)
                        SetProperty(DBReader("QuoUserID"), _QuoUserID)
                        SetProperty(DBReader("QuoUserName"), _QuoUserName)
                        SetProperty(DBReader("ProductID"), _ProductID)
                        SetProperty(DBReader("ProductNumber"), _ProductNumber)
                        SetProperty(DBReader("ProductName"), _ProductName)
                        SetProperty(DBReader("isCONFIDENTIAL"), _isCONFIDENTIAL)
                        SetProperty(DBReader("SupplierCode"), _SupplierCode)
                        SetProperty(DBReader("SupplierName"), _SupplierName)
                        SetProperty(DBReader("SupplierCountryCode"), _SupplierCountryCode)
                        SetProperty(DBReader("SupplierInfo"), _SupplierInfo)
                        SetProperty(DBReader("SupplierContactPerson"), _SupplierContactPerson)
                        SetProperty(DBReader("R3SupplierCode"), _R3SupplierCode)
                        SetProperty(DBReader("S4SupplierCode"), _S4SupplierCode)
                        SetProperty(DBReader("R3SupplierName"), _R3SupplierName)
                        SetProperty(DBReader("MakerCode"), _MakerCode)
                        SetProperty(DBReader("MakerName"), _MakerName)
                        SetProperty(DBReader("MakerCountryCode"), _MakerCountryCode)
                        SetProperty(DBReader("MakerInfo"), _MakerInfo)
                        SetProperty(DBReader("R3MakerCode"), _R3MakerCode)
                        SetProperty(DBReader("R3MakerName"), _R3MakerName)
                        SetProperty(DBReader("PaymentTermCode"), _PaymentTermCode)
                        SetProperty(DBReader("RequiredPurity"), _RequiredPurity)
                        SetProperty(DBReader("RequiredQMMethod"), _RequiredQMMethod)
                        SetProperty(DBReader("RequiredSpecification"), _RequiredSpecification)
                        SetProperty(DBReader("SpecSheet"), _SpecSheet)
                        SetProperty(DBReader("Specification"), _Specification)
                        SetProperty(DBReader("PurposeCode"), _PurposeCode)
                        SetProperty(DBReader("Purpose"), _Purpose)
                        SetProperty(DBReader("SupplierItemName"), _SupplierItemName)
                        SetProperty(DBReader("ShippingHandlingFee"), _ShippingHandlingFee)
                        SetProperty(DBReader("ShippingHandlingCurrencyCode"), _ShippingHandlingCurrencyCode)
                        SetProperty(DBReader("Comment"), _Comment)
                        SetProperty(DBReader("QuotedDate"), _QuotedDate)
                        SetProperty(DBReader("StatusCode"), _StatusCode)
                        SetProperty(DBReader("Priority"), _Priority)
                        SetProperty(DBReader("UpdateDate"), _UpdateDate)
                        SetProperty(DBReader("Status"), _Status)
                        SetProperty(DBReader("StatusSortOrder"), _StatusSortOrder)
                        SetProperty(DBReader("StatusChangeDate"), _StatusChangeDate)
                        SetProperty(DBReader("CreateDate"), _CreateDate)
                        SetProperty(DBReader("QuoStorageLocation"), _QuoStorageLocation)
                        SetProperty(DBReader("EnqStorageLocation"), _EnqStorageLocation)
                        SetProperty(DBReader("SupplierContactPersonSel"), _SupplierContactPersonSel)
                        SetProperty(DBReader("SAPMakerCode"), _SAPMakerCode)
                        SetProperty(DBReader("ProductWarning"), _ProductWarning)
                        SetProperty(DBReader("BUoM"), _BUoM)
                        SetProperty(DBReader("SupplierWarning"), _SupplierWarning)
                        SetProperty(DBReader("SupplierOfferValidTo"), _SupplierOfferValidTo)
                        SetProperty(DBReader("CASNumber"), _CASNumber)
                        SetProperty(DBReader("CodeExtensionCode"), _CodeExtensionCode)
                        SetProperty(DBReader("MMSTAInvalidation"), _MMSTAInvalidation)
                    Else
                        Throw New KeyNotFoundException(String.Format("RFQNumber:{0}", _RFQNumber))
                    End If
                    DBReader.Close()
                End Using
            End Using

        End Sub

        '''' <summary> 
        '''' データの存在チェックを行う。
        '''' </summary> 
        '''' <returns>存在する場合は True、しない場合は False を返す</returns> 
        'Public Shared Function IsExists() As Boolean

        '    ' データの存在チェックを行う SQL 文字列を生成する。
        '    Dim Value As New StringBuilder()
        '    Value.AppendLine("SELECT")
        '    Value.AppendLine("    COUNT(*)")
        '    Value.AppendLine("FROM")
        '    Value.AppendLine("    [v_RFQHeader]")

        '    Using DBConn As New SqlConnection(DB_CONNECT_STRING)
        '        Using DBCommand As SqlCommand = DBConn.CreateCommand()
        '            DBCommand.CommandText = Value.ToString()
        '            DBCommand.Parameters.Clear()
        '            DBConn.Open()
        '            Dim i_Count As Integer = Convert.ToInt32(DBCommand.ExecuteScalar())
        '            Return i_Count > 0
        '        End Using
        '    End Using

        'End Function

    End Class

    ''' <summary> 
    ''' v_RFQHeader リストクラス 
    ''' </summary> 
    Public Class v_RFQHeaderList
        Inherits List(Of v_RFQHeader)

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

    End Class

End Namespace

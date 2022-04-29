Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Text
Imports System.Data.SqlClient
Imports Purchase.TCIDataAccess.DBCommon
Imports Purchase.Common

Namespace TCIDataAccess.Join
    Public Class RFQHeader
        Protected _StatusChangeDate As datetime
        Protected _Status As String = String.Empty
        Protected _RFQNumber As Integer = 0
        Protected _Priority As String = String.Empty
        Protected _QuotedDate As datetime
        Protected _ProductID As Integer = 0
        Protected _ProductNumber As String = String.Empty
        Protected _ProductName As String = String.Empty
        Protected _SupplierCode As Integer = 0
        Protected _SupplierName As String = String.Empty
        Protected _SupplierInfo As String = String.Empty
        Protected _MakerCountryCode As String = String.Empty
        Protected _MakerCountryName As String = String.Empty
        Protected _Purpose As String = String.Empty
        Protected _MakerName As String = String.Empty
        Protected _MakerInfo As String = String.Empty
        Protected _SupplierCountryCode As String = String.Empty
        Protected _SupplierCountryName As String = String.Empty
        Protected _SupplierItemName As String = String.Empty
        Protected _ShippingHandlingCurrencyCode As String = String.Empty
        Protected _ShippingHandlingFee As Decimal? = Nothing
        Protected _EnqUserName As String = String.Empty
        Protected _EnqLocationName As String = String.Empty
        Protected _QuoUserName As String = String.Empty
        Protected _QuoLocationName As String = String.Empty
        Protected _Comment As String = String.Empty
        Protected _isCONFIDENTIAL As Boolean = False
        Protected _CodeExtensionCode As String = String.Empty
        ''' <summary> 
        ''' StatusChangeDate を設定、または取得する 
        ''' </summary> 
        Public Property StatusChangeDate() As datetime
            Get
                Return _StatusChangeDate
            End Get
            Set(ByVal value As datetime)
                _StatusChangeDate = value
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
        ''' QuotedDate を設定、または取得する 
        ''' </summary> 
        Public Property QuotedDate() As datetime
            Get
                Return _QuotedDate
            End Get
            Set(ByVal value As datetime)
                _QuotedDate = value
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
        ''' MakerCountryName を設定、または取得する 
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
        ''' SupplierCountryName を設定、または取得する 
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
        ''' ShippingHandlingFee を設定、または取得する 
        ''' <para>
        ''' ※ Decimal 変数へ格納する場合は以下のようにすること。
        '''     Dim val As Decimal = IIf(ShippingHandlingFee.HasValue, QuoPer, 0)
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
        
        Public Sub New()

        End Sub

    End Class
    Public Class RFQHeaderList
        Inherits List(Of RFQHeader)
        Protected Const AddTypeString As Integer = 0
        Protected Const AddTypeDateTime As Integer = 1
        Public Sub New()

        End Sub
        ''' <summary>
        ''' データ読込み
        ''' </summary>
        ''' <param name="SkipRecord">読み飛ばしレコード数</param>
        ''' <param name="PageSize">格納件数</param>
        ''' <param name="Cond">検索条件</param>
        ''' <returns>総データ件数</returns>
        ''' <remarks></remarks>
        Public Function Load(ByVal SkipRecord As Integer, ByVal PageSize As Integer, _
                                    ByVal Cond As KeywordSearchConditionParameter) As Integer
            Dim i_TotalCount As Integer = 0
            Dim sb_SQL As New StringBuilder()
            Dim sb_Cond As New StringBuilder()

            'SQL文の生成
            sb_SQL = CreateRFQSelectClauseSQL() 
            sb_SQL = CreateRFQHeaderBaseSQL(sb_SQL, Cond) 
            sb_SQL = CreateRFQOrderByClauseSQL(sb_SQL)

            Using DBConn As New SqlConnection(DBCommon.DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    Dim Params As SqlParameterCollection = DBCommand.Parameters
                    DBCommand.CommandText = sb_SQL.ToString()
                    DBCommand.Parameters.Clear()
                    '生成したSQL文に入力項目を反映
                    SetParamInClauseSQL(DBCommand,"RFQNumber",Cond.RFQNumber)
                    SetParamInClauseSQL(DBCommand,"ProductNumber",Cond.ProductNumber)
                    If Not String.IsNullOrEmpty(Cond.ProductName) Then
                        DBCommand.Parameters.AddWithValue("ProductName", StrConv(Cond.ProductName, VbStrConv.Narrow))
                    End If
                    SetParamInClauseSQL(DBCommand,"SupplierCode",Cond.SupplierCode)
                    SetParamInClauseSQL(DBCommand,"S4SupplierCode",Cond.S4SupplierCode)
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
                    SetPramMultipleSelectionInClauseSQL(DBCommand,"Purpose",Cond.Purpose)
                    SetPramMultipleSelectionInClauseSQL(DBCommand,"TerritoryCode",Cond.Territory)
                    If Not String.IsNullOrEmpty(Cond.Priority) Then
                        DBCommand.Parameters.AddWithValue("Priority", Cond.Priority)
                    End If

                    DBConn.Open()
                    Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()

                    While DBReader.Read()
                        i_TotalCount += 1
                        If i_TotalCount <= SkipRecord Then
                            'SkipRecord まで読み飛ばし
                            Continue While
                        End If
                        If PageSize > 0 AndAlso Me.Count >= PageSize Then
                            '格納件数 (ページサイズ) を超えたら読み飛ばし
                            Continue While
                        End If

                        Dim dc_Data As New RFQHeader
                        SetProperty(DBReader("StatusChangeDate"), dc_Data.StatusChangeDate)
                        SetProperty(DBReader("Status"), dc_Data.Status)
                        SetProperty(DBReader("RFQNumber"), dc_Data.RFQNumber)
                        SetProperty(DBReader("Priority"), dc_Data.Priority)
                        SetProperty(DBReader("QuotedDate"), dc_Data.QuotedDate)
                        SetProperty(DBReader("ProductID"), dc_Data.ProductID)
                        SetProperty(DBReader("ProductNumber"), dc_Data.ProductNumber)
                        SetProperty(DBReader("ProductName"), dc_Data.ProductName)
                        SetProperty(DBReader("SupplierCode"), dc_Data.SupplierCode)
                        SetProperty(DBReader("SupplierName"), dc_Data.SupplierName)
                        SetProperty(DBReader("SupplierInfo"), dc_Data.SupplierInfo)
                        SetProperty(DBReader("MakerCountryCode"), dc_Data.MakerCountryCode)
                        SetProperty(DBReader("MakerCountryName"), dc_Data.MakerCountryName)
                        SetProperty(DBReader("Purpose"), dc_Data.Purpose)
                        SetProperty(DBReader("MakerName"), dc_Data.MakerName)
                        SetProperty(DBReader("MakerInfo"), dc_Data.MakerInfo)
                        SetProperty(DBReader("SupplierCountryCode"), dc_Data.SupplierCountryCode)
                        SetProperty(DBReader("SupplierCountryName"), dc_Data.SupplierCountryName)
                        SetProperty(DBReader("SupplierItemName"), dc_Data.SupplierItemName)
                        SetProperty(DBReader("ShippingHandlingCurrencyCode"), dc_Data.ShippingHandlingCurrencyCode)
                        SetProperty(DBReader("ShippingHandlingFee"), dc_Data.ShippingHandlingFee)
                        SetProperty(DBReader("EnqUserName"), dc_Data.EnqUserName)
                        SetProperty(DBReader("EnqLocationName"), dc_Data.EnqLocationName)
                        SetProperty(DBReader("QuoUserName"), dc_Data.QuoUserName)
                        SetProperty(DBReader("QuoLocationName"), dc_Data.QuoLocationName)
                        SetProperty(DBReader("Comment"), dc_Data.Comment)
                        SetProperty(DBReader("isCONFIDENTIAL"), dc_Data.isCONFIDENTIAL)
                        SetProperty(DBReader("CodeExtensionCode"), dc_Data.CodeExtensionCode)

                        Me.Add(dc_Data)
                    End While
                    DBReader.Close()
                End Using
            End Using

            Return i_TotalCount
        End Function
        ''' <summary>
        ''' RFQHeader検索SQL文字列のSELECT句を生成します。
        ''' </summary>
        ''' <returns>sb_SQL：RFQHeader検索用SELECT句</returns>
        ''' <remarks></remarks>
        Private Function CreateRFQSelectClauseSQL() As Text.StringBuilder
            Dim Value As New Text.StringBuilder

            Value.AppendLine("SELECT ")
            Value.AppendLine("    rfh.[StatusChangeDate],")
            Value.AppendLine("    rfh.[Status],")
            Value.AppendLine("    rfh.[RFQNumber],")
            Value.AppendLine("    ISNULL(rfh.[Priority], '') AS Priority,")
            Value.AppendLine("    rfh.[QuotedDate],")
            Value.AppendLine("    rfh.[ProductID],")
            Value.AppendLine("    rfh.[ProductNumber],")
            Value.AppendLine("    rfh.[ProductName],")
            Value.AppendLine("    rfh.[SupplierCode],")
            Value.AppendLine("    rfh.[SupplierName],")
            Value.AppendLine("    rfh.[SupplierInfo],")
            Value.AppendLine("    rfh.[MakerCountryCode],")
            Value.AppendLine("    mcry.[Name] AS MakerCountryName,")
            Value.AppendLine("    rfh.[Purpose],")
            Value.AppendLine("    rfh.[MakerName],")
            Value.AppendLine("    rfh.[MakerInfo],")
            Value.AppendLine("    rfh.[SupplierCountryCode],")
            Value.AppendLine("    scry.[Name] AS SupplierCountryName,")
            Value.AppendLine("    rfh.[SupplierItemName],")
            Value.AppendLine("    rfh.[ShippingHandlingCurrencyCode],")
            Value.AppendLine("    rfh.[ShippingHandlingFee],")
            Value.AppendLine("    rfh.[EnqUserName],")
            Value.AppendLine("    rfh.[EnqLocationName],")
            Value.AppendLine("    rfh.[QuoUserName],")
            Value.AppendLine("    rfh.[QuoLocationName],")
            Value.AppendLine("    rfh.[Comment],")
            Value.AppendLine("    rfh.[isCONFIDENTIAL],")
            Value.AppendLine("    rfh.[CodeExtensionCode]")
            Return Value

        End Function
        ''' <summary>
        ''' RFQHeader検索のベースとなるSQL文字列を生成します。
        ''' </summary>
        ''' <param name="Value">SQL文字列用StringBuilder</param>
        ''' <param name="Cond">検索条件</param>
        ''' <returns>sb_SQL：RFQHeader検索時のベースとなるSQL文</returns>
        Private Function CreateRFQHeaderBaseSQL(ByVal Value As Text.StringBuilder,ByVal Cond As KeywordSearchConditionParameter) As Text.StringBuilder

            'ベースとなるSQL文字列の作成
            Value.AppendLine("FROM")
            Value.AppendLine("    v_RFQHeader rfh")
            Value.AppendLine("    LEFT JOIN")
            Value.AppendLine("        s_Country mcry")
            Value.AppendLine("        ON ")
            Value.AppendLine("        rfh.MakerCountryCode = mcry.CountryCode")
            Value.AppendLine("    LEFT JOIN")
            Value.AppendLine("        s_Country scry")
            Value.AppendLine("        ON ")
            Value.AppendLine("        rfh.SupplierCountryCode = scry.CountryCode")
            If Cond.ValidQuotation = "Valid Price" Then
                Value.AppendLine("    INNER JOIN ")
                Value.AppendLine("        ( SELECT ProductID,UnitPrice,RFQNumber")
                Value.AppendLine("        FROM v_RFQLine")
                Value.AppendLine("        GROUP BY ProductID,UnitPrice,RFQNumber HAVING SUM(UnitPrice) >= 0) vRL")
                Value.AppendLine("        ON")
                Value.AppendLine("        rfh.RFQNumber =  vRL.RFQNumber")
            End If
            If Cond.ValidQuotation = "Inalid Price" Then
                Value.AppendLine("    INNER JOIN ")
                Value.AppendLine("        ( SELECT ProductID,UnitPrice,RFQNumber")
                Value.AppendLine("        FROM v_RFQLine")
                Value.AppendLine("        WHERE UnitPrice IS NULL")
                Value.AppendLine("        GROUP BY ProductID,UnitPrice,RFQNumber ) vRL")
                Value.AppendLine("        ON")
                Value.AppendLine("        rfh.RFQNumber =  vRL.RFQNumber")
            End If
            If DBCommon.isTerritoryCheckd(Cond.Territory) Then
                Value.AppendLine("    LEFT JOIN")
                Value.AppendLine("        v_Territory vTertry")
                Value.AppendLine("        ON ")
                Value.AppendLine("        rfh. SupplierCode = vTertry.SupplierCode")
            End If
            '画面で入力された値のWhere句の生成
            Value.AppendLine("WHERE")

            Dim WhereClause As String = CreateRFQHeaderWhereClauseSQL(cond)
            Value.AppendLine(WhereClause)

            Return Value

        End Function

        ''' <summary>
        ''' RFQHeader検索で使用するWhere句SQL文字列を生成します。
        ''' </summary>
        ''' <param name="Cond">検索条件</param>
        ''' <returns>sb_SQL：RFQHeader検索時のベースとなるSQL文</returns>
        Public Shared Function CreateRFQHeaderWhereClauseSQL(ByVal Cond As KeywordSearchConditionParameter) As String

            Dim WhereClause As String = ""
            WhereClause = AddMultipleListItemWhereClauseSQL(WhereClause,CreateRFQInClauseSQL("RFQNumber", Cond.RFQNumber))

            WhereClause = AddMultipleListItemWhereClauseSQL(WhereClause,CreateRFQInClauseSQL("ProductNumber", Cond.ProductNumber))
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.ProductName LIKE '%' + @ProductName + '%' ", Cond.ProductName)
            WhereClause = AddMultipleListItemWhereClauseSQL(WhereClause,CreateRFQInClauseSQL("SupplierCode", Cond.SupplierCode))
            WhereClause = AddMultipleListItemWhereClauseSQL(WhereClause,CreateRFQInClauseSQL("S4SupplierCode", Cond.S4SupplierCode))
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.SupplierName LIKE '%' + @SupplierName + '%' ", Cond.SupplierName)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.SupplierCountryCode = @SupplierCountryCode", Cond.SupplierCountryCode)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.SupplierItemName = @SupplierItemName", Cond.SupplierItemName)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.StatusSortOrder >= @StatusFrom", Cond.StatusFrom)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.StatusSortOrder <= @StatusTo", Cond.StatusTo)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.CreateDate >= @RFQCreatedDateFrom", Cond.RFQCreatedDateFrom)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.CreateDate <= @RFQCreatedDateTo", Cond.RFQCreatedDateTo)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.QuotedDate >= @RFQQuotedDateFrom", Cond.RFQQuotedDateFrom)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.QuotedDate <= @RFQQuotedDateTo", Cond.RFQQuotedDateTo)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.StatusChangeDate >= @LastRFQStatusChangeDateFrom", Cond.LastRFQStatusChangeDateFrom)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.StatusChangeDate <= @LastRFQStatusChangeDateTo", Cond.LastRFQStatusChangeDateTo)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.EnqLocationCode = @EnqLocationCode", Cond.EnqLocationCode)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.EnqUserID = @EnqUserID", Cond.EnqUserID)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.EnqStorageLocation = @EnqStorageLocation", Cond.EnqStorageLocation)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.QuoLocationCode = @QuoLocationCode", Cond.QuoLocationCode)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.QuoUserID = @QuoUserID", Cond.QuoUserID)
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.QuoStorageLocation = @QuoStorageLocation", Cond.QuoStorageLocation)
            WhereClause = AddMultipleListItemWhereClauseSQL(WhereClause, CreateMultipleSelectionInClauseSQL("Purpose", Cond.Purpose))
            WhereClause = AddMultipleListItemWhereClauseSQL(WhereClause, CreateMultipleSelectionInClauseSQL("TerritoryCode", Cond.Territory))
            WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.Priority = @Priority", Cond.Priority)
            'ValidQuotationの入力判定
            If Not String.IsNullOrEmpty(Cond.ValidQuotation) Then
                If Cond.ValidQuotation = "Y" Then
                    WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.ValidQuotation = 1", "1")
                Else IF Cond.ValidQuotation = "N" Then
                    WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.ValidQuotation = 0", "0")
                End If
            End If
            '権限ロールに従い極秘品を除外する
            If Common.CheckSessionRole(Cond.s_RoleCode) = False Then
                WhereClause = AddRFQWhereClauseSQL(WhereClause, "rfh.isCONFIDENTIAL = 0", "0")
            End If

            Return WhereClause

        End Function

        ''' <summary>
        ''' RFQHeader検索SQL文字列のORDER BY句を生成します。
        ''' </summary>
        ''' <param name="Value">SQL文字列用StringBuilder</param>
        ''' <returns>sb_SQL：RFQHeader検索用ORDER BY句</returns>
        ''' <remarks></remarks>
        Private Function CreateRFQOrderByClauseSQL(ByVal Value As Text.StringBuilder) As Text.StringBuilder
            
            Value.AppendLine("ORDER BY ")
            Value.AppendLine("    rfh.[StatusSortOrder] ASC,")
            Value.AppendLine("    rfh.[QuotedDate] DESC,")
            Value.AppendLine("    rfh.[StatusChangeDate] DESC,")
            Value.AppendLine("    rfh.[RFQNumber] ASC")

            Return Value
        End Function

    End Class
    ''' <summary>
    ''' キーワード検索条件パラメータ情報クラス
    ''' </summary>
    ''' <remarks></remarks>
    Public Class KeywordSearchConditionParameter

        ''' <summary> 現在のページインデックス </summary>
        Public CurrentPageIndex As Integer = 0

        ''' <summary> SessionのLocationCode </summary>
        Public s_LocationCode As String = String.Empty

        ''' <summary> SessionのRoleCode </summary>
        Public s_RoleCode As String = String.Empty

        ''' <summary> RFQNumber </summary>
        Public RFQNumber() As String

        ''' <summary> Purpose </summary>
        Public Purpose As System.Web.UI.WebControls.ListItemCollection

        ''' <summary> Territory </summary>
        Public Territory As System.Web.UI.WebControls.ListItemCollection

        ''' <summary> ProductNumber </summary>
        Public ProductNumber () As String

        ''' <summary> ProductName </summary>
        Public ProductName As String = String.Empty

        ''' <summary> SupplierCode </summary>
        Public SupplierCode () As String

        ''' <summary> S4SupplierCode </summary>
        Public S4SupplierCode () As String

        ''' <summary> SupplierName </summary>
        Public SupplierName As String = String.Empty

        ''' <summary> SupplierItemName </summary>
        Public SupplierItemName As String = String.Empty

        ''' <summary> SupplierCountryCode </summary>
        Public SupplierCountryCode As String = String.Empty

        ''' <summary> StatusFrom </summary>
        Public StatusFrom As String = String.Empty

        ''' <summary> StatusTo </summary>
        Public StatusTo As String = String.Empty

        ''' <summary> EnqLocationCode </summary>
        Public EnqLocationCode As String = String.Empty

        ''' <summary> EnqUserID </summary>
        Public EnqUserID As String = String.Empty

        ''' <summary> EnqStorageLocation </summary>
        Public EnqStorageLocation As String = String.Empty

        ''' <summary> QuoLocationCode </summary>
        Public QuoLocationCode As String = String.Empty

        ''' <summary> QuoUserID </summary>
        Public QuoUserID As String = String.Empty

        ''' <summary> QuoUserID </summary>
        Public QuoStorageLocation As String = String.Empty

        ''' <summary> QuoUserID </summary>
        Public Priority As String = String.Empty

        ''' <summary> RFQCreatedDateFrom </summary>
        Public RFQCreatedDateFrom As String = String.Empty

        ''' <summary> RFQCreatedDateTo </summary>
        Public RFQCreatedDateTo As String = String.Empty

        ''' <summary> RFQQuotedDateFrom </summary>
        Public RFQQuotedDateFrom As String = String.Empty

        ''' <summary> RFQQuotedDateTo </summary>
        Public RFQQuotedDateTo As String = String.Empty

        ''' <summary> LastRFQStatusChangeDateFrom </summary>
        Public LastRFQStatusChangeDateFrom As String = String.Empty

        ''' <summary> LastRFQStatusChangeDateTo </summary>
        Public LastRFQStatusChangeDateTo As String = String.Empty
        ''' <summary> ValidQuotation </summary>
        Public ValidQuotation As String = String.Empty
            

        
    End Class

End Namespace
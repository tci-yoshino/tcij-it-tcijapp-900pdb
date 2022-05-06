Option Explicit On
Option Strict On
Option Infer Off

Imports System.Data.SqlClient

Namespace TCIDataAccess.Join

    ''' <summary> 
    ''' RFQListByProductDisp データクラス 
    ''' </summary> 
    Public Class RFQListByProductDisp

        Protected _ProductID As Integer = 0
        Protected _ProductNumber As String = String.Empty
        Protected _RFQNumber As Integer = 0
        Protected _ValidityQuotation As String = String.Empty
        Protected _ProductHeader As List(Of RFQListByProducttHeader) = New List(Of RFQListByProducttHeader)
        Protected _RFQListHeader As List(Of RFQListByProductRFQListHeader) = New List(Of RFQListByProductRFQListHeader)
        Protected _RFQLine As List(Of RFQListByProductRFQLine) = New List(Of RFQListByProductRFQLine)

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
        ''' ValidityQuotation を設定、または取得する 
        ''' </summary> 
        Public Property ValidityQuotation() As String
            Get
                Return _ValidityQuotation
            End Get
            Set(ByVal value As String)
                _ValidityQuotation = value
            End Set
        End Property

        ''' <summary> 
        ''' ProductHeader を設定、または取得する 
        ''' </summary> 
        Public Property ProductHeader() As List(Of RFQListByProducttHeader)
            Get
                Return _ProductHeader
            End Get
            Set(ByVal value As List(Of RFQListByProducttHeader))
                _ProductHeader = value
            End Set
        End Property

        ''' <summary> 
        ''' RFQListHeader を設定、または取得する 
        ''' </summary> 
        Public Property RFQListHeader() As List(Of RFQListByProductRFQListHeader)
            Get
                Return _RFQListHeader
            End Get
            Set(ByVal value As List(Of RFQListByProductRFQListHeader))
                _RFQListHeader = value
            End Set
        End Property

        ''' <summary> 
        ''' RFQLine を設定、または取得する 
        ''' </summary> 
        Public Property RFQLine() As List(Of RFQListByProductRFQLine)
            Get
                Return _RFQLine
            End Get
            Set(ByVal value As List(Of RFQListByProductRFQLine))
                _RFQLine = value
            End Set
        End Property

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' 製品検索SQL文字列を生成します。
        ''' </summary>
        ''' <returns>SQL文字列</returns>
        ''' <remarks></remarks>
        Protected Friend Function CreateProductHeaderSelectSQL() As String

            Dim sb_SQL As New Text.StringBuilder

            'SQL文字列の作成
            sb_SQL.AppendLine("SELECT ")
            sb_SQL.AppendLine("    P.[ProductNumber], ")
            sb_SQL.AppendLine("    P.[QuoName], ")
            sb_SQL.AppendLine("    P.[Name], ")
            sb_SQL.AppendLine("    P.[CASNumber], ")
            sb_SQL.AppendLine("    P.[MolecularFormula], ")
            sb_SQL.AppendLine("    P.[ProductWarning], ")
            sb_SQL.AppendLine("    MU.[EN] AS BUoM ")
            sb_SQL.AppendLine("FROM ")
            sb_SQL.AppendLine("    [Product] AS P ")
            sb_SQL.AppendLine("  LEFT OUTER JOIN [s_Material] AS M ON P.[ProductNumber] = M.[ERPProductNumber] ")
            sb_SQL.AppendLine("  LEFT OUTER JOIN [s_MaterialUnit] AS MU ON M.[BaseUnitOfMeasure] = MU.[Unit] ")
            sb_SQL.AppendLine("WHERE ")
            sb_SQL.AppendLine("    P.[ProductID] = @ProductID ")

            Return sb_SQL.ToString()

        End Function

        Protected Friend Sub GetProductHeader()
            Dim productHeader As List(Of RFQListByProducttHeader) = New List(Of RFQListByProducttHeader)

            Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.Parameters.Clear()
                    If Me.ProductID <> 0 Then
                        DBCommand.Parameters.AddWithValue("ProductID", Me.ProductID)
                    End If
                    DBCommand.CommandText = CreateProductHeaderSelectSQL()

                    ' 実行
                    DBConn.Open()
                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader()
                        While DBReader.Read
                            Dim dc_RFQListByProducttHeader As RFQListByProducttHeader = New RFQListByProducttHeader

                            DBCommon.SetProperty(DBReader("ProductNumber"), dc_RFQListByProducttHeader.ProductNumber)
                            DBCommon.SetProperty(DBReader("QuoName"), dc_RFQListByProducttHeader.QuoName)
                            DBCommon.SetProperty(DBReader("Name"), dc_RFQListByProducttHeader.Name)
                            DBCommon.SetProperty(DBReader("CASNumber"), dc_RFQListByProducttHeader.CASNumber)
                            DBCommon.SetProperty(DBReader("MolecularFormula"), dc_RFQListByProducttHeader.MolecularFormula)
                            DBCommon.SetProperty(DBReader("ProductWarning"), dc_RFQListByProducttHeader.ProductWarning)
                            DBCommon.SetProperty(DBReader("BUoM"), dc_RFQListByProducttHeader.BUoM)

                            productHeader.Add(dc_RFQListByProducttHeader)
                        End While
                    End Using
                End Using
            End Using

            Me.ProductHeader = productHeader
        End Sub

        Protected Friend Sub GetRFQListHeader()

            Dim sb_SQL As New Text.StringBuilder

            'SQL文字列の作成
            sb_SQL.AppendLine("SELECT ")
            sb_SQL.AppendLine("	 RFH.[StatusChangeDate], ")
            sb_SQL.AppendLine("	 RFH.[Status], ")
            sb_SQL.AppendLine("	 RFH.[RFQNumber], ")
            sb_SQL.AppendLine("	 ISNULL(RFH.[Priority], '') AS Priority, ")
            sb_SQL.AppendLine("	 RFH.[QuotedDate], ")
            sb_SQL.AppendLine("	 RFH.[ProductNumber], ")
            sb_SQL.AppendLine("	 ISNULL(RFH.[CodeExtensionCode], '') AS CodeExtension, ")
            sb_SQL.AppendLine("	 RFH.[ProductName], ")
            sb_SQL.AppendLine("	 RFH.[SupplierCode], ")
            sb_SQL.AppendLine("	 RFH.[SupplierName], ")
            sb_SQL.AppendLine("	 RFH.[SupplierInfo], ")
            sb_SQL.AppendLine("	 RFH.[MakerCountryCode], ")
            sb_SQL.AppendLine("	 MCRY.[Name] AS MakerCountryName, ")
            sb_SQL.AppendLine("	 RFH.[Purpose], ")
            sb_SQL.AppendLine("	 RFH.[MakerName], ")
            sb_SQL.AppendLine("	 RFH.[MakerInfo], ")
            sb_SQL.AppendLine("	 RFH.[SupplierCountryCode], ")
            sb_SQL.AppendLine("	 SCRY.[Name] AS SupplierCountryName, ")
            sb_SQL.AppendLine("	 RFH.[SupplierItemName], ")
            sb_SQL.AppendLine("	 RFH.[ShippingHandlingCurrencyCode],")
            sb_SQL.AppendLine("	 RFH.[ShippingHandlingFee], ")
            sb_SQL.AppendLine("	 RFH.[EnqUserName], ")
            sb_SQL.AppendLine("	 RFH.[EnqLocationName], ")
            sb_SQL.AppendLine("	 RFH.[QuoUserName], ")
            sb_SQL.AppendLine("	 RFH.[QuoLocationName], ")
            sb_SQL.AppendLine("	 RFH.[Comment], ")
            sb_SQL.AppendLine("	 RFH.[isCONFIDENTIAL] ")
            sb_SQL.AppendLine("FROM ")
            sb_SQL.AppendLine("  [v_RFQHeader] RFH ")
            sb_SQL.AppendLine("  LEFT OUTER JOIN [s_Country] MCRY ")
            sb_SQL.AppendLine("    ON RFH.[MakerCountryCode] = MCRY.[CountryCode] ")
            sb_SQL.AppendLine("  INNER JOIN [s_Country] SCRY ")
            sb_SQL.AppendLine("    ON RFH.[SupplierCountryCode] = SCRY.[CountryCode] ")
            sb_SQL.AppendLine("WHERE ")
            sb_SQL.AppendLine("	 RFH.[ProductID] = @ProductID ")
            Select Case ValidityQuotation
                Case "Y"
                    sb_SQL.AppendLine("	 AND RFH.[ValidQuotation ] = 1 ")
                Case "N"
                    sb_SQL.AppendLine("	 AND RFH.[ValidQuotation ] = 0 ")
            End Select
            sb_SQL.AppendLine("ORDER BY ")
            sb_SQL.AppendLine(" RFH.[StatusSortOrder] ASC, ")
            sb_SQL.AppendLine(" RFH.[QuotedDate] DESC, ")
            sb_SQL.AppendLine(" RFH.[StatusChangeDate] DESC, ")
            sb_SQL.AppendLine(" RFH.[RFQNumber] ASC ")

            Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.Parameters.Clear()
                    If Me.ProductID <> 0 Then
                        DBCommand.Parameters.AddWithValue("ProductID", Me.ProductID)
                    End If
                    DBCommand.CommandText = sb_SQL.ToString()

                    ' 実行
                    DBConn.Open()
                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader()
                        While DBReader.Read
                            Dim dc_RFQListHeader As RFQListByProductRFQListHeader = New RFQListByProductRFQListHeader

                            DBCommon.SetProperty(DBReader("StatusChangeDate"), dc_RFQListHeader.StatusChangeDate)
                            DBCommon.SetProperty(DBReader("Status"), dc_RFQListHeader.Status)
                            DBCommon.SetProperty(DBReader("RFQNumber"), dc_RFQListHeader.RFQNumber)
                            DBCommon.SetProperty(DBReader("Priority"), dc_RFQListHeader.Priority)
                            DBCommon.SetProperty(DBReader("QuotedDate"), dc_RFQListHeader.QuotedDate)
                            DBCommon.SetProperty(DBReader("ProductNumber"), dc_RFQListHeader.ProductNumber)
                            DBCommon.SetProperty(DBReader("CodeExtension"), dc_RFQListHeader.CodeExtension)
                            DBCommon.SetProperty(DBReader("ProductName"), dc_RFQListHeader.ProductName)
                            DBCommon.SetProperty(DBReader("SupplierCode"), dc_RFQListHeader.SupplierCode)
                            DBCommon.SetProperty(DBReader("SupplierName"), dc_RFQListHeader.SupplierName)
                            DBCommon.SetProperty(DBReader("SupplierInfo"), dc_RFQListHeader.SupplierInfo)
                            DBCommon.SetProperty(DBReader("MakerCountryCode"), dc_RFQListHeader.MakerCountryCode)
                            DBCommon.SetProperty(DBReader("MakerCountryName"), dc_RFQListHeader.MakerCountryName)
                            DBCommon.SetProperty(DBReader("Purpose"), dc_RFQListHeader.Purpose)
                            DBCommon.SetProperty(DBReader("MakerName"), dc_RFQListHeader.MakerName)
                            DBCommon.SetProperty(DBReader("MakerInfo"), dc_RFQListHeader.MakerInfo)
                            DBCommon.SetProperty(DBReader("SupplierCountryCode"), dc_RFQListHeader.SupplierCountryCode)
                            DBCommon.SetProperty(DBReader("SupplierCountryName"), dc_RFQListHeader.SupplierCountryName)
                            DBCommon.SetProperty(DBReader("SupplierItemName"), dc_RFQListHeader.SupplierItemName)
                            DBCommon.SetProperty(DBReader("ShippingHandlingCurrencyCode"), dc_RFQListHeader.ShippingHandlingCurrencyCode)
                            DBCommon.SetProperty(DBReader("ShippingHandlingFee"), dc_RFQListHeader.ShippingHandlingFee)
                            DBCommon.SetProperty(DBReader("EnqUserName"), dc_RFQListHeader.EnqUserName)
                            DBCommon.SetProperty(DBReader("EnqLocationName"), dc_RFQListHeader.EnqLocationName)
                            DBCommon.SetProperty(DBReader("QuoUserName"), dc_RFQListHeader.QuoUserName)
                            DBCommon.SetProperty(DBReader("QuoLocationName"), dc_RFQListHeader.QuoLocationName)
                            DBCommon.SetProperty(DBReader("Comment"), dc_RFQListHeader.Comment)
                            DBCommon.SetProperty(DBReader("isCONFIDENTIAL"), dc_RFQListHeader.isCONFIDENTIAL)

                            Me.RFQNumber = dc_RFQListHeader.RFQNumber
                            GetRFQLine()
                            dc_RFQListHeader.RFQLineList = Me.RFQLine

                            Me.RFQListHeader.Add(dc_RFQListHeader)
                        End While
                    End Using
                End Using
            End Using

        End Sub

        ''' <summary>
        ''' RFQヘッダー検索SQL文字列を生成します。
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Friend Function CreateRFQHeaderSelectSQL() As String

            Dim sb_SQL As New Text.StringBuilder

            'SQL文字列の作成
            sb_SQL.AppendLine("SELECT ")
            sb_SQL.AppendLine("    RFH.[StatusChangeDate], ")
            sb_SQL.AppendLine("    RFH.[Status], ")
            sb_SQL.AppendLine("    RFH.[RFQNumber], ")
            sb_SQL.AppendLine("    ISNULL(RFH.[Priority], '') AS Priority, ")
            sb_SQL.AppendLine("    RFH.[QuotedDate], ")
            sb_SQL.AppendLine("    RFH.[ProductNumber], ")
            sb_SQL.AppendLine("    RFH.[CodeExtensionCode], ")
            sb_SQL.AppendLine("    RFH.[ProductName], ")
            sb_SQL.AppendLine("    RFH.[SupplierCode], ")
            sb_SQL.AppendLine("    RFH.[SupplierName], ")
            sb_SQL.AppendLine("    RFH.[SupplierInfo], ")
            sb_SQL.AppendLine("    RFH.[MakerCountryCode], ")
            sb_SQL.AppendLine("    MCRY.[Name] AS MakerCountryName, ")
            sb_SQL.AppendLine("    RFH.[Purpose], ")
            sb_SQL.AppendLine("    RFH.[MakerName], ")
            sb_SQL.AppendLine("    RFH.[MakerInfo], ")
            sb_SQL.AppendLine("    RFH.[SupplierCountryCode], ")
            sb_SQL.AppendLine("    SCRY.[Name] AS SupplierCountryName, ")
            sb_SQL.AppendLine("    RFH.[SupplierItemName], ")
            sb_SQL.AppendLine("    RFH.[ShippingHandlingCurrencyCode], ")
            sb_SQL.AppendLine("    RFH.[ShippingHandlingFee], ")
            sb_SQL.AppendLine("    RFH.[EnqUserName], ")
            sb_SQL.AppendLine("    RFH.[EnqLocationName], ")
            sb_SQL.AppendLine("    RFH.[QuoUserName], ")
            sb_SQL.AppendLine("    RFH.[QuoLocationName], ")
            sb_SQL.AppendLine("    RFH.[Comment], ")
            sb_SQL.AppendLine("    RFH.[isCONFIDENTIAL] ")
            sb_SQL.AppendLine("FROM ")
            sb_SQL.AppendLine("    [v_RFQHeader] AS RFH ")
            sb_SQL.AppendLine("  INNER JOIN [s_Country] AS SCRY ON RFH.[SupplierCountryCode] = scry.[CountryCode] ")
            sb_SQL.AppendLine("  LEFT JOIN [s_Country] AS MCRY ON RFH.[MakerCountryCode] = mcry.[CountryCode] ")
            sb_SQL.AppendLine("WHERE ")
            sb_SQL.AppendLine("    RFH.[ProductID] = @ProductID ")
            Select Case ValidityQuotation
                Case "Y"
                    sb_SQL.AppendLine("	 AND RFH.[ValidQuotation] = 1 ")
                Case "N"
                    sb_SQL.AppendLine("	 AND RFH.[ValidQuotation] = 0 ")
            End Select
            sb_SQL.AppendLine("ORDER BY ")
            sb_SQL.AppendLine("    RFH.[StatusSortOrder] ASC, ")
            sb_SQL.AppendLine("    RFH.[QuotedDate] DESC, ")
            sb_SQL.AppendLine("    RFH.[StatusChangeDate] DESC, ")
            sb_SQL.AppendLine("    RFH.[RFQNumber] ASC")

            Return sb_SQL.ToString()

        End Function

        Protected Friend Sub GetRFQLine()

            Dim sb_SQL As New Text.StringBuilder

            'SQL文字列の作成
            sb_SQL.AppendLine("SELECT DISTINCT ")
            sb_SQL.AppendLine("	 RFL.[RFQNumber], ")
            sb_SQL.AppendLine("	 RFL.[RFQLineNumber], ")
            sb_SQL.AppendLine("	 RFL.[EnqQuantity], ")
            sb_SQL.AppendLine("	 RFL.[EnqUnitCode], ")
            sb_SQL.AppendLine("	 RFL.[EnqPiece], ")
            sb_SQL.AppendLine("	 RFL.[CurrencyCode], ")
            sb_SQL.AppendLine("	 RFL.[UnitPrice], ")
            sb_SQL.AppendLine("	 RFL.[QuoPer], ")
            sb_SQL.AppendLine("	 RFL.[QuoUnitCode], ")
            sb_SQL.AppendLine("	 RFL.[LeadTime], ")
            sb_SQL.AppendLine("	 RFL.[Packing], ")
            sb_SQL.AppendLine("	 RFL.[Purity], ")
            sb_SQL.AppendLine("	 RFL.[QMMethod], ")
            sb_SQL.AppendLine("	 RFL.[SupplierOfferNo], ")
            sb_SQL.AppendLine("	 RFL.[SupplierItemNumber], ")
            sb_SQL.AppendLine("	 RFL.[NoOfferReason], ")
            sb_SQL.AppendLine("	 PO.[RFQLineNumber] AS PO, ")
            sb_SQL.AppendLine("	 CASE ")
            sb_SQL.AppendLine("	   WHEN PO.[Priority] = 'C' THEN '' ")
            sb_SQL.AppendLine("	   ELSE PO.[Priority] ")
            sb_SQL.AppendLine("	 END AS Priority ")
            sb_SQL.AppendLine("FROM  ")
            sb_SQL.AppendLine("	 [v_RFQLine] RFL ")
            sb_SQL.AppendLine("	 LEFT OUTER JOIN (")
            sb_SQL.AppendLine("	   SELECT ")
            sb_SQL.AppendLine("	     PO.[RFQLineNumber], ")
            sb_SQL.AppendLine("	     MIN( CASE ")
            sb_SQL.AppendLine("	            WHEN PO.[QMStartingDate] Is Not NULL Or PO.[QMFinishDate] Is Not NULL THEN 'C' ")
            sb_SQL.AppendLine("	            ELSE ISNULL(PO.[Priority], 'C') ")
            sb_SQL.AppendLine("	          END ) AS Priority ")
            sb_SQL.AppendLine("	   FROM ")
            sb_SQL.AppendLine("	     [PO] AS PO ")
            sb_SQL.AppendLine("	   GROUP BY PO.[RFQLineNumber] ")
            sb_SQL.AppendLine("	 ) AS PO ")
            sb_SQL.AppendLine("	   ON RFL.[RFQLineNumber] = PO.[RFQLineNumber] ")
            sb_SQL.AppendLine("WHERE ")
            sb_SQL.AppendLine("	 RFL.[RFQNumber] = @RFQNumber ")
            Select Case ValidityQuotation
                Case "Valid Price"
                    sb_SQL.AppendLine("	 AND RFH.[ValidQuotation] = 1 ")
                Case "Valid Price"
                    sb_SQL.AppendLine("	 AND RFH.[ValidQuotation] = 0 ")
            End Select

            Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.Parameters.Clear()
                    If Me.RFQNumber <> 0 Then
                        DBCommand.Parameters.AddWithValue("RFQNumber", Me.RFQNumber)
                    End If
                    DBCommand.CommandText = sb_SQL.ToString()

                    ' 実行
                    DBConn.Open()
                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader()
                        While DBReader.Read
                            Dim dc_RFQListByProductRFQList As RFQListByProductRFQLine = New RFQListByProductRFQLine

                            DBCommon.SetProperty(DBReader("RFQNumber"), dc_RFQListByProductRFQList.RFQNumber)
                            DBCommon.SetProperty(DBReader("RFQLineNumber"), dc_RFQListByProductRFQList.RFQLineNumber)
                            DBCommon.SetProperty(DBReader("EnqQuantity"), dc_RFQListByProductRFQList.EnqQuantity)
                            DBCommon.SetProperty(DBReader("EnqUnitCode"), dc_RFQListByProductRFQList.EnqUnitCode)
                            DBCommon.SetProperty(DBReader("EnqPiece"), dc_RFQListByProductRFQList.EnqPiece)
                            DBCommon.SetProperty(DBReader("CurrencyCode"), dc_RFQListByProductRFQList.CurrencyCode)
                            DBCommon.SetProperty(DBReader("UnitPrice"), dc_RFQListByProductRFQList.UnitPrice)
                            DBCommon.SetProperty(DBReader("QuoPer"), dc_RFQListByProductRFQList.QuoPer)
                            DBCommon.SetProperty(DBReader("QuoUnitCode"), dc_RFQListByProductRFQList.QuoUnitCode)
                            DBCommon.SetProperty(DBReader("LeadTime"), dc_RFQListByProductRFQList.LeadTime)
                            DBCommon.SetProperty(DBReader("Packing"), dc_RFQListByProductRFQList.Packing)
                            DBCommon.SetProperty(DBReader("Purity"), dc_RFQListByProductRFQList.Purity)
                            DBCommon.SetProperty(DBReader("QMMethod"), dc_RFQListByProductRFQList.QMMethod)
                            DBCommon.SetProperty(DBReader("SupplierOfferNo"), dc_RFQListByProductRFQList.SupplierOfferNo)
                            DBCommon.SetProperty(DBReader("SupplierItemNumber"), dc_RFQListByProductRFQList.SupplierItemNumber)
                            DBCommon.SetProperty(DBReader("NoOfferReason"), dc_RFQListByProductRFQList.NoOfferReason)
                            DBCommon.SetProperty(DBReader("PO"), dc_RFQListByProductRFQList.PO)
                            DBCommon.SetProperty(DBReader("Priority"), dc_RFQListByProductRFQList.Priority)

                            Me.RFQLine.Add(dc_RFQListByProductRFQList)
                        End While
                    End Using
                End Using
            End Using

        End Sub

        ''' <summary>
        ''' RFQ詳細検索SQL文字列を生成します。
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Friend Function CreateRFQLineSelectSQL() As String

            Dim sb_SQL As New Text.StringBuilder

            'SQL文字列の作成
            sb_SQL.AppendLine("SELECT DISTINCT ")
            sb_SQL.AppendLine("    RL.[RFQNumber], ")
            sb_SQL.AppendLine("    RL.[RFQLineNumber], ")
            sb_SQL.AppendLine("    RL.[EnqQuantity], ")
            sb_SQL.AppendLine("    RL.[EnqUnitCode], ")
            sb_SQL.AppendLine("    RL.[EnqPiece], ")
            sb_SQL.AppendLine("    RL.[CurrencyCode], ")
            sb_SQL.AppendLine("    RL.[UnitPrice], ")
            sb_SQL.AppendLine("    RL.[QuoPer], ")
            sb_SQL.AppendLine("    RL.[QuoUnitCode], ")
            sb_SQL.AppendLine("    RL.[LeadTime], ")
            sb_SQL.AppendLine("    RL.[Packing], ")
            sb_SQL.AppendLine("    RL.[Purity], ")
            sb_SQL.AppendLine("    RL.[QMMethod], ")
            sb_SQL.AppendLine("    RL.[SupplierOfferNo], ")
            sb_SQL.AppendLine("    RL.[SupplierItemNumber], ")
            sb_SQL.AppendLine("    RL.[NoOfferReason], ")
            sb_SQL.AppendLine("    PO.[RFQLineNumber] AS PO, ")
            sb_SQL.AppendLine("    CASE ")
            sb_SQL.AppendLine("      WHEN PO.[Priority] = 'C' THEN '' ")
            sb_SQL.AppendLine("      ELSE PO.[Priority] ")
            sb_SQL.AppendLine("    END AS Priority ")
            sb_SQL.AppendLine("FROM ")
            sb_SQL.AppendLine("    [v_RFQLine] AS RL")
            sb_SQL.AppendLine("  LEFT OUTER JOIN ")
            sb_SQL.AppendLine("    (SELECT  ")
            sb_SQL.AppendLine("         [RFQLineNumber], ")
            sb_SQL.AppendLine("         MIN( ")
            sb_SQL.AppendLine("           CASE ")
            sb_SQL.AppendLine("             WHEN PO.[QMStartingDate] Is Not NULL Or PO.[QMFinishDate] Is Not NULL THEN 'C' ")
            sb_SQL.AppendLine("             ELSE ISNULL(PO.[Priority], 'C') ")
            sb_SQL.AppendLine("           END ")
            sb_SQL.AppendLine("         ) AS Priority ")
            sb_SQL.AppendLine("     FROM [PO] ")
            sb_SQL.AppendLine("     GROUP BY [RFQLineNumber]) AS PO ")
            sb_SQL.AppendLine("    ON RL.[RFQLineNumber] = PO.[RFQLineNumber] ")
            sb_SQL.AppendLine("WHERE ")
            sb_SQL.AppendLine("    RL.[RFQNumber] = @RFQNumber ")

            Return sb_SQL.ToString()

        End Function

    End Class

    Public Class RFQListByProducttHeader

        Protected _ProductNumber As String = String.Empty
        Protected _QuoName As String = String.Empty
        Protected _Name As String = String.Empty
        Protected _CASNumber As String = String.Empty
        Protected _MolecularFormula As String = String.Empty
        Protected _ProductWarning As String = String.Empty
        Protected _BUoM As String = String.Empty

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
        ''' QuoName を設定、または取得する 
        ''' </summary> 
        Public Property QuoName() As String
            Get
                Return _QuoName
            End Get
            Set(ByVal value As String)
                _QuoName = value
            End Set
        End Property

        ''' <summary> 
        ''' Name を設定、または取得する 
        ''' </summary> 
        Public Property Name() As String
            Get
                Return _Name
            End Get
            Set(ByVal value As String)
                _Name = value
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
        ''' MolecularFormula を設定、または取得する 
        ''' </summary> 
        Public Property MolecularFormula() As String
            Get
                Return _MolecularFormula
            End Get
            Set(ByVal value As String)
                _MolecularFormula = value
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
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

    End Class

    Public Class RFQListByProductRFQListHeader

        Protected _StatusChangeDate As String = String.Empty
        Protected _Status As String = String.Empty
        Protected _RFQNumber As Integer = 0
        Protected _Priority As String = String.Empty
        Protected _QuotedDate  As DateTime = New DateTime(0)
        Protected _ProductNumber As String = String.Empty
        Protected _CodeExtension As String = String.Empty
        Protected _ProductName As String = String.Empty
        Protected _SupplierCode As String = String.Empty
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
        Protected _ShippingHandlingFee As String = String.Empty
        Protected _EnqUserName As String = String.Empty
        Protected _EnqLocationName As String = String.Empty
        Protected _QuoUserName As String = String.Empty
        Protected _QuoLocationName As String = String.Empty
        Protected _Comment As String = String.Empty
        Protected _isCONFIDENTIAL As String = String.Empty
        Protected _RFQLineList As List(Of RFQListByProductRFQLine)

        ''' <summary> 
        ''' StatusChangeDate を設定、または取得する 
        ''' </summary> 
        Public Property StatusChangeDate() As String
            Get
                Return _StatusChangeDate
            End Get
            Set(ByVal value As String)
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
        Public Property QuotedDate() As DateTime
            Get
                Return _QuotedDate
            End Get
            Set(ByVal value As DateTime)
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
        ''' CodeExtension を設定、または取得する 
        ''' </summary> 
        Public Property CodeExtension() As String
            Get
                Return _CodeExtension
            End Get
            Set(ByVal value As String)
                _CodeExtension = value
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
        Public Property SupplierCode() As String
            Get
                Return _SupplierCode
            End Get
            Set(ByVal value As String)
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
        ''' </summary> 
        Public Property ShippingHandlingFee() As String
            Get
                Return _ShippingHandlingFee
            End Get
            Set(ByVal value As String)
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
        Public Property isCONFIDENTIAL() As String
            Get
                Return _isCONFIDENTIAL
            End Get
            Set(ByVal value As String)
                _isCONFIDENTIAL = value
            End Set
        End Property

        ''' <summary> 
        ''' RFQLineList を設定、または取得する 
        ''' </summary> 
        Public Property RFQLineList() As List(Of RFQListByProductRFQLine)
            Get
                Return _RFQLineList
            End Get
            Set(ByVal value As List(Of RFQListByProductRFQLine))
                _RFQLineList = value
            End Set
        End Property

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

    End Class
    Public Class RFQListByProductRFQLine

        Protected _RFQNumber As String = String.Empty
        Protected _RFQLineNumber As Integer = 0
        Protected _EnqQuantity As String = String.Empty
        Protected _EnqUnitCode As String = String.Empty
        Protected _EnqPiece As String = String.Empty
        Protected _CurrencyCode As String = String.Empty
        Protected _UnitPrice As String = String.Empty
        Protected _QuoPer As String = String.Empty
        Protected _QuoUnitCode As String = String.Empty
        Protected _LeadTime As String = String.Empty
        Protected _Packing As String = String.Empty
        Protected _Purity As String = String.Empty
        Protected _QMMethod As String = String.Empty
        Protected _SupplierOfferNo As String = String.Empty
        Protected _SupplierItemNumber As String = String.Empty
        Protected _NoOfferReason As String = String.Empty
        Protected _PO As String = String.Empty
        Protected _Priority As String = String.Empty

        ''' <summary> 
        ''' RFQNumber を設定、または取得する 
        ''' </summary> 
        Public Property RFQNumber() As String
            Get
                Return _RFQNumber
            End Get
            Set(ByVal value As String)
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
        Public Property EnqQuantity() As String
            Get
                Return _EnqQuantity
            End Get
            Set(ByVal value As String)
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
        Public Property EnqPiece() As String
            Get
                Return _EnqPiece
            End Get
            Set(ByVal value As String)
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
        ''' </summary> 
        Public Property UnitPrice() As String
            Get
                Return _UnitPrice
            End Get
            Set(ByVal value As String)
                _UnitPrice = value
            End Set
        End Property

        ''' <summary> 
        ''' QuoPer を設定、または取得する 
        ''' </summary> 
        Public Property QuoPer() As String
            Get
                Return _QuoPer
            End Get
            Set(ByVal value As String)
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
        ''' NoOfferReason を設定、または取得する 
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
        Public Property PO() As String
            Get
                Return _PO
            End Get
            Set(ByVal value As String)
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
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

    End Class
End Namespace

Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports System.Data.SqlClient   
Imports System.Text.RegularExpressions
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess.Join

    ''' <summary>
    ''' ReportOverview データクラス
    ''' </summary>
    Public Class Export_ProductListBySupplier
        Protected _ProductNumber As String = String.Empty
        Protected _ProductName As String = String.Empty
        Protected _SupplierItemNumber As String = String.Empty
        Protected _Note As String = String.Empty
        Protected _UpdateDate As String = String.Empty
        Protected _ValidQuotation As String = String.Empty

        ''' <summary> 
        ''' ProposalDeptCode  を設定、または取得する 
        ''' </summary> 
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
        Public Property SupplierItemNumber() As String
            Get
                Return _SupplierItemNumber
            End Get
            Set(ByVal value As String)
                _SupplierItemNumber = value
            End Set
        End Property
        Public Property Note() As String
            Get
                Return _Note
            End Get
            Set(ByVal value As String)
                _Note = value
            End Set
        End Property
        Public Property UpdateDate() As String
            Get
                Return _UpdateDate
            End Get
            Set(ByVal value As String)
                _UpdateDate = value
            End Set
        End Property

        Public Property ValidQuotation() As String
            Get
                Return _ValidQuotation
            End Get
            Set(ByVal value As String)
                _ValidQuotation = value
            End Set
        End Property

        ''' <summary> 
        ''' コンストラクタ
        ''' </summary> 
        Public Sub New()

        End Sub

    End Class

    ''' <summary>
    ''' ReportOverviewList データクラス
    ''' </summary>
    Public Class Export_ProductListBySupplierList
        Inherits List(Of Export_ProductListBySupplier)

        ''' <summary> 
        ''' コンストラクタ
        ''' </summary> 
        Public Sub New()

        End Sub

        ''' <summary>
        ''' 指定された条件の新製品データを提案元/指令・発注先ごとに、
        ''' 各ステータスごとの件数を集計したデータを読み込む
        ''' </summary>
        ''' <remarks></remarks>
        Public Sub Load(SupplierCode As String, SessionRole As Boolean, SorfField As String, SortType As String, FilterType As String)
            Dim sb_SQL As New StringBuilder()

            Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.Parameters.AddWithValue("SupplierCode", SupplierCode)

                    'SQL 生成
                    Dim strSql As StringBuilder = New StringBuilder
                    strSql.AppendLine("SELECT")
                    strSql.AppendLine("  P.[ProductID], ")
                    strSql.AppendLine("  P.[CASNumber], ")
                    strSql.AppendLine("  P.[ProductNumber], ")
                    strSql.AppendLine("  P.[NumberType], ")
                    strSql.AppendLine("  CASE WHEN NOT P.[QuoName] IS NULL THEN P.[QuoName] ELSE P.[Name] END AS ProductName, ")
                    strSql.AppendLine("  SP.[SupplierItemNumber], ")
                    strSql.AppendLine("  SP.[Note], ")
                    strSql.AppendLine("  SP.[UpdateDate], ")
                    strSql.AppendLine("  './SuppliersProductSetting.aspx?Action=Edit&Supplier= @SupplierCode &Product='+RTRIM(LTRIM(STR(P.[ProductID]))) AS Url, ")
                    strSql.AppendLine("  ISNULL(C.[isCONFIDENTIAL], 0) AS isCONFIDENTIAL, ")
                    strSql.AppendLine("  SP.[ValidQuotation] ")
                    strSql.AppendLine("FROM ")
                    strSql.AppendLine("  [Supplier_Product] AS SP ")
                    strSql.AppendLine("    LEFT OUTER JOIN [Product] AS P ON SP.[ProductID] = P.[ProductID] ")
                    strSql.AppendLine("    LEFT OUTER JOIN [v_CONFIDENTIAL] AS C ON C.[ProductID] = SP.[ProductID] ")
                    strSql.AppendLine("WHERE ")
                    strSql.AppendLine("  SP.[SupplierCode] =@SupplierCode")

                    '権限ロールに従い極秘品を除外する
                    If SessionRole = False Then
                        strSql.AppendLine("  AND C.[isCONFIDENTIAL] = 0")
                    End If

                    If FilterType = "Valid Price" Then
                        strSql.AppendLine("  AND SP.[ValidQuotation] = 'Y'")

                    ElseIf FilterType = "Invalid Price" Then
                        strSql.AppendLine("  AND SP.[ValidQuotation] = 'N'")

                    End If

                    If SorfField = "SupplierProductList_ProductNumHeader" Or String.IsNullOrEmpty(SorfField) Then
                        If SortType = "asc" Then
                            strSql.AppendLine("ORDER BY")
                            strSql.AppendLine("  CASE")
                            strSql.AppendLine("  WHEN")
                            strSql.AppendLine("  P.[NumberType] = 'CAS' THEN 1")
                            strSql.AppendLine("  WHEN")
                            strSql.AppendLine("  P.[NumberType] = 'NEW' THEN 2")
                            strSql.AppendLine("  ELSE 3")
                            strSql.AppendLine("  END,")
                            strSql.AppendLine("  P.[ProductNumber] ASC")
                        ElseIf SortType = "desc" Then
                            strSql.AppendLine("ORDER BY")
                            strSql.AppendLine("  CASE")
                            strSql.AppendLine("  WHEN")
                            strSql.AppendLine("  P.[NumberType] = 'TCI' THEN 1")
                            strSql.AppendLine("  WHEN")
                            strSql.AppendLine("  P.[NumberType] = 'NEW' THEN 2")
                            strSql.AppendLine("  ELSE 3")
                            strSql.AppendLine("  END,")
                            strSql.AppendLine("   P.[ProductNumber] ASC")
                        Else
                            strSql.AppendLine("ORDER BY")
                            strSql.AppendLine("  CASE")
                            strSql.AppendLine("  WHEN")
                            strSql.AppendLine("  P.[NumberType] = 'CAS THEN 1")
                            strSql.AppendLine("  WHEN")
                            strSql.AppendLine("  P.[NumberType] = 'NEW' THEN 2")
                            strSql.AppendLine("  ELSE 3")
                            strSql.AppendLine("  END,")
                            strSql.AppendLine("  P.[ProductNumber] ASC")
                        End If

                        'UpdateDateでのソート
                    ElseIf SorfField = "SupplierProductList_UpdateDateHeader" Then
                        strSql.AppendLine("ORDER BY")
                        strSql.AppendLine("  SP.[UpdateDate]")
                        If SortType = "asc" Then
                            strSql.AppendLine("  ASC")
                        ElseIf SortType = "desc" Then
                            strSql.AppendLine("  DESC")
                        Else
                            strSql.AppendLine("  ASC")
                        End If

                        'ValidQuotationでのソート
                    ElseIf SorfField = "SupplierProductList_ValidQuotationHeader" Then
                        strSql.AppendLine("ORDER BY")
                        strSql.AppendLine("SP.[Validquotation] ")
                        If SortType = "asc" Then
                            strSql.AppendLine("  ASC")

                        ElseIf SortType = "desc" Then
                            strSql.AppendLine("  DESC")
                        Else
                            strSql.AppendLine("  ASC")
                        End If
                    End If

                    DBCommand.CommandText = strSql.ToString

                    DBConn.Open()
                    Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
                    While DBReader.Read()
                        Dim dc_Data As New Export_ProductListBySupplier
                        SetProperty(DBReader("ProductNumber"), dc_Data.ProductNumber)
                        SetProperty(DBReader("ProductName"), dc_Data.ProductName)
                        SetProperty(DBReader("SupplierItemNumber"), dc_Data.SupplierItemNumber)
                        SetProperty(DBReader("Note"), dc_Data.Note)
                        SetProperty(DBReader("UpdateDate"), dc_Data.UpdateDate)
                        SetProperty(DBReader("ValidQuotation"), dc_Data.ValidQuotation)

                        Me.Add(dc_Data)
                    End While
                    DBReader.Close()
                End Using
            End Using

        End Sub

    End Class

End Namespace
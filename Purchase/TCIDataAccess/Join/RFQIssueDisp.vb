Option Explicit On
Option Strict On
Option Infer Off

Namespace TCIDataAccess.Join
    ''' <summary> 
    ''' ProductSearchDisp データクラス 
    ''' </summary> 
    Public Class RFQIssueDisp

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

        Public Function GetProductNumber(ByVal i_ProductID As Integer) As String 

            Dim product As Product = New Product
            product.Load(i_ProductID)

            Return product.ProductNumber

        End Function

        Public Function GetProductInfo(ByVal st_ProductNumber As String, ByVal st_RoleCode As String) As List(Of RFQIssueDispProductInfo)

            Dim product As Product = New Product
            Dim productInfo As List(Of RFQIssueDispProductInfo) = New List(Of RFQIssueDispProductInfo)
            productInfo = product.GetProductInfo(st_ProductNumber, st_RoleCode)

            Return productInfo

        End Function

        Public Function GetSupplierInfo(ByVal st_SupplierCode As String, ByVal st_RoleCode As String) As List(Of RFQIssueDispSupplierInfo)

            Dim supplier As Supplier = New Supplier
            Dim supplierInfo As List(Of RFQIssueDispSupplierInfo) = New List(Of RFQIssueDispSupplierInfo)
            supplierInfo = supplier.GetSupplierInfo(st_SupplierCode, st_RoleCode)

            Return supplierInfo

        End Function

        Public Function GetMakerInfo(ByVal st_MakerCode As String, ByVal st_RoleCode As String) As List(Of RFQIssueDispMakerInfo)

            Dim supplier As Supplier = New Supplier
            Dim makerInfo As List(Of RFQIssueDispMakerInfo) = New List(Of RFQIssueDispMakerInfo)
            makerInfo = supplier.GetMakerInfo(st_MakerCode, st_RoleCode)

            Return makerInfo

        End Function
    End Class

    Public Class RFQIssueDispProductInfo

        Protected _ProductID As Integer = 0
        Protected _ProductNumber As String = String.Empty 
        Protected _ProductName As String = String.Empty 
        Protected _CASNumber As String = String.Empty 

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
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub
    End Class

    Public Class RFQIssueDispSupplierInfo

        Protected _S4SupplierCode As String = String.Empty 
        Protected _SupplierCode As Integer = 0
        Protected _R3SupplierCode As String = String.Empty
        Protected _CountryCode As String = String.Empty
        Protected _Name As String = String.Empty
        Protected _CountryName As String = String.Empty
        Protected _QuoLocationCode As String = String.Empty
        Protected _QuoLocationName As String = String.Empty

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
        ''' CountryCode を設定、または取得する 
        ''' </summary> 
        Public Property CountryCode() As String
            Get
                Return _CountryCode
            End Get
            Set(ByVal value As String)
                _CountryCode = value
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
        ''' CountryName を設定、または取得する 
        ''' </summary> 
        Public Property CountryName() As String
            Get
                Return _CountryName
            End Get
            Set(ByVal value As String)
                _CountryName = value
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
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub
    End Class

    Public Class RFQIssueDispMakerInfo

        Protected _S4SupplierCode As String = String.Empty 
        Protected _SupplierCode As Integer = 0
        Protected _LocationCode As String = String.Empty
        Protected _CountryName As String = String.Empty
        Protected _Name As String = String.Empty

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
        ''' LocationCode を設定、または取得する 
        ''' </summary> 
        Public Property LocationCode() As String
            Get
                Return _LocationCode
            End Get
            Set(ByVal value As String)
                _LocationCode = value
            End Set
        End Property

        ''' <summary> 
        ''' CountryName を設定、または取得する 
        ''' </summary> 
        Public Property CountryName() As String
            Get
                Return _CountryName
            End Get
            Set(ByVal value As String)
                _CountryName = value
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
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub
    End Class

End Namespace

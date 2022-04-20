Imports System.Data.SqlClient

Namespace TCIDataAccess.Join

    ''' <summary> 
    ''' ProductListBySupplierDisp データクラス 
    ''' </summary> 
    Public Class ProductListBySupplierDisp

        Protected _ProductNumber As String = String.Empty
        Protected _CASNumber As String = String.Empty
        Protected _ProductName As String = String.Empty
        Protected _SupplierItemNumber As String = String.Empty
        Protected _Note As String = String.Empty
        Protected _ValidQuotation As String = String.Empty
        Protected _UpdateDate As String = String.Empty
        Protected _Url As String = String.Empty
        Protected _ProductID As String = String.Empty

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
        ''' Note を設定、または取得する 
        ''' </summary> 
        Public Property Note() As String
            Get
                Return _Note
            End Get
            Set(ByVal value As String)
                _Note = value
            End Set
        End Property

        ''' <summary> 
        ''' ValidQuotation を設定、または取得する 
        ''' </summary> 
        Public Property ValidQuotation() As String
            Get
                Return _ValidQuotation
            End Get
            Set(ByVal value As String)
                _ValidQuotation = value
            End Set
        End Property

        ''' <summary> 
        ''' UpdateDate を設定、または取得する 
        ''' </summary> 
        Public Property UpdateDate() As String
            Get
                Return _UpdateDate
            End Get
            Set(ByVal value As String)
                _UpdateDate = value
            End Set
        End Property

        ''' <summary> 
        ''' Url を設定、または取得する 
        ''' </summary> 
        Public Property Url() As String
            Get
                Return _Url
            End Get
            Set(ByVal value As String)
                _Url = value
            End Set
        End Property

        ''' <summary> 
        ''' ProductID を設定、または取得する 
        ''' </summary> 
        Public Property ProductID() As String
            Get
                Return _ProductID
            End Get
            Set(ByVal value As String)
                _ProductID = value
            End Set
        End Property

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub
    End Class

    ''' <summary> 
    ''' ProductListBySupplierのHeader情報 データクラス 
    ''' </summary> 
    Public Class ProductListBySupplierDispHeader

        Protected _Name3 As String = String.Empty
        Protected _Name4 As String = String.Empty
        Protected _Territory As String = String.Empty

        ''' <summary> 
        ''' Name3 を設定、または取得する 
        ''' </summary> 
        Public Property Name3() As String
            Get
                Return _Name3
            End Get
            Set(ByVal value As String)
                _Name3 = value
            End Set
        End Property

        ''' <summary> 
        ''' Name4 を設定、または取得する 
        ''' </summary> 
        Public Property Name4() As String
            Get
                Return _Name4
            End Get
            Set(ByVal value As String)
                _Name4 = value
            End Set
        End Property

        ''' <summary> 
        ''' Territory を設定、または取得する 
        ''' </summary> 
        Public Property Territory() As String
            Get
                Return _Territory
            End Get
            Set(ByVal value As String)
                _Territory = value
            End Set
        End Property

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub
    End Class

    ''' <summary> 
    ''' ProductListBySupplierDispList データクラス 
    ''' </summary> 
    ''' <remarks>
    ''' </remarks>

    Public Class ProductListBySupplierDispList
        Inherits List(Of ProductListBySupplierDisp)

        Protected _SupplierName As String = String.Empty
        Protected _Territory As String = String.Empty

        Protected _ProductListBySupplierList As List(Of ProductListBySupplierDisp)

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
        ''' Territory を設定、または取得する 
        ''' </summary> 
        Public Property Territory() As String
            Get
                Return _Territory
            End Get
            Set(ByVal value As String)
                _Territory = value
            End Set
        End Property

        ''' <summary> 
        ''' ProductListBySupplierList を設定、または取得する 
        ''' </summary> 
        Public Property ProductListBySupplierList As List(Of ProductListBySupplierDisp)
            Get
                Return _ProductListBySupplierList
            End Get
            Set(ByVal value As  List(Of ProductListBySupplierDisp))
                _ProductListBySupplierList = value
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
        ''' <param name="st_SupplierCode">SupplierCode</param>
        Public Sub Load(ByVal st_SupplierCode As String, ByVal st_RoleCode As String, ByVal st_ValidFilter As String, _
                        ByVal st_SupplierProductListID As String, ByVal st_HiddenSortField As String, ByVal st_HiddenSortType As String)
            Dim productListBySupplierDisp As List(Of ProductListBySupplierDisp) = New List(Of ProductListBySupplierDisp)

            ' Header情報取得
            Dim supplier As Supplier = New Supplier
            supplier.Load(Integer.Parse(st_SupplierCode))

            If Not String.IsNullOrEmpty(supplier.Name3) Then me.SupplierName = supplier.Name3
            If Not String.IsNullOrEmpty(supplier.Name4) Then
                If me.SupplierName = String.Empty Then
                    me.SupplierName = supplier.Name4
                Else
                    me.SupplierName = me.SupplierName & Space(1) & supplier.Name4
                End If
            End If

            Dim v_Territory As v_Territory = New v_Territory
            Dim territories As List(Of v_Territory) = New List(Of v_Territory)
            territories = v_Territory.Load(st_SupplierCode)
            For  Each territory As v_Territory In territories
                If Not String.IsNullOrEmpty(territory.TerritoryName) Then 
                    Me.Territory = territory.TerritoryName
                End If
            Next

            ' 一覧情報取得
            Dim supplierProduct As Supplier_Product = New Supplier_Product
            productListBySupplierDisp = supplierProduct.GetProductListBySupplierList(st_SupplierCode, st_RoleCode, st_ValidFilter, _
                                                                                     st_SupplierProductListID, st_HiddenSortField, st_HiddenSortType)

            Me.ProductListBySupplierList = productListBySupplierDisp
        End Sub

    
    End Class

End Namespace


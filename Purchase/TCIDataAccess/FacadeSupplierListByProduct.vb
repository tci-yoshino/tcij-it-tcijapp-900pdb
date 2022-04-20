Imports System.Data.SqlClient

Namespace TCIDataAccess

    ''' <summary> 
    ''' FacadeSupplierListByProduct データクラス 
    ''' </summary> 
    Public Class FacadeSupplierListByProduct

        Protected _SupplierCode As Integer = 0
        Protected _ProductID As Integer = 0

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
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' SupplierCodeとProductID を元に Supplier_Product から削除する
        ''' </summary>
        Public Sub Delete()

            Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                ' トランザクション開始※Using内はCommitのみでOK RollBackは不要
                DBConn.Open
                Using sqlTran As SqlTransaction = DBConn.BeginTransaction()
                    Using DBCommand As SqlCommand = DBConn.CreateCommand()

                        Dim supplier_Product As Supplier_Product = New Supplier_Product

                        ' RFQNumberを追加でセットする
                        supplier_Product.Delete(Me.SupplierCode, Me.ProductID)

                        ' コミット
                        sqlTran.Commit()

                    End Using
                End Using
            End Using

        End Sub
    End Class

End Namespace


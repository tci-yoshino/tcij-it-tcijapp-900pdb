Option Explicit On
Option Strict On
Option Infer Off

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Text
Imports System.Data.SqlClient
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess.Join
    Public Class StructureSearchDisp
        Protected _pStructure As Byte()
        Protected _Similarity As String = String.Empty
        Protected _ProductNumber As String = String.Empty
        Protected _CASNumber As String = String.Empty
        Protected _ProductName As String = String.Empty
        Protected _ProductID As String = String.Empty

        Public Property pStructure() As Byte()
            Get
                Return _pStructure
            End Get
            Set(ByVal value As Byte())
                _pStructure = value
            End Set
        End Property
        Public Property Similarity() As String
            Get
                Return _Similarity
            End Get
            Set(ByVal value As String)
                _Similarity = value
            End Set
        End Property
        Public Property ProductNumber() As String
            Get
                Return _ProductNumber
            End Get
            Set(ByVal value As String)
                _ProductNumber = value
            End Set
        End Property
        Public Property CASNumber() As String
            Get
                Return _CASNumber
            End Get
            Set(ByVal value As String)
                _CASNumber = value
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
        Public Property ProductID As String
            Get
                Return _ProductID
            End Get
            Set(ByVal value As String)
                _ProductID = value
            End Set
        End Property

        Public Shared Widening Operator CType(v As ProductSearchByStructure) As StructureSearchDisp
            Throw New NotImplementedException()
        End Operator

    End Class

    Public Class StructureSearchDispList
        Inherits List(Of StructureSearchDisp)

        Public Sub Load(ByVal st_RegistryNumber As String, ByVal st_Similarity As String, ByVal rollCode As String, ByRef bln_Result As Boolean)

            Dim sbValue As StringBuilder = New StringBuilder
            sbValue.AppendLine("Select ")
            sbValue.AppendLine("    [Product].[ProductNumber],")
            sbValue.AppendLine("    [Product].[CASNumber],")
            sbValue.AppendLine("    (CASE WHEN [Product].[QuoName] IS NULL THEN [Product].[Name] ELSE [Product].[QuoName ]END) As ProductName,")
            sbValue.AppendLine("    [Product].[ProductID]")
            sbValue.AppendLine("From s_NewProduct")
            sbValue.AppendLine("    Inner Join Product On")
            sbValue.AppendLine("        [s_NewProduct].[ProductNumber] = [Product].[ProductNumber]")
            sbValue.AppendLine("Where")
            sbValue.AppendLine("    [s_NewProduct].[RegistryNumber] = @RegistryNumber AND ")
            sbValue.AppendLine("    [Product].[NumberType] = 'TCI'")
            sbValue.AppendLine("union")
            sbValue.AppendLine("Select ")
            sbValue.AppendLine("    [Product].[ProductNumber],")
            sbValue.AppendLine("    [Product].[CASNumber],")
            sbValue.AppendLine("    (CASE WHEN [Product].[QuoName] IS NULL THEN [Product].[Name] ELSE [Product].[QuoName ]END) As ProductName,")
            sbValue.AppendLine("    [Product].[ProductID]")
            sbValue.AppendLine("From s_NewProduct")
            sbValue.AppendLine("    Inner Join Product On")
            sbValue.AppendLine("        [s_NewProduct].[ProductNumber] = [Product].[ProductNumber]")
            sbValue.AppendLine("Where")
            sbValue.AppendLine("    [s_NewProduct].[RegistryNumber] = @RegistryNumber AND ")
            sbValue.AppendLine("    Product.NumberType = 'NEW'")

            If rollCode = Purchase.Common.ROLE_WRITE_P OrElse rollCode = Purchase.Common.ROLE_READ_P Then
                sbValue.AppendLine("  AND NOT EXISTS (")
                sbValue.AppendLine("    SELECT 1")
                sbValue.AppendLine("    FROM")
                sbValue.AppendLine("      v_CONFIDENTIAL AS C")
                sbValue.AppendLine("    WHERE")
                sbValue.AppendLine("      C.[isCONFIDENTIAL] = 1")
                sbValue.AppendLine("      AND C.[ProductID] = P.[ProductID]")
                sbValue.AppendLine("  )")
            End If

            Using DBConn As SqlConnection = New SqlConnection(DBCommon.DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand
                    DBCommand.CommandText = sbValue.ToString
                    DBCommand.Parameters.Clear()

                    '絞り込み条件：ProductIDバインド変数設定
                    DBCommand.Parameters.AddWithValue("RegistryNumber", st_RegistryNumber.ToString)

                    DBConn.Open()
                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader
                        bln_Result = False
                        If DBReader.Read Then
                            Dim _pStructure As Byte() = {}
                            Dim _Similarity As String = String.Empty
                            Dim _ProductNumber As String = String.Empty
                            Dim _CASNumber As String = String.Empty
                            Dim _ProductName As String = String.Empty
                            Dim _ProductID As String = String.Empty

                            Dim dc_StructureSearch As StructureSearchDisp = New StructureSearchDisp
                            DBCommon.SetProperty(DBReader("ProductNumber"), _ProductNumber)
                            DBCommon.SetProperty(DBReader("CASNumber"), _CASNumber)
                            DBCommon.SetProperty(DBReader("ProductName"), _ProductName)
                            DBCommon.SetProperty(DBReader("ProductID"), _ProductID)

                            dc_StructureSearch.pStructure = _pStructure
                            dc_StructureSearch.Similarity = _Similarity
                            dc_StructureSearch.ProductNumber = _ProductNumber
                            dc_StructureSearch.CASNumber = _CASNumber
                            dc_StructureSearch.ProductName = _ProductName
                            dc_StructureSearch.ProductID = _ProductID

                            Me.Add(dc_StructureSearch)
                            bln_Result = True
                        End If
                    End Using
                End Using
            End Using
        End Sub

    End Class

End Namespace

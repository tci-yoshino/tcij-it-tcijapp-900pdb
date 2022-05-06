Option Explicit On
Option Strict On
Option Infer Off

Imports System.Collections.Generic
Imports System.Data.SqlClient

Namespace TCIDataAccess.Join
    ''' <summary> 
    ''' ProductSearchByKeywordDisp データクラス 
    ''' </summary> 
    Public Class ProductSearchByKeywordDisp

        Protected _ProductNumber As String = String.Empty
        Protected _CASNumber As String = String.Empty
        Protected _Name As String = String.Empty
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
    ''' DspProductNameSearchList データクラス 
    ''' </summary> 
    ''' <remarks>
    ''' </remarks>

    Public Class ProductSearchByKeywordDispList
        Inherits List(Of ProductSearchByKeywordDisp)

        Protected _ProductNumber As String = String.Empty
        Protected _Name As String = String.Empty

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

        ''' <summary>
        ''' データベースからデータを読み込む。
        ''' </summary>
        ''' <param name="st_RoleCode">role</param>
        Public Sub Load(ByVal st_RoleCode As String)
            'データベースからデータを読み込む SQL 文字列を生成する。
            Dim sb_SQL As StringBuilder = New StringBuilder

            sb_SQL.AppendLine("SELECT ")
            sb_SQL.AppendLine("  P.[ProductNumber], ")
            sb_SQL.AppendLine("  P.[CASNumber], ")
            sb_SQL.AppendLine("  CASE WHEN NOT P.[QuoName] IS NULL THEN P.[QuoName] ELSE P.[Name] END AS Name, ")
            sb_SQL.AppendLine("  P.[ProductID] ")
            sb_SQL.AppendLine("FROM ")
            sb_SQL.AppendLine("  [Product] AS P")

            sb_SQL.AppendLine("WHERE ")
            If (Not String.IsNullOrEmpty(Me.ProductNumber)) Then
                sb_SQL.AppendLine("  P.[ProductNumber] = @ProductNumber ")
            End If
            If (String.IsNullOrEmpty(Me.ProductNumber)) And (Not String.IsNullOrEmpty(Me.Name)) Then
                sb_SQL.AppendLine("  ( ")
                sb_SQL.AppendLine("  P.[Name] LIKE '%' + @Name + '%' ")
                sb_SQL.AppendLine("  OR P.[QuoName] LIKE '%' + @Name + '%' ")
                sb_SQL.AppendLine("  OR EXISTS ( ")
                sb_SQL.AppendLine("    SELECT 1 ")
                sb_SQL.AppendLine("    FROM ")
                sb_SQL.AppendLine("      [Supplier_Product] AS SP ")
                sb_SQL.AppendLine("	   WHERE ")
                sb_SQL.AppendLine("      SP.[ProductID] = P.[ProductID] ")
                sb_SQL.AppendLine("      AND SP.[Note] LIKE '%' + @Name + '%' ")
                sb_SQL.AppendLine("    ) ")
                sb_SQL.AppendLine("  ) ")

            ElseIf (Not String.IsNullOrEmpty(Me.ProductNumber)) And (Not String.IsNullOrEmpty(Me.Name)) Then
                sb_SQL.AppendLine("  AND  ( ")
                sb_SQL.AppendLine("    P.[Name] LIKE '%' + @Name + '%' ")
                sb_SQL.AppendLine("    OR P.[QuoName] LIKE '%' + @Name + '%' ")
                sb_SQL.AppendLine("    OR EXISTS ( ")
                sb_SQL.AppendLine("      SELECT 1 ")
                sb_SQL.AppendLine("      FROM ")
                sb_SQL.AppendLine("        [Supplier_Product] AS SP ")
                sb_SQL.AppendLine("	     WHERE ")
                sb_SQL.AppendLine("        SP.[ProductID] = P.[ProductID] ")
                sb_SQL.AppendLine("        AND SP.[Note] LIKE '%' + @Name + '%' ")
                sb_SQL.AppendLine("      ) ")
                sb_SQL.AppendLine("    ) ")
            End If

            '権限ロールに従い極秘品を除外する
            If Common.CheckSessionRole(st_RoleCode) = False Then
                sb_SQL.AppendLine("  AND ")
                sb_SQL.AppendLine("    Not EXISTS (")
                sb_SQL.AppendLine("        SELECT 1")
                sb_SQL.AppendLine("            FROM")
                sb_SQL.AppendLine("                [v_CONFIDENTIAL] As C")
                sb_SQL.AppendLine("            WHERE")
                sb_SQL.AppendLine("                C.[isCONFIDENTIAL] = 1 AND ")
                sb_SQL.AppendLine("                C.[ProductID] = P.[ProductID]")
                sb_SQL.AppendLine("        )")
            End If

            sb_SQL.AppendLine("ORDER BY ")
            sb_SQL.AppendLine("    CASE ")
            sb_SQL.AppendLine("        WHEN P.[NumberType] = 'TCI' THEN 1 ")
            sb_SQL.AppendLine("        WHEN P.[NumberType] = 'NEW' THEN 2 ")
            sb_SQL.AppendLine("        ELSE 3 ")
            sb_SQL.AppendLine("    END, ")
            sb_SQL.AppendLine("    P.[ProductNumber] ASC ")

            Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.Parameters.Clear()
                    If Not String.IsNullOrEmpty(Me.Name) Then
                        DBCommand.Parameters.AddWithValue("Name", Me.Name)
                    End If
                    If Not String.IsNullOrEmpty(Me.ProductNumber) Then
                        DBCommand.Parameters.AddWithValue("ProductNumber", Me.ProductNumber)
                    End If
                    DBCommand.CommandText = sb_SQL.ToString()

                    ' 実行
                    DBConn.Open()
                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader()
                        While DBReader.Read
                            Dim dc_DspProductNameSearch As ProductSearchByKeywordDisp = New ProductSearchByKeywordDisp

                            DBCommon.SetProperty(DBReader("ProductNumber"), dc_DspProductNameSearch.ProductNumber)
                            DBCommon.SetProperty(DBReader("CASNumber"), dc_DspProductNameSearch.CASNumber)
                            DBCommon.SetProperty(DBReader("Name"), dc_DspProductNameSearch.Name)
                            DBCommon.SetProperty(DBReader("ProductID"), dc_DspProductNameSearch.ProductID)

                            Me.Add(dc_DspProductNameSearch)
                        End While
                    End Using
                End Using
            End Using
        End Sub
    End Class

End Namespace
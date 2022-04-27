Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Text
Imports System.Data.SqlClient
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess.Join

    Public Class EhsHeader_Personalize_Sort

        Private _Item As String

        ''' <summary> 
        ''' Item を設定、または取得する 
        ''' </summary> 
        Public Property Item As String
            Get
                Return Me._Item
            End Get
            Set
                Me._Item = Value
            End Set
        End Property

        ''' <summary> 
        ''' コンストラクタ
        ''' </summary> 
        Public Sub New()
            MyBase.New
            Me._Item = String.Empty
        End Sub
    End Class

    ''' <summary>
    ''' EhsHeader_Personalize_Sort リストクラス 
    ''' </summary> 
    Public Class EhsHeader_Personalize_SortList
        Inherits List(Of EhsHeader_Personalize_Sort)

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()
            MyBase.New

        End Sub

        ''' <summary>
        ''' データベースからユーザ固有のデータを読み込む（拠点ソート）
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <param name="locationCode"></param>
        Public Sub Load(ByVal userID As Integer, ByVal locationCode As String)
            'データベースから全てのデータを読み込む SQL 文字列を生成する
            Dim Value As StringBuilder = New StringBuilder
            Value.AppendLine("SELECT")
            Value.AppendLine("    EHP.[Item]")
            Value.AppendLine("FROM ")
            Value.AppendLine("    [EhsHeader_Personalize] AS EHP")
            Value.AppendLine("INNER JOIN")
            Value.AppendLine("    [s_EhsHeader] AS EH")
            Value.AppendLine("ON")
            Value.AppendLine("    EHP.Item = EH.Item")
            Value.AppendLine("WHERE")
            Value.AppendLine("    UserID = @userID")
            Value.AppendLine("ORDER BY")
            Value.AppendLine("    CASE [LocationCode] ")
            Value.AppendLine("    WHEN @gl ")
            Value.AppendLine("        THEN 0")
            Value.AppendLine("    WHEN @jp ")
            Value.AppendLine("        THEN (SELECT CAST([SortOrder] AS int) ")
            Value.AppendLine("              FROM s_BranchSortOrder")
            Value.AppendLine("              WHERE LocationCode = @locationCode AND DisplayLocationCode = @jp) ")
            Value.AppendLine("    WHEN @us ")
            Value.AppendLine("        THEN (SELECT CAST([SortOrder] AS int) ")
            Value.AppendLine("              FROM s_BranchSortOrder")
            Value.AppendLine("              WHERE LocationCode = @locationCode AND DisplayLocationCode = @us) ")
            Value.AppendLine("    WHEN @eu ")
            Value.AppendLine("        THEN (SELECT CAST([SortOrder] AS int) ")
            Value.AppendLine("              FROM s_BranchSortOrder")
            Value.AppendLine("              WHERE LocationCode = @locationCode AND DisplayLocationCode = @eu) ")
            Value.AppendLine("    WHEN @in ")
            Value.AppendLine("        THEN (SELECT CAST([SortOrder] AS int) ")
            Value.AppendLine("              FROM s_BranchSortOrder")
            Value.AppendLine("              WHERE LocationCode = @locationCode AND DisplayLocationCode = @in) ")
            Value.AppendLine("    WHEN @cn ")
            Value.AppendLine("        THEN (SELECT CAST([SortOrder] AS int) ")
            Value.AppendLine("              FROM s_BranchSortOrder")
            Value.AppendLine("              WHERE LocationCode = @locationCode AND DisplayLocationCode = @cn) ")
            Value.AppendLine("    END,")
            Value.AppendLine("    [LocationCode], ")
            Value.AppendLine("    [SortOrder]")
            Dim DBConn As SqlConnection = New SqlConnection(DBCommon.DB_CONNECT_STRING)
            Dim DBCommand As SqlCommand = DBConn.CreateCommand
            DBCommand.CommandText = Value.ToString
            DBCommand.Parameters.AddWithValue("userID", userID)
            DBCommand.Parameters.AddWithValue("gl", Common.LOCATION_CODE_GL)
            DBCommand.Parameters.AddWithValue("jp", Common.LOCATION_CODE_JP)
            DBCommand.Parameters.AddWithValue("us", Common.LOCATION_CODE_US)
            DBCommand.Parameters.AddWithValue("eu", Common.LOCATION_CODE_EU)
            DBCommand.Parameters.AddWithValue("cn", Common.LOCATION_CODE_CN)
            DBCommand.Parameters.AddWithValue("in", Common.LOCATION_CODE_IN)
            DBCommand.Parameters.AddWithValue("locationCode", locationCode)
            DBConn.Open()
            Dim DBReader As SqlDataReader = DBCommand.ExecuteReader

            While DBReader.Read
                Dim _Item As String = String.Empty
                Dim ehsHeader_Personalize_Sort As EhsHeader_Personalize_Sort = New EhsHeader_Personalize_Sort
                DBCommon.SetProperty(DBReader("Item"), _Item)
                ehsHeader_Personalize_Sort.Item = _Item
                Me.Add(ehsHeader_Personalize_Sort)

            End While

            DBReader.Close()
        End Sub
    End Class
End Namespace
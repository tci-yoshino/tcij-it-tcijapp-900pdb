Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess

    ''' <summary> 
    ''' EhsHeader_Personalize データクラス 
    ''' </summary> 
    ''' <remarks>
    ''' <para>Generated by データクラス自動生成ツール Ver 4.1</para>
    ''' </remarks>
    Public Class EhsHeader_Personalize


#Region "User-Defined Constant"

#End Region 'User-Defined Constant End

        Protected _UserID As Integer = 0
        Protected _Item As String = String.Empty
        Protected _CreatedBy As Integer = 0
        Protected _CreateDate As DateTime = New DateTime(0)
        Protected _UpdatedBy As Integer = 0
        Protected _UpdateDate As DateTime = New DateTime(0)

        ''' <summary> 
        ''' UserID を設定、または取得する 
        ''' </summary> 
        Public Property UserID() As Integer
            Get
                Return _UserID
            End Get
            Set(ByVal value As Integer)
                _UserID = value
            End Set
        End Property

        ''' <summary> 
        ''' Item を設定、または取得する 
        ''' </summary> 
        Public Property Item() As String
            Get
                Return _Item
            End Get
            Set(ByVal value As String)
                _Item = value
            End Set
        End Property

        ''' <summary> 
        ''' CreatedBy を設定、または取得する 
        ''' </summary> 
        Public Property CreatedBy() As Integer
            Get
                Return _CreatedBy
            End Get
            Set(ByVal value As Integer)
                _CreatedBy = value
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
        ''' UpdatedBy を設定、または取得する 
        ''' </summary> 
        Public Property UpdatedBy() As Integer
            Get
                Return _UpdatedBy
            End Get
            Set(ByVal value As Integer)
                _UpdatedBy = value
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
        ''' コンストラクタ
        ''' </summary> 
        Public Sub New()

        End Sub

        ''' <summary>
        ''' データベースからデータを読み込む。
        ''' </summary>
        ''' <param name="UserID">UserID</param>
        ''' <param name="Item">Item</param>
        Public Sub Load(ByVal UserID As Integer, _
                        ByVal Item As String)

            'データベースからデータを読み込む SQL 文字列を生成する。
            Dim Value As New StringBuilder()
            Value.AppendLine("SELECT")
            Value.AppendLine("    [UserID],")
            Value.AppendLine("    [Item],")
            Value.AppendLine("    [CreatedBy],")
            Value.AppendLine("    [CreateDate],")
            Value.AppendLine("    [UpdatedBy],")
            Value.AppendLine("    [UpdateDate]")
            Value.AppendLine("FROM")
            Value.AppendLine("    [EhsHeader_Personalize]")
            Value.AppendLine("WHERE")
            Value.AppendLine("    [UserID] = @UserID")
            Value.AppendLine("    AND [Item] = @Item")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = Value.ToString()
                    DBCommand.Parameters.AddWithValue("UserID", UserID)
                    DBCommand.Parameters.AddWithValue("Item", Item)
                    DBConn.Open()
                    Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
                    While DBReader.Read()
                        SetProperty(DBReader("UserID"), _UserID)
                        SetProperty(DBReader("Item"), _Item)
                        SetProperty(DBReader("CreatedBy"), _CreatedBy)
                        SetProperty(DBReader("CreateDate"), _CreateDate)
                        SetProperty(DBReader("UpdatedBy"), _UpdatedBy)
                        SetProperty(DBReader("UpdateDate"), _UpdateDate)
                    End While
                    DBReader.Close()
                End Using
            End Using

        End Sub

        ''' <summary> 
        ''' データベースへデータを書き込む。
        ''' </summary> 
        ''' <returns>IDENTITY で自動的に挿入された ID 値。更新または IDENTITY 列が無い場合は 0 が返る。</returns> 
        Public Function Save() As Integer

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                DBConn.Open()
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    Dim i_Identity As Integer = Me.Save(DBCommand)
                    Return i_Identity
                End Using
            End Using

        End Function

        ''' <summary> 
        ''' データベースへデータを書き込む。(Facade 専用)
        ''' </summary> 
        ''' <param name="DBCommand">SqlCommand</param>
        ''' <returns>IDENTITY で自動的に挿入された ID 値。更新または IDENTITY 列が無い場合は 0 が返る。</returns> 
        Public Function Save(ByVal DBCommand As SqlCommand) As Integer

            Dim i_ID As Integer = 0
            DBCommand.CommandText = CreateSaveSQL()
            DBCommand.Parameters.Clear()
            DBCommand.Parameters.AddWithValue("UserID", _UserID)
            DBCommand.Parameters.AddWithValue("Item", _Item)
            DBCommand.Parameters.AddWithValue("CreatedBy", _CreatedBy)
            DBCommand.Parameters.AddWithValue("UpdatedBy", _UpdatedBy)
            Dim ob_ID As Object = DBCommand.ExecuteScalar()
            If Not IsDBNull(ob_ID) Then
                i_ID = CInt(ob_ID)
            End If
            Return i_ID

        End Function

        ''' <summary> 
        ''' データベースへデータを書き込む SQL 文字列を生成する。
        ''' </summary> 
        ''' <returns>生成した SQL 文字列</returns> 
        Private Function CreateSaveSQL() As String

            Dim Value As New StringBuilder()
            Value.AppendLine("IF(")
            Value.AppendLine("    SELECT")
            Value.AppendLine("        COUNT(*)")
            Value.AppendLine("    FROM")
            Value.AppendLine("        [EhsHeader_Personalize]")
            Value.AppendLine("    WHERE")
            Value.AppendLine("        [UserID] = @UserID")
            Value.AppendLine("        AND [Item] = @Item")
            Value.AppendLine("    ) = 0")
            Value.AppendLine("        INSERT [EhsHeader_Personalize] (")
            Value.AppendLine("            [UserID],")
            Value.AppendLine("            [Item],")
            Value.AppendLine("            [CreatedBy],")
            Value.AppendLine("            [CreateDate],")
            Value.AppendLine("            [UpdatedBy],")
            Value.AppendLine("            [UpdateDate]")
            Value.AppendLine("        )")
            Value.AppendLine("        Values(")
            Value.AppendLine("            @UserID,")
            Value.AppendLine("            @Item,")
            Value.AppendLine("            @CreatedBy,")
            Value.AppendLine("            GETDATE(),")
            Value.AppendLine("            @UpdatedBy,")
            Value.AppendLine("            GETDATE()")
            Value.AppendLine("        )")
            Value.AppendLine("    ELSE")
            Value.AppendLine("        UPDATE")
            Value.AppendLine("            [EhsHeader_Personalize]")
            Value.AppendLine("        SET")
            Value.AppendLine("            [UserID] = @UserID,")
            Value.AppendLine("            [Item] = @Item,")
            Value.AppendLine("            [UpdatedBy] = @UpdatedBy,")
            Value.AppendLine("            [UpdateDate] = GETDATE()")
            Value.AppendLine("        WHERE ")
            Value.AppendLine("            [UserID] = @UserID")
            Value.AppendLine("            AND [Item] = @Item")
            Value.AppendLine(";")
            Value.AppendLine("SELECT SCOPE_IDENTITY();")
            Return Value.ToString()

        End Function

        ''' <summary> 
        ''' データの存在チェックを行う。
        ''' </summary> 
        ''' <returns>存在する場合は True、しない場合は False を返す</returns> 
        ''' <param name="UserID">UserID</param>
        ''' <param name="Item">Item</param>
        Public Shared Function IsExists(ByVal UserID As Integer, _
                                        ByVal Item As String) As Boolean

            ' データの存在チェックを行う SQL 文字列を生成する。
            Dim Value As New StringBuilder()
            Value.AppendLine("SELECT")
            Value.AppendLine("    COUNT(*)")
            Value.AppendLine("FROM")
            Value.AppendLine("    [EhsHeader_Personalize]")
            Value.AppendLine("WHERE")
            Value.AppendLine("    [UserID] = @UserID")
            Value.AppendLine("    AND [Item] = @Item")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = Value.ToString()
                    DBCommand.Parameters.Clear()
                    DBCommand.Parameters.AddWithValue("UserID", UserID)
                    DBCommand.Parameters.AddWithValue("Item", Item)
                    DBConn.Open()
                    Dim i_Count As Integer = Convert.ToInt32(DBCommand.ExecuteScalar())
                    Return i_Count > 0
                End Using
            End Using

        End Function

#Region "User-Defined Methods"

        '''' <summary>
        '''' データベースのデータを削除する
        '''' </summary>
        'Public Sub Delete()

        '    Using DBConn As New SqlConnection(DB_CONNECT_STRING)
        '        DBConn.Open()
        '        Using DBCommand As SqlCommand = DBConn.CreateCommand()
        '            Delete(DBCommand)
        '        End Using
        '    End Using

        'End Sub

        '''' <summary>
        '''' データベースのデータを削除する (Facade 専用)
        '''' </summary>
        '''' <param name="DBCommand">SqlCommand</param>
        'Public Sub Delete(ByVal DBCommand As SqlCommand)

        '    'データベースのデータを削除する SQL 文字列を生成する
        '    Dim Value As New StringBuilder()
        '    Value.AppendLine("DELETE FROM [EhsHeader_Personalize]")

        '    DBCommand.CommandText = Value.ToString()
        '    DBCommand.Parameters.Clear()
        '    DBCommand.ExecuteNonQuery()

        'End Sub

#End Region 'User-Defined Methods End

    End Class

    ''' <summary> 
    ''' EhsHeader_Personalize リストクラス 
    ''' </summary> 
    Public Class EhsHeader_PersonalizeList
        Inherits List(Of EhsHeader_Personalize)

#Region "User-Defined Constant of List"

#End Region 'User-Defined Constant of List End

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

#Region "User-Defined Methods of List"

        '''' <summary>
        '''' データベースから全てのデータを読み込む
        '''' </summary>
        'Public Sub Load()

        '    'データベースから全てのデータを読み込む SQL 文字列を生成する
        '    Dim Value As New StringBuilder()
        '    Value.AppendLine("SELECT")
        '    Value.AppendLine("    [UserID],")
        '    Value.AppendLine("    [Item],")
        '    Value.AppendLine("    [CreatedBy],")
        '    Value.AppendLine("    [CreateDate],")
        '    Value.AppendLine("    [UpdatedBy],")
        '    Value.AppendLine("    [UpdateDate]")
        '    Value.AppendLine("FROM")
        '    Value.AppendLine("    [EhsHeader_Personalize]")

        '    Using DBConn As New SqlConnection(DB_CONNECT_STRING)
        '        Using DBCommand As SqlCommand = DBConn.CreateCommand()
        '            DBCommand.CommandText = Value.ToString()
        '            DBConn.Open()
        '            Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
        '            While DBReader.Read()
        '                Dim dc_EhsHeader_Personalize As New EhsHeader_Personalize()
        '                SetProperty(DBReader("UserID"), dc_EhsHeader_Personalize.UserID)
        '                SetProperty(DBReader("Item"), dc_EhsHeader_Personalize.Item)
        '                SetProperty(DBReader("CreatedBy"), dc_EhsHeader_Personalize.CreatedBy)
        '                SetProperty(DBReader("CreateDate"), dc_EhsHeader_Personalize.CreateDate)
        '                SetProperty(DBReader("UpdatedBy"), dc_EhsHeader_Personalize.UpdatedBy)
        '                SetProperty(DBReader("UpdateDate"), dc_EhsHeader_Personalize.UpdateDate)
        '                Me.Add(dc_EhsHeader_Personalize)
        '            End While
        '            DBReader.Close()
        '        End Using
        '    End Using

        'End Sub

#End Region 'User-Defined Methods of List End

    End Class

End Namespace

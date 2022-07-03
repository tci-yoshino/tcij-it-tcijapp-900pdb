Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess

    ''' <summary> 
    ''' RFQHistory データクラス 
    ''' </summary> 
    ''' <remarks>
    ''' <para>Generated by データクラス自動生成ツール Ver 4.1</para>
    ''' </remarks>
    Public Class RFQHistory


#Region "User-Defined Constant"

#End Region 'User-Defined Constant End

        Protected _RFQHistoryNumber As Integer = 0
        Protected _RFQNumber As Integer = 0
        Protected _RFQStatusCode As String = String.Empty
        Protected _StatusChangeDate As DateTime = New DateTime(0)
        Protected _RFQCorresCode As String = String.Empty
        Protected _Note As String = String.Empty
        Protected _SendLocationCode As String = String.Empty
        Protected _SendUserID As Integer? = Nothing
        Protected _RcptLocationCode As String = String.Empty
        Protected _RcptUserID As Integer? = Nothing
        Protected _isChecked As Boolean = False
        Protected _AddrLocationCode As String = String.Empty
        Protected _AddrUserID As Integer? = Nothing
        Protected _CCLocationCode1 As String = String.Empty
        Protected _CCUserID1 As Integer? = Nothing
        Protected _CCLocationCode2 As String = String.Empty
        Protected _CCUserID2 As Integer? = Nothing
        Protected _CreatedBy As Integer = 0
        Protected _CreateDate As DateTime = New DateTime(0)
        Protected _UpdatedBy As Integer = 0
        Protected _UpdateDate As DateTime = New DateTime(0)

        ''' <summary> 
        ''' RFQHistoryNumber を設定、または取得する 
        ''' </summary> 
        Public Property RFQHistoryNumber() As Integer
            Get
                Return _RFQHistoryNumber
            End Get
            Set(ByVal value As Integer)
                _RFQHistoryNumber = value
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
        ''' RFQStatusCode を設定、または取得する 
        ''' </summary> 
        Public Property RFQStatusCode() As String
            Get
                Return _RFQStatusCode
            End Get
            Set(ByVal value As String)
                _RFQStatusCode = value
            End Set
        End Property

        ''' <summary> 
        ''' StatusChangeDate を設定、または取得する 
        ''' </summary> 
        Public Property StatusChangeDate() As DateTime
            Get
                Return _StatusChangeDate
            End Get
            Set(ByVal value As DateTime)
                _StatusChangeDate = value
            End Set
        End Property

        ''' <summary> 
        ''' RFQCorresCode を設定、または取得する 
        ''' </summary> 
        Public Property RFQCorresCode() As String
            Get
                Return _RFQCorresCode
            End Get
            Set(ByVal value As String)
                _RFQCorresCode = value
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
        ''' SendLocationCode を設定、または取得する 
        ''' </summary> 
        Public Property SendLocationCode() As String
            Get
                Return _SendLocationCode
            End Get
            Set(ByVal value As String)
                _SendLocationCode = value
            End Set
        End Property

        ''' <summary> 
        ''' SendUserID を設定、または取得する 
        ''' <para>
        ''' ※ Integer 変数へ格納する場合は以下のようにすること。
        '''     Dim val As Integer = IIf(SendUserID.HasValue, SendUserID, 0)
        '''     Dim val As Integer = SendUserID.GetValueOrDefault() 
        ''' </para>
        ''' </summary> 
        Public Property SendUserID() As Integer?
            Get
                Return _SendUserID
            End Get
            Set(ByVal value As Integer?)
                _SendUserID = value
            End Set
        End Property

        ''' <summary> 
        ''' RcptLocationCode を設定、または取得する 
        ''' </summary> 
        Public Property RcptLocationCode() As String
            Get
                Return _RcptLocationCode
            End Get
            Set(ByVal value As String)
                _RcptLocationCode = value
            End Set
        End Property

        ''' <summary> 
        ''' RcptUserID を設定、または取得する 
        ''' <para>
        ''' ※ Integer 変数へ格納する場合は以下のようにすること。
        '''     Dim val As Integer = IIf(RcptUserID.HasValue, RcptUserID, 0)
        '''     Dim val As Integer = RcptUserID.GetValueOrDefault() 
        ''' </para>
        ''' </summary> 
        Public Property RcptUserID() As Integer?
            Get
                Return _RcptUserID
            End Get
            Set(ByVal value As Integer?)
                _RcptUserID = value
            End Set
        End Property

        ''' <summary> 
        ''' isChecked を設定、または取得する 
        ''' </summary> 
        Public Property isChecked() As Boolean
            Get
                Return _isChecked
            End Get
            Set(ByVal value As Boolean)
                _isChecked = value
            End Set
        End Property

        ''' <summary> 
        ''' AddrLocationCode を設定、または取得する 
        ''' </summary> 
        Public Property AddrLocationCode() As String
            Get
                Return _AddrLocationCode
            End Get
            Set(ByVal value As String)
                _AddrLocationCode = value
            End Set
        End Property

        ''' <summary> 
        ''' AddrUserID を設定、または取得する 
        ''' <para>
        ''' ※ Integer 変数へ格納する場合は以下のようにすること。
        '''     Dim val As Integer = IIf(AddrUserID.HasValue, AddrUserID, 0)
        '''     Dim val As Integer = AddrUserID.GetValueOrDefault() 
        ''' </para>
        ''' </summary> 
        Public Property AddrUserID() As Integer?
            Get
                Return _AddrUserID
            End Get
            Set(ByVal value As Integer?)
                _AddrUserID = value
            End Set
        End Property

        ''' <summary> 
        ''' CCLocationCode1 を設定、または取得する 
        ''' </summary> 
        Public Property CCLocationCode1() As String
            Get
                Return _CCLocationCode1
            End Get
            Set(ByVal value As String)
                _CCLocationCode1 = value
            End Set
        End Property

        ''' <summary> 
        ''' CCUserID1 を設定、または取得する 
        ''' <para>
        ''' ※ Integer 変数へ格納する場合は以下のようにすること。
        '''     Dim val As Integer = IIf(CCUserID1.HasValue, CCUserID1, 0)
        '''     Dim val As Integer = CCUserID1.GetValueOrDefault() 
        ''' </para>
        ''' </summary> 
        Public Property CCUserID1() As Integer?
            Get
                Return _CCUserID1
            End Get
            Set(ByVal value As Integer?)
                _CCUserID1 = value
            End Set
        End Property

        ''' <summary> 
        ''' CCLocationCode2 を設定、または取得する 
        ''' </summary> 
        Public Property CCLocationCode2() As String
            Get
                Return _CCLocationCode2
            End Get
            Set(ByVal value As String)
                _CCLocationCode2 = value
            End Set
        End Property

        ''' <summary> 
        ''' CCUserID2 を設定、または取得する 
        ''' <para>
        ''' ※ Integer 変数へ格納する場合は以下のようにすること。
        '''     Dim val As Integer = IIf(CCUserID2.HasValue, CCUserID2, 0)
        '''     Dim val As Integer = CCUserID2.GetValueOrDefault() 
        ''' </para>
        ''' </summary> 
        Public Property CCUserID2() As Integer?
            Get
                Return _CCUserID2
            End Get
            Set(ByVal value As Integer?)
                _CCUserID2 = value
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
        ''' <param name="RFQHistoryNumber">RFQHistoryNumber</param>
        Public Sub Load(ByVal RFQHistoryNumber As Integer)

            'データベースからデータを読み込む SQL 文字列を生成する。
            Dim Value As New StringBuilder()
            Value.AppendLine("SELECT")
            Value.AppendLine("    [RFQHistoryNumber],")
            Value.AppendLine("    [RFQNumber],")
            Value.AppendLine("    [RFQStatusCode],")
            Value.AppendLine("    [StatusChangeDate],")
            Value.AppendLine("    [RFQCorresCode],")
            Value.AppendLine("    [Note],")
            Value.AppendLine("    [SendLocationCode],")
            Value.AppendLine("    [SendUserID],")
            Value.AppendLine("    [RcptLocationCode],")
            Value.AppendLine("    [RcptUserID],")
            Value.AppendLine("    [isChecked],")
            Value.AppendLine("    [AddrLocationCode],")
            Value.AppendLine("    [AddrUserID],")
            Value.AppendLine("    [CCLocationCode1],")
            Value.AppendLine("    [CCUserID1],")
            Value.AppendLine("    [CCLocationCode2],")
            Value.AppendLine("    [CCUserID2],")
            Value.AppendLine("    [CreatedBy],")
            Value.AppendLine("    [CreateDate],")
            Value.AppendLine("    [UpdatedBy],")
            Value.AppendLine("    [UpdateDate]")
            Value.AppendLine("FROM")
            Value.AppendLine("    [RFQHistory]")
            Value.AppendLine("WHERE")
            Value.AppendLine("    [RFQHistoryNumber] = @RFQHistoryNumber")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = Value.ToString()
                    DBCommand.Parameters.AddWithValue("RFQHistoryNumber", RFQHistoryNumber)
                    DBConn.Open()
                    Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
                    While DBReader.Read()
                        SetProperty(DBReader("RFQHistoryNumber"), _RFQHistoryNumber)
                        SetProperty(DBReader("RFQNumber"), _RFQNumber)
                        SetProperty(DBReader("RFQStatusCode"), _RFQStatusCode)
                        SetProperty(DBReader("StatusChangeDate"), _StatusChangeDate)
                        SetProperty(DBReader("RFQCorresCode"), _RFQCorresCode)
                        SetProperty(DBReader("Note"), _Note)
                        SetProperty(DBReader("SendLocationCode"), _SendLocationCode)
                        SetProperty(DBReader("SendUserID"), _SendUserID)
                        SetProperty(DBReader("RcptLocationCode"), _RcptLocationCode)
                        SetProperty(DBReader("RcptUserID"), _RcptUserID)
                        SetProperty(DBReader("isChecked"), _isChecked)
                        SetProperty(DBReader("AddrLocationCode"), _AddrLocationCode)
                        SetProperty(DBReader("AddrUserID"), _AddrUserID)
                        SetProperty(DBReader("CCLocationCode1"), _CCLocationCode1)
                        SetProperty(DBReader("CCUserID1"), _CCUserID1)
                        SetProperty(DBReader("CCLocationCode2"), _CCLocationCode2)
                        SetProperty(DBReader("CCUserID2"), _CCUserID2)
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
            DBCommand.Parameters.AddWithValue("RFQHistoryNumber", _RFQHistoryNumber)
            DBCommand.Parameters.AddWithValue("RFQNumber", _RFQNumber)
            DBCommand.Parameters.AddWithValue("RFQStatusCode", _RFQStatusCode)
            DBCommand.Parameters.AddWithValue("StatusChangeDate", ConvertDefaultDateTimeToNull(_StatusChangeDate))
            DBCommand.Parameters.AddWithValue("RFQCorresCode", ConvertEmptyStringToNull(_RFQCorresCode))
            DBCommand.Parameters.AddWithValue("Note", ConvertEmptyStringToNull(_Note))
            DBCommand.Parameters.AddWithValue("SendLocationCode", ConvertEmptyStringToNull(_SendLocationCode))
            DBCommand.Parameters.AddWithValue("SendUserID", ConvertNothingToNull(_SendUserID))
            DBCommand.Parameters.AddWithValue("RcptLocationCode", ConvertEmptyStringToNull(_RcptLocationCode))
            DBCommand.Parameters.AddWithValue("RcptUserID", ConvertNothingToNull(_RcptUserID))
            DBCommand.Parameters.AddWithValue("isChecked", _isChecked)
            DBCommand.Parameters.AddWithValue("AddrLocationCode", ConvertEmptyStringToNull(_AddrLocationCode))
            DBCommand.Parameters.AddWithValue("AddrUserID", ConvertNothingToNull(_AddrUserID))
            DBCommand.Parameters.AddWithValue("CCLocationCode1", ConvertEmptyStringToNull(_CCLocationCode1))
            DBCommand.Parameters.AddWithValue("CCUserID1", ConvertNothingToNull(_CCUserID1))
            DBCommand.Parameters.AddWithValue("CCLocationCode2", ConvertEmptyStringToNull(_CCLocationCode2))
            DBCommand.Parameters.AddWithValue("CCUserID2", ConvertNothingToNull(_CCUserID2))
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
            Value.AppendLine("        [RFQHistory]")
            Value.AppendLine("    WHERE")
            Value.AppendLine("        [RFQHistoryNumber] = @RFQHistoryNumber")
            Value.AppendLine("    ) = 0")
            Value.AppendLine("        INSERT [RFQHistory] (")
            Value.AppendLine("            [RFQNumber],")
            Value.AppendLine("            [RFQStatusCode],")
            Value.AppendLine("            [StatusChangeDate],")
            Value.AppendLine("            [RFQCorresCode],")
            Value.AppendLine("            [Note],")
            Value.AppendLine("            [SendLocationCode],")
            Value.AppendLine("            [SendUserID],")
            Value.AppendLine("            [RcptLocationCode],")
            Value.AppendLine("            [RcptUserID],")
            Value.AppendLine("            [isChecked],")
            Value.AppendLine("            [AddrLocationCode],")
            Value.AppendLine("            [AddrUserID],")
            Value.AppendLine("            [CCLocationCode1],")
            Value.AppendLine("            [CCUserID1],")
            Value.AppendLine("            [CCLocationCode2],")
            Value.AppendLine("            [CCUserID2],")
            Value.AppendLine("            [CreatedBy],")
            Value.AppendLine("            [CreateDate],")
            Value.AppendLine("            [UpdatedBy],")
            Value.AppendLine("            [UpdateDate]")
            Value.AppendLine("        )")
            Value.AppendLine("        Values(")
            Value.AppendLine("            @RFQNumber,")
            Value.AppendLine("            @RFQStatusCode,")
            Value.AppendLine("            @StatusChangeDate,")
            Value.AppendLine("            @RFQCorresCode,")
            Value.AppendLine("            @Note,")
            Value.AppendLine("            @SendLocationCode,")
            Value.AppendLine("            @SendUserID,")
            Value.AppendLine("            @RcptLocationCode,")
            Value.AppendLine("            @RcptUserID,")
            Value.AppendLine("            @isChecked,")
            Value.AppendLine("            @AddrLocationCode,")
            Value.AppendLine("            @AddrUserID,")
            Value.AppendLine("            @CCLocationCode1,")
            Value.AppendLine("            @CCUserID1,")
            Value.AppendLine("            @CCLocationCode2,")
            Value.AppendLine("            @CCUserID2,")
            Value.AppendLine("            @CreatedBy,")
            Value.AppendLine("            GETDATE(),")
            Value.AppendLine("            @UpdatedBy,")
            Value.AppendLine("            GETDATE()")
            Value.AppendLine("        )")
            Value.AppendLine("    ELSE")
            Value.AppendLine("        UPDATE")
            Value.AppendLine("            [RFQHistory]")
            Value.AppendLine("        SET")
            Value.AppendLine("            [RFQNumber] = @RFQNumber,")
            Value.AppendLine("            [RFQStatusCode] = @RFQStatusCode,")
            Value.AppendLine("            [StatusChangeDate] = @StatusChangeDate,")
            Value.AppendLine("            [RFQCorresCode] = @RFQCorresCode,")
            Value.AppendLine("            [Note] = @Note,")
            Value.AppendLine("            [SendLocationCode] = @SendLocationCode,")
            Value.AppendLine("            [SendUserID] = @SendUserID,")
            Value.AppendLine("            [RcptLocationCode] = @RcptLocationCode,")
            Value.AppendLine("            [RcptUserID] = @RcptUserID,")
            Value.AppendLine("            [isChecked] = @isChecked,")
            Value.AppendLine("            [AddrLocationCode] = @AddrLocationCode,")
            Value.AppendLine("            [AddrUserID] = @AddrUserID,")
            Value.AppendLine("            [CCLocationCode1] = @CCLocationCode1,")
            Value.AppendLine("            [CCUserID1] = @CCUserID1,")
            Value.AppendLine("            [CCLocationCode2] = @CCLocationCode2,")
            Value.AppendLine("            [CCUserID2] = @CCUserID2,")
            Value.AppendLine("            [UpdatedBy] = @UpdatedBy,")
            Value.AppendLine("            [UpdateDate] = GETDATE()")
            Value.AppendLine("        WHERE ")
            Value.AppendLine("            [RFQHistoryNumber] = @RFQHistoryNumber")
            Value.AppendLine(";")
            Value.AppendLine("SELECT SCOPE_IDENTITY();")
            Return Value.ToString()

        End Function

        ''' <summary> 
        ''' データの存在チェックを行う。
        ''' </summary> 
        ''' <returns>存在する場合は True、しない場合は False を返す</returns> 
        ''' <param name="RFQHistoryNumber">RFQHistoryNumber</param>
        Public Shared Function IsExists(ByVal RFQHistoryNumber As Integer) As Boolean

            ' データの存在チェックを行う SQL 文字列を生成する。
            Dim Value As New StringBuilder()
            Value.AppendLine("SELECT")
            Value.AppendLine("    COUNT(*)")
            Value.AppendLine("FROM")
            Value.AppendLine("    [RFQHistory]")
            Value.AppendLine("WHERE")
            Value.AppendLine("    [RFQHistoryNumber] = @RFQHistoryNumber")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = Value.ToString()
                    DBCommand.Parameters.Clear()
                    DBCommand.Parameters.AddWithValue("RFQHistoryNumber", RFQHistoryNumber)
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
        '''' <param name="RFQHistoryNumber">RFQHistoryNumber</param>
        'Public Sub Delete(ByVal RFQHistoryNumber As Integer)

        '    Using DBConn As New SqlConnection(DB_CONNECT_STRING)
        '        DBConn.Open()
        '        Using DBCommand As SqlCommand = DBConn.CreateCommand()
        '            Delete(DBCommand, RFQHistoryNumber)
        '        End Using
        '    End Using

        'End Sub

        '''' <summary>
        '''' データベースのデータを削除する (Facade 専用)
        '''' </summary>
        '''' <param name="DBCommand">SqlCommand</param>
        '''' <param name="RFQHistoryNumber">RFQHistoryNumber</param>
        'Public Sub Delete(ByVal DBCommand As SqlCommand, _
        '                  ByVal RFQHistoryNumber As Integer)

        '    'データベースのデータを削除する SQL 文字列を生成する
        '    Dim Value As New StringBuilder()
        '    Value.AppendLine("DELETE FROM [RFQHistory]")
        '    Value.AppendLine("WHERE")
        '    Value.AppendLine("    [RFQHistoryNumber] = @RFQHistoryNumber")

        '    DBCommand.CommandText = Value.ToString()
        '    DBCommand.Parameters.Clear()
        '    DBCommand.Parameters.AddWithValue("RFQHistoryNumber", RFQHistoryNumber)
        '    DBCommand.ExecuteNonQuery()

        'End Sub

#End Region 'User-Defined Methods End

    End Class

    ''' <summary> 
    ''' RFQHistory リストクラス 
    ''' </summary> 
    Public Class RFQHistoryList
        Inherits List(Of RFQHistory)

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
        '    Value.AppendLine("    [RFQHistoryNumber],")
        '    Value.AppendLine("    [RFQNumber],")
        '    Value.AppendLine("    [RFQStatusCode],")
        '    Value.AppendLine("    [StatusChangeDate],")
        '    Value.AppendLine("    [RFQCorresCode],")
        '    Value.AppendLine("    [Note],")
        '    Value.AppendLine("    [SendLocationCode],")
        '    Value.AppendLine("    [SendUserID],")
        '    Value.AppendLine("    [RcptLocationCode],")
        '    Value.AppendLine("    [RcptUserID],")
        '    Value.AppendLine("    [isChecked],")
        '    Value.AppendLine("    [AddrLocationCode],")
        '    Value.AppendLine("    [AddrUserID],")
        '    Value.AppendLine("    [CCLocationCode1],")
        '    Value.AppendLine("    [CCUserID1],")
        '    Value.AppendLine("    [CCLocationCode2],")
        '    Value.AppendLine("    [CCUserID2],")
        '    Value.AppendLine("    [CreatedBy],")
        '    Value.AppendLine("    [CreateDate],")
        '    Value.AppendLine("    [UpdatedBy],")
        '    Value.AppendLine("    [UpdateDate]")
        '    Value.AppendLine("FROM")
        '    Value.AppendLine("    [RFQHistory]")

        '    Using DBConn As New SqlConnection(DB_CONNECT_STRING)
        '        Using DBCommand As SqlCommand = DBConn.CreateCommand()
        '            DBCommand.CommandText = Value.ToString()
        '            DBConn.Open()
        '            Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
        '            While DBReader.Read()
        '                Dim dc_RFQHistory As New RFQHistory()
        '                SetProperty(DBReader("RFQHistoryNumber"), dc_RFQHistory.RFQHistoryNumber)
        '                SetProperty(DBReader("RFQNumber"), dc_RFQHistory.RFQNumber)
        '                SetProperty(DBReader("RFQStatusCode"), dc_RFQHistory.RFQStatusCode)
        '                SetProperty(DBReader("StatusChangeDate"), dc_RFQHistory.StatusChangeDate)
        '                SetProperty(DBReader("RFQCorresCode"), dc_RFQHistory.RFQCorresCode)
        '                SetProperty(DBReader("Note"), dc_RFQHistory.Note)
        '                SetProperty(DBReader("SendLocationCode"), dc_RFQHistory.SendLocationCode)
        '                SetProperty(DBReader("SendUserID"), dc_RFQHistory.SendUserID)
        '                SetProperty(DBReader("RcptLocationCode"), dc_RFQHistory.RcptLocationCode)
        '                SetProperty(DBReader("RcptUserID"), dc_RFQHistory.RcptUserID)
        '                SetProperty(DBReader("isChecked"), dc_RFQHistory.isChecked)
        '                SetProperty(DBReader("AddrLocationCode"), dc_RFQHistory.AddrLocationCode)
        '                SetProperty(DBReader("AddrUserID"), dc_RFQHistory.AddrUserID)
        '                SetProperty(DBReader("CCLocationCode1"), dc_RFQHistory.CCLocationCode1)
        '                SetProperty(DBReader("CCUserID1"), dc_RFQHistory.CCUserID1)
        '                SetProperty(DBReader("CCLocationCode2"), dc_RFQHistory.CCLocationCode2)
        '                SetProperty(DBReader("CCUserID2"), dc_RFQHistory.CCUserID2)
        '                SetProperty(DBReader("CreatedBy"), dc_RFQHistory.CreatedBy)
        '                SetProperty(DBReader("CreateDate"), dc_RFQHistory.CreateDate)
        '                SetProperty(DBReader("UpdatedBy"), dc_RFQHistory.UpdatedBy)
        '                SetProperty(DBReader("UpdateDate"), dc_RFQHistory.UpdateDate)
        '                Me.Add(dc_RFQHistory)
        '            End While
        '            DBReader.Close()
        '        End Using
        '    End Using

        'End Sub

#End Region 'User-Defined Methods of List End

    End Class

End Namespace

Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess

    ''' <summary> 
    ''' v_User データクラス 
    ''' </summary> 
    ''' <remarks>
    ''' <para>Generated by データクラス自動生成ツール Ver 4.1</para>
    ''' </remarks>
    Public Class v_User


        Protected _UserID As Integer = 0
        Protected _AccountName As String = String.Empty
        Protected _Name As String = String.Empty
        Protected _RoleCode As String = String.Empty
        Protected _PrivilegeLevel As String = String.Empty
        Protected _R3ID As String = String.Empty
        Protected _R3PurchasingGroup As String = String.Empty
        Protected _Email As String = String.Empty
        Protected _isDisabled As Boolean = False
        Protected _LocationCode As String = String.Empty
        Protected _LocationName As String = String.Empty

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
        ''' AccountName を設定、または取得する 
        ''' </summary> 
        Public Property AccountName() As String
            Get
                Return _AccountName
            End Get
            Set(ByVal value As String)
                _AccountName = value
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
        ''' RoleCode を設定、または取得する 
        ''' </summary> 
        Public Property RoleCode() As String
            Get
                Return _RoleCode
            End Get
            Set(ByVal value As String)
                _RoleCode = value
            End Set
        End Property

        ''' <summary> 
        ''' PrivilegeLevel を設定、または取得する 
        ''' </summary> 
        Public Property PrivilegeLevel() As String
            Get
                Return _PrivilegeLevel
            End Get
            Set(ByVal value As String)
                _PrivilegeLevel = value
            End Set
        End Property

        ''' <summary> 
        ''' R3ID を設定、または取得する 
        ''' </summary> 
        Public Property R3ID() As String
            Get
                Return _R3ID
            End Get
            Set(ByVal value As String)
                _R3ID = value
            End Set
        End Property

        ''' <summary> 
        ''' R3PurchasingGroup を設定、または取得する 
        ''' </summary> 
        Public Property R3PurchasingGroup() As String
            Get
                Return _R3PurchasingGroup
            End Get
            Set(ByVal value As String)
                _R3PurchasingGroup = value
            End Set
        End Property

        ''' <summary> 
        ''' Email を設定、または取得する 
        ''' </summary> 
        Public Property Email() As String
            Get
                Return _Email
            End Get
            Set(ByVal value As String)
                _Email = value
            End Set
        End Property

        ''' <summary> 
        ''' isDisabled を設定、または取得する 
        ''' </summary> 
        Public Property isDisabled() As Boolean
            Get
                Return _isDisabled
            End Get
            Set(ByVal value As Boolean)
                _isDisabled = value
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
        ''' LocationName を設定、または取得する 
        ''' </summary> 
        Public Property LocationName() As String
            Get
                Return _LocationName
            End Get
            Set(ByVal value As String)
                _LocationName = value
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
        Public Sub Load()

            'データベースからデータを読み込む SQL 文字列を生成する。
            Dim Value As New StringBuilder()
            Value.AppendLine("SELECT")
            Value.AppendLine("    [UserID],")
            Value.AppendLine("    [AccountName],")
            Value.AppendLine("    [Name],")
            Value.AppendLine("    [RoleCode],")
            Value.AppendLine("    [PrivilegeLevel],")
            Value.AppendLine("    [R3ID],")
            Value.AppendLine("    [R3PurchasingGroup],")
            Value.AppendLine("    [Email],")
            Value.AppendLine("    [isDisabled],")
            Value.AppendLine("    [LocationCode],")
            Value.AppendLine("    [LocationName]")
            Value.AppendLine("FROM")
            Value.AppendLine("    [v_User]")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = Value.ToString()
                    DBConn.Open()
                    Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
                    While DBReader.Read()
                        SetProperty(DBReader("UserID"), _UserID)
                        SetProperty(DBReader("AccountName"), _AccountName)
                        SetProperty(DBReader("Name"), _Name)
                        SetProperty(DBReader("RoleCode"), _RoleCode)
                        SetProperty(DBReader("PrivilegeLevel"), _PrivilegeLevel)
                        SetProperty(DBReader("R3ID"), _R3ID)
                        SetProperty(DBReader("R3PurchasingGroup"), _R3PurchasingGroup)
                        SetProperty(DBReader("Email"), _Email)
                        SetProperty(DBReader("isDisabled"), _isDisabled)
                        SetProperty(DBReader("LocationCode"), _LocationCode)
                        SetProperty(DBReader("LocationName"), _LocationName)
                    End While
                    DBReader.Close()
                End Using
            End Using

        End Sub
        ''' <summary> 
        ''' Nameを昇順にデータ取得
        ''' </summary> 
        ''' <param name="PrivilegeLevel">ユーザごとのアクセス権限レベル</param>
        ''' <returns>データ取得SQL文</returns>
        Public Function CreateUserSelectSQL(ByVal PrivilegeLevel As String) As String
            Dim sb_SQL As New Text.StringBuilder
            sb_SQL.AppendLine("SELECT")
            sb_SQL.AppendLine("    [UserID],")
            sb_SQL.AppendLine("    [Name]")
            sb_SQL.AppendLine("FROM")
            sb_SQL.AppendLine("    [v_User]")
            If PrivilegeLevel = "P" Then
                sb_SQL.AppendLine("WHERE")
                sb_SQL.AppendLine("    [isDisabled] = 0 AND")
                sb_SQL.AppendLine("    [LocationCode] = @LocationCode")
            End If
            sb_SQL.AppendLine("ORDER BY")
            sb_SQL.AppendLine("    [Name] ASC")

            Return sb_SQL.ToString()
        End Function
        ''' <summary> 
        ''' データ件数を取得
        ''' </summary> 
        ''' <returns>データ件数SQL文</returns>
        Public Function CreateUserCountSQL() As String
            Dim sb_SQL As New Text.StringBuilder
            sb_SQL.AppendLine("SELECT")
            sb_SQL.AppendLine("    COUNT([UserID]) as count")
            sb_SQL.AppendLine("FROM")
            sb_SQL.AppendLine("    [v_User]")
            sb_SQL.AppendLine("WHERE")
            sb_SQL.AppendLine("    [LocationCode] = @LocationCode AND")
            sb_SQL.AppendLine("    [UserID] = @UserID")
            Return sb_SQL.ToString()
        End Function
        '''' <summary> 
        '''' データの存在チェックを行う。
        '''' </summary> 
        '''' <returns>存在する場合は True、しない場合は False を返す</returns> 
        'Public Shared Function IsExists() As Boolean

        '    ' データの存在チェックを行う SQL 文字列を生成する。
        '    Dim Value As New StringBuilder()
        '    Value.AppendLine("SELECT")
        '    Value.AppendLine("    COUNT(*)")
        '    Value.AppendLine("FROM")
        '    Value.AppendLine("    [v_User]")

        '    Using DBConn As New SqlConnection(DB_CONNECT_STRING)
        '        Using DBCommand As SqlCommand = DBConn.CreateCommand()
        '            DBCommand.CommandText = Value.ToString()
        '            DBCommand.Parameters.Clear()
        '            DBConn.Open()
        '            Dim i_Count As Integer = Convert.ToInt32(DBCommand.ExecuteScalar())
        '            Return i_Count > 0
        '        End Using
        '    End Using

        'End Function

    End Class

    ''' <summary> 
    ''' v_User リストクラス 
    ''' </summary> 
    Public Class v_UserList
        Inherits List(Of v_User)

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

    End Class

End Namespace

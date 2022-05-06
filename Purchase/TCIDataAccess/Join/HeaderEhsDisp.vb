Option Explicit On
Option Strict On
Option Infer Off

Imports System.Data.SqlClient

Namespace TCIDataAccess.Join

    ''' <summary> 
    ''' HeaderEhsDisp データクラス 
    ''' </summary> 
    Public Class HeaderEhsDisp

        Protected _UserID As Integer = 0
        Protected _LocationCode As String = String.Empty
        Protected _EhsHeaderList As List(Of s_EhsHeader) 
        Protected _EhsHeaderListForLocation As List(Of s_EhsHeader) 
        Protected _EhsHeaderListForPersonalize As List(Of s_EhsHeader) 

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
        ''' EhsHeaderList を設定、または取得する 
        ''' </summary> 
        Public Property EhsHeaderList() As List(Of s_EhsHeader)
            Get
                Return _EhsHeaderList
            End Get
            Set(ByVal value As List(Of s_EhsHeader))
                _EhsHeaderList = value
            End Set
        End Property

        ''' <summary> 
        ''' EhsHeaderListForLocation を設定、または取得する 
        ''' </summary> 
        Public Property EhsHeaderListForLocation() As List(Of s_EhsHeader)
            Get
                Return _EhsHeaderListForLocation
            End Get
            Set(ByVal value As List(Of s_EhsHeader))
                _EhsHeaderListForLocation = value
            End Set
        End Property

        ''' <summary> 
        ''' EhsHeaderListForPersonalize を設定、または取得する 
        ''' </summary> 
        Public Property EhsHeaderListForPersonalize() As List(Of s_EhsHeader)
            Get
                Return _EhsHeaderListForPersonalize
            End Get
            Set(ByVal value As List(Of s_EhsHeader))
                _EhsHeaderListForPersonalize = value
            End Set
        End Property

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' データベースから全てのデータを読み込む（拠点ソート）
        ''' </summary>
        Public Sub GetEhsHeader()

            '' データベースから全てのデータを読み込む SQL 文字列を生成する
            Dim sb_SQL As StringBuilder = New StringBuilder

            sb_SQL.AppendLine("SELECT ")
            sb_SQL.AppendLine("    EHP.[ITEM] ")
            sb_SQL.AppendLine("FROM ")
            sb_SQL.AppendLine("    [EhsHeader_Personalize] AS EHP ")
            sb_SQL.AppendLine("WHERE ")
            sb_SQL.AppendLine("    EHP.[UserID] = @userID ")

            Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.Parameters.Clear()
                    If Me.UserID <> 0 Then
                        DBCommand.Parameters.AddWithValue("userID", Me.UserID)
                    End If

                    DBCommand.CommandText = sb_SQL.ToString()

                    ' 実行
                    DBConn.Open()
                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader()
                        Dim dc_EhsHeaderList As List(Of s_EhsHeader) = New List(Of s_EhsHeader)
                        While DBReader.Read

                            Dim dc_EhsHeader As s_EhsHeader = New s_EhsHeader
                            DBCommon.SetProperty(DBReader("ITEM"), dc_EhsHeader.Item)

                            dc_EhsHeaderList.Add(dc_EhsHeader)

                        End While

                        Me.EhsHeaderList = dc_EhsHeaderList
                    End Using
                End Using
            End Using

        End Sub

        ''' <summary>
        ''' データベースから全てのデータを読み込む（拠点ソート）
        ''' </summary>
        Public Sub GetEhsHeaderPersonalize()
            Dim ehsHeaderListForPersonalize As List(Of s_EhsHeader) = New List(Of s_EhsHeader)
            '' データベースから全てのデータを読み込む SQL 文字列を生成する
            Dim sb_SQL As StringBuilder = New StringBuilder

            sb_SQL.AppendLine("SELECT ")
            sb_SQL.AppendLine("    EHP.[Item], ")
            sb_SQL.AppendLine("    SEH.[Text] ")
            sb_SQL.AppendLine("FROM ")
            sb_SQL.AppendLine("    [EhsHeader_Personalize] AS EHP")
            sb_SQL.AppendLine("  INNER JOIN [s_EhsHeader] AS SEH ON EHP.Item = SEH.Item")
            sb_SQL.AppendLine("WHERE ")
            sb_SQL.AppendLine("    EHP.[UserID] = @userID ")
            'sb_SQL.AppendLine("AND SEH.[locationCode] = @locationCode ")
            sb_SQL.AppendLine("ORDER BY ")
            sb_SQL.AppendLine("    CASE SEH.[LocationCode] ")
            sb_SQL.AppendLine("      WHEN @gl THEN 0 ")
            sb_SQL.AppendLine("      WHEN @jp THEN (SELECT CAST([SortOrder] AS int) ")
            sb_SQL.AppendLine("                     FROM [s_BranchSortOrder] ")
            sb_SQL.AppendLine("                     WHERE LocationCode = @locationCode AND DisplayLocationCode = @jp) ")
            sb_SQL.AppendLine("      WHEN @us THEN (SELECT CAST([SortOrder] AS int) ")
            sb_SQL.AppendLine("                     FROM [s_BranchSortOrder] ")
            sb_SQL.AppendLine("                     WHERE LocationCode = @locationCode AND DisplayLocationCode = @us) ")
            sb_SQL.AppendLine("      WHEN @eu THEN (SELECT CAST([SortOrder] AS int) ")
            sb_SQL.AppendLine("                     FROM [s_BranchSortOrder] ")
            sb_SQL.AppendLine("                     WHERE LocationCode = @locationCode AND DisplayLocationCode = @eu) ")
            sb_SQL.AppendLine("      WHEN @in THEN (SELECT CAST([SortOrder] AS int) ")
            sb_SQL.AppendLine("                     FROM [s_BranchSortOrder] ")
            sb_SQL.AppendLine("                     WHERE LocationCode = @locationCode AND DisplayLocationCode = @in) ")
            sb_SQL.AppendLine("      WHEN @cn THEN (SELECT CAST([SortOrder] AS int) ")
            sb_SQL.AppendLine("                     FROM [s_BranchSortOrder] ")
            sb_SQL.AppendLine("                     WHERE LocationCode = @locationCode AND DisplayLocationCode = @cn) ")
            sb_SQL.AppendLine("    END,")
            sb_SQL.AppendLine("    SEH.[SortOrder]")

            Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.Parameters.Clear()
                    DBCommand.Parameters.AddWithValue("gl", Common.LOCATION_GL)
                    DBCommand.Parameters.AddWithValue("jp", Common.LOCATION_JP)
                    DBCommand.Parameters.AddWithValue("us", Common.LOCATION_US)
                    DBCommand.Parameters.AddWithValue("eu", Common.LOCATION_EU)
                    DBCommand.Parameters.AddWithValue("cn", Common.LOCATION_CN)
                    DBCommand.Parameters.AddWithValue("in", Common.LOCATION_IN)
                    If Me.UserID <> 0 Then
                        DBCommand.Parameters.AddWithValue("userID", Me.UserID)
                    End If
                    If Not String.IsNullOrEmpty(Me.LocationCode) Then
                        DBCommand.Parameters.AddWithValue("locationCode", Me.LocationCode)
                    End If

                    DBCommand.CommandText = sb_SQL.ToString()

                    ' 実行
                    DBConn.Open()
                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader()
                        Dim dc_EhsHeaderList As List(Of s_EhsHeader) = New List(Of s_EhsHeader)
                        While DBReader.Read

                            Dim dc_EhsHeader As s_EhsHeader = New s_EhsHeader
                            DBCommon.SetProperty(DBReader("Item"), dc_EhsHeader.Item)
                            DBCommon.SetProperty(DBReader("Text"), dc_EhsHeader.Text)
                            dc_EhsHeaderList.Add(dc_EhsHeader)

                        End While

                        ehsHeaderListForPersonalize = dc_EhsHeaderList
                    End Using
                End Using
            End Using

            Me.EhsHeaderListForPersonalize = ehsHeaderListForPersonalize
        End Sub

        ''' <summary>
        ''' データベースから拠点初期設定のデータを読み込む（拠点ソート）
        ''' </summary>
        public Sub GetEhsHeaderLocation()
            Dim ehsHeaderListForLocation As List(Of s_EhsHeader) = New List(Of s_EhsHeader)
            '' データベースから全てのデータを読み込む SQL 文字列を生成する
            Dim sb_SQL As StringBuilder = New StringBuilder()

            sb_SQL.AppendLine("SELECT ")
            sb_SQL.AppendLine("    SEH.[Item], ")
            sb_SQL.AppendLine("    SEH.[Text] ")
            sb_SQL.AppendLine("FROM ")
            sb_SQL.AppendLine("    [s_EhsHeader] AS SEH ")
            sb_SQL.AppendLine("WHERE ")
            sb_SQL.AppendLine("    SEH.[LocationCode] IN (@gl, @locationCode) ")
            sb_SQL.AppendLine("ORDER BY ")
            sb_SQL.AppendLine("    CASE WHEN SEH.[LocationCode] = @gl THEN 0 ELSE 1 END, ")
            sb_SQL.AppendLine("    SEH.[SortOrder] ")

            Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.Parameters.Clear()
                    DBCommand.Parameters.AddWithValue("gl", Common.LOCATION_GL)
                    If Not String.IsNullOrEmpty(Me.LocationCode) Then
                        DBCommand.Parameters.AddWithValue("locationCode", Me.LocationCode)
                    End If

                    DBCommand.CommandText = sb_SQL.ToString()

                    ' 実行
                    DBConn.Open()
                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader()
                        While DBReader.Read
                            Dim dc_EhsHeader As s_EhsHeader = New s_EhsHeader
                            DBCommon.SetProperty(DBReader("Item"), dc_EhsHeader.Item)
                            DBCommon.SetProperty(DBReader("Text"), dc_EhsHeader.Text)

                            ehsHeaderListForLocation.Add(dc_EhsHeader)
                        End While
                    End Using
                End Using
            End Using

            Me.EhsHeaderListForLocation = ehsHeaderListForLocation
        End Sub

    End Class

End Namespace


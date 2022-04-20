Option Explicit On
Option Infer Off
Option Strict On

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Text
Imports System.Data.SqlClient
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess.Join
    ''' <summary>
    ''' v_UserAll データクラス
    ''' </summary>
    Public Class v_UserAll

        Protected _UserID As Integer = 0
        Protected _Name As String = String.Empty

        ''' <summary> 
        ''' UserID  を設定、または取得する 
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
        ''' Name  を設定、または取得する 
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

    ''' <summary>
    ''' v_UserAllList データクラス
    ''' </summary>
    Public Class v_UserAllList
        Inherits List(Of v_UserAll)

        ''' <summary> 
        ''' コンストラクタ
        ''' </summary> 
        Public Sub New()

        End Sub

       
        ''' <summary>
        ''' QuoUserドロップダウンリスト設定
        ''' </summary>
        ''' <param name="Combo">ドロップダウンリスト</param>
        ''' <param name="QuoLocationCode">QuoLocationCode</param>
        ''' <remarks></remarks>
        Public Sub SetQuoUserDropDownList(ByVal Combo As System.Web.UI.WebControls.ListControl,ByVal QuoLocationCode As String)
            
             Dim sb_SQL As New StringBuilder()
            sb_SQL.AppendLine("SELECT")
            sb_SQL.AppendLine("    RFQH.[QuoUserID],")
            sb_SQL.AppendLine("    VUA.[Name] AS QuoUserName")
            sb_SQL.AppendLine("FROM")
            sb_SQL.AppendLine("    [RFQHeader] AS RFQH,")
            sb_SQL.AppendLine("    [v_UserAll] AS VUA")
            sb_SQL.AppendLine("WHERE")
            sb_SQL.AppendLine("    RFQH.[QuoUserID] = VUA.[UserID] AND")
            sb_SQL.AppendLine("    RFQH.[QuoLocationCode] = @QuoLocationCode")
            sb_SQL.AppendLine("GROUP BY")
            sb_SQL.AppendLine("    RFQH.[QuoUserID],")
            sb_SQL.AppendLine("    VUA.[Name]")
            sb_SQL.AppendLine("ORDER BY")
            sb_SQL.AppendLine("    [QuoUserName] ASC")


            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = sb_SQL.ToString
                    DBCommand.Parameters.AddWithValue("QuoLocationCode", QuoLocationCode)
                    DBConn.Open()
                    Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
                    While DBReader.Read()
                        Dim dc_Data As New v_UserAll
                        SetProperty(DBReader("QuoUserID"), dc_Data.UserID)
                        SetProperty(DBReader("QuoUserName"), dc_Data.Name)
                    End While
                    DBReader.Close()
                End Using
            End Using

            Combo.Items.Clear()
            Combo.Items.Add(New ListItem(String.Empty, String.Empty))

            For Each User As v_UserAll In Me
                Combo.Items.Add(New ListItem(User.Name, User.UserID.ToString))
            Next
        End Sub

        ''' <summary>
        ''' EnqUserドロップダウンリスト設定
        ''' </summary>
        ''' <param name="Combo">ドロップダウンリスト</param>
        ''' <param name="EnqLocationCode">EnqLocationCode</param>
        ''' <remarks></remarks>
        Public Sub SetEnqUserDropDownList(ByVal Combo As System.Web.UI.WebControls.ListControl,ByVal EnqLocationCode As String)
            
             Dim sb_SQL As New StringBuilder()
            sb_SQL.AppendLine("SELECT")
            sb_SQL.AppendLine("    RFQH.[EnqUserID],")
            sb_SQL.AppendLine("    VUA.[Name] AS EnqUserName")
            sb_SQL.AppendLine("FROM")
            sb_SQL.AppendLine("    [RFQHeader] AS RFQH,")
            sb_SQL.AppendLine("    [v_UserAll] AS VUA")
            sb_SQL.AppendLine("WHERE")
            sb_SQL.AppendLine("    RFQH.[EnqUserID] = VUA.[UserID] AND")
            sb_SQL.AppendLine("    RFQH.[EnqLocationCode] = @EnqLocationCode")
            sb_SQL.AppendLine("GROUP BY")
            sb_SQL.AppendLine("    RFQH.[EnqUserID],")
            sb_SQL.AppendLine("    VUA.[Name]")
            sb_SQL.AppendLine("ORDER BY")
            sb_SQL.AppendLine("    [EnqUserName] ASC")


            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = sb_SQL.ToString
                    DBCommand.Parameters.AddWithValue("EnqLocationCode", EnqLocationCode)
                    DBConn.Open()
                    Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
                    While DBReader.Read()
                        Dim dc_Data As New v_UserAll
                        SetProperty(DBReader("EnqUserID"), dc_Data.UserID)
                        SetProperty(DBReader("EnqUserName"), dc_Data.Name)
                    End While
                    DBReader.Close()
                End Using
            End Using

            Combo.Items.Clear()
            Combo.Items.Add(New ListItem(String.Empty, String.Empty))

            For Each User As v_UserAll In Me
                Combo.Items.Add(New ListItem(User.Name, User.UserID.ToString))
            Next
        End Sub
    End Class

End Namespace


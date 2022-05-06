Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess.Join

    ''' <summary> 
    ''' v_Country データクラス 
    ''' </summary> 
    ''' <remarks>
    ''' <para>Generated by データクラス自動生成ツール Ver 4.1</para>
    ''' </remarks>
    Public Class v_Country


        Protected _CountryCode As String = String.Empty
        Protected _CountryName As String = String.Empty

        ''' <summary> 
        ''' CountryCode を設定、または取得する 
        ''' </summary> 
        Public Property CountryCode() As String
            Get
                Return _CountryCode
            End Get
            Set(ByVal value As String)
                _CountryCode = value
            End Set
        End Property

        ''' <summary> 
        ''' CountryName を設定、または取得する 
        ''' </summary> 
        Public Property CountryName() As String
            Get
                Return _CountryName
            End Get
            Set(ByVal value As String)
                _CountryName = value
            End Set
        End Property

        ''' <summary> 
        ''' コンストラクタ
        ''' </summary> 
        Public Sub New()

        End Sub

    End Class

    ''' <summary> 
    ''' v_Country リストクラス 
    ''' </summary> 
    Public Class v_CountryList
        Inherits List(Of v_Country)

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
            Value.AppendLine("SELECT DISTINCT")
            Value.AppendLine("    [v_Country].[CountryCode],")
            Value.AppendLine("    [v_Country].[CountryName]")
            Value.AppendLine("FROM")
            Value.AppendLine("    [v_Country] ")
            Value.AppendLine("INNER JOIN")
            Value.AppendLine("    [Supplier]")
            Value.AppendLine("ON")
            Value.AppendLine("    v_Country.CountryCode = Supplier.CountryCode")
            Value.AppendLine("ORDER BY ")
            Value.AppendLine("    v_Country.CountryName")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = Value.ToString()
                    DBConn.Open()
                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader()
                        While DBReader.Read()
                            Dim dc_Data As New v_Country
                            SetProperty(DBReader("CountryCode"), dc_Data.CountryCode)
                            SetProperty(DBReader("CountryName"), dc_Data.CountryName)
                            Me.Add(dc_Data)
                        End While
                    End Using
                End Using
            End Using
        End Sub
        ''' <summary>
        ''' v_Countryドロップダウンリスト設定
        ''' </summary>
        ''' <param name="Combo">ドロップダウンリスト</param>
        ''' <remarks></remarks>
        Public Sub Setv_CountryDropDownList(ByVal Combo As System.Web.UI.WebControls.ListControl)
            Combo.Items.Clear()
            Me.Load()

            Combo.Items.Add(New ListItem(String.Empty, String.Empty))

            For Each v_Country As v_Country In Me
                Combo.Items.Add(New ListItem(v_Country.CountryCode, v_Country.CountryCode))
            Next
        End Sub
    End Class

End Namespace

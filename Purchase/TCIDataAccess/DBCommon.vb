Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports System.Data.SqlClient
Imports System.Text.RegularExpressions

Namespace TCIDataAccess

    ''' <summary>
    ''' DBCommon クラス
    ''' </summary>
    ''' <remarks>
    ''' <para>共通の定数および関数を定義する。</para>
    ''' <para>Generated by データクラス自動生成ツール Ver 4.1</para>
    ''' </remarks>
    Public Class DBCommon

        '''' <summary>
        '''' データベース接続文字列
        '''' </summary>
        '''' <remarks></remarks>
        Public Shared ReadOnly DB_CONNECT_STRING As String = System.Configuration.ConfigurationManager.ConnectionStrings("DatabaseConnect").ConnectionString

#Region "User-Defined Constant"

#End Region 'User-Defined Constant End

        ''' <summary>
        ''' インスタンス生成防止用
        ''' </summary>
        Private Sub New()
        End Sub

        ''' <summary>
        ''' 空文字列を DBNull 値に変換する。
        ''' </summary>
        ''' <param name="Value">変換前文字列</param>
        ''' <returns>変換後のオブジェクト</returns>
        ''' <remarks></remarks>
        Public Shared Function ConvertEmptyStringToNull(ByVal Value As String) As Object

            Return IIf(String.IsNullOrEmpty(Value), System.DBNull.Value, Value)

        End Function

        ''' <summary>
        ''' 未初期化 Date を DBNull 値に変換する。
        ''' </summary>
        ''' <param name="Value">変換前日付</param>
        ''' <returns>変換後のオブジェクト</returns>
        ''' <remarks></remarks>
        Public Shared Function ConvertDefaultDateToNull(ByVal Value As Date) As Object

            Return IIf(Value.Equals(Nothing), System.DBNull.Value, Value)

        End Function

        ''' <summary>
        ''' 未初期化 DateTime を DBNull 値に変換する。
        ''' </summary>
        ''' <param name="Value">変換前日付時刻</param>
        ''' <returns>変換後のオブジェクト</returns>
        ''' <remarks></remarks>
        Public Shared Function ConvertDefaultDateTimeToNull(ByVal Value As DateTime) As Object

            Return IIf(Value.Equals(Nothing), System.DBNull.Value, Value)

        End Function

        ''' <summary>
        ''' Nothing を DBNull 値に変換する。
        ''' </summary>
        ''' <param name="Value">変換前数値</param>
        ''' <returns>変換後のオブジェクト</returns>
        ''' <remarks></remarks>
        Public Shared Function ConvertNothingToNull(ByVal Value As Integer?) As Object

            Return IIf(Value.HasValue, Value, System.DBNull.Value)

        End Function
        ''' <summary>
        ''' Nothing を DBNull 値に変換する。
        ''' </summary>
        ''' <param name="Value">変換前数値</param>
        ''' <returns>変換後のオブジェクト</returns>
        ''' <remarks></remarks>
        Public Shared Function ConvertNothingToNull(ByVal Value As Decimal?) As Object

            Return IIf(Value.HasValue, Value, System.DBNull.Value)

        End Function
        ''' <summary>
        ''' Nothing を DBNull 値に変換する。
        ''' </summary>
        ''' <param name="Value">変換前数値</param>
        ''' <returns>変換後のオブジェクト</returns>
        ''' <remarks></remarks>
        Public Shared Function ConvertNothingToNull(ByVal Value As Double?) As Object

            Return IIf(Value.HasValue, Value, System.DBNull.Value)

        End Function
        ''' <summary>
        ''' Nothing を DBNull 値に変換する。
        ''' </summary>
        ''' <param name="Value">変換前バイナリデータ</param>
        ''' <returns>変換後のオブジェクト</returns>
        ''' <remarks></remarks>
        Public Shared Function ConvertNothingToNull(ByVal Value As Byte()) As Object

            Return IIf(Value Is Nothing, System.DBNull.Value, Value)

        End Function

        ''' <summary>
        ''' DB値がNULL以外の場合にプロパティに値を格納する
        ''' </summary>
        ''' <param name="DbValue">DB値</param>
        ''' <param name="TargetProperty">格納するプロパティ</param>
        ''' <remarks></remarks>
        Public Shared Sub SetProperty(ByVal DbValue As Object, ByRef TargetProperty As String)
            If System.DBNull.Value.Equals(DbValue) = False Then
                TargetProperty = Convert.ToString(DbValue)
            End If
        End Sub
        ''' <summary>
        ''' DB値がNULL以外の場合にプロパティに値を格納する
        ''' </summary>
        ''' <param name="DbValue">DB値</param>
        ''' <param name="TargetProperty">格納するプロパティ</param>
        ''' <remarks></remarks>
        Public Shared Sub SetProperty(ByVal DbValue As Object, ByRef TargetProperty As Integer)
            If System.DBNull.Value.Equals(DbValue) = False Then
                TargetProperty = Convert.ToInt32(DbValue)
            End If
        End Sub
        ''' <summary>
        ''' DB値がNULL以外の場合にプロパティに値を格納する
        ''' </summary>
        ''' <param name="DbValue">DB値</param>
        ''' <param name="TargetProperty">格納するプロパティ</param>
        ''' <remarks></remarks>
        Public Shared Sub SetProperty(ByVal DbValue As Object, ByRef TargetProperty As Integer?)
            If System.DBNull.Value.Equals(DbValue) = False Then
                TargetProperty = Convert.ToInt32(DbValue)
            End If
        End Sub
        ''' <summary>
        ''' DB値がNULL以外の場合にプロパティに値を格納する
        ''' </summary>
        ''' <param name="DbValue">DB値</param>
        ''' <param name="TargetProperty">格納するプロパティ</param>
        ''' <remarks></remarks>
        Public Shared Sub SetProperty(ByVal DbValue As Object, ByRef TargetProperty As Long)
            If System.DBNull.Value.Equals(DbValue) = False Then
                TargetProperty = Convert.ToInt64(DbValue)
            End If
        End Sub
        ''' <summary>
        ''' DB値がNULL以外の場合にプロパティに値を格納する
        ''' </summary>
        ''' <param name="DbValue">DB値</param>
        ''' <param name="TargetProperty">格納するプロパティ</param>
        ''' <remarks></remarks>
        Public Shared Sub SetProperty(ByVal DbValue As Object, ByRef TargetProperty As Double)
            If System.DBNull.Value.Equals(DbValue) = False Then
                TargetProperty = Convert.ToDouble(DbValue)
            End If
        End Sub
        ''' <summary>
        ''' DB値がNULL以外の場合にプロパティに値を格納する
        ''' </summary>
        ''' <param name="DbValue">DB値</param>
        ''' <param name="TargetProperty">格納するプロパティ</param>
        ''' <remarks></remarks>
        Public Shared Sub SetProperty(ByVal DbValue As Object, ByRef TargetProperty As Double?)
            If System.DBNull.Value.Equals(DbValue) = False Then
                TargetProperty = Convert.ToDouble(DbValue)
            End If
        End Sub
        ''' <summary>
        ''' DB値がNULL以外の場合にプロパティに値を格納する
        ''' </summary>
        ''' <param name="DbValue">DB値</param>
        ''' <param name="TargetProperty">格納するプロパティ</param>
        ''' <remarks></remarks>
        Public Shared Sub SetProperty(ByVal DbValue As Object, ByRef TargetProperty As Decimal)
            If System.DBNull.Value.Equals(DbValue) = False Then
                TargetProperty = Convert.ToDecimal(DbValue)
            End If
        End Sub
        ''' <summary>
        ''' DB値がNULL以外の場合にプロパティに値を格納する
        ''' </summary>
        ''' <param name="DbValue">DB値</param>
        ''' <param name="TargetProperty">格納するプロパティ</param>
        ''' <remarks></remarks>
        Public Shared Sub SetProperty(ByVal DbValue As Object, ByRef TargetProperty As Decimal?)
            If System.DBNull.Value.Equals(DbValue) = False Then
                TargetProperty = Convert.ToDecimal(DbValue)
            End If
        End Sub
        ''' <summary>
        ''' DB値がNULL以外の場合にプロパティに値を格納する
        ''' </summary>
        ''' <param name="DbValue">DB値</param>
        ''' <param name="TargetProperty">格納するプロパティ</param>
        ''' <remarks></remarks>
        Public Shared Sub SetProperty(ByVal DbValue As Object, ByRef TargetProperty As DateTime)
            If System.DBNull.Value.Equals(DbValue) = False Then
                TargetProperty = Convert.ToDateTime(DbValue)
            End If
        End Sub
        ''' <summary>
        ''' DB値がNULL以外の場合にプロパティに値を格納する
        ''' </summary>
        ''' <param name="DbValue">DB値</param>
        ''' <param name="TargetProperty">格納するプロパティ</param>
        ''' <remarks></remarks>
        Public Shared Sub SetProperty(ByVal DbValue As Object, ByRef TargetProperty As Boolean)
            If System.DBNull.Value.Equals(DbValue) = False Then
                TargetProperty = Convert.ToBoolean(DbValue)
            End If
        End Sub
        ''' <summary>
        ''' DB値がNULL以外の場合にプロパティに値を格納する
        ''' </summary>
        ''' <param name="DbValue">DB値</param>
        ''' <param name="TargetProperty">格納するプロパティ</param>
        ''' <remarks></remarks>
        Public Shared Sub SetProperty(ByVal DbValue As Object, ByRef TargetProperty As Byte())
            If System.DBNull.Value.Equals(DbValue) = False Then
                TargetProperty = CType(DbValue, Byte())
            End If
        End Sub
        ''' <summary>
        ''' SQL文のWHERE句を追加
        ''' この関数で生成するWhere句はテキストボックス・ドロップダウンリストで入力された文字列のみです。
        ''' </summary>
        ''' <param name="WhereClause">検索SQL文のWhere句の変数</param>
        ''' <param name="SQL">Whereに追加するSQL文</param>
        ''' <returns>WhereClause：RFQHeader検索用WHERE句</returns>
        ''' <remarks></remarks>
        Public Shared Function AddMultipleListItemWhereClauseSQL(ByVal WhereClause As String, ByVal SQL As String) As String
            ' SQLが設定されていなければ、特に何もせずWhereClauseを返却する
            If String.IsNullOrEmpty(SQL) Then
                Return WhereClause
            End If

            ' AND を先に付ける必要があるか確認
            If String.IsNullOrEmpty(WhereClause) Then
                    WhereClause = SQL
                Else 
                    WhereClause = WhereClause & " AND " & SQL
            End If

            Return WhereClause

        End Function
        ''' <summary>
        ''' SQL文のWHERE句を追加
        ''' この関数で生成するWhere句はテキストボックス・ドロップダウンリストで入力された文字列のみです。
        ''' </summary>
        ''' <param name="WhereClause">検索SQL文のWhere句の変数</param>
        ''' <param name="ItemName">検索項目名</param>
        ''' <param name="ItemValue">画面で入力または選択された値</param>
        ''' <returns>WhereClause：RFQHeader検索用WHERE句</returns>
        ''' <remarks></remarks>
        Public Shared Function AddRFQWhereClauseSQL(ByVal WhereClause As String, ByVal ItemName As String, ByVal ItemValue As String) As String
            ' ItemNameが設定されていなければ、特に何もせずWhereClauseを返却する
            If String.IsNullOrEmpty(ItemValue) Then
                Return WhereClause
            End If

            ' AND を先に付ける必要があるか確認
            If String.IsNullOrEmpty(WhereClause) Then
                    WhereClause = ItemName
                Else 
                    WhereClause = WhereClause & " AND " & ItemName
            End If

            Return WhereClause

        End Function
        ''' <summary>
        ''' SQL文のIN句を生成します
        ''' </summary>
        ''' <param name="ItemName">複数入力の可能性がある項目名</param>
        ''' <param name="ItemValues">複数入力の可能性がある項目の値</param>
        ''' <returns>st_inClauseSQL：SQL文のIN句</returns>
        ''' <remarks></remarks>
        Public Shared Function CreateRFQInClauseSQL(ByVal ItemName As String, ByVal ItemValues() As String) As String
            Dim i_Count As Integer = 0

            '値が複数設定される可能性がある項目のIDのセット
            Dim st_ItemID As String = ItemName & i_Count
           
            Dim st_inClauseSQL As String = ""

            '値が複数設定される可能性がある項目のループ処理
            For Each ItemValue As String In ItemValues
                'DBに格納されているデータは半角のため、画面で全角文字列で入力されていた場合、文字列を半角文字列に変換
                Dim st_RequestValue As String = StrConv(ItemValue, VbStrConv.Narrow)
                If String.IsNullOrEmpty(st_RequestValue) Then
                    Continue For
                End If

                ' SQL 文への追加
                If String.IsNullOrEmpty(st_inClauseSQL) Then
                    st_inClauseSQL = " rfh." & ItemName & " In (" & "@" & st_ItemID
                Else
                    st_inClauseSQL = st_inClauseSQL & "," & "@" & st_ItemID
                End If
                i_Count = i_Count + 1
                st_ItemID = ItemName & i_Count
            Next

            'IN句に値が設定された場合、IN句を閉じる
            If Not String.IsNullOrEmpty(st_inClauseSQL) Then
                st_inClauseSQL = st_inClauseSQL & ")"
            End If

            Return st_inClauseSQL

        End Function
        ''' <summary>
        ''' SQL文のIN句に値を設定する
        ''' </summary>
        ''' <param name="DBCommand">DBCommand</param>
        ''' <param name="ItemName">複数入力の可能性がある項目名</param>
        ''' <param name="ItemValues">複数入力の可能性がある項目の値</param>
        ''' <remarks></remarks>
        Public Shared Sub SetParamInClauseSQL(ByRef DBCommand As SqlCommand,ByVal ItemName As String, ByVal ItemValues() As String)
            Dim i_Count As Integer = 0

            '値が複数設定される可能性がある項目のIDのセット
            Dim st_ItemID As String = ItemName & i_Count
           
            '値が複数設定される可能性がある項目のループ処理
            For Each ItemValue As String In ItemValues
                'DBに格納されているデータは半角のため、画面で全角文字列で入力されていた場合、文字列を半角文字列に変換
                Dim st_RequestValue As String = StrConv(ItemValue, VbStrConv.Narrow)
                If String.IsNullOrEmpty(st_RequestValue) Then
                    Continue For
                End If

                ' SQL 文への追加
                DBCommand.Parameters.AddWithValue(st_ItemID, ItemValue)
                i_Count = i_Count + 1
                st_ItemID = ItemName & i_Count
            Next
        End Sub
        ''' <summary>
        ''' 複数選択可能なドロップダウンリストのSQL文で使用するIN句を生成します
        ''' </summary>
        ''' <param name="ItemName">複数選択可能なドロップダウンリスト項目名</param>
        ''' <param name="ListItems">複数選択可能なドロップダウンリスト</param>
        ''' <returns>st_inClauseSQL：SQL文のIN句</returns>
        ''' <remarks></remarks>
        Public Shared Function CreateMultipleSelectionInClauseSQL(ByVal ItemName As String,ByVal ListItems As ListItemCollection) As String
            'Territory条件
            Dim StrComma As String = String.Empty
            Dim i_Count As Integer = 0
            Dim st_ItemID As String = ItemName & i_Count
            Dim st_inClauseSQL As String = String.Empty
            Dim st_isNullSQL As String = String.Empty
            Dim st_ReturnSQL As String = String.Empty

            If ItemName = "Purpose" Then
                For Each Purpose As ListItem In ListItems
                    If Purpose.Value = "ALL" And Purpose.Selected Then
                        Exit For
                    End If
                    'CheckboxListのチェックON判定
                    If Purpose.Selected Then
                        If i_Count = 0 Then
                            st_inClauseSQL = ItemName & " IN("
                        Else
                            'Purposeの二つ目以降の項目にOR条件を設定する
                            StrComma = ", "
                        End If
                        st_inClauseSQL = st_inClauseSQL + StrComma + "@" + st_ItemID

                        i_Count = i_Count + 1
                        st_ItemID = ItemName & i_Count
                    End If
                Next
            Else
                For Each Territory As ListItem In ListItems
                    'CheckboxListのチェックON判定
                    If Territory.Selected = False Then
                        Continue For
                    End If
                    If Territory.Value = Common.DIRECT Then
                        st_isNullSQL = ItemName & " IS NULL"
                    Else
                        If i_Count = 0 Then
                            st_inClauseSQL = ItemName & " IN("
                        Else
                            'Territoryの二つ目以降の項目にOR条件を設定する
                            StrComma = ", "
                        End If
                        st_inClauseSQL = st_inClauseSQL + StrComma + "@" + st_ItemID
                        
                        i_Count = i_Count + 1
                        st_ItemID = ItemName & i_Count
                    End If
                Next
            End If

            'IN句に値が設定された場合、IN句を閉じる
            If Not String.IsNullOrEmpty(st_inClauseSQL) Then
                st_inClauseSQL = st_inClauseSQL & ")"
            End If
            If Not String.IsNullOrEmpty(st_isNullSQL) And Not String.IsNullOrEmpty(st_inClauseSQL )  Then
                st_ReturnSQL = "(" & st_isNullSQL & " OR " & st_inClauseSQL & ")"
            Else If Not String.IsNullOrEmpty(st_isNullSQL) Then
                st_ReturnSQL = st_isNullSQL
            Else If Not String.IsNullOrEmpty(st_inClauseSQL) Then
                st_ReturnSQL = st_inClauseSQL
            End If

            Return st_ReturnSQL

        End Function
        ''' <summary>
        ''' 複数選択可能なドロップダウンリストのSQL文で使用するIN句に値を設定する
        ''' </summary>
        ''' <param name="DBCommand">DBCommand</param>
        ''' <param name="ItemName">複数選択可能なドロップダウンリスト項目名</param>
        ''' <param name="ListItems">複数選択可能なドロップダウンリスト</param>
        ''' <remarks></remarks>
        Public Shared Sub SetPramMultipleSelectionInClauseSQL(ByRef DBCommand As SqlCommand,ByVal ItemName As String,ByVal ListItems As ListItemCollection)
            'Territory条件
            Dim i_Count As Integer = 0
            Dim st_ItemID As String = ItemName & i_Count

            If ItemName = "Purpose" Then
                For Each Purpose As ListItem In ListItems
                    If Purpose.Value = "ALL" And Purpose.Selected Then
                        Exit For
                    End If
                    'CheckboxListのチェックON判定
                    If Purpose.Selected Then
                        DBCommand.Parameters.AddWithValue(st_ItemID, Purpose.Value)
                        i_Count = i_Count + 1
                        st_ItemID = ItemName & i_Count
                    End If
                Next
            Else
                For Each Territory As ListItem In ListItems
                    'CheckboxListのチェックON判定
                    If Territory.Selected Then
                        DBCommand.Parameters.AddWithValue(st_ItemID, Territory.Value)
                        i_Count = i_Count + 1
                        st_ItemID = ItemName & i_Count
                    End If
                Next
            End If
        End Sub
        ''' <summary>
        ''' Territoryが選択されているか(チェックがあるか)確認する
        ''' </summary>
        ''' <param name="TerritoryItems">TerritoryのItemCollection</param>
        ''' <returns>True:チェックあり,False：チェックなし</returns>
        ''' <remarks></remarks>
        Public Shared Function isTerritoryCheckd(ByVal TerritoryItems As ListItemCollection) As Boolean
            Dim isCheckd As Boolean = False
            For Each Territory As ListItem In TerritoryItems
                'CheckboxListのチェックON判定
                If Territory.Selected Then
                    isCheckd = True
                    Exit For
                End If
            Next

            Return isCheckd
        End Function
#Region "User-Defined Methods"

#End Region 'User-Defined Methods End

    End Class

End Namespace

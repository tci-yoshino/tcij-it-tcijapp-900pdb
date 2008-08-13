Option Strict On

''' <summary>
''' Common クラス
''' </summary>
''' <remarks>共通の定数および関数を定義する。</remarks>
Public Class Common

    ''' <summary>
    ''' データベース接続文字列
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly DB_CONNECT_STRING As String = ConfigurationManager.ConnectionStrings("DatabaseConnect").ConnectionString

    ''' <summary>
    ''' 拠点コードがダイレクトであることを表す文字列
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly DIRECT As String = "Direct"

    ''' <summary>
    ''' 日付フォーマット (時刻なし)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly DATE_FORMAT As String = "yyyy-MM-dd"

    ''' <summary>
    ''' 日付フォーマット (時刻あり)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly DATETIME_FORMAT As String = "yyyy-MM-dd HH:mm:ss"

    ''' <summary>
    ''' 日付フォーマット正規表現 (必須)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly DATE_REGEX As String = "\d{4}-\d{1,2}-\d{1,2}"

    ''' <summary>
    ''' 日付フォーマット正規表現 (任意)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly DATE_REGEX_OPTIONAL As String = "\d{4}-\d{1,2}-\d{1,2}|"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 10 桁, 小数 3 桁。必須)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly DECIMAL_10_3_REGEX As String = "^\d{1,10}(|\.)$|^\d{0,10}\.\d{1,3}$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 10 桁, 小数 3 桁。任意)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly DECIMAL_10_3_REGEX_OPTIONAL As String = "^\d{1,10}(|\.)$|^\d{0,10}\.\d{1,3}$|^$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 7 桁, 小数 3 桁。必須)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly DECIMAL_7_3_REGEX As String = "^\d{1,7}(|\.)$|^\d{0,7}\.\d{1,3}$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 7 桁, 小数 3 桁。任意)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly DECIMAL_7_3_REGEX_OPTIONAL As String = "^\d{1,7}(|\.)$|^\d{0,7}\.\d{1,3}$|^$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 5 桁, 小数 3 桁。必須)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly DECIMAL_5_3_REGEX As String = "^\d{1,5}(|\.)$|^\d{0,5}\.\d{1,3}$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 5 桁, 小数 3 桁。任意)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly DECIMAL_5_3_REGEX_OPTIONAL As String = "^\d{1,5}(|\.)$|^\d{0,5}\.\d{1,3}$|^$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 5 桁。必須)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly INT_5_REGEX As String = "^\d{1,5}$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 5 桁。任意)
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly INT_5_REGEX_OPTIONAL As String = "^\d{1,5}$|^$"

    ''' <summary>
    ''' メッセージ 「レコードはありません」
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly MSG_NO_DATA_FOUND As String = "No data found."

    ''' <summary>
    ''' エラーメッセージ 「不正なパラメータを受け取りました」
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly ERR_INVALID_PARAMETER As String = "SYSTEM ERROR: Invalid parameter supplied."

    ''' <summary>
    ''' エラーメッセージ 「○○は必須入力項目です」
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly ERR_REQUIRED_FIELD As String = " must be specified."

    ''' <summary>
    ''' エラーメッセージ 「○○は正しいフォーマットではありません」
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly ERR_INCORRECT_FORMAT As String = " is not in the correct format."

    ''' <summary>
    ''' エラーメッセージ 「○○はカレンダーにない日付です」
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly ERR_INVALID_DATE As String = " is an invalid date."

    ''' <summary>
    ''' エラーメッセージ 「○○は数値として不正です」
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly ERR_INVALID_NUMBER As String = " is an invalid number."

    ''' <summary>
    ''' エラーメッセージ 「検索条件に一致するレコードがありません」
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly ERR_NO_MATCH_FOUND As String = "No match found."

    Private Const LOCATION_JP As String = "JP"

    ''' <summary>
    ''' ローカル時間を取得する。
    ''' </summary>
    ''' <param name="LocationCode">拠点コード</param>
    ''' <param name="DatabaseTime">データベース時間 (JST)</param>
    ''' <returns>ローカル時間</returns>
    ''' <remarks></remarks>
    Public Shared Function GetLocalTime(ByVal LocationCode As String, ByVal DatabaseTime As Date) As String
        Dim st_ErrMsg As String = String.Empty
        Dim da_Date As Date = DatabaseTime

        If Not IsDate(da_Date) Then
            Return String.Empty
        End If

        If TCICommon.Func.ConvertDate(da_Date, LOCATION_JP, LocationCode, st_ErrMsg) < 0 Then
            Throw New Exception(String.Format("TCICommon.ConvertDate: {0}", st_ErrMsg))
        End If

        Return Format(da_Date, DATE_FORMAT)

    End Function

    ''' <summary>
    ''' データベース時間を取得する。
    ''' </summary>
    ''' <param name="LocationCode">拠点コード</param>
    ''' <param name="LocalTime">ローカル時間</param>
    ''' <returns>データベース時間 (JST)</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDatabaseTime(ByVal LocationCode As String, ByVal LocalTime As Date) As String
        Dim st_ErrMsg As String = String.Empty
        Dim da_Date As Date = LocalTime

        If TCICommon.Func.ConvertDate(da_Date, LocationCode, LOCATION_JP, st_ErrMsg) < 0 Then
            Throw New Exception(String.Format("TCICommon.ConvertDate: {0}", st_ErrMsg))
        End If

        Return Format(da_Date, DATE_FORMAT)

    End Function

    ''' <summary>
    ''' データベース時間を取得する。
    ''' </summary>
    ''' <param name="LocationCode">拠点コード</param>
    ''' <param name="LocalTime">ローカル時間</param>
    ''' <returns>データベース時間 (JST)</returns>
    ''' <remarks></remarks>
    Public Shared Function GetDatabaseTime(ByVal LocationCode As String, ByVal LocalTime As String) As Object
        Dim st_ErrMsg As String = String.Empty
        Dim obj_Date As Object = ConvertStringToDate(LocalTime)

        If IsDBNull(obj_Date) Then
            Return System.DBNull.Value
        End If

        If TCICommon.Func.ConvertDate(CDate(obj_Date), LocationCode, LOCATION_JP, st_ErrMsg) < 0 Then
            Throw New Exception(String.Format("TCICommon.ConvertDate: {0}", st_ErrMsg))
        End If

        Return obj_Date

    End Function

    ''' <summary>
    ''' 文字列を短縮する。一覧で製品名を表示する場合などに使用。
    ''' </summary>
    ''' <param name="str">文字列</param>
    ''' <returns>短縮された文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function CutShort(ByVal str As String) As String
        Dim st_Continue As String = "..."

        If str.Length <= 40 Then
            Return str
        End If

        Return str.Substring(0, 40) & st_Continue

    End Function

    ''' <summary>
    ''' LIKE 句のない SQL 文字列のサニタイジングを行う。
    ''' </summary>
    ''' <param name="SqlString">SQL 文字列</param>
    ''' <returns>サニタイジング済みの SQL 文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function SafeSqlLiteral(ByVal SqlString As String) As String

        Return SqlString.Replace("'", "''")

    End Function

    ''' <summary>
    ''' LIKE 句のある SQL 文字列のサニタイジングを行う。
    ''' パラメタライズドクエリについても使用必須。
    ''' </summary>
    ''' <param name="SqlString">SQL 文字列</param>
    ''' <returns>サニタイジング済みの SQL 文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function SafeSqlLikeClauseLiteral(ByVal SqlString As String) As String

        Return SqlString.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")

    End Function

    ''' <summary>
    ''' 空文字列を DBNull 値に変換する。
    ''' </summary>
    ''' <param name="str">文字列</param>
    ''' <returns>変換後のオブジェクト</returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertEmptyStringToNull(ByVal str As String) As Object

        Return IIf(Trim(str) Is String.Empty, System.DBNull.Value, str)

    End Function

    ''' <summary>
    ''' 文字列を Date 型に変換する。空文字列の場合は DBNull 値に変換する。
    ''' </summary>
    ''' <param name="str">文字列</param>
    ''' <returns>変換後のオブジェクト</returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertStringToDate(ByVal str As String) As Object

        If Trim(str) Is String.Empty Then
            Return System.DBNull.Value
        End If

        Return CDate("#" & str & "#")

    End Function

    ''' <summary>
    ''' 文字列を Decimal 型に変換する。空文字列の場合は DBNull 値に変換する。
    ''' </summary>
    ''' <param name="str">文字列</param>
    ''' <returns>変換後のオブジェクト</returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertStringToDec(ByVal str As String) As Object

        If Trim(str) Is String.Empty Then
            Return System.DBNull.Value
        End If

        Return CDec(str)

    End Function

    ''' <summary>
    ''' 文字列を Integer 型に変換する。空文字列の場合は DBNull 値に変換する。
    ''' </summary>
    ''' <param name="str">文字列</param>
    ''' <returns>変換後のオブジェクト</returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertStringToInt(ByVal str As String) As Object

        If Trim(str) Is String.Empty Then
            Return System.DBNull.Value
        End If

        Return CInt(str)

    End Function

End Class

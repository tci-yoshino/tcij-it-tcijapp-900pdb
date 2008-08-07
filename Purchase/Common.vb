Public Class Common
    Public Shared ReadOnly LOCATION_JP As String = "JP"

    Public Shared ReadOnly DIRECT As String = "Direct"

    Public Shared ReadOnly DATE_FORMAT As String = "yyyy-MM-dd"
    Public Shared ReadOnly DATETIME_FORMAT As String = "yyyy-MM-dd HH:mm:ss"

    Public Shared ReadOnly DATE_REGEX As String = "\d{4}-\d{1,2}-\d{1,2}"
    Public Shared ReadOnly DATE_REGEX_OPTIONAL As String = "\d{4}-\d{1,2}-\d{1,2}|"

    Public Shared ReadOnly MSG_NO_DATA_FOUND As String = "No data found."

    Public Shared ReadOnly ERR_INVALID_PARAMETER As String = "SYSTEM ERROR: Invalid parameter supplied."
    Public Shared ReadOnly ERR_REQUIRED_FIELD As String = " must be specified."
    Public Shared ReadOnly ERR_INCORRECT_FORMAT As String = " is not in the correct format."
    Public Shared ReadOnly ERR_INVALID_DATE As String = " is an invalid date."
    Public Shared ReadOnly ERR_NO_MATCH_FOUND As String = "No match found."

    Public Shared Function GetLocalTime(ByVal LocationCode As String, ByVal DatabaseTime As Date) As String
        Dim st_ErrMsg As String = ""
        Dim da_Date As Date = DatabaseTime

        If TCICommon.Func.ConvertDate(da_Date, LOCATION_JP, LocationCode, st_ErrMsg) < 0 Then
            Throw New Exception(String.Format("TCICommon.ConvertDate: {0}", st_ErrMsg))
        End If

        Return Format(da_Date, DATE_FORMAT)

    End Function

    Public Shared Function GetDatabaseTime(ByVal LocationCode As String, ByVal LocalTime As Date) As String
        Dim st_ErrMsg As String = ""
        Dim da_Date As Date = LocalTime

        If TCICommon.Func.ConvertDate(da_Date, LocationCode, LOCATION_JP, st_ErrMsg) < 0 Then
            Throw New Exception(String.Format("TCICommon.ConvertDate: {0}", st_ErrMsg))
        End If

        Return Format(da_Date, DATE_FORMAT)

    End Function

    Public Shared Function GetDatabaseTime(ByVal LocationCode As String, ByVal Localtime As String) As Object
        Dim st_ErrMsg As String = ""
        Dim da_Date As Date

        da_Date = ConvertStringToDate(Localtime)
        If IsDBNull(da_Date) Then
            Return System.DBNull.Value
        End If

        If TCICommon.Func.ConvertDate(da_Date, LocationCode, LOCATION_JP, st_ErrMsg) < 0 Then
            Throw New Exception(String.Format("TCICommon.ConvertDate: {0}", st_ErrMsg))
        End If

        Return da_Date

    End Function

    Public Shared Function CutShort(ByVal str As String) As String
        Dim st_Continue As String = "..."

        If str.Length <= 40 Then
            Return str
        End If

        Return str.Substring(0, 40) + st_Continue

    End Function

    Public Shared Function SafeSqlLiteral(ByVal SqlString As String) As String

        Return SqlString.Replace("'", "''")

    End Function

    Public Shared Function SafeSqlLikeClauseLiteral(ByVal SqlString As String) As String

        Return SqlString.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")

    End Function

    Public Shared Function ConvertEmptyStringToNull(ByVal str As String) As Object

        Return IIf(Trim(str) = "", System.DBNull.Value, str)

    End Function

    Public Shared Function ConvertStringToDate(ByVal str As String) As Object

        If Trim(str) = "" Then
            Return System.DBNull.Value
        End If

        Return CDate(str)

    End Function

    Public Shared Function ConvertStringToDec(ByVal str As String) As Object

        If Trim(str) = "" Then
            Return System.DBNull.Value
        End If

        Return CDec(str)

    End Function

    Public Shared Function ConvertStringToInt(ByVal str As String) As Object

        If Trim(str) = "" Then
            Return System.DBNull.Value
        End If

        Return CInt(str)

    End Function

End Class

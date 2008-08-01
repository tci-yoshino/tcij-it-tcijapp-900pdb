Public Class Common
    Public Shared ReadOnly LOCATION_JP As String = "JP"

    Public Shared ReadOnly DIRECT As String = "Direct"

    Public Shared ReadOnly DATE_FORMAT As String = "yyyy-MM-dd"
    Public Shared ReadOnly DATETIME_FORMAT As String = "yyyy-MM-dd HH:mm:ss"

    Public Shared ReadOnly DATE_REGEX As String = "\d{4}-\d{1,2}-\d{1,2}"
    Public Shared ReadOnly DATE_REGEX_OPTIONAL As String = "\d{4}-\d{1,2}-\d{1,2}|"

    Public Shared ReadOnly ERR_INVALID_PARAMETER As String = "SYSTEM ERROR: Invalid parameter supplied."
    Public Shared ReadOnly ERR_REQUIRED_FIELD As String = " must be specified."
    Public Shared ReadOnly ERR_INCORRECT_FORMAT As String = " is not in the correct format."
    Public Shared ReadOnly ERR_INVALID_DATE As String = " is an invalid date."

    Public Shared Function GetLocalTime(ByVal LocationCode As String, ByVal DatabaseTime As Date) As String
        Dim st_ErrMsg As String = ""
        Dim da_Date As Date = DatabaseTime

        If TCICommon.Func.ConvertDate(da_Date, LOCATION_JP, LocationCode, st_ErrMsg) < 0 Then
            Throw New Exception(String.Format("TCICommon.ConvertDate: {0}", st_ErrMsg))
        End If

        Return Format(da_Date, DATE_FORMAT)

    End Function

    Public Function GetDatabaseTime(ByVal LocationCode As String, ByVal LocalTime As Date) As String
        Dim st_ErrMsg As String = ""
        Dim da_Date As Date = LocalTime

        If TCICommon.Func.ConvertDate(da_Date, LocationCode, LOCATION_JP, st_ErrMsg) < 0 Then
            Throw New Exception(String.Format("TCICommon.ConvertDate: {0}", st_ErrMsg))
        End If

        Return Format(da_Date, DATE_FORMAT)

    End Function

    Public Function SafeSqlLiteral(ByVal SqlString As String) As String

        Return SqlString.Replace("'", "''")

    End Function

    Public Function SafeSqlLikeClauseLiteral(ByVal SqlString As String) As String

        Return SqlString.Replace("'", "''").Replace("[", "[[]").Replace("%", "[%]").Replace("_", "[_]")

    End Function

End Class

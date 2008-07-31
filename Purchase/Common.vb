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
End Class

Option Strict On
Imports System.Data.SqlClient
Imports Purchase.TCIDataAccess

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
    Public Const DIRECT As String = "Direct"

    ''' <summary>
    ''' 日付フォーマット (時刻なし)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DATE_FORMAT As String = "yyyy-MM-dd"

    ''' <summary>
    ''' 日付フォーマット (時刻あり)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DATETIME_FORMAT As String = "yyyy-MM-dd HH:mm:ss"

    ''' <summary>
    ''' 日付フォーマット正規表現 (必須)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DATE_REGEX As String = "^[0-9]{4}-[0-9]{1,2}-[0-9]{1,2}$"

    ''' <summary>
    ''' 日付フォーマット正規表現 (任意)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DATE_REGEX_OPTIONAL As String = "^[0-9]{4}-[0-9]{1,2}-[0-9]{1,2}$|^$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 10 桁, 小数 3 桁。必須)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DECIMAL_10_3_REGEX As String = "^[0-9]{1,10}(|\.)$|^[0-9]{0,10}\.[0-9]{1,3}$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 10 桁, 小数 3 桁。任意)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DECIMAL_10_3_REGEX_OPTIONAL As String = "^[0-9]{1,10}(|\.)$|^[0-9]{0,10}\.[0-9]{1,3}$|^$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 10 桁。任意)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DECIMAL_10_REGEX_OPTIONAL As String = "^[0-9]{1,10}$|^$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 7 桁, 小数 3 桁。必須)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DECIMAL_7_3_REGEX As String = "^[0-9]{1,7}(|\.)$|^[0-9]{0,7}\.[0-9]{1,3}$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 7 桁, 小数 3 桁。任意)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DECIMAL_7_3_REGEX_OPTIONAL As String = "^[0-9]{1,7}(|\.)$|^[0-9]{0,7}\.[0-9]{1,3}$|^$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 5 桁, 小数 3 桁。必須)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DECIMAL_5_3_REGEX As String = "^[0-9]{1,5}(|\.)$|^[0-9]{0,5}\.[0-9]{1,3}$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 5 桁, 小数 3 桁。任意)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const DECIMAL_5_3_REGEX_OPTIONAL As String = "^[0-9]{1,5}(|\.)$|^[0-9]{0,5}\.[0-9]{1,3}$|^$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 5 桁。必須)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const INT_5_REGEX As String = "^[0-9]{1,5}$"

    ''' <summary>
    ''' 数値フォーマット正規表現 (整数 5 桁。任意)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const INT_5_REGEX_OPTIONAL As String = "^[0-9]{1,5}$|^$"
    ''' <summary>
    ''' メールアドレスフォーマット(英数字@英数字.英数字)
    ''' </summary>
    ''' <remarks></remarks>
    Public Const EMAIL_REGEX As String = "^[A-Za-z0-9\-\._]+@[A-Za-z0-9\-_]+\.[A-Za-z0-9\-\._]+$|^$"

    ''' <summary>
    ''' URLフォーマット
    ''' </summary>
    ''' <remarks></remarks>
    Public Const URL_REGEX As String = "^[-_.!~*\'()a-zA-Z0-9;\/?:\@&=+\$,%#]+$|^$"

    ''' <summary>
    ''' Excel ContextType
    ''' </summary>
    ''' <remarks></remarks>
    Public Const EXCEL_CONTENTTYPE As String = "application/octet-stream"

    ''' <summary>
    ''' 文字数チェックで使用する。3000文字用
    ''' </summary>
    ''' <remarks></remarks>
    Public Const INT_3000 As Integer = 3000

    ''' <summary>
    ''' 文字数チェックで使用する。255文字用
    ''' </summary>
    ''' <remarks></remarks>
    Public Const INT_255 As Integer = 255

    ''' <summary>
    ''' メッセージ 「レコードはありません」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const MSG_NO_DATA_FOUND As String = "No record found."

    ''' <summary>
    ''' メッセージ 「データが新規登録されました」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const MSG_DATA_CREATED As String = "Record newly issued."

    ''' <summary>
    ''' メッセージ 「データが更新されました」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const MSG_DATA_UPDATED As String = "Record updated."

    ''' <summary>
    ''' メッセージ 「保存されました」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const MSG_DATA_SAVED As String = "Record saved."

    ''' <summary>
    ''' 「検索結果が1000以上です」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const MSG_RESULT_OVER_1000 As String = "The result is over 1000 hits as the limit.<br />Please change the criteria and search again."

    ''' <summary>
    ''' エラーメッセージ 「不正なパラメータを受け取りました」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_INVALID_PARAMETER As String = "SYSTEM ERROR: Invalid parameter supplied."

    ''' <summary>
    ''' エラーメッセージ 「○○は必須入力項目です」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_REQUIRED_FIELD As String = " must be specified."

    ''' <summary>
    ''' エラーメッセージ 「○○は正しいフォーマットではありません」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_INCORRECT_FORMAT As String = " is invalid format."

    ''' <summary>
    ''' エラーメッセージ 「○○はカレンダーにない日付です」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_INVALID_DATE As String = " is invalid date."

    ''' <summary>
    ''' エラーメッセージ 「○○は数値として不正です」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_INVALID_NUMBER As String = " is invalid number."

    ''' <summary>
    ''' エラーメッセージ 「検索条件に一致するレコードがありません」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_NO_MATCH_FOUND As String = "Your requested record does not exist."

    ''' <summary>
    ''' エラーメッセージ 「○○はマスタテーブルに登録されていません」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_DOES_NOT_EXIST As String = " can not be found in master table."

    ''' <summary>
    ''' エラーメッセージ 「データは他のユーザによって更新されました。」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_UPDATED_BY_ANOTHER_USER As String = "This record is now being updated by other user.<br />Please try again later.<br />(Any record can not be updated by two users at the same time.)"

    ''' <summary>
    ''' エラーメッセージ 「データは他のユーザによって削除されました。」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_DELETED_BY_ANOTHER_USER As String = "Your requested record has already been deleted by another user."

    ''' <summary>
    ''' エラーメッセージ 「○○には 32 文字以上登録することができません」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_OVER_32 As String = " cannot contain more than 32 characters."

    ''' <summary>
    ''' エラーメッセージ 「○○には 128 文字以上登録することができません」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_OVER_128 As String = " cannot contain more than 128 characters."

    ''' <summary>
    ''' エラーメッセージ 「○○には 255 文字以上登録することができません」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_OVER_255 As String = " cannot contain more than 255 characters."

    ''' <summary>
    ''' エラーメッセージ 「○○には 1000 文字以上登録することができません」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_OVER_1000 As String = " cannot contain more than 1000 characters."

    ''' <summary>
    ''' エラーメッセージ 「○○には 3000 文字以上登録することができません」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_OVER_3000 As String = " cannot contain more than 3000 characters."
    
    ''' <summary>
    ''' エラーメッセージ 「値が100を超えています。コピーする値は100以下である必要があります」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_ITEMS_OVER_100 As String = "Number of paste data exceeds 100 items. Pasting is allowed up to 100 items."
    
    ''' <summary>
    ''' エラーメッセージ 「未処理のコレポンが存在します」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_UNTREATED_CORRESPONDENCE As String = "There are any untreated correspondences."

    ''' <summary>
    ''' エラーメッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_CONFIDENTIAL_PRODUCT As String = "You don't have the authorization to specify this product number."

    ''' <summary>
    ''' エラーメッセージ 「○○は重複しています」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_DUPLICATE_CODE As String = " is duplicated."

    ''' <summary>
    ''' エラーメッセージ 「構造検索はできません。」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ERR_STRUCTURE_SEARCH As String = "Structure Search is not possible."

    ''' <summary>
    ''' エラーメッセージ 「○○件を越えました。条件を変更するかStructure Search画面に遷移して下さい。」
    ''' </summary>
    ''' <remarks></remarks>
    Public Const MSG_RESULT_OVER_LIMIT As String = "Search results exceeded {0} records (Display limit: {0} results only).<br />Please change your search criteria or try Structure search."


    ''  拠点情報
    ''' <summary>拠点：TCI-A </summary>
    Public Const LOCATION_US As String = "US"
    ''' <summary>拠点：TCI-E</summary>
    Public Const LOCATION_EU As String = "EU"
    ''' <summary>拠点：TCI-S</summary>
    Public Const LOCATION_CN As String = "CN"
    ''' <summary>拠点：TCI-I</summary>
    Public Const LOCATION_IN As String = "IN"
    ''' <summary>拠点：TCI-J</summary>
    Public Const LOCATION_JP As String = "JP"
    ''' <summary>拠点：GLOBAL</summary>
    Public Const LOCATION_GL As String = "GL"

    ''' <summary>
    ''' プライオリティ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const PRIORITY_A As String = "A"
    Public Const PRIORITY_B As String = "B"
    Public Const PRIORITY_AB As String = "AB"
    Public Const PRIORITY_ALL As String = "ALL"

    ''' <summary>
    ''' プライオリティ利用タイプ
    ''' </summary>
    ''' <remarks></remarks>
    Public Const PRIORITY_FOR_SEARCH As String = "Search"
    Public Const PRIORITY_FOR_RFQ_SEARCH As String = "RfqSearch"
    Public Const PRIORITY_FOR_EDIT As String = "Edit"

    ''' <summary>
    ''' RFQステータス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const RFQSTATUS_ALL As String = "ALL"

    ''' <summary>
    ''' ValidQuotation
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ValidQuotation_ALL As String = "ALL"

    ''' <summary>
    ''' 極秘表記
    ''' </summary>
    ''' <remarks></remarks>
    Public Const CONFIDENTIAL As String = "CONFIDENTIAL"

    ''' <summary>
    ''' 権限ロール
    ''' </summary>
    ''' <remarks></remarks>
    Public Const ROLE_READ_P As String = "READ_P"
    Public Const ROLE_WRITE_P As String = "WRITE_P"
    Public Const ROLE_WRITE As String = "WRITE"

    ''' <summary>
    ''' ステータス
    ''' </summary>
    ''' <remarks></remarks>
    Public Const STATUS_CLOSED As String = "C"
    Public Const STATUS_PAR_QM_FINISHED As String = "PQF"
    Public Const STATUS_PAR_PO_CANCELLED As String = "PPC"
    Public Const STATUS_CHI_PO_CANCELLED As String = "CPC"

    ' Excel出力用テンプレートの保存ディレクトリ
    ' SupplierProduct
    Public Shared ReadOnly EXCEL_TEMPLATE_DIRECTORY_SUPPLIERPRODUCT As String = ConfigurationManager.AppSettings("ReportTemplate_SupplierProduct")
    ' RFQSearch
    Public Shared ReadOnly REPORT_TEMPLATE_RFQSEARCH As String = ConfigurationManager.AppSettings("ReportTemplate_RFQSearch")

    ' Valid_Filter
    Public Const VALID_FILTER_VALID As String = "Valid Price"
    Public Const VALID_FILTER_INVALID As String = "Invalid Price"

    '--ファイル・ディレクトリ情報
    ''' <summary> ファイル・ディレクトリ情報：構造式 DB </summary>
    Public Shared ReadOnly FILE_NAME_STRUCTUREDB As String = ConfigurationManager.AppSettings("FileName_StructureDB")
    ''' <summary> ファイル・ディレクトリ情報：misearch.jar </summary>
    Public Shared ReadOnly FILE_PATH_MISEARCH As String = ConfigurationManager.AppSettings("FilePath_Misearch")
    ''' <summary> 画像なし GIF URL (相対パス) </summary>
    Public Const FILE_PATH_NOIMAGE As String = "./Image/NoImage.gif"
    '--URL書式
    ''' <summary> 画像表示の URL書式 (arg0 = RegistryNumber , arg1 = システム日付)  </summary>
    Public Shared ReadOnly NPMSURL As String = ConfigurationManager.AppSettings("NPMSURL")
    Public Const IMG_URL_FORMAT As String = "/Image.aspx?RegistryNumber={0}&Conf=Y&Key={1:yyMMddHHmmss}"

    ''' <summary>
    ''' Web.configに設定された各画面毎の定数値をリストのページあたりの件数を設定する共通処理
    ''' </summary>
    ''' <param name ="name">プログラム名（Request.Url.ToString()の形式で値を設定）</param>
    ''' <remarks></remarks>
    Public Shared ReadOnly Property LIST_ONEPAGE_ROW(name As String) As Integer
        Get
            name = System.IO.Path.GetFileNameWithoutExtension(name)
            Return Integer.Parse(System.Configuration.ConfigurationManager.AppSettings("LIST_ONEPAGE_ROW_" + name))
        End Get
    End Property
    ''' <summary>
    ''' RFQSearch画面のDownload実行時のSQL Command Timeout(秒)を指定する。
    ''' </summary>
    ''' <remarks>Web.Configに存在しない場合はNothingを返す</remarks>
    Public Shared ReadOnly Property SQL_COMMAND_TIMEOUT_RFQSearch_Download As Integer?
        Get
            Dim timeout As Integer? = Nothing
            If ConfigurationManager.AppSettings("SQL_COMMAND_TIMEOUT_RFQSearch_Download") IsNot Nothing Then
                timeout = CInt(ConfigurationManager.AppSettings("SQL_COMMAND_TIMEOUT_RFQSearch_Download"))
            End If
            Return timeout
        End Get
    End Property
    ''' <summary>
    ''' セッション情報
    ''' </summary>
    ''' <remarks></remarks>
    Public Const SESSION_ROLE_CODE As String = "Purchase.RoleCode"

    ' <summary>拠点：TCI-A </summary>
    Public Const LOCATION_CODE_US As String = "US"
    ' <summary>拠点：TCI-E</summary>
    Public Const LOCATION_CODE_EU As String = "EU"
    ' <summary>拠点：TCI-S</summary>
    Public Const LOCATION_CODE_CN As String = "CN"
    ' <summary>拠点：TCI-I</summary>
    Public Const LOCATION_CODE_IN As String = "IN"
    ' <summary>拠点：TCI-J</summary>
    Public Const LOCATION_CODE_JP As String = "JP"
    ' <summary>拠点：Global</summary>
    Public Const LOCATION_CODE_GL As String = "GL"

    ''' <summary>
    ''' ローカル時間を取得する。
    ''' </summary>
    ''' <param name="LocationCode">拠点コード</param>
    ''' <param name="DatabaseTime">データベース時間 (JST)</param>
    ''' <param name="InputHMS">時差を除く、時刻情報を持つデータを変換する場合は True を指定</param>
    ''' <param name="OutputHMS">時刻情報付きで戻り値を返す場合は True を指定</param>
    ''' <returns>ローカル時間</returns>
    ''' <remarks></remarks>
    Public Shared Function GetLocalTime(ByVal LocationCode As String, _
                                        ByVal DatabaseTime As Date, _
                                        ByVal InputHMS As Boolean, _
                                        ByVal OutputHMS As Boolean) As String
        Dim st_ErrMsg As String = String.Empty
        Dim st_Format As String = String.Empty
        Dim da_Date As Date = DatabaseTime
        IF da_Date.Equals(DateTime.MinValue)
            Return String.Empty
        End If

        If TCICommon.Func.ConvertDate(da_Date, LOCATION_JP, LocationCode, st_ErrMsg) < 0 Then
            Throw New Exception(String.Format("TCICommon.ConvertDate: {0}", st_ErrMsg))
        End If

        If InputHMS = False Then
            da_Date = DateAdd(DateInterval.Hour, 12, da_Date)
        End If

        If OutputHMS = False Then
            Return Format(da_Date, DATE_FORMAT)
        Else
            Return Format(da_Date, DATETIME_FORMAT)
        End If

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
    ''' <returns>データベース時間 (JST) 引値が空白時は DBNull.Value</returns>
    ''' <remarks>既存の同名関数ではオブジェクトキャスト時に時間情報が失われていたいた為、修正</remarks>
    Public Shared Function GetDatabaseTime(ByVal LocationCode As String, ByVal LocalTime As String) As Object
        Const ERR_ILLEGAL_TIME_FORMAT As String = "指定された時間が無効です。書式を再度ご確認下さい。"

        Dim st_ErrMsg As String = String.Empty

        '空値の時にはDBNullを返します（DB-Null更新処理が想定されるため）
        If String.IsNullOrEmpty(LocalTime) Then
            Return DBNull.Value
        End If

        If Not IsDate(LocalTime) Then
            Throw New Exception(String.Format("Common.GetDatabaseTime: {0}", ERR_ILLEGAL_TIME_FORMAT))
        End If

        Dim dt_Date As Date = New DateTime()
        dt_Date = CDate(String.Format("#{0}#", LocalTime))

        If TCICommon.Func.ConvertDate(dt_Date, LocationCode, LOCATION_JP, st_ErrMsg) < 0 Then
            Throw New Exception(String.Format("TCICommon.ConvertDate: {0}", st_ErrMsg))
        End If

        Return dt_Date

    End Function

    ''' <summary>
    ''' Privilege_Levelの取得
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared ReadOnly PRIVILEGE_LEVEL() As String = {"P", "A"}

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

        Return IIf(Trim(str) = String.Empty, System.DBNull.Value, str)

    End Function

    ''' <summary>
    ''' 文字列を Date 型に変換する。空文字列の場合は DBNull 値に変換する。
    ''' </summary>
    ''' <param name="str">文字列</param>
    ''' <returns>変換後のオブジェクト</returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertStringToDate(ByVal str As String) As Object

        If Trim(str) = String.Empty Then
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

        If Trim(str) = String.Empty Then
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

        If Trim(str) = String.Empty Then
            Return System.DBNull.Value
        End If

        Return CInt(str)

    End Function

    ''' <summary>
    ''' Boolean型をInt型に変換する
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function ConvertBoolToInt(ByVal value As Boolean) As Integer
        If value = True Then
            Return 1
        Else
            Return 0
        End If
    End Function

    ''' <summary>
    ''' 指定した条件でテーブルにデータが存在するかを返します。
    ''' </summary>
    ''' <param name="TableName">検索対象のテーブル名</param>
    ''' <param name="TableField">検索条件フィールド名</param>
    ''' <param name="CheckData">検索条件の値</param>
    ''' <returns>データが一件以上ある場合はTrue ない場合はFalseを返します。検索条件フィールドと検索条件の値で型が異なる場合はシステムエラーが発生します。</returns>
    ''' <remarks></remarks>
    Public Shared Function ExistenceConfirmation(ByVal TableName As String, ByVal TableField As String, ByVal CheckData As String) As Boolean
        '汎用存在確認チェック
        Dim st_SQLCommand As String = String.Empty
        st_SQLCommand = String.Format("SELECT COUNT(*) AS RecordCount FROM {0} WHERE {1} = @CheckData", _
                                    SafeSqlLiteral(TableName), SafeSqlLiteral(TableField))
        Try
            Using DBConn As New SqlClient.SqlConnection(DB_CONNECT_STRING), _
            DBCommand As SqlCommand = DBConn.CreateCommand()
                DBConn.Open()
                DBCommand.CommandText = st_SQLCommand
                DBCommand.Parameters.AddWithValue("CheckData", CheckData)

                Dim i_RecordCount As Integer = Convert.ToInt32(DBCommand.ExecuteScalar)
                If i_RecordCount > 0 Then
                    Return True
                End If
            End Using
        Catch ex As Exception
            Throw
        End Try
        Return False
    End Function

    ''' <summary>
    ''' レコードの更新日を yyyy-mm-dd hh:mi:ss 形式の文字列で取得します。
    ''' </summary>
    ''' <param name="TableName">検索対象のテーブル名</param>
    ''' <param name="PrimaryKey">検索条件主キー名</param>
    ''' <param name="PrimaryValue">検索条件主キーの値</param>
    ''' <returns>更新日</returns>
    ''' <remarks></remarks>
    Public Shared Function GetUpdateDate(ByVal TableName As String, ByVal PrimaryKey As String, ByVal PrimaryValue As String) As String

        Dim st_SQLCommand As String = String.Empty
        Dim st_UpdateDate As String = String.Empty
        Dim i As Integer = 0

        st_SQLCommand = String.Format("SELECT CONVERT(VARCHAR,UpdateDate,120) AS UpdateDate FROM {0} WHERE {1} = @CheckData ", _
                                    SafeSqlLiteral(TableName), SafeSqlLiteral(PrimaryKey))
        Using DBConn As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING), _
              DBCommand As SqlClient.SqlCommand = DBConn.CreateCommand()

            DBCommand.CommandText = st_SQLCommand
            DBCommand.Parameters.AddWithValue("CheckData", PrimaryValue)

            DBConn.Open()
            Dim reader As SqlClient.SqlDataReader = DBCommand.ExecuteReader()

            While reader.Read()
                If i >= 1 Then
                    Throw New Exception("Common.GetUpdateDate: 複数レコード取得されました。PrimaryKey には主キー名を指定してください。")
                    Exit While
                End If
                st_UpdateDate = reader("UpdateDate").ToString()
                i += 1
            End While
            reader.Close()
        End Using

        Return st_UpdateDate
    End Function

    ''' <summary>
    ''' 更新するレコードの更新日が指定した更新日と同一であるかを示します。
    ''' </summary>
    ''' <param name="TableName">検索対象のテーブル名</param>
    ''' <param name="TableField">検索条件フィールド名</param>
    ''' <param name="CheckData">検索条件の値</param>
    ''' <param name="UpdateDate">検索条件の更新日(yyyy-mm-dd hh:mi:ss)</param>
    ''' <returns>更新日が同一である場合は True 、そうでない場合は False を返します。</returns>
    ''' <remarks></remarks>
    Public Shared Function IsLatestData(ByVal TableName As String, ByVal TableField As String, ByVal CheckData As String, ByVal UpdateDate As String) As Boolean

        Dim st_SQLCommand As String = String.Empty
        st_SQLCommand = String.Format("SELECT COUNT(*) AS RecordCount FROM {0} WHERE {1} = @CheckData AND CONVERT(VARCHAR,UpdateDate,120) = @UpdateDate ", _
                                    SafeSqlLiteral(TableName), SafeSqlLiteral(TableField))
        Using DBConn As New SqlClient.SqlConnection(DB_CONNECT_STRING), _
              DBCommand As SqlClient.SqlCommand = DBConn.CreateCommand()

            DBCommand.CommandText = st_SQLCommand
            DBCommand.Parameters.AddWithValue("CheckData", CheckData)
            DBCommand.Parameters.AddWithValue("UpdateDate", UpdateDate)

            DBConn.Open()
            Dim i_RecordCount As Integer = Convert.ToInt32(DBCommand.ExecuteScalar)
            If i_RecordCount > 0 Then
                Return True
            End If
        End Using

        Return False
    End Function

    ''' <summary>
    ''' 対象の文字列が Integer かをチェックして結果を返します。
    ''' </summary>
    ''' <param name="str">対象となる文字列</param>
    ''' <returns>Integerの場合は True、 そうでない場合は False を返します。</returns>
    ''' <remarks></remarks>
    Public Shared Function IsInteger(ByVal str As String) As Boolean
        Dim i_Value As Integer
        If Not Integer.TryParse(str, i_Value) Then
            Return False
        End If

        Return True
    End Function

    ''' <summary>
    ''' Shift JIS換算の半角文字数を取得します。
    ''' </summary>
    ''' <param name="str">対象となる文字列</param>
    ''' <returns>半角換算の文字数</returns>
    ''' <remarks>Unicodeでは全角、半角の区別はありません。一度Shift_JISに変換後、バイト数を取得する必要があります</remarks>
    Public Shared Function GetByteCount_SJIS(ByVal str As String) As Integer
        Dim s_jis As System.Text.Encoding = System.Text.Encoding.GetEncoding("Shift_JIS")
        Return s_jis.GetByteCount(str)
    End Function

    ''' <summary>
    ''' テキストボックスの値が日付型に変換可能か評価します。
    ''' </summary>
    ''' <param name="TargetObject">対象となるTexrBoxオブジェクト</param>
    ''' <param name="AllowEmpty">空の文字列を許すかを設定します。Trueは許可 Falseは不許可 </param>
    ''' <returns>空文字、または変換可能なときはTrue 変換できないときはFalseを返します。</returns>
    ''' <remarks></remarks>
    Public Shared Function ValidateDateTextBox(ByVal TargetObject As TextBox, Optional ByVal AllowEmpty As Boolean = True) As Boolean

        If AllowEmpty And TargetObject.Text.Trim = String.Empty Then
            Return True
        End If

        If Not Regex.IsMatch(TargetObject.Text, DATE_REGEX_OPTIONAL) Then
            Return False
        End If

        If Not IsDate(TargetObject.Text.Trim) Then
            Return False
        End If
        Return True
    End Function

    ''' <summary>
    ''' actionパラメータを取得します。
    ''' </summary>
    ''' <param name="Request">呼出し元のページのHttpRequestオブジェクト</param>
    ''' <returns>取得したactionパラメータを返します。見つからない場合は空白を返します。</returns>
    ''' <remarks></remarks>
    Public Shared Function GetHttpAction(ByVal Request As HttpRequest) As String

        Return GetHttpQuery(Request, "Action")

    End Function

    ''' <summary>
    ''' リクエストに含まれるクエリーパラメータの内容を取得します。
    ''' </summary>
    ''' <param name="Request">呼出し元のページのHttpRequestオブジェクト</param>
    ''' <param name="Key">クエリーキー文字列</param>
    ''' <returns>取得したパラメータを返します。見つからない場合は空白を返します。</returns>
    ''' <remarks></remarks>
    Public Shared Function GetHttpQuery(ByVal Request As HttpRequest, ByVal Key As String) As String

        Dim st_Query As String = String.Empty

        If String.IsNullOrEmpty(Request.Form(Key)) Then
            st_Query = Request.QueryString(Key)
        Else
            st_Query = Request.Form(Key).ToString
        End If

        Return st_Query

    End Function

    ''' <summary>
    ''' 新製品登録番号のフォーマットチェック
    ''' </summary>
    ''' <param name="st_NewProductNumber">新製品登録番号</param>
    ''' <returns>True = 新製品登録番号のフォーマットに一致する, False = 一致しない</returns>
    ''' <remarks></remarks>
    Public Shared Function IsNewProductNumber(ByVal st_NewProductNumber As String) As Boolean

        '正規表現にて新製品登録番号のフォーマットと比較する。(新製品登録番号はアルファベット1～2桁+数字9桁) 
        If Regex.IsMatch(st_NewProductNumber, "^[A-Z]{1,2}[0-9]{9}$") Then
            Return True
        End If
        Return False

    End Function

    ''' <summary>
    ''' サプライヤ情報取得
    ''' </summary>
    ''' <param name="st_SupplierCode">サプライヤコード</param>
    ''' <returns>取得した Info を返却します。取得できなかった場合、String.Empty を返却します。</returns>
    ''' <remarks></remarks>
    Public Shared Function GetSupplierInfo(ByVal st_SupplierCode As String) As String

        Dim DBConn As New System.Data.SqlClient.SqlConnection(DB_CONNECT_STRING)
        Dim st_SupplierInfo As String = String.Empty
        Dim st_Return As String = String.Empty
        Dim DS As DataSet = New DataSet
        Dim i_SupplierCode As Integer = 0

        Dim b_ParseToInt As Boolean = Int32.TryParse(st_SupplierCode, i_SupplierCode)

        ' リンク押下時に設定されている Supplier または Maker の Info を取得し、表示する
        If Not String.IsNullOrEmpty(st_SupplierCode) AndAlso b_ParseToInt Then
            'Info 取得
            Using DBCommand As New SqlCommand("SELECT Info FROM Supplier WHERE SupplierCode = @SupplierCode", DBConn)
                DBCommand.Parameters.Add("SupplierCode", SqlDbType.Int).Value = i_SupplierCode
                Using DBAdapter = New SqlDataAdapter
                    DBAdapter.SelectCommand = DBCommand
                    DBAdapter.Fill(DS, "SupplierInfo")
                    If DS.Tables("SupplierInfo").Rows.Count > 0 Then
                        st_SupplierInfo = DS.Tables("SupplierInfo").Rows(0)("Info").ToString
                    End If
                    DS.Dispose()
                End Using
            End Using
        End If

        DBConn.Close()

        Return st_SupplierInfo

    End Function
    ''' <summary>
    ''' プライオリティドロップダウンリスト設定
    ''' </summary>
    ''' <param name="Combo">ドロップダウンリスト</param>
    ''' <param name="Type">利用タイプ</param>
    ''' <remarks></remarks>
    Public Shared Sub SetPriorityDropDownList(ByVal Combo As System.Web.UI.WebControls.ListControl, ByVal Type As String)

        If (Type = PRIORITY_FOR_SEARCH) Then
            Combo.Items.Add(PRIORITY_A)
            Combo.Items.Add(PRIORITY_B)
            Combo.Items.Add(PRIORITY_AB)
            Combo.Items.Add(PRIORITY_ALL)
        ElseIf (Type = PRIORITY_FOR_RFQ_SEARCH) Then
            Combo.Items.Add(New ListItem())
            Combo.Items.Add(PRIORITY_A)
            Combo.Items.Add(PRIORITY_B)
            Combo.Items.Add(PRIORITY_AB)
        Else
            Combo.Items.Add(PRIORITY_A)
            Combo.Items.Add(PRIORITY_B)
            Combo.Items.Insert(0, New ListItem())
        End If
    End Sub

    ''' <summary>
    ''' RFQOrderByドロップダウンリスト設定
    ''' </summary>
    ''' <param name="Combo">ドロップダウンリスト</param>
    ''' <remarks></remarks>
    Public Shared Sub SetRFQOrderByDropDownList(ByVal Combo As System.Web.UI.WebControls.ListControl)
        Combo.Items.Add(New ListItem("RFQ Reminder", "REM"))
        Combo.Items.Add(New ListItem("Last Status Change Date ASC", "ASC"))
        Combo.Items.Add(New ListItem("Last Status Change Date DESC", "DESC"))
        Combo.SelectedValue = "REM"
        Combo.DataBind()
    End Sub

    ''' <summary>
    ''' プライオリティ取得
    ''' </summary>
    ''' <param name="st_ParPONumber">親 PONumber</param>
    ''' <return>Priority</return>
    ''' <remarks></remarks>
    Public Shared Function GetParPOPriority(ByVal st_ParPONumber As String) As String

        If String.IsNullOrEmpty(st_ParPONumber) Then
            Return String.Empty
        End If

        Dim sqlConn As SqlConnection = Nothing

        Dim sb_Sql As StringBuilder = New StringBuilder

        sb_Sql.Append("SELECT ")
        sb_Sql.Append(" Priority ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append(" v_PO ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append(" PONumber = @PONumber ")

        Try
            sqlConn = New SqlConnection(DB_CONNECT_STRING)

            Dim sqlCmd As New SqlCommand(sb_Sql.ToString(), sqlConn)
            sqlCmd.Parameters.AddWithValue("PONumber", st_ParPONumber)
            sqlConn.Open()

            Dim obj_Return As Object = sqlCmd.ExecuteScalar()

            If obj_Return Is Nothing Then
                Return String.Empty
            End If

            Return obj_Return.ToString()

        Finally

            If Not (sqlConn Is Nothing) Then
                sqlConn.Close()
                sqlConn.Dispose()
            End If

        End Try

    End Function

    ''' <summary>
    ''' RFQStatusドロップダウンリスト設定
    ''' 追加する選択肢（""空文字列の場合先頭、"ALL"の場合最後に追加)
    ''' </summary>
    ''' <param name="Combo">ドロップダウンリスト</param>
    ''' <remarks></remarks>
    Public Shared Sub SetRFQStatusDropDownList(ByVal Combo As System.Web.UI.WebControls.ListControl,ByVal Optional st_AddRow As String = RFQSTATUS_ALL)
        Combo.Items.Clear()
        Dim list_RFQStatusList As TCIDataAccess.RFQStatusList = New TCIDataAccess.RFQStatusList
        list_RFQStatusList.Load()
        
        ' ALLでない場合先頭に第２引数の値をセット
        IF st_AddRow <> RFQSTATUS_ALL Then
            Combo.Items.Add(New ListItem(st_AddRow, String.Empty))
        End If

        For Each RFQStatus As TCIDataAccess.RFQStatus In list_RFQStatusList
            Combo.Items.Add(New ListItem(RFQStatus.Text, RFQStatus.RFQStatusCode))
        Next

        ' ALLの場合最後に「ALL」をセット
        IF st_AddRow = RFQSTATUS_ALL Then
            Combo.Items.Add(New ListItem(Common.RFQSTATUS_ALL, Common.RFQSTATUS_ALL))
        End If
    End Sub

    ''' <summary>
    ''' Locationドロップダウンリスト設定
    ''' </summary>
    ''' <param name="Combo">ドロップダウンリスト</param>
    ''' <remarks></remarks>
    Public Shared Sub SetLocationDropDownList(ByVal Combo As System.Web.UI.WebControls.ListControl)
        Combo.Items.Clear()
        Dim list_LocationList As New TCIDataAccess.s_LocationList
        list_LocationList.Load()

        ' リストの先頭に空文字をセット
        Combo.Items.Add(New ListItem(String.Empty, String.Empty))

        For Each s_Location As s_Location In list_LocationList
            Combo.Items.Add(New ListItem(s_Location.Name, s_Location.LocationCode))
        Next
    End Sub

    ''' <summary>
    ''' Storageドロップダウンリスト設定
    ''' </summary>
    ''' <param name="Combo">ドロップダウンリスト</param>
    ''' <param name="UserID">UserID</param>
    ''' <remarks></remarks>
    Public Shared Sub SetStorageDropDownList(ByVal Combo As System.Web.UI.WebControls.ListControl, ByVal UserID As Integer)
        Combo.Items.Clear()
        Dim storageList As New TCIDataAccess.StorageByPurchasingUserList
        storageList.Load(UserID)

        Combo.Items.Add(New ListItem(String.Empty, String.Empty))

        For Each StorageByPurchasingUser As StorageByPurchasingUser In storageList
            Combo.Items.Add(New ListItem(StorageByPurchasingUser.Storage, StorageByPurchasingUser.Storage))
        Next
    End Sub

    ''' <summary>
    ''' 指定された製品が極秘品か否かを判定する
    ''' </summary>
    ''' <param name="key">ProductID または ProductNumber</param>
    ''' <return>True: 極秘対象, False: 極秘対象外</return>
    ''' <remarks></remarks>
    Public Shared Function IsConfidentialItem(ByVal key As String) As Boolean

        If String.IsNullOrEmpty(key) Then
            Return False
        End If

        Dim productID As Integer = 0
        Dim returnValue As Boolean

        Dim sqlStr As StringBuilder = New StringBuilder
        sqlStr.AppendLine("SELECT")
        sqlStr.AppendLine("  1")
        sqlStr.AppendLine("FROM")
        sqlStr.AppendLine("  v_CONFIDENTIAL")
        sqlStr.AppendLine("WHERE")
        If Integer.TryParse(key, productID) Then '数値に変換できる場合は ProductID と判断
            sqlStr.AppendLine("  ProductID = @Keyword")
        Else
            sqlStr.AppendLine("  ProductNumber = @Keyword")
        End If
        sqlStr.AppendLine("  AND isCONFIDENTIAL = 1")

        Using sqlConn As SqlConnection = New SqlConnection(DB_CONNECT_STRING)
            Using sqlCmd As SqlCommand = New SqlCommand(sqlStr.ToString, sqlConn)
                sqlCmd.Parameters.AddWithValue("Keyword", key)
                sqlConn.Open()

                Using sqlReader As SqlDataReader = sqlCmd.ExecuteReader
                    returnValue = sqlReader.Read
                    sqlReader.Close()
                End Using
            End Using
        End Using

        Return returnValue

    End Function

    Public Shared Function GetRole(ByVal userID As String) As String

        If String.IsNullOrEmpty(userID) Then
            Return String.Empty
        End If

        Dim returnValue As String = String.Empty

        Dim sqlStr As StringBuilder = New StringBuilder
        sqlStr.AppendLine("SELECT")
        sqlStr.AppendLine("  RoleCode")
        sqlStr.AppendLine("FROM")
        sqlStr.AppendLine("  PurchasingUser")
        sqlStr.AppendLine("WHERE")
        sqlStr.AppendLine("  UserID = @UserID")

        Using sqlConn As SqlConnection = New SqlConnection(DB_CONNECT_STRING)
            Using sqlCmd As SqlCommand = New SqlCommand(sqlStr.ToString, sqlConn)
                sqlCmd.Parameters.AddWithValue("UserID", userID)
                sqlConn.Open()

                Using sqlReader As SqlDataReader = sqlCmd.ExecuteReader
                    If sqlReader.Read = True Then
                        returnValue = sqlReader("RoleCode").ToString
                    End If
                    sqlReader.Close()
                End Using
            End Using
        End Using

        Return returnValue

    End Function
    
    ''' <summary>
    ''' PurposeのプルダウンにPurposeCodeを設定する
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub SetPurposeDropDownList(ByVal SrcPurpose As System.Web.UI.WebControls.SqlDataSource)

        SrcPurpose.SelectCommand = "SELECT PurposeCode, Text FROM Purpose where IsVisiable=1  ORDER BY SortOrder"

    End Sub
    
    ''' <summary>
    ''' OrderUnitのプルダウンにUnitCodeを設定する 
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared Sub SetUnitDropDownList(ByVal SrcUnit As System.Web.UI.WebControls.SqlDataSource)

        SrcUnit.SelectCommand = "SELECT UnitCode FROM PurchasingUnit ORDER BY UnitCode"

    End Sub
    ''' <summary>
    ''' CodeExtension のドロップダウンリストに値を設定します。
    ''' </summary>
    ''' <param name="Combo"></param>
    ''' <param name="st_ProductNumber">ProductNumber</param>
    ''' <remarks></remarks>
    Public Shared Sub SetCodeExtensionDropDownList(ByVal Combo As System.Web.UI.WebControls.ListControl, ByVal st_ProductNumber As String)
        Dim lst_DropDownList As List(Of DropDownListItems) = New List(Of DropDownListItems)
        Dim codeExtension As CodeExtension = New CodeExtension
        Dim lst_DropDown As List(Of DropDownListItems) = codeExtension.GetCodeExtensionDropDownList(st_ProductNumber)

        Dim obj_DropDownListBlank As New DropDownListItems
        obj_DropDownListBlank.ItemValue = String.Empty 
        obj_DropDownListBlank.ItemText = String.Empty 
        obj_DropDownListBlank.ItemOrder = 0
        lst_DropDownList.Add(obj_DropDownListBlank)

        If lst_DropDown.Count <> 0 Then 
            For Each dropDownListItems As DropDownListItems In lst_DropDown
                If Not String.IsNullOrWhiteSpace(dropDownListItems.ItemValue) Then 
                    Dim obj_DropDownListItems As New DropDownListItems
                    obj_DropDownListItems.ItemValue = dropDownListItems.ItemValue
                    obj_DropDownListItems.ItemText = dropDownListItems.ItemValue 'CodeExtentionにText FiledはないのでItemValueを設定する
                    obj_DropDownListItems.ItemOrder = dropDownListItems.ItemOrder
                    lst_DropDownList.Add(obj_DropDownListItems)
                End If
            Next
        End If

        If lst_DropDownList.Count <> 0 Then 
            Combo.DataSource = lst_DropDownList
        End If

        Combo.DataTextField = "ItemText"
        Combo.DataValueField = "ItemValue"
        Combo.DataBind

    End Sub
    ''' <summary>
    ''' Supplier Contact Person のドロップダウンリストに値を設定します。
    ''' </summary>
    ''' <remarks></remarks>
    ''' <param name="Combo"></param>
    ''' <param name="st_SupplierCode">SupplierCode</param>
    Public Shared Sub SetSupplierContactPersonCodeList(ByVal Combo As System.Web.UI.WebControls.ListControl, ByVal st_SupplierCode As String)
        Dim lst_DropDownList As List(Of DropDownListItems) = New List(Of DropDownListItems)
        Dim supplier As Supplier = New Supplier
        Dim lst_DropDown As List(Of DropDownListItems) = supplier.GetSupplierContactPersonCodeDropDownList(st_SupplierCode)

            Dim obj_DropDownListBlank As New DropDownListItems
            obj_DropDownListBlank.ItemValue = String.Empty 
            obj_DropDownListBlank.ItemText = String.Empty 
            obj_DropDownListBlank.ItemOrder = 0
            lst_DropDownList.Add(obj_DropDownListBlank)

        If lst_DropDown.Count <> 0 Then 
            For Each dropDownListItems As DropDownListItems In lst_DropDown
                If Not String.IsNullOrWhiteSpace(dropDownListItems.ItemValue) Then 
                    Dim obj_DropDownListItems As New DropDownListItems
                    obj_DropDownListItems.ItemValue = dropDownListItems.ItemValue
                    obj_DropDownListItems.ItemText = dropDownListItems.ItemText
                    obj_DropDownListItems.ItemOrder = dropDownListItems.ItemOrder
                    lst_DropDownList.Add(obj_DropDownListItems)
                End If
            Next
        End If

        If lst_DropDownList.Count <> 0 Then 
            Combo.DataSource = lst_DropDownList
        End If

        Combo.DataTextField = "ItemText"
        Combo.DataValueField = "ItemValue"
        Combo.DataBind

    End Sub

    ''' <summary>
    ''' Territory のドロップダウンリストに値を設定します。
    ''' </summary>
    ''' <param name="Combo">ドロップダウンリスト</param>
    ''' <remarks></remarks>
    Public Shared Sub SetTerritoryDropDownList(ByVal Combo As System.Web.UI.WebControls.ListControl)
        Dim lst_DropDownList As List(Of DropDownListItems) = New List(Of DropDownListItems)
        Dim s_Location As s_LocationList = New s_LocationList
        s_Location.Load 
        Dim lst_DropDown As List(Of DropDownListItems) = New List(Of DropDownListItems)
        Dim i As Integer = 0

        Dim obj_DropDownListBlank As New DropDownListItems
        obj_DropDownListBlank.ItemValue = "Direct" 
        obj_DropDownListBlank.ItemText = Direct 
        obj_DropDownListBlank.ItemOrder = i
        lst_DropDownList.Add(obj_DropDownListBlank)
        i = i + 1

        If s_Location.Count <> 0 Then 
            For Each dropDownListItems As s_Location In s_Location
                Dim obj_DropDownListItems As New DropDownListItems
                obj_DropDownListItems.ItemValue = dropDownListItems.LocationCode
                obj_DropDownListItems.ItemText = dropDownListItems.Name
                obj_DropDownListItems.ItemOrder = i
                lst_DropDownList.Add(obj_DropDownListItems)
                i = i + 1
            Next
        End If

        If lst_DropDownList.Count <> 0 Then 
            Combo.DataSource = lst_DropDownList
        End If

        Combo.DataTextField = "ItemText"
        Combo.DataValueField = "ItemValue"
        Combo.DataBind

    End Sub

    ''' <summary>
    ''' Valid Quotation のドロップダウンリストに値を設定します。
    ''' 追加する選択肢（"ALL"の場合先頭、""空文字列の場合最後に追加)
    ''' </summary>
    ''' <remarks></remarks>
    ''' <param name="Combo">ドロップダウンリスト</param>
    ''' <param name="st_AddRow">追加する選択肢（"ALL"の場合先頭、""空文字列の場合最後に追加)</param>
    Public Shared Sub SetValidQuotationList(ByVal Combo As System.Web.UI.WebControls.ListControl, ByVal Optional st_AddRow As String = ValidQuotation_ALL)
        Dim lst_DropDownList As List(Of DropDownListItems) = New List(Of DropDownListItems)
        Dim order As Integer = 0
        If st_AddRow = String.Empty Then
            lst_DropDownList.Add(New DropDownListItems() With {.ItemOrder = order, .ItemText = st_AddRow, .ItemValue = st_AddRow}) : order += 1
        End If
        lst_DropDownList.Add(New DropDownListItems() With {.ItemOrder = order, .ItemText = "Valid Price", .ItemValue = "Y"}) : order += 1
        lst_DropDownList.Add(New DropDownListItems() With {.ItemOrder = order, .ItemText = "Invalid Price", .ItemValue = "N"}) : order += 1
        If st_AddRow = ValidQuotation_ALL Then
            lst_DropDownList.Add(New DropDownListItems() With {.ItemOrder = order, .ItemText = st_AddRow, .ItemValue = st_AddRow}) : order += 1
        End If

        If lst_DropDownList.Count <> 0 Then 
            Combo.DataSource = lst_DropDownList
        End If

        Combo.DataTextField = "ItemText"
        Combo.DataValueField = "ItemValue"
        Combo.DataBind

    End Sub

    ''' <summary>
    ''' SupplierNameのプルダウンに仕入先情報を設定します。
    ''' </summary>
    ''' <param name="SupplierCode">対象となるSupplierCode</param>
    ''' <param name="LocationCode">対象となるLocationCode</param>
    ''' <remarks></remarks>
    Public Shared Sub SetSupplierDropDownList(ByVal SrcSupplier As System.Web.UI.WebControls.SqlDataSource, _
                                                 ByVal SupplierCode As String, ByVal LocationCode As String, ByVal SessionLocationCode As String)
        Dim sb_Sql As StringBuilder = New StringBuilder

        ' 検索結果の並び順を固定させるために UNION を使用しています
        sb_Sql.Append("SELECT ")
        sb_Sql.Append("  SupplierCode, ")
        sb_Sql.Append("  LTRIM(RTRIM(ISNULL(Name1, '') + ' ' + ISNULL(Name2, ''))) AS Name, ")
        sb_Sql.Append("  1 AS SortOrder ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append("  Supplier ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append("  LocationCode = @LocationCode ")
        sb_Sql.Append("UNION ")
        sb_Sql.Append("SELECT ")
        sb_Sql.Append("  SupplierCode, ")
        sb_Sql.Append("  LTRIM(RTRIM(ISNULL(Name1, '') + ' ' + ISNULL(Name2, ''))) AS Name, ")
        sb_Sql.Append("  2 AS SortOrder ")
        sb_Sql.Append("FROM ")
        sb_Sql.Append("  Supplier ")
        sb_Sql.Append("WHERE ")
        sb_Sql.Append("  SupplierCode = @SupplierCode ")
        sb_Sql.Append("ORDER BY ")
        sb_Sql.Append("  SortOrder ")

        SrcSupplier.SelectCommand = sb_Sql.ToString
        SrcSupplier.SelectParameters.Clear()
        SrcSupplier.SelectParameters.Add("SupplierCode", SupplierCode)

        If (LocationCode = SessionLocationCode) Or (LocationCode = String.Empty) Then
            ' Direct 発注の場合に自拠点をリストアップしないための措置です
            SrcSupplier.SelectParameters.Add("LocationCode", "#%@$\")
        Else
            SrcSupplier.SelectParameters.Add("LocationCode", LocationCode)
        End If


    End Sub

    '生成txt文件
    Public Shared Function CreateTxtFile(ByVal fileName As String) As String
        Dim strFileName As String
        strFileName = System.IO.Path.Combine(ConfigurationManager.AppSettings("PoInterfaceDir"), fileName)
        Dim FileExists As Boolean
        FileExists = My.Computer.FileSystem.DirectoryExists(ConfigurationManager.AppSettings("PoInterfaceDir"))
        If FileExists = False Then
            My.Computer.FileSystem.CreateDirectory(ConfigurationManager.AppSettings("PoInterfaceDir"))
        End If
        Dim t As System.IO.StreamWriter = New System.IO.StreamWriter(strFileName, False, System.Text.Encoding.UTF8)
        t.Write("")
        t.Close()
        Return strFileName
    End Function

    Public Shared Sub WriteLintTxtFile(ByVal fileName As String, ByVal content As String)
        Dim FileExists As Boolean
        FileExists = My.Computer.FileSystem.DirectoryExists(ConfigurationManager.AppSettings("PoInterfaceDir"))
        If FileExists = False Then
            My.Computer.FileSystem.CreateDirectory(ConfigurationManager.AppSettings("PoInterfaceDir"))
        End If
        Dim t As System.IO.StreamWriter = New System.IO.StreamWriter(fileName, True, System.Text.Encoding.UTF8)
        t.WriteLine(content)
        t.Close()
    End Sub

    Public Shared Function GetDataTable(ByVal sql As String, ByVal tablename As String) As System.Data.DataTable
        Dim DBConn As New System.Data.SqlClient.SqlConnection(DB_CONNECT_STRING)
        Dim DBAdapter As System.Data.SqlClient.SqlDataAdapter
        Dim DBCommand As System.Data.SqlClient.SqlCommand
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()
        DBAdapter = New SqlDataAdapter
        Dim DS As DataSet = New DataSet
        DBCommand = New SqlCommand(sql, DBConn)
        DBAdapter.SelectCommand = DBCommand
        DBAdapter.Fill(DS, tablename)
        DBCommand.Dispose()
        DBConn.Close()
        Return DS.Tables(tablename)
    End Function

    Public Shared Function GetDataTable(ByVal sql As String) As System.Data.DataTable
        Dim DBConn As New System.Data.SqlClient.SqlConnection(DB_CONNECT_STRING)
        Dim DBAdapter As System.Data.SqlClient.SqlDataAdapter
        Dim DBCommand As System.Data.SqlClient.SqlCommand
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()
        DBAdapter = New SqlDataAdapter
        Dim DS As DataSet = New DataSet
        DBCommand = New SqlCommand(sql, DBConn)
        DBAdapter.SelectCommand = DBCommand
        DBAdapter.Fill(DS)
        DBCommand.Dispose()
        DBConn.Close()
        Return DS.Tables(0)
    End Function

    Public Shared Function GetDataRow(ByVal sql As String, ByVal tablename As String) As System.Data.DataRow
        Dim DataTable As System.Data.DataTable = GetDataTable(sql, tablename)
        If DataTable.Rows.Count > 0 Then
            Return DataTable.Rows(0)
        Else
            Return Nothing
        End If
    End Function
    
    ''' <summary>
    ''' ログインユーザーのセッション情報を確認する
    ''' </summary>
    ''' <param name="st_RoleCode">SESSION_ROLE_CODE</param>
    ''' <returns>
    ''' </returns>
    ''' <remarks>
    ''' 1.<br/>
    ''' 2.IDの文字列と合致する項目のフラグを活性化し、他のフラグは非活性化する
    ''' </remarks>
    Public Shared Function CheckSessionRole(ByVal st_RoleCode As String) As Boolean
        '権限ロールに従い極秘品を除外する
        If st_RoleCode = ROLE_WRITE_P OrElse st_RoleCode = ROLE_READ_P Then
            Return False
        Else
            Return True
        End If
    End Function
    
    ''' <summary>
    ''' 変換文字列を含むメッセージを組み立てる
    ''' </summary>
    ''' <param name="st_Msg">メッセージ</param>
    ''' <param name="ary_RplaceWords">変換後文字列配列</param>
    ''' <returns>
    ''' </returns>
    ''' <remarks>
    ''' </remarks>
    Public Shared Function CreateMSG(st_Msg As String, ary_RplaceWords As ArrayList) As String
        Dim msg As String = String.Empty 

        For i As Integer = 0 To ary_RplaceWords.Count - 1
            msg = st_Msg.Replace("{" + i.ToString + "}", ary_RplaceWords(i).ToString)
        Next

        Return msg

    End Function

End Class
Public Class DropDownListItems

    Protected _ItemValue As String = String.Empty
    Protected _ItemText As String = String.Empty
    Protected _ItemOrder As Integer = 0

    ''' <summary> 
    ''' ItemValue を設定、または取得する 
    ''' </summary> 
    Public Property ItemValue() As String
        Get
            Return _ItemValue
        End Get
        Set(ByVal value As String)
            _ItemValue = value
        End Set
    End Property

    ''' <summary> 
    ''' ItemText を設定、または取得する 
    ''' </summary> 
    Public Property ItemText() As String
        Get
            Return _ItemText
        End Get
        Set(ByVal value As String)
            _ItemText = value
        End Set
    End Property

    ''' <summary> 
    ''' ItemOrder を設定、または取得する 
    ''' </summary> 
    Public Property ItemOrder() As Integer
        Get
            Return _ItemOrder
        End Get
        Set(ByVal value As Integer)
            _ItemOrder = value
        End Set
    End Property

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    Public Sub New()

    End Sub

End Class


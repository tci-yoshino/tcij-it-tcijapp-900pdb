Imports System.Data.SqlClient

Partial Public Class UserList
    Inherits CommonPage

    Dim DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As SqlCommand
    Dim DBReader As SqlDataReader

    Dim st_ExlName As String = String.Empty             'Excel名格納領域
    Dim st_HeaderLine As String = String.Empty          'Header情報記憶
    Dim st_StartWorksheetLine As String = String.Empty  'Worksheet開始タグ記憶
    Dim st_EndWorksheetLine As String = String.Empty    'Worksheet終了タグ記憶
    Dim st_DataLine As String = String.Empty            'Data情報格納領域
    Dim st_StartRowLine As String = String.Empty        'Row開始タグ記憶
    Dim st_EndRowLine As String = String.Empty          'Row終了タグ記憶
    Dim st_StartTableLine As String = String.Empty      'Table開始タグ記憶
    Dim st_EndTableLine As String = String.Empty        'Table終了タグ記憶
    Dim st_EndWorkBookLine As String = String.Empty     'Book終了タグ記憶

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            SrcUser.SelectCommand = "SELECT UserID,LocationName,AccountName,SurName,GivenName,RoleCode,PrivilegeLevel,isAdmin,isDisabled,'UserSetting.aspx?Action=Edit&UserID=' + Cast(UserID AS varchar) AS URL, " & _
                                    "CASE isDisabled WHEN 1 THEN 'disable' ELSE '' END AS isDisabled_CSS " & _
                                    "FROM v_UserAll ORDER BY LocationName,isDisabled,SurName,GivenName"
        End If
    End Sub

    Protected Sub Download_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Download.Click
        '[Action確認]---------------------------------------------------------------------------------------------
        'Dim st_Action As String = CStr(IIf(String.IsNullOrEmpty(Request.QueryString("Action")), "", Request.QueryString("Action")))
        'If st_Action <> ACTION_ALLDATA Then
        '    Msg.Text = ERR_INVALID_PARAMETER
        '    Exit Sub
        'End If

        '[ダウンロード実行]---------------------------------------------------------------------------------------
        LoadExcelForm()
        Response.ContentType = Common.EXCEL_CONTENTTYPE
        Response.AddHeader("Content-Disposition", "attachment; filename=" & """PurchasingUser_" & Now.ToString("yyyyMMdd") & ".xls""")
        'Response.Flush()
        Response.Write(st_HeaderLine)
        '名称仕様変更 カタログ品名用⇒All Data
        CreateXML(CreateSQL(), "Purchase DB 全ユーザ一覧")
        Response.Write(st_EndWorkBookLine)
        Response.End()
    End Sub

    Public Function CreateSQL() As String
        '[SELECT文作成]--------------------------------------------------
        Dim st_SQL As String = "SELECT LocationName AS 拠点,Name AS ユーザー名,RoleCode AS 権限,Email AS メールアドレス,CASE WHEN isDisAbled=1 THEN '無効ユーザ' ELSE '' END AS 備考 FROM v_UserAll ORDER BY LocationName, isDisabled DESC, SurName"
        Return st_SQL
    End Function

    Public Sub LoadExcelForm()
        '[App_DataからExcelFormat読込み]--------------------------------------------------------------------------
        Dim st_ExcelBase As String
        Using sr As New System.IO.StreamReader(MapPath("./App_Data/ExcelForm.xml"), System.Text.Encoding.UTF8)
            st_ExcelBase = sr.ReadToEnd()
        End Using
        st_HeaderLine = Regex.Match(st_ExcelBase, "<\?xml.*/ExcelWorkbook>", RegexOptions.Singleline).Value
        st_StartWorksheetLine = Regex.Match(st_ExcelBase, "<Worksheet.*?>").Value
        st_StartTableLine = Regex.Match(st_ExcelBase, "<Table.*?>").Value
        st_StartRowLine = Regex.Match(st_ExcelBase, "<Row.*?>").Value
        st_DataLine = Regex.Match(st_ExcelBase, "<Cell.*?</Cell>", RegexOptions.Singleline).Value
        st_EndRowLine = Regex.Match(st_ExcelBase, "</Row.*?>").Value
        st_EndTableLine = Regex.Match(st_ExcelBase, "</Table>").Value
        st_EndWorksheetLine = Regex.Match(st_ExcelBase, "</Worksheet>").Value
        st_EndWorkBookLine = Regex.Match(st_ExcelBase, "</Workbook>").Value
    End Sub

    Private Sub CreateXML(ByVal sql As String, ByVal ShtName As String)
        '[XML スプレッドシートの作成]-----------------------------------------------------------------------------
        '[SheetNameの作成]
        Response.Write(st_StartWorksheetLine.Replace("@SheetName", ShtName))

        Response.Write(st_StartTableLine)
        DBCommand = DBConn.CreateCommand()
        DBCommand.CommandText = sql
        DBConn.Open()
        DBReader = DBCommand.ExecuteReader
        DBCommand.Dispose()
        '[見出行の作成]
        Response.Write(st_StartRowLine & vbCrLf)
        For i As Integer = 0 To DBReader.FieldCount - 1
            Response.Write(st_DataLine.Replace("@DataValue", DBReader.GetName(i)) & vbCrLf)
        Next
        Response.Write(st_EndRowLine & vbCrLf)
        '[データ行の作成]
        Do Until DBReader.Read = False
            Response.Write(st_StartRowLine & vbCrLf)
            For i As Integer = 0 To DBReader.FieldCount - 1
                Dim st_TempData As String = DBReader(i).ToString.Replace("<", "&lt;")
                st_TempData = st_TempData.Replace(">", "&gt;")
                Response.Write(st_DataLine.Replace("@DataValue", st_TempData) & vbCrLf)
            Next
            Response.Write(st_EndRowLine & vbCrLf)
        Loop
        DBReader.Close()
        DBConn.Close()
        Response.Write(st_EndTableLine)
        Response.Write(st_EndWorksheetLine)
    End Sub
End Class
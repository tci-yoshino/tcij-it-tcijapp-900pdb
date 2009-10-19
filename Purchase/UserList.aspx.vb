Option Explicit On
Option Infer Off
Option Strict On

Imports System.Data.SqlClient

Partial Public Class UserList
    Inherits CommonPage

    Private Structure ExcelLineType
        ''' <summary>Excel名</summary>
        Public ExlName As String
        ''' <summary>Header情報</summary>
        Public HeaderLine As String
        ''' <summary>Worksheet開始タグ</summary>
        Public StartWorksheetLine As String
        ''' <summary>Worksheet終了タグ</summary>
        Public EndWorksheetLine As String
        ''' <summary>Data情報</summary>
        Public DataLine As String
        ''' <summary>Row開始タグ</summary>
        Public StartRowLine As String
        ''' <summary>Row終了タグ</summary>
        Public EndRowLine As String
        ''' <summary>Table開始タグ</summary>
        Public StartTableLine As String
        ''' <summary>Table終了タグ</summary>
        Public EndTableLine As String
        ''' <summary>'Book終了タグ</summary>
        Public EndWorkBookLine As String
    End Structure

    Dim str_ExcelLine As ExcelLineType
    Const DOWNLOAD_ACTION As String = "Download"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Dim st_SQL As String = String.Empty
            st_SQL &= "SELECT "
            st_SQL &= " UserID, "
            st_SQL &= " LocationName, "
            st_SQL &= " AccountName, "
            st_SQL &= " SurName, "
            st_SQL &= " GivenName, "
            st_SQL &= " RoleCode, "
            st_SQL &= " PrivilegeLevel, "
            st_SQL &= " isAdmin, "
            st_SQL &= " isDisabled, "
            st_SQL &= " 'UserSetting.aspx?Action=Edit&UserID=' + Cast(UserID AS varchar) AS URL "
            'st_SQL &= " CASE isDisabled WHEN 1 THEN 'disable' ELSE '' END AS isDisabled_CSS "
            st_SQL &= "FROM "
            st_SQL &= " v_UserAll "
            st_SQL &= "ORDER BY "
            st_SQL &= " LocationName, "
            st_SQL &= " isDisabled, "
            st_SQL &= " SurName, "
            st_SQL &= " GivenName"
            SrcUser.SelectCommand = st_SQL
        End If
    End Sub

    Protected Sub Download_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Download.Click
        '[Action確認]-------------------------------------------------------------------------------------
        If Common.GetHttpAction(Request) = DOWNLOAD_ACTION Then
            '[ダウンロード実行]---------------------------------------------------------------------------
            LoadExcelForm()
            Response.ContentType = Common.EXCEL_CONTENTTYPE
            Response.AddHeader("Content-Disposition", "attachment; filename=" & """PurchasingUser_" & Now.ToString("yyyyMMdd") & ".xls""")
            Response.Write(str_ExcelLine.HeaderLine)
            CreateXML(CreateSQL(), "Purchase DB 全ユーザ一覧")
            Response.Write(str_ExcelLine.EndWorkBookLine)
            Response.End()
        End If
    End Sub

    Private Sub LoadExcelForm()
        '[App_DataからExcelFormat読込み]------------------------------------------------------------------
        Dim st_ExcelBase As String
        Using sr As New System.IO.StreamReader(MapPath("./App_Data/ExcelForm.xml"), System.Text.Encoding.UTF8)
            st_ExcelBase = sr.ReadToEnd()
        End Using
        str_ExcelLine.HeaderLine = Regex.Match(st_ExcelBase, "<\?xml.*/ExcelWorkbook>", RegexOptions.Singleline).Value
        str_ExcelLine.StartWorksheetLine = Regex.Match(st_ExcelBase, "<Worksheet.*?>").Value
        str_ExcelLine.StartTableLine = Regex.Match(st_ExcelBase, "<Table.*?>").Value
        str_ExcelLine.StartRowLine = Regex.Match(st_ExcelBase, "<Row.*?>").Value
        str_ExcelLine.DataLine = Regex.Match(st_ExcelBase, "<Cell.*?</Cell>", RegexOptions.Singleline).Value
        str_ExcelLine.EndRowLine = Regex.Match(st_ExcelBase, "</Row.*?>").Value
        str_ExcelLine.EndTableLine = Regex.Match(st_ExcelBase, "</Table>").Value
        str_ExcelLine.EndWorksheetLine = Regex.Match(st_ExcelBase, "</Worksheet>").Value
        str_ExcelLine.EndWorkBookLine = Regex.Match(st_ExcelBase, "</Workbook>").Value
    End Sub

    Private Function CreateSQL() As String
        '[Excel出力するデータ取得SQL]---------------------------------------------------------------------
        Dim st_SQL As String = String.Empty
        st_SQL &= "SELECT "
        st_SQL &= " LocationName AS 拠点, "
        st_SQL &= " Name AS ユーザー名, "
        st_SQL &= " RoleCode AS 権限, "
        st_SQL &= " Email AS メールアドレス, "
        st_SQL &= " CASE WHEN isDisAbled=1 THEN '無効ユーザ' ELSE '' END AS 備考 "
        st_SQL &= "FROM "
        st_SQL &= " v_UserAll "
        st_SQL &= "ORDER BY "
        st_SQL &= " LocationName, "
        st_SQL &= " isDisabled, "
        st_SQL &= " SurName, "
        st_SQL &= " GivenName"
        Return st_SQL
    End Function

    Private Sub CreateXML(ByVal sql As String, ByVal ShtName As String)
        '[XML スプレッドシートの作成]---------------------------------------------------------------------
        Response.Write(str_ExcelLine.StartWorksheetLine.Replace("@SheetName", ShtName))   '[SheetNameの作成]
        Response.Write(str_ExcelLine.StartTableLine)

        Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
            Dim DBCommand As SqlCommand = DBConn.CreateCommand()
            DBCommand.CommandText = sql
            DBConn.Open()
            Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
            Dim i_FieldCount As Integer = DBReader.FieldCount - 1

            '[見出行の作成]--------------------------------------------------------------------------------
            Response.Write(str_ExcelLine.StartRowLine & vbCrLf)
            For i As Integer = 0 To i_FieldCount
                Response.Write(str_ExcelLine.DataLine.Replace("@DataValue", DBReader.GetName(i)) & vbCrLf)
            Next
            Response.Write(str_ExcelLine.EndRowLine & vbCrLf)

            '[データ行の作成]------------------------------------------------------------------------------
            Do Until DBReader.Read = False
                Response.Write(str_ExcelLine.StartRowLine & vbCrLf)
                For i As Integer = 0 To i_FieldCount
                    Dim st_TempData As String = DBReader(i).ToString.Replace("<", "&lt;")
                    st_TempData = st_TempData.Replace(">", "&gt;")
                    Response.Write(str_ExcelLine.DataLine.Replace("@DataValue", st_TempData) & vbCrLf)
                Next
                Response.Write(str_ExcelLine.EndRowLine & vbCrLf)
            Loop
            DBReader.Close()
        End Using

        Response.Write(str_ExcelLine.EndTableLine)
        Response.Write(str_ExcelLine.EndWorksheetLine)
    End Sub
End Class
Imports System
Imports System.Data
Imports System.Reflection
Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet
Imports Ap = DocumentFormat.OpenXml.ExtendedProperties
Imports Vt = DocumentFormat.OpenXml.VariantTypes
Imports X14 = DocumentFormat.OpenXml.Office2010.Excel

''' <summary>
''' 製品化進捗レポート (Overview of items in process)
''' </summary>
''' <remarks></remarks>
Public Class Report_RFQSearch
    Inherits ExcelReportBase

    ''' <summary> HttpResponse </summary>
    Private _Response As System.Web.HttpResponse

    ''' <summary> EXCELシート名：Sheet1.xlsx </summary>
    Const EXCEL_SHEET As String = "Sheet1"

    ''' <summary> 一覧部分 出力セル情報 </summary>
    Const TABLE_START_COL As Integer = 1
    Const TABLE_START_ROW As Integer = 1

    '''' <summary> テーブルタイプ </summary>
    'Public Enum TableType
    '    PROPOSAL_DEPT = 1               '提案元ごとの明細と合計行
    '    ORDERED_FROM                    '指令・発注先ごとの明細と合計行
    '    AVERAGE_DAYS                    '平均日数
    'End Enum

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="HttpResponse">Response</param>
    Public Sub New(ByVal HttpResponse As System.Web.HttpResponse)
        _Response = HttpResponse
    End Sub

    ''' <summary>
    ''' Excel ダウンロード
    ''' </summary>
    ''' <param name="Cond">検索条件</param>
    ''' <remarks></remarks>
    Public Sub DownloadExcel(ByVal cond As TCIDataAccess.join.KeywordSearchConditionParameter)

        Dim outmemory As New System.IO.MemoryStream

        'テンプレートをコピー
        Dim bytes() As Byte = System.IO.File.ReadAllBytes(Common.REPORT_TEMPLATE_RFQSEARCH)
        outmemory.Write(bytes, 0, bytes.Length)

        'ファイルを作成したり保存したりするクラス
        Using document As SpreadsheetDocument = SpreadsheetDocument.Open(outmemory, True)

            'Zip に格納される workbook.xml をラップするクラス
            Dim wbpart As WorkbookPart = document.WorkbookPart

            'スタイル作成
            GenerateWorkbookStylesPart(wbpart.WorkbookStylesPart)

            '一覧出力 ----------------------------

            'SheetData オブジェクトの取得、開始行列設定
            Dim sheet As Sheet = wbpart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = EXCEL_SHEET).FirstOrDefault()
            Dim wspart As WorksheetPart = CType(wbpart.GetPartById(sheet.Id), WorksheetPart)
            Dim sheetdata As SheetData = wspart.Worksheet.Descendants(Of SheetData)().FirstOrDefault()
            Dim i_Row As Integer = TABLE_START_ROW    '注意: 行番号は常に持ち回る
            i_Row = AppendRow(sheetdata, 1, i_Row, TypeCode.String, "RFQ Search", CellStyle._14PT_BOLD_NONE_NONE, True, 17.25)

            'i_Row = AppendRow(sheetdata, 1, i_Row, TypeCode.String, "Proposal Dept.", CellStyle._9PT_BOLD_NONE_NONE, True, 17.25)
            Dim dc_RFQSearch As New TCIDataAccess.Join.ReportOverviewRFQSearchList
            dc_RFQSearch.Load(cond)
            i_Row = CreateRFQSearchExport(sheetdata, dc_RFQSearch, i_Row)
            i_Row = AppendRow(sheetdata, i_Row)

        End Using

        'Excel出力
        _Response.Clear()
        _Response.ContentType = EXCEL_CONTENTTYPE
        _Response.AppendHeader(EXCEL_APPEND_HEADER_NAME, String.Format(EXCEL_APPEND_HEADER_VALUE, "RFQSearch.xlsx"))
        _Response.BinaryWrite(outmemory.ToArray)
        _Response.End()

    End Sub

    ''' <summary>
    ''' Excel 出力処理
    ''' </summary>
    ''' <param name="sheetdata">SheetData オブジェクト</param>
    ''' <param name="dc_RFQSearchList">ReportOverviewList</param>
    ''' <param name="i_Row">出力行番号</param>
    ''' <returns>出力を終えた後の行番号</returns>
    Public Function CreateRFQSearchExport(ByVal sheetdata As SheetData,
                                                   ByVal dc_RFQSearchList As TCIDataAccess.Join.ReportOverviewRFQSearchList,
                                                   ByVal i_Row As Integer) As Integer

        'ProposalDeptCode のリスト作成
        Dim ProposalDeptList As New List(Of String)

        '出力用テーブル作成
        Dim dt_Table As New DataTable
        CreateTableData(dt_Table)

        Dim i_Col As Integer = TABLE_START_COL

        'ヘッダー部出力 ----------------------
        i_Row = AddHeaderRow(sheetdata, dt_Table, TABLE_START_COL, i_Row, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE, CellStyle._9PT_NONE_NONE_LIGHT_GREEN)

        'データ部出力 ------------------------
        Dim i_TotalByRow As Integer = 0
        For Each dc_RFQSearch As TCIDataAccess.Join.ReportOverviewRFQSearch In dc_RFQSearchList

            Dim newRow As New Row
            i_Col = TABLE_START_COL

            For Each dt_Row In dt_Table.Rows

                If dt_Row("PropertyNumber") = "RFQ Reference Number" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.RFQNumber, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Priority" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.Priority, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Current Status" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.Status, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Last Status Change Date" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, Common.GetLocalTime(dc_RFQSearchList.s_LocationCode,dc_RFQSearch.StatusChangeDate, True, False), CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Product Number" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.ProductNumber, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "CAS Number" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.CASNumber, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Product Name" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.ProductName, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Supplier Code" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.SupplierCode, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "SAP Supplier Code" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.S4SupplierCode, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Supplier Name" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.SupplierName, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Supplier Country" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.SupplierCountryName, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Purpose" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.Purpose, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Maker Code" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.MakerCode, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "SAP Maker Code" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.S4MakerCode, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Maker Name" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.MakerName, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Maker Country" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.MakerCountryName, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Supplier Item Name" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.SupplierItemName, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Handling Fee / Shipment Cost" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.ShippingHandlingCurrencyCode & Space(1) & dc_RFQSearch.ShippingHandlingFee, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Enq-User" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.EnqUserName, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Enq-Location" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.EnqLocationName, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Enq-Storage Location" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.EnqStorageLocation, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Quo-User" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.QuoUserName, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Quo-Location" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.QuoLocationName, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Quo-Storage Location" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.QuoStorageLocation, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Comment" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.Comment, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                'RFQLine
                ElseIf dt_Row("PropertyNumber") = "Line No." Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.LineNo, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Enq-Quantity" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.EnqQuantity & Space(1) & dc_RFQSearch.EnqUnitCode & Space(1) & dc_RFQSearch.EnqPiece, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Currency" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.CurrencyCode, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Price" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.UnitPrice, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Quo-Quantity" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.QuoPer & space(1) & dc_RFQSearch.QuoUnitCode, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Lead Time" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.LeadTime, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Packing" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.Packing, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Purity / Method" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.Purity, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Supplier Offer No." Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.SupplierOfferNo, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Supplier Item No." Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.SupplierItemNumber, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Reason for ""No Offer""" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.NoOfferReason, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "PO" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, dc_RFQSearch.PO, CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                
                ElseIf dt_Row("PropertyNumber") = "Created Date" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, Common.GetLocalTime(dc_RFQSearchList.s_LocationCode,dc_RFQSearch.StatusChangeDateN, True, False), CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Assigned Date" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, Common.GetLocalTime(dc_RFQSearchList.s_LocationCode,dc_RFQSearch.StatusChangeDateA, True, False), CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Enquired Date" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, Common.GetLocalTime(dc_RFQSearchList.s_LocationCode,dc_RFQSearch.StatusChangeDateE, True, False), CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Partly-Quoted Date" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, Common.GetLocalTime(dc_RFQSearchList.s_LocationCode,dc_RFQSearch.StatusChangeDatePQ, True, False), CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Quoted Date" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, Common.GetLocalTime(dc_RFQSearchList.s_LocationCode,dc_RFQSearch.StatusChangeDateQ, True, False), CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Interface Date" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, Common.GetLocalTime(dc_RFQSearchList.s_LocationCode,dc_RFQSearch.StatusChangeDateII, True, False), CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                ElseIf dt_Row("PropertyNumber") = "Closed Date" Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_Col, i_Row, TypeCode.String, Common.GetLocalTime(dc_RFQSearchList.s_LocationCode,dc_RFQSearch.StatusChangeDateV, True, False), CellStyle._9PT_NONE_NONE_LIGHT_TURQUOISE))
                End If

                i_Col += 1
            Next
            i_Row = AppendRow(sheetdata, newRow, i_Row)
        Next

        Return i_Row

    End Function


    ''' <summary>
    ''' 出力用テーブル作成
    ''' </summary>
    Private Sub CreateTableData(ByRef dt As DataTable)

        dt.Columns.Add("Title")
        dt.Columns.Add("PropertyNumber")
        dt.Columns.Add("PropertyType", System.Type.GetType("System.Int32"))
        dt.Columns.Add("TotalByProperty", System.Type.GetType("System.Int32"))

        'v_RFQHeader検索項目
        dt.Rows.Add("", "RFQ Reference Number", TypeCode.Int32, 0)
        dt.Rows.Add("", "Priority", TypeCode.Int32, 0)
        dt.Rows.Add("", "Current Status", TypeCode.Int32, 0)
        dt.Rows.Add("", "Last Status Change Date", TypeCode.Int32, 0)
        dt.Rows.Add("", "Product Number", TypeCode.Int32, 0)
        dt.Rows.Add("", "CAS Number", TypeCode.Int32, 0)
        dt.Rows.Add("", "Product Name", TypeCode.Int32, 0)
        dt.Rows.Add("", "Supplier Code", TypeCode.Int32, 0)
        dt.Rows.Add("", "SAP Supplier Code", TypeCode.Int32, 0)
        dt.Rows.Add("", "Supplier Name", TypeCode.Int32, 0)
        dt.Rows.Add("", "Supplier Country", TypeCode.Int32, 0)
        dt.Rows.Add("", "Purpose", TypeCode.Int32, 0)
        dt.Rows.Add("", "Maker Code", TypeCode.Int32, 0)
        dt.Rows.Add("", "SAP Maker Code", TypeCode.Int32, 0)
        dt.Rows.Add("", "Maker Name", TypeCode.Int32, 0)
        dt.Rows.Add("", "Maker Country", TypeCode.Int32, 0)
        dt.Rows.Add("", "Supplier Item Name", TypeCode.Int32, 0)
        dt.Rows.Add("", "Handling Fee / Shipment Cost", TypeCode.Int32, 0)
        dt.Rows.Add("", "Enq-User", TypeCode.Int32, 0)
        dt.Rows.Add("", "Enq-Location", TypeCode.Int32, 0)
        dt.Rows.Add("", "Enq-Storage Location", TypeCode.Int32, 0)
        dt.Rows.Add("", "Quo-User", TypeCode.Int32, 0)
        dt.Rows.Add("", "Quo-Location", TypeCode.Int32, 0)
        dt.Rows.Add("", "Quo-Storage Location", TypeCode.Int32, 0)
        dt.Rows.Add("", "Comment", TypeCode.Int32, 0)

        'v_RFQLine検索項目
        dt.Rows.Add("", "Line No.", TypeCode.Int32, 0)
        dt.Rows.Add("", "Enq-Quantity", TypeCode.Int32, 0)
        dt.Rows.Add("", "Currency", TypeCode.Int32, 0)
        dt.Rows.Add("", "Price", TypeCode.Int32, 0)
        dt.Rows.Add("", "Quo-Quantity", TypeCode.Int32, 0)
        dt.Rows.Add("", "Lead Time", TypeCode.Int32, 0)
        dt.Rows.Add("", "Packing", TypeCode.Int32, 0)
        dt.Rows.Add("", "Purity / Method", TypeCode.Int32, 0)
        dt.Rows.Add("", "Supplier Offer No.", TypeCode.Int32, 0)
        dt.Rows.Add("", "Supplier Item No.", TypeCode.Int32, 0)
        dt.Rows.Add("", "Reason for ""No Offer""", TypeCode.Int32, 0)
        dt.Rows.Add("", "PO", TypeCode.Int32, 0)
        dt.Rows.Add("", "Created Date", TypeCode.Int32, 0)
        dt.Rows.Add("", "Assigned Date", TypeCode.Int32, 0)
        dt.Rows.Add("", "Enquired Date", TypeCode.Int32, 0)
        dt.Rows.Add("", "Partly-Quoted Date", TypeCode.Int32, 0)
        dt.Rows.Add("", "Quoted Date", TypeCode.Int32, 0)
        dt.Rows.Add("", "Interface Date", TypeCode.Int32, 0)
        dt.Rows.Add("", "Closed Date", TypeCode.Int32, 0)

    End Sub

End Class

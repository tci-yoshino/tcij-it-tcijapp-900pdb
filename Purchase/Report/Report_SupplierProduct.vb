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
Public Class Report_SupplierProduct
    Inherits ExcelReportBase

    ''' <summary> HttpResponse </summary>
    Private _Response As System.Web.HttpResponse

    ''' <summary> EXCELシート名：Sheet1.xlsx </summary>
    Const EXCEL_SHEET As String = "Sheet1"

    ''' <summary> 一覧部分 出力セル情報 </summary>
    Const TABLE_START_COL As Integer = 1
    Const DATA_START_COL As Integer = 1
    Const TABLE_START_ROW As Integer = 1

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
    ''' <remarks></remarks>
    Public Sub DownloadExcel(ByVal st_SupplierCode As String, ByVal st_SupplierName As String, ByVal st_Territory As String, _
                             ByVal st_RoleCode As String, ByVal st_ValidFilter As String, ByVal st_SupplierProductListID As String, _
                             ByVal st_HiddenSortField As String, ByVal st_HiddenSortType As String)

        Dim outmemory As New System.IO.MemoryStream

        'テンプレートをコピー
        '※現時点でローカルを参照している状態なので、Web.Configを参照して決められた階層のテンプレート呼び込みを変える必要あり
        'TODO 要変更：最終的にはサーバー上に配置したExcelのテンプレートファイルを読み込む形式に変換する
        Dim bytes() As Byte = System.IO.File.ReadAllBytes(Common.EXCEL_TEMPLATE_DIRECTORY_SUPPLIERPRODUCT)
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

            ' データ取得
            Dim supplierProduct As TCIDataAccess.Supplier_Product = New TCIDataAccess.Supplier_Product
            Dim productListBySupplierDisp As List(Of TCIDataAccess.Join.ProductListBySupplierDisp) = New List(Of TCIDataAccess.Join.ProductListBySupplierDisp)
            productListBySupplierDisp = supplierProduct.GetProductListBySupplierList(st_SupplierCode, st_RoleCode, st_ValidFilter, _
                                                                                     st_SupplierProductListID, st_HiddenSortField, st_HiddenSortType)
            i_Row = CreateSupplierProductExport(sheetdata, st_SupplierCode, st_SupplierName, st_Territory, productListBySupplierDisp, i_Row)
            i_Row = AppendRow(sheetdata, i_Row)

        End Using

        'Excel出力
        _Response.Clear()
        _Response.ContentType = EXCEL_CONTENTTYPE
        _Response.AppendHeader(EXCEL_APPEND_HEADER_NAME, String.Format(EXCEL_APPEND_HEADER_VALUE, "SupplierProduct.xlsx"))
        _Response.BinaryWrite(outmemory.ToArray)
        _Response.End()

    End Sub

    ''' <summary>
    ''' Excel 出力処理
    ''' </summary>
    ''' <param name="sheetdata">SheetData オブジェクト</param>
    ''' <param name="dc_SupplierProductList">ReportOverviewList</param>
    ''' <param name="i_Row">出力行番号</param>
    ''' <returns>出力を終えた後の行番号</returns>
    Public Function CreateSupplierProductExport(ByVal sheetdata As SheetData, ByVal st_SupplierCode As String, ByVal st_SupplierName As String, ByVal st_Territory As String, _
                                                ByVal dc_SupplierProductList As List(Of TCIDataAccess.Join.ProductListBySupplierDisp), ByVal i_Row As Integer) As Integer

        'ProposalDeptCode のリスト作成
        Dim ProposalDeptList As New List(Of String)

        '出力用テーブル作成
        Dim dt_Table As New DataTable
        CreateTableData(dt_Table)

        Dim i_Col As Integer = TABLE_START_COL

        'ヘッダー部出力 ----------------------
        i_Row = AddHeaderRow(sheetdata, dt_Table, i_Col, i_Row, CellStyle._9PT_NONE_NONE_NONE, CellStyle._9PT_BOLD_NONE_NONE)

        'データ部出力 ------------------------
        Dim i_TotalByRow As Integer = 0
        For Each dc_SupplierProduct As  TCIDataAccess.Join.ProductListBySupplierDisp In dc_SupplierProductList

            Dim newRow As New Row
            dim i_DataCol = DATA_START_COL

            For Each dt_Row In dt_Table.Rows

                If i_DataCol = 1 Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_DataCol, i_Row, TypeCode.String, st_SupplierCode, CellStyle._9PT_NONE_NONE_NONE))
                End If
                If i_DataCol = 2 Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_DataCol, i_Row, TypeCode.String, st_SupplierName, CellStyle._9PT_NONE_NONE_NONE))
                End If
                If i_DataCol = 3 Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_DataCol, i_Row, TypeCode.String, st_Territory, CellStyle._9PT_NONE_NONE_NONE))
                End If
                If String.Equals(dt_Row("PropertyNumber"), "ProductNumber") Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_DataCol, i_Row, TypeCode.String, dc_SupplierProduct.ProductNumber, CellStyle._9PT_NONE_NONE_NONE))
                ElseIf String.Equals(dt_Row("PropertyNumber"), "CASNumber") Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_DataCol, i_Row, TypeCode.String, dc_SupplierProduct.CASNumber, CellStyle._9PT_NONE_NONE_NONE))
                ElseIf String.Equals(dt_Row("PropertyNumber"), "ProductName") Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_DataCol, i_Row, TypeCode.String, dc_SupplierProduct.ProductName, CellStyle._9PT_NONE_NONE_NONE))
                ElseIf String.Equals(dt_Row("PropertyNumber"), "Supplier Item Number") Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_DataCol, i_Row, TypeCode.String, dc_SupplierProduct.SupplierItemNumber, CellStyle._9PT_NONE_NONE_NONE))
                ElseIf String.Equals(dt_Row("PropertyNumber"), "Note") Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_DataCol, i_Row, TypeCode.String, dc_SupplierProduct.Note, CellStyle._9PT_NONE_NONE_NONE))
                ElseIf String.Equals(dt_Row("PropertyNumber"), "ValidQuotation") Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_DataCol, i_Row, TypeCode.String, dc_SupplierProduct.ValidQuotation, CellStyle._9PT_NONE_NONE_NONE))
                ElseIf String.Equals(dt_Row("PropertyNumber"), "UpdateDate") Then
                    newRow.CustomHeight = True
                    newRow.Height = 24.0
                    newRow.Append(NewCell(i_DataCol, i_Row, TypeCode.String, dc_SupplierProduct.UpdateDate, CellStyle._9PT_NONE_NONE_NONE))
                End If
                i_DataCol += 1
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

        dt.Rows.Add("", "SupplierCode", TypeCode.Int32, 0)
        dt.Rows.Add("", "SupplierName", TypeCode.Int32, 0)
        dt.Rows.Add("", "Territory", TypeCode.Int32, 0)

        dt.Rows.Add("", "ProductNumber", TypeCode.Int32, 0)
        dt.Rows.Add("", "CASNumber", TypeCode.Int32, 0)
        dt.Rows.Add("", "ProductName", TypeCode.Int32, 0)
        dt.Rows.Add("", "Supplier Item Number", TypeCode.Int32, 0)
        dt.Rows.Add("", "Note", TypeCode.Int32, 0)
        dt.Rows.Add("", "ValidQuotation", TypeCode.Int32, 0)
        dt.Rows.Add("", "UpdateDate", TypeCode.Int32, 0)


    End Sub

End Class

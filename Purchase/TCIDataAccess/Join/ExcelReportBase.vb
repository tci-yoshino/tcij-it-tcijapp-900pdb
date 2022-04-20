Imports System
Imports System.Data
Imports DocumentFormat.OpenXml
Imports DocumentFormat.OpenXml.Packaging
Imports DocumentFormat.OpenXml.Spreadsheet
Imports Ap = DocumentFormat.OpenXml.ExtendedProperties
Imports Vt = DocumentFormat.OpenXml.VariantTypes
Imports X14 = DocumentFormat.OpenXml.Office2010.Excel

''' <summary>
''' OpenXML 形式の Excel 帳票ベースクラス
''' </summary>
''' <remarks></remarks>
Public Class ExcelReportBase

    ''' <summary> Excel ContextType </summary>
    Public Const EXCEL_CONTENTTYPE As String = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"

    ''' <summary> Excel AppendHeader Name</summary>
    Public Const EXCEL_APPEND_HEADER_NAME As String = "content-disposition"

    ''' <summary> Excel AppendHeader Name</summary>
    Public Const EXCEL_APPEND_HEADER_VALUE As String = "attachment; filename={0}"

    ''' <summary> Cell Style </summary>
    ''' <remarks>サイズ_文字修飾_枠線_色</remarks>
    Public Enum CellStyle
        _9PT_NONE_NONE_NONE = 0               '標準
        _9PT_NONE_THIN_NONE                   '黒中線の囲い
        _9PT_NONE_NONE_LIGHT_TURQUOISE
        _9PT_NONE_NONE_LIGHT_GREEN
        _9PT_NONE_NONE_IVORY
        _9PT_NONE_NONE_LIME_GREEN
        _9PT_NONE_NONE_LIGHT_BLUE
        _14PT_BOLD_NONE_NONE
        _9PT_BOLD_NONE_NONE
        _11PT_BOLD_NONE_NONE
        _12PT_BOLD_ITALIC_NONE_NONE
    End Enum

    ''' <summary> 基本フォント名 </summary>
    Private Const BASIC_FONT_NAME As String = "Arial"

    ''' <summary> HttpResponse </summary>
    Protected _HttpResponse As System.Web.HttpResponse

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

    End Sub

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="HttpResponse">Response</param>
    Public Sub New(ByVal HttpResponse As System.Web.HttpResponse)
        _HttpResponse = HttpResponse
    End Sub

    ''' <summary>
    ''' テンプレｰトファイルから Excel (OpenXML 形式)デｰタ作成
    ''' </summary>
    ''' <param name="TemplateFilePath">テンプレｰトファイルパス</param>
    ''' <param name="DownloadFileName">ダウンロードファイル名</param>
    ''' <param name="ReplaceList">置換テーブル</param>
    ''' <remarks></remarks>
    Protected Sub CreateExcel(ByVal TemplateFilePath As String, ByVal DownloadFileName As String, _
                                                    ByVal ReplaceList As List(Of CellReplaceInfo))
        Dim outmemory As New System.IO.MemoryStream

        'テンプレートをコピー
        Dim bytes() As Byte = System.IO.File.ReadAllBytes(TemplateFilePath)
        outmemory.Write(bytes, 0, bytes.Length)


        'ファイルを作成したり保存したりするクラス
        Using document As SpreadsheetDocument = SpreadsheetDocument.Open(outmemory, True)

            'Zip に格納される workbook.xml をラップするクラス（ワｰクシｰトの XML の ID やシｰト名などのメタ情報を保持）
            Dim wbpart As WorkbookPart = document.WorkbookPart

            '共有文字列テーブル取得
            Dim stringtb = wbpart.GetPartsOfType(Of SharedStringTablePart).FirstOrDefault()

            For Each itemInfo In ReplaceList
                Dim st_Target As String = itemInfo.Target
                Dim st_Value As String = itemInfo.Value
                Dim st_SheetName As String = itemInfo.SheetName
                If itemInfo.IsCellReference Then
                    '対象となるシート検索
                    Dim sheet1 As Sheet = wbpart.Workbook.Descendants(Of Sheet)().Where( _
                                            Function(s) s.Name = st_SheetName).FirstOrDefault()
                    Dim wspart As WorksheetPart = CType(wbpart.GetPartById(sheet1.Id), WorksheetPart)

                    If itemInfo.ImageData IsNot Nothing AndAlso wspart.DrawingsPart IsNot Nothing Then
                        '=== 画像指定の場合

                        'DrawingsPart から ImagePart を検索
                        Dim imgpart = wspart.DrawingsPart.GetPartsOfType(Of ImagePart).FirstOrDefault()

                        'ImagePart  の Stream (画像) のデータを置換
                        Dim img As System.IO.MemoryStream = New System.IO.MemoryStream(itemInfo.ImageData)
                        imgpart.FeedData(img)
                        img.Close()

                    Else
                        '=== セルアドレス指定の場合

                        '対象となるセルを検索
                        Dim findCell = wspart.Worksheet.Descendants(Of Cell).Where( _
                                            Function(c) c.CellReference = st_Target).FirstOrDefault()
                        If findCell IsNot Nothing Then
                            findCell.DataType = itemInfo.CellDataType
                            findCell.CellValue = New CellValue(st_Value)
                        End If
                    End If

                Else
                    '=== 置換対象文字列指定の場合

                    'XMLコード用にエスケープ (「<　>　&　"　' 」を 「&lt;　&gt;　&amp;　&quot;　&apos;」に置き換える)
                    Dim st_ValueXml As String = System.Security.SecurityElement.Escape(st_Value)

                    '対象となる SharedStringItem を検索
                    Dim findItem = stringtb.SharedStringTable.Elements(Of SharedStringItem).Where( _
                                        Function(c) c.InnerText = st_Target).FirstOrDefault()
                    If Not findItem Is Nothing Then
                        findItem.InnerXml = findItem.InnerXml.Replace(st_Target, st_ValueXml)
                    End If
                End If
            Next

            'カスタマイズ出力
            Customize(wbpart)

        End Using

        '------------------------------------------------------------
        ' Excel ファイル出力
        '------------------------------------------------------------
        _HttpResponse.Clear()
        _HttpResponse.ContentType = EXCEL_CONTENTTYPE
        _HttpResponse.AppendHeader(EXCEL_APPEND_HEADER_NAME, String.Format(EXCEL_APPEND_HEADER_VALUE, DownloadFileName))
        _HttpResponse.BinaryWrite(outmemory.ToArray())
        _HttpResponse.End()

    End Sub

    ''' <summary>
    ''' カスタマイズ出力
    ''' </summary>
    ''' <param name="wbpart">WorkbookPart</param>
    ''' <remarks></remarks>
    Protected Overridable Sub Customize(ByVal wbpart As WorkbookPart)
        '継承先で個別に出力がある場合、実装する
    End Sub



    ''' <summary>
    ''' テンプレｰトファイルから Excel (OpenXML 形式) デｰタ作成
    ''' </summary>
    ''' <param name="WbPart">WorkbookPart オブジェクト</param>
    ''' <param name="ReplaceList">置換テーブル</param>
    Public Sub Replace(ByVal WbPart As WorkbookPart, _
                       ByVal ReplaceList As List(Of CellReplaceInfo))

        '共有文字列テーブル取得
        Dim stringtb = WbPart.GetPartsOfType(Of SharedStringTablePart).FirstOrDefault()

        'データ置換え
        For Each ItemInfo In ReplaceList
            Dim st_Target As String = ItemInfo.Target
            Dim st_Value As String = ItemInfo.Value
            If ItemInfo.IsCellReference Then
                Dim st_SheetName As String = ItemInfo.SheetName
                '対象となるシート検索
                Dim Sheet1 As Sheet = WbPart.Workbook.Descendants(Of Sheet)().Where(Function(s) s.Name = st_SheetName).FirstOrDefault()
                Dim WsPart As WorksheetPart = CType(WbPart.GetPartById(Sheet1.Id), WorksheetPart)
                '対象となるセルを検索
                Dim FindCell = WsPart.Worksheet.Descendants(Of Cell).Where(Function(c) c.CellReference = st_Target).FirstOrDefault()
                If FindCell IsNot Nothing Then
                    FindCell.DataType = ItemInfo.CellDataType
                    FindCell.CellValue = New CellValue(st_Value)
                    FindCell.StyleIndex = CellStyle._9PT_NONE_NONE_NONE
                End If
            Else
                '対象となるSharedStringItemを検索
                Dim FindItem = stringtb.SharedStringTable.Elements(Of SharedStringItem).Where(Function(c) c.InnerText = st_Target).FirstOrDefault()
                If Not FindItem Is Nothing Then
                    FindItem.InnerXml = FindItem.InnerXml.Replace(st_Target, st_Value)
                End If
            End If
        Next

    End Sub


    ''' <summary>
    ''' スタイル定義
    ''' </summary>
    ''' <param name="WbPart">WorkbookStylesPart オブジェクト</param>
    ''' <remarks></remarks>
    Public Sub GenerateWorkbookStylesPart(ByVal WbPart As WorkbookStylesPart)
        Dim newStyleSheet As New Stylesheet()

        '文字スタイル ------------------------
        Dim StyleFonts As New Fonts() With {.Count = 6}

        '#0 標準
        Dim font0 As New Font()
        Dim fontColor0 As New Font(New Color() With {.Rgb = "00000000"})
        Dim fontSize0 As New FontSize() With {.Val = 9D}
        Dim fontName0 As New FontName() With {.Val = BASIC_FONT_NAME}
        Dim fontFamilyNumbering0 As New FontFamilyNumbering() With {.Val = 3}
        Dim fontCharSet0 As New FontCharSet() With {.Val = 128}
        font0.Append(fontColor0)
        font0.Append(fontSize0)
        font0.Append(fontName0)
        font0.Append(fontFamilyNumbering0)
        font0.Append(fontCharSet0)
        StyleFonts.Append(font0)

        '#0 標準
        Dim font1 As New Font()
        Dim fontColor1 As New Font(New Color() With {.Rgb = "00000000"})
        Dim fontSize1 As New FontSize() With {.Val = 9D}
        Dim fontName1 As New FontName() With {.Val = BASIC_FONT_NAME}
        Dim fontFamilyNumbering1 As New FontFamilyNumbering() With {.Val = 3}
        Dim fontCharSet1 As New FontCharSet() With {.Val = 128}
        font1.Append(fontColor1)
        font1.Append(fontSize1)
        font1.Append(fontName1)
        font1.Append(fontFamilyNumbering1)
        font1.Append(fontCharSet1)
        StyleFonts.Append(font1)

        '#2 14pt. Bold
        Dim font2 As New Font()
        Dim fontColor2 As New Font(New Color() With {.Rgb = "00000000"})
        Dim fontSize2 As New FontSize() With {.Val = 14D}
        Dim fontName2 As New FontName() With {.Val = BASIC_FONT_NAME}
        Dim fontFamilyNumbering2 As New FontFamilyNumbering() With {.Val = 3}
        Dim fontCharSet2 As New FontCharSet() With {.Val = 128}
        Dim bold2 As New Bold()
        font2.Append(bold2)
        font2.Append(fontColor2)
        font2.Append(fontSize2)
        font2.Append(fontName2)
        font2.Append(fontFamilyNumbering2)
        font2.Append(fontCharSet2)
        StyleFonts.Append(font2)

        '#3 9pt. Bold
        Dim font3 As New Font()
        Dim fontColor3 As New Font(New Color() With {.Rgb = "00000000"})
        Dim fontSize3 As New FontSize() With {.Val = 9D}
        Dim fontName3 As New FontName() With {.Val = BASIC_FONT_NAME}
        Dim fontFamilyNumbering3 As New FontFamilyNumbering() With {.Val = 3}
        Dim fontCharSet3 As New FontCharSet() With {.Val = 128}
        Dim bold3 As New Bold()
        font3.Append(bold3)
        font3.Append(fontColor3)
        font3.Append(fontSize3)
        font3.Append(fontName3)
        font3.Append(fontFamilyNumbering3)
        font3.Append(fontCharSet3)
        StyleFonts.Append(font3)

        '#4 11pt. Bold
        Dim font4 As New Font()
        Dim fontColor4 As New Font(New Color() With {.Rgb = "00000000"})
        Dim fontSize4 As New FontSize() With {.Val = 11D}
        Dim fontName4 As New FontName() With {.Val = BASIC_FONT_NAME}
        Dim fontFamilyNumbering4 As New FontFamilyNumbering() With {.Val = 3}
        Dim fontCharSet4 As New FontCharSet() With {.Val = 128}
        Dim bold4 As New Bold()
        font4.Append(bold4)
        font4.Append(fontColor4)
        font4.Append(fontSize4)
        font4.Append(fontName4)
        font4.Append(fontFamilyNumbering4)
        font4.Append(fontCharSet4)
        StyleFonts.Append(font4)

        '#4 12pt. Bold Italic
        Dim font5 As New Font()
        Dim fontColor5 As New Font(New Color() With {.Rgb = "00000000"})
        Dim fontSize5 As New FontSize() With {.Val = 12D}
        Dim fontName5 As New FontName() With {.Val = BASIC_FONT_NAME}
        Dim fontFamilyNumbering5 As New FontFamilyNumbering() With {.Val = 3}
        Dim fontCharSet5 As New FontCharSet() With {.Val = 128}
        Dim bold5 As New Bold()
        Dim italic5 As New Italic()
        font5.Append(bold5)
        font5.Append(italic5)
        font5.Append(fontColor5)
        font5.Append(fontSize5)
        font5.Append(fontName5)
        font5.Append(fontFamilyNumbering5)
        font5.Append(fontCharSet5)
        StyleFonts.Append(font5)

        '背景色スタイル ----------------------
        Dim StyleFills As New Fills() With {.Count = 7}

        '#0 標準
        Dim fill0 As New Fill(New PatternFill() With {.PatternType = PatternValues.None})
        StyleFills.Append(fill0)

        '#1 Light Turquoise (dummy)
        Dim fill1 As New Fill()
        Dim PatternFill1 As New PatternFill() With {.PatternType = PatternValues.Solid}
        Dim backgroundColor1 As New BackgroundColor() With {.Rgb = "FFCCFFFF"}
        PatternFill1.Append(backgroundColor1)
        fill1.Append(PatternFill1)
        StyleFills.Append(fill1)

        '#2 Light Turquoise
        Dim fill2 As New Fill()
        Dim PatternFill2 As New PatternFill() With {.PatternType = PatternValues.Solid}
        Dim foregroundColor2 As New ForegroundColor() With {.Rgb = "FFCCFFFF"}
        Dim backgroundColor2 As New BackgroundColor() With {.Indexed = Convert.ToInt32(64)}
        PatternFill2.Append(foregroundColor2)
        PatternFill2.Append(backgroundColor2)
        fill2.Append(PatternFill2)
        StyleFills.Append(fill2)

        '#3 Light Green
        Dim fill3 As New Fill()
        Dim PatternFill3 As New PatternFill() With {.PatternType = PatternValues.Solid}
        Dim foregroundColor3 As New ForegroundColor() With {.Rgb = "FFCCFFCC"}
        Dim backgroundColor3 As New BackgroundColor() With {.Indexed = Convert.ToInt32(64)}
        PatternFill3.Append(foregroundColor3)
        PatternFill3.Append(backgroundColor3)
        fill3.Append(PatternFill3)
        StyleFills.Append(fill3)

        '#4 Ivory
        Dim fill4 As New Fill()
        Dim PatternFill4 As New PatternFill() With {.PatternType = PatternValues.Solid}
        Dim foregroundColor4 As New ForegroundColor() With {.Rgb = "FFFFFFCC"}
        Dim backgroundColor4 As New BackgroundColor() With {.Indexed = Convert.ToInt32(64)}
        PatternFill4.Append(foregroundColor4)
        PatternFill4.Append(backgroundColor4)
        fill4.Append(PatternFill4)
        StyleFills.Append(fill4)

        '#5 Lime Green
        Dim fill5 As New Fill()
        Dim PatternFill5 As New PatternFill() With {.PatternType = PatternValues.Solid}
        Dim foregroundColor5 As New ForegroundColor() With {.Rgb = "FFCCFF99"}
        Dim backgroundColor5 As New BackgroundColor() With {.Indexed = Convert.ToInt32(64)}
        PatternFill5.Append(foregroundColor5)
        PatternFill5.Append(backgroundColor5)
        fill5.Append(PatternFill5)
        StyleFills.Append(fill5)

        '#5 Lime Blue
        Dim fill6 As New Fill()
        Dim PatternFill6 As New PatternFill() With {.PatternType = PatternValues.Solid}
        Dim foregroundColor6 As New ForegroundColor() With {.Rgb = "FFCCECFF"}
        Dim backgroundColor6 As New BackgroundColor() With {.Indexed = Convert.ToInt32(64)}
        PatternFill6.Append(foregroundColor6)
        PatternFill6.Append(backgroundColor6)
        fill6.Append(PatternFill6)
        StyleFills.Append(fill6)


        '枠線スタイル ------------------------
        Dim StyleBorders As New Borders() With {.Count = 3}

        '#0 標準
        Dim border0 As New Border() With { _
            .LeftBorder = New LeftBorder(), _
            .RightBorder = New RightBorder(), _
            .TopBorder = New TopBorder(), _
            .BottomBorder = New BottomBorder(), _
            .DiagonalBorder = New DiagonalBorder() _
        }
        StyleBorders.Append(border0)

        '#1 黒・中線の囲い線
        Dim border1 As New Border() With { _
            .LeftBorder = New LeftBorder() With {.Style = BorderStyleValues.Thin, .Color = New Color() With {.Indexed = 64}}, _
            .RightBorder = New RightBorder() With {.Style = BorderStyleValues.Thin, .Color = New Color() With {.Indexed = 64}}, _
            .TopBorder = New TopBorder() With {.Style = BorderStyleValues.Thin, .Color = New Color() With {.Indexed = 64}}, _
            .BottomBorder = New BottomBorder() With {.Style = BorderStyleValues.Thin, .Color = New Color() With {.Indexed = 64}}, _
            .DiagonalBorder = New DiagonalBorder() _
        }
        StyleBorders.Append(border1)

        '#2 テスト用
        Dim border2 As New Border() With { _
            .LeftBorder = New LeftBorder() With {.Style = BorderStyleValues.Thin, .Color = New Color() With {.Indexed = 64}}, _
            .RightBorder = New RightBorder() With {.Style = BorderStyleValues.Thin, .Color = New Color() With {.Indexed = 64}}, _
            .TopBorder = New TopBorder() With {.Style = BorderStyleValues.Dotted, .Color = New Color() With {.Indexed = 64}}, _
            .BottomBorder = New BottomBorder() With {.Style = BorderStyleValues.MediumDashDotDot, .Color = New Color() With {.Indexed = 64}}, _
            .DiagonalBorder = New DiagonalBorder() _
        }
        StyleBorders.Append(border2)

        'スタイルパターン --------------------
        Dim StyleCellFormats As New CellFormats() With {.Count = 8}

        '#0 標準
        'Dim Alignment0 As New Alignment With {.ShrinkToFit = 1}
        'Dim cellFormat0 As New CellFormat() With {.NumberFormatId = 0, .FontId = 0, .FillId = 0, .BorderId = 0, .FormatId = 0, .Alignment = Alignment0}
        Dim cellFormat0 As New CellFormat() With {.NumberFormatId = 0, .FontId = 1, .FillId = 0, .BorderId = 0, .FormatId = 0}
        StyleCellFormats.Append(cellFormat0)

        '#1 黒中線の囲い
        Dim cellFormat1 As New CellFormat() With {.FontId = 1, .BorderId = 1}
        Dim alignment1 As New Alignment() With {.Vertical = VerticalAlignmentValues.Bottom, .WrapText = True}
        cellFormat1.Append(alignment1)
        StyleCellFormats.Append(cellFormat1)

        '#2 Light Turquoise
        Dim cellFormat2 As New CellFormat() With {.FontId = 1, .FillId = 2, .BorderId = 1}
        Dim alignment2 As New Alignment() With {.Vertical = VerticalAlignmentValues.Bottom, .WrapText = True}
        cellFormat2.Append(alignment2)
        StyleCellFormats.Append(cellFormat2)

        '#3 Light Green
        Dim cellFormat3 As New CellFormat() With {.FontId = 1, .FillId = 3, .BorderId = 1}
        Dim alignment3 As New Alignment() With {.Vertical = VerticalAlignmentValues.Bottom, .WrapText = True}
        cellFormat3.Append(alignment3)
        StyleCellFormats.Append(cellFormat3)

        '#4 Ivory
        Dim cellFormat4 As New CellFormat() With {.FontId = 1, .FillId = 4, .BorderId = 1}
        Dim alignment4 As New Alignment() With {.Vertical = VerticalAlignmentValues.Bottom, .WrapText = True}
        cellFormat4.Append(alignment4)
        StyleCellFormats.Append(cellFormat4)

        '#5 Lime Green
        Dim cellFormat5 As New CellFormat() With {.FontId = 1, .FillId = 5, .BorderId = 1}
        Dim alignment5 As New Alignment() With {.Vertical = VerticalAlignmentValues.Bottom, .WrapText = True}
        cellFormat5.Append(alignment5)
        StyleCellFormats.Append(cellFormat5)

        '#6 Lime Green
        Dim cellFormat6 As New CellFormat() With {.FontId = 1, .FillId = 6, .BorderId = 1}
        Dim alignment6 As New Alignment() With {.Vertical = VerticalAlignmentValues.Bottom, .WrapText = True}
        cellFormat6.Append(alignment6)
        StyleCellFormats.Append(cellFormat6)

        '#7 14Pt. Bold
        Dim cellFormat7 As New CellFormat() With {.NumberFormatId = 0, .FontId = 2, .FillId = 0, .BorderId = 0, .FormatId = 0}
        StyleCellFormats.Append(cellFormat7)

        '#8 9Pt. Bold
        Dim cellFormat8 As New CellFormat() With {.NumberFormatId = 0, .FontId = 3, .FillId = 0, .BorderId = 0, .FormatId = 0}
        StyleCellFormats.Append(cellFormat8)

        '#9 11Pt. Bold
        Dim cellFormat9 As New CellFormat() With {.NumberFormatId = 0, .FontId = 4, .FillId = 0, .BorderId = 0, .FormatId = 0}
        StyleCellFormats.Append(cellFormat9)

        '#9 12Pt. Bold Italic
        Dim cellFormat10 As New CellFormat() With {.NumberFormatId = 0, .FontId = 5, .FillId = 0, .BorderId = 0, .FormatId = 0}
        StyleCellFormats.Append(cellFormat10)

        ' スタイル・スタイルパターンをシートへ追加
        newStyleSheet.Append(StyleFonts)
        newStyleSheet.Append(StyleFills)
        newStyleSheet.Append(StyleBorders)
        newStyleSheet.Append(StyleCellFormats)
        WbPart.Stylesheet = newStyleSheet

    End Sub

    ''' <summary>
    ''' Excel の Cell オブジェクト作成
    ''' </summary>
    ''' <param name="ColNumber">列番号</param>
    ''' <param name="RowNumber">行番号</param>
    ''' <param name="CellDataType">データ型</param>
    ''' <param name="Value">値</param>
    ''' <param name="StyleIndex">スタイルインデックス</param>
    ''' <returns>Cell オブジェクト</returns>
    Protected Function NewCell(ByVal ColNumber As Integer, _
                               ByVal RowNumber As Integer, _
                               ByVal CellDataType As TypeCode, _
                               ByVal Value As Object, _
                               ByVal StyleIndex As Integer) As Cell
        'データ型を Excel 用のデータ型に変換
        Dim CellType As CellValues = CellValues.String
        Select Case CellDataType
            Case TypeCode.Boolean
                If StyleIndex > 0 Then
                    CellType = CellValues.Boolean
                End If
            Case TypeCode.DateTime
                If StyleIndex > 0 Then
                    CellType = CellValues.Date
                End If
            Case TypeCode.Decimal, TypeCode.Double, TypeCode.Single, TypeCode.Int16, TypeCode.Int32, TypeCode.Int64, _
                    TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt64
                CellType = CellValues.Number
            Case Else
        End Select

        '値を Excel 用の値に変換
        Dim st_Value As String = ""
        If (Not Value Is Nothing) AndAlso (Not Value Is DBNull.Value) Then
            If StyleIndex > 0 Then
                Select Case CellType
                    Case CellValues.Boolean
                        st_Value = IIf(CType(Value, Boolean), "1", "0")
                    Case CellValues.Date        'Date 型はシリアル値（書式定義ありの場合のみ）
                        st_Value = CType(Value, DateTime).ToOADate.ToString()
                    Case Else
                        st_Value = Value.ToString()
                End Select
            Else
                st_Value = Value.ToString()
            End If
        End If

        'Excel の Cell オブジェクトを作成して返す
        Dim NewCellItem As New Cell() With {.CellReference = String.Format("{0}{1}", ToA1(ColNumber), RowNumber), _
                                      .DataType = CellType, _
                                      .CellValue = New CellValue(st_Value)}
        If StyleIndex > 0 Then NewCellItem.StyleIndex = StyleIndex

        Return NewCellItem
    End Function

    ''' <summary>
    ''' Excel 出力処理 (ヘッダー部)
    ''' </summary>
    ''' <param name="ShtData">SheetData オブジェクト</param>
    ''' <param name="TableData">テーブルデータ</param>
    ''' <param name="ColNumber">出力列番号</param>
    ''' <param name="RowNumber">出力行番号</param>
    ''' <returns>出力を終えた後の行番号</returns>
    Public Function AddHeaderRow(ByVal ShtData As SheetData, _
                                 ByVal TableData As DataTable, _
                                 ByVal ColNumber As Integer, _
                                 ByVal RowNumber As Integer, _
                                 ByVal TitleStyle As CellStyle, _
                                 ByVal HeadStyle As CellStyle) As Integer

        Dim RowHeader As New Row
        Dim i_styleID As Integer = HeadStyle
        Dim st_Title As String
        For Each dt_Row In TableData.Rows
            Dim st_PropertyName As String = dt_Row("PropertyNumber").ToString
            If dt_Row("Title").ToString = String.Empty Then
                Dim dt_Status As New TCIDataAccess.RFQStatus

                st_Title = st_PropertyName
            Else
                '上記以外はプロパティ Title を出力
                st_Title = dt_Row("Title")
                If st_Title = String.Empty Then i_styleID = TitleStyle
            End If
            RowHeader.Append(NewCell(ColNumber, RowNumber, TypeCode.String, st_Title, i_styleID))
            i_styleID = HeadStyle
            ColNumber += 1
        Next
        RowNumber = AppendRow(ShtData, RowHeader, RowNumber)
        Return RowNumber

    End Function

    ''' <summary>
    ''' Excel 出力処理 (合計行)
    ''' </summary>
    ''' <param name="ShtData">SheetData オブジェクト</param>
    ''' <param name="TableData">テーブルデータ作成</param>
    ''' <param name="ColNumber">出力列番号</param>
    ''' <param name="RowNumber">出力行番号</param>
    ''' <returns>出力を終えた後の行番号</returns>
    Public Function AddTotalRow(ByVal ShtData As SheetData, _
                                ByVal TableData As DataTable, _
                                ByVal ColNumber As Integer, _
                                ByVal RowNumber As Integer, _
                                ByVal HeaderStyle As CellStyle) As Integer

        Dim newRow As New Row
        Dim i_TotalSum As Integer = 0

        For Each dt_Row In TableData.Rows

            If dt_Row("PropertyNumber") = "ROW_HEADER" Then
                'Total (固定値) 出力
                newRow.Append(NewCell(ColNumber, RowNumber, TypeCode.String, "Total", HeaderStyle))
            ElseIf dt_Row("PropertyNumber") = "ROW_TOTAL" Then
                '総合計出力
                newRow.Append(NewCell(ColNumber, RowNumber, TypeCode.Int32, i_TotalSum, CellStyle._9PT_NONE_THIN_NONE))
            Else
                '合計出力
                'Dim i_Total = CInt(dt_Row("PropertyNumber"))
                'newRow.Append(NewCell(ColNumber, RowNumber, TypeCode.Int32, i_Total, CellStyle._9PT_NONE_THIN_NONE))
                ''総合計加算
                'i_TotalSum += i_Total
            End If
            ColNumber += 1

        Next
        newRow.CustomHeight = True
        newRow.Height = 13.5
        RowNumber = AppendRow(ShtData, newRow, RowNumber)
        Return RowNumber

    End Function

    ''' <summary>
    ''' 行追加処理
    ''' </summary>
    ''' <param name="sheetdata">SheetData オブジェクト</param>
    ''' <param name="RowItem">Row オブジェクト</param>
    ''' <param name="RowNumber">出力行番号</param>
    ''' <returns>出力を終えた後の行番号</returns>
    Public Function AppendRow(ByVal SheetData As SheetData, _
                              ByVal RowItem As Row, _
                              ByVal RowNumber As Integer) As Integer
        SheetData.Append(RowItem)
        Return RowNumber + 1
    End Function

    ''' <summary>
    ''' 行追加処理
    ''' </summary>
    ''' <param name="sheetdata">SheetData オブジェクト</param>
    ''' <param name="ColNumber">列番号</param>
    ''' <param name="RowNumber">行番号</param>
    ''' <param name="Type">出力タイプ</param>
    ''' <param name="OutputData">出力データ</param>
    ''' <param name="Style">セルスタイル</param>
    ''' <param name="CostomeHeight">True: 行高さ変更あり False: なし</param>
    ''' <param name="Height">行高さ</param>
    ''' <returns>出力を終えた後の行番号</returns>
    Public Function AppendRow(ByVal SheetData As SheetData, _
                              ByVal ColNumber As Integer, _
                              ByVal RowNumber As Integer, _
                              ByVal Type As Integer, _
                              ByVal OutputData As Object, _
                              ByVal Style As CellStyle, _
                              ByVal CostomeHeight As Boolean, _
                              ByVal Height As Double) As Integer
        Dim newRow As New Row
        newRow.Append(NewCell(ColNumber, RowNumber, Type.ToString, OutputData, Style))
        If CostomeHeight Then
            newRow.CustomHeight = True
            newRow.Height = Height
        End If
        RowNumber = AppendRow(SheetData, newRow, RowNumber)
        Return RowNumber
    End Function

    ''' <summary>
    ''' 空行追加処理
    ''' </summary>
    ''' <param name="sheetdata">SheetData オブジェクト</param>
    ''' <param name="RowNumber">出力行番号</param>
    ''' <returns>出力を終えた後の行番号</returns>
    Public Function AppendRow(ByVal SheetData As SheetData, _
                              ByVal RowNumber As Integer) As Integer
        Dim newRow As New Row
        newRow.Append(NewCell(1, RowNumber, TypeCode.String, String.Empty, CellStyle._9PT_NONE_NONE_NONE))
        RowNumber = AppendRow(SheetData, newRow, RowNumber)
        Return RowNumber
    End Function

    ''' <summary>
    ''' R1C1 形式のアドレスを A1 形式に変換
    ''' </summary>
    ''' <param name="ColNumber">列番号(1～)</param>
    ''' <returns>A1 形式(ex."A1")</returns>
    ''' <remarks></remarks>
    Protected Function ToA1(ByVal ColNumber As Integer) As String
        Dim st_Alpha As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim st_A1 As String = ""

        If ColNumber <= 26 Then
            st_A1 = st_Alpha.Chars(ColNumber - 1)
        ElseIf ColNumber <= 702 Then
            st_A1 = st_Alpha.Chars(((ColNumber - 1) \ 26) - 1) & _
                    st_Alpha.Chars(((ColNumber - 1) Mod 26))
        Else
            st_A1 = st_Alpha.Chars(((ColNumber - 703) \ 676)) & _
                    st_Alpha.Chars((((ColNumber - 703) \ 26) Mod 26)) & _
                    st_Alpha.Chars(((ColNumber - 1) Mod 26))
        End If

        Return st_A1
    End Function

    '=== InnerClass ====================================================

    ''' <summary>
    ''' テンプレートに値セットする情報
    ''' </summary>
    ''' <remarks></remarks>
    Public Class CellReplaceInfo
        ''' <summary> True =セルアドレスで値をセット, False =置換対象文字列で値をセット </summary>
        Public IsCellReference As Boolean = False
        ''' <summary> 対象シート名(IsCellReference = True の場合のみ指定) </summary>
        Public SheetName As String
        ''' <summary> 置換対象文字列 or セルアドレス(ex."A1") </summary>
        Public Target As String
        ''' <summary> 値 </summary>
        Public Value As String
        ''' <summary> 画像データ </summary>
        Public ImageData As Byte()

        ''' <summary> データ型(IsCellReference = True の場合のみ指定) </summary>
        Public CellDataType As CellValues = CellValues.String

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' コンストラクタ（置換対象文字列を指定して値セット）
        ''' </summary>
        ''' <param name="st_Target">置換対象文字列</param>
        ''' <param name="st_Value">値</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal st_Target As String, ByVal st_Value As String)
            Me.IsCellReference = False
            Me.Target = st_Target
            Me.Value = st_Value
        End Sub

        ''' <summary>
        ''' コンストラクタ（セルアドレスを指定して値セット）
        ''' </summary>
        ''' <param name="st_SheetName">対象シート名</param>
        ''' <param name="st_CellRef">セルアドレス(ex."A1")</param>
        ''' <param name="cellDataType">データ型</param>
        ''' <param name="o_Value">値</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal st_SheetName As String, ByVal st_CellRef As String, ByVal cellDataType As TypeCode, ByVal o_Value As Object)
            Me.IsCellReference = True
            Me.SheetName = st_SheetName
            Me.Target = st_CellRef

            Select Case cellDataType
                Case TypeCode.DateTime
                    Me.CellDataType = CellValues.Date
                Case TypeCode.Decimal, TypeCode.Double, TypeCode.Single, TypeCode.Int16, TypeCode.Int32, TypeCode.Int64, _
                        TypeCode.UInt16, TypeCode.Int32, TypeCode.UInt64
                    Me.CellDataType = CellValues.Number
                Case Else
                    Me.CellDataType = CellValues.String
            End Select

            Me.Value = ""
            If (Not o_Value Is Nothing) AndAlso (Not o_Value Is DBNull.Value) Then
                Select Case cellDataType
                    Case TypeCode.DateTime
                        Me.Value = CType(o_Value, DateTime).ToOADate.ToString()
                    Case Else
                        Me.Value = o_Value.ToString()
                End Select
            End If
        End Sub

        ''' <summary>
        ''' コンストラクタ（画像を指定して値セット）
        ''' </summary>
        ''' <param name="st_SheetName">対象シート名</param>
        ''' <param name="by_ImageData">画像データ</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal st_SheetName As String, ByVal by_ImageData As Byte())
            Me.IsCellReference = True
            Me.SheetName = st_SheetName
            Me.ImageData = by_ImageData
        End Sub

    End Class

End Class

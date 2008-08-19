Option Explicit On

Imports System.Data.SqlClient
Imports Purchase.Common
Imports TCICommon.Func

Partial Public Class SuppliersProductImport
    Inherits CommonPage


#Region " Web フォーム デザイナで生成されたコード "
    '*****（Region内は変更しないこと）*****
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection
        Me.SqlConnection1.FireInfoMessageEventOnUserErrors = False
        Me.SqlConnection2 = New System.Data.SqlClient.SqlConnection
        Me.SqlConnection2.FireInfoMessageEventOnUserErrors = False
    End Sub

    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection
    Protected WithEvents SqlConnection2 As System.Data.SqlClient.SqlConnection

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub
#End Region

    ''' <summary>
    ''' エラー表示メッセージ定数です。
    ''' </summary>
    ''' <remarks></remarks>
    Const MSG_CANT_PREVIEW_ENV As String = "Previewできる環境でありません"
    Const MSG_CANT_IMPORT_ENV As String = "Importできる環境でありません"
    Const MSG_NOT_EXCEL_FILE As String = "読込みファイルはEXCELでありません"
    Const MSG_NOT_FILE_SET As String = "読込みファイルが設定されていません"
    Const MSG_NO_SUPPLIER_CODE As String = "SupplierCodeが設定されていません"
    Const MSG_ERR_CAS_NUMBER As String = "ERROR CAS_Number"

    ''' <summary>
    ''' SupplierProductList列位置定数
    ''' </summary>
    ''' <remarks></remarks>
    Const COL_POS_CAS As Integer = 0
    Const COL_POS_ITEM_NUMBER As Integer = 1
    Const COL_POS_ITEM_NAME As Integer = 2
    Const COL_POS_NOTE As Integer = 3


    'Stringの型タイプ定数です。
    Private ReadOnly TYPE_OF_STRING As System.Type = Type.GetType("System.String")

    'チェック画像表示HTMLタグ定数です。
    Const FILE_NAME_CHECK_IMAGE As String = "<img src=""./Image/Check.gif"" />"

    ''' <summary>
    ''' 他社プロダクト構造体です。
    ''' </summary>
    ''' <remarks></remarks>
    Private Structure CompetitorProductType
        Dim ALDRICH As String
        Dim ALFA As String
        Dim WAKO As String
        Dim KANTO As String

        ''' <summary>
        ''' 他社プロダクト構造体の初期化コンストラクタです。
        ''' </summary>
        ''' <param name="Value">初期化する値</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal Value As String)
            ALDRICH = String.Empty
            ALFA = String.Empty
            WAKO = String.Empty
            KANTO = String.Empty
        End Sub
    End Structure

    ''' <summary>
    ''' このページのロードイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの規定値</param>
    ''' <param name="e">ASP.NETの規定値</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then
            'If True Then
            If Request.QueryString("Supplier") <> "" Then
                Dim st_SupplierCode = Request.QueryString("Supplier").ToString()

                'テスト実行用ダミーコード
                'Dim st_SupplierCode As String = 1

                SupplierCode.Text = st_SupplierCode
                SupplierName.Text = GetSupplierNameBySupplierCode(st_SupplierCode)
            Else
                Msg.Text = MSG_NO_SUPPLIER_CODE
                File.Visible = False
                Preview.Visible = False
                Import.Visible = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' Previewボタンのクリックイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの規定値</param>
    ''' <param name="e">ASP.NETの規定値</param>
    ''' <remarks></remarks>
    Protected Sub Preview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Preview.Click

        Msg.Text = String.Empty

        'Actionパラメータの確認
        If Request.Form("Action") <> "Preview" Then
            Msg.Text = MSG_CANT_PREVIEW_ENV
            Exit Sub
        End If

        'ファイルのサーバ存在確認
        ''TODO 要確認
        If IO.Path.GetFileName(File.PostedFile.FileName) = String.Empty Then
            ClearSupplierProductList()
            Msg.Text = MSG_NOT_FILE_SET
            Exit Sub
        End If

        'ファイルタイプの確認(MIME)
        If Request.Files("File").ContentType <> "application/vnd.ms-excel" Then
            ClearSupplierProductList()
            Msg.Text = MSG_NOT_EXCEL_FILE
            Exit Sub
        End If

        Dim st_ExcelFileName As String = Request.Files("File").FileName
        ViewSupplierProductList(st_ExcelFileName)

        If SupplierProductList.Rows.Count > 0 Then
            Import.Visible = True
        End If

    End Sub


    ''' <summary>
    ''' SupplierProductListをクリアします。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ClearSupplierProductList()

        SupplierProductList.DataSourceID = String.Empty
        SupplierProductList.DataSource = String.Empty
        SupplierProductList.DataBind()
        Import.Visible = False

    End Sub


    ''' <summary>
    ''' 指定されたExcelファイルをフォーム上に表示します。
    ''' </summary>
    ''' <param name="ExcelFileName">指定されたExcelファイルのサーバ内パス</param>
    ''' <remarks></remarks>
    Private Sub ViewSupplierProductList(ByVal ExcelFileName As String)

        'Excelからデータをテーブルに取り込み
        Dim tb_Excel As DataTable = GetSuppliersProductTableFromExcel(ExcelFileName)

        '他社扱い情報をテーブルに設定
        SetCompetitorInfometionToTable(tb_Excel)

        '製品情報データをテーブルに設定
        SetProductInfometionToTable(tb_Excel)

        'チェック項目を画像ファイルに置き換え
        SetCheckImageHtmlToTable(tb_Excel)

        'テーブルデータを画面に表示
        SupplierProductList.DataSource = tb_Excel
        SupplierProductList.DataBind()

        'CASNumberエラー行をカラー表示
        SetCASErrorColorToSupplierProductList()

    End Sub

    ''' <summary>
    ''' サプライヤーコードからサプライヤーの名称を取得します。
    ''' </summary>
    ''' <param name="SupplierCode">サプライヤーコード</param>
    ''' <returns>サプライヤーの名称</returns>
    ''' <remarks></remarks>
    Private Function GetSupplierNameBySupplierCode(ByVal SupplierCode As String) As String
        Dim st_supplierName As String = String.Empty
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()

            cmd.CommandText = "SELECT Name3 FROM Supplier WHERE SupplierCode = @SupplierCode"
            cmd.Parameters.AddWithValue("SupplierCode", SupplierCode)

            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.Read = True Then
                st_supplierName = dr("Name3").ToString()
            End If
        Catch ex As Exception
            Throw
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
        Return st_supplierName
    End Function

    ''' <summary>
    ''' SupplierProductListのCASNumberエラー行をカラー表示し、エラー数を返します。
    ''' </summary>
    '''<returns>エラー行数</returns>
    ''' <remarks></remarks>
    Private Function SetCASErrorColorToSupplierProductList() As Integer

        Dim i_ErrCount As Integer = 0
        For Each row As GridViewRow In SupplierProductList.Rows

            Dim st_CAS = CType(row.FindControl("CASNumber"), TextBox).Text

            If Not IsCASNumber(st_CAS) And st_CAS <> String.Empty Then
                row.CssClass = "attention"
                i_ErrCount += 1
            End If

        Next
        Return i_ErrCount

    End Function


    ''' <summary>
    ''' テーブル内のチェック項目をHTMLのイメージタグへ置き換えます。
    ''' </summary>
    ''' <param name="Table">対象となるサプライヤプロダクトDataTable</param>
    ''' <remarks></remarks>
    Private Sub SetCheckImageHtmlToTable(ByRef Table As DataTable)

        For Each row As DataRow In Table.Rows
            row("AD") = CStr(IIf(row("AD") = "1", FILE_NAME_CHECK_IMAGE, String.Empty))
            row("AF") = CStr(IIf(row("AF") = "1", FILE_NAME_CHECK_IMAGE, String.Empty))
            row("WA") = CStr(IIf(row("WA") = "1", FILE_NAME_CHECK_IMAGE, String.Empty))
            row("KA") = CStr(IIf(row("KA") = "1", FILE_NAME_CHECK_IMAGE, String.Empty))
        Next

    End Sub

    ''' <summary>
    ''' 製品基本情報をテーブルに設定します。
    ''' </summary>
    ''' <param name="Table">対象となるサプライヤプロダクトDataTable</param>
    ''' <remarks></remarks>
    Private Sub SetProductInfometionToTable(ByRef Table As DataTable)

        Dim conn As SqlConnection = Nothing
        Dim i_DataCount As Integer
        Dim st_Separator As String

        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()
            cmd.CommandText = "SELECT ProductNumber,Status,ProposalDept,ProcumentDept FROM Product WHERE CASNumber = @CASNumber"
            conn.Open()

            For Each row As DataRow In Table.Rows
                cmd.Parameters.Clear()
                cmd.Parameters.AddWithValue("CASNumber", row("CAS Number"))

                Dim dr As SqlDataReader = cmd.ExecuteReader()

                i_DataCount = 0
                While dr.Read()
                    st_Separator = String.Empty
                    If i_DataCount > 1 Then
                        st_Separator = "<br/>"
                    End If

                    row("TCI Product Number") &= st_Separator & dr("ProductNumber").ToString()
                    row("EHS Status") &= st_Separator & dr("Status").ToString()
                    row("Proposal Dept") &= st_Separator & dr("ProposalDept").ToString()
                    row("Proc.Dept") &= st_Separator & dr("ProcumentDept").ToString()
                    i_DataCount += 1
                End While
                dr.Close()
            Next

        Catch ex As Exception
            Throw
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
    End Sub


    ''' <summary>
    ''' Excelからサプライヤー製品データのDataTableを生成します。
    ''' </summary>
    ''' <param name="ExcelFileName">対象となるExcelテーブル</param>
    ''' <returns>生成したDataTableオブジェクト</returns>
    ''' <remarks></remarks>
    Public Function GetSuppliersProductTableFromExcel(ByVal ExcelFileName As String) As DataTable

        Dim dsExcel As New DataSet
        Dim tbExcel As New DataTable

        'Excel OLEDB接続文字列の生成
        Dim conStrExcel As New OleDb.OleDbConnectionStringBuilder()
        conStrExcel.Provider = "Microsoft.JET.OLEDB.4.0"
        conStrExcel.DataSource = ExcelFileName
        conStrExcel("Extended Properties") = "Excel 8.0;HDR=YES;IMEX=1"

        'Excelデータの取得
        Dim sql As String = "SELECT * FROM [Sheet1$]"
        Using da As New OleDb.OleDbDataAdapter(sql, conStrExcel.ConnectionString)
            da.Fill(dsExcel, "SuppliersProductExcel")
        End Using

        tbExcel = dsExcel.Tables("SuppliersProductExcel")

        'Excelにないデータフィールドをテーブルに追加
        tbExcel.Columns.Add("TCI Product Number", TYPE_OF_STRING)
        tbExcel.Columns.Add("EHS Status", TYPE_OF_STRING)
        tbExcel.Columns.Add("Proposal Dept", TYPE_OF_STRING)
        tbExcel.Columns.Add("Proc.Dept", TYPE_OF_STRING)
        tbExcel.Columns.Add("AD", TYPE_OF_STRING)
        tbExcel.Columns.Add("AF", TYPE_OF_STRING)
        tbExcel.Columns.Add("WA", TYPE_OF_STRING)
        tbExcel.Columns.Add("KA", TYPE_OF_STRING)

        Return tbExcel

    End Function

    ''' <summary>
    ''' PhIDからEhsPhraseの英名を取得します。
    ''' </summary>
    ''' <param name="PhID">PhID</param>
    ''' <returns>EhsPhraseの英名</returns>
    ''' <remarks></remarks>
    Private Function GetEhsPhraseNameByPhID(ByVal PhID As String) As String
        Dim st_EhsPhraseName As String = String.Empty
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()

            cmd.CommandText = "SELECT ENai FROM dbo.s_EhsPhrase WHERE PhID = @PhID"
            cmd.Parameters.AddWithValue("PhID", PhID)

            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.Read = True Then
                st_EhsPhraseName = dr("ENai").ToString()
            Else
                st_EhsPhraseName = "-"
            End If
            Return st_EhsPhraseName
        Catch ex As Exception
            Throw
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
    End Function


    ''' <summary>
    ''' データテーブルへ他社扱い情報を設定します。
    ''' </summary>
    ''' <param name="Table">対象となるサプライヤプロダクトDataTable</param>
    ''' <remarks></remarks>
    Private Sub SetCompetitorInfometionToTable(ByRef Table As DataTable)

        Dim st_CASNumber As String
        Dim competitorProduct As CompetitorProductType

        For Each row As DataRow In Table.Rows
            st_CASNumber = row("CAS Number")
            competitorProduct = GetCompetitorProductByCASNumber(st_CASNumber)

            row("AD") = competitorProduct.ALDRICH
            row("AF") = competitorProduct.ALFA
            row("WA") = competitorProduct.WAKO
            row("KA") = competitorProduct.KANTO
        Next

    End Sub

    ''' <summary>
    ''' CasNumberから他社扱いの有無を取得します。
    ''' </summary>
    ''' <param name="CASNumber">対象となる製品のCasNumber</param>
    ''' <returns>取得した他社プロダクト構造体</returns>
    ''' <remarks></remarks>
    Private Function GetCompetitorProductByCASNumber(ByVal CASNumber As String) As CompetitorProductType
        Dim competitorProduct As New CompetitorProductType(String.Empty)
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()

            cmd.CommandText = "SELECT ALDRICH, ALFA, WAKO, KANTO FROM v_CompetitorProduct WHERE CASNumber = @CASNumber"
            cmd.Parameters.AddWithValue("CASNumber", CASNumber)

            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.Read() Then
                competitorProduct.ALDRICH = dr("ALDRICH").ToString()
                competitorProduct.ALFA = dr("ALFA").ToString()
                competitorProduct.WAKO = dr("WAKO").ToString()
                competitorProduct.KANTO = dr("KANTO").ToString()
            End If

            Return competitorProduct
        Catch ex As Exception
            Throw
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
    End Function


    ''' <summary>
    ''' インポートボタンのクリックイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの規定値</param>
    ''' <param name="e">ASP.NETの規定値</param>
    ''' <remarks></remarks>
    Protected Sub Import_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Import.Click

        If Request.Form("Action") <> "Import" Then
            Msg.Text = MSG_CANT_IMPORT_ENV
            Exit Sub
        End If

        'CASNumberのValidate 
        If SetCASErrorColorToSupplierProductList() > 0 Then
            Msg.Text = MSG_ERR_CAS_NUMBER
            Exit Sub
        End If

        'インポート処理の実行
        ImportData()

        Response.Redirect("./ProductListBySupplier.aspx?Supplier=" & SupplierCode.Text)
    End Sub


    ''' <summary>
    ''' 画面に表示された値をデータベースにインポートします。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ImportData()
        Dim st_CAS As String = String.Empty
        Dim st_ItemNo As String = String.Empty
        Dim st_ItemName As String = String.Empty
        Dim st_Note As String = String.Empty
        Dim st_ProductID As String = String.Empty

        Dim st_UserID As String = Session("UserID").ToString()
        Dim st_SupplierCode As String = SupplierCode.Text

        Dim conn As SqlConnection = New SqlConnection(DB_CONNECT_STRING)
        conn.Open()

        Dim sqlTran As SqlTransaction = Nothing
        For Each vrow As GridViewRow In SupplierProductList.Rows
            sqlTran = conn.BeginTransaction()
            Try
                st_CAS = CType(vrow.FindControl("CASNumber"), TextBox).Text
                st_ItemNo = vrow.Cells(COL_POS_ITEM_NUMBER).Text()
                st_ItemName = vrow.Cells(COL_POS_ITEM_NAME).Text()
                st_Note = vrow.Cells(COL_POS_NOTE).Text()

                'CAS番号を空白にした場合は処理対象外とします。
                If st_CAS = String.Empty Then
                    Continue For
                End If

                Dim dt As DataTable = GetDataTableFromProduct(st_CAS)
                If dt.Rows.Count > 0 Then
                    For Each rw As DataRow In dt.Rows
                        st_ProductID = rw("ProductID").ToString()

                        If rw("NumberType").ToString() = "CAS" Then
                            UpdateProduct(st_CAS, st_UserID, st_ProductID, st_CAS, conn, sqlTran)
                        End If

                        If ExistsSupplierProductData(st_ProductID, SupplierCode.Text) Then
                            UpdateSupplierProduct(st_ItemNo, st_Note, st_UserID, st_SupplierCode, st_ProductID, conn, sqlTran)
                        Else
                            InsertSupplierProduct(SupplierCode.Text(), st_ProductID, st_ItemNo, st_Note, st_UserID, conn, sqlTran)

                        End If
                    Next
                Else
                    st_ProductID = InsertProduct(st_CAS, st_ItemName, st_CAS, st_UserID, conn, sqlTran)
                    InsertSupplierProduct(st_SupplierCode, st_ProductID, st_ItemNo, st_Note, st_UserID, conn, sqlTran)

                End If
                sqlTran.Commit()
            Catch ex As Exception
                sqlTran.Rollback()
                Throw
            End Try
        Next

    End Sub


    ''' <summary>
    ''' Productテーブルから指定したCASNumberのデータをDataTableで取得します。
    ''' </summary>
    ''' <param name="CASNumber"></param>
    ''' <returns>生成したDataTableオブジェクト</returns>
    ''' <remarks></remarks>
    Private Function GetDataTableFromProduct(ByVal CASNumber As String) As DataTable

        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim st_SQL As String = "SELECT ProductID, ProductNumber, NumberType, Name, CASNumber FROM Product WHERE CASNumber = @CASNumber"

            Dim da As SqlDataAdapter = New SqlDataAdapter(st_SQL, conn)
            da.SelectCommand.Parameters.AddWithValue("CASNumber", CASNumber)

            Dim ds As DataSet = New DataSet()
            da.Fill(ds)

            Return ds.Tables(0)

        Catch ex As Exception
            Throw
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try

    End Function

    ''' <summary>
    ''' SupplierProductDataに指定のデータがあるかを返します。
    ''' </summary>
    ''' <param name="ProductID"></param>
    ''' <param name="SupplierCode"></param>
    ''' <returns>データが1件以上あるときはTure ない時はFalseを返します</returns>
    ''' <remarks></remarks>
    Private Function ExistsSupplierProductData(ByVal ProductID As String, ByVal SupplierCode As String)
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()

            cmd.CommandText = "SELECT Count(*) FROM Supplier_Product WHERE ProductID = @ProductID AND SupplierCode = @SupplierCode"
            cmd.Parameters.AddWithValue("ProductID", ProductID)
            cmd.Parameters.AddWithValue("SupplierCode", SupplierCode)

            conn.Open()
            Dim i_DataCount As Integer = CInt(cmd.ExecuteScalar())
            If i_DataCount > 0 Then
                Return True
            End If

        Catch ex As Exception
            Throw
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
        Return False

    End Function



    ''' <summary>
    ''' Productテーブルへデータを挿入し、ProductIDを返します。
    ''' </summary>
    ''' <param name="ProductNumber"></param>
    ''' <param name="Name"></param>
    ''' <param name="CASNumber"></param>
    ''' <param name="CreatedBy"></param>
    ''' <param name="Conn">SqlConnnectionオブジェクト</param>
    ''' <returns>取得したProductID</returns>
    ''' <remarks>トランザクション有効化のため、生成済みのSqlConnectionを参照渡しで受けます</remarks>
    Private Function InsertProduct(ByVal ProductNumber As String, ByVal Name As String, _
        ByVal CASNumber As String, ByVal CreatedBy As String, ByRef Conn As SqlConnection, _
        ByRef SqlTran As SqlTransaction) As String

        Dim cmd As SqlCommand = Conn.CreateCommand()
        cmd.Transaction = SqlTran

        cmd.CommandText = CreateSQLForInsertProduct()
        cmd.Parameters.AddWithValue("ProductNumber", ConvertEmptyStringToNull(ProductNumber))
        cmd.Parameters.AddWithValue("Name", ConvertEmptyStringToNull(Name))
        cmd.Parameters.AddWithValue("CASNumber", ConvertEmptyStringToNull(CASNumber))
        cmd.Parameters.AddWithValue("CreatedBy", ConvertStringToInt(CreatedBy))
        cmd.Parameters.AddWithValue("UpdatedBy", ConvertStringToInt(CreatedBy))

        If Conn.State <> ConnectionState.Open Then
            Conn.Open()
        End If

        cmd.ExecuteNonQuery()

        '挿入した行の一意IDを取得します。
        Dim st_SQL As String = "Select @@IDENTITY AS ID"
        'Dim st_SQL As String = "Select SCOPE_IDENTITY() AS ID"
        cmd.CommandText = st_SQL
        Dim st_ProductID As String = cmd.ExecuteScalar().ToString()

        Return st_ProductID

    End Function


    ''' <summary>
    ''' Product挿入SQL文字列を生成します。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateSQLForInsertProduct() As String
        Dim sb_SQL As StringBuilder = New StringBuilder()

        sb_SQL.Append("INSERT INTO Product ")
        sb_SQL.Append("( ")
        sb_SQL.Append("	ProductNumber, ")
        sb_SQL.Append("	NumberType, ")
        sb_SQL.Append("	Name, ")
        sb_SQL.Append("	CASNumber, ")
        sb_SQL.Append("	CreatedBy, ")
        sb_SQL.Append("	CreateDate, ")
        sb_SQL.Append("	UpdatedBy, ")
        sb_SQL.Append("	UpdateDate ")
        sb_SQL.Append(") ")
        sb_SQL.Append("values ")
        sb_SQL.Append("( ")
        sb_SQL.Append("	@ProductNumber, ")
        sb_SQL.Append("	'CAS', ")
        sb_SQL.Append("	@Name, ")
        sb_SQL.Append("	@CASNumber, ")
        sb_SQL.Append("	@CreatedBy, ")
        sb_SQL.Append("	GETDATE(), ")
        sb_SQL.Append("	@UpdatedBy, ")
        sb_SQL.Append("	GETDATE() ")
        sb_SQL.Append(") ")

        Return sb_SQL.ToString()

    End Function


    ''' <summary>
    ''' Productテーブルへデータを更新します。
    ''' </summary>
    ''' <param name="Name"></param>
    ''' <param name="UpdateBy"></param>
    ''' <param name="ProductID"></param>
    ''' <param name="CASNumber"></param>
    ''' <param name="Conn">SqlConnnectionオブジェクト</param>
    ''' <remarks>トランザクション有効化のため、生成済みのSqlConnectionを参照渡しで受けます</remarks>
    Private Sub UpdateProduct(ByVal Name As String, ByVal UpdateBy As String, _
                              ByVal ProductID As String, ByVal CASNumber As String, _
                              ByRef Conn As SqlConnection, ByRef SqlTran As SqlTransaction)

        Dim cmd As SqlCommand = Conn.CreateCommand()
        cmd.Transaction = SqlTran

        cmd.CommandText = CreateSQLForUpdateProduct()
        cmd.Parameters.AddWithValue("Name", ConvertEmptyStringToNull(Name))
        cmd.Parameters.AddWithValue("UpdatedBy", ConvertStringToInt(UpdateBy))
        cmd.Parameters.AddWithValue("ProductID", ConvertStringToInt(ProductID))
        cmd.Parameters.AddWithValue("CASNumber", ConvertEmptyStringToNull(CASNumber))

        If Conn.State <> ConnectionState.Open Then
            Conn.Open()
        End If

        cmd.ExecuteNonQuery()

    End Sub

    ''' <summary>
    ''' Product更新SQL文字列を生成します。
    ''' </summary>
    ''' <returns>生成した文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateSQLForUpdateProduct() As String
        Dim sb_SQL As StringBuilder = New StringBuilder()

        sb_SQL.Append("UPDATE Product ")
        sb_SQL.Append("SET ")
        sb_SQL.Append("	Name = @Name, ")
        sb_SQL.Append("	UpdatedBy = @UpdatedBy, ")
        sb_SQL.Append("	UpdateDate = GETDATE() ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("	ProductID = @ProductID ")
        sb_SQL.Append("	AND CASNumber = @CASNumber ")

        Return sb_SQL.ToString()

    End Function


    ''' <summary>
    ''' SupplierProductProductテーブルへデータを挿入します。
    ''' </summary>
    ''' <param name="SupplierCode"></param>
    ''' <param name="ProductID"></param>
    ''' <param name="SupplierItemNumber"></param>
    ''' <param name="Note"></param>
    ''' <param name="UpdateBy"></param>
    ''' <param name="Conn">SqlConnnectionオブジェクト</param>
    ''' <remarks>トランザクション有効化のため、生成済みのSqlConnectionを参照渡しで受けます</remarks>
    Private Sub InsertSupplierProduct(ByVal SupplierCode As String, ByVal ProductID As String, _
                                      ByVal SupplierItemNumber As String, ByVal Note As String, _
                                      ByVal CreatedBy As String, ByRef Conn As SqlConnection, _
                                      ByRef SqlTran As SqlTransaction)

        Dim cmd As SqlCommand = Conn.CreateCommand()
        cmd.Transaction = SqlTran

        cmd.CommandText = CreateSQLForInsertSupplierProduct()
        cmd.Parameters.AddWithValue("SupplierCode", ConvertStringToInt(SupplierCode))
        cmd.Parameters.AddWithValue("ProductID", ConvertStringToInt(ProductID))
        cmd.Parameters.AddWithValue("SupplierItemNumber", ConvertEmptyStringToNull(SupplierItemNumber))
        cmd.Parameters.AddWithValue("Note", ConvertEmptyStringToNull(Note))
        cmd.Parameters.AddWithValue("CreatedBy", ConvertStringToInt(CreatedBy))
        cmd.Parameters.AddWithValue("UpdatedBy", ConvertStringToInt(CreatedBy))

        If Conn.State <> ConnectionState.Open Then
            Conn.Open()
        End If
        cmd.ExecuteNonQuery()

    End Sub

    ''' <summary>
    ''' SupplierProduct挿入SQL文字列を生成します。
    ''' </summary>
    ''' <returns>生成した文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateSQLForInsertSupplierProduct() As String
        Dim sb_SQL As StringBuilder = New StringBuilder()

        sb_SQL.Append("INSERT INTO Supplier_Product ")
        sb_SQL.Append("(")
        sb_SQL.Append("	SupplierCode,")
        sb_SQL.Append("	ProductID,")
        sb_SQL.Append("	SupplierItemNumber,")
        sb_SQL.Append("	Note,")
        sb_SQL.Append("	CreatedBy,")
        sb_SQL.Append("	CreateDate,")
        sb_SQL.Append("	UpdatedBy,")
        sb_SQL.Append("	UpdateDate")
        sb_SQL.Append(")")
        sb_SQL.Append("VALUES")
        sb_SQL.Append("(")
        sb_SQL.Append("	@SupplierCode,")
        sb_SQL.Append("	@ProductID,")
        sb_SQL.Append("	@SupplierItemNumber,")
        sb_SQL.Append("	@Note,")
        sb_SQL.Append("	@CreatedBy,")
        sb_SQL.Append("	GETDATE(),")
        sb_SQL.Append("	@UpdatedBy,")
        sb_SQL.Append("	GETDATE()")
        sb_SQL.Append(")")

        Return sb_SQL.ToString()

    End Function


    ''' <summary>
    ''' SupplierProductテーブルを更新します。
    ''' </summary>
    ''' <param name="SupplierItemNumber"></param>
    ''' <param name="Note"></param>
    ''' <param name="UpdateBy"></param>
    ''' <param name="ProductID"></param>
    ''' <param name="Conn">SqlConnnectionオブジェクト</param>
    ''' <remarks>トランザクション有効化のため、生成済みのSqlConnectionを参照渡しで受けます</remarks>
    Private Sub UpdateSupplierProduct(ByVal SupplierItemNumber As String, ByVal Note As String, _
                                      ByVal UpdatedBy As String, ByVal SupplierCode As String, _
                                      ByVal ProductID As String, ByRef Conn As SqlConnection, _
                                      ByRef SqlTran As SqlTransaction)

        Dim cmd As SqlCommand = Conn.CreateCommand()
        cmd.Transaction = SqlTran

        cmd.CommandText = CreateSQLForUpdateSupplierProduct()
        cmd.Parameters.AddWithValue("SupplierItemNumber", ConvertEmptyStringToNull(SupplierItemNumber))
        cmd.Parameters.AddWithValue("Note", ConvertEmptyStringToNull(Note))
        cmd.Parameters.AddWithValue("UpdatedBy", ConvertStringToInt(UpdatedBy))
        cmd.Parameters.AddWithValue("SupplierCode", ConvertStringToInt(SupplierCode))
        cmd.Parameters.AddWithValue("ProductID", ConvertStringToInt(ProductID))

        If Conn.State <> ConnectionState.Open Then
            Conn.Open()
        End If
        cmd.ExecuteNonQuery()

    End Sub


    ''' <summary>
    ''' SupplierProduct更新SQL文字列を生成します。
    ''' </summary>
    ''' <returns>生成した文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateSQLForUpdateSupplierProduct() As String
        Dim sb_SQL As StringBuilder = New StringBuilder()

        sb_SQL.Append("UPDATE ")
        sb_SQL.Append("	Supplier_Product ")
        sb_SQL.Append("SET ")
        sb_SQL.Append("	SupplierItemNumber = @SupplierItemNumber, ")
        sb_SQL.Append("	Note = @Note, ")
        sb_SQL.Append("	UpdatedBy= @UpdatedBy, ")
        sb_SQL.Append("	UpdateDate= GETDATE() ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("	SupplierCode = @SupplierCode ")
        sb_SQL.Append("	AND ProductID = @ProductID ")

        Return sb_SQL.ToString()
    End Function



End Class
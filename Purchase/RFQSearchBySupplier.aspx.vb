Option Explicit On

Imports System.Data.SqlClient
Imports Purchase.Common

''' <summary>
''' RFQSearchBySupplierクラス
''' </summary>
''' <remarks>Supplier情報から見積依頼を検索します。</remarks>
Partial Public Class RFQSearchBySupplier
    Inherits CommonPage

    ''' <summary>
    ''' Supplier Codeの入力チェックで使用するエラーメッセージ
    ''' </summary>
    ''' <remarks></remarks>
    Private Const ERR_INCORRECT_SUPPLIERCODE As String = "Supplier Code" & ERR_INVALID_NUMBER
    ''' <summary>
    ''' RFQ検索キー構造体です。
    ''' </summary>
    ''' <remarks></remarks>
    Structure SearchKey
        Dim Code As String
        Dim R3Code As String
        Dim Name As String
        Dim Country As String
        Dim Region As String
    End Structure

    ''' <summary>
    ''' このクラスのロードイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        'No match found表示防止
        SupplierList.Visible = False
        If IsPostBack = False Then
            Call SetPostBackUrl()
        End If

        SrcSupplier.SelectCommand = String.Empty
    End Sub

    ''' <summary>
    ''' 検索ボタンのクリックイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        If Request.QueryString("Action") <> "Search" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If
        Dim i As Integer = 0
        Msg.Text = String.Empty
        'List初期化
        SrcSupplier.SelectCommand = ""
        SrcSupplier.DataBind()
        SupplierList.DataBind()
        '画面表示OK
        SupplierList.Visible = True
        '入力チェック
        If Not IsCheckInput() Then
            '何も入力されていない場合、aspxファイルの「EmptyDataTemplate」を表示するため処理無し。
            Exit Sub
        End If
        'Supplier Code は半角英数のみ
        SupplierCode.Text = StrConv(SupplierCode.Text, VbStrConv.Narrow)
        R3SupplierCode.Text = StrConv(R3SupplierCode.Text, VbStrConv.Narrow)
        'Supplier Code は数値型
        If Not Integer.TryParse(SupplierCode.Text, i) And Not SupplierCode.Text.Trim = String.Empty Then
            Msg.Text = ERR_INCORRECT_SUPPLIERCODE
            'LISTは非表示とする。
            SupplierList.Visible = False
            Exit Sub
        End If

        '入力された検索キーを構造体に代入
        Dim st_SearchKey As SearchKey
        st_SearchKey.Code = SupplierCode.Text.Trim
        st_SearchKey.R3Code = R3SupplierCode.Text.Trim
        st_SearchKey.Name = SupplierName.Text.Trim
        st_SearchKey.Country = Country.SelectedValue
        st_SearchKey.Region = Region.SelectedValue

        '検索の実行
        SearchRFQ(st_SearchKey)

    End Sub

    ''' <summary>
    ''' PostBackUrl の設定を行います。
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub SetPostBackUrl()
        Search.PostBackUrl = "~/RFQSearchBySupplier.aspx?Action=Search"
    End Sub


    ''' <summary>
    ''' Country ドロップダウンリストの変更時イベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks>UpdatePanelコントロールによるAjaxの非同期ポストバックで実行されます。</remarks>
    Protected Sub Country_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Country.SelectedIndexChanged
        Region.Items.Clear()
        Region.Items.Add(String.Empty)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "myscript", "colorful.set();navi('supplier');", True)
    End Sub

    ''' <summary>
    ''' クリアボタンクリックイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks>UpdatePanelコントロールによるAjaxの非同期ポストバックで実行されます。</remarks>
    Protected Sub Clear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Clear.Click
        Country.SelectedIndex = 0
        Region.Items.Clear()
        Region.Items.Add(String.Empty)
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType(), "myscript", "colorful.set();navi('supplier');", True)
    End Sub

    ''' <summary>
    ''' 入力チェック(全て未入力の場合)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function IsCheckInput() As Boolean
        If SupplierCode.Text.Trim = String.Empty And _
           R3SupplierCode.Text.Trim = String.Empty And _
           SupplierName.Text.Trim = String.Empty And _
           Country.SelectedValue = String.Empty And _
           Region.SelectedValue = String.Empty Then
            '検索条件が全て空白の場合
            Return False
        End If
        Return True
    End Function


    ''' <summary>
    ''' RFQの検索を行います。
    ''' </summary>
    ''' <param name="st_SearchKey">RFQ検索キー構造体</param>
    ''' <remarks></remarks>
    Private Sub SearchRFQ(ByVal st_SearchKey As SearchKey)

        Dim st_SQL As String = CreateSQLSentence(st_SearchKey)
        SrcSupplier.SelectCommand = st_SQL

        SrcSupplier.SelectParameters.Clear()
        SetParamesToSQL(st_SearchKey, SrcSupplier.SelectParameters)

        '1件のみ検索された場合の処理
        Dim st_SupplierCode As String = GetProductCodeWhenOneRecord(st_SearchKey)

        If st_SupplierCode <> String.Empty Then
            Dim st_URI As String = "./RFQListBySupplier.aspx?SupplierCode={0}"
            st_URI = String.Format(st_URI, st_SupplierCode)
            Response.Redirect(st_URI, False)
        Else
            SrcSupplier.DataBind()
        End If
    End Sub

    ''' <summary>
    ''' RFQ検索SQL文字列を生成します。
    ''' </summary>
    ''' <param name="st_SearchKey">RFQ検索キー構造体</param>
    ''' <returns>生成したSQL文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateSQLSentence(ByVal st_SearchKey As SearchKey) As String

        Dim sb_SQL As New Text.StringBuilder

        'SQL文字列の作成
        sb_SQL.Append("SELECT ")
        'sb_SQL.Append("	rfh.RFQNumber, ")
        sb_SQL.Append("	DISTINCT ")
        sb_SQL.Append("	SupplierCode, ")
        sb_SQL.Append("	R3SupplierCode, ")
        sb_SQL.Append("	LTRIM(RTRIM(ISNULL(Name3, '') + ' ' + ISNULL(Name4, ''))) AS SupplierName ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("	Supplier ")
        sb_SQL.Append("WHERE ")

        'スカラ変数にワイルドカードは入らないので、検索キー入力値でSQL条件生成を変化させます。
        Dim sb_SQLConditional As New Text.StringBuilder

        If st_SearchKey.Code <> String.Empty Then
            sb_SQLConditional.Append(IIf(sb_SQLConditional.Length > 0, " AND ", String.Empty))
            sb_SQLConditional.Append("SupplierCode = @SupplierCode ")
        End If

        If st_SearchKey.R3Code <> String.Empty Then
            sb_SQLConditional.Append(IIf(sb_SQLConditional.Length > 0, " AND ", String.Empty))
            sb_SQLConditional.Append("R3SupplierCode = @R3SupplierCode ")
        End If

        If st_SearchKey.Name <> String.Empty Then
            sb_SQLConditional.Append(IIf(sb_SQLConditional.Length > 0, " AND ", String.Empty))
            sb_SQLConditional.Append("(ISNULL(Name3,'') + N' ' + ISNULL(Name4,'') LIKE @SupplierName) ")
        End If

        If st_SearchKey.Country <> String.Empty Then
            sb_SQLConditional.Append(IIf(sb_SQLConditional.Length > 0, " AND ", String.Empty))
            sb_SQLConditional.Append("CountryCode = @Country ")
        End If

        If st_SearchKey.Region <> String.Empty Then
            sb_SQLConditional.Append(IIf(sb_SQLConditional.Length > 0, " AND ", String.Empty))
            sb_SQLConditional.Append("RegionCode = @RegionCode ")
        End If

        sb_SQL.Append(sb_SQLConditional.ToString())
        sb_SQL.Append(" ORDER BY SupplierName ")
        Return sb_SQL.ToString()

    End Function

    ''' <summary>
    ''' RFQ検索キーをSQLParametersへバインドします。
    ''' </summary>
    ''' <param name="Key">RFQ検索キー構造体</param>
    ''' <param name="Parameters">対象SQLParameters</param>
    ''' <remarks></remarks>
    Private Sub SetParamesToSQL(ByVal Key As SearchKey, ByRef Parameters As SqlParameterCollection)
        Dim R3SupplierCodeNumber As Integer = 0
        Dim SupplierName As String = String.Empty
        If Key.Code <> String.Empty Then
            Parameters.AddWithValue("SupplierCode", Key.Code)
        End If

        If Key.R3Code <> String.Empty Then
            If Integer.TryParse(Key.R3Code, R3SupplierCodeNumber) = True Then
                '数値を入力された場合は0詰めの処理を行う。
                Parameters.AddWithValue("R3SupplierCode", String.Format("{0:D10}", R3SupplierCodeNumber))
            Else
                Parameters.AddWithValue("R3SupplierCode", Key.R3Code)
            End If
        End If

        If Key.Name <> String.Empty Then
            SupplierName = "%{0}%"
            SupplierName = String.Format(SupplierName, SafeSqlLikeClauseLiteral(Key.Name))
            Parameters.AddWithValue("SupplierName", SupplierName)
        End If

        If Key.Country <> String.Empty Then
            Parameters.AddWithValue("Country", Key.Country)
        End If

        If Key.Region <> String.Empty Then
            Parameters.AddWithValue("RegionCode", Key.Region)
        End If


    End Sub

    ''' <summary>
    ''' RFQ検索キーをSQLParametersへバインドします。
    ''' </summary>
    ''' <param name="Key">RFQ検索キー構造体</param>
    ''' <param name="Parameters">対象SQLParameters</param>
    ''' <remarks></remarks>
    Private Sub SetParamesToSQL(ByVal Key As SearchKey, ByRef Parameters As Web.UI.WebControls.ParameterCollection)
        Dim R3SupplierCodeNumber As Integer = 0
        Dim SupplierName As String = String.Empty

        If Key.Code <> String.Empty Then
            Parameters.Add("SupplierCode", Key.Code)
        End If

        If Key.R3Code <> String.Empty Then
            If Integer.TryParse(Key.R3Code, R3SupplierCodeNumber) = True Then
                '数値を入力された場合は0詰めの処理を行う。
                Parameters.Add("R3SupplierCode", String.Format("{0:D10}", R3SupplierCodeNumber))
            Else
                Parameters.Add("R3SupplierCode", Key.R3Code)
            End If
        End If

        If Key.Name <> String.Empty Then
            SupplierName = "%{0}%"
            SupplierName = String.Format(SupplierName, SafeSqlLikeClauseLiteral(Key.Name))
            Parameters.Add("SupplierName", SupplierName)
        End If

        If Key.Country <> String.Empty Then
            Parameters.Add("Country", Key.Country)
        End If

        If Key.Region <> String.Empty Then
            Parameters.Add("RegionCode", Key.Region)
        End If

    End Sub

    Private Function GetProductCodeWhenOneRecord(ByVal st_SearchKey As SearchKey) As String

        Dim sqlConn As SqlConnection = New SqlConnection(DB_CONNECT_STRING)
        Dim sqlCmd As SqlCommand = New SqlCommand(CreateSQLSentence(st_SearchKey), sqlConn)
        SetParamesToSQL(st_SearchKey, sqlCmd.Parameters)

        Dim adp As SqlDataAdapter = New SqlDataAdapter(sqlCmd)

        Dim ds As DataSet = New DataSet()

        adp.Fill(ds, "RFQ")

        If ds.Tables("RFQ").Rows.Count = 1 Then
            Return ds.Tables("RFQ").Rows(0)("SupplierCode").ToString()
        Else
            Return String.Empty
        End If

    End Function
End Class
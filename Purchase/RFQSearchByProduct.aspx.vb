''' <summary>
''' RFQSearchByProductクラス
''' </summary>
''' <remarks>製品から見積依頼を検索します。</remarks>
Partial Public Class RFQSearchByProduct
    Inherits CommonPage

    ''' <summary>
    ''' 必須項目漏れのエラーメッセージ定数です。
    ''' </summary>
    ''' <remarks></remarks>
    Const MSG_REQUIED_PRODUCT_NUMBER = "Product Numberを入力してください"

    ''' <summary>
    ''' RFQ検索キー構造体です。
    ''' </summary>
    ''' <remarks></remarks>
    Structure SearchKey
        Dim Code As String
        Dim CAS As String
        Dim RFQ As String
    End Structure

    ''' <summary>
    ''' このクラスのロードイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'No match found表示防止
        ProductList.Visible = IsPostBack

        SrcProduct.SelectCommand = String.Empty

    End Sub

    ''' <summary>
    ''' 検索ボタンのクリックイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks></remarks>
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click

        '必須入力項目のチェック
        If Code.Text.Trim = String.Empty Then
            Msg.Text = MSG_REQUIED_PRODUCT_NUMBER
            Exit Sub
        Else
            Msg.Text = String.Empty
        End If

        '入力された検索キーを構造体に代入
        Dim st_SearchKey As SearchKey
        st_SearchKey.Code = Code.Text.Trim
        st_SearchKey.CAS = CAS.Text.Trim
        st_SearchKey.RFQ = RFQ.Text.Trim

        '検索の実行
        SearchRFQ(st_SearchKey)

    End Sub

    ''' <summary>
    ''' RFQの検索を行います。
    ''' </summary>
    ''' <param name="st_SearchKey">RFQ検索キー構造体</param>
    ''' <remarks></remarks>
    Private Sub SearchRFQ(ByVal st_SearchKey As SearchKey)

        Dim st_SQL As String = CreateRFQSelectSQL(st_SearchKey)
        SrcProduct.SelectCommand = st_SQL

        SetRFQSelectSQLParames(st_SearchKey, SrcProduct)

        ProductList.DataBind()

    End Sub


    ''' <summary>
    ''' RFQ検索SQL文字列を生成します。
    ''' </summary>
    ''' <param name="st_SearchKey">RFQ検索キー構造体</param>
    ''' <returns>生成したSQL文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateRFQSelectSQL(ByVal st_SearchKey As SearchKey) As String

        Dim sb_SQL As New Text.StringBuilder

        'SQL文字列の作成
        sb_SQL.Append("SELECT ")
        'sb_SQL.Append("	rfh.RFQNumber, ")
        sb_SQL.Append("	DISTINCT ")
        sb_SQL.Append("	rfh.ProductID, ")
        sb_SQL.Append("	rfh.ProductNumber, ")
        sb_SQL.Append("	rfh.ProductName ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("	v_RFQHeader rfh ")
        sb_SQL.Append("	LEFT OUTER JOIN ")
        sb_SQL.Append("	Product pr ")
        sb_SQL.Append("ON ")
        sb_SQL.Append("	pr.ProductNumber = rfh.ProductNumber ")
        sb_SQL.Append("WHERE ")

        'スカラ変数にワイルドカードは入らないので、検索キー入力値でSQL条件生成を変化させます。
        Dim sb_SQLConditional As New Text.StringBuilder

        If st_SearchKey.Code <> String.Empty Then
            sb_SQLConditional.Append(CStr(IIf(sb_SQLConditional.Length > 1, " AND ", String.Empty)))
            sb_SQLConditional.Append("pr.ProductNumber = @ProductNumber ")
        End If

        If st_SearchKey.CAS <> String.Empty Then
            sb_SQLConditional.Append(CStr(IIf(sb_SQLConditional.Length > 1, " AND ", String.Empty)))
            sb_SQLConditional.Append("pr.CASNumber = @CASNumber ")
        End If

        If st_SearchKey.RFQ <> String.Empty Then
            sb_SQLConditional.Append(CStr(IIf(sb_SQLConditional.Length > 1, " AND ", String.Empty)))
            sb_SQLConditional.Append("rfh.RFQNumber = @RFQNumber ")
        End If

        sb_SQL.Append(sb_SQLConditional.ToString())

        Return sb_SQL.ToString()

    End Function

    ''' <summary>
    ''' RFQ検索キーをSqlDataSourceへバインドします。
    ''' </summary>
    ''' <param name="st_SearchKey">RFQ検索キー構造体</param>
    ''' <param name="ds_SrcProduct">対象SqlDataSource</param>
    ''' <remarks></remarks>
    Private Sub SetRFQSelectSQLParames(ByVal st_SearchKey As SearchKey, ByRef ds_SrcProduct As SqlDataSource)

        ds_SrcProduct.SelectParameters.Clear()
        If st_SearchKey.Code <> String.Empty Then
            ds_SrcProduct.SelectParameters.Add("ProductNumber", st_SearchKey.Code)
        End If

        If st_SearchKey.CAS <> String.Empty Then
            ds_SrcProduct.SelectParameters.Add("CASNumber", st_SearchKey.CAS)
        End If

        If st_SearchKey.RFQ <> String.Empty Then
            ds_SrcProduct.SelectParameters.Add("RFQNumber", st_SearchKey.RFQ)
        End If

    End Sub
End Class
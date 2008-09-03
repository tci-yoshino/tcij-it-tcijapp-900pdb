Option Explicit On

Imports System.Data.SqlClient
Imports Purchase.Common

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
    Const MSG_REQUIED_PRODUCT_NUMBER As String = "Product Numberを入力してください"
    Const MSG_FIELD_RFQ As String = "RFQ Reference Number"

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
        If IsPostBack Then
            ProductList.Visible = True
        Else
            ProductList.Visible = False
        End If

        SrcProduct.SelectCommand = String.Empty

    End Sub

    ''' <summary>
    ''' 検索ボタンのクリックイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks></remarks>
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click

        Msg.Text = String.Empty

        '必須入力項目のチェック
        If Code.Text.Trim = String.Empty Then
            Msg.Text = MSG_REQUIED_PRODUCT_NUMBER
            Exit Sub
        End If

        If Not IsNaturalNumber(RFQ.Text) And RFQ.Text.Trim <> String.Empty Then
            Msg.Text = MSG_FIELD_RFQ + ERR_INCORRECT_FORMAT
            Exit Sub
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
    ''' 対象が自然数（0以上の整数）かどうかをチェックし結果を返します。
    ''' </summary>
    ''' <param name="value">対象となる Object</param>
    ''' <returns>自然数のときはTrue 異なるときはFalseを返します</returns>
    ''' <remarks></remarks>
    Private Function IsNaturalNumber(ByVal value As Object) As Boolean

        If Not IsNumeric(value) Then
            Return False
        End If

        Dim i_Value As Integer

        If Not Integer.TryParse(value, i_Value) Then
            Return False
        End If

        If i_Value <= 0 Then
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
        SrcProduct.SelectCommand = st_SQL

        SrcProduct.SelectParameters.Clear()
        SetParamesToSQL(st_SearchKey, SrcProduct.SelectParameters)

        '1件のみ検索された場合の処理
        Dim st_ProductCode As String = GetProductCodeWhenOneRecord(st_SearchKey)

        If st_ProductCode <> String.Empty Then
            Dim st_URI As String = "./RFQListByProduct.aspx?ProductID={0}"
            st_URI = String.Format(st_URI, st_ProductCode)
            Response.Redirect(st_URI, False)
        Else
            ProductList.DataBind()
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
            Return ds.Tables("RFQ").Rows(0)("ProductID").ToString()
        Else
            Return String.Empty
        End If

    End Function



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
        sb_SQL.Append("	pr.ProductID, ")
        sb_SQL.Append("	pr.ProductNumber, ")
        sb_SQL.Append("	pr.[Name] As ProductName ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("	Product pr ")
        sb_SQL.Append("	LEFT OUTER JOIN ")
        sb_SQL.Append("	v_RFQHeader rfh ")
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

        sb_SQL.Append("ORDER BY ")
        sb_SQL.Append(" ProductName ")

        Return sb_SQL.ToString()

    End Function

    ''' <summary>
    ''' RFQ検索キーをSQLParametersへバインドします。
    ''' </summary>
    ''' <param name="Key">RFQ検索キー構造体</param>
    ''' <param name="Parameters">対象SQLParameters</param>
    ''' <remarks></remarks>
    Private Sub SetParamesToSQL(ByVal Key As SearchKey, ByRef Parameters As SqlParameterCollection)

        '検索キーサニタイジング
        Key.Code = SafeSqlLikeClauseLiteral(Key.Code)
        Key.CAS = SafeSqlLikeClauseLiteral(Key.CAS)
        Key.RFQ = SafeSqlLikeClauseLiteral(Key.RFQ)

        If Key.Code <> String.Empty Then
            Parameters.AddWithValue("ProductNumber", Key.Code)
        End If

        If Key.CAS <> String.Empty Then
            Parameters.AddWithValue("CASNumber", Key.CAS)
        End If

        If Key.RFQ <> String.Empty Then
            Parameters.AddWithValue("RFQNumber", Key.RFQ)
        End If

    End Sub

    ''' <summary>
    ''' RFQ検索キーをSQLParametersへバインドします。
    ''' </summary>
    ''' <param name="Key">RFQ検索キー構造体</param>
    ''' <param name="Parameters">対象SQLParameters</param>
    ''' <remarks></remarks>
    Private Sub SetParamesToSQL(ByVal Key As SearchKey, ByRef Parameters As Web.UI.WebControls.ParameterCollection)

        If Key.Code <> String.Empty Then
            Parameters.Add("ProductNumber", Key.Code)
        End If

        If Key.CAS <> String.Empty Then
            Parameters.Add("CASNumber", Key.CAS)
        End If

        If Key.RFQ <> String.Empty Then
            Parameters.Add("RFQNumber", Key.RFQ)
        End If

    End Sub

End Class
Option Explicit On
Option Strict On
Option Infer Off

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
    Const MSG_REQUIED_PRODUCT_NUMBER As String = "Product Number" & Common.ERR_REQUIRED_FIELD     'Product Numberを入力してください

    Protected b_ProductListView_Flg As Boolean = True

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
        b_ProductListView_Flg = False

        '必須入力項目のチェック
        If Code.Text.Trim = String.Empty Then
            Msg.Text = MSG_REQUIED_PRODUCT_NUMBER
            Exit Sub
        End If

        'フラグの有効化
        b_ProductListView_Flg = True

        '検索の実行
        SearchRFQ(Code.Text.Trim)

    End Sub


    ''' <summary>
    ''' RFQの検索を行います。
    ''' </summary>
    ''' <param name="productNumber">検索キーとなるProductNumber(またはCASNumber)</param>
    ''' <remarks></remarks>
    Private Sub SearchRFQ(ByVal productNumber As String)

        Dim st_SQL As String = CreateSQLSentence()
        SrcProduct.SelectCommand = st_SQL

        SrcProduct.SelectParameters.Clear()
        productNumber = SafeSqlLikeClauseLiteral(productNumber)
        SrcProduct.SelectParameters.Add("SearchParam", productNumber)


        '1件のみ検索された場合の処理
        Dim i_ProductID As Integer? = GetProductCodeWhenOneRecord(productNumber)

        If Not (i_ProductID Is Nothing) Then
            Dim st_URI As String = "./RFQListByProduct.aspx?ProductID={0}"
            st_URI = String.Format(st_URI, i_ProductID.ToString())
            Response.Redirect(st_URI, False)
        Else
            ProductList.DataBind()
        End If
    End Sub

    ''' <summary>
    ''' 検索条件から一件のみのデータが抽出できる場合のみProductIDを返します。
    ''' </summary>
    ''' <param name="productNumber">検索キーとなるProductNumber(またはCASNumber)</param>
    ''' <returns>取得したProductID 該当がない時は空白を返します。</returns>
    ''' <remarks></remarks>
    Private Function GetProductCodeWhenOneRecord(ByVal productNumber As String) As Integer?

        Using sqlConn As SqlConnection = New SqlConnection(DB_CONNECT_STRING)
            Dim sqlCmd As SqlCommand = New SqlCommand(CreateSQLSentence(), sqlConn)
            sqlCmd.Parameters.AddWithValue("SearchParam", productNumber)

            Dim adp As SqlDataAdapter = New SqlDataAdapter(sqlCmd)
            Dim ds As DataSet = New DataSet()

            adp.Fill(ds, "Product")

            If ds.Tables("Product").Rows.Count = 1 Then
                Return CType(ds.Tables("Product").Rows(0)("ProductID"), Integer?)
            Else
                Return Nothing
            End If
        End Using

    End Function


    ''' <summary>
    ''' RFQ検索SQL文字列を生成します。
    ''' </summary>
    ''' <returns>生成したSQL文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateSQLSentence() As String

        Dim sb_SQL As New Text.StringBuilder

        'SQL文字列の作成
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("	 DISTINCT ")
        sb_SQL.Append("	 ProductID, ")
        sb_SQL.Append("	 ProductNumber, ")
        sb_SQL.Append("	 [Name] As ProductName ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("	 Product ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("	 ProductNumber = @SearchParam OR ")
        sb_SQL.Append("	 CASNumber = @SearchParam ")
        sb_SQL.Append("ORDER BY ")
        sb_SQL.Append("  ProductName ")

        Return sb_SQL.ToString()

    End Function

End Class
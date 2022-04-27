Option Explicit On
Option Strict On
Option Infer Off

Imports System.Data.SqlClient
Imports Purchase.Common
Imports Purchase.TCIDataAccess
Imports Purchase.TCIDataAccess.Join

Partial Public Class ProductSearchByKeyword
    Inherits CommonPage

    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <remarks>
    ''' ページを読み込む
    ''' </remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            UnDsp_ProductList()
        End If

    End Sub

    ''' <summary>
    ''' Clearボタン押下
    ''' </summary>
    ''' <remarks>
    ''' ProductNumberとProductNameと一覧を初期化する
    ''' </remarks>
    Protected Sub Clear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Clear.Click
        Msg.Text = String.Empty
        ProductNumber.Text = Nothing
        ProductName.Text = Nothing

    End Sub

    ''' <summary>
    ''' Searchボタン押下
    ''' </summary>
    ''' <remarks>
    ''' 入力されたProductNumberとProductNameに入力された条件に該当する情報を一覧に表示する。
    ''' </remarks>
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        Msg.Text = String.Empty
        '[Search実行可能確認]----------------------------------------------------------
        If Not String.Equals(Action.Value, "Search") Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub

        End If

        '[入力ProductNumberの正規化]---------------------------------------------------
        ProductNumber.Text = StrConv(ProductNumber.Text, VbStrConv.Narrow)
        ProductNumber.Text = UCase(ProductNumber.Text)

        st_ProductName.Value = ProductName.Text
        st_ProductNumber.Value = ProductNumber.Text

        ' 検索条件が未入力ならエラーメッセージを表示する
        If String.IsNullOrWhiteSpace(st_ProductName.Value) And String.IsNullOrWhiteSpace(st_ProductNumber.Value) Then
            Msg.Text = Common.ERR_NO_MATCH_FOUND
            UnDsp_ProductList()
            Exit Sub

        End If

        '[ProductListを表示]-----------------------------------------------------
        ProductList.Visible = True

        '検索を実行する
        Dim productSearchByKeywordDispList As ProductSearchByKeywordDispList = New ProductSearchByKeywordDispList

        productSearchByKeywordDispList.ProductNumber = st_ProductNumber.Value
        productSearchByKeywordDispList.Name = st_ProductName.Value
        '検索結果が100件以下の場合は検索結果を表示する
        '[ProductListにデータ設定]-----------------------------------------------------
        productSearchByKeywordDispList.Load(Session(SESSION_ROLE_CODE).ToString)

        Dim ds_ProductSearchByKeywordDispList As List(Of ProductSearchByKeywordDisp) = New List(Of ProductSearchByKeywordDisp) 
        Dim i_RFQCount As Integer = productSearchByKeywordDispList.Count

        If i_RFQCount = 0 Then
            '検索結果が0件ならエラーメッセージを表示する
            Msg.Text = Common.ERR_NO_MATCH_FOUND
            UnDsp_ProductList()
            Exit Sub

        ElseIf i_RFQCount > common.LIST_ONEPAGE_ROW_ProductSearchByKeyword Then
            '検索結果が100件以上ならエラーメッセージを表示する
            Dim ary_Msg As ArrayList = New ArrayList
            ary_Msg.Add(Common.LIST_ONEPAGE_ROW_ProductSearchByKeyword.ToString)
            Msg.Text = common.CreateMSG(Common.MSG_RESULT_OVER_LIMIT, ary_Msg)
            ds_ProductSearchByKeywordDispList = productSearchByKeywordDispList.GetRange(0, Common.LIST_ONEPAGE_ROW_ProductSearchByKeyword)
        Else
            ds_ProductSearchByKeywordDispList = productSearchByKeywordDispList
        End If

        Dsp_ProductList(ds_ProductSearchByKeywordDispList)

    End Sub

    ''' <summary>
    ''' 一覧のプロパティを変更にします。
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Protected Sub ProductList_PagePropertiesChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ProductList.PagePropertiesChanged
        Dim productSearchByKeywordDisp As ProductSearchByKeywordDispList = New ProductSearchByKeywordDispList
        'productSearchByKeywordDisp.ProductNumber = st_ProductNumber.Value
        'productSearchByKeywordDisp.Name = st_ProductName.Value
        'productSearchByKeywordDisp.Load(Session(SESSION_ROLE_CODE).ToString)

        'Dsp_ProductList(productSearchByKeywordDisp)

    End Sub

    ''' <summary>
    ''' 一覧とページャーを表示する。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Dsp_ProductList(ByVal list  As List(Of ProductSearchByKeywordDisp))
        ProductList.DataSource = list
        ProductList.DataBind

        ProductList.Visible = True

    End Sub

    ''' <summary>
    ''' 一覧とページャーを非表示にする。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub UnDsp_ProductList()
        ProductList.DataSource = Nothing
        ProductList.DataBind()

        ProductList.Visible = False

    End Sub

End Class
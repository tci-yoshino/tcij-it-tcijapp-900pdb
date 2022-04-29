Option Explicit On
Option Infer Off
Option Strict On

Imports System.Data.SqlClient
Imports Purchase.Common

''' <summary>
''' RFQListByProductクラス
''' </summary>
''' <remarks>製品からRFQ一覧を表示します</remarks>
Partial Public Class RFQListByProduct
    Inherits CommonPage

    Protected st_ProductID As String
    Protected i_DataNum As Integer = 0 ' 0 の場合は Supplier Data が無いと判断し、 Data not found. を表示する。
    Private st_ProductNumber As String

    ''' <summary>
    ''' このページのロードイベントです。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' パラメータ取得
        If Request.RequestType = "POST" And IsPostBack = False Then
            st_ProductID = CType(IIf(Request.Form("ProductID") = Nothing, "", Request.Form("ProductID")), String)
        ElseIf Request.RequestType = "GET" Or IsPostBack = True Then
            st_ProductID = CType(IIf(Request.QueryString("ProductID") = Nothing, "", Request.QueryString("ProductID")), String)
        End If

        If st_ProductID = "" Or IsInteger(st_ProductID) = False Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        If Not IsPostBack Then 
            ' Valid Quotation ドロップダウンリスト設定
            Common.SetValidQuotationList(ValidQuotation, "All")

            '' 一覧検索
            st_ProductID = st_ProductID.Trim()
            ShowList()

            ' EHSHeader 設定
            HeaderEhs.UserID = Integer.Parse(Session("UserID").ToString)
            HeaderEhs.LocationCode = Session("LocationCode").ToString
            HeaderEhs.ProductNumber = st_ProductNumber
            HeaderEhs.GetEhsHeader

        End If

    End Sub

    Private Sub ShowList
        ' 製品情報検索
        SearchProduct(st_ProductID)
        ' 見積情報検索
        SearchRFQHeader(st_ProductID)
    End Sub

    ''' <summary>
    ''' Searchボタン押下
    ''' </summary>
    ''' <remarks>
    ''' 入力されたProductNumberとProductNameに入力された条件に該当する情報を一覧に表示する。
    ''' </remarks>
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        'メッセージクリア
        Msg.Text = String.Empty
        '[Search実行可能確認]----------------------------------------------------------
        If Not String.Equals(Action.Value, "Search") Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub

        End If

        '[ProductListを表示]-----------------------------------------------------
        RFQHeaderList.Visible = True

        '' 一覧検索
        st_ProductID = st_ProductID.Trim()
        ShowList()

    End Sub

    ''' <summary>
    ''' Releaseボタンクリックイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Release_Click(sender As Object, e As EventArgs) Handles Release.Click
        'メッセージクリア
        Msg.Text = String.Empty
        'Valid Quotation クリア
        ValidQuotation.SelectedIndex = 0

        '' 一覧検索
        st_ProductID = st_ProductID.Trim()
        ShowList()

    End Sub

    Private Sub ReSetPager()

        'ページャーを初期化
        Dim PgrRFQPagerCountTop As DataPager
        PgrRFQPagerCountTop = CType(RFQHeaderList.FindControl("RFQPagerCountTop"), DataPager)

        Dim PgRFQPagerLinkTop As DataPager
        PgRFQPagerLinkTop = CType(RFQHeaderList.FindControl("RFQPagerLinkTop"), DataPager)

        Dim PgrRFQPagerLinkBottom As DataPager
        PgrRFQPagerLinkBottom = CType(RFQHeaderList.FindControl("RFQPagerLinkBottom"), DataPager)

        Dim PgrRFQPagerCountBottom As DataPager
        PgrRFQPagerCountBottom = CType(RFQHeaderList.FindControl("RFQPagerCountBottom"), DataPager)

        'ResetPageTemplatePagerField(PgrRFQPagerCountTop)
        ResetPageNumericPagerField(PgRFQPagerLinkTop)
        'ResetPageNumericPagerField(PgrRFQPagerLinkBottom)
        'ResetPageTemplatePagerField(PgrRFQPagerCountBottom)

    End Sub

    ''' <summary>
    ''' ページを初期化します。
    ''' </summary>
    private Sub ResetPageNumericPagerField(ByVal dp As DataPager)
        If Not IsNothing(dp) And Not dp.StartRowIndex = 0 Then
            Dim numericPF As NumericPagerField = Ctype(dp.Fields(0), NumericPagerField)
            If Not IsNothing(numericPF) Then
　　　　　　　　'' 引数に0をセット
                Dim args As CommandEventArgs = New CommandEventArgs("0", "")
　　　　　　　　'' イベント発生
                numericPF.HandleEvent(args)
            End If
        End If
    End Sub

    ''' <summary>
    ''' RFQList プロパティ変更時イベントハンドラ
    ''' </summary>
    ''' <remarks>
    ''' 
    ''' </remarks>
    Protected Sub RFQHeaderList_PagePropertiesChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RFQHeaderList.PagePropertiesChanged
        ' 一覧を表示する（ページャー押下時）
        ShowList()
    End Sub

    ''' <summary>
    ''' 製品の検索を行います。
    ''' </summary>
    ''' <param name="st_ProductID">製品ID</param>
    ''' <remarks></remarks>
    Private Sub SearchProduct(ByVal st_ProductID As String)
        Dim rFQListByProductDisp As TCIDataAccess.Join.RFQListByProductDisp = New TCIDataAccess.Join.RFQListByProductDisp 

        Using connection As New SqlClient.SqlConnection(DB_CONNECT_STRING)

            Dim command As New SqlClient.SqlCommand(rFQListByProductDisp.CreateProductHeaderSelectSQL(), connection)
            connection.Open()

            command.Parameters.AddWithValue("ProductID", st_ProductID)

            Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()

            If reader.Read() Then
                i_DataNum = 1
                ProductNumber.Text = reader("ProductNumber").ToString()
                st_ProductNumber = reader("ProductNumber").ToString()
                If Not IsDBNull(reader("QuoName")) Then
                    QuoName.Text = reader("QuoName").ToString()
                Else
                    QuoName.Text = reader("Name").ToString()
                End If
                ProductName.Text = reader("Name").ToString()
                labBUoM.Text = reader("BUoM").ToString()
                CASNumber.Text = reader("CASNumber").ToString()
                ProductWarning.Text = reader("ProductWarning").ToString()
                MolecularFormula.Text = reader("MolecularFormula").ToString()
            End If
            reader.Close()
        End Using
    End Sub

    ''' <summary>
    ''' 見積依頼の検索を行います。
    ''' </summary>
    ''' <param name="st_ProductID">製品ID</param>
    ''' <remarks></remarks>
    Private Sub SearchRFQHeader(ByVal st_ProductID As String)
        Dim rFQListByProduct As TCIDataAccess.Join.RFQListByProductDisp = New TCIDataAccess.Join.RFQListByProductDisp
        rFQListByProduct.ValidityQuotation = Me.ValidQuotation.SelectedValue

        If String.Equals(Action.Value, "Release") Then 
            '' 条件変更時はページャーをリセット
            ReSetPager
        End If

        SrcRFQHeader.SelectCommand = rFQListByProduct.CreateRFQHeaderSelectSQL()
        SrcRFQHeader.SelectParameters.Clear()
        SrcRFQHeader.SelectParameters.Add("ProductID", st_ProductID)
        RFQHeaderList.DataBind()

    End Sub


    ''' <summary>
    ''' RFQ詳細ListItemのデータバインドイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NET規定値</param>
    ''' <param name="e">ASP.NET規定値</param>
    ''' <remarks></remarks>
    Protected Sub GetRFQLine(ByVal sender As Object, ByVal e As EventArgs) Handles RFQHeaderList.ItemDataBound
        Dim lv As ListView = CType(CType(e, ListViewItemEventArgs).Item.FindControl("RFQLineList"), ListView)
        Dim src As SqlDataSource = CType(CType(e, ListViewItemEventArgs).Item.FindControl("SrcRFQLine"), SqlDataSource)
        Dim link As HyperLink = CType(CType(e, System.Web.UI.WebControls.ListViewItemEventArgs).Item.FindControl("RFQNumber"), HyperLink)

        Dim rFQListByProduct As TCIDataAccess.Join.RFQListByProductDisp = New TCIDataAccess.Join.RFQListByProductDisp

        src.SelectParameters.Clear()
        src.SelectParameters.Add("RFQNumber", link.Text)
        src.SelectCommand = rFQListByProduct.CreateRFQLineSelectSQL()
        lv.DataSourceID = src.ID
        lv.DataBind()
    End Sub

    Protected Sub SrcRFQHeader_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcRFQHeader.Selecting
        e.Command.CommandTimeout = 0
    End Sub

    Public Function GetRFQStatus(ByRef RFQNumber As String, ByRef RFQLineNumber As String) As String
        Dim ret As String = ""
        'Dim dt As DataTable = GetDataTable("select RFQStatusCode from  RFQHeader where RFQNumber=" + RFQNumber)
        Dim dt As DataTable = GetDataTable("select OutputStatus from RFQLine where RFQNumber='" + RFQNumber + "' and RFQLineNumber=" + RFQLineNumber)
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("OutputStatus").ToString = "True" Then
                ret = "Interface issued"
            End If
        End If
        Return ret
    End Function

End Class
Option Strict On
Option Explicit On
Option Infer Off
Imports Purchase.Common

Partial Public Class SearchResult
    Inherits System.Web.UI.UserControl

    ''' <summary>
    ''' 一覧の ClientID　プロパティ
    ''' </summary>
    Public ReadOnly Property ListClientID() As String
        Get
            Dim tbl As HtmlTable = DirectCast(ListSearchResult.FindControl("itemPlaceholderContainer"), HtmlTable)
            If tbl IsNot Nothing Then
                Return tbl.ClientID
            Else
                Return String.Empty
            End If
        End Get
    End Property

    ''' <summary>
    ''' ヘッダCSS　プロパティ
    ''' </summary>
    Public Property CssClass() As String
        Get
            Dim st_CSS As String = String.Empty
            If ViewState("CssClass") IsNot Nothing Then
                st_CSS = ViewState("CssClass").ToString()
            End If
            Return st_CSS
        End Get
        Set(ByVal value As String)
            ViewState("CssClass") = value
        End Set
    End Property

    ''' <summary>
    ''' 行クリックで更新ページに遷移するか　プロパティ（デフォルト:False=遷移しない）
    ''' </summary>
    Public Property EnableSelectRow() As Boolean
        Get
            Dim b_Visible As Boolean = False
            If ViewState("EnableSelectRow") IsNot Nothing Then
                b_Visible = DirectCast(ViewState("EnableSelectRow"), Boolean)
            End If
            Return b_Visible
        End Get
        Set(ByVal value As Boolean)
            ViewState("EnableSelectRow") = value
        End Set
    End Property

    ''' <summary>
    ''' ページャー(上部)を表示するか　プロパティ（デフォルト:True=表示する）
    ''' </summary>
    Public Property PagerTopVisible() As Boolean
        Get
            Dim b_Visible As Boolean = True
            If ViewState("PagerTopVisible") IsNot Nothing Then
                b_Visible = DirectCast(ViewState("PagerTopVisible"), Boolean)
            End If
            Return b_Visible
        End Get
        Set(ByVal value As Boolean)
            ViewState("PagerTopVisible") = value
        End Set
    End Property

    ''' <summary>
    ''' ページャー(下部)を表示するか　プロパティ（デフォルト:True=表示する）
    ''' </summary>
    Public Property PagerBottomVisible() As Boolean
        Get
            Dim b_Visible As Boolean = True
            If ViewState("PagerBottomVisible") IsNot Nothing Then
                b_Visible = DirectCast(ViewState("PagerBottomVisible"), Boolean)
            End If
            Return b_Visible
        End Get
        Set(ByVal value As Boolean)
            ViewState("PagerBottomVisible") = value
        End Set
    End Property

    ''' <summary>
    ''' ページサイズ　プロパティ（デフォルト:20）
    ''' </summary>
    Public Property PageSize() As Integer
        Get
            Dim i_PageSize As Integer = 20
            If ViewState("PageSize") IsNot Nothing Then
                i_PageSize = DirectCast(ViewState("PageSize"), Integer)
                If i_PageSize = 0 Then i_PageSize = 20
            End If
            Return i_PageSize
        End Get
        Set(ByVal value As Integer)
            ViewState("PageSize") = value
        End Set
    End Property

    ''' <summary>
    ''' ページブロックサイズ　プロパティ（デフォルト:10）
    ''' </summary>
    Public Property PageInBlock() As Integer
        Get
            Dim i_PageInBlock As Integer = 10
            If ViewState("PageInBlock") IsNot Nothing Then
                i_PageInBlock = DirectCast(ViewState("PageInBlock"), Integer)
                If i_PageInBlock = 0 Then i_PageInBlock = 10
            End If
            Return i_PageInBlock
        End Get
        Set(ByVal value As Integer)
            ViewState("PageInBlock") = value
        End Set
    End Property

    ''' <summary>
    ''' カレントページインデックス　プロパティ（デフォルト:0）
    ''' </summary>
    Public Property CurrentPageIndex() As Integer
        Get
            Dim i_CurrentPageIndex As Integer = 0
            If ViewState("CurrentPageIndex") IsNot Nothing Then
                i_CurrentPageIndex = DirectCast(ViewState("CurrentPageIndex"), Integer)
            End If
            Return i_CurrentPageIndex
        End Get
        Set(ByVal value As Integer)
            ViewState("CurrentPageIndex") = value
        End Set
    End Property

    ''' <summary>
    ''' 構造式検索スコアリスト (Key = RegistryNumber, Value = Score)
    ''' </summary>
    Public Property StructureScoreList() As Dictionary(Of String, String)
        Get
            If ViewState("StructureScoreList") IsNot Nothing Then
                Return DirectCast(ViewState("StructureScoreList"), Dictionary(Of String, String))
            Else
                Return New Dictionary(Of String, String)
            End If
        End Get
        Set(ByVal value As Dictionary(Of String, String))
            ViewState("StructureScoreList") = value
        End Set
    End Property

    ''' <summary>
    ''' ページングイベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e">e.CommandName = クリックされたリンクの種類 ("Prev" or "Next" or ページ番号) </param>
    ''' <remarks>
    '''   ex.) Protected Sub SearchResultList_Paging(ByVal sender As System.Object, ByVal e As PagingEventArgs) Handles SearchResultList.Paging
    ''' </remarks>
    Public Event Paging(ByVal sender As System.Object, ByVal e As PagingEventArgs)

    ''' <summary>
    ''' ページロード処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    ''' <summary>
    ''' Prev/Next リンク選択イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Pager_Command(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.CommandEventArgs) _
                                Handles PagerTopPrev.Command, PagerTopNext.Command, PagerBottomPrev.Command, PagerBottomNext.Command
        Dim i_NewCurrentPageIndex As Integer = Me.CurrentPageIndex
        If e.CommandName.Equals("Prev") Then       '前ページへ
            If CInt(Math.Truncate(i_NewCurrentPageIndex / PageInBlock)) > 0 Then
                i_NewCurrentPageIndex = CInt(Math.Truncate(i_NewCurrentPageIndex / PageInBlock)) * PageInBlock - 1
            End If
        ElseIf e.CommandName.Equals("Next") Then   '次ページへ
            i_NewCurrentPageIndex = (CInt(Math.Truncate(i_NewCurrentPageIndex / PageInBlock)) + 1) * PageInBlock
        End If
        Dim i_NewSkipRecord As Integer = Me.PageSize * i_NewCurrentPageIndex

        Dim ev As New PagingEventArgs(e.CommandName, i_NewCurrentPageIndex, i_NewSkipRecord)
        RaiseEvent Paging(sender, ev)
    End Sub

    ''' <summary>
    ''' ページ番号ページャー選択イベント
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub PagerNumber_ItemCommand(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewCommandEventArgs) _
                                Handles PagerTopNumber.ItemCommand, PagerBottomNumber.ItemCommand
        Dim i_NewCurrentPageIndex As Integer = Convert.ToInt32(e.CommandName) - 1
        Dim i_NewSkipRecord As Integer = Me.PageSize * i_NewCurrentPageIndex

        Dim ev As New PagingEventArgs(e.CommandName, i_NewCurrentPageIndex, i_NewSkipRecord)
        RaiseEvent Paging(sender, ev)
    End Sub

    ''' <summary>
    ''' 製品一覧 各行バインド後処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub ListSearchResult_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.ListViewItemEventArgs) Handles ListSearchResult.ItemDataBound
        Dim lv As ListView = CType(CType(e, ListViewItemEventArgs).Item.FindControl("RFQLineList"), ListView)
        Dim link As HyperLink = CType(CType(e, System.Web.UI.WebControls.ListViewItemEventArgs).Item.FindControl("lst_RFQNumber"), HyperLink)

        Dim RFQLineList As New TCIDataAccess.Join.RFQLineList()
        RFQLineList.Load(link.Text)
        lv.DataSource = RFQLineList
        lv.DataBind()

    End Sub

    ''' <summary>
    ''' 検索結果データバインド
    ''' </summary>
    ''' <param name="DataSource">データ（表示ページのみ）</param>
    ''' <param name="PrevPagerEnable">前ページリンク有効/無効</param>
    ''' <param name="NextPagerEnable">次ページリンク有効/無効</param>
    ''' <remarks></remarks>
    Public Sub SearchResultBind(ByVal DataSource As IList, ByVal PrevPagerEnable As Boolean, ByVal NextPagerEnable As Boolean)

        '------------------------------------------------------------
        ' 検索結果一覧設定
        '------------------------------------------------------------
        ListSearchResult.DataSource = DataSource
        ListSearchResult.DataBind()

        '------------------------------------------------------------
        ' ページャー設定
        '------------------------------------------------------------
        PagerTop.Visible = PagerTopVisible
        PagerBottom.Visible = PagerBottomVisible

        if PagerTopVisible Then
            '前へリンク
            PagerTopPrev.Enabled = PrevPagerEnable
            PagerTopPrev.Visible = PrevPagerEnable
            '次へリンク
            PagerTopNext.Enabled = NextPagerEnable
            PagerTopNext.Visible = NextPagerEnable
        End If

        If PagerBottomVisible Then
            '前へリンク
            PagerBottomPrev.Enabled = PrevPagerEnable
            PagerBottomPrev.Visible = PrevPagerEnable
            '次へリンク
            PagerBottomNext.Enabled = NextPagerEnable
            PagerBottomNext.Visible = NextPagerEnable
        End If


    End Sub
    ''' <summary>
    ''' 検索結果データバインド
    ''' </summary>
    ''' <param name="DataSource">データ（表示ページのみ）</param>
    ''' <param name="NewCurrentPageIndex">カレントページ</param>
    ''' <param name="TotalDataCount">データ件数 (検索にて Hit した件数)</param>
    ''' <remarks></remarks>
    Public Sub SearchResultBind(ByVal DataSource As IList, ByVal NewCurrentPageIndex As Integer, ByVal TotalDataCount As Integer)

        'カレントページをセット
        Me.CurrentPageIndex = NewCurrentPageIndex

        '総ページ数算出
        Dim i_MaxPageCount As Integer = Convert.ToInt32(Math.Ceiling(TotalDataCount / Convert.ToDouble(Me.PageSize)))

        Dim PrevPagerEnable As Boolean = False
        Dim NextPagerEnable As Boolean = False

        If i_MaxPageCount > 0 Then
            Dim pageList As New ListItemCollection
            'カレントページを含むページブロック数分のリンク設定
            Dim i_StartPage As Integer = Convert.ToInt32(Math.Floor(Me.CurrentPageIndex / Convert.ToDouble(PageInBlock))) * PageInBlock + 1

            For i As Integer = i_StartPage To Math.Min(i_StartPage + PageInBlock - 1, i_MaxPageCount)
                pageList.Add(New ListItem(i.ToString(), i.ToString(), (Not i.Equals(Me.CurrentPageIndex + 1))))
            Next

            Me.PagerTopVisible = True
            Me.PagerBottomVisible = True

            PagerTopNumber.DataSource = pageList
            PagerTopNumber.DataBind()
            PagerBottomNumber.DataSource = pageList
            PagerBottomNumber.DataBind()

            ' Page設定
            CurrentPageLabelTop.Text = IIf(TotalDataCount > 0, CurrentPageIndex + 1, 0).ToString
            CurrentPageLabelBottom.Text = CurrentPageLabelTop.Text 
            TotalPagesLabelTop.Text = Math.Ceiling(System.Convert.ToDouble(TotalDataCount) / PageSize).ToString
            TotalPagesLabelBottom.Text = TotalPagesLabelTop.Text
            TotalItemsLabelTop.Text = TotalDataCount.ToString
            TotalItemsLabelBottom.Text = TotalItemsLabelTop.Text

            '前へリンク
            PrevPagerEnable = ((CurrentPageIndex + 1) > PageInBlock)
            '次へリンク
            NextPagerEnable = Math.Truncate(i_MaxPageCount / PageInBlock) * PageInBlock > ( Me.CurrentPageIndex + 1)
        Else
            Me.PagerTopVisible = False
            Me.PagerBottomVisible = False

        End If
        
        SearchResultBind(DataSource, PrevPagerEnable, NextPagerEnable)
    End Sub

End Class


''' <summary>
''' ページングイベント引数
''' </summary>
''' <remarks></remarks>
Public Class PagingEventArgs
    Private _CommandName As String
    Private _NewCurrentPageIndex As Integer
    Private _NewSkipRecord As Integer

    ''' <summary>
    ''' コマンド
    ''' </summary>
    Public ReadOnly Property CommandName() As String
        Get
            Return _CommandName
        End Get
    End Property

    ''' <summary>
    ''' 次に表示するページ番号
    ''' </summary>
    Public ReadOnly Property NewCurrentPageIndex() As Integer
        Get
            Return _NewCurrentPageIndex
        End Get
    End Property

    ''' <summary>
    ''' 次に実行する読み飛ばしレコード数
    ''' </summary>
    Public ReadOnly Property NewSkipRecord() As Integer
        Get
            Return _NewSkipRecord
        End Get
    End Property

    ''' <summary>
    ''' コンストラクタ
    ''' </summary>
    ''' <param name="CommandName">コマンド</param>
    ''' <param name="NewCurrentPageIndex">次に表示するページ番号</param>
    ''' <param name="NewSkipRecord">次に実行する読み飛ばしレコード数</param>
    ''' <remarks></remarks>
    Public Sub New(ByVal CommandName As String, ByVal NewCurrentPageIndex As Integer, ByVal NewSkipRecord As Integer)
        _CommandName = CommandName
        _NewCurrentPageIndex = NewCurrentPageIndex
        _NewSkipRecord = NewSkipRecord
    End Sub

End Class
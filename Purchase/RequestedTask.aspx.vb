Option Explicit On
Option Strict On
Option Infer Off

Imports Purchase.Common

Partial Public Class RequestedTask
    Inherits CommonPage

    Protected st_Action As String = String.Empty ' aspx 側で読むため、Protected にする
    Protected lst_RequestedTask As List(Of TCIDataAccess.Join.RequestedTaskDisp)

    Const SWITCH_ACTION As String = "Switch"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Msg.Text = ""

        If Not IsPostBack Then
            ' RFQPriorityドロップダウンリスト設定
            SetPriorityDropDownList(RFQPriority, PRIORITY_FOR_SEARCH)
            RFQPriority.SelectedValue = PRIORITY_ALL

            ' RFQStatusドロップダウンリスト設定
            SetRFQStatusDropDownList(RFQStatus)
            RFQStatus.SelectedValue = RFQSTATUS_ALL

            ' Orderbyドロップダウンリスト設定
            SetRFQOrderByDropDownList(Orderby)
            Orderby.SelectedValue = "REM"

            ' 一覧取得（初期表示）
            SetPageSize()

            ShowList()
            RFQList.DataSource = lst_RequestedTask
            RFQList.DataBind()

        End If

    End Sub
    ''' <summary>
    ''' Switchボタン押下時処理  
    ''' </summary>
    Protected Sub Switch_Click() Handles Switch.Click
        Msg.Text = String.Empty
        RFQList.Visible = False

        ' パラメータ取得
        If String.IsNullOrEmpty(Request.Form("Action")) Then
            st_Action = Request.QueryString("Action")
        Else
            st_Action = Request.Form("Action")
        End If

        ' Action チェック
        If IsPostBack And ((String.IsNullOrEmpty(st_Action)) Or st_Action <> SWITCH_ACTION) Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            st_Action = ""
            Exit Sub
        End If

        ' 一覧取得（Switch押下時）
        SetPageSize()
        ReSetPager()

        ShowList()
        RFQList.DataSource = lst_RequestedTask
        RFQList.DataBind()

        RFQList.Visible = True

        Action.Value = String.Empty

    End Sub

    ''' <summary>
    ''' RFQList プロパティ変更時イベントハンドラ
    ''' </summary>
    ''' <remarks>
    ''' 
    ''' </remarks>
    Protected Sub RFQList_PagePropertiesChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RFQList.PagePropertiesChanged
        Msg.Text = String.Empty
        RFQList.Visible = False

        SetPageSize()

        ' 一覧を表示する（ページャー押下時）
        if IsPostBack And Not Action.Value.Equals(SWITCH_ACTION) Then
            ShowList()
            RFQList.DataSource = lst_RequestedTask
            RFQList.DataBind()
        End If

        RFQList.Visible = True

    End Sub

    ''' <summary>
    ''' 検索結果一覧を表示  
    ''' </summary>
    Protected Sub ShowList()
        ' RFQ データ取得用 SQLDataSource の設定
        Dim dc_RequestedTaskList As New TCIDataAccess.Join.RequestedTaskDispList
        RFQList.DataSource = Nothing

        dc_RequestedTaskList.Load(Cint(Session("UserID").ToString), RFQPriority.SelectedValue, RFQStatus.SelectedValue, 
                                  Orderby.SelectedValue, Session(SESSION_ROLE_CODE).ToString)

        lst_RequestedTask = dc_RequestedTaskList

    End Sub

    Private Sub SetPageSize()

        RFQPagerCountTop.PageSize = Common.LIST_ONEPAGE_ROW(Request.Url.ToString())
        RFQPagerLinkTop.PageSize = Common.LIST_ONEPAGE_ROW(Request.Url.ToString())
        RFQPagerLinkBottom.PageSize = Common.LIST_ONEPAGE_ROW(Request.Url.ToString())
        RFQPagerCountBottom.PageSize = Common.LIST_ONEPAGE_ROW(Request.Url.ToString())

    End Sub

    Private Sub ReSetPager()

        ResetPageNumericPagerField(RFQPagerLinkTop)
        ResetPageNumericPagerField(RFQPagerLinkBottom)

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

End Class
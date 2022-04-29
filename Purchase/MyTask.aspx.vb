Option Explicit On
Option Strict On
Option Infer Off

Imports Purchase.Common

Partial Public Class MyTask
    Inherits CommonPage

    Protected st_Action As String = String.Empty ' aspx 側で読むため、Protected にする
    Private st_UserID As String = String.Empty

    Const SWITCH_ACTION As String = "Switch"
    Const RFQ_PO_ACTION As String = "Cancel"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Msg.Text = ""
        '' パラメータ UserID 取得
        If IsPostBack = True Then
            '' 選択された User を退避
            st_UserID = UserID.SelectedValue
        Else
            st_UserID = Session("UserID").ToString
        End If

        '' 初期表示時は呼び元から渡された UserID を格納
        If String.IsNullOrEmpty(st_UserID) Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        If String.IsNullOrEmpty(st_UserID) Then
            '' 呼び元もしくは自画面から UserID が取得出来ない場合はログインユーザをセッションから格納
            st_UserID = Session("UserID").ToString
        End If

        ' セッション変数 PrivilegeLevel が  'P' の場合は 
        ' 変数 st_UserID がログインユーザと同じ拠点かをチェックし、ビュー v_User からデータ取得。
        ' セッション変数 PrivilegeLevel が 'A' の場合は v_UserAll からデータ取得。
        Dim ds As DataSet = New DataSet
        ds.Tables.Add("UserID")

        SetPageSize()

        If Session("Purchase.PrivilegeLevel").ToString = "P" Then
            Using connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)

                ' 拠点チェック
                Dim vUser As New TCIDataAccess.v_User()
                Dim st_query As String = vUser.CreateUserCountSQL()
                Dim command As New SqlClient.SqlCommand(st_query, connection)
                command.Parameters.AddWithValue("UserID", st_UserID)
                command.Parameters.AddWithValue("LocationCode", Session("LocationCode"))
                connection.Open()

                Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()
                Dim b_hasrows As Boolean = reader.HasRows
                reader.Close()

                ' 同拠点ならばデータ取得
                If b_hasrows Then
                    st_query = vUser.CreateUserSelectSQL(Session("Purchase.PrivilegeLevel").ToString)
                    command.CommandText = st_query

                    Dim adapter As New SqlClient.SqlDataAdapter()
                    adapter.SelectCommand = command
                    adapter.Fill(ds.Tables("UserID"))

                    UserID.DataValueField = "UserID"
                    UserID.DataTextField = "Name"
                    UserID.DataSource = ds.Tables("UserID")
                    UserID.DataBind()
                End If
            End Using
        ElseIf Session("Purchase.PrivilegeLevel").ToString = "A" Then
            Using connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
                Dim vUser As New TCIDataAccess.v_User()
                Dim st_query As String = vUser.CreateUserSelectSQL(Session("Purchase.PrivilegeLevel").ToString)
                Dim adapter As New SqlClient.SqlDataAdapter()
                Dim command As New SqlClient.SqlCommand(st_query, connection)
                adapter.SelectCommand = command
                adapter.Fill(ds.Tables("UserID"))

                UserID.DataValueField = "UserID"
                UserID.DataTextField = "Name"
                UserID.DataSource = ds.Tables("UserID")
                UserID.DataBind()
                UserID.SelectedValue = st_UserID
            End Using
        End If

        If Not IsPostBack Then
            'RFQPriorityドロップダウンリスト設定
            SetPriorityDropDownList(RFQPriority, PRIORITY_FOR_SEARCH)
            RFQPriority.SelectedValue = PRIORITY_ALL

            'RFQStatusドロップダウンリスト設定
            SetRFQStatusDropDownList(RFQStatus, RFQSTATUS_ALL)
            RFQStatus.SelectedValue = PRIORITY_ALL

            'Orderbyドロップダウンリスト設定
            SetRFQOrderByDropDownList(Orderby)
            Orderby.SelectedValue = "REM"

            ' 一覧初期表示
            ShowList()
        End If

    End Sub

    ''' <summary>
    ''' Switchボタン押下時処理  
    ''' </summary>
    Protected Sub Switch_Click() Handles Switch.Click
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

        ' 一覧を表示する（Switchボタン押下）
        ShowList()

    End Sub

    ''' <summary>
    ''' RFQList プロパティ変更時イベントハンドラ
    ''' </summary>
    ''' <remarks>
    ''' 
    ''' </remarks>
    Protected Sub RFQList_PagePropertiesChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RFQList.PagePropertiesChanged
        ' 一覧を表示する（初期表示、ページャー押下時）
        if IsPostBack Then
            ShowList()
        End If
        SetPageSize()
    End Sub

    Protected Sub SrcRFQ_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcRFQ.Selecting
        e.Command.CommandTimeout = 0
    End Sub

    Protected Sub ShowList()
        ' 前回の SQL パラメータを削除
        SrcRFQ.SelectParameters.Clear()
        SrcPO_Overdue.SelectParameters.Clear()
        SrcPO_PPI.SelectParameters.Clear()
        SrcPO_Par.SelectParameters.Clear()

        ' SQL パラメータ設定
        SrcRFQ.SelectParameters.Add("UserID", st_UserID)
        SrcPO_Overdue.SelectParameters.Add("UserID", st_UserID)
        SrcPO_PPI.SelectParameters.Add("UserID", st_UserID)
        SrcPO_Par.SelectParameters.Add("UserID", st_UserID)

        ' RFQ データ取得用 SQLDataSource の設定
        Dim dc_MyTaskList As New TCIDataAccess.Join.MyTaskDispList
        RFQList.DataSource = Nothing 
        dc_MyTaskList.Load(Cint(st_UserID), RFQPriority.SelectedValue, RFQStatus.SelectedValue, Orderby.SelectedValue, Session(SESSION_ROLE_CODE).ToString)
        RFQList.DataSource = dc_MyTaskList
        RFQList.DataBind()

        If dc_MyTaskList.Count > 0 Then
            '' 一覧の取得件数が0以上なら以下の処理を実行
            If Not HiddenUserID.Value.Equals(st_UserID) Or 
                    Not HiddenRFQPriority.Value.Equals(RFQPriority.SelectedValue) Or 
                    Not HiddenRFQStatus.Value.Equals(RFQStatus.SelectedValue) Or 
                    Not HiddenOrderby.Value.Equals(Orderby.SelectedValue) Then
                '' 条件変更時はページャーをリセット
                ReSetPager
            Else 
                ''ページング遷移時は何もしない
            End If
        End If

        '' 検索条件を退避
        HiddenUserID.Value = st_UserID
        HiddenRFQPriority.Value = RFQPriority.SelectedValue
        HiddenRFQStatus.Value = RFQStatus.SelectedValue
        HiddenOrderby.Value = Orderby.SelectedValue

    End Sub

    ' ユーザ選択プルダウンを前回選択したユーザに設定する
    Private Sub SetCtrl_UserIDSelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserID.DataBound
        Dim ddl As DropDownList = CType(sender, DropDownList)

        For Each item As ListItem In ddl.Items
            If item.Value = st_UserID Then
                ddl.SelectedValue = item.Value
                Exit For
            End If
        Next

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

    Protected Sub RFQCancelAssign_Click(ByVal source As Object, ByVal e As ListViewCommandEventArgs) Handles RFQList.ItemCommand
        ' Action チェック
        st_Action = Request.QueryString("Action")
        If st_Action <> RFQ_PO_ACTION Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            st_Action = ""
            Exit Sub
        End If

        '[SQL接続定義作成]--------------------------------------------------------------
        Dim connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
        Dim command As SqlClient.SqlCommand

        '[選択されたRFQNumberの取得]----------------------------------------------------
        Dim st_RFQNumber As String = CType(e.Item.FindControl("RFQNumber"), Label).Text

        '[QuoUserID=Nullにする]---------------------------------------------------------
        Dim sb_SQL As New Text.StringBuilder
        sb_SQL.Append("UPDATE ")
        sb_SQL.Append("  RFQHeader ")
        sb_SQL.Append("SET ")
        sb_SQL.Append("  QuoUserID=Null,RFQStatusCode = 'N' ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  RFQNumber= " & st_RFQNumber)
        command = connection.CreateCommand
        command.CommandText = sb_SQL.ToString
        connection.Open()
        command.ExecuteNonQuery()
        connection.Close()

        '[RFQList再表示]----------------------------------------------------------------
        ShowList()
    End Sub

    Protected Sub SetRFQCancelAssign(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles RFQList.ItemDataBound
        '[RFQCancelAssignの表示、Action設定]--------------------------------------------
        If DirectCast(e.Item.FindControl("StatusCode"), HiddenField).Value = "A" Then
            e.Item.FindControl("RFQCancelAssign").Visible = True
            CType(e.Item.FindControl("RFQCancelAssign"), Button).PostBackUrl = "MyTask.aspx?Action=" & RFQ_PO_ACTION
        End If
    End Sub

    Protected Sub SrcPO_Overdue_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcPO_Overdue.Selecting
        e.Command.CommandTimeout = 0
    End Sub

    Protected Sub SrcPO_PPI_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcPO_PPI.Selecting
        e.Command.CommandTimeout = 0
    End Sub

    Protected Sub SrcPO_Par_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcPO_Par.Selecting
        e.Command.CommandTimeout = 0
    End Sub

End Class
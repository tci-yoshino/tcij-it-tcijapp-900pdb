Imports Purchase.Common

Partial Public Class RequestedTask
    Inherits CommonPage
    Protected st_Action As String = String.Empty ' aspx 側で読むため、Protected にする
    Private st_UserID As String = String.Empty
    Private stb_PONumbers As StringBuilder = New StringBuilder ' PONumber を格納するオブジェクト。この値を見て、重複するPONumber を除外する。

    Dim RequestedTaskDate = New TCIDataAccess.Join.RequestedTaskDispList


    Const SWITCH_ACTION As String = "Switch"
    Const RFQ_PO_ACTION As String = "Cancel"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ' パラメータ UserID 取得
        If Request.RequestType = "POST" Then
            st_UserID = IIf(Request.Form("UserID") = Nothing, "", Request.Form("UserID"))
        ElseIf Request.RequestType = "GET" Then
            st_UserID = IIf(Request.QueryString("UserID") = Nothing, "", Request.QueryString("UserID"))
        End If

        If String.IsNullOrEmpty(st_UserID) Then
            st_UserID = Session("UserID")
        End If

        ' セッション変数 PrivilegeLevel が  'P' の場合は 
        ' 変数 st_UserID がログインユーザと同じ拠点かをチェックし、ビュー v_User からデータ取得。
        ' セッション変数 PrivilegeLevel が 'A' の場合は v_UserAll からデータ取得。
        Dim ds As DataSet = New DataSet
        ds.Tables.Add("UserID")

        If Session("Purchase.PrivilegeLevel") = "P" Then
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
                    st_query = vUser.CreateUserSelectSQL(Session("Purchase.PrivilegeLevel"))
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
        ElseIf Session("Purchase.PrivilegeLevel") = "A" Then
            Using connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
                Dim vUser As New TCIDataAccess.v_User()
                Dim st_query As String = vUser.CreateUserSelectSQL(Session("Purchase.PrivilegeLevel"))
                Dim adapter As New SqlClient.SqlDataAdapter()
                Dim command As New SqlClient.SqlCommand(st_query, connection)
                adapter.SelectCommand = command
                adapter.Fill(ds.Tables("UserID"))

                UserID.DataValueField = "UserID"
                UserID.DataTextField = "Name"
                UserID.DataSource = ds.Tables("UserID")
                UserID.DataBind()
            End Using
        End If

        If Not IsPostBack Then
            'RFQPriorityドロップダウンリスト設定
            SetPriorityDropDownList(RFQPriority, PRIORITY_FOR_SEARCH)
            RFQPriority.SelectedValue = PRIORITY_ALL

            'RFQStatusドロップダウンリスト設定
            Dim dc_RFQStatusList As New TCIDataAccess.RFQStatusList()
            dc_RFQStatusList.SetRFQStatusDropDownList(RFQStatus,RFQSTATUS_ALL)
            RFQStatus.SelectedValue = PRIORITY_ALL

            'Orderbyドロップダウンリスト設定
            SetRFQOrderByDropDownList(Orderby)
            ShowList()
        End If
    End Sub
    ''' <summary>
    ''' Switchボタン押下時処理  
    ''' </summary>
    Protected Sub Switch_Click()
        ShowList()
    End Sub

    Protected Sub SrcRFQ_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcRFQ.Selecting
        e.Command.CommandTimeout = 0
    End Sub

    ''' <summary>
    ''' 検索結果一覧を表示  
    ''' </summary>
    Protected Sub ShowList()
        Dim dc_RequestedTaskList As New TCIDataAccess.Join.RequestedTaskDispList
        dc_RequestedTaskList.Load(st_UserID, RFQPriority.SelectedValue, RFQStatus.SelectedValue, Orderby.SelectedValue, Session(SESSION_ROLE_CODE).ToString)
        RequestedTaskDate = dc_RequestedTaskList
        RFQList.DataSource = RequestedTaskDate
        RFQList.DataBind()
    End Sub
End Class
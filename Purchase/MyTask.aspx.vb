Public Partial Class MyTask
    Inherits CommonPage

    Protected st_Action As String = String.Empty ' aspx 側で読むため、Protected にする
    Private st_UserID As String = String.Empty
    Private stb_PONumbers As StringBuilder = New StringBuilder ' PONumber を格納するオブジェクト。この値を見て、重複するPONumber を除外する。

    Const SWITCH_ACTION As String = "Switch"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Msg.Text = ""

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
                Dim st_query As String = CreateUserCheckSQL()
                Dim command As New SqlClient.SqlCommand(st_query, connection)
                command.Parameters.AddWithValue("UserID", st_UserID)
                command.Parameters.AddWithValue("LocationCode", Session("LocationCode"))
                connection.Open()

                Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()
                Dim b_hasrows As Boolean = reader.HasRows
                reader.Close()

                ' 同拠点ならばデータ取得
                If b_hasrows Then
                    st_query = CreateUserSelectSQL(Session("Purchase.PrivilegeLevel"))
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
                Dim st_query As String = CreateUserSelectSQL(Session("Purchase.PrivilegeLevel"))
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
            Switch_Click()
        End If

    End Sub

    Protected Sub Switch_Click()

        ' パラメータ取得
        If Request.RequestType = "POST" Then
            st_Action = IIf(String.IsNullOrEmpty(Request.Form("Action")), "", Request.Form("Action"))
        ElseIf Request.RequestType = "GET" Then
            st_Action = IIf(String.IsNullOrEmpty(Request.QueryString("Action")), "", Request.QueryString("Action"))
        End If

        ' Action チェック
        If IsPostBack And ((String.IsNullOrEmpty(st_Action)) Or st_Action <> SWITCH_ACTION) Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            st_Action = ""
            Exit Sub
        End If

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
        SrcRFQ.SelectCommand = CreateRFQSelectSQL()
        RFQList.DataSourceID = "SrcRFQ"

        ' PO データ取得
        ' PO はクエリが複雑なため、別関数(GetPOList)で SqlDataAdapter を使用してデータ取得している。
        Dim dt_PO As New DataTable
        dt_PO = GetPOList(dt_PO, st_UserID)

        Dim dv_POOverDue As DataView = New DataView(dt_PO, "TaskType = 'OverDue'", "DueDate", DataViewRowState.CurrentRows)
        POList_Overdue.DataSource = dv_POOverDue
        POList_Overdue.DataBind()

        Dim dv_POPPI As DataView = New DataView(dt_PO, "TaskType = 'PPI'", "", DataViewRowState.CurrentRows)
        POList_PPI.DataSource = dv_POPPI
        POList_PPI.DataBind()

        Dim dv_POReminder As DataView = New DataView(dt_PO, "TaskType = 'Reminder'", "StatusSortOrder", DataViewRowState.CurrentRows)
        POList_Par.DataSource = dv_POReminder
        POList_Par.DataBind()

    End Sub

    ' ユーザ選択プルダウンを前回選択したユーザに設定する
    Private Sub SetCtrl_UserIDSelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserID.DataBound
        Dim ddl As DropDownList = sender

        For Each item As ListItem In ddl.Items
            If item.Value = st_UserID Then
                ddl.SelectedValue = item.Value
            End If
        Next

    End Sub

    ' POリスト取得
    Private Function GetPOList(ByVal ds As DataTable, ByVal st_UserID As String) As DataTable
        Using connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)

            Dim st_query As String = CreatePOSelectSQL()
            Dim command As New SqlClient.SqlCommand(st_query, connection)

            command.Parameters.AddWithValue("UserID", st_UserID)
            connection.Open()
            command.CommandText = st_query
            Dim adapter As New SqlClient.SqlDataAdapter()

            ' データベースからデータを取得
            adapter.SelectCommand = command
            adapter.Fill(ds)

            Return ds
        End Using
    End Function

    ' POList_Par の項目バインド時にその項目の子データがあった場合は取得する
    Protected Sub SetChildPO(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles POList_Par.ItemDataBound

        Dim lv As ListView = CType(e.Item.FindControl("POList_Chi"), ListView)
        Dim src As SqlDataSource = CType(e.Item.FindControl("SrcPO_Chi"), SqlDataSource)
        Dim label As Label = CType(e.Item.FindControl("PONumber"), Label)

        src.SelectParameters.Clear()
        src.SelectParameters.Add("PONumber", label.Text)
        src.SelectCommand = CreatePOChildSelectSQL()
        lv.DataSourceID = src.ID
        lv.DataBind()
    End Sub

    Protected Sub SrcRFQ_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcRFQ.Selecting
        e.Command.CommandTimeout = 0
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

    Private Function CreateUserCheckSQL() As String
        Dim sb_SQL As New Text.StringBuilder
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("  count(UserID) as count ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("  v_User ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  LocationCode = @LocationCode ")
        sb_SQL.Append("  AND UserID = @UserID ")
        Return sb_SQL.ToString()
    End Function

    Private Function CreateUserSelectSQL(ByVal PrivilegeLevel As String) As String
        Dim sb_SQL As New Text.StringBuilder
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("  UserID, ")
        sb_SQL.Append("  [Name] ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("  v_User ")
        If PrivilegeLevel = "P" Then
            sb_SQL.Append("WHERE ")
            sb_SQL.Append("  isDisabled = 0 ")
            sb_SQL.Append("  AND LocationCode = @LocationCode ")
        End If
        sb_SQL.Append("ORDER BY ")
        sb_SQL.Append("  [Name] ASC  ")

        Return sb_SQL.ToString()
    End Function

    Private Function CreateRFQSelectSQL() As String
        Dim sb_SQL As New Text.StringBuilder
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("  RH.RFQNumber, ")
        sb_SQL.Append("  RH.StatusChangeDate, ")
        sb_SQL.Append("  RH.Status, ")
        sb_SQL.Append("  RH.ProductNumber, ")
        sb_SQL.Append("  RH.ProductName AS ProductName, ")
        sb_SQL.Append("  RH.Purpose, ")
        sb_SQL.Append("  RH.QuoUserName, ")
        sb_SQL.Append("  RH.QuoLocationName, ")
        sb_SQL.Append("  RH.EnqUserName, ")
        sb_SQL.Append("  RH.EnqLocationName, ")
        sb_SQL.Append("  RH.SupplierName, ")
        sb_SQL.Append("  RH.MakerName, ")
        sb_SQL.Append("  RR.RFQCorres AS RFQCorrespondence ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("  v_RFQHeader AS RH ")
        sb_SQL.Append("    LEFT OUTER JOIN v_RFQReminder AS RR ON RH.RFQNumber = RR.RFQNumber AND @UserID = RR.RcptUserID ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  QuoUserID = @UserID ")
        sb_SQL.Append("  AND EnqUserID != @UserID ")
        sb_SQL.Append("  AND NOT (RH.StatusCode = 'Q' AND RR.RFQHistoryNumber IS NULL) ")
        sb_SQL.Append("  AND NOT (RH.StatusCode = 'C' AND RR.RFQHistoryNumber IS NULL) ")
        sb_SQL.Append("ORDER BY ")
        sb_SQL.Append("  StatusSortOrder, StatusChangeDate ASC ")
        Return sb_SQL.ToString()
    End Function

    Private Function CreatePOSelectSQL() As String
        Dim sb_SQL As New Text.StringBuilder
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("  P.PONumber, ")
        sb_SQL.Append("  P.StatusSortOrder, ")
        sb_SQL.Append("  P.StatusChangeDate, ")
        sb_SQL.Append("  P.StatusCode, ")
        sb_SQL.Append("  P.ProductNumber, ")
        sb_SQL.Append("  P.ProductName, ")
        sb_SQL.Append("  P.PODate, ")
        sb_SQL.Append("  P.POUserName, ")
        sb_SQL.Append("  P.POLocationName, ")
        sb_SQL.Append("  P.SupplierName, ")
        sb_SQL.Append("  P.MakerName, ")
        sb_SQL.Append("  P.DeliveryDate, ")
        sb_SQL.Append("  P.OrderQuantity, ")
        sb_SQL.Append("  P.OrderUnitCode, ")
        sb_SQL.Append("  P.CurrencyCode, ")
        sb_SQL.Append("  P.UnitPrice, ")
        sb_SQL.Append("  P.PerQuantity, ")
        sb_SQL.Append("  P.PerUnitCode, ")
        sb_SQL.Append("  P.DueDate, ")
        sb_SQL.Append("  PR.POCorres as POCorrespondence, ")
        sb_SQL.Append("  CASE ")
        sb_SQL.Append("    WHEN ")
        sb_SQL.Append("      P.POUserID = @UserID ")
        sb_SQL.Append("      AND P.DueDate <= GETDATE() ")
        sb_SQL.Append("      AND ((P.ParPONumber IS NULL) AND (P.StatusSortOrder <= 11) ")
        sb_SQL.Append("        OR (P.ParPONumber IS NOT NULL) AND (P.StatusCode = 'CPI')) ")
        sb_SQL.Append("    THEN ")
        sb_SQL.Append("      'Overdue' ")
        sb_SQL.Append("    WHEN ")
        sb_SQL.Append("      P.SOUserID = @UserID ")
        sb_SQL.Append("      AND P.StatusCode = 'PPI' ")
        sb_SQL.Append("    THEN ")
        sb_SQL.Append("      'PPI' ")
        sb_SQL.Append("    WHEN ")
        sb_SQL.Append("      P.ParPONumber IS NULL ")
        sb_SQL.Append("      AND PR.POHistoryNumber IS NOT NULL ")
        sb_SQL.Append("    THEN ")
        sb_SQL.Append("      'Reminder' ")
        sb_SQL.Append("    END AS TaskType ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("  v_PO AS P ")
        sb_SQL.Append("    LEFT OUTER JOIN v_POReminder AS PR ON PR.PONumber = P.PONumber AND PR.RcptUserID = @UserID ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  (POUserID = @UserID OR SOUserID = @UserID) ")
        Return sb_SQL.ToString()
    End Function

    Private Function CreatePOChildSelectSQL() As String
        Dim sb_SQL As New Text.StringBuilder
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("  P.PONumber, ")
        sb_SQL.Append("  P.ProductNumber, ")
        sb_SQL.Append("  P.ProductName, ")
        sb_SQL.Append("  P.PODate, ")
        sb_SQL.Append("  P.POUserName, ")
        sb_SQL.Append("  P.POLocationName, ")
        sb_SQL.Append("  P.SupplierName, ")
        sb_SQL.Append("  P.MakerName, ")
        sb_SQL.Append("  P.DeliveryDate, ")
        sb_SQL.Append("  P.OrderQuantity, ")
        sb_SQL.Append("  P.OrderUnitCode, ")
        sb_SQL.Append("  P.CurrencyCode, ")
        sb_SQL.Append("  P.UnitPrice, ")
        sb_SQL.Append("  P.PerQuantity, ")
        sb_SQL.Append("  P.PerUnitCode ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("  v_PO AS P ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  P.ParPONumber = @PONumber ")
        sb_SQL.Append("ORDER BY ")
        sb_SQL.Append("  P.StatusSortOrder ASC ")
        Return sb_SQL.ToString()
    End Function
End Class
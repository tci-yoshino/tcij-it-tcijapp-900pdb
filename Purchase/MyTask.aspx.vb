Public Partial Class MyTask
    Inherits CommonPage

    Protected st_Action As String = String.Empty ' aspx 側で読むため、Protected にする
    Private st_UserID As String = String.Empty
    Private stb_PONumbers As StringBuilder = New StringBuilder ' PONumber を格納するオブジェクト。この値を見て、重複するPONumber を除外する。

    Const SWITCH_ACTION As String = "Switch"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' コントロール初期化
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

        ' User 一覧取得
        Dim ds As DataSet = New DataSet
        ds.Tables.Add("UserID")

        If Session("Purchase.PrivilegeLevel") = "P" Then

            Using connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)

                Dim st_query As String = "SELECT count(UserID) as count FROM v_User WHERE LocationCode = @LocationCode AND UserID = @UserID"
                Dim command As New SqlClient.SqlCommand(st_query, connection)

                ' SQL SELECT パラメータの追加
                command.Parameters.AddWithValue("UserID", st_UserID)
                command.Parameters.AddWithValue("LocationCode", Session("LocationCode"))

                ' SqlDataReader を生成し、検索処理を実行。
                connection.Open()
                Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()

                ' 取得件数が 1 件以上の場合は True, 0 件の場合は False を取得。
                Dim b_hasrows As Boolean = reader.HasRows
                reader.Close()

                ' 取得件数が 1 件以上ある場合
                If b_hasrows Then

                    ' クエリ、コマンド、アダプタの生成
                    st_query = "SELECT UserID, [Name] FROM v_User WHERE isDisabled = 0 AND LocationCode = @LocationCode ORDER BY [Name] ASC "
                    command.CommandText = st_query
                    Dim adapter As New SqlClient.SqlDataAdapter()

                    ' データベースからデータを取得
                    adapter.SelectCommand = command
                    adapter.Fill(ds.Tables("UserID"))

                    ' User プルダウンにバインド
                    UserID.DataValueField = "UserID"
                    UserID.DataTextField = "Name"
                    UserID.DataSource = ds.Tables("UserID")
                    UserID.DataBind()
                End If

            End Using

        ElseIf Session("Purchase.PrivilegeLevel") = "A" Then
            Using connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
                ' クエリ、アダプタ、SQLコマンド オブジェクトの生成
                Dim st_query As String = "SELECT UserID, [Name] FROM v_User ORDER BY [Name] ASC "
                Dim adapter As New SqlClient.SqlDataAdapter()
                Dim command As New SqlClient.SqlCommand(st_query, connection)

                ' データベースからデータを取得
                adapter.SelectCommand = command
                adapter.Fill(ds.Tables("UserID"))

                ' User プルダウンにバインド
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

        ' 見積一覧データ取得、バインド
        SrcRFQ.SelectCommand = _
              "SELECT " _
            & "  RH.RFQNumber, RH.StatusChangeDate, RH.Status, RH.ProductNumber, " _
            & "  RH.ProductName AS ProductName, " _
            & "  RH.Purpose, RH.QuoUserName, RH.QuoLocationName, RH.EnqUserName, " _
            & "  RH.EnqLocationName, RH.SupplierName, RH.MakerName, " _
            & "  RR.RFQCorres AS RFQCorrespondence " _
            & "FROM " _
            & "  v_RFQHeader AS RH " _
            & "  LEFT OUTER JOIN v_RFQReminder AS RR " _
            & "    ON RH.RFQNumber = RR.RFQNumber AND @UserID = RR.RcptUserID " _
            & "WHERE " _
            & "  QuoUserID = @UserID " _
            & "  AND EnqUserID != @UserID " _
            & "  AND StatusCode NOT IN ('Q','C') " _
            & "  OR  (StatusCode IN ('Q','C') AND RR.RFQHistoryNumber IS NOT NULL) " _
            & "ORDER BY " _
            & "  StatusSortOrder, StatusChangeDate ASC  "
        RFQList.DataSourceID = "SrcRFQ"
        RFQList.DataBind()

        ' 購買発注一覧データ取得、バインド
        ' SrcPO_PPI の NOT IN () には、SrcPO_Overdue で取得した PONumber が入る。
        ' SrcPO_Par の NOT IN () には、SrcPO_Overdue, SrcPO_PPI で取得した PONumber が入る。
        ' 取得した PONumber が空の場合は '' が入る。(重複を避けるための処理)
        SrcPO_Overdue.SelectCommand = _
              "SELECT P.PONumber, P.StatusChangeDate, P.StatusCode, P.ProductNumber, P.ProductName, " _
            & "       P.PODate, P.POUserName, P.POLocationName, P.SupplierName, P.MakerName, P.DeliveryDate, " _
            & "       P.OrderQuantity, P.OrderUnitCode, P.CurrencyCode, P.UnitPrice, P.PerQuantity, P.PerUnitCode, PR.POCorres as POCorrespondence " _
            & "FROM v_PO AS P LEFT OUTER JOIN " _
            & "     v_POReminder AS PR ON PR.PONumber = P.PONumber AND PR.RcptUserID = @UserID " _
            & "WHERE POUserID = @UserID " _
            & "  AND CONVERT(VARCHAR,P.DueDate,112) <= CONVERT(VARCHAR,GETDATE(),112) " _
            & "  AND ((P.ParPONumber IS NULL) AND (P.StatusSortOrder <= 11) " _
            & "        OR (P.ParPONumber IS NOT NULL) AND (P.StatusCode = 'CPI')) " _
            & "ORDER BY DueDate ASC "
        POList_Overdue.DataSourceID = "SrcPO_Overdue"
        POList_Overdue.DataBind()

        SrcPO_PPI.SelectCommand = _
              "SELECT P.PONumber, P.StatusChangeDate, P.StatusCode, P.ProductNumber, P.ProductName, " _
            & "       P.PODate, P.POUserName, P.POLocationName, P.SupplierName, P.MakerName, P.DeliveryDate, " _
            & "       P.OrderQuantity, P.OrderUnitCode, P.CurrencyCode, P.UnitPrice, P.PerQuantity, P.PerUnitCode, PR.POCorres as POCorrespondence " _
            & "FROM v_PO AS P LEFT OUTER JOIN " _
            & "     v_POReminder AS PR ON PR.PONumber = P.PONumber AND PR.RcptUserID = @UserID " _
            & "WHERE P.SOUserID = @UserID " _
            & "  AND P.StatusCode = 'PPI' " _
            & "  AND P.PONumber NOT IN (" & IIf(String.IsNullOrEmpty(stb_PONumbers.ToString), "''", stb_PONumbers.ToString.Trim(",")) & ") "
        POList_PPI.DataSourceID = "SrcPO_PPI"
        POList_PPI.DataBind()

        SrcPO_Par.SelectCommand = _
              "SELECT P.PONumber, P.StatusChangeDate, P.StatusCode, P.ProductNumber, P.ProductName, " _
            & "       P.PODate, P.POUserName, P.POLocationName, P.SupplierName, P.MakerName, P.DeliveryDate, " _
            & "       P.OrderQuantity, P.OrderUnitCode, P.CurrencyCode, P.UnitPrice, P.PerQuantity, P.PerUnitCode, PR.POCorres as POCorrespondence " _
            & "FROM v_PO AS P INNER JOIN " _
            & "     v_POReminder AS PR ON PR.PONumber = P.PONumber AND PR.RcptUserID = @UserID " _
            & "WHERE ((P.SOUserID = @UserID) OR (P.POUserID = @UserID)) " _
            & "  AND P.ParPONumber IS NULL " _
            & "  AND P.PONumber NOT IN (" & IIf(String.IsNullOrEmpty(stb_PONumbers.ToString), "''", stb_PONumbers.ToString.Trim(",")) & ") " _
            & "ORDER BY P.StatusSortOrder ASC "
        POList_Par.DataSourceID = "SrcPO_Par"
        POList_Par.DataBind()

    End Sub

    ' POList_Overdue と POList_PPI のバインド終了時に PONumber を取得
    Protected Sub GetPONumberOverdue(ByVal sender As Object, ByVal e As EventArgs) Handles POList_Overdue.DataBound, POList_PPI.DataBound

        Dim lv As ListView = CType(sender, ListView)
        Dim label As Label = New Label

        If lv.Items.Count > 0 Then
            For i As Integer = 0 To lv.Items.Count - 1
                label.Text = CType(lv.Items(i).FindControl("PONumber"), Label).Text
                stb_PONumbers.Append(", " & label.Text)
            Next i
        End If

    End Sub

    ' POList_Par の項目バインド時にその項目の子データがあった場合は取得する
    Protected Sub SetChildPO(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles POList_Par.ItemDataBound

        Dim lv As ListView = CType(e.Item.FindControl("POList_Chi"), ListView)
        Dim src As SqlDataSource = CType(e.Item.FindControl("SrcPO_Chi"), SqlDataSource)
        Dim label As Label = CType(e.Item.FindControl("PONumber"), Label)

        src.SelectParameters.Clear()
        src.SelectParameters.Add("PONumber", label.Text)
        src.SelectCommand = _
              "SELECT P.PONumber, P.ProductNumber, P.ProductName, " _
            & "       P.PODate, P.POUserName, P.POLocationName, P.SupplierName, P.MakerName, P.DeliveryDate, " _
            & "       P.OrderQuantity, P.OrderUnitCode, P.CurrencyCode, P.UnitPrice, P.PerQuantity, P.PerUnitCode " _
            & "FROM v_PO AS P " _
            & "WHERE P.ParPONumber = @PONumber " _
            & "ORDER BY P.StatusSortOrder ASC "
        lv.DataSourceID = src.ID
        lv.DataBind()
    End Sub

    Private Sub SetCtrl_UserIDSelected(ByVal sender As Object, ByVal e As System.EventArgs) Handles UserID.DataBound
        Dim ddl As DropDownList = sender

        For Each item As ListItem In ddl.Items
            If item.Value = st_UserID Then
                ddl.SelectedValue = item.Value
            End If
        Next

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


End Class
Public Partial Class MyTask
    Inherits CommonPage

    Public st_UserID As String
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' パラメータ UserID 取得
        If Request.RequestType = "POST" Then
            st_UserID = IIf(Request.Form("UserID") = Nothing, "", Request.Form("UserID"))
        ElseIf Request.RequestType = "GET" Then
            st_UserID = IIf(Request.QueryString("UserID") = Nothing, "", Request.QueryString("UserID"))
        End If

        If String.IsNullOrEmpty(st_UserID) Then st_UserID = Session("UserID")

        ' User 一覧取得
        Dim ds As DataSet = New DataSet
        ds.Tables.Add("UserID")
        If Session("Purchase.PrivilegeLevel") = "P" Then
            Try
                Using connection As New SqlClient.SqlConnection(DBConnectString.ConnectionString)
                    Dim st_query As String = "SELECT count(UserID) as count FROM v_User WHERE LocationCode = @LocationCode AND UserID = @UserID"

                    Dim command As New SqlClient.SqlCommand(st_query, connection)
                    connection.Open()

                    ' SQL SELECT パラメータの追加
                    command.Parameters.AddWithValue("UserID", st_UserID)
                    command.Parameters.AddWithValue("LocationCode", Session("LocationCode"))

                    ' SqlDataReader を生成し、検索処理を実行。
                    Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()
                    ' 取得件数が 1 件以上の場合は True, 0 件の場合は False を取得。
                    Dim b_hasrows As Boolean = reader.HasRows
                    reader.Close()

                    ' 取得件数が 1 件以上ある場合
                    If b_hasrows Then
                        ' クエリ、コマンド、アダプタの生成
                        st_query = "SELECT UserID, [Name] FROM v_User WHERE LocationCode = @LocationCode"
                        command.CommandText = st_query
                        Dim adapter As New SqlClient.SqlDataAdapter()

                        ' データベースからデータを取得
                        adapter.SelectCommand = command
                        adapter.Fill(ds.Tables("UserID"))

                        ' User プルダウンにバインド
                        UserID.DataValueField = "UserID"
                        UserID.DataTextField = "Name"
                        UserID.SelectedIndex = UserID.SelectedIndex
                        UserID.DataSource = ds.Tables("UserID")
                        UserID.DataBind()
                    End If

                End Using
            Catch ex As Exception
                'Exception をスローする
                Throw
            End Try

        ElseIf Session("Purchase.PrivilegeLevel") = "A" Then
            Try
                Using connection As New SqlClient.SqlConnection(DBConnectString.ConnectionString)
                    ' クエリ、アダプタ、SQLコマンド オブジェクトの生成
                    Dim st_query As String = "SELECT UserID, [Name] FROM v_User"
                    Dim adapter As New SqlClient.SqlDataAdapter()
                    Dim command As New SqlClient.SqlCommand(st_query, connection)

                    ' データベースからデータを取得
                    adapter.SelectCommand = command
                    adapter.Fill(ds.Tables("UserID"))

                    ' User プルダウンにバインド
                    UserID.DataValueField = "UserID"
                    UserID.DataTextField = "Name"
                    UserID.SelectedIndex = UserID.SelectedIndex
                    UserID.DataSource = ds.Tables("UserID")
                    UserID.DataBind()
                End Using
            Catch ex As Exception
                'Exception をスローする
                Throw
            End Try
        End If




    End Sub

    Protected Sub Switch_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Switch.Click

        ' SQL パラメータ設定
        SrcRFQ.SelectParameters.Add("UserID", st_UserID)
        SrcPO_Overdue.SelectParameters.Add("UserID", st_UserID)
        SrcPO_PPI.SelectParameters.Add("UserID", st_UserID)
        SrcPO_Par.SelectParameters.Add("UserID", st_UserID)

        ' クエリ設定
        SrcRFQ.SelectCommand = _
              "SELECT RH.RFQNumber, RH.StatusChangeDate, RH.Status, RH.ProductNumber, RH.ProductName, " _
            & "       RH.Purpose, RH.QuoUserName, RH.QuoLocationName, " _
            & "       RH.SupplierName, RH.MakerName, RR.RFQCorres " _
            & "FROM v_RFQHeader AS RH LEFT OUTER JOIN " _
            & "     v_RFQReminder AS RR ON RH.RFQNumber = RR.RFQNumber AND RR.RcptUserID = @UserID " _
            & "WHERE EnqUserID = @UserID " _
            & "ORDER BY StatusSortOrder, StatusChangeDate "

        SrcPO_Overdue.SelectCommand = _
              "SELECT PONumber, StatusChangeDate, Status, ProductNumber, ProductName," _
            & "       PODate, POUserName, POLocationName, SupploerName, MalerName,DelivaryDate, " _
            & "       OrderQuantity, OrderUnitCode, CurrencyCode,  'Overdue' as POCorrespondesnce " _
            & "FROM v_PO " _
            & "WHERE POUserID = @UserID " _
            & "  AND DueDate < GETDATE() " _
            & "  AND ((ParPONumber IS NULL) AND (StatusSortOrder <= 11) " _
            & "        OR (ParPONumber IS NOT NULL) AND (StatusCode = 'CPI')) "

        SrcPO_PPI.SelectCommand = _
              "SELECT  " _
            & "  PONumber, POUserID, SOUserID, DueDate, ParPONumber, StatusSortOrder, StatusCode, Status, Status as POCorrespondesnce " _
            & "FROM v_PO " _
            & "WHERE SOUserID = @UserID " _
            & "  AND StatusCode = 'PPI' "

        SrcPO_Par.SelectCommand = _
              "SELECT  " _
            & "  v_PO.PONumber, v_PO.POUserID, v_PO.SOUserID, v_PO.DueDate, v_PO.ParPONumber, v_PO.StatusSortOrder, v_PO.StatusCode, v_PO.Status, v_POReminder.POCorres as POCorrespondesnce " _
            & "FROM v_PO INNER JOIN " _
            & "     v_POReminder ON v_POReminder.PONumber = v_PO.PONumber AND v_POReminder.RcptUserID = @UserID " _
            & "WHERE ((v_PO.SOUserID = @UserID) OR (v_PO.POUserID = @UserID)) " _
            & "  AND v_PO.ParPONumber IS NULL " _
            & "ORDER BY v_PO.StatusSortOrder ASC "


    End Sub

    Protected Sub GetChildPO(ByVal sender As Object, ByVal e As EventArgs) Handles POList_Par.ItemDataBound

        Dim ds As DataSet = New DataSet
        ds.Tables.Add("child")




    End Sub

End Class
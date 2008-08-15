Public Partial Class RFQSelect
    Inherits CommonPage
    ' 変数宣言
    Private DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    ' 定数宣言
    Private Const MSG_REQUIED_ParPONumber = "購買発注番号が指定されていません。"
    Private Const MSG_REQUIED_ACTION = "データを処理できませんでした。"
    ' 検索キー用構造体(親POのデータが入る）
    Structure ParPOData
        Dim PONumber As String
        Dim ProductID As String
        Dim SupplierCode As String
        Dim MakerCode As String
    End Structure

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' 変数宣言
        Dim PPO As ParPOData
        PPO.PONumber = ""
        PPO.ProductID = ""
        PPO.SupplierCode = ""
        PPO.MakerCode = ""

        ' コントロール初期化
        Msg.Text = ""
        SelectForm.Visible = True
        RFQHeaderList.Visible = True

        ' パラメータ取得
        If Request.RequestType = "POST" Then
            PPO.PONumber = IIf(String.IsNullOrEmpty(Request.Form("ParPONumber")), "", Request.Form("ParPONumber"))
        ElseIf Request.RequestType = "GET" Then
            PPO.PONumber = IIf(String.IsNullOrEmpty(Request.QueryString("ParPONumber")), "", Request.QueryString("ParPONumber"))
        End If

        ' 空白除去
        PPO.PONumber = PPO.PONumber.Trim

        ' HiddenField に設定
        ParPONumber.Value = PPO.PONumber

        ' パラメータを取得できなかった場合はエラー終了
        If String.IsNullOrEmpty(PPO.PONumber) Then
            Msg.Text = MSG_REQUIED_ParPONumber
            SelectForm.Visible = False
            Exit Sub
        End If

        ' 親データ取得。取得できなかった場合はエラー終了
        SetParPOData(PPO)
        If String.IsNullOrEmpty(PPO.SupplierCode) Then
            Msg.Text = Common.ERR_NO_MATCH_FOUND
            SelectForm.Visible = False
            Exit Sub
        End If

        ' 製品情報取得 & Label にセット
        SetProductData(PPO.ProductID)

        ' 仕入先情報取得 & Label にセット
        SetSupplierData(PPO.SupplierCode)

        ' RFQHeader 取得 & バインド
        SetRFQHeaderQuery(PPO)
    End Sub

    ' 親POデータを取得する
    ' ParPONumber をキーに PO を検索し、ProductID、SupplierCode、MaKerCode に値をセットする。
    Protected Sub SetParPOData(ByRef data As ParPOData)
        Try
            Using connection As New SqlClient.SqlConnection(DBConnectString.ConnectionString)
                Dim st_query As String = "SELECT ProductID, SupplierCode, MakerCode FROM PO WHERE PONumber = @PONumber"
                Dim command As New SqlClient.SqlCommand(st_query, connection)
                connection.Open()

                ' SQL SELECT パラメータの追加
                command.Parameters.AddWithValue("PONumber", data.PONumber)

                ' SqlDataReader を生成し、検索処理を実行。
                Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()

                ' データを変数にセット
                If reader.HasRows Then
                    reader.Read()
                    data.ProductID = IIf(IsDBNull(reader("ProductID")), "", reader("ProductID"))
                    data.SupplierCode = IIf(IsDBNull(reader("SupplierCode")), "", reader("SupplierCode"))
                    data.MakerCode = IIf(IsDBNull(reader("MakerCode")), "", reader("MakerCode"))
                End If

                reader.Close()
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' 製品情報を取得し、aspx のラベルにセットする
    Protected Sub SetProductData(ByVal ProductID As String)
        Try
            Using connection As New SqlClient.SqlConnection(DBConnectString.ConnectionString)
                Dim st_query As String = _
                      "SELECT " _
                    & "  ProductNumber, ISNULL(Name, QuoName) AS ProductName " _
                    & "FROM " _
                    & "  Product " _
                    & "WHERE " _
                    & "  (ProductID = @ProductID)"
                Dim command As New SqlClient.SqlCommand(st_query, connection)
                connection.Open()

                ' Add param
                command.Parameters.AddWithValue("ProductID", ProductID)

                ' Search
                Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()

                ' Label ctrl にデータをセット
                reader.Read()
                ProductNumber.Text = IIf(IsDBNull(reader("ProductNumber")), "", reader("ProductNumber"))
                ProductName.Text = IIf(IsDBNull(reader("ProductName")), "", reader("ProductName"))

                reader.Close()
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' 仕入先情報取得し、aspx のラベルにセットする
    Protected Sub SetSupplierData(ByVal SuppplierCode As String)
        Try
            Using connection As New SqlClient.SqlConnection(DBConnectString.ConnectionString)
                Dim st_query As String = _
                      "SELECT " _
                    & "  Name3, Name4, s_Country.[Name] AS CountryName " _
                    & "FROM " _
                    & "  Supplier, s_Country " _
                    & "WHERE " _
                    & "  (SupplierCode = @SupplierCode) " _
                    & "  AND (Supplier.CountryCode = s_Country.CountryCode)"

                Dim command As New SqlClient.SqlCommand(st_query, connection)
                connection.Open()

                ' Add param
                command.Parameters.AddWithValue("SupplierCode", SuppplierCode)

                ' Search
                Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()

                ' Label ctrl にデータをセット
                reader.Read()
                Country.Text = IIf(IsDBNull(reader("CountryName")), "", reader("CountryName"))
                If IsDBNull(reader("Name3")) Then
                    SupplierName.Text = reader("Name4")
                Else
                    SupplierName.Text = reader("Name3") & " " & reader("Name4")
                End If

                reader.Close()
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' RFQHeader を取得するためのクエリを SQL データソースコントロールに設定する
    Protected Sub SetRFQHeaderQuery(ByVal data As ParPOData)

        SrcRFQHeader.SelectParameters.Clear()
        SrcRFQHeader.SelectParameters.Add("ProductID", data.ProductID)
        SrcRFQHeader.SelectParameters.Add("SuplierCode", data.SupplierCode)
        Dim st_where As String = ""
        If Not String.IsNullOrEmpty(data.MakerCode) Then
            SrcRFQHeader.SelectParameters.Add("MakerCode", data.MakerCode)
            st_where = " AND MakerCode = @MakerCode "
        End If

        SrcRFQHeader.SelectCommand = _
              "SELECT  " _
            & "  RH.RFQNumber, RH.QuotedDate, RH.StatusChangeDate, RH.Status, RH.Purpose, " _
            & "  RH.ProductNumber, RH.ProductName, RH.SupplierName, RH.SupplierItemName,  " _
            & "  RH.MakerName, RH.ShippingHandlingFee, RH.ShippingHandlingCurrencyCode, " _
            & "  RH.EnqLocationName, RH.EnqUserName, " _
            & "  RH.QuoLocationName, RH.QuoUserName, RH.Comment,  " _
            & "  CS.Name AS SupplierCountryName,  " _
            & "  CM.Name AS MakerCountryName " _
            & "FROM " _
            & "  s_Country AS CM " _
            & "    RIGHT OUTER JOIN v_RFQHeader AS RH " _
            & "      INNER JOIN s_Country AS CS " _
            & "      ON RH.SupplierCountryCode = CS.CountryCode " _
            & "    ON CM.CountryCode = RH.MakerCountryCode " _
            & "WHERE " _
            & "  ProductID = @ProductID " _
            & "  AND SupplierCode = @SuplierCode " _
            & st_where _
            & "ORDER BY " _
            & "  RH.QuotedDate DESC, RH.StatusChangeDate DESC, RH.RFQNumber ASC"

    End Sub

    ' RFQLine を RFQHeader のレコードごとに取得し、バインドする
    Protected Sub Get_RFQLine(ByVal sender As Object, ByVal e As System.EventArgs) Handles RFQHeaderList.ItemDataBound
        Dim lv As ListView = CType(CType(e, ListViewItemEventArgs).Item.FindControl("RFQLineList"), ListView)
        Dim src As SqlDataSource = CType(CType(e, ListViewItemEventArgs).Item.FindControl("SrcRFQLine"), SqlDataSource)
        Dim label As Label = CType(CType(e, ListViewItemEventArgs).Item.FindControl("RFQNumber"), Label)

        src.SelectParameters.Clear()
        src.SelectParameters.Add("RFQNumber", label.Text)
        src.SelectCommand = _
              "SELECT RFQLineNumber, EnqQuantity, EnqUnitCode, EnqPiece, CurrencyCode, UnitPrice, " _
            & "  QuoPer, QuoUnitCode, LeadTime, Packing, Purity, QMMethod " _
            & "FROM v_RFQLine " _
            & "WHERE RFQNumber = @RFQNumber "
        'lv.DataSourceID = src.ID
        'lv.DataBind()
    End Sub

    ' NextPage ボタンクリック処理
    Protected Sub Next_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim st_Action As String = "" ' Action 格納変数
        Dim st_ParPONumber As String = ""
        Dim st_RFQLineData() As String ' LineNumber と UnitPrice を格納

        ' パラメータ取得
        If Request.RequestType = "POST" Then
            st_Action = IIf(Request.Form("Action") = Nothing, "", Request.Form("Action"))
            st_ParPONumber = IIf(Request.Form("ParPONumber") = Nothing, "", Request.Form("ParPONumber"))
        ElseIf Request.RequestType = "GET" Then
            st_Action = IIf(Request.QueryString("Action") = Nothing, "", Request.QueryString("Action"))
            st_ParPONumber = IIf(Request.QueryString("ParPONumber") = Nothing, "", Request.QueryString("ParPONumber"))
        End If

        st_RFQLineData = Split(CType(sender, Button).CommandArgument, ",")

        ' パラメータチェック
        ' 取得できない・"Next" で無い場合はエラー
        If String.IsNullOrEmpty(st_action) Or st_action <> "Next" Then
            Msg.Text = MSG_REQUIED_ACTION
            Exit Sub
        End If
        'ParPONumber が取得できない場合はエラー
        If String.IsNullOrEmpty(st_ParPONumber) Then
            Msg.Text = MSG_REQUIED_ACTION
            Exit Sub
        End If
        ' RFQLine のデータが取得できない場合はエラー
        If String.IsNullOrEmpty(st_RFQLineData(0)) Or String.IsNullOrEmpty(st_RFQLineData(1)) Then
            Msg.Text = MSG_REQUIED_ACTION
            Exit Sub
        End If

        ' POIssue.aspx に遷移
        Response.Redirect("POIssue.aspx?ParPONumber=" & st_ParPONumber & "&RFQLineNumber=" & st_RFQLineData(0))

    End Sub

End Class
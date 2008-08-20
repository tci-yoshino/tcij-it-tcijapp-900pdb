Public Partial Class RFQSelect
    Inherits CommonPage
    ' 変数宣言
    Private DBConnectString As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Protected st_ParPONumber As String = "" ' aspx 側で読むため、Protected にする
    Private st_ProductID As String = ""
    Private st_SupplierCode As String = ""
    Private st_MakerCode As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' コントロール初期化
        Msg.Text = ""

        ' パラメータ取得
        If Request.RequestType = "POST" Then
            st_ParPONumber = IIf(String.IsNullOrEmpty(Request.Form("ParPONumber")), "", Request.Form("ParPONumber"))
        ElseIf Request.RequestType = "GET" Then
            st_ParPONumber = IIf(String.IsNullOrEmpty(Request.QueryString("ParPONumber")), "", Request.QueryString("ParPONumber"))
        End If

        ' 空白除去
        st_ParPONumber = st_ParPONumber.Trim

        ' パラメータを取得できなかった場合はエラー終了
        If String.IsNullOrEmpty(st_ParPONumber) Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            st_ParPONumber = ""
            Exit Sub
        End If

        ' 親データ取得。取得できなかった場合はエラー終了
        Set_ParPOData(st_ParPONumber, st_ProductID, st_SupplierCode, st_MakerCode)
        If String.IsNullOrEmpty(st_SupplierCode) Then
            Msg.Text = Common.ERR_NO_MATCH_FOUND
            st_ParPONumber = ""
            Exit Sub
        End If

        ' 製品情報取得 & Label にセット
        Set_ProductData(st_ProductID)

        ' 仕入先情報取得 & Label にセット
        Set_SupplierData(st_SupplierCode)

        ' RFQHeader 取得 & バインド
        Set_RFQHeaderQuery(st_ProductID, st_SupplierCode, st_MakerCode)
    End Sub

    ' 親POデータを取得する
    ' ParPONumber をキーに PO を検索し、ProductID、SupplierCode、MaKerCode に値をセットする。
    Protected Sub Set_ParPOData(ByVal PONumber As String, ByRef ProductID As String, ByRef SupplierCode As String, ByRef MakerCode As String)
        Try
            Using connection As New SqlClient.SqlConnection(DBConnectString.ConnectionString)
                Dim st_query As String = "SELECT ProductID, SupplierCode, MakerCode FROM PO WHERE PONumber = @PONumber"
                Dim command As New SqlClient.SqlCommand(st_query, connection)
                connection.Open()

                ' SQL SELECT パラメータの追加
                command.Parameters.AddWithValue("PONumber", PONumber)

                ' SqlDataReader を生成し、検索処理を実行。
                Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()

                ' データを変数にセット
                If reader.HasRows Then
                    reader.Read()
                    ProductID = IIf(IsDBNull(reader("ProductID")), "", reader("ProductID"))
                    SupplierCode = IIf(IsDBNull(reader("SupplierCode")), "", reader("SupplierCode"))
                    MakerCode = IIf(IsDBNull(reader("MakerCode")), "", reader("MakerCode"))
                End If

                reader.Close()
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub

    ' 製品情報を取得し、aspx のラベルにセットする
    Protected Sub Set_ProductData(ByVal ProductID As String)
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
    End Sub

    ' 仕入先情報取得し、aspx のラベルにセットする
    Protected Sub Set_SupplierData(ByVal SuppplierCode As String)
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
    End Sub

    ' RFQHeader を取得するためのクエリを SQL データソースコントロールに設定する
    Protected Sub Set_RFQHeaderQuery(ByVal ProductID As String, ByVal SupplierCode As String, ByVal MakerCode As String)

        SrcRFQHeader.SelectParameters.Clear()
        SrcRFQHeader.SelectParameters.Add("ProductID", ProductID)
        SrcRFQHeader.SelectParameters.Add("SuplierCode", SupplierCode)
        Dim st_where As String = ""
        If Not String.IsNullOrEmpty(MakerCode) Then
            SrcRFQHeader.SelectParameters.Add("MakerCode", MakerCode)
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

    ' RFQLine を RFQHeader のレコードごとに取得する。
    Protected Sub Set_RFQLine(ByVal sender As Object, ByVal e As System.EventArgs) Handles RFQHeaderList.ItemDataBound

        Dim src As SqlDataSource = DirectCast(DirectCast(e, ListViewItemEventArgs).Item.FindControl("SrcRFQLine"), SqlDataSource)
        Dim label As Label = DirectCast(DirectCast(e, ListViewItemEventArgs).Item.FindControl("RFQNumber"), Label)

        src.SelectParameters.Clear()
        src.SelectParameters.Add("RFQNumber", label.Text)
        src.SelectCommand = _
              "SELECT RFQLineNumber, EnqQuantity, EnqUnitCode, EnqPiece, CurrencyCode, UnitPrice, " _
            & "  QuoPer, QuoUnitCode, LeadTime, Packing, Purity, QMMethod " _
            & "FROM v_RFQLine " _
            & "WHERE RFQNumber = @RFQNumber "

    End Sub

End Class
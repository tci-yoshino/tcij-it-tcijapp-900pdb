Public Partial Class RFQListBySupplier
    Inherits CommonPage
    Protected st_SupplierCode As String = String.Empty ' aspx 側で読むため、Protected にする
    Protected i_DataNum As Integer = 0 ' 0 の場合は Supplier Data が無いと判断し、 Data not found. を表示する。

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' パラメータ取得
        If Request.RequestType = "POST" Then
            st_SupplierCode = IIf(Request.Form("SupplierCode") = Nothing, "", Request.Form("SupplierCode"))
        ElseIf Request.RequestType = "GET" Then
            st_SupplierCode = IIf(Request.QueryString("SupplierCode") = Nothing, "", Request.QueryString("SupplierCode"))
        End If

        ' 空白除去
        st_SupplierCode = st_SupplierCode.Trim

        If st_SupplierCode = "" Or IsNumeric(st_SupplierCode) = False Then
            st_SupplierCode = String.Empty
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        ' Supplier 情報取得
        Using connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
            Dim st_query As String = _
                  "SELECT SupplierCode, LTRIM(RTRIM(ISNULL(Name3, '') + ' ' + ISNULL(Name4, ''))) AS Name, " _
                & "       Address1, Address2, Address3, PostalCode, Telephone, Fax, Email, " _
                & "       Website, v_Country.CountryName " _
                & "FROM Supplier,v_Country " _
                & "WHERE SupplierCode = @SupplierCode " _
                & "  AND Supplier.CountryCode = v_Country.CountryCode"
            Dim command As New SqlClient.SqlCommand(st_query, connection)
            connection.Open()

            ' SQL SELECT パラメータの追加
            command.Parameters.AddWithValue("SupplierCode", st_SupplierCode)

            ' SqlDataReader を生成し、検索処理を実行。
            Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()

            If reader.HasRows Then

                i_DataNum = 1
                reader.Read()

                ' 読込んだデータを各 Label に設定。
                SupplierCode.Text = reader("SupplierCode")
                SupplierName.Text = reader("Name").ToString()
                Address1.Text = reader("Address1").ToString()
                Address2.Text = reader("Address2").ToString()
                Address3.Text = reader("Address3").ToString()
                PostalCode.Text = reader("PostalCode").ToString()
                Telephone.Text = reader("Telephone").ToString()
                Fax.Text = reader("Fax").ToString()
                Email.Text = reader("Email").ToString()
                EmailLink.NavigateUrl = "mailto:" & reader("Email").ToString()
                Website.Text = reader("Website").ToString()
                WebsiteLink.NavigateUrl = "http://" & reader("Website").ToString()
                CountryName.Text = reader("CountryName").ToString()

            Else
                Exit Sub
            End If
            reader.Close()
        End Using

        ' RFQHeader 取得
        If i_DataNum = 1 Then

            SrcRFQHeader.SelectParameters.Clear()
            SrcRFQHeader.SelectParameters.Add("SupplierCode", st_SupplierCode)
            SrcRFQHeader.SelectCommand = _
                  "SELECT " _
                & "  RH.RFQNumber, RH.QuotedDate, RH.StatusChangeDate, RH.Status, " _
                & "  RH.ProductNumber,RH.ProductName, RH.SupplierName, " _
                & "  RH.Purpose, RH.MakerName, " _
                & "  RH.SupplierItemName, RH.ShippingHandlingFee, RH.ShippingHandlingCurrencyCode, " _
                & "  RH.EnqUserName, RH.EnqLocationName, RH.QuoUserName, RH.QuoLocationName, RH.Comment, " _
                & "  C.[Name] AS MakerCountryName, CS.[Name] AS SupplierCountryName " _
                & "FROM " _
                & "  v_RFQHeader AS RH INNER JOIN " _
                & "  s_Country AS CS ON CS.CountryCode = RH.SupplierCountryCode LEFT OUTER JOIN " _
                & "  s_Country AS C ON C.CountryCode = RH.MakerCountryCode " _
                & "WHERE " _
                & "  (RH.SupplierCode = @SupplierCode OR RH.MakerCode = @SupplierCode) " _
                & "ORDER BY " _
                & "  QuotedDate DESC, StatusChangeDate DESC, RFQNumber ASC "
        End If

    End Sub

    ' RFQLine を取得する。(RFQHeader 項目バインド時に発生)
    Protected Sub GetRFQLine(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles RFQHeaderList.ItemDataBound
        Dim lv As ListView = CType(e.Item.FindControl("RFQLineList"), ListView)
        Dim src As SqlDataSource = CType(e.Item.FindControl("SrcRFQLine"), SqlDataSource)
        Dim label As Label = CType(e.Item.FindControl("RFQNumber"), Label)

        src.SelectParameters.Clear()
        src.SelectParameters.Add("RFQNumber", label.Text)
        src.SelectCommand = _
              "SELECT distinct RL.RFQLineNumber, RL.EnqQuantity, RL.EnqUnitCode, RL.EnqPiece, " _
            & "       RL.CurrencyCode, RL.UnitPrice, RL.QuoPer, RL.QuoUnitCode, " _
            & "       RL.LeadTime, RL.Packing, RL.Purity, RL.QMMethod, RL.NoOfferReason, " _
            & "       PO.RFQLineNumber AS PO " _
            & "FROM v_RFQLine AS RL LEFT OUTER JOIN " _
            & "     PO ON PO.RFQLineNumber = RL.RFQLineNumber " _
            & "WHERE RL.RFQNumber = @RFQNumber"
        lv.DataSourceID = src.ID
        lv.DataBind()
    End Sub

    Protected Sub SrcRFQHeader_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcRFQHeader.Selecting
        e.Command.CommandTimeout = 0
    End Sub

End Class
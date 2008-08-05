Public Partial Class RFQListBySupplier
    Inherits CommonPage
    Public st_SupplierCode As String
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' パラメータ取得
        If Request.RequestType = "POST" Then
            st_SupplierCode = IIf(Request.Form("SupplierCode") = Nothing, "", Request.Form("SupplierCode"))
        ElseIf Request.RequestType = "GET" Then
            st_SupplierCode = IIf(Request.QueryString("SupplierCode") = Nothing, "", Request.QueryString("SupplierCode"))
        End If

        If st_SupplierCode = "" Then
            Msg.Text = "Supplier Code が指定されていない、または存在しない Supplier Code が指定されています。"
            Exit Sub
        End If

        ' Supplier 情報取得
        Try
            Using connection As New SqlClient.SqlConnection(DBConnectString.ConnectionString)
                Dim st_query As String = _
                      "SELECT SupplierCode, Name3, Name4, " _
                    & "       Address1, Address2, Address3, PostalCode, Telephone, Fax, Email, " _
                    & "       Website, v_Country.CountryName " _
                    & "FROM Supplier,v_Country " _
                    & "WHERE SupplierCode = '1' " _
                    & "  AND Supplier.CountryCode = v_Country.CountryCode"
                Dim command As New SqlClient.SqlCommand(st_query, connection)
                connection.Open()

                ' SQL SELECT パラメータの追加
                command.Parameters.AddWithValue("SupplierCode", st_SupplierCode)

                ' SqlDataReader を生成し、検索処理を実行。
                Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()
                ' データを一行読込む
                reader.Read()
                ' 読込んだデータを各 Label に設定。（aspx 側でできないだろうか？）
                SupplierCode.Text = reader("SupplierCode")
                SupplierName.Text = IIf(IsDBNull(reader("Name3")) Or IsDBNull(reader("Name4")), reader("Name3") & reader("Name4"), reader("Name3") & " " & reader("Name4"))
                Address1.Text = IIf(IsDBNull(reader("Address1")), "", reader("Address1"))
                Address2.Text = IIf(IsDBNull(reader("Address2")), "", reader("Address2"))
                Address3.Text = IIf(IsDBNull(reader("Address3")), "", reader("Address3"))
                PostalCode.Text = IIf(IsDBNull(reader("PostalCode")), "", reader("PostalCode"))
                Telephone.Text = IIf(IsDBNull(reader("Telephone")), "", reader("Telephone"))
                Fax.Text = IIf(IsDBNull(reader("Fax")), "", reader("Fax"))
                Email.Text = IIf(IsDBNull(reader("Email")), "", reader("Email"))
                EmailLink.NavigateUrl = IIf(IsDBNull(reader("Email")), "", reader("Email"))
                Website.Text = IIf(IsDBNull(reader("Website")), "", reader("Website"))
                WebsiteLink.NavigateUrl = IIf(IsDBNull(reader("Website")), "", reader("Website"))
                CountryName.Text = IIf(IsDBNull(reader("CountryName")), "", reader("CountryName"))

                reader.Close()

            End Using
        Catch ex As Exception
            'Exception をスローする
            Throw
        End Try

        ' RFQHeader 取得。
        SrcRFQHeader.SelectParameters.Clear()
        SrcRFQHeader.SelectParameters.Add("SupplierCode", st_SupplierCode)
        SrcRFQHeader.SelectCommand = _
              "SELECT RFQNumber, QuotedDate, StatusChangeDate, Status, " _
            & "       ProductNumber,ProductName,SupplierName, " _
            & "       Purpose, MakerName, " _
            & "       SupplierItemName, ShippingHandlingFee, ShippingHandlingCurrencyCode, " _
            & "       EnqUserName, EnqLocationName, QuoUserName, QuoLocationName, Comment " _
            & "FROM v_RFQHeader " _
            & "WHERE SupplierCode = @SupplierCode " _
            & "   OR MakerCode = @SupplierCode " _
            & "ORDER BY QuotedDate ASC, StatusChangeDate DESC, RFQNumber ASC "

    End Sub

    Protected Sub test(ByVal sender As Object, ByVal e As EventArgs) Handles RFQHeaderList.ItemDataBound
        Dim lv As ListView = CType(CType(e, ListViewItemEventArgs).Item.FindControl("RFQLineList"), ListView)
        Dim src As SqlDataSource = CType(CType(e, ListViewItemEventArgs).Item.FindControl("SrcRFQLine"), SqlDataSource)
        Dim label As Label = CType(CType(e, System.Web.UI.WebControls.ListViewItemEventArgs).Item.FindControl("RFQNumber"), Label)

        src.SelectParameters.Clear()
        src.SelectParameters.Add("RFQNumber", label.Text)
        src.SelectCommand = _
              "SELECT distinct RL.RFQNumber, RL.EnqQuantity, RL.EnqUnitCode, RL.EnqPiece, " _
            & "       RL.CurrencyCode, RL.UnitPrice, RL.QuoPer, RL.QuoUnitCode, " _
            & "       RL.LeadTime, RL.Packing, RL.Purity, RL.QMMethod, " _
            & "       PO.RFQLineNumber AS PO " _
            & "FROM v_RFQLine AS RL LEFT OUTER JOIN " _
            & "     PO ON PO.RFQLineNumber = RL.RFQLineNumber " _
            & "WHERE RL.RFQNumber = @RFQNumber"
        lv.DataSourceID = src.ID
        lv.DataBind()
    End Sub

End Class
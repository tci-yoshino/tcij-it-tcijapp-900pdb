Imports Purchase.Common

Partial Public Class RFQListBySupplier
    Inherits CommonPage
    Protected st_SupplierCode As String = String.Empty ' aspx 側で読むため、Protected にする
    Protected i_DataNum As Integer = 0 ' 0 の場合は Supplier Data が無いと判断し、 Data not found. を表示する。

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' パラメータ取得
        If Request.RequestType = "POST" And IsPostBack = False Then
            st_SupplierCode = IIf(Request.Form("SupplierCode") = Nothing, "", Request.Form("SupplierCode"))
        ElseIf Request.RequestType = "GET" Or IsPostBack = True Then
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
                & "       Website, Info, v_Country.CountryName,Note " _
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
                WebsiteLink.NavigateUrl = reader("Website").ToString()
                SupplierInfoLink.NavigateUrl = reader("Info").ToString()
                If Not String.IsNullOrEmpty(SupplierInfoLink.NavigateUrl) Then
                    SupplierInfo.Text = "Supplier Information"
                End If
                CountryName.Text = reader("CountryName").ToString()
                Comment.Text = Replace(reader("Note").ToString(), vbCrLf, "<br />")
            Else
                Exit Sub
            End If
            reader.Close()
        End Using

        ' RFQHeader 取得
        If i_DataNum = 1 Then

            Dim sqlStr As StringBuilder = New StringBuilder
            sqlStr.AppendLine("SELECT")
            sqlStr.AppendLine("  RH.RFQNumber, ISNULL(RH.Priority, '') AS Priority, RH.QuotedDate, RH.StatusChangeDate, RH.Status,")
            sqlStr.AppendLine("  RH.ProductNumber,RH.ProductName, RH.SupplierName,")
            sqlStr.AppendLine("  RH.Purpose, RH.MakerName, RH.MakerInfo,")
            sqlStr.AppendLine("  RH.SupplierItemName, RH.ShippingHandlingFee, RH.ShippingHandlingCurrencyCode,")
            sqlStr.AppendLine("  RH.EnqUserName, RH.EnqLocationName, RH.QuoUserName, RH.QuoLocationName, RH.Comment,")
            sqlStr.AppendLine("  MC.[Name] AS MakerCountryName, SC.[Name] AS SupplierCountryName,")
            sqlStr.AppendLine("  RH.isCONFIDENTIAL")
            sqlStr.AppendLine("FROM")
            sqlStr.AppendLine("  v_RFQHeader AS RH INNER JOIN ")
            sqlStr.AppendLine("  s_Country AS SC ON SC.CountryCode = RH.SupplierCountryCode LEFT OUTER JOIN ")
            sqlStr.AppendLine("  s_Country AS MC ON MC.CountryCode = RH.MakerCountryCode ")
            sqlStr.AppendLine("WHERE")
            sqlStr.AppendLine("  (RH.SupplierCode = @SupplierCode OR RH.MakerCode = @SupplierCode)")
            '権限ロールに従い極秘品を除外する
            If Session(SESSION_ROLE_CODE).ToString = ROLE_WRITE_P OrElse Session(SESSION_ROLE_CODE).ToString = ROLE_READ_P Then
                sqlStr.Append("  AND RH.isCONFIDENTIAL = 0 ")
            End If
            sqlStr.AppendLine("ORDER BY")
            sqlStr.AppendLine("  RH.StatusSortOrder ASC, RH.QuotedDate DESC, RH.StatusChangeDate DESC, RH.RFQNumber ASC")

            SrcRFQHeader.SelectParameters.Clear()
            SrcRFQHeader.SelectParameters.Add("SupplierCode", st_SupplierCode)
            SrcRFQHeader.SelectCommand = sqlStr.ToString
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
            & "       RL.LeadTime, RL.Packing, RL.Purity, RL.QMMethod, RL.NoOfferReason,SupplierOfferNo, " _
            & "       PO.RFQLineNumber AS PO, CASE WHEN PO.Priority='C' THEN '' ELSE PO.Priority END AS Priority " _
            & "FROM v_RFQLine AS RL LEFT OUTER JOIN " _
            & " (SELECT RFQLineNumber " _
            & " ,MIN(CASE WHEN PO.QMStartingDate IS NOT NULL OR PO.QMFinishDate IS NOT NULL THEN 'C'" _
            & "     ELSE ISNULL(PO.Priority, 'C') END) AS Priority " _
            & " FROM PO GROUP BY RFQLineNumber )" _
            & " PO ON PO.RFQLineNumber = RL.RFQLineNumber " _
            & "WHERE RL.RFQNumber = @RFQNumber"
        lv.DataSourceID = src.ID
        lv.DataBind()
    End Sub

    Protected Sub SrcRFQHeader_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcRFQHeader.Selecting
        e.Command.CommandTimeout = 0
    End Sub

End Class
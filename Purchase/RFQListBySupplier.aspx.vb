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

        If Not IsPostBack Then
            SetValidQuotation(String.Empty)
        End If

        ' Supplier 情報取得
        Using connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
            Dim st_query As String =
                  "SELECT SupplierCode, LTRIM(RTRIM(ISNULL(Name3, '') + ' ' + ISNULL(Name4, ''))) AS Name, " _
                & "       Address1, Address2, Address3, PostalCode, Telephone, Fax, Email, " _
                & "       Website, Info, v_Country.CountryName,Note,SupplierWarning " _
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
                SupplierWarning.Text = Replace(reader("SupplierWarning").ToString(), vbCrLf, "<br />")
            Else
                Exit Sub
            End If
            reader.Close()
        End Using

        ' RFQHeader 取得
        If i_DataNum = 1 Then
            ShowList(st_SupplierCode, ValidQuotation.SelectedValue)
        End If

    End Sub

    ' RFQLine を取得する。(RFQHeader 項目バインド時に発生)
    Protected Sub GetRFQLine(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles RFQHeaderList.ItemDataBound
        Dim lv As ListView = CType(e.Item.FindControl("RFQLineList"), ListView)
        Dim src As SqlDataSource = CType(e.Item.FindControl("SrcRFQLine"), SqlDataSource)
        Dim label As Label = CType(e.Item.FindControl("RFQNumber"), Label)

        Dim sqlStr As New StringBuilder
        sqlStr.AppendLine("WITH POPriority AS (")
        sqlStr.AppendLine("    SELECT")
        sqlStr.AppendLine("        RFQLineNumber")
        sqlStr.AppendLine("        ,MIN(CASE WHEN PO.QMStartingDate IS NOT NULL OR PO.QMFinishDate IS NOT NULL THEN 'C' ELSE ISNULL(PO.[Priority], 'C') END) AS [Priority]")
        sqlStr.AppendLine("    FROM")
        sqlStr.AppendLine("        PO")
        sqlStr.AppendLine("    GROUP BY")
        sqlStr.AppendLine("        RFQLineNumber")
        sqlStr.AppendLine(")")
        sqlStr.AppendLine("SELECT DISTINCT")
        sqlStr.AppendLine("    RL.RFQLineNumber")
        sqlStr.AppendLine("    ,RL.RFQNumber")
        sqlStr.AppendLine("    ,RL.EnqQuantity")
        sqlStr.AppendLine("    ,RL.EnqUnitCode")
        sqlStr.AppendLine("    ,RL.EnqPiece")
        sqlStr.AppendLine("    ,RL.CurrencyCode")
        sqlStr.AppendLine("    ,RL.UnitPrice")
        sqlStr.AppendLine("    ,RL.QuoPer")
        sqlStr.AppendLine("    ,RL.QuoUnitCode")
        sqlStr.AppendLine("    ,RL.LeadTime")
        sqlStr.AppendLine("    ,RL.Packing")
        sqlStr.AppendLine("    ,RL.Purity")
        sqlStr.AppendLine("    ,RL.QMMethod")
        sqlStr.AppendLine("    ,RL.SupplierOfferNo")
        sqlStr.AppendLine("    ,RL.SupplierItemNumber")
        sqlStr.AppendLine("    ,RL.NoOfferReason")
        sqlStr.AppendLine("    ,P.RFQLineNumber AS PO")
        sqlStr.AppendLine("    ,CASE WHEN P.[Priority]='C' THEN '' ELSE P.[Priority] END AS [Priority]")
        sqlStr.AppendLine("    ,CASE WHEN RL.OutputStatus = 1 THEN 'Interface issued' END AS OutputStatus")
        sqlStr.AppendLine("FROM")
        sqlStr.AppendLine("    v_RFQLine AS RL")
        sqlStr.AppendLine("        LEFT OUTER JOIN POPriority AS P ON P.RFQLineNumber = RL.RFQLineNumber")
        sqlStr.AppendLine("WHERE")
        sqlStr.AppendLine("    RL.RFQNumber = @RFQNumber")

        src.SelectParameters.Clear()
        src.SelectParameters.Add("RFQNumber", label.Text)
        src.SelectCommand = sqlStr.ToString
        lv.DataSourceID = src.ID
        lv.DataBind()
    End Sub

    Protected Sub SrcRFQHeader_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcRFQHeader.Selecting
        e.Command.CommandTimeout = 0
    End Sub

    ''' <summary>
    ''' RFQHeader の一覧を表示します。
    ''' </summary>
    ''' <param name="supplierCode">仕入先コード</param>
    ''' <param name="validQuotation">有額回答の有無</param>
    Private Sub ShowList(ByVal supplierCode As String, ByVal validQuotation As String)

        Dim sqlStr As StringBuilder = New StringBuilder
        sqlStr.AppendLine("SELECT")
        sqlStr.AppendLine("    RH.RFQNumber")
        sqlStr.AppendLine("    ,ISNULL(RH.[Priority], '') AS [Priority]")
        sqlStr.AppendLine("    ,RH.QuotedDate")
        sqlStr.AppendLine("    ,RH.StatusChangeDate")
        sqlStr.AppendLine("    ,RH.[Status]")
        sqlStr.AppendLine("    ,RH.ProductNumber")
        sqlStr.AppendLine("    ,RH.CodeExtensionCode")
        sqlStr.AppendLine("    ,RH.ProductName")
        sqlStr.AppendLine("    ,RH.SupplierCode")
        sqlStr.AppendLine("    ,RH.SupplierName")
        sqlStr.AppendLine("    ,RH.Purpose")
        sqlStr.AppendLine("    ,RH.MakerName")
        sqlStr.AppendLine("    ,RH.MakerInfo")
        sqlStr.AppendLine("    ,RH.SupplierItemName")
        sqlStr.AppendLine("    ,RH.ShippingHandlingFee")
        sqlStr.AppendLine("    ,RH.ShippingHandlingCurrencyCode")
        sqlStr.AppendLine("    ,RH.EnqUserName")
        sqlStr.AppendLine("    ,RH.EnqLocationName")
        sqlStr.AppendLine("    ,RH.QuoUserName")
        sqlStr.AppendLine("    ,RH.QuoLocationName")
        sqlStr.AppendLine("    ,RH.Comment")
        sqlStr.AppendLine("    ,MC.[Name] AS MakerCountryName")
        sqlStr.AppendLine("    ,SC.[Name] AS SupplierCountryName")
        sqlStr.AppendLine("    ,RH.isCONFIDENTIAL")
        sqlStr.AppendLine("    ,RH.SupplierWarning")
        sqlStr.AppendLine("    ,'./RFQListByProduct.aspx?ProductID=' + CONVERT(varchar(10), RH.ProductID) AS ProductRFQLink")
        sqlStr.AppendLine("FROM")
        sqlStr.AppendLine("    v_RFQHeader AS RH")
        sqlStr.AppendLine("        INNER JOIN s_Country AS SC ON SC.CountryCode = RH.SupplierCountryCode")
        sqlStr.AppendLine("        LEFT OUTER JOIN s_Country AS MC ON MC.CountryCode = RH.MakerCountryCode")
        sqlStr.AppendLine("WHERE")
        sqlStr.AppendLine("    (RH.SupplierCode = @SupplierCode")
        sqlStr.AppendLine("        OR RH.MakerCode = @SupplierCode)")

        '権限ロールに従い極秘品を除外する
        If Session(SESSION_ROLE_CODE).ToString = ROLE_WRITE_P OrElse Session(SESSION_ROLE_CODE).ToString = ROLE_READ_P Then
            sqlStr.Append("    AND RH.isCONFIDENTIAL = 0")
        End If

        '有額回答の有無が指定された場合は条件を追加する
        If Not String.IsNullOrEmpty(validQuotation) Then
            sqlStr.Append(String.Format("    AND RH.ValidQuotation = {0}", validQuotation))
        End If

        sqlStr.AppendLine("ORDER BY")
        sqlStr.AppendLine("    RH.StatusSortOrder")
        sqlStr.AppendLine("    ,RH.QuotedDate DESC")
        sqlStr.AppendLine("    ,RH.StatusChangeDate DESC")
        sqlStr.AppendLine("    ,RH.RFQNumber")

        SrcRFQHeader.SelectParameters.Clear()
        SrcRFQHeader.SelectParameters.Add("SupplierCode", supplierCode)
        SrcRFQHeader.SelectCommand = sqlStr.ToString

    End Sub

    ''' <summary>
    ''' Search ボタンクリック時の処理を行います。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Search_Click(sender As Object, e As EventArgs) Handles Search.Click

        ShowList(SupplierCode.Text, ValidQuotation.SelectedValue)

        'ページ番号をリセットする
        Dim pager As DataPager = RFQHeaderList.FindControl("RFQPagerCountTop")
        pager.SetPageProperties(0, pager.MaximumRows, False)

    End Sub

    ''' <summary>
    ''' Release ボタンクリック時の処理を行います。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Release_Click(sender As Object, e As EventArgs) Handles Release.Click

        SetValidQuotation(String.Empty)
        ShowList(SupplierCode.Text, ValidQuotation.SelectedValue)

        'ページ番号をリセットする
        Dim pager As DataPager = RFQHeaderList.FindControl("RFQPagerCountTop")
        pager.SetPageProperties(0, pager.MaximumRows, False)

    End Sub

    ''' <summary>
    ''' 有額回答の有無 (Validity Quotation) ドロップダウンリストを設定します。
    ''' </summary>
    ''' <param name="selectedValue">選択する値</param>
    Private Sub SetValidQuotation(ByVal selectedValue As String)

        ValidQuotation.Items.Clear()
        ValidQuotation.Items.Add(New ListItem("Valid Price", "1"))
        ValidQuotation.Items.Add(New ListItem("Invalid Price", "0"))
        ValidQuotation.Items.Add(New ListItem("ALL", String.Empty))
        ValidQuotation.Items.FindByValue(selectedValue).Selected = True

    End Sub

End Class
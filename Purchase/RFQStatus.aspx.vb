Imports Purchase.Common

Partial Public Class RFQStatus
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            '[ERR_NO_MATCH_FOUND表示防止]-------------------------------------------------------
            RFQHeaderList.Visible = False

            '[StatusSortOrderFrom,StatusSortOrderToの値設定]------------------------------------
            DBCommand = DBConn.CreateCommand()
            DBConn.Open()
            DBCommand.CommandText = "SELECT Text, SortOrder FROM RFQStatus ORDER BY SortOrder"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            StatusSortOrderFrom.Items.Clear()
            StatusSortOrderFrom.Items.Add(New ListItem("", ""))
            StatusSortOrderTo.Items.Clear()
            StatusSortOrderTo.Items.Add(New ListItem("", ""))
            Do Until DBReader.Read = False
                StatusSortOrderFrom.Items.Add(New ListItem(DBReader("Text"), DBReader("SortOrder")))
                StatusSortOrderTo.Items.Add(New ListItem(DBReader("Text"), DBReader("SortOrder")))
            Loop
            DBReader.Close()

            '[EnqLocationCode,QuoLocationCodeの値設定]------------------------------------------
            DBCommand.CommandText = "SELECT LocationCode, Name FROM s_Location ORDER BY Name"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            EnqLocationCode.Items.Clear()
            EnqLocationCode.Items.Add(New ListItem("", ""))
            QuoLocationCode.Items.Clear()
            QuoLocationCode.Items.Add(New ListItem("", ""))
            Do Until DBReader.Read = False
                EnqLocationCode.Items.Add(New ListItem(DBReader("Name"), DBReader("LocationCode")))
                QuoLocationCode.Items.Add(New ListItem(DBReader("Name"), DBReader("LocationCode")))
            Loop
            DBReader.Close()

            '[PaymentTermCodeの値設定]----------------------------------------------------------
            DBCommand.CommandText = "SELECT Text, PaymentTermCode FROM PurchasingPaymentTerm ORDER BY PaymentTermCode"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            PaymentTermCode.Items.Clear()
            PaymentTermCode.Items.Add(New ListItem("", ""))
            Do Until DBReader.Read = False
                PaymentTermCode.Items.Add(New ListItem(DBReader("Text"), DBReader("PaymentTermCode")))
            Loop
            DBReader.Close()
            DBConn.Close()
            SrcRFQHeader.SelectCommand = ""
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

    Protected Sub EnqLocationCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles EnqLocationCode.SelectedIndexChanged
        '[EnqUserIDの値設定]--------------------------------------------------------------------
        Msg.Text = String.Empty
        DBCommand = DBConn.CreateCommand()
        DBCommand.CommandText = "SELECT Name AS EnqUserName, EnqUserID FROM RFQHeader, v_User WHERE RFQHeader.EnqUserID = v_User.UserID AND EnqLocationCode = '" & EnqLocationCode.SelectedValue & "' Group BY EnqUserID, Name ORDER BY Name"
        DBConn.Open()
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        EnqUserID.Items.Clear()
        EnqUserID.Items.Add(New ListItem("", ""))
        Do Until DBReader.Read = False
            EnqUserID.Items.Add(New ListItem(DBReader("EnqUserName"), DBReader("EnqUserID")))
        Loop
        DBReader.Close()
        DBConn.Close()
    End Sub

    Protected Sub QuoLocationCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles QuoLocationCode.SelectedIndexChanged
        '[QuoUserIDの値設定]--------------------------------------------------------------------
        Msg.Text = String.Empty
        DBCommand = DBConn.CreateCommand()
        DBCommand.CommandText = "SELECT Name AS QuoUserName, QuoUserID FROM RFQHeader, v_User WHERE RFQHeader.QuoUserID = v_User.UserID AND QuoLocationCode = '" & QuoLocationCode.SelectedValue & "' Group BY QuoUserID, Name ORDER BY Name"
        DBConn.Open()
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        QuoUserID.Items.Clear()
        QuoUserID.Items.Add(New ListItem("", ""))
        Do Until DBReader.Read = False
            QuoUserID.Items.Add(New ListItem(DBReader("QuoUserName"), DBReader("QuoUserID")))
        Loop
        DBReader.Close()
        DBConn.Close()
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        '[Search実行可能確認]-------------------------------------------------------------------
        If Action.Value <> "Search" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        '[RFQPagerCountTop,RFQPagerCountBottomのカウントを1にする為実行]------------------------
        SrcRFQHeader.SelectCommand = getBaseRFQHeaderSQL() + "WHERE 1=0"
        RFQHeaderList.DataBind()

        SearchRFQHeader()
    End Sub

    Private Sub SearchRFQHeader()
        Msg.Text = String.Empty
        SrcRFQHeader.SelectCommand = ""
        RFQHeaderList.Visible = False

        '[Status設定順序チェック]---------------------------------------------------------------
        If StatusSortOrderFrom.Text = "" And StatusSortOrderTo.Text <> "" Then Exit Sub
        If StatusSortOrderFrom.Text <> "" And StatusSortOrderTo.Text <> "" Then
            If StatusSortOrderTo.Text < StatusSortOrderFrom.Text Then Exit Sub
        End If

        '[Dateを1Byte形式に変換する]------------------------------------------------------------
        QuotedDateFrom.Text = StrConv(QuotedDateFrom.Text, VbStrConv.Narrow)
        QuotedDateTo.Text = StrConv(QuotedDateTo.Text, VbStrConv.Narrow)
        StatusChangeDateFrom.Text = StrConv(StatusChangeDateFrom.Text, VbStrConv.Narrow)
        StatusChangeDateTo.Text = StrConv(StatusChangeDateTo.Text, VbStrConv.Narrow)

        '[日付妥当性チェック]-------------------------------------------------------------------
        If QuotedDateFrom.Text <> "" And Not (IsDate(QuotedDateFrom.Text) And Regex.IsMatch(QuotedDateFrom.Text, DATE_REGEX_OPTIONAL)) Then
            Msg.Text = "Quoted Date (From) " & ERR_INVALID_DATE
            Exit Sub
        End If
        If QuotedDateTo.Text <> "" And Not (IsDate(QuotedDateTo.Text) And Regex.IsMatch(QuotedDateTo.Text, DATE_REGEX_OPTIONAL)) Then
            Msg.Text = "Quoted Date (To) " & ERR_INVALID_DATE
            Exit Sub
        End If
        If StatusChangeDateFrom.Text <> "" And Not (IsDate(StatusChangeDateFrom.Text) And Regex.IsMatch(StatusChangeDateFrom.Text, DATE_REGEX_OPTIONAL)) Then
            Msg.Text = "Status Change Date (From) " & ERR_INVALID_DATE
            Exit Sub
        End If
        If StatusChangeDateTo.Text <> "" And Not (IsDate(StatusChangeDateTo.Text) And Regex.IsMatch(StatusChangeDateTo.Text, DATE_REGEX_OPTIONAL)) Then
            Msg.Text = "Status Change Date (To) " & ERR_INVALID_DATE
            Exit Sub
        End If

        '[日付設定順序チェック]-----------------------------------------------------------------
        If QuotedDateFrom.Text = "" And QuotedDateTo.Text <> "" Then Exit Sub
        If QuotedDateFrom.Text <> "" And QuotedDateTo.Text <> "" Then
            If QuotedDateTo.Text < QuotedDateFrom.Text Then Exit Sub
        End If
        If StatusChangeDateFrom.Text = "" And StatusChangeDateTo.Text <> "" Then Exit Sub
        If StatusChangeDateFrom.Text <> "" And StatusChangeDateTo.Text <> "" Then
            If StatusChangeDateTo.Text < StatusChangeDateFrom.Text Then Exit Sub
        End If

        '[SrcRFQHeaderの値設定]-----------------------------------------------------------------
        Dim st_SQL As New Text.StringBuilder
        st_SQL.Append("" & getBaseRFQHeaderSQL() & "")

        'WHERE句の作成
        Dim st_WHR As String = String.Empty
        If StatusSortOrderFrom.SelectedValue <> "" And StatusSortOrderTo.SelectedValue = "" Then
            st_WHR = st_WHR & "StatusSortOrder = '" & StatusSortOrderFrom.SelectedValue & "' AND "
        End If
        If StatusSortOrderFrom.SelectedValue <> "" And StatusSortOrderTo.SelectedValue <> "" Then
            st_WHR = st_WHR & "StatusSortOrder >= '" & StatusSortOrderFrom.SelectedValue & "' AND StatusSortOrder <= '" & StatusSortOrderTo.SelectedValue & "' AND "
        End If
        If EnqLocationCode.SelectedValue <> "" Then
            st_WHR = st_WHR & "EnqLocationCode = '" & EnqLocationCode.SelectedValue & "' AND "
        End If
        If EnqUserID.SelectedValue <> "" Then
            st_WHR = st_WHR & "EnqUserID = '" & EnqUserID.SelectedValue & "' AND "
        End If
        If QuoLocationCode.SelectedValue <> "" Then
            st_WHR = st_WHR & "QuoLocationCode = '" & QuoLocationCode.SelectedValue & "' AND "
        End If
        If QuoUserID.SelectedValue <> "" Then
            st_WHR = st_WHR & "QuoUserID = '" & QuoUserID.SelectedValue & "' AND "
        End If
        If QuotedDateFrom.Text <> "" And QuotedDateTo.Text = "" Then
            st_WHR = st_WHR & "QuoTedDate = '" & QuotedDateFrom.Text & "' AND "
        End If
        If QuotedDateFrom.Text <> "" And QuotedDateTo.Text <> "" Then
            st_WHR = st_WHR & "QuoTedDate >= '" & QuotedDateFrom.Text & "' AND QuoTedDate <= '" & QuotedDateTo.Text & "' AND "
        End If
        If StatusChangeDateFrom.Text <> "" And StatusChangeDateTo.Text = "" Then
            st_WHR = st_WHR & "StatusChangeDate = '" & StatusChangeDateFrom.Text & "' AND "
        End If
        If StatusChangeDateFrom.Text <> "" And StatusChangeDateTo.Text <> "" Then
            st_WHR = st_WHR & "StatusChangeDate >= '" & StatusChangeDateFrom.Text & "' AND StatusChangeDate <= '" & StatusChangeDateTo.Text & "' AND "
        End If
        If PaymentTermCode.Text <> "" Then
            st_WHR = st_WHR & "PaymentTermCode = '" & PaymentTermCode.Text & "' AND "
        End If

        If st_WHR <> String.Empty Then
            st_SQL.Append("WHERE ")
            st_WHR = Left(st_WHR.ToString, st_WHR.Length - 4)
            st_SQL.Append("" & st_WHR & "")
        Else
            '検索条件が何も指定されなかった場合の対応
            st_SQL.Append("WHERE 1=0 ")
        End If

        st_SQL.Append("ORDER BY StatusSortOrder, QuotedDate DESC, StatusChangeDate DESC, RFQNumber")
        SrcRFQHeader.SelectCommand = st_SQL.ToString
        RFQHeaderList.DataBind()
        RFQHeaderList.Visible = True
    End Sub

    Protected Sub RFQHeaderList_PagePropertiesChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RFQHeaderList.PagePropertiesChanged
        SearchRFQHeader()
    End Sub

    Private Function getBaseRFQHeaderSQL() As String
        '[SrcRFQHeaderの値設定]-----------------------------------------------------------------
        Dim st_SQL As New Text.StringBuilder
        st_SQL.Append("SELECT ")
        st_SQL.Append("	RFQNumber, ")
        st_SQL.Append("	EnqLocationName, ")
        st_SQL.Append("	EnqUserName, ")
        st_SQL.Append("	QuoLocationName, ")
        st_SQL.Append("	QuoUserName, ")
        st_SQL.Append("	v_RFQHeader.ProductNumber, ")
        st_SQL.Append("	ProductName, ")
        st_SQL.Append("	SupplierName, ")
        st_SQL.Append("	sc1.Name AS SupplierCountryName, ")
        st_SQL.Append("	MakerName, ")
        st_SQL.Append("	sc2.Name AS MakerCountryName, ")
        st_SQL.Append("	Purpose, ")
        st_SQL.Append("	SupplierItemName, ")
        st_SQL.Append("	ShippingHandlingFee, ")
        st_SQL.Append("	ShippingHandlingCurrencyCode, ")
        st_SQL.Append("	v_RFQHeader.Comment, ")
        st_SQL.Append("	QuotedDate, ")
        st_SQL.Append("	v_RFQHeader.Status, ")
        st_SQL.Append("	StatusChangeDate, ")
        st_SQL.Append("	CASNumber ")
        st_SQL.Append("FROM ")
        st_SQL.Append(" v_RFQHeader INNER JOIN ")
        st_SQL.Append(" s_Country AS sc1 ON sc1.CountryCode = v_RFQHeader.SupplierCountryCode LEFT OUTER JOIN ")
        st_SQL.Append(" s_Country AS sc2 ON sc2.CountryCode = v_RFQHeader.MakerCountryCode INNER JOIN ")
        st_SQL.Append(" Product ON v_RFQHeader.ProductID = Product.ProductID ")
        Return st_SQL.ToString()
    End Function

    Protected Sub Clear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Clear.Click
        Msg.Text=""
        StatusSortOrderFrom.SelectedIndex = 0
        StatusSortOrderTo.SelectedIndex = 0
        EnqLocationCode.SelectedIndex = 0
        EnqUserID.SelectedIndex = -1
        EnqUserID.Items.Clear()
        QuoLocationCode.SelectedIndex = 0
        QuoUserID.SelectedIndex = -1
        QuoUserID.Items.Clear()
        PaymentTermCode.SelectedIndex = 0
    End Sub

    Protected Sub SrcRFQHeader_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcRFQHeader.Selecting
        '[本ページのタイムアウトを無限にする]---------------------------------------------------
        'e.Command.CommandTimeout = 0
    End Sub
End Class
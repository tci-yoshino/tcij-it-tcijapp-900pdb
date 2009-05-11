Option Explicit On
Option Strict On

Imports Purchase.Common

Partial Public Class RFQStatus
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader

    Const MinDate As String = "1900-01-01"       '検索最小日付
    Const SESSION_KEY_LOCATION As String = "LocationCode"
    Const DATE_ADJUST_HOUR As Integer = -12      '日差補正関数（TCI国際化対応12時間）

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
            StatusSortOrderFrom.Items.Add(New ListItem(String.Empty, String.Empty))
            StatusSortOrderTo.Items.Clear()
            StatusSortOrderTo.Items.Add(New ListItem(String.Empty, String.Empty))
            Do Until DBReader.Read = False
                StatusSortOrderFrom.Items.Add(New ListItem(DBReader("Text").ToString, DBReader("SortOrder").ToString))
                StatusSortOrderTo.Items.Add(New ListItem(DBReader("Text").ToString, DBReader("SortOrder").ToString))
            Loop
            DBReader.Close()

            '[EnqLocationCode,QuoLocationCodeの値設定]------------------------------------------
            DBCommand.CommandText = "SELECT LocationCode, Name FROM s_Location ORDER BY Name"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            EnqLocationCode.Items.Clear()
            EnqLocationCode.Items.Add(New ListItem(String.Empty, String.Empty))
            QuoLocationCode.Items.Clear()
            QuoLocationCode.Items.Add(New ListItem(String.Empty, String.Empty))
            Do Until DBReader.Read = False
                EnqLocationCode.Items.Add(New ListItem(DBReader("Name").ToString, DBReader("LocationCode").ToString))
                QuoLocationCode.Items.Add(New ListItem(DBReader("Name").ToString, DBReader("LocationCode").ToString))
            Loop
            DBReader.Close()

            '[PaymentTermCodeの値設定]----------------------------------------------------------
            DBCommand.CommandText = "SELECT Text, PaymentTermCode FROM PurchasingPaymentTerm ORDER BY PaymentTermCode"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            PaymentTermCode.Items.Clear()
            PaymentTermCode.Items.Add(New ListItem(String.Empty, String.Empty))
            Do Until DBReader.Read = False
                PaymentTermCode.Items.Add(New ListItem(DBReader("Text").ToString, DBReader("PaymentTermCode").ToString))
            Loop
            DBReader.Close()
            DBConn.Close()
            SrcRFQHeader.SelectCommand = String.Empty
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
        DBCommand.CommandText = "SELECT Name AS EnqUserName, EnqUserID FROM RFQHeader, v_UserAll WHERE RFQHeader.EnqUserID = v_UserAll.UserID AND EnqLocationCode = '" & EnqLocationCode.SelectedValue & "' Group BY EnqUserID, Name ORDER BY Name"
        DBConn.Open()
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        EnqUserID.Items.Clear()
        EnqUserID.Items.Add(New ListItem(String.Empty, String.Empty))
        Do Until DBReader.Read = False
            EnqUserID.Items.Add(New ListItem(DBReader("EnqUserName").ToString, DBReader("EnqUserID").ToString))
        Loop
        DBReader.Close()
        DBConn.Close()
    End Sub

    Protected Sub QuoLocationCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles QuoLocationCode.SelectedIndexChanged
        '[QuoUserIDの値設定]--------------------------------------------------------------------
        Msg.Text = String.Empty
        DBCommand = DBConn.CreateCommand()
        DBCommand.CommandText = "SELECT Name AS QuoUserName, QuoUserID FROM RFQHeader, v_UserAll WHERE RFQHeader.QuoUserID = v_UserAll.UserID AND QuoLocationCode = '" & QuoLocationCode.SelectedValue & "' Group BY QuoUserID, Name ORDER BY Name"
        DBConn.Open()
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        QuoUserID.Items.Clear()
        QuoUserID.Items.Add(New ListItem(String.Empty, String.Empty))
        Do Until DBReader.Read = False
            QuoUserID.Items.Add(New ListItem(DBReader("QuoUserName").ToString, DBReader("QuoUserID").ToString))
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
        SrcRFQHeader.SelectCommand = String.Empty
        RFQHeaderList.Visible = False

        '[Status設定順序チェック]---------------------------------------------------------------
        If StatusSortOrderFrom.Text = String.Empty And StatusSortOrderTo.Text <> String.Empty Then
            Msg.Text = "Current Status (from) " & ERR_REQUIRED_FIELD
            Exit Sub
        End If

        '[Dateを1Byte形式に変換する]------------------------------------------------------------
        QuotedDateFrom.Text = StrConv(QuotedDateFrom.Text, VbStrConv.Narrow)
        QuotedDateTo.Text = StrConv(QuotedDateTo.Text, VbStrConv.Narrow)
        StatusChangeDateFrom.Text = StrConv(StatusChangeDateFrom.Text, VbStrConv.Narrow)
        StatusChangeDateTo.Text = StrConv(StatusChangeDateTo.Text, VbStrConv.Narrow)

        '[日付妥当性チェック]-------------------------------------------------------------------
        If QuotedDateFrom.Text <> String.Empty And Not (IsDate(QuotedDateFrom.Text) And Regex.IsMatch(QuotedDateFrom.Text, DATE_REGEX_OPTIONAL)) Then
            Msg.Text = "Quoted Date (from)" & ERR_INVALID_DATE
            Exit Sub
        End If
        If QuotedDateTo.Text <> String.Empty And Not (IsDate(QuotedDateTo.Text) And Regex.IsMatch(QuotedDateTo.Text, DATE_REGEX_OPTIONAL)) Then
            Msg.Text = "Quoted Date (to)" & ERR_INVALID_DATE
            Exit Sub
        End If
        If StatusChangeDateFrom.Text <> String.Empty And Not (IsDate(StatusChangeDateFrom.Text) And Regex.IsMatch(StatusChangeDateFrom.Text, DATE_REGEX_OPTIONAL)) Then
            Msg.Text = "Status Change Date (from)" & ERR_INVALID_DATE
            Exit Sub
        End If
        If StatusChangeDateTo.Text <> String.Empty And Not (IsDate(StatusChangeDateTo.Text) And Regex.IsMatch(StatusChangeDateTo.Text, DATE_REGEX_OPTIONAL)) Then
            Msg.Text = "Status Change Date (to)" & ERR_INVALID_DATE
            Exit Sub
        End If

        '[最小日付チェック(1900-01-01以下エラー)]-----------------------------------------------
        If QuotedDateFrom.Text <> String.Empty And QuotedDateFrom.Text < MinDate Then
            Msg.Text = "Quoted Date (from)" & ERR_INVALID_DATE
            Exit Sub
        End If
        If QuotedDateTo.Text <> String.Empty And QuotedDateTo.Text < MinDate Then
            Msg.Text = "Quoted Date (to)" & ERR_INVALID_DATE
            Exit Sub
        End If

        If StatusChangeDateFrom.Text <> String.Empty And StatusChangeDateFrom.Text < MinDate Then
            Msg.Text = "Status Change Date (from)" & ERR_INVALID_DATE
            Exit Sub
        End If
        If StatusChangeDateTo.Text <> String.Empty And StatusChangeDateTo.Text < MinDate Then
            Msg.Text = "Status Change Date (to)" & ERR_INVALID_DATE
            Exit Sub
        End If

        '[日付設定順序チェック]-----------------------------------------------------------------
        If QuotedDateFrom.Text = String.Empty And QuotedDateTo.Text <> String.Empty Then
            Msg.Text = "Quoted Date (from)" & ERR_REQUIRED_FIELD
            Exit Sub
        End If
        If StatusChangeDateFrom.Text = String.Empty And StatusChangeDateTo.Text <> String.Empty Then
            Msg.Text = "Status Change Date (from)" & ERR_REQUIRED_FIELD
            Exit Sub
        End If

        Dim s_LocationCode As String = Session(SESSION_KEY_LOCATION).ToString()
        Dim s_QuotedDateFromStart As String = String.Empty
        Dim s_QuotedDateFromEnd As String = String.Empty
        Dim s_QuotedDateToStart As String = String.Empty    '値は求めているが利用はしていない
        Dim s_QuotedDateToEnd As String = String.Empty
        If QuotedDateFrom.Text <> String.Empty Then
            '[QuotedDateFromから日差補正後のs_QuotedDateFromStartを求める]----------------------
            Dim dt_QuotedDateFrom As DateTime = CType(GetDatabaseTime(s_LocationCode, QuotedDateFrom.Text), Date).AddHours(DATE_ADJUST_HOUR)
            s_QuotedDateFromStart = dt_QuotedDateFrom.ToString("yyyy-MM-dd HH:mm:ss")
            '[更に1日後のs_QuotedDateFromEndを求める]-------------------------------------------
            s_QuotedDateFromEnd = dt_QuotedDateFrom.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")
        End If
        If QuotedDateTo.Text <> String.Empty Then
            '[QuotedDateToから日差補正後のs_QuotedDateToStartを求める]--------------------------
            Dim dt_QuotedDateTo As DateTime = CType(GetDatabaseTime(s_LocationCode, QuotedDateTo.Text), Date).AddHours(DATE_ADJUST_HOUR)
            s_QuotedDateToStart = dt_QuotedDateTo.ToString("yyyy-MM-dd HH:mm:ss")
            '[更に1日後のs_QuotedDateToEndを求める]---------------------------------------------
            s_QuotedDateToEnd = dt_QuotedDateTo.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")
        End If

        Dim s_StatusChangeDateFromStart As String = String.Empty
        Dim s_StatusChangeDateFromEnd As String = String.Empty
        Dim s_StatusChangeDateToStart As String = String.Empty     '値は求めているが利用はしていない
        Dim s_StatusChangeDateToEnd As String = String.Empty
        If StatusChangeDateFrom.Text <> String.Empty Then
            '[StatusChangeDateFromから日差補正後のs_StatusChangeDateFromStartを求める]----------
            Dim dt_StatusChangeDateFrom As DateTime = CType(GetDatabaseTime(s_LocationCode, StatusChangeDateFrom.Text), Date)
            s_StatusChangeDateFromStart = dt_StatusChangeDateFrom.ToString("yyyy-MM-dd HH:mm:ss")
            '[更に1日後のs_StatusChangeDateFromEndを求める]-------------------------------------
            s_StatusChangeDateFromEnd = dt_StatusChangeDateFrom.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")
        End If
        If StatusChangeDateTo.Text <> String.Empty Then
            '[StatusChangeDateToから日差補正後のs_StatusChangeDateToStartを求める]--------------
            Dim dt_StatusChangeDateTo As DateTime = CType(GetDatabaseTime(s_LocationCode, StatusChangeDateTo.Text), Date)
            s_StatusChangeDateToStart = dt_StatusChangeDateTo.ToString("yyyy-MM-dd HH:mm:ss")
            '[更に1日後のs_StatusChangeDateToEndを求める]---------------------------------------
            s_StatusChangeDateToEnd = dt_StatusChangeDateTo.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")
        End If

        'WHERE句の作成
        Dim st_WHR As String = String.Empty
        If StatusSortOrderFrom.SelectedValue <> String.Empty And StatusSortOrderTo.SelectedValue = String.Empty Then
            st_WHR &= "StatusSortOrder = '" & StatusSortOrderFrom.SelectedValue & "' AND "
        End If
        If StatusSortOrderFrom.SelectedValue <> String.Empty And StatusSortOrderTo.SelectedValue <> String.Empty Then
            st_WHR &= "StatusSortOrder >= '" & StatusSortOrderFrom.SelectedValue & "' AND StatusSortOrder <= '" & StatusSortOrderTo.SelectedValue & "' AND "
        End If
        If EnqLocationCode.SelectedValue <> String.Empty Then
            st_WHR &= "EnqLocationCode = '" & EnqLocationCode.SelectedValue & "' AND "
        End If
        If EnqUserID.SelectedValue <> String.Empty Then
            st_WHR &= "EnqUserID = " & EnqUserID.SelectedValue & " AND "
        End If
        If QuoLocationCode.SelectedValue <> String.Empty Then
            st_WHR &= "QuoLocationCode = '" & QuoLocationCode.SelectedValue & "' AND "
        End If
        If QuoUserID.SelectedValue <> String.Empty Then
            st_WHR &= "QuoUserID = '" & QuoUserID.SelectedValue & "' AND "
        End If
        If QuotedDateFrom.Text <> String.Empty And QuotedDateTo.Text = String.Empty Then
            st_WHR &= "QuoTedDate >= '" & s_QuotedDateFromStart & "' AND QuoTedDate < '" & s_QuotedDateFromEnd & "' AND "
        End If
        If QuotedDateFrom.Text <> String.Empty And QuotedDateTo.Text <> String.Empty Then
            st_WHR &= "QuoTedDate >= '" & s_QuotedDateFromStart & "' AND QuoTedDate < '" & s_QuotedDateToEnd & "' AND "
        End If
        If StatusChangeDateFrom.Text <> String.Empty And StatusChangeDateTo.Text = String.Empty Then
            st_WHR &= "StatusChangeDate >= '" & s_StatusChangeDateFromStart & "' AND StatusChangeDate < '" & s_StatusChangeDateFromEnd & "' AND "
        End If
        If StatusChangeDateFrom.Text <> String.Empty And StatusChangeDateTo.Text <> String.Empty Then
            st_WHR &= "StatusChangeDate >= '" & s_StatusChangeDateFromStart & "' AND StatusChangeDate <= '" & s_StatusChangeDateToEnd & "' AND "
        End If
        If PaymentTermCode.Text <> String.Empty Then
            st_WHR = st_WHR & "PaymentTermCode = '" & PaymentTermCode.Text & "' AND "
        End If

        '[SrcRFQHeaderの値設定]-----------------------------------------------------------------
        Dim st_SQL As New Text.StringBuilder
        st_SQL.Append(getBaseRFQHeaderSQL())

        If st_WHR <> String.Empty Then
            '[検索結果数の確認]-----------------------------------------------------------------
            DBCommand = DBConn.CreateCommand()
            DBConn.Open()
            st_WHR = "WHERE " & Left(st_WHR, st_WHR.Length - 4)   'st_WHRの最後の'AND 'を取り除く
            DBCommand.CommandText = getCountRFQHeaderSQL() & st_WHR & " OPTION(FORCE ORDER)"
            Dim i_RFQCount As Integer = CInt(DBCommand.ExecuteScalar())
            DBConn.Close()
            If i_RFQCount > 1000 Then
                Msg.Text = Common.MSG_RESULT_OVER_1000
                Exit Sub
            Else
                st_SQL.Append(st_WHR)
            End If
        Else
            '検索条件が何も指定されなかった場合の対応
            st_SQL.Append("WHERE 1=0 ")
        End If

        st_SQL.Append("ORDER BY StatusSortOrder, QuotedDate DESC, StatusChangeDate DESC, RFQNumber")
        st_SQL.Append(" OPTION(FORCE ORDER)")
        SrcRFQHeader.SelectCommand = st_SQL.ToString
        RFQHeaderList.DataBind()
        RFQHeaderList.Visible = True
    End Sub

    Protected Sub RFQHeaderList_PagePropertiesChanged(ByVal sender As Object, ByVal e As EventArgs) Handles RFQHeaderList.PagePropertiesChanged
        SearchRFQHeader()
    End Sub

    Private Function getCountRFQHeaderSQL() As String
        '[SrcRFQHeaderの値設定]-----------------------------------------------------------------
        Dim st_SQL As New Text.StringBuilder
        st_SQL.Append("SELECT COUNT(*) AS RFQCount ")
        st_SQL.Append("FROM ")
        st_SQL.Append(" v_RFQheader INNER JOIN ")
        st_SQL.Append(" s_Country AS sc1 ON sc1.CountryCode = v_RFQheader.SupplierCountryCode LEFT OUTER JOIN ")
        st_SQL.Append(" s_Country AS sc2 ON sc2.CountryCode = v_RFQheader.MakerCountryCode INNER JOIN ")
        st_SQL.Append(" Product ON v_RFQheader.ProductID = Product.ProductID ")
        Return st_SQL.ToString()
    End Function

    Private Function getBaseRFQHeaderSQL() As String
        '[SrcRFQHeaderの値設定]-----------------------------------------------------------------
        Dim st_SQL As New Text.StringBuilder
        st_SQL.Append("SELECT ")
        st_SQL.Append("	RFQNumber, ")
        st_SQL.Append("	EnqLocationName, ")
        st_SQL.Append("	EnqUserName, ")
        st_SQL.Append("	QuoLocationName, ")
        st_SQL.Append("	QuoUserName, ")
        st_SQL.Append("	v_RFQheader.ProductNumber, ")
        st_SQL.Append("	ProductName, ")
        st_SQL.Append("	SupplierName, ")
        st_SQL.Append("	sc1.Name AS SupplierCountryName, ")
        st_SQL.Append("	MakerName, ")
        st_SQL.Append("	sc2.Name AS MakerCountryName, ")
        st_SQL.Append("	Purpose, ")
        st_SQL.Append("	SupplierItemName, ")
        st_SQL.Append("	ShippingHandlingFee, ")
        st_SQL.Append("	ShippingHandlingCurrencyCode, ")
        st_SQL.Append("	v_RFQheader.Comment, ")
        st_SQL.Append("	QuotedDate, ")
        st_SQL.Append("	v_RFQheader.Status, ")
        st_SQL.Append("	StatusChangeDate, ")
        st_SQL.Append("	CASNumber ")
        st_SQL.Append("FROM ")
        st_SQL.Append(" v_RFQheader INNER JOIN ")
        st_SQL.Append(" s_Country AS sc1 ON sc1.CountryCode = v_RFQheader.SupplierCountryCode LEFT OUTER JOIN ")
        st_SQL.Append(" s_Country AS sc2 ON sc2.CountryCode = v_RFQheader.MakerCountryCode INNER JOIN ")
        st_SQL.Append(" Product ON v_RFQheader.ProductID = Product.ProductID ")
        Return st_SQL.ToString()
    End Function

    Protected Sub Clear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Clear.Click
        Msg.Text = String.Empty
        StatusSortOrderFrom.SelectedIndex = 0
        StatusSortOrderTo.SelectedIndex = 0
        EnqLocationCode.SelectedIndex = 0
        EnqUserID.Items.Clear()
        QuoLocationCode.SelectedIndex = 0
        QuoUserID.Items.Clear()
        PaymentTermCode.SelectedIndex = 0
    End Sub

    Protected Sub SrcRFQHeader_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcRFQHeader.Selecting
        'HACK 応答速度が充分得られないため、暫定的に180秒（3分）に変更　      
        e.Command.CommandTimeout = 180
    End Sub
End Class
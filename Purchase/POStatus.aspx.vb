Option Explicit On
Option Strict On

Imports Purchase.Common

Partial Public Class POStatus
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            '[ERR_NO_MATCH_FOUND表示防止]-------------------------------------------------------
            POList.Visible = False

            '[StatusSortOrderFrom,StatusSortOrderToの値設定]------------------------------------
            DBCommand = DBConn.CreateCommand()
            DBConn.Open()
            DBCommand.CommandText = "SELECT Text, SortOrder FROM POStatus ORDER BY SortOrder"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            StatusSortOrderFrom.Items.Clear()
            StatusSortOrderFrom.Items.Add(New ListItem("", ""))
            StatusSortOrderTo.Items.Clear()
            StatusSortOrderTo.Items.Add(New ListItem("", ""))
            Do Until DBReader.Read = False
                StatusSortOrderFrom.Items.Add(New ListItem(DBReader("Text").ToString, DBReader("SortOrder").ToString))
                StatusSortOrderTo.Items.Add(New ListItem(DBReader("Text").ToString, DBReader("SortOrder").ToString))
            Loop
            DBReader.Close()

            '[POLocationCodeの値設定]-----------------------------------------------------------
            DBCommand.CommandText = "SELECT LocationCode, Name FROM s_Location ORDER BY Name"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            POLocationCode.Items.Clear()
            POLocationCode.Items.Add(New ListItem("", ""))
            Do Until DBReader.Read = False
                POLocationCode.Items.Add(New ListItem(DBReader("Name").ToString, DBReader("LocationCode").ToString))
            Loop
            DBReader.Close()
        End If
    End Sub

    Protected Sub POLocationCode_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles POLocationCode.SelectedIndexChanged
        '[POUserIDの値設定]--------------------------------------------------------------------
        Msg.Text = String.Empty
        DBCommand = DBConn.CreateCommand()
        DBCommand.CommandText = "SELECT v_User.Name AS POUserName, PO.POUserID FROM PO INNER JOIN v_User ON PO.POUserID = v_User.UserID WHERE PO.POLocationCode = '" & POLocationCode.SelectedValue & "' GROUP BY PO.POUserID, v_User.Name ORDER BY POUserName"
        DBConn.Open()
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        POUserID.Items.Clear()
        POUserID.Items.Add(New ListItem("", ""))
        Do Until DBReader.Read = False
            POUserID.Items.Add(New ListItem(DBReader("POUserName").ToString, DBReader("POUserID").ToString))
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

        '[POPagerCountTop,POPagerCountBottomのカウントを1にする為実行]--------------------------
        SrcPO.SelectCommand = getBasePOSQL() + "WHERE 1=0"
        POList.DataBind()

        SearchPO()
    End Sub

    Private Sub SearchPO()
        Msg.Text = String.Empty
        SrcPO.SelectCommand = ""
        POList.Visible = False

        '[Status設定順序チェック]---------------------------------------------------------------
        If StatusSortOrderFrom.Text = "" And StatusSortOrderTo.Text <> "" Then
            Msg.Text = ""
            Exit Sub
        End If

        If StatusSortOrderFrom.Text <> "" And StatusSortOrderTo.Text <> "" Then
            If StatusSortOrderTo.Text < StatusSortOrderFrom.Text Then
                Msg.Text = ""
                Exit Sub
            End If
        End If

        '[入力データを1Byte形式に変換する]------------------------------------------------------
        SupplierCode.Text = Trim(StrConv(SupplierCode.Text, VbStrConv.Narrow))
        SupplierName.Text = Trim(SupplierName.Text)
        PODateFrom.Text = StrConv(PODateFrom.Text, VbStrConv.Narrow)
        PODateTo.Text = StrConv(PODateTo.Text, VbStrConv.Narrow)

        '[SupplierCodeの数字構成チェック]-------------------------------------------------------
        If Not Regex.IsMatch(SupplierCode.Text, DECIMAL_10_REGEX_OPTIONAL) Then
            Msg.Text = "Supplier Code" & ERR_INVALID_NUMBER
            Exit Sub
        End If

        '[日付妥当性チェック]-------------------------------------------------------------------
        If PODateFrom.Text <> "" And Not (IsDate(PODateFrom.Text) And Regex.IsMatch(PODateFrom.Text, DATE_REGEX_OPTIONAL)) Then
            Msg.Text = "PO Date (From) " & ERR_INVALID_DATE
            Exit Sub
        End If
        If PODateTo.Text <> "" And Not (IsDate(PODateTo.Text) And Regex.IsMatch(PODateTo.Text, DATE_REGEX_OPTIONAL)) Then
            Msg.Text = "PO Date (To) " & ERR_INVALID_DATE
            Exit Sub
        End If

        '[最小日付チェック(1900-01-01以下エラー)]-----------------------------------------------
        If PODateFrom.Text <> "" And PODateFrom.Text < "1900-01-01" Then
            Msg.Text = "PO Date (From) " & ERR_INVALID_DATE
            Exit Sub
        End If

        '[日付設定順序チェック]-----------------------------------------------------------------
        If PODateFrom.Text = "" And PODateTo.Text <> "" Then
            Msg.Text = ""
            Exit Sub
        End If
        If PODateFrom.Text <> "" And PODateTo.Text <> "" Then
            If PODateTo.Text < PODateFrom.Text Then
                Msg.Text = ""
                Exit Sub
            End If
        End If

        Const SESSION_KEY_LOCATION As String = "LocationCode"
        Dim s_LocationCode As String = Session(SESSION_KEY_LOCATION).ToString()

        '日差補正関数（TCI国際化対応12時間）
        Const DATE_ADJUST_HOUR As Integer = -12

        Dim s_PODateFromStart As String = String.Empty
        Dim s_PODateFromEnd As String = String.Empty
        Dim s_PODateToStart As String = String.Empty   '値は求めているが利用はしていない
        Dim s_PODateToEnd As String = String.Empty
        If PODateFrom.Text <> "" Then
            '[PODateFromから日差補正後のs_PODateFromStartを求める]------------------------------
            Dim dt_PODateFrom As DateTime = CType(GetDatabaseTime(s_LocationCode, PODateFrom.Text), Date).AddHours(DATE_ADJUST_HOUR)
            s_PODateFromStart = dt_PODateFrom.ToString("yyyy-MM-dd HH:mm:ss")
            '[更に1日後のs_PODateFromEndを求める]-----------------------------------------------
            s_PODateFromEnd = dt_PODateFrom.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")
        End If
        If PODateTo.Text <> "" Then
            '[PODateToから日差補正後のs_PODateToStartを求める]----------------------------------
            Dim dt_PODateTo As DateTime = CType(GetDatabaseTime(s_LocationCode, PODateTo.Text), Date).AddHours(DATE_ADJUST_HOUR)
            s_PODateToStart = dt_PODateTo.ToString("yyyy-MM-dd HH:mm:ss")
            '[更に1日後のs_PODateToEndを求める]-------------------------------------------------
            s_PODateToEnd = dt_PODateTo.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss")
        End If

        '[SrcPOの値設定]------------------------------------------------------------------------
        Dim st_SQL As New Text.StringBuilder
        st_SQL.Append(getBasePOSQL())

        'WHERE句の作成
        Dim st_WHR As String = String.Empty
        If StatusSortOrderFrom.SelectedValue <> "" And StatusSortOrderTo.SelectedValue = "" Then
            st_WHR &= "StatusSortOrder = '" & StatusSortOrderFrom.SelectedValue & "' AND "
        End If
        If StatusSortOrderFrom.SelectedValue <> "" And StatusSortOrderTo.SelectedValue <> "" Then
            st_WHR &= "StatusSortOrder >= '" & StatusSortOrderFrom.SelectedValue & "' AND StatusSortOrder <= '" & StatusSortOrderTo.SelectedValue & "' AND "
        End If
        If POLocationCode.SelectedValue <> "" Then
            st_WHR &= "POLocationCode = '" & POLocationCode.SelectedValue & "' AND "
        End If
        If POUserID.SelectedValue <> "" Then
            st_WHR &= "POUserID = '" & POUserID.SelectedValue & "' AND "
        End If
        If SupplierCode.Text <> "" Then
            st_WHR &= "SupplierCode = " & SupplierCode.Text & " AND "
        End If
        If SupplierName.Text <> "" Then
            st_WHR &= "SupplierName LIKE '%" & SafeSqlLikeClauseLiteral(SupplierName.Text) & "%' AND "
        End If
        If PODateFrom.Text <> "" And PODateTo.Text = "" Then
            st_WHR &= "PODate >= '" & s_PODateFromStart & "' AND PODate < '" & s_PODateFromEnd & "' AND "
        End If
        If PODateFrom.Text <> "" And PODateTo.Text <> "" Then
            st_WHR &= "PODate >= '" & s_PODateFromStart & "' AND PODate < '" & s_PODateToEnd & "' AND "
        End If

        If st_WHR <> String.Empty Then
            st_SQL.Append("WHERE ")
            st_WHR = Left(st_WHR, st_WHR.Length - 4)
            st_SQL.Append(st_WHR)
        Else
            '検索条件が何も指定されなかった場合の対応
            st_SQL.Append("WHERE 1=0 ")
        End If

        st_SQL.Append("ORDER BY ")
        st_SQL.Append(" PONumber ")
        SrcPO.SelectCommand = st_SQL.ToString
        POList.DataBind()
        POList.Visible = True
    End Sub

    Protected Sub Clear_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Clear.Click
        Msg.Text = ""
        StatusSortOrderFrom.SelectedIndex = 0
        StatusSortOrderTo.SelectedIndex = 0
        POLocationCode.SelectedIndex = 0
        POUserID.SelectedIndex = -1
        POUserID.Items.Clear()
    End Sub

    Private Function getBasePOSQL() As String
        '[SrcPOの値設定]------------------------------------------------------------------------
        Dim st_SQL As New Text.StringBuilder
        st_SQL.Append("SELECT ")
        st_SQL.Append("	PONumber, ")
        st_SQL.Append("	StatusChangeDate, ")
        st_SQL.Append("	Status, ")
        st_SQL.Append("	ProductNumber, ")
        st_SQL.Append("	ProductName, ")
        st_SQL.Append("	PODate, ")
        st_SQL.Append("	POUserName, ")
        st_SQL.Append("	POLocationName, ")
        st_SQL.Append("	SupplierName, ")
        st_SQL.Append("	MakerName, ")
        st_SQL.Append("	DeliveryDate, ")
        st_SQL.Append("	OrderQuantity, ")
        st_SQL.Append("	OrderUnitCode, ")
        st_SQL.Append("	CurrencyCode, ")
        st_SQL.Append("	UnitPrice, ")
        st_SQL.Append("	PerQuantity, ")
        st_SQL.Append("	PerUnitCode ")
        st_SQL.Append("FROM ")
        st_SQL.Append(" v_PO ")
        Return st_SQL.ToString()
    End Function

    Protected Sub SrcPO_Selecting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.SqlDataSourceSelectingEventArgs) Handles SrcPO.Selecting
        '[本ページのタイムアウトを無限にする]---------------------------------------------------
        'e.Command.CommandTimeout = 0
    End Sub

    Protected Sub POList_PagePropertiesChanged(ByVal sender As Object, ByVal e As EventArgs) Handles POList.PagePropertiesChanged
        SearchPO()
    End Sub
End Class
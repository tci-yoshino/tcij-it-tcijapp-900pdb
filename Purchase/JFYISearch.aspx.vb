Option Explicit On
Option Infer Off
Option Strict On

Imports System.Text
Imports Purchase.Common
Imports System.Text.RegularExpressions
Imports System.Data.SqlClient

Partial Public Class JFYISearch
    Inherits CommonPage

#Region "グローバル変数定義"
    Protected st_Action As String = String.Empty
    Protected b_FormVisible As Boolean = True
#End Region

#Region "定数定義"
    Const QUERY_KEY_ACTION As String = "Action"

    Const SESSION_KEY_ADMIN As String = "Purchase.isAdmin"
    Const SESSION_KEY_LOCATION As String = "LocationCode"
    
    Const ACTION_VALUE_SEARCH As String = "Search"

    Const PURPOSE_CODE_JFYI As String = "JFYI"

#End Region


    ''' <summary>
    ''' このページのロードイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Actionの取得
        If Not (Request.QueryString(QUERY_KEY_ACTION) Is Nothing) Then
            st_Action = Request.QueryString(QUERY_KEY_ACTION).ToString()
        ElseIf Not (Request.Form(QUERY_KEY_ACTION) Is Nothing) Then
            st_Action = Request.Form(QUERY_KEY_ACTION).ToString()
        End If

        '初回時の Common.ERR_NO_MATCH_FOUND 表示抑制 Actionパラメータ保存 
        If IsPostBack = True Then
            RFQHeaderList.Visible = True
        Else
            RFQHeaderList.Visible = False
        End If

    End Sub

    ''' <summary>
    ''' Search ボタンのクリックイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks></remarks>
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click

        'エラー発生時のリスト不可視化
        RFQHeaderList.Visible = False
        Msg.Text = String.Empty

        'Actionパラメータ検証
        If st_Action <> ACTION_VALUE_SEARCH Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            b_FormVisible = False
            Exit Sub
        End If


        '入力値検証
        If QuotedDateFrom.Text = String.Empty And QuotedDateTo.Text = String.Empty Then
            Msg.Text = "Quoted Date" & ERR_REQUIRED_FIELD
            Return
        End If

        If QuotedDateFrom.Text = String.Empty And QuotedDateTo.Text <> String.Empty Then
            Msg.Text = "Quoted Date (from)" & ERR_REQUIRED_FIELD
            Return
        End If


        If ValidateDateTextBox(QuotedDateFrom, False) = False Then
            Msg.Text = "Quoted Date (from)" & ERR_INVALID_DATE
            Return
        End If
        If ValidateDateTextBox(QuotedDateTo, True) = False Then
            Msg.Text = "Quoted Date (to)" & ERR_INVALID_DATE
            Return
        End If

        Dim b_useToDate As Boolean
        If QuotedDateTo.Text.Trim = String.Empty Then
            b_useToDate = False
        Else
            b_useToDate = True
        End If

        SrcRFQHeader.SelectCommand = CreateSQL_For_Select_RFQHeader(b_useToDate)
        SrcRFQHeader.SelectParameters.Clear()
        SrcRFQHeader.SelectParameters.Add("PurposeCode", PURPOSE_CODE_JFYI)
        Dim s_LocationCode As String = Session(SESSION_KEY_LOCATION).ToString()
        SrcRFQHeader.SelectParameters.Add("QuotedDateFrom", GetDatabaseTime(s_LocationCode, QuotedDateFrom.Text).ToString())
        If b_useToDate = True Then
            SrcRFQHeader.SelectParameters.Add("QuotedDateTo", GetDatabaseTime(s_LocationCode, QuotedDateTo.Text).ToString())
        End If

        'エラー未発生時のリスト可視化
        RFQHeaderList.Visible = True

    End Sub


    ''' <summary>
    ''' RFQヘッダーを取得するSQL文字列を生成します。
    ''' </summary>
    ''' <returns>生成したSQL文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateSQL_For_Select_RFQHeader(ByVal useToDate As Boolean) As String

        Dim sb_SQL As StringBuilder = New StringBuilder()

        sb_SQL.Append("SELECT ")
        sb_SQL.Append("	RH.RFQNumber, ")
        sb_SQL.Append("	RH.QuotedDate, ")
        sb_SQL.Append("	RH.StatusChangeDate, ")
        sb_SQL.Append("	RH.Status, ")
        sb_SQL.Append("	RH.ProductNumber,")
        sb_SQL.Append("	RH.ProductName, ")
        sb_SQL.Append("	RH.SupplierName, ")
        sb_SQL.Append("	RH.Purpose, ")
        sb_SQL.Append("	RH.MakerName, ")
        sb_SQL.Append("	RH.SupplierItemName, ")
        sb_SQL.Append("	RH.ShippingHandlingFee, ")
        sb_SQL.Append("	RH.ShippingHandlingCurrencyCode, ")
        sb_SQL.Append("	RH.EnqUserName, ")
        sb_SQL.Append("	RH.EnqLocationName, ")
        sb_SQL.Append("	RH.QuoUserName, ")
        sb_SQL.Append("	RH.QuoLocationName, ")
        sb_SQL.Append("	RH.Comment, ")
        sb_SQL.Append("	C.[Name] AS MakerCountryName, ")
        sb_SQL.Append("	CS.[Name] AS SupplierCountryName ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("	v_RFQHeader AS RH INNER JOIN ")
        sb_SQL.Append("	s_Country AS CS ON CS.CountryCode = RH.SupplierCountryCode LEFT OUTER JOIN ")
        sb_SQL.Append("	s_Country AS C ON C.CountryCode = RH.MakerCountryCode ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("	RH.PurposeCode = @PurposeCode ")

        If useToDate = True Then
            sb_SQL.Append("	AND RH.QuotedDate >= @QuotedDateFrom ")
            sb_SQL.Append("	AND RH.QuotedDate < DATEADD(d, 1, @QuotedDateTo) ")
        Else
            sb_SQL.Append("	AND RH.QuotedDate >= @QuotedDateFrom ")
            sb_SQL.Append("	AND RH.QuotedDate < DATEADD(d, 1, @QuotedDateFrom) ")
        End If

        sb_SQL.Append("ORDER BY ")
        sb_SQL.Append("	QuotedDate DESC, StatusChangeDate DESC, RFQNumber ASC ")

        Return sb_SQL.ToString()

    End Function

    ''' <summary>
    ''' RFQヘッダーのバインドイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks>RFQラインの条件を設定しバインドします</remarks>
    Protected Sub GetRFQLine(ByVal sender As Object, ByVal e As ListViewItemEventArgs) Handles RFQHeaderList.ItemDataBound
        Dim lv As ListView = CType(e.Item.FindControl("RFQLineList"), ListView)
        Dim src As SqlDataSource = CType(e.Item.FindControl("SrcRFQLine"), SqlDataSource)
        Dim label As Label = CType(e.Item.FindControl("RFQNumber"), Label)

        src.SelectParameters.Clear()
        src.SelectParameters.Add("RFQNumber", label.Text)
        src.SelectCommand = CreateSQL_For_Select_RFQLine()
        lv.DataSourceID = src.ID
        lv.DataBind()
    End Sub

    ''' <summary>
    ''' RFQラインを取得するSQL文字列を生成します。
    ''' </summary>
    ''' <returns>生成したSQL文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateSQL_For_Select_RFQLine() As String

        Dim sb_SQL As StringBuilder = New StringBuilder()

        sb_SQL.Append("SELECT ")
        sb_SQL.Append("	distinct ")
        sb_SQL.Append("	RL.RFQLineNumber, ")
        sb_SQL.Append("	RL.EnqQuantity, ")
        sb_SQL.Append("	RL.EnqUnitCode, ")
        sb_SQL.Append("	RL.EnqPiece, ")
        sb_SQL.Append("	RL.CurrencyCode, ")
        sb_SQL.Append("	RL.UnitPrice, ")
        sb_SQL.Append("	RL.QuoPer, ")
        sb_SQL.Append("	RL.QuoUnitCode, ")
        sb_SQL.Append("	RL.LeadTime, ")
        sb_SQL.Append("	RL.Packing, ")
        sb_SQL.Append("	RL.Purity, ")
        sb_SQL.Append("	RL.QMMethod, ")
        sb_SQL.Append("	PO.RFQLineNumber AS PO ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("	v_RFQLine AS RL LEFT OUTER JOIN ")
        sb_SQL.Append("	PO ON PO.RFQLineNumber = RL.RFQLineNumber ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("	RL.RFQNumber = @RFQNumber ")

        Return sb_SQL.ToString()

    End Function
End Class
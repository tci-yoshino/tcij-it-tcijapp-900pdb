﻿Option Explicit On
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
    Const SESSION_KEY_USER_ID As String = "UserID"

    Const ACTION_VALUE_UPDATE As String = "Update"
    Const ACTION_VALUE_CANCEL As String = "Cancel"
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
            b_FormVisible = True
        Else
            b_FormVisible = False
            Action.Value = ACTION_VALUE_SEARCH
        End If

    End Sub

    ''' <summary>
    ''' Search ボタンのクリックイベントです。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click

        '入力値検証
        If QuotedDateFrom.Text <> "" Then
            Msg.Text = ""
            Return
        End If

        If ValidateDateTextBox(QuotedDateFrom, False) = False Then
            Msg.Text = ""
            Return
        End If
        If ValidateDateTextBox(QuotedDateTo, False) = False Then
            Msg.Text = ""
            Return
        End If

        SrcRFQHeader.SelectCommand = CreateSQL_For_Select_RFQHeader()
        SrcRFQHeader.SelectParameters.Add("PurposeCode", PURPOSE_CODE_JFYI)
        'TODO 時差換算関数の実装が必要です。
        SrcRFQHeader.SelectParameters.Add("QuotedDateFrom", QuotedDateFrom.Text)
        SrcRFQHeader.SelectParameters.Add("QuotedDateTo", QuotedDateTo.Text)

        RFQHeaderList.DataBind()

    End Sub


    ''' <summary>
    ''' RFQヘッダーを取得するSQL文字列を生成します。
    ''' </summary>
    ''' <returns>生成したSQL文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateSQL_For_Select_RFQHeader() As String

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
        sb_SQL.Append("	RH.PurposeCode = @PurposeCode AND")
        sb_SQL.Append("	RH.QuotedDate <= @QuotedDateFrom AND ")
        sb_SQL.Append("	RH.QuotedDate >= @QuotedDateTo ")
        sb_SQL.Append("ORDER BY ")
        sb_SQL.Append("	QuotedDate DESC, StatusChangeDate DESC, RFQNumber ASC ")

        Return sb_SQL.ToString()

    End Function

    ''' <summary>
    ''' RFQヘッダーのバインドイベントです。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
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
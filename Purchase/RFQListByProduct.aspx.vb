﻿Option Explicit On
Option Infer Off
Option Strict On

Imports System.Data.SqlClient
Imports Purchase.Common

''' <summary>
''' RFQListByProductクラス
''' </summary>
''' <remarks>製品からRFQ一覧を表示します</remarks>
Partial Public Class RFQListByProduct
    Inherits CommonPage

    Protected st_ProductID As String
    Protected i_DataNum As Integer = 0 ' 0 の場合は Supplier Data が無いと判断し、 Data not found. を表示する。

    ''' <summary>
    ''' このページのロードイベントです。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' パラメータ取得
        If Request.RequestType = "POST" Then
            st_ProductID = CType(IIf(Request.Form("ProductID") = Nothing, "", Request.Form("ProductID")), String)
        ElseIf Request.RequestType = "GET" Then
            st_ProductID = CType(IIf(Request.QueryString("ProductID") = Nothing, "", Request.QueryString("ProductID")), String)
        End If

        If st_ProductID = "" Or IsInteger(st_ProductID) = False Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        ' 空白除去
        st_ProductID = st_ProductID.Trim()
        SearchProduct(st_ProductID)
        SearchRFQHeader(st_ProductID)

    End Sub

    ''' <summary>
    ''' 製品の検索を行います。
    ''' </summary>
    ''' <param name="st_ProductID">製品ID</param>
    ''' <remarks></remarks>
    Private Sub SearchProduct(ByVal st_ProductID As String)

        Using connection As New SqlClient.SqlConnection(DB_CONNECT_STRING)

            Dim command As New SqlClient.SqlCommand(CreateProductHeaderSelectSQL(), connection)
            connection.Open()

            command.Parameters.AddWithValue("ProductID", st_ProductID)

            Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()

            If reader.Read() Then
                i_DataNum = 1
                ProductNumber.Text = reader("ProductNumber").ToString()
                If Not IsDBNull(reader("QuoName")) Then
                    QuoName.Text = reader("QuoName").ToString()
                Else
                    QuoName.Text = reader("Name").ToString()
                End If
                ProductName.Text = reader("Name").ToString()
                CASNumber.Text = reader("CASNumber").ToString()
                MolecularFormula.Text = reader("MolecularFormula").ToString()
            End If
            reader.Close()
        End Using
    End Sub


    ''' <summary>
    ''' 見積依頼の検索を行います。
    ''' </summary>
    ''' <param name="st_ProductID">製品ID</param>
    ''' <remarks></remarks>
    Private Sub SearchRFQHeader(ByVal st_ProductID As String)

        SrcRFQHeader.SelectCommand = CreateRFQHeaderSelectSQL()
        SrcRFQHeader.SelectParameters.Add("ProductID", st_ProductID)
        RFQHeaderList.DataBind()

    End Sub


    ''' <summary>
    ''' RFQ詳細ListItemのデータバインドイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NET規定値</param>
    ''' <param name="e">ASP.NET規定値</param>
    ''' <remarks></remarks>
    Protected Sub GetRFQLine(ByVal sender As Object, ByVal e As EventArgs) Handles RFQHeaderList.ItemDataBound
        Dim lv As ListView = CType(CType(e, ListViewItemEventArgs).Item.FindControl("RFQLineList"), ListView)
        Dim src As SqlDataSource = CType(CType(e, ListViewItemEventArgs).Item.FindControl("SrcRFQLine"), SqlDataSource)
        Dim link As HyperLink = CType(CType(e, System.Web.UI.WebControls.ListViewItemEventArgs).Item.FindControl("RFQNumber"), HyperLink)

        src.SelectParameters.Clear()
        src.SelectParameters.Add("RFQNumber", link.Text)
        src.SelectCommand = CreateRFQLineSelectSQL()
        lv.DataSourceID = src.ID
        lv.DataBind()
    End Sub


    ''' <summary>
    ''' 製品検索SQL文字列を生成します。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateProductHeaderSelectSQL() As String

        Dim sb_SQL As New Text.StringBuilder

        'SQL文字列の作成
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("	ProductNumber, ")
        sb_SQL.Append("	QuoName, ")
        sb_SQL.Append("	Name, ")
        sb_SQL.Append("	CASNumber, ")
        sb_SQL.Append("	MolecularFormula ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("	Product ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("	ProductID = @ProductID ")

        Return sb_SQL.ToString()

    End Function

    ''' <summary>
    ''' RFQヘッダー検索SQL文字列を生成します。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateRFQHeaderSelectSQL() As String

        Dim sb_SQL As New Text.StringBuilder

        'SQL文字列の作成
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("	rfh.StatusChangeDate, ")
        sb_SQL.Append("	rfh.Status, ")
        sb_SQL.Append("	rfh.RFQNumber, ")
        sb_SQL.Append("	rfh.QuotedDate, ")
        sb_SQL.Append("	rfh.SupplierName, ")
        sb_SQL.Append("	rfh.MakerCountryCode, ")
        sb_SQL.Append("	mcry.Name AS MakerCountryName, ")
        sb_SQL.Append("	rfh.Purpose, ")
        sb_SQL.Append("	rfh.MakerName, ")
        sb_SQL.Append("	rfh.SupplierCountryCode, ")
        sb_SQL.Append("	scry.Name AS SupplierCountryName, ")
        sb_SQL.Append("	rfh.SupplierItemName, ")
        sb_SQL.Append("	rfh.ShippingHandlingCurrencyCode,")
        sb_SQL.Append("	rfh.ShippingHandlingFee, ")
        sb_SQL.Append("	rfh.EnqUserName, ")
        sb_SQL.Append("	rfh.EnqLocationName, ")
        sb_SQL.Append("	rfh.QuoUserName, ")
        sb_SQL.Append("	rfh.QuoLocationName, ")
        sb_SQL.Append("	rfh.Comment ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("	v_RFQHeader rfh ")
        sb_SQL.Append("LEFT JOIN ")
        sb_SQL.Append("	s_Country mcry ")
        sb_SQL.Append("ON ")
        sb_SQL.Append("	rfh.MakerCountryCode = mcry.CountryCode, ")
        sb_SQL.Append("	s_Country scry ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("	rfh.SupplierCountryCode = scry.CountryCode ")
        sb_SQL.Append("	AND ProductID = @ProductID ")
        sb_SQL.Append("ORDER BY ")
        sb_SQL.Append(" rfh.QuotedDate DESC, ")
        sb_SQL.Append(" rfh.StatusChangeDate DESC, ")
        sb_SQL.Append(" rfh.RFQNumber ASC")

        Return sb_SQL.ToString()

    End Function


    ''' <summary>
    ''' RFQ詳細検索SQL文字列を生成します。
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateRFQLineSelectSQL() As String

        Dim sb_SQL As New Text.StringBuilder

        'SQL文字列の作成
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("	DISTINCT ")
        sb_SQL.Append("	rl.RFQNumber, ")
        sb_SQL.Append("	rl.RFQLineNumber, ")
        sb_SQL.Append("	rl.EnqQuantity, ")
        sb_SQL.Append("	rl.EnqUnitCode, ")
        sb_SQL.Append("	rl.EnqPiece, ")
        sb_SQL.Append("	rl.CurrencyCode, ")
        sb_SQL.Append("	rl.UnitPrice, ")
        sb_SQL.Append("	rl.QuoPer, ")
        sb_SQL.Append("	rl.QuoUnitCode, ")
        sb_SQL.Append("	rl.LeadTime, ")
        sb_SQL.Append("	rl.Packing, ")
        sb_SQL.Append("	rl.Purity, ")
        sb_SQL.Append("	rl.QMMethod, ")
        sb_SQL.Append("	PO.RFQLineNumber AS PO ")
        sb_SQL.Append("FROM  ")
        sb_SQL.Append("	RFQLine rl")
        sb_SQL.Append("	LEFT OUTER JOIN ")
        sb_SQL.Append("	PO ")
        sb_SQL.Append("	ON ")
        sb_SQL.Append("PO.RFQLineNumber = RL.RFQLineNumber ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("	RFQNumber = @RFQNumber ")

        Return sb_SQL.ToString()

    End Function

End Class
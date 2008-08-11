﻿Imports System.Data.SqlClient

''' <summary>
''' RFQListByProductクラス
''' </summary>
''' <remarks>製品からRFQ一覧を表示します</remarks>
Partial Public Class RFQListByProduct
    Inherits CommonPage

    Public st_ProductID As String
    Public i_DataNum As Integer = 0 ' 0 の場合は Supplier Data が無いと判断し、 Data not found. を表示する。
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")

    ''' <summary>
    ''' 必須項目漏れのエラーメッセージ定数です。
    ''' </summary>
    ''' <remarks></remarks>
    Const MSG_REQUIED_PRODUCT_NUMBER = "Product Number が指定されていません。"

    ''' <summary>
    ''' このページのロードイベントです。
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Request.QueryString("ProductID") <> String.Empty Then

            st_ProductID = Request.QueryString("ProductID")

            ' パラメータ取得
            If Request.RequestType = "POST" Then
                st_ProductID = IIf(Request.Form("ProductID") = Nothing, "", Request.Form("ProductID"))
            ElseIf Request.RequestType = "GET" Then
                st_ProductID = IIf(Request.QueryString("ProductID") = Nothing, "", Request.QueryString("ProductID"))
            End If

            ' 空白除去
            st_ProductID = st_ProductID.Trim()

            If st_ProductID = "" Or IsNumeric(st_ProductID) = False Then
                Msg.Text = MSG_REQUIED_PRODUCT_NUMBER
                Exit Sub
            End If

            SearchProduct(st_ProductID)
            SearchRFQHeader(st_ProductID)
        End If

    End Sub

    ''' <summary>
    ''' 製品の検索を行います。
    ''' </summary>
    ''' <param name="st_ProductID">製品ID</param>
    ''' <remarks></remarks>
    Private Sub SearchProduct(ByVal st_ProductID)

        Using connection As New SqlClient.SqlConnection(DBConnectString.ConnectionString)

            Dim command As New SqlClient.SqlCommand(CreateProductHeaderSelectSQL(), connection)
            connection.Open()

            command.Parameters.AddWithValue("ProductID", st_ProductID)

            Dim reader As SqlClient.SqlDataReader = command.ExecuteReader()

            If reader.Read() Then
                i_DataNum = 1
                ProductNumber.Text = dbObjToStr(reader("ProductNumber"))
                If Not IsDBNull(reader("QuoName")) Then
                    QuoName.Text = dbObjToStr(reader("QuoName"))
                Else
                    QuoName.Text = dbObjToStr(reader("Name"))
                End If
                ProductName.Text = dbObjToStr(reader("Name"))
                CASNumber.Text = dbObjToStr(reader("CASNumber"))
                MolecularFormula.Text = dbObjToStr(reader("MolecularFormula"))
            End If
            reader.Close()
        End Using
    End Sub


    ''' <summary>
    ''' 見積依頼の検索を行います。
    ''' </summary>
    ''' <param name="st_ProductID">製品ID</param>
    ''' <remarks></remarks>
    Private Sub SearchRFQHeader(ByVal st_ProductID)

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

#Region "DB読み込み時変換関数"

    ''' <summary>
    ''' DBNullオブジェクトを空白文字列オブジェクトにします。
    ''' </summary>
    ''' <param name="obj">対象となるオブジェクト</param>
    ''' <returns>変換したオブジェクト</returns>
    ''' <remarks></remarks>
    Public Shared Function dbObjToObj(ByVal obj As Object) As Object
        Return dbObjToObj(obj, "")
    End Function

    ''' <summary>
    ''' DBNullオブジェクトを空白文字列オブジェクトにします。
    ''' </summary>
    ''' <param name="obj">対象となるオブジェクト</param>
    ''' <param name="retObj">DBNullの時に置き換えるオブジェクト</param>
    ''' <returns>変換したオブジェクト</returns>
    ''' <remarks></remarks>
    Public Shared Function dbObjToObj(ByVal obj As Object, ByVal retObj As Object) As Object
        If IsDBNull(obj) Then
            Return retObj
        End If

        If obj = Nothing Then
            Return retObj
        End If

        Return obj
    End Function

    ''' <summary>
    ''' DBNullオブジェクトをStringにします。
    ''' </summary>
    ''' <param name="obj">対象となるオブジェクト</param>
    ''' <returns>変換したString文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function dbObjToStr(ByVal obj As Object) As String
        Return CType(dbObjToObj(obj, ""), String)
    End Function

    ''' <summary>
    ''' DBNullオブジェクトをStringにします。
    ''' </summary>
    ''' <param name="obj">対象となるオブジェクト</param>
    ''' <param name="defaultStr">DBNullの時に置き換える文字列</param>
    ''' <returns>変換したString文字列</returns>
    ''' <remarks></remarks>
    Public Shared Function dbObjToStr(ByVal obj As Object, ByVal defaultStr As String) As String
        Return CType(dbObjToObj(obj, defaultStr), String)
    End Function

#End Region


End Class
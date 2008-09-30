Public Partial Class SupplierListByProduct
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader
    Public Url As String = ""
    Public AddUrl As String = ""
    Public ProductID As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        If IsPostBack = False Then
            If Request.QueryString("ProductID") = "" Then
                SrcSupplierProduct.SelectCommand = ""
                SupplierProductList.DataBind()
                Msg.Text = Common.ERR_INVALID_PARAMETER
                Exit Sub
            End If

            DBCommand.CommandText = "SELECT ProductNumber, Name, QuoName FROM dbo.Product WHERE ProductID = " + Request.QueryString("ProductID")
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                ProductNumber.Text = DBReader("ProductNumber")
                If Not TypeOf DBReader("Name") Is DBNull Then ProductName.Text = DBReader("Name")
                If Not TypeOf DBReader("QuoName") Is DBNull Then ProductName.Text = DBReader("QuoName")
            End If
            DBReader.Close()
            SrcSupplierProduct.SelectCommand = "SELECT dbo.Supplier_Product.SupplierCode, ISNULL(dbo.Supplier.Name3, '') + N' ' + ISNULL(dbo.Supplier.Name4, '') AS SupplierName, dbo.v_Country.CountryName, dbo.Supplier_Product.SupplierItemNumber, dbo.Supplier_Product.Note, dbo.Supplier_Product.UpdateDate, './SuppliersProductSetting.aspx?Action=Edit&Supplier=' + RTRIM(LTRIM(STR(dbo.Supplier_Product.SupplierCode))) + '&Product=" + Request.QueryString("ProductID") + "&Return=SP' AS Url " & _
                                               "FROM dbo.v_Country RIGHT OUTER JOIN dbo.Supplier ON dbo.v_Country.CountryCode = dbo.Supplier.CountryCode RIGHT OUTER JOIN dbo.Supplier_Product ON dbo.Supplier.SupplierCode = dbo.Supplier_Product.SupplierCode " & _
                                               "WHERE (dbo.Supplier_Product.ProductID = " + Request.QueryString("ProductID") + ")"
             SupplierProductList.DataBind()
        End If
    End Sub

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        If Request.Form("Action") = "Delete" Then
            '[指定レコード削除]-----------------------------------------------------------------
            DBCommand.CommandText = "DELETE Supplier_Product WHERE SupplierCode=" + Request.Form("SupplierCode") + " AND ProductID=" + Request.QueryString("ProductID")
            DBCommand.ExecuteNonQuery()
            Url = "./SupplierListByProduct.aspx?ProductID=" & Request.QueryString("ProductID")
            Response.Redirect(Url)
        End If

        '[New Suppliers ProductのURL設定]------------------------------------------------------------
        AddUrl = "./SuppliersProductSetting.aspx?Product=" + Request.QueryString("ProductID") + "&Return=SP"
        ProductID = Request.QueryString("ProductID")
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
    End Sub
End Class
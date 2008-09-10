Public Partial Class ProductListBySupplier
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader
    Public Url As String = ""
    Public AddUrl As String = ""
    Public ImpUrl As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '[DBの接続]-----------------------------------------------------------------------
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        If Not IsPostBack Then
            '[QueryString("Supplier")のチェック]----------------------------------------------
            If Request.QueryString("Supplier") = "" Then
                SrcSupplierProduct.SelectCommand = ""
                SupplierProductList.DataBind()
                Msg.Text = Common.ERR_INVALID_PARAMETER
                Exit Sub
            End If

            SupplierCode.Text = Request.QueryString("Supplier")
            DBCommand.CommandText = "SELECT Name3,Name4 FROM Supplier WHERE (SupplierCode = '" & SupplierCode.Text.ToString & "')"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                If Not TypeOf DBReader("Name3") Is DBNull Then SupplierName.Text = DBReader("Name3")
                If Not TypeOf DBReader("Name4") Is DBNull Then
                    If SupplierName.Text = String.Empty Then
                        SupplierName.Text = DBReader("Name4")
                    Else
                        SupplierName.Text = SupplierName.Text & " " & DBReader("Name4")
                    End If
                End If
            End If
            DBReader.Close()
            SrcSupplierProduct.SelectCommand = "SELECT Product.ProductID,Product.ProductNumber, CASE WHEN NOT Product.QuoName IS NULL THEN Product.QuoName ELSE Product.Name END AS ProductName, Supplier_Product.SupplierItemNumber, Supplier_Product.Note, REPLACE(CONVERT(char, Supplier_Product.UpdateDate, 111), '/', '-') AS UpdateDate, './SuppliersProductSetting.aspx?Action=Edit&Supplier=" + SupplierCode.Text.ToString + "&Product='+rtrim(ltrim(str(Product.ProductID))) AS Url " & _
                                               "FROM Supplier_Product LEFT OUTER JOIN Product ON Supplier_Product.ProductID = Product.ProductID " & _
                                               "WHERE (Supplier_Product.SupplierCode = '" & SupplierCode.Text.ToString & "')"
            SupplierProductList.DataBind()
        End If
    End Sub

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        If Request.Form("Action") = "Delete" Then
            '[指定レコード削除]-----------------------------------------------------------------
            DBCommand.CommandText = "DELETE Supplier_Product WHERE SupplierCode=" + Request.QueryString("Supplier") + " AND ProductID=" + Request.Form("ProductID")
            DBCommand.ExecuteNonQuery()
            Url = "./ProductListBySupplier.aspx?Supplier=" & SupplierCode.Text.ToString
            Response.Redirect(Url)
        End If

        '[New Suppliers ProductのURL設定]------------------------------------------------------------
        AddUrl = "./SuppliersProductSetting.aspx?Supplier=" & SupplierCode.Text.ToString

        '[Excel ImportのURL設定]---------------------------------------------------------------------
        ImpUrl = "./SuppliersProductImport.aspx?Supplier=" & SupplierCode.Text.ToString
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
    End Sub
End Class


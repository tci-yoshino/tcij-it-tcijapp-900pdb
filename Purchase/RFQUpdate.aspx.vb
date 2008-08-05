Public Partial Class RFQUpdate
    Inherits CommonPage
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    Public DBConn As New System.Data.SqlClient.SqlConnection
    Public DBCommand As System.Data.SqlClient.SqlCommand
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DBConn.ConnectionString = DBConnectString.ConnectionString
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        If IsPostBack = False Then
            Call FormDataSet()

        Else
            'ReadOnly項目の再設定
            R3SupplierCode.Text = Request.Form("R3SupplierCode").ToString
            SupplierName.Text = Request.Form("SupplierName").ToString
            SupplierCountry.Text = Request.Form("SupplierCountry").ToString
            MakerName.Text = Request.Form("MakerName").ToString
            MakerCountry.Text = Request.Form("MakerCountry").ToString
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack = False Then
            Update.PostBackUrl = "~/RFQUpdate.aspx?Action=Update"
            Close.PostBackUrl = "~/RFQUpdate.aspx?Action=Close"
        End If
    End Sub
    Private Sub FormDataSet()
        'Dim DBDataset As DataSet
        Dim st_RFQNumber As String
        Dim DBReader As System.Data.SqlClient.SqlDataReader
        If Request.QueryString("RFQNumber") <> "" Or Request.Form("RFQNumber") <> "" Then
            st_RFQNumber = IIf(Request.QueryString("RFQNumber") <> "", Request.QueryString("RFQNumber"), Request.Form("RFQNumber"))
            If IsNumeric(st_RFQNumber) Then
                DBCommand.CommandText = "Select * From v_RFQHeader Where RFQNumber = @i_RFQNumber"
                DBCommand.Parameters.Add("i_RFQNumber", SqlDbType.Int).Value = CInt(st_RFQNumber)
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.HasRows = True Then
                    While DBReader.Read
                        RFQNumber.Text = st_RFQNumber
                        CurrentRFQStatus.Text = DBReader("Status").ToString
                        ProductNumber.Text = DBReader("ProductNumber").ToString
                        ProductName.Text = DBReader("ProductName").ToString
                        SupplierCode.Text = DBReader("SupplierCode").ToString
                        R3SupplierCode.Text = DBReader("R3SupplierCode").ToString
                        SupplierName.Text = DBReader("SupplierName").ToString
                        SupplierCountry.Text = DBReader("SupplierCountryCode").ToString
                        SupplierContactPerson.Text = DBReader("SupplierContactPerson").ToString
                        MakerCode.Text = DBReader("MakerCode").ToString
                        MakerName.Text = DBReader("MakerName").ToString
                        MakerCountry.Text = DBReader("MakerCountryCode").ToString
                        SupplierItemName.Text = DBReader("SupplierItemName").ToString
                        PaymentTerm.SelectedValue = DBReader("PaymentTermCode").ToString
                        ShippingHandlingCurrency.SelectedValue = DBReader("ShippingHandlingCurrencyCode").ToString
                        ShippingHandlingFee.Text = DBReader("ShippingHandlingFee").ToString


                    End While
                End If
                DBReader.Close()
            End If
        End If
    End Sub

End Class
Public Partial Class SupplierListByProduct
    Inherits CommonPage

#Region " Region "
    '*****（Region内は変更しないこと）*****
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection
        Me.SqlConnection1.FireInfoMessageEventOnUserErrors = False
    End Sub

    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub
#End Region

    '*****（DB接続用変数定義）*****
    Dim DBConnString As String                              '接続文字列	
    Dim DBConn As New System.Data.SqlClient.SqlConnection   'データベースコネクション	
    Dim DBCommand As System.Data.SqlClient.SqlCommand       'データベースコマンド	
    Dim DBReader As System.Data.SqlClient.SqlDataReader     'データリーダー	
    Public Url As String = ""
    Public AddUrl As String = ""
    Public ProductID As String = ""

    Sub Set_DBConnectingString()
        Dim settings As ConnectionStringSettings
        '[接続文字列を設定ファイル(Web.config)から取得]---------------------------------------------
        settings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
        If Not settings Is Nothing Then
            '[接続文字列をイミディエイトに出力]-----------------------------------------------------
            Debug.Print(settings.ConnectionString)
        End If
        '[sqlConnectionに接続文字列を設定]----------------------------------------------------------
        Me.SqlConnection1.ConnectionString = settings.ConnectionString
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Set_DBConnectingString()
        DBConn = Me.SqlConnection1
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        If Request.QueryString("ProductID") <> "" Then
            If IsPostBack = False Then
                DBCommand.CommandText = "SELECT ProductNumber, Name, QuoName FROM dbo.Product WHERE ProductID = " + Request.QueryString("ProductID")
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    ProductNumber.Text = DBReader("ProductNumber")
                    If Not TypeOf DBReader("Name") Is DBNull Then ProductName.Text = DBReader("Name")
                    If Not TypeOf DBReader("QuoName") Is DBNull Then ProductName.Text = DBReader("QuoName")
                End If
                DBReader.Close()
                SrcSupplierProduct.SelectCommand = "SELECT dbo.Supplier_Product.SupplierCode, ISNULL(dbo.Supplier.Name3, '') + N' ' + ISNULL(dbo.Supplier.Name4, '') AS [SupplierName], dbo.Supplier_Product.SupplierItemNumber, dbo.Supplier_Product.Note, REPLACE(CONVERT(char, Supplier_Product.UpdateDate, 111), '/', '-') AS UpdateDate, './SuppliersProductSetting.aspx?Action=Edit&Supplier='+rtrim(ltrim(str(Supplier_Product.SupplierCode)))+'&Product=" + Request.QueryString("ProductID") + "&Return=SP' AS Url " & _
                                                   "FROM dbo.Supplier_Product LEFT OUTER JOIN dbo.Supplier ON dbo.Supplier_Product.SupplierCode = dbo.Supplier.SupplierCode " & _
                                                   "WHERE (dbo.Supplier_Product.ProductID = " + Request.QueryString("ProductID") + ")"
                SupplierProductList.DataBind()
            End If
        Else
            SrcSupplierProduct.SelectCommand = ""
            SupplierProductList.DataBind()
            Msg.Text = "ProductIDが設定されていません"
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
End Class
Public Partial Class ProductListBySupplier
    Inherits CommonPage

#Region " Web フォーム デザイナで生成されたコード "
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
        '[DBの接続]-----------------------------------------------------------------------
        Set_DBConnectingString()
        DBConn = Me.SqlConnection1
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        SupplierCode.Text = Request.QueryString("Supplier")
        DBCommand.CommandText = "SELECT Name3 FROM Supplier WHERE (SupplierCode = '" & SupplierCode.Text.ToString & "')"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.Read = True Then
            SupplierName.Text = DBReader("Name3")
        End If
        SrcSupplierProduct.SelectCommand = "SELECT Product.ProductID, Product.ProductNumber, CASE WHEN NOT Product.QuoName IS NULL THEN Product.QuoName ELSE Product.Name END AS ProductName, Supplier_Product.SupplierItemNumber, Supplier_Product.Note, REPLACE(CONVERT(char, Supplier_Product.UpdateDate, 111), '/', '-') AS UpdateDate, './SuppliersProductSetting.aspx?Action=Edit&Supplier=" + SupplierCode.Text.ToString + "&Product=' AS Url " & _
                                           "FROM Supplier_Product LEFT OUTER JOIN Product ON Supplier_Product.ProductID = Product.ProductID " & _
                                           "WHERE (Supplier_Product.SupplierCode = " & SupplierCode.Text.ToString & ")"
        SupplierProductList.DataBind()
    End Sub

End Class
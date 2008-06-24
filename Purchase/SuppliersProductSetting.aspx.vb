Public Partial Class SuppliersProductSetting
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
    Dim st_Action As String
    Dim st_Supplier As String

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
        '[DBの接続]---------------------------------------------------------------------------------
        Set_DBConnectingString()
        DBConn = Me.SqlConnection1
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        If IsPostBack = False Then
            st_Action = Request.QueryString("Action")
            st_Supplier = Request.QueryString("Supplier")
            Product.Value = Request.QueryString("Product ")




            '**********************************************************************************************************************
            st_Action = "Edit"
            st_Supplier = ""
            Product.Value = "5"
            '**********************************************************************************************************************




            If st_Action <> "Edit" Then SupplierSelect.Visible = False

            If st_Supplier <> "" Then
                '[SupplierNameの表示]---------------------------------------------------------------
                Supplier.Text = st_Supplier
                DBCommand.CommandText = "SELECT SupplierCode,Name3 FROM Supplier WHERE SupplierCode='" & st_Supplier & "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    Supplier.Text = DBReader("SupplierCode")
                    SupplierName.Text = DBReader("Name3")
                    Supplier.ReadOnly = True
                    Supplier.CssClass = "readonly"
                End If
                DBReader.Close()
            End If
            If Product.Value <> "" Then
                '[ProductNameの表示]----------------------------------------------------------------
                DBCommand.CommandText = "SELECT ProductNumber,Name FROM Product WHERE ProDuctID='" & Product.Value & "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    ProductNumber.Text = DBReader("ProductNumber")
                    ProductName.Text = DBReader("Name")
                    ProductNumber.ReadOnly = True
                    ProductNumber.CssClass = "readonly"
                End If
                DBReader.Close()
            End If
        End If
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        If Request.Form("Action") = "Save" Then
            Dim st_SQLSTR As String = ""
            Msg.Text = ""
            If Supplier.Text.ToString <> "" And ProductNumber.Text.ToString <> "" Then
                '[Supplier存在チェック]-------------------------------------------------------------
                DBCommand.CommandText = "SELECT SupplierCode,Name3 FROM Supplier WHERE SupplierCode='" & Supplier.Text.ToString & "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    Supplier.Text = DBReader("SupplierCode")
                    SupplierName.Text = DBReader("Name3")
                Else
                    Msg.Text = "SupplierCodeが見つかりません"
                End If
                DBReader.Close()

                '[Product存在チェック]--------------------------------------------------------------
                DBCommand.CommandText = "SELECT ProductNumber,Name FROM Product WHERE ProductNumber='" & ProductNumber.Text.ToString & "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    ProductNumber.Text = DBReader("ProductNumber")
                    ProductName.Text = DBReader("Name")
                Else
                    Msg.Text = "ProductNumberが見つかりません"
                End If
                DBReader.Close()

                '[Supplier_Product登録、更新]-------------------------------------------------------
                If Msg.Text = "" Then
                    If st_Action = "" Then
                        '[Supplier_Product登録]---------------------------------------------------------
                        st_SQLSTR = "INSERT INTO Supplier_Product (SupplierCode,ProductID,SupplierItemNumber,Note,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ("
                        st_SQLSTR = st_SQLSTR & Supplier.Text.ToString & "," & Request.Form("Product") & ","
                        If SupplierItemNumber.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SupplierItemNumber.Text.ToString & "',"
                        If Note.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Note.Text.ToString & "',"
                        st_SQLSTR = st_SQLSTR & "'" & Session("UserID") & "','" & Now() & "','" & Session("UserID") & "','" & Now() & "')"
                        DBCommand.CommandText = st_SQLSTR
                        DBCommand.ExecuteNonQuery()
                    Else
                        '[Supplier_Product更新]---------------------------------------------------------
                        DBCommand.CommandText = "SELECT SupplierCode,ProductID FROM Supplier_Product WHERE (SupplierCode = '" & Supplier.Text.ToString & "' AND ProductID='" & ProductNumber.Text.ToString & "')"
                        DBReader = DBCommand.ExecuteReader()
                        DBCommand.Dispose()
                        If DBReader.Read = True Then
                            DBReader.Close()
                            st_SQLSTR = "UPDATE Supplier_Product SET SupplierItemNumber="
                            If SupplierItemNumber.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SupplierItemNumber.Text.ToString & "',"
                            st_SQLSTR = st_SQLSTR & "Note="
                            If Note.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Note.Text.ToString & "',"
                            st_SQLSTR = st_SQLSTR & "UpdatedBy=" & Session("UserID") & ", UpdateDate='" & Now() & "' "
                            DBCommand.CommandText = st_SQLSTR
                            DBCommand.ExecuteNonQuery()
                        Else
                            DBReader.Close()
                        End If
                    End If
                End If
            Else
                Msg.Text = "必須項目を入力して下さい"
            End If
        End If
    End Sub
End Class
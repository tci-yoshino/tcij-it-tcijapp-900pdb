﻿Public Partial Class SuppliersProductSetting
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
    Public Url As String
    Public st_ProductID As String

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

        st_ProductID = Request.QueryString("Product")
        If IsPostBack = False Then
            If Request.QueryString("Action") = "Edit" Then
                SupplierSelect.Visible = False
            Else
                If Request.QueryString("Supplier") <> "" Then
                    SupplierSelect.Visible = False
                End If
            End If

            If Request.QueryString("Supplier") <> "" Then
                '[SupplierNameの表示]---------------------------------------------------------------
                Supplier.Text = Request.QueryString("Supplier")
                DBCommand.CommandText = "SELECT SupplierCode,Name3 FROM Supplier WHERE SupplierCode='" & Request.QueryString("Supplier") & "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    If Not TypeOf DBReader("SupplierCode") Is DBNull Then Supplier.Text = DBReader("SupplierCode")
                    If Not TypeOf DBReader("Name3") Is DBNull Then SupplierName.Text = DBReader("Name3")
                    Supplier.ReadOnly = True
                    Supplier.CssClass = "readonly"
                End If
                DBReader.Close()
            End If
            If Request.QueryString("Product") <> "" Then
                '[ProductNameの表示]----------------------------------------------------------------
                DBCommand.CommandText = "SELECT ProductNumber,Name,QuoName FROM Product WHERE ProDuctID='" & Request.QueryString("Product") & "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    If Not TypeOf DBReader("ProductNumber") Is DBNull Then ProductNumber.Text = DBReader("ProductNumber")
                    If Not TypeOf DBReader("Name") Is DBNull Then ProductName.Text = DBReader("Name")
                    If Not TypeOf DBReader("QuoName") Is DBNull Then ProductName.Text = DBReader("QuoName")
                    ProductNumber.ReadOnly = True
                    ProductNumber.CssClass = "readonly"
                End If
                DBReader.Close()
            End If
            If Supplier.Text.ToString <> "" And ProductNumber.Text.ToString <> "" Then
                DBCommand.CommandText = "SELECT SupplierItemNumber,Note FROM Supplier_Product WHERE (SupplierCode = '" & Request.QueryString("Supplier") & "' AND ProductID='" & Request.QueryString("Product") & "')"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    If Not TypeOf DBReader("SupplierItemNumber") Is DBNull Then SupplierItemNumber.Text = DBReader("SupplierItemNumber")
                    If Not TypeOf DBReader("Note") Is DBNull Then Note.Text = DBReader("Note")
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
                DBCommand.CommandText = "SELECT ProductID,ProductNumber,Name,QuoName FROM Product WHERE ProductNumber='" & ProductNumber.Text.ToString & "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    ProductNumber.Text = DBReader("ProductNumber")
                    If Not TypeOf DBReader("Name") Is DBNull Then ProductName.Text = DBReader("Name")
                    If Not TypeOf DBReader("QuoName") Is DBNull Then ProductName.Text = DBReader("QuoName")
                    '[ProductID取得]----------------------------------------------------------------
                    st_ProductID = DBReader("ProductID")
                Else
                    Msg.Text = "ProductNumberが見つかりません"
                End If
                DBReader.Close()

                If Msg.Text.ToString = "" Then
                    '[Supplier_Product登録、更新]-------------------------------------------------------
                    DBCommand.CommandText = "SELECT SupplierCode,ProductID FROM Supplier_Product WHERE (SupplierCode = '" & Supplier.Text.ToString & "' AND ProductID='" & st_ProductID & "')"
                    DBReader = DBCommand.ExecuteReader()
                    DBCommand.Dispose()
                    If DBReader.Read = True Then
                        '[Supplier_Product更新]---------------------------------------------------------
                        DBReader.Close()
                        st_SQLSTR = "UPDATE Supplier_Product SET SupplierItemNumber="
                        If SupplierItemNumber.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SupplierItemNumber.Text.ToString & "',"
                        st_SQLSTR = st_SQLSTR & "Note="
                        If Note.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Note.Text.ToString & "',"
                        st_SQLSTR = st_SQLSTR & "UpdatedBy=" & Session("UserID") & ", UpdateDate='" & Now() & "' "
                        st_SQLSTR = st_SQLSTR & "WHERE (SupplierCode = '" & Supplier.Text.ToString & "' AND ProductID='" & st_ProductID & "')"
                        DBCommand.CommandText = st_SQLSTR
                        DBCommand.ExecuteNonQuery()
                    Else
                        '[Supplier_Product登録]---------------------------------------------------------
                        DBReader.Close()
                        st_SQLSTR = "INSERT INTO Supplier_Product (SupplierCode,ProductID,SupplierItemNumber,Note,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ("
                        st_SQLSTR = st_SQLSTR & Supplier.Text.ToString & "," & st_ProductID & ","
                        If SupplierItemNumber.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SupplierItemNumber.Text.ToString & "',"
                        If Note.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Note.Text.ToString & "',"
                        st_SQLSTR = st_SQLSTR & "'" & Session("UserID") & "','" & Now() & "','" & Session("UserID") & "','" & Now() & "')"
                        DBCommand.CommandText = st_SQLSTR
                        DBCommand.ExecuteNonQuery()
                    End If
                    Url = "./ProductListBySupplier.aspx?Supplier=" & Supplier.Text.ToString
                    Response.Redirect(Url)
                End If
            Else
                Msg.Text = "必須項目を入力して下さい"
            End If
        Else
            Msg.Text = "Saveは拒否されました"
        End If
    End Sub

End Class
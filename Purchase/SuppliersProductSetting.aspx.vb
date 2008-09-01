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
    Dim ActNai As String                                    '処理判断内容
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
                DBCommand.CommandText = "SELECT SupplierCode,Name3,Name4 FROM Supplier WHERE SupplierCode='" & Request.QueryString("Supplier") & "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                Supplier.Text = String.Empty
                SupplierName.Text = String.Empty
                If DBReader.Read = True Then
                    If Not TypeOf DBReader("SupplierCode") Is DBNull Then Supplier.Text = DBReader("SupplierCode")
                    If Not TypeOf DBReader("Name3") Is DBNull Then SupplierName.Text = DBReader("Name3")
                    If Not TypeOf DBReader("Name4") Is DBNull Then
                        If SupplierName.Text = String.Empty Then
                            SupplierName.Text = DBReader("Name4")
                        Else
                            SupplierName.Text = SupplierName.Text & " " & DBReader("Name4")
                        End If
                    End If
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
                DBCommand.CommandText = "SELECT SupplierItemNumber,Note,UpdateDate FROM Supplier_Product WHERE (SupplierCode = '" & Request.QueryString("Supplier") & "' AND ProductID='" & Request.QueryString("Product") & "')"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    If Not TypeOf DBReader("SupplierItemNumber") Is DBNull Then SupplierItemNumber.Text = DBReader("SupplierItemNumber")
                    If Not TypeOf DBReader("Note") Is DBNull Then Note.Text = DBReader("Note")
                    UpdateDate.Value = DBReader("UpdateDate").ToString()
                Else
                    UpdateDate.Value = ""
                End If
                DBReader.Close()
            End If
        End If
    End Sub

    Private Sub SupplierProductSetting_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim wClient As String       'クライアントサイドの処理を格納する
        Dim Type2 As Type = Me.GetType
        wClient = Clientside()
        If wClient <> "" Then
            ClientScript.RegisterStartupScript(Type2, "startup", Chr(13) & Chr(10) & "<script language='JavaScript' type=text/javascript> " & wClient & " </script>")
        End If
    End Sub

    Private Function Clientside()
        Clientside = ""
        If ActNai = "SupplierSelect.aspx_Open" Then
            Clientside = "popup('SupplierSelect.aspx?code=" & Common.SafeSqlLiteral(Supplier.Text) & "')"
        End If
    End Function

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        Dim st_SQLSTR As String = String.Empty
        Msg.Text = String.Empty

        '[Acionチェック]---------------------------------------------------------------
        If Request.Form("Action") <> "Save" Then
            Msg.Text = "Saveは拒否されました"
            Exit Sub
        End If

        '[Supplier.Textの数字チェック]-------------------------------------------------
        If Not IsNumeric(Supplier.Text.ToString) Then
            Msg.Text = "SupplierCodeが数字でない"
            Exit Sub
        End If

        '[Noteの文字数Check]-----------------------------------------------------------
        If Note.Text.Length > 3000 Then
            Msg.Text = "Noteの文字数が3000を超えています。"
            Exit Sub
        End If

        '[必須項目チェック]------------------------------------------------------------
        If Supplier.Text.ToString = String.Empty Or ProductNumber.Text.ToString = String.Empty Then
            Msg.Text = "必須項目を入力して下さい"
            Exit Sub
        End If

        '[Supplier存在チェック]-------------------------------------------------------------
        DBCommand.CommandText = "SELECT SupplierCode,Name3,Name4 FROM Supplier WHERE SupplierCode='" & Common.SafeSqlLiteral(Supplier.Text) & "'"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        Supplier.Text = String.Empty
        SupplierName.Text = String.Empty
        If DBReader.Read = True Then
            If Not TypeOf DBReader("SupplierCode") Is DBNull Then Supplier.Text = DBReader("SupplierCode")
            If Not TypeOf DBReader("Name3") Is DBNull Then SupplierName.Text = DBReader("Name3")
            If Not TypeOf DBReader("Name4") Is DBNull Then
                If SupplierName.Text = String.Empty Then
                    SupplierName.Text = DBReader("Name4")
                Else
                    SupplierName.Text = SupplierName.Text & " " & DBReader("Name4")
                End If
            End If
        Else
            Msg.Text = "SupplierCodeが見つかりません"
        End If
        DBReader.Close()

        '[Product存在チェック]--------------------------------------------------------------
        DBCommand.CommandText = "SELECT ProductID,ProductNumber,Name,QuoName FROM Product WHERE ProductNumber='" & Common.SafeSqlLiteral(ProductNumber.Text) & "'"
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
            DBCommand.CommandText = "SELECT SupplierCode,ProductID,UpdateDate FROM Supplier_Product WHERE (SupplierCode = '" & Common.SafeSqlLiteral(Supplier.Text) & "' AND ProductID='" & st_ProductID & "')"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                If Request.QueryString("Action") = "Edit" Then
                    If DBReader("UpdateDate").ToString() = UpdateDate.Value Then

                        '[Supplier_Product更新]---------------------------------------------------------
                        DBReader.Close()
                        st_SQLSTR = "UPDATE Supplier_Product SET SupplierItemNumber="
                        If SupplierItemNumber.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(SupplierItemNumber.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "Note="
                        If Note.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Note.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "UpdatedBy=" & Session("UserID") & ", UpdateDate='" & Now() & "' "
                        st_SQLSTR = st_SQLSTR & "WHERE (SupplierCode = '" & Common.SafeSqlLiteral(Supplier.Text) & "' AND ProductID='" & st_ProductID & "')"
                        DBCommand.CommandText = st_SQLSTR
                        DBCommand.ExecuteNonQuery()
                    Else
                        DBReader.Close()
                        Msg.Text = "このデータは他のユーザーによって編集されました。その内容を確認し再度編集をお願いします"
                    End If
                Else
                    DBReader.Close()
                    Msg.Text = "このデータはすでに登録済です。その内容を確認し再度処理をお願いします"
                End If
            Else
                '[Supplier_Product登録]---------------------------------------------------------
                DBReader.Close()
                st_SQLSTR = "INSERT INTO Supplier_Product (SupplierCode,ProductID,SupplierItemNumber,Note,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ("
                st_SQLSTR = st_SQLSTR & Supplier.Text.ToString & "," & st_ProductID & ","
                If SupplierItemNumber.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(SupplierItemNumber.Text) & "',"
                If Note.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Note.Text) & "',"
                st_SQLSTR = st_SQLSTR & "'" & Session("UserID") & "','" & Now() & "','" & Session("UserID") & "','" & Now() & "')"
                DBCommand.CommandText = st_SQLSTR
                DBCommand.ExecuteNonQuery()
            End If
            If Msg.Text.ToString = "" Then
                If Request.QueryString("Return") = "SP" Then
                    Url = "./SupplierListByProduct.aspx?ProductID=" & st_ProductID
                Else
                    Url = "./ProductListBySupplier.aspx?Supplier=" & Common.SafeSqlLiteral(Supplier.Text)
                End If
                Response.Redirect(Url)
            End If
        End If
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
    End Sub

    Protected Sub SupplierSelect_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles SupplierSelect.Click
        ActNai = "SupplierSelect.aspx_Open"
    End Sub
End Class
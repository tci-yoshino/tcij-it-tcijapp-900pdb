Public Partial Class SuppliersProductSetting
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader
    Dim ActNai As String                                    '処理判断内容
    Public Url As String
    Public st_ProductID As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '[DBの接続]---------------------------------------------------------------------------------
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
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        '[必須項目チェック]------------------------------------------------------------
        If Supplier.Text = String.Empty Then
            Msg.Text = "Supplier Code" + Common.ERR_REQUIRED_FIELD
            Exit Sub
        End If
        If ProductNumber.Text = String.Empty Then
            Msg.Text = "Product Number" + Common.ERR_REQUIRED_FIELD
            Exit Sub
        End If

        '[Supplier.Textの数字チェック]-------------------------------------------------
        If Not IsNumeric(Supplier.Text.ToString) Then
            Msg.Text = "Supplier Code" + Common.ERR_INVALID_NUMBER
            Exit Sub
        End If

        '[Noteの文字数Check]-----------------------------------------------------------
        If Note.Text.Length > 3000 Then
            Msg.Text = "Noteの文字数が3000を超えています。"
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
            DBReader.Close()
            Msg.Text = "Supplier Code はマスタに存在しません。"
            Exit Sub
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
            DBReader.Close()
            Msg.Text = "Product Number はマスタに存在しません。"
            Exit Sub
        End If
        DBReader.Close()

        '[Supplier_Product登録、更新]-------------------------------------------------------
        DBCommand.CommandText = "SELECT SupplierCode,ProductID,UpdateDate FROM Supplier_Product WHERE (SupplierCode = '" & Common.SafeSqlLiteral(Supplier.Text) & "' AND ProductID='" & st_ProductID & "')"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.Read = True Then
            If Request.QueryString("Action") <> "Edit" Then
                DBReader.Close()
                Msg.Text = "同じ Supplier Code と Product Number を持つデータが既に登録されています。ご確認の上、再編集してください。"
                Exit Sub
            End If

            If DBReader("UpdateDate").ToString() <> UpdateDate.Value Then
                DBReader.Close()
                Msg.Text = "データは他のユーザによって既に更新されています。ご確認ください。"
                Exit Sub
            End If

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
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
    End Sub

    Protected Sub SupplierSelect_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles SupplierSelect.Click
        ActNai = "SupplierSelect.aspx_Open"
    End Sub
End Class
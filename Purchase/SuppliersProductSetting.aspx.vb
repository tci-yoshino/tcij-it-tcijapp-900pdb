﻿Imports Purchase.Common

Partial Public Class SuppliersProductSetting
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader
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

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        Dim st_SQLSTR As String = String.Empty
        Msg.Text = String.Empty

        '[Acionチェック]---------------------------------------------------------------
        If Request.Form("Action") <> "Save" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        '[必須項目チェック]------------------------------------------------------------
        If Supplier.Text = String.Empty Then
            Msg.Text = "Supplier Code" + ERR_REQUIRED_FIELD
            Exit Sub
        End If
        If ProductNumber.Text = String.Empty Then
            Msg.Text = "Product Number" + ERR_REQUIRED_FIELD
            Exit Sub
        End If

        '[Supplier.Textの数字チェック]-------------------------------------------------
        If Not IsInteger(Supplier.Text) Then
            Msg.Text = "Supplier Code" + ERR_INVALID_NUMBER
            Exit Sub
        Else
            If SafeSqlLiteral(Supplier.Text) Like "*+*" Then
                Msg.Text = "Supplier Code" + ERR_INVALID_NUMBER
                Exit Sub
            End If
        End If

        '[Noteの文字数Check]-----------------------------------------------------------
        Note.Text = Note.Text.Trim         '入力データ前後の改行コード、タブコードを除去
        If Note.Text.Length > INT_3000 Then
            Msg.Text = "Note" + ERR_OVER_3000
            Exit Sub
        End If

        '[Supplier存在チェック]-------------------------------------------------------------
        DBCommand.CommandText = "SELECT SupplierCode,Name3,Name4 FROM Supplier WHERE SupplierCode='" & SafeSqlLiteral(Supplier.Text) & "'"
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
            Msg.Text = "Supplier Code" + ERR_DOES_NOT_EXIST   '"Supplier Code はマスタに存在しません。"
            Exit Sub
        End If
        DBReader.Close()

        '[Product存在チェック]--------------------------------------------------------------
        DBCommand.CommandText = "SELECT ProductID,ProductNumber,Name,QuoName FROM Product WHERE ProductNumber='" & SafeSqlLiteral(ProductNumber.Text) & "'"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.Read = True Then
            '[ProductID取得]----------------------------------------------------------------
            st_ProductID = DBReader("ProductID")

            '権限ロールに従い極秘品はエラーとする
            If Session(SESSION_ROLE_CODE).ToString = ROLE_WRITE_P OrElse Session(SESSION_ROLE_CODE).ToString = ROLE_READ_P Then
                If IsConfidentialItem(st_ProductID) Then
                    Msg.Text = ERR_CONFIDENTIAL_PRODUCT
                    Exit Sub
                End If
            End If

            ProductNumber.Text = DBReader("ProductNumber")
            If Not TypeOf DBReader("Name") Is DBNull Then ProductName.Text = DBReader("Name")
            If Not TypeOf DBReader("QuoName") Is DBNull Then ProductName.Text = DBReader("QuoName")

        Else
            DBReader.Close()
            Msg.Text = "Product Number" + ERR_DOES_NOT_EXIST   '"Product Number はマスタに存在しません。"
            Exit Sub
        End If
        DBReader.Close()

        '[Supplier_Product登録、更新]-------------------------------------------------------
        DBCommand.CommandText = "SELECT SupplierCode,ProductID,UpdateDate FROM Supplier_Product WHERE (SupplierCode = '" & SafeSqlLiteral(Supplier.Text) & "' AND ProductID='" & st_ProductID & "')"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.Read = True Then
            If Request.QueryString("Action") <> "Edit" Then
                DBReader.Close()
                Msg.Text = "The same set of 'Supplier code' and 'Product Number' already exist.<br />(Please check again to avoid duplication.)"   '"同じ Supplier Code と Product Number を持つデータが既に登録されています。ご確認の上、再編集してください。"
                Exit Sub
            End If

            If DBReader("UpdateDate").ToString() <> UpdateDate.Value Then
                DBReader.Close()
                Msg.Text = ERR_UPDATED_BY_ANOTHER_USER   '"データは他のユーザによって既に更新されています。ご確認ください。"
                Exit Sub
            End If

            '[Supplier_Product更新]---------------------------------------------------------
            DBReader.Close()
            st_SQLSTR = "UPDATE Supplier_Product SET SupplierItemNumber="
            If SupplierItemNumber.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(SupplierItemNumber.Text) & "',"
            st_SQLSTR = st_SQLSTR & "Note="
            If Note.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Note.Text) & "',"
            st_SQLSTR = st_SQLSTR & "UpdatedBy=" & Session("UserID") & ", UpdateDate='" & Now() & "' "
            st_SQLSTR = st_SQLSTR & "WHERE (SupplierCode = '" & SafeSqlLiteral(Supplier.Text) & "' AND ProductID='" & st_ProductID & "')"
            DBCommand.CommandText = st_SQLSTR
            DBCommand.ExecuteNonQuery()
        Else
            '[Supplier_Product登録]---------------------------------------------------------
            DBReader.Close()
            st_SQLSTR = "INSERT INTO Supplier_Product (SupplierCode,ProductID,SupplierItemNumber,Note,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ("
            st_SQLSTR = st_SQLSTR & Supplier.Text.ToString & "," & st_ProductID & ","
            If SupplierItemNumber.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(SupplierItemNumber.Text) & "',"
            If Note.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Note.Text) & "',"
            st_SQLSTR = st_SQLSTR & "'" & Session("UserID") & "','" & Now() & "','" & Session("UserID") & "','" & Now() & "')"
            DBCommand.CommandText = st_SQLSTR
            DBCommand.ExecuteNonQuery()
        End If
        If Msg.Text.ToString = "" Then
            If Request.QueryString("Return") = "SP" Then
                Url = "./SupplierListByProduct.aspx?ProductID=" & st_ProductID
            Else
                Url = "./ProductListBySupplier.aspx?Supplier=" & SafeSqlLiteral(Supplier.Text)
            End If
            Response.Redirect(Url)
        End If
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
    End Sub

End Class
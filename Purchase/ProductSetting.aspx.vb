Public Partial Class ProductSetting
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader
    Public url As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        If IsPostBack = False Then
            '[ProductIDのセット]------------------------------------------------------------------------
            Mode.Value = Request.QueryString("Action")
            ProductID.Value = Request.QueryString("ProductID")

            If Mode.Value = "Edit" Then
                DBCommand.CommandText = "SELECT Product.ProductNumber, Product.Name, Product.QuoName, Product.CASNumber, Product.MolecularFormula, Product.Reference, Product.Comment, Product.UpdateDate, s_EhsPhrase.ENai AS Status, s_EhsPhrase_1.ENai AS ProposalDept, s_EhsPhrase_2.ENai AS ProcumentDept, s_EhsPhrase_3.ENai AS PD " & _
                                        "FROM Product LEFT OUTER JOIN " & _
                                        "s_EhsPhrase AS s_EhsPhrase_3 ON Product.PD = s_EhsPhrase_3.PhID LEFT OUTER JOIN " & _
                                        "s_EhsPhrase AS s_EhsPhrase_2 ON Product.ProcumentDept = s_EhsPhrase_2.PhID LEFT OUTER JOIN " & _
                                        "s_EhsPhrase AS s_EhsPhrase_1 ON Product.ProposalDept = s_EhsPhrase_1.PhID LEFT OUTER JOIN " & _
                                        "s_EhsPhrase ON Product.Status = s_EhsPhrase.PhID " & _
                                        "WHERE(dbo.Product.ProductID = " + ProductID.Value + ")"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    If Not TypeOf DBReader("ProductNumber") Is DBNull Then ProductNumber.Text = DBReader("ProductNumber")
                    If Not TypeOf DBReader("Name") Is DBNull Then ProductName.Text = DBReader("Name")
                    If Not TypeOf DBReader("QuoName") Is DBNull Then QuoName.Text = DBReader("QuoName")
                    If Not TypeOf DBReader("CASNumber") Is DBNull Then CASNumber.Text = DBReader("CASNumber")
                    If Not TypeOf DBReader("MolecularFormula") Is DBNull Then MolecularFormula.Text = DBReader("MolecularFormula")
                    If Not TypeOf DBReader("Reference") Is DBNull Then Reference.Text = DBReader("Reference")
                    If Not TypeOf DBReader("Comment") Is DBNull Then Comment.Text = DBReader("Comment")
                    If Not TypeOf DBReader("Status") Is DBNull Then Status.Text = DBReader("Status")
                    If Not TypeOf DBReader("ProposalDept") Is DBNull Then ProposalDept.Text = DBReader("ProposalDept")
                    If Not TypeOf DBReader("ProcumentDept") Is DBNull Then ProcumentDept.Text = DBReader("ProcumentDept")
                    If Not TypeOf DBReader("PD") Is DBNull Then PD.Text = DBReader("PD")
                    DBReader.Close()
                    UpdateDate.Value = Common.GetUpdateDate("Product", "ProductID", ProductID.Value) '[同時更新チェック用]
                Else
                    UpdateDate.Value = ""
                End If
                DBReader.Close()
            Else
                SupplierList.Visible = False
            End If
        End If

        If ProductID.Value <> "" Then
            SupplierList.NavigateUrl = "./SupplierListByProduct.aspx?ProductID=" & ProductID.Value
        Else
            SupplierList.NavigateUrl = "./SupplierListByProduct.aspx"
        End If
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        Dim NumberType As String = ""
        Dim st_SqlStr As String = ""
        Msg.Text = ""

        '[入力ProductNumberの正規化]---------------------------------------------------
        ProductNumber.Text = StrConv(ProductNumber.Text, VbStrConv.Narrow)
        ProductNumber.Text = UCase(ProductNumber.Text)

        '[Actionチェック]--------------------------------------------------------------
        If Action.Value <> "Save" Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        '[必須項目入力チェック]--------------------------------------------------------
        If ProductNumber.Text = "" Then
            Msg.Text = "Product Number" + Common.ERR_REQUIRED_FIELD
            Exit Sub
        End If
        If ProductName.Text = "" Then
            Msg.Text = "Product Name" + Common.ERR_REQUIRED_FIELD
            Exit Sub
        End If

        '[ProductNumber重複チェック]---------------------------------------------------
        If Mode.Value = "Edit" Then
            DBCommand.CommandText = "SELECT ProductNumber FROM Product WHERE ProductNumber = '" & Common.SafeSqlLiteral(ProductNumber.Text) & "' AND ProductID<>" & ProductID.Value
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                Msg.Text = "同じ Product Number のデータが既に登録されています。ご確認ください。"
                Exit Sub
            End If
            DBReader.Close()
        End If

        '[入力項目のLengthCheck]-------------------------------------------------------
        If Reference.Text.Length > 3000 Then
            Msg.Text = "Referenceの文字数が3000を超えています。"
            Exit Sub
        End If
        If Comment.Text.Length > 3000 Then
            Msg.Text = "Commentの文字数が3000を超えています。"
            Exit Sub
        End If

        '[CASNumberチェック]-----------------------------------------------------------
        NumberType = ""
        If TCICommon.Func.IsCASNumber(ProductNumber.Text.ToString) = True Then NumberType = "CAS"
        If TCICommon.Func.IsProductNumber(ProductNumber.Text.ToString) = True Then NumberType = "TCI"
        If TCICommon.Func.IsNewProductNumber(ProductNumber.Text.ToString) = True Then NumberType = "NEW"
        If NumberType = "" Then
            Msg.Text = "Product Number Typeが決定できません。"
            Exit Sub
        End If
        If CASNumber.Text <> "" Then
            If TCICommon.Func.IsCASNumber(CASNumber.Text.ToString) = False Then
                Msg.Text = "ERROR CAS_Number"
                Exit Sub
            Else
                If NumberType = "CAS" Then
                    If ProductNumber.Text <> CASNumber.Text Then
                        Msg.Text = "ProductNumberとCAS_Numberが異なります。"
                        Exit Sub
                    End If
                End If
            End If
        Else
            If NumberType = "CAS" Then
                CASNumber.Text = ProductNumber.Text
            End If
        End If

        '[Save処理]--------------------------------------------------------------------
        If Mode.Value = "Edit" Then
            '[ProductのUpdateDateチェック]-----------------------------------------------------------
            DBCommand.CommandText = "SELECT UpdateDate FROM dbo.Product WHERE ProductID = '" & ProductID.Value & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = False Then
                DBReader.Close()
                Msg.Text = "このデータは他のユーザーによって削除されました。"
                Exit Sub
            End If
            If Common.GetUpdateDate("Product", "ProductID", ProductID.Value) <> UpdateDate.Value Then
                DBReader.Close()
                Msg.Text = "データは他のユーザによって既に更新されています。ご確認ください。"
                Exit Sub
            End If
            DBReader.Close()

            '[Product更新処理]---------------------------------------------------------------
            st_SqlStr = "UPDATE dbo.Product SET ProductNumber="
            If ProductNumber.Text.ToString = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & Common.SafeSqlLiteral(ProductNumber.Text) & "',"
            st_SqlStr = st_SqlStr + "NumberType='" + NumberType + "',"
            st_SqlStr = st_SqlStr & "Name="
            If ProductName.Text.ToString = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & Common.SafeSqlLiteral(ProductName.Text) & "',"
            st_SqlStr = st_SqlStr & "QuoName="
            If QuoName.Text.ToString = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & Common.SafeSqlLiteral(QuoName.Text) & "',"
            st_SqlStr = st_SqlStr & "CASNumber="
            If CASNumber.Text.ToString = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & Common.SafeSqlLiteral(CASNumber.Text) & "',"
            st_SqlStr = st_SqlStr & "MolecularFormula="
            If MolecularFormula.Text.ToString = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & Common.SafeSqlLiteral(MolecularFormula.Text) & "',"
            st_SqlStr = st_SqlStr & "Reference="
            If Reference.Text.ToString = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & Common.SafeSqlLiteral(Reference.Text) & "',"
            st_SqlStr = st_SqlStr & "Comment="
            If Comment.Text.ToString = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & Common.SafeSqlLiteral(Comment.Text) & "',"
            st_SqlStr = st_SqlStr & "UpdatedBy=" & Session("UserID") & ", UpdateDate='" & Now() & "' "
            st_SqlStr = st_SqlStr & "WHERE ProductID = '" & ProductID.Value & "'"
            DBCommand.CommandText = st_SqlStr
            DBCommand.ExecuteNonQuery()
            Msg.Text = "データを更新しました。"

            '[引き続き更新処理ができるようにUpdateDate設定]----------------------------------
            UpdateDate.Value = Common.GetUpdateDate("Product", "ProductID", ProductID.Value) '[同時更新チェック用]
        Else
            '[Productの存在チェック]-----------------------------------------------------------
            DBCommand.CommandText = "SELECT ProductID FROM dbo.Product WHERE ProductNumber = '" & Common.SafeSqlLiteral(ProductNumber.Text) & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                Msg.Text = "このデータはすでに登録済です。その内容を確認し再度処理をお願いします。"
                Exit Sub
            End If
            DBReader.Close()

            '[Product登録処理]-----------------------------------------------------------------------
            st_SqlStr = "INSERT INTO Product (ProductNumber,NumberType,Name,QuoName,JapaneseName,ChineseName,CASNumber,MolecularFormula,Status,ProposalDept,ProcumentDept,PD,Reference,Comment,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ("
            If ProductNumber.Text.ToString = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + Common.SafeSqlLiteral(ProductNumber.Text) + "',"
            st_SqlStr = st_SqlStr + "'" + NumberType + "',"
            If ProductName.Text.ToString = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + Common.SafeSqlLiteral(ProductName.Text) + "',"
            If QuoName.Text.ToString = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + Common.SafeSqlLiteral(QuoName.Text) + "',"
            st_SqlStr = st_SqlStr + "null,null,"
            If CASNumber.Text.ToString = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + Common.SafeSqlLiteral(CASNumber.Text) + "',"
            If MolecularFormula.Text.ToString = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + Common.SafeSqlLiteral(MolecularFormula.Text) + "',"
            st_SqlStr = st_SqlStr + "null,null,null,null,"
            If Reference.Text.ToString = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + Common.SafeSqlLiteral(Reference.Text) + "',"
            If Comment.Text.ToString = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + Common.SafeSqlLiteral(Comment.Text) + "',"
            st_SqlStr = st_SqlStr + Session("UserID") + ",'" + Now() + "'," + Session("UserID") + ",'" + Now() + "'); "
            st_SqlStr = st_SqlStr & "SELECT ProductID FROM Product WHERE ProductID = SCOPE_IDENTITY()"  '←[新規登録されたProductIDの取得の為]
            DBCommand.CommandText = st_SqlStr
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                ProductID.Value = DBReader("ProductID")
                SupplierList.NavigateUrl = "./SupplierListByProduct.aspx?ProductID=" & ProductID.Value
            End If
            DBReader.Close()
            Msg.Text = "データを登録しました。"

            '[引き続き更新処理ができるようにUpdateDate設定]---------------------------------
            UpdateDate.Value = Common.GetUpdateDate("Product", "ProductID", ProductID.Value) '[同時更新チェック用]
            Mode.Value = "Edit"
            SupplierList.Visible = True
        End If
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
    End Sub
End Class
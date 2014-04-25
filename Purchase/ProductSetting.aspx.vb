Imports Purchase.Common

Partial Public Class ProductSetting
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(DB_CONNECT_STRING)
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
                    UpdateDate.Value = GetUpdateDate("Product", "ProductID", ProductID.Value) '[同時更新チェック用]
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
        Dim sb_SQL As StringBuilder = New StringBuilder()
        Msg.Text = String.Empty
        RunMsg.Text = String.Empty

        '[入力ProductNumberの正規化]---------------------------------------------------
        ProductNumber.Text = StrConv(ProductNumber.Text, VbStrConv.Narrow)
        ProductNumber.Text = UCase(ProductNumber.Text)

        '[Actionチェック]--------------------------------------------------------------
        If Action.Value <> "Save" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        '[必須項目入力チェック]--------------------------------------------------------
        If ProductNumber.Text = "" Then
            Msg.Text = "Product Number" + ERR_REQUIRED_FIELD
            Exit Sub
        End If
        If ProductName.Text = "" Then
            Msg.Text = "Product Name" + ERR_REQUIRED_FIELD
            Exit Sub
        End If

        '[ProductNumber重複チェック]---------------------------------------------------
        If Mode.Value = "Edit" Then
            DBCommand.CommandText = "SELECT ProductNumber FROM Product WHERE ProductNumber = '" & SafeSqlLiteral(ProductNumber.Text) & "' AND ProductID<>" & ProductID.Value
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                Msg.Text = "Your registering product number already exist.<br />(Please check again to avoid dupulication.)"   '"同じ Product Number のデータが既に登録されています。ご確認ください。"
                Exit Sub
            End If
            DBReader.Close()
        End If

        '[入力項目のLengthCheck]-------------------------------------------------------
        If GetByteCount_SJIS(MolecularFormula.Text) > 128 Then
            Msg.Text = "MolecularFormula" + ERR_OVER_128
            Exit Sub
        End If
        Reference.Text = Reference.Text.Trim        '入力データ前後の改行コード、タブコードを除去
        If Reference.Text.Length > INT_3000 Then
            Msg.Text = "Reference" + ERR_OVER_3000
            Exit Sub
        End If
        Comment.Text = Comment.Text.Trim            '入力データ前後の改行コード、タブコードを除去
        If Comment.Text.Length > INT_3000 Then
            Msg.Text = "Comment" + ERR_OVER_3000
            Exit Sub
        End If

        '[CASNumberチェック]-----------------------------------------------------------
        NumberType = ""
        If TCICommon.Func.IsCASNumber(ProductNumber.Text.ToString) = True Then NumberType = "CAS"
        If TCICommon.Func.IsProductNumber(ProductNumber.Text.ToString) = True Then NumberType = "TCI"
        If Common.IsNewProductNumber(ProductNumber.Text.ToString) = True Then NumberType = "NEW"
        If NumberType = "" Then
            Msg.Text = "Product Number" + ERR_INCORRECT_FORMAT   '"Product Number Typeが決定できません。"
            Exit Sub
        End If
        If CASNumber.Text <> "" Then
            If TCICommon.Func.IsCASNumber(CASNumber.Text.ToString) = False Then
                Msg.Text = "CAS Number" + ERR_INCORRECT_FORMAT   '"ERROR CAS_Number"
                Exit Sub
            Else
                If NumberType = "CAS" Then
                    If ProductNumber.Text <> CASNumber.Text Then
                        Msg.Text = "Please enter the CAS number in CAS section.<br />(When product number is CAS number, we need the same value in CAS section.)"   '"ProductNumberとCAS_Numberが異なります。"
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
        Dim st_ProductID As String = ProductID.Value
        Dim st_ProductNumber As String = ProductNumber.Text
        Dim st_ProductName As String = ProductName.Text
        Dim st_QuoName As String = QuoName.Text
        Dim st_JapaneseName As String = String.Empty
        Dim st_ChineseName As String = String.Empty
        Dim st_CASNumber As String = CASNumber.Text
        Dim st_MolecularFormula As String = MolecularFormula.Text
        Dim st_Status As String = String.Empty
        Dim st_ProposalDept As String = String.Empty
        Dim st_ProcumentDept As String = String.Empty
        Dim st_PD As String = String.Empty
        Dim st_Reference As String = Reference.Text
        Dim st_Comment As String = Comment.Text

        Dim MemoMode As String = Mode.Value
        If Mode.Value = "Edit" Then
            '[ProductのUpdateDateチェック]-----------------------------------------------------------
            DBCommand.CommandText = "SELECT UpdateDate FROM dbo.Product WHERE ProductID = '" & ProductID.Value & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = False Then
                DBReader.Close()
                Msg.Text = ERR_DELETED_BY_ANOTHER_USER   '"このデータは他のユーザーによって削除されました。"
                Exit Sub
            End If
            If GetUpdateDate("Product", "ProductID", ProductID.Value) <> UpdateDate.Value Then
                DBReader.Close()
                Msg.Text = ERR_UPDATED_BY_ANOTHER_USER   '"データは他のユーザによって既に更新されています。ご確認ください。"
                Exit Sub
            End If
            DBReader.Close()

            '[Product更新処理]---------------------------------------------------------------
            sb_SQL.Append("UPDATE dbo.Product ")
            sb_SQL.Append("SET ")
            sb_SQL.Append(" ProductNumber = @ProductNumber,")
            sb_SQL.Append(" NumberType = @NumberType,")
            sb_SQL.Append(" Name = @Name,")
            sb_SQL.Append(" QuoName = @QuoName,")
            sb_SQL.Append(" CASNumber = @CASNumber,")
            sb_SQL.Append(" MolecularFormula = @MolecularFormula,")
            sb_SQL.Append(" Reference = @Reference,")
            sb_SQL.Append(" Comment = @Comment,")
            sb_SQL.Append(" UpdatedBy = @UpdatedBy,")
            sb_SQL.Append(" UpdateDate = GETDATE() ")
            sb_SQL.Append("WHERE ")
            sb_SQL.Append(" ProductID = @ProductID")

            DBCommand.CommandText = sb_SQL.ToString()
            DBCommand.Parameters.AddWithValue("ProductNumber", ConvertEmptyStringToNull(st_ProductNumber))
            DBCommand.Parameters.AddWithValue("NumberType", ConvertEmptyStringToNull(NumberType))
            DBCommand.Parameters.AddWithValue("Name", ConvertEmptyStringToNull(st_ProductName))
            DBCommand.Parameters.AddWithValue("QuoName", ConvertEmptyStringToNull(st_QuoName))
            DBCommand.Parameters.AddWithValue("CASNumber", ConvertEmptyStringToNull(st_CASNumber))
            DBCommand.Parameters.AddWithValue("MolecularFormula", ConvertEmptyStringToNull(st_MolecularFormula))
            DBCommand.Parameters.AddWithValue("Reference", ConvertEmptyStringToNull(st_Reference))
            DBCommand.Parameters.AddWithValue("Comment", ConvertEmptyStringToNull(st_Comment))
            DBCommand.Parameters.AddWithValue("UpdatedBy", ConvertStringToInt(Session("UserID")))
            DBCommand.Parameters.AddWithValue("ProductID", ConvertStringToInt(st_ProductID))

            DBCommand.ExecuteNonQuery()
            RunMsg.Text = MSG_DATA_UPDATED   '"データを更新しました。"

            '[引き続き更新処理ができるようにUpdateDate設定]----------------------------------
            UpdateDate.Value = GetUpdateDate("Product", "ProductID", ProductID.Value) '[同時更新チェック用]
        Else
            '[Productの存在チェック]-----------------------------------------------------------
            DBCommand.CommandText = "SELECT ProductID FROM dbo.Product WHERE ProductNumber = '" & SafeSqlLiteral(ProductNumber.Text) & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                Msg.Text = "Your requested product number already exist.<br />(Please check again to avoid duplication.)"   '"このデータはすでに登録済です。その内容を確認し再度処理をお願いします。"
                Exit Sub
            End If
            DBReader.Close()

            '[Product登録処理]-----------------------------------------------------------------------
            sb_SQL.Append("INSERT INTO Product ")
            sb_SQL.Append("(")
            sb_SQL.Append(" ProductNumber,")
            sb_SQL.Append(" NumberType,")
            sb_SQL.Append(" Name,")
            sb_SQL.Append(" QuoName,")
            sb_SQL.Append(" JapaneseName,")
            sb_SQL.Append(" ChineseName,")
            sb_SQL.Append(" CASNumber,")
            sb_SQL.Append(" MolecularFormula,")
            sb_SQL.Append(" Status,")
            sb_SQL.Append(" ProposalDept,")
            sb_SQL.Append(" ProcumentDept,")
            sb_SQL.Append(" PD,")
            sb_SQL.Append(" Reference,")
            sb_SQL.Append(" Comment,")
            sb_SQL.Append(" CreatedBy,")
            sb_SQL.Append(" CreateDate,")
            sb_SQL.Append(" UpdatedBy,")
            sb_SQL.Append(" UpdateDate")
            sb_SQL.Append(") ")
            sb_SQL.Append("VALUES ")
            sb_SQL.Append("(")
            sb_SQL.Append(" @ProductNumber,")
            sb_SQL.Append(" @NumberType,")
            sb_SQL.Append(" @Name,")
            sb_SQL.Append(" @QuoName,")
            sb_SQL.Append(" @JapaneseName,")
            sb_SQL.Append(" @ChineseName,")
            sb_SQL.Append(" @CASNumber,")
            sb_SQL.Append(" @MolecularFormula,")
            sb_SQL.Append(" @Status,")
            sb_SQL.Append(" @ProposalDept,")
            sb_SQL.Append(" @ProcumentDept,")
            sb_SQL.Append(" @PD,")
            sb_SQL.Append(" @Reference,")
            sb_SQL.Append(" @Comment,")
            sb_SQL.Append(" @CreatedBy,")
            sb_SQL.Append(" GETDATE(),")
            sb_SQL.Append(" @UpdatedBy,")
            sb_SQL.Append(" GETDATE()")
            sb_SQL.Append(");")
            sb_SQL.Append("SELECT")
            sb_SQL.Append(" ProductID ")
            sb_SQL.Append("FROM Product ")
            sb_SQL.Append("WHERE ")
            sb_SQL.Append(" ProductID = SCOPE_IDENTITY()")

            DBCommand.CommandText = sb_SQL.ToString()
            DBCommand.Parameters.AddWithValue("ProductNumber", ConvertEmptyStringToNull(st_ProductNumber))
            DBCommand.Parameters.AddWithValue("NumberType", ConvertEmptyStringToNull(NumberType))
            DBCommand.Parameters.AddWithValue("Name", ConvertEmptyStringToNull(st_ProductName))
            DBCommand.Parameters.AddWithValue("QuoName", ConvertEmptyStringToNull(st_QuoName))
            DBCommand.Parameters.AddWithValue("JapaneseName", ConvertEmptyStringToNull(st_JapaneseName))
            DBCommand.Parameters.AddWithValue("ChineseName", ConvertEmptyStringToNull(st_ChineseName))
            DBCommand.Parameters.AddWithValue("CASNumber", ConvertEmptyStringToNull(st_CASNumber))
            DBCommand.Parameters.AddWithValue("MolecularFormula", ConvertEmptyStringToNull(st_MolecularFormula))
            DBCommand.Parameters.AddWithValue("Status", ConvertEmptyStringToNull(st_Status))
            DBCommand.Parameters.AddWithValue("ProposalDept", ConvertEmptyStringToNull(st_ProposalDept))
            DBCommand.Parameters.AddWithValue("ProcumentDept", ConvertEmptyStringToNull(st_ProcumentDept))
            DBCommand.Parameters.AddWithValue("PD", ConvertEmptyStringToNull(st_PD))
            DBCommand.Parameters.AddWithValue("Reference", ConvertEmptyStringToNull(st_Reference))
            DBCommand.Parameters.AddWithValue("Comment", ConvertEmptyStringToNull(st_Comment))
            DBCommand.Parameters.AddWithValue("CreatedBy", ConvertStringToInt(Session("UserID")))
            DBCommand.Parameters.AddWithValue("UpdatedBy", ConvertStringToInt(Session("UserID")))

            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                ProductID.Value = DBReader("ProductID")
                SupplierList.NavigateUrl = "./SupplierListByProduct.aspx?ProductID=" & ProductID.Value
            End If
            DBReader.Close()
            RunMsg.Text = MSG_DATA_CREATED   '"データを登録しました。"

            '[引き続き更新処理ができるようにUpdateDate設定]---------------------------------
            UpdateDate.Value = GetUpdateDate("Product", "ProductID", ProductID.Value) '[同時更新チェック用]
            Mode.Value = "Edit"
            SupplierList.Visible = True
        End If
        If MemoMode = "Edit" Then
            RunMsg.Text = MSG_DATA_UPDATED
        Else
            RunMsg.Text = MSG_DATA_CREATED
        End If
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
    End Sub
End Class
Public Partial Class RFQIssue
    Inherits CommonPage
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    Public DBConn As New System.Data.SqlClient.SqlConnection
    Public DBCommand As System.Data.SqlClient.SqlCommand

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim st_ProductID As String = ""
        Dim st_SupplierCode As String = ""
        Dim DBReader As System.Data.SqlClient.SqlDataReader
        DBConn.ConnectionString = DBConnectString.ConnectionString
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()
        If IsPostBack = False Then
            'パラメータチェック
            If Request.QueryString("ProductID") <> "" Then
                st_ProductID = Request.QueryString("ProductID")
                If IsNumeric(st_ProductID) Then
                    DBCommand.CommandText = "Select ProductNumber, Name FROM Product WHERE ProductID = @st_ProductID"
                    DBCommand.Parameters.Add("st_ProductID", SqlDbType.Int).Value = CInt(st_ProductID)
                    DBReader = DBCommand.ExecuteReader()
                    DBCommand.Dispose()
                    If DBReader.HasRows = True Then
                        While DBReader.Read
                            ProductNumber.Text = DBReader("ProductNumber").ToString
                            ProductName.Text = DBReader("Name").ToString
                        End While
                        ProductNumber.ReadOnly = True
                        ProductName.ReadOnly = True
                        ProductNumber.CssClass = "readonly"
                        ProductName.CssClass = "readonly"
                        ProductSelect.Visible = False
                    End If
                    DBReader.Close()
                End If
            End If
            If Request.QueryString("SupplierCode") <> "" Then
                st_SupplierCode = Request.QueryString("SupplierCode")
                If IsNumeric(st_SupplierCode) Then
                    DBCommand.CommandText = "SELECT R3SupplierCode, ISNULL(Name3, '') + ISNULL(Name4, '') AS SupplierName, CountryCode FROM Supplier WHERE SupplierCode = @st_SupplierCode"
                    DBCommand.Parameters.Add("st_SupplierCode", SqlDbType.Int).Value = CInt(st_SupplierCode)
                    DBReader = DBCommand.ExecuteReader()
                    DBCommand.Dispose()
                    If DBReader.HasRows = True Then
                        While DBReader.Read
                            R3SupplierCode.Text = DBReader("R3SupplierCode").ToString
                            SupplierName.Text = DBReader("SupplierName").ToString
                            SupplierCountry.Text = DBReader("CountryCode").ToString
                        End While
                        R3SupplierCode.ReadOnly = True
                        R3SupplierCode.CssClass = "readonly"
                        SupplierName.ReadOnly = True
                        SupplierName.CssClass = "readonly"
                        SupplierCountry.ReadOnly = True
                        SupplierCountry.CssClass = "readonly"
                        SupplierSelect.Visible = False
                    End If
                    DBReader.Close()
                End If
            End If
        Else
            'ReadOnly項目の再設定
            ProductName.Text = Request.Form("ProductName").ToString
            R3SupplierCode.Text = Request.Form("R3SupplierCode").ToString
            SupplierName.Text = Request.Form("SupplierName").ToString
            SupplierCountry.Text = Request.Form("SupplierCountry").ToString
            MakerName.Text = Request.Form("MakerName").ToString
            MakerCountry.Text = Request.Form("MakerCountry").ToString
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack = False Then
            Issue.PostBackUrl = "~/RFQIssue.aspx?Action=Issue"
        End If
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()

    End Sub

    Protected Sub Issue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Issue.Click
        Dim DBReader As System.Data.SqlClient.SqlDataReader
        Dim i As Integer = 0
        Dim st_Indispensability As String = ""
        Dim Bo_Line As Boolean = False
        Dim Bo_UnLine As Boolean = False
        Dim i_ProductID As Integer = -1
        Dim i_RFQNumber As Integer = -1

        If Request.QueryString("Action") <> "Issue" Then
            Exit Sub
        End If
        '必須入力項目チェックHeader
        If EnqLocation.SelectedValue = "" Then
            st_Indispensability = st_Indispensability & "Enq-Location "
        End If
        If EnqUser.SelectedValue = "" Then
            st_Indispensability = st_Indispensability & "Enq-User "
        End If
        If ProductNumber.Text = "" Then
            st_Indispensability = st_Indispensability & "ProductNumber "
        End If
        If SupplierCode.Text = "" Then
            st_Indispensability = st_Indispensability & "SupplierCode "
        End If
        If QuoLocation.SelectedValue = "" Then
            st_Indispensability = st_Indispensability & "Quo-Location "
        End If
        If Purpose.SelectedValue = "" Then
            st_Indispensability = st_Indispensability & "Purpose "
        End If
        If st_Indispensability <> "" Then
            Msg.Text = st_Indispensability & "を設定して下さい"
            Exit Sub
        End If

        '入力項目チェックLine
        If EnqQuantity_1.Text <> "" And EnqUnit_1.SelectedValue <> "" And EnqPiece_1.Text <> "" Then
            Bo_Line = True
        ElseIf EnqQuantity_1.Text = "" And EnqUnit_1.SelectedValue = "" And EnqPiece_1.Text = "" Then
        Else
            Bo_UnLine = True
        End If
        If EnqQuantity_2.Text <> "" And EnqUnit_2.SelectedValue <> "" And EnqPiece_2.Text <> "" Then
            Bo_Line = True
        ElseIf EnqQuantity_2.Text = "" And EnqUnit_2.SelectedValue = "" And EnqPiece_2.Text = "" Then
        Else
            Bo_UnLine = True
        End If
        If EnqQuantity_3.Text <> "" And EnqUnit_3.SelectedValue <> "" And EnqPiece_3.Text <> "" Then
            Bo_Line = True
        ElseIf EnqQuantity_3.Text = "" And EnqUnit_3.SelectedValue = "" And EnqPiece_3.Text = "" Then
        Else
            Bo_UnLine = True
        End If
        If EnqQuantity_4.Text <> "" And EnqUnit_4.SelectedValue <> "" And EnqPiece_4.Text <> "" Then
            Bo_Line = True
        ElseIf EnqQuantity_4.Text = "" And EnqUnit_4.SelectedValue = "" And EnqPiece_4.Text = "" Then
        Else
            Bo_UnLine = True
        End If
        If Bo_Line = False Then
            Msg.Text = "Enq-Quantity を設定して下さい"
            Exit Sub
        End If
        If Bo_UnLine = True Then
            Msg.Text = "Enq-Quantity の設定が不正です"
            Exit Sub
        End If

        '入力内容のチェック (とりあえずProductNumber以外は後回しで）
        DBCommand.CommandText = "SELECT ProductID FROM Product WHERE (ProductNumber = @st_ProductNumber)"
        DBCommand.Parameters.Add("st_ProductNumber", SqlDbType.VarChar, 32).Value = ProductNumber.Text
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.HasRows = True Then
            While DBReader.Read
                i_ProductID = IIf(IsNumeric(DBReader("ProductID").ToString) = True, CInt(DBReader("ProductID").ToString), -99)
            End While
        End If
        DBReader.Close()
        If i_ProductID = -1 Or i_ProductID = -99 Then
            Msg.Text = "ProductNumber の設定が不正です"
            Exit Sub
        End If


        Dim sqlTran As System.Data.SqlClient.SqlTransaction = DBConn.BeginTransaction()
        DBCommand.Transaction = sqlTran

        Try
            'Header登録
            DBCommand.CommandType = CommandType.Text
            DBCommand.CommandText = "INSERT INTO RFQHeader " _
                                  & "(EnqLocationCode, EnqUserID, QuoLocationCode, QuoUserID, " _
                                  & "ProductID, SupplierCode, MakerCode, PurposeCode, RequiredPurity, " _
                                  & "RequiredQMMethod, RequiredSpecification, Comment, RFQStatusCode, CreatedBy, UpdatedBy)" _
                                  & "VALUES(@EnqLocationCode, @EnqUserID, @QuoLocationCode, @QuoUserID, " _
                                  & "@ProductID, @SupplierCode, @MakerCode, @PurposeCode, @RequiredPurity, " _
                                  & "@RequiredQMMethod, @RequiredSpecification, @Comment, @RFQStatusCode, @CreatedBy, @UpdatedBy); " _
                                  & " SELECT RFQNumber FROM RFQHeader WHERE (RFQNumber = SCOPE_IDENTITY())"
            Dim param1 As System.Data.SqlClient.SqlParameter
            Dim param2 As System.Data.SqlClient.SqlParameter
            Dim param3 As System.Data.SqlClient.SqlParameter
            Dim param4 As System.Data.SqlClient.SqlParameter
            Dim param5 As System.Data.SqlClient.SqlParameter
            Dim param6 As System.Data.SqlClient.SqlParameter
            Dim param7 As System.Data.SqlClient.SqlParameter
            Dim param8 As System.Data.SqlClient.SqlParameter
            Dim param9 As System.Data.SqlClient.SqlParameter
            Dim param10 As System.Data.SqlClient.SqlParameter
            Dim param11 As System.Data.SqlClient.SqlParameter
            Dim param12 As System.Data.SqlClient.SqlParameter
            Dim param13 As System.Data.SqlClient.SqlParameter
            Dim param14 As System.Data.SqlClient.SqlParameter
            Dim param15 As System.Data.SqlClient.SqlParameter

            param1 = DBCommand.Parameters.Add("@EnqLocationCode", SqlDbType.VarChar, 5)
            param2 = DBCommand.Parameters.Add("@EnqUserID", SqlDbType.Int)
            param3 = DBCommand.Parameters.Add("@QuoLocationCode", SqlDbType.VarChar, 5)
            param4 = DBCommand.Parameters.Add("@QuoUserID", SqlDbType.Int)
            param5 = DBCommand.Parameters.Add("@ProductID", SqlDbType.Int)
            param6 = DBCommand.Parameters.Add("@SupplierCode", SqlDbType.Int)
            param7 = DBCommand.Parameters.Add("@MakerCode", SqlDbType.Int)
            param8 = DBCommand.Parameters.Add("@PurposeCode", SqlDbType.VarChar, 5)
            param9 = DBCommand.Parameters.Add("@RequiredPurity", SqlDbType.NVarChar, 255)
            param10 = DBCommand.Parameters.Add("@RequiredQMMethod", SqlDbType.NVarChar, 255)
            param11 = DBCommand.Parameters.Add("@RequiredSpecification", SqlDbType.NVarChar, 255)
            param12 = DBCommand.Parameters.Add("@Comment", SqlDbType.NVarChar, 3000)
            param13 = DBCommand.Parameters.Add("@RFQStatusCode", SqlDbType.VarChar, 5)
            param14 = DBCommand.Parameters.Add("@CreatedBy", SqlDbType.Int)
            param15 = DBCommand.Parameters.Add("@UpdatedBy", SqlDbType.Int)

            param1.Value = EnqLocation.SelectedValue
            param2.Value = EnqUser.SelectedValue
            param3.Value = IIf(QuoLocation.SelectedValue = "Direct", System.DBNull.Value, QuoLocation.SelectedValue)
            param4.Value = IIf(IsNumeric(QuoUser.SelectedValue) = True, QuoUser.SelectedValue, System.DBNull.Value)
            param5.Value = i_ProductID
            param6.Value = SupplierCode.Text
            param7.Value = IIf(IsNumeric(MakerCode.Text) = True, MakerCode.Text, System.DBNull.Value)
            param8.Value = Purpose.SelectedValue
            param9.Value = RequiredPurity.Text
            param10.Value = RequiredQMMethod.Text
            param11.Value = RequiredSpecification.Text
            param12.Value = Comment.Text
            param13.Value = IIf(QuoLocation.SelectedValue = "Direct", "N", IIf(IsNumeric(QuoUser.SelectedValue) = True, "A", ""))
            param14.Value = CInt(Session("UserID"))
            param15.Value = CInt(Session("UserID"))
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.HasRows = True Then
                While DBReader.Read
                    i_RFQNumber = IIf(IsNumeric(DBReader("RFQNumber").ToString) = True, CInt(DBReader("RFQNumber").ToString), -99)
                End While
            End If
            DBReader.Close()

            'MsgBox(i_RFQNumber)
            'OK正常に取得できていることを確認した。




            sqlTran.Commit()
        Catch ex As Exception
            sqlTran.Rollback()
            Throw
        Finally
            DBCommand.Dispose()
        End Try
    End Sub






End Class
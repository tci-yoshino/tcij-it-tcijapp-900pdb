Imports System.Data.SqlClient
Imports Purchase.Common
Partial Public Class RFQIssue
    Inherits CommonPage
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    Public DBConn As New SqlConnection
    Public DBCommand As SqlCommand

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim st_ProductID As String = ""
        Dim st_SupplierCode As String = ""
        Dim DBReader As SqlDataReader
        DBConn.ConnectionString = DBConnectString.ConnectionString
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()
        If IsPostBack = False Then
            '他画面から取得するパラメータのチェック
            If Request.QueryString("ProductID") <> "" Or Request.Form("ProductID") <> "" Then
                st_ProductID = IIf(Request.QueryString("ProductID") <> "", Request.QueryString("ProductID"), Request.Form("ProductID"))
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
                        ProductNumber.CssClass = "readonly"
                        ProductSelect.Visible = False
                    End If
                    DBReader.Close()
                End If
            End If
            If Request.QueryString("SupplierCode") <> "" Or Request.Form("SupplierCode") <> "" Then
                st_SupplierCode = IIf(Request.QueryString("SupplierCode") <> "", Request.QueryString("SupplierCode"), Request.Form("SupplierCode"))
                If IsNumeric(st_SupplierCode) Then
                    DBCommand.CommandText = "SELECT SupplierCode, R3SupplierCode, ISNULL(Name3, '') + ISNULL(Name4, '') AS SupplierName, CountryCode FROM Supplier WHERE SupplierCode = @st_SupplierCode"
                    DBCommand.Parameters.Add("st_SupplierCode", SqlDbType.Int).Value = CInt(st_SupplierCode)
                    DBReader = DBCommand.ExecuteReader()
                    DBCommand.Dispose()
                    If DBReader.HasRows = True Then
                        While DBReader.Read
                            SupplierCode.Text = DBReader("SupplierCode").ToString
                            R3SupplierCode.Text = DBReader("R3SupplierCode").ToString
                            SupplierName.Text = DBReader("SupplierName").ToString
                            SupplierCountry.Text = DBReader("CountryCode").ToString
                        End While
                        SupplierCode.ReadOnly = True
                        SupplierCode.CssClass = "readonly"
                        SupplierSelect.Visible = False
                    End If
                    DBReader.Close()
                End If
            End If
            'ドロップダウン初期設定
            EnqLocation.SelectedValue = Session("LocationCode").ToString
            EnqLocation.DataBind()
            EnqUser.DataBind()
            EnqUser.SelectedValue = Session("UserID").ToString
            QuoLocation.SelectedValue = Session("LocationCode").ToString
            QuoLocation.DataBind()
            QuoUser.DataBind()
            QuoUser.SelectedValue = Session("UserID").ToString
            'QuoLocation.SelectedItem.Text = "Direct"
        Else
            'ReadOnly項目の再設定
            ProductName.Text = Request.Form("ProductName").ToString
            R3SupplierCode.Text = Request.Form("R3SupplierCode").ToString
            SupplierName.Text = Request.Form("SupplierName").ToString
            SupplierCountry.Text = Request.Form("SupplierCountry").ToString
            MakerName.Text = Request.Form("MakerName").ToString
            MakerCountry.Text = Request.Form("MakerCountry").ToString
            If SupplierCode.ReadOnly = True Then
                SupplierCode.Text = Request.Form("SupplierCode").ToString
            End If
            If ProductNumber.ReadOnly = True Then
                ProductNumber.Text = Request.Form("ProductNumber").ToString
            End If
        End If
        'Quo-Location
        'If QuoLocation.SelectedValue = Session("LocationCode") Then
        '    QuoLocation.DataTextField
        'End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack = False Then
            'Issueボタンクリック時にPostBackするActionを追記する。
            Issue.PostBackUrl = "~/RFQIssue.aspx?Action=Issue"
        End If
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'DB切断
        DBConn.Close()
    End Sub

    Protected Sub Issue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Issue.Click
        Dim DBReader As SqlDataReader
        Dim i As Integer = 0
        Dim st_Indispensability As String = ""
        Dim Bo_Line As Boolean = False
        Dim Bo_UnLine As Boolean = False
        Dim i_ProductID As Integer = -1
        Dim i_SupplierCode As Integer = -1
        Dim i_MakerCode As Integer = -1
        Dim i_RFQNumber As Integer = -1
        Dim Enq_Quantity1 As Boolean = False
        Dim Enq_Quantity2 As Boolean = False
        Dim Enq_Quantity3 As Boolean = False
        Dim Enq_Quantity4 As Boolean = False
        Dim i_Result As Integer
        Msg.Text = ""
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
        If Integer.TryParse(SupplierCode.Text, i_Result) = False Then
            Msg.Text = "SupplierCode の設定が不正です"
            Exit Sub
        End If
        If MakerCode.Text = "" Then
            'MakerCodeは省略可能
        ElseIf Integer.TryParse(MakerCode.Text, i_Result) Then
            '数値に変換できた場合の処理(小数点含まず)は正常
        Else
            '数値に変換できなかった場合の処理(小数点含む場合もこちら)は入力値不正
            Msg.Text = "MakerCode の設定が不正です"
            Exit Sub
        End If
        '入力項目チェックLine
        If EnqQuantity_1.Text <> "" And EnqUnit_1.SelectedValue <> "" And EnqPiece_1.Text <> "" Then
            If Regex.IsMatch(EnqQuantity_1.Text, DECIMAL_7_3_REGEX) = False Or Integer.TryParse(EnqPiece_1.Text, i_Result) = False Then
                Bo_UnLine = True
            Else
                Enq_Quantity1 = True
            End If
            Bo_Line = True
        ElseIf EnqQuantity_1.Text = "" And EnqUnit_1.SelectedValue = "" And EnqPiece_1.Text = "" Then
        Else
            Bo_UnLine = True
        End If
        If EnqQuantity_2.Text <> "" And EnqUnit_2.SelectedValue <> "" And EnqPiece_2.Text <> "" Then
            If Regex.IsMatch(EnqQuantity_2.Text, DECIMAL_7_3_REGEX) = False Or Integer.TryParse(EnqPiece_2.Text, i_Result) = False Then
                Bo_UnLine = True
            Else
                Enq_Quantity2 = True
            End If
            Bo_Line = True
        ElseIf EnqQuantity_2.Text = "" And EnqUnit_2.SelectedValue = "" And EnqPiece_2.Text = "" Then
        Else
            Bo_UnLine = True
        End If
        If EnqQuantity_3.Text <> "" And EnqUnit_3.SelectedValue <> "" And EnqPiece_3.Text <> "" Then
            If Regex.IsMatch(EnqQuantity_3.Text, DECIMAL_7_3_REGEX) = False Or Integer.TryParse(EnqPiece_3.Text, i_Result) = False Then
                Bo_UnLine = True
            Else
                Enq_Quantity3 = True
            End If
            Bo_Line = True
        ElseIf EnqQuantity_3.Text = "" And EnqUnit_3.SelectedValue = "" And EnqPiece_3.Text = "" Then
        Else
            Bo_UnLine = True
        End If
        If EnqQuantity_4.Text <> "" And EnqUnit_4.SelectedValue <> "" And EnqPiece_4.Text <> "" Then
            If Regex.IsMatch(EnqQuantity_4.Text, DECIMAL_7_3_REGEX) = False Or Integer.TryParse(EnqPiece_4.Text, i_Result) = False Then
                Bo_UnLine = True
            Else
                Enq_Quantity4 = True
            End If
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

        '入力内容のチェック
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
        If RFQSupplierCheck(SupplierCode.Text) = False Then
            Msg.Text = "SupplierCode の設定が不正です"
            Exit Sub
        End If
        If MakerCode.Text <> "" Then
            If RFQSupplierCheck(MakerCode.Text) = False Then
                Msg.Text = "MakerCode の設定が不正です"
                Exit Sub
            End If
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

            DBCommand.Parameters.Add("@EnqLocationCode", SqlDbType.VarChar, 5).Value = EnqLocation.SelectedValue
            DBCommand.Parameters.Add("@EnqUserID", SqlDbType.Int).Value = EnqUser.SelectedValue
            DBCommand.Parameters.Add("@QuoLocationCode", SqlDbType.VarChar, 5).Value = IIf(QuoLocation.SelectedValue = "Direct", System.DBNull.Value, QuoLocation.SelectedValue)
            DBCommand.Parameters.Add("@QuoUserID", SqlDbType.Int).Value = IIf(IsNumeric(QuoUser.SelectedValue) = True, QuoUser.SelectedValue, System.DBNull.Value)
            DBCommand.Parameters.Add("@ProductID", SqlDbType.Int).Value = i_ProductID
            DBCommand.Parameters.Add("@SupplierCode", SqlDbType.Int).Value = SupplierCode.Text
            DBCommand.Parameters.Add("@MakerCode", SqlDbType.Int).Value = IIf(IsNumeric(MakerCode.Text) = True, MakerCode.Text, System.DBNull.Value)
            DBCommand.Parameters.Add("@PurposeCode", SqlDbType.VarChar, 5).Value = Purpose.SelectedValue
            DBCommand.Parameters.Add("@RequiredPurity", SqlDbType.NVarChar, 255).Value = IIf(Trim(RequiredPurity.Text) = "", System.DBNull.Value, RequiredPurity.Text)
            DBCommand.Parameters.Add("@RequiredQMMethod", SqlDbType.NVarChar, 255).Value = IIf(Trim(RequiredQMMethod.Text) = "", System.DBNull.Value, RequiredQMMethod.Text)
            DBCommand.Parameters.Add("@RequiredSpecification", SqlDbType.NVarChar, 255).Value = IIf(Trim(RequiredSpecification.Text) = "", System.DBNull.Value, RequiredSpecification.Text)
            DBCommand.Parameters.Add("@Comment", SqlDbType.NVarChar, 3000).Value = IIf(Trim(Comment.Text) = "", System.DBNull.Value, Comment.Text)
            DBCommand.Parameters.Add("@RFQStatusCode", SqlDbType.VarChar, 5).Value = IIf(QuoLocation.SelectedValue = "Direct", "N", IIf(IsNumeric(QuoUser.SelectedValue) = True, "A", ""))
            DBCommand.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = CInt(Session("UserID"))
            DBCommand.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = CInt(Session("UserID"))
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            'Header登録と登録時のIDを取得
            If DBReader.HasRows = True Then
                While DBReader.Read
                    'ID取得部分
                    i_RFQNumber = IIf(IsNumeric(DBReader("RFQNumber").ToString) = True, CInt(DBReader("RFQNumber").ToString), -99)
                End While
            End If
            DBReader.Close()
            'Line登録
            '登録用SQL文作成
            DBCommand.CommandText = "INSERT INTO RFQLine " _
              & "(RFQNumber, EnqQuantity, EnqUnitCode, EnqPiece, SupplierItemNumber, CreatedBy, UpdatedBy) " _
              & "VALUES(@RFQNumber, @EnqQuantity, @EnqUnitCode, @EnqPiece, " _
              & "@SupplierItemNumber, @CreatedBy, @UpdatedBy); "

            Dim param1 As SqlParameter = DBCommand.Parameters.Add("@RFQNumber", SqlDbType.Int)
            Dim param2 As SqlParameter = DBCommand.Parameters.Add("@EnqQuantity", SqlDbType.Decimal)
            Dim param3 As SqlParameter = DBCommand.Parameters.Add("@EnqUnitCode", SqlDbType.VarChar, 5)
            Dim param4 As SqlParameter = DBCommand.Parameters.Add("@EnqPiece", SqlDbType.Int)
            Dim param5 As SqlParameter = DBCommand.Parameters.Add("@SupplierItemNumber", SqlDbType.VarChar, 128)
            param1.Value = i_RFQNumber
            '画面内各行の入力欄が条件を満たしていたらTrueになっているため、Trueの行を登録する。
            If Enq_Quantity1 = True Then
                param2.Value = EnqQuantity_1.Text
                param3.Value = EnqUnit_1.SelectedValue
                param4.Value = EnqPiece_1.Text
                param5.Value = IIf(SupplierItemNumber_1.Text = "", System.DBNull.Value, SupplierItemNumber_1.Text)
                DBCommand.ExecuteNonQuery()
            End If
            If Enq_Quantity2 = True Then
                param2.Value = EnqQuantity_2.Text
                param3.Value = EnqUnit_2.SelectedValue
                param4.Value = EnqPiece_2.Text
                param5.Value = IIf(SupplierItemNumber_2.Text = "", System.DBNull.Value, SupplierItemNumber_2.Text)
                DBCommand.ExecuteNonQuery()
            End If
            If Enq_Quantity3 = True Then
                param2.Value = EnqQuantity_3.Text
                param3.Value = EnqUnit_3.SelectedValue
                param4.Value = EnqPiece_3.Text
                param5.Value = IIf(SupplierItemNumber_3.Text = "", System.DBNull.Value, SupplierItemNumber_3.Text)
                DBCommand.ExecuteNonQuery()
            End If
            If Enq_Quantity4 = True Then
                param2.Value = EnqQuantity_4.Text
                param3.Value = EnqUnit_4.SelectedValue
                param4.Value = EnqPiece_4.Text
                param5.Value = IIf(SupplierItemNumber_4.Text = "", System.DBNull.Value, SupplierItemNumber_4.Text)
                DBCommand.ExecuteNonQuery()
            End If
            sqlTran.Commit()
            Response.Redirect("RFQUpdate.aspx?RFQNumber=" & i_RFQNumber, False)
        Catch ex As Exception
            If IsNothing(sqlTran.Connection) = False Then
                'コミット後の処理があるため、コミットしてなかったらロールバックする。
                sqlTran.Rollback()
            End If
            Throw
        Finally
            DBCommand.Dispose()
        End Try
    End Sub
    Public Function RFQSupplierCheck(ByVal SupplierCode As String) As Boolean
        'Supplier 存在チェック
        RFQSupplierCheck = False
        Dim RFQConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
        Dim RFQConn As New SqlConnection
        Dim RFQCom As SqlCommand
        Dim RFQRead As SqlDataReader
        Dim i As Integer

        If Integer.TryParse(SupplierCode, i) = False Then
            Exit Function
        End If
        RFQConn.ConnectionString = RFQConnectString.ConnectionString
        RFQConn.Open()
        RFQCom = RFQConn.CreateCommand()

        RFQCom.CommandText = "SELECT SupplierCode FROM Supplier WHERE (SupplierCode = @st_SupplierCode)"
        RFQCom.Parameters.Add("st_SupplierCode", SqlDbType.Int).Value = CInt(SupplierCode)
        RFQRead = RFQCom.ExecuteReader()
        RFQCom.Dispose()
        If RFQRead.HasRows = True Then
            RFQSupplierCheck = True
        End If
        RFQRead.Close()
        RFQConn.Close()
    End Function

    Protected Sub EnqLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles EnqLocation.SelectedIndexChanged
        'EnqLocationの変更をQuoLocationと連動させる。
        QuoLocation.SelectedValue = EnqLocation.SelectedValue
        QuoLocation.DataBind()
        QuoUser.DataBind()
        QuoLocation.SelectedItem.Text = "Direct"
    End Sub
End Class
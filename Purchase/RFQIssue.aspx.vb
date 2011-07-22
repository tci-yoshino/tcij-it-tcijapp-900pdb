Imports System.Data.SqlClient
Imports Purchase.Common
Partial Public Class RFQIssue
    Inherits CommonPage
    Private DBConn As New SqlConnection
    Private DBCommand As SqlCommand
    'エラーメッセージ(入力値不正)
    Private Const ERR_INCORRECT_SUPPLIERCODE As String = "Supplier Code" & ERR_DOES_NOT_EXIST
    Private Const ERR_INCORRECT_MAKERCODE As String = "Maker Code" & ERR_DOES_NOT_EXIST
    Private Const ERR_INCORRECT_ENQQUANTITY As String = "Enq-Quantity" & ERR_INCORRECT_FORMAT
    Private Const ERR_INCORRECT_PRODUCTNUMBER As String = "Product Number" & ERR_DOES_NOT_EXIST
    'エラーメッセージ(必須入力項目なし)
    Private Const ERR_REQUIRED_ENQLOCATION As String = "Enq-Location" & ERR_REQUIRED_FIELD
    Private Const ERR_REQUIRED_ENQUSER As String = "Enq-User" & ERR_REQUIRED_FIELD
    Private Const ERR_REQUIRED_PRODUCTNUMBER As String = "ProductNumber" & ERR_REQUIRED_FIELD
    Private Const ERR_REQUIRED_SUPPLIERCODE As String = "SupplierCode" & ERR_REQUIRED_FIELD
    Private Const ERR_REQUIRED_QUOLOCATION As String = "Quo-Location" & ERR_REQUIRED_FIELD
    Private Const ERR_REQUIRED_PURPOSE As String = "Purpose" & ERR_REQUIRED_FIELD
    Private Const ERR_REQUIRED_ENQQUANTITY As String = "Please enter an item."
    'Private Const ERR_ISCASNUMBER As String = "You can not enquire with CAS Number. Please convert it into either ""New Product Registry Number"" or ""TCI Product Number""."
    'エラーメッセージ(文字数制限オーバー)
    Private Const ERR_COMMENT_OVER As String = "Comment" & ERR_OVER_3000
    Protected Parameter As Boolean = True

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        DBConn.ConnectionString = DB_CONNECT_STRING
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()
        If IsPostBack = False Then
            Call SetPostBackUrl()
            If CheckPram() = False Then
                Msg.Text = ERR_INVALID_PARAMETER
                '画面上の入力項目を隠す。
                Parameter = False
                Exit Sub
            End If
            Call InitDropDownList()
        Else
            Call SetReadOnlyItems()
        End If
        Call SetOnClientClick()
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'DB切断
        DBConn.Close()
    End Sub

    Protected Sub Issue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Issue.Click

        Dim i As Integer = 0
        Dim i_RFQNumber As Integer = -1
        Dim i_ProductID As Integer = -1
        Dim Enq_Quantity1 As Boolean = False
        Dim Enq_Quantity2 As Boolean = False
        Dim Enq_Quantity3 As Boolean = False
        Dim Enq_Quantity4 As Boolean = False
        Msg.Text = ""
        If Request.QueryString("Action") <> "Issue" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        If CheckRFQHeader() = False Then
            Exit Sub
        End If
        If CheckRFQLine(Enq_Quantity1, Enq_Quantity2, Enq_Quantity3, Enq_Quantity4) = False Then
            Exit Sub
        End If
        If CheckInsertColumn(ProductNumber.Text, i_ProductID) = False Then
            Exit Sub
        End If

        Dim sqlTran As System.Data.SqlClient.SqlTransaction = DBConn.BeginTransaction()
        DBCommand.Transaction = sqlTran
        Try
            'RFQHeader登録処理
            Dim DBReader As SqlDataReader
            DBCommand.CommandType = CommandType.Text
            DBCommand.CommandText = "INSERT INTO RFQHeader " _
                                  & "(EnqLocationCode, EnqUserID, QuoLocationCode, QuoUserID, " _
                                  & "ProductID, SupplierCode, MakerCode, PurposeCode, RequiredPurity, " _
                                  & "RequiredQMMethod, RequiredSpecification, Comment, RFQStatusCode, CreatedBy, UpdatedBy)" _
                                  & "VALUES(@EnqLocationCode, @EnqUserID, @QuoLocationCode, @QuoUserID, " _
                                  & "@ProductID, @SupplierCode, @MakerCode, @PurposeCode, @RequiredPurity, " _
                                  & "@RequiredQMMethod, @RequiredSpecification, @Comment, @RFQStatusCode, @CreatedBy, @UpdatedBy); " _
                                  & " SELECT RFQNumber FROM RFQHeader WHERE (RFQNumber = SCOPE_IDENTITY())"

            DBCommand.Parameters.Add("@EnqLocationCode", SqlDbType.VarChar).Value = EnqLocation.SelectedValue
            DBCommand.Parameters.Add("@EnqUserID", SqlDbType.Int).Value = EnqUser.SelectedValue
            DBCommand.Parameters.Add("@QuoLocationCode", SqlDbType.VarChar).Value = QuoLocation.SelectedValue
            DBCommand.Parameters.Add("@QuoUserID", SqlDbType.Int).Value = ConvertEmptyStringToNull(QuoUser.SelectedValue)
            DBCommand.Parameters.Add("@ProductID", SqlDbType.Int).Value = i_ProductID
            DBCommand.Parameters.Add("@SupplierCode", SqlDbType.Int).Value = SupplierCode.Text
            DBCommand.Parameters.Add("@MakerCode", SqlDbType.Int).Value = ConvertEmptyStringToNull(MakerCode.Text)
            DBCommand.Parameters.Add("@PurposeCode", SqlDbType.VarChar).Value = Purpose.SelectedValue
            DBCommand.Parameters.Add("@RequiredPurity", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(RequiredPurity.Text)
            DBCommand.Parameters.Add("@RequiredQMMethod", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(RequiredQMMethod.Text)
            DBCommand.Parameters.Add("@RequiredSpecification", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(RequiredSpecification.Text)
            DBCommand.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(Comment.Text)
            DBCommand.Parameters.Add("@RFQStatusCode", SqlDbType.VarChar).Value = IIf(Integer.TryParse(QuoUser.SelectedValue, i) = True, "A", "N")
            DBCommand.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Integer.Parse(Session("UserID"))
            DBCommand.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = Integer.Parse(Session("UserID"))
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            'Header登録と登録時のIDを取得
            If DBReader.HasRows = True Then
                While DBReader.Read
                    'ID取得部分
                    i_RFQNumber = Integer.Parse(DBReader("RFQNumber").ToString)
                End While
            End If
            DBReader.Close()
            'RFQLine登録
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

        Catch ex As Exception
            sqlTran.Rollback()
            Throw
        Finally
            DBCommand.Dispose()
        End Try
        Response.Redirect("RFQUpdate.aspx?RFQNumber=" & i_RFQNumber, False)
    End Sub

    Private Sub SetParamForRFQLine(ByVal param1 As SqlParameter, ByVal param2 As SqlParameter, ByVal param3 As SqlParameter, ByVal param4 As SqlParameter, ByVal param5 As SqlParameter)
        param2.Value = EnqQuantity_1.Text
        param3.Value = EnqUnit_1.SelectedValue
        param4.Value = EnqPiece_1.Text
        param5.Value = IIf(SupplierItemNumber_1.Text = "", System.DBNull.Value, SupplierItemNumber_1.Text)
        DBCommand.ExecuteNonQuery()
    End Sub

    Private Function IsAllNullOfRFQList(ByVal EnqQuantity As String, ByVal EnqUnit As String, ByVal EnqPiece As String) As Boolean
        '全ての項目が空白かチェック
        If EnqQuantity.Trim = String.Empty And EnqUnit.Trim = String.Empty And EnqPiece.Trim = String.Empty Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function IsAllInputOfRFQList(ByVal EnqQuantity As String, ByVal EnqUnit As String, ByVal EnqPiece As String) As Boolean

        '量入力の必須チェック
        If EnqQuantity.Trim = String.Empty Then
            Return False
        End If

        '単位入力の必須チェック
        If EnqUnit.Trim = String.Empty Then
            Return False
        End If

        '数量入力の必須チェック
        If EnqPiece.Trim = String.Empty Then
            Return False
        End If

        Return True
    End Function

    Private Function IsCheckRFQLineFormat(ByVal EnqQuantity As String, ByVal EnqPiece As String) As Boolean
        '量入力の書式チェック
        If Regex.IsMatch(EnqQuantity.Trim, DECIMAL_7_3_REGEX) = False Then
            Return False
        End If

        '数量入力の整数チェック
        Dim i_Result As Integer = 0
        If Regex.IsMatch(EnqPiece.Trim, INT_5_REGEX) = False Then
            Return False
        End If
        Return True
    End Function

    Protected Sub QuoLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles QuoLocation.SelectedIndexChanged
        'QuoUser ドロップダウンリストの初期化
        QuoUser.Items.Clear()
        QuoUser.Items.Add(String.Empty)
        '1行目に空行を追加する。
    End Sub
    Private Sub SetPostBackUrl()
        'ボタンクリック時にPostBackするActionを追記する。
        Issue.PostBackUrl = "~/RFQIssue.aspx?Action=Issue"
    End Sub
    Private Function CheckPram() As Boolean
        '他画面から取得するパラメータのチェック
        Dim st_ProductID As String = ""
        Dim st_SupplierCode As String = ""
        Dim DBReader As SqlDataReader
        Dim sb_Sql As New StringBuilder

        If Request.QueryString("ProductID") <> "" Or Request.Form("ProductID") <> "" Then
            st_ProductID = IIf(Request.QueryString("ProductID") <> "", Request.QueryString("ProductID"), Request.Form("ProductID"))
            If IsNumeric(st_ProductID) Then
                DBCommand.CommandText = "Select ProductNumber, Name, QuoName FROM Product WHERE ProductID = @i_ProductID"
                DBCommand.Parameters.Add("i_ProductID", SqlDbType.Int).Value = Integer.Parse(st_ProductID)
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.HasRows = True Then
                    While DBReader.Read
                        ProductNumber.Text = DBReader("ProductNumber").ToString
                        ProductName.Text = IIf(DBReader("QuoName").ToString.Trim = String.Empty, DBReader("Name").ToString, DBReader("QuoName").ToString)
                    End While
                    ProductNumber.ReadOnly = True
                    ProductNumber.CssClass = "readonly"
                    ProductSelect.Visible = False
                Else
                    Return False
                End If
                DBReader.Close()
            Else
                Return False
            End If
        End If
        If Request.QueryString("SupplierCode") <> "" Or Request.Form("SupplierCode") <> "" Then
            st_SupplierCode = IIf(Request.QueryString("SupplierCode") <> "", Request.QueryString("SupplierCode"), Request.Form("SupplierCode"))
            If IsNumeric(st_SupplierCode) Then
                sb_Sql.AppendLine("SELECT")
                sb_Sql.AppendLine("  S.SupplierCode,")
                sb_Sql.AppendLine("  S.R3SupplierCode,")
                sb_Sql.AppendLine("  ISNULL(S.Name3, '') + ' ' + ISNULL(S.Name4, '') AS SupplierName,")
                sb_Sql.AppendLine("  S.CountryCode,")
                sb_Sql.AppendLine("  I.QuoLocationCode AS DefaultQuoLocationCode")
                sb_Sql.AppendLine("")
                sb_Sql.AppendLine("FROM")
                sb_Sql.AppendLine("  Supplier AS S")
                sb_Sql.AppendLine("  LEFT OUTER JOIN IrregularRFQLocation AS I")
                sb_Sql.AppendLine("    ON I.SupplierCode = S.SupplierCode AND I.EnqLocationCode = @EnqLocationCode")
                sb_Sql.AppendLine("WHERE")
                sb_Sql.AppendLine("  S.SupplierCode = @SupplierCode")

                DBCommand.CommandText = sb_Sql.ToString
                DBCommand.Parameters.Add("EnqLocationCode", SqlDbType.VarChar).Value = Session("LocationCode").ToString
                DBCommand.Parameters.Add("SupplierCode", SqlDbType.Int).Value = Integer.Parse(st_SupplierCode)
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.HasRows = True Then
                    While DBReader.Read
                        SupplierCode.Text = DBReader("SupplierCode").ToString
                        R3SupplierCode.Text = DBReader("R3SupplierCode").ToString
                        SupplierName.Text = DBReader("SupplierName").ToString
                        Call SetCountryName(DBReader("CountryCode").ToString, DBReader("DefaultQuoLocationCode").ToString)
                    End While
                    SupplierCode.ReadOnly = True
                    SupplierCode.CssClass = "readonly"
                    SupplierSelect.Visible = False
                Else
                    Return False
                End If
                DBReader.Close()
            Else
                Return False
            End If
        End If
        Return True
    End Function
    Private Sub InitDropDownList()
        'ドロップダウン初期設定
        EnqLocation.SelectedValue = Session("LocationCode").ToString
        EnqLocation.DataBind()
        If Session("Purchase.isAdmin") = False Then
            EnqUser.SelectedValue = Session("UserID").ToString
        End If
        QuoLocation.DataBind()
        QuoUser.Items.Clear()
        QuoUser.Items.Add(String.Empty)
    End Sub
    Private Sub SetReadOnlyItems()
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
    End Sub
    Private Sub SetOnClientClick()
        'RFQSupplierSelect 画面へ遷移する際のパラメータを一部セットする。
        SupplierSelect.OnClientClick = _
        String.Format("return SupplierSelect_onclick(""" & _
                      Server.UrlEncode(ClientScript.GetPostBackEventReference(SupplierSelect, String.Empty)) _
                      & """)")
    End Sub
    Private Function CheckRFQHeader() As Boolean
        '必須入力項目チェックHeader
        Dim i_Result As Integer = 0

        If EnqLocation.SelectedValue = "" Then
            Msg.Text = ERR_REQUIRED_ENQLOCATION
            Return False
        End If
        If EnqUser.SelectedValue = "" Then
            Msg.Text = ERR_REQUIRED_ENQUSER
            Return False
        End If
        If ProductNumber.Text = "" Then
            Msg.Text = ERR_REQUIRED_PRODUCTNUMBER
            Return False

            'CAS からも RFQ が登録できるようにコメントアウトした。
            'ProductNumber が正しいかのチェックは CheckInsertColumn でされる。
            'ElseIf TCICommon.Func.IsCASNumber(ProductNumber.Text) = True Then 
            '    Msg.Text = ERR_ISCASNUMBER
            '    Return False
        End If
        If SupplierCode.Text = "" Then
            Msg.Text = ERR_REQUIRED_SUPPLIERCODE
            Return False
        End If
        If QuoLocation.SelectedValue = "" Then
            Msg.Text = ERR_REQUIRED_QUOLOCATION
            Return False
        End If
        If Purpose.SelectedValue = "" Then
            Msg.Text = ERR_REQUIRED_PURPOSE
            Return False
        End If
        If Integer.TryParse(SupplierCode.Text, i_Result) = False Then
            Msg.Text = ERR_INCORRECT_SUPPLIERCODE
            Return False
        End If
        If MakerCode.Text = "" Then
            'MakerCodeは省略可能
        ElseIf Integer.TryParse(MakerCode.Text, i_Result) Then
            '数値に変換できた場合の処理(小数点含まず)は正常
        Else
            '数値に変換できなかった場合の処理(小数点含む場合もこちら)は入力値不正
            Msg.Text = ERR_INCORRECT_MAKERCODE
            Return False
        End If
        '入力項目の文字数チェック
        If Comment.Text.Length > INT_3000 Then
            Msg.Text = ERR_COMMENT_OVER
            Exit Function
        End If
        Return True
    End Function
    Private Function CheckRFQLine(ByRef Enq_Quantity1 As Boolean, ByRef Enq_Quantity2 As Boolean, ByRef Enq_Quantity3 As Boolean, ByRef Enq_Quantity4 As Boolean) As Boolean
        '入力項目チェックLine
        Dim Bo_UnLine As Boolean = False

        Enq_Quantity1 = IsAllInputOfRFQList(EnqQuantity_1.Text, EnqUnit_1.SelectedValue, EnqPiece_1.Text)
        Dim bo_UnLine_1 = IsAllNullOfRFQList(EnqQuantity_1.Text, EnqUnit_1.SelectedValue, EnqPiece_1.Text)
        If Enq_Quantity1 = False And bo_UnLine_1 = False Then
            Bo_UnLine = True
        End If
        If IsCheckRFQLineFormat(EnqQuantity_1.Text, EnqPiece_1.Text) = False And bo_UnLine_1 = False Then
            Bo_UnLine = True
        End If

        Enq_Quantity2 = IsAllInputOfRFQList(EnqQuantity_2.Text, EnqUnit_2.SelectedValue, EnqPiece_2.Text)
        Dim bo_UnLine_2 = IsAllNullOfRFQList(EnqQuantity_2.Text, EnqUnit_2.SelectedValue, EnqPiece_2.Text)
        If Enq_Quantity2 = False And bo_UnLine_2 = False Then
            Bo_UnLine = True
        End If
        If IsCheckRFQLineFormat(EnqQuantity_2.Text, EnqPiece_2.Text) = False And bo_UnLine_2 = False Then
            Bo_UnLine = True
        End If

        Enq_Quantity3 = IsAllInputOfRFQList(EnqQuantity_3.Text, EnqUnit_3.SelectedValue, EnqPiece_3.Text)
        Dim bo_UnLine_3 = IsAllNullOfRFQList(EnqQuantity_3.Text, EnqUnit_3.SelectedValue, EnqPiece_3.Text)
        If Enq_Quantity3 = False And bo_UnLine_3 = False Then
            Bo_UnLine = True
        End If
        If IsCheckRFQLineFormat(EnqQuantity_3.Text, EnqPiece_3.Text) = False And bo_UnLine_3 = False Then
            Bo_UnLine = True
        End If

        Enq_Quantity4 = IsAllInputOfRFQList(EnqQuantity_4.Text, EnqUnit_4.SelectedValue, EnqPiece_4.Text)
        Dim bo_UnLine_4 = IsAllNullOfRFQList(EnqQuantity_4.Text, EnqUnit_4.SelectedValue, EnqPiece_4.Text)
        If Enq_Quantity4 = False And bo_UnLine_4 = False Then
            Bo_UnLine = True
        End If
        If IsCheckRFQLineFormat(EnqQuantity_4.Text, EnqPiece_4.Text) = False And bo_UnLine_4 = False Then
            Bo_UnLine = True
        End If
        If Enq_Quantity1 = False And Enq_Quantity2 = False And _
            Enq_Quantity3 = False And Enq_Quantity4 = False Then
            If Not Purpose.SelectedValue = "JFYI" Then
                'JFYI時は明細行なしで登録可能
                Msg.Text = ERR_REQUIRED_ENQQUANTITY
                Return False
            End If
        End If
        If Bo_UnLine = True Then
            Msg.Text = ERR_INCORRECT_ENQQUANTITY
            Return False
        End If
        Return True
    End Function
    Private Function CheckInsertColumn(ByVal CheckProductNumber As String, ByRef ReturnProductID As Integer) As Boolean
        'Insert内容の入力チェック ProductNumberからProductIDを取得して返す。
        '入力内容のチェック
        Dim DBReader As SqlDataReader
        Dim st_Supplier As String = "Supplier"
        Dim st_SupplierKey As String = "SupplierCode"
        'ProductNumberのチェック
        DBCommand.CommandText = "Select ProductID FROM Product WHERE ProductNumber = @ProductNumber"
        DBCommand.Parameters.Add("ProductNumber", SqlDbType.VarChar).Value = CheckProductNumber
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.HasRows = True Then
            While DBReader.Read
                ReturnProductID = DBReader("ProductID").ToString
            End While
        Else
            Msg.Text = ERR_INCORRECT_PRODUCTNUMBER
            Return False
        End If
        DBReader.Close()
        'Supplierのチェック
        If ExistenceConfirmation(st_Supplier, st_SupplierKey, SupplierCode.Text) = False Then
            Msg.Text = ERR_INCORRECT_SUPPLIERCODE
            Return False
        End If
        'Makerのチェック
        If MakerCode.Text <> "" Then
            If ExistenceConfirmation(st_Supplier, st_SupplierKey, MakerCode.Text) = False Then
                Msg.Text = ERR_INCORRECT_MAKERCODE
                Return False
            End If
        End If
        Return True
    End Function
    Private Sub SetCountryName(ByVal CountryCode As String, ByVal DefaultQuoLocationCode As String)
        Dim st_CountryName As String = String.Empty
        Dim st_DefaultQuoLocationName As String = String.Empty
        'SupplierCountryName取得
        Dim st_SQLCommand As String = String.Empty
        st_SQLCommand = "SELECT CountryName, DefaultQuoLocationCode FROM v_Country WHERE CountryCode = @st_CountryCode"
        Try
            Using DBConnection As New SqlClient.SqlConnection(DB_CONNECT_STRING), _
            DBSQLCommand As SqlCommand = DBConnection.CreateCommand()
                DBConnection.Open()
                DBSQLCommand.CommandText = st_SQLCommand
                DBSQLCommand.Parameters.AddWithValue("st_CountryCode", CountryCode)
                Dim DBSQLDataReader As SqlDataReader
                DBSQLDataReader = DBSQLCommand.ExecuteReader()
                If DBSQLDataReader.HasRows = True Then
                    While DBSQLDataReader.Read
                        SupplierCountry.Text = DBSQLDataReader("CountryName").ToString
                        QuoLocation.SelectedValue = IIf(DefaultQuoLocationCode = "", DBSQLDataReader("DefaultQuoLocationCode").ToString, DefaultQuoLocationCode)
                    End While
                End If
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Sub
End Class
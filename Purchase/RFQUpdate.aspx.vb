Imports System.Data.SqlClient
Imports Purchase.Common

Partial Public Class RFQUpdate
    Inherits CommonPage
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    Public DBConn As New System.Data.SqlClient.SqlConnection
    Public DBCommand As System.Data.SqlClient.SqlCommand
    Public DBAdapter As System.Data.SqlClient.SqlDataAdapter
    'エラーメッセージ(入力値不正)
    Private Const ERR_INCORRECT_SUPPLIERCODE As String = "SupplierCode" & ERR_INCORRECT_FORMAT
    Private Const ERR_INCORRECT_MAKERCODE As String = "MakerCode" & ERR_INCORRECT_FORMAT
    Private Const ERR_INCORRECT_SHIPPINGHANDLINGFEE As String = "ShippingHandlingFee" & ERR_INCORRECT_FORMAT
    Private Const ERR_INCORRECT_UNITPRICE As String = "UnitPrice" & ERR_INCORRECT_FORMAT
    Private Const ERR_INCORRECT_QUOPER As String = "Quo-Per" & ERR_INCORRECT_FORMAT
    'エラーメッセージ(必須入力項目)
    Private Const ERR_REQUIRED_SUPPLIERCODE As String = "SupplierCode" & ERR_REQUIRED_FIELD
    Private Const ERR_REQUIRED_QUOUSER As String = "Quo-User" & ERR_REQUIRED_FIELD
    'エラーメッセージ(他ユーザ更新)
    Private Const ERR_ALREADY_UPDATED As String = "このデータは他のユーザーによって編集されました。その内容を確認し再度編集をお願いします。"
    'エラーメッセージ(更新処理失敗)
    Private Const ERR_GET_RFQDATA_FAILURE As String = "RFQ データの取得に失敗しましたが、エラーが検出されませんでした。"
    'エラーメッセージ(他拠点情報更新)
    Private Const ERR_ANOTHER_LOCATION As String = "他拠点間のRFQ情報は更新できません。"
    '画面表示フラグ
    Protected Parameter As Boolean = True
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DBConn.ConnectionString = DBConnectString.ConnectionString
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        If IsPostBack = False Then
            Call SetPostBackUrl()
            If FormDataSet() = False Then
                Msg.Text = ERR_INVALID_PARAMETER
                '画面上の入力項目を隠す。
                Parameter = False
                Exit Sub
            End If
        Else
            Call SetReadOnlyItems()
        End If
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
    End Sub

    Protected Sub Update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Update.Click
        Dim RFQStatusCode As String = ""
        Dim st_QuotedDate As String = ""
        Msg.Text = ""
        If Request.QueryString("Action") <> "Update" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If
        If CheckSupplierCode() = False Then
            Exit Sub
        End If

        If ItemCheck() = False Then
            '入力された項目の型をチェックする(DB登録時にエラーになるもののみ)
            Exit Sub
        End If

        '他セッションでの更新チェック
        If CheckUpdatedate() = False Then
            Exit Sub
        End If
        '更新可能拠点の確認
        If CheckLocation() = False Then
            Exit Sub
        End If
        '更新処理
        Dim sqlTran As System.Data.SqlClient.SqlTransaction = DBConn.BeginTransaction()
        DBCommand.Transaction = sqlTran
        Try
            'RFQHeader の更新
            DBCommand.Parameters.Clear()
            If RFQStatus.SelectedValue <> "" Then
                RFQStatusCode = ", RFQStatusCode = @RFQStatusCode "
                DBCommand.Parameters.Add("@RFQStatusCode", SqlDbType.NVarChar).Value = RFQStatus.SelectedValue
            End If
            If QuotedDate.Value = "" Then
                'QuotedDateは初回のみ登録し上書きしない。登録条件はRFQStatusが「Q」or「PQ」
                If RFQStatus.SelectedValue = "Q" Or RFQStatus.SelectedValue = "PQ" Then
                    st_QuotedDate = ", QuotedDate = @st_QuotedDate "
                    DBCommand.Parameters.Add("@st_QuotedDate", SqlDbType.DateTime).Value = DateTime.Today
                End If
            End If
            DBCommand.CommandText = "Update RFQHeader SET QuoUserID = @QuoUserID, SupplierCode = @SupplierCode, MakerCode = @MakerCode," _
            & "SpecSheet = @SpecSheet, Specification = @Specification, SupplierContactPerson = @SupplierContactPerson," _
            & "SupplierItemName = @SupplierItemName, ShippingHandlingFee = @ShippingHandlingFee," _
            & "ShippingHandlingCurrencyCode = @ShippingHandlingCurrencyCode, PaymentTermCode = @PaymentTermCode," _
            & "Comment = @Comment, UpdatedBy = @UpdatedBy, UpdateDate = GETDATE()" & RFQStatusCode & st_QuotedDate _
            & " Where RFQNumber = @RFQNumber "
            DBCommand.Parameters.Add("@QuoUserID", SqlDbType.Int).Value = ConvertStringToInt(QuoUser.SelectedValue)
            DBCommand.Parameters.Add("@SupplierCode", SqlDbType.Int).Value = Integer.Parse(SupplierCode.Text)
            DBCommand.Parameters.Add("@MakerCode", SqlDbType.Int).Value = ConvertStringToInt(MakerCode.Text)
            DBCommand.Parameters.Add("@SpecSheet", SqlDbType.Bit).Value = SpecSheet.Checked
            DBCommand.Parameters.Add("@Specification", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(Specification.Text)
            DBCommand.Parameters.Add("@SupplierContactPerson", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(SupplierContactPerson.Text)
            DBCommand.Parameters.Add("@SupplierItemName", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(SupplierItemName.Text)
            DBCommand.Parameters.Add("@ShippingHandlingFee", SqlDbType.Decimal).Value = ConvertStringToDec(ShippingHandlingFee.Text)
            DBCommand.Parameters.Add("@ShippingHandlingCurrencyCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(ShippingHandlingCurrency.Text)
            DBCommand.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(PaymentTerm.SelectedValue)
            DBCommand.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(Comment.Text)
            DBCommand.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = Integer.Parse(Session("UserID"))
            DBCommand.Parameters.Add("@RFQNumber", SqlDbType.Int).Value = Integer.Parse(RFQNumber.Text)
            'DBCommand.Parameters.Add("@UpdateDate", SqlDbType.DateTime).Value = Now()
            DBCommand.ExecuteNonQuery()
            DBCommand.Parameters.Clear()
            DBCommand.Dispose()

            'RFQLine の更新
            DBCommand.CommandText = "UPDATE RFQLine SET CurrencyCode = @CurrencyCode, UnitPrice = @UnitPrice, " _
            & "QuoPer = @QuoPer, QuoUnitCode = @QuoUnitCode, LeadTime = @LeadTime, SupplierItemNumber = @SupplierItemNumber, " _
            & "IncotermsCode = @IncotermsCode, DeliveryTerm = @DeliveryTerm, Packing = @Packing, Purity = @Purity, " _
            & "QMMethod = @QMMethod, NoOfferReasonCode = @NoOfferReasonCode, UpdatedBy = @UpdatedBy, UpdateDate = GETDATE() " _
            & "Where RFQLineNumber = @RFQLineNumber"
            Dim param1 As SqlParameter = DBCommand.Parameters.Add("@RFQLineNumber", SqlDbType.Int)
            Dim param2 As SqlParameter = DBCommand.Parameters.Add("@CurrencyCode", SqlDbType.VarChar)
            Dim param3 As SqlParameter = DBCommand.Parameters.Add("@UnitPrice", SqlDbType.Decimal)
            Dim param4 As SqlParameter = DBCommand.Parameters.Add("@QuoPer", SqlDbType.Decimal)
            Dim param5 As SqlParameter = DBCommand.Parameters.Add("@QuoUnitCode", SqlDbType.VarChar)
            Dim param6 As SqlParameter = DBCommand.Parameters.Add("@LeadTime", SqlDbType.NVarChar)
            Dim param7 As SqlParameter = DBCommand.Parameters.Add("@SupplierItemNumber", SqlDbType.NVarChar)
            Dim param8 As SqlParameter = DBCommand.Parameters.Add("@IncotermsCode", SqlDbType.NVarChar)
            Dim param9 As SqlParameter = DBCommand.Parameters.Add("@DeliveryTerm", SqlDbType.NVarChar)
            Dim param10 As SqlParameter = DBCommand.Parameters.Add("@Packing", SqlDbType.NVarChar)
            Dim param11 As SqlParameter = DBCommand.Parameters.Add("@Purity", SqlDbType.NVarChar)
            Dim param12 As SqlParameter = DBCommand.Parameters.Add("@QMMethod", SqlDbType.NVarChar)
            Dim param13 As SqlParameter = DBCommand.Parameters.Add("@NoOfferReasonCode", SqlDbType.VarChar)
            DBCommand.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = Integer.Parse(Session("UserID"))
            If EnqQuantity_1.Text <> "" Then
                'RFQIssueで登録されたデータのみ更新可
                param1.Value = Integer.Parse(LineNumber1.Value)
                param2.Value = ConvertEmptyStringToNull(Currency_1.SelectedValue)
                param3.Value = ConvertStringToDec(UnitPrice_1.Text)
                param4.Value = ConvertStringToDec(QuoPer_1.Text)
                param5.Value = ConvertEmptyStringToNull(QuoUnit_1.SelectedValue)
                param6.Value = ConvertEmptyStringToNull(LeadTime_1.Text)
                param7.Value = ConvertEmptyStringToNull(SupplierItemNumber_1.Text)
                param8.Value = ConvertEmptyStringToNull(Incoterms_1.SelectedValue)
                param9.Value = ConvertEmptyStringToNull(DeliveryTerm_1.Text)
                param10.Value = ConvertEmptyStringToNull(Packing_1.Text)
                param11.Value = ConvertEmptyStringToNull(Purity_1.Text)
                param12.Value = ConvertEmptyStringToNull(QMMethod_1.Text)
                param13.Value = ConvertEmptyStringToNull(NoOfferReason_1.SelectedValue)
                DBCommand.ExecuteNonQuery()
            End If

            If EnqQuantity_2.Text <> "" Then
                param1.Value = Integer.Parse(LineNumber2.Value)
                param2.Value = ConvertEmptyStringToNull(Currency_2.SelectedValue)
                param3.Value = ConvertStringToDec(UnitPrice_2.Text)
                param4.Value = ConvertStringToDec(QuoPer_2.Text)
                param5.Value = ConvertEmptyStringToNull(QuoUnit_2.SelectedValue)
                param6.Value = ConvertEmptyStringToNull(LeadTime_2.Text)
                param7.Value = ConvertEmptyStringToNull(SupplierItemNumber_2.Text)
                param8.Value = ConvertEmptyStringToNull(Incoterms_2.SelectedValue)
                param9.Value = ConvertEmptyStringToNull(DeliveryTerm_2.Text)
                param10.Value = ConvertEmptyStringToNull(Packing_2.Text)
                param11.Value = ConvertEmptyStringToNull(Purity_2.Text)
                param12.Value = ConvertEmptyStringToNull(QMMethod_2.Text)
                param13.Value = ConvertEmptyStringToNull(NoOfferReason_2.SelectedValue)
                DBCommand.ExecuteNonQuery()
            End If
            If EnqQuantity_3.Text <> "" Then
                param1.Value = Integer.Parse(LineNumber3.Value)
                param2.Value = ConvertEmptyStringToNull(Currency_3.SelectedValue)
                param3.Value = ConvertStringToDec(UnitPrice_3.Text)
                param4.Value = ConvertStringToDec(QuoPer_3.Text)
                param5.Value = ConvertEmptyStringToNull(QuoUnit_3.SelectedValue)
                param6.Value = ConvertEmptyStringToNull(LeadTime_3.Text)
                param7.Value = ConvertEmptyStringToNull(SupplierItemNumber_3.Text)
                param8.Value = ConvertEmptyStringToNull(Incoterms_3.SelectedValue)
                param9.Value = ConvertEmptyStringToNull(DeliveryTerm_3.Text)
                param10.Value = ConvertEmptyStringToNull(Packing_3.Text)
                param11.Value = ConvertEmptyStringToNull(Purity_3.Text)
                param12.Value = ConvertEmptyStringToNull(QMMethod_3.Text)
                param13.Value = ConvertEmptyStringToNull(NoOfferReason_3.SelectedValue)
                DBCommand.ExecuteNonQuery()
            End If
            If EnqQuantity_4.Text <> "" Then
                param1.Value = Integer.Parse(LineNumber4.Value)
                param2.Value = ConvertEmptyStringToNull(Currency_4.SelectedValue)
                param3.Value = ConvertStringToDec(UnitPrice_4.Text)
                param4.Value = ConvertStringToDec(QuoPer_4.Text)
                param5.Value = ConvertEmptyStringToNull(QuoUnit_4.SelectedValue)
                param6.Value = ConvertEmptyStringToNull(LeadTime_4.Text)
                param7.Value = ConvertEmptyStringToNull(SupplierItemNumber_4.Text)
                param8.Value = ConvertEmptyStringToNull(Incoterms_4.SelectedValue)
                param9.Value = ConvertEmptyStringToNull(DeliveryTerm_4.Text)
                param10.Value = ConvertEmptyStringToNull(Packing_4.Text)
                param11.Value = ConvertEmptyStringToNull(Purity_4.Text)
                param12.Value = ConvertEmptyStringToNull(QMMethod_4.Text)
                param13.Value = ConvertEmptyStringToNull(NoOfferReason_4.SelectedValue)
                DBCommand.ExecuteNonQuery()
            End If
            sqlTran.Commit()
        Catch ex As Exception
            sqlTran.Rollback()
            Throw
        Finally
            DBCommand.Dispose()
        End Try
        If FormDataSet() = False Then
            '画面リフレッシュ
            Msg.Text = ERR_GET_RFQDATA_FAILURE
            '画面上の入力項目を隠す。
            Parameter = False
            Exit Sub
        End If
    End Sub

    Protected Sub Close_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Close.Click
        If Request.QueryString("Action") <> "Close" Then
            Exit Sub
        End If
        '他セッションでの更新チェック
        If CheckUpdatedate() = False Then
            Exit Sub
        End If
        '更新可能拠点の確認
        If CheckLocation() = False Then
            Exit Sub
        End If
        DBCommand.CommandText = "UPDATE RFQHeader SET RFQStatusCode = 'C', UpdatedBy = @UpdatedBy, UpdateDate = GETDATE() WHERE (RFQNumber = @RFQNumber)"
        DBCommand.Parameters.Add("@RFQNumber", SqlDbType.Int).Value = Integer.Parse(RFQNumber.Text)
        DBCommand.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = Integer.Parse(Session("UserID"))
        DBCommand.ExecuteNonQuery()
        DBCommand.Parameters.Clear()
        DBCommand.Dispose()
        If FormDataSet() = False Then
            '画面リフレッシュ
            Msg.Text = ERR_GET_RFQDATA_FAILURE
            '画面上の入力項目を隠す。
            Parameter = False
            Exit Sub
        End If
    End Sub

    Private Function FormDataSet() As Boolean
        Dim DS As DataSet = New DataSet
        Dim st_RFQNumber As String = String.Empty
        If IsPostBack = False Then
            If Request.QueryString("RFQNumber") <> "" Or Request.Form("RFQNumber") <> "" Then
                st_RFQNumber = IIf(Request.QueryString("RFQNumber") <> "", Request.QueryString("RFQNumber"), Request.Form("RFQNumber"))
            Else
                'パラメータが渡されない場合、エラーメッセージの表示はPage_Loadで行う。
                Return False
            End If
        Else
            st_RFQNumber = RFQNumber.Text
        End If

        If IsNumeric(st_RFQNumber) Then
            DBCommand = New SqlCommand("Select " _
& "EnqLocationName, EnqUserName, QuoLocationName, QuoUserID, QuoUserName, ProductNumber, " _
& "ProductName, SupplierCode, R3SupplierCode, SupplierName, SupplierCountryCode, MakerCode, " _
& "MakerName, MakerCountryCode, SupplierContactPerson, PaymentTermCode, RequiredPurity, " _
& "RequiredQMMethod, RequiredSpecification, SpecSheet, Specification, Purpose, SupplierItemName, " _
& "ShippingHandlingFee, ShippingHandlingCurrencyCode, Comment, QuotedDate, StatusCode, " _
& "UpdateDate, Status, StatusChangeDate " _
& " From v_RFQHeader Where RFQNumber = @i_RFQNumber", DBConn)
            DBCommand.Parameters.Add("i_RFQNumber", SqlDbType.Int).Value = Integer.Parse(st_RFQNumber)
            DBAdapter = New SqlDataAdapter
            DBAdapter.SelectCommand = DBCommand
            DBAdapter.Fill(DS, "RFQHeader")
            If DS.Tables("RFQHeader").Rows.Count = 0 Then
                'RFQNumber 不正
                Return False
            End If
            'Left
            RFQNumber.Text = st_RFQNumber
            CurrentRFQStatus.Text = DS.Tables("RFQHeader").Rows(0)("Status").ToString
            ProductNumber.Text = DS.Tables("RFQHeader").Rows(0)("ProductNumber").ToString
            ProductName.Text = DS.Tables("RFQHeader").Rows(0)("ProductName").ToString
            SupplierCode.Text = DS.Tables("RFQHeader").Rows(0)("SupplierCode").ToString
            R3SupplierCode.Text = DS.Tables("RFQHeader").Rows(0)("R3SupplierCode").ToString
            SupplierName.Text = DS.Tables("RFQHeader").Rows(0)("SupplierName").ToString
            SupplierCountry.Text = GetContryName(DS.Tables("RFQHeader").Rows(0)("SupplierCountryCode").ToString)
            SupplierContactPerson.Text = DS.Tables("RFQHeader").Rows(0)("SupplierContactPerson").ToString
            MakerCode.Text = DS.Tables("RFQHeader").Rows(0)("MakerCode").ToString
            MakerName.Text = DS.Tables("RFQHeader").Rows(0)("MakerName").ToString
            MakerCountry.Text = GetContryName(DS.Tables("RFQHeader").Rows(0)("MakerCountryCode").ToString)
            SupplierItemName.Text = DS.Tables("RFQHeader").Rows(0)("SupplierItemName").ToString
            PaymentTerm.SelectedValue = DS.Tables("RFQHeader").Rows(0)("PaymentTermCode").ToString
            ShippingHandlingCurrency.SelectedValue = DS.Tables("RFQHeader").Rows(0)("ShippingHandlingCurrencyCode").ToString
            ShippingHandlingFee.Text = SetNullORDecimal(DS.Tables("RFQHeader").Rows(0)("ShippingHandlingFee").ToString)
            'Right
            Purpose.Text = DS.Tables("RFQHeader").Rows(0)("Purpose").ToString
            RequiredPurity.Text = DS.Tables("RFQHeader").Rows(0)("RequiredPurity").ToString
            RequiredQMMethod.Text = DS.Tables("RFQHeader").Rows(0)("RequiredQMMethod").ToString
            RequiredSpecification.Text = DS.Tables("RFQHeader").Rows(0)("RequiredSpecification").ToString
            If DS.Tables("RFQHeader").Rows(0)("SpecSheet").ToString = True Then
                SpecSheet.Checked = True
                SpecSheet.Text = "yes"
            Else
                SpecSheet.Checked = False
                SpecSheet.Text = "no"
            End If
            Specification.Text = DS.Tables("RFQHeader").Rows(0)("Specification").ToString
            EnqUser.Text = DS.Tables("RFQHeader").Rows(0)("EnqUserName").ToString
            EnqLocation.Text = DS.Tables("RFQHeader").Rows(0)("EnqLocationName").ToString

            If DS.Tables("RFQHeader").Rows(0)("QuoLocationName").ToString = "" Then
                QuoLocation.Text = EnqLocation.Text
            Else
                QuoLocation.Text = DS.Tables("RFQHeader").Rows(0)("QuoLocationName").ToString
            End If
            SDS_RFQUpdate_QuoUser.DataBind()
            'QuoUser.DataBind()     '要不要を検討する。
            If IsDBNull(DS.Tables("RFQHeader").Rows(0)("QuoUserID")) = False Then
                QuoUser.SelectedValue = DS.Tables("RFQHeader").Rows(0)("QuoUserID").ToString
            End If
            Comment.Text = DS.Tables("RFQHeader").Rows(0)("Comment").ToString
            'Under
            RFQStatus.SelectedValue = DS.Tables("RFQHeader").Rows(0)("StatusCode").ToString
            'Hidden
            QuotedDate.Value = DS.Tables("RFQHeader").Rows(0)("QuotedDate").ToString
            UpdateDate.Value = DS.Tables("RFQHeader").Rows(0)("UpdateDate").ToString
            'Line
            DBCommand = New SqlCommand("Select " _
& "RFQLineNumber, EnqQuantity, EnqUnitCode, EnqPiece, CurrencyCode, " _
& "UnitPrice, QuoPer, QuoUnitCode, LeadTime, SupplierItemNumber, " _
& "IncotermsCode, DeliveryTerm, Packing, Purity, QMMethod, NoOfferReasonCode" _
& " From v_RFQLine Where RFQNumber = @i_RFQNumber Order by RFQLineNumber", DBConn)
            DBCommand.Parameters.Add("i_RFQNumber", SqlDbType.Int).Value = Integer.Parse(st_RFQNumber)
            DBAdapter.SelectCommand = DBCommand

            DBAdapter.Fill(DS, "RFQLine")
            DBCommand.Dispose()

            If DS.Tables("RFQLine").Rows.Count = 0 Then
                'RFQNumber 不正
                Return False
            End If

            Dim i As Integer
            For i = 0 To DS.Tables("RFQLine").Rows.Count - 1
                Select Case i
                    Case 0
                        EnqQuantity_1.Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("EnqQuantity").ToString)
                        EnqUnit_1.Text = DS.Tables("RFQLine").Rows(i).Item("EnqUnitCode").ToString
                        EnqPiece_1.Text = DS.Tables("RFQLine").Rows(i).Item("EnqPiece").ToString
                        Incoterms_1.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("IncotermsCode").ToString
                        Currency_1.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("CurrencyCode").ToString
                        UnitPrice_1.Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("UnitPrice").ToString)
                        DeliveryTerm_1.Text = DS.Tables("RFQLine").Rows(i).Item("DeliveryTerm").ToString
                        QuoPer_1.Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("QuoPer").ToString)
                        Purity_1.Text = DS.Tables("RFQLine").Rows(i).Item("Purity").ToString
                        QuoUnit_1.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("QuoUnitCode").ToString
                        QMMethod_1.Text = DS.Tables("RFQLine").Rows(i).Item("QMMethod").ToString
                        LeadTime_1.Text = DS.Tables("RFQLine").Rows(i).Item("LeadTime").ToString
                        Packing_1.Text = DS.Tables("RFQLine").Rows(i).Item("Packing").ToString
                        SupplierItemNumber_1.Text = DS.Tables("RFQLine").Rows(i).Item("SupplierItemNumber").ToString
                        NoOfferReason_1.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("NoOfferReasonCode").ToString
                        POIssue_1.Visible = True
                        POIssue_1.NavigateUrl = "./POIssue.aspx?RFQLineNumber=" & DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                        LineNumber1.Value = DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                    Case 1
                        EnqQuantity_2.Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("EnqQuantity").ToString)
                        EnqUnit_2.Text = DS.Tables("RFQLine").Rows(i).Item("EnqUnitCode").ToString
                        EnqPiece_2.Text = DS.Tables("RFQLine").Rows(i).Item("EnqPiece").ToString
                        Incoterms_2.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("IncotermsCode").ToString
                        Currency_2.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("CurrencyCode").ToString
                        UnitPrice_2.Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("UnitPrice").ToString)
                        DeliveryTerm_2.Text = DS.Tables("RFQLine").Rows(i).Item("DeliveryTerm").ToString
                        QuoPer_2.Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("QuoPer").ToString)
                        Purity_2.Text = DS.Tables("RFQLine").Rows(i).Item("Purity").ToString
                        QuoUnit_2.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("QuoUnitCode").ToString
                        QMMethod_2.Text = DS.Tables("RFQLine").Rows(i).Item("QMMethod").ToString
                        LeadTime_2.Text = DS.Tables("RFQLine").Rows(i).Item("LeadTime").ToString
                        Packing_2.Text = DS.Tables("RFQLine").Rows(i).Item("Packing").ToString
                        SupplierItemNumber_2.Text = DS.Tables("RFQLine").Rows(i).Item("SupplierItemNumber").ToString
                        NoOfferReason_2.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("NoOfferReasonCode").ToString
                        POIssue_2.Visible = True
                        POIssue_2.NavigateUrl = "./POIssue.aspx?RFQLineNumber=" & DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                        LineNumber2.Value = DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                    Case 2
                        EnqQuantity_3.Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("EnqQuantity").ToString)
                        EnqUnit_3.Text = DS.Tables("RFQLine").Rows(i).Item("EnqUnitCode").ToString
                        EnqPiece_3.Text = DS.Tables("RFQLine").Rows(i).Item("EnqPiece").ToString
                        Incoterms_3.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("IncotermsCode").ToString
                        Currency_3.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("CurrencyCode").ToString
                        UnitPrice_3.Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("UnitPrice").ToString)
                        DeliveryTerm_3.Text = DS.Tables("RFQLine").Rows(i).Item("DeliveryTerm").ToString
                        QuoPer_3.Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("QuoPer").ToString)
                        Purity_3.Text = DS.Tables("RFQLine").Rows(i).Item("Purity").ToString
                        QuoUnit_3.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("QuoUnitCode").ToString
                        QMMethod_3.Text = DS.Tables("RFQLine").Rows(i).Item("QMMethod").ToString
                        LeadTime_3.Text = DS.Tables("RFQLine").Rows(i).Item("LeadTime").ToString
                        Packing_3.Text = DS.Tables("RFQLine").Rows(i).Item("Packing").ToString
                        SupplierItemNumber_3.Text = DS.Tables("RFQLine").Rows(i).Item("SupplierItemNumber").ToString
                        NoOfferReason_3.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("NoOfferReasonCode").ToString
                        POIssue_3.Visible = True
                        POIssue_3.NavigateUrl = "./POIssue.aspx?RFQLineNumber=" & DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                        LineNumber3.Value = DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                    Case 3
                        EnqQuantity_4.Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("EnqQuantity").ToString)
                        EnqUnit_4.Text = DS.Tables("RFQLine").Rows(i).Item("EnqUnitCode").ToString
                        EnqPiece_4.Text = DS.Tables("RFQLine").Rows(i).Item("EnqPiece").ToString
                        Incoterms_4.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("IncotermsCode").ToString
                        Currency_4.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("CurrencyCode").ToString
                        UnitPrice_4.Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("UnitPrice").ToString)
                        DeliveryTerm_4.Text = DS.Tables("RFQLine").Rows(i).Item("DeliveryTerm").ToString
                        QuoPer_4.Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("QuoPer").ToString)
                        Purity_4.Text = DS.Tables("RFQLine").Rows(i).Item("Purity").ToString
                        QuoUnit_4.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("QuoUnitCode").ToString
                        QMMethod_4.Text = DS.Tables("RFQLine").Rows(i).Item("QMMethod").ToString
                        LeadTime_4.Text = DS.Tables("RFQLine").Rows(i).Item("LeadTime").ToString
                        Packing_4.Text = DS.Tables("RFQLine").Rows(i).Item("Packing").ToString
                        SupplierItemNumber_4.Text = DS.Tables("RFQLine").Rows(i).Item("SupplierItemNumber").ToString
                        NoOfferReason_4.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("NoOfferReasonCode").ToString
                        POIssue_4.Visible = True
                        POIssue_4.NavigateUrl = "./POIssue.aspx?RFQLineNumber=" & DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                        LineNumber4.Value = DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                    Case Else
                        '処理無し
                End Select
            Next
            DS.Clear()
        End If
        Return True
    End Function

    Private Function ItemCheck() As Boolean

        ItemCheck = False
        '型チェック
        If ShippingHandlingFee.Text <> "" Then
            If Not Regex.IsMatch(ShippingHandlingFee.Text, DECIMAL_10_3_REGEX) Then
                Msg.Text = ERR_INCORRECT_SHIPPINGHANDLINGFEE
                Exit Function
            End If
        End If

        If UnitPrice_1.Text <> "" Then
            If Not Regex.IsMatch(UnitPrice_1.Text, DECIMAL_10_3_REGEX) Then
                Msg.Text = ERR_INCORRECT_UNITPRICE
                Exit Function
            End If
        End If
        If UnitPrice_2.Text <> "" Then
            If Not Regex.IsMatch(UnitPrice_2.Text, DECIMAL_10_3_REGEX) Then
                Msg.Text = ERR_INCORRECT_UNITPRICE
                Exit Function
            End If
        End If
        If UnitPrice_3.Text <> "" Then
            If Not Regex.IsMatch(UnitPrice_3.Text, DECIMAL_10_3_REGEX) Then
                Msg.Text = ERR_INCORRECT_UNITPRICE
                Exit Function
            End If
        End If
        If UnitPrice_4.Text <> "" Then
            If Not Regex.IsMatch(UnitPrice_4.Text, DECIMAL_10_3_REGEX) Then
                Msg.Text = ERR_INCORRECT_UNITPRICE
                Exit Function
            End If
        End If

        If QuoPer_1.Text <> "" Then
            If Not Regex.IsMatch(QuoPer_1.Text, DECIMAL_5_3_REGEX) Then
                Msg.Text = ERR_INCORRECT_QUOPER
                Exit Function
            End If
        End If
        If QuoPer_2.Text <> "" Then
            If Not Regex.IsMatch(QuoPer_2.Text, DECIMAL_5_3_REGEX) Then
                Msg.Text = ERR_INCORRECT_QUOPER
                Exit Function
            End If
        End If
        If QuoPer_3.Text <> "" Then
            If Not Regex.IsMatch(QuoPer_3.Text, DECIMAL_5_3_REGEX) Then
                Msg.Text = ERR_INCORRECT_QUOPER
                Exit Function
            End If
        End If
        If QuoPer_4.Text <> "" Then
            If Not Regex.IsMatch(QuoPer_4.Text, DECIMAL_5_3_REGEX) Then
                Msg.Text = ERR_INCORRECT_QUOPER
                Exit Function
            End If
        End If
        ItemCheck = True

    End Function

    Private Sub SetReadOnlyItems()
        'ReadOnly項目の再設定
        R3SupplierCode.Text = Request.Form("R3SupplierCode").ToString
        SupplierName.Text = Request.Form("SupplierName").ToString
        SupplierCountry.Text = Request.Form("SupplierCountry").ToString
        MakerName.Text = Request.Form("MakerName").ToString
        MakerCountry.Text = Request.Form("MakerCountry").ToString
    End Sub

    Private Function GetContryName(ByVal Code As String) As String
        Dim DBReader As SqlDataReader
        GetContryName = ""
        DBCommand.CommandText = "SELECT CountryName FROM v_Country WHERE (CountryCode = @CountryCode)"
        DBCommand.Parameters.Add("@CountryCode", SqlDbType.NVarChar).Value = Code
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Parameters.Clear()
        DBCommand.Dispose()
        If DBReader.HasRows = True Then
            While DBReader.Read
                GetContryName = DBReader("CountryName").ToString
            End While
        End If
        DBReader.Close()
    End Function

    Private Function CheckSupplierCode() As Boolean
        'Supplier,Makerの入力内容のチェック
        Dim st_Supplier As String = "Supplier"
        Dim st_SupplierKey As String = "SupplierCode"

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

    Private Sub SetPostBackUrl()
        'ボタンクリック時にPostBackするActionを追記する。
        If IsPostBack = False Then
            Update.PostBackUrl = "~/RFQUpdate.aspx?Action=Update"
            Close.PostBackUrl = "~/RFQUpdate.aspx?Action=Close"
        End If
    End Sub

    Private Function SetNullORDecimal(ByVal str As String) As String
        Dim de_Str As Decimal
        If IsDBNull(str) = True Or str = String.Empty Then
            Return String.Empty
        Else
            If Decimal.TryParse(str, de_Str) = True Then
                Return de_Str.ToString("G29")
            Else
                Return str
            End If
        End If
    End Function

    Private Function CheckUpdatedate() As Boolean
        Using DBConn As New SqlClient.SqlConnection(DB_CONNECT_STRING), _
            DBCommand As SqlCommand = DBConn.CreateCommand()
            Dim DBReader As SqlDataReader
            DBConn.Open()
            DBCommand.CommandText = "SELECT UpdateDate FROM RFQHeader WHERE (RFQNumber = @RFQNumber)"
            DBCommand.Parameters.Add("@RFQNumber", SqlDbType.Int).Value = Integer.Parse(RFQNumber.Text)
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Parameters.Clear()
            If DBReader.HasRows = False Then
                '行削除済のため処理を抜ける。
                Msg.Text = ERR_GET_RFQDATA_FAILURE
                Return False
            Else
                DBReader.Read()
                If DBReader("UpdateDate").ToString = UpdateDate.Value Then
                    Return True
                Else
                    Msg.Text = ERR_ALREADY_UPDATED
                    Return False
                End If
            End If
        End Using
    End Function

    Private Function CheckLocation() As Boolean
        If Session("Purchase.isAdmin") = False Then
            If Session("LocationName") <> EnqLocation.Text And Session("LocationName") <> QuoLocation.Text Then
                Msg.Text = ERR_ANOTHER_LOCATION
                Return False
            End If
        End If
        Return True
    End Function
End Class
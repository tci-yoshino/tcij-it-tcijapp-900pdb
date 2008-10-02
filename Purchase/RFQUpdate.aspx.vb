Imports System.Data.SqlClient
Imports Purchase.Common

Partial Public Class RFQUpdate
    Inherits CommonPage
    Private DBConn As New System.Data.SqlClient.SqlConnection(DB_CONNECT_STRING)
    Private DBCommand As System.Data.SqlClient.SqlCommand
    Private DBAdapter As System.Data.SqlClient.SqlDataAdapter
    'エラーメッセージ(入力値不正)
    Private Const ERR_INCORRECT_SUPPLIERCODE As String = "Supplier Code" & ERR_DOES_NOT_EXIST
    Private Const ERR_INCORRECT_MAKERCODE As String = "Maker Code" & ERR_DOES_NOT_EXIST
    Private Const ERR_INCORRECT_SHIPPINGHANDLINGFEE As String = "ShippingHandlingFee" & ERR_INVALID_NUMBER
    Private Const ERR_INCORRECT_UNITPRICE As String = "UnitPrice" & ERR_INVALID_NUMBER
    Private Const ERR_INCORRECT_QUOPER As String = "Quo-Per" & ERR_INVALID_NUMBER
    Private Const ERR_INCORRECT_CURRENCY As String = "Please enter a quotation (price)."
    'エラーメッセージ(必須入力項目)
    Private Const ERR_INCORRECT_ENQQUANTITY As String = "Enq-Quantity" & ERR_REQUIRED_FIELD
    Private Const ERR_REQUIRED_SUPPLIERCODE As String = "SupplierCode" & ERR_REQUIRED_FIELD
    Private Const ERR_REQUIRED_QUOUSER As String = "Quo-User" & ERR_REQUIRED_FIELD
    'エラーメッセージ(更新処理失敗)(Exception扱いなので日本語のままとする。)
    Private Const ERR_GET_RFQDATA_FAILURE As String = "RFQ データの更新に失敗しましたが、エラーが検出されませんでした。"
    'エラーメッセージ(他拠点情報更新)
    Private Const ERR_ANOTHER_LOCATION As String = "You can not edit the enquiry of other locations"
    'エラーメッセージ(文字数制限オーバー)
    Private Const ERR_COMMENT_OVER As String = "Comment" & ERR_OVER_3000
    Private Const ERR_SPECIFICATION_OVER As String = "Specification" & ERR_OVER_255

    '画面表示フラグ
    Protected Parameter As Boolean = True
    'RFQNumber
    Protected st_RFQNumber As String = String.Empty

    'RFQLineのコントロール配列化用定数
    Const LINE_START As Integer = 1
    Const LINE_COUNT As Integer = 4

    ' コントロール配列の定義
    Private EnqQuantity(LINE_COUNT) As TextBox
    Private EnqUnit(LINE_COUNT) As DropDownList
    Private EnqPiece(LINE_COUNT) As TextBox
    Private Currency(LINE_COUNT) As DropDownList
    Private UnitPrice(LINE_COUNT) As TextBox
    Private QuoPer(LINE_COUNT) As TextBox
    Private QuoUnit(LINE_COUNT) As DropDownList
    Private LeadTime(LINE_COUNT) As TextBox
    Private SupplierItemNumber(LINE_COUNT) As TextBox
    Private POIssue(LINE_COUNT) As HyperLink
    Private LineNumber(LINE_COUNT) As HiddenField
    Private Incoterms(LINE_COUNT) As DropDownList
    Private DeliveryTerm(LINE_COUNT) As TextBox
    Private Purity(LINE_COUNT) As TextBox
    Private QMMethod(LINE_COUNT) As TextBox
    Private Packing(LINE_COUNT) As TextBox
    Private NoOfferReason(LINE_COUNT) As DropDownList

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        Call SetControlArray()
        If IsPostBack = False Then
            If SetRFQNumber() = False Then
                'RFQNumberのチェックとst_RFQNumberへのセットを行う。
                Msg.Text = ERR_INVALID_PARAMETER
                '画面上の入力項目を隠す。
                Parameter = False
                Exit Sub
            End If
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
        Dim RFQStatusCode As String = String.Empty
        Dim st_QuotedDate As String = String.Empty
        Dim SQLLineUpdate As String = String.Empty
        Dim SQLLineInsert As String = String.Empty
        RunMsg.Text = String.Empty
        Msg.Text = String.Empty
        If SetRFQNumber() = False Then
            'RFQNumberのチェックとst_RFQNumberへのセットを行う。
            Msg.Text = ERR_INVALID_PARAMETER
            '画面上の入力項目を隠す。
            Parameter = False
            Exit Sub
        End If
        If Request.QueryString("Action") <> "Update" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If
        '入力項目の文字数チェック
        If Comment.Text.Length > INT_3000 Then
            Msg.Text = ERR_COMMENT_OVER
            Exit Sub
        End If
        If Specification.Text.Length > INT_255 Then
            Msg.Text = ERR_SPECIFICATION_OVER
            Exit Sub
        End If
        If CheckSupplierCode() = False Then
            'Supplier及びMakerの存在チェック
            Exit Sub
        End If
        If LineCheck() = False Then
            'RFQLineの必須入力チェック
            Exit Sub
        End If
        If ItemCheck() = False Then
            '入力された項目の型をチェックする(DB登録時にエラーになるもののみ)
            Exit Sub
        End If
        '更新可能拠点の確認
        If CheckLocation() = False Then
            Exit Sub
        End If
        '他セッションでの更新チェック
        If isLatestData("RFQHeader", "RFQNumber", st_RFQNumber, UpdateDate.Value) = False Then
            Msg.Text = ERR_UPDATED_BY_ANOTHER_USER
            Exit Sub
        End If
        '更新処理
        Dim sqlTran As System.Data.SqlClient.SqlTransaction = DBConn.BeginTransaction()
        DBCommand.Transaction = sqlTran
        Try
            'RFQHeader の更新
            DBCommand.Parameters.Clear()
            If RFQStatus.SelectedValue <> String.Empty Then
                RFQStatusCode = ", RFQStatusCode = @RFQStatusCode "
                DBCommand.Parameters.Add("@RFQStatusCode", SqlDbType.NVarChar).Value = RFQStatus.SelectedValue
            End If
            If QuotedDate.Value = String.Empty Then
                'QuotedDateは初回のみ登録し上書きしない。登録条件はRFQStatusが「Q」or「PQ」
                If RFQStatus.SelectedValue = "Q" Or RFQStatus.SelectedValue = "PQ" Then
                    Dim da_QuoDate As Date
                    Dim st_QuoDate As String = String.Empty
                    Date.TryParse(GetLocalTime(Session("LocationCode").ToString, Now, True), da_QuoDate)
                    st_QuoDate = da_QuoDate.Year & "/" & da_QuoDate.Month & "/" & da_QuoDate.Day
                    st_QuoDate = GetDatabaseTime(Session("LocationCode").ToString, st_QuoDate)
                    st_QuotedDate = ", QuotedDate = '" & st_QuoDate & "'"
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
            DBCommand.Parameters.Add("@RFQNumber", SqlDbType.Int).Value = Integer.Parse(st_RFQNumber)
            DBCommand.ExecuteNonQuery()
            DBCommand.Parameters.Clear()
            DBCommand.Dispose()

            'RFQLine の更新もしくはデータ追加
            'Update文作成
            SQLLineUpdate = "UPDATE RFQLine SET CurrencyCode = @CurrencyCode, UnitPrice = @UnitPrice, " _
& "QuoPer = @QuoPer, QuoUnitCode = @QuoUnitCode, LeadTime = @LeadTime, SupplierItemNumber = @SupplierItemNumber, " _
& "IncotermsCode = @IncotermsCode, DeliveryTerm = @DeliveryTerm, Packing = @Packing, Purity = @Purity, " _
& "QMMethod = @QMMethod, NoOfferReasonCode = @NoOfferReasonCode, UpdatedBy = @UpdatedBy, UpdateDate = GETDATE() " _
& "Where RFQLineNumber = @RFQLineNumber"

            'Insert文作成
            SQLLineInsert = "INSERT INTO RFQLine (RFQNumber, EnqQuantity, EnqUnitCode, EnqPiece, CurrencyCode," _
& " UnitPrice, QuoPer, QuoUnitCode, LeadTime, SupplierItemNumber, IncotermsCode," _
& " DeliveryTerm, Packing, Purity, QMMethod, NoOfferReasonCode, CreatedBy, UpdatedBy)" _
& " VALUES(@RFQNumber, @EnqQuantity, @EnqUnitCode, @EnqPiece, @CurrencyCode," _
& " @UnitPrice, @QuoPer, @QuoUnitCode, @LeadTime, @SupplierItemNumber, @IncotermsCode," _
& " @DeliveryTerm, @Packing, @Purity, @QMMethod, @NoOfferReasonCode, @CreatedBy,@UpdatedBy);"
            For i As Integer = LINE_START To LINE_COUNT
                If EnqQuantity(i).Text.Trim <> String.Empty Then
                    DBCommand.Parameters.Add("@RFQLineNumber", SqlDbType.Int).Value = ConvertStringToInt(LineNumber(i).Value)
                    DBCommand.Parameters.Add("@CurrencyCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(Currency(i).SelectedValue)
                    DBCommand.Parameters.Add("@UnitPrice", SqlDbType.Decimal).Value = ConvertStringToDec(UnitPrice(i).Text)
                    DBCommand.Parameters.Add("@QuoPer", SqlDbType.Decimal).Value = ConvertStringToDec(QuoPer(i).Text)
                    DBCommand.Parameters.Add("@QuoUnitCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(QuoUnit(i).SelectedValue)
                    DBCommand.Parameters.Add("@LeadTime", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(LeadTime(i).Text)
                    DBCommand.Parameters.Add("@SupplierItemNumber", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(SupplierItemNumber(i).Text)
                    DBCommand.Parameters.Add("@IncotermsCode", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(Incoterms(i).SelectedValue)
                    DBCommand.Parameters.Add("@DeliveryTerm", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(DeliveryTerm(i).Text)
                    DBCommand.Parameters.Add("@Packing", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(Packing(i).Text)
                    DBCommand.Parameters.Add("@Purity", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(Purity(i).Text)
                    DBCommand.Parameters.Add("@QMMethod", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(QMMethod(i).Text)
                    DBCommand.Parameters.Add("@NoOfferReasonCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(NoOfferReason(i).SelectedValue)
                    DBCommand.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = ConvertStringToInt(Session("UserID"))
                    If LineNumber(i).Value.Trim <> String.Empty Then
                        '更新処理
                        DBCommand.CommandText = SQLLineUpdate
                        DBCommand.ExecuteNonQuery()
                        DBCommand.Parameters.Clear()
                    Else
                        '登録処理
                        DBCommand.Parameters.Add("@RFQNumber", SqlDbType.Int).Value = ConvertStringToInt(st_RFQNumber)
                        DBCommand.Parameters.Add("@EnqQuantity", SqlDbType.Decimal).Value = ConvertStringToDec(EnqQuantity(i).Text)
                        DBCommand.Parameters.Add("@EnqUnitCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(EnqUnit(i).SelectedValue)
                        DBCommand.Parameters.Add("@EnqPiece", SqlDbType.Int).Value = ConvertStringToInt(EnqPiece(i).Text)
                        DBCommand.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = ConvertStringToInt(Session("UserID"))
                        DBCommand.CommandText = SQLLineInsert
                        DBCommand.ExecuteNonQuery()
                        DBCommand.Parameters.Clear()
                    End If
                End If
            Next
            sqlTran.Commit()
        Catch ex As Exception
            sqlTran.Rollback()
            Throw
        Finally
            DBCommand.Dispose()
        End Try
        If FormDataSet() = False Then
            '画面リフレッシュ
            Throw New Exception(ERR_GET_RFQDATA_FAILURE & "(UPDATE)")
            '画面上の入力項目を隠す。
            Parameter = False
            Exit Sub
        End If
        RunMsg.Text = MSG_DATA_UPDATED
    End Sub

    Protected Sub Close_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Close.Click
        RunMsg.Text = String.Empty
        Msg.Text = String.Empty
        If Request.QueryString("Action") <> "Close" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If
        If SetRFQNumber() = False Then
            'RFQNumberのチェックとst_RFQNumberへのセットを行う。
            Msg.Text = ERR_INVALID_PARAMETER
            '画面上の入力項目を隠す。
            Parameter = False
            Exit Sub
        End If
        '更新可能拠点の確認
        If CheckLocation() = False Then
            Exit Sub
        End If
        '他セッションでの更新チェック
        If isLatestData("RFQHeader", "RFQNumber", st_RFQNumber, UpdateDate.Value) = False Then
            Msg.Text = ERR_UPDATED_BY_ANOTHER_USER
            Exit Sub
        End If
        DBCommand.CommandText = "UPDATE RFQHeader SET RFQStatusCode = 'C', UpdatedBy = @UpdatedBy, UpdateDate = GETDATE() WHERE (RFQNumber = @RFQNumber)"
        DBCommand.Parameters.Add("@RFQNumber", SqlDbType.Int).Value = Integer.Parse(st_RFQNumber)
        DBCommand.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = Integer.Parse(Session("UserID"))
        DBCommand.ExecuteNonQuery()
        DBCommand.Parameters.Clear()
        DBCommand.Dispose()
        If FormDataSet() = False Then
            '画面リフレッシュ
            Throw New Exception(ERR_GET_RFQDATA_FAILURE & "(CLOSE)")
            '画面上の入力項目を隠す。
            Parameter = False
            Exit Sub
        End If
        RunMsg.Text = MSG_DATA_UPDATED
    End Sub

    Private Function FormDataSet() As Boolean
        Dim i_TryParse As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim DS As DataSet = New DataSet
        Call ClearLineData()
        If Integer.TryParse(st_RFQNumber, i_TryParse) Then
            DBCommand = New SqlCommand("Select " _
& "EnqLocationName, EnqUserName, QuoLocationName, QuoUserID, QuoUserName, ProductNumber, " _
& "ProductName, SupplierCode, R3SupplierCode, SupplierName, SupplierCountryCode, MakerCode, " _
& "MakerName, MakerCountryCode, SupplierContactPerson, PaymentTermCode, RequiredPurity, " _
& "RequiredQMMethod, RequiredSpecification, SpecSheet, Specification, Purpose, SupplierItemName, " _
& "ShippingHandlingFee, ShippingHandlingCurrencyCode, Comment, QuotedDate, StatusCode, " _
& "UpdateDate, Status, StatusChangeDate, EnqLocationCode, QuoLocationCode" _
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
            ProductName.Text = CutShort(DS.Tables("RFQHeader").Rows(0)("ProductName").ToString)
            SupplierCode.Text = DS.Tables("RFQHeader").Rows(0)("SupplierCode").ToString
            R3SupplierCode.Text = DS.Tables("RFQHeader").Rows(0)("R3SupplierCode").ToString
            SupplierName.Text = DS.Tables("RFQHeader").Rows(0)("SupplierName").ToString
            SupplierCountry.Text = GetCountryName(DS.Tables("RFQHeader").Rows(0)("SupplierCountryCode").ToString)
            SupplierContactPerson.Text = DS.Tables("RFQHeader").Rows(0)("SupplierContactPerson").ToString
            MakerCode.Text = DS.Tables("RFQHeader").Rows(0)("MakerCode").ToString
            MakerName.Text = DS.Tables("RFQHeader").Rows(0)("MakerName").ToString
            MakerCountry.Text = GetCountryName(DS.Tables("RFQHeader").Rows(0)("MakerCountryCode").ToString)
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
            Else
                SpecSheet.Checked = False
            End If
            Specification.Text = DS.Tables("RFQHeader").Rows(0)("Specification").ToString
            EnqUser.Text = DS.Tables("RFQHeader").Rows(0)("EnqUserName").ToString
            EnqLocation.Text = DS.Tables("RFQHeader").Rows(0)("EnqLocationName").ToString

            If DS.Tables("RFQHeader").Rows(0)("QuoLocationName").ToString = String.Empty Then
                QuoLocation.Text = EnqLocation.Text
            Else
                QuoLocation.Text = DS.Tables("RFQHeader").Rows(0)("QuoLocationName").ToString
            End If
            SDS_RFQUpdate_QuoUser.DataBind()
            If IsDBNull(DS.Tables("RFQHeader").Rows(0)("QuoUserID")) = False Then
                QuoUser.SelectedValue = DS.Tables("RFQHeader").Rows(0)("QuoUserID").ToString
            End If
            Comment.Text = DS.Tables("RFQHeader").Rows(0)("Comment").ToString
            'Hidden
            QuotedDate.Value = DS.Tables("RFQHeader").Rows(0)("QuotedDate").ToString
            UpdateDate.Value = GetUpdateDate("v_RFQHeader", "RFQNumber", st_RFQNumber)
            EnqLocationCode.Value = DS.Tables("RFQHeader").Rows(0)("EnqLocationCode").ToString
            QuoLocationCode.Value = DS.Tables("RFQHeader").Rows(0)("QuoLocationCode").ToString
            'Under
            RFQStatus.SelectedValue = ""
            If Session("LocationCode") <> EnqLocationCode.Value Then
                Close.Visible = False
            Else
                Close.Visible = True
            End If
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
            Else
                Dim i_Cnt As Integer = 0
                i_Cnt = IIf(LINE_COUNT > DS.Tables("RFQLine").Rows.Count, DS.Tables("RFQLine").Rows.Count, LINE_COUNT)
                For i = 0 To i_Cnt - 1
                    j = i + 1
                    EnqQuantity(j).Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("EnqQuantity").ToString)
                    EnqQuantity(j).ReadOnly = True
                    EnqQuantity(j).CssClass = "readonly number"
                    EnqUnit(j).SelectedValue = DS.Tables("RFQLine").Rows(i).Item("EnqUnitCode").ToString
                    EnqUnit(j).CssClass = "readonly"
                    EnqPiece(j).Text = DS.Tables("RFQLine").Rows(i).Item("EnqPiece").ToString
                    EnqPiece(j).ReadOnly = True
                    EnqPiece(j).CssClass = "readonly number"
                    Incoterms(j).SelectedValue = DS.Tables("RFQLine").Rows(i).Item("IncotermsCode").ToString
                    Currency(j).SelectedValue = DS.Tables("RFQLine").Rows(i).Item("CurrencyCode").ToString
                    UnitPrice(j).Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("UnitPrice").ToString)
                    DeliveryTerm(j).Text = DS.Tables("RFQLine").Rows(i).Item("DeliveryTerm").ToString
                    QuoPer(j).Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("QuoPer").ToString)
                    Purity(j).Text = DS.Tables("RFQLine").Rows(i).Item("Purity").ToString
                    QuoUnit(j).SelectedValue = DS.Tables("RFQLine").Rows(i).Item("QuoUnitCode").ToString
                    QMMethod(j).Text = DS.Tables("RFQLine").Rows(i).Item("QMMethod").ToString
                    LeadTime(j).Text = DS.Tables("RFQLine").Rows(i).Item("LeadTime").ToString
                    Packing(j).Text = DS.Tables("RFQLine").Rows(i).Item("Packing").ToString
                    SupplierItemNumber(j).Text = DS.Tables("RFQLine").Rows(i).Item("SupplierItemNumber").ToString
                    NoOfferReason(j).SelectedValue = DS.Tables("RFQLine").Rows(i).Item("NoOfferReasonCode").ToString
                    POIssue(j).Visible = True
                    POIssue(j).NavigateUrl = "./POIssue.aspx?RFQLineNumber=" & DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                    LineNumber(j).Value = DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                Next
            End If
            DS.Clear()
        End If
        Return True
    End Function

    Private Function ItemCheck() As Boolean
        ItemCheck = False
        '型チェック
        If ShippingHandlingFee.Text <> String.Empty Then
            If Not Regex.IsMatch(ShippingHandlingFee.Text, DECIMAL_10_3_REGEX) Then
                Msg.Text = ERR_INCORRECT_SHIPPINGHANDLINGFEE
                Exit Function
            End If
        End If
        For i As Integer = LINE_START To LINE_COUNT
            If UnitPrice(i).Text <> String.Empty Then
                If Not Regex.IsMatch(UnitPrice(i).Text, DECIMAL_10_3_REGEX) Then
                    Msg.Text = ERR_INCORRECT_UNITPRICE
                    Exit Function
                End If
            End If
            If QuoPer(i).Text <> String.Empty Then
                If Not Regex.IsMatch(QuoPer(i).Text, DECIMAL_5_3_REGEX) Then
                    Msg.Text = ERR_INCORRECT_QUOPER
                    Exit Function
                End If
            End If
        Next
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

    Private Function GetCountryName(ByVal Code As String) As String
        Dim DBReader As SqlDataReader
        GetCountryName = String.Empty
        DBCommand.CommandText = "SELECT CountryName FROM v_Country WHERE (CountryCode = @CountryCode)"
        DBCommand.Parameters.Add("@CountryCode", SqlDbType.NVarChar).Value = Code
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Parameters.Clear()
        DBCommand.Dispose()
        If DBReader.HasRows = True Then
            While DBReader.Read
                GetCountryName = DBReader("CountryName").ToString
            End While
        End If
        DBReader.Close()
    End Function

    Private Function CheckSupplierCode() As Boolean
        'Supplier,Makerの入力内容のチェック
        Dim st_Supplier As String = "Supplier"
        Dim st_SupplierKey As String = "SupplierCode"

        'Supplierのチェック
        If Not IsInteger(SupplierCode.Text) Then
            Msg.Text = ERR_INCORRECT_SUPPLIERCODE
            Return False
        End If
        If ExistenceConfirmation(st_Supplier, st_SupplierKey, SupplierCode.Text) = False Then
            Msg.Text = ERR_INCORRECT_SUPPLIERCODE
            Return False
        End If
        'Makerのチェック
        If MakerCode.Text <> String.Empty Then
            If Not IsInteger(MakerCode.Text) Then
                Msg.Text = ERR_INCORRECT_MAKERCODE
                Return False
            End If
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

    Private Function CheckLocation() As Boolean
        If Session("Purchase.isAdmin") = False Then
            If Session("LocationCode") <> EnqLocationCode.Value And Session("LocationCode") <> QuoLocationCode.Value Then
                Msg.Text = ERR_ANOTHER_LOCATION
                Return False
            End If
        End If
        Return True
    End Function
    Private Function LineCheck() As Boolean
        If CheckLineEnqQuantity() = False Then
            'EnqQuantity,EnqUnit,EnqPieceの入力チェック
            Msg.Text = ERR_INCORRECT_ENQQUANTITY
            Return False
        End If
        If CheckLineSet() = False Then
            'Currency,Price,Quo-Per,Quo-Unitの入力チェック
            Msg.Text = ERR_INCORRECT_CURRENCY
            Return False
        End If
        Return True
    End Function
    Private Function CheckLineSet() As Boolean
        'RFQLineのCurrency,Price,QuoPer,QuoUnitはどこかが空白で更新することができない。
        For i As Integer = LINE_START To LINE_COUNT
            If Currency(i).SelectedValue.Trim = String.Empty And UnitPrice(i).Text.Trim = String.Empty And QuoPer(i).Text.Trim = String.Empty And QuoUnit(i).SelectedValue.Trim = String.Empty Then
            ElseIf Currency(i).SelectedValue.Trim = String.Empty Then
                Return False
            ElseIf UnitPrice(i).Text.Trim = String.Empty Then
                Return False
            ElseIf QuoPer(i).Text.Trim = String.Empty Then
                Return False
            ElseIf QuoUnit(i).SelectedValue.Trim = String.Empty Then
                Return False
            End If
        Next
        Return True
    End Function
    Private Function CheckLineEnqQuantity() As Boolean
        'RFQLineのEnqQuantity,EnqUnit,EnqPieceはどこかが空白で登録することができない。
        For i As Integer = LINE_START To LINE_COUNT
            If POIssue(i).Visible = True Then
                '登録済で変更不可の行はチェックしない。
                Continue For
            End If
            If EnqQuantity(i).Text.Trim = String.Empty And EnqUnit(i).SelectedValue.Trim = String.Empty And EnqPiece(i).Text.Trim = String.Empty Then
                Continue For
            ElseIf EnqQuantity(i).Text.Trim = String.Empty Then
                Return False
            ElseIf EnqUnit(i).SelectedValue.Trim = String.Empty Then
                Return False
            ElseIf EnqPiece(i).Text.Trim = String.Empty Then
                Return False
            End If
            '量入力の書式チェック
            If Regex.IsMatch(EnqQuantity(i).Text.Trim, DECIMAL_7_3_REGEX) = False Then
                Return False
            End If
            '数量入力の整数チェック
            If Regex.IsMatch(EnqPiece(i).Text.Trim, INT_5_REGEX) = False Then
                Return False
            End If
        Next
        Return True
    End Function

    Private Function SetRFQNumber() As Boolean
        Dim i_TryParse As Integer = 0
        If Request.QueryString("RFQNumber") <> String.Empty Or Request.Form("RFQNumber") <> String.Empty Then
            st_RFQNumber = IIf(Request.QueryString("RFQNumber") <> String.Empty, Request.QueryString("RFQNumber"), Request.Form("RFQNumber"))
        ElseIf RFQNumber.Text <> String.Empty Then
            st_RFQNumber = RFQNumber.Text
        Else
            Return False
        End If
        If Integer.TryParse(st_RFQNumber, i_TryParse) = False Then
            Return False
        End If
        Return True
    End Function

    Private Sub SetControlArray()
        'RFQLineのコントロール配列を作成する。
        For i As Integer = LINE_START To LINE_COUNT
            EnqQuantity(i) = CType(FindControl(String.Format("{0}_{1}", "EnqQuantity", i)), TextBox)
            EnqUnit(i) = CType(FindControl(String.Format("{0}_{1}", "EnqUnit", i)), DropDownList)
            EnqPiece(i) = CType(FindControl(String.Format("{0}_{1}", "EnqPiece", i)), TextBox)
            Currency(i) = CType(FindControl(String.Format("{0}_{1}", "Currency", i)), DropDownList)
            UnitPrice(i) = CType(FindControl(String.Format("{0}_{1}", "UnitPrice", i)), TextBox)
            QuoPer(i) = CType(FindControl(String.Format("{0}_{1}", "QuoPer", i)), TextBox)
            QuoUnit(i) = CType(FindControl(String.Format("{0}_{1}", "QuoUnit", i)), DropDownList)
            LeadTime(i) = CType(FindControl(String.Format("{0}_{1}", "LeadTime", i)), TextBox)
            SupplierItemNumber(i) = CType(FindControl(String.Format("{0}_{1}", "SupplierItemNumber", i)), TextBox)
            POIssue(i) = CType(FindControl(String.Format("{0}_{1}", "POIssue", i)), HyperLink)
            LineNumber(i) = CType(FindControl(String.Format("{0}{1}", "LineNumber", i)), HiddenField)
            Incoterms(i) = CType(FindControl(String.Format("{0}_{1}", "Incoterms", i)), DropDownList)
            DeliveryTerm(i) = CType(FindControl(String.Format("{0}_{1}", "DeliveryTerm", i)), TextBox)
            Purity(i) = CType(FindControl(String.Format("{0}_{1}", "Purity", i)), TextBox)
            QMMethod(i) = CType(FindControl(String.Format("{0}_{1}", "QMMethod", i)), TextBox)
            Packing(i) = CType(FindControl(String.Format("{0}_{1}", "Packing", i)), TextBox)
            NoOfferReason(i) = CType(FindControl(String.Format("{0}_{1}", "NoOfferReason", i)), DropDownList)
        Next
    End Sub
    Private Sub ClearLineData()
        For i As Integer = LINE_START To LINE_COUNT
            EnqQuantity(i).Text = String.Empty
            EnqUnit(i).Items.Clear()
            EnqUnit(i).Items.Add(String.Empty)
            EnqUnit(i).DataSourceID = "SDS_RFQUpdate_Qua"
            EnqUnit(i).DataTextField = "UnitCode"
            EnqUnit(i).DataValueField = "UnitCode"
            EnqUnit(i).DataBind()
            EnqPiece(i).Text = String.Empty
            Currency(i).Items.Clear()
            Currency(i).Items.Add(String.Empty)
            Currency(i).DataSourceID = "SDS_RFQUpdate_Currency"
            Currency(i).DataTextField = "CurrencyCode"
            Currency(i).DataValueField = "CurrencyCode"
            Currency(i).DataBind()
            UnitPrice(i).Text = String.Empty
            QuoPer(i).Text = String.Empty
            QuoUnit(i).Items.Clear()
            QuoUnit(i).Items.Add(String.Empty)
            QuoUnit(i).DataSourceID = "SDS_RFQUpdate_Unit"
            QuoUnit(i).DataTextField = "UnitCode"
            QuoUnit(i).DataValueField = "UnitCode"
            QuoUnit(i).DataBind()
            LeadTime(i).Text = String.Empty
            SupplierItemNumber(i).Text = String.Empty
            Incoterms(i).Items.Clear()
            Incoterms(i).Items.Add(String.Empty)
            Incoterms(i).DataSourceID = "SDS_RFQUpdate_Incoterms"
            Incoterms(i).DataTextField = "IncotermsCode"
            Incoterms(i).DataValueField = "IncotermsCode"
            Incoterms(i).DataBind()
            DeliveryTerm(i).Text = String.Empty
            Purity(i).Text = String.Empty
            QMMethod(i).Text = String.Empty
            Packing(i).Text = String.Empty
            NoOfferReason(i).Items.Clear()
            NoOfferReason(i).Items.Add(String.Empty)
            NoOfferReason(i).DataSourceID = "SDS_RFQUpdate_NoOffer"
            NoOfferReason(i).DataTextField = "Text"
            NoOfferReason(i).DataValueField = "NoOfferReasonCode"
            NoOfferReason(i).DataBind()
        Next
    End Sub
End Class
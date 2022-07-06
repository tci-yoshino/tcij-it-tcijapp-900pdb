Imports System.Collections.Generic
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
    Private Const ERR_REQUIRED_ENQUSER As String = "Enq-User" & ERR_REQUIRED_FIELD
    Private Const ERR_REQUIRED_QUOUSER As String = "Quo-User" & ERR_REQUIRED_FIELD
    'エラーメッセージ(更新処理失敗)(Exception扱いなので日本語のままとする。)
    Private Const ERR_GET_RFQDATA_FAILURE As String = "RFQ データの更新に失敗しましたが、エラーが検出されませんでした。"
    'エラーメッセージ(他拠点情報更新)
    Private Const ERR_ANOTHER_LOCATION As String = "You can not edit the enquiry of other locations"
    'エラーメッセージ(文字数制限オーバー)
    Private Const ERR_COMMENT_OVER As String = "Comment" & ERR_OVER_3000
    Private Const ERR_SPECIFICATION_OVER As String = "Specification" & ERR_OVER_255

    '更新前 EnqUserID, QuoUserID を格納する ViewState のキー名定数
    Private Const OLD_ENQUSER_ID As String = "OldEnqUserID"
    Private Const OLD_QUOUSER_ID As String = "OldQuoUserID"

    '画面表示フラグ
    Protected Parameter As Boolean = True
    'RFQNumber
    Protected st_RFQNumber As String = String.Empty
    Protected da_vRFQHeader As TCIDataAccess.v_RFQHeader = Nothing

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
    Private POInterface(LINE_COUNT) As HyperLink
    Private POInterfaceButton(LINE_COUNT) As Button
    Private SupplierOfferNo(LINE_COUNT) As TextBox
    Public ComentInfo As String
    Public isSHowOldEnqUserName As Boolean = False
    Public isSHowOldQuoUserName As Boolean = False
    Public OldEnqUserName As String = ""
    Public OldQuoUserName As String = ""


    Protected Class BOOLMMSTAInvalidationEditable
        Public Shared Y As String = "1"
        Public Shared N As String = "0"
    End Class
    Protected Class BOOLMMSTAInvalidationValue
        Public Shared Y As String = "True"
        Public Shared N As String = "False"
    End Class


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        Call SetControlArray()
        'IsPostBack为True是回发的页面
        If SetRFQNumber() = False Then
            'RFQNumberのチェックとst_RFQNumberへのセットを行う。
            Msg.Text = ERR_INVALID_PARAMETER
            '画面上の入力項目を隠す。
            Parameter = False
            Exit Sub
        End If
        If IsPostBack = False Then
            Call SetPostBackUrl()
            If FormDataSet() = False Then
                Msg.Text = ERR_INVALID_PARAMETER
                '画面上の入力項目を隠す。
                Parameter = False
                Exit Sub
            End If
            '获取编号   从User表中找到用户名   显示在左侧txt中
            LeadTime_1.Attributes.Add("onchange", "return  RegleadTime(1)")
            LeadTime_2.Attributes.Add("onchange", "return  RegleadTime(2)")
            LeadTime_3.Attributes.Add("onchange", "return  RegleadTime(3)")
            LeadTime_4.Attributes.Add("onchange", "return  RegleadTime(4)")
        Else
            Try
                da_vRFQHeader = New TCIDataAccess.v_RFQHeader
                da_vRFQHeader.Load(Me.st_RFQNumber)
            Catch ex As KeyNotFoundException
                'RFQNumber 不正
            End Try

            Call SetReadOnlyItems()
        End If
        Me.Msg.Text = String.Empty
        Call EnqUserPlantSpmatlStatus()
        Call QuoUserPlantSpmatlStatus()

        'ログインユーザ＝RFQUser の場合、Priority 編集可
        Dim st_ENQUser As String = String.Empty
        st_ENQUser = EnqUser.SelectedValue
        If String.IsNullOrEmpty(st_ENQUser) Then
            '画面初期表示時のみ SelectedValue で値が取得できないため、直接データ参照する
            st_ENQUser = ViewState(OLD_ENQUSER_ID)
        End If
        Dim st_QuoUser As String = String.Empty
        st_QuoUser = QuoUser.SelectedValue
        If String.IsNullOrEmpty(st_QuoUser) Then
            '画面初期表示時のみ SelectedValue で値が取得できないため、直接データ参照する
            st_QuoUser = ViewState(OLD_QUOUSER_ID)
        End If

        If (Session("UserID").ToString = st_ENQUser) Then
            Priority.Enabled = True
            Priority.Visible = True
            LabelPriority.Visible = False
        Else
            Priority.Enabled = False
            Priority.Visible = False
            LabelPriority.Visible = True
        End If

        POInterfaceMsg.Text = String.Empty
        POInterfaceConfirmMsg.Text = String.Empty
        If MMSTAInvalidation.Checked = False Then
            Dim encResult As String = String.Empty
            Dim quoResult As String = String.Empty
            encResult = EnqMMSTAValidationSearch()
            quoResult = QuoMMSTAValidationSearch()
            If encResult <> String.Empty AndAlso quoResult <> String.Empty Then
                Call POInterfaceMsgSet(encResult, quoResult)
            End If
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
        '変更前 Enq-User のコレポンチェック (False = 未処理コレポン有り)
        'If ViewState(OLD_ENQUSER_ID) <> EnqUser.SelectedValue Then
        '    If CheckUntreatedCorrespondence(RFQNumber.Text, ViewState(OLD_ENQUSER_ID)) = False Then
        '        Msg.Text = ERR_UNTREATED_CORRESPONDENCE
        '        Exit Sub
        '    End If
        'End If
        '変更前 Quo-User のコレポンチェック (False = 未処理コレポン有り)
        ' 以下の場合はチェックをスキップ
        ' ・変更前のQuo-Userが未設定(Null)の場合
        ' ・変更前後でQuo-Userが同じ場合（変更が無い場合）
        If (Not String.IsNullOrEmpty(ViewState(OLD_QUOUSER_ID))) AndAlso ViewState(OLD_QUOUSER_ID) <> QuoUser.SelectedValue Then
            If CheckUntreatedCorrespondence(RFQNumber.Text, ViewState(OLD_QUOUSER_ID)) = False Then
                Msg.Text = ERR_UNTREATED_CORRESPONDENCE
                Exit Sub
            End If
        End If
        '更新可能拠点の確認
        If CheckLocation() = False Then
            Exit Sub
        End If

        '他セッションでの更新チェック
        If IsLatestData("RFQHeader", "RFQNumber", st_RFQNumber, UpdateDate.Value) = False Then
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
                'RFQStatus を選択してある場合は RFQStatusCode を更新する。
                RFQStatusCode = ", RFQStatusCode = @RFQStatusCode "
                DBCommand.Parameters.Add("@RFQStatusCode", SqlDbType.NVarChar).Value = RFQStatus.SelectedValue
            End If
            If QuotedDate.Value = String.Empty Then
                'QuotedDateは初回のみ登録し上書きしない。登録条件はRFQStatusが「Q」or「PQ」
                If RFQStatus.SelectedValue = "Q" Or RFQStatus.SelectedValue = "PQ" Then
                    Dim st_QuoDate As String = String.Empty
                    st_QuoDate = GetLocalTime(Session("LocationCode").ToString, Now.Date, False, False)
                    st_QuoDate = GetDatabaseTime(Session("LocationCode").ToString, st_QuoDate)
                    st_QuotedDate = ", QuotedDate = '" & st_QuoDate & "'"
                End If
            End If

            'プルダウンが編集可能な場合はプルダウンから値を取得する
            Dim st_Priority As String = String.Empty
            If (Priority.Visible) Then
                st_Priority = Priority.Text
            Else
                st_Priority = LabelPriority.Text
            End If

            Dim st_PurposeCode As String = String.Empty
            If (ListPurpose.Visible) Then
                st_PurposeCode = ListPurpose.SelectedValue
            Else
                st_PurposeCode = PurposeCode.Value
            End If

            Dim st_EnqLocationCode As String = String.Empty
            If EnqLocation.CssClass = "readonly" Then
                st_EnqLocationCode = EnqLocationCode.Value
            Else
                st_EnqLocationCode = EnqLocation.SelectedValue
            End If

            Dim st_QuoLocationCode As String = String.Empty
            If EnqLocation.CssClass = "readonly" Then
                st_QuoLocationCode = QuoLocationCode.Value
            Else
                st_QuoLocationCode = QuoLocation.SelectedValue
            End If

            Dim st_EnqStorageLocationCode As String = String.Empty
            If EnqLocation.CssClass = "readonly" Then
                'st_EnqStorageLocationCode = EnqStorageLOcationCode.Value
                st_EnqStorageLocationCode = StorageLocation.SelectedValue
            Else
                st_EnqStorageLocationCode = StorageLocation.SelectedValue
            End If

            Dim st_QuoStorageLocationCode As String = String.Empty
            If EnqLocation.CssClass = "readonly" Then
                'st_QuoStorageLocationCode = QuoStorageLOcationCode.Value
                st_QuoStorageLocationCode = StorageLocation2.SelectedValue
            Else
                st_QuoStorageLocationCode = StorageLocation2.SelectedValue
            End If

            Dim sqlStr As String = "Update RFQHeader 
                SET EnqLocationCode = @EnqLocationCode,QuoLocationCode = @QuoLocationCode, EnqUserID = @EnqUserID, QuoUserID = @QuoUserID, CodeExtensionCode = @CodeExtensionCode, SupplierCode = @SupplierCode, MakerCode = @MakerCode,SAPMakerCode = @SAPMakerCode,
                    SpecSheet = @SpecSheet, Specification = @Specification, SupplierContactPerson = @SupplierContactPerson,
                    SupplierItemName = @SupplierItemName, ShippingHandlingFee = @ShippingHandlingFee,
                    ShippingHandlingCurrencyCode = @ShippingHandlingCurrencyCode, PaymentTermCode = @PaymentTermCode,
                    Comment = @Comment, Priority = @Priority , PurposeCode = @PurposeCode , UpdatedBy = @UpdatedBy,EnqStorageLocation=@EnqStorageLocation,QuoStorageLocation=@QuoStorageLocation,SupplierContactPersonSel=@SupplierContactPersonSel, UpdateDate = GETDATE()"
            sqlStr &= RFQStatusCode & st_QuotedDate & ",SupplierOfferValidTo = @SupplierOfferValidTo "
            If Session("Purchase.MMSTAInvalidationEditable") = BOOLMMSTAInvalidationEditable.Y Then
                sqlStr &= ", MMSTAInvalidation = @MMSTAInvalidation "
            End If
            sqlStr &= " Where RFQNumber = @RFQNumber "
            DBCommand.CommandText = sqlStr 
            DBCommand.Parameters.Add("@EnqLocationCode", SqlDbType.VarChar).Value = st_EnqLocationCode
            DBCommand.Parameters.Add("@QuoLocationCode", SqlDbType.VarChar).Value = st_QuoLocationCode
            DBCommand.Parameters.Add("@EnqUserID", SqlDbType.Int).Value = ConvertStringToInt(EnqUser.SelectedValue)
            DBCommand.Parameters.Add("@QuoUserID", SqlDbType.Int).Value = ConvertStringToInt(QuoUser.SelectedValue)
            DBCommand.Parameters.Add("@SupplierCode", SqlDbType.Int).Value = Integer.Parse(SupplierCode.Text)
            DBCommand.Parameters.Add("@CodeExtensionCode", SqlDbType.VarChar).Value = CodeExtensionList.SelectedValue
            DBCommand.Parameters.Add("@MakerCode", SqlDbType.Int).Value = ConvertStringToInt(MakerCode.Text)
            DBCommand.Parameters.Add("@SAPMakerCode", SqlDbType.Int).Value = ConvertStringToInt(SAPMakerCode.Text)
            DBCommand.Parameters.Add("@SpecSheet", SqlDbType.Bit).Value = SpecSheet.Checked
            DBCommand.Parameters.Add("@Specification", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(Specification.Text)
            DBCommand.Parameters.Add("@SupplierContactPerson", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(SupplierContactPerson.Text)
            DBCommand.Parameters.Add("@SupplierItemName", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(SupplierItemName.Text.Trim)
            DBCommand.Parameters.Add("@ShippingHandlingFee", SqlDbType.Decimal).Value = ConvertStringToDec(ShippingHandlingFee.Text)
            DBCommand.Parameters.Add("@ShippingHandlingCurrencyCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(ShippingHandlingCurrency.Text)
            DBCommand.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(PaymentTerm.SelectedValue)
            DBCommand.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(Comment.Text)
            DBCommand.Parameters.Add("@Priority", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(st_Priority)
            DBCommand.Parameters.Add("@PurposeCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(st_PurposeCode)
            DBCommand.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = Integer.Parse(Session("UserID"))
            DBCommand.Parameters.Add("@EnqStorageLocation", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(st_EnqStorageLocationCode)
            DBCommand.Parameters.Add("@QuoStorageLocation", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(st_QuoStorageLocationCode)
            DBCommand.Parameters.Add("@RFQNumber", SqlDbType.Int).Value = Integer.Parse(st_RFQNumber)
            DBCommand.Parameters.Add("@SupplierContactPersonSel", SqlDbType.NVarChar).Value = SupplierContactPersonCodeList.SelectedValue
            DBCommand.Parameters.Add("@SupplierOfferValidTo", SqlDbType.NVarChar).Value = txtVaildTo.Text

            If Session("Purchase.MMSTAInvalidationEditable") = BOOLMMSTAInvalidationEditable.Y Then
                If MMSTAInvalidation.Checked = True Then
                    DBCommand.Parameters.Add("@MMSTAInvalidation", SqlDbType.Bit).Value = 1
                Else
                    DBCommand.Parameters.Add("@MMSTAInvalidation", SqlDbType.Bit).Value = 0
                End If
            End If
            DBCommand.ExecuteNonQuery()
            DBCommand.Parameters.Clear()
            DBCommand.Dispose()

            'RFQLine の更新もしくはデータ追加
            'Update文作成
            SQLLineUpdate = "UPDATE RFQLine SET EnqQuantity =@EnqQuantity, EnqUnitCode =@EnqUnitCode, EnqPiece =@EnqPiece, CurrencyCode = @CurrencyCode, UnitPrice = @UnitPrice, " _
& "QuoPer = @QuoPer, QuoUnitCode = @QuoUnitCode, LeadTime = @LeadTime, SupplierItemNumber = @SupplierItemNumber, " _
& "IncotermsCode = @IncotermsCode, DeliveryTerm = @DeliveryTerm, Packing = @Packing, Purity = @Purity, " _
& "QMMethod = @QMMethod,SupplierOfferNo=@SupplierOfferNo,NoOfferReasonCode = @NoOfferReasonCode, UpdatedBy = @UpdatedBy, UpdateDate = GETDATE() " _
& "Where RFQLineNumber = @RFQLineNumber"

            'Insert文作成
            SQLLineInsert = "INSERT INTO RFQLine (RFQNumber, EnqQuantity, EnqUnitCode, EnqPiece, CurrencyCode," _
& " UnitPrice, QuoPer, QuoUnitCode, LeadTime, SupplierItemNumber, IncotermsCode," _
& " DeliveryTerm, Packing, Purity, QMMethod,SupplierOfferNo, NoOfferReasonCode, CreatedBy, UpdatedBy)" _
& " VALUES(@RFQNumber, @EnqQuantity, @EnqUnitCode, @EnqPiece, @CurrencyCode," _
& " @UnitPrice, @QuoPer, @QuoUnitCode, @LeadTime, @SupplierItemNumber, @IncotermsCode," _
& " @DeliveryTerm, @Packing, @Purity, @QMMethod,@SupplierOfferNo, @NoOfferReasonCode, @CreatedBy,@UpdatedBy);"
            For i As Integer = LINE_START To LINE_COUNT
                If EnqQuantity(i).Text.Trim <> String.Empty Then
                    DBCommand.Parameters.Add("@RFQLineNumber", SqlDbType.Int).Value = ConvertStringToInt(LineNumber(i).Value)
                    DBCommand.Parameters.Add("@EnqQuantity", SqlDbType.Decimal).Value = ConvertStringToDec(EnqQuantity(i).Text)
                    DBCommand.Parameters.Add("@EnqUnitCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(EnqUnit(i).SelectedValue)
                    DBCommand.Parameters.Add("@EnqPiece", SqlDbType.Int).Value = ConvertStringToInt(EnqPiece(i).Text)
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
                    DBCommand.Parameters.Add("@SupplierOfferNo", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(SupplierOfferNo(i).Text)
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
        If IsLatestData("RFQHeader", "RFQNumber", st_RFQNumber, UpdateDate.Value) = False Then
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

    ''' <summary>
    ''' Supplier Infomation リンク（Supplier）クリック時処理。
    ''' </summary>
    Protected Sub SupplierInfo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles SupplierInfo.Click
        Dim st_SupplierCode As String = SupplierCode.Text
        OpenSupplierInfo(st_SupplierCode)
    End Sub

    ''' <summary>
    ''' Supplier Infomation リンク（Maker）クリック時処理。
    ''' </summary>
    Protected Sub MakerInfo_Click(ByVal sender As Object, ByVal e As EventArgs) Handles MakerInfo.Click
        Dim st_SupplierCode As String = MakerCode.Text
        OpenSupplierInfo(st_SupplierCode)
    End Sub


    ''' <summary>
    ''' Supplier Infomation リンクオープン処理。
    ''' </summary>
    ''' <param name="st_SupplierCode">画面から取得した対象サプライヤのコード</param>
    ''' <remarks></remarks>
    Protected Sub OpenSupplierInfo(ByVal st_SupplierCode As String)
        Msg.Text = ""

        Dim st_SupplierInfo As String = String.Empty
        st_SupplierInfo = Common.GetSupplierInfo(st_SupplierCode)

        If String.IsNullOrEmpty(st_SupplierInfo) Then
            Msg.Text = "Supplier Information" & ERR_DOES_NOT_EXIST
        Else
            ScriptManager.RegisterStartupScript(Me, Me.GetType, "WindowOpen", "window.open('" & st_SupplierInfo & "');", True)
        End If

    End Sub

    Private Function FormDataSet() As Boolean
        Dim i_TryParse As Integer = 0
        Dim i As Integer = 0
        Dim j As Integer = 0
        Dim st_SelectCommand As String = String.Empty
        Dim PurchaseCommand As String = String.Empty
        Dim DS As DataSet = New DataSet
        Call ClearLineData()

        If Integer.TryParse(st_RFQNumber, i_TryParse) Then
            Try
                da_vRFQHeader = New TCIDataAccess.v_RFQHeader
                da_vRFQHeader.Load(Me.st_RFQNumber)
            Catch ex As KeyNotFoundException
                'RFQNumber 不正
                Return False
            End Try

            '権限ロールに従い極秘品はエラーとする
            If Session(SESSION_ROLE_CODE).ToString = ROLE_WRITE_P OrElse Session(SESSION_ROLE_CODE).ToString = ROLE_READ_P Then
                If IsConfidentialItem(da_vRFQHeader.ProductNumber) Then
                    Response.Redirect("AuthError.html")
                End If
            End If

            ''Purposeのプルダウンを設定
            If IsPostBack = False Then
                ListPurpose.Items.Clear()
                ListPurpose.Items.Add(String.Empty)
                ListPurpose.DataSourceID = "SrcPurpose"
                ListPurpose.DataTextField = "Text"
                ListPurpose.DataValueField = "PurposeCode"
                ListPurpose.DataBind()

                StorageLocation.Items.Clear()
                StorageLocation.Items.Add(String.Empty)
                StorageLocation.DataSourceID = "SDS_RFQUpdate_EnqStorageLocation"
                StorageLocation.DataTextField = "Storage"
                StorageLocation.DataValueField = "Storage"
                StorageLocation.DataBind()

                StorageLocation2.Items.Clear()
                StorageLocation2.Items.Add(String.Empty)
                StorageLocation2.DataSourceID = "SDS_RFQUpdate_QuoStorageLocation"
                StorageLocation2.DataTextField = "Storage"
                StorageLocation2.DataValueField = "Storage"
                StorageLocation2.DataBind()

                QuoUser.Items.Clear()
                QuoUser.Items.Add(String.Empty)
                QuoUser.DataSourceID = "SDS_RFQUpdate_QuoUser"
                QuoUser.DataTextField = "Name"
                QuoUser.DataValueField = "UserID"
                QuoUser.DataBind()

            End If
            SetPurposeDropDownList(SrcPurpose)
            Common.SetCodeExtensionDropDownList(CodeExtensionList, da_vRFQHeader.ProductNumber)
            Me.Priority.Items.Clear()
            Common.SetPriorityDropDownList(Me.Priority, Common.PRIORITY_FOR_EDIT)

            'Hidden
            QuotedDate.Value = TCIDataAccess.DBCommon.ConvertDefaultDateTimeToNull(da_vRFQHeader.QuotedDate).ToString
            UpdateDate.Value = GetUpdateDate("v_RFQHeader", "RFQNumber", st_RFQNumber)
            EnqLocationCode.Value = da_vRFQHeader.EnqLocationCode
            QuoLocationCode.Value = da_vRFQHeader.QuoLocationCode
            Hi_RFQStatusCode.Value = da_vRFQHeader.StatusCode

            'Left
            Confidential.Text = IIf(CBool(da_vRFQHeader.isCONFIDENTIAL), Common.CONFIDENTIAL, String.Empty).ToString
            RFQNumber.Text = st_RFQNumber
            CurrentRFQStatus.Text = da_vRFQHeader.Status.ToString
            CASNumber.Text = da_vRFQHeader.CASNumber.ToString
            RFQListByProductID.NavigateUrl = "#"
            RFQListByProductID.Attributes.Add("onclick", "window.open(""" & "./RFQListByProduct.aspx?ProductID=" + da_vRFQHeader.ProductID.ToString & """, ""_blank"", ""noopener"").forcus();return false;")
            RFQListByProductID.Text = CutShort(da_vRFQHeader.ProductNumber.ToString)
            CodeExtensionList.SelectedValue = da_vRFQHeader.CodeExtensionCode.ToString
            ProductName.Text = CutShort(da_vRFQHeader.ProductName.ToString)
            ProductWarning.Text = da_vRFQHeader.ProductWarning.ToString '20190902 WYS 赋值ProductWarning
            SupplierWarning.Text = da_vRFQHeader.SupplierWarning.ToString '20190902 WYS SupplierWarning
            txtVaildTo.Text = da_vRFQHeader.SupplierOfferValidTo.ToString '20191012 WYS SupplierOfferValidTo
            labBUoM.Text = da_vRFQHeader.BUoM.ToString '20200610 WYS 赋值BUoM
            SupplierCode.Text = da_vRFQHeader.SupplierCode.ToString
            R3SupplierCode.Text = da_vRFQHeader.S4SupplierCode.ToString
            SupplierName.Text = da_vRFQHeader.SupplierName.ToString
            SupplierCountry.Text = GetCountryName(da_vRFQHeader.SupplierCountryCode.ToString)
            SuplierCountryShort.Text = da_vRFQHeader.SupplierCountryCode.ToString
            CountryWarning.Text = GetCountryQuoName(da_vRFQHeader.SupplierCountryCode.ToString)
            SupplierContactPerson.Text = da_vRFQHeader.SupplierContactPerson.ToString
            MakerCode.Text = If(da_vRFQHeader.MakerCode.HasValue, da_vRFQHeader.MakerCode.GetValueOrDefault().ToString, String.Empty)
            SAPMakerCode.Text = If(da_vRFQHeader.SAPMakerCode.HasValue, da_vRFQHeader.SAPMakerCode.GetValueOrDefault().ToString, String.Empty)
            MakerName.Text = da_vRFQHeader.MakerName.ToString
            MakerCountry.Text = GetCountryName(da_vRFQHeader.MakerCountryCode.ToString)
            SupplierItemName.Text = da_vRFQHeader.SupplierItemName.ToString
            PaymentTerm.SelectedValue = da_vRFQHeader.PaymentTermCode.ToString
            ShippingHandlingCurrency.SelectedValue = da_vRFQHeader.ShippingHandlingCurrencyCode.ToString
            ShippingHandlingFee.Text = SetNullORDecimal(da_vRFQHeader.ShippingHandlingFee.ToString)
            'Right
            Purpose.Text = da_vRFQHeader.Purpose.ToString
            PurposeCode.Value = da_vRFQHeader.PurposeCode.ToString
            '判断当前值是否在下拉框中, 在则选中否则不选中
            If da_vRFQHeader.PurposeCode.ToString = "" Then
                ListPurpose.SelectedValue = ""
            Else
                Dim PurposeDt As DataTable = GetDataTable("select * from Purpose where IsVisiable=1 and Purposecode='" + da_vRFQHeader.PurposeCode.ToString + "'", "Purpose")
                If PurposeDt.Rows.Count > 0 Then
                    ListPurpose.SelectedValue = da_vRFQHeader.PurposeCode.ToString
                Else
                    ListPurpose.SelectedValue = ""
                End If
            End If
            Priority.SelectedValue = da_vRFQHeader.Priority.ToString
            LabelPriority.Text = da_vRFQHeader.Priority.ToString
            RequiredPurity.Text = da_vRFQHeader.RequiredPurity.ToString
            RequiredQMMethod.Text = da_vRFQHeader.RequiredQMMethod.ToString
            RequiredSpecification.Text = da_vRFQHeader.RequiredSpecification.ToString
            If da_vRFQHeader.SpecSheet.ToString = True Then
                SpecSheet.Checked = True
            Else
                SpecSheet.Checked = False
            End If


            Specification.Text = da_vRFQHeader.Specification.ToString

            SetSupplierContactPersonCodeList()
            SupplierContactPersonCodeList.SelectedValue = da_vRFQHeader.SupplierContactPersonSel

            If CBool(da_vRFQHeader.isCONFIDENTIAL) Then
                SDS_RFQUpdate_EnqUser.SelectCommand = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 AND RoleCode IN ('WRITE', 'WRITE_AA') and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') " _
                                             & "UNION SELECT UserID, [Name] FROM v_UserAll WHERE (UserID = {1}) ORDER BY [Name]" _
                                             , EnqLocationCode.Value, da_vRFQHeader.EnqUserID.ToString)
            Else
                SDS_RFQUpdate_EnqUser.SelectCommand = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') " _
                                                 & "UNION SELECT UserID, [Name] FROM v_UserAll WHERE (UserID = {1}) ORDER BY [Name]" _
                                                 , EnqLocationCode.Value, da_vRFQHeader.EnqUserID.ToString)
            End If

            EnqUser.SelectedValue = da_vRFQHeader.EnqUserID.ToString
            ViewState(OLD_ENQUSER_ID) = da_vRFQHeader.EnqUserID.ToString

            ' EnqLocationの設定
            SDS_RFQUpdate_EnqLocation.SelectCommand = String.Format("SELECT LocationCode, Name FROM s_Location ORDER BY Name")
            EnqLocation.DataBind()
            QuoLocation.DataBind()

            EnqLocation.SelectedValue = da_vRFQHeader.EnqLocationCode.ToString
            'by wjh
            If da_vRFQHeader.EnqUserID.ToString.Length > 0 Then
                SDS_RFQUpdate_EnqStorageLocation.SelectCommand = String.Format("SELECT Storage FROM StorageLocation  where Storage in(select Storage from StorageByPurchasingUser where UserId=" + da_vRFQHeader.EnqUserID.ToString + ") ORDER BY Storage")
            Else
                SDS_RFQUpdate_EnqStorageLocation.SelectCommand = String.Format("SELECT Storage FROM StorageLocation ORDER BY Storage")
            End If
            If da_vRFQHeader.EnqStorageLocation.ToString <> "" Then
                Dim enqTmpDt As DataTable
                enqTmpDt = GetDataTable(String.Format("SELECT Storage FROM StorageLocation where Storage in(select Storage from StorageByPurchasingUser where UserId=" + da_vRFQHeader.EnqUserID.ToString + ") and Storage='" + da_vRFQHeader.EnqStorageLocation.ToString + "'  ORDER BY Storage"))
                If enqTmpDt.Rows.Count > 0 Then
                    StorageLocation.Items.Clear()
                    StorageLocation.Items.Add(New ListItem())
                    StorageLocation.DataBind()
                    StorageLocation.SelectedValue = da_vRFQHeader.EnqStorageLocation.ToString
                End If
            End If
            If da_vRFQHeader.QuoUserID.HasValue Then
                SDS_RFQUpdate_QuoStorageLocation.SelectCommand = String.Format("SELECT Storage FROM StorageLocation where Storage in(select Storage from StorageByPurchasingUser where UserId=" + da_vRFQHeader.QuoUserID.GetValueOrDefault.ToString + ") ORDER BY Storage")
            Else
                SDS_RFQUpdate_QuoStorageLocation.SelectCommand = String.Format("SELECT Storage FROM StorageLocation ORDER BY Storage")
            End If

            If da_vRFQHeader.QuoStorageLocation.ToString <> "" Then
                Dim quoTmpDt As DataTable
                quoTmpDt = GetDataTable(String.Format("SELECT Storage FROM StorageLocation where Storage in(select Storage from StorageByPurchasingUser where UserId=" + da_vRFQHeader.QuoUserID.GetValueOrDefault.ToString + ") and Storage='" + da_vRFQHeader.QuoStorageLocation.ToString + "'  ORDER BY Storage"))
                If quoTmpDt.Rows.Count > 0 Then
                    StorageLocation2.Items.Clear()
                    StorageLocation2.Items.Add(New ListItem())
                    StorageLocation2.DataBind()
                    StorageLocation2.SelectedValue = da_vRFQHeader.QuoStorageLocation.ToString
                End If
            End If

            If da_vRFQHeader.QuoLocationName.ToString = String.Empty Then
                QuoLocation.SelectedValue = da_vRFQHeader.EnqLocationCode.ToString
            Else
                QuoLocation.SelectedValue = da_vRFQHeader.QuoLocationCode.ToString
            End If

            If CBool(da_vRFQHeader.isCONFIDENTIAL.ToString) Then
                SDS_RFQUpdate_QuoUser.SelectCommand = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 AND RoleCode IN ('WRITE', 'WRITE_AA') and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') " _
                                             & "UNION SELECT UserID, [Name] FROM v_UserAll WHERE (UserID = {1}) ORDER BY [Name]" _
                                             , QuoLocationCode.Value, da_vRFQHeader.QuoUserID.GetValueOrDefault.ToString)
            Else
                SDS_RFQUpdate_QuoUser.SelectCommand = String.Format("SELECT UserID, [Name] FROM v_User WHERE (LocationCode = '{0}' AND isDisabled = 0 and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') " _
                                                 & "UNION SELECT UserID, [Name] FROM v_UserAll WHERE (UserID = {1}) ORDER BY [Name]" _
                                                 , QuoLocationCode.Value, da_vRFQHeader.QuoUserID.GetValueOrDefault.ToString)
            End If

            QuoUser.DataBind()
            QuoUser.SelectedValue = If(da_vRFQHeader.QuoUserID.HasValue, da_vRFQHeader.QuoUserID.GetValueOrDefault.ToString, String.Empty)
            ViewState(OLD_QUOUSER_ID) = If(da_vRFQHeader.QuoUserID.HasValue, da_vRFQHeader.QuoUserID.GetValueOrDefault.ToString, String.Empty)

            Comment.Text = da_vRFQHeader.Comment.ToString
            'Under
            RFQStatus.SelectedValue = ""
            If Session("LocationCode") <> EnqLocationCode.Value Then
                Close.Visible = False
            Else
                Close.Visible = True
            End If
            If da_vRFQHeader.StatusCode.ToString = "II" And Session("LocationCode") = QuoLocationCode.Value Then
                Close.Visible = True
            End If

            ' RFQHeader.MMSTAInvalidation = 1 の場合はチェックボックスをオン、0の場合はオフ
            Me.MMSTAInvalidation.Checked = (da_vRFQHeader.MMSTAInvalidation.ToString = BOOLMMSTAInvalidationValue.Y)
            ' Session.MMSTAInvalidationEditable = 1 の場合はチェックボックスを活性化、0の場合は非活性化とする
            Me.MMSTAInvalidation.Enabled = Session.Contents("Purchase.MMSTAInvalidationEditable") AndAlso (Session("Purchase.MMSTAInvalidationEditable").ToString = BOOLMMSTAInvalidationEditable.Y)


            'Line
            DBCommand = New SqlCommand("Select " _
& "RFQLineNumber, StatusCode, EnqQuantity, EnqUnitCode, EnqPiece, CurrencyCode, " _
& "UnitPrice, QuoPer, QuoUnitCode, LeadTime, SupplierItemNumber, " _
& "IncotermsCode, DeliveryTerm, Packing, Purity, QMMethod,SupplierOfferNo, NoOfferReasonCode,OutputStatus" _
& " From v_RFQLine Where RFQNumber = @i_RFQNumber Order by RFQLineNumber", DBConn)
            DBCommand.Parameters.Add("i_RFQNumber", SqlDbType.Int).Value = Integer.Parse(st_RFQNumber)
            DBAdapter = New SqlDataAdapter
            DBAdapter.SelectCommand = DBCommand

            DBAdapter.Fill(DS, "RFQLine")
            DBCommand.Dispose()

            Dim RFQLineNumberList As List(Of String) = New List(Of String)
            If DS.Tables("RFQLine").Rows.Count = 0 Then
            Else
                Dim i_Cnt As Integer = 0
                i_Cnt = IIf(LINE_COUNT > DS.Tables("RFQLine").Rows.Count, DS.Tables("RFQLine").Rows.Count, LINE_COUNT)
                For i = 0 To i_Cnt - 1
                    j = i + 1

                    Dim statusCode = DS.Tables("RFQLine").Rows(i).Item("StatusCode").ToString
                    Dim isReadOnly As Boolean
                    isReadOnly = Not ("N" & vbTab & "A").Split(vbTab).Contains(statusCode)

                    ' Current Status(v_RFQHeader.StatusCode)が 'N’ (Create) および ‘A’ (Assigned) でない、かつ何らかの値が登録されている場合は非活性化する。
                    EnqQuantity(j).Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("EnqQuantity").ToString)
                    EnqQuantity(j).ReadOnly = isReadOnly AndAlso (Not String.IsNullOrEmpty(EnqQuantity(j).Text))
                    EnqQuantity(j).CssClass = If(EnqQuantity(j).ReadOnly, "readonly number", "")

                    ' Current Status(v_RFQHeader.StatusCode)が 'N’ (Create) および ‘A’ (Assigned) でない、かつ何らかの値が登録されている場合は非活性化する。
                    EnqUnit(j).SelectedValue = DS.Tables("RFQLine").Rows(i).Item("EnqUnitCode").ToString
                    EnqUnit(j).Enabled = Not (isReadOnly AndAlso (Not String.IsNullOrEmpty(EnqUnit(j).SelectedValue)))
                    EnqUnit(j).CssClass = If(Not EnqUnit(j).Enabled, "readonly", "")

                    ' Current Status(v_RFQHeader.StatusCode)が 'N’ (Create) および ‘A’ (Assigned) でない、かつ何らかの値が登録されている場合は非活性化する。
                    EnqPiece(j).Text = DS.Tables("RFQLine").Rows(i).Item("EnqPiece").ToString
                    EnqPiece(j).ReadOnly = isReadOnly AndAlso (Not String.IsNullOrEmpty(EnqPiece(j).Text))
                    EnqPiece(j).CssClass = If(EnqPiece(j).ReadOnly, "readonly number", "")

                    Incoterms(j).SelectedValue = DS.Tables("RFQLine").Rows(i).Item("IncotermsCode").ToString
                    Currency(j).SelectedValue = DS.Tables("RFQLine").Rows(i).Item("CurrencyCode").ToString
                    UnitPrice(j).Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("UnitPrice").ToString)
                    DeliveryTerm(j).Text = DS.Tables("RFQLine").Rows(i).Item("DeliveryTerm").ToString
                    QuoPer(j).Text = SetNullORDecimal(DS.Tables("RFQLine").Rows(i).Item("QuoPer").ToString)
                    Purity(j).Text = DS.Tables("RFQLine").Rows(i).Item("Purity").ToString
                    If DS.Tables("RFQLine").Rows(i).Item("QuoUnitCode").ToString = "" Then
                        QuoUnit(j).SelectedValue = DS.Tables("RFQLine").Rows(i).Item("EnqUnitCode").ToString
                    Else
                        QuoUnit(j).SelectedValue = DS.Tables("RFQLine").Rows(i).Item("QuoUnitCode").ToString()
                    End If
                    QMMethod(j).Text = DS.Tables("RFQLine").Rows(i).Item("QMMethod").ToString
                    SupplierOfferNo(j).Text = DS.Tables("RFQLine").Rows(i).Item("SupplierOfferNo").ToString
                    LeadTime(j).Text = DS.Tables("RFQLine").Rows(i).Item("LeadTime").ToString
                    Packing(j).Text = DS.Tables("RFQLine").Rows(i).Item("Packing").ToString
                    SupplierItemNumber(j).Text = DS.Tables("RFQLine").Rows(i).Item("SupplierItemNumber").ToString
                    NoOfferReason(j).SelectedValue = DS.Tables("RFQLine").Rows(i).Item("NoOfferReasonCode").ToString
                    POIssue(j).Visible = True
                    POIssue(j).Enabled = False
                    POIssue(j).NavigateUrl = "./POIssue.aspx?RFQLineNumber=" & DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                    LineNumber(j).Value = DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                    RFQLineNumberList.Add(DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString)
                    'POInterface(j).Visible = True
                    'POInterface(j).NavigateUrl = "./POInterface.aspx?RFQLineNumber=" & DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                    POInterfaceButton(j).Visible = True
                Next
            End If
            '判断Purpose是否合法  
            Dim tmpQuoUnitCode As String = ""
            Dim isFirstClickPointerfac As String = ""
            If DS.Tables("RFQLine").Rows.Count >= 1 Then
                tmpQuoUnitCode = DS.Tables("RFQLine").Rows(0).Item("QuoUnitCode").ToString
                isFirstClickPointerfac = DS.Tables("RFQLine").Rows(0).Item("OutputStatus").ToString
                '判断Quo单位是否是L ML LB 
                If tmpQuoUnitCode = "L" Or tmpQuoUnitCode = "ML" Or tmpQuoUnitCode = "LB" Then
                    '判断是否是第一次点击
                    If isFirstClickPointerfac = "1" Or isFirstClickPointerfac.ToLower() = "true" Then
                        PFC1.Value = "2"
                        POIssue_1.Enabled = False
                        'POIssue_1.Attributes.Add("onclick", "return alert('The function was removed!')")
                    Else
                        PFC1.Value = "3"
                    End If
                Else
                    '判断是否是第一次点击
                    If isFirstClickPointerfac = "1" Or isFirstClickPointerfac.ToLower() = "true" Then
                        PFC1.Value = "4"
                        POIssue_1.Enabled = False
                        'POIssue_1.Attributes.Add("onclick", "return alert('The function was removed!')")
                    Else
                        PFC1.Value = "5"
                    End If
                End If
            End If

            If DS.Tables("RFQLine").Rows.Count >= 2 Then
                tmpQuoUnitCode = DS.Tables("RFQLine").Rows(1).Item("QuoUnitCode").ToString
                isFirstClickPointerfac = DS.Tables("RFQLine").Rows(1).Item("OutputStatus").ToString
                If tmpQuoUnitCode = "L" Or tmpQuoUnitCode = "ML" Or tmpQuoUnitCode = "LB" Then
                    If isFirstClickPointerfac = "1" Or isFirstClickPointerfac.ToLower() = "true" Then
                        PFC2.Value = "2"
                        POIssue_2.Enabled = False
                        'POIssue_2.Attributes.Add("onclick", "return alert('The function was removed!')")
                    Else
                        PFC2.Value = "3"
                    End If
                Else
                    If isFirstClickPointerfac = "1" Or isFirstClickPointerfac.ToLower() = "true" Then
                        PFC2.Value = "4"
                        POIssue_2.Enabled = False
                        'POIssue_2.Attributes.Add("onclick", "return alert('The function was removed!')")
                    Else
                        PFC2.Value = "5"
                    End If
                End If
            End If

            If DS.Tables("RFQLine").Rows.Count >= 3 Then
                tmpQuoUnitCode = DS.Tables("RFQLine").Rows(2).Item("QuoUnitCode").ToString
                isFirstClickPointerfac = DS.Tables("RFQLine").Rows(2).Item("OutputStatus").ToString
                If tmpQuoUnitCode = "L" Or tmpQuoUnitCode = "ML" Or tmpQuoUnitCode = "LB" Then
                    If isFirstClickPointerfac = "1" Or isFirstClickPointerfac.ToLower() = "true" Then
                        PFC3.Value = "2"
                        POIssue_3.Enabled = False
                        'POIssue_3.Attributes.Add("onclick", "return alert('The function was removed!')")
                    Else
                        PFC3.Value = "3"
                    End If
                Else
                    If isFirstClickPointerfac = "1" Or isFirstClickPointerfac.ToLower() = "true" Then
                        PFC3.Value = "4"
                        POIssue_3.Enabled = False
                        'POIssue_3.Attributes.Add("onclick", "return alert('The function was removed!')")
                    Else
                        PFC3.Value = "5"
                    End If
                End If
            End If

            If DS.Tables("RFQLine").Rows.Count >= 4 Then
                tmpQuoUnitCode = DS.Tables("RFQLine").Rows(3).Item("QuoUnitCode").ToString
                isFirstClickPointerfac = DS.Tables("RFQLine").Rows(3).Item("OutputStatus").ToString
                If tmpQuoUnitCode = "L" Or tmpQuoUnitCode = "ML" Or tmpQuoUnitCode = "LB" Then
                    If isFirstClickPointerfac = "1" Or isFirstClickPointerfac.ToLower() = "true" Then
                        PFC4.Value = "2"
                        POIssue_4.Enabled = False
                        'POIssue_4.Attributes.Add("onclick", "return alert('The function was removed!')")
                    Else
                        PFC4.Value = "3"
                    End If
                Else
                    If isFirstClickPointerfac = "1" Or isFirstClickPointerfac.ToLower() = "true" Then
                        PFC4.Value = "4"
                        POIssue_4.Enabled = False
                        'POIssue_4.Attributes.Add("onclick", "return alert('The function was removed!')")
                    Else
                        PFC4.Value = "5"
                    End If
                End If
            End If
            ' EnqLocationの活性制御
            If RFQLineNumberList.Count > 0 Then
                Dim RFQLineNumberWhere As StringBuilder = New StringBuilder()
                For Each RFQLineNumber In RFQLineNumberList
                    RFQLineNumberWhere.Append(" " & RFQLineNumber & ",")
                Next
                DBCommand = New SqlCommand("Select " _
        & " Count(PONumber) as PoCount " _
        & " From PO Where CancellationDate is Null " _
        & " AND RFQLineNumber In (" & RFQLineNumberWhere.ToString().TrimEnd(",") & ") ", DBConn)
                DBAdapter = New SqlDataAdapter
                DBAdapter.SelectCommand = DBCommand
                DBAdapter.Fill(DS, "PO")
                Dim PoCount As Integer = Integer.Parse(DS.Tables("PO").Rows(0)("PoCount"))
                If PoCount > 0 Then
                    'Poのキャンセル以外のデータが存在する場合、EnqLocation編集不可
                    EnqLocation.CssClass = "readonly"
                    EnqLocation.Enabled = False
                    'EnqLocation.AutoPostBack = False
                    'StorageLocation.CssClass = "readonly"
                    'StorageLocation.AutoPostBack = False
                    QuoLocation.CssClass = "readonly"
                    QuoLocation.Enabled = False
                End If
            End If

            DS.Clear()
        End If



        If Hi_RFQStatusCode.Value = STATUS_CLOSED Then
            'ListPurpose.Visible = False
            ListPurpose.Attributes.Add("style", "display:none")
            Purpose.Visible = True
        Else
            'ListPurpose.Visible = True
            ListPurpose.Attributes.Add("style", "display:block")
            Purpose.Visible = False
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

    Private Function isNum(ByVal a As String) As Boolean
        Try
            If System.Int32.Parse(a) < 0 Then
                Return True
            End If
        Catch ex As Exception
            Return True
        End Try
        Return False
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

    ''' <summary>
    ''' 根据countrycode获取对应的quoname
    ''' </summary>
    ''' <param name="Code"></param>
    ''' <returns></returns>
    Private Function GetCountryQuoName(ByVal Code As String) As String
        Dim DBReader As SqlDataReader
        GetCountryQuoName = String.Empty
        DBCommand.CommandText = "SELECT DefaultQuoLocationName FROM v_Country WHERE (CountryCode = @CountryCode)"
        DBCommand.Parameters.Add("@CountryCode", SqlDbType.NVarChar).Value = Code
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Parameters.Clear()
        DBCommand.Dispose()
        If DBReader.HasRows = True Then
            While DBReader.Read
                GetCountryQuoName = DBReader("DefaultQuoLocationName").ToString
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
            Else
                Dim supplierDt = GetDataTable("select S4SupplierCode from supplier where SupplierCode=" + MakerCode.Text)
                If supplierDt.Rows.Count > 0 Then
                    SAPMakerCode.Text = supplierDt.Rows(0)("S4SupplierCode").ToString
                Else
                    SAPMakerCode.Text = ""
                End If
            End If
        End If
        'If MakerCode.Text <> String.Empty Then
        '    If SAPMakerCode.Text = "" Then
        '        Msg.Text = "Please make sure SAP Maker Code already been created!"
        '        Return False
        '    End If
        'End If
        Return True
    End Function

    Protected Function CheckUntreatedCorrespondence(ByVal RFQNumber As Integer, ByVal UserID As Integer) As Boolean
        ' 未処理コレポンチェック。未処理コレポンがある場合False を返す。
        Dim b_flag As Boolean = True

        Using connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)

            Dim command As New SqlClient.SqlCommand("SELECT 1 FROM v_RFQReminder WHERE RFQNumber = @RFQNumber AND RcptUserID = @UserID", connection)
            Dim reader As SqlClient.SqlDataReader

            command.Parameters.AddWithValue("UserID", UserID)
            command.Parameters.AddWithValue("RFQNumber", RFQNumber)
            connection.Open()

            reader = command.ExecuteReader()
            If reader.HasRows Then
                b_flag = False
            End If

            reader.Close()
            connection.Close()

        End Using

        Return b_flag

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
        ' ログインユーザーが管理権限がない場合に判定する
        If Session("Purchase.isAdmin") = False Then
            ' 登録済みのEnqLocationCode　が　TCI-J or TCI-Sの場合
            If EnqLocationCode.Value.Equals("JP") Or EnqLocationCode.Value.Equals("CN") Then
                ' ログインユーザが TCI-J または TCI-S または Quo-Location に所属しない場合False
                If Session("LocationCode") <> "JP" And Session("LocationCode") <> "CN" And Session("LocationCode") <> QuoLocationCode.Value Then
                    Msg.Text = ERR_ANOTHER_LOCATION
                    Return False
                End If
            Else
                ' ログインユーザが Enq-Location または Quo-Location に所属しない場合False
                If Session("LocationCode") <> EnqLocationCode.Value And Session("LocationCode") <> QuoLocationCode.Value Then
                    Msg.Text = ERR_ANOTHER_LOCATION
                    Return False
                End If
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

        'If CheckLineSet() = False Then
        '    'Currency,Price,Quo-Per,Quo-Unitの入力チェック
        '    Msg.Text = ERR_INCORRECT_CURRENCY
        '    Return False
        'End If
        Return CheckLineSet()
    End Function
    Private Function CheckLineSet() As Boolean
        If RFQStatus.SelectedValue = "Q" Then
            'RFQLineのCurrency,Price,QuoPer,QuoUnitはどこかが空白で更新することができない。
            For i As Integer = LINE_START To LINE_COUNT
                If Currency(i).SelectedValue.Trim = String.Empty And UnitPrice(i).Text.Trim = String.Empty And QuoPer(i).Text.Trim = String.Empty Then
                    'If Currency(i).SelectedValue.Trim = String.Empty And UnitPrice(i).Text.Trim = String.Empty And QuoUnit(i).SelectedValue.Trim = String.Empty Then
                    '判断当前行的Reason for "No Offer"是否有值，有值不处理，没有值就提示
                    If LineNumber(i).Value.ToString <> "" And NoOfferReason(i).SelectedValue.ToString = "" Then
                        Msg.Text = "Please fill in Price or select Reason for 'No Offer'"
                        Return False
                    End If
                ElseIf Currency(i).SelectedValue.Trim = String.Empty Then
                    Msg.Text = ERR_INCORRECT_CURRENCY
                    Return False
                ElseIf UnitPrice(i).Text.Trim = String.Empty Then
                    Msg.Text = ERR_INCORRECT_CURRENCY
                    Return False
                ElseIf QuoPer(i).Text.Trim = String.Empty Then
                    Msg.Text = ERR_INCORRECT_CURRENCY
                    Return False
                    'ElseIf QuoUnit(i).SelectedValue.Trim = String.Empty Then
                    '    Return False
                End If
            Next
            Return True
        Else
            Return True
        End If
    End Function
    Private Function CheckLineEnqQuantity() As Boolean
        'RFQLineのEnqQuantity,EnqUnit,EnqPieceはどこかが空白で登録することができない。
        Dim b_IsNull_ALLLine As Boolean = True
        'b_IsNull_ALLLine が True の場合は、全行が空行である。一行でもデータ入力がある場合は False にする。
        For i As Integer = LINE_START To LINE_COUNT
            If POIssue(i).Visible = True Then
                '登録済で変更不可の行はチェックしない。
                b_IsNull_ALLLine = False
                Continue For
            End If
            'If POInterface(i).Visible = True Then
            '    b_IsNull_ALLLine = False
            '    Continue For
            'End If
            If POInterfaceButton(i).Visible = True Then
                b_IsNull_ALLLine = False
                Continue For
            End If
            If EnqQuantity(i).Text.Trim = String.Empty _
                And EnqUnit(i).SelectedValue.Trim = String.Empty _
                And EnqPiece(i).Text.Trim = String.Empty Then
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
            b_IsNull_ALLLine = False
        Next
        If b_IsNull_ALLLine = True And Purpose.Text <> "JFYI" Then
            'JFYI 以外は全て未入力で登録することができない。'
            Return False
        End If
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
            SupplierOfferNo(i) = CType(FindControl(String.Format("{0}_{1}", "SupplierOfferNo", i)), TextBox)
            Packing(i) = CType(FindControl(String.Format("{0}_{1}", "Packing", i)), TextBox)
            NoOfferReason(i) = CType(FindControl(String.Format("{0}_{1}", "NoOfferReason", i)), DropDownList)
            'POInterface(i) = CType(FindControl(String.Format("{0}_{1}", "POInterface", i)), HyperLink)
            POInterfaceButton(i) = CType(FindControl(String.Format("{0}_{1}", "POInterfaceButton", i)), Button)
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
            SupplierOfferNo(i).Text = String.Empty
            Packing(i).Text = String.Empty
            NoOfferReason(i).Items.Clear()
            NoOfferReason(i).Items.Add(String.Empty)
            NoOfferReason(i).DataSourceID = "SDS_RFQUpdate_NoOffer"
            NoOfferReason(i).DataTextField = "Text"
            NoOfferReason(i).DataValueField = "NoOfferReasonCode"
            NoOfferReason(i).DataBind()
        Next
    End Sub

    Protected Sub EnqLocation_SelectedIndexChanged(sender As Object, e As EventArgs) Handles EnqLocation.SelectedIndexChanged
        '[EnqUserIDの値設定]--------------------------------------------------------------------
        If EnqLocation.CssClass = "readonly" Then
            ' 処理を中断して変更不可とする。
            EnqLocation.SelectedValue = EnqLocationCode.Value

            Msg.Text = String.Empty
            'DBCommand = DBConn.CreateCommand()
            'If String.IsNullOrEmpty(Confidential.Text) Then
            '    DBCommand.CommandText = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') ORDER BY [Name] " _
            '                                 , EnqLocation.SelectedValue)
            'Else
            '    DBCommand.CommandText = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 AND RoleCode = 'WRITE' and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') ORDER BY [Name] " _
            '                                     , EnqLocation.SelectedValue)
            'End If
            'Dim DBReader As System.Data.SqlClient.SqlDataReader
            'DBReader = DBCommand.ExecuteReader()
            'DBCommand.Dispose()
            'EnqUser.Items.Clear()
            'Do Until DBReader.Read = False
            '    EnqUser.Items.Add(New ListItem(DBReader("Name").ToString, DBReader("UserID").ToString))
            'Loop
            'DBReader.Close()
            'DBConn.Close()
            Dim sql As String = ""
            Dim dt As DataTable
            If String.IsNullOrEmpty(Confidential.Text) Then
                sql = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') ORDER BY [Name] " _
                                             , EnqLocation.SelectedValue)
            Else
                sql = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 AND RoleCode IN ('WRITE', 'WRITE_AA') and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') ORDER BY [Name] " _
                                                 , EnqLocation.SelectedValue)
            End If
            dt = GetDataTable(sql)
            EnqUser.Items.Clear()
            For i As Integer = 0 To dt.Rows.Count - 1
                EnqUser.Items.Add(New ListItem(dt.Rows(i)("Name").ToString, dt.Rows(i)("UserID").ToString))
            Next
            If dt.Rows.Count > 0 Then
                Dim tmpdt As DataTable
                tmpdt = GetDataTable("select * from StorageLocation where Storage in(select Storage from StorageByPurchasingUser where UserId=" + dt.Rows(0)("UserID").ToString + ")")
                StorageLocation.Items.Clear()
                StorageLocation.Items.Add(String.Empty)
                For i As Integer = 0 To tmpdt.Rows.Count - 1
                    StorageLocation.Items.Add(New ListItem(tmpdt.Rows(i)("Storage").ToString, tmpdt.Rows(i)("Storage").ToString))
                Next
            End If
        Else
            Msg.Text = String.Empty
            'DBCommand = DBConn.CreateCommand()
            'If String.IsNullOrEmpty(Confidential.Text) Then
            '    DBCommand.CommandText = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') ORDER BY [Name] " _
            '                                 , EnqLocation.SelectedValue)
            'Else
            '    DBCommand.CommandText = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 AND RoleCode = 'WRITE' and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') ORDER BY [Name] " _
            '                                     , EnqLocation.SelectedValue)
            'End If
            'Dim DBReader As System.Data.SqlClient.SqlDataReader
            'DBReader = DBCommand.ExecuteReader()
            'DBCommand.Dispose()
            'EnqUser.Items.Clear()
            'Do Until DBReader.Read = False
            '    EnqUser.Items.Add(New ListItem(DBReader("Name").ToString, DBReader("UserID").ToString))
            'Loop
            'DBReader.Close()
            'DBConn.Close()
            Dim sql As String = ""
            Dim dt As DataTable
            If String.IsNullOrEmpty(Confidential.Text) Then
                sql = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') ORDER BY [Name] " _
                                             , EnqLocation.SelectedValue)
            Else
                sql = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 AND RoleCode IN ('WRITE', 'WRITE_AA') and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') ORDER BY [Name] " _
                                                 , EnqLocation.SelectedValue)
            End If
            dt = GetDataTable(sql)
            EnqUser.Items.Clear()
            For i As Integer = 0 To dt.Rows.Count - 1
                EnqUser.Items.Add(New ListItem(dt.Rows(i)("Name").ToString, dt.Rows(i)("UserID").ToString))
            Next
            If dt.Rows.Count > 0 Then
                Dim tmpdt As DataTable
                tmpdt = GetDataTable("select * from StorageLocation where Storage in(select Storage from StorageByPurchasingUser where UserId=" + dt.Rows(0)("UserID").ToString + ")")
                StorageLocation.Items.Clear()
                StorageLocation.Items.Add(String.Empty)
                For i As Integer = 0 To tmpdt.Rows.Count - 1
                    StorageLocation.Items.Add(New ListItem(tmpdt.Rows(i)("Storage").ToString, tmpdt.Rows(i)("Storage").ToString))
                Next
            End If
        End If

    End Sub

    Protected Sub POInterfaceButton_1_Click(sender As Object, e As EventArgs) Handles POInterfaceButton_1.Click
        If SetRFQNumber() = False Then
            Exit Sub
        End If
        '先判断数据是否符合update验证
        If RFQCheck(st_RFQNumber, "1") = False Then
            Exit Sub
        End If
        Dim tmpQuoUnitCode As String = ""
        tmpQuoUnitCode = POInterfaceFunction(st_RFQNumber, LineNumber1.Value, 1)
        If tmpQuoUnitCode <> "" Then
            POIssue_1.Enabled = False
            'POIssue_1.Attributes.Add("onclick", "return alert('The function was removed!')")
            If tmpQuoUnitCode = "LB" Or tmpQuoUnitCode = "L" Or tmpQuoUnitCode = "ML" Then
                PFC1.Value = "2"
            Else
                PFC1.Value = "4"
            End If
            'Response.Write("<script>alert('PO Interface create successfully!')</script>")
            ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>MyFun();</script>")
        End If
    End Sub

    Protected Sub POInterfaceButton_2_Click(sender As Object, e As EventArgs) Handles POInterfaceButton_2.Click
        If SetRFQNumber() = False Then
            Exit Sub
        End If
        If RFQCheck(st_RFQNumber, "2") = False Then
            Exit Sub
        End If
        Dim tmpQuoUnitCode As String = ""
        tmpQuoUnitCode = POInterfaceFunction(st_RFQNumber, LineNumber2.Value, 2)
        If tmpQuoUnitCode <> "" Then
            POIssue_2.Enabled = False
            'POIssue_2.Attributes.Add("onclick", "return alert('The function was removed!')")
            If tmpQuoUnitCode = "LB" Or tmpQuoUnitCode = "L" Or tmpQuoUnitCode = "ML" Then
                PFC2.Value = "2"
            Else
                PFC2.Value = "4"
            End If
            'Response.Write("<script>alert('PO Interface create successfully!')</script>")
            ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>MyFun();</script>")
        End If
    End Sub

    Protected Sub POInterfaceButton_3_Click(sender As Object, e As EventArgs) Handles POInterfaceButton_3.Click
        If SetRFQNumber() = False Then
            Exit Sub
        End If
        If RFQCheck(st_RFQNumber, "3") = False Then
            Exit Sub
        End If
        Dim tmpQuoUnitCode As String = ""
        tmpQuoUnitCode = POInterfaceFunction(st_RFQNumber, LineNumber3.Value, 3)
        If tmpQuoUnitCode <> "" Then
            POIssue_3.Enabled = False
            'POIssue_3.Attributes.Add("onclick", "return alert('The function was removed!')")
            If tmpQuoUnitCode = "LB" Or tmpQuoUnitCode = "L" Or tmpQuoUnitCode = "ML" Then
                PFC3.Value = "2"
            Else
                PFC3.Value = "4"
            End If
            'Response.Write("<script>alert('PO Interface create successfully!')</script>")
            ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>MyFun();</script>")
        End If
    End Sub

    Protected Sub POInterfaceButton_4_Click(sender As Object, e As EventArgs) Handles POInterfaceButton_4.Click
        If SetRFQNumber() = False Then
            Exit Sub
        End If
        If RFQCheck(st_RFQNumber, "4") = False Then
            Exit Sub
        End If
        Dim tmpQuoUnitCode As String = ""
        tmpQuoUnitCode = POInterfaceFunction(st_RFQNumber, LineNumber4.Value, 4)
        If tmpQuoUnitCode <> "" Then
            POIssue_4.Enabled = False
            'POIssue_4.Attributes.Add("onclick", "return alert('The function was removed!')")
            If tmpQuoUnitCode = "LB" Or tmpQuoUnitCode = "L" Or tmpQuoUnitCode = "ML" Then
                PFC4.Value = "2"
            Else
                PFC4.Value = "4"
            End If
            'Response.Write("<script>alert('PO Interface create successfully!')</script>")
            ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>MyFun();</script>")
        End If
    End Sub

    Protected Sub SupplierContactPersonCodeList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles SupplierContactPersonCodeList.SelectedIndexChanged
        'Dim Sql As String = ""
        'Sql = "select * from ( select * from (select '' as supplierInfo,'' as SupplierContactperson FROM  Supplier where SupplierCode=" + SupplierCode.Text + " "
        'Sql += "Union All select (SupplierEmailID1+'-'+ SupplierEmail1) as supplierInfo,ISNULL(SupplierContactperson1,'') as SupplierContactperson FROM  Supplier where SupplierCode=" + SupplierCode.Text + " "
        'Sql += "Union All select (SupplierEmailID2+'-'+ SupplierEmail2) as supplierInfo,ISNULL(SupplierContactperson2,'') as SupplierContactperson FROM  Supplier where SupplierCode=" + SupplierCode.Text + " "
        'Sql += "Union All select (SupplierEmailID3+'-'+ SupplierEmail3) as supplierInfo,ISNULL(SupplierContactperson3,'') as SupplierContactperson FROM  Supplier where SupplierCode=" + SupplierCode.Text + " "
        'Sql += "Union All select (SupplierEmailID4+'-'+ SupplierEmail4) as supplierInfo,ISNULL(SupplierContactperson4,'') as SupplierContactperson FROM  Supplier where SupplierCode=" + SupplierCode.Text + " "
        'Sql += "Union All select (SupplierEmailID5+'-'+ SupplierEmail5) as supplierInfo,ISNULL(SupplierContactperson5,'') as SupplierContactperson FROM  Supplier where SupplierCode=" + SupplierCode.Text + " "
        'Sql += "Union All select (SupplierEmailID6+'-'+ SupplierEmail6) as supplierInfo,ISNULL(SupplierContactperson6,'') as SupplierContactperson FROM  Supplier where SupplierCode=" + SupplierCode.Text + " "
        'Sql += "Union All select (SupplierEmailID7+'-'+ SupplierEmail7) as supplierInfo,ISNULL(SupplierContactperson7,'') as SupplierContactperson FROM  Supplier where SupplierCode=" + SupplierCode.Text + " "
        'Sql += "Union All select (SupplierEmailID8+'-'+ SupplierEmail8) as supplierInfo,ISNULL(SupplierContactperson8,'') as SupplierContactperson FROM  Supplier where SupplierCode=" + SupplierCode.Text + " "
        'Sql += "Union All select (SupplierEmailID9+'-'+ SupplierEmail9) as supplierInfo,ISNULL(SupplierContactperson9,'') as SupplierContactperson FROM  Supplier where SupplierCode=" + SupplierCode.Text + " "
        'Sql += "Union All select (SupplierEmailID10+'-'+ SupplierEmail10) as supplierInfo,ISNULL(SupplierContactperson10,'') as SupplierContactperson FROM  Supplier where SupplierCode=" + SupplierCode.Text + ") as A "
        'Sql += "where supplierInfo is not null) As B where supplierInfo='" + SupplierContactPersonCodeList.SelectedValue + "'"
        'Dim DBConn As New System.Data.SqlClient.SqlConnection(DB_CONNECT_STRING)
        'Dim DBAdapter As System.Data.SqlClient.SqlDataAdapter
        'Dim DBCommand As System.Data.SqlClient.SqlCommand
        'DBConn.Open()
        'DBCommand = DBConn.CreateCommand()
        'DBAdapter = New SqlDataAdapter
        'Dim DS As DataSet = New DataSet
        'DBCommand = New SqlCommand(Sql, DBConn)
        'DBAdapter.SelectCommand = DBCommand
        'DBAdapter.Fill(DS)
        'Dim tmpdt As DataTable
        'tmpdt = DS.Tables(0)
        'DBCommand.Dispose()
        'DBConn.Close()
        'If tmpdt.Rows.Count > 0 Then
        '    SupplierContactPerson.Text = tmpdt.Rows(0)("SupplierContactperson")
        'End If
        If SupplierContactPersonCodeList.SelectedValue.Length > 0 AndAlso SupplierContactPersonCodeList.SelectedValue.IndexOf("-") > -1 Then
            SupplierContactPerson.Text = SupplierContactPersonCodeList.SelectedValue.Split("-")(0).ToString
        End If
    End Sub

    Public Function GetInfo() As String
        Dim DataTable As System.Data.DataTable = GetDataTable("select Comment from v_RFQHeader where RFQNumber='" + RFQNumber.Text + "'", "v_RFQHeader")
        If DataTable IsNot Nothing And DataTable.Rows.Count > 0 Then
            'Return DataTable.Rows(0)("Comment")
            If Not IsDBNull(DataTable.Rows(0)("Comment")) Then
                Dim info = Replace(DataTable.Rows(0)("Comment"), Chr(13), "")
                info = Replace(info, Chr(10), " ")
                Return info
            Else
                Return ""
            End If
        End If
        Return ""
    End Function

    Public Function POInterfaceFunction(ByVal RFQNumber As String, ByVal RFQLineNumber As String, ByVal index As String) As String
        Msg.Text = String.Empty

        If Not String.IsNullOrEmpty(POInterfaceMsg.Text) Then
            Msg.Text = POInterfaceMsg.Text
            Msg.Text = Replace(Msg.Text, "[Enq-Location]", EnqUserStatus.Text)
            Msg.Text = Replace(Msg.Text, "[Quo-Location]", EnqUserStatus.Text)
            Return String.Empty
        End If

        Dim ExecuteSql As String = ""
        '获取主表信息
        Dim DS As DataSet = New DataSet
        DBCommand = New SqlCommand("Select * From v_RFQHeader Where RFQNumber = @i_RFQNumber", DBConn)
        DBCommand.Parameters.Add("i_RFQNumber", SqlDbType.Int).Value = Integer.Parse(RFQNumber)
        DBAdapter = New SqlDataAdapter
        DBAdapter.SelectCommand = DBCommand
        DBAdapter.Fill(DS, "RFQHeader")
        DBCommand.Dispose()
        '获取选中行的信息
        Dim DS2 As DataSet = New DataSet
        DBCommand = New SqlCommand("Select * From v_RFQLine Where RFQNumber = @i_RFQNumber and RFQLineNumber=" + RFQLineNumber + " Order by RFQLineNumber", DBConn)
        DBCommand.Parameters.Add("i_RFQNumber", SqlDbType.Int).Value = Integer.Parse(RFQNumber)
        DBAdapter.SelectCommand = DBCommand
        DBAdapter.Fill(DS2, "RFQLine")
        DBCommand.Dispose()
        Dim parameter(39) As String
        '整理数据
        '获取编号从配置文件获取最大的编号
        parameter(0) = DS2.Tables("RFQLine").Rows(0).Item("RFQLineNumber").ToString
        'Pattern
        Dim QuoPlantRow As DataRow = GetDataRow("select * from  StorageLocation where Storage='" + DS.Tables("RFQHeader").Rows(0)("QuoStorageLocation").ToString + "'", "StorageLocation")
        Dim QuoPlant As String = ""
        If QuoPlantRow IsNot Nothing Then
            QuoPlant = QuoPlantRow("Plant")
        End If
        Dim EnqPlantRow As DataRow = GetDataRow("select * from  StorageLocation where Storage='" + DS.Tables("RFQHeader").Rows(0)("EnqStorageLocation").ToString + "'", "StorageLocation")
        Dim EnqPlant As String = ""
        If EnqPlantRow IsNot Nothing Then
            EnqPlant = EnqPlantRow("Plant")
        End If
        If QuoPlant <> "" And EnqPlant <> "" Then
            If QuoPlant = EnqPlant Then
                parameter(1) = "A"
            ElseIf QuoPlant.Substring(0, 1) = "H" Then
                parameter(1) = "C"
            Else
                parameter(1) = "B"
            End If
        Else
            parameter(1) = "B"
        End If
        'Supplying Plant (PDB Quo-USER plant)
        parameter(2) = QuoPlant
        'Receiving Plant (PDB Enq-USER plant)
        parameter(3) = EnqPlant
        'Purchase Org. for Shipping of STO
        Dim QuoInfo As DataTable = GetDataTable("select * from PurchasingUser where  UserID='" + DS.Tables("RFQHeader").Rows(0)("QuoUserID").ToString + "'", "PurchasingUser")
        'If QuoInfo.Rows.Count > 0 Then
        '    If QuoInfo.Rows(0)("R3PurchasingGroup").ToString.Length > 0 Then
        '        parameter(4) = QuoInfo.Rows(0)("R3PurchasingGroup").ToString.Substring(0, 1) + "K00"
        '    Else
        '        parameter(4) = ""
        '    End If
        'Else
        '    parameter(4) = ""
        'End If
        '计算逻辑错误导致的修改-0520
        If DS.Tables("RFQHeader").Rows(0)("QuoStorageLocation").ToString.Length > 0 Then
            parameter(4) = DS.Tables("RFQHeader").Rows(0)("QuoStorageLocation").ToString.Substring(0, 1) + "K00"
        Else
            parameter(4) = ""
        End If
        'Purchase Org. for receving of STO
        Dim EnqInfo As DataTable = GetDataTable("select * from PurchasingUser where  UserID='" + DS.Tables("RFQHeader").Rows(0)("EnqUserID").ToString + "'", "PurchasingUser")
        'If EnqInfo.Rows.Count > 0 Then
        '    If EnqInfo.Rows(0)("R3PurchasingGroup").ToString.Length > 0 Then
        '        parameter(5) = EnqInfo.Rows(0)("R3PurchasingGroup").ToString.Substring(0, 1) + "K00"
        '    Else
        '        parameter(5) = ""
        '    End If
        'Else
        '    parameter(5) = ""
        'End If
        '计算逻辑错误导致的修改-0520
        If DS.Tables("RFQHeader").Rows(0)("EnqStorageLocation").ToString.Length > 0 Then
            parameter(5) = DS.Tables("RFQHeader").Rows(0)("EnqStorageLocation").ToString.Substring(0, 1) + "K00"
        Else
            parameter(5) = ""
        End If
        'Material number (PDB Product name)
        parameter(6) = CutShort(DS.Tables("RFQHeader").Rows(0)("ProductNumber").ToString) & Me.CodeExtensionList.SelectedValue
        'Vendor (PDB Supplier Name)
        Dim S4SupplierCode As DataTable = GetDataTable("select * from Supplier where  SupplierCode='" + DS.Tables("RFQHeader").Rows(0)("SupplierCode").ToString + "'", "Supplier")
        If S4SupplierCode.Rows.Count > 0 Then
            parameter(7) = Common.SafeSqlLiteral(S4SupplierCode.Rows(0)("S4SupplierCode").ToString())
        Else
            parameter(7) = ""
        End If
        'Price
        parameter(8) = SetNullORDecimal(DS2.Tables("RFQLine").Rows(0).Item("UnitPrice").ToString)
        'Price Unit (PDB Quo-Unit)
        parameter(9) = DS2.Tables("RFQLine").Rows(0).Item("QuoUnitCode").ToString
        'Order Price Unit
        parameter(10) = SetNullORDecimal(DS2.Tables("RFQLine").Rows(0).Item("QuoPer").ToString)
        'Currency
        parameter(11) = DS2.Tables("RFQLine").Rows(0).Item("CurrencyCode").ToString
        'RFQ Reference Number (in PDB)
        parameter(12) = RFQNumber
        'Supplier Contact Person Code
        parameter(13) = Common.SafeSqlLiteral(DS.Tables("RFQHeader").Rows(0)("SupplierContactPersonSel").ToString)
        'Maker Code
        'parameter(14) = DS.Tables("RFQHeader").Rows(0)("MakerCode").ToString
        parameter(14) = DS.Tables("RFQHeader").Rows(0)("SAPMakerCode").ToString
        'Supplier Item Name
        ' 20200402 WYS 增加转义替换单引号 start
        parameter(15) = Common.SafeSqlLiteral(DS.Tables("RFQHeader").Rows(0)("SupplierItemName").ToString)
        ' 20200402 WYS 增加转义替换单引号 end
        'Payment Terms
        Dim PaymentTermInfo As DataTable = GetDataTable("select * from PurchasingPaymentTerm where  PaymentTermCode='" + DS.Tables("RFQHeader").Rows(0)("PaymentTermCode").ToString + "'", "PurchasingPaymentTerm")
        If PaymentTermInfo.Rows.Count > 0 Then
            parameter(16) = Common.SafeSqlLiteral(PaymentTermInfo.Rows(0)("Text").ToString())
        Else
            parameter(16) = ""
        End If
        'Handling fee(currency)
        parameter(17) = DS.Tables("RFQHeader").Rows(0)("ShippingHandlingCurrencyCode").ToString.Replace("'", "''")
        'Shipment cost
        parameter(18) = SetNullORDecimal(DS.Tables("RFQHeader").Rows(0)("ShippingHandlingFee").ToString)
        'Purpose
        parameter(19) = DS.Tables("RFQHeader").Rows(0)("PurposeCode").ToString
        'Priority
        parameter(20) = DS.Tables("RFQHeader").Rows(0)("Priority").ToString
        'Enq User (Requester)
        If EnqInfo.Rows.Count > 0 Then
            parameter(21) = EnqInfo.Rows(0)("R3PurchasingGroup").ToString
        Else
            parameter(21) = ""
        End If
        'Quo-user(Purchaser)
        If QuoInfo.Rows.Count > 0 Then
            parameter(22) = QuoInfo.Rows(0)("R3PurchasingGroup").ToString
        Else
            parameter(22) = ""
        End If
        'Enq-Quantity
        parameter(23) = DS2.Tables("RFQLine").Rows(0).Item("EnqQuantity") * DS2.Tables("RFQLine").Rows(0).Item("EnqPiece")
        'Lead time
        parameter(24) = DS2.Tables("RFQLine").Rows(0).Item("LeadTime").ToString
        'Supplier Item Number
        parameter(25) = DS2.Tables("RFQLine").Rows(0).Item("SupplierItemNumber").ToString
        'Incoterms
        parameter(26) = DS2.Tables("RFQLine").Rows(0).Item("IncotermsCode").ToString
        'Terms of delivery
        parameter(27) = DS2.Tables("RFQLine").Rows(0).Item("DeliveryTerm").ToString
        'Purity & Method
        parameter(28) = DS2.Tables("RFQLine").Rows(0).Item("Purity").ToString
        'Packing
        parameter(29) = DS2.Tables("RFQLine").Rows(0).Item("Packing").ToString
        'Supplying Plant ' s offer vaild date from (not interfaced data)
        parameter(30) = ""
        'Supplying Plant' s offer vaild date from (not interfaced data)
        parameter(31) = ""
        'Supplying Plant' s Reminding 1
        'Supplying Plant' s Reminding 2
        'Supplying Plant' s Reminding 3

        Dim POReminderInfo As DataTable = GetDataTable("select * from Reminder where  SupplyingPlant='" + QuoPlant + "'", "Reminder")
        If POReminderInfo.Rows.Count > 0 Then
            Dim POReminderFirstRem As String = POReminderInfo.Rows(0)("FirstRem").ToString()
            Dim POReminderSecondRem As String = POReminderInfo.Rows(0)("SecondRem").ToString()
            Dim POReminderThirdRem As String = POReminderInfo.Rows(0)("ThirdRem").ToString()
            Dim POReminderShowType As Integer = POReminderInfo.Rows(0)("ShowType")

            If POReminderShowType = 1 Then
                If IsDBNull(DS2.Tables("RFQLine").Rows(0).Item("LeadTime")) Then
                    Msg.Text = "Lead time error, PO interface create failed!"
                    Return ""
                    Exit Function
                End If
                If IsNumeric(DS2.Tables("RFQLine").Rows(0).Item("LeadTime")) = False Then
                    Msg.Text = "Lead time error, PO interface create failed!"
                    Return ""
                    Exit Function
                End If
                If Convert.ToDouble(DS2.Tables("RFQLine").Rows(0).Item("LeadTime")) < 1 Then
                    Msg.Text = "Lead time error, PO interface create failed!"
                    Return ""
                    Exit Function
                End If
                parameter(32) = System.Math.Round(DS2.Tables("RFQLine").Rows(0).Item("LeadTime") * POReminderFirstRem, 0)
                parameter(33) = System.Math.Round(DS2.Tables("RFQLine").Rows(0).Item("LeadTime") * POReminderSecondRem, 0)
                parameter(34) = System.Math.Round(DS2.Tables("RFQLine").Rows(0).Item("LeadTime") * POReminderThirdRem, 0)
            Else
                parameter(32) = POReminderFirstRem
                parameter(33) = POReminderSecondRem
                parameter(34) = POReminderThirdRem
            End If
        Else
            parameter(32) = "0"
            parameter(33) = "0"
            parameter(34) = "0"
        End If
        'Receiving Plant's offer vaild date from (not interfaced data)
        parameter(35) = ""
        'Receiving Plant's offer vaild date  to (not interfaced data)
        parameter(36) = ""
        'Supplying Storage location(PDB Quo-USER storage location)
        parameter(37) = DS.Tables("RFQHeader").Rows(0)("QuoStorageLocation").ToString()
        'Receiving Storage location(PDB Enq-USER storage location)
        parameter(38) = DS.Tables("RFQHeader").Rows(0)("EnqStorageLocation").ToString()
        'Supplier Offer No.
        parameter(39) = DS2.Tables("RFQLine").Rows(0).Item("SupplierOfferNo").ToString()

        ' 20200701 WYS The material not yet created in S4. PO interface creation failed! start
        If labBUoM.Text.Trim.ToString().Equals("") Then
            Msg.Text = "The material not yet created in S4. PO interface creation failed!"
            Return ""
            Exit Function
        End If
        ' 20200701 WYS end

        ' 20200720 lxs start
        '20200727修改
        If StorageLocation2.SelectedItem.Text <> "" Then
            If Left(StorageLocation2.SelectedItem.Text, 1) = "H" And DS2.Tables("RFQLine").Rows(0).Item("CurrencyCode").ToString <> "CNY" Then
                Msg.Text = "The currency conflict with defalt. PO Interface creation failed!"
                Return ""
                Exit Function
            End If
        End If
        If StorageLocation2.SelectedItem.Text <> "" Then
            If Left(StorageLocation2.SelectedItem.Text, 1) = "N" And DS2.Tables("RFQLine").Rows(0).Item("CurrencyCode").ToString <> "INR" Then
                Msg.Text = "The currency conflict with defalt. PO Interface creation failed!"
                Return ""
                Exit Function
            End If
        End If

        ' 20200720 lxs end
        '20200727 start 如果触发的采购订单接口数据的价格为“0” 
        If SetNullORDecimal(DS2.Tables("RFQLine").Rows(0).Item("UnitPrice").ToString) = SetNullORDecimal(0) Then
            Msg.Text = "PDB Interface can not create free charge PO, please make it manually. PO interface creation failed!"
            Return ""
            Exit Function
        End If
        ' 20200727 lxs end
        ' 20200630 WYS 增加warring：Please review the Unit of Enq-Quantity for PO Interface. PO interface creation failed!  start
        If parameter(9).Equals("ST") Then
            If labBUoM.Text.Trim.ToString() <> "EA" Then
                Msg.Text = "Please review the Unit of Enq-Quantity for PO Interface. PO interface creation failed!"
                Return ""
                Exit Function
            End If
        End If
        ' 20200630 WYS end

        ' 20200402 WYS 增加对LeadTime是否为数值的判断 start
        If IsNumeric(DS2.Tables("RFQLine").Rows(0).Item("LeadTime")) Then

        Else
            Msg.Text = "Lead time should be integer, PO interface output failed!"
            Return ""
            Exit Function
        End If
        ' 20200402 WYS 增加对LeadTime是否为数值的判断 end

        '20191012 WYS 追加SupplierOfferValidTo
        Dim _date As DateTime
        If txtVaildTo.Text = "" Then
            If DateTime.Parse("9999-12-12") < DateTime.Now Then
                Msg.Text = "Please recheck the valid date with supplier at first. PO interface create failed!"
                Return ""
                Exit Function
            End If
        Else
            If DateTime.TryParse(txtVaildTo.Text, _date) Then
                If _date < DateTime.Now Then
                    Msg.Text = "Please recheck the valid date with supplier at first. PO interface create failed!"
                    Return ""
                    Exit Function
                End If
            Else
                Msg.Text = "Supplier offer valid to Incorrect format!"
                Return ""
                Exit Function
            End If
        End If

        If StorageLocation.Items.Count = 0 OrElse StorageLocation2.Items.Count = 0 Then
            ' EnqStorageLocation または QuoStorageLocation の選択肢が存在しない場合はエラーにする。
            ' 選択肢が存在し、空文字列（String.Empty）を選択している場合はこのエラーの対象外
            Msg.Text = "Please review the Enq-user's and  Quo-user's storage location. PO interface creation failed!"
            Return ""
            Exit Function
        End If
        '20200617 WYS Please review the Enq-user's and  Quo-user's storage location. PO interface creation failed! start
        If StorageLocation.SelectedValue <> StorageLocation2.SelectedValue Then
            If Left(StorageLocation.SelectedValue, 1) = Left(StorageLocation2.SelectedValue, 1) Then
                If Left(StorageLocation.SelectedValue, 1) <> "H" And Left(StorageLocation.SelectedValue, 1) <> "N" Then
                    Msg.Text = "Please review the Enq-user's and  Quo-user's storage location. PO interface creation failed!"
                    Return ""
                    Exit Function
                End If

            End If
        End If


        '20200617 WYS end

        '判断当前数据是否合法是否需要提醒

        If CheckIsClickPoInterface(st_RFQNumber) = False Then
            Msg.Text = "Purpose Not exist!"
            Return ""
            Exit Function
        End If
        If DS.Tables("RFQHeader").Rows(0)("StatusCode").ToString <> "Q" Then
            Msg.Text = "Please quote And update RFQ first! PO Interface create failed!"
            Return ""
            Exit Function
        End If
        If DS.Tables("RFQHeader").Rows(0)("S4SupplierCode").ToString = "" Then
            Msg.Text = "SAP Supplier code Is blank! PO Interface create failed!"
            Return ""
            Exit Function
        End If

        ' 20200609 WYS If "Quo-user storage location's Plant number= "SAP supplier code" or both first letter of "Quo-user storage location's Plant number and first letter of  "SAP supplier code"are "A" or "C"  start
        If R3SupplierCode.Text <> "" And StorageLocation2.SelectedItem.ToString() <> "" Then
            If R3SupplierCode.Text.Equals(QuoPlant) Then
                Msg.Text = "Please review the Quo-user's storage location and SAP Supplier code. PO interface creation failed!"
                Return ""
                Exit Function
            End If
            Dim R3F As String = R3SupplierCode.Text.ToString().Substring(0, 1)
            Dim quoF As String = QuoPlant.Substring(0, 1)
            If R3F = quoF Then
                If R3F = "A" Or R3F = "C" Then
                    Msg.Text = "Please review the Quo-user's storage location and SAP Supplier code. PO interface creation failed!"
                    Return ""
                    Exit Function
                End If
            End If
        End If
        ' 20200609 WYS end

        '临时测试用--功能是选中的EnqUserID与登录用户UserID不一致，用户不能进行更新数据操作
        If DS.Tables("RFQHeader").Rows(0)("EnqUserID").ToString <> Session("UserID").ToString Then
            Msg.Text = "You are not authorized to issue this PO interface!"
            Return ""
            Exit Function
        End If

        If DS.Tables("RFQHeader").Rows(0)("MakerCode").ToString <> "" Then
            If DS.Tables("RFQHeader").Rows(0)("SAPMakerCode").ToString = "" Then
                Msg.Text = "Please make sure SAP Maker Code already been created! PO interface create failed!"
                Return ""
                Exit Function
            End If
        End If
        If parameter(9).ToString = "" Then
            Msg.Text = "Quo-Unit is blank!PO interface create failed!"
            Return ""
            Exit Function
        End If

        If parameter(10).ToString = "" Then
            Msg.Text = "Quo-Per is blank!PO interface create failed!"
            Return ""
            Exit Function
        End If
        If parameter(8).ToString = "" Then
            Msg.Text = "Price is blank!PO interface create failed!"
            Return ""
            Exit Function
        End If
        If parameter(11).ToString = "" Then
            Msg.Text = "Currency is blank!PO interface create failed!"
            Return ""
            Exit Function
        End If
        If parameter(18).ToString <> "" Then
            If parameter(17).ToString = "" Then
                Msg.Text = "Handling fee(Currency) is blank!PO interface create failed!"
                Return ""
                Exit Function
            End If
        End If
        If DS.Tables("RFQHeader").Rows(0)("EnqUserID").ToString = "" Then
            Msg.Text = "Enq-User is blank!"
            Return ""
            Exit Function
        End If
        If DS.Tables("RFQHeader").Rows(0)("QuoUserID").ToString = "" Then
            Msg.Text = "Quo-user  is blank!"
            Return ""
            Exit Function
        End If
        If parameter(23).ToString = "" Then
            Msg.Text = "Enq-Quantity is blank!PO interface create failed!"
            Return ""
            Exit Function
        End If
        If parameter(24).ToString = "" Then
            Msg.Text = "Lead time is blank!"
            Return ""
            Exit Function
        End If
        If DS.Tables("RFQHeader").Rows(0)("QuoStorageLocation").ToString = "" Then
            If StorageLocation2.SelectedValue <> "" Then
                Msg.Text = "please update RFQ first"
                Return ""
                Exit Function
            End If
            Msg.Text = "Quo-User storage location is blank!"
            Return ""
            Exit Function
        End If

        ' 20200513 wys 追加判断StorageLocation2选中的值首字母为“H”，Lead time的输入值如果<3 (也就是等于1 or 2)时，给出报警“Lead time must >3, PO interface output failed! ”,退出PO interface    begin
        If StorageLocation2.SelectedValue <> "" Then
            If StorageLocation2.SelectedValue.Substring(0, 1).Equals("H") Then
                If Convert.ToInt32(parameter(24).ToString) < 3 Then
                    Msg.Text = "Lead time must >= 3, PO interface output failed! "
                    Return ""
                    Exit Function
                End If
            End If
        End If
        ' 20200513 wys end

        If DS.Tables("RFQHeader").Rows(0)("EnqStorageLocation").ToString = "" Then
            If StorageLocation.SelectedValue <> "" Then
                Msg.Text = "please update RFQ first"
                Return ""
                Exit Function
            End If
            Msg.Text = "Enq-User storage location is blank!"
            Return ""
            Exit Function
        End If

        If parameter(10) <> String.Empty Then
            If isNum(parameter(10)) Then
                Msg.Text = "Please set the Quo-per as integer. PO interface create failed! "
                Return ""
                Exit Function
            End If
        End If


        Dim DataTable As System.Data.DataTable = Purchase.Common.GetDataTable("select * from  POInterface where RFQLineNumber=" + RFQLineNumber, "POInterface")
        Dim sql As String = String.Empty

        Dim dt As System.Data.DataTable = Purchase.Common.GetDataTable("select * from  POInterface ", "POInterface")
        sql += "Delete POInterface;"
        Dim MaxId As Integer = 0
        If dt.Rows.Count > 0 Then
            For i = 0 To dt.Rows.Count - 1
                If dt.Rows(i)("RFQNumber").ToString().Equals(RFQNumber) Then
                Else
                    MaxId += 1
                    sql += "INSERT INTO POInterface "
                    sql += "(Id,RFQLineNumber,RFQNumber"
                    sql += ",Pattern"
                    sql += ",SupplyingPlant"
                    sql += ",ReceivingPlant"
                    sql += ",PurOrgShipping"
                    sql += ",PurOrgReceving"
                    sql += ",MaterialNumber"
                    sql += ",Vendor"
                    sql += ",Price"
                    sql += ",PriceUnit"
                    sql += ",OrderPriceUnit"
                    sql += ",Currency"
                    sql += ",RFQReferenceNumber"
                    sql += ",SupplierContactPersonCode"
                    sql += ",MakerCode"
                    sql += ",SupplierItemName"
                    sql += ",PaymentTerms"
                    sql += ",HandlingFee"
                    sql += ",ShipmentCost"
                    sql += ",Purpose"
                    sql += ",Priority"
                    sql += ",EnqUser"
                    sql += ",QuoUser"
                    sql += ",EnqQuantity"
                    sql += ",LeadTime"
                    sql += ",SupplierItemNumber"
                    sql += ",Incoterms"
                    sql += ",TermsDelivery"
                    sql += ",PurityMethod"
                    sql += ",Packing"
                    sql += ",SupplyingOfferVaildDateFrom"
                    sql += ",SupplyingOfferVaildDateTo"
                    sql += ",SupplyingPlantReminding1"
                    sql += ",SupplyingPlantReminding2"
                    sql += ",SupplyingPlantReminding3"
                    sql += ",ReceivingOfferVaildDateFrom"
                    sql += ",ReceivingOfferVaildDateTo"
                    sql += ",SupplyingStorageLocation"
                    sql += ",ReceivingStorageLocation"
                    sql += ",SupplierOfferNo"

                    sql += ")"
                    sql += " VALUES(" + MaxId.ToString
                    sql += "," + dt.Rows(i)("RFQLineNumber").ToString + ""
                    sql += "," + dt.Rows(i)("RFQNumber").ToString + ""
                    sql += ",'" + dt.Rows(i)("Pattern").ToString + "'"
                    sql += ",'" + dt.Rows(i)("SupplyingPlant").ToString + "'"
                    sql += ",'" + dt.Rows(i)("ReceivingPlant").ToString + "'"
                    sql += ",'" + dt.Rows(i)("PurOrgShipping").ToString + "'"
                    sql += ",'" + dt.Rows(i)("PurOrgReceving").ToString + "'"
                    sql += ",'" + dt.Rows(i)("MaterialNumber").ToString + "'"
                    sql += ",'" + dt.Rows(i)("Vendor").ToString + "'"
                    sql += ",'" + dt.Rows(i)("Price").ToString + "'"
                    sql += ",'" + dt.Rows(i)("PriceUnit").ToString + "'"
                    sql += ",'" + dt.Rows(i)("OrderPriceUnit").ToString + "'"
                    sql += ",'" + dt.Rows(i)("Currency").ToString + "'"
                    sql += ",'" + dt.Rows(i)("RFQReferenceNumber").ToString + "'"
                    sql += ",'" + dt.Rows(i)("SupplierContactPersonCode").ToString + "'"
                    sql += ",'" + dt.Rows(i)("MakerCode").ToString + "'"
                    sql += ",'" + dt.Rows(i)("SupplierItemName").ToString + "'"
                    sql += ",'" + dt.Rows(i)("PaymentTerms").ToString + "'"
                    sql += ",'" + dt.Rows(i)("HandlingFee").ToString + "'"
                    sql += ",'" + dt.Rows(i)("ShipmentCost").ToString + "'"
                    sql += ",'" + dt.Rows(i)("Purpose").ToString + "'"
                    sql += ",'" + dt.Rows(i)("Priority").ToString + "'"
                    sql += ",'" + dt.Rows(i)("EnqUser").ToString + "'"
                    sql += ",'" + dt.Rows(i)("QuoUser").ToString + "'"
                    sql += ",'" + dt.Rows(i)("EnqQuantity").ToString + "'"
                    sql += ",'" + dt.Rows(i)("LeadTime").ToString + "'"
                    sql += ",'" + dt.Rows(i)("SupplierItemNumber").ToString + "'"
                    sql += ",'" + dt.Rows(i)("Incoterms").ToString + "'"
                    sql += ",'" + dt.Rows(i)("TermsDelivery").ToString + "'"
                    sql += ",'" + dt.Rows(i)("PurityMethod").ToString + "'"
                    sql += ",'" + dt.Rows(i)("Packing").ToString + "'"
                    sql += ",'" + dt.Rows(i)("SupplyingOfferVaildDateFrom").ToString + "'"
                    sql += ",'" + dt.Rows(i)("SupplyingOfferVaildDateTo").ToString + "'"
                    sql += ",'" + dt.Rows(i)("SupplyingPlantReminding1").ToString + "'"
                    sql += ",'" + dt.Rows(i)("SupplyingPlantReminding2").ToString + "'"
                    sql += ",'" + dt.Rows(i)("SupplyingPlantReminding3").ToString + "'"
                    sql += ",'" + dt.Rows(i)("ReceivingOfferVaildDateFrom").ToString + "'"
                    sql += ",'" + dt.Rows(i)("ReceivingOfferVaildDateTo").ToString + "'"
                    sql += ",'" + dt.Rows(i)("SupplyingStorageLocation").ToString + "'"
                    sql += ",'" + dt.Rows(i)("ReceivingStorageLocation").ToString + "'"
                    sql += ",'" + dt.Rows(i)("SupplierOfferNo").ToString + "'"
                    sql += ");"
                End If
            Next
        End If

        MaxId += 1
        sql += "INSERT INTO POInterface "
        sql += "(Id,RFQLineNumber,RFQNumber"
        'For i = 1 To 39
        '    sql += ",Field" + Trim(i) + ""
        'Next
        sql += ",Pattern"
        sql += ",SupplyingPlant"
        sql += ",ReceivingPlant"
        sql += ",PurOrgShipping"
        sql += ",PurOrgReceving"
        sql += ",MaterialNumber"
        sql += ",Vendor"
        sql += ",Price"
        sql += ",PriceUnit"
        sql += ",OrderPriceUnit"
        sql += ",Currency"
        sql += ",RFQReferenceNumber"
        sql += ",SupplierContactPersonCode"
        sql += ",MakerCode"
        sql += ",SupplierItemName"
        sql += ",PaymentTerms"
        sql += ",HandlingFee"
        sql += ",ShipmentCost"
        sql += ",Purpose"
        sql += ",Priority"
        sql += ",EnqUser"
        sql += ",QuoUser"
        sql += ",EnqQuantity"
        sql += ",LeadTime"
        sql += ",SupplierItemNumber"
        sql += ",Incoterms"
        sql += ",TermsDelivery"
        sql += ",PurityMethod"
        sql += ",Packing"
        sql += ",SupplyingOfferVaildDateFrom"
        sql += ",SupplyingOfferVaildDateTo"
        sql += ",SupplyingPlantReminding1"
        sql += ",SupplyingPlantReminding2"
        sql += ",SupplyingPlantReminding3"
        sql += ",ReceivingOfferVaildDateFrom"
        sql += ",ReceivingOfferVaildDateTo"
        sql += ",SupplyingStorageLocation"
        sql += ",ReceivingStorageLocation"
        sql += ",SupplierOfferNo"

        sql += ")"
        sql += " VALUES(" + MaxId.ToString + "," + RFQLineNumber + "," + RFQNumber + ""
        For i = 1 To 39
            sql += ",'" + parameter(i) + "'"
        Next
        sql += ");"

        DBConn.Open()
        DBCommand = DBConn.CreateCommand()
        DBCommand.CommandText = sql
        DBCommand.ExecuteNonQuery()

        '更新RfqLine的OutputStatus
        DBCommand.CommandText = "
            Update RfqLine   set OutputStatus='1' where RFQLineNumber=@RFQLineNumber;
            Update RFQHeader set RFQStatusCode='II', CodeExtensionCode=@CodeExtensionCode, MMSTAInvalidation=@MMSTAInvalidation where RFQNumber=@RFQNumber;
            Update RFQHeader Set MMSTAInvalidation=0 where RFQNumber=@RFQNumber and MMSTAInvalidation=1;"
        DBCommand.Parameters.Clear()
        DBCommand.Parameters.AddWithValue("RFQLineNumber", RFQLineNumber)
        DBCommand.Parameters.AddWithValue("RFQNumber", RFQNumber)
        DBCommand.Parameters.AddWithValue("CodeExtensionCode", Me.CodeExtensionList.SelectedValue)
        DBCommand.Parameters.AddWithValue("MMSTAInvalidation", If(Me.MMSTAInvalidation.Checked, 1, 0))

        DBCommand.ExecuteNonQuery()

        DBCommand.Dispose()
        DBConn.Close()
        If MMSTAInvalidation.Checked = True Then
            MMSTAInvalidation.Checked = False
        End If
        Return DS2.Tables("RFQLine").Rows(0).Item("QuoUnitCode").ToString
    End Function
    Public Function CheckIsClickPoInterface(ByVal RFQNumber As String) As Boolean
        Dim isAbleClickPoInterface As Boolean = False
        Dim purposeDt As DataTable = GetDataTable("Select *  from Purpose where PurposeCode In (Select PurposeCode from  RFQHeader where RFQNumber=" + RFQNumber + ") And IsVisiable ='1'", "Purpose")
        If purposeDt.Rows.Count > 0 Then
            isAbleClickPoInterface = True
        End If
        Return isAbleClickPoInterface
    End Function

    Protected Sub EnqUnit_1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles EnqUnit_1.SelectedIndexChanged
        QuoUnit(1).SelectedValue = EnqUnit_1.SelectedValue
    End Sub

    Protected Sub EnqUnit_2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles EnqUnit_2.SelectedIndexChanged
        QuoUnit(2).SelectedValue = EnqUnit_2.SelectedValue
    End Sub

    Protected Sub EnqUnit_3_SelectedIndexChanged(sender As Object, e As EventArgs) Handles EnqUnit_3.SelectedIndexChanged
        QuoUnit(3).SelectedValue = EnqUnit_3.SelectedValue
    End Sub

    Protected Sub EnqUnit_4_SelectedIndexChanged(sender As Object, e As EventArgs) Handles EnqUnit_4.SelectedIndexChanged
        QuoUnit(4).SelectedValue = EnqUnit_4.SelectedValue
    End Sub

    Protected Sub QuoLocation_SelectedIndexChanged(sender As Object, e As EventArgs) Handles QuoLocation.SelectedIndexChanged
        If QuoLocation.CssClass = "readonly" Then
            QuoLocation.SelectedValue = QuoLocationCode.Value

            Dim sql As String = ""
            Dim dt As DataTable
            If String.IsNullOrEmpty(Confidential.Text) Then
                sql = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') ORDER BY [Name] " _
                                             , QuoLocation.SelectedValue)
            Else
                sql = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 AND RoleCode IN ('WRITE', 'WRITE_AA') and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') ORDER BY [Name] " _
                                                 , QuoLocation.SelectedValue)
            End If
            dt = GetDataTable(sql)
            QuoUser.Items.Clear()
            'QuoUser.Items.Add(String.Empty)
            For i As Integer = 0 To dt.Rows.Count - 1
                QuoUser.Items.Add(New ListItem(dt.Rows(i)("Name").ToString, dt.Rows(i)("UserID").ToString))
            Next
            If dt.Rows.Count > 0 Then
                Dim tmpdt As DataTable
                tmpdt = GetDataTable("select * from StorageLocation where Storage in(select Storage from StorageByPurchasingUser where UserId=" + dt.Rows(0)("UserID").ToString + ")")
                StorageLocation2.Items.Clear()
                StorageLocation2.Items.Add(String.Empty)
                For i As Integer = 0 To tmpdt.Rows.Count - 1
                    StorageLocation2.Items.Add(New ListItem(tmpdt.Rows(i)("Storage").ToString, tmpdt.Rows(i)("Storage").ToString))
                Next
            End If
        Else
            Msg.Text = String.Empty
            'DBCommand = DBConn.CreateCommand()
            'If String.IsNullOrEmpty(Confidential.Text) Then
            '    DBCommand.CommandText = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') ORDER BY [Name] " _
            '                                 , QuoLocation.SelectedValue)
            'Else
            '    DBCommand.CommandText = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 AND RoleCode = 'WRITE' and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') ORDER BY [Name] " _
            '                                     , QuoLocation.SelectedValue)
            'End If
            'Dim DBReader As System.Data.SqlClient.SqlDataReader
            'DBReader = DBCommand.ExecuteReader()
            'DBCommand.Dispose()
            'QuoUser.Items.Clear()
            'Do Until DBReader.Read = False
            '    QuoUser.Items.Add(New ListItem(DBReader("Name").ToString, DBReader("UserID").ToString))
            'Loop
            'DBReader.Close()

            Dim sql As String = ""
            Dim dt As DataTable
            If String.IsNullOrEmpty(Confidential.Text) Then
                sql = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') ORDER BY [Name] " _
                                             , QuoLocation.SelectedValue)
            Else
                sql = String.Format("SELECT UserID, [Name] FROM v_UserAll WHERE (LocationCode = '{0}' AND isDisabled = 0 AND RoleCode IN ('WRITE', 'WRITE_AA') and  R3PurchasingGroup  is not null and R3PurchasingGroup <>'') ORDER BY [Name] " _
                                                 , QuoLocation.SelectedValue)
            End If
            dt = GetDataTable(sql)
            QuoUser.Items.Clear()
            'QuoUser.Items.Add(String.Empty)
            For i As Integer = 0 To dt.Rows.Count - 1
                QuoUser.Items.Add(New ListItem(dt.Rows(i)("Name").ToString, dt.Rows(i)("UserID").ToString))
            Next
            If dt.Rows.Count > 0 Then
                Dim tmpdt As DataTable
                tmpdt = GetDataTable("select * from StorageLocation where Storage in(select Storage from StorageByPurchasingUser where UserId=" + dt.Rows(0)("UserID").ToString + ")")
                StorageLocation2.Items.Clear()
                StorageLocation2.Items.Add(String.Empty)
                For i As Integer = 0 To tmpdt.Rows.Count - 1
                    StorageLocation2.Items.Add(New ListItem(tmpdt.Rows(i)("Storage").ToString, tmpdt.Rows(i)("Storage").ToString))
                Next
            End If

        End If
    End Sub

    Protected Sub EnqUser_SelectedIndexChanged(sender As Object, e As EventArgs) Handles EnqUser.SelectedIndexChanged
        Msg.Text = String.Empty
        DBCommand = DBConn.CreateCommand()
        DBCommand.CommandText = "select * from StorageLocation where Storage in(select Storage from StorageByPurchasingUser where UserId=" + EnqUser.SelectedValue + ")"
        Dim DBReader As System.Data.SqlClient.SqlDataReader
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        StorageLocation.Items.Clear()
        StorageLocation.Items.Add(String.Empty)
        Do Until DBReader.Read = False
            StorageLocation.Items.Add(New ListItem(DBReader("Storage").ToString, DBReader("Storage").ToString))
        Loop
        DBReader.Close()
    End Sub

    Protected Sub QuoUser_SelectedIndexChanged(sender As Object, e As EventArgs) Handles QuoUser.SelectedIndexChanged
        Msg.Text = String.Empty
        DBCommand = DBConn.CreateCommand()
        Dim sql As String = "1=2"
        If QuoUser.SelectedValue <> "" Then
            sql = "UserId=" + QuoUser.SelectedValue
        End If
        DBCommand.CommandText = "select * from StorageLocation where Storage in(select Storage from StorageByPurchasingUser where " + sql + ")"
        Dim DBReader As System.Data.SqlClient.SqlDataReader
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        StorageLocation2.Items.Clear()
        StorageLocation2.Items.Add(String.Empty)
        Do Until DBReader.Read = False
            StorageLocation2.Items.Add(New ListItem(DBReader("Storage").ToString, DBReader("Storage").ToString))
        Loop
        DBReader.Close()
    End Sub

    Protected Sub SupplierCode_TextChanged(sender As Object, e As EventArgs) Handles SupplierCode.TextChanged
        ' 20200402 追加SupplierCode.text为空判断 start
        SetSupplierContactPersonCodeList()
        If SupplierContactPersonCodeList.SelectedValue.Length > 0 AndAlso SupplierContactPersonCodeList.SelectedValue.IndexOf("-") > -1 Then
            SupplierContactPerson.Text = SupplierContactPersonCodeList.SelectedValue.Split("-")(0).ToString
        End If
        ' 20200402 追加SupplierCode.text为空判断 end
    End Sub

    Private Sub SetSupplierContactPersonCodeList()
        Dim sql As String
        If SupplierCode.Text.Trim().Length > 0 Then
            'If IsNumeric(SupplierCode.Text) Then

            'Else
            '    Msg.Text = "Supplier Code is invalid number!"
            '    SupplierCode.Focus()
            'End If

            sql = "select * from ("
            sql += "select '' as supplierInfo,'' as SupplierContactperson,'' as SupplierEmailID FROM  Supplier where SupplierCode=" + SupplierCode.Text + ""
            sql += " Union All "
            sql += "select (SupplierEmailID1+'-'+ ISNULL(SupplierContactperson1,'')) as supplierInfo,ISNULL(SupplierContactperson1,'') as SupplierContactperson,SupplierEmailID1 as SupplierEmailID FROM  Supplier where SupplierCode=" + SupplierCode.Text + " and SupplierEmailID1 <>'' and SupplierEmailID1 is not null and SupplierEmailID1 <>'000'"
            sql += " Union All "
            sql += "select (SupplierEmailID2+'-'+ ISNULL(SupplierContactperson2,'')) as supplierInfo,ISNULL(SupplierContactperson2,'') as SupplierContactperson,SupplierEmailID2 as SupplierEmailID FROM  Supplier where SupplierCode=" + SupplierCode.Text + " and SupplierEmailID2 <>'' and SupplierEmailID2 is not null and SupplierEmailID2 <>'000'"
            sql += " Union All "
            sql += "select (SupplierEmailID3+'-'+ ISNULL(SupplierContactperson3,'')) as supplierInfo,ISNULL(SupplierContactperson3,'') as SupplierContactperson,SupplierEmailID3 as SupplierEmailID FROM  Supplier where SupplierCode=" + SupplierCode.Text + " and SupplierEmailID3 <>'' and SupplierEmailID3 is not null and SupplierEmailID3 <>'000'"
            sql += " Union All "
            sql += "select (SupplierEmailID4+'-'+ ISNULL(SupplierContactperson4,'')) as supplierInfo,ISNULL(SupplierContactperson4,'') as SupplierContactperson,SupplierEmailID4 as SupplierEmailID FROM  Supplier where SupplierCode=" + SupplierCode.Text + " and SupplierEmailID4 <>'' and SupplierEmailID4 is not null and SupplierEmailID4 <>'000'"
            sql += " Union All "
            sql += "select (SupplierEmailID5+'-'+ ISNULL(SupplierContactperson5,'')) as supplierInfo,ISNULL(SupplierContactperson5,'') as SupplierContactperson,SupplierEmailID5 as SupplierEmailID FROM  Supplier where SupplierCode=" + SupplierCode.Text + " and SupplierEmailID5 <>'' and SupplierEmailID5 is not null and SupplierEmailID5 <>'000'"
            sql += " Union All "
            sql += "select (SupplierEmailID6+'-'+ ISNULL(SupplierContactperson6,'')) as supplierInfo,ISNULL(SupplierContactperson6,'') as SupplierContactperson,SupplierEmailID6 as SupplierEmailID FROM  Supplier where SupplierCode=" + SupplierCode.Text + " and SupplierEmailID6 <>'' and SupplierEmailID6 is not null and SupplierEmailID6 <>'000'"
            sql += " Union All "
            sql += "select (SupplierEmailID7+'-'+ ISNULL(SupplierContactperson7,'')) as supplierInfo,ISNULL(SupplierContactperson7,'') as SupplierContactperson,SupplierEmailID7 as SupplierEmailID FROM  Supplier where SupplierCode=" + SupplierCode.Text + " and SupplierEmailID7 <>'' and SupplierEmailID7 is not null and SupplierEmailID7 <>'000'"
            sql += " Union All "
            sql += "select (SupplierEmailID8+'-'+ ISNULL(SupplierContactperson8,'')) as supplierInfo,ISNULL(SupplierContactperson8,'') as SupplierContactperson,SupplierEmailID9 as SupplierEmailID FROM  Supplier where SupplierCode=" + SupplierCode.Text + " and SupplierEmailID8 <>'' and SupplierEmailID8 is not null and SupplierEmailID8 <>'000'"
            sql += " Union All "
            sql += "select (SupplierEmailID9+'-'+ ISNULL(SupplierContactperson9,'')) as supplierInfo,ISNULL(SupplierContactperson9,'') as SupplierContactperson,SupplierEmailID9 as SupplierEmailID FROM  Supplier where SupplierCode=" + SupplierCode.Text + " and SupplierEmailID9 <>'' and SupplierEmailID9 is not null and SupplierEmailID9 <>'000'"
            sql += " Union All "
            sql += "select (SupplierEmailID10+'-'+ ISNULL(SupplierContactperson10,'')) as supplierInfo,ISNULL(SupplierContactperson10,'') as SupplierContactperson,SupplierEmailID10 as SupplierEmailID FROM  Supplier where SupplierCode=" + SupplierCode.Text + " and SupplierEmailID10 <>'' and SupplierEmailID10 is not null and SupplierEmailID10 <>'000'"
            sql += ") as A where supplierInfo is not null"
            SDS_SupplierContactPersonCodeList.SelectCommand = sql
            SupplierContactPersonCodeList.DataBind()

        End If
    End Sub


    Public Function RFQCheck(ByVal RFQNumber As String, ByVal i As String) As Boolean
        Dim Ret As Boolean = True
        Dim SQLLineUpdate As String = String.Empty
        Msg.Text = String.Empty
        If Comment.Text.Length > INT_3000 Then
            Msg.Text = ERR_COMMENT_OVER
            Ret = False
            Exit Function
        End If
        If Specification.Text.Length > INT_255 Then
            Msg.Text = ERR_SPECIFICATION_OVER
            Ret = False
            Exit Function
        End If
        If CheckSupplierCode() = False Then
            Ret = False
            Exit Function
        End If
        If CheckLocation() = False Then
            Ret = False
            Exit Function
        End If
        If IsLatestData("RFQHeader", "RFQNumber", st_RFQNumber, UpdateDate.Value) = False Then
            Msg.Text = ERR_UPDATED_BY_ANOTHER_USER
            Ret = False
            Exit Function
        End If
        Dim sqlTran As System.Data.SqlClient.SqlTransaction = DBConn.BeginTransaction()
        DBCommand.Transaction = sqlTran
        Try
            DBCommand.Parameters.Clear()
            Dim st_Priority As String = String.Empty
            If (Priority.Visible) Then
                st_Priority = Priority.Text
            Else
                st_Priority = LabelPriority.Text
            End If
            Dim st_PurposeCode As String = String.Empty
            If (ListPurpose.Visible) Then
                st_PurposeCode = ListPurpose.SelectedValue
            Else
                st_PurposeCode = PurposeCode.Value
            End If
            Dim st_EnqLocationCode As String = String.Empty
            If EnqLocation.CssClass = "readonly" Then
                st_EnqLocationCode = EnqLocationCode.Value
            Else
                st_EnqLocationCode = EnqLocation.SelectedValue
            End If
            Dim st_QuoLocationCode As String = String.Empty
            If EnqLocation.CssClass = "readonly" Then
                st_QuoLocationCode = QuoLocationCode.Value
            Else
                st_QuoLocationCode = QuoLocation.SelectedValue
            End If
            Dim st_EnqStorageLocationCode As String = String.Empty
            If EnqLocation.CssClass = "readonly" Then
                'st_EnqStorageLocationCode = EnqStorageLOcationCode.Value
                st_EnqStorageLocationCode = StorageLocation.SelectedValue
            Else
                st_EnqStorageLocationCode = StorageLocation.SelectedValue
            End If
            Dim st_QuoStorageLocationCode As String = String.Empty
            If EnqLocation.CssClass = "readonly" Then
                'st_QuoStorageLocationCode = QuoStorageLOcationCode.Value
                st_QuoStorageLocationCode = StorageLocation2.SelectedValue
            Else
                st_QuoStorageLocationCode = StorageLocation2.SelectedValue
            End If
            DBCommand.CommandText = "Update RFQHeader SET EnqLocationCode = @EnqLocationCode,QuoLocationCode = @QuoLocationCode, EnqUserID = @EnqUserID, QuoUserID = @QuoUserID, SupplierCode = @SupplierCode, MakerCode = @MakerCode,SAPMakerCode = @SAPMakerCode," _
            & "SpecSheet = @SpecSheet, Specification = @Specification, SupplierContactPerson = @SupplierContactPerson," _
            & "SupplierItemName = @SupplierItemName, ShippingHandlingFee = @ShippingHandlingFee," _
            & "ShippingHandlingCurrencyCode = @ShippingHandlingCurrencyCode, PaymentTermCode = @PaymentTermCode," _
            & "Comment = @Comment, Priority = @Priority , PurposeCode = @PurposeCode , UpdatedBy = @UpdatedBy,EnqStorageLocation=@EnqStorageLocation,QuoStorageLocation=@QuoStorageLocation,SupplierContactPersonSel=@SupplierContactPersonSel, UpdateDate = GETDATE()" _
            & ",SupplierOfferValidTo = @SupplierOfferValidTo" _
            & ",CodeExtensionCode = @CodeExtensionCode" _
            & " Where RFQNumber = @RFQNumber "
            DBCommand.Parameters.Add("@EnqLocationCode", SqlDbType.VarChar).Value = st_EnqLocationCode
            DBCommand.Parameters.Add("@QuoLocationCode", SqlDbType.VarChar).Value = st_QuoLocationCode
            DBCommand.Parameters.Add("@EnqUserID", SqlDbType.Int).Value = ConvertStringToInt(EnqUser.SelectedValue)
            DBCommand.Parameters.Add("@QuoUserID", SqlDbType.Int).Value = ConvertStringToInt(QuoUser.SelectedValue)
            DBCommand.Parameters.Add("@SupplierCode", SqlDbType.Int).Value = Integer.Parse(SupplierCode.Text)
            DBCommand.Parameters.Add("@MakerCode", SqlDbType.Int).Value = ConvertStringToInt(MakerCode.Text)
            DBCommand.Parameters.Add("@SAPMakerCode", SqlDbType.Int).Value = ConvertStringToInt(SAPMakerCode.Text)
            DBCommand.Parameters.Add("@SpecSheet", SqlDbType.Bit).Value = SpecSheet.Checked
            DBCommand.Parameters.Add("@Specification", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(Specification.Text)
            DBCommand.Parameters.Add("@SupplierContactPerson", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(SupplierContactPerson.Text)
            DBCommand.Parameters.Add("@SupplierItemName", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(SupplierItemName.Text.Trim)
            DBCommand.Parameters.Add("@ShippingHandlingFee", SqlDbType.Decimal).Value = ConvertStringToDec(ShippingHandlingFee.Text)
            DBCommand.Parameters.Add("@ShippingHandlingCurrencyCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(ShippingHandlingCurrency.Text)
            DBCommand.Parameters.Add("@PaymentTermCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(PaymentTerm.SelectedValue)
            DBCommand.Parameters.Add("@Comment", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(Comment.Text)
            DBCommand.Parameters.Add("@Priority", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(st_Priority)
            DBCommand.Parameters.Add("@PurposeCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(st_PurposeCode)
            DBCommand.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = Integer.Parse(Session("UserID"))
            DBCommand.Parameters.Add("@EnqStorageLocation", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(st_EnqStorageLocationCode)
            DBCommand.Parameters.Add("@QuoStorageLocation", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(st_QuoStorageLocationCode)
            DBCommand.Parameters.Add("@RFQNumber", SqlDbType.Int).Value = Integer.Parse(st_RFQNumber)
            DBCommand.Parameters.Add("@SupplierContactPersonSel", SqlDbType.NVarChar).Value = SupplierContactPersonCodeList.SelectedValue
            DBCommand.Parameters.Add("@SupplierOfferValidTo", SqlDbType.NVarChar).Value = txtVaildTo.Text
            DBCommand.Parameters.Add("@CodeExtensionCode", SqlDbType.NVarChar).Value = Me.CodeExtensionList.SelectedValue
            DBCommand.ExecuteNonQuery()
            DBCommand.Parameters.Clear()
            DBCommand.Dispose()
            SQLLineUpdate = "UPDATE RFQLine SET CurrencyCode = @CurrencyCode, UnitPrice = @UnitPrice, " _
& "QuoPer = @QuoPer, QuoUnitCode = @QuoUnitCode, LeadTime = @LeadTime, SupplierItemNumber = @SupplierItemNumber, " _
& "IncotermsCode = @IncotermsCode, DeliveryTerm = @DeliveryTerm, Packing = @Packing, Purity = @Purity, " _
& "QMMethod = @QMMethod,SupplierOfferNo=@SupplierOfferNo,NoOfferReasonCode = @NoOfferReasonCode, UpdatedBy = @UpdatedBy, UpdateDate = GETDATE() " _
& "Where RFQLineNumber = @RFQLineNumber"
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
                DBCommand.Parameters.Add("@SupplierOfferNo", SqlDbType.NVarChar).Value = ConvertEmptyStringToNull(SupplierOfferNo(i).Text)
                DBCommand.Parameters.Add("@NoOfferReasonCode", SqlDbType.VarChar).Value = ConvertEmptyStringToNull(NoOfferReason(i).SelectedValue)
                DBCommand.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = ConvertStringToInt(Session("UserID"))
                If LineNumber(i).Value.Trim <> String.Empty Then
                    DBCommand.CommandText = SQLLineUpdate
                    DBCommand.ExecuteNonQuery()
                    DBCommand.Parameters.Clear()
                End If
            End If
            sqlTran.Commit()
            UpdateDate.Value = GetUpdateDate("v_RFQHeader", "RFQNumber", st_RFQNumber)
        Catch ex As Exception
            sqlTran.Rollback()
            Ret = False
            Throw
        Finally
            DBCommand.Dispose()
            DBConn.Close()
        End Try
        Return Ret
    End Function
    ''' <summary> 
    ''' EnqUserパラメータのPlantを取得
    ''' </summary> 
    Private Sub EnqUserPlantSpmatlStatus()

        Dim sb_SQL As New Text.StringBuilder
        sb_SQL.Clear()
        sb_SQL.Append(" SELECT
                                MP.MaterialStatus , PM.[Text]
                            FROM
                                StorageLocation As SL
                                INNER Join s_MaterialPlant AS MP
                                    On MP.Plant = SL.Plant
                                INNER Join s_PlantMaterialStatus AS PM
                                    On PM.MaterialStatus = MP.MaterialStatus
                            WHERE 
                                MP.ERPProductNumber = @ERPProductNumber
                            AND SL.Storage = @StorageLocation
                            ")
        Dim Connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
        Dim DBCommand As SqlClient.SqlCommand
        DBCommand = Connection.CreateCommand
        DBCommand.CommandText = sb_SQL.ToString
        DBCommand.Parameters.Clear()
        DBCommand.Parameters.Add("@ERPProductNumber", SqlDbType.VarChar).Value = RFQListByProductID.Text & Me.CodeExtensionList.SelectedValue
        DBCommand.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = StorageLocation.SelectedValue

        Connection.Open()
        DBCommand.ExecuteNonQuery()
        Connection.Close()

        Dim DBAdapter As New SqlClient.SqlDataAdapter()
        DBAdapter.SelectCommand = DBCommand
        Dim DS As DataSet = New DataSet
        DBAdapter.Fill(DS, "s_MaterialPlant")

        If DS.Tables("s_MaterialPlant").Rows.Count > 0 Then
            EnqUserPlantStatus.Text = DS.Tables("s_MaterialPlant").Rows(0)("MaterialStatus").ToString
            EnqUserPlantDescriptions.Text = DS.Tables("s_MaterialPlant").Rows(0)("Text").ToString
        Else
            EnqUserPlantStatus.Text = String.Empty
            EnqUserPlantDescriptions.Text = String.Empty
        End If
    End Sub
    ''' <summary> 
    ''' QuoUserパラメータのPlantを取得
    ''' </summary> 
    Private Sub QuoUserPlantSpmatlStatus()

        Dim sb_SQL As New Text.StringBuilder
        sb_SQL.Clear()
        sb_SQL.Append(" SELECT
                            MP.MaterialStatus , PM.[Text]
                        FROM
                            StorageLocation As SL
                            INNER Join s_MaterialPlant AS MP
                                On MP.Plant = SL.Plant
                            INNER Join s_PlantMaterialStatus AS PM
                                On PM.MaterialStatus = MP.MaterialStatus
                        WHERE 
                            MP.ERPProductNumber = @ERPProductNumber
                        AND SL.Storage = @StorageLocation
                        ")

        Dim Connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
        Dim DBCommand As SqlClient.SqlCommand
        DBCommand = Connection.CreateCommand
        DBCommand.CommandText = sb_SQL.ToString
        DBCommand.Parameters.Clear()
        DBCommand.Parameters.Add("@ERPProductNumber", SqlDbType.VarChar).Value = RFQListByProductID.Text & Me.CodeExtensionList.SelectedValue
        DBCommand.Parameters.Add("@StorageLocation", SqlDbType.VarChar).Value = StorageLocation2.SelectedValue

        Connection.Open()
        DBCommand.ExecuteNonQuery()
        Connection.Close()

        Dim DBAdapter As New SqlClient.SqlDataAdapter()
        Dim DS As DataSet = New DataSet
        DBAdapter.SelectCommand = DBCommand
        DBAdapter.Fill(DS, "s_MaterialPlant")
        If DS.Tables("s_MaterialPlant").Rows.Count > 0 Then
            QuoUserPlantStatus.Text = DS.Tables("s_MaterialPlant").Rows(0)("MaterialStatus").ToString
            QuoUserPlantDescriptions.Text = DS.Tables("s_MaterialPlant").Rows(0)("Text").ToString
        Else
            QuoUserPlantStatus.Text = String.Empty
            QuoUserPlantDescriptions.Text = String.Empty
        End If
    End Sub
    ''' <summary> 
    ''' EnqUserMMSTAValidationステータスを取得
    ''' </summary> 
    Private Function EnqMMSTAValidationSearch() As String
        'Enq-User と Quo-User の Storage Location が同一の場合は EnqResult = S (Success) と判定
        If (Not String.IsNullOrEmpty(Me.StorageLocation.SelectedValue)) AndAlso Me.StorageLocation.SelectedValue = Me.StorageLocation2.SelectedValue Then
            Return "S"
        End If
        'Enq-User の Plant-sp.matl Status がブランクの場合は EnqResult = S と判定
        If String.IsNullOrEmpty(Me.EnqUserPlantStatus.Text) Then
            Return "S"
        End If

        Dim DS As DataSet = New DataSet
        Dim Connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
        Dim DBCommand As SqlClient.SqlCommand
        Dim DBAdapter As New SqlClient.SqlDataAdapter()

        Dim EnqCountry As String = Nothing
        Dim QuoCountry As String = Nothing
        Dim DomesticFlag As String


        Dim sb_SQL As New Text.StringBuilder

        'Enq-User の　CountryCode 取得
        sb_SQL.Clear()
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("  CountryCode ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("  StorageLocation ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  Storage = @StorageLOcationText")

        DBCommand = Connection.CreateCommand
        DBCommand.CommandText = sb_SQL.ToString
        DBCommand.Parameters.Clear()
        DBCommand.Parameters.Add("@StorageLOcationText", SqlDbType.VarChar).Value = Me.StorageLocation.SelectedValue 'EnqLocationCode.Value
        Connection.Open()
        DBCommand.ExecuteNonQuery()
        Connection.Close()
        DBAdapter.SelectCommand = DBCommand
        DBAdapter.Fill(DS, "EnqStorageLocation")
        If DS.Tables("EnqStorageLocation").Rows.Count > 0 Then
            EnqCountry = DS.Tables("EnqStorageLocation").Rows(0)("CountryCode").ToString
        End If

        'Quo-User の CountryCode 取得
        DBCommand = Connection.CreateCommand
        DBCommand.CommandText = sb_SQL.ToString
        DBCommand.Parameters.Clear()
        DBCommand.Parameters.Add("@StorageLOcationText", SqlDbType.VarChar).Value = Me.StorageLocation2.SelectedValue ' EnqLocationCode.Value
        Connection.Open()
        DBCommand.ExecuteNonQuery()
        Connection.Close()
        DBAdapter.SelectCommand = DBCommand
        DBAdapter.Fill(DS, "QuoStorageLocation")
        If DS.Tables("QuoStorageLocation").Rows.Count > 0 Then
            QuoCountry = DS.Tables("QuoStorageLocation").Rows(0)("CountryCode").ToString
        End If

        If EnqCountry IsNot Nothing AndAlso EnqCountry = QuoCountry Then
            DomesticFlag = "1"
        Else
            DomesticFlag = "0"
        End If

        sb_SQL.Clear()
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("  Result ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("  MMSTAValidation ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  MaterialStatus = @PlantStatus  AND DomesticFlag = @DomesticFlag")

        DBCommand = Connection.CreateCommand
        DBCommand.CommandText = sb_SQL.ToString
        DBCommand.Parameters.Clear()

        DBCommand.Parameters.Add("@PlantStatus", SqlDbType.VarChar).Value = EnqUserPlantStatus.Text
        DBCommand.Parameters.Add("@DomesticFlag", SqlDbType.Bit).Value = CInt(DomesticFlag)
        Connection.Open()
        DBCommand.ExecuteNonQuery()
        Connection.Close()


        DBAdapter.SelectCommand = DBCommand
        DBAdapter.Fill(DS, "MMSTAValidation")
        If DS.Tables("MMSTAValidation").Rows.Count > 0 Then
            EnqUserStatus.Text = DS.Tables("MMSTAValidation").Rows(0)("Result").ToString
        End If

        Return EnqUserStatus.Text
    End Function
    ''' <summary> 
    ''' QuoUserMMSTAValidationステータスを取得
    ''' </summary> 
    Private Function QuoMMSTAValidationSearch() As String
        'Quo-User の Plant-sp.matl Status がブランクの場合は QuoResult = S と判定
        If String.IsNullOrEmpty(Me.QuoUserPlantStatus.Text) Then
            Return "S"
        End If

        Dim DS As DataSet = New DataSet
        Dim Connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
        Dim DBCommand As SqlClient.SqlCommand
        Dim DBAdapter As New SqlClient.SqlDataAdapter()

        Dim QuoCountry As String = String.Empty
        Dim DomesticFlag As String

        Dim sb_SQL As New Text.StringBuilder

        'Quo-User の　CountryCode 取得
        sb_SQL.Clear()
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("  CountryCode ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("  StorageLocation ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  Storage = @StorageLOcationText")

        DBCommand = Connection.CreateCommand
        DBCommand.CommandText = sb_SQL.ToString
        DBCommand.Parameters.Clear()
        DBCommand.Parameters.Add("@StorageLOcationText", SqlDbType.VarChar).Value = Me.StorageLocation2.SelectedValue 'EnqLocationCode.Value
        Connection.Open()
        DBCommand.ExecuteNonQuery()
        Connection.Close()
        DBAdapter.SelectCommand = DBCommand
        DBAdapter.Fill(DS, "StorageLocation")
        If DS.Tables("StorageLocation").Rows.Count > 0 Then
            QuoCountry = DS.Tables("StorageLocation").Rows(0)("CountryCode").ToString
        End If
        Dim SupCountry As String = String.Empty

        If (Not String.IsNullOrWhiteSpace(Me.SupplierCode.Text)) And IsNumeric(SupplierCode.Text) Then
            Dim tdaSupplier As New TCIDataAccess.Supplier()
            tdaSupplier.Load(Me.SupplierCode.Text)
            SupCountry = tdaSupplier.CountryCode
        End If

        If SupCountry = QuoCountry Then
            DomesticFlag = "1"
        Else
            DomesticFlag = "0"
        End If

        sb_SQL.Clear()
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("  Result ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("   MMSTAValidation ")
        sb_SQL.Append("WHERE ")
        sb_SQL.Append("  MaterialStatus = @PlantStatus  AND DomesticFlag = @DomesticFlag")

        DBCommand = Connection.CreateCommand
        DBCommand.CommandText = sb_SQL.ToString
        DBCommand.Parameters.Clear()
        DBCommand.Parameters.Add("@PlantStatus", SqlDbType.VarChar).Value = QuoUserPlantStatus.Text
        DBCommand.Parameters.Add("@DomesticFlag", SqlDbType.Bit).Value = CInt(DomesticFlag)
        Connection.Open()
        DBCommand.ExecuteNonQuery()
        Connection.Close()

        DBAdapter.SelectCommand = DBCommand
        DBAdapter.Fill(DS, "MMSTAValidation")
        If DS.Tables("MMSTAValidation").Rows.Count > 0 Then
            QuoUserStatus.Text = DS.Tables("MMSTAValidation").Rows(0)("Result").ToString
        End If

        Return QuoUserStatus.Text
    End Function
    ''' <summary> 
    ''' POInterfaceメッセージ取得
    ''' </summary> 
    Private Sub POInterfaceMsgSet(encResult As String, quoResult As String)
        Dim DS As DataSet = New DataSet
        Dim Connection As New SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
        Dim DBCommand As SqlClient.SqlCommand

        Dim sb_SQL As New Text.StringBuilder
        sb_SQL.Append("SELECT ")
        sb_SQL.Append("  EnqResult, QuoResult, Message, MessageType ")
        sb_SQL.Append("FROM ")
        sb_SQL.Append("  MMSTAValidationMessage ")
        sb_SQL.Append("WHERE")
        sb_SQL.Append("  EnqResult = @EnqResult AND QuoResult = @QuoResult")

        DBCommand = Connection.CreateCommand
        DBCommand.CommandText = sb_SQL.ToString
        DBCommand.Parameters.Clear()

        DBCommand.Parameters.Add("@EnqResult", SqlDbType.VarChar).Value = encResult
        DBCommand.Parameters.Add("@QuoResult", SqlDbType.VarChar).Value = quoResult

        Connection.Open()
        DBCommand.ExecuteNonQuery()
        Connection.Close()

        Dim DBAdapter As New SqlClient.SqlDataAdapter()
        DBAdapter.SelectCommand = DBCommand
        DBAdapter.Fill(DS, "MMSTAValidationMessage")

        Dim message As String = String.Empty
        Dim messageType As String = String.Empty

        Dim messageTypes As Dictionary(Of String, String) = New Dictionary(Of String, String) From {{"C", "Confirm"}, {"E", "Error"}}

        If DS.Tables("MMSTAValidationMessage").Rows.Count > 0 Then
            EnqUserStatus.Text = DS.Tables("MMSTAValidationMessage").Rows(0)("EnqResult").ToString
            QuoUserStatus.Text = DS.Tables("MMSTAValidationMessage").Rows(0)("QuoResult").ToString
            message = DS.Tables("MMSTAValidationMessage").Rows(0)("Message").ToString
            messageType = DS.Tables("MMSTAValidationMessage").Rows(0)("MessageType").ToString
        End If

        POInterfaceConfirmMsg.Text = ""
        If Not message.Equals(String.Empty) AndAlso messageTypes.ContainsKey(messageType) Then
            If messageTypes(messageType) = "Error" Then
                POInterfaceMsg.Text = message.Replace("[Enq-Location]", Me.StorageLocation.SelectedValue).Replace("[Quo-Location]", Me.StorageLocation2.SelectedValue)
            Else
                POInterfaceConfirmMsg.Text = message.Replace("[Enq-Location]", Me.StorageLocation.SelectedValue).Replace("[Quo-Location]", Me.StorageLocation2.SelectedValue)
            End If
        End If
    End Sub

    Private Sub CodeExtensionList_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CodeExtensionList.SelectedIndexChanged
        Call EnqUserPlantSpmatlStatus()
        Call QuoUserPlantSpmatlStatus()
    End Sub

End Class
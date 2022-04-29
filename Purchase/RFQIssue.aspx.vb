Option Explicit On
Option Strict On
Option Infer Off

Imports System.Data.SqlClient
Imports Purchase.Common
Imports Purchase.TCIDataAccess

Partial Public Class RFQIssue
    Inherits CommonPage
    Private DBConn As New SqlConnection
    Private DBCommand As SqlCommand
    Protected b_IsDebug As Boolean
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
    Protected bol_Parameter As Boolean = True
    Protected st_Role As String = Nothing
    Protected st_ProductID As String = Nothing
    Protected st_ProductNumber As String = Nothing
    Protected st_SupplierCode As String = Nothing

    ''' <summary>
    ''' ページロード
    ''' </summary>
    ''' <remarks>
    ''' ページを読み込む
    ''' </remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DBConn.ConnectionString = DB_CONNECT_STRING
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()
        ' ロールの取得
        st_Role = Session(SESSION_ROLE_CODE).ToString
        Dim st_ProductID As String = String.Empty
        '        Dim st_ProductNumber As String = String.Empty
        Dim st_SupplierCode As String = String.Empty

        If IsPostBack = False Then
            ' 初期遷移の場合
            If String.IsNullOrWhiteSpace(Request.QueryString("ProductID")) = False Then
                st_ProductID = Request.QueryString("ProductID")
                Dim rFQIssueDisp As Join.RFQIssueDisp = New Join.RFQIssueDisp
                st_ProductNumber = rFQIssueDisp.GetProductNumber(Cint(st_ProductID))
            End If
            If String.IsNullOrWhiteSpace(Request.QueryString("SupplierCode")) = False Then
                st_SupplierCode = Request.QueryString("SupplierCode")
            End If
            Call SetPostBackUrl()

            If CheckPram(st_ProductID, st_SupplierCode) = False Then
                Msg.Text = ERR_INVALID_PARAMETER
                '画面上の入力項目を隠す。
                bol_Parameter = False
                Exit Sub
            End If
            Call InitDropDownList(st_ProductID, st_SupplierCode)

        Else
            ' ポストバックの場合
            If String.IsNullOrWhiteSpace(Request.Form("ProductNumber")) = False Then
                st_ProductNumber = Request.Form("ProductNumber")
            ElseIf String.IsNullOrWhiteSpace(Request.Form("ProductID")) = False Then
                st_ProductID = Request.Form("ProductID")
                Dim rFQIssueDisp As Join.RFQIssueDisp = New Join.RFQIssueDisp
                st_ProductNumber = rFQIssueDisp.GetProductNumber(Cint(st_ProductID))
            End If
            If String.IsNullOrWhiteSpace(Request.Form("SupplierCode")) = False Then
                st_SupplierCode = Request.Form("SupplierCode")
            End If

            Call SetReadOnlyItems()

        End If

        Call SetOnClientClick()

    End Sub

    ''' <summary>
    ''' ページアンロード
    ''' </summary>
    ''' <remarks>
    ''' ページアンロード
    ''' </remarks>
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        'DB切断
        DBConn.Close()
    End Sub

    ''' <summary>
    ''' Issueボタン押下
    ''' </summary>
    ''' <remarks>
    ''' 入力された情報をRFQHeaderとRFQLineに登録する。
    ''' </remarks>
    Protected Sub Issue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Issue.Click

        Dim i_RFQNumber As Integer = -1
        Dim i_ProductID As Integer = -1
        Dim bol_EnqQuantity1 As Boolean = False
        Dim bol_EnqQuantity2 As Boolean = False
        Dim bol_EnqQuantity3 As Boolean = False
        Dim bol_EnqQuantity4 As Boolean = False

        Msg.Text = ""
        ' パラメータの確認
        If Request.QueryString("Action") <> "Issue" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If
        ' RFQHeaderの入力チェック
        If CheckRFQHeader() = False Then
            Exit Sub
        End If
        ' RFQLineの入力チェック
        If CheckRFQLine(bol_EnqQuantity1, bol_EnqQuantity2, bol_EnqQuantity3, bol_EnqQuantity4) = False Then
            Exit Sub
        End If
        ' Insert情報の入力チェック
        If CheckInsertColumn(ProductNumber.Text, i_ProductID) = False Then
            Exit Sub
        End If

        ' RFQheaderとRFQLineにレコード追加を実行
        Dim i_InsCount As Integer = 0

        Dim f_RFQIssue As FacadeRFQIssue = New FacadeRFQIssue
        f_RFQIssue.RFQHeader = SetRFQheaderInfo(i_ProductID)
        f_RFQIssue.RFQLineList = SetRFQLineInfo(i_RFQNumber, bol_EnqQuantity1, bol_EnqQuantity2, bol_EnqQuantity3, bol_EnqQuantity4)
        i_InsCount = f_RFQIssue.Save()

        Response.Redirect("RFQUpdate.aspx?RFQNumber=" & i_InsCount, False)

    End Sub

    ''' <summary>
    ''' RFQheaderの追加に必要な画面情報を格納する
    ''' </summary>
    ''' <remarks></remarks>
    Private Function SetRFQheaderInfo(ByVal productID As Integer) As TCIDataAccess.RFQHeader
        Dim RFQHeaderInfo As TCIDataAccess.RFQHeader = New TCIDataAccess.RFQHeader
        Dim i As Integer = 0

        RFQHeaderInfo.EnqLocationCode = EnqLocation.SelectedValue
        RFQHeaderInfo.EnqUserID = CInt(EnqUser.SelectedValue)
        RFQHeaderInfo.QuoLocationCode = QuoLocation.SelectedValue
        RFQHeaderInfo.QuoUserID = CInt(If(String.IsNullOrWhiteSpace(QuoUser.SelectedValue) = True, Nothing, QuoUser.SelectedValue))
        RFQHeaderInfo.ProductID = productID
        RFQHeaderInfo.SupplierCode = CInt(SupplierCode.Text)
        RFQHeaderInfo.MakerCode = CInt(If(String.IsNullOrWhiteSpace(MakerCode.Text) = True, Nothing, MakerCode.Text))
        RFQHeaderInfo.PurposeCode = Purpose.SelectedValue
        RFQHeaderInfo.RequiredPurity = If(String.IsNullOrWhiteSpace(RequiredPurity.Text) = True, Nothing, RequiredPurity.Text)
        RFQHeaderInfo.RequiredQMMethod = If(String.IsNullOrWhiteSpace(RequiredQMMethod.Text) = True, Nothing, RequiredQMMethod.Text)
        RFQHeaderInfo.RequiredSpecification = If(String.IsNullOrWhiteSpace(RequiredSpecification.Text) = True, Nothing, RequiredSpecification.Text)
        RFQHeaderInfo.SupplierContactPerson = If(String.IsNullOrWhiteSpace(SupplierContactPerson.Text) = True, Nothing, SupplierContactPerson.Text)
        RFQHeaderInfo.SupplierItemName = If(String.IsNullOrWhiteSpace(SupplierItemName.Text) = True, Nothing, SupplierItemName.Text)
        RFQHeaderInfo.Comment = If(String.IsNullOrWhiteSpace(Comment.Text) = True, Nothing, Comment.Text)
        RFQHeaderInfo.RFQStatusCode = Cstr(IIf(Integer.TryParse(QuoUser.SelectedValue, i) = True, "A", "N"))
        RFQHeaderInfo.Priority = If(String.IsNullOrWhiteSpace(Priority.SelectedValue) = True, Nothing, Priority.SelectedValue)
        RFQHeaderInfo.CreatedBy = CInt(Session("UserID").ToString)
        RFQHeaderInfo.UpdatedBy = CInt(Session("UserID").ToString)
        RFQHeaderInfo.SupplierContactPersonSel = SupplierContactPersonCodeList.SelectedValue
        RFQHeaderInfo.SAPMakerCode = If(String.IsNullOrWhiteSpace(SAPMakerCode.Text) = True, Nothing, Cint(SAPMakerCode.Text))
        RFQHeaderInfo.CodeExtensionCode = CodeExtensionList.SelectedValue

        Return RFQHeaderInfo

    End Function

    ''' <summary>
    ''' RFQLineの追加に必要な画面情報を格納する
    ''' </summary>
    ''' <remarks></remarks>
    Private Function  SetRFQLineInfo(ByVal RFQNumber As Integer, ByVal enqQuantity1 As Boolean, ByVal enqQuantity2 As Boolean, ByVal enqQuantity3 As Boolean, ByVal enqQuantity4 As Boolean) As TCIDataAccess.RFQLineList
        Dim RFQLineListInfo As TCIDataAccess.RFQLineList = New TCIDataAccess.RFQLineList

        If enqQuantity1 = True Then
            Dim RFQLineInfo As TCIDataAccess.RFQLine = New TCIDataAccess.RFQLine 
            RFQLineInfo.RFQNumber = RFQNumber
            RFQLineInfo.EnqQuantity = CDec(EnqQuantity_1.Text)
            RFQLineInfo.EnqUnitCode = EnqUnit_1.SelectedValue
            RFQLineInfo.EnqPiece = CInt(EnqPiece_1.Text)
            RFQLineInfo.SupplierItemNumber = Cstr(IIf(String.IsNullOrEmpty(SupplierItemNumber_1.Text), Nothing, SupplierItemNumber_1.Text))

            RFQLineListInfo.Add(RFQLineInfo)
        End If

        If enqQuantity2 = True Then
            Dim RFQLineInfo As TCIDataAccess.RFQLine = New TCIDataAccess.RFQLine 
            RFQLineInfo.RFQNumber = RFQNumber
            RFQLineInfo.EnqQuantity = Cdec(EnqQuantity_2.Text)
            RFQLineInfo.EnqUnitCode = EnqUnit_2.SelectedValue
            RFQLineInfo.EnqPiece = Cint(EnqPiece_2.Text)
            RFQLineInfo.SupplierItemNumber = Cstr(IIf(String.IsNullOrEmpty(SupplierItemNumber_2.Text), Nothing, SupplierItemNumber_2.Text))

            RFQLineListInfo.Add(RFQLineInfo)
        End If

        If enqQuantity3 = True Then
            Dim RFQLineInfo As TCIDataAccess.RFQLine = New TCIDataAccess.RFQLine 
            RFQLineInfo.RFQNumber = RFQNumber
            RFQLineInfo.EnqQuantity = Cdec(EnqQuantity_3.Text)
            RFQLineInfo.EnqUnitCode = EnqUnit_3.SelectedValue
            RFQLineInfo.EnqPiece = Cint(EnqPiece_3.Text)
            RFQLineInfo.SupplierItemNumber = Cstr(IIf(String.IsNullOrEmpty(SupplierItemNumber_3.Text), Nothing, SupplierItemNumber_3.Text))

            RFQLineListInfo.Add(RFQLineInfo)
        End If

        If enqQuantity4 = True Then
            Dim RFQLineInfo As TCIDataAccess.RFQLine = New TCIDataAccess.RFQLine 
            RFQLineInfo.RFQNumber = RFQNumber
            RFQLineInfo.EnqQuantity = Cdec(EnqQuantity_4.Text)
            RFQLineInfo.EnqUnitCode = EnqUnit_4.SelectedValue
            RFQLineInfo.EnqPiece = Cint(EnqPiece_4.Text)
            RFQLineInfo.SupplierItemNumber = Cstr(IIf(String.IsNullOrEmpty(SupplierItemNumber_4.Text), Nothing, SupplierItemNumber_4.Text))

            RFQLineListInfo.Add(RFQLineInfo)
        End If

        Return RFQLineListInfo

    End Function

    Private Sub SetPostBackUrl()
        'ボタンクリック時にPostBackするActionを追記する。
        Issue.PostBackUrl = "~/RFQIssue.aspx?Action=Issue"
    End Sub
    Private Function CheckPram(ByVal st_ProductID As String, ByVal st_SupplierCode As String) As Boolean
        '他画面から取得するパラメータのチェック
        Dim DBReader As SqlDataReader
        Dim sb_Sql As New StringBuilder

        If Not String.IsNullOrEmpty(st_ProductID) Then
            If IsNumeric(st_ProductID) Then
                Dim product As Product = New Product
                product.Load(CInt(st_ProductID))

                ProductNumber.Text = product.ProductNumber
                CASNumber.Text = product.CASNumber
                ProductName.Text = Cstr(IIf(String.IsNullOrEmpty(product.QuoName.Trim), product.Name, product.QuoName))
                ProductNumber.ReadOnly = True
                ProductNumber.CssClass = "readonly"
                ProductSelect.Visible = False
            Else
                Return False
            End If
        End If

        If Not String.IsNullOrEmpty(st_SupplierCode) Then
            If IsNumeric(st_SupplierCode) Then
                DBCommand.CommandText = CreateSql_SelectQuoLocation()
                DBCommand.Parameters.Add("SupplierCode", SqlDbType.Int).Value = Cint(st_SupplierCode)
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.HasRows = True Then
                    While DBReader.Read
                        SupplierCode.Text = DBReader("SupplierCode").ToString
                        R3SupplierCode.Text = DBReader("S4SupplierCode").ToString
                        SupplierName.Text = DBReader("SupplierName").ToString
                        SupplierCountry.Text = DBReader("CountryName").ToString

                        If DBReader("QuoLocationName").ToString = DIRECT Then
                            QuoLocation.SelectedValue = Session("LocationCode").ToString
                        Else
                            QuoLocation.SelectedValue = DBReader("QuoLocationCode").ToString
                        End If
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
    Private Sub InitDropDownList(ByVal st_ProductID As String, ByVal st_SupplierCode As String)
        'ドロップダウン初期設定
        EnqLocation.SelectedValue = Session("LocationCode").ToString
        EnqLocation.DataBind()

        SetControl_EnqUser()
        isAdmin.Text = Session("Purchase.isAdmin").ToString
        userId.Text = Session("UserID").ToString
        If Boolean.Parse(Session("Purchase.isAdmin").ToString) = False Then
            '            Dim flg As Boolean = False
            '            For index = 0 To EnqUser.Items.Count
            '                If index = EnqUser.Items.Count Then
            '                    GoTo tuichu
            '                End If
            '                If EnqUser.Items(index).Value = Session("UserID").ToString Then
            '                    flg = True
            '                    GoTo tuichu
            '                End If
            '            Next
            'tuichu:
            '            If Session("UserID").ToString = "" Or flg = False Then
            '                Response.Redirect("IsuseError.html")
            '            Else
            '                EnqUser.SelectedValue = Session("UserID").ToString
            '            End If
            'EnqUser.SelectedValue = Session("UserID").ToString
        End If

        QuoLocation.DataBind()
        QuoUser.Items.Clear()
        QuoUser.Items.Add(String.Empty)

        ' Priorityのドロップダウンリストに値を設定する
        SetPriorityDropDownList(Priority, PRIORITY_FOR_EDIT)
        Priority.SelectedValue = ""
        Priority.Visible = True

        ' Code Extension のドロップダウンリストに値を設定する
        If Not String.IsNullOrWhiteSpace(st_ProductID) Then
            If IsNumeric(st_ProductID) Then
                ' Code Extension のドロップダウンリストの値を取得する
                Common.SetCodeExtensionDropDownList(CodeExtensionList, st_ProductNumber)
            End If
        End If
        ' Supplier Contact PersonCode のドロップダウンリストに値を設定する
        If Not String.IsNullOrWhiteSpace(st_SupplierCode) Then
            If IsNumeric(st_SupplierCode) Then
                ' SupplierContactPersonCode のドロップダウンリストの値を取得する
                Common.SetSupplierContactPersonCodeList(SupplierContactPersonCodeList, st_SupplierCode)
            Else
                Msg.Text = ERR_INCORRECT_SUPPLIERCODE
            End If
        End If

    End Sub

    Private Sub SetReadOnlyItems()
        'ReadOnly項目の再設定
        If ProductNumber.ReadOnly = True Then
            ProductNumber.Text = Request.Form("ProductNumber").ToString
        End If
        CASNumber.Text = Request.Form("CASNumber").ToString
        ProductName.Text = Request.Form("ProductName").ToString
        If SupplierCode.ReadOnly = True Then
            SupplierCode.Text = Request.Form("SupplierCode").ToString
        End If
        R3SupplierCode.Text = Request.Form("R3SupplierCode").ToString
        SupplierName.Text = Request.Form("SupplierName").ToString
        SupplierCountry.Text = Request.Form("SupplierCountry").ToString
        SAPMakerCode.Text = Request.Form("SAPMakerCode").ToString
        MakerName.Text = Request.Form("MakerName").ToString
        MakerCountry.Text = Request.Form("MakerCountry").ToString
    End Sub

    ''' <summary>
    ''' RFQSupplierSelect 画面へ遷移する際のパラメータを一部セット
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    Private Sub SetOnClientClick()
        'RFQSupplierSelect 画面へ遷移する際のパラメータを一部セットする。
        SupplierSelect.OnClientClick =
        String.Format("return SupplierSelect_onclick(""" &
                      Server.UrlEncode(ClientScript.GetPostBackEventReference(SupplierSelect, String.Empty)) _
                      & """)")
    End Sub

    ''' <summary>
    ''' RFQHeaderの必須入力と文字数をチェックする
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <returns>Boolean</returns>
    Private Function CheckRFQHeader() As Boolean
        '必須入力項目チェックHeader
        Dim i_Result As Integer = 0

        If String.IsNullOrWhiteSpace(EnqLocation.SelectedValue) Then
            Msg.Text = ERR_REQUIRED_ENQLOCATION
            Return False
        End If
        If String.IsNullOrWhiteSpace(EnqUser.SelectedValue) Then
            Msg.Text = ERR_REQUIRED_ENQUSER
            Return False
        End If
        If String.IsNullOrWhiteSpace(ProductNumber.Text) Then
            Msg.Text = ERR_REQUIRED_PRODUCTNUMBER
            Return False
        Else
            '権限ロールに従い極秘品はエラーとする
            If String.Equals(st_Role, Common.ROLE_WRITE_P) OrElse String.Equals(st_Role, Common.ROLE_READ_P) Then
                If IsConfidentialItem(ProductNumber.Text) Then
                    Msg.Text = Common.ERR_CONFIDENTIAL_PRODUCT
                    Return False
                End If
            End If

            'CAS からも RFQ が登録できるようにコメントアウトした。
            'ProductNumber が正しいかのチェックは CheckInsertColumn でされる。
            'ElseIf TCICommon.Func.IsCASNumber(ProductNumber.Text) = True Then 
            '    Msg.Text = ERR_ISCASNUMBER
            '    Return False
        End If
        If String.IsNullOrWhiteSpace(SupplierCode.Text) Then
            Msg.Text = ERR_REQUIRED_SUPPLIERCODE
            Return False
        End If
        If String.IsNullOrWhiteSpace(QuoLocation.SelectedValue) Then
            Msg.Text = ERR_REQUIRED_QUOLOCATION
            Return False
        End If
        If String.IsNullOrWhiteSpace(Purpose.SelectedValue) Then
            Msg.Text = ERR_REQUIRED_PURPOSE
            Return False
        End If
        If Integer.TryParse(SupplierCode.Text, i_Result) = False Then
            Msg.Text = ERR_INCORRECT_SUPPLIERCODE
            Return False
        End If
        If String.IsNullOrWhiteSpace(MakerCode.Text) Then
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

    ''' <summary>
    ''' RFQLineの入力値の妥当性をチェックする。
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <returns>Boolean</returns>
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

    ''' <summary>
    ''' RFQLineの必須入力をチェックする。
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <returns>Boolean</returns>
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

    ''' <summary>
    ''' RFQLineが全て空欄である事をチェックする。
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <returns>Boolean</returns>
    Private Function IsAllNullOfRFQList(ByVal EnqQuantity As String, ByVal EnqUnit As String, ByVal EnqPiece As String) As Boolean
        '全ての項目が空白かチェック
        If String.IsNullOrWhiteSpace(EnqQuantity.Trim) And String.IsNullOrWhiteSpace(EnqUnit.Trim) And String.IsNullOrWhiteSpace(EnqPiece.Trim) Then
            Return True
        Else
            Return False
        End If
    End Function

    ''' <summary>
    ''' RFQLineの入力をチェックする
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <returns>Boolean</returns>
    Private Function CheckRFQLine(ByRef enqQuantity1 As Boolean, ByRef enqQuantity2 As Boolean, ByRef enqQuantity3 As Boolean, ByRef enqQuantity4 As Boolean) As Boolean
        '入力項目チェックLine
        Dim Bo_UnLine As Boolean = False

        enqQuantity1 = IsAllInputOfRFQList(EnqQuantity_1.Text, EnqUnit_1.SelectedValue, EnqPiece_1.Text)
        Dim bo_UnLine_1 As Boolean = IsAllNullOfRFQList(EnqQuantity_1.Text, EnqUnit_1.SelectedValue, EnqPiece_1.Text)
        If enqQuantity1 = False And bo_UnLine_1 = False Then
            Bo_UnLine = True
        End If
        If IsCheckRFQLineFormat(EnqQuantity_1.Text, EnqPiece_1.Text) = False And bo_UnLine_1 = False Then
            Bo_UnLine = True
        End If

        enqQuantity2 = IsAllInputOfRFQList(EnqQuantity_2.Text, EnqUnit_2.SelectedValue, EnqPiece_2.Text)
        Dim bo_UnLine_2 As Boolean = IsAllNullOfRFQList(EnqQuantity_2.Text, EnqUnit_2.SelectedValue, EnqPiece_2.Text)
        If enqQuantity2 = False And bo_UnLine_2 = False Then
            Bo_UnLine = True
        End If
        If IsCheckRFQLineFormat(EnqQuantity_2.Text, EnqPiece_2.Text) = False And bo_UnLine_2 = False Then
            Bo_UnLine = True
        End If

        enqQuantity3 = IsAllInputOfRFQList(EnqQuantity_3.Text, EnqUnit_3.SelectedValue, EnqPiece_3.Text)
        Dim bo_UnLine_3 As Boolean = IsAllNullOfRFQList(EnqQuantity_3.Text, EnqUnit_3.SelectedValue, EnqPiece_3.Text)
        If enqQuantity3 = False And bo_UnLine_3 = False Then
            Bo_UnLine = True
        End If
        If IsCheckRFQLineFormat(EnqQuantity_3.Text, EnqPiece_3.Text) = False And bo_UnLine_3 = False Then
            Bo_UnLine = True
        End If

        enqQuantity4 = IsAllInputOfRFQList(EnqQuantity_4.Text, EnqUnit_4.SelectedValue, EnqPiece_4.Text)
        Dim bo_UnLine_4 As Boolean = IsAllNullOfRFQList(EnqQuantity_4.Text, EnqUnit_4.SelectedValue, EnqPiece_4.Text)
        If enqQuantity4 = False And bo_UnLine_4 = False Then
            Bo_UnLine = True
        End If
        If IsCheckRFQLineFormat(EnqQuantity_4.Text, EnqPiece_4.Text) = False And bo_UnLine_4 = False Then
            Bo_UnLine = True
        End If
        If enqQuantity1 = False And enqQuantity2 = False And enqQuantity3 = False And enqQuantity4 = False Then
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

    ''' <summary>
    ''' Insert処理の事前チェックを行う。
    ''' </summary>
    ''' <remarks>
    ''' </remarks>
    ''' <returns>Boolean</returns>
    Private Function CheckInsertColumn(ByVal CheckProductNumber As String, ByRef ReturnProductID As Integer) As Boolean
        'Insert内容の入力チェック ProductNumberからProductIDを取得して返す。
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
                ReturnProductID = Cint(DBReader("ProductID").ToString)
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
                'If ExistenceConfirmation(st_Supplier, "S4SupplierCode", MakerCode.Text) = False Then
                Msg.Text = ERR_INCORRECT_MAKERCODE
                Return False
            Else
                Dim supplierDt As DataTable = GetDataTable("select S4SupplierCode from supplier where SupplierCode=" + MakerCode.Text)
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

    'CreateSql_SelectQuoLocation() に処理を統一したためコメントアウト
    '
    'Private Sub SetCountryName(ByVal CountryCode As String, ByVal DefaultQuoLocationCode As String)
    '    Dim st_CountryName As String = String.Empty
    '    Dim st_DefaultQuoLocationName As String = String.Empty
    '    'SupplierCountryName取得
    '    Dim st_SQLCommand As String = String.Empty
    '    st_SQLCommand = "SELECT CountryName, DefaultQuoLocationCode FROM v_Country WHERE CountryCode = @st_CountryCode"
    '    Try
    '        Using DBConnection As New SqlClient.SqlConnection(DB_CONNECT_STRING), _
    '        DBSQLCommand As SqlCommand = DBConnection.CreateCommand()
    '            DBConnection.Open()
    '            DBSQLCommand.CommandText = st_SQLCommand
    '            DBSQLCommand.Parameters.AddWithValue("st_CountryCode", CountryCode)
    '            Dim DBSQLDataReader As SqlDataReader
    '            DBSQLDataReader = DBSQLCommand.ExecuteReader()
    '            If DBSQLDataReader.HasRows = True Then
    '                While DBSQLDataReader.Read
    '                    SupplierCountry.Text = DBSQLDataReader("CountryName").ToString
    '                    QuoLocation.SelectedValue = IIf(DefaultQuoLocationCode = "", DBSQLDataReader("DefaultQuoLocationCode").ToString, DefaultQuoLocationCode)
    '                End While
    '            End If
    '        End Using
    '    Catch ex As Exception
    '        Throw
    '    End Try
    'End Sub

    ''' <summary>
    ''' QuoLocation プルダウンリストが変更されたときに発生するイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NET の既定値</param>
    ''' <param name="e">ASP.NET の既定値</param>
    ''' <remarks></remarks>
    Protected Sub QuoLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles QuoLocation.SelectedIndexChanged

        SetControl_QuoUser()

    End Sub

    ''' <summary>
    ''' EnqLocation プルダウンリストが変更されたときに発生するイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NET の既定値</param>
    ''' <param name="e">ASP.NET の既定値</param>
    ''' <remarks></remarks>
    Private Sub EnqLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles EnqLocation.SelectedIndexChanged
        Dim DBReader As SqlDataReader

        SetControl_EnqUser()

        'Supplier Code が数字でない場合は処理を中断する
        If Regex.IsMatch(SupplierCode.Text, "^[0-9]+$") = False Then Exit Sub

        DBCommand.CommandText = CreateSql_SelectQuoLocation()
        DBCommand.Parameters.Add("SupplierCode", SqlDbType.Int).Value = CInt(SupplierCode.Text)
        DBReader = DBCommand.ExecuteReader
        DBCommand.Dispose()
        If DBReader.HasRows = True Then
            While DBReader.Read
                'Direct の場合は Quo-Location に Enq-Location を設定する
                If DBReader("QuoLocationName").ToString = DIRECT Then
                    QuoLocation.SelectedValue = EnqLocation.SelectedValue
                Else
                    QuoLocation.SelectedValue = DBReader("QuoLocationCode").ToString
                End If
            End While
        End If
        DBReader.Close()

        'Quo-User を強制的にリセットする
        SetControl_QuoUser()

        Exit Sub

    End Sub

    ''' <summary>
    ''' QuoLocation を取得するクエリを生成します。
    ''' </summary>
    ''' <returns>SQL 文字列</returns>
    ''' <remarks></remarks>
    Private Function CreateSql_SelectQuoLocation() As String
        Dim sb_Sql As New StringBuilder

        sb_Sql.AppendLine("WITH [IrregularQuoLocation] AS (")
        sb_Sql.AppendLine("  SELECT")
        sb_Sql.AppendLine("    I.[SupplierCode],")
        sb_Sql.AppendLine("    I.[QuoLocationCode] AS QuoLocationCode,")
        sb_Sql.AppendLine("    ISNULL(L.[Name], '" & DIRECT & "') AS QuoLocationName")
        sb_Sql.AppendLine("  FROM")
        sb_Sql.AppendLine("    [IrregularRFQLocation] AS I")
        sb_Sql.AppendLine("      LEFT OUTER JOIN [s_Location] AS L ON L.[LocationCode] = I.[QuoLocationCode]")
        sb_Sql.AppendLine(")")
        sb_Sql.AppendLine("SELECT")
        sb_Sql.AppendLine("  S.[SupplierCode],")
        sb_Sql.AppendLine("  S.[R3SupplierCode],")
        sb_Sql.AppendLine("  S.[S4SupplierCode],")
        sb_Sql.AppendLine("  LTRIM(RTRIM(ISNULL(S.[Name3], '') + ' ' + ISNULL(S.[Name4], ''))) AS SupplierName,")
        sb_Sql.AppendLine("  S.[CountryCode],")
        sb_Sql.AppendLine("  C.[CountryName],")
        sb_Sql.AppendLine("  CASE WHEN I.[QuoLocationName] IS NULL THEN C.[DefaultQuoLocationCode] ELSE I.[QuoLocationCode] END AS QuoLocationCode,")
        sb_Sql.AppendLine("  ISNULL(I.[QuoLocationName], C.[DefaultQuoLocationName]) AS QuoLocationName")
        sb_Sql.AppendLine("FROM")
        sb_Sql.AppendLine("  [Supplier] AS S")
        sb_Sql.AppendLine("    LEFT OUTER JOIN [IrregularQuoLocation] AS I ON I.[SupplierCode] = S.[SupplierCode],")
        sb_Sql.AppendLine("  [v_Country] AS C")
        sb_Sql.AppendLine("WHERE")
        sb_Sql.AppendLine("  S.[CountryCode] = C.[CountryCode]")
        sb_Sql.AppendLine("  AND S.[SupplierCode] = @SupplierCode")

        Return sb_Sql.ToString

    End Function

    ''' <summary>
    ''' EnqUser コントロールを設定します。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetControl_EnqUser()

        EnqUser.Items.Clear()
        EnqUser.Items.Add(String.Empty)

        If IsConfidentialItem(ProductNumber.Text) Then
            SDS_RFQIssue_Enq_U.SelectCommand = "SELECT [UserID], [Name] FROM [v_User] WHERE [LocationCode] = @LocationCode AND isDisabled = 0 AND [RoleCode] = 'WRITE' and  [R3PurchasingGroup] is not null and [R3PurchasingGroup] <>'' ORDER BY [Name]"
        Else
            SDS_RFQIssue_Enq_U.SelectCommand = "SELECT [UserID], [Name] FROM [v_User] WHERE [LocationCode] = @LocationCode AND isDisabled = 0 and [R3PurchasingGroup] is not null and [R3PurchasingGroup] <>'' ORDER BY [Name]"
        End If

        SDS_RFQIssue_Enq_U.SelectParameters.Clear()
        SDS_RFQIssue_Enq_U.SelectParameters.Add("LocationCode", EnqLocation.SelectedValue.ToString)

    End Sub

    ''' <summary>
    ''' QuoUser コントロールを設定します。
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub SetControl_QuoUser()

        QuoUser.Items.Clear()
        QuoUser.Items.Add(String.Empty)

        If IsConfidentialItem(ProductNumber.Text) Then
            SDS_RFQIssue_Que_U.SelectCommand = "SELECT [UserID], [Name] FROM [v_User] WHERE [LocationCode] = @LocationCode AND [isDisabled] = 0 AND [RoleCode] = 'WRITE' and [R3PurchasingGroup] is not null and [R3PurchasingGroup] <>'' ORDER BY [Name]"
        Else
            SDS_RFQIssue_Que_U.SelectCommand = "SELECT [UserID], [Name] FROM [v_User] WHERE [LocationCode] = @LocationCode AND [isDisabled] = 0 and [R3PurchasingGroup] is not null and [R3PurchasingGroup] <>'' ORDER BY [Name]"
        End If

        SDS_RFQIssue_Que_U.SelectParameters.Clear()
        SDS_RFQIssue_Que_U.SelectParameters.Add("LocationCode", QuoLocation.SelectedValue.ToString)

    End Sub

    ''' <summary>
    ''' ProductNumber 変更時イベント
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ProductNumber_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ProductNumber.TextChanged

        Dim rFQIssueDisp As Join.RFQIssueDisp = New Join.RFQIssueDisp
        Dim productNumberInfo As List(Of Join.RFQIssueDispProductInfo) = New List(Of Join.RFQIssueDispProductInfo)
        If Not String.IsNullOrWhiteSpace(ProductNumber.Text) Then
            productNumberInfo = rFQIssueDisp.GetProductInfo(ProductNumber.Text, Session(SESSION_ROLE_CODE).ToString)
            If productNumberInfo.Count <> 0 Then
                For Each productInfo As Join.RFQIssueDispProductInfo In productNumberInfo
                    CASNumber.Text = productInfo.CASNumber
                    ProductName.Text = productInfo.ProductName
                Next
            End If
        Else
            CASNumber.Text = String.Empty
            ProductName.Text = String.Empty
        End If

        st_ProductNumber = ProductNumber.Text
        CodeExtensionList.Items.Clear()
        If Not String.IsNullOrWhiteSpace(st_ProductNumber) Then
            ' Code Extensionのドロップダウンリストに値を設定するメソッドを呼び出す
            Common.SetCodeExtensionDropDownList(CodeExtensionList, st_ProductNumber)
        End If

        If Common.IsConfidentialItem(st_ProductNumber) Then
            If String.Equals(Common.GetRole(EnqUser.SelectedValue), Common.ROLE_WRITE_P) Then
                SetControl_EnqUser()
            End If

            If String.Equals(Common.GetRole(QuoUser.SelectedValue), Common.ROLE_WRITE_P) Then
                SetControl_QuoUser()
            End If
        End If

    End Sub

    ''' <summary>
    '''  SupplierCod 変更時イベント
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks>UpdatePanelコントロールによるAjaxの非同期ポストバックで実行されます。</remarks>
    Protected Sub SupplierCode_TextChanged(ByVal sender As Object, e As EventArgs) Handles SupplierCode.TextChanged

        Dim rFQIssueDisp As Join.RFQIssueDisp = New Join.RFQIssueDisp
        Dim supplierCodeInfo As List(Of Join.RFQIssueDispSupplierInfo) = New List(Of Join.RFQIssueDispSupplierInfo)
        If Not String.IsNullOrWhiteSpace(SupplierCode.Text) Then
            supplierCodeInfo = rFQIssueDisp.GetSupplierInfo(SupplierCode.Text, Session(SESSION_ROLE_CODE).ToString)

            If supplierCodeInfo.Count <> 0 Then
                For Each supplierInfo As Join.RFQIssueDispSupplierInfo In supplierCodeInfo
                    R3SupplierCode.Text = supplierInfo.R3SupplierCode
                    SupplierName.Text = supplierInfo.Name
                    SupplierCountry.Text = supplierInfo.CountryName
                Next
            End If
        Else
            R3SupplierCode.Text = String.Empty
            SupplierName.Text = String.Empty
            SupplierCountry.Text = String.Empty
        End If

        Dim st_SupplierCode As String = SupplierCode.Text
        SupplierContactPersonCodeList.Items.Clear()
        If Not String.IsNullOrWhiteSpace(st_SupplierCode) Then
            If IsNumeric(st_SupplierCode) Then
                ' SupplierContactPersonCodeのドロップダウンリストに値を設定するメソッドを呼び出す
                Common.SetSupplierContactPersonCodeList(SupplierContactPersonCodeList, st_SupplierCode)
            Else
                Msg.Text = ERR_INCORRECT_SUPPLIERCODE
            End If
        End If

    End Sub

    ''' <summary>
    '''  MakerCode 変更時イベント
    ''' </summary>
    ''' <param name="sender">ASP.NETの既定値</param>
    ''' <param name="e">ASP.NETの既定値</param>
    ''' <remarks>UpdatePanelコントロールによるAjaxの非同期ポストバックで実行されます。</remarks>
    Protected Sub MakerCode_TextChanged(ByVal sender As Object, e As EventArgs) Handles MakerCode.TextChanged

        Dim rFQIssueDisp As Join.RFQIssueDisp = New Join.RFQIssueDisp
        Dim makerCodeInfo As List(Of Join.RFQIssueDispMakerInfo) = New List(Of Join.RFQIssueDispMakerInfo)
        If Not String.IsNullOrWhiteSpace(MakerCode.Text) Then
            makerCodeInfo = rFQIssueDisp.GetMakerInfo(MakerCode.Text, Session(SESSION_ROLE_CODE).ToString)

            If makerCodeInfo.Count <> 0 Then
                For Each makerInfo As Join.RFQIssueDispMakerInfo In makerCodeInfo
                    SAPMakerCode.Text = makerInfo.S4SupplierCode
                    MakerName.Text = makerInfo.Name
                    MakerCountry.Text = makerInfo.CountryName
                Next
            End If
        Else
            SAPMakerCode.Text = String.Empty
            MakerName.Text = String.Empty
            MakerCountry.Text = String.Empty
        End If

    End Sub

End Class
﻿Imports System.Data.SqlClient
Imports Purchase.Common
Partial Public Class RFQIssue
    Inherits CommonPage
    Private DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    Private DBConn As New SqlConnection
    Private DBCommand As SqlCommand

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        DBConn.ConnectionString = DBConnectString.ConnectionString
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()
        If IsPostBack = False Then
            Call SetPostBackUrl()
            Call CheckPram()
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
        Dim DBReader As SqlDataReader
        Dim i As Integer = 0
        Dim i_ProductID As Integer = -1
        Dim i_SupplierCode As Integer = -1
        Dim i_MakerCode As Integer = -1
        Dim i_RFQNumber As Integer = -1
        Dim Enq_Quantity1 As Boolean = False
        Dim Enq_Quantity2 As Boolean = False
        Dim Enq_Quantity3 As Boolean = False
        Dim Enq_Quantity4 As Boolean = False
        Msg.Text = ""
        If Request.QueryString("Action") <> "Issue" Then
            Exit Sub
        End If

        If CheckRFQHeader() = False Then
            Exit Sub
        End If
        If CheckRFQLine(Enq_Quantity1, Enq_Quantity2, Enq_Quantity3, Enq_Quantity4) = False Then
            Exit Sub
        End If
        If CheckInsertColumn() = False Then
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
            DBCommand.Parameters.Add("@CreatedBy", SqlDbType.Int).Value = Integer.Parse(Session("UserID"))
            DBCommand.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = Integer.Parse(Session("UserID"))
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            'Header登録と登録時のIDを取得
            If DBReader.HasRows = True Then
                While DBReader.Read
                    'ID取得部分
                    i_RFQNumber = IIf(IsNumeric(DBReader("RFQNumber").ToString) = True, Integer.Parse(DBReader("RFQNumber").ToString), -99)
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

        Catch ex As Exception
            sqlTran.Rollback()
            Throw
        Finally
            DBCommand.Dispose()
        End Try
        Response.Redirect("RFQUpdate.aspx?RFQNumber=" & i_RFQNumber, False)
    End Sub

    Private Sub SetParamForRFQLine(ByVal param2 As SqlParameter, ByVal param3 As SqlParameter, ByVal param4 As SqlParameter, ByVal param5 As SqlParameter)
        param2.Value = EnqQuantity_1.Text
        param3.Value = EnqUnit_1.SelectedValue
        param4.Value = EnqPiece_1.Text
        param5.Value = IIf(SupplierItemNumber_1.Text = "", System.DBNull.Value, SupplierItemNumber_1.Text)
        DBCommand.ExecuteNonQuery()
    End Sub

    'Private Function RFQSupplierCheck(ByVal SupplierCode As String) As Boolean
    '    'Supplier 存在チェック
    '    RFQSupplierCheck = False
    '    Dim RFQConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    '    Dim RFQConn As New SqlConnection
    '    Dim RFQCom As SqlCommand
    '    Dim RFQRead As SqlDataReader
    '    Dim i As Integer

    '    If Integer.TryParse(SupplierCode, i) = False Then
    '        Exit Function
    '    End If
    '    RFQConn.ConnectionString = RFQConnectString.ConnectionString
    '    RFQConn.Open()
    '    RFQCom = RFQConn.CreateCommand()

    '    RFQCom.CommandText = "SELECT SupplierCode FROM Supplier WHERE (SupplierCode = @st_SupplierCode)"
    '    RFQCom.Parameters.Add("st_SupplierCode", SqlDbType.Int).Value = Integer.Parse(SupplierCode)
    '    RFQRead = RFQCom.ExecuteReader()
    '    RFQCom.Dispose()
    '    If RFQRead.HasRows = True Then
    '        RFQSupplierCheck = True
    '    End If
    '    RFQRead.Close()
    '    RFQConn.Close()

    'End Function


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
        'QuoUser.DataBind()
        '1行目に空行を追加する。
    End Sub
    Private Sub SetPostBackUrl()
        'Issueボタンクリック時にPostBackするActionを追記する。
        Issue.PostBackUrl = "~/RFQIssue.aspx?Action=Issue"
    End Sub
    Private Sub CheckPram()
        '他画面から取得するパラメータのチェック
        Dim st_ProductID As String = ""
        Dim st_SupplierCode As String = ""
        Dim DBReader As SqlDataReader
        If Request.QueryString("ProductID") <> "" Or Request.Form("ProductID") <> "" Then
            st_ProductID = IIf(Request.QueryString("ProductID") <> "", Request.QueryString("ProductID"), Request.Form("ProductID"))
            If IsNumeric(st_ProductID) Then
                DBCommand.CommandText = "Select ProductNumber, Name FROM Product WHERE ProductID = @st_ProductID"
                DBCommand.Parameters.Add("st_ProductID", SqlDbType.Int).Value = Integer.Parse(st_ProductID)
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
                DBCommand.Parameters.Add("st_SupplierCode", SqlDbType.Int).Value = Integer.Parse(st_SupplierCode)
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
    End Sub
    Private Sub InitDropDownList()
        'ドロップダウン初期設定
        EnqLocation.SelectedValue = Session("LocationCode").ToString
        EnqLocation.DataBind()
        EnqUser.SelectedValue = Session("UserID").ToString
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
            Msg.Text = "Enq-Location を設定して下さい"
            Return False
        End If
        If EnqUser.SelectedValue = "" Then
            Msg.Text = "Enq-User を設定して下さい"
            Return False
        End If
        If ProductNumber.Text = "" Then
            Msg.Text = "ProductNumber を設定して下さい"
            Return False
        End If
        If SupplierCode.Text = "" Then
            Msg.Text = "SupplierCode を設定して下さい"
            Return False
        End If
        If QuoLocation.SelectedValue = "" Then
            Msg.Text = "Quo-Location を設定して下さい"
            Return False
        End If
        If Purpose.SelectedValue = "" Then
            Msg.Text = "Purpose を設定して下さい"
            Return False
        End If
        If Integer.TryParse(SupplierCode.Text, i_Result) = False Then
            Msg.Text = "SupplierCode の設定が不正です"
            Return False
        End If
        If MakerCode.Text = "" Then
            'MakerCodeは省略可能
        ElseIf Integer.TryParse(MakerCode.Text, i_Result) Then
            '数値に変換できた場合の処理(小数点含まず)は正常
        Else
            '数値に変換できなかった場合の処理(小数点含む場合もこちら)は入力値不正
            Msg.Text = "MakerCode の設定が不正です"
            Return False
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

        Enq_Quantity2 = IsAllInputOfRFQList(EnqQuantity_2.Text, EnqUnit_2.SelectedValue, EnqPiece_2.Text)
        Dim bo_UnLine_2 = IsAllNullOfRFQList(EnqQuantity_2.Text, EnqUnit_2.SelectedValue, EnqPiece_2.Text)
        If Enq_Quantity2 = False And bo_UnLine_2 = False Then
            Bo_UnLine = True
        End If

        Enq_Quantity3 = IsAllInputOfRFQList(EnqQuantity_3.Text, EnqUnit_3.SelectedValue, EnqPiece_3.Text)
        Dim bo_UnLine_3 = IsAllNullOfRFQList(EnqQuantity_3.Text, EnqUnit_3.SelectedValue, EnqPiece_3.Text)
        If Enq_Quantity3 = False And bo_UnLine_3 = False Then
            Bo_UnLine = True
        End If

        Enq_Quantity4 = IsAllInputOfRFQList(EnqQuantity_4.Text, EnqUnit_4.SelectedValue, EnqPiece_4.Text)
        Dim bo_UnLine_4 = IsAllNullOfRFQList(EnqQuantity_4.Text, EnqUnit_4.SelectedValue, EnqPiece_4.Text)
        If Enq_Quantity4 = False And bo_UnLine_4 = False Then
            Bo_UnLine = True
        End If

        If Enq_Quantity1 = False And Enq_Quantity2 = False And _
            Enq_Quantity3 = False And Enq_Quantity4 = False Then

            Msg.Text = "Enq-Quantity を設定して下さい"
            Return False
        End If

        If Bo_UnLine = True Then
            Msg.Text = "Enq-Quantity の設定が不正です"
            Return False
        End If
        Return True
    End Function
    Private Function CheckInsertColumn() As Boolean
        'Insert内容の入力チェック
        '入力内容のチェック
        Dim st_Product As String = "Product"
        Dim st_ProductKey As String = "ProductNumber"
        Dim st_Supplier As String = "Supplier"
        Dim st_SupplierKey As String = "SupplierCode"
        'ProductNumberのチェック
        If ExistenceConfirmation(st_Product, st_ProductKey, ProductNumber.Text) = False Then
            Msg.Text = "ProductNumber の設定が不正です"
            Return False
        End If
        'Supplierのチェック
        If ExistenceConfirmation(st_Supplier, st_SupplierKey, SupplierCode.Text) = False Then
            Msg.Text = "SupplierCode の設定が不正です"
            Return False
        End If
        'Makerのチェック
        If MakerCode.Text <> "" Then
            If ExistenceConfirmation(st_Supplier, st_SupplierKey, MakerCode.Text) = False Then
                Msg.Text = "MakerCode の設定が不正です"
                Return False
            End If
        End If
        Return True
    End Function
    Private Function ExistenceConfirmation(ByVal TableName As String, ByVal TableField As String, ByVal CheckData As Object) As Boolean
        'DB汎用存在確認チェック
        Dim st_SQLCommand As String = String.Empty
        st_SQLCommand = "SELECT COUNT(*) AS RecordCount FROM " & TableName & " WHERE " & TableField & " = @CheckData"
        'st_SQLCommand = "SELECT COUNT(*) AS RecordCount FROM @TableName WHERE @TableField = @CheckData"
        Try
            Using DBConn As New SqlClient.SqlConnection(DB_CONNECT_STRING), _
            DBCommand As SqlCommand = DBConn.CreateCommand()
                DBConn.Open()
                Dim i As Integer = 0
                DBCommand.CommandText = st_SQLCommand
                'DBCommand.Parameters.AddWithValue("TableName", TableName)
                'DBCommand.Parameters.AddWithValue("TableField", TableField)
                DBCommand.Parameters.AddWithValue("CheckData", CheckData)

                Using DBReader As SqlClient.SqlDataReader = DBCommand.ExecuteReader()
                    If DBReader.HasRows = True Then
                        While DBReader.Read
                            If DBReader("RecordCount").ToString > 0 Then
                                Return True
                            End If
                        End While
                    End If
                End Using
            End Using
        Catch ex As Exception
            Throw
        End Try
        Return False
    End Function
End Class
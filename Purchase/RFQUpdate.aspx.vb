Imports System.Data.SqlClient
Partial Public Class RFQUpdate
    Inherits CommonPage
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    Public DBConn As New System.Data.SqlClient.SqlConnection
    Public DBCommand As System.Data.SqlClient.SqlCommand
    Public DBAdapter As System.Data.SqlClient.SqlDataAdapter
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        DBConn.ConnectionString = DBConnectString.ConnectionString
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        If IsPostBack = False Then
            Call FormDataSet()

        Else
            'ReadOnly項目の再設定
            R3SupplierCode.Text = Request.Form("R3SupplierCode").ToString
            SupplierName.Text = Request.Form("SupplierName").ToString
            SupplierCountry.Text = Request.Form("SupplierCountry").ToString
            MakerName.Text = Request.Form("MakerName").ToString
            MakerCountry.Text = Request.Form("MakerCountry").ToString
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack = False Then
            Update.PostBackUrl = "~/RFQUpdate.aspx?Action=Update"
            Close.PostBackUrl = "~/RFQUpdate.aspx?Action=Close"
        End If
    End Sub

    Private Sub FormDataSet()
        Dim DS As DataSet = New DataSet
        Dim st_RFQNumber As String
        Dim testRFQNumber As String = "1000000030"

        '        If Request.QueryString("RFQNumber") <> "" Or Request.Form("RFQNumber") <> "" Then
        If Request.QueryString("RFQNumber") <> "" Or Request.Form("RFQNumber") <> "" Or testRFQNumber <> "" Then
            st_RFQNumber = IIf(Request.QueryString("RFQNumber") <> "", Request.QueryString("RFQNumber"), Request.Form("RFQNumber"))
            If st_RFQNumber = "" Then       'test用
                st_RFQNumber = testRFQNumber
            End If
            If IsNumeric(st_RFQNumber) Then
                DBCommand = New SqlCommand("Select * From v_RFQHeader Where RFQNumber = @i_RFQNumber", DBConn)
                DBCommand.Parameters.Add("i_RFQNumber", SqlDbType.Int).Value = CInt(st_RFQNumber)
                DBAdapter = New SqlDataAdapter
                DBAdapter.SelectCommand = DBCommand

                DBAdapter.Fill(DS, "RFQHeader")
                DBCommand.Dispose()

                If DS.Tables("RFQHeader").Rows.Count = 0 Then
                    'RFQNumber 不正
                    Msg.Text = "RFQの情報が見つかりません。"
                    Exit Sub
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
                ShippingHandlingFee.Text = DS.Tables("RFQHeader").Rows(0)("ShippingHandlingFee").ToString
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
                QuoUser.DataBind()
                Comment.Text = DS.Tables("RFQHeader").Rows(0)("Comment").ToString
                'Hidden
                QuotedDate.Value = DS.Tables("RFQHeader").Rows(0)("QuotedDate").ToString
                'Line
                DBCommand = New SqlCommand("Select * From v_RFQLine Where RFQNumber = @i_RFQNumber Order by RFQLineNumber", DBConn)
                DBCommand.Parameters.Add("i_RFQNumber", SqlDbType.Int).Value = CInt(st_RFQNumber)
                DBAdapter.SelectCommand = DBCommand

                DBAdapter.Fill(DS, "RFQLine")
                DBCommand.Dispose()

                If DS.Tables("RFQLine").Rows.Count = 0 Then
                    'RFQNumber 不正
                    Msg.Text = "RFQの明細情報が見つかりません。"
                    Exit Sub
                End If

                Dim i As Integer
                For i = 0 To DS.Tables("RFQLine").Rows.Count - 1
                    Select Case i
                        Case 0
                            EnqQuantity_1.Text = CSng(DS.Tables("RFQLine").Rows(i).Item("EnqQuantity").ToString)
                            EnqUnit_1.Text = DS.Tables("RFQLine").Rows(i).Item("EnqUnitCode").ToString
                            EnqPiece_1.Text = DS.Tables("RFQLine").Rows(i).Item("EnqPiece").ToString
                            Incoterms_1.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("IncotermsCode").ToString
                            Currency_1.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("CurrencyCode").ToString
                            UnitPrice_1.Text = DS.Tables("RFQLine").Rows(i).Item("UnitPrice").ToString
                            DeliveryTerm_1.Text = DS.Tables("RFQLine").Rows(i).Item("DeliveryTerm").ToString
                            QuoPer_1.Text = DS.Tables("RFQLine").Rows(i).Item("QuoPer").ToString
                            Purity_1.Text = DS.Tables("RFQLine").Rows(i).Item("Purity").ToString
                            QuoUnit_1.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("QuoUnitCode").ToString
                            QMMethod_1.Text = DS.Tables("RFQLine").Rows(i).Item("QMMethod").ToString
                            LeadTime_1.Text = DS.Tables("RFQLine").Rows(i).Item("LeadTime").ToString
                            Packing_1.Text = DS.Tables("RFQLine").Rows(i).Item("Packing").ToString
                            SupplierItemNumber_1.Text = DS.Tables("RFQLine").Rows(i).Item("SupplierItemNumber").ToString
                            NoOfferReason_1.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("NoOfferReasonCode").ToString
                            POIssue_1.Visible = True
                            POIssue_1.NavigateUrl = "./POIssue.aspx?RFQLineNumber=" & DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                        Case 1
                            EnqQuantity_2.Text = CSng(DS.Tables("RFQLine").Rows(i).Item("EnqQuantity").ToString)
                            EnqUnit_2.Text = DS.Tables("RFQLine").Rows(i).Item("EnqUnitCode").ToString
                            EnqPiece_2.Text = DS.Tables("RFQLine").Rows(i).Item("EnqPiece").ToString
                            Incoterms_2.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("IncotermsCode").ToString
                            Currency_2.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("CurrencyCode").ToString
                            UnitPrice_2.Text = DS.Tables("RFQLine").Rows(i).Item("UnitPrice").ToString
                            DeliveryTerm_2.Text = DS.Tables("RFQLine").Rows(i).Item("DeliveryTerm").ToString
                            QuoPer_2.Text = DS.Tables("RFQLine").Rows(i).Item("QuoPer").ToString
                            Purity_2.Text = DS.Tables("RFQLine").Rows(i).Item("Purity").ToString
                            QuoUnit_2.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("QuoUnitCode").ToString
                            QMMethod_2.Text = DS.Tables("RFQLine").Rows(i).Item("QMMethod").ToString
                            LeadTime_2.Text = DS.Tables("RFQLine").Rows(i).Item("LeadTime").ToString
                            Packing_2.Text = DS.Tables("RFQLine").Rows(i).Item("Packing").ToString
                            SupplierItemNumber_2.Text = DS.Tables("RFQLine").Rows(i).Item("SupplierItemNumber").ToString
                            NoOfferReason_2.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("NoOfferReasonCode").ToString
                            POIssue_2.Visible = True
                            POIssue_2.NavigateUrl = "./POIssue.aspx?RFQLineNumber=" & DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                        Case 2
                            EnqQuantity_3.Text = CSng(DS.Tables("RFQLine").Rows(i).Item("EnqQuantity").ToString)
                            EnqUnit_3.Text = DS.Tables("RFQLine").Rows(i).Item("EnqUnitCode").ToString
                            EnqPiece_3.Text = DS.Tables("RFQLine").Rows(i).Item("EnqPiece").ToString
                            Incoterms_3.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("IncotermsCode").ToString
                            Currency_3.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("CurrencyCode").ToString
                            UnitPrice_3.Text = DS.Tables("RFQLine").Rows(i).Item("UnitPrice").ToString
                            DeliveryTerm_3.Text = DS.Tables("RFQLine").Rows(i).Item("DeliveryTerm").ToString
                            QuoPer_3.Text = DS.Tables("RFQLine").Rows(i).Item("QuoPer").ToString
                            Purity_3.Text = DS.Tables("RFQLine").Rows(i).Item("Purity").ToString
                            QuoUnit_3.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("QuoUnitCode").ToString
                            QMMethod_3.Text = DS.Tables("RFQLine").Rows(i).Item("QMMethod").ToString
                            LeadTime_3.Text = DS.Tables("RFQLine").Rows(i).Item("LeadTime").ToString
                            Packing_3.Text = DS.Tables("RFQLine").Rows(i).Item("Packing").ToString
                            SupplierItemNumber_3.Text = DS.Tables("RFQLine").Rows(i).Item("SupplierItemNumber").ToString
                            NoOfferReason_3.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("NoOfferReasonCode").ToString
                            POIssue_3.Visible = True
                            POIssue_3.NavigateUrl = "./POIssue.aspx?RFQLineNumber=" & DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                        Case 3
                            EnqQuantity_4.Text = CSng(DS.Tables("RFQLine").Rows(i).Item("EnqQuantity").ToString)
                            EnqUnit_4.Text = DS.Tables("RFQLine").Rows(i).Item("EnqUnitCode").ToString
                            EnqPiece_4.Text = DS.Tables("RFQLine").Rows(i).Item("EnqPiece").ToString
                            Incoterms_4.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("IncotermsCode").ToString
                            Currency_4.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("CurrencyCode").ToString
                            UnitPrice_4.Text = DS.Tables("RFQLine").Rows(i).Item("UnitPrice").ToString
                            DeliveryTerm_4.Text = DS.Tables("RFQLine").Rows(i).Item("DeliveryTerm").ToString
                            QuoPer_4.Text = DS.Tables("RFQLine").Rows(i).Item("QuoPer").ToString
                            Purity_4.Text = DS.Tables("RFQLine").Rows(i).Item("Purity").ToString
                            QuoUnit_4.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("QuoUnitCode").ToString
                            QMMethod_4.Text = DS.Tables("RFQLine").Rows(i).Item("QMMethod").ToString
                            LeadTime_4.Text = DS.Tables("RFQLine").Rows(i).Item("LeadTime").ToString
                            Packing_4.Text = DS.Tables("RFQLine").Rows(i).Item("Packing").ToString
                            SupplierItemNumber_4.Text = DS.Tables("RFQLine").Rows(i).Item("SupplierItemNumber").ToString
                            NoOfferReason_4.SelectedValue = DS.Tables("RFQLine").Rows(i).Item("NoOfferReasonCode").ToString
                            POIssue_4.Visible = True
                            POIssue_4.NavigateUrl = "./POIssue.aspx?RFQLineNumber=" & DS.Tables("RFQLine").Rows(i).Item("RFQLineNumber").ToString
                        Case Else
                            '処理無し
                    End Select
                Next
                DS.Clear()

            End If
        End If
    End Sub
    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
    End Sub
    Protected Sub SpecSheet_CheckedChanged(ByVal sender As Object, ByVal e As EventArgs) Handles SpecSheet.CheckedChanged
        If SpecSheet.Checked = True Then
            SpecSheet.Text = "yes"
        Else
            SpecSheet.Text = "no"
        End If

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

    Protected Sub Update_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Update.Click
        Dim RFQStatusCode As String
        If Request.QueryString("Action") <> "Update" Then
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

        If ItemCheck() = False Then
            '入力された項目の型をチェックする(DB登録時にエラーになるもののみ)
            Exit Sub
        End If
        'RFQHeader の更新

        DBCommand.Parameters.Clear()
        If RFQStatus.SelectedValue = "" Then
            RFQStatusCode = ""
        Else
            RFQStatusCode = " RFQStatusCode = @RFQStatusCode "
            DBCommand.Parameters.Add("@RFQStatusCode", SqlDbType.NVarChar).Value = RFQStatus.SelectedValue
        End If

        'Update RFQHeader
        'SET                          QuoUserID = @QuoUserID, SupplierCode =, MakerCode =, SpecSheet =, Specification =, SupplierContactPerson =, 
        '                                  SupplierItemName =, ShippingHandlingFee =, ShippingHandlingCurrencyCode =, PaymentTermCode =, Comment =, UpdatedBy =, 
        'UpdateDate =











        If EnqQuantity_1.Text <> "" Then
            'RFQIssueで登録されたデータのみ更新可

        End If

        If EnqQuantity_2.Text <> "" Then

        End If
        If EnqQuantity_3.Text <> "" Then

        End If
        If EnqQuantity_4.Text <> "" Then

        End If



    End Sub

    Protected Sub Close_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Close.Click
        If Request.QueryString("Action") <> "Close" Then
            Exit Sub
        End If
        DBCommand.CommandText = "UPDATE RFQHeader SET RFQStatusCode = 'C' WHERE (RFQNumber = @RFQNumber)"
        DBCommand.Parameters.Add("@RFQNumber", SqlDbType.Int).Value = CInt(RFQNumber.Text)
        DBCommand.ExecuteNonQuery()
        DBCommand.Parameters.Clear()
        DBCommand.Dispose()
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
    Private Function ItemCheck() As Boolean
        Dim i As Integer

        ItemCheck = False
        '型チェック
        If ShippingHandlingFee.Text <> "" Then
            If Decimal.TryParse(ShippingHandlingFee.Text, i) = False Then
                Msg.Text = "ShippingHandlingFee の設定が不正です"
                Exit Function
            End If
        End If

        If UnitPrice_1.Text <> "" Then
            If Decimal.TryParse(UnitPrice_1.Text, i) = False Then
                Msg.Text = "UnitPrice の設定が不正です"
                Exit Function
            End If
        End If
        If UnitPrice_2.Text <> "" Then
            If Decimal.TryParse(UnitPrice_2.Text, i) = False Then
                Msg.Text = "UnitPrice の設定が不正です"
                Exit Function
            End If
        End If
        If UnitPrice_3.Text <> "" Then
            If Decimal.TryParse(UnitPrice_3.Text, i) = False Then
                Msg.Text = "UnitPrice の設定が不正です"
                Exit Function
            End If
        End If
        If UnitPrice_4.Text <> "" Then
            If Decimal.TryParse(UnitPrice_4.Text, i) = False Then
                Msg.Text = "UnitPrice の設定が不正です"
                Exit Function
            End If
        End If

        If QuoPer_1.Text <> "" Then
            If Decimal.TryParse(QuoPer_1.Text, i) = False Then
                Msg.Text = "Quoper の設定が不正です"
                Exit Function
            End If
        End If
        If QuoPer_2.Text <> "" Then
            If Decimal.TryParse(QuoPer_2.Text, i) = False Then
                Msg.Text = "Quoper の設定が不正です"
                Exit Function
            End If
        End If
        If QuoPer_3.Text <> "" Then
            If Decimal.TryParse(QuoPer_3.Text, i) = False Then
                Msg.Text = "Quoper の設定が不正です"
                Exit Function
            End If
        End If
        If QuoPer_4.Text <> "" Then
            If Decimal.TryParse(QuoPer_4.Text, i) = False Then
                Msg.Text = "Quoper の設定が不正です"
                Exit Function
            End If
        End If
        ItemCheck = True

    End Function
End Class
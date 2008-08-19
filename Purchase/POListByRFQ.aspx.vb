Public Partial Class POListByRFQ
    Inherits CommonPage

    Protected st_RFQNumber As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' コントロール初期化
        Msg.Text = ""

        'パラメータ取得
        st_RFQNumber = IIf(String.IsNullOrEmpty(Request.QueryString("RFQNumber")), "", Request.QueryString("RFQNumber"))

        ' 空白除去
        st_RFQNumber = st_RFQNumber.Trim

        ' パラメータチェック
        If (String.IsNullOrEmpty(st_RFQNumber)) Or (Not Regex.IsMatch(st_RFQNumber, "^[0-9]+$")) Then
            st_RFQNumber = ""
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        ' PO データを取得する
        SrcPO.SelectParameters.Clear()
        SrcPO.SelectParameters.Add("RFQNumber", st_RFQNumber)
        SrcPO.SelectCommand = _
              "SELECT " _
            & "  PONumber, StatusChangeDate, Status, ProductNumber, ProductName, " _
            & "  PODate, POUserName, POLocationName, SupplierName, MakerName, " _
            & "  DeliveryDate, OrderQuantity, OrderUnitCode, CurrencyCode, " _
            & "  UnitPrice, PerQuantity, PerUnitCode " _
            & "FROM " _
            & "  v_PO " _
            & "WHERE " _
            & "  RFQNumber = @RFQNumber "

    End Sub

End Class
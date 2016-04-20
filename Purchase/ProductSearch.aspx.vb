Imports Purchase.Common

Partial Public Class ProductSearch
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            UnDsp_ProductList()
        End If
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        Msg.Text = String.Empty
        '[Search実行可能確認]----------------------------------------------------------
        If Action.Value <> "Search" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        '[入力ProductNumberの正規化]---------------------------------------------------
        ProductNumber.Text = StrConv(ProductNumber.Text, VbStrConv.Narrow)
        ProductNumber.Text = UCase(ProductNumber.Text)
        If ProductNumber.Text = "" Then
            Msg.Text = "Product Number" + ERR_REQUIRED_FIELD
            UnDsp_ProductList()
            Exit Sub
        End If

        '[ProductListにデータ設定]-----------------------------------------------------
        Dim sqlStr As StringBuilder = New StringBuilder
        sqlStr.AppendLine("SELECT")
        sqlStr.AppendLine("  P.ProductNumber,")
        sqlStr.AppendLine("  CASE WHEN NOT P.QuoName IS NULL THEN P.QuoName ELSE P.Name END AS ProductName,")
        sqlStr.AppendLine("  './ProductSetting.aspx?Action=Edit&ProductID=' + RTRIM(LTRIM(Str(P.ProductID))) AS Url")
        sqlStr.AppendLine("FROM")
        sqlStr.AppendLine("  Product AS P")
        sqlStr.AppendLine("WHERE")
        sqlStr.AppendLine("  (P.ProductNumber = '" + SafeSqlLiteral(ProductNumber.Text) + "' OR P.CASNumber = '" + SafeSqlLiteral(ProductNumber.Text) + "')")
        '権限ロールに従い極秘品を除外する
        If Session(SESSION_ROLE_CODE).ToString = ROLE_WRITE_P OrElse Session(SESSION_ROLE_CODE).ToString = ROLE_READ_P Then
            sqlStr.AppendLine("  AND NOT EXISTS (")
            sqlStr.AppendLine("    SELECT 1")
            sqlStr.AppendLine("    FROM")
            sqlStr.AppendLine("      v_CONFIDENTIAL AS C")
            sqlStr.AppendLine("    WHERE")
            sqlStr.AppendLine("      C.isCONFIDENTIAL = 1")
            sqlStr.AppendLine("      AND C.ProductID = P.ProductID")
            sqlStr.AppendLine("  )")
        End If

        ProductList.Visible = True
        SrcProduct.SelectCommand = sqlStr.ToString
        If ProductNumber.Text = "" Then
            UnDsp_ProductList()
        End If
    End Sub

    Public Sub UnDsp_ProductList()
        SrcProduct.SelectCommand = ""
        ProductList.Visible = False
    End Sub
End Class
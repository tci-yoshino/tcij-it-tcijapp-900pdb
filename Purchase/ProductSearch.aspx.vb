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
        ProductList.Visible = True
        SrcProduct.SelectCommand = "SELECT ProductNumber, CASE WHEN NOT Product.QuoName IS NULL THEN Product.QuoName ELSE Product.Name END AS ProductName, './ProductSetting.aspx?Action=Edit&ProductID=' + Rtrim(Ltrim(Str(ProductID))) AS Url FROM dbo.Product WHERE (ProductNumber = '" + SafeSqlLiteral(ProductNumber.Text) + "') OR (CASNumber = '" + SafeSqlLiteral(ProductNumber.Text) + "')"
        If ProductNumber.Text = "" Then
            UnDsp_ProductList()
        End If
    End Sub

    Public Sub UnDsp_ProductList()
        SrcProduct.SelectCommand = ""
        ProductList.Visible = False
    End Sub
End Class
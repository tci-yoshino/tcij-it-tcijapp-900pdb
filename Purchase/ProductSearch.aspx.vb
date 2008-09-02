Public Partial Class ProductSearch
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            SrcProduct.SelectCommand = ""
            ProductList.Visible = False
        End If
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        '[入力ProductNumberの正規化]---------------------------------------------------
        ProductNumber.Text = StrConv(ProductNumber.Text, VbStrConv.Narrow)
        ProductNumber.Text = UCase(ProductNumber.Text)

        If ProductNumber.Text = "" Then
            Msg.Text = "Product Number" + Common.ERR_REQUIRED_FIELD
            Exit Sub
        End If

        ProductList.Visible = True
        Dim st_SqlStr As String = " "
        SrcProduct.SelectCommand = "SELECT ProductNumber, CASE WHEN NOT Product.QuoName IS NULL THEN Product.QuoName ELSE Product.Name END AS ProductName, './ProductSetting.aspx?Action=Edit&ProductID=' + Rtrim(Ltrim(Str(ProductID))) AS Url FROM dbo.Product "
        If ProductNumber.Text.ToString <> "" Then st_SqlStr = st_SqlStr + "WHERE (ProductNumber = '" + Common.SafeSqlLiteral(ProductNumber.Text) + "')"
        If CASNumber.Text.ToString <> "" Then
            If Right(st_SqlStr, 1) = ")" Then
                st_SqlStr = st_SqlStr + " AND (CASNumber = '" + Common.SafeSqlLiteral(CASNumber.Text) + "')"
            Else
                st_SqlStr = st_SqlStr + "WHERE (CASNumber = '" + Common.SafeSqlLiteral(CASNumber.Text) + "')"
            End If
        End If

        '[検索項目すべて指定しない場合は結果無しとする]-------------------------------
        If st_SqlStr = " " Then st_SqlStr = ""
        If st_SqlStr = "" Then
            SrcProduct.SelectCommand = ""
        Else
            SrcProduct.SelectCommand = SrcProduct.SelectCommand + st_SqlStr
        End If
    End Sub
End Class
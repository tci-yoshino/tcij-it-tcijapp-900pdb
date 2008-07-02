Public Partial Class ProductSearch
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SrcProduct.SelectCommand = ""
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        Dim st_SqlStr As String = " "
        SrcProduct.SelectCommand = "SELECT ProductNumber, CASE WHEN NOT Product.QuoName IS NULL THEN Product.QuoName ELSE Product.Name END AS ProductName, './ProductSetting.aspx?Action=Edit&ProductID=' + Rtrim(Ltrim(Str(ProductID))) AS Url FROM dbo.Product "
        If ProductNumber.Text.ToString <> "" Then st_SqlStr = st_SqlStr + "WHERE (ProductNumber = '" + ProductNumber.Text.ToString + "')"
        If CASNumber.Text.ToString <> "" Then
            If Right(st_SqlStr, 1) = ")" Then
                st_SqlStr = st_SqlStr + " AND (CASNumber = '" + CASNumber.Text.ToString + "')"
            Else
                st_SqlStr = st_SqlStr + "WHERE (CASNumber = '" + CASNumber.Text.ToString + "')"
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
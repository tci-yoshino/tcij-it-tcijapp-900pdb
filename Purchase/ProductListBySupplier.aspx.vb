Imports Purchase.Common

Partial Public Class ProductListBySupplier
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader
    Public Url As String = ""
    Public AddUrl As String = ""
    Public ImpUrl As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '[DBの接続]-----------------------------------------------------------------------
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        If Not IsPostBack Then
            '[QueryString("Supplier")のチェック]----------------------------------------------
            If Request.QueryString("Supplier") = "" Then
                SrcSupplierProduct.SelectCommand = ""
                SupplierProductList.DataBind()
                Msg.Text = Common.ERR_INVALID_PARAMETER
                Exit Sub
            End If

            SupplierCode.Text = Request.QueryString("Supplier")
            DBCommand.CommandText = "SELECT Name3,Name4 FROM Supplier WHERE (SupplierCode = '" & SupplierCode.Text.ToString & "')"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                If Not TypeOf DBReader("Name3") Is DBNull Then SupplierName.Text = DBReader("Name3")
                If Not TypeOf DBReader("Name4") Is DBNull Then
                    If SupplierName.Text = String.Empty Then
                        SupplierName.Text = DBReader("Name4")
                    Else
                        SupplierName.Text = SupplierName.Text & " " & DBReader("Name4")
                    End If
                End If
            End If
            DBReader.Close()

            Dim strSql As StringBuilder = New StringBuilder
            strSql.AppendLine("SELECT")
            strSql.AppendLine("  P.ProductID,")
            strSql.AppendLine("  P.ProductNumber,")
            strSql.AppendLine("  CASE WHEN NOT P.QuoName IS NULL THEN P.QuoName ELSE P.Name END AS ProductName,")
            strSql.AppendLine("  SP.SupplierItemNumber,")
            strSql.AppendLine("  SP.Note,")
            strSql.AppendLine("  SP.UpdateDate,")
            strSql.AppendLine("  './SuppliersProductSetting.aspx?Action=Edit&Supplier=" + SupplierCode.Text.ToString + "&Product='+RTRIM(LTRIM(STR(P.ProductID))) AS Url,")
            strSql.AppendLine("  ISNULL(C.isCONFIDENTIAL, 0) AS isCONFIDENTIAL")
            strSql.AppendLine("FROM")
            strSql.AppendLine("  Supplier_Product AS SP")
            strSql.AppendLine("    LEFT OUTER JOIN Product AS P ON SP.ProductID = P.ProductID")
            strSql.AppendLine("    LEFT OUTER JOIN v_CONFIDENTIAL AS C ON C.ProductID = SP.ProductID")
            strSql.AppendLine("WHERE")
            strSql.AppendLine("  SP.SupplierCode = '" & SupplierCode.Text.ToString & "'")

            '権限ロールに従い極秘品を除外する
            If Session(SESSION_ROLE_CODE).ToString = ROLE_WRITE_P OrElse Session(SESSION_ROLE_CODE).ToString = ROLE_READ_P Then
                strSql.AppendLine("  AND C.isCONFIDENTIAL = 0")
            End If

            SrcSupplierProduct.SelectCommand = strSql.ToString
            SupplierProductList.DataBind()
        End If
    End Sub

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        If Request.Form("Action") = "Delete" Then
            '[指定レコード削除]-----------------------------------------------------------------
            DBCommand.CommandText = "DELETE Supplier_Product WHERE SupplierCode=" + Request.QueryString("Supplier") + " AND ProductID=" + Request.Form("ProductID")
            DBCommand.ExecuteNonQuery()
            Url = "./ProductListBySupplier.aspx?Supplier=" & SupplierCode.Text.ToString
            Response.Redirect(Url)
        End If

        '[New Suppliers ProductのURL設定]------------------------------------------------------------
        AddUrl = "./SuppliersProductSetting.aspx?Supplier=" & SupplierCode.Text.ToString

        '[Excel ImportのURL設定]---------------------------------------------------------------------
        ImpUrl = "./SuppliersProductImport.aspx?Supplier=" & SupplierCode.Text.ToString
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
    End Sub
End Class


Public Partial Class RFQIssue
    Inherits CommonPage
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    Public DBConn As New System.Data.SqlClient.SqlConnection   'データベースコネクション
    Public DBCommand As System.Data.SqlClient.SqlCommand       'データベースコマンド
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim st_ProductID As String = ""
        Dim st_SupplierCode As String = ""
        Dim DBReader As System.Data.SqlClient.SqlDataReader     'データリーダー
        DBConn.ConnectionString = DBConnectString.ConnectionString
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()
        'PostBackがFalse時もパラメータのチェックが必要
        'パラメータチェック
        If Request.QueryString("ProductID") <> "" Then
            st_ProductID = Request.QueryString("ProductID")
            If IsNumeric(st_ProductID) Then
                DBCommand.CommandText = "Select ProductNumber, Name FROM Product WHERE ProductID = @st_ProductID"
                DBCommand.Parameters.Add("st_ProductID", SqlDbType.Int).Value = CInt(st_ProductID)
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.HasRows = True Then
                    While DBReader.Read
                        ProductNumber.Text = DBReader("ProductNumber").ToString
                        ProductName.Text = DBReader("Name").ToString
                    End While
                    ProductNumber.ReadOnly = True
                    ProductName.ReadOnly = True
                    ProductNumber.CssClass = "readonly"
                    ProductName.CssClass = "readonly"
                Else

                End If
                DBReader.Close()
            End If
        End If
        'こっからまた後で考える。
        If Request.QueryString("SupplierCode") <> "" Then
            st_SupplierCode = Request.QueryString("SupplierCode")
            If IsNumeric(st_ProductID) Then
                DBCommand.CommandText = "Select ProductNumber, Name FROM Product WHERE ProductID = @st_ProductID"
                DBCommand.Parameters.Add("st_ProductID", SqlDbType.Int).Value = CInt(st_ProductID)
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.HasRows = True Then
                    While DBReader.Read
                        ProductNumber.Text = DBReader("ProductNumber").ToString
                        ProductName.Text = DBReader("Name").ToString
                    End While
                    ProductNumber.ReadOnly = True
                    ProductName.ReadOnly = True
                    ProductNumber.CssClass = "readonly"
                    ProductName.CssClass = "readonly"
                Else

                End If
                DBReader.Close()
            End If
        End If



            'If IsPostBack = True Then

            '    If Request.QueryString("Action") = "Issue" Then

            '    Else

            '    End If
            'Else
            '    TCICommon.Func.ConvertDate(Now, "JP", "US", a)
            'End If
    End Sub

    Protected Sub EnqLocation_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles EnqLocation.SelectedIndexChanged
        'ドロップダウンリストの項目を入れ替える。

    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack = False Then
            Issue.PostBackUrl = "~/RFQIssue.aspx?Action=Issue"

        End If
        If Request.QueryString("ProductID").Length > 0 Then

        End If

        'パラメータ ProductID を受け取った場合
        'テキストボックス ProductNumber，ProductName を ReadOnly="true" CssClass="readonly" ProductNumber 横の虫眼鏡は非表示にする。 
        'パラメータ SupplierCode が渡されたとき
        'テキストボックス SupplierCode，R3SupplierCode，SupplierName，SupplierCountry を ReadOnly="true" CssClass="readonly" SupplierCode 横の虫眼鏡は非表示にする。 
        'それぞれのパラメータが渡されない場合は ReadOnly CssClass の指定は変更しない。 

    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()

    End Sub
End Class
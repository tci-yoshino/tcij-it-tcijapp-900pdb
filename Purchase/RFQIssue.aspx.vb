Public Partial Class RFQIssue
    Inherits CommonPage
    Public DBConnectString As ConnectionStringSettings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
    Public DBConn As New System.Data.SqlClient.SqlConnection
    Public DBCommand As System.Data.SqlClient.SqlCommand

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Dim st_ProductID As String = ""
        Dim st_SupplierCode As String = ""
        Dim DBReader As System.Data.SqlClient.SqlDataReader     'データリーダー
        DBConn.ConnectionString = DBConnectString.ConnectionString
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()
        If IsPostBack = False Then
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
                        ProductSelect.Visible = False
                    End If
                    DBReader.Close()
                End If
            End If
            If Request.QueryString("SupplierCode") <> "" Then
                st_SupplierCode = Request.QueryString("SupplierCode")
                If IsNumeric(st_SupplierCode) Then
                    DBCommand.CommandText = "SELECT R3SupplierCode, ISNULL(Name3, '') + ISNULL(Name4, '') AS SupplierName, CountryCode FROM Supplier WHERE SupplierCode = @st_SupplierCode"
                    DBCommand.Parameters.Add("st_SupplierCode", SqlDbType.Int).Value = CInt(st_SupplierCode)
                    DBReader = DBCommand.ExecuteReader()
                    DBCommand.Dispose()
                    If DBReader.HasRows = True Then
                        While DBReader.Read
                            R3SupplierCode.Text = DBReader("R3SupplierCode").ToString
                            SupplierName.Text = DBReader("SupplierName").ToString
                            SupplierCountry.Text = DBReader("CountryCode").ToString
                        End While
                        R3SupplierCode.ReadOnly = True
                        R3SupplierCode.CssClass = "readonly"
                        SupplierName.ReadOnly = True
                        SupplierName.CssClass = "readonly"
                        SupplierCountry.ReadOnly = True
                        SupplierCountry.CssClass = "readonly"
                        SupplierSelect.Visible = False
                    End If
                    DBReader.Close()
                End If
            End If
        Else
            'ReadOnly項目の再設定
            ProductName.Text = Request.Form("ProductName").ToString
            R3SupplierCode.Text = Request.Form("R3SupplierCode").ToString
            SupplierName.Text = Request.Form("SupplierName").ToString
            SupplierCountry.Text = Request.Form("SupplierCountry").ToString
            MakerName.Text = Request.Form("MakerName").ToString
            MakerCountry.Text = Request.Form("MakerCountry").ToString
        End If
    End Sub

    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If IsPostBack = False Then
            Issue.PostBackUrl = "~/RFQIssue.aspx?Action=Issue"
        End If
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()

    End Sub

    Protected Sub Issue_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Issue.Click
        Dim st_Indispensability As String = ""
        If Request.QueryString("Action") <> "Issue" Then
            Exit Sub
        End If
        '必須入力項目チェック
        If EnqLocation.SelectedValue = "" Then
            st_Indispensability = st_Indispensability & "Enq-Location "
        End If
        If EnqUser.SelectedValue = "" Then
            st_Indispensability = st_Indispensability & "Enq-User "
        End If
        If ProductNumber.Text = "" Then
            st_Indispensability = st_Indispensability & "ProductNumber "
        End If
        If SupplierCode.Text = "" Then
            st_Indispensability = st_Indispensability & "SupplierCode "
        End If
        If QuoLocation.SelectedValue = "" Then
            st_Indispensability = st_Indispensability & "Quo-Location "
        End If
        If Purpose.SelectedValue <> "" Then
            st_Indispensability = st_Indispensability & "Purpose "
        End If
        If st_Indispensability <> "" Then
            Msg.Text = st_Indispensability & "を設定して下さい。"
        End If

        'enq用入力チェック。作成中
        If EnqQuantity_1.Text <> "" And EnqUnit_1.SelectedValue <> "" And EnqPiece_1.Text <> "" Then

        End If
        If EnqQuantity_2.Text <> "" And EnqUnit_2.SelectedValue <> "" And EnqPiece_2.Text <> "" Then

        End If
        If EnqQuantity_3.Text <> "" And EnqUnit_3.SelectedValue <> "" And EnqPiece_3.Text <> "" Then

        End If
        If EnqQuantity_4.Text <> "" And EnqUnit_4.SelectedValue <> "" And EnqPiece_4.Text <> "" Then

        End If
        'EnqQuantity_1	EnqUnit_1	EnqPiece_1
        'EnqQuantity_2	EnqUnit_2	EnqPiece_2
        'EnqQuantity_3	EnqUnit_3	EnqPiece_3
        'EnqQuantity_4	EnqUnit_4	EnqPiece_4

        'めもめも
        ' = "INSERT INTO TestInsert(Name, State)  _"
        '& "VALUES (@Name, @State);" _
        '& " SELECT ID, Name, Stat FROM TestInsert WHERE (ID = SCOPE_IDENTITY())"






    End Sub
End Class
Public Partial Class SupplierSetting
    Inherits CommonPage

#Region " Web フォーム デザイナで生成されたコード "
    '*****（Region内は変更しないこと）*****
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection
        Me.SqlConnection1.FireInfoMessageEventOnUserErrors = False
        Me.SqlConnection2 = New System.Data.SqlClient.SqlConnection
        Me.SqlConnection2.FireInfoMessageEventOnUserErrors = False
    End Sub

    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection
    Protected WithEvents SqlConnection2 As System.Data.SqlClient.SqlConnection

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub
#End Region

    '*****（DB接続用変数定義）*****
    Dim DBConnString As String                              '接続文字列	
    Dim DBConn As New System.Data.SqlClient.SqlConnection   'データベースコネクション	
    Dim DBCommand As System.Data.SqlClient.SqlCommand       'データベースコマンド	
    Dim DBReader As System.Data.SqlClient.SqlDataReader     'データリーダー	
    Dim DBConn2 As New System.Data.SqlClient.SqlConnection  'データベースコネクション	
    Dim DBCommand2 As System.Data.SqlClient.SqlCommand      'データベースコマンド	
    Dim DBReader2 As System.Data.SqlClient.SqlDataReader    'データリーダー	
    Public url As String = ""

    Sub Set_DBConnectingString()
        Dim settings As ConnectionStringSettings
        '[接続文字列を設定ファイル(Web.config)から取得]---------------------------------------------
        settings = ConfigurationManager.ConnectionStrings("DatabaseConnect")
        If Not settings Is Nothing Then
            '[接続文字列をイミディエイトに出力]-----------------------------------------------------
            Debug.Print(settings.ConnectionString)
        End If
        '[sqlConnectionに接続文字列を設定]----------------------------------------------------------
        Me.SqlConnection1.ConnectionString = settings.ConnectionString
        Me.SqlConnection2.ConnectionString = settings.ConnectionString
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '[DBの接続]-----------------------------------------------------------------------
        Set_DBConnectingString()
        DBConn = Me.SqlConnection1
        DBConn.Open()
        DBConn2 = Me.SqlConnection2
        DBConn2.Open()
        DBCommand = DBConn.CreateCommand()
        DBCommand2 = DBConn2.CreateCommand()

        '[初期データ表示]-----------------------------------------------------------------
        If IsPostBack = False Then
            '[StAction設定]---------------------------------------------------------------
            Mode.Value = Request.QueryString("Action")

            '[Country設定]----------------------------------------------------------------
            DBCommand.CommandText = "SELECT CountryCode,CountryName FROM v_Country ORDER BY CountryName"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            Country.Items.Clear()
            Do Until DBReader.Read = False
                Country.Items.Add(New ListItem(DBReader("CountryName"), DBReader("CountryCode")))
            Loop
            DBReader.Close()

            '[DefaultQuoLocation設定]-----------------------------------------------------
            DBCommand.CommandText = "SELECT LocationCode, Name FROM dbo.s_Location"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            DefaultQuoLocation.Items.Clear()
            DefaultQuoLocation.Items.Add(New ListItem("", ""))
            DefaultQuoLocation.Items.Add(New ListItem("Direct", "Direct"))
            Do Until DBReader.Read = False
                DefaultQuoLocation.Items.Add(New ListItem(DBReader("Name"), DBReader("LocationCode")))
            Loop
            DBReader.Close()

            If Mode.Value = "Edit" Then
                Code.Text = Trim(Request.QueryString("Code"))
                DataDisplay1()
                SetTownName()
                DataDisplay2()
            End If
        End If

        If Code.Text <> "" Then
            SuppliersProduct.NavigateUrl = "./ProductListBySupplier.aspx?Supplier=" & Code.Text.ToString
        Else
            SuppliersProduct.NavigateUrl = "./ProductListBySupplier.aspx"
        End If
    End Sub

    Protected Sub Country_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Country.SelectedIndexChanged
        SetTownName()
    End Sub


    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        Dim st_SQLSTR As String = ""
        Msg.Text = ""

        '[Actionチェック]--------------------------------------------------------------------
        If Request.Form("Action") <> "Save" Then
            Msg.Text = "Saveは拒否されました"
            Exit Sub
        End If

        '[必須項目チェック]------------------------------------------------------------------
        If SupplierName3.Text.ToString = "" Or Address1.Text.ToString = "" Or Country.Text.ToString = "" Then
            Msg.Text = "必須項目を入力して下さい"
            Exit Sub
        End If

        If UpdateDate.Value <> "" Then
            '[SupplierのUpdateDateチェック]-----------------------------------------------------------
            DBCommand.CommandText = "SELECT UpdateDate FROM dbo.Supplier WHERE SupplierCode = '" & Common.SafeSqlLiteral(Code.Text) & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                If Common.GetUpdateDate("Supplier", "SupplierCode", Code.Text.ToString) <> UpdateDate.Value Then
                    Msg.Text = "このデータは他のユーザーによって編集されました。その内容を確認し再度編集をお願いします"
                End If
            End If
            DBReader.Close()
        End If

        If Msg.Text.ToString = "" Then
            Dim sqlTran As System.Data.SqlClient.SqlTransaction = DBConn.BeginTransaction()
            DBCommand.Transaction = sqlTran
            Try
                If Mode.Value = "Edit" Then
                    '[Supplierの更新]-------------------------------------------------------------------
                    DBCommand.CommandText = "SELECT SupplierCode FROM dbo.Supplier WHERE SupplierCode = '" & Common.SafeSqlLiteral(Code.Text) & "'"
                    DBReader = DBCommand.ExecuteReader()
                    DBCommand.Dispose()
                    If DBReader.Read = True Then
                        DBReader.Close()
                        '[Supplierの更新処理]------------------------------------------
                        st_SQLSTR = "UPDATE [Supplier] SET R3SupplierCode="
                        If R3SupplierCode.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(R3SupplierCode.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "Name1="
                        If SupplierName1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(SupplierName1.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "Name2="
                        If SupplierName2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(SupplierName2.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "Name3="
                        If SupplierName3.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(SupplierName3.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "Name4="
                        If SupplierName4.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(SupplierName4.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "SearchTerm1="
                        If SearchTerm1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(SearchTerm1.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "SearchTerm2="
                        If SearchTerm2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(SearchTerm2.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "Address1="
                        If Address1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Address1.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "Address2="
                        If Address2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Address2.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "Address3="
                        If Address3.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Address3.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "PostalCode="
                        If PostalCode.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(PostalCode.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "CountryCode="
                        If Country.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Country.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "RegionCode="
                        If Region.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Region.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "Telephone="
                        If Telephone.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Telephone.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "Fax="
                        If Fax.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Fax.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "Email="
                        If Email.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Email.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "Website="
                        If Website.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null, " Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Website.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "Comment="
                        If R3Comment.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null, " Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(R3Comment.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "Note="
                        If Comment.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null, " Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Comment.Text) & "',"
                        st_SQLSTR = st_SQLSTR & "UpdatedBy=" & Session("UserID") & ", UpdateDate='" & Now() & "' "
                        st_SQLSTR = st_SQLSTR & "WHERE SupplierCode='" & Common.SafeSqlLiteral(Code.Text) & "'"
                        DBCommand.CommandText = st_SQLSTR
                        DBCommand.ExecuteNonQuery()

                        '[IrregularRFQLocationの更新]---------------------------------------------------
                        IRFQLocation_Mainte()
                    Else
                        DBReader.Close()
                    End If
                Else
                    '[Supplierの登録]-------------------------------------------------------------------
                    st_SQLSTR = "INSERT INTO Supplier (R3SupplierCode,Name1,Name2,Name3,Name4,SearchTerm1,SearchTerm2,Address1,Address2,Address3,PostalCode,CountryCode,RegionCode,Telephone,Fax,Email,Comment,Website,Note,LocationCode,isDisabled,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ("
                    If R3SupplierCode.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(R3SupplierCode.Text) & "',"
                    If SupplierName1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(SupplierName1.Text) & "',"
                    If SupplierName2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(SupplierName2.Text) & "',"
                    If SupplierName3.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(SupplierName3.Text) & "',"
                    If SupplierName4.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(SupplierName4.Text) & "',"
                    If SearchTerm1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(SearchTerm1.Text) & "',"
                    If SearchTerm2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(SearchTerm2.Text) & "',"
                    If Address1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Address1.Text) & "',"
                    If Address2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Address2.Text) & "',"
                    If Address3.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Address3.Text) & "',"
                    If PostalCode.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(PostalCode.Text) & "',"
                    If Country.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Country.Text) & "',"
                    If Region.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Region.Text) & "',"
                    If Telephone.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Telephone.Text) & "',"
                    If Fax.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Fax.Text) & "',"
                    If Email.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Email.Text) & "',"
                    If R3Comment.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(R3Comment.Text) & "',"
                    If Website.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Website.Text) & "',"
                    If Comment.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Common.SafeSqlLiteral(Comment.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "null,0,'" & Session("UserID") & "','" & Now() & "','" & Session("UserID") & "','" & Now() & "'); "
                    st_SQLSTR = st_SQLSTR & "SELECT SupplierCode FROM Supplier WHERE SupplierCode = SCOPE_IDENTITY()"  '←[新規登録されたSupplierCodeの取得の為]
                    DBCommand.CommandText = st_SQLSTR
                    DBReader = DBCommand.ExecuteReader()
                    DBCommand.Dispose()
                    If DBReader.Read = True Then
                        Code.Text = DBReader("SupplierCode")
                        SuppliersProduct.NavigateUrl = "./ProductListBySupplier.aspx?Supplier=" & DBReader("SupplierCode")
                    End If
                    DBReader.Close()

                    '[IrregularRFQLocationの更新]--------------------------------------------------------
                    IRFQLocation_Mainte()

                    '[StActionをEditにする]--------------------------------------------------------------
                    Mode.Value = "Edit"
                End If

                'ここまでエラーがなかったらコミット
                sqlTran.Commit()
            Catch ex As Exception
                'エラーがあった場合はロールバック
                sqlTran.Rollback()
                Throw
            End Try

            '[SupplierからUpdateDate取得]--------------------------------------------------------------
            UpdateDate.Value = Common.GetUpdateDate("Supplier", "SupplierCode", Code.Text.ToString)  '[同時更新チェック用]
        End If
    End Sub

    Public Sub SetTownName()
        '[RegionにText及びValue設定]----------------------------------------------------------------
        DBCommand.CommandText = "SELECT CountryCode,RegionCode,Name FROM s_Region WHERE CountryCode='" & Common.SafeSqlLiteral(Country.Text) & "' ORDER BY name"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        Region.Items.Clear()
        Region.Items.Add(New ListItem("", ""))
        Do Until DBReader.Read = False
            Region.Items.Add(New ListItem(DBReader("Name"), DBReader("RegionCode")))
        Loop
        DBReader.Close()
    End Sub

    
    Public Sub DataDisplay1()
        DBCommand.CommandText = "SELECT SupplierCode, R3SupplierCode, Name1, Name2, Name3, Name4, SearchTerm1, SearchTerm2, Address1, Address2, Address3, PostalCode, CountryCode, RegionCode, Telephone, Fax, Email, Comment, Website, Note, UpdateDate " & _
                                "FROM dbo.Supplier WHERE SupplierCode = '" & Code.Text.ToString & "'"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.Read = True Then
            If Not TypeOf DBReader("R3SupplierCode") Is DBNull Then R3SupplierCode.Text = DBReader("R3SupplierCode")
            If Not TypeOf DBReader("Name1") Is DBNull Then SupplierName1.Text = DBReader("Name1")
            If Not TypeOf DBReader("Name2") Is DBNull Then SupplierName2.Text = DBReader("Name2")
            If Not TypeOf DBReader("Name3") Is DBNull Then SupplierName3.Text = DBReader("Name3")
            If Not TypeOf DBReader("Name4") Is DBNull Then SupplierName4.Text = DBReader("Name4")
            If Not TypeOf DBReader("SearchTerm1") Is DBNull Then SearchTerm1.Text = DBReader("SearchTerm1")
            If Not TypeOf DBReader("SearchTerm2") Is DBNull Then SearchTerm2.Text = DBReader("SearchTerm2")
            If Not TypeOf DBReader("Address1") Is DBNull Then Address1.Text = DBReader("Address1")
            If Not TypeOf DBReader("Address2") Is DBNull Then Address2.Text = DBReader("Address2")
            If Not TypeOf DBReader("Address3") Is DBNull Then Address3.Text = DBReader("Address3")
            If Not TypeOf DBReader("PostalCode") Is DBNull Then PostalCode.Text = DBReader("PostalCode")
            If Not TypeOf DBReader("Telephone") Is DBNull Then Telephone.Text = DBReader("Telephone")
            If Not TypeOf DBReader("Fax") Is DBNull Then Fax.Text = DBReader("Fax")
            If Not TypeOf DBReader("Email") Is DBNull Then Email.Text = DBReader("Email")
            If Not TypeOf DBReader("Website") Is DBNull Then Website.Text = DBReader("Website")
            If Not TypeOf DBReader("Comment") Is DBNull Then R3Comment.Text = DBReader("Comment")
            If Not TypeOf DBReader("Note") Is DBNull Then Comment.Text = DBReader("Note")
            Country.SelectedValue = DBReader("CountryCode")
            UpdateDate.Value = Common.GetUpdateDate("Supplier", "SupplierCode", Code.Text.ToString) '[同時更新チェック用]
            DBReader.Close()
        Else
            UpdateDate.Value = ""
            DBReader.Close()
        End If
    End Sub

    Public Sub DataDisplay2()
        DBCommand.CommandText = "SELECT CountryCode, RegionCode FROM dbo.Supplier WHERE SupplierCode = '" & Common.SafeSqlLiteral(Code.Text) & "'"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.Read = True Then
            '[Country,Regionにデータ表示]-------------------------------------------------------
            If Not TypeOf DBReader("CountryCode") Is DBNull Then Country.Text = DBReader("CountryCode")
            If Not TypeOf DBReader("RegionCode") Is DBNull Then Region.Text = DBReader("RegionCode")

            '[DefaultQuoLocation.Item設定]------------------------------------------------------
            DBCommand2.CommandText = "SELECT QuoLocationCode FROM dbo.IrregularRFQLocation WHERE (SupplierCode = '" & Common.SafeSqlLiteral(Code.Text) & "')"
            DBReader2 = DBCommand2.ExecuteReader()
            DBCommand2.Dispose()
            If DBReader2.Read = True Then
                If Not TypeOf DBReader2("QuoLocationCode") Is DBNull Then
                    DefaultQuoLocation.Text = DBReader2("QuoLocationCode")
                Else
                    DefaultQuoLocation.Text = "Direct"
                End If
            End If
            DBReader2.Close()
        End If
        DBReader.Close()
    End Sub

    Public Sub IRFQLocation_Mainte()
        '[IrregularRFQLocationの更新]-------------------------------------------------------------------
        If DefaultQuoLocation.SelectedValue = "" Then
            DBCommand.CommandText = "SELECT SupplierCode FROM [IrregularRFQLocation] WHERE SupplierCode='" & Common.SafeSqlLiteral(Code.Text) & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                DBReader.Close()
                DBCommand.CommandText = "DELETE FROM IrregularRFQLocation WHERE SupplierCode = '" & Common.SafeSqlLiteral(Code.Text) & "'"
                DBCommand.ExecuteNonQuery()
            Else
                DBReader.Close()
            End If
        ElseIf DefaultQuoLocation.SelectedValue = "Direct" Then
            DBCommand.CommandText = "SELECT SupplierCode FROM [IrregularRFQLocation] WHERE SupplierCode='" & Common.SafeSqlLiteral(Code.Text) & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                DBReader.Close()
                DBCommand.CommandText = "UPDATE IrregularRFQLocation SET QuoLocationCode=null WHERE SupplierCode = '" & Common.SafeSqlLiteral(Code.Text) & "'"
                DBCommand.ExecuteNonQuery()
            Else
                DBReader.Close()
                DBCommand.CommandText = "INSERT INTO IrregularRFQLocation (EnqLocationCode,SupplierCode,QuoLocationCode,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ('" & Session("LocationCode") & "','" & Common.SafeSqlLiteral(Code.Text) & "',null,'" & Session("UserID") & "','" & Now() & "','" & Session("UserID") & "','" & Now() & "')"
                DBCommand.ExecuteNonQuery()
            End If
        Else
            DBCommand.CommandText = "SELECT SupplierCode FROM [IrregularRFQLocation] WHERE SupplierCode='" & Common.SafeSqlLiteral(Code.Text) & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                DBReader.Close()
                DBCommand.CommandText = "UPDATE IrregularRFQLocation SET QuoLocationCode='" & DefaultQuoLocation.SelectedValue & "' WHERE SupplierCode = '" & Common.SafeSqlLiteral(Code.Text) & "'"
                DBCommand.ExecuteNonQuery()
            Else
                DBReader.Close()
                DBCommand.CommandText = "INSERT INTO IrregularRFQLocation (EnqLocationCode,SupplierCode,QuoLocationCode,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ('" & Session("LocationCode") & "','" & Common.SafeSqlLiteral(Code.Text) & "','" & DefaultQuoLocation.SelectedValue & "','" & Session("UserID") & "','" & Now() & "','" & Session("UserID") & "','" & Now() & "')"
                DBCommand.ExecuteNonQuery()
            End If
        End If
    End Sub

    Private Sub SupplierSetting_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
        DBConn2.Close()
    End Sub
End Class
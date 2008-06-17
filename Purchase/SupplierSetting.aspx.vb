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
    Dim st_CountryCode As String = ""                       '選択したCountryCode
    Dim st_RegionCode As String = ""                        '選択したRegionCode

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

        '[初期データ表示]-----------------------------------------------------------------
        If IsPostBack = False Then
            '[Country設定]----------------------------------------------------------------
            DBCommand = DBConn.CreateCommand()
            DBCommand.CommandText = "SELECT name FROM s_Country ORDER BY name"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            Country.Items.Clear()
            Do Until DBReader.Read = False
                Country.Items.Add(DBReader("Name"))
            Loop
            DBReader.Close()

            '[Region設定]-----------------------------------------------------------------
            DBCommand = DBConn.CreateCommand()
            DBCommand.CommandText = "SELECT name FROM s_Region ORDER BY name"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            Region.Items.Clear()
            Do Until DBReader.Read = False
                Region.Items.Add(DBReader("Name"))
            Loop
            DBReader.Close()

            '[DefaultQuoLocation設定]-----------------------------------------------------
            DBCommand = DBConn.CreateCommand()
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

            If Request.QueryString("Action") = "Edit" Then
                Code.Text = Request.QueryString("Code")
                DataDisplay1()
                SetCountryCode()
                SetTownName()
                SetRegionCode()
                DataDisplay2()
            Else
                SetCountryCode()
                SetTownName()
                SetRegionCode()
            End If
        End If
    End Sub

    Protected Sub Country_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Country.SelectedIndexChanged
        SetCountryCode()
        SetTownName()
    End Sub

    Protected Sub Region_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles Region.SelectedIndexChanged
        SetRegionCode()
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        Dim st_SQLSTR As String = ""
        If SupplierName3.Text.ToString <> "" And Address1.Text.ToString <> "" And Country.Text.ToString <> "" Then
            SetCountryCode()
            SetRegionCode()
            If Request.QueryString("Action") = "Edit" Then
                '[Supplierの更新]-------------------------------------------------------------------
                DBCommand = DBConn.CreateCommand()
                DBCommand.CommandText = "SELECT SupplierCode FROM dbo.Supplier WHERE SupplierCode = '" & Trim(Code.Text.ToString) & "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    DBReader.Close()
                    '[Supplierの更新処理]------------------------------------------
                    st_SQLSTR = "UPDATE [Supplier] SET R3SupplierCode="
                    If R3SupplierCode.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & R3SupplierCode.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "Name1="
                    If SupplierName1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SupplierName1.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "Name2="
                    If SupplierName2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SupplierName2.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "Name3="
                    If SupplierName3.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SupplierName3.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "Name4="
                    If SupplierName4.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SupplierName4.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "SearchTerm1="
                    If SearchTerm1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SearchTerm1.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "SearchTerm2="
                    If SearchTerm2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SearchTerm2.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "Address1="
                    If Address1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Address1.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "Address2="
                    If Address2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Address2.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "Address3="
                    If Address3.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Address3.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "PostalCode="
                    If PostalCode.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & PostalCode.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "CountryCode="
                    If Country.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & st_CountryCode & "',"
                    st_SQLSTR = st_SQLSTR & "RegionCode="
                    If Region.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & st_RegionCode & "',"
                    st_SQLSTR = st_SQLSTR & "Telephone="
                    If Telephone.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Telephone.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "Fax="
                    If Fax.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Fax.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "Email="
                    If Email.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & Email.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "Website="
                    If Website.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null, " Else st_SQLSTR = st_SQLSTR & "'" & Website.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "Note="
                    If Comment.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null, " Else st_SQLSTR = st_SQLSTR & "'" & Comment.Text.ToString & "',"
                    st_SQLSTR = st_SQLSTR & "UpdatedBy=" & Session("UserID") & ", UpdateDate='" & Now() & "' "
                    st_SQLSTR = st_SQLSTR & "WHERE SupplierCode='" & Trim(Code.Text.ToString) & "'"
                    DBCommand.CommandText = st_SQLSTR
                    DBCommand.ExecuteNonQuery()

                    '[IrregularRFQLocationの更新]-------------------------------------------------------------------
                    If DefaultQuoLocation.SelectedValue = "" Then

                    ElseIf DefaultQuoLocation.SelectedValue = "Direct" Then
                        DBCommand.CommandText = "UPDATE IrregularRFQLocation SET QuoLocationCode=null WHERE SupplierCode = '" & Code.Text.ToString & "'"
                        DBCommand.ExecuteNonQuery()
                    Else
                        DBCommand.CommandText = "UPDATE IrregularRFQLocation SET QuoLocationCode='" & DefaultQuoLocation.SelectedValue & "' WHERE SupplierCode = '" & Trim(Code.Text.ToString) & "'"
                        DBCommand.ExecuteNonQuery()
                    End If
                Else
                    DBReader.Close()
                End If
            Else
                '[Supplierの登録]-------------------------------------------------------------------
            End If

            '[呼出元のフォームに戻る]------------------------------------------
            'Response.Redirect("SupplierSearch.aspx")
        Else
            Msg.Text = "必須項目を入力して下さい"
        End If
    End Sub

    Public Sub SetCountryCode()
        '[選択したCountryCode取得]-------------------------------------------------------------------
        DBCommand = DBConn.CreateCommand()
        DBCommand.CommandText = "SELECT CountryCode FROM [s_Country] WHERE name='" & Country.Text.ToString & "'"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.Read = True Then
            st_CountryCode = DBReader("CountryCode")
        End If
        DBReader.Close()
    End Sub

    Public Sub SetTownName()
        '[選択したCountryCodeで都市名選出]-----------------------------------------------------------
        DBCommand = DBConn.CreateCommand()
        DBCommand.CommandText = "SELECT name FROM s_Region WHERE countrycode='" & st_CountryCode & "' ORDER BY name"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        Region.Items.Clear()
        Do Until DBReader.Read = False
            Region.Items.Add(DBReader("Name"))
        Loop
        DBReader.Close()
    End Sub

    Public Sub SetRegionCode()
        '[選択したRegionCode取得]-------------------------------------------------------------------
        DBCommand = DBConn.CreateCommand()
        DBCommand.CommandText = "SELECT RegionCode FROM s_Region WHERE (CountryCode = '" & st_CountryCode & "') AND (Name = '" & Region.Text.ToString & "')"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.Read = True Then
            st_RegionCode = DBReader("RegionCode")
        End If
        DBReader.Close()
    End Sub

    Public Sub DataDisplay1()
        DBCommand = DBConn.CreateCommand()
        DBCommand.CommandText = "SELECT SupplierCode, R3SupplierCode, Name1, Name2, Name3, Name4, SearchTerm1, SearchTerm2, Address1, Address2, Address3, PostalCode, CountryCode, RegionCode, Telephone, Fax, Email, Comment, Website, Note " & _
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
            DBCommand2 = DBConn2.CreateCommand()
            DBCommand2.CommandText = "SELECT name FROM [s_Country] WHERE CountryCode='" & DBReader("CountryCode") & "'"
            DBReader2 = DBCommand2.ExecuteReader()
            DBCommand2.Dispose()
            If DBReader2.Read = True Then
                Country.Text = DBReader2("name")
            End If
            DBReader2.Close()
        End If
        DBReader.Close()
    End Sub

    Public Sub DataDisplay2()
        DBCommand = DBConn.CreateCommand()
        DBCommand.CommandText = "SELECT CountryCode, RegionCode FROM dbo.Supplier WHERE SupplierCode = '" & Code.Text.ToString & "'"
        DBReader = DBCommand.ExecuteReader()
        DBCommand.Dispose()
        If DBReader.Read = True Then
            '[Country.Item設定]-----------------------------------------------------------------
            DBCommand2 = DBConn2.CreateCommand()
            DBCommand2.CommandText = "SELECT name FROM [s_Country] WHERE CountryCode='" & DBReader("CountryCode") & "'"
            DBReader2 = DBCommand2.ExecuteReader()
            DBCommand2.Dispose()
            If DBReader2.Read = True Then
                Country.Text = DBReader2("name")
            End If
            DBReader2.Close()
            '[Region.Item設定]------------------------------------------------------------------
            DBCommand2 = DBConn2.CreateCommand()
            DBCommand2.CommandText = "SELECT Name FROM dbo.s_Region WHERE (CountryCode = '" & DBReader("CountryCode") & "') AND (RegionCode = '" & DBReader("RegionCode") & "')"
            DBReader2 = DBCommand2.ExecuteReader()
            DBCommand2.Dispose()
            If DBReader2.Read = True Then
                Region.Text = DBReader2("name")
            End If
            DBReader2.Close()
            '[DefaultQuoLocation.Item設定]------------------------------------------------------
            DBCommand2 = DBConn2.CreateCommand()
            DBCommand2.CommandText = "SELECT QuoLocationCode FROM dbo.IrregularRFQLocation WHERE (SupplierCode = '" & Trim(Code.Text.ToString) & "')"
            'DBCommand2.CommandText = "SELECT dbo.s_Location.Name FROM dbo.s_Location RIGHT OUTER JOIN dbo.IrregularRFQLocation ON dbo.s_Location.LocationCode = dbo.IrregularRFQLocation.QuoLocationCode WHERE (dbo.IrregularRFQLocation.SupplierCode = '" & Code.Text.ToString & "')"
            DBReader2 = DBCommand2.ExecuteReader()
            DBCommand2.Dispose()
            If DBReader2.Read = True Then
                If Not TypeOf DBReader2("QuoLocationCode") Is DBNull Then
                    DefaultQuoLocation.Text = DBReader2("QuoLocationCode")
                End If
            End If
            DBReader2.Close()
        End If
        DBReader.Close()
    End Sub

End Class
Imports Purchase.Common

Partial Public Class SupplierSetting
    Inherits CommonPage

    Dim DBConn As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand As System.Data.SqlClient.SqlCommand
    Dim DBReader As System.Data.SqlClient.SqlDataReader
    Dim DBConn2 As New System.Data.SqlClient.SqlConnection(Common.DB_CONNECT_STRING)
    Dim DBCommand2 As System.Data.SqlClient.SqlCommand
    Dim DBReader2 As System.Data.SqlClient.SqlDataReader
    Public url As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        '[DBの接続]-----------------------------------------------------------------------
        DBConn.Open()
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
            DBCommand.CommandText = "SELECT LocationCode, Name FROM dbo.s_Location ORDER BY Name"
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
            Else
                SuppliersProduct.Visible = False
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
        Msg.Text = String.Empty
        RunMsg.Text = String.Empty

        '[Actionチェック]--------------------------------------------------------------------
        If Request.Form("Action") <> "Save" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        '[必須項目チェック]------------------------------------------------------------------
        If SupplierName3.Text = "" Then
            Msg.Text = "Supplier Name" + ERR_REQUIRED_FIELD
            Exit Sub
        End If
        If Address1.Text = "" Then
            Msg.Text = "Address" + ERR_REQUIRED_FIELD
            Exit Sub
        End If
        If Country.Text = "" Then
            Msg.Text = "Country" + ERR_REQUIRED_FIELD
            Exit Sub
        End If

        '[Email,URLのCheck]------------------------------------------------------------------
        If Not Regex.IsMatch(Email.Text, EMAIL_REGEX) Then
            Msg.Text = "E-mail" + ERR_INCORRECT_FORMAT
            Exit Sub
        End If
        If Not Regex.IsMatch(Website.Text, URL_REGEX) Then
            Msg.Text = "Website" + ERR_INCORRECT_FORMAT
            Exit Sub
        End If

        '[入力項目の項目長Check]-------------------------------------------------------------
        If GetByteCount_SJIS(PostalCode.Text) > 32 Then
            Msg.Text = "PostalCode" + ERR_TOO_LONG
            Exit Sub
        End If
        If GetByteCount_SJIS(Telephone.Text) > 32 Then
            Msg.Text = "Telephone" + ERR_TOO_LONG
            Exit Sub
        End If
        If GetByteCount_SJIS(Fax.Text) > 32 Then
            Msg.Text = "Fax" + ERR_TOO_LONG
            Exit Sub
        End If
        If Comment.Text.Length > INT_3000 Then
            Msg.Text = "Comment" + ERR_OVER_3000
            Exit Sub
        End If

        If UpdateDate.Value <> "" Then
            '[SupplierのUpdateDateチェック]--------------------------------------------------
            DBCommand.CommandText = "SELECT UpdateDate FROM dbo.Supplier WHERE SupplierCode = '" & SafeSqlLiteral(Code.Text) & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                If GetUpdateDate("Supplier", "SupplierCode", Code.Text.ToString) <> UpdateDate.Value Then
                    Msg.Text = ERR_UPDATED_BY_ANOTHER_USER   '"データは他のユーザによって既に更新されています。ご確認ください。"
                    Exit Sub
                End If
            End If
            DBReader.Close()
        End If

        Dim sqlTran As System.Data.SqlClient.SqlTransaction = DBConn.BeginTransaction()
        DBCommand.Transaction = sqlTran
        Dim MemoMode As String = Mode.Value
        Try
            If Mode.Value = "Edit" Then
                '[Supplierの更新]--------------------------------------------------------
                DBCommand.CommandText = "SELECT SupplierCode FROM dbo.Supplier WHERE SupplierCode = '" & SafeSqlLiteral(Code.Text) & "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    DBReader.Close()
                    '[Supplierの更新処理]------------------------------------------------
                    st_SQLSTR = "UPDATE [Supplier] SET R3SupplierCode="
                    If R3SupplierCode.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(R3SupplierCode.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "Name1="
                    If SupplierName1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(SupplierName1.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "Name2="
                    If SupplierName2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(SupplierName2.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "Name3="
                    If SupplierName3.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(SupplierName3.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "Name4="
                    If SupplierName4.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(SupplierName4.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "SearchTerm1="
                    If SearchTerm1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(SearchTerm1.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "SearchTerm2="
                    If SearchTerm2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(SearchTerm2.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "Address1="
                    If Address1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Address1.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "Address2="
                    If Address2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Address2.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "Address3="
                    If Address3.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Address3.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "PostalCode="
                    If PostalCode.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(PostalCode.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "CountryCode="
                    If Country.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Country.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "RegionCode="
                    If Region.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Region.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "Telephone="
                    If Telephone.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Telephone.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "Fax="
                    If Fax.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Fax.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "Email="
                    If Email.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Email.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "Website="
                    If Website.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null, " Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Website.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "Comment="
                    If R3Comment.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null, " Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(R3Comment.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "Note="
                    If Comment.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null, " Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Comment.Text) & "',"
                    st_SQLSTR = st_SQLSTR & "UpdatedBy=" & Session("UserID") & ", UpdateDate='" & Now() & "' "
                    st_SQLSTR = st_SQLSTR & "WHERE SupplierCode='" & SafeSqlLiteral(Code.Text) & "'"
                    DBCommand.CommandText = st_SQLSTR
                    DBCommand.ExecuteNonQuery()

                    '[IrregularRFQLocationの更新]----------------------------------------
                    IRFQLocation_Mainte()
                Else
                    DBReader.Close()
                End If
            Else
                '[Supplierの登録]--------------------------------------------------------
                st_SQLSTR = "INSERT INTO Supplier (R3SupplierCode,Name1,Name2,Name3,Name4,SearchTerm1,SearchTerm2,Address1,Address2,Address3,PostalCode,CountryCode,RegionCode,Telephone,Fax,Email,Comment,Website,Note,LocationCode,isDisabled,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ("
                If R3SupplierCode.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(R3SupplierCode.Text) & "',"
                If SupplierName1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(SupplierName1.Text) & "',"
                If SupplierName2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(SupplierName2.Text) & "',"
                If SupplierName3.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(SupplierName3.Text) & "',"
                If SupplierName4.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(SupplierName4.Text) & "',"
                If SearchTerm1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(SearchTerm1.Text) & "',"
                If SearchTerm2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(SearchTerm2.Text) & "',"
                If Address1.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Address1.Text) & "',"
                If Address2.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Address2.Text) & "',"
                If Address3.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Address3.Text) & "',"
                If PostalCode.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(PostalCode.Text) & "',"
                If Country.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Country.Text) & "',"
                If Region.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Region.Text) & "',"
                If Telephone.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Telephone.Text) & "',"
                If Fax.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Fax.Text) & "',"
                If Email.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Email.Text) & "',"
                If R3Comment.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(R3Comment.Text) & "',"
                If Website.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Website.Text) & "',"
                If Comment.Text.ToString = "" Then st_SQLSTR = st_SQLSTR & "null," Else st_SQLSTR = st_SQLSTR & "'" & SafeSqlLiteral(Comment.Text) & "',"
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

                '[IrregularRFQLocationの更新]--------------------------------------------
                IRFQLocation_Mainte()

                '[StActionをEditにする]--------------------------------------------------
                Mode.Value = "Edit"
                SuppliersProduct.Visible = True
            End If

            'ここまでエラーがなかったらコミット
            sqlTran.Commit()
            If MemoMode = "Edit" Then
                RunMsg.Text = MSG_DATA_UPDATED
            Else
                RunMsg.Text = MSG_DATA_CREATED
            End If
        Catch ex As Exception
            'エラーがあった場合はロールバック
            sqlTran.Rollback()
            Throw
        End Try

        '[SupplierからUpdateDate取得]----------------------------------------------------
        UpdateDate.Value = GetUpdateDate("Supplier", "SupplierCode", Code.Text.ToString)  '[同時更新チェック用]
    End Sub

    Public Sub SetTownName()
        '[RegionにText及びValue設定]---------------------------------------------------------
        DBCommand.CommandText = "SELECT CountryCode,RegionCode,Name FROM s_Region WHERE CountryCode='" & SafeSqlLiteral(Country.Text) & "' ORDER BY Name"
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
        If IsInteger(Code.Text) Then
            DBCommand.CommandText = "SELECT SupplierCode, R3SupplierCode, Name1, Name2, Name3, Name4, SearchTerm1, SearchTerm2, Address1, Address2, Address3, PostalCode, CountryCode, RegionCode, Telephone, Fax, Email, Comment, Website, Note, UpdateDate " & _
                                            "FROM dbo.Supplier WHERE SupplierCode = " & Code.Text.ToString
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
                UpdateDate.Value = GetUpdateDate("Supplier", "SupplierCode", Code.Text.ToString) '[同時更新チェック用]
                DBReader.Close()
            Else
                UpdateDate.Value = ""
                DBReader.Close()
            End If
        End If
    End Sub

    Public Sub DataDisplay2()
        If IsInteger(Code.Text) Then
            DBCommand.CommandText = "SELECT CountryCode, RegionCode FROM dbo.Supplier WHERE SupplierCode = " & SafeSqlLiteral(Code.Text)
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                '[Country,Regionにデータ表示]-------------------------------------------------------
                If Not TypeOf DBReader("CountryCode") Is DBNull Then Country.Text = DBReader("CountryCode")
                If Not TypeOf DBReader("RegionCode") Is DBNull Then Region.Text = DBReader("RegionCode")

                '[DefaultQuoLocation.Item設定]------------------------------------------------------
                DBCommand2.CommandText = "SELECT QuoLocationCode FROM dbo.IrregularRFQLocation WHERE (SupplierCode = '" & SafeSqlLiteral(Code.Text) & "')"
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
            Else
                INV_Supplier()
            End If
            DBReader.Close()
        Else
            INV_Supplier()
            Exit Sub
        End If
    End Sub

    Public Sub INV_Supplier()
        Msg.Text = "Supplier Code" + ERR_DOES_NOT_EXIST
        SuppliersProduct.Enabled = False
        Save.Enabled = False
    End Sub

    Public Sub IRFQLocation_Mainte()
        '[IrregularRFQLocationの更新]--------------------------------------------------------
        If DefaultQuoLocation.SelectedValue = "" Then
            DBCommand.CommandText = "SELECT SupplierCode FROM [IrregularRFQLocation] WHERE SupplierCode='" & SafeSqlLiteral(Code.Text) & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                DBReader.Close()
                DBCommand.CommandText = "DELETE FROM IrregularRFQLocation WHERE SupplierCode = '" & SafeSqlLiteral(Code.Text) & "'"
                DBCommand.ExecuteNonQuery()
            Else
                DBReader.Close()
            End If
        ElseIf DefaultQuoLocation.SelectedValue = "Direct" Then
            DBCommand.CommandText = "SELECT SupplierCode FROM [IrregularRFQLocation] WHERE SupplierCode='" & SafeSqlLiteral(Code.Text) & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                DBReader.Close()
                DBCommand.CommandText = "UPDATE IrregularRFQLocation SET QuoLocationCode=null WHERE SupplierCode = '" & SafeSqlLiteral(Code.Text) & "'"
                DBCommand.ExecuteNonQuery()
            Else
                DBReader.Close()
                DBCommand.CommandText = "INSERT INTO IrregularRFQLocation (EnqLocationCode,SupplierCode,QuoLocationCode,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ('" & Session("LocationCode") & "','" & SafeSqlLiteral(Code.Text) & "',null,'" & Session("UserID") & "','" & Now() & "','" & Session("UserID") & "','" & Now() & "')"
                DBCommand.ExecuteNonQuery()
            End If
        Else
            DBCommand.CommandText = "SELECT SupplierCode FROM [IrregularRFQLocation] WHERE SupplierCode='" & SafeSqlLiteral(Code.Text) & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                DBReader.Close()
                DBCommand.CommandText = "UPDATE IrregularRFQLocation SET QuoLocationCode='" & DefaultQuoLocation.SelectedValue & "' WHERE SupplierCode = '" & SafeSqlLiteral(Code.Text) & "'"
                DBCommand.ExecuteNonQuery()
            Else
                DBReader.Close()
                DBCommand.CommandText = "INSERT INTO IrregularRFQLocation (EnqLocationCode,SupplierCode,QuoLocationCode,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ('" & Session("LocationCode") & "','" & SafeSqlLiteral(Code.Text) & "','" & DefaultQuoLocation.SelectedValue & "','" & Session("UserID") & "','" & Now() & "','" & Session("UserID") & "','" & Now() & "')"
                DBCommand.ExecuteNonQuery()
            End If
        End If
    End Sub

    Private Sub SupplierSetting_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
        DBConn2.Close()
    End Sub

End Class
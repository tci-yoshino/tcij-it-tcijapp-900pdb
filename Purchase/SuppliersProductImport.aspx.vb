﻿Public Partial Class SuppliersProductImport
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
    Dim DBConn2 As New System.Data.SqlClient.SqlConnection   'データベースコネクション	
    Dim DBCommand2 As System.Data.SqlClient.SqlCommand       'データベースコマンド	
    Dim DBReader2 As System.Data.SqlClient.SqlDataReader     'データリーダー	
    Dim st_CAS As String = ""
    Dim st_ItemNo As String = ""
    Dim st_ItemName As String = ""
    Dim st_Note As String = ""
    Dim st_SqlStr As String = ""
    Dim CntA As Integer

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
        Set_DBConnectingString()
        DBConn = Me.SqlConnection1
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()
        DBConn2 = Me.SqlConnection2
        DBConn2.Open()
        DBCommand2 = DBConn2.CreateCommand()

        If Request.QueryString("Supplier") <> "" Then
            If IsPostBack = False Then
                SupplierCode.Text = Request.QueryString("Supplier")
                DBCommand.CommandText = "SELECT Name3 FROM Supplier WHERE (SupplierCode = '" & SupplierCode.Text.ToString & "')"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    SupplierName.Text = DBReader("Name3")
                End If
                DBReader.Close()
            End If
        Else
            Msg.Text = "SupplierCodeが設定されていません"
            File.Visible = False
            Preview.Visible = False
            Import.Visible = False
        End If
    End Sub

    Private Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        'SupplierProductList.Columns(9).ItemStyle.HorizontalAlign = HorizontalAlign.Center

        'If IsPostBack = True Then
        '    SupplierProductList.Columns(0).ItemStyle.BackColor = Drawing.Color.Red
        'End If
    End Sub

    Protected Sub Preview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Preview.Click
        Msg.Text = ""
        '[Preview実行環境確認]--------------------------------------------------------------
        If IO.Path.GetFileName(File.PostedFile.FileName) = "" Then
            SupplierProductList.DataSourceID = ""
            SupplierProductList.DataSource = ""
            SupplierProductList.DataBind()
            Import.Visible = False
            Msg.Text = "読込みファイルが設定されていません"
            Exit Sub
        End If

        'Action確認]------------------------------------------------------------------------
        If Request.Form("Action") <> "Preview" Then
            Msg.Text = "Previewできる環境でありません"
            Exit Sub
        End If

        '[読込みファイルがEXCELか確認]------------------------------------------------------
        If Request.Files("File").ContentType <> "application/vnd.ms-excel" Then
            Msg.Text = "読込みファイルはEXCELでありません"
            Exit Sub
        End If

        '[作成Table名の決定]----------------------------------------------------------------
        ImportFileName.Value = "D:\\temp\\G_System4\ID_" & Session("UserID") & IO.Path.GetFileName(File.PostedFile.FileName)

        '[作成Tableがある場合削除する]------------------------------------------------------
        If Dir(ImportFileName.Value) <> "" Then
            Kill(ImportFileName.Value)
        End If

        '[指定ルートにTableを登録する]------------------------------------------------------
        If File.PostedFile.FileName <> "" Then
            File.PostedFile.SaveAs(ImportFileName.Value)
        End If

        '[読込みファイルをGridViewに必要項目を付加して表示]---------------------------------
        Dim st_ProductNumber As String = ""
        Dim st_Status As String = ""
        Dim st_ProposalDept As String = ""
        Dim st_ProcumentDept As String = ""
        Dim ds As New DataSet()
        Dim table As DataTable
        Dim i As Integer
        Dim conStr As New OleDb.OleDbConnectionStringBuilder()
        conStr.Provider = "Microsoft.JET.OLEDB.4.0"
        conStr.DataSource = ImportFileName.Value
        conStr.DataSource = Request.Files("File").FileName
        conStr("Extended Properties") = "Excel 8.0;HDR=YES;IMEX=1"
        Dim sql As String = "SELECT * FROM [Sheet1$]"
        Using da As New OleDb.OleDbDataAdapter(sql, conStr.ConnectionString)
            da.Fill(ds, "Sheet1")
        End Using

        table = ds.Tables("Sheet1")
        table.Columns.Add("TCI Product Number", Type.GetType("System.String"))
        table.Columns.Add("EHS Status", Type.GetType("System.String"))
        table.Columns.Add("Proposal Dept", Type.GetType("System.String"))
        table.Columns.Add("Proc.Dept", Type.GetType("System.String"))
        table.Columns.Add("AD", Type.GetType("System.String"))
        table.Columns.Add("AF", Type.GetType("System.String"))
        table.Columns.Add("WA", Type.GetType("System.String"))
        table.Columns.Add("KA", Type.GetType("System.String"))

        For i = 0 To table.Rows.Count - 1
            DBCommand.CommandText = "SELECT ALDRICH, ALFA, WAKO, KANTO FROM dbo.v_CompetitorProduct WHERE CASNumber = N'" & table.Rows(i).Item("CAS Number") & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                table.Rows(i).Item("AD") = DBReader("ALDRICH")
                table.Rows(i).Item("AF") = DBReader("ALFA")
                table.Rows(i).Item("WA") = DBReader("WAKO")
                table.Rows(i).Item("KA") = DBReader("KANTO")
            End If
            DBReader.Close()
        Next i

        SupplierProductList.DataSourceID = ""
        SupplierProductList.DataSource = table
        SupplierProductList.DataBind()

        For i = 0 To SupplierProductList.Rows.Count - 1
            DBCommand.CommandText = "SELECT ProductNumber,Status,ProposalDept,ProcumentDept FROM dbo.Product WHERE CASNumber = '" & table.Rows(i).Item("CAS Number") & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            CntA = 0
            Do Until DBReader.Read = False
                CntA = CntA + 1
                If Not TypeOf DBReader("ProductNumber") Is DBNull Then st_ProductNumber = DBReader("ProductNumber") Else st_ProductNumber = ""
                If Not TypeOf DBReader("Status") Is DBNull Then st_Status = DBReader("Status") Else st_Status = ""
                If Not TypeOf DBReader("ProposalDept") Is DBNull Then st_ProposalDept = DBReader("ProposalDept") Else st_ProposalDept = ""
                If Not TypeOf DBReader("ProcumentDept") Is DBNull Then st_ProcumentDept = DBReader("ProcumentDept") Else st_ProcumentDept = ""
                '[ProductNumberのセット]------------------------------------------------------------
                If CntA = 1 Then
                    SupplierProductList.Rows(i).Cells(5).Text = st_ProductNumber
                Else
                    SupplierProductList.Rows(i).Cells(5).Text = SupplierProductList.Rows(i).Cells(5).Text & "<br>" & st_ProductNumber
                End If
                '[Statusのセット]-------------------------------------------------------------------
                If st_Status <> "" Then
                    DBCommand2.CommandText = "SELECT ENai FROM dbo.s_EhsPhrase WHERE PhID = N'" & st_Status & "'"
                    DBReader2 = DBCommand2.ExecuteReader()
                    DBCommand2.Dispose()
                    If DBReader2.Read = True Then
                        If CntA = 1 Then
                            SupplierProductList.Rows(i).Cells(6).Text = DBReader2("ENai")
                        Else
                            SupplierProductList.Rows(i).Cells(6).Text = SupplierProductList.Rows(i).Cells(6).Text & "<br>" & DBReader2("ENai")
                        End If
                    End If
                    DBReader2.Close()
                Else
                    If CntA = 1 Then
                        SupplierProductList.Rows(i).Cells(6).Text = "-"
                    Else
                        SupplierProductList.Rows(i).Cells(6).Text = SupplierProductList.Rows(i).Cells(6).Text & "<br>" & "-"
                    End If
                End If
                '[ProposalDeptのセット]-------------------------------------------------------------
                If st_ProposalDept <> "" Then
                    DBCommand2.CommandText = "SELECT ENai FROM dbo.s_EhsPhrase WHERE PhID = N'" & st_ProposalDept & "'"
                    DBReader2 = DBCommand2.ExecuteReader()
                    DBCommand2.Dispose()
                    If DBReader2.Read = True Then
                        If CntA = 1 Then
                            SupplierProductList.Rows(i).Cells(7).Text = DBReader2("ENai")
                        Else
                            SupplierProductList.Rows(i).Cells(7).Text = SupplierProductList.Rows(i).Cells(7).Text & "<br>" & DBReader2("ENai")
                        End If
                    End If
                    DBReader2.Close()
                Else
                    If CntA = 1 Then
                        SupplierProductList.Rows(i).Cells(7).Text = "-"
                    Else
                        SupplierProductList.Rows(i).Cells(7).Text = SupplierProductList.Rows(i).Cells(7).Text & "<br>" & "-"
                    End If
                End If
                '[ProcumentDeptのセット]------------------------------------------------------------
                If st_ProcumentDept <> "" Then
                    DBCommand2.CommandText = "SELECT ENai FROM dbo.s_EhsPhrase WHERE PhID = N'" & st_ProcumentDept & "'"
                    DBReader2 = DBCommand2.ExecuteReader()
                    DBCommand2.Dispose()
                    If DBReader2.Read = True Then
                        If CntA = 1 Then
                            SupplierProductList.Rows(i).Cells(8).Text = DBReader2("ENai")
                        Else
                            SupplierProductList.Rows(i).Cells(8).Text = SupplierProductList.Rows(i).Cells(8).Text & "<br>" & DBReader2("ENai")
                        End If
                    End If
                    DBReader2.Close()
                Else
                    If CntA = 1 Then
                        SupplierProductList.Rows(i).Cells(8).Text = "-"
                    Else
                        SupplierProductList.Rows(i).Cells(8).Text = SupplierProductList.Rows(i).Cells(8).Text & "<br>" & "-"
                    End If
                End If
            Loop
            DBReader.Close()

            '[CASNumberチェック]-------------------------------------------------------------
            If TCICommon.Func.IsCASNumber(table.Rows(i).Item("CAS Number")) = False Then
                Msg.Text = "ERROR CAS_Number"
                SupplierProductList.Rows(i).Cells(1).ForeColor = Drawing.Color.Red
                Exit For
            End If

            '[AD,AF,WA,KAにイメージ挿入]-----------------------------------------------------
            If SupplierProductList.Rows(i).Cells(9).Text = "1" Then SupplierProductList.Rows(i).Cells(9).Text = "<img src=""./Image/Check.gif"" />" Else SupplierProductList.Rows(i).Cells(9).Text = ""
            If SupplierProductList.Rows(i).Cells(10).Text = "1" Then SupplierProductList.Rows(i).Cells(10).Text = "<img src=""./Image/Check.gif"" />" Else SupplierProductList.Rows(i).Cells(10).Text = ""
            If SupplierProductList.Rows(i).Cells(11).Text = "1" Then SupplierProductList.Rows(i).Cells(11).Text = "<img src=""./Image/Check.gif"" />" Else SupplierProductList.Rows(i).Cells(11).Text = ""
            If SupplierProductList.Rows(i).Cells(12).Text = "1" Then SupplierProductList.Rows(i).Cells(12).Text = "<img src=""./Image/Check.gif"" />" Else SupplierProductList.Rows(i).Cells(12).Text = ""
        Next i

        '[Import.Visibleの設定]---------------------------------------------------------
        If Msg.Text.ToString = "" Then Import.Visible = True Else Import.Visible = False

        '[作成したTableを削除する]------------------------------------------------------
        Kill(ImportFileName.Value)
    End Sub

    Protected Sub Import_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Import.Click
        '[Data Import]------------------------------------------------------------------
        If Request.Form("Action") <> "Import" Then
            Msg.Text = "Importできる環境でありません"
            Exit Sub
        End If

        For i = 0 To SupplierProductList.Rows.Count - 1
            Dim sqlTran As System.Data.SqlClient.SqlTransaction = DBConn.BeginTransaction()
            DBCommand.Transaction = sqlTran
            Try
                st_CAS = SupplierProductList.Rows.Item(i).Cells(1).Text()
                st_ItemNo = SupplierProductList.Rows.Item(i).Cells(2).Text()
                st_ItemName = SupplierProductList.Rows.Item(i).Cells(3).Text()
                st_Note = SupplierProductList.Rows.Item(i).Cells(4).Text()
                DBCommand.CommandText = "SELECT ProductID, ProductNumber, NumberType, Name, CASNumber FROM dbo.Product WHERE CASNumber = '" & st_CAS & "'"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    DBReader.Close()
                    DBCommand.CommandText = "SELECT ProductID, ProductNumber, NumberType, Name, CASNumber FROM dbo.Product WHERE CASNumber = '" & st_CAS & "'"
                    DBReader = DBCommand.ExecuteReader()
                    DBCommand.Dispose()
                    Do Until DBReader.Read = False
                        '[ProductIDの記憶]-------------------------------------------------------
                        ProductID.Value = DBReader("ProductID")

                        '[ProductNumber='CAS'の場合Productを更新]--------------------------------
                        If DBReader("NumberType") = "CAS" Then
                            st_SqlStr = "UPDATE [Product] SET ProductNumber='" & st_CAS & "',"
                            If st_ItemName <> "" Then st_SqlStr = st_SqlStr & "Name='" & st_ItemName & "',"
                            st_SqlStr = st_SqlStr & "CASNumber='" & st_CAS & "',UpdatedBy=" & Session("UserID") & ", UpdateDate='" & Now() & "'  WHERE ProductID='" & ProductID.Value & "' AND CASNumber = '" & st_CAS & "'"
                            DBCommand2.CommandText = st_SqlStr
                            DBCommand2.ExecuteNonQuery()
                        End If

                        '[Supplier_Productの存在確認]--------------------------------------------
                        DBCommand2.CommandText = "SELECT SupplierCode, ProductID, SupplierItemNumber, Note FROM dbo.Supplier_Product WHERE (ProductID = '" & ProductID.Value & "') AND (SupplierCode = '" & SupplierCode.Text.ToString & "')"
                        DBReader2 = DBCommand2.ExecuteReader()
                        DBCommand2.Dispose()
                        If DBReader2.Read = True Then
                            DBReader2.Close()
                            '[Supplier_Productの更新]--------------------------------------------
                            st_SqlStr = "UPDATE Supplier_Product SET SupplierItemNumber="
                            If st_ItemNo = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & st_ItemNo & "',"
                            st_SqlStr = st_SqlStr & "Note="
                            If st_Note = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & st_Note & "',"
                            st_SqlStr = st_SqlStr & "UpdatedBy=" & Session("UserID") & ", UpdateDate='" & Now() & "' "
                            st_SqlStr = st_SqlStr & "WHERE (ProductID = '" & ProductID.Value & "') AND (SupplierCode = '" & SupplierCode.Text.ToString & "')"
                            DBCommand2.CommandText = st_SqlStr
                            DBCommand2.ExecuteNonQuery()
                        Else
                            DBReader2.Close()
                            '[Supplier_Product登録]----------------------------------------------
                            st_SqlStr = "INSERT INTO Supplier_Product (SupplierCode,ProductID,SupplierItemNumber,Note,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ("
                            st_SqlStr = st_SqlStr & SupplierCode.Text.ToString & "," & ProductID.Value & ","
                            If st_ItemNo = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & st_ItemNo & "',"
                            If st_Note = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & st_Note & "',"
                            st_SqlStr = st_SqlStr & "'" & Session("UserID") & "','" & Now() & "','" & Session("UserID") & "','" & Now() & "')"
                            DBCommand2.CommandText = st_SqlStr
                            DBCommand2.ExecuteNonQuery()
                        End If
                    Loop
                    DBReader.Close()
                Else
                    DBReader.Close()
                    '[Product追加処理]-------------------------------------------------------
                    st_SqlStr = "INSERT INTO Product (ProductNumber,NumberType,Name,QuoName,JapaneseName,ChineseName,CASNumber,MolecularFormula,Status,ProposalDept,ProcumentDept,PD,Reference,Comment,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ("
                    If st_CAS = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + st_CAS + "',"
                    st_SqlStr = st_SqlStr + "'CAS',"
                    If st_ItemName = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + st_ItemName + "',"
                    st_SqlStr = st_SqlStr + "null,null,null,"
                    If st_CAS = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + st_CAS + "',"
                    st_SqlStr = st_SqlStr + "null,null,null,null,null,null,null,"
                    st_SqlStr = st_SqlStr + Session("UserID") + ",'" + Now() + "'," + Session("UserID") + ",'" + Now() + "')"
                    DBCommand.CommandText = st_SqlStr
                    DBCommand.ExecuteNonQuery()

                    '[新規登録されたProductIDの取得]--------------------------------------------------
                    DBCommand.CommandText = "Select @@IDENTITY as ProductID"
                    DBReader = DBCommand.ExecuteReader()
                    DBCommand.Dispose()
                    If DBReader.Read = True Then
                        ProductID.Value = DBReader("ProductID")
                    End If
                    DBReader.Close()

                    '[Supplier_Product登録]--------------------------------------------------
                    st_SqlStr = "INSERT INTO Supplier_Product (SupplierCode,ProductID,SupplierItemNumber,Note,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ("
                    st_SqlStr = st_SqlStr & SupplierCode.Text.ToString & "," & ProductID.Value & ","
                    If st_ItemNo = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & st_ItemNo & "',"
                    If st_Note = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & st_Note & "',"
                    st_SqlStr = st_SqlStr & "'" & Session("UserID") & "','" & Now() & "','" & Session("UserID") & "','" & Now() & "')"
                    DBCommand.CommandText = st_SqlStr
                    DBCommand.ExecuteNonQuery()
                End If
                'ここまでエラーがなかったらコミット
                sqlTran.Commit()
            Catch ex As Exception
                'エラーがあった場合はロールバック
                sqlTran.Rollback()
            End Try
        Next
        Response.Redirect("./ProductListBySupplier.aspx?Supplier=" & SupplierCode.Text.ToString)
    End Sub

    Private Sub up(ByVal sender As Object, ByVal e As EventArgs) Handles SupplierProductList.RowEditing

    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
        DBConn2.Close()
    End Sub
End Class
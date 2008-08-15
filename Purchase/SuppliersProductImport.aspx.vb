Imports System.Data.SqlClient
Imports Purchase.Common

Partial Public Class SuppliersProductImport
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

    ''' <summary>
    ''' このページのロードイベントです。
    ''' </summary>
    ''' <param name="sender">ASP.NETの規定値</param>
    ''' <param name="e">ASP.NETの規定値</param>
    ''' <remarks></remarks>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Set_DBConnectingString()
        DBConn = Me.SqlConnection1
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()
        DBConn2 = Me.SqlConnection2
        DBConn2.Open()
        DBCommand2 = DBConn2.CreateCommand()

        If IsPostBack = False Then
            If Request.QueryString("Supplier") <> "" Then
                Dim st_SupplierCode = Request.QueryString("Supplier").ToString()
                SupplierCode.Text = st_SupplierCode
                SupplierName.Text = GetSupplierNameBySupplierCode(st_SupplierCode)
            Else
                Msg.Text = "SupplierCodeが設定されていません"
                File.Visible = False
                Preview.Visible = False
                Import.Visible = False
            End If
        End If
    End Sub

    ''' <summary>
    ''' サプライヤーコードからサプライヤーの名称を取得します。
    ''' </summary>
    ''' <param name="SupplierCode">サプライヤーコード</param>
    ''' <returns>サプライヤーの名称</returns>
    ''' <remarks></remarks>
    Private Function GetSupplierNameBySupplierCode(ByVal SupplierCode As String) As String
        Dim st_supplierName As String = String.Empty
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()

            cmd.CommandText = "SELECT Name3 FROM Supplier WHERE SupplierCode = @SupplierCode"
            cmd.Parameters.AddWithValue("SupplierCode", SupplierCode)

            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.Read = True Then
                st_supplierName = dr("Name3").ToString()
            End If
        Catch ex As Exception
            Throw
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
        Return st_supplierName
    End Function


    Protected Sub Preview_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Preview.Click
        Msg.Text = ""
        '[Action確認]------------------------------------------------------------------------
        If Request.Form("Action") <> "Preview" Then
            Msg.Text = "Previewできる環境でありません"
            Exit Sub
        End If

        '[Preview実行環境確認]--------------------------------------------------------------
        If IO.Path.GetFileName(File.PostedFile.FileName) <> "" Then
            '[読込みファイルがEXCELか確認]------------------------------------------------------
            If Request.Files("File").ContentType <> "application/vnd.ms-excel" Then
                SupplierProductListClear()
                Msg.Text = "読込みファイルはEXCELでありません"
                Exit Sub
            Else
                ImportFileName.Value = IO.Path.GetFileName(File.PostedFile.FileName)
            End If
        Else
            SupplierProductListClear()
            Msg.Text = "読込みファイルが設定されていません"
            Exit Sub
        End If

        '[読込みファイルをGridViewに必要項目を付加して表示]---------------------------------
        Dim st_ProductNumber As String = ""     'SupplierProductListに表示するProductNumber
        Dim st_Status As String = ""            'SupplierProductListに表示するStatus(最終的にはs_EhsPhraseのENai)
        Dim st_ProposalDept As String = ""      'SupplierProductListに表示するProposalDept(最終的にはs_EhsPhraseのENai)
        Dim st_ProcumentDept As String = ""     'SupplierProductListに表示するProcumentDept(最終的にはs_EhsPhraseのENai)
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

        '[4社の取扱状況表示]----------------------------------------------------------------
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

        '[SupplierProductListの表示]--------------------------------------------------------
        SupplierProductList.DataSource = table
        SupplierProductList.DataBind()

        '[付加項目にデータをセットする]-----------------------------------------------------
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


                '[Statusのデータ取得]---------------------------------------------------------------
                st_Status = GetEhsPhraseNameByPhID(st_Status)

                '[ProposalDeptのデータ取得]---------------------------------------------------------
                st_ProposalDept = GetEhsPhraseNameByPhID(st_ProposalDept)

                '[ProcumentDeptのセット]------------------------------------------------------------
                st_ProcumentDept = GetEhsPhraseNameByPhID(st_ProcumentDept)


                'Dim st_Separator As String = String.Empty
                'If CntA > 1 Then
                '    st_Separator = "<br>"
                'End If
                'SupplierProductList.Rows(i).Cells(4).Text &= st_Separator & st_ProductNumber
                'SupplierProductList.Rows(i).Cells(5).Text &= st_Separator & st_Status
                'SupplierProductList.Rows(i).Cells(6).Text &= st_Separator & st_ProposalDept
                'SupplierProductList.Rows(i).Cells(7).Text &= st_Separator & st_ProcumentDept


                '[SupplierProductListに追加項目表示]------------------------------------------------
                If CntA = 1 Then
                    SupplierProductList.Rows(i).Cells(4).Text = st_ProductNumber
                    SupplierProductList.Rows(i).Cells(5).Text = st_Status
                    SupplierProductList.Rows(i).Cells(6).Text = st_ProposalDept
                    SupplierProductList.Rows(i).Cells(7).Text = st_ProcumentDept
                Else
                    SupplierProductList.Rows(i).Cells(4).Text = SupplierProductList.Rows(i).Cells(4).Text & "<br>" & st_ProductNumber
                    SupplierProductList.Rows(i).Cells(5).Text = SupplierProductList.Rows(i).Cells(5).Text & "<br>" & st_Status
                    SupplierProductList.Rows(i).Cells(6).Text = SupplierProductList.Rows(i).Cells(6).Text & "<br>" & st_ProposalDept
                    SupplierProductList.Rows(i).Cells(7).Text = SupplierProductList.Rows(i).Cells(7).Text & "<br>" & st_ProcumentDept
                End If
            Loop
            DBReader.Close()

            '[CASNumberチェック]-------------------------------------------------------------
            If TCICommon.Func.IsCASNumber(table.Rows(i).Item("CAS Number")) = False Then
                Msg.Text = "ERROR CAS_Number"

                'TODO 16進表記からSystem.Drawing.Colorへの変換処理が必要
                Dim i_CellsCount As Integer = 0
                For i_CellsCount = 0 To SupplierProductList.Rows(i).Cells.Count - 1
                    SupplierProductList.Rows(i).Cells(i_CellsCount).BackColor = Drawing.Color.Red
                Next
                Continue For
            End If

            '[AD,AF,WA,KAにイメージ挿入]-----------------------------------------------------
            If SupplierProductList.Rows(i).Cells(8).Text = "1" Then SupplierProductList.Rows(i).Cells(8).Text = "<img src=""./Image/Check.gif"" />" Else SupplierProductList.Rows(i).Cells(8).Text = ""
            If SupplierProductList.Rows(i).Cells(9).Text = "1" Then SupplierProductList.Rows(i).Cells(9).Text = "<img src=""./Image/Check.gif"" />" Else SupplierProductList.Rows(i).Cells(9).Text = ""
            If SupplierProductList.Rows(i).Cells(10).Text = "1" Then SupplierProductList.Rows(i).Cells(10).Text = "<img src=""./Image/Check.gif"" />" Else SupplierProductList.Rows(i).Cells(10).Text = ""
            If SupplierProductList.Rows(i).Cells(11).Text = "1" Then SupplierProductList.Rows(i).Cells(11).Text = "<img src=""./Image/Check.gif"" />" Else SupplierProductList.Rows(i).Cells(11).Text = ""
        Next i

        '[Import.Visibleの設定]---------------------------------------------------------
        'If Msg.Text.ToString = "" Then Import.Visible = True Else Import.Visible = False
    End Sub

    ''' <summary>
    ''' PhIDからEhsPhraseの英名を取得します。
    ''' </summary>
    ''' <param name="PhID">PhID</param>
    ''' <returns>EhsPhraseの英名</returns>
    ''' <remarks></remarks>
    Private Function GetEhsPhraseNameByPhID(ByVal PhID As String) As String
        Dim st_EhsPhraseName As String = String.Empty
        Dim conn As SqlConnection = Nothing
        Try
            conn = New SqlConnection(DB_CONNECT_STRING)
            Dim cmd As SqlCommand = conn.CreateCommand()

            cmd.CommandText = "SELECT ENai FROM dbo.s_EhsPhrase WHERE PhID = @PhID"
            cmd.Parameters.AddWithValue("PhID", PhID)

            conn.Open()
            Dim dr As SqlDataReader = cmd.ExecuteReader()
            If dr.Read = True Then
                st_EhsPhraseName = dr("ENai").ToString()
            Else
                st_EhsPhraseName = "-"
            End If
        Catch ex As Exception
            Throw
        Finally
            If Not conn Is Nothing Then
                conn.Close()
            End If
        End Try
        Return st_EhsPhraseName
    End Function


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

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
        DBConn2.Close()
    End Sub

    Public Sub SupplierProductListClear()
        SupplierProductList.DataSourceID = ""
        SupplierProductList.DataSource = ""
        SupplierProductList.DataBind()
        Import.Visible = False
    End Sub

End Class
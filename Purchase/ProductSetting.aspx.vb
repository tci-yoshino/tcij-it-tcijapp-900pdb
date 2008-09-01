Public Partial Class ProductSetting
    Inherits CommonPage

#Region " Region "
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.SqlConnection1 = New System.Data.SqlClient.SqlConnection
        Me.SqlConnection1.FireInfoMessageEventOnUserErrors = False
    End Sub

    Protected WithEvents SqlConnection1 As System.Data.SqlClient.SqlConnection

    Private Sub Page_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Init
        InitializeComponent()
    End Sub
#End Region

    Dim DBConnString As String                              '接続文字列	
    Dim DBConn As New System.Data.SqlClient.SqlConnection   'データベースコネクション	
    Dim DBCommand As System.Data.SqlClient.SqlCommand       'データベースコマンド	
    Dim DBReader As System.Data.SqlClient.SqlDataReader     'データリーダー	
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
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Set_DBConnectingString()
        DBConn = Me.SqlConnection1
        DBConn.Open()
        DBCommand = DBConn.CreateCommand()

        If IsPostBack = False Then
            '[ProductIDのセット]------------------------------------------------------------------------
            Mode.Value = Request.QueryString("Action")
            ProductID.Value = Request.QueryString("ProductID")

            If Mode.Value = "Edit" Then
                DBCommand.CommandText = "SELECT ProductNumber, Name, QuoName, CASNumber, MolecularFormula, Reference, Comment, Status, ProposalDept, ProcumentDept, PD, UpdateDate " & _
                                        "FROM dbo.Product WHERE (ProductID = " + ProductID.Value + ")"
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    If Not TypeOf DBReader("ProductNumber") Is DBNull Then ProductNumber.Text = DBReader("ProductNumber")
                    If Not TypeOf DBReader("Name") Is DBNull Then ProductName.Text = DBReader("Name")
                    If Not TypeOf DBReader("QuoName") Is DBNull Then QuoName.Text = DBReader("QuoName")
                    If Not TypeOf DBReader("CASNumber") Is DBNull Then CASNumber.Text = DBReader("CASNumber")
                    If Not TypeOf DBReader("MolecularFormula") Is DBNull Then MolecularFormula.Text = DBReader("MolecularFormula")
                    If Not TypeOf DBReader("Reference") Is DBNull Then Reference.Text = DBReader("Reference")
                    If Not TypeOf DBReader("Comment") Is DBNull Then Comment.Text = DBReader("Comment")
                    If Not TypeOf DBReader("Status") Is DBNull Then Status.Text = DBReader("Status")
                    If Not TypeOf DBReader("ProposalDept") Is DBNull Then ProposalDept.Text = DBReader("ProposalDept")
                    If Not TypeOf DBReader("ProcumentDept") Is DBNull Then ProcumentDept.Text = DBReader("ProcumentDept")
                    If Not TypeOf DBReader("PD") Is DBNull Then PD.Text = DBReader("PD")
                    UpdateDate.Value = Common.GetUpdateDate("Product", "ProductID", ProductID.Value) '[同時更新チェック用]
                Else
                    UpdateDate.Value = ""
                End If
                DBReader.Close()
            End If
        End If

        If ProductID.Value <> "" Then
            SupplierList.NavigateUrl = "./SupplierListByProduct.aspx?ProductID=" & ProductID.Value
        Else
            SupplierList.NavigateUrl = "./SupplierListByProduct.aspx"
        End If
    End Sub

    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        Dim NumberType As String = ""
        Dim st_SqlStr As String = ""
        Msg.Text = ""

        '[入力ProductNumberの正規化]---------------------------------------------------
        ProductNumber.Text = StrConv(ProductNumber.Text, VbStrConv.Narrow)
        ProductNumber.Text = UCase(ProductNumber.Text)

        '[Actionチェック]--------------------------------------------------------------
        If Action.Value <> "Save" Then
            Msg.Text = "Saveは拒否されました"
            Exit Sub
        End If

        '[必須項目入力チェック]--------------------------------------------------------
        If ProductNumber.Text.ToString = "" Or ProductName.Text.ToString = "" Then
            Msg.Text = "必須項目を入力して下さい"
            Exit Sub
        End If

        '[ProductNumber重複チェック]---------------------------------------------------
        If Mode.Value = "Edit" Then
            DBCommand.CommandText = "SELECT ProductNumber FROM Product WHERE ProductNumber = '" & Common.SafeSqlLiteral(ProductNumber.Text) & "' AND ProductID<>" & ProductID.Value
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                Msg.Text = "同じ Product Number のデータが既に登録されています。ご確認ください。"
                Exit Sub
            End If
            DBReader.Close()
        End If

        '[入力項目のLengthCheck]-------------------------------------------------------
        If Reference.Text.Length > 3000 Then
            Msg.Text = "Referenceが3000桁以上です。"
            Exit Sub
        End If
        If Comment.Text.Length > 3000 Then
            Msg.Text = "Commentが3000桁以上です。"
            Exit Sub
        End If

        '[CASNumberチェック]-----------------------------------------------------------
        NumberType = ""
        If TCICommon.Func.IsCASNumber(ProductNumber.Text.ToString) = True Then NumberType = "CAS"
        If TCICommon.Func.IsProductNumber(ProductNumber.Text.ToString) = True Then NumberType = "TCI"
        If TCICommon.Func.IsNewProductNumber(ProductNumber.Text.ToString) = True Then NumberType = "NEW"
        If NumberType = "" Then
            Msg.Text = "ProductNumberTypeが決定できません。"
            Exit Sub
        End If

        If CASNumber.Text <> "" Then
            If TCICommon.Func.IsCASNumber(CASNumber.Text.ToString) = False Then
                Msg.Text = "ERROR CAS_Number"
                Exit Sub
            Else
                If NumberType = "CAS" Then
                    If ProductNumber.Text <> CASNumber.Text Then
                        Msg.Text = "ProductNumberとCAS_Numberが異なります。"
                        Exit Sub
                    End If
                End If
            End If
        End If

        '[Save処理]--------------------------------------------------------------------
        If Mode.Value = "Edit" Then
            '[ProductのUpdateDateチェック]-----------------------------------------------------------
            DBCommand.CommandText = "SELECT UpdateDate FROM dbo.Product WHERE ProductID = '" & ProductID.Value & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                If Common.GetUpdateDate("Product", "ProductID", ProductID.Value) = UpdateDate.Value Then
                    DBReader.Close()
                    '[Product更新処理]---------------------------------------------------------------
                    st_SqlStr = "UPDATE dbo.Product SET ProductNumber="
                    If ProductNumber.Text.ToString = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & Common.SafeSqlLiteral(ProductNumber.Text) & "',"
                    st_SqlStr = st_SqlStr + "NumberType='" + NumberType + "',"
                    st_SqlStr = st_SqlStr & "Name="
                    If ProductName.Text.ToString = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & Common.SafeSqlLiteral(ProductName.Text) & "',"
                    st_SqlStr = st_SqlStr & "QuoName="
                    If QuoName.Text.ToString = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & Common.SafeSqlLiteral(QuoName.Text) & "',"
                    st_SqlStr = st_SqlStr & "CASNumber="
                    If CASNumber.Text.ToString = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & Common.SafeSqlLiteral(CASNumber.Text) & "',"
                    st_SqlStr = st_SqlStr & "MolecularFormula="
                    If MolecularFormula.Text.ToString = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & Common.SafeSqlLiteral(MolecularFormula.Text) & "',"
                    st_SqlStr = st_SqlStr & "Reference="
                    If Reference.Text.ToString = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & Common.SafeSqlLiteral(Reference.Text) & "',"
                    st_SqlStr = st_SqlStr & "Comment="
                    If Comment.Text.ToString = "" Then st_SqlStr = st_SqlStr & "null," Else st_SqlStr = st_SqlStr & "'" & Common.SafeSqlLiteral(Comment.Text) & "',"
                    st_SqlStr = st_SqlStr & "UpdatedBy=" & Session("UserID") & ", UpdateDate='" & Now() & "' "
                    st_SqlStr = st_SqlStr & "WHERE ProductID = '" & ProductID.Value & "'"
                    DBCommand.CommandText = st_SqlStr
                    DBCommand.ExecuteNonQuery()
                    Msg.Text = "データを更新しました。"

                    '[引き続き更新処理ができるようにUpdateDate設定]----------------------------------
                    UpdateDate.Value = Common.GetUpdateDate("Product", "ProductID", ProductID.Value) '[同時更新チェック用]
                Else
                    DBReader.Close()
                    Msg.Text = "データは他のユーザによって既に更新されています。ご確認ください。"
                End If
            Else
                DBReader.Close()
            End If
        Else
            '[Productの存在チェック]-----------------------------------------------------------
            DBCommand.CommandText = "SELECT ProductID FROM dbo.Product WHERE ProductNumber = '" & Common.SafeSqlLiteral(ProductNumber.Text) & "'"
            DBReader = DBCommand.ExecuteReader()
            DBCommand.Dispose()
            If DBReader.Read = True Then
                Msg.Text = "このデータはすでに登録済です。その内容を確認し再度処理をお願いします。"
            End If
            DBReader.Close()

            If Msg.Text.ToString = "" Then
                '[Product追加処理]-----------------------------------------------------------------------
                st_SqlStr = "INSERT INTO Product (ProductNumber,NumberType,Name,QuoName,JapaneseName,ChineseName,CASNumber,MolecularFormula,Status,ProposalDept,ProcumentDept,PD,Reference,Comment,CreatedBy,CreateDate,UpdatedBy,UpdateDate) values ("
                If ProductNumber.Text.ToString = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + Common.SafeSqlLiteral(ProductNumber.Text) + "',"
                st_SqlStr = st_SqlStr + "'" + NumberType + "',"
                If ProductName.Text.ToString = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + Common.SafeSqlLiteral(ProductName.Text) + "',"
                If QuoName.Text.ToString = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + Common.SafeSqlLiteral(QuoName.Text) + "',"
                st_SqlStr = st_SqlStr + "null,null,"
                If CASNumber.Text.ToString = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + Common.SafeSqlLiteral(CASNumber.Text) + "',"
                If MolecularFormula.Text.ToString = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + Common.SafeSqlLiteral(MolecularFormula.Text) + "',"
                st_SqlStr = st_SqlStr + "null,null,null,null,"
                If Reference.Text.ToString = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + Common.SafeSqlLiteral(Reference.Text) + "',"
                If Comment.Text.ToString = "" Then st_SqlStr = st_SqlStr + "null," Else st_SqlStr = st_SqlStr + "'" + Common.SafeSqlLiteral(Comment.Text) + "',"
                st_SqlStr = st_SqlStr + Session("UserID") + ",'" + Now() + "'," + Session("UserID") + ",'" + Now() + "'); "
                st_SqlStr = st_SqlStr & "SELECT ProductID FROM Product WHERE ProductID = SCOPE_IDENTITY()"  '←[新規登録されたProductIDの取得の為]
                DBCommand.CommandText = st_SqlStr
                DBReader = DBCommand.ExecuteReader()
                DBCommand.Dispose()
                If DBReader.Read = True Then
                    ProductID.Value = DBReader("ProductID")
                    SupplierList.NavigateUrl = "./SupplierListByProduct.aspx?ProductID=" & ProductID.Value
                End If
                DBReader.Close()
                Msg.Text = "データを登録しました。"

                '[引き続き更新処理ができるようにUpdateDate設定]---------------------------------
                UpdateDate.Value = Common.GetUpdateDate("Product", "ProductID", ProductID.Value) '[同時更新チェック用]
                Mode.Value = "Edit"
            End If
        End If
    End Sub

    Private Sub Page_Unload(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Unload
        DBConn.Close()
    End Sub
End Class
Imports Purchase.Common

Partial Public Class SupplierSearch
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            SrcSupplier.SelectCommand = ""
            SupplierList.Visible = False
        End If
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        Msg.Text = String.Empty
        SupplierList.Visible = False

        '[Search実行可能確認]-------------------------------------------------------------
        If Action.Value <> "Search" Then
            Msg.Text = ERR_INVALID_PARAMETER
            Exit Sub
        End If

        '[Code,R3Codeを1Byte形式に変換する]-----------------------------------------------
        Code.Text = StrConv(Code.Text.ToString, VbStrConv.Narrow)
        R3Code.Text = StrConv(R3Code.Text.ToString, VbStrConv.Narrow)

        '[検索項目が入力されなかった場合]-------------------------------------------------
        If Trim(Code.Text).Length = 0 Then Code.Text = ""
        If Trim(R3Code.Text).Length = 0 Then R3Code.Text = ""
        If Trim(Name.Text).Length = 0 Then Name.Text = ""
        If Code.Text.Length + R3Code.Text.Length + Name.Text.Length = 0 Then
            UnDsp_SrcSupplier()
            Exit Sub
        End If


        '[検索項目が入力された場合]-------------------------------------------------------
        Dim SQLStr As String = ""
        SrcSupplier.SelectCommand = "SELECT SupplierCode AS [Supplier Code], R3SupplierCode AS [R/3 Supplier Code], ISNULL(Name3, '') + N' ' + ISNULL(Name4, '') AS [Supplier Name], './SupplierSetting.aspx?Action=Edit&Code=' + rtrim(ltrim(str([SupplierCode]))) AS Url  FROM dbo.Supplier "
        If Code.Text.ToString <> "" Then
            SQLStr = SQLStr + "WHERE (SupplierCode = " + SafeSqlLiteral(Code.Text) + ")"
            If Not IsNumeric(SafeSqlLiteral(Code.Text)) Then
                UnDsp_SrcSupplier()
                Exit Sub
            End If
        End If

        '[R3Codeが数字の場合と文字の場合とでは検索が異なる]-------------------------------
        If R3Code.Text.ToString <> "" Then
            If SQLStr = "" Then SQLStr = "WHERE " Else SQLStr = SQLStr + " AND "
            If IsNumeric(R3Code.Text.ToString) And R3Code.Text Like "*.*" = False Then
                SQLStr = SQLStr + "(R3SupplierCode = " + SafeSqlLiteral(R3Code.Text) + ")"
            Else
                SQLStr = SQLStr + "(R3SupplierCode = '" + SafeSqlLiteral(R3Code.Text) + "')"
            End If
        End If
        If Name.Text.ToString <> "" Then
            If SQLStr = "" Then SQLStr = "WHERE " Else SQLStr = SQLStr + " AND "
            SQLStr = SQLStr + "ISNULL(Name3,'') + N' ' + ISNULL(Name4,'') LIKE '%" + SafeSqlLikeClauseLiteral(Name.Text) + "%'"
        End If

        '[SrcSupplierの表示]--------------------------------------------------------------
        SrcSupplier.SelectCommand = SrcSupplier.SelectCommand + SQLStr
        SupplierList.Visible = True
    End Sub

    Public Sub UnDsp_SrcSupplier()
        SrcSupplier.SelectCommand = ""
        SupplierList.Visible = True
    End Sub
End Class
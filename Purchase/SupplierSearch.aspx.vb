﻿Public Partial Class SupplierSearch
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            SrcSupplier.SelectCommand = ""
            SupplierList.Visible = False
        End If
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        '[Code,R3Codeを1Byte形式に変換する]-----------------------------------------------
        Code.Text = StrConv(Code.Text.ToString, VbStrConv.Narrow)
        R3Code.Text = StrConv(R3Code.Text.ToString, VbStrConv.Narrow)

        Msg.Text = ""
        SupplierList.Visible = False

        '[何も入力されていない場合検索しない]---------------------------------------------
        If Trim(Code.Text).Length = 0 Then Code.Text = ""
        If Trim(R3Code.Text).Length = 0 Then R3Code.Text = ""
        If Trim(Name.Text).Length = 0 Then Name.Text = ""
        If Code.Text.Length + R3Code.Text.Length + Name.Text.Length = 0 Then Exit Sub

        '[Supplier検索]-------------------------------------------------------------------
        If Code.Text.ToString <> "" And Not IsNumeric(Code.Text.ToString) Then
            Msg.Text = "SupplierCodeには数字を入力して下さい"
            Exit Sub
        ElseIf Code.Text Like "*.*" = True Then
            Msg.Text = "SupplierCodeには整数を入力して下さい"
            Exit Sub
        End If

        SupplierList.Visible = True

        Dim SQLStr As String = ""
        SrcSupplier.SelectCommand = "SELECT SupplierCode AS [Supplier Code], R3SupplierCode AS [R/3 Supplier Code], ISNULL(Name3, '') + N' ' + ISNULL(Name4, '') AS [Supplier Name], './SupplierSetting.aspx?Action=Edit&Code=' + rtrim(ltrim(str([SupplierCode]))) AS Url  FROM dbo.Supplier "
        If Code.Text.ToString <> "" Then
            If SQLStr = "" Then SQLStr = "WHERE "
            SQLStr = SQLStr + "(SupplierCode = '" + Common.SafeSqlLiteral(Code.Text) + "')"
        End If
        '[R3Codeが数字の場合と文字の場合とでは検索が異なる]---------------------------
        If R3Code.Text.ToString <> "" Then
            If SQLStr = "" Then SQLStr = "WHERE " Else SQLStr = SQLStr + " AND "
            If IsNumeric(R3Code.Text.ToString) And R3Code.Text Like "*.*" = False Then
                SQLStr = SQLStr + "(R3SupplierCode = " + Common.SafeSqlLiteral(R3Code.Text) + ")"
            Else
                SQLStr = SQLStr + "(R3SupplierCode = '" + Common.SafeSqlLiteral(R3Code.Text) + "')"
            End If
        End If
        If Name.Text.ToString <> "" Then
            If SQLStr = "" Then SQLStr = "WHERE " Else SQLStr = SQLStr + " AND "
            SQLStr = SQLStr + "ISNULL(Name1,'') + N' ' + ISNULL(Name2,'') LIKE '%" + Common.SafeSqlLikeClauseLiteral(Name.Text) + "%'"
        End If

        '[検索項目すべて指定しない場合は結果無しとする]-------------------------------
        If SQLStr = "" Then
            SrcSupplier.SelectCommand = ""
        Else
            SrcSupplier.SelectCommand = SrcSupplier.SelectCommand + SQLStr
        End If
    End Sub

End Class
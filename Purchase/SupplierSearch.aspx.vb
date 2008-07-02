Public Partial Class SupplierSearch
    Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SrcSupplier.SelectCommand = ""
    End Sub

    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click
        '[Code,R3Codeを1Byte形式に変換する]-----------------------------------------------
        Code.Text = StrConv(Code.Text.ToString, VbStrConv.Narrow)
        R3Code.Text = StrConv(R3Code.Text.ToString, VbStrConv.Narrow)

        '[Supplier検索]-------------------------------------------------------------------
        If Code.Text.ToString <> "" And Not IsNumeric(Code.Text.ToString) Then
            Msg.Text = "SupplierCodeには数字を入力して下さい"
            SrcSupplier.SelectCommand = ""
            SupplierList.DataBind()
        Else
            Msg.Text = ""
            Dim SQLStr As String = ""
            SrcSupplier.SelectCommand = "SELECT SupplierCode AS [Supplier Code], R3SupplierCode AS [R/3 Supplier Code], ISNULL(Name3, '') + N' ' + ISNULL(Name4, '') AS [Supplier Name], './SupplierSetting.aspx?Action=Edit&Code=' + rtrim(ltrim(str([SupplierCode]))) AS Url  FROM dbo.Supplier "
            If Code.Text.ToString <> "" Then
                If SQLStr = "" Then SQLStr = "WHERE "
                SQLStr = SQLStr + "(SupplierCode = '" & Code.Text.ToString & "')"
            End If
            '[R3Codeが数字の場合と文字の場合とでは検索が異なる]---------------------------
            If R3Code.Text.ToString <> "" Then
                If SQLStr = "" Then SQLStr = "WHERE " Else SQLStr = SQLStr + " AND "
                If IsNumeric(R3Code.Text.ToString) Then
                    SQLStr = SQLStr + "((R3SupplierCode) = " & (R3Code.Text.ToString) & ")"
                Else
                    SQLStr = SQLStr + "((R3SupplierCode) = '" & (R3Code.Text.ToString) & "')"
                End If
            End If
            If Name.Text.ToString <> "" Then
                If SQLStr = "" Then SQLStr = "WHERE " Else SQLStr = SQLStr + " AND "
                SQLStr = SQLStr + "(Name1 + N' ' + Name2 LIKE '%" & Name.Text.ToString & "%')"
            End If

            '[検索項目すべて指定しない場合は結果無しとする]-------------------------------
            If SQLStr = "" Then
                SrcSupplier.SelectCommand = ""
            Else
                SrcSupplier.SelectCommand = SrcSupplier.SelectCommand + SQLStr
            End If
        End If
    End Sub

End Class
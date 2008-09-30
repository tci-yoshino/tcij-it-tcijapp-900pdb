﻿Partial Public Class SupplierSelect
    Inherits CommonPage

    ' 接続文字列
    Private st_Code As String = String.Empty
    Private st_Name As String = String.Empty
    Private st_Errorr_Meggage As String = String.Empty
    Const SEARCH_ACTION As String = "Search"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' コントロール初期化
        Msg.Text = ""

        ' パラメータを取得
        If Request.RequestType = "POST" Then
            st_Code = IIf(Request.Form("Code") = Nothing, "", Request.Form("Code"))
            st_Name = IIf(Request.Form("Name") = Nothing, "", Request.Form("Name"))
        ElseIf Request.RequestType = "GET" Then
            st_Code = IIf(Request.QueryString("Code") = Nothing, "", Request.QueryString("Code"))
            st_Name = IIf(Request.QueryString("Name") = Nothing, "", Request.QueryString("Name"))
        End If

        ' 空白除去
        st_Code = Trim(st_Code)
        st_Name = Trim(st_Name)

        ' URL デコード
        st_Code = HttpUtility.UrlDecode(st_Code)
        st_Name = HttpUtility.UrlDecode(st_Name)

        ' 全角を半角に変換
        st_Code = StrConv(st_Code, VbStrConv.Narrow)

        ' 検索ブロックの TextBox の値を書き換え
        Code.Text = st_Code
        Name.Text = st_Name

        ' GET 且つ QueryString("Code") が送信されている場合は検索処理を実行
        If (Request.RequestType = "GET") And (Request.QueryString("Code") <> Nothing) Then
            SetControl_SrcSupplier()
        End If

    End Sub

    ' Search ボタンクリック処理
    Protected Sub Search_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Search.Click

        Dim st_Action As String = IIf(Request.QueryString("Action") = Nothing, "", Request.QueryString("Action"))

        If st_Action = SEARCH_ACTION Then
            SetControl_SrcSupplier()
        End If

    End Sub


    ' SQL データソースコントロールに SELECT 文を設定
    Private Sub SetControl_SrcSupplier()

        ' パラメータチェック
        If Not String.IsNullOrEmpty(st_Code) Then
            If Not Common.IsInteger(st_Code) Then
                st_Code = String.Empty
                SupplierList.DataBind()
                Exit Sub
            End If
        End If

        SrcSupplier.SelectParameters.Clear()

        ' Where 句の生成
        Dim st_where As String = String.Empty
        If Not String.IsNullOrEmpty(st_Code) Then
            SrcSupplier.SelectParameters.Add("Code", Common.SafeSqlLiteral(st_Code))
            st_where = IIf(st_where.Length > 1, st_where & " AND ", "")
            st_where = st_where & " Supplier.SupplierCode = @Code "
        End If
        If Not String.IsNullOrEmpty(st_Name) Then
            SrcSupplier.SelectParameters.Add("Name", Common.SafeSqlLikeClauseLiteral(st_Name))
            st_where = IIf(st_where.Length > 1, st_where & " AND ", "")
            st_where = st_where & " ISNULL(Supplier.Name3,'') + ' ' + ISNULL(Supplier.Name4,'') LIKE N'%' + @Name + '%' "
        End If

        ' Where 句が生成できなかった場合は検索処理を行わずに処理を終了する
        If String.IsNullOrEmpty(st_where) Then
            SupplierList.DataBind()
            Exit Sub
        End If

        SrcSupplier.SelectCommand = _
              " SELECT " _
            & "  SupplierCode, s_Country.[Name] AS CountryName, " _
            & "  LTRIM(RTRIM(ISNULL(Supplier.Name3, '') + ' ' + ISNULL(Supplier.Name4, ''))) AS Name " _
            & " FROM " _
            & "  Supplier " _
            & "  LEFT OUTER JOIN s_Country " _
            & "   ON s_Country.CountryCode = Supplier.CountryCode " _
            & " WHERE " & st_where _
            & " ORDER BY SupplierCode, Name3 "

    End Sub
End Class

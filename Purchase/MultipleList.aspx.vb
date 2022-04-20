Public Class MultipleList
    Inherits CommonPage

    Protected st_ScreenName As String
    Protected st_SearchWord As String
    Protected st_SearchItemId  As String

    Protected ScreenName As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
         ' パラメータ取得
        If Request.RequestType = "POST" And IsPostBack = False Then
            st_ScreenName = IIf(Request.Form("ScreenName") = Nothing, "", Request.Form("ScreenName"))
            st_SearchWord = IIf(Request.Form("SearchWord") = Nothing, "", Request.Form("SearchWord"))
            st_SearchItemId = IIf(Request.Form("SearchItemId") = Nothing, "", Request.Form("SearchItemId"))
        ElseIf Request.RequestType = "GET" Or IsPostBack = True Then
            st_ScreenName = IIf(Request.QueryString("ScreenName") = Nothing, "", Request.QueryString("ScreenName"))
            st_SearchWord = IIf(Request.QueryString("SearchWord") = Nothing, "", Request.QueryString("SearchWord"))
            st_SearchItemId = IIf(Request.QueryString("SearchItemId") = Nothing, "", Request.QueryString("SearchItemId"))
        End If
        ' 空白除去
        st_ScreenName = st_ScreenName.Trim
        st_SearchWord = st_SearchWord.Trim
        st_SearchItemId = st_SearchItemId.Trim

        If String.IsNullOrEmpty(st_ScreenName) Then
            st_ScreenName = String.Empty
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If
        If String.IsNullOrEmpty(st_SearchItemId) Then
            st_SearchItemId = String.Empty
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If
        ScreenName = st_ScreenName

        if Not String.IsNullOrEmpty(st_SearchWord)
            ' 親画面のRFQNumberテキストボックスに入力されている値をカンマ区切りで配列にセット
            Dim ar_SearchWord() As String = Split(st_SearchWord, "|")
            Dim i_Count As Integer = 1

            For Each st_SearchWord As String In ar_SearchWord
                ' 各テキストボックスへパラメータの設定
                Dim st_ItemID = "SearchWord" & i_Count
                Dim txb_SearchWord As System.Web.UI.WebControls.TextBox = FindControl(st_ItemID) 
                txb_SearchWord.Text = st_SearchWord.Trim

                i_Count = i_Count + 1
            Next
        End If
                    
    End Sub

End Class
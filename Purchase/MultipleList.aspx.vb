Public Class MultipleList
    Inherits CommonPage

    Protected st_ScreenName As String
    Protected st_SearchWord As String
    Protected st_SearchItemId  As String

    Protected ScreenName As String
    Protected MaxLength As Integer

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

        ' 入力項目の入力可能最大文字数を設定
        If(st_SearchItemId = "ProductNumber") Then
            MaxLength = 32
        Else
            MaxLength = 10
        End If
        DataBindSearchWord()

        if Not String.IsNullOrEmpty(st_SearchWord)
            ' 親画面のRFQNumberテキストボックスに入力されている値をカンマ区切りで配列にセット
            Dim ar_SearchWord() As String = Split(st_SearchWord, "|")
            Dim i_Count As Integer = 1

            For Each st_SearchWord As String In ar_SearchWord
                ' 各テキストボックスへパラメータの設定
                Dim st_ItemID = "SearchWord" & i_Count
                Dim txb_SearchWord As System.Web.UI.WebControls.TextBox = FindControl(st_ItemID) 
                txb_SearchWord.Text = Left(st_SearchWord.Trim(),MaxLength)

                i_Count = i_Count + 1
            Next
        End If
                    
    End Sub

    ''' <summary>
    ''' MaxLength設定を目的としたDataBindの実行
    ''' </summary>
    ''' <remarks></remarks>
    private sub DataBindSearchWord()
        SearchWord1.DataBind
        SearchWord2.DataBind
        SearchWord3.DataBind
        SearchWord4.DataBind
        SearchWord5.DataBind
        SearchWord6.DataBind
        SearchWord7.DataBind
        SearchWord8.DataBind
        SearchWord9.DataBind
        SearchWord10.DataBind
        SearchWord11.DataBind
        SearchWord12.DataBind
        SearchWord13.DataBind
        SearchWord14.DataBind
        SearchWord15.DataBind
        SearchWord16.DataBind
        SearchWord17.DataBind
        SearchWord18.DataBind
        SearchWord19.DataBind
        SearchWord20.DataBind
        SearchWord21.DataBind
        SearchWord22.DataBind
        SearchWord23.DataBind
        SearchWord24.DataBind
        SearchWord25.DataBind
        SearchWord26.DataBind
        SearchWord27.DataBind
        SearchWord28.DataBind
        SearchWord29.DataBind
        SearchWord30.DataBind
        SearchWord31.DataBind
        SearchWord32.DataBind
        SearchWord33.DataBind
        SearchWord34.DataBind
        SearchWord35.DataBind
        SearchWord36.DataBind
        SearchWord37.DataBind
        SearchWord38.DataBind
        SearchWord39.DataBind
        SearchWord40.DataBind
        SearchWord41.DataBind
        SearchWord42.DataBind
        SearchWord43.DataBind
        SearchWord44.DataBind
        SearchWord45.DataBind
        SearchWord46.DataBind
        SearchWord47.DataBind
        SearchWord48.DataBind
        SearchWord49.DataBind
        SearchWord50.DataBind
        SearchWord51.DataBind
        SearchWord52.DataBind
        SearchWord53.DataBind
        SearchWord54.DataBind
        SearchWord55.DataBind
        SearchWord56.DataBind
        SearchWord57.DataBind
        SearchWord58.DataBind
        SearchWord59.DataBind
        SearchWord60.DataBind
        SearchWord61.DataBind
        SearchWord62.DataBind
        SearchWord63.DataBind
        SearchWord64.DataBind
        SearchWord65.DataBind
        SearchWord66.DataBind
        SearchWord67.DataBind
        SearchWord68.DataBind
        SearchWord69.DataBind
        SearchWord70.DataBind
        SearchWord71.DataBind
        SearchWord72.DataBind
        SearchWord73.DataBind
        SearchWord74.DataBind
        SearchWord75.DataBind
        SearchWord76.DataBind
        SearchWord77.DataBind
        SearchWord78.DataBind
        SearchWord79.DataBind
        SearchWord80.DataBind
        SearchWord81.DataBind
        SearchWord82.DataBind
        SearchWord83.DataBind
        SearchWord84.DataBind
        SearchWord85.DataBind
        SearchWord86.DataBind
        SearchWord87.DataBind
        SearchWord88.DataBind
        SearchWord89.DataBind
        SearchWord90.DataBind
        SearchWord91.DataBind
        SearchWord92.DataBind
        SearchWord93.DataBind
        SearchWord94.DataBind
        SearchWord95.DataBind
        SearchWord96.DataBind
        SearchWord97.DataBind
        SearchWord98.DataBind
        SearchWord99.DataBind
        SearchWord100.DataBind
    End sub

End Class
Imports Purchase.Common

Public Class CCReminder
    Inherits CommonPage

    Private Const PAGE_SIZE As Integer = 50

    ''' <summary>
    ''' ページ読み込み時の処理を行います
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If IsPostBack = False Then

            ShowList(CInt(Session("UserID")))

        End If

    End Sub

    ''' <summary>
    ''' ページ番号押下時の処理を行います
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub RFQList_PagePropertiesChanged(sender As Object, e As EventArgs) Handles RFQList.PagePropertiesChanged

        RFQList.Visible = False

        If IsPostBack Then

            ShowList(CInt(Session("UserID")))

        End If

        RFQList.Visible = True

    End Sub

    ''' <summary>
    ''' 一覧を表示します
    ''' </summary>
    ''' <param name="UserID">ユーザ ID</param>
    Private Sub ShowList(ByVal UserID As Integer)

        RFQPagerCountTop.PageSize = PAGE_SIZE
        RFQPagerLinkTop.PageSize = PAGE_SIZE
        RFQPagerLinkBottom.PageSize = PAGE_SIZE
        RFQPagerCountBottom.PageSize = PAGE_SIZE

        Dim ccList As New TCIDataAccess.Join.CCReminderDispList
        ccList.Load(UserID)
        RFQList.DataSource = ccList
        RFQList.DataBind()

    End Sub

End Class
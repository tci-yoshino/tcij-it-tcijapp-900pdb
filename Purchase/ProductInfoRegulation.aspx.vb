Option Explicit On
Option Strict On
Option Infer Off
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports Purchase.Common

Partial Public Class ProductInfoRegulation
    Inherits CommonPage

    Private branchPrevious As String = String.Empty

    Private ehsHeaderData As TCIDataAccess.Join.ProductInfoRegulationList = New TCIDataAccess.Join.ProductInfoRegulationList

    Private separatorFirst As Boolean = True

    ''' <summary>
    ''' 初期表示処理
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs)
        Dim action As String = String.Empty
        Dim authFlg As Boolean = False
        lblMsg.Text = String.Empty
        If Not IsPostBack Then
            action = "Save"
            'EHSヘッダーデータ表示
            Me.ShowEhsHeader()
        Else
            action = "Save"
        End If

    End Sub

    ''' <summary>
    ''' EHSヘッダーデータ表示
    ''' </summary>
    Private Sub ShowEhsHeader()
        'EHSヘッダーデータ取得
        ehsHeaderData = New TCIDataAccess.Join.ProductInfoRegulationList
        ehsHeaderData.Load_CreateEhsHeaderPersonalizeListSelectSQL(Integer.Parse(Session("UserID").ToString), Session("LocationCode").ToString)
        'ListViewにバインド
        ltvEhsHeader.DataSource = Me.ehsHeaderData
        ltvEhsHeader.DataBind()
        'Dim dc_EHTemp As TCIDataAccess.s_EhsHeaderList = New TCIDataAccess.s_EhsHeaderList
        'Dim maxUpdateDateDel As DateTime = dc_EHTemp.GetLatestUpdateDate()
        'hidLastUpdateDate.Value = maxUpdateDateDel.ToString("yyyyMMddHHmmssfff")
    End Sub

    ''' <summary>
    ''' 一覧の行ごとの設定
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub ltvEhsHeader_ItemDataBound(ByVal sender As Object, ByVal e As ListViewItemEventArgs)
        'Separator
        Dim trSeparator As HtmlControl = CType(e.Item.FindControl("trRowSeparator"), HtmlControl)
        'lblLocationName
        Dim locationName As Label = CType(e.Item.FindControl("lblLocationName"), Label)
        'hidLocationCode
        Dim locationCode As HiddenField = CType(e.Item.FindControl("hidLocationCode"), HiddenField)
        'chkOnOrOff
        Dim onOffChk As CheckBox = CType(e.Item.FindControl("chkOnOrOff"), CheckBox)
        Dim hidOnOffChk As HiddenField = CType(e.Item.FindControl("hidOnOrOff"), HiddenField)
        'tdBranch
        Dim td As HtmlTableCell = CType(e.Item.FindControl("tdBranch"), HtmlTableCell)
        'ShowEhsHeaderで取得したEhsHeaderの現在行データ
        Dim currentEhsHeaderRow As TCIDataAccess.Join.ProductInfoRegulationDisp = CType(CType(e.Item, ListViewDataItem).DataItem, TCIDataAccess.Join.ProductInfoRegulationDisp)
        'Branchの設定
        If (locationCode.Value = Me.branchPrevious) Then
            locationName.Visible = False
            trSeparator.Visible = False
        Else
            Me.branchPrevious = locationCode.Value
            locationName.Visible = True
            If (Me.separatorFirst = True) Then
                trSeparator.Visible = False
                Me.separatorFirst = False
            Else
                trSeparator.Visible = True
            End If

        End If

        ' On/Offチェックボックスの設定
        If Not IsExist() Then
            '一度も保存していない(EhsHeader_Personalizeにデータがない)場合
            'Globalと自拠点のみチェックする
            'If ((currentEhsHeaderRow.LocationCode = Common.LOCATION_CODE_GL)
            If ((currentEhsHeaderRow.LocationCode = Common.LOCATION_CODE_GL) _
                        Or (currentEhsHeaderRow.LocationCode = Session("LocationCode").ToString)) Then
                onOffChk.Checked = True
                hidOnOffChk.Value = "True"
            Else
                onOffChk.Checked = False
                hidOnOffChk.Value = "False"
            End If

        Else
            '既に保存している(EhsHeader_Personalizeにデータがある)場合
            Dim list As New List(Of String)
            If ehsHeaderData.Exists(Function(n) n.UserID = CType(Session("UserID"), Integer) _
                                        AndAlso (n.Item = currentEhsHeaderRow.Item)) Then
                onOffChk.Checked = True
                hidOnOffChk.Value = "True"
            Else
                hidOnOffChk.Value = "False"
            End If

        End If

    End Sub

    '<summary>
    '保存ボタン押下時の処理
    '</summary>
    '<param name="sender"></param>
    '<param name="e"></param>
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As EventArgs)

        If Not Me.CheckChangesOnOrOff(ltvEhsHeader) Then
            '画面に変更が無い場合は処理を終了(メッセージは表示しない)
            Return
        End If

        Dim registerList As TCIDataAccess.EhsHeader_PersonalizeList = New TCIDataAccess.EhsHeader_PersonalizeList
        Dim deleteList As TCIDataAccess.EhsHeader_PersonalizeList = New TCIDataAccess.EhsHeader_PersonalizeList
        Dim updateList As TCIDataAccess.s_EhsHeaderList = New TCIDataAccess.s_EhsHeaderList
        For Each item As ListViewItem In ltvEhsHeader.Items
            Dim ehsHeaderPersonalize As TCIDataAccess.EhsHeader_Personalize = New TCIDataAccess.EhsHeader_Personalize
            Dim ehsHeader As TCIDataAccess.s_EhsHeader = New TCIDataAccess.s_EhsHeader
            Dim onOff As CheckBox = CType(item.FindControl("chkOnOrOff"), CheckBox)
            Dim text As Label = CType(item.FindControl("lblText"), Label)
            Dim itam As HiddenField = CType(item.FindControl("hidItem"), HiddenField)

            'EhsHeader_Personalizeテーブルの登録List・削除List作成
            If onOff.Checked Then
                '登録List作成
                ehsHeaderPersonalize.UserID = Integer.Parse(Session("UserID").ToString)
                ehsHeaderPersonalize.Item = itam.Value
                ehsHeaderPersonalize.CreatedBy = Integer.Parse(Session("UserID").ToString)
                ehsHeaderPersonalize.UpdatedBy = Integer.Parse(Session("UserID").ToString)
                registerList.Add(ehsHeaderPersonalize)
            Else
                '削除List作成
                ehsHeaderPersonalize.UserID = Integer.Parse(Session("UserID").ToString)
                ehsHeaderPersonalize.Item = itam.Value
                deleteList.Add(ehsHeaderPersonalize)
            End If

        Next
        'INS,UPD,DELの実行
        TCIDataAccess.FacadeEhsHeader.Save(registerList, deleteList, updateList)

        '初期状態ならばすべて削除する
        Dim checkEhsHeaderData As TCIDataAccess.Join.ProductInfoRegulationList = New TCIDataAccess.Join.ProductInfoRegulationList
        checkEhsHeaderData.Load_CreateEhsHeaderPersonalizeListSelectSQL(Integer.Parse(Session("UserID").ToString), (Session("LocationCode").ToString))
        Dim allDelete As Boolean = True
        Dim i As Integer = 0
        Do While (i < checkEhsHeaderData.Count)
            ' Globalまたは自分のロケーションの項目でチェックされていない項目がある時は削除しない
            'If ((checkEhsHeaderData(i).LocationCode = Common.LOCATION_CODE_GL)
            If ((checkEhsHeaderData(i).LocationCode = "GL") _
                        OrElse (checkEhsHeaderData(i).LocationCode = (Session("LocationCode").ToString))) Then
                If (checkEhsHeaderData(i).UserID <> Integer.Parse(Session("UserID").ToString)) Then
                    allDelete = False
                End If

            Else
                ' Globalまたは自分のロケーションと等しくない項目がチェックされた時は削除しない
                If (checkEhsHeaderData(i).UserID = Integer.Parse(Session("UserID").ToString)) Then
                    allDelete = False
                End If

            End If

            i = (i + 1)
        Loop

        ' 初期状態であるとき削除の実行
        If (allDelete = True) Then
            Dim allDeleteList As TCIDataAccess.EhsHeader_PersonalizeList = New TCIDataAccess.EhsHeader_PersonalizeList

            Dim a As Integer = 0
            Do While (a < checkEhsHeaderData.Count)
                If (checkEhsHeaderData(a).UserID = Integer.Parse(Session("UserID").ToString)) Then
                    Dim ehsHeaderPersonalize As TCIDataAccess.EhsHeader_Personalize = New TCIDataAccess.EhsHeader_Personalize
                    '削除List作成
                    ehsHeaderPersonalize.UserID = Integer.Parse(Session("UserID").ToString)
                    ehsHeaderPersonalize.Item = checkEhsHeaderData(a).Item
                    allDeleteList.Add(ehsHeaderPersonalize)
                End If

                a = (a + 1)
            Loop

            ' DELの実行
            TCIDataAccess.FacadeEhsHeader.Save(New TCIDataAccess.EhsHeader_PersonalizeList, allDeleteList, New TCIDataAccess.s_EhsHeaderList)
        End If

        '完了メッセージ
        lblMsg.Text = Common.MSG_DATA_SAVED
        'EhsHeader一覧表示処理
        Me.ShowEhsHeader()
    End Sub

    '<summary>
    '取得したEhsヘッダデータに自身のデータが存在するかチェック
    '</summary>
    '<returns>true:データあり/false:データなし</returns>
    Private Function IsExist() As Boolean

        Return ehsHeaderData.Exists(Function(n) n.UserID = CType(Session("UserID"), Integer))

    End Function

    '<summary>
    '画面の変更があるかチェックする
    '</summary>
    '<param name="ltv"></param>
    '<returns></returns>
    Private Function CheckChangesOnOrOff(ByVal ltv As ListView) As Boolean
        For Each item As ListViewItem In ltv.Items
            Dim chkOnOrOff As CheckBox = CType(item.FindControl("chkOnOrOff"), CheckBox)
            Dim hidOnOrOff As HiddenField = CType(item.FindControl("hidOnOrOff"), HiddenField)
            Dim chkOutputTransferOrder As CheckBox = CType(item.FindControl("chkOutputTransferOrder"), CheckBox)
            Dim hidOutputTransferOrder As HiddenField = CType(item.FindControl("hidOutputTransferOrder"), HiddenField)
            Dim chkOutputPriceRevision As CheckBox = CType(item.FindControl("chkOutputPriceRevision"), CheckBox)
            Dim hidOutputPriceRevision As HiddenField = CType(item.FindControl("hidOutputPriceRevision"), HiddenField)
            If (chkOnOrOff.Checked.ToString <> hidOnOrOff.Value) Then
                Return True
            End If

        Next
        Return False
    End Function

End Class
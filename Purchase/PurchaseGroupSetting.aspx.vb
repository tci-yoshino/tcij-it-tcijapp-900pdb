Option Explicit On
Option Infer Off
Option Strict On

Imports System.Data.SqlClient
Imports Purchase.Common

Partial Public Class PurchaseGroupSetting
    Inherits CommonPage

    Const SAVE_ACTION As String = "Save"

    ''' <summary>
    ''' ページロード処理を行います
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Msg.Text = String.Empty

        If IsPostBack = False Then
            UserID.Value = Common.GetHttpQuery(Request, "UserID")

            'パラメータが整数でない場合はエラー
            If Common.IsInteger(UserID.Value) = False Then
                Msg.Text = Common.ERR_INVALID_PARAMETER
                Exit Sub
            End If

            'ログオンユーザの所属拠点と対象ユーザの所属拠点が異なる場合はエラー
            If TCIDataAccess.Join.PurchasingUserDisp.IsActive(Session("LocationCode").ToString, CInt(UserID.Value)) = False Then
                Msg.Text = Common.ERR_INVALID_PARAMETER
                Exit Sub
            End If

            Dim uid As Integer = CInt(UserID.Value)

            Dim purchasingUser As New TCIDataAccess.Join.PurchasingUserDisp
            purchasingUser.Load(uid)

            LocationCode.Value = purchasingUser.LocationCode
            LocationName.Text = purchasingUser.LocationName
            UserName.Text = purchasingUser.UserName
            RoleName.Text = purchasingUser.RoleName
            R3PurchasingGroup.Text = purchasingUser.R3PurchasingGroup
            RFQCorrespondenceEditable.Checked = purchasingUser.RFQCorrespondenceEditable
            MMSTAInvalidationEditable.Checked = purchasingUser.MMSTAInvalidationEditable

            Dim dc_StorageByPurchasingUserDispList As New TCIDataAccess.Join.StorageByPurchasingUserDispList
            dc_StorageByPurchasingUserDispList.Load(uid)

            For Each dc_StorageByPurchasingUserDisp As TCIDataAccess.Join.StorageByPurchasingUserDisp In dc_StorageByPurchasingUserDispList
                Dim item As New ListItem
                item.Text = dc_StorageByPurchasingUserDisp.Storage
                item.Value = dc_StorageByPurchasingUserDisp.Storage
                item.Selected = dc_StorageByPurchasingUserDisp.IsChecked
                StorageLocationCheckBoxList.Items.Add(item)
            Next

            Dim dc_UserDispList As New TCIDataAccess.Join.PurchasingUserDispList
            dc_UserDispList.LoadEditUsers(LocationCode.Value)

            Dim cc1 As Boolean = False
            Dim cc2 As Boolean = False

            DefaultCCUser1.Items.Add(New ListItem())
            DefaultCCUser2.Items.Add(New ListItem())
            For Each dc_UserDisp As TCIDataAccess.Join.PurchasingUserDisp In dc_UserDispList

                If (dc_UserDisp.UserID = purchasingUser.DefaultCCUserID1) Then
                    cc1 = True
                End If

                If (dc_UserDisp.UserID = purchasingUser.DefaultCCUserID2) Then
                    cc2 = True
                End If

                DefaultCCUser1.Items.Add(New ListItem(dc_UserDisp.UserName, dc_UserDisp.UserID.ToString))
                DefaultCCUser2.Items.Add(New ListItem(dc_UserDisp.UserName, dc_UserDisp.UserID.ToString))
            Next
            If cc1 Then DefaultCCUser1.SelectedValue = purchasingUser.DefaultCCUserID1.ToString
            If cc2 Then DefaultCCUser2.SelectedValue = purchasingUser.DefaultCCUserID2.ToString

        End If

    End Sub

    ''' <summary>
    ''' Save ボタンクリック時の処理を行います
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click

        If Common.GetHttpAction(Request) <> SAVE_ACTION Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        Dim uid As Integer = CInt(UserID.Value)

        Dim dc_PurchasingUser As New TCIDataAccess.PurchasingUser
        dc_PurchasingUser.Load(uid)
        dc_PurchasingUser.R3PurchasingGroup = R3PurchasingGroup.Text
        dc_PurchasingUser.RFQCorrespondenceEditable = RFQCorrespondenceEditable.Checked
        dc_PurchasingUser.MMSTAInvalidationEditable = MMSTAInvalidationEditable.Checked

        If (DefaultCCUser1.SelectedIndex = 0) Then
            dc_PurchasingUser.DefaultCCUserID1 = Nothing
        Else
            dc_PurchasingUser.DefaultCCUserID1 = CInt(DefaultCCUser1.SelectedValue)
        End If

        If (DefaultCCUser2.SelectedIndex = 0) Then
            dc_PurchasingUser.DefaultCCUserID2 = Nothing
        Else
            dc_PurchasingUser.DefaultCCUserID2 = CInt(DefaultCCUser2.SelectedValue)
        End If

        Dim dc_StorageByPurchasingUserList As New TCIDataAccess.StorageByPurchasingUserList
        For Each item As ListItem In StorageLocationCheckBoxList.Items
            If item.Selected Then
                Dim dc_StorageByPurchasingUser As New TCIDataAccess.StorageByPurchasingUser
                dc_StorageByPurchasingUser.Storage = item.Value
                dc_StorageByPurchasingUserList.Add(dc_StorageByPurchasingUser)
            End If

        Next

        TCIDataAccess.FacadePurchaseGroupSetting.Save(uid, dc_PurchasingUser, dc_StorageByPurchasingUserList)

        Response.Redirect("PurchaseGroup.aspx")

    End Sub

End Class
Option Explicit On
Option Infer Off
Option Strict On

Imports System.Data.SqlClient
Imports Purchase.Common
Partial Public Class PurchaseGroupSetting
    Inherits CommonPage

    Const SAVE_ACTION As String = "Save"
    Const EDIT_ACTION As String = "Edit"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Msg.Text = String.Empty

        If IsPostBack = False Then
            Mode.Value = Common.GetHttpAction(Request)
            UserID.Value = Common.GetHttpQuery(Request, "UserID")
            If Common.IsInteger(UserID.Value) = False Or UserID.Value.Length = 0 Then
                Msg.Text = Common.ERR_INVALID_PARAMETER
                Exit Sub
            End If

            Dim dc_PurchaseGroupSettingDispList As TCIDataAccess.Join.PurchaseGroupSettingDispList = New TCIDataAccess.Join.PurchaseGroupSettingDispList
            dc_PurchaseGroupSettingDispList.Load(Cint(UserID.Value))

            If dc_PurchaseGroupSettingDispList.Count <> 0  Then
                R3PurchasingGroup.Text = dc_PurchaseGroupSettingDispList(0).R3PurchasingGroup
                Me.Location.Text = dc_PurchaseGroupSettingDispList(0).LocationName
                Name.Text = dc_PurchaseGroupSettingDispList(0).Name

                For Each storagePUser As TCIDataAccess.StorageByPurchasingUser In dc_PurchaseGroupSettingDispList(0).StorageByPurchasingUserList
                    If storagePUser.Storage = "AL10" Then
                        AL10.Checked = True
                    End If
                    If storagePUser.Storage = "AL11" Then
                        AL11.Checked = True
                    End If
                    If storagePUser.Storage = "AL20" Then
                        AL20.Checked = True
                    End If
                    If storagePUser.Storage = "AL40" Then
                        AL40.Checked = True
                    End If
                    If storagePUser.Storage = "AL50" Then
                        AL50.Checked = True
                    End If
                    If storagePUser.Storage = "CL10" Then
                        CL10.Checked = True
                    End If
                    'If storagePUser.Storage = "CL20" Then
                    '    CL20.Checked = True
                    'End If
                    'If storagePUser.Storage = "CL30" Then
                    '    CL30.Checked = True
                    'End If
                    If storagePUser.Storage = "CL40" Then
                        CL40.Checked = True
                    End If
                    If storagePUser.Storage = "CL70" Then
                        CL70.Checked = True
                    End If
                    If storagePUser.Storage = "EL10" Then
                        EL10.Checked = True
                    End If
                    If storagePUser.Storage = "EL20" Then
                        EL20.Checked = True
                    End If
                    If storagePUser.Storage = "HL10" Then
                        HL10.Checked = True
                    End If
                    If storagePUser.Storage = "HL30" Then
                        HL30.Checked = True
                    End If
                    If storagePUser.Storage = "HL50" Then
                        HL50.Checked = True
                    End If
                    If storagePUser.Storage = "NL10" Then
                        NL10.Checked = True
                    End If
                    If storagePUser.Storage = "NL20" Then
                        NL20.Checked = True
                    End If
                Next
            Else
                Msg.Text = Common.MSG_NO_DATA_FOUND
                Exit Sub
            End If

            If dc_PurchaseGroupSettingDispList(0).RFQCorrespondenceEditable = "Y" Then
                RFQCorrespondenceEditable.Checked = True
            End If

            If dc_PurchaseGroupSettingDispList(0).MMSTAInvalidationEditable = "Y" Then
                MMSTAInvalidationEditable.Checked = True
            End If

        End If

    End Sub
    Protected Sub Save_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Save.Click
        If Common.GetHttpAction(Request) <> SAVE_ACTION Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If
        If UserID.Value.Length = 0 Then
            Msg.Text = "User ID " & Common.ERR_REQUIRED_FIELD
            Exit Sub
        End If
        If Common.IsInteger(UserID.Value) = False Then
            Msg.Text = "User ID" & Common.ERR_INVALID_NUMBER
            Exit Sub
        End If
        If Common.ExistenceConfirmation("s_User", "UserID", UserID.Value) = False Then
            Msg.Text = "User ID" & Common.ERR_DOES_NOT_EXIST
            Exit Sub
        End If

        If Common.GetHttpQuery(Request, "Mode") = EDIT_ACTION Then
            If Common.ExistenceConfirmation("PurchasingUser", "UserID", UserID.Value) = False Then    '[入力UserIDのPurchasingUser存在有無]
                Msg.Text = Common.ERR_DELETED_BY_ANOTHER_USER
                Exit Sub
            End If

            Dim st_R3PurchasingGroup As String = String.Empty
            st_R3PurchasingGroup = R3PurchasingGroup.Text

            Dim bl_RFQCorrespondenceEditable As Boolean = False
            If RFQCorrespondenceEditable.Checked = True Then
                bl_RFQCorrespondenceEditable = True
            End If

            Dim bl_MMSTAInvalidationEditable As Boolean = False
            If MMSTAInvalidationEditable.Checked = True Then
                bl_MMSTAInvalidationEditable = True
            End If

            Dim StorageList As List(Of String) = New List(Of String)
            If AL10.Checked.Equals(True) Then
                StorageList.Add(AL10.ID.ToString)
            End If
            If AL11.Checked = True Then
                StorageList.Add(AL11.ID.ToString)
            End If
            If AL20.Checked = True Then
                StorageList.Add(AL20.ID.ToString)
            End If
            If AL40.Checked = True Then
                StorageList.Add(AL40.ID.ToString)
            End If
            If AL50.Checked = True Then
                StorageList.Add(AL50.ID.ToString)
            End If
            If CL10.Checked = True Then
                StorageList.Add(CL10.ID.ToString)
            End If
            'If CL20.Checked = True Then
                'StorageList.Add(CL20.ID.ToString)
            'End If
            'If CL30.Checked = True Then
                'StorageList.Add(CL30.ID.ToString)
            'End If
            If CL40.Checked = True Then
                StorageList.Add(CL40.ID.ToString)
            End If
            If CL70.Checked = True Then
                StorageList.Add(CL70.ID.ToString)
            End If
            If EL10.Checked = True Then
                StorageList.Add(EL10.ID.ToString)
            End If
            If EL20.Checked = True Then
                StorageList.Add(EL20.ID.ToString)
            End If
            If HL10.Checked = True Then
                StorageList.Add(HL10.ID.ToString)
            End If
            If HL30.Checked = True Then
                StorageList.Add(HL30.ID.ToString)
            End If
            If HL50.Checked = True Then
                StorageList.Add(HL50.ID.ToString)
            End If
            If NL10.Checked = True Then
                StorageList.Add(NL10.ID.ToString)
            End If
            If NL20.Checked = True Then
                StorageList.Add(NL20.ID.ToString)
            End If

            '' 
            TCIDataAccess.FacadePurchaseGroupSetting.Save(CInt(UserID.Value), CInt(Session("UserID")), st_R3PurchasingGroup,
                                                          bl_RFQCorrespondenceEditable, bl_MMSTAInvalidationEditable, StorageList)
        Else
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        Response.Redirect("PurchaseGroup.aspx")
    End Sub
End Class
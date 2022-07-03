Option Explicit On
Option Strict On
Option Infer Off

Imports System.Data.SqlClient

Namespace TCIDataAccess

    ''' <summary> 
    ''' FacadePurchaseGroupSetting データクラス 
    ''' </summary> 
    Public Class FacadePurchaseGroupSetting

        ''' <summary>
        ''' インスタンス生成防止用コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

        '''' <summary>
        '''' PurchasingUser を更新する
        '''' </summary>
        'Public Shared Sub Save(ByVal i_UserID As Integer, ByVal i_UpdatedBy As Integer, ByVal st_R3PurchasingGroup As String,
        '                       ByVal bi_RFQCorrespondenceEditable As Boolean, ByVal bi_MMSTAInvalidationEditable As Boolean, ByVal lst_StorageList As List(Of String))
        '    Dim sb_SQL As StringBuilder = New StringBuilder
        '    Dim DBConn As SqlConnection = New SqlConnection(DBCommon.DB_CONNECT_STRING)
        '    DBConn.Open()
        '    Dim DBTran As SqlTransaction = DBConn.BeginTransaction
        '    Try
        '        Dim DBCommand As SqlCommand = DBConn.CreateCommand
        '        DBCommand.Transaction = DBTran
        '        ' PurchasingUser DBを更新する
        '        Dim dc_PurchasingUser As PurchasingUser = New PurchasingUser
        '        dc_PurchasingUser.Save(DBCommand, i_UserID, i_UpdatedBy, st_R3PurchasingGroup, bi_RFQCorrespondenceEditable, bi_MMSTAInvalidationEditable)

        '        ' StorageByPurchasingUser DBを削除する
        '        Dim dc_StorageByPurchasingUser As StorageByPurchasingUser = New StorageByPurchasingUser
        '        dc_StorageByPurchasingUser.Delete(DBCommand, i_UserID)

        '        ' StorageByPurchasingUser DBを登録する
        '        dc_StorageByPurchasingUser.UserID = i_UserID
        '        For Each st_Storage As String In lst_StorageList
        '            dc_StorageByPurchasingUser.Storage = st_Storage
        '            dc_StorageByPurchasingUser.Entry(DBCommand)
        '        Next

        '        DBTran.Commit()
        '    Catch ex As Exception
        '        DBTran.Rollback()
        '        Throw
        '    Finally
        '        If (Not (DBTran) Is Nothing) Then
        '            DBTran.Dispose()
        '        End If

        '    End Try

        'End Sub

        ''' <summary>
        ''' PurchasingUser および StorageByPurchasingUser を登録する
        ''' </summary>
        ''' <param name="UserID"></param>
        ''' <param name="dc_PurchasingUser"></param>
        ''' <param name="dc_StorageByPurchasingUserList"></param>
        Public Shared Sub Save(ByVal UserID As Integer, ByVal dc_PurchasingUser As PurchasingUser, ByVal dc_StorageByPurchasingUserList As StorageByPurchasingUserList)

            Dim dc_StorageByPurchasingUser As StorageByPurchasingUser

            Using DBConn As New SqlConnection(DBCommon.DB_CONNECT_STRING)
                DBConn.Open()

                Using DBTran As SqlTransaction = DBConn.BeginTransaction
                    Using DBCommand As SqlCommand = DBConn.CreateCommand
                        DBCommand.Transaction = DBTran

                        'PurchasingUser
                        dc_PurchasingUser.UserID = UserID
                        dc_PurchasingUser.CreateDate = DateTime.Now
                        dc_PurchasingUser.UpdateDate = DateTime.Now
                        dc_PurchasingUser.Save(DBCommand)

                        'StorageByPurchasingUser
                        dc_StorageByPurchasingUser = New StorageByPurchasingUser
                        dc_StorageByPurchasingUser.Delete(DBCommand, UserID)

                        For Each dc_StorageByPurchasingUser In dc_StorageByPurchasingUserList
                            dc_StorageByPurchasingUser.UserID = UserID
                            dc_StorageByPurchasingUser.Save(DBCommand)
                        Next

                    End Using

                    DBTran.Commit()

                End Using
            End Using

        End Sub

    End Class

End Namespace


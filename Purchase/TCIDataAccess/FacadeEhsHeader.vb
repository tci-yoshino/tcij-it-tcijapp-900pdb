Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Text

Namespace TCIDataAccess

    Public Class FacadeEhsHeader
        ''' <summary>
        ''' 画面上で変更された全件を一括でDBに更新する
        ''' </summary>
        ''' <param name="registerList"></param>
        ''' <param name="deleteList"></param>
        ''' <param name="updateList"></param>
        Public Shared Sub Save(ByVal registerList As EhsHeader_PersonalizeList, ByVal deleteList As EhsHeader_PersonalizeList, ByVal updateList As s_EhsHeaderList)
            Dim sb_SQL As StringBuilder = New StringBuilder
            Using DBConn As SqlConnection = New SqlConnection(DBCommon.DB_CONNECT_STRING)
                DBConn.Open()
                Using DBTran As SqlTransaction = DBConn.BeginTransaction
                    Try
                        Using DBCommand As SqlCommand = DBConn.CreateCommand
                            DBCommand.Transaction = DBTran
                            'EhsHeader_PersonalizeをDBに登録する(ON/OFFチェックボックスでチェックしたデータ)
                            For Each item As EhsHeader_Personalize In registerList
                                item.Save(DBCommand)
                            Next
                            'EhsHeader_PersonalizeをDBに削除する(ON/OFFチェックボックスでチェックを外したデータ)
                            For Each item As EhsHeader_Personalize In deleteList
                                item.Delete(DBCommand)
                            Next

                            DBTran.Commit()
                        End Using
                    Catch ex As Exception
                        DBTran.Rollback()
                        Throw
                    Finally
                        If (Not (DBTran) Is Nothing) Then
                            DBTran.Dispose()
                        End If
                    End Try
                End Using
            End Using
        End Sub
    End Class
End Namespace
Imports System.Data.SqlClient
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess.Join

    ''' <summary>
    ''' StorageByPurchasingUserDisp データクラス
    ''' </summary>
    Public Class StorageByPurchasingUserDisp
        Inherits TCIDataAccess.StorageByPurchasingUser

        Protected _IsChecked As Boolean = False

        ''' <summary>
        ''' IsChecked を設定, または取得します
        ''' </summary>
        ''' <returns></returns>
        Public Property IsChecked() As Boolean
            Get
                Return _IsChecked
            End Get
            Set(ByVal value As Boolean)
                _IsChecked = value
            End Set
        End Property

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

    End Class

    ''' <summary>
    ''' StorageByPurchasingUserDisp データリストクラス
    ''' </summary>
    Public Class StorageByPurchasingUserDispList
        Inherits List(Of StorageByPurchasingUserDisp)

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' データを読み込みます
        ''' </summary>
        ''' <param name="UserID">ユーザ ID</param>
        Public Sub Load(ByVal UserID As Integer)

            Dim Value As New StringBuilder()
            Value.AppendLine("SELECT")
            Value.AppendLine("    SL.[Storage],")
            Value.AppendLine("    CASE WHEN SPU.[UserID] IS NULL THEN 0 ELSE 1 END AS [IsChecked]")
            Value.AppendLine("FROM")
            Value.AppendLine("    [StorageLocation] AS SL")
            Value.AppendLine("        LEFT OUTER JOIN [StorageByPurchasingUser] AS SPU ON SPU.[UserID] = @UserID AND SPU.[Storage] = SL.[Storage]")
            Value.AppendLine("ORDER BY")
            Value.AppendLine("    SL.[Storage]")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                DBConn.Open()

                Using DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = Value.ToString()
                    DBCommand.Parameters.AddWithValue("UserID", UserID)

                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader()
                        While DBReader.Read()
                            Dim dc_StorageByPurchasingUserDisp As New StorageByPurchasingUserDisp()
                            SetProperty(UserID, dc_StorageByPurchasingUserDisp.UserID)
                            SetProperty(DBReader("Storage"), dc_StorageByPurchasingUserDisp.Storage)
                            SetProperty(DBReader("IsChecked"), dc_StorageByPurchasingUserDisp.IsChecked)

                            Me.Add(dc_StorageByPurchasingUserDisp)
                        End While
                    End Using
                End Using
            End Using

        End Sub

    End Class

End Namespace

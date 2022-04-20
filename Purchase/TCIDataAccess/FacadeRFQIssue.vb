Imports System.Data.SqlClient

Namespace TCIDataAccess

    ''' <summary> 
    ''' FacadeRFQIssue データクラス 
    ''' </summary> 
    Public Class FacadeRFQIssue 

        protected _RFQHeader As RFQHeader = New RFQHeader
        protected _RFQLine As RFQLineList = New RFQLineList

        ''' <summary> 
        ''' RFQNumber を設定、または取得する 
        ''' </summary> 
        Public Property RFQHeader() As RFQHeader
            Get
                Return _RFQHeader
            End Get
            Set(ByVal value As RFQHeader)
                _RFQHeader = value
            End Set
        End Property

        ''' <summary> 
        ''' RFQNumber を設定、または取得する 
        ''' </summary> 
        Public Property RFQLineList() As RFQLineList
            Get
                Return _RFQLine
            End Get
            Set(ByVal value As RFQLineList)
                _RFQLine = value
            End Set
        End Property
        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

        Public Function Save() As Integer
            Dim i_RFQNumber As Integer = 0

            Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                ' トランザクション開始※Using内はCommitのみでOK RollBackは不要
                DBConn.Open 
                using sqlTran As SqlTransaction  = DBConn.BeginTransaction()
                    Using DBCommand As SqlCommand = DBConn.CreateCommand()
                        ' RFQHeaderの追加
                        Dim insRFQHeader As RFQHeader = New RFQHeader
                        insRFQHeader = Me.RFQHeader
                        ' 追加と同時に自動採番されたRFQNumberを取得する。
                        i_RFQNumber = insRFQHeader.Save()

                        ' RFQLineの追加
                        For Each rFQLine As RFQLine In Me.RFQLineList
                            Dim insRFQLine As RFQLine = New RFQLine
                            insRFQLine = rFQLine
                            ' RFQNumberを追加でセットする
                            insRFQLine.RFQNumber = i_RFQNumber
                            insRFQLine.Save()
                        Next

                        ' コミット
                        sqlTran.Commit()

                    End Using
                End Using
            End Using

            Return i_RFQNumber
        End Function

    End Class

End Namespace

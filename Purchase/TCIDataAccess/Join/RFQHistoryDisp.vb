Imports System.Data.SqlClient
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess.Join
    Public Class RFQHistoryDisp
        Inherits TCIDataAccess.RFQHistory

        Protected _RFQStatus As String = String.Empty
        Protected _RFQCorres As String = String.Empty
        Protected _SendLocationName As String = String.Empty
        Protected _SendUserName As String = String.Empty
        Protected _RcptLocationName As String = String.Empty
        Protected _RcptUserName As String = String.Empty
        Protected _AddrLocationName As String = String.Empty
        Protected _AddrUserName As String = String.Empty
        Protected _CCLocationName1 As String = String.Empty
        Protected _CCUserName1 As String = String.Empty
        Protected _CCLocationName2 As String = String.Empty
        Protected _CCUserName2 As String = String.Empty


        Public Property RFQStatus As String
            Get
                Return _RFQStatus
            End Get
            Set(ByVal value As String)
                _RFQStatus = value
            End Set
        End Property


        Public Property RFQCorres As String
            Get
                Return _RFQCorres
            End Get
            Set(ByVal value As String)
                _RFQCorres = value
            End Set
        End Property


        Public Property SendLocationName As String
            Get
                Return _SendLocationName
            End Get
            Set(ByVal value As String)
                _SendLocationName = value
            End Set
        End Property


        Public Property SendUserName As String
            Get
                Return _SendUserName
            End Get
            Set(ByVal value As String)
                _SendUserName = value
            End Set
        End Property


        Public Property RcptLocationName As String
            Get
                Return _RcptLocationName
            End Get
            Set(ByVal value As String)
                _RcptLocationName = value
            End Set
        End Property


        Public Property RcptUserName As String
            Get
                Return _RcptUserName
            End Get
            Set(ByVal value As String)
                _RcptUserName = value
            End Set
        End Property


        Public Property AddrLocationName As String
            Get
                Return _AddrLocationName
            End Get
            Set(ByVal value As String)
                _AddrLocationName = value
            End Set
        End Property


        Public Property AddrUserName As String
            Get
                Return _AddrUserName
            End Get
            Set(ByVal value As String)
                _AddrUserName = value
            End Set
        End Property


        Public Property CCLocationName1 As String
            Get
                Return _CCLocationName1
            End Get
            Set(ByVal value As String)
                _CCLocationName1 = value
            End Set
        End Property


        Public Property CCUserName1 As String
            Get
                Return _CCUserName1
            End Get
            Set(ByVal value As String)
                _CCUserName1 = value
            End Set
        End Property


        Public Property CCLocationName2 As String
            Get
                Return _CCLocationName2
            End Get
            Set(ByVal value As String)
                _CCLocationName2 = value
            End Set
        End Property


        Public Property CCUserName2 As String
            Get
                Return _CCUserName2
            End Get
            Set(ByVal value As String)
                _CCUserName2 = value
            End Set
        End Property


        Public Sub New()

        End Sub


    End Class

    Public Class RFQHistoryDispList
        Inherits List(Of RFQHistoryDisp)

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub


        Public Sub Load(ByVal RFQNumber As Integer)

            Dim Value As New StringBuilder
            Value.AppendLine("SELECT")
            Value.AppendLine("    RH.RFQHistoryNumber")
            Value.AppendLine("    ,RH.RFQNumber")
            Value.AppendLine("    ,RH.RFQStatusCode")
            Value.AppendLine("    ,RS.[Text] AS RFQStatus")
            Value.AppendLine("    ,RH.StatusChangeDate")
            Value.AppendLine("    ,RH.RFQCorresCode")
            Value.AppendLine("    ,RC.[Text] AS RFQCorres")
            Value.AppendLine("    ,REPLACE(RH.Note, CHAR(10), '<br />') AS Note")
            Value.AppendLine("    ,RH.SendLocationCode")
            Value.AppendLine("    ,'(' + LS.[Name] + ')' AS SendLocationName")
            Value.AppendLine("    ,RH.SendUserID")
            Value.AppendLine("    ,US.AD_DisplayName AS SendUserName")
            Value.AppendLine("    ,RH.RcptLocationCode")
            Value.AppendLine("    ,'(' + LR.[Name] + ')' AS RcptLocationName")
            Value.AppendLine("    ,RH.RcptUserID")
            Value.AppendLine("    ,UR.AD_DisplayName AS RcptUserName")
            Value.AppendLine("    ,RH.isChecked")
            Value.AppendLine("    ,RH.AddrLocationCode")
            Value.AppendLine("    ,'(' + LA.[Name] + ')' AS AddrLocationName")
            Value.AppendLine("    ,RH.AddrUserID")
            Value.AppendLine("    ,UA.AD_DisplayName AS AddrUserName")
            Value.AppendLine("    ,RH.CCLocationCode1")
            Value.AppendLine("    ,'(' + LC1.[Name] + ')' AS CCLocationName1")
            Value.AppendLine("    ,RH.CCUserID1")
            Value.AppendLine("    ,UC1.AD_DisplayName AS CCUserName1")
            Value.AppendLine("    ,RH.CCLocationCode2")
            Value.AppendLine("    ,'(' + LC2.[Name] + ')' AS CCLocationName2")
            Value.AppendLine("    ,RH.CCUserID2")
            Value.AppendLine("    ,UC2.AD_DisplayName AS CCUserName2")
            Value.AppendLine("    ,RH.CreatedBy")
            Value.AppendLine("    ,RH.CreateDate")
            Value.AppendLine("    ,RH.UpdatedBy")
            Value.AppendLine("    ,RH.UpdateDate")
            Value.AppendLine("FROM")
            Value.AppendLine("    RFQStatus As RS")
            Value.AppendLine("    ,RFQHistory AS RH")
            Value.AppendLine("        LEFT OUTER JOIN RFQCorres As RC ON RC.RFQCorresCode = RH.RFQCorresCode")
            Value.AppendLine("        LEFT OUTER JOIN s_Location AS LS ON LS.LocationCode = RH.SendLocationCode")
            Value.AppendLine("        LEFT OUTER JOIN s_User AS US ON US.UserID = RH.SendUserID")
            Value.AppendLine("        LEFT OUTER JOIN s_Location AS LR ON LR.LocationCode = RH.RcptLocationCode")
            Value.AppendLine("        LEFT OUTER JOIN s_User AS UR ON UR.UserID = RH.RcptUserID")
            Value.AppendLine("        LEFT OUTER JOIN s_Location AS LA ON LA.LocationCode = RH.AddrLocationCode")
            Value.AppendLine("        LEFT OUTER JOIN s_User AS UA ON UA.UserID = RH.AddrUserID")
            Value.AppendLine("        LEFT OUTER JOIN s_Location AS LC1 ON LC1.LocationCode = RH.CCLocationCode1")
            Value.AppendLine("        LEFT OUTER JOIN s_User AS UC1 ON UC1.UserID = RH.CCUserID1")
            Value.AppendLine("        LEFT OUTER JOIN s_Location AS LC2 ON LC2.LocationCode = RH.CCLocationCode2")
            Value.AppendLine("        LEFT OUTER JOIN s_User AS UC2 ON UC2.UserID = RH.CCUserID2")
            Value.AppendLine("WHERE")
            Value.AppendLine("    RH.RFQStatusCode = RS.RFQStatusCode")
            Value.AppendLine("    AND RH.RFQNumber = @RFQNumber")
            Value.AppendLine("ORDER BY")
            Value.AppendLine("    RH.RFQHistoryNumber DESC")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                DBConn.Open()

                Using DBCommand As SqlCommand = DBConn.CreateCommand
                    DBCommand.CommandText = Value.ToString
                    DBCommand.Parameters.Add("RFQNumber", SqlDbType.Int)
                    DBCommand.Parameters("RFQNumber").Value = RFQNumber

                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader
                        While DBReader.Read
                            Dim history As New RFQHistoryDisp
                            SetProperty(DBReader("RFQHistoryNumber"), history.RFQHistoryNumber)
                            SetProperty(DBReader("RFQNumber"), history.RFQNumber)
                            SetProperty(DBReader("RFQStatusCode"), history.RFQStatusCode)
                            SetProperty(DBReader("RFQStatus"), history.RFQStatus)
                            SetProperty(DBReader("StatusChangeDate"), history.StatusChangeDate)
                            SetProperty(DBReader("RFQCorresCode"), history.RFQCorresCode)
                            SetProperty(DBReader("RFQCorres"), history.RFQCorres)
                            SetProperty(DBReader("Note"), history.Note)
                            SetProperty(DBReader("SendLocationCode"), history.SendLocationCode)
                            SetProperty(DBReader("SendLocationName"), history.SendLocationName)
                            SetProperty(DBReader("SendUserID"), history.SendUserID)
                            SetProperty(DBReader("SendUserName"), history.SendUserName)
                            SetProperty(DBReader("RcptLocationCode"), history.RcptLocationCode)
                            SetProperty(DBReader("RcptLocationName"), history.RcptLocationName)
                            SetProperty(DBReader("RcptUserID"), history.RcptUserID)
                            SetProperty(DBReader("RcptUserName"), history.RcptUserName)
                            SetProperty(DBReader("isChecked"), history.isChecked)
                            SetProperty(DBReader("AddrLocationCode"), history.AddrLocationCode)
                            SetProperty(DBReader("AddrLocationName"), history.AddrLocationName)
                            SetProperty(DBReader("AddrUserID"), history.AddrUserID)
                            SetProperty(DBReader("AddrUserName"), history.AddrUserName)
                            SetProperty(DBReader("CCLocationCode1"), history.CCLocationCode1)
                            SetProperty(DBReader("CCLocationName1"), history.CCLocationName1)
                            SetProperty(DBReader("CCUserID1"), history.CCUserID1)
                            SetProperty(DBReader("CCUserName1"), history.CCUserName1)
                            SetProperty(DBReader("CCLocationCode2"), history.CCLocationCode2)
                            SetProperty(DBReader("CCLocationName2"), history.CCLocationName2)
                            SetProperty(DBReader("CCUserID2"), history.CCUserID2)
                            SetProperty(DBReader("CCUserName2"), history.CCUserName2)
                            SetProperty(DBReader("CreatedBy"), history.CreatedBy)
                            SetProperty(DBReader("CreateDate"), history.CreateDate)
                            SetProperty(DBReader("UpdatedBy"), history.UpdatedBy)
                            SetProperty(DBReader("UpdateDate"), history.UpdateDate)
                            Me.Add(history)
                        End While
                    End Using
                End Using
            End Using

        End Sub

    End Class

End Namespace

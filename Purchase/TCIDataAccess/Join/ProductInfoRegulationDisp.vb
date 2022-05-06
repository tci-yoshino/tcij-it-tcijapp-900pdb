Option Explicit On
Option Strict On
Option Infer Off

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Text
Imports System.Data.SqlClient
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess.Join

    Public Class ProductInfoRegulationDisp

        Protected _LocationName As String = String.Empty
        Protected _LocationCode As String = String.Empty

        Protected _Item As String = String.Empty
        Protected _Text As String = String.Empty
        Protected _IsOutputTransferOrder As Integer = 0
        Protected _IsOutputPriceRevision As Integer = 0
        Protected _UserID As Integer = 0



        Public Property LocationName() As String
            Get
                Return _LocationName
            End Get
            Set(ByVal value As String)
                _LocationName = value
            End Set
        End Property
        Public Property LocationCode() As String
            Get
                Return _LocationCode
            End Get
            Set(ByVal value As String)
                _LocationCode = value
            End Set
        End Property

        Public Property Item() As String
            Get
                Return _Item
            End Get
            Set(ByVal value As String)
                _Item = value
            End Set
        End Property

        Public Property Text As String
            Get
                Return _Text
            End Get
            Set(ByVal value As String)
                _Text = value
            End Set
        End Property

        Public Property IsOutputTransferOrder As Integer
            Get
                Return _IsOutputTransferOrder
            End Get
            Set(ByVal value As Integer)
                _IsOutputTransferOrder = value
            End Set
        End Property

        Public Property IsOutputPriceRevision As Integer
            Get
                Return _IsOutputPriceRevision
            End Get
            Set(ByVal value As Integer)
                _IsOutputPriceRevision = value
            End Set
        End Property

        Public Property UserID As Integer
            Get
                Return _UserID
            End Get
            Set(ByVal value As Integer)
                _UserID = value
            End Set
        End Property

        Public Shared Widening Operator CType(v As ProductInfoRegulationList) As ProductInfoRegulationDisp
            Throw New NotImplementedException()
        End Operator
    End Class

    Public Class ProductInfoRegulationList
        Inherits List(Of ProductInfoRegulationDisp)

        ''' <summary>
        ''' EHSヘッダー情報を取得する
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <param name="locationCode"></param>
        Public Sub Load_CreateEhsHeaderPersonalizeListSelectSQL(ByVal userID As Integer, ByVal locationCode As String)
            Dim Value As StringBuilder = New StringBuilder
            Value.AppendLine("SELECT")
            Value.AppendLine("    EH.LocationName,")
            Value.AppendLine("    EH.LocationCode,")
            Value.AppendLine("    EH.Item,")
            Value.AppendLine("    EH.Text AS Text,")
            Value.AppendLine("    EP.UserID")
            Value.AppendLine("FROM")
            Value.AppendLine("    [S_EhsHeader] AS EH")
            Value.AppendLine("LEFT OUTER JOIN")
            Value.AppendLine("    [EhsHeader_Personalize] AS EP")
            Value.AppendLine("ON")
            Value.AppendLine("    EH.Item = EP.Item")
            Value.AppendLine("    AND EP.UserID = @UserID")
            Value.AppendLine("LEFT OUTER JOIN")
            Value.AppendLine("    [s_BranchSortOrder] AS BS")
            Value.AppendLine("ON")
            Value.AppendLine("    EH.LocationCode = BS.DisplayLocationCode")
            Value.AppendLine("    AND BS.LocationCode = @LocationCode")
            Value.AppendLine("ORDER BY")
            Value.AppendLine("    CASE WHEN EH.LocationCode = @GL THEN 0 ELSE 1 END,")
            Value.AppendLine("    BS.SortOrder,")
            Value.AppendLine("    EH.SortOrder")
            Dim DBConn As SqlConnection = New SqlConnection(DBCommon.DB_CONNECT_STRING)
            Dim DBCommand As SqlCommand = DBConn.CreateCommand
            DBCommand.CommandText = Value.ToString
            DBCommand.Parameters.Clear()
            DBCommand.Parameters.AddWithValue("UserID", userID)
            DBCommand.Parameters.AddWithValue("LocationCode", locationCode)


            DBCommand.Parameters.AddWithValue("GL", Common.LOCATION_CODE_GL)
            DBConn.Open()
            Dim DBReader As SqlDataReader = DBCommand.ExecuteReader

            While DBReader.Read
                Dim _LocationName As String = String.Empty
                Dim _LocationCode As String = String.Empty
                Dim _Item As String = String.Empty
                Dim _Text As String = String.Empty
                Dim _isOutputTransferOrder As Integer = 0
                Dim _isOutputPriceRevision As Integer = 0
                Dim _userID As Integer = 0
                Dim dc_ProductInfoRegulationList As ProductInfoRegulationDisp = New ProductInfoRegulationDisp
                DBCommon.SetProperty(DBReader("LocationName"), _LocationName)
                DBCommon.SetProperty(DBReader("LocationCode"), _LocationCode)
                DBCommon.SetProperty(DBReader("Item"), _Item)
                DBCommon.SetProperty(DBReader("Text"), _Text)
                DBCommon.SetProperty(DBReader("UserID"), _userID)
                dc_ProductInfoRegulationList.LocationName = _LocationName
                dc_ProductInfoRegulationList.LocationCode = _LocationCode
                dc_ProductInfoRegulationList.Item = _Item
                dc_ProductInfoRegulationList.Text = _Text
                dc_ProductInfoRegulationList.UserID = _userID
                Me.Add(dc_ProductInfoRegulationList)

            End While

            DBReader.Close()
        End Sub



        ''' <summary>
        ''' EHSヘッダー情報を更新する
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <param name="locationCode"></param>
        Public Sub Update_ProductInfoRegulationListSQL(ByVal userID As Integer, ByVal locationCode As String)

        End Sub

        ''' <summary>
        ''' EHSヘッダー情報を削除する
        ''' </summary>
        ''' <param name="userID"></param>
        ''' <param name="locationCode"></param>
        Public Sub Delete_ProductInfoRegulationListSQL(ByVal userID As Integer, ByVal locationCode As String)

        End Sub
    End Class
End Namespace
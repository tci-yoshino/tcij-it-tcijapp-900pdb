Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Web
Imports System.Text
Imports System.Data.SqlClient
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess.Join

    ''' <summary> 
    ''' SupplierListByProductDisp データクラス 
    ''' </summary> 
    Public Class SupplierListByProductDisp

        Protected _SupplierCode As Integer = 0
        Protected _SupplierName As String = String.Empty
        Protected _Country As String = String.Empty
        Protected _Territory As String = String.Empty
        Protected _SupplierItemNumber As String = String.Empty
        Protected _Note As String = String.Empty
        Protected _UpdateDate As DateTime = New DateTime(0)
        Protected _ValidQuotation As String = String.Empty
        Protected _Url As String = String.Empty

        ''' <summary> 
        ''' SupplierCode を設定、または取得する 
        ''' </summary> 
        Public Property SupplierCode() As Integer
            Get
                Return _SupplierCode
            End Get
            Set(ByVal value As Integer)
                _SupplierCode = value
            End Set
        End Property

        ''' <summary> 
        ''' SupplierName を設定、または取得する 
        ''' </summary> 
        Public Property SupplierName() As String
            Get
                Return _SupplierName
            End Get
            Set(ByVal value As String)
                _SupplierName = value
            End Set
        End Property

        ''' <summary> 
        ''' Country を設定、または取得する 
        ''' </summary> 
        Public Property Country() As String
            Get
                Return _Country
            End Get
            Set(ByVal value As String)
                _Country = value
            End Set
        End Property

        ''' <summary> 
        ''' Territory を設定、または取得する 
        ''' </summary> 
        Public Property Territory() As String
            Get
                Return _Territory
            End Get
            Set(ByVal value As String)
                _Territory = value
            End Set
        End Property

        ''' <summary> 
        ''' SupplierItemNumber を設定、または取得する 
        ''' </summary> 
        Public Property SupplierItemNumber() As String
            Get
                Return _SupplierItemNumber
            End Get
            Set(ByVal value As String)
                _SupplierItemNumber = value
            End Set
        End Property

        ''' <summary> 
        ''' Note を設定、または取得する 
        ''' </summary> 
        Public Property Note As String
            Get
                Return _Note
            End Get
            Set(ByVal value As String)
                _Note = value
            End Set
        End Property

        ''' <summary> 
        ''' UpdateDate を設定、または取得する 
        ''' </summary> 
        Public Property UpdateDate() As DateTime
            Get
                Return _UpdateDate
            End Get
            Set(ByVal value As DateTime)
                _UpdateDate = value
            End Set
        End Property

        ''' <summary> 
        ''' ValidQuotation を設定、または取得する 
        ''' </summary> 
        Public Property ValidQuotation() As String
            Get
                Return _ValidQuotation
            End Get
            Set(ByVal value As String)
                _ValidQuotation = value
            End Set
        End Property

        ''' <summary> 
        ''' Url を設定、または取得する 
        ''' </summary> 
        Public Property Url() As String
            Get
                Return _Url
            End Get
            Set(ByVal value As String)
                _Url = value
            End Set
        End Property

        Public Shared Widening Operator CType(v As SupplierListByProduct) As SupplierListByProductDisp
            Throw New NotImplementedException()
        End Operator

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub
    End Class

    ''' <summary> 
    ''' SupplierListByProductDisp リストクラス 
    ''' </summary> 
    Public Class SupplierListByProductDispList
        Inherits List(Of SupplierListByProductDisp)

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' SupplierListByProduct情報の取得
        ''' </summary>
        ''' <param name="i_ProductID"></param>
        ''' <param name="cbl_TerritoryList"></param>
        ''' <param name="st_UpdateDateFrom"></param>
        ''' <param name="st_UpdateDateTo"></param>
        ''' <param name="hf_HiddenSortField"></param>
        ''' <param name="hf_HiddenSortType"></param>
        ''' <param name="st_ListID"></param>
        Public Sub Load(ByVal i_ProductID As Integer, ByVal cbl_TerritoryList As CheckBoxList, ByVal st_UpdateDateFrom As String, ByVal st_UpdateDateTo As String, ByVal hf_HiddenSortField As HiddenField, ByVal hf_HiddenSortType As HiddenField, ByVal st_ListID As String)
            Dim Value As StringBuilder = New StringBuilder


            Value.AppendLine("SELECT ")
            Value.AppendLine("  SP.[SupplierCode], ")
            Value.AppendLine("  ISNULL(S.[Name3], '') + N' ' + ISNULL(S.[Name4], '') AS SupplierName, ")
            Value.AppendLine("  VC.[CountryName] AS Country, ")
            Value.AppendLine("  VT.[TerritoryName] AS Territory, ")
            Value.AppendLine("  SP.[SupplierItemNumber], ")
            Value.AppendLine("  SP.[Note], ")
            Value.AppendLine("  SP.[UpdateDate], ")
            Value.AppendLine("  CASE WHEN SP.[ValidQuotation] = 0 THEN 'Y' WHEN SP.[ValidQuotation] = 1 THEN 'N' ELSE '' END AS ValidQuotation, ")
            Value.AppendLine("  CONCAT('./SuppliersProductSetting.aspx?Action=Edit&Supplier=', RTRIM(LTRIM(STR(SP.[SupplierCode]))), '&Product=', RTRIM(LTRIM(STR(P.[ProductID]))), '&Return=SP') AS Url ")
            Value.AppendLine("FROM ")
            Value.AppendLine("  [Supplier_Product] AS SP ")
            Value.AppendLine("  INNER JOIN [Supplier] AS S ")
            Value.AppendLine("    ON SP.[SupplierCode] = S.[SupplierCode] ")
            Value.AppendLine("  INNER JOIN [v_Country] AS VC ")
            Value.AppendLine("    ON S.[CountryCode] = VC.[CountryCode] ")
            Value.AppendLine("  INNER JOIN [Product] AS P ")
            Value.AppendLine("	ON SP.[ProductID] = P.[ProductID] ")
            Value.AppendLine("  INNER JOIN [v_Territory] AS VT ")
            Value.AppendLine("	ON SP.[SupplierCode] = VT.[SupplierCode] ")
            Value.AppendLine("WHERE ")
            Value.AppendLine("  SP.[ProductID] = @ProductID ")

            '絞り込み条件追加
            'Territory条件
            Dim StrComma As String = ""
            Dim i As Integer = 0
            StrComma = ""
            i = 0
            For Each Territory As ListItem In cbl_TerritoryList.Items
                'CheckboxListのチェックON判定
                If Territory.Selected Then
                    If i = 0 Then
                        Value.Append("  AND VT.[TerritoryName] IN( ")
                    End If
                    Territory.Value = Territory.Value.Replace("-", "").Replace(" ", "")
                    Value.Append(StrComma + "@" + Territory.Value)
                    'Purposeの二つ目以降の項目にOR条件を設定する
                    StrComma = ", "
                    i = i + 1
                End If
            Next
            If i > 0 Then
                Value.AppendLine(") ")
            End If
            'Update Date条件
            If Not String.IsNullOrEmpty(st_UpdateDateFrom.ToString) Then
                Value.AppendLine("    AND CONVERT(date, SP.[UpdateDate]) >= @UpdateDateFrom ")
            End If
            If Not String.IsNullOrEmpty(st_UpdateDateTo.ToString) Then
                Value.AppendLine("    AND CONVERT(date, SP.[UpdateDate]) <= @UpdateDateTo ")
            End If
            'Sort条件
            Select Case hf_HiddenSortField.Value
                Case st_ListID + "_" + "SupplierCodeHeader"
                    Select Case hf_HiddenSortType.Value
                        Case "asc"
                            Value.AppendLine("ORDER BY SP.[SupplierCode] ASC")
                        Case "desc"
                            Value.AppendLine("ORDER BY SP.[SupplierCode] DESC")
                        Case Else
                            Value.AppendLine("")
                    End Select
                Case st_ListID + "_" + "CountryHeader"
                    Select Case hf_HiddenSortType.Value
                        Case "asc"
                            Value.AppendLine("ORDER BY VC.[CountryName] ASC")
                        Case "desc"
                            Value.AppendLine("ORDER BY VC.[CountryName] DESC")
                        Case Else
                            Value.AppendLine("")
                    End Select
                Case st_ListID + "_" + "UpdateDateHeader"
                    Select Case hf_HiddenSortType.Value
                        Case "asc"
                            Value.AppendLine("ORDER BY SP.[UpdateDate] ASC")
                        Case "desc"
                            Value.AppendLine("ORDER BY SP.[UpdateDate] DESC")
                        Case Else
                            Value.AppendLine("")
                    End Select
                Case st_ListID + "_" + "ValidQuotationHeader"
                    Select Case hf_HiddenSortType.Value
                        Case "asc"
                            Value.AppendLine("ORDER BY SP.[ValidQuotation] ASC")
                        Case "desc"
                            Value.AppendLine("ORDER BY SP.[ValidQuotation] DESC")
                        Case Else
                            Value.AppendLine("")
                    End Select
                Case Else
                    Value.AppendLine("ORDER BY SP.[SupplierCode] ASC")
            End Select

            Dim DBConn As SqlConnection = New SqlConnection(DBCommon.DB_CONNECT_STRING)
            Dim DBCommand As SqlCommand = DBConn.CreateCommand
            DBCommand.CommandText = Value.ToString
            DBCommand.Parameters.Clear()

            '絞り込み条件：ProductIDバインド変数設定
            DBCommand.Parameters.AddWithValue("ProductID", i_ProductID.ToString)

            '絞り込み条件：Territory指定判定
            'CheckboxListのチェックされた項目のみバインド変数に値を設定する
            For Each TerritoryItem As ListItem In cbl_TerritoryList.Items
                'CheckboxListチェックON判定
                If TerritoryItem.Selected = True Then
                    Dim TerritoryValue As String = TerritoryItem.Text
                    TerritoryValue = TerritoryValue.Replace("-", "").Replace(" ", "")
                    DBCommand.Parameters.AddWithValue(TerritoryValue, TerritoryItem.Text)
                End If
            Next

            '絞り込み条件：Update Date(From) バインド変数設定
            If Not String.IsNullOrEmpty(st_UpdateDateFrom.ToString) Then
                DBCommand.Parameters.AddWithValue("UpdateDateFrom", st_UpdateDateFrom.ToString)
            End If

            '絞り込み条件：Update Date(To) バインド変数設定
            If Not String.IsNullOrEmpty(st_UpdateDateTo.ToString) Then
                DBCommand.Parameters.AddWithValue("UpdateDateTo", st_UpdateDateTo.ToString)
            End If

            DBConn.Open()
            Dim DBReader As SqlDataReader = DBCommand.ExecuteReader

            While DBReader.Read

                Dim dc_SupplierListByProduct As SupplierListByProductDisp = New SupplierListByProductDisp
                DBCommon.SetProperty(DBReader("SupplierCode"), dc_SupplierListByProduct.SupplierCode)
                DBCommon.SetProperty(DBReader("SupplierName"), dc_SupplierListByProduct.SupplierName)
                DBCommon.SetProperty(DBReader("Country"), dc_SupplierListByProduct.Country)
                DBCommon.SetProperty(DBReader("Territory"), dc_SupplierListByProduct.Territory)
                DBCommon.SetProperty(DBReader("SupplierItemNumber"), dc_SupplierListByProduct.SupplierItemNumber)
                DBCommon.SetProperty(DBReader("Note"), dc_SupplierListByProduct.Note)
                DBCommon.SetProperty(DBReader("UpdateDate"), dc_SupplierListByProduct.UpdateDate)
                DBCommon.SetProperty(DBReader("ValidQuotation"), dc_SupplierListByProduct.ValidQuotation)
                DBCommon.SetProperty(DBReader("Url"), dc_SupplierListByProduct.Url)

                Me.Add(dc_SupplierListByProduct)

            End While

            DBReader.Close()

        End Sub

    End Class

End Namespace

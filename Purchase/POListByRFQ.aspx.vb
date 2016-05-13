Imports System.Data.SqlClient
Imports Purchase.Common

Partial Public Class POListByRFQ
    Inherits CommonPage

    Protected st_RFQLineNumber As String = String.Empty

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        ' コントロール初期化
        Msg.Text = ""

        ' パラメータ取得
        If Request.RequestType = "POST" Then
            st_RFQLineNumber = IIf(Request.Form("RFQLineNumber") = Nothing, "", Request.Form("RFQLineNumber"))
        ElseIf Request.RequestType = "GET" Then
            st_RFQLineNumber = IIf(Request.QueryString("RFQLineNumber") = Nothing, "", Request.QueryString("RFQLineNumber"))
        End If

        ' 空白除去
        st_RFQLineNumber = st_RFQLineNumber.Trim

        ' パラメータチェック
        If (String.IsNullOrEmpty(st_RFQLineNumber)) Or (Not Regex.IsMatch(st_RFQLineNumber, "^[0-9]+$")) Then
            st_RFQLineNumber = ""
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If

        '権限ロールに従い極秘品はエラーとする
        If Session(SESSION_ROLE_CODE).ToString = ROLE_WRITE_P OrElse Session(SESSION_ROLE_CODE).ToString = ROLE_READ_P Then
            Using sqlConn As SqlConnection = New SqlConnection(DB_CONNECT_STRING)
                Using sqlCmd As SqlCommand = sqlConn.CreateCommand()

                    sqlCmd.CommandText = "SELECT 1 FROM v_RFQLine WHERE isCONFIDENTIAL = 1 AND RFQLineNumber = @RFQLineNumber"
                    sqlCmd.Parameters.AddWithValue("RFQLineNumber", st_RFQLineNumber)
                    sqlConn.Open()
                    Dim dr As SqlDataReader = sqlCmd.ExecuteReader
                    If dr.Read = True Then
                        Response.Redirect("AuthError.html")
                    End If
                End Using
            End Using
        End If

        ' PO データを取得する
        SrcPO.SelectParameters.Clear()
        SrcPO.SelectParameters.Add("RFQLineNumber", st_RFQLineNumber)
        SrcPO.SelectCommand = _
              "SELECT " _
            & "  PONumber, StatusChangeDate, Status, ProductNumber, ProductName, " _
            & "  PODate, POUserName, POLocationName, SupplierName, MakerName, " _
            & "  DeliveryDate, OrderQuantity, OrderUnitCode, CurrencyCode, " _
            & "  UnitPrice, PerQuantity, PerUnitCode, isCONFIDENTIAL " _
            & "FROM " _
            & "  v_PO " _
            & "WHERE " _
            & "  RFQLineNumber = @RFQLineNumber " _
            & "ORDER BY " _
            & "  StatusSortOrder ASC " _
            & "  PONumber ASC "

    End Sub

End Class
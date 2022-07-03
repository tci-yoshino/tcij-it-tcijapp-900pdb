Imports System.Data.SqlClient
Imports Purchase.TCIDataAccess.DBCommon

Namespace TCIDataAccess.Join

    ''' <summary>
    ''' PurchasingUserDisp データクラス
    ''' </summary>
    Public Class PurchasingUserDisp
        Inherits TCIDataAccess.PurchasingUser

        Protected _UserName As String = String.Empty
        Protected _AccountName As String = String.Empty
        Protected _Surname As String = String.Empty
        Protected _GivenName As String = String.Empty
        Protected _Email As String = String.Empty
        Protected _LocationCode As String = String.Empty
        Protected _LocationName As String = String.Empty
        Protected _RoleName As String = String.Empty
        Protected _DefaultCCUserName1 As String = String.Empty
        Protected _DefaultCCUserName2 As String = String.Empty

        ''' <summary>
        ''' UserName を設定, または取得します
        ''' </summary>
        ''' <returns></returns>
        Public Property UserName() As String
            Get
                Return _UserName
            End Get
            Set(ByVal value As String)
                _UserName = value
            End Set
        End Property

        ''' <summary>
        ''' AccountName を設定, または取得します
        ''' </summary>
        Public Property AccountName() As String
            Get
                Return _AccountName
            End Get
            Set(ByVal value As String)
                _AccountName = value
            End Set
        End Property

        ''' <summary>
        ''' Surname を設定, または取得します
        ''' </summary>
        Public Property Surname() As String
            Get
                Return _Surname
            End Get
            Set(ByVal value As String)
                _Surname = value
            End Set
        End Property

        ''' <summary>
        ''' GivenName を設定, または取得します
        ''' </summary>
        Public Property GivenName() As String
            Get
                Return _GivenName
            End Get
            Set(ByVal value As String)
                _GivenName = value
            End Set
        End Property

        ''' <summary>
        ''' Email を設定, または取得します
        ''' </summary>
        Public Property Email() As String
            Get
                Return _Email
            End Get
            Set(ByVal value As String)
                _Email = value
            End Set
        End Property

        ''' <summary>
        ''' LocationCode を設定, または取得します
        ''' </summary>
        Public Property LocationCode() As String
            Get
                Return _LocationCode
            End Get
            Set(ByVal value As String)
                _LocationCode = value
            End Set
        End Property

        ''' <summary>
        ''' LocationName を設定, または取得します
        ''' </summary>
        ''' <returns></returns>
        Public Property LocationName() As String
            Get
                Return _LocationName
            End Get
            Set(ByVal value As String)
                _LocationName = value
            End Set
        End Property

        ''' <summary>
        ''' RoleName を設定, または取得します
        ''' </summary>
        Public Property RoleName() As String
            Get
                Return _RoleName
            End Get
            Set(ByVal value As String)
                _RoleName = value
            End Set
        End Property

        ''' <summary>
        ''' DefaultCCUserName1 を設定, または取得します
        ''' </summary>
        Public Property DefaultCCUserName1() As String
            Get
                Return _DefaultCCUserName1
            End Get
            Set(ByVal value As String)
                _DefaultCCUserName1 = value
            End Set
        End Property

        ''' <summary>
        ''' DefaultCCUserName2 を設定, または取得します
        ''' </summary>
        Public Property DefaultCCUserName2() As String
            Get
                Return _DefaultCCUserName2
            End Get
            Set(ByVal value As String)
                _DefaultCCUserName2 = value
            End Set
        End Property

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' ユーザ情報を取得します
        ''' </summary>
        ''' <param name="UserID">ユーザ ID</param>
        Public Overloads Sub Load(ByVal UserID As Integer)

            Dim Value As New StringBuilder
            Value.AppendLine("SELECT")
            Value.AppendLine("    PU.UserID")
            Value.AppendLine("    ,U.AD_DisplayName AS UserName")
            Value.AppendLine("    ,U.AD_AccountName AS AccountName")
            Value.AppendLine("    ,U.AD_Surname AS Surname")
            Value.AppendLine("    ,U.AD_GivenName AS GivenName")
            Value.AppendLine("    ,U.AD_Email AS Email")
            Value.AppendLine("    ,U.LocationCode")
            Value.AppendLine("    ,L.[Name] AS LocationName")
            Value.AppendLine("    ,PU.RoleCode")
            Value.AppendLine("    ,CASE PU.RoleCode")
            Value.AppendLine("        WHEN 'WRITE_AA' THEN 'AA'")
            Value.AppendLine("        WHEN 'WRITE' THEN 'A'")
            Value.AppendLine("        WHEN 'WRITE_P' THEN 'B'")
            Value.AppendLine("        WHEN 'READ_P' THEN 'C'")
            Value.AppendLine("        WHEN 'ADMIN' THEN 'ADMIN'")
            Value.AppendLine("    END AS RoleName")
            Value.AppendLine("    ,PU.PrivilegeLevel")
            Value.AppendLine("    ,PU.R3PurchasingGroup")
            Value.AppendLine("    ,PU.RFQCorrespondenceEditable")
            Value.AppendLine("    ,PU.MMSTAInvalidationEditable")
            Value.AppendLine("    ,PU.DefaultCCUserID1")
            Value.AppendLine("    ,CC1.[Name] AS DefaultCCUserName1")
            Value.AppendLine("    ,PU.DefaultCCUserID2")
            Value.AppendLine("    ,CC2.[Name] AS DefaultCCUserName2")
            Value.AppendLine("    ,PU.isAdmin")
            Value.AppendLine("    ,PU.isDisabled")
            Value.AppendLine("    ,PU.CreatedBy")
            Value.AppendLine("    ,PU.CreateDate")
            Value.AppendLine("    ,PU.UpdatedBy")
            Value.AppendLine("    ,PU.UpdateDate")
            Value.AppendLine("FROM")
            Value.AppendLine("    s_Location AS L")
            Value.AppendLine("    ,s_User AS U")
            Value.AppendLine("    ,PurchasingUser AS PU")
            Value.AppendLine("        LEFT OUTER JOIN v_User AS CC1 ON CC1.UserID = PU.DefaultCCUserID1")
            Value.AppendLine("        LEFT OUTER JOIN v_User AS CC2 ON CC2.UserID = PU.DefaultCCUserID2")
            Value.AppendLine("WHERE")
            Value.AppendLine("    PU.UserID = @UserID")
            Value.AppendLine("    AND PU.UserID = U.UserID")
            Value.AppendLine("    AND U.LocationCode = L.LocationCode")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                DBConn.Open()

                Using DBCommand As SqlCommand = DBConn.CreateCommand
                    DBCommand.CommandText = Value.ToString
                    DBCommand.Parameters.Add("UserID", SqlDbType.Int)
                    DBCommand.Parameters("UserID").Value = UserID

                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader
                        While DBReader.Read()
                            SetProperty(DBReader("UserID"), _UserID)
                            SetProperty(DBReader("UserName"), _UserName)
                            SetProperty(DBReader("AccountName"), _AccountName)
                            SetProperty(DBReader("Surname"), _Surname)
                            SetProperty(DBReader("GivenName"), _GivenName)
                            SetProperty(DBReader("Email"), _Email)
                            SetProperty(DBReader("LocationCode"), _LocationCode)
                            SetProperty(DBReader("LocationName"), _LocationName)
                            SetProperty(DBReader("RoleCode"), _RoleCode)
                            SetProperty(DBReader("RoleName"), _RoleName)
                            SetProperty(DBReader("PrivilegeLevel"), _PrivilegeLevel)
                            SetProperty(DBReader("R3PurchasingGroup"), _R3PurchasingGroup)
                            SetProperty(DBReader("RFQCorrespondenceEditable"), _RFQCorrespondenceEditable)
                            SetProperty(DBReader("MMSTAInvalidationEditable"), _MMSTAInvalidationEditable)
                            SetProperty(DBReader("DefaultCCUserID1"), _DefaultCCUserID1)
                            SetProperty(DBReader("DefaultCCUserName1"), _DefaultCCUserName1)
                            SetProperty(DBReader("DefaultCCUserID2"), _DefaultCCUserID2)
                            SetProperty(DBReader("DefaultCCUserName2"), _DefaultCCUserName2)
                            SetProperty(DBReader("isAdmin"), _isAdmin)
                            SetProperty(DBReader("isDisabled"), _isDisabled)
                            SetProperty(DBReader("CreatedBy"), _CreatedBy)
                            SetProperty(DBReader("CreateDate"), _CreateDate)
                            SetProperty(DBReader("UpdatedBy"), _UpdatedBy)
                            SetProperty(DBReader("UpdateDate"), _UpdateDate)
                        End While
                    End Using
                End Using
            End Using

        End Sub

        ''' <summary>
        ''' ユーザが有効か否かを返します
        ''' </summary>
        ''' <param name="LocationCode">拠点コード</param>
        ''' <param name="UserID">ユーザ ID</param>
        ''' <returns>有効な場合は True, 無効な場合は False</returns>
        Public Shared Function IsActive(ByVal LocationCode As String, ByVal UserID As Integer) As Boolean

            Dim Value As New StringBuilder
            Value.AppendLine("SELECT")
            Value.AppendLine("    COUNT(*)")
            Value.AppendLine("FROM")
            Value.AppendLine("    [v_UserAll]")
            Value.AppendLine("WHERE")
            Value.AppendLine("    [isDisabled] = 0")
            Value.AppendLine("    AND [LocationCode] = @LocationCode")
            Value.AppendLine("    AND [UserID] = @UserID")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                DBConn.Open()

                Using DBCommand As SqlCommand = DBConn.CreateCommand
                    DBCommand.CommandText = Value.ToString
                    DBCommand.Parameters.Add("LocationCode", SqlDbType.VarChar)
                    DBCommand.Parameters.Add("UserID", SqlDbType.Int)
                    DBCommand.Parameters("LocationCode").Value = LocationCode
                    DBCommand.Parameters("UserID").Value = UserID

                    Dim count As Integer = Convert.ToInt32(DBCommand.ExecuteScalar())
                    Return count > 0
                End Using
            End Using

        End Function

    End Class

    ''' <summary>
    ''' PurchasingUserDisp データリストクラス
    ''' </summary>
    Public Class PurchasingUserDispList
        Inherits List(Of PurchasingUserDisp)

        ''' <summary>
        ''' コンストラクタ
        ''' </summary>
        Public Sub New()

        End Sub

        ''' <summary>
        ''' ユーザ情報を読み込みます
        ''' </summary>
        ''' <param name="LocationCode">拠点コード</param>
        Public Sub LoadAllUsers(ByVal LocationCode As String)

            Dim Value As New StringBuilder
            Value.AppendLine("SELECT")
            Value.AppendLine("    PU.UserID")
            Value.AppendLine("    ,U.AD_DisplayName AS UserName")
            Value.AppendLine("    ,U.AD_AccountName AS AccountName")
            Value.AppendLine("    ,U.AD_Surname AS Surname")
            Value.AppendLine("    ,U.AD_GivenName AS GivenName")
            Value.AppendLine("    ,U.AD_Email AS Email")
            Value.AppendLine("    ,U.LocationCode")
            Value.AppendLine("    ,L.[Name] AS LocationName")
            Value.AppendLine("    ,PU.RoleCode")
            Value.AppendLine("    ,CASE PU.RoleCode")
            Value.AppendLine("        WHEN 'WRITE_AA' THEN 'AA'")
            Value.AppendLine("        WHEN 'WRITE' THEN 'A'")
            Value.AppendLine("        WHEN 'WRITE_P' THEN 'B'")
            Value.AppendLine("        WHEN 'READ_P' THEN 'C'")
            Value.AppendLine("        WHEN 'ADMIN' THEN 'ADMIN'")
            Value.AppendLine("    END AS RoleName")
            Value.AppendLine("    ,PU.PrivilegeLevel")
            Value.AppendLine("    ,PU.R3PurchasingGroup")
            Value.AppendLine("    ,PU.RFQCorrespondenceEditable")
            Value.AppendLine("    ,PU.MMSTAInvalidationEditable")
            Value.AppendLine("    ,PU.DefaultCCUserID1")
            Value.AppendLine("    ,CC1.[Name] AS DefaultCCUserName1")
            Value.AppendLine("    ,PU.DefaultCCUserID2")
            Value.AppendLine("    ,CC2.[Name] AS DefaultCCUserName2")
            Value.AppendLine("    ,PU.isAdmin")
            Value.AppendLine("    ,PU.isDisabled")
            Value.AppendLine("    ,PU.CreatedBy")
            Value.AppendLine("    ,PU.CreateDate")
            Value.AppendLine("    ,PU.UpdatedBy")
            Value.AppendLine("    ,PU.UpdateDate")
            Value.AppendLine("FROM")
            Value.AppendLine("    s_Location AS L")
            Value.AppendLine("    ,s_User AS U")
            Value.AppendLine("    ,PurchasingUser AS PU")
            Value.AppendLine("        LEFT OUTER JOIN v_User AS CC1 ON CC1.UserID = PU.DefaultCCUserID1")
            Value.AppendLine("        LEFT OUTER JOIN v_User AS CC2 ON CC2.UserID = PU.DefaultCCUserID2")
            Value.AppendLine("WHERE")
            Value.AppendLine("    PU.isDisabled = 0")
            Value.AppendLine("    AND PU.UserID = U.UserID")
            Value.AppendLine("    AND U.LocationCode = @LocationCode")
            Value.AppendLine("    AND U.LocationCode = L.LocationCode")
            Value.AppendLine("ORDER BY")
            Value.AppendLine("    U.AD_Surname")
            Value.AppendLine("    ,U.AD_GivenName")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                DBConn.Open()

                Using DBCommand As SqlCommand = DBConn.CreateCommand
                    DBCommand.CommandText = Value.ToString
                    DBCommand.Parameters.Add("LocationCode", SqlDbType.VarChar)
                    DBCommand.Parameters("LocationCode").Value = LocationCode

                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader
                        While DBReader.Read
                            Dim user As New PurchasingUserDisp
                            SetProperty(DBReader("UserID"), user.UserID)
                            SetProperty(DBReader("UserName"), user.UserName)
                            SetProperty(DBReader("AccountName"), user.AccountName)
                            SetProperty(DBReader("Surname"), user.Surname)
                            SetProperty(DBReader("GivenName"), user.GivenName)
                            SetProperty(DBReader("Email"), user.Email)
                            SetProperty(DBReader("LocationCode"), user.LocationCode)
                            SetProperty(DBReader("LocationName"), user.LocationName)
                            SetProperty(DBReader("RoleCode"), user.RoleCode)
                            SetProperty(DBReader("RoleName"), user.RoleName)
                            SetProperty(DBReader("PrivilegeLevel"), user.PrivilegeLevel)
                            SetProperty(DBReader("R3PurchasingGroup"), user.R3PurchasingGroup)
                            SetProperty(DBReader("RFQCorrespondenceEditable"), user.RFQCorrespondenceEditable)
                            SetProperty(DBReader("MMSTAInvalidationEditable"), user.MMSTAInvalidationEditable)
                            SetProperty(DBReader("DefaultCCUserID1"), user.DefaultCCUserID1)
                            SetProperty(DBReader("DefaultCCUserName1"), user.DefaultCCUserName1)
                            SetProperty(DBReader("DefaultCCUserID2"), user.DefaultCCUserID2)
                            SetProperty(DBReader("DefaultCCUserName2"), user.DefaultCCUserName2)
                            SetProperty(DBReader("isAdmin"), user.isAdmin)
                            SetProperty(DBReader("isDisabled"), user.isDisabled)
                            SetProperty(DBReader("CreatedBy"), user.CreatedBy)
                            SetProperty(DBReader("CreateDate"), user.CreateDate)
                            SetProperty(DBReader("UpdatedBy"), user.UpdatedBy)
                            SetProperty(DBReader("UpdateDate"), user.UpdateDate)
                            Me.Add(user)
                        End While
                    End Using
                End Using
            End Using

        End Sub

        ''' <summary>
        ''' 編集権限を持つユーザ情報を読み込みます
        ''' </summary>
        ''' <param name="LocationCode">拠点コード</param>
        Public Sub LoadEditUsers(ByVal LocationCode As String)

            LoadEditUsers(LocationCode, False)

        End Sub

        ''' <summary>
        ''' 編集権限を持つユーザ情報を読み込みます
        ''' </summary>
        ''' <param name="LocationCode">拠点コード</param>
        ''' <param name="IsConfidential">極秘品フラグ</param>
        Public Sub LoadEditUsers(ByVal LocationCode As String, ByVal IsConfidential As Boolean)

            Dim Value As New StringBuilder
            Value.AppendLine("SELECT")
            Value.AppendLine("    PU.UserID")
            Value.AppendLine("    ,U.AD_DisplayName AS UserName")
            Value.AppendLine("    ,U.AD_AccountName AS AccountName")
            Value.AppendLine("    ,U.AD_Surname AS Surname")
            Value.AppendLine("    ,U.AD_GivenName AS GivenName")
            Value.AppendLine("    ,U.AD_Email AS Email")
            Value.AppendLine("    ,U.LocationCode")
            Value.AppendLine("    ,L.[Name] AS LocationName")
            Value.AppendLine("    ,PU.RoleCode")
            Value.AppendLine("    ,CASE PU.RoleCode")
            Value.AppendLine("        WHEN 'WRITE_AA' THEN 'AA'")
            Value.AppendLine("        WHEN 'WRITE' THEN 'A'")
            Value.AppendLine("        WHEN 'WRITE_P' THEN 'B'")
            Value.AppendLine("        WHEN 'READ_P' THEN 'C'")
            Value.AppendLine("        WHEN 'ADMIN' THEN 'ADMIN'")
            Value.AppendLine("    END AS RoleName")
            Value.AppendLine("    ,PU.PrivilegeLevel")
            Value.AppendLine("    ,PU.R3PurchasingGroup")
            Value.AppendLine("    ,PU.RFQCorrespondenceEditable")
            Value.AppendLine("    ,PU.MMSTAInvalidationEditable")
            Value.AppendLine("    ,PU.DefaultCCUserID1")
            Value.AppendLine("    ,CC1.[Name] AS DefaultCCUserName1")
            Value.AppendLine("    ,PU.DefaultCCUserID2")
            Value.AppendLine("    ,CC2.[Name] AS DefaultCCUserName2")
            Value.AppendLine("    ,PU.isAdmin")
            Value.AppendLine("    ,PU.isDisabled")
            Value.AppendLine("    ,PU.CreatedBy")
            Value.AppendLine("    ,PU.CreateDate")
            Value.AppendLine("    ,PU.UpdatedBy")
            Value.AppendLine("    ,PU.UpdateDate")
            Value.AppendLine("FROM")
            Value.AppendLine("    s_Location AS L")
            Value.AppendLine("    ,s_User AS U")
            Value.AppendLine("    ,PurchasingUser AS PU")
            Value.AppendLine("        LEFT OUTER JOIN v_User AS CC1 ON CC1.UserID = PU.DefaultCCUserID1")
            Value.AppendLine("        LEFT OUTER JOIN v_User AS CC2 ON CC2.UserID = PU.DefaultCCUserID2")
            Value.AppendLine("WHERE")
            Value.AppendLine("    PU.isDisabled = 0")
            If IsConfidential Then
                Value.AppendLine("    AND PU.RoleCode IN ('WRITE', 'WRITE_AA')")
            Else
                Value.AppendLine("    AND PU.RoleCode IN ('WRITE_P', 'WRITE', 'WRITE_AA')")
            End If
            Value.AppendLine("    AND PU.UserID = U.UserID")
            Value.AppendLine("    AND U.LocationCode = @LocationCode")
            Value.AppendLine("    AND U.LocationCode = L.LocationCode")
            Value.AppendLine("ORDER BY")
            Value.AppendLine("    U.AD_DisplayName")

            Using DBConn As New SqlConnection(DB_CONNECT_STRING)
                DBConn.Open()

                Using DBCommand As SqlCommand = DBConn.CreateCommand
                    DBCommand.CommandText = Value.ToString
                    DBCommand.Parameters.Add("LocationCode", SqlDbType.VarChar)
                    DBCommand.Parameters("LocationCode").Value = LocationCode

                    Using DBReader As SqlDataReader = DBCommand.ExecuteReader
                        While DBReader.Read
                            Dim user As New PurchasingUserDisp
                            SetProperty(DBReader("UserID"), user.UserID)
                            SetProperty(DBReader("UserName"), user.UserName)
                            SetProperty(DBReader("AccountName"), user.AccountName)
                            SetProperty(DBReader("Surname"), user.Surname)
                            SetProperty(DBReader("GivenName"), user.GivenName)
                            SetProperty(DBReader("Email"), user.Email)
                            SetProperty(DBReader("LocationCode"), user.LocationCode)
                            SetProperty(DBReader("LocationName"), user.LocationName)
                            SetProperty(DBReader("RoleCode"), user.RoleCode)
                            SetProperty(DBReader("RoleName"), user.RoleName)
                            SetProperty(DBReader("PrivilegeLevel"), user.PrivilegeLevel)
                            SetProperty(DBReader("R3PurchasingGroup"), user.R3PurchasingGroup)
                            SetProperty(DBReader("RFQCorrespondenceEditable"), user.RFQCorrespondenceEditable)
                            SetProperty(DBReader("MMSTAInvalidationEditable"), user.MMSTAInvalidationEditable)
                            SetProperty(DBReader("DefaultCCUserID1"), user.DefaultCCUserID1)
                            SetProperty(DBReader("DefaultCCUserName1"), user.DefaultCCUserName1)
                            SetProperty(DBReader("DefaultCCUserID2"), user.DefaultCCUserID2)
                            SetProperty(DBReader("DefaultCCUserName2"), user.DefaultCCUserName2)
                            SetProperty(DBReader("isAdmin"), user.isAdmin)
                            SetProperty(DBReader("isDisabled"), user.isDisabled)
                            SetProperty(DBReader("CreatedBy"), user.CreatedBy)
                            SetProperty(DBReader("CreateDate"), user.CreateDate)
                            SetProperty(DBReader("UpdatedBy"), user.UpdatedBy)
                            SetProperty(DBReader("UpdateDate"), user.UpdateDate)
                            Me.Add(user)
                        End While
                    End Using
                End Using
            End Using

        End Sub

    End Class

End Namespace


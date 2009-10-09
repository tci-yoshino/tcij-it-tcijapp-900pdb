Partial Public Class UserList
    Inherits Page
    'Inherits CommonPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        SrcUser.SelectCommand = "SELECT UserID,LocationName,AccountName,SurName,GivenName,RoleCode,PrivilegeLevel,isAdmin,isDisabled,'UserSetting.aspx?Action=Edit&UserID=' + Cast(UserID AS varchar) AS URL " & _
                                "FROM v_UserAll ORDER BY LocationName,isDisabled,SurName,GivenName"
    End Sub

End Class
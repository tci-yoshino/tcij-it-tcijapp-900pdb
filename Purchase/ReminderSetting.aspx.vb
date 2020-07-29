Option Explicit On
Option Infer Off
Option Strict On
Imports System.Data.SqlClient
Public Class ReminderSetting
    'Inherits System.Web.UI.Page
    Inherits CommonPage

    Const SAVE_ACTION As String = "Save"
    Const EDIT_ACTION As String = "Edit"
    Const ALREADY_EXIST As String = "Your requested Plant already exist.<br />(Please check again to avoid duplication.)"
    Dim pId As Integer
    Dim rem1 As String
    Dim rem2 As String
    Dim rem3 As String
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Msg.Text = String.Empty
        If IsPostBack = False Then
            If Common.GetHttpAction(Request) = EDIT_ACTION Then
                txtPlant.Text = Common.GetHttpQuery(Request, "SupplyingPlant")
                If txtPlant.Text.Length = 0 Then
                    Msg.Text = Common.ERR_INVALID_PARAMETER
                    Exit Sub
                End If
                Dim st_SQL As String = String.Empty
                st_SQL &= "SELECT "
                st_SQL &= "SupplyingPlant, "
                st_SQL &= "ShowType, "
                st_SQL &= "FirstRem, "
                st_SQL &= "SecondRem, "
                st_SQL &= "ThirdRem "
                'st_SQL &= "isnull(ShowType, 0) As ShowType, "
                'st_SQL &= "Case "
                'st_SQL &= "WHEN isnull(ShowType, 0) = 0 THEN "
                'st_SQL &= "Case when FirstRem=''then '0.8' else "
                'st_SQL &= "isnull(FirstRem, 0.8) End "
                'st_SQL &= "Else "
                'st_SQL &= "FirstRem "
                'st_SQL &= "End As FirstRem, "
                'st_SQL &= "Case "
                'st_SQL &= "WHEN isnull(ShowType, 0) = 0 THEN "
                'st_SQL &= "Case when SecondRem=''then '0.2' else "
                'st_SQL &= "isnull(SecondRem, 0.2) End "
                'st_SQL &= "Else "
                'st_SQL &= "SecondRem "
                'st_SQL &= "End As SecondRem, "
                'st_SQL &= "Case "
                'st_SQL &= "WHEN isnull(ShowType, 0) = 0 THEN "
                'st_SQL &= "Case when ThirdRem=''then '0.6' else "
                'st_SQL &= "isnull(ThirdRem, 0.6) End "
                'st_SQL &= "Else "
                'st_SQL &= "ThirdRem "
                'st_SQL &= "End As ThirdRem "
                st_SQL &= "FROM "
                st_SQL &= "Purchase.dbo.Reminder "
                st_SQL &= "WHERE "
                st_SQL &= "SupplyingPlant= "
                st_SQL &= "'" + txtPlant.Text + "'"
                Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
                    Dim DBCommand As SqlCommand = DBConn.CreateCommand()
                    DBCommand.CommandText = st_SQL
                    DBConn.Open()
                    Dim DBReader As SqlDataReader = DBCommand.ExecuteReader()
                    If DBReader.Read = True Then
                        'txtFirstRem.Text = CStr(DBReader("FirstRem"))
                        'txtSecondRem.Text = CStr(DBReader("SecondRem"))
                        'txtThirdRem.Text = CStr(DBReader("ThirdRem"))
                        ddlShowType.SelectedValue = CStr(DBReader("ShowType"))
                        rem1 = CStr(DBReader("FirstRem"))
                        rem2 = CStr(DBReader("SecondRem"))
                        rem3 = CStr(DBReader("ThirdRem"))
                        pId = CInt(ddlShowType.SelectedValue)
                        If pId = 0 Then
                            txtConstant1.Text = rem1
                            txtConstant2.Text = rem2
                            txtConstant3.Text = rem3
                            txtFirstRem.ReadOnly = True
                            txtSecondRem.ReadOnly = True
                            txtThirdRem.ReadOnly = True
                            txtConstant1.Focus()
                        Else
                            txtFirstRem.Text = rem1
                            txtSecondRem.Text = rem2
                            txtThirdRem.Text = rem3
                            txtConstant1.ReadOnly = True
                            txtConstant2.ReadOnly = True
                            txtConstant3.ReadOnly = True
                            txtFirstRem.Focus()
                        End If
                    Else
                        Msg.Text = Common.MSG_NO_DATA_FOUND
                        Exit Sub
                    End If
                    DBReader.Close()
                End Using
            Else
                txtPlant.CssClass = String.Empty
                'txtPlant.ReadOnly = False
            End If
        End If

    End Sub

    Protected Sub Save_Click(sender As Object, e As EventArgs) Handles Save.Click
        If Common.GetHttpAction(Request) <> SAVE_ACTION Then
            Msg.Text = Common.ERR_INVALID_PARAMETER
            Exit Sub
        End If
        If txtPlant.Text.Length = 0 Then
            Msg.Text = "Plant " & Common.ERR_REQUIRED_FIELD
            Exit Sub
        End If
        Dim st_SQL As String = String.Empty
        If Common.ExistenceConfirmation("Reminder", "SupplyingPlant", txtPlant.Text) = False Then    '[SupplyingPlantのReminder存在有無]
            Msg.Text = Common.ERR_DELETED_BY_ANOTHER_USER
            Exit Sub
        End If
        Dim SId As Integer
        SId = CInt(ddlShowType.SelectedValue)
        If SId = 0 Then
            If IsNumeric(txtConstant1.Text) And IsNumeric(txtConstant2.Text) And IsNumeric(txtConstant3.Text) Then
                Dim Constant1 As Match = Regex.Match(txtConstant1.Text, "^-?[1-9]\d*$|^0$")
                Dim Constant2 As Match = Regex.Match(txtConstant2.Text, "^-?[1-9]\d*$|^0$")
                Dim Constant3 As Match = Regex.Match(txtConstant3.Text, "^-?[1-9]\d*$|^0$")
                If Constant1.Success = True And Constant2.Success = True And Constant3.Success = True Then
                Else
                    ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>IntoInteger();</script>")
                    Exit Sub
                End If
            Else
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>IntoNumber();</script>")
                Exit Sub
            End If
        Else
            If IsNumeric(txtFirstRem.Text) And IsNumeric(txtSecondRem.Text) And IsNumeric(txtThirdRem.Text) Then
                Dim FirstRem As Match = Regex.Match(txtFirstRem.Text, "^[0]+(.[0-9]{1,3})?$")
                Dim SecondRem As Match = Regex.Match(txtSecondRem.Text, "^[0]+(.[0-9]{1,3})?$")
                Dim ThirdRem As Match = Regex.Match(txtThirdRem.Text, "^[0]+(.[0-9]{1,3})?$")
                If FirstRem.Success = True And SecondRem.Success = True And ThirdRem.Success = True Then
                Else
                    ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>IntoDecimal();</script>")
                    Exit Sub
                End If
            Else
                ClientScript.RegisterStartupScript(ClientScript.GetType(), "myscript", "<script>IntoNumber();</script>")
                Exit Sub
            End If
        End If
        'st_SQL &= "USE [Purchase] "
        'st_SQL &= "GO "

        If SId = 0 Then
            st_SQL &= "UPDATE [dbo].[Reminder] "
            st_SQL &= "SET "
            st_SQL &= "FirstRem='" & txtConstant1.Text & "', "
            st_SQL &= "SecondRem='" & txtConstant2.Text & "', "
            st_SQL &= "ThirdRem='" & txtConstant3.Text & "', "
            st_SQL &= "ShowType='" & ddlShowType.Text & "' "
            st_SQL &= "where SupplyingPlant='" & txtPlant.Text & "'"
        Else
            st_SQL &= "UPDATE [dbo].[Reminder] "
            st_SQL &= "SET "
            st_SQL &= "FirstRem='" & txtFirstRem.Text & "', "
            st_SQL &= "SecondRem='" & txtSecondRem.Text & "', "
            st_SQL &= "ThirdRem='" & txtThirdRem.Text & "', "
            st_SQL &= "ShowType='" & ddlShowType.Text & "' "
            st_SQL &= "where SupplyingPlant='" & txtPlant.Text & "'"
        End If

        Using DBConn As New SqlConnection(Common.DB_CONNECT_STRING)
            Dim DBCommand As SqlCommand = DBConn.CreateCommand()
            DBCommand.CommandText = st_SQL
            DBConn.Open()
            DBCommand.ExecuteNonQuery()
        End Using

        '[呼出元のフォームに戻る]----------------------------------------------------------
        Response.Redirect("ReminderList.aspx")
    End Sub


    Protected Sub ddlShowType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlShowType.SelectedIndexChanged
        Dim SId As Integer
        SId = CInt(ddlShowType.SelectedValue)
        If SId = 0 Then
            txtFirstRem.ReadOnly = True
            txtSecondRem.ReadOnly = True
            txtThirdRem.ReadOnly = True
            txtConstant1.ReadOnly = False
            txtConstant2.ReadOnly = False
            txtConstant3.ReadOnly = False
        Else
            txtFirstRem.ReadOnly = False
            txtSecondRem.ReadOnly = False
            txtThirdRem.ReadOnly = False
            txtConstant1.ReadOnly = True
            txtConstant2.ReadOnly = True
            txtConstant3.ReadOnly = True

        End If
        If pId = 0 And SId = pId Then
            txtConstant1.Text = 0.ToString()
            txtConstant2.Text = 0.ToString()
            txtConstant3.Text = 0.ToString()
            txtFirstRem.Text = ""
            txtSecondRem.Text = ""
            txtThirdRem.Text = ""
            txtConstant1.Focus()
        Else
            txtFirstRem.Text = 0.ToString()
            txtSecondRem.Text = 0.ToString()
            txtThirdRem.Text = 0.ToString()
            txtConstant1.Text = ""
            txtConstant2.Text = ""
            txtConstant3.Text = ""
            txtFirstRem.Focus()
        End If
    End Sub

    Protected Sub txtFirstRem_TextChanged(sender As Object, e As EventArgs) Handles txtFirstRem.TextChanged

    End Sub
End Class
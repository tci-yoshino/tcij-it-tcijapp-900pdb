<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReminderSetting.aspx.vb" Inherits="Purchase.ReminderSetting" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>
    <form id="CountryForm" runat="server">
        <div id="content">
            <div class="tabs"></div>
            <h3>Reminder Setting</h3>
            <div class="main">
                <p class="attention">
                    <asp:Label ID="Msg" runat="server" Text=""></asp:Label>
                </p>
                <table>
                    <tr>
                        <th>Plant : </th>
                        <td>
                            <asp:TextBox ID="txtPlant" runat="server" Width="7em" MaxLength="5" ReadOnly="true"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Remind Pattern : </th>
                        <td>
                            <asp:DropDownList ID="ddlShowType" runat="server" Width="7.7em" AutoPostBack="true">
                                <asp:ListItem Value="0" Text="Constant">Constant</asp:ListItem>
                                <asp:ListItem Value="1" Text="Formula">Formula</asp:ListItem>
                            </asp:DropDownList></td>
                    </tr>
                    <tr>
                        <th>Remind Coefficient1 : </th>
                        <td>
                            <asp:TextBox ID="txtFirstRem" runat="server" Width="7em" MaxLength="5"></asp:TextBox>
                        </td>
                        <th>
                            <label>Constant1:</label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtConstant1" runat="server" Width="7em" MaxLength="5"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Remind Coefficient2 : </th>
                        <td>
                            <asp:TextBox ID="txtSecondRem" runat="server" Width="7em" MaxLength="5"></asp:TextBox>
                        </td>
                        <th>
                            <label>Constant2:</label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtConstant2" runat="server" Width="7em" MaxLength="5"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Remind Coefficient3 : </th>
                        <td>
                            <asp:TextBox ID="txtThirdRem" runat="server" Width="7em" MaxLength="5"></asp:TextBox>
                        </td>
                        <th>
                            <label>Constant3:</label>
                        </th>
                        <td>
                            <asp:TextBox ID="txtConstant3" runat="server" Width="7em" MaxLength="5"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <%--  <asp:HiddenField ID="Mode" runat="server" value="" />--%>
                <%--		<asp:HiddenField ID="UpdateDate" runat="server" />--%>
                <asp:HiddenField ID="Action" runat="server" Value="Save" />
                <div class="btns">
                    <asp:Button ID="Save" runat="server" Text="Save" />
                </div>
            </div>
        </div>
        <!-- Main Content Area END -->

        <!-- Footer -->
        <!--#include virtual="./Footer.html" -->
        <!-- Footer END -->

        <script language ="javascript" type="text/javascript">
            //function Search_onclick() {
            //    var UserID = encodeURIComponent(document.getElementById('UserID').value);
            //    popup('./UserSelect.aspx?UserID=' + UserID);
            //    return false;
            //}
            //function selectDpList(dp) {
            //    var sIndex = dp.selectedIndex;
            //    if (sIndex == 0) {

            //    }
            //}
            //小数
            function IntoDecimal() {
                alert("Please enter decimal!");
            }
            //整数
            function IntoInteger() {
                alert("Please enter an integer!");
            }
            //数字
            function IntoNumber(){
                alert("The format you entered is not correct. Please enter a number!");
            }
        </script>
    </form>
</body>
</html>

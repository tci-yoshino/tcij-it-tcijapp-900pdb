<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserList.aspx.vb" Inherits="Purchase.UserList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>
    <form id="ListForm" runat="server">

    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"><a href="./UserSetting.aspx">New User</a>&nbsp;&nbsp;</div>

        <h3>User</h3>

        <p><asp:LinkButton ID="Download" runat="server" PostBackUrl="UserList.aspx?Action=Download">Download</asp:LinkButton></p>

        <div class="list">
            <p class="attention"></p>

            <asp:ListView ID="UserList" runat="server" DataSourceID="SrcUser" DataKeyNames="UserID">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server" style="width:8%">Location</th>
                            <th runat="server" style="width:20%">Account</th>
                            <th runat="server" style="width:25%">Surname</th>
                            <th runat="server" style="width:25%">Given Name</th>
                            <th runat="server" style="width:10%">Role</th>
                            <th runat="server" style="width:8%">is Admin</th>
                            <th style="width:4%"></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <EmptyDataTemplate>
                   <h3 style="font-style:italic"><% =Purchase.Common.ERR_NO_MATCH_FOUND%></h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr>
                        <td class = "<%# Eval("isDisabled_CSS") %>"><asp:Label ID="LocationLabel" runat="server" Text='<%# Eval("LocationName") %>' /></td>
                        <td class = "<%# Eval("isDisabled_CSS") %>"><asp:Label ID="AccountNameLabel" runat="server" Text='<%# Eval("AccountName") %>' /></td>
                        <td class = "<%# Eval("isDisabled_CSS") %>"><asp:Label ID="SurnameLabel" runat="server" Text='<%# Eval("Surname") %>' /></td>
                        <td class = "<%# Eval("isDisabled_CSS") %>"><asp:Label ID="GivenNameLabel" runat="server" Text='<%# Eval("GivenName") %>' /></td>
                        <td class = "<%# Eval("isDisabled_CSS") %>"><asp:Label ID="RoleCodeLabel" runat="server" Text='<%# Eval("RoleCode") %>' /></td>
                        <td class = "<%# Eval("isDisabled_CSS") %>"><asp:Label ID="isAdminLabel" runat="server" Text='<%# Eval("isAdmin") %>' /></td>
                        <td class = "<%# Eval("isDisabled_CSS") %>"><asp:HyperLink ID="Edit" runat="server" NavigateUrl='<%# Eval("Url") %>'>Edit</asp:HyperLink></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcUser" runat="server" 
    ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>">
    </asp:SqlDataSource>
    </form>
    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

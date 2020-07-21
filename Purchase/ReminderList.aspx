<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ReminderList.aspx.vb" Inherits="Purchase.ReminderList" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <%--<title></title>--%>
    <title>Reminder</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>
     <form id="ListForm" runat="server">

    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"><a href="./ReminderSetting.aspx">Reminder</a>&nbsp;&nbsp;</div>

        <h3>Reminder</h3>

        <%--<p><asp:LinkButton ID="Download" runat="server" PostBackUrl="UserList.aspx?Action=Download">Download</asp:LinkButton></p>--%>

        <div class="list">
            <p class="attention"></p>

            <asp:ListView ID="ReminedList" runat="server" DataSourceID="SrcRemined" DataKeyNames="SupplyingPlant">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server" style="width:16%">Plant</th>
                            <th runat="server" style="width:17%">ShowType</th>
                            <th runat="server" style="width:16%">FirstRem</th>
                            <th runat="server" style="width:16%">SecondRem</th>
                            <th runat="server" style="width:16%">ThirdRem</th>
                            <th style="width:17%"></th>
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
                        <td><asp:Label ID="PlantLabel" runat="server" Text='<%# Eval("SupplyingPlant") %>' /></td>
                        <td><asp:Label ID="ShowTypeLabel" runat="server" Text='<%# Eval("ShowType") %>' /></td>
                        <td><asp:Label ID="FirstRemLabel" runat="server" Text='<%# Eval("FirstRem") %>' /></td>
                        <td><asp:Label ID="SecondRemLabel" runat="server" Text='<%# Eval("SecondRem") %>' /></td>
                        <td><asp:Label ID="ThirdRemLabel" runat="server" Text='<%# Eval("ThirdRem") %>' /></td>
                        <td><asp:HyperLink ID="Edit" runat="server" NavigateUrl='<%# Eval("Url") %>'>Edit</asp:HyperLink></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcRemined" runat="server" 
    ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>">
    </asp:SqlDataSource>
    </form>
</body>
</html>

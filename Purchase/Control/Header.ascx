<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="Header.ascx.vb" Inherits="Purchase.Header" %>

<link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
<!-- Header -->
<div id="header">
    <table>
        <tr>
            <td><a href="./MyTask.aspx" target="main"><img width="60" height="45" border="0" src="./Image/Logo.gif" alt="Purchase DB" /></a></td>
            <td><h1>Purchase DB</h1></td>
        </tr>
    </table>
    <p><strong>Logged in as : </strong><asp:Label ID="UserName" runat="server" Text=""><%=Session("UserName") %></asp:Label><strong class="indent">Location : </strong><asp:Label ID="LocationName" runat="server" Text=""><%=Session("LocationName") %></asp:Label></p>
</div><!-- Header END -->

<!-- Navigation -->
<asp:Panel ID="mainMenuPanel" runat="server" Visible="False">
    <div id="navi">
        <asp:ListView ID="mainMenu" runat="server" Visible="False">
            <LayoutTemplate>
                <ul id="itemPlaceholderContainer" runat="server" >
                    <li runat="server" id="itemPlaceholder" />
                </ul>
            </LayoutTemplate>
            <ItemTemplate>
                <li class='<%#Eval("CSS")%>'><asp:HyperLink ID="MainMenu" runat="server" NavigateUrl='<%#Eval("NavigateUrl")%>'><%#Eval("MenuName")%></asp:HyperLink>
                </li>
            </ItemTemplate>
        </asp:ListView>
    </div>
</asp:Panel>
<asp:Panel ID="subMenuPanel" runat="server" Visible="False">
    <div id="subNavi">
        <asp:ListView ID="subMenu" runat="server" Visible="False">
            <LayoutTemplate>
                <span runat="server" id="itemPlaceholder"></span>
            </LayoutTemplate>
            <ItemTemplate>
                <asp:HyperLink class='<%#Eval("CSS")%>' ID="SubMenu" runat="server" NavigateUrl='<%#Eval("NavigateUrl")%>'><%#Eval("MenuName")%></asp:HyperLink>
            </ItemTemplate>
            <ItemSeparatorTemplate>|</ItemSeparatorTemplate>
        </asp:ListView>
    </div>
</asp:Panel>
<!-- Navigation -->

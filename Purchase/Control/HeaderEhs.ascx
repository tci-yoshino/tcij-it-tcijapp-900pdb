<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="HeaderEhs.ascx.vb" Inherits="Purchase.HeaderEhs" %>

<link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
<!-- HeaderEhs -->
<div id="ehs">
    <strong >EHS Reference :</strong>
    <asp:ListView ID="ehsList" runat="server">
        <LayoutTemplate>
            <ul id="itemPlaceholderContainer" runat="server">
                <li id="itemPlaceholder" runat="server"></li>
            </ul>
        </LayoutTemplate>
        <EmptyDataTemplate>
<!--        <h3 style="font-style: italic"><% =Purchase.Common.MSG_NO_DATA_FOUND%></h3>-->
        </EmptyDataTemplate>
        <ItemTemplate>
            <li>
                <strong><asp:Label ID="itemName" runat="server" Text='<%# Eval("itemName") + " : " %>' /></strong>
                <span class="value"><asp:Label ID="ItemValue" runat="server" Text='<%# Eval("ItemValue") %>' /></span>
            </li>
        </ItemTemplate>
    </asp:ListView>
</div>
<!-- HeaderEhs End -->

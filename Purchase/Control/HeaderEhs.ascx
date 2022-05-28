<%@ Control Language="vb" AutoEventWireup="true" CodeBehind="HeaderEhs.ascx.vb" Inherits="Purchase.HeaderEhs" %>

<!-- HeaderEhs -->
<div id="ehs">
    <p><strong>EHS Reference :</strong></p>
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
                <span class="name"><asp:Label ID="itemName" runat="server" Text='<%# Eval("itemName") + " : " %>' /></span>
                <span class="value"><asp:Label ID="ItemValue" runat="server" Text='<%# Eval("ItemValue") %>' /></span>
            </li>
        </ItemTemplate>
    </asp:ListView>
</div>
<!-- HeaderEhs End -->

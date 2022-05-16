<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PurchaseGroup.aspx.vb" Inherits="Purchase.PurchaseGroup" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>
    <!-- Header -->
    <commonUC:Header ID="HeaderMenu" runat="server" />
    <!-- Header End -->
    <form id="ListForm" runat="server">
        <div id="content">
            <h3>User</h3>
            <div class="list">
                <p class="attention"></p>
                <asp:ListView ID="UserList" runat="server">
                    <LayoutTemplate>
                        <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr runat="server" style="">
                                <th runat="server" style="width:7%">Location</th>
                                <th runat="server" style="width:15%">Account</th>
                                <th runat="server" style="width:10%">Surname</th>
                                <th runat="server" style="width:10%">Given Name</th>
                                <th runat="server" style="width:10%">SAP Pur. Grp</th>
                                <th runat="server" style="width:20%">Storage Locations</th>
                                <th runat="server" style="width:10%">RFQ Correspondence<br />Editable</th>
                                <th runat="server" style="width:10%">MMSTA Invalidation<br />Editable</th>
                                <th></th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <EmptyDataTemplate>
                        <h3 style="font-style: italic"><% =Purchase.Common.ERR_NO_MATCH_FOUND%></h3>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <tr>
                            <td class="<%# iif(Eval("isDisabled")="1","disable","") %>">
                                <asp:Label ID="LocationLabel" runat="server" Text='<%# Eval("LocationName") %>' /></td>
                            <td class="<%# iif(Eval("isDisabled")="1","disable","") %>">
                                <asp:Label ID="AccountNameLabel" runat="server" Text='<%# Eval("AccountName") %>' /></td>
                            <td class="<%# iif(Eval("isDisabled")="1","disable","") %>">
                                <asp:Label ID="SurnameLabel" runat="server" Text='<%# Eval("Surname") %>' /></td>
                            <td class="<%# iif(Eval("isDisabled")="1","disable","") %>">
                                <asp:Label ID="GivenNameLabel" runat="server" Text='<%# Eval("GivenName") %>' /></td>
                            <td class="<%# iif(Eval("isDisabled")="1","disable","") %>">
                                <asp:Label ID="R3PurchasingGroup" runat="server" Text='<%# Eval("R3PurchasingGroup") %>' /></td>
                            <td class="<%# iif(Eval("isDisabled")="1","disable","") %>" style="white-space:normal">
                                <asp:Label ID="Storages" runat="server" Text='<%# GetStorageLocations(Eval("UserID"))  %>' /></td>
                            <td class="<%# iif(Eval("isDisabled")="1","disable","") %>">
                                <asp:Label ID="RFQCorrespondenceEditable" runat="server" Text='<%# Eval("RFQCorrespondenceEditable") %>' /></td>
                            <td class="<%# iif(Eval("isDisabled")="1","disable","") %>">
                                <asp:Label ID="MMSTAInvalidationEditable" runat="server" Text='<%# Eval("MMSTAInvalidationEditable") %>' /></td>
                            <td class="<%# iif(Eval("isDisabled")="1","disable","") %>">
                                <asp:HyperLink ID="Edit" runat="server" NavigateUrl='<%# Eval("Url") %>'>Edit</asp:HyperLink></td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div>
    </form>
</body>
</html>

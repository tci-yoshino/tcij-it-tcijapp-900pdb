<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Setting.aspx.vb" Inherits="Purchase.Setting" %>

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
    <form id="form1" runat="server">
        <div>
            <div id="content">
                <div class="tabs"></div>
                <h3>Setting</h3>
                <div class="main">
                    <ul>
                        <li id="CountryListli" runat="server"><a href="./CountryList.aspx">Country</a></li>
                        <li id="SupplierSearchli" runat="server"><a href="./SupplierSearch.aspx">Supplier</a></li>
                        <li id="ProductSearchli" runat="server"><a href="./ProductSearch.aspx">Product</a></li>
                        <li id="PurchaseGroupli" runat="server"><a href="./PurchaseGroup.aspx">Purchasing Group</a></li>
                        <li id="ReminderListli" runat="server"><a href="./ReminderList.aspx">Reminder</a></li>
                        <%--<li id="UserListli" runat="server"><a href="./UserList.aspx">User</a></li>--%>
                    </ul>
                </div>

            </div>
        </div>
    </form>
</body>
</html>

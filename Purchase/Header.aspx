<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="Header.aspx.vb" Inherits="Purchase.Header" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
</head>
<body>
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

    <!-- Global Navigation -->
    <div id="navi">
        <ul>
            <li id="home"><a href="./MyTask.aspx" target="main">Home</a></li>
            <li id="product"><a href="./RFQSearchByProduct.aspx" target="main">Product</a></li>
            <li id="supplier"><a href="./RFQSearchBySupplier.aspx" target="main">Supplier</a></li>
            <li id="rfq_status"><a href="./RFQStatus.aspx" target="main">RFQ Status</a></li>
            <li id="po_status"><a href="./POStatus.aspx" target="main">PO Status</a></li>
            <li id="setting"><a href="./Setting.html" target="main">Setting</a></li>
        </ul>
    </div><!-- Global Navigation END -->
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="POListByRFQ.aspx.vb" Inherits="Purchase.POListByRFQ" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>
    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"></div>

        <h3>PO List</h3>

        <div class="list">
            <asp:ListView ID="POList" runat="server" DataSourceID="SrcPO">
                <LayoutTemplate>
                    <div ID="itemPlaceholderContainer" runat="server">
                        <div ID="itemPlaceholder" runat="server">
                        </div>
                    </div>
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <h3 style="font-style:italic">No data found.</h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <table>
                        <tr>
                            <th class="subhead" colspan="2">PO Number : <asp:HyperLink ID="POUpdate" runat="server" NavigateUrl="./POUpdate.aspx"><asp:label id="PONumber" runat="server" Text=""></asp:label></asp:HyperLink><span class="indent"></span></th>
                            <th class="subhead" colspan="4"><asp:label id="POStatusChangeDate" runat="server" Text=""></asp:label><span class="indent"><asp:label id="POStatus" runat="server" Text=""></asp:label></span></th>
                        </tr>
                        <tr>
                            <th style="width:17%">Product Number / Name</th>
                            <td style="width:33%"><asp:label id="ProductNumber" runat="server" Text=""></asp:label><span class="indent"><asp:label id="ProductName" runat="server" Text=""></asp:label></span></td>
                            <th style="width:10%">PO Date</th>
                            <td style="width:12%"><asp:label id="PODate" runat="server" Text=""></asp:label></td>
                            <th style="width:10%">PO-User</th>
                            <td style="width:18%"><asp:label id="POUser" runat="server" Text=""></asp:label><span class="indent">(<asp:label id="POLocation" runat="server" Text=""></asp:label>)</span></td>
                        </tr>
                        <tr>
                            <th>Supplier Name</th>
                            <td><asp:label id="SupplierName" runat="server" Text=""></asp:label></td>
                            <th>Maker Name</th>
                            <td colspan="3"><asp:label id="MakerName" runat="server" Text=""></asp:label></td>
                        </tr>
                        <tr>
                            <th>Delivery Date</th>
                            <td><asp:label id="DeliveryDate" runat="server" Text=""></asp:label></td>
                            <th>Order Quantity</th>
                            <td><asp:label id="OrderQuantity" runat="server" Text=""></asp:label> <asp:label id="OrderUnit" runat="server" Text=""></asp:label> x <asp:label id="OrderPiece" runat="server" Text=""></asp:label></td>
                            <th>Price</th>
                            <td><asp:label id="Currency" runat="server" Text=""></asp:label> <asp:label id="UnitPrice" runat="server" Text=""></asp:label> / <asp:label id="PerQuantity" runat="server" Text=""></asp:label> <asp:label id="PerUnit" runat="server" Text=""></asp:label></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcPO" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MyTask.aspx.vb" Inherits="Purchase.MyTask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>
    <!-- Sub Navigation -->
    <div id="subNavi">
        <a href="./MyTask.aspx" class="current">My Tasks</a> | <a href="./RequestedTask.aspx">Requested Tasks</a> | <a href="./UnassignedTask.aspx">Unassigned Tasks</a>
    </div><!-- Sub Navigation END -->

    <!-- Main Content Area -->
    <div id="content">

        <div class="main switch">
            <form ID="SwitchForm" runat="server">
                <table style="margin-bottom:0">
                    <tr>
                        <th>User : </th>
                        <td>
                            <asp:DropDownList ID="User" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td><asp:Button ID="Switch" runat="server" Text="Switch" /></td>
                    </tr>
                </table>
            </form>
        </div>

        <h3>RFQ</h3>

        <div class="list">
            <asp:Repeater ID="RFQList" runat="server" DataSourceID="SrcRFQ">
                <ItemTemplate>
                    <table>
                        <tr>
                            <th class="subhead" colspan="2">RFQ Reference Number : <asp:HyperLink ID="RFQUpdate" runat="server"><asp:label id="RFQNumber" runat="server" Text=""></asp:label></asp:HyperLink><span class="indent"><em><asp:label id="RFQCorrespondence" runat="server" Text=""></asp:label></em></span></th>
                            <th class="subhead" colspan="4"><asp:label id="RFQStatusChangeDate" runat="server" Text=""></asp:label><span class="indent"><asp:label id="RFQStatus" runat="server" Text=""></asp:label></span></th>
                        </tr>
                        <tr>
                            <th style="width:17%">Product Number / Name</th>
                            <td style="width:33%"><asp:label id="RFQProductNumber" runat="server" Text=""></asp:label><span class="indent"><asp:label id="RFQProductName" runat="server" Text=""></asp:label></span></td>
                            <th style="width:10%">Purpose</th>
                            <td style="width:12%"><asp:label id="RFQPurpose" runat="server" Text=""></asp:label></td>
                            <th style="width:10%">Enq-User</th>
                            <td style="width:18%"><asp:label id="RFQEnqUser" runat="server" Text=""></asp:label><span class="indent">(<asp:label id="RFQEnqLocation" runat="server" Text=""></asp:label>)</span></td>
                        </tr>
                        <tr>
                            <th>Supplier Name</th>
                            <td><asp:label id="RFQSupplierName" runat="server" Text=""></asp:label></td>
                            <th>Maker Name</th>
                            <td colspan="3"><asp:label id="RFQMakerName" runat="server" Text=""></asp:label></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:Repeater>
        </div>

        <hr />

        <h3>PO</h3>

        <div class="list">
            <asp:Repeater ID="POList" runat="server" DataSourceID="SrcPO">
                <ItemTemplate>
                    <table>
                        <tr>
                            <th class="subhead" colspan="2">PO Number : <asp:HyperLink ID="POUpdate" runat="server"><asp:label id="PONumber" runat="server" Text=""></asp:label></asp:HyperLink><span class="indent"><em><asp:label id="POCorrespondence" runat="server" Text=""></asp:label></em></span></th>
                            <th class="subhead" colspan="4"><asp:label id="POStatusChangeDate" runat="server" Text=""></asp:label><span class="indent"><asp:label id="POStatus" runat="server" Text=""></asp:label></span></th>
                        </tr>
                        <tr>
                            <th style="width:17%">Product Number / Name</th>
                            <td style="width:33%"><asp:label id="POProductNumber" runat="server" Text=""></asp:label><span class="indent"><asp:label id="POProductName" runat="server" Text=""></asp:label></span></td>
                            <th style="width:10%">PO Date</th>
                            <td style="width:12%"><asp:label id="PODate" runat="server" Text=""></asp:label></td>
                            <th style="width:10%">PO-User</th>
                            <td style="width:18%"><asp:label id="POUser" runat="server" Text=""></asp:label><span class="indent">(<asp:label id="POLocation" runat="server" Text=""></asp:label>)</span></td>
                        </tr>
                        <tr>
                            <th>Supplier Name</th>
                            <td><asp:label id="POSupplierName" runat="server" Text=""></asp:label></td>
                            <th>Maker Name</th>
                            <td colspan="3"><asp:label id="POMakerName" runat="server" Text=""></asp:label></td>
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
                <AlternatingItemTemplate>
                    <table class="child">
                        <tr>
                            <th class="subhead" colspan="2">Chi-PO Number : <asp:HyperLink ID="POUpdate" runat="server"><asp:label id="PONumber" runat="server" Text=""></asp:label></asp:HyperLink><span class="indent"><em><asp:label id="POCorrespondence" runat="server" Text=""></asp:label></em></span></th>
                            <th class="subhead" colspan="4"><asp:label id="POStatusChangeDate" runat="server" Text=""></asp:label><span class="indent"><asp:label id="POStatus" runat="server" Text=""></asp:label></span></th>
                        </tr>
                        <tr>
                            <th style="width:17%">Product Number / Name</th>
                            <td style="width:33%"><asp:label id="POProductNumber" runat="server" Text=""></asp:label><span class="indent"><asp:label id="POProductName" runat="server" Text=""></asp:label></span></td>
                            <th style="width:10%">PO Date</th>
                            <td style="width:12%"><asp:label id="PODate" runat="server" Text=""></asp:label></td>
                            <th style="width:10%">PO-User</th>
                            <td style="width:18%"><asp:label id="POUser" runat="server" Text=""></asp:label><span class="indent">(<asp:label id="POLocation" runat="server" Text=""></asp:label>)</span></td>
                        </tr>
                        <tr>
                            <th>Supplier Name</th>
                            <td><asp:label id="POSupplierName" runat="server" Text=""></asp:label></td>
                            <th>Maker Name</th>
                            <td colspan="3"><asp:label id="POMakerName" runat="server" Text=""></asp:label></td>
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
                </AlternatingItemTemplate>
            </asp:Repeater>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcRFQ" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SrcPO" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="~/Footer.html" --><!-- Footer END -->
</body>
</html>

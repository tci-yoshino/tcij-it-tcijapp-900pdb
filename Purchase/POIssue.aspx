<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="POIssue.aspx.vb" Inherits="Purchase.POIssue" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>
    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"></div>

        <h3>PO Issue</h3>

        <form id="POForm" runat="server">
            <div class="main">
                <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
            
                <table class="left">
                    <tr>
                        <th>RFQ Reference Number : </th>
                        <td><asp:Label ID="RFQNumber" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>R/3 PO Number : </th>
                        <td><asp:TextBox ID="R3PONumber" runat="server" Width="10em" MaxLength="10"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>PO Date <span class="required">*</span> : </th>
                        <td><asp:TextBox ID="PODate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                    </tr>
                    <tr>
                        <th>PO-User <span class="required">*</span> : </th>
                        <td>
                            <asp:DropDownList ID="POUser" runat="server">
                            </asp:DropDownList>
                            (<asp:Label ID="POLocation" runat="server" Text=""></asp:Label>)
                        </td>
                    </tr>
                    <tr>
                        <th>Product Number / Name : </th>
                        <td><asp:Label ID="ProductNumber" runat="server" Text=""></asp:Label>
                        <span class="indent"><asp:Label ID="ProductName" runat="server" Text=""></asp:Label></span></td>
                    </tr>
                    <tr>
                        <th>Order Quantity <span class="required">*</span> : </th>
                        <td>
                            <asp:TextBox ID="OrderQuantity" runat="server" Width="5em" MaxLength="10"></asp:TextBox>
                            <asp:DropDownList ID="OrderUnit" runat="server">
                            </asp:DropDownList>
                            x <asp:TextBox ID="OrderPiece" runat="server" Width="3em" MaxLength="10"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Delivery Date : </th>
                        <td><asp:TextBox ID="DeliveryDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                    </tr>
                    <tr>
                        <th>Price : </th>
                        <td><asp:Label ID="Currency" runat="server" Text=""></asp:Label> <asp:Label ID="UnitPrice" runat="server" Text=""></asp:Label> / <asp:Label ID="PerQuantity" runat="server" Text=""></asp:Label> <asp:Label ID="PerUnit" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>R/3 Supplier Code : </th>
                        <td><asp:Label ID="R3SupplierCode" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>R/3 Supplier Name : </th>
                        <td><asp:Label ID="R3SupplierName" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>R/3 Maker Code : </th>
                        <td><asp:Label ID="R3MakerCode" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>R/3 Maker Name : </th>
                        <td><asp:Label ID="R3MakerName" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Payment Terms : </th>
                        <td><asp:Label ID="PaymentTerm" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Incoterms : </th>
                        <td><asp:Label ID="Incoterms" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Terms of Delivery : </th>
                        <td><asp:Label ID="DeliveryTerm" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Purpose : </th>
                        <td>
                            <asp:DropDownList ID="Purpose" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Raw Material for : </th>
                        <td><asp:TextBox ID="RawMaterialFor" runat="server" Width="15em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Requested By : </th>
                        <td><asp:TextBox ID="RequestedBy" runat="server" Width="15em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Supplier Item Number : </th>
                        <td><asp:TextBox ID="SupplierItemNumber" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Supplier Lot Number : </th>
                        <td><asp:TextBox ID="SupplierLotNumber" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                    </tr>
                </table>

                <table>
                    <tr>
                        <th>Due Date : </th>
                        <td><asp:TextBox ID="DueDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                    </tr>
                    <tr>
                        <th>Goods Arrived Date : </th>
                        <td><asp:TextBox ID="GoodsArrivedDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                    </tr>
                    <tr>
                        <th>TCI Lot Number : </th>
                        <td><asp:TextBox ID="LotNumber" runat="server" Width="10em" MaxLength="10"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Commercial Invoice Received Date : </th>
                        <td><asp:TextBox ID="InvoceReceivedDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                    </tr>
                    <tr>
                        <th>Import Custom Clearance Date : </th>
                        <td><asp:TextBox ID="ImportCustomClearanceDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                    </tr>
                    <tr>
                        <th>QM Starting Date : </th>
                        <td><asp:TextBox ID="QMStartingDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                    </tr>
                    <tr>
                        <th>QM Finish Date : </th>
                        <td><asp:TextBox ID="QMFinishDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                    </tr>
                    <tr>
                        <th>QM Result : </th>
                        <td><asp:TextBox ID="QMResult" runat="server" Width="15em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Chi-PO Request Quantity : </th>
                        <td><asp:TextBox ID="RequestQuantity" runat="server" Width="15em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Scheduled Export Date : </th>
                        <td><asp:TextBox ID="ScheduledExportDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                    </tr>
                    <tr>
                        <th>Purchasing Requisition Number : </th>
                        <td><asp:TextBox ID="PurchasingRequisitionNumber" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                    </tr>
                </table>

                <div class="btns">
                    <asp:Button ID="Issue" runat="server" Text="Issue" />
                </div>
            </div>
        </form>

    </div><!-- Main Content Area END -->

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

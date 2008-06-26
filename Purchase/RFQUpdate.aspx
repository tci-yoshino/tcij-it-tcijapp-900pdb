﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQUpdate.aspx.vb" Inherits="Purchase.RFQUpdate" %>

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
        <div class="tabs"><a href="#" onclick="popup('2-4-2.shtml')">RFQ Correspondence / History</a></div>

        <h3>Quotation Reply</h3>

        <form id="RFQForm" runat="server">
            <div class="main">
                <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
            
                <table class="left">
                    <tr>
                        <th>RFQ Reference Number : </th><td><asp:Label ID="RFQNumber" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Current Status : </th><td><asp:Label ID="CurrentRFQStatus" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Product Number / Name : </th><td><asp:Label ID="ProductNumber" runat="server" Text=""></asp:Label><span class="indent"><asp:Label ID="ProductName" runat="server" Text=""></asp:Label></span></td>
                    </tr>
					<tr>
						<th>Supplier Code <span class="required">*</span> : </th>
						<td>
						    <asp:TextBox ID="SupplierCode" runat="server" Width="7em" MaxLength="10" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						    <asp:ImageButton ID="SupplierSelect" runat="server" ImageUrl="./Image/Search.gif" CssClass="magnify" OnClientClick="popup('./SupplierSelect.aspx')" />
						</td>
					</tr>
					<tr>
						<th>R/3 Supplier Code : </th>
						<td><asp:TextBox ID="R3SupplierCode" runat="server" Width="7em" MaxLength="10" ReadOnly="true" CssClass="readonly"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Supplier Name / Country <span class="required">*</span> : </th>
						<td>
						    <asp:TextBox ID="SupplierName" runat="server" Width="21em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						    <asp:TextBox ID="SupplierCountry" runat="server" Width="4em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						</td>
					</tr>
                    <tr>
                        <th>Supplier Contact Person : </th>
                        <td><asp:TextBox ID="SupplierContactPerson" runat="server" Width="21em" MaxLength="255"></asp:TextBox></td>
                    </tr>
					<tr>
						<th>Maker Code : </th>
						<td>
						    <asp:TextBox ID="MakerCode" runat="server" Width="7em" MaxLength="10" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						    <asp:ImageButton ID="MakerSelect" runat="server" ImageUrl="./Image/Search.gif" CssClass="magnify" OnClientClick="popup('./MakerSelect.aspx')" />
						</td>
					</tr>
					<tr>
						<th>Maker Name / Country : </th>
						<td>
						    <asp:TextBox ID="MakerName" runat="server" Width="21em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						    <asp:TextBox ID="MakerCountry" runat="server" Width="4em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						</td>
					</tr>
                    <tr>
                        <th>Supplier Item Name : </th>
                        <td><asp:TextBox ID="SupplierItemName" runat="server" Width="21em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Payment Terms : </th>
                        <td>
                            <asp:DropDownList ID="PaymentTerm" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Handling Fee / Shipment Cost : </th>
                        <td>
                            <asp:DropDownList ID="ShippingHandlingCurrency" runat="server">
                            </asp:DropDownList>
                            <asp:TextBox ID="TextBox1" runat="server" Width="5em" MaxLength="21" CssClass="number"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <table>
                    <tr>
                        <th>Purpose : </th><td><asp:Label ID="Purpose" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Required Purity : </th><td><asp:Label ID="RequiredPurity" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Required QM Method : </th><td><asp:Label ID="RequiredQMMethod" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Required Specification : </th><td><asp:Label ID="RequiredSpecification" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Spec Sheet : </th><td><asp:CheckBox ID="SpecSheet" runat="server" Text="yes" /></td>
                    </tr>
                    <tr>
                        <th>Specification : </th>
                        <td><asp:TextBox ID="Specification" runat="server" Columns="50" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Enq-User : </th><td><asp:Label ID="EnqUser" runat="server" Text=""></asp:Label><span class="indent">(<asp:Label ID="EnqLocation" runat="server" Text=""></asp:Label>)</span></td>
                    </tr>
                    <tr>
                        <th>Quo-User <span class="required">*</span> : </th>
                        <td>
                            <asp:DropDownList ID="QuoUser" runat="server">
                            </asp:DropDownList>
                            (<asp:Label ID="QuoLocation" runat="server" Text=""></asp:Label>)
                        </td>
                    </tr>
                    <tr>
                        <th>Comment : </th>
                        <td><asp:TextBox ID="Comment" runat="server" Columns="50" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                </table>
            </div>

            <div class="list">
                <table>
                    <tr>
                        <th style="width:3%" rowspan="2">No.</th>
                        <th>Enq-Quantity</th>
                        <th>Currency</th>
                        <th>Price</th>
                        <th>Quo-Per</th>
                        <th>Quo-Unit</th>
                        <th>Lead Time</th>
                        <th>Supplier Item Number</th>
                        <th style="width:5%" rowspan="2">PO Issue</th>
                    </tr>
                    <tr>
                        <th>Incoterms</th>
                        <th colspan="2">Terms of Delivery</th>
                        <th>Purity</th>
                        <th>Method</th>
                        <th>Packing</th>
                        <th>Reason for "No Offer"</th>
                    </tr>
                    <tr>
                        <th rowspan="2">1</th>
                        <td>
                            <asp:Label ID="EnqQuantity_1" runat="server" Text=""></asp:Label> <asp:Label ID="EnqUnit_1" runat="server" Text=""></asp:Label> x <asp:Label ID="EnqPiece_1" runat="server" Text=""></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="Currency_1" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="UnitPrice_1" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox></td>
                        <td><asp:TextBox ID="QuoPer_1" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="QuoUnit_1" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="LeadTime_1" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="SupplierItemNumber_1" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                        <td rowspan="2"><asp:HyperLink ID="POIssue_1" runat="server" NavigateUrl="./POIssue.aspx">PO Issue</asp:HyperLink></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="Incoterms_1" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2"><asp:TextBox ID="DeliveryTerm_1" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Purity_1" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="QMMethod_1" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Packing_1" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="NoOfferReason_1" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th rowspan="2">2</th>
                        <td>
                            <asp:Label ID="EnqQuantity_2" runat="server" Text=""></asp:Label> <asp:Label ID="EnqUnit_2" runat="server" Text=""></asp:Label> x <asp:Label ID="EnqPiece_2" runat="server" Text=""></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="Currency_2" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="UnitPrice_2" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox></td>
                        <td><asp:TextBox ID="QuoPer_2" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="QuoUnit_2" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="LeadTime_2" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="SupplierItemNumber_2" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                        <td rowspan="2"><asp:HyperLink ID="POIssue_2" runat="server" NavigateUrl="./POIssue.aspx">PO Issue</asp:HyperLink></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="Incoterms_2" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2"><asp:TextBox ID="DeliveryTerm_2" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Purity_2" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="QMMethod_2" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Packing_2" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="NoOfferReason_2" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th rowspan="2">3</th>
                        <td>
                            <asp:Label ID="EnqQuantity_3" runat="server" Text=""></asp:Label> <asp:Label ID="EnqUnit_3" runat="server" Text=""></asp:Label> x <asp:Label ID="EnqPiece_3" runat="server" Text=""></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="Currency_3" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="UnitPrice_3" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox></td>
                        <td><asp:TextBox ID="QuoPer_3" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="QuoUnit_3" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="LeadTime_3" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="SupplierItemNumber_3" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                        <td rowspan="2"><asp:HyperLink ID="POIssue_3" runat="server" NavigateUrl="./POIssue.aspx">PO Issue</asp:HyperLink></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="Incoterms_3" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2"><asp:TextBox ID="DeliveryTerm_3" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Purity_3" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="QMMethod_3" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Packing_3" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="NoOfferReason_3" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th rowspan="2">4</th>
                        <td>
                            <asp:Label ID="EnqQuantity_4" runat="server" Text=""></asp:Label> <asp:Label ID="EnqUnit_4" runat="server" Text=""></asp:Label> x <asp:Label ID="EnqPiece_4" runat="server" Text=""></asp:Label></td>
                        <td>
                            <asp:DropDownList ID="Currency_4" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="UnitPrice_4" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox></td>
                        <td><asp:TextBox ID="QuoPer_4" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="QuoUnit_4" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="LeadTime_4" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="SupplierItemNumber_4" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                        <td rowspan="2"><asp:HyperLink ID="POIssue_4" runat="server" NavigateUrl="./POIssue.aspx">PO Issue</asp:HyperLink></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="Incoterms_4" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td colspan="2"><asp:TextBox ID="DeliveryTerm_4" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Purity_4" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="QMMethod_4" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Packing_4" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="NoOfferReason_4" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>

                <div class="btns">
                    <strong>Status : </strong>
                    <asp:DropDownList ID="RFQStatus" runat="server">
                    </asp:DropDownList>
                    <asp:Button ID="Update" runat="server" Text="Update" />
                    <span class="indent"></span>
                    <asp:Button ID="Close" runat="server" Text="Close" />
                </div>
            </div>
        </form>
    </div><!-- Main Content Area END -->

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>
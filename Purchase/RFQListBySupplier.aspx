<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQListBySupplier.aspx.vb" Inherits="Purchase.RFQListBySupplier" %>

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
        <div class="tabs">
            <a href="./RFQIssue.aspx?SupplierCode=<%=st_SupplierCode %>">RFQ Issue</a>
            | <a href="#" onclick="popup('./SupplierSetting.aspx?SupplierCode=<%=st_SupplierCode %>')">Supplier Setting</a>
        </div>

        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
        </div>
<%  If Not String.IsNullOrEmpty(st_SupplierCode) Then%>
        <h3><asp:Label ID="SupplierCode" runat="server" Text=""></asp:Label><span class="indent"><asp:Label ID="SupplierName" runat="server" Text=""></asp:Label></span></h3>

        <div class="main">
            <p>
                <strong>Address : </strong><asp:Label ID="Address1" runat="server" Text=""></asp:Label> <asp:Label ID="Address2" runat="server" Text=""></asp:Label> <asp:Label ID="Address3" runat="server" Text=""></asp:Label>
                <strong class="indent">Postal Code : </strong><asp:Label ID="PostalCode" runat="server" Text=""></asp:Label>
                <strong class="indent">Country : </strong><asp:Label ID="CountryName" runat="server" Text=""></asp:Label>
            </p>
            <p>
                <strong>Telephone : </strong><asp:Label ID="Telephone" runat="server" Text=""></asp:Label>
                <strong class="indent">Fax : </strong><asp:Label ID="Fax" runat="server" Text=""></asp:Label>
                <strong class="indent">E-mail : </strong><asp:HyperLink ID="EmailLink" runat="server" NavigateUrl=""><asp:Label ID="Email" runat="server" Text=""></asp:Label></asp:HyperLink>
                <strong class="indent">Website : </strong><asp:HyperLink ID="WebsiteLink" runat="server" NavigateUrl="" Target="_blank"><asp:Label ID="Website" runat="server" Text=""></asp:Label></asp:HyperLink>
            </p>
        </div>

        <hr />

        <div class="list">
            <asp:ListView ID="RFQHeaderList" runat="server" DataSourceID="SrcRFQHeader">
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
                            <th class="subhead" colspan="2">RFQ Reference Number : <a href="./RFQUpdate.aspx"><asp:Label ID="RFQNumber" runat="server" Text='<%#Eval("RFQNumber")%>'></asp:Label></a></th>
                            <th class="subhead" colspan="2">Quoted Date : <asp:Label ID="QuotedDate" runat="server" Text='<%#Eval("QuotedDate")%>'></asp:Label></th>
                            <th class="subhead" colspan="2"><asp:Label ID="RFQStatusChangeDate" runat="server" Text='<%#Eval("StatusChangeDate")%>'></asp:Label><span class="indent"><asp:Label ID="RFQStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label></span></th>
                        </tr>
                        <tr>
                            <th>Product Number / Name</th>
                            <td colspan="5"><asp:Label ID="ProductNumber" runat="server" Text='<%#Eval("ProductNumber")%>'></asp:Label><span class="indent"><asp:Label ID="ProductName" runat="server" Text='<%#Eval("ProductName")%>'></asp:Label></span></td>
                        </tr>
                        <tr>
                            <th>Supplier Name / Country</th>
                            <td colspan="3"><asp:Label ID="SupplierName" runat="server" Text='<%#Eval("SupplierName")%>'></asp:Label><span class="indent">(<asp:Label ID="SupplierCountry" runat="server" Text=''></asp:Label>)</span></td>
                            <th>Purpose</th>
                            <td><asp:Label ID="Purpose" runat="server" Text='<%#Eval("Purpose")%>'></asp:Label></td>
                        </tr>
                        <tr>
                            <th>Maker Name / Country</th>
                            <td colspan="3"><asp:Label ID="MakerName" runat="server" Text='<%#Eval("MakerName")%>'></asp:Label><span class="indent">(<asp:Label ID="MakerCountry" runat="server" Text=''></asp:Label>)</span></td>
                            <th>Supplier Item Name</th>
                            <td><asp:Label ID="SupplierItemName" runat="server" Text='<%#Eval("SupplierItemName")%>'></asp:Label></td>
                        </tr>
                        <tr>
                            <th style="width:20%">Handling Fee / Shipment Cost</th>
                            <td style="width:20%"><asp:Label ID="ShippingHandlingCurrency" runat="server" Text='<%#Eval("ShippingHandlingCurrencyCode")%>'></asp:Label> <asp:Label ID="ShippingHandlingFee" runat="server" Text='<%#Eval("ShippingHandlingFee")%>'></asp:Label></td>
                            <th style="width:10%">Enq-User / Location</th>
                            <td style="width:20%"><asp:Label ID="EnqUser" runat="server" Text='<%#Eval("EnqUserName")%>'></asp:Label><span class="indent">(<asp:Label ID="EnqLocation" runat="server" Text='<%#Eval("EnqLocationName")%>'></asp:Label>)</span></td>
                            <th style="width:10%">Quo-User / Location</th>
                            <td style="width:20%"><asp:Label ID="QuoUser" runat="server" Text='<%#Eval("QuoUserName")%>'></asp:Label><span class="indent">(<asp:Label ID="QuoLocation" runat="server" Text='<%#Eval("QuoLocationName")%>'></asp:Label>)</span></td>
                        </tr>
                        <tr>
                            <th>Comment</th>
                            <td colspan="5"><asp:Label ID="Comment" runat="server" Text='<%#If(IsDBNull(Eval("Comment")), "", Replace(Eval("Comment"), vbCrLf, "<br />"))%>'></asp:Label></td>
                        </tr>
                    </table>

                    <asp:ListView ID="RFQLineList" runat="server" DataSourceID="SrcRFQLine">
                        <LayoutTemplate>
                            <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                                <tr>
                                    <th id="Th1"  runat="server" style="width:7%">No.</th>
                                    <th id="Th2"  runat="server" style="width:10%">Enq-Quantity</th>
                                    <th id="Th3"  runat="server" style="width:10%">Currency</th>
                                    <th id="Th4"  runat="server" style="width:10%">Price</th>
                                    <th id="Th5"  runat="server" style="width:10%">Quo-Quantity</th>
                                    <th id="Th6"  runat="server" style="width:10%">Lead Time</th>
                                    <th id="Th7"  runat="server" style="width:10%">Packing</th>
                                    <th id="Th8"  runat="server" style="width:10%">Purity</th>
                                    <th id="Th9" runat="server" style="width:10%">Method</th>
                                    <th id="Th10" runat="server" style="width:3%">PO</th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <EmptyDataTemplate>
                            <h3 style="font-style:italic">No data found.</h3>
                        </EmptyDataTemplate>
                        <ItemTemplate>
                        <tr ID="itemPlaceholder" runat="server">
                            <th><asp:Label ID="Seq" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></th>
                            <td><asp:Label ID="EnqQuantity" runat="server" Text='<%#Eval("EnqQuantity") %>'></asp:Label> <asp:Label ID="EnqUnit" runat="server" Text='<%#Eval("EnqUnitCode") %>'></asp:Label> x <asp:Label ID="EnqPiece" runat="server" Text='<%#Eval("EnqPiece") %>'></asp:Label></td>
                            <td><asp:Label ID="Currency" runat="server" Text='<%#Eval("CurrencyCode") %>'></asp:Label></td>
                            <td class="number"><asp:Label ID="UnitPrice" runat="server" Text=''><%#Eval("UnitPrice").ToString().TrimEnd(ChrW(0))%></asp:Label></td>
                            <td class="number"><asp:Label ID="QuoPer" runat="server" Text='<%#Eval("QuoPer") %>'></asp:Label> <asp:Label ID="QuoUnit" runat="server" Text='<%#Eval("QuoUnitCode") %>'></asp:Label></td>
                            <td><asp:Label ID="LeadTime" runat="server" Text='<%#Eval("LeadTime") %>'></asp:Label></td>
                            <td><asp:Label ID="Packing" runat="server" Text='<%#Eval("Packing") %>'></asp:Label></td>
                            <td><asp:Label ID="Purity" runat="server" Text='<%#Eval("Purity") %>'></asp:Label></td>
                            <td><asp:Label ID="QMMethod" runat="server" Text='<%#Eval("QMMethod") %>'></asp:Label></td>
                            <td><asp:HyperLink ID="PO" runat="server" NavigateUrl='<%#If(IsDBNull(Eval("PO")), "", "./POListByRFQ.aspx?RFQNumber=" & Eval("RFQNumber"))%>'><%#If(IsDBNull(Eval("PO")), "", "PO")%></asp:HyperLink></td>
                        </tr>
                        </ItemTemplate>
                    </asp:ListView>
                    <asp:SqlDataSource ID="SrcRFQLine" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
<%  End If%>
    <asp:SqlDataSource ID="SrcRFQHeader" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQSelect.aspx.vb" Inherits="Purchase.RFQSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8; IE=EmulateIE9" />
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
    <%' JavaScript は VB ファイルの Set_JavaScript 関数で生成しています。 %>
</head>
<body>
    <!-- Header -->
    <commonUC:Header ID="HeaderMenu" runat="server" />
    <!-- Header End -->

    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"></div>

        <h3>RFQ Select</h3>
        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
        </div>

        <%If Not String.IsNullOrEmpty(st_ParPONumber) Then%>
        <div class="main">
            <table>
                <tr>
                    <th>Product Number / Name : </th>
                    <td><asp:Label ID="ProductNumber" runat="server" Text=""></asp:Label><span class="indent"><asp:Label ID="ProductName" runat="server" Text=""></asp:Label></span></td>
                </tr>
                <tr>
                    <th>Supplier Name / Country : </th>
                    <td><asp:Label ID="SupplierName" runat="server" Text=""></asp:Label><span class="indent">(<asp:Label ID="Country" runat="server" Text=""></asp:Label>)</span></td>
                </tr>
            </table>
        </div>

        <hr />

        <div class="list">
            <form id="SelectForm" runat="server">
                <asp:ListView ID="RFQHeaderList" runat="server" DataSourceID="SrcRFQHeader">
                    <LayoutTemplate>
                        <div ID="itemPlaceholderContainer" runat="server">
                            <div ID="itemPlaceholder" runat="server">
                            </div>
                        </div>
                    </LayoutTemplate>
                    <EmptyDataTemplate>
                        <h3 style="font-style:italic"><%=Purchase.Common.MSG_NO_DATA_FOUND%></h3>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <table runat="server">
                            <tr>
                                <th class="subhead" colspan="2">RFQ Reference Number : <asp:Label ID="RFQNumber" runat="server" Text='<%#Eval("RFQNumber")%>'></asp:Label></th>
                                <th class="subhead" colspan="2">Quoted Date : <asp:Label ID="QuotedDate" runat="server" Text='<%#if(isDBNull(Eval("QuotedDate")), "", Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("QuotedDate"), False, False)) %>'></asp:Label></th>
                            </tr>
                            <tr>
                                <th style="width:17%">Purpose</th>
                                <td style="width:33%"><asp:Label ID="Purpose" runat="server" Text='<%#Eval("Purpose") %>'></asp:Label></td>
                                <th style="width:17%">Handling Fee / Shipment Cost</th>
                                <td style="width:33%"><asp:Label ID="ShippingHandlingCurrency" runat="server" Text='<%#Eval("ShippingHandlingCurrencyCode") %>'></asp:Label> <asp:Label ID="ShippingHandlingFee" runat="server" Text='<%#Eval("ShippingHandlingFee","{0:G29}") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <th>Maker Name / Country</th>
                                <td><asp:Label ID="MakerName" runat="server" Text='<%#Eval("MakerName") %>'></asp:Label><span class="indent">(<asp:Label ID="MakerCountry" runat="server" Text='<%#Eval("MakerCountryName") %>'></asp:Label>)</span></td>
                                <th>Supplier Item Name</th>
                                <td><asp:Label ID="SupplierItemName" runat="server" Text='<%#Eval("SupplierItemName") %>'></asp:Label></td>
                            </tr>
                            <tr>
                                <th>Enq-User / Location</th>
                                <td><asp:Label ID="EnqUser" runat="server" Text='<%#Eval("EnqUserName") %>'></asp:Label><span class="indent">(<asp:Label ID="EnqLocation" runat="server" Text='<%#Eval("EnqLocationName") %>'></asp:Label>)</span></td>
                                <th>Quo-User / Location</th>
                                <td><asp:Label ID="QuoUser" runat="server" Text='<%#Eval("QuoUserName") %>'></asp:Label><span class="indent">(<asp:Label ID="QuoLocation" runat="server" Text='<%#Eval("QuoLocationName") %>'></asp:Label>)</span></td>
                            </tr>
                            <tr>
                                <th>Comment</th>
                                <td colspan="3"><asp:Label ID="Comment" runat="server" Text='<%#Replace(Eval("Comment").ToString(), vbCrLf, "<br />")%>'></asp:Label></td>
                            </tr>
                        </table>

                        <asp:ListView ID="RFQLineList" runat="server" DataSourceID="SrcRFQLine">
                            <LayoutTemplate>
                                <%' ここのテーブルIDを変更する場合はVBファイルの Set_JavaScript 関数も修正してください。 %>
                                <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                                    <tr>
                                        <th id="Th2" runat="server" style="width:5%">No.</th>
                                        <th id="Th3" runat="server" style="width:10%">Enq-Quantity</th>
                                        <th id="Th4" runat="server" style="width:8%">Currency</th>
                                        <th id="Th5" runat="server" style="width:8%">Price</th>
                                        <th id="Th6" runat="server" style="width:10%">Quo-Quantity</th>
                                        <th id="Th7" runat="server" style="width:10%">Lead Time</th>
                                        <th id="Th8" runat="server" style="width:10%">Packing</th>
                                        <th id="Th9" runat="server" style="width:10%">Purity</th>
                                        <th id="Th10" runat="server" style="width:10%">Method</th>
                                        <th id="Th11" runat="server" style="width:14%">Reason for "No Offer"</th>
                                    </tr>
                                    <tr ID="itemPlaceholder" runat="server">
                                    </tr>
                                </table>
                            </LayoutTemplate>
                            <EmptyDataTemplate></EmptyDataTemplate>
                            <ItemTemplate>
                              <tr onclick="<%#"goPOIssue('" & Eval("UnitPrice") & "','" & st_ParPONumber & "','" & Eval("RFQLineNumber") & "')"%>">
                                <th><asp:Label ID="Seq" runat="server" Text='<%#Container.DataItemIndex + 1 %>'></asp:Label></th>
                                <td><asp:Label ID="EnqQuantity" runat="server" Text='<%#Eval("EnqQuantity","{0:G29}") %>'></asp:Label> <asp:Label ID="EnqUnit" runat="server" Text='<%#Eval("EnqUnitCode") %>'></asp:Label> x <asp:Label ID="EnqPiece" runat="server" Text='<%#Eval("EnqPiece","{0:G29}") %>'></asp:Label></td>
                                <td><asp:Label ID="Currency" runat="server" Text='<%#Eval("CurrencyCode") %>'></asp:Label></td>
                                <td class="number"><asp:Label ID="UnitPrice" runat="server" Text='<%#Eval("UnitPrice","{0:G29}") %>'></asp:Label></td>
                                <td class="number"><asp:Label ID="QuoPer" runat="server" Text='<%#Eval("QuoPer","{0:G29}") %>'></asp:Label> <asp:Label ID="QuoUnit" runat="server" Text='<%#Eval("QuoUnitCode") %>'></asp:Label></td>
                                <td><asp:Label ID="LeadTime" runat="server" Text='<%#Eval("LeadTime") %>'></asp:Label></td>
                                <td><asp:Label ID="Packing" runat="server" Text='<%#Eval("Packing") %>'></asp:Label></td>
                                <td><asp:Label ID="Purity" runat="server" Text='<%#Eval("Purity") %>'></asp:Label></td>
                                <td><asp:Label ID="QMMethod" runat="server" Text='<%#Eval("QMMethod") %>'></asp:Label></td>
                                <td><asp:Label ID="NoOfferReason" runat="server" Text='<%#Eval("NoOfferReason") %>'></asp:Label></td>
                              </tr>
                            </ItemTemplate>
                        </asp:ListView>
                        <asp:SqlDataSource ID="SrcRFQLine" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                    </ItemTemplate>
                </asp:ListView>
                <asp:HiddenField runat="server" ID="ParPONumber" Value='<%=st_ParPONumber %>' />
                <asp:HiddenField runat="server" ID="Action" Value="Next" />
            </form>
        </div>
    </div><!-- Main Content Area END -->
    <%End If%>
    <asp:SqlDataSource ID="SrcRFQHeader" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQListBySupplier.aspx.vb" Inherits="Purchase.RFQListBySupplier" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
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
    <form id="RFQListForm" runat="server">
    <!-- Main Content Area --> 
    <div id="content">
        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
        </div>
<%  If Not String.IsNullOrEmpty(st_SupplierCode) Then%>
    <%If i_DataNum = 0 Then%>
        <h3 style="font-style:italic"><%=Purchase.Common.MSG_NO_DATA_FOUND%></h3>
    <%Else%>
        <div class="tabs">
            <a href="./RFQIssue.aspx?SupplierCode=<%Response.Write(st_SupplierCode)%>">RFQ Issue</a>
            | <a href="#" onclick="popup('./SupplierSetting.aspx?Action=Edit&Code=<%Response.Write(st_SupplierCode)%>')">Supplier Setting</a>
            | <a href="./ProductListBySupplier.aspx?Supplier=<%Response.Write(st_SupplierCode)%>" target="_blank">Product List</a>
        </div>

        <h3><asp:Label ID="SupplierCode" runat="server" Text=""></asp:Label><span class="indent"><asp:Label ID="SupplierName" runat="server" Text=""></asp:Label></span></h3>

        <div class="main">
            <p>
                <strong>Address : </strong><asp:Label ID="Address1" runat="server" Text=""></asp:Label> <asp:Label ID="Address2" runat="server" Text=""></asp:Label> <asp:Label ID="Address3" runat="server" Text=""></asp:Label>
                <strong class="indent">Postal Code : </strong><asp:Label ID="PostalCode" runat="server" Text=""></asp:Label>
                <strong class="indent">Country or Region : </strong><asp:Label ID="CountryName" runat="server" Text=""></asp:Label>
            </p>
            <p>
                <strong>Telephone : </strong><asp:Label ID="Telephone" runat="server" Text=""></asp:Label>
                <strong class="indent">Fax : </strong><asp:Label ID="Fax" runat="server" Text=""></asp:Label>
                <strong class="indent">E-mail : </strong><asp:HyperLink ID="EmailLink" runat="server" NavigateUrl=""><asp:Label ID="Email" runat="server" Text=""></asp:Label></asp:HyperLink>
                <strong class="indent">Website : </strong><asp:HyperLink ID="WebsiteLink" runat="server" NavigateUrl="" Target="_blank"><asp:Label ID="Website" runat="server" Text=""></asp:Label></asp:HyperLink>
                <strong class="indent">ECM : </strong><asp:HyperLink ID="SupplierInfoLink" runat="server" NavigateUrl="" Target="_blank"><asp:Label ID="SupplierInfo" runat="server" Text=""></asp:Label></asp:HyperLink>
            </p>
            <table align="left">
                <tr>
                    <td><a href="#" onclick="popup('./SupplierSetting.aspx?Comment=1&Action=Edit&Code=<%Response.Write(st_SupplierCode)%>')"><strong>Comment : </strong></a></td>
                </tr>
            </table>
            <table >
                <tr>
                    <td><asp:Label ID="Comment" runat="server" Text=""></asp:Label></td>
                </tr>
            </table> 
            <table align="left">
                <tr>
                    <td><a href="#" onclick="popup('./SupplierSetting.aspx?Comment=2&Action=Edit&Code=<%Response.Write(st_SupplierCode)%>')"><strong>Supplier Warning : </strong></a></td>
                </tr>
            </table>
            <table >
                <tr>
                    <td><asp:Label ID="SupplierWarning" runat="server" Text=""></asp:Label></td>
                </tr>
            </table> 
        </div>

        <br />
        <hr />
        
        <div class="main switch">
            <table style="margin-bottom:5px">
                <tr>
                    <th>Validity Quotation : </th>
                    <td><asp:DropDownList ID="ValidQuotation" runat="server" DataValueField="ValidQuotation" DataTextField="ValidQuotationText"></asp:DropDownList></td>
                    <td>
                        <asp:Button runat="server" ID="Search" Text="Search" />
                        <asp:Button runat="server" ID="Release" Text="Release" />
                    </td>
                </tr>
            </table>
        </div>
        
        <div class="list">
            <asp:ListView ID="RFQHeaderList" runat="server" DataSourceID="SrcRFQHeader">
                <LayoutTemplate>
               
                    <div class="pagingHead">
                        <asp:DataPager ID="RFQPagerCountTop" runat="server" PageSize="10">    
                            <Fields>
                                <asp:TemplatePagerField>              
                                    <PagerTemplate>
                                    Page
                                    <asp:Label runat="server" ID="CurrentPageLabel" 
                                    Text="<%# IIf(Container.TotalRowCount>0,  (Container.StartRowIndex / Container.PageSize) + 1 , 0) %>" />
                                    of
                                    <asp:Label runat="server" ID="TotalPagesLabel" 
                                    Text="<%# Math.Ceiling (System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
                                    (<asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" /> records)
                                    </PagerTemplate>
                                </asp:TemplatePagerField>
                            </Fields>
                        </asp:DataPager>
                    </div>
                    
                    <div class="paging">
                        <asp:DataPager ID="RFQPagerLinkTop" runat="server" PageSize="10">
                            <Fields>
                                <asp:NumericPagerField ButtonCount="10" CurrentPageLabelCssClass="current" NumericButtonCssClass="numeric" PreviousPageText="&laquo; Previous" NextPageText="Next &raquo;" />
                            </Fields>
                        </asp:DataPager>
                    </div>                    
                    
                    <div ID="itemPlaceholderContainer" runat="server">
                        <div ID="itemPlaceholder" runat="server">
                        </div>
                    </div>
                    
                    <div class="paging">
                        <asp:DataPager ID="RFQPagerLinkBottom" runat="server" PageSize="10">
                            <Fields>
                                <asp:NumericPagerField ButtonCount="10" CurrentPageLabelCssClass="current" NumericButtonCssClass="numeric" PreviousPageText="&laquo; Previous" NextPageText="Next &raquo;" />
                            </Fields>
                        </asp:DataPager>
                    </div>
                    
                    <div class="pagingHead">
                        <asp:DataPager ID="RFQPagerCountBottom" runat="server" PageSize="10">    
                            <Fields>
                                <asp:TemplatePagerField>              
                                    <PagerTemplate>
                                    Page
                                    <asp:Label runat="server" ID="CurrentPageLabel" 
                                    Text="<%# IIf(Container.TotalRowCount>0,  (Container.StartRowIndex / Container.PageSize) + 1 , 0) %>" />
                                    of
                                    <asp:Label runat="server" ID="TotalPagesLabel" 
                                    Text="<%# Math.Ceiling (System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
                                    (<asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" /> records)
                                    </PagerTemplate>
                                </asp:TemplatePagerField>
                            </Fields>
                        </asp:DataPager>
                    </div>
                    

                    
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <h3 style="font-style:italic"><%=Purchase.Common.MSG_NO_DATA_FOUND%></h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <table>
                        <tr>
                            <th class="subhead" colspan="1">RFQ Reference Number : <a href='<%#"./RFQUpdate.aspx?RFQNumber=" & Eval("RFQNumber")%>'><asp:Label ID="RFQNumber" runat="server" Text='<%#Eval("RFQNumber")%>'></asp:Label></a></th>
                            <th class="subhead" colspan="1"><span class="placedright"><asp:label id="Priority_Title" runat="server" Text="Priority : " Visible='<%#IIF(Eval("Priority")="", False,True) %>' CssClass='<%#IIF(Eval("Priority")="B", "priorityB", "priorityA") %>'></asp:label><asp:label id="Priority" runat="server" Text='<%# Eval("Priority") %>' CssClass='<%#IIF(Eval("Priority")="B", "priorityB", "priorityA") %>'></asp:label></span></th>
                            <th class="subhead" colspan="2">Quoted Date : <asp:Label ID="QuotedDate" runat="server" Text='<%#if(isDBNull(Eval("QuotedDate")), "", Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("QuotedDate"), False, False)) %>'></asp:Label></th>
                            <th class="subhead" colspan="2">
                                <asp:Label ID="RFQStatusChangeDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("StatusChangeDate"), True, False)%>'></asp:Label>
                                <span class="indent"><asp:Label ID="RFQStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label></span>
                                <span class="indent"><asp:Label ID="RFQConfidential" runat="server" Text='<%#IIF(Eval("isCONFIDENTIAL")=True,Purchase.Common.CONFIDENTIAL,"") %>' CssClass="confidential"></asp:Label></span>
                            </th>
                        </tr>
                        <tr>
                            <th>Product Number / Name</th>
                            <td colspan="5">
                                <asp:HyperLink ID="ProductRFQLink" runat="server" NavigateUrl='<%#Eval("ProductRFQLink")%>' Target="_blank"><asp:Label ID="ProductNumber" runat="server" Text='<%#Eval("ProductNumber")%>'></asp:Label></asp:HyperLink>
                                <span class="indent"><asp:Label ID="CodeExtensionCode" runat="server" Text='<%#Eval("CodeExtensionCode")%>'></asp:Label></span>
                                <span class="indent"><asp:Label ID="ProductName" runat="server" Text='<%#Eval("ProductName")%>'></asp:Label></span>
                            </td>
                        </tr>
                        <tr>
                            <th>Supplier Code / Name / Country or Region</th>
                            <td colspan="3"><asp:Label ID="SupplierCode" runat="server" Text='<%#Eval("SupplierCode")%>'></asp:Label><span class="indent"><asp:Label ID="SupplierName" runat="server" Text='<%#Eval("SupplierName")%>'></asp:Label></span><span class="indent">(<asp:Label ID="SupplierCountry" runat="server" Text='<%#Eval("SupplierCountryName")%>'></asp:Label>)</span></td>
                            <th>Purpose</th>
                            <td><asp:Label ID="Purpose" runat="server" Text='<%#Eval("Purpose")%>'></asp:Label></td>
                        </tr>
                        <tr>
                            <th>Maker Code / Name / Country or Region</th>
                            <td colspan="3">
                                <!-- １段下に改行されて表示される現象の対策のため他項目より前に記述する -->
                                <span class="placedright"><asp:HyperLink ID="MakerInfoLink" runat="server" NavigateUrl='<%#Eval("MakerInfo")%>' Target="_blank"><asp:Label ID="MakerInfo" runat="server" Text='<%#If(IsDBNull(Eval("MakerInfo")), "", "Supplier Information")%>'></asp:Label></asp:HyperLink></span>
                                <asp:HyperLink ID="MakerRFQLink" runat="server" NavigateUrl='<%#Eval("MakerRFQLink")%>' Target="_blank"><asp:Label ID="MakerCode" runat="server" Text='<%#Eval("MakerCode")%>'></asp:Label></asp:HyperLink><span class="indent"><asp:Label ID="MakerName" runat="server" Text='<%#Eval("MakerName")%>'></asp:Label></span><span class="indent">(<asp:Label ID="MakerCountry" runat="server" Text='<%#Eval("MakerCountryName")%>'></asp:Label>)</span>
                            </td>
                            <th>Supplier Item Name</th>
                            <td><asp:Label ID="SupplierItemName" runat="server" Text='<%#Eval("SupplierItemName")%>'></asp:Label></td>
                        </tr>
                        <tr>
                            <th style="width:20%">Handling Fee / Shipment Cost</th>
                            <td style="width:20%"><asp:Label ID="ShippingHandlingCurrency" runat="server" Text='<%#Eval("ShippingHandlingCurrencyCode")%>'></asp:Label> <asp:Label ID="ShippingHandlingFee" runat="server" Text='<%#Eval("ShippingHandlingFee","{0:G29}")%>'></asp:Label></td>
                            <th style="width:10%">Enq-User / Location</th>
                            <td style="width:20%"><asp:Label ID="EnqUser" runat="server" Text='<%#Eval("EnqUserName")%>'></asp:Label><span class="indent">(<asp:Label ID="EnqLocation" runat="server" Text='<%#Eval("EnqLocationName")%>'></asp:Label>)</span></td>
                            <th style="width:10%">Quo-User / Location</th>
                            <td style="width:20%"><asp:Label ID="QuoUser" runat="server" Text='<%#Eval("QuoUserName")%>'></asp:Label><span class="indent">(<asp:Label ID="QuoLocation" runat="server" Text='<%#Eval("QuoLocationName")%>'></asp:Label>)</span></td>
                        </tr>
                        <tr>
                            <th>Comment</th>
                            <td colspan="5"><asp:Label ID="Comment" runat="server" Text='<%#Replace(Eval("Comment").ToString(), vbCrLf, "<br />")%>'></asp:Label></td>
                        </tr>
                    </table>

                    <asp:ListView ID="RFQLineList" runat="server" DataSourceID="SrcRFQLine">
                        <LayoutTemplate>
                            <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                                <tr>
                                    <th id="Th1"  runat="server" style="width:5%">No.</th>
                                    <th id="Th2"  runat="server" style="width:10%">Enq-Quantity</th>
                                    <th id="Th3"  runat="server" style="width:8%">Currency</th>
                                    <th id="Th4"  runat="server" style="width:8%">Price</th>
                                    <th id="Th5"  runat="server" style="width:10%">Quo-Quantity</th>
                                    <th id="Th6"  runat="server" style="width:10%">Lead Time</th>
                                    <th id="Th7"  runat="server" style="width:10%">Packing</th>
                                    <th id="Th8"  runat="server" style="width:10%">Purity/Method</th>
                                    <th id="Th9" runat="server" style="width:10%">Supplier Offer No</th>
                                    <th id="Th10" runat="server" style="">Supplier Item Number</th>
                                    <th id="Th11" runat="server" style="width:14%">Reason for "No Offer"</th>
                                    <th id="Th12" runat="server" style="width:5%">PO</th>
                                    <th id="Th13" runat="server" style="width:5%">Interface</th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>
                        <EmptyDataTemplate></EmptyDataTemplate>
                        <ItemTemplate>
                        <tr ID="itemPlaceholder" runat="server">
                            <th><asp:Label ID="Seq" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></th>
                            <td><asp:Label ID="EnqQuantity" runat="server" Text='<%#Eval("EnqQuantity","{0:G29}") %>'></asp:Label> <asp:Label ID="EnqUnit" runat="server" Text='<%#Eval("EnqUnitCode") %>'></asp:Label> x <asp:Label ID="EnqPiece" runat="server" Text='<%#Eval("EnqPiece") %>'></asp:Label></td>
                            <td><asp:Label ID="Currency" runat="server" Text='<%#Eval("CurrencyCode") %>'></asp:Label></td>
                            <td class="number"><asp:Label ID="UnitPrice" runat="server" Text='<%#Eval("UnitPrice","{0:G29}")%>'></asp:Label></td>
                            <td class="number"><asp:Label ID="QuoPer" runat="server" Text='<%#Eval("QuoPer","{0:G29}") %>'></asp:Label> <asp:Label ID="QuoUnit" runat="server" Text='<%#Eval("QuoUnitCode") %>'></asp:Label></td>
                            <td><asp:Label ID="LeadTime" runat="server" Text='<%#Eval("LeadTime") %>'></asp:Label></td>
                            <td><asp:Label ID="Packing" runat="server" Text='<%#Eval("Packing") %>'></asp:Label></td>
                            <td><asp:Label ID="Purity" runat="server" Text='<%# (Eval("Purity").ToString+ Eval("QMMethod").ToString) %>'></asp:Label></td>
                            <td><asp:Label ID="SupplierOfferNo" runat="server" Text='<%#Eval("SupplierOfferNo")%>'></asp:Label></td>
                            <td><asp:Label ID="SupplierItemNumber" runat="server" Text='<%#Eval("SupplierItemNumber")%>'></asp:Label></td>
                            <td><asp:Label ID="NoOfferReason" runat="server" Text='<%#Eval("NoOfferReason") %>'></asp:Label></td>
                            <td><asp:HyperLink ID="PO" runat="server" NavigateUrl='<%#If(IsDBNull(Eval("PO")), "", "./POListByRFQ.aspx?RFQLineNumber=" & Eval("RFQLineNumber"))%>'><%#If(IsDBNull(Eval("PO")), "", IIf(Eval("Priority") = "", "PO", "PO-" & Eval("Priority")))%></asp:HyperLink></td>
                            <td><asp:Label ID="OutputStatus" runat="server" Text='<%#Eval("OutputStatus")%>'></asp:Label></td>
                        </tr>
                        </ItemTemplate>
                    </asp:ListView>
                    <asp:SqlDataSource ID="SrcRFQLine" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <% End If%>
<%  End If%>
    <asp:SqlDataSource ID="SrcRFQHeader" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</form>
</body>
</html>

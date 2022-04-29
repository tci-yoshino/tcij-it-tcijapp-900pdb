<%@ Control Language="vb" AutoEventWireup="false" CodeBehind="SearchResult.ascx.vb" Inherits="Purchase.SearchResult" %>
<!-- Search Result -->
<div class="list">
    <asp:Panel ID="PagerTop" runat="server" CssClass="paging" Visible="false">
        <asp:LinkButton ID="PagerTopPrev" Text="&laquo; Prev" runat="server" CommandName="Prev"></asp:LinkButton><span class="indent"></span>
        <asp:ListView ID="PagerTopNumber" runat="server">
            <LayoutTemplate>
                <div ID="itemPlaceholder" runat="server"></div>
            </LayoutTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="PagerNumber" Text='<%#Eval("Text") %>' runat="server" CommandName='<%#Eval("Value") %>' CssClass='<%#iif(Eval("enabled"),"","current") %>'></asp:LinkButton><span class="indent"></span>
            </ItemTemplate>
        </asp:ListView>
        <asp:LinkButton ID="PagerTopNext" Text="Next &raquo;" runat="server" CommandName="Next"></asp:LinkButton>
    </asp:Panel>
    
    <asp:ListView ID="ListSearchResult" runat="server">
        <EmptyDataTemplate>
            <h3 style="font-style: italic"><% =Purchase.Common.MSG_NO_DATA_FOUND%></h3>
        </EmptyDataTemplate>
        <ItemTemplate>
            <table>
                <tr>
                    <th class="subhead" colspan="1">RFQ Reference Number : 
                        <asp:HyperLink ID="lst_RFQNumber" runat="server" NavigateUrl='<%#Eval("RFQNumber","../RFQUpdate.aspx?RFQNumber={0}")%>' Text='<%#Eval("RFQNumber")%>' />
                    </th>
                    <th class="subhead" colspan="1">
                        <span class="placedright"><asp:Label ID="Priority_Title" runat="server" Text="Priority : " Visible='<%#IIF(Eval("Priority")="", False,True) %>' CssClass='<%#IIF(Eval("Priority")="B", "priorityB", "priorityA") %>'></asp:Label>
                            <asp:Label ID="lst_Priority" runat="server" Text='<%# Eval("Priority") %>' CssClass='<%#IIF(Eval("Priority")="B", "priorityB", "priorityA") %>'></asp:Label>
                        </span>
                    </th>
                    <th class="subhead" colspan="2">Quoted Date :
                        <asp:Label ID="QuotedDate" runat="server" Text=''><%#If(String.IsNullOrEmpty(Eval("QuotedDate")), "", Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("QuotedDate"), False, False))%></asp:Label>
                    </th>
                    <th class="subhead" colspan="2">
                        <asp:Label ID="RFQStatusChangeDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("StatusChangeDate"), True, False)%>'></asp:Label>
                        <span class="indent">
                            <asp:Label ID="RFQStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label>
                        </span>
                        <span class="indent">
                            <asp:Label ID="RFQConfidential" runat="server" Text='<%#IIF(Eval("isCONFIDENTIAL")=True,Purchase.Common.CONFIDENTIAL,"") %>' CssClass="confidential"></asp:Label>
                        </span>
                    </th>
                </tr>
                <tr>
                    <th>Product Number / Name</th>
                    <td colspan="5">
                        <asp:HyperLink ID="ProductNumberLink" runat="server" NavigateUrl='<%#Eval("ProductID","../RFQListByProduct.aspx?ProductID={0}")%>' Text='<%#Eval("ProductNumber")%>' />
                        <span class="indent">
                            <asp:Label ID="CodeExtensionCode" runat="server" Text='<%#Eval("CodeExtensionCode")%>'></asp:Label>
                        </span>
                        <span class="indent">
                            <asp:Label ID="ProductName" runat="server" Text='<%#Eval("ProductName")%>'></asp:Label>
                        </span>
                    </td>
                </tr>
                <tr>
                    <th>Supplier Code / Name / Country</th>
                    <td colspan="3"><span class="placedright"></span>
                        <!-- １段下に改行されて表示される現象の対策のため他項目より前に記述する -->
                        <span class="placedright">
                            <asp:HyperLink ID="SupplierInfoLink" runat="server" NavigateUrl='<%#Eval("SupplierInfo")%>' Target="_blank">
                                <asp:Label ID="SupplierInfo" runat="server" Text='<%#If(String.IsNullOrEmpty(Eval("SupplierInfo")), "", "Supplier Information")%>'></asp:Label>
                            </asp:HyperLink>
                        </span>
                        <asp:HyperLink ID="SupplierCodeLink" runat="server" NavigateUrl='<%#Eval("SupplierCode","../RFQListBySupplier.aspx?SupplierCode={0}")%>' Text='<%#Eval("SupplierCode")%>' />
                        
                        <asp:Label ID="lst_SupplierName" runat="server" Text='<%#Eval("SupplierName")%>'></asp:Label>
                        <span class="indent">(<asp:Label ID="SupplierCountry" runat="server" Text='<%#Eval("SupplierCountryName")%>'></asp:Label>)</span>
                    </td>
                    <th>Purpose</th>
                    <td>
                        <asp:Label ID="lst_Purpose" runat="server" Text='<%#Eval("Purpose")%>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th>Maker Name / Country</th>
                    <td colspan="3">
                        <!-- １段下に改行されて表示される現象の対策のため他項目より前に記述する -->
                        <span class="placedright">
                            <asp:HyperLink ID="MakerInfoLink" runat="server" NavigateUrl='<%#Eval("MakerInfo")%>' Target="_blank">
                                <asp:Label ID="MakerInfo" runat="server" Text='<%#If(String.IsNullOrEmpty(Eval("MakerInfo")), "", "Supplier Information")%>'></asp:Label>
                            </asp:HyperLink>
                        </span>
                        <asp:Label ID="MakerName" runat="server" Text='<%#Eval("MakerName")%>'></asp:Label>
                        <span class="indent">(<asp:Label ID="MakerCountry" runat="server" Text='<%#Eval("MakerCountryName")%>'></asp:Label>)</span>
                    </td>
                    <th>Supplier Item Name</th>
                    <td>
                        <asp:Label ID="lst_SupplierItemName" runat="server" Text='<%#Eval("SupplierItemName")%>'></asp:Label>
                    </td>
                </tr>
                <tr>
                    <th style="width: 20%">Handling Fee / Shipment Cost</th>
                    <td style="width: 20%">
                        <asp:Label ID="ShippingHandlingCurrency" runat="server" Text='<%#Eval("ShippingHandlingCurrencyCode")%>'></asp:Label>
                        <asp:Label ID="ShippingHandlingFee" runat="server" Text='<%#Eval("ShippingHandlingFee","{0:G29}")%>'></asp:Label>
                    </td>
                    <th style="width: 10%">Enq-User / Location</th>
                    <td style="width: 20%">
                        <asp:Label ID="EnqUser" runat="server" Text='<%#Eval("EnqUserName")%>'></asp:Label>
                        <span class="indent">(<asp:Label ID="EnqLocation" runat="server" Text='<%#Eval("EnqLocationName")%>'></asp:Label>)</span>
                    </td>
                    <th style="width: 10%">Quo-User / Location</th>
                    <td style="width: 20%">
                        <asp:Label ID="QuoUser" runat="server" Text='<%#Eval("QuoUserName")%>'></asp:Label>
                        <span class="indent">(<asp:Label ID="QuoLocation" runat="server" Text='<%#Eval("QuoLocationName")%>'></asp:Label>)</span>
                    </td>
                </tr>
                <tr>
                    <th>Comment</th>
                    <td colspan="5">
                        <asp:Label ID="Comment" runat="server" Text='<%#If(String.IsNullOrEmpty(Eval("Comment")), "", Replace(Eval("Comment"), vbCrLf, "<br />"))%>'></asp:Label>
                    </td>
                </tr>
            </table>

            <asp:ListView ID="RFQLineList" runat="server" >
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr>
                            <th id="Th1" runat="server" style="width: 5%">No.</th>
                            <th id="Th2" runat="server" style="width: 10%">Enq-Quantity</th>
                            <th id="Th3" runat="server" style="width: 8%">Currency</th>
                            <th id="Th4" runat="server" style="width: 8%">Price</th>
                            <th id="Th5" runat="server" style="width: 9%">Quo-Quantity</th>
                            <th id="Th6" runat="server" style="width: 9%">Lead Time</th>
                            <th id="Th7" runat="server" style="width: 9%">Packing</th>
                            <th id="Th8" runat="server" style="width: 9%">Purity/Method</th>
                            <th id="Th9" runat="server" style="width: 9%">Supplier Offer No</th>
                            <th id="Th10" runat="server" style="width: 9%">Supplier Item Number</th>
                            <th id="Th11" runat="server" style="width: 14%">Reason for "No Offer"</th>
                            <th id="Th12" runat="server" style="width: 5%">PO</th>
                            <th id="Th13" runat="server" style="width: 5%">Interface</th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server"></tr>
                    </table>
                </LayoutTemplate>
                <EmptyDataTemplate></EmptyDataTemplate>
                <ItemTemplate>
                    <tr id="itemPlaceholder" runat="server">
                        <th><asp:Label ID="Seq" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label></th>
                        <td><asp:Label ID="EnqQuantity" runat="server" Text='<%#Eval("EnqQuantity","{0:G29}") %>'></asp:Label><asp:Label ID="EnqUnit" runat="server" Text='<%#Eval("EnqUnitCode") %>'></asp:Label> x <asp:Label ID="EnqPiece" runat="server" Text='<%#Eval("EnqPiece") %>'></asp:Label></td>
                        <td><asp:Label ID="Currency" runat="server" Text='<%#Eval("CurrencyCode") %>'></asp:Label></td>
                        <td class="number"><asp:Label ID="UnitPrice" runat="server" Text='<%#Eval("UnitPrice","{0:G29}")%>'></asp:Label></td>
                        <td class="number"><asp:Label ID="QuoPer" runat="server" Text='<%#Eval("QuoPer","{0:G29}") %>'></asp:Label><asp:Label ID="QuoUnit" runat="server" Text='<%#Eval("QuoUnitCode") %>'></asp:Label></td>
                        <td><asp:Label ID="LeadTime" runat="server" Text='<%#Eval("LeadTime") %>'></asp:Label></td>
                        <td><asp:Label ID="Packing" runat="server" Text='<%#Eval("Packing")%>'></asp:Label></td>
                        <td><asp:Label ID="Purity" runat="server" Text='<%#(Eval("Purity").ToString+ Eval("QMMethod").ToString) %>'></asp:Label></td>
                        <td><asp:Label ID="SupplierOfferNo" runat="server" Text='<%#Eval("SupplierOfferNo")%>'></asp:Label></td>
                        <td><asp:Label ID="SupplierItemNumber" runat="server" Text='<%#Eval("SupplierItemNumber")%>'></asp:Label></td>
                        <td><asp:Label ID="NoOfferReason" runat="server" Text='<%#Eval("NoOfferReason") %>'></asp:Label></td>
                        <td><asp:HyperLink ID="PO" runat="server" NavigateUrl='<%#If(String.IsNullOrEmpty(Eval("PO")), "", "../POListByRFQ.aspx?RFQLineNumber=" & Eval("RFQLineNumber"))%>'><%#If(String.IsNullOrEmpty(Eval("PO")), "", IIf(Eval("Priority") = "", "PO", "PO-" & Eval("Priority")))%></asp:HyperLink></td>
                        <td><asp:Label ID="Interface" runat="server" Text='<%#Eval("OutputStatusInterface") %>'></asp:Label></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>

        </ItemTemplate>
    </asp:ListView>
    
    <asp:Panel ID="PagerBottom" runat="server" CssClass="paging" Visible="false">
        <asp:LinkButton ID="PagerBottomPrev" Text="&laquo; Prev" runat="server" CommandName="Prev"></asp:LinkButton><span class="indent"></span>
        <asp:ListView ID="PagerBottomNumber" runat="server">
            <LayoutTemplate>
                <div ID="itemPlaceholder" runat="server"></div>
            </LayoutTemplate>
            <ItemTemplate>
                <asp:LinkButton ID="PagerNumber" Text='<%#Eval("Text") %>' runat="server" CommandName='<%#Eval("Value") %>' CssClass='<%#iif(Eval("enabled"),"","current") %>'></asp:LinkButton><span class="indent"></span>
            </ItemTemplate>
        </asp:ListView>
        <asp:LinkButton ID="PagerBottomNext" Text="Next &raquo;" runat="server" CommandName="Next"></asp:LinkButton>
    </asp:Panel>
    
</div>
<!-- Search Result End -->
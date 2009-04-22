<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQStatus.aspx.vb" Inherits="Purchase.RFQStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
    <script type="text/javascript">
    //<![CDATA[
        window.onload = function() {
            colorful.set();
            if (document.SearchForm) {
                document.SearchForm.StatusSortOrderFrom.focus();
            }
        }
    //]]>
    </script>
</head>
<body>
    <form id="SearchForm" runat="server">
    <!-- Main Content Area -->
        <div id="content">
            <div class="tabs">　</div>

            <h3>RFQ Status</h3>

            <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
                <table>
                    <tr>
                        <th>Current Status <span class="required"></span> : </th>
                        <td>
                            from <asp:DropDownList ID="StatusSortOrderFrom" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            to <asp:DropDownList ID="StatusSortOrderTo" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Enq-Location / User : </th>
                        <td>
                            <asp:DropDownList ID="EnqLocationCode" runat="server" AutoPostBack="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <span class="indent"></span>
                            <asp:DropDownList ID="EnqUserID" runat="server">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Quo-Location / User : </th>
                        <td>
                            <asp:DropDownList ID="QuoLocationCode" runat="server" AutoPostBack="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <span class="indent"></span>
                            <asp:DropDownList ID="QuoUserID" runat="server">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Quoted Date : </th>
                        <td>
                            from <asp:TextBox ID="QuotedDateFrom" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
                            to <asp:TextBox ID="QuotedDateTo" runat="server" Width="7em" MaxLength="10"></asp:TextBox><span class="format">(YYYY-MM-DD)</span>
                        </td>
                    </tr>
                    <tr>
                        <th>Status Change Date : </th>
                        <td>
                            from <asp:TextBox ID="StatusChangeDateFrom" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
                            to <asp:TextBox ID="StatusChangeDateTo" runat="server" Width="7em" MaxLength="10"></asp:TextBox><span class="format">(YYYY-MM-DD)</span>
                        </td>
                    </tr>
                    <tr>
                        <th>Payment Term Code : </th>
                        <td>
                            <asp:DropDownList ID="PaymentTermCode" runat="server" AutoPostBack="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="Search" runat="server" Text="Search" />
                <asp:Button ID="Clear" runat="server" Text="Clear" OnClientClick ="clearForm('SearchForm');" />
            </div>

            <hr />

            <div class="list">
                <asp:ListView ID="RFQHeaderList" runat="server" DataSourceID="SrcRFQHeader">
                    <%--<AlternatingItemTemplate>
                        <table class="alternative">
                            <tr>
                                <th class="subhead" colspan="2">RFQ Reference Number : <a href='<%#"./RFQUpdate.aspx?RFQNumber=" & Eval("RFQNumber")%>'><asp:Label ID="RFQNumber" runat="server" Text='<%#Eval("RFQNumber")%>'></asp:Label></a></th>
                                <th class="subhead" colspan="2">Quoted Date : <asp:Label ID="QuotedDate" runat="server" Text='<%#if(isDBNull(Eval("QuotedDate")), "", Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("QuotedDate"), False, False)) %>'></asp:Label></th>
                                <th class="subhead" colspan="2"><asp:Label ID="RFQStatusChangeDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("StatusChangeDate"), True, False)%>'></asp:Label><span class="indent"><asp:Label ID="RFQStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label></span></th>
                            </tr>
                            <tr>
                                <th>Product Number / Name</th>
                                <td colspan="3"><asp:Label ID="ProductNumber" runat="server" Text='<%#Eval("ProductNumber")%>'></asp:Label><span class="indent"><asp:Label ID="ProductName" runat="server" Text='<%#Eval("ProductName")%>'></asp:Label></span></td>
                                <th>CAS Number</th>
                                <td><asp:Label ID="CASNumber" runat="server" Text='<%#Eval("CASNumber")%>'></asp:Label></td>
                            </tr>
                            <tr>
                                <th>Supplier Name / Country</th>
                                <td colspan="3"><asp:Label ID="SupplierName" runat="server" Text='<%#Eval("SupplierName")%>'></asp:Label><span class="indent">(<asp:Label ID="SupplierCountry" runat="server" Text='<%#Eval("SupplierCountryName")%>'></asp:Label>)</span></td>
                                <th>Purpose</th>
                                <td><asp:Label ID="Purpose" runat="server" Text='<%#Eval("Purpose")%>'></asp:Label></td>
                            </tr>
                            <tr>
                                <th>Maker Name / Country</th>
                                <td colspan="3"><asp:Label ID="MakerName" runat="server" Text='<%#Eval("MakerName")%>'></asp:Label><span class="indent">(<asp:Label ID="MakerCountry" runat="server" Text='<%#Eval("MakerCountryName")%>'></asp:Label>)</span></td>
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
                    </AlternatingItemTemplate>--%>
                    
                    <LayoutTemplate>
                        <div class="pagingHead" >
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
                        <h3 style="font-style:italic"><%=Purchase.Common.ERR_NO_MATCH_FOUND%></h3>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <table>
                            <tr>
                                <th class="subhead" colspan="2">RFQ Reference Number : <a href='<%#"./RFQUpdate.aspx?RFQNumber=" & Eval("RFQNumber")%>'><asp:Label ID="RFQNumber" runat="server" Text='<%#Eval("RFQNumber")%>'></asp:Label></a></th>
                                <th class="subhead" colspan="2">Quoted Date : <asp:Label ID="QuotedDate" runat="server" Text='<%#if(isDBNull(Eval("QuotedDate")), "", Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("QuotedDate"), False, False)) %>'></asp:Label></th>
                                <th class="subhead" colspan="2"><asp:Label ID="RFQStatusChangeDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("StatusChangeDate"), True, False)%>'></asp:Label><span class="indent"><asp:Label ID="RFQStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label></span></th>
                            </tr>
                            <tr>
                                <th>Product Number / Name</th>
                                <td colspan="3"><asp:Label ID="ProductNumber" runat="server" Text='<%#Eval("ProductNumber")%>'></asp:Label><span class="indent"><asp:Label ID="ProductName" runat="server" Text='<%#Eval("ProductName")%>'></asp:Label></span></td>
                                <th>CAS Number</th>
                                <td><asp:Label ID="CASNumber" runat="server" Text='<%#Eval("CASNumber")%>'></asp:Label></td>
                            </tr>
                            <tr>
                                <th>Supplier Name / Country</th>
                                <td colspan="3"><asp:Label ID="SupplierName" runat="server" Text='<%#Eval("SupplierName")%>'></asp:Label><span class="indent">(<asp:Label ID="SupplierCountry" runat="server" Text='<%#Eval("SupplierCountryName")%>'></asp:Label>)</span></td>
                                <th>Purpose</th>
                                <td><asp:Label ID="Purpose" runat="server" Text='<%#Eval("Purpose")%>'></asp:Label></td>
                            </tr>
                            <tr>
                                <th>Maker Name / Country</th>
                                <td colspan="3"><asp:Label ID="MakerName" runat="server" Text='<%#Eval("MakerName")%>'></asp:Label><span class="indent">(<asp:Label ID="MakerCountry" runat="server" Text='<%#Eval("MakerCountryName")%>'></asp:Label>)</span></td>
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
                                        <th id="Th8"  runat="server" style="width:10%">Purity</th>
                                        <th id="Th9" runat="server" style="width:10%">Method</th>
                                        <th id="Th11" runat="server" style="width:14%">Reason for "No Offer"</th>
                                        <th id="Th10" runat="server" style="width:5%">PO</th>
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
                                <td><asp:Label ID="Purity" runat="server" Text='<%#Eval("Purity") %>'></asp:Label></td>
                                <td><asp:Label ID="QMMethod" runat="server" Text='<%#Eval("QMMethod") %>'></asp:Label></td>
                                <td><asp:Label ID="NoOfferReason" runat="server" Text='<%#Eval("NoOfferReason") %>'></asp:Label></td>
                                <td><asp:HyperLink ID="PO" runat="server" NavigateUrl='<%#If(IsDBNull(Eval("PO")), "", "./POListByRFQ.aspx?RFQLineNumber=" & Eval("RFQLineNumber"))%>'><%#If(IsDBNull(Eval("PO")), "", "PO")%></asp:HyperLink></td>
                            </tr>
                            </ItemTemplate>
                        </asp:ListView>
                        <asp:SqlDataSource ID="SrcRFQLine" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                    </ItemTemplate>
                </asp:ListView>
                <asp:SqlDataSource ID="SrcRFQHeader" runat="server" 
                    ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                    ProviderName="<%$ ConnectionStrings:DatabaseConnect.ProviderName %>" SelectCommand="SELECT                  TOP (100) PERCENT RFQNumber, EnqLocationCode, EnqLocationName, EnqUserID, EnqUserName, QuoLocationCode, QuoLocationName, 
                                      QuoUserID, QuoUserName, ProductID, ProductNumber, ProductName, SupplierCode, SupplierName, SupplierCountryCode, 
                                      SupplierContactPerson, R3SupplierCode, R3SupplierName, MakerCode, MakerName, MakerCountryCode, R3MakerCode, R3MakerName, 
                                      PaymentTermCode, RequiredPurity, RequiredQMMethod, RequiredSpecification, SpecSheet, Specification, PurposeCode, Purpose, 
                                      SupplierItemName, ShippingHandlingFee, ShippingHandlingCurrencyCode, Comment, QuotedDate, StatusCode, UpdateDate, Status, 
                                      StatusSortOrder, StatusChangeDate, '' as CASNumber, '' as SupplierCountryName, '' as MakerCountryName
    FROM                     dbo.v_RFQHeader
    WHERE                   (EnqLocationCode = 'JP') AND (EnqUserID = 1174) AND (QuoLocationCode = 'EU') AND (QuoUserID = 1437) AND 
                                      (QuotedDate &gt;= CONVERT(DATETIME, '2000-01-01 00:00:00', 102)) AND (QuotedDate &lt;= CONVERT(DATETIME, 
                                      '2008-12-31 00:00:00', 102)) AND (StatusChangeDate &gt;= CONVERT(DATETIME, '2009-04-01 00:00:00', 102)) AND 
                                      (StatusChangeDate &lt;= CONVERT(DATETIME, '2009-04-30 00:00:00', 102)) AND (StatusSortOrder &gt;= 1) AND (StatusSortOrder &lt;= 6)
    ORDER BY           StatusSortOrder, QuotedDate DESC, StatusChangeDate DESC, RFQNumber">
                </asp:SqlDataSource>
            </div>
        </div><!-- Main Content Area END -->

        <!-- Footer -->
        <!--#include virtual="./Footer.html" --><!-- Footer END -->
    <asp:HiddenField ID="Action" runat="server" Value="Search" />
    </form>
</body>
</html>

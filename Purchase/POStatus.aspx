<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="POStatus.aspx.vb" Inherits="Purchase.POStatus" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
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

            <h3>PO Status</h3>

            <div class="main">
                <table>
                    <tr>
                        <th>Current Status <span class="required">*</span> : </th>
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
                        <th>PO Location / User : </th>
                        <td>
                            <asp:DropDownList ID="POLocationCode" runat="server">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <span class="indent"></span>
                            <asp:DropDownList ID="POUserID" runat="server">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Supplier Code : </th>
                        <td><asp:TextBox ID="SupplierCode" runat="server" Width="7em" MaxLength="10"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Supplier Name : </th>
                        <td><asp:TextBox ID="SupplierName" runat="server" Width="21em" MaxLength="255"></asp:TextBox> (Partial text match)</td>
                    </tr>
                    <tr>
                        <th>PO Date : </th>
                        <td>
                            from <asp:TextBox ID="PODateFrom" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
                            to <asp:TextBox ID="PODateTo" runat="server" Width="7em" MaxLength="10"></asp:TextBox><span class="format">(YYYY-MM-DD)</span>
                        </td>
                    </tr>
                </table>
                <asp:Button ID="Search" runat="server" Text="Search" />
                <input type="button" value="Clear" onclick="clearForm('SearchForm')"/>
            </div>

            <hr />

            <div class="list">
                <asp:ListView ID="POList" runat="server" DataSourceID="SrcPO">
                    <LayoutTemplate>
                        <div class="pagingHead">
                            <asp:DataPager ID="POPagerCountTop" runat="server" PageSize="10">    
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
                            <asp:DataPager ID="POPagerLinkTop" runat="server" PageSize="10">
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
                            <asp:DataPager ID="POPagerLinkBottom" runat="server" PageSize="10">
                                <Fields>
                                    <asp:NumericPagerField ButtonCount="10" CurrentPageLabelCssClass="current" NumericButtonCssClass="numeric" PreviousPageText="&laquo; Previous" NextPageText="Next &raquo;" />
                                </Fields>
                            </asp:DataPager>
                        </div>
                        
                        <div class="pagingHead">
                            <asp:DataPager ID="POPagerCountBottom" runat="server" PageSize="10">    
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
                                <th class="subhead" colspan="2">PO Number : <asp:HyperLink ID="POUpdate" runat="server" NavigateUrl='<%#"./POUpdate.aspx?PONumber=" & Eval("PONumber")%>'><asp:label id="PONumber" runat="server" Text='<%#Eval("PONumber")%>'></asp:label></asp:HyperLink><span class="indent"></span></th>
                                <th class="subhead" colspan="4"><asp:label id="POStatusChangeDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"),Eval("StatusChangeDate"), True, False)%>'></asp:label><span class="indent"><asp:label id="POStatus" runat="server" Text='<%#Eval("Status")%>'></asp:label></span></th>
                            </tr>
                            <tr>
                                <th style="width:17%">Product Number / Name</th>
                                <td style="width:33%"><asp:label id="ProductNumber" runat="server" Text='<%#Eval("ProductNumber")%>'></asp:label><span class="indent"><asp:label id="ProductName" runat="server" Text='<%#Purchase.Common.CutShort(Eval("ProductName").ToString())%>'></asp:label></span></td>
                                <th style="width:10%">PO Date</th>
                                <td style="width:12%"><asp:label id="PODate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"),Eval("PODate"), False, False)%>'></asp:label></td>
                                <th style="width:10%">PO-User</th>
                                <td style="width:18%"><asp:label id="POUser" runat="server" Text='<%#Eval("POUserName")%>'></asp:label><span class="indent">(<asp:label id="POLocation" runat="server" Text='<%#Eval("POLocationName")%>'></asp:label>)</span></td>
                            </tr>
                            <tr>
                                <th>Supplier Name</th>
                                <td><asp:label id="SupplierName" runat="server" Text='<%#Eval("SupplierName")%>'></asp:label></td>
                                <th>Maker Name</th>
                                <td colspan="3"><asp:label id="MakerName" runat="server" Text='<%#Eval("MakerName")%>'></asp:label></td>
                            </tr>
                            <tr>
                                <th>Delivery Date</th>
                                <td><asp:label id="DeliveryDate" runat="server" Text='<%#If(IsDBNull(Eval("DeliveryDate")), Eval("DeliveryDate"), Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("DeliveryDate"), False, False))%>'></asp:label></td>
                                <th>Order Quantity</th>
                                <td><asp:label id="OrderQuantity" runat="server" Text='<%#Eval("OrderQuantity","{0:G29}")%>'></asp:label> <asp:label id="OrderUnit" runat="server" Text='<%#Eval("OrderUnitCode")%>'></asp:label></td>
                                <th>Price</th>
                                <td><asp:label id="Currency" runat="server" Text='<%#Eval("CurrencyCode")%>'></asp:label> <asp:label id="UnitPrice" runat="server" Text='<%#Eval("UnitPrice","{0:G29}")%>'></asp:label> / <asp:label id="PerQuantity" runat="server" Text='<%#Eval("PerQuantity","{0:G29}")%>'></asp:label> <asp:label id="PerUnit" runat="server" Text='<%#Eval("PerUnitCode")%>'></asp:label></td>
                            </tr>
                        </table>
                    </ItemTemplate>
                </asp:ListView>
            </div>
        </div><!-- Main Content Area END -->
        <asp:SqlDataSource ID="SrcPO" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

        <!-- Footer -->
        <!--#include virtual="./Footer.html" --><!-- Footer END -->
    </form>

</body>
</html>

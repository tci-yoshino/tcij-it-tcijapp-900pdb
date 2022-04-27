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
    <!-- Header -->
    <commonUC:Header ID="HeaderMenu" runat="server" />
    <!-- Header End -->

    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"></div>

        <h3>PO List</h3>
        
        <div class="main">
          <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
        </div>

<%  If Not String.IsNullOrEmpty(st_RFQLineNumber) Then%>
        <div class="list">
            <asp:ListView ID="POList" runat="server" DataSourceID="SrcPO">
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
                    <table>
                        <tr>
                            <th class="subhead" colspan="2">PO Number : <asp:HyperLink ID="POUpdate" runat="server" NavigateUrl='<%#"./POUpdate.aspx?PONumber=" & Eval("PONumber")%>'><asp:label id="PONumber" runat="server" Text='<%#Eval("PONumber")%>'></asp:label></asp:HyperLink><span class="indent"></span></th>
                            <th class="subhead" colspan="4">
                                <asp:label id="POStatusChangeDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"),Eval("StatusChangeDate"), True, False)%>'></asp:label>
                                <span class="indent"><asp:label id="POStatus" runat="server" Text='<%#Eval("Status")%>'></asp:label></span>
                                <span class="indent"><asp:Label ID="POConfidential" runat="server" Text='<%#IIF(Eval("isCONFIDENTIAL")=True,Purchase.Common.CONFIDENTIAL,"") %>' CssClass="confidential"></asp:Label></span>
                            </th>
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
<%  End If%>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcPO" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

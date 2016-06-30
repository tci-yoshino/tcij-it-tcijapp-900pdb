﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MyTask.aspx.vb" Inherits="Purchase.MyTask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
    <script type="text/javascript">
    <!--

window.onload = function() {
   colorful.set();
   navi('home');
   
}
    -->
    </script>
</head>
<body>
    <!-- Sub Navigation -->
    <div id="subNavi">
        <a href="./MyTask.aspx" class="current">My Tasks</a> | <a href="./RequestedTask.aspx">Requested Tasks</a> | <a href="./UnassignedTask.aspx">Unassigned Tasks</a> | <a href="./JFYISearch.aspx">JFYI</a>
    </div><!-- Sub Navigation END -->

    <!-- Main Content Area -->
    <div id="content">   
        <form ID="SwitchForm" runat="server">
        <div class="main switch">
                <table style="margin-bottom:0">
                    <tr>
                        <th>User : </th>
                        <td>
                            <asp:DropDownList ID="UserID" runat="server">
                            </asp:DropDownList>
                        </td>
                        <th>RFQ Priority : </th>
                        <td>
                            <asp:DropDownList ID="RFQPriority" runat="server">
                            </asp:DropDownList>
                        </td>
                        <th>PO Priority : </th>
                        <td>
                            <asp:DropDownList ID="POPriority" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td><asp:Button ID="Switch" runat="server" Text="Switch" OnClick="Switch_Click" PostBackUrl="MyTask.aspx?Action=Switch" /></td>
                    </tr>
                </table>
        </div>

       <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
       </div>

<%  If (IsPostBack) And (String.IsNullOrEmpty(st_Action)) Then%>
<%Else%>
        <h3>RFQ</h3>

        <div class="list">
            <asp:ListView ID="RFQList" runat="server">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <h3 style="font-style:italic"><%=Purchase.Common.MSG_NO_DATA_FOUND%></h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr>
                        <th class="subhead" colspan="2">
                            <!-- １段下に改行されて表示される現象の対策のため「RFQ Reference Number」の前に記述する -->
                            <span class="placedright"><asp:label id="Priority_Title_RFQ" runat="server" Text="Priority : " Visible='<%#IIF(Eval("Priority")="", False,True) %>' CssClass='<%#IIF(Eval("Priority")="B", "priorityB", "priorityA") %>'></asp:label><asp:label id="Priority_RFQ" runat="server" Text='<%# Eval("Priority") %>' CssClass='<%#IIF(Eval("Priority")="B", "priorityB", "priorityA") %>'></asp:label></span>
                            RFQ Reference Number : <asp:HyperLink ID="RFQUpdate" runat="server" NavigateUrl='<%# "./RFQUpdate.aspx?RFQNumber=" & Eval("RFQNumber") %>'><asp:label id="RFQNumber" runat="server" Text='<%# Eval("RFQNumber") %>'></asp:label></asp:HyperLink>
                            <span class="indent"><em><asp:label id="RFQCorrespondence" runat="server" Text='<%# Eval("RFQCorrespondence") %>'></asp:label></em></span>
                        </th>
                        <th class="subhead" colspan="3">
                            <asp:label id="RFQCreateDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("CreateDate"), True, False) & "　Create"%>'></asp:label>
                            <span class="indent"><asp:label id="RFQStatusChangeDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("StatusChangeDate"), True, False)%>'></asp:label></span>
                            <span class="indent"><asp:label id="RFQStatus" runat="server" Text='<%# Eval("Status") %>'></asp:label></span>
                            <span class="indent"><asp:Label ID="RFQConfidential" runat="server" Text='<%#IIF(Eval("isCONFIDENTIAL")=True,Purchase.Common.CONFIDENTIAL,"") %>' CssClass="confidential"></asp:Label></span>
                        </th>
                        <th class="subhead" style="text-align:right">
                           <asp:Button ID="RFQCancelAssign" runat="server" Text="Cancel Assignment" Visible="False" />
                           <asp:HiddenField ID="StatusCode" runat="server" Value='<%# Eval("StatusCode") %>'/>
                        </th>
                    </tr>
                    <tr>
                        <th style="width:17%">Product Number / Name</th>
                        <td style="width:33%"><asp:label id="ProductNumber" runat="server" Text='<%# Eval("ProductNumber") %>'></asp:label><span class="indent"><asp:label id="ProductName" runat="server" Text='<%#Purchase.Common.CutShort(Eval("ProductName").ToString())%>'></asp:label></span></td>
                        <th style="width:10%">Purpose</th>
                        <td style="width:12%"><asp:label id="Purpose" runat="server" Text='<%# Eval("Purpose") %>'></asp:label></td>
                        <th style="width:10%">Enq-User</th>
                        <td style="width:18%"><asp:label id="EnqUser" runat="server" Text='<%# Eval("EnqUserName") %>'></asp:label><span class="indent">(<asp:label id="EnqLocation" runat="server" Text='<%# Eval("EnqLocationName") %>'></asp:label>)</span></td>
                    </tr>
                    <tr>
                        <th>Supplier Name</th>
                        <td><asp:label id="SupplierName" runat="server" Text='<%# Eval("SupplierName") %>'></asp:label></td>
                        <th>Maker Name</th>
                        <td colspan="3"><asp:label id="MakerName" runat="server" Text='<%# Eval("MakerName") %>'></asp:label></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>

        <hr />

        <h3>PO</h3>

        <div class="list">
            <asp:ListView ID="POList_Overdue" runat="server">
                 <LayoutTemplate>
                    <div ID="itemPlaceholderContainer" runat="server">
                        <div ID="itemPlaceholder" runat="server">
                        </div>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <table>
                        <tr>
                            <th class="subhead" colspan="2">
                                <!-- １段下に改行されて表示される現象の対策のため「PO Number」の前に記述する -->
                                <span class="placedright"><asp:label id="Priority_Title_PO" runat="server" Text="Priority : " Visible='<%#IIF(Eval("Priority")="", False,True) %>' CssClass='<%#IIF(Eval("Priority")="B", "priorityB", "priorityA") %>'></asp:label><asp:label id="Priority_PO" runat="server" Text='<%# Eval("Priority") %>' CssClass='<%#IIF(Eval("Priority")="B", "priorityB", "priorityA") %>'></asp:label></span>
                                PO Number : <asp:HyperLink ID="POUpdate" runat="server" NavigateUrl='<%# "./POUpdate.aspx?PONumber=" & Eval("PONumber") %>'><asp:label id="PONumber" runat="server" Text='<%# Eval("PONumber") %>'></asp:label></asp:HyperLink><span class="indent"><asp:label id="Overdue" runat="server" Text="Overdue"></asp:label></span><span class="indent"><em><asp:label id="POCorrespondence" runat="server" Text='<%# Eval("POCorrespondence") %>'></asp:label></em></span>
                            </th>
                            <th class="subhead" colspan="4">
                                <asp:label id="POStatusChangeDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("StatusChangeDate"), True, False)%>'></asp:label>
                                <span class="indent"><asp:label id="POStatus" runat="server" Text='<%# Eval("StatusCode") %>'></asp:label></span>
                                <span class="indent"><asp:Label ID="POConfidential" runat="server" Text='<%#IIF(Eval("isCONFIDENTIAL")=True,Purchase.Common.CONFIDENTIAL,"") %>' CssClass="confidential"></asp:Label></span>
                            </th>
                        </tr>
                        <tr>
                            <th style="width:17%">Product Number / Name</th>
                            <td style="width:33%"><asp:label id="ProductNumber" runat="server" Text='<%# Eval("ProductNumber") %>'></asp:label><span class="indent"><asp:label id="ProductName" runat="server" Text='<%#Purchase.Common.CutShort(Eval("ProductName").ToString())%>'></asp:label></span></td>
                            <th style="width:10%">PO Date</th>
                            <td style="width:12%"><asp:label id="PODate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("PODate"), False, False)%>'></asp:label></td>
                            <th style="width:10%">PO-User</th>
                            <td style="width:18%"><asp:label id="POUser" runat="server" Text='<%# Eval("POUserName") %>'></asp:label><span class="indent">(<asp:label id="POLocation" runat="server" Text='<%# Eval("POLocationName") %>'></asp:label>)</span></td>
                        </tr>
                        <tr>
                            <th>Supplier Name</th>
                            <td><asp:label id="SupplierName" runat="server" Text='<%# Eval("SupplierName") %>'></asp:label></td>
                            <th>Maker Name</th>
                            <td colspan="3"><asp:label id="MakerName" runat="server" Text='<%# Eval("MakerName") %>'></asp:label></td>
                        </tr>
                        <tr>
                            <th>Delivery Date</th>
                            <td><asp:label id="DeliveryDate" runat="server" Text='<%#If(IsDBNull(Eval("DeliveryDate")), Eval("DeliveryDate"), Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("DeliveryDate"), False, False))%>'></asp:label></td>
                            <th>Order Quantity</th>
                            <td><asp:label id="OrderQuantity" runat="server" Text='<%#Eval("OrderQuantity","{0:G29}")%>'></asp:label> <asp:label id="OrderUnit" runat="server" Text='<%# Eval("OrderUnitCode") %>'></asp:label></td>
                            <th>Price</th>
                            <td><asp:label id="Currency" runat="server" Text='<%# Eval("CurrencyCode") %>'></asp:label> <asp:label id="UnitPrice" runat="server" Text='<%# Eval("UnitPrice","{0:G29}") %>'></asp:label> / <asp:label id="PerQuantity" runat="server" Text='<%# Eval("PerQuantity","{0:G29}") %>'></asp:label> <asp:label id="PerUnit" runat="server" Text='<%# Eval("PerUnitCode") %>'></asp:label></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:ListView>
            <asp:ListView ID="POList_PPI" runat="server">
                <LayoutTemplate>
                    <div ID="itemPlaceholderContainer" runat="server">
                        <div ID="itemPlaceholder" runat="server">
                        </div>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <table>
                        <tr>
                            <th class="subhead" colspan="2">
                                <!-- １段下に改行されて表示される現象の対策のため「PO Number」の前に記述する -->
                                <span class="placedright"><asp:label id="Priority_Title_PO" runat="server" Text="Priority : " Visible='<%#IIF(Eval("Priority")="", False,True) %>' CssClass='<%#IIF(Eval("Priority")="B", "priorityB", "priorityA") %>'></asp:label><asp:label id="Priority_PO" runat="server" Text='<%# Eval("Priority") %>' CssClass='<%#IIF(Eval("Priority")="B", "priorityB", "priorityA") %>'></asp:label></span>
                                PO Number : <asp:HyperLink ID="POUpdate" runat="server" NavigateUrl='<%# "./POUpdate.aspx?PONumber=" & Eval("PONumber") %>'><asp:label id="PONumber" runat="server" Text='<%# Eval("PONumber") %>'></asp:label></asp:HyperLink><span class="indent"><asp:label id="PPI" runat="server" Text="Par-PO Issued"></asp:label></span><span class="indent"><em><asp:label id="POCorrespondence" runat="server" Text='<%# Eval("POCorrespondence") %>'></asp:label></em></span>
                            </th>
                            <th class="subhead" colspan="3">
                                <asp:label id="POStatusChangeDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("StatusChangeDate"), True, False)%>'></asp:label>
                                <span class="indent"><asp:label id="POStatus" runat="server" Text='<%# Eval("StatusCode") %>'></asp:label></span>
                                <span class="indent"><asp:Label ID="POConfidential" runat="server" Text='<%#IIF(Eval("isCONFIDENTIAL")=True,Purchase.Common.CONFIDENTIAL,"") %>' CssClass="confidential"></asp:Label></span>
                            </th>
                            <th class="subhead" style="text-align:right">
                                <asp:Button ID="POCancelAssign" runat="server" Text="Cancel Assignment" />
                            </th>
                        </tr>
                        <tr>
                            <th style="width:17%">Product Number / Name</th>
                            <td style="width:33%"><asp:label id="ProductNumber" runat="server" Text='<%# Eval("ProductNumber") %>'></asp:label><span class="indent"><asp:label id="ProductName" runat="server" Text='<%#Purchase.Common.CutShort(Eval("ProductName").ToString())%>'></asp:label></span></td>
                            <th style="width:10%">PO Date</th>
                            <td style="width:12%"><asp:label id="PODate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("PODate"), False, False)%>'></asp:label></td>
                            <th style="width:10%">PO-User</th>
                            <td style="width:18%"><asp:label id="POUser" runat="server" Text='<%# Eval("POUserName") %>'></asp:label><span class="indent">(<asp:label id="POLocation" runat="server" Text='<%# Eval("POLocationName") %>'></asp:label>)</span></td>
                        </tr>
                        <tr>
                            <th>Supplier Name</th>
                            <td><asp:label id="SupplierName" runat="server" Text='<%# Eval("SupplierName") %>'></asp:label></td>
                            <th>Maker Name</th>
                            <td colspan="3"><asp:label id="MakerName" runat="server" Text='<%# Eval("MakerName") %>'></asp:label></td>
                        </tr>
                        <tr>
                            <th>Delivery Date</th>
                            <td><asp:label id="DeliveryDate" runat="server" Text='<%#If(IsDBNull(Eval("DeliveryDate")), Eval("DeliveryDate"), Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("DeliveryDate"), False, False))%>'></asp:label></td>
                            <th>Order Quantity</th>
                            <td><asp:label id="OrderQuantity" runat="server" Text='<%# Eval("OrderQuantity","{0:G29}") %>'></asp:label> <asp:label id="OrderUnit" runat="server" Text='<%# Eval("OrderUnitCode") %>'></asp:label></td>
                            <th>Price</th>
                            <td><asp:label id="Currency" runat="server" Text='<%# Eval("CurrencyCode") %>'></asp:label> <asp:label id="UnitPrice" runat="server" Text='<%# Eval("UnitPrice","{0:G29}") %>'></asp:label> / <asp:label id="PerQuantity" runat="server" Text='<%# Eval("PerQuantity","{0:G29}") %>'></asp:label> <asp:label id="PerUnit" runat="server" Text='<%# Eval("PerUnitCode") %>'></asp:label></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:ListView>
            <asp:ListView ID="POList_Par" runat="server">
                <LayoutTemplate>
                    <div ID="itemPlaceholderContainer" runat="server">
                        <div ID="itemPlaceholder" runat="server">
                        </div>
                    </div>
                </LayoutTemplate>
                <ItemTemplate>
                    <table>
                        <tr>
                            <th class="subhead" colspan="2">
                                <!-- １段下に改行されて表示される現象の対策のため「PO Number」の前に記述する -->
                                <span class="placedright"><asp:label id="Priority_Title_PO" runat="server" Text="Priority : " Visible='<%#IIF(Eval("Priority")="", False,True) %>' CssClass='<%#IIF(Eval("Priority")="B", "priorityB", "priorityA") %>'></asp:label><asp:label id="Priority_PO" runat="server" Text='<%# Eval("Priority") %>' CssClass='<%#IIF(Eval("Priority")="B", "priorityB", "priorityA") %>'></asp:label></span>
                                PO Number : <asp:HyperLink ID="POUpdate" runat="server" NavigateUrl='<%# "./POUpdate.aspx?PONumber=" & Eval("PONumber") %>'><asp:label id="PONumber" runat="server" Text='<%# Eval("PONumber") %>'></asp:label></asp:HyperLink><span class="indent"><em><asp:label id="POCorrespondence" runat="server" Text='<%# Eval("POCorrespondence") %>'></asp:label></em></span>
                            </th>
                            <th class="subhead" colspan="4">
                                <asp:label id="POStatusChangeDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("StatusChangeDate"), True, False)%>'></asp:label>
                                <span class="indent"><asp:label id="POStatus" runat="server" Text='<%# Eval("StatusCode") %>'></asp:label></span>
                                <span class="indent"><asp:Label ID="POConfidential" runat="server" Text='<%#IIF(Eval("isCONFIDENTIAL")=True,Purchase.Common.CONFIDENTIAL,"") %>' CssClass="confidential"></asp:Label></span>
                            </th>
                        </tr>
                        <tr>
                            <th style="width:17%">Product Number / Name</th>
                            <td style="width:33%"><asp:label id="ProductNumber" runat="server" Text='<%# Eval("ProductNumber") %>'></asp:label><span class="indent"><asp:label id="ProductName" runat="server" Text='<%#Purchase.Common.CutShort(Eval("ProductName").ToString())%>'></asp:label></span></td>
                            <th style="width:10%">PO Date</th>
                            <td style="width:12%"><asp:label id="PODate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("PODate"), False, False)%>'></asp:label></td>
                            <th style="width:10%">PO-User</th>
                            <td style="width:18%"><asp:label id="POUser" runat="server" Text='<%# Eval("POUserName") %>'></asp:label><span class="indent">(<asp:label id="POLocation" runat="server" Text='<%# Eval("POLocationName") %>'></asp:label>)</span></td>
                        </tr>
                        <tr>
                            <th>Supplier Name</th>
                            <td><asp:label id="SupplierName" runat="server" Text='<%# Eval("SupplierName") %>'></asp:label></td>
                            <th>Maker Name</th>
                            <td colspan="3"><asp:label id="MakerName" runat="server" Text='<%# Eval("MakerName") %>'></asp:label></td>
                        </tr>
                        <tr>
                            <th>Delivery Date</th>
                            <td><asp:label id="DeliveryDate" runat="server" Text='<%#If(IsDBNull(Eval("DeliveryDate")), Eval("DeliveryDate"), Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("DeliveryDate"), False, False))%>'></asp:label></td>
                            <th>Order Quantity</th>
                            <td><asp:label id="OrderQuantity" runat="server" Text='<%# Eval("OrderQuantity","{0:G29}") %>'></asp:label> <asp:label id="OrderUnit" runat="server" Text='<%# Eval("OrderUnitCode") %>'></asp:label></td>
                            <th>Price</th>
                            <td><asp:label id="Currency" runat="server" Text='<%# Eval("CurrencyCode") %>'></asp:label> <asp:label id="UnitPrice" runat="server" Text='<%# Eval("UnitPrice","{0:G29}") %>'></asp:label> / <asp:label id="PerQuantity" runat="server" Text='<%# Eval("PerQuantity","{0:G29}") %>'></asp:label> <asp:label id="PerUnit" runat="server" Text='<%# Eval("PerUnitCode") %>'></asp:label></td>
                        </tr>
                    </table>
                        <asp:ListView ID="POList_Chi" runat="server">
                        <LayoutTemplate>
                            <div ID="itemPlaceholderContainer" runat="server">
                                <div ID="itemPlaceholder" runat="server">
                                </div>
                            </div>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <table class="child">
                                <tr>
                                    <th class="subhead" colspan="2">
                                        Chi-PO Number : <asp:HyperLink ID="POUpdate" runat="server" NavigateUrl='<%# "./POUpdate.aspx?PONumber=" & Eval("PONumber") %>'><asp:label id="PONumber" runat="server" Text='<%# Eval("PONumber") %>'></asp:label></asp:HyperLink>
                                    </th>
                                    <th class="subhead" colspan="4"></th>
                                </tr>
                                <tr>
                                    <th style="width:17%">Product Number / Name</th>
                                    <td style="width:33%"><asp:label id="ProductNumber" runat="server" Text='<%# Eval("ProductNumber") %>'></asp:label><span class="indent"><asp:label id="ProductName" runat="server" Text='<%#Purchase.Common.CutShort(Eval("ProductName").ToString())%>'></asp:label></span></td>
                                    <th style="width:10%">PO Date</th>
                                    <td style="width:12%"><asp:label id="PODate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("PODate"), False, False)%>'></asp:label></td>
                                    <th style="width:10%">PO-User</th>
                                    <td style="width:18%"><asp:label id="POUser" runat="server" Text='<%# Eval("POUserName") %>'></asp:label><span class="indent">(<asp:label id="POLocation" runat="server" Text='<%# Eval("POLocationName") %>'></asp:label>)</span></td>
                                </tr>
                                <tr>
                                    <th>Supplier Name</th>
                                    <td><asp:label id="SupplierName" runat="server" Text='<%# Eval("SupplierName") %>'></asp:label></td>
                                    <th>Maker Name</th>
                                    <td colspan="3"><asp:label id="MakerName" runat="server" Text='<%# Eval("MakerName") %>'></asp:label></td>
                                </tr>
                                <tr>
                                    <th>Delivery Date</th>
                                    <td><asp:label id="DeliveryDate" runat="server" Text='<%#If(IsDBNull(Eval("DeliveryDate")), Eval("DeliveryDate"), Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("DeliveryDate"), False, False))%>'></asp:label></td>
                                    <th>Order Quantity</th>
                                    <td><asp:label id="OrderQuantity" runat="server" Text='<%# Eval("OrderQuantity","{0:G29}") %>'></asp:label> <asp:label id="OrderUnit" runat="server" Text='<%# Eval("OrderUnitCode") %>'></asp:label></td>
                                    <th>Price</th>
                                    <td><asp:label id="Currency" runat="server" Text='<%# Eval("CurrencyCode") %>'></asp:label> <asp:label id="UnitPrice" runat="server" Text='<%# Eval("UnitPrice","{0:G29}") %>'></asp:label> / <asp:label id="PerQuantity" runat="server" Text='<%# Eval("PerQuantity","{0:G29}") %>'></asp:label> <asp:label id="PerUnit" runat="server" Text='<%# Eval("PerUnitCode") %>'></asp:label></td>
                                </tr>
                            </table>
                        </ItemTemplate>
                        <EmptyDataTemplate></EmptyDataTemplate>
                    </asp:ListView>
                    <asp:SqlDataSource ID="SrcPO_Chi" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                </ItemTemplate>
            </asp:ListView>
        <%  If POList_Overdue.Items.Count + POList_PPI.Items.Count + POList_Par.Items.Count <= 0 Then%>
            <h3 style="font-style:italic"><%=Purchase.Common.MSG_NO_DATA_FOUND%></h3>
        <% End If%>
        </div>
<%End If%>
        </form>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcRFQ" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SrcPO_Overdue" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SrcPO_PPI" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SrcPO_Par" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

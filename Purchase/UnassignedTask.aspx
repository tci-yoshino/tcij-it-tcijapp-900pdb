<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UnassignedTask.aspx.vb" Inherits="Purchase.UnassignedTask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>
    <!-- Sub Navigation -->
    <div id="subNavi">
        <a href="./MyTask.aspx">My Tasks</a> | <a href="./RequestedTask.aspx">Requested Tasks</a> | <a href="./UnassignedTask.aspx" class="current">Unassigned Tasks</a>
    </div><!-- Sub Navigation END -->

    <!-- Main Content Area -->
    <div id="content">
    <form ID="list" runat="server" action="">
        <h3>RFQ</h3>

        <div class="list">
            <p class="attention"><asp:Label ID="Msg" runat="server"></asp:Label></p>
            <asp:ListView ID="RFQList" runat="server" DataSourceID="SrcRFQ" DataKeyNames="RFQNumber">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <h3 style="font-style:italic"><%=Purchase.Common.MSG_NO_DATA_FOUND %></h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr>
                        <th class="subhead" colspan="2">
                            RFQ Reference Number : <asp:HyperLink ID="RFQUpdate" runat="server" NavigateUrl='<%# "./RFQUpdate.aspx?RFQNumber=" & Eval("RFQNumber")%>'><asp:Label ID="RFQNumber" runat="server" Text='<%#Eval("RFQNumber")%>'></asp:Label></asp:HyperLink>
                        </th>
                        <th class="subhead" colspan="2">
                            <asp:Label ID="RFQStatusChangeDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("StatusChangeDate"))%>'></asp:Label><span class="indent"><asp:Label ID="RFQStatus" runat="server" Text='<%#Eval("Status")%>'></asp:Label></span>
                        </th>
                        <th class="subhead" colspan="2" style="text-align:right">
                            Assign to : <asp:DropDownList ID="QuoUser" runat="server" DataSourceID="SrcUser" DataTextField="Name" DataValueField="UserID"></asp:DropDownList>
                            <asp:HiddenField runat="server" ID="UpdateDate" Value="" />
                            <asp:Button ID="Assign" runat="server" Text="Assign" CommandArgument="" />
                        </th>
                    </tr>
                    <tr>
                        <th style="width:17%">Product Number / Name</th>
                        <td style="width:33%">
                            <asp:Label ID="ProductNumber" runat="server" Text='<%#Eval("ProductNumber")%>'></asp:Label>
                            <span class="indent"><asp:Label ID="ProductName" runat="server" Text='<%#Purchase.Common.CutShort(Eval("ProductName")) %>'></asp:Label></span>
                        </td>
                        <th style="width:10%">Purpose</th>
                        <td style="width:12%"><asp:Label ID="Purpose" runat="server" Text='<%#Eval("Purpose")%>'></asp:Label></td>
                        <th style="width:10%">Enq-User</th>
                        <td style="width:18%">
                            <asp:Label ID="EnqUser" runat="server" Text='<%#Eval("EnqUserName")%>'></asp:Label>
                            <span class="indent">(<asp:Label ID="EnqLocation" runat="server" Text='<%#Eval("EnqLocationName")%>'></asp:Label>)</span>
                        </td>
                    </tr>
                    <tr>
                        <th>Supplier Name</th>
                        <td><asp:Label ID="SupplierName" runat="server" Text='<%#Eval("SupplierName")%>'></asp:Label></td>
                        <th>Maker Name</th>
                        <td colspan="3"><asp:Label ID="MakerName" runat="server" Text='<%#Eval("MakerName")%>'></asp:Label></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>

        <hr />

        <h3>PO</h3>

        <div class="list">
            <asp:ListView ID="POList" runat="server" DataSourceID="SrcPO">
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
                            PO Number : <asp:HyperLink ID="POUpdate" runat="server" NavigateUrl='<%# "./POUpdate.aspx?PONumber=" & Eval("PONumber")%>'><asp:Label ID="PONumber" runat="server" Text='<%#Eval("PONumber")%>'></asp:Label></asp:HyperLink>
                        </th>
                        <th class="subhead">
                            <asp:Label ID="POStatusChangeDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("StatusChangeDate"))%>'></asp:Label>
                            <span class="indent"><asp:Label ID="POStatus" runat="server" Text='<%#Eval("StatusCode")%>'></asp:Label></span>
                        </th>
                        <th class="subhead" style="text-align:right">
                            Assign to : <asp:DropDownList ID="SOUser" runat="server" DataSourceID="SrcUser" DataTextField="Name" DataValueField="UserID"></asp:DropDownList>
                            <asp:HiddenField runat="server" ID="UpdateDate" Value="" />
                            <asp:Button ID="Assign" runat="server" Text="Assign" />
                        </th>
                    </tr>
                    <tr>
                        <th style="width:17%">Product Number / Name</th>
                        <td style="width:33%"><asp:Label ID="ProductNumber" runat="server" Text='<%#Eval("ProductNumber")%>'></asp:Label><span class="indent"><asp:Label ID="ProductName" runat="server" Text='<%#Purchase.Common.CutShort(Eval("ProductName").ToString()) %>'></asp:Label></span></td>
                        <th style="width:17%">PO-User</th>
                        <td style="width:33%"><asp:Label ID="POUser" runat="server" Text='<%#Eval("POUserName")%>'></asp:Label><span class="indent">(<asp:Label ID="POLocation" runat="server" Text='<%#Eval("POLocationName")%>'></asp:Label>)</span></td>
                    </tr>
                    <tr>
                        <th>Supplier Name</th>
                        <td><asp:Label ID="SupplierName" runat="server" Text='<%#Eval("SupplierName")%>'></asp:Label></td>
                        <th>Maker Name</th>
                        <td><asp:Label ID="MakerName" runat="server" Text='<%#Eval("MakerName")%>'></asp:Label></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
        <asp:HiddenField runat="server" ID="Action" Value="Assign" />
    </form>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcRFQ" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SrcPO" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
	<asp:SqlDataSource ID="SrcUser" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RequestedTask.aspx.vb" Inherits="Purchase.RequestedTask" %>

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
        <a href="./MyTask.aspx">My Tasks</a> | <a href="./RequestedTask.aspx" class="current">Requested Tasks</a> | <a href="./UnassignedTask.aspx">Unassigned Tasks</a>
    </div><!-- Sub Navigation END -->

    <!-- Main Content Area -->
    <div id="content">
        <h3>RFQ</h3>

        <div class="list">
            <asp:ListView ID="RFQList" runat="server" DataSourceID="SrcRFQ">
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
                            RFQ Reference Number : <asp:HyperLink ID="RFQUpdate" runat="server" NavigateUrl='<%# "./RFQUpdate.aspx?RFQNumber=" & Eval("RFQNumber")%>'><asp:label id="RFQNumber" runat="server" Text='<%# Eval("RFQNumber")%>'></asp:label></asp:HyperLink>
                            <span class="indent"><em><asp:label id="RFQCorrespondence" runat="server" Text='<%# Eval("RFQCorrespondence")%>'></asp:label></em></span>
                        </th>
                        <th class="subhead" colspan="4">
                            <asp:label id="RFQStatusChangeDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("StatusChangeDate"))%>'></asp:label><span class="indent"><asp:label id="RFQStatus" runat="server" Text='<%# Eval("Status")%>'></asp:label></span>
                        </th>
                    </tr>
                    <tr>
                        <th style="width:17%">Product Number / Name</th>
                        <td style="width:33%"><asp:label id="ProductNumber" runat="server" Text='<%# Eval("ProductNumber")%>'></asp:label><span class="indent"><asp:label id="ProductName" runat="server" Text='<%#Purchase.Common.CutShort(Eval("ProductName").ToString)%>'></asp:label></span></td>
                        <th style="width:10%">Purpose</th>
                        <td style="width:12%"><asp:label id="Purpose" runat="server" Text='<%# Eval("Purpose")%>'></asp:label></td>
                        <th style="width:10%">Quo-User</th>
                        <td style="width:18%"><asp:label id="QuoUser" runat="server" Text='<%# Eval("QuoUserName")%>'></asp:label><span class="indent">(<asp:label id="QuoLocation" runat="server" Text='<%# Eval("QuoLocationName")%>'></asp:label>)</span></td>
                    </tr>
                    <tr>
                        <th>Supplier Name</th>
                        <td><asp:label id="SupplierName" runat="server" Text='<%# Eval("SupplierName")%>'></asp:label></td>
                        <th>Maker Name</th>
                        <td colspan="3"><asp:label id="MakerName" runat="server" Text='<%# Eval("MakerName")%>'></asp:label></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcRFQ" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

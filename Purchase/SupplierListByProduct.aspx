<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SupplierListByProduct.aspx.vb" Inherits="Purchase.SupplierListByProduct" %>

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
        <div class="tabs"><a href="./SuppliersProductSetting.aspx">New Suppliers Product</a></div>

        <h3>Supplier List</h3>

        <div class="main">
            <p class="attention"></p>
            
            <table>
                <tr>
                    <th>Product Number : </th>
                    <td><asp:Label ID="ProductNumber" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th>Product Name : </th>
                    <td><asp:Label ID="ProductName" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
        </div>

        <div class="list">
            <asp:ListView ID="SupplierProductList" runat="server" DataSourceID="SrcSupplierProduct">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr>
                            <th style="width:12%">Supplier Code</th>
                            <th style="width:38%">Supplier Name</th>
                            <th style="width:10%">Supplier Item Number</th>
                            <th style="width:20%">Note</th>
                            <th style="width:8%">Update Date</th>
                            <th style="width:6%">Edit</th>
                            <th style="width:6%">Delete</th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <h3 style="font-style:italic">No data found.</h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr>
                        <td><asp:Label ID="SupplierCode" runat="server" Text='' /></td>
                        <td><asp:Label ID="SupplierName" runat="server" Text='' /></td>
                        <td><asp:Label ID="SupplierItemNumber" runat="server" Text='' /></td>
                        <td><asp:Label ID="Note" runat="server" Text='' /></td>
                        <td><asp:Label ID="UpdateDate" runat="server" Text='' /></td>
                        <td><asp:HyperLink ID="Edit" runat="server" NavigateUrl=''>Edit</asp:HyperLink></td>
                        <td><asp:HyperLink ID="Delete" runat="server" NavigateUrl=''>Delete</asp:HyperLink></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcSupplierProduct" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

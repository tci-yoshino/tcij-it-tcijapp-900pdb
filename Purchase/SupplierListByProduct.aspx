﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SupplierListByProduct.aspx.vb" Inherits="Purchase.SupplierListByProduct" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
    <script type="text/javascript">
<!--
function deleteLine(code) {
    if (confirm("It can't be restored once deleted.\nAre you sure to delete this entry?")) {
      document.forms["DeleteForm"].SupplierCode.value = code;
      document.forms["DeleteForm"].submit();
      return true;
    }
}
//-->
    </script>
</head>
<body>
    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"><a href="<%=AddUrl %>">New Suppliers Product</a></div>

        <h3>Supplier List</h3>

        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
            
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

        <hr />

        <div class="list">
            <form id="DeleteForm" runat="server" action="" method="post">
                <input type="hidden" runat="server" id="ProductID" value='<%=ProductID  %>' />
                <input type="hidden" runat="server" id="SupplierCode" />
                <input type="hidden" runat="server" id="Action" value="Delete" />
            </form>
        
            <asp:ListView ID="SupplierProductList" runat="server" DataSourceID="SrcSupplierProduct">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr>
                            <th style="width:12%">Supplier Code</th>
                            <th style="width:28%">Supplier Name</th>
                            <th style="width:10%">Country</th>
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
                    <h3 style="font-style:italic"><% =Purchase.Common.MSG_NO_DATA_FOUND%></h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr>
                        <td><asp:HyperLink ID="SupplierCode" runat="server" NavigateUrl='<%#Eval("SupplierCode","./SupplierSetting.aspx?Action=Edit&Code={0}")%>' Text = '<%#Eval("SupplierCode")%>' /></td>
                        <td><asp:HyperLink ID="SupplierName" runat="server" NavigateUrl='<%#Eval("SupplierCode","./SupplierSetting.aspx?Action=Edit&Code={0}")%>' Text = '<%#Eval("SupplierName")%>' /></td>
                        <td><asp:Label ID="CountryName" runat="server" Text='<%# Eval("CountryName") %>' /></td>
                        <td><asp:Label ID="SupplierItemNumber" runat="server" Text='<%# Eval("SupplierItemNumber") %>' /></td>
                        <td><asp:Label ID="Note" runat="server" Text='<%# Eval("Note") %>' /></td>
                        <td><asp:Label ID="UpdateDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("UpdateDate"), True, false)%>' /></td>
                        <td><asp:HyperLink ID="Edit" runat="server" NavigateUrl='<%# Eval("Url") %>'>Edit</asp:HyperLink></td>
                        <td><asp:HyperLink ID="Delete" runat="server" NavigateUrl='<%# "javascript:deleteLine(" & Eval("SupplierCode") & ");" %>'>Delete</asp:HyperLink></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcSupplierProduct" runat="server" 
    ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
    
        SelectCommand="SELECT                  dbo.Supplier_Product.SupplierCode, ISNULL(dbo.Supplier.Name3, '') + N' ' + ISNULL(dbo.Supplier.Name4, '') AS SupplierName, 
                                  dbo.Supplier_Product.SupplierItemNumber, dbo.Supplier_Product.Note, REPLACE(CONVERT(char, dbo.Supplier_Product.UpdateDate, 111), 
                                  '/', '-') AS UpdateDate, 
                                  './SuppliersProductSetting.aspx?Action=Edit&amp;Supplier=' + RTRIM(LTRIM(STR(dbo.Supplier_Product.SupplierCode))) 
                                  + '&amp;Product=&quot; + Request.QueryString(&quot;ProductID&quot;) + &quot;&amp;Return=SP' AS Url, 
                                  './SuppliersProductSetting.aspx?Action=Delete&amp;Supplier=' + RTRIM(LTRIM(STR(dbo.Supplier_Product.SupplierCode))) 
                                  + '&amp;ProductID=&quot; + Request.QueryString(&quot;ProductID&quot;) + &quot;' AS DelUrl
FROM                     dbo.Supplier_Product LEFT OUTER JOIN
                                  dbo.Supplier ON dbo.Supplier_Product.SupplierCode = dbo.Supplier.SupplierCode"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

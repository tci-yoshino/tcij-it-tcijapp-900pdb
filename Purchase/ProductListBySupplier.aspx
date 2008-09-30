<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProductListBySupplier.aspx.vb" Inherits="Purchase.ProductListBySupplier" %>

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
      document.forms["DeleteForm"].ProductID.value = code;
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
        <div class="tabs"><a href="<%=AddUrl %>">New Suppliers Product</a> | <a href="<%=ImpUrl %>">Excel Import</a></div>

        <h3>Suppliers Product</h3>

        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
            
            <table>
                <tr>
                    <th>Supplier Code : </th>
                    <td><asp:Label ID="SupplierCode" runat="server" Text=""></asp:Label></td>
                </tr>
                <tr>
                    <th>Supplier Name : </th>
                    <td><asp:Label ID="SupplierName" runat="server" Text=""></asp:Label></td>
                </tr>
            </table>
        </div>

        <hr />

        <div class="list">
            <form id="DeleteForm" runat="server" action="" method="post">
                <input type="hidden" runat="server" id="ProductID" />
                <input type="hidden" runat="server" id="Action" value="Delete" />
            </form>
            
            <asp:ListView ID="SupplierProductList" runat="server" DataSourceID="SrcSupplierProduct">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr>
                            <th style="width:12%">Product Number</th>
                            <th style="width:38%">Product Name</th>
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
                    <h3 style="font-style:italic"><% =Purchase.Common.ERR_NO_MATCH_FOUND%></h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr>
                        <td><asp:Label ID="ProductNumber" runat="server" Text='<%# Eval("ProductNumber") %>' /></td>
                        <td><asp:Label ID="ProductName" runat="server" Text='<%# Eval("ProductName") %>' /></td>
                        <td><asp:Label ID="SupplierItemNumber" runat="server" Text='<%# Eval("SupplierItemNumber") %>' /></td>
                        <td><asp:Label ID="Note" runat="server" Text='<%# Eval("Note") %>' /></td>
                        <td><asp:Label ID="UpdateDate" runat="server" Text='<%#Left(Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("UpdateDate"), True), 10)%>' /></td>
                        <td><asp:HyperLink ID="Edit" runat="server" NavigateUrl='<%# Eval("Url") %>'>Edit</asp:HyperLink></td>
                        <td>
                            <asp:HyperLink ID="Delete" runat="server" NavigateUrl='<%# "javascript:deleteLine(" & Eval("ProductID") & ");" %>'>Delete</asp:HyperLink>
                            
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcSupplierProduct" runat="server" 
    ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
    
        
        SelectCommand="SELECT                  dbo.Product.ProductID, dbo.Product.ProductNumber, CASE WHEN NOT Product .QuoName IS NULL 
                                  THEN Product .QuoName ELSE Product .Name END AS ProductName, dbo.Supplier_Product.SupplierItemNumber, 
                                  dbo.Supplier_Product.Note, REPLACE(CONVERT(char, dbo.Supplier_Product.UpdateDate, 111), '/', '-') AS UpdateDate, 
                                  './SuppliersProductSetting.aspx?Action=Edit&amp;Supplier=&quot; + SupplierCode.Text.ToString + &quot;&amp;Product=' + RTRIM(LTRIM(STR(dbo.Product.ProductID)))
                                   AS Url, 
                                  './ProductListBySupplier.aspx?Action=Delete&amp;Supplier=&quot; + SupplierCode.Text.ToString + &quot;&amp;ProductID=' + RTRIM(LTRIM(STR(dbo.Product.ProductID)))
                                   AS DelUrl
FROM                     dbo.Supplier_Product LEFT OUTER JOIN
                                  dbo.Product ON dbo.Supplier_Product.ProductID = dbo.Product.ProductID"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
  <!--   -->
</body>
</html>

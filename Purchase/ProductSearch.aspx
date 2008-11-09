<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProductSearch.aspx.vb" Inherits="Purchase.ProductSearch" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
   <script type="text/javascript">
    <!--

window.onload = function() {
   colorful.set();
   document.SearchForm.ProductNumber.focus();
}
    -->
    </script>
</head>
<body>
    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"><a href="./ProductSetting.aspx">New Product</a></div>

        <h3>Product Search</h3>

        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server"></asp:Label></p>
            
            <form id="SearchForm" runat="server">
            <input id="Dummy" type="text" style = "display:none"/>
                <table>
                    <tr>
                        <th>Product Number <span class="required">*</span> : 
                            <asp:HiddenField ID="Action" runat="server" Value="Search" />
                        </th>
                        <td><asp:TextBox ID="ProductNumber" runat="server" Width="7em" MaxLength="32"></asp:TextBox></td>
                    </tr>
                </table>
                <asp:Button ID="Search" runat="server" Text="Search" />
                <input type="button" value="Clear" onclick="clearForm('SearchForm');" />
            </form>
        </div>

        <div class="note">
            Product Number could be:
            <ul>
                <li>TCI product number</li>
                <li>New item registry number</li>
                <li>CAS Number</li>
            </ul>(This CAS Product Number is only for data strage use, and can not be used to make enquiry record or issue PO.)
        </div>

        <hr />

        <div class="list">
            <asp:ListView ID="ProductList" runat="server" DataSourceID="SrcProduct">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server">
                        <tr id="Tr1" runat="server">
                            <th id="Th1" runat="server" style="width:15%">Product Number</th>
                            <th id="Th2" runat="server" style="width:80%">Product Name</th>
                            <th id="Th3" runat="server" style="width:5%"></th>
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
                        <td><asp:Label ID="ProductNumber" runat="server" Text='<%# Eval("[ProductNumber]") %>' /></td>
                        <td><asp:Label ID="ProductName" runat="server" Text='<%# Eval("[ProductName]") %>' /></td>
                        <td><asp:HyperLink ID="Edit" runat="server" NavigateUrl='<%# Eval("Url") %>'>Edit</asp:HyperLink></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcProduct" runat="server" 
    ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
    
    SelectCommand="SELECT ProductNumber, CASE WHEN NOT Product.QuoName IS NULL THEN Product.QuoName ELSE Product.Name END AS ProductName, CASNumber FROM dbo.Product">
</asp:SqlDataSource>
    
    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
    </body>
</html>

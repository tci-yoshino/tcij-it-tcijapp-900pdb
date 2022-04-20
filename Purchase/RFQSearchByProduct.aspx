<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQSearchByProduct.aspx.vb" Inherits="Purchase.RFQSearchByProduct" %>

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
   navi('product');
   document.SearchForm.Code.focus();
}
    -->
    </script>
</head>
<body>
    <!-- Header -->
    <commonUC:Header ID="HeaderMenu" runat="server" />
    <!-- Header End -->
    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"><a href="#" onclick = "popup('./ProductSetting.aspx')">New Product</a></div>

        <h3>RFQ Search by Product</h3>

        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server"></asp:Label></p>
            
            <form id="SearchForm" runat="server">
                <input id="Dummy" type="text" style = "display:none"/>
                <table>
                    <tr>
                        <th>Product Number <span class="required">*</span> : </th>
                        <td><asp:TextBox ID="Code" runat="server" Width="7em" MaxLength="32"></asp:TextBox></td>
                    </tr>
                </table>
                <asp:Button ID="Search" runat="server" Text="Search" />
                <input type="button" value="Clear" onclick="clearForm('SearchForm')"/>
                <asp:HiddenField ID="Action" runat="server" value="Search" />
            </form>
        </div>
        
        <div class="note">
            Product Number could be:
            <ul>
                <li>TCI product number</li>
                <li>New item registry number</li>
                <li>CAS Number</li>
            </ul>
        </div>

        <hr />

        <div class="list">
            <asp:ListView ID="ProductList" runat="server" DataSourceID="SrcProduct">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server">
                        <tr id="Tr1" runat="server">
                            <th id="Th1" runat="server" style="width:20%">Product Number</th>
                            <th id="Th2" runat="server" style="width:80%">Product Name</th>
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
                        <td>
                        <asp:HyperLink ID="ProductNumber" runat="server" NavigateUrl='<%#Eval("ProductID","./RFQListByProduct.aspx?ProductID={0}")%>' Text = '<%#Eval("ProductNumber")%>' />
                        </td>
                        <td>
                        <asp:HyperLink ID="ProductName" runat="server" NavigateUrl='<%#Eval("ProductID","./RFQListByProduct.aspx?ProductID={0}")%>' Text='<%#Eval("ProductName")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcProduct" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
    
    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

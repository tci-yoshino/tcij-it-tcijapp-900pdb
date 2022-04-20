<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProductSearchByKeyword.aspx.vb" Inherits="Purchase.ProductSearchByKeyword" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Purchase DB</title>
        <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />

        <script type="text/javascript" src="./JS/Common.js"></script>
        <script type="text/javascript" src="./JS/Colorful.js"></script>
        <script type="text/javascript">
            window.onload = function() {
               colorful.set();
               navi('product_search');
               document.SearchForm.ProductNumber.focus();
            }
        </script>
    </head>

    <body>
        <!-- Header -->
        <commonUC:Header ID="HeaderMenu" runat="server" />
        <!-- Header End -->
        <!-- Main Content Area -->
        <div id="content">
            <h3>Product Search By Keyword</h3>

            <form runat="server" id="SearchForm" >
                <div class="main">
                    <p class="attention"><asp:Label runat="server"  ID="Msg" Text=""></asp:Label></p>

                        <input id="Dummy" type="text" style = "display:none"/>
                        <asp:HiddenField runat="server" ID="Action" Value="Search" />
                        <table>
                            <tr>
                                <th>Product Number :  
                                    <asp:HiddenField runat="server" ID="st_ProductNumber" />
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="ProductNumber" Width="7em" MaxLength="32" />
                                </td>
                            </tr>
                            <tr>
                                <th>Product Name : 
                                    <asp:HiddenField runat="server" ID="st_ProductName" />
                                </th>
                                <td>
                                    <asp:TextBox runat="server" ID="ProductName" Width="20em" />(Partial text match)
                                </td>
                            </tr>
                        </table>
                        <asp:Button runat="server" ID="Search" Text="Search" OnClientClick="setAction('Search');" />
                        <!-- <input type="button" value="Clear" onclick="clearForm('SearchForm');" /> -->
                        <asp:Button runat="server" ID="Clear" Text="Clear" OnClientClick="setAction('Clear');" />
                </div>
                <hr />

                <div class="list">
                    <asp:ListView runat="server" ID="ProductList">
                        <LayoutTemplate>
                            <table runat="server" id="itemPlaceholderContainer" >
                                <tr runat="server" id="Tr1" >
                                    <th>Product Number</th>
                                    <th>CAS Number</th>
                                    <th>Product Name</th>
                                    <th></th>
                                    <th></th>
                                </tr>
                                <tr runat="server" ID="itemPlaceholder" >
                                </tr>
                            </table>
                        </LayoutTemplate>
 
                        <EmptyDataTemplate>
                            <h3 style="font-style:italic"><% =Purchase.Common.MSG_NO_DATA_FOUND%></h3>
                        </EmptyDataTemplate>

                        <ItemTemplate>
                            <tr runat="server">
                                <td>
                                    <asp:Label runat="server" ID="ProductNumber" 
                                        Text='<%#Eval("ProductNumber")%>' />

                                </td>
                                <td>
                                    <asp:Label runat="server" ID="CASNumber" 
                                        Text='<%#Eval("CASNumber")%>' />

                                </td>
                                <td>
                                    <asp:Label runat="server" ID="Name" 
                                        Text='<%#Eval("Name")%>' />

                                </td>
                                <td>
                                    <asp:HyperLink runat="server"  ID="Setting"
                                        NavigateUrl='<%#"./ProductSetting.aspx?Action=Edit&ProductID=" & Eval("ProductID") %>'>Product Setting</asp:HyperLink>

                                </td>
                                <td>
                                    <asp:HyperLink runat="server"  ID="RFQList"
                                        NavigateUrl='<%#"./RFQListByProduct.aspx?ProductID=" & Eval("ProductID") %>'>Product RFQ List</asp:HyperLink>

                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>

            </form>
        </div>
        <!-- Main Content Area END -->
        <!-- Footer -->
        <!--#include virtual="./Footer.html" -->
        <!-- Footer END -->
    </body>
</html>

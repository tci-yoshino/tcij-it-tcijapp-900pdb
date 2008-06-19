<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SuppliersProductSetting.aspx.vb" Inherits="Purchase.SuppliersProductSetting" %>

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
        <div class="tabs"></div>

        <h3>Suppliers Product</h3>

        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>

            <form id="SupplierProductForm" runat="server">
                <table>
                    <tr>
                        <th>Supplier Code <span class="required">*</span> : </th>
                        <td>
						    <asp:TextBox ID="SupplierCode" runat="server" Width="7em" MaxLength="10" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						    <asp:ImageButton ID="SupplierSelect" runat="server" ImageUrl="./Image/Search.gif" CssClass="magnify" OnClientClick="popup('./SupplierSelect.aspx')" />
                        </td>
                    </tr>
                    <tr>
                        <th>Supplier Name : </th>
                        <td>
                            <asp:TextBox ID="SupplierName" runat="server" Width="21em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Product Number <span class="required">*</span> : </th>
                        <td>
                            <asp:TextBox ID="ProductNumber" runat="server" Width="7em" MaxLength="32" ReadOnly="true" CssClass="readonly"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Product Name : </th>
                        <td>
                            <asp:TextBox ID="ProductName" runat="server" Width="21em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Supplier Item Number : </th>
                        <td>
                            <asp:TextBox ID="SupplierItemNumber" runat="server" Width="10em" MaxLength="128"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Note : </th>
                        <td>
                            <asp:TextBox ID="Note" runat="server" Width="21em" MaxLength="3000"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <div class="btns">
                    <asp:Button ID="Save" runat="server" Text="Save" />
                </div>
            </form>
        </div>
    </div><!-- Main Content Area END -->

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

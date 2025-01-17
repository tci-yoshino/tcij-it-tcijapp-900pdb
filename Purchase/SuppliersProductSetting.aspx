﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SuppliersProductSetting.aspx.vb" Inherits="Purchase.SuppliersProductSetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>
    <!-- Header -->
    <commonUC:Header ID="HeaderMenu" runat="server" />
    <!-- Header End -->

    <!-- Main Content Area -->
      <form id="SupplierProductForm" runat="server">
    <div id="content">
        <div class="tabs"></div>

        <h3>Suppliers Product</h3>

        <div class="main">

          
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>

                <table>
                    <tr>
                        <th>Supplier Code <span class="required">*</span> : </th>
                        <td>
						    <asp:TextBox ID="Supplier" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
						    <asp:ImageButton ID="SupplierSelect" runat="server" ImageUrl="./Image/Search.gif" CssClass="magnify" OnClientClick="return SupplierSelect_onclick()" />
                        </td>
                    </tr>
                    <tr>
                        <th>Supplier Name : </th>
                        <td>
                            <asp:TextBox ID="SupplierName" runat="server" Width="21em" ReadOnly="True" 
                                CssClass="readonly"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Product Number <span class="required">*</span> : </th>
                        <td>
                            <asp:TextBox ID="ProductNumber" runat="server" Width="7em" MaxLength="32"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Product Name : </th>
                        <td>
                            <asp:TextBox ID="ProductName" runat="server" Width="21em" ReadOnly="True" 
                                CssClass="readonly"></asp:TextBox>
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
                            <asp:TextBox ID="Note" runat="server" Width="21em" MaxLength="3000" 
                                Height="142px" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                </table>
                <asp:HiddenField ID="Product" runat="server" />
                
                <asp:HiddenField ID="UpdateDate" runat="server" />
                
                <asp:HiddenField ID="Action" runat="server" Value="Save" />
                
                <div class="btns">
                    <asp:Button ID="Save" runat="server" Text="Save" UseSubmitBehavior="false" />
                </div>

        </div>
    </div><!-- Main Content Area END -->

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
    </form>
		<script language ="javascript" type="text/javascript">
		    function SupplierSelect_onclick() {
        var Supplier = encodeURIComponent(document.getElementById('Supplier').value);
        popup('./SupplierSelect.aspx?Code=' + Supplier);
	    	return false;
		}
		</script>
</body>
</html>

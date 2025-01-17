﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProductSelect.aspx.vb" Inherits="Purchase.ProductSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8; IE=EmulateIE9" />
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
    <script type="text/javascript">
    <!--

window.onload = function() {
   colorful.set();
   changeCellColor("ProductList_itemPlaceholderContainer")
   document.SearchForm.ProductNumber.focus();
}
    function returnValues(code, name){
      if(opener){
        opener.document.getElementById('ProductNumber').value = code
        opener.document.getElementById('ProductName').value = name
        opener.document.getElementById('ProductNumber').onchange()
      }
      window.close();
    }

    -->
    </script>
</head>
<body>
	<!-- Main Content Area -->
	<div id="content">
		<div class="tabs"></div>

		<h3>Product Select</h3>

		<div class="main">
			<p class="attention"><asp:Label ID="Msg" runat="server"></asp:Label></p>

			<form id="SearchForm" runat="server">
				<table>
					<tr>
						<th>Product Number : </th>
						<td><asp:TextBox ID="ProductNumber" runat="server" Width="7em" MaxLength="32"></asp:TextBox></td>
					</tr>
					<tr>
						<th>CAS Number : </th>
						<td><asp:TextBox ID="CASNumber" runat="server" Width="7em" MaxLength="32"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Product Name : </th>
						<td><asp:TextBox ID="ProductName" runat="server" Width="21em" MaxLength="255"></asp:TextBox> (Partial text match)</td>
					</tr>
				</table>

				<asp:Button ID="Search" runat="server" Text="Search" PostBackUrl="ProductSelect.aspx?Action=Search" />
				<input type="button" value="Clear" onclick="clearForm('SearchForm')" />
			</form>
		</div>

		<hr />

        <div class="list">
            <asp:ListView ID="ProductList" runat="server" DataSourceID="SrcProduct">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server">
                        <tr id="Tr1" runat="server">
                            <th id="Th1" runat="server" style="width:30%">Product Number</th>
                            <th id="Th2" runat="server" style="width:70%">Product Name</th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <h3 style="font-style:italic"><%=Purchase.Common.ERR_NO_MATCH_FOUND%></h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr onclick="returnValues('<%#Eval("ProductNumber")%>','<%#Replace(Eval("ProductName").ToString(), "'", "\'")%>')">
                        <td><asp:Label ID="ProductNumber" runat="server" Text='<%#Eval("ProductNumber")%>' /></td>
                        <td><asp:Label ID="ProductName" runat="server" Text='<%#Eval("ProductName")%>' /></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcProduct" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

</body>
</html>

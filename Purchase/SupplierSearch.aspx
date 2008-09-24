<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SupplierSearch.aspx.vb" Inherits="Purchase.SupplierSearch" %>

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
   document.SearchForm.Code.focus();
}
    -->
    </script>
</head>
<body>

			<form id="SearchForm" runat="server">
	<!-- Main Content Area -->
	<div id="content">
		<div class="tabs"><a href="./SupplierSetting.aspx">New Supplier</a></div>
		<h3>Supplier Search</h3>
		<div class="main">
		    <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
				<table>
					<tr>
						<th>Supplier Code : </th>
						<td><asp:TextBox ID="Code" runat="server" Width="7em" MaxLength="5"></asp:TextBox></td>
					</tr>
					<tr>
						<th>TCI-J Supplier Code : 
                            <asp:HiddenField ID="Action" runat="server" Value="Search" />
                        </th>
						<td><asp:TextBox ID="R3Code" runat="server" Width="7em" MaxLength="10"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Supplier Name : </th>
						<td><asp:TextBox ID="Name" runat="server" Width="21em" MaxLength="255"></asp:TextBox> (Partial text match)</td>
					</tr>
				</table>
				<asp:Button ID="Search" runat="server" Text="Search" />
				<input type="button" value="Clear" onclick="clearForm('SearchForm');" />
		</div>
		<hr />
        <div class="list">
            <asp:ListView ID="SupplierList" runat="server" DataSourceID="SrcSupplier" DataKeyNames="Supplier Code">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server">
                        <tr id="Tr1" runat="server">
                            <th id="Th1" runat="server" style="width:15%">Supplier Code</th>
                            <th id="Th2" runat="server" style="width:15%">R/3 Supplier Code</th>
                            <th id="Th3" runat="server" style="width:70%">Supplier Name</th>
                            <th></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <EmptyDataTemplate>
                   <h3 style="font-style:italic"><%If IsPostBack = True Then%>No match found.<%End If%></h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr style="">
                        <td><asp:Label ID="Supplier_CodeLabel" runat="server" Text='<%# Eval("[Supplier Code]") %>' /></td>
                        <td><asp:Label ID="R_3_Supplier_CodeLabel" runat="server" Text='<%# Eval("[R/3 Supplier Code]") %>' /></td>
                        <td><asp:Label ID="Supplier_NameLabel" runat="server" Text='<%# Eval("[Supplier Name]") %>' /></td>
                        <td><asp:HyperLink ID="Edit" runat="server" NavigateUrl='<%# Eval("Url") %>'>Edit</asp:HyperLink></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcSupplier" runat="server" 
    ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" SelectCommand="SELECT SupplierCode AS [Supplier Code], R3SupplierCode AS [R/3 Supplier Code], ISNULL(Name3, '') + N' ' + ISNULL(Name4, '') AS [Supplier Name], './SupplierSetting.aspx?Action=Edit&Code=' + str([SupplierCode]) AS Url FROM dbo.Supplier">
    </asp:SqlDataSource>

	<!-- Footer -->
	<!--#include virtual="./Footer.html" --><!-- Footer END -->
			</form>
		</body>
</html>

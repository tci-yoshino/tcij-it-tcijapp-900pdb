<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SupplierSetting.aspx.vb" Inherits="Purchase.SupplierSetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>

			<form id="SupplierForm" runat="server">
	<!-- Main Content Area -->
	<div id="content">
		<div class="tabs">
			<asp:HiddenField ID="UpdateDate" runat="server" />
			<asp:HiddenField ID="Mode" runat="server" />
			<asp:HyperLink ID="SuppliersProduct" runat="server">Suppliers Product</asp:HyperLink>
		</div>

		<h3>Supplier Setting</h3>

		<div class="main">
			<p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>

				<table>
					<tr>
						<th>Supplier Code : </th>
						<td><asp:Label ID="Code" runat="server" Text=""></asp:Label></td>
					</tr>
					<tr>
						<th>TCI-J Supplier Code : </th>
						<td><asp:TextBox ID="R3SupplierCode" runat="server" Width="7em" MaxLength="10"></asp:TextBox></td>
					</tr>
					<tr>
						<th>TCI-J Supplier Name : </th>
						<td>
                            <asp:TextBox ID="SupplierName1" runat="server"  Width="21em" MaxLength="255"></asp:TextBox>
							<asp:TextBox ID="SupplierName2" runat="server"  Width="21em" MaxLength="255"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<th>Supplier Name <span class="required">*</span> : </th>
						<td>
							<asp:TextBox ID="SupplierName3" runat="server"  Width="21em" MaxLength="255"></asp:TextBox>
							<asp:TextBox ID="SupplierName4" runat="server"  Width="21em" MaxLength="255"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<th>Search Term : </th>
						<td>
							<asp:TextBox ID="SearchTerm1" runat="server"  Width="21em" MaxLength="255"></asp:TextBox>
							<asp:TextBox ID="SearchTerm2" runat="server"  Width="21em" MaxLength="255"></asp:TextBox>
						</td>
					</tr>
				</table>

				<table class="left" style="margin-left:21px">
					<tr>
						<th rowspan="3">Address <span class="required">*</span> : </th>
						<td><asp:TextBox ID="Address1" runat="server"  Width="21em" MaxLength="255"></asp:TextBox></td>
					</tr>
					<tr>
						<td><asp:TextBox ID="Address2" runat="server"  Width="21em" MaxLength="255"></asp:TextBox></td>
					</tr>
					<tr>
						<td><asp:TextBox ID="Address3" runat="server"  Width="21em" MaxLength="255"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Postal Code : </th>
						<td><asp:TextBox ID="PostalCode" runat="server" Width="7em" MaxLength="32"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Country <span class="required">*</span> : </th>
						<td>
                            <asp:DropDownList ID="Country" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
						</td>
					</tr>
					<tr>
						<th>Region : </th>
						<td>
                            <asp:DropDownList ID="Region" runat="server">
                            </asp:DropDownList>
						</td>
					</tr>
					<tr>
						<th>Telephone : </th>
						<td><asp:TextBox ID="Telephone" runat="server"  Width="10em" MaxLength="32"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Fax : </th>
						<td><asp:TextBox ID="Fax" runat="server"  Width="10em" MaxLength="32"></asp:TextBox></td>
					</tr>
				</table>

				<table>
					<tr>
						<th>E-mail : </th>
						<td><asp:TextBox ID="Email" runat="server"  Width="21em" MaxLength="255"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Website : </th>
						<td>http:// <asp:TextBox ID="Website" runat="server"  Width="21em" MaxLength="255"></asp:TextBox></td>
					</tr>
					<tr>
						<th>R/3 Comments : </th>
						<td><asp:TextBox ID="R3Comment" runat="server"  Width="21em" MaxLength="255"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Comments : </th>
						<td><asp:TextBox ID="Comment" runat="server" Columns="50" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Default Quo-Location : </th>
						<td>
                            <asp:DropDownList ID="DefaultQuoLocation" runat="server" 
                                AppendDataBoundItems="True">
                            </asp:DropDownList>
						</td>
					</tr>
				</table>
                <asp:HiddenField ID="Action" runat="server" value="Save" />

				<div class="btns">
				    <p class="message"><asp:Label ID="RunMsg" runat="server"></asp:Label></p>
                    <asp:Button ID="Save" runat="server" Text="Save" UseSubmitBehavior="False" />
				</div>
		</div>
	</div><!-- Main Content Area END -->

	<!-- Footer -->
	<!--#include virtual="./Footer.html" --><!-- Footer END -->
			</form>
		</body>
</html>

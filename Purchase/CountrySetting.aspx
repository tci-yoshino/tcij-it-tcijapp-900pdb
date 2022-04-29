<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CountrySetting.aspx.vb" Inherits="Purchase.CountrySetting" %>

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
	<form id="CountryForm" runat="server">
	<div id="content">
		<div class="tabs"></div>
		<h3>Country Setting</h3>
		<div class="main">
			<p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
				<table>
					<tr>
						<th>Country Code <span class="required">*</span> : </th>
						<td>
						    <asp:TextBox ID="Code" runat="server" Width="7em" MaxLength="5" ReadOnly="true" CssClass="readonly" TabIndex="1"></asp:TextBox>
						    <asp:ImageButton ID="Search" runat="server" ImageUrl="./Image/Search.gif" CssClass="magnify" OnClientClick="return Search_onclick()" />
						</td>
					</tr>
					<tr>
						<th>Country Name : </th>
						<td><asp:TextBox ID="Name" runat="server" Width="21em" MaxLength="255" 
                                ReadOnly="True" CssClass="readonly" TabIndex="2"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Default Quo-Location <span class="required">*</span> : </th>
						<td><asp:DropDownList ID="Location" runat="server"></asp:DropDownList></td>
					</tr>
				</table>
                <asp:HiddenField ID="Action" runat="server" value="Save" />
				<asp:HiddenField ID="UpdateDate" runat="server" />
				<div class="btns">
                    <asp:Button ID="Save" runat="server" Text="Save" />
				</div>
		</div>
	</div><!-- Main Content Area END -->

	<!-- Footer -->
	<!--#include virtual="./Footer.html" --><!-- Footer END -->	
	</form>
		<script language ="javascript" type="text/javascript">
		    function Search_onclick() {
		        var Code = encodeURIComponent(document.getElementById('Code').value);
		        popup('./CountrySelect.aspx?Code=' + Code);
		        return false;
		    }
		</script>		
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserSetting.aspx.vb" Inherits="Purchase.UserSetting" %>

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
	<form id="CountryForm" runat="server">
	<div id="content">
		<div class="tabs"></div>
		<h3>User Setting</h3>
		<div class="main">
			<p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
				<table>
					<tr>
						<th>User ID<span class="required">*</span> : </th>
						<td>
						    <asp:TextBox ID="UserID" runat="server" Width="7em" MaxLength="5" 
                                ReadOnly="true" CssClass="readonly" TabIndex="1"></asp:TextBox>
						    <asp:ImageButton ID="Search" runat="server" ImageUrl="./Image/Search.gif" CssClass="magnify" OnClientClick="return Search_onclick()" />
						</td>
					</tr>
					<tr>
						<th>Location : </th>
						<td>
						    <asp:TextBox ID="Location" runat="server" Width="7em" MaxLength="5" 
                                ReadOnly="true" CssClass="readonly" TabIndex="1"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Account Name : </th>
						<td><asp:TextBox ID="AccountName" runat="server" Width="21em" MaxLength="255" 
                                ReadOnly="True" CssClass="readonly" TabIndex="2"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Name : </th>
						<td><asp:TextBox ID="Name" runat="server" Width="21em" MaxLength="255" 
                                ReadOnly="True" CssClass="readonly" TabIndex="2"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Role : </th>
						<td><asp:DropDownList ID="RoleCode" runat="server"></asp:DropDownList></td>
					</tr>
					<tr>
						<th>Privilege Level : </th>
						<td><asp:DropDownList ID="PrivilegeLevel" runat="server"></asp:DropDownList></td>
					</tr>
				    <tr>
				       <th>is Admin : </th>
                        <td>
                            <asp:CheckBox ID="isAdmin" runat="server" />
                        </td>
                    </tr>
                    <tr>
				       <th>is Disabled : </th>
                        <td>
                            <asp:CheckBox ID="isDisabled" runat="server" />
                        </td>
                    </tr>
				</table>
                <asp:HiddenField ID="Mode" runat="server" value="" />
				<asp:HiddenField ID="UpdateDate" runat="server" />
				<asp:HiddenField ID="Action" runat="server" Value="Save" />
				<div class="btns">
                    <asp:Button ID="Save" runat="server" Text="Save"/>
				</div>
		</div>
	</div><!-- Main Content Area END -->

	<!-- Footer -->
	<!--#include virtual="./Footer.html" --><!-- Footer END -->	
	</form>
		<script language ="javascript" type="text/javascript">
		    function Search_onclick() {
		        var UserID = encodeURIComponent(document.getElementById('UserID').value);
		        popup('./UserSelect.aspx?UserID=' + UserID);
		        return false;
		    }
		</script>		
</body>
</html>

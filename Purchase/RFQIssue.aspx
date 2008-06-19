<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQIssue.aspx.vb" Inherits="Purchase.RFQIssue" %>

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

		<h3>RFQ Issue</h3>

		<form id="RFQForm" runat="server">
			<div class="main">
			    <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
			    
				<table class="left">
					<tr>
						<th>Enq-Location <span class="required">*</span> : </th>
						<td>
                            <asp:DropDownList ID="EnqLocation" runat="server">
                            </asp:DropDownList>
						</td>
					</tr>
					<tr>
						<th>Enq-User <span class="required">*</span> : </th>
						<td>
                            <asp:DropDownList ID="EnqUser" runat="server">
                            </asp:DropDownList>
						</td>
					</tr>
					<tr>
						<th>Product Number <span class="required">*</span> : </th>
						<td>
						    <asp:TextBox ID="ProductNumber" runat="server" Width="7em" MaxLength="10" ReadOnly="true" CssClass="readonly"></asp:TextBox> 
                            <asp:ImageButton ID="ProductSelect" runat="server" ImageUrl="./Image/Search.gif" CssClass="magnify" OnClientClick="popup('./ProductSelect.aspx')" />
                        </td>
					</tr>
					<tr>
						<th>Product Name <span class="required">*</span> : </th>
						<td><asp:TextBox ID="ProductName" runat="server" Width="21em" ReadOnly="true" CssClass="readonly"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Supplier Code <span class="required">*</span> : </th>
						<td>
						    <asp:TextBox ID="SupplierCode" runat="server" Width="7em" MaxLength="10" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						    <asp:ImageButton ID="SupplierSelect" runat="server" ImageUrl="./Image/Search.gif" CssClass="magnify" OnClientClick="popup('./SupplierSelect.aspx')" />
						</td>
					</tr>
					<tr>
						<th>R/3 Supplier Code : </th>
						<td><asp:TextBox ID="R3SupplierCode" runat="server" Width="7em" MaxLength="10" ReadOnly="true" CssClass="readonly"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Supplier Name / Country <span class="required">*</span> : </th>
						<td>
						    <asp:TextBox ID="SupplierName" runat="server" Width="21em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						    <asp:TextBox ID="SupplierCountry" runat="server" Width="4em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<th>Maker Code : </th>
						<td>
						    <asp:TextBox ID="MakerCode" runat="server" Width="7em" MaxLength="10" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						    <asp:ImageButton ID="MakerSelect" runat="server" ImageUrl="./Image/Search.gif" CssClass="magnify" OnClientClick="popup('./MakerSelect.aspx')" />
						</td>
					</tr>
					<tr>
						<th>Maker Name / Country : </th>
						<td>
						    <asp:TextBox ID="MakerName" runat="server" Width="21em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						    <asp:TextBox ID="MakerCountry" runat="server" Width="4em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						</td>
					</tr>
				</table>

				<table>
					<tr>
						<th>Quo-Location <span class="required">*</span> : </th>
						<td>
                            <asp:DropDownList ID="QuoLocation" runat="server">
                            </asp:DropDownList>
						</td>
					</tr>
					<tr>
						<th>Quo-User : </th>
						<td>
                            <asp:DropDownList ID="QuoUser" runat="server">
                            </asp:DropDownList>
						</td>
					</tr>
					<tr>
						<th>Purpose <span class="required">*</span> : </th>
						<td>
                            <asp:DropDownList ID="Purpose" runat="server">
                            </asp:DropDownList>
						</td>
					</tr>
					<tr>
						<th>Required Purity : </th><td><asp:TextBox ID="RequiredPurity" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Required QM Method : </th><td><asp:TextBox ID="RequiredQMMethod" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Required Specification : </th><td><asp:TextBox ID="RequiredSpecification" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Comment : </th>
						<td><asp:TextBox ID="Comment" runat="server" Columns="50" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
					</tr>
				</table>
			</div>

			<div class="list">
                <table style="width:50%">
                    <tr>
                        <th>No.</th>
                        <th>Enq-Quantity <span class="required">*</span></th>
                        <th>Supplier Item Number</th>
                    </tr>
                    <tr>
                        <th>1</th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_1" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_1" runat="server">
                            </asp:DropDownList>
                            x <asp:TextBox ID="EnqPiece_1" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                        </td>
                        <td><asp:TextBox ID="SupplierItemNumber_1" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>2</th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_2" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_2" runat="server">
                            </asp:DropDownList>
                            x <asp:TextBox ID="EnqPiece_2" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                        </td>
                        <td><asp:TextBox ID="SupplierItemNumber_2" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>3</th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_3" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_3" runat="server">
                            </asp:DropDownList>
                            x <asp:TextBox ID="EnqPiece_3" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                        </td>
                        <td><asp:TextBox ID="SupplierItemNumber_3" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>4</th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_4" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_4" runat="server">
                            </asp:DropDownList>
                            x <asp:TextBox ID="EnqPiece_4" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                        </td>
                        <td><asp:TextBox ID="SupplierItemNumber_4" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                    </tr>
                </table>

				<div class="btns">
                    <asp:Button ID="Issue" runat="server" Text="Issue" />
				</div>
			</div>
		</form>
	</div><!-- Main Content Area END -->
    
	<!-- Footer -->
	<!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQIssue.aspx.vb" Inherits="Purchase.RFQIssue" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>

		<form id="RFQForm" runat="server">
	<!-- Main Content Area -->
	<div id="content">
		<div class="tabs"></div>

		<h3>RFQ Issue</h3>

			<div class="main">
			    <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
			    
				<table class="left">
					<tr>
						<th>Enq-Location <span class="required">*</span> : </th>
						<td>
                            <asp:DropDownList ID="EnqLocation" runat="server" AutoPostBack="True" 
                                DataSourceID="SDS_RFQIssue_Loc" DataTextField="Name" 
                                DataValueField="LocationCode">
                            </asp:DropDownList>
						    <asp:SqlDataSource ID="SDS_RFQIssue_Loc" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                                SelectCommand="SELECT LocationCode, Name FROM s_Location ORDER BY Name">
                            </asp:SqlDataSource>
						</td>
					</tr>
					<tr>
						<th>Enq-User <span class="required">*</span> : </th>
						<td>
                            <asp:DropDownList ID="EnqUser" runat="server" DataSourceID="SDS_RFQIssue_Enq_U" 
                                DataTextField="Name" DataValueField="UserID">
                            </asp:DropDownList>
						    <asp:SqlDataSource ID="SDS_RFQIssue_Enq_U" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                                SelectCommand="SELECT [UserID], [Name] FROM [v_User] WHERE ([LocationCode] = @LocationCode) ORDER BY [Name]">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="EnqLocation" Name="LocationCode" 
                                        PropertyName="SelectedValue" Type="String" />
                                </SelectParameters>
                            </asp:SqlDataSource>
						</td>
					</tr>
					<tr>
						<th>Product Number <span class="required">*</span> : </th>
						<td>
						    <asp:TextBox ID="ProductNumber" runat="server" Width="7em" MaxLength="10"></asp:TextBox> 
                            <asp:ImageButton ID="ProductSelect" runat="server" 
                                ImageUrl="./Image/Search.gif" CssClass="magnify"  
                                OnClientClick="return ProductSelect_onclick()" />
                        </td>
					</tr>
					<tr>
						<th>Product Name : </th>
						<td><asp:TextBox ID="ProductName" runat="server" Width="21em" ReadOnly="True" 
                                CssClass="readonly" EnableViewState="False"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Supplier Code <span class="required">*</span> : </th>
						<td>
						    <asp:TextBox ID="SupplierCode" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
						    <asp:ImageButton ID="SupplierSelect" runat="server" ImageUrl="./Image/Search.gif" CssClass="magnify" OnClientClick="return SupplierSelect_onclick()" />
						</td>
					</tr>
					<tr>
						<th>R/3 Supplier Code : </th>
						<td><asp:TextBox ID="R3SupplierCode" runat="server" Width="7em" MaxLength="10" 
                                ReadOnly="true" CssClass="readonly" EnableViewState="False"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Supplier Name / Country : </th>
						<td>
						    <asp:TextBox ID="SupplierName" runat="server" Width="21em" ReadOnly="true" 
                                CssClass="readonly" EnableViewState="False"></asp:TextBox>
						    <asp:TextBox ID="SupplierCountry" runat="server" Width="4em" ReadOnly="true" 
                                CssClass="readonly" EnableViewState="False"></asp:TextBox>
						</td>
					</tr>
					<tr>
						<th>Maker Code : </th>
						<td>
						    <asp:TextBox ID="MakerCode" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
						    <asp:ImageButton ID="MakerSelect" runat="server" ImageUrl="./Image/Search.gif" CssClass="magnify" OnClientClick="return MakerSelect_onclick()" />
						</td>
					</tr>
					<tr>
						<th>Maker Name / Country : </th>
						<td>
						    <asp:TextBox ID="MakerName" runat="server" Width="21em" ReadOnly="true" 
                                CssClass="readonly" EnableViewState="False"></asp:TextBox>
						    <asp:TextBox ID="MakerCountry" runat="server" Width="4em" ReadOnly="true" 
                                CssClass="readonly" EnableViewState="False"></asp:TextBox>
						</td>
					</tr>
				</table>

				<table>
					<tr>
						<th>Quo-Location <span class="required">*</span> : </th>
						<td>
                            <asp:DropDownList ID="QuoLocation" runat="server" 
                                AutoPostBack="True" DataSourceID="SDS_RFQIssue_Loc" DataTextField="Name" 
                                DataValueField="LocationCode">
                                <asp:ListItem>Direct</asp:ListItem>
                            </asp:DropDownList>
						</td>
					</tr>
					<tr>
						<th>Quo-User : </th>
						<td>
                            <asp:DropDownList ID="QuoUser" runat="server" DataSourceID="SDS_RFQIssue_Que_U" 
                                DataTextField="Name" DataValueField="UserID">
                            </asp:DropDownList>
						    <asp:SqlDataSource ID="SDS_RFQIssue_Que_U" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                                SelectCommand="SELECT [UserID], [Name] FROM [v_User] WHERE ([LocationCode] = @LocationCode) ORDER BY [Name]">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="QuoLocation" Name="LocationCode" 
                                        PropertyName="SelectedValue" Type="String" />
                                </SelectParameters>
                            </asp:SqlDataSource>
						</td>
					</tr>
					<tr>
						<th>Purpose <span class="required">*</span> : </th>
						<td>
                            <asp:DropDownList ID="Purpose" runat="server" DataSourceID="SDS_RFQIssue_Pur" 
                                DataTextField="Text" DataValueField="PurposeCode">
                            </asp:DropDownList>
						    <asp:SqlDataSource ID="SDS_RFQIssue_Pur" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                                SelectCommand="SELECT [PurposeCode], [Text] FROM [Purpose] ORDER BY [SortOrder]">
                            </asp:SqlDataSource>
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
                            <asp:DropDownList ID="EnqUnit_1" runat="server" AppendDataBoundItems="True" 
                                DataSourceID="SDS_RFQIssue_Qua" DataTextField="UnitCode" 
                                DataValueField="UnitCode">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            x <asp:TextBox ID="EnqPiece_1" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                            <asp:SqlDataSource ID="SDS_RFQIssue_Qua" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                                SelectCommand="SELECT [UnitCode] FROM [PurchasingUnit] ORDER BY [UnitCode]">
                            </asp:SqlDataSource>
                        </td>
                        <td><asp:TextBox ID="SupplierItemNumber_1" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>2</th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_2" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_2" runat="server" AppendDataBoundItems="True" 
                                DataSourceID="SDS_RFQIssue_Qua" DataTextField="UnitCode" 
                                DataValueField="UnitCode">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            x <asp:TextBox ID="EnqPiece_2" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                        </td>
                        <td><asp:TextBox ID="SupplierItemNumber_2" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>3</th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_3" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_3" runat="server" AppendDataBoundItems="True" 
                                DataSourceID="SDS_RFQIssue_Qua" DataTextField="UnitCode" 
                                DataValueField="UnitCode">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            x <asp:TextBox ID="EnqPiece_3" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                        </td>
                        <td><asp:TextBox ID="SupplierItemNumber_3" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>4</th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_4" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_4" runat="server" AppendDataBoundItems="True" 
                                DataSourceID="SDS_RFQIssue_Qua" DataTextField="UnitCode" 
                                DataValueField="UnitCode">
                                <asp:ListItem></asp:ListItem>
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
	</div><!-- Main Content Area END -->
    
	<!-- Footer -->
	<!--#include virtual="./Footer.html" --><!-- Footer END -->
		</form>
		<script language ="javascript" type="text/javascript">
		function ProductSelect_onclick() {
    		var ProductNumber = encodeURIComponent(document.getElementById('ProductNumber').value);
	    	popup('./ProductSelect.aspx?ProductNumber=' + ProductNumber);
		}
		function SupplierSelect_onclick() {
    		var SupplierCode = encodeURIComponent(document.getElementById('SupplierCode').value);
    		var EnqLocation = encodeURIComponent(document.getElementById('EnqLocation').value);
	    	popup('./RFQSupplierSelect.aspx?Code=' + SupplierCode + '&Location=' + EnqLocation);
		}
		function MakerSelect_onclick() {
    		var MakerCode = encodeURIComponent(document.getElementById('MakerCode').value);
	    	popup('./MakerSelect.aspx?Code=' + MakerCode);
		}
		</script>
	</body>
</html>

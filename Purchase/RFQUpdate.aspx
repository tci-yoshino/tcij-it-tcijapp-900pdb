<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQUpdate.aspx.vb" Inherits="Purchase.RFQUpdate" %>

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
        <div class="tabs"><a href="#" onclick="return SupplierSetting_onclick()">Supplier Setting</a> | <a href="#" onclick="return Correspondence_onclick()">RFQ Correspondence / History</a></div>
        <h3>Quotation Reply</h3>

            <div class="main">
                <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
                <%  If Parameter = True Then%>
                <table class="left">
                    <tr>
                        <th>RFQ Reference Number : </th><td><asp:Label ID="RFQNumber" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Current Status : </th><td><asp:Label ID="CurrentRFQStatus" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Product Number / Name : </th><td><asp:Label ID="ProductNumber" runat="server" Text=""></asp:Label><span class="indent"><asp:Label ID="ProductName" runat="server" Text=""></asp:Label></span></td>
                    </tr>
					<tr>
						<th>Supplier Code <span class="required">*</span> : </th>
						<td>
						    <asp:TextBox ID="SupplierCode" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
						    <asp:ImageButton ID="SupplierSelect" runat="server" 
                                ImageUrl="./Image/Search.gif" CssClass="magnify" 
                                OnClientClick="return SupplierSelect_onclick()" />
                            <asp:LinkButton runat="server" ID="SupplierInfo" Text="Supplier Information" OnClientClick="return SupplierInfo_onclick()"/>
						</td>
					</tr>
					<tr>
						<th>TCI-J Supplier Code : </th>
						<td><asp:TextBox ID="R3SupplierCode" runat="server" Width="7em" MaxLength="10" ReadOnly="true" CssClass="readonly"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Supplier Name / Country : </th>
						<td>
						    <asp:TextBox ID="SupplierName" runat="server" Width="21em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						    <asp:TextBox ID="SupplierCountry" runat="server" Width="4em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						</td>
					</tr>
                    <tr>
                        <th>Supplier Contact Person : </th>
                        <td><asp:TextBox ID="SupplierContactPerson" runat="server" Width="21em" MaxLength="255"></asp:TextBox></td>
                    </tr>
					<tr>
						<th>Maker Code : </th>
						<td>
						    <asp:TextBox ID="MakerCode" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
						    <asp:ImageButton ID="MakerSelect" runat="server" ImageUrl="./Image/Search.gif" 
                                CssClass="magnify" OnClientClick="return MakerSelect_onclick()" />
                            <asp:LinkButton runat="server" ID="MakerInfo" Text="Supplier Information" OnClientClick="return MakerInfo_onclick()"/>
						</td>
					</tr>
					<tr>
						<th>Maker Name / Country : </th>
						<td>
						    <asp:TextBox ID="MakerName" runat="server" Width="21em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						    <asp:TextBox ID="MakerCountry" runat="server" Width="4em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
						</td>
					</tr>
                    <tr>
                        <th>Supplier Item Name : </th>
                        <td><asp:TextBox ID="SupplierItemName" runat="server" Width="21em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Payment Terms : </th>
                        <td>
                            <asp:DropDownList ID="PaymentTerm" runat="server" 
                                DataSourceID="SDS_RFQUpdate_PaymentTerms" DataTextField="Text" 
                                DataValueField="PaymentTermCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_PaymentTerms" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                                
                                SelectCommand="SELECT PaymentTermCode, Text FROM PurchasingPaymentTerm ORDER BY PaymentTermCode">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <th>Handling Fee / Shipment Cost : </th>
                        <td>
                            <asp:DropDownList ID="ShippingHandlingCurrency" runat="server" 
                                DataSourceID="SDS_RFQUpdate_Currency" DataTextField="CurrencyCode" 
                                DataValueField="CurrencyCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_Currency" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                                
                                SelectCommand="SELECT [CurrencyCode] FROM [PurchasingCurrency] ORDER BY [CurrencyCode]">
                            </asp:SqlDataSource>
                            <asp:TextBox ID="ShippingHandlingFee" runat="server" Width="5em" MaxLength="14" CssClass="number"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <table>
                    <tr>
                        <th>Purpose : </th><td><asp:Label ID="Purpose" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Required Purity : </th><td><asp:Label ID="RequiredPurity" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Required QM Method : </th><td><asp:Label ID="RequiredQMMethod" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Required Specification : </th><td><asp:Label ID="RequiredSpecification" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Spec Sheet : </th><td>
                        <asp:CheckBox ID="SpecSheet" runat="server" 
                            Text="yes" /></td>
                    </tr>
                    <tr>
                        <th>Specification : </th>
                        <td><asp:TextBox ID="Specification" runat="server" Columns="50" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Enq-User <span class="required">*</span> : </th>
                        <td>
                            <asp:DropDownList ID="EnqUser" runat="server" 
                                DataSourceID="SDS_RFQUpdate_EnqUser" DataTextField="Name" 
                                DataValueField="UserID">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_EnqUser" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                            (<asp:Label ID="EnqLocation" runat="server" Text=""></asp:Label>)
                        </td>
                    </tr>
                    <tr>
                        <th>Quo-User <span class="required">*</span> : </th>
                        <td>
                            <asp:DropDownList ID="QuoUser" runat="server" 
                                DataSourceID="SDS_RFQUpdate_QuoUser" DataTextField="Name" 
                                DataValueField="UserID">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_QuoUser" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                                
                                SelectCommand="SELECT UserID, Name FROM v_User WHERE (LocationName = @Location) ORDER BY Name ">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="QuoLocation" Name="Location" 
                                        PropertyName="Text" />
                                </SelectParameters>
                            </asp:SqlDataSource>
                            (<asp:Label ID="QuoLocation" runat="server" Text=""></asp:Label>)
                        </td>
                    </tr>
                    <tr>
                        <th>Comment : </th>
                        <td><asp:TextBox ID="Comment" runat="server" Columns="50" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                </table>
            </div>

            <div class="list">
                <table>
                    <tr>
                        <th style="width:3%" rowspan="2">No.</th>
                        <th>Enq-Quantity <span class="required">*</span></th>
                        <th>Currency</th>
                        <th>Price</th>
                        <th>Quo-Per</th>
                        <th>Quo-Unit</th>
                        <th>Lead Time</th>
                        <th>Supplier Item Number</th>
                        <th style="width:5%" rowspan="2">PO Issue</th>
                    </tr>
                    <tr>
                        <th>Incoterms</th>
                        <th colspan="2">Terms of Delivery</th>
                        <th>Purity</th>
                        <th>Method</th>
                        <th>Packing</th>
                        <th>Reason for "No Offer"</th>
                    </tr>
                    <tr>
                        <th rowspan="2">1</th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_1" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_1" runat="server" AppendDataBoundItems="True" 
                                DataSourceID="SDS_RFQUpdate_Qua" DataTextField="UnitCode" 
                                DataValueField="UnitCode" >
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            x <asp:TextBox ID="EnqPiece_1" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_Qua" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                                SelectCommand="SELECT [UnitCode] FROM [PurchasingUnit] ORDER BY [UnitCode]">
                            </asp:SqlDataSource></td>
                        <td>
                            <asp:DropDownList ID="Currency_1" runat="server" 
                                DataSourceID="SDS_RFQUpdate_Currency" DataTextField="CurrencyCode" 
                                DataValueField="CurrencyCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="UnitPrice_1" runat="server" Width="5em" MaxLength="14" CssClass="number"></asp:TextBox></td>
                        <td><asp:TextBox ID="QuoPer_1" runat="server" Width="5em" MaxLength="9" 
                                CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="QuoUnit_1" runat="server" 
                                DataSourceID="SDS_RFQUpdate_Unit" DataTextField="UnitCode" 
                                DataValueField="UnitCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_Unit" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                                
                                SelectCommand="SELECT [UnitCode] FROM [PurchasingUnit] ORDER BY [UnitCode]"></asp:SqlDataSource>
                        </td>
                        <td><asp:TextBox ID="LeadTime_1" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="SupplierItemNumber_1" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                        <td rowspan="2"><asp:HyperLink ID="POIssue_1" runat="server"  
                                NavigateUrl="./POIssue.aspx" Visible="False">PO Issue</asp:HyperLink>
                            <asp:HiddenField ID="LineNumber1" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="Incoterms_1" runat="server" 
                                DataSourceID="SDS_RFQUpdate_Incoterms" DataTextField="IncotermsCode" 
                                DataValueField="IncotermsCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_Incoterms" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                                
                                SelectCommand="SELECT IncotermsCode FROM s_Incoterms ORDER BY IncotermsCode "></asp:SqlDataSource>
                        </td>
                        <td colspan="2"><asp:TextBox ID="DeliveryTerm_1" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Purity_1" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="QMMethod_1" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Packing_1" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="NoOfferReason_1" runat="server" 
                                DataSourceID="SDS_RFQUpdate_NoOffer" DataTextField="Text" 
                                DataValueField="NoOfferReasonCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_NoOffer" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                                
                                SelectCommand="SELECT [NoOfferReasonCode], [Text] FROM [NoOfferReason] ORDER BY [SortOrder]">
                            </asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <th rowspan="2">2</th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_2" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_2" runat="server" AppendDataBoundItems="True" 
                                DataSourceID="SDS_RFQUpdate_Qua" DataTextField="UnitCode" 
                                DataValueField="UnitCode">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            x <asp:TextBox ID="EnqPiece_2" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="Currency_2" runat="server" 
                                DataSourceID="SDS_RFQUpdate_Currency" DataTextField="CurrencyCode" 
                                DataValueField="CurrencyCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="UnitPrice_2" runat="server" Width="5em" MaxLength="14" CssClass="number"></asp:TextBox></td>
                        <td><asp:TextBox ID="QuoPer_2" runat="server" Width="5em" MaxLength="9" 
                                CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="QuoUnit_2" runat="server" 
                                DataSourceID="SDS_RFQUpdate_Unit" DataTextField="UnitCode" 
                                DataValueField="UnitCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="LeadTime_2" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="SupplierItemNumber_2" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                        <td rowspan="2"><asp:HyperLink ID="POIssue_2" runat="server" 
                                NavigateUrl="./POIssue.aspx" Visible="False">PO Issue</asp:HyperLink>
                            <asp:HiddenField ID="LineNumber2" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="Incoterms_2" runat="server" 
                                DataSourceID="SDS_RFQUpdate_Incoterms" DataTextField="IncotermsCode" 
                                DataValueField="IncotermsCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="2"><asp:TextBox ID="DeliveryTerm_2" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Purity_2" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="QMMethod_2" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Packing_2" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="NoOfferReason_2" runat="server" 
                                DataSourceID="SDS_RFQUpdate_NoOffer" DataTextField="Text" 
                                DataValueField="NoOfferReasonCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th rowspan="2">3</th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_3" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_3" runat="server" AppendDataBoundItems="True" 
                                DataSourceID="SDS_RFQUpdate_Qua" DataTextField="UnitCode" 
                                DataValueField="UnitCode">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            x <asp:TextBox ID="EnqPiece_3" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="Currency_3" runat="server" 
                                DataSourceID="SDS_RFQUpdate_Currency" DataTextField="CurrencyCode" 
                                DataValueField="CurrencyCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="UnitPrice_3" runat="server" Width="5em" MaxLength="14" CssClass="number"></asp:TextBox></td>
                        <td><asp:TextBox ID="QuoPer_3" runat="server" Width="5em" MaxLength="9" 
                                CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="QuoUnit_3" runat="server" 
                                DataSourceID="SDS_RFQUpdate_Unit" DataTextField="UnitCode" 
                                DataValueField="UnitCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="LeadTime_3" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="SupplierItemNumber_3" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                        <td rowspan="2"><asp:HyperLink ID="POIssue_3" runat="server" 
                                NavigateUrl="./POIssue.aspx" Visible="False">PO Issue</asp:HyperLink>
                            <asp:HiddenField ID="LineNumber3" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="Incoterms_3" runat="server" 
                                DataSourceID="SDS_RFQUpdate_Incoterms" DataTextField="IncotermsCode" 
                                DataValueField="IncotermsCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="2"><asp:TextBox ID="DeliveryTerm_3" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Purity_3" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="QMMethod_3" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Packing_3" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="NoOfferReason_3" runat="server" 
                                DataSourceID="SDS_RFQUpdate_NoOffer" DataTextField="Text" 
                                DataValueField="NoOfferReasonCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th rowspan="2">4</th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_4" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_4" runat="server" AppendDataBoundItems="True" 
                                DataSourceID="SDS_RFQUpdate_Qua" DataTextField="UnitCode" 
                                DataValueField="UnitCode">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            x <asp:TextBox ID="EnqPiece_4" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="Currency_4" runat="server" 
                                DataSourceID="SDS_RFQUpdate_Currency" DataTextField="CurrencyCode" 
                                DataValueField="CurrencyCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="UnitPrice_4" runat="server" Width="5em" MaxLength="14" CssClass="number"></asp:TextBox></td>
                        <td><asp:TextBox ID="QuoPer_4" runat="server" Width="5em" MaxLength="9" 
                                CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="QuoUnit_4" runat="server" 
                                DataSourceID="SDS_RFQUpdate_Unit" DataTextField="UnitCode" 
                                DataValueField="UnitCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td><asp:TextBox ID="LeadTime_4" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="SupplierItemNumber_4" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                        <td rowspan="2"><asp:HyperLink ID="POIssue_4" runat="server" 
                                NavigateUrl="./POIssue.aspx" Visible="False">PO Issue</asp:HyperLink>
                            <asp:HiddenField ID="LineNumber4" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:DropDownList ID="Incoterms_4" runat="server" 
                                DataSourceID="SDS_RFQUpdate_Incoterms" DataTextField="IncotermsCode" 
                                DataValueField="IncotermsCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td colspan="2"><asp:TextBox ID="DeliveryTerm_4" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Purity_4" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="QMMethod_4" runat="server" Width="5em" MaxLength="255"></asp:TextBox></td>
                        <td><asp:TextBox ID="Packing_4" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="NoOfferReason_4" runat="server" 
                                DataSourceID="SDS_RFQUpdate_NoOffer" DataTextField="Text" 
                                DataValueField="NoOfferReasonCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                </table>

                <div class="btns">
                    <p class="message"><asp:Label ID="RunMsg" runat="server"></asp:Label></p>
                    <strong>Status : </strong>
                    <asp:DropDownList ID="RFQStatus" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="E">Enquired</asp:ListItem>
                        <asp:ListItem Value="PQ">Partly-quoted</asp:ListItem>
                        <asp:ListItem Value="Q">Quoted</asp:ListItem>
                    </asp:DropDownList>
                    
                    <asp:Button ID="Update" runat="server" Text="Update" />
                    <span class="indent"></span>
                    <asp:Button ID="Close" runat="server" Text="Close" />
                    <asp:HiddenField ID="QuotedDate" runat="server" />
                    <asp:HiddenField ID="UpdateDate" runat="server" />
                    <asp:HiddenField ID="EnqLocationCode" runat="server" />
                    <asp:HiddenField ID="QuoLocationCode" runat="server" />
                    <asp:HiddenField ID="Hi_RFQStatusCode" runat="server" />
                    <asp:HiddenField ID="PopupSupplierCode" runat="server" />
                </div>
				<% End If%>                
            </div>
    </div><!-- Main Content Area END -->
		<script language ="javascript" type="text/javascript">
		function SupplierSelect_onclick() {
    		var SupplierCode = encodeURIComponent(document.getElementById('SupplierCode').value);
    		var EnqLocation = encodeURIComponent(document.getElementById('EnqLocation').innerHTML);
	    	popup('./RFQSupplierSelect.aspx?Code=' + SupplierCode + '&Location=' + EnqLocation);
	    	return false;
		}
		function MakerSelect_onclick() {
    		var MakerCode = encodeURIComponent(document.getElementById('MakerCode').value);
	    	popup('./MakerSelect.aspx?Code=' + MakerCode);
	    	return false;
		}
		function Correspondence_onclick() {
            if (document.getElementById('RFQNumber')) {
        		var RFQNumber = encodeURIComponent(document.getElementById('RFQNumber').innerHTML);
	        	popup('./RFQCorrespondence.aspx?RFQNumber=' + RFQNumber);
	        }
	        else {
	            popup('./RFQCorrespondence.aspx?RFQNumber=');
	        }
        }
        function SupplierSetting_onclick() {
            if (document.getElementById('SupplierCode')) {
                var SupplierCode = encodeURIComponent(document.getElementById('SupplierCode').value);
                popup('./SupplierSetting.aspx?Action=Edit&Code=' + SupplierCode);
            }
            else {
                popup('./SupplierSetting.aspx?Action=Edit&Code=');
            }
        }

        function SupplierInfo_onclick() {
            if (document.getElementById('SupplierCode')) {
                document.getElementById('PopupSupplierCode').value = document.getElementById('SupplierCode').value;
                return true;
            } else {
                return false;
            }
            
        }
        function MakerInfo_onclick() {
            if (document.getElementById('MakerCode')) {
                document.getElementById('PopupSupplierCode').value = document.getElementById('MakerCode').value;
                return true;
            } else {
                return false;
            }
        }
        </script>
    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
        </form>
    </body>
</html>

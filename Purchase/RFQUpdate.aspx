<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQUpdate.aspx.vb" Inherits="Purchase.RFQUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
    <script type="text/javascript">
        function RFQListByProductSupplier() {
            var SupplierCode = (document.getElementById('Hidden_SupplierCode').value).replace(/\s|　/g, "");
            var SupplierCode = encodeURIComponent(SupplierCode);
            if (SupplierCode !== "") {
                location.assign('./RFQListBySupplier.aspx?SupplierCode=' + SupplierCode);
                return false;
            }
        }
    </script>
    <style type="text/css">
        #POIssue_1 {
            text-decoration: none;
            cursor: default;
        }

        #POIssue_2 {
            text-decoration: none;
            cursor: default;
        }

        #POIssue_3 {
            text-decoration: none;
            cursor: default;
        }

        #POIssue_4 {
            text-decoration: none;
            cursor: default;
        }
    </style>
</head>
<body>
    <!-- Header -->
    <commonUC:Header ID="HeaderMenu" runat="server" />
    <!-- Header End -->
    <form id="RFQForm" runat="server">
        <!-- Main Content Area -->
        <div id="content">
            <div class="tabs"><a href="#" onclick="return SupplierSetting_onclick()">Supplier Setting</a> | <a href="#" onclick="return Correspondence_onclick()">RFQ Correspondence / History</a></div>
            <h3>Quotation Reply</h3>
            <span style="float: right">
                <asp:Label ID="Confidential" runat="server" Text='' CssClass="confidential"></asp:Label></span>

            <div class="main">
                <p class="attention">
                    <asp:Label ID="Msg" runat="server" Text=""></asp:Label>
                </p>
                <%  If Me.Parameter = True Then%>
                <table class="left">
                    <tr>
                        <th>RFQ Reference Number : </th>
                        <td>
                            <asp:Label ID="RFQNumber" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Current Status : </th>
                        <td>
                            <asp:Label ID="CurrentRFQStatus" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th class="auto-style1">Product Number <span class="required">*</span> : </th>
                        <td>
                            <asp:HyperLink ID="RFQListByProductID" runat="server" Text=""></asp:HyperLink>
                            <span class="indent">
                                <span class="message">Code Extension : </span>
                                    <asp:DropDownList ID="CodeExtensionList" runat="server" Width="8em" Height="20px" AutoPostBack="True">
                                        <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </span>
                        </td>
                    </tr>
                    <tr>
                        <th>CAS : </th>
                        <td>
                            <asp:Label ID="CASNumber" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Product Name : </th>
                        <td>
                            <asp:Label ID="ProductName" runat="server" Text=""></asp:Label><asp:Label ID="ProductWarning" runat="server" Text="" Style="display: none"></asp:Label><asp:Label ID="SupplierWarning" runat="server" Text="" Style="display: none"></asp:Label><asp:Label ID="CountryWarning" runat="server" Text="" Style="display: none"></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Supplier Code <span class="required">*</span> : </th>
                        <td>
                            <asp:TextBox ID="SupplierCode" runat="server" Width="7em" MaxLength="10" OnTextChanged="SupplierCode_TextChanged" AutoPostBack="true"></asp:TextBox>
                            <asp:ImageButton ID="SupplierSelect" runat="server"
                                ImageUrl="./Image/Search.gif" CssClass="magnify"
                                OnClientClick="return SupplierSelect_onclick()" />
                            <asp:LinkButton runat="server" ID="SupplierInfo" Text="Supplier Info" />
                            <asp:HyperLink ID="SupplierRFQList" runat="server" Text="  Supplier RFQ List" onclick="return RFQListByProductSupplier()"></asp:HyperLink>
                        </td>
                    </tr>
                    <tr>
                        <th>SAP Supplier Code : </th>
                        <td>
                            <asp:TextBox ID="R3SupplierCode" runat="server" Width="7em" MaxLength="10" ReadOnly="true" CssClass="readonly"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Supplier Name / Country : </th>
                        <td>
                            <asp:TextBox ID="Hidden_SupplierCode" runat="server" AutoPostBack="true" Style="display: none;"></asp:TextBox>
                            <asp:TextBox ID="SupplierName" runat="server" Width="21em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
                            <asp:TextBox ID="SupplierCountry" runat="server" Width="4em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Supplier Contact Person : </th>
                        <td><span>
                            <asp:TextBox ID="SupplierContactPerson" runat="server" Width="10em" MaxLength="255"></asp:TextBox>
                            <asp:DropDownList ID="SupplierContactPersonCodeList" AutoPostBack="True"
                                DataSourceID="SDS_SupplierContactPersonCodeList" DataTextField="supplierInfo"
                                DataValueField="SupplierEmailID" runat="server" Width="16em" Height="20px">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_SupplierContactPersonCodeList" runat="server"
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>">
                                <%--SelectCommand="SELECT supplierCode,(CAST(supplierCode as nvarchar)+'-'+ Email) as supplierInfo  FROM Supplier ORDER BY supplierCode">--%>
                            </asp:SqlDataSource>
                        </span>
                        </td>
                    </tr>
                    <tr>
                        <th>Maker Code : </th>
                        <td>
                            <asp:TextBox ID="MakerCode" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
                            <asp:ImageButton ID="MakerSelect" runat="server" ImageUrl="./Image/Search.gif"
                                CssClass="magnify" OnClientClick="return MakerSelect_onclick()" />
                            <asp:LinkButton runat="server" ID="MakerInfo" Text="Supplier Information" />
                        </td>
                    </tr>
                    <tr>
                        <th>SAP Maker Code: </th>
                        <td>
                            <asp:TextBox ID="SAPMakerCode" runat="server" Width="7em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
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
                        <td>
                            <asp:TextBox ID="SupplierItemName" runat="server" Width="21em" MaxLength="255"></asp:TextBox></td>
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
                                SelectCommand="SELECT PaymentTermCode, Text FROM PurchasingPaymentTerm ORDER BY PaymentTermCode"></asp:SqlDataSource>
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
                                SelectCommand="SELECT [CurrencyCode] FROM [PurchasingCurrency] where CurrencyCode<>'RMB' and CurrencyCode<>'BEF' and CurrencyCode<>'CAD' and CurrencyCode<>'DEM' and CurrencyCode<>'DKK' and CurrencyCode<>'FRF' and CurrencyCode<>'NLG' and CurrencyCode<>'NOK' and CurrencyCode<>'SEK' ORDER BY [sortOrder]"></asp:SqlDataSource>
                            <asp:TextBox ID="ShippingHandlingFee" runat="server" Width="5em" MaxLength="14" CssClass="number"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Supplier Offer Valid To : </th>
                        <td>
                            <asp:TextBox ID="txtVaildTo" runat="server" Width="10em" MaxLength="10" CssClass="number"></asp:TextBox>
                            <span class="indent">
                                <asp:Label ID="Label1" runat="server" Text="">(YYYY-MM-DD)</asp:Label></span>
                        </td>
                    </tr>
                </table>

                <table>
                    <tr>
                        <th>Purpose : </th>
                        <td>
                            <asp:Label ID="Purpose" runat="server" Text=""></asp:Label>
                            <asp:DropDownList ID="ListPurpose" runat="server" DataSourceID="SrcPurpose" DataTextField="Text" DataValueField="PurposeCode" AutoPostBack="True" AppendDataBoundItems="true">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SrcPurpose" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                        </td>

                    </tr>
                    <tr>
                        <th>Priority : </th>
                        <td>
                            <asp:DropDownList ID="Priority" runat="server"></asp:DropDownList>
                            <asp:Label ID="LabelPriority" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>Required Purity : </th>
                        <td>
                            <asp:Label ID="RequiredPurity" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Required QM Method : </th>
                        <td>
                            <asp:Label ID="RequiredQMMethod" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Required Specification : </th>
                        <td>
                            <asp:Label ID="RequiredSpecification" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Spec Sheet : </th>
                        <td>
                            <asp:CheckBox ID="SpecSheet" runat="server"
                                Text="yes" /></td>
                    </tr>
                    <tr>
                        <th>Specification : </th>
                        <td>
                            <asp:TextBox ID="Specification" runat="server" Columns="50" Rows="3" TextMode="MultiLine"></asp:TextBox><td>
                                <div align="center" valign="bottom" style="margin-left: 0px">Plant-sp.matl status</div>
                            </td>
                        </td>
                    </tr>
                    <tr>
                        <th>Enq-User <span class="required">*</span> : </th>
                        <td style="position: relative">
                            <asp:DropDownList ID="EnqUser" runat="server" AutoPostBack="True"
                                DataSourceID="SDS_RFQUpdate_EnqUser" DataTextField="Name"
                                DataValueField="UserID">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_EnqUser" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                            <asp:DropDownList ID="EnqLocation" runat="server" AutoPostBack="True"
                                DataSourceID="SDS_RFQUpdate_EnqLocation" DataTextField="Name"
                                DataValueField="LocationCode">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_EnqLocation" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                            <asp:DropDownList ID="StorageLocation" runat="server" AutoPostBack="True"
                                DataSourceID="SDS_RFQUpdate_EnqStorageLocation" DataTextField="Storage"
                                DataValueField="Storage" AppendDataBoundItems="true">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_EnqStorageLocation" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                        </td>
                        <td>
                            <asp:TextBox ID="EnqUserPlantStatus" runat="server" Width="4em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
                            <asp:TextBox ID="EnqUserPlantDescriptions" runat="server" Width="8em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Quo-User <span class="required">*</span> : </th>
                        <td>
                            <asp:DropDownList ID="QuoUser" runat="server" AutoPostBack="True"
                                DataSourceID="SDS_RFQUpdate_QuoUser" DataTextField="Name"
                                DataValueField="UserID" AppendDataBoundItems="true">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_QuoUser" runat="server"
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>">
                                <%-- <SelectParameters>
                                    <asp:ControlParameter ControlID="QuoLocation" Name="Location" 
                                        PropertyName="Text" />
                                </SelectParameters>--%>
                            </asp:SqlDataSource>
                            <%--(<asp:Label ID="QuoLocation" runat="server" Text="" ></asp:Label>)--%>
                            <asp:DropDownList ID="QuoLocation" runat="server" AutoPostBack="True"
                                DataSourceID="SDS_RFQUpdate_EnqLocation" DataTextField="Name"
                                DataValueField="LocationCode">
                            </asp:DropDownList>

                            <asp:DropDownList ID="StorageLocation2" runat="server" AutoPostBack="True"
                                DataSourceID="SDS_RFQUpdate_QuoStorageLocation" DataTextField="Storage"
                                DataValueField="Storage" AppendDataBoundItems="true">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_QuoStorageLocation" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                        </td>
                        <td>
                            <asp:TextBox ID="QuoUserPlantStatus" runat="server" Width="4em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
                            <asp:TextBox ID="QuoUserPlantDescriptions" runat="server" Width="8em" ReadOnly="true" CssClass="readonly"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Comment : </th>
                        <td>
                            <asp:TextBox ID="Comment" runat="server" Columns="50" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                        <td>MMSTA Invalidation<asp:CheckBox ID="MMSTAInvalidation" runat="server" Width="7em" ReadOnly="true" AutoPostBack="True"></asp:CheckBox>
                        </td>
                    </tr>
                    <tr>
                        <th>BUoM : </th>
                        <td>
                            <asp:Label ID="labBUoM" runat="server" Text=""></asp:Label></td>
                    </tr>
                </table>
            </div>

            <div class="list">
                <table>
                    <tr>
                        <th style="width: 3%" rowspan="2">No.</th>
                        <th>Enq-Quantity <span class="required">*</span></th>
                        <th>Currency</th>
                        <th>Price</th>
                        <th>Quo-Per</th>
                        <th>Quo-Unit</th>
                        <th>Lead Time(Days)</th>
                        <th>Supplier Item Number</th>
                        <th style="width: 5%" rowspan="2">PO Issue</th>
                        <th style="width: 5%" rowspan="2">PO Interface</th>
                    </tr>
                    <tr>
                        <th>Incoterms</th>
                        <th colspan="2">Terms of Delivery</th>
                        <th>Purity / Method</th>
                        <th>Supplier Offer No</th>
                        <th>Packing</th>
                        <th>Reason for "No Offer"</th>
                    </tr>
                    <tr>
                        <th rowspan="2">1
                            <asp:HiddenField ID="PFC1" runat="server" />
                        </th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_1" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_1" runat="server" AppendDataBoundItems="True"
                                DataSourceID="SDS_RFQUpdate_Qua" DataTextField="UnitCode"
                                DataValueField="UnitCode" AutoPostBack="True" OnSelectedIndexChanged="EnqUnit_1_SelectedIndexChanged">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            x
                            <asp:TextBox ID="EnqPiece_1" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_Qua" runat="server"
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"
                                SelectCommand="SELECT [UnitCode] FROM [PurchasingUnit] where UnitCode<>'ZZ' and UnitCode<>'PC' and UnitCode<>'TON' and UnitCode<>'-' and UnitCode<>'MT' and UnitCode<>'MU' ORDER BY [SortOrder]"></asp:SqlDataSource>
                        </td>
                        <td>
                            <asp:DropDownList ID="Currency_1" runat="server"
                                DataSourceID="SDS_RFQUpdate_Currency" DataTextField="CurrencyCode"
                                DataValueField="CurrencyCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="UnitPrice_1" runat="server" Width="5em" MaxLength="14" CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="QuoPer_1" runat="server" Width="5em" MaxLength="9"
                                CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="QuoUnit_1" runat="server"
                                DataSourceID="SDS_RFQUpdate_Unit" DataTextField="UnitCode"
                                DataValueField="UnitCode" AppendDataBoundItems="True" Enabled="False">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_Unit" runat="server"
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"
                                SelectCommand="SELECT [UnitCode] FROM [PurchasingUnit] where UnitCode<>'ZZ' and UnitCode<>'PC' and UnitCode<>'TON' and UnitCode<>'-' and UnitCode<>'MT' and UnitCode<>'MU' ORDER BY [SortOrder]"></asp:SqlDataSource>
                        </td>
                        <td style="position: relative;">
                            <asp:TextBox ID="LeadTime_1" runat="server" Width="10em" MaxLength="255" OnClientClick="" onmouseover="ShowComment(1)" onmouseout="HideComment(1)"></asp:TextBox>
                            <div id="comment1" style="position: absolute; left: 50%; background-color: #cbd0d3; top: 50%; padding: 10px; width: 300px; line-height: 15px; margin: auto; height: 30px; display: none;">
                                1. The date here is on calendor day base.<br />
                                2. Please add your local supplier shipping buffer.
                            </div>
                        </td>
                        <td>
                            <asp:TextBox ID="SupplierItemNumber_1" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                        <td rowspan="2">
                            <asp:HyperLink ID="POIssue_1" runat="server"
                                NavigateUrl="./POIssue.aspx" Visible="False" Enabled="False" ForeColor="#CBD0D3">PO Issue</asp:HyperLink>
                            <asp:HiddenField ID="LineNumber1" runat="server" />
                        </td>
                        <td rowspan="2">
                            <%-- <asp:HyperLink ID="POInterface_1" runat="server"  
                                NavigateUrl="./POInterface.aspx" Visible="False">PO Interface</asp:HyperLink>--%>
                            <asp:Button ID="POInterfaceButton_1" Visible="False" runat="server" Text="PO Interface" BackColor="#E5ECF3" BorderColor="#E5ECF3" BorderStyle="None" ForeColor="#2651A5" OnClientClick="return POInterfaceClient(1);" />
                            <asp:HiddenField ID="HiddenField1" runat="server" />
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
                                SelectCommand="SELECT IncotermsCode, [Text] FROM s_Incoterms ORDER BY IncotermsCode "></asp:SqlDataSource>
                        </td>
                        <td colspan="2">
                            <asp:TextBox ID="DeliveryTerm_1" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="Purity_1" runat="server" Width="10em" MaxLength="255"></asp:TextBox>
                            <asp:TextBox ID="QMMethod_1" runat="server" Visible="false" Width="5em" MaxLength="255"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="SupplierOfferNo_1" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="Packing_1" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="NoOfferReason_1" runat="server"
                                DataSourceID="SDS_RFQUpdate_NoOffer" DataTextField="Text"
                                DataValueField="NoOfferReasonCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SDS_RFQUpdate_NoOffer" runat="server"
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"
                                SelectCommand="SELECT [NoOfferReasonCode], [Text] FROM [NoOfferReason] ORDER BY [SortOrder]"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <th rowspan="2">2
                            <asp:HiddenField ID="PFC2" runat="server" />
                        </th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_2" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_2" runat="server" AppendDataBoundItems="True"
                                DataSourceID="SDS_RFQUpdate_Qua" DataTextField="UnitCode"
                                DataValueField="UnitCode" AutoPostBack="True" OnSelectedIndexChanged="EnqUnit_2_SelectedIndexChanged">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            x
                            <asp:TextBox ID="EnqPiece_2" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="Currency_2" runat="server"
                                DataSourceID="SDS_RFQUpdate_Currency" DataTextField="CurrencyCode"
                                DataValueField="CurrencyCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="UnitPrice_2" runat="server" Width="5em" MaxLength="14" CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="QuoPer_2" runat="server" Width="5em" MaxLength="9"
                                CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="QuoUnit_2" runat="server"
                                DataSourceID="SDS_RFQUpdate_Unit" DataTextField="UnitCode"
                                DataValueField="UnitCode" AppendDataBoundItems="True" Enabled="False">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="position: relative;">
                            <asp:TextBox ID="LeadTime_2" runat="server" Width="10em" MaxLength="255" onmouseover="ShowComment(2)" onmouseout="HideComment(2)"></asp:TextBox>
                            <div id="comment2" style="position: absolute; left: 50%; background-color: #cbd0d3; top: 50%; padding: 10px; width: 300px; line-height: 15px; margin: auto; height: 30px; display: none;">
                                1. The date here is on calendor day base.<br />
                                2. Please add your local supplier shipping buffer.
                            </div>
                        </td>
                        <td>
                            <asp:TextBox ID="SupplierItemNumber_2" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                        <td rowspan="2">
                            <asp:HyperLink ID="POIssue_2" runat="server"
                                NavigateUrl="./POIssue.aspx" Visible="False" ForeColor="#CBD0D3">PO Issue</asp:HyperLink>
                            <asp:HiddenField ID="LineNumber2" runat="server" />
                        </td>
                        <td rowspan="2">
                            <%-- <asp:HyperLink ID="POInterface_2" runat="server" 
                                NavigateUrl="./POInterface.aspx" Visible="False">PO Interface</asp:HyperLink>--%>
                            <asp:Button ID="POInterfaceButton_2" Visible="False" runat="server" Text="PO Interface" BackColor="#E5ECF3" BorderColor="#E5ECF3" BorderStyle="None" ForeColor="#2651A5" OnClientClick="return POInterfaceClient(2)" />
                            <asp:HiddenField ID="HiddenField2" runat="server" />
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
                        <td colspan="2">
                            <asp:TextBox ID="DeliveryTerm_2" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="Purity_2" runat="server" Width="10em" MaxLength="255"></asp:TextBox>
                            <asp:TextBox ID="QMMethod_2" runat="server" Width="5em" MaxLength="255" Visible="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="SupplierOfferNo_2" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="Packing_2" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="NoOfferReason_2" runat="server"
                                DataSourceID="SDS_RFQUpdate_NoOffer" DataTextField="Text"
                                DataValueField="NoOfferReasonCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th rowspan="2">3
                            <asp:HiddenField ID="PFC3" runat="server" />
                        </th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_3" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_3" runat="server" AppendDataBoundItems="True"
                                DataSourceID="SDS_RFQUpdate_Qua" DataTextField="UnitCode"
                                DataValueField="UnitCode" AutoPostBack="True" OnSelectedIndexChanged="EnqUnit_3_SelectedIndexChanged">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            x
                            <asp:TextBox ID="EnqPiece_3" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="Currency_3" runat="server"
                                DataSourceID="SDS_RFQUpdate_Currency" DataTextField="CurrencyCode"
                                DataValueField="CurrencyCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="UnitPrice_3" runat="server" Width="5em" MaxLength="14" CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="QuoPer_3" runat="server" Width="5em" MaxLength="9"
                                CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="QuoUnit_3" runat="server"
                                DataSourceID="SDS_RFQUpdate_Unit" DataTextField="UnitCode"
                                DataValueField="UnitCode" AppendDataBoundItems="True" Enabled="False">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="position: relative;">
                            <asp:TextBox ID="LeadTime_3" runat="server" Width="10em" MaxLength="255" onmouseover="ShowComment(3)" onmouseout="HideComment(3)"></asp:TextBox>
                            <div id="comment3" style="position: absolute; left: 50%; background-color: #cbd0d3; top: 50%; padding: 10px; width: 300px; line-height: 15px; margin: auto; height: 30px; display: none;">
                                1. The date here is on calendor day base.<br />
                                2. Please add your local supplier shipping buffer.
                            </div>
                        </td>
                        <td>
                            <asp:TextBox ID="SupplierItemNumber_3" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                        <td rowspan="2">
                            <asp:HyperLink ID="POIssue_3" runat="server"
                                NavigateUrl="./POIssue.aspx" Visible="False" ForeColor="#CBD0D3">PO Issue</asp:HyperLink>
                            <asp:HiddenField ID="LineNumber3" runat="server" />
                        </td>
                        <td rowspan="2">
                            <%--<asp:HyperLink ID="POInterface_3" runat="server"  
                                NavigateUrl="./POInterface.aspx" Visible="False">PO Interface</asp:HyperLink>--%>
                            <asp:Button ID="POInterfaceButton_3" Visible="False" runat="server" Text="PO Interface" BackColor="#E5ECF3" BorderColor="#E5ECF3" BorderStyle="None" ForeColor="#2651A5" OnClientClick="return POInterfaceClient(3)" />

                            <asp:HiddenField ID="HiddenField3" runat="server" />
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
                        <td colspan="2">
                            <asp:TextBox ID="DeliveryTerm_3" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="Purity_3" runat="server" Width="10em" MaxLength="255"></asp:TextBox>
                            <asp:TextBox ID="QMMethod_3" runat="server" Width="5em" MaxLength="255" Visible="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="SupplierOfferNo_3" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="Packing_3" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="NoOfferReason_3" runat="server"
                                DataSourceID="SDS_RFQUpdate_NoOffer" DataTextField="Text"
                                DataValueField="NoOfferReasonCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th rowspan="2">4
                            <asp:HiddenField ID="PFC4" runat="server" />
                        </th>
                        <td>
                            <asp:TextBox ID="EnqQuantity_4" runat="server" Width="5em" MaxLength="18" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="EnqUnit_4" runat="server" AppendDataBoundItems="True"
                                DataSourceID="SDS_RFQUpdate_Qua" DataTextField="UnitCode"
                                DataValueField="UnitCode" AutoPostBack="True" OnSelectedIndexChanged="EnqUnit_4_SelectedIndexChanged">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            x
                            <asp:TextBox ID="EnqPiece_4" runat="server" Width="5em" MaxLength="5" CssClass="number"></asp:TextBox>
                        </td>
                        <td>
                            <asp:DropDownList ID="Currency_4" runat="server"
                                DataSourceID="SDS_RFQUpdate_Currency" DataTextField="CurrencyCode"
                                DataValueField="CurrencyCode" AppendDataBoundItems="True">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:TextBox ID="UnitPrice_4" runat="server" Width="5em" MaxLength="14" CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="QuoPer_4" runat="server" Width="5em" MaxLength="9"
                                CssClass="number"></asp:TextBox></td>
                        <td>
                            <asp:DropDownList ID="QuoUnit_4" runat="server"
                                DataSourceID="SDS_RFQUpdate_Unit" DataTextField="UnitCode"
                                DataValueField="UnitCode" AppendDataBoundItems="True" Enabled="False">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td style="position: relative;">
                            <asp:TextBox ID="LeadTime_4" runat="server" Width="10em" MaxLength="255" onmouseover="ShowComment(4)" onmouseout="HideComment(4)"></asp:TextBox>
                            <div id="comment4" style="position: absolute; left: 50%; background-color: #cbd0d3; top: 50%; padding: 10px; width: 300px; line-height: 15px; margin: auto; height: 30px; display: none;">
                                1. The date here is on calendor day base.<br />
                                2. Please add your local supplier shipping buffer.
                            </div>
                        </td>
                        <td>
                            <asp:TextBox ID="SupplierItemNumber_4" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                        <td rowspan="2">
                            <asp:HyperLink ID="POIssue_4" runat="server"
                                NavigateUrl="./POIssue.aspx" Visible="False" ForeColor="#CBD0D3">PO Issue</asp:HyperLink>
                            <asp:HiddenField ID="LineNumber4" runat="server" />
                        </td>
                        <td rowspan="2">
                            <%--<asp:HyperLink ID="POInterface_4" runat="server"  
                                NavigateUrl="./POInterface.aspx" Visible="False">PO Interface</asp:HyperLink>--%>
                            <asp:Button ID="POInterfaceButton_4" Visible="False" runat="server" Text="PO Interface" BackColor="#E5ECF3" BorderColor="#E5ECF3" BorderStyle="None" ForeColor="#2651A5" OnClientClick="return POInterfaceClient(4)" />
                            <asp:HiddenField ID="HiddenField4" runat="server" />
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
                        <td colspan="2">
                            <asp:TextBox ID="DeliveryTerm_4" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="Purity_4" runat="server" Width="10em" MaxLength="255"></asp:TextBox>
                            <asp:TextBox ID="QMMethod_4" runat="server" Width="5em" MaxLength="255" Visible="false"></asp:TextBox>
                        </td>
                        <td>
                            <asp:TextBox ID="SupplierOfferNo_4" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
                        <td>
                            <asp:TextBox ID="Packing_4" runat="server" Width="10em" MaxLength="255"></asp:TextBox></td>
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
                    <p class="message">
                        <asp:Label ID="RunMsg" runat="server"></asp:Label>
                    </p>
                    <strong>Status : </strong>
                    <asp:DropDownList ID="RFQStatus" runat="server">
                        <asp:ListItem></asp:ListItem>
                        <asp:ListItem Value="E">Enquired</asp:ListItem>
                        <asp:ListItem Value="PQ">Partly-quoted</asp:ListItem>
                        <asp:ListItem Value="Q">Quoted</asp:ListItem>
                    </asp:DropDownList>

                    <asp:Button ID="Update" runat="server" Text="Update" OnClientClick="checkStatus()" />
                    <span class="indent"></span>
                    <asp:Button ID="Close" runat="server" Text="Close" />
                    <asp:HiddenField ID="QuotedDate" runat="server" />
                    <asp:HiddenField ID="UpdateDate" runat="server" />
                    <asp:HiddenField ID="EnqLocationCode" runat="server" />
                    <asp:HiddenField ID="QuoLocationCode" runat="server" />
                    <asp:HiddenField ID="Hi_RFQStatusCode" runat="server" />
                    <asp:HiddenField ID="PurposeCode" runat="server" />
                    <asp:HiddenField ID="EnqStorageLOcationCode" runat="server" />
                    <asp:HiddenField ID="QuoStorageLOcationCode" runat="server" />
                    <asp:Label ID="SuplierCountryShort" runat="server" Text="" Style="display: none"></asp:Label>
                    <asp:Label ID="EnqUserStatus" runat="server" Text="" Style="display: none"></asp:Label>
                    <asp:Label ID="QuoUserStatus" runat="server" Text="" Style="display: none"></asp:Label>
                    <asp:Label ID="POInterfaceMsg" runat="server" Text="" Style="display: none"></asp:Label>
                    <asp:Label ID="POInterfaceConfirmMsg" runat="server" Text="" Style="display: none"></asp:Label>
                </div>
                <% End If%>
            </div>
        </div>
        <!-- Main Content Area END -->
        <script language="javascript" type="text/javascript">
            function SupplierSelect_onclick() {
                var SupplierCode = encodeURIComponent(document.getElementById('SupplierCode').value);
                var EnqLocation = encodeURIComponent(document.getElementById('EnqLocation').value);
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
            function RegleadTime(data) {
                if (!(/^[0-9]+$/.test(document.getElementById("LeadTime_" + data).value) && document.getElementById("LeadTime_" + data).value > 0)) {
                    alert('Please input natural number')
                    document.getElementById("LeadTime_" + data).value = "";
                    return;
                }
                return '';
            }
            function ShowComment(i) {
                document.getElementById("comment" + i).style.display = "block";
            }
            function HideComment(i) {
                document.getElementById("comment" + i).style.display = "none";
            }
            function insert_flg(str, flg, sn) {
                var newstr = "";
                for (var i = 0; i < str.length; i += sn) {
                    var tmp = str.substring(i, i + sn);
                    newstr += tmp + flg;
                }
                return newstr;
            }

            function POInterfaceClient(lin) {
                var confirmMsg = document.getElementById("POInterfaceConfirmMsg").innerHTML;
                if (confirmMsg.length > 0) {
                    var msg = confirmMsg.replace(/<br>/g, "\n");
                    if (!confirm(msg)) return false;
                }
                if (document.getElementById("ProductWarning").innerText != "") {
                    var warning1 = insert_flg(document.getElementById("ProductWarning").innerText, '\n', 40);
                    //if (confirm("Product Warning:\n" + warning1 + "\n Do you still wants to create the interface?")) {

                    //} else {
                    //    return false;
                    //}
                    alert("Product Warning:\n" + warning1 + "\n Do you still wants to create the interface?");
                }
                if (document.getElementById("SupplierWarning").innerText != "") {
                    var warning2 = insert_flg(document.getElementById("SupplierWarning").innerText, '\n', 40);
                    //if (confirm("Supplier Warning:\n" + warning2 + "\n Do you still wants to create the interface?")) {

                    //} else {
                    //    return false;
                    //}
                    alert("Supplier Warning:\n" + warning2 + "\n Do you still wants to create the interface?");
                }

                if (document.getElementById("CountryWarning").innerText != "" && document.getElementById("CountryWarning").innerText != document.getElementById("QuoLocation").options[document.getElementById("QuoLocation").selectedIndex].text) {
                    var warning3 = insert_flg("Please check if the supplier extended to your company or Pur. Org.in BP first.", '\n', 40);
                    //if (confirm("Product Warning:\n" + warning1 + "\n Do you still wants to create the interface?")) {

                    //} else {
                    //    return false;
                    //}
                    alert(warning3);
                }
                if (document.getElementById("EnqUserStatus").innerText == "Success" && document.getElementById("EnqUserStatus").innerText != "") {
                    if (document.getElementById("QuoUserStatus").innerText == "Warning") {
                        var POIMsg = document.getElementById("POInterfaceMsg").innerText
                        if (confirm(POIMsg.replace("<br>","\n"))) {
                            return true
                        }
                        else {
                            return false;
                        }

                    }
                }
                if (document.getElementById("EnqUserStatus").innerText == "Warning" && document.getElementById("EnqUserStatus").innerText != "") {
                    if (document.getElementById("QuoUserStatus").innerText == "Success") {
                        var POIMsg = document.getElementById("POInterfaceMsg").innerText
                        if (confirm(POIMsg.replace("<br>", "\n"))) {
                            return true
                        }
                        else {
                            return false;
                        }
                    }
                    if (document.getElementById("QuoUserStatus").innerText == "Warning") {
                        var POIMsg = document.getElementById("POInterfaceMsg").innerText
                        if (confirm(POIMsg.replace("<br>", "\n"))) {
                            return true
                        }
                        else {
                            return false;
                        }
                    }
                }

                var op = ""
                switch (lin) {
                    case 1:
                        op = document.getElementById("<%= PFC1.ClientID %>").value;
                        break;
                    case 2:
                        op = document.getElementById("<%= PFC2.ClientID%>").value;
                    break;
                case 3:
                    op = document.getElementById("<%= PFC3.ClientID%>").value;
                    break;
                case 4:
                    op = document.getElementById("<%= PFC4.ClientID%>").value;
                    break;
            }
            switch (op) {
                case "2":
                    alert('Please make sure Material Master have unit conversion!');
                    if (confirm('Duplicated/Revise output?'))
                    { return true; }
                    else { return false; }
                    break;
                case "3":
                    alert('Please make sure Material Master have unit conversion!');
                    return true;
                    break;
                case "4":
                    if (confirm('Duplicated/Revise output?'))
                    { return true; }
                    else { return false; }
                    break;
                case "5":
                    return true;
                    break;
            }

        }
        function checkStatus() {
            var status = document.getElementById("<%=RFQStatus.ClientID %>").value;
            if (status == "Q") {
                var linUnit1 = document.getElementById("<%=EnqUnit_1.ClientID %>").value;
		            var linUnit2 = document.getElementById("<%=EnqUnit_2.ClientID%>").value;
		            var linUnit3 = document.getElementById("<%=EnqUnit_3.ClientID%>").value;
		            var linUnit4 = document.getElementById("<%=EnqUnit_4.ClientID%>").value;
		            if (linUnit1 == "LB" || linUnit1 == "L" || linUnit1 == "ML" || linUnit1 == "ST") {
		                alert("Please make sure Material Master have unit conversion!");
		                return true;
		            }
		            if (linUnit2 == "LB" || linUnit2 == "L" || linUnit2 == "ML" || linUnit2 == "ST") {
		                alert("Please make sure Material Master have unit conversion!");
		                return true;
		            }
		            if (linUnit3 == "LB" || linUnit3 == "L" || linUnit3 == "ML" || linUnit3 == "ST") {
		                alert("Please make sure Material Master have unit conversion!");
		                return true;
		            }
		            if (linUnit4 == "LB" || linUnit4 == "L" || linUnit4 == "ML" || linUnit4 == "ST") {
		                alert("Please make sure Material Master have unit conversion!");
		                return true;
		            }
		        }
            }
            function MyFun() {
                alert("PO Interface create successfully!");
            }
        </script>
        <!-- Footer -->
        <!--#include virtual="./Footer.html" -->
        <!-- Footer END -->
    </form>
</body>
</html>

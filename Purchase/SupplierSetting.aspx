<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SupplierSetting.aspx.vb" Inherits="Purchase.SupplierSetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
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
                <asp:HiddenField ID="Para_Comment" runat="server" />
                <asp:HyperLink ID="SuppliersProduct" runat="server">Suppliers Product</asp:HyperLink>
            </div>

            <h3>Supplier Setting</h3>

            <div class="main">
                <p class="attention">
                    <asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>

                <table>
                    <tr>
                        <th>SAP Supplier Code : </th>
                        <td>
                            <asp:Label ID="S4SupplierCode" runat="server" Text="" Width="7em"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>Supplier Code : </th>
                        <td>
                            <asp:Label ID="Code" runat="server" Text="" Width="7em"></asp:Label>
                            &nbsp;
                            <asp:Label ID="EmailID" runat="server" Text="Email ID" Width="5em" style="margin-left:8em;"></asp:Label>
                            &nbsp;
                            <asp:Label ID="EmailAddress" runat="server" Text="E-mail Address" Width="14em"></asp:Label>
                            <asp:Label ID="SupplierCode" runat="server" Text="Contact person" Width="7em"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th><span class="r3">SAP  Supplier Code 1</span> : </th>
                        <td>
                            <asp:TextBox ID="R3SupplierCode" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
                            <%--<span class="indent"></span>TCI-J only--%>
                            <asp:TextBox ID="SupplierEmailID1" runat="server" Width="5em" MaxLength="10" style="margin-left:8em;"></asp:TextBox>
                            <asp:TextBox ID="SupplierEmail1" runat="server" Width="14em" MaxLength="30"></asp:TextBox>
                            <asp:TextBox ID="SupplierCode1" runat="server" Width="7em" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <th>SAP  Supplier Code 2 : </th>
                        <td>
                            <asp:TextBox ID="R3SupplierCode2" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
                            <asp:DropDownList ID="SupplierLocationCode2" runat="server" Visible="False">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="SupplierEmailID2" runat="server" Width="5em" MaxLength="10" style="margin-left:8em;"></asp:TextBox>
                            <asp:TextBox ID="SupplierEmail2" runat="server" Width="14em" MaxLength="30"></asp:TextBox>
                            <asp:TextBox ID="SupplierCode2" runat="server" Width="7em" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>SAP  Supplier Code 3 : </th>
                        <td>
                            <asp:TextBox ID="R3SupplierCode3" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
                            <asp:DropDownList ID="SupplierLocationCode3" runat="server" Visible="False">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="SupplierEmailID3" runat="server" Width="5em" MaxLength="10" style="margin-left:8em;"></asp:TextBox>
                            <asp:TextBox ID="SupplierEmail3" runat="server" Width="14em" MaxLength="30"></asp:TextBox>
                            <asp:TextBox ID="SupplierCode3" runat="server" Width="7em" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>SAP  Supplier Code 4 : </th>
                        <td>
                            <asp:TextBox ID="R3SupplierCode4" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
                            <asp:DropDownList ID="SupplierLocationCode4" runat="server" Visible="False">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="SupplierEmailID4" runat="server" Width="5em" MaxLength="10" style="margin-left:8em;"></asp:TextBox>
                            <asp:TextBox ID="SupplierEmail4" runat="server" Width="14em" MaxLength="30"></asp:TextBox>
                            <asp:TextBox ID="SupplierCode4" runat="server" Width="7em" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <th>SAP  Supplier Code 5 : </th>
                        <td>
                            <asp:TextBox ID="R3SupplierCode5" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
                            <asp:DropDownList ID="SupplierLocationCode5" runat="server" Visible="False">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="SupplierEmailID5" runat="server" Width="5em" MaxLength="10" style="margin-left:8em;"></asp:TextBox>
                            <asp:TextBox ID="SupplierEmail5" runat="server" Width="14em" MaxLength="30"></asp:TextBox>
                            <asp:TextBox ID="SupplierCode5" runat="server" Width="7em" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <th>SAP  Supplier Code 6 : </th>
                        <td>
                            <asp:TextBox ID="R3SupplierCode6" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
                            <asp:DropDownList ID="SupplierLocationCode6" runat="server" Visible="False">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="SupplierEmailID6" runat="server" Width="5em" MaxLength="10" style="margin-left:8em;"></asp:TextBox>
                            <asp:TextBox ID="SupplierEmail6" runat="server" Width="14em" MaxLength="30"></asp:TextBox>
                            <asp:TextBox ID="SupplierCode6" runat="server" Width="7em" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <th>SAP  Supplier Code 7 : </th>
                        <td>
                            <asp:TextBox ID="R3SupplierCode7" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
                            <asp:DropDownList ID="SupplierLocationCode7" runat="server" Visible="False">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="SupplierEmailID7" runat="server" Width="5em" MaxLength="10" style="margin-left:8em;"></asp:TextBox>
                            <asp:TextBox ID="SupplierEmail7" runat="server" Width="14em" MaxLength="30"></asp:TextBox>
                            <asp:TextBox ID="SupplierCode7" runat="server" Width="7em" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <th>SAP  Supplier Code 8 : </th>
                        <td>
                            <asp:TextBox ID="R3SupplierCode8" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
                            <asp:DropDownList ID="SupplierLocationCode8" runat="server" Visible="False">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="SupplierEmailID8" runat="server" Width="5em" MaxLength="10" style="margin-left:8em;"></asp:TextBox>
                            <asp:TextBox ID="SupplierEmail8" runat="server" Width="14em" MaxLength="30"></asp:TextBox>
                            <asp:TextBox ID="SupplierCode8" runat="server" Width="7em" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <th>SAP  Supplier Code 9 : </th>
                        <td>
                            <asp:TextBox ID="R3SupplierCode9" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
                            <asp:DropDownList ID="SupplierLocationCode9" runat="server" Visible="False">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="SupplierEmailID9" runat="server" Width="5em" MaxLength="10" style="margin-left:8em;"></asp:TextBox>
                            <asp:TextBox ID="SupplierEmail9" runat="server" Width="14em" MaxLength="30"></asp:TextBox>
                            <asp:TextBox ID="SupplierCode9" runat="server" Width="7em" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <th>SAP  Supplier Code 10 : </th>
                        <td>
                            <asp:TextBox ID="R3SupplierCode10" runat="server" Width="7em" MaxLength="10"></asp:TextBox>
                            <asp:DropDownList ID="SupplierLocationCode10" runat="server" Visible="False">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:TextBox ID="SupplierEmailID10" runat="server" Width="5em" MaxLength="10" style="margin-left:8em;"></asp:TextBox>
                            <asp:TextBox ID="SupplierEmail10" runat="server" Width="14em" MaxLength="30"></asp:TextBox>
                            <asp:TextBox ID="SupplierCode10" runat="server" Width="7em" MaxLength="30"></asp:TextBox>
                        </td>
                    </tr>

                    <tr>
                        <th>Supplier Information : </th>
                        <td>
                            <asp:TextBox ID="SupplierInfo" runat="server" Width="21em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><span class="r3">TCI-J Supplier Name</span> : </th>
                        <td>
                            <asp:TextBox ID="SupplierName1" runat="server" Width="21em" MaxLength="255"></asp:TextBox>
                            <asp:TextBox ID="SupplierName2" runat="server" Width="21em" MaxLength="255"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th><span class="r3">Supplier Name</span><span class="required">*</span> : </th>
                        <td>
                            <asp:TextBox ID="SupplierName3" runat="server" Width="21em" MaxLength="255"></asp:TextBox>
                            <asp:TextBox ID="SupplierName4" runat="server" Width="21em" MaxLength="255"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th><span class="r3">Search Term</span> : </th>
                        <td>
                            <asp:TextBox ID="SearchTerm1" runat="server" Width="21em" MaxLength="255"></asp:TextBox>
                            <asp:TextBox ID="SearchTerm2" runat="server" Width="21em" MaxLength="255"></asp:TextBox>
                        </td>
                    </tr>
                </table>

                <table class="left" style="margin-left: 4em">
                    <tr>
                        <th rowspan="3"><span class="r3">Address</span><span class="required">*</span> : </th>
                        <td>
                            <asp:TextBox ID="Address1" runat="server" Width="21em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="Address2" runat="server" Width="21em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <td>
                            <asp:TextBox ID="Address3" runat="server" Width="21em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><span class="r3">Postal Code</span> : </th>
                        <td>
                            <asp:TextBox ID="PostalCode" runat="server" Width="7em" MaxLength="32"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><span class="r3">Country</span><span class="required">*</span> : </th>
                        <td>
                            <asp:DropDownList ID="Country" runat="server" AutoPostBack="True">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th><span class="r3">Region</span> : </th>
                        <td>
                            <asp:DropDownList ID="Region" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th><span class="r3">Telephone</span> : </th>
                        <td>
                            <asp:TextBox ID="Telephone" runat="server" Width="10em" MaxLength="32"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><span class="r3">Fax</span> : </th>
                        <td>
                            <asp:TextBox ID="Fax" runat="server" Width="10em" MaxLength="32"></asp:TextBox></td>
                    </tr>
                </table>

                <table>
                    <tr>
                        <th>E-mail : </th>
                        <td>
                            <asp:TextBox ID="Email" runat="server" Width="21em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Website : </th>
                        <td>
                            <asp:TextBox ID="Website" runat="server" Width="21em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th><span class="r3">R/3 Comments</span> : </th>
                        <td>
                            <asp:TextBox ID="R3Comment" runat="server" Width="21em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Comments : </th>
                        <td>
                            <asp:TextBox ID="Comment" runat="server" Columns="50" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
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
                <asp:HiddenField ID="Action" runat="server" Value="Save" />

                <span class="r3" style="font-weight: bold; font-style: italic">Items in blue letters are automatically revised based on ERP.</span>

                <div class="btns">
                    <p class="message">
                        <asp:Label ID="RunMsg" runat="server"></asp:Label></p>
                    <asp:Button ID="Save" runat="server" Text="Save" UseSubmitBehavior="False" />
                </div>
            </div>
        </div>
        <!-- Main Content Area END -->

        <!-- Footer -->
        <!--#include virtual="./Footer.html" -->
        <!-- Footer END -->
    </form>
</body>
</html>

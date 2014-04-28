<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="POIssue.aspx.vb" Inherits="Purchase.POIssue" %>

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

        <h3>PO Issue</h3>

        <form id="POForm" runat="server">
            <div class="main">
                <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>

<%  If bo_DisplayForm = True Then%>
                <table class="left">
                    <tr>
                        <th>RFQ Reference Number : </th>
                        <td><asp:Label ID="RFQNumber" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Priority : </th>
                        <td>
                            <asp:DropDownList ID="Priority" runat="server"></asp:DropDownList>
                            <asp:Label ID="LabelPriority" runat="server"></asp:Label>
                        </td>
                    </tr>
    <% If Not String.IsNullOrEmpty(st_ParPONumber) Then%>
                    <tr>
                        <th>Par-PO Number : </th>
                        <td><asp:Label ID="ParPONumber_Label" runat="server" Text=""></asp:Label></td>
                    </tr>
    <% End If%>
                    <tr>
                        <th>R/3 PO Number : </th>
                        <td>
                            <asp:TextBox ID="R3PONumber" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                            <asp:TextBox ID="R3POLineNumber" runat="server" Width="5em" MaxLength="5"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>PO Date <span class="required">*</span> : </th>
                        <td><asp:TextBox ID="PODate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                    </tr>
                    <tr>
                        <th>PO-User <span class="required">*</span> : </th>
                        <td>
                            <asp:DropDownList ID="POUser" runat="server" DataSourceID="SrcUser" DataTextField="Name" DataValueField="UserID">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SrcUser" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                            (<asp:Label ID="POLocationName" runat="server" Text=""></asp:Label>)
                        </td>
                    </tr>
                    <tr>
                        <th>Product Number / Name : </th>
                        <td><asp:Label ID="ProductNumber" runat="server" Text=""></asp:Label>
                        <span class="indent"><asp:Label ID="ProductName" runat="server" Text=""></asp:Label></span></td>
                    </tr>
                    <tr>
                        <th>Order Quantity <span class="required">*</span> : </th>
                        <td>
                            <asp:TextBox ID="OrderQuantity" runat="server" Width="5em" MaxLength="11" CssClass="number"></asp:TextBox>
                            <asp:DropDownList ID="OrderUnit" runat="server" DataSourceID="SrcUnit" DataTextField="UnitCode" DataValueField="UnitCode">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SrcUnit" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <th>Delivery Date : </th>
                        <td><asp:TextBox ID="DeliveryDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                    </tr>
                    <tr>
                        <th>Price : </th>
                        <td><asp:Label ID="CurrencyCode" runat="server" Text=""></asp:Label> <asp:Label ID="UnitPrice" runat="server" Text=""></asp:Label> / <asp:Label ID="PerQuantity" runat="server" Text=""></asp:Label> <asp:Label ID="PerUnit" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>TCI-J Supplier Name : </th>
                        <td>
                            <asp:DropDownList ID="Supplier" runat="server" DataSourceID="SrcSupplier" DataTextField="Name" DataValueField="SupplierCode">
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SrcSupplier" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <th>TCI-J Maker Code : </th>
                        <td><asp:Label ID="R3MakerCode" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>TCI-J Maker Name : </th>
                        <td><asp:Label ID="R3MakerName" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Payment Terms : </th>
                        <td><asp:Label ID="PaymentTerm" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Incoterms : </th>
                        <td><asp:Label ID="Incoterms" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Terms of Delivery : </th>
                        <td><asp:Label ID="DeliveryTerm" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Purpose : </th>
                        <td>
                            <asp:DropDownList ID="Purpose" runat="server" DataSourceID="SrcPurpose" DataTextField="Text" DataValueField="PurposeCode" AppendDataBoundItems="true">
                                <asp:ListItem></asp:ListItem>
                            </asp:DropDownList>
                            <asp:SqlDataSource ID="SrcPurpose" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                        </td>
                    </tr>
                    <tr>
                        <th>Raw Material for : </th>
                        <td><asp:TextBox ID="RawMaterialFor" runat="server" Width="15em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Requested By : </th>
                        <td><asp:TextBox ID="RequestedBy" runat="server" Width="15em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Supplier Item Number : </th>
                        <td><asp:TextBox ID="SupplierItemNumber" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Supplier Lot Number : </th>
                        <td><asp:TextBox ID="SupplierLotNumber" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                    </tr>
                </table>

                <table>
                    <tr>
                        <th>Due Date : </th>
                        <td><asp:TextBox ID="DueDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                    </tr>
                </table>
                
                <div class="btns">
                    <asp:Button ID="Issue" runat="server" Text="Issue" />
                </div>
                
                <asp:HiddenField ID="RFQLineNumber" runat="server" />
                <asp:HiddenField ID="ParPONumber" runat="server" />
                <asp:HiddenField ID="POLocationCode" runat="server" />
                <asp:HiddenField ID="ProductID" runat="server" />
                <asp:HiddenField ID="MakerCode" runat="server" />
                <asp:HiddenField ID="PaymentTermCode" runat="server" />
                <asp:HiddenField ID="IncotermsCode" runat="server" />
                <asp:HiddenField ID="Action" runat="server" Value="Issue" />
<% End If%>
            </div>
        </form>

    </div><!-- Main Content Area END -->

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="POUpdate.aspx.vb" Inherits="Purchase.POUpdate" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>
    <!-- Main Content Area -->
    <div id="content">
        <form id="POForm" runat="server">
            <div class="tabs"><a href = "#" id = "POCorrespondence" onclick="popup('./POCorrespondence.aspx?PONumber=<%Response.Write(st_ParPONumber)%>')">PO Correspondence / History</a></div>
            <h3>PO Update</h3>
            <div class="main">
                <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>

            <% If b_FormVisible Then%>
                
                    <table class="left">
                        <tr>
                            <th>RFQ Reference Number : </th>
                            <td><asp:Label ID="RFQNumber" runat="server" Text=""></asp:Label></td>
                        </tr>
                        
                        <% If b_ChildVisible Then%>
                        <tr>
                            <th>Par-PO Number : </th>
                            <td><asp:Label ID="ParPONumber" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <% End If%> 
                        
                        <tr>
                            <th>Priority : </th>
                            <td>
                                <asp:DropDownList ID="Priority" runat="server"></asp:DropDownList>
                                <asp:Label ID="LabelPriority" runat="server"></asp:Label>
                            </td>
                        </tr>
                                                
                        <tr>
                            <th>R/3 PO Number : </th>
                            <td><asp:TextBox ID="R3PONumber" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                            <asp:TextBox ID="R3POLineNumber" runat="server" Width="5em" MaxLength="5"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>PO Date  : </th>
                            <td><asp:Label ID="PODate" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <th>PO-User  : </th>
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
                            <th>Order Quantity  : </th>
                            <td><asp:Label ID="OrderQuantity" runat="server" Text=""></asp:Label> <asp:Label ID="OrderUnit" runat="server" Text=""></asp:Label> </td>
                        </tr>
                        <tr>
                            <th>Delivery Date : </th>
                            <td><asp:TextBox ID="DeliveryDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                        </tr>
                        <tr>
                            <th>Price : </th>
                            <td><asp:Label ID="Currency" runat="server" Text=""></asp:Label> <asp:Label ID="UnitPrice" runat="server" Text=""></asp:Label> / <asp:Label ID="PerQuantity" runat="server" Text=""></asp:Label> <asp:Label ID="PerUnit" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <th>TCI-J Supplier Code : </th>
                            <td><asp:Label ID="R3SupplierCode" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <th>TCI-J Supplier Name : </th>
                            <td><asp:Label ID="R3SupplierName" runat="server" Text=""></asp:Label></td>
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
                            <td><asp:Label ID="Purpose" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <th>Raw Material for : </th>
                            <td><asp:Label ID="RawMaterialFor" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <th>Requested By : </th>
                            <td><asp:Label ID="RequestedBy" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <th>Supplier Item Number : </th>
                            <td><asp:Label ID="SupplierItemNumber" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <th>Supplier Lot Number : </th>
                            <td><asp:Label ID="SupplierLotNumber" runat="server" Text=""></asp:Label></td>
                        </tr>
                    </table>

                    <table>
                        <tr>
                            <th>Due Date : </th>
                            <td><asp:TextBox ID="DueDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                        </tr>
                        <tr>
                            <th>Goods Arrived Date : </th>
                            <td><asp:TextBox ID="GoodsArrivedDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                        </tr>
                        <tr>
                            <th>TCI Lot Number : </th>
                            <td><asp:TextBox ID="LotNumber" runat="server" Width="10em" MaxLength="10"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>Commercial Invoice Received Date : </th>
                            <td><asp:TextBox ID="InvoceReceivedDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                        </tr>
                        <tr>
                            <th>Import Custom Clearance Date : </th>
                            <td><asp:TextBox ID="ImportCustomClearanceDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                        </tr>
                        <tr>
                            <th>QM Starting Date : </th>
                            <td><asp:TextBox ID="QMStartingDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                        </tr>
                        <tr>
                            <th>QM Finish Date : </th>
                            <td><asp:TextBox ID="QMFinishDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                        </tr>
                        <tr>
                            <th>QM Result : </th>
                            <td><asp:TextBox ID="QMResult" runat="server" Width="15em" MaxLength="255"></asp:TextBox></td>
                        </tr>

                        <% If b_ChildVisible Then%>            
                        <tr>
                            <th>Chi-PO Request Quantity : </th>
                            <td><asp:TextBox ID="RequestQuantity" runat="server" Width="15em" MaxLength="255"></asp:TextBox></td>
                        </tr>
                        <% End If%> 
                       
                        <tr>
                            <th>Scheduled Export Date : </th>
                            <td><asp:TextBox ID="ScheduledExportDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                        </tr>
                        <tr>
                            <th>Purchasing Requisition Number : </th>
                            <td><asp:TextBox ID="PurchasingRequisitionNumber" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <th>Order Cancellation Date : </th>
                            <td><asp:TextBox ID="CancellationDate" runat="server" Width="7em" MaxLength="10"></asp:TextBox> <span class="format">(YYYY-MM-DD)</span></td>
                        </tr>
                        <tr>
                            <th></th>
                            <% If b_ChiPOIssueVisible Then%>
                            <td style="text-align:right"><asp:HyperLink ID="ChiPOIssue" runat="server" NavigateUrl="./POIssue.aspx">Chi-PO Issue</asp:HyperLink></td>
                            <% End If%>                      
                        </tr>
                    </table>
				    <asp:HiddenField ID="UpdateDate" runat="server" />

                    <div class="btns">
                        <p class="message"><asp:Label ID="RunMsg" runat="server"></asp:Label></p>
                        <asp:Button ID="Update" runat="server" Text="Update" />
                        <span class="indent"></span>
                        <asp:Button ID="Cancell" runat="server" Text="Cancel" />
                    </div>
                </div>

            <% End If%>

        </form>

    </div><!-- Main Content Area END -->

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

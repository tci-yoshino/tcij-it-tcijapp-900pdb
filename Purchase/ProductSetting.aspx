<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProductSetting.aspx.vb" Inherits="Purchase.ProductSetting" %>

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
        <div class="tabs"><a href="./SupplierListByProduct.aspx">Supplier List</a></div>

        <h3>Product Setting</h3>

        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>

            <form id="Product" runat="server">
                <table class="left">
                    <tr>
                        <th>Product Number <span class="required">*</span> : </th>
                        <td><asp:TextBox ID="Code" runat="server" Width="7em" MaxLength="32"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Product Name 1 <span class="required">*</span> : </th>
                        <td><asp:TextBox ID="ProductName" runat="server" Width="21em" MaxLength="1000"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Product Name 2 : </th>
                        <td><asp:TextBox ID="QuoName" runat="server" Width="21em" MaxLength="1000"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>CAS Number : </th>
                        <td><asp:TextBox ID="CASNumber" runat="server" Width="7em" MaxLength="32"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Molecular Formula : </th>
                        <td><asp:TextBox ID="MolecularFormula" runat="server" Width="10em" MaxLength="128"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Reference : </th>
                        <td><asp:TextBox ID="Reference" runat="server" Columns="50" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Comment : </th>
                        <td><asp:TextBox ID="Comment" runat="server" Columns="50" Rows="3" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                </table>

                <table>
                    <tr>
                        <th>Status : </th>
                        <td><asp:Label ID="Status" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Proposal Department : </th>
                        <td><asp:Label ID="ProposalDept" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Procument Department : <br />(or Manufacturing Department)&nbsp;&nbsp;&nbsp;</th>
                        <td><asp:Label ID="ProcumentDept" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>P/D : </th>
                        <td><asp:Label ID="PD" runat="server" Text=""></asp:Label></td>
                    </tr>
                </table>
                <asp:HiddenField ID="ProductID" runat="server" />

                <div class="btns">
                    <asp:Button ID="Save" runat="server" Text="Save" />
                </div>
            </form>
        </div>
    </div><!-- Main Content Area END -->

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

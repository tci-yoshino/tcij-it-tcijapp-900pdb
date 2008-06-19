<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SuppliersProductImport.aspx.vb" Inherits="Purchase.SuppliersProductImport" %>

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

        <h3>Excel Import to Suppliers Product</h3>

        <form id="ExcelImportForm" runat="server">
            <div class="main">
                <p class="attention"></p>

                <table>
                    <tr>
                        <th>Supplier Code : </th>
                        <td><asp:Label ID="SupplierCode" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>Supplier Name : </th>
                        <td><asp:Label ID="SupplierName" runat="server" Text=""></asp:Label></td>
                    </tr>
                    <tr>
                        <th>File : </th>
                        <td><asp:FileUpload ID="File" runat="server" /> 
                            <asp:Button ID="Preview" runat="server" Text="Preview" /></td>
                    </tr>
                </table>

                <p><a href="./Sample.xls">Sample Download</a></p>
            </div>

            <hr />

            <div class="list">
                <asp:GridView ID="SupplierProductList" runat="server" DataSourceID="SrcSupplierProduct">
                    <Columns>
                        <asp:BoundField HeaderText="CAS Number" />
                        <asp:BoundField HeaderText="Supplier Item Number" />
                        <asp:BoundField HeaderText="Supplier Item Name" />
                        <asp:BoundField HeaderText="Note" />
                        <asp:BoundField HeaderText="TCI Product Number" />
                        <asp:BoundField HeaderText="EHS Status" />
                        <asp:BoundField HeaderText="Proposal Dept" />
                        <asp:BoundField HeaderText="Proc.Dept / Manu.Dept" />
                        <asp:BoundField HeaderText="AD" />
                        <asp:BoundField HeaderText="AF" />
                        <asp:BoundField HeaderText="WA" />
                        <asp:BoundField HeaderText="KA" />
                    </Columns>
                </asp:GridView>

                <div class="btns">
                    <asp:Button ID="Import" runat="server" Text="Import" />
                </div>
            </div>
        </form>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcSupplierProduct" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
    
    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

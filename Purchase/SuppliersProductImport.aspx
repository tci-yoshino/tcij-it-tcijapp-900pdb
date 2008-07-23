﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SuppliersProductImport.aspx.vb" Inherits="Purchase.SuppliersProductImport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
        <script type="text/javascript">
<!--
function set_Action(action){
  if (action == "Preview") {
    document.forms["ExcelImportForm"].Action.value = action
    document.forms["ExcelImportForm"].submit();
    return true;
  }else if(action == "Import"){
    document.forms["ExcelImportForm"].Action.value = action
    document.forms["ExcelImportForm"].submit();
    return true;
  }else{
    return false;
  }
}
//-->
    </script>
</head>
<body>

        <form id="ExcelImportForm" runat="server">
    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"></div>

        <h3>Excel Import to Suppliers Product</h3>

            <div class="main">
                <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>

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
                            <asp:Button ID="Preview" runat="server" Text="Preview" onclientclick="javascript:set_Action('Preview');" /></td>
                    </tr>
                </table>

                <p><a href="./Sample.xls">Sample Download</a></p>
            </div>

            <hr />

            <div class="list">
                <asp:GridView ID="SupplierProductList" runat="server" DataKeyNames="CAS Number">
                    <Columns>
                        <asp:CommandField EditText="Edit" ShowEditButton="True" />
                    </Columns>
                </asp:GridView>

                <div class="btns">
                    <asp:Button ID="Import" runat="server" Text="Import" Visible="False" onclientclick="javascript:set_Action('Import');" />
                </div>
            </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcSupplierProduct" runat="server" 
        ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" SelectCommand="SELECT                  ProductID, ProductNumber, JapaneseName, ChineseName, CASNumber
FROM                     dbo.Product
WHERE                   (ProductID &lt; 100)"></asp:SqlDataSource>
    
                <asp:HiddenField ID="ProductID" runat="server" />
                <asp:HiddenField ID="ImportFileName" runat="server" />
                <input type="hidden" id ="Action" runat="server" value="" />
    
    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
        </form>
    </body>
</html>

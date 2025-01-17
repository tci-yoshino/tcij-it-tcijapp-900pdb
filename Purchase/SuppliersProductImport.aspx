﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SuppliersProductImport.aspx.vb" Inherits="Purchase.SuppliersProductImport" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
    <script type="text/javascript">
        //-----------------------------
        // マウスカーソルのStyleを変更
        //-----------------------------
        function ChangeCursorStyle(curStyle) {
            var obj;
            document.getElementById("ExcelImportForm").style.cursor = curStyle;
            document.getElementById("File").style.cursor = curStyle;
            document.getElementById("Preview").style.cursor = curStyle;
            obj = document.getElementById("ReCheck")
            if (obj) {
                document.getElementById("ReCheck").style.cursor = curStyle;
            }
            obj = document.getElementById("Import")
            if (obj) {
                document.getElementById("Import").style.cursor = curStyle;
            }          
        }
    </script>
</head>
<body onload="ChangeCursorStyle('default')">
        <!-- Header -->
        <commonUC:Header ID="HeaderMenu" runat="server" />
        <!-- Header End -->

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
                            <asp:Button ID="Preview" runat="server" Text="Preview" 
                                UseSubmitBehavior="false" OnClientClick="ChangeCursorStyle('wait')" />
                        </td>
                    </tr>
                </table>

                <p><a href="./Sample.xls">Sample Download</a></p>
            </div>

            <div class="note">
                <strong>&lt;&lt; Attention &gt;&gt;</strong>
                <ul>
                    <li>"Sheet1" has to contain the same column as "Sample" sheet.
                        <ul>
                            <li>Sheet name has to be "Sheet1"</li>
                            <li>You may freely use column from column-E</li>
                            <li>Please split a file with more than 1000 lines</li>
                        </ul>
                    </li>
                    <li>Original MS Excel file has to be closed when you preview the import data through the system.</li>
                </ul>
            </div>

            <hr />

            <div class="list">
                <asp:GridView ID="SupplierProductList" runat="server" 
                    AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="CAS Number">
                            <ItemTemplate>
                                <asp:TextBox ID="CASNumber" runat="server" Text='<%# Eval("CASNumber") %>'></asp:TextBox>                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Supplier Item Number" 
                            DataField="SupplierItemNumber" />
                        <asp:TemplateField HeaderText="Supplier Item Name">
                            <ItemTemplate>
                                <asp:Label ID="SupplierItemName" runat="server" Text='<%# Eval("SupplierItemName") %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Note" DataField="Note" />

                        <asp:TemplateField HeaderText="TCI Product Number">
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="EHS Status">
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Proposal Dept">
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Proc.Dept / Manu.Dept">
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="AD">
                            <ItemTemplate>
                                <%#Eval("AD")%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="AF">
                            <ItemTemplate>
                                <%#Eval("AF")%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="WA">
                            <ItemTemplate>
                                <%#Eval("WA")%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="KA">
                            <ItemTemplate>
                                <%#Eval("KA")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                
                <div class="btns">
                    <asp:Button ID="ReCheck" runat="server" Text="ReCheck" UseSubmitBehavior="false" OnClientClick="ChangeCursorStyle('wait')" />
                    <span class="indent"></span>
                    <asp:Button ID="Import" runat="server" Text="Import" 
                        UseSubmitBehavior="false" OnClientClick="ChangeCursorStyle('wait')" />
                    <span class="indent"></span>
                    <asp:Button ID="Export" runat="server" Text="Export" 
                        UseSubmitBehavior="false"  />
                </div>
            </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcSupplierProduct" runat="server" 
        ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" SelectCommand="SELECT                  ProductID, ProductNumber, JapaneseName, ChineseName, CASNumber
FROM                     dbo.Product
WHERE                   (ProductID &lt; 100)"></asp:SqlDataSource>
    
                <asp:HiddenField ID="ImportFileName" runat="server" />
                <input type="hidden" id ="Action" runat="server" value="" />
    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
        </form>
    </body>
</html>

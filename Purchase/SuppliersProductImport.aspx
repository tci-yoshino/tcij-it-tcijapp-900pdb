<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SuppliersProductImport.aspx.vb" Inherits="Purchase.SuppliersProductImport" %>
<%  Server.ScriptTimeout = 300%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
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
                            <asp:Button ID="Preview" runat="server" Text="Preview" 
                                UseSubmitBehavior="false" /></td>
                    </tr>
                </table>

                <p><a href="./Sample.xls">Sample Download</a></p>
            </div>

            <hr />

            <div class="list">
                <asp:GridView ID="SupplierProductList" runat="server" 
                    AutoGenerateColumns="False">
                    <Columns>
                        <asp:TemplateField HeaderText="CAS Number">
                            <ItemTemplate>
                                <asp:TextBox ID="CASNumber" runat="server" Text='<%# Eval("CAS Number") %>'></asp:TextBox>                                
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField HeaderText="Supplier Item Number" 
                            DataField="Supplier Item Number" />
                        <asp:BoundField HeaderText="Supplier Item Name" 
                            DataField="Supplier Item Name" />
                        <asp:BoundField HeaderText="Note" DataField="Note" />

                        <asp:TemplateField HeaderText="TCI Product Number">
                            <ItemTemplate>
                                <%#Eval("TCI Product Number")%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="EHS Status">
                            <ItemTemplate>
                                <%#Eval("EHS Status")%>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Proposal Dept">
                            <ItemTemplate>
                                <%#Eval("Proposal Dept")%>
                            </ItemTemplate>
                        </asp:TemplateField>
             
                        <asp:TemplateField HeaderText="Proc.Dept / Manu.Dept">
                            <ItemTemplate>
                                <%#Eval("Proc_Dept")%>
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
                    <asp:Button ID="ReCheck" runat="server" Text="ReCheck" UseSubmitBehavior="false" />
                    <span class="indent"></span>
                    <asp:Button ID="Import" runat="server" Text="Import" 
                        UseSubmitBehavior="false" />
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

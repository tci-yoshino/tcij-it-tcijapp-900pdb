<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProductSearchByStructure.aspx.vb" Inherits="Purchase.ProductSearchByStructure" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Purchase DB</title>
        <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8; IE=EmulateIE9" />
        <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
        <link rel="stylesheet" href="./CSS/Print.css" type="text/css" media="print" />
        <script type="text/javascript" src="./JS/Common.js"></script>
        <script type="text/javascript" src="./JS/Colorful.js"></script>
        <script type="text/javascript" language="javascript" src="./jsme/jsme.nocache.js"></script>
        <script type="text/javascript">
            function jsmeOnLoad() {
                var startingStructure = document.getElementById('search_jme').value;
                //Instantiate a new JSME:
                //arguments: HTML id, width, height (must be string not number!)
                jsmeApplet = new JSApplet.JSME("appletContainer", "380px", "340px", {
                    //optional parameters
                    "options": "multipart,norbutton,query,noreaction,nocanonize,noquery",
                    "jme": startingStructure
                });
                //jsmeApplet has the same API as the original Java applet
                //One can mimic the JME Java applet access to simplify the adaptation of HTML and JavaScript code:
                document.JME = jsmeApplet;
            }
            window.onload = function () {
            }
            function search(type) {
                setAction('Search');
                document.getElementById('search_smiles').value = document.JME.smiles();
                document.getElementById('search_jme').value = document.JME.jmeFile();
                document.getElementById('search_type').value = type;
            }
        </script>
    </head>
    <body>
        <!-- Header -->
        <commonUC:Header ID="HeaderMenu" runat="server" />
        <!-- Header End -->
        <form id="form1" runat="server" defaultbutton="SimilaritySearch">
            <!-- Main Content Area -->
            <div id="content">
                <div class="tabs"></div>
                <div class="main">
                    <div class="attention">
                        <asp:Label ID="Msg" runat="server" Text=""></asp:Label>
                    </div>
                    <table>
                        <tr>
                            <td id="appletContainer" colspan="2"></td>
                        </tr>
                    </table>
                    <div class="abtns">
                        <asp:Button ID="SimilaritySearch" runat="server" Text="Similarity Search" Width="12em" OnClientClick="search('-simisearch')"  />　
                    </div>
                    <input type="hidden" id ="Action" runat="server" value="" />
                    <asp:HiddenField runat="server" ID="search_smiles" />
                    <asp:HiddenField runat="server" ID="search_type" />
                    <asp:HiddenField runat="server" ID="search_jme" />
                </div>
            </div>
            <div class="list">
                <asp:ListView ID="StructureList" runat="server" >
                    <LayoutTemplate>
        <table id="itemPlaceholderContainer" runat="server">
                            <tr id="Tr1" runat="server">
                                <th id="Th1" runat="server" style="width:10%;" >Structure</th>
                                <th id="Th2" runat="server" style="width:5%">Similarity</th>
                                <th id="Th3" runat="server" style="width:10%">Product Number</th>
                                <th id="Th4" runat="server" style="width:10%">CAS Number</th>
                                <th id="Th5" runat="server" style="width:40%">Product Name</th>
                                <th id="Th6" runat="server" style="width:5%"></th>
                                <th id="Th7" runat="server" style="width:5%"></th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server"></tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr id="TrSearchResult" runat="server" class='<%#IIf((Container.DataItemIndex Mod 2) = 0, "", "zebra") %>' onclick=''>
                            <td><asp:Image ID="pStructure" runat="server" CssClass="structure-img" src='<%# Eval("pStructure") %>' onerror="this.src='./Image/NoImage.gif'"  /></td>
                            <td class="number percent"><asp:Label ID="Similariry" runat="server" Text='<%# Eval("Similarity") %>' /></td>
                            <td><asp:Label ID="ProductNumber" runat="server" Text='<%# Eval("ProductNumber") %>' /></td>
                            <td><asp:Label ID="CASNumber" runat="server" Text='<%# Eval("CASNumber") %>' /></td>
                            <td><asp:Label ID="ProductName" runat="server" Text='<%# Eval("ProductName") %>' /></td>
                            <td><asp:HyperLink ID="Setting" runat="server"
                                        NavigateUrl='<%#"./ProductSetting.aspx?Action=Edit&ProductID=" & Eval("ProductID") %>' Target="_blank" >Product Setting</asp:HyperLink></td>
                            <td><asp:HyperLink ID="RFQList" runat="server"
                                        NavigateUrl='<%#"./RFQListByProduct.aspx?ProductID=" & Eval("ProductID") %>' Target="_blank" >RFQ List</asp:HyperLink></td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <h3 style="font-style:italic"><% =Purchase.Common.ERR_NO_MATCH_FOUND%></h3>
                    </EmptyDataTemplate>
                </asp:ListView>
            </div>
            <!-- Main Content Area END -->
            <asp:SqlDataSource ID="SrcStructure" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
        </form>

        <!-- Footer -->
        <!--#include virtual="./Footer.html" -->
        <!-- Footer END -->
    </body>
</html>

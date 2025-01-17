﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MakerSelect.aspx.vb" Inherits="Purchase.MakerSelect" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=EmulateIE8; IE=EmulateIE9" />
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
    <script type="text/javascript">
    <!--

window.onload = function() {
   colorful.set();
   changeCellColor("SupplierList_itemPlaceholderContainer")
   document.SearchForm.Code.focus();
}
function returnValues(code, name, countryName,sapcode){
      if(opener){
        opener.document.getElementById('MakerCode').value=code
        opener.document.getElementById('MakerName').value=name
        opener.document.getElementById('MakerCountry').value=countryName
        opener.document.getElementById('SAPMakerCode').value=sapcode
       }
       window.close();
    }

    -->
    </script>
</head>
<body>
    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"></div>

        <h3>Supplier Select</h3>

        <div class="main">
            <p class="attention">
                <asp:Label ID="Msg" runat="server"></asp:Label></p>
            <form id="SearchForm" runat="server">
                <table>
                    <tr>
                        <th>Supplier Code : </th>
                        <td>
                            <asp:TextBox ID="Code" runat="server" Width="7em" MaxLength="5"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Supplier Name : </th>
                        <td>
                            <asp:TextBox ID="Name" runat="server" Width="21em" MaxLength="255"></asp:TextBox>
                            (Partial text match)</td>
                    </tr>
                </table>

                <asp:Button ID="Search" runat="server" Text="Search" PostBackUrl="MakerSelect.aspx?Action=Search" />
                <input type="button" value="Clear" onclick="clearForm('SearchForm')" />
            </form>
        </div>

        <hr />

        <div class="list">
            <asp:ListView ID="SupplierList" runat="server" DataSourceID="SrcMaker">
                <LayoutTemplate>
                    <table id="itemPlaceholderContainer" runat="server">
                        <tr id="Tr1" runat="server">
                            <th id="Th1" runat="server" style="width: 15%">Supplier Code</th>
                            <th id="Th2" runat="server" style="width: 70%">Supplier Name</th>
                            <th id="Th3" runat="server" style="width: 15%">Supplier code in SAP</th>
                        </tr>
                        <tr id="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <h3 style="font-style: italic"><%=Purchase.Common.ERR_NO_MATCH_FOUND%></h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr onclick="returnValues('<%#Eval("SupplierCode")%>','<%# Replace(Eval("Name").ToString(), "'", "\'")%>','<%#Eval("CountryName") %>','<%#Eval("S4SupplierCode")%>');">
                        <td>
                            <asp:Label ID="SupplierCode" runat="server" Text='<%#Eval("SupplierCode")%>' /></td>
                        <td>
                            <asp:Label ID="SupplierName3" runat="server" Text='<%#Eval("Name")%>' />
                        </td>
                        <td>
                            <asp:Label ID="QuoLocation" runat="server" Text='<%#Eval("S4SupplierCode")%>' />
                        </td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div>
    <!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcMaker" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>


</body>
</html>

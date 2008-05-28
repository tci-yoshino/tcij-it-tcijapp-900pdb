<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CountryList.aspx.vb" Inherits="Purchase.CountryList" %>

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
        <div class="tabs"><a href="~/CountrySetting.aspx">New Country</a></div>

        <h3>Country</h3>

        <div class="list">
            <p class="attention"></p>

            <asp:Repeater ID="CountryList" runat="server" DataSourceID="SrcCountry">
                <HeaderTemplate>
                    <table>
                        <tr>
                            <th style="width:10%">Country Code</th>
                            <th style="width:15%">Country Name</th>
                            <th style="width:15%">Default Quo-Location</th>
                            <th>Edit</th>
                        </tr>
                </HeaderTemplate>
                <ItemTemplate>
                        <tr>
                            <td><asp:Label ID="CountryCode" runat="server" Text="" /></td>
                            <td><asp:Label ID="CountryName" runat="server" Text="" /></td>
                            <td><asp:Label ID="DefaultQuoLocationName" runat="server" Text="" /></td>
                            <td><asp:HyperLink ID="Edit" runat="server" NavigateUrl="./CountrySetting.aspx">Edit &raquo;</asp:HyperLink></td>
                        </tr>
                </ItemTemplate>
                <FooterTemplate>
                    </table>
                </FooterTemplate>
            </asp:Repeater>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcCountry" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

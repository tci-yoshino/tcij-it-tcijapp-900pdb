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
        <div class="tabs"><a href="./CountrySetting.aspx">New Country</a></div>

        <h3>Country</h3>

        <div class="list">
            <p class="attention"></p>

            <asp:ListView ID="CountryList" runat="server" DataSourceID="SrcCountry" DataKeyNames="CountryCode">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server" border="0" style="">
                        <tr runat="server" style="">
                            <th runat="server" style="width:10%">Country Code</th>
                            <th runat="server" style="width:15%">Country Name</th>
                            <th runat="server" style="width:15%">Default Quo-Location</th>
                            <th></th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <h3 style="font-style:italic">No data found.</h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr>
                        <td><asp:Label ID="CountryCodeLabel" runat="server" Text='<%# Eval("CountryCode") %>' /></td>
                        <td><asp:Label ID="CountryNameLabel" runat="server" Text='<%# Eval("CountryName") %>' /></td>
                        <td><asp:Label ID="DefaultQuoLocationNameLabel" runat="server" Text='<%# Eval("DefaultQuoLocationName") %>' /></td>
                        <td><asp:HyperLink ID="Edit" runat="server" NavigateUrl="./CountrySetting.aspx">Edit</asp:HyperLink></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcCountry" runat="server" 
    ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
    SelectCommand="SELECT [CountryCode], [CountryName], [DefaultQuoLocationName] FROM [v_Country]"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

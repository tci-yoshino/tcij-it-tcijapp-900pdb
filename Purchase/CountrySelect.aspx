﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="CountrySelect.aspx.vb" Inherits="Purchase.CountrySelect" %>

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

        <h3>Country Select</h3>

        <div class="main">
            <p class="attention"></p>

            <form id="Form1" runat="server">
                <table>
                    <tr>
                        <th>Country Code : </th>
                        <td><asp:TextBox ID="Code" runat="server" Width="7em" MaxLength="5"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Country Name : </th>
                        <td><asp:TextBox ID="Name" runat="server" Width="21em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                </table>

                <asp:Button ID="Search" runat="server" Text="Search" />
                <input type="button" value="Clear" />
            </form>
        </div>

        <hr />

        <div class="list">
            <asp:ListView ID="CountryList" runat="server" AutoGenerateColumns="False" DataSourceID="SrcCountry">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server">
                        <tr runat="server">
                            <th runat="server" style="width:30%">Country Code</th>
                            <th runat="server" style="width:70%">Country Name</th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <h3 style="font-style:italic">No match found.</h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr>
                        <td><asp:Label ID="CountryCode" runat="server" Text='' /></td>
                        <td><asp:Label ID="CountryName" runat="server" Text='' /></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>

    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcCountry" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
</body>
</html>
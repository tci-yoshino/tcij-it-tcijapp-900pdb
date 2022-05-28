<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQSearchBySupplier.aspx.vb" Inherits="Purchase.RFQSearchBySupplier" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
    <script type="text/javascript">
    <!--
window.onload = function() {
    colorful.set();
    document.SearchForm.SupplierCode.focus();
}
    -->
    </script>
</head>
<body>
    <!-- Header -->
    <commonUC:Header ID="HeaderMenu" runat="server" />
    <!-- Header End -->
    <form id="SearchForm" runat="server">
    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"><a href="#" onclick="popup('./SupplierSetting.aspx')">New Supplier</a></div>

        <h3>RFQ Search by Supplier</h3>

        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
            
                <table>
					<tr>
						<th>Supplier Code : </th>
						<td><asp:TextBox ID="SupplierCode" runat="server" Width="7em" MaxLength="10"></asp:TextBox></td>
					</tr>
					<tr>
						<th>SAP Supplier Code : </th>
						<td><asp:TextBox ID="R3SupplierCode" runat="server" Width="7em" MaxLength="10"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Supplier Name : </th>
						<td><asp:TextBox ID="SupplierName" runat="server" Width="21em" MaxLength="255"></asp:TextBox> (Partial text match)</td>
					</tr>
                    <tr>
                        <th>Country : </th>                        
                        <td><asp:ScriptManager ID="SM_RSS" runat="server"></asp:ScriptManager>
                                <asp:UpdatePanel ID="UP_Country" runat="server">
                                    <ContentTemplate>
                                        <asp:DropDownList ID="Country" runat="server" AppendDataBoundItems="True" 
                                DataSourceID="SDS_SBS_Country" DataTextField="CountryName" 
                                DataValueField="CountryCode" AutoPostBack="True">
                                            <asp:ListItem></asp:ListItem>
                                        </asp:DropDownList>
                                    </ContentTemplate>
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="Clear" EventName="Click" />
                                    </Triggers>
                            </asp:UpdatePanel>
                            <asp:SqlDataSource ID="SDS_SBS_Country" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                                 SelectCommand="SELECT DISTINCT v_Country.CountryCode, v_Country.CountryName FROM v_Country INNER JOIN Supplier ON v_Country.CountryCode = Supplier.CountryCode ORDER BY v_Country.CountryName">
                            </asp:SqlDataSource>

                        </td>
                    </tr>
                    <tr>
                        <th>Region : </th>
                        <td><asp:UpdatePanel ID="UP_Region" runat="server">
                                <ContentTemplate>
                                    <asp:DropDownList ID="Region" runat="server" AppendDataBoundItems="True" 
                                        DataSourceID="SDS_SBS_Region" 
                                        DataTextField="Name" DataValueField="RegionCode">
                                        <asp:ListItem></asp:ListItem>
                                    </asp:DropDownList>
                                </ContentTemplate>
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="Country" 
                                        EventName="SelectedIndexChanged" />
                                </Triggers>
                            </asp:UpdatePanel>
                            <asp:SqlDataSource ID="SDS_SBS_Region" runat="server" 
                                ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
                                
                                
                                SelectCommand="SELECT DISTINCT RegionCode, Name
FROM                     s_Region
WHERE                   (RegionCode IN
                                      (SELECT DISTINCT RegionCode
                                            FROM                     Supplier
                                            WHERE                   (CountryCode = @Country))) AND (CountryCode = @Country) ORDER BY s_Region.Name">
                                <SelectParameters>
                                    <asp:ControlParameter ControlID="Country" Name="Country" 
                                        PropertyName="SelectedValue" />
                                </SelectParameters>
                            </asp:SqlDataSource>

                        </td>
                    </tr>
                </table>

				<asp:Button ID="Search" runat="server" Text="Search" />
				<asp:Button ID="Clear" runat="server" Text="Clear" OnClientClick ="clearForm('SearchForm');" />
            </div>

        <hr />

        <div class="list">
            <asp:ListView ID="SupplierList" runat="server" DataSourceID="SrcSupplier">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server">
                        <tr id="Tr1" runat="server">
                            <th id="Th1" runat="server" style="width:15%">Supplier Code</th>
                            <th id="Th2" runat="server" style="width:15%">SAP Supplier Code</th>
                            <th id="Th3" runat="server" style="width:70%">Supplier Name</th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <EmptyDataTemplate>
                        <h3 style="font-style:italic"><% =Purchase.Common.ERR_NO_MATCH_FOUND%></h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr style="">
                        <td><asp:HyperLink ID="SupplierCode" runat="server" NavigateUrl='<%#Eval("SupplierCode","./RFQListBySupplier.aspx?SupplierCode={0}")%>' Text = '<%#Eval("SupplierCode")%>' /></td>
                        <td><asp:HyperLink ID="R3SupplierCode" runat="server" NavigateUrl='<%#Eval("SupplierCode","./RFQListBySupplier.aspx?SupplierCode={0}")%>' Text = '<%#Eval("S4SupplierCode")%>' /></td>
                        <td><asp:HyperLink ID="SupplierName" runat="server" NavigateUrl='<%#Eval("SupplierCode","./RFQListBySupplier.aspx?SupplierCode={0}")%>' Text = '<%#Eval("SupplierName")%>' /></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>

    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcSupplier" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
            </form>
        </body>
</html>

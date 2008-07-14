<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQSupplierSelect.aspx.vb" Inherits="Purchase.RFQSupplierSelect" %>

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
   changeCellColor("SupplierList_itemPlaceholderContainer")
   
}
    function returnValues(code, r3code, name3, name4, countryCode, location){
      if(opener){
        var name = name3 + " " + name4;
        if (name3 == "") name = name4;
        opener.document.getElementById('SupplierCode').value=code
        opener.document.getElementById('R3SupplierCode').value=r3code
        opener.document.getElementById('SupplierName').value=name
        opener.document.getElementById('SupplierCountry').value=countryCode
        if(opener.document.getElementById('QuoLocation')){
          opener.document.getElementById('QuoLocation').selectedIndex = location
        }
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
			<p class="attention"><asp:Label ID="ErrorMessages" runat="server"></asp:Label></p>
<%  If Not String.IsNullOrEmpty(st_Location) Then%>
			<form id="SearchForm" runat="server">
				<table>
					<tr>
						<th>Supplier Code : </th>
						<td><asp:TextBox ID="Code" runat="server" Width="7em" MaxLength="5"></asp:TextBox></td>
					</tr>
					<tr>
						<th>Supplier Name : </th>
						<td><asp:TextBox ID="Name" runat="server" Width="21em" MaxLength="255"></asp:TextBox> (Partial text match)</td>
					</tr>
				</table>

				<asp:HiddenField ID="Location" runat="server" Value="" />
				<asp:Button ID="Search" runat="server" Text="Search" />
				<input type="button" value="Clear" onclick="clearForm('SearchForm')" />
			</form>
		</div>

		<hr />

        <div class="list">
            <asp:ListView ID="SupplierList" runat="server" AutoGenerateColumns="False">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server">
                        <tr id="Tr1" runat="server">
                            <th id="Th1" runat="server" style="width:15%">Supplier Code</th>
                            <th id="Th2" runat="server" style="width:70%">Supplier Name</th>
                            <th id="Th3" runat="server" style="width:15%">Quo Location</th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <h3 style="font-style:italic">No match found.</h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr onclick="returnValues('<%#Eval("SupplierCode")%>','<%#Eval("R3SupplierCode") %>','<%#Eval("Name3")%>','<%#Eval("Name4")%>','<%#Eval("CountryCode") %>','<%#Eval("QuoLocationCode") %>');">
                        <td><asp:Label ID="SupplierCode" runat="server" Text='<%#Eval("SupplierCode")%>' /></td>
                        <td>
                          <asp:Label ID="SupplierName3" runat="server" Text='<%#Eval("Name3")%>' />&nbsp;
                          <asp:Label ID="SupplierName4" runat="server" Text='<%#Eval("Name4")%>' />
                        </td>
                        <td><asp:Label ID="QuoLocationCode" runat="server" Text='<%#Eval("QuoLocationCode") %>' /></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>
<% End If%>

    </div><!-- Main Content Area END -->


</body>
</html>

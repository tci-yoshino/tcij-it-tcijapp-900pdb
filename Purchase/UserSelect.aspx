<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="UserSelect.aspx.vb" Inherits="Purchase.UserSelect" %>

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
   changeCellColor("UserList_itemPlaceholderContainer")
   document.UserForm.LocationName.focus();
}


function returnValues(UserID,LocationName, AccountName, Name) {
  if (opener) {
      opener.document.getElementById('UserID').value = UserID
      opener.document.getElementById('Location').value = LocationName
      opener.document.getElementById('AccountName').value = AccountName
      opener.document.getElementById('Name').value = Name
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

        <h3>User Select</h3>

        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>

            <form id="UserForm" runat="server">
                <table>
                    <tr>
                        <th>Location : </th>
                        <td><asp:DropDownList ID="LocationName" runat="server"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <th>AD_DisplayName : </th>
                        <td><asp:TextBox ID="UserName" runat="server" Width="21em" MaxLength="255"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>AD_DeptName : </th>
                        <td><asp:TextBox ID="DeptName" runat="server" Width="13em" MaxLength="255"></asp:TextBox></td>
                    </tr>   
                </table>

                <asp:Button ID="Search" runat="server" Text="Search" PostBackUrl="UserSelect.aspx?Action=Search" />
                <input type="button" value="Clear" onclick="clearForm('UserForm')" />
            </form>
        </div>

        <hr />

        <div class="list">
            <asp:ListView ID="UserList" runat="server" DataSourceID="SrcUser">
                <LayoutTemplate>
                    <table ID="itemPlaceholderContainer" runat="server">
                        <tr runat="server">
                            <th runat="server" style="width:10%">Location</th>
                            <th runat="server" style="width:20%">AD_AccountName</th>
                            <th runat="server" style="width:30%">AD_DisplayName</th>
                            <th runat="server" style="width:30%">AD_DeptName</th>
                        </tr>
                        <tr ID="itemPlaceholder" runat="server">
                        </tr>
                    </table>
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <h3 style="font-style:italic"><%=Purchase.Common.ERR_NO_MATCH_FOUND%></h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <tr onclick="returnValues('<%#Eval("UserID")%>','<%#Eval("LocationName")%>','<%#Eval("AccountName")%>','<%#Eval("Name")%>');">
                        <td><asp:Label ID="LocationLabel" runat="server" Text='<%#Eval("LocationName")%>' /></td>
                        <td><asp:Label ID="AD_AccountNameLabel" runat="server" Text='<%#Eval("AccountName")%>' /></td>
                        <td><asp:Label ID="AD_DisplayNameLabel" runat="server" Text='<%#Eval("AD_DisplayName")%>' /></td>
                        <td><asp:Label ID="AD_DeptNameNameLabel" runat="server" Text='<%#Eval("AD_DeptName")%>' /></td>
                    </tr>
                </ItemTemplate>
            </asp:ListView>
        </div>

    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcUser" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
</body>
</html>

      
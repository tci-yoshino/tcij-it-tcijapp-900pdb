<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="PurchaseGroupSetting.aspx.vb" Inherits="Purchase.PurchaseGroupSetting" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>
    <!-- Header -->
    <commonUC:Header ID="HeaderMenu" runat="server" />
    <!-- Header End -->

    <form id="CountryForm" runat="server">
        <div id="content">
            <div class="tabs"></div>
            <h3>User Setting</h3>
            <div class="main">
                <p class="attention">
                    <asp:Label ID="Msg" runat="server" Text=""></asp:Label>
                </p>
                <asp:HiddenField ID="UserID" runat="server" />
                <asp:HiddenField ID="LocationCode" runat="server" />
                <table>
                    <tr>
                        <th>User Name : </th>
                        <td>
                            <asp:Label ID="UserName" runat="server"></asp:Label><span class="indent">(<asp:Label ID="LocationName" runat="server"></asp:Label>)</span>
                        </td>
                    </tr>
                    <tr>
                        <th>Role : </th>
                        <td>
                            <asp:Label ID="RoleName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <th>SAP Purchasing Group : </th>
                        <td>
                            <asp:TextBox ID="R3PurchasingGroup" runat="server" Width="7em" MaxLength="5"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Storage Locations : </th>
                        <td>
                            <asp:CheckBoxList ID="StorageLocationCheckBoxList" runat="server" RepeatLayout="UnorderedList" CssClass="storagelist"></asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <th>RFQ Correspondence Editable :</th>
                        <td><asp:CheckBox ID="RFQCorrespondenceEditable" runat="server" /></td>
                    </tr>
                    <tr>
                        <th>MMSTA Invalidation Editable :</th>
                        <td>
                            <asp:CheckBox ID="MMSTAInvalidationEditable" runat="server" />
                            <span>(The User need to close &amp; reopen browser to reflect change)</span>
                        </td>
                    </tr>
                    <tr>
                        <th>Default CC User 1 :</th>
                        <td><asp:DropDownList ID="DefaultCCUser1" runat="server"></asp:DropDownList></td>
                    </tr>
                    <tr>
                        <th>Default CC User 2 :</th>
                        <td><asp:DropDownList ID="DefaultCCUser2" runat="server"></asp:DropDownList></td>
                    </tr>

                </table>
                <asp:HiddenField ID="Mode" runat="server" Value="" />
                <asp:HiddenField ID="Action" runat="server" Value="Save" />
                <div class="btns">
                    <asp:Button ID="Save" runat="server" Text="Save" />
                </div>
            </div>
        </div>
    </form>
</body>
</html>

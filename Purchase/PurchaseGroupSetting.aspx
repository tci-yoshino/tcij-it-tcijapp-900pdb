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
                <table>
                    <tr>
                        <th>SAP Purchasing Group : </th>
                        <td>
                            <asp:TextBox ID="R3PurchasingGroup" runat="server" Width="7em" MaxLength="5"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <th>Location : </th>
                        <td>
                            <asp:TextBox ID="Location" runat="server" Width="7em" MaxLength="5"
                                ReadOnly="true" CssClass="readonly" TabIndex="1"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>Name : </th>
                        <td>
                            <asp:TextBox ID="Name" runat="server" Width="21em" MaxLength="255"
                                ReadOnly="True" CssClass="readonly" TabIndex="2"></asp:TextBox></td>
                    </tr>
                    <tr>
                        <th>StorageLocationIDs : </th>
                        <td>
                            <asp:Panel ID="StorageLocationIDsPanel" runat="server">
                                <asp:CheckBox ID="AL10" Text="AL10" runat="server" />
                                <asp:CheckBox ID="AL11" Text="AL11" runat="server" />
                                <asp:CheckBox ID="AL20" Text="AL20" runat="server" />
                                <asp:CheckBox ID="AL40" Text="AL40" runat="server" />
                                <asp:CheckBox ID="AL50" Text="AL50" runat="server" />
                                <asp:CheckBox ID="CL10" Text="CL10" runat="server" />
                                <%--<asp:CheckBox ID="CL20" Text="CL20" runat="server" />
                                <asp:CheckBox ID="CL30" Text="CL30" runat="server" />--%>
                                <asp:CheckBox ID="CL40" Text="CL40" runat="server" />
                                <asp:CheckBox ID="CL70" Text="CL70" runat="server" />
                                <asp:CheckBox ID="EL10" Text="EL10" runat="server" />
                                <asp:CheckBox ID="EL20" Text="EL20" runat="server" />
                                <asp:CheckBox ID="HL10" Text="HL10" runat="server" />
                                <asp:CheckBox ID="HL30" Text="HL30" runat="server" />
                                <asp:CheckBox ID="HL50" Text="HL50" runat="server" />
                                <asp:CheckBox ID="NL10" Text="NL10" runat="server" />
                                <asp:CheckBox ID="NL20" Text="NL20" runat="server" />

                            </asp:Panel>
                        </td>
                    </tr>

                    <tr>
                        <th>RFQ Correspondence Editable :</th>
                        <td>
                            <asp:CheckBox ID="RFQCorrespondenceEditable" runat="server" /></td>
                    </tr>

                    <tr>
                        <th>MMSTA Invalidation Editable :</th>
                        <td>
                            <asp:CheckBox ID="MMSTAInvalidationEditable" runat="server" />
                        </td>
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

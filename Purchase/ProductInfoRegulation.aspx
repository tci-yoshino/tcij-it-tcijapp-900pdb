<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="ProductInfoRegulation.aspx.vb" Inherits="Purchase.ProductInfoRegulation" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Purchase DB</title>
    <script type="text/javascript" src="./JS/Common.js"></script>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
</head>
<body>
    <!-- Header -->
    <commonUC:Header ID="HeaderMenu" runat="server" />
    <!-- Header End -->
    <form id="ProductInfoForm" runat="server">
        <div id="content">
            <h3>Product Info &amp; Regulation</h3>

            <div class="list">
                <p class="message"><asp:Label runat="server" ID="lblMsg"></asp:Label></p>

                <div class="btns" style="text-align:left">
                    <asp:Button ID="btnSaveTop" runat="server" Text="Save" OnClientClick="SetAction('Save')" OnClick="btnSave_Click" />
                </div>

                <asp:ListView ID="ltvEhsHeader" runat="server" OnItemDataBound="ltvEhsHeader_ItemDataBound">
                    <LayoutTemplate>
                        <table id="itemPlaceholderContainer" style="width:60%" runat="server">
                            <tr>
                                <th style="width:15%">Location</th>
                                <th style="width:10%">ON/OFF</th>
                                <th>Product Info &amp; Regulation</th>
                            </tr>
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                            <tr>
                                <td colspan="3" class="divider">
                                    <hr />
                                </td>
                            </tr>
                        </table>
                    </LayoutTemplate>
                    <ItemTemplate>
                        <tr runat="server" id="trRowSeparator">
                            <td colspan="3" class="divider">
                                <hr />
                            </td>
                        </tr>
                        <tr id="trItem" runat="server" class='<%# iif(Container.DataItemIndex / 2=0,"zebra","") %>'>

                            <td runat="server" id="tdBranch" class="nobgcolor">
                                <asp:Label ID="lblLocationName" runat="server" Text='<%#Eval("LocationName")%>' />
                                <asp:HiddenField runat="server" ID="hidLocationCode" Value='<%#Eval("LocationCode")%>' />
                            </td>
                            <td runat="server" id="tdOnOrOff">
                                <asp:CheckBox ID="chkOnOrOff" runat="server" />
                                <asp:HiddenField ID="hidOnOrOff" runat="server" />
                            </td>
                            <td class="textnowrap" runat="server" id="tdText">
                                <asp:Label ID="lblText" runat="server" Text='<%#Eval("Text")%>' />
                                <asp:HiddenField runat="server" ID="hidItem" Value='<%#Eval("Item")%>' />
                            </td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>

                <div class="btns" style="text-align:left">
                    <asp:Button ID="btnSaveBottom" runat="server" Text="Save" OnClientClick="SetAction('Save');" OnClick="btnSave_Click" />
                </div>

            </div>
        </div>
        <asp:HiddenField runat="server" ID="hidLastUpdateDate" Value='' />
        <asp:HiddenField ID="Action" runat="server" />
    </form>
</body>
</html>


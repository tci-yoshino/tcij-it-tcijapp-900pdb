<%@ Page Language="vb" AutoEventWireup="true" CodeBehind="ProductInfoRegulation.aspx.vb" Inherits="Purchase.ProductInfoRegulation" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <title>Inventory Management System</title>
    <script type="text/javascript" src="./JS/Common.js"></script>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
</head>
<body>
    <!-- Header -->
    <commonUC:Header ID="HeaderMenu" runat="server" />
    <!-- Header End -->
    <form id="ProductInfoForm" runat="server">
        <div id="content">
            <div class="main">
                <h3 id="H1" runat="server">Product Info & Regulation</h3>
                <p class="attention">
                    <asp:Label runat="server" ID="lblMsg"></asp:Label>
                </p> 
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSaveTop" runat="server" Text="Save" CssClass="savebutton"
                                    OnClientClick="SetAction('Save')" onclick="btnSave_Click" />
                        </td>
                    </tr>
                </table>
                <div id="Div1" class="list" runat="server">
                    <asp:ListView ID="ltvEhsHeader" runat="server" 
                        onitemdatabound="ltvEhsHeader_ItemDataBound">
                        <LayoutTemplate>
                            <table ID="itemPlaceholderContainer" runat="server">
                                <tr >
                                    <th style="Width:8%">Branch</th>
                                    <th style="Width:6%">ON/OFF</th>
                                    <th class="textnowrap" style="Width:20%">Product Info & Regulation</th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                                <tr >
                                    <td colspan="5" class="nobgcolor"><hr /></td>
                                </tr>    
                            </table>
                        </LayoutTemplate>
                        <ItemTemplate>
                            <tr runat="server" id="trRowSeparator">
                                <td colspan="5" class="nobgcolor">
                                    <hr />
                                </td>
                            </tr>
                            <tr id="trItem" runat="server" class='<%# iif(Container.DataItemIndex / 2=0,"zebra","") %>'>

                                <td runat="server" ID="tdBranch" class="nobgcolor">
                                    <asp:Label ID="lblLocationName" runat="server" Text='<%#Eval("LocationName")%>' />
                                    <asp:HiddenField runat="server" ID="hidLocationCode" Value='<%#Eval("LocationCode")%>' />
                                </td>
                                <td runat="server" ID="tdOnOrOff">
                                    <asp:CheckBox ID="chkOnOrOff" runat="server" />
                                    <asp:HiddenField ID="hidOnOrOff" runat="server" />
                                </td>
                                <td class="textnowrap" runat="server" ID="tdText">
                                    <asp:Label ID="lblText" runat="server" Text='<%#Eval("Text")%>' />
                                    <asp:HiddenField runat="server" ID="hidItem" Value='<%#Eval("Item")%>' />
                                </td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
                <table>
                    <tr>
                        <td>
                            <asp:Button ID="btnSaveBottom" runat="server" Text="Save" CssClass="savebutton" 
                                    OnClientClick="SetAction('Save');" onclick="btnSave_Click" />
                        </td>
                    </tr>
                </table>
            </div>
        </div>
        <asp:HiddenField runat="server" ID="hidLastUpdateDate" Value=''/>
        <asp:HiddenField ID="Action" runat="server" />
    </form>
</body>
</html>


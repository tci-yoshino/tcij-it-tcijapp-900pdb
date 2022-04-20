<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="MyTask.aspx.vb" Inherits="Purchase.MyTask" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
    <script type="text/javascript">
<% 
'<!--

'window.onload = Function() {
'    colorful.set();
'    navi('home');

'}
'-->
%>
    </script>
</head>
<body>
    <!-- Header -->
    <commonUC:Header ID="HeaderMenu" runat="server" />
    <!-- Header End -->

    <!-- Main Content Area -->
    <div id="content">
        <form id="SwitchForm" runat="server">
            <div class="main switch">
                <table style="margin-bottom: 0">
                    <tr>
                        <th>User : </th>
                        <td>
                            <asp:DropDownList ID="UserID" runat="server">
                            </asp:DropDownList>
                        </td>
                        <th>RFQ Priority : </th>
                        <td>
                            <asp:DropDownList ID="RFQPriority" runat="server">
                            </asp:DropDownList>
                        </td>
                        <th>RFQ Status : </th>
                        <td>
                            <asp:DropDownList ID="RFQStatus" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>Order by:
                            <asp:DropDownList ID="Orderby" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td>
                            <asp:Button ID="Switch" runat="server" Text="Switch" OnClick="Switch_Click" PostBackUrl="MyTask.aspx?Action=Switch" /></td>
                    </tr>
                </table>
            </div>

            <div class="main">
                <p class="attention">
                    <asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
            </div>

            <%  If (IsPostBack) And (String.IsNullOrEmpty(st_Action)) Then%>
            <%Else%>
            <h3>RFQ</h3>
            <div class="list">
                <%--ページング時の押下ボタンフラグ保持用にHiddenField作成--%>
                <asp:HiddenField ID="HiddenSelectedButton" runat="server" Value="" />
                <asp:ListView ID="RFQList" runat="server">
                    <LayoutTemplate>
                        <div class="pagingHead">
                            <asp:DataPager ID="RFQPagerCountTop" runat="server" PageSize="50">
                                <Fields>
                                    <asp:TemplatePagerField>
                                        <PagerTemplate>
                                            Page
                                        <asp:Label runat="server" ID="CurrentPageLabel"
                                            Text="<%# IIf(Container.TotalRowCount > 0, CInt(Container.StartRowIndex / Container.PageSize) + 1, 0) %>" />
                                            of
                                        <asp:Label runat="server" ID="TotalPagesLabel"
                                            Text="<%# Math.Ceiling(System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
                                            (<asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                            records)
                                        </PagerTemplate>
                                    </asp:TemplatePagerField>
                                </Fields>
                            </asp:DataPager>
                        </div>

                        <div class="paging">
                            <asp:DataPager ID="RFQPagerLinkTop" runat="server" PageSize="50">
                                <Fields>
                                    <asp:NumericPagerField ButtonCount="10" CurrentPageLabelCssClass="current" NumericButtonCssClass="numeric" PreviousPageText="&laquo; Previous" NextPageText="Next &raquo;" />
                                </Fields>
                            </asp:DataPager>
                        </div>

                        <div id="Div1" runat="server">
                            <div id="itemPlaceholder2" runat="server">
                            </div>
                        </div>

                        <table id="itemPlaceholderContainer" runat="server" border="0" style="">
                            <tr id="itemPlaceholder" runat="server">
                            </tr>
                        </table>
                        <div class="paging">
                            <asp:DataPager ID="RFQPagerLinkBottom" runat="server" PageSize="50">
                                <Fields>
                                    <asp:NumericPagerField ButtonCount="10" CurrentPageLabelCssClass="current" NumericButtonCssClass="numeric" PreviousPageText="&laquo; Previous" NextPageText="Next &raquo;" />
                                </Fields>
                            </asp:DataPager>
                        </div>

                        <div class="pagingHead">
                            <asp:DataPager ID="RFQPagerCountBottom" runat="server" PageSize="50">
                                <Fields>
                                    <asp:TemplatePagerField>
                                        <PagerTemplate>
                                            Page
                                        <asp:Label runat="server" ID="CurrentPageLabel"
                                            Text="<%# IIf(Container.TotalRowCount > 0, CInt(Container.StartRowIndex / Container.PageSize) + 1, 0) %>" />
                                            of
                                        <asp:Label runat="server" ID="TotalPagesLabel"
                                            Text="<%# Math.Ceiling(System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
                                            (<asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" />
                                            records)
                                        </PagerTemplate>
                                    </asp:TemplatePagerField>
                                </Fields>
                            </asp:DataPager>
                        </div>
                    </LayoutTemplate>
                    <EmptyDataTemplate>
                        <h3 style="font-style: italic"><%=Purchase.Common.MSG_NO_DATA_FOUND%></h3>
                    </EmptyDataTemplate>
                    <ItemTemplate>
                        <tr>
                            <th class="subhead" colspan="2">
                                <!-- １段下に改行されて表示される現象の対策のため「RFQ Reference Number」の前に記述する -->
                                <span class="placedright">
                                    <asp:Label ID="Priority_Title_RFQ" runat="server" Text="Priority : " Visible='<%#IIf(Eval("Priority") = "", False, True) %>' CssClass='<%#IIf(Eval("Priority") = "B", "priorityB", "priorityA") %>'></asp:Label><asp:Label ID="Priority_RFQ" runat="server" Text='<%# Eval("Priority") %>' CssClass='<%#IIf(Eval("Priority") = "B", "priorityB", "priorityA") %>'></asp:Label></span>
                                RFQ Reference Number :
                                <asp:HyperLink ID="RFQUpdate" runat="server" NavigateUrl='<%# "./RFQUpdate.aspx?RFQNumber=" & Eval("RFQNumber") %>'>
                                    <asp:Label ID="RFQNumber" runat="server" Text='<%# Eval("RFQNumber") %>'></asp:Label></asp:HyperLink>
                                <span class="indent"><em>
                                    <asp:Label ID="RFQCorrespondence" runat="server" Text='<%# Eval("RFQCorrespondence") %>'></asp:Label></em></span>
                            </th>
                            <th class="subhead" colspan="3">
                                <asp:Label ID="RFQCreateDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("CreateDate"), True, False)%>'></asp:Label>
                                <span class="indent">
                                    <asp:Label ID="Label1" runat="server" Text='Create'></asp:Label></span>
                                <span style="margin-left: 2.5em">
                                    <asp:Label ID="RFQStatusChangeDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("StatusChangeDate"), True, False)%>'></asp:Label></span>
                                <span class="indent">
                                    <asp:Label ID="RFQStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label></span>
                                <span class="indent">
                                    <asp:Label ID="RFQConfidential" runat="server" Text='<%#IIf(Eval("isCONFIDENTIAL") = True, Purchase.Common.CONFIDENTIAL, "") %>' CssClass="confidential"></asp:Label></span>
                            </th>
                            <th class="subhead" style="text-align: right">
                                <asp:Button ID="RFQCancelAssign" runat="server" Text="Cancel Assignment" Visible="False" />
                                <asp:HiddenField ID="StatusCode" runat="server" Value='<%# Eval("StatusCode") %>' />
                            </th>
                        </tr>
                        <tr>
                            <th style="width: 17%">Product Number / Name</th>
                            <td style="width: 33%">
                                <asp:HyperLink ID="ProductPage" runat="server" NavigateUrl='<%# "./RFQListByProduct.aspx?ProductID=" & Eval("ProductID") %>'>
                                    <asp:Label ID="ProductNumber" runat="server" Text='<%# Eval("ProductNumber") %>'></asp:Label></asp:HyperLink><span class="indent"><asp:Label ID="ProductName" runat="server" Text='<%#Purchase.Common.CutShort(Eval("ProductName").ToString())%>'></asp:Label></span></td>
                            <th style="width: 10%">Purpose</th>
                            <td style="width: 12%">
                                <asp:Label ID="Purpose" runat="server" Text='<%# Eval("Purpose") %>'></asp:Label></td>
                            <th style="width: 10%">Enq-User</th>
                            <td style="width: 18%">
                                <asp:Label ID="EnqUser" runat="server" Text='<%# Eval("EnqUserName") %>'></asp:Label><span class="indent">(<asp:Label ID="EnqLocation" runat="server" Text='<%# Eval("EnqLocationName") %>'></asp:Label>)</span></td>
                        </tr>
                        <tr>
                            <th>Supplier Code / Name</th>
                            <td>
                                <asp:HyperLink ID="SupplierPage" runat="server" NavigateUrl='<%# "./RFQListBySupplier.aspx?SupplierCode=" & Eval("SupplierCode") %>'>
                                    <asp:Label ID="SupplierCode" runat="server" Text='<%# Eval("SupplierCode") %>'></asp:Label></asp:HyperLink><span class="indent"><asp:Label ID="SupplierName" runat="server" Text='<%#Purchase.Common.CutShort(Eval("SupplierName").ToString())%>'></asp:Label></span></td>
                            <th>Maker Name</th>
                            <td colspan="3">
                                <asp:Label ID="MakerName" runat="server" Text='<%# Eval("MakerName") %>'></asp:Label></td>
                        </tr>
                    </ItemTemplate>
                </asp:ListView>
            </div>
            <hr />

            <%End If%>
        </form>
    </div>
    <!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcRFQ" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SrcPO_Overdue" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SrcPO_PPI" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
    <asp:SqlDataSource ID="SrcPO_Par" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>

    <!-- Footer -->
    <!--#include virtual="./Footer.html" -->
    <!-- Footer END -->
</body>
</html>

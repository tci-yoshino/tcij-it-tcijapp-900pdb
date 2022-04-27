<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="ProductListBySupplier.aspx.vb" Inherits="Purchase.ProductListBySupplier"  %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Purchase DB</title>
        <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />

        <script type="text/javascript" src="./JS/Common.js"></script>
        <script type="text/javascript" src="./JS/Colorful.js"></script>
        <script type="text/javascript" src="./JS/jquery-1.11.2.min.js"></script>
        <script type="text/javascript">

            function deleteLine(code) {
                setAction('Delete');
                if (confirm("It can't be restored once deleted.\nAre you sure to delete this entry?")) {
                    document.forms["PageForm"].ProductID.value = code;
                    document.forms["PageForm"].submit();
                    return true;
                }
            }

            // 画面表示時・ポストバック時の処理
            $(document).on('ready', function () {
                let selectedValue = $('#HiddenSelectedValidityFilter').val();
                $('#SelValidity').val(selectedValue);
            })

            function setFilterType() {
                $('#HiddenSelectedValidityFilter').val($('#SelValidity').val());
            }

        </script>
    </head>

    <body>
        <form id="PageForm" runat="server" method="post">
            <!-- Main Content Area -->
            <div id="content">
                <div class="tabs"><a href="<%=AddUrl %>">New Suppliers Product</a> | <a href="<%=ImpUrl %>">Excel Import</a></div>

                <h3>Suppliers Product</h3>

                <div class="main">
                    <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
                    <table>
                        <tr>
                            <th>Supplier Code : </th>
                            <td><asp:Label ID="SupplierCode" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <th>Supplier Name : </th>
                            <td><asp:Label ID="SupplierName" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <th>Terrirtory : </th>
                            <td><asp:Label ID="Territory" runat="server" Text=""></asp:Label></td>
                        </tr>
                    </table>
                </div>
                <hr />

                <div class="list">
                 
                    <asp:Button ID ="ExcelExportBtn" runat="server" Text="Download"/>
                    <asp:HiddenField ID="hidSourceID" runat="server" />
                    <input type="hidden" runat="server" id="ProductID" />
                    <input type="hidden" runat="server" id="Action" value="" />

<%--  ページング時の押下ボタンフラグ保持用にHiddenField作成  --%>
                    <asp:HiddenField ID="HiddenSelectedValidityFilter" runat="server" Value ="ALL"/>
                    <asp:HiddenField ID="HiddenSortType" runat="server" Value =""/>
                    <asp:HiddenField ID="HiddenSortField" runat="server" Value =""/>
                    <asp:ListView ID="SupplierProductList" runat="server" >
                        <LayoutTemplate>
                            <div class="pagingHead" >
                                <asp:DataPager ID="SupplierProductPagerCountTop" runat="server">
                                    <Fields>
                                        <asp:TemplatePagerField>
                                            <PagerTemplate>
                                                Page
                                                <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# IIf(Container.TotalRowCount > 0, CInt(Container.StartRowIndex / Container.PageSize) + 1, 0) %>" />
                                                of
                                                <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling(System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
                                                (
                                                <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" /> 
                                                records)
                                            </PagerTemplate>
                                        </asp:TemplatePagerField>
                                    </Fields>
                                </asp:DataPager>
                            </div>

                            <div class="paging">
                                <asp:DataPager ID="SupplierProductPagerLinkTop" runat="server">
                                    <Fields>
                                        <asp:NumericPagerField ButtonCount="10" CurrentPageLabelCssClass="current" NumericButtonCssClass="numeric" PreviousPageText="&laquo; Previous" NextPageText="Next &raquo;" />
                                    </Fields>
                                </asp:DataPager>
                            </div>
                        
                            <div ID="Div1" runat="server">
                                <div ID="itemPlaceholder2" runat="server">
                                </div>
                            </div>

                            <table class="table" ID="itemPlaceholderContainer" runat="server" border="0" style="">
                                <tr>
                                    <th id ="ProductNumHeader" style="width:15%" class="sortField" >Product Number</th>
                                    <th style="width:10%">CAS Number</th>
                                    <th style="width:20%">Product Name</th>
                                    <th style="width:10%">Supplier Item Number</th>
                                    <th style="width:10%">Note</th>
                                    <th id ="ValidQuotationHeader" class="sortField" style="width:15%">Valid Quotation</th>
                                    <th id ="UpdateDateHeader" class="sortField" style="width:10%">Update Date</th>
                                    <th style="width:5%">Edit</th>
                                    <th style="width:5%">Delete</th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>

                            <div class="paging">
                                <asp:DataPager ID="SupplierProductPagerLinkBottom" runat="server">
                                    <Fields>
                                        <asp:NumericPagerField ButtonCount="10" CurrentPageLabelCssClass="current" NumericButtonCssClass="numeric" PreviousPageText="&laquo; Previous" NextPageText="Next &raquo;" />
                                    </Fields>
                                </asp:DataPager>
                            </div>
                        
                            <div class="pagingHead">
                                <asp:DataPager ID="SupplierProductPagerCountBottom" runat="server">    
                                    <Fields>
                                        <asp:TemplatePagerField>              
                                            <PagerTemplate>
                                                Page
                                                <asp:Label runat="server" ID="CurrentPageLabel" Text="<%# IIf(Container.TotalRowCount > 0, CInt(Container.StartRowIndex / Container.PageSize) + 1, 0) %>" />
                                                of
                                                <asp:Label runat="server" ID="TotalPagesLabel" Text="<%# Math.Ceiling(System.Convert.ToDouble(Container.TotalRowCount) / Container.PageSize) %>" />
                                                (
                                                <asp:Label runat="server" ID="TotalItemsLabel" Text="<%# Container.TotalRowCount%>" /> 
                                                records)
                                            </PagerTemplate>
                                        </asp:TemplatePagerField>
                                    </Fields>
                                </asp:DataPager>
                            </div>
                        </LayoutTemplate>

                        <EmptyDataTemplate>
                            <h3 style="font-style:italic"><% =Purchase.Common.MSG_NO_DATA_FOUND%></h3>
                        </EmptyDataTemplate>

                        <ItemTemplate>
                            <tr>
                                <td><asp:Label ID="ProductNumber" runat="server" Text='<%# Eval("ProductNumber") %>' /></td>
                                <td><asp:Label ID="CASNumber" runat="server" Text='<%# Eval("CASNumber") %>' /></td>
                                <td><asp:Label ID="ProductName" runat="server" Text='<%# Eval("ProductName") %>' /></td>
                                <td><asp:Label ID="SupplierItemNumber" runat="server" Text='<%# Eval("SupplierItemNumber") %>' /></td>
                                <td><asp:Label ID="Note" runat="server" Text='<%# Eval("Note") %>' /></td>
                                <td><asp:Label ID="ValidQuotation" runat="server" Text='<%# Eval("ValidQuotation") %>' /></td>
                                <td><asp:Label ID="UpdateDate" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("UpdateDate"), True, false)%>' /></td>
                                <td><asp:HyperLink ID="Edit" runat="server" NavigateUrl='<%# Eval("Url") %>'>Edit</asp:HyperLink></td>
                                <td><asp:HyperLink ID="Delete" runat="server" NavigateUrl='<%# "javascript:deleteLine(" & Eval("ProductID") & ");" %>'>Delete</asp:HyperLink></td>
                            </tr>
                        </ItemTemplate>
                    </asp:ListView>
                </div>
            </div>
            <!-- Main Content Area END -->
            <!-- Footer -->
            <!-- Footer END -->
        </form>

        <script type="text/javascript">
            var hidden_sort_type = document.getElementById("<%= HiddenSortType.ClientID %>")
            var hidden_sort_field = document.getElementById("<%= HiddenSortField.ClientID %>")
            ListSort(hidden_sort_type, hidden_sort_field);
        </script>

    </body>
    
</html>

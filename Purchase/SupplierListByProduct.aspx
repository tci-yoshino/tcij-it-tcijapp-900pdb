<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SupplierListByProduct.aspx.vb" Inherits="Purchase.SupplierListByProduct" EnableEventValidation="false"　%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Purchase DB</title>
        <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
        <script type="text/javascript" src="./JS/Common.js"></script>
        <script type="text/javascript" src="./JS/Colorful.js"></script>
        <script type="text/javascript" src="./JS/jquery-1.11.2.min.js"></script>
        <script type="text/javascript">
        <!--
            // [Delete]リンクアクション時処理
            function deleteLine(code) {
                setAction('Delete');
                if (confirm("It can't be restored once deleted.\nAre you sure to delete this entry?")) {
                    document.forms["PageForm"].SupplierCode.value = code;
                    document.forms["PageForm"].submit();
                    return true;
                }
            }
            // 画面ボタンアクション時判定処理
            function setFormAction(button_type) {
                if (button_type == "Delete") {
                    setAction('Delete');
                } else if (button_type == "Search") {
                    setAction('Search');
                } else if (button_type == "Release") {
                    setAction('Release');
                    clearForm('SearchForm');
                } else {
                    document.getElementById("Action").value = "";
                }
            }
            $(function () {
                // リサイズしたときの処理
                $(window).on('resize', function () {
                    scrollset();
                });
                function scrollset() {
                    var winHeight = window.innerHeight ? window.innerHeight : $(window).height();
                    var height1 = $('div#FixedHeaderInfo').outerHeight(true);
                    var height2 = $('div#FixedHeaderInfo2').outerHeight(true);
                    var height3 = $('div#FixedHeaderInfo3').outerHeight(true);
                    var height4 = $('div#FixedHeaderInfo4').outerHeight(true);
                    // divのスクロールバーは出るがブラウザのスクロールバーは出ない値に設定
                    var divScrollHeight = winHeight - height1 - height2 - height3 - height4 - 80;
                    if (divScrollHeight <= 0) {
                        $('.list').css({ 'height': '' });
                        $('.list').css({ 'overflow-y': '' });
                    } else {
                        $('.list').css({ 'height': divScrollHeight });
                        $('.list').css({ 'overflow-y': 'scroll' });
                    }
                }
                // Territory リスト表示制御
                $('#Territory').click(function () {
                    var offsetTop = $(this).offset().top;
                    var offsetLeft = $(this).offset().left;
                    var height = $('#Territory').height();
                    $('#divTerritory').css('position', 'absolute');
                    $('#divTerritory').css('top', offsetTop + height);
                    $('#divTerritory').css('left', offsetLeft);
                    $('#divTerritory').fadeIn();
                });
                // Territory 選択確定時
                $('#btnTerritoryClose').click(function () {
                    $('#divTerritory').fadeOut();
                });
            });
            //-->
        </script>
    </head>

    <body>
        <!-- Header -->
        <commonUC:Header ID="HeaderMenu" runat="server" />
        <!-- Header End -->

        <form id="PageForm" runat="server"  method="post">
            <!-- Main Content Area -->
            <div id="content">
                <div class="tabs"><a href="<%=AddUrl %>">New Suppliers Product</a></div>

                <h3>Supplier List</h3>
                
                <div class="main">
                    <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>
                    <table>
                        <tr>
                            <th>Product Number : </th>
                            <td><asp:Label ID="ProductNumber" runat="server" Text=""></asp:Label></td>
                        </tr>
                        <tr>
                            <th>Product Name : </th>
                            <td><asp:Label ID="ProductName" runat="server" Text=""></asp:Label></td>
                        </tr>
                    </table>
                </div>
                <hr />

                <div class="cond-right">
                    <b>Territory : </b>
                    <span id="Territory">
                        <asp:DropDownList runat="server" ID="SelTerritory" CssClass="filterdata">
                            <asp:ListItem>(see the list.)</asp:ListItem>
                        </asp:DropDownList>&nbsp;
                    </span> 
                    <b>UpdateDate : From</b>
                    <span>
                        <asp:TextBox ID="UpdateDateFrom" runat="server" Text="" Width="9em" MaxLength="10"></asp:TextBox>
                    </span>
                    <b>To</b>
                    <span>
                        <asp:TextBox ID="UpdateDateTo" runat="server" Text="" Width="9em" MaxLength="10"></asp:TextBox>
                    </span>
                    <b>
                    <span class="indent">
                            <asp:Label ID="Label1" runat="server" Text="">(YYYY-MM-DD)</asp:Label>
                        </span>
                    </b>
                    <asp:Button ID="Search" runat="server" Text="Search"  OnClientClick ="setFormAction('Search');" />&nbsp;
                    <asp:Button ID="Release" runat="server" Text="Release" OnClientClick ="setFormAction('Release');" />
                </div>
                <div id="divTerritory" class="territory">
                    <table border="0" cellpadding="1" cellspacing="0">
                        <tr>
                            <td>
                                <asp:CheckBoxList runat ="server" ID="TerritoryList" DataTextField="ItemValue" DataValueField="ItemText">
                                    <asp:ListItem></asp:ListItem>
                                </asp:CheckBoxList>
                            </td>
                        </tr>
                        <tr><td><input type="button" id="btnTerritoryClose" value="OK" /></td></tr>
                    </table>
                </div>
                <div class="list">
                    <input type="hidden" runat="server" id="ProductID" />
                    <input type="hidden" runat="server" id="SupplierCode" />
                    <input type="hidden" runat="server" id="Action" value="" />

                    <asp:HiddenField ID="HiddenSortType" runat="server" Value =""/>
                    <asp:HiddenField ID="HiddenSortField" runat="server" Value =""/>
                    <asp:ListView ID="SupplierProductList" runat="server" >
                        <LayoutTemplate>
                            <table ID="itemPlaceholderContainer" class ="table" runat="server" border="0" style="">
                                <tr>
                                    <th id ="SupplierCodeHeader" class="sortField" style="width:15%">Supplier Code</th>
                                    <th style="width:15%">Supplier Name</th>
                                    <th id ="CountryHeader" class="sortField" style="width:5%">Country</th>
                                    <th style="width:10%">Territory</th>
                                    <th style="width:15%">Supplier Item Number</th>
                                    <th style="width:10%">Note</th>
                                    <th id ="UpdateDateHeader" class="sortField" style="width:10%">Update Date</th>
                                    <th id ="ValidQuotationHeader" class="sortField" style="width:15%">Valid Quotation</th>
                                    <th style="width:5%">Edit</th>
                                    <th style="width:5%">Delete</th>
                                </tr>
                                <tr ID="itemPlaceholder" runat="server">
                                </tr>
                            </table>
                        </LayoutTemplate>

                        <EmptyDataTemplate>
                            <h3 style="font-style:italic"><% =Purchase.Common.MSG_NO_DATA_FOUND%></h3>
                        </EmptyDataTemplate>

                        <ItemTemplate>
                            <tr>
                                <td><asp:HyperLink ID="SupplierCode" runat="server" NavigateUrl='<%#Eval("SupplierCode","./SupplierSetting.aspx?Action=Edit&Code={0}")%>' Text = '<%#Eval("SupplierCode")%>' /></td>
                                <td><asp:HyperLink ID="SupplierName" runat="server" NavigateUrl='<%#Eval("SupplierCode", "./SupplierSetting.aspx?Action=Edit&Code={0}")%>' Text = '<%#Eval("SupplierName")%>' /></td>
                                <td><asp:Label ID="Country" runat="server" Text='<%# Eval("Country") %>' /></td>
                                <td><asp:Label ID="Territory" runat="server" Text='<%# Eval("Territory") %>' /></td>
                                <td><asp:Label ID="SupplierItemNumber" runat="server" Text='<%# Eval("SupplierItemNumber") %>' /></td>
                                <td><asp:Label ID="Note" runat="server" Text='<%# Eval("Note") %>' /></td>
                                <td><asp:Label ID="UpdateDate" runat="server" Text='<%# Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("UpdateDate"), True, False)%>' /></td>
                                <td><asp:Label ID="ValidQuotation" runat="server" Text='<%# Eval("ValidQuotation") %>' /></td>
                                <td><asp:HyperLink ID="Edit" runat="server" NavigateUrl='<%# Eval("Url") %>'>Edit</asp:HyperLink></td>
                                <td><asp:HyperLink ID="Delete" runat="server" NavigateUrl='<%# "javascript:deleteLine(" & Eval("SupplierCode") & ");" %>'>Delete</asp:HyperLink></td>
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

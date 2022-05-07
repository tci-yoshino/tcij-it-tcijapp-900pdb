﻿<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQSearch.aspx.vb" Inherits="Purchase.RFQSearch" EnableEventValidation="false"%>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
    <head runat="server">
        <title>Purchase DB</title>
        <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
        <script type="text/javascript" src="./JS/Common.js"></script>
        <script type="text/javascript" src="./JS/Colorful.js"></script>
        <script type="text/javascript" src="./JS/jquery-1.11.2.min.js"></script>
        <script type="text/javascript" language="javascript">
            <!--
            window.onload = function () {
                colorful.set();
                document.SearchForm.RFQNumber.focus();

                //Advanced Search⇔Basic Search切り替え処理
                var ModeChange = document.getElementById("ModeChange_link");
                ModeChange.onclick = function () {
                    if (SearchMode.value === "Basic") {
                        SearchMode.value = "Advanced"
                    } else {
                        SearchMode.value = "Basic"
                    }
                    modeChange();
                }
                modeChange();
                changeRowColor('<%=SearchResultList.ListClientID%>');
            }
            function modeChange() {
                let SearchMode = document.getElementById('SearchMode');

                if (SearchMode.value === "Basic") {
                    document.getElementById('ModeChange_link').innerText = "* Basic Search";
                    document.getElementById('Title').innerHTML = "<h3>* Advanced Search</h3>";
                    document.getElementById("ProductName_line").style.visibility = "visible";
                    document.getElementById("SAPSupplier_line").style.visibility = "visible";
                    document.getElementById("SupplierCountry_line").style.visibility = "visible";
                    document.getElementById("SupplierItemName_line").style.visibility = "visible";
                    document.getElementById("RFQCreatedDate_line").style.visibility = "visible";
                    document.getElementById("RFQQuotedDate_line").style.visibility = "visible";
                    document.getElementById("RFQPriority_line").style.visibility = "visible";
                    document.getElementById("ValidityQuotation_line").style.visibility = "visible";

                } else {
                    document.getElementById('ModeChange_link').innerText = "* Advanced Search";
                    document.getElementById('Title').innerHTML = "<h3>Basic Search</h3>";
                    document.getElementById("ProductName_line").style.visibility = "collapse";
                    document.getElementById("SAPSupplier_line").style.visibility = "collapse";
                    document.getElementById("SupplierCountry_line").style.visibility = "collapse";
                    document.getElementById("SupplierItemName_line").style.visibility = "collapse";
                    document.getElementById("RFQCreatedDate_line").style.visibility = "collapse";
                    document.getElementById("RFQQuotedDate_line").style.visibility = "collapse";
                    document.getElementById("RFQPriority_line").style.visibility = "collapse";
                    document.getElementById("ValidityQuotation_line").style.visibility = "collapse";
                    //Advanced Search画面で表示したテキストボックス・プルダウンリスト・チェックボックスの初期化
                    document.getElementById("ProductName").value = "";
                    document.getElementById("S4SupplierCode").value = "";
                    document.getElementById("SupplierCountryCode").value = "";
                    var SupplierCountryCodeSelect = document.getElementById("SupplierCountryCode");
                    SupplierCountryCodeSelect.selectedIndex = -1;
                    document.getElementById("SupplierItemName").value = "";
                    document.getElementById("RFQCreatedDateFrom").value = "";
                    document.getElementById("RFQCreatedDateTo").value = "";
                    document.getElementById("RFQQuotedDateFrom").value = "";
                    document.getElementById("RFQQuotedDateTo").value = "";
                    var PrioritySelect = document.getElementById("Priority");
                    PrioritySelect.selectedIndex = -1;
                    var PrioritySelect = document.getElementById("ValidityQuotation");
                    ValidityQuotation.selectedIndex = -1;
                }
            }
            function disableSubmit(form) {
                var elements = form.elements;
                for (var i = 0; i < elements.length; i++) {
                    if (elements[i].type == 'submit') {
                        elements[i].disabled = true;
                    }
                }
            }
            function RFQReferenceNumberBtn_onclick(Postback) {
                opneMultipleListWindow("RFQNumber", "RFQ Reference Number", document.SearchForm.RFQNumber.value)

                return false;
            }
            function ProductNumberBtn_onclick(Postback) {
                opneMultipleListWindow("ProductNumber", "Product Number", document.SearchForm.ProductNumber.value)

                return false;
            }
            function SupplierCodeBtn_onclick(Postback) {
                opneMultipleListWindow("SupplierCode", "Supplier Code", document.SearchForm.SupplierCode.value)

                return false;
            }
            function S4SupplierCodeBtn_onclick(Postback) {
                opneMultipleListWindow("S4SupplierCode", "SAP Supplier Code", document.SearchForm.S4SupplierCode.value)

                return false;
            }
            function opneMultipleListWindow(SearchItemId, ScreenName, SearchWord) {
                setAction('')

                document.SearchForm.SearchItemId.value = SearchItemId
                document.SearchForm.ScreenName.value = ScreenName
                document.SearchForm.SearchWord.value = SearchWord
                
                let option = "width=" + 600 + ",height=" + 500;
                window.open('', 'MultipleList', option + ",left=100,top=100,scrollbars=no,menubar=no,toolbar=yes,location=yes,statusbar=no,resizable=yes,directories=no");

                // フォーム情報をMultipleListへポストするため一時的にフォーム情報を書き換える
                let originalAction = document.SearchForm.action;
                let originalTarget = document.SearchForm.target
                document.SearchForm.action = 'MultipleList.aspx';
                document.SearchForm.target = 'MultipleList';
                document.SearchForm.submit();
                document.SearchForm.action = originalAction;
                document.SearchForm.target = originalTarget;
            }

            $(function() {
                // Purpose リスト表示制御
                $('#PurposeDropDown').click(function () {
                    var offsetTop = $(this).offset().top;
                    var offsetLeft = $(this).offset().left;
                    var height = $('#PurposeDropDown').height();
                    $('#divPurpose').css('position', 'absolute');
                    $('#divPurpose').css('top', offsetTop + height);
                    $('#divPurpose').css('left', offsetLeft);
                    $('#divPurpose').fadeIn();
                });

                // Purpose 選択確定時
                $('#btnPurposeClose').click(function () {
                    $('#divPurpose').fadeOut();
                });

                // Purpose 選択値の相関制御
                $('#PurposeList_0').click(function () {
                    if ($('#PurposeList_0').is(':checked') == true) {
                        $("[id^='PurposeList']").removeProp('checked');
                        $("[id^='PurposeList']").prop('disabled', 'disabled');
                        $('#PurposeList_0').removeProp('disabled');
                        $("#PurposeList_0").prop('checked', 'checked');
                    } else {
                        $("[id^='PurposeList']").removeProp('disabled');
                    }
                });

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
        <div id="content">
            <div class="tabs" style="text-align: right">
                <a href="#" id="ModeChange_link" runat="server"></a>
            </div>
            
            <div id ="Title">
                <h3></h3> 
            </div>
            
            <div class="main">
                <p class="attention">
                    <asp:Label ID="Msg" runat="server"></asp:Label>
                </p>
                <form id="SearchForm" runat="server">
                    <input type="hidden" id ="Action" runat="server" value="" />
                    <input type="hidden" id ="ScreenName" runat="server" value="" />
                    <input type="hidden" id ="SearchWord" runat="server" value="" />
                    <input type="hidden" id ="SearchItemId" runat="server" value="" />
                    <input type="hidden" id ="SearchMode" runat="server" value="" />
                    <table style= " margin-bottom: 0px;">
                        <tr>
                            <th>RFQ Reference Number : </th>
                            <td>
                                <%-- (RFQNumberの桁数 10 + パイプライン区切り 1) * RFQNumberの入力可能数 100 = 1100 --%>
                                <asp:TextBox ID="RFQNumber" runat="server" Width="10em" MaxLength="1100"></asp:TextBox>
                                <asp:Button ID="RFQReferenceNumberBtn" runat="server" Text="Multiple List" OnClientClick="return RFQReferenceNumberBtn_onclick()"/>
                            </td>
                        </tr>
                        <tr>
                            <th>Product Number : </th>
                            <td>
                                <%-- (ProductNumberの桁数 32 + パイプライン区切り 1) * ProductNumberの入力可能数 100 = 3300 --%>
                                <asp:TextBox ID="ProductNumber" runat="server" Width="10em" MaxLength="3300"></asp:TextBox>
                                <asp:Button ID="ProductNumberBtn" runat="server" Text="Multiple List" OnClientClick="return ProductNumberBtn_onclick()"/>
                            </td>
                        </tr>
                        <tr id ="ProductName_line" style="visibility: collapse;">
                            <th>Product Name: </th>
                            <td>
                                <asp:TextBox ID="ProductName" runat="server" Width="10em" MaxLength="255"></asp:TextBox>
                                <span id ="ProductNameNotes">(Partial text match)</span>
                            </td>
                        </tr>
                        <tr>
                            <th>Supplier Code : </th>
                            <td>
                                <%-- (SupplierCodeの桁数 10 + パイプライン区切り 1) * SupplierCodeの入力可能数 100 = 1100 --%>
                                <asp:TextBox ID="SupplierCode" runat="server" Width="10em" MaxLength="1100"></asp:TextBox>
                                <asp:Button ID="SupplierCodeBtn" runat="server" Text="Multiple List" OnClientClick="return SupplierCodeBtn_onclick()"/>
                            </td>
                        </tr>
                        <tr id ="SAPSupplier_line" style="visibility: collapse;">
                            <th>SAP Supplier Code : </th>
                            <td>
                                <%-- (S4SupplierCodeの桁数 10 + パイプライン区切り 1) * S4SupplierCodeの入力可能数 100 = 1100 --%>
                                <asp:TextBox ID="S4SupplierCode" runat="server" Width="10em" MaxLength="1100"></asp:TextBox>
                                <asp:Button ID="S4SupplierCodeBtn" runat="server" Text="Multiple List" OnClientClick="return S4SupplierCodeBtn_onclick()"/>
                            </td>
                        </tr>
                        <tr>
                            <th>Supplier Name : </th>
                            <td>
                                <asp:TextBox ID="SupplierName" runat="server" Width="10em" MaxLength="255"></asp:TextBox>
                                <span id ="SupplierNameNotes">(Partial text match)</span>
                            </td>
                        </tr>
                        <tr id ="SupplierCountry_line" style="visibility: collapse;">
                            <th>Supplier Country : </th>
                            <td>
                                <asp:DropDownList ID="SupplierCountryCode" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                
                            </td>
                        </tr>
                        <tr id ="SupplierItemName_line" style="visibility: collapse;">
                            <th>Supplier Item Name : </th>
                            <td>
                                <asp:TextBox ID="SupplierItemName" runat="server" Width="10em" MaxLength="255"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <th>RFQ Current Status : </th>
                            <td>From
                                <asp:DropDownList ID="StatusFrom" runat="server">
                                </asp:DropDownList>
                                To
                                <asp:DropDownList ID="StatusTo" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="RFQCreatedDate_line" style="visibility: collapse;">
                            <th>RFQ Created Date : </th>
                            <td>From
                                <asp:TextBox ID="RFQCreatedDateFrom" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                                To
                                <asp:TextBox ID="RFQCreatedDateTo" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                                <span class="format">(YYYY-MM-DD)</span>
                            </td>
                        </tr>
                        <tr id ="RFQQuotedDate_line" style="visibility: collapse;">
                            <th>RFQ Quoted Date : </th>
                            <td>From
                                <asp:TextBox ID="RFQQuotedDateFrom" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                                To
                                <asp:TextBox ID="RFQQuotedDateTo" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                                <span class="format">(YYYY-MM-DD)</span>
                            </td>
                        </tr>
                        <tr>
                            <th>Last RFQ Status Change Date : </th>
                            <td>From
                                <asp:TextBox ID="LastRFQStatusChangeDateFrom" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                                To
                                <asp:TextBox ID="LastRFQStatusChangeDateTo" runat="server" Width="10em" MaxLength="10"></asp:TextBox>
                                <span class="format">(YYYY-MM-DD)</span>
                            </td>
                        </tr>
                        <tr>
                            <th>Enq-Location / User / Storage : </th>
                            <td>
                                <asp:DropDownList ID="EnqLocationCode" runat="server" AutoPostBack = "true">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="EnqUserID" runat="server" AutoPostBack = "true">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="EnqStorageLocation" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>Quo-Location / User / Storage : </th>
                            <td>
                                <asp:DropDownList ID="QuoLocationCode" runat="server" AutoPostBack = "true">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="QuoUserID" runat="server" AutoPostBack = "true">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                                <asp:DropDownList ID="QuoStorageLocation" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id ="RFQPriority_line" style="visibility: collapse;">
                            <th>RFQ Priority : </th>
                            <td>
                                <asp:DropDownList ID="Priority" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>Purpose : </th>
                            <td id="PurposeDropDown">
                                <asp:DropDownList runat="server" ID="Purpose" CssClass="filterdata" Height="16px" Width="94px">
                                    <asp:ListItem>(see the list.)</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <th>Territory : </th>
                            <td id="Territory">
                                <asp:DropDownList runat="server" ID="SelTerritory" CssClass="filterdata" Height="16px" Width="94px">
                                    <asp:ListItem>(see the list.)</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id ="ValidityQuotation_line" style="visibility: collapse;">
                            <th>Validity Quotation : </th>
                            <td>
                                <asp:DropDownList ID="ValidityQuotation" runat="server">
                                    <asp:ListItem></asp:ListItem>
                                    <asp:ListItem>Valid Price</asp:ListItem>
                                    <asp:ListItem>Inalid Price</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                    </table>

                    <div class="btns" style="text-align: left">
                        <asp:Button ID="Search" runat="server" Text="Search" OnClientClick="setAction('Search');"/>&nbsp;
                        <asp:Button ID="Clear" runat="server" Text="Clear" OnClientClick ="clearForm('SearchForm');" />&nbsp;
                        <asp:Button ID="Download" runat="server" Text="Download" OnClientClick="setAction('Download');"/>&nbsp;
                    </div>

                    <div id="divPurpose" class="purpose">
                        <table border="0" cellpadding="1" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:CheckBoxList runat ="server" ID="PurposeList" DataTextField="Text" DataValueField="Text" AppendDataBoundItems="True">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td><input type="button" id="btnPurposeClose" value="OK" /></td>
                            </tr>
                        </table>
                    </div>
                    <div id="divTerritory" class="territory">
                        <table border="0" cellpadding="1" cellspacing="0">
                            <tr>
                                <td>
                                    <asp:CheckBoxList runat ="server" ID="TerritoryList" DataTextField="Name" DataValueField="Name" AppendDataBoundItems="True">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td><input type="button" id="btnTerritoryClose" value="OK" /></td>
                            </tr>
                        </table>
                    </div>

                            <asp:Panel ID="ResultArea" runat="server" Visible="false">
                                <commonUC:SearchResult ID="SearchResultList" runat="server" CssClass="search" EnableSelectRow="true" />
                            </asp:Panel>
                    <asp:SqlDataSource ID="SrcRFQHeader" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
                </form>
            </div>

        </div>

        <!-- Footer -->
        <!--#include virtual="./Footer.html" -->
        <!-- Footer END -->
    </body>

</html>

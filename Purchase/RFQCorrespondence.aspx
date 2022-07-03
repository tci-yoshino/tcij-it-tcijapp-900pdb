<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="RFQCorrespondence.aspx.vb" Inherits="Purchase.RFQCorrespondence" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>
<form id="RFQCorresForm" runat="server">
    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"></div>

        <h3>RFQ Correspondence</h3>
        
        <h4><asp:Label ID="ProductNumber" runat="server" /><span class="indent"><asp:Label ID="ProductName" runat="server" /></span></h4>

        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>

                <table>
                    <tr>
                        
                        <th>Addressee <span class="required">*</span> : </th>
                        <td>
                            <asp:RadioButton ID="EnqUser" GroupName="Addressee" runat="server" AutoPostBack="true" OnCheckedChanged="Addressee_CheckedChanged" /><span class="indent"><asp:Label ID="EnqLocation" runat="server" Text=""></asp:Label></span>
                            <br />
                            <asp:RadioButton ID="QuoUser" GroupName="Addressee" runat="server" AutoPostBack="true" OnCheckedChanged="Addressee_CheckedChanged" /><span class="indent"><asp:Label ID="QuoLocation" runat="server" Text=""></asp:Label></span>
                        </td>
                    </tr>
                    <tr>
                        <th>CC 1 : </th>
                        <td>
                            <asp:DropDownList ID="CCUser1" runat="server"></asp:DropDownList>
                            <asp:DropDownList ID="CCLocation1" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CCLocation1_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>CC 2 : </th>
                        <td>
                            <asp:DropDownList ID="CCUser2" runat="server"></asp:DropDownList>
                            <asp:DropDownList ID="CCLocation2" runat="server" AutoPostBack="true" OnSelectedIndexChanged="CCLocation2_SelectedIndexChanged"></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Title <span class="required">*</span> : </th>
                        <td>
                            <asp:DropDownList ID="CorresTitle" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <th>Note  <span class="required">*</span> : </th>
                        <td><asp:TextBox ID="CorresNote" runat="server" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                </table>
   
                <div class="btns">
                    <asp:Button ID="Send" runat="server" Text="Send" 
                        PostBackUrl="RFQCorrespondence.aspx?Action=Send" />
                </div>
            <asp:HiddenField ID="EnqUserID" runat="server" />
            <asp:HiddenField ID="EnqLocationCode" runat="server" />
            <asp:HiddenField ID="QuoUserID" runat="server" />
            <asp:HiddenField ID="QuoLocationCode" runat="server" />
        </div>

        <hr />

        <h3>Correspondence History</h3>

        <div class="main">
            <asp:ListView ID="RFQHistory" runat="server" OnItemDataBound="RFQHistory_ItemDataBound" OnItemCommand="RFQHistory_ItemCommand">
                <AlternatingItemTemplate>
                    <table class="zebra2">
                        <tr>
                            <th style="width:20%">Status : </th>
                            <td style="width:60%"><asp:Label ID="StatusLabel" runat="server" Font-Bold="True" Text='<%# Eval("RFQStatus") %>' /></td>
                            <td style="width:20%">
                                <asp:LinkButton ID="Check" runat="server" PostBackUrl="RFQCorrespondence.aspx?Action=Check">
                                <asp:Image ID="ImgCheck" runat="server" ImageUrl="./Image/Check.gif" /> Mark as Read</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <th>Date : </th>
                            <td colspan="2"><asp:Label ID="DateLabel" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("CreateDate"), True, True)%>' /></td>
                        </tr>
                        <tr>
                            <th>Sender : </th>
                            <td colspan="2">
                                <asp:Label ID="SenderLabel" runat="server" Text='<%# Eval("SendUserName") %>' />
                                <span class="indent"></span>
                                <asp:Label ID="SenderLocationLabel" runat="server" Text='<%# Eval("SendLocationName") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>Addressee : </th>
                            <td colspan="2">
                                <asp:Label ID="AddresseeLabel" runat="server" Text='<%# Eval("AddrUserName") %>' />
                                <span class="indent"></span>
                                <asp:Label ID="AddresseeLocationLabel" runat="server" Text='<%# Eval("AddrLocationName") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>CC 1 : </th>
                            <td colspan="2">
                                <asp:Label ID="CCUserName1Label" runat="server" Text='<%# Eval("CCUserName1") %>' />
                                <span class="indent"></span>
                                <asp:Label ID="CCLocationName1Label" runat="server" Text='<%# Eval("CCLocationName1") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>CC 2 : </th>
                            <td colspan="2">
                                <asp:Label ID="CCUserName2Label" runat="server" Text='<%# Eval("CCUserName2") %>' />
                                <span class="indent"></span>
                                <asp:Label ID="CCLocationName2Label" runat="server" Text='<%# Eval("CCLocationName2") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>Notes : </th>
                            <td colspan="2">
                                <asp:Label ID="TitleLabel" runat="server" CssClass="attention" Text='<%# Eval("RFQCorres") %>' />
                                <%#If(String.IsNullOrEmpty(Eval("RFQCorres")), "", "<br />")%>
                                <asp:Label ID="NoteLabel" runat="server" Text='<%# Eval("Note") %>' />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="isChecked" runat="server" Value='<%# Eval("isChecked") %>' />
                    <asp:HiddenField ID="RcptUserID" runat="server" Value='<%# Eval("RcptUserID") %>' />
                    <asp:HiddenField ID="RFQHistoryNumber" runat="server" Value='<%# Eval("RFQHistoryNumber") %>' />
                </AlternatingItemTemplate>
                <LayoutTemplate>
                  <div ID="itemPlaceholderContainer" runat="server">
                        <div ID="itemPlaceholder" runat="server">
                        </div>
                    </div>
                </LayoutTemplate>
                <EmptyDataTemplate>
                    <h3 style="font-style:italic">No data found.</h3>
                </EmptyDataTemplate>
                <ItemTemplate>
                    <table class="zebra1">
                        <tr>
                            <th style="width:20%">Status : </th>
                            <td style="width:60%"><asp:Label ID="StatusLabel" runat="server" Font-Bold="True" Text='<%# Eval("RFQStatus") %>' /></td>
                            <td style="width:20%">
                                <asp:LinkButton ID="Check" runat="server" PostBackUrl="RFQCorrespondence.aspx?Action=Check">
                                <asp:Image ID="ImgCheck" runat="server" ImageUrl="./Image/Check.gif" /> Mark as Read</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <th>Date : </th>
                            <td colspan="2"><asp:Label ID="DateLabel" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("CreateDate"), True, True)%>' /></td>
                        </tr>
                        <tr>
                            <th>Sender : </th>
                            <td colspan="2">
                                <asp:Label ID="SenderLabel" runat="server" Text='<%# Eval("SendUserName") %>' />
                                <span class="indent"></span>
                                <asp:Label ID="SenderLocationLabel" runat="server" Text='<%# Eval("SendLocationName") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>Addressee : </th>
                            <td colspan="2">
                                <asp:Label ID="AddresseeLabel" runat="server" Text='<%# Eval("AddrUserName") %>' />
                                <span class="indent"></span>
                                <asp:Label ID="AddresseeLocationLabel" runat="server" Text='<%# Eval("AddrLocationName") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>CC 1 : </th>
                            <td colspan="2">
                                <asp:Label ID="CCUserName1Label" runat="server" Text='<%# Eval("CCUserName1") %>' />
                                <span class="indent"></span>
                                <asp:Label ID="CCLocationName1Label" runat="server" Text='<%# Eval("CCLocationName1") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>CC 2 : </th>
                            <td colspan="2">
                                <asp:Label ID="CCUserName2Label" runat="server" Text='<%# Eval("CCUserName2") %>' />
                                <span class="indent"></span>
                                <asp:Label ID="CCLocationName2Label" runat="server" Text='<%# Eval("CCLocationName2") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>Notes : </th>
                            <td colspan="2">
                                <asp:Label ID="TitleLabel" runat="server" CssClass="attention" Text='<%# Eval("RFQCorres") %>' />
                                <%#If(String.IsNullOrEmpty(Eval("RFQCorres")), "", "<br />")%>
                                <asp:Label ID="NoteLabel" runat="server" Text='<%# Eval("Note") %>' />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="isChecked" runat="server" Value='<%# Eval("isChecked") %>' />
                    <asp:HiddenField ID="RcptUserID" runat="server" Value='<%# Eval("RcptUserID") %>' />
                    <asp:HiddenField ID="RFQHistoryNumber" runat="server" Value='<%# Eval("RFQHistoryNumber") %>' />
                </ItemTemplate>
            </asp:ListView>
            <asp:HiddenField ID="RFQNumber" runat="server" />
        </div>
    </div><!-- Main Content Area END -->
</form>
</body>
</html>

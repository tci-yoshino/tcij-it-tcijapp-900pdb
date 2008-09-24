<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="POCorrespondence.aspx.vb" Inherits="Purchase.POCorrespondence" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Purchase DB</title>
    <link rel="stylesheet" href="./CSS/Style.css" type="text/css" media="screen,print" />
    <script type="text/javascript" src="./JS/Common.js"></script>
    <script type="text/javascript" src="./JS/Colorful.js"></script>
</head>
<body>

            <form id="POCorresForm" runat="server">
    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"></div>

        <h3>PO Correspondence</h3>

        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>

                <table>
                    <tr>
                        <th>Addressee <span class="required">*</span> : </th>
                        <td>
                            <asp:RadioButton ID="POUser" GroupName="Addressee" runat="server" /><span class="indent">
                            <asp:Label ID="POLocation" runat="server" Text=""></asp:Label></span>
                            <br />
                            <asp:RadioButton ID="SOUser" GroupName="Addressee" runat="server" /><span class="indent">
                            <asp:Label ID="SOLocation" runat="server" Text=""></asp:Label></span>
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
                        <th>Note <span class="required">*</span> : </th>
                        <td><asp:TextBox ID="CorresNote" runat="server" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                </table>
   
                <div class="btns">
                    <asp:Button ID="Send" runat="server" Text="Send"
                        PostBackUrl="POCorrespondence.aspx?Action=Send" />
                </div>
        </div>

        <hr />

        <h3>Correspondence History</h3>

        <div class="main">
            <asp:ListView ID="POHistory" runat="server" DataSourceID="SrcPOHistory">
                <AlternatingItemTemplate>
                    <table class="zebra2">
                        <tr>
                            <th style="width:20%">Status:</th>
                            <td style="width:65%"><asp:Label ID="StatusLabel" runat="server" Font-Bold="True" Text='<%# Eval("Status") %>' /></td>
                            <td style="width:15%" rowspan="4">
                                <asp:LinkButton ID="Check" runat="server" PostBackUrl="POCorrespondence.aspx?Action=Check">
                                  <asp:Image ID="ImgCheck" runat="server" ImageUrl="./Image/Check.gif" />Check
                                </asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <th>Date:</th>
                            <td><asp:Label ID="DateLabel" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("Date"),true)%>' /></td>
                        </tr>
                        <tr>
                            <th>Sender:</th>
                            <td>         
                                <asp:Label ID="SenderLabel" runat="server" Text='<%# Eval("Sender") %>' />
                                <span class="indent"></span>
                                <asp:Label ID="SenderLocationLabel" runat="server" Text='<%# Eval("SenderLocation") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>Addressee:</th>
                             <td>
                                <asp:Label ID="AddresseeLabel" runat="server" Text='<%# Eval("Addressee") %>' />
                                <span class="indent"></span>
                                <asp:Label ID="AddresseeLocationLabel" runat="server" Text='<%# Eval("AddresseeLocation") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>Notes:</th>
                            <td>
                                <asp:Label ID="TitleLabel" runat="server" CssClass="attention" Text='<%# Eval("Title") %>' />
                                <%#If(IsDBNull(Eval("Title")), "", "<br />")%>
                                <asp:Label ID="Label1" runat="server" Text='<%# Eval("Notes") %>' />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="ischecked" runat="server" Value='<%# Eval("isChecked") %>' />
                    <asp:HiddenField ID="RcptUserID" runat="server" Value='<%# Eval("RcptUserID") %>' />
                    <asp:HiddenField ID="POHistoryNumber" runat="server" Value='<%# Eval("POHistoryNumber") %>' />
                </AlternatingItemTemplate>
                <LayoutTemplate>
                  <div ID="itemPlaceholderContainer" runat="server" style="">
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
                            <th style="width:20%">Status:</th>
                            <td style="width:65%"><asp:Label ID="StatusLabel" runat="server" Font-Bold="True" Text='<%# Eval("Status") %>' /></td>
                            <td style="width:15%" rowspan="4">
                                <asp:LinkButton ID="Check" runat="server" PostBackUrl="POCorrespondence.aspx?Action=Check">
                                <asp:Image ID="ImgCheck" runat="server" ImageUrl="./Image/Check.gif" />Check</asp:LinkButton>
                            </td>
                        </tr>
                        <tr>
                            <th>Date:</th>
                            <td><asp:Label ID="DateLabel" runat="server" Text='<%#Purchase.Common.GetLocalTime(Session("LocationCode"), Eval("Date"),true)%>' /></td>
                        </tr>
                        <tr>
                            <th>Sender:</th>
                            <td>  
                                <asp:Label ID="SenderLabel" runat="server" Text='<%# Eval("Sender") %>' />
                                <span class="indent"></span>
                                <asp:Label ID="SenderLocationLabel" runat="server" Text='<%# Eval("SenderLocation") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>Addressee:</th>
                            <td>
                                <asp:Label ID="AddresseeLabel" runat="server" Text='<%# Eval("Addressee") %>' />
                                <span class="indent"></span>
                                <asp:Label ID="AddresseeLocationLabel" runat="server" Text='<%# Eval("AddresseeLocation") %>' />
                            </td>
                        </tr>
                        <tr>
                            <th>Notes:</th>
                            <td>
                                <asp:Label ID="Label1" runat="server" CssClass="attention" Text='<%# Eval("Title") %>' />
                                <%#If(IsDBNull(Eval("Title")), "", "<br />")%>
                                <asp:Label ID="Label2" runat="server" Text='<%# Eval("Notes") %>' />
                            </td>
                        </tr>
                    </table>
                    <asp:HiddenField ID="ischecked" runat="server" Value='<%# Eval("isChecked") %>' />
                    <asp:HiddenField ID="RcptUserID" runat="server" Value='<%# Eval("RcptUserID") %>' />
                    <asp:HiddenField ID="POHistoryNumber" runat="server" Value='<%# Eval("POHistoryNumber") %>' />
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcPOHistory" runat="server"    
        ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" >
        <SelectParameters>
            <asp:ControlParameter ControlID="hd_PONumber" Name="PONumber" 
                PropertyName="Value" />
        </SelectParameters>
    </asp:SqlDataSource>
    
    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
                    <asp:HiddenField ID="hd_PONumber" runat="server" />
            </form>
        </body>
</html>

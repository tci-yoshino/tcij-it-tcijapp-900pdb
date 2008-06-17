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
    <!-- Main Content Area -->
    <div id="content">
        <div class="tabs"></div>

        <h3>PO Correspondence</h3>

        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>

            <form id="POCorresForm" runat="server">
                <table>
                    <tr>
                        <th>Addressee <span class="required">*</span> : </th>
                        <td>
                            <asp:RadioButton ID="POUser" GroupName="Addressee" runat="server" /><span class="indent"><asp:Label ID="POLocation" runat="server" Text=""></asp:Label></span>
                            <br />
                            <asp:RadioButton ID="SOUser" GroupName="Addressee" runat="server" /><span class="indent"><asp:Label ID="SOLocation" runat="server" Text=""></asp:Label></span>
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
                        <th>Note : </th>
                        <td><asp:TextBox ID="CorresNote" runat="server" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                </table>
   
                <div class="btns">
                    <asp:Button ID="Send" runat="server" Text="Send" />
                </div>
            </form>
        </div>

        <hr />

        <h3>Correspondence History</h3>

        <div class="main">
            <asp:ListView ID="POHistory" runat="server" DataSourceID="SrcPOHistory">
                <AlternatingItemTemplate>
                    <table class="zebra2">
                        <tr>
                            <th style="width:20%">Status : </th>
                            <td style="width:65%"><strong><asp:Label ID="Status" runat="server" Text='' /></strong></td>
                            <td style="width:15%" rowspan="4">
                                <asp:HyperLink ID="Check" runat="server">
                                <asp:Image ID="ImgCheck" runat="server" ImageUrl="./Image/Check.gif" />Check</asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <th>Date : </th>
                            <td><asp:Label ID="CreateDate" runat="server" Text='' /></td>
                        </tr>
                        <tr>
                            <th>Sender : </th>
                            <td><asp:Label ID="CreatedBy" runat="server" Text='' /><span class="indent">(<asp:Label ID="Location" runat="server" Text='' />)</span></td>
                        </tr>
                        <tr>
                            <th>Addressee : </th>
                            <td><asp:Label ID="RcptUser" runat="server" Text='' /><span class="indent">(<asp:Label ID="RcptLocation" runat="server" Text='' />)</span></td>
                        </tr>
                        <tr>
                            <th>Note : </th>
                            <td><asp:Label ID="Note" runat="server" Text='' /></td>
                        </tr>
                    </table>
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
                            <td style="width:65%"><strong><asp:Label ID="Status" runat="server" Text='' /></strong></td>
                            <td style="width:15%" rowspan="4">
                                <asp:HyperLink ID="Check" runat="server">
                                <asp:Image ID="ImgCheck" runat="server" ImageUrl="./Image/Check.gif" />Check</asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <th>Date : </th>
                            <td><asp:Label ID="CreateDate" runat="server" Text='' /></td>
                        </tr>
                        <tr>
                            <th>Sender : </th>
                            <td><asp:Label ID="CreatedBy" runat="server" Text='' /><span class="indent">(<asp:Label ID="Location" runat="server" Text='' />)</span></td>
                        </tr>
                        <tr>
                            <th>Addressee : </th>
                            <td><asp:Label ID="RcptUser" runat="server" Text='' /><span class="indent">(<asp:Label ID="RcptLocation" runat="server" Text='' />)</span></td>
                        </tr>
                        <tr>
                            <th>Note : </th>
                            <td><asp:Label ID="Note" runat="server" Text='' /></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcPOHistory" runat="server" ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>"></asp:SqlDataSource>
    
    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
</body>
</html>

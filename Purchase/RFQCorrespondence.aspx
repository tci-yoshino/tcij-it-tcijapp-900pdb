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

        <div class="main">
            <p class="attention"><asp:Label ID="Msg" runat="server" Text=""></asp:Label></p>

                <table>
                    <tr>
                        <th>Addressee <span class="required">*</span> : </th>
                        <td>
                            <asp:RadioButton ID="EnqUser" GroupName="Addressee" runat="server" /><span class="indent"><asp:Label ID="EnqLocation" runat="server" Text=""></asp:Label></span>
                            <br />
                            <asp:RadioButton ID="QuoUser" GroupName="Addressee" runat="server" /><span class="indent"><asp:Label ID="QuoLocation" runat="server" Text=""></asp:Label></span>
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
        </div>

        <hr />

        <h3>Correspondence History<asp:HiddenField ID="Action" runat="server" 
                Value="Send" />
                        </h3>

        <div class="main">
            <asp:ListView ID="RFQHistory" runat="server" DataSourceID="SrcRFQHistory">
                <AlternatingItemTemplate>
                    <table class="zebra2">
                        <tr>
                            <th style="width:20%">Status:</th>
                            <td style="width:65%"><asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' /></td>
                            <td style="width:15%" rowspan="4">
                                <asp:HyperLink ID="Check" runat="server">
                                <asp:Image ID="ImgCheck" runat="server" ImageUrl="./Image/Check.gif" />Check</asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <th>Date:</th>
                            <td><asp:Label ID="DateLabel" runat="server" Text='<%# Eval("Date") %>' /></td>
                        </tr>
                        <tr>
                            <th>Sender:</th>
                            <td><asp:Label ID="SenderLabel" runat="server" Text='<%# Eval("Sender") %>' /></td>
                        </tr>
                        <tr>
                            <th>Addressee:</th>
                            <td><asp:Label ID="AddresseeLabel" runat="server" Text='<%# Eval("Addressee") %>' /></td>
                        </tr>
                        <tr>
                            <th>Notes:</th>
                            <td><asp:Label ID="NotesLabel" runat="server" Text='<%# Eval("Notes") %>' /></td>
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
                            <th style="width:20%">Status:</th>
                            <td style="width:65%"><asp:Label ID="StatusLabel" runat="server" Text='<%# Eval("Status") %>' /></td>
                            <td style="width:15%" rowspan="4">
                                <asp:HyperLink ID="Check" runat="server">
                                <asp:Image ID="ImgCheck" runat="server" ImageUrl="./Image/Check.gif" />Check</asp:HyperLink>
                            </td>
                        </tr>
                        <tr>
                            <th>Date:</th>
                            <td><asp:Label ID="DateLabel" runat="server" Text='<%# Eval("Date") %>' /></td>
                        </tr>
                        <tr>
                            <th>Sender:</th>
                            <td><asp:Label ID="SenderLabel" runat="server" Text='<%# Eval("Sender") %>' /></td>
                        </tr>
                        <tr>
                            <th>Addressee:</th>
                            <td><asp:Label ID="AddresseeLabel" runat="server" Text='<%# Eval("Addressee") %>' /></td>
                        </tr>
                        <tr>
                            <th>Notes:</th>
                            <td><asp:Label ID="NotesLabel" runat="server" Text='<%# Eval("Notes") %>' /></td>
                        </tr>
                    </table>
                </ItemTemplate>
            </asp:ListView>
        </div>
    </div><!-- Main Content Area END -->
    <asp:SqlDataSource ID="SrcRFQHistory" runat="server" 
    ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" SelectCommand="SELECT dbo.RFQStatus.Text AS Status, dbo.RFQHistory.StatusChangeDate AS Date,  dbo.v_User.Name + '      (' + dbo.v_User.LocationName + ')' AS Sender, v_User_1.Name AS Addressee, dbo.RFQHistory.Note AS Notes
FROM dbo.RFQHistory INNER JOIN dbo.RFQStatus ON dbo.RFQHistory.RFQStatusCode = dbo.RFQStatus.RFQStatusCode LEFT OUTER JOIN dbo.v_User AS v_User_1 ON dbo.RFQHistory.RcptUserID = v_User_1.UserID LEFT OUTER JOIN dbo.v_User ON dbo.RFQHistory.CreatedBy = dbo.v_User.UserID
WHERE (dbo.RFQHistory.RFQNumber = '1000000001')
ORDER BY dbo.RFQHistory.RFQHistoryNumber DESC"></asp:SqlDataSource>
    
    <!-- Footer -->
    <!--#include virtual="./Footer.html" --><!-- Footer END -->
            </form>
        </body>
</html>

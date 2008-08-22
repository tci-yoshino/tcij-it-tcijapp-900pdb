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
                        <th>Note : </th>
                        <td><asp:TextBox ID="CorresNote" runat="server" Columns="60" Rows="5" TextMode="MultiLine"></asp:TextBox></td>
                    </tr>
                </table>
   
                <div class="btns">
                    <asp:Button ID="Send" runat="server" Text="Send" />
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
                            <td>
                                <asp:Label ID="TitleLabel" runat="server" ForeColor="Red" Font-Bold="True" Text='<%# Eval("Title") %>' /><br />
                                <asp:Label ID="NotesLabel" runat="server" Text='<%# Eval("Notes") %>' />
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
                            <td>
                                <asp:Label ID="TitleLabel" runat="server" ForeColor="Red" Font-Bold="True" Text='<%# Eval("Title") %>' /><br />
                                <asp:Label ID="NotesLabel" runat="server" Text='<%# Eval("Notes") %>' />
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
                        
        ConnectionString="<%$ ConnectionStrings:DatabaseConnect %>" 
        
        SelectCommand="SELECT dbo.POStatus.Text AS Status, dbo.POHistory.CreateDate AS Date, dbo.v_User.Name + '(' + dbo.s_Location.Name + ')' AS Sender, v_User_1.Name + '(' + s_Location_1.Name + ')' AS Addressee, dbo.POCorres.Text AS Title, dbo.POHistory.Note AS Notes, dbo.POHistory.isChecked, dbo.POHistory.RcptUserID, dbo.POHistory.POHistoryNumber
FROM dbo.POHistory LEFT OUTER JOIN dbo.POCorres ON dbo.POHistory.POCorresCode = dbo.POCorres.POCorresCode LEFT OUTER JOIN dbo.s_Location AS s_Location_1 ON dbo.POHistory.RcptLocationCode = s_Location_1.LocationCode LEFT OUTER JOIN dbo.s_Location ON dbo.POHistory.SendLocationCode = dbo.s_Location.LocationCode LEFT OUTER JOIN dbo.v_User AS v_User_1 ON dbo.POHistory.RcptUserID = v_User_1.UserID LEFT OUTER JOIN dbo.v_User ON dbo.POHistory.SendUserID = dbo.v_User.UserID LEFT OUTER JOIN dbo.POStatus ON dbo.POHistory.POStatusCode = dbo.POStatus.POStatusCode
WHERE (dbo.POHistory.PONumber = @PONumber)
ORDER BY dbo.POHistory.POHistoryNumber DESC">
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

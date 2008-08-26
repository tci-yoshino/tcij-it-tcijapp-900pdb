<%@ Page Language="vb" AutoEventWireup="false" CodeBehind="SystemError.aspx.vb" Inherits="Purchase.SystemError" %>

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
    <h1 class="attention">Internal System Error</h1>

    <div class="main">
        <p style="margin-bottom:1em">I'm afraid we are currently having problems on the site.<br />An email has been sent to the system administrator to report the problem.</p>
        <input type="button" value="Back" onclick="javascript:history.back()" />
    </div>
    
<%  If b_IsDebug Then%>
    <hr />
    
    <div class="list">
        <h3><asp:Label ID="Message" runat="server" Text=""></asp:Label></h3>
        
        <h3>Stack Trace</h3>
        <pre><asp:Label ID="StackTrace" runat="server" Text=""></asp:Label></pre>
    </div>
<%End If%>

  </div><!-- Main Content Area END -->
</body>
</html>

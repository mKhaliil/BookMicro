<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="Outsourcing_System.WebForm2" %>

<%@ Register Assembly="PdfViewer" Namespace="PdfViewer" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="scripts/styles.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="margin: 100px;">
        <a class="workspace" href="img/Page-1.jpg">
            <img src="img/Page-1.jpg" style="width: 100%; height: 650px;" alt="" />
        </a>
    </div>
    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4.2/jquery.min.js"></script>
    <script src="scripts/jquery.mk.magnifier.js" type="text/javascript"></script>
    <script>
        $('.workspace').mkMagnifier({ width: 300, height: 460, ratio: 1.65, magnifier_radius: 90 });
</script>
    </form>
</body>
</html>

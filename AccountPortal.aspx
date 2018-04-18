<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AccountPortal.aspx.cs" Inherits="Outsourcing_System.AccountPortal" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>Untitled Page</title>
</head>
<body>
    <form id="form1" runat="server">
    <table width="100%">
    <tr>
        <td><asp:LinkButton PostBackUrl="~/AccountInformation.aspx" ID="LinkButton1" runat="server">Account Information</asp:LinkButton></td>        
        <td><asp:LinkButton PostBackUrl="~/Withdraw.aspx" ID="LinkButton2" runat="server">Withdraw Information</asp:LinkButton></td>
        <td><asp:LinkButton PostBackUrl="~/Transactions.aspx" ID="LinkButton3" runat="server">Transactions</asp:LinkButton></td>        
    </tr>
    </table>
    </form>
</body>
</html>

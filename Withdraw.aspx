<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/EditorMaster.Master"
    AutoEventWireup="true" CodeBehind="Withdraw.aspx.cs" Inherits="Outsourcing_System.Withdraw"
    Title="Withdraw" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
    <tr>
    <td colspan="2" align="center"><asp:Label ID="lblMessage" Text="" runat="server" /></td>
    </tr>
    <tr>
        <td align="right"><b>Amount Unpaid</b></td><td align="left"><asp:Label ID="lblAmount" Text="" runat="server" /></td>
    </tr>
    <tr>
        <td align="right"><b>Withdraw Amount</b></td><td align="left"><asp:TextBox ID="txtWithdrawAmount" runat="server"></asp:TextBox>
        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
            ControlToValidate="txtWithdrawAmount" ValidationGroup="v" ErrorMessage="* Amount"></asp:RequiredFieldValidator>
        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" 
            ControlToValidate="txtWithdrawAmount" ValidationGroup="v" ErrorMessage="Amount should be in digits" 
            ValidationExpression="([0-9])*(\.)?([0-9])+"></asp:RegularExpressionValidator>
        </td>
    </tr>
    <tr>
        <td>&nbsp;</td><td align="left">
                <asp:Button Text="Withdraw" runat="server" ValidationGroup="v" ID="btnWithdraw" onclick="btnWithdraw_Click" />&nbsp;<asp:Button ID="btnCancel" runat="server" 
                    Text="Cancel" onclick="btnCancel_Click" /></td>
    </tr>
    </table>
</asp:Content>

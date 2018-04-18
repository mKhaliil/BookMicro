<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/EditorMaster.Master"
    AutoEventWireup="true" CodeBehind="AccountInformation.aspx.cs" Inherits="Outsourcing_System.AccountInformation"
    Title="Account Information" %>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%">
    <tr>
    <td colspan="2" align="center"><asp:Label ID="lblMessage" Text="" runat="server" /><div style="float:left;"><a href="AccountDetail.aspx">Account Detail</a></div><div style="float:right;"><a href="AmountConfirmation.aspx">Amount Confirmation</a></div></td>
    </tr>
    <tr>
        <td align="right"><b>Account No</b></td><td align="left"><asp:TextBox ID="txtAcNo" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                    ControlToValidate="txtAcNo" ErrorMessage="*">*</asp:RequiredFieldValidator>
                </td>
    </tr>
    <tr>
        <td align="right"><b>Account Title</b></td><td align="left"><asp:TextBox ID="txtAcTitle" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                    ControlToValidate="txtAcTitle" ErrorMessage="*">*</asp:RequiredFieldValidator>
                </td>
    </tr>
    <tr>
        <td align="right"><b>Account Type</b></td><td align="left">
                <asp:DropDownList ID="ddAcType" runat="server">
                    <asp:ListItem Selected="True">Saving</asp:ListItem>
                    <asp:ListItem>Current</asp:ListItem>
                </asp:DropDownList>
                </td>
    </tr>
    <tr>
        <td align="right"><b>Bank Branch</b></td><td align="left"><asp:TextBox ID="txtBankBranch" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                    ControlToValidate="txtBankBranch" ErrorMessage="*">*</asp:RequiredFieldValidator>
                </td>
    </tr>
    <tr>
        <td align="right"><b>Branch Code</b></td><td align="left"><asp:TextBox ID="txtBankCode" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" 
                    ControlToValidate="txtBankCode" ErrorMessage="*">*</asp:RequiredFieldValidator>
                </td>
    </tr>   
    <tr>
        <td align="right"><b>City</b></td><td align="left"><asp:TextBox ID="txtCity" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" 
                    ControlToValidate="txtCity" ErrorMessage="*">*</asp:RequiredFieldValidator>
                </td>
    </tr>   
    <tr>
        <td align="right"><b>Country</b></td><td align="left"><asp:TextBox ID="txtCountry" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" 
                    ControlToValidate="txtCountry" ErrorMessage="*">*</asp:RequiredFieldValidator>
                </td>
    </tr>  
    <tr>
        <td align="right"><b>Pending Transaction Amount</b></td><td align="left"><asp:Label ID="lblUnpaidAmount" runat="server" Text="0.00" /></td>
    </tr>  
    <tr>
        <td align="right"><b>Total Amount</b></td><td align="left"><asp:Label ID="lblTotalAmount" runat="server" Text="0.00" /></td>
    </tr>  
    <tr>
        <td>&nbsp;</td><td align="left">
                <asp:Button Text="Submit" runat="server" ID="btnSubmit" onclick="btnSubmit_Click" 
                     />&nbsp;<asp:Button ID="btnUpdate" runat="server" 
                    Text="Update" onclick="btnUpdate_Click" /></td>
    </tr>
    </table>
   </asp:Content>

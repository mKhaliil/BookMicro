<%@ Page Title="" Language="C#" MasterPageFile="~/UserMaster.Master" AutoEventWireup="true" CodeBehind="ForgotPassword.aspx.cs" Inherits="Outsourcing_System.ForgotPassword" %>

<%@ Register src="UserControls/CommonControl/ucShowMessage.ascx" tagname="ucShowMessage" tagprefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainHeadContents" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainBodyContents" runat="server">

        <div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">
                   <uc1:ucShowMessage ID="ucShowMessage1" runat="server" />
                </div>

    <div id="wraper" style="width:60%; min-height: 30%; margin-left:auto; margin-right:auto; background-color:#e9e9e9">
        <div style="margin-left: 2%;">
            
        </div>
        <table style="margin-left: 2%; margin-top:5%;">
            <tr>
                <td><h3 style="color: #2a4f96; font-size: 18px;">Please enter your email address used for Bookmicro login. New password will be send at this address.</h3></td>

                
            </tr>
            <tr><td></td></tr>
            <tr><td> <asp:ValidationSummary ID="vsSendEmail" runat="server" ValidationGroup="SendEmail"
                        ForeColor="red" DisplayMode="List" /></td></tr>
            <tr><td></td></tr>
            <tr>
                <td>
                    <p style="color: #2a4f96; font-size: 18px;">Email :
                    <asp:TextBox ID="tbxEmail" runat="server" style="margin-left:2%;width:300px;"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="tbxEmail" ErrorMessage="Email is required." ForeColor="red" 
                             Text="*" ToolTip="Required Field" ValidationGroup="SendEmail"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" ControlToValidate="tbxEmail"
                        runat="server" ValidationExpression="\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*"
                        ValidationGroup="SendEmail" ForeColor="red" Text="*" ErrorMessage="Email is not vaild."></asp:RegularExpressionValidator>
                        <asp:Button ID="btnSendPassword" runat="server" ValidationGroup="SendEmail" Text="Send Password" Width="100px" 
                            CssClass="button"  OnClick="btnSendPassword_Click"/>
                    </p>
                </td>
            </tr>
            
        </table>
    </div>
</asp:Content>

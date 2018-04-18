<%@ Page Title="" Language="C#" MasterPageFile="~/UserMaster.Master" AutoEventWireup="true" CodeBehind="ChangePassword.aspx.cs" Inherits="Outsourcing_System.ChangePassword" %>
<%@ Register src="UserControls/CommonControl/ucShowMessage.ascx" tagname="ucShowMessage" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainHeadContents" runat="server">

    <style type="text/css">
        #divChangePassword {
    border: 1px solid #cccccc;
    border-radius: 25px;
    padding: 10px 10px 10px 10px;
    text-align: left;
    width: 100%;
    margin-left: auto;
    margin-right: auto;
    margin-bottom: 60px;
    background-color: #ADDFFF;
}
    </style>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainBodyContents" runat="server">
    
     <div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">
                <uc1:ucShowMessage ID="ucShowMessage1" runat="server" />
                </div>
    
       <div id="Passwordwrapper" style="margin-left:auto; margin-right:auto; width:44%; margin-top:3.7%" >
           
           <h1>
        <strong style="margin-left: auto; margin-right: auto;">Change Password</strong>
    </h1>

       <div id="divChangePassword">
            <table style="margin-top: 5%; margin-bottom: 7%; margin-left: 5%; width: 100%; margin-right: 0%;">
                <tr><td colspan="3"><asp:ValidationSummary ID="vsChangePassword" runat="server" ValidationGroup="changePassword" ForeColor="red" DisplayMode="List" />
</td></tr>
                <tr>
                    <td colspan="3">
                       <%-- <p><h3 style="color: #2a4f96; font-size: 28px;">Change Password</h3></p>--%>

                    </td></tr>
                  <tr>
                    <td align="left" width="35%">
                        <strong style="color: #2a4f96; font-size: 18px;">Email: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxEmail" Style="border: 1px solid black; height: 30px; width: 300px; border-radius: 10px" runat="server"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfvEmail" ValidationGroup="changePassword" runat="server" ErrorMessage="Email is required." 
                        ControlToValidate="tbxEmail" ToolTip="Email is a required field" ForeColor="red" Text="*">
                      </asp:RequiredFieldValidator>
                         <asp:RegularExpressionValidator ID="revEmail" ControlToValidate="tbxEmail"
                        runat="server" ValidationExpression="\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*"
                        ValidationGroup="changePassword" ForeColor="red" Text="*" ErrorMessage="Email is not vaild."></asp:RegularExpressionValidator>
                    </td>
                </tr>
                <tr>
                    <td align="left">
                        <strong style="color: #2a4f96; font-size: 18px;">Old Password: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxOldPassword" TextMode="Password" Style="border: 1px solid black; height: 30px; width: 300px; border-radius: 10px" runat="server"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfvOldPassword" ValidationGroup="changePassword" runat="server" ErrorMessage="Old Password is required." 
                        ControlToValidate="tbxOldPassword" ToolTip="Old Password is a required field" ForeColor="red" Text="*">
                      </asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong style="color: #2a4f96; font-size: 18px;">New Password: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxNewPassword"  TextMode="Password" Style="border: 1px solid black; height: 30px; width: 300px; border-radius: 10px" runat="server"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rvfNewPassword" ValidationGroup="changePassword" runat="server" ErrorMessage="New Password is required." 
                        ControlToValidate="tbxNewPassword" ToolTip="New Password is a required field" ForeColor="red" Text="*">
                      </asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong style="color: #2a4f96; font-size: 18px;">Confirm New Password: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxConNewPassword" TextMode="Password" Style="border: 1px solid black; height: 30px; width: 300px; border-radius: 10px" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvConNewPassword" ValidationGroup="changePassword" runat="server" ErrorMessage="Confirm New Password is required." 
                        ControlToValidate="tbxConNewPassword" ToolTip="Confirm New Password is a required field" ForeColor="red" Text="*">
                      </asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvConNewPassword" ValidationGroup="changePassword" runat="server" ControlToValidate="tbxConNewPassword" ControlToCompare="tbxNewPassword"
                         ErrorMessage="Confirm New Password don't match with new password." ToolTip="Password must be the same" ForeColor="red" Text="*"/>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"></td>
                    </tr>
                <tr>
                    <td></td>
                    <td align="left">
                        <asp:Button ID="btnChangePassword" runat="server" ValidationGroup="changePassword" CssClass="button" Text="Change Password" Style="width: 120px; border: 1px solid black;"
                            OnClick="btnChangePassword_Click" />
                      <%--  <asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close" Style="width: 60px; border: 1px solid black; margin-left: 2px;"
                            OnClick="btnClose_Click" />--%>
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/MasterPages/OnlineTestMasterPage.Master" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="Outsourcing_System.Signup" Title="Untitled Page" %>
<%@ Register src="CustomControls/ProcessControl.ascx" tagname="ProcessControl" tagprefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="LinkPortion" runat="server">
<div style="float:left"><a class="link" href="UpdateUser.aspx">Update User Info</a></div>
<div style="float:right"><asp:LinkButton ID="lnkLogout" runat="server" onclick="lnkLogout_Click" class="link">Logout</asp:LinkButton></div>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" align="center">
        <tr>
            <td align="right" class="normaltext" style="height: 37px" >Full Name :</td>
            <td align="left"  style="height: 37px"><asp:TextBox class="normaltext" ID="txtFullName" runat="server"></asp:TextBox></td>
         <tr>
            <td align="right" class="normaltext" style="height: 33px">User Name :</td>
            <td align="left"  style="height: 33px"><asp:TextBox  class="normaltext" ID="txtUserName" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right" class="normaltext" style="height: 32px">Password :</td>
            <td align="left"  style="height: 32px">
                <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"  class="normaltext"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td align="right" class="normaltext" style="height: 34px">Confirm Password :</td>
            <td align="left"  style="height: 34px">
                <asp:TextBox ID="txtConfirmPassword" runat="server" TextMode="Password"  class="normaltext"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right"  class="normaltext" style="height: 43px">Email :</td>
            <td align="left"  style="height: 43px"><asp:TextBox ID="txtEmail" runat="server"  class="normaltext"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right"  class="normaltext" style="height: 23px">Category</td>
            <td align="left" style="height: 23px"><asp:DropDownList ID="ddCategory" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right"  class="normaltext" style="height: 23px">Can Perform Tasks</td>
            <td align="left" style="height: 23px">
                <asp:Panel ID="PanelTasks" runat="server">
                    <uc1:ProcessControl ID="ProcessControl1" runat="server" />
                </asp:Panel>
            </td>
        </tr>
        <tr>
            <td>&nbsp;</td>
            <td align="left" style="height: 23px"><asp:Button ID="btnAddUser" runat="server" Text="Add User" CssClass="button" 
                    onclick="btnAddUser_Click" />
            
            </td>
        </tr>

    </table>
</asp:Content>

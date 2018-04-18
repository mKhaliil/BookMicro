<%@ Page Language="C#" MasterPageFile="~/MasterPages/AdminMaster_Hiring.Master" AutoEventWireup="true" CodeBehind="UpdateUser.aspx.cs" Inherits="Outsourcing_System.UpdateUser" Title="Untitled Page" %>
<%@ Register src="CustomControls/ProcessControl.ascx" tagname="ProcessControl" tagprefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="LinkPortion" runat="server">
<!--div style="float:left"><a class="link" href="AdminPanel.aspx">Admin Panel</a></div-->
<!--div style="float:right"><asp:LinkButton ID="lnkLogout" runat="server" onclick="lnkLogout_Click" class="link">Logout</asp:LinkButton></div-->

</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table width="100%" align="center">
        
        <tr>
            <td align="right" class="normaltext" >User Name :</td>
            <td align="left"><asp:DropDownList ID="ddUser" runat="server" style="width:146px" 
                    AutoPostBack="True" onselectedindexchanged="ddUser_SelectedIndexChanged"></asp:DropDownList>
            </td>
         </tr>
        <tr>
            <td align="right" class="normaltext" >Full Name :</td>
            <td align="left"><asp:TextBox class="normaltext" ID="txtFullName" runat="server"></asp:TextBox></td>
         </tr>
         
        <tr>
            <td align="right" class="normaltext">Password :</td>
            <td align="left">
                <asp:TextBox ID="txtPassword" runat="server"  class="normaltext"></asp:TextBox>
            </td>
        </tr>
         <tr>
            <td align="right" class="normaltext">Confirm Password :</td>
            <td align="left">
                <asp:TextBox ID="txtConfirmPassword" runat="server"  class="normaltext"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right"  class="normaltext">Email :</td>
            <td align="left"><asp:TextBox ID="txtEmail" runat="server"  class="normaltext"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right"  class="normaltext" style="height: 23px">Category</td>
            <td align="left" style="height: 23px"><asp:DropDownList ID="ddCategory" runat="server"></asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" class="normaltext">Active User :</td>
            <td align="left"><asp:DropDownList ID="ddUserStatus" runat="server" CssClass="normaltext" style="width:146px">
            <asp:ListItem Text="Active" Value="1" />
            <asp:ListItem Text="Disabled" Value="0" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="right"  class="normaltext">Can Perform Tasks</td>
            <td align="left">
                <uc1:ProcessControl ID="ProcessControl1" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="left">&nbsp;</td>
            <td align="left"><asp:Button ID="btnUpdateUser" 
                    runat="server" Text="Update User" CssClass="button" 
                    onclick="btnUpdateUser_Click" /></td>
        </tr>

    </table>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="~/EditorMaster.master" AutoEventWireup="true" CodeBehind="Mail.aspx.cs" Inherits="Outsourcing_System.Mail" Title="Untitled Page" %>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
<asp:Label ID="lblMessage" runat="server" CssClass="message" />
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="LinkPortion" runat="server">
<!--table width="100%">
<tr>
    <td align="left" style="width:19%"><asp:LinkButton CssClass="link" 
            ID="lnkUserlPanel" runat="server" onclick="lnkUserlPanel_Click">User Panel</asp:LinkButton></td>
    <td align="left" style="width:19%"><asp:LinkButton CssClass="link" ID="lnkInbox" 
            runat="server" onclick="lnkInbox_Click">Inbox</asp:LinkButton></td>
    <td align="left" style="width:19%"><asp:LinkButton CssClass="link" 
            ID="lnkComposeMail" runat="server" onclick="lnkComposeMail_Click">Compose Mail</asp:LinkButton></td>
    <td align="left" style="width:19%"><asp:LinkButton CssClass="link" ID="lnkOutbox" 
            runat="server" onclick="lnkOutbox_Click">Outbox</asp:LinkButton></td>
    <td align="left" style="width:19%"><asp:LinkButton CssClass="link" ID="lnkSentMail" 
            runat="server" onclick="lnkSentMail_Click">Sent Mail</asp:LinkButton></td>
    <td align="right" style="width:5%"><asp:LinkButton CssClass="link" ID="lnkLogout" 
            runat="server" onclick="lnkLogout_Click">Logout</asp:LinkButton></td>
</tr>
</table-->
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <table>        
        <tr>
            <td align="right" class="normaltext">To :</td>
            <td align="left"><asp:DropDownList ID="To" runat="server" CssClass="normaltext" style="width:265px"></asp:DropDownList></td>
        </tr>
        <tr>
            <td align="right" class="normaltext">Subject :</td>
            <td align="left"><asp:TextBox ID="txtSubject" runat="server" CssClass="normaltext"  style="width:260px"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right" class="normaltext" valign="top">Message :</td>
            <td align="left"><asp:TextBox TextMode="MultiLine" Columns="30" Rows="10" ID="txtMessage" runat="server" CssClass="normaltext"></asp:TextBox></td>
        </tr>
        <tr>
            <td align="right">&nbsp;</td>
            <td align="left"><asp:Button ID="Send" runat="server" Text="Send" CssClass="button" onclick="Send_Click"/></td>
        </tr>
        </table>
</asp:Content>

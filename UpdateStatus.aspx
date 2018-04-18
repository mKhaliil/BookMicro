<%@ Page Language="C#" MasterPageFile="AdminMaster.Master" AutoEventWireup="true" Inherits="UpdateStatus" Title="Untitled Page" Codebehind="UpdateStatus.aspx.cs" %>

<asp:Content ID="Content4" ContentPlaceHolderID="mainHeadContents" runat="server">
    <div style="float:left"><a class="link" href="AdminPanel.aspx">Admin Panel</a></div>
<div style="float:right"><asp:LinkButton ID="lnkLogout" runat="server" onclick="lnkLogout_Click" class="link">Logout</asp:LinkButton></div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="mainBodyContents" runat="server">
<table>           

        <!--<asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
            <ContentTemplate>-->
               <tr>
                    <td align="right" valign="top" class="bbw">Status :</td>
                    <td align="left">
                        <asp:DropDownList ID="ddStatus" runat="server">
                        <asp:ListItem>Unassigned</asp:ListItem>
                        <asp:ListItem>Working</asp:ListItem>
                        <asp:ListItem>Pending Confirmation</asp:ListItem>
                        <asp:ListItem>Approved</asp:ListItem>
                        <asp:ListItem>Rejected</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>     
               <tr>
                    <td align="right" valign="top" class="bbw">File :</td>
                    <td align="left"><input type="file" id="File1" class="bbw" runat="server" /></td>
                </tr>     
                <tr>
                    <td align="right" valign="top" class="bbw">&nbsp;</td>
                    <td align="left"><asp:Button ID="btnAssign" runat="server" 
                            Text="Submit" CssClass="button" onclick="btnAssign_Click" />&nbsp;&nbsp;<asp:Button 
                            ID="btnCancel" runat="server" Text="Cancel" CssClass="button" 
                            onclick="btnCancel_Click" /></td>
                </tr>          
            <!--</ContentTemplate>                   
        </asp:UpdatePanel>-->
        </table>
    
    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="message"></asp:Label>
</asp:Content>
<%@ Page Language="C#" MasterPageFile="AdminMaster.Master" AutoEventWireup="true" CodeBehind="ErrorAdjustment.aspx.cs" Inherits="Outsourcing_System.ErrorAdjustment" Title="Untitled Page" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="Server">
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
 <asp:Label ID="lblMessage" runat="server" Text="" CssClass="message"></asp:Label>

<div id="errors" runat="server" /><br />
    <table>                
        <tr>
            <td style="text-align: right" class="bbw">Select Corrected .RHYW File :</td>
            <td style="text-align: left" class="bbw"><input id="File1" type="file" runat="server" /></td>
         </tr>
         <tr>
            <td>&nbsp;</td>
            <td style="text-align: left"><asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                    onclick="Submit_Click" CssClass="button" />&nbsp;&nbsp;
                </td>
        </tr>
    </table>
</asp:Content>

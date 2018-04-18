<%@ Page Language="C#" MasterPageFile="~/AdminMaster.Master" Async="true" AutoEventWireup="true" CodeBehind="UploadZIP.aspx.cs" Inherits="Outsourcing_System.UploadZIP" Title="Untitled Page" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
 <asp:Label ID="lblMessage" runat="server" Text="" CssClass="message"></asp:Label>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
    <table>          
        <tr>
            <td style="text-align: right" class="bbw">Select Zip File :</td>
            <td style="text-align: left" class="bbw"><input id="File1" type="file" runat="server" /></td>
         </tr>
         <tr>
            <td style="text-align: right" class="bbw">Select CSV File :</td>
            <td style="text-align: left" class="bbw"><input id="File2" type="file" runat="server" /></td>
         </tr>

         <tr>
            <td>&nbsp;</td>
            <td style="text-align: left"><asp:Button ID="btnSubmit" runat="server" Text="Submit" 
                    onclick="Submit_Click" CssClass="button" />&nbsp;&nbsp;
                </td>
        </tr>
    </table>
</asp:Content>

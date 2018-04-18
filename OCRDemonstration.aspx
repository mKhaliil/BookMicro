<%@ Page Title="" Language="C#" MasterPageFile="~/EditorMaster.Master" AutoEventWireup="true"
    CodeBehind="OCRDemonstration.aspx.cs" Inherits="Outsourcing_System.OCRDemonstration" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder0" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LinkPortion" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td style="text-align: left;">
                Please upload Image to OCR:
            </td>
            <td style="text-align: left;">
                <asp:FileUpload ID="FileUpload1" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="text-align: left;">
                <asp:Image ID="img" runat="server"/>
            </td>
            <td style="text-align: left;">
                <asp:TextBox ID="txtContent" Width="990" Height="405" runat="server" TextMode="MultiLine"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
            </td>
            <td>
                <asp:Button ID="btnOCR" runat="server" Text="Perform OCR" OnClick="btnOCR_Click" />
                <asp:Button ID="btnDownload" runat="server" Text="Download .txt" OnClick="btnDownload_Click"/>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

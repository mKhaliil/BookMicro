<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AdminMaster_Hiring.Master" AutoEventWireup="true"
    CodeBehind="ValueAddSettings.aspx.cs" Inherits="Outsourcing_System.ValueAddSettings" %>

<asp:Content ID="Content1" ContentPlaceHolderID="LinkPortion" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                Insert Volume Breaks:
            </td>
            <td>
                <asp:CheckBox ID="cbcVolumBreaks" runat="server" />
            </td>
        </tr>
        <tr>
            <td>
                No of Pages More than:
            </td>
            <td>
                <asp:TextBox ID="txtpages" runat="server" Width="220"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnUpdate" runat="server" Text="Update" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/EditorMaster.Master" AutoEventWireup="true"
    CodeBehind="OnlineTest.aspx.cs" Inherits="Outsourcing_System.OnlineTest" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder0" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LinkPortion" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table>
        <tr>
            <td>
                <asp:Label ID="lblName" runat="server" Text="Enter Name:"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtName" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblPass" runat="server" Text="Password"></asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtPassword" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td colspan="2">
                <asp:Button ID="btnTest" runat="server" Text="Go For Test" OnClick="btnTest_Click" />
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

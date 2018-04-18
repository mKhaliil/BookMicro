<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/AdminMaster_Hiring.Master" AutoEventWireup="true"
    CodeBehind="AddVolumeBreaks.aspx.cs" Inherits="Outsourcing_System.AddVolumeBreaks" %>
    <%@ MasterType VirtualPath="~/MasterPages/AdminMaster_Hiring.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LinkPortion" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <div style="height: 50px;">
        </div>
        <table width="70%">
            <%-- <tr>
                <td style="width: 190px;">
                    Select Book ID:
                </td>
                <td>
                    <asp:DropDownList ID="ddlBookIds" Width="220" runat="server">
                    </asp:DropDownList>
                </td>
            </tr>--%>
            <tr>
                <td style="width: 190px;">
                    Upload Edited Pdf:
                </td>
                <td>
                    <asp:FileUpload ID="FileUpload1" runat="server" />
                    <asp:Button ID="btnUpload" runat="server" Text="Upload File" OnClick="btnUpload_Click" />
                </td>
            </tr>
            <tr>
                <td style="width: 190px;">
                    Insert Volume Breaks After:
                </td>
                <td>
                    <asp:DropDownList ID="ddlPages" Width="50" runat="server">
                        <asp:ListItem Text="3" Value="3"></asp:ListItem>
                        <asp:ListItem Text="5" Value="5"></asp:ListItem>
                        <asp:ListItem Text="8" Value="8"></asp:ListItem>
                        <asp:ListItem Text="10" Value="10"></asp:ListItem>
                    </asp:DropDownList>
                    &nbsp Pages
                </td>
            </tr>
            <tr>
                <td style="width: 190px;">
                </td>
                <td>
                    <asp:Button ID="btnInsertVolumeBreak" runat="server" Text="Insert Volume Breaks"
                        OnClick="btnInsertVolumeBreak_Click" />
                </td>
            </tr>
        </table>
        <div style="height: 50px;">
        </div>
    </div>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

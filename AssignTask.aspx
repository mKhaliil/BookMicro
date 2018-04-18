<%@ Page Language="C#" MasterPageFile="~/MasterPages/AdminMaster_Hiring.Master" AutoEventWireup="true"
    Inherits="AssignTask" Title="Untitled Page" CodeBehind="AssignTask.aspx.cs" %>

<%@ Register Src="CustomControls/ProcessControl.ascx" TagName="ProcessControl" TagPrefix="uc1" %>
<asp:Content ID="Content3" ContentPlaceHolderID="LinkPortion" runat="server">
    <!--div style="float:left"><a class="link" href="AdminPanel.aspx">Admin Panel</a></div-->
    <!--div style="float:right"><asp:LinkButton ID="lnkLogout" runat="server" onclick="lnkLogout_Click" class="link">Logout</asp:LinkButton></div-->
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table>
        <tr>
            <td class="normaltext" align="right">
                For Task :
            </td>
            <td class="normaltext" align="left">
                <uc1:ProcessControl ID="ProcessControl1" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right" valign="top" class="normaltext">
                Dead Line :
            </td>
            <td align="left">
                <input runat="server" class="normaltext" name="Calendar1" id="Calendar1" autocomplete="off"
                    onfocus="showDcsCalendar(this);" type="text" style="width: 220px" /><asp:RequiredFieldValidator
                        ID="RequiredFieldValidator1" runat="server" ErrorMessage="*" ControlToValidate="Calendar1"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td align="right" valign="top" class="normaltext">
                Comments :
            </td>
            <td align="left">
                <asp:TextBox ID="txtComments" runat="server" Columns="25" Rows="10" TextMode="MultiLine"
                    CssClass="normaltext"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="right" class="normaltext">
                Extra Attachment :
            </td>
            <td align="left">
                <input type="file" id="File1" runat="server" />
            </td>
        </tr>
        <tr>
            <td align="right">
            </td>
            <td align="left">
                <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Button ID="btnShowUser" runat="server" Text="Assign" CssClass="button" OnClick="btnShowUser_Click" />
                        <br />
                        <asp:Label runat="server" ID="lblStatus" Text="" CssClass="message" />
                        <br />
                        <asp:Panel ID="Panel1" runat="server" Visible="false">
                            <asp:ListBox ID="lstUser" runat="server" CssClass="normaltext" Style="width: 230px"
                                SelectionMode="Multiple" OnSelectedIndexChanged="lstUser_SelectedIndexChanged"
                                AutoPostBack="True"></asp:ListBox>
                            <br />
                            <asp:Label ID="lblRate" runat="server" ForeColor="#009900" />
                            <br />
                            <asp:Button ID="btnAssign" runat="server" Text="Assign" CssClass="button" OnClick="btnAssign_Click" />
                            <br />
                            <br />
                            <span runat="server" id="instruction"><font color='red'>*</font><sub> All Tasks except
                                Image, TaggingUntagged and Meta show amount per unit</sub></span>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </td>
        </tr>
    </table>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="message"></asp:Label>
</asp:Content>

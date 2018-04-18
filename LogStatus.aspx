<%@ Page Language="C#" MasterPageFile="~/MasterPages/OnlineTestMasterPage.Master" AutoEventWireup="true" Inherits="LogStatus" Title="Untitled Page" Codebehind="LogStatus.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/AdminMaster_Hiring.Master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="LinkPortion" runat="server">
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div align="center" class="bbw">Book ID : <asp:TextBox ID="txtBookID" runat="server"></asp:TextBox> 
    <asp:Button ID="btnShow" runat="server" Text="Show" onclick="btnShow_Click" /></div>
    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
        GridLines="None" Width="100%" AutoGenerateColumns="false" 
        AllowPaging="true" PageSize="100">
        <Columns>
            <asp:BoundField HeaderText="Book ID" DataField="MainBook" 
                ItemStyle-Width="10%" >
<ItemStyle Width="10%"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Process" DataField="Process" ItemStyle-Width="13%" >
<ItemStyle Width="13%"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Status" DataField="Status"  ItemStyle-Width="10%" >
<ItemStyle Width="10%"></ItemStyle>
            </asp:BoundField>
            <asp:BoundField HeaderText="Detail" DataField="Detail" ItemStyle-Width="62%" >
<ItemStyle Width="62%"></ItemStyle>
            </asp:BoundField>
            <asp:HyperLinkField HeaderText="Del" DataTextField="LogID" 
                DataNavigateUrlFields="LogID" 
                DataNavigateUrlFormatString="LogStatus.aspx?logid={0}" ItemStyle-Width="5%" >
            
<ItemStyle Width="5%"></ItemStyle>
            </asp:HyperLinkField>
            
        </Columns>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <RowStyle BackColor="#EFF3FB" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" HorizontalAlign="Left" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
</asp:Content>
<asp:Content ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="message"></asp:Label>
</asp:Content>


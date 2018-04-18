<%@ Page Language="C#" MasterPageFile="~/MasterPages/AdminMaster_Hiring.Master" AutoEventWireup="true" Inherits="CompleteBooks" Title="Untitled Page" Codebehind="CompleteBooks.aspx.cs" %>
<%@ MasterType VirtualPath="~/MasterPages/AdminMaster_Hiring.Master" %>
<asp:Content ID="Content3" ContentPlaceHolderID="LinkPortion" runat="server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div align="center" class="bbw">Book ID : <asp:TextBox ID="txtBookID" runat="server"></asp:TextBox> 
    <asp:Button ID="btnShow" runat="server" Text="Show" onclick="btnShow_Click" /></div>                     
    <asp:GridView ID="GridView1" runat="server" CellPadding="4" ForeColor="#333333" 
        GridLines="None" Width="100%" AutoGenerateColumns="false" DataKeyNames="MainBook" 
        AllowPaging="true" onpageindexchanging="GridView1_PageIndexChanging">
        <Columns>
        <asp:BoundField HeaderText="Book ID" DataField="MainBook" />
        <asp:TemplateField HeaderText="Detail of the Book">
            <ItemTemplate>   
                    <%# InnerContent(Eval("MainBook").ToString(), Eval("Status").ToString(),"")%>
            </ItemTemplate>
        </asp:TemplateField>
        <asp:TemplateField HeaderText="Other Action">
            <ItemTemplate>   
                    <%# InnerContent(Eval("MainBook").ToString(), Eval("Status").ToString(),"last")%>
            </ItemTemplate>
        </asp:TemplateField>
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


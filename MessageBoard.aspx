<%@ Page Language="C#" MasterPageFile="~/EditorMaster.master" AutoEventWireup="true" CodeBehind="MessageBoard.aspx.cs" Inherits="Outsourcing_System.MessageBoard" Title="Untitled Page" %>


<asp:Content ID="Content1" ContentPlaceHolderID="LinkPortion" runat="server">
<!--table width="100%">
<tr>
    <td align="left" style="width:19%"><asp:LinkButton CssClass="link" 
            ID="lnkUserlPanel" runat="server" onclick="lnkUserlPanel_Click">Main Panel</asp:LinkButton></td>
    <td align="left" style="width:19%"><asp:LinkButton CssClass="link" ID="lnkInbox" 
            runat="server" onclick="lnkInbox_Click">Inbox</asp:LinkButton></td>
    <td align="left" style="width:19%"><asp:LinkButton CssClass="link" 
            ID="lnkComposeMail" runat="server" onclick="lnkComposeMail_Click">Compose Mail</asp:LinkButton></td>
    <td align="left" style="width:19%"><asp:LinkButton CssClass="link" ID="lnkOutbox" 
            runat="server" onclick="lnkOutbox_Click">Outbox</asp:LinkButton></td>
    <td align="left" style="width:19%"><asp:LinkButton CssClass="link" ID="lnkSentMail" 
            runat="server" onclick="lnkSentMail_Click">Sent Mail</asp:LinkButton></td>
    <td align="right" style="width:5%"><asp:LinkButton CssClass="link" ID="lnkLogout" 
            runat="server" onclick="lnkLogout_Click">Logout</asp:LinkButton></td>
</tr>
</table-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">

<asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            DataKeyNames="MailID" AllowPaging="True" 
            PageSize="20" Width="100%" 
            onselectedindexchanged="GridView1_SelectedIndexChanged" BorderWidth="1px" 
            BackColor="Transparent" GridLines="Vertical"   >
            <RowStyle  />
        <Columns>
            <asp:CommandField ShowSelectButton="True" ControlStyle-CssClass="link" >
<ControlStyle CssClass="link"></ControlStyle>
            </asp:CommandField>
            <asp:TemplateField HeaderText="Subject">
                <ItemTemplate>
                    <asp:Image ID="Image1" runat="server" ImageUrl='<%# CheckImage(Eval("Stat").ToString()) %>' />            
                    <asp:Label Text='<%# Eval("Subject") %>' ID="label2" runat="server" />                    
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Reciever--Sender">
                <ItemTemplate>
                    <asp:Label Text='<%# Eval("Reciever")+"--"+Eval("Sender") %>' ID="label2" runat="server" />
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Date">
                <ItemTemplate>
                    <asp:Label Text='<%# Eval("Dat","{0:dd-M-yyyy}") %>' ID="label1" runat="server" />
                </ItemTemplate>
                <HeaderStyle HorizontalAlign="Left" />
                <ItemStyle HorizontalAlign="Left" />
            </asp:TemplateField>
            
        </Columns>
            <HeaderStyle BackColor="#507CD1" ForeColor="White" />
    </asp:GridView>
 
        <div id="message" runat="server" />
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" />
</asp:Content>

<%@ Page Language="C#" MasterPageFile="AdminMaster.Master" AutoEventWireup="true" Inherits="ApprovedTask" Title="Untitled Page" 
Codebehind="ApprovedTaskDetails.aspx.cs" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
<!--table width="100%">
    <tr>
        <td align="left"><asp:LinkButton CssClass="link" ID="lnkNewTask" runat="server" PostBackUrl="~/NewTask.aspx">Add New Task</asp:LinkButton></td>
               
        <td align="left"><asp:LinkButton ID="lnkAdminPanel" runat="server" CssClass="link" onclick="lnkAdminPanel_Click">Admin Panel</asp:LinkButton></td>
        <td align="left"><asp:LinkButton ID="lnkAddUser" runat="server" CssClass="link" 
                    onclick="lnkAddUser_Click">Manage User</asp:LinkButton></td>
        <td align="right"><asp:LinkButton ID="lnkLogout" runat="server" CssClass="link"
                    onclick="lnkLogout_Click">Logout</asp:LinkButton></td>
    </tr>
  </table-->
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" Runat="Server">

    <asp:Repeater ID="Repeater1" runat="server" >
    <HeaderTemplate>
        <table width="100%">
        <tr>
            <TH align="left">Action</TH>
            <TH align="left" style="padding-left:5pt;">Book Title</TH>
            <TH align="left" style="padding-left:5pt;">Download</TH>            
            <TH align="left" style="padding-left:5pt;">Assign Date</TH>
            <TH align="left" style="padding-left:5pt;">Assigned To</TH>
            <TH align="left" style="padding-left:5pt;">For Task</TH>
            <TH align="left" style="padding-left:5pt;">Deadline</TH>                       
            <TH align="left" style="padding-left:5pt;">Comp. Date</TH>
             <TH align="left" style="padding-left:5pt;">Status</TH>
        </tr>
    </HeaderTemplate>
    
    <ItemTemplate>
        <tr>
            <td align="left"><a class="link" href='AssignTask.aspx?bid=<%# Eval("BID")%>'>Assign Task</a></td>
            <td align="left" class="normaltext" style="padding-left:5pt;"><%# Eval("BTitle")%></td>
            <td align="left" style="padding-left:5pt;"><%#DownloadLink(Eval("BIdentityNo").ToString(), Eval("Task").ToString(), Eval("CompletionDate", "{0:dd-M-yyyy}"))%></td>
            <td align="left" class="normaltext" style="padding-left:5pt;"><%# Eval("[UpDate]", "{0:dd-M-yyyy}")%></td>
            <td align="left" class="normaltext" style="padding-left:5pt;"><%# Eval("[AssignedTo]")%></td>
            <td align="left" class="normaltext" style="padding-left:5pt;"><%# Eval("Task")%></td>
            <td align="left" class="normaltext" style="padding-left:5pt;"><%# Eval("DeadLine", "{0:dd-M-yyyy}")%></td>
            <td align="left" class="normaltext" style="padding-left:5pt;"><%# Eval("CompletionDate", "{0:dd-M-yyyy}")%></td>            
            <td align="left" class="normaltext" style="padding-left:5pt;"><%# Eval("Status")%></td>           
        </tr>
    </ItemTemplate>
    <FooterTemplate>
        </table>
    </FooterTemplate>
    </asp:Repeater>
    
    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="message"></asp:Label>
</asp:Content>



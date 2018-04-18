<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="AdminMaster.Master"
    AutoEventWireup="true" Inherits="UserPanel" Title="Untitled Page" CodeBehind="UserPanel.aspx.cs" %>

<asp:Content ID="Content3" ContentPlaceHolderID="LinkPortion" runat="server">
    <!--table width="100%">
<tr>    
    <td align="left" style="width:24%"><asp:LinkButton CssClass="link" ID="lnkInbox" 
            runat="server" onclick="lnkInbox_Click">Inbox</asp:LinkButton></td>
    <td align="left" style="width:24%"><asp:LinkButton CssClass="link" 
            ID="lnkComposeMail" runat="server" onclick="lnkComposeMail_Click">Compose Mail</asp:LinkButton></td>
    <td align="left" style="width:24%"><asp:LinkButton CssClass="link" ID="lnkOutbox" 
            runat="server" onclick="lnkOutbox_Click">Outbox</asp:LinkButton></td>
    <td align="left" style="width:24%"><asp:LinkButton CssClass="link" ID="lnkSentMail" 
            runat="server" onclick="lnkSentMail_Click">Sent Mail</asp:LinkButton></td>
    <td align="right" style="width:4%"><asp:LinkButton CssClass="link" ID="lnkLogout" 
            runat="server" onclick="lnkLogout_Click">Logout</asp:LinkButton></td>
</tr>
</table-->
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <table width="100%">
        <tr>
            <td align="left" class="bbw">
                Select Processwise Task :
                <asp:DropDownList ID="ddTask" runat="server" AutoPostBack="True" OnSelectedIndexChanged="ddTask_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
            <td align="left">
                <div align="left" class="bbw" id="dispute" runat="server">
                </div>
            </td>
            <td align="right">
                <div align="right" class="bbw" id="balance" style=" color:White;" runat="server">
                </div>
            </td>
        </tr>
    </table>
    <asp:Repeater ID="Repeater1" runat="server">
        <HeaderTemplate>
            <table width="100%" border="0" align="center" cellpadding="2" cellspacing="2" class="bgg">
                <tr style="background-color: #507CD1; color: White;">
                    <th align="left">
                        Action
                    </th>
                    <th align="left" style="padding-left: 5pt;">
                        Book ID
                    </th>
                    <th align="left" style="padding-left: 5pt;">
                        Source File
                    </th>
                    <th align="left" style="padding-left: 5pt;">
                        Deadline
                    </th>
                    <th align="left" style="padding-left: 5pt;">
                        Comp. Date
                    </th>
                    <th align="left" style="padding-left: 5pt;">
                        Task
                    </th>
                    <th align="left" style="padding-left: 5pt;">
                        Unit Cost
                    </th>
                    <th align="left" style="padding-left: 5pt;">
                        No of Units
                    </th>
                    <th align="left" style="padding-left: 5pt;">
                        Assigned By
                    </th>
                    <th align="left" style="padding-left: 5pt;">
                        Status
                    </th>
                </tr>
        </HeaderTemplate>
        <ItemTemplate>
            <tr style="background-color: #003366;">
                <td class="bbw">
                    <%# SubmitTaskLink(Eval("AID").ToString(), Eval("BIdentityNo").ToString(), Eval("Status").ToString(), Eval("Task").ToString())%>
                </td>
                <td class="bbw">
                    <%# Eval("BIdentityNo").ToString()%>
                </td>
                <td class="bbw">
                    <%#DownloadLink(Eval("BIdentityNo").ToString(), Eval("Task").ToString(),Eval("Status").ToString())%>
                </td>
                <td class="bbw">
                    <%# Eval("DeadLine","{0:dd-M-yyyy}")%>
                </td>
                <td class="bbw">
                    <%# Eval("CompletionDate","{0:dd-M-yyyy}")%>
                </td>
                <td class="bbw">
                    <%# Eval("Task").ToString()%>
                </td>
                <td class="bbw">
                    <%# Eval("Cost","{0:f2}").ToString()%><span><font color='red'><sup>*</sup></font></span>
                </td>
                <td class="bbw">
                    <%# GetNullValue(Eval("Count").ToString())%>
                </td>
                <td class="bbw">
                    <%# Eval("AssignedBy")%>
                </td>
                <td class="bbw">
                    <%# ToolTip(Eval("Status").ToString(),Eval("Comments").ToString())%>
                </td>
            </tr>
        </ItemTemplate>
        <FooterTemplate>
            </table> <span><font color='red'>*</font><sub> All Tasks except Meta have amount per
                unit</sub></span>
        </FooterTemplate>
    </asp:Repeater>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="message"></asp:Label>
</asp:Content>

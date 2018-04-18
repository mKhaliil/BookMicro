<%@ Page Language="C#" MasterPageFile="~/MasterPages/AdminMaster_Hiring.Master" AutoEventWireup="true" Inherits="Outsourcing_System.Transactions" Title="Transactions Approval" Codebehind="Transactions.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%">
    <tr><td align="left">
        <asp:LinkButton ID="lnkPendingTransactions" runat="server" 
            onclick="lnkPendingTransactions_Click">Pending Transactions</asp:LinkButton>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
            SelectCommand="SELECT * FROM [Transactions]"></asp:SqlDataSource>
        </td><td align="right">
            <asp:LinkButton ID="lnkApprovedTransactions" 
                PostBackUrl="~/Transactions.aspx?type=app" runat="server" 
                onclick="lnkApprovedTransactions_Click">Approved Transactions</asp:LinkButton></td></tr>
    <tr>
    <td colspan="2">
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AutoGenerateColumns="False" DataKeyNames="WID,UID" DataSourceID="SqlDataSource1" 
            onselectedindexchanged="GridView1_SelectedIndexChanged" Width="100%">
        <RowStyle BackColor="#EFF3FB" />
            <Columns>
              <asp:CommandField ShowSelectButton="True" SelectText="Approve" />              
              <asp:BoundField DataField="UserName" HeaderText="User Name" SortExpression="UserName" HeaderStyle-HorizontalAlign="Left" />
              <asp:BoundField DataField="WithdrawAmount" HeaderText="Withdraw Amount" 
                  SortExpression="WithdrawAmount" HeaderStyle-HorizontalAlign="Left" />
              <asp:BoundField DataField="TransactionDate" HeaderText="Transaction Date" 
                  SortExpression="TransactionDate" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:dd-M-yyyy}" />  
              <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <a href="Transactions.aspx?wid=<%#Eval("WID").ToString() %>">Reject</a>
                </ItemTemplate>
              </asp:TemplateField>              
            </Columns>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="White" />

        </asp:GridView>
    </td>
    </tr>
    </table>
</asp:Content>

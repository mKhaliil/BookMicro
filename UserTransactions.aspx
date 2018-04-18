<%@ Page Language="C#" MasterPageFile="~/EditorMaster.Master" AutoEventWireup="true" Inherits="Outsourcing_System.UserTransactions" Title="My Transactions" Codebehind="UserTransactions.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <table width="100%">
    <tr><td align="left">
        <asp:LinkButton ID="lnkPendingTransactions" runat="server" 
            onclick="lnkPendingTransactions_Click">Pending Transactions</asp:LinkButton>
        <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
            ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
            SelectCommand="SELECT * FROM [Transactions]"></asp:SqlDataSource>
        </td>
        <td align="center">
            <asp:LinkButton ID="LinkButton1" 
                PostBackUrl="~/UserTransactions.aspx?type=app" runat="server" 
                onclick="lnkRejectedTransactions_Click">Rejected Transactions</asp:LinkButton></td>
        <td align="right">
            <asp:LinkButton ID="lnkApprovedTransactions" 
                PostBackUrl="~/UserTransactions.aspx?type=app" runat="server" 
                onclick="lnkApprovedTransactions_Click">Approved Transactions</asp:LinkButton></td></tr>
    <tr>
    <td colspan="3">
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AutoGenerateColumns="False" DataKeyNames="WID" DataSourceID="SqlDataSource1" 
             Width="100%">
        <RowStyle BackColor="#EFF3FB" />
            <Columns>            
              <asp:BoundField DataField="UserName" HeaderText="User Name" 
                    SortExpression="UserName" HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
              <asp:BoundField DataField="WithdrawAmount" HeaderText="Withdraw Amount" 
                  SortExpression="WithdrawAmount" HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
              <asp:BoundField DataField="TransactionDate" HeaderText="Transaction Date" 
                  SortExpression="TransactionDate" HeaderStyle-HorizontalAlign="Left" 
                    DataFormatString="{0:dd-M-yyyy}" >  
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
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

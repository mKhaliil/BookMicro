<%@ Page Language="C#" MasterPageFile="~/EditorMaster.Master" AutoEventWireup="true" Inherits="Outsourcing_System.AmountConfirmation" Title="Amount Confirmation" Codebehind="AmountConfirmation.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"></asp:SqlDataSource>

    <table width="100%">
    <tr>
    <td>
        <asp:GridView ID="GridView1" runat="server" AllowPaging="True" 
            AutoGenerateColumns="False" DataKeyNames="DID,AID" DataSourceID="SqlDataSource1" 
            onselectedindexchanged="GridView1_SelectedIndexChanged" Width="100%">
        <RowStyle BackColor="#EFF3FB" />
            <Columns>
              <asp:CommandField ShowSelectButton="True" SelectText="Accept" />              
              <asp:BoundField DataField="UserName" HeaderText="User Name" 
                    SortExpression="UserName" HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
              <asp:BoundField DataField="AcAmount" HeaderText="Actual Amount" 
                  SortExpression="WithdrawAmount" HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
                <asp:BoundField DataField="PpAmount" HeaderText="Proposed Amount" 
                  SortExpression="WithdrawAmount" HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
               <asp:BoundField DataField="Bonus" HeaderText="Bonus" 
                  SortExpression="Bonus" HeaderStyle-HorizontalAlign="Left" >
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
              <asp:BoundField DataField="DDate" HeaderText="Date" 
                  SortExpression="DDate" HeaderStyle-HorizontalAlign="Left" 
                    DataFormatString="{0:dd-M-yyyy}" >  
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
                <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <a href="DisputeDetail.aspx?aid=<%#Eval("AID").ToString() %>">Detail</a>
                </ItemTemplate>
              </asp:TemplateField> 
              <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <a href="AmountConfirmation.aspx?did=<%#Eval("DID").ToString() %>">Dispute</a>
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

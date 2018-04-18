<%@ Page ValidateRequest="false" Language="C#" MasterPageFile="~/MasterPages/AdminMaster_Hiring.Master" AutoEventWireup="true" Inherits="Outsourcing_System.Dispute" Title="Dispute Approval" Codebehind="Dispute.aspx.cs" %>

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
              <asp:CommandField ShowSelectButton="True" SelectText="Show" />              
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
              <asp:BoundField DataField="DDate" HeaderText="Date" 
                  SortExpression="DDate" HeaderStyle-HorizontalAlign="Left" 
                    DataFormatString="{0:dd-M-yyyy}" >  
<HeaderStyle HorizontalAlign="Left"></HeaderStyle>
                </asp:BoundField>
              <asp:TemplateField HeaderText="">
                <ItemTemplate>
                    <a href="DisputeDetail.aspx?did=<%#Eval("DID").ToString() %>&aid=<%#Eval("AID").ToString() %>">
                    Detail</a>
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
    <tr>
        <td>
        <asp:UpdatePanel runat="server" ID="uplDetail" UpdateMode="Conditional">
        <ContentTemplate>
            <asp:Panel runat="server" ID="pnlDetail" Visible="false">
                <table align="center">
                    <tr><td align="right" valign="top">Amount :</td><td align="left">
                        <asp:TextBox ID="txtAmount" runat="server" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtAmount" ErrorMessage="* Amount"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ErrorMessage="* Amount in Fiqures"  ControlToValidate="txtAmount" ValidationExpression="([0-9])+(\.)?([0-9])*"></asp:RegularExpressionValidator></td></tr>
                    <tr><td align="right" valign="top">Remarks :</td><td align="left"><asp:TextBox ID="txtRemarks" runat="server" TextMode="MultiLine" Rows="5" /></td></tr>
                    <tr><td align="right">Winner :&nbsp;</td><td align="left"><asp:DropDownList ID="ddWinner" runat="server"><asp:ListItem Value="User">User</asp:ListItem><asp:ListItem Value="Admin">Admin</asp:ListItem></asp:DropDownList></td></tr>
                    <tr><td align="right"></td><td align="left"><asp:Button ID="btnResolve" Text="Resolve" runat="server" onclick="btnResolve_Click" /></td></tr>                
                </table>
            </asp:Panel>  
        </ContentTemplate>
        </asp:UpdatePanel>      
        </td>
    </tr>
    </table>
</asp:Content>

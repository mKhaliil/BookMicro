<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true"
    CodeBehind="TestUserAccountDetails.aspx.cs" Inherits="Outsourcing_System.TestUserAccountDetails" %>

<%@ Register Src="UserControls/CommonControl/ucShowMessage.ascx" TagName="ucShowMessage" TagPrefix="uc1" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">

    <div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">
        <uc1:ucShowMessage ID="ucShowMessage1" runat="server" />
    </div>

    <%--<div id="pending" style="margin-left: 300px; float: left;" runat="server" />--%>
    <div id="balance" style="margin-left: 150px; float: left;" runat="server" />
    <div style="margin-left: 930px; margin-bottom: 5px;">
        <asp:Button ID="btnWithdraw" runat="server" Text="Withdraw Amount" Width="105px" CssClass="button" OnClick="btnWithdraw_Click" />
    </div>
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" ConnectionString="<%$ ConnectionStrings:ConnectionString %>"
        SelectCommand=""></asp:SqlDataSource>
    <asp:GridView ID="GridView1" runat="server" AllowPaging="True" AutoGenerateColumns="False"
        DataKeyNames="DID" ShowHeaderWhenEmpty="True" EmptyDataText="No data available in Accounts Grid."
        DataSourceID="SqlDataSource1" Width="100%"
        OnRowDataBound="GridView1_RowDataBound">
        <RowStyle BackColor="#EFF3FB" />
        <Columns>
            <asp:BoundField DataField="Date" HeaderText="Date" DataFormatString="{0:dd-M-yyyy}"
                SortExpression="Date" HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Description" HeaderText="Description" SortExpression="Description"
                HeaderStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Deposit" HeaderText="Deposit" ItemStyle-ForeColor="Green"
                SortExpression="Deposit" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:f2}" />
            <asp:BoundField DataField="Withdraw" HeaderText="Withdraw" ItemStyle-ForeColor="Red"
                SortExpression="Withdraw" HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:f2}" />
            <asp:BoundField DataField="Balance" HeaderText="Balance" SortExpression="Balance"
                HeaderStyle-HorizontalAlign="Left" DataFormatString="{0:f2}" />
        </Columns>
        <FooterStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <PagerStyle BackColor="#2461BF" ForeColor="White" HorizontalAlign="Center" />
        <SelectedRowStyle BackColor="#D1DDF1" Font-Bold="True" ForeColor="#333333" />
        <HeaderStyle BackColor="#507CD1" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#2461BF" />
        <AlternatingRowStyle BackColor="White" />
    </asp:GridView>
    <div id="divWithdrawAmount" runat="server" visible="false">
        <div style="display: block; left: 0; top: 0; position: fixed; width: 100%; height: 100%; padding: 200px; opacity: 0.7; background-color: Gray;">
        </div>
        <div style="position: fixed; left: 35%; top: 30%; border: solid 6px #294b96; border-radius: 15px; background-color: White; padding: 20px;">
            <table style="">
                <tr>
                    <td>Balance :
                    </td>
                    <td>
                        <asp:Label ID="lblBalance" runat="server" Text="0"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td align="left">Withdraw Amount:
                    </td>
                    <td>
                        <asp:RadioButtonList ID="rbtnlAmount" RepeatDirection="Horizontal"
                            runat="server" AutoPostBack="true"
                            OnSelectedIndexChanged="rbtnlAmount_SelectedIndexChanged">
                            <asp:ListItem Selected="True">Balance</asp:ListItem>
                            <asp:ListItem>Other Amount</asp:ListItem>
                        </asp:RadioButtonList>

                    </td>
                </tr>
                <tr>
                    <td align="right" colspan="2">
                        <asp:TextBox ID="tbxWithdrawAmount" runat="server" Style="border: 1px solid black; height: 20px; border-radius: 0" Width="120" Visible="false"></asp:TextBox>
                    </td>
                </tr>
            </table>
            <div style="margin-left: 188px; margin-top: 20px;">

                <asp:Button ID="btnWithdrawAmount" runat="server" CssClass="button" Text="Withdraw" Style="width: 100px" OnClick="btnWithdrawAmount_Click" />
                <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Cancel" OnClick="btnCancel_Click" />

            </div>
        </div>
    </div>
</asp:Content>

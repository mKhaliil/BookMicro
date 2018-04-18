<%@ Page Language="C#" MasterPageFile="~/MasterPages/AdminMaster_Hiring.Master" AutoEventWireup="true" CodeBehind="DisputeDetail.aspx.cs" Inherits="Outsourcing_System.DisputeDetail" Title="Dispute Detail" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LinkPortion" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:SqlDataSource ID="SqlDataSource1" runat="server" 
        ConnectionString="<%$ ConnectionStrings:ConnectionString %>" 
        SelectCommand="SELECT     [User].UserName, Book.BIdentityNo, Activity.Task, Activity.AssignedBy, Activity.AssigmentDate, Activity.DeadLine, Earnings.Remarks,Activity.AID FROM Book INNER JOIN Activity ON Book.BID = Activity.BID INNER JOIN  Earnings ON Activity.AID = Earnings.AID INNER JOIN [User] ON Activity.UID = [User].UID"></asp:SqlDataSource>
    <asp:DetailsView ID="DetailsView1" runat="server" Height="150px" Width="800px" 
        AutoGenerateRows="False" DataKeyNames="AID" DataSourceID="SqlDataSource1" 
        CellPadding="4" ForeColor="#333333" GridLines="None">
        <FooterStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <CommandRowStyle BackColor="#E2DED6" Font-Bold="True" />
        <RowStyle BackColor="#F7F6F3" ForeColor="#333333" />
        <FieldHeaderStyle BackColor="#E9ECF1" Font-Bold="True" />
        <PagerStyle BackColor="#284775" ForeColor="White" HorizontalAlign="Left" />
        <Fields>
            <asp:BoundField DataField="UserName" HeaderText="User Name" HeaderStyle-HorizontalAlign="Left" 
                SortExpression="UserName" ItemStyle-HorizontalAlign="Left" HeaderStyle-Width="140px" />
            <asp:BoundField DataField="BIdentityNo" HeaderText="Book ID" HeaderStyle-HorizontalAlign="Left" 
                SortExpression="BIdentityNo"  ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Task" HeaderText="Task" HeaderStyle-HorizontalAlign="Left"  
                SortExpression="Task"  ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="AssignedBy" HeaderText="Assigned By" HeaderStyle-HorizontalAlign="Left" 
                SortExpression="AssignedBy"  ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="AssigmentDate" HeaderText="Assigment Date" HeaderStyle-HorizontalAlign="Left" 
                SortExpression="AssigmentDate" DataFormatString="{0:dd-M-yyyy}"  ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="DeadLine" HeaderText="DeadLine" HeaderStyle-HorizontalAlign="Left" 
                SortExpression="DeadLine" DataFormatString="{0:dd-M-yyyy}"  ItemStyle-HorizontalAlign="Left" />
            <asp:BoundField DataField="Remarks" HeaderText="Remarks" HeaderStyle-HorizontalAlign="Left" 
                SortExpression="Remarks"  ItemStyle-HorizontalAlign="Left" />
        </Fields>
        <HeaderStyle BackColor="#5D7B9D" Font-Bold="True" ForeColor="White" />
        <EditRowStyle BackColor="#999999" />
        <AlternatingRowStyle BackColor="White" ForeColor="#284775" />
    </asp:DetailsView>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

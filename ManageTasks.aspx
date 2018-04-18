<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true"
    CodeBehind="ManageTasks.aspx.cs" Inherits="Outsourcing_System.ManageTasks" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainHeadContents" runat="server">
    <script type="text/javascript">

        $(function () {
            var gv = $('#<%=gvTaskPriorities.ClientID%>');
            var thead = $('<thead/>');
            thead.append(gv.find('tr:eq(0)'));
            gv.append(thead);
            gv.dataTable({
                "sPaginationType": "full_numbers",
                "iDisplayLength": 10,
                "aaSorting": [],
                "bDestroy": true,
                "bRetrieve": true,
                'bProcessing': true,
                'bServerSide': false
            });
        });

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainBodyContents" runat="server">
    <div id="divArchiveTasks" style="margin-top: 1px; margin-bottom: 35px">
        Manage Task Priorities
    </div>
    <div style="margin-left: auto; margin-right: auto; width: 85%;">
        <asp:GridView ID="gvTaskPriorities" runat="server" Style="background-color: #E9E9E9;
            margin-top: 15px;" AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" EmptyDataText='No Task available in archive Tasks.'
            DataKeyNames="bid" Width="100%" OnRowDataBound="gvTaskPriorities_RowDataBound"
            OnRowCommand="gvTaskPriorities_RowCommand">
            <Columns>
                <asp:TemplateField HeaderText="Sr. No#" HeaderStyle-Width="4%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSrNo" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Book Id" HeaderStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTestName" Text='<%# Eval("BookId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--   <asp:TemplateField HeaderText="Task Type" HeaderStyle-Width="12%">
                            <ItemTemplate>
                                <asp:Label runat="server" ID="lblIssueCategory" Text='<%# Eval("TaskType") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                <asp:TemplateField HeaderText="Status" HeaderStyle-Width="10%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("TaskStatus") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Priority" HeaderStyle-Width="7%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPriority" Text='<%# Eval("Priority") %>' />
                        <asp:DropDownList ID="ddlPriority" runat="server" Visible="false" OnClick="btnPriority_Click">
                            <asp:ListItem>Normal</asp:ListItem>
                            <asp:ListItem>High</asp:ListItem>
                            <asp:ListItem>Urgent</asp:ListItem>
                        </asp:DropDownList>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Edit" HeaderStyle-Width="9%">
                    <ItemTemplate>
                        <asp:Button ID="btnEdit" runat="server" CssClass="formatGvBtn" Text="Edit" CommandName="editBtnClick"
                            CommandArgument='<%# Container.DataItemIndex%>' />
                        <asp:Button ID="btnSave" runat="server" CssClass="formatGvBtn" Text="Save" CommandName="saveBtnClick"
                            CommandArgument='<%# Container.DataItemIndex%>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="gridHeader" />
            <RowStyle HorizontalAlign="Center"></RowStyle>
        </asp:GridView>
    </div>
</asp:Content>

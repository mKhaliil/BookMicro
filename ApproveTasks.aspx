<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true" CodeBehind="ApproveTasks.aspx.cs" Inherits="Outsourcing_System.ApproveTasks" %>

<%@ Register Src="UserControls/CommonControl/ucShowMessage.ascx" TagName="ucShowMessage" TagPrefix="uc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="mainHeadContents" runat="server">
    <script type="text/javascript">

        $(document).ready(function () {

            //iterate through each textboxes and add keyup
            //handler to trigger sum event
            $(".addAmount").each(function () {

                $(this).keyup(function () {
                    calculateSum();
                });
            });

        });

        function calculateSum() {

            var sum = 0;
            //iterate through each textboxes and add the values
            $(".addAmount").each(function () {

                //add only if the value is number
                if (!isNaN(this.value) && this.value.length != 0) {
                    sum += parseFloat(this.value);
                }

            });
            //.toFixed() method will roundoff the final sum to 2 decimal places
            $('#<%=lblTotal.ClientID%>').html("AU $ " + sum.toFixed(2));
        }

        function ShowTransationDialog(cost, aid) {

            $('#<%=tbxAmount.ClientID%>').val(cost);
            $('#<%=hfAId.ClientID%>').val(aid);
            calculateSum();

            $("#divReleaseAmount").dialog({
                appendTo: "#dialogAfterMe",
                title: "Release Amount",
                height: 350,
                width: 500,
                position: "center",
                resizable: false,
                modal: true
            });
        };

        function CloseResultDialog() {
            $("#divReleaseAmount").dialog('close');
        }

        $(document).ready(function () {

            //Dropdownlist Selectedchange event
            $('#<%=ddlTransactionType.ClientID%>').change(function () {

                //alert('Dropdownlist Selectedchange event');

                // Get Dropdownlist seleted item text
                var value = $("#<%=ddlTransactionType.ClientID%> option:selected").text();

                //// Get Dropdownlist selected item value
                //$("#lblid").text($("#ddlCountry").val());

                if (value == "Other")
                    $('#<%=tbxOther.ClientID%>').css("display", "block");

                else if (value == "Bank Account")
                    $('#<%=tbxOther.ClientID%>').css("display", "none");
            });
        });

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="mainBodyContents" runat="server">
    
    <asp:HiddenField ID="hfAId" runat="server" />

    <div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">
        <uc1:ucShowMessage ID="ucShowMessage1" runat="server" />
    </div>

    <div style="width: 97%; margin-left: auto; margin-top: 3%; margin-bottom: 3%; margin-right: auto;">
       
          <table width="100%">
            <tr>
                <td><h3>
            <asp:Label runat="server" Style="float: left; margin-bottom: 2%; color: #003366; font-size: 26px"
                ID="Label3">Approved Tasks</asp:Label></h3></td>
                <td align="right"><asp:Button CssClass="button" ID="btnExportApprovedTasks" Width="90px" Text="Export Data" runat="server"
                                    OnClick="btnExportApprovedTasks_Click" /></td>
            </tr>
        </table> 

        <asp:GridView ID="gvApprovedTask" runat="server" Style="background-color: #E9E9E9"
            AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" EmptyDataText='No Task available in Approved Tasks.'
            DataKeyNames="TaskCompletionDate, TaskAssigmentDate, AID, Priority"
            Width="100%" OnRowDataBound="gvApprovedTask_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="S/No" HeaderStyle-Width="4%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSrNo" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Book Id" HeaderStyle-Width="7%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblBookId" Text='<%# Eval("BookId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Complexity" HeaderStyle-Width="6%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblComplexity" Text='<%# Eval("Complexity") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                  <asp:TemplateField HeaderText="Total Pages" HeaderStyle-Width="6%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTotalPages" Text='<%# Eval("PageCount") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Task Name" HeaderStyle-Width="9%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTastName" Text='<%# Eval("TaskName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Resource Name" HeaderStyle-Width="16%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblResourceName" Text='<%# Eval("FullName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Description" HeaderStyle-Width="25%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDescription" Text='<%# Eval("Description") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
               
                <asp:TemplateField HeaderText="Start Date" HeaderStyle-Width="8%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblStartDate" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="End Date" HeaderStyle-Width="8%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblCompletionDate" />
                    </ItemTemplate>
                </asp:TemplateField>
               <%--  <asp:TemplateField HeaderText="Status" HeaderStyle-Width="8%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("TaskStatus") %>' />
                    </ItemTemplate>
                </asp:TemplateField>--%>
                 <asp:TemplateField HeaderText="Priority" HeaderStyle-Width="8%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPriority" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Time Spent" HeaderStyle-Width="8%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTimeSpent" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Task Amount" HeaderStyle-Width="7%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPrice" Text='<%# Eval("TaskCost") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
               <%-- <asp:TemplateField HeaderStyle-Width="9%">
                    <ItemTemplate>
                        <div style="text-align: center; padding: 2px;">
                            <input id="btnReleaseAmount" type="button" class="button" style="width: 90%;" value="Release Amount"
                                onclick="<%# "ShowTransationDialog(" + Eval("TaskCost") + "," + Eval("AID") + ");" %>" />
                        </div>
                    </ItemTemplate>
                </asp:TemplateField>--%>
            </Columns>
            <HeaderStyle CssClass="gridHeader" />
            <RowStyle HorizontalAlign="Center"></RowStyle>
        </asp:GridView>
    </div>

    <div style="width: 97%; margin-left: auto; margin-top: 3%; margin-bottom: 3%; margin-right: auto;">
         <table width="100%">
            <tr>
                <td><h3>
            <asp:Label runat="server" Style="float: left; margin-bottom: 2%; color: #003366; font-size: 26px"
                ID="Label1">Working Tasks</asp:Label></h3></td>
                <td align="right"><asp:Button CssClass="button" ID="btnExportWorkingTasks"  Width="90px" Text="Export Data" runat="server"
                                    OnClick="btnExportWorkingTasks_Click" /></td>
            </tr>
        </table> 

        <asp:GridView ID="gvInProgressTasks" runat="server" Style="background-color: #E9E9E9"
            AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" EmptyDataText='No Task available in working Tasks.'
            DataKeyNames="TaskAssigmentDate, TaskDeadLine, Priority"
            Width="100%" OnRowDataBound="gvInProgressTasks_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="S/No" HeaderStyle-Width="4%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSrNo" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Book Id" HeaderStyle-Width="5%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblBookId" Text='<%# Eval("BookId") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Complexity" HeaderStyle-Width="6%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblComplexity" Text='<%# Eval("Complexity") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Task Name" HeaderStyle-Width="8%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTastName" Text='<%# Eval("TaskName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Resource Name" HeaderStyle-Width="12%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblResourceName" Text='<%# Eval("FullName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Starting Date" HeaderStyle-Width="8%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblStartingDate" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Task DeadLine" HeaderStyle-Width="8%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDeadLine" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Priority" HeaderStyle-Width="6%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPriority" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Total Pages" HeaderStyle-Width="6%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTotalPages" Text='<%# Eval("PageCount") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Viewed Pages" HeaderStyle-Width="7%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPagesDone" Text='<%# Eval("PageViewed") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                
                 <asp:TemplateField HeaderText="Total Injected Mistakes" HeaderStyle-Width="8%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTotalMistakes" Text='<%# Eval("TotalInjectedMistakes") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Detected Injected Mistakes" HeaderStyle-Width="9%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblDetectedMistakes" Text='<%# Eval("DetectedInjectedMistakes") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Detected Other Mistakes" HeaderStyle-Width="8%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblOtherMistakes" Text='<%# Eval("DetectedOtherMistakes") %>' />
                    </ItemTemplate>
                </asp:TemplateField>

               <%-- <asp:TemplateField HeaderText="Status" HeaderStyle-Width="6%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("TaskStatus") %>' />
                    </ItemTemplate>
                </asp:TemplateField>--%>
               
                <asp:TemplateField HeaderText="Task Amount" HeaderStyle-Width="7%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblPrice" Text='<%# Eval("TaskCost") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="gridHeader" />
            <RowStyle HorizontalAlign="Center"></RowStyle>
        </asp:GridView>
    </div>

    <div id="dialogAfterMe">
    </div>

    <div id="divReleaseAmount" style="width: 100%; height: 50px; margin-top: 3.7%; display: none;">
        <table cellspacing="10" width="98%">
            <tr>
                <td width="40%">
                    <strong>Transaction Id: </strong>
                </td>
                <td width="60%">
                    <asp:TextBox ID="tbxTransationId" runat="server" Style="border: 1px solid black; overflow: auto; border-radius: 0px; height: 25px; width: 100%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td style="vertical-align: top">
                    <strong>Transaction Type: </strong>
                </td>
                <td>
                    <asp:DropDownList ID="ddlTransactionType" runat="server" Style="border: 1px solid black; width: 100%">
                        <asp:ListItem>Bank Account</asp:ListItem>
                        <asp:ListItem>Other</asp:ListItem>
                    </asp:DropDownList>
                    <input type="text" id="tbxOther" runat="server" style="display: none; margin-top: 2%; border: 1px solid black; width: 100%" />
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Task Amount: </strong>
                </td>
                <td>
                    <asp:TextBox ID="tbxAmount" CssClass="addAmount" runat="server" Style="border: 1px solid black; overflow: auto; border-radius: 0px; height: 25px; width: 100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Bonus: </strong>
                </td>
                <td>
                    <asp:TextBox ID="tbxBonus" runat="server" CssClass="addAmount" Style="border: 1px solid black; overflow: auto; border-radius: 0px; height: 25px; width: 100px"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td>
                    <strong>Total: </strong>
                </td>
                <td>
                    <%--<asp:TextBox ID="tbxTotal" runat="server" Style="border: 1px solid black; overflow: auto; border-radius: 0px; height: 25px; width: 100px"></asp:TextBox>--%>
                    <asp:Label ID="lblTotal" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td></td>
                <td>
                    <asp:Button ID="btnDone" runat="server" Text="Done" Width="60" CssClass="button" OnClick="btnDone_Click" />
                    <input type="button" value="Close" onclick="CloseResultDialog();" width="60" class="button" />
                </td>
            </tr>
        </table>
    </div>
</asp:Content>

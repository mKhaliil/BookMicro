<%@ Page Title="" Language="C#" MasterPageFile="AdminMaster.Master" AutoEventWireup="true" CodeBehind="ApproveTest.aspx.cs" Inherits="Outsourcing_System.ApproveTest" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
   <%-- <link href="Styles/jquery-ui-1.8.7.custom.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.4.3.min.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.8.7.custom.min.js" type="text/javascript"></script>--%>

<%--    <link href="Styles/demo_table.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery.dataTables.columnFilter.js" type="text/javascript"></script>
    <script src="scripts/jquery.dataTables.min.js" type="text/javascript"></script>
--%>
    <script type="text/javascript">
        $(function () {
            var gv = $('#<%=gvPendingTests.ClientID%>');
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

        $(function () {
            var gv = $('#<%=gvPassedUsers.ClientID%>');
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

        $(function () {
            var gv = $('#<%=gvFailedUsers.ClientID%>');
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

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">    
    <div id="mid" align="center">
        <div id="profile">
            <div style="text-align: center; margin-bottom: 4px; margin-left: 130px;">
                <asp:Label ID="lblErrorMsg" Style="font-family: Verdana; font-size: large; font-weight: normal;"
                    runat="server" Visible="false"></asp:Label>
            </div>
            <div id="DivError" runat="server" visible="false">
                <div style="font-size: 10pt; color: Green; text-align: left; background-color: #ffcdd6;
                    margin-left: 400px; margin-right: 400px; border: 1px solid red; padding: 5px;
                    font-family: Sans-Serif;">
                    <table>
                        <tr>
                            <td rowspan="2">
                                <img src="img/red-error.gif" />
                            </td>
                            <td>
                                <asp:Label ID="lblError" runat="server" CssClass="ErrorMsgText"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="spacer" style="padding-top: 2px">
                </div>
            </div>
            <div id="divSuccess" runat="server" visible="false">
                <div style="font-size: 10pt; color: Green; text-align: left; background-color: #B3FFB3;
                    margin-left: 480px; margin-right: 470px; border: 1px solid Green; padding: 5px;
                    font-family: Sans-Serif;">
                    <table>
                        <tr>
                            <td rowspan="2">
                                <img src="img/green-ok.gif" />
                            </td>
                            <td>
                                <asp:Label ID="lblSuccess" runat="server" CssClass="ErrorMsgText"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="spacer" style="padding-top: 2px">
                </div>
            </div>
                <div id="divTestResult" style="padding: 25px;">
                   <%-- <div id="Div2" style="font-size: 14pt; color: Green; text-align: left; color: #FFFFFF;
                        background-color: #CCCCCC; width: 100%; text-align: center">
                        Tests for Pending Confirmation
                    </div>--%>

                    <div style="width:105%;margin-left:3px">
                     <h3>
                        <asp:Label runat="server" Style="float:left; margin-bottom:2%; color: #003366; font-size: 20px"
                            ID="Label2">Tests for Pending Confirmation</asp:Label></h3>
                  <%--  <div>
                        &nbsp;</div>--%>
                    <asp:GridView ID="gvPendingTests" runat="server" Style="margin-left: 1px; background-color: #E9E9E9" CssClass="display"

                        AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" EmptyDataText='No data available in Task Manager.'
                        DataKeyNames="id" Width="95%">
                        <Columns>
                            <asp:TemplateField HeaderText="User Name" HeaderStyle-Width="18%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblUser" Text='<%# Eval("uid") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Test Type" HeaderStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestTypw" Text='<%# Eval("testtype") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Test Name" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestName" Text='<%# Eval("testName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Start Time" HeaderStyle-Width="14%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStartTime" Text='<%# Eval("startTime") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="End Time" HeaderStyle-Width="14%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEndTime" Text='<%# Eval("endTiem") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="status" HeaderStyle-Width="13%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("status") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <div style="text-align: center; padding: 2px;">
                                        <asp:Button ID="btnDownloadTest" runat="server" CssClass="button" Text="Download" Width="80px" 
                                            OnClick="btnDownloadTest_Click" />
                                        <asp:Button ID="btnpassTest" runat="server" CssClass="button" Width="80px" Text="Passed" OnClick="btnApproveTest_Click" />
                                        <asp:Button ID="btnFailTest" runat="server" CssClass="button" Width="80px" Text="Failed" OnClick="btnFailTest_Click" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gridHeader" />
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                    </asp:GridView>
                    <div>
                        &nbsp;
                    </div>
                    </div>
                 <%--   <br />
                    <br />--%>
                  <%--  <div id="hd" style="font-size: 14pt; color: Green; text-align: left; color: #FFFFFF;
                        background-color: #CCCCCC; width: 100%; text-align: center">
                        PASSED USERS
                    </div>--%>

                    <div style="width:105%;margin-left:3px;margin-top:50px">
                     <h3>
                        <asp:Label runat="server" Style="float:left; margin-bottom:2%; color: #003366; font-size: 20px"
                            ID="Label1">PASSED USERS</asp:Label></h3>
                    <%--<div>
                        &nbsp;</div>--%>
                    <asp:GridView ID="gvPassedUsers" runat="server" Style="margin-left: 1px; background-color: #E9E9E9"  CssClass="display"
                        AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" EmptyDataText='No data available in Task Manager.'
                        Width="95%" OnRowDataBound="gvPassedUsers_RowDataBound" DataKeyNames="TotalScore,ObtainedScore,FullName,Email,ProfileName,Password,verification">
                        <Columns>
                            <asp:TemplateField HeaderText="S/No" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSrNo" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-Width="13%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblName" Text='<%# Eval("FullName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEmail" Text='<%# Eval("Email") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Test Date" HeaderStyle-Width="16%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestDate" Text='<%# Eval("TestDate") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Test Name" HeaderStyle-Width="7%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestName" Text='<%# Eval("testName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Score" HeaderStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPassedScore" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("Status") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TestAttempts" HeaderStyle-Width="9%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestAttempts" Text='<%# Eval("TestAttempts") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IP Address" HeaderStyle-Width="10%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblIpAddress" Text='<%# Eval("IPAddress") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="5%" HeaderText="Profile">
                                <ItemTemplate>
                                    <asp:LinkButton ID="lbtnView" runat="server" OnClick="lbtnView_Click">View</asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderStyle-Width="4%">
                                <ItemTemplate>
                                    <div style="text-align: center; padding: 2px;">
                                        <asp:Button ID="btnApproveProfile" runat="server" CssClass="button" Text="Approve" Width="80px"
                                            OnClick="btnApproveProfile_Click" />
                                    </div>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                        <HeaderStyle CssClass="gridHeader" />
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                    </asp:GridView>
                    <div>
                        &nbsp;
                    </div>
                    </div>

                    
                <div style="width:105%;margin-left:3px;margin-top:50px;margin-bottom:100px;">
                    <h3>
                        <asp:Label runat="server" Style="float:left; margin-bottom:2%; color: #003366; font-size: 20px"
                            ID="Label3">FAILED USERS</asp:Label></h3>
                   <%-- <div id="Div1" style="font-size: 14pt; color: Green; text-align: left; color: #FFFFFF;
                        background-color: #CCCCCC; width: 100%; text-align: center">
                        FAILED USERS
                    </div>--%>
                   <%-- <div>
                        &nbsp;</div>--%>
                    <asp:GridView ID="gvFailedUsers" runat="server" Style="margin-left: 1px; background-color: #E9E9E9"  CssClass="display"
                        AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" EmptyDataText='No data available in failed gridview.'
                        Width="95%" OnRowDataBound="gvFailedUsers_RowDataBound" DataKeyNames="TotalScore,ObtainedScore,FullName">
                        <Columns>
                            <asp:TemplateField HeaderText="S/No" HeaderStyle-Width="5%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblSrNo" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Name" HeaderStyle-Width="15%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblName" Text='<%# Eval("FullName") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email" HeaderStyle-Width="20%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblEmail" Text='<%# Eval("Email") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Test Date" HeaderStyle-Width="14%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestDate" Text='<%# Eval("TestDate") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--  <asp:TemplateField HeaderText="Test Type" HeaderStyle-Width="7%">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="lblTestType" Text='<%# Eval("TestType") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                            <asp:TemplateField HeaderText="Score" HeaderStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblPassedScore" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Status" HeaderStyle-Width="6%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblStatus" Text='<%# Eval("Status") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="TestAttempts" HeaderStyle-Width="8%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblTestAttempts" Text='<%# Eval("TestAttempts") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="IP Address" HeaderStyle-Width="12%">
                                <ItemTemplate>
                                    <asp:Label runat="server" ID="lblIpAddress" Text='<%# Eval("IPAddress") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <%--  <asp:TemplateField HeaderStyle-Width="6%" HeaderText="Profile">
                        <ItemTemplate>
                            <asp:LinkButton ID="lbtnView" runat="server" OnClick="lbtnView_Click">View</asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderStyle-Width="5%">
                        <ItemTemplate>
                            <div style="text-align: center; padding: 2px;">
                                <asp:Button ID="btnApproveProfile" runat="server" CssClass="button" Text="Approve"
                                    OnClick="btnApproveProfile_Click" />
                            </div>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                        </Columns>
                        <HeaderStyle CssClass="gridHeader" />
                        <RowStyle HorizontalAlign="Center"></RowStyle>
                    </asp:GridView>
                    <div>
                        &nbsp;
                    </div>
                    </div>
                    <div id="divViewUserDetails" runat="server" visible="false">
                        <div style="display: block; left: 0; top: 0; position: fixed; width: 100%; height: 100%;
                            padding: 200px; opacity: 0.7; background-color: Gray;">
                        </div>
                        <div style="position: fixed; left: 35%; top: 8%; border: solid 6px #2a4f96; border-radius: 15px;
                            background-color: White; padding: 20px;">
                            <div style="width: 490px; height: 450px; background-color: White;">
                                <div id="img">
                                    <%--<img src="img/765-default-avatar.png" id="" width="100" height="100" align="left" />--%>
                                    <asp:Image ID="imgUserProfile" Width="100" Height="100" Style="margin-left: 12px;
                                        margin-bottom: 20px" align="left" runat="server" /></div>
                                <%-- <div style="overflow:auto"> --%>
                                <p style="text-align: left">
                                    <strong style="margin-left: 12px">Full Name: </strong>
                                    <asp:Label ID="lblFullName" Style="border: 1px solid black; margin-left: 76px;" runat="server"></asp:Label>
                                </p>
                                <p style="text-align: left">
                                    <strong style="margin-left: 12px">Email: </strong>
                                    <asp:Label ID="lblEmail" Enabled="false" Style="border: 1px solid black; margin-left: 108px;"
                                        runat="server"></asp:Label>
                                </p>
                                <p style="text-align: left">
                                    <strong style="margin-left: 12px">CNIC NO: </strong>
                                    <asp:Label ID="lblCnicNo" Enabled="false" Style="border: 1px solid black; margin-left: 87px;"
                                        runat="server"></asp:Label>
                                </p>
                                <p style="text-align: left">
                                    <strong style="margin-left: 12px">Account Number: </strong>
                                    <asp:Label ID="lblAccountNumber" Style="border: 1px solid black; margin-left: 28px;"
                                        runat="server"></asp:Label>
                                </p>
                                <p style="text-align: left">
                                    <strong style="margin-left: 12px">Mobile Number: </strong>
                                    <asp:Label ID="tbxMobileNumber" Style="border: 1px solid black; margin-left: 40px;"
                                        runat="server"></asp:Label>
                                </p>
                                <p style="text-align: left">
                                    <strong style="margin-left: 12px">Education: </strong>
                                    <asp:Label ID="lblEducation" Style="border: 1px solid black; margin-left: 76px;"
                                        runat="server"></asp:Label>
                                </p>
                                <p style="text-align: left">
                                    <strong style="margin-left: 12px">Experience: </strong>
                                    <asp:Label ID="tbxExperience" Style="border: 1px solid black; margin-left: 68px;"
                                        runat="server"></asp:Label>
                                </p>
                                <p style="text-align: left">
                                    <strong style="margin-left: 12px">Description: </strong>
                                    <asp:TextBox ID="lblDescription" Style="border: 1px solid black; margin-left: 169px;
                                        overflow: auto; border-radius: 0px" runat="server" TextMode="MultiLine" Height="81px"
                                        Width="300px"></asp:TextBox>
                                    <%-- <asp:Label ID="lblDescription" Style="border: 1px solid black; margin-left: 68px;
                            " runat="server" ></asp:Label>--%>
                                </p>
                                <%--  <h1>
                        <strong>
                            <asp:Label ID="lblUserName" runat="server"></asp:Label>Aamir</strong></h1>--%>
                                <asp:Button ID="btnCanceldialogue" runat="server" Text="Close" Width="60" CssClass="button"
                                    Style="margin-left: -90x;" OnClick="btnCancel_Click" />
                            </div>
                        </div>
                    </div>
                    <asp:HiddenField ID="hfAmountSelected" runat="server" />
                    <div id="divReleaseAmount" runat="server" visible="false">
                        <div style="display: block; left: 0; top: 0; position: fixed; width: 100%; height: 100%;
                            padding: 200px; opacity: 0.7; background-color: Gray;">
                        </div>
                        <div style="position: fixed; left: 35%; top: 25%; border: solid 6px #2a4f96; border-radius: 15px;
                            background-color: White; padding: 20px;">
                            <div style="width: 370px; height: 180px; background-color: White;">
                                <p style="text-align: left">
                                    <strong style="margin-left: 12px">Transaction Id: </strong>
                                    <asp:TextBox ID="tbxTransationId" runat="server" Style="border: 1px solid black;
                                        margin-left: 29px; overflow: auto; border-radius: 0px; height: 25px; width: 200px"></asp:TextBox>
                                </p>
                                <p style="text-align: left">
                                    <strong style="margin-left: 12px">Transaction Type: </strong>
                                    <%--<asp:Label ID="lblTransactionType" Style="margin-left: 8.5px;" runat="server" Text="Bank Account"></asp:Label>--%>
                                    <asp:DropDownList ID="ddlTransactionType" runat="server" Style="border: 1px solid black;
                                        margin-left: 10px; width: 150px" AutoPostBack="true" OnSelectedIndexChanged="ddlTransactionType_SelectedIndexChanged">
                                        <asp:ListItem>Bank Account</asp:ListItem>
                                        <asp:ListItem>Other</asp:ListItem>
                                    </asp:DropDownList>
                                    <asp:TextBox ID="tbxOther" runat="server" Style="border: 1px solid black; margin-left: 155px;
                                        margin-top: 13px; overflow: auto; border-radius: 0px; height: 25px; width: 200px"
                                        Visible="false"></asp:TextBox>
                                </p>
                                <p style="text-align: left">
                                    <strong style="margin-left: 12px">Amount: </strong>
                                    <asp:TextBox ID="tbxAmount" runat="server" Style="border: 1px solid black; margin-left: 77px;
                                        overflow: auto; border-radius: 0px; height: 25px; width: 200px"></asp:TextBox>
                                </p>
                                <p style="float: left; margin-left: 230px">
                                    <asp:Button ID="btnDone" runat="server" Text="Done" Width="60" CssClass="button"
                                        Style="margin-left: -90x;" OnClick="btnDone_Click" />
                                    <asp:Button ID="btnClosedivReleaseAmount" runat="server" Text="Close" Width="60"
                                        CssClass="button" OnClick="btnClosedivReleaseAmount_Click" />
                                </p>
                            </div>
                        </div>
                    </div>
                </div>
        </div>
    </div>
</asp:Content>

<%@ Page Language="C#" MasterPageFile="AdminMaster.Master" AutoEventWireup="true"
    Inherits="AdminPanel" Title="Untitled Page" CodeBehind="AdminPanel.aspx.cs" %>

<%@ Register src="UserControls/CommonControl/ucShowMessage.ascx" tagname="ucShowMessage" tagprefix="uc1" %>


<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="Server">
    <style type="text/css">
        .highlightRow
        {
            background-color: #9999FF;
        }
    </style>
    <%--<script type="text/javascript" src="http://code.jquery.com/jquery-1.11.0.min.js"></script>--%>
 <script type="text/javascript">
     
     //function highlightSelectedRow() {
     //    $('#tblRepeator .trclass').addClass("highlightRow").siblings().removeClass("highlightRow");
     //}

     $(function () {
         $(".trclass").click(function () {
             $(this).addClass("highlightRow").siblings().removeClass("highlightRow");
         });
     });
 </script>
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
    <asp:Panel ID="pnlAdminSearch" runat="server" DefaultButton="btnSearch1">
               
     <div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">
        <uc1:ucShowMessage ID="ucShowMessage1" runat="server" />
    </div>
    <div style="width:40%; height:10%; margin-top:1%; margin-bottom:1%; padding-left:1%; display:none;">
        <label>Insert records in WorkMeter Db : </label>
        <asp:Button ID="btnsynDb" Text="Synchronize Db" OnClick="btnsynDb_Click" style="position:relative; display:inline-block; margin-left:5%; width:22%; left: 0px;" runat="server"/>
    </div>
    <table width="100%">
        <tr>
            <td>Processwise Task :
            </td>
            <td align="left" class="normaltext">
                <asp:DropDownList ID="ddProcess" runat="server" CssClass="normaltext">
                </asp:DropDownList>
            </td>
            <td>Select Statuswise Task :
            </td>
            <td align="left" class="normaltext">
                <asp:DropDownList ID="ddStatus" runat="server" CssClass="normaltext">
                    <asp:ListItem Value="All">All</asp:ListItem>
                    <asp:ListItem>Unassigned</asp:ListItem>
                    <%--<asp:ListItem>Assigned</asp:ListItem>--%>
                    <asp:ListItem Selected="True">Working</asp:ListItem>
                    <asp:ListItem>Pending Confirmation</asp:ListItem>
                    <asp:ListItem>Approved</asp:ListItem>
                    <asp:ListItem>In Process</asp:ListItem>
                </asp:DropDownList>
            </td>
            <td>Search By Book ID :
            </td>
            <td>
                <div class="sfb">
                    <div class="stb">
                        <asp:TextBox ID="txtSearch" runat="server" Style="border: 1px solid #003366; margin-left: 70px; height: 20px; width: 190px; border-radius: 2px"></asp:TextBox>
                    </div>
                </div>
            </td>
            <td>
                <span style="padding-left: 10px;">
                    <asp:ImageButton ID="btnSearch1" runat="server" OnClick="btnSearch_Click" ImageUrl="img/sb.gif" />
            </td>
        </tr>
        <tr>
            <td colspan="7" align="center">
                <asp:Repeater ID="Repeater1" runat="server">
                    <HeaderTemplate>
                        <table id="tblRepeator" width="99%" border="0" align="center" cellpadding="2" cellspacing="2" class="bgg">
                            <tr style="background-color: #507CD1; color: White;">
                                <th align="left">&nbsp;
                                </th>
                                <th height="30" align="center">Book ID
                                </th>
                                <th align="center">Download
                                </th>
                                <th align="center">Assign Date
                                </th>
                                <th align="center">Assigned To
                                </th>
                                <th align="center">Task
                                </th>
                                <th align="center">Total Pages
                                </th>
                                <th align="center">Viewed Pages
                                </th>
                                <th align="center">Deadline
                                </th>
                                <th align="center">Comp. Date
                                </th>
                                <th align="center">Time Spent
                                </th>
                                <th align="center">Cost
                                </th>
                                <th align="center">Status
                                </th>
                                <th align="left" style="padding-left: 5pt;" colspan="2">&nbsp;
                                </th>
                            </tr>
                    </HeaderTemplate>
                    <ItemTemplate>
                        
                        <tr class="trclass" id="trID" runat="server" >
                            <td class="bbw" align="center">
                                <%#AssignTaskLink(Eval("Status").ToString(), Eval("BID").ToString(), Eval("Task").ToString(), Eval("AID").ToString(), Eval("PAGES").ToString())%>
                            </td>
                            <td class="bbw" align="center">
                                <%# Eval("BIdentityNo")%>
                            </td>
                            <td align="center">
                                <%--<img src="img/zipImage.jpg" />--%>
                                <%#DownloadLink(Eval("BIdentityNo").ToString(), Eval("Task").ToString(), Eval("CompletionDate", "{0:dd-M-yyyy}"), Eval("Status").ToString())%>
                            </td>
                            <td class="bbw" align="center">
                                <%# Eval("[UpDate]", "{0:dd-M-yyyy}")%>
                            </td>
                            <td class="bbw" align="center">
                                <%# Eval("[AssignedTo]")%>
                            </td>
                            <td class="bbw" align="center">
                                <%# Eval("Task")%>
                            </td>
                            <td class="bbw" align="center">
                                <%# GetNullValue(Eval("PageCount").ToString())%>
                            </td>
                            <td class="bbw" align="center">
                                <%# GetNullValue(Eval("PageViewed").ToString())%>
                            </td>
                            <td class="bbw" align="center">
                                <%# Eval("DeadLine", "{0:dd-M-yyyy}")%>
                            </td>
                            <td class="bbw" align="center">
                                <%# Eval("CompletionDate", "{0:dd-M-yyyy}")%>
                            </td>
                            <td class="bbw" align="center">
                                <%# Eval("TimeSpent")%>
                            </td>
                            <td class="bbw" align="center">
                                <%# Eval("Cost","{0:f2}").ToString()%><span><font color='red'><sup>*</sup></font></span>
                            </td>
                            <td class="bbw" align="center">
                                <%# LinkCreation(Eval("Task").ToString(), Eval("Status").ToString(), Eval("AID").ToString(), "edit")%>
                            </td>
                            <td class="bbw" align="center">
                                <%# LinkCreation(Eval("Task").ToString(), Eval("Status").ToString(), Eval("AID").ToString(), "cfm")%>
                            </td>
                            <td class="bbw" align="center">
                                <%# LinkCreation(Eval("Task").ToString(), Eval("Status").ToString(), Eval("AID").ToString(), "rtd")%>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <FooterTemplate>
                        </table> <span><font color='red'>*</font><sub> All Tasks except Meta have amount per
                            unit</sub></span>
                    </FooterTemplate>
                </asp:Repeater>
            </td>
        </tr>
        <asp:UpdatePanel ID="uplComments" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlComments" runat="server" Visible="false">
                    <tr>
                        <td valign="top">Rejection Cause :
                        </td>
                        <td colspan="6" align="left" valign="top">
                            <asp:TextBox ID="txtComments" TextMode="MultiLine" Rows="5" Columns="82" runat="server"></asp:TextBox><asp:RequiredFieldValidator
                                ID="RequiredFieldValidator1" ControlToValidate="txtComments" ValidationGroup="rgtd"
                                runat="server" ErrorMessage="* Add Rejection Cause" ForeColor="Red"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">&nbsp;
                        </td>
                        <td colspan="6" align="center">
                            <asp:Button CssClass="button" ID="btnAddComments" runat="server" Text="Add Comments"
                                OnClick="btnAddComments_Click" ValidationGroup="rgtd" />
                        </td>
                    </tr>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:UpdatePanel ID="uplAccount" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnlAmount" runat="server" Visible="false">
                    <tr>
                        <td valign="top">Amount :
                        </td>
                        <td colspan="6" align="left">
                            <asp:TextBox ID="txtPayableAmount" Enabled="False" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">Bonus :
                        </td>
                        <td colspan="6" align="left">
                            <asp:TextBox ID="txtBonus" Enabled="False" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                         <td valign="top">Uncheck to Allow Bonus :
                        </td>
                        <td>
                            <asp:CheckBox ID="cbxBonux" Checked="true" runat="server"/>
                            Only if wrong mistakes are less then 10.
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">Remarks :
                        </td>
                        <td colspan="6" align="left">
                            <asp:TextBox ID="txtRemarks" TextMode="MultiLine" Rows="5" Columns="82" Style="width: 637px; height: 80px; resize: none" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top">&nbsp;
                        </td>
                        <td colspan="6" align="center">
                            <asp:Button ID="btnUpdateAmount" runat="server" Text="Approve Task"
                                OnClick="btnUpdateAmount_Click" stlye="width:20%; height: 24px;padding: 4px 5px 1px 5px; border: 1px solid #2A4F96; background: #FFFFFF;
                                                                        text-transform: capitalize; font: normal 11px Arial, Helvetica, sans-serif; color: #5D781D;" 
                                />
                        </td>
                    </tr>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </table>
    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="message"></asp:Label>
        </asp:Panel> 
</asp:Content>

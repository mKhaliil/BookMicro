<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master"
    AutoEventWireup="true" CodeBehind="TableTest.aspx.cs" Inherits="Outsourcing_System.TableTest" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
    <div id="DivError" runat="server" visible="false">
        <div style="font-size: 10pt; color: Green; text-align: left; background-color: #ffcdd6;
            border: 1px solid red; padding: 5px; font-family: Sans-Serif;">
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
    <div style="border-radius: 15px; border: 3px solid gray; width: 60%; margin: auto;
        padding: 25px;">
        <asp:LinkButton ID="lnkTest" runat="server" Font-Size="X-Large" Text="Click here to get a Table test."
            OnClick="lnkTest_Click"></asp:LinkButton><br />
        <br />
        <div id="ConfirmationPnl" runat="server">
            <table>
                <tr>
                    <td>
                        <asp:LinkButton ID="lnkAssignedTest" runat="server" Font-Size="X-Large" Text="Click here to get assigned Test."
                            OnClick="lnkAssignedTest_Click"></asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <h5>
                            If you have done your test Please make a zip file of All Files, and Upload it Here.</h5>
                    </td>
                </tr>
                <tr>
                    <td>
                        Select zip File containing tables :
                    </td>
                    <td>
                        <asp:FileUpload ID="fZipUpload" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnUpload" runat="server" Text="Submit Test" OnClick="btnUpload_Click" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:GridView ID="gvTestHistory" runat="server" Style="margin-left: 1px; background-color: #E9E9E9"
            AutoGenerateColumns="False" ShowHeaderWhenEmpty="True" EmptyDataText='No data available in Test History.'
            DataKeyNames="ID" Width="95%">
            <Columns>
                <asp:TemplateField HeaderText="Test Name" HeaderStyle-Width="30%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lbltestName" Text='<%# Eval("testName") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Test Type" HeaderStyle-Width="30%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblTestType" Text='<%# Eval("testtype") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="Status" HeaderStyle-Width="60%">
                    <ItemTemplate>
                        <asp:Label runat="server" ID="lblSTATUS" Text='<%# Eval("status") %>' />
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
            <HeaderStyle CssClass="gridHeader" />
            <RowStyle HorizontalAlign="Center"></RowStyle>
        </asp:GridView>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true"
    CodeBehind="SpellChecker.aspx.cs" Inherits="Outsourcing_System.SpellChecker1" %>

<%@ Register Assembly="PdfViewer" Namespace="PdfViewer" TagPrefix="cc1" %>
<%@ Register Assembly="pdfview" Namespace="SkySof" TagPrefix="cc1" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            // ShowSpellChecker();
        });
    </script>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
    <asp:UpdatePanel ID="uPanel" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="lstBxMistakes" EventName="SelectedIndexChanged" />
            <asp:AsyncPostBackTrigger ControlID="btnReplace" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnReplaceAll" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnIgnore" EventName="Click" />
            <asp:AsyncPostBackTrigger ControlID="btnFinish" EventName="Click" />            
        </Triggers>
        <ContentTemplate>
            <div style="z-index: -5000; position: absolute; display: none;">
                <asp:TextBox ID="txtContent" runat="server"></asp:TextBox>
            </div>
            <div style="width: 90%; margin-top: 10px; padding: 20px; margin-bottom: 10px; border: 3px solid gray;">
                <table cellpadding="3" style="width: 100%;">
                    <tr>
                        <td style="width: 60%;">
                            <b>Source Pdf Page no:<asp:Label ID="lblSourcePageNo" runat="server" Text="" ForeColor="Green"></asp:Label></b>
                            <asp:Label ID="lblPageNo" runat="server" Text=""></asp:Label><br />
                            <cc1:ShowPdf ID="PDFViewerSource" runat="server" BorderWidth="1px" BorderColor="Gray"
                                Height="600px" Width="100%" />
                        </td>
                        <td valign="top">
                            <div style="width: 420px; padding-left: 20px;">
                                <div style="width: 200px; min-height: 450px; float: left;">
                                    <b>Spell Mistakes:</b><br />
                                    <asp:ListBox ID="lstBxMistakes" AutoPostBack="true" runat="server" Style="min-height: 450px;"
                                        Width="200px" OnSelectedIndexChanged="lstBxMistakes_SelectedIndexChanged"></asp:ListBox>
                                </div>
                                <div style="width: 200px; min-height: 450px; float: right;">
                                    <b>Suggestions:</b><br />
                                    <asp:ListBox ID="lstBxSuggestions" runat="server" Style="min-height: 450px;" Width="200px">
                                    </asp:ListBox>
                                    <asp:Label ID="lblSuggestion" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label>
                                </div>
                                <b>Details:</b><br />
                                <div style="text-align: left; float: right; margin-top: 5px; padding: 8px; border: 1px solid gray;
                                    width: 96%;">
                                    This word is on page:&nbsp&nbsp<asp:Label ID="lblPages" ForeColor="Green" Font-Bold="true"
                                        runat="server"></asp:Label><br />
                                    Total occurences:&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp<asp:Label ID="lblOccurences" ForeColor="Green"
                                        Font-Bold="true" runat="server"></asp:Label><br />
                                    <br />
                                    Manually Update Spell:<asp:TextBox ID="txtManualSpell" runat="server" Width="310"></asp:TextBox><asp:Label ID="lblManualyText" runat="server" Text="*" ForeColor="Red" Visible="false"></asp:Label><br />
                                    <asp:Button ID="btnReplace" runat="server" Text="Replace" OnClick="btnReplace_Click" />
                                    <asp:Button ID="btnReplaceAll" runat="server" Text="Replace All" OnClick="btnReplaceAll_Click" />
                                    <asp:Button ID="btnIgnore" runat="server" Text="Ignore" OnClick="btnIgnore_Click" />
                                    <asp:Button ID="btnFinish" runat="server" Text="Ignore All" OnClick="btnFinish_Click" />
                                </div>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

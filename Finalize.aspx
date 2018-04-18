<%@ Page Title="" Language="C#" MasterPageFile="~/EditorMaster.Master" AutoEventWireup="true"
    ValidateRequest="false" CodeBehind="Finalize.aspx.cs" Inherits="Outsourcing_System.Finalize" %>

<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder0" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LinkPortion" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <style type="text/css">
        a:hover
        {
            cursor: default;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function getSelectionHtml() {
            var html = "";
            if (typeof window.getSelection != "undefined") {
                var sel = window.getSelection();
                if (sel.rangeCount) {
                    var container = document.createElement("div");
                    for (var i = 0, len = sel.rangeCount; i < len; ++i) {
                        container.appendChild(sel.getRangeAt(i).cloneContents());
                    }
                    html = container.innerHTML;
                }
            } else if (typeof document.selection != "undefined") {
                if (document.selection.type == "Text") {
                    html = document.selection.createRange().htmlText;
                }
            }
            return html;
        }
        function getsSelectedText() {
            var abc = getSelectionHtml();
            document.getElementById("ctl00_ContentPlaceHolder1_hFSelectedText").value = abc;
            ShowLoadingGif();
            if (abc == "") {
                return false;
            }
            else {
                return true;
            }

        }
    </script>
    <%--<asp:UpdatePanel ID="upanel" runat="server" UpdateMode="Conditional">
        <ContentTemplate>--%>
    <asp:HiddenField ID="hFSelectedText" runat="server" Value="" />
    <div style="min-height: 550px; vertical-align: top; width: 984px; border: 4px solid gray;
        padding-top: 20px; margin-top: 15px;">
        <div style="text-align: right; padding-right: 20px;">
            <asp:Button ID="btnInlineMarkUp" class="menu" runat="server" Text="Inline MarkUp"
                OnClick="btnInlineMarkUp_Click" OnClientClick="ShowLoadingGif();" />
            <asp:Button ID="btnPageBreaks" class="menu" OnClientClick="ShowLoadingGif();" runat="server"
                Text="Page Breaks" OnClick="btnPageBreaks_Click" />
            <asp:Button ID="btnFinalize" runat="server" Text="Finalize" class="menu" OnClientClick="ShowLoadingGif();"
                OnClick="btnFinalize_Click" />
        </div>
        <div id="divInlineMarkUp" runat="server" visible="false" style="vertical-align: top;
            margin-top: 20px; margin-left: 50px; background-color: #fffb99; margin-right: 50px;
            padding: 15px;">
            <table width="100%">
                <tr>
                    <td style="width: 25%; text-align: justify; vertical-align: top;">
                        <div style="height: 450px; overflow: scroll;">
                            <asp:TreeView ID="treeMarkUps" runat="server" ShowLines="true" OnSelectedNodeChanged="treeMarkUps_SelectedNodeChanged">
                            </asp:TreeView>
                        </div>
                    </td>
                    <td style="vertical-align: top; width: 75%;">
                        m<div>
                            <table>
                                <tr>
                                    <td>
                                        Plese Insert some Text:&nbsp&nbsp&nbsp<asp:TextBox ID="txtSearch" Width="390px" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        <asp:Button ID="btnFind" Text="Find" runat="server" OnClick="btnFind_Click" />
                                        <asp:Button ID="btnCancel" Text="Cancle" runat="server" OnClick="btnCancel_Click" />
                                        <asp:Button ID="btnCreateInline" Text="Inline Mark Up" runat="server" OnClientClick="return getsSelectedText();"
                                            OnClick="btnCreateInline_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                        <div>
                            <div id="divText" runat="server" style="width: 565px; background-color: White; height: 350px;
                                overflow: auto;">
                            </div>
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divPageBreak" runat="server" visible="false" style="vertical-align: top;
            margin-top: 20px; margin-left: 50px; background-color: #fffb99; margin-right: 50px;
            padding: 15px;">
            <table width="100%">
                <tr>
                    <td style="width: 25%; text-align: justify; vertical-align: top;">
                        <div style="height: 450px; overflow: scroll;">
                            <asp:TreeView ID="treePageBreaks" runat="server" ShowCheckBoxes="Leaf">
                            </asp:TreeView>
                        </div>
                    </td>
                    <td style="vertical-align: top; width: 75%;">
                        <table>
                            <tr>
                                <td>
                                    <asp:Button ID="btnAddPageBreaks" runat="server" Text="Add Page Breaks" OnClientClick="ShowLoadingGif();" OnClick="btnAddPageBreaks_Click" />
                                    <asp:Button ID="btnDeletePageBreak" runat="server" Visible="false" Text="Delete Page Break" OnClientClick="ShowLoadingGif();"
                                        OnClick="btnDeletePageBreak_Click" />
                                    <asp:Button ID="btnSaveChanges" runat="server" Text="Save Changes" OnClick="btnSaveChanges_Click" OnClientClick="ShowLoadingGif();"/>
                                    <asp:Button ID="btnPageBreakCancel" runat="server" Text="Cancel" OnClick="btnPageBreakCancel_Click" OnClientClick="ShowLoadingGif();"/>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <div id="divPageBreakResult" style="color: Red;" runat="server">
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
        <div id="divFinalize" runat="server" visible="false" style="vertical-align: top;
            margin-top: 20px; margin-left: 50px; background-color: #fffb99; margin-right: 50px;
            padding: 15px;">
            <table>
                <tr>
                    <td>
                        This Book is Finalized. please go your desktop for next.
                        <br />
                        <asp:Button ID="btnFinalizedProcess" runat="server" Text="Start Fianlizing" OnClientClick="ShowLoadingGif();" OnClick="btnFinalizedProcess_Click" />
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divFinalizedResult" runat="server">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <%--   </ContentTemplate>
    </asp:UpdatePanel>--%>
    <asp:Panel ID="pnlMessage" runat="server">
        <asp:UpdatePanel ID="upnlMessagePortion" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="divMessageBox" class="l-box">
                    <asp:Label ID="lblMessage" runat="server" Text="Some Error Here"></asp:Label>
                    <asp:Button ID="btnMessageOk" runat="server" Text="Ok" CssClass="button" OnClick="btnMessageOk_Click" />
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <div id="lightbox-panel1">
        <img src="img/loading.gif" alt="" />
    </div>
    <div id="lightbox">
    </div>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

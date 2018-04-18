<%@ Control Language="C#" AutoEventWireup="true" Inherits="Controls_PDFTextCmp" Codebehind="PDFTextCmp.ascx.cs" %>
<%@ Register TagPrefix="cc1" Namespace="PdfViewer" Assembly="PdfViewer" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="~/UserControls/PDFViewer.ascx" TagName="PDF" TagPrefix="ViewerCtrl" %>
<style type="text/css">
    .style1
    {
    }
    .style2
    {
        width: 32px;
    }
</style>
<script src="../scripts/BlockUI.js" type="text/javascript" />
<link href="../Styles/TextCmpStyle.css" rel="stylesheet" type="text/css" />
<asp:ScriptManager ID="sm" runat="server" />
<script type="text/javascript">
    function alertME() {
        alert(' im enable');
    }
    function EnableDisableCommentBox() {
        //event.returnValue = false;

        //alert('rb ID: ' + '<%= rbOther.ClientID %>');
        var rbOtherCheckedVal = document.getElementById('<%= rbOther.ClientID %>').checked;

        //alert('checked Value: ' + rbOtherCheckedVal);
        var txtCmts = document.getElementById('<%= txtComments.ClientID %>');
        //alert('rbOtherCheckedVal' + rbOtherCheckedVal);
        if (rbOtherCheckedVal == true) {
            txtCmts.disabled = false;
            txtCmts.value = '';
        }
        else {
            txtCmts.disabled = true;
        }
    }

</script>
<table width="100%" border="1">
    <tr id="trLists" runat="server" visible="false">
        <td>
        </td>
        <td align="center">
            <asp:ListBox ID="listBox1" runat="server" OnSelectedIndexChanged="listBox1_SelectedIndexChanged"
                AutoPostBack="true"></asp:ListBox>
        </td>
        <td>
        </td>
        <td>
        </td>
        <td align="center">
            <asp:ListBox ID="listBox2" runat="server" OnSelectedIndexChanged="listBox2_SelectedIndexChanged"
                AutoPostBack="true"></asp:ListBox>
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <tr id="trBtnMatch" runat="server" visible="false">
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
        <td colspan="2" align="center">
            <asp:Button ID="btnMatch" runat="server" Text="Match" OnClick="btnMatch_Click" />
        </td>
        <td>
            &nbsp;
        </td>
        <td>
            &nbsp;
        </td>
    </tr>
    <%--<tr id="trPDF" runat="server" visible="false">
        <%--<td>
        </td>-- %>
        <td colspan="2">
            <cc1:ShowPdf ID="pdf1" runat="server" BorderStyle="Inset" BorderWidth="2px" Height="552px"
                Style="z-index: 103; left: 24px; top: 128px" Width="100%" />
        </td>
        <%--<td>
        </td>
        <td>
        </td>-- %>
        <td colspan="2">
            <cc1:ShowPdf ID="pdf2" runat="server" BorderStyle="Inset" BorderWidth="2px" Height="552px"
                Style="z-index: 103; left: 24px; top: 128px" Width="100%" />
        </td>
        <%--<td>
        </td>-- %>
    </tr>--%>
    <tr id="trPDF" runat="server" visible="false">
        <%--<td>
        </td>--%>
        <td colspan="2">
        </td>
        <td>
            <cc1:ShowPdf ID="pdf1" runat="server" BorderStyle="Inset" BorderWidth="2px" Height="552px"
                Style="z-index: 103; left: 24px; top: 128px" />
        </td>
        <td>
        </td>
        <td colspan="2">
            <cc1:ShowPdf ID="pdf2" runat="server" BorderStyle="Inset" BorderWidth="2px" Height="552px"
                Style="z-index: 103; left: 24px; top: 128px" />
        </td>
    </tr>
    </table>

    <table width="80%" border="0" align="center">
    <tr id="trImg" runat="server" visible="true">
        <td width="40%">
            <div id='mycustomscroll' class='flexcroll' style="font-family: 'Times New Roman', Times, serif;
                font-size: 13px; color: #000000;">
                <div style="border-style: solid; border-color: inherit; border-width: 1px; position: relative;
                    height: 845px; margin-bottom: 0px;">
                    <asp:Image runat="server" AlternateText="Source PDF Image" ID="imgSrc" ImageUrl="~/web/images/NotFound.JPG"
                        Height="845px" Style="z-index: 103;width:100%;" />

                        <%--<input id="Image1" type="image" src="~/web/images/NotFound.JPG" alt="Source PDF Image" style="height:845px;z-index: 103;width:100%;"/>--%>
                </div>
            </div>
        </td>
        <td width="60%">
            <div id='mycustomscroll3' class='flexcroll' style="color: #000000;">
                <div style="position: relative; height: 845px; border: 1px solid">
                    <ViewerCtrl:PDF ID="pdfCtrl" runat="server" OnFileEdit="PDFCtrl_FileEdit" PDFFile="/Resources/NotUploaded.pdf" />
                </div>
            </div>
        </td>
    </tr>
    </table>

    <table width="100%" border="1">
    <tr id="trComments" runat="server" visible="false">
        <td align="center">
            <div style="width: 200px">
                <ajaxtoolkit:CollapsiblePanelExtender ID="cpe" runat="Server" TargetControlID="ContentPanel"
                    CollapsedSize="0" ExpandedSize="180" Collapsed="True" ExpandControlID="Panel"
                    CollapseControlID="Panel" AutoCollapse="False" AutoExpand="False" ScrollContents="False"
                    TextLabelID="TextLabel" CollapsedText="" ExpandedText="" ImageControlID="imgArrows"
                    ExpandedImage="~/web/images/collapse.jpg" CollapsedImage="~/web/images/expand.jpg"
                    ExpandDirection="Vertical" />
                <asp:Panel ID="Panel" runat="server">
                    <div>
                        <div style="float: left; color: Black; margin-top: 5px;">
                            <asp:Image ID="imgArrows" runat="server" />
                        </div>
                        <div style="cursor: pointer; color: #5377A9; font-family: Arial,Sans-Serif; font-size: 1.0em;
                            font-weight: bold; text-align: left">
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Comments
                        </div>
                    </div>
                </asp:Panel>
                <asp:Panel ID="ContentPanel" runat="server">
                    <%--<div onclick="EnableDisableCommentBox()">--%>
                    <table style="color: Black">
                        <tr>
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td>
                                <asp:RadioButton ID="rbMissing" runat="server" Checked="true" GroupName="Remarks"
                                    OnClick="EnableDisableCommentBox();" Text="Missing" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td>
                                <asp:RadioButton ID="rbExtra" runat="server" GroupName="Remarks" OnClick="EnableDisableCommentBox();"
                                    Text="Extra" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td>
                                <asp:RadioButton ID="rbSpell" runat="server" GroupName="Remarks" OnClick="EnableDisableCommentBox();"
                                    Text="Spell" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td>
                                <asp:RadioButton ID="rbOther" runat="server" GroupName="Remarks" OnClick="EnableDisableCommentBox();"
                                    Text="Other" />
                                &nbsp;&nbsp;&nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style1" colspan="2" style="text-align: center">
                                <asp:TextBox ID="txtComments" runat="server" Enabled="false" Text="Add Comments Here" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style1" colspan="2" style="text-align: center">
                                <asp:Button ID="btnAddComment" runat="server" Text="Add" OnClick="btnAddComment_Click"
                                    CssClass="button"></asp:Button>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
        </td>
    </tr>
</table>

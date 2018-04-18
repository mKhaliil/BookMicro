﻿<%@ Page Language="C#" MasterPageFile="~/EditorMaster.master" AutoEventWireup="true"
    CodeBehind="TaggingUnTagged.aspx.cs" Inherits="Outsourcing_System.TaggingUnTagged" %>
<%@ Register Assembly="PdfViewer" Namespace="PdfViewer" TagPrefix="cc1" %>
<%@ Register Assembly="pdfview" Namespace="SkySof" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="LinkPortion" runat="server">
    <asp:Label ID="lblMessage" CssClass="message" Text="" runat="server" />
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">

    <td>
        <!--table width="100%">
<tr>    
    <td align="left"><asp:LinkButton CssClass="link" ID="lnkUserPanle" runat="server" 
            onclick="lnkUserPanle_Click">User Panel</asp:LinkButton></td>
    <td align="right"><asp:LinkButton CssClass="link" ID="lnkLogout" runat="server" 
            onclick="lnkLogout_Click">Logout</asp:LinkButton></td>
</tr>
</table-->
    </td>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <table id="Table1" width="100%" cellpadding="2" cellspacing="0">
        <tr class="td">
            <th>
                UnTagged Text
            </th>
            <th>
            </th>
            <th>
                Target File Preview
            </th>
        </tr>
        <tr class="bbw" valign="top">
            <td width="50%" align="center" valign="middle">
                <asp:Panel ID="Panel1" runat="server" Height="190px" Width="100%">
                    <asp:Label ID="Label1" runat="server" Text="Un-Tagged Text" Font-Bold="True" ForeColor="#3366CC"></asp:Label>
                    <asp:Label ID="elemType" CssClass="message" runat="server" Text="" Style="float: right" />
                    <br />
                    <asp:TextBox ID="UnTaggedText" runat="server" TextMode="MultiLine" Height="100px"
                        Width="100%" CssClass="bbw"></asp:TextBox>
                    <br />
                   <%-- <asp:ScriptManager ID="ScriptManager1" runat="server">
                    </asp:ScriptManager>--%>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table>
                                <tr>
                                    <td align="center" colspan="2">
                                        <asp:Label ID="UntaggedRemaining" CssClass="message" runat="server" Text="" />
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        Assign Tag :
                                    </td>
                                    <td align="left">
                                        <asp:DropDownList ID="cmbTag" runat="server" OnSelectedIndexChanged="cmbTag_SelectedIndexChanged"
                                            AutoPostBack="True" CssClass="bbw">
                                            <asp:ListItem Value="">Select Option</asp:ListItem>
                                            <asp:ListItem Value="pre-section">Pre-Section</asp:ListItem>
                                            <asp:ListItem Value="post-section">Post-Section</asp:ListItem>
                                            <asp:ListItem Value="section">Section</asp:ListItem>
                                            <asp:ListItem Value="text">Text</asp:ListItem>
                                            <asp:ListItem Value="skip">Remove</asp:ListItem>
                                            <asp:ListItem Value="footnote">Footnote</asp:ListItem>
                                            <asp:ListItem Value="upara">Upara</asp:ListItem>
                                            <asp:ListItem Value="spara">Spara</asp:ListItem>
                                            <asp:ListItem Value="npara">Npara</asp:ListItem>
                                            <asp:ListItem Value="emphasized">Emphasized</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Button ID="btnHelp" runat="server" Text="Help" OnClick="btnHelp_Click" />
                                    </td>
                                </tr>
                            </table>
                            <asp:Panel ID="pnlSpara" runat="server" Width="100%" Visible="false">
                                <table>
                                    <tr>
                                        <td align="right">
                                            Spara Type :
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddSparaType" runat="server" CssClass="bbw" OnSelectedIndexChanged="ddSparaType_SelectedIndexChanged"
                                                AutoPostBack="True">
                                                <asp:ListItem Value="quotation">Quotation</asp:ListItem>
                                                <asp:ListItem Value="letter">Letter</asp:ListItem>
                                                <asp:ListItem Value="verse">Verse</asp:ListItem>
                                                <asp:ListItem Value="poem">Poem</asp:ListItem>
                                                <asp:ListItem Value="other">Other</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Stanza Type :
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddStanzaType" runat="server" CssClass="bbw">
                                                <asp:ListItem Value="para">Para</asp:ListItem>
                                                <asp:ListItem Value="line">Line</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Background Color :
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddBackgroundColor" runat="server" CssClass="bbw">
                                                <asp:ListItem Value="none">None</asp:ListItem>
                                                <asp:ListItem Value="gray">Gray</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Other Align :
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddOtherAlign" runat="server" CssClass="bbw" Visible="false">
                                                <asp:ListItem Value="left">Left</asp:ListItem>
                                                <asp:ListItem Value="center">Center</asp:ListItem>
                                                <asp:ListItem Value="right">Right</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Stanza :
                                        </td>
                                        <td align="left">
                                            <asp:CheckBox ID="chkStanza" runat="server" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlNpara" runat="server" Width="100%" Visible="false">
                                <table>
                                    <tr>
                                        <td align="right">
                                            Has Number :
                                        </td>
                                        <td align="left">
                                            <asp:CheckBox ID="chkHaseNumber" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            Start With :
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddStartOption1" runat="server">
                                                <asp:ListItem Value="">Select Option</asp:ListItem>
                                                <asp:ListItem Value="1">1</asp:ListItem>
                                                <asp:ListItem Value="a">a</asp:ListItem>
                                                <asp:ListItem Value="i">i</asp:ListItem>
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddStartOption2" runat="server">
                                                <asp:ListItem Value="">Select Option</asp:ListItem>
                                                <asp:ListItem Value=".">.</asp:ListItem>
                                                <asp:ListItem Value=")">)</asp:ListItem>
                                                <asp:ListItem Value="-">-</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlSection" runat="server" Width="100%" Visible="false">
                                <table>
                                    <tr>
                                        <td align="right">
                                            Section Type :
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddLevels" runat="server" AutoPostBack="True" CssClass="bbw"
                                                OnSelectedIndexChanged="ddLevels_SelectedIndexChanged">
                                                <asp:ListItem Value="">Select Option</asp:ListItem>
                                                <asp:ListItem Value="chapter">Chapter</asp:ListItem>
                                                <asp:ListItem Value="level1">Level 1</asp:ListItem>
                                                <asp:ListItem Value="level2">Level 2</asp:ListItem>
                                                <asp:ListItem Value="level3">Level 3</asp:ListItem>
                                                <asp:ListItem Value="level4">Level 4</asp:ListItem>
                                                <asp:ListItem Value="book">Book</asp:ListItem>
                                                <asp:ListItem Value="part">Part</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <table>
                                        <div runat="server" id="pnlLevelSubtype" visible="false">
                                            <tr>
                                                <td align="right">
                                                    Sub Type :
                                                </td>
                                                <td align="left">
                                                    <asp:DropDownList ID="ddLevelSubtype" runat="server" AutoPostBack="false" CssClass="bbw">
                                                        <asp:ListItem Value="title">Title</asp:ListItem>
                                                        <asp:ListItem Value="prefix">Prefix</asp:ListItem>
                                                        <asp:ListItem Value="continue">Continue Title</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td align="right">
                                                    Caps :
                                                </td>
                                                <td align="left">
                                                    <asp:CheckBox ID="chkCaps" runat="server" />
                                                </td>
                                            </tr>
                                            <tr align="center">
                                                <td />
                                                <td align="center"> 
                                                        <asp:Button ID="btnApplyAll" runat="server" Text="Apply All" OnClick="btnApplyAll_Click"  OnClientClick="fakeClick(event, document.getElementById('clickMe'))"/>
                                                </td>
                                                <td />
                                            </tr>
                                        </div>
                                    </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlText" runat="server" Width="100%" Visible="false">
                                <table>
                                    <tr>
                                        <td align="right">
                                            Text Type :
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddTextType" runat="server" AutoPostBack="false" CssClass="bbw">
                                                <asp:ListItem Value="">Select Option</asp:ListItem>
                                                <asp:ListItem Value="superscript">Superscript</asp:ListItem>
                                                <asp:ListItem Value="subscript">Subscript</asp:ListItem>
                                                <asp:ListItem Value="text">Text</asp:ListItem>
                                                <asp:ListItem Value="continue">Continue Text</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:Button ID="btnTextApplyAll" runat="server" Text="Apply All" OnClick="btnTextApplyAll_Click"  OnClientClick="fakeClick(event, document.getElementById('clickMe'))"/>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlEmphasis" runat="server" Width="100%" Visible="false">
                                <table>
                                    <tr>
                                        <td align="right" valign="top">
                                            Type :
                                        </td>
                                        <td align="left">
                                            <asp:RadioButtonList ID="rdoEmphasis" runat="server">
                                                <asp:ListItem Value="italic" Selected="True">Italic</asp:ListItem>
                                                <asp:ListItem Value="bold">Bold</asp:ListItem>
                                                <asp:ListItem Value="bold-italic">Bold-Italic</asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlFootNote" runat="server" Width="100%" Visible="false">
                                <table>
                                    <tr>
                                        <td align="right" valign="top">
                                            Footnote Definition :
                                        </td>
                                        <td align="left">
                                            <asp:TextBox ID="txtFootNoteDef" runat="server" TextMode="MultiLine" Columns="35"
                                                Rows="15"></asp:TextBox>
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                            <asp:Panel ID="pnlHelp" runat="server" Width="100%" Visible="false">
                                <div id="pnlHelpText" runat="server">
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </asp:Panel>
            </td>
            <td align="center" valign="middle">
                <asp:Panel ID="Panel2" runat="server">
                    <asp:ImageButton ID="btnSubmit" ImageUrl="img/sb.gif" runat="server" OnClick="btnSubmit_Click" />
                </asp:Panel>
                <asp:ImageButton ID="btnFinish" ImageUrl="img/finish.gif" runat="server" OnClick="btnFinish_Click"
                    Visible="false" />
            </td>
            <td width="50%">
                <cc1:ShowPdf ID="PDFViewerTarget" runat="server" BorderStyle="Inset" BorderWidth="2px"
                    Height="550px" Width="460px" />
            </td>
        </tr>
    </table>
</asp:Content>
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                    
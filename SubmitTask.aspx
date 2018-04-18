<%@ Page Language="C#" ValidateRequest="false" MasterPageFile="UserMaster.Master" EnableEventValidation="false"
    AutoEventWireup="true" Inherits="SubmitTask" Title="Untitled Page" CodeBehind="SubmitTask.aspx.cs" %>

<%@ Register src="UserControls/CommonControl/ucShowMessage.ascx" tagname="ucShowMessage" tagprefix="uc1" %>

<asp:Content ID="Content3" ContentPlaceHolderID="mainHeadContents" runat="server">
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="mainBodyContents" runat="Server">
    <div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">
         <uc1:ucShowMessage ID="ucShowMessage1" runat="server" />
        </div>
            <%--<asp:Label runat="server" ID="lblMessage" Text="" />--%>
            <div style="width: 100%; margin-top: 5%;">
                <table style="margin: 0 auto; width: 65%;">
                    <tr>
                        <td valign="top" align="right" class="normaltext">
                           
                            Process :
                        </td>
                        <td align="left">
                            <div class="sfb">
                                <div class="stb">
                                    <asp:TextBox ID="txtProcess" runat="server" ReadOnly="true" class="fieldBoxC"></asp:TextBox>
                                </div>
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" class="normaltext">Select Image's zip :
                        </td>
                        <td align="left">
                            <%--  <input id="File1" type="file" runat="server" class="normaltext" />&nbsp;--%>
                            <asp:FileUpload ID="fuPdf" runat="server" class="multi" />
                            <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload"
                                Width="66px" ValidationGroup="v3" Visible="false" />
                        </td>
                    </tr>
                    <tr>
                        <td align="right" valign="top" class="normaltext">Comments :
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtComments" runat="server" Columns="39" Rows="10" TextMode="MultiLine"
                                CssClass="normaltext"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="right">&nbsp;
                        </td>
                        <td align="left">
                            <asp:ImageButton ID="btnSubmit" ImageUrl="img/sb.gif" runat="server" OnClick="btnSubmit_Click"
                                ToolTip="Upload Zip File and Submit" />
                            <asp:ImageButton ID="btnSubmit0" ImageUrl="img/finish.gif" runat="server" OnClick="btnSubmit0_Click"
                                Visible="false" ToolTip="Finsih task if no tables exist" />&nbsp;&nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <asp:UpdatePanel ID="updConfirmation" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="btnSubmit0" EventName="Click" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:Panel ID="pnlDelConfirmation" runat="server" Visible="false">
                                        <table style="width: 100%">
                                            <tr>
                                                <td style="height: 38px; text-align: center">
                                                    <asp:Label ID="lblDelOrderConfirmation" runat="server" Font-Bold="True" Font-Size="Medium"
                                                        Text="Are you sure ?" Width="100%"></asp:Label>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td style="height: 3px; text-align: center">&nbsp;<asp:Button ID="btnYesDelOrder" runat="server" OnClick="btnYes_Click" Text="YES"
                                                    Width="66px" ValidationGroup="v3" />
                                                    &nbsp;&nbsp; &nbsp;&nbsp;
                                        <asp:Button ID="btnNoDelOrder" runat="server" OnClick="btnNo_Click" Text="NO" Width="61px"
                                            ValidationGroup="v3" />
                                                    &nbsp;&nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </td>
                    </tr>
                    <tr>
                        <td align="left" colspan="2">
                            <div id="panelDiv">
                                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnSubmit0" EventName="Click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <asp:Panel ID="pnlExtraImgConf" runat="server" Visible="false">
                                            <table style="width: 100%">
                                                <tr>
                                                    <td style="height: 38px; text-align: center">
                                                        <asp:Label ID="Label1" runat="server" Font-Bold="True" Font-Size="Medium" Text="Zip contains extra images, are you sure you want to continue?"
                                                            Width="100%"></asp:Label>
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td style="height: 3px; text-align: center">&nbsp; &nbsp;&nbsp; &nbsp;&nbsp;<asp:Button ID="Button1" runat="server" OnClick="btnYesImg_Click"
                                                        Text="YES" Width="66px" ValidationGroup="v3" />
                                                        <%--                                        <asp:Button ID="btnNoImg" runat="server" OnClick="btnNoImg_Click" Text="NO" Width="61px"
                                            ValidationGroup="v3" />
                                                        --%>
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <input type="button" onclick="javascript: document.getElementById('panelDiv').style.display = 'none';"
                                                value="NO" style="width: 60px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </asp:Panel>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
            <div id="lightbox-panel">
                <!--Login DIV -->
                <!--close-panel3-->
                <a id="close-panel3">
                    <div style="float: right;">
                        <img src="img/close.gif" />
                    </div>
                </a>
                <div id="simpleDialog" class="l-box">
                    <div class="formBoxC">
                        <div id="ProgressBar">
                            <asp:Image ID="Image1" runat="server" ImageUrl="img/loading.gif" Width="25" Height="25"
                                AlternateText="Processing" />
                        </div>
                    </div>
                    <div class="noFloat">
                    </div>
                    <div class="formBoxC">
                        <div class="titleC">
                            <%--<asp:UpdatePanel ID="upd2" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSubmit0" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>
                            <input name="lightBoxLabel" type="text" value="testing lable" />
                        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                            <div id="lblDiv">
                                <asp:Label runat="server" ID="lblMessage" Text="" />
                            </div>
                        </div>
                    </div>
                    <br />
                    <div class="noFloat">
                    </div>
                    <div class="formBoxC" style="margin: 10px 10px 10px 50px;">
                        <%--<asp:Button ID="btnSave" runat="server" CssClass="button" Text="Edit" OnClick="btnSave_Click"
                        OnClientClick="hideFreeTextBox();" />
                    <asp:Button ID="btnSplit" runat="server" CssClass="button" Text="Split" OnClick="btnSplit_Click"
                        OnClientClick="hideFreeTextBox();" />
                    <asp:Button ID="btnCaps" runat="server" CssClass="button" Text="Caps" OnClick="btnCaps_Click"
                        OnClientClick="hideFreeTextBox();" />--%>
                    </div>
                </div>
            </div>
            <%-- <div id="lightbox-panel1">
        <img src="img/loading.gif" alt="" />
    </div>--%>
            <div id="lightbox">
            </div>
      
</asp:Content>


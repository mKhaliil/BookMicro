﻿<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/OnlineTestMasterPage.Master"
    AutoEventWireup="true" CodeBehind="MappingTest.aspx.cs" Inherits="Outsourcing_System.MappingTest" %>

<%@ Register Assembly="PdfViewer" Namespace="PdfViewer" TagPrefix="cc1" %>
<%@ Register Assembly="pdfview" Namespace="SkySof" TagPrefix="cc1" %>
<%@ Register Assembly="FreeTextBox" Namespace="FreeTextBoxControls" TagPrefix="FTB" %>
<%@ Register TagPrefix="telerik" Namespace="Telerik.Web.UI" Assembly="Telerik.Web.UI" %>
<%@ Register Assembly="ASPNetSpell" Namespace="ASPNetSpell" TagPrefix="ASPNetSpell" %>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder0" runat="server">
    <script type="text/javascript">

        function m(obj) {
            for (var i = 0; i < obj.length; i++) {
                if (obj.substring(i, i + 1) == ":")
                    break;
            }
            return (obj.substring(0, i));
        }
        function s(obj) {
            for (var i = 0; i < obj.length; i++) {
                if (obj.substring(i, i + 1) == ":")
                    break;
            }
            return (obj.substring(i + 1, obj.length));
        }
        function dis(mins, secs) {
            var disp;
            if (mins <= 9) {
                disp = " 0";
            } else {
                disp = " ";
            }
            disp += mins + ":";
            if (secs <= 9) {
                disp += "0" + secs;
            } else {
                disp += secs;
            }
            return (disp);
        }
        function redo() {
            secs--;
            if (secs == -1) {
                secs = 59;
                mins--;
            }
            document.getElementById('txt1').value = dis(mins, secs);


            //              alert(dis(mins, secs));
            //            alert($('#timehdnmin').value);
            //            $('#hfTime').value = dis(mins, secs);
            //              $('#timehdnmin').value = mins;
            //              $('#timehdnsec').value = secs;
            //              alert('min are ' + $('#timehdnmin').value);
            //                                                document.getElementById('timehdnmin').value = mins;
            //                                                document.getElementById('timehdnsec').value = secs;
            //                        alert(document.getElementById('timehdnmin').value);
            //                        alert(document.getElementById('timehdnsec').value);

            //            '<%Session["mins"] = "' + mins + '";%>';
            //            '<%Session["secs"] = "' + secs + '";%>';

            //            alert('session min value after is ' + '<%=Session["mins"]%>');

            //            alert('session sec value after is ' + '<%=Session["secs"]%>');

            //            alert('min in session ' + '<%=Session["mins"] %>');
            //            alert('<%=Session["secs"] %>');
            //            if ((mins == 0) && (secs == 0)) {
            //                window.alert("Time is up. Press OK to continue.");
            //                OnGetMessageSuccess();
            //            } else {

            //                cd = setTimeout("redo()", 1000);
            //            }
        }

        //        function OnGetMessageSuccess() {

        //            //                alert('2222');
        //            $.ajax({
        //                type: "POST",
        //                url: 'Step2.aspx/GetTestResult',
        //                data: "{}",
        //                contentType: "application/json; charset=utf-8",
        //                dataType: "json",
        //                success: function (res) {

        //                    //                        alert('result is ' + res.d);
        //                    if (res.d >= 60)
        //                        window.location = "Passed.aspx";
        //                    else
        //                        window.location = "Step3.aspx";
        //                }
        //            });
        //            //                PageMethods.SetName();
        //            //                window.location = "Step3.aspx";
        //        }

        function Timer() {

            document.getElementById('header').style.visibility = 'hidden';
            document.getElementById('menu').style.visibility = 'hidden';
            document.getElementById('mid').setAttribute("style", "float: left;margin-top:-255px;");




            //            redo();
        }

        function mouseOver() {
            document.getElementById('input').style.display = 'none'
        }
        function mouseOut() {

            document.getElementById('input').style.display = 'block'
        }
        function FootNoteDisplay(abc) {
            if (abc == 'true') {

                document.getElementById('divFootNoteDetail').setAttribute('display', 'block');
                alert(document.getElementById('divFootNoteDetail').getAttribute('display'));
            }
            else {

                document.getElementById('divFootNoteDetail').setAttribute('display', 'none');
                alert(document.getElementById('divFootNoteDetail').getAttribute('display'));

            }
        }
        var editor = null;
        function OnClientLoad(sender) {

            editor = sender;
        }
        function PageBreakTextSelected() {

            if (document.getElementById('ctl00$ContentPlaceHolder1$txtPageBreakNo').value == '') {
                alert('Please insert page break no.');
                return false;
            }
            else {
                return true;
            }
        }
        function HyperLinkTextSelected() {
            if (editor.getSelectionHtml() == '') {
                alert('Please select text in edit Box and insert footnote Detail.');
                return false;
            }
            else {
                document.getElementById('ctl00_ContentPlaceHolder1_txtSelectedText').value = editor.getSelectionHtml();
                return true;
            }
        }
        function AlertSelectedHtml() {
            if (editor.getSelectionHtml() == '' || document.getElementById('ctl00_ContentPlaceHolder1_txtFootNote').value == '') {
                var ob = 552;
                alert('Please select text in edit Box and insert footnote Detail.');
                return false;
            }
            else {
                document.getElementById('ctl00_ContentPlaceHolder1_txtSelectedText').value = editor.getSelectionHtml();
                document.getElementById('isTreePostBack').value = 'true';
                return true;
            }
        }
        function disableButton(btnId) {
            ShowLoadingGif()
            var btn = document.getElementById(btnId);
            btn.enabled = false;
        }
    </script>
    <link href="scripts/style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .reTool .AllCaps
        {
            background-image: url(img/To_Upper.png);
        }
        .reTool .AllLower
        {
            background-image: url(img/To_Lower.png);
        }
        .reTool .ResetContent
        {
            background-image: url(img/reset.gif);
        }
    </style>
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="LinkPortion" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
    <td>
        <script src="scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
        <script type="text/javascript" src="scripts/loader.js" />
        <script type="text/javascript" language="javascript">
            window.onload = function () {
                document.getElementById("ctl00_ContentPlaceHolder1_btnGenerate").focus();
            };
           
        </script>
    </td>
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:Panel ID="frmBookPreview" runat="server" DefaultButton="btnGenerate">
        <input id="txt1" readonly="true" type="text" value="" border="0" style="margin-left: -8px;
            color: red; font-size: large;" name="disp" />
        <input id="isTreePostBack" type="hidden" value="false" />
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
        <table id="Table1" width="100%" cellpadding="2" cellspacing="0">
            <tr class="td">
                <th>
                    Source File Preview
                </th>
                <th>
                    Target File Preview
                </th>
                <th>
                    File Tree
                </th>
            </tr>
            <tr>
                <td width="45%" valign="top">
                    <cc1:ShowPdf ID="PDFViewerSource" runat="server" BorderWidth="1px" Height="748px"
                        Width="100%" />
                </td>
                <td width="45%" valign="top">
                    <cc1:ShowPdf ID="PDFViewerTarget" runat="server" BorderWidth="1px" Height="748px"
                        Width="100%" />
                </td>
                <td>
                    <div id="TreeDIV" runat="server" style="overflow: scroll; border: 1px solid gray;
                        height: 748px; width: 270px;">
                        <asp:UpdatePanel ID="pnlTreeview" runat="server" UpdateMode="Conditional">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="btnSplit" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnSave" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnAddTable" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnDelTable" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnMerge" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnAddBox" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnAddImage" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="btnConvert" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="lnkAddUpara" EventName="Click" />
                                <asp:AsyncPostBackTrigger ControlID="imgClose" EventName="Click" />
                            </Triggers>
                            <ContentTemplate>
                                <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" CssClass="bbw" ShowCheckBoxes="Root"
                                    OnSelectedNodeChanged="TreeView1_SelectedNodeChanged" OnTreeNodeDataBound="TreeView1_TreeNodeDataBound">
                                    <DataBindings>
                                        <asp:TreeNodeBinding DataMember="upara" ValueField="outerxml" Text="upara"
                                            ToolTip="upara"></asp:TreeNodeBinding>
                                        <asp:TreeNodeBinding DataMember="spara" ValueField="outerxml" Text="spara"
                                            ToolTip="spara"></asp:TreeNodeBinding>
                                        <asp:TreeNodeBinding DataMember="npara" ValueField="outerxml" text="npara"
                                            ToolTip="npara"></asp:TreeNodeBinding>
                                        <asp:TreeNodeBinding DataMember="image" ValueField="outerxml" Text="image" ToolTip="image">
                                        </asp:TreeNodeBinding>
                                        <asp:TreeNodeBinding DataMember="table" ValueField="outerxml" Text="table" ToolTip="table">
                                        </asp:TreeNodeBinding>
                                        <asp:TreeNodeBinding DataMember="box" ValueField="outerxml" Text="box" ToolTip="box">
                                        </asp:TreeNodeBinding>
                                        <asp:TreeNodeBinding DataMember="section-title" ValueField="outerxml" TextField="displaytext"
                                            ToolTip="section-title"></asp:TreeNodeBinding>
                                        <asp:TreeNodeBinding DataMember="ln" ValueField="outerxml" TextField="displaytext">
                                        </asp:TreeNodeBinding>
                                        <asp:TreeNodeBinding DataMember="emphasis" ValueField="outerxml" Text="Italic"></asp:TreeNodeBinding>
                                    </DataBindings>
                                </asp:TreeView>
                                <asp:XmlDataSource ID="XmlDataSource1" runat="server" XPath="//box|//section|//section-title|//upara|//spara|//npara|table|//image|//emphasis">
                                </asp:XmlDataSource>
                                <br />
                                <asp:HiddenField ID="hfNodeText" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </td>
            </tr>
            <tr>
                <td align="center" colspan="2">
                    <table>
                        <tr>
                            <td>
                                <asp:Button ID="btnFirst" CssClass="button" runat="server" Text="<<" OnClick="btnPrevious_Click"
                                    OnClientClick="ShowLoadingGif();" />
                            </td>
                            <td>
                                <asp:Button ID="btnPrevious" CssClass="button" runat="server" Text="<" OnClick="btnPrevious_Click"
                                    OnClientClick="ShowLoadingGif();" />
                            </td>
                            <td>
                                <asp:TextBox ID="txtCurrentPage" CssClass="bbw" runat="server" Text="1" Style="float: right;
                                    text-align: right; width: 25px" /></div>
                            </td>
                            <td>
                                <asp:Label ID="lblTotalPages" runat="server" Text="000" Style="float: left" />
                            </td>
                            <td>
                                <asp:Button ID="btnNext" CssClass="button" runat="server" Text=">" OnClick="btnNext_Click"
                                    OnClientClick="ShowLoadingGif();" />
                            </td>
                            <td>
                                <asp:Button ID="btnLast" CssClass="button" runat="server" Text=">>" OnClick="btnLast_Click"
                                    OnClientClick="ShowLoadingGif();" />
                            </td>
                            <td>
                                <asp:Button ID="btnGenerate" CssClass="button" runat="server" Text="Generate" OnClick="btnGenerate_Click"
                                    OnClientClick="ShowLoadingGif();" />
                            </td>
                            <td>
                                <%-- <asp:Button ID="btnIssue" runat="server" Text="Issue" CssClass="button" OnClick="btnIssue_Click"
                                    Visible="false" OnClientClick="ShowLoadingGif();" />--%>
                                <cc1:PdfViewer ID="PdfViewer1" runat="server" Visible="false" />
                            </td>
                        </tr>
                    </table>
                </td>
                <td align="center">
                    <ASPNetSpell:SpellButton ID="SpellButton1" Visible="false" runat="server" />
                    <asp:ImageButton ID="btnAddTable" Width="25" Height="15" runat="server" ImageUrl="img/table.png"
                        OnClientClick="ShowLoadingGif();" OnClick="btnAddTable_Click" />
                    <asp:ImageButton ID="btnDelTable" Width="25" Height="15" runat="server" ImageUrl="img/cross.png"
                        OnClientClick="ShowLoadingGif();" OnClick="btnDelTable_Click" />
                    <asp:ImageButton ID="btnMerge" ToolTip="Merge" runat="server" Width="25" Height="15"
                        ImageUrl="img/merg.png" OnClientClick="ShowLoadingGif();" OnClick="btnMerge_Click" />
                    <asp:ImageButton ID="imgMergAll" ToolTip="Merge at Once" runat="server" Width="25"
                        Height="15" ImageUrl="img/MergAll.jpg" OnClientClick="ShowLoadingGif();" OnClick="imgMergAll_Click" />
                    <asp:ImageButton ID="imgSplitAll" ToolTip="Split" runat="server" Width="25" Height="15"
                        ImageUrl="img/split_all.png" OnClientClick="ShowLoadingGif();" OnClick="iimgSplitAll_Click" />
                    <asp:ImageButton ID="btnAddBox" runat="server" ImageUrl="img/box.png" Width="25"
                        Height="15" OnClientClick="ShowLoadingGif();" OnClick="btnAddBox_Click" />
                    <asp:ImageButton ID="btnAddImage" runat="server" ImageUrl="img/image.gif" Width="25"
                        Height="15" OnClientClick="ShowLoadingGif();" OnClick="btnAddImage_Click" />
                </td>
            </tr>
        </table>
        <%-- </ContentTemplate>
        </asp:UpdatePanel>--%>
        <div id="lightbox-panel">
            <asp:Panel ID="pnlTreeEdit" runat="server" DefaultButton="btnSave">
                <a id="close-panel3">
                    <div style="float: right;">
                        <asp:ImageButton ID="imgClose" runat="server" ImageUrl="img/close.gif" OnClick="imgClose_Click" /></div>
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
                        <div id="txtBoxDiv" class="titleC">
                            <asp:UpdatePanel ID="updFreeTextBox" runat="server">
                                <Triggers>
                                    <asp:AsyncPostBackTrigger ControlID="TreeView1" EventName="SelectedNodeChanged" />
                                </Triggers>
                                <ContentTemplate>
                                    <asp:TextBox ID="txtSelectedText" runat="server" BorderStyle="None" ForeColor="white"
                                        Text="TextBox"></asp:TextBox>
                                    <div id="divToolBar" class="divToolBar">
                                        <div style="padding-top: 4px;">
                                            <asp:LinkButton ToolTip="Insert FootNote" ID="lnkFootNote" Height="20" Width="25"
                                                CssClass="LinkButton" runat="server" OnClick="btnFootNote_Click" OnClientClick="return AlertSelectedHtml();">
                                                            <img width="15" height="15" src="img/FNote.gif" alt="Image here"/>
                                            </asp:LinkButton>
                                            <asp:LinkButton ToolTip="Insert Page Break" ID="lnkPageBreak" Height="20" Width="25"
                                                CssClass="LinkButton" runat="server" OnClick="btnPageBreak_Click" OnClientClick="return PageBreakTextSelected();">
                                                            <img width="15" height="15" src="img/PageBreaks.gif" alt="Image here"/>
                                            </asp:LinkButton>
                                            <asp:LinkButton ToolTip="Insert HyperLink" ID="lnkHyperlink" Height="20" Width="25"
                                                OnClientClick="return HyperLinkTextSelected();" CssClass="LinkButton" runat="server"
                                                OnClick="btnHyperLink_Click">
                                                            <img width="15" height="15" src="img/Hyperlink.png" alt="Image here"/>
                                            </asp:LinkButton>
                                            <asp:LinkButton ToolTip="Insert Special Chracter" ID="lnkSpecialChrac" Height="20"
                                                Width="25" OnClientClick="return HyperLinkTextSelected();" CssClass="LinkButton"
                                                runat="server" OnClick="btnSpecialChrac_Click">
                                                            <img width="15" height="15" src="img/Sp_Chracter.png" alt="Image here"/>
                                            </asp:LinkButton>
                                        </div>
                                        <%--<img src="img/FNote.gif" onclick="AlertSelectedHtml();" />--%>
                                    </div>
                                    <telerik:RadEditor runat="server" ID="RadEditor1" Width="450" Height="200" EditModes="Design"
                                        ForeColor="White" Style="background-color: white" OnClientLoad="OnClientLoad">
                                        <CssClasses>
                                            <telerik:EditorCssClass Value="scripts/RadEditor.css" />
                                        </CssClasses>
                                        <Tools>
                                            <telerik:EditorToolGroup>
                                                <%--<telerik:EditorTool Name="ApplySizeColor" Text="Apply Size and Color" />
                                        <telerik:EditorTool Name="InsertCustomDate" Text="Insert Custom Date" />>
                                            <telerik:EditorTool Name="ResetContent" Text="Reset Content" /--%>
                                                <telerik:EditorTool Name="AllCaps" Text="All Caps" />
                                                <telerik:EditorTool Name="AllLower" Text="All Lower" />
                                                <telerik:EditorTool Name="Superscript" Text="Superscript" />
                                                <telerik:EditorTool Name="Subscript" Text="Subscript" />
                                                <telerik:EditorTool Name="Bold" Text="Bold" />
                                                <telerik:EditorTool Name="Italic" Text="Italic" />
                                            </telerik:EditorToolGroup>
                                        </Tools>
                                    </telerik:RadEditor>
                                    <div id="divFootNoteDetail" runat="server" style="text-align: left;">
                                        Add FootNote detail Here:<br />
                                        <asp:TextBox ID="txtFootNote" runat="server" CssClass="txtMultiLine" TextMode="MultiLine"
                                            Width="446"></asp:TextBox><br />
                                        Insert Page Break No:&nbsp
                                        <asp:TextBox ID="txtPageBreakNo" runat="server" Width="50"></asp:TextBox><br />
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </div>
                        <div id="Div1" class="titleC">
                        </div>
                    </div>
                    <br />
                    <div class="noFloat">
                    </div>
                    <div class="formBoxC" style="margin: 10px 10px 10px 0px;">
                        <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" OnClick="btnSave_Click"
                            OnClientClick="hideFreeTextBox();" />
                        <asp:Button ID="btnSplit" runat="server" CssClass="button" Text="Split" OnClientClick="hideFreeTextBox();"
                            OnClick="btnSplit_Click" />
                        <%--<asp:Button ID="Button3" runat="server" CssClass="button" Text="Caps" OnClick="btnCaps_Click"
                        OnClientClick="hideFreeTextBox();" />--%>
                    </div>
                </div>
            </asp:Panel>
            <!--set - region -->
            <asp:Panel ID="pnlTableConf" runat="server" DefaultButton="btnYesTable">
                <div id="SetRegion" class="l-box">
                    <div class="formBoxC">
                        <asp:Label runat="server" ID="lblCOnfirmation" Text="The Current Table does not exist. Do you want to add as extra?" />
                        <br />
                        <br />
                        <asp:Button ID="btnYesTable" runat="server" OnClick="btnYesTable_Click" Text="Yes"
                            OnClientClick="HideTableConfirmation();" />
                        <%--<asp:Button ID="btnNoTable" runat="server" Text="No" />--%>
                        <input type="button" value="No" onclick="HideTableConfirmation();" />
                    </div>
                    <div class="noFloat">
                    </div>
                    <br />
                </div>
            </asp:Panel>
            <!--set - region -->
            <asp:Panel ID="pnlImage" runat="server" DefaultButton="btnImageSave">
                <div id="imageBox" class="titleC">
                    <div class="titleC">
                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                            <Triggers>
                                <asp:AsyncPostBackTrigger ControlID="TreeView1" EventName="SelectedNodeChanged" />
                                <asp:PostBackTrigger ControlID="btnImageSave" />
                            </Triggers>
                            <ContentTemplate>
                                <table width="600" align="center">
                                    <tr>
                                        <td align="left">
                                            Path&nbsp;&nbsp;&nbsp;&nbsp; :<asp:TextBox ID="txtImageURL" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Caption :<asp:TextBox ID="txtImageCaption" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="left">
                                            Upload&nbsp; :<input type="file" runat="server" id="File1" style="width: 150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Button ID="btnImageSave" runat="server" OnClick="btnImageSave_Click" Text="Save"
                                                OnClientClick="HideImagePanel();" />
                                            <input type="button" value="Cancel" onclick="HideImagePanel();" />
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                    <div class="noFloat">
                    </div>
                    <br />
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlBox" runat="server">
                <div id="divBox" class="l-box">
                    <br />
                    <br />
                    <table width="100%">
                        <tr>
                            <td>
                                Box Title:
                            </td>
                            <td>
                                <asp:TextBox ID="txtBoxTitle" runat="server" Width="300"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Box Border&nbsp&nbsp<asp:CheckBox ID="chkBoxBorder" runat="server" />
                            </td>
                            <td>
                                Box BackColor&nbsp&nbsp<asp:CheckBox ID="chhBackColor" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2">
                                <asp:Button ID="btnBoxSave" runat="server" Text="Save" CssClass="button" OnClick="btnBoxSave_Click" />
                                <asp:Button ID="btnBoxCancel" runat="server" Text="Cancel" CssClass="button" />
                            </td>
                        </tr>
                    </table>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlParaConvert" runat="server">
                <div id="divParaConvert" class="l-box">
                    <br />
                    <asp:UpdatePanel ID="uPanelParaConvert" runat="server" UpdateMode="Conditional">
                        <ContentTemplate>
                            <table width="100%">
                                <tr>
                                    <td colspan="3">
                                        <asp:RadioButton ID="rbUpara" Checked="true" AutoPostBack="true" runat="server" GroupName="gpPara"
                                            Text="Upara" OnCheckedChanged="rbUpara_CheckedChanged" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rbNpara" AutoPostBack="true" runat="server" GroupName="gpPara"
                                            Text="Npara" OnCheckedChanged="rbNpara_CheckedChanged" />
                                    </td>
                                    <td>
                                        <asp:CheckBox ID="chkHasNumbers" runat="server" Text="Has Numbers" />
                                    </td>
                                    <td>
                                        Start no:
                                        <asp:DropDownList ID="ddlHasStartNo" runat="server" Width="40">
                                            <asp:ListItem Text="1" Value="1"></asp:ListItem>
                                            <asp:ListItem Text="i" Value="i"></asp:ListItem>
                                            <asp:ListItem Text="a" Value="a"></asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:DropDownList ID="ddlSign" runat="server" Width="40">
                                            <asp:ListItem Text="." Value="."></asp:ListItem>
                                            <asp:ListItem Text=")" Value=")"></asp:ListItem>
                                            <asp:ListItem Text="-" Value="-"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:RadioButton ID="rbSpara" AutoPostBack="true" runat="server" OnCheckedChanged="rbSpara_CheckedChanged"
                                            GroupName="gpPara" Text="Spara" />
                                    </td>
                                    <td colspan="2">
                                        <asp:DropDownList ID="ddlSparaType" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSparaType_SelectedIndexChanged">
                                            <asp:ListItem Text="letter" Value="letter"></asp:ListItem>
                                            <asp:ListItem Text="quotation" Value="Qutation"></asp:ListItem>
                                            <asp:ListItem Text="poem" Value="poem"></asp:ListItem>
                                            <asp:ListItem Text="verse" Value="verse"></asp:ListItem>
                                            <asp:ListItem Text="other" Value="other"></asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp
                                        <asp:DropDownList ID="ddlSparaSubType" runat="server">
                                            <asp:ListItem Text="para" Value="para"></asp:ListItem>
                                            <asp:ListItem Text="line" Value="line"></asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp
                                        <asp:DropDownList ID="ddlSparaOrientation" Enabled="false" runat="server">
                                            <asp:ListItem Text="right" Value="right"></asp:ListItem>
                                            <asp:ListItem Text="left" Value="left"></asp:ListItem>
                                            <asp:ListItem Text="center" Value="center"></asp:ListItem>
                                        </asp:DropDownList>
                                        &nbsp
                                        <asp:DropDownList ID="ddlSparaBackground" Enabled="false" runat="server">
                                            <asp:ListItem Text="none" Value="none"></asp:ListItem>
                                            <asp:ListItem Text="gray" Value="gray"></asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td colspan="2">
                                        <asp:CheckBox ID="chkStanza" runat="server" Text="Stanza" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" align="right">
                                        <asp:Button ID="btnConvert" runat="server" OnClick="btnConvert_Click" Text="Convert"
                                            CssClass="button" />
                                        <asp:Button ID="btnConvertCancel" runat="server" Text="Cancel" CssClass="button" />
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </asp:Panel>
            <asp:Panel ID="pnlMessage" runat="server">
                <asp:UpdatePanel ID="upnlMessagePortion" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnConvert" EventName="Click" />
                    </Triggers>
                    <ContentTemplate>
                        <div id="divMessageBox" class="l-box">
                            <table>
                                <tr>
                                    <td align="center">
                                        <asp:Label ID="lblMessage" runat="server" Text="Some Error Here"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center">
                                        <br />
                                        <br />
                                        <asp:Button ID="btnMessageOk" runat="server" Text="Ok" CssClass="button" OnClick="btnMessageOk_Click" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:Panel ID="pnlSectionType" runat="server">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="divSectionType" class="l-box">
                            <br />
                            <br />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
            <asp:Panel ID="pnlRootNodeActions" runat="server">
                <asp:UpdatePanel ID="upanelRootNode" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <div id="divRootNodeActions" class="l-box">
                            <ul id="verticalmenu" class="glossymenu">
                                <li>
                                    <asp:LinkButton ID="lnkConvert" Text="Convert" runat="server" OnClick="lnkConvert_Click"></asp:LinkButton>
                                </li>
                                <li>
                                    <asp:LinkButton ID="lnkAddUpara" Text="Add Upara" runat="server" OnClick="lnkAddUpara_Click"></asp:LinkButton></li>
                                <li><a href="">Add Section</a>
                                    <ul>
                                        <li>
                                            <asp:LinkButton ID="lnklevel1" Text="level 1" runat="server" OnClick="lnklevel1_Click"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnklevel2" Text="level 2" runat="server" OnClick="lnklevel2_Click"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnklevel3" Text="level 3" runat="server" OnClick="lnklevel3_Click"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnklevel4" Text="level 4" runat="server" OnClick="lnklevel4_Click"></asp:LinkButton></li>
                                        <li><a>--------------------</a> </li>
                                        <li>
                                            <asp:LinkButton ID="lnkChapter" Text="Chapter" runat="server" OnClick="lnkChapter_Click"></asp:LinkButton></li>
                                        <li><a>--------------------</a> </li>
                                        <li>
                                            <asp:LinkButton ID="lnkPart" Text="Part" runat="server" OnClick="lnkPart_Click"></asp:LinkButton></li>
                                        <li>
                                            <asp:LinkButton ID="lnkOther" Text="Other" runat="server" OnClick="lnkOther_Click"></asp:LinkButton></li>
                                    </ul>
                                </li>
                            </ul>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </asp:Panel>
        </div>
        <div id="lightbox-panel1">
            <img id="imgLoading" src="img/loading.gif" alt="" />
        </div>
        <div id="lightbox">
        </div>
    </asp:Panel>
    <asp:HiddenField ID="dfSelectedNode" runat="server" />
    <%-- </ContentTemplate>
    </asp:UpdatePanel>--%>
</asp:Content>

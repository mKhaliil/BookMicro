<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true"
    Async="true" CodeBehind="ComplexBitsMapping.aspx.cs" Inherits="Outsourcing_System.ComplexBitsMapping" %>

<%@ Register TagPrefix="cc1" Namespace="PdfViewer" Assembly="PdfViewer" %>

<%@ Register Src="UserControls/CommonControl/ucShowMessage.ascx" TagName="ucShowMessage" TagPrefix="uc1" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">

    <style type="text/css">
        .style1 {
            height: 34px;
        }

        .style2 {
            width: 155px;
        }

        .auto-style1 {
            width: 220px;
        }

        #ibtnCloseDialog {
            width: 86px;
        }
    </style>

    <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.10.1/jquery.min.js"></script>

    <script src="scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="FinalStyles/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" type="text/css" />
    <script src="FinalScripts/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>

    <script type="text/javascript">

        function UncheckOthers(objchkbox) {
            var objchkList = objchkbox.parentNode.parentNode.parentNode;
            var chkboxControls = objchkList.getElementsByTagName("input");
            for (var i = 0; i < chkboxControls.length; i++) {
                if (chkboxControls[i] != objchkbox && objchkbox.checked) {
                    chkboxControls[i].checked = false;
                }
            }
        }

        $(function () {

            if ($("#<%= ddlParaType.ClientID %>")[0].selectedIndex == 10) {

                $("#<%= cbxEndOfChap.ClientID %>").parent().css({ "display": "inline-block" });
                $("#sparaOptions").css("display", "none");
            }

            $("#<%= ddlParaType.ClientID %>").change(function () {
                if ($("#<%= ddlParaType.ClientID %>")[0].selectedIndex == 0) {

                    $("#sparaOptions").css("display", "block");
                    $("#divChkBox").css({ "display": "inline-block" });

                    $("#<%= cbxEndOfChap.ClientID %>").parent().css({ "display": "none" });

                    $("#<%= ddlSparaOrientation.ClientID %>").css({ "display": "none" });
                    $("#<%= ddlSparaBackground.ClientID %>").css({ "display": "none" });
                }
                else if ($("#<%= ddlParaType.ClientID %>")[0].selectedIndex == 10) {

                    $("#<%= cbxEndOfChap.ClientID %>").parent().css({ "display": "inline-block" });
                    $("#sparaOptions").css("display", "none");
                }
                else {

                    $("#sparaOptions").css("display", "none");
                    $("#<%= cbxEndOfChap.ClientID %>").parent().css({ "display": "none" });
                }
            });

            $("#<%= ddlSparaType.ClientID %>").change(function () {

                $("#<%= ddlSparaSubType.ClientID %>").css({ "display": "inline-block" });

                //If spara type is letter, poem or verse then hide para in ddlSparaSubType
                if ($("#<%= ddlSparaType.ClientID %>")[0].selectedIndex == 0 ||
                    $("#<%= ddlSparaType.ClientID %>")[0].selectedIndex == 2 ||
                    $("#<%= ddlSparaType.ClientID %>")[0].selectedIndex == 3) {

                    $("#<%= ddlSparaSubType.ClientID %>").find('option[value="para"]').hide();
                    $("#<%= ddlSparaSubType.ClientID %>").find('option[value="line"]').show();
                    
                    $("#<%= ddlSparaSubType.ClientID %>")[0].selectedIndex = 1;

                    $("#<%= ddlSparaOrientation.ClientID %>").css({ "display": "none" });
                    $("#<%= ddlSparaBackground.ClientID %>").css({ "display": "none" });
                    //$("#<%= chkStanza.ClientID %>").css({ "display": "inline-block" });
                    $("#divChkBox").css({ "display": "inline-block" });
                }

                //If spara type is quotation then hide line in ddlSparaSubType
                else if ($("#<%= ddlSparaType.ClientID %>")[0].selectedIndex == 1) {

                    $("#<%= ddlSparaSubType.ClientID %>").find('option[value="line"]').hide();
                    $("#<%= ddlSparaSubType.ClientID %>").find('option[value="para"]').show();

                    $("#<%= ddlSparaSubType.ClientID %>")[0].selectedIndex = 0;

                    $("#<%= ddlSparaOrientation.ClientID %>").css({ "display": "none" });
                    $("#<%= ddlSparaBackground.ClientID %>").css({ "display": "none" });
                    <%--$("#<%= chkStanza.ClientID %>").css({ "display": "none" });--%>
                    $("#divChkBox").css({ "display": "none" });
            }

            //If spara type is other
            else if ($("#<%= ddlSparaType.ClientID %>")[0].selectedIndex == 4) {

                $("#<%= ddlSparaOrientation.ClientID %>").css({ "display": "inline-block" });
                $("#<%= ddlSparaBackground.ClientID %>").css({ "display": "inline-block" });

                $("#<%= ddlSparaSubType.ClientID %>").find('option[value="line"]').show();
                $("#<%= ddlSparaSubType.ClientID %>").find('option[value="para"]').hide();

                $("#<%= ddlSparaSubType.ClientID %>")[0].selectedIndex = 1;

                <%--$("#<%= chkStanza.ClientID %>").css({ "display": "inline-block" });--%>
                $("#divChkBox").css({ "display": "inline-block" });
              }



                <%--  else if ($("#<%= ddlSparaType.ClientID %>")[0].selectedIndex != 1) {

                   $("#<%= ddlSparaSubType.ClientID %>").find('option[value="line"]').show();
                   $("#<%= ddlSparaSubType.ClientID %>").find('option[value="para"]').hide();
               }--%>

             <%--   else {
                   
                    $("#<%= ddlSparaOrientation.ClientID %>").css({ "display": "none" });
                   $("#<%= ddlSparaBackground.ClientID %>").css({ "display": "none" });
                   $("#<%= ddlSparaSubType.ClientID %>").find('option[value="line"]').show();
                   $("#<%= ddlSparaSubType.ClientID %>").find('option[value="para"]').hide();
                }--%>
            });
        });

        function ShowSParaOptions() {

            $("#sparaOptions").css("display", "block");
            $("#divChkBox").css({ "display": "inline-block" });

            if ($("#<%= ddlSparaType.ClientID %>")[0].selectedIndex == 4) {

                $("#<%= ddlSparaOrientation.ClientID %>").css({ "display": "inline-block" });
                $("#<%= ddlSparaBackground.ClientID %>").css({ "display": "inline-block" });
                $("#<%= ddlSparaSubType.ClientID %>").find('option[value="line"]').show();
                $("#<%= ddlSparaSubType.ClientID %>").find('option[value="para"]').hide();

                $("#<%= ddlSparaSubType.ClientID %>")[0].selectedIndex = 1;

                //$("#<%= chkStanza.ClientID %>").css({ "display": "inline-block" });
                $("#divChkBox").css({ "display": "inline-block" });
            }

            //If spara type is quotation then hide line in ddlSparaSubType
            else if ($("#<%= ddlSparaType.ClientID %>")[0].selectedIndex == 1) {

                $("#<%= ddlSparaSubType.ClientID %>").find('option[value="line"]').hide();
                $("#<%= ddlSparaSubType.ClientID %>").find('option[value="para"]').show();

                $("#<%= ddlSparaSubType.ClientID %>")[0].selectedIndex = 0;

                $("#<%= ddlSparaOrientation.ClientID %>").css({ "display": "none" });
                $("#<%= ddlSparaBackground.ClientID %>").css({ "display": "none" });
                //$("#<%= chkStanza.ClientID %>").css({ "display": "none" });
                $("#divChkBox").css({ "display": "none" });
            }

            //If spara type is letter, poem or verse then hide line in ddlSparaSubType
            else if ($("#<%= ddlSparaType.ClientID %>")[0].selectedIndex == 0 ||
                $("#<%= ddlSparaType.ClientID %>")[0].selectedIndex == 2 ||
                    $("#<%= ddlSparaType.ClientID %>")[0].selectedIndex == 3) {

                $("#<%= ddlSparaSubType.ClientID %>").find('option[value="para"]').hide();
                $("#<%= ddlSparaSubType.ClientID %>").find('option[value="line"]').show();

                $("#<%= ddlSparaSubType.ClientID %>")[0].selectedIndex = 1;

                $("#<%= ddlSparaOrientation.ClientID %>").css({ "display": "none" });
                $("#<%= ddlSparaBackground.ClientID %>").css({ "display": "none" });
                //$("#<%= chkStanza.ClientID %>").css({ "display": "inline-block" });
                $("#divChkBox").css({ "display": "inline-block" });
            }

            <%--  else if ($("#<%= ddlSparaType.ClientID %>")[0].selectedIndex != 1) {

                $("#<%= ddlSparaSubType.ClientID %>").find('option[value="line"]').show();
                $("#<%= ddlSparaSubType.ClientID %>").find('option[value="para"]').hide();
            }--%>

           <%-- else {

                $("#<%= ddlSparaOrientation.ClientID %>").css({ "display": "none" });
                $("#<%= ddlSparaBackground.ClientID %>").css({ "display": "none" });
                $("#<%= ddlSparaSubType.ClientID %>").find('option[value="line"]').show();
                $("#<%= ddlSparaSubType.ClientID %>").find('option[value="para"]').hide();
            }--%>
        }

        function DisableButtons() {
            var inputs = document.getElementsByTagName("INPUT");
            for (var i in inputs) {
                if (inputs[i].type == "button" || inputs[i].type == "submit" || inputs[i].type == "file") {
                    inputs[i].disabled = true;
                }
            }
        }
        window.onbeforeunload = DisableButtons;

        function ShowEndNoteSelDialog() {

            //alert('3333');

            $("#divDialogEndNote").css("display", "block");

            //$(function () {

            $("#divDialogEndNote").dialog({
                appendTo: "#dialogAfterEndNoteSel",
                title: "Select EndNote Chapter",
                height: 450,
                width: 550,
                position: "center",
                resizable: false,
                modal: true
            });
            //});
        };



        //PDFViewerTarget.FilePath = "DisplayPdf.ashx?bid=" + Convert.ToString(Request.QueryString["bid"]) + "&page=" + page;


        function ShowEndNoteOtherPageDialog() {

            //alert('444');

            $("#divDialogEndNote").css("display", "block");
            ShowEndNoteSelDialog();

            $("#divViewPageDialog").css("display", "block");

            //$(function () {

            $("#divViewPageDialog").dialog({
                appendTo: "#dialogAfterViewPage",
                title: "EndNote Chapter Page",
                height: 600,
                width: 600,
                position: "right",
                resizable: false,
                modal: true
            });
            //});
        };

        function CloseResultDialog() {
            $("#ibtnCloseDialog").dialog('close');
        }


        //function CloseResultDialog() {
        //    $("#divDialogEndNote").dialog('close');
        //}

        <%--  function onCategoryChange() {
            if ($("#<%= ddlParaType.ClientID %>")[0].selectedIndex == 7 || $("#<%= ddlParaType.ClientID %>")[0].selectedIndex == 8) {
                __doPostBack('<%= ddlParaType.ClientID %>', $("#<%= ddlParaType.ClientID %>")[0].selectedIndex);
            } else {
                return false;
            }
        }--%>

        <%-- function onCategoryChange() {
           
            if ($("#<%= ddlParaType.ClientID %>")[0].selectedIndex == 10) {
                //alert('0000000');
                __doPostBack('<%= ddlParaType.ClientID %>', $("#<%= ddlParaType.ClientID %>")[0].selectedIndex);
            } else {
                return false;
            }
        }--%>

    </script>

</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">

    <div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">
        <uc1:ucShowMessage ID="ucShowMessage1" runat="server" />
        <%--<asp:LinkButton ID="lbtnFinish" runat="server" Visible="False" OnClick="lbtnFinish_Click">Finish</asp:LinkButton>--%>
    </div>

    <asp:HiddenField runat="server" ID="hfScreenResolution" />
    <asp:Label ID="lblMessage" CssClass="message" Text="" runat="server" Visible="false" />

    <div id="divSparaAssignment" runat="server" style="left: 1%; top: 3%; width: 99%; min-height: 500px;">
        <%--<div id="the-svg" style="display: none"></div>--%>
        <table style="margin-left: auto; margin-right: auto; width: 85%;">
            <tr>
                <td></td>
                <td align="center">
                    <asp:Label ID="lblPage" runat="server"></asp:Label>&nbsp;&nbsp;/&nbsp;
                    <asp:Label ID="lblTotalPages" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="width: 44%; vertical-align: top">
                    <table>
                        <tr>
                            <td>
                                <%--<asp:TextBox  Width="380" Height="380" TextMode="MultiLine"></asp:TextBox>--%>
                                <div contenteditable="true" style="width: 500px; height: 440px; padding: 5px; overflow: auto; margin-top: 0.8%; border: 2px solid gray;" id="divParaText" runat="server">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>Convert to :&nbsp
					<%--<asp:DropDownList ID="ddlParaType" Width="150" runat="server" AutoPostBack="true"
                        OnSelectedIndexChanged="ddlParaType_SelectedIndexChanged">--%>

                                <%-- <asp:DropDownList ID="ddlParaType" Width="150" runat="server" AutoPostBack="True" onchange="return onCategoryChange();"
                            OnSelectedIndexChanged="ddlParaType_SelectedIndexChanged">--%>

                                <asp:DropDownList ID="ddlParaType" Width="100" runat="server">
                                    <asp:ListItem Text="Spara" Value="spara"></asp:ListItem>
                                  <%--  <asp:ListItem Text="level1" Value="level1"></asp:ListItem>
                                    <asp:ListItem Text="level2" Value="level2"></asp:ListItem>
                                    <asp:ListItem Text="level3" Value="level3"></asp:ListItem>
                                    <asp:ListItem Text="level4" Value="level4"></asp:ListItem>
                                    <asp:ListItem Text="chapter" Value="chapter"></asp:ListItem>--%>
                                    <asp:ListItem Text="Upara" Value="upara" Selected="True"></asp:ListItem>
                                    <asp:ListItem Text="Npara" Value="npara"></asp:ListItem>
                                    <asp:ListItem Text="Box" Value="box"></asp:ListItem>
                                    <asp:ListItem Text="FootNote" Value="footnote"></asp:ListItem>
                                    <asp:ListItem Text="EndNote" Value="endnote"></asp:ListItem>
                                </asp:DropDownList>

                                &nbsp<asp:Button ID="btnConvert" runat="server" Text="Convert" OnClick="btnConvert_Click" />

                                <asp:CheckBox ID="cbxEndOfChap" runat="server" Style="margin-left: 1%; display: none;" Text="At End of Chapter" />
                                <asp:CheckBox ID="cbxApplyAll" runat="server" Style="margin-left: 1%;" Text="Apply to all" />

                                <%--  <asp:CheckBox ID="cbxApplyAll" runat="server" stye="margin-left:1%;" Text="Apply to all" />
                                 <asp:CheckBox ID="cbxEndOfChap" runat="server" stye="margin-left:1%;" Text="At End of Chapter" />--%>
                                
                                &nbsp<asp:Button ID="btnUndoMapping" runat="server" Style="margin-left: 2%; display: inline-block;" Text="Undo" OnClick="btnUndoMapping_Click" />

                                &nbsp<asp:Button ID="btnFinishCompMapping" runat="server" Visible="False" Text="Finish" OnClick="btnFinishCompMapping_Click" />
                            </td>
                        </tr>
                        <%--<tr id="sparaOptions" runat="server" visible="false">--%>
                        <tr>
                            <td id="sparaOptions" style="display: none;">
                                <asp:DropDownList ID="ddlSparaType" runat="server">
                                    <asp:ListItem Text="letter" Value="letter"></asp:ListItem>
                                    <asp:ListItem Text="quotation" Value="quotation"></asp:ListItem>
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
                                
		        	<asp:DropDownList ID="ddlSparaOrientation" runat="server">
                        <asp:ListItem Text="right" Value="right"></asp:ListItem>
                        <asp:ListItem Text="left" Value="left"></asp:ListItem>
                        <asp:ListItem Text="center" Value="center"></asp:ListItem>
                    </asp:DropDownList>
                                &nbsp
					<asp:DropDownList ID="ddlSparaBackground" runat="server">
                        <asp:ListItem Text="none" Value="none"></asp:ListItem>
                        <asp:ListItem Text="gray" Value="gray"></asp:ListItem>
                    </asp:DropDownList>
                                <div id="divChkBox">
                                    <asp:CheckBox ID="chkStanza" runat="server" Text="Stanza" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                 <%--<div id="divCbxSplitted" style="display: inline-block;">
                                    <asp:CheckBox ID="cbxSplitted" runat="server" Text="Split" Checked="False"/>
                                </div>
                                <div id="divCbxMergePrevLines" style="display: inline-block;">
                                    <asp:CheckBox ID="cbxMergePrevLines" runat="server" Text="Merge Previous lines" Checked="False"/>
                                </div>
                                 <div id="divCbxMergeNextLines" style="display: inline-block;">
                                    <asp:CheckBox ID="cbxMergeNextLines" runat="server" Text="Merge Next lines" Checked="False"/>
                                </div>
                                 <div id="divCbxDiffType" style="display: inline-block;">
                                    <asp:CheckBox ID="cbxDiffType" runat="server" Text="Different Type" Checked="False"/>
                                </div>--%>
                                
                                 <asp:CheckBoxList ID="cbxLParaConv" runat="Server" RepeatDirection="Horizontal">
                            <asp:ListItem Text="Split" Value="Split" onclick="UncheckOthers(this);"></asp:ListItem>
                            <asp:ListItem Text="Merge Previous" Value="MergePrevious" onclick="UncheckOthers(this);"></asp:ListItem>
                            <asp:ListItem Text="Merge Next" Value="MergeNext" onclick="UncheckOthers(this);"></asp:ListItem>
                                     <asp:ListItem Text="Different Type" Value="DifferentType" onclick="UncheckOthers(this);"></asp:ListItem>
                        </asp:CheckBoxList>
                            </td>
                        </tr>
                    </table>
                </td>


                <td style="width: 56%">

                    <table style="width: 100%; padding: 1px;" cellpadding="1">
                        <tr>
                            <td style="width: 65%; vertical-align: top; padding-top: 1px;">
                                <div id="scrollDiv" style="border: 1px solid #7f9db9; margin-left: 10px;">
                                    <cc1:ShowPdf ID="PDFViewerTarget" runat="server" BorderStyle="None" Height="650px"
                                        Width="100%" />
                                </div>
                            </td>
                        </tr>
                    </table>

                </td>
            </tr>
        </table>
    </div>
    <div id="lightbox">
    </div>

    <div id="divDialogEndNote" style="display: none;">

        <asp:TreeView ID="tvChapters" runat="server" ShowLines="True" CssClass="bbw" ShowCheckBoxes="Root"
            OnSelectedNodeChanged="tvChapters_SelectedNodeChanged" OnTreeNodeDataBound="tvChapters_TreeNodeDataBound">
        </asp:TreeView>

        <div style="width: 90%; padding: 2%; margin-top: 3%;">
            <asp:Label runat="server" ID="lblOtherPage" Style="margin-right: 2%;">Enter Other Page</asp:Label>
            <asp:TextBox runat="server" ID="tbxOtherPage" Style="width: 8%;"></asp:TextBox>
            <asp:Button runat="server" ID="btnViewPage" OnClick="btnViewPage_Click" Text="View Page" Style="width: 20%;" />
            
             <div style="width: 90%; float:left; margin-top:5%">
                <asp:Button runat="server" ID="btnSaveEndNotePage" OnClick="btnSaveEndNotePage_Click" Text="Save EndNote" Style="width: 30%; display: inline-block;" />

            <%--<input type="button" id="ibtnEndNotePage" value="View Page" onclick="ShowEndNoteOtherPageDialog();" />--%>

            <input type="button" id="ibtnCloseDialog" value="Close" style="width: 20%; display: inline;" onclick="CloseResultDialog();" />
            </div>
            
        </div>

    </div>

    <div id="divViewPageDialog" style="display: none;">

        <div id="scrollPageDiv" style="border: 1px solid #7f9db9; margin-left: 10px;">
            <cc1:ShowPdf ID="ShowPdf1" runat="server" BorderStyle="None" Height="650px"
                Width="100%" />
        </div>
    </div>

    <div id="dialogAfterEndNoteSel">
    </div>

    <div id="dialogAfterViewPage">
    </div>
</asp:Content>

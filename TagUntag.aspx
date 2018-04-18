<%@ Page Title="" Language="C#" MasterPageFile="AdminMaster.Master" AutoEventWireup="true"
	Async="true" CodeBehind="TagUntag.aspx.cs" Inherits="Outsourcing_System.TagUntag"
	EnableEventValidation="false" %>

<%@ Register Assembly="PdfViewer" Namespace="PdfViewer" TagPrefix="cc1" %>
<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">


	<script type="text/javascript">

		//$(function () {
		$("#<%= chkAll.ClientID %>").bind("click", function () {
			if ($(this).is(":checked")) {
				$("#<%= cbxUserRanks.ClientID %> input:checkbox").attr("checked", "checked");
			} else {
				$("#<%= cbxUserRanks.ClientID %> input:checkbox").removeAttr("checked");
			}
		});
		$("#<%= cbxUserRanks.ClientID %> input:checkbox").bind("click", function () {
			if ($("#<%= cbxUserRanks.ClientID %> input:checked").length == $("#<%= cbxUserRanks.ClientID %> input:checkbox").length) {
				$("#<%= chkAll.ClientID %>").attr("checked", "checked");
			} else {
				$("#<%= chkAll.ClientID %>").removeAttr("checked");
			}
		});

		$(document).ready(function () {
			$('#<% =btnFinish.ClientID %>').click(function (e) {
				$('#<% =hfScreenResolution.ClientID %>').attr('value', screen.height + "," + screen.width);
				});
		}
		);

	</script>

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
	<script type="text/javascript" src="PDFJS_SVG/pdf.js"></script>
	<%--<script type="text/javascript" src="PDFJS_SVG/minimal.js"></script>--%>
    
    <style type="text/css">
    .modal
    {
        position: fixed;
        top: 0;
        left: 0;
        background-color: black;
        z-index: 99;
        opacity: 0.8;
        filter: alpha(opacity=80);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 100%;
    }
    .loading
    {
        font-family: Arial;
        font-size: 10pt;
        border: 5px solid #67CFF5;
        width: 230px;
        height: 100px;
        display: none;
        position: fixed;
        background-color: White;
        z-index: 999;
    }
</style>

    <script type="text/javascript">
        function ShowProgress() {

            //alert('6666');
            setTimeout(function () {
                var modal = $('<div />');
                modal.addClass("modal");
                $('body').append(modal);
                var loading = $(".loading");
                loading.show();
                var top = Math.max($(window).height() / 2 - loading[0].offsetHeight / 2, 0);
                var left = Math.max($(window).width() / 2 - loading[0].offsetWidth / 2, 0);
                loading.css({ top: top, left: left });
            }, 200);
        }
        //$('form').live("submit", function () {
        //    ShowProgress();
        //});
</script>

	<script type="text/javascript">

		window.onload = function () {
			if (typeof PDFJS === 'undefined') {
				alert('Built version of pdf.js is not found\nPlease run `node make generic`');
				return;
			}
            
			//var loading = $(".loading");
			//loading.hide();
			//ShowLoadingGif();

            //For local system
			PDFJS.workerSrc = "/PDFJS_SVG/pdf.worker.js";

            //for deployment
			//PDFJS.workerSrc = "/BookMicroBeta/PDFJS_SVG/pdf.worker.js";

			var pdfPath = document.getElementById('hfSvgContentPath').value;


			//alert('window.onload started');

			if (pdfPath != '' || pdfPath != null) {

				PDFJS.getDocument(pdfPath)
					.then(function (pdf) {

						// Get div#container and cache it for later use
						var container = document.getElementById("the-svg");

						var counter = 0;

						// Loop from 1 to total_number_of_pages in PDF document
						for (var i = 1; i <= pdf.numPages; i++) {

							// Get desired page
							pdf.getPage(i).then(function (page) {

								var scale = 1.5;
								var viewport = page.getViewport(scale);

								// Set dimensions
								//container.style.width = viewport.width + 'px';
								//container.style.height = viewport.height + 'px';

								var div = document.createElement("div");

								// Set id attribute with page-#{pdf_page_number} format
								div.setAttribute("page", (page.pageIndex + 1));

								// This will keep positions of child elements as per our needs
								div.setAttribute("style", "position: relative");

								// SVG rendering by PDF.js
								page.getOperatorList()
									.then(function (opList) {
										var svgGfx = new PDFJS.SVGGraphics(page.commonObjs, page.objs);
										return svgGfx.getSVG(opList, viewport);
									})
									.then(function (svg) {

								    counter++;
										div.appendChild(svg);
										container.appendChild(div);

										//document.getElementById('hfSvgContent').value = $('#the-svg')[0].outerHTML;

										ShowProgress();

										if (counter == pdf.numPages) {

										    //alert('all pages loaded');
										    //alert(counter);
										    //alert($('#the-svg')[0].outerHTML);
										    document.getElementById('hfSvgContent').value = $('#the-svg')[0].outerHTML;

										    //var loading = $(".loading");
										    //loading.hide();

										    //window.location.href = '<%= Page.ResolveUrl("~/AdminPanel.aspx") %>';

										    SaveSvgAsXml();
										}
									});
							});
						}
					});
			}

			//alert('loaded end');
			//SaveSvgXml();
		};

	  

		//function EndRequestHandler(sender, args) {
		//    alert('// do your printing logic here');
		//}

		function DisableButtons() {
			var inputs = document.getElementsByTagName("INPUT");
			for (var i in inputs) {
				if (inputs[i].type == "button" || inputs[i].type == "submit" || inputs[i].type == "file") {
					inputs[i].disabled = true;
				}
			}
		}
		window.onbeforeunload = DisableButtons;

		function SaveSvgAsXml() {
		    $('#<%= btnSaveSvgXml.ClientID %>').click();
		}

	</script>

	<script src="scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
	<script src="scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
	<link href="FinalStyles/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" type="text/css" />
	<script src="FinalScripts/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>


	<script type="text/javascript">

		function ShowEndNoteSelDialog() {

			$("#divDialogEndNote").css("display", "block");

			//$(function () {

			$("#divDialogEndNote").dialog({
				appendTo: "#dialogAfterEndNoteSel",
				title: "Select EndNote Chapter",
				height: 430,
				width: 450,
				position: "center",
				resizable: false,
				modal: true
			});
			//});
		};

		//function CloseResultDialog() {
		//    $("#divDialogEndNote").dialog('close');
		//}

	</script>

</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
	<%--<asp:UpdatePanel ID="upFontType" runat="server" UpdateMode="Conditional">
		<ContentTemplate>--%>

	<asp:HiddenField ID="hfSvgContent" ClientIDMode="Static" runat="server" />
	<asp:HiddenField ID="hfSvgContentPath" ClientIDMode="Static" runat="server" Value="WebHandlers/GetPdfInSVG.ashx" />

	<asp:HiddenField runat="server" ID="hfScreenResolution" />
	<asp:Label ID="lblMessage" CssClass="message" Text="" runat="server" Visible="false" />

	 <asp:Button ID="btnSaveSvgXml" runat="server" OnClick="btnSaveSvgXml_Click"
				AccessKey="e" ClientIDMode="Static" Style="display: none" />
    
    <div id="the-svg" style="display: block;"></div>
    <%--<div id="the-svg" style="display: none"></div>--%>

	<table style="width: 100%; padding: 10px;" cellpadding="4">
		<tr>
			<td style="width: 30%">
				<div id="divFontsAssignment" runat="server" style="width: 450px;">
					<div style="text-align: left; height: 15px;">
						Pdf Page No's
					</div>
					<table width="100%" style="border: 1px solid #7f9db9;" cellpadding="2" cellspacing="4">
						<tr>
							<td></td>
							<td>Start
							</td>
							<td>End
							</td>
						</tr>
						<tr>
							<td>Pre Section:
							</td>
							<td>
								<asp:TextBox ID="txtPreSectionStart" runat="server" Style="border: 1px solid #003366; height: 25px; width: 90px; border-radius: 5px"></asp:TextBox>
								<asp:RequiredFieldValidator ID="rvPreSectionStart" Text="*" runat="server" ControlToValidate="txtPreSectionStart"
									ValidationGroup="vgPageNo" Display="Static"></asp:RequiredFieldValidator>
							</td>
							<td>
								<asp:TextBox ID="txtPreSectionEnd" runat="server" Style="border: 1px solid #003366; height: 25px; width: 90px; border-radius: 5px"></asp:TextBox>
								<asp:RequiredFieldValidator ID="rvPresectionEnd" Text="*" runat="server" ControlToValidate="txtPreSectionEnd"
									ValidationGroup="vgPageNo" Display="Static"></asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td>Body:
							</td>
							<td>
								<asp:TextBox ID="txtBodyStart" runat="server" Style="border: 1px solid #003366; height: 25px; width: 90px; border-radius: 5px"></asp:TextBox>
								<asp:RequiredFieldValidator ID="rvBodyStart" Text="*" runat="server" ControlToValidate="txtBodyStart"
									ValidationGroup="vgPageNo" Display="Static"></asp:RequiredFieldValidator>
							</td>
							<td>
								<asp:TextBox ID="txtBodyEnd" runat="server" Style="border: 1px solid #003366; height: 25px; width: 90px; border-radius: 5px"></asp:TextBox>
								<asp:RequiredFieldValidator ID="rvBodyEnd" Text="*" runat="server" ControlToValidate="txtBodyEnd"
									ValidationGroup="vgPageNo" Display="Static"></asp:RequiredFieldValidator>
							</td>
						</tr>
						<tr>
							<td>Post Section:
							</td>
							<td>
								<asp:TextBox ID="txtPostSectionStart" runat="server" Style="border: 1px solid #003366; height: 25px; width: 90px; border-radius: 5px"></asp:TextBox>
								<asp:RequiredFieldValidator ID="rvPostSectionStart" Text="*" runat="server" ControlToValidate="txtPostSectionStart"
									ValidationGroup="vgPageNo" Display="Static"></asp:RequiredFieldValidator>
							</td>
							<td>
								<asp:TextBox ID="txtPostSectionEnd" runat="server" Style="border: 1px solid #003366; height: 25px; width: 90px; border-radius: 5px"></asp:TextBox>
								<asp:RequiredFieldValidator ID="rvPostSectionEnd" Text="*" runat="server" ControlToValidate="txtPostSectionEnd"
									ValidationGroup="vgPageNo" Display="Static"></asp:RequiredFieldValidator>
							</td>
						</tr>
                        <tr>
							<td>Page Break:
							</td>
							<td>
								<asp:TextBox ID="txtPageBreak" runat="server" Style="border: 1px solid #003366; height: 25px; width: 90px; border-radius: 5px"></asp:TextBox>
								<asp:RequiredFieldValidator ID="rvPageBreak" Text="*" runat="server" ControlToValidate="txtPostSectionStart"
									ValidationGroup="vgPageNo" Display="Static"></asp:RequiredFieldValidator>
							</td>
							<td>
								
							</td>
						</tr>
						<tr>
							<td colspan="3" style="text-align: center; padding-right: 30px;" class="style1">
								<asp:Button ID="btnOk" runat="server" Text="Done" ValidationGroup="vgPageNo" OnClick="btnOk_Click" />
							</td>
						</tr>
					</table>
					<br />
					<table width="100%" style="border: 1px solid #7f9db9;" cellpadding="2" cellspacing="4">
						<tr>
							<td style="text-align: left" class="normaltext">
								<asp:CheckBox ID="chkIndex" runat="server" Text="Index Task" AutoPostBack="true"
									OnCheckedChanged="chkIndex_CheckedChanged" />
							</td>
							<td style="text-align: right" class="normaltext">Start Page :
							</td>
							<td style="text-align: left" class="normaltext">
								<asp:TextBox ID="txtindexStart" runat="server" Style="border: 1px solid #003366; height: 25px; width: 90px; border-radius: 5px"
									Enabled="false"></asp:TextBox>&nbsp&nbsp
							</td>
							<td style="text-align: right" class="normaltext">End Page :
							</td>
							<td style="text-align: left" class="normaltext">
								<asp:TextBox ID="txtindexEnd" runat="server" Style="border: 1px solid #003366; height: 25px; width: 90px; border-radius: 5px"
									Enabled="false"></asp:TextBox>
							</td>
						</tr>
						<tr>
							<td style="text-align: left" class="normaltext">
								<asp:CheckBox ID="chkImage" runat="server" Text="Image Task" Checked="False"/>
							</td>
                           <td style="text-align: right" class="normaltext">
								<asp:CheckBox ID="cbxSPara" runat="server" Text="SPara" Checked="True" />
							</td>
                            <td style="text-align: right" class="normaltext">
								<asp:CheckBox ID="cbxFootNotes" runat="server" Text="FootNotes" Checked="True" />
							</td>
						</tr>
						<tr>
							<td style="text-align: left" class="normaltext">
								<asp:CheckBox ID="chkTable" runat="server" Text="Table Task"  Checked="False"/>
							</td>
							<td style="text-align: right" class="normaltext">
								<asp:CheckBox ID="cbxAlgo1" runat="server" Text="Green" Checked="True" />
							</td>
							<td style="text-align: right" class="normaltext">
								<asp:CheckBox ID="cbxalgo2" runat="server" Text="Yellow"  Checked="True"/>
							</td>
							<td style="text-align: right" class="normaltext">
								<asp:CheckBox ID="cbxalgo3" runat="server" Text="Blue" Checked="True" />
							</td>
						</tr>
						<tr>
							<td style="text-align: left" class="normaltext">
								<asp:CheckBox ID="cbxNPara" runat="server" Text="NPara" Checked="True" />
							</td>
							<td style="text-align: right" class="normaltext">
								<asp:CheckBox ID="cbxNParaAlgo1" runat="server" Text="Algo 1" Checked="True" />
							</td>
							<td style="text-align: right" class="normaltext">
								<asp:CheckBox ID="cbxNParaAlgo2" runat="server" Text="Algo 2" Checked="True" />
							</td>
						</tr>

						<%--<tr>
							<td colspan="5" style="text-align: left" class="normaltext">
								<asp:CheckBox ID="cbxSPara" runat="server" Text="SPara" Checked="True" />&nbsp&nbsp
							</td>
						</tr>--%>
					</table>
					<table width="100%" style="border: 1px solid #7f9db9; margin-top: 3.5%" cellpadding="2" cellspacing="4">
						<tr>
							<td style="text-align: left" class="style2">Task Priority:
							</td>
							<td style="text-align: left" class="normaltext">
								<asp:DropDownList ID="ddlTasKPriority" runat="server">
									<asp:ListItem Selected="True">Normal</asp:ListItem>
									<asp:ListItem>High</asp:ListItem>
									<asp:ListItem>Urgent</asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
					</table>
					<table width="100%" style="border: 1px solid #7f9db9; margin-top: 3.5%; display:none;" cellpadding="2" cellspacing="4">
						<tr>
							<td style="text-align: left" class="auto-style1">Error Detection is allowed to:
							</td>
							<td style="text-align: left" class="normaltext">

								<asp:CheckBox ID="chkAll" Text="Select All" runat="server" Style="margin-left: 19%;" />
								<asp:CheckBoxList ID="cbxUserRanks" runat="server" Style="margin-left: 18%;">
									<asp:ListItem Text="Trainee Editor" />
									<asp:ListItem Text="Junior Editor" />
									<asp:ListItem Text="Editor" />
									<asp:ListItem Text="Senior Editor" />
									<asp:ListItem Text="Expert Editor" />
								</asp:CheckBoxList>
							</td>
						</tr>
					</table>
					<div style="height: 20px;">
					</div>
					<table width="100%" style="border: 1px solid #7f9db9;" cellpadding="2" cellspacing="4">
						<tr>
							<td colspan="2">Pdf Page No:&nbsp
								<asp:Label ID="lblPageno" runat="server" Text="0"></asp:Label>&nbsp Remaining Fonts
								to be Mapped: &nbsp
								<asp:Label ID="lblRemainingFonts" runat="server" Text="0"></asp:Label>
							</td>
						</tr>
						<tr>
							<td style="width: 30%;">Font:
							</td>
							<td>&nbsp<asp:TextBox ID="txtFontType" runat="server" ReadOnly="true" Style="border: 1px solid #003366; height: 30px; width: 235px; border-radius: 5px"></asp:TextBox>
								<asp:CheckBox ID="chkCaps" runat="server" Checked="false" Text="Caps" />
							</td>
						</tr>
						<tr>
							<td style="width: 30%;">Actual Text:
							</td>
							<td>&nbsp<asp:TextBox ID="txtActualText" runat="server" ReadOnly="true" Style="border: 1px solid #003366; height: 30px; width: 235px; border-radius: 5px"></asp:TextBox>
							</td>
						</tr>
					</table>
					<div style="height: 15px;">
					</div>
					<div style="text-align: left; height: 15px;">
						Mapping
					</div>
					<table width="100%" style="border: 1px solid #7f9db9;" cellpadding="2" cellspacing="4">
						<tr>
							<td style="width: 30%;">Pre Section:
							</td>
							<td>
								<asp:DropDownList ID="ddlPreSection" runat="server" Width="237px" AutoPostBack="true"
									OnSelectedIndexChanged="ddlPreSection_SelectedIndexChanged">
									<asp:ListItem Text="pre-section" Value="pre-section"></asp:ListItem>
									<asp:ListItem Text="chap-prefix" Value="chap-prefix"></asp:ListItem>
									<%--<asp:ListItem Text="pre-section-prefix" Value="pre-section-prefix"></asp:ListItem>--%>
									<asp:ListItem Text="upara" Value="upara"></asp:ListItem>
									<asp:ListItem Text="italic" Value="italic"></asp:ListItem>
									<asp:ListItem Text="level1" Value="level1"></asp:ListItem>
									<asp:ListItem Text="level2" Value="level2"></asp:ListItem>
									<asp:ListItem Text="level3" Value="level3"></asp:ListItem>
									<asp:ListItem Text="level4" Value="level4"></asp:ListItem>
									<asp:ListItem Text="bold" Value="bold"></asp:ListItem>
									<asp:ListItem Text="bold-italic" Value="bold-italic"></asp:ListItem>
									<asp:ListItem Text="section-break" Value="section-break"></asp:ListItem>
									<asp:ListItem Text="end-node" Value="end-node"></asp:ListItem>
                                   <asp:ListItem Text="super-script" Value="super-script"></asp:ListItem>
                                    <asp:ListItem Text="sub-script" Value="sub-script"></asp:ListItem>
                                    <asp:ListItem Text="caption" Value="caption"></asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td style="width: 30%;">Body:
							</td>
							<td>
								<asp:DropDownList ID="ddlBody" runat="server" Width="237px">
									<asp:ListItem Text="book" Value="book"></asp:ListItem>
									<asp:ListItem Text="part" Value="part"></asp:ListItem>
									<asp:ListItem Text="chapter" Value="chapter"></asp:ListItem>
									<asp:ListItem Text="book-prefix" Value="book-prefix"></asp:ListItem>
									<asp:ListItem Text="part-prefix" Value="part-prefix"></asp:ListItem>
									<asp:ListItem Text="chap-prefix" Value="chap-prefix"></asp:ListItem>
									<asp:ListItem Text="level1" Value="level1"></asp:ListItem>
									<asp:ListItem Text="level2" Value="level2"></asp:ListItem>
									<asp:ListItem Text="level3" Value="level3"></asp:ListItem>
									<asp:ListItem Text="level4" Value="level4"></asp:ListItem>
									<asp:ListItem Text="upara" Value="upara"></asp:ListItem>
									<asp:ListItem Text="italic" Value="italic"></asp:ListItem>
									<asp:ListItem Text="bold" Value="bold"></asp:ListItem>
									<asp:ListItem Text="bold-italic" Value="bold-italic"></asp:ListItem>
									<asp:ListItem Text="section-break" Value="section-break"></asp:ListItem>
									<asp:ListItem Text="end-node" Value="end-node"></asp:ListItem>
                                    <asp:ListItem Text="super-script" Value="super-script"></asp:ListItem>
                                    <asp:ListItem Text="sub-script" Value="sub-script"></asp:ListItem>
                                    <asp:ListItem Text="caption" Value="caption"></asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td style="width: 30%;">Post Section:
							</td>
							<td>
								<asp:DropDownList ID="ddlPostSection" runat="server" Width="237px">
									<asp:ListItem Text="post-section" Value="post-section"></asp:ListItem>
									<asp:ListItem Text="chap-prefix" Value="chap-prefix"></asp:ListItem>
									<%--<asp:ListItem Text="post-section-prefix" Value="post-section-prefix"></asp:ListItem>--%>
									<asp:ListItem Text="level1" Value="level1"></asp:ListItem>
									<asp:ListItem Text="level2" Value="level2"></asp:ListItem>
									<asp:ListItem Text="level3" Value="level3"></asp:ListItem>
									<asp:ListItem Text="level4" Value="level4"></asp:ListItem>
									<asp:ListItem Text="upara" Value="upara"></asp:ListItem>
									<asp:ListItem Text="italic" Value="italic"></asp:ListItem>
									<asp:ListItem Text="bold" Value="bold"></asp:ListItem>
									<asp:ListItem Text="bold-italic" Value="bold-italic"></asp:ListItem>
									<asp:ListItem Text="section-break" Value="section-break"></asp:ListItem>
									<asp:ListItem Text="end-node" Value="end-node"></asp:ListItem>
                                    <asp:ListItem Text="super-script" Value="super-script"></asp:ListItem>
                                    <asp:ListItem Text="sub-script" Value="sub-script"></asp:ListItem>
                                    <asp:ListItem Text="caption" Value="caption"></asp:ListItem>
								</asp:DropDownList>
							</td>
						</tr>
						<tr>
							<td colspan="2" style="text-align: right; padding-right: 20px;">
								<asp:Button ID="btnAssign" runat="server" Text="Assign" OnClick="btnAssign_Click1"
									Enabled="false" />
							</td>
						</tr>
					</table>
					<div style="height: 15px;">
					</div>

					<div style="text-align: left; height: 15px;">
						Mapped Fonts
					</div>
					<div style="padding-right: 5px; max-height: 350px; width: 100%; overflow: scroll;">
						<asp:DataList ID="dlistMappedFonts" BorderStyle="Solid" CellSpacing="2" Width="377px"
							BorderColor="#7f9db9" BorderWidth="1px" OnItemDataBound="dlistMappedFonts_ItemDataBound"
							runat="server" OnItemCommand="dlistMappedFonts_ItemCommand">
							<ItemTemplate>
								<table cellpadding="0" width="378px">
									<tr>
										<td>
											<asp:LinkButton ID="lnkFontName" runat="server" Text=""></asp:LinkButton>
										</td>
									</tr>
								</table>
							</ItemTemplate>
						</asp:DataList>
					</div>
					<div style="padding-top: 12px; text-align: right;">
						<asp:Button ID="btnFinish" runat="server" Width="80" Text="Finish" OnClick="btnFinish_Click" Enabled="false" />
                        <%--<asp:Button ID="btnCreateNextTask" runat="server" Width="80" Text="Create Next Task" />--%>
					</div>
				</div>
			</td>
			<td style="width: 65%; vertical-align: top; padding-top: 15px;">
				<div id="scrollDiv" style="border: 1px solid #7f9db9; margin-left: 10px;">
					<cc1:ShowPdf ID="PDFViewerTarget" runat="server" BorderStyle="None" Height="650px"
						Width="100%" />
				</div>
			</td>
		</tr>
	</table>
	<%-- </ContentTemplate>
	</asp:UpdatePanel>--%>
	<%-- <div id="lightbox-panel1">
		<img src="img/loading.gif" alt="" />
	</div>--%>
	<div id="divSparaAssignment" runat="server" visible="false" style="position: absolute; left: 2%; top: 37%; width: 30%; min-height: 500px; border: 2px solid gray;">
		<%--<div id="the-svg" style="display: none"></div>--%>
		<table>
			<tr>
				<td>
					<br />
					<%--<asp:TextBox  Width="380" Height="380" TextMode="MultiLine"></asp:TextBox>--%>
					<div contenteditable="true" style="width: 380px; height: 380px; padding: 5px; overflow: auto;" id="divParaText" runat="server">
					</div>
				</td>
			</tr>
			<tr>
				<td>Convert to :&nbsp
					<asp:DropDownList ID="ddlParaType" Width="150" runat="server" AutoPostBack="true"
						OnSelectedIndexChanged="ddlParaType_SelectedIndexChanged">
						<asp:ListItem Text="Spara" Value="spara"></asp:ListItem>
						<asp:ListItem Text="level1" Value="level1"></asp:ListItem>
						<asp:ListItem Text="level2" Value="level2"></asp:ListItem>
						<asp:ListItem Text="level3" Value="level3"></asp:ListItem>
						<asp:ListItem Text="level4" Value="level4"></asp:ListItem>
						<asp:ListItem Text="chapter" Value="chapter"></asp:ListItem>
						<asp:ListItem Text="Upara" Value="upara" Selected="True"></asp:ListItem>
						<asp:ListItem Text="Npara" Value="npara"></asp:ListItem>
						<asp:ListItem Text="Box" Value="box"></asp:ListItem>
						<asp:ListItem Text="FootNote" Value="footnote"></asp:ListItem>
						<asp:ListItem Text="EndNote" Value="endnote"></asp:ListItem>
					</asp:DropDownList>
					&nbsp<asp:Button ID="btnConvert" runat="server" Text="Convert" OnClick="btnConvert_Click" />
					<asp:CheckBox ID="cbxApplyAll" runat="server" stye="margin-left:1%;" Text="Apply to all" />
				</td>
			</tr>
			<tr id="sparaOptions" runat="server" visible="false">
				<td>
					<asp:DropDownList ID="ddlSparaType" runat="server" AutoPostBack="true">
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
					<asp:CheckBox ID="chkStanza" runat="server" Text="Stanza" />
				</td>
			</tr>
		</table>

		<%--  <asp:Button runat="server" ID="sample" Text="Get Box" OnClick="Sample_Click"/>--%>
	</div>
	<div id="lightbox">
	</div>

	<div id="divDialogEndNote" style="display: none;">

		<asp:TreeView ID="tvChapters" runat="server" ShowLines="True" CssClass="bbw" ShowCheckBoxes="Root"
			OnSelectedNodeChanged="tvChapters_SelectedNodeChanged" OnTreeNodeDataBound="tvChapters_TreeNodeDataBound">
		</asp:TreeView>

		<div style="width:90%;padding:2%; margin-top:3%;">
			<asp:Label runat="server" ID="lblOtherPage" style="margin-right:2%;">Enter Other Page</asp:Label>
			<asp:TextBox runat="server" ID="tbxOtherPage" style="width:14%;"></asp:TextBox>
            <asp:Button runat="server" ID="btnEndNotePage" OnClick="btnEndNotePage_Click" Text="Save" style="width:20%;"/>
            <input type="button" id="ibtnCloseDialog" value="Close"/>
		</div>
	   
	</div>
	<div id="dialogAfterEndNoteSel">
	</div>
    
    <div class="loading" align="center">
    <%--Loading. Please wait.<br />--%>
         Saving Svg Xml. Please wait...<br />
    <br />
    <img src="img/loader.gif" alt="" />
</div>
</asp:Content>


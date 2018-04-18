<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/EditorMaster.Master"
    ClientIDMode="Static" AutoEventWireup="true" CodeBehind="ComparisonTask.aspx.cs"
    Inherits="Outsourcing_System.ComparisonTask" %>

<%@ Register Assembly="PdfViewer" Namespace="PdfViewer" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder0" runat="server">
    <script src="PdftoHtml5Scripts/pdf.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/l10n.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/viewerControl.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/api.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/bidi.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/core.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/canvas.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/charsets.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/cidmaps.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/colorspace.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/compatibility.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/obj.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/crypto.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/debugger.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/evaluator.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/fonts.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/function.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/glyphlist.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/image.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/jpg.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/jpx.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/metadata.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/metrics.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/parser.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/pattern.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/stream.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/util.js" type="text/javascript"></script>
    <script src="PdftoHtml5Scripts/worker.js" type="text/javascript"></script>
    <script type="text/javascript">        PDFJS.workerSrc = 'PdftoHtml5Scripts/worker_loader.js';</script>
    <script src="PdftoHtml5Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>    
    <link href="PdftoHtml5Scripts/ViewerControl.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript">
        /*-----------------------------------------------------------------------*/
       
        function getCompleteLine(currEl) {
            var tempTop = currEl.style["top"];
            var prevTop = "", newTop = "";
            var origCurrEl = currEl;
            allParentHTML = '';
            prevTop = currEl.style["top"];
            if (currEl.previousSibling != null) {
                currEl = currEl.previousSibling;
                newTop = currEl.style["top"];
            }
            while (newTop == prevTop) {
                allParentHTML = currEl.innerHTML + allParentHTML;
                prevTop = currEl.style["top"];
                if (currEl.previousSibling != null) {
                    currEl = currEl.previousSibling;
                    newTop = currEl.style["top"];
                } else {
                    newTop = currEl.style["top"];
                    break;
                }
            } //
            currEl = origCurrEl;
            do {
                allParentHTML += currEl.innerHTML;
                prevTop = currEl.style["top"];
                if (currEl.nextSibling != null) {
                    currEl = currEl.nextSibling;
                    newTop = currEl.style["top"];
                } else {
                    newTop = currEl.style["top"];
                    break;
                }
            } while (newTop == prevTop);
            return allParentHTML;
        }

        /*-----------------------------------------------------------------------*/
        function getSelectionParentsOuterHTML() {
            var parentEl = null;
            var allParentHTML = "";
            if (window.getSelection) {
                var sel = window.getSelection();

                if (sel.rangeCount) {
                    var selElements = new Array();
                    var selElementsTop = new Array();
                    var cuurrEl = null;
                    var cuurrEl2 = null;
                    var tempEl = null;
                    var startTop, endTop;
                    if (sel.getRangeAt(0).startContainer.nextSibling == null) {
                        currEl = sel.getRangeAt(0).startContainer.parentNode;
                    } else {
                        currEl = sel.getRangeAt(0).startContainer;
                    }
                    startTop = currEl.style["top"];
                    if (sel.getRangeAt(0).endContainer.nextSibling == null) {
                        currEl2 = sel.getRangeAt(0).endContainer.parentNode;
                    } else {
                        currEl2 = sel.getRangeAt(0).endContainer;
                    }
                    endTop = currEl2.style["top"];
                    if (endTop == "")
                        currEl2 = sel.getRangeAt(0).endContainer;

                    tempEl = currEl;


                    if (endTop == startTop) {
                        allParentHTML += getCompleteLine(currEl);
                    } else {

                        var endTop1 = endTop.replace('px', ''); //trimmed
                        var startTop1 = startTop.replace('px', ''); //trimmed

                        while (parseFloat(startTop1) <= parseFloat(endTop1)) //Getting all the distinct elements in array
                        {
                            startTop = tempEl.style["top"];
                            startTop1 = startTop.replace('px', '');

                            var chckElementTop = true;
                            for (var x = 0; x <= selElementsTop.length - 1; x++) {
                                var checkElement = selElementsTop[x];
                                if (checkElement == startTop) {
                                    chckElementTop = false;
                                    break;
                                }
                            }

                            if (chckElementTop) {
                                selElements.push(tempEl);
                                selElementsTop.push(tempEl.style["top"]);
                            }
                            tempEl = tempEl.nextSibling;
                            startTop = tempEl.style["top"];
                            startTop1 = startTop.replace('px', '');
                        }

                        var tempCount = true;
                        for (var x = 0; x <= selElementsTop.length - 1; x++) {
                            var lineElement = selElements[x];
                            if (tempCount) {
                                allParentHTML += getCompleteLine(lineElement);
                                tempCount = false;
                            } else {
                                allParentHTML += "<br/>" + getCompleteLine(lineElement);
                            }
                        }
                    }
                }


            } else if (document.selection && document.selection.type != "Control") {
                parentEl = document.selection.createRange().parentElement();
                allParentHTML = parentEl;
            }
            return allParentHTML;
        }

        /*-----------------------------------------------------------------------*/

        function getSelectionCoords() {
            var sel = document.selection, range, rect;
            var x = 0, y = 0;
            if (sel) {
                if (sel.type != "Control") {
                    range = sel.createRange();
                    range.collapse(true);
                    x = range.boundingLeft;
                    y = range.boundingTop;
                }
            } else if (window.getSelection) {
                sel = window.getSelection();
                if (sel.rangeCount) {
                    range = sel.getRangeAt(0).cloneRange();
                    if (range.getClientRects) {
                        range.collapse(true);
                        rect = range.getClientRects()[0];
                        x = rect.left;
                        y = rect.top;
                    }
                    // Fall back to inserting a temporary element
                    if (x == 0 && y == 0) {
                        var span = document.createElement("span");
                        if (span.getClientRects) {
                            // Ensure span has dimensions and position by
                            // adding a zero-width space character
                            span.appendChild(document.createTextNode("\u200b"));
                            range.insertNode(span);
                            rect = span.getClientRects()[0];
                            x = rect.left;
                            y = rect.top;
                            var spanParent = span.parentNode;
                            spanParent.removeChild(span);

                            // Glue any broken text nodes back together
                            spanParent.normalize();
                        }
                    }
                }
            }
            $('#divDialogue').css("display", "block");
            $('#divEditText').css("display", "none");
            $('#cssmenu').css("display", "block");
            $('#divParaConvert').css("display", "none");
            x = x + document.body.scrollLeft
                    + document.documentElement.scrollLeft;
            y = y + document.body.scrollTop
                    + document.documentElement.scrollTop;
            $('#divDialogue').css("left", x + 15);
            $('#divDialogue').css("top", y + 9);
            var selectedText = $('#txtSelectedLineHidden').val().split('<br/>');
            if (selectedText.length > 1) {
                $('#mnuSplit').css("display", "none");
                $('#mnuMerge').css("display", "block");
            }
            else {
                $('#mnuMerge').css("display", "none");
                $('#mnuSplit').css("display", "block");
            }

        }

        /*-----------------------------------------------------------------------*/

        function ShowInnerDiv(divtoshow) {

            switch (divtoshow) {
                case "cssmenu":
                    $('#cssmenu').css("dislay", "block");
                    $('#divEditText').css("dislay", "none");
                    $('#divParaConvert').css("dislay", "none");
                    break;
                case "divEditText":
                    $('#divEditText').css("dislay", "block");
                    $('#cssmenu').css("dislay", "none");
                    $('#divParaConvert').css("dislay", "none");
                    break;
                case "divParaConvert":
                    $('#divParaConvert').css("dislay", "block");
                    $('#divEditText').css("dislay", "none");
                    $('#cssmenu').css("dislay", "none");
                    break;
                default:
                    /*--Nothing to document*/
            }
        };

        /*-----------------------------------------------------------------------*/

        function getSelectionCharOffsetsWithin(element) {
            var start = 0, end = 0;
            var sel, range, priorRange;
            if (typeof window.getSelection != "undefined") {
                range = window.getSelection().getRangeAt(0);
                priorRange = range.cloneRange();
                priorRange.selectNodeContents(element);
                priorRange.setEnd(range.startContainer, range.startOffset);
                start = priorRange.toString().length;
                end = start + range.toString().length;
            } else if (typeof document.selection != "undefined" &&
            (sel = document.selection).type != "Control") {
                range = sel.createRange();
                priorRange = document.body.createTextRange();
                priorRange.moveToElementText(element);
                priorRange.setEndPoint("EndToStart", range);
                start = priorRange.text.length;
                end = start + range.text.length;
            }
            return {
                start: start,
                end: end
            };
        }

        /*-----------------------------------------------------------------------*/

        function alertSelection() {
            var mainDiv = document.getElementById("Html5Viewer");
            var sel = getSelectionCharOffsetsWithin(mainDiv);
            getSelectionCoords();
        }

        /*-----------------------------------------------------------------------*/


        function getMouseXY(event) {
            var tempx = 0;
            var tempy = 0;
            tempX = event.clientX + document.body.scrollLeft
                    + document.documentElement.scrollLeft;
            tempY = event.clientY + document.body.scrollTop
                    + document.documentElement.scrollTop;
            $("#btn").css("left", tempX - 14);
            $("#btn").css("top", tempY - 14);
            return true;
        }
        function fnAddLevel1() {
            $("#btn").css("display", "block");
            $("#btn").css("position", "absolute");
            $("#btn").css("z-index", "158");
            document.onmousemove = getMouseXY;
            var currentpage = $("#txtCurrentPage").val();
            var SelectedLineHidden = $("#txtSelectedLineHidden").val();


            var Data = '{"CurrentPage": "' + currentpage + '","SelectedLineHidden": "' + SelectedLineHidden + '","sectionType": "level1","SectionLevel": "4"}';
            $.ajax({
                type: "POST",
                url: "ComparisonTask.aspx/AddSectionOperation",
                data: Data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
        function fnAddLevel2() {
            $("#btn").css("display", "block");
            $("#btn").css("position", "absolute");
            $("#btn").css("z-index", "158");
            document.onmousemove = getMouseXY;
            var currentpage = $("#txtCurrentPage").val();
            var SelectedLineHidden = $("#txtSelectedLineHidden").val();


            var Data = '{"CurrentPage": "' + currentpage + '","SelectedLineHidden": "' + SelectedLineHidden + '","sectionType": "level2","SectionLevel": "3"}';
            $.ajax({
                type: "POST",
                url: "ComparisonTask.aspx/AddSectionOperation",
                data: Data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
        function fnAddLevel3() {
            $("#btn").css("display", "block");
            $("#btn").css("position", "absolute");
            $("#btn").css("z-index", "158");
            document.onmousemove = getMouseXY;
            var currentpage = $("#txtCurrentPage").val();
            var SelectedLineHidden = $("#txtSelectedLineHidden").val();


            var Data = '{"CurrentPage": "' + currentpage + '","SelectedLineHidden": "' + SelectedLineHidden + '","sectionType": "level3","SectionLevel": "2"}';
            $.ajax({
                type: "POST",
                url: "ComparisonTask.aspx/AddSectionOperation",
                data: Data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
        function fnAddLevel4() {
            $("#btn").css("display", "block");
            $("#btn").css("position", "absolute");
            $("#btn").css("z-index", "158");
            document.onmousemove = getMouseXY;
            var currentpage = $("#txtCurrentPage").val();
            var SelectedLineHidden = $("#txtSelectedLineHidden").val();


            var Data = '{"CurrentPage": "' + currentpage + '","SelectedLineHidden": "' + SelectedLineHidden + '","sectionType": "level4","SectionLevel": "1"}';
            $.ajax({
                type: "POST",
                url: "ComparisonTask.aspx/AddSectionOperation",
                data: Data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
        function fnSplitOperation() {
            $("#btn").css("display", "block");
            $("#btn").css("position", "absolute");
            $("#btn").css("z-index", "158");
            document.onmousemove = getMouseXY;
            var currentpage = $("#txtCurrentPage").val();
            var SelectedLineHidden = $("#txtSelectedLineHidden").val();
            var Data = '{"CurrentPage": "' + currentpage + '","SelectedLineHidden": "' + SelectedLineHidden + '"}';
            $.ajax({
                type: "POST",
                url: "ComparisonTask.aspx/SplitOperation",
                data: Data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
        function fnMergeOperation() {
            $("#btn").css("display", "block");
            $("#btn").css("position", "absolute");
            $("#btn").css("z-index", "158");
            document.onmousemove = getMouseXY;
            var currentpage = $("#txtCurrentPage").val();
            var SelectedLineHidden = $("#txtSelectedLineHidden").val();
            var Data = '{"CurrentPage": "' + currentpage + '","SelectedLineHidden": "' + SelectedLineHidden + '"}';
            $.ajax({
                type: "POST",
                url: "ComparisonTask.aspx/MergeOperation",
                data: Data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
        function SaveEditing() {
            $("#btn").css("display", "block");
            $("#btn").css("position", "absolute");
            $("#btn").css("z-index", "158");
            document.onmousemove = getMouseXY;
            var currentpage = $("#txtCurrentPage").val();
            var SelectedLineHidden = $("#txtSelectedLineHidden").val();
            var txtSelectedWordHidden = $("#txtSelectedWordHidden").val();
            var newText = $("#txtSelectedText").val();

            var Data = '{"CurrentPage": "' + currentpage + '","SelectedLineHidden": "' + SelectedLineHidden + '","SelectedWordHidden": "' + txtSelectedWordHidden + '","UpdatedText": "' + newText + '"}';
            $.ajax({
                type: "POST",
                url: "ComparisonTask.aspx/EditOperation",
                data: Data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                }
            });
        }

        function ConvertParaType() {
            $("#btn").css("display", "block");
            $("#btn").css("position", "absolute");
            $("#btn").css("z-index", "1000");
            document.onmousemove = getMouseXY;
            var currentpage = $("#txtCurrentPage").val();
            var SelectedLineHidden = $("#txtSelectedLineHidden").val();
            var uparatype = $("input:radio[ID=rbUpara]").is(":checked");
            var saparatype = $("input:radio[ID=rbSpara]").is(":checked");
            var nparatype = $("input:radio[ID=rbNpara]").is(":checked");
            var SparaOrientation = $("#ddlSparaOrientation option:selected").text();
            var SparaBackground = $("#ddlSparaBackground option:selected").text();
            var SparaType = $("#ddlSparaType option:selected").text();
            var SparaSubType = $("#ddlSparaSubType option:selected").text();
            var chkStanza = $("input:[ID=chkStanza]").is(":checked");
            var HasNumbers = $("input:[ID=chkHasNumbers]").is(":checked");
            var HasStartNo = $("#ddlHasStartNo option:selected").text();
            var sign = $("#ddlSign option:selected").text();
            var Data = '{"CurrentPage": "' + currentpage + '","SelectedLineHidden": "' + SelectedLineHidden + '","uparatype": "' + uparatype + '","saparatype": "' + saparatype + '","nparatype": "' + nparatype + '","SparaOrientation": "' + SparaOrientation + '","SparaBackground": "' + SparaBackground + '","SparaType": "' + SparaType + '","SparaSubType": "' + SparaSubType + '","chkStanza": "' + chkStanza + '","HasNumbers": "' + HasNumbers + '","HasStartNo": "' + HasStartNo + '","sign": "' + sign + '"}';
            $.ajax({
                type: "POST",
                url: "ComparisonTask.aspx/ConvertParaType",
                data: Data,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: OnSuccess,
                failure: function (response) {
                    alert(response.d);
                }
            });
        }
        function noAction(e) {
            e.preventDefault();
        }
        function OnSuccess(response) {
            $("#btn").css("display", "none");
            $('#divDialogue').css("display", "none");
            alert(response.d);
        }

        /*-----------------------------------------------------------------------*/

        $(document).ready(function () {
            $('#btnSample').click(function () { alertSelection(); });
            $('#Html5Viewer').mouseup(function () {
                if (window.getSelection() != "") {

                    $('#txtSelectedWordHidden').val(window.getSelection());
                    $('#txtSelectedLineHidden').val(getSelectionParentsOuterHTML());
                    alertSelection();
                } else {
                    {
                        $('#cssmenu').css("dislay", "block");
                        $('#divEditText').css("dislay", "none");
                        $('#divDialogue').css("display", "none");
                    }
                }
            });
            $('body').keydown(function (e) {
                if (e.keyCode == 27) {
                    $('#divDialogue').css("display", "none");
                }

            });
            var e = $.Event("keydown", {
                keyCode: 27
            });

            $('body').keypress(function () {
                $("body").trigger(e);
            });
            $('#cssmenu > ul > li > a').click(function () {
                $('#cssmenu li').removeClass('active');
                $(this).closest('li').addClass('active');
                var checkElement = $(this).next();
                if ((checkElement.is('ul')) && (checkElement.is(':visible'))) {
                    $(this).closest('li').removeClass('active');
                    checkElement.slideUp('normal');
                }
                if ((checkElement.is('ul')) && (!checkElement.is(':visible'))) {
                    $('#cssmenu ul ul:visible').slideUp('normal');
                    checkElement.slideDown('normal');
                }
                if ($(this).closest('li').find('ul').children().length == 0) {
                    return true;
                } else {
                    return false;
                }
            });
            $('#mnuEditText').click(function () {
                $('#txtSelectedText').val('');
                $('#divEditText').css("display", "block");
                $('#cssmenu').css("display", "none");
                $('#divParaConvert').css("display", "none");
                $('#txtSelectedText').val(window.getSelection());
            });
            $('#btnCancel').click(function (e) {
                $('#divEditText').css("display", "none");
                $('#cssmenu').css("display", "block");
                $('#divParaConvert').css("display", "none");
                e.preventDefault();
            });

            $('#mnuParaCnvert').bind("click", function (e) {
                $('#divEditText').css("display", "none");
                $('#cssmenu').css("display", "none");
                $('#divParaConvert').css("display", "block");
                e.preventDefault();
            });

            $('#btnConvertCancel').bind("click", function (e) {
                $('#divEditText').css("display", "none");
                $('#cssmenu').css("display", "block");
                $('#divParaConvert').css("display", "none");
                e.preventDefault();
            });
            $('#rbNpara').bind("click", function () {
                if ($('#divSparaOptions').is(':visible')) {
                    $('#divSparaOptions').slideUp("slow");
                };
                if ($("#divNparaoptions").is(":hidden")) {
                    $("#divNparaoptions").slideDown("slow");
                }


            });
            $('#rbSpara').bind("click", function () {
                if ($('#divNparaoptions').is(':visible')) {
                    $('#divNparaoptions').slideUp("slow");
                };
                if ($("#divSparaOptions").is(":hidden")) {
                    $("#divSparaOptions").slideDown("slow");
                }


            });
            $('#btnConvert').bind("click", function (e) {
                ConvertParaType();
                e.preventDefault();
            })
            $('#btnSave').bind("click", function (e) {
                SaveEditing();
                e.preventDefault();
            })
            $('#lnkLevel1').bind("click", function (e) {
                fnAddLevel1();
                e.preventDefault();
            })
            $('#lnkLevel2').bind("click", function (e) {
                fnAddLevel2();
                e.preventDefault();
            })
            $('#lnkLevel3').bind("click", function (e) {
                fnAddLevel3();
                e.preventDefault();
            })
            $('#lnkLevel4').bind("click", function (e) {
                fnAddLevel4();
                e.preventDefault();
            })
            $('#lnkSpit').bind("click", function (e) {
                fnSplitOperation();
                e.preventDefault();
            })
            $('#lnkMerge').bind("click", function (e) {
                fnMergeOperation();
                e.preventDefault();
            })

            $('#rbUpara').bind("click", function () {
                if ($('#divNparaoptions').is(':visible')) {
                    $('#divNparaoptions').slideUp('normal');
                };
                if ($('#divSparaOptions').is(':visible')) {
                    $('#divSparaOptions').slideUp('normal');
                };

            });
            $("#divNavigationPanel").hover(
                function () {
                    $(this).css("height", "90px");
                }, function () {
                    $(this).css("height", "12px");
                });
        });

        /*-----------------------------------------------------------------------*/     

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LinkPortion" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <div>
        <table style="width: 100%;">
            <tr>
                <td>
                    <img id="btn" onclick="noAction()" src="img/Busy.gif" style="height: 40px; width: 40px;
                        display: none;" alt="Edit" /><br />
                    <br />
                    <b>Source File Preview</b>
                </td>
                <td>
                    <b>Target File Preview</b>
                    <%--<strong>(Text Zoom)
                        <button data-id="0">
                            enable</button></strong>--%>
                </td>
            </tr>
            <tr>
                <td style="width: 50%; vertical-align: top;">
                    <cc1:ShowPdf ID="PDFViewerSource" runat="server" BorderWidth="1px" Width="99.5%"
                        Height="845px" />
                </td>
                <td>
                    <div id="Html5Viewer" style="height: 845px; vertical-align: top; border: 1px solid Gray;
                        overflow: scroll;">
                        <asp:HiddenField ID="FileLoadPath" ClientIDMode="Static" runat="server" Value="PdftoHtml5Scripts/Page1.pdf" />
                        <div style="display: none">
                            hello world</div>
                        <div id="sidebarContainer">
                            <div id="toolbarSidebar">
                                <div class="splitToolbarButton toggled">
                                    <button id="viewThumbnail" class="toolbarButton toggled" title="Show Thumbnails"
                                        onclick="PDFView.switchSidebarView('thumbs')" tabindex="1" data-l10n-id="thumbs">
                                        <span data-l10n-id="thumbs_label">Thumbnails</span>
                                    </button>
                                    <button id="viewOutline" class="toolbarButton" title="Show Document Outline" onclick="PDFView.switchSidebarView('outline')"
                                        tabindex="2" data-l10n-id="outline">
                                        <span data-l10n-id="outline_label">Document Outline</span>
                                    </button>
                                </div>
                            </div>
                            <div id="sidebarContent">
                                <div id="thumbnailView">
                                </div>
                                <div id="outlineView" class="hidden">
                                </div>
                            </div>
                        </div>
                        <!-- sidebarContainer -->
                        <div id="mainContainer">
                            <button id="btnEditText" style="display: none;" type="button" onclick="getSelText()">
                                Edit</button>                            
                            <div class="toolbar" style="display: none;">
                                <div id="toolbarContainer">
                                    <div id="toolbarViewer">
                                        <textarea id="taSelText" name="selectedtext" rows="2" cols="20" style="display: none;"></textarea>
                                        <textarea id="taSelParent" name="selectedparent" rows="2" cols="20" style="display: none"></textarea>
                                        <%-- <asp:Button ID="btnGetSelText" runat="server" ClientIDMode="Static" Text="Update"
                                                OnClick="btnGetSelText_Click" OnClientClick="SendXml();" Style="display: none" />
                                            <asp:Button ID="btnSplit" runat="server" Text="Splite" OnClientClick="SendXml();"
                                                ClientIDMode="Static" Style="display: none" OnClick="btnSplit_Click" />
                                            <asp:Button ID="btnMerger" runat="server" Text="Merge" OnClientClick="SendXml();"
                                                ClientIDMode="Static" Style="display: none" OnClick="btnMerge_Click" />
                                            <asp:Button ID="btnLogMistake" runat="server" Text="Log Mistake" OnClientClick="SendXml();"
                                                AccessKey="e" ClientIDMode="Static" Style="display: none" OnClick="btnLogMistake_Click" />--%>
                                        <div id="toolbarViewerLeft">
                                            <button id="sidebarToggle" class="toolbarButton" title="Toggle Sidebar" tabindex="3"
                                                data-l10n-id="toggle_slider" style="display: none;">
                                                <span data-l10n-id="toggle_slider_label">Toggle Sidebar</span>
                                            </button>
                                            <div class="toolbarButtonSpacer">
                                            </div>
                                            <div class="splitToolbarButton">
                                                <button class="toolbarButton pageUp" title="Previous Page" onclick="PDFView.page--"
                                                    id="previous" tabindex="4" data-l10n-id="previous" style="display: none">
                                                    <span data-l10n-id="previous_label">Previous</span>
                                                </button>
                                                <div class="splitToolbarButtonSeparator">
                                                </div>
                                                <button class="toolbarButton pageDown" title="Next Page" onclick="PDFView.page++"
                                                    id="next" tabindex="5" data-l10n-id="next" style="display: none">
                                                    <span data-l10n-id="next_label">Next</span>
                                                </button>
                                            </div>
                                            <label id="pageNumberLabel" class="toolbarLabel" for="pageNumber" data-l10n-id="page_label"
                                                style="display: none">
                                                Page:
                                            </label>
                                            <input type="number" id="pageNumber" class="toolbarField pageNumber" onchange="PDFView.page = this.value;"
                                                value="1" size="4" min="1" tabindex="6" style="display: none"> </input>
                                            <span id="numPages" class="toolbarLabel" style="display: none"></span>
                                        </div>
                                        <div id="toolbarViewerRight">
                                            <input id="fileInput" class="fileInput" type="file" oncontextmenu="return false;"
                                                style="visibility: hidden; position: fixed; right: 0; top: 0" />
                                            <button id="openFile" class="toolbarButton openFile" title="Open File" tabindex="10"
                                                data-l10n-id="open_file" onclick="document.getElementById('fileInput').click()"
                                                style="display: none">
                                                <span data-l10n-id="open_file_label">Open</span>
                                            </button>
                                            <button id="download" class="toolbarButton download" title="Download" onclick="PDFView.download();"
                                                tabindex="12" data-l10n-id="download" style="display: none">
                                                <span data-l10n-id="download_label">Download</span>
                                            </button>
                                            <div style="display: none">
                                                <a href="#" id="viewBookmark" class="toolbarButton bookmark" title="Current view (copy or open in new window)"
                                                    tabindex="13" data-l10n-id="bookmark"><span data-l10n-id="bookmark_label" style="visibility: hidden">
                                                        Current View</span></a></div>
                                        </div>
                                        <div class="outerCenter">
                                            <div class="innerCenter" id="toolbarViewerMiddle">
                                                <div class="splitToolbarButton">
                                                    <button class="toolbarButton zoomOut" title="Zoom Out" onclick="PDFView.zoomOut();"
                                                        tabindex="7" data-l10n-id="zoom_out" style="display: none">
                                                        <span data-l10n-id="zoom_out_label">Zoom Out</span>
                                                    </button>
                                                    <div class="splitToolbarButtonSeparator" style="display: none">
                                                    </div>
                                                    <button class="toolbarButton zoomIn" title="Zoom In" onclick="PDFView.zoomIn();"
                                                        tabindex="8" data-l10n-id="zoom_in" style="display: none">
                                                        <span data-l10n-id="zoom_in_label">Zoom In</span>
                                                    </button>
                                                </div>
                                                <span id="scaleSelectContainer" class="dropdownToolbarButton" style="display: none;">
                                                    <select id="scaleSelect" onchange="PDFView.parseScale(this.value);" title="Zoom"
                                                        oncontextmenu="return false;" tabindex="9" data-l10n-id="zoom">
                                                        <option id="pageAutoOption" value="auto" selected="selected" data-l10n-id="page_scale_auto">
                                                            Automatic Zoom</option>
                                                        <option id="pageActualOption" value="page-actual" data-l10n-id="page_scale_actual">Actual
                                                            Size</option>
                                                        <option id="pageFitOption" value="page-fit" data-l10n-id="page_scale_fit">Fit Page</option>
                                                        <option id="pageWidthOption" value="page-width" data-l10n-id="page_scale_width">Full
                                                            Width</option>
                                                        <option id="customScaleOption" value="custom"></option>
                                                        <option value="0.5">50%</option>
                                                        <option value="0.75">75%</option>
                                                        <option value="1">100%</option>
                                                        <option value="1.25">125%</option>
                                                        <option value="1.5">150%</option>
                                                        <option value="2">200%</option>
                                                    </select>
                                                </span>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <%--<div id="viewerContainer" style="margin-top: 8px;top: -8px;left: 2px;margin-right: 5px;margin-left: -15px;width: 507px;">--%>
                            <div id="viewerContainer" style="margin-top: 8px; top: -8px; left: -2px; margin-right: 5px;
                                margin-left: -3px; width: 100%;">
                                <div id="viewer">
                                </div>
                            </div>
                            <div id="loadingBox">
                                <div id="loading" data-l10n-id="loading" data-l10n-args='{"percent": 0}'>
                                    Loading... 0%</div>
                                <div id="loadingBar">
                                    <div class="progress">
                                    </div>
                                </div>
                            </div>
                            <div id="errorWrapper" hidden='true'>
                                <div id="errorMessageLeft">
                                    <span id="errorMessage"></span>
                                    <button id="errorShowMore" onclick="" oncontextmenu="return false;" data-l10n-id="error_more_info">
                                        More Information
                                    </button>
                                    <button id="errorShowLess" onclick="" oncontextmenu="return false;" data-l10n-id="error_less_info"
                                        hidden='true'>                                        
                                        Less Information
                                    </button>
                                </div>
                                <div id="errorMessageRight">
                                    <button id="errorClose" oncontextmenu="return false;" data-l10n-id="error_close">
                                        Close
                                    </button>
                                </div>
                                <div class="clearBoth">
                                </div>
                                <textarea id="errorMoreInfo" hidden='true' readonly="readonly"></textarea>
                            </div>
                        </div>
                        <!-- mainContainer -->
                    </div>
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogue" style="display: none; border: 4px solid #D6D6D6; position: absolute;
        left: 900px; top: 300px; border-top-right-radius: 20px; border-bottom-right-radius: 20px;
        padding: 6px; border-bottom-left-radius: 20px; background-color: #F0F0F0;">
        <div id='cssmenu'>
            <ul>
                <li id="mnuEditText" class='active'><a href='#'><span>Edit Text</span></a></li>
                <li id="mnuSplit" class='last'>
                    <asp:LinkButton ID="lnkSpit" runat="server" Text="Split"></asp:LinkButton></li>
                <li id="mnuMerge" class='last'>
                    <asp:LinkButton ID="lnkMerge" runat="server" Text="Merge"></asp:LinkButton></li>
                <li id="mnuParaCnvert"><a href='#'><span>Convert</span></a> </li>
                <li class='has-sub'><a href='#'><span>Add Section</span></a>
                    <ul>
                        <li id="mnuLevel1">
                            <asp:LinkButton ID="lnkLevel1" runat="server" Text="Level1"></asp:LinkButton></li>
                        <li id="mnuLevel2" class='last'>
                            <asp:LinkButton ID="lnkLevel2" runat="server" Text="Level2"></asp:LinkButton></li>
                        <li id="mnuLevel3" class='last'>
                            <asp:LinkButton ID="lnkLevel3" runat="server" Text="Level3"></asp:LinkButton></li>
                        <li id="mnuLevel4" class='last'>
                            <asp:LinkButton ID="lnkLevel4" runat="server" Text="Level4"></asp:LinkButton></li>
                    </ul>
                </li>
                <li id="mnuChapter" class='last'><a href='#'><span>Chapter</span></a></li>
            </ul>
        </div>
        <div id="divEditText" style="display: none;">
            <div id="divToolBar" class="divToolBar">
                <div style="padding-top: 4px;">
                    <asp:LinkButton ToolTip="Insert FootNote" ID="lnkFootNote" Height="20" Width="25"
                        CssClass="LinkButton" runat="server">
                                                            <img width="15" height="15" src="img/FNote.gif" alt="Image here"/>
                    </asp:LinkButton>
                    <asp:LinkButton ToolTip="Insert Page Break" ID="lnkPageBreak" Height="20" Width="25"
                        CssClass="LinkButton" runat="server">
                                                            <img width="15" height="15" src="img/PageBreaks.gif" alt="Image here"/>
                    </asp:LinkButton>
                    <asp:LinkButton ToolTip="Insert HyperLink" ID="lnkHyperlink" Height="20" Width="25"
                        CssClass="LinkButton" runat="server">
                                                            <img width="15" height="15" src="img/Hyperlink.png" alt="Image here"/>
                    </asp:LinkButton>
                    <asp:LinkButton ToolTip="Insert Special Chracter" ID="lnkSpecialChrac" Height="20"
                        Width="25" CssClass="LinkButton" runat="server">
                                                            <img width="15" height="15" src="img/Sp_Chracter.png" alt="Image here"/>
                    </asp:LinkButton>
                </div>
                <%--<img src="img/FNote.gif" onclick="AlertSelectedHtml();" />--%>
            </div>
            <div id="divFootNoteDetail" runat="server" style="text-align: left;">
                <asp:TextBox ID="txtSelectedText" runat="server" Width="304" Height="100" TextMode="MultiLine"></asp:TextBox><br />
                Add FootNote detail Here:<br />
                <asp:TextBox ID="txtFootNote" runat="server" CssClass="txtMultiLine" TextMode="MultiLine"
                    Height="30" Width="300"></asp:TextBox><br />
                Insert Page Break No:&nbsp
                <asp:TextBox ID="txtPageBreakNo" runat="server" Width="50"></asp:TextBox><br />
            </div>
            <asp:Button ID="btnSave" runat="server" Text="Save" OnClick="btnSave_Click" />
            <asp:Button ID="btnCancel" runat="server" Text="Cancel" />
        </div>
        <div id="divParaConvert" style="display: none; width: 450px;">
            <table width="100%">
                <tr>
                    <td style="width: 33%;">
                        <asp:RadioButton ID="rbUpara" Checked="true" runat="server" GroupName="gpPara" Text="Upara" />
                    </td>
                    <td style="width: 33%;">
                        <asp:RadioButton ID="rbNpara" runat="server" GroupName="gpPara" Text="Npara" />
                    </td>
                    <td style="width: 33%;">
                        <asp:RadioButton ID="rbSpara" runat="server" GroupName="gpPara" Text="Spara" />
                    </td>
                </tr>
                <tr style="height: 60px;">
                    <td>
                    </td>
                    <td style="vertical-align: top;">
                        <div id="divNparaoptions" style="display: none; border: 1px solid gray; background-color: lightskyblue;">
                            <asp:CheckBox ID="chkHasNumbers" runat="server" Text="Has Numbers" /><br />
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
                        </div>
                        <div style="width: 145px">
                        </div>
                    </td>
                    <td style="vertical-align: top;">
                        <div id="divSparaOptions" style="display: none; border: 1px solid gray; background-color: lightskyblue;">
                            <asp:DropDownList ID="ddlSparaType" runat="server">
                                <asp:ListItem Text="letter" Value="letter"></asp:ListItem>
                                <asp:ListItem Text="quotation" Value="Qutation"></asp:ListItem>
                                <asp:ListItem Text="poem" Value="poem"></asp:ListItem>
                                <asp:ListItem Text="verse" Value="verse"></asp:ListItem>
                                <asp:ListItem Text="other" Value="other"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <asp:DropDownList ID="ddlSparaSubType" runat="server">
                                <asp:ListItem Text="para" Value="para"></asp:ListItem>
                                <asp:ListItem Text="line" Value="line"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <asp:DropDownList ID="ddlSparaOrientation" Enabled="false" runat="server">
                                <asp:ListItem Text="right" Value="right"></asp:ListItem>
                                <asp:ListItem Text="left" Value="left"></asp:ListItem>
                                <asp:ListItem Text="center" Value="center"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <asp:DropDownList ID="ddlSparaBackground" Enabled="false" runat="server">
                                <asp:ListItem Text="none" Value="none"></asp:ListItem>
                                <asp:ListItem Text="gray" Value="gray"></asp:ListItem>
                            </asp:DropDownList>
                            <br />
                            <asp:CheckBox ID="chkStanza" runat="server" Text="Stanza" />
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" align="right">
                        <asp:Button ID="btnConvert" runat="server" Text="Convert" CssClass="button" OnClick="btnConvert_Click" />
                        <asp:Button ID="btnConvertCancel" runat="server" Text="Cancel" CssClass="button" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divNavigationPanel" style="position: fixed; top: 0px; left: 37%; width: 25%;
        border-bottom-right-radius: 20px; padding: 6px; border-bottom-left-radius: 20px;
        color: White; background-color: #565656; height: 10px; overflow: hidden;">
        <br />
        <asp:Button ID="btnFirstpage" runat="server" Text="<<" Width="60px" />
        <asp:Button ID="btnPreviousPage" runat="server" Text="<" Width="60px" OnClick="btnPrevious_Click" />
        <asp:TextBox ID="txtCurrentPage" runat="server" Width="30px" Text=""></asp:TextBox>
        <asp:Label ID="lblTotalPages" runat="server" Text="450"></asp:Label>
        <asp:Button ID="btnNextPage" runat="server" Text=">" Width="60px" OnClick="btnNext_Click" />
        <asp:Button ID="btnLastPage" runat="server" Text=">>" Width="60px" />
        <br />
        <asp:Button ID="btnGenerate" runat="server" Text="Generate Preview" OnClick="btnGenerate_Click" />
        <asp:CheckBox ID="chkRegenrate" runat="server" Text="Reflect Changes" /><br />
        <asp:Label ID="lblMistakesCount" ForeColor="white" runat="server" Visible="true"></asp:Label>
        <br />
        <br />
        <br />
        <br />
        <br />
        <asp:TextBox ID="txtSelectedWordHidden" Text="" runat="server"></asp:TextBox>
        <asp:TextBox ID="txtSelectedLineHidden" Text="" runat="server"></asp:TextBox>
    </div>
    <div id="divMessageBox" runat="server">
        <div style="position: fixed; top: 0px; left: 0px; width: 100%; background-color: #565656;
            min-height: 100%; overflow: hidden; opacity: .8;">
        </div>
        <div style="position: fixed; top: 10%; left: 37%; width: 25%; border-radius: 20px;
            padding: 6px; color: White; background-color: White; border: 3px solid red; min-height: 150px;
            overflow: hidden; vertical-align: middle;">
            <br />
            <br />
            <br />
            <br />
            <asp:Label ID="lblMessage" runat="server" Text="" ForeColor="Red">Message Will show here......</asp:Label>
            <br />
            <br />
            <asp:Button ID="btnOk" runat="server" Text="OK" OnClick="btnOk_Click" />
        </div>
    </div>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ParaSelection.aspx.cs" Inherits="Outsourcing_System.ParaSelection" %>

<%@ Register Src="UserControls/CommonControl/ucShowMessage.ascx" TagName="ucShowMessage" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="FinalScripts/ParaSelectionPdfJs/jquery-1.9.1.js"></script>
    <link href="FinalStyles/minimal.css" rel="stylesheet" type="text/css" />
    <link href="FinalStyles/viewer.css" rel="stylesheet" />
    <script src="FinalScripts/ParaSelectionPdfJs/pdf.js" type="text/javascript"></script>
    <script src="FinalScripts/ParaSelectionPdfJs/ui_utils.js" type="text/javascript"></script>
    <script src="FinalScripts/ParaSelectionPdfJs/text_layer_builder.js" type="text/javascript"></script>
    <script src="FinalScripts/ParaSelectionPdfJs/minimal.js" type="text/javascript"></script>

    <script src="scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <link href="FinalStyles/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" type="text/css" />
    <script src="FinalScripts/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>

    <script src="FinalScripts/ifvisible.js" type="text/javascript"></script>
    <script src="FinalScripts/TImeMe.js" type="text/javascript"></script>

    <style type="text/css">
        .success {
            color: #4F8A10;
            background: #DFF2BF url('img/green-ok.gif') no-repeat 10px center;
        }

        .error {
            color: #D8000C;
            background: #FFBABA url('img/red-error.gif') no-repeat 10px center;
        }

        .selected {
            background-color: #f0f0f0;
        }

        .ui-selecting, .ui-selected {
            background: lightBlue;
        }

        /*table, th, td {
    border: 1px solid black;
    border-collapse: collapse;
}*/
        .auto-style1 {
            width: 8%;
        }

        .auto-style2 {
            height: 90px;
            width: 4%;
        }
    </style>

    <script type="text/javascript">
        
        function GetMissingParaPage() {

            var bookId = document.getElementById('<%= hfParaSelBookId.ClientID %>').value;

            methodURL = "ParaSelection.aspx/GetParaSelMissingPage";
            parameters = "{'bookId':'" + bookId + "'}";

            //var obj = {};
            //obj.text = bookId;

            $.ajax({
                type: "POST",
                //data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                url: methodURL,
                data: parameters,
                dataType: "json",
                async: false,
                success: function (data) {

                    if (data.d != null) {

                        $('#<%=lblParaInserted.ClientID%>').html(data.d);
                    } else {
                        
                              $("#divFinishTaskConfirm").css("margin-top", "1%");

                    }
                }
            });
        }

        function showProductionPanel() {

            $('#<%= btnInternal.ClientID %>').click();
        }

        function showConfirmationDialog() {

            $("#divFinishTaskConfirm").css("display", "block");

                GetMissingParaPage();
                $("#skipConfirmDialog").dialog({
                    resizable: false,
                    height: 230,
                    width: 550,
                    modal: true,
                    buttons: {
                        "Yes": function () {
                            $(this).dialog("close");
                            showProductionPanel();
                        },
                        "No": function () {
                            $(this).dialog("close");
                        }
                    },
                    beforeClose: function (event, ui) {
                        //$("#btnGoTo").hide();
                        //$("#btnFinish_Task").hide();
                    }
               });
        }

        $(function () {

            $("#divNavigationPanel").hover(
                function () {
                    $(this).css("height", "10%");
                    $(this).css({ 'z-index': '999' });

                }, function () {
                    $(this).css("height", "0.01%");
                    $(this).css({ 'z-index': '999' });
                });
        });

        function getSelectedHtml() {
            var range;
            if (document.selection && document.selection.createRange) {
                range = document.selection.createRange();
                return range.htmlText;
            }
            else if (window.getSelection) {
                var selection = window.getSelection();
                if (selection.rangeCount > 0) {
                    range = selection.getRangeAt(0);
                    var clonedSelection = range.cloneContents();
                    var div = document.createElement('div');
                    div.appendChild(clonedSelection);
                    return div.innerHTML;
                }
                else {
                    return '';
                }
            }
            else {
                return '';
            }
        }

        //function getSelectionText() {
        //    var text = "";

        //    //var text = $('.textLayer').text();

        //    if (window.getSelection) {
        //        text = window.getSelection().toString();
        //    } else if (document.selection && document.selection.type != "Control") {
        //        text = document.selection.createRange().text;
        //    }
        //    return text;
        //}

        //function SetDimensions() {
        //    alert('11222');
        //}
        //window.onbeforeunload = SetDimensions;

        function ShowMessege(updateStatus) {

            if (updateStatus == true) {
                ShowSuccessMessege();
            }
            else if (updateStatus == false) {
                ShowErrorMessege();
            }
        }

        function ShowSuccessMessege() {

            $('#divSuccessMessage').html('');
            $('#divSuccessMessage').append('Table is saved successfully.');
            $("#divSuccessMessage").display = 'block';
            $("#divSuccessMessage").slideDown("slow");
            setTimeout(function () { $("#divSuccessMessage").slideUp("slow"); }, 5000);
        }

        function ShowErrorMessege() {

            $('#divErrorMessage').html('');
            $('#divErrorMessage').append('Sorry! Some error has occured while updating xml.');
            $("#divErrorMessage").display = 'block';
            $("#divErrorMessage").slideDown("slow");
            setTimeout(function () { $("#divErrorMessage").slideUp("slow"); }, 10000);
        }

        $(function () {

            document.onkeydown = function (e) {

                // ctrl + e (e = 69)

                if (e.ctrlKey === true) {
                    if (e.keyCode === 69) {
                        //                alert('dddddddddddddd');

                        //alert(getSelectedHtml());
                        e.preventDefault();
                        LogMistakeCallBack();
                    }
                }

                // ctrl + Right arrow (Right arrow 39)
                if (e.ctrlKey === true) {
                    if (e.keyCode === 39) {
                        //                alert('NavigateNextPageCallBack');
                        e.preventDefault();
                        NavigateNextPageCallBack();
                    }
                }

                // ctrl + Left arrow (Left arrow 	37)
                if (e.ctrlKey === true) {
                    if (e.keyCode === 37) {
                        //                alert('NavigatePrevPageCallBack');
                        e.preventDefault();
                        NavigatePrevPageCallBack();
                    }
                }

                // ctrl + m (e = 77)
                if (e.ctrlKey === true) {
                    if (e.keyCode === 77) {
                        //                alert('NavigatePrevPageCallBack');
                        e.preventDefault();
                        mergeRows();
                    }
                }

                // ctrl + q (e = 81)
                if (e.ctrlKey === true) {
                    if (e.keyCode === 81) {
                        //                alert('NavigatePrevPageCallBack');
                        e.preventDefault();
                        mergeRows();
                    }
                }
            };
        });

        <%--   function LogMistakeCallBack() {
            $('#<%= btnMarkTableLines.ClientID %>').click();
        }--%>
        function NavigateNextPageCallBack() {
            $('#<%= btnNextPage.ClientID %>').click();
        }
        function NavigatePrevPageCallBack() {
            $('#<%= btnPrevPage.ClientID %>').click();
        }



        function MarkTableHeaderCapInXml(RowType) {

            //var parentElement = getSelectionParentsOuterHTML();
            //var selectedTextLine = getInnerHTML(parentElement);
            //alert(selectedTextLine);

            var selectedTextLine = getSelectedHtml();

            //alert(selectedTextLine);

            if (selectedTextLine != null || selectedTextLine != '') {

                var obj = {};

                obj.text = selectedTextLine + "/~/" + RowType;

                methodURL = "Process1.aspx/MarkTableHeaderCaption";
                var returnVal = false;

                $.ajax({
                    type: "POST",
                    data: JSON.stringify(obj),
                    contentType: "application/json; charset=utf-8",
                    url: methodURL,
                    dataType: "json",
                    async: false,
                    success: function (data) {
                        $('#divMarkWholeTable').css("dislay", "none");
                        $('#divMarkTableHeadCap').css("dislay", "none");
                        $('#divEditText').css("dislay", "none");
                        $('#divDialogue').css("display", "none");
                    },
                    error: function (jqXHR, textStatus, errorThrown) {

                        alert('error');
                    },
                    success: function (data) {

                        <%--if (RowType == 'tableHeader')
                                document.getElementById('<%= hfSetManualTableHeader.ClientID %>').value = 'false';
                            else if (RowType == 'tableCaption')
                                document.getElementById('<%= hfSetManualTableCaption.ClientID %>').value = 'false';--%>

                        ShowMessege(data.d);
                    }
                });
            }
        }

        function MarkUParaInPdf() {

            var uParaLines = getSelectedHtml();

            if (uParaLines != null && uParaLines != '') {
                document.getElementById('<%= hfMarkedUParaText.ClientID %>').value = uParaLines;
            }
        }

        function MarkSParaInPdf() {

            var sParaLines = getSelectedHtml();

            if (sParaLines != null && sParaLines != '') {
                document.getElementById('<%= hfMarkedSParaText.ClientID %>').value = sParaLines;
            }
        }

        function FindPosition(oElement) {

            if (typeof (oElement.offsetParent) != "undefined") {
                for (var posX = 0, posY = 0; oElement; oElement = oElement.offsetParent) {
                    posX += oElement.offsetLeft;
                    posY += oElement.offsetTop;
                }
                return [posX, posY];
            }
            else {
                return [oElement.x, oElement.y];
            }
        }

        function getInnerHTML(ParentNodeHTML) {
            var div = document.createElement('div');
            div.innerHTML = ParentNodeHTML;
            var HTMLWithBreaks = div.textContent;
            try {
                var cleaned = HTMLWithBreaks.replace(/<br[ ]?[/]>/g, "\n");
            }
            catch (e) {
                document.write(e);
                alert(e);
            }
            return cleaned;
        }

        function getSelectionParentsOuterHTML() {
            var parentEl = null;
            if (window.getSelection) {
                var sel = window.getSelection();
                var allParentHTML = "";
                if (sel.rangeCount) {
                    /*if (sel.getRangeAt(0).endContainer == sel.getRangeAt(0).startContainer) //Only one div is selected
                        {
                            parentEl = sel.getRangeAt(0).commonAncestorContainer;
                            if (parentEl.nodeType != 1) {
                                parentEl = parentEl.parentNode;
                                //allParentHTML = parentEl;
                                allParentHTML = parentEl.outerHTML;
                            }
                        }
                        else */
                    {
                        var selElements = new Array();
                        var selElementsTop = new Array();

                        var cuurrEl = null;
                        var cuurrEl2 = null;
                        var tempEl = null;
                        var startTop, endTop;

                        if (sel.getRangeAt(0).startContainer.nextSibling == null) {
                            currEl = sel.getRangeAt(0).startContainer.parentNode;
                        }
                        else {
                            currEl = sel.getRangeAt(0).startContainer;
                        }

                        startTop = currEl.style["top"];

                        if (sel.getRangeAt(0).endContainer.nextSibling == null) {
                            currEl2 = sel.getRangeAt(0).endContainer.parentNode;
                        }
                        else {
                            currEl2 = sel.getRangeAt(0).endContainer;
                        }
                        endTop = currEl2.style["top"];
                        if (endTop == "")
                            currEl2 = sel.getRangeAt(0).endContainer;

                        tempEl = currEl;


                        if (endTop == startTop) {
                            allParentHTML += getCompleteLine(currEl);
                        } else {

                            var endTop1 = endTop.replace('px', '');//trimmed
                            var startTop1 = startTop.replace('px', '');//trimmed

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
                                    allParentHTML += "&lt;br/&gt;" + getCompleteLine(lineElement);
                                }
                            }
                        }
                    }

                }
            } else if (document.selection && document.selection.type != "Control") {
                parentEl = document.selection.createRange().parentElement();
                allParentHTML = parentEl;
            }
            //        if(allParentHTML.indexOf("&lt;br/&gt;")!=-1)
            //                           {
            //                           allParentHTML+=" khalil";
            //   }     
            return allParentHTML;
        }

        function getCompleteLine(currEl) {
            var tempTop = currEl.style["top"];
            //alert($('[top='+tempTop+']'));

            var prevTop = "", newTop = "";
            //var cuurrEl  = null;
            var origCurrEl = currEl;
            //currEl = sel.getRangeAt(0).startContainer.parentNode;
            //alert(currEl.outerHTML);
            allParentHTML = '<div id="ParentHTML">';
            //prevTop = currEl.style["top"]; //current selected div's top

            prevTop = currEl.style["top"];
            if (currEl.previousSibling != null) {
                currEl = currEl.previousSibling;
                newTop = currEl.style["top"];
            }

            while (Math.round(newTop.replace('px', '')) == Math.round(prevTop.replace('px', ''))) //THis condition is used for picking complete line before selection
            {
                allParentHTML = currEl.outerHTML + allParentHTML;
                prevTop = currEl.style["top"];
                if (currEl.previousSibling != null) {
                    currEl = currEl.previousSibling;
                    newTop = currEl.style["top"];
                }
                else {
                    newTop = currEl.style["top"];
                    break;
                }
            }
            currEl = origCurrEl;//sel.getRangeAt(0).startContainer.parentNode;
            do {
                allParentHTML += currEl.outerHTML;
                //alert(currEl.outerHTML);
                prevTop = currEl.style["top"];
                if (currEl.nextSibling != null) {
                    currEl = currEl.nextSibling;
                    newTop = currEl.style["top"];
                } else {
                    newTop = currEl.style["top"];
                    break;
                }
                //if (newTop != prevTop) {
                //  allParentHTML += "&lt;br/&gt;";
                //}
            } while (Math.round(newTop.replace('px', '')) == Math.round(prevTop.replace('px', ''))); //THis condition is used for picking complete line after selection
            //while (currEl != sel.getRangeAt(0).endContainer.parentNode);  //THis condition is used for picking the selected text
            //allParentHTML += currEl.outerHTML;
            allParentHTML += '</div>';
            return allParentHTML;
        }

        function highlightSelection(text) {

        }

        $(function () {

            //var mainDiv = document.getElementById("divPdfPanel");
            var mainDiv = document.getElementById("tdPrdPdf");
            mainDiv.onmouseup = GetCoordinates;

            function GetCoordinates(e) {

                getPage_Comments = 0;

                //var parentElement = getSelectionParentsOuterHTML();
                //var selectedTextLine = getInnerHTML(parentElement);

                <%--var selectedTextLine = getSelectedHtml();

                 document.getElementById('<%= hfTableSelectedLine.ClientID %>').value = '';
                 document.getElementById('<%= hfTableSelectedLine.ClientID %>').value = selectedTextLine;--%>

                //OriginalText.value = selectedTextLine;

                //hfPageAllText.value = e.target.innerHTML;

                if (window.getSelection() != "") {
                    var posX = 0;
                    var posY = 0;
                    var divPos;
                    divPos = FindPosition(mainDiv);
                    if (!e) var e = window.event;
                    if (e.pageX || e.pageY) {
                        posX = e.pageX;
                        posY = e.pageY;
                    }
                    else if (e.clientX || e.clientY) {
                        posX = e.clientX + document.body.scrollLeft
                            + document.documentElement.scrollLeft;
                        posY = e.clientY + document.body.scrollTop
                            + document.documentElement.scrollTop;
                    }
                    posX = posX - divPos[0];
                    posY = posY - divPos[1];

                    <%-- document.getElementById('<%= hfPopupPosX.ClientID %>').value = posX;
                document.getElementById('<%= hfPopupPosY.ClientID %>').value = posY;--%>

                    //            alert('posX = ' + (posX - 650) + ' posY = ' + (posY - 40));

                    <%-- if ((document.getElementById('<%= hfSetManualTableHeader.ClientID %>').value == 'true') ||
                    (document.getElementById('<%= hfSetManualTableCaption.ClientID %>').value == 'true')) {

                    $('#divDialogue').css("display", "block");
                    $("#divDialogue").css("left", posX - 145 + "px");
                    $("#divDialogue").css("top", posY - 0 + "px");
                    $('#divMarkWholeTable').css("display", "none");
                    $('#divMarkTableHeadCap').css("display", "block");

                    if (document.getElementById('<%= hfSetManualTableHeader.ClientID %>').value == 'false' ||
                        document.getElementById('<%= hfSetManualTableHeader.ClientID %>').value == '')
                        $('#btnMarkTableHeader').css("display", "none");

                    if (document.getElementById('<%= hfSetManualTableCaption.ClientID %>').value == 'false' ||
                        document.getElementById('<%= hfSetManualTableCaption.ClientID %>').value == '')
                        $('#btnMarkTableCaption').css("display", "none");

                } else {--%>

                    $('#divDialogue').css("display", "block");
                    $("#divDialogue").css("left", posX - 145 + "px");
                    $("#divDialogue").css("top", posY - 0 + "px");
                    $('#divMarkWholeTable').css("display", "block");
                    $('#divMarkTableHeadCap').css("display", "none");
                    //}

                    //$('#divDialogue').css("display", "block");
                    //$("#divDialogue").css("left", posX - 145 + "px");
                    //$("#divDialogue").css("top", posY - 0 + "px");
                    //$('#divMarkWholeTable').css("display", "block");
                }
                else {
                    $('#divMarkWholeTable').css("dislay", "none");
                    $('#divDialogue').css("display", "none");
                    $('#divMarkTableHeadCap').css("dislay", "none");

                    $('#divTableBodyDialogue').css("display", "none");
                    $('#divTextEdit').css("display", "none");

                } //end main else
            }
        });


        function CloseMarkTableDialog() {
            $("#btnCloseTableDialog").dialog('close');
        }

        function CloseSaveImageDialog() {
            $("#btnCloseImageDialog").dialog('close');
        }

        var winW = $(window).width() - 800;
        var winH = $(window).height() - 80;




        //var winWidth = $(window).width() - 1000;
        //var winHeight = $(window).height() - 400;

        //function ShowTableEditDialog() {

        //    $('#divTableBodyDialogue').css("display", "block");
        //    $('#divTextEdit').css("display", "block");

        //    $("#divTableBodyDialogue").dialog({
        //        appendTo: "#dialogAfterEditTable",
        //        resizable: false,
        //        height: winHeight,
        //        width: winWidth,
        //        modal: false,
        //        beforeClose: function (event, ui) {
        //            //$("#btnGoTo").hide();
        //            //$("#btnFinish_Task").hide();
        //        }
        //    });

        //    // remove the title bar
        //    //$(".ui-dialog-titlebar").hide();
        //}




        //function var1() {
        //    document.getElementById('text').innerHTML = 'hi';
        //}

        //window.onload = UpdateTableBody;







        $(function () {
            $("#btnManualHeader").click(function () {
                $("#btnManualHeader").attr("disabled", true);
            });
            $("#btnManualCaption").click(function () {
                $("#btnManualCaption").attr("disabled", true);
            });
        });

        $(document).ready(function () {

            var textLayer = $('#pdfContainer').find('.textLayer');

            textLayer.bind('load', function () {
                //$('#loader1').hide();
                //$('.textLayer').hide();
            });
        });

    </script>

</head>
<body>
    <form id="form1" runat="server" style="background-color: white">
        <asp:HiddenField ID="hfParaSelFileLoadPath" runat="server" Value="WebHandlers/ShowParaSelection.ashx" />
        <asp:HiddenField ID="hfMarkedUParaText" runat="server" />
        <asp:HiddenField ID="hfMarkedSParaText" runat="server" />
        <asp:HiddenField ID="hfParaSelBookId" runat="server" ClientIDMode="Static" />
        
        <asp:Button ID="btnInternal" runat="server" OnClick="btnFinish_Click" Style="display: none;" />

        <div runat="server" id="divPdfPanel">

            <%--<div runat="server" id="divNavigationPanel" style="position: fixed; top: 0px; left: 38%; width: 25%; border-bottom-right-radius: 20px; padding: 6px; border-bottom-left-radius: 20px; color: White; background-color: lightsteelblue; height: 1px; overflow: hidden; padding-top: 6px;">
                <div style="width: 70%; padding-top: 6px; margin-left: auto; margin-right: auto;">

                    <div style="text-align: center;">
                        <asp:Button ID="btnPrevPage" Width="60px" runat="server" Text="<" OnClick="btnPrevPage_Click" />
                        
                         <asp:TextBox ID="tbxPageNum" Width="40px" runat="server" />&nbsp;&nbsp;/&nbsp;
                            <asp:Label ID="lblTotalPages" runat="server"></asp:Label>&nbsp;&nbsp;

                        <asp:Button ID="btnNextPage" Width="60px" runat="server" Text=">" OnClick="btnNextPage_Click" />
                    </div>
                    
                    <div style="text-align: center; color: black; margin-top:2%;">
                        <asp:Button ID="btnGoTo" runat="server" Text="Preview" style="width:30%" OnClick="btnGoTo_Click"/>
                         <asp:Button ID="btnFinish_Task" runat="server" Text="Preview" style="width:30%" OnClick="btnFinish_Click"/>
                    </div>
                    

                </div>
            </div>--%>

            <%--<div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">
                <uc1:ucShowMessage ID="ucShowMessage1" runat="server" />
            </div>--%>

            <table cellspacing="4" style="width: 95%; margin-left: auto; margin-right: auto;">
                <tr>
                    <td align="center" colspan="4">
                       <%-- <div id="divSuccessMessage" class="success" style="display: none; margin-left: auto; margin-right: auto; width: 40%; height: 40px; padding-top: 2%; border-radius: 10px;">
                        </div>
                        <div id="divErrorMessage" class="error" style="display: none; margin-left: auto; margin-right: auto; width: 40%; height: 40px; padding-top: 2%; border-radius: 10px;">
                        </div>--%>
                        
                         <div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">
                <uc1:ucShowMessage ID="ucShowMessage1" runat="server" />
            </div>

                    </td>
                </tr>
                <tr>
                    <td id="tdPrdPdf" align="right" style="width: 60%; height: 716px; min-height: 611px; vertical-align: top;">
                        <div id="pdfContainer">
                        </div>
                    </td>

                    <td align="left" style="width: 5%;"></td>
                    <td style="vertical-align: top; width: 40%;">
                        <div style="width: 85%; padding-top: 6px; margin-left: auto; margin-right: auto;">
                            <table cellspacing="5" style="width: 100%; position: relative; margin-top: 1%; top: 0px; left: 0px;background-color: aliceblue; padding-top: 5%; padding-bottom: 5%; padding-right: 1%; padding-left: 2%; border-radius: 2%; border: 1px solid black;">
                                <tr style="background-color: aliceblue;">
                                    <td align="left" style="width: 30%; vertical-align: central" colspan="2">
                                        <div style="width: 100%; padding-top: 6px; padding-left: 1%; padding-bottom: 3%; font-size: 95%;">
                                            <p style="margin-bottom: 2%; font-weight: bold;">Instructions:</p>
                                            <p style="margin-bottom: 1%; margin-top: 0%;">- Select UPara and SPara on any 2 pages (even and odd page)</p>
                                            <p style="margin-bottom: 1%; margin-top: 0%;">- UPara should contains minimum 2 lines. </p>
                                            <p style="margin-bottom: 1%; margin-top: 0%;">- SPara (Quote Para) should contains minimum 2 lines. </p>
                                        </div>
                                    </td>

                                </tr>
                                <tr>
                                    <td colspan="2" style="border: thin solid black;"></td>
                                </tr>

                                <tr>
                                    <td align="left" style="width: 30%; vertical-align: central" colspan="2">
                                        <div style="width: 100%; padding-top: 6px; padding-left: 1%; padding-bottom: 3%; font-size: 95%;">
                                            <p style="margin-bottom: 2%; font-weight: bold;">ShortcutKeys:</p>
                                            <p style="margin-bottom: 1%; margin-top: 0%;">Press ctrl + Right arrow for next page</p>
                                            <p style="margin-bottom: 0%; margin-top: 1%;">Press ctrl + Left arrow for previous page</p>
                                            <%-- <p style="margin-bottom: 0%; margin-top: 1%;">Press ctr + e for marking table</p>
                                        <p style="margin-bottom: 0%; margin-top: 1%;">Press ctr + q for merging table rows</p>--%>
                                        </div>


                                    </td>

                                </tr>
                                
                                <tr>
                                    <td>
                                       
                                    </td>

                                </tr>

                            </table>
                            
                            <br/>
                            <br/>
                            <table style="width: 100%;">
                                <tr>
                                    <td>
                                         <div style="width: 90%; padding: 20px; margin-left: auto; margin-right: auto; background-color: #dff2bf">

                                            <div style="text-align: center;">
                                                <asp:Button ID="btnPrevPage" Width="60px" runat="server" Text="<" OnClick="btnPrevPage_Click" />

                                                <asp:TextBox ID="tbxPageNum" Width="40px" runat="server" />&nbsp;&nbsp;/&nbsp;
                            <asp:Label ID="lblTotalPages" runat="server"></asp:Label>&nbsp;&nbsp;

                        <asp:Button ID="btnNextPage" Width="60px" runat="server" Text=">" OnClick="btnNextPage_Click" />
                                            </div>

                                            <div style="text-align: center; color: black; margin-top: 2%;">
                                                <asp:Button ID="btnGoTo" runat="server" Text="Preview" Style="width: 30%" OnClick="btnGoTo_Click" />
                                                <%--<asp:Button ID="btnFinish_Task" runat="server" Text="Finish Task" Style="width: 38%" OnClick="btnFinish_Click" />--%>
                                                <input id="btnFinish_Task" type="button" value="Finish Task" onclick="showConfirmationDialog();" style="width: 30%"/>
                                            </div>


                                        </div>
                                    </td>
                                </tr>
                            </table>
                        </div>

                    </td>
                </tr>
                <tr>
                    <td class="auto-style1" colspan="3">
                        <div style="width: 100%;">
                            <asp:LinkButton ID="lbtnHome" OnClick="lbtnHome_Click" runat="server" Text="Home" Style="display: inline-block; font-size: 18px; font-weight: normal; color: darkcyan"></asp:LinkButton>
                            <asp:LinkButton ID="lbtnLogOut" OnClick="lbtnLogOut_Click" runat="server" Style="display: inline-block; position: relative; margin-left: 0px; font-size: 18px; font-weight: normal; color: darkcyan">Log Out</asp:LinkButton>

                            <%--<asp:Button ID="btnFinishTask" runat="server" Text="Finish Task" Visible="False"
                                Style="display: inline-block; margin-left: 6%; width: 15%;" OnClick="btnFinishTask_Click" />--%>
                        </div>
                    </td>
                </tr>
            </table>

        </div>

        <div id="divDialogue" style="display: none; border: 4px solid #D6D6D6; position: absolute; left: 150px; top: 250px; border-top-right-radius: 20px; border-bottom-right-radius: 20px; padding: 6px; border-bottom-left-radius: 20px; background-color: #F0F0F0;">
            <div id='divMarkWholeTable'>
                <asp:Button ID="btnMarkUPara" runat="server" Text="Mark UPara" OnClientClick="MarkUParaInPdf();return true;"
                    OnClick="btnMarkUPara_Click" Style="margin-left: 10px;" />
                <asp:Button ID="btnMarkSPara" runat="server" Text="Mark SPara (Quote Para)"
                    OnClick="btnMarkSPara_Click" OnClientClick="MarkSParaInPdf();return true;" />
            </div>
            <%-- <div id='divMarkTableHeadCap' style="display: none;">
                <asp:Button ID="btnMarkTableHeader" runat="server" Text="Mark Table Header" OnClientClick="MarkTableHeaderCapInXml('tableHeader');return false;"
                    Style="margin-left: 10px;" ClientIDMode="Static" />
                <asp:Button ID="btnMarkTableCaption" runat="server" Text="Mark Table Caption" OnClientClick="MarkTableHeaderCapInXml('tableCaption');return false;"
                    OnClick="btnMarkTableCaption_Click" ClientIDMode="Static" />
            </div>--%>
        </div>
        
         <div id="skipConfirmDialog" title="Are you sure you want to Finish Task ?">
            <div id="divFinishTaskConfirm" style="width: 100%; height: 50px; margin-top: 5.5%; display: none;">
                <div>
                    <asp:Label ID="lblParaInserted" runat="server"></asp:Label>
                </div>
                <%--<div style="margin-top: 2.5%;">Before finishing a task you have to view all pages.</div>--%>
            </div>
        </div>

        <div id="dialogAfterMarkTable">
        </div>
        <div id="dialogAfterSaveImage">
        </div>
    </form>
</body>
</html>

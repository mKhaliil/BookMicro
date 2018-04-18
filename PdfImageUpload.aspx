<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="PdfImageUpload.aspx.cs" Inherits="Outsourcing_System.PdfImageUpload" %>

<%@ Register Src="UserControls/CommonControl/ucShowMessage.ascx" TagName="ucShowMessage" TagPrefix="uc1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="FinalScripts/MissingImagePdfJs/jquery-1.9.1.js"></script>
    <link href="FinalStyles/minimal.css" rel="stylesheet" type="text/css" />
    <link href="FinalStyles/viewer.css" rel="stylesheet" />
    <script src="FinalScripts/MissingImagePdfJs/pdf.js" type="text/javascript"></script>
    <script src="FinalScripts/MissingImagePdfJs/ui_utils.js" type="text/javascript"></script>
    <script src="FinalScripts/MissingImagePdfJs/text_layer_builder.js" type="text/javascript"></script>
    <script src="FinalScripts/MissingImagePdfJs/minimal.js" type="text/javascript"></script>

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
    </style>

    <script type="text/javascript">

        $(function () {

            $("#divNavigationPanel").hover(
                function () {
                    $(this).css("height", "7%");
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

        function LogMistakeCallBack() {
        }
        function NavigateNextPageCallBack() {

        }
        function NavigatePrevPageCallBack() {

        }

        function MarkTableAsImageInXml() {

            var cbxIgnoreAlgo = document.getElementById('<%= hfIsIgnoreAlgo.ClientID %>');
            var pageCompleteText = $('.textLayer')[0].innerHTML;

            var tableLines = getSelectedHtml();

            if (tableLines != null || tableLines != '') {

                var obj = {};

                if (cbxIgnoreAlgo.value == null || cbxIgnoreAlgo.value == '')
                    cbxIgnoreAlgo.value = 'false';

                obj.text = tableLines + "/~/" + cbxIgnoreAlgo.value + '/~/' + pageCompleteText;;

                methodURL = "Process1.aspx/MarkTable";
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
                        $('#divEditText').css("dislay", "none");
                        $('#divDialogue').css("display", "none");
                    },
                    error: function (jqXHR, textStatus, errorThrown) {

                    },
                    success: function (data) {

                        document.getElementById('<%= hfIsIgnoreAlgo.ClientID %>').value = 'false';

                        ShowMessege(data.d);
                    }
                });
                }
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

                            if (RowType == 'tableHeader')
                                document.getElementById('<%= hfSetManualTableHeader.ClientID %>').value = 'false';
                            else if (RowType == 'tableCaption')
                                document.getElementById('<%= hfSetManualTableCaption.ClientID %>').value = 'false';

                            ShowMessege(data.d);
                        }
                    });
            }
        }

        function MarkTableInXml() {

            var tableLines = getSelectedHtml();

            if (tableLines != null || tableLines != '') {
                document.getElementById('<%= hfMarkedTableText.ClientID %>').value = tableLines;
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

            var mainDiv = document.getElementById("divPdfPanel");
            mainDiv.onmouseup = GetCoordinates;

            function GetCoordinates(e) {

                //alert('444');
                //ShowSaveImageDialog();
                // getPage_Comments = 0;





                <%--var selectedTextLine = getSelectedHtml();

                 document.getElementById('<%= hfTableSelectedLine.ClientID %>').value = '';
                 document.getElementById('<%= hfTableSelectedLine.ClientID %>').value = selectedTextLine;--%>

                //OriginalText.value = selectedTextLine;

                //hfPageAllText.value = e.target.innerHTML;

                if (window.getSelection() != "") {

                    var parentElement = getSelectionParentsOuterHTML();
                    var selectedTextLine = getInnerHTML(parentElement);

                    //alert(selectedTextLine);

                    $('#hfImageSelectedLine').val(selectedTextLine);

                    ShowSaveImageDialog();
                }
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

        function ShowMarkedTableDialog() {

            $('#divMarkTable').css("display", "block");
            $('#divTable').css("display", "block");

            var tableBodyRows = document.getElementById('<%= hfTableBodyRows.ClientID %>').value;
            $('#mytable').append(tableBodyRows);

            var tableColumnWidth = document.getElementById('<%= hfColumnWidths.ClientID %>').value;
            $('#tblAdjustColWidth').append(tableColumnWidth);

            var tbl = document.getElementById('mytable');
            $("#lblRowCount").html(tbl.rows.length);
            //$("#lblColumnCount").html(tbl.rows[0].cells.length);

            var max = 0;
            $('#mytable tr').each(function () { max = Math.max(max, $('td', this).length); });
            $("#lblColumnCount").html(max);

            $("#divMarkTable").dialog({
                appendTo: "#dialogAfterMarkTable",
                resizable: false,
                height: winH,
                width: winW,
                position:
                {
                    my: "right",
                    at: "right top",
                    of: window
                },
                //height: 440,
                //width: 700,

                //height: 'auto',
                //width: 'auto',
                //maxHeight: 450,
                //maxWidth: 720,
                modal: false,
                beforeClose: function (event, ui) {
                    //$("#btnGoTo").hide();
                    //$("#btnFinish_Task").hide();
                }
            });
        }

        function ShowSaveImageDialog() {

            $('#divUploadImage').css("display", "block");
            $('#divImage').css("display", "block");

            $("#divUploadImage").dialog({
                appendTo: "#dialogAfterSaveImage",
                resizable: false,
                height: 220,
                width: 340,
                modal: true,
                beforeClose: function (event, ui) {
                    //$("#btnGoTo").hide();
                    //$("#btnFinish_Task").hide();
                }
            });


        }

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

        function DisplaySourcePath() {


        };

        function SetManualTableHeader() {

            document.getElementById('<%= hfSetManualTableHeader.ClientID %>').value = 'true';
    };

    function SetManualTableCaption() {

        document.getElementById('<%= hfSetManualTableCaption.ClientID %>').value = 'true';
    };


    //function var1() {
    //    document.getElementById('text').innerHTML = 'hi';
    //}

    //window.onload = UpdateTableBody;

    function UpdateTableBody() {

        var tableRows = $('#mytable').html();
        document.getElementById('<%= hfTableBodyRows.ClientID %>').value = tableRows;
    };

    function showHideHeaderSelection() {


    }

    function showHideCaptionSelection() {


    }

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

        <%--function DisplaySourcePath() {

            var file = document.getElementById("<%=fuSourceFile.ClientID%>");

                var path = file.value;
                if (path != '' && path != null) {
                    var q = path.substring(path.lastIndexOf('\\') + 1);
                    var name = q.substring(0, q.lastIndexOf('.'));
                    $('#tbxSourceFilePath').val(name);
                }
            };--%>

    </script>

</head>
<body>
    <form id="form1" runat="server" style="background-color: white">
        <asp:HiddenField ID="hfImgPdfloadPath" runat="server" Value="WebHandlers/ShowMissingImgPdf.ashx" />
        <asp:HiddenField ID="hfImageSelectedLine" runat="server" />

        <asp:HiddenField ID="hfIsIgnoreAlgo" runat="server" />
        <asp:HiddenField ID="hfPopupPosX" runat="server" />
        <asp:HiddenField ID="hfPopupPosY" runat="server" />
        <asp:HiddenField ID="hfMarkedTableText" runat="server" />
        <asp:HiddenField ID="hfSetManualTableHeader" runat="server" />
        <asp:HiddenField ID="hfSetManualTableCaption" runat="server" />
        <asp:HiddenField ID="hfTableBodyRows" runat="server" />
        <asp:HiddenField ID="hfMergedRows" runat="server" />
        <asp:HiddenField ID="hfColumnWidths" runat="server" />

        <div runat="server" id="divPdfPanel">

            <div runat="server" id="divNavigationPanel" style="position: fixed; top: 0px; left: 43%; width: 15%; border-bottom-right-radius: 20px; padding: 6px; border-bottom-left-radius: 20px; color: White; background-color: lightsteelblue; height: 1px; overflow: hidden; padding-top: 6px;">
                <%--<div style="width: 70%; padding-top: 6px; padding-left: 15px; margin-left: auto; margin-right: auto;">
                    <p style="margin-bottom: 2%">ShortcutKeys:</p>
                    <p style="margin-bottom: 1%; margin-top: 0%;">Press ctrl + Right arrow for next page</p>
                    <p style="margin-bottom: 0%; margin-top: 1%;">Press ctrl + Left arrow for previous page</p>
                    <p style="margin-bottom: 0%; margin-top: 1%;">Press ctr + e for marking table</p>
                </div>--%>

                <div style="width: 70%; padding-top: 6px; margin-left: auto; margin-right: auto;">

                    <div style="text-align: center;">
                        <asp:Button ID="btnPrevPage" Width="60px" runat="server" Text="<" OnClick="btnPrevPage_Click" />

                        <asp:Button ID="btnNextPage" Width="60px" runat="server" Text=">" OnClick="btnNextPage_Click" />
                    </div>
                    <div style="text-align: center; color: black;">
                        <asp:Label runat="server" ID="lblPageNum" />&nbsp;/
                            <asp:Label ID="lblTotalPages" runat="server"></asp:Label>
                    </div>
                </div>
            </div>

            <div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">
                <uc1:ucShowMessage ID="ucShowMessage1" runat="server" />
            </div>

            <table cellspacing="4" style="width: 95%; margin-left: auto; margin-right: auto;">
                <tr>
                    <td align="center" colspan="4">
                        <div id="divSuccessMessage" class="success" style="display: none; margin-left: auto; margin-right: auto; width: 40%; height: 40px; padding-top: 2%; border-radius: 10px;">
                        </div>
                        <div id="divErrorMessage" class="error" style="display: none; margin-left: auto; margin-right: auto; width: 40%; height: 40px; padding-top: 2%; border-radius: 10px;">
                        </div>

                    </td>
                </tr>
                <tr>
                    <td align="top" class="auto-style1" valign="top">
                        <%--<asp:Button ID="btnPrevPage" Width="60px" runat="server" Text="<" OnClick="btnPrevPage_Click" />--%>
                        <div style="width: 100%;">
                            <%-- <asp:LinkButton ID="lbtnHome" OnClick="lbtnHome_Click" runat="server" Text="Home" Style="display: inline-block; font-size: 18px; font-weight: normal; color: darkcyan"></asp:LinkButton>
                        <asp:LinkButton ID="lbtnLogOut" OnClick="lbtnLogOut_Click" runat="server" Style="display: inline-block; position: relative; margin-left: 0px; font-size: 18px; font-weight: normal; color: darkcyan">Log Out</asp:LinkButton>--%>
                        </div>
                    </td>

                    <td id="tdPrdPdf" align="left" style="width: 468px; height: 716px; min-height: 611px; vertical-align: top;">
                        <div id="pdfContainer">
                        </div>
                    </td>
                    <td align="left" style="width: 1%;"></td>
                    <td style="vertical-align: top; width: 46%;">

                        <%--  <table cellspacing="5" style="width: 100%; min-height: 400px; position: relative; margin-top: 1%; top: 0px; left: 0px; background-color: aliceblue; padding-top: 5%; padding-bottom: 2%; padding-right: 2%; padding-left: 2%; border-radius: 2%; border: 1px solid black;">
                            <tr>
                                <td colspan="2" align="right">
                                    <div style="width: 100%;">
                                        <asp:LinkButton ID="lbtnHome" OnClick="lbtnHome_Click" runat="server" Text="Submit Task" Style="display: inline-block; font-size: 18px; font-weight: normal; color: darkcyan"></asp:LinkButton>
                                        <asp:LinkButton ID="lbtnLogOut" runat="server" Style="display: inline-block; position: relative; margin-left: 5px; font-size: 18px; font-weight: normal; color: darkcyan">Log Out</asp:LinkButton>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="left" style="width: 30%; vertical-align: central" colspan="2">
                                    <div style="width: 100%; padding-top: 6px; padding-left: 3%; padding-bottom: 3%; font-size: 95%;">
                                        <asp:Label runat="server" ID="lblMissingImgMsg" Text="Image 1 is missing"></asp:Label>
                                    </div>
                                </td>
                            </tr>
                            <tr>
                                <td align="right" style="width: 30%; vertical-align: central" >Select Image :
                                </td>
                                <td  style="width: 30%; vertical-align: central">
                                    <asp:FileUpload ID="fuPdf" runat="server" class="multi" />
                                    
                                </td>
                            </tr>
                            <tr>
                                <td></td>
                                <td>
                                    <asp:Button ID="btnUpload" runat="server" OnClick="btnUpload_Click" Text="Upload" 
                                        Width="66px" />
                                    <asp:Button ID="btnNotRequired" runat="server" OnClick="btnUpload_Click" Text="Not An Image"
                                        Width="126px" />
                                </td>
                            </tr>
                        </table>--%>

                        <div style="width: 80%; min-height: 400px; position: relative; margin-top: 1%; top: 0px; left: 0px; padding-top: 5%; padding-bottom: 2%; padding-right: 2%; padding-left: 2%; border-radius: 2%; border: 1px solid black;">
                            <div style="width: 100%;">
                                <asp:LinkButton ID="lbtnHome" OnClick="lbtnHome_Click" runat="server" Text="Submit Task" Style="display: inline-block; font-size: 18px; font-weight: normal; color: darkcyan"></asp:LinkButton>
                                <asp:LinkButton ID="lbtnLogOut" OnClick="lbtnLogOut_Click" runat="server" Style="display: inline-block; position: relative; margin-left: 5px; font-size: 18px; font-weight: normal; color: darkcyan">Log Out</asp:LinkButton>
                            </div>

                           

                            <div id="divSelectImgLoc" style="width: 99%; background-color: gainsboro" runat="server" Visible="False">
                                <div style="width: 100%; margin-top: 10%; padding-top: 6px; padding-left: 3%; padding-bottom: 3%; font-size: 95%;">
                                    <p style="margin-bottom: 2%; font-weight: bold;">Instruction:</p>
                                    <p style="margin-bottom: 1%; margin-top: 0%;">
                                        
                                         <asp:Label runat="server" ID="lblMissingImgInXmlMsg" Text="Image_1_001" style="display:inline-block;"></asp:Label>
                                          is found in zip file but not in xml.</p>
                                     <br/>
                                    <p style="margin-bottom: 0%; margin-top: 1%;">Insert image tag in xml by selecting a line from Pdf or</p>
                                    <p style="margin-bottom: 0%; margin-top: 1%;">Press "Not An Image" button if image is not present in source Pdf.</p>
                                </div>
                            </div>
                            
                             <div id="divMissingImgInXmlMsg" style="margin-top: 5%; margin-bottom: 3%; width: 95%; padding-top: 15px; padding-left: 1%; 
    padding-right: 1%; padding-bottom: 5%;" runat="server" Visible="False">
                               
                                 
                                  <asp:Button ID="btnNoImgInXmlRequired" runat="server" OnClick="btnNoImgInXmlRequired_Click" Text="Not An Image" style="margin-top:3%;"
                                        Width="116px" />

                            </div>

                            <div id="divUploadMissingImg" style="width: 99%; background-color: gainsboro;" runat="server" Visible="False">
                                <div style="width: 100%; margin-top: 10%; padding-top: 6px; padding-left: 3%; padding-bottom: 3%; font-size: 95%;">
                                    <p style="margin-bottom: 2%; font-weight: bold;">Instruction:</p>
                                    <p style="margin-bottom: 1%; margin-top: 0%;">
                                          <asp:Label runat="server" ID="lblUploadMissingImgMsg" Text="Image_1_001" style="display:inline-block;"></asp:Label>
                                          is found in xml file but not in zip file.</p>
                                    
                                   
                                    <br/>
                                    <p style="margin-bottom: 0%; margin-top: 1%;">Upload missing Image or</p>
                                    <p style="margin-bottom: 0%; margin-top: 1%;">Press "Not An Image" button if image is not present in source Pdf.</p>
                                </div>
                            </div>

                            <div id="divUploadImg" style="width: 99%; height:70px; background-color: gainsboro; margin-top: 10%;" runat="server" Visible="False">
                                
                                <div style="padding-top:7%; padding-left:3%;">
                                    <asp:Label Text="Select Image :" runat="server"  />
                                    <asp:FileUpload ID="fuPdf" runat="server" class="multi" Style="display: inline-block; width: 60%;" />
                                </div>
                                </div>

                                <div id="divUploadBtns" style="width: 100%; margin-top: 10%;" runat="server" Visible="False">
                                    <asp:Button ID="btnUploadMissingImg" runat="server" OnClick="btnUploadMissingImg_Click" Text="Upload"
                                        Width="66px" />
                                    <asp:Button ID="btnNoImgUploadRequired" runat="server" OnClick="btnNoImgUploadRequired_Click" Text="Not An Image"
                                        Width="126px" />
                                </div>
                           

                        </div>
                        
                        <div style="width:100%; height:50px; padding-top:5%; padding-left:2%;">
                            <asp:Button ID="btnFinishTask" runat="server" Text="Finish Task"  OnClick="btnFinishTask_Click" Visible="False" Style="display: inline-block; margin-left: 6%; width: 20%;" />
                        </div>

                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">
                        <%--<div style="width: 100%;">
                            <asp:LinkButton ID="lbtnHome" OnClick="lbtnHome_Click" runat="server" Text="Home" Style="display: inline-block; font-size: 18px; font-weight: normal; color: darkcyan"></asp:LinkButton>
                            <asp:LinkButton ID="lbtnLogOut" runat="server" Style="display: inline-block; position: relative; margin-left: 0px; font-size: 18px; font-weight: normal; color: darkcyan">Log Out</asp:LinkButton>

                        </div>--%>
                    </td>
                    <td style="width: 30%;">
                        <div style="margin-left: 22%;">
                            <%--<asp:CheckBox ID="cbxIgnoreAlgo" runat="server" Checked="False" Text="Ignore Algo Mapped"  />--%>
                            <%--<asp:Button ID="btnFinishTask" runat="server" Text="Finish Task" Visible="False" Style="display: inline-block; margin-left: 6%; width: 20%;" />--%>
                        </div>
                    </td>
                    <td></td>
                    <td>
                        <%-- <asp:Button ID="btnPrevTable" Width="120px" runat="server" Text="Previous Table" OnClick="btnNextPage_Click" />
                        <asp:Button ID="btnNextTable" Width="100px" runat="server" Text="Next Table" OnClick="btnNextPage_Click" Style="display: inline-block; margin-left: 6%;"/>--%>
                            
                    </td>
                </tr>
                <tr>
                    <td class="auto-style1">

                        <%--<div style="width:100%;">
                             <asp:LinkButton ID="lbtnHome" OnClick="lbtnHome_Click" runat="server" Text="Home" Style="display: inline-block; font-size: 18px; font-weight: normal; color: darkcyan"></asp:LinkButton>
                        <asp:LinkButton ID="lbtnLogOut" OnClick="lbtnLogOut_Click" runat="server" Style="display: inline-block; position: relative; margin-left: 0px; font-size: 18px; font-weight: normal; color: darkcyan">Log Out</asp:LinkButton>

                        </div>--%>

                    </td>
                    <td style="width: 46%;" align="center"></td>
                    <td></td>
                </tr>
            </table>

        </div>

        <div id="divUploadImage" title="Save Image Tag" style="width: 60%; height: 90px; margin-left: auto; margin-right: auto; display: block; padding: 5px; display: none;">
            <div id="divImage" style="width: 90%; padding-left: 5%; padding-right: 5%; padding-top: 3%; padding-bottom: 3%; display: none;">
                <label>Set Image Tag Location in Xml:</label>
                <br />
                <asp:RadioButtonList ID="rbtnlSelectImgLoc" runat="server" Style="margin-top: 3%;">
                    <asp:ListItem>Above Selcted Line</asp:ListItem>
                    <asp:ListItem Selected="True">Below Selcted Line</asp:ListItem>
                </asp:RadioButtonList>
                <br />
                <div style="width: 98%; margin-left: auto; margin-right: auto;">
                    <div style="float: right;">
                        <asp:Button ID="btnSaveImage" runat="server" OnClick="btnSaveImage_Click" Text="Save" Width="120px" />
                        <asp:Button ID="btnCloseImageDialog" OnClientClick="CloseSaveImageDialog(); return false;" runat="server" Text="Close" Width="84px" />
                    </div>
                </div>
            </div>
        </div>

        <div id="dialogAfterMarkTable">
        </div>
        <div id="dialogAfterSaveImage">
        </div>
    </form>
</body>
</html>

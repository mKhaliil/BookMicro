<%@ Control Language="C#" AutoEventWireup="true" Inherits="UserControls_PDFViewer"
    CodeBehind="PDFViewer.ascx.cs" %>
<script src="../scripts/PdfWebCompare/compatibility.js" type="text/javascript"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/l10n.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/core.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/util.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/api.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/metadata.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/canvas.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/obj.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/function.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/charsets.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/cidmaps.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/colorspace.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/crypto.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/evaluator.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/fonts.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/glyphlist.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/image.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/metrics.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/parser.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/pattern.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/stream.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/worker.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/jpg.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/jpx.js"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/bidi.js"></script>
<script type="text/javascript">    PDFJS.workerSrc = '../scripts/PdfWebCompare/worker_loader.js';</script>
<script src="../scripts/PdfWebCompare/debugger.js" type="text/javascript"></script>
<script type="text/javascript" src="../scripts/PdfWebCompare/viewerControl.js"></script>
<script src="../scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
<%--<script type="text/javascript" src="scripts/jquery-impromptu.4.0.min.js"></script>--%>
<script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.7.2/jquery.min.js"></script>
<script src="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/jquery-ui.js" type="text/javascript"></script>
<link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css"
    rel="stylesheet" type="text/css" />

<%--<script src="../FinalScripts/ifvisible.js" type="text/javascript"></script>
<script src="../FinalScripts/TImeMe.js" type="text/javascript"></script>--%>


<script type="text/javascript">


//    TimeMe.setIdleDurationInSeconds(30);
//    TimeMe.setCurrentPageName("Comparison");
//    TimeMe.initialize();


//    window.onbeforeunload = function (event) {
////        xmlhttp = new XMLHttpRequest();
////        xmlhttp.open("POST", "ENTER_URL_HERE", false);
////        xmlhttp.setRequestHeader("Content-type", "application/x-www-form-urlencoded");
//        var timeSpentOnPage = TimeMe.getTimeOnCurrentPageInSeconds();

//        alert('timeSpentOnPage ' + timeSpentOnPage);
////        xmlhttp.send(timeSpentOnPage);
//    };


    function ShowPopupJq(message) {
        $(function () {
            var wWidth = $(window).width();
            var dWidth = wWidth * 0.5;
            var wHeight = $(window).height();
            var dHeight = wHeight * 0.6;

            $("#dialogData").html(message);
            $("#dialog").dialog({
                title: "Table",
                height: dHeight,
                width: dWidth,
                //                maxHeight: dHeight,
                //                maxWidth: dWidth,
                position: "center",
                resizable: false,
                buttons: {
                    Close: function () {
                        $(this).dialog('close');
                    }
                },
                modal: true
            });
        });
    };

    //    $(function () {
    //        $(document).tooltip();
    //    });

    //    // Move the dialog divs back into the form tag so that values in the dialog can be posted and used on the server side.
    //    $('#dialog').parent().appendTo($('#dialogPlaceHolder'));
</script>
<script type="text/javascript">

    function ShowLightBox(e) {

        //        if (document.getElementById('<%= hfTableText.ClientID %>').value != "1") {
        //            
        //        }

        //            document.getElementById('<%= hfTableText.ClientID %>').value = "1";

        //            alert('1');
        //        $("#hfTableText").each(function () {

        //            alert('999');
        //            var value = $(this).val();
        //            // do something with the value
        //        });



        //        var table = document.getElementById('<%= hfTableText.ClientID %>').value;
        //        
        //        alert('table.length ' + table.length);
        //        table.each(function () {
        //            alert('1111');
        ////            var value = $(this).val();
        //        });

        var bcgDiv = document.getElementById("divBackground");
        var imgDiv = document.getElementById("divImage");
        var tableText = document.getElementById("divTableText");
        var imgLoader = document.getElementById("imgLoader");



        imgLoader.style.display = "block";

        tableText.innerHTML = document.getElementById('<%= hfTable.ClientID %>').value;

        var width = document.body.clientWidth;
        if (document.body.clientHeight > document.body.scrollHeight) {
            bcgDiv.style.height = document.body.clientHeight + "px";
            bcgDiv.style.width = document.body.clientWidth + "px";
        }
        else {
            bcgDiv.style.height = document.body.scrollHeight + "px";
            bcgDiv.style.width = document.body.scrollWidth + "px";
        }

        imgDiv.style.left = (width - 650) / 2 + "px";
        imgDiv.style.top = "20px";
        bcgDiv.style.width = "100%";

        bcgDiv.style.display = "block";
        imgDiv.style.display = "block";
        return false;
    }

    function HideLightBox() {
        var bcgDiv = document.getElementById("divBackground");
        var imgDiv = document.getElementById("divImage");
        var tableText = document.getElementById("divTableText");
        if (bcgDiv != null) {
            bcgDiv.style.display = "none";
            imgDiv.style.display = "none";
            tableText.style.display = "none";
        }
    }
   
</script>
<style type="text/css">
    body
    {
        margin: 0;
        padding: 0;
        height: 100%;
        overflow-y: auto;
    }
    .sup
    {
        font-size: xx-small;
        vertical-align: top;
    }
    .modal
    {
        display: none;
        position: absolute;
        top: 0px;
        left: 0px;
        background-color: black;
        z-index: 100;
        opacity: 0.8;
        filter: alpha(opacity=60);
        -moz-opacity: 0.8;
        min-height: 100%;
        width: 300px;
    }
    #divImage
    {
        display: none;
        z-index: 1000;
        position: fixed;
        top: 0;
        left: 0;
        background-color: White;
        height: 550px;
        width: 600px;
        padding: 3px;
        border: solid 1px black;
    }
    * html #divImage
    {
        position: absolute;
    }
    
    .hover
    {
        background-color: yellow;
    }
    
    .active
    {
        background-color: red;
    }
</style>
<script type="text/javascript">

    $(document).ready(function () {
        $('.image').click(function () {
            $(this).css('width', function (_, cur) {
                return cur === '100px' ? '100%' : '100px';
            });  // original width is 500px 
        });
    });

    function SelectText(element) {
        alert('aaa');
        var doc = document;
        var text = doc.getElementById(element);
        alert(text.innerText);
    }

    function GetZoomIcon_MouseClick(e) {

        var rr = e.toElement.innerHTML.replace("&nbsp;", "");

        if (rr == 'here') {

            //            alert('finally clicked once');
            var table_Html = document.getElementById('<%= hfTableText.ClientID %>').value;
            var table_Text = document.getElementById('<%= hfTable.ClientID %>').value;

            //            var temp_Table = table_Text.split("~//~");

            var res = table_Text.split(" ");

            var parentElement = getSelectionParentsOuterHTML();
            var selectedTextLine = getInnerHTML(parentElement);

            //            alert('complete selected text line is ' + selectedTextLine);

            var mainDiv = document.getElementById("mainContainer");

            for (i = 0; i < res.length; i++) {

                if (res[i].trim().toLowerCase() == window.getSelection().anchorNode.nodeValue.trim().toLowerCase()) {

                    //                    alert('equal');
                    ShowPopupJq(table_Html);
                    break;
                }
            }
        }
    }

    var getPage_Comments = 0;


    ///////////////////////////Change of cursor to zoom icon when cursor is moved over 'click here to open'////////////////////////////
    function GetZoomIcon_MouseOver(e) {


        //     $(function () {

        ////            window.name = "OnlineTestUser";
        ////                    alert('Current windows name is ' + window.name);
        ////                });


//        if (window.name = 'Comparison') {
//            alert('Current windows name is ' + window.name);
//        } else {
//            alert('Current page is not comparison');
//        }

        var hoverText = e.toElement.innerHTML.replace("&nbsp;", "");

        if (hoverText.length <= 20) {
            var table_Text = document.getElementById('<%= hfTable.ClientID %>').value;

            if (table_Text != '') {
                var res = table_Text.split(" ");

                if ((hoverText == 'Click') || (hoverText == 'here') || (hoverText == 'to') || (hoverText == 'Open') || (hoverText == 'Table')) {
                    $('.textLayer div').css('cursor', '-webkit-zoom-in');
                }
                //                else if (hoverText != 'Click') {
                //                    for (i = 0; i < res.length; i++) {
                //                        if (res[i].trim().toLowerCase() == hoverText.trim().toLowerCase()) {
                //                            $('.textLayer div').css('cursor', '-webkit-zoom-in');
                //                            break;
                //                        }
                //                    }
                //                }
                else {
                    $('.textLayer div').css('cursor', 'text');
                }
            }
        }
        else {
            $('.textLayer div').css('cursor', 'text');

            if ((e.target.innerHTML.indexOf("<div data-canvas-width=") == 0) && (getPage_Comments == 0)) {
                GetErrorComments(e.target.innerHTML);
                        }




//            var input = []; 
//            var divs = $('.textLayer div');

//            for (m = 0; m < divs.length; m++) {

//                if (divs[m] != '') {

//                    input.push(divs[m].innerText + "~//~"); 
//                }
//            }

//            GetErrorComments(input);





        }
    }


    function OnSelectionGetPassedTests() {

        methodURL = "Comparison.aspx/GetPassedTests";

        $.ajax({
            type: "POST",
            url: methodURL,
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        });
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

    //////////////////////////////////////////////Ajax method to show paragraph type and error comments/////////////////////////////////////////
    function GetTextType(lineText) {
        methodURL = "Comparison.aspx/GetParaType";
        parameters = "{'text':'" + lineText + "'}";
        var returnVal = false;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: methodURL,
            data: parameters,
            dataType: "json",
            async: false,
            success: function (data) {

                if (data.d != null) {

                    jQuery("label[for='paraType']").html("<span style='color:DarkBlue;font:bold;font-size:large'>" + data.d + "</span>");
                }
            }
        });
    }
    /////////////////////////////////////////////////////////////////////end//////////////////////////////////////////////////////////////////////

    /////////////////////////////////Ajax method to set comments of all errors in a page inserted through error dialog////////////////////////////
    function GetErrorComments(lineText) {
        methodURL = "Comparison.aspx/GetErrorComments";
        parameters = "{'text':'" + lineText + "'}";
        var returnVal = false;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: methodURL,
            data: parameters,
            dataType: "json",
            async: false,
            success: function (data) {

                if (data.d != null) {

                    var divWithComments = data.d;

                    if (divWithComments != '') {

                        var errorText = divWithComments.split("~//~");

                        if ((errorText != '') && (errorText.length > 0)) {

                            for (i = 0; i < errorText.length; i++) {

                                if (errorText[i] != '') {

                                    var divWithComments = errorText[i].split(",");
                                    var comment = divWithComments[divWithComments.length - 1];

                                    if ((divWithComments != '') && (divWithComments.length > 0)) {

                                        var allDivs = $('.textLayer div');

                                        for (j = 0; j < allDivs.length; j++) {

                                            for (k = 0; k < divWithComments.length; k++) {

                                                if ((j == divWithComments[k]) && (divWithComments[k] != '')) {

                                                    $(allDivs[j]).attr('title', comment);
                                                    //                                                    $('.textLayer div').attr('background-color', 'yellow')
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    returnVal = true;
                    getPage_Comments = 1;
                }
            }
        });

        return returnVal;
    }
    /////////////////////////////////////////////////////////////////////end//////////////////////////////////////////////////////////////////////

    ///////////////////////////////////////Ajax method to show comments of an error inserted through error dialog////////////////////////////////
    function ShowErrorComment(lineText) {
        methodURL = "Comparison.aspx/ShowComment_ByError";
        parameters = "{'text':'" + lineText + "'}";
        var returnVal = false;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: methodURL,
            data: parameters,
            dataType: "json",
            async: false,
            success: function (data) {

                if (data.d != null) {
                    $('#<%= tbxComments.ClientID %>').val(data.d);
                }
            }
        });
    }
    /////////////////////////////////////////////////////////////////////end//////////////////////////////////////////////////////////////////////

    ////////////////////////////////////////////////Highlight mistakes selected through error dialog//////////////////////////////////////////////
    function LogMistakesInXml() {

        //        alert('LogMistakesInXml');
        var parentElement = getSelectionParentsOuterHTML();
        var selectedTextLine = getInnerHTML(parentElement);

//        OriginalText.value = selectedTextLine;

//        var pageCompleteText = $('.textLayer')[0].innerHTML;

        //        var temp = selectedTextLine + '/~/' + pageCompleteText;

        var pageCompleteText = $('.textLayer')[0].innerHTML;

        var tbxCommments = $('#<%= tbxComments.ClientID %>').val();

        var temp = tbxCommments + '/~/' + OriginalText.value + '/~/' + pageCompleteText;


        methodURL = "Comparison.aspx/LogMistakes";
        parameters = "{'text':'" + temp + "'}";
        var returnVal = false;
        $.ajax({
            type: "POST",
            contentType: "application/json; charset=utf-8",
            url: methodURL,
            data: parameters,
            dataType: "json",
            async: false,
            success: function (data) {

                if (data.d != null) {

                    HighlightSelectedText(data.d);

                    //                    $('#CtrlPdfCmp_imgSrc').attr('src', 'file:///D:/Office%20Data/Files_Dev/001/001-1/Comparison/Comparison-1/97/1.pdf.jpeg');

                    //imagePanel.attr('src', 'file:///D:/Office%20Data/Files_Dev/001/001-1/Comparison/Comparison-1/97/1.pdf.jpeg');

                    //$('#CtrlPdfCmp_imgSrc').href('../showPdf.ashx?Page=1&pdfType=src&type=img; return false;');

                    $('#cssmenu').css("dislay", "none");
                    $('#divEditText').css("dislay", "none");
                    $('#divDialogue').css("display", "none");

                }
            }
        });
    }

    function HighlightSelectedText(selectedLineDivs) {

        if (selectedLineDivs != '') {

            var divs = selectedLineDivs.split(",");

            if ((divs != '') && (divs.length > 0)) {

                var allDivs = $('.textLayer div');

                for (j = 0; j < allDivs.length; j++) {

                    for (k = 0; k < divs.length; k++) {

                        if ((j == divs[k]) && (divs[k] != '')) {

                            $(allDivs[j]).css('color', 'black');
                            $(allDivs[j]).css('background-color', '#FFB0B0');

                        }
                    }
                }
            }
        }
    }
    /////////////////////////////////////////////////////////////////////end//////////////////////////////////////////////////////////////////////

    function GetCoordinates(e) {

        getPage_Comments = 0;

        var parentElement = getSelectionParentsOuterHTML();
        var selectedTextLine = getInnerHTML(parentElement);

        OriginalText.value = selectedTextLine;

        //        if (e.target.innerHTML.indexOf("<div data-canvas-width=") == 0) {

        hfPageAllText.value = e.target.innerHTML;
        //        }

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

            //            alert('posX = ' + (posX - 650) + ' posY = ' + (posY - 40));
            $('#divDialogue').css("display", "block");
            $("#divDialogue").css("left", posX - 650 + "px");
            $("#divDialogue").css("top", posY - 40 + "px");
            $('#cssmenu').css("display", "block");


            GetTextType(selectedTextLine);

            ShowErrorComment(selectedTextLine);

        }
        else {
            $('#cssmenu').css("dislay", "none");
            $('#divEditText').css("dislay", "none");
            $('#divDialogue').css("display", "none");

            if (window.getSelection().anchorOffset > 0) {

                //                alert('valid text');

                var table_Html = document.getElementById('<%= hfTableText.ClientID %>').value;
                var table_Text = document.getElementById('<%= hfTable.ClientID %>').value;

                //                var parentElement = getSelectionParentsOuterHTML();
                //                var selectedTextLine = getInnerHTML(parentElement);
                var startIndex = selectedTextLine.indexOf('here');

                //Get selected table number from hidden field by matching clicked text with its text.
                var tableNumber = 0;
                var temp_Table = table_Text.split("~//~");

                if ((temp_Table != '') && (temp_Table.length > 0)) {

                    for (i = 0; i < temp_Table.length; i++) {
                        if (temp_Table[i] != '') {

                            var res = temp_Table[i].split(" ");

                            if (res != '') {
                                for (j = 0; j < res.length; j++) {
                                    if (res[j] != '') {
                                        if (res[j].trim().toLowerCase() == window.getSelection().anchorNode.nodeValue.trim().toLowerCase()) {
                                            tableNumber = i;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


                var temp_TableHtml = table_Html.split("~//~");
                var current_Table = temp_TableHtml[tableNumber];




                //                var openTableText = selectedTextLine.substring(startIndex - 6, startIndex + 26);

                //                if (openTableText === 'Click here to Open Table') {

                //                }

                ////                if ((window.getSelection().anchorNode.nodeValue.trim().toLowerCase() == 'here')
                ////                &&
                ////                (
                ////                    (selectedTextLine.substring(startIndex - 6, startIndex - 1) == 'Click') &&
                ////                    (selectedTextLine.substring(startIndex + 5, startIndex + 7) == 'to') &&
                ////                    (selectedTextLine.substring(startIndex + 8, startIndex + 12) == 'Open') &&
                ////                    (selectedTextLine.substring(startIndex + 13, startIndex + 18) == 'Table')
                ////                )
                ////            ) {

                ////                    ShowPopupJq(current_Table);
                ////                }

                if (window.getSelection().anchorNode.nodeValue.trim().toLowerCase() == 'here') {

                    ShowPopupJq(current_Table);
                }

                else {
                    var res = table_Text.split(" ");

                    for (i = 0; i < res.length; i++) {
                        if (res[i].trim().toLowerCase() == window.getSelection().anchorNode.nodeValue.trim().toLowerCase()) {
                            ShowPopupJq(current_Table);
                            break;
                        }
                    }
                } //end inner else
            } //end inner if
        } //end main else
    }

    $(function () {

//        window.name = "Comparison";
        var mainDiv = document.getElementById("mainContainer");
        mainDiv.onmouseover = GetZoomIcon_MouseOver;
        mainDiv.onmouseup = GetCoordinates;

    });

    function getSelectedText() {
        var text = "";
        if (typeof window.getSelection != "undefined") {
            text = window.getSelection().toString();
        } else if (typeof document.selection != "undefined" && document.selection.type == "Text") {
            text = document.selection.createRange().text;
        }

        //        alert('text = ' + text);
        return text;
    }

    function doSomethingWithSelectedText() {
        //        alert('222');
        var selectedText = getSelectedText();
        if (selectedText) {
            getSelText_ByShortcutKey();
            //            alert("Got selected text " + selectedText);
        }
    }

    document.onmouseup = doSomethingWithSelectedText;
    document.onkeyup = doSomethingWithSelectedText;

    var dialogTextAreaVal;
    $(document).keydown(function (e) {
        //        alert('Key down');
        var code = e.keyCode ? e.keyCode : e.which;
        if (code == "27") {
            $("#btn").css("display", "none");
        }
    });



    //    document.onkeydown = function (e) {

    //        /// check ctrl + e key
    //        if (e.ctrlKey === true && e.keyCode === 69) {
    //            e.preventDefault();
    //            LogMistakeCallBack();
    //        }
    //    };

    //    document.onkeydown = function (e) {

    //        /// check ctrl + e key
    //        if (e.ctrlKey === true) {
    //            if (e.keyCode === 69) {
    //                e.preventDefault();
    //                LogMistakeCallBack();
    //            }
    //        }
    //    };


    $(function () {

        document.onkeydown = function (e) {

            // ctrl + e (e = 69)
            if (e.ctrlKey === true) {
                if (e.keyCode === 69) {
                    //                alert('dddddddddddddd');
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
        };
    });

    function checkMe() {

        var txt = "";
        $("#btn").css("display", "none");
        if (window.getSelection) {
            txt = window.getSelection();
        }
        else if (document.getSelection) {
            txt = document.getSelection();
        } else if (document.selection) {
            txt = document.selection.createRange().text;
        }

        alert(txt);
    }
    function addcomment() {

        var modifiedText = document.getElementById('CommentsEnterd');
        modifiedText.value = document.getElementById('ctl00_ContentPlaceHolder1_CtrlPdfCmp_pdfCtrl_commentBox').value;

    }
    function showMsg() {
        alert("You have selected some of the text!");
    }

    function ShowPopup(id) {

        var selTextVal = $("#taSelText").val();
        if (selTextVal.indexOf("\n") != -1) {
            var txt = '<div style="width: 100%"><table style="width: 100%"><tr><td align="left">Please edit selected text:</td>' +
                '<td align="right"><input  type="button" disabled="disabled" onclick="SpliteCallBack()"' +
                ' value="Split" /><input type="button" onclick="MergeCallBack()" value="Merge" /><input type="button" onclick="LogMistakeCallBack()" value="Log Mistake" />'
        }
        else {
            var txt = '<div style="width: 100%"><table style="width: 100%"><tr><td align="left">Please edit selected text:</td>' +
                '<td align="right"><input  type="button" disabled="disabled" onclick="SpliteCallBack()"' +
                ' value="Split" /><input type="button"  disabled="disabled" onclick="MergeCallBack()" value="Merge" /><input type="button" onclick="LogMistakeCallBack()" value="Log Mistake" />'
        }
        $("#taSelText").val(selTextVal);
        txt += '</td></tr></table></div><textarea type="text" id="alertName" name="alertName" class="popuptextarea">' + selTextVal + '</textarea>' +
            '<br><br><br>Enter your comments:<br><input type="text" id="commentBox" onBlur="addcomment()"   name="commentBox" ' +
            'class="popuptextbox" value="" />';

        $.prompt(txt, {
            callback: popupCallback,
            buttons: { Update: true, Cancel: false }
        });

    }

    function popupCallback(e, v, m, f) {
        if (v != undefined && v != false) {
            $("#taSelText").val(f.alertName);
            $('#<%= btnGetSelText.ClientID %>').click();
        }
    }
    function SpliteCallBack() {
        $('#<%= btnSplit.ClientID %>').click();
    }
    function MergeCallBack() {
        $('#<%= btnMerger.ClientID %>').click();
    }
    function LogMistakeCallBack() {
        $('#<%= btnLogMistake.ClientID %>').click();
    }
    function NavigateNextPageCallBack() {
        $('#<%= btnNavigateNextPage.ClientID %>').click();
    }
    function NavigatePrevPageCallBack() {
        $('#<%= btnNavigatePrevPage.ClientID %>').click();
    }

    function ShowTablePopUp() {
        $('#<%= btnShowTablePopUp.ClientID %>').click();
    }
    

</script>
<script type="text/javascript" language="javascript">
    function SendXml() {

//        alert('sendxml');
        var pageXml = document.getElementById('pageXML');
        //alert('id: = ' + pageXml);
        pageXml.value = document.getElementById('viewer').innerHTML;

        var modifiedText = document.getElementById('ModifiedText');
        modifiedText.value = document.getElementById('taSelText').value;

        var selXmlParents = document.getElementById('SelectedXMLParents');
        selXmlParents.value = document.getElementById('taSelParent').value;

        //pageXml.innerText = '<p>hello testing</p>';
    }

    function getSelText_ByShortcutKey() {
           
        $("#btn").css("display", "none");
        var currForm = document.getElementsByTagName("form");
        var currFormID;
        if (currForm != null) {
            currFormID = currForm.id;
        }
        var txt = '';
        if (window.getSelection) {
            txt = window.getSelection();
            //txt = txt;//  + '::1';
        }
        else if (document.getSelection) {
            txt = document.getSelection();
            //txt = txt;//  + '::2';
        }
        else if (document.selection) {
            txt = document.selection.createRange().text;
            //txt = txt + '::3';
        }
        else return;

        var parentElement = getSelectionParentsOuterHTML();
        var selectedTextWithBreaks = getInnerHTML(parentElement);

      
//        document.aspnetForm.selectedtext.value = selectedTextWithBreaks;

//        document.aspnetForm.OriginalText.value = selectedTextWithBreaks;

//        document.aspnetForm.selectedparent.value = parentElement;
    }

    function getSelText() {
        //return false;
//        alert('getSelText');
        $("#btn").css("display", "none");
        var currForm = document.getElementsByTagName("form");
        var currFormID;
        if (currForm != null) {
            currFormID = currForm.id;
        }
        var txt = '';
        if (window.getSelection) {
            txt = window.getSelection();
            //txt = txt;//  + '::1';
        }
        else if (document.getSelection) {
            txt = document.getSelection();
            //txt = txt;//  + '::2';
        }
        else if (document.selection) {
            txt = document.selection.createRange().text;
            //txt = txt + '::3';
        }
        else return;

        //document.aspnetForm.selectedtext.value = txt;
        //document.aspnetForm.OriginalText.value = txt;

        var parentElement = getSelectionParentsOuterHTML();
        var selectedTextWithBreaks = getInnerHTML(parentElement);

//        document.aspnetForm.selectedtext.value = selectedTextWithBreaks;

//        document.aspnetForm.OriginalText.value = selectedTextWithBreaks;

//        document.aspnetForm.selectedparent.value = parentElement;

        ShowPopup();
    }
    function getInnerHTML(ParentNodeHTML) {
        var div = document.createElement('div');
        div.innerHTML = ParentNodeHTML;
        var HTMLWithBreaks = div.textContent;
        try {
            var cleaned = HTMLWithBreaks.replace(/<br[ ]?[/]>/g,"\n");
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
                    var selElements=new Array();
                    var selElementsTop=new Array();

                    var cuurrEl  = null;
                    var cuurrEl2  = null;
                    var tempEl = null;
                    var startTop, endTop;

                    if(sel.getRangeAt(0).startContainer.nextSibling == null)
                    {
                        currEl = sel.getRangeAt(0).startContainer.parentNode;
                    }
                    else
                    {
                        currEl = sel.getRangeAt(0).startContainer;
                    }
                    
                    startTop = currEl.style["top"];

                    if(sel.getRangeAt(0).endContainer.nextSibling == null)
                    {
                        currEl2 = sel.getRangeAt(0).endContainer.parentNode;
                    }
                    else
                    {
                        currEl2 = sel.getRangeAt(0).endContainer;
                    }
                    endTop = currEl2.style["top"];
                    if(endTop == "")
                        currEl2 = sel.getRangeAt(0).endContainer;

                    tempEl = currEl;

                    
                    if(endTop==startTop){
                        allParentHTML += getCompleteLine(currEl);
                    }else{
                       
                        var endTop1 =  endTop.replace('px', '');//trimmed
                        var startTop1 =  startTop.replace('px', '');//trimmed
                          
                        while(parseFloat(startTop1) <= parseFloat(endTop1)) //Getting all the distinct elements in array
                        {                            
                            startTop = tempEl.style["top"];
                            startTop1 =  startTop.replace('px', '');

                            var chckElementTop = true;
                            for ( var x = 0; x <= selElementsTop.length-1; x++ ) {
                                var checkElement = selElementsTop[x];
                                if(checkElement==startTop){
                                    chckElementTop = false;
                                    break;
                                }
                            }
                        
                            if(chckElementTop){
                                selElements.push(tempEl);
                                selElementsTop.push(tempEl.style["top"]);                               
                            }
                            tempEl = tempEl.nextSibling;
                            startTop = tempEl.style["top"];
                            startTop1 =  startTop.replace('px', '');
                        }
                    
                        var tempCount = true;
                        for ( var x = 0; x <= selElementsTop.length-1; x++ ) {
                            var lineElement = selElements[x];
                            if(tempCount){
                                allParentHTML += getCompleteLine(lineElement);
                                tempCount = false;
                            }else{
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

    function getCompleteLine(currEl){
        var tempTop = currEl.style["top"];
        //alert($('[top='+tempTop+']'));

        var prevTop="", newTop="";
        //var cuurrEl  = null;
        var origCurrEl = currEl;
        //currEl = sel.getRangeAt(0).startContainer.parentNode;
        //alert(currEl.outerHTML);
        allParentHTML = '<div id="ParentHTML">';
        //prevTop = currEl.style["top"]; //current selected div's top
                   
        prevTop = currEl.style["top"];
        if(currEl.previousSibling != null)
        {
            currEl = currEl.previousSibling;
            newTop = currEl.style["top"];
        }
        
                    
        while(newTop == prevTop) //THis condition is used for picking complete line before selection
        {
            allParentHTML = currEl.outerHTML + allParentHTML;
            prevTop = currEl.style["top"];
            if(currEl.previousSibling!=null)
            {
                currEl = currEl.previousSibling;
                newTop = currEl.style["top"]; 
            }
            else{
                newTop = currEl.style["top"]; 
                break;
            }
            //alert(currEl.outerHTML);
                  
            //if (newTop != prevTop) {
            //    allParentHTML += "&lt;br/&gt;";
            //}
        }
        currEl = origCurrEl;//sel.getRangeAt(0).startContainer.parentNode;
        do {
            allParentHTML += currEl.outerHTML;
            //alert(currEl.outerHTML);
            prevTop = currEl.style["top"];
            if(currEl.nextSibling != null)
            {
                currEl = currEl.nextSibling;
                newTop = currEl.style["top"];
            }
            else{
                newTop = currEl.style["top"];
                break;
            }
            //if (newTop != prevTop) {
            //  allParentHTML += "&lt;br/&gt;";
            //}
        } while(newTop == prevTop); //THis condition is used for picking complete line after selection
        //while (currEl != sel.getRangeAt(0).endContainer.parentNode);  //THis condition is used for picking the selected text
        //allParentHTML += currEl.outerHTML;
        allParentHTML += '</div>';
        return allParentHTML;
    }

</script>
<%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
<%--<asp:HiddenField ID="HiddenField1" runat="server" ClientIDMode="Static" />--%>
<input type="hidden" clientidmode="Static" id="FileLoadPathHTML" runat="server" value="" />
<asp:HiddenField ID="FileLoadPath" ClientIDMode="Static" runat="server" Value="" />
<asp:HiddenField ID="pageXML" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="OriginalText" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="hfPageAllText" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="ModifiedText" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="CommentsEnterd" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="SelectedTextWithBreaks" runat="server" ClientIDMode="Static" />
<asp:HiddenField ID="ShowPopUpNow" runat="server" ClientIDMode="Static" Value="0" />
<asp:HiddenField ID="SelectedXMLParents" runat="server" ClientIDMode="Static" />
<%--<img id="btn" src="Images/1344591618_edit-notes.png" style="height: 30px; width: 30px;
    display: none;" alt="Edit" onclick="getSelText()" />--%>
<div style="display: none">
    hello world</div>
<div id="outerContainer">
    <asp:HiddenField runat="server" ID="hfTable" />
    <asp:HiddenField runat="server" ID="hfTableText" />
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
                    <%-- <textarea id="taSelText" name="selectedtext" rows="2" cols="20" style="display: none;"></textarea>--%>
                    <%--  <textarea id="taSelParent" name="selectedparent" rows="2" cols="20" style="display: none"></textarea>--%>
                    <asp:Button ID="btnGetSelText" runat="server" ClientIDMode="Static" Text="Update"
                        OnClientClick="SendXml();" Style="display: none" />
                    <asp:Button ID="btnSplit" runat="server" Text="Splite" OnClientClick="SendXml();"
                        ClientIDMode="Static" Style="display: none" />
                    <asp:Button ID="btnMerger" runat="server" Text="Merge" OnClientClick="SendXml();"
                        ClientIDMode="Static" Style="display: none" />
                    <%-- <asp:Button ID="btnLogMistake" runat="server" Text="Log Mistake" OnClientClick="SendXml();"
                        AccessKey="e" ClientIDMode="Static" Style="display: none" OnClick="btnLogMistake_Click" />--%>
                    <asp:Button ID="btnLogMistake" runat="server" Text="Log Mistake" OnClientClick="LogMistakesInXml();return false"
                        AccessKey="e" ClientIDMode="Static" Style="display: none" />
                    <asp:Button ID="btnNavigateNextPage" runat="server" Text="" OnClick="btnNavigateNextPage_Click"
                        AccessKey="e" ClientIDMode="Static" Style="display: none" />
                    <asp:Button ID="btnNavigatePrevPage" runat="server" Text="" OnClick="btnNavigatePrevPage_Click"
                        AccessKey="e" ClientIDMode="Static" Style="display: none" />
                    <asp:Button ID="btnShowTablePopUp" runat="server" Text="" OnClientClick="SendXml();"
                        AccessKey="e" ClientIDMode="Static" Style="display: none" OnClick="btnShowTablePopUp_Click" />
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
                <canvas id="myCanvas" width="578" height="200"></canvas>
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
<%--<div id="divDialogue" style="display: none; border: 4px solid #D6D6D6; position: absolute;
    left: 150px; top: 250px; border-top-right-radius: 20px; border-bottom-right-radius: 20px;
    padding: 6px; border-bottom-left-radius: 20px; background-color: #F0F0F0;">
    <div id='cssmenu'>
        <asp:Button ID="btnAddError" runat="server" Text="Error" OnClick="btnLogMistake_Click"
            OnClientClick="SendXml();" Style="margin-left: 82px;" />
        <div id="divNodeType" runat="server" style="margin-top: 5px;">
            Node Type :
            <label for="paraType">
            </label>
        </div>
        <div id="divCommentsLabel" runat="server">
        
        Comments :</div>
        <div id="divComments" runat="server" style="margin-top: 5px;">
            <asp:TextBox ID="tbxComments" Width="220px" Height="40px" TextMode="MultiLine" runat="server"></asp:TextBox>
        </div>
    </div>
</div>--%>
<%--<div id="divDialogue" style="display: none; border: 4px solid #D6D6D6; position: absolute;
    width: 345px; height: 155px; left: 150px; top: 250px; border-top-right-radius: 20px;
    border-bottom-right-radius: 20px; padding: 6px; border-bottom-left-radius: 20px;
    background-color: #F0F0F0;">
    <div id='Div2'>
        <table>
            <tr>
                <td>
                    <asp:RadioButtonList ID="rbtnlInsertMistake" runat="server" RepeatDirection="Horizontal"
                        ValidationGroup="insertMistake">
                        <asp:ListItem Text="Text"></asp:ListItem>
                        <asp:ListItem Text="Image"></asp:ListItem>
                        <asp:ListItem Text="Table"></asp:ListItem>
                        <asp:ListItem Text="Missing Para"></asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DropDownList ID="ddlMissingParaPosition" runat="server" Style="margin-left: 270px;
                        margin-top: -21px;">
                        <asp:ListItem>Up</asp:ListItem>
                        <asp:ListItem>Below</asp:ListItem>
                    </asp:DropDownList>
                    <asp:Label ID="lblDescription" runat="server" Text="Description:" Style="margin-left: 8px;"></asp:Label>
                    <asp:TextBox runat="server" ID="tbxDescription" TextMode="MultiLine" Rows="3" Columns="38"
                        Style="margin-left: 8px; resize: none;">
                    </asp:TextBox>
                </td>
            </tr>
        </table>
        <br />
        <asp:Button ID="Button1" runat="server" Text="Insert Error" OnClick="btnLogMistake_Click"
            Style="margin-left: 11px;" OnClientClick="SendXml();" />
    </div>
</div>--%>
<%--<div id="dialog" title="Error Message">
    <p style="margin-left: 5px; color: Fuchsia;">
        Mistake is not inserted because more then 1 line is selected.</p>
</div>--%>
<!-- outerContainer -->
<div id="divHtmlTable" runat="server" visible="false">
    <div style="display: block; left: 0; top: 0; position: fixed; width: 100%; height: 100%;
        padding: 200px; opacity: 0.7; background-color: Gray;">
    </div>
    <div style="position: fixed; left: 35%; top: 10%; border: solid 6px #D6D6D6; border-radius: 15px;
        background-color: White; padding: 20px;">
        <div id="divTableText" runat="server" style="scroll: auto">
        </div>
        <%--<asp:Button ID="btnClose" runat="server" Text="Close" OnClick="btnClose_Click" />--%>
    </div>
</div>
<div id="divBackground" class="modal">
</div>
<div id="divImage" class="info">
    <table style="height: 100%; width: 100%">
        <tr>
            <td valign="middle" align="center">
                <img id="imgLoader" alt="" src="../images/loader.gif" />
                <div id="divTableText">
                </div>
            </td>
        </tr>
        <tr>
            <td align="center" valign="bottom">
                <input id="btnClose" type="button" value="close" onclick="HideLightBox()" />
            </td>
        </tr>
    </table>
</div>
<div id="dialog" style="display: none; overflow: auto">
    <div id="dialogData">
    </div>
</div>
<div id="divDialogue" style="display: none; border: 4px solid #D6D6D6; position: absolute;
    left: 150px; top: 250px; border-top-right-radius: 20px; border-bottom-right-radius: 20px;
    padding: 6px; border-bottom-left-radius: 20px; background-color: #F0F0F0;">
    <div id='cssmenu'>
        <asp:Button ID="btnAddError" runat="server" Text="Error" OnClientClick="LogMistakesInXml();return false;"
            Style="margin-left: 82px;" />
        <div id="divNodeType" runat="server" style="margin-top: 5px;">
            Node Type :
            <label for="paraType">
            </label>
        </div>
        <div id="divCommentsLabel" runat="server">
            Comments :</div>
        <div id="divComments" runat="server" style="margin-top: 5px;">
            <asp:TextBox ID="tbxComments" Width="220px" Height="40px" TextMode="MultiLine" runat="server"></asp:TextBox>
        </div>
    </div>
</div>

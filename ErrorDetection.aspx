<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ErrorDetection.aspx.cs"
    Inherits="Outsourcing_System.ErrorDetection" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <script src="FinalScripts/jquery-1.9.1.js" type="text/javascript"></script>
    <link href="FinalStyles/minimal.css" rel="stylesheet" type="text/css" />
    <script src="FinalScripts/pdf.js" type="text/javascript"></script>
    <script src="FinalScripts/ui_utils.js" type="text/javascript"></script>
    <script src="FinalScripts/text_layer_builder.js" type="text/javascript"></script>
    <script src="FinalScripts/minimal.js" type="text/javascript"></script>
    <%--     <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <link href="FinalStyles/ViewerControl.css" rel="stylesheet" type="text/css" />--%>
    <script src="scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <%-- <script src="scripts/jquery.highlight-5.js" type="text/javascript"></script>--%>
    <title>ReadHowYouWant PDF WebCompare Utility</title>
    <script src="FinalScripts/ifvisible.js" type="text/javascript"></script>
    <script src="FinalScripts/TImeMe.js" type="text/javascript"></script>
    <link href="FinalStyles/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" type="text/css" />
    <script src="FinalScripts/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        //$(function() {
        //        $('#ibtnNext').hide();
        //        $('#ibtnPrevious').hide();
        //});

        //function DisableButton() {

        //    $('#btnNext').hide();
        //    $('#btnPrevious').hide();
        //}




        //var iframe = document.createElement('iframe');
        //iframe.onload = function () { alert('myframe is loaded'); }; // before setting 'src'


        //$('<iframe />').load(function () {
        //    alert('the iframe is done loading');
        //}).appendTo('body');


        //$(document).ready(function () {
        //    alert('showCidFontLines_ready');
        //    $('.textLayer div').css('background-color', '#FFFF00');
        //    $('.textLayer div').css('color', '#FF0000');
        //});

        //function showCidFontLines() {

        //    alert('showCidFontLines');
        //    $('.textLayer div').css('background-color', '#FFFF00');
        //    $('.textLayer div').css('color', '#FF0000');
        //}


        //function DisableButton() {
        //    alert('222322');
        //    document.getElementById('ibtnPrevious').style.display = "none";
        //    document.getElementById('ibtnNext').style.display = "none";
        //}
        //window.onbeforeunload = DisableButton;

        $(function () {

            ////            // bind the events (jQuery way)
            ////            $(document).ready(function () {
            ////                bindEvents();
            ////            });

            ////            // attach the event binding function to every partial update
            ////            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(function (evt, args) {
            ////                bindEvents();
            ////            });

            //            alert('window.name = ' + window.name);

            var srcPdf = $("#divNavigateSrcPdfName");
            var prdPdf = $("#divNavigatePrdPdfName");

            $(this).css("height", "0.5%");
            $(srcPdf).css("height", "0%");
            $(prdPdf).css("height", "0%");

            $(this).css({ 'z-index': '999' });
            $(srcPdf).css({ 'z-index': '-10' });
            $(prdPdf).css({ 'z-index': '-10' });

            //alert('111');

            $("#divNavigationPanel").hover(
                function () {
                    $(this).css("height", "25%");
                    $(this).css({ 'z-index': '999' });
                    $(srcPdf).css("height", "20%");
                    $(srcPdf).css({ 'z-index': '999' });
                    $(prdPdf).css("height", "20%");
                    $(prdPdf).css({ 'z-index': '999' });
                    $("#btnGoTo").show();
                    $("#btnFinish_Task").show();

                }, function () {
                    $(this).css("height", "0.5%");
                    $(srcPdf).css("height", "0%");
                    $(prdPdf).css("height", "0%");

                    $(this).css({ 'z-index': '60' });
                    $(srcPdf).css({ 'z-index': '-10' });
                    $(prdPdf).css({ 'z-index': '-10' });
                });
        });

        //$(function () {

        //    //            alert('window.name = ' + window.name);

        //    var srcPdf = $("#divNavigateSrcPdfName");
        //    var prdPdf = $("#divNavigatePrdPdfName");

        //    alert('222');

        //    $("#divNavigationPanel").hover(
        //        function () {
        //            $(this).css("height", "25%");
        //            $(this).css({ 'z-index': '999' });
        //            $(srcPdf).css("height", "25%");
        //            $(srcPdf).css({ 'z-index': '999' });
        //            $(prdPdf).css("height", "25%");
        //            $(prdPdf).css({ 'z-index': '999' });

        //        }, function () {
        //            $(this).css("height", "1.1%");
        //            $(srcPdf).css("height", "1.1%");
        //            $(prdPdf).css("height", "1.1%");
        //        });
        //});

        $(function () {

            var divLegnd = $("#divLegend");

            $(divLegnd).css("height", "50%");
            $(divLegnd).css("width", "0.1%");
            $(divLegnd).css({ 'z-index': '999' });

            $("#divLegend").hover(
                function () {
                    $(this).css("height", "50%");
                    $(this).css("width", "22%");
                    $(this).css({ 'z-index': '999' });

                }, function () {
                    $(this).css("height", "50%");
                    $(this).css("width", "0.1%");
                    $(this).css({ 'z-index': '60' });
                });
        });

        TimeMe.setIdleDurationInSeconds(30);
        TimeMe.setCurrentPageName("Comparison");
        TimeMe.initialize();

        //                window.onload = function() {
        //                    setInterval(function() {
        //                        var timeSpentOnPage = TimeMe.getTimeOnCurrentPageInSeconds();
        //                        document.getElementById('timeInSeconds').textContent = timeSpentOnPage;
        //                    }, 25);
        //                };

        window.onbeforeunload = function (event) {
            var timeSpentOnPage = TimeMe.getTimeOnCurrentPageInSeconds();
            SaveComparisonTimeSpent(timeSpentOnPage);
        };

        function SaveComparisonTimeSpent(timeSpent) {
            methodURL = "ErrorDetection.aspx/SaveTimeSpent";
            parameters = "{'text':'" + timeSpent + "'}";

            //var obj = {};
            //obj.text = timeSpent;

            //    data: JSON.stringify(obj),


            $.ajax({
                type: "POST",
                //data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                url: methodURL,
                data: parameters,
                dataType: "json",
                async: false,
                success: function (data) {

                    //                    $("#lblTimeSpent").innerText = data.d;
                    document.getElementById('<%= hfComparisonTimeSpent.ClientID %>').value = data.d;
                }
            });
            }


            function ShowComparisonTimeSpent() {

                var time = document.getElementById('<%= hfComparisonTimeSpent.ClientID %>').value;
            $("#lblTimeSpent").html(time);
            //            $("#lblTimeSpent").innerHTML = time;
        }







        ///////////////////////////////////////////////////////Timer Code////////////////////////////////////////////////////////

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
            //            document.getElementById('txt1').value = dis(mins, secs);

           // alert("min = " + mins + ", sec = " + secs);
            document.getElementById('txt1').innerText = dis(mins, secs);
            document.getElementById('<%= timehdnmin.ClientID %>').value = mins;
            document.getElementById('<%= timehdnsec.ClientID %>').value = secs;

            if ((mins == 0) && (secs == 0)) {
                window.alert("Time is up. Press OK to continue.");

                var testType = document.getElementById('<%= hfComparisonTaskType.ClientID %>').value;

                if (testType == 'onepagetest') {
                    ShowPopupQuizResult();
                }
                else {
                    comparisonEntryTest_TimeUp();
                }
            } else {

                cd = setTimeout("redo()", 1000);
            }
        }

        function comparisonEntryTest_TimeUp() {

            //                alert('2222');
            $.ajax({
                type: "POST",
                url: 'ErrorDetection.aspx/GetTestResult',
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {

                    //                        alert('result is ' + res.d);
                    if (res.d >= 80)
                        window.location = "Passed.aspx";
                    else
                        window.location = "Failed.aspx";
                }
            });
        }

        function Timer() {
            //alert('333');
            $("#divFinishQuiz").show();
            mins = document.getElementById('<%= timehdnmin.ClientID %>').value;
            secs = document.getElementById('<%= timehdnsec.ClientID %>').value;
            redo();
        }

        ///////////////////////////////////////////////end//////////////////////////////////////////////////////////




        function BtnFinishTask() {
            document.getElementById('<%= hfBtnFinishClicked.ClientID %>').value = "1";
        };

        //////////////////////////////////////////////Dialog Code//////////////////////////////////////////////////
        function ShowPopupQuizResult() {
            $(function () {
                GetQuizResult();

                var NewUserDialog = $("#dialogQuizResult").dialog({
                    appendTo: "#dialogAfterMeQuiz",
                    title: "Quiz Result",
                    height: 200,
                    width: 400,
                    position: "center",
                    resizable: false,
                    modal: true
                });
            });
        };

        function CloseResultDialog() {
            $("#dialogQuizResult").dialog('close');
        }

        ///////////////////////////////////////////////end//////////////////////////////////////////////////////////

        function GetQuizResult() {

            $.ajax({
                type: "POST",
                url: 'ErrorDetection.aspx/GetQuizResult',
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d != null) {

                        result = data.d;
                        $("#divQuiz").html(result);
                    }
                }
            });
        }

    </script>
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


        //Turn on Chrome PDF viewer

        //1.	Go to chrome://plugins
        //2.	Under "Chrome PDF Viewer," click Enable PDF viewing.
        //Chrome will now open PDFs automatically when you click on them.




        ////        function GetZoomIcon_MouseClick(e) {

        ////            var rr = e.toElement.innerHTML.replace("&nbsp;", "");

        ////            if (rr == 'here') {

        ////                //            alert('finally clicked once');
        ////                var table_Html = document.getElementById('<%= hfTableText.ClientID %>').value;
        ////                var table_Text = document.getElementById('<%= hfTable.ClientID %>').value;

        ////                //            var temp_Table = table_Text.split("~//~");

        ////                var res = table_Text.split(" ");

        ////                var parentElement = getSelectionParentsOuterHTML();
        ////                var selectedTextLine = getInnerHTML(parentElement);

        ////                //            alert('complete selected text line is ' + selectedTextLine);

        ////                var mainDiv = document.getElementById("mainContainer");

        ////                for (i = 0; i < res.length; i++) {

        ////                    if (res[i].trim().toLowerCase() == window.getSelection().anchorNode.nodeValue.trim().toLowerCase()) {

        ////                        //                    alert('equal');
        ////                        ShowPopupJq(table_Html);
        ////                        break;
        ////                    }
        ////                }
        ////            }
        ////        }

        var getPage_Comments = 0;


        ///////////////////////////Change of cursor to zoom icon when cursor is moved over 'click here to open'////////////////////////////
        function GetParaNames(e) {

            var pageText = document.getElementById('<%= hfPageParasType.ClientID %>').value;
            if (pageText == '') {

                var fullPageHtml = $('.textLayer').html();

                SetParaNameAsTooltip(fullPageHtml);
            }




            <%--var hoverText = e.toElement.innerHTML.replace("&nbsp;", "");

          if (hoverText.length <= 20) {
                var table_Text = document.getElementById('<%= hfTable.ClientID %>').value;

                if (table_Text != '') {
                    var res = table_Text.split(" ");

                    if ((hoverText == 'Click') || (hoverText == 'here') || (hoverText == 'to') || (hoverText == 'Open') || (hoverText == 'Table')) {
                        $('.textLayer div').css('cursor', '-webkit-zoom-in');
                    }
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
            }--%>
        }


        ///////////////////////////Change of cursor to zoom icon when cursor is moved over 'click here to open'////////////////////////////
        //old method
        function GetZoomIcon_MouseOver(e) {

            var hoverText = e.toElement.innerHTML.replace("&nbsp;", "");

            if (hoverText.length <= 20) {
                var table_Text = document.getElementById('<%= hfTable.ClientID %>').value;

              if (table_Text != '') {
                  var res = table_Text.split(" ");

                  if ((hoverText == 'Click') || (hoverText == 'here') || (hoverText == 'to') || (hoverText == 'Open') || (hoverText == 'Table')) {
                      $('.textLayer div').css('cursor', '-webkit-zoom-in');
                  }
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
          }
      }


      function OnSelectionGetPassedTests() {

          methodURL = "ErrorDetection.aspx/GetPassedTests";

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

          var pageCompleteText = $('.textLayer')[0].innerHTML;
          var tbxCommments = $('#<%= tbxComments.ClientID %>').val();
            var temp = tbxCommments + '/~/' + lineText + '/~/' + pageCompleteText;

            var obj = {};
            obj.text = temp;


            methodURL = "ErrorDetection.aspx/GetParaType";
            //parameters = "{'text':'" + temp + "'}";
            var returnVal = false;
            $.ajax({
                //error: function (request, status, error) {
                //    alert(request.responseText);
                //},
                type: "POST",
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                url: methodURL,
                //data: parameters,
                dataType: "json",
                async: false,
                //traditional: true,
                success: function (data) {

                    if (data.d != null) {

                        if (data.d != '') {

                            var errorText = data.d.split(",");

                            if ((errorText != '') && (errorText.length > 1)) {

                                var status = errorText[1];

                                if (status.trim() == 'true') {
                                    btnAddError.value = 'Undo';
                                    jQuery("label[for='paraType']").html("<span style='color:DarkBlue;font:bold;font-size:large'>" + errorText[0] + "</span>");
                                }
                            }
                            else {
                                btnAddError.value = 'Error';
                                jQuery("label[for='paraType']").html("<span style='color:DarkBlue;font:bold;font-size:large'>" + data.d + "</span>");
                            }
                        }

                        //jQuery("label[for='paraType']").html("<span style='color:DarkBlue;font:bold;font-size:large'>" + data.d + "</span>");
                    }
                }
                //,

                //error: function (request, status, error) {
                //alert(request.responseText);
            });
        }
        /////////////////////////////////////////////////////////////////////end//////////////////////////////////////////////////////////////////////


        /////////////////////////////////Ajax method to get para name and show as tooltip/////////////////////////////////////////////////////////////
        function GetParaNameOnMouseHover(e) {

            var lineText = e.toElement.innerHTML.replace("&nbsp;", "");

            methodURL = "ErrorDetection.aspx/GetParaName";
            //parameters = "{'text':'" + lineText + "'}";

            var obj = {};
            obj.text = lineText;

            var returnVal = false;
            $.ajax({
                type: "POST",
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                url: methodURL,
                //data: parameters,
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
                                                        //$('.textLayer div').attr('background-color', 'yellow')
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


        /////////////////////////////////Ajax method to set comments of all errors in a page inserted through error dialog////////////////////////////
        function SetParaNameAsTooltip(lineText) {

            methodURL = "ErrorDetection.aspx/GetParaName";
            //parameters = "{'text':'" + lineText + "'}";

            var obj = {};
            obj.text = lineText;

            var returnVal = false;
            $.ajax({
                type: "POST",
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                url: methodURL,
                //data: parameters,
                dataType: "json",
                async: false,
                success: function (data) {

                    if (data.d != null) {

                        var divWithComments = data.d;

                        if (divWithComments != '' && divWithComments.length > 0) {

                            var allDivs = $('.textLayer div');
                            var lineType = '';

                            var lineDivs = divWithComments.split("~//~");

                            if ((lineDivs != '') && (lineDivs.length > 0)) {

                                for (i = 0; i < lineDivs.length; i++) {

                                    if (lineDivs[i] != '') {

                                        var lineDivNumbers = lineDivs[i].split(",");

                                        if ((lineDivNumbers != '') && (lineDivNumbers.length > 0)) {

                                            if (i + 1 < lineDivs.length)
                                                lineType = lineDivs[i + 1];

                                            for (j = 0; j < allDivs.length; j++) {

                                                for (k = 0; k < lineDivNumbers.length; k++) {

                                                    if ((j == lineDivNumbers[k]) && (lineDivNumbers[k] != '')) {

                                                        $(allDivs[j]).attr('title', lineType);
                                                        //$('.textLayer div').attr('background-color', 'yellow')
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }

                            document.getElementById('<%= hfPageParasType.ClientID %>').value = '1';

                           <%-- var pageText = document.getElementById('<%= hfPageParasType.ClientID %>').value;
                            if (pageText != '') {
                                document.getElementById('<%= hfPageParasType.ClientID %>').value = '';
                            }--%>
                        }

                        returnVal = true;
                        getPage_Comments = 1;
                    }
                }
            });

            return returnVal;
        }
        /////////////////////////////////////////////////////////////////////end//////////////////////////////////////////////////////////////////////


        /////////////////////////////////Ajax method to set comments of all errors in a page inserted through error dialog////////////////////////////
        function GetErrorComments(lineText) {

            methodURL = "ErrorDetection.aspx/GetErrorComments";
            //parameters = "{'text':'" + lineText + "'}";

            var obj = {};
            obj.text = lineText;

            var returnVal = false;
            $.ajax({
                type: "POST",
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                url: methodURL,
                //data: parameters,
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
                                                        //$('.textLayer div').attr('background-color', 'yellow')
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
            methodURL = "ErrorDetection.aspx/ShowComment_ByError";
            //parameters = "{'text':'" + lineText + "'}";
            var returnVal = false;

            var obj = {};
            obj.text = lineText;


            $.ajax({
                type: "POST",
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                url: methodURL,
                //data: parameters,
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

            if (btnAddError.value == 'Undo') UndoMistakesInXml();
            else if (btnAddError.value == 'Error') {
                var parentElement = getSelectionParentsOuterHTML();
                var selectedTextLine = getInnerHTML(parentElement);
                var pageCompleteText = $('.textLayer')[0].innerHTML;

                var tbxCommments = $('#<%= tbxComments.ClientID %>').val();

                var temp = tbxCommments + '/~/' + OriginalText.value + '/~/' + pageCompleteText;

                var obj = {};
                obj.text = temp;

                methodURL = "ErrorDetection.aspx/LogMistakes";
                //parameters = "{'text':'" + temp + "'}";
                var returnVal = false;
                $.ajax({
                    type: "POST",
                    data: JSON.stringify(obj),
                    contentType: "application/json; charset=utf-8",
                    url: methodURL,
                    //data: parameters,
                    dataType: "json",
                    async: false,
                    success: function (data) {

                        if (data.d != null) {

                            HighlightSelectedText(data.d);

                            $('#cssmenu').css("dislay", "none");
                            $('#divEditText').css("dislay", "none");
                            $('#divDialogue').css("display", "none");

                        }
                    }
                });
            }
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

    //$('.textLayer div').css('background-color', '#FFFF00'); $('.textLayer div').css('color', 'black');
    //$('.textLayer div').css('background-color', '#FFFF00'); $('.textLayer div').css('color', '#FF0000');
    /////////////////////////////////////////////////////////////////////end//////////////////////////////////////////////////////////////////////


    ////////////////////////////////////////////////Undo inserted error///////////////////////////////////////////////////////////////////////////
    function UndoMistakesInXml() {

        //alert('UndoMistakesInXml');
        var parentElement = getSelectionParentsOuterHTML();
        var selectedTextLine = getInnerHTML(parentElement);
        var pageCompleteText = $('.textLayer')[0].innerHTML;

        var tbxCommments = $('#<%= tbxComments.ClientID %>').val();

            var temp = tbxCommments + '/~/' + OriginalText.value + '/~/' + pageCompleteText;


            methodURL = "ErrorDetection.aspx/UndoMistakes";
            //parameters = "{'text':'" + temp + "'}";
            var returnVal = false;

            var obj = {};
            obj.text = temp;

            $.ajax({
                type: "POST",
                data: JSON.stringify(obj),
                contentType: "application/json; charset=utf-8",
                url: methodURL,
                //data: parameters,
                dataType: "json",
                async: false,
                success: function (data) {

                    if (data.d != null) {

                        RemoveHighlighting(data.d);

                        //if (data.d == 'true') {
                        $('#cssmenu').css("dislay", "none");
                        $('#divEditText').css("dislay", "none");
                        $('#divDialogue').css("display", "none");
                        //}

                    }
                }
            });
        }

        function RemoveHighlighting(selectedLineDivs) {

            if (selectedLineDivs != '') {

                var divs = selectedLineDivs.split(",");

                if ((divs != '') && (divs.length > 0)) {

                    var allDivs = $('.textLayer div');

                    for (j = 0; j < allDivs.length; j++) {

                        for (k = 0; k < divs.length; k++) {

                            if ((j == divs[k]) && (divs[k] != '')) {

                                $(allDivs[j]).css('color', '');
                                $(allDivs[j]).css('background-color', '');

                            }
                        }
                    }
                }
            }
        }
        /////////////////////////////////////////////////////////////////////end//////////////////////////////////////////////////////////////////////




        $(function () {

            //        window.name = "Comparison";
            var mainDiv = document.getElementById("divOuter");

            // mainDiv.onmouseover = GetParaNameOnMouseHover;
            mainDiv.onmouseover = GetParaNames;
            mainDiv.onmouseup = GetCoordinates;

            function GetCoordinates(e) {

                getPage_Comments = 0;

                var parentElement = getSelectionParentsOuterHTML();
                var selectedTextLine = getInnerHTML(parentElement);

                OriginalText.value = selectedTextLine;

                hfPageAllText.value = e.target.innerHTML;

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
                    $("#divDialogue").css("left", posX - 245 + "px");
                    $("#divDialogue").css("top", posY - 40 + "px");
                    $('#cssmenu').css("display", "block");


                    GetTextType(selectedTextLine);

                    ShowErrorComment(selectedTextLine);

                }
                else {
                    $('#cssmenu').css("dislay", "none");
                    $('#divEditText').css("dislay", "none");
                    $('#divDialogue').css("display", "none");


                    //On clicking on table icon to show table is commented
                    //                    if (window.getSelection().anchorOffset > 0) {

                    //                        var table_Html = document.getElementById('<%= hfTableText.ClientID %>').value;
                    //                        var table_Text = document.getElementById('<%= hfTable.ClientID %>').value;
                    //                        var startIndex = selectedTextLine.indexOf('here');

                    //                        //Get selected table number from hidden field by matching clicked text with its text.
                    //                        var tableNumber = 0;
                    //                        var temp_Table = table_Text.split("~//~");

                    //                        if ((temp_Table != '') && (temp_Table.length > 0)) {

                    //                            for (i = 0; i < temp_Table.length; i++) {
                    //                                if (temp_Table[i] != '') {

                    //                                    var res = temp_Table[i].split(" ");

                    //                                    if (res != '') {
                    //                                        for (j = 0; j < res.length; j++) {
                    //                                            if (res[j] != '') {
                    //                                                if (res[j].trim().toLowerCase() == window.getSelection().anchorNode.nodeValue.trim().toLowerCase()) {
                    //                                                    tableNumber = i;
                    //                                                    break;
                    //                                                }
                    //                                            }
                    //                                        }
                    //                                    }
                    //                                }
                    //                            }
                    //                        }


                    //                        var temp_TableHtml = table_Html.split("~//~");
                    //                        var current_Table = temp_TableHtml[tableNumber];

                    //                        if (window.getSelection().anchorNode.nodeValue.trim().toLowerCase() == 'here') {

                    //                            ShowPopupJq(current_Table);
                    //                        }

                    //                        else {
                    //                            var res = table_Text.split(" ");

                    //                            for (i = 0; i < res.length; i++) {
                    //                                if (res[i].trim().toLowerCase() == window.getSelection().anchorNode.nodeValue.trim().toLowerCase()) {
                    //                                    ShowPopupJq(current_Table);
                    //                                    break;
                    //                                }
                    //                            }
                    //                        } //end inner else
                    //                    } //end inner if
                } //end main else
            }
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
            $('#<%= btnNextPage.ClientID %>').click();
        }
        function NavigatePrevPageCallBack() {
            $('#<%= btnPreviousPage.ClientID %>').click();
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

            var parentElement = getSelectionParentsOuterHTML();
            var selectedTextWithBreaks = getInnerHTML(parentElement);

            ShowPopup();
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

    </script>
    <style>
        #iframe1 {
            height: 360px;
            width: 640px;
            margin: 0;
            padding: 0;
            border: 0;
        }

        #loader1 {
            position: absolute;
            left: 46.4%;
            top: 40%;
            border-radius: 20px;
            padding: 25px;
            border: 1px solid #777777;
            background: #ffffff;
            box-shadow: 0px 0px 10px #777777;
        }
    </style>
    <script>
        $(document).ready(function () {

            $('#srcImage').on('load', function () {
                $('#loader1').hide();
            });

            var textLayer = $('#pdfContainer').find('.textLayer');

            textLayer.bind('load', function () {
                $('#loader1').hide();
            });
        });
    </script>

    <script type="text/javascript">

        function showConfirmationDialog() {

            document.getElementById('<%= hfBtnFinishClicked.ClientID %>').value = "1";

            $("#divFinishTaskConfirm").css("display", "block");

            var taskType = document.getElementById('<%= hfTaskType.ClientID %>').value;

            if (taskType == "starttest") {

                $("#skipConfirmDialogStartTest").dialog({
                    resizable: false,
                    height: 100,
                    width: 463,
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
                        $("#btnGoTo").hide();
                        $("#btnFinish_Task").hide();
                    }
                });


            } else {

                GetFinishBtnClickedCount();
                $("#skipConfirmDialog").dialog({
                    resizable: false,
                    height: 220,
                    width: 543,
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
                        $("#btnGoTo").hide();
                        $("#btnFinish_Task").hide();
                    }
                });
            }
        }

        function showProductionPanel() {

            $('#<%= btnInternal.ClientID %>').click();
        }

        function GetFinishBtnClickedCount() {

            var bookId = document.getElementById('<%= hfBookId.ClientID %>').value;

            methodURL = "ErrorDetection.aspx/GetFinishBtnClickedCount";
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

                        var msg;

                        if (data.d == 1) {
                            msg = data.d + " chance";
                        }
                        else if (data.d > 1) {
                            msg = data.d + " chances";
                        }
                        $('#<%=lblFinishClickedCount.ClientID%>').html(msg);
                    }
                }
            });
        }
    </script>
</head>
<body>
    <form id="form" runat="server">
        <asp:HiddenField runat="server" ID="timehdnmin" />
        <asp:HiddenField runat="server" ID="timehdnsec" />
        <asp:HiddenField runat="server" ID="hfComparisonTimeSpent" />
        <asp:HiddenField runat="server" ID="hfComparisonTaskType" />
        <asp:HiddenField runat="server" ID="hfBtnFinishClicked" />
        <asp:HiddenField ID="hfFileLoadPath" ClientIDMode="Static" runat="server" Value="showPdf.ashx" />
        <asp:HiddenField ID="hfPdfDimensions" ClientIDMode="Static" runat="server" />
        <asp:HiddenField runat="server" ID="hfTable" />
        <asp:HiddenField runat="server" ID="hfTableText" />
        <asp:HiddenField ID="FileLoadPath" ClientIDMode="Static" runat="server" Value="" />
        <asp:HiddenField ID="pageXML" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="OriginalText" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hfPageAllText" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="ModifiedText" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="CommentsEnterd" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="SelectedTextWithBreaks" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="ShowPopUpNow" runat="server" ClientIDMode="Static" Value="0" />
        <asp:HiddenField ID="SelectedXMLParents" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hfBookId" runat="server" ClientIDMode="Static" />
        <asp:HiddenField ID="hfTaskType" runat="server" ClientIDMode="Static" />

        <asp:HiddenField ID="hfPageParasType" runat="server" ClientIDMode="Static" />

        <asp:Button ID="btnInternal" runat="server" OnClick="btnFinish_Task_Click" Style="display: none;" />

        <div id="divNavigationPanel" style="position: fixed; top: 0px; left: 29%; width: 43%; border-bottom-right-radius: 20px; padding: 6px; border-bottom-left-radius: 20px; color: White; background-color: #3A7676; height: 1px; overflow: hidden; padding-top: 10px;">
            <table style="width: 100%; margin-top: -14px;">
                <tr id="trTimer" runat="server" align="center">
                    <td>
                        <asp:Label ID="lblTimeRemaining" runat="server" Style="margin-left: 120px; color: red; font-size: large;"
                            Text="">Time Remaining : </asp:Label>
                        <label id="txt1" style="margin-right: 150px; background-color: #3A7676; border-color: #3A7676; color: red; font-size: large;">
                        </label>
                    </td>
                    <%-- <td>
                    <label id="txt1" style="margin-right: 150px; background-color: #3A7676; border-color: #3A7676;
                        color: red; font-size: large;">
                    </label>
                </td>--%>
                </tr>
                <tr>
                    <td>
                        <br />
                    </td>
                </tr>
                <tr id="trComparisonTimeSpent" runat="server">
                    <td>
                        <span style="margin-left: 13px; color: white;">Time : </span><span style="margin-left: 2px; margin-top: 20px; background-color: #3A7676; border-color: #3A7676; color: white;"
                            id="lblTimeSpent"></span>
                    </td>
                </tr>
            </table>
            <div style="width: 100%; padding-top: 2px; padding-left: 15px;">
                <table style="width: 100%; margin-top: 0%">
                    <tr id="trNavigateMistake" runat="server">
                        <td>Navigate Mistakes
                        </td>
                        <td align="left">
                            <asp:Panel runat="server" ID="pnlNav" DefaultButton="btnGoToError">
                                <asp:TextBox Width="40px" runat="server" ID="TextErrorNum" />&nbsp;&nbsp;/&nbsp;
                            <asp:Label ID="lblTotalErrors" runat="server" Text="0"></asp:Label>
                                <asp:Button ID="btnGoToError" runat="server" Text="GoTo" OnClick="btnGoToError_Click" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr>
                        <td>Navigate Pages
                        </td>
                        <td align="left">
                            <asp:Panel runat="server" ID="Panel1" DefaultButton="btnGoTo">
                                <asp:Button ID="btnPreviousPage" runat="server" Text="<" Width="60px" OnClick="ibtnPrev_Click" />
                                <asp:TextBox Width="40px" runat="server" ID="txtPageNum" />&nbsp;&nbsp;/&nbsp;
                            <asp:Label ID="lblTotalPages" runat="server"></asp:Label>&nbsp;&nbsp;
                            <asp:Button ID="btnNextPage" runat="server" Text=">" Width="60px" OnClick="ibtnNext_Click" />
                            </asp:Panel>
                        </td>
                    </tr>
                    <tr id="trcheckBoxes" runat="server">
                        <td>Keep
                        </td>
                        <td align="left">
                            <asp:CheckBoxList ID="cbxlFont" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Text="Font-Name"></asp:ListItem>
                                <asp:ListItem Text="Font-Size"></asp:ListItem>
                                <asp:ListItem Text="Text-Position"></asp:ListItem>
                                <asp:ListItem Text="Page-Size"></asp:ListItem>
                            </asp:CheckBoxList>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td>
                            <asp:Button ID="btnGoTo" runat="server" Text="Generate Preview" OnClick="btnGoTo_Click"
                                OnClientClick="ShowLoadingGif();" />
                            <%-- <asp:Button ID="btnFinish_Task" runat="server" Text="Finish Task" OnClick="btnFinish_Task_Click"
                            OnClientClick="BtnFinishTask();" />--%>

                            <input id="btnFinish_Task" type="button" value="Finish Task" onclick="showConfirmationDialog();" />

                            <div id="divFinishQuiz" style="margin-left: 34%; margin-top: -5.6%;" runat="server"
                                visible="False">
                                <input id="btnFinishTask" type="button" value="Finish Test" onclick="ShowPopupQuizResult();" />
                            </div>

                            <%-- <input type="button" value="Skip this Step" onclick="showConfirmationDialog();" 
                             style="width: 150px; height: 30px; border-radius: 18px; background-color: darkgray;
                             margin-top: 10px; margin-left: 10%; color: floralwhite; font-size:65%"
                        runat="server" />--%>

                        </td>
                    </tr>
                </table>
            </div>
        </div>

        <div id="divLegend" style="position: fixed; top: 0px; left: 0%; border-bottom-right-radius: 20px; padding: 6px; border-bottom-left-radius: 20px; color: White; background-color: #3A7676; height: 1px; overflow: hidden; padding-top: 10px;">
            <table style="width: 100%; margin-top: 8px;">
                <%--<tr>
                    <td align="center" style="width: 30%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: lawngreen; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">UPara</td>
                </tr>
                <tr>
                    <td align="center" style="width: 30%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: yellow; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">SPara</td>
                </tr>
                <tr>
                    <td align="center" style="width: 30%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: deepskyblue; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">NPara</td>
                </tr>
                <tr>
                    <td align="center" style="width: 30%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: deepskyblue; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Chapter</td>
                </tr>
                <tr>
                    <td align="center" style="width: 30%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: deepskyblue; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Level 1</td>
                </tr>
                <tr>
                    <td align="center" style="width: 30%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: deepskyblue; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Level 2</td>
                </tr>
                <tr>
                    <td align="center" style="width: 30%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: deepskyblue; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Level 3</td>
                </tr>
                <tr>
                    <td align="center" style="width: 30%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: deepskyblue; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Level 4</td>
                </tr>
                
                  <tr>
                    <td align="center" style="width: 30%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: #D3D3D3 ;; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Box</td>
                </tr>
                <tr>
                    <td align="center" style="width: 30%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: chocolate margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">FootNote</td>
                </tr>
                <tr>
                    <td align="center" style="width: 30%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: deepskyblue; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Table</td>
                </tr>
                <tr>
                    <td align="center" style="width: 30%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: deepskyblue; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">EndNote</td>
                </tr>--%>
                
               <%-- <tr>
                    <td align="center" style="width: 8%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: lawngreen; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Normal Para</td>
                </tr>--%>
                <tr>
                    <td align="center" style="width: 8%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: #ff4d4d; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Special Para (Poems, Letters etc)</td>
                </tr>
                <tr>
                    <td align="center" style="width: 8%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: #eda704; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Numbered & Bulleted Lists</td>
                </tr>
                <tr>
                    <td align="center" style="width: 10%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: #2ECC71; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Chapter</td>
                </tr>
                <tr>
                    <td align="center" style="width: 8%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: #130363; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Heading 1</td>
                </tr>
                <tr>
                    <td align="center" style="width: 8%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: #03440d; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Heading 2</td>
                </tr>
                <tr>
                    <td align="center" style="width: 8%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: #63020e; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Heading 3</td>
                </tr>
                <tr>
                    <td align="center" style="width: 8%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: #630261; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Heading 4</td>
                </tr>
                
                  <tr>
                    <td align="center" style="width: 10%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: #D3D3D3; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Highlighted Text</td>
                </tr>
                <tr>
                    <td align="center" style="width: 8%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: #DC0484; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">FootNote</td>
                </tr>
                <tr>
                    <td align="center" style="width: 8%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: #DF7522; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">Table</td>
                </tr>
                <tr>
                    <td align="center" style="width: 8%; vertical-align: central">
                        <div style="width: 80%; height: 20px; background-color: deepskyblue; margin-bottom: 5%;">
                        </div>
                    </td>
                    <td style="width: 30%; vertical-align: central; font-size: 95%;">EndNote</td>
                </tr>
            </table>
        </div>

        <div id="divNavigateSrcPdfName" style="position: fixed; top: 0px; left: 15.8%; width: 13%; border-bottom-right-radius: 20px; padding: 6px; border-bottom-left-radius: 20px; color: White; height: 1px; overflow: hidden; padding-top: 10px;">
            <div style="width: 100%; padding-top: 1px; padding-left: 15px;">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="50%" align="center">
                            <img src="img/hd2a.jpg" />
                    </tr>
                </table>
            </div>
        </div>
        <div id="divNavigatePrdPdfName" style="position: fixed; top: 0px; left: 69.9%; width: 20%; border-bottom-right-radius: 20px; padding: 6px; border-bottom-left-radius: 20px; color: White; height: 1px; overflow: hidden; padding-top: 10px;">
            <div style="width: 66%; padding-top: 1px; padding-left: 15px;">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td width="50%" align="center">
                            <img src="img/hd2b.jpg" />
                    </tr>
                </table>
            </div>
        </div>
        <div id="divOuter" style="margin: 0 auto; position: relative;">
            <div style="width: 1%;">
                <table width="100%" style="margin-left: auto; margin-right: auto">
                    <tr>
                        <td id="tdSrcImage">
                            <input type="image" id="srcImage" src="" style="border: 1px solid black; display: none" onclick="return false;"
                                runat="server" />
                        </td>
                        <td id="tdPrdPdf">
                            <div id="pdfContainer">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td></td>
                        <td align="center">

                            <asp:ImageButton ID="ibtnPrevious" runat="server" ImageUrl="img/previousBtnIcon.jpg" Visible="False" OnClick="btnPrevious_Click"
                                Style="display: inline-block; width: 4%; position: absolute; top: 50%; left: 52%;" />
                            <asp:ImageButton ID="ibtnNext" runat="server" ImageUrl="img/nextBtnIcon.jpg" Visible="False" OnClick="btnNext_Click"
                                Style="display: inline-block; width: 4%; position: absolute; top: 50%; left: 95.5%;" />
                        </td>

                    </tr>
                </table>
            </div>
        </div>
        <%-- <div id="divSubNavigation" style="position:absolute;transform: translate(80px, 10px);">--%>
        <%--<asp:Button ID="btnPrevious" runat="server" Visible="False" Text="<" style="display:inline-block; width:5%; position:absolute; top:60%;left:54%;" 
                                OnClick="btnPrevious_Click" /> 
                            <asp:Button ID="btnNext" runat="server"  Visible="False" Text=">" style="display:inline-block; width:5%;position:absolute; top:60%; left:90%;" OnClick="btnNext_Click" 
                                 /> --%>
        <%--</div>--%>

        <div id="divDialogue" style="display: none; border: 4px solid #D6D6D6; position: absolute; left: 150px; top: 250px; border-top-right-radius: 20px; border-bottom-right-radius: 20px; padding: 6px; border-bottom-left-radius: 20px; background-color: #F0F0F0;">
            <div id='cssmenu'>
                <asp:Button ID="btnAddError" runat="server" Text="Error" OnClientClick="LogMistakesInXml();return false;"
                    Style="margin-left: 82px;" />
                <div id="divNodeType" runat="server" style="margin-top: 5px;">
                    Node Type :
                <label for="paraType">
                </label>
                </div>
                <div id="divCommentsLabel" runat="server">
                    Comments :
                </div>
                <div id="divComments" runat="server" style="margin-top: 5px;">
                    <asp:TextBox ID="tbxComments" Width="220px" Height="40px" TextMode="MultiLine" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
        <div>
            <asp:Button ID="btnGetSelText" runat="server" ClientIDMode="Static" Text="Update"
                OnClientClick="SendXml();" Style="display: none" />
            <asp:Button ID="btnSplit" runat="server" Text="Splite" OnClientClick="SendXml();"
                ClientIDMode="Static" Style="display: none" />
            <asp:Button ID="btnMerger" runat="server" Text="Merge" OnClientClick="SendXml();"
                ClientIDMode="Static" Style="display: none" />
            <%-- <asp:Button ID="btnLogMistake" runat="server" Text="Log Mistake" OnClientClick="SendXml();"
                        AccessKey="e" ClientIDMode="Static" Style="display: none" OnClick="btnLogMistake_Click" />--%>
            <asp:Button ID="btnLogMistake" runat="server" Text="Log Mistake" OnClientClick="LogMistakesInXml();return false;"
                AccessKey="e" ClientIDMode="Static" Style="display: none" />
            <%--<asp:Button ID="btnNavigateNextPage" runat="server" Text="" OnClick="btnNavigateNextPage_Click"
                        AccessKey="e" ClientIDMode="Static" Style="display: none" />
                    <asp:Button ID="btnNavigatePrevPage" runat="server" Text="" OnClick="btnNavigatePrevPage_Click"
                        AccessKey="e" ClientIDMode="Static" Style="display: none" />
                    <asp:Button ID="btnShowTablePopUp" runat="server" Text="" OnClientClick="SendXml();"
                        AccessKey="e" ClientIDMode="Static" Style="display: none" OnClick="btnShowTablePopUp_Click" />--%>
        </div>
        <div id="frameWrap">
            <img id="loader1" src="img/loading.gif" width="36" height="36" alt="loading gif" />
        </div>
        <div id="dialogQuizResult" style="display: none; overflow: auto; width: 20px; height: 20px; background-color: #FFF1B9">
            <div id="divResultMessage" style="margin-right: 4px">
                <div style="margin-top: 20px; font-size: 12pt; color: Green; padding: 5px; font-family: Sans-Serif;"
                    id="divQuiz">
                </div>
            </div>
            <div style="margin-top: 30px;">
                <asp:Button ID="btnViewAnswer" runat="server" CssClass="button" Text="View Answer"
                    Style="width: 130px; border: 1px solid black; margin-left: 2px;" ValidationGroup="registrationForm"
                    UseSubmitBehavior="false" OnClick="btnViewAnswer_Click" Visible="true" />
                <asp:Button ID="btnCloseDialog" runat="server" CssClass="button" Text="Close" Style="width: 60px; border: 1px solid black; margin-left: 2px;"
                    ValidationGroup="registrationForm"
                    UseSubmitBehavior="false" OnClick="btnCloseDialog_Click" />
            </div>
        </div>
        <div id="dialogAfterMeQuiz">
        </div>

        <div id="skipConfirmDialog" title="Are you sure you want to finish Task ?">
            <div id="divFinishTaskConfirm" style="width: 100%; height: 50px; margin-top: 2.5%; display: none;">
                <div>
                    You have
                    <asp:Label ID="lblFinishClickedCount" runat="server"></asp:Label>
                    remaining to click on finish task.
                </div>
                <div style="margin-top: 2.5%;">Before finishing a task you have to view all pages.</div>
            </div>
        </div>

        <div id="skipConfirmDialogStartTest" title="Are you sure you want to finish Test ?">
            <div id="divFinishTaskConfirmStartTest" style="width: 100%; height: 50px; margin-top: 2.5%; display: none;">
            </div>
        </div>

    </form>
</body>
</html>

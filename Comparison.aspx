<%@ Page Language="C#" AutoEventWireup="true" Inherits="web_Comparison" ValidateRequest="false"
    CodeBehind="Comparison.aspx.cs" %>

<%@ Register Src="~/UserControls/PDFTextCmp.ascx" TagName="PDFComparison" TagPrefix="PDFCmp" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">--%>
    <meta http-equiv="Content-Type" content="text/html; charset=iso-8859-1" />
    <link href="FinalStyles/ViewerControl.css" rel="stylesheet" type="text/css" />
    <link rel="stylesheet" href="Styles/FreeTextBoxStyle.css" />
    <script src="scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script src="scripts/jquery-1.4.1.min.js" type="text/javascript"></script>
    <script src="scripts/jquery.highlight-5.js" type="text/javascript"></script>
    <title>ReadHowYouWant PDF WebCompare Utility</title>
    <link href="Styles/MainStyle.css" rel="stylesheet" type="text/css" />
    <script src="../FinalScripts/ifvisible.js" type="text/javascript"></script>
    <script src="../FinalScripts/TImeMe.js" type="text/javascript"></script>
    <script type="text/javascript">

   
//function disableBackButton()
//{
//window.history.forward();
//}
//setTimeout("disableBackButton()",0);



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
            methodURL = "Comparison.aspx/SaveTimeSpent";
            parameters = "{'text':'" + timeSpent + "'}";

            $.ajax({
                type: "POST",
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




        $(function () {

            //            alert('window.name = ' + window.name);

            var srcPdf = $("#divNavigateSrcPdfName");
            var prdPdf = $("#divNavigatePrdPdfName");

            $("#divNavigationPanel").hover(
                function () {
                    $(this).css("height", "25%");
                    $(this).css({ 'z-index': '999' });
                    $(srcPdf).css("height", "25%");
                    $(srcPdf).css({ 'z-index': '999' });
                    $(prdPdf).css("height", "25%");
                    $(prdPdf).css({ 'z-index': '999' });

                }, function () {
                    $(this).css("height", "1.1%");
                    $(srcPdf).css("height", "1.1%");
                    $(prdPdf).css("height", "1.1%");
                });
        });


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
                url: 'Comparison.aspx/GetTestResult',
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (res) {

                    //                        alert('result is ' + res.d);
                    if (res.d >= 60)
                        window.location = "Passed.aspx";
                    else
                        window.location = "Failed.aspx";
                }
            });
            //                PageMethods.SetName();
            //                window.location = "Step3.aspx";
        }

        //        function OnGetMessageSuccess() {

        //            //                alert('2222');
        //            $.ajax({
        //                type: "POST",
        //                url: 'Comparison.aspx/GetTestResult',
        //                data: "{}",
        //                contentType: "application/json; charset=utf-8",
        //                dataType: "json"
        //                    ,
        //                success: function (url) {

        //                    //                        alert('result is ' + url.d);
        //                    if (url.d != '')
        //                        window.location = url.d;

        //                }
        //            });
        //        }

        function Timer() {
            //            document.getElementById('divFinishQuiz').style.visibility = 'visible';
            $("#divFinishQuiz").show();
            //            var finishButton = document.getElementById('btnFinishTask');
            //            finishButton.style.display = "inline";

            //                document.getElementById('divFinishQuiz').style.visibility = 'block';
            //                document.getElementById('menu').style.visibility = 'hidden';
            //                document.getElementById('mid').setAttribute("style", "float: left;margin-top:-255px;");


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
                //                $("#divQuiz").html(result);

                var NewUserDialog = $("#dialogQuizResult").dialog({
                    appendTo: "#dialogAfterMeQuiz",
                    title: "Quiz Result",
                    height: 200,
                    width: 400,
                    //                maxHeight: dHeight,
                    //                maxWidth: dWidth,
                    position: "center",
                    resizable: false,
                    //                    buttons: {
                    //                        Close: function () {
                    //                            $(this).dialog('close');
                    //                        }
                    //                    },
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
                url: 'Comparison.aspx/GetQuizResult',
                data: "{}",
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: function (data) {

                    if (data.d != null) {

                        result = data.d;
                        //                        if (result.indexOf("Congradulations") >= 0) {

                        //                            $("#ContentPlaceHolder1_btnTryAgain").attr("display", "none");
                        //                            $("#ContentPlaceHolder1_btnViewAnswer").attr("display", "block");
                        //                        }

                        //                        else if (result.indexOf("Sorry") >= 0) {
                        //                            $("#ContentPlaceHolder1_btnTryAgain").attr("display", "block");
                        //                            $("#ContentPlaceHolder1_btnViewAnswer").attr("display", "none");
                        //                        }

                        $("#divQuiz").html(result);
                    }
                }
            });
        }


    </script>
    <!--scroller-->
    <link href="scroller/tutorsty.css" rel="stylesheet" type="text/css" />
    <link href="scroller/flexcrollstyles.css" rel="stylesheet" type="text/css" />
    <script type='text/javascript' src="scroller/flexcroll.js"></script>
    <style type="text/css">
        div.wrapper
        {
            position: relative;
            margin: 0 auto 30px auto;
            width: 500px;
            text-align: left;
            border: solid 1px #aaaaaa;
        }
        #users
        {
        }
        #users .user
        {
            border: solid 1px #bbbbbb;
            background-color: #dddddd;
            padding: 10px;
            margin: 5px;
        }
        #users .user .controls
        {
            float: right;
        }
        
        /*-------------impromptu---------- */
        .jqifade
        {
            position: absolute;
            background-color: #aaaaaa;
        }
        div.jqi
        {
            width: 65%; /*450px; height: 150px;*/
            font-family: Verdana, Geneva, Arial, Helvetica, sans-serif;
            position: absolute;
            background-color: #ffffff;
            font-size: 11px;
            text-align: left;
            border: solid 1px #eeeeee;
            -moz-border-radius: 10px;
            -webkit-border-radius: 10px;
            padding: 7px;
        }
        div.jqi .jqicontainer
        {
            font-weight: bold;
        }
        div.jqi .jqiclose
        {
            position: absolute;
            top: 4px;
            right: -2px;
            width: 18px;
            cursor: default;
            color: #bbbbbb;
            font-weight: bold;
        }
        div.jqi .jqimessage
        {
            padding: 10px;
            line-height: 20px;
            color: #444444;
        }
        div.jqi .jqibuttons
        {
            text-align: right;
            padding: 5px 0 5px 0;
            border: solid 1px #eeeeee;
            background-color: #f4f4f4; /*margin-top: 35px;*/
        }
        .mybuttons
        {
            padding: 3px 10px;
            margin: 0 10px;
            background-color: #2F6073;
            border: solid 1px #f4f4f4;
            color: #ffffff;
            font-weight: bold;
            font-size: 12px;
        }
        .mybuttons:hover
        {
            background-color: #728A8C;
        }
        div.jqi button
        {
            padding: 3px 10px;
            margin: 0 10px;
            background-color: #2F6073;
            border: solid 1px #f4f4f4;
            color: #ffffff;
            font-weight: bold;
            font-size: 12px;
        }
        div.jqi button:hover
        {
            background-color: #728A8C;
        }
        div.jqi button.jqidefaultbutton
        {
            background-color: #BF5E26;
        }
        .jqiwarning .jqi .jqibuttons
        {
            background-color: #BF5E26;
        }
        .popuptextarea
        {
            width: 98%;
            height: 80px;
            margin-bottom: -35px;
        }
        .popuptextbox
        {
            width: 98%;
        }
        
        .highlight
        {
            background-color: yellow;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <%--</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">--%>
<%--    <span style="background-color: white; color: blue; margin-left: 20px;" id="timeInSeconds">
    </span><span>seconds</span>--%>
    <div id="mainDiv">
        <img id="btn" src="img/1344591618_edit-notes.png" style="height: 30px; width: 30px;
            display: none; z-index: 5;" alt="Edit" onclick="getSelText()" />
        <asp:HiddenField runat="server" ID="timehdnmin" />
        <asp:HiddenField runat="server" ID="timehdnsec" />
         <asp:HiddenField runat="server" ID="hfComparisonTimeSpent" />
         <asp:HiddenField runat="server" ID="hfComparisonTaskType" />

         <asp:HiddenField runat="server" ID="hfBtnFinishClicked" />
        <%--   <asp:HiddenField runat="server" ID="timehdnmin" />
        <asp:HiddenField runat="server" ID="timehdnsec" />
        <asp:Label ID="lblTimeRemaining" runat="server" Style="margin-left: 540px; color: red;
            font-size: large;" Text="">Time Remaining : </asp:Label>
        <input id="txt1" readonly="true" type="text" value="" border="0" style="margin-left: -8px;
            color: red; font-size: large;" name="disp" />--%>
        <table width="98%" border="0" align="center" cellpadding="0" cellspacing="0" bgcolor="#FFFFFF">
            <%-- <tr>
                <td>
                    &nbsp;
                </td>
            </tr>--%>
            <%-- <tr>
                <td height="80" align="left" valign="top" style="padding-top: 10px;">
                    <img src="images/hd2.gif" width="364" height="35" />&nbsp;<asp:ImageButton ID="btnGenerate"
                        runat="server" OnClick="btnGenReport_Click" Text="Generate Report" ImageUrl="images/b2.jpg" />
                    <asp:Button ID="btnFinish" runat="server" Text="Finish" OnClick="btnFinish_Click" />
                </td>
            </tr>--%>
            <tr>
                <td>
                </td>
            </tr>
            <%--      <tr>
                <td height="30" align="center" valign="top">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td width="50%" align="center">
                                <img src="images/hd2a.gif" />
                            </td>
                            <td align="center">
                                <img src="images/hd2b.gif" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>--%>
            <tr>
                <td height="30" align="center" valign="top">
                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                        <tr>
                            <td colspan="2">
                                <PDFCmp:PDFComparison ID="CtrlPdfCmp" runat="server" OnErrorsComplete="PDFComparison_ErrorsComplete"
                                    OnFileEdit="PDFComparison_FileEdit" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <%--    <tr>
                <td height="80" align="left" valign="top" style="padding-top: 10px;">
                    <img src="images/hd2.gif" width="364" height="35" />&nbsp;
                    <asp:ImageButton ID="btnGenerate"
                        runat="server" OnClick="btnGenReport_Click" Text="Generate Report" ImageUrl="images/b2.jpg" />
                    <asp:Button ID="btnFinish" runat="server" Text="Finish" OnClick="btnFinish_Click" />
                </td>
            </tr>--%>
            <tr>
                <td height="300" align="center" valign="top" style="display: none">
                    <blockquote>
                        <table width="55%" border="0" cellspacing="0" cellpadding="0">
                            <tr>
                                <td class="bgm">
                                    <table width="100%" border="0" cellspacing="1" cellpadding="1">
                                        <tr>
                                            <td bgcolor="#FFFFFF" class="bgms">
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </blockquote>
                </td>
            </tr>
            <tr id="trNav" runat="server">
                <td>
                    <label id="lblComments" title="" runat="server">
                    </label>
                    <asp:Label ID="lblMsg" runat="server" Text="" ForeColor="Green" />
                </td>
            </tr>
        </table>
    </div>
    <div id="divDialogue" style="display: none; border: 4px solid #D6D6D6; position: absolute;
        left: 900px; top: 300px; border-top-right-radius: 20px; border-bottom-right-radius: 20px;
        padding: 6px; border-bottom-left-radius: 20px; background-color: #F0F0F0; z-index: 150;">
        <div id='cssmenu'>
            <asp:Button ID="btnAddError" runat="server" Text="Error" OnClick="btnAddError_Click" />
        </div>
    </div>
    <div id="divNavigationPanel" style="position: fixed; top: 0px; left: 34%; width: 35%;
        border-bottom-right-radius: 20px; padding: 6px; border-bottom-left-radius: 20px;
        color: White; background-color: #3A7676; height: 1px; overflow: hidden; padding-top: 10px;">
        <table style="width: 100%;margin-top:-14px;">
            <tr id="trTimer" runat="server">
                <td>
                    <asp:Label ID="lblTimeRemaining" runat="server" Style="margin-left: 120px; color: red;
                        font-size: large;" Text="">Time Remaining : </asp:Label>
                </td>
                <td>
                    <%-- <input id="txt1" readonly="true" type="text" style="margin-right: 100px; background-color: #3A7676;
                        border-color: #3A7676; color: red; font-size: large;" />--%>
                    <label id="txt1" style="margin-right: 150px; background-color: #3A7676; border-color: #3A7676;
                        color: red; font-size: large;">
                    </label>
                </td>
               
            </tr>
            <tr id="trComparisonTimeSpent" runat="server">
                <td>
                   <%-- <asp:Label ID="Label1" runat="server" Style="margin-left: 12px; color: white;
                        font-size: large;" Text="">Time : </asp:Label>
               
                     <label id="lblTimeSpent" style="margin-left:2px; margin-top:20px; background-color: #3A7676; border-color: #3A7676;
                        color: white; font-size: large;">--%>
                        
                        <span style="margin-left: 13px; color: white;">Time : </span>
                         <span style="margin-left:2px; margin-top:20px; background-color: #3A7676; border-color: #3A7676;
                        color: white;" id="lblTimeSpent"> </span>

                </td>
            </tr>
        </table>
        <div style="width: 100%; padding-top: 5px; padding-left: 15px;">
        
        
                   <%-- <asp:Label ID="Label2" runat="server" Style="margin-left: 120px; color: white;
                        font-size: large;" Text="">Time : </asp:Label>
               
                     <label id="Label3" style="margin-left: 260px; margin-top:20px; background-color: #3A7676; border-color: #3A7676;
                        color: white; font-size: large;">--%>

                

            <table style="width: 100%; margin-top:4% ">
                <tr id="trNavigateMistake" runat="server">
                    <td>
                        Navigate Mistakes
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
                    <td>
                        Navigate Pages
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
                    <td>
                        Keep
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
                    <td>
                    </td>
                    <td>
                        <asp:Button ID="btnGoTo" runat="server" Text="Generate Preview" OnClick="btnGoTo_Click" />
                        <asp:Button ID="btnFinish_Task" runat="server" Text="Finish Task" OnClick="btnFinish_Task_Click" OnClientClick="BtnFinishTask();"/>
                        <%--OnClick="btnFinishTask_Click" />--%>
                        <div id="divFinishQuiz" style="margin-left: 125px; margin-top: -26px;" runat="server"
                            visible="False">
                            <input id="btnFinishTask" type="button" value="Finish Test" onclick="ShowPopupQuizResult();" /></div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divNavigateSrcPdfName" style="position: fixed; top: 0px; left: 20.5%; width: 13%;
        border-bottom-right-radius: 20px; padding: 6px; border-bottom-left-radius: 20px;
        color: White; height: 1px; overflow: hidden; padding-top: 10px;">
        <div style="width: 100%; padding-top: 5px; padding-left: 15px;">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="50%" align="center">
                        <img src="img/hd2a.gif" />
                </tr>
            </table>
        </div>
    </div>
    <div id="divNavigatePrdPdfName" style="position: fixed; top: 0px; left: 67.5%; width: 20%;
        border-bottom-right-radius: 20px; padding: 6px; border-bottom-left-radius: 20px;
        color: White; height: 1px; overflow: hidden; padding-top: 10px;">
        <div style="width: 100%; padding-top: 5px; padding-left: 15px;">
            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                <tr>
                    <td width="50%" align="center">
                        <img src="img/hd2b.gif" />
                </tr>
            </table>
        </div>
    </div>
    <div id="dialogQuizResult" style="display: none; overflow: auto; width: 20px; height: 20px;
        background-color: #FFF1B9">
        <div id="divResultMessage" style="margin-right: 4px">
            <div style="margin-top: 20px; font-size: 12pt; color: Green; padding: 5px; font-family: Sans-Serif;"
                id="divQuiz">
            </div>
            <%--   <div style="margin-top:20px;font-size: 12pt; color: Green;  padding: 5px; font-family: Sans-Serif;" id="divFailed">
        </div>--%>
            <%-- <div style="margin-top:20px;">
                <label style="font-size: 12pt; color: Green;  padding: 5px; font-family: Sans-Serif; ">
                    Congradulations! You have cleared the test</label>
            </div>--%>
            <%--<div>
                <label>
                    You have failed the test</label>background-color: #ffcdd6;
            </div>--%>
        </div>
        <div style="margin-top: 30px;">
            <%-- <asp:Button ID="btnTryAgain" runat="server" CssClass="button" Text="Try Again" Style="width: 90px;
                border: 1px solid black; margin-left: 2px;" ValidationGroup="registrationForm"
                UseSubmitBehavior="false" Visible="true" />--%>
            <asp:Button ID="btnViewAnswer" runat="server" CssClass="button" Text="View Answer"
                Style="width: 100px; border: 1px solid black; margin-left: 2px;" ValidationGroup="registrationForm"
                UseSubmitBehavior="false" OnClick="btnViewAnswer_Click" Visible="true" />
            <asp:Button ID="btnCloseDialog" runat="server" CssClass="button" Text="Close" Style="width: 60px;
                border: 1px solid black; margin-left: 2px;" ValidationGroup="registrationForm"
                UseSubmitBehavior="false" OnClick="btnCloseDialog_Click" />
            <%-- <input id="btnCloseDialog" type="button" value="Close" OnClientClick="CloseResultDialog();""/>--%>
        </div>
    </div>
    <div id="dialogAfterMeQuiz">
    </div>
    <%--</asp:Content>--%>
    </form>
</body>
</html>

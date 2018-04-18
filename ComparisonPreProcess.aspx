<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master"
    AutoEventWireup="true" Inherits="Outsourcing_System.web_ComparisonPreProcess" Codebehind="ComparisonPreProcess.aspx.cs" %>

<%@ Register TagPrefix="eo" Namespace="EO.Web" Assembly="EO.Web" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="Server">
    <script src="Scripts/jquery-1.7.1.js" type="text/javascript"></script>
    <script src="Scripts/jquery-1.7.1.min.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        //function CheckPlugin() {

        //    var is_chrome = navigator.userAgent.toLowerCase().indexOf('chrome') > -1;

        //    function findPlugin(ext) {

        //        var thisExt, findExt, fileName;

        //        for (var n = 0; n < navigator.plugins.length; n++) {
        //            //alert("n value"+ navigator.plugins.length);
        //            for (var m = 0; m < navigator.plugins[n].length; m++) {
        //                //alert("m length:"+navigator.plugins[n].length);
        //                thisExt = navigator.plugins[n][m].description.toLowerCase();
        //                // alert("this exten"+thisExt);
        //                findExt = thisExt.substring(0, thisExt.indexOf(" "));

        //                fileName = navigator.plugins[n].name;

        //                //alert("findexes"+findExt);
        //                if (findExt == ext)
        //                    return (true);
        //            }
        //        }
        //        return (false);
        //    }

        //    if (is_chrome == true) {
        //        //alert("chrome browser");
        //        if (findPlugin("acrobat")) {
        //            ////alert("Adobe Acrobat pdf viewer");
        //            //return true;
        //        }
        //        else {

        //            //alert("please disable the chrome pdf viewer and enable the Adobe Acrobat PDF viewer \n Please follow the steps:\n 1.Open the new tab 'chrome://plugins/' type the Address. \n 2. Click the Enable link in 'Adobe Acrobat' field. \n 3. click the Disable link in 'Chrome PDF Viewer'. \n 4. Close the tab and you can open the PDf files in chrome browser.");
        //            //return false;

        //            alert("Chrome pdf viewer is disabled in your browser.\n Please follow the steps:\n 1.Open the new tab 'chrome://plugins/' type the Address. \n 2. Click the Enable link in 'Adobe Acrobat' field. \n 3. click the enable link in 'Chrome PDF Viewer'. \n 4. Close the tab and you can open the PDf files in chrome browser.");
        //            return false;
        //        }
        //    }
        //    else {
        //        //alert("not chrome");
        //        alert("Please use google Chrome for error detection.");
        //    }
        //}




        function preventBack() {
            window.history.forward();
        }
        setTimeout("preventBack()", 3);
        window.onunload = function () { null };

//        function disableBackButton() {
//            window.history.forward();
//        }
//        setTimeout("disableBackButton()", 0);


        $(document).ready(function () {
            //alert($("#FirstTime").text());
//            if ($("#FirstTime").text() == "abc") {

//                __doPostBack('btnProceed', 'OnClick');
//            }

            //alert("in ready");
            //__doPostBack('btnProceed', 'OnClick');
        });
        //        document.onload = onLoad();
        //        function onLoad() {
        //            alert('loadin');
        //            __doPostBack('btnProceed', 'OnClick');
        //        }
        function end_progressBar() {
            //The second argument of UpdateProgress is passed to
            //the client and can be retrieved by getExtraData



//            var hv = $("#hfBid").val();
//            alert(hv);

            //For older comparison viewer
//            window.open("Comparison.aspx", "_self");

            window.open("ErrorDetection.aspx", "_self");
        }

        function OnProgress(progressBar) {
            var extraData = progressBar.getExtraData();
            if (extraData) {
                //The following code demonstrates how to update
                //client side DHTML element based on the value
                //RunTask passed to us with e.UpdateProgress
                var div = document.getElementById("divStatus");
                div.innerHTML = extraData;
            }
        }


        function openPopup() {
            window.open('CODBooksProgress.aspx', 'BooksStatus', "height=200, width=200");

        }

        function disableButton(param) {
            param = document.getElementById(param);
            param.disabled = true;
            param.value = "Processing...";
        }

        function SaveDataViaAjax() {
            var a = 0;
            methodURL = "ComparisonPreProcess.aspx/test1";
            //"{'cateogryID':'" + a + "'}";
            parameters = "{'cateogryID':'" + a + "'}";

            $.ajax({
                type: "POST",
                contentType: "application/json; charset=utf-8",
                url: methodURL,
                data: parameters,
                dataType: "text",
                async: true,
                success: function (data) {
                    alert(data);
                },
                error: function (result) {
                    alert("Due to unexpected errors we were unable to load data");
                }
            });
        }


    </script>
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="Server">

<%--<div class="instruction" id="divTestInstr" runat="server" visible="false">
<b>Instructions For Test</b><br />
1. Time allowed for this test is 5 minutes.<br />
2. Please Click on the proceed button to start the test.<br />
</div>

    <div id="divPdfNotOpen" style="float:left;">
    <b>Instructions to Turn on google Chrome PDF viewer if not working</b><br />
1. Go to chrome://plugins.<br />
2. Under "Chrome PDF Viewer," click Enable PDF viewing.<br />
    Chrome will now open PDFs automatically when you click on them.
</div>--%>
    
    <%--<asp:HyperLink ID="HyperLink1" runat="server" onclick="return CheckPlugin();">Agency profile app</asp:HyperLink>--%>
    
    <table>
        <tr>
            <td><div class="instruction" id="divTestInstr" runat="server" visible="false">
<b>Instructions For Test</b><br />
1. Time allowed for this test is 5 minutes.<br />
2. Please Click on the proceed button to start the test.<br />
</div></td>
            <td>
                <div id="divPdfNotOpen">
    <b>Instructions to Turn on google Chrome PDF viewer if not working</b><br />
1. Go to chrome://plugins.<br />
2. Under "Chrome PDF Viewer" click Enable PDF viewing.<br />
    Chrome will now open PDFs in browser instead of downloading them.
</div>
            </td>
        </tr>
    </table>
    <br />
     <br />
    <table width="968" height="250" border="0" align="center" cellpadding="0" cellspacing="0"
        bgcolor="#FFFFFF">
        <tr id="trProceed" runat="server" visible="true">
            <td align="center">
                <asp:Button ID="btnProceed" runat="server" Text="Proceed" Visible="true" CssClass="button"/>
            </td>
        </tr>
        <tr>
            <td align="center" valign="middle">
                <eo:ProgressBar runat="server" ID="ProgressBar1" ShowPercentage="true" IndicatorImage="00060304"
                    BackgroundImageRight="00060103" ControlSkinID="None" BackgroundImage="00060101"
                    IndicatorIncrement="7" BackgroundImageLeft="00060102" Width="357px" StartTaskButton="btnProceed"
                    ClientSideOnTaskDone="end_progressBar" ClientSideOnValueChanged="OnProgress"
                    OnRunTask="ProgressBar1_RunTask">
                </eo:ProgressBar>
                <div id="divStatus" style="color: #4C4E6C; font-weight: bold; font-size: small">
                </div>
            </td>
        </tr>
    </table>
</asp:Content>


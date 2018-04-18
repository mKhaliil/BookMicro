<%@ Page Title="" Language="C#" MasterPageFile="~/UserMaster.Master" AutoEventWireup="true" CodeBehind="BookMicroUpgraded.aspx.cs" Inherits="Outsourcing_System.BookMicroUpgraded" %>

<%@ Register Src="UserControls/CommonControl/ucShowMessage.ascx" TagName="ucShowMessage" TagPrefix="uc1" %>


<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
    <style type="text/css">
        .visited {
            color: blue;
        }

        .visited1 {
            color: white;
        }

        #wrapper {
            margin-left: auto;
            margin-right: auto;
            width: 1330px;
        }

        .popupWrapper {
            margin-left: auto;
            margin-right: auto;
            width: 500px;
        }

        /*#container #mid h3 {font-family:"Open Sans";font-size:26px;color:#666666;text-align:center;}*/
    </style>
    <script type="text/javascript">

        //        function disableBackButton() {
        //            window.history.forward();
        //        }
        //        setTimeout("disableBackButton()", 0);

        function stopRKey(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            if ((evt.keyCode == 13)) { return false; }
        }

        document.onkeypress = stopRKey;

        $(function () {

            //alert('val of password = ' + $('#ctl00_mainBodyContents_tbxEmail').val());

            //$('#ctl00_mainBodyContents_tbxPassword').val('');

            if (!window.chrome) {
                //            alert('This site is best viewed in chrome');
                $("#divBrowserInfo").show();
            } else {
                $('#divBrowserInfo').hide();
            }
        });

    </script>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
    <div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">
        <uc1:ucShowMessage ID="ucShowMessage1" runat="server" />
    </div>
    <div id="divBrowserInfo">
        <p style="color: #fc8b01; font-size: 16pt; font-weight: bold;">
            Note: Best viewed in Google Chrome
        </p>
    </div>
    <div id="mid">
        <br />
        <br />
        <br />
        <div style="width: 100%;">
            <%--  <asp:Button ID="btnSimpletest" runat="server" Text="Simple Test" Style="width:10%; height:35px;background-color:#2a4f96; 
border-radius:25px; color:white;font-size: 120%;float:right;" OnClick="btnSimpletest_Click"/>--%>

           <%-- <asp:LinkButton ID="lbtnSimpletest" runat="server" OnClick="lbtnSimpletest_Click"
                Style="width: 7%; height: 25px; background-color: #2a4f96; border-radius: 18px; color: white; font-size: 140%; float: right; padding: 1%;">Sample Test</asp:LinkButton>--%>

        </div>
        <div id="videoPanel">
            <div id="videoBox">
                <div id="video">
                    <%--<iframe src="http://player.vimeo.com/video/94433382" frameborder="0" width="515"
                        height="300" scrolling="no" align="middle"></iframe>--%>
                    <iframe src="http://www.youtube.com/embed/RnC8ZytEg9Y" frameborder="0" width="515"
                        height="300" scrolling="no" align="middle" allowfullscreen="true"></iframe>
                </div>
            </div>
        </div>


        <!--Start Modal Popup-->
        <div id="popup" align="center">
            <p>
                <asp:Label ID="lblMessage" Visible="false" runat="server" Style="margin-left: 30px; font-size=xx-large; font-weight: bold; color: Red"></asp:Label>
            </p>
            <br />
            <%--<a class="modalLink" href="#modal1">
                <img id="imgbtnStartTest" runat="server" src="img/button_startTest-upgraded.jpg" alt="" border="0" /></a>--%>


            <%--<a href="#" runat="server" onServerClick="imgbtnTest_Click" >
                <img id="img1" runat="server" src="img/button_startTest-upgraded.jpg" alt="" border="0" /></a>--%>


            <div style="width: 100%; margin-top: 1%;">
                <%--<asp:Button ID="btnStartTest" runat="server" Text="START TEST" onClick="btnStartTest_Click" 
                Style="width: 36%;height: 105px;background-color:#2a4f96;border-radius:25px;color:white;font-size: 440%;" />--%>

                <%--<asp:Button ID="btnStartTest" runat="server" Text="START TEST"  
                Style="width: 36%;height: 105px;background-color:#2a4f96;border-radius:25px;color:white;font-size: 440%;" />--%>
                <asp:LinkButton ID="lbtnStartTest" runat="server" OnClick="lbtnStartTest_Click"
                    Style="width: 36%; height: 105px; background-color: #2a4f96; border-radius: 25px; color: white; font-size: 460%; padding: 1.3% 4.5%;">START TEST</asp:LinkButton>
                <br/>
                <br/><br/>
                <br/><br/>
                 <asp:LinkButton ID="lbtnSampletest" runat="server" OnClick="lbtnSampletest_Click"
                Style="clear:both; width: 7%; height: 25px; background-color: #2a4f96; border-radius: 18px; color: white; font-size: 140%; padding: 1%;">Sample Test</asp:LinkButton>
                
                <asp:LinkButton ID="lbtnDownloadPdf" runat="server" Style="margin-left : 4%;" OnClick="lbtnDownloadPdf_Click">Download Pdf for Video 1</asp:LinkButton>
            </div>

            <div class="overlay">
            </div>
            <%-- <div id="modal_Login" class="modal">
                <div class="popupWrapper">
                <asp:Panel ID="pnlLogInSubmitButton" runat="server" DefaultButton="imgbtnLogin">
                    <p class="closeBtn">
                        <img src="img/icon_close.png" align="right" /></p>

                      <h3>Enter below details to continue....</h3>
                    
                    <asp:ValidationSummary ID="vsLoginPopup" runat="server" ValidationGroup="Login"
                        ForeColor="red" DisplayMode="List" />
                    <label id="lblEmail" style="margin-left: 29px;">
                    Email:
                    </label>
                    <asp:TextBox ID="tbxEmail" runat="server" CssClass="txtLoginFormat"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ControlToValidate="tbxEmail" ErrorMessage="Email is required." ForeColor="red" Text="*" ToolTip="Required Field" ValidationGroup="Login"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="revEmail" ControlToValidate="tbxEmail"
                        runat="server" ValidationExpression="\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*"
                        ValidationGroup="Login" ForeColor="red" Text="*" ErrorMessage="Email is not vaild."></asp:RegularExpressionValidator>
                    <br />
                    <br />
                    <label id="lblPassword">
                    Password:
                    </label>
                    <asp:TextBox ID="tbxPassword" runat="server" CssClass="txtPasswordFormat" TextMode="Password" ValidationGroup="Login"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rvfPassword" runat="server" ControlToValidate="tbxPassword" ErrorMessage="Password is required." ForeColor="red" Text="*" ToolTip="Required Field" ValidationGroup="Login"></asp:RequiredFieldValidator>
                    <br />
                    <p>
                        <asp:LinkButton ID="lbtnForgotPassword" Style="margin-left:9%;" OnClick="lbtnForgotPassword_Click" runat="server">Forgot Password ?</asp:LinkButton>
                        <asp:CheckBox ID="cbxRememberMe" Style="margin-left:32%;" runat="server" />Stay signed in
                        
                    </p>
                    <br />
                    <asp:ImageButton ID="imgbtnLogin" runat="server" OnClick="imgbtnLogin_Click" src="img/btn_submit.png" ValidationGroup="Login" />
                </asp:Panel>
                    </div>
            </div>
            <div id="modal1" class="modal">
                <div class="popupWrapper">
                    
                <asp:Panel ID="pnlTestSubmitButton" runat="server" DefaultButton="imgbtnTest">
                    <p class="closeBtn">
                        <img src="img/icon_close.png" align="right" /></p>
                    <h3>
                        Enter below details to continue....</h3>
                    <asp:ValidationSummary ID="vsStartTestPopup" runat="server" ValidationGroup="StartTest"
                        ForeColor="red" DisplayMode="List" />
                    <label id="lblName">
                    Name:
                    </label>
                    <asp:TextBox ID="txtName" runat="server" CssClass="txtLoginFormat"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rfvName" runat="server" ControlToValidate="txtName" ErrorMessage="Name is required." 
                        ForeColor="red" Text="*" ToolTip="Required Field" ValidationGroup="StartTest"></asp:RequiredFieldValidator>
                    <br />
                    <br />
                    <label id="lblStartTestEmail" style="margin-left: 10px;">
                    Email:
                    </label>
                    <asp:TextBox ID="txtEmail" runat="server" CssClass="txtPasswordFormat" ValidationGroup="LoginForm"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="rvfEmail" runat="server" ControlToValidate="txtEmail" ErrorMessage="Email is required." 
                        ForeColor="red" Text="*" ToolTip="Required Field" ValidationGroup="StartTest"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtEmail" 
                        ErrorMessage="Email is not vaild." ForeColor="red" Text="*" ValidationExpression="\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*" 
                        ValidationGroup="StartTest"></asp:RegularExpressionValidator>
                    <br />
                    <br />
                    <asp:ImageButton ID="imgbtnTest" runat="server" OnClick="imgbtnTest_Click" src="img/btn_submit.png" ValidationGroup="StartTest" />
                </asp:Panel>
                </div>

            </div>--%>
        </div>
        <!--End Modal Popup-->

        <%-- <asp:Label ID="lblTestIp" runat="server" Text="Label"></asp:Label>
        <br />--%>

        <%--<h2 style="font-family: 'Open Sans'; font-size: 38px; color: #2a4f96; text-align: center;">Here are some videos to help you get started ...</h2>--%>

        <h2>Here are some videos to help you get started ...</h2>

        <div style="width: 340px; font-size: 16px; margin-bottom: 20px; margin-left: auto; margin-right: auto; display: none;">
            <asp:Label ID="Label2" runat="server" Style="color: #2a4f96;" Text="Select Training Video's Language:"></asp:Label>&nbsp;&nbsp;
            <asp:LinkButton ID="lbtnEnglish" OnClick="lblEnglish_Click" runat="server" Enabled="false">English</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="lbtnUrdu" OnClick="lbtnUrdu_Click" runat="server" Visible="False" Enabled="false">Urdu</asp:LinkButton>
        </div>

        <div id="wrapper">
            <div id="videos">
                <asp:MultiView ID="mvTrainingVideos" runat="server">
                    <asp:View ID="vUrduVideos" runat="server">
                        <div id="listV" style="margin: 0 auto;">
                            <div id="listBox">
                                <a class="modalLink" href="#modal0">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="https://i.vimeocdn.com/video/519143128_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal0" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://player.vimeo.com/video/128127080" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                                </div>
                                <div id="title">
                                    Introduction
                                </div>
                                <div id="txt2">
                                </div>
                            </div>
                            <div id="spr">
                            </div>
                            <div id="listBox">
                                <a class="modalLink" href="#modal2">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="https://i.vimeocdn.com/video/519279337_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal2" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://player.vimeo.com/video/128226149" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                                </div>
                                <div id="title">
                                    Space issue
                                </div>
                                <div id="txt2">
                                    How to find Space errors
                                </div>
                            </div>
                            <div id="spr">
                            </div>
                            <div id="listBox">
                                <a class="modalLink" href="#modal3">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="https://i.vimeocdn.com/video/519143151_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal3" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://player.vimeo.com/video/128127141" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                                </div>
                                <div id="title">
                                    Different Text Colors
                                </div>
                                <div id="txt2">
                                    How to find different text colors
                                </div>
                            </div>
                            <div id="spr">
                            </div>
                            <div id="listBox">
                                <a class="modalLink" href="#modal4">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="http://i.vimeocdn.com/video/519454026_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal4" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="Videos/Split%20and%20Merge%20Issue%20Updated.mp4" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                                </div>
                                <div id="title">
                                    Split and Merge
                                </div>
                                <div id="txt2">
                                    How to find Split and Merge errors
                                </div>
                            </div>
                            <div id="spr">
                            </div>
                            <div id="listBox">
                                <a class="modalLink" href="#modal13">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="http://i.vimeocdn.com/video/520137263_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal13" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://player.vimeo.com/video/128859502" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                                </div>
                                <div id="title">
                                    Test Demo
                                </div>
                                <div id="txt2">
                                    How to attempt test
                                </div>
                            </div>
                        </div>
                    </asp:View>

                    <asp:View ID="vEnglishVideos" runat="server">
                        <div id="listV_English" style="margin: 0 auto;height:780px !important;">
                            <div id="listBox">
                                <a class="modalLink" href="#modal5">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="http://i.vimeocdn.com/video/516773311_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal5" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://www.youtube.com/embed/QiLTzgHdB5w" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen="true" mozallowfullscreen="true" allowfullscreen="true"></iframe>
                                </div>
                                <div id="title">
                                    Colors Introduction
                                </div>
                                 
                                <div id="txt2">
                                </div>
                            </div>
                            <div id="spr">
                            </div>
                            <div id="listBox">
                                <a class="modalLink" href="#modal6">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="http://i.vimeocdn.com/video/517672361_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal6" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://www.youtube.com/embed/lMAYCx60SP4" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen="true" mozallowfullscreen="true" allowfullscreen="true"></iframe>
                                </div>
                                <div id="title">
                                    Chapter Introduction
                                </div>
                                <div id="txt2">
                                    How to find Chapter by color
                                </div>
                            </div>
                            <div id="spr">
                            </div>
                            <div id="listBox">
                                <a class="modalLink" href="#modal7">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="http://i.vimeocdn.com/video/517672354_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal7" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://www.youtube.com/embed/beevqYaX_jQ" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen="true" mozallowfullscreen="true" allowfullscreen="true"></iframe>
                                </div>
                                <div id="title">
                                    Heading 1
                                </div>
                                <div id="txt2">
                                    How to find Heading 1 by color
                                </div>
                            </div>
                            <div id="spr">
                            </div>
                            <div id="listBox">
                                <a class="modalLink" href="#modal8">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="http://i.vimeocdn.com/video/517784177_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal8" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://www.youtube.com/embed/nKHu9yWoHIo" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen="true" mozallowfullscreen="true" allowfullscreen="true"></iframe>
                                </div>
                                <div id="title">
                                    Heading 2
                                </div>
                                <div id="txt2">
                                    How to find Heading 2 by color
                                </div>
                            </div>
                            <div id="spr">
                            </div>
                            <div id="listBox">
                                <a class="modalLink" href="#modal9">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="https://i.vimeocdn.com/video/517792671_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal9" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://www.youtube.com/embed/-4Tasc-dx-U" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen="true" mozallowfullscreen="true" allowfullscreen="true"></iframe>
                                </div>
                                <div id="title">
                                    Heading 3
                                </div>
                                <div id="txt2">
                                    How to find Heading 3 by color
                                </div>
                            </div>
                            
                            
                            <%--Second row of videos--%>
                            
                             <div id="listBox">
                                <a class="modalLink" href="#modal10">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="https://i.vimeocdn.com/video/517792671_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal10" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://www.youtube.com/embed/OFdq-CWZVYo" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen="true" mozallowfullscreen="true" allowfullscreen="true"></iframe>
                                </div>
                                <div id="title">
                                    Heading 4
                                </div>
                               <div id="txt2">
                                    How to find Heading 4 by color
                                </div>
                            </div>
                            
                            
                             <div id="spr">
                            </div>
                            <div id="listBox">
                                <a class="modalLink" href="#modal11">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="https://i.vimeocdn.com/video/517792671_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal11" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://www.youtube.com/embed/YGIptUKRapU" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen="true" mozallowfullscreen="true" allowfullscreen="true"></iframe>
                                </div>
                                <div id="title">
                                    Highlited Text
                                </div>
                                <div id="txt2">
                                    How to find Heading Text by color
                                </div>
                            </div>
                            
                            
                             <div id="spr">
                            </div>
                            <div id="listBox">
                                <a class="modalLink" href="#modal12">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="https://i.vimeocdn.com/video/517792671_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal12" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://www.youtube.com/embed/wJh2R_B6TZ8" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen="true" mozallowfullscreen="true" allowfullscreen="true"></iframe>
                                </div>
                                <div id="title">
                                    Spara
                                </div>
                                <div id="txt2">
                                    How to find Spara by color
                                </div>
                            </div>
                            
                            
                             <div id="spr">
                            </div>
                            <div id="listBox">
                                <a class="modalLink" href="#modal13">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="https://i.vimeocdn.com/video/517792671_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal13" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://www.youtube.com/embed/QfYd1OcbP5I" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen="true" mozallowfullscreen="true" allowfullscreen="true"></iframe>
                                </div>
                                <div id="title">
                                    Table
                                </div>
                                <div id="txt2">
                                    How to find Table by color
                                </div>
                            </div>
                            
                             <div id="spr">
                            </div>
                            <div id="listBox">
                                <a class="modalLink" href="#modal14">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="https://i.vimeocdn.com/video/517792671_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal14" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://www.youtube.com/embed/jrw-5zg9NTU" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen="true" mozallowfullscreen="true" allowfullscreen="true"></iframe>
                                </div>
                                <div id="title">
                                    Bulleted list
                                </div>
                               <div id="txt2">
                                    How to find Bulleted list by color
                                </div>
                            </div>
                            
                            
                            <div id="listBox">
                                <a class="modalLink" href="#modal15">
                                    <div style="position: relative">
                                        <div id="im">
                                            <img src="https://i.vimeocdn.com/video/517792671_1280.jpg" width="200" height="200" />
                                        </div>
                                        <div id="imPlay">
                                            <img src="img/play.png">
                                        </div>
                                    </div>
                                </a>
                                <div class="overlay">
                                </div>
                                <div id="modal15" class="modal">
                                    <p class="closeBtn">
                                        <img src="img/icon_close.png" align="right" />
                                    </p>
                                    <iframe src="http://www.youtube.com/embed/089qAguagkI" width="500" height="375" frameborder="0"
                                        webkitallowfullscreen="true" mozallowfullscreen="true" allowfullscreen="true"></iframe>
                                </div>
                                <div id="title">
                                   Footnotes
                                </div>
                                <div id="txt2">
                                    How to find footnotes by color
                                </div>
                            </div>
                        </div>
                    </asp:View>
                </asp:MultiView>
            </div>

        </div>
    </div>
</asp:Content>

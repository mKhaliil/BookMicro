<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true"
    CodeBehind="BookMicro.aspx.cs" Inherits="Outsourcing_System.OnlineTest_New" %>

<%@ Register src="UserControls/CommonControl/ucShowMessage.ascx" tagname="ucShowMessage" tagprefix="uc1" %>


<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
    <style type="text/css">
        .visited
        {
            color: blue;
        }
        .visited1
        {
            color: white;
        }
        #wrapper {
    margin-left:auto;
    margin-right:auto;
    width:1330px;
}
        .popupWrapper {
    margin-left:auto;
    margin-right:auto;
    width:500px;
}

        /*#container #mid h3 {font-family:"Open Sans";font-size:26px;color:#666666;text-align:center;}*/

    </style>
    <script type="text/javascript">

        function GetQueryStringParams(sParam) {
            var sPageURL = window.location.search.substring(1);
            var sURLVariables = sPageURL.split('&');
            for (var i = 0; i < sURLVariables.length; i++) {
                var sParameterName = sURLVariables[i].split('=');
                if (sParameterName[0] == sParam) {
                    return sParameterName[1];
                }
            }
        }

        var tech = GetQueryStringParams('testType');

        if (tech != null) {
            
        }


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
            Note: Best viewed in Google Chrome</p>
    </div>
    <div id="mid">
        <br />
        <br />
        <br />
        <div id="videoPanel">
            <div id="videoBox">
                <div id="video">
                    <%--<iframe src="http://player.vimeo.com/video/94433382" frameborder="0" width="515"
                        height="300" scrolling="no" align="middle"></iframe>--%>
                    <iframe src="http://www.youtube.com/embed/6FlPxnTZ20s" frameborder="0" width="515"
                        height="300" scrolling="no" align="middle" allowFullScreen="true"></iframe>
                </div>
            </div>
        </div>
        <!--Start Modal Popup-->
        <div id="popup" align="center">
            <p>
                <asp:Label ID="lblMessage" Visible="false" runat="server" Style="margin-left: 30px;
                    font-size=xx-large; font-weight: bold; color: Red"></asp:Label></p>
            <br />
            <a class="modalLink" href="#modal1" >
                <img id="imgbtnStartTest" runat="server" src="img/button_startTest.jpg" alt="" border="0" /></a>

           <%-- <a href="#" runat="server" onServerClick="imgbtnTest_Click" >
                <img id="imgbtnStartTest" runat="server" src="img/button_startTest.jpg" alt="" border="0" /></a>--%>


            <%--<asp:ImageButton ID="imgbtnStartTest" ImageUrl="img/button_startTest.jpg" AlternateText=""
                runat="server" OnClick="imgbtnStartTest_Click" />--%>
            <div class="overlay">
            </div>
            <div id="modal_Login" class="modal">
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

                    <%--<asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Login"
                        ForeColor="red" DisplayMode="List" />
                    <asp:Label ID="lblEmail" Style="margin-top: 20px" runat="server">Email:</asp:Label>
                    <asp:TextBox ID="tbxEmail" CssClass="txtLoginFormat" Style="margin-top: 20px; margin-left: 28px;
                        margin-bottom: -35px" runat="server"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="red" runat="server"
                        ErrorMessage="Email is required." ControlToValidate="tbxEmail" Text="*" ToolTip="Required Field"
                        ValidationGroup="Login"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tbxEmail"
                        runat="server" ValidationExpression="\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*"
                        ValidationGroup="Login" ForeColor="red" Text="*" ErrorMessage="Email is not vaild."></asp:RegularExpressionValidator>
                    <br />
                    <br />
                    <asp:Label ID="lblPassword" Style="margin-left: -2%; margin-right: 0.8%" runat="server">Password:</asp:Label>
                    <asp:TextBox ID="tbxPassword" runat="server" CssClass="txtPasswordFormat" TextMode="Password"
                        ValidationGroup="register"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Password is required."
                        ControlToValidate="tbxPassword" ForeColor="red" Text="*" ToolTip="Required Field"
                        ValidationGroup="Login"></asp:RequiredFieldValidator>
                    <br />
                    <br />
                    <p>
                        <asp:CheckBox ID="cbxRememberMe" Style="margin-left: 72px" runat="server" />
                        Keep me signed in
                    </p>
                    <br />
                    <br />
                    <asp:ImageButton src="img/btn_submit.png" Style="margin-left: auto; margin-right: auto;
                        margin-bottom: 4px" ID="imgbtnLogin" OnClick="imgbtnLogin_Click" runat="server"
                        ValidationGroup="Login" />--%>
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

            </div>
        </div>
        <!--End Modal Popup-->

       <%-- <asp:Label ID="lblTestIp" runat="server" Text="Label"></asp:Label>
        <br />--%>

        <h2>            
            Here are some videos to help you get started ...</h2>
        <div style="width: 340px; font-size: 16px; margin-bottom: 20px; margin-left: auto; display:none;
            margin-right: auto;">
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
                                        <img src="https://i.vimeocdn.com/video/519143128_1280.jpg" width="200" height="200" /></div>
                                    <div id="imPlay">
                                        <img src="img/play.png"></div>
                                </div>
                            </a>
                            <div class="overlay">
                            </div>
                            <div id="modal0" class="modal">
                                <p class="closeBtn">
                                    <img src="img/icon_close.png" align="right" /></p>
                                <iframe src="http://player.vimeo.com/video/128127080" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                            </div>
                            <div id="title">
                                Introduction</div>
                            <div id="txt2">
                            </div>
                        </div>
                        <div id="spr">
                        </div>
                        <div id="listBox">
                            <a class="modalLink" href="#modal2">
                                <div style="position: relative">
                                    <div id="im">
                                        <img src="https://i.vimeocdn.com/video/519279337_1280.jpg" width="200" height="200" /></div>
                                    <div id="imPlay">
                                        <img src="img/play.png"></div>
                                </div>
                            </a>
                            <div class="overlay">
                            </div>
                            <div id="modal2" class="modal">
                                <p class="closeBtn">
                                    <img src="img/icon_close.png" align="right" /></p>
                                <iframe src="http://player.vimeo.com/video/128226149" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                            </div>
                            <div id="title">
                                Space issue
                            </div>
                            <div id="txt2">
                                How to find Space errors</div>
                        </div>
                        <div id="spr">
                        </div>
                        <div id="listBox">
                            <a class="modalLink" href="#modal3">
                                <div style="position: relative">
                                    <div id="im">
                                        <img src="https://i.vimeocdn.com/video/519143151_1280.jpg" width="200" height="200" /></div>
                                    <div id="imPlay">
                                        <img src="img/play.png"></div>
                                </div>
                            </a>
                            <div class="overlay">
                            </div>
                            <div id="modal3" class="modal">
                                <p class="closeBtn">
                                    <img src="img/icon_close.png" align="right" /></p>
                                <iframe src="http://player.vimeo.com/video/128127141" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                            </div>
                            <div id="title">
                                Different Text Colors</div>
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
                                        <img src="http://i.vimeocdn.com/video/519454026_1280.jpg" width="200" height="200" /></div>
                                    <div id="imPlay">
                                        <img src="img/play.png"></div>
                                </div>
                            </a>
                            <div class="overlay">
                            </div>
                            <div id="modal4" class="modal">
                                <p class="closeBtn">
                                    <img src="img/icon_close.png" align="right" /></p>
                               <%-- <iframe src="http://player.vimeo.com/video/128354145" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>--%>
                                
                                    <iframe src="Videos/Split%20and%20Merge%20Issue%20Updated.mp4" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                            </div>
                            <div id="title">
                                Split and Merge</div>
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
                                        <img src="http://i.vimeocdn.com/video/520137263_1280.jpg" width="200" height="200" /></div>
                                    <div id="imPlay">
                                        <img src="img/play.png"></div>
                                </div>
                            </a>
                            <div class="overlay">
                            </div>
                            <div id="modal13" class="modal">
                                <p class="closeBtn">
                                    <img src="img/icon_close.png" align="right" /></p>
                                <iframe src="http://player.vimeo.com/video/128859502" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                            </div>
                            <div id="title">
                                Test Demo</div>
                            <div id="txt2">
                                How to attempt test
                            </div>
                        </div>
                    </div>
                </asp:View>

                <asp:View ID="vEnglishVideos" runat="server">
                    <div id="listV_English" style="margin: 0 auto;">
                        <div id="listBox">
                            <a class="modalLink" href="#modal5">
                                <div style="position: relative">
                                    <div id="im">
                                        <img src="http://i.vimeocdn.com/video/516773311_1280.jpg" width="200" height="200" /></div>
                                    <div id="imPlay">
                                        <img src="img/play.png"></div>
                                </div>
                            </a>
                            <div class="overlay">
                            </div>
                            <div id="modal5" class="modal">
                                <p class="closeBtn">
                                    <img src="img/icon_close.png" align="right" /></p>
                                <%--<iframe src="http://player.vimeo.com/video/126359714" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>--%>
                                
                                <iframe src="http://www.youtube.com/embed/6FlPxnTZ20s" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen="true" mozallowfullscreen="true" allowFullScreen="true"></iframe>
                            </div>
                            <div id="title">
                                Introduction</div>
                            <div id="txt2">
                            </div>
                        </div>
                        <div id="spr">
                        </div>
                        <div id="listBox">
                            <a class="modalLink" href="#modal6">
                                <div style="position: relative">
                                    <div id="im">
                                        <img src="http://i.vimeocdn.com/video/517672361_1280.jpg" width="200" height="200" /></div>
                                    <div id="imPlay">
                                        <img src="img/play.png"></div>
                                </div>
                            </a>
                            <div class="overlay">
                            </div>
                            <div id="modal6" class="modal">
                                <p class="closeBtn">
                                    <img src="img/icon_close.png" align="right" /></p>
                             <%--   <iframe src="http://player.vimeo.com/video/127030215" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>--%>
                                 <iframe src="http://www.youtube.com/embed/O-ahremerwU" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen="true" mozallowfullscreen="true" allowFullScreen="true"></iframe>
                            </div>
                            <div id="title">
                                Split and Merge
                            </div>
                            <div id="txt2">
                                How to find splitting and merging errors</div>
                        </div>
                         <div id="spr">
                        </div>
                        <div id="listBox">
                            <a class="modalLink" href="#modal7">
                                <div style="position: relative">
                                    <div id="im">
                                        <img src="http://i.vimeocdn.com/video/517672354_1280.jpg" width="200" height="200" /></div>
                                    <div id="imPlay">
                                        <img src="img/play.png"></div>
                                </div>
                            </a>
                            <div class="overlay">
                            </div>
                            <div id="modal7" class="modal">
                                <p class="closeBtn">
                                    <img src="img/icon_close.png" align="right" /></p>
                         <%--       <iframe src="http://player.vimeo.com/video/127030176" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>--%>
                                
                                <iframe src="http://www.youtube.com/embed/X7OXs61JdA8" width="500" height="375" frameborder="0"
                                   webkitallowfullscreen="true" mozallowfullscreen="true" allowFullScreen="true"></iframe>
                            </div>
                            <div id="title">
                                Space issue</div>
                            <div id="txt2">
                                How to find space issues
                            </div>
                        </div>
                        <div id="spr">
                        </div>
                        <div id="listBox">
                            <a class="modalLink" href="#modal8">
                                <div style="position: relative">
                                    <div id="im">
                                        <img src="http://i.vimeocdn.com/video/517784177_1280.jpg" width="200" height="200" /></div>
                                    <div id="imPlay">
                                        <img src="img/play.png"></div>
                                </div>
                            </a>
                            <div class="overlay">
                            </div>
                            <div id="modal8" class="modal">
                                <p class="closeBtn">
                                    <img src="img/icon_close.png" align="right" /></p>
                     <%--           <iframe src="http://player.vimeo.com/video/127112225" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>--%>
                                
                                <iframe src="http://www.youtube.com/embed/60orL2mE5js" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen="true" mozallowfullscreen="true" allowFullScreen="true"></iframe>
                            </div>
                            <div id="title">
                                Different Text Colors</div>
                            <div id="txt2">
                                How to find different text colors
                            </div>
                        </div>
                        <div id="spr">
                        </div>
                        <div id="listBox">
                            <a class="modalLink" href="#modal9">
                                <div style="position: relative">
                                    <div id="im">
                                        <img src="https://i.vimeocdn.com/video/517792671_1280.jpg" width="200" height="200" /></div>
                                    <div id="imPlay">
                                        <img src="img/play.png"></div>
                                </div>
                            </a>
                            <div class="overlay">
                            </div>
                            <div id="modal9" class="modal">
                                <p class="closeBtn">
                                    <img src="img/icon_close.png" align="right" /></p>
                     <%--           <iframe src="http://player.vimeo.com/video/127118994" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>--%>
                                
                                <iframe src="http://www.youtube.com/embed/Hl7EUi-hl78" width="500" height="375" frameborder="0"
                                    webkitallowfullscreen="true" mozallowfullscreen="true" allowFullScreen="true"></iframe>
                            </div>
                            <div id="title">
                                Test Demo</div>
                            <div id="txt2">
                                How to attempt test
                            </div>
                        </div>
                    </div>
                </asp:View>
            </asp:MultiView>
            <%-- <div id="listV" style="margin: 0 auto;">
                <div id="listBox">
                    <a class="modalLink" href="#modal2">
                        <div style="position: relative">
                            <div id="im">
                                <img src="https://i.vimeocdn.com/video/515975638_1280.jpg" width="200" height="200" /></div>
                            <div id="imPlay">
                                <img src="img/play.png"></div>
                        </div>
                    </a>
                    <div class="overlay">
                    </div>
                    <div id="modal2" class="modal">
                        <p class="closeBtn">
                            <img src="img/icon_close.png" align="right" /></p>
                        <iframe src="http://player.vimeo.com/video/125762210" width="500" height="375" frameborder="0"
                            webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                    </div>
                    <div id="title">
                        Introduction</div>
                    <div id="txt2">
                    </div>
                </div>
                <div id="spr">
                </div>
                <div id="listBox">
                    <a class="modalLink" href="#modal3">
                        <div style="position: relative">
                            <div id="im">
                                <img src="https://i.vimeocdn.com/video/515975637_1280.jpg" width="200" height="200" /></div>
                            <div id="imPlay">
                                <img src="img/play.png"></div>
                        </div>
                    </a>
                    <div class="overlay">
                    </div>
                    <div id="modal3" class="modal">
                        <p class="closeBtn">
                            <img src="img/icon_close.png" align="right" /></p>
                        <iframe src="http://player.vimeo.com/video/125762207" width="500" height="375" frameborder="0"
                            webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                    </div>
                    <div id="title">
                        Splitting 1</div>
                    <div id="txt2">
                        How to find splitting errors</div>
                </div>
                <div id="spr">
                </div>
                <div id="listBox">
                    <a class="modalLink" href="#modal4">
                        <div style="position: relative">
                            <div id="im">
                                <img src="https://i.vimeocdn.com/video/515995935_1280.jpg" width="200" height="200" /></div>
                            <div id="imPlay">
                                <img src="img/play.png"></div>
                        </div>
                    </a>
                    <div class="overlay">
                    </div>
                    <div id="modal4" class="modal">
                        <p class="closeBtn">
                            <img src="img/icon_close.png" align="right" /></p>
                        <iframe src="http://player.vimeo.com/video/125762208" width="500" height="375" frameborder="0"
                            webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                    </div>
                    <div id="title">
                        Splitting 2</div>
                    <div id="txt2">
                        Which buttons or functions you need to know</div>
                </div>
                <div id="spr">
                </div>
                <div id="listBox">
                    <a class="modalLink" href="#modal5">
                        <div style="position: relative">
                            <div id="im">
                                <img src="https://i.vimeocdn.com/video/515975644_1280.jpg" width="200" height="200" /></div>
                            <div id="imPlay">
                                <img src="img/play.png"></div>
                        </div>
                    </a>
                    <div class="overlay">
                    </div>
                    <div id="modal5" class="modal">
                        <p class="closeBtn">
                            <img src="img/icon_close.png" align="right" /></p>
                        <iframe src="http://player.vimeo.com/video/125762209" width="500" height="375" frameborder="0"
                            webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                    </div>
                    <div id="title">
                        Splitting Complete</div>
                    <div id="txt2">
                        &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp; &nbsp; Complete splitting video  &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                        &nbsp;&nbsp;&nbsp; &nbsp;&nbsp;&nbsp;
                    </div>
                </div>
            </div>--%>
        </div>
        
        </div>
    </div>
    <div id="divRegionInfo" runat="server" visible="false">
        <div style="display: block; left: 0; top: 0; position: fixed; width: 100%; height: 100%;
            padding: 200px; opacity: 0.7; background-color: Gray;">
        </div>
        <div style="position: fixed; left: 35%; top: 30%; border: solid 6px red; border-radius: 15px;
            background-color: White; padding: 20px; height: 20%;">
            <table cellpadding="3px" style="text-align: left;">
                <tr>
                    <td>
                        <strong>Please check your region information</strong>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                    </td>
                </tr>
                <tr>
                    <td>
                        Select Your region:&nbsp&nbsp
                        <asp:DropDownList ID="ddlRegion" runat="server" Width="180" Height="30">
                            <asp:ListItem Text="Australia" Value="Australia"></asp:ListItem>
                            <asp:ListItem Text="Bangladesh" Value="Bangladesh"></asp:ListItem>
                            <asp:ListItem Text="India" Value="India"></asp:ListItem>
                            <asp:ListItem Text="Pakistan" Value="Pakistan"></asp:ListItem>
                            <asp:ListItem Text="Phillipines" Value="Phillipines"></asp:ListItem>
                            <asp:ListItem Text="UK" Value="UK"></asp:ListItem>
                            <asp:ListItem Text="US" Value="US"></asp:ListItem>
                            <asp:ListItem Text="Other" Value="Other"></asp:ListItem>
                        </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td align="center">
                        <br />
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="button" OnClick="btnSubmit_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</asp:Content>

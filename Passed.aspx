<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true"
    CodeBehind="Passed.aspx.cs" Inherits="Outsourcing_System.Passed" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
    <style type="text/css">
        .userDetails {
            border: 1px solid #cccccc;
            border-radius: 25px;
            padding: 10px 10px 10px 10px;
            text-align: left;
            width: 95%;
            margin: auto;
            margin-bottom: 60px;
            background-color: #B3FFB3;
        }
    </style>
     <script type="text/javascript" src="http://code.jquery.com/jquery-1.10.0.min.js"></script>
       
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">

    <%--<div id="mid" align="center">
        <h2>
            Congratulations! You have passed the Online Test. </br></br></br>An email has been sent to you by hr of the company.</h2>
        <asp:Label ID="lbl" runat="server"></asp:Label>
    </div>--%>
    <!--Start Failed Message-->
    <%--<div style="text-align: center; margin-bottom: 4px;">
    <asp:Label ID="lblErrorMsg" style="font-family: Verdana;font-size: large;font-weight: normal;color: #FF0000;" runat="server" visible="false"></asp:Label>
            </div>--%>
    <div id="DivError" runat="server" visible="false">
        <div style="font-size: 10pt; color: Green; text-align: left; background-color: #ffcdd6; border: 1px solid red; padding: 5px; font-family: Sans-Serif; width: 93%; margin: auto;">
            <table>
                <tr>
                    <td rowspan="2">
                        <img src="img/red-error.gif" />
                    </td>
                    <td>
                        <div id="divText" runat="server"></div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer" style="padding-top: 2px">
        </div>
    </div>

    <div class="userDetails">

        <%-- <h1>--%>
        <h4 id="h4TestStatus" runat="server">
            <strong>Congratulations!</strong> You qualified the test. You are assigned a title
                of &quot;Trainee Editor&quot; at BookMicro.</h4>
        
         <h4 id="h4UpgradedTestStatus" runat="server">
            <strong>Congratulations!</strong> You qualified the test and your level is upgraded.</h4>
        
         <h4 id="h4UpgradedSampleTestStatus" runat="server">
            <strong>Congratulations!</strong> You have passed sample test, now you are ready to attempt actual test. Best of Luck!
         </h4>
        
        <div style="text-align: center" id="divProfileLink" runat="server">
            <h4>Please
                    <asp:LinkButton ID="lbtnCompleteProfile" runat="server" OnClick="lbtnCompleteProfile_Click">click here </asp:LinkButton>
                to complete your profile.
            </h4>
        </div>

         <div style="text-align: center" id="divGoToUserPage" runat="server">
            <h4>Please
                    <asp:LinkButton ID="lbtnGoToProfile" runat="server" OnClick="lbtnGoToProfile_Click">click here </asp:LinkButton>
                to go to profile page.
            </h4>
        </div>
        <script src="FinalScripts/easypiechart.min.js" type="text/javascript"></script>
       
        <div id="divCanvas" runat="server" style="text-align: center; width: 100%;">
            <asp:Label ID="lbl" runat="server" class="chart"><span class="percent"></span></asp:Label>
        </div>
        <script type="text/javascript">
            document.addEventListener('DOMContentLoaded', function () {
                var chart = window.chart = new EasyPieChart(document.querySelector('span'), {
                    easing: 'easeOutElastic',
                    delay: 3000,
                    onStep: function (from, to, percent) {
                        this.el.children[0].innerHTML = Math.round(percent);
                    }
                });

                document.querySelector('.js_update').addEventListener('click', function (e) {
                    chart.update(Math.random() * 200 - 100);
                });
            });
        </script>
        <!--End Start success Message-->
        <div id="tests" style="display: none;">
            <h1>
                <strong>Welcome to BookMicro's Training Session!<br />
                    Qualify below tests to get work. </strong>
            </h1>
            <div id="roundBox">
                <h4>Table Tasks
                </h4>
                <p style="float: right;">
                    <img src="img/btn_startTest.jpg" border="0" align="right" /><p>
                        &nbsp;
                    </p>
                    <img src="img/btn_trainingVideos.jpg" border="0" align="right" style="clear: both; float: right;" />
                </p>
                <p>
                    <strong>Skills Required: </strong>MS Excel Beginner level
                </p>
                <p>
                    This test assesses your knowledge about converting a table from pdf to excel sheet.
                        The test is of 20 minutes. One attempt per day is allowed.
                </p>
                <p>
                    <i><b>Duration:</b> 20 minutes</i>
                </p>
                <p>
                    <i><b>Reward:</b> Rs. 400</i>
                </p>
                <hr />
                <h4>Indexing Tasks</h4>
                <p style="float: right;">
                    <img src="img/btn_startTest.jpg" border="0" align="right" /><p>
                        &nbsp;
                    </p>
                    <img src="img/btn_trainingVideos.jpg" border="0" align="right" style="clear: both; float: right;" />
                </p>
                <p>
                    <strong>Skills Required: </strong>MS Excel Beginner level
                </p>
                <p>
                    This test assesses your knowledge of converting pdf index to excel. The test is
                        of 30 mins. One attempt per week is allowed.
                </p>
                <p>
                    <i><b>Duration:</b> 30 minutes</i>
                </p>
                <p>
                    <i><b>Reward:</b> Rs. 1000</i>
                </p>
                <hr />
                <h4>Images Tasks</h4>
                <p style="float: right;">
                    <img src="img/btn_startTest.jpg" border="0" align="right" /><p>
                        &nbsp;
                    </p>
                    <img src="img/btn_trainingVideos.jpg" border="0" align="right" style="clear: both; float: right;" />
                </p>
                <p>
                    <strong>Skills Required: </strong>Photoshop CS5 Beginner level
                </p>
                <p>
                    This test assesses your knowledge of resizing and cropping images from pdf. The
                        test is of 20 minutes to complete. One attempt per week is allowed. Result of test
                        will be emailed to you.
                </p>
                <p>
                    <i><b>Duration:</b> 20 minutes</i>
                </p>
                <p>
                    <i><b>Reward:</b> Rs. 300</i>
                </p>
                <hr />
                <h4>Mapping Tasks
                </h4>
                <p style="float: right;">
                    <img src="img/btn_startTest.jpg" border="0" align="right" /><p>
                        &nbsp;
                    </p>
                    <img src="img/btn_trainingVideos.jpg" border="0" align="right" style="clear: both; float: right;" />
                </p>
                <p>
                    <strong>Skills Required: </strong>BookMicro
                </p>
                <p>
                    This test assesses your skills of checking and editing mapped book. The test is
                        of 60 minutes. One attempt per week is allowed.
                </p>
                <p>
                    <i><b>Duration:</b> 60 minutes</i>
                </p>
                <p>
                    <i><b>Reward:</b> Rs. 3000</i>
                </p>
                <hr />
                <h4>Proofreading Tasks</h4>
                <p style="float: right;">
                    <img src="img/btn_startTest.jpg" border="0" align="right" /><p>
                        &nbsp;
                    </p>
                    <img src="img/btn_trainingVideos.jpg" border="0" align="right" style="clear: both; float: right;" />
                </p>
                <p>
                    <strong>Skills Required: </strong>English Average
                </p>
                <p>
                    This test assesses your skill of rectifying mistakes in the text by looking at the
                        image source. This test is of 30 mins. One attempt per day is allowed.
                </p>
                <p>
                    <i><b>Duration:</b> 30 minutes</i>
                </p>
                <p>
                    <i><b>Reward:</b> Rs. 500</i>
                </p>
            </div>
        </div>
    </div>
    
     <div id="modal_Login" class="modal">
                <asp:Panel ID="pnlLogInSubmitButton" runat="server" DefaultButton="imgbtnLogin">
                    <p class="closeBtn">
                        <img src="img/icon_close.png" align="right" /></p>
                    <p style="font-size: 28px; text-align: center; color: #666666; font-family: 'Open Sans';
                        margin-top: 20px;">
                        Enter below details to login....</p>
                    <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Login"
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
                    <asp:Label ID="lblPassword" Style="margin-left: 0%; margin-right: 1.8%" runat="server">Password:</asp:Label>
                    <%--  <label id="lblPassword">
                    Password:
                </label>--%>
                    <asp:TextBox ID="tbxPassword" runat="server" CssClass="txtPasswordFormat" TextMode="Password"
                        ValidationGroup="register"></asp:TextBox>
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Password is required."
                        ControlToValidate="tbxPassword" ForeColor="red" Text="*" ToolTip="Required Field"
                        ValidationGroup="Login"></asp:RequiredFieldValidator>
                    <br />
                    <br />
                    <p>
                        <asp:CheckBox ID="cbxRememberMe" Style="margin-left: 68%" runat="server" />
                        Keep me signed in
                    </p>
                    <br />
                    <br />
                    <asp:ImageButton src="img/btn_submit.png" Style="margin-left: 30%; margin-right: auto;
                        margin-bottom: 4px" ID="imgbtnLogin" OnClick="imgbtnLogin_Click" runat="server"
                        ValidationGroup="Login" />
                </asp:Panel>
            </div>
</asp:Content>

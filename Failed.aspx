<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true"
    CodeBehind="Failed.aspx.cs" Inherits="Outsourcing_System.Failed" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
    <%--<script type="text/javascript" src="http://code.jquery.com/jquery-1.10.0.min.js"></script>--%>
    <%--<script src="FinalScripts/jquery.js"></script>--%>
<%--    <script src="FinalScripts/jquery-ui.min.js"></script>--%>
    
    <%--<script src="FinalScripts/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>--%>
    <%-- <script type="text/javascript" src="http://code.jquery.com/jquery-1.10.0.min.js"></script>
     <link href="FinalStyles/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" type="text/css" />
    <script src="FinalScripts/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>--%>
    
   <%-- <script src="FinalScripts/easypiechart.min.js" type="text/javascript"></script>--%>
     <%-- <link href="FinalStyles/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" type="text/css" />
    <script src="FinalScripts/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>--%>
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
   
    <asp:HiddenField runat="server" ID="hdPieChartResult" />
    
  <%--  <div id="DivError" runat="server" visible="false">
            <div style="font-size: 10pt; color: Green; text-align: left; background-color: #ffcdd6;
                margin-left: 280px; margin-right: 270px; border: 1px solid red; padding: 5px;
                margin-top: 4%; font-family: Sans-Serif;">
                <table>
                    <tr>
                        <td rowspan="2">
                            <img src="img/red-error.gif" />
                        </td>
                        <td>
                            <asp:Label ID="lblError" runat="server" CssClass="ErrorMsgText"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="spacer" style="padding-top: 2px">
            </div>
        </div>
        <div id="divSuccess" runat="server" visible="false">
            <div style="font-size: 10pt; color: Green; text-align: left; background-color: #B3FFB3;
                margin-left: 430px; margin-right: 420px; border: 1px solid Green; padding: 5px;
                font-family: Sans-Serif;">
                <table>
                    <tr>
                        <td rowspan="2">
                        </td>
                        <td>
                            <asp:Label ID="lblSuccess" runat="server" CssClass="ErrorMsgText"></asp:Label>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="spacer" style="padding-top: 2px">
            </div>
        </div>--%>
    <div id="mid" align="center">
        <h2>
            Sorry you did not qualify. Please try next time.</h2>
        <asp:Label ID="lbl" runat="server" class="chart"><span class="percent"></span></asp:Label>
        <script type="text/javascript" src="http://code.jquery.com/jquery-1.10.0.min.js"></script>
        <script src="FinalScripts/easypiechart.min.js" type="text/javascript"></script>
        <script type="text/javascript">
            document.addEventListener('DOMContentLoaded', function () {
                var chart = window.chart = new EasyPieChart(document.querySelector('span'), {
                    easing: 'easeOutElastic',
                    delay: 3000,
                    onStep: function (from, to, percent) {
                        this.el.children[0].innerHTML = Math.round(percent);
                    }
                });
            });
        </script>
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

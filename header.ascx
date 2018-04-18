<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="header.ascx.cs" Inherits="ReadHowYouWant.controls.header" %>
<%@ Register Src="LoginControl.ascx" TagName="Lin" TagPrefix="LoginCntrl" %>

<script language="javascript" type="text/javascript">
    var elem = '<%= ddSelectedCountry.ClientID %>';
</script>

<table width="100%" border="0" style="border-width: 0px" cellspacing="0" cellpadding="0">
    <tr>
        <td align="left" valign="top">
            <table width="100%" border="0" cellspacing="0" cellpadding="0" class="Header">
                <tr>
                    <td>
                        <div class="headerLogo">
                            <div class="headerLogoLinkArea" onclick="javascript:window.location='/'">
                            </div>
                            <div class="chooseFormatButtonLink" onclick="javascript:window.location='/format-selection-guide.aspx'">
                            </div>
                        </div>
                    </td>
                    <td>
                        <table width="95%" border="0" cellspacing="0" cellpadding="0">
                            <LoginCntrl:Lin ID="LoginControl" runat="server" />
                            <tr>
                                <td width="9" align="right" valign="top">
                                    <img src="../Images/bluepanel_left.jpg" width="9" alt="" />
                                </td>
                                <td align="left" width="100%" class="bluepanelbg">
                                    <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                        <tr>
                                            <td width="33%" align="left">
                                                <a id="lnkBrailDaisySite" rel="lnkHeaderLogin" href="/Braille" valign="middle" class="toplinks"
                                                    title="Text-only Braille Daisy Site">Braille & Daisy Site</a>&nbsp;
                                            </td>
                                            <td width="1%" align="center" valign="middle">
                                                <img src="../Images/bluepanel_sep.jpg" width="5" height="23" alt="" />
                                            </td>
                                            <td width="48%" align="left">
                                                &nbsp;Region:&nbsp;
                                                <label id="lblForCountrySelect" runat="server">
                                                    <asp:DropDownList ID="ddSelectedCountry" runat="server" CssClass="SelectList" name="select country"
                                                        AutoPostBack="True" OnSelectedIndexChanged="ddSelectedCountry_SelectedIndexChanged">
                                                        <asp:ListItem Selected="True" Title="United States" label="United States">United States</asp:ListItem>
                                                        <asp:ListItem Title="Australia" label="Australia" Value="Australia">Australia</asp:ListItem>
                                                        <asp:ListItem Title="United Kingdom" label="United Kingdom">United Kingdom</asp:ListItem>
                                                        <asp:ListItem Title="Canada" label="Canada">Canada</asp:ListItem>
                                                        <asp:ListItem Title="NewZealand" label="NewZealand" Value="NewZealand">NewZealand</asp:ListItem>
                                                    </asp:DropDownList>
                                                </label>
                                            </td>
                                            <td width="1%" align="center" valign="middle">
                                                <img src="../images/bluepanel_sep.jpg" width="5" height="23" alt="" />
                                            </td>
                                            <td width="9%" align="center" valign="middle">
                                                &nbsp;<a href="/accessibility/text-size-guideline.aspx" title="Guidelines for text size"
                                                    class="toplinks">
                                                    <img src="../images/text-guidelines.png" width="22px" height="20px" alt="Text Size" /></a>&nbsp;
                                            </td>
                                            <td width="1%" align="center" valign="middle">
                                                <img src="../images/bluepanel_sep.jpg" width="5" height="23" alt="" />
                                            </td>
                                            <td width="10%" align="right">
                                                <a href="http://www.readhowyouwant.com/blog/" class="toplinks" target="_blank" title="Company blog">
                                                    Blog</a>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td width="6" align="left" valign="top">
                                    <img src="../images/bluepanel_right.jpg" width="6" alt="" />
                                </td>
                            </tr>
                            <tr style="height: 20px;">
                                <td id="phoneDiv" runat="server" colspan="3" class="helpLine">
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>
<!--LOGIN DIV SHOW PANEL-->
<div id="lightbox-panel">
    <!--Login DIV -->
    <!--close-panel3-->
    <asp:Panel ID="pnlLogin" runat="server" DefaultButton="btnSigIn123">
        <a id="close-panel3" href="#">
            <div style="float: right;">
                <img src="/images/close-icon.png"></div>
        </a>
        <div id="simpleDialog" class="l-box">
            <img src="/images/LogInImg.gif" />
            <div class="formBoxC">
                <div id="ProgressBar">
                    <asp:Image ID="Image1" runat="server" ImageUrl="/images/loading.gif" Width="25" Height="25"
                        AlternateText="Processing" />
                </div>
                <center>
                    <asp:Label ID="lblError" runat="server" ForeColor="Red"></asp:Label></center>
                <div class="titleC">
                    Email:<span class="red"> * </span>
                </div>
                <div class="fieldC">
                    <label id="lblForUserName" runat="server">
                        <asp:TextBox ID="txtUserName" CssClass="FormTextControls" runat="server" Style="width: 165px;"
                            ToolTip="User name" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtUserName"
                            ErrorMessage="Please enter user id" Font-Size="10px" SetFocusOnError="True" ValidationGroup="LoginGroup"
                            Display="Dynamic" EnableTheming="True" Font-Names="sans-serif"></asp:RequiredFieldValidator>
                        <asp:RegularExpressionValidator ID="RegularExpressionValidator2" runat="server" ErrorMessage="Please enter a valid email"
                            Font-Size="9px" SetFocusOnError="True" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                            ControlToValidate="txtUserName" ValidationGroup="LoginGroup" Display="Dynamic"></asp:RegularExpressionValidator>
                    </label>
                </div>
            </div>
            <div class="noFloat">
            </div>
            <div class="formBoxC">
                <div class="titleC">
                    Password: <span class="red">*</span></div>
                <div class="fieldC">
                    <label id="lblForPassword" runat="server">
                        <asp:TextBox ID="txtPassword" CssClass="FormTextControls" runat="server" Style="width: 165px;"
                            TextMode="Password" ToolTip="Password" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtPassword"
                            ErrorMessage="Please enter password" Font-Size="9px" SetFocusOnError="True" ValidationGroup="LoginGroup"></asp:RequiredFieldValidator>
                    </label>
                </div>
            </div>
            <br />
            <div class="noFloat">
            </div>
            <asp:CheckBox ID="chkLogin" runat="server" Text="Remember me at this computer." Checked="True"
                ForeColor="Black" CssClass="LoginCheckBox"></asp:CheckBox>
            <div class="formBoxC" style="margin: 10px 10px 10px 50px;">
                <asp:ImageButton ID="btnSigIn123" runat="server" OnClientClick="ShowProgressBar()"
                    ValidationGroup="LoginGroup" ImageUrl="/images/signin_btn.png" CssClass="SubmitBoxC"
                    OnClick="btnSigIn123_Click" AlternateText="Click to sign in" />
                <a id="close-panel" href="#">
                    <img src="/images/cancel_btn.png" alt="cancel login" />
                </a>
            </div>
            <div class="optionsB">
                <div class="soptions">
                    <a id="show-panel1" href="#" onclick="displayDiv('ForgotPassword','simpleDialog')"
                        class="a3">Forgot Password </a><b>|</b><a href="#" onclick="displayDiv('CreateUser','simpleDialog')"
                            class="a3"> Register Now!</a>
                </div>
                <div class="soptions">
                    <!--
                <a class="options" href="/Customer/register.aspx" target="_self" title="Get registered">
                    </a>
             -->
                </div>
            </div>
        </div>
    </asp:Panel>
    <!--set - region -->
    <asp:Panel ID="pnlRegion" runat="server" DefaultButton="btnSetRegion">
        <div id="SetRegion" class="l-box" style="background-image: url('/images/select-region.jpg');">
            <div class="formBoxC">
                <div id="Div2">
                </div>
                <div class="titleC">
                    <br />
                </div>
                <div class="fieldC">
                    <br />
                    <asp:DropDownList ID="lstRegions" runat="server" Font-Size="Medium" CssClass="SelectList"
                        name="select country" AutoPostBack="False" Width="240px">
                        <asp:ListItem Selected="True" Title="United States" label="United States">United States</asp:ListItem>
                        <asp:ListItem Title="Australia" label="Australia" Value="Australia">Australia</asp:ListItem>
                        <asp:ListItem Title="United Kingdom" label="United Kingdom">United Kingdom</asp:ListItem>
                        <asp:ListItem Title="Canada" label="Canada">Canada</asp:ListItem>
                        <asp:ListItem Title="NewZealand" label="NewZealand" Value="NewZealand">NewZealand</asp:ListItem>
                    </asp:DropDownList>
                    <br />
                    <asp:Button ID="btnSetRegion" Text="Proceed" runat="server" OnClick="btnSetRegion_Click" />
                </div>
            </div>
            <div class="noFloat">
            </div>
            <br />
        </div>
    </asp:Panel>
    <!--Forgot Password DIV -->
    <asp:Panel ID="pnlForgot" runat="server" DefaultButton="LinkBtn2">
        <div class="login-box" id="ForgotPassword" style="display: none">
            <img src="/images/ForgotPassword.jpg" />
            <br />
            <center>
                <asp:Label ID="lblForgotError" runat="server" ForeColor="Red"></asp:Label></center>
            <br />
            <div class="formBoxC">
                <div class="titleC">
                    Email:<span class="red"> * </span>
                </div>
                <div class="fieldC">
                    <asp:TextBox ID="txtUserNameForgot" CssClass="FormTextControls" runat="server" Style="width: 165px;" />
                    <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage="Please enter email"
                        Font-Size="9px" ControlToValidate="txtUserNameForgot" ValidationGroup="forgot"
                        SetFocusOnError="True" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ValidationGroup="forgot"
                        ControlToValidate="txtUserNameForgot" ErrorMessage="Please enter a valid email"
                        Font-Size="9px" ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"
                        SetFocusOnError="True" Display="Dynamic"></asp:RegularExpressionValidator>
                </div>
            </div>
            <div class="formBoxC" style="margin: 10px 10px 10px 55px;">
                <asp:ImageButton ID="LinkBtn2" runat="server" ImageUrl="/images/btnSend.png" OnClick="LinkBtn2_Click"
                    ValidationGroup="forgot" CssClass="SubmitBoxC" />
                <a id="close-panel1" href="#">
                    <img src="/images/cancel_btn.png" alt="cancel login" />
                </a>
            </div>
            <a href="#" onclick="displayDiv('simpleDialog','ForgotPassword')" class="a3">Login</a>
            <b>|</b><a href="#" onclick="displayDiv('CreateUser','ForgotPassword')" class="a3">
                Register Now!</a>
        </div>
    </asp:Panel>
    <!-- Creat User Account DIV -->
    <asp:Panel ID="pnlRegister" runat="server" DefaultButton="btnRegister123">
        <div class="r-box" id="CreateUser" style="display: none">
        <asp:Button OnClick="sldkfja"

        </div>
    </asp:Panel>
</div>
<!-- /lightbox-panel -->
<div id="lightbox">
</div>
<!-- /lightbox -->

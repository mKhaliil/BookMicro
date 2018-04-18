<%@ Page Language="C#" MasterPageFile="~/MasterPages/OnlineTestMasterPage.Master" AutoEventWireup="true" Inherits="Login" Title="Untitled Page" Codebehind="Login.aspx.cs" %>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <script src="scripts/jquery-1.4.1.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        window.onload = function load() {
            Sys.WebForms.PageRequestManager.getInstance().add_endRequest(EndRequestHandler);
        }
        $(document).ready(function() {
            $("a#show-panel").click(function() {
                $("#lightbox, #lightbox-panel").fadeIn(400);
            })
            $("a#close-panel").click(function() {
                $("#lightbox, #lightbox-panel").fadeOut(400);
            })
            $("a#close-panel1").click(function() {
                $("#lightbox, #lightbox-panel").fadeOut(400);
            })
            $("a#close-panel2").click(function() {
                $("#lightbox, #lightbox-panel").fadeOut(400);
            })
            $("a#close-panel3").click(function() {
                $("#lightbox, #lightbox-panel").fadeOut(400);
            })
        })
        function showLightDialog() {
            $("#lightbox, #lightbox-panel").fadeIn(400);
        }
        function displayDiv(divshow, divhide) {
            ///DIV SHOW.
            document.getElementById(divshow).style.display = "block";
            //DIV HIDE.
            document.getElementById(divhide).style.display = "none";
        }
        function ShowOnlyLogin() {
            document.getElementById("simpleDialog").style.display = "block";
            //document.getElementById("ForgotPassword").style.display = "none";
            document.getElementById("CreateUser").style.display = "none";
        }

        function Test() {
            $("#lightbox, #lightbox-panel").fadeIn(10);
        }
        function Test1() {
            $("#lightbox, #lightbox-panel").fadeIn(10);
        }
        function ShowProgressBar() {

            if (typeof (Page_ClientValidate("LoginGroup")) == 'function')

                Page_ClientValidate();

            if (Page_IsValid) {


                document.getElementById("ProgressBar").style.visibility = "visible";

            }
            return true;
        }
        function anchorClick(event, anchorObj) {
            if (anchorObj.click) {
                anchorObj.click()
            } else if (document.createEvent) {
                if (event.target !== anchorObj) {
                    var evt = document.createEvent("MouseEvents");
                    evt.initMouseEvent("click", true, true, window,
          0, 0, 0, 0, 0, false, false, false, false, 0, null);
                    var allowDefault = anchorObj.dispatchEvent(evt);
                    // you can check allowDefault for false to see if
                    // any handler called evt.preventDefault().
                    // Firefox will *not* redirect to anchorObj.href
                    // for you. However every other browser will.
                }
            }
        }
        function treeViewPostBack() {
            document.getElementById('isTreePostBack').value = 'true';
        }
        function hideFreeTextBox() {
            anchorClick(event, document.getElementById('close-panel3'));
        }
        function EndRequestHandler() {
            try {
                if (document.getElementById('isTreePostBack').value == 'true') {
                    //alert('isTreePostBack is true');
                    //anchorClick(event, document.getElementById('show-panel'))
                    showLightDialog();
                    document.getElementById('isTreePostBack').value = 'false';
                }
            }
            catch (e) {
                alert(e);
            }
        }
    </script>

    <input id="isTreePostBack" type="hidden" value="false" />
    <table width="100%" border="0" align="center" cellpadding="0" cellspacing="0">
        <tr>
            <td width="250" align="left" valign="top">
                <table width="100%" border="0" cellspacing="0" cellpadding="0">
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr class="bth">
                                    <td width="1">
                                    </td>
                                    <td align="center">
                                        <img src="img/si_02.gif" alt="Sign In " width="95" height="33" />
                                    </td>
                                    <td width="1">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbg">
                                    </td>
                                    <td align="left" valign="top">
                                        <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0">
                                            <tr>
                                                <td height="5" align="left" valign="middle" class="fo">
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="20" align="left" valign="middle" class="fo">
                                                    User Name:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="20" align="left" valign="middle" style="padding-left: 20px;">
                                                    <div class="sfb">
                                                        <div class="stb">
                                                            <asp:TextBox ID="txtUserName" runat="server" class="fieldBoxC" Style="width: 165px;"></asp:TextBox></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="20" align="left" valign="middle" class="fo">
                                                    Password:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="20" align="left" valign="middle" style="padding-left: 20px;">
                                                    <div class="sfb">
                                                        <div class="stb">
                                                            <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" class="fieldBoxC"
                                                                Style="width: 165px;"></asp:TextBox></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="20" align="left" valign="middle" class="fo">
                                                    User Type:
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="20" align="left" valign="middle" style="padding-left: 10px;">
                                                    <table>
                                                        <tr>
                                                            <td valign="middle">
                                                                <label for="radio-1">
                                                                    Admin</label>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:RadioButton ID="rdAdmin" runat="server" GroupName="type" Checked="true" />
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="middle">
                                                                <label for="radio-2">
                                                                    Editor</label>
                                                            </td>
                                                            <td valign="top">
                                                                <asp:RadioButton ID="rdUser" runat="server" GroupName="type" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="20" align="left" valign="middle" style="padding-left: 10px;">
                                                    <asp:ImageButton ID="Submit" runat="server" ImageUrl="img/sb.gif" OnClick="btnLogin_Click" OnClientClick="treeViewPostBack();"/>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="bbg">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="bbg" height="1">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr class="bth">
                                    <td width="1">
                                    </td>
                                    <td align="center">
                                        <img src="img/si_03.gif" alt="Search " width="98" height="33" />
                                    </td>
                                    <td width="1">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbg">
                                    </td>
                                    <td align="left" valign="top" style="padding: 2px 2px 5px 2px;">
                                        <table width="95%" border="0" align="center" cellpadding="0" cellspacing="0" class="smg">
                                            <tr>
                                                <td height="25" align="center" valign="middle">
                                                    <h3>
                                                        <img src="img/sch.gif" alt="Search" width="140" height="12" /></h3>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="25" align="center" valign="middle">
                                                    <div class="sfb">
                                                        <div class="stb">
                                                            <input class="fieldBoxC" name="srch" type="text" style="width: 165px;" /></div>
                                                    </div>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="25" align="center" valign="middle">
                                                    <span class="fo">
                                                        <input type="image" name="sb2" src="img/sb.gif" />
                                                    </span>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="bbg">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="bbg" height="1">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr class="bth">
                                    <td width="1">
                                    </td>
                                    <td align="center">
                                        <img src="img/si_04.gif" width="54" height="33" alt="" /><img src="img/si_05.gif"
                                            width="55" height="33" alt="" /><img src="img/si_06.gif" width="54" height="33" alt="" /><img
                                                src="img/si_07.gif" width="54" height="33" alt="" />
                                    </td>
                                    <td width="1">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbg">
                                    </td>
                                    <td align="center" valign="middle">
                                        <img src="img/fb.gif" width="212" height="154" alt="" />
                                    </td>
                                    <td class="bbg">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="bbg" height="1">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <table width="100%" border="0" cellspacing="0" cellpadding="0">
                                <tr class="bth">
                                    <td width="1">
                                    </td>
                                    <td align="center">
                                        <img src="img/si_08.gif" width="54" height="33" alt="" /><img src="img/si_09.gif"
                                            width="55" height="33" alt="" /><img src="img/si_10.gif" width="54" height="33" alt="" /><img
                                                src="img/si_11.gif" width="54" height="33" alt="" />
                                    </td>
                                    <td width="1">
                                    </td>
                                </tr>
                                <tr>
                                    <td class="bbg">
                                    </td>
                                    <td align="center" valign="middle">
                                        <img src="img/tw.gif" width="215" height="169" alt="" />
                                    </td>
                                    <td class="bbg">
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" class="bbg" height="1">
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </td>
            <td align="left" valign="top" style="padding-left: 10px; padding-top: 5px;">
                <span class="bl">BookMicro</span> by ReadHowYouWant is a seamless book conversion
                outsourcing solution. Being first of its kind, it plays a role of a 'task manager'
                between outsource contractors/providers and authors/publishers who want their book
                to be converted into different formats. The main advantage of using BookMicro is
                its reliability. It is because built-in quick conversion tools help create same
                quality from people with different backgrounds and working habbits. So, if you want
                to experience the wonders of BookMicro, just write to us at info@bookmicro.com.
                <p>
                    &nbsp;</p>
                <div align="center">
                    <object type="application/x-shockwave-flash" style="width: 425px; height: 350px;"
                        data="http://www.youtube.com/v/GwQMnpUsj8I">
                        <param name="movie" value="http://www.youtube.com/v/GwQMnpUsj8I" />
                    </object>
                </div>
                <p>
                    <img src="img/serv.gif" alt="Services" width="225" height="30" /></p>
                <p>
                    <img src="img/blt.gif" width="9" height="12" alt="" />
                    Print Formats</p>
                <p>
                    <img src="img/blt.gif" width="9" height="12" alt="" />
                    Braille</p>
                <p>
                    <img src="img/blt.gif" width="9" height="12" alt="" />
                    DAISY &amp; Audio</p>
                <p>
                    <img src="img/blt.gif" width="9" height="12" alt="" />
                    E-books
                </p>
            </td>
        </tr>
    </table>
    <div id="lightbox-panel">
        <!--Login DIV -->
        <!--close-panel3-->
        <a id="close-panel3" href="#">
            <div style="float: right;">
                <img src="img/close.gif" /></div>
        </a>
        <div id="simpleDialog" class="l-box">
            <div class="formBoxC">
                <div id="ProgressBar">
                    <asp:Image ID="Image1" runat="server" ImageUrl="img/loading.gif" Width="25" Height="25"
                        AlternateText="Processing" />
                </div>
            </div>
            <div class="noFloat">
            </div>
            <div class="formBoxC">
                <div class="titleC">
                    <asp:UpdatePanel ID="upd2" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="Submit" EventName="Click" />
                        </Triggers>
                        <ContentTemplate>
                            <asp:Label ID="lblMessage" runat="server" Text="Incorrect Login Details!"></asp:Label>
                            <%-- <ftb:freetextbox id="txtEditor1" runat="server" height="65px" width="250px" text=""
                                    toolbarlayout="Bold,Italic;CreateLink,Unlink;Cut,Copy,Paste;Undo,Redo">
                                </ftb:freetextbox>--%>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </div>
            <br />
            <div class="noFloat">
            </div>
            <div class="formBoxC" style="margin: 10px 10px 10px 50px;">
                <%--<asp:Button ID="btnSave" runat="server" CssClass="button" Text="Edit" OnClick="btnSave_Click"
                        OnClientClick="hideFreeTextBox();" />
                    <asp:Button ID="btnSplit" runat="server" CssClass="button" Text="Split" OnClick="btnSplit_Click"
                        OnClientClick="hideFreeTextBox();" />
                    <asp:Button ID="btnCaps" runat="server" CssClass="button" Text="Caps" OnClick="btnCaps_Click"
                        OnClientClick="hideFreeTextBox();" />--%>
            </div>
        </div>
    </div>
    <div id="lightbox">
    </div>
</asp:Content>

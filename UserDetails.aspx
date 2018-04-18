<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true"
    CodeBehind="UserDetails.aspx.cs" Inherits="Outsourcing_System.UserDetails" %>

<%@ Register Src="UserControls/CommonControl/ucShowMessage.ascx" TagName="ucShowMessage" TagPrefix="uc1" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
    <script type="text/javascript">
        $(function () {
            var windowWidth = $(window).width() / 2;
            var windowHeight = $(window).height() / 2;
            $('#userDetails').css({ 'width': windowWidth, 'height': windowHeight });
        });

    </script>
    <style type="text/css">
        #userDetails {
            border: 1px solid #cccccc;
            border-radius: 25px;
            padding: 10px 10px 10px 10px;
            text-align: left;
            width: 40%;
            margin-left: auto;
            margin-right: auto;
            margin-bottom: 60px;
            background-color: #ADDFFF;
        }
    </style>
    <script type="text/javascript">

        function showProfileCreatedDialog() {

            $("#divProfileCreatedMsg").css("display", "block");

            $("#divProfileCreatedDialog").dialog({
                resizable: false,
                height: 100,
                width: 463,
                modal: true,
                buttons: {
                    "Ok": function () {
                        $(this).dialog("close");
                        window.location = '<%= ResolveUrl("OnlineTestUser.aspx") %>';
                    },
                    "Close": function () {
                        $(this).dialog("close");
                        window.location = '<%= ResolveUrl("OnlineTestUser.aspx") %>';
                    }
                }
            });
        }

        $(function () {

            //////////Dialog///////////////

            var dlg = $("#dialog1").dialog({

                appendTo: "#dialogAfterMe",
                autoOpen: false,
                modal: true,
                draggable: true,
                height: 400,
                width: 570,
                position: "center",
                resizable: false
                //                ,
                //                open: function (type, data) {
                //                    $(this).parent().appendTo("form");
                //                }
            });

            //            dlg.parent().appendTo(jQuery("form:first"));

            $("#ibtnAddDetails").click(function () {
                //                                alert('aa');
                $("#dialog1").dialog("open");
            });

            $("#ibtnSave").click(function () {
                //dlg.parent().appendTo(jQuery("form:first"));
                $("#dialog1").dialog("close");
                //$("#ibtnAddDetails").attr("disabled", "disabled");
                //$('#ctl00_mainBodyContents_tbxAccountNumber').val('');
            });
            $("#ibtnClose").click(function () {
                //                alert('aa');
                //                dlg.parent().appendTo(jQuery("form:first"));
                $("#dialog1").dialog("close");
            });


            function check() {

                if (document.getElementById('tbxAccountTitle').value == ""
                    || document.getElementById('tbxAccountTitle').value == undefined) {
                    alert("Please Enter a value.");
                    return false;
                }
                return true;
            }
        });

        function clearAllFields() {
            $('#ctl00_mainBodyContents_tbxProfileName').val(' ');
            $('#ctl00_mainBodyContents_tbxPassword').val(' ');
            $('#ctl00_mainBodyContents_tbxIdCardNumber').val(' ');
            $('#ctl00_mainBodyContents_tbxMobileNumber').val(' ');
            $('#ctl00_mainBodyContents_tbxExperience').val(' ');
            $('#ctl00_mainBodyContents_tbxDescription').val(' ');
            $('#ctl00_mainBodyContents_tbxAccountNumber').val(' ');
            $('#ctl00_mainBodyContents_ddlEducation').val('Please Select');
            $('#ctl00_mainBodyContents_ddlGender').val('Please Select');
            $('#ctl00_mainBodyContents_tbxCountryOfResidence').val(' ');
            $('#ctl00_mainBodyContents_tbxPaypalNumber').val(' ');
        }
    </script>
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
    <div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">

        <uc1:ucShowMessage ID="ucShowMessage1" runat="server" />

    </div>

    <div id="modal_Login" class="modal">
        <asp:Panel ID="pnlLogInSubmitButton" runat="server" DefaultButton="imgbtnLogin">
            <p class="closeBtn">
                <img src="img/icon_close.png" align="right" />
            </p>
            <p style="font-size: 28px; text-align: center; color: #666666; font-family: 'Open Sans'; margin-top: 20px;">
                Enter below details to login....
            </p>
            <asp:ValidationSummary ID="ValidationSummary1" runat="server" ValidationGroup="Login"
                ForeColor="red" DisplayMode="List" />
            <asp:Label ID="lblEmail" Style="margin-top: 20px" runat="server">Email:</asp:Label>
            <asp:TextBox ID="tbxPopupEmail" CssClass="txtLoginFormat" Style="margin-top: 20px; margin-left: 28px; margin-bottom: -35px"
                runat="server"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator1" ForeColor="red" runat="server"
                ErrorMessage="Email is required." ControlToValidate="tbxPopupEmail" Text="*" ToolTip="Required Field"
                ValidationGroup="Login"></asp:RequiredFieldValidator>
            <asp:RegularExpressionValidator ID="RegularExpressionValidator2" ControlToValidate="tbxPopupEmail"
                runat="server" ValidationExpression="\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*"
                ValidationGroup="Login" ForeColor="red" Text="*" ErrorMessage="Email is not vaild."></asp:RegularExpressionValidator>
            <br />
            <br />
            <asp:Label ID="lblPassword" Style="margin-left: 0%; margin-right: 1.8%" runat="server">Password:</asp:Label>
            <asp:TextBox ID="tbxPopupPassword" runat="server" CssClass="txtPasswordFormat" TextMode="Password"
                ValidationGroup="register"></asp:TextBox>
            <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ErrorMessage="Password is required."
                ControlToValidate="tbxPopupPassword" ForeColor="red" Text="*" ToolTip="Required Field"
                ValidationGroup="Login"></asp:RequiredFieldValidator>
            <br />
            <br />
            <p>
                <asp:CheckBox ID="cbxRememberMe" Style="margin-left: 346px" runat="server" />
                Keep me signed in
            </p>
            <br />
            <br />
            <asp:ImageButton src="img/btn_submit.png" Style="margin-left: 155px; margin-right: auto; margin-bottom: 4px"
                ID="imgbtnLogin" OnClick="imgbtnLogin_Click" runat="server"
                ValidationGroup="Login" />
        </asp:Panel>
    </div>


    <h1>
        <strong style="margin-left: auto; margin-right: auto;">Please Complete the form below.</strong>
    </h1>
    <asp:ValidationSummary ID="vsUserDetails" runat="server" Style="margin-left: 580px"
        ValidationGroup="registrationForm" DisplayMode="List" />
    <br />
    <table id="userDetails">
        <tr>
            <td>
                <table>
                    <tr>
                        <td align="left">
                            <strong>Login Name: </strong>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbxProfileName" Style="border: 1px solid black; height: 30px; width: 280px"
                                runat="server"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvLoginName" runat="server" ErrorMessage="Login Name is required."
                                ControlToValidate="tbxProfileName" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                    <tr id="trPassword" runat="server">
                        <td align="left">
                            <strong>Password: </strong>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbxPassword" Style="border: 1px solid black; height: 30px; width: 280px"
                                runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password is required."
                                ControlToValidate="tbxPassword" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator>
                        </td>
                    </tr>
                      <tr id="trConfirmPasword" runat="server">
                        <td align="left">
                            <strong>Confirm Password: </strong>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbxConfirmPassword" Style="border: 1px solid black; height: 30px; width: 280px"
                                runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvConfirmPassword" runat="server" ErrorMessage="Confirm Password is required."
                                ControlToValidate="tbxConfirmPassword" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator>
                            <asp:CompareValidator ID="cvConNewPassword" ValidationGroup="changePassword" runat="server" ControlToValidate="tbxConfirmPassword"
                                ControlToCompare="tbxPassword"
                         ErrorMessage="Confirm Password don't match with password." ToolTip="Password must be the same" ForeColor="red" Text="*"/>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>Full Name: </strong>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbxFullName" Enabled="false" Style="border: 1px solid black; height: 30px; width: 280px"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>Email: </strong>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbxEmail" Style="border: 1px solid black; height: 30px; width: 280px"
                                runat="server" Enabled="false"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>Gender: </strong>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlGender" runat="server" Style="width: 150px; border-radius: 10px; height: 30px;">
                                <asp:ListItem>Please Select</asp:ListItem>
                                <asp:ListItem>Male</asp:ListItem>
                                <asp:ListItem>Female</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>National Id Number: </strong>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbxIdCardNumber" Style="border: 1px solid black; height: 30px; width: 280px"
                                runat="server"></asp:TextBox>
                            <%-- <asp:RequiredFieldValidator ID="rfvIdCardNum" runat="server" ErrorMessage="Id Card Number is required."
                        ControlToValidate="tbxIdCardNumber" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator>--%>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>Country: </strong>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbxCountryOfResidence" Style="border: 1px solid black; height: 25px; width: 280px; border-radius: 0px;" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>Mobile Number: </strong>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbxMobileNumber" Style="border: 1px solid black; height: 30px; width: 280px"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>Education: </strong>
                        </td>
                        <td align="left">
                            <asp:DropDownList ID="ddlEducation" runat="server" Style="width: 150px; border-radius: 10px; height: 30px;">
                                <asp:ListItem>Please Select</asp:ListItem>
                                <asp:ListItem>PhD</asp:ListItem>
                                <asp:ListItem>Master</asp:ListItem>
                                <asp:ListItem>Bachelor</asp:ListItem>
                                <asp:ListItem>College</asp:ListItem>
                                <asp:ListItem>High School</asp:ListItem>
                                <asp:ListItem>School</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>Bank Account Number: </strong>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbxAccountNumber" Style="border: 1px solid black; height: 30px; width: 280px"
                                runat="server"></asp:TextBox>
                            <input type="button" value="Add Bank Details" name="ibtnAddDetails" id="ibtnAddDetails" />
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>Paypal Account : </strong>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbxPaypalNumber" Style="border: 1px solid black; height: 30px; width: 280px"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>Experience: </strong>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbxExperience" Style="border: 1px solid black; height: 30px; width: 280px"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>Description: </strong>
                        </td>
                        <td align="left">
                            <asp:TextBox ID="tbxDescription" TextMode="MultiLine" Style="border: 1px solid black; height: 100px; width: 365px; resize: none;"
                                runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td align="left">
                            <strong>Upload Image: </strong>
                        </td>
                        <td align="left">
                            <asp:FileUpload ID="fuImage" Style="height: 30px; width: 350px; border-radius: 0px"
                                runat="server" />
                            <asp:Label ID="lblUploadImageName" runat="server"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td align="left"></td>
                        <td align="left">
                            <asp:Button ID="btnSave" runat="server" CssClass="button" Text="Save" Style="width: 60px; border: 1px solid black;"
                                OnClick="btnSave_Click" ValidationGroup="registrationForm" />
                            <%--<asp:Button ID="btnClearAll" runat="server" Text="Clear All" OnClientClick="clearAllFields();" />--%>
                            <input type="button" value="Clear All" class="button" id="btnClearAll" onclick="clearAllFields();" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    <div id="dialog1" title="Add Bank Details">
        <table>
            <tr>
                <td align="left">
                    <strong>Account Title: </strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="tbxAccountTitle" Style="border: 1px solid black; height: 25px; width: 300px; border-radius: 0px;" runat="server" EnableViewState="true"></asp:TextBox>
                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ErrorMessage="Login Name is required."
                ControlToValidate="tbxProfileName" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <strong>Account Type: </strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="tbxAccountType" Style="border: 1px solid black; height: 25px; width: 300px; border-radius: 0px;" runat="server"></asp:TextBox>
                    <%--<asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ErrorMessage="Password is required."
                ControlToValidate="tbxPassword" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <strong>Bank Name: </strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="tbxBankName" Style="border: 1px solid black; height: 25px; width: 300px; border-radius: 0px;" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <strong>Branch: </strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="tbxBranch" Style="border: 1px solid black; height: 25px; width: 300px; border-radius: 0px;" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <strong>Bank Code: </strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="tbxBankCode" Style="border: 1px solid black; height: 25px; width: 300px; border-radius: 0px;" runat="server"></asp:TextBox>
                    <%-- <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ErrorMessage="Id Card Number is required."
                ControlToValidate="tbxIdCardNumber" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator>--%>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <strong>City: </strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="tbxCity" Style="border: 1px solid black; height: 25px; width: 300px; border-radius: 0px;" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <strong>Country: </strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="tbxCountry" Style="border: 1px solid black; height: 25px; width: 300px; border-radius: 0px;" runat="server"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td></td>
                <td></td>
            </tr>
        </table>
        <div style="margin-left: 53.4%; margin-top: 20px;">
            <input type="button" value="Save" class="button" id="ibtnSave" onclick="return check();" />
            <input type="button" value="Cancel" class="button" id="ibtnClose" />
        </div>
    </div>
    <div id="dialogAfterMe">
    </div>

    <div id="divProfileCreatedDialog" title="Your profile is created successfully!">
        <%--<div id="divProfileCreatedMsg" style="display: none;">
            <p style="font-size:medium;">Your profile is created successfully</p>
        </div>--%>
    </div>

</asp:Content>

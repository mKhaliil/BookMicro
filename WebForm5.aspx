<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true"
    CodeBehind="WebForm5.aspx.cs" Inherits="Outsourcing_System.WebForm5" %>
    
<asp:Content ID="Content1" ContentPlaceHolderID="mainHeadContents" runat="server">

   <%-- <link href="Styles/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>--%>
    <script type="text/javascript">

        /////////////////////Auto complete textbox////////////////////
        $(function () {
            var aa = $("#tbxEditorNames");
            $("#tbxEditorNames").autocomplete(
            {
                source: "autocompleteHandler.ashx",
                minLength: 1,
                delay: 100,
                select: AutoCompleteSelectHandler
            });
        });

        function AutoCompleteSelectHandler(event, ui) {
            var selectedObj = ui.item;
            $("#divCheckBoxListControl").html("");
            $('#divSuccessMessage').html('');
            $("#divSuccessMessage").hide();
            OnSelectionGetPassedTests(selectedObj.value);
        }

        function OnSelectionGetPassedTests(name) {

            //            selectedCheckBoxes = [];
            methodURL = "WebForm5.aspx/GetPassedTests";
            parameters = "{'name':'" + name + "'}";

            $.ajax({
                type: "POST",
                url: methodURL,
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: BindCheckBoxList
            });
        }

        function BindCheckBoxList(result) {

            var TestTypes = ["Comparison", "Image", "Table", "Index"];
            var check = false;

            var ClearedTests = result.d.split(',');

            for (var i = 0; i < TestTypes.length; i++) {

                check = false;

                for (var j = 0; j < ClearedTests.length; j++) {
                    if (TestTypes[i] == ClearedTests[j]) {
                        check = true;
                    }
                }

                if (check) {
                    var checkBox_Checked = "<input type='checkbox' class='chk' name='cbxlTests' onclick = 'createAlerts(this)' checked value='" + TestTypes[i] + "'/>" + TestTypes[i];
                    $('#divCheckBoxListControl').append(checkBox_Checked);
                } else {
                    var checkBox = "<input type='checkbox' name='cbxlTests' onclick = 'createAlerts(this)' value='" + TestTypes[i] + "'/>" + TestTypes[i];
                    $('#divCheckBoxListControl').append(checkBox);
                }
            }
        }

        var selectedCheckBoxes = [];
        function createAlerts(theCheckbox) {
            if (theCheckbox.checked) {
                selectedCheckBoxes.push(theCheckbox.value);
//                $("#divSuccessMessage").hide();
            }
            else {
                selectedCheckBoxes.pop(theCheckbox.value);
//                $("#divSuccessMessage").hide();
            }
        }

        ////////////////////////////////end/////////////////////////////

        //On save button's click get list of checked check boxes
        function GetSelectedCheckBoxList() {

            var chkArray = [];
            $(".chk:checked").each(function () {
                chkArray.push($(this).val());
            });

            var selected = chkArray.join(',') + "," + selectedCheckBoxes.join(',');
            SaveSelectedTests(selected);
        }

        function SaveSelectedTests(result) {

            $('#divSuccessMessage').html('');
            methodURL = "WebForm5.aspx/GetTests_ByCheckBox";
            parameters = "{'passedTests':'" + result + "'}";

            $.ajax({
                type: "POST",
                url: methodURL,
                data: parameters,
                contentType: "application/json; charset=utf-8",
                dataType: "json",
                success: ShowSuccessMessege(result)
            });
        }

        function ShowSuccessMessege(result) {

            //            $('#divSuccessMessage').display = 'block';
            $("#divSuccessMessage").show();
            $('#divSuccessMessage').append('Tests are successfully cleared.');
        }


        function CloseDialog() {
            $(this).dialog('close');
        }

        function ShowPopupJq() {
            $(function () {
                var wWidth = $(window).width();
                var dWidth = wWidth * 0.52;
                var wHeight = $(window).height();
                var dHeight = wHeight * 0.8;

                var NewUserDialog = $("#dialog").dialog({
                    appendTo: "#dialogAfterMe",
                    title: "Add New Editor",
                    height: dHeight,
                    width: dWidth,
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
    </script>

</asp:Content>

<asp:Content ID="Content5" ContentPlaceHolderID="mainBodyContents" runat="server">
        <div id="DivError" runat="server" visible="false">
        <div style="font-size: 10pt; color: Green; text-align: left; background-color: #ffcdd6;
            border: 1px solid red; padding: 5px; font-family: Sans-Serif; width: 93%; margin: auto;">
            <table>
                <tr>
                    <td rowspan="2">
                        <img src="img/red-error.gif" />
                    </td>
                    <td>
                        <div id="divText" runat="server">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer" style="padding-top: 2px">
        </div>
    </div>
    <div id="divSuccess" runat="server" visible="false">
        <div style="font-size: 10pt; color: Green; text-align: left; background-color: #B3FFB3;
            margin-left: 280px; margin-right: 190px; border: 1px solid Green; padding: 5px;
            font-family: Sans-Serif;">
            <table>
                <tr>
                    <td rowspan="2">
                        <img src="img/green-ok.gif" />
                    </td>
                    <td>
                        <asp:Label ID="lblSuccess" runat="server" CssClass="ErrorMsgText"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
        <div class="spacer" style="padding-top: 2px">
        </div>
    </div>
    <div style="width: 105%; margin-left: 10px; margin-top: 35px">
       
        <h3>
            <asp:Label runat="server" Style="margin-right: 1230px; color: #003366; font-size: 20px"
                ID="Label1">Admin Panel</asp:Label></h3>
    </div>
    <br />
    <div id="divSuccessMessage" visible="false" style="font-size: 10pt; color: Green;
        text-align: left; background-color: #B3FFB3; margin-left: 280px; margin-right: 190px; margin-bottom: 40px;
        border: 1px solid Green; padding: 5px; font-size: 14px; font-family: Sans-Serif; display:none">
    </div>
    <div id="divErrorMessege">
    </div>
    <p style="margin-left: 44px; color: #003366; font-size: 15px">
        Editor Name
        <input type="text" id="tbxEditorNames" style="border: 1px solid black; margin-left: 40px;
            height: 28px; width: 300px; border-radius: 3px" />
        <input id="AddNewEditor" type="button" onclick="ShowPopupJq();" value="Add New" style="border: 1px solid #2A4F96;
            background-color: #2A4F96; color: #FFFFFF; width: 70px; height: 28px; margin-left: 2px;"></p>
    <div style="width: 105%; margin-left: 44px; margin-top: 25px; color: #003366; font-size: 15px">
        Clear Tests
        <div style="width: 600px; margin-top: -22px; margin-left: 130px;">
            <div id="divCheckBoxListControl">
            </div>
            <p>
                <input type="button" id="btnSave" style="border: 1px solid #2A4F96; background-color: #2A4F96;
                    color: #FFFFFF; width: 55px; height: 28px; margin-left: 252px; margin-top: 20px"
                    value="Save" onclick="GetSelectedCheckBoxList();" />
            </p>
        </div>
    </div>
    <div id="dialog" style="display: none; overflow: auto">
        <div id="userDetails" style="margin-right: 10px">
            <p>
                <strong style="margin-left: 66px">Login Name: </strong>
                <asp:TextBox ID="tbxProfileName" Style="border: 1px solid black; margin-left: 55px;
                    height: 30px; border-radius: 2px" runat="server"></asp:TextBox>
                <%-- <asp:RequiredFieldValidator ID="rfvLoginName" runat="server" ErrorMessage="Login Name is required."
                            ControlToValidate="tbxProfileName" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator>--%>
            </p>
            <p>
                <strong style="margin-left: 66px">Password: </strong>
                <asp:TextBox ID="tbxPassword" Style="border: 1px solid black; margin-left: 70px;
                    height: 30px; border-radius: 2px" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password is required."
                    ControlToValidate="tbxPassword" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator>
            </p>
            <p>
                <strong style="margin-left: 68px">Full Name: </strong>
                <asp:TextBox ID="tbxFullName" Style="border: 1px solid black; margin-left: 67px;
                    height: 30px; border-radius: 2px" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ErrorMessage="Full Name is required."
                    ControlToValidate="tbxFullName" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator>
            </p>
            <p>
                <strong style="margin-left: 66px">Email: </strong>
                <asp:TextBox ID="tbxEmail" Style="border: 1px solid black; margin-left: 102px; height: 30px;
                    border-radius: 2px" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email is required."
                    ControlToValidate="tbxEmail" CssClass="err" Text="*" ToolTip="Required Field"
                    ValidationGroup="registrationForm"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" ControlToValidate="tbxEmail" runat="server"
                    ErrorMessage="Email is not vaild." ValidationExpression="\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*"
                    Text="*" ValidationGroup="registrationForm"></asp:RegularExpressionValidator>
            </p>
            <p>
                <strong style="margin-left: 66px">Id Card Number: </strong>
                <asp:TextBox ID="tbxIdCardNumber" Style="border: 1px solid black; margin-left: 27px;
                    height: 30px; border-radius: 2px" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvIdCardNum" runat="server" ErrorMessage="Id Card Number is required."
                    ControlToValidate="tbxIdCardNumber" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator>
            </p>
            <p>
                <strong style="margin-left: 66px">Mobile Number: </strong>
                <asp:TextBox ID="tbxMobileNumber" Style="border: 1px solid black; margin-left: 35px;
                    height: 30px; border-radius: 2px" runat="server"></asp:TextBox>
            </p>
            <p>
                <strong style="margin-left: 66px">Education: </strong>
                <asp:DropDownList ID="ddlEducation" runat="server" Style="margin-left: 74px; width: 150px;
                    border-radius: 10px; height: 30px; border-radius: 2px">
                    <asp:ListItem>Please Select</asp:ListItem>
                    <asp:ListItem>PhD</asp:ListItem>
                    <asp:ListItem>Master</asp:ListItem>
                    <asp:ListItem>Bachelor</asp:ListItem>
                    <asp:ListItem>College</asp:ListItem>
                    <asp:ListItem>High School</asp:ListItem>
                    <asp:ListItem>School</asp:ListItem>
                </asp:DropDownList>
            </p>
            <%--    <p>
                        <strong style="margin-left: 66px">Account Number: </strong>
                        <asp:TextBox ID="tbxAccountNumber" Style="border: 1px solid black; margin-left: 24px;
                            height: 30px;border-radius:2px" runat="server"></asp:TextBox>
                    </p>
                    <p>
                        <strong style="margin-left: 66px">Experience: </strong>
                        <asp:TextBox ID="tbxExperience" Style="border: 1px solid black; margin-left: 64px;
                            height: 30px;border-radius:2px" runat="server"></asp:TextBox>
                    </p>--%>
            <p>
                <strong style="margin-left: 66px">Experience: </strong>
                <asp:TextBox ID="tbxExperience" TextMode="MultiLine" Style="border: 1px solid black;
                    margin-left: 64px; height: 100px; width: 395px; border-radius: 2px" runat="server"></asp:TextBox>
            </p>
            <%--   <p>
                        <strong style="margin-left: 66px">Upload Image: </strong>
                        <asp:FileUpload ID="fuImage" Style="margin-left: 44px; height: 30px; width: 350px;
                            border-radius: 0px" runat="server" />
                        <asp:Label ID="lblUploadImageName" runat="server"></asp:Label>
                    </p>--%>
            <%-- <p>
            <asp:Button ID="Button1" runat="server" CssClass="button" Text="Save" Style="width: 60px;
                border: 1px solid black; margin-left: 220px;" OnClick="btnSave_Click" ValidationGroup="registrationForm" />
        </p>--%>
        </div>
        <asp:Button ID="btnSaveDialog" runat="server" CssClass="button" Text="Save" Style="width: 60px;
            border: 1px solid black; margin-left: 492px;" ValidationGroup="registrationForm"
            UseSubmitBehavior="false" OnClick="btnSave_Click" />
        <asp:Button ID="btnCloseDialog" runat="server" CssClass="button" Text="Close" Style="width: 60px;
            border: 1px solid black; margin-left: 2px;" ValidationGroup="registrationForm"
            OnClientClick="CloseDialog();" />
    </div>
    <div id="dialogAfterMe">
    </div>
</asp:Content>
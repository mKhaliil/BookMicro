<%@ Page Title="" Language="C#" MasterPageFile="~/AdminMaster.Master" AutoEventWireup="true"
    CodeBehind="ManageUser.aspx.cs" Inherits="Outsourcing_System.ManageUser" %>

<%@ Register Src="CustomControls/ProcessControl.ascx" TagName="ProcessControl" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="mainHeadContents" runat="server">
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
            methodURL = "ManageUser.aspx/GetPassedTests";
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
            methodURL = "ManageUser.aspx/GetTests_ByCheckBox";
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
                var dWidth = wWidth * 0.37;
                var wHeight = $(window).height();
                var dHeight = wHeight * 0.77;

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
    <style type="text/css">
        .style1
        {
            width: 133px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="mainBodyContents" runat="server">
    <%--<link href="Styles/jquery-ui-1.10.3.custom.min.css" rel="stylesheet" type="text/css" />
    <script src="scripts/jquery-1.9.1.js" type="text/javascript"></script>
    <script src="scripts/jquery-ui-1.10.3.custom.min.js" type="text/javascript"></script>--%>
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
    <div id="divSuccessMessage" visible="false" style="font-size: 10pt; color: Green;
        text-align: left; background-color: #B3FFB3; margin-left: 280px; margin-right: 190px;
        margin-bottom: 40px; border: 1px solid Green; padding: 5px; font-size: 14px;
        font-family: Sans-Serif; display: none">
    </div>
    <div id="divErrorMessege">
    </div>
    <div style="width:100%; height:100%">
  
    <div style="margin: 0 auto; border-radius: 10px">
       
    <table width="90%" align="center" style="margin-left: 40px">
        <tr>
            <td class="title" colspan="2">
                 <h3>
            <asp:Label runat="server" Style="color: #003366; font-size: 20px; " ID="Label2">BookMicro Account Management</asp:Label></h3>
            </td>

        </tr>
        <tr>
            <td> &nbsp;</td><td>&nbsp;</td>
            
        </tr>
        <tr>
            <td align="left" style="margin-left: 44px; color: #003366; font-size: 15px">
                Select User Category :
            </td>
            <td align="left">
                <asp:DropDownList ID="ddUserCategory" runat="server" Style="width: 146px" AutoPostBack="True"
                    OnSelectedIndexChanged="ddUserCategory_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left" style="margin-left: 44px; color: #003366; font-size: 15px">
                Select User Name :
            </td>
            <td align="left">
                <asp:DropDownList ID="ddUser" runat="server" Style="width: 146px" AutoPostBack="True"
                    OnSelectedIndexChanged="ddUser_SelectedIndexChanged">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left" style="margin-left: 44px; color: #003366; font-size: 15px">
                Full Name :
            </td>
            <td align="left">
                <asp:TextBox class="normaltext" ID="txtFullName" Style="border: 1px solid #003366;
                    height: 28px; width: 300px; border-radius: 3px" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" style="margin-left: 44px; color: #003366; font-size: 15px">
                Password :
            </td>
            <td align="left">
                <asp:TextBox ID="txtPassword" runat="server" Style="border: 1px solid #003366; height: 28px;
                    width: 300px; border-radius: 3px" class="normaltext"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" style="margin-left: 44px; color: #003366; font-size: 15px">
                Confirm Password :
            </td>
            <td align="left">
                <asp:TextBox ID="txtConfirmPassword" runat="server" Style="border: 1px solid #003366;
                    height: 28px; width: 300px; border-radius: 3px" class="normaltext"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" style="margin-left: 44px; color: #003366; font-size: 15px">
                Email :
            </td>
            <td align="left">
                <asp:TextBox ID="txtEmail" runat="server" Style="border: 1px solid #003366; height: 28px;
                    width: 300px; border-radius: 3px" class="normaltext"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td align="left" style="margin-left: 44px; color: #003366; font-size: 15px; height: 23px">
                Category :
            </td>
            <td align="left" style="height: 23px">
                <asp:DropDownList ID="ddCategory" runat="server">
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left" style="margin-left: 44px; color: #003366; font-size: 15px">
                Active User :
            </td>
            <td align="left">
                <asp:DropDownList ID="ddUserStatus" runat="server" CssClass="normaltext" Style="width: 146px">
                    <asp:ListItem Text="Active" Value="1" />
                    <asp:ListItem Text="Disabled" Value="0" />
                </asp:DropDownList>
            </td>
        </tr>
        <tr>
            <td align="left" class="normaltext" style="margin-left: 44px; color: #003366; font-size: 15px">
                Can Perform Tasks :
            </td>
            <td align="left">
                <div>
                    <uc1:ProcessControl ID="ProcessControl1" runat="server" />
                </div>
            </td>
        </tr>
        <tr>
            <td align="left">
                &nbsp;
            </td>
            <td align="left">
                <div style="margin-top: 20px;">
                    <asp:Button ID="btnUpdateUser" runat="server" Text="Update User" Style="border: 1px solid #2A4F96;
                        background-color: #2A4F96; color: #FFFFFF; width: 70px; height: 28px; margin-left: 2px;
                        width: 85px;" OnClick="btnUpdateUser_Click" />
                    <input id="AddNewEditor" type="button" onclick="ShowPopupJq();" value="Add New User"
                        style="border: 1px solid #2A4F96; background-color: #2A4F96; color: #FFFFFF;
                        width: 100px; height: 28px; margin-left: 2px;">
                    <asp:Button ID="btnDeleteUser" runat="server" Text="Delete User" Style="border: 1px solid #2A4F96;
                        background-color: #2A4F96; color: #FFFFFF; width: 70px; height: 28px; margin-left: 2px;
                        width: 85px;" OnClick="btnDeleteUser_Click" />
                </div>
            </td>
        </tr>
    </table>
    </div>
    </div>
    
    <div style="margin-left: 0; margin-right: 55%; margin-top: 2%;  border-radius: 10px">
    <div style="width: 197.4%; margin-left: 7.1%; margin-top: 15px"  class="title">
        <h3>
            <asp:Label runat="server" Style="color: #003366; font-size: 20px" ID="Label1">Clear Test</asp:Label></h3>
    </div>
    <%--   <div id="divSuccessMessage" visible="false" style="font-size: 10pt; color: Green;
        text-align: left; background-color: #B3FFB3; margin-left: 280px; margin-right: 190px; margin-bottom: 40px;
        border: 1px solid Green; padding: 5px; font-size: 14px; font-family: Sans-Serif; display:none">
    </div>
    <div id="divErrorMessege">
    </div>--%>
    <p style="margin-left: 44px; color: #003366; font-size: 15px">
        Editor Name :
        <input type="text" id="tbxEditorNames" style="border: 1px solid #003366; margin-left: 7.1%;
            height: 28px; width: 300px; border-radius: 3px" />
        <%-- <input id="AddNewEditor" type="button" onclick="ShowPopupJq();" value="Add New" style="border: 1px solid #2A4F96;
            background-color: #2A4F96; color: #FFFFFF; width: 70px; height: 28px; margin-left: 2px;">--%>
    </p>
    <div style="width: 105%; margin-left: 44px; margin-top: 25px; color: #003366; font-size: 15px">
        Clear Tests :
        <div style="width: 300px; margin-top: -22px; margin-left: 162px;">
            <div id="divCheckBoxListControl">
            </div>
            <p>
                <input type="button" id="btnSave" style="border: 1px solid #2A4F96; background-color: #2A4F96;
                    color: #FFFFFF; width: 55px; height: 28px; margin-left: 312px; margin-top: -20px"
                    value="Clear" onclick="GetSelectedCheckBoxList();" />
            </p>
        </div>
    </div>
    </div>
    
    <div style="margin-left: 0; margin-right: 5%; margin-top: 2%; border-radius: 10px">
        <div style="width: 93.4%; margin-left: 3.25%; margin-top: 35px" class="title">
            <h3>
                <asp:Label runat="server" Style="color: #003366; font-size: 20px"  ID="Label4">Create user account in workmeter</asp:Label></h3>
        </div>
        <div style="text-align: center; margin-top: 2%; margin-bottom: 5%">
            <div style="text-align: left; width: 65%; margin: auto; height: 600px;">
                <table style="margin-top: 3%; width: 824px;">
                    <tr>
                        <td style="color: #0090CB; font-size: large; font-weight: bold; font-weight: bolder">
                            Passed New Users
                        </td>
                       <%-- <td class="style1">
                        </td>
                        <td style="color: #0090CB; font-size: large; font-weight: bold; font-weight: bolder">
                            Passed Old Users
                        </td>--%>
                    </tr>
                    <tr>
                        <td>
                            <asp:ListBox ID="lbxLeft" runat="server" Height="352px" Width="245px" SelectionMode="Multiple">
                            </asp:ListBox>
                        </td>
                        <%--<td>
                                        <input type="button" class="btnSubmitReport" id="left" value="<<" />
                                        <input type="button" class="btnSubmitReport" id="right" value=">>" />
                                    </td>
                                    <td>
                                        <asp:ListBox ID="lbxRight" runat="server" Height="352px" Width="245px" SelectionMode="Multiple">
                                        </asp:ListBox>
                                    </td>--%>
                      <%--  <td class="style1">
                        </td>
                        <td>
                            <asp:ListBox ID="lbxRight" runat="server" Height="352px" Width="245px" SelectionMode="Multiple">
                            </asp:ListBox>
                        </td>--%>
                        <tr>
                            <td>
                                <asp:Button ID="btnGetNewUser" runat="server" Text="Get Users" Style="border: 1px solid #2A4F96;
                        background-color: #2A4F96; color: #FFFFFF; width: 70px; height: 28px; margin-left: 2px;
                        width: 85px;" OnClick="btnGetNewUser_Click"/>
                                 <asp:Button  Id="btnCreateAccount" Text="Create Account" Style="border: 1px solid #2A4F96;
                        background-color: #2A4F96; color: #FFFFFF; width: 100px; height: 28px; margin-left: 2px;
                        width: 113px;" OnClick="btnCreateAccount_Click" runat="server"/>
                            </td>
                            <td class="style1">
                            </td>
                            <%--<td>
                                <asp:Button ID="btnGetOldUser" runat="server" Text="Get Users" Style="border: 1px solid #2A4F96;
                        background-color: #2A4F96; color: #FFFFFF; width: 70px; height: 28px; margin-left: 2px;
                        width: 85px;" OnClick="btnGetOldUser_Click"/>--%>
                                <%--<asp:Button  Id="btnMappEmail" Text="Mapp Email" Style="border: 1px solid #2A4F96;
                        background-color: #2A4F96; color: #FFFFFF; width: 70px; height: 28px; margin-left: 2px;
                        width: 92px;" OnClick="btnMappEmail_Click" runat="server"/>
                            </td>--%>
                        </tr>
                    </tr>
                </table>
            </div>
        </div>
        </div>

    <div id="dialog" style="display: none; overflow: auto; width:60%">
        <div id="userDetails" style="margin-left:3%">
            <table>
                  <tr>
                    <td><strong>User Category: </strong></td>
                    <td>
                         <asp:DropDownList ID="ddlUserCategory" runat="server" Style="width: 150px; border-radius: 10px; height: 30px; border-radius: 2px; width:265px">
                    <asp:ListItem>Please Select</asp:ListItem>
                    <asp:ListItem>Junior</asp:ListItem>
                    <asp:ListItem>Senior</asp:ListItem>
                    <asp:ListItem>Admin</asp:ListItem>
                    <asp:ListItem>Team Lead</asp:ListItem>
                    <asp:ListItem>Power User</asp:ListItem>
                </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                   <td><strong>Profile Name: </strong></td>
                    <td>
                         <asp:TextBox ID="tbxProfileName" Style="border: 1px solid #003366; height: 30px; border-radius: 2px; width:265px" runat="server"></asp:TextBox>
                <%-- <asp:RequiredFieldValidator ID="rfvLoginName" runat="server" ErrorMessage="Login Name is required."
                            ControlToValidate="tbxProfileName" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator>--%>
                    </td>
                </tr>
                
                <tr>
                    <td>
                     <strong>Password: </strong>    
                    </td>
                    <td>
                        <asp:TextBox ID="tbxPassword" Style="border: 1px solid #003366;height: 30px; border-radius: 2px; width:265px" runat="server" TextMode="Password"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvPassword" runat="server" ErrorMessage="Password is required."
                    ControlToValidate="tbxPassword" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator>
                    </td>

                </tr>
                
                <tr>
                    <td><strong>Full Name: </strong></td>
                    <td><asp:TextBox ID="tbxFullName" Style="border: 1px solid #003366;height: 30px; border-radius: 2px; width:265px" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvFullName" runat="server" ErrorMessage="Full Name is required."
                    ControlToValidate="tbxFullName" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator></td>
                </tr>
                
                 <tr>
                    <td><strong>Email: </strong></td>
                    <td> <asp:TextBox ID="tbxEmail" Style="border: 1px solid #003366;height: 30px; border-radius: 2px; width:265px" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvEmail" runat="server" ErrorMessage="Email is required."
                    ControlToValidate="tbxEmail" CssClass="err" Text="*" ToolTip="Required Field"
                    ValidationGroup="registrationForm"></asp:RequiredFieldValidator>
                <asp:RegularExpressionValidator ID="revEmail" ControlToValidate="tbxEmail" runat="server"
                    ErrorMessage="Email is not vaild." ValidationExpression="\s*\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*\s*"
                    Text="*" ValidationGroup="registrationForm"></asp:RegularExpressionValidator></td>
                </tr>
                
                 <tr>
                    <td>
                         <strong>Id Card Number: </strong>
                    </td>
                    <td>
                        <asp:TextBox ID="tbxIdCardNumber" Style="border: 1px solid #003366;height: 30px; border-radius: 2px; width:265px" runat="server"></asp:TextBox>
                <asp:RequiredFieldValidator ID="rfvIdCardNum" runat="server" ErrorMessage="Id Card Number is required."
                    ControlToValidate="tbxIdCardNumber" Text="*" ToolTip="Required Field" ValidationGroup="registrationForm"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                
                 <tr>
                    <td>
                        <strong>Mobile Number: </strong>
                    </td>
                    <td>
                         <asp:TextBox ID="tbxMobileNumber" Style="border: 1px solid #003366;height: 30px; border-radius: 2px; width:265px" runat="server"></asp:TextBox>
                    </td>
                </tr>
                
                 <tr>
                    <td>
                         <strong>Education: </strong>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlEducation" runat="server" Style="width:265px;
                    border-radius: 10px; height: 30px; border-radius: 2px">
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
                    <td>
                         <strong>Experience: </strong>
                    </td>
                    <td>
                        <asp:TextBox ID="tbxExperience" TextMode="MultiLine" Style="border: 1px solid #003366;
                    height: 100px; width: 260px; border-radius: 2px" runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
           
        </div>
        <div style="margin-left:65.5%; margin-top: 1%">
            <asp:Button ID="btnSaveDialog" runat="server" CssClass="button" Text="Save" Style="width: 60px;
            border: 1px solid black; " ValidationGroup="registrationForm"
            UseSubmitBehavior="false" OnClick="btnSave_Click" />
        <asp:Button ID="btnCloseDialog" runat="server" CssClass="button" Text="Close" Style="width: 60px;
            border: 1px solid black; " ValidationGroup="registrationForm"
            OnClientClick="CloseDialog();" />
        </div>
        
    </div>
    <div id="dialogAfterMe">
    </div>
</asp:Content>

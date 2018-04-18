<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ucOnlineTestTasks.ascx.cs"
    Inherits="Outsourcing_System.UserControls.ucOnlineTestTasks" %>

<style type="text/css">
    .TestClearedFormatting {
        background-color: #FFE4B5;
        border: 1px solid #FC8B01;
        border-radius: 5px;
        float: left;
        margin-right: 20px;
        padding: 3px;
        text-align: left;
    }
</style>
<div>
    <div id="divNormalProfile" runat="server" visible="False">
        <div id="profileHD" style="margin-left: 20%;">
            <div id="info">
                <div id="img">
                    <asp:Image ID="imgUserProfile" Width="100" Height="100" align="left" runat="server" />
                </div>
                <h1>
                    <strong>
                        <asp:Label ID="lblUserName" runat="server"></asp:Label></strong></h1>
                <div style="margin-top: 0px;">
                    <div id="ql">
                        <div id="divImagesTest" class="TestClearedFormatting" runat="server" visible="false">
                            <asp:Label ID="lblImages" runat="server"> </asp:Label>
                        </div>
                        <div id="divTablesTest" class="TestClearedFormatting" runat="server" visible="false">
                            <asp:Label ID="lblTables" runat="server"></asp:Label>
                        </div>
                        <div id="divIndexTest" class="TestClearedFormatting" runat="server" visible="false">
                            <asp:Label ID="lblIndex" runat="server"></asp:Label>
                        </div>
                        <div id="divMappingTest" class="TestClearedFormatting" runat="server" visible="false">
                            <asp:Label ID="lblMapping" runat="server"></asp:Label>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <p>
            <br />
            <div style="margin-left: -15%; width: 600px;">
                <asp:Label ID="lblDescription" runat="server" Style="font-size: 13px; font-family: Open Sans; color: #333333"> </asp:Label>
            </div>
        </p>
    </div>
    <div id="divEditProfile" runat="server" visible="False">
        <div id="userDetails" style="border: 3px solid #ADDFFF; background-color: #C2DFFF; width: 525px; border-radius: 20px; margin-left: -4.5%; margin-bottom: 15px;">
            <table style="margin-top: 5%; margin-bottom: 5%; margin-right: 2%">
                <tr>
                    <td align="left">
                        <strong>Login Name: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxProfileName" Style="border: 1px solid black; height: 30px; width: 300px; border-radius: 10px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Full Name: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxFullName" Style="border: 1px solid black; height: 30px; width: 300px; border-radius: 10px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Email: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxEmail" Enabled="false" Style="border: 1px solid black; height: 30px; width: 300px; border-radius: 10px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>CNIC NO: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxCnicNo" Enabled="false" Style="border: 1px solid black; height: 30px; width: 300px; border-radius: 10px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr style="display: none">
                    <td align="left">
                        <strong>Account Number: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxAccountNumber" Style="border: 1px solid black; height: 30px; width: 300px; border-radius: 10px" runat="server"></asp:TextBox>

                        <asp:LinkButton ID="lbtnEditDetails" runat="server" OnClick="lbtnEditBankDetails_Click"
                            Style="color: #2a4f96; text-decoration: none; font-family: Verdana; font-size: 15px; font-weight: bold">Edit details</asp:LinkButton>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Mobile Number: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxMobileNumber" Style="border: 1px solid black; height: 30px; width: 300px; border-radius: 10px" runat="server"></asp:TextBox>
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
                        <strong>Experience: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxExperience" Style="border: 1px solid black; height: 30px; width: 300px; border-radius: 10px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Description: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxDescription" Style="resize: none; border: 1px solid black; border-radius: 0px" runat="server" TextMode="MultiLine" Height="150px" Width="300px"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Upload Image: </strong>
                    </td>
                    <td align="left">
                        <asp:FileUpload ID="fuProfileImage" Style="height: 25px; width: 304px; border-radius: 0px"
                            runat="server" />
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnEdit" runat="server" CssClass="button" Text="Edit" Style="width: 60px; border: 1px solid black; margin-left: 181px;"
                            OnClick="btnEdit_Click" />
                        <asp:Button ID="btnCancel" runat="server" CssClass="button" Text="Close" Style="width: 60px; border: 1px solid black; margin-left: 2px;"
                            OnClick="btnClose_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divChangePassword" runat="server" visible="False">
        <div id="divChangePassword" style="border: 3px solid #ADDFFF; background-color: #C2DFFF; width: 525px; border-radius: 20px; margin-left: -4.5%; margin-bottom: 15px;">
            
              <asp:ValidationSummary ID="vsChangePassword" runat="server" ValidationGroup="changePassword" ForeColor="red" DisplayMode="List" />

            <table style="margin-top: 5%; margin-bottom: 5%; margin-right: 1%">
                <tr>
                    <td align="left">
                        <strong>Old Password: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxOldPassword" TextMode="Password" Style="border: 1px solid black; height: 30px; width: 300px; border-radius: 10px" runat="server"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rfvOldPassword" ValidationGroup="changePassword" runat="server" ErrorMessage="Old Password is required." 
                        ControlToValidate="tbxOldPassword" ToolTip="Old Password is a required field" ForeColor="red" Text="*">
                      </asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>New Password: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxNewPassword"  TextMode="Password" Style="border: 1px solid black; height: 30px; width: 300px; border-radius: 10px" runat="server"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="rvfNewPassword" ValidationGroup="changePassword" runat="server" ErrorMessage="New Password is required." 
                        ControlToValidate="tbxNewPassword" ToolTip="New Password is a required field" ForeColor="red" Text="*">
                      </asp:RequiredFieldValidator>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Confirm New Password: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxConNewPassword" TextMode="Password" Style="border: 1px solid black; height: 30px; width: 300px; border-radius: 10px" runat="server"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvConNewPassword" ValidationGroup="changePassword" runat="server" ErrorMessage="Confirm New Password is required." 
                        ControlToValidate="tbxConNewPassword" ToolTip="Confirm New Password is a required field" ForeColor="red" Text="*">
                      </asp:RequiredFieldValidator>
                        <asp:CompareValidator ID="cvConNewPassword" ValidationGroup="changePassword" runat="server" ControlToValidate="tbxConNewPassword" ControlToCompare="tbxNewPassword"
                         ErrorMessage="Confirm New Password don't match with new password." ToolTip="Password must be the same" ForeColor="red" Text="*"/>
                    </td>
                </tr>

                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnChangePassword" runat="server" ValidationGroup="changePassword" CssClass="button" Text="Save" Style="width: 60px; border: 1px solid black; margin-left: 176px;"
                            OnClick="btnChangePassword_Click" />
                        <asp:Button ID="btnClose" runat="server" CssClass="button" Text="Close" Style="width: 60px; border: 1px solid black; margin-left: 2px;"
                            OnClick="btnClose_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    <div id="divEditBankDetails" runat="server" visible="False">
        <div id="divBankDetails" style="border: 3px solid #ADDFFF; background-color: #C2DFFF; width: 525px; border-radius: 20px; margin-left: -4.5%; margin-bottom: 15px;">

           <table style="margin-top: 5%; margin-bottom: 5%; margin-right: 2%">
                <tr>
                    <td align="left">
                        <strong>Account Title: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxAccountTitle" Style="border: 1px solid black; height: 30px; border-radius: 0px; width: 350px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Account Type: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxAccountType" Style="border: 1px solid black; height: 30px; border-radius: 0px; width: 350px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Bank Name: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxBankName" Style="border: 1px solid black; height: 30px; border-radius: 0px; width: 350px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Branch: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxBranch" Style="border: 1px solid black; height: 30px; border-radius: 0px; width: 350px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Bank Code: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxBankCode" Style="border: 1px solid black; height: 30px; border-radius: 0px; width: 350px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>City: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxCity" Style="border: 1px solid black; height: 30px; border-radius: 0px; width: 350px"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Country: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxCountry" Style="border: 1px solid black; height: 30px; border-radius: 0px; width: 350px"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td></td>
                    <td>
                        <asp:Button ID="btnEditBankDetails" runat="server" CssClass="button" Text="Edit" Style="width: 72px" OnClick="btnEditBankDetails_Click" />
                        <asp:Button ID="btnCancelBankDetails" runat="server" CssClass="button" Text="Cancel" OnClick="btnCancelBankDetails_Click" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
</div>
<%--<div id="divAccountInformation" runat="server" visible="false">
    <div style="display: block; left: 0; top: 0; position: fixed; width: 100%; height: 100%; padding: 200px; opacity: 0.7; background-color: Gray;">
    </div>
    <div style="position: fixed; left: 26%; top: 16%; border: solid 1px #addfff; border-radius: 0px; background-color: White; padding: 20px;">
        <div id="divAccountDetails">
            <div style="background-color: #c2dfff; color: black; height: 35px; width: 572px; margin-top: -17px; margin-left: -20px; margin-right: -20px; margin-bottom: 20px; border-radius: 6px">
                <span style="margin-top: 7px; margin-left: 20px; float: left; text-decoration: none; font-family: Verdana; font-size: 16px; font-weight: bold">Edit Bank Details</span>
            </div>

            <table>
                <tr>
                    <td align="left">
                        <strong>Account Title: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxAccountTitle" Style="border: 1px solid black; height: 30px; border-radius: 0px; width: 350px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Account Type: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxAccountType" Style="border: 1px solid black; height: 30px; border-radius: 0px; width: 350px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Bank Name: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxBankName" Style="border: 1px solid black; height: 30px; border-radius: 0px; width: 350px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Branch: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxBranch" Style="border: 1px solid black; height: 30px; border-radius: 0px; width: 350px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Bank Code: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxBankCode" Style="border: 1px solid black; height: 30px; border-radius: 0px; width: 350px" runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>City: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxCity" Style="border: 1px solid black; height: 30px; border-radius: 0px; width: 350px"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>

                <tr>
                    <td align="left">
                        <strong>Country: </strong>
                    </td>
                    <td align="left">
                        <asp:TextBox ID="tbxCountry" Style="border: 1px solid black; height: 30px; border-radius: 0px; width: 350px"
                            runat="server"></asp:TextBox>
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-top: 20px; margin-left: 56.3%;">
            <asp:Button ID="btnEditBankDetails" runat="server" CssClass="button" Text="Edit"
                Style="width: 72px" OnClick="btnEditBankDetails_Click" />
            <asp:Button ID="btnCancelBankDetails" runat="server" CssClass="button" Text="Cancel"
                OnClick="btnCancelBankDetails_Click" />
        </div>
    </div>
</div>--%>

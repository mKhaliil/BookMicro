<%@ Page Language="C#" MasterPageFile="AdminMaster.Master" Async="true" AutoEventWireup="true" Inherits="NewTask" Title="Untitled Page" CodeBehind="NewTask.aspx.cs" %>

<%@ Register src="UserControls/CommonControl/ucShowMessage.ascx" tagname="ucShowMessage" tagprefix="uc1" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="Server">
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
        <!--div style="float:left"><a  class="link" href="AdminPanel.aspx">Admin Panel</a></div>
<div style="float:right"><asp:LinkButton ID="LinkButton1" runat="server" onclick="LinkButton1_Click" class="link">Log Out</asp:LinkButton></div-->

    <div id="divStatusMessages" style="width: 42.5%; margin: 0 auto;" runat="server">
                   <uc1:ucShowMessage ID="ucShowMessage1" runat="server" />
                </div>

     <table style="margin-left:18.5%; margin-top:3.3%">
        <tr>
            <td align="right" class="normaltext">
                Book ID :                
            </td>
            <td align="left">
                <asp:TextBox ID="txtBookID" runat="server" Style="border: 1px solid #003366; margin-left: 70px;
                    height: 25px; width:190px; border-radius: 5px" class="normaltext"></asp:TextBox>
                <asp:RegularExpressionValidator ID="RegularExpressionValidator1" runat="server" ControlToValidate="txtBookID"
                    CssClass="bbw" ErrorMessage="Invalid Book ID" ValidationExpression="[0-9]+" ValidationGroup="singleFile"></asp:RegularExpressionValidator>
            </td>
        </tr>
        <tr>
            <td align="right" class="normaltext">
                Book Title :
            </td>
            <td align="left">
                <asp:TextBox ID="txtBookTitle" runat="server" Style="border: 1px solid #003366; margin-left: 70px;
                    height: 25px; width:190px; border-radius: 5px" class="normaltext"></asp:TextBox>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtBookTitle"
                    CssClass="bbw" ErrorMessage="Book Title Required" ValidationGroup="singleFile"></asp:RequiredFieldValidator>
            </td>
        </tr>
        <tr>
            <td style="text-align: left" class="normaltext">
                Select Cropped PDF File :
            </td>
            <td style="text-align: center" class="normaltext">
                <%--<input id="File1" type="file" runat="server" />--%>
                <%--<asp:FileUpload ID="fuPdf" runat="server" AllowMultiple="true"/>--%>
                <asp:FileUpload ID="fuPdf" runat="server" />
            </td>
        </tr>
        
        <tr>
            <td>
                &nbsp;
            </td>
              <td colspan="2" align="center">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" OnClick="Submit_Click" CssClass="button"
                    ValidationGroup="singleFile" />&nbsp;&nbsp;<asp:Button ID="btnBatch" runat="server"
                        Text="Batch File" CssClass="button" OnClick="btnBatch_Click" />
            </td>
        </tr>
    </table>
</asp:Content>

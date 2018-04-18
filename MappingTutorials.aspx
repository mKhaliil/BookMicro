<%@ Page Title="" Language="C#" MasterPageFile="~/MasterPages/OnlineTestMasterPage.Master" AutoEventWireup="true" CodeBehind="MappingTutorials.aspx.cs" Inherits="Outsourcing_System.MappingTutorials" %>
<asp:Content ID="Content1" ContentPlaceHolderID="ContentPlaceHolder0" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="LinkPortion" runat="server">
</asp:Content>
<asp:Content ID="Content3" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<div id="mid" align="center">
        <h2>
            TUTORIALS FOR MAPPING TASKS</h2>
        <div>
            <asp:LinkButton ID="lbtnTestTraining" OnClick="lbtnTestTraining_Click" runat="server"
            Style="margin-left: 566px;float:left;margin-top:-130px; color: #2A4F96; font-size: 16px;">Tests and Trainings</asp:LinkButton>
        </div>
        <table border="0" align="right">
            <tr align="left" valign="middle">
                <td align="right">
                    <h3>
                        Are you Ready?</h3>
                </td>
                <td>
                    <asp:ImageButton ID="ibtnStartTest" ImageUrl="img/btn_startTest2.png" Width="216"
                        Height="81" runat="server" />
                </td>
            </tr>
        </table>
        <div id="spacer">
        </div>
        <div id="tutorials">
            <div id="tutorialL">
                <h4>
                    BookMicro Comparison 1</h4>
                <p>
                    <asp:ImageButton ID="ibtnBookComparison1_Text" ImageUrl="img/file-text.png" Width="32"
                        Height="32" runat="server" OnClick="ibtnBookComparison1_Text_Click" Style="display: inline" />
                    <asp:ImageButton ID="ibtnBookComparison1_Video" ImageUrl="img/file-video.png" Width="32"
                        Height="32" OnClick="ibtnBookComparison1_Video_Click" runat="server" />
                    This tutorial describes the ways to resize and crop images in Adobe Photoshop so
                    that the quality is not destroyed.....<span id="more"><asp:LinkButton ID="lbtnBookComparison1ReadMore"
                        OnClick="ibtnBookComparison1_Text_Click" runat="server">View Tutorial</asp:LinkButton></span>
                </p>
                <h4>
                    BookMicro Comparison 2
                </h4>
                <p>
                    <asp:ImageButton ID="ibtnBookComparison2_Text" ImageUrl="img/file-text.png" Width="32"
                        Height="32" runat="server" OnClick="ibtnBookComparison2_Text_Click" Style="display: inline" />
                    <asp:ImageButton ID="ibtnBookComparison2_Video" ImageUrl="img/file-video.png" Width="32"
                        Height="32" runat="server" OnClick="ibtnBookComparison2_Video_Click" />
                    This tutorial describes the ways to resize and crop images in Adobe Photoshop so
                    that the quality is not destroyed.....<span id="more"><asp:LinkButton ID="lbtnibtnBookComparison2ReadMore"
                        OnClick="ibtnBookComparison2_Text_Click" runat="server">View Tutorial</asp:LinkButton></span>
                </p>
             
                <h4>
                    &nbsp;
                </h4>
            </div>
            <div id="tutorialR">
                <h4>
                    Book Hierarchy
                </h4>
                <p>
                     <asp:ImageButton ID="ibtnBookHierarchy_Text" ImageUrl="img/file-text.png" Width="32"
                        Height="32" runat="server" OnClick="ibtnBookHierarchy_Text_Click" Style="display: inline" />
                    <asp:ImageButton ID="ibtnBookHierarchy_Video" ImageUrl="img/file-video.png" Width="32"
                        Height="32" runat="server" OnClick="ibtnBookHierarchy_Video_Click" />
                    This tutorial describes the ways to resize and crop images in Adobe Photoshop so
                    that the quality is not destroyed.....<span id="Span2"><asp:LinkButton ID="lbtnBookHierarchyReadMore"
                        OnClick="ibtnBookHierarchy_Text_Click" runat="server">View Tutorial</asp:LinkButton></span>
                </p>
                <h4>
                    Complex Bits</h4>
                <p>
                    <asp:ImageButton ID="ibtnComplexBits_Text" ImageUrl="img/file-text.png" Width="32"
                        Height="32" runat="server" OnClick="ibtnComplexBits_Text_Click" Style="display: inline" />
                    <asp:ImageButton ID="ibtnComplexBits_Video" ImageUrl="img/file-video.png" Width="32"
                        Height="32" runat="server" OnClick="ibtnComplexBits_Video_Click" />
                    This tutorial describes the ways to resize and crop images in Adobe Photoshop so
                    that the quality is not destroyed.....<span id="Span1"><asp:LinkButton ID="lbtnComplexBitsReadMore"
                        OnClick="ibtnComplexBits_Text_Click" runat="server">View Tutorial</asp:LinkButton></span>
                </p>
                
                <p>
                    &nbsp;</p>
            </div>
        </div>
    </div>
    <p>
        &nbsp;</p>
</asp:Content>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder2" runat="server">
</asp:Content>

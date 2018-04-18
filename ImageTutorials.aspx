<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true"
    CodeBehind="ImageTutorials.aspx.cs" Inherits="Outsourcing_System.ImageTutorials" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
        <div id="mid" align="center">
        <h2>
            TUTORIALS FOR IMAGES TASKS</h2>
        <div>
            <asp:LinkButton ID="lbtnTestTraining" OnClick="lbtnTestTraining_Click" runat="server"
            Style="margin-left: 566px;float:left;margin-top:-100px; color: #2A4F96; font-size: 16px;">Tests and Trainings</asp:LinkButton>
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
                    Image Resizing and Cropping</h4>
                <p>
                    <asp:ImageButton ID="ibtnImgCropping_Text" ImageUrl="img/file-text.png" Width="32"
                        Height="32" runat="server" OnClick="ibtnImgCropping_Text_Click" Style="display: inline" />
                    <asp:ImageButton ID="ibtnImgCropping_Video" ImageUrl="img/file-video.png" Width="32"
                        Height="32" OnClick="ibtnImgCropping_Video_Click" runat="server" />
                    This tutorial describes the ways to resize and crop images in Adobe Photoshop so
                    that the quality is not destroyed.....<span id="more"><asp:LinkButton ID="lbtnImgCroppReadMore"
                        OnClick="ibtnImgCropping_Text_Click" runat="server">View Tutorial</asp:LinkButton></span>
                </p>
                <h4>
                    Book Notices
                </h4>
                <p>
                    <asp:ImageButton ID="ibtnBookNotices_Text" ImageUrl="img/file-text.png" Width="32"
                        Height="32" runat="server" OnClick="ibtnBookNotices_Text_Click" Style="display: inline" />
                    <asp:ImageButton ID="ibtnBookNotices_Video" ImageUrl="img/file-video.png" Width="32"
                        Height="32" runat="server" OnClick="ibtnBookNotices_Video_Click" />
                    This tutorial describes the ways to resize and crop images in Adobe Photoshop so
                    that the quality is not destroyed.....<span id="more"><asp:LinkButton ID="lbtnBookNoticesReadMore"
                        OnClick="ibtnBookNotices_Text_Click" runat="server">View Tutorial</asp:LinkButton></span>
                </p>
               <%-- <h4>
                    Title Page Cropping
                </h4>
                <p>
                    <asp:ImageButton ID="ibtnTitleCropping_Text" ImageUrl="img/file-text.png" Width="32"
                        Height="32" runat="server" OnClick="ibtnTitleCropping_Text_Click" Style="display: inline" />
                    <asp:ImageButton ID="ibtnTitleCropping_Video" ImageUrl="img/file-video.png" Width="32"
                        Height="32" runat="server" OnClick="ibtnTitleCropping_Video_Click" />
                    This tutorial describes the ways to resize and crop images in Adobe Photoshop so
                    that the quality is not destroyed.....<span id="more"><asp:LinkButton ID="lbtnTitleCroppingReadMore"
                        OnClick="ibtnTitleCropping_Text_Click" runat="server">View Tutorial</asp:LinkButton></span>
                </p>--%>
                <h4>
                    &nbsp;
                </h4>
            </div>
            <div id="tutorialR">
                <%--<h4>
                    Adding effects to images
                </h4>
                <p>
                    <asp:ImageButton ID="ImageButton7" ImageUrl="img/file-text.png" Width="32" Height="32"
                        runat="server" Style="display: inline" />
                    <asp:ImageButton ID="ImageButton8" ImageUrl="img/file-video.png" Width="32" Height="32"
                        runat="server" />
                    This tutorial describes the ways to resize and crop images in Adobe Photoshop so
                    that the quality is not destroyed.....<span id="more">View Tutorial</span>
                </p>
                <h4>
                    Image Resizing and Cropping</h4>
                <p>
                    <asp:ImageButton ID="ImageButton9" ImageUrl="img/file-text.png" Width="32" Height="32"
                        runat="server" Style="display: inline" />
                    <asp:ImageButton ID="ImageButton10" ImageUrl="img/file-video.png" Width="32" Height="32"
                        runat="server" />
                    This tutorial describes the ways to resize and crop images in Adobe Photoshop so
                    that the quality is not destroyed.....<span id="more">View Tutorial</span>
                </p>
                <h4>
                    Converting Images to Vectors
                </h4>
                <p>
                    <asp:ImageButton ID="ImageButton11" ImageUrl="img/file-text.png" Width="32" Height="32"
                        runat="server" Style="display: inline" />
                    <asp:ImageButton ID="ImageButton12" ImageUrl="img/file-video.png" Width="32" Height="32"
                        runat="server" />
                    This tutorial describes the ways to resize and crop images in Adobe Photoshop so
                    that the quality is not destroyed.....<span id="more">View Tutorial</span>
                </p>--%>
                 <h4>
                    Title Page Cropping
                </h4>
                <p>
                    <asp:ImageButton ID="ibtnTitleCropping_Text" ImageUrl="img/file-text.png" Width="32"
                        Height="32" runat="server" OnClick="ibtnTitleCropping_Text_Click" Style="display: inline" />
                    <asp:ImageButton ID="ibtnTitleCropping_Video" ImageUrl="img/file-video.png" Width="32"
                        Height="32" runat="server" OnClick="ibtnTitleCropping_Video_Click" />
                    This tutorial describes the ways to resize and crop images in Adobe Photoshop so
                    that the quality is not destroyed.....<span id="Span1"><asp:LinkButton ID="lbtnTitleCroppingReadMore"
                        OnClick="ibtnTitleCropping_Text_Click" runat="server">View Tutorial</asp:LinkButton></span>
                </p>
                <p>
                    &nbsp;</p>
            </div>
        </div>
    </div>
    <p>
        &nbsp;</p>
</asp:Content>

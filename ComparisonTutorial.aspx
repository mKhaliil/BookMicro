<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true"
    CodeBehind="ComparisonTutorial.aspx.cs" Inherits="Outsourcing_System.ComparisonTutorial" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
    <div id="mid" align="center" style="font-weight: normal">
        <div style="margin-left: -750px; font-size: 16px;">
            <asp:Label ID="Label2" runat="server" Style="color: #2a4f96;" Text="Select Training Language:"></asp:Label>&nbsp;&nbsp;
            <asp:LinkButton ID="lbtnEnglish" OnClick="lblEnglish_Click" runat="server" Enabled="false">English</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="lbtnUrdu" OnClick="lbtnUrdu_Click" runat="server" Enabled="false">Urdu</asp:LinkButton>
        </div>
        <div style="float: right;">
            <a href="ComparisonTutorial.aspx#tutorials">
                <img src="img/file-text.png" /></a> <a href="ComparisonTutorial.aspx#video">
                    <img src="img/file-video.png" /></a></div>
        <h2>
            Comparison
        </h2>
        <div>
            <asp:LinkButton ID="lbtnTestTraining" OnClick="lbtnTestTraining_Click" runat="server"
                Style="margin-left: 566px; float: left; margin-top: -130px; color: #2A4F96; font-size: 16px;">Tests and Trainings</asp:LinkButton>
        </div>
        <asp:MultiView ID="mvTrainingVideos" runat="server">
            <asp:View ID="vEnglishVideos" runat="server">
                <div id="videoPanel">
                    <div id="videoBox">
                        <div id="video">
                            <iframe src="http://player.vimeo.com/video/126359714" width="500" height="375" frameborder="0"
                                webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="vUrduVideos" runat="server">
                <div id="videoPanel">
                    <div id="videoBox">
                        <div id="video">
                            <iframe src="http://player.vimeo.com/video/122422830" width="500" height="375" frameborder="0"
                                webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                        </div>
                    </div>
                </div>
            </asp:View>
        </asp:MultiView>
        <br />
        <br />
        <div id="tutorials">
            <br />
            <br />
            <br />
        </div>
    </div>
</asp:Content>

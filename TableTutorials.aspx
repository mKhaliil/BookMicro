<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true"
    CodeBehind="TableTutorials.aspx.cs" Inherits="Outsourcing_System.TableTutorials" %>

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
            <a href="TableTutorials.aspx#tutorials">
                <img src="img/file-text.png" /></a><a href="TableTutorials.aspx#video"><img src="img/file-video.png" /></a></div>
        <h2>
            Table
        </h2>
        <div>
            <asp:LinkButton ID="lbtnTestTraining" OnClick="lbtnTestTraining_Click" runat="server"
                Style="margin-left: 566px; float: left; margin-top: -130px; color: #2A4F96; font-size: 16px;">Tests and Trainings</asp:LinkButton>
        </div>
        <asp:MultiView ID="mvTrainingVideos" runat="server" ActiveViewIndex="0">
            <asp:View ID="vEnglishVideos" runat="server">
                <div id="videoPanel">
                    <div id="videoBox">
                        <div id="video">
                            <iframe src="http://player.vimeo.com/video/98420399" width="500" height="375" frameborder="0"
                                webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                        </div>
                    </div>
                </div>
            </asp:View>
            <asp:View ID="vUrduVideos" runat="server">
                <div id="videoPanel">
                    <div id="videoBox">
                        <div id="video">
                           <iframe src="http://player.vimeo.com/video/98729586" width="500" height="375" frameborder="0"
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
            <div align="left" id="txt" style="margin-left: 400px;">
                <p>
                    For Table making following software should be installed at PC.</p>
                <ul>
                    <li>Adobe Acrobat</li>
                    <li>Ms-Excel</li>
                </ul>
                <p>
                    1. Select and right click at the Table and choose &#8220;Open Table in Spreadsheet&#8221;.</p>
                <br />
                <br />
                <br />
                <img alt="" src="img/tablePic1.jpg" height="400" width="600" />
                 <br />
                <br />
                <br />
                <p>
                    2. Merge the Splitted Cells</p>
                <br />
                <br />
                <br />
                <img alt="" src="img/tablePic2.jpg" height="400" width="600" />
                 <br />
                <br />
                <br />
                <p>
                    3. Select and Delete the empty rows after merging the cells.
                </p>
                <br />
                <br />
                <br />
                <img alt="" src="img/tablePic3.jpg" height="400" width="600" />
                 <br />
                <br />
                <br />
                <p>
                    4. Highlight the Head Row or bold.
                </p>
                <br />
                <br />
                <br />
                <img alt="" src="img/tablePic4.jpg" height="400" width="600" />
                <br />
                <br />
                <br />
                <p>
                    5. Copy and paste the Table caption from Source file.
                </p>
                <br />
                <br />
                <br />
                <img alt="" src="img/tablePic5.jpg" height="400" width="600" />
                <br />
                <br />
                <br />
                <p>Table naming should be according to page no and table number. i.e table_1_1, table_1_2, table_2_3</p>
                <br />
                <br />
                <br />
                <br />
                <br />
                <br />
            </div>
        </div>
    </div>    
</asp:Content>


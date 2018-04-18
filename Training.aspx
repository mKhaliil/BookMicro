<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true"
    CodeBehind="Training.aspx.cs" Inherits="Outsourcing_System.Training" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
</asp:Content>
<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
    <div id="tests" style="margin-top: 6%; margin-bottom: 0px; margin-left: auto; margin-right: auto;
        height: 1550px;">
        <h1>
            <div id="DivError" runat="server" visible="false">
                <div style="font-size: 10pt; color: Green; text-align: left; background-color: #ffcdd6;
                    margin-left: 280px; margin-right: 270px; border: 1px solid red; padding: 5px;
                    font-family: Sans-Serif;">
                    <table>
                        <tr>
                            <td rowspan="2">
                                <img src="img/red-error.gif" />
                            </td>
                            <td>
                                <asp:Label ID="lblError" runat="server" CssClass="ErrorMsgText"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </div>
                <div class="spacer" style="padding-top: 2px">
                </div>
            </div>
            <div id="divSuccess" runat="server" visible="false">
                <div style="font-size: 10pt; color: Green; text-align: left; background-color: #B3FFB3;
                    margin-left: 290px; margin-right: 400px; border: 1px solid Green; padding: 5px;
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
            <asp:Button ID="btnGoToProfile" Style="margin-left: 20px; margin-top: -50px; float: right;
                border: 1px solid #2A4F96; background-color: #2A4F96; color: #FFFFFF; width: 100px;
                height: 35px;" runat="server" Text="Go To Profile" OnClick="btnGoToProfile_Click" />
            <strong>Welcome to BookMicro's Training Session!<br />
                Qualify below tests to get work. </strong>
        </h1>
        <h4>
            <asp:LinkButton ID="lnkTest" runat="server" Font-Size="X-Large" Text="Click here to have a sample Book."
                OnClick="lnkTest_Click"></asp:LinkButton><br />
        </h4>
        <div id="roundBox">
<%--            <h4>
                Comparison Tasks
            </h4>
            <p>
                <asp:ImageButton ID="ImageButton1" Width="150" Height="52" ImageUrl="img/Quiz.jpg"
                    OnClick="ibtnQuiz_Click" Style="margin-left: 85.2%" runat="server" /></p>
            <p style="float: right;">
                <asp:ImageButton ID="ibtnCompTask" ImageUrl="img/btn_startTest.jpg" OnClick="ibtnCompTask_Click"
                    Style="margin-right: 0px" runat="server" Enabled="false" />
                <p>
                    &nbsp;</p>
                <asp:ImageButton ID="ibtnCompTaskTraining" ImageUrl="img/btn_trainingVideos.jpg"
                    Style="clear: both; float: right;" runat="server" OnClick="ibtnCompTaskTraining_Click" />
            </p>
            <p>
                <strong>Skills Required: </strong>BookMicro</p>
            <p>
                This test assesses your skills of checking mistakes.
            </p>
            <p>
                <i><b>Duration:</b> Same day</i></p>
            <p>
                <i><b>Reward:</b> Rs. 500</i></p>
            <hr />--%>
            
                        <h4>
                Comparison Tasks
            </h4>
            <p style="float: right;">
                <asp:ImageButton ID="ImageButton1" Width="150" Height="52" ImageUrl="img/Quiz.jpg"
                    OnClick="ibtnQuiz_Click" runat="server" />

                <p>
                    &nbsp;</p>
                <asp:ImageButton ID="ibtnCompTaskTraining" ImageUrl="img/btn_trainingVideos.jpg"
                    Style="clear: both; float: right;" runat="server" OnClick="ibtnCompTaskTraining_Click" />
                    </p>
            <p>
                <strong>Skills Required: </strong>BookMicro</p>
            <p>
                This test assesses your skills of checking mistakes.
            </p>
            <p>
                <i><b>Duration:</b> Same day</i></p>
            <p>
                <i><b>Reward:</b> 
                <%--Rs. 500--%>
                <asp:Label ID="lblComparisonTaskReward" runat="server"></asp:Label>
                </i></p>

                

            <hr />
            <h4>
                Table Tasks
            </h4>
            <p style="float: right;">
                <asp:ImageButton ID="ibtnTable" ImageUrl="img/btn_startTest.jpg" PostBackUrl="TableTest.aspx"
                    runat="server" />
                <p>
                    &nbsp;</p>
                <asp:ImageButton ID="ibtnTableTraining" ImageUrl="img/btn_trainingVideos.jpg" OnClick="ibtnTableTraining_Click"
                    Style="clear: both; float: right;" runat="server" />
            </p>
            <p>
                <strong>Skills Required: </strong>MS Excel Beginner level
            </p>
            <p>
                This test assesses your knowledge about converting a table from pdf to excel sheet.
                <%-- The test is of 20 minutes. One attempt per day is allowed.--%>
            </p>
            <p>
                <i><b>Duration:</b> Same day</i></p>
            <p>
                <i><b>Reward:</b> 
                <%--Rs. 300--%>
                 <asp:Label ID="lblTableTaskReward" runat="server"></asp:Label>
                </i></p>
            <hr />
            <h4>
                Images Tasks</h4>
            <p style="float: right;">
                <asp:ImageButton ID="ibtnImages" ImageUrl="img/btn_startTest.jpg" PostBackUrl="ImageTest.aspx"
                    runat="server" />
                <p>
                    &nbsp;</p>
                <asp:ImageButton ID="ibtnImagesTraining" ImageUrl="img/btn_trainingVideos.jpg" OnClick="ibtnImagesTraining_Click"
                    Style="clear: both; float: right;" runat="server" />
            </p>
            <p>
                <strong>Skills Required: </strong>Photoshop CS5 Beginner level
            </p>
            <p>
                This test assesses your knowledge of resizing and cropping images from pdf.
                <%--The test is of 20 minutes to complete.--%>
                <%-- One attempt per week is allowed. --%>
                Result of test will be emailed to you.
            </p>
            <p>
                <i><b>Duration:</b> Same day</i></p>
            <p>
                <i><b>Reward:</b> 
                <%--Rs. 200--%>
                <asp:Label ID="lblImageTaskReward" runat="server"></asp:Label>
                </i></p>
            <hr />
            <h4>
                Indexing Tasks</h4>
            <p style="float: right;">
                <asp:ImageButton ID="ibtnIndexing" ImageUrl="img/btn_startTest.jpg" PostBackUrl="IndexTest.aspx"
                    runat="server" />
                <p>
                    &nbsp;</p>
                <asp:ImageButton ID="ibtnIndexingTraining" ImageUrl="img/btn_trainingVideos.jpg"
                    OnClick="ibtnIndexingTraining_Click" Style="clear: both; float: right;" runat="server" />
            </p>
            <p>
                <strong>Skills Required: </strong>MS Excel Beginner level</p>
            <p>
                This test assesses your knowledge of converting pdf index to excel.
                <%-- The test is
                of 30 mins. One attempt per week is allowed.--%>
            </p>
            <p>
                <i><b>Duration:</b> Same day</i></p>
            <p>
                <i><b>Reward:</b> 
              <%--  Rs. 500--%>
                <asp:Label ID="lblIndexTaskReward" runat="server"></asp:Label>
                </i></p>
            <hr />
            <h4>
                Mapping Tasks
            </h4>
            <p style="float: right;">
                <asp:ImageButton ID="ibtnMapping" ImageUrl="img/btn_startTest.jpg" OnClick="ibtnMapping_Click"
                    runat="server" />
                <p>
                    &nbsp;</p>
                <asp:ImageButton ID="ibtnMappingTraining" ImageUrl="img/btn_trainingVideos.jpg" Style="clear: both;
                    float: right;" runat="server" OnClick="ibtnMappingTraining_Click" />
            </p>
            <p>
                <strong>Skills Required: </strong>BookMicro</p>
            <p>
                This test assesses your skills of checking and editing mapped book.
                <%-- The test is
                of 60 minutes. One attempt per week is allowed.--%>
            </p>
            <p>
                <i><b>Duration:</b> Same day</i></p>
            <p>
                <i><b>Reward:</b> 
                <%--Rs. 3500--%>
                 <asp:Label ID="lblMappingTaskReward" runat="server"></asp:Label>
                </i></p>
            <%-- <hr />--%>
            <%-- <p>
                *Amount will be given if test is passed in first 2 attempts.</p>--%>
            <%--<h4>
                Proofreading Tasks</h4>
            <p style="float: right;">
                <asp:ImageButton ID="ibtnProofreading" ImageUrl="img/btn_startTest.jpg" runat="server" />
                <p>
                    &nbsp;</p>
                <asp:ImageButton ID="ibtnProofreadingTraining" ImageUrl="img/btn_trainingVideos.jpg"
                    Style="clear: both; float: right;" runat="server" />
            </p>
            <p>
                <strong>Skills Required: </strong>English Average
            </p>
            <p>
                This test assesses your skill of rectifying mistakes in the text by looking at the
                image source. This test is of 30 mins. One attempt per day is allowed.</p>
            <p>
                <i><b>Duration:</b> 30 minutes</i></p>
            <p>
                <i><b>Reward:</b> Rs. 500</i></p>--%>
        </div>
    </div>
</asp:Content>

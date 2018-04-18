<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true"
    CodeBehind="IndexingTutorials.aspx.cs" Inherits="Outsourcing_System.IndexingTutorials" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
        <div id="mid" align="center">
        <div style="float: right;">
            <a href="IndexingTutorials.aspx#tutorials">
                <img src="img/file-text.png" /></a><a href="IndexingTutorials.aspx#video"><img src="img/file-video.png" /></a></div>
        <h2>
            Indexing
        </h2>
        <div>
            <asp:LinkButton ID="lbtnTestTraining" OnClick="lbtnTestTraining_Click" runat="server"
                Style="margin-left: 566px; float: left; margin-top: -100px; color: #2A4F96; font-size: 16px;">Tests and Trainings</asp:LinkButton>
        </div>
        <div id="videoPanel">
            <div id="videoBox">
                <div id="video">
                    <iframe src="http://player.vimeo.com/video/98420230" width="500" height="375" frameborder="0"
                        webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                </div>
            </div>
        </div>
    </div>
    <p>
        &nbsp;</p>
    <div id="tutorials" align="center">
        <%-- <div>
             <a href="IndexingTutorials.aspx#Page1" onclick="this.parentNode.style.display = 'none'">Page 1</a>
             <a href="IndexingTutorials.aspx#Page2">Page 2</a>
             <a href="IndexingTutorials.aspx#Page3">Page 3</a>
             </div>--%>
        <div align="left" id="txt" style="margin-left: 150px;>
            <%-- <div id="Page1">--%>
            <p style="font-size:x-large">
                INDEX MAKING STEPS</p>
            <p>
                1. Extract Index into Spread Sheet.</p>
            <br />
            <br />
            <img alt="" src="img/IndexingPic1.JPG" height="400" width="600" />
            <br />
            <br />
            <p>
                <p>
                    <img alt="" src="img/IndexingPic2.JPG" height="400" width="600" /></p>
                <br />
                2. Split Keywords and page numbers. Using E indexer tool. <asp:LinkButton ID="lnlIndexTool" runat="server" Font-Size="X-Large" Text="Click here to get an executable file of Eindex tool." OnClick="lnlIndexTool_Click"></asp:LinkButton><br />
                <br />
                    <img alt="" src="img/IndexingPic3.JPG" height="400" width="600" />
                <br />
                <br />
                <%--  </div>
               <div id="Page2">--%>
                
            <br />
            <br />
            <img alt="" src="img/IndexingPic4.JPG" height="400" width="600" />
            <br />
            <br />
            <p>
                <img alt="" src="img/IndexingPic5.JPG" height="400" width="600" /></p>
            <br />
             <p>
                3. Right click at Column &#8216;C&#8217; Select Format cells, In Category Options
                Select Text.</p>
                 <br />
            <p>
                <img alt="" src="img/IndexingPic6.JPG" height="400" width="600" /></p>
            <br />
           <p>
                4. Check for invalid page numbers in Column C.</p>
            <p>
                Like 9-10 changed into 430074 and 5-8 changed into 50023.</p>
            <br />
            <img alt="" src="img/IndexingPic7.JPG" height="400" width="600" />
            
            <%--</div>
                 <div id="Page3">--%>
           
            <br />
            <p>
                5. Check Keywords for &#8216;See&#8217; and Separate those entries which are starting
                from See.</p>
            <p>
                6. Replace En Dashes into hyphen in column C.</p>
            <p>
                7. Check for Extra Spaces B/W Digits in Column C.</p>
            <p>
                8. Check the Page Range and Remove Pre- and Post-Section Page numbers and their
                interies.</p>
            <p>
                9. Check for (/) in keywords and fix them accordingly. Slash Interies will be deleted.
                But if slash intery is in Main level and it has also sub level under it, Then (/)<br />
                will be changed with (or).</p>
            <p>
                10. Remove entries containing URL (web addresses)</p>
            <p>
                11. Remove blank rows (CTRL+G, select Special, check Blanks click Ok and then delete
                all empty rows with CTRL + -)</p>
            <p>
                12. Insert Levels in Column A. Level will be differentiate with respect to their
                indentation. (means from where they start) Mention in below image.
            </p>
            <br />
            <p>
                <img alt="" src="img/IndexingPic8.JPG" height="400" width="600" /></p>
            <br />
            <p>
                NOTE:</p>
            <p>
                PLEASE MAKE SURE&#8230;</p>
            <ul style="list-style-type:upper-roman">
                <li>THERE IS NO EMPTY ROW PRESENT IN SHEET.</li>
                <li>ALL ENTRIES CONTAINING SLASHES HAVE BEEN FIXED ACCORDINGLY.</li>
                <li>THERE IS NO PUNCTUATION AT THE END OF KEYWORDS EXCEPT SINGLE CLOSING QUOTE OR FULL-STOP
                    WITH ABBREVIATION.</li>
                <li>THERE IS NO FULL-STOP, SEMI COLON, COLON AND SLASH IN COLUMN C.</li>
            </ul>
             <br />
    <br />
        </div>
       
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true" CodeBehind="ImgCroppingUrdu.aspx.cs" Inherits="Outsourcing_System.ImgCroppingUrdu" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
      <div id="mid" align="center">
        <div style="float: right;">
            <a href="ImgCroppingTutorial.aspx#tutorials">
                <img src="img/file-text.png" /></a><a href="ImgCroppingTutorial.aspx#video"><img src="img/file-video.png" /></a></div>
        <h2>
            IMAGE RESIZING AND CROPPING
        </h2>
        <div>
         <asp:LinkButton ID="LinkButton1" OnClick="lbtnTestTraining_Click" runat="server"
            Style="margin-left: 580px; float: left; margin-top: -128px; color: #2A4F96; font-size: 16px;">Image Trainings</asp:LinkButton>
        <%--<asp:LinkButton ID="lbtnTestTraining" OnClick="lbtnTestTraining_Click" runat="server"
            Style="margin-left: 1110px;float:left;margin-top:-68px; color: #0066CC; font-size: 16px;">Image Trainings</asp:LinkButton>--%>
    </div>
        <div id="videoPanel">
            <div id="videoBox">
                <div id="video">
                    <iframe src="http://player.vimeo.com/video/98729666" width="500" height="375" frameborder="0"
                        webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                </div>
            </div>
        </div>
        <div id="tutorials">
            <div align="left" id="txt" style="margin-left: 50px; font-weight: bold;margin-right:5px;">
                 <br />
        <br />
        <br />
                <p>
                    If you often have to resize images to set sizes and resolutions, the Crop tool comes
                    in very handy. I prefer to use the Crop tool, rather than the Image Size window,
                    to resize images for my Web pages.</p>
                <p>
                    For example, I often resize images to the following fixed dimensions:</p>
                <ul>
                    <li>Width: 300px</li>
                    <li>Height: 225px</li>
                    <li>Resolution: 72 pixels/inch</li>
                </ul>
                <p>
                    Sometimes, I need to resize to a fixed width and resolution, but I want the height
                    to be proportional to the original image. The Crop tool works great for these images,
                    too.</p>
                <ol>
                    <li>To use the Crop tool to resize an image:</li>
                    <li>Open the image you want to crop.</li>
                    <li>Click or press C to select the Crop tool.</li>
                    <li>In the options bar, set the crop options you want:</li>
                    <li>Select the No Restrictions aspect ratio.</li>
                    <li>Enter the width, height and resolution you want for the resized image. The aspect
                        ratio will change to Custom.<br />
                        Crop options</li>
                    <li>Click-and-hold where you want the top left corner of the crop to begin. Then, drag
                        to where you want the crop to end and release the mouse button. The crop box will
                        be constrained to the width/height proportions you specified. The cropped area will
                        be highlighted and the area outside the crop dimmed.</li>
                    <li>Click OK or press Enter to complete the crop.</li>
                </ol>
                <p>
                    To resize to a fixed width and a variable height, enter only the width and resolution
                    in the options bar. Leave the height blank. The resized image will have the same
                    relative proportions as the original.</p>
                <p>
                    Conversely, to resize to a fixed height and variable width, enter only the height
                    and resolution in the options bar. Leave the width blank. The resized image will
                    have the same relative proportions as the original.<br />
                </p>
                  <br />
        <br />
        <br />
            </div>
        </div>
    </div>
    <p>
        &nbsp;</p>
</asp:Content>

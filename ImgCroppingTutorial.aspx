<%@ Page Title="" Language="C#" MasterPageFile="UserMaster.Master" AutoEventWireup="true"
    CodeBehind="ImgCroppingTutorial.aspx.cs" Inherits="Outsourcing_System.ImgTutorialDetaisl" %>

<asp:Content ID="ContentHead" ContentPlaceHolderID="mainHeadContents" runat="server">
</asp:Content>

<asp:Content ID="ContentBody" ContentPlaceHolderID="mainBodyContents" runat="server">
        <div id="mid" align="center">
     <div style="margin-left: -750px; font-size: 16px;">
            <asp:Label ID="Label2" runat="server" Style="color: #2a4f96;" Text="Select Training Language:"></asp:Label>&nbsp;&nbsp;
            <asp:LinkButton ID="lbtnEnglish" OnClick="lblEnglish_Click" runat="server" Enabled="false">English</asp:LinkButton>
            &nbsp;
            <asp:LinkButton ID="lbtnUrdu" OnClick="lbtnUrdu_Click" runat="server" Enabled="false">Urdu</asp:LinkButton>
        </div>
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
        <asp:MultiView ID="mvTrainingVideos" runat="server" ActiveViewIndex="0">
            <asp:View ID="vEnglishVideos" runat="server">
        <div id="videoPanel">
            <div id="videoBox">
                <div id="video">
                    <iframe src="http://player.vimeo.com/video/98419255" width="500" height="375" frameborder="0"
                        webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                </div>
            </div>
        </div>
         </asp:View>
            <asp:View ID="vUrduVideos" runat="server">
            <div id="videoPanel">
            <div id="videoBox">
                <div id="video">
                    <iframe src="http://player.vimeo.com/video/98729666" width="500" height="375" frameborder="0"
                        webkitallowfullscreen mozallowfullscreen allowfullscreen></iframe>
                </div>
            </div>
        </div>
         </asp:View>
        </asp:MultiView>
       <%-- <div id="tutorials">
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
        </div>--%>
        <div id="tutorials">
            <br />
            <br />
            <div align="left" id="txt" style="margin-left: 130px; font-weight: normal; margin-right: 20px;">
                <br />
                <br />
                <br />
                <p style="font-weight: bolder">
                    We need to have white background for each image</p>
                <img alt="" src="img/titleImg1.JPG" height="400" width="600" />
                <p style="font-weight: bolder">
                    Continuous Images
                </p>
                <p>
                    When block of text referring to two different images.</p>
                <p>
                    Solution: We need to refer according to the image caption within text. We will place
                    the block of text as well as two images in single box in order to avoid flow of
                    images.
                </p>
                <img alt="" src="img/titleImg2.JPG" height="400" width="600" />
                <p style="font-weight: bolder">
                    Images before Start of the Chapter/ Image in the start of the Chapter</p>
                <p>
                    For both of the cases we place them after the chapter heading, this helps us to
                    restrict their typesetting flow within text.
                </p>
                <p style="font-weight: bolder">
                    Images on Images</p>
                <p>
                    We will rasterize the image in Photoshop. Crop that accordingly and label each image.
                </p>
                <img alt="" src="img/titleImg3.JPG" height="400" width="600" />
                <p style="font-weight: bolder">
                    Taking Box</p>
                <p>
                    Text present in the source which looks different from the normal text or appears
                    in a box like structure is taken in a box.</p>
                <p style="font-weight: bolder">
                    Images with and Without Border</p>
                <p>
                    Borders are not to be preferred.
                </p>
                <p>
                    However, if the image is based on design which requires a border then we will take
                    the border with image and will make sure it’s consistent with top, bottom, left
                    and right.</p>
                <p style="font-weight: bolder">
                    Logos</p>
                <p>
                    Images of Publisher’s logo or logo of any other Organization are to be placed where
                    it is in source with no caption and reference.</p>
                <p style="font-weight: bolder">
                    Tables to be taken as Image</p>
                <p>
                    We always prefer not to take tables as images because it lacks accessibility.</p>
                <p>
                    However, there are certain cases in which tables have lots of columns and it becomes
                    impossible to handle them in table – especially for super large formats.
                </p>
                <img alt="" src="img/TtitleImg4.JPG" height="400" width="600" />
                <p style="font-weight: bolder">
                    Complex format table</p>
                <p>
                    Table in which short text in one column relates to long text in some other column
                    (in the same row) then we take the table as image.</p>
                <img alt="" src="img/titleImg5.JPG" height="400" width="600" />
                <p>
                    <p style="font-weight: bolder">
                        Empty tables</p>
                    in the book are taken as images to avoid shrinking of table after production.</p>
                <img alt="" src="img/titleImg6.JPG" height="400" width="600" />
                <p>
                    11. Forms which have formatting complicated to achieve are to be taken as images</p>
                <p style="font-weight: bolder">
                    Placement of Images in the text</p>
                <p>
                    We have three scenarios for image placement</p>
                <p style="font-weight: bolder">
                    Images which don’t have any reference in the text.</p>
                <ul>
                    <li>Such images come at any place within text. So we can place them where they are coming
                        and their caption and reference will be given in preceding para.</li>
                    <li>Some times images come before the start of the chapter. In such case we will place
                        this image in the start of the chapter. This image will have caption but no reference.</li>
                    <li><p>Images which come at the start of a chapter or a level heading are to be placed
                        at their place as in source, their caption is to be inserted and if they have any
                        reference<br />in proceeding para then their reference is to be given at the end of that
                        para.</li>
                </ul>
                <p style="font-weight: bolder">
                    Images which are explained in the text in different para’s.</p>
                <ul>
                    <li>Those are the images which don’t have any specific definition in some specific para,
                        rather they have explanation in different para’s. Such images will be placed where<br />
                        they are coming in source and will have caption and reference in preceding or proceeding
                        para.</li>
                </ul>
                <p style="font-weight: bolder">
                    Images which are explained in a specific para.</p>
                <ul>
                    <li>These are the images which have their definition in some specific para. In such
                        case we will place those images after that specific para. Remember this will be
                        done only<br /> if the para having explanation of the image is near to that image, not
                        in the case where the definition is occurring after so many para’s.</li>
                </ul>
                <p style="font-weight: bolder">
                    Logos</p>
                <ul>
                    <li>Images of Publisher’s logo or logo of any other Organization are to be placed where
                        it is in source with no caption and reference.</li>
                </ul>
                <p style="font-weight: bolder; list-style-type: decimal;">
                    Steps</p>
                <ul>
                    <li>First Change the Resolution to: 300 pixels/inch (make sure "constrain proportion"
                        and "Resample Image - Bicubic" check box are checked)</li>
                    <li>If image has to be re-sized then adjust the 'Pixel Dimensions' values within</li>
                </ul>
                <p>
                    Image Size dimensions are:
                </p>
                <p style="font-weight: bolder">
                    Max Width = 1239 pixel</p>
                <p style="font-weight: bolder">
                    Max Height = 2066 pixel</p>
                <br />
                <p style="font-weight: bolder">
                    Max Width: 105mm</p>
                <p style="font-weight: bolder">
                    Max Height: 175mm</p>
                <br />
                <p style="font-weight: bolder">
                    Color Mode and Resolution</p>
                <p>
                    Resolution: 300</p>
                <p>
                    Color Mode: CMYK</p>
                <p style="font-weight: bolder">
                    *Please note that this rule is applicable to all images including Copyright Page
                    Image, Front & Back Images and all images inside the book.</p>
                <p style="font-weight: bolder">
                    Naming Convention for Images</p>
                <ol style="list-style-type: decimal;">
                    <li>For image titles use lower case letters separated by underscores.</li>
                    <li>Clearly mention the page number of the book to which the image belongs i.e. Like image_1_1, image_1_2, image_2_1.jpg</li>
                </ol>
                <p style="font-weight: bolder">
                    NO SPACE IN IMAGE NAME</p>
                <ol>
                    <li>No need to mention book name</li>
                </ol>
                <p style="font-weight: bolder">
                    Image caption and its reference (If Post-Section Have Refrences)</p>
                <p style="font-weight: bolder">
                    Image must have its caption
                </p>
                <ol style="list-style-type: decimal;">
                    <li>Images in pre-section should be named as "Figure A" or "Image A” depending on its
                        occurrence in the section. </li>
                    <li>Images in body are to be named as "Figure 1.1" or "Image 1.1" depending on its occurrence
                        in the chapter.</li>
                    <li>Images in post-section should be named as "Figure I" (roman numbers) or "Image I"
                        depending on its occurrence in the section.</li>
                </ol>
                <p style="font-weight: bolder">
                    Its reference in the para after which this image has to be placed same as its caption.</p>
                <br />
                <br />
                <br />
            </div>
        </div>
    </div>
    <p>
        &nbsp;</p>
</asp:Content>

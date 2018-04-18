using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using Outsourcing_System.CommonClasses;
using Outsourcing_System.PdfCompare_Classes;

namespace Outsourcing_System
{
    public partial class PdfImageUpload : Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        GlobalVar objGlobal = new GlobalVar();
        TableDetection tblDetectionObj = new TableDetection();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                List<string> missingImagesNameList = GetMissingImagePages();
                List<string> missingImgPageList = new List<string>();

                if (missingImagesNameList != null && missingImagesNameList.Count > 0 && !string.IsNullOrEmpty(Convert.ToString(Session["MainBook"])))
                {
                    string bookId = Convert.ToString(Session["MainBook"]);
                    string savingPath = Common.GetDirectoryPath();

                    if (CreatePdfPreviewDirectory(bookId, savingPath))
                    {
                        if (missingImagesNameList.Count > 0)
                        {
                            foreach (string imgName in missingImagesNameList)
                            {
                                if (imgName.Contains('_') &&
                                    !missingImgPageList.Contains(imgName.Trim().Split('_').Length < 1
                                        ? "0"
                                        : Convert.ToString(imgName.Trim().Split('_')[1])))
                                {
                                    missingImgPageList.Add(Convert.ToString(imgName.Trim().Split('_')[1]));
                                }
                            }
                        }

                        Session["MissingImagesNames"] = missingImagesNameList;
                        Session["ImagePdfTotalPageCount"] = Convert.ToString(missingImgPageList.Count);
                        Session["ImagePdfPages"] = missingImgPageList;

                        lblTotalPages.Text = Convert.ToString(missingImgPageList.Count);

                        if (string.IsNullOrEmpty(Convert.ToString(Session["CurrentPage"])))
                        {
                            Session["CurrentPage"] = 1;
                            lblPageNum.Text = "1";
                        }

                        int pageNum = 0;

                        if (string.IsNullOrEmpty(Convert.ToString(Session["ActualPdfPage"])))
                        {
                            Session["ActualPdfPage"] = missingImgPageList[0];
                            pageNum = Convert.ToInt32(missingImgPageList[0]);
                        }
                        else
                        {
                            lblPageNum.Text = Convert.ToString(Session["ActualPdfPage"]);
                            pageNum = Convert.ToInt32(Session["ActualPdfPage"]);
                        }

                        ShowPdf(pageNum);
                        ShowMissingImage(pageNum);
                    }
                }
            }
        }

        public void ShowPdf(int page)
        {
            if ((Convert.ToInt32(Session["CurrentPage"])) == Convert.ToInt32(Session["ImagePdfTotalPageCount"]))
                btnFinishTask.Visible = true;

            if (!string.IsNullOrEmpty(Convert.ToString(Session["MainBook"])))
            {
                string bookId = Convert.ToString(Session["MainBook"]);

                string mainDirectoryPath = Common.GetDirectoryPath();
                string pdfPreviewDirPath = mainDirectoryPath + "\\" + bookId + bookId + "-1\\Image\\PdfPreview";

                string mainPdfPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + ".pdf";
                string outputPdfPath = pdfPreviewDirPath + "\\" + page + ".pdf";

                if (!File.Exists(outputPdfPath))
                {
                    tblDetectionObj.ExtractPage(mainPdfPath, outputPdfPath, page);
                    Session["misingImgPdfPath"] = outputPdfPath;
                }
                else
                {
                    Session["misingImgPdfPath"] = outputPdfPath;
                }
            }
        }

        public void ShowMissingImage(int pageNum)
        {
            var missingImg = (List<string>)Session["MissingImagesNames"];
            List<string> pageImagesList = missingImg.Where(x => x.Trim().Split('_').Length > 1 && x.Trim().Split('_')[1]
                                                        .Equals(Convert.ToString(pageNum))).ToList();

            if (pageImagesList.Count > 0)
            {
                if (pageImagesList[0].Contains("_Zip"))
                {
                    divUploadMissingImg.Visible = true;
                    divUploadImg.Visible = true;
                    divUploadBtns.Visible = true;

                    divSelectImgLoc.Visible = false;
                    divMissingImgInXmlMsg.Visible = false;

                    lblUploadMissingImgMsg.Text = pageImagesList[0].Replace("_Zip", "");
                }
                else
                {
                    divSelectImgLoc.Visible = true;
                    divMissingImgInXmlMsg.Visible = true;

                    divUploadMissingImg.Visible = false;
                    divUploadImg.Visible = false;
                    divUploadBtns.Visible = false;

                    lblMissingImgInXmlMsg.Text = pageImagesList[0].Replace("_Xml", "");
                }
            }
        }

        protected void btnUploadMissingImg_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(fuPdf.PostedFile.FileName))
                {
                    string extension = Path.GetExtension(fuPdf.PostedFile.FileName);
                    string process = "Image";
                    string mainBook = Convert.ToString(Session["MainBook"]);
                    string uploadPath = objMyDBClass.MainDirPhyPath + "\\" + mainBook + "\\" + mainBook + "-1\\" + process + "\\" + mainBook;
                    string uploadedZipPath = objMyDBClass.MainDirPhyPath + "\\" + mainBook + "\\" + mainBook + "-1\\" +
                                             process + "\\" + mainBook + "\\" + Path.GetFileNameWithoutExtension(fuPdf.PostedFile.FileName) + extension;

                    if (process.ToLower().Contains("image"))
                    {
                        if (extension != ".jpg")
                        {
                            uploadPath = "";
                            ucShowMessage1.ShowMessage(MessageTypes.Info, "Please Upload an image");
                        }
                        else
                        {
                            if (!Directory.Exists(uploadPath))
                            {
                                ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured while uploading zip.");
                                return;
                            }

                            fuPdf.PostedFile.SaveAs(uploadedZipPath);

                            var missingImg = (List<string>)Session["MissingImagesNames"];
                            missingImg.Remove(Path.GetFileNameWithoutExtension(fuPdf.PostedFile.FileName) + "_Zip");
                            missingImg.Remove(Path.GetFileNameWithoutExtension(fuPdf.PostedFile.FileName) + "_Xml");

                            if (!string.IsNullOrEmpty(Convert.ToString(Session["ActualPdfPage"])))
                            {
                                int currentPage = Convert.ToInt32(Session["ActualPdfPage"]);

                                List<string> pageImagesList = missingImg.Where(x => x.Trim().Split('_').Length > 1 && x.Trim().Split('_')[1]
                                                                           .Equals(Convert.ToString(currentPage))).ToList();

                                if (pageImagesList.Count > 0)
                                    ShowMissingImage(currentPage);
                            }

                            btnNextPage_Click(this, null);
                        }
                    }
                }
                else
                {
                    ucShowMessage1.ShowMessage(MessageTypes.Error, "Please Select Process Relevant File to Upload.");
                    //this.Master.ShowMessageBox("Please Select Process Relevant File to Upload", "error");
                }
            }
            catch (Exception ex)
            {
                ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured while uploading image.");
            }
        }

        protected void btnNoImgUploadRequired_Click(object sender, EventArgs e)
        {
            string imageName = lblUploadMissingImgMsg.Text.Replace(" is missing in zip file", "");
            string extension = "";
            string mainBook = Convert.ToString(Session["MainBook"]);
            objGlobal.XMLPath = objMyDBClass.MainDirPhyPath + "\\" + mainBook + "\\" + mainBook + "-1\\TaggingUntagged\\" + mainBook + "-1.rhyw";
            objGlobal.LoadXml();

            XmlNodeList imagesPlaceHoldersList = objGlobal.PBPDocument.SelectNodes("//image");

            if (imagesPlaceHoldersList != null && imagesPlaceHoldersList.Count > 1)
            {
                List<XmlNode> matchedNodes = imagesPlaceHoldersList.Cast<XmlNode>().Where(x => Path.GetFileNameWithoutExtension(x.Attributes["image-url"].Value
                    .Replace("Resources\\", "")).Equals(imageName.Trim())).ToList();

                if (matchedNodes.Count == 1 && matchedNodes[0].ParentNode != null)
                {
                    matchedNodes[0].ParentNode.RemoveChild(matchedNodes[0]);

                    var missingImg = (List<string>)Session["MissingImagesNames"];
                    missingImg.Remove(imageName + "_Zip");
                    missingImg.Remove(imageName + "_Xml");

                    if (!string.IsNullOrEmpty(Convert.ToString(Session["ActualPdfPage"])))
                    {
                        int currentPage = Convert.ToInt32(Session["ActualPdfPage"]);

                        List<string> pageImagesList = missingImg.Where(x => x.Trim().Split('_').Length > 1 && x.Trim().Split('_')[1]
                                                                   .Equals(Convert.ToString(currentPage))).ToList();

                        if (pageImagesList.Count > 0)
                            ShowMissingImage(currentPage);
                        else
                            btnNextPage_Click(this, null);
                    }
                }
            }
        }

        protected void btnNoImgInXmlRequired_Click(object sender, EventArgs e)
        {
            string extension = "";
            string imageName = lblMissingImgInXmlMsg.Text.Replace(" is missing in xml file", "");
            var missingImg = (List<string>)Session["MissingImagesNames"];
            missingImg.Remove(imageName + "_Zip");
            missingImg.Remove(imageName + "_Xml");

            if (!string.IsNullOrEmpty(Convert.ToString(Session["ActualPdfPage"])))
            {
                int currentPage = Convert.ToInt32(Session["ActualPdfPage"]);

                List<string> pageImagesList = missingImg.Where(x => x.Trim().Split('_').Length > 1 && x.Trim().Split('_')[1]
                                                           .Equals(Convert.ToString(currentPage))).ToList();

                if (pageImagesList.Count > 0)
                    ShowMissingImage(currentPage);
                else
                    btnNextPage_Click(this, null);
            }
        }

        protected void btnSaveImage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hfImageSelectedLine.Value))
                SetImageTagInXml(hfImageSelectedLine.Value, rbtnlSelectImgLoc.SelectedIndex, "", "1");

            string imageName = lblMissingImgInXmlMsg.Text.Replace(" is missing in xml file", "");
            var missingImg = (List<string>)Session["MissingImagesNames"];
            missingImg.Remove(imageName + "_Zip");
            missingImg.Remove(imageName + "_Xml");

            if (!string.IsNullOrEmpty(Convert.ToString(Session["ActualPdfPage"])))
            {
                int currentPage = Convert.ToInt32(Session["ActualPdfPage"]);

                List<string> pageImagesList = missingImg.Where(x => x.Trim().Split('_').Length > 1 && x.Trim().Split('_')[1]
                                                           .Equals(Convert.ToString(currentPage))).ToList();

                if (pageImagesList.Count > 0)
                    ShowMissingImage(currentPage);
                else
                    btnNextPage_Click(this, null);
            }
        }

        public void SetImageTagInXml(string selectedLine, int rbtnlIndex, string imgName, string imgId)
        {
            string mainBook = Convert.ToString(Session["MainBook"]);
            objGlobal.XMLPath = objMyDBClass.MainDirPhyPath + "\\" + mainBook + "\\" + mainBook + "-1\\TaggingUntagged\\" + mainBook + "-1.rhyw";
            objGlobal.LoadXml();

            int page = Convert.ToInt32(Session["ActualPdfPage"]);

            XmlNodeList pageLinesList = objGlobal.PBPDocument.SelectNodes("//ln[@page='" + page + "']");

            if (pageLinesList != null && pageLinesList.Count > 1)
            {
                foreach (XmlNode line in pageLinesList)
                {
                    if (IsContainsSameText(line.InnerText.Trim(), selectedLine))
                    {
                        XmlElement imageNode = objGlobal.PBPDocument.CreateElement("image");
                        XmlAttribute attribId = objGlobal.PBPDocument.CreateAttribute("id");
                        XmlAttribute attribUrl = objGlobal.PBPDocument.CreateAttribute("image-url");
                        attribId.Value = imgId;
                        attribUrl.Value = imgName;
                        imageNode.Attributes.Append(attribId);
                        imageNode.Attributes.Append(attribUrl);

                        XmlElement lnElem = objGlobal.PBPDocument.CreateElement("ln");

                        string cood = GetImageCoord(line);

                        lnElem.SetAttribute("coord", cood);
                        lnElem.SetAttribute("page", Convert.ToString(page));
                        lnElem.SetAttribute("height", "");
                        lnElem.SetAttribute("font", "");
                        lnElem.SetAttribute("fontsize", "0");
                        lnElem.SetAttribute("error", "0");
                        lnElem.SetAttribute("isUserSigned", "0");
                        lnElem.SetAttribute("isEditted", "false");
                        lnElem.SetAttribute("ispreviewpassed", "false");

                        imageNode.AppendChild(lnElem);

                        //((XmlElement)imageNode).SetAttribute("id", imageNode.Attributes["id"].Value);
                        //((XmlElement)imageNode).SetAttribute("image-url", imageNode.Attributes["pnum"].Value);

                        if (line.ParentNode != null && line.ParentNode.ParentNode != null)
                        {
                            if (line.ParentNode.Name.Equals("upara") || line.ParentNode.Name.Equals("npara"))
                            {
                                if (rbtnlIndex == 0)
                                    line.ParentNode.ParentNode.InsertAfter(imageNode, line.ParentNode);
                                else
                                    line.ParentNode.ParentNode.InsertBefore(imageNode, line.ParentNode);
                            }
                            else if (line.ParentNode.ParentNode.ParentNode != null && line.ParentNode.ParentNode.Name.Equals("spara"))
                            {
                                if (rbtnlIndex == 0)
                                    line.ParentNode.ParentNode.ParentNode.InsertAfter(imageNode, line.ParentNode.ParentNode);
                                else
                                    line.ParentNode.ParentNode.ParentNode.InsertBefore(imageNode, line.ParentNode.ParentNode);
                            }
                        }
                    }
                }
            }
        }

        protected void btnFinishTask_Click(object sender, EventArgs e)
        {
            Response.Redirect("OnlineTestUser.aspx?ct=image", true);
        }

        protected void btnNextPage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["CurrentPage"])) &&
                !string.IsNullOrEmpty(Convert.ToString(Session["ImagePdfTotalPageCount"])))
            {
                tblDetectionObj.TableHeader = null;
                tblDetectionObj.TableCaption = null;

                //CurrentPageTableId = 0;

                int currentPage = Convert.ToInt32(Session["CurrentPage"]);
                int totalPages = Convert.ToInt32(Session["ImagePdfTotalPageCount"]);
                int pageNum = 0;

                List<string> tableContainingPages = (List<string>)Session["ImagePdfPages"];

                if (currentPage > tableContainingPages.Count) return;
                pageNum = Convert.ToInt32(tableContainingPages.ElementAt(currentPage - 1));
                //SaveChangesInXml(Convert.ToInt32(pageNum));

                currentPage = currentPage < totalPages ? ++currentPage : currentPage;

                if (currentPage > 1 && currentPage <= tableContainingPages.Count)
                {
                    Session["CurrentPage"] = currentPage;
                    pageNum = Convert.ToInt32(tableContainingPages.ElementAt(currentPage - 1));
                    Session["ActualPdfPage"] = pageNum;
                    Session["isNextClicked"] = "True";
                    //SaveChangesInXml(Convert.ToInt32(pageNum));

                    //cbxIgnoreAlgo.Checked = false;
                    Session["isIgnorAlgoChecked"] = false;

                    lblPageNum.Text = Convert.ToString(currentPage);

                }

                ShowPdf(pageNum);
                ShowMissingImage(pageNum);
            }
        }

        protected void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["CurrentPage"])))
            {
                tblDetectionObj.TableHeader = null;
                tblDetectionObj.TableCaption = null;

                //CurrentPageTableId = 0;
                int currentPage = Convert.ToInt32(Session["CurrentPage"]);
                currentPage = currentPage > 1 ? --currentPage : currentPage;

                List<string> tableContainingPages = (List<string>)Session["ImagePdfPages"];

                if (currentPage >= tableContainingPages.Count) return;

                Session["CurrentPage"] = currentPage;

                int pageNum = Convert.ToInt32(tableContainingPages.ElementAt(currentPage - 1));

                Session["ActualPdfPage"] = pageNum;

                //cbxIgnoreAlgo.Checked = false;
                Session["isIgnorAlgoChecked"] = false;

                lblPageNum.Text = Convert.ToString(currentPage);

                ////if (!cbxIgnoreAlgo.Checked)
                ////{
                //SetTableCountInUi(pageNum);
                //btnMarkTableLines_Click(this, null);
                ////}

                ShowPdf(pageNum);
                ShowMissingImage(pageNum);
            }
        }

        private bool CreatePdfPreviewDirectory(string bookId, string savingPath)
        {
            string pdfPreviewDirPath = savingPath + "\\" + bookId + bookId + "-1\\Image\\PdfPreview";

            if (Directory.Exists(pdfPreviewDirPath))
                return true;

            try
            {
                if (!Directory.Exists(pdfPreviewDirPath))
                    Directory.CreateDirectory(pdfPreviewDirPath);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        protected void lbtnLogOut_Click(object sender, System.EventArgs e)
        {
            Session.Clear();
            Response.Redirect("BookMicro.aspx", true);
        }

        protected void lbtnHome_Click(object sender, System.EventArgs e)
        {
            if (Session["LoginId"] != null)
            {
                Response.Redirect("SubmitTask.aspx");
            }
            else
            {
                Response.Redirect("BookMicro.aspx", true);
            }
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {

        }

        public List<string> GetMissingImagePages()
        {
            try
            {
                List<string> xmlImageNameList = new List<string>();
                List<string> missingImageNameList = new List<string>();

                if (!string.IsNullOrEmpty(Convert.ToString(Session["MainBook"])))
                {
                    string mainBook = Convert.ToString(Session["MainBook"]);

                    objGlobal.XMLPath = objMyDBClass.MainDirPhyPath + "\\" + mainBook + "\\" + mainBook + "-1\\TaggingUntagged\\" + mainBook + "-1.rhyw";
                    objGlobal.LoadXml();

                    //XmlNodeList imagesPlaceHoldersPageList = objGlobal.PBPDocument.SelectNodes("//image//preceding-sibling::upara/descendant::ln/@page");

                    XmlNodeList imagesPlaceHoldersList = objGlobal.PBPDocument.SelectNodes("//image");

                    foreach (XmlNode img in imagesPlaceHoldersList)
                    {
                        if (img.Attributes != null && img.Attributes["image-url"] != null && !img.Attributes["image-url"].Value.Contains("BookNotices"))
                        {
                            xmlImageNameList.Add(Path.GetFileNameWithoutExtension(Convert.ToString(img.Attributes["image-url"].Value).Replace("Resources\\", "")));
                        }
                    }

                    string extractedZipPath = objMyDBClass.MainDirPhyPath + "\\" + mainBook + "\\" + mainBook + "-1\\Image\\" + mainBook;

                    if (!string.IsNullOrEmpty(extractedZipPath))
                    {
                        List<string> uploadedImageFiles = Directory.GetFiles(extractedZipPath, "*.jpg").ToList()
                            .Select(x => Path.GetFileNameWithoutExtension(x)).ToList();

                        if (uploadedImageFiles.Count > 0)
                        {
                            //missingImageNameList = xmlImageNameList.Except(uploadedImageFiles).Union(uploadedImageFiles.Except(xmlImageNameList)).ToList();

                            var test = xmlImageNameList.Except(uploadedImageFiles).Union(uploadedImageFiles.Except(xmlImageNameList)).ToList();

                            var missingImageInZip = xmlImageNameList.Except(uploadedImageFiles).ToList();

                            for (int i = 0; i < missingImageInZip.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(missingImageInZip[i]))
                                {
                                    missingImageInZip[i] = missingImageInZip[i] + "_Zip";
                                }
                            }

                            var missingImageInXml = uploadedImageFiles.Except(xmlImageNameList).ToList();

                            for (int i = 0; i < missingImageInXml.Count; i++)
                            {
                                if (!string.IsNullOrEmpty(missingImageInXml[i]))
                                {
                                    missingImageInXml[i] = missingImageInXml[i] + "_Xml";
                                }
                            }

                            missingImageNameList.AddRange(missingImageInZip);
                            missingImageNameList.AddRange(missingImageInXml);
                        }
                    }
                }

                return missingImageNameList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public string GetImageCoord(XmlNode matchedLine)
        {
            if (matchedLine.ParentNode != null && matchedLine.ParentNode.ChildNodes.Count > 1)
            {

            }
            return "";
        }

        public bool IsContainsSameText(string xmlText, string selectedText)
        {
            xmlText = xmlText.Trim().Replace("…", "").Replace(".", "");
            selectedText = selectedText.Trim().Replace("…", "").Replace(".", "");

            var splittedXmlText = Regex.Split(xmlText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToArray();
            var splittedSelectedText = Regex.Split(selectedText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (splittedXmlText != null && splittedSelectedText != null)
            {
                if (splittedXmlText.Length > 0 && splittedSelectedText.Length > 0)
                {
                    if (splittedXmlText.Length == 1 && splittedSelectedText.Length == 1)
                    {
                        if (splittedXmlText[0].Equals(splittedSelectedText[0]))
                            return true;
                    }
                    else
                    {
                        if (CheckSameLenWords(splittedXmlText, splittedSelectedText))
                        {
                            //if (splittedXmlText.Length == 1 && splittedSelectedText.Length == 1)
                            //{
                            //    if (splittedXmlText[0].Equals(splittedSelectedText[0]))
                            //        return true;
                            //}
                            if (splittedXmlText.Length == 2 && splittedSelectedText.Length == 2)
                            {
                                if (splittedXmlText[0].Equals(splittedSelectedText[0]))
                                {
                                    if (splittedXmlText[1].Equals(splittedSelectedText[1]))
                                        return true;
                                }
                            }
                            else if (splittedXmlText.Length > 2 && splittedSelectedText.Length > 2)
                            {
                                if (splittedXmlText[0].Equals(splittedSelectedText[0]))
                                {
                                    if (splittedXmlText[1].Equals(splittedSelectedText[1]))
                                    {
                                        if (splittedXmlText[2].Equals(splittedSelectedText[2]))
                                            return true;
                                    }
                                }
                            }
                        }
                        else
                        {
                            string xmlTextWithoutSpace = RemoveWhiteSpace(xmlText);
                            string selectedTextWithoutSpace = RemoveWhiteSpace(selectedText);

                            int xmlTextLength = xmlTextWithoutSpace.Length;
                            int pdfJsTextLength = selectedTextWithoutSpace.Length;

                            if (xmlTextLength < 2 || pdfJsTextLength < 3)
                                return false;

                            StringBuilder sbWords = new StringBuilder();

                            if (xmlTextLength == pdfJsTextLength)
                            {
                                if (xmlTextWithoutSpace.Equals(selectedTextWithoutSpace))
                                    return true;
                            }
                            else if (xmlTextLength < pdfJsTextLength)
                            {
                                sbWords.Append(selectedTextWithoutSpace.Substring(0, xmlTextLength));
                                if (xmlTextWithoutSpace.Equals(RemoveWhiteSpace(Convert.ToString(sbWords))))
                                    return true;
                            }
                            else if (pdfJsTextLength < xmlTextLength)
                            {
                                sbWords.Append(xmlTextWithoutSpace.Substring(0, pdfJsTextLength));
                                if (selectedTextWithoutSpace.Equals(RemoveWhiteSpace(Convert.ToString(sbWords))))
                                    return true;
                            }
                        }
                    }
                }
            }
            return false;
        }

        public string RemoveWhiteSpace(string input)
        {
            StringBuilder output = new StringBuilder(input.Length);

            for (int index = 0; index < input.Length; index++)
            {
                if (!Char.IsWhiteSpace(input, index))
                {
                    output.Append(input[index]);
                }
            }

            return output.ToString();
        }

        public bool CheckSameLenWords(string[] xmlText, string[] pdfJsText)
        {
            int xmlTextArrayLen = xmlText.Length;
            int pdfJsArrayLen = pdfJsText.Length;

            if (xmlTextArrayLen != pdfJsArrayLen)
                return false;

            for (int i = 0; i < xmlTextArrayLen; i++)
            {
                if (xmlText[i].Length != pdfJsText[i].Length)
                    return false;
            }

            return true;
        }

        public XmlElement CreateImageTag(string strID, string Url, XmlDocument xmlDoc, string caption)
        {
            XmlElement NodeImage = xmlDoc.CreateElement("image");
            XmlAttribute AttribId = xmlDoc.CreateAttribute("id");
            XmlAttribute AttribUrl = xmlDoc.CreateAttribute("image-url");
            AttribId.Value = strID;
            AttribUrl.Value = Url;
            NodeImage.Attributes.Append(AttribId);
            NodeImage.Attributes.Append(AttribUrl);
            if (caption.Trim() != string.Empty)
            {
                XmlElement NodeCation = xmlDoc.CreateElement("caption");
                NodeCation.InnerText = caption;
                NodeImage.AppendChild(NodeCation);
            }
            return NodeImage;
        }
    }
}
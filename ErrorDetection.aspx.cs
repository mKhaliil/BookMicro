using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Web;
using System.IO;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using System.Configuration;
using System.Xml;
using System.Xml.Linq;
using iTextSharp.text;
using System.Text;
using iTextSharp.text.pdf;
using System.Drawing;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf.parser;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;
using System.Drawing.Imaging;
using Outsourcing_System.PdfCompare_Classes;

namespace Outsourcing_System
{
    public partial class ErrorDetection : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();

        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            //Session["PdfPath"] = @"D:\Office Data\Files_Dev\31671\31671-1\Comparison\Comparison-1\135\Produced_4.pdf";
            //trNavigateMistake.Visible = false;Session["email"]

            //if (!string.IsNullOrEmpty(Convert.ToString(Session["largerPrdPdfCount"])))
            //{
            //    btnNext.Visible = true;
            //    btnPrevious.Visible = true;
            //}
            //else
            //{
            //    btnNext.Visible = false;
            //    btnPrevious.Visible = false;
            //}

            ibtnPrevious.Visible = false;

            if (!string.IsNullOrEmpty(Convert.ToString(Session["ComparisonTask"])))
            {
                if (Convert.ToString(Session["ProducePdfType"]).Equals("SubPdf"))
                    btnNextPage.Enabled = false;
                else btnNextPage.Enabled = true;
            }

            if (!Page.IsPostBack)
            {
                hfBookId.Value = Convert.ToString(Session["MainBook"]);

                if (Convert.ToString(Session["email"]) == "")
                {
                    Response.Redirect("BookMicro.aspx", true);
                }

                if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
                {
                    hfComparisonTaskType.Value = "onepagetest";
                    //trNavigateMistake.Visible = false;

                    //trComparisonTimeSpent.Visible = false;
                    trComparisonTimeSpent.Attributes.Add("style", "display:none");

                    trNavigateMistake.Visible = false;
                    trcheckBoxes.Visible = false;
                    //btnFinish_Task.Visible = false;
                    divFinishQuiz.Visible = true;
                    trTimer.Visible = true;

                    if (Convert.ToString(Session["TimeUpdator_StartTime"]) == "")
                    {
                        timehdnmin.Value = "2";
                        timehdnsec.Value = "0";

                        Session["TimeUpdator_StartTime"] = DateTime.Now;
                        Session["TimeUpdator_EndTime"] = DateTime.Now.AddMinutes(2);
                    }
                    else
                    {
                        System.TimeSpan difference =
                            Convert.ToDateTime(Session["TimeUpdator_EndTime"]).Subtract(DateTime.Now);
                        timehdnmin.Value = Convert.ToString(difference.Minutes);
                        timehdnsec.Value = Convert.ToString(difference.Seconds);
                    }

                    Timer();
                }

                else if (Convert.ToString(Session["ComparisonTask"]).Equals("task"))
                {
                    if ((Session["LoginId"] == null) || (Convert.ToString(Session["LoginId"]) == ""))
                    {
                        Response.Redirect("BookMicro.aspx", true);
                    }

                    trTimer.Visible = false;
                    trComparisonTimeSpent.Visible = true;
                    ShowComparisonTime();

                    foreach (System.Web.UI.WebControls.ListItem li in cbxlFont.Items)
                    {
                        li.Selected = true;
                    }
                }

                else if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
                {
                    trComparisonTimeSpent.Attributes.Add("style", "display:none");
                    trTimer.Visible = false;
                    trNavigateMistake.Visible = false;
                    trcheckBoxes.Visible = false;
                }

                if (Convert.ToString(Session["ComparisonTask"]).Equals("comparisonEntryTest"))
                {
                    hfComparisonTaskType.Value = "comparisonEntryTest";
                    //trNavigateMistake.Visible = false;

                    //trComparisonTimeSpent.Visible = false;
                    trComparisonTimeSpent.Attributes.Add("style", "display:none");

                    trNavigateMistake.Visible = false;
                    trcheckBoxes.Visible = false;
                    //btnFinish_Task.Visible = true;
                    divFinishQuiz.Visible = false;
                    trTimer.Visible = true;

                    foreach (System.Web.UI.WebControls.ListItem li in cbxlFont.Items)
                    {
                        li.Selected = true;
                    }

                    if (Convert.ToString(Session["TimeUpdator_StartTime"]) == "")
                    {
                        timehdnmin.Value = "5";
                        timehdnsec.Value = "0";

                        Session["TimeUpdator_StartTime"] = DateTime.Now;
                        Session["TimeUpdator_EndTime"] = DateTime.Now.AddMinutes(5);
                    }
                    else
                    {
                        System.TimeSpan difference = Convert.ToDateTime(Session["TimeUpdator_EndTime"]).Subtract(DateTime.Now);
                        timehdnmin.Value = Convert.ToString(difference.Minutes);
                        timehdnsec.Value = Convert.ToString(difference.Seconds);
                    }

                    Timer();
                }
                else if (Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedSampleTest"))
                {
                    hfComparisonTaskType.Value = "CompUpgradedSampleTest";
                    //trNavigateMistake.Visible = false;

                    //trComparisonTimeSpent.Visible = false;
                    trComparisonTimeSpent.Attributes.Add("style", "display:none");

                    trNavigateMistake.Visible = false;
                    trcheckBoxes.Visible = false;
                    //btnFinish_Task.Visible = true;
                    divFinishQuiz.Visible = false;
                    trTimer.Visible = true;

                    foreach (System.Web.UI.WebControls.ListItem li in cbxlFont.Items)
                    {
                        li.Selected = true;
                    }

                    if (Convert.ToString(Session["TimeUpdator_StartTime"]) == "")
                    {
                        timehdnmin.Value = "10";
                        timehdnsec.Value = "0";

                        Session["TimeUpdator_StartTime"] = DateTime.Now;
                        Session["TimeUpdator_EndTime"] = DateTime.Now.AddMinutes(10);
                    }
                    else
                    {
                        System.TimeSpan difference = Convert.ToDateTime(Session["TimeUpdator_EndTime"]).Subtract(DateTime.Now);
                        timehdnmin.Value = Convert.ToString(difference.Minutes);
                        timehdnsec.Value = Convert.ToString(difference.Seconds);
                    }

                    Timer();
                }
                else if (Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedStartTest"))
                {
                    hfComparisonTaskType.Value = "CompUpgradedStartTest";
                    //trNavigateMistake.Visible = false;

                    //trComparisonTimeSpent.Visible = false;
                    trComparisonTimeSpent.Attributes.Add("style", "display:none");

                    trNavigateMistake.Visible = false;
                    trcheckBoxes.Visible = false;
                    //btnFinish_Task.Visible = true;
                    divFinishQuiz.Visible = false;
                    trTimer.Visible = true;

                    foreach (System.Web.UI.WebControls.ListItem li in cbxlFont.Items)
                    {
                        li.Selected = true;
                    }

                    if (Convert.ToString(Session["TimeUpdator_StartTime"]) == "")
                    {
                        timehdnmin.Value = "10";
                        timehdnsec.Value = "0";

                        Session["TimeUpdator_StartTime"] = DateTime.Now;
                        Session["TimeUpdator_EndTime"] = DateTime.Now.AddMinutes(10);
                    }
                    else
                    {
                        System.TimeSpan difference = Convert.ToDateTime(Session["TimeUpdator_EndTime"]).Subtract(DateTime.Now);
                        timehdnmin.Value = Convert.ToString(difference.Minutes);
                        timehdnsec.Value = Convert.ToString(difference.Seconds);
                    }

                    Timer();
                }

                int currentPage = Convert.ToInt32(Session["MainCurrPage"]);

                if (currentPage > 0)
                    LoadNewPageInControl(currentPage);
            }
            else
            {
                if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
                {
                    Timer();
                }
                else if (Convert.ToString(Session["ComparisonTask"]).Equals("comparisonEntryTest") ||
                    Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedSampleTest") ||
                    Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedStartTest"))
                {
                    Timer();
                }
            }
        }

        protected void ibtnNext_Click(object sender, EventArgs e)
        {
            int totalPages = Convert.ToInt32(Session["srcTotalPages"]);

            int pNum = -1;
            int pageNum = 0;

            if (int.TryParse(this.txtPageNum.Text.Trim(), out pNum))
            {
                pageNum = pNum;

                pageNum = pageNum < totalPages ? (pageNum + 1) : totalPages;

                //this.CtrlPdfCmp.LoadTableText();

                //foreach (System.Web.UI.WebControls.ListItem li in cbxlFont.Items)
                //{
                //    li.Selected = true;
                //}

                //if (Convert.ToString(Session["ComparisonTest"]).Equals("1"))
                //{
                //    string pDirPath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
                //    string userDir_Path = pDirPath + "\\Tests\\" + Convert.ToString(Session["CompTestUser_Email"]) + "/ComparisonTests/";

                //    Session["srcPdfPagePath"] = userDir_Path + "/" + PageNum + ".pdf";
                //    Session["prdPdfPagePath"] = userDir_Path + "/" + "Produced_" + PageNum + ".pdf";
                //}
                //else
                //{

                //    Session["srcPdfPagePath"] = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"])) + "\\" + PageNum + ".pdf";
                //    Session["prdPdfPagePath"] = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"])) + "\\" + "Produced_" + PageNum + ".pdf";
                //}
                //setDefaultFont(PageNum);

                hfPageParasType.Value = "";
                Session["MainCurrPage"] = pageNum;
                LoadNewPageInControl(pageNum);

                //PDFManipulation pdfMan = new PDFManipulation(Convert.ToString(Session["srcPdfPagePath"]));

                //string mistakeNum = pdfMan.GetMistakeByPage(pageNum);

                //if (mistakeNum != null)
                //    TextErrorNum.Text = mistakeNum;
            }
            else
            {
                //Invalid
            }
        }

        protected void ibtnPrev_Click(object sender, EventArgs e)
        {
            int totalPages = Convert.ToInt32(Session["srcTotalPages"]);

            int pNum = -1;
            int pageNum = 0;

            if (int.TryParse(this.txtPageNum.Text.Trim(), out pNum))
            {
                pageNum = pNum;

                pageNum = pageNum > 1 ? (pageNum - 1) : 1;

                //SiteSession.MainCurrPage = PageNum;
                //this.CtrlPdfCmp.LoadTableText();

                //foreach (System.Web.UI.WebControls.ListItem li in cbxlFont.Items)
                //{
                //    li.Selected = true;
                //}

                //if (Convert.ToString(Session["ComparisonTest"]).Equals("1"))
                //{
                //    string pDirPath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
                //    string userDir_Path = pDirPath + "\\Tests\\" + Convert.ToString(Session["CompTestUser_Email"]) + "/ComparisonTests/";

                //    Session["srcPdfPagePath"] = userDir_Path + "/" + PageNum + ".pdf";
                //    Session["prdPdfPagePath"] = userDir_Path + "/" + "Produced_" + PageNum + ".pdf";
                //}
                //else
                //{
                //    Session["srcPdfPagePath"] = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"])) + "\\" + PageNum + ".pdf";
                //    Session["prdPdfPagePath"] = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"])) + "\\" + "Produced_" + PageNum + ".pdf";
                //}

                //setDefaultFont(PageNum);

                hfPageParasType.Value = "";
                Session["MainCurrPage"] = pageNum;
                LoadNewPageInControl(pageNum);

                //PDFManipulation pdfMan = new PDFManipulation(Convert.ToString(Session["srcPdfPagePath"]));

                //string mistakeNum = pdfMan.GetMistakeByPage(pageNum);

                //if (mistakeNum != null)
                //    TextErrorNum.Text = mistakeNum;
            }
            else
            {
                //Invalid
            }
        }

        protected void btnGoToError_Click(object sender, EventArgs e)
        {
            int pageNum;

            if (int.TryParse(TextErrorNum.Text.Trim(), out pageNum))
            {
                int mistakeNum = Convert.ToInt32(pageNum);
                if (mistakeNum > 0)
                {
                    PDFManipulation c = new PDFManipulation("");

                    List<Mistakes> list = c.GetTotalMistakes_List(Convert.ToString(Session["MainXMLFilePath"]));
                    int pdfPage = 0;

                    if ((mistakeNum - 1) < list.Count && mistakeNum > 0)
                    {
                        Mistakes item = list.ElementAt(mistakeNum - 1);
                        pdfPage = item.page;

                        //foreach (var item in list)
                        //{
                        //    if (item.mistakeNum == Convert.ToInt32(TextErrorNum.Text.Trim()))
                        //    {
                        //        pdfPage = item.page;
                        //    }
                        //}

                        if (!(pdfPage > 0))
                            return;

                        int eNum = -1;
                        if (int.TryParse(this.TextErrorNum.Text.Trim(), out eNum))
                        {
                            Session["MainCurrPage"] = pdfPage;
                            LoadNewPageInControl(pdfPage);
                        }
                        else
                        {
                            //Invalid
                        }
                    }
                }
            }
        }

        protected void btnGoTo_Click(object sender, EventArgs e)
        {
            int pNum = -1;
            int pageNum = 0;
            if (int.TryParse(txtPageNum.Text.Trim(), out pNum))
            {
                pageNum = pNum;

                //if (Convert.ToString(Session["ComparisonTest"]).Equals("1"))
                //{
                //    string pDirPath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
                //    string userDir_Path = pDirPath + "\\Tests\\" + Convert.ToString(Session["CompTestUser_Email"]) + "/ComparisonTests/";

                //    Session["srcPdfPagePath"] = userDir_Path + "/" + PageNum + ".pdf";
                //    Session["prdPdfPagePath"] = userDir_Path + "/" + "Produced_" + PageNum + ".pdf";
                //}
                //else
                //{
                //    Session["srcPdfPagePath"] = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"])) + "\\" + +PageNum + ".pdf";
                //    Session["prdPdfPagePath"] = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"])) + "\\" + "Produced_" + PageNum + ".pdf";
                //}

                //this.CtrlPdfCmp.LoadTableText();
                //setDefaultFont(PageNum);

                Session["MainCurrPage"] = pageNum;

                string srcPdfPath = Common.GetPdfPath(pageNum, "src");
                //CtrlPdfCmp.ShowSrcPDFAsImageInControl(srcPdfPath.Replace(".pdf", "_Stamped.pdf.jpeg"), Convert.ToString(pageNum));
                //CtrlPdfCmp.ShowPrdPDFInJSControl(Convert.ToString(pageNum));

                GetSrcPageMistakes(pageNum, srcPdfPath);

                LoadNewPageInControl(pageNum);

                //PDFManipulation pdfMan = new PDFManipulation(Convert.ToString(Session["srcPdfPagePath"]));

                //string mistakeNum = pdfMan.GetMistakeByPage(PageNum);

                //if (mistakeNum != null)
                //    TextErrorNum.Text = mistakeNum;
            }
            else
            {
                //Invalid
            }
        }

        protected void btnFinish_Task_Click(object sender, EventArgs e)
        {
            MyDBClass dbObj = new MyDBClass();
            int finishBtnClickedCount = dbObj.GetFinishTaskClickedCount(Convert.ToString(Session["MainBook"]));

            if (finishBtnClickedCount == 3)
            {
                string strStatus = "true";
                Page.RegisterStartupScript("Disable", "<script language=JavaScript>document.getElementById('btnFinish_Task').disabled = " + strStatus + ";</script>");
                return;
            }

            if (!string.IsNullOrEmpty(Convert.ToString(Session["MainBook"])) && finishBtnClickedCount < 3)
            {
                finishBtnClickedCount++;
                string status = dbObj.InsertFinishTaskClickCount(Convert.ToString(Session["MainBook"]), finishBtnClickedCount);
            }

            //int mistakeCorrection = 0;
            string mainXmlPath = Convert.ToString(Session["MainXMLFilePath"]);

            if (String.IsNullOrEmpty(mainXmlPath))
                return;

            if ((Convert.ToString(Session["ComparisonTask"]).Equals("test")) || (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest")))
            {
                FinishTest(mainXmlPath);
            }
            else if ((Convert.ToString(Session["ComparisonTask"]).Equals("comparisonEntryTest"))||
                (Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedSampleTest")) ||
                (Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedStartTest")))
            {
                FinishComparisonEntryTest(mainXmlPath);
            }
            else
            {
                //StreamReader sr = new StreamReader(mainXmlPath);
                //string xmlFile = sr.ReadToEnd();
                //sr.Close();
                //XmlDocument xmlDocOrigXml = new XmlDocument();
                //xmlDocOrigXml.LoadXml(xmlFile);
                //string totalMistakes = xmlDocOrigXml.SelectNodes("//ln[@autoInjection]").Count.ToString();

                //string bookId = Convert.ToString(Session["BookId"]) + "-1";

                ////Set status of complete task
                ////First user completes then status = 1 and when second user completes status = 2
                //MyDBClass db = new MyDBClass();

                //string status = db.CompleteMistakeInj_Task(bookId, Convert.ToInt32(Session["LoginId"]));

                //////Check detected errors count
                ////double injectedMistakes = Convert.ToDouble(Session["InjectedMistakesCount"]);

                //////Get mistake pages from xml 1
                ////string pDirPath = ConfigurationManager.AppSettings["MainDirPhyPath"];
                ////string xmlPath = pDirPath + bookId.Replace("-1", "") + "\\" + bookId + "\\Comparison\\Comparison-" + Convert.ToString(Session["comparisonType"]) + "\\" +
                ////                 Convert.ToString(Session["LoginId"]) + "\\" + bookId + ".xml";

                ////XmlDocument xmlDoc_Xml1 = Common.LoadXmlDocument(xmlPath);
                ////double mistakes_Xml1 = xmlDoc_Xml1.SelectNodes(@"//ln[@PDFmistake and (@correction='' or @conversion ='')]").Count;

                ////if (injectedMistakes == mistakes_Xml1)
                ////{
                ////    if (status.Equals("2"))
                ////    {
                ////        //When status = 2 compare two xmls and create a new xml which contains mistakes from both xmls
                ////        CompareTwoXml(Convert.ToString(Session["BookId"]), Convert.ToString(Session["LoginId"]));
                ////    }

                ////    //Change status of task to pending confirmation
                ////    finishTask(bookId, Convert.ToString(Session["LoginId"]));

                ////    mainXmlPath = Convert.ToString(Session["MainXMLFilePath"]);
                ////    if (String.IsNullOrEmpty(mainXmlPath))
                ////        return;

                ////    sr = new StreamReader(mainXmlPath);
                ////    xmlFile = sr.ReadToEnd();
                ////    sr.Close();
                ////    xmlDocOrigXml = new XmlDocument();
                ////    xmlDocOrigXml.LoadXml(xmlFile);
                ////    string remainingMistakes = xmlDocOrigXml.SelectNodes("//ln[@autoInjection]").Count.ToString();
                ////    int obtainedScore = Convert.ToInt32(totalMistakes) - Convert.ToInt32(remainingMistakes);
                ////}
                //else
                //{
                //    //Delete comparison task and set status to working
                //    //Response.Redirect("http://localhost:30074/OnlineTestUser.aspx?UserId=" + Convert.ToString(Session["userId"]) +
                //    //              "&bid=" + Convert.ToString(Session["test_Id"]) + "&ct=" + Session["comparisonType"], true);

                //    Response.Redirect(string.Format("http://{0}:91/BMicroHiring/OnlineTestUser.aspx?UserId={0}&bid={1}&ct={2}", Convert.ToString(Session["LoginId"]), Convert.ToString(Session["test_Id"]), Convert.ToString(Session["comparisonType"])), true);
                //}

                ////GlobalVar objGlobal = new GlobalVar();
                ////objGlobal.PBPDocument = xmlDocOrigXml;
                ////objGlobal.XMLPath = mainXmlPath;
                ////objGlobal.SaveXml();

                Response.Redirect(string.Format("OnlineTestUser.aspx?bid={0}&ct={1}", Convert.ToString(Session["BookId"]), Convert.ToString(Session["comparisonType"])), true);
            }
        }

        protected void btnViewAnswer_Click(object sender, EventArgs e)
        {
            string xmlPath = Convert.ToString(Session["MainXMLFilePath"]);
            int currentPage = Convert.ToInt32(Session["MainCurrPage"]);
            string quizType = Convert.ToString(Session["quizType"]);
            string mistakeAttribute = "";

            if (Convert.ToString(Session["quizType"]) != "")
            {
                if (quizType.Equals("Splitting"))
                {
                    mistakeAttribute = @"//ln[@PDFMergemistake ='merge,']";
                }
                else if (quizType.Equals("Merging"))
                {
                    mistakeAttribute = @"//ln[@PDFSplitmistake ='split,']";
                }
                else if (quizType.Equals("Space"))
                {
                    mistakeAttribute = @"//ln[@PDFmistake !='']";
                }
            }

            Common comObj = new Common();
            var listMistakes = comObj.LoadXmlDocument(xmlPath).
                                 SelectNodes(mistakeAttribute).Cast<XmlNode>().
                                 Select(node => Convert.ToString(node.InnerText)).ToList();

            List<string> originalText = new List<string>();

            foreach (string text in listMistakes)
            {
                originalText.Add(text);
            }

            //To do aamir 2016-05-18
            //Common.LoadMistakePanel(Convert.ToString(currentPage), xmlPath, originalText, "producedPdf");
        }

        protected void btnCloseDialog_Click(object sender, EventArgs e)
        {
            Response.Redirect("OnlineTestUser.aspx");
        }

        #endregion

        #region Public Methods

        public void ShowComparisonTime()
        {
            string bookId = Convert.ToString(Session["BookId"]);

            if (bookId != "")
            {
                string timeSpent = objMyDBClass.GetComparisonTimeByBookId(bookId);

                timeSpent = timeSpent == "" ? "0" : timeSpent;

                if (timeSpent.Equals("0"))
                {
                    trComparisonTimeSpent.Attributes.Add("style", "display:none");
                }
                else
                {
                    double totalHours = Convert.ToDouble(timeSpent);
                    var timeSpan = TimeSpan.FromHours(totalHours);
                    int hour = timeSpan.Hours;
                    int minute = timeSpan.Minutes;

                    hfComparisonTimeSpent.Value = hour + " hour " + minute + " min";
                    Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "ShowComparisonTimeSpent()", true);
                }
            }
        }

        public void Timer()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "Timer()", true);
        }

        public void setDefaultFont(int page, XmlDocument xmlFromRhyw)
        {
            string xslPath = "";
            string xslCoordPath = "";

            if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]) != "")
            {
                if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
                {
                    xslCoordPath = Common.GetXSLCoordDirectoryPath();
                }
                else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest"))
                {
                    xslPath = Common.GetXSLCoordDirectoryPath();
                    xslCoordPath = Common.GetXSLCoordDirectoryPath();
                }
                else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("comparisonEntryTest") ||
                    Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("CompUpgradedSampleTest") ||
                    Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("CompUpgradedStartTest"))
                {
                    xslPath = Common.GetXSLDirectoryPath_StartTest();
                    xslCoordPath = Common.GetXSLDirectoryPath_StartTest();
                }
                else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
                {
                    xslPath = Common.GetXSLDirectoryPath();
                    xslCoordPath = Common.GetXSLCoordDirectoryPath();
                }
            }

            page = Convert.ToInt32(Convert.ToString(page).Replace("-", ""));

            if (page < 1)
                return;

            string fontName = "";
            string fontSize = "";
            double top = 0;
            double bottom = 0;
            double right = 0;
            double left = 0;
            double width = 0;
            double height = 0;
            double topMargin = 0;
            double imgTop = 0;

            string marginTop = "xsl:variable[@name=\"margin-top\"]";
            string marginBottom = "xsl:variable[@name=\"margin-bottom\"]";
            string marginRight = "xsl:variable[@name=\"margin-right\"]";
            string marginLeft = "xsl:variable[@name=\"margin-left\"]";
            string pageWidth = "xsl:variable[@name=\"doc-page-width\"]";
            string pageHeight = "xsl:variable[@name=\"doc-page-height\"]";
            //string topPageMargin = "xsl:variable[@name=\"topPageMargin\"]";
            //string imgTopMargin = "xsl:variable[@name=\"imageMarginTop\"]";

            string previouslyUsedXsl = Convert.ToString(Session["previouslyUsedXsl"]);
            string xslUsed = "";
            string tetFilePath = "";

            string mainXml = Convert.ToString(Session["MainXMLFilePath"]).Replace(".xml", ".pdf");

            if (mainXml == "")
                return;

            PdfReader pdfReader = new PdfReader(mainXml);

            //Formulas
            //1 Inch = 72 Points [Postscript]
            //1 Point = 0.01388888889 Inch
            //1 PostScript point = 0.352777778 millimeters

            //Pdf width and height is in points
            var pdfPage = pdfReader.GetPageSize(page);

            double offset = 20;

            //Convert point to mm for use in xsl file
            width = (pdfPage.Width * 0.352777778) + offset;
            height = pdfPage.Height * 0.352777778;

            //If page Height checkbox is selected
            if (cbxlFont.Items[3].Selected)
            {
                iTextSharp.text.Rectangle cropbox = pdfReader.GetCropBox(page);
                var box = pdfReader.GetPageSizeWithRotation(page);

                top = Math.Round((box.Top - cropbox.Top) * 0.352777778, 3);
                bottom = Math.Round(cropbox.Bottom * 0.352777778, 3);
                right = Math.Round((box.Right - cropbox.Right) * 0.352777778, 3);
                left = Math.Round(cropbox.Left * 0.352777778, 3);
            }

            //If coord XSL checkbox is selected
            if (cbxlFont.Items[2].Selected)
            {
                //Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPathCoord"];

                Session["setDefaultXSL"] = xslCoordPath;

                xslUsed = "XSLCoord";

                //UpdateTopMargin_Offset(page);

                ////var topMargin_Temp = UpdateTopMargin(page);


                //topMargin = Convert.ToDouble(topMargin_Temp.Split(',')[0]);
                //imgTop = Convert.ToDouble(topMargin_Temp.Split(',')[1]);

                //var imageMargins_List = UpdateTopMargin_Image(page);
                //imgTop = Convert.ToDouble(imageMargins_List[0].Split(',')[1]);
                //imgLeft = Convert.ToDouble(imageMargins_List[0].Split(',')[0]);
                //imgTop = pdfPage.Height - imgTop - topMargin;

                XmlDocument doc = new XmlDocument();
                doc.Load(xslCoordPath);
                XmlNode root = doc.DocumentElement;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

                string defaultFontName = "xsl:variable[@name=\"xmlFontName\"]";
                string defaultFontSize = "xsl:variable[@name=\"xmlFontSize\"]";

                //If both check boxes are selected use xml's font name and size.
                if ((cbxlFont.Items[0].Selected) && (cbxlFont.Items[1].Selected))
                {
                    fontName = "1";
                    fontSize = "1";
                }

                //If both check boxes are not selected use default font name and size.
                else if ((!cbxlFont.Items[0].Selected) && (!cbxlFont.Items[1].Selected))
                {
                    fontName = "0";
                    fontSize = "0";
                }

                //If font name is checked and font size is not then use xml's font name and default font size.
                else if ((cbxlFont.Items[0].Selected) && (!cbxlFont.Items[1].Selected))
                {
                    fontName = "1";
                    fontSize = "0";
                }

                //If font name is not checked and font size is checked then use default font name and default xml's font.
                else if ((!cbxlFont.Items[0].Selected) && (cbxlFont.Items[1].Selected))
                {
                    fontName = "0";
                    fontSize = "1";
                }

                root.SelectSingleNode(defaultFontName, nsmgr).Attributes["select"].Value = fontName;
                root.SelectSingleNode(defaultFontSize, nsmgr).Attributes["select"].Value = fontSize;

                if (cbxlFont.Items[3].Selected)
                {
                    root.SelectSingleNode(marginTop, nsmgr).Attributes["select"].Value = Convert.ToString(top);
                    root.SelectSingleNode(marginBottom, nsmgr).Attributes["select"].Value = Convert.ToString(bottom);
                    root.SelectSingleNode(marginRight, nsmgr).Attributes["select"].Value = Convert.ToString(right);
                    root.SelectSingleNode(marginLeft, nsmgr).Attributes["select"].Value = Convert.ToString(left);
                    root.SelectSingleNode(pageWidth, nsmgr).Attributes["select"].Value = Convert.ToString(width);
                    root.SelectSingleNode(pageHeight, nsmgr).Attributes["select"].Value = Convert.ToString(height);
                    //root.SelectSingleNode(topPageMargin, nsmgr).Attributes["select"].Value = Convert.ToString(topMargin);
                    //root.SelectSingleNode(imgTopMargin, nsmgr).Attributes["select"].Value = Convert.ToString(imgTop);
                    //root.SelectSingleNode(imgLeftMargin, nsmgr).Attributes["select"].Value = Convert.ToString(imgLeft);
                }
                else
                {
                    root.SelectSingleNode(marginTop, nsmgr).Attributes["select"].Value = Convert.ToString("0");
                    root.SelectSingleNode(marginBottom, nsmgr).Attributes["select"].Value = Convert.ToString("0");
                    root.SelectSingleNode(marginRight, nsmgr).Attributes["select"].Value = Convert.ToString("10");
                    root.SelectSingleNode(marginLeft, nsmgr).Attributes["select"].Value = Convert.ToString("0");
                    //root.SelectSingleNode(pageWidth, nsmgr).Attributes["select"].Value = Convert.ToString("210");
                    //root.SelectSingleNode(pageHeight, nsmgr).Attributes["select"].Value = Convert.ToString("297");

                    root.SelectSingleNode(pageWidth, nsmgr).Attributes["select"].Value = Convert.ToString(width);
                    root.SelectSingleNode(pageHeight, nsmgr).Attributes["select"].Value = Convert.ToString(height);
                }

                doc.Save(xslCoordPath);
            }

            //When normal XSL files are selected
            else
            {
                Session["setDefaultXSL"] = xslPath;

                xslUsed = "XSLNormal";

                XmlDocument doc = new XmlDocument();
                doc.Load(xslPath);
                XmlNode root = doc.DocumentElement;
                XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
                nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

                string defaultFontName = "xsl:variable[@name=\"xmlFontName\"]";
                string defaultFontSize = "xsl:variable[@name=\"xmlFontSize\"]";

                //If both check boxes are selected use xml's font name and size.
                if ((cbxlFont.Items[0].Selected) && (cbxlFont.Items[1].Selected))
                {
                    fontName = "1";
                    fontSize = "1";
                }

                //If both check boxes are not selected use default font name and size.
                else if ((!cbxlFont.Items[0].Selected) && (!cbxlFont.Items[1].Selected))
                {
                    fontName = "0";
                    fontSize = "0";
                }

                //If font name is checked and font size is not then use xml's font name and default font size.
                else if ((cbxlFont.Items[0].Selected) && (!cbxlFont.Items[1].Selected))
                {
                    fontName = "1";
                    fontSize = "0";
                }

                //If font name is not checked and font size is checked then use default font name and default xml's font.
                else if ((!cbxlFont.Items[0].Selected) && (cbxlFont.Items[1].Selected))
                {
                    fontName = "0";
                    fontSize = "1";
                }

                root.SelectSingleNode(defaultFontName, nsmgr).Attributes["select"].Value = fontName;
                root.SelectSingleNode(defaultFontSize, nsmgr).Attributes["select"].Value = fontSize;

                if (cbxlFont.Items[3].Selected)
                {
                    root.SelectSingleNode(marginTop, nsmgr).Attributes["select"].Value = Convert.ToString(top);
                    root.SelectSingleNode(marginBottom, nsmgr).Attributes["select"].Value = Convert.ToString(bottom);
                    root.SelectSingleNode(marginRight, nsmgr).Attributes["select"].Value = Convert.ToString(right);
                    root.SelectSingleNode(marginLeft, nsmgr).Attributes["select"].Value = Convert.ToString(left);
                    root.SelectSingleNode(pageWidth, nsmgr).Attributes["select"].Value = Convert.ToString(width);
                    root.SelectSingleNode(pageHeight, nsmgr).Attributes["select"].Value = Convert.ToString(height);
                }
                else
                {
                    root.SelectSingleNode(marginTop, nsmgr).Attributes["select"].Value = Convert.ToString("25");
                    root.SelectSingleNode(marginBottom, nsmgr).Attributes["select"].Value = Convert.ToString("0");
                    root.SelectSingleNode(marginRight, nsmgr).Attributes["select"].Value = Convert.ToString("20");
                    root.SelectSingleNode(marginLeft, nsmgr).Attributes["select"].Value = Convert.ToString("20");
                    root.SelectSingleNode(pageWidth, nsmgr).Attributes["select"].Value = Convert.ToString("210");
                    root.SelectSingleNode(pageHeight, nsmgr).Attributes["select"].Value = Convert.ToString("297");
                }

                doc.Save(xslPath);
            }

            //Create new pdf by xep after applying font related changes on it
            Common obj = new Common();

            string prdPdfPagePath_Final = obj.GetProducePdf(page, xmlFromRhyw);

            //If xsl is changed then create new teteml of that page
            ////if (!(xslUsed.Trim().Equals(previouslyUsedXsl.Trim())))
            //tetFilePath = Common.Createtetml(prdPdfPagePath_Final);

            Session["previouslyUsedXsl"] = xslUsed;

            ////var list = getMistakeTextList(page);

            ////string mainXmlPath = Convert.ToString(Session["MainXMLFilePath"]);

            ////this.CtrlPdfCmp.childControl(Convert.ToString(page), mainXmlPath, list);
        }



        private string AddAnottation(string outputFile, int pageNum)
        {
            string mainXml = Convert.ToString(Session["MainXMLFilePath"]);

            Common obj = new Common();
            XmlDocument xmlDoc = obj.LoadXmlFromFile(mainXml.Replace(".xml", ".rhyw"));

            XmlDocument pageXML = Common.GetPageXmlDoc(Convert.ToString(pageNum), xmlDoc);
            string dirPath = System.IO.Path.GetDirectoryName(outputFile);
            string xmlPath = dirPath + "\\" + pageNum + ".xml";
            pageXML.Save(xmlPath);
            string prodFilePath = xmlPath.TrimEnd(".xml".ToCharArray()) + ".pdf";
            prodFilePath = AddAnnotationInPDF(outputFile, xmlPath);
            return prodFilePath;
        }

        private string AddAnnotationInPDF(string pdfFilePath, string xmlFilePath)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(xmlFilePath);
            XmlNodeList xmlElements = xmlDoc.SelectNodes("//upara|//spara|//npara|//table|//section");
            ArrayList annotations = new ArrayList();
            string anotHeading = "";
            string anotText = "";
            float llx = 0;
            float lly = 0;
            float urx = 0;
            float ury = 0;
            RHYWAnnotation rhywAnnot = null;

            foreach (XmlNode xmlElement in xmlElements)
            {
                XmlNode lnNode = xmlElement.SelectSingleNode("ln");

                if (lnNode != null)
                {
                    XmlAttribute attrNode = lnNode.Attributes["coord"];
                    string cordinates = attrNode.Value;
                    anotHeading = xmlElement.Name;
                    anotText = xmlElement.Name;

                    if (anotHeading.Equals("image"))
                        rhywAnnot = new RHYWAnnotation(AnnotType.Image);
                    else if (anotHeading.Equals("upara"))
                        rhywAnnot = new RHYWAnnotation(AnnotType.Upara);
                    else if (anotHeading.Equals("spara"))
                        rhywAnnot = new RHYWAnnotation(AnnotType.Spara);
                    else if (anotHeading.Equals("npara"))
                        rhywAnnot = new RHYWAnnotation(AnnotType.Npara);
                    else if (anotHeading.Equals("section"))
                    {
                        rhywAnnot = new RHYWAnnotation(AnnotType.Chapter);
                    }
                    rhywAnnot.Description = lnNode.InnerText;
                    llx = float.Parse(cordinates.Split(':')[0]);
                    lly = float.Parse(cordinates.Split(':')[1]);
                    urx = float.Parse(cordinates.Split(':')[2]);
                    ury = float.Parse(cordinates.Split(':')[3]);
                    float leftOffset = 20f;
                    rhywAnnot.llx = llx;
                    rhywAnnot.lly = lly;
                    rhywAnnot.urx = urx;
                    rhywAnnot.ury = ury;
                    rhywAnnot.llx -= leftOffset;
                }
                ////WriteAnnotationInFile(pdfFilePath, 1, anotHeading, anotText, llx, lly, urx, ury);
                annotations.Add(rhywAnnot);//aamir
            }
            string annotedFilePath = WriteAnnotationsInFile(pdfFilePath, annotations);
            return annotedFilePath;
        }

        private string WriteAnnotationsInFile(string pdfFilePath, ArrayList annotations)
        {
            try
            {
                //string origFile = pdfFilePath;
                //string filename=Path.GetFileNameWithoutExtension(pdfFilePath)+ "_Final.pdf";            
                string newFile = System.IO.Path.GetDirectoryName(pdfFilePath) + "\\" + System.IO.Path.GetFileNameWithoutExtension(pdfFilePath) +
                                 "_Annotated.pdf";
                int pageNum = 1;
                // open the reader
                PdfReader reader = new PdfReader(pdfFilePath);
                iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(pageNum);
                Document document = new Document(size);

                // open the writer
                FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // the pdf content
                PdfContentByte cb = writer.DirectContent;
                for (int i = 0; i < annotations.Count; i++)
                {
                    RHYWAnnotation anot = (RHYWAnnotation)annotations[i];
                    string annotText = anot.Description;
                    string annotHeading = anot.GetHeading();
                    float llx = anot.llx;
                    float lly = anot.lly;
                    float urx = anot.urx;
                    float ury = anot.ury;

                    //document.Add(new Annotation(annotHeading, annotText, llx, lly, urx, ury));
                    //writer.AddAnnotation(PdfAnnotation.CreateText(writer, new Rectangle(50, 620, 70, 640),"NewParagraph", "...", false, "Comment"));

                    //writer.AddAnnotation(PdfAnnotation.CreateText(writer, new Rectangle(llx, lly, urx, ury), annotHeading, annotText, false, "Comment"));
                    PdfAnnotation annotation = new PdfAnnotation(writer, new iTextSharp.text.Rectangle(llx, lly, urx, ury));
                    annotation.Put(PdfName.SUBTYPE, PdfName.TEXT);
                    annotation.Put(PdfName.OPEN, PdfBoolean.PDFFALSE);
                    annotation.Put(PdfName.T, new PdfString(annotHeading));
                    annotation.Put(PdfName.C, new PdfArray(new float[] { 0.0f, 1.0f, 1.0f, 0.0f }));
                    annotation.Put(PdfName.CONTENTS, new PdfString(annotText));
                    writer.AddAnnotation(annotation);
                }
                // create the new page and add it to the pdf
                PdfImportedPage page1 = writer.GetImportedPage(reader, pageNum); //Importing page 1 of the documetn
                //PdfImportedPage page2 = writer.GetImportedPage(reader, 1);
                cb.AddTemplate(page1, 0, 0);
                //cb.AddTemplate(page2, 0, 0);

                // close the streams and voilá the file should be changed :)
                cb.ClosePath();
                document.Close();
                fs.Close();
                writer.Close();
                reader.Close();

                //File.Delete(pdfFilePath);
                return newFile;

                //return Server.MapPath(newFile);
                //return "http://46.4.195.234/pdfweb/newfile.pdf";
                //this.Dispose();
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
        }

        private string WriteAnnotationInFile(string pdfFilePath, int pageNum, string annotHeading, string annotText, float llx, float lly, float urx, float ury)
        {
            try
            {
                string origFile = pdfFilePath;
                string newFile = pdfFilePath + "_2.pdf";

                // open the reader
                PdfReader reader = new PdfReader(origFile);
                iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(pageNum);
                Document document = new Document(size);

                // open the writer
                FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
                PdfWriter writer = PdfWriter.GetInstance(document, fs);
                document.Open();

                // the pdf content
                PdfContentByte cb = writer.DirectContent;

                document.Add(new Annotation(annotHeading, annotText, llx, lly, urx, ury));

                // create the new page and add it to the pdf
                PdfImportedPage page1 = writer.GetImportedPage(reader, pageNum);//Importing page 1 of the documetn
                //PdfImportedPage page2 = writer.GetImportedPage(reader, 1);
                cb.AddTemplate(page1, 0, 0);
                //cb.AddTemplate(page2, 0, 0);

                // close the streams and voilá the file should be changed :)
                document.Close();
                fs.Close();
                writer.Close();
                reader.Close();
                return newFile;
                //return Server.MapPath(newFile);
                //return "http://46.4.195.234/pdfweb/newfile.pdf";
                //this.Dispose();
            }
            catch (Exception exp)
            {
                return exp.Message;
            }
        }

        public void LoadNewPageInControl(int currentPage)
        {
            try
            {
                string srcPagePDFPath = "";

                Session["largerPrdPdfCount"] = "";
                Session["ProducePdfType"] = "Pdf";

                if (Convert.ToString(Session["ProducePdfType"]).Equals("Pdf"))
                {
                    btnNextPage.Enabled = true;

                    ibtnNext.Enabled = true;
                    ibtnPrevious.Enabled = true;
                    ibtnNext.ImageUrl = "~/img/nextBtnIcon.jpg";
                    ibtnPrevious.ImageUrl = "~/img/previousBtnIcon.jpg";
                }

                //if (!string.IsNullOrEmpty(Convert.ToString(Session["ProducedPdfSubPage"])))
                //{
                //    if (Convert.ToString(Session["ProducedPdfSubPage"]).Equals("1")) ibtnPrevious.Enabled = false;
                //    else ibtnPrevious.Enabled = true;
                //}

                int totalPages = Convert.ToInt32(Session["srcTotalPages"]);

                if (currentPage <= totalPages)
                {
                    //Insert PDFmistake attribute in xml 
                    //if(Convert.ToString(Session["SelectedMistakeText"])!="")
                    //    Common.LogMistakesInXml(Convert.ToString(Session["SelectedMistakeText"]));

                    string srcPdfPath = Common.GetPdfPath(currentPage, "src");
                    string prdPdfPath = Common.GetPdfPath(currentPage, "prd");

                    Session["Current_SrcPdfPage"] = currentPage + ".pdf";
                    Session["Current_PrdPdfPage"] = "Produced_" + currentPage + ".pdf";

                    string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);
                    if (String.IsNullOrEmpty(mainXmlPath))
                        return;

                    Common obj = new Common();
                    XmlDocument xmlFromRhyw = obj.LoadXmlFromFile(mainXmlPath.Replace(".xml", ".rhyw"));
                    //this.CtrlPdfCmp.LoadTableText();
                    setDefaultFont(currentPage, xmlFromRhyw);

                    List<PdfJsLine> pdfJsLines = null;

                    if (currentPage > 0)
                    {
                        if ((Convert.ToString(Session["ComparisonTask"]).Equals("test")) || 
                            ((Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest")) ||
                            (Convert.ToString(Session["ComparisonTask"]).Equals("comparisonEntryTest")) ||
                            Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedSampleTest")) ||
                            Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedStartTest"))
                        {
                            hfTaskType.Value = "starttest";
                            if (GetTotalMistakes_Comparison0Test(currentPage) > 0)
                            {
                                pdfJsLines = GetAllLinesFromOnlineTestXml(currentPage);
                                //GetComparison0TestMistakes(currentPage);
                                GetMistakes(currentPage, srcPdfPath, prdPdfPath, pdfJsLines);
                            }
                        }
                        else
                        {
                            //If current page has any mistake on it then highlight function should be called which is inside GetMistakes function.
                            int totalErrors = GetTotalMistakes(currentPage);
                            if (totalErrors > 0)
                            {
                                var lines = GetAllLinesFromXml(currentPage);
                                GetMistakes(currentPage, srcPdfPath, prdPdfPath, lines);
                                Session["UndoHighlighting"] = "false";
                            }
                            else
                            {
                                srcPdfPath = srcPdfPath.Replace("_Annotated", "").Replace("_Stamped", "");
                                prdPdfPath = srcPdfPath.Replace("_Annotated", "").Replace("_Stamped", "");

                                Session["UndoHighlighting"] = "true";
                            }

                            //Show all mistakes inserted inside main xml file
                            string mainXXmlPath = Convert.ToString(Session["MainXMLFilePath"]);
                            if (mainXXmlPath != "")
                                lblTotalErrors.Text = Convert.ToString(GetTotalMistakes(Convert.ToString(Session["MainXMLFilePath"])));

                            //Show current mistake number on a page
                            StreamReader strreader = new StreamReader(mainXXmlPath);
                            string xmlInnerText = strreader.ReadToEnd();
                            strreader.Close();

                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(xmlInnerText);

                            var totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @PDFmistake!='undo' and @page=" + currentPage + "]");

                            //if ((totalMistakes != null) && (totalMistakes.Count > 0))
                            //    TextErrorNum.Text = totalMistakes[0].Attributes["PDFmistake"].Value;

                            MyDBClass dbObj = new MyDBClass();

                            if (!string.IsNullOrEmpty(Convert.ToString(Session["MainBook"])))
                            {
                                int finishBtnClickedCount = dbObj.GetFinishTaskClickedCount(Convert.ToString(Session["MainBook"]));

                                if (finishBtnClickedCount == 3)
                                {
                                    string strStatus = "true";
                                    Page.RegisterStartupScript("Disable", "<script language=JavaScript>document.getElementById('btnFinish_Task').disabled = " +
                                                                strStatus + ";</script>");

                                }
                            }

                            int viewPdfcount = GetViewedProducedPdfs();
                            if (!string.IsNullOrEmpty(Convert.ToString(Session["BookId"])) && !string.IsNullOrEmpty(Convert.ToString(Session["srcTotalPages"])))
                            {
                                if (viewPdfcount <= Convert.ToInt32(Session["srcTotalPages"]))
                                {
                                    string status = dbObj.InsertPageViewedCount(Convert.ToString(Session["BookId"]), viewPdfcount);
                                }
                            }
                        }
                    }

                    txtPageNum.Text = Convert.ToString(currentPage);
                    lblTotalPages.Text = Convert.ToString(Session["srcTotalPages"]);

                    //MyDBClass dbObj = new MyDBClass();

                    //if (!string.IsNullOrEmpty(Convert.ToString(Session["MainBook"])))
                    //{
                    //    int finishBtnClickedCount = dbObj.GetFinishTaskClickedCount(Convert.ToString(Session["MainBook"]));

                    //    if (finishBtnClickedCount == 3)
                    //    {
                    //        string strStatus = "true";
                    //        Page.RegisterStartupScript("Disable", "<script language=JavaScript>document.getElementById('btnFinish_Task').disabled = " + 
                    //                                    strStatus + ";</script>");

                    //    }
                    //}

                    //int viewPdfcount = GetViewedProducedPdfs();
                    //if (!string.IsNullOrEmpty(Convert.ToString(Session["BookId"])) && !string.IsNullOrEmpty(Convert.ToString(Session["srcTotalPages"])))
                    //{
                    //    if (viewPdfcount < Convert.ToInt32(Session["srcTotalPages"]))
                    //    {
                    //        string status = dbObj.InsertPageViewedCount(Convert.ToString(Session["BookId"]), viewPdfcount);
                    //    }
                    //}

                    ShowSrcPDFAsImageInControl(srcPdfPath, Convert.ToString(currentPage));
                    ShowPrdPDFInJSControl(Convert.ToString(currentPage));

                    if (!string.IsNullOrEmpty(Convert.ToString(Session["largerPrdPdfCount"])))
                    {
                        ibtnNext.Visible = true;
                        ibtnPrevious.Visible = true;
                    }
                    else
                    {
                        ibtnNext.Visible = false;
                        ibtnPrevious.Visible = false;
                    }
                    //ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showCidFontLines();", true);
                }
            }
            catch (Exception excep)
            {
                LogWritter.WriteLineInLog("Exception: " + excep.Message);
                //return false;
            }
        }

        private int GetViewedProducedPdfs()
        {
            int viewedPdfCount = 0;
            string path = Common.GetTaskFiles_SavingPath();

            if (!string.IsNullOrEmpty(path))
            {
                var list = Directory.GetFiles(path, "*.pdf");

                if (list != null && list.Length > 0)
                {
                    var producedPdfList = list.Where(x => x.Contains("Produced_")).ToList();

                    if (producedPdfList != null && producedPdfList.Count > 0)
                    {
                        int producedPdfCount = list.Where(x => x.Contains("Produced_")).ToList().Count;
                        int highlightedPdfCount = producedPdfList.Where(x => (x.Contains("_Stamped") || x.Contains("_Annotated"))).ToList().Count;

                        viewedPdfCount = producedPdfCount - highlightedPdfCount;

                        return viewedPdfCount <= 0 ? 0 : viewedPdfCount;
                    }
                }
            }
            return viewedPdfCount;
        }

        public void ShowSrcPDFAsImageInControl(string pdfPath, string page)
        {
            ConvertPdfToImage(pdfPath);

            this.srcImage.Src = "~/showPdf.ashx?Page=" + page + "&pdfType=src" + "&type=img";

            //hfPdfDimensions.Value = "../showPdf.ashx?Page=" + page + "&pdfType=src" + "&type=img";
        }

        //old method
        private void ConvertPdfToImage(string pdfPath)
        {
            PDF2JPG_Sofnix.Convert cnv = new PDF2JPG_Sofnix.Convert();
            bool output = cnv.PDF2ImageFromFile(pdfPath, "");
            //HttpContext.Current.Session["NewPdfPath"] = pdfPath;
        }

        //new
        //private void ConvertPdfToImage(string pdfPath)
        //{
        //    if (string.IsNullOrEmpty(pdfPath)) return;

        //    try
        //    {
        //        pdfPath = pdfPath.Replace("/", "\\");
        //        string destinationPath = Path.GetDirectoryName(pdfPath).Replace("/", "\\");

        //        Process pdfToImgConProcess = new Process();
        //        pdfToImgConProcess.StartInfo.UseShellExecute = false;
        //        pdfToImgConProcess.StartInfo.RedirectStandardError = true;
        //        pdfToImgConProcess.StartInfo.RedirectStandardOutput = true;
        //        pdfToImgConProcess.StartInfo.CreateNoWindow = true;

        //        //pdfToImgConProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

        //        pdfToImgConProcess.StartInfo.Arguments = "\"" + pdfPath + "\"" + " " + "\"" + destinationPath + "\"" + " " + "Jpeg";
        //        pdfToImgConProcess.StartInfo.FileName = Server.MapPath("~\\PdfToImgConverison") + "\\pdftoimg.exe";
        //        //pdfToImgConProcess.StartInfo.FileName = @"F:\Office Data\Currently Working Projects\2016-11-21 BookMicroBeta\PdfToImgConverison\pdftoimg.exe";

        //        pdfToImgConProcess.Start();
        //        //pdfToImgConProcess.WaitForExit();

        //        if (pdfToImgConProcess != null)
        //        {
        //            pdfToImgConProcess.Close();
        //            pdfToImgConProcess.Dispose();
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return;
        //    }
        //    //ConvertPdfToImageOld(pdfPath);
        //}

        public void ShowPrdPDFInJSControl(string page)
        {
            HttpContext.Current.Session["Handler_Page"] = page;
            HttpContext.Current.Session["Handler_pdfType"] = "prd";
            HttpContext.Current.Session["Handler_type"] = "pdf";

            //hfPdfDimensions.Value = "showPdf.ashx?Page=" + page + "&pdfType=prd" + "&type=pdf";
        }

        #region Highlight Mistakes on Pdf Panels

        public int GetSubPrdPdfsCount(string pdfPath, string page)
        {
            int count = 0;

            if (!string.IsNullOrEmpty(pdfPath))
            {
                DirectoryInfo di = new DirectoryInfo(System.IO.Path.GetDirectoryName(pdfPath));
                var pdfFiles = di.GetFiles("*.pdf").Where(file => file.Name.Contains("Produced_" + page) && !file.Name.Contains("_Stamped"))
                                                   .Where(x => !x.Name.Equals("Produced_" + page + ".pdf"))
                                                   .Select(file => file.Name).ToList();

                if (pdfFiles.Count > 0) count = pdfFiles.Count;
            }

            return count;
        }

        public void GetMistakes(int page, string srcPdfPath, string prdPdfPath, List<PdfJsLine> lines)
        {
            //var lines = GetAllLinesFromXml(page);

            if (lines == null || lines.Count == 0) return;

            Common commonObj = new Common();

            string currentPage = Convert.ToString(SiteSession.MainCurrPage);

            ///////////////////////////////////////////

            var directoryPath = GetDirectoryPath();

            string srcPath = directoryPath + "/" + Convert.ToString(HttpContext.Current.Session["Current_SrcPdfPage"]);
            string prdPath = directoryPath + "/" + Convert.ToString(HttpContext.Current.Session["Current_PrdPdfPage"]);

            string tetmlPrdFilePath = GetTetmlFilePath(currentPage, "producedPdf");

            //if (GetSubPrdPdfsCount(prdPath, currentPage) > 0)
            //{
            //    prdPath = prdPath.Replace("Produced_" + currentPage, "Produced_" + currentPage + "_" + subPdfPage);
            //    tetmlPrdFilePath = tetmlPrdFilePath.Replace("Produced_" + currentPage, "Produced_" + currentPage + "_" + subPdfPage);

            //    HttpContext.Current.Session["Handler_PrdPdfSubPage"] = subPdfPage;
            //    HttpContext.Current.Session["ProducePdfType"] = "SubPdf";

            //    if (!string.IsNullOrEmpty(Convert.ToString(Session["ComparisonTask"])))
            //    {
            //        if (Convert.ToString(Session["ProducePdfType"]).Equals("SubPdf"))
            //            btnNextPage.Enabled = false;

            //        else btnNextPage.Enabled = true;
            //    }

            //    if (subPdfPage == 1)
            //    {
            //        ibtnPrevious.Enabled = false;
            //        ibtnPrevious.ImageUrl = "~/img/previousBtnIcon_disabled.jpg";
            //    }
            //    else
            //    {
            //        ibtnPrevious.Enabled = true;
            //        ibtnPrevious.ImageUrl = "~/img/previousBtnIcon.jpg";
            //    }

            //    //Session["ProducedPdfSubPage"] = "1";

            //    //if (!string.IsNullOrEmpty(Convert.ToString(Session["ProducedPdfSubPage"])))
            //    //{
            //    //    if (Convert.ToString(Session["ProducedPdfSubPage"]).Equals("1")) ibtnPrevious.Enabled = false;
            //    //    else ibtnPrevious.Enabled = true;
            //    //}
            //}
            //else

               ibtnPrevious.Enabled = true;

                HttpContext.Current.Session["ProducePdfType"] = "Pdf";

            var tetmlSrcFilePath = GetTetmlFilePath(currentPage, "sourcePdf");
            var tetmlSrcDoc = LoadTetmlXmlDocument(tetmlSrcFilePath);
            List<TetmlLine> sourcePdftetmlLine = commonObj.GetAllLinesFromTetml(tetmlSrcDoc);

            //string tetmlPrdFilePath = GetTetmlFilePath(currentPage, "producedPdf");
            var tetmlPrdDoc = LoadTetmlXmlDocument(tetmlPrdFilePath);
            List<TetmlLine> prdPdftetmlLine = commonObj.GetAllLinesFromTetml(tetmlPrdDoc);

            string highlightedSrcPdfPath = "";
            string highlightedPrdPdfPath = "";

            if (sourcePdftetmlLine != null && prdPdftetmlLine != null)
            {
                if (prdPdftetmlLine[prdPdftetmlLine.Count - 1].Text.Trim().Equals("1"))
                    prdPdftetmlLine.RemoveAt(prdPdftetmlLine.Count - 1);

                //if ((sourcePdftetmlLine.Count != prdPdftetmlLine.Count) || (prdPdftetmlLine.Count != lines.Count) || (sourcePdftetmlLine.Count != lines.Count))
                //{
                    highlightedPrdPdfPath = commonObj.MatchTextByWords(currentPage, lines, tetmlPrdDoc, prdPath, "producedPdf");
                    highlightedSrcPdfPath = commonObj.MatchTextByWords(currentPage, lines, tetmlSrcDoc, srcPath, "sourcePdf");
                //}
                //else if (sourcePdftetmlLine.Count == prdPdftetmlLine.Count && lines.Count == prdPdftetmlLine.Count)
                //{
                //    var srcPdfLinesCoords = GerTetmlCoordOfMistakeLines(lines, sourcePdftetmlLine, currentPage);
                //    PDFManipulation pdfMan = new PDFManipulation(srcPath);
                //    highlightedSrcPdfPath = pdfMan.HighlightWord(srcPath, srcPdfLinesCoords);

                //    var prdPdfLinesCoords = GerTetmlCoordOfMistakeLines(lines, prdPdftetmlLine, currentPage);
                //    PDFManipulation pdfManPrd = new PDFManipulation(prdPath);
                //    highlightedPrdPdfPath = pdfManPrd.HighlightWord(prdPath, prdPdfLinesCoords);
                //}
            }

            HttpContext.Current.Session["Highlighted_prdPdfPagePath"] = highlightedPrdPdfPath;

            if (!File.Exists(highlightedSrcPdfPath.Replace(".pdf", ".pdf.jpeg"))) ConvertPdfToImage(highlightedSrcPdfPath);
            HttpContext.Current.Session["Highlighted_srcPdfPagePath"] = highlightedSrcPdfPath;
        }

        private string GetDirectoryPath()
        {
            string directoryPath = "";

            if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]) != "")
            {
                if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
                    directoryPath = Common.GetTestFiles_SavingPath();

                else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest"))
                    directoryPath = Common.GetOnePageTestFiles_SavingPath();

                else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
                    directoryPath = Common.GetTaskFiles_SavingPath();

                else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("comparisonEntryTest") ||
                    Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("CompUpgradedSampleTest") ||
                    Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("CompUpgradedStartTest"))
                    directoryPath = Common.GetComparisonEntryTestFiles_SavingPath();
            }
            return directoryPath;
        }

        private List<PdfWord> GerTetmlCoordOfMistakeLines(List<PdfJsLine> lines, List<TetmlLine> sourcePdftetmlLine, string currentPage)
        {
            string coordinates = "";
            List<PdfWord> wrdList = new List<PdfWord>();

            foreach (var text in lines)
            {
                if (text.IsErrorLine)
                {
                    List<TetmlLine> tetmlLine = sourcePdftetmlLine.Where(x => x.LineNum == text.LineNum).ToList();

                    if (tetmlLine != null)
                    {
                        if (tetmlLine.Count > 0)
                        {
                            coordinates = tetmlLine[0].Llx + ":" + tetmlLine[0].Lly + ":" + tetmlLine[0].Urx + ":" +
                                          tetmlLine[0].Ury;

                            wrdList.Add(new PdfWord(Convert.ToInt32(currentPage.Replace("Produced_", "").Trim()), -1,
                                tetmlLine[0].Text, coordinates));
                        }
                    }
                }
            }
            return wrdList;
        }

        private List<PdfJsLine> GetAllLinesFromXml(int page)
        {
            List<PdfJsLine> selectedText = new List<PdfJsLine>();
            Common comObj = new Common();

            XmlDocument xmlDoc = comObj.LoadXmlDocument(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));
            List<XmlNode> xmlLinesList = xmlDoc.SelectNodes(@"//*[@page=" + page + "]").Cast<XmlNode>().ToList();

            List<double> llyList = xmlLinesList.Where(x => x.Attributes["top"] != null)
                                               .Select(x => x.Attributes["top"] == null ? 0 : Convert.ToDouble(x.Attributes["top"].Value)).Distinct().ToList();

            List<string> pageLinesList = new List<string>();
            StringBuilder sameLlyLine = new StringBuilder();
            int lineCounter = 0;
            int sameUryLineCounter = 0;
            bool isErrorLine = false;
            bool isFirstLineHasError = false;
            bool fontType = false;

            if (llyList != null)
            {
                foreach (double llyValue in llyList)
                {
                    var matchedXmlLines = xmlLinesList.Where(x => x.Attributes["top"] != null && Convert.ToDouble(x.Attributes["top"].Value) == llyValue).ToList();
                    if (matchedXmlLines != null)
                    {
                        if (matchedXmlLines.Count == 1)
                        {
                            lineCounter++;

                            if (matchedXmlLines[0].Attributes["PDFmistake"] != null &&
                                matchedXmlLines[0].Attributes["PDFmistake"].Value != "" &&
                                matchedXmlLines[0].Attributes["PDFmistake"].Value != "undo")
                            {
                                isErrorLine = true;
                            }
                            else
                                isErrorLine = false;

                            selectedText.Add(new PdfJsLine
                            {
                                Text = matchedXmlLines[0].InnerText,
                                LineNum = lineCounter,
                                IsErrorLine = isErrorLine,
                                IsEmbededFontLine = matchedXmlLines[0].Attributes["fonttype"] == null
                                    ? true
                                    : (matchedXmlLines[0].Attributes["fonttype"].Value == "Embeded" ? true : false)
                            });

                        }
                        else if (matchedXmlLines.Count > 1)
                        {
                            lineCounter++;

                            foreach (XmlNode line in matchedXmlLines)
                            {
                                sameUryLineCounter++;

                                if (sameUryLineCounter == 1)
                                {
                                    if (line.Attributes["PDFmistake"] != null &&
                                        line.Attributes["PDFmistake"].Value != "" &&
                                        line.Attributes["PDFmistake"].Value != "undo")
                                    {
                                        isErrorLine = true;
                                    }
                                    else
                                        isErrorLine = false;

                                    fontType = line.Attributes["fonttype"] == null ? true : line.Attributes["fonttype"].Value == "Embeded" ? true : false;
                                }
                                sameLlyLine.Append(line.InnerText + " ");
                            }

                            selectedText.Add(new PdfJsLine
                                   {
                                       Text = Convert.ToString(sameLlyLine),
                                       LineNum = lineCounter,
                                       IsErrorLine = isErrorLine,
                                       IsEmbededFontLine = fontType
                                   });

                        }

                    }
                    sameUryLineCounter = 0;
                    sameLlyLine.Length = 0;
                }
            }

            return selectedText;
        }

        ///
        /// 
        /// 
        //old 2016-10-19
        //private List<PdfJsLine> GetAllLinesFromXml(int page)
        //{
        //    List<PdfJsLine> selectedText = new List<PdfJsLine>();
        //    Common comObj = new Common();

        //    XmlDocument xmlDoc = comObj.LoadXmlDocument(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));
        //    XmlNodeList pdfmistakes_Nodes = xmlDoc.SelectNodes(@"//*[@page=" + page + "]");


        //    if (pdfmistakes_Nodes.Count > 0)
        //    {
        //        for (int i = 0; i < pdfmistakes_Nodes.Count; i++)
        //        {
        //            if (pdfmistakes_Nodes[i].Name != "Table")
        //            {
        //                if (pdfmistakes_Nodes[i].Attributes["PDFmistake"] != null &&
        //                    pdfmistakes_Nodes[i].Attributes["PDFmistake"].Value != "" &&
        //                    pdfmistakes_Nodes[i].Attributes["PDFmistake"].Value != "undo")
        //                {
        //                    selectedText.Add(new PdfJsLine
        //                    {
        //                        Text = pdfmistakes_Nodes[i].InnerText,
        //                        LineNum = i + 1,
        //                        IsErrorLine = true,
        //                        IsEmbededFontLine = pdfmistakes_Nodes[i].Attributes["fonttype"] == null
        //                            ? true
        //                            : (pdfmistakes_Nodes[i].Attributes["fonttype"].Value == "Embeded" ? true : false)
        //                    });
        //                }
        //                else
        //                {
        //                    selectedText.Add(new PdfJsLine
        //                    {
        //                        Text = pdfmistakes_Nodes[i].InnerText,
        //                        LineNum = i + 1,
        //                        IsErrorLine = false,
        //                        IsEmbededFontLine = pdfmistakes_Nodes[i].Attributes["fonttype"] == null
        //                            ? true
        //                            : (pdfmistakes_Nodes[i].Attributes["fonttype"].Value == "Embeded" ? true : false)
        //                    });
        //                }
        //            }
        //        }
        //    }
        //    return selectedText;
        //}

        private List<PdfJsLine> GetAllLinesFromOnlineTestXml(int page)
        {
            var path = Convert.ToString(Session["MainXMLFilePath"]);

            List<PdfJsLine> selectedText = new List<PdfJsLine>();
            Common comObj = new Common();

            XmlDocument xmlDoc = comObj.LoadXmlDocument(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));
            //XmlNodeList pdfmistakes_Nodes = xmlDoc.SelectNodes(@"//*[@PDFmistakeTest!='' and @page=" + page + "]");
            XmlNodeList pdfmistakes_Nodes = xmlDoc.SelectNodes(@"//*[@page=" + page + "]");

            if (pdfmistakes_Nodes.Count > 0)
            {
                for (int i = 0; i < pdfmistakes_Nodes.Count; i++)
                {
                    if (pdfmistakes_Nodes[i].Attributes["PDFmistakeTest"] != null && pdfmistakes_Nodes[i].Attributes["PDFmistakeTest"].Value != "")
                    {
                        selectedText.Add(new PdfJsLine
                        {
                            Text = pdfmistakes_Nodes[i].InnerText,
                            LineNum = i + 1,
                            IsErrorLine = true
                        });
                    }
                    else
                    {
                        selectedText.Add(new PdfJsLine
                        {
                            Text = pdfmistakes_Nodes[i].InnerText,
                            LineNum = i + 1,
                            IsErrorLine = false
                        });
                    }
                }
            }
            return selectedText;
        }

        private void GetCidFontLines(List<PdfJsLine> pageLines)
        {

        }

        private string GetTetmlFilePath(string currentPage, string pdfType)
        {
            string tetFilePath = "";

            //Set Task and tests tetml paths
            if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
            {
                if (pdfType.Equals("producedPdf"))
                    tetFilePath = Common.GetTestFiles_SavingPath() + "/" + "\\Produced_" + currentPage + ".tetml";

                else
                    tetFilePath = Common.GetTestFiles_SavingPath() + "/" + "\\" + currentPage + ".tetml";
            }
            else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest"))
            {
                if (pdfType.Equals("producedPdf"))
                    tetFilePath = Common.GetOnePageTestFiles_SavingPath() + "/" + "\\Produced_" + currentPage + ".tetml";

                else
                    tetFilePath = Common.GetOnePageTestFiles_SavingPath() + "/" + "\\" + currentPage + ".tetml";
            }
            else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("comparisonEntryTest") ||
                Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("CompUpgradedSampleTest") ||
                Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("CompUpgradedStartTest"))
            {
                if (pdfType.Equals("producedPdf"))
                    tetFilePath = Common.GetComparisonEntryTestFiles_SavingPath() + "/" + "\\Produced_" + currentPage + ".tetml";

                else
                    tetFilePath = Common.GetComparisonEntryTestFiles_SavingPath() + "/" + "\\" + currentPage + ".tetml";
            }
            else
            {
                if (pdfType.Equals("producedPdf"))
                    tetFilePath = Common.GetTaskFiles_SavingPath() + "/" + "\\Produced_" + currentPage + ".tetml";

                else
                    tetFilePath = Common.GetTaskFiles_SavingPath() + "/" + "\\" + currentPage + ".tetml";
            }
            //end
            return tetFilePath;
        }

        private XmlDocument LoadTetmlXmlDocument(string tetFilePath)
        {
            XmlDocument tetDoc = new XmlDocument();
            try
            {
                StreamReader sr = new StreamReader(tetFilePath);
                string xmlText = sr.ReadToEnd();
                sr.Close();
                string documentXML = Regex.Match(xmlText, "<Document.*?</Document>", RegexOptions.Singleline).ToString();
                tetDoc.LoadXml(documentXML);
            }
            catch
            {
                return null;
            }
            return tetDoc;
        }

        #endregion


        public void ReplacePrdText(List<string> listMistakeText)
        {
            List<string> list_PrdText = (List<string>)HttpContext.Current.Session["list_PrdPdfLines"];
            List<string> list_SrcText = (List<string>)HttpContext.Current.Session["list_SrcPdfLines"];

            List<string> list_SrcTextNew = new List<string>();
            bool check1 = false;
            int count = 0;

            if (list_PrdText == null)
                return;

            for (int k = 0; k < listMistakeText.Count; k++)
            {
                for (int i = 0; i < list_PrdText.Count; i++)
                {
                    count = 0;
                    check1 = false;

                    if (listMistakeText[k].Trim().Equals(list_PrdText[i].Trim()))
                    {

                        check1 = true;
                        count = i;
                    }

                    if (check1)
                    {
                        for (int j = 0; j < list_SrcText.Count; j++)
                        {
                            if (list_SrcText[j].Trim().Equals(list_PrdText[count + 1].Trim()))
                            //if (MatchLineByWords(list_SrcText[j].Trim(), list_PrdText[count + 1].Trim()))
                            {
                                listMistakeText[k] = list_SrcText[j - 1].Trim();
                                count = 1;
                            }

                            if (count == 1)
                            {
                                break;
                            }
                        }

                        break;
                    }
                }
            }

            var tt = list_SrcText;

            //return list_SrcText;
        }

        public int GetTotalMistakes_Comparison0Test(int page)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["MainXMLFilePath"]))) return 0;

            if (!File.Exists(Convert.ToString(Session["MainXMLFilePath"]))) return 0;

            StreamReader strreader = new StreamReader(Convert.ToString(Session["MainXMLFilePath"]));
            string xmlInnerText = strreader.ReadToEnd();
            strreader.Close();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlInnerText);

            int totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistakeTest!='' and @page=" + page + "]").Count;
            return totalMistakes;
        }

        public int GetTotalMistakes(int page)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["MainXMLFilePath"]))) return 0;

            if (!File.Exists(Convert.ToString(Session["MainXMLFilePath"]))) return 0;

            StreamReader strreader = new StreamReader(Convert.ToString(Session["MainXMLFilePath"]));

            string xmlInnerText = strreader.ReadToEnd();
            strreader.Close();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlInnerText);

            int totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @PDFmistake!='undo' and @page=" + page + "]").Count;
            return totalMistakes;
        }

        private int GetTotalMistakes(string xmlPath)
        {
            StreamReader strreader = new StreamReader(xmlPath);
            string xmlInnerText = strreader.ReadToEnd();
            strreader.Close();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlInnerText);

            int totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @PDFmistake!='undo']").Count;

            return totalMistakes;
        }

        //public void GetComparison0TestMistakes(int page)
        //{
        //    StreamReader strreader = new StreamReader(Convert.ToString(Session["MainXMLFilePath"]));
        //    string xmlInnerText = strreader.ReadToEnd();
        //    strreader.Close();

        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.LoadXml(xmlInnerText);

        //    XmlNodeList pdfmistakes_Nodes = xmlDoc.SelectNodes(@"//*[@PDFmistakeTest!='' and @page=" + page + "]");
        //    List<string> selectedText = new List<string>();

        //    if (pdfmistakes_Nodes.Count > 0)
        //    {
        //        foreach (XmlNode node in pdfmistakes_Nodes)
        //        {
        //            selectedText.Add(node.InnerText);
        //        }
        //    }

        //    //string srcPdf = Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["srcPdfPagePath"]));
        //    //string prdPdf = Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["prdPdfPagePath"]));

        //    string mainXmlPath = Convert.ToString(Session["MainXMLFilePath"]);

        //    //To do aamir 2016-06-18
        //    ////string secondfilepath = LoadMistakePanel(Convert.ToString(page), mainXmlPath, selectedText, "producedPdf");
        //    //string secondfilepath = Common.LoadMistakePanel(Convert.ToString(page), mainXmlPath, selectedText, "producedPdf");

        //    //HttpContext.Current.Session["prdPdfPagePath"] = secondfilepath;

        //    ////To do
        //    ////ReplacePrdText(selectedText);

        //    ////string firstfilepath = LoadMistakePanel(Convert.ToString(page), mainXmlPath, selectedText, "sourcePdf");
        //    //string firstfilepath = Common.LoadMistakePanel(Convert.ToString(page), mainXmlPath, selectedText, "sourcePdf");

        //    //HttpContext.Current.Session["srcPdfPagePath"] = firstfilepath;
        //    ////GetTotalMistakes_List();

        //    ////HttpContext.Current.Response.Redirect("Comparison.aspx?mistake=1", true);
        //}

        public void GetComparison0TestMistakes(int page)
        {
            var lines = GetAllLinesFromOnlineTestXml(page);

            Common commonObj = new Common();

            string currentPage = Convert.ToString(SiteSession.MainCurrPage);

            ///////////////////////////////////////////

            var directoryPath = GetDirectoryPath();

            var tetmlSrcFilePath = GetTetmlFilePath(currentPage, "sourcePdf");
            var tetmlSrcDoc = LoadTetmlXmlDocument(tetmlSrcFilePath);
            List<TetmlLine> sourcePdftetmlLine = commonObj.GetAllLinesFromTetml(tetmlSrcDoc);

            var tetmlPrdFilePath = GetTetmlFilePath(currentPage, "producedPdf");
            var tetmlPrdDoc = LoadTetmlXmlDocument(tetmlPrdFilePath);
            List<TetmlLine> prdPdftetmlLine = commonObj.GetAllLinesFromTetml(tetmlPrdDoc);

            string srcPath = directoryPath + "/" + Convert.ToString(HttpContext.Current.Session["Current_SrcPdfPage"]);
            string prdPath = directoryPath + "/" + Convert.ToString(HttpContext.Current.Session["Current_PrdPdfPage"]);

            string highlightedSrcPdfPath = "";
            string highlightedPrdPdfPath = "";

            if (sourcePdftetmlLine != null && prdPdftetmlLine != null)
            {
                if (prdPdftetmlLine[prdPdftetmlLine.Count - 1].Text.Trim().Equals("1"))
                    prdPdftetmlLine.RemoveAt(prdPdftetmlLine.Count - 1);

                if (sourcePdftetmlLine.Count != prdPdftetmlLine.Count)
                {
                    highlightedPrdPdfPath = commonObj.MatchTextByWords(currentPage, lines, tetmlPrdDoc, prdPath, "producedPdf");
                    highlightedSrcPdfPath = commonObj.MatchTextByWords(currentPage, lines, tetmlSrcDoc, srcPath, "sourcePdf");
                }
                else if (sourcePdftetmlLine.Count == prdPdftetmlLine.Count)
                {
                    var srcPdfLinesCoords = GerTetmlCoordOfMistakeLines(lines, sourcePdftetmlLine, currentPage);
                    PDFManipulation pdfMan = new PDFManipulation(srcPath);
                    highlightedSrcPdfPath = pdfMan.HighlightWord(srcPath, srcPdfLinesCoords);

                    var prdPdfLinesCoords = GerTetmlCoordOfMistakeLines(lines, prdPdftetmlLine, currentPage);
                    PDFManipulation pdfManPrd = new PDFManipulation(prdPath);
                    highlightedPrdPdfPath = pdfManPrd.HighlightWord(prdPath, prdPdfLinesCoords);
                }
            }

            HttpContext.Current.Session["Highlighted_prdPdfPagePath"] = highlightedPrdPdfPath;

            if (!File.Exists(highlightedSrcPdfPath.Replace(".pdf", ".pdf.jpeg"))) ConvertPdfToImage(highlightedSrcPdfPath);
            HttpContext.Current.Session["Highlighted_srcPdfPagePath"] = highlightedSrcPdfPath;



            //StreamReader strreader = new StreamReader(Convert.ToString(Session["MainXMLFilePath"]));
            //string xmlInnerText = strreader.ReadToEnd();
            //strreader.Close();

            //XmlDocument xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(xmlInnerText);

            ////XmlNodeList pdfmistakes_Nodes = xmlDoc.SelectNodes(@"//*[@PDFmistakeTest!='' and @page=" + page + "]");
            ////List<string> selectedText = new List<string>();

            ////if (pdfmistakes_Nodes.Count > 0)
            ////{
            ////    foreach (XmlNode node in pdfmistakes_Nodes)
            ////    {
            ////        selectedText.Add(node.InnerText);
            ////    }
            ////}

            //string srcPdf = Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["srcPdfPagePath"]));
            //string prdPdf = Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["prdPdfPagePath"]));

            //string mainXmlPath = Convert.ToString(Session["MainXMLFilePath"]);

            //To do aamir 2016-06-18
            ////string secondfilepath = LoadMistakePanel(Convert.ToString(page), mainXmlPath, selectedText, "producedPdf");
            //string secondfilepath = Common.LoadMistakePanel(Convert.ToString(page), mainXmlPath, selectedText, "producedPdf");

            //HttpContext.Current.Session["prdPdfPagePath"] = secondfilepath;

            ////To do
            ////ReplacePrdText(selectedText);

            ////string firstfilepath = LoadMistakePanel(Convert.ToString(page), mainXmlPath, selectedText, "sourcePdf");
            //string firstfilepath = Common.LoadMistakePanel(Convert.ToString(page), mainXmlPath, selectedText, "sourcePdf");

            //HttpContext.Current.Session["srcPdfPagePath"] = firstfilepath;
            ////GetTotalMistakes_List();

            ////HttpContext.Current.Response.Redirect("Comparison.aspx?mistake=1", true);
        }

        public void FinishTest(string xmlPath)
        {
            StreamReader sr = new StreamReader(xmlPath);
            string xmlFile = sr.ReadToEnd();
            sr.Close();

            XmlDocument xmlDocOrigXml = new XmlDocument();
            xmlDocOrigXml.LoadXml(xmlFile);

            double detectedMistakes = xmlDocOrigXml.SelectNodes(@"//ln[@PDFmistake='']").Count;
            double totalMistakes = Convert.ToDouble(Session["ComparisonTestTotalMistakes"]);
            //string testName = Path.GetFileNameWithoutExtension(xmlPath);
            string email = Convert.ToString(Session["ComparisonTestUser_Email"]);
            string testName = System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"]));

            double percentage = 0.0;

            if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
            {
                if (totalMistakes > 0)
                {
                    percentage = Convert.ToDouble(detectedMistakes / totalMistakes) * 100;

                    if (percentage >= 100)
                    {
                        Response.Redirect("OnlineTestUser.aspx");
                    }
                }
            }
            else
            {
                //MyDBClass objMyDBClass = new MyDBClass();
                //string querySel = "Select distinct(Name) from OnlineTestScore where Email='" + email + "'";
                //DataSet dsBookInfo = objMyDBClass.GetDataSet(querySel);
                //string userName = dsBookInfo.Tables[0].Rows[0]["Name"].ToString();

                if (totalMistakes > 0)
                {
                    percentage = Math.Round(Convert.ToDouble(detectedMistakes / totalMistakes) * 100);

                    if (percentage >= 100)
                    {
                        //string rows = objMyDBClass.SaveResult(userName, email, testName, Convert.ToString(detectedMistakes), "passed");
                        //string rows1 = objMyDBClass.MovePassedResult(userName, email);
                        //string rows2 = objMyDBClass.ClearComparisonTest("ErrorDetection", Convert.ToString(Session["userId"]), "passed", testName);

                        objMyDBClass.updateOnlineTestDetails("ErrorDetection", Convert.ToString(Session["userId"]), "Passed", System.DateTime.Now.ToString());
                        Response.Redirect(string.Format("Passed.aspx?userId={0}&type=test&p={1}&t={2}", Convert.ToString(Session["userId"]), percentage, testName), true);
                    }

                    else
                    {
                        //string value = objMyDBClass.SaveResult(userName, email, testName, Convert.ToString(detectedMistakes), "failed");
                        objMyDBClass.updateOnlineTestDetails("ErrorDetection", Convert.ToString(Session["userId"]), "Failed", System.DateTime.Now.ToString());
                        Response.Redirect(string.Format("Failed.aspx?userId={1}&type=test&p={2}&t={3}", Request.Url.Host, Convert.ToString(Session["userId"]), percentage, testName), true);
                    }
                }
            }
        }

        public void FinishComparisonEntryTest(string xmlPath)
        {
            StreamReader sr = new StreamReader(xmlPath);
            string xmlFile = sr.ReadToEnd();
            sr.Close();

            XmlDocument xmlDocOrigXml = new XmlDocument();
            xmlDocOrigXml.LoadXml(xmlFile);

            string email = Convert.ToString(Session["ComparisonTestUser_Email"]);
            string testName = System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"]));

            double detectedMistakes = xmlDocOrigXml.SelectNodes(@"//ln[@PDFmistake='']").Count;
            double totalMistakes = Convert.ToDouble(Session["TotalMarks"]);

            double percentage = 0.0;

            Session.Remove("TimeUpdator_StartTime");
            Session.Remove("TimeUpdator_EndTime");

            percentage = Convert.ToDouble(detectedMistakes/totalMistakes)*100;

            Session["Result"] = detectedMistakes;

            if (Convert.ToString(Session["ComparisonTask"]).Equals("comparisonEntryTest"))
            {
                if (percentage >= 80)
                    Response.Redirect("Passed.aspx", false);
                else
                    Response.Redirect("Failed.aspx", false);
            }
            if (Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedSampleTest"))
            {
                if (percentage >= 80)
                    Response.Redirect("Passed.aspx?type=CompUpgradedSampleTest", false);
                else
                    Response.Redirect("Failed.aspx", false);
            }
            if (Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedStartTest"))
            {
                if (percentage >= 80)
                    Response.Redirect("Passed.aspx?type=CompUpgradedStartTest", false);
                else
                    Response.Redirect("Failed.aspx", false);
            }

            //if (percentage >= 80)
            //    Response.Redirect("Passed.aspx?type=upgraded", false);
            //else
            //    Response.Redirect("Failed.aspx", false);
        }

        public void GetSrcPageMistakes(int page, string srcPdfPath)
        {
            Common comObj = new Common();
            XmlDocument xmlDoc = comObj.LoadXmlDocument(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));

            if (xmlDoc == null)
                return;

            XmlNodeList pdfmistakes_Nodes = xmlDoc.SelectNodes(@"//*[@PDFmistakeTest!='' and @page=" + page + "]");
            List<string> selectedText = new List<string>();

            if (pdfmistakes_Nodes.Count > 0)
            {
                foreach (XmlNode node in pdfmistakes_Nodes)
                {
                    selectedText.Add(node.InnerText);
                }
            }

            //To do aamir 2016-06-18
            //string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);
            //int currentPage = SiteSession.MainCurrPage;

            //string firstfilepath = Common.LoadMistakePanel(Convert.ToString(currentPage), mainXmlPath, selectedText, "sourcePdf");

            //HttpContext.Current.Session["Highlighted_srcPdfPagePath"] = firstfilepath;
        }

        //protected void btnNext_Click(object sender, EventArgs e)
        //{
        //    Common comObj = new Common();

        //    //string pdfPath = Convert.ToString(Session["Current_PrdPdfPage"]);
        //    int currentPage = Convert.ToInt32(Session["MainCurrPage"]);

        //    string pdfPath = Common.GetPdfPath(currentPage, "prd");

        //    pdfPath = pdfPath.Replace("Produced_", "Produced_" + currentPage + "_1");

        //    //comObj.ExtractPages(pdfPath, pdfPath.Replace("Produced_", "Produced_a_"), currentPage, currentPage);

        //    //HttpContext.Current.Session["Handler_Page"] = currentPage;
        //    //HttpContext.Current.Session["Handler_pdfType"] = "prd";
        //    HttpContext.Current.Session["Handler_type"] = "subPdf";
        //}
        //protected void btnPrevious_Click(object sender, EventArgs e)
        //{

        //}

        //public void ShowPrdPagesInControl(int currentPage)
        //{
        //    if (!string.IsNullOrEmpty(Convert.ToString(Session["largerPrdPdfCount"])))
        //    {
        //        ibtnNext.Visible = true;
        //        ibtnPrevious.Visible = true;

        //        //ibtnNext.Attributes.Add("style", "position:absolute; top:50%;left:52%;");
        //        //ibtnPrevious.Attributes.Add("style", "position:absolute; top:50%; left:95.5%;");
        //    }
        //    else
        //    {
        //        ibtnNext.Visible = false;
        //        ibtnPrevious.Visible = false;
        //    }

        //    int mainCurrentPage = Convert.ToInt32(Session["MainCurrPage"]);

        //    //If current page has any mistake on it then highlight function should be called which is inside GetMistakes function. 
        //    int totalErrors = GetTotalMistakes(mainCurrentPage);

        //    if (totalErrors > 0)
        //    {
        //        string srcPdfPath = Common.GetPdfPath(mainCurrentPage, "src");
        //        string prdPdfPath = Common.GetPdfPath(mainCurrentPage, "prd");

        //        prdPdfPath = prdPdfPath.Replace("Produced_" + mainCurrentPage, "Produced_" + mainCurrentPage + "_" + currentPage)
        //                               .Replace("Produced_" + mainCurrentPage + "_Stamped", "Produced_" + mainCurrentPage + "_Stamped_" + currentPage);

        //        var lines = GetAllLinesFromXml(mainCurrentPage);
        //        GetMistakes(currentPage, srcPdfPath, prdPdfPath, lines);

        //        if (!string.IsNullOrEmpty(Convert.ToString(Session["ComparisonTask"])))
        //        {
        //            if (Convert.ToString(Session["ProducePdfType"]).Equals("SubPdf") &&
        //                currentPage == Convert.ToInt32(Session["largerPrdPdfCount"]))
        //            {
        //                btnNextPage.Enabled = true;
        //                ibtnNext.Enabled = false;
        //                ibtnNext.ImageUrl = "~/img/nextBtnIcon_disabled.jpg";
        //            }
        //            else
        //            {
        //                ibtnNext.Enabled = true;
        //                ibtnNext.ImageUrl = "~/img/nextBtnIcon.jpg";
        //            }
        //        }
        //    }

        //    Session["ProducedPdfSubPage"] = currentPage;
        //    HttpContext.Current.Session["Handler_PrdPdfSubPage"] = currentPage;
        //    //HttpContext.Current.Session["Handler_type"] = "subPdf";
        //    HttpContext.Current.Session["ProducePdfType"] = "SubPdf";
        //}

        protected void btnNext_Click(object sender, EventArgs e)
        {
            //ibtnPrevious.Visible = false;
            //ibtnNext.Enabled = false;

            int totalPages = Convert.ToInt32(Session["largerPrdPdfCount"]);
            int currentSubPage = Convert.ToInt32(Session["ProducedPdfSubPage"]);

            int pageNum = 0;

            pageNum = currentSubPage < totalPages ? (currentSubPage + 1) : totalPages;

            //ShowPrdPagesInControl(pageNum);

            
        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            int currentSubPage = Convert.ToInt32(Session["ProducedPdfSubPage"]);

            int pageNum = 1;

            pageNum = currentSubPage > 1 ? (currentSubPage - 1) : 1;

            Session["ProducedPdfSubPage"] = pageNum;
            //ShowPrdPagesInControl(pageNum);

            if (!string.IsNullOrEmpty(Convert.ToString(Session["ProducedPdfSubPage"])))
            {
                if (Convert.ToString(Session["ProducedPdfSubPage"]).Equals("1")) ibtnPrevious.Enabled = false;
                else ibtnPrevious.Enabled = true;
            }
        }

        #endregion

        #region Web Methods

        [WebMethod]
        public static string GetParaType(string text)
        {
            //PDFManipulation pdfMan = new PDFManipulation("");
            //string textType = pdfMan.GetTextType(text);
            //return textType;

            if (string.IsNullOrEmpty(text)) return "";

            var temp = text.Split(new string[] { "/~/" }, StringSplitOptions.None);

            if ((temp != null) && (temp.Length > 1))
            {
                PDFManipulation pdfMan = new PDFManipulation("");
                string textType = pdfMan.GetTextType(temp[1], temp[2]);
                return textType;
            }
            return "";
        }

        [WebMethod]
        public static string GetParaName(string text)
        {
            if (string.IsNullOrEmpty(text)) return "";

            PDFManipulation pdfMan = new PDFManipulation("");
            string list_Comments = null;

            list_Comments = pdfMan.GetParaNameAsTooltip(text, SiteSession.MainCurrPage);

            return list_Comments;
        }

        [WebMethod]
        public static string GetErrorComments(string text)
        {
            if (text.Equals(""))
                return null;

            PDFManipulation pdfMan = new PDFManipulation("");
            string list_Comments = null;

            //if (pdfMan.GetTotalMistakes(SiteSession.MainCurrPage) > 0)
            //{
            list_Comments = pdfMan.GetComments(text, SiteSession.MainCurrPage);
            //}

            return list_Comments;
        }

        [WebMethod]
        public static string ShowComment_ByError(string text)
        {
            PDFManipulation pdfMan = new PDFManipulation("");
            string list_Comments = null;

            list_Comments = pdfMan.ShowComment(text, SiteSession.MainCurrPage);

            return list_Comments;
        }

        [WebMethod]
        public static string GetTestResult()
        {
            string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);

            if (!File.Exists(mainXmlPath)) return "0";

            StreamReader sr = new StreamReader(mainXmlPath);
            string xmlFile = sr.ReadToEnd();
            sr.Close();

            XmlDocument xmlDocOrigXml = new XmlDocument();
            xmlDocOrigXml.LoadXml(xmlFile);

            string email = Convert.ToString(HttpContext.Current.Session["ComparisonTestUser_Email"]);
            string testName = System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["SrcPDF"]));

            double detectedMistakes = xmlDocOrigXml.SelectNodes(@"//ln[@PDFmistake='']").Count;
            double totalMistakes = Convert.ToDouble(HttpContext.Current.Session["TotalMarks"]);

            double percentage = 0.0;

            HttpContext.Current.Session.Remove("TimeUpdator_StartTime");
            HttpContext.Current.Session.Remove("TimeUpdator_EndTime");

            percentage = Convert.ToDouble(detectedMistakes / totalMistakes) * 100;
            HttpContext.Current.Session["Result"] = detectedMistakes;

            return Convert.ToString(percentage);
        }

        [WebMethod]
        public static string GetQuizResult()
        {
            string resultMsg = "";
            web_Comparison obj = new web_Comparison();
            string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);

            StreamReader sr = new StreamReader(mainXmlPath);
            string xmlFile = sr.ReadToEnd();
            sr.Close();

            XmlDocument xmlDocOrigXml = new XmlDocument();
            xmlDocOrigXml.LoadXml(xmlFile);

            string quizType = Convert.ToString(HttpContext.Current.Session["quizType"]);

            string mistakeAttributeWordSpacing = @"//ln[@PDFmistake ='']";
            string mistakeAttributeSplitting = @"//ln[@PDFMergemistake ='']";
            string mistakeAttributeMerging = @"//ln[@PDFSplitmistake ='']";

            if (Convert.ToString(HttpContext.Current.Session["quizType"]) != "")
            {
                if (quizType.Equals("Splitting"))
                {
                    double splitMistakes = xmlDocOrigXml.SelectNodes(mistakeAttributeSplitting).Count;
                    if (splitMistakes > 0)
                    {
                        resultMsg = "Congradulations! You have got the split mistake";
                    }
                    else
                    {
                        resultMsg = "Sorry! You have not find the split mistake";
                    }
                }
                else if (quizType.Equals("Merging"))
                {
                    double mergeMistakes = xmlDocOrigXml.SelectNodes(mistakeAttributeMerging).Count;
                    if (mergeMistakes > 0)
                    {
                        resultMsg = "Congradulations! You have got the merge mistake";
                    }
                    else
                    {
                        resultMsg = "Sorry! You have not find the merge mistake";
                    }
                }
                else if (quizType.Equals("Space"))
                {
                    double wordSpaceMistakes = xmlDocOrigXml.SelectNodes(mistakeAttributeWordSpacing).Count;
                    if (wordSpaceMistakes > 0)
                    {
                        resultMsg = "Congradulations! You have got the word spacing mistake";
                    }
                    else
                    {
                        resultMsg = "Sorry! You have not find the word spacing mistake";
                    }
                }
            }

            else
            {
                resultMsg = "Sorry! There is some error in Quiz";
            }

            return resultMsg;
        }

        //[WebMethod]
        //public static string LogMistakes(string text)
        //{
        //    //List<string> selectedText = new List<string>();

        //    string selectedTextDivNumbers = "";

        //    //Get selected line text + whole page text separated by a string.
        //    var temp = text.Split(new string[] { "/~/" }, StringSplitOptions.None);

        //    if ((temp != null) && (temp.Length > 0))
        //    {
        //        //HttpContext.Current.Session["SelectedMistakeText"] = Convert.ToString(temp[0]);
        //        //Insert PDFmistake attribute in xml 
        //        Common.LogMistakesInXml(temp[0], temp[1]);
        //        PDFManipulation pdfMan = new PDFManipulation("");
        //        selectedTextDivNumbers = pdfMan.GetSelectedTextFromProducedPage(temp[1], temp[2], Convert.ToInt32(HttpContext.Current.Session["MainCurrPage"]));

        //        //selectedText.Add(Convert.ToString(temp[0]));
        //        //string srcPdfMistakeCoords = Common.GetPdfMistakeCoords(Convert.ToString(HttpContext.Current.Session["MainCurrPage"]),
        //        //                             Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]), selectedText, "sourcePdf");
        //        //DrawRectangle(srcPdfMistakeCoords);
        //    }
        //    return selectedTextDivNumbers;
        //}

        [WebMethod]
        public static string LogMistakes(string text)
        {
            string selectedTextDivNumbers = "";

            //Get selected line text + whole page text separated by a string.
            var temp = text.Split(new string[] { "/~/" }, StringSplitOptions.None);

            if ((temp != null) && (temp.Length > 0))
            {
                //Insert PDFmistake attribute in xml 
                selectedTextDivNumbers = Common.LogMistakesInXml(temp[0], temp[1], temp[2]);
                //PDFManipulation pdfMan = new PDFManipulation("");
                //selectedTextDivNumbers = pdfMan.GetSelectedTextFromProducedPage(temp[1], temp[2], Convert.ToInt32(HttpContext.Current.Session["MainCurrPage"]));
            }
            return selectedTextDivNumbers;
        }

        [WebMethod]
        public static string UndoMistakes(string text)
        {
            string selectedTextDivNumbers = "";

            //Get selected line text + whole page text separated by a string.
            var temp = text.Split(new string[] { "/~/" }, StringSplitOptions.None);

            if ((temp != null) && (temp.Length > 0))
            {
                //Undo PDFmistake attribute in xml 
                selectedTextDivNumbers = Common.UndoMistakesInXml(temp[0], temp[1], temp[2]);

                //status = "true";
                //PDFManipulation pdfMan = new PDFManipulation("");
                //selectedTextDivNumbers = pdfMan.GetSelectedTextFromProducedPage(temp[1], temp[2], Convert.ToInt32(HttpContext.Current.Session["MainCurrPage"]));
            }

            return selectedTextDivNumbers;
        }

        [WebMethod]
        public static void SaveTimeSpent(string text)
        {
            if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]) != "")
            {
                if (!(Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task")))
                    return;
            }

            double totalMinutes = Convert.ToDouble(text) / 60;

            var timeSpan = TimeSpan.FromMinutes(totalMinutes);
            int hour = timeSpan.Hours;
            int minute = timeSpan.Minutes;

            HttpContext.Current.Session["TimeSpent_ComparisonTask"] = Convert.ToString(hour) + ":" + Convert.ToString(minute);

            //DateTime today = DateTime.Now;
            //DateTime timeWithDate = today.Add(timeSpan);

            string currentStatus = "pause";

            web_ComparisonPreProcess cp = new web_ComparisonPreProcess();
            string timeWorked = cp.StopTask(false, currentStatus);
        }

        [WebMethod]
        public static int GetFinishBtnClickedCount(string bookId)
        {
            MyDBClass dbObj = new MyDBClass();
            int count = dbObj.GetFinishTaskClickedCount(bookId);
            int remainingTry = 0;

            if (count == 0) remainingTry = 3;
            else if (count == 1) remainingTry = 2;
            else if (count == 2) remainingTry = 1;
            else if (count == 3) remainingTry = 0;

            return remainingTry;
        }

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
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
using Outsourcing_System;

public partial class web_Comparison : System.Web.UI.Page
{
    MyDBClass objMyDBClass = new MyDBClass();

    void Page_PreInit(Object sender, EventArgs e)
    {
        //this.MasterPageFile = " ";
    }

    private int PageNum
    {
        set
        {
            SiteSession.MainCurrPage = value;
            this.txtPageNum.Text = SiteSession.MainCurrPage.ToString();
        }
        get
        {
            return SiteSession.MainCurrPage;
        }
    }

    private int ErrorIndex
    {
        set
        {
            SiteSession.CurrentErrorIndex = value;
            // this.lblErrorNum.Text = (SiteSession.CurrentErrorIndex + 1).ToString();
        }
        get
        {
            return SiteSession.CurrentErrorIndex;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //trNavigateMistake.Visible = false;

        if (!Page.IsPostBack)
        {
            if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
            {
                hfComparisonTaskType.Value = "onepagetest";
                //trNavigateMistake.Visible = false;

                //trComparisonTimeSpent.Visible = false;
                trComparisonTimeSpent.Attributes.Add("style", "display:none");

                trNavigateMistake.Visible = false;
                trcheckBoxes.Visible = false;
                btnFinish_Task.Visible = false;
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
                btnFinish_Task.Visible = true;
                divFinishQuiz.Visible = false;
                trTimer.Visible = true;

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

            int currentPage = Convert.ToInt32(Session["MainCurrPage"]);
            LoadNewPageInControl(currentPage);
        }
        else
        {
            if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
            {
                Timer();
            }
            else if (Convert.ToString(Session["ComparisonTask"]).Equals("comparisonEntryTest"))
            {
                Timer();
            }
        }
    }

    public void Timer()
    {
        Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "Timer()", true);
    }

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

    public void LoadNewPageInControl(int currentPage)
    {
        try
        {
            string srcPagePDFPath = "";

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

                this.CtrlPdfCmp.LoadTableText();
                setDefaultFont(currentPage);

                if (currentPage > 0)
                {
                    if ((Convert.ToString(Session["ComparisonTask"]).Equals("test")) || ((Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest")) ||
                        (Convert.ToString(Session["ComparisonTask"]).Equals("comparisonEntryTest"))))
                    {
                        if (GetTotalMistakes_Comparison0Test(currentPage) > 0)
                        {
                            GetComparison0TestMistakes(currentPage);
                        }
                    }
                    else
                    {
                        //If current page has any mistake on it then highlight function should be called which is inside GetMistakes function.
                        int totalErrors = GetTotalMistakes(currentPage);
                        if (totalErrors > 0)
                        {
                            GetMistakes(currentPage, srcPdfPath, prdPdfPath);
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

                        //int mistakeNumber = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @page=" + currentPage + "]").
                        //                    Cast<XmlNode>().Select(node => Convert.ToInt32(node.Attributes["PDFmistake"].Value)).Distinct().ToList().Min();

                        //if (mistakeNumber > 0)
                        //{
                        //    TextErrorNum.Text = Convert.ToString(mistakeNumber);
                        //}

                        var totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @page=" + currentPage + "]");

                        if ((totalMistakes != null) && (totalMistakes.Count > 0))
                            TextErrorNum.Text = totalMistakes[0].Attributes["PDFmistake"].Value;
                    }
                }

                txtPageNum.Text = Convert.ToString(currentPage);
                lblTotalPages.Text = Convert.ToString(Session["srcTotalPages"]);

                CtrlPdfCmp.ShowSrcPDFAsImageInControl(srcPdfPath, Convert.ToString(currentPage));
                CtrlPdfCmp.ShowPrdPDFInJSControl(Convert.ToString(currentPage));
            }
        }
        catch (Exception excep)
        {
            LogWritter.WriteLineInLog("Exception: " + excep.Message);
            //return false;
        }
    }

    public int GetTotalMistakes_Comparison0Test(int page)
    {
        StreamReader strreader = new StreamReader(Convert.ToString(Session["MainXMLFilePath"]));
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        int totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistakeTest!='' and @page=" + page + "]").Count;
        return totalMistakes;
    }

    public void GetComparison0TestMistakes(int page)
    {
        StreamReader strreader = new StreamReader(Convert.ToString(Session["MainXMLFilePath"]));
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        XmlNodeList pdfmistakes_Nodes = xmlDoc.SelectNodes(@"//*[@PDFmistakeTest!='' and @page=" + page + "]");
        List<string> selectedText = new List<string>();

        if (pdfmistakes_Nodes.Count > 0)
        {
            foreach (XmlNode node in pdfmistakes_Nodes)
            {
                selectedText.Add(node.InnerText);
            }
        }

        //To Do aamir
        //////string srcPdf = Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["srcPdfPagePath"]));
        //////string prdPdf = Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["prdPdfPagePath"]));

        ////string mainXmlPath = Convert.ToString(Session["MainXMLFilePath"]);

        //////string secondfilepath = LoadMistakePanel(Convert.ToString(page), mainXmlPath, selectedText, "producedPdf");
        ////string secondfilepath = Common.LoadMistakePanel(Convert.ToString(page), mainXmlPath, selectedText, "producedPdf");

        ////HttpContext.Current.Session["prdPdfPagePath"] = secondfilepath;

        //////To do
        //////ReplacePrdText(selectedText);

        //////string firstfilepath = LoadMistakePanel(Convert.ToString(page), mainXmlPath, selectedText, "sourcePdf");
        ////string firstfilepath = Common.LoadMistakePanel(Convert.ToString(page), mainXmlPath, selectedText, "sourcePdf");

        ////HttpContext.Current.Session["srcPdfPagePath"] = firstfilepath;
        //GetTotalMistakes_List();

        //HttpContext.Current.Response.Redirect("Comparison.aspx?mistake=1", true);
    }


    protected void btnGoTo_Click(object sender, EventArgs e)
    {
        int pNum = -1;
        int pageNum = 0;
        if (int.TryParse(this.txtPageNum.Text.Trim(), out pNum))
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
        PDFManipulation c = new PDFManipulation("");

        List<Mistakes> list = c.GetTotalMistakes_List(Convert.ToString(Session["MainXMLFilePath"]));
        int pdfPage = 0;

        foreach (var item in list)
        {
            if (item.mistakeNum == Convert.ToInt32(TextErrorNum.Text.Trim()))
            {
                pdfPage = item.page;
            }
        }

        if (!(pdfPage > 0))
            return;

        int eNum = -1;
        if (int.TryParse(this.TextErrorNum.Text.Trim(), out eNum))
        {
            //SiteSession.MainCurrPage = pdfPage;
            //this.CtrlPdfCmp.LoadTableText();

            //foreach (System.Web.UI.WebControls.ListItem li in cbxlFont.Items)
            //{
            //    li.Selected = true;
            //}

            //setDefaultFont(PageNum);

            Session["MainCurrPage"] = pdfPage;
            LoadNewPageInControl(pdfPage);
        }
        else
        {
            //Invalid
        }
    }



    public void GetMistakes(int page, string srcPdfPath, string prdPdfPath)
    {
        //StreamReader strreader = new StreamReader(SiteSession.MainXMLFilePath_PDF);
        //string xmlInnerText = strreader.ReadToEnd();
        //strreader.Close();

        //XmlDocument xmlDoc = new XmlDocument();
        //xmlDoc.LoadXml(xmlInnerText);

        Common comObj = new Common();
        XmlDocument xmlDoc = comObj.LoadXmlDocument(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));
        XmlNodeList pdfmistakes_Nodes = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @page=" + page + "]");
        List<string> selectedText = new List<string>();

        if (pdfmistakes_Nodes.Count > 0)
        {
            foreach (XmlNode node in pdfmistakes_Nodes)
            {
                selectedText.Add(node.InnerText);
            }
        }

        //string srcPdf = Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["srcPdfPagePath"]));
        //string prdPdf = Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["prdPdfPagePath"]));

        string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);
        int currentPage = SiteSession.MainCurrPage;

        //To do aamir 2016-06-18
        //string secondfilepath = Common.LoadMistakePanel(Convert.ToString(currentPage), mainXmlPath, selectedText, "producedPdf");

        //HttpContext.Current.Session["Highlighted_prdPdfPagePath"] = secondfilepath;

        ////To do
        //ReplacePrdText(selectedText);

        //string firstfilepath = Common.LoadMistakePanel(Convert.ToString(currentPage), mainXmlPath, selectedText, "sourcePdf");

        //HttpContext.Current.Session["Highlighted_srcPdfPagePath"] = firstfilepath;
    }

    public void GetSrcPageMistakes(int page, string srcPdfPath)
    {
        Common comObj = new Common();
        XmlDocument xmlDoc = comObj.LoadXmlDocument(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));
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


    public string LoadMistakePanel(string pageNumber, string mainXmlPath, List<string> originalText, string pdfType)
    {
        string directoryPath = Common.GetTaskFiles_SavingPath();

        string pdfPath = "";

        if (pdfType.Equals("producedPdf"))
        {
            pdfPath = directoryPath + Convert.ToString(HttpContext.Current.Session["Current_PrdPdfPage"]);
        }
        else
        {
            pdfPath = directoryPath + Convert.ToString(HttpContext.Current.Session["Current_SrcPdfPage"]);
        }

        List<PdfWord> wrdList = new List<PdfWord>();
        PdfWord wrd_Produced = null;

        string pdfName = Convert.ToString(HttpContext.Current.Session["pdfName"]);

        //string tetFilePath = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(pdfName) + "\\" + pageNumber + ".tetml";

        string tetFilePath = "";

        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
        {
            string pDirPath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
            string userDir_Path = pDirPath + "\\Tests\\" + Convert.ToString(HttpContext.Current.Session["CompTestUser_Email"]) + "/ComparisonTests/";

            tetFilePath = userDir_Path + "\\" + pageNumber + ".tetml";
        }
        else
        {
            //tetFilePath = ConfigurationManager.AppSettings["PDFDirPhyPath"] + "\\" + Path.GetFileNameWithoutExtension(pdfName).Replace("-1", "") + "\\" +
            //                     Path.GetFileNameWithoutExtension(pdfName) + "\\Comparison\\Comparison-" + Convert.ToString(HttpContext.Current.Session["comparisonType"]) + "\\" +
            //                     Convert.ToString(HttpContext.Current.Session["userId"]) + "\\" + pageNumber + ".tetml";

            //Session["Current_SrcPdfPage"] = "Produced_" + currentPage + ".pdf";
            //    Session["Current_PrdPdfPage"]

            tetFilePath = Common.GetTaskFiles_SavingPath() + Convert.ToString(HttpContext.Current.Session["Current_SrcPdfPage"]).Replace(".pdf", ".tetml");
        }

        XmlDocument tetDoc = new XmlDocument();
        try
        {
            StreamReader sr = new StreamReader(tetFilePath);
            string xmlText = sr.ReadToEnd();
            sr.Close();
            string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
            tetDoc.LoadXml(documentXML);
        }
        catch { }

        string llx = "";
        string lly = "";
        string urx = "";
        string ury = "";
        string temp = "";
        string urx_EndLine = "";
        string lly_EndLine = "";

        string coordinates = "";
        string word = "";
        string[] textToCheck = null;
        int counter = 0;

        foreach (var text in originalText)
        {
            XmlNodeList words = tetDoc.SelectNodes("//Word");
            XmlNodeList pages = tetDoc.SelectNodes("//Page");
            word = text.Replace(",", "");

            foreach (XmlNode page in pages)
            {
                textToCheck = ReplaceWhiteSpace_(word.Trim()).Split(',');
                XmlNodeList innerwords = page.SelectNodes("//Text");

                for (int i = 0; i < innerwords.Count; i++)
                {
                    if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 0))
                    {
                        if (innerwords[i].InnerText.Replace(",", "").Trim().Equals(textToCheck[0]))
                        {
                            //Calculating coordinates for Highlighting single word
                            if (textToCheck.Length == 1)
                            {
                                XmlNode boxNode = innerwords[i].NextSibling;
                                llx = boxNode.Attributes["llx"].Value;
                                lly = boxNode.Attributes["lly"].Value;
                                urx = boxNode.Attributes["urx"].Value;
                                ury = boxNode.Attributes["ury"].Value;

                                coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

                                wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1, innerwords[i].InnerText + innerwords[i + 1].InnerText + innerwords[i + 2].InnerText, coordinates));
                                counter++;
                            }//end

                            //Calculating right end side line x coordinate for highlighting whole line
                            else if (textToCheck.Length > 1)
                            {
                                for (int j = i; j < innerwords.Count; j++)
                                {
                                    XmlNode boxNode = innerwords[j].NextSibling;
                                    lly_EndLine = boxNode.Attributes["lly"].Value;
                                    urx_EndLine = boxNode.Attributes["urx"].Value;

                                    if (temp != lly_EndLine)
                                    {
                                        if (innerwords[j - 1] != null)
                                        {
                                            XmlNode boxNode_Endline = innerwords[j - 1].NextSibling;

                                            urx_EndLine = boxNode_Endline.Attributes["urx"].Value;

                                            if (temp != "")
                                                break;
                                        }
                                    }
                                    temp = lly_EndLine;
                                }
                            }//end

                            if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 1))
                            {
                                if ((i + 1) < innerwords.Count)
                                {
                                    if (innerwords[i + 1].InnerText.Replace(",", "").Trim().Equals(textToCheck[1]))
                                    {
                                        //Calculating coordinates for Highlighting 2 words
                                        if (textToCheck.Length == 2)
                                        {
                                            XmlNode boxNode = innerwords[i].NextSibling;
                                            llx = boxNode.Attributes["llx"].Value;
                                            lly = boxNode.Attributes["lly"].Value;
                                            urx = urx_EndLine;
                                            ury = boxNode.Attributes["ury"].Value;

                                            coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

                                            wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1, innerwords[i].InnerText + innerwords[i + 1].InnerText + innerwords[i + 2].InnerText, coordinates));
                                            counter++;
                                        }//end

                                        if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 2))
                                        {
                                            if (innerwords[i + 2].InnerText.Replace(",", "").Trim().Equals(textToCheck[2]))
                                            {
                                                XmlNode boxNode = innerwords[i].NextSibling;
                                                llx = boxNode.Attributes["llx"].Value;
                                                lly = boxNode.Attributes["lly"].Value;
                                                urx = urx_EndLine;
                                                ury = boxNode.Attributes["ury"].Value;

                                                coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

                                                wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1, innerwords[i].InnerText + innerwords[i + 1].InnerText + innerwords[i + 2].InnerText, coordinates));
                                                counter++;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    //if (originalText.Count == counter)
                    //    break;
                }//end outer for loop
            }//end outer foreach loop
        }

        //string pdfFilePath = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(pdfName) + "\\" + pageNumOriginal + ".pdf";
        return SelectCurrentWordInPDFWithAnnotation(pdfPath, wrdList);
    }

    static string ReplaceWhiteSpace_(string input)
    {
        StringBuilder output = new StringBuilder(input.Length);

        for (int index = 0; index < input.Length; index++)
        {
            if (!Char.IsWhiteSpace(input, index))
            {
                output.Append(input[index]);
            }
            else
            {
                output.Append(",");
            }
        }

        return output.ToString();
    }

    private static string SelectCurrentWordInPDFWithAnnotation(string pdfFilePath, List<PdfWord> wrd)
    {
        //if ((Convert.ToString(HttpContext.Current.Session["srcPdfPagePath"]) == "") || (wrd.Count == 0))
        //    return pdfFilePath;

        int page = wrd[0].PageNumber;

        ////string savePath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
        ////string pdfFile = Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["pdfName"]));

        ////string stampedSrcPdf = "";
        ////string stampePrddPdf = "";

        ////string srcPDFPath = Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["srcPdfPagePath"]));
        ////string prdPDFPath = Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["prdPdfPagePath"]));


        ////stampedSrcPdf = savePath + "\\" + pdfFile + "\\" + srcPDFPath + ".pdf";
        ////stampePrddPdf = savePath + "\\" + pdfFile + "\\" + prdPDFPath + ".pdf";



        ////string srcPdfFile = Path.GetFileNameWithoutExtension(Convert.ToString(Session["srcPdfPagePath"]).Split('\\')[5]);
        ////string[] temp = Convert.ToString(Session["srcPdfPagePath"]).Split('\\');
        ////string stampedSrcPdf = temp[0] + "\\" + temp[1] + "\\" + temp[2] + "\\" + temp[3] + "\\" + temp[4] + "\\" + srcPdfFile + "_Stamped.pdf";

        ////string prdPdfFile = Path.GetFileNameWithoutExtension(Convert.ToString(Session["prdPdfPagePath"]).Split('\\')[5]);
        ////string[] temp1 = Convert.ToString(Session["prdPdfPagePath"]).Split('\\');
        ////string stampePrddPdf = temp[0] + "\\" + temp[1] + "\\" + temp[2] + "\\" + temp[3] + "\\" + temp[4] + "\\" + prdPdfFile + "_Stamped.pdf";

        ////string srcPDFPath = "";
        ////string prdPDFPath = "";

        //if (File.Exists(stampedSrcPdf))
        //{
        //    srcPDFPath = stampedSrcPdf;
        //}

        //if (File.Exists(stampePrddPdf))
        //{
        //    prdPDFPath = stampePrddPdf;
        //}

        //else
        //{
        //srcPDFPath = Convert.ToString(HttpContext.Current.Session["srcPdfPagePath"]).Replace("_Stamped", "");
        //prdPDFPath = Convert.ToString(HttpContext.Current.Session["prdPdfPagePath"]).Replace("_Stamped", "");
        ////}


        //if (Path.GetFileNameWithoutExtension(pdfFilePath).Contains("Produced_"))
        //{
        //    pdfFilePath = prdPDFPath;
        //}
        //else
        //{
        //    pdfFilePath = srcPDFPath;
        //}

        PDFManipulation pdfMan = new PDFManipulation(pdfFilePath);

        string extractedFile = pdfMan.ExtractPageWithAnnotation(SiteSession.MainCurrPage);

        string highlightedfilePath = pdfMan.HighlightWord(extractedFile, wrd);
        //File.Delete(extractedFile);//aamir
        return highlightedfilePath;
    }
    public int GetTotalMistakes(int page)
    {
        StreamReader strreader = new StreamReader(Convert.ToString(Session["MainXMLFilePath"]));
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        int totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @page=" + page + "]").Count;
        return totalMistakes;
    }

    private int GetTotalMistakes(string xmlPath)
    {
        StreamReader strreader = new StreamReader(xmlPath);
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        int totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistake]").Count;

        return totalMistakes;
    }

    protected void LoadComparisonSummary()
    {
        //this.lblTotalErrorPages.Text = this.lblTotalMisMatches.Text = SiteSession.ErrorList.Count.ToString();
        //this.lblSourcePDFPages.Text = SiteSession.PDFManFileObj.TotalPages.ToString();
        //this.lblRHYWPages.Text = SiteSession.RHYWManFileObj.TotalPages.ToString();
    }

    protected void btnDialogOK_Click(object sender, EventArgs e)
    {
        //LoadComparisonPanel();
    }



    protected void LoadComparisonPanel()
    {

        if (SiteSession.errorHl != null)
        {
            int errorCount = SiteSession.errorHl.ErrorCount;

            this.lblTotalErrors.Text = errorCount.ToString();//aamir
            this.lblTotalPages.Text = SiteSession.AllSeperatePages.Count.ToString();

            //this.lblTotalPages.Text = SiteSession.AllSeperatePages.Count.ToString();
            //this.lblNavigationTotalErrors.Text = SiteSession.AllSeperatePages.Count.ToString();

            if (errorCount == 0)
            {
                // this.lblError.Visible = true;
            }
            MisMatchError mmeNew;

            if (errorCount > 0)
            {

                ErrorIndex = 0;
                TextErrorNum.Text = "1";//aamir
                mmeNew = (MisMatchError)SiteSession.errorHl.GetErrorAtIndex(ErrorIndex);
                PdfWord wrd1 = mmeNew.list1Word;
                PageNum = wrd1.PageNumber;
                string srcPDFPath = mmeNew.pdfSourcePath;
                string prdPDFPath = mmeNew.pdfProducedPath;
                string xmlPath = mmeNew.xmlPath;
                CtrlPdfCmp.MisMatchComments = mmeNew.misMatch;
                CtrlPdfCmp.SrcPDFPath = srcPDFPath;
                CtrlPdfCmp.ProdPDFPath = prdPDFPath;
                CtrlPdfCmp.XMLFilePath = xmlPath;
                CtrlPdfCmp.CurrPageNumber = PageNum;
                CtrlPdfCmp.LoadFiles(true);
            }
        }
    }

    protected void LoadMistakePanel()
    {
        setDefaultFont(PageNum);

        string srcPDFPath = Convert.ToString(Session["srcPdfPagePath"]);
        string prdPDFPath = Convert.ToString(Session["prdPdfPagePath"]);
        string xmlPath = Convert.ToString(Session["savedXMLPath"]);

        CtrlPdfCmp.SrcPDFPath = srcPDFPath;
        CtrlPdfCmp.ProdPDFPath = prdPDFPath;
        CtrlPdfCmp.XMLFilePath = xmlPath;
        CtrlPdfCmp.CurrPageNumber = PageNum;
        CtrlPdfCmp.LoadFilesWithoutMatching(true);

        if (Convert.ToString(Session["MainXMLFilePath"]) == "")
            return;
    }

    protected void PDFComparison_ErrorsComplete(object sender, CommandEventArgs e)
    {
        MisMatchError mme = (MisMatchError)SiteSession.errorHl.GetErrorAtIndex(ErrorIndex);
        MisMatch misMatchObj = mme.misMatch;
        misMatchObj = CtrlPdfCmp.MisMatchComments;
        int errorCount = SiteSession.errorHl.ErrorCount;
        if (ErrorIndex < errorCount - 1)
            ErrorIndex++;
        else
        {
            lblMsg.Text = "All Errors Logged, Please Generate Report";
            trNav.Visible = false;
            CtrlPdfCmp.HideComments();
            return;
            //All Errors Complete you can generate the report
        }
        int counter = ErrorIndex;
        MisMatchError mmeNew;
        mmeNew = (MisMatchError)SiteSession.errorHl.GetErrorAtIndex(counter);
        if (SiteSession.CurrentErrorIndex < errorCount)
        {
            PdfWord wrd1 = mmeNew.list1Word;
            PageNum = wrd1.PageNumber;
            string srcPDFPath = mmeNew.pdfSourcePath;
            string prdPDFPath = mmeNew.pdfProducedPath;
            string xmlPath = mmeNew.xmlPath;
            CtrlPdfCmp.SrcPDFPath = srcPDFPath;
            CtrlPdfCmp.ProdPDFPath = prdPDFPath;
            CtrlPdfCmp.XMLFilePath = xmlPath;
            CtrlPdfCmp.MisMatchComments = mmeNew.misMatch;
            CtrlPdfCmp.LoadFiles(true);
        }
    }

    protected void PDFComparison_FileEdit(object sender, CommandEventArgs e)
    {
        string pDirPath = ConfigurationManager.AppSettings["HighlightDirPP"];
        string vDirPath = ConfigurationManager.AppSettings["HighlightDirVP"];
        string newPDFPath = e.CommandArgument.ToString();

        String fileName = System.IO.Path.GetFileName(newPDFPath);
        String dir = Directory.GetParent(newPDFPath).Name;


        //string pdfFile = Path.GetFileName(newPDFPath);

        string phyFilePath = pDirPath + "\\" + dir + "\\" + fileName;

        int counter = ErrorIndex;
        //ArrayList errorList = SiteSession.ErrorList;

        //Word wrd1 = ((MisMatchError)errorList[counter]).list1Word;
        MisMatchError mmeNew = (MisMatchError)SiteSession.errorHl.GetErrorAtIndex(ErrorIndex);
        mmeNew.commetns = SiteSession.PDFViewerComments;

        PdfWord wrd1 = mmeNew.list1Word;
        PageNum = wrd1.PageNumber;

        MisMatchError mmeNewCopied = new MisMatchError();
        mmeNewCopied = mmeNew;
        SiteSession.errorHl.FindErrorAt(phyFilePath, ErrorIndex);

        int currPage = SiteSession.MainCurrPage;
        ((SeperatePages)SiteSession.AllSeperatePages[PageNum - 1]).ProdPDFPath = phyFilePath;

        if (SiteSession.PDFViewerComments != null && (SiteSession.PDFViewerComments.Trim().Length > 0))
        {
            SiteSession.ReportListForComments.Add(mmeNewCopied);
        }

        //  SiteSession.errorHl.removeError(counter); //skipped this line
        LoadComparisonPanel();
    }

    protected void btnPrev_Click(object sender, EventArgs e)
    {
        LoadPrevError();
        //PageNum -= 1;
        //LoadNewPageInControl(PageNum);
    }

    protected void btnNext_Click(object sender, EventArgs e)
    {
        LoadNextError();
        //PageNum += 1;
        //LoadNewPageInControl(PageNum);
    }

    private void LoadErrorNumberwise(int errorNumber)
    {
        int errorCount = SiteSession.errorHl.ErrorCount;
        if (errorNumber > errorCount)
        {
            CtrlPdfCmp.RetainBothPDFs();
            return;
        }
        ErrorIndex = errorNumber - 1;
        MisMatchError mme = (MisMatchError)SiteSession.errorHl.GetErrorAtIndex(errorNumber - 1);
        MisMatch misMatchObj = mme.misMatch;
        if (misMatchObj != null)
            misMatchObj.misMatchType = CtrlPdfCmp.CurrentMisMatch;
        if (SiteSession.CurrentErrorIndex < errorCount)
        {
            PdfWord wrd1 = mme.list1Word;
            PageNum = wrd1.PageNumber;
            string srcPDFPath = mme.pdfSourcePath;
            string prdPDFPath = mme.pdfProducedPath;
            string xmlPath = mme.xmlPath;
            CtrlPdfCmp.SrcPDFPath = srcPDFPath;
            CtrlPdfCmp.ProdPDFPath = prdPDFPath;
            CtrlPdfCmp.XMLFilePath = xmlPath;
            CtrlPdfCmp.CurrPageNumber = PageNum;
            CtrlPdfCmp.MisMatchComments = mme.misMatch;
            CtrlPdfCmp.LoadFiles(true);
        }
        else
        {
            lblMsg.Text = "All Errors Finished, you can Generate Report";
        }
    }

    private void LoadNextError()
    {
        //ArrayList errorList = SiteSession.ErrorList;
        int errorCount = SiteSession.errorHl.ErrorCount;
        if (ErrorIndex >= (errorCount - 1))
        {
            CtrlPdfCmp.RetainBothPDFs();
            return;
        }

        //MisMatch misMatchObj = ((MisMatchError)errorList[ErrorIndex]).misMatch;
        MisMatchError mme = (MisMatchError)SiteSession.errorHl.GetErrorAtIndex(ErrorIndex);
        MisMatch misMatchObj = mme.misMatch;
        if (misMatchObj != null)
            misMatchObj.misMatchType = CtrlPdfCmp.CurrentMisMatch;
        ErrorIndex++;
        int counter = ErrorIndex;
        MisMatchError mmeNew;
        mmeNew = (MisMatchError)SiteSession.errorHl.GetErrorAtIndex(counter);
        if (SiteSession.CurrentErrorIndex < errorCount)
        {
            //Word wrd1 = ((MisMatchError)errorList[counter]).list1Word;
            PdfWord wrd1 = mmeNew.list1Word;

            PageNum = wrd1.PageNumber;
            string srcPDFPath = mmeNew.pdfSourcePath;
            string prdPDFPath = mmeNew.pdfProducedPath;
            string xmlPath = mmeNew.xmlPath;
            CtrlPdfCmp.SrcPDFPath = srcPDFPath;
            CtrlPdfCmp.ProdPDFPath = prdPDFPath;
            CtrlPdfCmp.XMLFilePath = xmlPath;
            CtrlPdfCmp.CurrPageNumber = PageNum;
            CtrlPdfCmp.MisMatchComments = mmeNew.misMatch;
            CtrlPdfCmp.LoadFiles(true);
        }
        else
        {
            lblMsg.Text = "All Errors Finished, you can Generate Report";
        }
    }

    private void LoadPrevError()
    {
        if (ErrorIndex == 0)
        {
            CtrlPdfCmp.RetainBothPDFs();
            return;
        }
        //ArrayList errorList = SiteSession.ErrorList;
        int errorCount = SiteSession.errorHl.ErrorCount;

        //MisMatch misMatchObj = ((MisMatchError)errorList[ErrorIndex]).misMatch;
        MisMatchError mme = (MisMatchError)SiteSession.errorHl.GetErrorAtIndex(ErrorIndex);
        MisMatch misMatchObj = mme.misMatch;

        if (misMatchObj != null)
            misMatchObj.misMatchType = CtrlPdfCmp.CurrentMisMatch;

        ErrorIndex--;
        int counter = ErrorIndex;
        MisMatchError mmeNew;
        mmeNew = (MisMatchError)SiteSession.errorHl.GetErrorAtIndex(counter);
        if (SiteSession.CurrentErrorIndex < errorCount && SiteSession.CurrentErrorIndex >= 0)
        {
            //Word wrd1 = ((MisMatchError)errorList[counter]).list1Word;
            PdfWord wrd1 = mmeNew.list1Word;
            PageNum = wrd1.PageNumber;
            string srcPDFPath = mmeNew.pdfSourcePath;
            string prdPDFPath = mmeNew.pdfProducedPath;
            string xmlPath = mmeNew.xmlPath;
            CtrlPdfCmp.SrcPDFPath = srcPDFPath;
            CtrlPdfCmp.ProdPDFPath = prdPDFPath;
            CtrlPdfCmp.XMLFilePath = xmlPath;
            CtrlPdfCmp.CurrPageNumber = PageNum;
            CtrlPdfCmp.MisMatchComments = mmeNew.misMatch;
            CtrlPdfCmp.LoadFiles(true);
        }
        else
        {
            lblMsg.Text = "All Errors Finished, you can Generate Report";
            //All Errors Complete you can generate the report
        }
    }



    protected void btnGenReport_Click(object sender, EventArgs e)
    {
        string reportFilePath = CtrlPdfCmp.PrintList();
        FileStream fs = new FileStream(reportFilePath, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);
        byte[] fileBytes = br.ReadBytes((int)fs.Length);
        string file = reportFilePath;
        Response.AddHeader("Content-disposition", "attachment; filename=Report_" + reportFilePath);
        Response.ContentType = "application/vnd.ms-excel";
        Response.BinaryWrite(fileBytes);
        Response.End();
    }

    protected void btnAddError_Click(object sender, EventArgs e)
    {
        string reportFilePath = CtrlPdfCmp.PrintList();
        FileStream fs = new FileStream(reportFilePath, FileMode.Open);
        BinaryReader br = new BinaryReader(fs);
        byte[] fileBytes = br.ReadBytes((int)fs.Length);
        string file = reportFilePath;
        Response.AddHeader("Content-disposition", "attachment; filename=Report_" + reportFilePath);
        Response.ContentType = "application/vnd.ms-excel";
        Response.BinaryWrite(fileBytes);
        Response.End();
    }

    //protected void btnGoTo_Click(object sender, EventArgs e)
    //{
    //    int pNum = -1;
    //    if (int.TryParse(this.txtPageNum.Text.Trim(), out pNum))
    //    {
    //        //SiteSession.MainCurrPage = pNum;
    //        PageNum = pNum;

    //        SiteSession.MainCurrPage = PageNum;

    //        if (Convert.ToString(Session["ComparisonTest"]).Equals("1"))
    //        {
    //            string pDirPath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
    //            string userDir_Path = pDirPath + "\\Tests\\" + Convert.ToString(Session["CompTestUser_Email"]) + "/ComparisonTests/";

    //            Session["srcPdfPagePath"] = userDir_Path + "/" + PageNum + ".pdf";
    //            Session["prdPdfPagePath"] = userDir_Path + "/" + "Produced_" + PageNum + ".pdf";
    //        }
    //        else
    //        {
    //            Session["srcPdfPagePath"] = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"])) + "\\" + +PageNum + ".pdf";
    //            Session["prdPdfPagePath"] = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"])) + "\\" + "Produced_" + PageNum + ".pdf";
    //        }

    //        this.CtrlPdfCmp.LoadTableText();
    //        setDefaultFont(PageNum);

    //        LoadNewPageInControl(PageNum);

    //        PDFManipulation pdfMan = new PDFManipulation(Convert.ToString(Session["srcPdfPagePath"]));

    //        string mistakeNum = pdfMan.GetMistakeByPage(PageNum);

    //        if (mistakeNum != null)
    //            TextErrorNum.Text = mistakeNum;
    //    }
    //    else
    //    {
    //        //Invalid
    //    }
    //}

    //////public void UpdateTopMargin_Offset(int page)
    //////{
    //////    double topOffset = 0;

    //////    //Get top yCoordinate value from xml
    //////    string CurrentXMLFilePath = Convert.ToString(Session["prdPdfPagePath"]).Replace(".pdf", ".xml");
    //////    StreamReader strreader = new StreamReader(CurrentXMLFilePath);
    //////    string xmlInnerText = strreader.ReadToEnd();
    //////    strreader.Close();

    //////    XmlDocument xmlDoc = new XmlDocument();
    //////    xmlDoc.LoadXml(xmlInnerText);

    //////    var list_LinesTop = xmlDoc.SelectNodes(@"//*[@top]");
    //////    List<double> line_Top = new List<double>();

    //////    if (list_LinesTop != null)
    //////    {
    //////        if (list_LinesTop.Count > 0)
    //////        {
    //////            foreach (XmlElement value in list_LinesTop)
    //////            {
    //////                line_Top.Add(Convert.ToDouble(value.Attributes["top"].Value));
    //////                break;
    //////            }
    //////        }
    //////    }

    //////    //Get top yCoordinate value from tetml output of first page of source pdf
    //////    string prdPdfPagePath = Common.GetProducePdf(page);
    //////    string tetFilePath = Common.Createtetml(prdPdfPagePath);

    //////    XmlDocument tetDoc = new XmlDocument();
    //////    try
    //////    {
    //////        StreamReader sr = new StreamReader(tetFilePath);
    //////        string xmlText = sr.ReadToEnd();
    //////        sr.Close();
    //////        string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
    //////        tetDoc.LoadXml(documentXML);
    //////    }
    //////    catch { }

    //////    string lly = "";

    //////    XmlNodeList words = tetDoc.SelectNodes("//Word");
    //////    XmlNodeList pages = tetDoc.SelectNodes("//Page");

    //////    foreach (XmlNode yCoord in pages)
    //////    {
    //////        XmlNodeList innerwords = yCoord.SelectNodes("//Text");

    //////        for (int i = 0; i < innerwords.Count; i++)
    //////        {
    //////            XmlNode boxNode = innerwords[i].NextSibling;
    //////            lly = boxNode.Attributes["lly"].Value;
    //////            break;
    //////        }
    //////    }

    //////    topOffset = line_Top[0] - Convert.ToDouble(lly);

    //////    if (!CheckPageTop(Convert.ToString(page)))
    //////    {
    //////        //Update main xml file with changed top values
    //////        string mainXmlPath = Convert.ToString(Session["MainXMLFilePath"]);
    //////        StreamReader sr_Update = new StreamReader(mainXmlPath);
    //////        string xmlFile = sr_Update.ReadToEnd();
    //////        sr_Update.Close();
    //////        XmlDocument xmlDocOrigXml = new XmlDocument();
    //////        xmlDocOrigXml.LoadXml(xmlFile);

    //////        SiteSession.xmlDoc = xmlDocOrigXml;

    //////        XmlNodeList nodes = xmlDocOrigXml.SelectNodes(@"//*[@top!='' and @page=" + page + "]");

    //////        foreach (XmlNode node in nodes)
    //////        {
    //////            if (node != null)
    //////            {
    //////                node.Attributes["top"].Value = Convert.ToString(Convert.ToDouble(node.Attributes["top"].Value) - Convert.ToDouble(topOffset));
    //////            }
    //////        }

    //////        xmlDocOrigXml.Save(mainXmlPath);

    //////        //GlobalVar.XMLPath = mainXmlPath;

    //////        XmlDocument pageXML = Common.GetPageXmlDoc(page.ToString());
    //////        string dirPath = ConfigurationManager.AppSettings["HighlightDirPP"];
    //////        string pageXMLSavedPath = dirPath + "\\" + Convert.ToString(Session["pdfName"]).Replace(".pdf", "") +
    //////                                  "\\Produced_" + page + ".xml";
    //////        pageXML.Save(pageXMLSavedPath);

    //////        Session["XmlPages_TopCorrection"] = Convert.ToString(Session["XmlPages_TopCorrection"]) + "," + page;
    //////    }
    //////}

    public bool CheckPageTop(string page)
    {
        if (Convert.ToString(Session["XmlPages_TopCorrection"]) != "")
        {
            var pageNum = Convert.ToString(Session["XmlPages_TopCorrection"]).Split(',');

            foreach (var item in pageNum)
            {
                if (item.Trim().Equals(page))
                {
                    return true;
                }
            }
        }
        return false;
    }

    //public string GetTetmlMargin(string tetmlType)
    //{
    //    string savePath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
    //    string mainPdfFile = Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"]));
    //    string pdfFile = "";
    //    string lly = "";
    //    string tetFilePath = "";
    //    double pdf_Img_YPosition = 0;
    //    double maxYCoord = 0;

    //    List<double> list_YCoord = new List<double>();

    //    if (tetmlType.Equals("sourcepdf"))
    //    {
    //        pdfFile = Path.GetFileNameWithoutExtension(Convert.ToString(Session["srcPdfPagePath"]));
    //    }
    //    else
    //    {
    //        pdfFile = Path.GetFileNameWithoutExtension(Convert.ToString(Session["prdPdfPagePath"]));
    //    }

    //    tetFilePath = savePath + mainPdfFile + "\\" + pdfFile + ".tetml";

    //    XmlDocument tetDoc = new XmlDocument();
    //    try
    //    {
    //        StreamReader sr = new StreamReader(tetFilePath);
    //        string xmlText = sr.ReadToEnd();
    //        sr.Close();
    //        string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
    //        tetDoc.LoadXml(documentXML);
    //    }
    //    catch { }

    //    XmlNodeList words = tetDoc.SelectNodes("//Word");
    //    XmlNodeList pages = tetDoc.SelectNodes("//Page");

    //    foreach (XmlNode line in pages)
    //    {
    //        XmlNodeList innerwords = line.SelectNodes("//Text");

    //        for (int i = 0; i < innerwords.Count; i++)
    //        {
    //            XmlNode boxNode = innerwords[i].NextSibling;
    //            lly = boxNode.Attributes["lly"].Value;
    //            list_YCoord.Add(Convert.ToDouble(lly));
    //        }
    //    }

    //    var list_Lines = from element in list_YCoord
    //                     orderby element descending
    //                     select element;

    //    if (list_YCoord.Count > 0)
    //    {
    //        maxYCoord = list_YCoord.Max();
    //    }

    //    foreach (XmlNode image in pages)
    //    {
    //        XmlNodeList images = image.SelectNodes("//PlacedImage");

    //        for (int i = 0; i < images.Count; i++)
    //        {
    //            if (images[i].Attributes["image"].Value.Equals("I1"))
    //            {
    //                pdf_Img_YPosition = Convert.ToDouble(images[i].Attributes["y"].Value);
    //                break;
    //            }
    //        }
    //    }

    //    return Convert.ToString(maxYCoord) + "," + Convert.ToString(pdf_Img_YPosition);
    //}

    public List<Double> GetTetmlTextMargin(string tetmlType)
    {
        //string savePath = Common.GetDirectoryPath();
        string pdfFile = "";
        string lly = "";
        string tetFilePath = "";
        double pdf_Img_YPosition = 0;
        double maxYCoord = 0;

        List<double> list_YCoord = new List<double>();
        List<double> list_ImageCoord = new List<double>();

        if (tetmlType.Equals("sourcepdf"))
        {
            pdfFile = System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(Session["Current_SrcPdfPage"]));
        }
        else
        {
            pdfFile = System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(Session["Current_PrdPdfPage"]));
        }

        //tetFilePath = savePath + mainPdfFile + "\\" + pdfFile + ".tetml";

        //tetFilePath = savePath + Path.GetFileNameWithoutExtension(Convert.ToString(Session["Book_Id"])).Replace("-1", "") + "\\" +
        //                    Path.GetFileNameWithoutExtension(Convert.ToString(Session["Book_Id"])) + "-1\\Comparison\\Comparison-" +
        //                    Convert.ToString(Session["comparisonType"]) + "\\" + Convert.ToString(Session["userId"]) + "\\" + pdfFile + ".tetml";

        if (Convert.ToString(Session["ComparisonTask"]) != "")
        {
            if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
            {
                tetFilePath = Common.GetTestFiles_SavingPath() + pdfFile + ".tetml";
            }
            else if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
            {
                tetFilePath = Common.GetOnePageTestFiles_SavingPath() + pdfFile + ".tetml";
            }
            else if (Convert.ToString(Session["ComparisonTask"]).Equals("task"))
            {
                tetFilePath = Common.GetTaskFiles_SavingPath() + pdfFile + ".tetml";
            }
        }

        XmlDocument tetDoc = new XmlDocument();
        try
        {
            StreamReader sr = new StreamReader(tetFilePath);
            string xmlText = sr.ReadToEnd();
            sr.Close();
            string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
            tetDoc.LoadXml(documentXML);
        }
        catch { }

        XmlNodeList words = tetDoc.SelectNodes("//Word");
        XmlNodeList pages = tetDoc.SelectNodes("//Page");

        foreach (XmlNode line in pages)
        {
            XmlNodeList innerwords = line.SelectNodes("//Text");

            for (int i = 0; i < innerwords.Count; i++)
            {
                XmlNode boxNode = innerwords[i].NextSibling;
                lly = boxNode.Attributes["lly"].Value;
                list_YCoord.Add(Convert.ToDouble(lly));
            }
        }

        var list_Lines = (from element in list_YCoord
                          orderby element descending
                          select element).Distinct().ToList();

        return list_Lines;
    }

    public List<string> GetTetmlImageMargin(string tetmlType)
    {
        //string savePath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
        //string mainPdfFile = Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"]));
        string pdfFile = "";
        string tetFilePath = "";
        double pdf_Img_YPosition = 0;
        double maxYCoord = 0;

        List<string> list_ImageCoord = new List<string>();

        if (tetmlType.Equals("sourcepdf"))
        {
            pdfFile = System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(Session["Current_SrcPdfPage"]));
        }
        else
        {
            pdfFile = System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(Session["Current_PrdPdfPage"]));
        }

        //tetFilePath = savePath + mainPdfFile + "\\" + pdfFile + ".tetml";
        //tetFilePath = savePath + Path.GetFileNameWithoutExtension(Convert.ToString(Session["Book_Id"])).Replace("-1", "") + "\\" +
        //Path.GetFileNameWithoutExtension(Convert.ToString(Session["Book_Id"])) + "-1\\Comparison\\Comparison-" +
        //Convert.ToString(Session["comparisonType"]) + "\\" + Convert.ToString(Session["userId"]) + "\\" + pdfFile + ".tetml";

        if (Convert.ToString(Session["ComparisonTask"]) != "")
        {
            if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
            {
                tetFilePath = Common.GetTestFiles_SavingPath() + pdfFile + ".tetml";
            }
            else if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
            {
                tetFilePath = Common.GetOnePageTestFiles_SavingPath() + pdfFile + ".tetml";
            }
            else if (Convert.ToString(Session["ComparisonTask"]).Equals("task"))
            {
                tetFilePath = Common.GetTaskFiles_SavingPath() + pdfFile + ".tetml";
            }
        }

        XmlDocument tetDoc = new XmlDocument();
        try
        {
            StreamReader sr = new StreamReader(tetFilePath);
            string xmlText = sr.ReadToEnd();
            sr.Close();
            string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
            tetDoc.LoadXml(documentXML);
        }
        catch { }

        XmlNodeList pages = tetDoc.SelectNodes("//Page");

        foreach (XmlNode line in pages)
        {
            XmlNodeList images = line.SelectNodes("//PlacedImage");

            for (int i = 0; i < images.Count; i++)
            {
                list_ImageCoord.Add(images[i].Attributes["x"].Value + "," + images[i].Attributes["y"].Value);
            }
        }

        var list_Lines = (from element in list_ImageCoord
                          orderby element.ToString().Split(',')[1] descending
                          select element).Distinct().ToList();

        return list_Lines;
    }

    public string UpdateTopMargin(int page)
    {
        string tetFilePath = "";
        double topOffset = 0;
        double imgOffset = 0;

        List<double> srcPdfValues = GetTetmlTextMargin("sourcepdf");
        //string[] topOffsetSrc_Temp = srcPdfValues.Split(',');

        List<double> prdPdfValues = GetTetmlTextMargin("producedpdf");

        if (prdPdfValues.Count == 0)
            return null;

        List<string> list_LineMargin = new List<string>();

        if (((srcPdfValues != null) && (srcPdfValues.Count > 0)) && (prdPdfValues != null) && (prdPdfValues.Count > 0))
        {
            for (int i = 0; i < prdPdfValues.Count; i++)
            {
                if (i < srcPdfValues.Count)
                {
                    list_LineMargin.Add((i + 1) + "," + Convert.ToString(srcPdfValues[i] - prdPdfValues[i]));
                }
            }
        }

        var tt = list_LineMargin;

        if (list_LineMargin.Count == 0)
            return null;

        //Get x and y coordinates of images from both source and produced tetml 
        List<string> srcPdfImgValues = GetTetmlImageMargin("sourcepdf");
        List<string> prdPdfImgValues = GetTetmlImageMargin("producedpdf");
        List<string> list_ImgMargin = new List<string>();
        List<string> srcPdfImgValues_New = new List<string>();

        //foreach (var prdImg in prdPdfImgValues)
        //{
        //    foreach (var srcImg in srcPdfImgValues)
        //    {
        //        if (srcImg.ToString().Split(',')[0].Equals(prdImg.ToString().Split(',')[0]))
        //        {
        //            srcPdfImgValues_New.Add(srcImg);
        //        }
        //    }
        //}

        //if (((srcPdfImgValues_New != null) && (srcPdfImgValues_New.Count > 0)) && ((prdPdfImgValues != null) && (prdPdfImgValues.Count > 0)))
        //{
        //    for (int i = 0; i < prdPdfImgValues.Count; i++)
        //    {
        //        list_ImgMargin.Add((i + 1) + "," + Convert.ToString(Convert.ToDouble(srcPdfImgValues_New[i].ToString().Split(',')[1]) - Convert.ToDouble(prdPdfImgValues[i].ToString().Split(',')[1])));
        //    }
        //}


        if (((srcPdfImgValues != null) && (srcPdfImgValues.Count > 0)) && ((prdPdfImgValues != null) && (prdPdfImgValues.Count > 0)))
        {
            for (int i = 0; i < prdPdfImgValues.Count; i++)
            {
                list_ImgMargin.Add((i + 1) + "," + Convert.ToString(Math.Abs(Convert.ToDouble(srcPdfImgValues[0].ToString().Split(',')[1]) - Convert.ToDouble(prdPdfImgValues[i].ToString().Split(',')[1]))));
            }
        }

        if (!CheckPageTop(Convert.ToString(page)))
        {
            //Update main xml file with changed top values
            string mainXmlPath = Convert.ToString(Session["MainXMLFilePath"]);
            StreamReader sr_Update = new StreamReader(mainXmlPath);
            string xmlFile = sr_Update.ReadToEnd();
            sr_Update.Close();
            XmlDocument xmlDocOrigXml = new XmlDocument();
            xmlDocOrigXml.LoadXml(xmlFile);

            Common commObj = new Common();
            commObj.xmlDoc = xmlDocOrigXml;

            XmlNodeList nodes = xmlDocOrigXml.SelectNodes(@"//*[@top!='' and @page=" + page + "]");

            int lineCount = 0;

            foreach (var ln in srcPdfValues)
            {
                foreach (XmlNode node in nodes)
                {
                    if ((ln != null) && (node != null))
                    {
                        if (Convert.ToString(ln).Equals(node.Attributes["top"].Value))
                        {
                            if (lineCount < list_LineMargin.Count)
                            {
                                node.Attributes["top"].Value =
                                    Convert.ToString(Convert.ToDouble(node.Attributes["top"].Value) +
                                                     Convert.ToDouble(list_LineMargin[lineCount].Split(',')[1]));
                                break;
                            }
                        }
                    }
                }

                lineCount++;
            }

            XmlNodeList images = xmlDocOrigXml.SelectNodes(@"//image");
            int imageCount = 0;

            if (prdPdfImgValues != null)
            {
                foreach (var ln in prdPdfImgValues)
                {
                    foreach (XmlNode img in images)
                    {
                        if ((ln != null) && (img != null))
                        {
                            if (img.InnerXml != "")
                            {
                                if (img.ChildNodes[0].Name.Equals("caption"))
                                {
                                    if (
                                        img.ChildNodes[0].ChildNodes[0].Attributes["page"].Value.Equals(
                                            Convert.ToString(page)))
                                    {
                                        var temp = img.ChildNodes[0].ChildNodes[0].Attributes["coord"].Value.Split(':');

                                        if (temp[0] != "0" && temp[1] != "0" && temp[2] != "0" && temp[3] != "0" &&
                                            list_ImgMargin.Count > 0)
                                        {
                                            string yCoord =
                                                Convert.ToString(Convert.ToDouble(temp[1]) -
                                                                 Convert.ToDouble(
                                                                     list_ImgMargin[imageCount].Split(',')[1]));

                                            img.ChildNodes[0].ChildNodes[0].Attributes["coord"].Value = temp[0] + ":" +
                                                                                                        yCoord + ":" +
                                                                                                        temp[2] + ":" +
                                                                                                        temp[3];
                                        }
                                    }
                                }
                                else
                                {
                                    if (img.ChildNodes[0].Attributes["page"].Value.Equals(Convert.ToString(page)))
                                    {
                                        var temp = img.ChildNodes[0].Attributes["coord"].Value.Split(':');

                                        if (temp[0] != "0" && temp[1] != "0" && temp[2] != "0" && temp[3] != "0" &&
                                            list_ImgMargin.Count > 0)
                                        {
                                            string yCoord =
                                                Convert.ToString(Convert.ToDouble(temp[1]) -
                                                                 Convert.ToDouble(
                                                                     list_ImgMargin[imageCount].Split(',')[1]));

                                            img.ChildNodes[0].Attributes["coord"].Value = temp[0] + ":" + yCoord + ":" +
                                                                                          temp[2] + ":" + temp[3];
                                        }
                                    }
                                }
                            }
                        }
                    }

                    imageCount++;
                }
            }

            xmlDocOrigXml.Save(mainXmlPath);

            //GlobalVar.XMLPath = mainXmlPath;

            //string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);
            //if (String.IsNullOrEmpty(mainXmlPath))
            //    return;

            Common obj = new Common();
            XmlDocument xmlFromRhyw = obj.LoadXmlFromFile(mainXmlPath.Replace(".xml", ".rhyw"));

            XmlDocument pageXML = Common.GetPageXmlDoc(page.ToString(), xmlFromRhyw);
            string savePath = Common.GetDirectoryPath();

            //string pageXMLSavedPath = dirPath + "\\" + Convert.ToString(Session["pdfName"]).Replace(".pdf", "") +
            //                          "\\Produced_" + page + ".xml";

            string pageXMLSavedPath = savePath + System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(Session["Book_Id"])).Replace("-1", "") + "\\" +
                           System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(Session["Book_Id"])) + "-1\\Comparison\\Comparison-" +
                           Convert.ToString(Session["comparisonType"]) + "\\" + Convert.ToString(Session["userId"]) + "\\" + "Produced_" + page + ".xml";

            if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
            {
                pageXMLSavedPath = Common.GetTestFiles_SavingPath() + "/" + Convert.ToString(Session["Current_PrdPdfPage"]).Replace(".pdf", ".xml");
            }
            else if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
            {
                pageXMLSavedPath = Common.GetOnePageTestFiles_SavingPath() + "/" + Convert.ToString(Session["Current_PrdPdfPage"]).Replace(".pdf", ".xml");
            }
            else if (Convert.ToString(Session["ComparisonTask"]).Equals("task"))
            {
                pageXMLSavedPath = Common.GetTaskFiles_SavingPath() + Convert.ToString(Session["Current_PrdPdfPage"]).Replace(".pdf", ".xml");
            }

            pageXML.Save(pageXMLSavedPath);

            Session["XmlPages_TopCorrection"] = Convert.ToString(Session["XmlPages_TopCorrection"]) + "," + page;
        }

        return topOffset + "," + imgOffset;
    }

    public string GetMarginValues()
    {
        double topOffset = 0;
        double imgOffset = 0;

        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationManager.AppSettings["XSLPathCoord"]);
        XmlNode root = doc.DocumentElement;
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
        nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

        string topPageMargin = "xsl:variable[@name=\"topPageMargin\"]";
        string imgTopMargin = "xsl:variable[@name=\"imageMarginTop\"]";

        topOffset = Convert.ToDouble(root.SelectSingleNode(topPageMargin, nsmgr).Attributes["select"].Value);
        imgOffset = Convert.ToDouble(root.SelectSingleNode(imgTopMargin, nsmgr).Attributes["select"].Value);

        return (topOffset + "," + imgOffset);
    }

    public void setDefaultFont(int page)
    {
        //if (Convert.ToString(Session["ComparisonTask"]) != "")
        //{
        //    if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
        //    {
        //        cbxlFont.Items[2].Selected = true;
        //    }
        //    else if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
        //    {
        //        cbxlFont.Items[2].Selected = true;
        //    }
        //}

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

        //1 Inch = 72 Points [Postscript]
        //1 Point = 0.01388888889 Inch
        //1 PostScript point = 0.352777778 millimeters

        //Pdf width and height is in points
        var pdfPage = pdfReader.GetPageSize(page);

        //Convert point to mm for use in xsl file
        width = pdfPage.Width * 0.352777778;
        height = pdfPage.Height * 0.352777778;

        if (cbxlFont.Items[3].Selected)
        {
            iTextSharp.text.Rectangle cropbox = pdfReader.GetCropBox(page);
            var box = pdfReader.GetPageSizeWithRotation(page);

            top = Math.Round((box.Top - cropbox.Top) * 0.352777778, 3);
            bottom = Math.Round(cropbox.Bottom * 0.352777778, 3);
            right = Math.Round((box.Right - cropbox.Right) * 0.352777778, 3);
            left = Math.Round(cropbox.Left * 0.352777778, 3);
        }

        if (cbxlFont.Items[2].Selected)
        {
            string xslCoordPath = Common.GetXSLCoordDirectoryPath();

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
            doc.Load(ConfigurationManager.AppSettings["XSLPathCoord"]);
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

            if(xslCoordPath!="")
                doc.Save(xslCoordPath);
            
            //doc.Save(ConfigurationManager.AppSettings["XSLPathCoord"]);
        }
        else
        {
            //Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPath"];

            string xslPath = Common.GetXSLDirectoryPath();

            if (xslPath != "")
                Session["setDefaultXSL"] = xslPath;

            xslUsed = "XSLNormal";

            XmlDocument doc = new XmlDocument();
            doc.Load(ConfigurationManager.AppSettings["XSLPath"]);
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

            //doc.Save(ConfigurationManager.AppSettings["XSLPath"]);

            if (xslPath != "")
                doc.Save(xslPath);
        }

        //Create new pdf by xep after applying font related changes on it
        Common obj = new Common();

        //string prdPdfPagePath_Final = obj.GetProducePdf(page);

        //If xsl is changed then create new teteml of that page
        ////if (!(xslUsed.Trim().Equals(previouslyUsedXsl.Trim())))
        //tetFilePath = Common.Createtetml(prdPdfPagePath_Final); aamir commented

        Session["previouslyUsedXsl"] = xslUsed;

        ////var list = getMistakeTextList(page);

        ////string mainXmlPath = Convert.ToString(Session["MainXMLFilePath"]);

        ////this.CtrlPdfCmp.childControl(Convert.ToString(page), mainXmlPath, list);
    }

    public List<string> getMistakeTextList(int page)
    {
        StreamReader strreader = new StreamReader(SiteSession.MainXMLFilePath_PDF);
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        string TextFromPDFJs = Convert.ToString(Session["OriginalText"]);

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        XmlNodeList pdfdmistakes_Nodes = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @page=" + page + "]");
        List<string> selectedText = new List<string>();

        if (pdfdmistakes_Nodes.Count > 0)
        {
            foreach (XmlNode node in pdfdmistakes_Nodes)
            {
                var singleText = Regex.Split(TextFromPDFJs, @"\s+");
                selectedText.Add(node.InnerText);
            }
        }

        return selectedText;
    }

    protected void btnGeneratePdf_Click(object sender, EventArgs e)
    {
        int pNum = -1;
        if (int.TryParse(this.txtPageNum.Text.Trim(), out pNum))
        {
            //SiteSession.MainCurrPage = pNum;
            PageNum = pNum;
            SiteSession.MainCurrPage = PageNum;

            //this.CtrlPdfCmp.LoadTableText();

            foreach (System.Web.UI.WebControls.ListItem li in cbxlFont.Items)
            {
                li.Selected = true;
            }

            Session["srcPdfPagePath"] = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"])) + "\\" + "\\" + +PageNum + ".pdf";
            Session["prdPdfPagePath"] = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"])) + "\\" + "\\" + "Produced_" + PageNum + ".pdf";

            LoadNewPageInControl(PageNum);
        }
        else
        {
            //Invalid
        }
    }

    protected void btnFinish_Task_Click(object sender, EventArgs e)
    {
        //int mistakeCorrection = 0;
        string mainXmlPath = Convert.ToString(Session["MainXMLFilePath"]);

        if (String.IsNullOrEmpty(mainXmlPath))
            return;

        if ((Convert.ToString(Session["ComparisonTask"]).Equals("test")) || (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest")))
        {
            FinishTest(mainXmlPath);
        }
        else if ((Convert.ToString(Session["ComparisonTask"]).Equals("comparisonEntryTest")))
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

                if (percentage >= 60)
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

                if (percentage >= 60)
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

        percentage = Convert.ToDouble(detectedMistakes / totalMistakes) * 100;

        Session["Result"] = detectedMistakes;

        if (percentage >= 60)
            Response.Redirect("Passed.aspx", false);
        else
            Response.Redirect("Failed.aspx", false);
    }

    public void CompareTwoXml(String bId, string uid)
    {
        string pDirPath = ConfigurationManager.AppSettings["PDFDirPhyPath"];

        String pdfName = Convert.ToString(Session["pdfName"]);

        //Get other resource userId
        int userId = getXmlComparison_UserId();

        string xmlTwo = pDirPath + System.IO.Path.GetFileNameWithoutExtension(pdfName).Replace("-1", "") + "\\" + System.IO.Path.GetFileNameWithoutExtension(pdfName) + "\\Comparison\\Comparison-2\\" + Convert.ToString(Session["userId"]) + "\\" + pdfName.Replace("pdf", "xml");
        string xmlOne = pDirPath + System.IO.Path.GetFileNameWithoutExtension(pdfName).Replace("-1", "") + "\\" + System.IO.Path.GetFileNameWithoutExtension(pdfName) + "\\Comparison\\Comparison-1\\" + Convert.ToString(userId) + "\\" + pdfName.Replace("pdf", "xml");

        List<int> pages = new List<int>();

        string FinalInspection = pDirPath + System.IO.Path.GetFileNameWithoutExtension(pdfName).Replace("-1", "") + "\\" + System.IO.Path.GetFileNameWithoutExtension(pdfName).Replace("-1", "") + ".xml";
        Common comObj = new Common();

        //Get mistake pages from xml 1
        XmlDocument xmlDoc_Xml1 = comObj.LoadXmlDocument(xmlOne);
        XmlNodeList nodes_Xml1 = xmlDoc_Xml1.SelectNodes(@"//ln[@PDFmistake!='']");

        foreach (XmlNode lines in nodes_Xml1)
        {
            pages.Add(Convert.ToInt32(lines.Attributes["page"].Value));
        }

        //Get mistake pages from xml 2
        XmlDocument xmlDoc_Xml2 = comObj.LoadXmlDocument(xmlTwo);
        XmlNodeList nodes_Xml2 = xmlDoc_Xml2.SelectNodes(@"//ln[@PDFmistake!='']");

        foreach (XmlNode lines in nodes_Xml2)
        {
            pages.Add(Convert.ToInt32(lines.Attributes["page"].Value));
        }

        //Get distinct mistake pages from both xml
        var mistakePages = pages.Distinct().ToList();

        //Get page number which do not have any mistakes 
        var pagesToDel = Enumerable.Range(1, mistakePages.Count + 1).Except(mistakePages).ToList();

        XmlDocument xmlDoc = comObj.LoadXmlDocument(xmlOne);
        xmlDoc.Save(FinalInspection);

        ////Delete pages which do not have any mistakes from xml 1 and save as a new xml
        //XmlDocument xmlDoc = LoadXmlDocument(xmlOne);

        //foreach (var missingPage in pagesToDel)
        //{
        //    if (missingPage != 0)
        //    {
        //        XmlNodeList nodes = xmlDoc.SelectNodes("//*[@page=\"" + missingPage + "\"]/ancestor::upara|" +
        //                                                            "//*[@page=\"" + missingPage + "\"]/ancestor::spara|" +
        //                                                            "//*[@page=\"" + missingPage + "\"]/ancestor::npara|" +
        //                                                            "//*[@page=\"" + missingPage + "\"]/ancestor::image|" +
        //                                                            "//*[@page=\"" + missingPage + "\"]/ancestor::section-title|" +
        //                                                            "//*[@page=\"" + missingPage + "\"]/ancestor::prefix|" +
        //                                                            "//*[@page=\"" + missingPage + "\"]/ancestor::table"
        //                                                            );

        //        foreach (XmlNode node in nodes)
        //        {
        //            node.ParentNode.RemoveChild(node);
        //        }

        //        xmlDoc.Save(FinalInspection);
        //    }
        //}

        var innerText_Xml1 = nodes_Xml1.Cast<XmlNode>()
           .Select(node => Convert.ToString(node.InnerText))
           .ToList();

        var innerText_Xml2 = nodes_Xml2.Cast<XmlNode>()
           .Select(node => Convert.ToString(node.InnerText))
           .ToList();

        string attrName = "PDFmistake";
        int mistakeNum = 0;
        int counter = 0;

        foreach (var mistakePage in mistakePages)
        {
            if (mistakePage != 0)
            {
                //Load FinalInspection (created from xml 1) and add new mistakes (if any from xml 2) in it.
                XmlDocument xmlDoc_Final = comObj.LoadXmlDocument(FinalInspection);
                XmlNodeList allNodes = xmlDoc_Final.SelectNodes("//ln[@page=\"" + mistakePage + "\"]");

                foreach (XmlElement line in allNodes)
                {
                    if (innerText_Xml2.Any(str => str.Equals(line.InnerText)))
                    {
                        if (!line.HasAttribute(attrName))
                        {
                            XmlAttribute newAttr = xmlDoc_Final.CreateAttribute(attrName);

                            if (counter == 0)
                                mistakeNum = GetTotalMistakes(xmlOne) + 1;

                            else
                                mistakeNum += 1;

                            newAttr.Value = Convert.ToString(mistakeNum);
                            line.SetAttributeNode(newAttr);
                            counter++;
                        }
                    }
                }

                Common commObj=new Common();
                commObj.LoadXml(xmlDoc_Final.OuterXml);
                string newXmlPath = FinalInspection.Replace(".xml", ".rhyw");
                commObj.xmlDoc.Save(newXmlPath);
                //Response.Redirect(string.Format("http://localhost:30077/ComparisonTask.aspx?id={0}&p={1}&t={2}", id, pass, type));
            }
        }

        CreateQaInspectionTask(bId + "-1", uid);
    }

    //private void finishTask(String bId, string userId)
    //{
    //    try
    //    {
    //        MyDBClass objMyDBClass = new MyDBClass();
    //        string querySel = "Select BID from BOOK Where BIdentityNo='" + bId + "'";
    //        DataSet dsBookInfo = objMyDBClass.GetDataSet(querySel);
    //        string bookID = dsBookInfo.Tables[0].Rows[0]["BID"].ToString();

    //        string query_InProcess = "Update ACTIVITY Set Status='In Process', CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "' Where UID=" + userId + " AND BID=" + bookID + " AND Task='ErrorDetection'";
    //        int upRes = objMyDBClass.ExecuteCommand(query_InProcess);
    //        if (upRes > 0)
    //        {
    //            string queryUpdate = "Update ACTIVITY Set Status='Approved' Where BID=" + bookID + " AND Task='ErrorDetection' AND Status='In Process'";
    //            int rowEffected = objMyDBClass.ExecuteCommand(queryUpdate);

    //            //CreateQaInspectionTask(bId + "-1", userId);

    //            if (rowEffected > 0)
    //            {

    //            }
    //        }

    //    }
    //    catch (Exception ex)
    //    {

    //    }
    //}

    public void CreateQaInspectionTask(string bid, string userId)
    {
        MyDBClass objMyDBClass = new MyDBClass();
        string querySel = "Select BID from BOOK Where BIdentityNo='" + bid + "'";
        DataSet dsBookInfo = objMyDBClass.GetDataSet(querySel);
        string bookID = dsBookInfo.Tables[0].Rows[0]["BID"].ToString();

        //int inResult = CreateTask(bookID, "Unassigned", "QaInspection", userId);

        string pdfFilePath_User1 = "";
        string pdfFilePath_User2 = "";

        var temp = GetMistakeXmlStatus(bid);

        if ((temp != null) && (temp.Count > 0))
        {
            string comparison = "";

            if (temp.Count == 1)
            {
                return;
            }

            if (temp.Count == 2)
            {
                if (temp[1].Split(',')[1].Equals("2"))
                {
                    pdfFilePath_User1 = ConfigurationManager.AppSettings["PDFDirPhyPath"] + "/" + bid.Replace("-1", "") + "/" + bid.Replace("-1", "") + "-1/Comparison/Comparison-" + temp[0].Split(',')[1] + "/" + temp[0].Split(',')[0] + "/" + bid.Replace("-1", "") + "-1.xml";
                    pdfFilePath_User2 = ConfigurationManager.AppSettings["PDFDirPhyPath"] + "/" + bid.Replace("-1", "") + "/" + bid.Replace("-1", "") + "-1/Comparison/Comparison-" + temp[1].Split(',')[1] + "/" + temp[1].Split(',')[0] + "/" + bid.Replace("-1", "") + "-1.xml";

                    ////Check task approval of other editor
                    //string queryApproval = "Select Activity.status From Activity inner join book on activity.BID = Book.BID" +
                    //                       " where Activity.task='MistakeInjection' and Book.MainBook='" + bookId +
                    //                       "' and Activity.AID <> " + Convert.ToString(Request.QueryString["aid"]);

                    //string OtherEditor_Status = objMyDBClass.GetID(queryApproval);

                    ////Calculate efficiency when both editor tasks's are approved by the admin
                    //if (OtherEditor_Status.Trim().Equals("Approved"))
                    //{
                    double efficiency = Calculate_CumulativeEfficiency(pdfFilePath_User1, pdfFilePath_User2);

                    if (efficiency == 100)
                    {
                        int inResult = CreateTask(bookID, "Unassigned", "QaInspection", userId);
                        //}
                        //else
                        //{
                        //inResult = objMyDBClass.CreateTask(BID, "Unassigned", "ErrorAdjustment", (Session["objUser"] as UserClass).UserName);
                    }

                    return;
                    //}
                    //else
                    //{
                    //    return;
                    //}
                }
            }
        }
    }

    public List<string> GetMistakeXmlStatus(string bookId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        string strQuery = "GetMistakeXmlStatus";
        SqlCommand cmd = new SqlCommand(strQuery, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BookId", bookId.Trim());

        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        List<string> list = null;

        using (con)
        {
            if (dr.HasRows)
            {
                list = new List<string>();

                while (dr.Read())
                {
                    list.Add(Convert.ToString(dr["UserId"]) + "," + Convert.ToString(dr["TaskStatus"]));
                }
            }
        }

        return list;
    }

    public int CreateTask(string BID, string Status, string Task, string Admin)
    {
        string queryUnnowID = "Select * from [User] where UserName='unknown'";
        string UnKnownID = GetID(queryUnnowID);
        string queryInsert = "If not exists(select Task from Activity where BID = " + BID + " and UID = " + UnKnownID + " and Task = '" + Task + "' and Status='Unassigned')" +
                             " Begin Insert into ACTIVITY(UID,BID,AssignedBy,Status,Task)" +
                             " VALUES(" + UnKnownID + "," + BID + ",'" + Admin + "','" + Status + "','" + Task + "') end";
        int inResult = ExecuteCommand(queryInsert);
        return inResult;
    }

    public int ExecuteCommand(string Query)
    {
        SqlConnection con = null;

        try
        {
            con = new SqlConnection(ConnectionString());
            con.Open();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            int result = da.SelectCommand.ExecuteNonQuery();
            return result;
        }
        finally
        {
            con.Close();
        }
    }

    public string ConnectionString()
    {
        string constring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        return constring;
    }

    public string GetID(string Query)
    {
        SqlConnection con = null;
        try
        {
            con = new SqlConnection(ConnectionString());
            con.Open();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.SelectCommand.ExecuteNonQuery();
            DataSet ds = new DataSet();
            da.Fill(ds, "temp");
            con.Close();
            return ds.Tables[0].Rows[0][0].ToString();
        }
        catch (Exception excep)
        {
            return "0";
        }
        finally
        {
            con.Close();
        }
    }

    private int getXmlComparison_UserId()
    {
        MyDBClass db = new MyDBClass();
        int userId = db.GetXmlComparison_UserId(System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"])), "1");
        return userId;
    }

    public double Calculate_CumulativeEfficiency(string xmlOne, string xmlTwo)
    {
        Common comObj = new Common();

        //Get total injected mistakes 
        double injectedMistakes = Convert.ToDouble(Session["InjectedMistakesCount"]);

        //Get mistake pages from xml 1
        XmlDocument xmlDoc_Xml1 = comObj.LoadXmlDocument(xmlOne);
        double mistakes_Xml1 = xmlDoc_Xml1.SelectNodes(@"//ln[@PDFmistake and (@correction='' or @conversion ='')]").Count;

        //Get mistake pages from xml 2
        XmlDocument xmlDoc_Xml2 = comObj.LoadXmlDocument(xmlTwo);
        double mistakes_Xml2 = xmlDoc_Xml2.SelectNodes(@"//ln[@PDFmistake and (@correction='' or @conversion ='')]").Count;

        double efficiency = ((mistakes_Xml1 + mistakes_Xml2) / (2 * injectedMistakes)) * 100;
        return efficiency;
    }


    //protected void btnFinish_Click(object sender, EventArgs e)
    //{
    //    String MergedXMLFilePath = GlobalVar.MergeAllPages();

    //    System.IO.FileStream fs = null;
    //    fs = System.IO.File.Open(MergedXMLFilePath, System.IO.FileMode.Open);
    //    byte[] btFile = new byte[fs.Length];
    //    fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
    //    fs.Close();
    //    String fileName = Path.GetFileName(MergedXMLFilePath);
    //    Response.AddHeader("Content-disposition", "attachment; filename=" + fileName);
    //    Response.ContentType = "application/octet-stream";
    //    Response.BinaryWrite(btFile);
    //    Response.End();
    //}

    protected void btnFinish_Click(object sender, EventArgs e)
    {
        string mainXmlPath = Convert.ToString(Session["MainXMLFilePath"]);

        if (String.IsNullOrEmpty(mainXmlPath))
            return;

        System.IO.FileStream fs = null;
        fs = System.IO.File.Open(mainXmlPath, System.IO.FileMode.Open);
        byte[] btFile = new byte[fs.Length];
        fs.Read(btFile, 0, Convert.ToInt32(fs.Length));
        fs.Close();
        String fileName = System.IO.Path.GetFileName(mainXmlPath);
        Response.AddHeader("Content-disposition", "attachment; filename=" + fileName);
        Response.ContentType = "application/octet-stream";
        Response.BinaryWrite(btFile);
        Response.End();
    }

    protected void btnCloseDialog_Click(object sender, EventArgs e)
    {
        Response.Redirect("OnlineTestUser.aspx");
    }



    [WebMethod]
    public static string GetParaType(string text)
    {
        PDFManipulation pdfMan = new PDFManipulation("");
        string textType = "";//pdfMan.GetTextType(text);
        return textType;
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

        //To do aamir 2016-06-18
        //List<string> originalText = new List<string>();

        //foreach (string text in listMistakes)
        //{
        //    originalText.Add(text);
        //}

        //Common.LoadMistakePanel(Convert.ToString(currentPage), xmlPath, originalText, "producedPdf");
        ////Common.LoadMistakePanel(Convert.ToString(currentPage), xmlPath, originalText, "sourcePdf");
    }

    [WebMethod]
    public static string LogMistakes(string text)
    {
        //List<string> selectedText = new List<string>();

        string selectedTextDivNumbers = "";

        //Get selected line text + whole page text separated by a string.
        var temp = text.Split(new string[] { "/~/" }, StringSplitOptions.None);

        if ((temp != null) && (temp.Length > 0))
        {
            //HttpContext.Current.Session["SelectedMistakeText"] = Convert.ToString(temp[0]);
            //Insert PDFmistake attribute in xml 
            Common.LogMistakesInXml(temp[0], temp[1], temp[2]);
            PDFManipulation pdfMan = new PDFManipulation("");
            selectedTextDivNumbers = pdfMan.GetSelectedTextFromProducedPage(temp[1], temp[2], Convert.ToInt32(HttpContext.Current.Session["MainCurrPage"]));

            //selectedText.Add(Convert.ToString(temp[0]));
            //string srcPdfMistakeCoords = Common.GetPdfMistakeCoords(Convert.ToString(HttpContext.Current.Session["MainCurrPage"]),
            //                             Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]), selectedText, "sourcePdf");
            //DrawRectangle(srcPdfMistakeCoords);
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

        //MyDBClass obj = new MyDBClass();StopTask(bool complete, string status)
        //obj.SaveComparisonTimeSpent(hour, minute, Convert.ToString(HttpContext.Current.Session["LoginId"]));
        //string selectedTextDivNumbers = "";

        //return timeWorked;
    }

    //public static void DrawRectangle(string coords)
    //{
    //    if (coords == "")
    //        return;

    //    string directoryPath = "";

    //    if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]) != "")
    //    {
    //        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
    //        {
    //            directoryPath = Common.GetTestFiles_SavingPath();
    //        }
    //        else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest"))
    //        {
    //            directoryPath = Common.GetOnePageTestFiles_SavingPath();
    //        }
    //        else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    //        {
    //            directoryPath = Common.GetTaskFiles_SavingPath();
    //        }
    //    }

    //    string pdfPath = directoryPath + "/" + Convert.ToString(HttpContext.Current.Session["Current_SrcPdfPage"]);
    //    string imgPath = pdfPath + ".jpeg";
    //    var image = System.Drawing.Image.FromFile(imgPath);

    //    string[] Coord = coords.Split(":".ToCharArray());
    //    float llx = float.Parse(Coord[0]);
    //    float lly = float.Parse(Coord[1]);
    //    float urx = float.Parse(Coord[2]);
    //    float ury = float.Parse(Coord[3]);

    //    using (MemoryStream memory = new MemoryStream())
    //    {
    //        image.Save(memory, ImageFormat.Jpeg);
    //        image.Dispose();
    //        var img = System.Drawing.Image.FromStream(memory);
    //        Bitmap bmp = new Bitmap(img);

    //        int x = (int)llx;
    //        int y = (int)lly;
    //        int height = (int)ury - (int)lly;
    //        int width = (int)urx - (int)llx;

    //        System.Drawing.Rectangle rect = new System.Drawing.Rectangle(x, y, width, height);

    //        // Draw line to screen.
    //        using (var graphics = Graphics.FromImage(bmp))
    //        {
    //            //graphics.FillRectangle(b1, x1, y1, x2, y2);
    //            graphics.FillRectangle(new SolidBrush(Color.FromArgb(150, 55, 255, 183)), rect);
    //        }

    //        //string savePath = @"D:\Office Data\Files_Dev\Tests\aamirghafoor2002@yahoo.com\ComparisonTests\100\1.pdf.jpeg";
    //        bmp.Save(imgPath);
    //    }
    //}

    //[WebMethod]
    //public static string GetTextDivs(string pageAllText)
    //{
    //    PDFManipulation pdfMan = new PDFManipulation("");
    //    string selectedTextWithDivNumbers = null;

    //    //if (pdfMan.GetTotalMistakes(SiteSession.MainCurrPage) > 0)
    //    //{
    //    selectedTextWithDivNumbers = pdfMan.GetSelectedTextFromProducedPage(pageAllText, Convert.ToInt32(HttpContext.Current.Session["MainCurrPage"]));
    //    //}

    //    return selectedTextWithDivNumbers;
    //}

}
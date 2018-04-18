using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.Collections.Generic;
using Outsourcing_System;

public partial class Controls_PDFTextCmp : System.Web.UI.UserControl
{
    public void ShowSrcPDFAsImageInControl(string pdfPath, string page)
    {
        PDF2JPG_Sofnix.Convert cnv = new PDF2JPG_Sofnix.Convert();
        this.pdf1.FilePath = pdfPath;
        bool output = cnv.PDF2ImageFromFile(pdfPath, "");
        this.imgSrc.ImageUrl = "../showPdf.ashx?Page=" + page + "&pdfType=src" + "&type=img";
    }

    public void ShowPrdPDFInJSControl(string page)
    {
        pdfCtrl.PDFFile = "showPdf.ashx?Page=" + page + "&pdfType=prd" + "&type=pdf";
    }

    //public void ShowSrcPDFAsImageInControl(string bookId, string pdfPath, string uid, string page, string compType)
    //{
    //    PDF2JPG_Sofnix.Convert cnv = new PDF2JPG_Sofnix.Convert();
    //    this.pdf1.FilePath = pdfPath;
    //    bool output = cnv.PDF2ImageFromFile(pdfPath, "");

    //    //this.imgSrc.ImageUrl = "../showPdf.ashx?Page=" + page + "&bid=" + bookId + "&type=img" + "&taskType=task&uid=" + uid + "&compType=" + compType;
    //    this.imgSrc.ImageUrl = "../showPdf.ashx?Page=" + page + "&pdfType=src" + "&type=img";
    //}

    //public void ShowPrdPDFInJSControl(string bookId, string pdfPath, string uid, string page, string compType)
    //{
    //    //pdfCtrl.PDFFile = "showPdf.ashx?Page=" + page + "&bid=" + bookId + "&type=pdf" + "&taskType=task&uid=" + uid + "&compType=" + compType;
    //    pdfCtrl.PDFFile = "showPdf.ashx?Page=" + page + "&pdfType=prd" + "&type=pdf";
    //}

    public event CommandEventHandler ErrorsComplete;
    public event CommandEventHandler FileEdit;

    private bool LoadPDFAsImage
    {
        set
        {
            PDFCmpSesssion.LoadPDFAsImage = value;
            if (value == true)
            {
                this.trPDF.Visible = false;
                this.trImg.Visible = true;
            }
        }
        get
        {
            return PDFCmpSesssion.LoadPDFAsImage;
        }
    }

    //public void childControl(string page, string xmlPath, List<string> list)
    //{
    //    this.pdfCtrl.LoadMistakePanel(page, xmlPath, list);

    //}

    protected void Button_Click(object sender, EventArgs e)
    {


    }

    public void LoadTableText()
    {
        this.pdfCtrl.LoadTableText();
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //aamir
        //if (!Page.IsPostBack)
        //{
        //    PDFCmpSesssion.misMatchList = new ArrayList();
        //}
        //else
        //{
        //    RetainBothPDFs();
        //}
    }

    /// <summary>
    /// The current mismatch type and comments
    /// </summary>
    public MisMatch MisMatchComments
    {    //sdfsdf
        set
        {
            PDFCmpSesssion.CurrentPageMisMatch = value;
            //if (value != null)
            //{
            //    if (((MisMatch)value).misMatchType == MisMatchType.Extra)
            //    {

            //        rbMissing.Checked = false;
            //        rbSpell.Checked = false;
            //        rbOther.Checked = false;

            //        rbExtra.Checked = true;
            //    }
            //    else if (((MisMatch)value).misMatchType == MisMatchType.Miss)
            //    {

            //        rbSpell.Checked = false;
            //        rbOther.Checked = false;
            //        rbExtra.Checked = false;

            //        rbMissing.Checked = true;
            //    }
            //    else if (((MisMatch)value).misMatchType == MisMatchType.Spell)
            //    {
            //        rbMissing.Checked = false;
            //        rbOther.Checked = false;
            //        rbExtra.Checked = false;

            //        rbSpell.Checked = true;
            //    }
            //    else if (((MisMatch)value).misMatchType == MisMatchType.Other)
            //    {
            //        rbMissing.Checked = false;
            //        rbSpell.Checked = false;
            //        rbExtra.Checked = false;

            //        rbOther.Checked = true;
            //    }
            //}
        }
        get
        {
            return PDFCmpSesssion.CurrentPageMisMatch;
        }
    }

    public void HideComments()
    {
        this.trComments.Visible = false;
    }


    /// <summary>
    /// The MisMatch currently selected in the control
    /// </summary>
    public MisMatchType CurrentMisMatch
    {
        get
        {
            return getMisMatchType();
        }
    }

    /// <summary>
    /// First PDF to compare from 
    /// </summary>
    public string SrcPDFPath
    {
        set
        {
            PDFCmpSesssion.FirstPDFPath = value;
        }
        get
        {
            return PDFCmpSesssion.FirstPDFPath;
        }
    }

    /// <summary>
    /// Second PDF to compare with
    /// </summary>
    public string ProdPDFPath
    {
        set
        {
            PDFCmpSesssion.SecondPDFPath = value;
        }
        get
        {
            return PDFCmpSesssion.SecondPDFPath;
        }
    }

    public string XMLFilePath
    {
        set
        {
            PDFCmpSesssion.XMLFilePath = value;
        }
        get
        {
            return PDFCmpSesssion.XMLFilePath;
        }
    }

    private int currPageNum;
    public int CurrPageNumber
    {
        set
        {
            this.currPageNum = value;
        }
        get
        {
            return this.currPageNum;
        }
    }

    public void LoadFiles(bool LoadAsImage)
    {
        LoadPDFAsImage = LoadAsImage;
        PDFFile xmlPDF = new PDFFile(PDFCmpSesssion.SecondPDFPath);
        PDFFile pdfPDF = new PDFFile(PDFCmpSesssion.FirstPDFPath);
        PDFCmpSesssion.cf = new CompareFiles();
        PDFCmpSesssion.cf.firstFile = pdfPDF;
        PDFCmpSesssion.cf.secondFile = xmlPDF;
        PDFCmpSesssion.list1PrevMatchIndex = -1;
        PDFCmpSesssion.list2PrevMatchIndex = -1;
        PDFCmpSesssion.List1CurrPageNo = 1;
        PDFCmpSesssion.List2CurrPageNo = 1;
        //PDFCmpSesssion.List1CurrPageNo = currPageNum;
        //PDFCmpSesssion.List2CurrPageNo = currPageNum;

        PDFCmpSesssion.list1ShowingIndex = 0;
        PDFCmpSesssion.list2ShowingIndex = 0;

        //lblList1PageNo.Text = "1";
        //lblList2PageNo.Text = "1";

        PDFCmpSesssion.wrdList1 = PDFCmpSesssion.cf.firstFile.GenerateAndGetAllWordsInFile();
        PDFCmpSesssion.wrdList2 = PDFCmpSesssion.cf.secondFile.GenerateAndGetAllWordsInFile();

        showLists();
        //StartMatching(0, 0); //aamir
        ShowComments();
    }

    private void ShowComments()
    {
        MisMatch misMatch = MisMatchComments;
        misMatch.misMatchType = getMisMatchType();
        if (misMatch.misMatchType == MisMatchType.Other)
        {
            rbOther.Checked = true;
            txtComments.Text = misMatch.comments;
        }
        else if (misMatch.misMatchType == MisMatchType.Miss)
        {
            rbMissing.Checked = true;
        }
        else if (misMatch.misMatchType == MisMatchType.Extra)
        {
            rbExtra.Checked = true;
        }
        else if (misMatch.misMatchType == MisMatchType.Spell)
        {
            rbSpell.Checked = true;
        }
    }

    public void LoadFilesWithoutMatching(bool LoadAsImage)
    {
        LoadPDFAsImage = LoadAsImage;

        if (Convert.ToString(Session["srcPdfPagePath"]) == "")
            return;

        string savePath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
        string pdfFile = Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"]));

        string stampedSrcPdf = "";
        string stampePrddPdf = "";

        string srcPdfFile = Path.GetFileNameWithoutExtension(Convert.ToString(Session["srcPdfPagePath"]));
        string prdPdfFile = Path.GetFileNameWithoutExtension(Convert.ToString(Session["prdPdfPagePath"]));

        if (srcPdfFile.Contains("_Stamped"))
        {
            stampedSrcPdf = savePath + pdfFile.Replace("-1", "") + "\\" + pdfFile + "\\Comparison\\Comparison-" + Convert.ToString(Session["comparisonType"]) +
                            "\\" + Convert.ToString(Session["userId"]) + "\\" + srcPdfFile + ".pdf";

            stampePrddPdf = savePath + pdfFile.Replace("-1", "") + "\\" + pdfFile + "\\Comparison\\Comparison-" + Convert.ToString(Session["comparisonType"]) +
                            "\\" + Convert.ToString(Session["userId"]) + "\\" + prdPdfFile + ".pdf";
        }
        else
        {
            //stampedSrcPdf = savePath + pdfFile + "\\" + pdfFile + "\\" + srcPdfFile + "_Stamped.pdf";
            //stampePrddPdf = savePath + pdfFile + "\\" + pdfFile + "\\" + prdPdfFile + "_Stamped.pdf";
            stampedSrcPdf = savePath + pdfFile.Replace("-1", "") + "\\" + pdfFile + "\\Comparison\\Comparison-" + Convert.ToString(Session["comparisonType"]) +
                            "\\" + Convert.ToString(Session["userId"]) + "\\" + srcPdfFile + "_Stamped.pdf";
            stampePrddPdf = savePath + pdfFile.Replace("-1", "") + "\\" + pdfFile + "\\Comparison\\Comparison-" + Convert.ToString(Session["comparisonType"]) +
                            "\\" + Convert.ToString(Session["userId"]) + "\\" + prdPdfFile + "_Stamped.pdf";
        }

        string srcPDFPath = "";
        string prdPDFPath = "";

        if (File.Exists(stampedSrcPdf))
        {
            srcPDFPath = stampedSrcPdf;
        }

        if (File.Exists(stampePrddPdf))
        {
            prdPDFPath = stampePrddPdf;
        }

        else
        {
            srcPDFPath = Convert.ToString(Session["srcPdfPagePath"]);
            prdPDFPath = Convert.ToString(Session["prdPdfPagePath"]);
        }


        //aamir
        //PDFFile xmlPDF = new PDFFile(PDFCmpSesssion.SecondPDFPath);
        //PDFFile pdfPDF = new PDFFile(PDFCmpSesssion.FirstPDFPath);

        //PDFCmpSesssion.cf = new CompareFiles();
        //PDFCmpSesssion.cf.firstFile = xmlPDF;
        //PDFCmpSesssion.cf.secondFile = pdfPDF;
        //PDFCmpSesssion.list1PrevMatchIndex = -1;
        //PDFCmpSesssion.list2PrevMatchIndex = -1;
        //PDFCmpSesssion.List1CurrPageNo = 1;
        //PDFCmpSesssion.List2CurrPageNo = 1;
        //PDFCmpSesssion.list1ShowingIndex = 0;
        //PDFCmpSesssion.list2ShowingIndex = 0;
        //end

        //StartMatching(0, 0);
        //string annotedPDF = AddAnnotationInPDF(srcPDFPath);
        //Session["srcPdfPagePath"] = annotedPDF;//aamir

        ShowPDFInControl(this.pdf1.ID, srcPDFPath);
        ShowPDFInControl(this.pdf2.ID, prdPDFPath);

        this.pdfCtrl.ShowTableDiv = true;
    }

    private void showLists()
    {
        ArrayList list1, list2;
        GetWordListsAgainstPage(PDFCmpSesssion.List1CurrPageNo, PDFCmpSesssion.List2CurrPageNo, out list1, out list2);
        listBox1.Items.Clear();
        listBox2.Items.Clear();
        for (int i = 0; i < list1.Count; i++)
        {
            ListItem li = new ListItem(list1[i].ToString(), i.ToString());
            listBox1.Items.Add(li);
            //listBox1.Items.Add(list1[i].ToString());
        }
        for (int i = 0; i < list2.Count; i++)
        {
            ListItem li = new ListItem(list2[i].ToString(), i.ToString());
            listBox2.Items.Add(li);
            //listBox2.Items.Add(list2[i].ToString());
        }
    }

    private void GetWordListsAgainstPage(int l1pNum, int l2pNum, out ArrayList firstPageList, out ArrayList secondPageList)
    {
        firstPageList = new ArrayList();
        secondPageList = new ArrayList();
        ArrayList list1 = PDFCmpSesssion.wrdList1;
        for (int i = 0; i < list1.Count; i++)
        {
            Word wrd = ((Word)list1[i]);
            if (wrd.PageNumber == l1pNum)
            {
                //wrd.PageNumber = l1pNum;
                firstPageList.Add(wrd.Text);
            }
            else if (wrd.PageNumber > l1pNum)
                break;
        }
        ArrayList list2 = PDFCmpSesssion.wrdList2;
        for (int j = 0; j < list2.Count; j++)
        {
            Word wrd = ((Word)list2[j]);
            if (wrd.PageNumber == l2pNum)
            {
                //wrd.PageNumber=l2pNum;
                secondPageList.Add(wrd.Text);
            }
            else if (wrd.PageNumber > l2pNum)
                break;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns>Generateed Report Path</returns>
    public string GenerateReport()
    {
        return "";
    }

    /// <summary>
    /// Starts matching from the provided indeces in respective lists and returns when a mismatch or end of lists is reached
    /// </summary>
    /// <param name="list1StartIndex">Index of the listbox not the original pdf word list's</param>
    /// <param name="list2StartIndex">Index of the listbox not the original pdf word list's</param>
    private void StartMatching(int list1StartIndex, int list2StartIndex)
    {
        int mainList1Index = list1StartIndex + PDFCmpSesssion.list1ShowingIndex;
        int mainList2Index = list2StartIndex + PDFCmpSesssion.list2ShowingIndex;
        if (this.listBox1.Items.Count > 0 && this.listBox2.Items.Count > 0)
        {
            if (PDFCmpSesssion.list1PrevMatchIndex != -1)
            {
                if (PDFCmpSesssion.list1PrevMatchIndex > mainList1Index || PDFCmpSesssion.list2PrevMatchIndex > mainList2Index)
                {
                    //lblMsg.Text = "Cannot select previous elements, please reselect";
                    listBox1.SelectedIndex = PDFCmpSesssion.list1PrevMatchIndex - PDFCmpSesssion.list1ShowingIndex;
                    listBox2.SelectedIndex = PDFCmpSesssion.list2PrevMatchIndex - PDFCmpSesssion.list2ShowingIndex;
                    return;
                }
                else
                {
                    AddMisMatchItem(mainList1Index, mainList2Index);
                }
            }
            int i = mainList1Index, j = mainList2Index;
            string list1Item = "", list2Item = "";
            do
            {
                Word list1Wrd = (Word)PDFCmpSesssion.wrdList1[i];
                list1Item = list1Wrd.Text;
                Word list2Wrd = (Word)PDFCmpSesssion.wrdList2[j];
                list2Item = list2Wrd.Text;
                if (list1Item != list2Item)
                {
                    PDFCmpSesssion.list1PrevMatchIndex = i;
                    PDFCmpSesssion.List1CurrPageNo = list1Wrd.PageNumber;
                    updateList1();
                    PDFCmpSesssion.List2CurrPageNo = list2Wrd.PageNumber;
                    updateList2();

                    SelectListViewItem(ref listBox1, i);// - CurrSesssion.list1ShowingIndex);
                    SelectListViewItem(ref listBox2, j);// - CurrSesssion.list2ShowingIndex);
                    PDFCmpSesssion.list2PrevMatchIndex = j;
                    listBox1.SelectedIndex = i - PDFCmpSesssion.list1ShowingIndex;
                    listBox2.SelectedIndex = j - PDFCmpSesssion.list2ShowingIndex;
                    //lblMsg.Text = "Possible Mismatch, please select a Match and continue";
                    break;
                }
                if (i <= PDFCmpSesssion.wrdList1.Count - 2)
                {
                    i++;
                }
                else
                {
                    //printList();
                    Finished();
                    break;
                }
                if (j <= PDFCmpSesssion.wrdList2.Count - 2)
                {
                    j++;
                }
                else
                {
                    //printList();
                    Finished();
                    break;
                }
            } while (true);

        }
    }

    /// <summary>
    /// Updates the list1 with the words of the page number in the current session
    /// </summary>
    private void updateList1()
    {
        //lblList1PageNo.Text = PDFCmpSesssion.List1CurrPageNo.ToString();
        listBox1.Items.Clear();
        ArrayList list1;
        UpdateWordList1(PDFCmpSesssion.List1CurrPageNo, out list1);
        for (int i = 0; i < list1.Count; i++)
        {
            ListItem li = new ListItem(list1[i].ToString(), i.ToString());
            listBox1.Items.Add(li);
            //listBox1.Items.Add(list1[i].ToString());
        }
    }

    private void updateList2()
    {
        //lblList2PageNo.Text = PDFCmpSesssion.List2CurrPageNo.ToString();
        listBox2.Items.Clear();
        ArrayList list2;
        UpdateWordList2(PDFCmpSesssion.List2CurrPageNo, out list2);
        for (int i = 0; i < list2.Count; i++)
        {
            ListItem li = new ListItem(list2[i].ToString(), i.ToString());
            listBox2.Items.Add(li);
            //listBox2.Items.Add(list2[i].ToString());
        }
    }

    private void UpdateWordList1(int l1pNum, out ArrayList firstPageList)
    {
        firstPageList = new ArrayList();
        ArrayList list1 = PDFCmpSesssion.wrdList1;
        bool isFirstIndexSet = false;
        for (int i = 0; i < list1.Count; i++)
        {
            Word wrd = ((Word)list1[i]);
            if (wrd.PageNumber == l1pNum)
            {
                if (!isFirstIndexSet)
                {
                    PDFCmpSesssion.list1ShowingIndex = i;
                    isFirstIndexSet = true;
                }
                firstPageList.Add(wrd.Text);
            }
            else if (wrd.PageNumber > l1pNum)
                break;
        }
    }

    private void UpdateWordList2(int l2pNum, out ArrayList secondPageList)
    {
        secondPageList = new ArrayList();
        ArrayList list2 = PDFCmpSesssion.wrdList2;
        bool isFirstIndexSet = false;
        for (int j = 0; j < list2.Count; j++)
        {
            Word wrd = ((Word)list2[j]);
            if (wrd.PageNumber == l2pNum)
            {
                if (!isFirstIndexSet)
                {
                    PDFCmpSesssion.list2ShowingIndex = j;
                    isFirstIndexSet = true;
                }

                secondPageList.Add(wrd.Text);
            }
            else if (wrd.PageNumber > l2pNum)
                break;
        }
    }

    private bool SelectListViewItem(ref ListBox lb, int index)
    {
        //if (index >= 0 && index < lb.Items.Count)
        {
            //lb.Focus();
            //lb.Items[index].Selected = true;
            //lb.Items[index]. EnsureVisible();
            //Word wrd = (Word)lb.Items[index].Tag;
            //Word wrd = null;
            //if (lb.ID == "listBox1")
            //{
            //    wrd = (Word)PDFCmpSesssion.wrdList1[index];
            //    //this.txtList1PageNo.Text = wrd.PageNumber.ToString();
            //    //this.txtList1LineNo.Text = wrd.LineNumber.ToString();
            //    string highFilePath1 = SelectCurrentWordInPDFWithAnnotation(PDFCmpSesssion.cf.firstFile.FilePath, wrd);
            //    ShowPDFInControl(this.pdf1.ID, highFilePath1);
            //    RetainSamePDF(false);
            //}
            //else
            //{
            //    wrd = (Word)PDFCmpSesssion.wrdList2[index];
            //    //this.txtList2PageNo.Text = wrd.PageNumber.ToString();
            //    //this.txtList2LineNo.Text = wrd.LineNumber.ToString();
            //    string highFilePath2 = SelectCurrentWordInPDF(PDFCmpSesssion.cf.secondFile.FilePath, wrd);
            //    ShowPDFInControl(this.pdf2.ID, highFilePath2);
            //    RetainSamePDF(true);
            //}

            return true;
        }
        //else
        //  return false;
    }

    private void RetainSamePDF(bool retainInList1)
    {
        if (retainInList1)
        {
            string pdfPath = PDFCmpSesssion.PDF1PrevPath;
            ShowPDFInControl(this.pdf1.ID, pdfPath);
        }
        else
        {
            string pdfPath = PDFCmpSesssion.PDF2PrevPath;
            ShowPDFInControl(this.pdf2.ID, pdfPath);
        }
    }

    public void RetainBothPDFs()
    {
        //commented by aamir
        //if (!LoadPDFAsImage)
        //{


        string pdf1Path = Convert.ToString(Session["srcPdfPagePath"]);
        string pdf2Path = Convert.ToString(Session["prdPdfPagePath"]);

        //end

        //string pdfName = Convert.ToString(Session["pdfName"]);
        //string pdf2Path = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(pdfName) + "\\15_Produced_Stamped.pdf";
        //string pdf1Path = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(pdfName) + "\\15_Stamped.pdf";

        //if (File.Exists(pdf1Path))
        {
            ShowPDFInControl(this.pdf1.ID, pdf1Path);
        }
        //if (File.Exists(pdf2Path))
        {
            ShowPDFInControl(this.pdf2.ID, pdf2Path);
        }
        //}
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="wrd"></param>
    /// <returns>highlighted File Path</returns>
    private string SelectCurrentWordInPDF(string pdfFilePath, List<PdfWord> wrd)
    {
        int page = wrd[0].PageNumber;
        PDFManipulation pdfMan = new PDFManipulation(pdfFilePath);
        //string extractedFile = pdfMan.ExtractPage(page);
        //string highlightedfilePath = pdfMan.HighlightWord(extractedFile, wrd);
        string highlightedfilePath = pdfMan.HighlightWord(pdfFilePath, wrd);
        return highlightedfilePath;
    }
    //
    private string SelectCurrentWordInPDFWithAnnotation(string pdfFilePath, List<PdfWord> wrd)
    {
        int page = wrd[0].PageNumber;
        PDFManipulation pdfMan = new PDFManipulation(pdfFilePath);
        string extractedFile = pdfMan.ExtractPageWithAnnotation(SiteSession.MainCurrPage);
        string highlightedfilePath = pdfMan.HighlightWord(extractedFile, wrd);
        File.Delete(extractedFile);
        return highlightedfilePath;
    }

    protected void btnMatch_Click(object sender, EventArgs e)
    {
        int lb1SelIndex = this.listBox1.SelectedIndex;
        int lb2SelIndex = this.listBox2.SelectedIndex;

        //StartMatching(lb1SelIndex, lb2SelIndex); aamir
    }

    protected void listBox1_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = listBox1.SelectedIndex + PDFCmpSesssion.list1ShowingIndex;
        SelectListViewItem(ref listBox1, index);
    }

    protected void listBox2_SelectedIndexChanged(object sender, EventArgs e)
    {
        int index = listBox2.SelectedIndex + PDFCmpSesssion.list2ShowingIndex;
        SelectListViewItem(ref listBox2, index);
    }

    private bool ShowPDFInJSControl(string pdfPath)
    {
        pdfCtrl.XMLFile = PDFCmpSesssion.XMLFilePath;
        //PDFCmpSesssion.SecondPDFPath = pdfPath;
        //////string fileName = Path.GetFileName(pdfPath);
        //////string dir = ConfigurationManager.AppSettings["HighlightDirPP"];
        //pdfCtrl.PDFFile = pdfPath;
        string bookId = Convert.ToString(Session["test_Id"]);
        string prdPdfFileName = Path.GetFileNameWithoutExtension(Convert.ToString(Session["prdPdfPagePath"]));

        //pdfCtrl.PDFFile = pdfPath;

        if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
        {
            //string pDirPath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
            //string userDir_Path = pDirPath + "\\Tests\\" + Convert.ToString(Session["CompTestUser_Email"]) + "/ComparisonTests/";

            pdfCtrl.PDFFile = "showPdf.ashx?Page=" + SiteSession.MainCurrPage + "&type=pdf" + "&bid=" + bookId +
                              "&uid=" + Convert.ToString(Session["userId"]) + "&comType=test" + "&filename=" + prdPdfFileName + "&email=" + Convert.ToString(Session["CompTestUser_Email"]);
        }
        else
        {
            pdfCtrl.PDFFile = "showPdf.ashx?Page=" + SiteSession.MainCurrPage + "&type=pdf" + "&bid=" + bookId +
                              "&uid=" + Convert.ToString(Session["userId"]) + "&comType=" + Convert.ToString(Session["comparisonType"]) + "&filename=" + prdPdfFileName;
        }

        ////changing Asad - otherwise uncomment the above line and comment below till return
        //string []dummy = ProdPDFPath.Split('\\');

        //pdfPath = dummy[dummy.Length - 3] + "\\" + dummy[dummy.Length - 2] + "\\" + dummy[dummy.Length - 1];
        //pdfCtrl.PDFFile = pdfPath;

        ////////////
        return true;
    }

    public void ShowPDFIn_Mistakes(string pdfPath)
    {
        this.pdf1.FilePath = pdfPath;
    }

    private bool ShowPDFInControl(string pdfPanelName, string pdfPath)
    {
        if (LoadPDFAsImage)
        {
            if (this.pdf1.ID == pdfPanelName)
            {
                return ShowPDFAsImageInControl(this.imgSrc.ID, pdfPath);
            }
            else
            {
                //return ShowPDFAsImageInControl(this.imgPrd.ID, pdfPath);
                return ShowPDFInJSControl(pdfPath);
            }
        }
        else
        {
            string fileName = System.IO.Path.GetFileName(pdfPath);
            string vDirPath = System.Configuration.ConfigurationManager.AppSettings["HighlightDirVP"];
            string vfilePath = vDirPath + "/" + fileName;
            if (this.pdf1.ID == pdfPanelName)
            {
                this.pdf1.FilePath = vfilePath;
                PDFCmpSesssion.PDF1PrevPath = vfilePath;
            }
            else
            {
                this.pdf2.FilePath = vfilePath;
                PDFCmpSesssion.PDF2PrevPath = vfilePath;
            }
            return true;
        }
    }

    private bool ShowPDFAsImageInControl(string PanelName, string pdfPath)
    {
        if (pdfPath.Equals(""))
            return false;
        string fileName = System.IO.Path.GetFileName(pdfPath);
        //string vDirPath = System.Configuration.ConfigurationManager.AppSettings["HighlightDirVP"];

        string vDirPath = pdfPath;
        string vfilePath = vDirPath;
        string vImgFilePath = pdfPath + ".jpeg";

        string bookId = Convert.ToString(Session["test_Id"]);

        LogWritter.WriteLineInLog("ShowPDFAsImageInControl vDirPath::" + vDirPath);
        LogWritter.WriteLineInLog("ShowPDFAsImageInControl fileName::" + fileName);
        LogWritter.WriteLineInLog("ShowPDFAsImageInControl vfilePath::" + vfilePath);

        string phyDirPath = System.Configuration.ConfigurationManager.AppSettings["PDFDirPhyPath"];
        string phyFilePath = vDirPath;

        LogWritter.WriteLineInLog("ShowPDFAsImageInControl phyDirPath::" + phyDirPath);

        PDF2JPG_Sofnix.Convert cnv = new PDF2JPG_Sofnix.Convert();
        if (this.imgSrc.ID == PanelName)
        {
            this.pdf1.FilePath = vfilePath;

            bool output = cnv.PDF2ImageFromFile(phyFilePath, "");
            //if (output)
            {
                string File_Name = Path.GetFileName(vImgFilePath);

                string book_dir = "";
                if (Session["SrcPDFPath"] != null)
                {
                    book_dir = Path.GetFileNameWithoutExtension(Session["SrcPDFPath"].ToString());
                }

                if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
                {
                    this.imgSrc.ImageUrl = "../showPdf.ashx?Page=" + SiteSession.MainCurrPage + "&type=img" + "&filename=" + File_Name + "&bid=" + bookId
                                           + "&uid=" + Convert.ToString(Session["userId"]) + "&comType=test" + "&email=" + Convert.ToString(Session["CompTestUser_Email"]); ;
                }
                else
                {
                    //this.imgSrc.ImageUrl = phyDirPath + book_dir.Replace("-1", "") + "/" + book_dir + "/Comparison/Comparison-" + Convert.ToString(Session["comparisonType"]) + "/" + Convert.ToString(Session["userId"]) + "/" + File_Name;
                    this.imgSrc.ImageUrl = "../showPdf.ashx?Page=" + SiteSession.MainCurrPage + "&type=img" + "&filename=" + File_Name + "&bid=" + bookId
                                               + "&uid=" + Convert.ToString(Session["userId"]) + "&comType=" + Convert.ToString(Session["comparisonType"]);
                }

                LogWritter.WriteLineInLog("ShowPDFAsImageInControl vImgFilePath::" + vImgFilePath);
                //PDFCmpSesssion.PDF1PrevPath = vfilePath;
                PDFCmpSesssion.PDF1PrevPath = "";
            }
        }
        else
        {
            this.pdf2.FilePath = vfilePath;
            bool output = cnv.PDF2ImageFromFile(phyFilePath, "");
            //if (output)
            {
                //this.imgPrd.ImageUrl = vImgFilePath;
                //PDFCmpSesssion.PDF2PrevPath = vfilePath;
                PDFCmpSesssion.PDF2PrevPath = "";
            }
        }
        return true;
    }



    private string AddAnnotationInPDF(string pdfPath)
    {
        string fileName = System.IO.Path.GetFileName(pdfPath);
        string vDirPath = ConfigurationManager.AppSettings["HighlightDirVP"];
        string vfilePath = vDirPath + "/" + fileName;
        PDFManipulation pdfMan = new PDFManipulation(pdfPath);
        string extractedFile = pdfMan.ExtractPageWithAnnotation(SiteSession.MainCurrPage);
        return extractedFile;
    }

    private void AddMisMatchItem(int list1StartIndex, int list2StartIndex)
    {
        MisMatch mm = new MisMatch();
        mm.list1Index = PDFCmpSesssion.list1PrevMatchIndex;
        mm.list2Index = PDFCmpSesssion.list2PrevMatchIndex;
        mm.list1MMLen = list1StartIndex - PDFCmpSesssion.list1PrevMatchIndex;
        mm.list2MMLen = list2StartIndex - PDFCmpSesssion.list2PrevMatchIndex;

        Word wrdList1 = ((Word)PDFCmpSesssion.cf.firstFile.WordsArrayList[list1StartIndex]);
        Word wrdList2 = ((Word)PDFCmpSesssion.cf.secondFile.WordsArrayList[list2StartIndex]);
        //mm.list1PNum = wrdList1.PageNumber;
        //mm.list1LNum = wrdList1.LineNumber;
        //mm.list2PNum = wrdList2.PageNumber;
        //mm.list2LNum = wrdList2.LineNumber;
        mm.PageNumber = wrdList1.PageNumber;
        mm.LineNumber = wrdList1.LineNumber;

        mm.misMatchType = getMisMatchType();
        if (mm.misMatchType == MisMatchType.Other)
        {
            mm.comments = txtComments.Text.Trim();
        }
        PDFCmpSesssion.misMatchList.Add(mm);
    }

    private MisMatchType getMisMatchType()
    {
        //return MisMatchType.Spell;
        MisMatchType misMatch = new MisMatchType();
        if (rbMissing.Checked)
            misMatch = MisMatchType.Miss;
        else if (rbExtra.Checked)
            misMatch = MisMatchType.Extra;
        else if (rbSpell.Checked)
            misMatch = MisMatchType.Spell;
        else if (rbOther.Checked)
            misMatch = MisMatchType.Other;
        return misMatch;
    }

    private void Finished()
    {
        ErrorsComplete(this, new CommandEventArgs("", ""));
    }

    protected void PDFCtrl_FileEdit(object sender, CommandEventArgs e)
    {
        FileEdit(sender, e);
    }

    public string PrintList()
    {
        return GetDataSet();
    }

    private string GetDataSet()
    {
        ///Need to Modify this function to add all the generated Reports of each page in session

        ArrayList misMatchList = PDFCmpSesssion.misMatchList;
        DataSet ds = new DataSet();
        DataTable dt = new DataTable("FileComparison");
        dt.Columns.Add("MismatchInlist1");
        dt.Columns.Add("MismatchInlist2");

        dt.Columns.Add("List1PageNo");
        dt.Columns.Add("List2PageNo");

        dt.Columns.Add("MismatchType");
        dt.Columns.Add("Comments");


        //int errorCount = SiteSession.errorHl.ErrorCount;
        int errorCount = SiteSession.ReportListForComments.Count;
        MisMatchError mme;

        //if (SiteSession.CurrentErrorIndex < errorCount)
        for (int i = 0; i < errorCount; i++)
        {
            mme = (MisMatchError)SiteSession.ReportListForComments[i];
            PdfWord wrd1 = mme.list1Word;
            int PageNum1 = wrd1.PageNumber;

            PdfWord wrd2 = mme.list2Word;
            int PageNum2 = wrd2.PageNumber;

            DataRow dr = dt.NewRow();

            MisMatch misMatchObj = mme.misMatch;

            try
            {
                dr[0] = wrd1.Text + "";// getArraylistAsStringCSV(tmp1List);
                dr[1] = wrd2.Text + "";// getArraylistAsStringCSV(tmp2List);

                dr[2] = wrd1.PageNumber;
                dr[3] = wrd2.PageNumber;
                if (misMatchObj != null)
                {
                    dr[4] = misMatchObj.misMatchType.ToString();
                }
                dr[5] = mme.commetns;
                dt.Rows.Add(dr);
            }
            catch
            {
                continue;
            }
        }

        ds.Tables.Add(dt);
        string defPath = Server.MapPath(".");

        String reportOutputPath = System.Configuration.ConfigurationManager.AppSettings["ReportOutput"];
        if (reportOutputPath == null)
        {
            ExcelHandler.FileWritePath = defPath + "\\ReportOutput.xls";
        }
        else
        {
            ExcelHandler.FileWritePath = System.Configuration.ConfigurationManager.AppSettings["ReportOutput"] + "\\ReportOutput.xls";
        }
        ExcelHandler.GenerateExcelFile(ds);
        return ExcelHandler.FileWritePath;
    }

    private ArrayList getListItemsAsArraylist(int startIndex, int length, ArrayList list)
    {
        ArrayList subArray = new ArrayList();
        for (int i = startIndex; i < startIndex + length; i++)
        {
            subArray.Add(list[i]);
        }
        return subArray;
    }

    private String getArraylistAsStringCSV(ArrayList arraylist)
    {
        StringBuilder sb = new StringBuilder();
        foreach (Word item in arraylist)
        {
            sb.Append(item.Text + ",");
        }
        return sb.ToString();
    }

    protected void rbOther_CheckedChanged(object sender, EventArgs e)
    {
        if (rbOther.Checked)
        {
            txtComments.Enabled = true;
            txtComments.Text = "";
        }
        else
        {
            txtComments.Enabled = false;
        }
    }

    protected void btnAddComment_Click(object sender, EventArgs e)
    {
        AddComment();
        ErrorsComplete(this, new CommandEventArgs("", ""));
    }

    private void AddComment()
    {
        MisMatch mm = MisMatchComments;
        if (rbExtra.Checked)
        {
            ;
            mm.misMatchType = MisMatchType.Extra;
        }
        else if (rbSpell.Checked)
        {
            mm.misMatchType = MisMatchType.Spell;
        }
        else if (rbMissing.Checked)
        {
            mm.misMatchType = MisMatchType.Miss;
        }
        else if (rbOther.Checked)
        {
            mm.misMatchType = MisMatchType.Other;
            mm.comments = this.txtComments.Text;
        }
        MisMatchComments = mm;
    }

    public void ShowFiles(int page)
    {
        //if (Convert.ToString(Session["srcPdfPagePath"]) == "")
        //    return;

        //string savePath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
        //string pdfFile = Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"]));

        //string stampedSrcPdf = "";
        //string stampePrddPdf = "";

        //string srcPdfFile = Path.GetFileNameWithoutExtension(Convert.ToString(Session["srcPdfPagePath"]));
        //string prdPdfFile = Path.GetFileNameWithoutExtension(Convert.ToString(Session["prdPdfPagePath"]));

        //if (srcPdfFile.Contains("_Stamped"))
        //{
        //    stampedSrcPdf = savePath + pdfFile.Replace("-1", "") + "\\" + pdfFile + "\\Comparison\\Comparison-" + Convert.ToString(Session["comparisonType"]) +
        //                    "\\" + Convert.ToString(Session["userId"]) + "\\" + srcPdfFile + ".pdf";

        //    stampePrddPdf = savePath + pdfFile.Replace("-1", "") + "\\" + pdfFile + "\\Comparison\\Comparison-" + Convert.ToString(Session["comparisonType"]) +
        //                    "\\" + Convert.ToString(Session["userId"]) + "\\" + prdPdfFile + ".pdf";
        //}
        //else
        //{
        //    //stampedSrcPdf = savePath + pdfFile + "\\" + pdfFile + "\\" + srcPdfFile + "_Stamped.pdf";
        //    //stampePrddPdf = savePath + pdfFile + "\\" + pdfFile + "\\" + prdPdfFile + "_Stamped.pdf";
        //    stampedSrcPdf = savePath + pdfFile.Replace("-1", "") + "\\" + pdfFile + "\\Comparison\\Comparison-" + Convert.ToString(Session["comparisonType"]) +
        //                    "\\" + Convert.ToString(Session["userId"]) + "\\" + srcPdfFile + "_Stamped.pdf";
        //    stampePrddPdf = savePath + pdfFile.Replace("-1", "") + "\\" + pdfFile + "\\Comparison\\Comparison-" + Convert.ToString(Session["comparisonType"]) +
        //                    "\\" + Convert.ToString(Session["userId"]) + "\\" + prdPdfFile + "_Stamped.pdf";
        //}

        string srcPDFPath = "";
        string prdPDFPath = "";

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
        //    srcPDFPath = Convert.ToString(Session["srcPdfPagePath"]);
        //    prdPDFPath = Convert.ToString(Session["prdPdfPagePath"]);
        //}

        srcPDFPath = Common.GetTaskFiles_SavingPath() + "\\" + page + ".pdf";
        prdPDFPath = Common.GetTaskFiles_SavingPath() + "\\" + page + "_Produced.pdf";

        ShowPDFInControl(this.pdf1.ID, srcPDFPath);
        ShowPDFInControl(this.pdf2.ID, prdPDFPath);

        //this.pdfCtrl.ShowTableDiv = true;
    }
}

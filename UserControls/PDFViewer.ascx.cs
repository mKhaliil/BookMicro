using System;
using System.Collections.Generic;
//using System.Data.Linq;
using System.Linq;
using System.Web;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Xml;
using System.IO;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.codec;
using iTextSharp.text.pdf.parser;
using System.Web.Services;
using System.Data.SqlClient;
using System.Data;

public partial class UserControls_PDFViewer : System.Web.UI.UserControl
{
    #region Fields and Properties

    public MisMatch MisMatchComments
    {
        set
        {
            PDFCmpSesssion.CurrentPageMisMatch = value;
        }
        get
        {
            return PDFCmpSesssion.CurrentPageMisMatch;
        }
    }

    private string xmlFile;
    MyDBClass obj = new MyDBClass();

    public string XMLFile
    {
        set
        {
            Session["XMLFile"] = value;
        }
        get
        {
            if (Session["XMLFile"] == null)
                Response.Redirect("Index.aspx");
            return Session["XMLFile"].ToString();
        }
    }

    public string PDFDirPath;
    public string GeneratedPDFPhyPath
    {
        set
        {
            Session["GeneratedPDFPhyPath"] = value;
        }
        get
        {
            return Session["GeneratedPDFPhyPath"].ToString();
        }
    }

    public string GeneratedPDFVirPath
    {
        set
        {
            Session["GeneratedPDFVirPath"] = value; ;
        }
        get
        {
            return Session["GeneratedPDFVirPath"].ToString();
        }
    }


    #endregion

    #region Page Events

    protected void Page_PreInit(object sender, EventArgs e)
    {
        string[] dummy = PDFCmpSesssion.SecondPDFPath.Split('\\');
        string path = "PDFs/2.pdf";//dummy[dummy.Length - 3] + "/" + dummy[dummy.Length - 2] + "/" + dummy[dummy.Length - 1];
        PDFFile = path;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        if ((Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test")) ||
            (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest")) ||
            (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("comparisonEntryTest")))
        {
            divComments.Visible = false;
            divCommentsLabel.Visible = false;
            divNodeType.Visible = false;
        }

        if (Session["MainXMLFilePath"] != null)
        {
            string fileName = Session["MainXMLFilePath"].ToString().Replace(".xml", "original.xml").Replace(".rhyw", "original.rhyw");
            //int curentPage = SiteSession.MainCurrPage;
            //var aa = GetTableText();
            //hfTable.Value = aa;

            if (!File.Exists(fileName))
            {
                //var aa = GetTableText();
                //hfTable.Value = aa;
                StreamReader strreader = new StreamReader(Convert.ToString(Session["MainXMLFilePath"]));
                string xmlInnerText = strreader.ReadToEnd();
                strreader.Close();

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xmlInnerText);
                xmlDoc.Save(fileName);
                Session["ActualFile"] = fileName;
            }
        }

        PDFDirPath = ConfigurationManager.AppSettings["HighlightDirPP"];
        string vDirPath = ConfigurationManager.AppSettings["HighlightDirVP"];
        if (!Page.IsPostBack)
        {
            //string []dummy = PDFCmpSesssion.SecondPDFPath.Split('.');

            //xmlFile = dummy[0]+".xml";



            //string[] dummy = PDFCmpSesssion.SecondPDFPath.ToString().Split('\\');
            //string book_dir = "" + dummy[dummy.Length - 2];
            //PDFFile = dummy[dummy.Length - 3] + "/" + dummy[dummy.Length - 2] + "/" + dummy[dummy.Length - 1];
            // PDFFile = PDFCmpSesssion.SecondPDFPath;

        }
    }

    #endregion

    #region Local Functions

    #endregion

    public event CommandEventHandler FileEdit;


    public string PDFFile
    {
        set
        {
            String newFIlePath = value;
            if (value.Contains(":"))
            {
                String fileName = System.IO.Path.GetFileName(value);
                String dir = Directory.GetParent(value).Name;
                String virDir = ConfigurationManager.AppSettings["PDFDirVirPath"];
                newFIlePath = virDir + "/" + System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"])) + "/" + dir + "/" + fileName;
            }
            FileLoadPath.Value = newFIlePath;
        }
        get
        {
            return FileLoadPath.Value;
        }
    }

    public bool ShowTableDiv
    {
        set
        {
            divTableText.Visible = value;
        }
    }




    public void LoadTableText()
    {
        //int curentPage = SiteSession.MainCurrPage;
        var aa = GetTableText();
    }

    static string RemoveWhitespace(string input)
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

    private int GetTotalMistakes()
    {
        StreamReader strreader = new StreamReader(Convert.ToString(Session["MainXMLFilePath"]));
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        int totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistake]").Count;

        return totalMistakes;
    }

    private int GetTotalMistakes_PerPage(string page)
    {
        StreamReader strreader = new StreamReader(Convert.ToString(Session["MainXMLFilePath"]));
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        //count(//ln[@mistake="" and @page=1] ) 

        int mistakes = xmlDoc.SelectNodes(@"//*[@PDFmistake!="" and @page=" + page + "]").Count;

        //int mistakes = xmlDoc.SelectNodes(@"//*[@mistake]").Count;

        return mistakes;
    }

    private List<Mistakes> GetTotalMistakes_List()
    {
        List<Mistakes> list = new List<Mistakes>();

        StreamReader strreader = new StreamReader(Convert.ToString(Session["MainXMLFilePath"]));
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        XmlNodeList nodes = xmlDoc.SelectNodes(@"//*[@PDFmistake]");

        int i = 1;

        if (nodes.Count > 0)
        {
            foreach (XmlElement node in nodes)
            {
                list.Add(new Mistakes
                {
                    mistakeNum = i,
                    page = Convert.ToInt32(node.Attributes["page"].Value),
                    comments = Convert.ToString(node.Attributes["Comments"].Value),
                    mistakeId = Convert.ToString(node.Attributes["QaMistakeId"].Value)
                });

                i++;
            }
        }

        return list;
    }

    public bool IsFileLocked(FileInfo file)
    {
        FileStream stream = null;

        try
        {
            stream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        }
        catch (IOException)
        {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            return true;
        }
        finally
        {
            if (stream != null)
                stream.Close();
        }

        //file is not locked
        return false;
    }

    public List<String> GetTextFromPrdPdf(string text)
    {
        StringBuilder line = new StringBuilder();
        StringBuilder newLine = new StringBuilder();
        double top = 0.0;

        String[] temp = text.Split(new string[] { "data-canvas-width" }, StringSplitOptions.None);
        List<String> list_Lines = new List<string>();
        string previousYPosition = "";
        string previousYPosition1 = "";
        int lineNum = 0;
        int i = 0;
        bool endLine = false;

        foreach (var item in temp)
        {
            i++;
            var wordDetails = item.Split(new string[] { "</div><div" }, StringSplitOptions.None);
            wordDetails = wordDetails.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (wordDetails.Length > 1)
            {
                string word = wordDetails[0].Split(new string[] { ">" }, StringSplitOptions.None)[1];

                //string temp_yPosition = wordDetails[1].Substring(wordDetails[1].IndexOf("top:"), 26);

                string temp_yPosition = wordDetails[0].Substring(wordDetails[0].IndexOf("top:"), 26);

                int px_Index = temp_yPosition.IndexOf("px");

                string yPosition = wordDetails[0].Substring(wordDetails[0].IndexOf("top:"), px_Index).Trim();

                if ((yPosition == previousYPosition) || (previousYPosition == ""))
                {
                    line.Append(newLine + "" + word + "");
                    endLine = false;
                    newLine.Remove(0, newLine.Length);
                }
                else
                {
                    endLine = true;
                    newLine.Append(word + "");
                }

                previousYPosition = yPosition;
            }

            if ((endLine) || (i == temp.Length))
            {
                if (line.ToString() != "")
                {
                    lineNum++;
                    list_Lines.Add(line.ToString().Replace("&nbsp;", " ").Trim());
                    line.Remove(0, line.Length);
                    previousYPosition = "";
                }
            }
        }
        return list_Lines;
    }

    //public string CreateLineByUry(XmlNodeList innerwords)
    //{
    //    string temp_Ury = "";
    //    string temp_Lines = "";
    //    int count = 0;
    //    bool match = false;

    //    StringBuilder tableLine = new StringBuilder();
    //    List<double> lines = new List<double>();
    //    List<double> sortedLines = new List<double>();

    //    if ((innerwords != null) && (innerwords.Count > 0))
    //    {
    //        //Get total number of lines from tetml words
    //        for (int t = 0; t < innerwords.Count; t++)
    //        {
    //            if (temp_Lines != innerwords[t].NextSibling.Attributes["ury"].Value)
    //            {
    //                lines.Add(Convert.ToDouble(innerwords[t].NextSibling.Attributes["ury"].Value));
    //            }

    //            temp_Lines = Convert.ToString(innerwords[t].NextSibling.Attributes["ury"].Value);
    //        }

    //        var total_Lines = lines.Distinct();

    //        foreach (var item in total_Lines.OrderByDescending(x => x))
    //        {
    //            sortedLines.Add(item);
    //        }

    //        //Get text from tetml words line by line by matching ury with table/line text
    //        for (int i = 0; i < sortedLines.Count; i++)
    //        {
    //            count = 0;
    //            match = false;

    //            for (int j = 0; j < innerwords.Count; j++)
    //            {
    //                if (Convert.ToDouble(innerwords[j].NextSibling.Attributes["ury"].Value).Equals(Convert.ToDouble(sortedLines[i])))
    //                {
    //                    if (count == 0)
    //                        tableLine.Append(innerwords[j].NextSibling.Attributes["llx"].Value + ":" + innerwords[j].NextSibling.Attributes["lly"].Value + ":" + innerwords[j].NextSibling.Attributes["urx"].Value + ":" + innerwords[j].NextSibling.Attributes["ury"].Value + " ");

    //                    tableLine.Append(innerwords[j].NextSibling.InnerText + " ");
    //                    count++;
    //                    match = true;
    //                }
    //            }

    //            if (match)
    //                tableLine.Append("~NewLine~");
    //        }
    //    }
    //    return tableLine.ToString();
    //}

    public PdfText[] CreateLineByUry(XmlNodeList innerwords)
    {
        string temp_Ury = "";
        string temp_Lines = "";
        int count = 0;
        bool match = false;

        StringBuilder tableLine = new StringBuilder();
        List<double> lines = new List<double>();
        List<double> sortedLines = new List<double>();

        List<PdfText> list = new List<PdfText>();
        PdfText[] sortedCoord = null;

        if ((innerwords != null) && (innerwords.Count > 0))
        {
            //Get total number of lines from tetml words
            for (int t = 0; t < innerwords.Count; t++)
            {
                if (temp_Lines != innerwords[t].NextSibling.Attributes["ury"].Value)
                {
                    lines.Add(Convert.ToDouble(innerwords[t].NextSibling.Attributes["ury"].Value));
                }

                temp_Lines = Convert.ToString(innerwords[t].NextSibling.Attributes["ury"].Value);
            }

            var total_Lines = lines.Distinct();

            foreach (var item in total_Lines.OrderByDescending(x => x))
            {
                sortedLines.Add(item);
            }

            //Get text from tetml words line by line by matching ury with table/line text
            for (int i = 0; i < sortedLines.Count; i++)
            {
                count = 0;
                match = false;

                for (int j = 0; j < innerwords.Count; j++)
                {
                    if (Convert.ToDouble(innerwords[j].NextSibling.Attributes["ury"].Value).Equals(Convert.ToDouble(sortedLines[i])))
                    {
                        list.Add(new PdfText
                        {
                            llx = Convert.ToDouble(innerwords[j].NextSibling.Attributes["llx"].Value),
                            lly = Convert.ToDouble(innerwords[j].NextSibling.Attributes["lly"].Value),
                            urx = Convert.ToDouble(innerwords[j].NextSibling.Attributes["urx"].Value),
                            ury = Convert.ToDouble(innerwords[j].NextSibling.Attributes["ury"].Value),
                            Word = innerwords[j].NextSibling.InnerText
                        }
                        );
                    }
                }
            }

            var tt = list;

            sortedCoord = list.OrderByDescending(y => y.lly).ThenBy(x => x.llx).ToArray();
        }

        return sortedCoord;
    }

    //public string GetTableMargin_Coord(string pageNum, string text)
    //{
    //    string lly = "";
    //    string ury = "";
    //    string llx = "";
    //    string urx = "";
    //    string savePath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
    //    string mainPdfFile = Path.GetFileNameWithoutExtension(Convert.ToString(Session["pdfName"]));
    //    string tetFilePath = "";

    //    tetFilePath = savePath + mainPdfFile + "\\" + pageNum + ".tetml";

    //    XmlDocument tetDoc = new XmlDocument();
    //    try
    //    {
    //        StreamReader sr = new StreamReader(tetFilePath);
    //        string xmlText = sr.ReadToEnd();
    //        sr.Close();
    //        string documentXML =
    //            System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>",
    //                System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
    //        tetDoc.LoadXml(documentXML);
    //    }
    //    catch (Exception ex)
    //    {

    //    }

    //    XmlNodeList pages = tetDoc.SelectNodes("//Page");

    //    foreach (XmlNode page in pages)
    //    {
    //        var textToCheck = Regex.Split(text, @"\s+");
    //        textToCheck = textToCheck.Where(x => !string.IsNullOrEmpty(x)).ToArray();

    //        XmlNodeList innerwords = page.SelectNodes("//Text");

    //        var textFromWords = CreateLineByUry(innerwords);

    //        string[] newwords = textFromWords[0].ToString().Split(new string[] { "~NewLine~" }, StringSplitOptions.None);
    //        newwords = newwords.Where(x => !string.IsNullOrEmpty(x)).ToArray();

    //        bool find = false;

    //        if (newwords.Length > 0)
    //        {
    //            for (int i = 0; find == false; i++)
    //            {
    //                if (i >= newwords.Length)
    //                {
    //                    break;
    //                }

    //                var word = Regex.Split(newwords[i], @"\s+");
    //                //word = word.Where(x => !string.IsNullOrEmpty(x)).Where(w => w != word[0]).ToArray();

    //                word = word.Where(x => !string.IsNullOrEmpty(x)).Where(w => w != word[0]).ToArray();

    //                for (int j = 0; j < word.Count(); j++)
    //                {
    //                    if ((word.Length > 0) && (textToCheck.Length > 0))
    //                    {
    //                        if (word[j].Replace(",", "").Trim().Equals(textToCheck[0]))
    //                        {
    //                            //Calculating coordinate if there is only 1 word in the line above table
    //                            if (word.Length == 1)
    //                            {
    //                                llx = newwords[i].Split(' ')[0].Split(':')[0];
    //                                lly = newwords[i].Split(' ')[0].Split(':')[1];
    //                                urx = newwords[i].Split(' ')[0].Split(':')[2];
    //                                ury = newwords[i].Split(' ')[0].Split(':')[3];
    //                                find = true;
    //                                break;
    //                            }//end

    //                            if ((word.Length > 1) && (j < word.Length - 1) && (textToCheck.Length > 1))
    //                            {
    //                                if (word[j + 1].Replace(",", "").Trim().Equals(textToCheck[1]))
    //                                {
    //                                    //Calculating coordinate if there are 2 words in the line above table
    //                                    //if (word.Length == 2)
    //                                    //{
    //                                    llx = newwords[i].Split(' ')[0].Split(':')[0];
    //                                    lly = newwords[i].Split(' ')[0].Split(':')[1];
    //                                    urx = newwords[i].Split(' ')[0].Split(':')[2];
    //                                    ury = newwords[i].Split(' ')[0].Split(':')[3];
    //                                    find = true;
    //                                    break;
    //                                    //}//end

    //                                    //if ((word.Length > 2) && (j < word.Length - 2) && (textToCheck.Length > 2))
    //                                    //{
    //                                    //    if (word[j + 2].Replace(",", "").Trim().Equals(textToCheck[2]))
    //                                    //    {
    //                                    //        //Calculating coordinate if words are greator then 2 in the line above table
    //                                    //        llx = newwords[i].Split(' ')[0].Split(':')[0];
    //                                    //        lly = newwords[i].Split(' ')[0].Split(':')[1];
    //                                    //        urx = newwords[i].Split(' ')[0].Split(':')[2];
    //                                    //        ury = newwords[i].Split(' ')[0].Split(':')[3];
    //                                    //        find = true;
    //                                    //        break;
    //                                    //    }
    //                                    //}
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    if (llx == "")
    //        llx = "0";

    //    if (lly == "")
    //        lly = "0";

    //    if (urx == "")
    //        urx = "0";

    //    if (ury == "")
    //        ury = "0";

    //    return llx + ":" + lly + ":" + urx + ":" + ury;
    //}

    public string GetTableMargin_Coord(string pageNum, string text)
    {
        string lly = "";
        string ury = "";
        string llx = "";
        string urx = "";
        string tetFilePath = "";

        tetFilePath = Common.GetTaskFiles_SavingPath() + "\\" + pageNum + ".tetml";

        if (!File.Exists(tetFilePath))
            return null;

        XmlDocument tetDoc = new XmlDocument();
        try
        {
            StreamReader sr = new StreamReader(tetFilePath);
            string xmlText = sr.ReadToEnd();
            sr.Close();
            string documentXML =
                System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>",
                    System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
            tetDoc.LoadXml(documentXML);
        }
        catch (Exception ex)
        {

        }

        XmlNodeList pages = tetDoc.SelectNodes("//Page");

        foreach (XmlNode page in pages)
        {
            var textToCheck = Regex.Split(text, @"\s+");
            textToCheck = textToCheck.Where(x => !string.IsNullOrEmpty(x)).ToArray();

            XmlNodeList innerwords = page.SelectNodes("//Text");

            PdfText[] textFromWords = CreateLineByUry(innerwords);

            bool find = false;

            if (textFromWords.Length > 0)
            {
                for (int i = 0; find == false; i++)
                {
                    if (i >= textFromWords.Length)
                    {
                        break;
                    }

                    for (int j = 0; j < textFromWords.Count(); j++)
                    {
                        if ((textFromWords.Length > 0) && (textToCheck.Length > 0))
                        {
                            if (textFromWords[j].Word.Trim().Equals(textToCheck[0].Trim()))
                            {
                                //Calculating coordinate if there is only 1 word in the line above table
                                if (textToCheck.Length == 1)
                                {
                                    llx = Convert.ToString(textFromWords[j].llx);
                                    lly = Convert.ToString(textFromWords[j].lly);
                                    urx = Convert.ToString(textFromWords[j].urx);
                                    ury = Convert.ToString(textFromWords[j].ury);
                                    find = true;
                                    break;
                                }//end

                                if ((textFromWords.Length > 1) && (j < textFromWords.Length - 1) && (textToCheck.Length > 1))
                                {
                                    if (textFromWords[j + 1].Word.Trim().Equals(textToCheck[1]))
                                    {
                                        //Calculating coordinate if there are 2 words in the line above table
                                        if (textToCheck.Length == 2)
                                        {
                                            llx = Convert.ToString(textFromWords[j].llx);
                                            lly = Convert.ToString(textFromWords[j].lly);
                                            urx = Convert.ToString(textFromWords[j].urx);
                                            ury = Convert.ToString(textFromWords[j].ury);
                                            find = true;
                                            break;
                                        }//end

                                        if ((textFromWords.Length > 2) && (j < textFromWords.Length - 2) && (textToCheck.Length > 2))
                                        {
                                            if (textFromWords[j + 2].Word.Trim().Equals(textToCheck[2]))
                                            {
                                                //Calculating coordinate if words are greator then 2 in the line above table
                                                llx = Convert.ToString(textFromWords[j].llx);
                                                lly = Convert.ToString(textFromWords[j].lly);
                                                urx = Convert.ToString(textFromWords[j].urx);
                                                ury = Convert.ToString(textFromWords[j].ury);
                                                find = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        if (llx == "")
            llx = "0";

        if (lly == "")
            lly = "0";

        if (urx == "")
            urx = "0";

        if (ury == "")
            ury = "0";

        return llx + ":" + lly + ":" + urx + ":" + ury;
    }

    public List<String> GetTextFromSrcPdf(String pdfPath)
    {
        var pdfReader = new PdfReader(pdfPath);
        List<String> list_Lines = new List<string>();
        int lineNum = 0;

        for (var page = 1; page <= pdfReader.NumberOfPages; page++)
        {
            ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();

            var currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

            string[] words = currentText.Split('\n');

            foreach (var item in words)
            {
                lineNum++;
                list_Lines.Add(item.Trim());
            }
        }

        return list_Lines;
    }

    public PdfText[] GetTableTextLineByLine(string pageNum, string lineNearTable)
    {
        string tetFilePath = "";
        PdfText[] textFromWords = null;

        tetFilePath = Common.GetTaskFiles_SavingPath() +  "\\" + pageNum + ".tetml";

        if (!File.Exists(tetFilePath))
            return null;

        XmlDocument tetDoc = new XmlDocument();
        try
        {
            StreamReader sr = new StreamReader(tetFilePath);
            string xmlText = sr.ReadToEnd();
            sr.Close();
            string documentXML =
                System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>",
                    System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
            tetDoc.LoadXml(documentXML);
        }
        catch (Exception ex)
        {

        }

        XmlNodeList pages = tetDoc.SelectNodes("//Page");

        foreach (XmlNode page in pages)
        {
            XmlNodeList innerwords = page.SelectNodes("//Text");

            textFromWords = CreateLineByUry(innerwords);
        }

        return textFromWords;
    }

    //public List<PdfText> SortTableTextByLine(PdfText[] PageWords, string[] tableWords)
    //{
    //    List<PdfText> tableText_Temp = new List<PdfText>();
    //    double tempYCoord = 0;
    //    bool found = false;
    //    int counter = 0;

    //    double currentLine_YCoord = 0;

    //    if (tableWords.Length > 0)
    //    {
    //        for (int i = 0; i < tableWords.Count(); i++)
    //        {
    //            for (int j = 0; j < PageWords.Count(); j++)
    //            {
    //                if (tableWords[i].Equals(PageWords[j].Word))
    //                {
    //                    for (int k = i; k < tableWords.Count(); k++)
    //                    {
    //                        if (tableWords[k].Equals(PageWords[j + k].Word))
    //                        {
    //                            tableText_Temp.Add(new PdfText
    //                            {
    //                                Word = PageWords[j + k].Word,
    //                                llx = PageWords[j + k].llx,
    //                                lly = PageWords[j + k].lly,
    //                                urx = PageWords[j + k].urx,
    //                                ury = PageWords[j + k].ury
    //                            });
    //                            counter++;
    //                            //currentLine_YCoord = PageWords[j + k].ury;
    //                        }
    //                        //else
    //                        //{

    //                        //}
    //                    }

    //                    if (counter <= tableWords.Count())
    //                    {
    //                        found = true;
    //                        break;
    //                    }
    //                }

    //                if (found)
    //                    break;
    //            }

    //            if (found)
    //                break;
    //        }
    //    }

    //    var aa = tableText_Temp;

    //    return tableText_Temp;
    //}

    public List<PdfText> SortTableTextByLine(PdfText[] PageWords, string[] tableWords)
    {
        if (PageWords == null)
            return null;

        List<PdfText> tableText_Temp = new List<PdfText>();
        bool found = false;

        if (tableWords.Length > 0)
        {
            for (int i = 0; i < tableWords.Count(); i++)
            {
                for (int j = 0; j < PageWords.Count(); j++)
                {
                    if (tableWords[i].Equals(PageWords[j].Word))
                    {
                        tableText_Temp = MatchPageTextWithTable(j, PageWords, tableWords);
                        if ((tableText_Temp != null) && (tableText_Temp.Count > 0))
                        {
                            found = true;
                            break;
                        }
                    }
                }

                if (found)
                    break;
            }
        }

        var aa = tableText_Temp;

        return tableText_Temp;
    }

    public List<PdfText> MatchPageTextWithTable(int startIndex, PdfText[] pageWords, string[] tableWords)
    {
        List<PdfText> tableText_Temp = new List<PdfText>();
        int endIndex = startIndex + tableWords.Length;
        int counter = 0;

        for (int i = startIndex; i < endIndex; i++)
        {
            for (int j = 0; j < tableWords.Length; j++)
            {
                if (pageWords[i].Word.Equals(tableWords[j]))
                {
                    counter++;
                    break;
                }
            }
        }

        if (counter == tableWords.Length)
        {
            for (int i = startIndex; i < endIndex; i++)
            {
                tableText_Temp.Add(new PdfText
                {
                    Word = pageWords[i].Word,
                    llx = pageWords[i].llx,
                    lly = pageWords[i].lly,
                    urx = pageWords[i].urx,
                    ury = pageWords[i].ury
                });
            }
            return tableText_Temp;
        }

        return null;
    }

    public void SetTablePosition(string pageNum, double pageHeight, double top, string tableText, string table_LineText, string linePosition, int rowCount, string tableId)
    {
        double tableTop = 0;
        double tableMarginFromLine = 0;
        string tableStartLineWords = "";
        string tableEndLineWords = "";
        string coord = "";
        double tableTotalHeight = 0;
        double tableRowHeight = 0;
        string tableEndLine = null;
        string tableFullText = "";

        //Finding table text last line for current table
        var temp = tableText.Replace("\n", "").Split(new string[] { "~//~" }, StringSplitOptions.None);
        temp = temp.Where(x => !string.IsNullOrEmpty(x)).ToArray();

        if ((temp.Length == 1) && (temp[0] != ""))
        {
            string text = temp[0].ToString();
            tableFullText = text;
            tableEndLine = text.Substring(text.Length - 50, text.Length - text.Length + 50);
        }

        else if (temp.Length > 1)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                if (Convert.ToString(i) == tableId)
                {
                    tableEndLine = temp[i];
                    tableFullText = temp[i];
                    break;
                }
            }
        }

        string[] tblText = Regex.Split(tableFullText, @"\s+");
        tblText = tblText.Where(x => !string.IsNullOrEmpty(x)).ToArray();

        List<PdfText> tableTextLineByLine = null;
        double lineYCoord = 0;
        double lineYCoord_FromList = 0;

        //Get text of a page from tetml with coordinates in order to find table coordinates
        PdfText[] Table_Temp = GetTableTextLineByLine(pageNum, table_LineText);

        //Get table text with coordinates from tetml after matching table text obtained from xml file.Table text has no line coordinates.
        tableTextLineByLine = SortTableTextByLine(Table_Temp, tblText);

        //Getting first line of table text
        if ((tableTextLineByLine != null) && (tableTextLineByLine.Count() > 0))
        {
            if (tableTextLineByLine.Count() == 1)
                tableStartLineWords = tableTextLineByLine[0].Word;

            if (tableTextLineByLine.Count() == 2)
                tableStartLineWords = tableTextLineByLine[0].Word + " " + tableTextLineByLine[1].Word;

            if (tableTextLineByLine.Count() >= 3)
                tableStartLineWords = tableTextLineByLine[0].Word + " " + tableTextLineByLine[1].Word + " " + tableTextLineByLine[2].Word;
        }

        //Getting last line of table text
        if ((tableTextLineByLine != null) && (tableTextLineByLine.Count() > 0))
        {
            if (tableTextLineByLine.Count() == 1)
                tableEndLineWords = tableTextLineByLine[tableTextLineByLine.Count() - 1].Word;

            if (tableTextLineByLine.Count() == 2)
                tableEndLineWords = tableTextLineByLine[tableTextLineByLine.Count() - 2].Word + " " + tableTextLineByLine[tableTextLineByLine.Count() - 1].Word;

            if (tableTextLineByLine.Count() >= 3)
                tableEndLineWords = tableTextLineByLine[tableTextLineByLine.Count() - 3].Word + " " + tableTextLineByLine[tableTextLineByLine.Count() - 2].Word + " " + tableTextLineByLine[tableTextLineByLine.Count() - 1].Word;
        }

        //Getting coordinates for table start line and end line
        string table_TopLine_Coord = GetTableMargin_Coord(pageNum, tableStartLineWords);
        string table_bottomLine_Coord = GetTableMargin_Coord(pageNum, tableEndLineWords);

        //Getting coordinates for line before or after the table 
        //(lly = bottom y coordinate and ury =  top y coordinate of a line, llx = left x coordinate and urx = right x coordinate of a line)
        string ury_NearTableLine = GetTableMargin_Coord(pageNum, table_LineText);

        tableTotalHeight = Convert.ToDouble(table_TopLine_Coord.Split(':')[3]) - Convert.ToDouble(table_bottomLine_Coord.Split(':')[1]);

        double lineStartX = 0;
        double lineEndX = 0;
        double lineStartY = 0;
        double lineEndY = 0;

        var temp_LineCoord = ury_NearTableLine.Split(':');

        if ((temp_LineCoord != null) && (temp_LineCoord.Length > 0))
        {
            lineStartX = Convert.ToDouble(temp_LineCoord[0]);
            lineStartY = Convert.ToDouble(temp_LineCoord[1]);
            lineEndX = Convert.ToDouble(temp_LineCoord[2]);
            lineEndY = Convert.ToDouble(temp_LineCoord[3]);
        }

        if (linePosition.Equals("lineBefore"))
        {
            tableMarginFromLine = Math.Abs(lineStartY - Convert.ToDouble(table_TopLine_Coord.Split(':')[3]));
            tableTop = (lineStartY - tableMarginFromLine - (tableTotalHeight / (2)));
        }
        else if (linePosition.Equals("lineAfter"))
        {
            tableMarginFromLine = Math.Abs(lineEndY - Convert.ToDouble(table_bottomLine_Coord.Split(':')[1]));
            tableTop = (lineEndY + tableMarginFromLine + (tableTotalHeight / (2)));
        }
        else
        {
            tableMarginFromLine = Math.Abs(Convert.ToDouble(table_TopLine_Coord.Split(':')[3]));
            tableTop = Convert.ToDouble(ury_NearTableLine.Split(':')[1]) - tableMarginFromLine - (tableTotalHeight / (2));
        }

        //coord = table_bottomLine_Coord.Split(':')[0] + ":" + table_bottomLine_Coord.Split(':')[1] + ":" +
        //        table_TopLine_Coord.Split(':')[2] + ":" + table_TopLine_Coord.Split(':')[3];

        //tableTotalHeight = Convert.ToDouble(table_TopLine_Coord.Split(':')[3]) - Convert.ToDouble(table_bottomLine_Coord.Split(':')[1]);
        //tableRowHeight = (tableTotalHeight / rowCount + 4);

        //Update main xml file with height, top and left values of table
        string mainXmlPath = Convert.ToString(Session["MainXMLFilePath"]);
        StreamReader sr_mainXml = new StreamReader(mainXmlPath);
        string xmlFile = sr_mainXml.ReadToEnd();
        sr_mainXml.Close();
        XmlDocument xmlDocOrigXml = new XmlDocument();
        xmlDocOrigXml.LoadXml(xmlFile);

        SiteSession.xmlDoc = xmlDocOrigXml;

        XmlNodeList nodes = xmlDocOrigXml.SelectNodes(@"//body");

        foreach (XmlNode node in nodes)
        {
            if ((node != null) && (node.ChildNodes.Count > 0))
            {
                if (node.ChildNodes[0].Name != "image")
                {
                    foreach (XmlElement tableNode in node)
                    {
                        if (tableNode.Name == "table")
                        {
                            if ((tableNode.ChildNodes[0].Attributes[1].Value == pageNum) && (tableNode.Attributes["id"].Value == tableId))
                            {
                                //if (!(tableNode.HasAttribute("height") && tableNode.HasAttribute("top")))
                                //{
                                //XmlAttribute attr_coord = xmlDocOrigXml.CreateAttribute("coord");
                                //attr_coord.Value = coord;
                                //tableNode.SetAttributeNode(attr_coord);

                                XmlAttribute attr_Height = xmlDocOrigXml.CreateAttribute("height");
                                attr_Height.Value = Convert.ToString(pageHeight);
                                tableNode.SetAttributeNode(attr_Height);

                                XmlAttribute attr_Top = xmlDocOrigXml.CreateAttribute("top");
                                attr_Top.Value = Convert.ToString(tableTop);
                                tableNode.SetAttributeNode(attr_Top);
                                //}

                                //XmlAttribute attr_Left = xmlDocOrigXml.CreateAttribute("left");
                                //attr_Left.Value = Convert.ToString("96.79");
                                //tableNode.SetAttributeNode(attr_Left);
                            }
                        }
                    }
                }
            }
        }

        xmlDocOrigXml.Save(mainXmlPath);

        //PdfCompareGlobalVar.XMLPath = mainXmlPath;

        //PdfCompareGlobalVar.SaveXml(mainXmlPath);


        //string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);
        //if (String.IsNullOrEmpty(mainXmlPath))
        //    return null;

        Common obj = new Common();
        XmlDocument xmlFromRhyw = obj.LoadXmlFromFile(mainXmlPath.Replace(".xml", ".rhyw"));

        //Get selected page xml from updated xml
        XmlDocument pageXML = Common.GetPageXmlDoc(pageNum, xmlFromRhyw);

        //string dirPath = ConfigurationManager.AppSettings["HighlightDirPP"];
        //string pageXMLSavedPath = dirPath + "\\" + Convert.ToString(Session["pdfName"]).Replace(".pdf", "") +
        //                          "\\Produced_" + pageNum + ".xml";

        string pageXMLSavedPath = Common.GetTaskFiles_SavingPath() + "\\Produced_" + pageNum + ".xml";

        pageXML.Save(pageXMLSavedPath);
    }

    public string GetTableText()
    {
        int page = Convert.ToInt32(Session["MainCurrPage"]);
        string dirPath = ConfigurationManager.AppSettings["HighlightDirPP"];

        //string CurrentXMLFilePath = dirPath + "\\" + Convert.ToString(Session["pdfName"]).Replace(".pdf", "") + "\\Produced_" + page + ".xml";
        string CurrentXMLFilePath = Common.GetTaskFiles_SavingPath() + "\\Produced_" + page + ".xml";
        if ((CurrentXMLFilePath == "") || (!(File.Exists(CurrentXMLFilePath))))
            return null;

        StreamReader strreader = new StreamReader(CurrentXMLFilePath);
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);
        string tableText = "";
        string tText = "";
        string tHeader = "";
        string tHeadRow = "";
        string tHeaderCol = "";
        string tRow = "";
        string tCol = "";
        int strartIndex = 0;
        int endIndex = 0;
        string text = "";
        string headerRows = "";
        string caption = "";
        string thText = "";
        string tCaption = "";
        int count = 0;
        int rowCount = 0;

        List<string> newText = null;
        List<string> rowList = null;

        StringBuilder sb = new StringBuilder();
        StringBuilder sb_TableText = new StringBuilder();

        XmlNodeList list_Tables = xmlDoc.SelectNodes(@"//table");

        if ((list_Tables != null) && (list_Tables.Count > 0))
        {
            foreach (XmlNode table in list_Tables)
            {
                tableText = "";
                tText = "";
                tHeader = "";
                tHeadRow = "";
                tHeaderCol = "";
                tRow = "";
                tCol = "";
                strartIndex = 0;
                endIndex = 0;
                text = "";
                headerRows = "";
                caption = "";
                thText = "";
                tCaption = "";
                rowCount = 0;

                newText = new List<string>();
                rowList = new List<string>();

                count++;
                tText = table.InnerXml;

                if (tText == "")
                    return null;

                tHeader = tText.Split(new string[] { "<header>" }, StringSplitOptions.None)[1].Split(new string[] { "</header>" }, StringSplitOptions.None)[0];

                sb_TableText.Append(tHeader + " ");

                tHeadRow = tText.Split(new string[] { "<head-row>" }, StringSplitOptions.None)[1].Split(new string[] { "</head-row>" }, StringSplitOptions.None)[0];

                var wtHeaderCol = tHeadRow.Split(new string[] { "<head-col>" }, StringSplitOptions.None);

                var tt = wtHeaderCol[0].Split(new string[] { "</head-col>" }, StringSplitOptions.None);
                tt = tt.Where(x => !string.IsNullOrEmpty(x)).ToArray();

                foreach (var item in tt)
                {
                    strartIndex = item.IndexOf("<head-col");
                    endIndex = item.IndexOf(">");

                    text = item.Substring(strartIndex, endIndex + 1);
                    thText = item.Replace(text, "").Replace("\r", "");
                    newText.Add(thText);
                    sb_TableText.Append(thText + " ");
                }

                sb.Append("<div>" + tHeader + "</div>");
                sb.Append("<table border='1'>");

                sb.Append("<tr style='white-space:nowrap'>");
                foreach (var hRow in newText)
                {
                    sb.Append("<th >" + hRow + "</th>");
                }
                sb.Append("</tr>");

                var rows_Temp = tText.Split(new string[] { "<header>" }, StringSplitOptions.None)[1].Split(new string[] { "</header>" }, StringSplitOptions.None)[1];

                var t_StartIndex = rows_Temp.IndexOf("<head-row>");
                var T_endIndex = rows_Temp.IndexOf("</head-row>");

                var text_Temp = rows_Temp.Substring(t_StartIndex, T_endIndex + 11);
                rows_Temp = rows_Temp.Replace(text_Temp, "");

                int row_StartIndex = 0;
                int row_EndIndex = 0;

                foreach (var item in rows_Temp)
                {
                    row_StartIndex = rows_Temp.IndexOf("<row><col>");
                    row_EndIndex = rows_Temp.IndexOf("</col></row>");

                    if ((row_StartIndex != -1) && (row_EndIndex != -1))
                    {
                        var new_Temp = rows_Temp.Substring(row_StartIndex, row_EndIndex + 12);
                        rows_Temp = rows_Temp.Replace(new_Temp, "");

                        rowList.Add(new_Temp.Replace("<row>", "").Replace("</row>", "").Replace("<col>", ""));
                    }
                    else
                    {
                        caption = rows_Temp.Replace("<caption>", "").Replace("</caption>", "").Replace("</tbody>", "");
                        break;
                    }
                }

                rowCount = rowList.Count;
                string normalText = "";
                string supScript = "";
                string subScript = "";

                foreach (string row in rowList)
                {
                    var temp_row = row.Split(new string[] { "</col>" }, StringSplitOptions.None);

                    sb.Append("<tr style='white-space:nowrap'>");
                    foreach (var item in temp_row)
                    {
                        if (item != "")
                        {
                            //sb.Append("<td>" + "<input type='text' value='" + item.Replace("\r", "").Replace("<sup>", " <sup>") + "'>" + "</td>");
                            sb.Append("<td>" + "<div contenteditable='true'>" + item.Replace("\r", "").Replace("<sup>", " <sup>") + "</div>" + "</td>");
                            sb_TableText.Append(item.Replace("\r", "").Replace("<sup>", " ").Replace("</sup>", " ") + " ");

                            //sb.Append("<td>" + "<input type='text' value='" + item.Replace("\r", "") + "'>" + "</td>");
                            //sb_TableText.Append(item.Replace("\r", "") + " ");
                        }
                    }
                    sb.Append("</tr>");
                }

                if ((list_Tables.Count > 1) && (count < list_Tables.Count))
                {
                    sb.Append("<caption align='bottom' style='position:absolute'>" + caption + "</caption></tbody></table>" + "~//~");
                    sb_TableText.Append("~//~");
                }
                else
                {
                    sb.Append("<caption align='bottom' style='position:absolute'>" + caption + "</caption></tbody></table>");
                }

                //If there is a line before table
                if (table.PreviousSibling != null)
                {
                    SetTablePosition(Convert.ToString(page), Convert.ToDouble(table.PreviousSibling.LastChild.Attributes["height"].Value),
                        Convert.ToDouble(table.PreviousSibling.LastChild.Attributes["coord"].Value.Split(':')[3]), sb_TableText.ToString(),
                        table.PreviousSibling.LastChild.InnerText, "lineBefore", rowCount, table.Attributes["id"].Value);
                }
                //If there is a line after table
                else if (table.NextSibling != null)
                {
                    SetTablePosition(Convert.ToString(page), Convert.ToDouble(table.NextSibling.LastChild.Attributes["height"].Value),
                       Convert.ToDouble(table.NextSibling.FirstChild.Attributes["coord"].Value.Split(':')[3]), sb_TableText.ToString(),
                       table.NextSibling.FirstChild.InnerText, "lineAfter", rowCount, table.Attributes["id"].Value);
                }
                //If there is only a table on the page
                else
                {
                    SetTablePosition(Convert.ToString(page), Convert.ToDouble(table.NextSibling.LastChild.Attributes["height"].Value),
                      Convert.ToDouble(table.NextSibling.FirstChild.Attributes["coord"].Value.Split(':')[3]), sb_TableText.ToString(),
                      table.NextSibling.FirstChild.InnerText, "noLine", rowCount, table.Attributes["id"].Value);
                }
            }
        }

        hfTable.Value = sb_TableText.ToString();
        hfTableText.Value = sb.ToString();

        string text_Hf = hfTable.Value;
        string text_WithHtml_Hf = hfTableText.Value;

        return sb.ToString();
    }

    //protected int CountStringOccurrences(string text, string pattern)
    //{
    //    int count = 0;
    //    int i = 0;
    //    while ((i = text.IndexOf(pattern, i)) != -1)
    //    {
    //        i += pattern.Length;
    //        count++;
    //    }
    //    return count;
    //}

    protected void btnShowTablePopUp_Click(object sender, EventArgs e)
    {
        string tableText = GetTableText();

        if (tableText != null)
        {
            if (tableText.Length > 0)
            {
                divTableText.InnerHtml = tableText;
                divHtmlTable.Visible = true;
            }
        }
    }

    //[WebMethod]
    //public static string LogMistake(string text)
    //{
    //    string TextFromPDFJs = OriginalText.Value;  //The original text that was selected for editing

    //    return TextFromPDFJs;
    //}

    //added by aamir ghafoor for logging mistake
    protected void btnLogMistake_Click(object sender, EventArgs e)
    {
        string mainXmlPath = Convert.ToString(Session["MainXMLFilePath"]);

        if (String.IsNullOrEmpty(mainXmlPath))
            return;

        //StreamReader sr = new StreamReader(mainXmlPath); //Load the main xml from which pdf was produced
        //string xmlFile = sr.ReadToEnd();
        //sr.Close();
        //XmlDocument xmlDocOrigXml = new XmlDocument();
        //xmlDocOrigXml.LoadXml(xmlFile);  // Load the original xml which was used to produce PDF

        Common comObj = new Common();

        XmlDocument xmlDocOrigXml = comObj.LoadXmlDocument(mainXmlPath);

        if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
        {
            string completeRenderedPageXml = this.pageXML.Value;    //Get the complete rendered xml by PDF.js
            List<String> list_PrdPdfLines = GetTextFromPrdPdf(completeRenderedPageXml);
            string prdPdfPath = Common.GetTestFiles_SavingPath() + "/" + Convert.ToString(Session["Current_PrdPdfPage"]);
            List<String> list_SrcPdfLines = GetTextFromSrcPdf(prdPdfPath);

            Session["list_PrdPdfLines"] = list_PrdPdfLines;
            Session["list_SrcPdfLines"] = list_SrcPdfLines;
        }
        else if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
        {
            string completeRenderedPageXml = this.pageXML.Value;    //Get the complete rendered xml by PDF.js
            List<String> list_PrdPdfLines = GetTextFromPrdPdf(completeRenderedPageXml);
            string prdPdfPath = Common.GetOnePageTestFiles_SavingPath() + "/" + Convert.ToString(Session["Current_PrdPdfPage"]);
            List<String> list_SrcPdfLines = GetTextFromSrcPdf(prdPdfPath);

            Session["list_PrdPdfLines"] = list_PrdPdfLines;
            Session["list_SrcPdfLines"] = list_SrcPdfLines;
        }
        else if (Convert.ToString(Session["ComparisonTask"]).Equals("task"))
        {
            string completeRenderedPageXml = this.pageXML.Value;    //Get the complete rendered xml by PDF.js
            List<String> list_PrdPdfLines = GetTextFromPrdPdf(completeRenderedPageXml);
            string prdPdfPath = Common.GetTaskFiles_SavingPath() + Convert.ToString(Session["Current_PrdPdfPage"]);
            List<String> list_SrcPdfLines = GetTextFromSrcPdf(prdPdfPath);

            Session["list_PrdPdfLines"] = list_PrdPdfLines;
            Session["list_SrcPdfLines"] = list_SrcPdfLines;
        }


        List<string> selectedText = new List<string>();
        string TextFromPDFJs = this.OriginalText.Value;  //The original text that was selected for editing

        Session["OriginalText"] = TextFromPDFJs;

        if (TextFromPDFJs == "")
            return;

        var selectedLines = TextFromPDFJs.Split(new string[] { "\r\n" }, StringSplitOptions.None);

        selectedLines = selectedLines.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

        if ((selectedLines != null) && (selectedLines.Length > 0))
        {
            string attrName = "PDFmistake";
            string attrNameTest = "PDFmistakeTest";
            int mistakeNum = 0;
            bool check = false;
            string pageNum = "";

            foreach (var line in selectedLines)
            {
                check = false;

                XmlNodeList nodes = xmlDocOrigXml.SelectNodes("//ln");

                XmlElement root = xmlDocOrigXml.DocumentElement;

                foreach (XmlElement node in nodes)
                {
                    mistakeNum = 0;

                    if ((RemoveWhitespace(node.InnerText).Equals(RemoveWhitespace(line))) && (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                    {
                        pageNum = node.Attributes["page"].Value;

                        if ((Convert.ToString(Session["ComparisonTask"]).Equals("test")) || (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest")))
                        {
                            if (node.HasAttribute(attrName))
                            {
                                node.SetAttribute(attrName, "");
                            }

                            if (!node.HasAttribute(attrNameTest))
                            {
                                XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrNameTest);

                                newAttr.Value = Convert.ToString("1");
                                node.SetAttributeNode(newAttr);
                            }

                            check = false;
                        }

                        else if (!node.HasAttribute(attrName))
                        {
                            XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrName);

                            if (GetTotalMistakes() == 0)
                                mistakeNum = 1;
                            else
                                mistakeNum = GetTotalMistakes() + 1;

                            newAttr.Value = Convert.ToString(mistakeNum);
                            node.SetAttributeNode(newAttr);

                            if (node.Attributes["autoInjection"] != null)
                            {
                                node.RemoveAttribute("autoInjection");
                            }

                            if (node.Attributes["correction"] != null)
                            {
                                node.SetAttribute("correction", "");
                            }

                            if (node.Attributes["conversion"] != null)
                            {
                                node.SetAttribute("conversion", "");
                            }

                            if (node.Attributes["missing"] != null)
                            {
                                node.SetAttribute("missing", "");
                            }

                            check = true;
                        }

                        //update existing error comments
                        else
                        {
                            if (!Convert.ToString(Session["ComparisonTask"]).Equals("test"))
                            {
                                string updateId = obj.InsertQaMistakes(Convert.ToString(Session["TestName"]) + "-1", Convert.ToInt32(Session["userId"]), 1, Convert.ToInt32(node.Attributes["PDFmistake"].Value),
                                                    DateTime.Now, Convert.ToInt32(pageNum), tbxComments.Text.Trim());
                            }
                        }
                    }

                    if (check)
                    {
                        string Id = obj.InsertQaMistakes(Convert.ToString(Session["TestName"]) + "-1", Convert.ToInt32(Session["userId"]), 1, mistakeNum, DateTime.Now, Convert.ToInt32(pageNum),
                            tbxComments.Text.Trim());

                        XmlAttribute qaMistakeId = xmlDocOrigXml.CreateAttribute("QaMistakeId");
                        qaMistakeId.Value = Convert.ToString(Id);
                        node.SetAttributeNode(qaMistakeId);
                        break;
                    }
                }

                xmlDocOrigXml.Save(mainXmlPath);

                //if (!check)
                //{
                //    Literal ltr = new Literal();
                //    ltr.Text = @"<script type='text/javascript'> alert('" + "Mistake cannot be logged because more then one line is selected." + "') </script>";
                //    Page.Controls.Add(ltr);
                //    return;
                //}
            }
        }

        int totalMistakes = GetTotalMistakes();

        if (!Convert.ToString(Session["ComparisonTask"]).Equals("test"))
        {
            //Update mistake count
            MyDBClass db = new MyDBClass();
            db.UpdateMistakeCount(Convert.ToString(Session["TestName"]) + "-1", Convert.ToInt32(Session["userId"]), totalMistakes, 0);
        }

        Session["mistakeCounter"] = GetTotalMistakes();

        Response.Redirect("Comparison.aspx?mistake=1", true);
    }

    protected void btnNavigateNextPage_Click(object sender, EventArgs e)
    {
        NavigateNextPage();
    }

    protected void btnNavigatePrevPage_Click(object sender, EventArgs e)
    {
        NavigatePrevPage();
    }

    public void NavigateNextPage()
    {
        int totalPages = Convert.ToInt32(Session["srcTotalPages"]);

        int pNum = -1;
        int pageNum = 0;

        if (int.TryParse(((web_Comparison)this.Page).txtPageNum.Text.Trim(), out pNum))
        {
            pageNum = pNum;

            pageNum = pageNum < totalPages ? (pageNum + 1) : totalPages;

            Session["MainCurrPage"] = pageNum;

            ((web_Comparison)this.Page).LoadNewPageInControl(pageNum);

            PDFManipulation pdfMan = new PDFManipulation(Convert.ToString(Session["srcPdfPagePath"]));

            string mistakeNum = pdfMan.GetMistakeByPage(pageNum);

            if (mistakeNum != null)
                ((web_Comparison)this.Page).TextErrorNum.Text = mistakeNum;
        }
        else
        {
            //Invalid
        }
    }

    public void NavigatePrevPage()
    {
        int totalPages = Convert.ToInt32(Session["srcTotalPages"]);

        int pNum = -1;
        int pageNum = 0;

        if (int.TryParse(((web_Comparison)this.Page).txtPageNum.Text.Trim(), out pNum))
        {
            pageNum = pNum;

            pageNum = pageNum > 1 ? (pageNum - 1) : 1;

            Session["MainCurrPage"] = pageNum;
            ((web_Comparison)this.Page).LoadNewPageInControl(pageNum);

            PDFManipulation pdfMan = new PDFManipulation(Convert.ToString(Session["srcPdfPagePath"]));

            string mistakeNum = pdfMan.GetMistakeByPage(pageNum);

            if (mistakeNum != null)
                ((web_Comparison)this.Page).TextErrorNum.Text = mistakeNum;
        }
        else
        {
            //Invalid
        }
    }

    protected void btnClose_Click(object sender, System.EventArgs e)
    {
        divHtmlTable.Visible = false;
        divTableText.Visible = false;
    }

    [WebMethod]
    public static string GetPassedTests(string name)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        string qaMistakeId = "";

        List<Mistakes> list = new List<Mistakes>();
        StreamReader strreader = new StreamReader(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        XmlNodeList nodes = xmlDoc.SelectNodes(@"//*[@PDFmistake]");

        int i = 1;

        if (nodes.Count > 0)
        {
            foreach (XmlElement node in nodes)
            {
                list.Add(new Mistakes
                {
                    mistakeNum = i,
                    page = Convert.ToInt32(node.Attributes["page"].Value),
                    comments = Convert.ToString(node.Attributes["Comments"].Value),
                    mistakeId = Convert.ToString(node.Attributes["QaMistakeId"].Value)
                });

                i++;
            }
        }

        var list_MIstakes = list;

        string strQuery = "GetQaMistakeComments";
        SqlCommand cmd = new SqlCommand(strQuery, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Id", qaMistakeId.Trim());

        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        string Comments = "";

        using (con)
        {
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    Comments = Convert.ToString(dr["Comments"]);
                }
            }
        }

        return Comments.Trim();
    }


    [WebMethod]
    public static string GetError_Comments(string text)
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString);
        string qaMistakeId = "";


        //List<Mistakes> list = new List<Mistakes>();
        //StreamReader strreader = new StreamReader(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));
        //string xmlInnerText = strreader.ReadToEnd();
        //strreader.Close();

        //XmlDocument xmlDoc = new XmlDocument();
        //xmlDoc.LoadXml(xmlInnerText);

        //XmlNodeList nodes = xmlDoc.SelectNodes(@"//*[@PDFmistake]");

        //int i = 1;

        //if (nodes.Count > 0)
        //{
        //    foreach (XmlElement node in nodes)
        //    {
        //        list.Add(new Mistakes
        //        {
        //            mistakeNum = i,
        //            page = Convert.ToInt32(node.Attributes["page"].Value),
        //            comments = Convert.ToString(node.Attributes["Comments"].Value),
        //            mistakeId = Convert.ToString(node.Attributes["QaMistakeId"].Value)
        //        });

        //        i++;
        //    }
        //}

        //var list_MIstakes = list;

        //string strQuery = "GetQaMistakeComments";
        //SqlCommand cmd = new SqlCommand(strQuery, con);
        //cmd.CommandType = CommandType.StoredProcedure;
        //cmd.Parameters.AddWithValue("@Id", qaMistakeId.Trim());

        //con.Open();
        //SqlDataReader dr = cmd.ExecuteReader();
        string Comments = "";

        //using (con)
        //{
        //    if (dr.HasRows)
        //    {
        //        while (dr.Read())
        //        {
        //            Comments = Convert.ToString(dr["Comments"]);
        //        }
        //    }
        //}

        return Comments.Trim();
    }


    //commented by aamir ghafoor for code refactoring 2014-11-27
    //private string GenearatePDFPreview(string srcXMLFile, string targetPDFPath)
    //{
    //    return ShowPdfPreview(srcXMLFile, targetPDFPath, ConfigurationManager.AppSettings["XEPPath"], ConfigurationManager.AppSettings["XSLPath"]);
    //}

    //private string ShowPdfPreview(string xmlfile, string PdfFile, string xepfile, string xslfile)
    //{
    //    string retMessage = "";
    //    try
    //    {
    //        string cmdStr = "-xml " + "\"" + xmlfile + "\"" + " -xsl " + "\"" + xslfile + "\"" + " -out " + "\"" + PdfFile + "\"";
    //        if (File.Exists(PdfFile))
    //        {
    //            File.Delete(PdfFile);
    //        }
    //        Process pPdfPreview = new Process();
    //        Process pPdfPreviewInPDF = new Process();

    //        //tells operating system not to use a shell;
    //        pPdfPreview.StartInfo.UseShellExecute = false;
    //        //allow me to capture stdout, i.e. results
    //        pPdfPreview.StartInfo.RedirectStandardOutput = true;
    //        //#my command arguments, i.e. what site to ping
    //        pPdfPreview.StartInfo.Arguments = cmdStr;
    //        //#the command to invoke under MSDOS
    //        pPdfPreview.StartInfo.FileName = xepfile;
    //        //#do not show MSDOS window
    //        pPdfPreview.StartInfo.CreateNoWindow = true;
    //        //#do it!
    //        bool bStarted = pPdfPreview.Start();
    //        while (!pPdfPreview.HasExited)
    //        {
    //            //Application.DoEvents();
    //            System.Diagnostics.Debug.Write(".");
    //        }
    //        pPdfPreview.WaitForExit();
    //        // Check if PDF size is greater than zero
    //        // IF-ELSE Block Added
    //        FileInfo PdfFileInfo = new FileInfo(PdfFile);
    //        if (File.Exists(PdfFile) && PdfFileInfo.Length > 0)
    //        {
    //            retMessage = "Successfull";
    //        }
    //        else
    //        {
    //            retMessage = "No PDF File found";
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        retMessage = ex.Message;
    //    }
    //    return retMessage;
    //}

    #region Unused Comparison Related Code

    //protected void btnMerge_Click(object sender, EventArgs e)
    //{
    //    StreamReader sr = new StreamReader(XMLFile); //Load the xml from which pdf was produced
    //    string xmlFile = sr.ReadToEnd();
    //    sr.Close();
    //    XmlDocument xmlDocOrigXml = new XmlDocument();
    //    xmlDocOrigXml.LoadXml(xmlFile);  // Load the original xml which was used to produce PDF
    //    XmlNodeList uparaNodes = xmlDocOrigXml.SelectNodes("//upara");
    //    if (uparaNodes.Count > 0)
    //    {
    //        int i = 1;
    //        foreach (XmlNode item in uparaNodes)
    //        {
    //            item.Attributes["id"].Value = i.ToString();
    //            i = i + 1;
    //        }
    //        xmlDocOrigXml.Save(XMLFile);
    //    }
    //    XmlDocument xmlPDFJSDoc = new XmlDocument();

    //    string completeRenderedPageXml = this.pageXML.Value;    //Get the complete rendered xml by PDF.js
    //    string originalText = this.OriginalText.Value;  //The original text that was selected for editing


    //    string modifiedText = this.ModifiedText.Value;  // The new edited text 
    //    string selParentsXml = this.SelectedXMLParents.Value;

    //    string[] lines = Regex.Split(modifiedText, "\r\n");
    //    List<XmlNode> MatchedNode = new List<XmlNode>();


    //    foreach (string line in lines)
    //    {
    //        XmlNode lnNode = getExactMatchingLnNode(xmlDocOrigXml, line);
    //        MatchedNode.Add(lnNode);
    //    }
    //    if (MatchedNode.Count > 0)
    //    {
    //        XmlNode MainNode = MatchedNode[0].ParentNode.Clone();
    //        MainNode.RemoveAll();
    //        foreach (XmlAttribute attr in MatchedNode[0].ParentNode.Attributes)
    //        {
    //            ((XmlElement)MainNode).SetAttribute(attr.Name, attr.Value);
    //        }
    //        foreach (XmlNode node in MatchedNode)
    //        {
    //            foreach (XmlNode subNode in node.ParentNode.ChildNodes)
    //            {
    //                MainNode.AppendChild(subNode.Clone());
    //            }
    //        }

    //        XmlNode MainBodyNode = xmlDocOrigXml.SelectSingleNode("//body");
    //        for (int k = 0; k < MatchedNode.Count; k++)
    //        {
    //            //if (k == 0)
    //            //{
    //            XmlNode parentNode = MatchedNode[0].ParentNode;
    //            int ParaID = Convert.ToInt32(parentNode.Attributes["id"].Value);
    //            bool result = false;
    //            foreach (XmlNode item in MatchedNode)
    //            {
    //                if (item.ParentNode != null && (Convert.ToInt32(item.ParentNode.Attributes["id"].Value)) != ParaID)
    //                {
    //                    result = true;
    //                }

    //            }
    //            if (result == true && (MatchedNode[k + 1].ParentNode != null))
    //            {
    //                parentNode.AppendChild(MatchedNode[k + 1].Clone());
    //                MatchedNode[k + 1].ParentNode.RemoveChild(MatchedNode[k + 1]);
    //            }

    //            //if (result == false)
    //            //{
    //            //    Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", string.Format("<script type='text/javascript'>alert('Cannot Merge Lines of Same Paragraph.')</script>"));
    //            //}

    //        }
    //        xmlDocOrigXml.Save(XMLFile);
    //    }

    //    string dummy = XMLFile;

    //    string pdfPath = dummy.Replace(".xml", ".pdf"); //"_" + rndVal + ".pdf";
    //    //string pdfProducedFullPath = PDFDirPath + "\\" + pdfPath;
    //    //GenearatePDFPreview(XMLFile, pdfPath); //commented by aamir ghafoor for code refactoring 2014-11-27

    //    RHYWManipulation rhywObj = new RHYWManipulation(XMLFile);
    //    rhywObj.GenearatePDFPreview(XMLFile, pdfPath);
    //    String virDirPath = ConfigurationManager.AppSettings["PDFDirVirPath"];

    //    //this.FileLoadPath.Value = "PDFs\\_" + rndVal + ".pdf";

    //    //this.FileLoadPath.Value = virDirPath + "\\" + pdfPath;
    //    PDFFile = pdfPath;


    //    FileEdit(this, new CommandEventArgs("EditedFilePath", this.FileLoadPath.Value));


    //}

    //protected void btnSplit_Click(object sender, EventArgs e)
    //{
    //    StreamReader sr = new StreamReader(XMLFile); //Load the xml from which pdf was produced
    //    string xmlFile = sr.ReadToEnd();
    //    sr.Close();
    //    XmlDocument xmlDocOrigXml = new XmlDocument();
    //    xmlDocOrigXml.LoadXml(xmlFile);  // Load the original xml which was used to produce PDF
    //    XmlDocument xmlPDFJSDoc = new XmlDocument();

    //    string completeRenderedPageXml = this.pageXML.Value;    //Get the complete rendered xml by PDF.js
    //    string originalText = this.OriginalText.Value;  //The original text that was selected for editing
    //    completeRenderedPageXml = completeRenderedPageXml.Replace("&nbsp;", " ");     //Clean xml for comparison - remove spaces
    //    xmlPDFJSDoc.LoadXml("<body>" + completeRenderedPageXml + "</body>");

    //    XmlElement matchedParentsNode = xmlPDFJSDoc.CreateElement("parent");

    //    string modifiedText = this.ModifiedText.Value;  // The new edited text 
    //    string selParentsXml = this.SelectedXMLParents.Value;

    //    matchedParentsNode.InnerXml = selParentsXml;

    //    ArrayList arrLines = GetCompleteLinesWithMatchingAttrValue(matchedParentsNode, "", "", 0.0f);
    //    string topAttrValue = Regex.Match(selParentsXml, "top:.*?px", RegexOptions.Singleline).Value; //Get the top attribute value
    //    string clientCompleteLine = GetAllNodesWithMatchingAttrValue(xmlPDFJSDoc, "style", topAttrValue, 0.0f); // Get complete line by matching the top attribute

    //    if (arrLines.Count == 1)
    //    {

    //        XmlNode lnNode = getExactMatchingLnNode(xmlDocOrigXml, clientCompleteLine); ;

    //        XmlNode lnParentNode = lnNode.SelectSingleNode("ancestor::upara|ancestor::spara|ancestor::npara");
    //        if (lnParentNode == null)
    //            return;
    //        XmlNode newNodeToInsert = lnParentNode.Clone();

    //        XmlNodeList newChildNodes = lnNode.SelectNodes(". | following-sibling::*");
    //        if (newChildNodes.Count > 0)
    //        {
    //            if (newNodeToInsert.Attributes["text-indent"] != null)
    //            {
    //                newNodeToInsert.Attributes["text-indent"].Value = newChildNodes[0].Attributes["coord"].Value.ToString().Split(new char[] { ':' })[0];
    //            }
    //        }
    //        if (lnParentNode.Name == "spara")
    //        {
    //            XmlNode sparaChild = newNodeToInsert.FirstChild;
    //            sparaChild.RemoveAll();

    //            for (int i = 0; i < newChildNodes.Count; i++)
    //            {
    //                sparaChild.AppendChild(newChildNodes[i]);
    //            }
    //        }
    //        else
    //        {
    //            newNodeToInsert.RemoveAll();
    //            ///Select the current and the following sibling nodes
    //            for (int i = 0; i < newChildNodes.Count; i++)
    //            {
    //                newNodeToInsert.AppendChild(newChildNodes[i]);
    //            }
    //        }
    //        foreach (XmlAttribute attr in lnParentNode.Attributes)
    //        {
    //            ((XmlElement)newNodeToInsert).SetAttribute(attr.Name, attr.Value);
    //        }

    //        lnParentNode.ParentNode.InsertAfter(newNodeToInsert, lnParentNode);
    //        xmlDocOrigXml.Save(XMLFile);

    //        string dummy = XMLFile;

    //        string pdfPath = dummy.Replace(".xml", ".pdf"); //"_" + rndVal + ".pdf";
    //        //string pdfProducedFullPath = PDFDirPath + "\\" + pdfPath;
    //        //GenearatePDFPreview(XMLFile, pdfPath);

    //        RHYWManipulation rhywObj = new RHYWManipulation(XMLFile);
    //        rhywObj.GenearatePDFPreview(XMLFile, pdfPath);

    //        String virDirPath = ConfigurationManager.AppSettings["PDFDirVirPath"];

    //        //this.FileLoadPath.Value = "PDFs\\_" + rndVal + ".pdf";

    //        //this.FileLoadPath.Value = virDirPath + "\\" + pdfPath;
    //        PDFFile = pdfPath;


    //        FileEdit(this, new CommandEventArgs("EditedFilePath", this.FileLoadPath.Value));
    //    }

    //}
    //protected void btnGetSelText_Click(object sender, EventArgs e)
    //{
    //    string Comments_Mod = this.CommentsEnterd.Value;

    //    SiteSession.PDFViewerComments = Comments_Mod;

    //    StreamReader sr = new StreamReader(XMLFile); //Load the xml from which pdf was produced
    //    string xmlFile = sr.ReadToEnd();
    //    sr.Close();
    //    XmlDocument xmlDocOrigXml = new XmlDocument();
    //    xmlDocOrigXml.LoadXml(xmlFile);  // Load the original xml which was used to produce PDF

    //    XmlDocument xmlPDFJSDoc = new XmlDocument();

    //    string completeRenderedPageXml = this.pageXML.Value;    //Get the complete rendered xml by PDF.js
    //    string originalText = this.OriginalText.Value;  //The original text that was selected for editing
    //    completeRenderedPageXml = completeRenderedPageXml.Replace("&nbsp;", " ");     //Clean xml for comparison - remove spaces
    //    xmlPDFJSDoc.LoadXml("<body>" + completeRenderedPageXml + "</body>");

    //    XmlElement matchedParentsNode = xmlPDFJSDoc.CreateElement("parent");

    //    string modifiedText = this.ModifiedText.Value;  // The new edited text 
    //    string selParentsXml = this.SelectedXMLParents.Value;

    //    matchedParentsNode.InnerXml = selParentsXml;

    //    //New
    //    ArrayList arrLines = GetCompleteLinesWithMatchingAttrValue(matchedParentsNode, "", "", 0.0f);
    //    string[] modifiedLinesText = modifiedText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
    //    string[] originalLinesText = originalText.Split(new string[] { "\r\n" }, StringSplitOptions.None);
    //    int modifiedTextLinesLen = modifiedLinesText.Length;
    //    if (modifiedTextLinesLen == 1)  // If only single line was modified
    //    {
    //        string topAttrValue = Regex.Match(selParentsXml, "top:.*?px", RegexOptions.Singleline).Value; //Get the top attribute value
    //        string clientCompleteLine = GetAllNodesWithMatchingAttrValue(xmlPDFJSDoc, "style", topAttrValue, 0.0f); // Get complete line by matching the top attribute
    //        originalText = Regex.Replace(originalText, "\\s", " ", RegexOptions.Singleline); //Replaces 0x00a0 with space character
    //        string clientModifiedLine = clientCompleteLine.Replace(originalText, modifiedText); // Get the modified text

    //        ArrayList error_List = SiteSession.errorHl.CurrentErrors;
    //        foreach (MisMatchError item in error_List)
    //        {
    //            if (originalText.Contains(item.list1Word.Text))
    //            {
    //                if (modifiedText.Contains(item.list1Word.Text))
    //                {
    //                    SiteSession.ModifiedWord = item.list1Word.Text;
    //                }
    //            }

    //        }
    //        string selParentsXmlForMatching = selParentsXml.Replace(" ", "[ ]");

    //        if (clientModifiedLine == "")//Could not find matching line in orignial xmlDoc
    //        {
    //            if (Comments_Mod.Trim() == "")
    //            {
    //                Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", string.Format("<script type='text/javascript'>alert('Cannot update line, Please Add Comment!')</script>"));
    //                return;
    //            }
    //            else
    //                Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", string.Format("<script type='text/javascript'>alert('Cannot update line, However Comment Added!')</script>"));
    //        }

    //        XmlNode origTextLnNode = getExactMatchingLnNode(xmlDocOrigXml, clientCompleteLine);  //Get the original ln Node from the xmlDoc
    //        if (origTextLnNode == null) //i.e. if corresponding ln Node not found in rhyw page document
    //        {
    //            ArrayList arrAllContainingLnNodes = getContainingLnNodes(xmlDocOrigXml, clientCompleteLine);
    //            if (arrAllContainingLnNodes.Count == 1)
    //            {
    //                origTextLnNode = ((XmlNode)arrAllContainingLnNodes[0]);
    //                String toReplace = Regex.Replace(origTextLnNode.InnerText, clientCompleteLine, clientModifiedLine);
    //                origTextLnNode.InnerText = toReplace;
    //            }
    //        }
    //        else // Corresponding ln Node found
    //        {
    //            origTextLnNode.InnerText = clientModifiedLine;
    //        }
    //    }
    //    else if (modifiedTextLinesLen == arrLines.Count)
    //    {
    //        for (int i = 0; i < arrLines.Count; i++)
    //        {
    //            string clientCompleteLine = arrLines[i].ToString();
    //            //originalLinesText has the same length as of arrLines, because arrLines contain complete lines obtained from originalText itself
    //            originalText = Regex.Replace(originalLinesText[i], "\\s", " ", RegexOptions.Singleline); //Replaces 0x00a0 with space character
    //            string clientModifiedLine = clientCompleteLine.Replace(originalText, modifiedLinesText[i]); // Get the modified text

    //            string selParentsXmlForMatching = selParentsXml.Replace(" ", "[ ]");

    //            XmlNode origTextLnNode = getExactMatchingLnNode(xmlDocOrigXml, clientCompleteLine);  //Get the original ln Node from the xmlDoc
    //            if (origTextLnNode == null)//Could not find matching line in orignial xmlDoc
    //            {
    //                if (Comments_Mod.Trim() == "")
    //                {
    //                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", string.Format("<script type='text/javascript'>alert('Cannot update line, Please Add Comment!')</script>"));
    //                    return;
    //                }
    //                else
    //                    Page.ClientScript.RegisterStartupScript(this.GetType(), "ShowMessage", string.Format("<script type='text/javascript'>alert('Cannot update line, However Comment Added!')</script>"));
    //            }
    //            else
    //            {
    //                origTextLnNode.InnerText = clientModifiedLine;
    //            }
    //        }
    //    }
    //    else if (modifiedTextLinesLen < arrLines.Count)
    //    {

    //    }
    //    else //(modifiedTextLinesLen > arrLines.Count)
    //    {

    //    }


    //    xmlDocOrigXml.Save(XMLFile);


    //    string dummy = XMLFile;

    //    string pdfPath = dummy.Replace(".xml", ".pdf"); //"_" + rndVal + ".pdf";
    //    //string pdfProducedFullPath = PDFDirPath + "\\" + pdfPath;
    //    //GenearatePDFPreview(XMLFile, pdfPath);

    //    RHYWManipulation rhywObj = new RHYWManipulation(XMLFile);
    //    rhywObj.GenearatePDFPreview(XMLFile, pdfPath);
    //    String virDirPath = ConfigurationManager.AppSettings["PDFDirVirPath"];

    //    //this.FileLoadPath.Value = "PDFs\\_" + rndVal + ".pdf";

    //    //this.FileLoadPath.Value = virDirPath + "\\" + pdfPath;
    //    PDFFile = pdfPath;


    //    FileEdit(this, new CommandEventArgs("EditedFilePath", this.FileLoadPath.Value));
    //}

    /// <summary>
    /// Gets the innerText of all nodes conatining the Matching attribute value, caters an offset as well, for e.g if attributeName
    /// is style and top: 123px is passed, it will try to match top: 123px anywhere in the style attribute and get all such nodes'
    /// innertext appended
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <param name="attributeName"></param>
    /// <param name="attributeValue"></param>
    /// <param name="offset">The offset value by which to compare the value, 1.0 would bring nodes with value 1.0 and 2.0\r\nDefault: 0.0</param>
    /// <returns></returns>
    private string GetAllNodesWithMatchingAttrValue(XmlDocument xmlDoc, string attributeName, string attributeValue, float offset)
    {
        XmlNodeList xmlSameNodes = xmlDoc.SelectNodes("//*[contains(@" + attributeName + ", \"" + attributeValue + "\")]"); //*[contains(@coord, ".12")] //*[@left="91.53"]
        StringBuilder sb = new StringBuilder();
        for (int i = 0; i < xmlSameNodes.Count; i++)
        {
            sb.Append(xmlSameNodes[i].InnerText);
        }
        return sb.ToString();
    }

    private ArrayList GetCompleteLinesWithMatchingAttrValue(XmlNode xmlNode, string attributeName, string attributeValue, float offset)
    {
        ArrayList allLines = new ArrayList();
        ///PartialSelLInes is the partial lines that are selected, the user may have not selected the complete lines.
        string[] partialSelLines = xmlNode.InnerXml.Split(new string[] { "&lt;br/&gt;" }, StringSplitOptions.RemoveEmptyEntries);
        foreach (string singlePartialLine in partialSelLines)
        {
            string currLineTopAttr = Regex.Match(singlePartialLine, "top:.*?px", RegexOptions.Singleline).Value;
            string completeSingleLine = GetAllNodesWithMatchingAttrValue(xmlNode.OwnerDocument, "style", currLineTopAttr, 0.0f);
            allLines.Add(completeSingleLine);
        }
        return allLines;
    }

    /// <summary>
    /// Gets the Ln Node which contains the exact text as strLnString, this way it reduces the possiblity of strLnString occuring
    /// in any other Ln Node
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <param name="strLnString"></param>
    /// <returns></returns>
    private XmlNode getExactMatchingLnNode(XmlDocument xmlDoc, string strLnString)
    {
        string cleanedLnStringToMatch = CleanString(strLnString);
        XmlNodeList lnNodes = xmlDoc.SelectNodes("//ln");
        foreach (XmlNode ln in lnNodes)
        {
            string cleanLn = CleanString(ln.InnerText);
            if (cleanedLnStringToMatch.Equals(cleanLn))
            {
                return ln;
            }
        }
        return null;
    }

    /// <summary>
    /// Gets All Ln Nodes containing strLnString, Does Not match completly, runs a partial match on each Node. Preferably called after
    /// getExactMatchingLnNode method
    /// </summary>
    /// <param name="xmlDoc"></param>
    /// <param name="strLnString"></param>
    /// <returns></returns>
    private ArrayList getContainingLnNodes(XmlDocument xmlDoc, string strLnString)
    {
        ArrayList arrMatchingLnNodes = new ArrayList();
        string cleanedLnStringToMatch = CleanString(strLnString);
        XmlNodeList lnNodes = xmlDoc.SelectNodes("//ln");
        foreach (XmlNode ln in lnNodes)
        {
            string cleanLn = CleanString(ln.InnerText);
            if (Regex.IsMatch(cleanLn, cleanedLnStringToMatch))
            {
                arrMatchingLnNodes.Add(ln);
            }
        }
        return arrMatchingLnNodes;
    }


    public ArrayList GetMatchedLines(XmlDocument xmlDoc, string strToModify)
    {
        ArrayList arrMatchedLines = new ArrayList();
        XmlNodeList lnNodes = xmlDoc.SelectNodes("//ln");
        foreach (XmlNode lnNode in lnNodes)
        {
            string lnAsString = Regex.Match(lnNode.InnerText, "[a-zA-Z]+", RegexOptions.Singleline).ToString();
            if (lnNode.InnerText.Contains(strToModify))
            {
                string lnNodeCleanedText = lnNode.InnerText;
                arrMatchedLines.Add(lnNode);
            }
        }
        return arrMatchedLines;
    }

    /// <summary>
    /// Cleans the string from numeric and special characters
    /// </summary>
    /// <param name="strToClean"></param>
    /// <returns></returns>
    private string CleanString(string str)
    {
        StringBuilder sb = new StringBuilder(str.Length);
        foreach (char c in str)
        {
            if ((c >= '0' && c <= '9') || (c >= 'A' && c <= 'Z') || (c >= 'a' && c <= 'z'))
            {
                sb.Append(c);
            }
        }
        return sb.ToString();
    }

    #endregion
}


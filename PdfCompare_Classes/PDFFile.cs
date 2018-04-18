using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.IO;
using System.Diagnostics;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Outsourcing_System;

public class PDFFile : ICompFile
{
    #region ICompFile Members

    string filePath;
    public string FilePath
    {
        get
        {
            return filePath;
        }
        set
        {
            this.filePath = value;
        }
    }
    int totalPages;
    public int TotalPages
    {
        get
        {
            return totalPages;
        }
    }
    int wrdCount;
    public int WordCount
    {
        get
        {
            return wrdCount;
        }
    }
    ArrayList wrdList;
    PDFTextExtract pdfOB;
    public ArrayList WordsArrayList
    {
        get
        {
            return this.wrdList;
        }
    }

    public ArrayList GenerateAndGetAllWordsInFile()
    {
        //PDFTextExtract pdfTE = new PDFTextExtract(this.filePath);
        //GetWordObjListFromTet();
        GetWordObjListFromTetWithCord();
        return this.wrdList;
    }

    ArrayList strWrdList;
    public ArrayList GetStrWordList()
    {
        //PDFTextExtract pdfTE = new PDFTextExtract(this.filePath);
        GetWordObjListFromTet();
        return this.wrdList;
    }

    PDFFile()
    {
        this.strWrdList = new ArrayList();
        this.wrdList = new ArrayList();
    }

    public string GetPageLines(int PageNumber, string linedelimeterString)
    {
        XmlDocument tetLineDoc = new XmlDocument();
        //tetLineDoc.LoadXml(this.lineTETMLPath);
        try
        {
            StreamReader sr = new StreamReader(this.lineTETMLPath);
            string xmlText = sr.ReadToEnd();
            sr.Close();
            string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
            tetLineDoc.LoadXml(documentXML);
            //tetDoc.Load(tetFilePath);
        }
        catch { }
        XmlNode page = tetLineDoc.SelectSingleNode("//Page[@number=\"" + PageNumber + "\"]");

        XmlNodeList lines = page.SelectNodes("descendant::Line");
        StringBuilder sblines = new StringBuilder();
        foreach (XmlNode line in lines)
        {
            string lineText = line.InnerText;
            sblines.Append(lineText + linedelimeterString);
        }
        return sblines.ToString();
    }

    /// <summary>
    /// Gets word list wihtout cordinats
    /// </summary>
    public void GetWordObjListFromTet()
    {
        ArrayList arrWrdList = new ArrayList();
        //PDFTextExtract pdfTE = new PDFTextExtract(this.filePath);
        //string tetFilePath = pdfTE.CreateteTmlWithLineOption();
        string tetFilePath = CreateteTmlWithLineOption();
        XmlDocument tetDoc = new XmlDocument();
        try
        {
            StreamReader sr = new StreamReader(tetFilePath);
            string xmlText = sr.ReadToEnd();
            sr.Close();
            string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
            tetDoc.LoadXml(documentXML);
            //tetDoc.Load(tetFilePath);
        }
        catch { }
        XmlNodeList pages = tetDoc.SelectNodes("//Page");


        foreach (XmlNode page in pages)
        {
            int currentPageNum = int.Parse(page.Attributes["number"].Value);
            XmlNodeList lines = page.SelectNodes("descendant::Line");
            int currLint = 0;
            foreach (XmlNode line in lines)
            {
                string lineText = line.InnerText;
                string[] words = lineText.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    string cword = word.Replace("•", "");
                    if (cword.Trim() != string.Empty)
                    {
                        PdfWord wrd = new PdfWord(currentPageNum, currLint, cword, "");
                        this.strWrdList.Add(cword);
                        arrWrdList.Add(wrd);
                    }
                }
                currLint++;
            }
        }
        this.wrdList = arrWrdList;
    }

    public void GetWordObjListFromTetWithCord()
    {
        ArrayList arrWrdList = new ArrayList();
        //PDFTextExtract pdfTE = new PDFTextExtract(this.filePath);
        //string tetFilePath = pdfTE.CreateteTmlWithLineOption();
        string tetFilePath = Createtetml();

        if (tetFilePath == null)
            return;

        XmlDocument tetDoc = new XmlDocument();
        try
        {
            StreamReader sr = new StreamReader(tetFilePath);
            string xmlText = sr.ReadToEnd();
            sr.Close();
            string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
            tetDoc.LoadXml(documentXML);
            //tetDoc.Load(tetFilePath);
        }
        catch { }
        XmlNodeList words = tetDoc.SelectNodes("//Word");
        foreach (XmlNode word in words)
        {
            //int currentPageNum = int.Parse(page.Attributes["number"].Value);
            int currentPageNum = int.Parse(word.SelectSingleNode("ancestor::Page").Attributes["number"].Value);
            //XmlNodeList lines = page.SelectNodes("descendant::Line");
            int currLint = 0;
            //foreach (XmlNode line in lines)
            string wordText = word.SelectSingleNode("Text").InnerText;
            string cword = wordText.Replace("•", "").Trim();
            {
                //string lineText = line.InnerText;
                string box = word.SelectSingleNode("descendant::Box").OuterXml;
                string llx = Regex.Match(box, "(?<=llx=\")[^\"]*(?=\")").Value;
                string lly = Regex.Match(box, "(?<=lly=\")[^\"]*(?=\")").Value;
                string urx = Regex.Match(box, "(?<=urx=\")[^\"]*(?=\")").Value;
                string ury = Regex.Match(box, "(?<=ury=\")[^\"]*(?=\")").Value;
                string coordinates = llx + ":" + lly + ":" + urx + ":" + ury;
                PdfWord wrd = new PdfWord(currentPageNum, -1, cword, "");
                wrd.StrCordinates = coordinates;
                this.strWrdList.Add(cword);
                arrWrdList.Add(wrd);
            }
        }
        this.wrdList = arrWrdList;
    }

    //public void GetWordObjListFromTetWithCord()
    //{
    //    ArrayList arrWrdList = new ArrayList();
    //    //PDFTextExtract pdfTE = new PDFTextExtract(this.filePath);
    //    //string tetFilePath = pdfTE.CreateteTmlWithLineOption();
    //    string tetFilePath = Createtetml();
    //    XmlDocument tetDoc = new XmlDocument();
    //    try
    //    {
    //        StreamReader sr = new StreamReader(tetFilePath);
    //        string xmlText = sr.ReadToEnd();
    //        sr.Close();
    //        string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
    //        tetDoc.LoadXml(documentXML);
    //        //tetDoc.Load(tetFilePath);
    //    }
    //    catch { }
    //    XmlNodeList words = tetDoc.SelectNodes("//Word");
    //    foreach (XmlNode word in words)
    //    {
    //        //int currentPageNum = int.Parse(page.Attributes["number"].Value);
    //        int currentPageNum = int.Parse(word.SelectSingleNode("ancestor::Page").Attributes["number"].Value);
    //        //XmlNodeList lines = page.SelectNodes("descendant::Line");
    //        int currLint = 0;
    //        //foreach (XmlNode line in lines)
    //        string wordText = word.SelectSingleNode("Text").InnerText;
    //        string cword = wordText.Replace("•", "").Trim();
    //        {
    //            //string lineText = line.InnerText;
    //            string box = word.SelectSingleNode("descendant::Box").OuterXml;
    //            string llx = Regex.Match(box, "(?<=llx=\")[^\"]*(?=\")").Value;
    //            string lly = Regex.Match(box, "(?<=lly=\")[^\"]*(?=\")").Value;
    //            string urx = Regex.Match(box, "(?<=urx=\")[^\"]*(?=\")").Value;
    //            string ury = Regex.Match(box, "(?<=ury=\")[^\"]*(?=\")").Value;
    //            string coordinates = llx + ":" + lly + ":" + urx + ":" + ury;
    //            Word wrd = new Word(currentPageNum, -1, cword);
    //            wrd.StrCordinates = coordinates;
    //            this.strWrdList.Add(cword);
    //            arrWrdList.Add(wrd);
    //        }
    //    }
    //    this.wrdList = arrWrdList;
    //}

    public PDFFile(string pdfFilePath)
    {
        if (pdfFilePath != "" && File.Exists(pdfFilePath))
        {
            this.strWrdList = new ArrayList();
            this.filePath = pdfFilePath;
            //pdfOB = new PDFTextExtract(pdfFilePath);

            //this.totalPages = pdfOB.GetWordCountFromTet();
            //this.wrdCount = this.PDFPageCount;
        }
    }
    #endregion

    private string wordTETMLPath;
    private string lineTETMLPath;
    public string Createtetml()
    {
        if (this.filePath == null)
            return null;

        //WriteLog("Generating tetml File............ Please Wait");
        //WriteLog("This Will Take Time Depending upon PDF Pages");
        string DirectoryPath = Directory.GetParent(this.filePath).ToString();
        this.wordTETMLPath = DirectoryPath + "\\" + Path.GetFileNameWithoutExtension(this.filePath) + ".tetml";
        //tetFile = XmlFile;
        //string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks}\" -o \"" + XmlFile + "\" \"" + PDFFilePath + "\"";
        string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks} clippingarea={cropbox}\" -o \"" + this.wordTETMLPath + "\" \"" + this.filePath + "\"";
        //string Img_Conversion_bat = @"D:\work\tet.exe";
        string Img_Conversion_bat = System.Configuration.ConfigurationSettings.AppSettings["TetPath"].ToString();
        Process pConvertTetml = new Process();
        pConvertTetml.StartInfo.UseShellExecute = false;
        pConvertTetml.StartInfo.RedirectStandardError = true;
        pConvertTetml.StartInfo.RedirectStandardOutput = true;
        pConvertTetml.StartInfo.CreateNoWindow = true;
        pConvertTetml.StartInfo.Arguments = strParameter;
        pConvertTetml.StartInfo.FileName = Img_Conversion_bat;
        pConvertTetml.Start();
        pConvertTetml.WaitForExit();
        return wordTETMLPath;
        //SetXmlFilePath(XmlFile);
    }

    public string CreateteTmlWithLineOption()
    {
        //WriteLog("Generating tetml File............ Please Wait");
        //WriteLog("This Will Take Time Depending upon PDF Pages");
        string DirectoryPath = Directory.GetParent(this.filePath).ToString();
        this.lineTETMLPath = DirectoryPath + "//" + Path.GetFileNameWithoutExtension(this.filePath) + ".tetml";
        //tetFile = XmlFile;
        //string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks}\" -o \"" + XmlFile + "\" \"" + PDFFilePath + "\"";
        string strParameter = "-m line --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks} clippingarea={cropbox}\" -o \"" + this.lineTETMLPath + "\" \"" + this.filePath + "\"";
        //string Img_Conversion_bat = @"D:\work\tet.exe";
        string Img_Conversion_bat = System.Configuration.ConfigurationSettings.AppSettings["TetPath"].ToString();
        Process pConvertTetml = new Process();
        pConvertTetml.StartInfo.UseShellExecute = false;
        pConvertTetml.StartInfo.RedirectStandardError = true;
        pConvertTetml.StartInfo.RedirectStandardOutput = true;
        pConvertTetml.StartInfo.CreateNoWindow = true;
        pConvertTetml.StartInfo.Arguments = strParameter;
        pConvertTetml.StartInfo.FileName = Img_Conversion_bat;
        pConvertTetml.Start();
        pConvertTetml.WaitForExit();
        return this.lineTETMLPath;
        //SetXmlFilePath(XmlFile);
    }
}

public class MisMatch
{
    public int list1Index;
    public int list2Index;
    //public ArrayList list1Items;
    //public ArrayList list2Items;
    public int list1MMLen;
    public int list2MMLen;
    public string comments;
    public MisMatchType misMatchType;
    public int list1LNum;
    public int list1PNum;
    public int list2LNum;
    public int list2PNum;
    public int PageNumber;
    public int LineNumber;
    public MisMatch()
    {
        list1Index = -1;
        list2Index = -1;
        list2MMLen = -1;
        list1MMLen = -1;
        list1LNum = -1;
        list1PNum = -1;
        list2LNum = -1;
        list2PNum = -1;
        LineNumber = -1;
        PageNumber = -1;
        comments = "";
        misMatchType = new MisMatchType();

    }


    public MisMatch(int List1Index, int List2Index)
    {
        list1Index = List1Index;
        list2Index = List2Index;
        list1MMLen = -1;
        list2MMLen = -1;
        list1LNum = -1;
        list1PNum = -1;
        list2LNum = -1;
        list2PNum = -1;
        LineNumber = -1;
        PageNumber = -1;
        comments = "";
        misMatchType = new MisMatchType();
    }
}

public enum MisMatchType { Miss, Extra, Spell, Other, MissingInList1, MissingInList2 };
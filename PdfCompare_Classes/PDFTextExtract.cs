using System;
using System.IO;
using System.Diagnostics;
using System.Xml;
using System.Collections;
using System.Text.RegularExpressions;
using System.Text;
using TET_dotnet;

class PDFTextExtract
{
    PDFFileInfo pdfFileOBj;
    string PDFFilePath;
    string TETFilePath;
    public ArrayList extractedWords;
    public PDFTextExtract(string pdfFilePath)
    {
        this.PDFFilePath = pdfFilePath;
        this.TETFilePath = Createtetml();
        extractedWords = new ArrayList();
        //int wrdCount = GetWordCountFromTet(tetFilePath);
        //GetPDF();
    }



    public int GetWordCountFromTet()
    {
        string tetFilePath = TETFilePath;
        StreamReader sr = new StreamReader(tetFilePath);
        string tetContents = sr.ReadToEnd();
        int matchCount = Regex.Matches(tetContents, "<Text>").Count;
        int volBreakCount = Regex.Matches(tetContents, "<Text>Volume-Break</Text>").Count;
        int spCharCount = Regex.Matches(tetContents, "<Text>•</Text>").Count;
        matchCount = matchCount - volBreakCount - spCharCount;

        sr.Close();
        MatchCollection mc = Regex.Matches(tetContents, "(?<=<Text>).*?(?=</Text>)");
        string wordsFilePath = Path.GetFullPath(tetFilePath) + "-Wrds.txt";
        StreamWriter sw = new StreamWriter(wordsFilePath);
        for (int i = 0; i < mc.Count; i++)
        {
            if (!(mc[i].Value.Contains("Volume-Break") || mc[i].Value.Contains("•")))//|| mc[i].Value.Contains("-")))
            {
                extractedWords.Add(mc[i].Value);
                sw.WriteLine(mc[i].Value);
            }
        }
        sw.Close();

        return matchCount;
    }

    public int GetPDFWordCount()
    {
        //return pdfFileOBj.WordCount;
        return 1;
    }

    public PDFFileInfo GetPDF()
    {
        string filePath = this.PDFFilePath;
        //pdfFileOBj = new PDFFileInfo();

        /* global option list */
        string globaloptlist = "searchpath={../data ../../data}";

        /* document-specific  option list */
        string docoptlist = "";

        /* page-specific option list */
        //string pageoptlist = "granularity=page";

        string pageoptlist = "granularity=page";

        /* separator to emit after each chunk of text. This depends on the
         * applications needs; for granularity=word a space character may be useful.
         */
        string separator = "\n";

        /* basic image extract options (more below) */
        string baseimageoptlist = "compression=auto format=auto";

        /* set this to 1 to generate image data in memory */
        int inmemory = 0;

        TET tet;
        FileStream outfile;
        BinaryWriter w;
        int pageno = 0;
        string suffix = ".txt";
        string outfilename;
        string outfilebase;

        UnicodeEncoding unicode = new UnicodeEncoding(false, true);
        Byte[] byteOrderMark = unicode.GetPreamble();


        tet = new TET();

        try
        {
            int n_pages = -1;

            tet.set_option(globaloptlist);

            int doc = tet.open_document(filePath, docoptlist);

            if (doc == -1)
            {
                Console.WriteLine("Error {0} in {1}(): {2}",
                    tet.get_errnum(), tet.get_apiname(), tet.get_errmsg());
                // return (2);
            }

            /* get number of pages in the document */
            try
            {
                n_pages = (int)tet.pcos_get_number(doc, "length:pages");
            }
            catch { }
            //pdfFileOBj.pages = n_pages;
            StringBuilder docContent = new StringBuilder();
            for (int currPage = 1; currPage <= n_pages; currPage++)
            {
                int page;
                page = tet.open_page(doc, currPage, pageoptlist);
                string pageContent = tet.get_text(page);
                docContent.Append(pageContent);
                //tet.get
                string[] firstPageContents = pageContent.Split('\n');
                tet.close_page(page);
            }
            tet.close_document(doc);
            string text;
            //int page;
            int imageno = -1;

            if (tet.get_errnum() != 0)
            {
            }
        }
        catch (TETException e)
        {
        }
        catch (Exception e)
        {
        }
        finally
        {
            if (tet != null)
            {
                tet.Dispose();
            }
        }

        return pdfFileOBj;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public string Createtetml()
    {
        if (PDFFilePath == null)
            return null;

        //WriteLog("Generating tetml File............ Please Wait");
        //WriteLog("This Will Take Time Depending upon PDF Pages");
        string DirectoryPath = Directory.GetParent(PDFFilePath).ToString();
        string XmlFile = DirectoryPath + "//" + Path.GetFileNameWithoutExtension(PDFFilePath) + ".tetml";
        //tetFile = XmlFile;
        //string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks}\" -o \"" + XmlFile + "\" \"" + PDFFilePath + "\"";
        string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks} clippingarea={cropbox}\" -o \"" + XmlFile + "\" \"" + PDFFilePath + "\"";
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
        return XmlFile;
        //SetXmlFilePath(XmlFile);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns>Tetml file path</returns>
    public string CreateteTmlWithLineOption()
    {
        //WriteLog("Generating tetml File............ Please Wait");
        //WriteLog("This Will Take Time Depending upon PDF Pages");
        string DirectoryPath = Directory.GetParent(PDFFilePath).ToString();
        string XmlFile = DirectoryPath + "//" + Path.GetFileNameWithoutExtension(PDFFilePath) + ".tetml";
        //tetFile = XmlFile;
        //string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks}\" -o \"" + XmlFile + "\" \"" + PDFFilePath + "\"";
        string strParameter = "-m line --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks} clippingarea={cropbox}\" -o \"" + XmlFile + "\" \"" + PDFFilePath + "\"";
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
        return XmlFile;
        //SetXmlFilePath(XmlFile);
    }

}
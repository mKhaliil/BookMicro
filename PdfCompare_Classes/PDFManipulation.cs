using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using System.IO;
using iTextSharp.text;
using System.Xml;
using System.Configuration;
using System.Collections;
using System.Diagnostics;
using System.Web;
using System.Text.RegularExpressions;
using System.Drawing;
using System.Drawing.Drawing2D;
using Microsoft.Ajax.Utilities;
using Outsourcing_System.PdfCompare_Classes;
using Outsourcing_System;

public class PDFManipulation
{
    MyDBClass obj = new MyDBClass();

    private string PDFPath;

    private PDFManipulation()
    {
    }

    public PDFManipulation(string pdfPath)
    {
        this.PDFPath = pdfPath;
    }

    private int totalPages;

    public int TotalPages
    {
        get
        {
            try
            {
                // get input document
                PdfReader inputPdf = new PdfReader(this.PDFPath);
                // retrieve the total number of pages
                return inputPdf.NumberOfPages;
            }
            catch
            {
                return -1;
            }
        }
    }

    /// <summary>
    /// Extracts Page number of the pdfFile
    /// </summary>
    /// <param name="pageNum"></param>
    /// <returns>The path of the extracted file</returns>
    public string ExtractPageWithAnnotation(int pageNum)
    {

        #region annotation code

        string annotedFilePath = AddAnottation(PDFPath, pageNum);
        //File.Delete(outputFile);
        return annotedFilePath;

        #endregion
    }

    public List<Mistakes> GetTotalMistakes_List(string xmlPath)
    {
        if (xmlPath == "")
            return null;

        List<Mistakes> list = new List<Mistakes>();

        StreamReader strreader = new StreamReader(xmlPath);
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        XmlNodeList nodes = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @PDFmistake!='undo']");

        //int i = 1;

        if (nodes.Count > 0)
        {
            foreach (XmlElement node in nodes)
            {
                list.Add(new Mistakes
                {
                    mistakeNum = Convert.ToInt32(node.Attributes["PDFmistake"].Value),
                    page = Convert.ToInt32(node.Attributes["page"].Value)
                });

                //i++;
            }
        }

        return list;
    }

    //public double GetUpdatedTop(int page)
    //{
    //    if (xmlPath == "")
    //        return null;

    //    List<Mistakes> list = new List<Mistakes>();

    //    StreamReader strreader = new StreamReader(xmlPath);
    //    string xmlInnerText = strreader.ReadToEnd();
    //    strreader.Close();

    //    XmlDocument xmlDoc = new XmlDocument();
    //    xmlDoc.LoadXml(xmlInnerText);

    //    XmlNodeList nodes = xmlDoc.SelectNodes(@"//*[@PDFmistake]");

    //    //int i = 1;

    //    if (nodes.Count > 0)
    //    {
    //        foreach (XmlElement node in nodes)
    //        {
    //            list.Add(new Mistakes
    //            {
    //                mistakeNum = Convert.ToInt32(node.Attributes["PDFmistake"].Value),
    //                page = Convert.ToInt32(node.Attributes["page"].Value)
    //            });

    //            //i++;
    //        }
    //    }

    //    return list;
    //}


    public string ExtractPage(int pageNum)
    {
        string directory = Directory.GetParent(this.PDFPath).ToString();

        string outputFile = directory + "\\" + pageNum + ".pdf";
        ExtractPages(PDFPath, outputFile, pageNum, pageNum);

        return outputFile;
    }

    private string AddAnottation(string outputFile, int pageNum)
    {
        string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);
        if (String.IsNullOrEmpty(mainXmlPath))
            return null;

        Common obj = new Common();
        XmlDocument xmlFromRhyw = obj.LoadXmlFromFile(mainXmlPath.Replace(".xml", ".rhyw"));

        XmlDocument pageXML = Common.GetPageXmlDoc(pageNum.ToString(), xmlFromRhyw);
        string dirPath = Path.GetDirectoryName(outputFile);
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
        foreach (XmlNode xmlElement in xmlElements)
        {
            XmlNode lnNode = xmlElement.SelectSingleNode("ln");

            if (lnNode != null)
            {
                XmlAttribute attrNode = lnNode.Attributes["coord"];
                string cordinates = attrNode.Value;
                string anotHeading = xmlElement.Name;
                string anotText = xmlElement.Name;
                RHYWAnnotation rhywAnnot = null;
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
                float llx = float.Parse(cordinates.Split(':')[0]);
                float lly = float.Parse(cordinates.Split(':')[1]);
                float urx = float.Parse(cordinates.Split(':')[2]);
                float ury = float.Parse(cordinates.Split(':')[3]);
                float leftOffset = 20f;
                rhywAnnot.llx = llx;
                rhywAnnot.lly = lly;
                rhywAnnot.urx = urx;
                rhywAnnot.ury = ury;
                rhywAnnot.llx -= leftOffset;
            }
            //WriteAnnotationInFile(pdfFilePath, 1, anotHeading, anotText, llx, lly, urx, ury);
            //annotations.Add(rhywAnnot);//aamir
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
            string newFile = Path.GetDirectoryName(pdfFilePath) + "\\" + Path.GetFileNameWithoutExtension(pdfFilePath) +
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


    //private void ExtractPages(string inputFile, string outputFile, int start, int end)
    //{
    //    // get input document
    //    PdfReader inputPdf = new PdfReader(inputFile);
    //    // retrieve the total number of pages
    //    int pageCount = inputPdf.NumberOfPages;
    //    if (end < start || end > pageCount)
    //    {
    //        end = pageCount;
    //    }

    //    //var pgSize = new iTextSharp.text.Rectangle(myWidth, myHeight);
    //    //var doc = new iTextSharp.text.Document(pgSize, leftMargin, rightMargin, topMargin, bottomMargin);

    //    // load the input document
    //    Document inputDoc = new Document(inputPdf.GetCropBox(1));

    //    // create the filestream
    //    using (FileStream fs = new FileStream(outputFile, FileMode.Create))
    //    {
    //        // create the output writer
    //        PdfWriter outputWriter = PdfWriter.GetInstance(inputDoc, fs);
    //        inputDoc.Open();
    //        PdfContentByte cb1 = outputWriter.DirectContent;

    //        // copy pages from input to output document
    //        for (int i = start; i <= end; i++)
    //        {
    //            inputDoc.SetPageSize(inputPdf.GetPageSizeWithRotation(i));
    //            inputDoc.NewPage();

    //            PdfImportedPage page = outputWriter.GetImportedPage(inputPdf, i);
    //            int rotation = inputPdf.GetPageRotation(i);

    //            if (rotation == 90 || rotation == 270)
    //            {
    //                cb1.AddTemplate(page, 0, -1f, 1f, 0, 0, inputPdf.GetPageSizeWithRotation(i).Height);
    //            }
    //            else
    //            {
    //                cb1.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
    //            }
    //        }
    //        inputDoc.Close();
    //    }
    //}

    private void ExtractPages(string inputFile, string outputFile, int start, int end)
    {

        PdfReader reader = null;
        Document document = null;
        PdfCopy pdfCopyProvider = null;
        PdfImportedPage importedPage = null;

        try
        {
            // Intialize a new PdfReader instance with the contents of the source Pdf file:
            reader = new PdfReader(inputFile);

            // Capture the correct size and orientation for the page:
            document = new Document(reader.GetPageSizeWithRotation(1));

            // Initialize an instance of the PdfCopyClass with the source 
            // document and an output file stream:
            pdfCopyProvider = new PdfCopy(document,
                new System.IO.FileStream(outputFile, System.IO.FileMode.Create));

            document.Open();

            // Extract the desired page number:
            importedPage = pdfCopyProvider.GetImportedPage(reader, start);
            pdfCopyProvider.AddPage(importedPage);
            document.Close();
            reader.Close();
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string Createtetml(string filePath)
    {
        if (filePath == null)
            return null;

        if (File.Exists(filePath.Replace("pdf", "tetml")))
        {
            File.Delete(filePath.Replace("pdf", "tetml"));
        }

        //WriteLog("Generating tetml File............ Please Wait");
        //WriteLog("This Will Take Time Depending upon PDF Pages");
        string DirectoryPath = Directory.GetParent(filePath).ToString();
        string wordTETMLPath = DirectoryPath + "\\" + Path.GetFileNameWithoutExtension(filePath) + ".tetml";
        //tetFile = XmlFile;
        //string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks}\" -o \"" + XmlFile + "\" \"" + PDFFilePath + "\"";
        string strParameter =
            "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks} clippingarea={cropbox}\" -o \"" +
            wordTETMLPath + "\" \"" + filePath + "\"";
        //string Img_Conversion_bat = @"D:\work\tet.exe";

        string email = "";
        if (Convert.ToString(HttpContext.Current.Session["Email"]) != "")
        {
            email = Convert.ToString(HttpContext.Current.Session["Email"]);
        }
        else
        {
            return "";
        }
        string tetFilePath = Common.GetDirectoryPath() + "User Files/" + email.Trim() + "/XSL/tet.exe";

        //string Img_Conversion_bat = System.Configuration.ConfigurationSettings.AppSettings["TetPath"].ToString();

        string Img_Conversion_bat = tetFilePath;
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
    }

    //public string HighlightWord(string pdfPath, List<PdfWord> wrdList, int subPdfPage)
    //{
    //    string outputFile = "";
    //    outputFile = SelectThroughStamper(pdfPath, wrdList, wrdList[0].PageNumber, HighlightType.Added, subPdfPage);
    //    return outputFile;
    //}

    public string HighlightWord(string pdfPath, List<PdfWord> wrdList)
    {
        string outputFile = "";
        outputFile = SelectThroughStamper(pdfPath, wrdList, wrdList[0].PageNumber, HighlightType.Added);
        return outputFile;
    }

    private bool WaitForFile(string fullPath)
    {
        int numTries = 0;
        while (true)
        {
            ++numTries;
            try
            {
                // Attempt to open the file exclusively.
                using (FileStream fs = new FileStream(fullPath,
                    FileMode.Open, FileAccess.ReadWrite,
                    FileShare.None, 100))
                {
                    fs.ReadByte();

                    // If we got this far the file is ready
                    break;
                }
            }
            catch (Exception ex)
            {
                //Log.LogWarning(
                //   "WaitForFile {0} failed to get an exclusive lock: {1}",
                //    fullPath, ex.ToString());

                if (numTries > 20)
                {
                    //Log.LogWarning(
                    //    "WaitForFile {0} giving up after 10 tries",
                    //    fullPath);
                    return false;
                }

                // Wait for the lock to be released
                System.Threading.Thread.Sleep(1000);
            }
        }

        //Log.LogTrace("WaitForFile {0} returning true after {1} tries",
        //    fullPath, numTries);
        return true;
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


    ///// <summary>
    ///// 
    ///// </summary>
    ///// <param name="prmCoord"></param>
    ///// <param name="prmPageNo"></param>
    ///// <param name="hType"></param>
    ///// <returns>The path of the higlighted PDF file</returns>
    //private string SelectThroughStamper(string inputFile, List<Word> prmCoordList, int prmPageNo, HighlightType hType)
    //{
    //    Stream inputImageStream = null;
    //    //string inputFile = this.PDFPath;
    //    // Random rnd = new Random();
    //    string outputPDFFile;
    //    if (!Path.GetFileNameWithoutExtension(inputFile).Contains("Produced"))
    //    {
    //        outputPDFFile = Path.GetDirectoryName(inputFile) + "\\" +
    //                        Path.GetFileNameWithoutExtension(inputFile).Split('_')[0] + "_Stamped.pdf";
    //    }
    //    else
    //    {
    //        outputPDFFile = Path.GetDirectoryName(inputFile) + "\\" + "Produced_" + prmPageNo + "_Stamped.pdf";
    //    }

    //    string inputPDFFile = "";
    //    string imageFileName = "";

    //    //if ((Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test")) && (Common.GetTotalMistakes_Comparison0Test(prmPageNo) > 0))
    //    //{
    //    //    imageFileName = ConfigurationManager.AppSettings["highlightGIF"];
    //    //}
    //    //else
    //    //{
    //    imageFileName = System.Configuration.ConfigurationManager.AppSettings["highlightGIF"].ToString();
    //    //}


    //    //if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
    //    //{
    //    //    imageFileName = "";
    //    //}
    //    //else
    //    //{
    //    //    imageFileName = ConfigurationManager.AppSettings["highlightGIF"];
    //    //}

    //    if (File.Exists(imageFileName))
    //    {

    //        //if (hType == HighlightType.Strike)
    //        //    imageFileName = "D:\\strike.jpg";
    //        //else if (hType == HighlightType.Added)
    //        //    imageFileName = "D:\\high.jpg";
    //        //else
    //        //    imageFileName = "D:\\high.jpg";
    //    }
    //    else
    //    {
    //        return inputFile;
    //    }

    //    if (File.Exists(outputPDFFile))
    //    {
    //        bool check = IsFileLocked(new FileInfo(outputPDFFile));

    //        if (check)
    //        {
    //            System.GC.Collect();
    //            System.GC.WaitForPendingFinalizers();
    //            File.Delete(outputPDFFile);
    //        }
    //    }

    //    int count = 1;

    //    if (prmCoordList.Count > 0)
    //    {
    //        foreach (var item in prmCoordList)
    //        {
    //            if (count == 2)
    //                inputFile = outputPDFFile;

    //            using (Stream inputPdfStream = new FileStream(inputFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
    //            {
    //                using (inputImageStream = new FileStream(imageFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
    //                {
    //                    using (
    //                        Stream outputPdfStream = new FileStream(outputPDFFile, FileMode.OpenOrCreate, FileAccess.ReadWrite,
    //                            FileShare.ReadWrite))
    //                    {
    //                        string[] Coord = item.StrCordinates.Split(":".ToCharArray());
    //                        float llx = float.Parse(Coord[0]);
    //                        float lly = float.Parse(Coord[1]);
    //                        float urx = float.Parse(Coord[2]);
    //                        float ury = float.Parse(Coord[3]);

    //                        var reader = new PdfReader(inputPdfStream);
    //                        var stamper = new PdfStamper(reader, outputPdfStream);
    //                        var pdfContentByte = stamper.GetUnderContent(1);

    //                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);
    //                        float oldHeight = image.ScaledHeight;
    //                        float imgScaledHeight = ury - lly;
    //                        float heightAdjustment = oldHeight - imgScaledHeight;
    //                        image.ScaleAbsolute(urx - llx, ury - lly);
    //                        image.SetAbsolutePosition(llx, lly); // + heightAdjustment);
    //                        pdfContentByte.AddImage(image);
    //                        stamper.Close();
    //                        outputPdfStream.Close();
    //                        outputPdfStream.Dispose();
    //                    }
    //                }
    //            }
    //            count++;
    //        }
    //    }
    //    return outputPDFFile;
    //}

    //aamir

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prmCoord"></param>
    /// <param name="prmPageNo"></param>
    /// <param name="hType"></param>
    /// <returns>The path of the higlighted PDF file</returns>
    private string SelectThroughStamper(string inputFile, List<PdfWord> prmCoordList, int prmPageNo, HighlightType hType)
    {
        //, int subPdfPage
        try
        {

            Stream inputImageStream = null;

            string outputPDFFile;
            if (!Path.GetFileNameWithoutExtension(inputFile).Contains("Produced"))
            {
                outputPDFFile = Path.GetDirectoryName(inputFile) + "\\" +
                                Path.GetFileNameWithoutExtension(inputFile).Split('_')[0] + "_Stamped.pdf";
            }
            else
            {
                //if (subPdfPage == 0)
                //    outputPDFFile = Path.GetDirectoryName(inputFile) + "\\" + "Produced_" + prmPageNo + "_Stamped.pdf";
                //else
                //    outputPDFFile = Path.GetDirectoryName(inputFile) + "\\" + "Produced_" + prmPageNo + "_" + subPdfPage + "_Stamped.pdf";

                    outputPDFFile = Path.GetDirectoryName(inputFile) + "\\" + "Produced_" + prmPageNo + "_Stamped.pdf";
            }

            string inputPDFFile = "";
            string imageFileName = "";

            imageFileName = ConfigurationManager.AppSettings["highlightGIF"].ToString();

            //if (File.Exists(imageFileName))
            //{
            //}
            //else
            //{
            //    return inputFile;
            //}

            if (File.Exists(outputPDFFile))
            {
                bool check = IsFileLocked(new FileInfo(outputPDFFile));

                if (check)
                {
                    System.GC.Collect();
                    System.GC.WaitForPendingFinalizers();
                    File.Delete(outputPDFFile);
                }

                //if (IsFileLocked(new FileInfo(inputFile)))
                //{

                //    //Process[] open_procs = Process.GetProcesses();
                //    //if (open_procs.Length > 0)
                //    //{
                //    //    foreach (var proc in open_procs)
                //    //    {
                //    //        proc.Kill();
                //    //    }
                //    //}
                //}
            }

            int count = 1;

            if (prmCoordList.Count > 0)
            {
                PdfReader reader = new PdfReader(File.ReadAllBytes(inputFile));

                //foreach (var item in prmCoordList)
                //{
                if (count == 2)
                    inputFile = outputPDFFile;

                //using (Stream inputPdfStream = new FileStream(inputFile, FileMode.Open, FileAccess.Read, FileShare.Read))
                //{
                using (inputImageStream = new FileStream(imageFileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                {
                    using (Stream outputPdfStream = new FileStream(outputPDFFile, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite))
                    {
                        //var reader = new PdfReader(inputPdfStream);


                        iTextSharp.text.Rectangle cropbox = reader.GetCropBox(1);
                        var box = reader.GetPageSizeWithRotation(1);

                        double top = (box.Top - cropbox.Top);
                        double bottom = cropbox.Bottom;
                        double right = (box.Right - cropbox.Right);
                        double left = cropbox.Left;

                        var stamper = new PdfStamper(reader, outputPdfStream);
                        var pdfContentByte = stamper.GetUnderContent(1);
                        iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(inputImageStream);

                        foreach (var item in prmCoordList)
                        {
                            string[] Coord = item.StrCordinates.Split(":".ToCharArray());
                            float llx = float.Parse(Coord[0]) + (float)left;
                            float lly = float.Parse(Coord[1]) + (float)bottom;
                            float urx = float.Parse(Coord[2]) + (float)left;
                            float ury = float.Parse(Coord[3]) + (float)bottom;

                            //float oldHeight = image.ScaledHeight;
                            //float imgScaledHeight = ury - lly;
                            //float heightAdjustment = oldHeight - imgScaledHeight;
                            image.ScaleAbsolute(urx - llx, ury - lly);
                            image.SetAbsolutePosition(llx, lly); // + heightAdjustment);
                            pdfContentByte.AddImage(image);
                        }

                        stamper.Close();
                        outputPdfStream.Close();
                        outputPdfStream.Dispose();
                        inputImageStream.Close();
                        inputImageStream.Dispose();
                    }
                }
                //}
                count++;
                //}
            }
            return outputPDFFile;
        }
        catch (Exception ex)
        {
            return "";
        }
    }

    public string LoadMistakePanel(string pageNumber, string mainXmlPath, List<string> originalText, string pdfType)
    {
        //string pageNumOriginal = pageNUm;
        //string modifiedPageNum = pageNUm;
        //string pageNumber = pageNUm;


        //if (modifiedPageNum.Contains("_Produced_Stamped"))
        //{
        //    pageNumber = modifiedPageNum.Replace("_Produced_Stamped", "").Trim();
        //    pageNumber = "Produced_" + pageNumber;
        //}

        //else if (modifiedPageNum.Contains("_Stamped"))
        //{
        //    pageNumber = modifiedPageNum.Replace("_Stamped", "").Trim();
        //}

        //Session["Current_SrcPdfPage"] = "Produced_" + currentPage + ".pdf";
        //       Session["Current_PrdPdfPage"]

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
        Word wrd_Produced = null;

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
                //textToCheck = ReplaceWhiteSpace_(word.Trim()).Split(',');

                textToCheck = RemoveWhiteSpace(word.Trim()).Split(',');
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
        //return SelectCurrentWordInPDFWithAnnotation(pdfPath, wrdList);

        PDFManipulation pdfMan = new PDFManipulation(pdfPath);
        return pdfMan.HighlightWord(pdfPath, wrdList);
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

    //public int GetTotalMistakes(int page)
    //{
    //    StreamReader strreader = new StreamReader(Convert.ToString(Session["MainXMLFilePath"]));
    //    string xmlInnerText = strreader.ReadToEnd();
    //    strreader.Close();

    //    XmlDocument xmlDoc = new XmlDocument();
    //    xmlDoc.LoadXml(xmlInnerText);

    //    int totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @page=" + page + "]").Count;
    //    return totalMistakes;
    //}

    public string GetMistakeByPage(int page)
    {
        string mistakeNumber = null;

        string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);

        if (mainXmlPath == "")
            return null;

        StreamReader strreader = new StreamReader(mainXmlPath);
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        var totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @page=" + page + "]");

        if (totalMistakes.Count > 0)
            mistakeNumber = totalMistakes[0].Attributes["PDFmistake"].Value;

        return mistakeNumber;
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

        string secondfilepath = LoadMistakePanel(Convert.ToString(currentPage), mainXmlPath, selectedText, "producedPdf");

        HttpContext.Current.Session["Highlighted_prdPdfPagePath"] = secondfilepath;

        //To do
        ReplacePrdText(selectedText);

        string firstfilepath = LoadMistakePanel(Convert.ToString(currentPage), mainXmlPath, selectedText, "sourcePdf");

        HttpContext.Current.Session["Highlighted_srcPdfPagePath"] = firstfilepath;
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

    public bool MatchLineByWords(string srcLineText, string prdLineText)
    {
        var srcLineText_Temp = Regex.Split(srcLineText, @"\s+");
        var prdLineText_Temp = Regex.Split(prdLineText, @"\s+");
        int counter = 0;
        double matchCounter = 0;

        if ((srcLineText_Temp.Length == 1) || (srcLineText_Temp.Length == 2))
            matchCounter = 1;

        else if ((srcLineText_Temp.Length > 2) && (srcLineText_Temp.Length <= 5))
            matchCounter = 2;

        else if (srcLineText_Temp.Length > 5)
            matchCounter = 4;

        for (int i = 0; i < srcLineText_Temp.Length; i++)
        {
            for (int j = 0; j < prdLineText_Temp.Length; j++)
            {
                if (srcLineText_Temp[i].Equals(prdLineText_Temp[j]))
                {
                    counter++;
                    break;
                }
            }
        }

        if (counter >= matchCounter)
            return true;

        else
            return false;
    }

    private List<Mistakes> GetTotalMistakes_List()
    {
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
                    page = Convert.ToInt32(node.Attributes["page"].Value)
                });

                i++;
            }
        }

        return list;
    }

    public void SetXls_Variables()
    {
        string marginTop = "xsl:variable[@name=\"margin-top\"]";
        string marginBottom = "xsl:variable[@name=\"margin-bottom\"]";
        string marginRight = "xsl:variable[@name=\"margin-right\"]";
        string marginLeft = "xsl:variable[@name=\"margin-left\"]";
        string pageWidth = "xsl:variable[@name=\"doc-page-width\"]";
        string pageHeight = "xsl:variable[@name=\"doc-page-height\"]";
        //string tableTopMargin = "xsl:variable[@name=\"tableTopMargin\"]";
        //string topPageMargin = "xsl:variable[@name=\"topPageMargin\"]";
        //string imgTopMargin = "xsl:variable[@name=\"imageMarginTop\"]";
        double top = 0;
        double bottom = 0;
        double right = 0;
        double left = 0;
        double width = 0;
        double height = 0;
        double tableMargin = 0;
        //double topMargin = 0;
        //double imgTop = 0;
        int page = 1;

        string mainXml = SiteSession.MainXMLFilePath_PDF.Replace(".xml", ".pdf");
        PdfReader pdfReader = new PdfReader(mainXml);

        var pdfPage = pdfReader.GetPageSize(page);

        //1 Inch = 72 Points [Postscript]

        //1 Point = 0.01388888889 Inch

        //1 PostScript point = 0.352777778 millimeters
        //units in mm
        width = pdfPage.Width * 0.352777778;
        height = pdfPage.Height * 0.352777778;

        iTextSharp.text.Rectangle cropbox = pdfReader.GetCropBox(page);
        var box = pdfReader.GetPageSizeWithRotation(page);

        top = Math.Round((box.Top - cropbox.Top) * 0.352777778, 3);
        bottom = Math.Round(cropbox.Bottom * 0.352777778, 3);
        right = Math.Round((box.Right - cropbox.Right) * 0.352777778, 3);
        left = Math.Round(cropbox.Left * 0.352777778, 3);

        XmlDocument doc = new XmlDocument();
        doc.Load(ConfigurationManager.AppSettings["XSLPathCoord"]);
        XmlNode root = doc.DocumentElement;
        XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
        nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

        root.SelectSingleNode(marginTop, nsmgr).Attributes["select"].Value = Convert.ToString(top);
        root.SelectSingleNode(marginBottom, nsmgr).Attributes["select"].Value = Convert.ToString(bottom);
        root.SelectSingleNode(marginRight, nsmgr).Attributes["select"].Value = Convert.ToString(right);
        root.SelectSingleNode(marginLeft, nsmgr).Attributes["select"].Value = Convert.ToString(left);
        root.SelectSingleNode(pageWidth, nsmgr).Attributes["select"].Value = Convert.ToString(width);
        root.SelectSingleNode(pageHeight, nsmgr).Attributes["select"].Value = Convert.ToString(height);
        //root.SelectSingleNode(tableTopMargin, nsmgr).Attributes["select"].Value = Convert.ToString(tableMargin);
        //root.SelectSingleNode(topPageMargin, nsmgr).Attributes["select"].Value = Convert.ToString(topMargin);
        //root.SelectSingleNode(imgTopMargin, nsmgr).Attributes["select"].Value = Convert.ToString(imgTop);

        doc.Save(ConfigurationManager.AppSettings["XSLPathCoord"]);
    }

    //public bool IsContainsSameText(string xmlText, string selectedText)
    //{
    //    xmlText = xmlText.Replace("…", "").Replace(".", "");
    //    selectedText = selectedText.Replace("…", "").Replace(".", "");

    //    var splittedXmlText = Regex.Split(xmlText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
    //    var splittedSelectedText = Regex.Split(selectedText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

    //    if (splittedXmlText != null && splittedSelectedText != null)
    //    {
    //        if (splittedXmlText.Count > 0 && splittedSelectedText.Count > 0)
    //        {
    //            if (splittedXmlText.Count == 1 && splittedSelectedText.Count == 1)
    //            {
    //                if (splittedXmlText[0].Equals(splittedSelectedText[0]))
    //                    return true;
    //            }
    //            else if (splittedXmlText.Count == 2 && splittedSelectedText.Count == 2)
    //            {
    //                if (splittedXmlText[0].Equals(splittedSelectedText[0]))
    //                {
    //                    if (splittedXmlText[1].Equals(splittedSelectedText[1]))
    //                        return true;
    //                }
    //            }
    //            else if (splittedXmlText.Count > 2 && splittedSelectedText.Count > 2)
    //            {
    //                if (splittedXmlText[0].Equals(splittedSelectedText[0]))
    //                {
    //                    if (splittedXmlText[1].Equals(splittedSelectedText[1]))
    //                    {
    //                        if (splittedXmlText[2].Equals(splittedSelectedText[2]))
    //                            return true;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    return false;
    //}

    private bool IsContainsSameText(string paraLine, string tetmlLine)
    {
        if (string.IsNullOrEmpty(paraLine) || string.IsNullOrEmpty(tetmlLine)) return false;

        List<string> pdfJsLineTempList = Regex.Split(paraLine, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
        List<string> xmlLineTempList = Regex.Split(tetmlLine, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

        string pdfJsText = Regex.Replace(paraLine, "[^A-Za-z0-9]", "");
        string xmlText = Regex.Replace(tetmlLine, "[^A-Za-z0-9]", "");

        if (!string.IsNullOrEmpty(paraLine) && string.IsNullOrEmpty(tetmlLine))
        {
            int matchingPer = GetMatchingPercentage(pdfJsLineTempList[0].Trim(), xmlLineTempList[0].Trim());
            if (matchingPer < 50)
                return false;
        }

        if (paraLine.Trim().Equals(tetmlLine.Trim()))
            return true;

        //string pdfJsText = RemoveWhiteSpaceFromText(RemoveSpecialCharacters(paraLine));
        //string xmlText = RemoveWhiteSpaceFromText(RemoveSpecialCharacters(Convert.ToString(tetmlLine)));

        string finalPdfJsText = "";
        string finalxmlText = "";

        if (pdfJsText.Length == xmlText.Length)
        {
            if (pdfJsText != xmlText)
            {
                int matchingPer = GetMatchingPercentage(pdfJsText, xmlText);
                if (matchingPer >= 80)
                    return true;
            }
            else
                return true;
        }
        else if (pdfJsText.Length > xmlText.Length)
        {
            finalPdfJsText = pdfJsText.Substring(0, xmlText.Length);
            finalxmlText = xmlText;
        }
        else if (xmlText.Length > pdfJsText.Length)
        {
            finalPdfJsText = pdfJsText;
            finalxmlText = xmlText.Substring(0, pdfJsText.Length);
        }

        if (string.IsNullOrEmpty(finalPdfJsText) || string.IsNullOrEmpty(finalxmlText)) return false;

        if (finalPdfJsText.ToLower().Equals(finalxmlText.ToLower()))
        {
            return true;
        }
        else
        {
            if (pdfJsLineTempList.Count > 3 && xmlLineTempList.Count > 3)
            {
                if (pdfJsLineTempList[0].Trim().Equals(xmlLineTempList[0].Trim()))
                {
                    if (pdfJsLineTempList[1].Trim().Equals(xmlLineTempList[1].Trim()))
                    {
                        if (pdfJsLineTempList[2].Trim().Equals(xmlLineTempList[2].Trim()))
                        {
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public string RemoveSpecialCharacters(string word)
    {
        return word.Replace(",", "").Replace(",", "").Replace("’", "").Replace("‘", "").Replace(",", "").Replace("***", "")
            .Replace(".", "").Replace("…", "").Replace("-", "").Trim().ToLower();
    }

    public int GetMatchingPercentage(string pdfJsLine, string tetmlLine)
    {
        double matchedCharCount = 0;
        double totalChars = pdfJsLine.Length;

        for (int i = 0; i < pdfJsLine.Length; i++)
        {
            if (i <= tetmlLine.Length - 1)
            {
                if (Convert.ToString(pdfJsLine[i]).Equals(Convert.ToString(tetmlLine[i]))) matchedCharCount++;
            }
        }

        if ((int)totalChars == 0 || (int)matchedCharCount == 0) return 0;

        double result = (matchedCharCount / totalChars) * 100;

        return Convert.ToInt32(result);
    }

    public string RemoveWhiteSpaceFromText(string input)
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

    public bool IsContainsSameText__old(string xmlText, string selectedText)
    {
        xmlText = xmlText.Trim().Replace("…", "").Replace(".", "").Replace("­-", "");
        selectedText = selectedText.Trim().Replace("…", "").Replace(".", "").Replace("­-", "");

        var splittedXmlText = Regex.Split(xmlText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToArray();
        var splittedSelectedText = Regex.Split(selectedText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToArray();

        if (splittedXmlText != null && splittedSelectedText != null)
        {
            if (splittedXmlText.Length > 0 && splittedSelectedText.Length > 0)
            {
                if (CheckSameLenWords(splittedXmlText, splittedSelectedText))
                {
                    if (splittedXmlText.Length == 1 && splittedSelectedText.Length == 1)
                    {
                        if (splittedXmlText[0].Equals(splittedSelectedText[0]))
                            return true;
                    }
                    else if (splittedXmlText.Length == 2 && splittedSelectedText.Length == 2)
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

                    if (xmlTextLength < 3 || pdfJsTextLength < 3)
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
        return false;
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

    public int GetSelectedLineNum(string lineText, string innerHtml, string page)
    {
        if (string.IsNullOrEmpty(page)) return 0;

        if (innerHtml.Equals(""))
        {
            return 0;
        }
        List<string> divText = new List<string>();

        List<string> allDivList = innerHtml.Split(new string[] { "<div data-canvas-width" }, StringSplitOptions.None).Where(x => !string.IsNullOrEmpty(x)).ToList();
        int topStartIndex = 0;
        int fontSizeStartIndex = 0;
        int nextTopStartIndex = 0;
        int nextFontSizeStartIndex = 0;

        int lineCounter = 0;
        string topValue = "";
        string nextTopValue = "";
        double topMarginValue = 1;

        List<PdfJsLine> pdfJsLines = new List<PdfJsLine>();
        StringBuilder sbDivText = new StringBuilder();
        StringBuilder sbDivNumber = new StringBuilder();

        for (int i = 0; i < allDivList.Count; i++)
        {
            topStartIndex = allDivList[i].IndexOf("top");
            fontSizeStartIndex = allDivList[i].IndexOf("font-size");

            if (topStartIndex != -1 && fontSizeStartIndex != -1)
            {
                topValue = allDivList[i].Substring(topStartIndex + 4, fontSizeStartIndex - topStartIndex - 4).Replace("px", "")
                                        .Replace(";", "").Trim();

                var tempText = allDivList[i].Split(new string[] { ">" }, StringSplitOptions.None)[1].Split(new string[] { "</div" },
                                                                StringSplitOptions.None).Where(x => (!string.IsNullOrEmpty(x))).ToArray();

                sbDivText.Append(Convert.ToString(tempText[0]).Replace("&nbsp;", "") + " ");
                sbDivNumber.Append(i + ",");

                if (!string.IsNullOrEmpty(topValue))
                {
                    if (i + 1 < allDivList.Count)
                    {
                        nextTopStartIndex = allDivList[i + 1].IndexOf("top");
                        nextFontSizeStartIndex = allDivList[i + 1].IndexOf("font-size");

                        if (nextTopStartIndex != -1 && nextFontSizeStartIndex != -1)
                        {
                            nextTopValue = allDivList[i + 1].Substring(nextTopStartIndex + 4, nextFontSizeStartIndex - nextTopStartIndex - 4)
                                                        .Replace("px", "").Replace(";", "").Trim();

                            if (Math.Abs(Convert.ToDouble(topValue) - Convert.ToDouble(nextTopValue)) > topMarginValue)
                            {
                                lineCounter++;

                                pdfJsLines.Add(new PdfJsLine
                                {
                                    Text = Convert.ToString(sbDivText),
                                    Top = topValue,
                                    LineNum = lineCounter,
                                    DivNum = Convert.ToString(sbDivNumber)
                                });

                                sbDivText.Length = 0;
                                sbDivNumber.Length = 0;
                                topValue = "";
                            }
                        }
                    }
                }
            }
        }//end for loop

        int lineNumber = 0;

        foreach (var line in pdfJsLines)
        {
            if (RemoveWhiteSpace(line.Text).Equals(RemoveWhiteSpace(lineText)))
            {
                lineNumber = line.LineNum;
                break;
            }
        }

        if (lineNumber > 0)
        {
            var selectedLineNum = pdfJsLines.Where(x => x.LineNum > 0 && x.LineNum == lineNumber).ToList()[0].LineNum;

            return selectedLineNum;
        }
        return 0;
    }


    public string GetParaName(string text)
    {
        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test")) return null;

        string pdfJsPageText = "";
        StringBuilder textType = new StringBuilder();
        //int page = SiteSession.MainCurrPage;
        string page = Convert.ToString(HttpContext.Current.Session["MainCurrPage"]);

        string attrName = "PDFmistake";
        bool containsPdfMistake = false;

        StringBuilder SparaType = new StringBuilder();
        StringBuilder SparaOrientation = new StringBuilder();
        StringBuilder SparaBackground = new StringBuilder();
        StringBuilder SparaSubType = new StringBuilder();
        bool IsSelectedErrorLine = false;

        Common comObj = new Common();
        XmlDocument xmlDoc = comObj.LoadXmlDocument(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));

        List<PdfJsLine> pdfJsLines = comObj.GetCurrentPdfJsPageLines(pdfJsPageText, Convert.ToInt32(page));
        int lineNum = 0;

        if (pdfJsLines != null)
        {
            if (pdfJsLines.Count > 0)
            {
                List<PdfJsLine> matchedLine = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Replace("\n", "").Equals(RemoveWhiteSpace(text).Replace("\n", ""))).ToList();

                if (matchedLine != null)
                {
                    if (matchedLine.Count > 0)
                    {
                        //text = matchedLine[0].Text;

                        if (matchedLine[0].LineNum > 0)
                        {
                            lineNum = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Replace("\n", "").Equals(RemoveWhiteSpace(text).Replace("\n", ""))).ToList()[0].LineNum;
                        }
                    }
                }
                //int lineNum = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(text))).ToList()[0].LineNum;

                ////int lineNum = GetSelectedLineNum(text, pdfJsPageText, Convert.ToString(page));

                XmlNodeList list_Lines = xmlDoc.SelectNodes(@"//ln [@page=" + page + "]");

                int lineCounter = 0;

                if ((list_Lines != null) && (list_Lines.Count > 0))
                {
                    int xmlLineCount = list_Lines.Count;
                    int pdfJsLinesCount = pdfJsLines.Count;

                    foreach (XmlNode ln in list_Lines)
                    {
                        lineCounter++;

                        //if (Regex.Replace(ln.InnerText, @"\s+", "").Equals(Regex.Replace(text, @"\s+", "")))
                        //if (Common.RemoveWhitespace(ln.InnerText).Equals(Common.RemoveWhitespace(text)))
                        //if (IsContainsSameText(ln.InnerText, text))

                        if (xmlLineCount != pdfJsLinesCount)
                        {
                            if (ln.ParentNode.Name.Equals("col") || ln.ParentNode.Name.Equals("head-col"))
                            {
                                double colUry = ln.Attributes["top"] != null
                                    ? Convert.ToDouble(ln.Attributes["top"].Value)
                                    : 0;
                                StringBuilder sbRowText = new StringBuilder();

                                if (ln.ParentNode.ParentNode != null)
                                {
                                    XmlNode rowNode = ln.ParentNode.ParentNode;

                                    var rowLines =
                                        rowNode.SelectNodes("//ln[@page='" + Convert.ToString(SiteSession.MainCurrPage) +
                                                            "']").Cast<XmlNode>()
                                            .Where(x => Convert.ToDouble(x.Attributes["top"].Value) == colUry)
                                            .ToList();

                                    if (rowLines != null && rowLines.Count >= 1)
                                    {
                                        foreach (XmlNode line in rowLines)
                                        {
                                            sbRowText.Append(line.InnerText + " ");
                                        }
                                    }
                                }

                                if (sbRowText.Length > 0)
                                {
                                    if (IsContainsSameText(Convert.ToString(sbRowText), text) &&
                                        (ln.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                                    {
                                        IsSelectedErrorLine = true;
                                    }
                                }
                            }
                            else
                            {
                                if (IsContainsSameText(ln.InnerText, text) && (ln.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                                {
                                    IsSelectedErrorLine = true;
                                }
                            }
                        }
                        else if (lineCounter == lineNum && xmlLineCount == pdfJsLinesCount)
                        {
                            IsSelectedErrorLine = true;
                        }

                        //if (lineCounter == lineNumber)
                        if (IsSelectedErrorLine)
                        {
                            IsSelectedErrorLine = false;
                            //if (lineCounter == lineNum)
                            //{
                            if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["ComparisonTask"])))
                            {
                                if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
                                {
                                    if (ln.Attributes != null && ln.Attributes.Count > 0)
                                    {
                                        foreach (XmlNode attr in ln.Attributes)
                                        {
                                            if (attr.Name.Equals(attrName) && !string.IsNullOrEmpty(attr.Value) &&
                                                !attr.Value.Equals("undo"))
                                            {
                                                containsPdfMistake = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (ln.ParentNode.Name.Equals("section-title"))
                            {
                                if (ln.ParentNode.ParentNode.ParentNode != null)
                                {
                                    textType.Append(Convert.ToString(ln.ParentNode.ParentNode.ParentNode.Attributes["type"]) != "" ? (ln.ParentNode.ParentNode.ParentNode.Attributes["type"].Value) : null);
                                }
                            }
                            else
                            {
                                if (ln.ParentNode.Name.Equals("line"))
                                {
                                    if (ln.ParentNode.ParentNode.Name.Equals("spara"))
                                    {
                                        SparaType.Append(ln.ParentNode.ParentNode.Attributes["type"] == null ? null : ln.ParentNode.ParentNode.Attributes["type"].Value);
                                        SparaOrientation.Append(ln.ParentNode.ParentNode.Attributes["h-align"] == null ? null : ln.ParentNode.ParentNode.Attributes["h-align"].Value);
                                        SparaBackground.Append(ln.ParentNode.ParentNode.Attributes["bgcolor"] == null ? null : ln.ParentNode.ParentNode.Attributes["bgcolor"].Value);
                                        SparaSubType.Append(ln.ParentNode.ParentNode.ChildNodes[0].Name);

                                        if (SparaType.ToString().Equals("other"))
                                            textType.Append("spara:" + SparaType + ":" + SparaSubType + ":" + SparaOrientation);
                                        else
                                            textType.Append("spara:" + SparaType + ":" + SparaSubType);
                                    }
                                }
                                else if (ln.ParentNode.Name.Equals("head-col") || ln.ParentNode.Name.Equals("col"))
                                {
                                    textType.Append("table");
                                }
                                else
                                {
                                    if (!(ln.ParentNode.Name.Equals("caption")))
                                    {
                                        textType.Append(ln.ParentNode.Name);
                                    }
                                }
                            }
                            break;
                        }
                    }
                }

                if (containsPdfMistake) return Convert.ToString(textType + ", true");

                else return Convert.ToString(textType);
            }
        }
        return null;
    }

    public string GetTextType(string text, string pdfJsPageText)
    {
        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test")) return null;

        StringBuilder textType = new StringBuilder();
        //int page = SiteSession.MainCurrPage;
        string page = Convert.ToString(HttpContext.Current.Session["MainCurrPage"]);

        string attrName = "PDFmistake";
        bool containsPdfMistake = false;

        StringBuilder SparaType = new StringBuilder();
        StringBuilder SparaOrientation = new StringBuilder();
        StringBuilder SparaBackground = new StringBuilder();
        StringBuilder SparaSubType = new StringBuilder();
        bool IsSelectedErrorLine = false;

        Common comObj = new Common();
        XmlDocument xmlDoc = comObj.LoadXmlDocument(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));

        if (xmlDoc == null) return null;

        List<PdfJsLine> pdfJsLines = comObj.GetCurrentPdfJsPageLines(pdfJsPageText, Convert.ToInt32(page));
        int lineNum = 0;

        if (pdfJsLines != null)
        {
            if (pdfJsLines.Count > 0)
            {
                List<PdfJsLine> matchedLine = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Replace("\n", "").Equals(RemoveWhiteSpace(text).Replace("\n", ""))).ToList();

                if (matchedLine != null)
                {
                    if (matchedLine.Count > 0)
                    {
                        //text = matchedLine[0].Text;

                        if (matchedLine[0].LineNum > 0)
                        {
                            lineNum = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Replace("\n", "").Equals(RemoveWhiteSpace(text).Replace("\n", ""))).ToList()[0].LineNum;
                        }
                    }
                }
                //int lineNum = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(text))).ToList()[0].LineNum;

                ////int lineNum = GetSelectedLineNum(text, pdfJsPageText, Convert.ToString(page));

                XmlNodeList list_Lines = xmlDoc.SelectNodes(@"//ln [@page=" + page + "]");

                int lineCounter = 0;

                if ((list_Lines != null) && (list_Lines.Count > 0))
                {
                    int xmlLineCount = list_Lines.Count;
                    int pdfJsLinesCount = pdfJsLines.Count;

                    foreach (XmlNode ln in list_Lines)
                    {
                        lineCounter++;

                        //if (Regex.Replace(ln.InnerText, @"\s+", "").Equals(Regex.Replace(text, @"\s+", "")))
                        //if (Common.RemoveWhitespace(ln.InnerText).Equals(Common.RemoveWhitespace(text)))
                        //if (IsContainsSameText(ln.InnerText, text))

                        if (xmlLineCount != pdfJsLinesCount)
                        {
                            if (ln.ParentNode.Name.Equals("col") || ln.ParentNode.Name.Equals("head-col"))
                            {
                                double colUry = ln.Attributes["top"] != null
                                    ? Convert.ToDouble(ln.Attributes["top"].Value)
                                    : 0;
                                StringBuilder sbRowText = new StringBuilder();

                                if (ln.ParentNode.ParentNode != null)
                                {
                                    XmlNode rowNode = ln.ParentNode.ParentNode;

                                    var rowLines =
                                        rowNode.SelectNodes("//ln[@page='" + Convert.ToString(SiteSession.MainCurrPage) +
                                                            "']").Cast<XmlNode>()
                                            .Where(x => Convert.ToDouble(x.Attributes["top"].Value) == colUry)
                                            .ToList();

                                    if (rowLines != null && rowLines.Count >= 1)
                                    {
                                        foreach (XmlNode line in rowLines)
                                        {
                                            sbRowText.Append(line.InnerText + " ");
                                        }
                                    }
                                }

                                if (sbRowText.Length > 0)
                                {
                                    if (IsContainsSameText(Convert.ToString(sbRowText), text) &&
                                        (ln.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                                    {
                                        IsSelectedErrorLine = true;
                                    }
                                }
                            }
                            else
                            {
                                if (IsContainsSameText(ln.InnerText, text) && (ln.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                                {
                                    IsSelectedErrorLine = true;
                                }
                            }
                        }
                        else if (lineCounter == lineNum && xmlLineCount == pdfJsLinesCount)
                        {
                            IsSelectedErrorLine = true;
                        }

                        //if (lineCounter == lineNumber)
                        if (IsSelectedErrorLine)
                        {
                            IsSelectedErrorLine = false;
                            //if (lineCounter == lineNum)
                            //{
                            if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["ComparisonTask"])))
                            {
                                if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
                                {
                                    if (ln.Attributes != null && ln.Attributes.Count > 0)
                                    {
                                        foreach (XmlNode attr in ln.Attributes)
                                        {
                                            if (attr.Name.Equals(attrName) && !string.IsNullOrEmpty(attr.Value) &&
                                                !attr.Value.Equals("undo"))
                                            {
                                                containsPdfMistake = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (ln.ParentNode.Name.Equals("section-title"))
                            {
                                if (ln.ParentNode.ParentNode.ParentNode != null)
                                {
                                    //textType.Append(Convert.ToString(ln.ParentNode.ParentNode.ParentNode.Attributes["type"]) != "" ? (ln.ParentNode.ParentNode.ParentNode.Attributes["type"].Value) : null);

                                    string nodeType = Convert.ToString(ln.ParentNode.ParentNode.ParentNode.Attributes["type"]) != "" ?            (ln.ParentNode.ParentNode.ParentNode.Attributes["type"].Value) : null;

                                    if (nodeType != null)
                                    {
                                        if (nodeType.Equals("level1"))
                                            textType.Append("Heading 1");

                                        else if (nodeType.Equals("level2"))
                                            textType.Append("Heading 2");

                                        else if (nodeType.Equals("level3"))
                                            textType.Append("Heading 3");

                                        else if (nodeType.Equals("level4"))
                                            textType.Append("Heading 4");

                                        else if (nodeType.Equals("chapter"))
                                            textType.Append("chapter");
                                    }
                                }
                            }
                            else if (ln.Attributes != null && ln.Attributes["lType"] != null && ln.Attributes["lType"].Value.Equals("footNote"))
                            {
                                textType.Append("footNote");
                            }
                            else if (ln.ParentNode != null && 
                                     ln.ParentNode.ParentNode != null &&
                                ln.ParentNode.ParentNode.Name.Equals("box"))
                            {
                                textType.Append("Highlighted Text");
                            }
                            else if (ln.ParentNode != null &&
                           ln.ParentNode.Name.Equals("upara"))
                            {
                                textType.Append("Normal Para");
                            }
                            else
                            {
                                if (ln.ParentNode.Name.Equals("line") || ln.ParentNode.Name.Equals("para"))
                                {
                                    if (ln.ParentNode.ParentNode.Name.Equals("spara"))
                                    {
                                        SparaType.Append(ln.ParentNode.ParentNode.Attributes["type"] == null ? null : ln.ParentNode.ParentNode.Attributes["type"].Value);
                                        SparaOrientation.Append(ln.ParentNode.ParentNode.Attributes["h-align"] == null ? null : ln.ParentNode.ParentNode.Attributes["h-align"].Value);
                                        SparaBackground.Append(ln.ParentNode.ParentNode.Attributes["bgcolor"] == null ? null : ln.ParentNode.ParentNode.Attributes["bgcolor"].Value);
                                        SparaSubType.Append(ln.ParentNode.ParentNode.ChildNodes[0].Name);

                                        //if (SparaType.ToString().Equals("other"))
                                        //    textType.Append("spara:" + SparaType + ":" + SparaSubType + ":" + SparaOrientation);
                                        //else
                                        //    textType.Append("spara:" + SparaType + ":" + SparaSubType);

                                        if (SparaType.ToString().Equals("other"))
                                            textType.Append("Special Para");
                                        else
                                            textType.Append("Special Para");
                                    }
                                }
                                else if (ln.ParentNode.Name.Equals("head-col") || ln.ParentNode.Name.Equals("col"))
                                {
                                    textType.Append("table");
                                }
                                else
                                {
                                    if (!(ln.ParentNode.Name.Equals("caption")))
                                    {
                                        textType.Append(ln.ParentNode.Name);
                                    }
                                }
                            }
                            break;
                        }
                    }
                }

                if (containsPdfMistake) return Convert.ToString(textType + ", true");

                else return Convert.ToString(textType);
            }
        }
        return null;
    }

    //2017-08-28
    //public string GetParaNameAsTooltip(string innerHtml, int page)
    //{
    //    List<String> divText = new List<string>();

    //    var allDivs = innerHtml.Split(new string[] { "<div data-canvas-width" }, StringSplitOptions.None);

    //    allDivs = allDivs.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

    //    foreach (var div in allDivs)
    //    {
    //        var temp = div.Split(new string[] { ">" }, StringSplitOptions.None)[1].Split(new string[] { "</div" }, StringSplitOptions.None);
    //        divText.Add(temp[0].ToString().Replace("&nbsp;", " "));
    //    }

    //    Common comObj = new Common();
    //    XmlDocument xmlDoc = comObj.LoadXmlDocument(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));

    //    if (xmlDoc == null) return "";

    //    XmlNodeList list_Lines = xmlDoc.SelectNodes(@"//ln[@page=" + page + "]");

    //    int count = 0;
    //   string line_Char = "";
    //    string div_Char = "";
    //    string div_Count = "";
    //    StringBuilder sb_LineWithComments = new StringBuilder();

    //    List<PdfJsLine> pdfJsPageLines = new List<PdfJsLine>();

    //    for (int i = 0; i < list_Lines.Count; i++)
    //    {
    //        if (!string.IsNullOrEmpty(list_Lines[i].InnerText.Trim()))
    //        {
    //            line_Char = String.Join("", list_Lines[i].InnerText.Where(c => (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))));

    //            div_Char = "";
    //            div_Count = "";

    //            for (int j = count; j < divText.Count; j++)
    //            {
    //                div_Char += String.Join("", divText[j].Where(c => (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))));
    //                //div_Count += (j + 1) + ",";
    //                div_Count += j + ",";

    //                if (div_Char.Equals(line_Char))
    //                {
    //                    pdfJsPageLines.Add(new PdfJsLine
    //                    {
    //                        Text = list_Lines[i].InnerText,
    //                        //DivNum = (i + 1) + "," + div_Count
    //                        DivNum = div_Count
    //                    });
    //                    count = j + 1;
    //                    break;
    //                }
    //            }
    //        }
    //    }

    //    if (pdfJsPageLines == null || pdfJsPageLines.Count == 0 || page < 1) return "";


    //    if (list_Lines != null && list_Lines.Count > 0)
    //    {
    //        foreach (var line in pdfJsPageLines)
    //        {
    //            var matchedLine = list_Lines.Cast<XmlNode>().Where(x => RemoveWhiteSpace(x.InnerText).Replace("\n", "")
    //                                               .Equals(RemoveWhiteSpace(line.Text).Replace("\n", ""))).ToList();

    //            if (matchedLine != null && matchedLine.Count > 0)
    //            {
    //                if (matchedLine[0].ParentNode != null)
    //                {
    //                    if (matchedLine[0].ParentNode.ParentNode != null && matchedLine[0].ParentNode.ParentNode.Name.Equals("box"))
    //                    {
    //                        line.ParaType = matchedLine[0].ParentNode.ParentNode.Name;
    //                    }
    //                    else if (matchedLine[0].ParentNode.Name.Equals("npara") || matchedLine[0].ParentNode.Name.Equals("upara"))
    //                    {
    //                        line.ParaType = matchedLine[0].ParentNode.Name;
    //                    }
    //                    else if (matchedLine[0].ParentNode.ParentNode != null && matchedLine[0].ParentNode.ParentNode.Name.Equals("spara"))
    //                    {
    //                        line.ParaType = matchedLine[0].ParentNode.ParentNode.Name;
    //                    }
    //                    else if (matchedLine[0].ParentNode.Name.Equals("section-title") && 
    //                        matchedLine[0].ParentNode.ParentNode != null &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode != null &&
    //                        matchedLine[0].ParentNode.ParentNode.Name.Equals("head") &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode.Name.Equals("section") &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes != null &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"] != null &&
    //                       matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"].Value.Equals("level1"))
    //                    {
    //                        line.ParaType = "level1";
    //                    }
    //                    else if (matchedLine[0].ParentNode.Name.Equals("section-title") &&
    //                       matchedLine[0].ParentNode.ParentNode != null &&
    //                       matchedLine[0].ParentNode.ParentNode.ParentNode != null &&
    //                       matchedLine[0].ParentNode.ParentNode.Name.Equals("head") &&
    //                       matchedLine[0].ParentNode.ParentNode.ParentNode.Name.Equals("section") &&
    //                       matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes != null &&
    //                       matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"] != null &&
    //                      matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"].Value.Equals("level2"))
    //                    {
    //                        line.ParaType = "level2";
    //                    }
    //                    else if (matchedLine[0].ParentNode.Name.Equals("section-title") &&
    //                        matchedLine[0].ParentNode.ParentNode != null &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode != null &&
    //                        matchedLine[0].ParentNode.ParentNode.Name.Equals("head") &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode.Name.Equals("section") &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes != null &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"] != null &&
    //                       matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"].Value.Equals("level3"))
    //                    {
    //                        line.ParaType = "level3";
    //                    }
    //                    else if (matchedLine[0].ParentNode.Name.Equals("section-title") &&
    //                        matchedLine[0].ParentNode.ParentNode != null &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode != null &&
    //                        matchedLine[0].ParentNode.ParentNode.Name.Equals("head") &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode.Name.Equals("section") &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes != null &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"] != null &&
    //                       matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"].Value.Equals("level4"))
    //                    {
    //                        line.ParaType = "level4";
    //                    }
    //                    else if (matchedLine[0].ParentNode.Name.Equals("section-title") &&
    //                        matchedLine[0].ParentNode.ParentNode != null &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode != null &&
    //                        matchedLine[0].ParentNode.ParentNode.Name.Equals("head") &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode.Name.Equals("section") &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes != null &&
    //                        matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"] != null &&
    //                       matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"].Value.Equals("chapter"))
    //                    {
    //                        line.ParaType = "chapter";
    //                    }
    //                }
                    
    //            }
    //            else
    //                line.ParaType = "";

    //            sb_LineWithComments.Append(line.DivNum + "~//~" + line.ParaType + "~//~");
    //        }
    //    }


    //    //foreach (var line in pdfJsPageLines)
    //    //{
    //    //    sb_LineWithComments.Append(line.DivNum + "~//~" + line.ParaType + "~//~");
    //    //}

    //    return sb_LineWithComments.ToString();
    //}
    
    public string GetParaNameAsTooltip(string innerHtml, int page)
    {
        List<String> divText = new List<string>();

        var allDivs = innerHtml.Split(new string[] { "<div data-canvas-width" }, StringSplitOptions.None);

        allDivs = allDivs.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

        foreach (var div in allDivs)
        {
            var temp = div.Split(new string[] { ">" }, StringSplitOptions.None)[1].Split(new string[] { "</div" }, StringSplitOptions.None);
            divText.Add(temp[0].ToString().Replace("&nbsp;", " "));
        }

        Common comObj = new Common();
        XmlDocument xmlDoc = comObj.LoadXmlDocument(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));

        if (xmlDoc == null) return "";

        XmlNodeList list_Lines = xmlDoc.SelectNodes(@"//ln[@page=" + page + "]");

        int count = 0;
        string line_Char = "";
        string div_Char = "";
        string div_Count = "";
        StringBuilder sb_LineWithComments = new StringBuilder();

        List<PdfJsLine> pdfJsPageLines = new List<PdfJsLine>();

        for (int i = 0; i < list_Lines.Count; i++)
        {
            if (!string.IsNullOrEmpty(list_Lines[i].InnerText.Trim()))
            {
                line_Char = String.Join("", list_Lines[i].InnerText.Where(c => (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))));

                div_Char = "";
                div_Count = "";

                for (int j = count; j < divText.Count; j++)
                {
                    div_Char += String.Join("", divText[j].Where(c => (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))));
                    //div_Count += (j + 1) + ",";
                    div_Count += j + ",";

                    if (div_Char.Equals(line_Char))
                    {
                        pdfJsPageLines.Add(new PdfJsLine
                        {
                            Text = list_Lines[i].InnerText,
                            //DivNum = (i + 1) + "," + div_Count
                            DivNum = div_Count
                        });
                        count = j + 1;
                        break;
                    }
                }
            }
        }

        if (pdfJsPageLines == null || pdfJsPageLines.Count == 0 || page < 1) return "";

        if (list_Lines != null && list_Lines.Count > 0)
        {
            foreach (var line in pdfJsPageLines)
            {
                var matchedLine = list_Lines.Cast<XmlNode>().Where(x => RemoveWhiteSpace(x.InnerText).Replace("\n", "")
                                                   .Equals(RemoveWhiteSpace(line.Text).Replace("\n", ""))).ToList();

                if (matchedLine != null && matchedLine.Count > 0)
                {
                    if (matchedLine[0].ParentNode != null)
                    {
                        if (matchedLine[0].ParentNode.ParentNode != null && matchedLine[0].ParentNode.ParentNode.Name.Equals("box"))
                        {
                            //line.ParaType = matchedLine[0].ParentNode.ParentNode.Name;
                            line.ParaType = "Highlighted Text";
                        }
                        else if (matchedLine[0].ParentNode.Name.Equals("npara") || matchedLine[0].ParentNode.Name.Equals("upara"))
                        {
                            //line.ParaType = matchedLine[0].ParentNode.Name;
                            line.ParaType = "Normal Para";
                        }
                        else if (matchedLine[0].ParentNode.ParentNode != null && matchedLine[0].ParentNode.ParentNode.Name.Equals("spara"))
                        {
                            //line.ParaType = matchedLine[0].ParentNode.ParentNode.Name;
                            line.ParaType = "Special Para";
                        }
                        else if (matchedLine[0].ParentNode.Name.Equals("section-title") &&
                            matchedLine[0].ParentNode.ParentNode != null &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode != null &&
                            matchedLine[0].ParentNode.ParentNode.Name.Equals("head") &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode.Name.Equals("section") &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes != null &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"] != null &&
                           matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"].Value.Equals("level1"))
                        {
                            line.ParaType = "Heading 1";
                        }
                        else if (matchedLine[0].ParentNode.Name.Equals("section-title") &&
                           matchedLine[0].ParentNode.ParentNode != null &&
                           matchedLine[0].ParentNode.ParentNode.ParentNode != null &&
                           matchedLine[0].ParentNode.ParentNode.Name.Equals("head") &&
                           matchedLine[0].ParentNode.ParentNode.ParentNode.Name.Equals("section") &&
                           matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes != null &&
                           matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"] != null &&
                          matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"].Value.Equals("level2"))
                        {
                            line.ParaType = "Heading 2";
                        }
                        else if (matchedLine[0].ParentNode.Name.Equals("section-title") &&
                            matchedLine[0].ParentNode.ParentNode != null &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode != null &&
                            matchedLine[0].ParentNode.ParentNode.Name.Equals("head") &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode.Name.Equals("section") &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes != null &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"] != null &&
                           matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"].Value.Equals("level3"))
                        {
                            line.ParaType = "Heading 3";
                        }
                        else if (matchedLine[0].ParentNode.Name.Equals("section-title") &&
                            matchedLine[0].ParentNode.ParentNode != null &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode != null &&
                            matchedLine[0].ParentNode.ParentNode.Name.Equals("head") &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode.Name.Equals("section") &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes != null &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"] != null &&
                           matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"].Value.Equals("level4"))
                        {
                            line.ParaType = "Heading 4";
                        }
                        else if (matchedLine[0].ParentNode.Name.Equals("section-title") &&
                            matchedLine[0].ParentNode.ParentNode != null &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode != null &&
                            matchedLine[0].ParentNode.ParentNode.Name.Equals("head") &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode.Name.Equals("section") &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes != null &&
                            matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"] != null &&
                           matchedLine[0].ParentNode.ParentNode.ParentNode.Attributes["type"].Value.Equals("chapter"))
                        {
                            line.ParaType = "chapter";
                        }
                    }

                }
                else
                    line.ParaType = "";

                sb_LineWithComments.Append(line.DivNum + "~//~" + line.ParaType + "~//~");
            }
        }


        //foreach (var line in pdfJsPageLines)
        //{
        //    sb_LineWithComments.Append(line.DivNum + "~//~" + line.ParaType + "~//~");
        //}

        return sb_LineWithComments.ToString();
    }

    public string GetParaName(string text, string pdfJsPageText)
    {
        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test")) return null;

        StringBuilder textType = new StringBuilder();
        //int page = SiteSession.MainCurrPage;
        string page = Convert.ToString(HttpContext.Current.Session["MainCurrPage"]);

        string attrName = "PDFmistake";
        bool containsPdfMistake = false;

        StringBuilder SparaType = new StringBuilder();
        StringBuilder SparaOrientation = new StringBuilder();
        StringBuilder SparaBackground = new StringBuilder();
        StringBuilder SparaSubType = new StringBuilder();
        bool IsSelectedErrorLine = false;

        Common comObj = new Common();
        XmlDocument xmlDoc = comObj.LoadXmlDocument(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));

        List<PdfJsLine> pdfJsLines = comObj.GetCurrentPdfJsPageLines(pdfJsPageText, Convert.ToInt32(page));
        int lineNum = 0;

        if (pdfJsLines != null)
        {
            if (pdfJsLines.Count > 0)
            {
                List<PdfJsLine> matchedLine = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Replace("\n", "").Equals(RemoveWhiteSpace(text).Replace("\n", ""))).ToList();

                if (matchedLine != null)
                {
                    if (matchedLine.Count > 0)
                    {
                        //text = matchedLine[0].Text;

                        if (matchedLine[0].LineNum > 0)
                        {
                            lineNum = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Replace("\n", "").Equals(RemoveWhiteSpace(text).Replace("\n", ""))).ToList()[0].LineNum;
                        }
                    }
                }
                //int lineNum = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(text))).ToList()[0].LineNum;

                ////int lineNum = GetSelectedLineNum(text, pdfJsPageText, Convert.ToString(page));

                XmlNodeList list_Lines = xmlDoc.SelectNodes(@"//ln [@page=" + page + "]");

                int lineCounter = 0;

                if ((list_Lines != null) && (list_Lines.Count > 0))
                {
                    int xmlLineCount = list_Lines.Count;
                    int pdfJsLinesCount = pdfJsLines.Count;

                    foreach (XmlNode ln in list_Lines)
                    {
                        lineCounter++;

                        //if (Regex.Replace(ln.InnerText, @"\s+", "").Equals(Regex.Replace(text, @"\s+", "")))
                        //if (Common.RemoveWhitespace(ln.InnerText).Equals(Common.RemoveWhitespace(text)))
                        //if (IsContainsSameText(ln.InnerText, text))

                        if (xmlLineCount != pdfJsLinesCount)
                        {
                            if (ln.ParentNode.Name.Equals("col") || ln.ParentNode.Name.Equals("head-col"))
                            {
                                double colUry = ln.Attributes["top"] != null
                                    ? Convert.ToDouble(ln.Attributes["top"].Value)
                                    : 0;
                                StringBuilder sbRowText = new StringBuilder();

                                if (ln.ParentNode.ParentNode != null)
                                {
                                    XmlNode rowNode = ln.ParentNode.ParentNode;

                                    var rowLines =
                                        rowNode.SelectNodes("//ln[@page='" + Convert.ToString(SiteSession.MainCurrPage) +
                                                            "']").Cast<XmlNode>()
                                            .Where(x => Convert.ToDouble(x.Attributes["top"].Value) == colUry)
                                            .ToList();

                                    if (rowLines != null && rowLines.Count >= 1)
                                    {
                                        foreach (XmlNode line in rowLines)
                                        {
                                            sbRowText.Append(line.InnerText + " ");
                                        }
                                    }
                                }

                                if (sbRowText.Length > 0)
                                {
                                    if (IsContainsSameText(Convert.ToString(sbRowText), text) &&
                                        (ln.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                                    {
                                        IsSelectedErrorLine = true;
                                    }
                                }
                            }
                            else
                            {
                                if (IsContainsSameText(ln.InnerText, text) && (ln.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                                {
                                    IsSelectedErrorLine = true;
                                }
                            }
                        }
                        else if (lineCounter == lineNum && xmlLineCount == pdfJsLinesCount)
                        {
                            IsSelectedErrorLine = true;
                        }

                        //if (lineCounter == lineNumber)
                        if (IsSelectedErrorLine)
                        {
                            IsSelectedErrorLine = false;
                            //if (lineCounter == lineNum)
                            //{
                            if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["ComparisonTask"])))
                            {
                                if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
                                {
                                    if (ln.Attributes != null && ln.Attributes.Count > 0)
                                    {
                                        foreach (XmlNode attr in ln.Attributes)
                                        {
                                            if (attr.Name.Equals(attrName) && !string.IsNullOrEmpty(attr.Value) &&
                                                !attr.Value.Equals("undo"))
                                            {
                                                containsPdfMistake = true;
                                                break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (ln.ParentNode.Name.Equals("section-title"))
                            {
                                if (ln.ParentNode.ParentNode.ParentNode != null)
                                {
                                    textType.Append(Convert.ToString(ln.ParentNode.ParentNode.ParentNode.Attributes["type"]) != "" ? (ln.ParentNode.ParentNode.ParentNode.Attributes["type"].Value) : null);
                                }
                            }
                            else
                            {
                                if (ln.ParentNode.Name.Equals("line"))
                                {
                                    if (ln.ParentNode.ParentNode.Name.Equals("spara"))
                                    {
                                        SparaType.Append(ln.ParentNode.ParentNode.Attributes["type"] == null ? null : ln.ParentNode.ParentNode.Attributes["type"].Value);
                                        SparaOrientation.Append(ln.ParentNode.ParentNode.Attributes["h-align"] == null ? null : ln.ParentNode.ParentNode.Attributes["h-align"].Value);
                                        SparaBackground.Append(ln.ParentNode.ParentNode.Attributes["bgcolor"] == null ? null : ln.ParentNode.ParentNode.Attributes["bgcolor"].Value);
                                        SparaSubType.Append(ln.ParentNode.ParentNode.ChildNodes[0].Name);

                                        //if (SparaType.ToString().Equals("other"))
                                        //    textType.Append("spara:" + SparaType + ":" + SparaSubType + ":" + SparaOrientation);
                                        //else
                                        //    textType.Append("spara:" + SparaType + ":" + SparaSubType);

                                        if (SparaType.ToString().Equals("other"))
                                            textType.Append("Special Para");
                                        else
                                            textType.Append("Special Para");
                                    }
                                }
                                else if (ln.ParentNode.Name.Equals("head-col") || ln.ParentNode.Name.Equals("col"))
                                {
                                    textType.Append("table");
                                }
                                else
                                {
                                    if (!(ln.ParentNode.Name.Equals("caption")))
                                    {
                                        textType.Append(ln.ParentNode.Name);
                                    }
                                }
                            }
                            break;
                        }
                    }
                }

                if (containsPdfMistake) return Convert.ToString(textType + ", true");

                else return Convert.ToString(textType);
            }
        }
        return null;
    }

    public string GetComments(string innerHtml, int page)
    {
        List<String> divText = new List<string>();

        var allDivs = innerHtml.Split(new string[] { "<div data-canvas-width" }, StringSplitOptions.None);

        allDivs = allDivs.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

        foreach (var div in allDivs)
        {
            var temp = div.Split(new string[] { ">" }, StringSplitOptions.None)[1].Split(new string[] { "</div" }, StringSplitOptions.None);
            divText.Add(temp[0].ToString().Replace("&nbsp;", " "));
        }

        PDFManipulation pdfMan = new PDFManipulation();
        var XMLTextList = pdfMan.GetXMLText_List(page);

        int count = 0;
        int lineCharLength = 0;
        int xmlCharLength = 0;

        string line_Char = "";
        string div_Char = "";
        string div_Count = "";
        StringBuilder sb_LineWithComments = new StringBuilder();

        List<String> div_AllLines = new List<string>();
        List<String> div_errorLines = new List<String>();

        for (int i = 0; i < XMLTextList.Count; i++)
        {
            if (XMLTextList[i].innerText != "")
            {
                line_Char = String.Join("", XMLTextList[i].innerText.Where(c => (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))));
                div_Char = "";
                div_Count = "";

                for (int j = count; j < divText.Count; j++)
                {
                    div_Char += String.Join("", divText[j].Where(c => (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))));
                    div_Count += (j + 1) + ",";

                    if (div_Char.Equals(line_Char))
                    {
                        if (XMLTextList[i].mistakeId != null)
                        {
                            div_errorLines.Add(XMLTextList[i].mistakeId + "," + div_Count);
                        }

                        div_AllLines.Add((i + 1) + "," + div_Count);
                        count = j + 1;
                        break;
                    }
                }
            }
        }

        List<String> mistakeComments_List = obj.GetQaMistake_Comments(Convert.ToString(HttpContext.Current.Session["LoginId"]), Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1", page);

        if ((mistakeComments_List != null) && (mistakeComments_List.Count > 0))
        {
            foreach (var comment in mistakeComments_List)
            {
                foreach (var item in div_errorLines)
                {
                    if (comment.Split(',')[0].Trim().Equals(item.Split(',')[0].Trim()))
                    {
                        sb_LineWithComments.Append((item.Remove(0, item.IndexOf(',') + 1)) + comment.Split(',')[1] + "~//~");
                    }
                }
            }
        }

        return sb_LineWithComments.ToString();
    }

    //public string GetComments(string innerHtml, int page)
    //{
    //    if (page < 0)
    //        return null;

    //    if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
    //    {
    //        return null;
    //    }

    //    List<String> divText = new List<string>();

    //    var allDivs = innerHtml.Split(new string[] { "~//~" }, StringSplitOptions.None);

    //    allDivs = allDivs.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

    //    if (allDivs.Length == 1)
    //        return null;

    //    foreach (var div in allDivs)
    //    {
    //        var temp = div.Split(new string[] { "," }, StringSplitOptions.None).Where(x => (!string.IsNullOrEmpty(x))).ToArray(); 
    //        divText.Add(temp[0]);
    //    }

    //    PDFManipulation pdfMan = new PDFManipulation();
    //    var XMLTextList = pdfMan.GetXMLText_List(page);

    //    int count = 0;
    //    int lineCharLength = 0;
    //    int xmlCharLength = 0;

    //    string line_Char = "";
    //    string div_Char = "";
    //    string div_Count = "";
    //    StringBuilder sb_LineWithComments = new StringBuilder();

    //    List<String> div_AllLines = new List<string>();
    //    List<String> div_errorLines = new List<String>();

    //    for (int i = 0; i < XMLTextList.Count; i++)
    //    {
    //        if (XMLTextList[i].innerText != "")
    //        {
    //            line_Char = String.Join("", XMLTextList[i].innerText.Where(c => !char.IsWhiteSpace(c)));
    //            div_Char = "";
    //            div_Count = "";

    //            for (int j = count; j < divText.Count; j++)
    //            {
    //                div_Char += String.Join("", divText[j].Where(c => !char.IsWhiteSpace(c)));
    //                div_Count += (j + 1) + ",";

    //                if (div_Char.Equals(line_Char))
    //                {
    //                    if (XMLTextList[i].mistakeId != null)
    //                    {
    //                        div_errorLines.Add(XMLTextList[i].mistakeId + "," + div_Count);
    //                    }

    //                    div_AllLines.Add((i + 1) + "," + div_Count);
    //                    count = j + 1;
    //                    break;
    //                }
    //            }
    //        }
    //    }

    //    List<String> mistakeComments_List = obj.GetQaMistake_Comments("2002", Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1", page);

    //    if ((mistakeComments_List != null) && (mistakeComments_List.Count > 0) && (mistakeComments_List.Count == div_errorLines.Count))
    //    {
    //        foreach (var comment in mistakeComments_List)
    //        {
    //            foreach (var item in div_errorLines)
    //            {
    //                if (comment.Split(',')[0].Trim().Equals(item.Split(',')[0].Trim()))
    //                {
    //                    sb_LineWithComments.Append((item.Remove(0, item.IndexOf(',') + 1)) + comment.Split(',')[1] + "~//~");
    //                }
    //            }
    //        }
    //    }

    //    return sb_LineWithComments.ToString();
    //}

    public string ShowComment(string text, int page)
    {
        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
        {
            return null;
        }

        int qaMistakeId = GetQaMistakeId_ByText(text, page);
        String comment = obj.ShowComment_ByError(Convert.ToString(HttpContext.Current.Session["LoginId"]), Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1", page, qaMistakeId);

        return comment;
    }

    public List<Mistakes> GetXMLText_List(int page)
    {
        if (page < 1)
            return null;

        List<Mistakes> list = new List<Mistakes>();

        StreamReader strreader = new StreamReader(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        XmlNodeList nodes = xmlDoc.SelectNodes(@"//*[@page=" + page + "]");
        //XmlNodeList nodes = xmlDoc.SelectNodes(@"//*[@PDFmistake!="" and @page=" + page + "]");

        if (nodes.Count > 0)
        {
            foreach (XmlElement node in nodes)
            {
                if (node.Attributes["QaMistakeId"] != null)
                    list.Add(new Mistakes { innerText = node.InnerText, mistakeId = Convert.ToString(node.Attributes["QaMistakeId"].Value) });

                else
                    list.Add(new Mistakes { innerText = node.InnerText });
            }
        }

        return list;
    }

    public int GetQaMistakeId_ByText(string text, int page)
    {
        if (page < 1 ||
            string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"])) ||
            File.Exists(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"])))
            return 0;

        int id = 0;
        string text_Char = "";
        string div_Char = "";

        StreamReader strreader = new StreamReader(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        XmlNodeList nodes = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @page=" + page + "]");

        if (nodes.Count > 0)
        {
            text_Char = String.Join("", text.Where(c => !char.IsWhiteSpace(c)));

            foreach (XmlElement node in nodes)
            {
                div_Char = String.Join("", node.InnerText.Where(c => !char.IsWhiteSpace(c)));

                if (text_Char.Equals(div_Char))
                {
                    id = node.Attributes["QaMistakeId"] != null ? Convert.ToInt32(node.Attributes["QaMistakeId"].Value) : 0;
                }
            }
        }

        return id;
    }

    private int GetTotalMistakes()
    {
        StreamReader strreader = new StreamReader(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        int totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistake]").Count;

        return totalMistakes;
    }

    /// <summary>
    /// Find all images paths from xml and calculates there height and width 
    /// </summary>
    /// <param name="path"></param>
    public void GetAllImages(string path)
    {
        int target_width = 0;
        int target_height = 0;

        string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);

        StreamReader strreader = new StreamReader(mainXmlPath);
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        //XmlNodeList nodes = xmlDoc.SelectNodes(@"//image/ln[@coord and @page=" + page + "]");
        XmlNodeList nodes = xmlDoc.SelectNodes(@"//image/ln[@coord]");

        if (nodes.Count > 0)
        {
            var images_Paths = nodes.Cast<XmlNode>()
                                .Select(node => new
                                {
                                    ImageId = Convert.ToInt32(node.ParentNode.Attributes["id"].Value),
                                    image_url = Convert.ToString(node.ParentNode.Attributes["image-url"].Value),
                                    page = Convert.ToInt32(node.Attributes["page"].Value),
                                    coord = Convert.ToString(node.Attributes["coord"].Value)
                                })
                                .ToList();

            foreach (var item in images_Paths)
            {
                string imgPath = path + item.image_url.Replace("Resources", "");

                var temp = item.coord.Split(':');

                if ((temp != null) && (temp.Length > 1))
                {
                    double width = Convert.ToDouble(temp[2]);
                    double height = Convert.ToDouble(temp[3]);

                    //System.Drawing.Image newImage = System.Drawing.Image.FromFile(imgPath);
                    //var dpi = newImage.HorizontalResolution;

                    //target_width = Math.Abs((Convert.ToInt32(width) * Convert.ToInt32(dpi)) / 72);
                    //target_height = Math.Abs((Convert.ToInt32(height) * Convert.ToInt32(dpi)) / 72);

                    //points = pixels * 72 / 96
                    //pixel = (points * 96)/72
                    //Converting points to pixel
                    target_width = Math.Abs((Convert.ToInt32(width) * 96) / 72);
                    target_height = Math.Abs((Convert.ToInt32(height) * 96) / 72);

                    //ResizeImage(imgPath, target_width, target_height);

                    System.Drawing.Image img = System.Drawing.Image.FromFile(path + item.image_url.Replace("Resources", ""));
                    img = resizeImage(img);
                    img.Save(path + "Resized_" + item.image_url.Replace("Resources", ""));
                }
            }
        }
    }

    private System.Drawing.Image resizeImage(System.Drawing.Image imgToResize, Size size)
    {
        //Get the image current width
        int sourceWidth = imgToResize.Width;
        //Get the image current height
        int sourceHeight = imgToResize.Height;

        float nPercent = 0;
        float nPercentW = 0;
        float nPercentH = 0;
        //Calulate  width with new desired size
        nPercentW = ((float)size.Width / (float)sourceWidth);
        //Calculate height with new desired size
        nPercentH = ((float)size.Height / (float)sourceHeight);


        if (nPercentH < nPercentW)
            nPercent = nPercentH;
        else
            nPercent = nPercentW;
        //New Width
        int destWidth = (int)(sourceWidth * nPercent);
        //New Height
        int destHeight = (int)(sourceHeight * nPercent);

        Bitmap b = new Bitmap(destWidth, destHeight);
        Graphics g = Graphics.FromImage((System.Drawing.Image)b);
        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
        // Draw image with new width and height
        g.DrawImage(imgToResize, 0, 0, destWidth, destHeight);
        g.Dispose();

        return (System.Drawing.Image)b;
    }

    private System.Drawing.Image resizeImage(System.Drawing.Image img)
    {
        Bitmap b = new Bitmap(img);
        System.Drawing.Image i = resizeImage(b, new Size(100, 100));
        return i;
    }

    ///// <summary>
    ///// Resize image according to its width and height in xml
    ///// </summary>
    ///// <param name="imgPath"></param>
    ///// <param name="target_width"></param>
    ///// <param name="target_height"></param>
    //public void ResizeImage(string imgPath, int target_width, int target_height)
    //{
    //    float aspectRatio = 0;

    //    System.Drawing.Image original_image = null;
    //    System.Drawing.Bitmap final_image = null;
    //    System.Drawing.Graphics graphic = null;

    //    try
    //    {
    //        if (imgPath != null)
    //        {
    //            // Retrieve the uploaded image
    //            original_image = System.Drawing.Image.FromStream(new MemoryStream(File.ReadAllBytes(imgPath)));
    //            // Calculate the new width and height
    //            int width__ = original_image.Width;
    //            int height__ = original_image.Height;
    //            int new_width = 0;
    //            int new_height = 0;

    //            float target_ratio = (float)target_width / (float)target_height;
    //            aspectRatio = (float)width__ / (float)height__;

    //            if (target_ratio > aspectRatio)
    //            {
    //                new_height = target_height;
    //                new_width = (int)Math.Floor(aspectRatio * (float)target_height);
    //            }
    //            else
    //            {
    //                new_height = (int)Math.Floor((float)target_width / aspectRatio);
    //                new_width = target_width;
    //            }

    //            new_width = new_width > target_width ? target_width : new_width;
    //            new_height = new_height > target_height ? target_height : new_height;

    //            final_image = new System.Drawing.Bitmap(new_width, new_height);
    //            graphic = System.Drawing.Graphics.FromImage(final_image);
    //            graphic.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Black), new System.Drawing.Rectangle(0, 0, new_width, new_height));
    //            int paste_x = 0;// (target_width - new_width) / 2;
    //            int paste_y = 0;// (target_height - new_height) / 2;
    //            graphic.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic; /* new way */
    //            graphic.DrawImage(original_image, paste_x, paste_y, new_width, new_height);

    //            final_image.Save(imgPath, System.Drawing.Imaging.ImageFormat.Jpeg);
    //        }
    //    }
    //    catch
    //    {
    //        //TODO: show err, image could not be uploaded
    //    }
    //    finally
    //    {
    //        // Clean up
    //        if (final_image != null) final_image.Dispose();
    //        if (graphic != null) graphic.Dispose();
    //        if (original_image != null) original_image.Dispose();
    //    }
    //}

    public List<String> GetAllImagesPath(string resourceFolderPath)
    {
        DirectoryInfo di = new DirectoryInfo(resourceFolderPath);
        FileInfo[] images = di.GetFiles();

        if (images.Length > 0)
        {
            return images.Select(f => f.FullName).ToList();
        }
        return null;
    }

    public List<Double> GetImage_Dimensions(int page)
    {
        StreamReader strreader = new StreamReader(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        XmlNodeList nodes = xmlDoc.SelectNodes(@"//image/ln[@coord and @page=" + page + "]");

        if (nodes.Count > 0)
        {
            var coordValues = nodes.Cast<XmlNode>()
                .Select(node => new { coord = Convert.ToString(node.Attributes["coord"].Value), pageNum = page })
                .ToList();
        }
        return null;
    }

    //public string GetSelectedTextFromProducedPage(string lineText, string innerHtml, int page)
    //{
    //    if (page < 0)
    //        return null;

    //    if (innerHtml.Equals(""))
    //    {
    //        return null;
    //    }
    //    List<String> divText = new List<string>();

    //    var allDivs = innerHtml.Split(new string[] { "<div data-canvas-width" }, StringSplitOptions.None);

    //    allDivs = allDivs.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

    //    foreach (var div in allDivs)
    //    {
    //        var temp = div.Split(new string[] { ">" }, StringSplitOptions.None)[1].Split(new string[] { "</div" },
    //                   StringSplitOptions.None).Where(x => (!string.IsNullOrEmpty(x))).ToArray();
    //        divText.Add(Convert.ToString(temp[0]).Replace("&nbsp;", " "));
    //    }

    //    //Get text lines from xml
    //    var XMLTextList = GetXMLText_List(page);

    //    if (XMLTextList == null)
    //        return "";

    //    int count = 0;
    //    int lineCharLength = 0;
    //    int xmlCharLength = 0;

    //    string line_Char = "";
    //    string div_Char = "";
    //    string div_Count = "";
    //    StringBuilder sb_LineWithComments = new StringBuilder();

    //    List<String> div_AllLines = new List<string>();
    //    string selectedLineDivs = "";

    //    string selectedLineText = String.Join("", lineText.Where(c => (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))));

    //    for (int i = 0; i < XMLTextList.Count; i++)
    //    {
    //        if (XMLTextList[i].innerText != "")
    //        {
    //            line_Char = String.Join("", XMLTextList[i].innerText.Where(c => (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))));
    //            div_Char = "";
    //            div_Count = "";

    //            for (int j = count; j < divText.Count; j++)
    //            {
    //                div_Char += String.Join("", divText[j].Where(c => (!char.IsWhiteSpace(c) && !char.IsPunctuation(c))));
    //                div_Count += (j) + ",";

    //                if (div_Char.Equals(line_Char))
    //                {
    //                    if (div_Char.Equals(selectedLineText))
    //                    {
    //                        selectedLineDivs = (div_Count);
    //                        break;
    //                    }

    //                    div_AllLines.Add((i + 1) + "," + div_Count);
    //                    count = j + 1;
    //                    break;
    //                }
    //            }
    //        }
    //    }

    //    return selectedLineDivs;
    //}

    public string GetSelectedTextFromProducedPage(string lineText, string innerHtml, int page)
    {
        if (page < 0)
            return null;

        if (innerHtml.Equals(""))
        {
            return null;
        }
        List<string> divText = new List<string>();

        List<string> allDivList = innerHtml.Split(new string[] { "<div data-canvas-width" }, StringSplitOptions.None).Where(x => !string.IsNullOrEmpty(x)).ToList();
        int topStartIndex = 0;
        int fontSizeStartIndex = 0;
        int nextTopStartIndex = 0;
        int nextFontSizeStartIndex = 0;

        int lineCounter = 0;
        string topValue = "";
        string nextTopValue = "";
        double topMarginValue = 1;

        List<PdfJsLine> pdfJsLines = new List<PdfJsLine>();
        StringBuilder sbDivText = new StringBuilder();
        StringBuilder sbDivNumber = new StringBuilder();

        for (int i = 0; i < allDivList.Count; i++)
        {
            topStartIndex = allDivList[i].IndexOf("top");
            fontSizeStartIndex = allDivList[i].IndexOf("font-size");

            if (topStartIndex != -1 && fontSizeStartIndex != -1)
            {
                topValue = allDivList[i].Substring(topStartIndex + 4, fontSizeStartIndex - topStartIndex - 4).Replace("px", "")
                                        .Replace(";", "").Trim();

                var tempText = allDivList[i].Split(new string[] { ">" }, StringSplitOptions.None)[1].Split(new string[] { "</div" },
                                                                StringSplitOptions.None).Where(x => (!string.IsNullOrEmpty(x))).ToArray();

                sbDivText.Append(Convert.ToString(tempText[0]).Replace("&nbsp;", "") + " ");
                sbDivNumber.Append(i + ",");

                if (!string.IsNullOrEmpty(topValue))
                {
                    if (i + 1 < allDivList.Count)
                    {
                        nextTopStartIndex = allDivList[i + 1].IndexOf("top");
                        nextFontSizeStartIndex = allDivList[i + 1].IndexOf("font-size");

                        if (nextTopStartIndex != -1 && nextFontSizeStartIndex != -1)
                        {
                            nextTopValue = allDivList[i + 1].Substring(nextTopStartIndex + 4, nextFontSizeStartIndex - nextTopStartIndex - 4)
                                                        .Replace("px", "").Replace(";", "").Trim();

                            if (Math.Abs(Convert.ToDouble(topValue) - Convert.ToDouble(nextTopValue)) > topMarginValue)
                            {
                                lineCounter++;

                                pdfJsLines.Add(new PdfJsLine
                                {
                                    Text = Convert.ToString(sbDivText),
                                    Top = topValue,
                                    LineNum = lineCounter,
                                    DivNum = Convert.ToString(sbDivNumber)
                                });

                                sbDivText.Length = 0;
                                sbDivNumber.Length = 0;
                                topValue = "";
                            }
                        }
                    }
                }
            }
        }//end for loop

        int lineNumber = 0;

        foreach (var line in pdfJsLines)
        {
            if (RemoveWhiteSpace(line.Text).Equals(RemoveWhiteSpace(lineText)))
            {
                lineNumber = line.LineNum;
                break;
            }
        }

        var selectedLineDivs = pdfJsLines.Where(x => x.LineNum == lineNumber).ToList()[0].DivNum;

        return selectedLineDivs;
    }

    string RemoveWhiteSpace(string input)
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
}
public enum HighlightType { Strike, Missing, Added }




using System;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Xml;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;

/// <summary>
/// Summary description for RHYWPDF
/// </summary>
public class RHYWManipulation
{
    string rhywFilePath;
    PdfCompareGlobalVar objGlobal = new PdfCompareGlobalVar();
    public RHYWManipulation()
    {
    }

    //public RHYWManipulation(string rhywFilePath)
    //{
    //    this.rhywFilePath = rhywFilePath;
    //    GlobalVar.XMLPath = rhywFilePath;
    //    SiteSession.xmlDoc = new XmlDocument();
    //    GlobalVar.LoadXml();
    //}

    private int totalPages;
    //public int TotalPages
    //{
    //    get
    //    {
    //        try
    //        {
    //            return GlobalVar.GetMaxPageNo();
    //        }
    //        catch
    //        {
    //            return -1;
    //        }
    //    }
    //}

    public bool LoadXMLDecrypt()
    {
        try
        {
            return true;
        }
        catch
        {
            return false;
        }
    }

    private string pageXMLSavedPath;
    /// <summary>
    /// The path of the current XML File which has been used to produce PDF
    /// </summary>
    public string CurrXMLFilePath
    {
        get
        {
            return pageXMLSavedPath;
        }
        set
        {
            pageXMLSavedPath = value;
        }
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

    /// <summary>
    /// Produces single page from the current rhywFile
    /// </summary>
    /// <param name="PageNum"></param>
    /// <returns>Produced File Path</returns>
    //public string ProduceSinglePage(int PageNum)
    //{
    //    string fileName = Path.GetFileNameWithoutExtension(this.rhywFilePath);
    //    XmlDocument pageXML = GlobalVar.GetPageXmlDoc(PageNum.ToString());
    //    string dirPath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
    //    //Random rnd = new Random();

    //    if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
    //    {
    //        pageXMLSavedPath = dirPath + "\\Tests\\" + Convert.ToString(HttpContext.Current.Session["CompTestUser_Email"]) +
    //                                     "/ComparisonTests/" + "\\Produced_" + PageNum + ".xml";
    //    }
    //    else
    //    {
    //        //pageXMLSavedPath = dirPath + fileName.Replace("-1", "") + "\\" + fileName + 
    //        //                   "\\Comparison\\Comparison-" + Convert.ToString(HttpContext.Current.Session["comparisonType"]) + "\\" + 
    //        //                   Convert.ToInt32(HttpContext.Current.Session["userId"]) + "\\Produced_" + PageNum + ".xml";

    //        pageXMLSavedPath = Common.GetTaskFiles_SavingPath() + "\\Produced_" + PageNum + ".xml";


    //        LogWritter.WriteLineInLog("In Produced Single Page :: pageXMLSavedPath -  ::" + pageXMLSavedPath);

    //        ///////////////////////////////////////////////////////////////////////////////////////////////////


    //    ////    double normalx = 0;
    //    ////    double normalIndent = 0;
    //    ////    double normalFont = Convert.ToDouble(HttpContext.Current.Session["normalFont"]);
    //    ////    List<XmlNode> abnormalFontLines = AbnormalFontCheck(pageXML, GlobalVar.PBPDocument);

    //    ////    normalx = Convert.ToDouble(HttpContext.Current.Session["normalx"]);
    //    ////    normalIndent = Convert.ToDouble(HttpContext.Current.Session["normalIndent"]);

    //    ////    XmlNodeList PageLines = pageXML.SelectNodes("//ln");

    //    ////    foreach (XmlNode line in PageLines)
    //    ////    {
    //    ////        if (line.Attributes["left"] != null)
    //    ////        {
    //    ////            double lineX = Math.Round(Convert.ToDouble(line.Attributes["left"].Value));
    //    ////            double diffr = Math.Abs(normalx - lineX);
    //    ////            if (diffr > 2)
    //    ////            {

    //    ////                if (line.NextSibling != null)
    //    ////                {
    //    ////                    XmlNode nextLine = line.NextSibling;

    //    ////                    if (nextLine.Name != "break")
    //    ////                    {
    //    ////                        lineX = Math.Round(Convert.ToDouble(nextLine.Attributes["left"].Value));

    //    ////                        diffr = Math.Abs(normalx - lineX);
    //    ////                        if (diffr > 2)
    //    ////                        {

    //    ////                            string coord = line.Attributes["coord"].Value;
    //    ////                            string page = line.Attributes["page"].Value;
    //    ////                            //page xml is not proper xml so original line is selected from Main XML in this case all sections are selected chapter, or level either not present on current page...
    //    ////                            XmlNode Actualline =
    //    ////                                GlobalVar.PBPDocument.SelectSingleNode("//ln[@coord='" + coord + "' and @page='" + page +
    //    ////                                                                       "']");
    //    ////                            if (line.ParentNode != null)
    //    ////                            {

    //    ////                                if (!line.ParentNode.Name.Equals("section-title"))
    //    ////                                {
    //    ////                                    if (Actualline != null && (!abnormalFontLines.Contains(Actualline)))
    //    ////                                    {
    //    ////                                        abnormalFontLines.Add(Actualline);
    //    ////                                        //if a single line is much indented and next lines are normal.. but whole para is not spara then all lines are selected here...
    //    ////                                        while (Actualline.NextSibling != null)
    //    ////                                        {
    //    ////                                            Actualline = Actualline.NextSibling;
    //    ////                                            if (!abnormalFontLines.Contains(Actualline))
    //    ////                                            {
    //    ////                                                abnormalFontLines.Add(Actualline);
    //    ////                                            }
    //    ////                                        }
    //    ////                                    }
    //    ////                                }
    //    ////                            }


    //    ////                            #region |Previous logic to check if it is in spara then ignore|

    //    ////                            //XmlNode LinePara = line.SelectSingleNode("ancestor::upara|ancestor::spara|ancestor::npara");
    //    ////                            //if (LinePara != null && (LinePara.Name.Equals("upara")))
    //    ////                            //{
    //    ////                            //    string coord = line.Attributes["coord"].Value;
    //    ////                            //    string page = line.Attributes["page"].Value;
    //    ////                            //    page xml is not proper xml so original line is selected from Main XML in this case all sections are selected chapter, or level either not present on current page...
    //    ////                            //    XmlNode Actualline = GlobalVar.PBPDocument.SelectSingleNode("//ln[@coord='" + coord + "' and @page='" + page + "']");
    //    ////                            //    if (Actualline != null && (!abnormalFontLines.Contains(Actualline)))
    //    ////                            //    {
    //    ////                            //        abnormalFontLines.Add(Actualline);
    //    ////                            //        if a single line is much indented and next lines are normal.. but whole para is not spara then all lines are selected here...
    //    ////                            //        while (Actualline.NextSibling != null)
    //    ////                            //        {
    //    ////                            //            Actualline = Actualline.NextSibling;
    //    ////                            //            if (!abnormalFontLines.Contains(Actualline))
    //    ////                            //            {
    //    ////                            //                abnormalFontLines.Add(Actualline);
    //    ////                            //            }
    //    ////                            //        }
    //    ////                            //    }
    //    ////                            //}

    //    ////                            #endregion
    //    ////                        }
    //    ////                    }
    //    ////                }
    //    ////                else
    //    ////                {
    //    ////                    diffr = Math.Abs(normalIndent - lineX);
    //    ////                    if (diffr > 1)
    //    ////                    {
    //    ////                        if (line.ParentNode != null)
    //    ////                        {

    //    ////                            if (!line.ParentNode.Name.Equals("section-title"))
    //    ////                            {
    //    ////                                string coord = line.Attributes["coord"].Value;
    //    ////                                string page = line.Attributes["page"].Value;
    //    ////                                //page xml is not proper xml so original line is selected from Main XML in this case all sections are selected chapter, or level either not present on current page...
    //    ////                                XmlNode Actualline =
    //    ////                                    GlobalVar.PBPDocument.SelectSingleNode("//ln[@coord='" + coord + "' and @page='" + page +
    //    ////                                                                           "']");
    //    ////                                if (Actualline != null && (!abnormalFontLines.Contains(Actualline)))
    //    ////                                {
    //    ////                                    abnormalFontLines.Add(Actualline);
    //    ////                                }
    //    ////                            }
    //    ////                        }
    //    ////                    }
    //    ////                }

    //    ////                #region |Commented|

    //    ////                //else if (diffr > 2)
    //    ////                //{
    //    ////                //    XmlNode LinePara = line.SelectSingleNode("ancestor::upara|ancestor::spara|ancestor::npara");
    //    ////                //    if (LinePara != null && (LinePara.Name.Equals("upara")))
    //    ////                //    {
    //    ////                //        string coord = line.Attributes["coord"].Value;
    //    ////                //        string page = line.Attributes["page"].Value;
    //    ////                //        //page xml is not proper xml so original line is selected from Main XML in this case all sections are selected chapter, or level either not present on current page...
    //    ////                //        XmlNode Actualline = mainDoc.SelectSingleNode("//ln[@coord='" + coord + "' and @page='" + page + "']");
    //    ////                //        if (Actualline!=null && (!abnormalFontLines.Contains(Actualline)))
    //    ////                //        {
    //    ////                //            abnormalFontLines.Add(Actualline);
    //    ////                //            //if a single line is much indented and next lines are normal.. but whole para is not spara then all lines are selected here...
    //    ////                //            while (Actualline.NextSibling != null)
    //    ////                //            {
    //    ////                //                Actualline = Actualline.NextSibling;
    //    ////                //                if (!abnormalFontLines.Contains(Actualline))
    //    ////                //                {
    //    ////                //                    abnormalFontLines.Add(Actualline);
    //    ////                //                }
    //    ////                //            }
    //    ////                //        }
    //    ////                //    }
    //    ////                //}

    //    ////                #endregion
    //    ////            }
    //    ////        }
    //    ////    }
    //    ////    foreach (XmlNode line in abnormalFontLines)
    //    ////    {
    //    ////        if (line.Attributes["coord"] != null)
    //    ////        {
    //    ////            string coord = line.Attributes["coord"].Value;
    //    ////            string page = line.Attributes["page"].Value;
    //    ////            XmlNode pageline = pageXML.SelectSingleNode("//ln[@coord='" + coord + "' and @page='" + page + "']");
    //    ////            if (pageline != null)
    //    ////            {
    //    ////                if (pageline.Attributes["left"] != null)
    //    ////                {
    //    ////                    pageline.Attributes["left"].Value = normalx.ToString();
    //    ////                    pageline.Attributes["fontsize"].Value = normalFont.ToString();

    //    ////                    //Create a new attribute
    //    ////                    XmlAttribute attr = pageXML.CreateAttribute("color");
    //    ////                    attr.Value = "1";
    //    ////                    pageline.Attributes.Append(attr);
    //    ////                }
    //    ////            }
    //    ////        }
    //    ////    }
    //    }
    //    ///////////////////////////////////////////////////////////////////////////////////////////////////////////
    //    pageXML.Save(pageXMLSavedPath);
    //    string prodFilePath = pageXMLSavedPath.TrimEnd(".xml".ToCharArray()) + ".pdf";
    //    LogWritter.WriteLineInLog("In Produced Single Page :: prodFilePath ::" + prodFilePath);

    //    if (File.Exists(prodFilePath))
    //    {
    //        if (IsFileLocked(new FileInfo(prodFilePath)))
    //            return null;

    //        File.Delete(prodFilePath);
    //    }

    //    //if (!File.Exists(prodFilePath))
    //    //{
    //    //ImageValidation.ImageValidationService imgValidator = new ImageValidation.ImageValidationService();
    //    try
    //    {
    //        LogWritter.WriteLineInLog("In Produced Single Page: pageXMLSavedPath::" + pageXMLSavedPath);
    //        LogWritter.WriteLineInLog("In Produced Single Page: prodFilePath::" + prodFilePath);
    //        //imgValidator.GenearatePDFPreview(xmlPath, prodFilePath);
    //        string result = GenearatePDFPreview(pageXMLSavedPath, prodFilePath);

    //        if (!result.Equals("Successfull"))
    //        {
    //            prodFilePath = "";
    //        }
    //        LogWritter.WriteLineInLog("generate output: " + result);
    //        //prodFilePath = AddAnnotationInPDF(prodFilePath, xmlPath);
    //    }
    //    finally
    //    {
    //        //imgValidator.Dispose();
    //    }
    //    //}
    //    return prodFilePath;
    //}


    /// <summary>
    /// Saves the loaded RHYW file as xml file on the path specified
    /// </summary>
    /// <param name="XmlSavePath"></param>
    /// <returns></returns>
    //public bool SaveCompleteXMLFile(string XmlSavePath)
    //{
    //    return GlobalVar.SaveXml(XmlSavePath);
    //}

    private string AddAnnotationInPDF(string pdfFilePath, string xmlFilePath)
    {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(xmlFilePath);
        XmlNodeList xmlElements = xmlDoc.SelectNodes("//upara|//spara|//npara|e|//table|//section");
        ArrayList annotations = new ArrayList();
        foreach (XmlNode xmlElement in xmlElements)
        {
            XmlNode lnNode = xmlElement.SelectSingleNode("ln");
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
            rhywAnnot.llx = llx;
            rhywAnnot.lly = lly;
            rhywAnnot.urx = urx;
            rhywAnnot.ury = ury;

            //WriteAnnotationInFile(pdfFilePath, 1, anotHeading, anotText, llx, lly, urx, ury);
            annotations.Add(rhywAnnot);
        }
        string annotedFilePath = WriteAnnotationsInFile(pdfFilePath, annotations);
        return annotedFilePath;
    }

    private string WriteAnnotationInFile(string pdfFilePath, int pageNum, string annotHeading, string annotText, float llx, float lly, float urx, float ury)
    {
        try
        {
            string origFile = pdfFilePath;
            string newFile = pdfFilePath + "_2.pdf";

            // open the reader
            PdfReader reader = new PdfReader(origFile);
            Rectangle size = reader.GetPageSizeWithRotation(pageNum);
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

    private string WriteAnnotationsInFile(string pdfFilePath, ArrayList annotations)
    {
        try
        {
            string origFile = pdfFilePath;
            string newFile = pdfFilePath + "_2.pdf";
            int pageNum = 1;
            // open the reader
            PdfReader reader = new PdfReader(origFile);
            Rectangle size = reader.GetPageSizeWithRotation(pageNum);
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
                document.Add(new Annotation(annotHeading, annotText, llx, lly, urx, ury));
            }
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

    public string GenearatePDFPreview(string srcXMLFile, string targetPDFPath)
    {
        string xslsPath = System.Configuration.ConfigurationManager.AppSettings["XSLPath"];
        //return ShowPdfPreview(srcXMLFile, targetPDFPath, "c:\\XEP\\xep.bat", @"C:\XSL\XSLS\PBPBook.xsl");
        return ShowPdfPreview(srcXMLFile, targetPDFPath, "c:\\XEP\\xep.bat", xslsPath);
    }

    //public void setDefaultPageSize()
    //{
    //    XmlDocument doc_Normal = new XmlDocument();
    //    doc_Normal.Load(ConfigurationManager.AppSettings["XSLPath"]);
    //    XmlNode root_Normal = doc_Normal.DocumentElement;
    //    XmlNamespaceManager nsmgr_Normal = new XmlNamespaceManager(root_Normal.NameTable);
    //    nsmgr_Normal.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");
    //    string pageWidth_Normal = "xsl:variable[@name=\"doc-page-width\"]";
    //    string pageHeight_Normal = "xsl:variable[@name=\"doc-page-height\"]";

    //    root_Normal.SelectSingleNode(pageWidth_Normal, nsmgr_Normal).Attributes["select"].Value = Convert.ToString("210");
    //    root_Normal.SelectSingleNode(pageHeight_Normal, nsmgr_Normal).Attributes["select"].Value = Convert.ToString("297");
    //    doc_Normal.Save(ConfigurationManager.AppSettings["XSLPath"]);

    //    XmlDocument doc_Coord = new XmlDocument();
    //    doc_Coord.Load(ConfigurationManager.AppSettings["XSLPathCoord"]);
    //    XmlNode root_Coord = doc_Coord.DocumentElement;
    //    XmlNamespaceManager nsmgr_Coord = new XmlNamespaceManager(doc_Coord.NameTable);
    //    nsmgr_Coord.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

    //    string pageWidth_Coord = "xsl:variable[@name=\"doc-page-width\"]";
    //    string pageHeight_Coord = "xsl:variable[@name=\"doc-page-height\"]";

    //    root_Coord.SelectSingleNode(pageWidth_Coord, nsmgr_Coord).Attributes["select"].Value = Convert.ToString("210");
    //    root_Coord.SelectSingleNode(pageHeight_Coord, nsmgr_Coord).Attributes["select"].Value = Convert.ToString("297");
    //    doc_Coord.Save(ConfigurationManager.AppSettings["XSLPathCoord"]);
    //}

    private string ShowPdfPreview(string xmlfile, string PdfFile, string xepfile, string xslfile)
    {
        string retMessage = "";
        try
        {
            if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]) != "")
            {
                if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
                {
                    HttpContext.Current.Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPathCoord"];
                }
                else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest"))
                {
                    HttpContext.Current.Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPathCoord"];
                }
                else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
                {
                    HttpContext.Current.Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPathCoord"];
                }
            }

            if (SiteSession.setDefaultXSL != null)
            {
                xslfile = SiteSession.setDefaultXSL;
            }

            string cmdStr = "-xml " + "\"" + xmlfile + "\"" + " -xsl " + "\"" + xslfile + "\"" + " -out " + "\"" + PdfFile + "\"";

            if (File.Exists(PdfFile))
            {
                File.Delete(PdfFile);
            }
            Process pPdfPreview = new Process();
            Process pPdfPreviewInPDF = new Process();

            //tells operating system not to use a shell;
            pPdfPreview.StartInfo.UseShellExecute = false;
            //allow me to capture stdout, i.e. results
            pPdfPreview.StartInfo.RedirectStandardOutput = true;
            //#my command arguments, i.e. what site to ping
            pPdfPreview.StartInfo.Arguments = cmdStr;
            //#the command to invoke under MSDOS
            pPdfPreview.StartInfo.FileName = xepfile;
            //#do not show MSDOS window
            pPdfPreview.StartInfo.CreateNoWindow = true;
            //#do it!
            bool bStarted = pPdfPreview.Start();
            while (!pPdfPreview.HasExited)
            {
                //Application.DoEvents();
                //System.Diagnostics.Debug.Write(".");
                LogWritter.WriteLineInLog("..");
            }
            pPdfPreview.WaitForExit();
            // Check if PDF size is greater than zero
            // IF-ELSE Block Added
            FileInfo PdfFileInfo = new FileInfo(PdfFile);
            if (File.Exists(PdfFile) && PdfFileInfo.Length > 0)
            {
                retMessage = "Successfull";
            }
            else
            {
                retMessage = "Command= " + cmdStr + " File Name=" + PdfFile + "  No PDF File found";
            }
        }
        catch (Exception ex)
        {
            retMessage = ex.Message;
        }
        return retMessage;
    }

    #region |Abnormal Font Check Region|

    public List<XmlNode> AbnormalFontCheck(XmlDocument pageXML, XmlDocument mainXML)
    {
        double normalFont = Convert.ToDouble(HttpContext.Current.Session["normalFont"]);
        string level1 = Convert.ToString(HttpContext.Current.Session["level1"]);
        string level2 = Convert.ToString(HttpContext.Current.Session["level2"]);
        string level3 = Convert.ToString(HttpContext.Current.Session["level3"]);
        string level4 = Convert.ToString(HttpContext.Current.Session["level4"]);
        double level1Font = 0;
        double level2Font = 0;
        double level3Font = 0;
        double level4Font = 0;

        if (level1 != "")
        {
            level1Font = Convert.ToDouble(level1);
        }
        if (level2 != "")
        {
            level2Font = Convert.ToDouble(level2);
        }
        if (level3 != "")
        {
            level3Font = Convert.ToDouble(level3);
        }
        if (level4 != "")
        {
            level4Font = Convert.ToDouble(level4);
        }

        XmlNodeList lines = pageXML.SelectNodes("//ln");
        List<XmlNode> effectedLines = new List<XmlNode>();
        foreach (XmlNode line in lines)
        {
            string coord = line.Attributes["coord"].Value;
            string page = line.Attributes["page"].Value;
            XmlNode Actualline = mainXML.SelectSingleNode("//ln[@coord='" + coord + "' and @page='" + page + "']");

            if (Actualline != null)
            {
                double lineFont = Convert.ToDouble(Actualline.Attributes["fontsize"].Value);

                if (lineFont != normalFont)
                {
                    //effectedLines.Add(Actualline);

                    #region |Previous Logic to check if it is in level or spara then ignore else add in effected lines|

                    XmlNode LinePara = Actualline.SelectSingleNode("ancestor::upara|ancestor::spara|ancestor::npara");
                    if (LinePara != null && (LinePara.Name.Equals("upara")))
                    {
                        if (Actualline.ParentNode.Name.Equals("section-title"))
                        {
                            XmlNode parentSection = Actualline.ParentNode.ParentNode.ParentNode;
                            if (parentSection.Name.Equals("section"))
                            {
                                if (parentSection != null &&
                                    (parentSection.Attributes["type"].Value.StartsWith("level")))
                                {
                                    string currentLevel = parentSection.Attributes["type"].Value;
                                    switch (currentLevel)
                                    {
                                        case "level1":
                                            if (lineFont != level1Font)
                                            {
                                                effectedLines.Add(Actualline);
                                            }
                                            break;
                                        case "level2":
                                            if (lineFont != level2Font)
                                            {
                                                effectedLines.Add(Actualline);
                                            }
                                            break;
                                        case "level3":
                                            if (lineFont != level3Font)
                                            {
                                                effectedLines.Add(Actualline);
                                            }
                                            break;
                                        case "level4":
                                            if (lineFont != level4Font)
                                            {
                                                effectedLines.Add(Actualline);
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                effectedLines.Add(Actualline);
                            }
                        }
                        else
                        {
                            effectedLines.Add(Actualline);
                        }
                    }

                    #endregion
                }
            }
        }
        return effectedLines;
    }

    //public string GetNormalFont(XmlDocument xmlDoc)
    //{
    //    int count = 0, k = 1;
    //    string normalTextSize = "", normalFontFamily = "";
    //    XmlNodeList Lines = xmlDoc.SelectNodes("//ln");

    //    foreach (XmlNode lnNode in Lines)
    //    {
    //        double tempSize = (lnNode != null) ? double.Parse(lnNode.Attributes["fontsize"].Value) : 0;
    //        string tempFamily = lnNode.Attributes["font"].Value;
    //        if (xmlDoc.SelectNodes("//ln[@fontsize=\"" + tempSize + "\"]").Count > count)
    //        {
    //            count = xmlDoc.SelectNodes("//ln[@fontsize=\"" + tempSize + "\"]").Count;
    //            normalTextSize = tempSize.ToString();
    //            normalFontFamily = tempFamily;
    //        }
    //        if (k < Convert.ToInt32(lnNode.Attributes["page"].Value))
    //        {
    //            k = Convert.ToInt32(lnNode.Attributes["page"].Value);
    //        }
    //        if (k == 11)
    //        {
    //            break;
    //        }
    //    }

    //    return normalTextSize;
    //}
    
    public string GetLevelFontSize(string level, XmlDocument xmlDoc)
    {
        int count = 0, k = 1;
        string LevelTextSize = "", LevelFontFamily = "";
        XmlNodeList SecitonTitleLines = xmlDoc.SelectNodes("//section[@type='" + level + "']/head/section-title/ln");
        foreach (XmlNode TitleLine in SecitonTitleLines)
        {
            double tempSize = (TitleLine != null) ? double.Parse(TitleLine.Attributes["fontsize"].Value) : 0;
            string tempFamily = TitleLine.Attributes["font"].Value;
            if (xmlDoc.SelectNodes("//section[@type='" + level + "']/head/section-title/ln[@fontsize=\"" + tempSize + "\"]").Count > count)
            {
                count = xmlDoc.SelectNodes("//Word[@fontsize=\"" + tempSize + "\"]").Count;
                LevelTextSize = tempSize.ToString();
                LevelFontFamily = tempFamily;
            }
        }
        return LevelTextSize;
    }

    //public void NormalAndIndentX_Old(XmlNode mainDoc, ref double NormalX, ref double NormalIndentX)
    //{
    //    Hashtable Occurence = new Hashtable();
    //    ArrayList x1Occuence = new ArrayList();
    //    XmlNodeList lineList = mainDoc.SelectNodes("//ln/@left");
    //    for (int n = 0; n < lineList.Count; n++)
    //    {
    //        if (lineList[n].Value.ToString().Contains("."))
    //        {
    //            double roundVal = double.Parse(lineList[n].Value.ToString()) + 0.01;
    //            lineList[n].Value = roundVal.ToString();
    //        }
    //    }
    //    string Xpath = "";
    //    if (lineList.Count > 0)
    //    {
    //        //Find out the unique X1 in Lines
    //        for (int n = 0; n < lineList.Count; n++)
    //        {
    //            Xpath = ".//ln[@left=\"" + lineList[n].Value + "\"]";
    //            int count = mainDoc.SelectNodes(Xpath).Count;
    //            string Entry = lineList[n].Value + "#" + count;
    //            if (!x1Occuence.Contains(lineList[n].Value))
    //            {
    //                x1Occuence.Add(lineList[n].Value);
    //                Occurence.Add(lineList[n].Value, count);
    //            }
    //        }
    //        //Find out the Normal X
    //        int max1 = int.Parse(Occurence[x1Occuence[0].ToString()].ToString());
    //        double firstVal = double.Parse(x1Occuence[0].ToString());

    //        int max2 = (x1Occuence.Count == 1) ? 0 : int.Parse(Occurence[x1Occuence[1].ToString()].ToString());

    //        double secondVal = (max2 == 0) ? 0 : double.Parse(x1Occuence[1].ToString());

    //        for (int r = 1; r < x1Occuence.Count; r++)
    //        {
    //            if (int.Parse(Occurence[x1Occuence[r].ToString()].ToString()) >= max1)
    //            {
    //                if (double.Parse(x1Occuence[r].ToString()) != firstVal)
    //                {
    //                    max2 = max1;
    //                    secondVal = firstVal;
    //                    max1 = int.Parse(Occurence[x1Occuence[r].ToString()].ToString());
    //                    firstVal = double.Parse(x1Occuence[r].ToString());
    //                }
    //            }
    //            else if (int.Parse(Occurence[x1Occuence[r].ToString()].ToString()) >= max2)
    //            {
    //                max2 = int.Parse(Occurence[x1Occuence[r].ToString()].ToString());
    //                secondVal = double.Parse(x1Occuence[r].ToString());

    //            }
    //        }

    //        if (firstVal > secondVal && firstVal != 0 && secondVal != 0)
    //        {
    //            NormalIndentX = firstVal;
    //            NormalX = secondVal;
    //        }
    //        else if (firstVal < secondVal && firstVal != 0 && secondVal != 0)
    //        {
    //            NormalIndentX = secondVal;
    //            NormalX = firstVal;
    //        }
    //        else
    //        {
    //            NormalX = firstVal;
    //        }
    //    }
    //    NormalX = Math.Floor(NormalX);
    //    NormalIndentX = Math.Floor(NormalIndentX);
    //}

    public string GetNormalFont(XmlDocument xmlDoc)
    {
        double normalTextSize = 0;
        XmlNodeList Lines = xmlDoc.SelectNodes("//ln");

        if (Lines.Count > 0)
        {
            var fontValues1 = Lines.Cast<XmlNode>().ToList();

            var fontValues2 = Lines.Cast<XmlNode>().Where(node => (node.Attributes != null)).ToList();

            //Convert xmlNodeList to list<Double>
            var fontValues = Lines.Cast<XmlNode>()
                .Select(node => (node.Attributes!=null && node.Attributes["fontsize"] != null ? Convert.ToDouble(node.Attributes["fontsize"].Value) : 0))
                .ToList();

            //Get fontsize values with total occurences 
            var q = fontValues.GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count).ToList();

            normalTextSize = Convert.ToDouble(q[0].Value);
        }

        return Convert.ToString(normalTextSize);
    }

    //public void NormalAndIndentX(XmlNode mainDoc, ref double NormalX, ref double NormalIndentX)
    //{
    //    XmlNodeList lineList = mainDoc.SelectNodes("//ln/@left");

    //    if (lineList.Count > 0)
    //    {
    //        //Convert xmlNodeList to list<string>
    //        var leftValues = lineList.Cast<XmlNode>()
    //            .Select(node => node.InnerText)
    //            .ToList();

    //        ////Find normal Indent x which is the maximum value
    //        //var list_IndentX = leftValues.GroupBy(x => x)
    //        //    .Select(g => new { Value = g.Key, Count = g.Count() })
    //        //    .OrderByDescending(x => x.Value).ToList();
    //        //NormalIndentX1 = Convert.ToDouble(list_IndentX[0].Value);

    //        //Find normal x which is the value with maximum occurences
    //        var list_NormalX = leftValues.GroupBy(x => x)
    //            .Select(g => new { Value = g.Key, Count = g.Count() })
    //            .OrderByDescending(x => x.Count).ToList();

    //        NormalIndentX = Convert.ToDouble(list_NormalX[0].Value);
    //        NormalX = Convert.ToDouble(list_NormalX[1].Value);
    //    }

    //    NormalX = Math.Floor(NormalX);
    //    NormalIndentX = Math.Floor(NormalIndentX);
    //}

    public void NormalAndIndentX(XmlNode mainDoc, ref double NormalX, ref double NormalIndentX)
    {
        XmlNodeList lineList = mainDoc.SelectNodes("//ln/@left");

        if (lineList.Count > 0)
        {
            //Convert xmlNodeList to list<string>
            var leftValues = lineList.Cast<XmlNode>()
                .Select(node => node.InnerText)
                .ToList();

            ////Find normal Indent x which is the maximum value
            //var list_IndentX = leftValues.GroupBy(x => x)
            //    .Select(g => new { Value = g.Key, Count = g.Count() })
            //    .OrderByDescending(x => x.Value).ToList();
            //NormalIndentX1 = Convert.ToDouble(list_IndentX[0].Value);

            //Find normal x which is the value with maximum occurences
            var list_NormalX = leftValues.GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count).ToList();

            NormalIndentX = Convert.ToDouble(list_NormalX[0].Value);
            NormalX = Convert.ToDouble(list_NormalX[1].Value);
        }

        NormalX = Math.Floor(NormalX);
        NormalIndentX = Math.Floor(NormalIndentX);
    }

    public void NormalAndIndentX_EvenPages(XmlNode mainDoc, ref double NormalX, ref double NormalIndentX)
    {
        NormalIndentX = 0;
        NormalX = 0;

        XmlNodeList lines = mainDoc.SelectNodes("//ln");

        var evenPageLines = lines.Cast<XmlNode>().Where(page => page.Attributes != null && Convert.ToInt32(page.Attributes["page"].Value) % 2 == 0)
           .OrderBy(x => Convert.ToInt32(x.Attributes["page"].Value)).ToList();

        if (evenPageLines.Count > 0)
        {
            var leftValues = evenPageLines.Cast<XmlNode>().Where(node => node.Attributes["left"] != null).Select(node => node.Attributes["left"].Value).ToList();

            //Find normal x (largest value with maximum occurences) and normal indent x (2nd largest value with maximum occurences).
            var list_NormalX = leftValues.GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count).ToList();

            NormalX = Math.Floor(Convert.ToDouble(list_NormalX[0].Value));
            NormalIndentX = Math.Floor(Convert.ToDouble(list_NormalX[1].Value));

            if (NormalX - NormalIndentX < 2)
            {
                double normalx = NormalX;
                var list_NormalIndentX = list_NormalX.Where(x => Math.Floor(Convert.ToDouble(x.Value)) > normalx).OrderByDescending(x => x.Count).ToList();
                NormalIndentX = Math.Floor(Convert.ToDouble(list_NormalIndentX[0].Value)); 
            }
        }
    }

    public void NormalAndIndentX_OddPages(XmlNode mainDoc, ref double NormalX, ref double NormalIndentX)
    {
        NormalIndentX = 0;
        NormalX = 0;

        XmlNodeList lines = mainDoc.SelectNodes("//ln");

        var oddPageLines = lines.Cast<XmlNode>().Where(page => page.Attributes != null && Convert.ToInt32(page.Attributes["page"].Value) % 2 == 1)
            .OrderBy(x=>Convert.ToInt32(x.Attributes["page"].Value)).ToList();

        if (oddPageLines.Count > 0)
        {
            //var leftValues = oddPageLines.Cast<XmlNode>().Select(node => node.Attributes["left"].Value).ToList();
            var leftValues = oddPageLines.Cast<XmlNode>().Where(node => node.Attributes["left"] != null).Select(node => node.Attributes["left"].Value).ToList();

            //Find normal x (largest value with maximum occurences) and normal indent x (2nd largest value with maximum occurences).
            var list_NormalX = leftValues.GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count).ToList();

            NormalX = Math.Floor(Convert.ToDouble(list_NormalX[0].Value));
            NormalIndentX = Math.Floor(Convert.ToDouble(list_NormalX[1].Value));

            if (NormalX - NormalIndentX < 2)
            {
                double normalx = NormalX;
                var list_NormalIndentX = list_NormalX.Where(x => Math.Floor(Convert.ToDouble(x.Value)) > normalx).OrderByDescending(x => x.Count).ToList();
                NormalIndentX = Math.Floor(Convert.ToDouble(list_NormalIndentX[0].Value));
            }
        }
    }

    #endregion
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;
using System.Web.UI.WebControls;
using EO.Web.Internal;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Xml;
using System.Diagnostics;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Text;
using Outsourcing_System;
using System.Data;
using iTextSharp.text.pdf.parser;
using System.Text.RegularExpressions;
using Outsourcing_System.PdfCompare_Classes;
using System.Collections;

/// <summary>
/// Summary description for Common
/// </summary>
public class Common
{
    public static string GetDirectoryPath()
    {
        return ConfigurationManager.AppSettings["MainDirPhyPath"];
    }

    public static string GetXSLSourcePath()
    {
        return ConfigurationManager.AppSettings["XSLFolder"];
    }

    public static string GetXSLDirectoryPath()
    {
        string email = "";
        if (Convert.ToString(HttpContext.Current.Session["Email"]) != "")
        {
            email = Convert.ToString(HttpContext.Current.Session["Email"]);
        }
        else
        {
            return "";
        }

        string xslFilePath = Common.GetDirectoryPath() + "User Files/" + email.Trim() + "/XSL/XSLS/PBPBook.xsl";

        if (!File.Exists(xslFilePath))
            return "";

        return xslFilePath;
    }

    public static string GetXSLCoordDirectoryPath()
    {
        string email = "";
        if (Convert.ToString(HttpContext.Current.Session["Email"]) != "")
        {
            email = Convert.ToString(HttpContext.Current.Session["Email"]);
        }
        else
        {
            return "";
        }

        string xslFilePath = Common.GetDirectoryPath() + "User Files/" + email.Trim() + "/XSL/XSLS_Coord/PBPBook.xsl";

        if (!File.Exists(xslFilePath))
            return "";

        return xslFilePath;
    }

    public static string GetXSLDirectoryPath_StartTest()
    {
        string email = "";
        if (Convert.ToString(HttpContext.Current.Session["Email"]) != "")
        {
            email = Convert.ToString(HttpContext.Current.Session["Email"]);
        }
        else
        {
            return "";
        }

        string xslFilePath = Common.GetDirectoryPath() + "User Files/Tests/" + email.Trim() + "/XSL/XSLS_Coord/PBPBook.xsl";

        if (!File.Exists(xslFilePath))
            return "";

        return xslFilePath;
    }

    //public static string GetTetDirectoryPath()
    //{
    //    string email = "";
    //    if (Convert.ToString(HttpContext.Current.Session["Email"]) != "")
    //    {
    //        email = Convert.ToString(HttpContext.Current.Session["Email"]);
    //    }
    //    else
    //    {
    //        return "";
    //    }

    //    string tetFilePath = Common.GetDirectoryPath() + "User Files/" + email.Trim() + "/XSL/tet.exe";

    //    return tetFilePath;
    //}

    public static string GetTaskFiles_InputFilesPath()
    {
        string path = GetDirectoryPath() + "/" + Convert.ToString(HttpContext.Current.Session["BookId"]) + "\\" +
                            Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1\\TaggingUntagged\\";
        return path;
    }

    public static string GetTaskFiles_SavingPath()
    {
        string bookId = Convert.ToString(HttpContext.Current.Session["BookId"]);
        string path = GetDirectoryPath() + "/" + bookId + "/" + bookId + "-1/Comparison/Comparison-" +
                      Convert.ToString(HttpContext.Current.Session["comparisonType"]) + "/" +
                      Convert.ToString(HttpContext.Current.Session["userId"]) + "/";
        return path;
    }

    public static string GetTaskFiles_ImagesPath()
    {
        string bookId = Convert.ToString(HttpContext.Current.Session["BookId"]);
        //string path = GetDirectoryPath() + bookId + "\\" + bookId + "-1\\Image\\" + bookId + "\\" + bookId + ".zip";
        string path = GetDirectoryPath() + bookId + "\\" + bookId + "-1\\Image\\";
        return path;
    }

    public static string GetTestFiles_InputFilesPath()
    {
        return GetDirectoryPath() + "\\Tests\\Original\\Comparison\\";
    }

    public static string GetTestFiles_SavingPath()
    {
        string path = GetDirectoryPath() + "\\Tests\\" + Convert.ToString(HttpContext.Current.Session["ComparisonTestUser_Email"]) + "/ComparisonTests/" +
                      System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["BookId"]));
        return path;
    }

    public static string GetComparisonEntryTestFiles_InputFilesPath()
    {
        return GetDirectoryPath() + "\\Tests\\Original\\Comparison\\";
    }

    public static string GetComparisonEntryTestFiles_SavingPath()
    {
        string path = GetDirectoryPath() + "\\Tests\\" + Convert.ToString(HttpContext.Current.Session["ComparisonTestUser_Email"]) + "/ComparisonTests/" +
                      Convert.ToString(HttpContext.Current.Session["TestName"]) + "/" + Convert.ToString(HttpContext.Current.Session["TestName"]) + "-1/Comparison/";
        return path;
    }

    public static string GetOnePageTestFiles_InputFilesPath()
    {
        return GetDirectoryPath() + "\\Tests\\Original\\Comparison\\";
    }

    public static string GetOnePageTestFiles_SavingPath()
    {
        string path = GetDirectoryPath() + "\\Tests\\" + Convert.ToString(HttpContext.Current.Session["ComparisonTestUser_Email"]) + "/ComparisonTests/" +
                      System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["BookId"]));
        return path;
    }

    public static string GetPdfPath(int currentPage, string pdfType)
    {
        string pdfPath = "";
        string normalSrcPdfPath = "";
        string normalPrdPdfPath = "";
        string stampedSrcPdfPath = "";
        string stampedPrdPdfPath = "";

        string comparisonTask = Convert.ToString(HttpContext.Current.Session["ComparisonTask"]);

        string normalSrcPdf = currentPage + ".pdf";
        string normalPrdPdf = "Produced_" + currentPage + ".pdf";
        string stampedSrcPdf = currentPage + "_Stamped.pdf";
        string stampedPrdPdf = "Produced_" + currentPage + "_Stamped.pdf";

        if (comparisonTask.Equals("test"))
        {
            normalSrcPdfPath = GetTestFiles_SavingPath() + "/" + normalSrcPdf;
            stampedSrcPdfPath = GetTestFiles_SavingPath() + "/" + stampedSrcPdf;
            normalPrdPdfPath = GetTestFiles_SavingPath() + "/" + normalPrdPdf;
            stampedPrdPdfPath = GetTestFiles_SavingPath() + "/" + stampedPrdPdf;
        }
        else if (comparisonTask.Equals("onepagetest"))
        {
            normalSrcPdfPath = GetOnePageTestFiles_SavingPath() + "/" + normalSrcPdf;
            stampedSrcPdfPath = GetOnePageTestFiles_SavingPath() + "/" + stampedSrcPdf;
            normalPrdPdfPath = GetOnePageTestFiles_SavingPath() + "/" + normalPrdPdf;
            stampedPrdPdfPath = GetOnePageTestFiles_SavingPath() + "/" + stampedPrdPdf;
        }
        else if (comparisonTask.Equals("task"))
        {
            normalSrcPdfPath = GetTaskFiles_SavingPath() + normalSrcPdf;
            stampedSrcPdfPath = GetTaskFiles_SavingPath() + stampedSrcPdf;
            normalPrdPdfPath = GetTaskFiles_SavingPath() + normalPrdPdf;
            stampedPrdPdfPath = GetTaskFiles_SavingPath() + stampedPrdPdf;
        }
        else if (comparisonTask.Equals("comparisonEntryTest") ||
                    comparisonTask.Equals("CompUpgradedSampleTest") ||
                    comparisonTask.Equals("CompUpgradedStartTest"))
        {
            normalSrcPdfPath = GetComparisonEntryTestFiles_SavingPath() + normalSrcPdf;
            stampedSrcPdfPath = GetComparisonEntryTestFiles_SavingPath() + stampedSrcPdf;
            normalPrdPdfPath = GetComparisonEntryTestFiles_SavingPath() + normalPrdPdf;
            stampedPrdPdfPath = GetComparisonEntryTestFiles_SavingPath() + stampedPrdPdf;
        }

        if (pdfType.Equals("src"))
        {
            if (System.IO.File.Exists(stampedSrcPdfPath))
            {
                pdfPath = stampedSrcPdfPath;
            }
            else
            {
                pdfPath = normalSrcPdfPath;
            }
        }
        else if (pdfType.Equals("prd"))
        {
            if (System.IO.File.Exists(stampedPrdPdfPath))
            {
                pdfPath = stampedPrdPdfPath;
            }
            else
            {
                pdfPath = normalPrdPdfPath;
            }
        }

        return pdfPath;
    }

    private static bool IsContainsSameText(string paraLine, string tetmlLine)
    {
        if (string.IsNullOrEmpty(paraLine) || string.IsNullOrEmpty(tetmlLine)) return false;

        List<string> pdfJsLineTempList = Regex.Split(paraLine, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
        List<string> xmlLineTempList = Regex.Split(tetmlLine, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

        string pdfJsText = Regex.Replace(paraLine, "[^A-Za-z0-9]", "");
        string xmlText = Regex.Replace(tetmlLine, "[^A-Za-z0-9]", "");

        //if (!string.IsNullOrEmpty(paraLine) && !string.IsNullOrEmpty(tetmlLine))
        //{
        //    int matchingPer = GetMatchingPercentage(pdfJsLineTempList[0].Trim(), xmlLineTempList[0].Trim());
        //    if (matchingPer < 50)
        //        return false;
        //}

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

    //2017-10-05 working
    //public static bool IsContainsSameText(string xmlText, string selectedText, string pageNum)
    //{
    //    xmlText = xmlText.Trim().Replace("…", "").Replace(".", "");
    //    selectedText = selectedText.Trim().Replace("…", "").Replace(".", "");

    //    var splittedXmlText = Regex.Split(xmlText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToArray();
    //    var splittedSelectedText = Regex.Split(selectedText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToArray();

    //    if (splittedXmlText != null && splittedSelectedText != null)
    //    {
    //        if (splittedXmlText.Length > 0 && splittedSelectedText.Length > 0)
    //        {
    //            if (splittedXmlText.Length == 1 && splittedSelectedText.Length == 1)
    //            {
    //                if (splittedXmlText[0].Equals(splittedSelectedText[0]))
    //                    return true;
    //            }
    //            else
    //            {
    //                if (CheckSameLenWords(splittedXmlText, splittedSelectedText))
    //                {
    //                    //if (splittedXmlText.Length == 1 && splittedSelectedText.Length == 1)
    //                    //{
    //                    //    if (splittedXmlText[0].Equals(splittedSelectedText[0]))
    //                    //        return true;
    //                    //}
    //                    if (splittedXmlText.Length == 2 && splittedSelectedText.Length == 2)
    //                    {
    //                        if (splittedXmlText[0].Equals(splittedSelectedText[0]))
    //                        {
    //                            if (splittedXmlText[1].Equals(splittedSelectedText[1]))
    //                                return true;
    //                        }
    //                    }
    //                    else if (splittedXmlText.Length > 2 && splittedSelectedText.Length > 2)
    //                    {
    //                        if (splittedXmlText[0].Equals(splittedSelectedText[0]))
    //                        {
    //                            if (splittedXmlText[1].Equals(splittedSelectedText[1]))
    //                            {
    //                                if (splittedXmlText[2].Equals(splittedSelectedText[2]))
    //                                    return true;
    //                            }
    //                        }
    //                    }
    //                }
    //                else
    //                {
    //                    string xmlTextWithoutSpace = RemoveWhiteSpace(xmlText);
    //                    string selectedTextWithoutSpace = RemoveWhiteSpace(selectedText);

    //                    int xmlTextLength = xmlTextWithoutSpace.Length;
    //                    int pdfJsTextLength = selectedTextWithoutSpace.Length;

    //                    if (xmlTextLength < 2 || pdfJsTextLength < 3)
    //                        return false;

    //                    StringBuilder sbWords = new StringBuilder();

    //                    if (xmlTextLength == pdfJsTextLength)
    //                    {
    //                        if (xmlTextWithoutSpace.Equals(selectedTextWithoutSpace))
    //                            return true;
    //                    }
    //                    else if (xmlTextLength < pdfJsTextLength)
    //                    {
    //                        sbWords.Append(selectedTextWithoutSpace.Substring(0, xmlTextLength));
    //                        if (xmlTextWithoutSpace.Equals(RemoveWhiteSpace(Convert.ToString(sbWords))))
    //                            return true;
    //                    }
    //                    else if (pdfJsTextLength < xmlTextLength)
    //                    {
    //                        sbWords.Append(xmlTextWithoutSpace.Substring(0, pdfJsTextLength));
    //                        if (selectedTextWithoutSpace.Equals(RemoveWhiteSpace(Convert.ToString(sbWords))))
    //                            return true;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    return false;
    //}

    public static string GetTableRowLine(XmlNode tableLineNode, string selectedText, string pageNum)
    {
        string colUry = tableLineNode.Attributes["top"] != null ? tableLineNode.Attributes["top"].Value : "";
        StringBuilder line = new StringBuilder();

        if (tableLineNode.ParentNode.ParentNode != null)
        {
            XmlNode rowNode = tableLineNode.ParentNode.ParentNode;
            var rowLines = rowNode.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes["top"].Value == colUry).ToList();

            if (rowLines != null)
            {
                foreach (XmlNode ln in rowLines)
                {
                    line.Append(ln.InnerText + " ");
                }
            }
        }
        return Convert.ToString(line);
    }

    public static bool CheckSameLenWords(string[] xmlText, string[] pdfJsText)
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


    //public static void LogMistakesInXml(string comments, string lineText)
    //{
    //    string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);

    //    if (String.IsNullOrEmpty(mainXmlPath))
    //        return;

    //    MyDBClass obj = new MyDBClass();
    //    Common comObj = new Common();

    //    XmlDocument xmlDocOrigXml = Common.LoadXmlDocument(mainXmlPath);

    //    string attrName = "PDFmistake";

    //    string quizType = Convert.ToString(HttpContext.Current.Session["quizType"]);

    //    if (quizType != "")
    //    {
    //        if (quizType.Equals("Splitting"))
    //        {
    //            attrName = "PDFMergemistake";
    //        }
    //        else if (quizType.Equals("Merging"))
    //        {
    //            attrName = "PDFSplitmistake";
    //        }
    //        else if (quizType.Equals("Space"))
    //        {
    //            attrName = "PDFmistake";
    //        }
    //    }

    //    List<string> selectedText = new List<string>();
    //    string TextFromPDFJs = lineText;  //The original text that was selected for editing

    //    HttpContext.Current.Session["OriginalText"] = TextFromPDFJs;

    //    if (TextFromPDFJs == "")
    //        return;

    //    var selectedLines = TextFromPDFJs.Split(new string[] { "\r\n" }, StringSplitOptions.None);

    //    selectedLines = selectedLines.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

    //    try
    //    {


    //        if ((selectedLines != null) && (selectedLines.Length > 0) && (xmlDocOrigXml != null))
    //        {
    //            //string attrName = "PDFmistake";
    //            string attrNameTest = "PDFmistakeTest";
    //            int mistakeNum = 0;
    //            bool check = false;
    //            bool isMistakeRemoved = false;
    //            bool nonInjectedErrors_Check = true;
    //            string pageNum = "";

    //            foreach (var line in selectedLines)
    //            {
    //                check = false;

    //                XmlNodeList nodes = xmlDocOrigXml.SelectNodes("//ln");

    //                XmlElement root = xmlDocOrigXml.DocumentElement;

    //                foreach (XmlElement node in nodes)
    //                {
    //                    mistakeNum = 0;

    //                    //if ((RemoveWhitespace(node.InnerText).Equals(RemoveWhitespace(line))) && (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    //                    if (IsContainsSameText(node.InnerText, line) && (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    //                    {
    //                        pageNum = node.Attributes["page"].Value;

    //                        //If task is test or one page test then log attrNameTest attribute and clear PDFmistake attribute value
    //                        if ((Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test")) ||
    //                            (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest")) ||
    //                            (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("comparisonEntryTest")))
    //                        {
    //                            if (node.HasAttribute(attrName))
    //                            {
    //                                node.SetAttribute(attrName, "");
    //                            }

    //                            if (!node.HasAttribute(attrNameTest))
    //                            {
    //                                XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrNameTest);

    //                                newAttr.Value = Convert.ToString("1");
    //                                node.SetAttributeNode(newAttr);
    //                            }

    //                            check = false;
    //                        }

    //                        //In comparison task, lines which don't have correction/conversion tag and user log errors on these line then correction/conversion
    //                        //tag is inserted into a new xml and this xml is send for qa inspection in structuring tool.
    //                        else if (!node.HasAttribute(attrName) || (node.HasAttribute(attrName) && node.Attributes["PDFmistake"].Value.Equals("undo")))
    //                        {
    //                            if (node.Attributes["PDFmistake"] != null && node.Attributes["PDFmistake"].Value.Equals("undo"))
    //                            {
    //                                // Remove the PDFmistake attribute.
    //                                node.RemoveAttribute("PDFmistake");
    //                            }

    //                            XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrName);

    //                            if (comObj.GetTotalMistakes() == 0)
    //                                mistakeNum = 1;
    //                            else
    //                                mistakeNum = comObj.GetTotalMistakes() + 1;

    //                            newAttr.Value = Convert.ToString(mistakeNum);
    //                            node.SetAttributeNode(newAttr);

    //                            if (node.Attributes["autoInjection"] != null)
    //                            {
    //                                node.RemoveAttribute("autoInjection");
    //                                nonInjectedErrors_Check = false;
    //                            }

    //                            if (node.Attributes["PDFMergemistake"] != null)
    //                            {
    //                                node.SetAttribute("PDFMergemistake", "");
    //                                nonInjectedErrors_Check = false;
    //                            }

    //                            if (node.Attributes["PDFSplitmistake"] != null)
    //                            {
    //                                node.SetAttribute("PDFSplitmistake", "");
    //                                nonInjectedErrors_Check = false;
    //                            }

    //                            //correction="merge,"

    //                            if (node.Attributes["correction"] != null)
    //                            {
    //                                node.SetAttribute("correction", "");
    //                                nonInjectedErrors_Check = false;
    //                            }

    //                            //if (node.ParentNode.Attributes["correction"] != null)
    //                            //{
    //                            //    node.ParentNode.Attributes["correction"].Value = "";
    //                            //    nonInjectedErrors_Check = false;
    //                            //}

    //                            //Removing split mistake attributes from both paras
    //                            if (node.ParentNode.Attributes["correction"] != null)
    //                            {
    //                                isMistakeRemoved = false;

    //                                //Removing split mistake attributes (correction="merge,") from first para
    //                                if (node.ParentNode.Attributes["correction"].Value.Equals("merge,"))
    //                                {
    //                                    node.ParentNode.Attributes["correction"].Value = "";
    //                                }

    //                                //Remove merge tag from previous para
    //                                if (node.ParentNode.PreviousSibling != null && node.ParentNode.PreviousSibling.Attributes["correction"] != null && !isMistakeRemoved)
    //                                {
    //                                    if (node.ParentNode.PreviousSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                    {
    //                                        node.ParentNode.PreviousSibling.Attributes["correction"].Value = "";
    //                                        isMistakeRemoved = true;
    //                                    }
    //                                }

    //                                //Remove merge tag from next para
    //                                if (node.ParentNode.NextSibling != null && node.ParentNode.NextSibling.Attributes["correction"] != null && !isMistakeRemoved)
    //                                {
    //                                    if (node.ParentNode.NextSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                    {
    //                                        node.ParentNode.NextSibling.Attributes["correction"].Value = "";
    //                                    }
    //                                }

    //                                nonInjectedErrors_Check = false;
    //                            }

    //                            if (node.Attributes["conversion"] != null)
    //                            {
    //                                node.SetAttribute("conversion", "");
    //                                nonInjectedErrors_Check = false;
    //                            }

    //                            if (node.Attributes["missing"] != null)
    //                            {
    //                                node.SetAttribute("missing", "");
    //                                nonInjectedErrors_Check = false;
    //                            }

    //                            check = true;

    //                            //If error is not of injected type then log correction/conversion attribute in a new xml which is saved before mistake injection task.
    //                            if (nonInjectedErrors_Check)
    //                            {
    //                                LogMistakesInNewXml(node.InnerText);
    //                            }
    //                        }

    //                        //update existing error comments
    //                        else
    //                        {
    //                            if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    //                            {
    //                                if (!string.IsNullOrEmpty(comments.Trim()) && !string.IsNullOrEmpty(node.Attributes["PDFmistake"].Value))
    //                                {
    //                                    string updateId = obj.InsertQaMistakes(Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1",
    //                                            Convert.ToInt32(HttpContext.Current.Session["userId"]), 1,
    //                                            Convert.ToInt32(node.Attributes["PDFmistake"].Value),
    //                                            DateTime.Now, Convert.ToInt32(pageNum), comments.Trim());
    //                                }
    //                            }
    //                        }
    //                    }

    //                    if (check)
    //                    {
    //                        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    //                        {
    //                            string Id = obj.InsertQaMistakes(Convert.ToString((HttpContext.Current.Session["BookId"])) + "-1",
    //                                    Convert.ToInt32((HttpContext.Current.Session["userId"])), 1, mistakeNum,
    //                                    DateTime.Now, Convert.ToInt32(pageNum), comments.Trim());

    //                            XmlAttribute qaMistakeId = xmlDocOrigXml.CreateAttribute("QaMistakeId");
    //                            qaMistakeId.Value = Convert.ToString(Id);
    //                            node.SetAttributeNode(qaMistakeId);
    //                            break;
    //                        }
    //                    }
    //                }

    //                xmlDocOrigXml.Save(mainXmlPath);

    //                GlobalVar objGlobal = new GlobalVar();
    //                objGlobal.PBPDocument = xmlDocOrigXml;
    //                objGlobal.XMLPath = mainXmlPath.Replace(".xml", ".rhyw");
    //                objGlobal.SaveXml();
    //                ////XmlDocument xmlDoc = new XmlDocument(); aamir
    //                ////xmlDoc = objGlobal.PBPDocument;
    //            }
    //        }

    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }

    //    int detectedInjMistakes = 0;
    //    int detectedOtherMistakes = 0;

    //    string temp = comObj.GetTotalMistakesAll();

    //    string[] listMistakes = temp.Split(',');

    //    if (listMistakes != null && listMistakes.Length > 1)
    //    {
    //        detectedOtherMistakes = listMistakes[0] == "" ? 0 : Convert.ToInt32(listMistakes[0]);
    //        detectedInjMistakes = listMistakes[1] == "" ? 0 : Convert.ToInt32(listMistakes[1]);
    //    }

    //    if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    //    {
    //        //Update mistake count
    //        MyDBClass db = new MyDBClass();
    //        db.UpdateMistakeCount(Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1", Convert.ToInt32(HttpContext.Current.Session["userId"]),
    //                              detectedInjMistakes, detectedOtherMistakes);
    //    }

    //    HttpContext.Current.Session["mistakeCounter"] = comObj.GetTotalMistakes();
    //    HttpContext.Current.Session["SelectedMistakeText"] = "";
    //}

    public static string GetSelectedLineNum(string lineText, string innerHtml, string page, out int lineNum)
    {
        lineNum = 0;

        if (string.IsNullOrEmpty(page)) return "";

        if (innerHtml.Equals(""))
        {
            return "";
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

        foreach (var line in pdfJsLines)
        {
            if (RemoveWhiteSpace(line.Text).Equals(RemoveWhiteSpace(lineText)))
            {
                lineNum = line.LineNum;
                break;
            }
        }

        int lnNumber = lineNum;

        lineNum = pdfJsLines.Where(x => x.LineNum == lnNumber).ToList()[0].LineNum;

        return pdfJsLines.Where(x => x.LineNum == lnNumber).ToList()[0].DivNum;
    }

    //Get PdfJs all lines from a produced pdf page
    public List<PdfJsLine> GetPdfJsPageLines(string innerHtml, string page)
    {
        if (string.IsNullOrEmpty(page) || string.IsNullOrEmpty(innerHtml)) return null;

        List<string> allDivList = innerHtml.Split(new string[] { "<div data-canvas-width" }, StringSplitOptions.None).Where(x => !string.IsNullOrEmpty(x)).ToList();
        int topStartIndex = 0;
        int fontSizeStartIndex = 0;
        int nextTopStartIndex = 0;
        int nextFontSizeStartIndex = 0;

        int lineCounter = 0;
        string topValue = "";
        string nextTopValue = "";
        double topMarginValue = 3;

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
                                    DivNum = Convert.ToString(sbDivNumber),
                                    PageNum = Convert.ToInt32(page)
                                });

                                sbDivText.Length = 0;
                                sbDivNumber.Length = 0;
                                topValue = "";
                            }
                        }
                    }
                    else if (i == allDivList.Count - 1)
                    {
                        nextTopStartIndex = allDivList[i].IndexOf("top");
                        nextFontSizeStartIndex = allDivList[i].IndexOf("font-size");

                        if (nextTopStartIndex != -1 && nextFontSizeStartIndex != -1)
                        {
                            nextTopValue = allDivList[i].Substring(nextTopStartIndex + 4, nextFontSizeStartIndex - nextTopStartIndex - 4)
                                                        .Replace("px", "").Replace(";", "").Trim();

                            lineCounter++;

                            pdfJsLines.Add(new PdfJsLine
                            {
                                Text = Convert.ToString(sbDivText),
                                Top = topValue,
                                LineNum = lineCounter,
                                DivNum = Convert.ToString(sbDivNumber),
                                PageNum = Convert.ToInt32(page)
                            });

                            sbDivText.Length = 0;
                            sbDivNumber.Length = 0;
                            topValue = "";
                        }
                    }
                }
            }
        }//end for loop

        return pdfJsLines;
    }

    //public List<PdfJsLine> GetCurrentPdfJsPageLines(string pdfJsPageText, int currentPage)
    //{
    //    List<PdfJsLine> pdfJsLines = null;
    //    Common comObj = null;

    //    try
    //    {
    //        if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["pdfJsPageLines"])))
    //        {
    //            pdfJsLines = (List<PdfJsLine>)(HttpContext.Current.Session["pdfJsPageLines"]);

    //            if (pdfJsLines != null)
    //            {
    //                if (pdfJsLines.Count > 0)
    //                {
    //                    if (pdfJsLines[0].PageNum != currentPage)
    //                    {
    //                        comObj = new Common();
    //                        pdfJsLines = comObj.GetPdfJsPageLines(pdfJsPageText, Convert.ToString(currentPage));
    //                        HttpContext.Current.Session["pdfJsPageLines"] = pdfJsLines;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            comObj = new Common();
    //            pdfJsLines = comObj.GetPdfJsPageLines(pdfJsPageText, Convert.ToString(currentPage));
    //            HttpContext.Current.Session["pdfJsPageLines"] = pdfJsLines;
    //        }

    //        return pdfJsLines;
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //}

    public List<PdfJsLine> GetCurrentPdfJsPageLines(string pdfJsPageText, int currentPage)
    {
        List<PdfJsLine> pdfJsLines = null;
        Common comObj = null;

        try
        {
            //if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["pdfJsPageLines"])))
            //{
            //    pdfJsLines = (List<PdfJsLine>)(HttpContext.Current.Session["pdfJsPageLines"]);

            //    if (pdfJsLines != null)
            //    {
            //        if (pdfJsLines.Count > 0)
            //        {
            //            if (pdfJsLines[0].PageNum != currentPage)
            //            {
            //                comObj = new Common();
            //                pdfJsLines = comObj.GetPdfJsPageLines(pdfJsPageText, Convert.ToString(currentPage));
            //                HttpContext.Current.Session["pdfJsPageLines"] = pdfJsLines;
            //            }
            //        }
            //    }
            //}
            //else
            //{
            comObj = new Common();
            pdfJsLines = comObj.GetPdfJsPageLines(pdfJsPageText, Convert.ToString(currentPage));
            HttpContext.Current.Session["pdfJsPageLines"] = pdfJsLines;
            //}

            return pdfJsLines;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    //public static string LogMistakesInXml(string comments, string lineText, string pdfJsPageText)
    //{
    //    string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);

    //    if (String.IsNullOrEmpty(mainXmlPath))
    //        return "";

    //    MyDBClass obj = new MyDBClass();
    //    Common comObj = new Common();

    //    string pdfJsDivNumbers = string.Empty;

    //    XmlDocument xmlDocOrigXml = comObj.LoadXmlDocument(mainXmlPath);

    //    string attrName = "PDFmistake";

    //    string quizType = Convert.ToString(HttpContext.Current.Session["quizType"]);

    //    if (quizType != "")
    //    {
    //        if (quizType.Equals("Splitting"))
    //        {
    //            attrName = "PDFMergemistake";
    //        }
    //        else if (quizType.Equals("Merging"))
    //        {
    //            attrName = "PDFSplitmistake";
    //        }
    //        else if (quizType.Equals("Space"))
    //        {
    //            attrName = "PDFmistake";
    //        }
    //    }

    //    List<string> selectedText = new List<string>();
    //    string TextFromPDFJs = lineText;  //The original text that was selected for editing

    //    HttpContext.Current.Session["OriginalText"] = TextFromPDFJs;

    //    if (TextFromPDFJs == "")
    //        return "";

    //    var selectedLines = TextFromPDFJs.Split(new string[] { "\r\n" }, StringSplitOptions.None);

    //    selectedLines = selectedLines.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

    //    try
    //    {
    //        if ((selectedLines != null) && (selectedLines.Length > 0) && (xmlDocOrigXml != null))
    //        {
    //            //string attrName = "PDFmistake";
    //            string attrNameTest = "PDFmistakeTest";
    //            int mistakeNum = 0;
    //            bool check = false;
    //            bool isMistakeRemoved = false;
    //            bool nonInjectedErrors_Check = true;
    //            string pageNum = "";
    //            int lineCounter = 0;
    //            bool IsSelectedErrorLine = false;
    //            int lineNumber = 0;

    //            List<PdfJsLine> pdfJsLines = comObj.GetCurrentPdfJsPageLines(pdfJsPageText, Convert.ToInt32(SiteSession.MainCurrPage));
    //            if (pdfJsLines != null)
    //            {
    //                if (pdfJsLines.Count > 0)
    //                {
    //                    List<PdfJsLine> selectedPdfJsline = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(lineText))).ToList();
    //                    if (selectedPdfJsline != null)
    //                    {
    //                        if (selectedPdfJsline.Count > 0)
    //                        {
    //                            lineNumber = selectedPdfJsline[0].LineNum;
    //                            pdfJsDivNumbers = selectedPdfJsline[0].DivNum;
    //                        }
    //                    }
    //                }
    //            }

    //            foreach (var line in selectedLines)
    //            {
    //                check = false;
    //                lineCounter = 0;

    //                var nodes = xmlDocOrigXml.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes.Count > 0 &&
    //                                                                                        x.Attributes["page"] != null &&
    //                                                                                        x.Attributes["page"].Value == Convert.ToString(SiteSession.MainCurrPage));
    //                int xmlLineCount = nodes.Count();
    //                int pdfJsLinesCount = pdfJsLines.Count;

    //                foreach (XmlElement node in nodes)
    //                {
    //                    mistakeNum = 0;
    //                    lineCounter++;

    //                    if (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage)))
    //                    {
    //                        if (xmlLineCount != pdfJsLinesCount)
    //                        {
    //                            if (node.ParentNode != null && node.ParentNode.Name.Equals("col"))
    //                            {
    //                                string rowText = GetTableRowLine(node, line, node.Attributes["page"].Value);

    //                                if (!string.IsNullOrEmpty(rowText))
    //                                {
    //                                    if (IsContainsSameText(rowText, line, node.Attributes["page"].Value) &&
    //                                 (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    //                                    {
    //                                        IsSelectedErrorLine = true;
    //                                    }
    //                                }
    //                            }
    //                            else
    //                            {
    //                                if (IsContainsSameText(node.InnerText, line, node.Attributes["page"].Value) &&
    //                                   (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    //                                {
    //                                    IsSelectedErrorLine = true;
    //                                }
    //                            }
    //                        }
    //                        else if (lineCounter == lineNumber && xmlLineCount == pdfJsLinesCount)
    //                        {
    //                            IsSelectedErrorLine = true;
    //                        }

    //                        //if (lineCounter == lineNumber)
    //                        if (IsSelectedErrorLine)
    //                        {
    //                            IsSelectedErrorLine = false;
    //                            pageNum = node.Attributes["page"].Value;

    //                            //If task is test or one page test then log attrNameTest attribute and clear PDFmistake attribute value
    //                            if ((Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test")) ||
    //                                (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest")) ||
    //                                (Convert.ToString(HttpContext.Current.Session["ComparisonTask"])
    //                                    .Equals("comparisonEntryTest")))
    //                            {
    //                                if (node.HasAttribute(attrName))
    //                                {
    //                                    node.SetAttribute(attrName, "");
    //                                }

    //                                if (!node.HasAttribute(attrNameTest))
    //                                {
    //                                    XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrNameTest);

    //                                    newAttr.Value = Convert.ToString("1");
    //                                    node.SetAttributeNode(newAttr);
    //                                }

    //                                check = false;
    //                            }

    //                                //In comparison task, lines which don't have correction/conversion tag and user log errors on these line then correction/conversion
    //                            //tag is inserted into a new xml and this xml is send for qa inspection in structuring tool.
    //                            else if (!node.HasAttribute(attrName) ||
    //                                     (node.HasAttribute(attrName) && node.Attributes["PDFmistake"].Value.Equals("undo")))
    //                            {
    //                                if (node.Attributes["PDFmistake"] != null &&
    //                                    node.Attributes["PDFmistake"].Value.Equals("undo"))
    //                                {
    //                                    // Remove the PDFmistake attribute.
    //                                    node.RemoveAttribute("PDFmistake");
    //                                }

    //                                XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrName);

    //                                if (comObj.GetTotalMistakes() == 0)
    //                                    mistakeNum = 1;
    //                                else
    //                                    mistakeNum = comObj.GetTotalMistakes() + 1;

    //                                newAttr.Value = Convert.ToString(mistakeNum);
    //                                node.SetAttributeNode(newAttr);

    //                                if (node.Attributes["autoInjection"] != null)
    //                                {
    //                                    node.RemoveAttribute("autoInjection");
    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                if (node.Attributes["PDFMergemistake"] != null)
    //                                {
    //                                    node.SetAttribute("PDFMergemistake", "");
    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                if (node.Attributes["PDFSplitmistake"] != null)
    //                                {
    //                                    node.SetAttribute("PDFSplitmistake", "");
    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                //correction="merge,"

    //                                if (node.Attributes["correction"] != null)
    //                                {
    //                                    node.SetAttribute("correction", "");
    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                //Removing split mistake attributes from both uparas
    //                                if (node.ParentNode.Attributes["correction"] != null)
    //                                {
    //                                    isMistakeRemoved = false;

    //                                    //Removing split mistake attributes (correction="merge,") from first upara
    //                                    if (node.ParentNode.Attributes["correction"].Value.Equals("merge,"))
    //                                    {
    //                                        node.ParentNode.Attributes["correction"].Value = "";
    //                                    }

    //                                    //Remove merge tag from previous para
    //                                    if (node.ParentNode.PreviousSibling != null &&
    //                                        node.ParentNode.PreviousSibling.Attributes["correction"] != null &&
    //                                        !isMistakeRemoved)
    //                                    {

    //                                        if (node.ParentNode.PreviousSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                        {
    //                                            //    node.ParentNode.PreviousSibling.Attributes["correction"].Value = "";
    //                                            //    isMistakeRemoved = true;

    //                                            if (node.ParentNode.PreviousSibling.ChildNodes != null &&
    //                                                node.ParentNode.PreviousSibling.ChildNodes.Count > 0)
    //                                            {
    //                                                node.ParentNode.PreviousSibling.Attributes["correction"].Value = "";
    //                                                isMistakeRemoved = true;
    //                                            }
    //                                            else
    //                                            {
    //                                                if ((node.ParentNode.PreviousSibling.ChildNodes.Count == 0) &&
    //                                                    (node.ParentNode.PreviousSibling.Attributes["correction"] != null))
    //                                                    node.ParentNode.PreviousSibling.Attributes["correction"].Value = "";

    //                                                if (node.ParentNode.PreviousSibling.PreviousSibling != null &&
    //                                                    node.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"] != null &&
    //                                                    !isMistakeRemoved)
    //                                                {
    //                                                    node.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"].Value = "";
    //                                                    isMistakeRemoved = true;
    //                                                }
    //                                            }
    //                                        }
    //                                    }

    //                                    //Remove merge tag from next para
    //                                    if (node.ParentNode.NextSibling != null &&
    //                                        node.ParentNode.NextSibling.Attributes["correction"] != null &&
    //                                        !isMistakeRemoved)
    //                                    {
    //                                        if (node.ParentNode.NextSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                        {
    //                                            //    node.ParentNode.NextSibling.Attributes["correction"].Value = "";
    //                                            //}

    //                                            if (node.ParentNode.NextSibling.ChildNodes != null &&
    //                                                node.ParentNode.NextSibling.ChildNodes.Count > 0)
    //                                            {
    //                                                node.ParentNode.NextSibling.Attributes["correction"].Value = "";
    //                                                //isMistakeRemoved = true;
    //                                            }
    //                                            else
    //                                            {
    //                                                if ((node.ParentNode.NextSibling.ChildNodes.Count == 0) &&
    //                                                    (node.ParentNode.NextSibling.Attributes["correction"] != null))
    //                                                    node.ParentNode.NextSibling.Attributes["correction"].Value = "";

    //                                                if (node.ParentNode.NextSibling.NextSibling != null &&
    //                                                    node.ParentNode.NextSibling.NextSibling.Attributes["correction"] != null && !isMistakeRemoved)
    //                                                {
    //                                                    node.ParentNode.NextSibling.NextSibling.Attributes["correction"].Value = "";
    //                                                    //isMistakeRemoved = true;
    //                                                }
    //                                            }
    //                                        }
    //                                    }

    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                //Removing split mistake attributes from both sparas
    //                                if (node.ParentNode.ParentNode.Attributes["correction"] != null)
    //                                {
    //                                    isMistakeRemoved = false;

    //                                    //Removing split mistake attributes (correction="merge,") from first spara
    //                                    if (node.ParentNode.ParentNode.Attributes["correction"].Value.Equals("merge,"))
    //                                    {
    //                                        node.ParentNode.ParentNode.Attributes["correction"].Value = "";
    //                                    }

    //                                    //Remove merge tag from previous spara
    //                                    if (node.ParentNode.ParentNode.PreviousSibling != null &&
    //                                        node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"] != null &&
    //                                        !isMistakeRemoved)
    //                                    {
    //                                        if (node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                        {
    //                                            //if (node.ParentNode.ParentNode.PreviousSibling.ChildNodes != null)
    //                                            //{
    //                                            if (node.ParentNode.ParentNode.PreviousSibling.ChildNodes.Count == 1 &&
    //                                                node.ParentNode.ParentNode.PreviousSibling.ChildNodes[0].ChildNodes.Count > 0)
    //                                            {
    //                                                node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value = "";
    //                                            }
    //                                            //}
    //                                            else
    //                                            {
    //                                                if (node.ParentNode.ParentNode.PreviousSibling.ChildNodes[0].ChildNodes.Count == 0 &&
    //                                                    node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"] != null)
    //                                                    node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value = "";

    //                                                if (node.ParentNode.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"] != null)
    //                                                {
    //                                                    if (node.ParentNode.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                                    {
    //                                                        node.ParentNode.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"].Value = "";
    //                                                    }
    //                                                }
    //                                            }
    //                                        }
    //                                    }

    //                                    //Remove merge tag from next spara
    //                                    if (node.ParentNode.ParentNode.NextSibling != null &&
    //                                        node.ParentNode.ParentNode.NextSibling.Attributes["correction"] != null &&
    //                                        !isMistakeRemoved)
    //                                    {
    //                                        if (node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                        {
    //                                            if (node.ParentNode.ParentNode.NextSibling.ChildNodes.Count == 1 &&
    //                                               node.ParentNode.ParentNode.NextSibling.ChildNodes[0].ChildNodes.Count > 0)
    //                                            {
    //                                                node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value = "";
    //                                            }
    //                                            else
    //                                            {
    //                                                if (node.ParentNode.ParentNode.NextSibling.ChildNodes[0].ChildNodes.Count == 0 &&
    //                                                        node.ParentNode.ParentNode.NextSibling.Attributes["correction"] != null)
    //                                                    node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value = "";

    //                                                if (node.ParentNode.ParentNode.NextSibling.NextSibling.Attributes["correction"] != null)
    //                                                {
    //                                                    if (node.ParentNode.ParentNode.NextSibling.NextSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                                    {
    //                                                        node.ParentNode.ParentNode.NextSibling.NextSibling.Attributes["correction"].Value = "";
    //                                                    }
    //                                                }
    //                                            }
    //                                        }
    //                                    }

    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                if (node.Attributes["conversion"] != null)
    //                                {
    //                                    node.SetAttribute("conversion", "");
    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                if (node.Attributes["missing"] != null)
    //                                {
    //                                    node.SetAttribute("missing", "");
    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                check = true;

    //                                //If error is not of injected type then log correction/conversion attribute in a new xml which is saved before mistake injection task.
    //                                if (nonInjectedErrors_Check)
    //                                {
    //                                    LogMistakesInNewXml(lineText, pdfJsPageText, lineNumber);
    //                                }
    //                            }

    //                            //update existing error comments
    //                            else
    //                            {
    //                                if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    //                                {
    //                                    if (!string.IsNullOrEmpty(comments.Trim()) &&
    //                                        !string.IsNullOrEmpty(node.Attributes["PDFmistake"].Value))
    //                                    {
    //                                        string updateId =
    //                                            obj.InsertQaMistakes(
    //                                                Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1",
    //                                                Convert.ToInt32(HttpContext.Current.Session["userId"]), 1,
    //                                                Convert.ToInt32(node.Attributes["PDFmistake"].Value),
    //                                                DateTime.Now, Convert.ToInt32(pageNum), comments.Trim());
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }

    //                    if (check)
    //                    {
    //                        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    //                        {
    //                            string Id = obj.InsertQaMistakes(Convert.ToString((HttpContext.Current.Session["BookId"])) + "-1",
    //                                    Convert.ToInt32((HttpContext.Current.Session["userId"])), 1, mistakeNum,
    //                                    DateTime.Now, Convert.ToInt32(pageNum), comments.Trim());

    //                            XmlAttribute qaMistakeId = xmlDocOrigXml.CreateAttribute("QaMistakeId");
    //                            qaMistakeId.Value = Convert.ToString(Id);
    //                            node.SetAttributeNode(qaMistakeId);
    //                            break;
    //                        }
    //                    }
    //                }

    //                xmlDocOrigXml.Save(mainXmlPath);

    //                GlobalVar objGlobal = new GlobalVar();
    //                objGlobal.PBPDocument = xmlDocOrigXml;
    //                objGlobal.XMLPath = mainXmlPath.Replace(".xml", ".rhyw");
    //                objGlobal.SaveXml();
    //                ////XmlDocument xmlDoc = new XmlDocument(); aamir
    //                ////xmlDoc = objGlobal.PBPDocument;
    //            }
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }

    //    int detectedInjMistakes = 0;
    //    int detectedOtherMistakes = 0;

    //    string temp = comObj.GetTotalMistakesAll();

    //    string[] listMistakes = temp.Split(',');

    //    if (listMistakes != null && listMistakes.Length > 1)
    //    {
    //        detectedOtherMistakes = listMistakes[0] == "" ? 0 : Convert.ToInt32(listMistakes[0]);
    //        detectedInjMistakes = listMistakes[1] == "" ? 0 : Convert.ToInt32(listMistakes[1]);
    //    }

    //    if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    //    {
    //        //Update mistake count
    //        MyDBClass db = new MyDBClass();
    //        db.UpdateMistakeCount(Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1", Convert.ToInt32(HttpContext.Current.Session["userId"]),
    //                              detectedInjMistakes, detectedOtherMistakes);
    //    }

    //    HttpContext.Current.Session["mistakeCounter"] = comObj.GetTotalMistakes();
    //    HttpContext.Current.Session["SelectedMistakeText"] = "";

    //    return pdfJsDivNumbers;
    //}

    ////public static string LogMistakesInXml(string comments, string lineText, string pdfJsPageText)
    ////{
    ////    string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);

    ////    if (String.IsNullOrEmpty(mainXmlPath))
    ////        return "";

    ////    MyDBClass obj = new MyDBClass();
    ////    Common comObj = new Common();

    ////    string pdfJsDivNumbers = string.Empty;

    ////    XmlDocument xmlDocOrigXml = comObj.LoadXmlDocument(mainXmlPath);

    ////    string attrName = "PDFmistake";

    ////    string quizType = Convert.ToString(HttpContext.Current.Session["quizType"]);

    ////    if (quizType != "")
    ////    {
    ////        if (quizType.Equals("Splitting"))
    ////        {
    ////            attrName = "PDFMergemistake";
    ////        }
    ////        else if (quizType.Equals("Merging"))
    ////        {
    ////            attrName = "PDFSplitmistake";
    ////        }
    ////        else if (quizType.Equals("Space"))
    ////        {
    ////            attrName = "PDFmistake";
    ////        }
    ////    }

    ////    List<string> selectedText = new List<string>();
    ////    string TextFromPDFJs = lineText;  //The original text that was selected for editing

    ////    HttpContext.Current.Session["OriginalText"] = TextFromPDFJs;

    ////    if (TextFromPDFJs == "")
    ////        return "";

    ////    var selectedLines = TextFromPDFJs.Split(new string[] { "\r\n" }, StringSplitOptions.None);

    ////    selectedLines = selectedLines.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

    ////    try
    ////    {
    ////        if ((selectedLines != null) && (selectedLines.Length > 0) && (xmlDocOrigXml != null))
    ////        {
    ////            //string attrName = "PDFmistake";
    ////            string attrNameTest = "PDFmistakeTest";
    ////            int mistakeNum = 0;
    ////            bool check = false;
    ////            bool isMistakeRemoved = false;
    ////            bool nonInjectedErrors_Check = true;
    ////            string pageNum = "";
    ////            int lineCounter = 0;
    ////            bool IsSelectedErrorLine = false;
    ////            int lineNumber = 0;

    ////            List<PdfJsLine> pdfJsLines = comObj.GetCurrentPdfJsPageLines(pdfJsPageText, Convert.ToInt32(SiteSession.MainCurrPage));
    ////            if (pdfJsLines != null)
    ////            {
    ////                if (pdfJsLines.Count > 0)
    ////                {
    ////                    List<PdfJsLine> selectedPdfJsline = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(lineText))).ToList();
    ////                    if (selectedPdfJsline != null)
    ////                    {
    ////                        if (selectedPdfJsline.Count > 0)
    ////                        {
    ////                            lineNumber = selectedPdfJsline[0].LineNum;
    ////                            pdfJsDivNumbers = selectedPdfJsline[0].DivNum;
    ////                        }
    ////                    }
    ////                }
    ////            }

    ////            //foreach (var line in selectedLines)
    ////            for (int line = 0; line < selectedLines.Count(); line++)
    ////            {
    ////                check = false;
    ////                lineCounter = 0;

    ////                var nodes = xmlDocOrigXml.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes.Count > 0 &&
    ////                                                                                        x.Attributes["page"] != null &&
    ////                                                                                        x.Attributes["page"].Value == Convert.ToString(SiteSession.MainCurrPage));
    ////                int xmlLineCount = nodes.Count();
    ////                int pdfJsLinesCount = pdfJsLines.Count;

    ////                foreach (XmlElement node in nodes)
    ////                {
    ////                    mistakeNum = 0;
    ////                    lineCounter++;

    ////                    if (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage)))
    ////                    {
    ////                        if (xmlLineCount != pdfJsLinesCount)
    ////                        {
    ////                            if (node.ParentNode != null && node.ParentNode.Name.Equals("col"))
    ////                            {
    ////                                //string rowText = GetTableRowLine(node, selectedLines[line], node.Attributes["page"].Value);
    ////                                string colUry = node.Attributes["top"] != null ? node.Attributes["top"].Value : "";
    ////                                StringBuilder sbRowText = new StringBuilder();

    ////                                if (node.ParentNode.ParentNode != null)
    ////                                {
    ////                                    XmlNode rowNode = node.ParentNode.ParentNode;
    ////                                    var rowLines = rowNode.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes["top"].Value == colUry).ToList();

    ////                                    if (rowLines != null)
    ////                                    {
    ////                                        foreach (XmlNode ln in rowLines)
    ////                                        {
    ////                                            sbRowText.Append(ln.InnerText + " ");
    ////                                            line++;
    ////                                        }
    ////                                    }
    ////                                }

    ////                                if (!string.IsNullOrEmpty(Convert.ToString(sbRowText)))
    ////                                {
    ////                                    if (IsContainsSameText(Convert.ToString(sbRowText), selectedLines[line], node.Attributes["page"].Value) &&
    ////                                 (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    ////                                    {
    ////                                        IsSelectedErrorLine = true;
    ////                                    }
    ////                                }
    ////                            }
    ////                            else
    ////                            {
    ////                                if (IsContainsSameText(node.InnerText, selectedLines[line], node.Attributes["page"].Value) &&
    ////                                   (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    ////                                {
    ////                                    IsSelectedErrorLine = true;
    ////                                }
    ////                            }
    ////                        }
    ////                        else if (lineCounter == lineNumber && xmlLineCount == pdfJsLinesCount)
    ////                        {
    ////                            IsSelectedErrorLine = true;
    ////                        }

    ////                        //if (lineCounter == lineNumber)
    ////                        if (IsSelectedErrorLine)
    ////                        {
    ////                            IsSelectedErrorLine = false;
    ////                            pageNum = node.Attributes["page"].Value;

    ////                            //If task is test or one page test then log attrNameTest attribute and clear PDFmistake attribute value
    ////                            if ((Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test")) ||
    ////                                (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest")) ||
    ////                                (Convert.ToString(HttpContext.Current.Session["ComparisonTask"])
    ////                                    .Equals("comparisonEntryTest")))
    ////                            {
    ////                                if (node.HasAttribute(attrName))
    ////                                {
    ////                                    node.SetAttribute(attrName, "");
    ////                                }

    ////                                if (!node.HasAttribute(attrNameTest))
    ////                                {
    ////                                    XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrNameTest);

    ////                                    newAttr.Value = Convert.ToString("1");
    ////                                    node.SetAttributeNode(newAttr);
    ////                                }

    ////                                check = false;
    ////                            }

    ////                                //In comparison task, lines which don't have correction/conversion tag and user log errors on these line then correction/conversion
    ////                            //tag is inserted into a new xml and this xml is send for qa inspection in structuring tool.
    ////                            else if (!node.HasAttribute(attrName) ||
    ////                                     (node.HasAttribute(attrName) && node.Attributes["PDFmistake"].Value.Equals("undo")))
    ////                            {
    ////                                if (node.Attributes["PDFmistake"] != null &&
    ////                                    node.Attributes["PDFmistake"].Value.Equals("undo"))
    ////                                {
    ////                                    // Remove the PDFmistake attribute.
    ////                                    node.RemoveAttribute("PDFmistake");
    ////                                }

    ////                                XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrName);

    ////                                if (comObj.GetTotalMistakes() == 0)
    ////                                    mistakeNum = 1;
    ////                                else
    ////                                    mistakeNum = comObj.GetTotalMistakes() + 1;

    ////                                newAttr.Value = Convert.ToString(mistakeNum);
    ////                                node.SetAttributeNode(newAttr);

    ////                                if (node.Attributes["autoInjection"] != null)
    ////                                {
    ////                                    node.RemoveAttribute("autoInjection");
    ////                                    nonInjectedErrors_Check = false;
    ////                                }

    ////                                if (node.Attributes["PDFMergemistake"] != null)
    ////                                {
    ////                                    node.SetAttribute("PDFMergemistake", "");
    ////                                    nonInjectedErrors_Check = false;
    ////                                }

    ////                                if (node.Attributes["PDFSplitmistake"] != null)
    ////                                {
    ////                                    node.SetAttribute("PDFSplitmistake", "");
    ////                                    nonInjectedErrors_Check = false;
    ////                                }

    ////                                //correction="merge,"

    ////                                if (node.Attributes["correction"] != null)
    ////                                {
    ////                                    node.SetAttribute("correction", "");
    ////                                    nonInjectedErrors_Check = false;
    ////                                }

    ////                                //Removing split mistake attributes from both uparas
    ////                                if (node.ParentNode.Attributes["correction"] != null)
    ////                                {
    ////                                    isMistakeRemoved = false;

    ////                                    //Removing split mistake attributes (correction="merge,") from first upara
    ////                                    if (node.ParentNode.Attributes["correction"].Value.Equals("merge,"))
    ////                                    {
    ////                                        node.ParentNode.Attributes["correction"].Value = "";
    ////                                    }

    ////                                    //Remove merge tag from previous para
    ////                                    if (node.ParentNode.PreviousSibling != null &&
    ////                                        node.ParentNode.PreviousSibling.Attributes["correction"] != null &&
    ////                                        !isMistakeRemoved)
    ////                                    {

    ////                                        if (node.ParentNode.PreviousSibling.Attributes["correction"].Value.Equals("merge,"))
    ////                                        {
    ////                                            //    node.ParentNode.PreviousSibling.Attributes["correction"].Value = "";
    ////                                            //    isMistakeRemoved = true;

    ////                                            if (node.ParentNode.PreviousSibling.ChildNodes != null &&
    ////                                                node.ParentNode.PreviousSibling.ChildNodes.Count > 0)
    ////                                            {
    ////                                                node.ParentNode.PreviousSibling.Attributes["correction"].Value = "";
    ////                                                isMistakeRemoved = true;
    ////                                            }
    ////                                            else
    ////                                            {
    ////                                                if ((node.ParentNode.PreviousSibling.ChildNodes.Count == 0) &&
    ////                                                    (node.ParentNode.PreviousSibling.Attributes["correction"] != null))
    ////                                                    node.ParentNode.PreviousSibling.Attributes["correction"].Value = "";

    ////                                                if (node.ParentNode.PreviousSibling.PreviousSibling != null &&
    ////                                                    node.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"] != null &&
    ////                                                    !isMistakeRemoved)
    ////                                                {
    ////                                                    node.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"].Value = "";
    ////                                                    isMistakeRemoved = true;
    ////                                                }
    ////                                            }
    ////                                        }
    ////                                    }

    ////                                    //Remove merge tag from next para
    ////                                    if (node.ParentNode.NextSibling != null &&
    ////                                        node.ParentNode.NextSibling.Attributes["correction"] != null &&
    ////                                        !isMistakeRemoved)
    ////                                    {
    ////                                        if (node.ParentNode.NextSibling.Attributes["correction"].Value.Equals("merge,"))
    ////                                        {
    ////                                            //    node.ParentNode.NextSibling.Attributes["correction"].Value = "";
    ////                                            //}

    ////                                            if (node.ParentNode.NextSibling.ChildNodes != null &&
    ////                                                node.ParentNode.NextSibling.ChildNodes.Count > 0)
    ////                                            {
    ////                                                node.ParentNode.NextSibling.Attributes["correction"].Value = "";
    ////                                                //isMistakeRemoved = true;
    ////                                            }
    ////                                            else
    ////                                            {
    ////                                                if ((node.ParentNode.NextSibling.ChildNodes.Count == 0) &&
    ////                                                    (node.ParentNode.NextSibling.Attributes["correction"] != null))
    ////                                                    node.ParentNode.NextSibling.Attributes["correction"].Value = "";

    ////                                                if (node.ParentNode.NextSibling.NextSibling != null &&
    ////                                                    node.ParentNode.NextSibling.NextSibling.Attributes["correction"] != null && !isMistakeRemoved)
    ////                                                {
    ////                                                    node.ParentNode.NextSibling.NextSibling.Attributes["correction"].Value = "";
    ////                                                    //isMistakeRemoved = true;
    ////                                                }
    ////                                            }
    ////                                        }
    ////                                    }

    ////                                    nonInjectedErrors_Check = false;
    ////                                }

    ////                                //Removing split mistake attributes from both sparas
    ////                                if (node.ParentNode.ParentNode.Attributes["correction"] != null)
    ////                                {
    ////                                    isMistakeRemoved = false;

    ////                                    //Removing split mistake attributes (correction="merge,") from first spara
    ////                                    if (node.ParentNode.ParentNode.Attributes["correction"].Value.Equals("merge,"))
    ////                                    {
    ////                                        node.ParentNode.ParentNode.Attributes["correction"].Value = "";
    ////                                    }

    ////                                    //Remove merge tag from previous spara
    ////                                    if (node.ParentNode.ParentNode.PreviousSibling != null &&
    ////                                        node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"] != null &&
    ////                                        !isMistakeRemoved)
    ////                                    {
    ////                                        if (node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value.Equals("merge,"))
    ////                                        {
    ////                                            //if (node.ParentNode.ParentNode.PreviousSibling.ChildNodes != null)
    ////                                            //{
    ////                                            if (node.ParentNode.ParentNode.PreviousSibling.ChildNodes.Count == 1 &&
    ////                                                node.ParentNode.ParentNode.PreviousSibling.ChildNodes[0].ChildNodes.Count > 0)
    ////                                            {
    ////                                                node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value = "";
    ////                                            }
    ////                                            //}
    ////                                            else
    ////                                            {
    ////                                                if (node.ParentNode.ParentNode.PreviousSibling.ChildNodes[0].ChildNodes.Count == 0 &&
    ////                                                    node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"] != null)
    ////                                                    node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value = "";

    ////                                                if (node.ParentNode.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"] != null)
    ////                                                {
    ////                                                    if (node.ParentNode.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"].Value.Equals("merge,"))
    ////                                                    {
    ////                                                        node.ParentNode.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"].Value = "";
    ////                                                    }
    ////                                                }
    ////                                            }
    ////                                        }
    ////                                    }

    ////                                    //Remove merge tag from next spara
    ////                                    if (node.ParentNode.ParentNode.NextSibling != null &&
    ////                                        node.ParentNode.ParentNode.NextSibling.Attributes["correction"] != null &&
    ////                                        !isMistakeRemoved)
    ////                                    {
    ////                                        if (node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value.Equals("merge,"))
    ////                                        {
    ////                                            if (node.ParentNode.ParentNode.NextSibling.ChildNodes.Count == 1 &&
    ////                                               node.ParentNode.ParentNode.NextSibling.ChildNodes[0].ChildNodes.Count > 0)
    ////                                            {
    ////                                                node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value = "";
    ////                                            }
    ////                                            else
    ////                                            {
    ////                                                if (node.ParentNode.ParentNode.NextSibling.ChildNodes[0].ChildNodes.Count == 0 &&
    ////                                                        node.ParentNode.ParentNode.NextSibling.Attributes["correction"] != null)
    ////                                                    node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value = "";

    ////                                                if (node.ParentNode.ParentNode.NextSibling.NextSibling.Attributes["correction"] != null)
    ////                                                {
    ////                                                    if (node.ParentNode.ParentNode.NextSibling.NextSibling.Attributes["correction"].Value.Equals("merge,"))
    ////                                                    {
    ////                                                        node.ParentNode.ParentNode.NextSibling.NextSibling.Attributes["correction"].Value = "";
    ////                                                    }
    ////                                                }
    ////                                            }
    ////                                        }
    ////                                    }

    ////                                    nonInjectedErrors_Check = false;
    ////                                }

    ////                                if (node.Attributes["conversion"] != null)
    ////                                {
    ////                                    node.SetAttribute("conversion", "");
    ////                                    nonInjectedErrors_Check = false;
    ////                                }

    ////                                if (node.Attributes["missing"] != null)
    ////                                {
    ////                                    node.SetAttribute("missing", "");
    ////                                    nonInjectedErrors_Check = false;
    ////                                }

    ////                                check = true;

    ////                                //If error is not of injected type then log correction/conversion attribute in a new xml which is saved before mistake injection task.
    ////                                if (nonInjectedErrors_Check)
    ////                                {
    ////                                    LogMistakesInNewXml(lineText, pdfJsPageText, lineNumber);
    ////                                }
    ////                            }

    ////                            //update existing error comments
    ////                            else
    ////                            {
    ////                                if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    ////                                {
    ////                                    if (!string.IsNullOrEmpty(comments.Trim()) &&
    ////                                        !string.IsNullOrEmpty(node.Attributes["PDFmistake"].Value))
    ////                                    {
    ////                                        string updateId =
    ////                                            obj.InsertQaMistakes(
    ////                                                Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1",
    ////                                                Convert.ToInt32(HttpContext.Current.Session["userId"]), 1,
    ////                                                Convert.ToInt32(node.Attributes["PDFmistake"].Value),
    ////                                                DateTime.Now, Convert.ToInt32(pageNum), comments.Trim());
    ////                                    }
    ////                                }
    ////                            }
    ////                        }
    ////                    }

    ////                    if (check)
    ////                    {
    ////                        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    ////                        {
    ////                            string Id = obj.InsertQaMistakes(Convert.ToString((HttpContext.Current.Session["BookId"])) + "-1",
    ////                                    Convert.ToInt32((HttpContext.Current.Session["userId"])), 1, mistakeNum,
    ////                                    DateTime.Now, Convert.ToInt32(pageNum), comments.Trim());

    ////                            XmlAttribute qaMistakeId = xmlDocOrigXml.CreateAttribute("QaMistakeId");
    ////                            qaMistakeId.Value = Convert.ToString(Id);
    ////                            node.SetAttributeNode(qaMistakeId);
    ////                            break;
    ////                        }
    ////                    }
    ////                }

    ////                xmlDocOrigXml.Save(mainXmlPath);

    ////                GlobalVar objGlobal = new GlobalVar();
    ////                objGlobal.PBPDocument = xmlDocOrigXml;
    ////                objGlobal.XMLPath = mainXmlPath.Replace(".xml", ".rhyw");
    ////                objGlobal.SaveXml();

    ////            }//end for loop
    ////        }
    ////    }
    ////    catch (Exception)
    ////    {
    ////        throw;
    ////    }

    ////    int detectedInjMistakes = 0;
    ////    int detectedOtherMistakes = 0;

    ////    string temp = comObj.GetTotalMistakesAll();

    ////    string[] listMistakes = temp.Split(',');

    ////    if (listMistakes != null && listMistakes.Length > 1)
    ////    {
    ////        detectedOtherMistakes = listMistakes[0] == "" ? 0 : Convert.ToInt32(listMistakes[0]);
    ////        detectedInjMistakes = listMistakes[1] == "" ? 0 : Convert.ToInt32(listMistakes[1]);
    ////    }

    ////    if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    ////    {
    ////        //Update mistake count
    ////        MyDBClass db = new MyDBClass();
    ////        db.UpdateMistakeCount(Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1", Convert.ToInt32(HttpContext.Current.Session["userId"]),
    ////                              detectedInjMistakes, detectedOtherMistakes);
    ////    }

    ////    HttpContext.Current.Session["mistakeCounter"] = comObj.GetTotalMistakes();
    ////    HttpContext.Current.Session["SelectedMistakeText"] = "";

    ////    return pdfJsDivNumbers;
    ////}

    #region Log Mistake from PDFJs into xml

    public static string LogMistakesInLines(string comments, string pdfJsSelectedLineText, string pdfJsPageText)
    {
        string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);

        if (String.IsNullOrEmpty(mainXmlPath)) return "";

        MyDBClass obj = new MyDBClass();
        Common comObj = new Common();
        XmlDocument pdfXml = comObj.LoadXmlDocument(mainXmlPath);
        var xmlLines = pdfXml.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes.Count > 0 && x.Attributes["page"] != null &&
                                                                                 x.Attributes["page"].Value == Convert.ToString(SiteSession.MainCurrPage));
        string pdfJsDivNumbers = string.Empty;

        string attrName = "PDFmistake";

        string quizType = Convert.ToString(HttpContext.Current.Session["quizType"]);

        if (quizType != "")
        {
            if (quizType.Equals("Splitting")) attrName = "PDFMergemistake";
            else if (quizType.Equals("Merging")) attrName = "PDFSplitmistake";
            else if (quizType.Equals("Space")) attrName = "PDFmistake";
        }

        List<string> selectedText = new List<string>();

        HttpContext.Current.Session["OriginalText"] = pdfJsSelectedLineText;

        if (pdfJsSelectedLineText == "") return "";

        List<string> pdfJsSelectedLines = pdfJsSelectedLineText.Split(new string[] { "\r\n" }, StringSplitOptions.None)
                                                               .Where(x => (!string.IsNullOrEmpty(x))).ToList();

        if ((pdfJsSelectedLines == null || pdfJsSelectedLines.Count == 0) || (pdfXml == null)) return "";

        string attrNameTest = "PDFmistakeTest";
        int mistakeNum = 0;
        bool check = false;
        bool isMistakeRemoved = false;
        bool nonInjectedErrors_Check = true;
        string pageNum = "";
        int lineCounter = 0;
        bool IsSelectedErrorLine = false;
        int lineNumber = 0;

        List<PdfJsLine> pdfJsLines = comObj.GetCurrentPdfJsPageLines(pdfJsPageText, Convert.ToInt32(SiteSession.MainCurrPage));
        if (pdfJsLines == null || pdfJsLines.Count == 0) return "";

        List<PdfJsLine> selectedPdfJsline = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(pdfJsSelectedLineText))).ToList();
        if (selectedPdfJsline != null)
        {
            if (selectedPdfJsline.Count > 0)
            {
                lineNumber = selectedPdfJsline[0].LineNum;
                pdfJsDivNumbers = selectedPdfJsline[0].DivNum;
            }
        }

        int xmlLineCount = xmlLines.Count();
        int pdfJsLinesCount = pdfJsLines.Count;

        for (int i = 0; i < pdfJsSelectedLines.Count; i++)
        {
            check = false;
            lineCounter = 0;

            foreach (XmlElement node in xmlLines)
            {
                mistakeNum = 0;
                lineCounter++;

                if (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage)))
                {
                    if (node.ParentNode != null && node.ParentNode.Name.Equals("col"))
                    {
                        string colUry = node.Attributes["top"] != null ? node.Attributes["top"].Value : "";
                        StringBuilder sbRowText = new StringBuilder();

                        if (node.ParentNode.ParentNode != null)
                        {
                            XmlNode rowNode = node.ParentNode.ParentNode;
                            var rowLines = rowNode.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes["top"].Value == colUry).ToList();

                            if (rowLines != null)
                            {
                                foreach (XmlNode ln in rowLines)
                                {
                                    sbRowText.Append(ln.InnerText + " ");
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(sbRowText)))
                        {
                            //if (IsContainsSameText(node.InnerText, pdfJsSelectedLines[i], node.Attributes["page"].Value) &&
                            //    (node.Attributes["page"].Value.Equals(
                            //        Convert.ToString(SiteSession.MainCurrPage))))
                            if (IsContainsSameText(node.InnerText, pdfJsSelectedLines[i]) &&
                               (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                            {
                                IsSelectedErrorLine = true;
                                pdfJsSelectedLines.Add("");
                            }
                        }
                    }
                    else
                    {
                        if (IsContainsSameText(node.InnerText, pdfJsSelectedLines[i]) &&
                            (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                        {
                            IsSelectedErrorLine = true;
                        }
                    }
                }
            }
        }
        //}
        return "";
    }

    public static string LogMistakesInXml(string comments, string lineText, string pdfJsPageText)
    {
        string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);
        if (String.IsNullOrEmpty(mainXmlPath)) return "";

        MyDBClass obj = new MyDBClass();
        Common comObj = new Common();

        string pdfJsDivNumbers = string.Empty;

        XmlDocument xmlDocOrigXml = comObj.LoadXmlDocument(mainXmlPath);

        string attrName = "PDFmistake";

        string quizType = Convert.ToString(HttpContext.Current.Session["quizType"]);

        if (quizType != "")
        {
            if (quizType.Equals("Splitting"))
            {
                attrName = "PDFMergemistake";
            }
            else if (quizType.Equals("Merging"))
            {
                attrName = "PDFSplitmistake";
            }
            else if (quizType.Equals("Space"))
            {
                attrName = "PDFmistake";
            }
        }

        string TextFromPDFJs = lineText;  //The original text that was selected for editing

        HttpContext.Current.Session["OriginalText"] = TextFromPDFJs;

        if (TextFromPDFJs == "") return "";

        List<string> processedXmlLines = new List<string>();

        var selectedLines = TextFromPDFJs.Split(new string[] { "\r\n" }, StringSplitOptions.None).Where(x => (!string.IsNullOrEmpty(x))).ToList();

        try
        {
            if ((selectedLines != null) && (selectedLines.Count > 0) && (xmlDocOrigXml != null))
            {
                string attrNameTest = "PDFmistakeTest";
                int mistakeNum = 0;
                bool check = false;
                bool isMistakeRemoved = false;
                bool nonInjectedErrors_Check = true;
                string pageNum = "";
                int lineCounter = 0;
                bool IsSelectedErrorLine = false;
                int lineNumber = 0;

                List<PdfJsLine> pdfJsLines = comObj.GetCurrentPdfJsPageLines(pdfJsPageText, Convert.ToInt32(SiteSession.MainCurrPage));
                if (pdfJsLines != null)
                {
                    if (pdfJsLines.Count > 0)
                    {
                        List<PdfJsLine> selectedPdfJsline = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text.Replace("\0", "")).Equals(RemoveWhiteSpace(lineText))).ToList();
                        if (selectedPdfJsline != null)
                        {
                            if (selectedPdfJsline.Count > 0)
                            {
                                lineNumber = selectedPdfJsline[0].LineNum;
                                pdfJsDivNumbers = selectedPdfJsline[0].DivNum;

                                if (selectedLines.Count == 1)
                                    selectedLines[0] = selectedPdfJsline[0].Text;
                            }
                        }
                    }
                }

                var nodes = xmlDocOrigXml.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes.Count > 0 &&
                                                                                           x.Attributes["page"] != null &&
                                                                                           x.Attributes["page"].Value == Convert.ToString(SiteSession.MainCurrPage));


                if (nodes == null) return "";

                ////string pdfPath = "";

                ////string pageText = GetTextFromPdf(pdfPath);
                ////List<string> linesWithHyphenList = new List<string>();

                ////if (!string.IsNullOrEmpty(pageText))
                ////    linesWithHyphenList = GetHyphenLines(pageText);


                int xmlLineCount = nodes.Count();
                int pdfJsLinesCount = pdfJsLines.Count;

                for (int i = 0; i < selectedLines.Count; i++)
                {
                    check = false;
                    lineCounter = 0;

                    foreach (XmlElement node in nodes)
                    {
                        if (!processedXmlLines.Contains(node.InnerText.Trim()))
                        {
                            mistakeNum = 0;
                            lineCounter++;

                            if (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage)))
                            {
                                if (xmlLineCount != pdfJsLinesCount)
                                {
                                    if (node.ParentNode != null && (node.ParentNode.Name.Equals("col") || node.ParentNode.Name.Equals("head-col")))
                                    {
                                        double colUry = node.Attributes["top"] != null ? Convert.ToDouble(node.Attributes["top"].Value) : 0;
                                        StringBuilder sbRowText = new StringBuilder();

                                        if (node.ParentNode.ParentNode != null)
                                        {
                                            XmlNode rowNode = node.ParentNode.ParentNode;

                                            var rowLines = rowNode.SelectNodes("//ln[@page='" + Convert.ToString(SiteSession.MainCurrPage) + "']").Cast<XmlNode>()
                                                                    .Where(x => Convert.ToDouble(x.Attributes["top"].Value) == colUry)
                                                                    .ToList();

                                            if (rowLines != null && rowLines.Count >= 1)
                                            {
                                                bool isFirstLineMatched = false;

                                                foreach (XmlNode ln in rowLines)
                                                {
                                                    sbRowText.Append(ln.InnerText + " ");

                                                    if (IsContainsSameText(ln.InnerText, selectedLines[i]) &&
                                                        (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                                                    {
                                                        IsSelectedErrorLine = true;
                                                        isFirstLineMatched = true;
                                                        processedXmlLines.Add(ln.InnerText.Trim());
                                                    }
                                                    else if (isFirstLineMatched)
                                                    {
                                                        if (!selectedLines.Contains(ln.InnerText.Trim()))
                                                        {
                                                            selectedLines.Add(ln.InnerText.Trim());
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                    else
                                    {
                                        //if (IsContainsSameText(node.InnerText, selectedLines[i], node.Attributes["page"].Value) &&
                                        //    (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                                        if (IsContainsSameText(node.InnerText, selectedLines[i]) &&
                                           (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                                        {
                                            IsSelectedErrorLine = true;
                                        }
                                    }
                                }
                                else if (lineCounter == lineNumber && xmlLineCount == pdfJsLinesCount)
                                {
                                    IsSelectedErrorLine = true;
                                }

                                if (IsSelectedErrorLine)
                                {
                                    IsSelectedErrorLine = false;
                                    pageNum = node.Attributes["page"].Value;

                                    //If task is test or one page test then log attrNameTest attribute and clear PDFmistake attribute value
                                    if ((Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test")) ||
                                        (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest")) ||
                                        (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("comparisonEntryTest")) ||
                                        (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("CompUpgradedSampleTest")) ||
                                        (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("CompUpgradedStartTest")))
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

                                    //In comparison task, lines which don't have correction/conversion tag and user log errors on these line 
                                    //then correction/conversion tag is inserted into a new xml and this xml is send for qa inspection in structuring tool.
                                    else if (!node.HasAttribute(attrName) ||
                                             (node.HasAttribute(attrName) &&
                                              node.Attributes["PDFmistake"].Value.Equals("undo")))
                                    {
                                        if (node.Attributes["PDFmistake"] != null &&
                                            node.Attributes["PDFmistake"].Value.Equals("undo"))
                                        {
                                            // Remove the PDFmistake attribute.
                                            node.RemoveAttribute("PDFmistake");
                                        }

                                        XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrName);

                                        if (comObj.GetTotalMistakes() == 0)
                                            mistakeNum = 1;
                                        else
                                            mistakeNum = comObj.GetTotalMistakes() + 1;

                                        newAttr.Value = Convert.ToString(mistakeNum);
                                        node.SetAttributeNode(newAttr);

                                        if (node.Attributes["autoInjection"] != null)
                                        {
                                            node.RemoveAttribute("autoInjection");
                                            nonInjectedErrors_Check = false;
                                        }

                                        if (node.Attributes["PDFMergemistake"] != null)
                                        {
                                            node.SetAttribute("PDFMergemistake", "");
                                            nonInjectedErrors_Check = false;
                                        }

                                        if (node.Attributes["PDFSplitmistake"] != null)
                                        {
                                            node.SetAttribute("PDFSplitmistake", "");
                                            nonInjectedErrors_Check = false;
                                        }

                                        if (node.Attributes["correction"] != null)
                                        {
                                            node.SetAttribute("correction", "");
                                            nonInjectedErrors_Check = false;
                                        }

                                        //Removing split mistake attributes from both uparas
                                        if (node.ParentNode.Attributes["correction"] != null)
                                        {
                                            isMistakeRemoved = false;

                                            //Removing split mistake attributes (correction="merge,") from first upara
                                            if (node.ParentNode.Attributes["correction"].Value.Equals("merge,"))
                                            {
                                                node.ParentNode.Attributes["correction"].Value = "";
                                            }

                                            //Remove merge tag from previous para
                                            if (node.ParentNode.PreviousSibling != null &&
                                                node.ParentNode.PreviousSibling.Attributes["correction"] != null &&
                                                !isMistakeRemoved)
                                            {

                                                if (node.ParentNode.PreviousSibling.Attributes["correction"].Value.Equals("merge,"))
                                                {
                                                    if (node.ParentNode.PreviousSibling.ChildNodes != null &&
                                                        node.ParentNode.PreviousSibling.ChildNodes.Count > 0)
                                                    {
                                                        node.ParentNode.PreviousSibling.Attributes["correction"].Value =
                                                            "";
                                                        isMistakeRemoved = true;
                                                    }
                                                    else
                                                    {
                                                        if ((node.ParentNode.PreviousSibling.ChildNodes.Count == 0) &&
                                                            (node.ParentNode.PreviousSibling.Attributes["correction"] !=
                                                             null))
                                                            node.ParentNode.PreviousSibling.Attributes["correction"]
                                                                .Value = "";

                                                        if (node.ParentNode.PreviousSibling.PreviousSibling != null &&
                                                            node.ParentNode.PreviousSibling.PreviousSibling.Attributes[
                                                                "correction"] != null &&
                                                            !isMistakeRemoved)
                                                        {
                                                            node.ParentNode.PreviousSibling.PreviousSibling.Attributes[
                                                                "correction"].Value = "";
                                                            isMistakeRemoved = true;
                                                        }
                                                    }
                                                }
                                            }

                                            //Remove merge tag from next para
                                            if (node.ParentNode.NextSibling != null &&
                                                node.ParentNode.NextSibling.Attributes["correction"] != null &&
                                                !isMistakeRemoved)
                                            {
                                                if (node.ParentNode.NextSibling.Attributes["correction"].Value.Equals("merge,"))
                                                {
                                                    if (node.ParentNode.NextSibling.ChildNodes != null &&
                                                        node.ParentNode.NextSibling.ChildNodes.Count > 0)
                                                    {
                                                        node.ParentNode.NextSibling.Attributes["correction"].Value = "";
                                                    }
                                                    else
                                                    {
                                                        if ((node.ParentNode.NextSibling.ChildNodes.Count == 0) &&
                                                            (node.ParentNode.NextSibling.Attributes["correction"] !=
                                                             null))
                                                            node.ParentNode.NextSibling.Attributes["correction"].Value =
                                                                "";

                                                        if (node.ParentNode.NextSibling.NextSibling != null &&
                                                            node.ParentNode.NextSibling.NextSibling.Attributes["correction"] != null && !isMistakeRemoved)
                                                        {
                                                            node.ParentNode.NextSibling.NextSibling.Attributes["correction"].Value = "";
                                                        }
                                                    }
                                                }
                                            }

                                            nonInjectedErrors_Check = false;
                                        }

                                        //Removing split mistake attributes from both sparas
                                        if (node.ParentNode.ParentNode.Attributes["correction"] != null)
                                        {
                                            isMistakeRemoved = false;

                                            //Removing split mistake attributes (correction="merge,") from first spara
                                            if (node.ParentNode.ParentNode.Attributes["correction"].Value.Equals(
                                                    "merge,"))
                                            {
                                                node.ParentNode.ParentNode.Attributes["correction"].Value = "";
                                            }

                                            //Remove merge tag from previous spara
                                            if (node.ParentNode.ParentNode.PreviousSibling != null &&
                                                node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"] != null && !isMistakeRemoved)
                                            {
                                                if (node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value.Equals("merge,"))
                                                {
                                                    if (node.ParentNode.ParentNode.PreviousSibling.ChildNodes.Count == 1 &&
                                                        node.ParentNode.ParentNode.PreviousSibling.ChildNodes[0]
                                                            .ChildNodes.Count > 0)
                                                    {
                                                        node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value = "";
                                                    }
                                                    else
                                                    {
                                                        if (node.ParentNode.ParentNode.PreviousSibling.ChildNodes[0]
                                                                .ChildNodes.Count == 0 &&
                                                            node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"] != null)
                                                            node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value = "";

                                                        if (node.ParentNode.ParentNode.PreviousSibling.PreviousSibling
                                                                .Attributes["correction"] != null)
                                                        {
                                                            if (node.ParentNode.ParentNode.PreviousSibling
                                                                    .PreviousSibling.Attributes["correction"].Value
                                                                    .Equals("merge,"))
                                                            {
                                                                node.ParentNode.ParentNode.PreviousSibling
                                                                    .PreviousSibling.Attributes["correction"].Value = "";
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            //Remove merge tag from next spara
                                            if (node.ParentNode.ParentNode.NextSibling != null &&
                                                node.ParentNode.ParentNode.NextSibling.Attributes["correction"] != null &&
                                                !isMistakeRemoved)
                                            {
                                                if (node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value.Equals("merge,"))
                                                {
                                                    if (node.ParentNode.ParentNode.NextSibling.ChildNodes.Count == 1 &&
                                                        node.ParentNode.ParentNode.NextSibling.ChildNodes[0].ChildNodes.Count > 0)
                                                    {
                                                        node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value = "";
                                                    }
                                                    else
                                                    {
                                                        if (node.ParentNode.ParentNode.NextSibling.ChildNodes[0]
                                                                .ChildNodes.Count == 0 &&
                                                            node.ParentNode.ParentNode.NextSibling.Attributes["correction"] != null)
                                                            node.ParentNode.ParentNode.NextSibling.Attributes[
                                                                "correction"].Value = "";

                                                        if (node.ParentNode.ParentNode.NextSibling.NextSibling != null &&
                                                            node.ParentNode.ParentNode.NextSibling.NextSibling
                                                                .Attributes["correction"] != null)
                                                        {
                                                            if (node.ParentNode.ParentNode.NextSibling.NextSibling
                                                                    .Attributes["correction"].Value.Equals("merge,"))
                                                            {
                                                                node.ParentNode.ParentNode.NextSibling.NextSibling
                                                                    .Attributes["correction"].Value = "";
                                                            }
                                                        }
                                                    }
                                                }
                                            }

                                            nonInjectedErrors_Check = false;
                                        }

                                        if (node.Attributes["conversion"] != null)
                                        {
                                            node.SetAttribute("conversion", "");
                                            nonInjectedErrors_Check = false;
                                        }

                                        if (node.Attributes["missing"] != null)
                                        {
                                            node.SetAttribute("missing", "");
                                            nonInjectedErrors_Check = false;
                                        }

                                        check = true;

                                        //If error is not of injected type then log correction/conversion attribute in a new xml which is saved before 
                                        //mistake injection task.
                                        if (nonInjectedErrors_Check)
                                        {
                                            LogMistakesInNewXml(lineText, pdfJsPageText, lineNumber);
                                        }
                                    }
                                    //update existing error comments
                                    else
                                    {
                                        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
                                        {
                                            if (!string.IsNullOrEmpty(comments.Trim()) && !string.IsNullOrEmpty(node.Attributes["PDFmistake"].Value))
                                            {
                                                string updateId = obj.InsertQaMistakes(Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1",
                                                        Convert.ToInt32(HttpContext.Current.Session["userId"]), 1,
                                                        Convert.ToInt32(node.Attributes["PDFmistake"].Value),
                                                        DateTime.Now, Convert.ToInt32(pageNum), comments.Trim());
                                            }
                                        }
                                    }
                                }//end IsSelectedErrorLine
                            }
                        }

                        if (check)
                        {
                            if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
                            {
                                string Id = obj.InsertQaMistakes(Convert.ToString((HttpContext.Current.Session["BookId"])) + "-1",
                                        Convert.ToInt32((HttpContext.Current.Session["userId"])), 1, mistakeNum,
                                        DateTime.Now, Convert.ToInt32(pageNum), comments.Trim());

                                XmlAttribute qaMistakeId = xmlDocOrigXml.CreateAttribute("QaMistakeId");
                                qaMistakeId.Value = Convert.ToString(Id);
                                node.SetAttributeNode(qaMistakeId);
                                break;
                            }
                        }
                    }//end xml foreach loop

                    xmlDocOrigXml.Save(mainXmlPath);

                    GlobalVar objGlobal = new GlobalVar();
                    objGlobal.PBPDocument = xmlDocOrigXml;
                    objGlobal.XMLPath = mainXmlPath.Replace(".xml", ".rhyw");
                    objGlobal.SaveXml();

                }//end main select line for
            }
        }
        catch (Exception)
        {

        }

        int detectedInjMistakes = 0;
        int detectedOtherMistakes = 0;

        string temp = comObj.GetTotalMistakesAll();

        string[] listMistakes = temp.Split(',');

        if (listMistakes != null && listMistakes.Length > 1)
        {
            detectedOtherMistakes = listMistakes[0] == "" ? 0 : Convert.ToInt32(listMistakes[0]);
            detectedInjMistakes = listMistakes[1] == "" ? 0 : Convert.ToInt32(listMistakes[1]);
        }

        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
        {
            //Update mistake count
            MyDBClass db = new MyDBClass();
            db.UpdateMistakeCount(Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1", Convert.ToInt32(HttpContext.Current.Session["userId"]),
                                  detectedInjMistakes, detectedOtherMistakes);
        }

        HttpContext.Current.Session["mistakeCounter"] = comObj.GetTotalMistakes();
        HttpContext.Current.Session["SelectedMistakeText"] = "";

        return pdfJsDivNumbers;
    }

    //Modified one
    //public static string LogMistakesInXml(string comments, string lineText, string pdfJsPageText)
    //{
    //    string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);

    //    if (String.IsNullOrEmpty(mainXmlPath))
    //        return "";

    //    MyDBClass obj = new MyDBClass();
    //    Common comObj = new Common();

    //    string pdfJsDivNumbers = string.Empty;

    //    XmlDocument xmlDocOrigXml = comObj.LoadXmlDocument(mainXmlPath);

    //    string attrName = "PDFmistake";

    //    string quizType = Convert.ToString(HttpContext.Current.Session["quizType"]);

    //    if (quizType != "")
    //    {
    //        if (quizType.Equals("Splitting"))
    //        {
    //            attrName = "PDFMergemistake";
    //        }
    //        else if (quizType.Equals("Merging"))
    //        {
    //            attrName = "PDFSplitmistake";
    //        }
    //        else if (quizType.Equals("Space"))
    //        {
    //            attrName = "PDFmistake";
    //        }
    //    }

    //    List<string> selectedText = new List<string>();
    //    string TextFromPDFJs = lineText;  //The original text that was selected for editing

    //    HttpContext.Current.Session["OriginalText"] = TextFromPDFJs;

    //    if (TextFromPDFJs == "")
    //        return "";

    //    List<string> processedXmlLines = new List<string>();

    //    var selectedLines = TextFromPDFJs.Split(new string[] { "\r\n" }, StringSplitOptions.None).Where(x => (!string.IsNullOrEmpty(x))).ToList();

    //    try
    //    {
    //        if ((selectedLines != null) && (selectedLines.Count > 0) && (xmlDocOrigXml != null))
    //        {
    //            //string attrName = "PDFmistake";
    //            string attrNameTest = "PDFmistakeTest";
    //            int mistakeNum = 0;
    //            bool check = false;
    //            bool isMistakeRemoved = false;
    //            bool nonInjectedErrors_Check = true;
    //            string pageNum = "";
    //            int lineCounter = 0;
    //            bool IsSelectedErrorLine = false;
    //            int lineNumber = 0;

    //            List<PdfJsLine> pdfJsLines = comObj.GetCurrentPdfJsPageLines(pdfJsPageText, Convert.ToInt32(SiteSession.MainCurrPage));
    //            if (pdfJsLines != null)
    //            {
    //                if (pdfJsLines.Count > 0)
    //                {
    //                    List<PdfJsLine> selectedPdfJsline = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(lineText))).ToList();
    //                    if (selectedPdfJsline != null)
    //                    {
    //                        if (selectedPdfJsline.Count > 0)
    //                        {
    //                            lineNumber = selectedPdfJsline[0].LineNum;
    //                            pdfJsDivNumbers = selectedPdfJsline[0].DivNum;

    //                            if (selectedLines.Count == 1)
    //                                selectedLines[0] = selectedPdfJsline[0].Text;
    //                        }
    //                    }
    //                    //lineNumber = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(lineText))).ToList()[0].LineNum;
    //                    //pdfJsDivNumbers = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(lineText))).ToList()[0].DivNum;
    //                }
    //            }

    //            var nodes = xmlDocOrigXml.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes.Count > 0 &&
    //                                                                                       x.Attributes["page"] != null &&
    //                                                                                       x.Attributes["page"].Value == Convert.ToString(SiteSession.MainCurrPage));


    //            //foreach (var line in selectedLines.ToList())
    //            for (int i = 0; i < selectedLines.Count; i++)
    //            {
    //                check = false;
    //                lineCounter = 0;

    //                int xmlLineCount = nodes.Count();
    //                int pdfJsLinesCount = pdfJsLines.Count;

    //                foreach (XmlElement node in nodes)
    //                {
    //                    if (!processedXmlLines.Contains(node.InnerText.Trim()))
    //                    {
    //                        mistakeNum = 0;
    //                        lineCounter++;

    //                        if (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage)))
    //                        {
    //                            //if ((RemoveWhitespace(node.InnerText).Equals(RemoveWhitespace(line))) && (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    //                            //if (IsContainsSameText(node.InnerText, line, node.Attributes["page"].Value) && (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))

    //                            if (xmlLineCount != pdfJsLinesCount)
    //                            {
    //                                if (node.ParentNode != null && (node.ParentNode.Name.Equals("col") || node.ParentNode.Name.Equals("head-col")))
    //                                {
    //                                    //string rowText = GetTableRowLine(node, selectedLines[line], node.Attributes["page"].Value);
    //                                    double colUry = node.Attributes["top"] != null ? Convert.ToDouble(node.Attributes["top"].Value) : 0;
    //                                    StringBuilder sbRowText = new StringBuilder();

    //                                    if (node.ParentNode.ParentNode != null)
    //                                    {
    //                                        XmlNode rowNode = node.ParentNode.ParentNode;
    //                                        var rowLines = rowNode.SelectNodes("//ln").Cast<XmlNode>()
    //                                                                .Where(x => Convert.ToDouble(x.Attributes["top"].Value) == colUry)
    //                                                                .ToList();

    //                                        if (rowLines != null && rowLines.Count >= 1)
    //                                        {
    //                                            bool isFirstLineMatched = false;

    //                                            foreach (XmlNode ln in rowLines)
    //                                            {
    //                                                sbRowText.Append(ln.InnerText + " ");

    //                                                if (IsContainsSameText(ln.InnerText, selectedLines[i], node.Attributes["page"].Value) &&
    //                                                    (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    //                                                {
    //                                                    IsSelectedErrorLine = true;
    //                                                    isFirstLineMatched = true;
    //                                                    processedXmlLines.Add(ln.InnerText.Trim());
    //                                                }
    //                                                else if (isFirstLineMatched)
    //                                                {
    //                                                    if (!selectedLines.Contains(ln.InnerText.Trim()))
    //                                                    {
    //                                                        selectedLines.Add(ln.InnerText.Trim());
    //                                                    }
    //                                                }
    //                                            }
    //                                        }
    //                                    }

    //                                    //if (!string.IsNullOrEmpty(Convert.ToString(sbRowText)))
    //                                    //{
    //                                    //    if (IsContainsSameText(node.InnerText, line, node.Attributes["page"].Value) &&
    //                                    // (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    //                                    //    {
    //                                    //        IsSelectedErrorLine = true;
    //                                    //    }
    //                                    //}
    //                                }
    //                                else
    //                                {
    //                                    if (IsContainsSameText(node.InnerText, selectedLines[i], node.Attributes["page"].Value) &&
    //                                        (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    //                                    {
    //                                        IsSelectedErrorLine = true;
    //                                    }
    //                                }

    //                                //if (IsContainsSameText(node.InnerText, line, node.Attributes["page"].Value) &&
    //                                //(node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    //                                //{
    //                                //    IsSelectedErrorLine = true;
    //                                //}
    //                            }
    //                            else if (lineCounter == lineNumber && xmlLineCount == pdfJsLinesCount)
    //                            {
    //                                IsSelectedErrorLine = true;
    //                            }

    //                            //if (lineCounter == lineNumber)
    //                            if (IsSelectedErrorLine)
    //                            {
    //                                IsSelectedErrorLine = false;
    //                                pageNum = node.Attributes["page"].Value;

    //                                //If task is test or one page test then log attrNameTest attribute and clear PDFmistake attribute value
    //                                if ((Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test")) ||
    //                                    (Convert.ToString(HttpContext.Current.Session["ComparisonTask"])
    //                                        .Equals("onepagetest")) ||
    //                                    (Convert.ToString(HttpContext.Current.Session["ComparisonTask"])
    //                                        .Equals("comparisonEntryTest")))
    //                                {
    //                                    if (node.HasAttribute(attrName))
    //                                    {
    //                                        node.SetAttribute(attrName, "");
    //                                    }

    //                                    if (!node.HasAttribute(attrNameTest))
    //                                    {
    //                                        XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrNameTest);

    //                                        newAttr.Value = Convert.ToString("1");
    //                                        node.SetAttributeNode(newAttr);
    //                                    }

    //                                    check = false;
    //                                }

    //                                    //In comparison task, lines which don't have correction/conversion tag and user log errors on these line then correction/conversion
    //                                    //tag is inserted into a new xml and this xml is send for qa inspection in structuring tool.
    //                                else if (!node.HasAttribute(attrName) ||
    //                                         (node.HasAttribute(attrName) &&
    //                                          node.Attributes["PDFmistake"].Value.Equals("undo")))
    //                                {
    //                                    if (node.Attributes["PDFmistake"] != null &&
    //                                        node.Attributes["PDFmistake"].Value.Equals("undo"))
    //                                    {
    //                                        // Remove the PDFmistake attribute.
    //                                        node.RemoveAttribute("PDFmistake");
    //                                    }

    //                                    XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrName);

    //                                    if (comObj.GetTotalMistakes() == 0)
    //                                        mistakeNum = 1;
    //                                    else
    //                                        mistakeNum = comObj.GetTotalMistakes() + 1;

    //                                    newAttr.Value = Convert.ToString(mistakeNum);
    //                                    node.SetAttributeNode(newAttr);

    //                                    if (node.Attributes["autoInjection"] != null)
    //                                    {
    //                                        node.RemoveAttribute("autoInjection");
    //                                        nonInjectedErrors_Check = false;
    //                                    }

    //                                    if (node.Attributes["PDFMergemistake"] != null)
    //                                    {
    //                                        node.SetAttribute("PDFMergemistake", "");
    //                                        nonInjectedErrors_Check = false;
    //                                    }

    //                                    if (node.Attributes["PDFSplitmistake"] != null)
    //                                    {
    //                                        node.SetAttribute("PDFSplitmistake", "");
    //                                        nonInjectedErrors_Check = false;
    //                                    }

    //                                    //correction="merge,"

    //                                    if (node.Attributes["correction"] != null)
    //                                    {
    //                                        node.SetAttribute("correction", "");
    //                                        nonInjectedErrors_Check = false;
    //                                    }

    //                                    //if (node.ParentNode.Attributes["correction"] != null)
    //                                    //{
    //                                    //    node.ParentNode.Attributes["correction"].Value = "";
    //                                    //    nonInjectedErrors_Check = false;
    //                                    //}

    //                                    //Removing split mistake attributes from both uparas
    //                                    if (node.ParentNode.Attributes["correction"] != null)
    //                                    {
    //                                        isMistakeRemoved = false;

    //                                        //Removing split mistake attributes (correction="merge,") from first upara
    //                                        if (node.ParentNode.Attributes["correction"].Value.Equals("merge,"))
    //                                        {
    //                                            node.ParentNode.Attributes["correction"].Value = "";
    //                                        }

    //                                        //Remove merge tag from previous para
    //                                        if (node.ParentNode.PreviousSibling != null &&
    //                                            node.ParentNode.PreviousSibling.Attributes["correction"] != null &&
    //                                            !isMistakeRemoved)
    //                                        {

    //                                            if (
    //                                                node.ParentNode.PreviousSibling.Attributes["correction"].Value
    //                                                    .Equals("merge,"))
    //                                            {
    //                                                //    node.ParentNode.PreviousSibling.Attributes["correction"].Value = "";
    //                                                //    isMistakeRemoved = true;

    //                                                if (node.ParentNode.PreviousSibling.ChildNodes != null &&
    //                                                    node.ParentNode.PreviousSibling.ChildNodes.Count > 0)
    //                                                {
    //                                                    node.ParentNode.PreviousSibling.Attributes["correction"].Value =
    //                                                        "";
    //                                                    isMistakeRemoved = true;
    //                                                }
    //                                                else
    //                                                {
    //                                                    if ((node.ParentNode.PreviousSibling.ChildNodes.Count == 0) &&
    //                                                        (node.ParentNode.PreviousSibling.Attributes["correction"] !=
    //                                                         null))
    //                                                        node.ParentNode.PreviousSibling.Attributes["correction"]
    //                                                            .Value = "";

    //                                                    if (node.ParentNode.PreviousSibling.PreviousSibling != null &&
    //                                                        node.ParentNode.PreviousSibling.PreviousSibling.Attributes[
    //                                                            "correction"] != null &&
    //                                                        !isMistakeRemoved)
    //                                                    {
    //                                                        node.ParentNode.PreviousSibling.PreviousSibling.Attributes[
    //                                                            "correction"].Value = "";
    //                                                        isMistakeRemoved = true;
    //                                                    }
    //                                                }
    //                                            }
    //                                        }

    //                                        //Remove merge tag from next para
    //                                        if (node.ParentNode.NextSibling != null &&
    //                                            node.ParentNode.NextSibling.Attributes["correction"] != null &&
    //                                            !isMistakeRemoved)
    //                                        {
    //                                            if (
    //                                                node.ParentNode.NextSibling.Attributes["correction"].Value.Equals(
    //                                                    "merge,"))
    //                                            {
    //                                                //    node.ParentNode.NextSibling.Attributes["correction"].Value = "";
    //                                                //}

    //                                                if (node.ParentNode.NextSibling.ChildNodes != null &&
    //                                                    node.ParentNode.NextSibling.ChildNodes.Count > 0)
    //                                                {
    //                                                    node.ParentNode.NextSibling.Attributes["correction"].Value = "";
    //                                                    //isMistakeRemoved = true;
    //                                                }
    //                                                else
    //                                                {
    //                                                    if ((node.ParentNode.NextSibling.ChildNodes.Count == 0) &&
    //                                                        (node.ParentNode.NextSibling.Attributes["correction"] !=
    //                                                         null))
    //                                                        node.ParentNode.NextSibling.Attributes["correction"].Value =
    //                                                            "";

    //                                                    if (node.ParentNode.NextSibling.NextSibling != null &&
    //                                                        node.ParentNode.NextSibling.NextSibling.Attributes[
    //                                                            "correction"] != null && !isMistakeRemoved)
    //                                                    {
    //                                                        node.ParentNode.NextSibling.NextSibling.Attributes[
    //                                                            "correction"].Value = "";
    //                                                        //isMistakeRemoved = true;
    //                                                    }
    //                                                }
    //                                            }
    //                                        }

    //                                        nonInjectedErrors_Check = false;
    //                                    }

    //                                    //Removing split mistake attributes from both sparas
    //                                    if (node.ParentNode.ParentNode.Attributes["correction"] != null)
    //                                    {
    //                                        isMistakeRemoved = false;

    //                                        //Removing split mistake attributes (correction="merge,") from first spara
    //                                        if (
    //                                            node.ParentNode.ParentNode.Attributes["correction"].Value.Equals(
    //                                                "merge,"))
    //                                        {
    //                                            node.ParentNode.ParentNode.Attributes["correction"].Value = "";
    //                                        }

    //                                        //Remove merge tag from previous spara
    //                                        if (node.ParentNode.ParentNode.PreviousSibling != null &&
    //                                            node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"] !=
    //                                            null &&
    //                                            !isMistakeRemoved)
    //                                        {
    //                                            //if (node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                            //{
    //                                            //    node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value = "";
    //                                            //    isMistakeRemoved = true;
    //                                            //}
    //                                            //node.ParentNode.ParentNode.PreviousSibling.ChildNodes[0].ChildNodes


    //                                            if (
    //                                                node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"]
    //                                                    .Value.Equals("merge,"))
    //                                            {
    //                                                //if (node.ParentNode.ParentNode.PreviousSibling.ChildNodes != null)
    //                                                //{
    //                                                if (node.ParentNode.ParentNode.PreviousSibling.ChildNodes.Count == 1 &&
    //                                                    node.ParentNode.ParentNode.PreviousSibling.ChildNodes[0]
    //                                                        .ChildNodes.Count > 0)
    //                                                {
    //                                                    node.ParentNode.ParentNode.PreviousSibling.Attributes[
    //                                                        "correction"].Value = "";
    //                                                }
    //                                                    //}
    //                                                else
    //                                                {
    //                                                    if (node.ParentNode.ParentNode.PreviousSibling.ChildNodes[0]
    //                                                            .ChildNodes.Count == 0 &&
    //                                                        node.ParentNode.ParentNode.PreviousSibling.Attributes[
    //                                                            "correction"] != null)
    //                                                        node.ParentNode.ParentNode.PreviousSibling.Attributes[
    //                                                            "correction"].Value = "";

    //                                                    if (node.ParentNode.ParentNode.PreviousSibling.PreviousSibling
    //                                                            .Attributes["correction"] != null)
    //                                                    {
    //                                                        if (node.ParentNode.ParentNode.PreviousSibling
    //                                                                .PreviousSibling.Attributes["correction"].Value
    //                                                                .Equals("merge,"))
    //                                                        {
    //                                                            node.ParentNode.ParentNode.PreviousSibling
    //                                                                .PreviousSibling.Attributes["correction"].Value = "";
    //                                                        }
    //                                                    }
    //                                                }
    //                                            }

    //                                        }

    //                                        //Remove merge tag from next spara
    //                                        if (node.ParentNode.ParentNode.NextSibling != null &&
    //                                            node.ParentNode.ParentNode.NextSibling.Attributes["correction"] != null &&
    //                                            !isMistakeRemoved)
    //                                        {
    //                                            //if (node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                            //{
    //                                            //    node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value = "";
    //                                            //}

    //                                            if (
    //                                                node.ParentNode.ParentNode.NextSibling.Attributes["correction"]
    //                                                    .Value.Equals("merge,"))
    //                                            {
    //                                                //if (node.ParentNode.ParentNode.NextSibling.Attributes["correction"].ChildNodes != null)
    //                                                //{
    //                                                //    node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value = "";
    //                                                //}

    //                                                if (node.ParentNode.ParentNode.NextSibling.ChildNodes.Count == 1 &&
    //                                                    node.ParentNode.ParentNode.NextSibling.ChildNodes[0].ChildNodes
    //                                                        .Count > 0)
    //                                                {
    //                                                    node.ParentNode.ParentNode.NextSibling.Attributes["correction"]
    //                                                        .Value = "";
    //                                                }
    //                                                else
    //                                                {
    //                                                    //if ((node.ParentNode.ParentNode.NextSibling.Attributes["correction"].ChildNodes == null) &&
    //                                                    //(node.ParentNode.ParentNode.NextSibling.Attributes["correction"] != null))
    //                                                    //    node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value = "";

    //                                                    if (
    //                                                        node.ParentNode.ParentNode.NextSibling.ChildNodes[0]
    //                                                            .ChildNodes.Count == 0 &&
    //                                                        node.ParentNode.ParentNode.NextSibling.Attributes[
    //                                                            "correction"] != null)
    //                                                        node.ParentNode.ParentNode.NextSibling.Attributes[
    //                                                            "correction"].Value = "";


    //                                                    if (
    //                                                        node.ParentNode.ParentNode.NextSibling.NextSibling
    //                                                            .Attributes["correction"] != null)
    //                                                    {
    //                                                        if (
    //                                                            node.ParentNode.ParentNode.NextSibling.NextSibling
    //                                                                .Attributes["correction"].Value.Equals("merge,"))
    //                                                        {
    //                                                            node.ParentNode.ParentNode.NextSibling.NextSibling
    //                                                                .Attributes["correction"].Value = "";
    //                                                        }
    //                                                    }
    //                                                }
    //                                            }
    //                                        }

    //                                        nonInjectedErrors_Check = false;
    //                                    }

    //                                    if (node.Attributes["conversion"] != null)
    //                                    {
    //                                        node.SetAttribute("conversion", "");
    //                                        nonInjectedErrors_Check = false;
    //                                    }

    //                                    if (node.Attributes["missing"] != null)
    //                                    {
    //                                        node.SetAttribute("missing", "");
    //                                        nonInjectedErrors_Check = false;
    //                                    }

    //                                    check = true;

    //                                    //If error is not of injected type then log correction/conversion attribute in a new xml which is saved before mistake injection task.
    //                                    if (nonInjectedErrors_Check)
    //                                    {
    //                                        LogMistakesInNewXml(lineText, pdfJsPageText, lineNumber);
    //                                    }
    //                                }

    //                                    //update existing error comments
    //                                else
    //                                {
    //                                    if (
    //                                        Convert.ToString(HttpContext.Current.Session["ComparisonTask"])
    //                                            .Equals("task"))
    //                                    {
    //                                        if (!string.IsNullOrEmpty(comments.Trim()) &&
    //                                            !string.IsNullOrEmpty(node.Attributes["PDFmistake"].Value))
    //                                        {
    //                                            string updateId =
    //                                                obj.InsertQaMistakes(
    //                                                    Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1",
    //                                                    Convert.ToInt32(HttpContext.Current.Session["userId"]), 1,
    //                                                    Convert.ToInt32(node.Attributes["PDFmistake"].Value),
    //                                                    DateTime.Now, Convert.ToInt32(pageNum), comments.Trim());
    //                                        }
    //                                    }
    //                                }
    //                            }//end IsSelectedErrorLine
    //                        }
    //                    }

    //                    if (check)
    //                    {
    //                        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    //                        {
    //                            string Id = obj.InsertQaMistakes(Convert.ToString((HttpContext.Current.Session["BookId"])) + "-1",
    //                                    Convert.ToInt32((HttpContext.Current.Session["userId"])), 1, mistakeNum,
    //                                    DateTime.Now, Convert.ToInt32(pageNum), comments.Trim());

    //                            XmlAttribute qaMistakeId = xmlDocOrigXml.CreateAttribute("QaMistakeId");
    //                            qaMistakeId.Value = Convert.ToString(Id);
    //                            node.SetAttributeNode(qaMistakeId);
    //                            break;
    //                        }
    //                    }
    //                }//end xml foreach loop

    //                xmlDocOrigXml.Save(mainXmlPath);

    //                GlobalVar objGlobal = new GlobalVar();
    //                objGlobal.PBPDocument = xmlDocOrigXml;
    //                objGlobal.XMLPath = mainXmlPath.Replace(".xml", ".rhyw");
    //                objGlobal.SaveXml();
    //                ////XmlDocument xmlDoc = new XmlDocument(); aamir
    //                ////xmlDoc = objGlobal.PBPDocument;

    //            }//end main select line for
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }

    //    int detectedInjMistakes = 0;
    //    int detectedOtherMistakes = 0;

    //    string temp = comObj.GetTotalMistakesAll();

    //    string[] listMistakes = temp.Split(',');

    //    if (listMistakes != null && listMistakes.Length > 1)
    //    {
    //        detectedOtherMistakes = listMistakes[0] == "" ? 0 : Convert.ToInt32(listMistakes[0]);
    //        detectedInjMistakes = listMistakes[1] == "" ? 0 : Convert.ToInt32(listMistakes[1]);
    //    }

    //    if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    //    {
    //        //Update mistake count
    //        MyDBClass db = new MyDBClass();
    //        db.UpdateMistakeCount(Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1", Convert.ToInt32(HttpContext.Current.Session["userId"]),
    //                              detectedInjMistakes, detectedOtherMistakes);
    //    }

    //    HttpContext.Current.Session["mistakeCounter"] = comObj.GetTotalMistakes();
    //    HttpContext.Current.Session["SelectedMistakeText"] = "";

    //    return pdfJsDivNumbers;
    //}

    //old method
    //public static string LogMistakesInXml(string comments, string lineText, string pdfJsPageText)
    //{
    //    string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);

    //    if (String.IsNullOrEmpty(mainXmlPath))
    //        return "";

    //    MyDBClass obj = new MyDBClass();
    //    Common comObj = new Common();

    //    string pdfJsDivNumbers = string.Empty;

    //    XmlDocument xmlDocOrigXml = comObj.LoadXmlDocument(mainXmlPath);

    //    string attrName = "PDFmistake";

    //    string quizType = Convert.ToString(HttpContext.Current.Session["quizType"]);

    //    if (quizType != "")
    //    {
    //        if (quizType.Equals("Splitting"))
    //        {
    //            attrName = "PDFMergemistake";
    //        }
    //        else if (quizType.Equals("Merging"))
    //        {
    //            attrName = "PDFSplitmistake";
    //        }
    //        else if (quizType.Equals("Space"))
    //        {
    //            attrName = "PDFmistake";
    //        }
    //    }

    //    List<string> selectedText = new List<string>();
    //    string TextFromPDFJs = lineText;  //The original text that was selected for editing

    //    HttpContext.Current.Session["OriginalText"] = TextFromPDFJs;

    //    if (TextFromPDFJs == "")
    //        return "";

    //    var selectedLines = TextFromPDFJs.Split(new string[] { "\r\n" }, StringSplitOptions.None);

    //    selectedLines = selectedLines.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

    //    try
    //    {
    //        if ((selectedLines != null) && (selectedLines.Length > 0) && (xmlDocOrigXml != null))
    //        {
    //            //string attrName = "PDFmistake";
    //            string attrNameTest = "PDFmistakeTest";
    //            int mistakeNum = 0;
    //            bool check = false;
    //            bool isMistakeRemoved = false;
    //            bool nonInjectedErrors_Check = true;
    //            string pageNum = "";
    //            int lineCounter = 0;
    //            bool IsSelectedErrorLine = false;
    //            int lineNumber = 0;

    //            List<PdfJsLine> pdfJsLines = comObj.GetCurrentPdfJsPageLines(pdfJsPageText, Convert.ToInt32(SiteSession.MainCurrPage));
    //            if (pdfJsLines != null)
    //            {
    //                if (pdfJsLines.Count > 0)
    //                {
    //                    List<PdfJsLine> selectedPdfJsline = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(lineText))).ToList();
    //                    if (selectedPdfJsline != null)
    //                    {
    //                        if (selectedPdfJsline.Count > 0)
    //                        {
    //                            lineNumber = selectedPdfJsline[0].LineNum;
    //                            pdfJsDivNumbers = selectedPdfJsline[0].DivNum;
    //                        }
    //                    }
    //                    //lineNumber = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(lineText))).ToList()[0].LineNum;
    //                    //pdfJsDivNumbers = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(lineText))).ToList()[0].DivNum;
    //                }
    //            }

    //            //var nodes = xmlDocOrigXml.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes.Count > 0 &&
    //            //                                                                           x.Attributes["page"] != null &&
    //            //                                                                           x.Attributes["page"].Value == Convert.ToString(SiteSession.MainCurrPage));
    //            foreach (var line in selectedLines)
    //            {
    //                check = false;
    //                lineCounter = 0;

    //                var nodes = xmlDocOrigXml.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes.Count > 0 &&
    //                                                                                        x.Attributes["page"] != null &&
    //                                                                                        x.Attributes["page"].Value == Convert.ToString(SiteSession.MainCurrPage));

    //                XmlElement root = xmlDocOrigXml.DocumentElement;


    //                //pdfJsDivNumbers = GetSelectedLineNum(lineText, pdfJsPageText, Convert.ToString(SiteSession.MainCurrPage), out lineNumber);

    //                //List<PdfJsLine> pdfJsLines = comObj.GetPdfJsPageLines(Convert.ToString(SiteSession.MainCurrPage), pdfJsPageText);

    //                //List<PdfJsLine> pdfJsLines = comObj.GetCurrentPdfJsPageLines(pdfJsPageText, Convert.ToInt32(SiteSession.MainCurrPage));
    //                //if (pdfJsLines != null)
    //                //{
    //                //    if (pdfJsLines.Count > 0)
    //                //    {
    //                //        lineNumber = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(lineText))).ToList()[0].LineNum;
    //                //        pdfJsDivNumbers = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(lineText))).ToList()[0].DivNum;
    //                //    }
    //                //}

    //                int xmlLineCount = nodes.Count();
    //                int pdfJsLinesCount = pdfJsLines.Count;

    //                foreach (XmlElement node in nodes)
    //                {
    //                    mistakeNum = 0;
    //                    lineCounter++;

    //                    if (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage)))
    //                    {
    //                        //if ((RemoveWhitespace(node.InnerText).Equals(RemoveWhitespace(line))) && (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    //                        //if (IsContainsSameText(node.InnerText, line, node.Attributes["page"].Value) && (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))

    //                        if (xmlLineCount != pdfJsLinesCount)
    //                        {
    //                            if (node.ParentNode != null && node.ParentNode.Name.Equals("col"))
    //                            {
    //                                //string rowText = GetTableRowLine(node, selectedLines[line], node.Attributes["page"].Value);
    //                                string colUry = node.Attributes["top"] != null ? node.Attributes["top"].Value : "";
    //                                StringBuilder sbRowText = new StringBuilder();

    //                                if (node.ParentNode.ParentNode != null)
    //                                {
    //                                    XmlNode rowNode = node.ParentNode.ParentNode;
    //                                    var rowLines = rowNode.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes["top"].Value == colUry).ToList();

    //                                    if (rowLines != null)
    //                                    {
    //                                        foreach (XmlNode ln in rowLines)
    //                                        {
    //                                            sbRowText.Append(ln.InnerText + " ");
    //                                        }
    //                                    }
    //                                }

    //                                if (!string.IsNullOrEmpty(Convert.ToString(sbRowText)))
    //                                {
    //                                    if (IsContainsSameText(node.InnerText, line, node.Attributes["page"].Value) &&
    //                                 (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    //                                    {
    //                                        IsSelectedErrorLine = true;
    //                                    }
    //                                }
    //                            }
    //                            else
    //                            {
    //                                if (IsContainsSameText(node.InnerText, line, node.Attributes["page"].Value) &&
    //                                   (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    //                                {
    //                                    IsSelectedErrorLine = true;
    //                                }
    //                            }

    //                            //if (IsContainsSameText(node.InnerText, line, node.Attributes["page"].Value) &&
    //                            //(node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    //                            //{
    //                            //    IsSelectedErrorLine = true;
    //                            //}
    //                        }
    //                        else if (lineCounter == lineNumber && xmlLineCount == pdfJsLinesCount)
    //                        {
    //                            IsSelectedErrorLine = true;
    //                        }

    //                        //if (lineCounter == lineNumber)
    //                        if (IsSelectedErrorLine)
    //                        {
    //                            IsSelectedErrorLine = false;
    //                            pageNum = node.Attributes["page"].Value;

    //                            //If task is test or one page test then log attrNameTest attribute and clear PDFmistake attribute value
    //                            if ((Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test")) ||
    //                                (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest")) ||
    //                                (Convert.ToString(HttpContext.Current.Session["ComparisonTask"])
    //                                    .Equals("comparisonEntryTest")))
    //                            {
    //                                if (node.HasAttribute(attrName))
    //                                {
    //                                    node.SetAttribute(attrName, "");
    //                                }

    //                                if (!node.HasAttribute(attrNameTest))
    //                                {
    //                                    XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrNameTest);

    //                                    newAttr.Value = Convert.ToString("1");
    //                                    node.SetAttributeNode(newAttr);
    //                                }

    //                                check = false;
    //                            }

    //                                //In comparison task, lines which don't have correction/conversion tag and user log errors on these line then correction/conversion
    //                            //tag is inserted into a new xml and this xml is send for qa inspection in structuring tool.
    //                            else if (!node.HasAttribute(attrName) ||
    //                                     (node.HasAttribute(attrName) && node.Attributes["PDFmistake"].Value.Equals("undo")))
    //                            {
    //                                if (node.Attributes["PDFmistake"] != null &&
    //                                    node.Attributes["PDFmistake"].Value.Equals("undo"))
    //                                {
    //                                    // Remove the PDFmistake attribute.
    //                                    node.RemoveAttribute("PDFmistake");
    //                                }

    //                                XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrName);

    //                                if (comObj.GetTotalMistakes() == 0)
    //                                    mistakeNum = 1;
    //                                else
    //                                    mistakeNum = comObj.GetTotalMistakes() + 1;

    //                                newAttr.Value = Convert.ToString(mistakeNum);
    //                                node.SetAttributeNode(newAttr);

    //                                if (node.Attributes["autoInjection"] != null)
    //                                {
    //                                    node.RemoveAttribute("autoInjection");
    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                if (node.Attributes["PDFMergemistake"] != null)
    //                                {
    //                                    node.SetAttribute("PDFMergemistake", "");
    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                if (node.Attributes["PDFSplitmistake"] != null)
    //                                {
    //                                    node.SetAttribute("PDFSplitmistake", "");
    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                //correction="merge,"

    //                                if (node.Attributes["correction"] != null)
    //                                {
    //                                    node.SetAttribute("correction", "");
    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                //if (node.ParentNode.Attributes["correction"] != null)
    //                                //{
    //                                //    node.ParentNode.Attributes["correction"].Value = "";
    //                                //    nonInjectedErrors_Check = false;
    //                                //}

    //                                //Removing split mistake attributes from both uparas
    //                                if (node.ParentNode.Attributes["correction"] != null)
    //                                {
    //                                    isMistakeRemoved = false;

    //                                    //Removing split mistake attributes (correction="merge,") from first upara
    //                                    if (node.ParentNode.Attributes["correction"].Value.Equals("merge,"))
    //                                    {
    //                                        node.ParentNode.Attributes["correction"].Value = "";
    //                                    }

    //                                    //Remove merge tag from previous para
    //                                    if (node.ParentNode.PreviousSibling != null &&
    //                                        node.ParentNode.PreviousSibling.Attributes["correction"] != null &&
    //                                        !isMistakeRemoved)
    //                                    {

    //                                        if (node.ParentNode.PreviousSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                        {
    //                                            //    node.ParentNode.PreviousSibling.Attributes["correction"].Value = "";
    //                                            //    isMistakeRemoved = true;

    //                                            if (node.ParentNode.PreviousSibling.ChildNodes != null &&
    //                                                node.ParentNode.PreviousSibling.ChildNodes.Count > 0)
    //                                            {
    //                                                node.ParentNode.PreviousSibling.Attributes["correction"].Value = "";
    //                                                isMistakeRemoved = true;
    //                                            }
    //                                            else
    //                                            {
    //                                                if ((node.ParentNode.PreviousSibling.ChildNodes.Count == 0) &&
    //                                                    (node.ParentNode.PreviousSibling.Attributes["correction"] != null))
    //                                                    node.ParentNode.PreviousSibling.Attributes["correction"].Value = "";

    //                                                if (node.ParentNode.PreviousSibling.PreviousSibling != null &&
    //                                                    node.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"] != null &&
    //                                                    !isMistakeRemoved)
    //                                                {
    //                                                    node.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"].Value = "";
    //                                                    isMistakeRemoved = true;
    //                                                }
    //                                            }
    //                                        }
    //                                    }

    //                                    //Remove merge tag from next para
    //                                    if (node.ParentNode.NextSibling != null &&
    //                                        node.ParentNode.NextSibling.Attributes["correction"] != null &&
    //                                        !isMistakeRemoved)
    //                                    {
    //                                        if (node.ParentNode.NextSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                        {
    //                                            //    node.ParentNode.NextSibling.Attributes["correction"].Value = "";
    //                                            //}

    //                                            if (node.ParentNode.NextSibling.ChildNodes != null &&
    //                                                node.ParentNode.NextSibling.ChildNodes.Count > 0)
    //                                            {
    //                                                node.ParentNode.NextSibling.Attributes["correction"].Value = "";
    //                                                //isMistakeRemoved = true;
    //                                            }
    //                                            else
    //                                            {
    //                                                if ((node.ParentNode.NextSibling.ChildNodes.Count == 0) &&
    //                                                    (node.ParentNode.NextSibling.Attributes["correction"] != null))
    //                                                    node.ParentNode.NextSibling.Attributes["correction"].Value = "";

    //                                                if (node.ParentNode.NextSibling.NextSibling != null &&
    //                                                    node.ParentNode.NextSibling.NextSibling.Attributes["correction"] != null && !isMistakeRemoved)
    //                                                {
    //                                                    node.ParentNode.NextSibling.NextSibling.Attributes["correction"].Value = "";
    //                                                    //isMistakeRemoved = true;
    //                                                }
    //                                            }
    //                                        }
    //                                    }

    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                //Removing split mistake attributes from both sparas
    //                                if (node.ParentNode.ParentNode.Attributes["correction"] != null)
    //                                {
    //                                    isMistakeRemoved = false;

    //                                    //Removing split mistake attributes (correction="merge,") from first spara
    //                                    if (node.ParentNode.ParentNode.Attributes["correction"].Value.Equals("merge,"))
    //                                    {
    //                                        node.ParentNode.ParentNode.Attributes["correction"].Value = "";
    //                                    }

    //                                    //Remove merge tag from previous spara
    //                                    if (node.ParentNode.ParentNode.PreviousSibling != null &&
    //                                        node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"] != null &&
    //                                        !isMistakeRemoved)
    //                                    {
    //                                        //if (node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                        //{
    //                                        //    node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value = "";
    //                                        //    isMistakeRemoved = true;
    //                                        //}
    //                                        //node.ParentNode.ParentNode.PreviousSibling.ChildNodes[0].ChildNodes


    //                                        if (node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                        {
    //                                            //if (node.ParentNode.ParentNode.PreviousSibling.ChildNodes != null)
    //                                            //{
    //                                            if (node.ParentNode.ParentNode.PreviousSibling.ChildNodes.Count == 1 &&
    //                                                node.ParentNode.ParentNode.PreviousSibling.ChildNodes[0].ChildNodes.Count > 0)
    //                                            {
    //                                                node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value = "";
    //                                            }
    //                                            //}
    //                                            else
    //                                            {
    //                                                if (node.ParentNode.ParentNode.PreviousSibling.ChildNodes[0].ChildNodes.Count == 0 &&
    //                                                    node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"] != null)
    //                                                    node.ParentNode.ParentNode.PreviousSibling.Attributes["correction"].Value = "";

    //                                                if (node.ParentNode.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"] != null)
    //                                                {
    //                                                    if (node.ParentNode.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                                    {
    //                                                        node.ParentNode.ParentNode.PreviousSibling.PreviousSibling.Attributes["correction"].Value = "";
    //                                                    }
    //                                                }
    //                                            }
    //                                        }

    //                                    }

    //                                    //Remove merge tag from next spara
    //                                    if (node.ParentNode.ParentNode.NextSibling != null &&
    //                                        node.ParentNode.ParentNode.NextSibling.Attributes["correction"] != null &&
    //                                        !isMistakeRemoved)
    //                                    {
    //                                        //if (node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                        //{
    //                                        //    node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value = "";
    //                                        //}

    //                                        if (node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                        {
    //                                            //if (node.ParentNode.ParentNode.NextSibling.Attributes["correction"].ChildNodes != null)
    //                                            //{
    //                                            //    node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value = "";
    //                                            //}

    //                                            if (node.ParentNode.ParentNode.NextSibling.ChildNodes.Count == 1 &&
    //                                               node.ParentNode.ParentNode.NextSibling.ChildNodes[0].ChildNodes.Count > 0)
    //                                            {
    //                                                node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value = "";
    //                                            }
    //                                            else
    //                                            {
    //                                                //if ((node.ParentNode.ParentNode.NextSibling.Attributes["correction"].ChildNodes == null) &&
    //                                                //(node.ParentNode.ParentNode.NextSibling.Attributes["correction"] != null))
    //                                                //    node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value = "";

    //                                                if (node.ParentNode.ParentNode.NextSibling.ChildNodes[0].ChildNodes.Count == 0 &&
    //                                                        node.ParentNode.ParentNode.NextSibling.Attributes["correction"] != null)
    //                                                    node.ParentNode.ParentNode.NextSibling.Attributes["correction"].Value = "";


    //                                                if (node.ParentNode.ParentNode.NextSibling.NextSibling.Attributes["correction"] != null)
    //                                                {
    //                                                    if (node.ParentNode.ParentNode.NextSibling.NextSibling.Attributes["correction"].Value.Equals("merge,"))
    //                                                    {
    //                                                        node.ParentNode.ParentNode.NextSibling.NextSibling.Attributes["correction"].Value = "";
    //                                                    }
    //                                                }
    //                                            }
    //                                        }
    //                                    }

    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                if (node.Attributes["conversion"] != null)
    //                                {
    //                                    node.SetAttribute("conversion", "");
    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                if (node.Attributes["missing"] != null)
    //                                {
    //                                    node.SetAttribute("missing", "");
    //                                    nonInjectedErrors_Check = false;
    //                                }

    //                                check = true;

    //                                //If error is not of injected type then log correction/conversion attribute in a new xml which is saved before mistake injection task.
    //                                if (nonInjectedErrors_Check)
    //                                {
    //                                    LogMistakesInNewXml(lineText, pdfJsPageText, lineNumber);
    //                                }
    //                            }

    //                                //update existing error comments
    //                            else
    //                            {
    //                                if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    //                                {
    //                                    if (!string.IsNullOrEmpty(comments.Trim()) &&
    //                                        !string.IsNullOrEmpty(node.Attributes["PDFmistake"].Value))
    //                                    {
    //                                        string updateId =
    //                                            obj.InsertQaMistakes(
    //                                                Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1",
    //                                                Convert.ToInt32(HttpContext.Current.Session["userId"]), 1,
    //                                                Convert.ToInt32(node.Attributes["PDFmistake"].Value),
    //                                                DateTime.Now, Convert.ToInt32(pageNum), comments.Trim());
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }

    //                    if (check)
    //                    {
    //                        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    //                        {
    //                            string Id = obj.InsertQaMistakes(Convert.ToString((HttpContext.Current.Session["BookId"])) + "-1",
    //                                    Convert.ToInt32((HttpContext.Current.Session["userId"])), 1, mistakeNum,
    //                                    DateTime.Now, Convert.ToInt32(pageNum), comments.Trim());

    //                            XmlAttribute qaMistakeId = xmlDocOrigXml.CreateAttribute("QaMistakeId");
    //                            qaMistakeId.Value = Convert.ToString(Id);
    //                            node.SetAttributeNode(qaMistakeId);
    //                            break;
    //                        }
    //                    }
    //                }

    //                xmlDocOrigXml.Save(mainXmlPath);

    //                GlobalVar objGlobal = new GlobalVar();
    //                objGlobal.PBPDocument = xmlDocOrigXml;
    //                objGlobal.XMLPath = mainXmlPath.Replace(".xml", ".rhyw");
    //                objGlobal.SaveXml();
    //                ////XmlDocument xmlDoc = new XmlDocument(); aamir
    //                ////xmlDoc = objGlobal.PBPDocument;
    //            }
    //        }
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }

    //    int detectedInjMistakes = 0;
    //    int detectedOtherMistakes = 0;

    //    string temp = comObj.GetTotalMistakesAll();

    //    string[] listMistakes = temp.Split(',');

    //    if (listMistakes != null && listMistakes.Length > 1)
    //    {
    //        detectedOtherMistakes = listMistakes[0] == "" ? 0 : Convert.ToInt32(listMistakes[0]);
    //        detectedInjMistakes = listMistakes[1] == "" ? 0 : Convert.ToInt32(listMistakes[1]);
    //    }

    //    if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    //    {
    //        //Update mistake count
    //        MyDBClass db = new MyDBClass();
    //        db.UpdateMistakeCount(Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1", Convert.ToInt32(HttpContext.Current.Session["userId"]),
    //                              detectedInjMistakes, detectedOtherMistakes);
    //    }

    //    HttpContext.Current.Session["mistakeCounter"] = comObj.GetTotalMistakes();
    //    HttpContext.Current.Session["SelectedMistakeText"] = "";

    //    return pdfJsDivNumbers;
    //}

    #endregion

    public static string UndoMistakesInXml(string comments, string lineText, string pdfJsPageText)
    {
        string mainXmlPath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);
        if (String.IsNullOrEmpty(mainXmlPath)) return "";

        MyDBClass obj = new MyDBClass();
        Common comObj = new Common();
        XmlDocument xmlDocOrigXml = comObj.LoadXmlDocument(mainXmlPath);

        string attrName = "PDFmistake";

        List<string> selectedText = new List<string>();
        string TextFromPDFJs = lineText;  //The original text that was selected for editing

        HttpContext.Current.Session["OriginalText"] = TextFromPDFJs;

        if (TextFromPDFJs == "") return "";

        string pdfJsDivs = "";
        bool IsSelectedErrorLine = false;

        var selectedLines = TextFromPDFJs.Split(new string[] { "\r\n" }, StringSplitOptions.None);
        selectedLines = selectedLines.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

        if ((selectedLines != null) && (selectedLines.Length > 0) && (xmlDocOrigXml != null))
        {
            int mistakeNum = 0;
            bool check = false;
            bool isMistakeRemoved = false;
            bool nonInjectedErrors_Check = true;
            string pageNum = "";
            int lineCounter = 0;

            foreach (var line in selectedLines)
            {
                check = false;

                //XmlNodeList nodes = xmlDocOrigXml.SelectNodes("//ln");
                //XmlElement root = xmlDocOrigXml.DocumentElement;

                var nodes = xmlDocOrigXml.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes.Count > 0 &&
                                                                                           x.Attributes["page"] != null &&
                                                                                           x.Attributes["page"].Value == Convert.ToString(SiteSession.MainCurrPage));

                XmlElement root = xmlDocOrigXml.DocumentElement;

                //int lineNum;
                //pdfJsDivs = GetSelectedLineNum(lineText, pdfJsPageText, Convert.ToString(SiteSession.MainCurrPage), out lineNum);

                int lineNumber = 0;

                //pdfJsDivNumbers = GetSelectedLineNum(lineText, pdfJsPageText, Convert.ToString(SiteSession.MainCurrPage), out lineNumber);

                //List<PdfJsLine> pdfJsLines = comObj.GetPdfJsPageLines(Convert.ToString(SiteSession.MainCurrPage), pdfJsPageText);
                List<PdfJsLine> pdfJsLines = comObj.GetCurrentPdfJsPageLines(pdfJsPageText, Convert.ToInt32(SiteSession.MainCurrPage));
                if (pdfJsLines != null)
                {
                    if (pdfJsLines.Count > 0)
                    {
                        //lineNumber = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(lineText))).ToList()[0].LineNum;
                        //pdfJsDivs = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(lineText))).ToList()[0].DivNum;

                        var pdfJsList = pdfJsLines.Where(x => RemoveWhiteSpace(x.Text).Equals(RemoveWhiteSpace(lineText))).ToList();
                        if (pdfJsList != null && pdfJsList.Count > 0)
                        {
                            lineNumber = pdfJsList[0].LineNum;
                            pdfJsDivs = pdfJsList[0].DivNum;
                        }
                    }
                }

                int xmlLineCount = nodes.Count();
                int pdfJsLinesCount = pdfJsLines.Count;

                foreach (XmlElement node in nodes)
                {
                    mistakeNum = 0;
                    lineCounter++;

                    //if ((RemoveWhitespace(node.InnerText).Equals(RemoveWhitespace(line))) && (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                    //if (IsContainsSameText(node.InnerText, line, "") && (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))

                    if (xmlLineCount != pdfJsLinesCount)
                    {
                        //if (IsContainsSameText(node.InnerText, line, node.Attributes["page"].Value) &&
                        //       (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))

                        if (IsContainsSameText(node.InnerText, line) &&
                               (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                        {
                            IsSelectedErrorLine = true;
                        }
                    }
                    else if (lineCounter == lineNumber && xmlLineCount == pdfJsLinesCount)
                    {
                        IsSelectedErrorLine = true;
                    }

                    //if (lineCounter == lineNumber)
                    if (IsSelectedErrorLine)
                    {
                        IsSelectedErrorLine = false;

                        //if (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage)))
                        //{
                        //    if (lineCounter == lineNumber)
                        //    {
                        pageNum = node.Attributes["page"].Value;

                        //If task then clear PDFmistake attribute value
                        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
                        {
                            if (node.HasAttribute(attrName))
                            {
                                if (!node.Attributes[attrName].Value.Equals("undo"))
                                {
                                    node.SetAttribute(attrName, "undo");
                                    break;
                                }
                            }
                        }
                    }
                }

                xmlDocOrigXml.Save(mainXmlPath);

                Common commObj = new Common();
                GlobalVar objGlobal = new GlobalVar();
                objGlobal.PBPDocument = xmlDocOrigXml;
                objGlobal.XMLPath = mainXmlPath.Replace(".xml", ".rhyw");
                objGlobal.SaveXml();
                commObj.xmlDoc = xmlDocOrigXml;
            }
        }

        int totalMistakes = comObj.GetTotalMistakes();

        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
        {
            //Update mistake count
            MyDBClass db = new MyDBClass();
            db.UpdateMistakeCount(Convert.ToString(HttpContext.Current.Session["BookId"]) + "-1", Convert.ToInt32(HttpContext.Current.Session["userId"]),
                totalMistakes, 0);
        }

        HttpContext.Current.Session["mistakeCounter"] = comObj.GetTotalMistakes();
        HttpContext.Current.Session["SelectedMistakeText"] = "";

        return pdfJsDivs;
    }

    public static void LogMistakesInNewXml(string lineText, string pdfJsPageText, int lineNumber)
    {
        string xmlFilePath = Common.GetDirectoryPath() + Convert.ToString(HttpContext.Current.Session["MainBook"]) + "/" + Convert.ToString(HttpContext.Current.Session["MainBook"]) +
                                "-1/TaggingUntagged" + "/" + Convert.ToString(HttpContext.Current.Session["MainBook"]) + "-1_actual.rhyw";

        if ((String.IsNullOrEmpty(xmlFilePath)) || (!File.Exists(xmlFilePath)))
            return;

        MyDBClass obj = new MyDBClass();
        Common commObj = new Common();
        commObj.LoadXml(xmlFilePath);

        XmlDocument xmlDocOrigXml = commObj.xmlDoc;

        string attrName = "PDFmistake";

        List<string> selectedText = new List<string>();
        string TextFromPDFJs = lineText;  //The original text that was selected for editing

        HttpContext.Current.Session["OriginalText"] = TextFromPDFJs;

        if (TextFromPDFJs == "")
            return;

        var selectedLines = TextFromPDFJs.Split(new string[] { "\r\n" }, StringSplitOptions.None);

        selectedLines = selectedLines.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

        if ((selectedLines != null) && (selectedLines.Length > 0))
        {
            int mistakeNum = 0;
            bool check = false;
            string pageNum = "";
            int lineCounter = 0;

            foreach (var line in selectedLines)
            {
                check = false;

                //XmlNodeList nodes = xmlDocOrigXml.SelectNodes("//ln");
                //XmlElement root = xmlDocOrigXml.DocumentElement;

                var nodes = xmlDocOrigXml.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes.Count > 0 &&
                                                            x.Attributes["page"].Value == Convert.ToString(SiteSession.MainCurrPage));

                XmlElement root = xmlDocOrigXml.DocumentElement;
                //int lineNum;

                //string pdfJsDivs = GetSelectedLineNum(lineText, pdfJsPageText, Convert.ToString(SiteSession.MainCurrPage), out lineNum);

                foreach (XmlElement node in nodes)
                {
                    mistakeNum = 0;
                    lineCounter++;

                    //if ((RemoveWhitespace(node.InnerText).Equals(RemoveWhitespace(line))) && (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
                    if (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage)))
                    {
                        if (lineCounter == lineNumber)
                        {
                            //pageNum = node.Attributes["page"].Value;

                            if (!node.HasAttribute(attrName))
                            {
                                XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrName);

                                if (commObj.GetTotalMistakes() == 0)
                                    mistakeNum = 1;
                                else
                                    mistakeNum = commObj.GetTotalMistakes() + 1;

                                newAttr.Value = Convert.ToString(mistakeNum);
                                node.SetAttributeNode(newAttr);
                            }
                        }
                    }
                }

                //xmlDocOrigXml.Save(xmlFilePath);

                GlobalVar objGlobal = new GlobalVar();
                objGlobal.PBPDocument = xmlDocOrigXml;
                objGlobal.XMLPath = xmlFilePath.Replace(".xml", ".rhyw");
                objGlobal.SaveXml();
            }
        }
    }

    //public static void LogMistakesInNewXml(string lineText)
    //{
    //    string xmlFilePath = Common.GetDirectoryPath() + Convert.ToString(HttpContext.Current.Session["MainBook"]) + "/" + Convert.ToString(HttpContext.Current.Session["MainBook"]) +
    //                            "-1/TaggingUntagged" + "/" + Convert.ToString(HttpContext.Current.Session["MainBook"]) + "-1_actual.rhyw";

    //    if ((String.IsNullOrEmpty(xmlFilePath)) || (!File.Exists(xmlFilePath)))
    //        return;

    //    MyDBClass obj = new MyDBClass();
    //    Common commObj = new Common();
    //    commObj.LoadXml(xmlFilePath);

    //    XmlDocument xmlDocOrigXml = commObj.xmlDoc;

    //    string attrName = "PDFmistake";

    //    List<string> selectedText = new List<string>();
    //    string TextFromPDFJs = lineText;  //The original text that was selected for editing

    //    HttpContext.Current.Session["OriginalText"] = TextFromPDFJs;

    //    if (TextFromPDFJs == "")
    //        return;

    //    var selectedLines = TextFromPDFJs.Split(new string[] { "\r\n" }, StringSplitOptions.None);

    //    selectedLines = selectedLines.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

    //    if ((selectedLines != null) && (selectedLines.Length > 0))
    //    {
    //        int mistakeNum = 0;
    //        bool check = false;
    //        string pageNum = "";

    //        foreach (var line in selectedLines)
    //        {
    //            check = false;

    //            XmlNodeList nodes = xmlDocOrigXml.SelectNodes("//ln");

    //            XmlElement root = xmlDocOrigXml.DocumentElement;

    //            foreach (XmlElement node in nodes)
    //            {
    //                mistakeNum = 0;

    //                if ((RemoveWhitespace(node.InnerText).Equals(RemoveWhitespace(line))) && (node.Attributes["page"].Value.Equals(Convert.ToString(SiteSession.MainCurrPage))))
    //                {
    //                    pageNum = node.Attributes["page"].Value;

    //                    if (!node.HasAttribute(attrName))
    //                    {
    //                        XmlAttribute newAttr = xmlDocOrigXml.CreateAttribute(attrName);

    //                        if (commObj.GetTotalMistakes() == 0)
    //                            mistakeNum = 1;
    //                        else
    //                            mistakeNum = commObj.GetTotalMistakes() + 1;

    //                        newAttr.Value = Convert.ToString(mistakeNum);
    //                        node.SetAttributeNode(newAttr);
    //                    }
    //                }
    //            }

    //            //xmlDocOrigXml.Save(xmlFilePath);

    //            GlobalVar objGlobal = new GlobalVar();
    //            objGlobal.PBPDocument = xmlDocOrigXml;
    //            objGlobal.XMLPath = xmlFilePath.Replace(".xml", ".rhyw");
    //            objGlobal.SaveXml();
    //        }
    //    }
    //}

    public static string RemoveWhitespace(string input)
    {
        StringBuilder output = new StringBuilder(input.Length);

        for (int index = 0; index < input.Length; index++)
        {
            if ((!Char.IsWhiteSpace(input, index)) && (!Char.IsPunctuation(input, index)))
            {
                output.Append(input[index]);
            }
        }

        return output.ToString();
    }

    //public static string LoadMistakePanel(string pageNumber, string mainXmlPath, List<PdfJsLine> originalText, string pdfType)
    //{
    //    string directoryPath = "";
    //    string pdfPath = "";

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

    //        else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("comparisonEntryTest"))
    //        {
    //            directoryPath = Common.GetComparisonEntryTestFiles_SavingPath();
    //        }
    //    }

    //    if (pdfType.Equals("producedPdf"))
    //    {
    //        //pdfPath = directoryPath + Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["SrcPDF"])) + "/" + Convert.ToString(HttpContext.Current.Session["Current_PrdPdfPage"]);
    //        pdfPath = directoryPath + "/" + Convert.ToString(HttpContext.Current.Session["Current_PrdPdfPage"]);
    //    }
    //    else
    //    {
    //        pdfPath = directoryPath + "/" + Convert.ToString(HttpContext.Current.Session["Current_SrcPdfPage"]);
    //    }

    //    List<Word> wrdList = new List<Word>();
    //    Word wrd_Produced = null;

    //    //string pdfName = Convert.ToString(HttpContext.Current.Session["pdfName"]);

    //    //string tetFilePath = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(pdfName) + "\\" + pageNumber + ".tetml";

    //    string tetFilePath = "";

    //    if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
    //    {
    //        //string pDirPath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
    //        //string userDir_Path = pDirPath + "\\Tests\\" + Convert.ToString(HttpContext.Current.Session["CompTestUser_Email"]) + "/ComparisonTests/";

    //        //tetFilePath = userDir_Path + "\\" + pageNumber + ".tetml";

    //        if (pdfType.Equals("producedPdf"))
    //        {
    //            tetFilePath = Common.GetTestFiles_SavingPath() + "/" + "\\Produced_" + pageNumber + ".tetml";
    //        }
    //        else
    //        {
    //            tetFilePath = Common.GetTestFiles_SavingPath() + "/" + "\\" + pageNumber + ".tetml";
    //        }
    //    }
    //    else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest"))
    //    {
    //        //string pDirPath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
    //        //string userDir_Path = pDirPath + "\\Tests\\" + Convert.ToString(HttpContext.Current.Session["CompTestUser_Email"]) + "/ComparisonTests/";

    //        //tetFilePath = userDir_Path + "\\" + pageNumber + ".tetml";

    //        //tetFilePath = Common.GetOnePageTestFiles_SavingPath() + "/" + "\\" + pageNumber + ".tetml";
    //        if (pdfType.Equals("producedPdf"))
    //        {
    //            tetFilePath = Common.GetOnePageTestFiles_SavingPath() + "/" + "\\Produced_" + pageNumber + ".tetml";
    //        }
    //        else
    //        {
    //            tetFilePath = Common.GetOnePageTestFiles_SavingPath() + "/" + "\\" + pageNumber + ".tetml";
    //        }

    //        //Common.GetTaskFiles_SavingPath() + Convert.ToString(HttpContext.Current.Session["Current_SrcPdfPage"]).Replace(".pdf", ".tetml");
    //    }
    //    else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("comparisonEntryTest"))
    //    {
    //        if (pdfType.Equals("producedPdf"))
    //        {
    //            tetFilePath = Common.GetComparisonEntryTestFiles_SavingPath() + "/" + "\\Produced_" + pageNumber + ".tetml";
    //        }
    //        else
    //        {
    //            tetFilePath = Common.GetComparisonEntryTestFiles_SavingPath() + "/" + "\\" + pageNumber + ".tetml";
    //        }
    //    }
    //    else
    //    {
    //        //tetFilePath = ConfigurationManager.AppSettings["PDFDirPhyPath"] + "\\" + Path.GetFileNameWithoutExtension(pdfName).Replace("-1", "") + "\\" +
    //        //                     Path.GetFileNameWithoutExtension(pdfName) + "\\Comparison\\Comparison-" + Convert.ToString(HttpContext.Current.Session["comparisonType"]) + "\\" +
    //        //                     Convert.ToString(HttpContext.Current.Session["userId"]) + "\\" + pageNumber + ".tetml";

    //        //Session["Current_SrcPdfPage"] = "Produced_" + currentPage + ".pdf";
    //        //    Session["Current_PrdPdfPage"]

    //        if (pdfType.Equals("producedPdf"))
    //        {
    //            tetFilePath = Common.GetTaskFiles_SavingPath() + "/" + "\\Produced_" + pageNumber + ".tetml";
    //        }
    //        else
    //        {
    //            tetFilePath = Common.GetTaskFiles_SavingPath() + "/" + "\\" + pageNumber + ".tetml";
    //        }

    //        //tetFilePath = Common.GetTaskFiles_SavingPath() + Convert.ToString(HttpContext.Current.Session["Current_SrcPdfPage"]).Replace(".pdf", ".tetml");
    //    }

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

    //    string llx = "";
    //    string lly = "";
    //    string urx = "";
    //    string ury = "";
    //    string temp = "";
    //    string urx_EndLine = "";
    //    string lly_EndLine = "";

    //    string coordinates = "";
    //    string word = "";
    //    string[] textToCheck = null;
    //    int counter = 0;

    //    foreach (var text in originalText)
    //    {
    //        XmlNodeList words = tetDoc.SelectNodes("//Word");
    //        XmlNodeList pages = tetDoc.SelectNodes("//Page");
    //        word = text.Text.Replace(",", " ");

    //        foreach (XmlNode page in pages)
    //        {
    //            textToCheck = ReplaceWhiteSpace_(word.Trim()).Split(',');
    //            textToCheck = textToCheck.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

    //            XmlNodeList innerwords = page.SelectNodes("//Text");

    //            for (int i = 0; i < innerwords.Count; i++)
    //            {
    //                if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 0))
    //                {
    //                    if (innerwords[i].InnerText.Replace(",", "").Trim().Equals(textToCheck[0]))
    //                    {
    //                        if (textToCheck.Length == 1)
    //                        {
    //                            XmlNode boxNode = innerwords[i].NextSibling;
    //                            llx = boxNode.Attributes["llx"].Value;
    //                            lly = boxNode.Attributes["lly"].Value;
    //                            urx = boxNode.Attributes["urx"].Value;
    //                            ury = boxNode.Attributes["ury"].Value;

    //                            coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

    //                            if (i + 2 < innerwords.Count)
    //                            {
    //                                wrdList.Add(new Word(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1, innerwords[i].InnerText +
    //                                                                                                                      innerwords[i + 1].InnerText +
    //                                                                                                                      innerwords[i + 2].InnerText, coordinates));
    //                            }

    //                            else if (i + 1 < innerwords.Count)
    //                            {
    //                                wrdList.Add(new Word(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1, innerwords[i].InnerText +
    //                                                                                                                      innerwords[i + 1].InnerText, coordinates));
    //                            }
    //                            counter++;
    //                        }//end

    //                        //Calculating right end side line x coordinate for highlighting whole line
    //                        else if (textToCheck.Length > 1)
    //                        {
    //                            for (int j = i; j < innerwords.Count; j++)
    //                            {
    //                                XmlNode boxNode = innerwords[j].NextSibling;
    //                                lly_EndLine = boxNode.Attributes["lly"].Value;
    //                                urx_EndLine = boxNode.Attributes["urx"].Value;

    //                                if (temp != lly_EndLine)
    //                                {
    //                                    if (innerwords[j - 1] != null)
    //                                    {
    //                                        XmlNode boxNode_Endline = innerwords[j - 1].NextSibling;

    //                                        urx_EndLine = boxNode_Endline.Attributes["urx"].Value;

    //                                        if (temp != "")
    //                                            break;
    //                                    }
    //                                }
    //                                temp = lly_EndLine;
    //                            }
    //                        }//end

    //                        if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 1))
    //                        {
    //                            if ((i + 1) < innerwords.Count)
    //                            {
    //                                if (innerwords[i + 1].InnerText.Replace(",", "").Trim().Equals(textToCheck[1]))
    //                                {
    //                                    //Calculating coordinates for Highlighting 2 words
    //                                    if (textToCheck.Length == 2)
    //                                    {
    //                                        XmlNode boxNode = innerwords[i].NextSibling;
    //                                        llx = boxNode.Attributes["llx"].Value;
    //                                        lly = boxNode.Attributes["lly"].Value;
    //                                        urx = urx_EndLine;
    //                                        ury = boxNode.Attributes["ury"].Value;

    //                                        coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

    //                                        wrdList.Add(new Word(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1, innerwords[i].InnerText + innerwords[i + 1].InnerText + innerwords[i + 2].InnerText, coordinates));
    //                                        counter++;
    //                                    }//end

    //                                    if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 2))
    //                                    {
    //                                        if (innerwords[i + 2].InnerText.Replace(",", "").Trim().Equals(textToCheck[2]))
    //                                        {
    //                                            XmlNode boxNode = innerwords[i].NextSibling;
    //                                            llx = boxNode.Attributes["llx"].Value;
    //                                            lly = boxNode.Attributes["lly"].Value;
    //                                            urx = urx_EndLine;
    //                                            ury = boxNode.Attributes["ury"].Value;

    //                                            coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

    //                                            wrdList.Add(new Word(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1, innerwords[i].InnerText + innerwords[i + 1].InnerText + innerwords[i + 2].InnerText, coordinates));
    //                                            counter++;
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                }
    //                //if (originalText.Count == counter)
    //                //    break;
    //            }//end outer for loop
    //        }//end outer foreach loop
    //    }

    //    //string pdfFilePath = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(pdfName) + "\\" + pageNumOriginal + ".pdf";
    //    return SelectCurrentWordInPDFWithAnnotation(pdfPath, wrdList);
    //}

    //public string MatchTextByWords(string pageNumber, List<PdfJsLine> originalText, XmlDocument tetDoc, string pdfPath)
    //{
    //    List<PdfWord> wrdList = new List<PdfWord>();

    //    string llx = "";
    //    string lly = "";
    //    string urx = "";
    //    string ury = "";
    //    string temp = "";
    //    string urx_EndLine = "";
    //    string lly_EndLine = "";

    //    string coordinates = "";
    //    string word = "";
    //    string[] textToCheck = null;
    //    //int counter = 0;
    //    //innerwords[12].NextSibling.ChildNodes[1].Attributes[1].Value

    //    foreach (var text in originalText)
    //    {
    //        if (text.IsErrorLine)
    //        {
    //            XmlNodeList words = tetDoc.SelectNodes("//Word");
    //            XmlNodeList pages = tetDoc.SelectNodes("//Page");
    //            word = text.Text.Replace(",", " ");

    //            foreach (XmlNode page in pages)
    //            {
    //                textToCheck = ReplaceWhiteSpace_(word.Trim()).Split(',');
    //                textToCheck = textToCheck.Where(x => (!string.IsNullOrEmpty(x)) && !x.Equals(",") && !x.Equals("‘") && !x.Equals("’")).ToArray();

    //                XmlNodeList innerwords = page.SelectNodes("//Text");

    //                for (int i = 0; i < innerwords.Count; i++)
    //                {
    //                    if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 0))
    //                    {
    //                        if (innerwords[i].InnerText.Replace(",", "").Replace("’", "").Replace("‘", "").Replace(",", "").Replace("***", "").ToLower().Trim().Equals(textToCheck[0].Replace(",", "").Replace("’", "").Replace("‘", "").Replace(",", "").Replace("***", "").Trim().ToLower()))
    //                        {
    //                            if (textToCheck.Length == 1)
    //                            {
    //                                XmlNode boxNode = innerwords[i].NextSibling;
    //                                llx = boxNode.Attributes["llx"].Value;
    //                                lly = boxNode.Attributes["lly"].Value;
    //                                urx = boxNode.Attributes["urx"].Value;
    //                                ury = boxNode.Attributes["ury"].Value;

    //                                coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

    //                                if (i + 2 < innerwords.Count)
    //                                {
    //                                    wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1,
    //                                            innerwords[i].InnerText +
    //                                            innerwords[i + 1].InnerText +
    //                                            innerwords[i + 2].InnerText, coordinates));
    //                                }

    //                                else if (i + 1 < innerwords.Count)
    //                                {
    //                                    wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1,
    //                                            innerwords[i].InnerText +
    //                                            innerwords[i + 1].InnerText, coordinates));
    //                                }
    //                                //counter++;
    //                            } //end

    //                            if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 1))
    //                            {
    //                                if ((i + 1) < innerwords.Count)
    //                                {
    //                                    if (innerwords[i + 1].InnerText.Replace(",", "").Replace(",", "").Replace("’", "").Replace("‘", "")
    //                                        .Replace(",", "").Replace("***", "").Trim().ToLower().Equals(textToCheck[1].Replace(",", "").Replace("’", "").Replace("‘", "").Replace(",", "").Replace("***", "").ToLower().Trim()))
    //                                    {
    //                                        //Calculating coordinates for Highlighting 2 words
    //                                        if (textToCheck.Length == 2)
    //                                        {
    //                                            //Calculating right end side line x coordinate for highlighting whole line
    //                                            temp = "";

    //                                            for (int j = i; j < innerwords.Count; j++)
    //                                            {
    //                                                XmlNode boxNode2 = innerwords[j].NextSibling;
    //                                                lly_EndLine = boxNode2.Attributes["lly"].Value;
    //                                                urx_EndLine = boxNode2.Attributes["urx"].Value;

    //                                                if (temp != lly_EndLine)
    //                                                {
    //                                                    if (innerwords[j - 1] != null)
    //                                                    {
    //                                                        XmlNode boxNode_Endline2 = innerwords[j - 1].NextSibling;

    //                                                        urx_EndLine = boxNode_Endline2.Attributes["urx"].Value;

    //                                                        if (temp != "")
    //                                                            break;
    //                                                    }
    //                                                }
    //                                                temp = lly_EndLine;
    //                                            }

    //                                            XmlNode boxNode = innerwords[i].NextSibling;
    //                                            llx = boxNode.Attributes["llx"].Value;
    //                                            lly = boxNode.Attributes["lly"].Value;
    //                                            urx = urx_EndLine;
    //                                            ury = boxNode.Attributes["ury"].Value;

    //                                            coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

    //                                            wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1,
    //                                                    innerwords[i].InnerText + innerwords[i + 1].InnerText +
    //                                                    innerwords[i + 2].InnerText, coordinates));
    //                                            //counter++;
    //                                        } //end

    //                                        if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 2))
    //                                        {
    //                                            var word1 =
    //                                                innerwords[i + 2].InnerText.Replace(",", "")
    //                                                    .Replace(",", "")
    //                                                    .Replace("’", "")
    //                                                    .Replace("‘", "")
    //                                                    .Replace(",", "")
    //                                                    .Replace("***", "")
    //                                                    .Trim();
    //                                            var word2 =
    //                                                textToCheck[2].Replace(",", "")
    //                                                    .Replace("’", "")
    //                                                    .Replace("‘", "")
    //                                                    .Replace(",", "").Replace("***", "");

    //                                            if (innerwords[i + 2].InnerText.Replace(",", "").Replace(",", "").Replace("’", "").Replace("‘", "").Replace(",", "").Replace("***", "").Trim().Equals(textToCheck[2].Replace(",", "").Replace("’", "").Replace("‘", "").Replace(",", "").Replace("***", "")))
    //                                            {
    //                                                //Calculating right end side line x coordinate for highlighting whole line
    //                                                temp = "";

    //                                                for (int j = i; j < innerwords.Count; j++)
    //                                                {
    //                                                    XmlNode boxNode3 = innerwords[j].NextSibling;
    //                                                    lly_EndLine = boxNode3.Attributes["lly"].Value;
    //                                                    urx_EndLine = boxNode3.Attributes["urx"].Value;

    //                                                    if (temp != lly_EndLine)
    //                                                    {
    //                                                        if (innerwords[j - 1] != null)
    //                                                        {
    //                                                            XmlNode boxNode_Endline3 = innerwords[j - 1].NextSibling;

    //                                                            urx_EndLine = boxNode_Endline3.Attributes["urx"].Value;

    //                                                            if (temp != "")
    //                                                                break;
    //                                                        }
    //                                                    }
    //                                                    temp = lly_EndLine;
    //                                                }

    //                                                XmlNode boxNode = innerwords[i].NextSibling;
    //                                                llx = boxNode.Attributes["llx"].Value;
    //                                                lly = boxNode.Attributes["lly"].Value;
    //                                                urx = urx_EndLine;
    //                                                ury = boxNode.Attributes["ury"].Value;

    //                                                coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

    //                                                wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()),
    //                                                        -1,
    //                                                        innerwords[i].InnerText + innerwords[i + 1].InnerText +
    //                                                        innerwords[i + 2].InnerText, coordinates));
    //                                                //counter++;
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                    //if (originalText.Count == counter)
    //                    //    break;
    //                } //end outer for loop
    //            } //end outer foreach loop
    //        }
    //    }

    //    //string pdfFilePath = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(pdfName) + "\\" + pageNumOriginal + ".pdf";

    //    if (wrdList.Count > 0)
    //    {
    //        PDFManipulation pdfMan = new PDFManipulation(pdfPath);
    //        return pdfMan.HighlightWord(pdfPath, wrdList);
    //    }
    //    return "";
    //}

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

    //public void MatchTextInSubPds(string pageNumber, List<PdfJsLine> originalText, XmlDocument tetDoc, string pdfPath, string pdfType)
    //{
    //    if (GetSubPrdPdfsCount(pdfPath, pageNumber) > 0)
    //    {
    //        int currentSubPage = Convert.ToInt32(HttpContext.Current.Session["ProducedPdfSubPage"]);

    //        //var tetmlPrdFilePath = GetTetmlFilePath(currentPage, "producedPdf");
    //        //var tetmlPrdDoc = LoadTetmlXmlDocument(tetmlPrdFilePath);
    //    }
    //}

    //In Process
    public string MatchTextByWords(string pageNumber, List<PdfJsLine> originalText, XmlDocument tetDoc, string pdfPath, string pdfType)
    {
        if (tetDoc == null) return "";

        try
        {
            List<PdfWord> wrdList = new List<PdfWord>();

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
            string wordWithSameCharSize = "";

            XmlNodeList pages = tetDoc.SelectNodes("//Page");
            if (pages == null) return "";

            foreach (XmlNode page in pages)
            {
                List<double> uryList = page.SelectNodes("//@ury")
                        .Cast<XmlNode>()
                        .Select(x => Math.Truncate(Convert.ToDouble(x.Value)))
                        .Distinct()
                        .OrderByDescending(x => x)
                        .ToList();

                //var uryListTest = page.SelectNodes("//@ury")
                //        .Cast<XmlNode>()
                //        .Select(x => (Convert.ToDouble(x.Value)))
                //        .Distinct()
                //        .OrderByDescending(x => Convert.ToDouble(x))
                //        .ToList();

                List<XmlNode> innerwordsList = page.SelectNodes("//Text").Cast<XmlNode>().ToList();
                List<string> pageLinesList = new List<string>();
                StringBuilder sameUryLine = new StringBuilder();
                int lineCounter = 0;

                if (uryList != null)
                {
                    foreach (double uryValue in uryList)
                    {
                        lineCounter = 0;

                        //var matchedTetmlWords = innerwordsList.Where(x => Math.Truncate(Convert.ToDouble(x.NextSibling.Attributes["ury"].Value)) == uryValue).ToList();

                        var matchedTetmlWords = innerwordsList.Where(x => (x.NextSibling != null &&
                                                                    Math.Truncate(Convert.ToDouble(x.NextSibling.Attributes["ury"].Value)) == uryValue) ||
                                                                    (x.NextSibling.NextSibling != null &&
                                                                    Math.Truncate(Convert.ToDouble(x.NextSibling.NextSibling.Attributes["ury"].Value)) == uryValue))
                                                                    .ToList();
                        if (matchedTetmlWords.Count > 0)
                        {
                            foreach (XmlNode value in matchedTetmlWords)
                            {
                                lineCounter++;

                                if (lineCounter == 1)
                                {
                                    if (value.NextSibling.NextSibling != null)
                                    {
                                        llx = value.NextSibling.NextSibling == null ? "0" : value.NextSibling.NextSibling.Attributes["llx"].Value;
                                        lly = value.NextSibling == null ? "0" : value.NextSibling.NextSibling.Attributes["lly"].Value;
                                        ury = value.NextSibling == null ? "0" : value.NextSibling.NextSibling.Attributes["ury"].Value;
                                    }
                                    else
                                    {
                                        llx = value.NextSibling == null ? "0" : value.NextSibling.Attributes["llx"].Value;
                                        lly = value.NextSibling == null ? "0" : value.NextSibling.Attributes["lly"].Value;
                                        ury = value.NextSibling == null ? "0" : value.NextSibling.Attributes["ury"].Value;
                                    }
                                }
                                if (lineCounter == matchedTetmlWords.Count)
                                    urx = value.NextSibling == null ? "0" : value.NextSibling.Attributes["urx"].Value;


                                if (lineCounter == 1)
                                {
                                    if (value.NextSibling.NextSibling != null)
                                    {
                                        sameUryLine.Append(value.NextSibling.NextSibling.InnerText + " ");
                                    }
                                    else
                                        sameUryLine.Append(value.InnerText + " ");
                                }
                                else
                                    sameUryLine.Append(value.InnerText + " ");
                            }

                            coordinates = llx + ":" + lly + ":" + urx + ":" + ury;
                            pageLinesList.Add(Convert.ToString(sameUryLine));

                            wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1,
                                Convert.ToString(sameUryLine), coordinates));

                            sameUryLine.Length = 0;
                        }
                    }
                }
            }

            if (wrdList.Count == 0) return "";

            if (pdfType.Equals("sourcePdf"))
            {
                string pageText = GetTextFromPdf(pdfPath);
                List<string> linesWithHyphenList = new List<string>();

                if (!string.IsNullOrEmpty(pageText))
                    linesWithHyphenList = GetHyphenLines(pageText);

                for (int i = 0; i < linesWithHyphenList.Count; i++)
                {
                    for (int j = 0; j < originalText.Count; j++)
                    {
                        if (originalText[j].Text.Equals(linesWithHyphenList[i]))
                        {
                            if (j + 1 < originalText.Count)
                            {
                                string remainWord = GetHyphensNextLineWord(pageText);
                                originalText[j + 1].Text = remainWord + " " + originalText[j + 1].Text;
                                break;
                            }
                        }
                    }
                }
            }

            List<PdfWord> highlightedWrdList = new List<PdfWord>();

            foreach (var text in originalText)
            {
                if (text.IsErrorLine)
                {
                    foreach (var tetmlLine in wrdList)
                    {
                        if (MatchLineText(text, tetmlLine, highlightedWrdList)) break;
                    }
                }
            }

            if (highlightedWrdList.Count > 0)
            {
                PDFManipulation pdfMan = new PDFManipulation(pdfPath);
                return pdfMan.HighlightWord(pdfPath, highlightedWrdList);
            }
            return "";
        }
        catch (Exception ex)
        {
            return "";
        }
    }

    //2017-10-09 old working
    //private bool MatchLineText(PdfJsLine pdfJsSelectedLine, PdfWord tetmlLine, List<PdfWord> highlightedWrdList)
    //{
    //    if (string.IsNullOrEmpty(pdfJsSelectedLine.Text) || string.IsNullOrEmpty(tetmlLine.Text)) return false;

    //    List<string> pdfJsLineTempList = Regex.Split(pdfJsSelectedLine.Text, @"\s+").ToList();
    //    List<string> tetmlLineTempList = Regex.Split(tetmlLine.Text, @"\s+").ToList();

    //    string pdfJsText = Regex.Replace(pdfJsSelectedLine.Text, "[^A-Za-z0-9]", "");
    //    string xmlText = Regex.Replace(tetmlLine.Text, "[^A-Za-z0-9]", "");

    //    if (!string.IsNullOrEmpty(pdfJsText) && !string.IsNullOrEmpty(xmlText))
    //    {
    //        int matchingPer = GetMatchingPercentage(pdfJsText, xmlText);
    //        if (matchingPer < 50)
    //            return false;
    //    }

    //    if (pdfJsSelectedLine.Text.Trim().Equals(tetmlLine.Text.Trim()))
    //    {
    //        highlightedWrdList.Add(tetmlLine); return true;
    //    }

    //    //string pdfJsText = RemoveWhiteSpace(RemoveSpecialChars(pdfJsSelectedLine.Text));
    //    //string xmlText = RemoveWhiteSpace(RemoveSpecialChars(tetmlLine.Text));

    //    string finalPdfJsText = "";
    //    string finalxmlText = "";

    //    if (pdfJsText.Length == xmlText.Length)
    //    {
    //        if (pdfJsText != xmlText)
    //        {
    //            int matchingPer = GetMatchingPercentage(pdfJsText, xmlText);
    //            if (matchingPer >= 80)
    //            {
    //                highlightedWrdList.Add(tetmlLine); return true;
    //            }
    //        }
    //        else
    //        {
    //            highlightedWrdList.Add(tetmlLine); return true;
    //        }
    //    }
    //    else if (pdfJsText.Length > xmlText.Length)
    //    {
    //        finalPdfJsText = pdfJsText.Substring(0, xmlText.Length);
    //        finalxmlText = xmlText;
    //    }
    //    else if (xmlText.Length > pdfJsText.Length)
    //    {
    //        finalPdfJsText = pdfJsText;
    //        finalxmlText = xmlText.Substring(0, pdfJsText.Length);
    //    }

    //    if (string.IsNullOrEmpty(finalPdfJsText) || string.IsNullOrEmpty(finalxmlText)) return false;

    //    if (finalPdfJsText.ToLower().Equals(finalxmlText.ToLower()))
    //    {
    //        highlightedWrdList.Add(tetmlLine);
    //        return true;
    //    }
    //    else
    //    {
    //        if (pdfJsLineTempList.Count > 3 && tetmlLineTempList.Count > 3)
    //        {
    //            if (pdfJsLineTempList[0].Trim().Equals(tetmlLineTempList[0].Trim()))
    //            {
    //                if (pdfJsLineTempList[1].Trim().Equals(tetmlLineTempList[1].Trim()))
    //                {
    //                    if (pdfJsLineTempList[2].Trim().Equals(tetmlLineTempList[2].Trim()))
    //                    {
    //                        highlightedWrdList.Add(tetmlLine);
    //                        return true;
    //                    }
    //                }
    //            }
    //            //Excluding first hyphen word from list
    //            else if (pdfJsLineTempList[0].Trim().Equals(tetmlLineTempList[1].Trim()))
    //            {
    //                if (pdfJsLineTempList[1].Trim().Equals(tetmlLineTempList[2].Trim()))
    //                {
    //                    if (pdfJsLineTempList[2].Trim().Equals(tetmlLineTempList[3].Trim()))
    //                    {
    //                        highlightedWrdList.Add(tetmlLine);
    //                        return true;
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    return false;
    //}

    private bool MatchLineText(PdfJsLine pdfJsSelectedLine, PdfWord tetmlLine, List<PdfWord> highlightedWrdList)
    {
        if (string.IsNullOrEmpty(pdfJsSelectedLine.Text) || string.IsNullOrEmpty(tetmlLine.Text)) return false;

        List<string> pdfJsLineTempList = Regex.Split(pdfJsSelectedLine.Text, @"\s+").ToList();
        List<string> tetmlLineTempList = Regex.Split(tetmlLine.Text, @"\s+").ToList();

        //var tt = string.Join(" ", pdfJsLineTempList.ToArray());

        //if (!string.IsNullOrEmpty(pdfJsText) && !string.IsNullOrEmpty(xmlText))
        //{
        //    int matchingPer = GetMatchingPercentage(pdfJsText, xmlText);
        //    if (matchingPer < 50)
        //        return false;
        //}

        string pdfJsText = Regex.Replace(pdfJsSelectedLine.Text, "[^A-Za-z0-9]", "");
        string xmlText = Regex.Replace(tetmlLine.Text, "[^A-Za-z0-9]", "");

        //if (!string.IsNullOrEmpty(pdfJsText) && !string.IsNullOrEmpty(xmlText))
        //{
        //    int matchingPer = GetMatchingPercentage(pdfJsText, xmlText);
        //    if (matchingPer < 50)
        //        return false;
        //}

        if (pdfJsSelectedLine.Text.Trim().Equals(tetmlLine.Text.Trim()))
        {
            highlightedWrdList.Add(tetmlLine); return true;
        }

        //string pdfJsText = RemoveWhiteSpace(RemoveSpecialChars(pdfJsSelectedLine.Text));
        //string xmlText = RemoveWhiteSpace(RemoveSpecialChars(tetmlLine.Text));

        string finalPdfJsText = "";
        string finalxmlText = "";

        if (pdfJsText.Length == xmlText.Length)
        {
            if (pdfJsText != xmlText)
            {
                int matchingPer = GetMatchingPercentage(pdfJsText, xmlText);
                if (matchingPer >= 80)
                {
                    highlightedWrdList.Add(tetmlLine); return true;
                }
            }
            else
            {
                highlightedWrdList.Add(tetmlLine); return true;
            }
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
            highlightedWrdList.Add(tetmlLine);
            return true;
        }
        else
        {
            if (pdfJsLineTempList.Count > 3 && tetmlLineTempList.Count > 3)
            {
                if (pdfJsLineTempList[0].Trim().Equals(tetmlLineTempList[0].Trim()))
                {
                    if (pdfJsLineTempList[1].Trim().Equals(tetmlLineTempList[1].Trim()))
                    {
                        if (pdfJsLineTempList[2].Trim().Equals(tetmlLineTempList[2].Trim()))
                        {
                            highlightedWrdList.Add(tetmlLine);
                            return true;
                        }
                    }
                }
                //Excluding first hyphen word from list
                else if (pdfJsLineTempList[0].Trim().Equals(tetmlLineTempList[1].Trim()))
                {
                    if (pdfJsLineTempList[1].Trim().Equals(tetmlLineTempList[2].Trim()))
                    {
                        if (pdfJsLineTempList[2].Trim().Equals(tetmlLineTempList[3].Trim()))
                        {
                            highlightedWrdList.Add(tetmlLine);
                            return true;
                        }
                    }
                }
            }
        }
        return false;
    }

    public static int GetMatchingPercentage(string pdfJsLine, string tetmlLine)
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


    //Old matching 2016-10-19
    public string MatchTextByWords11(string pageNumber, List<PdfJsLine> originalText, XmlDocument tetDoc, string pdfPath)
    {
        try
        {
            List<PdfWord> wrdList = new List<PdfWord>();

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
            string wordWithSameCharSize = "";

            foreach (var text in originalText)
            {
                if (text.IsErrorLine)
                {
                    //XmlNodeList words = tetDoc.SelectNodes("//Word");
                    XmlNodeList pages = tetDoc.SelectNodes("//Page");
                    word = text.Text.Replace(",", " ");

                    foreach (XmlNode page in pages)
                    {
                        textToCheck = ReplaceWhiteSpace_(word.Trim()).Split(',');
                        textToCheck = textToCheck.Where(x => (!string.IsNullOrEmpty(x)) && !x.Equals(",") && !x.Equals("‘") && !x.Equals("’")).ToArray();

                        XmlNodeList innerwords = page.SelectNodes("//Text");

                        for (int i = 0; i < innerwords.Count; i++)
                        {
                            if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 0))
                            {
                                wordWithSameCharSize = RemoveLargerFontChar(innerwords[i]);

                                if (RemoveSpecialChars(wordWithSameCharSize).Equals(RemoveSpecialChars(textToCheck[0])))
                                {
                                    //}
                                    //if (innerwords[i].InnerText
                                    //    .Replace(",", "").Replace("’", "").Replace("‘", "").Replace(",", "").Replace("***", "").ToLower().Trim()
                                    //    .Equals(textToCheck[0].Replace(",", "").Replace("’", "").Replace("‘", "").Replace(",", "").Replace("***", "").Trim().ToLower()))
                                    //{
                                    if (textToCheck.Length == 1)
                                    {
                                        XmlNode boxNode = innerwords[i].NextSibling;
                                        llx = boxNode.Attributes["llx"].Value;
                                        lly = boxNode.Attributes["lly"].Value;
                                        urx = boxNode.Attributes["urx"].Value;
                                        ury = boxNode.Attributes["ury"].Value;

                                        coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

                                        if (i + 2 < innerwords.Count)
                                        {
                                            wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1,
                                                    innerwords[i].InnerText +
                                                    innerwords[i + 1].InnerText +
                                                    innerwords[i + 2].InnerText, coordinates));
                                        }

                                        else if (i + 1 < innerwords.Count)
                                        {
                                            wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1,
                                                    innerwords[i].InnerText +
                                                    innerwords[i + 1].InnerText, coordinates));
                                        }
                                        //counter++;
                                    } //end

                                    if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 1))
                                    {
                                        if ((i + 1) < innerwords.Count)
                                        {
                                            wordWithSameCharSize = RemoveLargerFontChar(innerwords[i + 1]);

                                            if (RemoveSpecialChars(wordWithSameCharSize).Equals(RemoveSpecialChars(textToCheck[1])))
                                            {

                                                //if (innerwords[i + 1].InnerText.Replace(",", "").Replace(",", "").Replace("’", "").Replace("‘", "")
                                                //    .Replace(",", "").Replace("***", "").Trim().ToLower().Equals(textToCheck[1].Replace(",", "").Replace("’", "").Replace("‘", "").Replace(",", "").Replace("***", "").ToLower().Trim()))
                                                //{
                                                //Calculating coordinates for Highlighting 2 words
                                                if (textToCheck.Length == 2)
                                                {
                                                    //Calculating right end side line x coordinate for highlighting whole line
                                                    temp = "";

                                                    for (int j = i; j < innerwords.Count; j++)
                                                    {
                                                        XmlNode boxNode2 = innerwords[j].NextSibling;
                                                        lly_EndLine = boxNode2.Attributes["lly"].Value;
                                                        urx_EndLine = boxNode2.Attributes["urx"].Value;

                                                        if (temp != lly_EndLine)
                                                        {
                                                            if (innerwords[j - 1] != null)
                                                            {
                                                                XmlNode boxNode_Endline2 = innerwords[j - 1].NextSibling;

                                                                urx_EndLine = boxNode_Endline2.Attributes["urx"].Value;

                                                                if (temp != "")
                                                                    break;
                                                            }
                                                        }
                                                        temp = lly_EndLine;
                                                    }

                                                    XmlNode boxNode = innerwords[i].NextSibling;
                                                    llx = boxNode.Attributes["llx"].Value;
                                                    lly = boxNode.Attributes["lly"].Value;
                                                    urx = urx_EndLine;
                                                    ury = boxNode.Attributes["ury"].Value;

                                                    coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

                                                    wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1,
                                                            innerwords[i].InnerText + innerwords[i + 1].InnerText +
                                                            innerwords[i + 2].InnerText, coordinates));
                                                    //counter++;
                                                } //end

                                                if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 2))
                                                {
                                                    wordWithSameCharSize = RemoveLargerFontChar(innerwords[i + 2]);

                                                    if (RemoveSpecialChars(wordWithSameCharSize).Equals(RemoveSpecialChars(textToCheck[2])))
                                                    {
                                                        //if (innerwords[i + 2].InnerText.Replace(",", "").Replace(",", "").Replace("’", "").Replace("‘", "").
                                                        //Replace(",", "").Replace("***", "").Trim().Equals(textToCheck[2].Replace(",", "").Replace("’", "").Replace("‘", "").
                                                        //    Replace(",", "").Replace("***", "")))
                                                        //{
                                                        //Calculating right end side line x coordinate for highlighting whole line
                                                        temp = "";

                                                        for (int j = i; j < innerwords.Count; j++)
                                                        {
                                                            XmlNode boxNode3 = innerwords[j].NextSibling;
                                                            lly_EndLine = boxNode3.Attributes["lly"].Value;
                                                            urx_EndLine = boxNode3.Attributes["urx"].Value;

                                                            if (temp != lly_EndLine)
                                                            {
                                                                if (innerwords[j - 1] != null)
                                                                {
                                                                    XmlNode boxNode_Endline3 = innerwords[j - 1].NextSibling;

                                                                    urx_EndLine = boxNode_Endline3.Attributes["urx"].Value;

                                                                    if (temp != "")
                                                                        break;
                                                                }
                                                            }
                                                            temp = lly_EndLine;
                                                        }

                                                        XmlNode boxNode = innerwords[i].NextSibling;
                                                        llx = boxNode.Attributes["llx"].Value;
                                                        lly = boxNode.Attributes["lly"].Value;
                                                        urx = urx_EndLine;
                                                        ury = boxNode.Attributes["ury"].Value;

                                                        coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

                                                        wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()),
                                                                -1,
                                                                innerwords[i].InnerText + innerwords[i + 1].InnerText +
                                                                innerwords[i + 2].InnerText, coordinates));
                                                        //counter++;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            //if (originalText.Count == counter)
                            //    break;
                        } //end outer for loop
                    } //end outer foreach loop
                }
            }

            //string pdfFilePath = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(pdfName) + "\\" + pageNumOriginal + ".pdf";

            if (wrdList.Count > 0)
            {
                PDFManipulation pdfMan = new PDFManipulation(pdfPath);
                return pdfMan.HighlightWord(pdfPath, wrdList);
            }
            return "";
        }
        catch (Exception ex)
        {
            return "";
        }
    }

    public string RemoveLargerFontChar(XmlNode nodeText)
    {
        StringBuilder text = new StringBuilder();

        if (nodeText.NextSibling != null)
        {
            if (nodeText.NextSibling.ChildNodes != null)
            {
                int startIndex = 0;

                if (nodeText.NextSibling.ChildNodes.Count > 1)
                {
                    if (Convert.ToDouble(nodeText.NextSibling.ChildNodes[0].Attributes[1].Value) >
                        Convert.ToDouble(nodeText.NextSibling.ChildNodes[1].Attributes[1].Value))
                    {
                        startIndex = 1;
                    }
                    else
                    {
                        startIndex = 0;
                    }
                }

                for (int i = startIndex; i < nodeText.NextSibling.ChildNodes.Count; i++)
                {
                    text.Append(nodeText.NextSibling.ChildNodes[i].InnerText);
                }
            }
        }

        return Convert.ToString(text);
    }

    public string RemoveSpecialChars(string word)
    {
        return word.Replace(",", "").Replace(",", "").Replace("’", "").Replace("‘", "").Replace(",", "").Replace("***", "").Trim().ToLower();
    }


    //public string MatchTextByWords(string pageNumber, List<PdfJsLine> originalText, XmlDocument tetDoc, string pdfPath)
    //{
    //    List<PdfWord> wrdList = new List<PdfWord>();

    //    string llx = "";
    //    string lly = "";
    //    string urx = "";
    //    string ury = "";
    //    string temp = "";
    //    string urx_EndLine = "";
    //    string lly_EndLine = "";

    //    string coordinates = "";
    //    string word = "";
    //    string[] textToCheck = null;
    //    //int counter = 0;

    //    foreach (var text in originalText)
    //    {
    //        if (text.IsErrorLine)
    //        {
    //            XmlNodeList words = tetDoc.SelectNodes("//Word");
    //            XmlNodeList pages = tetDoc.SelectNodes("//Page");
    //            word = text.Text.Replace(",", " ");

    //            foreach (XmlNode page in pages)
    //            {
    //                textToCheck = ReplaceWhiteSpace_(word.Trim()).Split(',');
    //                textToCheck = textToCheck.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

    //                XmlNodeList innerwords = page.SelectNodes("//Text");

    //                for (int i = 0; i < innerwords.Count; i++)
    //                {
    //                    if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 0))
    //                    {
    //                        if (innerwords[i].InnerText.Replace(",", "").ToLower().Trim().Equals(textToCheck[0].Trim().ToLower()))
    //                        {
    //                            if (textToCheck.Length == 1)
    //                            {
    //                                XmlNode boxNode = innerwords[i].NextSibling;
    //                                llx = boxNode.Attributes["llx"].Value;
    //                                lly = boxNode.Attributes["lly"].Value;
    //                                urx = boxNode.Attributes["urx"].Value;
    //                                ury = boxNode.Attributes["ury"].Value;

    //                                coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

    //                                if (i + 2 < innerwords.Count)
    //                                {
    //                                    wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1,
    //                                            innerwords[i].InnerText +
    //                                            innerwords[i + 1].InnerText +
    //                                            innerwords[i + 2].InnerText, coordinates));
    //                                }

    //                                else if (i + 1 < innerwords.Count)
    //                                {
    //                                    wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1,
    //                                            innerwords[i].InnerText +
    //                                            innerwords[i + 1].InnerText, coordinates));
    //                                }
    //                                //counter++;
    //                            } //end


    //                            ////Calculating right end side line x coordinate for highlighting whole line
    //                            //else if (textToCheck.Length > 1)
    //                            //{
    //                            //    for (int j = i; j < innerwords.Count; j++)
    //                            //    {
    //                            //        XmlNode boxNode = innerwords[j].NextSibling;
    //                            //        lly_EndLine = boxNode.Attributes["lly"].Value;
    //                            //        urx_EndLine = boxNode.Attributes["urx"].Value;

    //                            //        if (temp != lly_EndLine)
    //                            //        {
    //                            //            if (innerwords[j - 1] != null)
    //                            //            {
    //                            //                XmlNode boxNode_Endline = innerwords[j - 1].NextSibling;

    //                            //                urx_EndLine = boxNode_Endline.Attributes["urx"].Value;

    //                            //                if (temp != "")
    //                            //                    break;
    //                            //            }
    //                            //        }
    //                            //        temp = lly_EndLine;
    //                            //    }
    //                            //} //end

    //                            if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 1))
    //                            {
    //                                if ((i + 1) < innerwords.Count)
    //                                {
    //                                    if (innerwords[i + 1].InnerText.Replace(",", "").Trim().ToLower().Equals(textToCheck[1].ToLower().Trim()))
    //                                    {
    //                                        //Calculating coordinates for Highlighting 2 words
    //                                        if (textToCheck.Length == 2)
    //                                        {
    //                                            //Calculating right end side line x coordinate for highlighting whole line
    //                                            temp = "";

    //                                            for (int j = i; j < innerwords.Count; j++)
    //                                            {
    //                                                XmlNode boxNode2 = innerwords[j].NextSibling;
    //                                                lly_EndLine = boxNode2.Attributes["lly"].Value;
    //                                                urx_EndLine = boxNode2.Attributes["urx"].Value;

    //                                                if (temp != lly_EndLine)
    //                                                {
    //                                                    if (innerwords[j - 1] != null)
    //                                                    {
    //                                                        XmlNode boxNode_Endline2 = innerwords[j - 1].NextSibling;

    //                                                        urx_EndLine = boxNode_Endline2.Attributes["urx"].Value;

    //                                                        if (temp != "")
    //                                                            break;
    //                                                    }
    //                                                }
    //                                                temp = lly_EndLine;
    //                                            }

    //                                            XmlNode boxNode = innerwords[i].NextSibling;
    //                                            llx = boxNode.Attributes["llx"].Value;
    //                                            lly = boxNode.Attributes["lly"].Value;
    //                                            urx = urx_EndLine;
    //                                            ury = boxNode.Attributes["ury"].Value;

    //                                            coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

    //                                            wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1,
    //                                                    innerwords[i].InnerText + innerwords[i + 1].InnerText +
    //                                                    innerwords[i + 2].InnerText, coordinates));
    //                                            //counter++;
    //                                        } //end

    //                                        if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 2))
    //                                        {
    //                                            if (innerwords[i + 2].InnerText.Replace(",", "").Trim().Equals(textToCheck[2]))
    //                                            {
    //                                                //Calculating right end side line x coordinate for highlighting whole line
    //                                                temp = "";

    //                                                for (int j = i; j < innerwords.Count; j++)
    //                                                {
    //                                                    XmlNode boxNode3 = innerwords[j].NextSibling;
    //                                                    lly_EndLine = boxNode3.Attributes["lly"].Value;
    //                                                    urx_EndLine = boxNode3.Attributes["urx"].Value;

    //                                                    if (temp != lly_EndLine)
    //                                                    {
    //                                                        if (innerwords[j - 1] != null)
    //                                                        {
    //                                                            XmlNode boxNode_Endline3 = innerwords[j - 1].NextSibling;

    //                                                            urx_EndLine = boxNode_Endline3.Attributes["urx"].Value;

    //                                                            if (temp != "")
    //                                                                break;
    //                                                        }
    //                                                    }
    //                                                    temp = lly_EndLine;
    //                                                }

    //                                                XmlNode boxNode = innerwords[i].NextSibling;
    //                                                llx = boxNode.Attributes["llx"].Value;
    //                                                lly = boxNode.Attributes["lly"].Value;
    //                                                urx = urx_EndLine;
    //                                                ury = boxNode.Attributes["ury"].Value;

    //                                                coordinates = llx + ":" + lly + ":" + urx + ":" + ury;

    //                                                wrdList.Add(new PdfWord(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()),
    //                                                        -1,
    //                                                        innerwords[i].InnerText + innerwords[i + 1].InnerText +
    //                                                        innerwords[i + 2].InnerText, coordinates));
    //                                                //counter++;
    //                                            }
    //                                        }
    //                                    }
    //                                }
    //                            }
    //                        }
    //                    }
    //                    //if (originalText.Count == counter)
    //                    //    break;
    //                } //end outer for loop
    //            } //end outer foreach loop
    //        }
    //    }

    //    //string pdfFilePath = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(pdfName) + "\\" + pageNumOriginal + ".pdf";

    //    if (wrdList.Count > 0)
    //    {
    //        PDFManipulation pdfMan = new PDFManipulation(pdfPath);
    //        return pdfMan.HighlightWord(pdfPath, wrdList);
    //    }
    //    return "";
    //}


    ////public string LoadMistakePanel(string pageNumber, string mainXmlPath, List<PdfJsLine> originalText, string pdfType)
    ////{
    ////    string directoryPath = "";
    ////    string pdfPath = "";

    ////    if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]) != "")
    ////    {
    ////        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
    ////        {
    ////            directoryPath = Common.GetTestFiles_SavingPath();
    ////        }
    ////        else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest"))
    ////        {
    ////            directoryPath = Common.GetOnePageTestFiles_SavingPath();
    ////        }
    ////        else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
    ////        {
    ////            directoryPath = Common.GetTaskFiles_SavingPath();
    ////        }

    ////        else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("comparisonEntryTest"))
    ////        {
    ////            directoryPath = Common.GetComparisonEntryTestFiles_SavingPath();
    ////        }
    ////    }

    ////    if (pdfType.Equals("producedPdf"))
    ////    {
    ////        //pdfPath = directoryPath + Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["SrcPDF"])) + "/" + Convert.ToString(HttpContext.Current.Session["Current_PrdPdfPage"]);
    ////        pdfPath = directoryPath + "/" + Convert.ToString(HttpContext.Current.Session["Current_PrdPdfPage"]);
    ////    }
    ////    else
    ////    {
    ////        pdfPath = directoryPath + "/" + Convert.ToString(HttpContext.Current.Session["Current_SrcPdfPage"]);
    ////    }

    ////    List<Word> wrdList = new List<Word>();
    ////    Word wrd_Produced = null;

    ////    //string pdfName = Convert.ToString(HttpContext.Current.Session["pdfName"]);

    ////    //string tetFilePath = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(pdfName) + "\\" + pageNumber + ".tetml";

    ////    string tetFilePath = "";

    ////    if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
    ////    {
    ////        //string pDirPath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
    ////        //string userDir_Path = pDirPath + "\\Tests\\" + Convert.ToString(HttpContext.Current.Session["CompTestUser_Email"]) + "/ComparisonTests/";

    ////        //tetFilePath = userDir_Path + "\\" + pageNumber + ".tetml";

    ////        if (pdfType.Equals("producedPdf"))
    ////        {
    ////            tetFilePath = Common.GetTestFiles_SavingPath() + "/" + "\\Produced_" + pageNumber + ".tetml";
    ////        }
    ////        else
    ////        {
    ////            tetFilePath = Common.GetTestFiles_SavingPath() + "/" + "\\" + pageNumber + ".tetml";
    ////        }
    ////    }
    ////    else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest"))
    ////    {
    ////        //string pDirPath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
    ////        //string userDir_Path = pDirPath + "\\Tests\\" + Convert.ToString(HttpContext.Current.Session["CompTestUser_Email"]) + "/ComparisonTests/";

    ////        //tetFilePath = userDir_Path + "\\" + pageNumber + ".tetml";

    ////        //tetFilePath = Common.GetOnePageTestFiles_SavingPath() + "/" + "\\" + pageNumber + ".tetml";
    ////        if (pdfType.Equals("producedPdf"))
    ////        {
    ////            tetFilePath = Common.GetOnePageTestFiles_SavingPath() + "/" + "\\Produced_" + pageNumber + ".tetml";
    ////        }
    ////        else
    ////        {
    ////            tetFilePath = Common.GetOnePageTestFiles_SavingPath() + "/" + "\\" + pageNumber + ".tetml";
    ////        }

    ////        //Common.GetTaskFiles_SavingPath() + Convert.ToString(HttpContext.Current.Session["Current_SrcPdfPage"]).Replace(".pdf", ".tetml");
    ////    }
    ////    else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("comparisonEntryTest"))
    ////    {
    ////        if (pdfType.Equals("producedPdf"))
    ////        {
    ////            tetFilePath = Common.GetComparisonEntryTestFiles_SavingPath() + "/" + "\\Produced_" + pageNumber + ".tetml";
    ////        }
    ////        else
    ////        {
    ////            tetFilePath = Common.GetComparisonEntryTestFiles_SavingPath() + "/" + "\\" + pageNumber + ".tetml";
    ////        }
    ////    }
    ////    else
    ////    {
    ////        //tetFilePath = ConfigurationManager.AppSettings["PDFDirPhyPath"] + "\\" + Path.GetFileNameWithoutExtension(pdfName).Replace("-1", "") + "\\" +
    ////        //                     Path.GetFileNameWithoutExtension(pdfName) + "\\Comparison\\Comparison-" + Convert.ToString(HttpContext.Current.Session["comparisonType"]) + "\\" +
    ////        //                     Convert.ToString(HttpContext.Current.Session["userId"]) + "\\" + pageNumber + ".tetml";

    ////        //Session["Current_SrcPdfPage"] = "Produced_" + currentPage + ".pdf";
    ////        //    Session["Current_PrdPdfPage"]

    ////        if (pdfType.Equals("producedPdf"))
    ////        {
    ////            tetFilePath = Common.GetTaskFiles_SavingPath() + "/" + "\\Produced_" + pageNumber + ".tetml";
    ////        }
    ////        else
    ////        {
    ////            tetFilePath = Common.GetTaskFiles_SavingPath() + "/" + "\\" + pageNumber + ".tetml";
    ////        }

    ////        //tetFilePath = Common.GetTaskFiles_SavingPath() + Convert.ToString(HttpContext.Current.Session["Current_SrcPdfPage"]).Replace(".pdf", ".tetml");
    ////    }

    ////    XmlDocument tetDoc = new XmlDocument();
    ////    try
    ////    {
    ////        StreamReader sr = new StreamReader(tetFilePath);
    ////        string xmlText = sr.ReadToEnd();
    ////        sr.Close();
    ////        string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
    ////        tetDoc.LoadXml(documentXML);
    ////    }
    ////    catch { }

    ////    string llx = "";
    ////    string lly = "";
    ////    string urx = "";
    ////    string ury = "";
    ////    string temp = "";
    ////    string urx_EndLine = "";
    ////    string lly_EndLine = "";

    ////    string coordinates = "";
    ////    string word = "";
    ////    string[] textToCheck = null;
    ////    int counter = 0;

    ////    foreach (var text in originalText)
    ////    {
    ////        if (text.IsErrorLine)
    ////        {
    ////            List<TetmlLine> tetmlLine = GetLineFromTetml(tetDoc, text.LineNum);

    ////            if (tetmlLine != null)
    ////            {
    ////                if (tetmlLine.Count > 0)
    ////                {
    ////                    coordinates = tetmlLine[0].Llx + ":" + tetmlLine[0].Lly + ":" + tetmlLine[0].Urx + ":" + tetmlLine[0].Ury;

    ////                    wrdList.Add(new Word(Convert.ToInt32(pageNumber.Replace("Produced_", "").Trim()), -1, tetmlLine[0].Text, coordinates));
    ////                }
    ////            }
    ////        }
    ////    }

    ////    //string pdfFilePath = ConfigurationManager.AppSettings["HighlightDirPP"] + "\\" + Path.GetFileNameWithoutExtension(pdfName) + "\\" + pageNumOriginal + ".pdf";

    ////    //return SelectCurrentWordInPDFWithAnnotation(pdfPath, wrdList);

    ////    PDFManipulation pdfMan = new PDFManipulation(pdfPath);
    ////    return pdfMan.HighlightWord(pdfPath, wrdList);
    ////}

    public List<TetmlLine> GetLineFromTetml(XmlDocument tetDoc, int selectedLine)
    {
        string lly = "";
        string nextLinelly = "";
        int lineCounter = 0;

        List<TetmlLine> tetmlLines = new List<TetmlLine>();
        StringBuilder sbLineText = new StringBuilder();
        //XmlNode boxNode = null;
        double topMarginValue = 1;
        TetmlLine line = new TetmlLine();
        bool isStartingllx = true;

        XmlNodeList pages = tetDoc.SelectNodes("//Page");

        foreach (XmlNode page in pages)
        {
            XmlNodeList innerwords = page.SelectNodes("//Text");

            for (int i = 0; i < innerwords.Count; i++)
            {
                if (Convert.ToString(innerwords[i]) != "")
                {
                    XmlNode boxNode = innerwords[i].NextSibling;
                    lly = boxNode.Attributes["lly"].Value;

                    if (isStartingllx)
                    {
                        line.Llx = boxNode.Attributes["llx"].Value;
                        isStartingllx = false;
                    }

                    line.Lly = lly;
                    line.Ury = boxNode.Attributes["ury"].Value;

                    sbLineText.Append(innerwords[i].InnerText + " ");

                    if (!string.IsNullOrEmpty(lly))
                    {
                        if (i + 1 < innerwords.Count)
                        {
                            XmlNode nextLineBoxNode = innerwords[i + 1].NextSibling;
                            nextLinelly = nextLineBoxNode.Attributes["lly"].Value;

                            if (Math.Abs(Convert.ToDouble(lly) - Convert.ToDouble(nextLinelly)) > topMarginValue)
                            {
                                lineCounter++;
                                line.Urx = boxNode.Attributes["urx"].Value;
                                line.Text = Convert.ToString(sbLineText);
                                line.LineNum = lineCounter;
                                tetmlLines.Add(line);
                                sbLineText.Length = 0;
                                line = new TetmlLine();
                                isStartingllx = true;
                            }
                        }
                    }
                }
            }
        } //end for loop

        //string pdfFilePath = "";
        //ReadPdfFile("", 11);

        var currentLine = tetmlLines.Where(x => x.LineNum == selectedLine).ToList();

        return currentLine;
    }

    public List<TetmlLine> GetAllLinesFromTetml(XmlDocument tetDoc)
    {
        string lly = "";
        string nextLinelly = "";
        int lineCounter = 0;

        try
        {
            List<TetmlLine> tetmlLines = new List<TetmlLine>();
            StringBuilder sbLineText = new StringBuilder();
            //XmlNode boxNode = null;
            double topMarginValue = 1;
            TetmlLine line = new TetmlLine();
            bool isStartingllx = true;

            XmlNodeList pages = tetDoc.SelectNodes("//Page");

            foreach (XmlNode page in pages)
            {
                XmlNodeList innerwords = page.SelectNodes("//Text");

                for (int i = 0; i < innerwords.Count; i++)
                {
                    if (i == 315)
                    {

                    }

                    if (Convert.ToString(innerwords[i]) != "")
                    {
                        //XmlNode boxNode = innerwords[i].NextSibling;
                        //lly = boxNode.Attributes["lly"].Value;

                        //if (isStartingllx)
                        //{
                        //    line.Llx = boxNode.Attributes["llx"].Value;
                        //    isStartingllx = false;
                        //}

                        XmlNode boxNode = innerwords[i].NextSibling;
                        lly = boxNode.Attributes["lly"].Value;

                        if (isStartingllx)
                        {
                            line.Llx = boxNode.Attributes["llx"].Value;
                            isStartingllx = false;
                        }


                        line.Lly = lly;
                        line.Ury = boxNode.Attributes["ury"].Value;

                        sbLineText.Append(innerwords[i].InnerText + " ");

                        if (!string.IsNullOrEmpty(lly))
                        {
                            if (i + 1 < innerwords.Count)
                            {
                                XmlNode nextLineBoxNode = innerwords[i + 1].NextSibling;
                                nextLinelly = nextLineBoxNode.Attributes["lly"].Value;

                                if (Math.Abs(Convert.ToDouble(lly) - Convert.ToDouble(nextLinelly)) > topMarginValue)
                                {
                                    lineCounter++;
                                    line.Urx = boxNode.Attributes["urx"].Value;
                                    line.Text = Convert.ToString(sbLineText);
                                    line.LineNum = lineCounter;
                                    tetmlLines.Add(line);
                                    sbLineText.Length = 0;
                                    line = new TetmlLine();
                                    isStartingllx = true;
                                }
                            }
                            else if (i + 1 == innerwords.Count)
                            {
                                lineCounter++;
                                line.Urx = boxNode.Attributes["urx"].Value;
                                line.Text = Convert.ToString(sbLineText);
                                line.LineNum = lineCounter;
                                tetmlLines.Add(line);
                                sbLineText.Length = 0;
                                line = new TetmlLine();
                                isStartingllx = true;
                            }
                        }
                    }
                }
            } //end for loop

            if (tetmlLines.Count > 0)
                return tetmlLines;

            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public List<TetmlLine> GetAllLinesFromTetmlWithHyphenVal(XmlDocument tetDoc, int pageNum)
    {
        string lly = "";
        string nextLinelly = "";
        int lineCounter = 0;

        try
        {
            List<TetmlLine> tetmlLines = new List<TetmlLine>();
            StringBuilder sbLineText = new StringBuilder();
            //XmlNode boxNode = null;
            double topMarginValue = 1;
            TetmlLine line = new TetmlLine();
            bool isStartingllx = true;
            double hyphenWordLlx = 0;
            double hyphenWordLly = 0;

            var fontNamesDic = GetFontNamesDictionary(tetDoc);

            XmlNodeList pages = tetDoc.SelectNodes("//Page[@number='" + pageNum + "']");

            foreach (XmlNode page in pages)
            {
                XmlNodeList innerwords = page.SelectNodes("//Text");

                for (int i = 0; i < innerwords.Count; i++)
                {
                    if (i == 315)
                    {

                    }

                    if (Convert.ToString(innerwords[i]) != "")
                    {
                        //XmlNode boxNode = innerwords[i].NextSibling;
                        //lly = boxNode.Attributes["lly"].Value;

                        //if (isStartingllx)
                        //{
                        //    line.Llx = boxNode.Attributes["llx"].Value;
                        //    isStartingllx = false;
                        //}

                        if (innerwords[i].NextSibling != null &&
                            innerwords[i].NextSibling.NextSibling != null &&
                            innerwords[i].NextSibling.NextSibling.Name.Equals("Box") &&
                            innerwords[i].NextSibling.NextSibling.Attributes != null &&
                            innerwords[i].NextSibling.NextSibling.Attributes["llx"] != null)
                        {
                            hyphenWordLlx = Convert.ToDouble(innerwords[i].NextSibling.NextSibling.Attributes["llx"].Value);
                            hyphenWordLly = Convert.ToDouble(innerwords[i].NextSibling.NextSibling.Attributes["lly"].Value);
                        }

                        //Font[@id='F4']/@name

                        XmlNode boxNode = innerwords[i].NextSibling;
                        lly = boxNode.Attributes["lly"].Value;

                        if (isStartingllx)
                        {
                            if (Convert.ToDouble(lly).Equals(hyphenWordLly))
                            {
                                line.Llx = Convert.ToString(hyphenWordLlx);
                                hyphenWordLlx = 0;
                                hyphenWordLly = 0;
                            }
                            else
                            {
                                line.Llx = boxNode.Attributes["llx"].Value;
                            }

                            isStartingllx = false;
                        }

                        line.Lly = lly;
                        line.Ury = boxNode.Attributes["ury"].Value;

                        sbLineText.Append(innerwords[i].InnerText + " ");

                        if (!string.IsNullOrEmpty(lly))
                        {
                            if (i + 1 < innerwords.Count)
                            {
                                XmlNode nextLineBoxNode = innerwords[i + 1].NextSibling;
                                nextLinelly = nextLineBoxNode.Attributes["lly"].Value;

                                if (Math.Abs(Convert.ToDouble(lly) - Convert.ToDouble(nextLinelly)) > topMarginValue)
                                {
                                    lineCounter++;
                                    line.Urx = boxNode.Attributes["urx"].Value;
                                    //line.Font = boxNode.ChildNodes[0].Attributes["font"].Value;

                                    var fontDetail = fontNamesDic.Where(x => x.Key.Equals(boxNode.ChildNodes[0].Attributes["font"].Value)).ToList();
                                    if (fontDetail.Count > 0)
                                    {
                                        line.Font = fontDetail[0].Value;
                                    }
                                    
                                    line.FontSize = boxNode.ChildNodes[0].Attributes["size"].Value;
                                    line.Text = Convert.ToString(sbLineText);
                                    line.LineNum = lineCounter;
                                    tetmlLines.Add(line);
                                    sbLineText.Length = 0;
                                    line = new TetmlLine();
                                    isStartingllx = true;
                                }
                            }
                            else if (i + 1 == innerwords.Count)
                            {
                                lineCounter++;
                                line.Urx = boxNode.Attributes["urx"].Value;
                                //line.Font = boxNode.ChildNodes[0].Attributes["font"].Value;

                                var fontDetail = fontNamesDic.Where(x => x.Key.Equals(boxNode.ChildNodes[0].Attributes["font"].Value)).ToList();
                                if (fontDetail.Count > 0)
                                {
                                    line.Font = fontDetail[0].Value;
                                }

                                line.FontSize = boxNode.ChildNodes[0].Attributes["size"].Value;
                                line.Text = Convert.ToString(sbLineText);
                                line.LineNum = lineCounter;
                                tetmlLines.Add(line);
                                sbLineText.Length = 0;
                                line = new TetmlLine();
                                isStartingllx = true;
                            }
                        }
                    }
                }
            } //end for loop

            if (tetmlLines.Count > 0)
                return tetmlLines;

            return null;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public Dictionary<string, string> GetFontNamesDictionary(XmlDocument tetDoc)
    {
        if (tetDoc == null) return null;

        Dictionary<string, string> fontsDic = null;
        XmlNodeList allFontsList = tetDoc.SelectNodes("//Fonts/Font");

        if (allFontsList == null) return null;

        fontsDic = new Dictionary<string, string>();

        foreach (XmlNode font in allFontsList)
        {
            if (font.Attributes != null && font.Attributes["id"] != null)
            {
                fontsDic.Add(font.Attributes["id"].Value, font.Attributes["name"].Value);
            }
        }

        return fontsDic;
    }

    //public string ReadPdfFile(string pdfPath, int page)
    //{
    //    StringBuilder text = new StringBuilder();

    //    if (File.Exists(pdfPath))
    //    {
    //        PdfReader pdfReader = new PdfReader(pdfPath);

    //        //for (int page = 1; page <= pdfReader.NumberOfPages; page++)
    //        //{
    //        ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
    //        string currentText = PdfTextExtractor.GetTextFromPage(pdfReader, page, strategy);

    //        currentText = Encoding.UTF8.GetString(ASCIIEncoding.Convert(Encoding.Default, Encoding.UTF8, Encoding.Default.GetBytes(currentText)));
    //        text.Append(currentText);
    //        //}
    //        pdfReader.Close();
    //    }
    //    return text.ToString();
    //}

    public static string GetPdfMistakeCoords(string pageNumber, string mainXmlPath, List<string> originalText, string pdfType)
    {
        string directoryPath = "";
        string pdfPath = "";

        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]) != "")
        {
            if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
            {
                directoryPath = Common.GetTestFiles_SavingPath();
            }
            else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest"))
            {
                directoryPath = Common.GetOnePageTestFiles_SavingPath();
            }
            else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
            {
                directoryPath = Common.GetTaskFiles_SavingPath();
            }
        }

        if (pdfType.Equals("producedPdf"))
        {
            pdfPath = directoryPath + "/" + Convert.ToString(HttpContext.Current.Session["Current_PrdPdfPage"]);
        }
        else
        {
            pdfPath = directoryPath + "/" + Convert.ToString(HttpContext.Current.Session["Current_SrcPdfPage"]);
        }

        List<Word> wrdList = new List<Word>();
        Word wrd_Produced = null;

        string tetFilePath = "";

        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
        {
            tetFilePath = Common.GetTestFiles_SavingPath() + "/" + "\\" + pageNumber + ".tetml";
        }
        else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest"))
        {
            tetFilePath = Common.GetOnePageTestFiles_SavingPath() + "/" + "\\" + pageNumber + ".tetml";
        }
        else
        {
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
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }//end outer for loop
            }//end outer foreach loop
        }
        return coordinates;
    }

    public static int GetTotalMistakes_Comparison0Test(int page)
    {
        StreamReader strreader = new StreamReader(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        int totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistakeTest!='' and @page=" + page + "]").Count;
        return totalMistakes;
    }

    private static string SelectCurrentWordInPDFWithAnnotation(string pdfFilePath, List<PdfWord> wrd)
    {
        if ((wrd.Count == 0))
        {
            pdfFilePath = pdfFilePath.Replace("_Annotated", "").Replace("_Stamped", "");
            return pdfFilePath;
        }

        int page = wrd[0].PageNumber;

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

    static string RemoveWhiteSpace(string input)
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

    public XmlDocument LoadXmlDocument(string xmlPath)
    {
        if ((xmlPath == "") || (xmlPath == null) || (!File.Exists(xmlPath)))
            return null;

        StreamReader strreader = new StreamReader(xmlPath);
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        return xmlDoc;
    }

    public void LoadXml(string xmlPath)
    {
        if (!File.Exists(xmlPath)) return;

        Stream xmlStream = null;
        try
        {
            xmlStream = PdfCompareImageIndex.GetHeader(xmlPath, true);
        }
        catch
        {

        }
        StreamReader reader = new StreamReader(xmlStream);
        byte[] bytes1 = new byte[xmlStream.Length];
        xmlStream.Position = 0;
        xmlStream.Read(bytes1, 0, (int)xmlStream.Length);
        string text = System.Text.Encoding.Unicode.GetString(bytes1); //commented by aamir on 2016-07-22
        //string text = System.Text.Encoding.UTF8.GetString(bytes1);
        try
        {
            //xmlDoc = new XmlDocument();
            //xmlDoc.LoadXml(text);

            if (xmlDoc == null)
            {
                xmlDoc = new XmlDocument();
            }
            xmlDoc.LoadXml(text);
        }
        catch
        {
            //MessageBox.Show("Cannot load xml file");
        }
    }


    public XmlDocument LoadXmlFromFile(string xmlPath)
    {
        if (!File.Exists(xmlPath)) return null;

        Stream xmlStream = null;
        try
        {
            xmlStream = PdfCompareImageIndex.GetHeader(xmlPath, true);
        }
        catch
        {

        }
        byte[] bytes1 = new byte[xmlStream.Length];
        xmlStream.Position = 0;
        xmlStream.Read(bytes1, 0, (int)xmlStream.Length);
        string text = Encoding.Unicode.GetString(bytes1); //aamir on 2016-06-20
        //string text = Encoding.UTF8.GetString(bytes1);
        try
        {
            if (string.IsNullOrEmpty(text)) return null;
            XmlDocument newDoc = new XmlDocument();
            newDoc.LoadXml(text);
            return newDoc;
        }
        catch
        {
            return null;
        }
    }

    public XmlDocument xmlDoc
    {
        get
        {
            if (HttpContext.Current.Session["xmlDoc"] != null)
            {
                return ((XmlDocument)HttpContext.Current.Session["xmlDoc"]);
            }
            return null;
        }
        set
        {
            HttpContext.Current.Session["xmlDoc"] = value;
        }
    }

    //public static string ExtractPage(int pageNum)
    //{
    //    string inputFilePath = "";
    //    string outputFile = "";

    //    if (Convert.ToString(Session["ComparisonTask"]) != "")
    //    {
    //        if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
    //        {
    //            inputFilePath = Common.GetDirectoryPath() + "\\Tests\\" + Convert.ToString(Session["ComparisonTestUser_Email"]) +
    //                            "/ComparisonTests/" + Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"])) + "/" +
    //                            Convert.ToString(Session["SrcPDF"]);

    //            outputFile = Common.GetDirectoryPath() + "\\Tests\\" + Convert.ToString(Session["ComparisonTestUser_Email"]) +
    //                            "/ComparisonTests/" + Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"])) + "/" +
    //                            "/" + pageNum + ".pdf";
    //        }
    //        else if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
    //        {
    //            inputFilePath = Common.GetDirectoryPath() + "\\Tests\\" + Convert.ToString(Session["ComparisonTestUser_Email"]) +
    //                             "/ComparisonTests/" + Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"])) + "/" +
    //                             Convert.ToString(Session["SrcPDF"]);

    //            outputFile = Common.GetDirectoryPath() + "\\Tests\\" + Convert.ToString(Session["ComparisonTestUser_Email"]) +
    //                            "/ComparisonTests/" + Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"])) + "/" +
    //                            "/" + pageNum + ".pdf";
    //        }
    //        else if (Convert.ToString(Session["ComparisonTask"]).Equals("task"))
    //        {
    //            inputFilePath = Common.GetDirectoryPath() + Convert.ToString(Session["BookId"]) + "/" + Convert.ToString(Session["BookId"]) +
    //                            "-1/Comparison/Comparison-" + Convert.ToString(Session["comparisonType"]) + "/" + Convert.ToString(Session["userId"]) +
    //                            "/" + Convert.ToString(Session["SrcPDF"]);

    //            outputFile = Common.GetDirectoryPath() + Convert.ToString(Session["BookId"]) + "/" + Convert.ToString(Session["BookId"]) +
    //                         "-1/Comparison/Comparison-" + Convert.ToString(Session["comparisonType"]) + "/" + Convert.ToString(Session["userId"]) +
    //                         "/" + "\\" + pageNum + ".pdf";

    //        }
    //    }

    //    //string inputFilePath = Common.GetTestFiles_SavingPath() + Convert.ToString(Session["SrcPDF"]);
    //    //string outputFile = Common.GetTestFiles_SavingPath() + pageNum + ".pdf"; 
    //    //string inputFilePath = Common.GetDirectoryPath() + Convert.ToString(Session["BookId"]) + "/" + Convert.ToString(Session["BookId"]) +
    //    //       "-1/Comparison/Comparison-" + Convert.ToString(Session["comparisonType"]) + "/" + Convert.ToString(Session["userId"]) + "/" + Convert.ToString(Session["SrcPDF"]);

    //    //string outputFile = Common.GetDirectoryPath() + Convert.ToString(Session["BookId"]) + "/" + Convert.ToString(Session["BookId"]) +
    //    //       "-1/Comparison/Comparison-" + Convert.ToString(Session["comparisonType"]) + "/" + Convert.ToString(Session["userId"]) + "/" + "\\" + pageNum + ".pdf";

    //    ExtractPages(inputFilePath, outputFile, pageNum, pageNum);
    //    Createtetml(outputFile);
    //    return outputFile;
    //}

    //private static void ExtractPages(string inputFile, string outputFile, int start, int end)
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
    //    Document inputDoc = new Document(inputPdf.GetPageSizeWithRotation(1));

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

    public string GetTextFromAllPages(String pdfPath)
    {
        try
        {
            if (!File.Exists(pdfPath)) return "";

            PdfReader reader = new PdfReader(pdfPath);

            if (reader == null) return "";

            StringBuilder pageText = new StringBuilder();

            for (int i = 1; i <= reader.NumberOfPages; i++)
                pageText.Append(PdfTextExtractor.GetTextFromPage(reader, i, new SimpleTextExtractionStrategy()));

            return pageText.ToString();
        }
        catch (Exception)
        {
            return "";
        }
    }

    public static string GetTextFromPdf(String pdfPath)
    {
        try
        {
            if (!File.Exists(pdfPath)) return "";

            PdfReader reader = new PdfReader(pdfPath);

            if (reader == null) return "";

            StringBuilder pageText = new StringBuilder();

            for (int i = 1; i <= reader.NumberOfPages; i++)
                pageText.Append(PdfTextExtractor.GetTextFromPage(reader, i, new SimpleTextExtractionStrategy()));

            return pageText.ToString();
        }
        catch (Exception)
        {
            return "";
        }
    }

    public static string Createtetml(string filePath)
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
        string wordTETMLPath = DirectoryPath + "\\" + System.IO.Path.GetFileNameWithoutExtension(filePath) + ".tetml";
        //tetFile = XmlFile;
        //string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks}\" -o \"" + XmlFile + "\" \"" + PDFFilePath + "\"";
        string strParameter =
            "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks} clippingarea={cropbox}\" -o \"" +
            wordTETMLPath + "\" \"" + filePath + "\"";
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
    }

    public string GetProducePdf(int PageNum, XmlDocument xmlFromRhyw)
    {
        string pageXMLSavedPath = "";

        XmlDocument pageXML = GetPageXmlDoc(PageNum.ToString(), xmlFromRhyw);

        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]) != "")
        {
            if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
            {
                pageXMLSavedPath = Common.GetDirectoryPath() + "\\Tests\\" + Convert.ToString(HttpContext.Current.Session["ComparisonTestUser_Email"]) +
                                   "/ComparisonTests/" + System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["SrcPDF"])) + "/" + "\\Produced_" + PageNum + ".xml";
            }
            else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest"))
            {
                pageXMLSavedPath = Common.GetDirectoryPath() + "\\Tests\\" + Convert.ToString(HttpContext.Current.Session["ComparisonTestUser_Email"]) +
                                   "/ComparisonTests/" + System.IO.Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["SrcPDF"])) + "/" + "\\Produced_" + PageNum + ".xml";
            }
            else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
            {
                //pageXMLSavedPath = @"C:\Users\Aamir\Desktop\New folder\output" + "\\Produced_" + PageNum + ".xml";

                pageXMLSavedPath = Common.GetDirectoryPath() + Convert.ToString(HttpContext.Current.Session["BookId"]) + "/" + Convert.ToString(HttpContext.Current.Session["BookId"]) +
                                   "-1/Comparison/Comparison-" + Convert.ToString(HttpContext.Current.Session["comparisonType"]) + "/" + Convert.ToString(HttpContext.Current.Session["userId"]) +
                                   "/" + "\\Produced_" + PageNum + ".xml";
            }
            else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("comparisonEntryTest") ||
                    Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("CompUpgradedSampleTest") ||
                    Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("CompUpgradedStartTest"))
            {
                //pageXMLSavedPath = Common.GetDirectoryPath() + "\\Tests\\" + Convert.ToString(HttpContext.Current.Session["ComparisonTestUser_Email"]) +
                //                  "/ComparisonTests/" + Path.GetFileNameWithoutExtension(Convert.ToString(HttpContext.Current.Session["SrcPDF"])) + "/" + 
                //                  "\\Produced_" + PageNum + ".xml";

                pageXMLSavedPath = Common.GetDirectoryPath() + "\\Tests\\" + Convert.ToString(HttpContext.Current.Session["ComparisonTestUser_Email"]) +
                                       "/ComparisonTests/" + Convert.ToString(HttpContext.Current.Session["TestName"]) + "/" +
                                       Convert.ToString(HttpContext.Current.Session["TestName"]) + "-1/Comparison/" +
                                       "\\Produced_" + PageNum + ".xml";
            }
        }

        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
        {
            //LoadXml(pageXMLSavedPath);
            double normalx = 0;
            double normalIndent = 0;
            double normalFont = Convert.ToDouble(HttpContext.Current.Session["normalFont"]);
            //List<XmlNode> abnormalFontLines = AbnormalFontCheck(pageXML, xmlDoc);

            List<XmlNode> abnormalFontLines = new List<XmlNode>();

            if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["EvenOddLeftDifference"])))
            {
                if (Convert.ToString(HttpContext.Current.Session["EvenOddLeftDifference"]).Equals("true"))
                {
                    if (PageNum % 2 == 0)
                    {
                        normalx = Convert.ToDouble(HttpContext.Current.Session["normalx_EvenPages"]);
                        normalIndent = Convert.ToDouble(HttpContext.Current.Session["normalIndent_EvenPages"]);
                    }
                    else if (PageNum % 2 == 1)
                    {
                        normalx = Convert.ToDouble(HttpContext.Current.Session["normalx_OddPages"]);
                        normalIndent = Convert.ToDouble(HttpContext.Current.Session["normalIndent_OddPages"]);
                    }
                }
            }
            else
            {
                normalx = Convert.ToDouble(HttpContext.Current.Session["normalx"]);
                normalIndent = Convert.ToDouble(HttpContext.Current.Session["normalIndent"]);
            }

            string pageText = GetTextFromAllPages(pageXMLSavedPath.Replace("Produced_" + PageNum + ".xml", PageNum + ".pdf"));

            List<string> linesWithHyphenList = new List<string>();

            if (!string.IsNullOrEmpty(pageText))
                linesWithHyphenList = GetHyphenLines(pageText);

            //XmlNodeList uParaList = pageXML.SelectNodes("//upara");
            ////Setting upara's next line to normal indent in case of hyphen in previous line
            //if (uParaList != null)
            //{
            //    for (int i = 0; i < uParaList.Count; i++)
            //    {
            //        if (uParaList[i].ChildNodes != null)
            //        {
            //            //for (int j = 0; j < uParaList[i].ChildNodes.Count; j++)
            //            //{
            //            if (uParaList[i].ChildNodes.Count > 1)
            //            {
            //                XmlNode firstChild = uParaList[i].ChildNodes[0];

            //                if (firstChild.Attributes["left"] != null)
            //                {
            //                    double lineX = Math.Round(Convert.ToDouble(firstChild.Attributes["left"].Value));
            //                    double diffr = Math.Abs(normalx - lineX);
            //                    if (diffr > 2)
            //                    {
            //                        //XmlNode secondChild = uParaList[i].ChildNodes[1];

            //                        //string coord = uParaList[i].Attributes["coord"].Value;
            //                        //string page = uParaList[i].Attributes["page"].Value;

            //                        XmlNode Actualline =
            //                            xmlDoc.SelectSingleNode("//ln[@coord='" + coord + "' and @page='" + page + "']");

            //                        if (Actualline != null)
            //                        {
            //                            while (Actualline.NextSibling != null)
            //                            {
            //                                ((XmlElement) Actualline.NextSibling).SetAttribute("left",
            //                                    Convert.ToString(normalx));

            //                                Actualline = Actualline.NextSibling;
            //                            }
            //                        }
            //                    }
            //                }
            //            }
            //            //}
            //            //if (uParaList[i].ChildNodes.Name.Equals("upara"))
            //            //{
            //            //    if (uParaList[i].FirstChild)
            //            //    {

            //            //    }
            //            //    if (uParaList[i].Attributes["left"] != null)
            //            //    {
            //            //        double lineX = Math.Round(Convert.ToDouble(uParaList[i].Attributes["left"].Value));
            //            //        double diffr = Math.Abs(normalx - lineX);
            //            //        if (diffr > 2)
            //            //        {
            //            //            string coord = uParaList[i].Attributes["coord"].Value;
            //            //            string page = uParaList[i].Attributes["page"].Value;

            //            //            XmlNode Actualline =
            //            //                xmlDoc.SelectSingleNode("//ln[@coord='" + coord + "' and @page='" + page + "']");

            //            //            if (Actualline != null)
            //            //            {
            //            //                while (Actualline.NextSibling != null)
            //            //                {
            //            //                    ((XmlElement)Actualline.NextSibling).SetAttribute("left",
            //            //                        Convert.ToString(normalx));

            //            //                    Actualline = Actualline.NextSibling;
            //            //                }
            //            //            }
            //            //        }
            //            //    }
            //            //}
            //        }
            //    }
            //}
            ////end

            //2017-09-05 not required for teml 5.1
            ////XmlNodeList PageLines = pageXML.SelectNodes("//ln");

            ////foreach (XmlNode line in PageLines)
            ////{
            ////    if (line.Attributes["left"] != null)
            ////    {
            ////        double lineX = Math.Round(Convert.ToDouble(line.Attributes["left"].Value));
            ////        double diffr = Math.Abs(normalx - lineX);
            ////        if (diffr > 2)
            ////        {
            ////            //Moving Hyphen's containing next line to normal indentation
            ////            //Case 1 When a Hyphen containging line came is the 3rd or greater line in a para so that lines below it contains normal indentation.
            ////            if (line.PreviousSibling != null)
            ////            {
            ////                if (linesWithHyphenList.Any(x => RemoveWhiteSpace(x).Equals(RemoveWhiteSpace(line.PreviousSibling.InnerText))))
            ////                {
            ////                    ((XmlElement)line).SetAttribute("left", Convert.ToString(normalx));
            ////                }
            ////            }
            ////            //End Case 1

            ////            if (line.NextSibling != null)
            ////            {
            ////                XmlNode nextLine = line.NextSibling;

            ////                if (nextLine.Name != "break")
            ////                {
            ////                    lineX = Math.Round(Convert.ToDouble(nextLine.Attributes["left"].Value));

            ////                    diffr = Math.Abs(normalx - lineX);
            ////                    if (diffr > 2)
            ////                    {
            ////                        if (!linesWithHyphenList.Any(x => RemoveWhiteSpace(x).Equals(RemoveWhiteSpace(line.InnerText))))
            ////                        {
            ////                            string coord = line.Attributes["coord"].Value;
            ////                            string page = line.Attributes["page"].Value;
            ////                            //page xml is not proper xml so original line is selected from Main XML in this case all sections are selected chapter, 
            ////                            //or level either not present on current page...
            ////                            XmlNode Actualline =
            ////                                xmlDoc.SelectSingleNode("//ln[@coord='" + coord + "' and @page='" + page +
            ////                                                        "']");
            ////                            if (line.ParentNode != null)
            ////                            {
            ////                                if (!line.ParentNode.Name.Equals("section-title"))
            ////                                {
            ////                                    if (Actualline != null && (!abnormalFontLines.Contains(Actualline)))
            ////                                    {
            ////                                        abnormalFontLines.Add(Actualline);
            ////                                        //if a single line is much indented and next lines are normal.. but whole para is not spara then all lines
            ////                                        //are selected here...
            ////                                        while (Actualline.NextSibling != null)
            ////                                        {
            ////                                            ((XmlElement)Actualline.NextSibling).SetAttribute("left", Convert.ToString(normalx));

            ////                                            Actualline = Actualline.NextSibling;
            ////                                            if (!abnormalFontLines.Contains(Actualline))
            ////                                            {
            ////                                                abnormalFontLines.Add(Actualline);
            ////                                            }
            ////                                        }
            ////                                    }
            ////                                }
            ////                            }

            ////                            #region |Previous logic to check if it is in spara then ignore|

            ////                            //XmlNode LinePara = line.SelectSingleNode("ancestor::upara|ancestor::spara|ancestor::npara");
            ////                            //if (LinePara != null && (LinePara.Name.Equals("upara")))
            ////                            //{
            ////                            //    string coord = line.Attributes["coord"].Value;
            ////                            //    string page = line.Attributes["page"].Value;
            ////                            //    page xml is not proper xml so original line is selected from Main XML in this case all sections are selected chapter, or level either not present on current page...
            ////                            //    XmlNode Actualline = GlobalVar.PBPDocument.SelectSingleNode("//ln[@coord='" + coord + "' and @page='" + page + "']");
            ////                            //    if (Actualline != null && (!abnormalFontLines.Contains(Actualline)))
            ////                            //    {
            ////                            //        abnormalFontLines.Add(Actualline);
            ////                            //        if a single line is much indented and next lines are normal.. but whole para is not spara then all lines are selected here...
            ////                            //        while (Actualline.NextSibling != null)
            ////                            //        {
            ////                            //            Actualline = Actualline.NextSibling;
            ////                            //            if (!abnormalFontLines.Contains(Actualline))
            ////                            //            {
            ////                            //                abnormalFontLines.Add(Actualline);
            ////                            //            }
            ////                            //        }
            ////                            //    }
            ////                            //}

            ////                            #endregion
            ////                        }
            ////                        //Moving Hyphen's containing next line to normal indentation
            ////                        //Case 2 When a Hyphen containging line came is the 1st line in a para so that its second line has abnormal indentation.
            ////                        else
            ////                        {
            ////                            ((XmlElement)nextLine).SetAttribute("left", Convert.ToString(normalx));
            ////                        }
            ////                    }
            ////                }
            ////            }
            ////            else
            ////            {
            ////                diffr = Math.Abs(normalIndent - lineX);
            ////                if (diffr > 1)
            ////                {
            ////                    if (line.ParentNode != null)
            ////                    {
            ////                        if (!line.ParentNode.Name.Equals("section-title"))
            ////                        {
            ////                            if (line.PreviousSibling != null)
            ////                            {
            ////                                if (!linesWithHyphenList.Any(x => RemoveWhiteSpace(x).Equals(RemoveWhiteSpace(line.PreviousSibling.InnerText))))
            ////                                {
            ////                                    string coord = line.Attributes["coord"].Value;
            ////                                    string page = line.Attributes["page"].Value;
            ////                                    //page xml is not proper xml so original line is selected from Main XML in this case all sections are selected chapter,
            ////                                    //or level either not present on current page...
            ////                                    XmlNode Actualline = xmlDoc.SelectSingleNode("//ln[@coord='" + coord + "' and @page='" + page + "']");
            ////                                    if (Actualline != null && (!abnormalFontLines.Contains(Actualline)))
            ////                                    {
            ////                                        abnormalFontLines.Add(Actualline);
            ////                                    }
            ////                                }
            ////                            }
            ////                        }
            ////                    }
            ////                }
            ////            }

            ////            #region |Commented|

            ////            //else if (diffr > 2)
            ////            //{
            ////            //    XmlNode LinePara = line.SelectSingleNode("ancestor::upara|ancestor::spara|ancestor::npara");
            ////            //    if (LinePara != null && (LinePara.Name.Equals("upara")))
            ////            //    {
            ////            //        string coord = line.Attributes["coord"].Value;
            ////            //        string page = line.Attributes["page"].Value;
            ////            //        //page xml is not proper xml so original line is selected from Main XML in this case all sections are selected chapter, or level either not present on current page...
            ////            //        XmlNode Actualline = mainDoc.SelectSingleNode("//ln[@coord='" + coord + "' and @page='" + page + "']");
            ////            //        if (Actualline!=null && (!abnormalFontLines.Contains(Actualline)))
            ////            //        {
            ////            //            abnormalFontLines.Add(Actualline);
            ////            //            //if a single line is much indented and next lines are normal.. but whole para is not spara then all lines are selected here...
            ////            //            while (Actualline.NextSibling != null)
            ////            //            {
            ////            //                Actualline = Actualline.NextSibling;
            ////            //                if (!abnormalFontLines.Contains(Actualline))
            ////            //                {
            ////            //                    abnormalFontLines.Add(Actualline);
            ////            //                }
            ////            //            }
            ////            //        }
            ////            //    }
            ////            //}

            ////            #endregion
            ////        }
            ////    }
            ////}

            ////2016-03-31 Abnormal lines red color is commented and will be used for complex books.
            //if (abnormalFontLines != null && abnormalFontLines.Count > 0)
            //{
            //    foreach (XmlNode line in abnormalFontLines)
            //    {
            //        //if (IsAbNormalLine(linesWithHyphenList, line.InnerText))
            //        //{
            //        if (line.Attributes["coord"] != null)
            //        {
            //            string coord = line.Attributes["coord"].Value;
            //            string page = line.Attributes["page"].Value;
            //            XmlNode pageline = pageXML.SelectSingleNode("//ln[@coord='" + coord + "' and @page='" + page + "']");
            //            if (pageline != null)
            //            {
            //                if (pageline.Attributes["left"] != null)
            //                {
            //                    pageline.Attributes["left"].Value = Convert.ToString(normalx);
            //                    pageline.Attributes["fontsize"].Value = Convert.ToString(normalFont);

            //                    //Create a new attribute for making color of abnormal line red
            //                    XmlAttribute attr = pageXML.CreateAttribute("color");
            //                    attr.Value = "1";
            //                    pageline.Attributes.Append(attr);
            //                }
            //            }
            //        }
            //        //}
            //    }
            //}
        }

        ///////////////////////////////////////////////////////////////////////////////////////////////////////////

        pageXML.Save(pageXMLSavedPath);

        string prodFilePath = pageXMLSavedPath.TrimEnd(".xml".ToCharArray()) + ".pdf";
        try
        {
            string result = GenearatePDFPreview(pageXMLSavedPath, prodFilePath, PageNum);

            if (!result.Equals("Successfull"))
            {
                prodFilePath = "";
            }

            else if (result.Equals("Successfull"))
            {
                Createtetml(prodFilePath);
            }
        }
        finally
        {
            //imgValidator.Dispose();
        }
        return prodFilePath;
    }

    private static List<string> GetHyphenLines(string pageText)
    {
        List<string> linesWithHyphenList = new List<string>();
        StringBuilder hyphenLine = new StringBuilder();

        if (!string.IsNullOrEmpty(pageText))
        {
            List<string> linesList = pageText.Split('\n').ToList();
            for (int i = 0; i < linesList.Count; i++)
            {
                string line = linesList[i].Trim();

                if (!string.IsNullOrEmpty(line))
                {
                    if (line[line.Length - 1].Equals('-'))
                    {
                        hyphenLine.Append(line.Remove(line.Length - 1, 1));

                        if (i + 1 < linesList.Count)
                        {
                            //string nextLine = linesList[i + 1].Trim();
                            //List<string> wordsList = nextLine.Split(' ').ToList();

                            //if (wordsList != null && wordsList.Count == 1) linesWithHyphenList.Add(wordsList[0]);

                            //else if (wordsList != null && wordsList.Count > 1)
                            //{
                            //    wordsList.RemoveAt(0);
                            //    linesWithHyphenList.Add(string.Join(" ", wordsList.ToArray()));
                            //}

                            string nextLine = linesList[i + 1].Trim();
                            List<string> wordsList = nextLine.Split(' ').ToList();
                            if (wordsList != null && wordsList.Count > 0)
                            {
                                hyphenLine.Append(wordsList[0]);
                                linesWithHyphenList.Add(Convert.ToString(hyphenLine));
                                hyphenLine.Clear();
                            }
                        }
                    }
                }
            }
        }
        return linesWithHyphenList;
    }

    private static string GetHyphensNextLineWord(string pageText)
    {
        List<string> linesWithHyphenList = new List<string>();
        StringBuilder hyphenLineRemainingWord = new StringBuilder();

        if (!string.IsNullOrEmpty(pageText))
        {
            List<string> linesList = pageText.Split('\n').ToList();
            for (int i = 0; i < linesList.Count; i++)
            {
                string line = linesList[i].Trim();

                if (!string.IsNullOrEmpty(line))
                {
                    if (line[line.Length - 1].Equals('-'))
                    {
                        if (i + 1 < linesList.Count)
                        {
                            string nextLine = linesList[i + 1].Trim();
                            List<string> wordsList = nextLine.Split(' ').ToList();
                            if (wordsList != null && wordsList.Count > 0)
                            {
                                hyphenLineRemainingWord.Append(wordsList[0]);
                            }
                        }
                    }
                }
            }
        }
        return Convert.ToString(hyphenLineRemainingWord);
    }

    //public bool IsAbNormalLine(List<string> hyphensNextLineList, string xmlLine)
    //{
    //    bool status = true;
    //    if (hyphensNextLineList.Any(x => RemoveWhiteSpace(x).Equals(RemoveWhiteSpace(xmlLine)))) status = false;
    //    else if (MatchLinesByWords(hyphensNextLineList, xmlLine)) status = false;
    //    return status;
    //}

    //public bool MatchLinesByWords(List<string> hyphensNextLineList, string xmlLine)
    //{
    //    //List<string> xmlwordsList = xmlLine.Split(' ');

    //    //foreach (string line in hyphensNextLineList)
    //    //{
    //    //    List<string> wordsList = line.Split(' ').ToList();

    //    //    for (int i = 0; i < wordsList.Count; i++)
    //    //    {
    //    //    }
    //    //}

    //    //textToCheck = ReplaceWhiteSpace_(word.Trim()).Split(',');
    //    //textToCheck = textToCheck.Where(x => (!string.IsNullOrEmpty(x))).ToArray();



    //    //         for (int i = 0; i < innerwords.Count; i++)
    //    //         {
    //    //             if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 0))
    //    //             {
    //    //                 if (innerwords[i].InnerText.Replace(",", "").Trim().Equals(textToCheck[0]))
    //    //                 {
    //    //                     if (textToCheck.Length == 1)
    //    //                     {

    //    //                     }//end

    //    //                     if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 1))
    //    //                     {
    //    //                         if ((i + 1) < innerwords.Count)
    //    //                         {
    //    //                             if (innerwords[i + 1].InnerText.Replace(",", "").Trim().Equals(textToCheck[1]))
    //    //                             {
    //    //                                 //Calculating coordinates for Highlighting 2 words
    //    //                                 if (textToCheck.Length == 2)
    //    //                                 {

    //    //                                 }//end

    //    //                                 if ((Convert.ToString(innerwords[i]) != "") && (textToCheck.Length > 2))
    //    //                                 {
    //    //                                     if (innerwords[i + 2].InnerText.Replace(",", "").Trim().Equals(textToCheck[2]))
    //    //                                     {

    //    //                                     }
    //    //                                 }
    //    //                             }
    //    //                         }
    //    //                     }
    //    //                 }
    //    //             }
    //    return false;
    //}

    //public string RemoveSpecialCharacters(string input)
    //{
    //    return Regex.Replace(input, @"[^0-9a-zA-Z]+", "");

    //    //Regex r = new Regex("(?:[^A-Za-z0-9]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
    //    //return r.Replace(input, String.Empty);
    //}

    public string GenearatePDFPreview(string srcXMLFile, string targetPDFPath, int PageNum)
    {
        return ShowPdfPreview(srcXMLFile, targetPDFPath, PageNum);
    }

    private string ShowPdfPreview(string xmlfile, string PdfFile, int PageNum)
    {
        string xslfile = "";

        string xslCoordPath = "";
        string xepfile = ConfigurationManager.AppSettings["XEPPath"];

        if (File.Exists(xmlfile))
        {
            //Common comObj = new Common();
            //XmlDocument xmlDocOrigXml = comObj.LoadXmlDocument(xmlfile);
            //var nodes = xmlDocOrigXml.SelectNodes("//npara");

            //if (nodes != null && nodes.Count > 0)
            //    xslfile = Convert.ToString(HttpContext.Current.Session["setDefaultXSL"]).Replace("_Coord", "");
            //else
            xslfile = Convert.ToString(HttpContext.Current.Session["setDefaultXSL"]);
        }

        string retMessage = "";
        try
        {
            //string xslCoordPath = Common.GetXSLCoordDirectoryPath();


            if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]) != "")
            {
                if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
                {
                    xslCoordPath = Common.GetXSLCoordDirectoryPath();
                    //HttpContext.Current.Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPathCoord"];
                    HttpContext.Current.Session["setDefaultXSL"] = xslCoordPath;
                }
                else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest"))
                {
                    xslCoordPath = Common.GetXSLCoordDirectoryPath();
                    //HttpContext.Current.Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPathCoord"];
                    HttpContext.Current.Session["setDefaultXSL"] = xslCoordPath;
                }
                else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("comparisonEntryTest") ||
                    Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("CompUpgradedSampleTest") ||
                    Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("CompUpgradedStartTest"))
                {
                    xslCoordPath = Common.GetXSLDirectoryPath_StartTest();
                    //HttpContext.Current.Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPathCoord"];
                    HttpContext.Current.Session["setDefaultXSL"] = xslCoordPath;
                }
                else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
                {
                    xslCoordPath = Common.GetXSLCoordDirectoryPath();
                    //HttpContext.Current.Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPathCoord"];
                    HttpContext.Current.Session["setDefaultXSL"] = xslCoordPath;
                }
            }

            if (xslCoordPath == "")
                return "";

            //if (Convert.ToString(HttpContext.Current.Session["setDefaultXSL"]) != "")
            //{
            //    xslfile = @"C:\XSL\XSLS\PBPBook.xsl"; // Convert.ToString(HttpContext.Current.Session["setDefaultXSL"]);
            //}

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
            }
            pPdfPreview.WaitForExit();
            // Check if PDF size is greater than zero
            // IF-ELSE Block Added
            FileInfo PdfFileInfo = new FileInfo(PdfFile);
            if (File.Exists(PdfFile) && PdfFileInfo.Length > 0)
            {
                if (File.Exists(PdfFile))
                {
                    PdfReader pdfFile = new PdfReader(PdfFile);
                    int pageCount = pdfFile.NumberOfPages;

                    string outPutPdfPath = System.IO.Path.GetDirectoryName(PdfFile);

                    if (pageCount > 1)
                    {
                        HttpContext.Current.Session["largerPrdPdfCount"] = pageCount;

                        string currentPage = Convert.ToString(HttpContext.Current.Session["MainCurrPage"]);
                        HttpContext.Current.Session["ProducedPdfSubPage"] = 1;

                        for (int page = 1; page <= pageCount; page++)
                        {
                            ExtractPages(PdfFile, outPutPdfPath + "\\Produced_" + currentPage + "_" + page + ".pdf", page);
                            Createtetml(outPutPdfPath + "\\Produced_" + currentPage + "_" + page + ".pdf");
                        }
                    }
                }

                retMessage = "Successfull";
                //AddAnottation(PdfFile, PageNum);
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


    //private string AddAnottation(string outputFile, int pageNum)
    //{
    //    string mainXml = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);

    //    Common obj = new Common();
    //    XmlDocument xmlDoc = obj.LoadXmlFromFile(mainXml.Replace(".xml", ".rhyw"));

    //    XmlDocument pageXML = Common.GetPageXmlDoc(Convert.ToString(pageNum), xmlDoc);
    //    string dirPath = System.IO.Path.GetDirectoryName(outputFile);
    //    string xmlPath = dirPath + "\\" + pageNum + ".xml";
    //    pageXML.Save(xmlPath);
    //    string prodFilePath = xmlPath.TrimEnd(".xml".ToCharArray()) + ".pdf";
    //    prodFilePath = AddAnnotationInPDF(outputFile, xmlPath);
    //    return prodFilePath;
    //}

    //private string AddAnnotationInPDF(string pdfFilePath, string xmlFilePath)
    //{
    //    XmlDocument xmlDoc = new XmlDocument();
    //    xmlDoc.Load(xmlFilePath);
    //    XmlNodeList xmlElements = xmlDoc.SelectNodes("//upara|//spara|//npara|//table|//section|//box");
    //    ArrayList annotations = new ArrayList();
    //    string anotHeading = "";
    //    string anotText = "";
    //    float llx = 0;
    //    float lly = 0;
    //    float urx = 0;
    //    float ury = 0;
    //    RHYWAnnotation rhywAnnot = null;

    //    foreach (XmlNode xmlElement in xmlElements)
    //    {
    //        XmlNode lnNode = xmlElement.SelectSingleNode("ln");

    //        if (lnNode != null)
    //        {
    //            XmlAttribute attrNode = lnNode.Attributes["coord"];
    //            string cordinates = attrNode.Value;
    //            anotHeading = xmlElement.Name;
    //            //anotText = xmlElement.Name;

    //            if (anotHeading.Equals("image"))
    //                rhywAnnot = new RHYWAnnotation(AnnotType.Image);
    //            else if (anotHeading.Equals("upara"))
    //                rhywAnnot = new RHYWAnnotation(AnnotType.Upara);
    //            else if (anotHeading.Equals("spara"))
    //                rhywAnnot = new RHYWAnnotation(AnnotType.Spara);
    //            else if (anotHeading.Equals("npara"))
    //                rhywAnnot = new RHYWAnnotation(AnnotType.Npara);
    //            else if (anotHeading.Equals("section"))
    //                rhywAnnot = new RHYWAnnotation(AnnotType.Chapter);
    //            else if (anotHeading.Equals("box"))
    //                rhywAnnot = new RHYWAnnotation(AnnotType.Box);

    //           // rhywAnnot.Description = lnNode.InnerText;
    //            llx = float.Parse(cordinates.Split(':')[0]);
    //            lly = float.Parse(cordinates.Split(':')[1]);
    //            urx = float.Parse(cordinates.Split(':')[2]);
    //            ury = float.Parse(cordinates.Split(':')[3]);
    //            float leftOffset = 20f;
    //            rhywAnnot.llx = llx;
    //            rhywAnnot.lly = lly;
    //            rhywAnnot.urx = urx;
    //            rhywAnnot.ury = ury;
    //            rhywAnnot.llx -= leftOffset;
    //        }
    //        WriteAnnotationInFile(pdfFilePath, 1, anotHeading, anotText, llx, lly, urx, ury);
    //        annotations.Add(rhywAnnot);//aamir
    //    }
    //    string annotedFilePath = WriteAnnotationsInFile(pdfFilePath, annotations);
    //    return annotedFilePath;
    //}

    //private string WriteAnnotationsInFile(string pdfFilePath, ArrayList annotations)
    //{
    //    try
    //    {
    //        //string origFile = pdfFilePath;
    //        //string filename=Path.GetFileNameWithoutExtension(pdfFilePath)+ "_Final.pdf";            
    //        string newFile = System.IO.Path.GetDirectoryName(pdfFilePath) + "\\" + System.IO.Path.GetFileNameWithoutExtension(pdfFilePath) +
    //                         "_Annotated.pdf";
    //        int pageNum = 1;
    //        // open the reader
    //        PdfReader reader = new PdfReader(pdfFilePath);
    //        iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(pageNum);
    //        Document document = new Document(size);

    //        // open the writer
    //        FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
    //        PdfWriter writer = PdfWriter.GetInstance(document, fs);
    //        document.Open();

    //        // the pdf content
    //        PdfContentByte cb = writer.DirectContent;
    //        for (int i = 0; i < annotations.Count; i++)
    //        {
    //            RHYWAnnotation anot = (RHYWAnnotation)annotations[i];
    //            string annotText = anot.Description;
    //            string annotHeading = anot.GetHeading();
    //            float llx = anot.llx;
    //            float lly = anot.lly;
    //            float urx = anot.urx;
    //            float ury = anot.ury;

    //            //document.Add(new Annotation(annotHeading, annotText, llx, lly, urx, ury));
    //            //writer.AddAnnotation(PdfAnnotation.CreateText(writer, new Rectangle(50, 620, 70, 640),"NewParagraph", "...", false, "Comment"));

    //            //writer.AddAnnotation(PdfAnnotation.CreateText(writer, new Rectangle(llx, lly, urx, ury), annotHeading, annotText, false, "Comment"));
    //            ////PdfAnnotation annotation = new PdfAnnotation(writer, new Rectangle(llx, lly, urx, ury));

    //            PdfAnnotation annotation = new PdfAnnotation(writer, llx, lly, urx, ury, new PdfString("upara"), new PdfString("abcd sfsfsfs "));

    //            annotation.Put(PdfName.SUBTYPE, PdfName.TEXT);
    //           // annotation.Put(PdfName.OPEN, PdfBoolean.PDFFALSE);
    //            annotation.Put(PdfName.T, new PdfString(annotHeading));
    //            //annotation.Put(PdfName.C, new PdfArray(new float[] { 0.0f, 1.0f, 1.0f, 0.0f }));
    //            //annotation.Put(PdfName.CONTENTS, new PdfString(annotText));
    //            writer.AddAnnotation(annotation);

    //            //MemoryStream memoryStream = new MemoryStream();
    //            //using (PdfStamper stamper = new PdfStamper(reader, memoryStream))
    //            //{
    //            //    PdfAnnotation popup = PdfAnnotation.CreatePopup(writer, new Rectangle(size.GetLeft(0) + 10, size.GetBottom(0) + 10,
    //            // size.GetLeft(0) + 200, size.GetBottom(0) + 100), null, false);
    //            //    stamper.AddAnnotation(popup, 1);
    //            //}
    //        }
    //        // create the new page and add it to the pdf
    //        PdfImportedPage page1 = writer.GetImportedPage(reader, pageNum); //Importing page 1 of the documetn
    //        //PdfImportedPage page2 = writer.GetImportedPage(reader, 1);
    //        cb.AddTemplate(page1, 0, 0);
    //        //cb.AddTemplate(page2, 0, 0);

    //        // close the streams and voilá the file should be changed :)
    //        cb.ClosePath();
    //        document.Close();
    //        fs.Close();
    //        writer.Close();
    //        reader.Close();

    //        //File.Delete(pdfFilePath);
    //        return newFile;

    //        //return Server.MapPath(newFile);
    //        //return "http://46.4.195.234/pdfweb/newfile.pdf";
    //        //this.Dispose();
    //    }
    //    catch (Exception exp)
    //    {
    //        return exp.Message;
    //    }
    //}

    //private string WriteAnnotationInFile(string pdfFilePath, int pageNum, string annotHeading, string annotText, float llx, float lly, float urx, float ury)
    //{
    //    try
    //    {
    //        string origFile = pdfFilePath;
    //        string newFile = pdfFilePath + "_2.pdf";

    //        // open the reader
    //        PdfReader reader = new PdfReader(origFile);
    //        iTextSharp.text.Rectangle size = reader.GetPageSizeWithRotation(pageNum);
    //        Document document = new Document(size);

    //        // open the writer
    //        FileStream fs = new FileStream(newFile, FileMode.Create, FileAccess.Write);
    //        PdfWriter writer = PdfWriter.GetInstance(document, fs);
    //        document.Open();

    //        // the pdf content
    //        PdfContentByte cb = writer.DirectContent;

    //        document.Add(new Annotation(annotHeading, annotText, llx, lly, urx, ury));

    //        // create the new page and add it to the pdf
    //        PdfImportedPage page1 = writer.GetImportedPage(reader, pageNum);//Importing page 1 of the documetn
    //        //PdfImportedPage page2 = writer.GetImportedPage(reader, 1);
    //        cb.AddTemplate(page1, 0, 0);
    //        //cb.AddTemplate(page2, 0, 0);

    //        // close the streams and voilá the file should be changed :)
    //        document.Close();
    //        fs.Close();
    //        writer.Close();
    //        reader.Close();
    //        return newFile;
    //        //return Server.MapPath(newFile);
    //        //return "http://46.4.195.234/pdfweb/newfile.pdf";
    //        //this.Dispose();
    //    }
    //    catch (Exception exp)
    //    {
    //        return exp.Message;
    //    }
    //}

    //2017-08-31 working
    //////public static XmlDocument GetPageXmlDoc(string num, XmlDocument xmlFromRhyw)
    //////{
    //////    try
    //////    {
    //////        //XmlNodeList pageContents = GlobalVar.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/..");
    //////        //Common commObj=new Common();
    //////        //XmlNodeList pageContents = xmlFromRhyw.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::upara|" +
    //////        //                                                            "//*[@page=\"" + num + "\"]/ancestor::spara|" +
    //////        //                                                            "//*[@page=\"" + num + "\"]/ancestor::npara|" +
    //////        //                                                            "//*[@page=\"" + num + "\"]/ancestor::image|" +
    //////        //                                                            "//*[@page=\"" + num + "\"]/ancestor::section-title|" +
    //////        //                                                            "//*[@page=\"" + num + "\"]/ancestor::prefix|" +
    //////        //    //"//*[@page=\"" + num + "\"]/ancestor::table"
    //////        //                                                            "//*[@page=\"" + num + "\"]/ancestor::Table"
    //////        //    //"//Table[@page=" + num + "]"
    //////        //                                                            );

    //////        XmlNodeList pageContents = xmlFromRhyw.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::upara|" +
    //////                                                                   "//*[@page=\"" + num + "\"]/ancestor::spara|" +
    //////                                                                   "//*[@page=\"" + num + "\"]/ancestor::npara|" +
    //////                                                                   "//*[@page=\"" + num + "\"]/ancestor::image|" +
    //////                                                                   "//*[@page=\"" + num + "\"]/ancestor::section-title|" +
    //////                                                                   "//*[@page=\"" + num + "\"]/ancestor::prefix|" +
    //////                                                                   "//*[@page=\"" + num + "\"]/ancestor::box|" +
    //////                                                                   "//*[@page=\"" + num + "\"]/ancestor::table|" +
    //////                                                                   "//*[@page=\"" + num + "\"]/ancestor::footnote"
    //////            //"//Table[@page=" + num + "]"
    //////                                                                   );

    //////        int counter = 0;
    //////        bool isContinuesPara = false;

    //////        XmlDocument tmpPageXml = new XmlDocument();
    //////        if (pageContents.Count == 0)
    //////        {
    //////            return PreProcess(null);
    //////        }
    //////        else
    //////        {
    //////            XmlNode rootElement = tmpPageXml.CreateElement("body");
    //////            tmpPageXml.AppendChild(rootElement);
    //////            XmlNode validNode = null;
    //////            XmlNode paraInSpara = null;
    //////            XmlNode validNode_Parent = null;

    //////            foreach (XmlNode para in pageContents)
    //////            {
    //////                counter = 0;
    //////                isContinuesPara = false;

    //////                //if (para.Name.Equals("footnote"))
    //////                //{
    //////                //    InsertLnTagInFootNotes(para);
    //////                //}

    //////                if ((para.Name.Equals("section-title")) && (para.ParentNode.Name.Equals("head")))
    //////                {
    //////                    validNode_Parent = xmlFromRhyw.CreateElement(para.ParentNode.ParentNode.Name);

    //////                    foreach (XmlAttribute att in para.ParentNode.ParentNode.Attributes)
    //////                    {
    //////                        ((XmlElement)validNode_Parent).SetAttribute(att.Name, att.Value);
    //////                    }
    //////                }
    //////                else
    //////                {
    //////                    validNode_Parent = null;
    //////                }

    //////                validNode = xmlFromRhyw.CreateElement(para.Name);
    //////                foreach (XmlAttribute att in para.Attributes)
    //////                {
    //////                    ((XmlElement)validNode).SetAttribute(att.Name, att.Value);
    //////                }

    //////                XmlNodeList linesList = para.ChildNodes;

    //////                //if (para.Name.ToLower().Equals("table"))
    //////                //{
    //////                //    var tableLines = para.SelectNodes("//Table[@page='" + num + "']/descendant::ln");
    //////                //    if (tableLines != null)
    //////                //    {
    //////                //        foreach (XmlNode ln in tableLines)
    //////                //        {
    //////                //            validNode.InnerXml += ln.OuterXml;
    //////                //        }
    //////                //    }
    //////                //}
    //////                //else
    //////                //{
    //////                foreach (XmlNode ln in linesList)
    //////                {
    //////                    if (isContinuesPara)
    //////                    {
    //////                        if (ln.Attributes["page"] != null && ln.Attributes["page"].Value == num)
    //////                        {
    //////                            //Create a new attribute in first line of a continuous upara whose some lines are in previous page 
    //////                            XmlAttribute attr = xmlFromRhyw.CreateAttribute("iscontinuouspara");
    //////                            attr.Value = "1";
    //////                            ln.Attributes.Append(attr);
    //////                            isContinuesPara = false;
    //////                        }
    //////                    }

    //////                    if (para.Name.Equals("spara"))
    //////                    {
    //////                        paraInSpara = xmlFromRhyw.CreateElement(ln.Name);
    //////                        foreach (XmlNode ln_SPara in ln)
    //////                        {
    //////                            //if (counter < 1)
    //////                            //{
    //////                            //    //Create a new attribute in first line of every spara for changing its color to blue
    //////                            //    XmlAttribute attr = xmlFromRhyw.CreateAttribute("colorChange");
    //////                            //    attr.Value = "1";
    //////                            //    lineInParaNode.Attributes.Append(attr);
    //////                            //    counter++;

    //////                            //    if (lineInParaNode.Attributes["page"] != null && lineInParaNode.Attributes["page"].Value == num)
    //////                            //    {
    //////                            //        validNode.InnerXml += ln.OuterXml;
    //////                            //    }
    //////                            //}

    //////                            if (ln_SPara.Attributes["page"] != null && ln_SPara.Attributes["page"].Value == num)
    //////                            {
    //////                                if (isContinuesPara)
    //////                                {
    //////                                    //Create a new attribute in first line of a continuous upara whose some lines are in previous page 
    //////                                    XmlAttribute attr = xmlFromRhyw.CreateAttribute("iscontinuouspara");
    //////                                    attr.Value = "1";
    //////                                    ln_SPara.Attributes.Append(attr);
    //////                                    isContinuesPara = false;
    //////                                }

    //////                                if (counter < 1)
    //////                                {
    //////                                    //Create a new attribute in first line of every spara for changing its color to blue
    //////                                    XmlAttribute attr = xmlFromRhyw.CreateAttribute("colorChange");
    //////                                    attr.Value = "1";
    //////                                    ln_SPara.Attributes.Append(attr);
    //////                                    counter++;
    //////                                }

    //////                                paraInSpara.InnerXml += ln_SPara.OuterXml;
    //////                            }
    //////                            else if (ln_SPara.Name.Equals("break"))
    //////                            {
    //////                                isContinuesPara = true;
    //////                            }
    //////                        } //end foreach line inside para in spara
    //////                        validNode.InnerXml += paraInSpara.OuterXml;
    //////                    }

    //////                    else if (para.Name.Equals("image"))
    //////                    {
    //////                        if (ln.Name.Equals("caption"))
    //////                        {
    //////                            if (ln.FirstChild.Attributes["page"].Value != null &&
    //////                                ln.FirstChild.Attributes["page"].Value == num)
    //////                            {
    //////                                validNode.InnerXml += ln.OuterXml;
    //////                            }
    //////                        }
    //////                        //If there is no caption tag in image
    //////                        else
    //////                        {
    //////                            if (ln.Attributes["page"] != null && ln.Attributes["page"].Value == num)
    //////                            {
    //////                                validNode.InnerXml += ln.OuterXml;
    //////                            }
    //////                        }
    //////                    }

    //////                    else
    //////                    {
    //////                        if (ln.Attributes["page"] != null && ln.Attributes["page"].Value == num)
    //////                        {
    //////                            validNode.InnerXml += ln.OuterXml;
    //////                        }
    //////                        else if (ln.Name.Equals("break"))
    //////                        {
    //////                            isContinuesPara = true;
    //////                        }
    //////                        else if (para.Name.ToLower().Equals("table") && (para.Attributes["page"] != null && para.Attributes["page"].Value == num))
    //////                        {
    //////                            validNode.InnerXml += ln.OuterXml;
    //////                        }
    //////                        else if (para.Name.Equals("box"))
    //////                        {
    //////                            validNode.InnerXml += ln.OuterXml;
    //////                        }
    //////                    }
    //////                } //end foreach line inside a UPara
    //////                //}

    //////                if (!rootElement.InnerXml.Contains(validNode.OuterXml))
    //////                {
    //////                    if (validNode_Parent != null)
    //////                        rootElement.InnerXml += validNode_Parent.OuterXml + validNode.OuterXml;

    //////                    else
    //////                        rootElement.InnerXml += validNode.OuterXml;
    //////                }

    //////            }//end foreach Main Paras
    //////            return PreProcess(tmpPageXml);
    //////        }
    //////    }
    //////    catch (Exception)
    //////    {
    //////        return null;
    //////    }
    //////}

    public static XmlDocument GetPageXmlDoc(string num, XmlDocument xmlFromRhyw)
    {
        try
        {
            XmlNodeList pageContents = xmlFromRhyw.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::upara|" +
                                                                        "//*[@page=\"" + num + "\"]/ancestor::spara|" +
                                                                        "//*[@page=\"" + num + "\"]/ancestor::npara|" +
                                                                        "//*[@page=\"" + num + "\"]/ancestor::image|" +
                                                                        "//*[@page=\"" + num + "\"]/ancestor::section-title|" +
                                                                        "//*[@page=\"" + num + "\"]/ancestor::prefix|" +
                                                                        "//*[@page=\"" + num + "\"]/ancestor::box|" +
                                                                        "//*[@page=\"" + num + "\"]/ancestor::table|" +
                                                                        "//*[@page=\"" + num + "\"]/ancestor::footnote"
                //"//Table[@page=" + num + "]"
                                                                        );

            int counter = 0;
            bool isContinuesPara = false;

            XmlDocument tmpPageXml = new XmlDocument();
            if (pageContents.Count == 0)
            {
                return PreProcess(null);
            }
            else
            {
                XmlNode rootElement = tmpPageXml.CreateElement("body");
                tmpPageXml.AppendChild(rootElement);
                XmlNode validNode = null;
                XmlNode paraInSpara = null;
                XmlNode validNode_Parent = null;

                foreach (XmlNode para in pageContents)
                {
                    counter = 0;
                    isContinuesPara = false;

                    //if (para.Name.Equals("footnote"))
                    //{
                    //    InsertLnTagInFootNotes(para);
                    //}

                    if ((para.Name.Equals("section-title")) && (para.ParentNode.Name.Equals("head")))
                    {
                        validNode_Parent = xmlFromRhyw.CreateElement(para.ParentNode.ParentNode.Name);

                        foreach (XmlAttribute att in para.ParentNode.ParentNode.Attributes)
                        {
                            ((XmlElement)validNode_Parent).SetAttribute(att.Name, att.Value);
                        }
                    }
                    else
                    {
                        validNode_Parent = null;
                    }

                    validNode = xmlFromRhyw.CreateElement(para.Name);
                    foreach (XmlAttribute att in para.Attributes)
                    {
                        ((XmlElement)validNode).SetAttribute(att.Name, att.Value);
                    }

                    XmlNodeList linesList = para.ChildNodes;

                    if (para.Name.Equals("npara") && linesList.Count > 0)
                    {
                        //Create a new attribute in first line of every npara for changing its color 
                        XmlAttribute attr = xmlFromRhyw.CreateAttribute("lType");
                        attr.Value = "npara";
                        linesList[0].Attributes.Append(attr);
                    }

                    //if (para.Name.ToLower().Equals("table"))
                    //{
                    //    var tableLines = para.SelectNodes("//Table[@page='" + num + "']/descendant::ln");
                    //    if (tableLines != null)
                    //    {
                    //        foreach (XmlNode ln in tableLines)
                    //        {
                    //            validNode.InnerXml += ln.OuterXml;
                    //        }
                    //    }
                    //}
                    //else
                    //{
                    foreach (XmlNode ln in linesList)
                    {
                        if (isContinuesPara)
                        {
                            if (ln.Attributes["page"] != null && ln.Attributes["page"].Value == num)
                            {
                                //Create a new attribute in first line of a continuous upara whose some lines are in previous page 
                                XmlAttribute attr = xmlFromRhyw.CreateAttribute("iscontinuouspara");
                                attr.Value = "1";
                                ln.Attributes.Append(attr);
                                isContinuesPara = false;
                            }
                        }

                        if (para.Name.Equals("spara"))
                        {
                            paraInSpara = xmlFromRhyw.CreateElement(ln.Name);
                            foreach (XmlNode ln_SPara in ln)
                            {
                                //if (counter < 1)
                                //{
                                //    //Create a new attribute in first line of every spara for changing its color to blue
                                //    XmlAttribute attr = xmlFromRhyw.CreateAttribute("colorChange");
                                //    attr.Value = "1";
                                //    lineInParaNode.Attributes.Append(attr);
                                //    counter++;

                                //    if (lineInParaNode.Attributes["page"] != null && lineInParaNode.Attributes["page"].Value == num)
                                //    {
                                //        validNode.InnerXml += ln.OuterXml;
                                //    }
                                //}

                                if (ln_SPara.Attributes["page"] != null && ln_SPara.Attributes["page"].Value == num)
                                {
                                    if (isContinuesPara)
                                    {
                                        //Create a new attribute in first line of a continuous upara whose some lines are in previous page 
                                        XmlAttribute attr = xmlFromRhyw.CreateAttribute("iscontinuouspara");
                                        attr.Value = "1";
                                        ln_SPara.Attributes.Append(attr);
                                        isContinuesPara = false;
                                    }

                                    if (counter < 1)
                                    {
                                        //Create a new attribute in first line of every spara for changing its color 
                                        XmlAttribute attr = xmlFromRhyw.CreateAttribute("lType");
                                        attr.Value = "spara";
                                        ln_SPara.Attributes.Append(attr);
                                        counter++;
                                    }

                                    paraInSpara.InnerXml += ln_SPara.OuterXml;
                                }
                                else if (ln_SPara.Name.Equals("break"))
                                {
                                    isContinuesPara = true;
                                }
                            } //end foreach line inside para in spara
                            validNode.InnerXml += paraInSpara.OuterXml;
                        }

                        else if (para.Name.Equals("image"))
                        {
                            if (ln.Name.Equals("caption"))
                            {
                                if (ln.FirstChild.Attributes["page"].Value != null &&
                                    ln.FirstChild.Attributes["page"].Value == num)
                                {
                                    validNode.InnerXml += ln.OuterXml;
                                }
                            }
                            //If there is no caption tag in image
                            else
                            {
                                if (ln.Attributes["page"] != null && ln.Attributes["page"].Value == num)
                                {
                                    validNode.InnerXml += ln.OuterXml;
                                }
                            }
                        }

                        else
                        {
                            if (ln.Attributes["page"] != null && ln.Attributes["page"].Value == num)
                            {
                                validNode.InnerXml += ln.OuterXml;
                            }
                            else if (ln.Name.Equals("break"))
                            {
                                isContinuesPara = true;
                            }
                            else if (para.Name.ToLower().Equals("table") && (para.Attributes["page"] != null && para.Attributes["page"].Value == num))
                            {
                                validNode.InnerXml += ln.OuterXml;
                            }
                            else if (para.Name.Equals("box"))
                            {
                                validNode.InnerXml += ln.OuterXml;
                            }
                        }
                    } //end foreach line inside a UPara
                    //}

                    if (!rootElement.InnerXml.Contains(validNode.OuterXml))
                    {
                        if (validNode_Parent != null)
                            rootElement.InnerXml += validNode_Parent.OuterXml + validNode.OuterXml;

                        else
                            rootElement.InnerXml += validNode.OuterXml;
                    }

                }//end foreach Main Paras
                return PreProcess(tmpPageXml);
            }
        }
        catch (Exception)
        {
            return null;
        }
    }

    private void InsertLnTagInFootNotes(XmlNode para)
    {



    }

    //public static XmlDocument GetPageXmlDoc(string num, XmlDocument xmlFromRhyw)
    //{
    //    try
    //    {
    //        //XmlNodeList pageContents = GlobalVar.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/..");
    //        //Common commObj=new Common();
    //        XmlNodeList pageContents = xmlFromRhyw.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::upara|" +
    //                                                                     "//*[@page=\"" + num + "\"]/ancestor::spara|" +
    //                                                                     "//*[@page=\"" + num + "\"]/ancestor::npara|" +
    //                                                                     "//*[@page=\"" + num + "\"]/ancestor::image|" +
    //                                                                     "//*[@page=\"" + num + "\"]/ancestor::section-title|" +
    //                                                                     "//*[@page=\"" + num + "\"]/ancestor::prefix|" +
    //            //"//*[@page=\"" + num + "\"]/ancestor::table"

    //                                                                     "//Table[@page=" + num + "]"
    //                                                                     );
    //        int counter = 0;
    //        bool isContinuesPara = false;

    //        if (pageContents.Count == 0)
    //            return PreProcess(null);

    //        XmlDocument tmpPageXml = new XmlDocument();
    //        XmlNode rootElement = tmpPageXml.CreateElement("body");
    //        tmpPageXml.AppendChild(rootElement);
    //        XmlNode validNode = null;
    //        XmlNode paraInSpara = null;
    //        XmlNode validNode_Parent = null;

    //        foreach (XmlNode para in pageContents)
    //        {
    //            counter = 0;
    //            isContinuesPara = false;

    //            if ((para.Name.Equals("section-title")) && (para.ParentNode.Name.Equals("head")))
    //            {
    //                validNode_Parent = xmlFromRhyw.CreateElement(para.ParentNode.ParentNode.Name);

    //                foreach (XmlAttribute att in para.ParentNode.ParentNode.Attributes)
    //                {
    //                    ((XmlElement)validNode_Parent).SetAttribute(att.Name, att.Value);
    //                }
    //            }
    //            else
    //            {
    //                validNode_Parent = null;
    //            }

    //            int count = 0;

    //            //validNode = xmlFromRhyw.CreateElement(para.Name);
    //            //foreach (XmlAttribute att in para.Attributes)
    //            //{
    //            //    count++;

    //            //    ((XmlElement)validNode).SetAttribute(att.Name, att.Value);

    //            //    if (count == 1)
    //            //    {
    //            //        if (para.Name.ToLower().Equals("table"))
    //            //        {
    //            //            validNode.InnerXml += para.OuterXml;
    //            //        }
    //            //    }
    //            //}

    //            XmlNodeList linesList = para.ChildNodes;

    //            foreach (XmlNode ln in linesList)
    //            {

    //                if (isContinuesPara)
    //                {
    //                    if (ln.Attributes["page"] != null && ln.Attributes["page"].Value == num)
    //                    {
    //                        //Create a new attribute in first line of a continuous upara whose some lines are in previous page 
    //                        XmlAttribute attr = xmlFromRhyw.CreateAttribute("iscontinuouspara");
    //                        attr.Value = "1";
    //                        ln.Attributes.Append(attr);
    //                        isContinuesPara = false;
    //                    }
    //                }

    //                if (para.Name.Equals("spara"))
    //                {
    //                    paraInSpara = xmlFromRhyw.CreateElement(ln.Name);
    //                    foreach (XmlNode ln_SPara in ln)
    //                    {
    //                        //if (counter < 1)
    //                        //{
    //                        //    //Create a new attribute in first line of every spara for changing its color to blue
    //                        //    XmlAttribute attr = xmlFromRhyw.CreateAttribute("colorChange");
    //                        //    attr.Value = "1";
    //                        //    lineInParaNode.Attributes.Append(attr);
    //                        //    counter++;

    //                        //    if (lineInParaNode.Attributes["page"] != null && lineInParaNode.Attributes["page"].Value == num)
    //                        //    {
    //                        //        validNode.InnerXml += ln.OuterXml;
    //                        //    }
    //                        //}

    //                        if (ln_SPara.Attributes["page"] != null && ln_SPara.Attributes["page"].Value == num)
    //                        {
    //                            if (isContinuesPara)
    //                            {
    //                                //Create a new attribute in first line of a continuous upara whose some lines are in previous page 
    //                                XmlAttribute attr = xmlFromRhyw.CreateAttribute("iscontinuouspara");
    //                                attr.Value = "1";
    //                                ln_SPara.Attributes.Append(attr);
    //                                isContinuesPara = false;
    //                            }

    //                            if (counter < 1)
    //                            {
    //                                //Create a new attribute in first line of every spara for changing its color to blue
    //                                XmlAttribute attr = xmlFromRhyw.CreateAttribute("colorChange");
    //                                attr.Value = "1";
    //                                ln_SPara.Attributes.Append(attr);
    //                                counter++;
    //                            }

    //                            paraInSpara.InnerXml += ln_SPara.OuterXml;
    //                        }
    //                        else if (ln_SPara.Name.Equals("break"))
    //                        {
    //                            isContinuesPara = true;
    //                        }
    //                    }//end foreach line inside para in spara
    //                    validNode.InnerXml += paraInSpara.OuterXml;
    //                }

    //                else if (para.Name.Equals("image"))
    //                {
    //                    if (ln.Name.Equals("caption"))
    //                    {
    //                        if (ln.FirstChild.Attributes["page"].Value != null && ln.FirstChild.Attributes["page"].Value == num)
    //                        {
    //                            validNode.InnerXml += ln.OuterXml;
    //                        }
    //                    }
    //                    //If there is no caption tag in image
    //                    else
    //                    {
    //                        if (ln.Attributes["page"] != null && ln.Attributes["page"].Value == num)
    //                        {
    //                            validNode.InnerXml += ln.OuterXml;
    //                        }
    //                    }
    //                }
    //                ////if (para.Name.Equals("table"))
    //                ////{
    //                ////    paraInSpara = xmlFromRhyw.CreateElement(ln.Name);
    //                ////    foreach (XmlNode ln_SPara in ln)
    //                ////    {
    //                ////        if (ln_SPara.Attributes["page"] != null && ln_SPara.Attributes["page"].Value == num)
    //                ////        {
    //                ////            if (isContinuesPara)
    //                ////            {
    //                ////                //Create a new attribute in first line of a continuous upara whose some lines are in previous page 
    //                ////                XmlAttribute attr = xmlFromRhyw.CreateAttribute("iscontinuouspara");
    //                ////                attr.Value = "1";
    //                ////                ln_SPara.Attributes.Append(attr);
    //                ////                isContinuesPara = false;
    //                ////            }

    //                ////            if (counter < 1)
    //                ////            {
    //                ////                //Create a new attribute in first line of every spara for changing its color to blue
    //                ////                XmlAttribute attr = xmlFromRhyw.CreateAttribute("colorChange");
    //                ////                attr.Value = "1";
    //                ////                ln_SPara.Attributes.Append(attr);
    //                ////                counter++;
    //                ////            }

    //                ////            paraInSpara.InnerXml += ln_SPara.OuterXml;
    //                ////        }
    //                ////        else if (ln_SPara.Name.Equals("break"))
    //                ////        {
    //                ////            isContinuesPara = true;
    //                ////        }
    //                ////    }//end foreach line inside para in spara
    //                ////    validNode.InnerXml += paraInSpara.OuterXml;
    //                ////}

    //                else
    //                {
    //                    if (ln.Attributes["page"] != null && ln.Attributes["page"].Value == num)
    //                    {
    //                        validNode.InnerXml += ln.OuterXml;
    //                    }
    //                    else if (ln.Name.Equals("break"))
    //                    {
    //                        isContinuesPara = true;
    //                    }
    //                }
    //            }//end foreach line inside a Upara/Spara

    //            if (validNode_Parent != null)
    //                rootElement.InnerXml += validNode_Parent.OuterXml + validNode.OuterXml;

    //            else
    //                rootElement.InnerXml += validNode.OuterXml;

    //        }//end main foreach paras

    //        return PreProcess(tmpPageXml);
    //    }
    //    catch (Exception)
    //    {
    //        return null;
    //    }
    //}

    static XmlDocument PreProcess(XmlDocument xmlDoc)
    {
        XmlDocument newXmlDoc = new XmlDocument();
        XmlNode rootNode = newXmlDoc.CreateElement("pbp-book");
        newXmlDoc.AppendChild(rootNode);

        string necessaryElements = "<pbp-meta><pbp-info tag-operator=\"pakistan\" tag-date=\"2009-01-01\" file-name=\"XXXXXX\" schema-name=\"PBPBook_P02.xsd\" publication-status=\"NOT FOR PUBLICATION\" book-type=\"OTHER\" copyright-status=\"IN COPYRIGHT\" book-title=\"XXXXX\" schema-rev=\"p02\"/><doc-track></doc-track><bookrep-info><author-id>1</author-id><book-summary></book-summary><author-info></author-info></bookrep-info></pbp-meta><pbp-front><cover><image-model><front image-url=\"\"/><spine image-url=\"\"/><back image-url=\"\"/></image-model></cover><BISAC><BISAC-item><BISAC-text></BISAC-text><BISAC-code></BISAC-code></BISAC-item></BISAC><ISBN></ISBN><title-block><book-title><main-title>XX</main-title><running-header></running-header></book-title><author><full-name>FullName</full-name><prenominal /><first-name>FirstName</first-name><last-name>XX</last-name></author></title-block><book-notices></book-notices></pbp-front>";
        rootNode.InnerXml = necessaryElements;
        string innerXML = "";
        if (xmlDoc == null)
        {
            innerXML = "<body><upara><ln coord=\"237.65:568.72:388.55:586.72\" page=\"1\" height=\"792\" left=\"237.65\" top=\"568.72\" font=\"BemboStd\" fontsize=\"18\" error=\"0\" ispreviewpassed=\"true\" isUserSigned=\"0\" isEditted=\"false\">Sorry! The page is blank</ln></upara></body>";
        }
        else
        {
            innerXML = xmlDoc.InnerXml;
        }
        rootNode.InnerXml += "<pbp-body>" + innerXML + "</pbp-body>";
        return newXmlDoc;
    }


    /////////////////////////////////////////////////////////////////////////////////////////

    //public static int GetTotalMistakes(int page)
    //{
    //    if (SiteSession.MainXMLFilePath_PDF == "")
    //        return 0;

    //    StreamReader strreader = new StreamReader(SiteSession.MainXMLFilePath_PDF);
    //    string xmlInnerText = strreader.ReadToEnd();
    //    strreader.Close();

    //    XmlDocument xmlDoc = new XmlDocument();
    //    xmlDoc.LoadXml(xmlInnerText);

    //    int totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @page=" + page + "]").Count;
    //    return totalMistakes;
    //}

    private int GetTotalMistakes()
    {
        string filePath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);

        if ((filePath == "") || (!File.Exists(filePath)))
            return 0;

        StreamReader strreader = new StreamReader(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        int totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @PDFmistake!='undo']").Count;

        return totalMistakes;
    }

    private string GetTotalMistakesAll()
    {
        string filePath = Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]);

        if ((filePath == "") || (!File.Exists(filePath)))
            return "0";

        StreamReader strreader = new StreamReader(Convert.ToString(HttpContext.Current.Session["MainXMLFilePath"]));
        string xmlInnerText = strreader.ReadToEnd();
        strreader.Close();

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlInnerText);

        int totalMistakes = xmlDoc.SelectNodes(@"//*[@PDFmistake!='' and @PDFmistake!='undo']").Count;
        int totalDetectedInjMistakes = xmlDoc.SelectNodes(@"//*[@correction='']").Count + xmlDoc.SelectNodes(@"//*[@conversion='']").Count;

        return totalMistakes + "," + totalDetectedInjMistakes;
    }

    public static byte[] ReadFile(string filePath)
    {
        byte[] buffer;
        FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
        try
        {
            int length = (int)fileStream.Length; // get file length
            buffer = new byte[length]; // create buffer
            int count; // actual number of bytes read
            int sum = 0; // total number of bytes read

            // read until Read method returns 0 (end of the stream has been reached)
            while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                sum += count; // sum is a buffer offset for next reading
        }
        finally
        {
            fileStream.Close();
        }
        return buffer;
    }

    public void Error_Log(string errorMsg)
    {
        string LogFilePath = ConfigurationManager.AppSettings["LogFilePath"];
        StreamWriter file = new StreamWriter(LogFilePath + "\\Log.txt", true);
        file.WriteLine(errorMsg);
        file.Close();
    }

    public static int TotalPages(string pdfPath)
    {
        try
        {
            PdfReader inputPdf = new PdfReader(pdfPath);
            return inputPdf.NumberOfPages;
        }
        catch
        {
            return -1;
        }
    }

    /// <summary>
    /// Produces single page from the current rhywFile
    /// </summary>
    /// <param name="PageNum"></param>
    /// <returns>Produced File Path</returns>
    //public static string ProduceSinglePage(int PageNum)
    //{
    //    string fileName = GetDirectoryPath();
    //    //SiteSession.xmlDoc = 
    //    XmlDocument pageXML = GlobalVar.GetPageXmlDoc(PageNum.ToString());
    //    string dirPath = ConfigurationManager.AppSettings["PDFDirPhyPath"];
    //    //Random rnd = new Random();

    //    string pageXMLSavedPath = "";

    //    if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
    //    {
    //        pageXMLSavedPath = dirPath + "\\Tests\\" +
    //                           Convert.ToString(HttpContext.Current.Session["CompTestUser_Email"]) +
    //                           "/ComparisonTests/" + "\\Produced_" + PageNum + ".xml";
    //    }
    //    else
    //    {
    //        pageXMLSavedPath = dirPath + fileName.Replace("-1", "") + "\\" + fileName +
    //                           "\\Comparison\\Comparison-" +
    //                           Convert.ToString(HttpContext.Current.Session["comparisonType"]) + "\\" +
    //                           Convert.ToInt32(HttpContext.Current.Session["userId"]) + "\\Produced_" + PageNum + ".xml";


    //        LogWritter.WriteLineInLog("In Produced Single Page :: pageXMLSavedPath -  ::" + pageXMLSavedPath);
    //    }
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

    #region |Abnormal Font Check Region|

    public static List<XmlNode> AbnormalFontCheck(XmlDocument pageXML, XmlDocument mainXML)
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
            //Convert xmlNodeList to list<Double>
            var fontValues = Lines.Cast<XmlNode>()
                .Select(node => Convert.ToDouble(node.Attributes["fontsize"].Value))
                .ToList();

            //Get fontsize values with total occurences 
            var q = fontValues.GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count).ToList();

            normalTextSize = Convert.ToDouble(q[0].Value);
        }

        return Convert.ToString(normalTextSize);
    }

    public void NormalAndIndentX(XmlNode mainDoc, ref double NormalX, ref double NormalIndentX)
    {
        XmlNodeList lineList = mainDoc.SelectNodes("//ln/@left");

        if (lineList.Count > 0)
        {
            //Convert xmlNodeList to list<string>
            var leftValues = lineList.Cast<XmlNode>()
                .Select(node => node.InnerText)
                .ToList();

            //Find normal Indent x which is the maximum value
            var list_IndentX = leftValues.GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Value).ToList();

            NormalIndentX = Convert.ToDouble(list_IndentX[0].Value);

            //Find normal x which is the value with maximum occurences
            var list_NormalX = leftValues.GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderByDescending(x => x.Count).ToList();

            NormalX = Convert.ToDouble(list_NormalX[0].Value);
        }

        NormalX = Math.Floor(NormalX);
        NormalIndentX = Math.Floor(NormalIndentX);
    }

    #endregion

    public static bool IsFileLocked(FileInfo file)
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

    public static void finishTask(String bId, string userId)
    {
        try
        {
            MyDBClass objMyDBClass = new MyDBClass();
            string querySel = "Select BID from BOOK Where BIdentityNo='" + bId + "'";
            DataSet dsBookInfo = objMyDBClass.GetDataSet(querySel);
            string bookID = dsBookInfo.Tables[0].Rows[0]["BID"].ToString();

            string query_InProcess = "Update ACTIVITY Set Status='In Process', CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] +
                "' Where UID=" + userId + " AND BID=" + bookID + " AND Task='ErrorDetection'";
            int upRes = objMyDBClass.ExecuteCommand(query_InProcess);
            if (upRes > 0)
            {
                string queryUpdate = "Update ACTIVITY Set Status='Pending Confirmation' Where BID=" + bookID + " AND Task='ErrorDetection' AND Status='In Process'";
                int rowEffected = objMyDBClass.ExecuteCommand(queryUpdate);

                //CreateQaInspectionTask(bId + "-1", userId);

                if (rowEffected > 0)
                {

                }
            }
        }
        catch (Exception ex)
        {

        }
    }

    public static void ExtractPages(string inputFile, string outputFile, int page)
    {
        PdfReader reader = null;
        Document document = null;
        PdfCopy pdfCopyProvider = null;
        PdfImportedPage importedPage = null;

        try
        {
            using (Stream outPutStream = new FileStream(outputFile, FileMode.Create))
            {
                // Intialize a new PdfReader instance with the contents of the source Pdf file:
                reader = new PdfReader(inputFile);

                // Capture the correct size and orientation for the page:
                document = new Document(reader.GetPageSizeWithRotation(1));

                // Initialize an instance of the PdfCopyClass with the source 
                // document and an output file stream:
                //pdfCopyProvider = new PdfCopy(document, new FileStream(outputFile, FileMode.Create));

                pdfCopyProvider = new PdfCopy(document, outPutStream);
                document.Open();

                // Extract the desired page number:
                importedPage = pdfCopyProvider.GetImportedPage(reader, page);
                pdfCopyProvider.AddPage(importedPage);

                document.Close();
                pdfCopyProvider.Close();
                importedPage.ClosePath();
                reader.Close();
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }
}
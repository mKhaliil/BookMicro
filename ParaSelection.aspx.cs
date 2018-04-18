using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using iTextSharp.text.pdf;
using Outsourcing_System.CommonClasses;
using System.Web.Services;
using Outsourcing_System.PdfCompare_Classes;

namespace Outsourcing_System
{
    public partial class ParaSelection : System.Web.UI.Page
    {
        TableDetection tblDetectionObj = new TableDetection();
        GlobalVar objGlobal = new GlobalVar();
        MyDBClass objMyDBClass = new MyDBClass();
        Common commonObj = new Common();

        private bool CreatePdfPreviewDirectory(string bookId, string savingPath)
        {
            string pdfPreviewDirPath = savingPath + "\\" + bookId + "\\ParaSelection\\PdfPreview";
            bool status = false;

            try
            {
                if (!Directory.Exists(pdfPreviewDirPath))
                    Directory.CreateDirectory(pdfPreviewDirPath);

                return true;
            }
            catch (Exception)
            {
                return status;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //hfIsIgnoreAlgo.Value = "false";
                string bookId = Convert.ToString(Session["MainBook"]);
                hfParaSelBookId.Value = bookId;
                string savingPath = Common.GetDirectoryPath();

                CreatePdfPreviewDirectory(bookId, savingPath);

                //string mainDirectoryPath = Common.GetDirectoryPath();
                //string pdfDirectoryPath = mainDirectoryPath + "\\" + bookId;
                //string highlightedPdfPath = pdfDirectoryPath + "\\" + bookId + "_Highlighted.pdf";
                //string xslsFilesPath = pdfDirectoryPath + "\\DetectedTables\\TableTasks\\";

                //Session["tempXmlPath"] = pdfDirectoryPath + "\\DetectedTables\\Temp_Table.xml";
                //Session["tempXmlPath_actual"] = pdfDirectoryPath + "\\DetectedTables\\Temp_Table_actual.xml";

                //int avgWordSpace = tblDetectionObj.AvgWordSpace(pdfDirectoryPath + "\\DetectedTables\\Temp_Table.xml");
                //int AverageVeticalSpace = tblDetectionObj.AvgLineSpace(pdfDirectoryPath + "\\DetectedTables\\Temp_Table_actual.xml");
                //Session["AverageVeticalSpace"] = AverageVeticalSpace;
                //Session["AverageSpace"] = avgWordSpace;

                //int totalPages = GetTotalPageCount(highlightedPdfPath);
                //if (totalPages > 0)
                //{
                //    Session["TotalPages"] = totalPages;
                //    ////lblTotalPages.Text = Convert.ToString(totalPages);
                //}

                //Session["CurrentPage"] = 1;

                //ShowPdf(Convert.ToString(Session["CurrentPage"]));

                string sourcePdfPath = savingPath + "\\" + bookId + "\\" + bookId + ".pdf";
                string sourceTetmlPath = savingPath + "\\" + bookId + "\\" + bookId + ".tetml";

                if (File.Exists(sourcePdfPath) && File.Exists(sourceTetmlPath))
                {
                    string targetPdfPath = savingPath + "\\" + bookId + "\\ParaSelection\\PdfPreview\\" + bookId + ".pdf";
                    if (!File.Exists(targetPdfPath))
                        File.Copy(sourcePdfPath, targetPdfPath, true);

                    string targetTetmlPath = savingPath + "\\" + bookId + "\\ParaSelection\\PdfPreview\\" + bookId + ".tetml";
                    if (!File.Exists(targetTetmlPath))
                        File.Copy(sourceTetmlPath, targetTetmlPath, true);

                    PdfReader pdfReader = new PdfReader(targetPdfPath);
                    int numberOfPages = pdfReader.NumberOfPages;

                    Session["ParaSelPdfPageCount"] = numberOfPages;
                    ////lblTotalPages.Text = Convert.ToString(totalPages);

                    lblTotalPages.Text = Convert.ToString(numberOfPages);

                    if (string.IsNullOrEmpty(Convert.ToString(Session["CurrentParaSelPdfPage"])))
                    {
                        Session["CurrentParaSelPdfPage"] = 1;
                        //lblPageNum.Text = "1";
                        tbxPageNum.Text = "1";
                        ShowPdf(1);
                    }
                    else
                    {
                        ShowPdf(Convert.ToInt32(Session["CurrentParaSelPdfPage"]));
                    }
                }
            }
        }

        public void ShowPdf(int page)
        {
            tbxPageNum.Text = Convert.ToString(page);

            string bookId = Convert.ToString(Session["MainBook"]);
            string savingPath = Common.GetDirectoryPath();

            string pdfPreviewDirPath = savingPath + "\\" + bookId + "\\ParaSelection\\PdfPreview";
            string sourcePdfPath = pdfPreviewDirPath + "\\" + bookId + ".pdf";
            string outputPdfPath = pdfPreviewDirPath + "\\" + page + ".pdf";
            string outputHighlightedPdfPath = pdfPreviewDirPath + "\\" + page + "_Highlighted.pdf";

            //if (!File.Exists(outputHighlightedPdfPath))
            //{
            tblDetectionObj.ExtractPage(sourcePdfPath, outputPdfPath, page);
            Session["paraSelectionPdfPath"] = outputPdfPath;
            //}
            //else
            //{
            //    tblDetectionObj.ExtractPage(outputHighlightedPdfPath, outputManualHighlightedPdfPath, page);
            //    Session["paraSelectionPdfPath"] = outputManualHighlightedPdfPath;
            //}
        }

        protected void btnNextPage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["CurrentParaSelPdfPage"])) &&
                !string.IsNullOrEmpty(Convert.ToString(Session["ParaSelPdfPageCount"])))
            {
                int currentPage = Convert.ToInt32(Session["CurrentParaSelPdfPage"]);
                int totalPages = Convert.ToInt32(Session["ParaSelPdfPageCount"]);

                if (currentPage > totalPages) return;

                currentPage = currentPage < totalPages ? ++currentPage : currentPage;

                if (currentPage > 1)
                {
                    Session["CurrentParaSelPdfPage"] = currentPage;

                    //lblPageNum.Text = Convert.ToString(currentPage);

                    ShowPdf(currentPage);
                }
            }
        }

        protected void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["CurrentParaSelPdfPage"])))
            {
                int currentPage = Convert.ToInt32(Session["CurrentParaSelPdfPage"]);
                currentPage = currentPage > 1 ? --currentPage : currentPage;

                //int totalPages = Convert.ToInt32(Session["ParaSelPdfPageCount"]);

                if (currentPage == 0) return;

                Session["CurrentParaSelPdfPage"] = currentPage;

                ShowPdf(currentPage);
            }
        }

        protected void lbtnHome_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
            {
                Response.Redirect("AdminPanel.aspx");
            }
            else
            {
                Response.Redirect("BookMicro.aspx", true);
            }
        }

        protected void lbtnLogOut_Click(object sender, System.EventArgs e)
        {
            Session.Clear();
            Response.Redirect("BookMicro.aspx", true);
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

        protected void btnMarkUPara_Click(object sender, EventArgs e)
        {
            bool status = false;

            try
            {
                if (!string.IsNullOrEmpty(hfMarkedUParaText.Value) && !string.IsNullOrEmpty(Convert.ToString(Session["CurrentParaSelPdfPage"])))
                {
                    List<string> pdfJsLinesList = tblDetectionObj.GetPdfJsSelectedLines(hfMarkedUParaText.Value, Convert.ToInt32(Session["CurrentParaSelPdfPage"]));
                    List<string> pdfJsSParaLinesWithoutHyph = RemoveHyphenWords(pdfJsLinesList);
                    List<TetmlLine> currentPageTetmlLineList = GetTetmlLines(Convert.ToInt32(Session["CurrentParaSelPdfPage"]));

                    if (pdfJsSParaLinesWithoutHyph.Count > 0 && currentPageTetmlLineList.Count > 0)
                    {
                        List<TetmlLine> uParaIndenLines = GetParaIndentations(pdfJsSParaLinesWithoutHyph, currentPageTetmlLineList);

                        if (uParaIndenLines.Count > 0)
                        {
                            if (Convert.ToInt32(Session["CurrentParaSelPdfPage"]) % 2 == 0)
                            {
                                if (SaveIndentationInDb(uParaIndenLines, "upara", "even", Convert.ToInt32(Session["CurrentParaSelPdfPage"])))
                                {
                                    status = true;
                                }
                            }
                            else if (Convert.ToInt32(Session["CurrentParaSelPdfPage"]) % 2 != 0)
                            {
                                if (SaveIndentationInDb(uParaIndenLines, "upara", "odd", Convert.ToInt32(Session["CurrentParaSelPdfPage"])))
                                {
                                    status = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                status = false;
            }

            if (status)
            {
                ShowMessage(MessageTypes.Success, "UPara is saved successfully.");
            }
            else
            {
                ShowMessage(MessageTypes.Error, "Some error has occured.");
            }
        }

        protected void btnMarkSPara_Click(object sender, EventArgs e)
        {
            bool status = false;
            try
            {
                if (!string.IsNullOrEmpty(hfMarkedSParaText.Value) && !string.IsNullOrEmpty(Convert.ToString(Session["CurrentParaSelPdfPage"])))
                {
                    List<string> pdfJsSParaLinesList = tblDetectionObj.GetPdfJsSelectedLines(hfMarkedSParaText.Value, Convert.ToInt32(Session["CurrentParaSelPdfPage"]));
                    List<string> pdfJsSParaLinesWithoutHyph = RemoveHyphenWords(pdfJsSParaLinesList);
                    List<TetmlLine> currentPageTetmlLineList = GetTetmlLines(Convert.ToInt32(Session["CurrentParaSelPdfPage"]));

                    if (pdfJsSParaLinesWithoutHyph.Count > 0 && currentPageTetmlLineList.Count > 0)
                    {
                        List<TetmlLine> sParaIndenLines = GetParaIndentations(pdfJsSParaLinesWithoutHyph, currentPageTetmlLineList);

                        if (sParaIndenLines.Count > 0)
                        {
                            if (Convert.ToInt32(Session["CurrentParaSelPdfPage"]) % 2 == 0)
                            {
                                if (SaveIndentationInDb(sParaIndenLines, "spara", "even", Convert.ToInt32(Session["CurrentParaSelPdfPage"])))
                                {
                                    status = true;
                                }
                            }
                            else if (Convert.ToInt32(Session["CurrentParaSelPdfPage"]) % 2 != 0)
                            {
                                if (SaveIndentationInDb(sParaIndenLines, "spara", "odd", Convert.ToInt32(Session["CurrentParaSelPdfPage"])))
                                {
                                    status = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                status = false;
            }

            if (status)
            {
                ShowMessage(MessageTypes.Success, "SPara is saved successfully.");
            }
            else
            {
                ShowMessage(MessageTypes.Error, "Some error has occured.");
            }
        }

        public void ShowMessage(MessageTypes messageType, string msgText)
        {
            switch (messageType)
            {
                case MessageTypes.Success:
                    ucShowMessage1.ShowMessage(MessageTypes.Success, msgText);
                    break;
                case MessageTypes.Error:
                    ucShowMessage1.ShowMessage(MessageTypes.Error, msgText);
                    break;
                case MessageTypes.Info:
                    ucShowMessage1.ShowMessage(MessageTypes.Info, msgText);
                    break;
                default:
                    break;
            }
        }

        public bool SaveIndentationInDb(List<TetmlLine> sParaIndenLines, string paraType, string pageType, int pageNum)
        {
            bool status = true;

            try
            {
                if (sParaIndenLines.Count > 0)
                {
                    string bookId = Convert.ToString(Session["MainBook"]);
                    double xIndentVal = Convert.ToDouble(sParaIndenLines[0].Llx);
                    double xVal = Convert.ToDouble(sParaIndenLines[1].Llx);

                    //if (xVal > xIndentVal)
                    //{
                    //    double temp = xVal;
                    //    xIndentVal = xVal;
                    //    xVal = temp;
                    //}

                    double endXVal = Convert.ToDouble(sParaIndenLines[1].Urx);
                    double fontSize = Convert.ToDouble(sParaIndenLines[0].FontSize);
                    string fontName = sParaIndenLines[0].Font;

                    if (!string.IsNullOrEmpty(bookId) && !string.IsNullOrEmpty(paraType) && xVal > 0 && xIndentVal > 0)
                        objMyDBClass.SaveIndentationInDb(bookId, pageType, xVal, xIndentVal, endXVal, fontSize, fontName, paraType, pageNum);
                }
            }
            catch (Exception)
            {
                status = false;
            }
            return status;
        }

        public List<string> RemoveHyphenWords(List<string> pdfJsSParaLinesList)
        {
            if (pdfJsSParaLinesList.Count > 0)
            {
                for (int i = 0; i < pdfJsSParaLinesList.Count; i++)
                {
                    string line = pdfJsSParaLinesList[i].Trim();

                    if (!string.IsNullOrEmpty(line))
                    {
                        if (line[line.Length - 1].Equals('-'))
                        {
                            List<string> hyphenLineWordsList = Regex.Split(line, @"\s+").ToList();
                            if (hyphenLineWordsList.Count > 0)
                            {
                                hyphenLineWordsList.RemoveAt(hyphenLineWordsList.Count - 1);
                                pdfJsSParaLinesList[i] = (string.Join(" ", hyphenLineWordsList.ToArray()));
                            }

                            if (i + 1 < pdfJsSParaLinesList.Count)
                            {
                                string nextLine = pdfJsSParaLinesList[i + 1].Trim();
                                List<string> wordsList = Regex.Split(nextLine, @"\s+").ToList();
                                if (wordsList.Count > 0)
                                {
                                    wordsList.RemoveAt(0);
                                    pdfJsSParaLinesList[i + 1] = (string.Join(" ", wordsList.ToArray()));
                                }
                            }
                        }
                    }
                }
            }

            return pdfJsSParaLinesList;
        }

        public string RemoveSpecialChars(string word)
        {
            return word.Replace(",", "").Replace(",", "").Replace("’", "").Replace("‘", "").Replace(",", "").Replace("***", "").Trim().ToLower();
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

        private bool IsLineMatched(string paraLine, string tetmlLine)
        {
            if (string.IsNullOrEmpty(paraLine) || string.IsNullOrEmpty(tetmlLine)) return false;

            List<string> pdfJsLineTempList = Regex.Split(paraLine, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
            List<string> xmlLineTempList = Regex.Split(tetmlLine, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

            if (pdfJsLineTempList.Count > 0 && xmlLineTempList.Count > 0)
            {
                int matchingPer = GetMatchingPercentage(pdfJsLineTempList[0].Trim(), xmlLineTempList[0].Trim());
                if (matchingPer < 50)
                    return false;
            }

            if (paraLine.Trim().Equals(tetmlLine.Trim()))
                return true;

            string pdfJsText = RemoveWhiteSpace(RemoveSpecialChars(paraLine)).Replace(".", "").Replace("…", "");
            string xmlText = RemoveWhiteSpace(RemoveSpecialChars(Convert.ToString(tetmlLine))).Replace(".", "").Replace("…", "");

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

        public List<TetmlLine> GetParaIndentations(List<string> pdfJsSParaLinesList, List<TetmlLine> currentPageTetmlLineList)
        {
            List<TetmlLine> indentationLines = null;

            if (pdfJsSParaLinesList.Count >= 2)
            {
                indentationLines = new List<TetmlLine>();

                for (int i = 0; i < currentPageTetmlLineList.Count; i++)
                {
                    if (IsLineMatched(pdfJsSParaLinesList[0], currentPageTetmlLineList[i].Text))
                    {
                        if (i + 1 < currentPageTetmlLineList.Count && IsLineMatched(pdfJsSParaLinesList[1], currentPageTetmlLineList[i + 1].Text))
                        {
                            indentationLines.Add(currentPageTetmlLineList[i]);
                            indentationLines.Add(currentPageTetmlLineList[i + 1]);
                        }
                    }
                }
            }
            return indentationLines;
        }

        //public List<TetmlLine> GetTetmlLines(int page)
        //{
        //    string bookId = Convert.ToString(Session["MainBook"]);
        //    string savingPath = Common.GetDirectoryPath();

        //    string sourcePdfPath = savingPath + "\\" + bookId + "\\ParaSelection\\PdfPreview\\" + page + ".pdf";
        //    string tetmlFilePath = savingPath + "\\" + bookId + "\\ParaSelection\\PdfPreview\\" + page + ".tetml";

        //    if (!File.Exists(tetmlFilePath) && File.Exists(sourcePdfPath))
        //    {
        //        if (Createtetml(sourcePdfPath, tetmlFilePath))
        //        {
        //            var tetmlSrcDoc = LoadTetmlXmlDocument(tetmlFilePath);
        //            return commonObj.GetAllLinesFromTetmlWithHyphenVal(tetmlSrcDoc);
        //        }
        //    }
        //    else if (File.Exists(tetmlFilePath) && File.Exists(sourcePdfPath))
        //    {
        //        var tetmlSrcDoc = LoadTetmlXmlDocument(tetmlFilePath);
        //        return commonObj.GetAllLinesFromTetmlWithHyphenVal(tetmlSrcDoc);
        //    }

        //    return null;
        //}

        public List<TetmlLine> GetTetmlLines(int page)
        {
            string bookId = Convert.ToString(Session["MainBook"]);
            string savingPath = Common.GetDirectoryPath();

            string sourcePdfPath = savingPath + "\\" + bookId + "\\ParaSelection\\PdfPreview\\" + page + ".pdf";
            string tetmlFilePath = savingPath + "\\" + bookId + "\\ParaSelection\\PdfPreview\\" + bookId + ".tetml";

            if (File.Exists(tetmlFilePath) && File.Exists(sourcePdfPath))
            {
                var tetmlSrcDoc = LoadTetmlXmlDocument(tetmlFilePath);
                return commonObj.GetAllLinesFromTetmlWithHyphenVal(tetmlSrcDoc, page);
            }

            return null;
        }

        public bool Createtetml(string sourcePdfPath, string tetmlOutputPath)
        {
            bool status = true;

            try
            {
                string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks} clippingarea={cropbox}\" -o \"" + tetmlOutputPath + "\" \"" + sourcePdfPath + "\"";

                string tetmlExePath = Convert.ToString(System.Configuration.ConfigurationSettings.AppSettings["TetPath"]);
                Process pConvertTetml = new Process();
                pConvertTetml.StartInfo.UseShellExecute = false;
                pConvertTetml.StartInfo.RedirectStandardError = true;
                pConvertTetml.StartInfo.RedirectStandardOutput = true;
                pConvertTetml.StartInfo.CreateNoWindow = true;
                pConvertTetml.StartInfo.Arguments = strParameter;
                pConvertTetml.StartInfo.FileName = tetmlExePath;
                pConvertTetml.Start();
                pConvertTetml.WaitForExit();
            }
            catch (Exception)
            {
                status = false;
            }

            return status;
        }

        protected void btnGoTo_Click(object sender, EventArgs e)
        {
            int pNum = -1;
            int pageNum = 0;
            if (int.TryParse(tbxPageNum.Text.Trim(), out pNum))
            {
                pageNum = pNum;

                if (!string.IsNullOrEmpty(Convert.ToString(Session["ParaSelPdfPageCount"])))
                {
                    int totalPages = Convert.ToInt32(Session["ParaSelPdfPageCount"]);

                    if (pageNum == 0 || pageNum > totalPages) return;

                    Session["CurrentParaSelPdfPage"] = pageNum;
                    ShowPdf(pageNum);
                }
            }
            else
            {
                //Invalid
            }
        }

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["MainBook"])) &&
                !string.IsNullOrEmpty(Convert.ToString(Session["AID"])))
            {
                int status = objMyDBClass.UpdateParaIndentationStatus(Convert.ToString(Session["MainBook"]), true);

                //if (status > 0)
                //{
                   Response.Redirect("TagUntag.aspx?aid=" + Convert.ToString(Session["AID"]) + "&bid=" + Convert.ToString(Session["MainBook"]) + "-1");
                //}
                //else
                //{
                //    ShowMessage(MessageTypes.Error, "Some error has occured while finishing task.");
                //}
            }
        }

        protected void btnSaveTable_Click(object sender, EventArgs e)
        {
        }

        protected void btnSaveImage_Click(object sender, EventArgs e)
        {
        }

        protected void btnFinishTask_Click(object sender, EventArgs e)
        {
        }

        [WebMethod]
        public static bool MarkTable(string text)
        {
            bool status = false;

            try
            {
                if (string.IsNullOrEmpty(text)) return status;

                var temp = text.Split(new string[] { "/~/" }, StringSplitOptions.None).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

                if ((temp != null) && (temp.Count > 2))
                {
                    //var tableLines = temp[0].Split('\n').Select(x => x.Trim()).Distinct().ToList();
                    var tableLines = temp[0];
                    bool isIgnorAlgoChecked = Convert.ToString(temp[1]) == "" ? false : Convert.ToBoolean(temp[1]);

                    //HttpContext.Current.Session["isIgnorAlgoChecked"] = Convert.ToString(temp[1]) == "" ? false : Convert.ToBoolean(temp[1]);

                    TableDetection tblDetecObj = new TableDetection();
                    ////var tableXml = tblDetectionObj.SaveTablesInXml(tableLines, isIgnorAlgoChecked, true, "manual");

                    int pageNumber = tblDetecObj.GetCurrentPageNum();
                    List<string> pdfJsLinesList = tblDetecObj.GetPdfJsSelectedLines(tableLines, pageNumber);

                    //var tableXml = tblDetecObj.MarkTableInXml(pdfJsLinesList, pageNumber, isIgnorAlgoChecked, false);

                    var tableXml = tblDetecObj.SaveTablesInTempXml(pdfJsLinesList, pageNumber, isIgnorAlgoChecked, false);

                    if (tableXml != null)
                    {
                        tblDetecObj.CreateXls(tableXml);
                        //tblDetectionObj.SaveAsXml(tables);

                        string mainDirectoryPath = Common.GetDirectoryPath();
                        string bookId = Convert.ToString(HttpContext.Current.Session["MainBook"]);
                        string sourcePdfPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\" + bookId + "_actual.tetml";
                        string outputHighlightedPdfPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\" + bookId + "_actual_Manual_Highlighted.pdf";

                        //aamir
                        ////List<string> abbrCoord = tblDetectionObj.GetCoordinates(sourcePdfPath, tableXml);
                        ////if (abbrCoord != null)
                        ////{
                        ////    if (abbrCoord.Count > 0)
                        ////    {
                        ////        if (tblDetectionObj.HighLightTables(sourcePdfPath.Replace(".tetml", ".pdf"),
                        ////            outputHighlightedPdfPath, BaseColor.GREEN, abbrCoord))
                        ////        {
                        ////            List<string> tableContainingPages = (List<string>)HttpContext.Current.Session["TablePages"];
                        ////            int currentPage = Convert.ToInt32(HttpContext.Current.Session["CurrentPage"]);
                        ////            string pageNum = tableContainingPages.ElementAt(currentPage - 1);

                        ////            string pdfPreviewDirPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\PdfPreview";
                        ////            string outputManualHighlightedPdfPath = pdfPreviewDirPath + "\\" + pageNum + "_Manual_Highlighted.pdf";

                        ////            tblDetectionObj.ExtractPage(outputHighlightedPdfPath, outputManualHighlightedPdfPath, pageNum);
                        ////            //HttpContext.Current.Session["isIgnorAlgoChecked"] = "false";
                        ////        }
                        ////    }
                        ////}

                        status = true;

                        //HttpContext.Current.Session["isNextClicked"] = "False";
                    }
                }

                return status;
            }
            catch (Exception)
            {
                return status;
            }
        }

        [WebMethod]
        public static string GetParaSelMissingPage(string bookId)
        {
            MyDBClass dbObj = new MyDBClass();
            List<PdfPara> paraObj = dbObj.GetParaSelectedPageNum(bookId);

            string result = "";

            if (paraObj != null && paraObj.Count > 0)
            {
                List<PdfPara> uParaList = paraObj.Where(x => x.ParaType.Equals("upara")).ToList();
                if (uParaList.Count == 1)
                {
                    string missinguPara = uParaList[0].ParaType == "even" ? "odd" : "even";
                    result = "You have not selected upara on " + missinguPara + " page ";
                    return result;
                }
                else if (uParaList.Count == 0)
                {
                    result = "You have not selected any upara on any even and odd page";
                    return result;
                }

                List<PdfPara> sParaList = paraObj.Where(x => x.ParaType.Equals("spara")).ToList();
                if (sParaList.Count == 1)
                {
                    string missingsPara = sParaList[0].ParaType == "even" ? "odd" : "even";
                    result = "You have not selected spara on " + missingsPara + " page ";
                    return result;
                }
                else if (sParaList.Count == 0)
                {
                    result = "You have not selected any spara on any even and odd page";
                    return result;
                }
            }
            else
            {
                result = "You have not selected any upara on any even and odd page";
            }
            return result;
        }
    }
}
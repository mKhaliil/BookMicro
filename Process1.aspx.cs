using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Services;
using System.Web.UI;
using System.Xml;
using EO.Web.Internal;
using HtmlAgilityPack;
using iTextSharp.text;
using OfficeOpenXml;
using Outsourcing_System.CommonClasses;
using Outsourcing_System.PdfCompare_Classes;

namespace Outsourcing_System
{
    public partial class Process1 : Page
    {
        TableDetection tblDetectionObj = new TableDetection();
        GlobalVar objGlobal = new GlobalVar();
        MyDBClass objMyDBClass = new MyDBClass();

        #region ViewState

        public int CurrentPageTableId
        {
            get
            {
                if (ViewState["CurrentPageTableId"] != null)
                    return Convert.ToInt32(ViewState["CurrentPageTableId"]);

                return 0;
            }
            set { ViewState["CurrentPageTableId"] = value; }
        }

        #endregion

        public void ClearHeaderRowText()
        {
            if (divHeaderRow != null) divHeaderRow.InnerText = "";
        }

        public void ClearCaptionRowText()
        {
            if (divCaptionRow != null) divCaptionRow.InnerText = "";
        }


        #region Page Events

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //hfIsIgnoreAlgo.Value = "false";
                string bookId = Convert.ToString(Session["MainBook"]);
                string savingPath = Common.GetDirectoryPath();

                CreatePdfPreviewDirectory(bookId, savingPath);

                string mainDirectoryPath = Common.GetDirectoryPath();
                string pdfDirectoryPath = mainDirectoryPath + "\\" + bookId;
                string highlightedPdfPath = pdfDirectoryPath + "\\" + bookId + "_Highlighted.pdf";
                string xslsFilesPath = pdfDirectoryPath + "\\DetectedTables\\TableTasks\\";

                Session["tempXmlPath"] = pdfDirectoryPath + "\\DetectedTables\\Temp_Table.xml";
                Session["tempXmlPath_actual"] = pdfDirectoryPath + "\\DetectedTables\\Temp_Table_actual.xml";

                int avgWordSpace = tblDetectionObj.AvgWordSpace(pdfDirectoryPath + "\\DetectedTables\\Temp_Table.xml");
                int AverageVeticalSpace = tblDetectionObj.AvgLineSpace(pdfDirectoryPath + "\\DetectedTables\\Temp_Table_actual.xml");
                Session["AverageVeticalSpace"] = AverageVeticalSpace;
                Session["AverageSpace"] = avgWordSpace;

                //int totalPages = GetTotalPageCount(highlightedPdfPath);
                //if (totalPages > 0)
                //{
                //    Session["TotalPages"] = totalPages;
                //    ////lblTotalPages.Text = Convert.ToString(totalPages);
                //}

                //Session["CurrentPage"] = 1;

                //ShowPdf(Convert.ToString(Session["CurrentPage"]));

                List<string> tableContainingPages = GetTablePages(xslsFilesPath);

                if (tableContainingPages != null && tableContainingPages.Count > 0)
                {
                    Session["TablePages"] = tableContainingPages;
                    Session["TotalPages"] = tableContainingPages.Count;
                    ////lblTotalPages.Text = Convert.ToString(totalPages);

                    lblTotalPages.Text = Convert.ToString(tableContainingPages.Count);

                    if (string.IsNullOrEmpty(Convert.ToString(Session["CurrentPage"])))
                    {
                        Session["CurrentPage"] = 1;
                        lblPageNum.Text = "1";
                    }

                    int pageNum = 0;

                    if (string.IsNullOrEmpty(Convert.ToString(Session["ActualPdfPage"])))
                    {
                        Session["ActualPdfPage"] = tableContainingPages[0];
                        pageNum = Convert.ToInt32(tableContainingPages[0]);
                    }
                    else
                    {
                        lblPageNum.Text = Convert.ToString(Session["ActualPdfPage"]);
                        pageNum = Convert.ToInt32(Session["ActualPdfPage"]);
                    }

                    if (!cbxIgnoreAlgo.Checked)
                    {
                        SetTableCountInUi(pageNum);
                        btnMarkTableLines_Click(this, null);
                    }

                    ShowPdf(pageNum);
                }
            }
        }

        //if (!string.IsNullOrEmpty(Convert.ToString(Session["CurrentPage"])) &&
        //        !string.IsNullOrEmpty(Convert.ToString(Session["CurrentTable"])) &&
        //        !string.IsNullOrEmpty(Convert.ToString(Session["TotalTables"])))

        protected void btnNextTable_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["ActualPdfPage"])) &&
                !string.IsNullOrEmpty(Convert.ToString(Session["CurrentTable"])) &&
                !string.IsNullOrEmpty(Convert.ToString(Session["TotalTables"])))
            {
                int currentTable = Convert.ToInt32(Session["CurrentTable"]);
                int totalTables = Convert.ToInt32(Session["TotalTables"]);

                currentTable = currentTable < totalTables ? ++currentTable : currentTable;

                if (currentTable > 1)
                {
                    lblCurrentTable.Text = Convert.ToString(currentTable);

                    Session["CurrentTable"] = currentTable;

                    if (!cbxIgnoreAlgo.Checked)
                        btnMarkTableLines_Click(this, null);
                }
            }
        }

        protected void btnPrevTable_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["ActualPdfPage"])) &&
                !string.IsNullOrEmpty(Convert.ToString(Session["CurrentTable"])) &&
                !string.IsNullOrEmpty(Convert.ToString(Session["TotalTables"])))
            {
                int currentTable = Convert.ToInt32(Session["CurrentTable"]);

                currentTable = currentTable > 1 ? --currentTable : currentTable;

                if (currentTable > 0)
                {
                    lblCurrentTable.Text = Convert.ToString(currentTable);

                    Session["CurrentTable"] = currentTable;

                    if (!cbxIgnoreAlgo.Checked)
                        btnMarkTableLines_Click(this, null);
                }
            }
        }

        public void SetTableCountInUi(int pageNum)
        {
            string bookId = Convert.ToString(Session["MainBook"]);
            string mainDirectoryPath = Common.GetDirectoryPath();

            if (string.IsNullOrEmpty(mainDirectoryPath)) return;

            string sourceFilePath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\TableXmls\\";

            int detectedTablesCount = new DirectoryInfo(sourceFilePath).GetFiles()
                .Where(o => (o.Name.Split('_').Length < 1 ? false : o.Name.Split('_')[1].Equals(Convert.ToString(pageNum))))
                .Where(x => x.Name.Contains(".xml")).Count();

            if (detectedTablesCount > 0)
            {
                Session["TotalTables"] = detectedTablesCount;
                Session["CurrentTable"] = 1;

                lblCurrentTable.Text = "1";
                lblTotalTables.Text = "/ " + Convert.ToString(detectedTablesCount);
            }
        }

        protected void btnNextPage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["CurrentPage"])) &&
                !string.IsNullOrEmpty(Convert.ToString(Session["TotalPages"])) &&
                !string.IsNullOrEmpty(Convert.ToString(Session["TablePages"])))
            {
                tblDetectionObj.TableHeader = null;
                tblDetectionObj.TableCaption = null;

                CurrentPageTableId = 0;

                int currentPage = Convert.ToInt32(Session["CurrentPage"]);
                int totalPages = Convert.ToInt32(Session["TotalPages"]);
                int pageNum = 0;

                List<string> tableContainingPages = (List<string>)Session["TablePages"];

                if (currentPage > tableContainingPages.Count) return;
                pageNum = Convert.ToInt32(tableContainingPages.ElementAt(currentPage - 1));
                SaveChangesInXml(Convert.ToInt32(pageNum));

                currentPage = currentPage < totalPages ? ++currentPage : currentPage;

                if (currentPage > 1)
                {
                    Session["CurrentPage"] = currentPage;
                    pageNum = Convert.ToInt32(tableContainingPages.ElementAt(currentPage - 1));
                    Session["ActualPdfPage"] = pageNum;
                    Session["isNextClicked"] = "True";
                    //SaveChangesInXml(Convert.ToInt32(pageNum));

                    cbxIgnoreAlgo.Checked = false;
                    Session["isIgnorAlgoChecked"] = false;

                    lblPageNum.Text = Convert.ToString(pageNum);

                    //if (!cbxIgnoreAlgo.Checked)
                    //{
                    SetTableCountInUi(pageNum);
                    btnMarkTableLines_Click(this, null);
                    //btnSaveTable_Click(this, null);
                    //}
                }

                ShowPdf(pageNum);
            }
        }

        protected void btnPrevPage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["CurrentPage"])))
            {
                tblDetectionObj.TableHeader = null;
                tblDetectionObj.TableCaption = null;

                CurrentPageTableId = 0;
                int currentPage = Convert.ToInt32(Session["CurrentPage"]);
                currentPage = currentPage > 1 ? --currentPage : currentPage;

                List<string> tableContainingPages = (List<string>)Session["TablePages"];

                if (currentPage >= tableContainingPages.Count) return;

                Session["CurrentPage"] = currentPage;

                int pageNum = Convert.ToInt32(tableContainingPages.ElementAt(currentPage - 1));

                Session["ActualPdfPage"] = pageNum;

                cbxIgnoreAlgo.Checked = false;
                Session["isIgnorAlgoChecked"] = false;

                lblPageNum.Text = Convert.ToString(pageNum);

                //if (!cbxIgnoreAlgo.Checked)
                //{
                SetTableCountInUi(pageNum);
                btnMarkTableLines_Click(this, null);
                //}

                ShowPdf(pageNum);
            }
        }

        //Save button inside of table popup
        protected void btnSaveTable_Click(object sender, EventArgs e)
        {
            string tempXmlPath = Convert.ToString(HttpContext.Current.Session["tempXmlPath"]);
            TableDetection tableObj = new TableDetection();

            if (tableObj.TempXmlDoc == null)
                return;

            //Inserting table tag instead of table lines in tem.xml
            tableObj.TempXmlDoc.Save(tempXmlPath);

            //var finalTableHtml = tableBodyRows.InnerHtml;

            var finalTableHtml = hfTableBodyRows.Value;

            //Para is selected
            if (rbtnlTableHeader.SelectedIndex == 0)
            {

            }
            //Table body is selected
            else if (rbtnlTableHeader.SelectedIndex == 1)
            {

            }
            //Table header is selected
            else if (rbtnlTableHeader.SelectedIndex == 2)
            {

            }

            var table = tableObj.TableXml;
            if (table != null)
            {
                string mainDirectoryPath = Common.GetDirectoryPath();
                string bookId = Convert.ToString(HttpContext.Current.Session["MainBook"]);
                string sourcePdfPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\" + bookId + "_actual.tetml";
                string outputHighlightedPdfPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\" + bookId + "_actual_Manual_Highlighted.pdf";

                int pageNumber = tableObj.GetCurrentPageNum();

                string dirPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table";
                string xmlDirPath = dirPath + "//TableXmls";
                string tableSavingPath = xmlDirPath + "//" + "Table_" + pageNumber + "_" + CurrentPageTableId + ".xml";

                if (CurrentPageTableId == 0) CurrentPageTableId = 1;
                else
                {
                    if (!string.IsNullOrEmpty(tableSavingPath) && File.Exists(tableSavingPath))
                    {
                        if (tableSavingPath.Split('_').Length > 0)
                            CurrentPageTableId = Convert.ToInt32(tableSavingPath.Split('_')[2]);
                    }
                    else CurrentPageTableId++;
                }

                bool isFinalXmlCreated = false;

                List<string> llyList = new List<string>();
                llyList = table.SelectNodes("descendant::Box/@lly").Cast<XmlNode>().Select(x => x.Value).Distinct().ToList();

                if (llyList.Count > 0)
                    isFinalXmlCreated = true;

                XmlNode finalXml = null;

                if (isFinalXmlCreated)
                    finalXml = tableObj.ConvertToFinalXml(table, finalTableHtml, pageNumber, CurrentPageTableId,
                        rbtnlTableBorder.SelectedIndex,
                        rbtnlHeaderRow.SelectedIndex, divHeaderRow.InnerText, divCaptionRow.InnerText,
                        rbtnlTableHeader.SelectedIndex, rbtnlTableCaption.SelectedIndex);
                else
                    finalXml = tableObj.ConvertToFinalXmlUpdate(table, finalTableHtml, pageNumber, CurrentPageTableId, rbtnlTableBorder.SelectedIndex,
                                            rbtnlHeaderRow.SelectedIndex, divHeaderRow.InnerText, divCaptionRow.InnerText, rbtnlTableHeader.SelectedIndex, rbtnlTableCaption.SelectedIndex);

                    //finalXml = table;

                //var finalTableXml = AdjustEmptyColumns(finalXml);
                //tblDetectionObj.CreateXls(finalTableXml);

                tblDetectionObj.CreateXls(finalXml);

                //List<string> abbrCoord = tblDetectionObj.GetCoordinates(sourcePdfPath, table);
                //if (abbrCoord != null)
                //{
                //    if (abbrCoord.Count > 0)
                //    {
                //        if (tblDetectionObj.HighLightTables(sourcePdfPath.Replace(".tetml", ".pdf"),
                //            outputHighlightedPdfPath, BaseColor.GREEN, abbrCoord))
                //        {
                //            List<string> tableContainingPages = (List<string>)HttpContext.Current.Session["TablePages"];
                //            int currentPage = Convert.ToInt32(HttpContext.Current.Session["CurrentPage"]);
                //            string pageNum = tableContainingPages.ElementAt(currentPage - 1);

                //            string pdfPreviewDirPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\PdfPreview";
                //            string outputManualHighlightedPdfPath = pdfPreviewDirPath + "\\" + pageNum + "_Manual_Highlighted.pdf";

                //            tblDetectionObj.ExtractPage(outputHighlightedPdfPath, outputManualHighlightedPdfPath, pageNum);
                //        }
                //    }
                //}

                List<string> coordList = new List<string>();

                if (!string.IsNullOrEmpty(sourcePdfPath))
                {
                    coordList = tblDetectionObj.GetCoordinates(sourcePdfPath.Replace(".pdf", ".tetml"), finalXml, pageNumber, "tableHeader");
                    if (coordList != null && coordList.Count > 0)
                        tblDetectionObj.HighLightTables(sourcePdfPath.Replace(".tetml", ".pdf"), outputHighlightedPdfPath, BaseColor.GRAY, coordList);

                    coordList = tblDetectionObj.GetCoordinates(sourcePdfPath.Replace(".pdf", ".tetml"), finalXml, pageNumber, "headerRow");
                    if (coordList != null && coordList.Count > 0)
                        tblDetectionObj.HighLightTables(sourcePdfPath.Replace(".tetml", ".pdf"), outputHighlightedPdfPath, BaseColor.LIGHT_GRAY, coordList);

                    coordList = tblDetectionObj.GetCoordinates(sourcePdfPath.Replace(".pdf", ".tetml"), finalXml, pageNumber, "tableCaption");
                    if (coordList != null && coordList.Count > 0)
                        tblDetectionObj.HighLightTables(sourcePdfPath.Replace(".tetml", ".pdf"), outputHighlightedPdfPath, BaseColor.PINK, coordList);

                    coordList = tblDetectionObj.GetCoordinates(sourcePdfPath.Replace(".pdf", ".tetml"), finalXml, pageNumber, "tableRows");
                    if (coordList != null && coordList.Count > 0)
                        tblDetectionObj.HighLightTables(sourcePdfPath.Replace(".tetml", ".pdf"), outputHighlightedPdfPath, BaseColor.GREEN, coordList);

                    List<string> tableContainingPages = (List<string>)HttpContext.Current.Session["TablePages"];
                    int currentPage = Convert.ToInt32(HttpContext.Current.Session["CurrentPage"]);
                    int pageNum = Convert.ToInt32(tableContainingPages.ElementAt(currentPage - 1));

                    string pdfPreviewDirPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\PdfPreview";
                    string outputManualHighlightedPdfPath = pdfPreviewDirPath + "\\" + pageNum + "_Manual_Highlighted.pdf";

                    if (File.Exists(outputHighlightedPdfPath))
                        tblDetectionObj.ExtractPage(outputHighlightedPdfPath, outputManualHighlightedPdfPath, pageNum);
                }

                tableObj.TempXmlDoc = null;
                tableObj.TableXml = null;

                Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "ShowMessege(true)", true);
            }
        }

        protected void saveAutoDetectedTablesInXml(int tableHeaderIdex, int tableCaptionIdex)
        {
            string tempXmlPath = Convert.ToString(HttpContext.Current.Session["tempXmlPath"]);
            TableDetection tableObj = new TableDetection();

            if (tableObj.TempXmlDoc == null)
                return;

            //Inserting table tag instead of table lines in tem.xml
            tableObj.TempXmlDoc.Save(tempXmlPath);

            var finalTableHtml = hfTableBodyRows.Value;

            var table = tableObj.TableXml;
            if (table != null)
            {
                int pageNumber = tableObj.GetCurrentPageNum();

                if (CurrentPageTableId == 0) CurrentPageTableId = 1;
                else CurrentPageTableId++;

                XmlNode finalXml = tableObj.ConvertToFinalXml(table, finalTableHtml, pageNumber, CurrentPageTableId,
                                                        rbtnlTableBorder.SelectedIndex, rbtnlHeaderRow.SelectedIndex, divHeaderRow.InnerText, divCaptionRow.InnerText, tableHeaderIdex, tableCaptionIdex);
                tblDetectionObj.CreateXls(finalXml);

                //Change by aamir 2017-02-08
                //tableObj.TempXmlDoc = null;
                //tableObj.TableXml = null;
            }
        }

        //Table popup after selecting text
        protected void btnMarkTableLines_Click(object sender, EventArgs e)
        {
            XmlNode tableXml = null;
            List<string> tableLinesList = new List<string>();

            TableDetection tblDetecObj = new TableDetection();
            int pageNumber = tblDetecObj.GetCurrentPageNum();

            bool isManualSelection = true;

            List<string> llyList = new List<string>();

            if (e == null)
            {
                divHeaderRow.InnerHtml = "";
                divCaptionRow.InnerHtml = "";

                tableXml = GetAutoMarkedTableXml();

                ReadAutoDetectedTableXml(tableXml, pageNumber);

                //tableXml = GetAutoMarkedTableXml();

                //if (tableXml == null) return;

                //XmlDocument doc = new XmlDocument();
                //doc.LoadXml(tableXml.OuterXml);

                //llyList = tableXml.SelectNodes("descendant::Box/@lly").Cast<XmlNode>().Select(x => x.Value).Distinct().ToList();

                //if (llyList.Count > 0)
                //{
                //    tableLinesList = GetLinesFromAutoDetecXml(tableXml);
                //    if (tableLinesList == null) return;

                //    tableXml = tblDetecObj.SaveTablesInTempXml(tableLinesList, pageNumber, cbxIgnoreAlgo.Checked, false);
                //}

                //tblDetecObj.TempXmlDoc = doc;
                //tblDetecObj.TableXml = tableXml;
                //isManualSelection = false;
            }
            else
            {
                if (string.IsNullOrEmpty(hfMarkedTableText.Value)) return;
                tableLinesList = tblDetecObj.GetPdfJsSelectedLines(hfMarkedTableText.Value, pageNumber);

                if (tableLinesList == null) return;

                tableXml = tblDetecObj.SaveTablesInTempXml(tableLinesList, pageNumber, cbxIgnoreAlgo.Checked, false);

                //tableXml = tblDetecObj.MarkTableInXml(tableLinesList, pageNumber, cbxIgnoreAlgo.Checked, false);

                ////if (tableLinesList == null) return;

                ////tableXml = tblDetecObj.SaveTablesInTempXml(tableLinesList, pageNumber, cbxIgnoreAlgo.Checked, false);

                ////if (tableXml == null) return;

                //tableXml = tblDetecObj.InsertEmptyColumns(tableXml);

                ////if (llyList.Count > 0)
                ////    tableXml = tblDetecObj.InsertEmptyColumns(tableXml);
                ////else
                ////    tableXml = tblDetecObj.InsertEmptyColumnsInFinalXml(tableXml);

                bool containsBoldFont = false;

                if (tableXml == null) return;

                var rows = tableXml.SelectNodes("//Row");

                if (rows == null || rows.Count < 1) return;

                int tableHeaderCount = 0;
                StringBuilder tableHeader = new StringBuilder();
                StringBuilder tableCaption = new StringBuilder();
                StringBuilder tableBody = new StringBuilder();

                var tableHeaderList = rows.Cast<XmlNode>().TakeWhile(x => x.ChildNodes.Count == 1).ToList();

                foreach (XmlNode row in tableHeaderList)
                {
                    var tableHeaderLine =
                        row.ChildNodes[0].SelectNodes("descendant::Text")
                            .Cast<XmlNode>()
                            .Select(y => y.InnerText)
                            .ToList();
                    tableHeader.Append(String.Join(" ", tableHeaderLine.ToArray()) + " ");
                    tableHeader.Append("<br />");
                }

                tableHeaderCount = tableHeaderList.Count;

                //var tblBodyRows = rows.Cast<XmlNode>().Skip(tableHeaderCount).ToList();

                //tableXml = InsertEmptyColumns(tblBodyRows[0]);

                ////var tablecaptionList = rows.Cast<XmlNode>().Skip(tableHeaderCount).Where(x => x.ChildNodes.Count == 1).ToList();

                var tablecaptionList = rows.Cast<XmlNode>().Reverse().TakeWhile(x => x.ChildNodes.Count == 1).ToList();

                if (tablecaptionList.Count > 0)
                {
                    for (int i = tablecaptionList.Count - 1; i >= 0; i--)
                    {
                        var tableCaptionLine =
                            tablecaptionList[i].SelectNodes("descendant::Text")
                                .Cast<XmlNode>()
                                .Select(y => y.InnerText)
                                .ToList();
                        tableCaption.Append(String.Join(" ", tableCaptionLine.ToArray()) + " ");
                        tableCaption.Append("<br />");
                    }
                }
                //foreach (XmlNode row in tablecaptionList)
                //{
                //    var tableCaptionLine = row.ChildNodes[0].SelectNodes("descendant::Text").Cast<XmlNode>().Select(y => y.InnerText).ToList();
                //    tableCaption.Append(String.Join(" ", tableCaptionLine.ToArray()) + " ");
                //}

                var tableRowsList = rows.Cast<XmlNode>().Where(x => x.ChildNodes.Count > 1).ToList();

                if (tableRowsList.Count > 0)
                    tableBody.Append(DrawTableBody(tableRowsList));

                //int maxColumns = tableRowsList.Select(x => x.ChildNodes.Count).Max();
                //foreach (XmlNode row in tableRowsList)
                //{
                //    var tableBodyLines = row.ChildNodes[0].SelectNodes("descendant::Text")
                //                        .Cast<XmlNode>()
                //                        .Select(y => y.InnerText)
                //                        .ToList();
                //    tableBody.Append(String.Join(" ", tableBodyLines.ToArray()) + " ");
                //}

                //By default table border is on
                rbtnlTableBorder.SelectedIndex = 0;

                if (containsBoldFont) rbtnlHeaderRow.SelectedIndex = 0;
                else rbtnlHeaderRow.SelectedIndex = 1;

                if (!String.IsNullOrEmpty(Convert.ToString(tableHeader)))
                {
                    rbtnlTableHeader.SelectedIndex = 2;
                    btnManualHeader.Attributes.Add("style", "display:none");
                }
                else
                {
                    rbtnlTableHeader.SelectedIndex = 3;
                    btnManualHeader.Attributes.Add("style", "display:inline-block; position:relative; bottom:4px");
                }

                if (!String.IsNullOrEmpty(Convert.ToString(tableCaption)))
                {
                    rbtnlTableCaption.SelectedIndex = 2;
                    btnManualCaption.Attributes.Add("style", "display:none");
                }
                else
                {
                    rbtnlTableCaption.SelectedIndex = 3;
                    btnManualCaption.Attributes.Add("style", "display:inline-block; position:relative; bottom:4px");
                }

                divHeaderRow.InnerHtml = Convert.ToString(tableHeader);
                divCaptionRow.InnerHtml = Convert.ToString(tableCaption);

                hfTableBodyRows.Value = Convert.ToString(tableBody);
                hfColumnWidths.Value = DrawColumnWidthTable(tableRowsList);
                ClientScript.RegisterStartupScript(this.GetType(), "showTableDialog", "ShowMarkedTableDialog();", true);

                //if (!isManualSelection)
                //    saveAutoDetectedTablesInXml(rbtnlTableHeader.SelectedIndex, rbtnlTableCaption.SelectedIndex);
            }
        }

        public void ReadAutoDetectedTableXml(XmlNode tableXml, int pageNum)
        {
            //tableXml = GetAutoMarkedTableXml();

            if (tableXml == null) return;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(tableXml.OuterXml);

            List<string> llyList = new List<string>();
            llyList =
                tableXml.SelectNodes("descendant::Box/@lly").Cast<XmlNode>().Select(x => x.Value).Distinct().ToList();

            TableDetection tblDetecObj = new TableDetection();

            if (llyList.Count > 0)
            {
                List<string> tableLinesList = new List<string>();

                tableLinesList = GetLinesFromAutoDetecXml(tableXml);
                if (tableLinesList == null) return;

                tableXml = tblDetecObj.SaveTablesInTempXml(tableLinesList, pageNum, cbxIgnoreAlgo.Checked, false);

                bool containsBoldFont = false;

                if (tableXml == null) return;

                var rows = tableXml.SelectNodes("//Row");

                if (rows == null || rows.Count < 1) return;

                int tableHeaderCount = 0;
                StringBuilder tableHeader = new StringBuilder();
                StringBuilder tableCaption = new StringBuilder();
                StringBuilder tableBody = new StringBuilder();

                var tableHeaderList = rows.Cast<XmlNode>().TakeWhile(x => x.ChildNodes.Count == 1).ToList();

                foreach (XmlNode row in tableHeaderList)
                {
                    var tableHeaderLine =
                        row.ChildNodes[0].SelectNodes("descendant::Text")
                            .Cast<XmlNode>()
                            .Select(y => y.InnerText)
                            .ToList();
                    tableHeader.Append(String.Join(" ", tableHeaderLine.ToArray()) + " ");
                    tableHeader.Append("<br />");
                }

                var tablecaptionList = rows.Cast<XmlNode>().Reverse().TakeWhile(x => x.ChildNodes.Count == 1).ToList();

                if (tablecaptionList.Count > 0)
                {
                    for (int i = tablecaptionList.Count - 1; i >= 0; i--)
                    {
                        var tableCaptionLine =
                            tablecaptionList[i].SelectNodes("descendant::Text")
                                .Cast<XmlNode>()
                                .Select(y => y.InnerText)
                                .ToList();
                        tableCaption.Append(String.Join(" ", tableCaptionLine.ToArray()) + " ");
                        tableCaption.Append("<br />");
                    }
                }

                var tableRowsList = rows.Cast<XmlNode>().Where(x => x.ChildNodes.Count > 1).ToList();

                if (tableRowsList.Count > 0)
                    tableBody.Append(DrawTableBody(tableRowsList));

                //By default table border is on
                rbtnlTableBorder.SelectedIndex = 0;

                if (containsBoldFont) rbtnlHeaderRow.SelectedIndex = 0;
                else rbtnlHeaderRow.SelectedIndex = 1;

                if (!String.IsNullOrEmpty(Convert.ToString(tableHeader)))
                {
                    rbtnlTableHeader.SelectedIndex = 2;
                    btnManualHeader.Attributes.Add("style", "display:none");
                }
                else
                {
                    rbtnlTableHeader.SelectedIndex = 3;
                    btnManualHeader.Attributes.Add("style", "display:inline-block; position:relative; bottom:4px");
                }

                if (!String.IsNullOrEmpty(Convert.ToString(tableCaption)))
                {
                    rbtnlTableCaption.SelectedIndex = 2;
                    btnManualCaption.Attributes.Add("style", "display:none");
                }
                else
                {
                    rbtnlTableCaption.SelectedIndex = 3;
                    btnManualCaption.Attributes.Add("style", "display:inline-block; position:relative; bottom:4px");
                }

                tblDetecObj.TempXmlDoc = doc;
                tblDetecObj.TableXml = tableXml;

                divHeaderRow.InnerHtml = Convert.ToString(tableHeader);
                divCaptionRow.InnerHtml = Convert.ToString(tableCaption);

                hfTableBodyRows.Value = Convert.ToString(tableBody);
                hfColumnWidths.Value = DrawColumnWidthTable(tableRowsList);
                ClientScript.RegisterStartupScript(this.GetType(), "showTableDialog", "ShowMarkedTableDialog();", true);
            }
            else
                ReadFinalTableXml(tableXml, pageNum);
        }

        public void ReadFinalTableXml(XmlNode tableXml, int pageNum)
        {
            tableXml = GetAutoMarkedTableXml();

            if (tableXml == null) return;

            XmlDocument doc = new XmlDocument();
            doc.LoadXml(tableXml.OuterXml);

            TableDetection tblDetecObj = new TableDetection();

            bool containsBoldFont = false;

            StringBuilder tableHeader = new StringBuilder();
            StringBuilder tableCaption = new StringBuilder();
            StringBuilder tableBody = new StringBuilder();

            var tableHeaderLines = tableXml.SelectNodes("//header/descendant::ln");

            if (tableHeaderLines != null && tableHeaderLines.Count > 0)
            {
                foreach (XmlNode ln in tableHeaderLines)
                {
                    tableHeader.Append(ln.InnerText + "<br />");
                }
            }

            var tablecaptionLines = tableXml.SelectNodes("//caption/descendant::ln");

            if (tablecaptionLines != null && tablecaptionLines.Count > 0)
            {
                for (int i = tablecaptionLines.Count - 1; i >= 0; i--)
                {
                    tableCaption.Append(tablecaptionLines[i].InnerText + "<br />");
                }
            }

            var tableBodyRows = tableXml.SelectNodes("//Row").Cast<XmlNode>().ToList();

            if (tableBodyRows == null || tableBodyRows.Count < 1) return;

            tableBody.Append(DrawTableBodyAutoXml(tableBodyRows));

            //By default table border is on
            rbtnlTableBorder.SelectedIndex = 0;

            if (containsBoldFont) rbtnlHeaderRow.SelectedIndex = 0;
            else rbtnlHeaderRow.SelectedIndex = 1;

            if (!String.IsNullOrEmpty(Convert.ToString(tableHeader)))
            {
                rbtnlTableHeader.SelectedIndex = 2;
                btnManualHeader.Attributes.Add("style", "display:none");
            }
            else
            {
                rbtnlTableHeader.SelectedIndex = 3;
                btnManualHeader.Attributes.Add("style", "display:inline-block; position:relative; bottom:4px");
            }

            if (!String.IsNullOrEmpty(Convert.ToString(tableCaption)))
            {
                rbtnlTableCaption.SelectedIndex = 2;
                btnManualCaption.Attributes.Add("style", "display:none");
            }
            else
            {
                rbtnlTableCaption.SelectedIndex = 3;
                btnManualCaption.Attributes.Add("style", "display:inline-block; position:relative; bottom:4px");
            }

            tblDetecObj.TempXmlDoc = doc;
            tblDetecObj.TableXml = tableXml;

            if (!String.IsNullOrEmpty(Convert.ToString(tableHeader)))
                divHeaderRow.InnerHtml = Convert.ToString(tableHeader);

            if (!String.IsNullOrEmpty(Convert.ToString(tableCaption)))
                divCaptionRow.InnerHtml = Convert.ToString(tableCaption);

            hfTableBodyRows.Value = Convert.ToString(tableBody);
            hfColumnWidths.Value = DrawColumnWidthTable(tableBodyRows);
            ClientScript.RegisterStartupScript(this.GetType(), "showTableDialog", "ShowMarkedTableDialog();", true);

            //saveAutoDetectedTablesInXml(rbtnlTableHeader.SelectedIndex, rbtnlTableCaption.SelectedIndex);
        }

        protected XmlNode GetAutoMarkedTableXml()
        {
            if (string.IsNullOrEmpty(Convert.ToString(Session["ActualPdfPage"])) ||
                string.IsNullOrEmpty(Convert.ToString(Session["CurrentTable"])) ||
                string.IsNullOrEmpty(Convert.ToString(Session["MainBook"])))
                return null;

            int pageNum = Convert.ToInt32(Session["ActualPdfPage"]);
            int currentTable = Convert.ToInt32(Session["CurrentTable"]);
            string bookId = Convert.ToString(Session["MainBook"]);

            string mainDirectoryPath = Common.GetDirectoryPath();

            if (string.IsNullOrEmpty(mainDirectoryPath)) return null;

            //string sourceFilePath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\TableXmls\\";

            string sourceFilePath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table\\TableXmls\\";

            if (!Directory.Exists(sourceFilePath))
                sourceFilePath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\TableXmls\\";

            var detectedTableXmls = new DirectoryInfo(sourceFilePath).GetFiles()
                .Where(o => (o.Name.Split('_').Length < 1 ? false : o.Name.Split('_')[1].Equals(Convert.ToString(pageNum))))
                .Where(x => x.Name.Contains(".xml")).ToList();

            if (detectedTableXmls.Count > 0 && currentTable > 0 && currentTable <= detectedTableXmls.Count)
            {
                XmlDocument tableXmlNode = new XmlDocument();

                if (detectedTableXmls.Count > 1)
                {
                    btnNextTable.Visible = true;
                    btnPrevTable.Visible = true;
                    lblCurrentTable.Visible = true;
                    lblTotalTables.Visible = true;
                }
                else
                {
                    btnNextTable.Visible = false;
                    btnPrevTable.Visible = false;
                    lblCurrentTable.Visible = false;
                    lblTotalTables.Visible = false;
                }

                if (File.Exists(detectedTableXmls[currentTable - 1].FullName))
                {
                    tableXmlNode.Load(detectedTableXmls[currentTable - 1].FullName);

                    return tableXmlNode;
                }
            }

            return null;
        }

        protected void btnFinishTask_Click(object sender, EventArgs e)
        {
            int currentPage = Convert.ToInt32(Session["CurrentPage"]);
            string pageNum = "";

            List<string> tableContainingPages = (List<string>)Session["TablePages"];

            if (currentPage > tableContainingPages.Count) return;

            pageNum = tableContainingPages.ElementAt(currentPage - 1);
            SaveChangesInXml(Convert.ToInt32(pageNum));

            string bookId = Convert.ToString(Session["MainBook"]);
            string mainDirectoryPath = Common.GetDirectoryPath();

            objGlobal.XMLPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\TaggingUntagged\\" + bookId + "-1.rhyw";
            objGlobal.LoadXml();

            string finalXslsFilesPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table\\FinalTableTasks\\";

            if (!Directory.Exists(finalXslsFilesPath)) return;

            string[] uploadedXslFiles = Directory.GetFiles(finalXslsFilesPath, "*.xlsx");

            string tableXmlPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table\\TableXmls\\";

            //if (dummyTableCount == uploadedXslFiles.Length)
            //{

            if (uploadedXslFiles == null || uploadedXslFiles.Length == 0) return;

            bool status = false;

            var sortedXlsxPagesList = GetXlsxSortedPages(uploadedXslFiles);

            List<string> xlsLines = new List<string>();
            XmlNodeList tableColLinesList = null;

            string fileName = "";

            if (sortedXlsxPagesList != null)
            {
                if (sortedXlsxPagesList.Count > 0)
                {
                    foreach (var xlsxFile in sortedXlsxPagesList)
                    {
                        int page = xlsxFile.PageNum;

                        if (page == 13)
                        {

                        }

                        //var tableXmlWithCoord = GetTablesFromXml(xlsxFile, tableXmlPath);
                        //var tableXmlNode = GetXlsWithoutCoord(xlsxFile, finalXslsFilesPath);

                        fileName = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table\\TableXmls\\" + xlsxFile.Name + ".xml";

                        XmlDocument tableXmlNode = new XmlDocument();

                        if (File.Exists(fileName))
                            tableXmlNode.Load(fileName);

                        if (tableXmlNode != null)
                        {
                            List<string> tblStartLineWords = new List<string>();
                            List<string> tblEndLineWords = new List<string>();

                            if (tableXmlNode != null)
                            {
                                xlsLines = tableXmlNode.SelectNodes("//col").Cast<XmlNode>().Select(x => x.InnerText)
                                                                            .Where(x => (!string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x))).ToList();

                                tableColLinesList = tableXmlNode.SelectNodes("//col");

                                if (xlsLines.Count > 0)
                                {
                                    if (xlsLines.Count == 1)
                                    {
                                        tblStartLineWords.Add(xlsLines[0]);
                                        tblEndLineWords.Add(xlsLines[xlsLines.Count - 1]);
                                    }
                                    else if (xlsLines.Count == 2)
                                    {
                                        tblStartLineWords.Add(xlsLines[0]);
                                        tblStartLineWords.Add(xlsLines[1]);

                                        tblEndLineWords.Add(xlsLines[xlsLines.Count - 2]);
                                        tblEndLineWords.Add(xlsLines[xlsLines.Count - 1]);
                                    }
                                    else if (xlsLines.Count >= 3)
                                    {
                                        tblStartLineWords.Add(xlsLines[0]);
                                        tblStartLineWords.Add(xlsLines[1]);
                                        tblStartLineWords.Add(xlsLines[2]);

                                        tblEndLineWords.Add(xlsLines[xlsLines.Count - 3]);
                                        tblEndLineWords.Add(xlsLines[xlsLines.Count - 2]);
                                        tblEndLineWords.Add(xlsLines[xlsLines.Count - 1]);
                                    }
                                }
                            }

                            status = tblDetectionObj.SaveTablesInFinalXml(tableColLinesList, tblStartLineWords, tblEndLineWords,
                               Convert.ToString(xlsxFile.PageNum), tableXmlPath + xlsxFile.Name + ".xml");
                        }
                    }
                }
            }

            if (status)
            {
                string aid = Convert.ToString(Session["AID"]);
                string userId = Convert.ToString(Session["LoginId"]);
                string queryBookID = "Select BID From Activity Where AID=" + aid;
                string bid = objMyDBClass.GetID(queryBookID);

                CompleteTableTask(userId, bid);

                int result = objMyDBClass.CreateTask(bid, "Working", "ComplexBitsMapping", userId);

                //CreateMistakeInjectionTask(userId, bid);
                Response.Redirect("OnlineTestUser.aspx?ct=table", true);
            }
            else
            {
                //ucShowMessage1.ShowMessage(MessageTypes.Error, "Some Error occurs in Table Task.");
                Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "ShowMessege(false)", true);
            }
        }

        protected void btnMarkImage_Click(object sender, EventArgs e)
        {
            ClientScript.RegisterStartupScript(this.GetType(), "showTableDialog", "ShowSaveImageDialog();", true);
        }

        protected void btnSaveImage_Click(object sender, EventArgs e)
        {
            try
            {
                if (fuSourceFile.HasFile && !string.IsNullOrEmpty(tbxImagePath.Text.Trim()))
                {
                    string bookId = Convert.ToString(Session["MainBook"]);
                    string mainDirectoryPath = Common.GetDirectoryPath();
                    string imgSavingPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Image";
                    bool status = CreateDirectory(imgSavingPath);
                    if (status)
                    {
                        string imgPath = imgSavingPath + "\\" + Path.GetFileNameWithoutExtension(fuSourceFile.FileName) + ".jpeg";
                        fuSourceFile.SaveAs(imgPath);
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnMarkTableHeader_Click(object sender, EventArgs e)
        {
            hfSetManualTableHeader.Value = "false";
        }

        protected void btnMarkTableCaption_Click(object sender, EventArgs e)
        {
            hfSetManualTableCaption.Value = "false";
        }

        protected void lbtnHome_Click(object sender, System.EventArgs e)
        {
            if (Session["LoginId"] != null)
            {
                Response.Redirect("OnlineTestUser.aspx");
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

        protected void cbxIgnoreAlgo_CheckedChanged(object sender, EventArgs e)
        {
            hfIsIgnoreAlgo.Value = cbxIgnoreAlgo.Checked == true ? "true" : "false";

            Session["isIgnorAlgoChecked"] = hfIsIgnoreAlgo.Value;

            if (hfIsIgnoreAlgo.Value.Equals("true"))
            {
                lblCurrentTable.Visible = false;
                lblTotalTables.Visible = false;
                btnNextTable.Visible = false;
                btnPrevTable.Visible = false;
            }

            List<string> tableContainingPages = (List<string>)Session["TablePages"];

            if (tableContainingPages == null) return;

            int currentPage = Convert.ToInt32(Session["CurrentPage"]);

            if (currentPage < 1) return;

            int pageNum = Convert.ToInt32(tableContainingPages.ElementAt(currentPage - 1));

            ShowPdf(pageNum);
        }

        #endregion

        #region Public Functions

        #endregion

        private bool CreatePdfPreviewDirectory(string bookId, string savingPath)
        {
            string pdfPreviewDirPath = savingPath + "\\" + bookId + "\\DetectedTables\\PdfPreview";
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

        public List<string> GetTablePages(string pdfPath)
        {
            try
            {
                List<string> fileNames = Directory.GetFiles(pdfPath, "*.xlsx").Select(x => Path.GetFileNameWithoutExtension(x))
                    .Select(y => (y.Split('_').ToList().Count > 0 ? y.Split('_')[1] : "0")).Distinct().OrderBy(z => Convert.ToInt32(z)).ToList();

                return fileNames;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private bool CreateDirectory(string savingPath)
        {
            bool status = false;

            try
            {
                if (!Directory.Exists(savingPath))
                    Directory.CreateDirectory(savingPath);

                return true;
            }
            catch (Exception)
            {
                return status;
            }
        }

        public void SaveChangesInXml(int page)
        {
            //page -= 1;

            string bookId = Convert.ToString(Session["MainBook"]);
            string mainDirectoryPath = Common.GetDirectoryPath();
            string pdfPreviewDirPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\PdfPreview";
            string outputManualHighlightedPdfPath = pdfPreviewDirPath + "\\" + page + "_Manual_Highlighted.pdf";

            if (!cbxIgnoreAlgo.Checked && !File.Exists(outputManualHighlightedPdfPath))
            {

                SaveChangesInTempXml_actual(page);
            }
        }

        public void SaveChangesInTempXml_actual(int pageNum)
        {
            ////MoveXlsxFiles(pageNum);

            string tempXml_actualPath = Convert.ToString(HttpContext.Current.Session["tempXmlPath_actual"]);
            XmlDocument xmlDoc_actual = new XmlDocument();

            if (!File.Exists(tempXml_actualPath)) return;

            xmlDoc_actual.Load(tempXml_actualPath);
            XmlNode listLines_actual = xmlDoc_actual.SelectSingleNode("//Page[@number=" + pageNum + "]");

            string tempXmlPath = Convert.ToString(HttpContext.Current.Session["tempXmlPath"]);
            XmlDocument xmlDoc = new XmlDocument();

            if (!File.Exists(tempXmlPath)) return;

            xmlDoc.Load(tempXmlPath);
            XmlNode listLines = xmlDoc.SelectSingleNode("//Page[@number=" + pageNum + "]");

            if (listLines == null) return;

            listLines_actual.InnerXml = listLines.InnerXml;
            xmlDoc_actual.Save(tempXml_actualPath);
        }

        private void MoveXlsxFiles(int pageNum)
        {
            string mainDirectoryPath = Common.GetDirectoryPath();
            string bookId = Convert.ToString(Session["MainBook"]);

            string directoryPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table\\";

            if (!Directory.Exists(directoryPath + "\\FinalTableTasks"))
                Directory.CreateDirectory(directoryPath + "\\FinalTableTasks");

            string destFilePath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table\\FinalTableTasks\\";
            string sourceFilePath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\TableTasks\\";

            var uploadedXslFiles = new DirectoryInfo(sourceFilePath).GetFiles()
                .Where(o => (o.Name.Split('_').Length < 1 ? false : o.Name.Split('_')[1].Equals(Convert.ToString(pageNum))))
                .Where(x => x.Name.Contains(".xlsx")).ToList();

            string finalXslsFilesPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table\\FinalTableTasks\\";

            if (uploadedXslFiles != null)
            {
                foreach (var xlsxFile in uploadedXslFiles)
                {
                    if (!File.Exists(destFilePath + "\\" + xlsxFile.Name) && File.Exists(xlsxFile.FullName))
                        File.Copy(xlsxFile.FullName, destFilePath + "\\" + xlsxFile.Name);

                    List<List<String>> rows = ReadXlsxFile(xlsxFile.Name, "", sourceFilePath);
                    //var rows = ReadXlsxFile(xlsxFile.Name, "", sourceFilePath);

                    List<string> tableLines = GetTableLines(rows);
                    int pageNumber = tblDetectionObj.GetCurrentPageNum();
                    //var table = tblDetectionObj.MoveTablesInXml(tableLines,pageNumber, cbxIgnoreAlgo.Checked);

                    var table = tblDetectionObj.SaveTablesInTempXml(tableLines, pageNumber, cbxIgnoreAlgo.Checked, false);
                    if (table != null)
                    {
                        //var tableXmlNode = GetXlsWithoutCoord(xlsxFile, finalXslsFilesPath, 1);
                        tblDetectionObj.SaveAsXml(table);
                    }
                }
            }
        }

        //////To Do
        ////public XmlNode ConvertXmlToFinalFormat(XlsxFile excelFile, string xslsFilesPath)
        ////{
        ////    try
        ////    {
        ////        var rows = ReadXlsxFile(excelFile.Name, excelFile.Extension, xslsFilesPath);

        ////        if (rows == null) return null;

        ////        string bookId = Convert.ToString(Session["MainBook"]);
        ////        string mainDirectoryPath = Common.GetDirectoryPath();

        ////        //string xmlText = "<table id=\"0\" border=\"off\" head-row=\"on\"><tbody ispreviewpassed=\"false\" page=\"2\"><header/>" +
        ////        //                 "<head-row></head-row><Row></Row><caption/></tbody></table>";

        ////        XmlNode table = objGlobal.PBPDocument.CreateElement("table");
        ////        XmlAttribute idAttr = objGlobal.PBPDocument.CreateAttribute("id");
        ////        idAttr.Value = Convert.ToString(excelFile.TableNum);
        ////        XmlAttribute borderAttr = objGlobal.PBPDocument.CreateAttribute("border");
        ////        borderAttr.Value = "off";
        ////        XmlAttribute headrowAttr = objGlobal.PBPDocument.CreateAttribute("head-row");
        ////        headrowAttr.Value = "off";
        ////        table.Attributes.Append(idAttr);
        ////        table.Attributes.Append(borderAttr);
        ////        table.Attributes.Append(headrowAttr);

        ////        XmlNode headerNode = objGlobal.PBPDocument.CreateElement("header");
        ////        table.AppendChild(headerNode);
        ////        XmlNode voiceDescriptionNode = objGlobal.PBPDocument.CreateElement("voice-description");
        ////        table.AppendChild(voiceDescriptionNode);
        ////        XmlNode headrowNode = objGlobal.PBPDocument.CreateElement("head-row");
        ////        table.AppendChild(headrowNode);
        ////        XmlNode RowNode = objGlobal.PBPDocument.CreateElement("Row");
        ////        table.AppendChild(RowNode);
        ////        XmlNode captionNode = objGlobal.PBPDocument.CreateElement("caption");
        ////        table.AppendChild(captionNode);

        ////        for (int i = 0; i < rows.Count; i++)
        ////        {
        ////            if (i == 0)
        ////            {
        ////                XmlNode headeRow = table.SelectSingleNode("//head-row");

        ////                for (int j = 0; j < rows[i].Count; j++)
        ////                {
        ////                    XmlNode headercol = objGlobal.PBPDocument.CreateElement("head-col");
        ////                    XmlAttribute headerWidth = objGlobal.PBPDocument.CreateAttribute("width");
        ////                    headercol.Attributes.Append(headerWidth);
        ////                    headercol.InnerText = rows[i][j];
        ////                    headeRow.AppendChild(headercol);
        ////                }
        ////            }
        ////            else
        ////            {
        ////                XmlNode lastRow = table.SelectNodes("//Row").Cast<XmlNode>().Last();
        ////                XmlNode rowNode = objGlobal.PBPDocument.CreateElement("Row");
        ////                for (int j = 0; j < rows[i].Count; j++)
        ////                {
        ////                    XmlNode column = objGlobal.PBPDocument.CreateElement("col");
        ////                    column.InnerText = rows[i][j];
        ////                    rowNode.AppendChild(column);
        ////                }
        ////                lastRow.ParentNode.InsertAfter(rowNode, lastRow);
        ////            }
        ////        }
        ////        table.SelectNodes("//Row").Cast<XmlNode>().First().ParentNode.RemoveChild(table.SelectNodes("//Row").Cast<XmlNode>().First());

        ////        string dirPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table";
        ////        string xmlDirPath = dirPath + "//TableXmls";
        ////        if (!File.Exists(xmlDirPath))
        ////            Directory.CreateDirectory(xmlDirPath);

        ////        string tableSavingPath = xmlDirPath + "//" + Path.GetFileNameWithoutExtension(excelFile.Name) + ".xml";
        ////        XmlDocument xmlDoc = new XmlDocument();
        ////        xmlDoc.LoadXml(table.OuterXml);
        ////        //xmlDoc.Save(tableSavingPath);

        ////        return table;
        ////    }
        ////    catch (Exception)
        ////    {
        ////        return null;
        ////    }
        ////}

        //private void MoveXmlFiles(int pageNum)
        //{
        //    string mainDirectoryPath = Common.GetDirectoryPath();
        //    string bookId = Convert.ToString(Session["MainBook"]);

        //    string directoryPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table\\";

        //    if (!Directory.Exists(directoryPath + "\\FinalTableTasks"))
        //        Directory.CreateDirectory(directoryPath + "\\FinalTableTasks");

        //    string destFilePath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table\\FinalTableTasks\\";
        //    string sourceFilePath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\TableTasks\\";

        //    var uploadedXslFiles = new DirectoryInfo(sourceFilePath).GetFiles()
        //        .Where(o => (o.Name.Split('_').Length < 1 ? false : o.Name.Split('_')[1].Equals(Convert.ToString(pageNum))))
        //        .Where(x => x.Name.Contains(".xlsx")).ToList();

        //    if (uploadedXslFiles != null)
        //    {
        //        foreach (var xlsxFile in uploadedXslFiles)
        //        {
        //            if (!File.Exists(destFilePath + "\\" + xlsxFile.Name) && File.Exists(xlsxFile.FullName))
        //                File.Copy(xlsxFile.FullName, destFilePath + "\\" + xlsxFile.Name);

        //            //List<List<String>> rows = ReadXlsxFile(xlsxFile.Name, "", sourceFilePath);
        //            //List<string> tableLines = GetTableLines(rows);
        //            //var tables = tblDetectionObj.SaveTablesInXml(tableLines, true);
        //        }
        //    }
        //}

        public List<string> GetTableLines(List<List<String>> rows)
        {
            List<string> tblLines = new List<string>();
            StringBuilder sbRowText = new StringBuilder();

            foreach (var row in rows)
            {
                foreach (var col in row)
                {
                    sbRowText.Append(col + " ");
                }
                tblLines.Add(Convert.ToString(sbRowText));
                sbRowText.Length = 0;
            }

            return tblLines.Where(x => !string.IsNullOrEmpty(x) && !string.IsNullOrWhiteSpace(x)).ToList();
        }

        public void CompleteTableTask(string userId, string bid)
        {
            string queryUpdate = "Update ACTIVITY Set Status='Approved',CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] +
                                    "' Where Task='Table' AND BID=" + bid;

            objMyDBClass.ExecuteCommand(queryUpdate);
        }

        public void CreateMistakeInjectionTask(string userId, string bid)
        {
            int inResult = objMyDBClass.CreateTask(bid, "Unassigned", "MistakeInjection", userId);
        }

        private List<XlsxFile> GetXlsxSortedPages(string[] uploadedXslFiles)
        {
            List<XlsxFile> tableXlsxFiles = new List<XlsxFile>();

            foreach (var file in uploadedXslFiles)
            {
                var fileNamesList = Path.GetFileNameWithoutExtension(file).Split('_').ToList();

                if (fileNamesList != null)
                {
                    if (fileNamesList.Count > 0)
                    {
                        tableXlsxFiles.Add(
                            new XlsxFile
                            {
                                PageNum = Convert.ToInt32(fileNamesList[1]),
                                TableNum = Convert.ToInt32(Path.GetFileNameWithoutExtension(fileNamesList[2])),
                                Extension = Path.GetExtension(file),
                                Name = Path.GetFileNameWithoutExtension(file)
                            });
                    }
                }
            }

            if (tableXlsxFiles != null)
            {
                if (tableXlsxFiles.Count > 0)
                {
                    return tableXlsxFiles.GroupBy(o => new { o.PageNum, o.TableNum })
                        .Select(o => o.FirstOrDefault()).OrderBy(x => x.PageNum)
                        .ToList();
                }
            }

            return null;
        }

        public XmlNode GetTablesFromXml(XlsxFile excelFile, string xslsFilesPath)
        {
            try
            {
                //List<List<String>> rows = ReadXlsxFile(excelFile.Name, excelFile.Extension, xslsFilesPath);
                List<List<String>> rows = ReadXmlFile(excelFile, xslsFilesPath);

                if (rows == null) return null;

                string bookId = Convert.ToString(Session["MainBook"]);
                string mainDirectoryPath = Common.GetDirectoryPath();

                //string xmlText = "<table id=\"0\" border=\"off\" head-row=\"on\"><tbody ispreviewpassed=\"false\" page=\"2\"><header/>" +
                //                 "<head-row></head-row><Row></Row><caption/></tbody></table>";

                XmlNode table = objGlobal.PBPDocument.CreateElement("table");
                XmlAttribute idAttr = objGlobal.PBPDocument.CreateAttribute("id");
                idAttr.Value = Convert.ToString(excelFile.TableNum);
                XmlAttribute pageAttr = objGlobal.PBPDocument.CreateAttribute("page");
                pageAttr.Value = Convert.ToString(excelFile.PageNum);
                XmlAttribute borderAttr = objGlobal.PBPDocument.CreateAttribute("border");
                borderAttr.Value = "off";
                XmlAttribute headrowAttr = objGlobal.PBPDocument.CreateAttribute("head-row");
                headrowAttr.Value = "off";
                table.Attributes.Append(idAttr);
                table.Attributes.Append(borderAttr);
                table.Attributes.Append(headrowAttr);

                XmlNode headerNode = objGlobal.PBPDocument.CreateElement("header");
                table.AppendChild(headerNode);
                XmlNode voiceDescriptionNode = objGlobal.PBPDocument.CreateElement("voice-description");
                table.AppendChild(voiceDescriptionNode);
                XmlNode headrowNode = objGlobal.PBPDocument.CreateElement("head-row");
                table.AppendChild(headrowNode);
                XmlNode RowNode = objGlobal.PBPDocument.CreateElement("Row");
                table.AppendChild(RowNode);
                XmlNode captionNode = objGlobal.PBPDocument.CreateElement("caption");
                table.AppendChild(captionNode);

                for (int i = 0; i < rows.Count; i++)
                {
                    if (i == 0)
                    {
                        XmlNode headeRow = table.SelectSingleNode("//head-row");

                        for (int j = 0; j < rows[i].Count; j++)
                        {
                            XmlNode headercol = objGlobal.PBPDocument.CreateElement("head-col");
                            XmlAttribute headerWidth = objGlobal.PBPDocument.CreateAttribute("width");
                            headercol.Attributes.Append(headerWidth);
                            headercol.InnerText = rows[i][j];
                            headeRow.AppendChild(headercol);
                        }
                    }
                    else
                    {
                        XmlNode lastRow = table.SelectNodes("//Row").Cast<XmlNode>().Last();
                        XmlNode rowNode = objGlobal.PBPDocument.CreateElement("Row");
                        for (int j = 0; j < rows[i].Count; j++)
                        {
                            XmlNode column = objGlobal.PBPDocument.CreateElement("col");
                            column.InnerText = rows[i][j];
                            rowNode.AppendChild(column);
                        }
                        lastRow.ParentNode.InsertAfter(rowNode, lastRow);
                    }
                }
                table.SelectNodes("//Row").Cast<XmlNode>().First().ParentNode.RemoveChild(table.SelectNodes("//Row").Cast<XmlNode>().First());

                string dirPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table";
                string xmlDirPath = dirPath + "//TableXmls";
                if (!File.Exists(xmlDirPath))
                    Directory.CreateDirectory(xmlDirPath);

                string tableSavingPath = xmlDirPath + "//" + Path.GetFileNameWithoutExtension(excelFile.Name) + ".xml";
                XmlDocument xmlDoc = new XmlDocument();

                xmlDoc.LoadXml(table.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">"));
                xmlDoc.Save(tableSavingPath);

                return table;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //old method and was commented
        public XmlNode GetXlsWithoutCoord(FileInfo excelFile, string xslsFilesPath, int tableNUm)
        {
            try
            {
                List<List<String>> rows = ReadXlsxFile(excelFile.Name, excelFile.Extension, xslsFilesPath);

                if (rows == null) return null;

                string bookId = Convert.ToString(Session["MainBook"]);
                string mainDirectoryPath = Common.GetDirectoryPath();

                //string xmlText = "<table id=\"0\" border=\"off\" head-row=\"on\"><tbody ispreviewpassed=\"false\" page=\"2\"><header/>" +
                //                 "<head-row></head-row><Row></Row><caption/></tbody></table>";

                XmlNode table = objGlobal.PBPDocument.CreateElement("table");
                XmlAttribute idAttr = objGlobal.PBPDocument.CreateAttribute("id");
                idAttr.Value = Convert.ToString(tableNUm);
                XmlAttribute borderAttr = objGlobal.PBPDocument.CreateAttribute("border");
                borderAttr.Value = "off";
                XmlAttribute headrowAttr = objGlobal.PBPDocument.CreateAttribute("head-row");
                headrowAttr.Value = "off";
                table.Attributes.Append(idAttr);
                table.Attributes.Append(borderAttr);
                table.Attributes.Append(headrowAttr);

                XmlNode headerNode = objGlobal.PBPDocument.CreateElement("header");
                table.AppendChild(headerNode);
                XmlNode voiceDescriptionNode = objGlobal.PBPDocument.CreateElement("voice-description");
                table.AppendChild(voiceDescriptionNode);
                XmlNode headrowNode = objGlobal.PBPDocument.CreateElement("head-row");
                table.AppendChild(headrowNode);
                XmlNode RowNode = objGlobal.PBPDocument.CreateElement("Row");
                table.AppendChild(RowNode);
                XmlNode captionNode = objGlobal.PBPDocument.CreateElement("caption");
                table.AppendChild(captionNode);

                for (int i = 0; i < rows.Count; i++)
                {
                    if (i == 0)
                    {
                        XmlNode headeRow = table.SelectSingleNode("//head-row");

                        for (int j = 0; j < rows[i].Count; j++)
                        {
                            XmlNode headercol = objGlobal.PBPDocument.CreateElement("head-col");
                            XmlAttribute headerWidth = objGlobal.PBPDocument.CreateAttribute("width");
                            headercol.Attributes.Append(headerWidth);
                            headercol.InnerText = rows[i][j];
                            headeRow.AppendChild(headercol);
                        }
                    }
                    else
                    {
                        XmlNode lastRow = table.SelectNodes("//Row").Cast<XmlNode>().Last();
                        XmlNode rowNode = objGlobal.PBPDocument.CreateElement("Row");
                        for (int j = 0; j < rows[i].Count; j++)
                        {
                            XmlNode column = objGlobal.PBPDocument.CreateElement("col");
                            column.InnerText = rows[i][j];
                            rowNode.AppendChild(column);
                        }
                        lastRow.ParentNode.InsertAfter(rowNode, lastRow);
                    }
                }
                table.SelectNodes("//Row").Cast<XmlNode>().First().ParentNode.RemoveChild(table.SelectNodes("//Row").Cast<XmlNode>().First());

                string dirPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table";
                string xmlDirPath = dirPath + "//TableXmls";
                if (!File.Exists(xmlDirPath))
                    Directory.CreateDirectory(xmlDirPath);

                string tableSavingPath = xmlDirPath + "//" + Path.GetFileNameWithoutExtension(excelFile.Name) + ".xml";
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(table.OuterXml);
                xmlDoc.Save(tableSavingPath);

                return table;
            }
            catch (Exception)
            {
                return null;
            }
        }

        ////old method
        public List<List<String>> ReadXlsxFile(string xlsFileName, string ext, string uploadedDirectory)
        {
            try
            {
                string fileName = uploadedDirectory + "\\" + xlsFileName + ext;
                FileInfo fileObj = new FileInfo(fileName);

                string conStr = string.Empty;
                List<List<String>> rows = null;
                List<string> row = null;

                using (ExcelPackage xlPackage = new ExcelPackage(fileObj))
                {
                    ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];

                    var rowCount = worksheet.Dimension.End.Row;
                    var columnCount = worksheet.Dimension.End.Column;

                    if (rowCount > 0 && columnCount > 0)
                    {
                        rows = new List<List<string>>();

                        for (int r = 1; r <= rowCount; r++)
                        {
                            row = new List<string>();

                            for (int c = 1; c <= columnCount; c++)
                            {
                                if (worksheet.Cells[r, c] != null)
                                    row.Add(Convert.ToString(worksheet.Cells[r, c].Value).Replace("&apos;", "'").Replace("&quot;", "\""));
                            }
                            rows.Add(row);
                        }
                    }
                }

                return rows;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        //public List<PdfTable> ReadXlsxFile(string xlsFileName, string ext, string uploadedDirectory)
        //{
        //    try
        //    {
        //        string fileName = uploadedDirectory + "\\" + xlsFileName + ext;
        //        FileInfo fileObj = new FileInfo(fileName);

        //        string conStr = string.Empty;
        //        List<PdfTable> rows = null;
        //        List<string> row = null;

        //        using (ExcelPackage xlPackage = new ExcelPackage(fileObj))
        //        {
        //            ExcelWorksheet worksheet = xlPackage.Workbook.Worksheets[1];

        //            var rowCount = worksheet.Dimension.End.Row;
        //            var columnCount = worksheet.Dimension.End.Column;

        //            var startingRow = worksheet.Dimension.Start.Row;

        //            List<TableLine> tblLines = new List<TableLine>();

        //            if (rowCount > 0 && columnCount > 0)
        //            {
        //                rows = new List<PdfTable>();

        //                //if (worksheet.MergedCells.Count > 0)
        //                //{
        //                //    tblLines.Add(new TableLine
        //                //    {
        //                //        Text = ""
        //                //    });
        //                //}

        //                for (int r = 1; r <= rowCount; r++)
        //                {
        //                    row = new List<string>();

        //                    for (int c = 1; c <= columnCount; c++)
        //                    {
        //                        if (worksheet.Cells[r, c] != null)
        //                            row.Add(Convert.ToString(worksheet.Cells[r, c].Value).Replace("&apos;", "'").Replace("&quot;", "\""));
        //                    }

        //                    tblLines.Add(new TableLine
        //                    {
        //                        Text = ""
        //                    });
        //                }
        //            }
        //        }

        //        return rows;
        //    }
        //    catch (Exception ex)
        //    {
        //        return null;
        //    }
        //}

        public List<List<String>> ReadXmlFile(XlsxFile xlsFile, string uploadedDirectory)
        {
            try
            {
                string conStr = string.Empty;
                List<List<String>> rows = null;
                List<string> row = null;

                string fileName = uploadedDirectory + "\\" + xlsFile.Name + ".xml";

                XmlDocument tableXml = new XmlDocument();
                tableXml.Load(fileName);

                var tableNodes = tableXml.SelectNodes("//Row");

                StringBuilder cellWords = new StringBuilder();

                rows = new List<List<string>>();

                foreach (XmlNode tblRow in tableNodes)
                {
                    row = new List<string>();

                    XmlNodeList cells = tblRow.ChildNodes;

                    foreach (XmlNode cell in cells)
                    {
                        var wordsList = cell.SelectNodes("descendant::Word");

                        cellWords.Clear();

                        foreach (XmlNode word in wordsList)
                        {
                            if (word.ChildNodes.Count > 1)
                            {
                                string lineSignature = LnSignatureBuild(word.ChildNodes[0], xlsFile.PageNum);

                                string columnLine = lineSignature + word.ChildNodes[1].InnerText + "</ln>";
                                //row.Add(Convert.ToString(cell.InnerXml));
                                cellWords.Append(columnLine);
                            }
                        }
                        row.Add(Convert.ToString(cellWords));
                    }
                    rows.Add(row);
                }

                //var finalTable = CombibeCellWordsByUry(rows);

                return rows;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<List<String>> CombibeCellWordsByUry(List<List<String>> tableRows)
        {
            try
            {
                string conStr = string.Empty;
                List<List<String>> rows = null;

                foreach (var row in tableRows)
                {
                    foreach (var col in row)
                    {
                        foreach (var cell in col)
                        {

                        }
                    }
                }

                return rows;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string LnSignatureBuild(XmlNode Line, int Page)
        {
            string newLn = "<ln coord=\"";
            newLn += Line.Attributes["llx"].Value + ":" + Line.Attributes["lly"].Value + ":"
                    + Line.Attributes["urx"].Value + ":" + Line.Attributes["ury"].Value + "\" page=\"";
            newLn += Page + "\" ";
            newLn += "height=\"" + Line.Attributes["height"].Value + "\" ";
            newLn += "left=\"" + Line.Attributes["llx"].Value + "\" ";
            newLn += "top=\"" + Line.Attributes["lly"].Value + "\" ";
            newLn += "font=\"" + Line.Attributes["font"].Value + "\" ";
            newLn += "fontsize=\"" + Line.Attributes["fontsize"].Value + "\" ";
            //newLn += "error=\"0\" ispreviewpassed=\"false\" isUserSigned=\"0\" isEditted=\"false\" ";
            //if (Line.Attributes["MergeError"] != null)
            //{
            //    newLn += "MergeError=\"1\" ";
            //}
            //if (Line.Attributes["SplitError"] != null)
            //{
            //    newLn += "SplitError=\"1\" ";
            //}
            //if (Line.Attributes["NextPage"] != null)
            //{
            //    newLn += "NextPage=\"1\" ";
            //}
            newLn += "fonttype=\"" + Line.Attributes["fonttype"].Value + "\" ";
            newLn = newLn.Trim();
            newLn += ">";// 
            return newLn;
        }

        public string LnSignatureBuildNew(XmlNode Line, int Page)
        {
            string newLn = "<ln coord=\"";
            newLn += Line.Attributes["llx"].Value + ":" + Line.Attributes["lly"].Value + ":"
                    + Line.Attributes["urx"].Value + ":" + Line.Attributes["ury"].Value + "\" page=\"";
            newLn += Page + "\" ";
            newLn += "height=\"" + Line.Attributes["height"].Value + "\" ";
            newLn += "left=\"" + Line.Attributes["llx"].Value + "\" ";
            newLn += "top=\"" + Line.Attributes["lly"].Value + "\" ";
            newLn += "font=\"" + Line.Attributes["font"].Value + "\" ";
            newLn += "fontsize=\"" + Line.Attributes["fontsize"].Value + "\" ";
            newLn += "fonttype=\"" + Line.Attributes["fonttype"].Value + "\" ";
            newLn = newLn.Trim();
            newLn += ">";// 
            return newLn;
        }

        public void ShowPdf(int page)
        {
            if ((Convert.ToInt32(Session["CurrentPage"])) == Convert.ToInt32(Session["TotalPages"]))
                btnFinishTask.Visible = true;

            //if ((Convert.ToInt32(Session["CurrentPage"])) > 1)
            //    divNavigationPanel.Style["display"] = "none";

            //else
            //    divNavigationPanel.Style["display"] = "block";

            string bookId = Convert.ToString(Session["MainBook"]);

            string mainDirectoryPath = Common.GetDirectoryPath();
            string pdfDirectoryPath = mainDirectoryPath + "\\" + bookId;
            //string pdfPath = pdfDirectoryPath + "\\DetectedTables\\" + bookId + "_actual.pdf";
            string pdfPath = pdfDirectoryPath + "\\DetectedTables\\" + bookId + ".pdf";
            //string highlightedPdfPath = pdfDirectoryPath + "\\DetectedTables\\" + bookId + "_actual_Highlighted.pdf";
            string highlightedPdfPath = pdfDirectoryPath + "\\DetectedTables\\" + bookId + "_Highlighted.pdf";
            //string manualHighlightedPdfPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\" + bookId + "_actual_Manual_Highlighted.pdf";
            string manualHighlightedPdfPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\" + bookId + "_Highlighted.pdf";

            string pdfPreviewDirPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\PdfPreview";
            string outputPdfPath = pdfPreviewDirPath + "\\" + page + ".pdf";
            string outputHighlightedPdfPath = pdfPreviewDirPath + "\\" + page + "_Highlighted.pdf";
            string outputManualHighlightedPdfPath = pdfPreviewDirPath + "\\" + page + "_Manual_Highlighted.pdf";

            //if (!string.IsNullOrEmpty(Convert.ToString(Session["isIgnorAlgoChecked"])))
            //{
            //    if (Convert.ToString(Session["isIgnorAlgoChecked"]).ToLower().Equals("false"))
            //    {
            //        hfIsIgnoreAlgo.Value = "false";
            //        //cbxIgnoreAlgo.Checked = false;
            //    }
            //}

            if (!string.IsNullOrEmpty(Convert.ToString(Session["isIgnorAlgoChecked"])))
                hfIsIgnoreAlgo.Value = Convert.ToString(Session["isIgnorAlgoChecked"]);

            if (hfIsIgnoreAlgo.Value.Equals("true"))
            {
                //tblDetectionObj.ExtractPage(pdfPath, outputPdfPath, page);
                //Session["pdfPath"] = outputPdfPath;

                if (!File.Exists(outputManualHighlightedPdfPath))
                {
                    tblDetectionObj.ExtractPage(pdfPath, outputPdfPath, page);
                    Session["pdfPath"] = outputPdfPath;
                }
                else
                {
                    tblDetectionObj.ExtractPage(manualHighlightedPdfPath, outputManualHighlightedPdfPath, page);
                    Session["pdfPath"] = outputManualHighlightedPdfPath;
                }
            }
            else
            {
                //if (File.Exists(manualHighlightedPdfPath) && Convert.ToString(Session["isNextClicked"]).Equals("False"))
                //{
                //    tblDetectionObj.ExtractPage(manualHighlightedPdfPath, outputManualHighlightedPdfPath, page);
                //    Session["pdfPath"] = outputManualHighlightedPdfPath;
                //}
                //else
                //{
                //    tblDetectionObj.ExtractPage(highlightedPdfPath, outputHighlightedPdfPath, page);
                //    Session["pdfPath"] = outputHighlightedPdfPath;
                //}

                if (!File.Exists(outputManualHighlightedPdfPath))
                {
                    tblDetectionObj.ExtractPage(highlightedPdfPath, outputHighlightedPdfPath, page);
                    Session["pdfPath"] = outputHighlightedPdfPath;
                }
                else
                {
                    tblDetectionObj.ExtractPage(manualHighlightedPdfPath, outputManualHighlightedPdfPath, page);
                    Session["pdfPath"] = outputManualHighlightedPdfPath;
                }
            }
        }

        //protected void btnMarkTableLines_Click(object sender, EventArgs e)
        //{
        //    List<string> tableContainingPages = (List<string>)Session["TablePages"];
        //    int currentPage = Convert.ToInt32(Session["CurrentPage"]);
        //    string pageNum = tableContainingPages.ElementAt(currentPage - 1);

        //    ShowPdf(pageNum);
        //}

        protected void btnConfirmTblParameters_Click(object sender, EventArgs e)
        {
            List<string> tableContainingPages = (List<string>)Session["TablePages"];
            int currentPage = Convert.ToInt32(Session["CurrentPage"]);
            int pageNum = Convert.ToInt32(tableContainingPages.ElementAt(currentPage - 1));

            ShowPdf(pageNum);
        }

        public XmlNode ConvertToFinalXml(XmlNode tableNode, string finalTableHtml, int page, int tableId, int rbtnTableBorderIndex, int rbtnHeadRowIndex)
        {
            string tableBorderStatus = rbtnTableBorderIndex == 0 ? "on" : "off";
            string headerRowStatus = rbtnHeadRowIndex == 0 ? "on" : "off";

            string bookId = Convert.ToString(Session["MainBook"]);
            string mainDirectoryPath = Common.GetDirectoryPath();

            //string xmlText = "<table id=\"0\" border=\"off\" head-row=\"on\"><tbody ispreviewpassed=\"false\" page=\"2\"><header/>" +
            //                 "<head-row></head-row><Row></Row><caption/></tbody></table>";

            List<string> colWidthList = new List<string>();
            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(finalTableHtml);
            int maxColumns = doc.DocumentNode.SelectNodes("//tr").Cast<HtmlNode>().Select(x => x.ChildNodes.Count).Max();

            var tdList = doc.DocumentNode.SelectNodes("//tr").Where(c => c.ChildNodes.Count == maxColumns).Take(1).ToList();
            if (tdList.Count > 0)
            {
                foreach (HtmlNode td in tdList[0].ChildNodes)
                {
                    if (td.Attributes["style"] != null)
                        colWidthList.Add(td.Attributes["style"].Value.Split(':')[2].Replace("%;", "").Trim());
                }
            }

            XmlDocument xmlDoc = new XmlDocument();
            XmlNode finalTable = xmlDoc.CreateElement("table");
            XmlAttribute idAttr = xmlDoc.CreateAttribute("id");
            idAttr.Value = Convert.ToString(tableId);
            XmlAttribute borderAttr = xmlDoc.CreateAttribute("border");
            borderAttr.Value = tableBorderStatus;
            XmlAttribute headrowAttr = xmlDoc.CreateAttribute("head-row");
            headrowAttr.Value = headerRowStatus;
            XmlAttribute pageAttr = xmlDoc.CreateAttribute("page");
            pageAttr.Value = Convert.ToString(page);

            finalTable.Attributes.Append(idAttr);
            finalTable.Attributes.Append(borderAttr);
            finalTable.Attributes.Append(headrowAttr);
            finalTable.Attributes.Append(pageAttr);

            XmlNode headerNode = xmlDoc.CreateElement("header");
            finalTable.AppendChild(headerNode);
            XmlNode headrowNode = xmlDoc.CreateElement("head-row");
            finalTable.AppendChild(headrowNode);
            XmlNode RowNode = xmlDoc.CreateElement("Row");
            finalTable.AppendChild(RowNode);
            XmlNode captionNode = xmlDoc.CreateElement("caption");
            finalTable.AppendChild(captionNode);
            XmlNode voiceDescriptionNode = xmlDoc.CreateElement("voice-description");
            finalTable.AppendChild(voiceDescriptionNode);

            XmlNode tableHeader = finalTable.SelectSingleNode("//header");
            XmlNode tableHeadercol = xmlDoc.CreateElement("col");

            XmlNode tableCaption = finalTable.SelectSingleNode("//caption");
            XmlNode tableCaptioncol = xmlDoc.CreateElement("col");

            //tableCaptioncol.InnerText = divCaptionRow.InnerText;
            //tableCaption.AppendChild(tableCaptioncol);

            StringBuilder colText = new StringBuilder();
            int tblHeaderRowCount = 0;

            var taleHeaderLines = tableNode.Cast<XmlNode>().TakeWhile(x => x.ChildNodes.Count == 1).ToList();

            tblHeaderRowCount = taleHeaderLines.Count;

            var colLine = GetRowLinesWithCoord(taleHeaderLines, page);
            tableHeadercol.InnerText = colLine.Trim();
            tableHeader.AppendChild(tableHeadercol);

            if (!string.IsNullOrEmpty(divCaptionRow.InnerText))
            {
                var taleCaptionLines = tableNode.Cast<XmlNode>().Skip(tblHeaderRowCount).Where(x => x.ChildNodes.Count == 1).ToList();
                var colCapLine = GetRowLinesWithCoord(taleCaptionLines, page);
                tableCaptioncol.InnerText = colCapLine.Trim();
                tableCaption.AppendChild(tableCaptioncol);
            }

            colText.Length = 0;

            var rows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Where(x => x.ChildNodes.Count > 1).ToList();
            int colIndex = 0;

            for (int i = 0; i < rows.Count; i++)
            {
                if (i == 0 && headerRowStatus.Equals("off"))
                {
                    XmlNode headeRow = finalTable.SelectSingleNode("//head-row");
                    XmlNode headercol = null;
                    XmlAttribute headerWidth = null;

                    if (colWidthList.Count > 0 && colIndex < colWidthList.Count)
                    {
                        foreach (string colWidth in colWidthList)
                        {
                            headercol = xmlDoc.CreateElement("head-col");
                            headerWidth = xmlDoc.CreateAttribute("width");

                            headerWidth.Value = colWidth;
                            headercol.Attributes.Append(headerWidth);
                            headercol.InnerText = "";
                            headeRow.AppendChild(headercol);
                        }
                    }
                }
                else if (i == 0 && headerRowStatus.Equals("on"))
                {
                    XmlNode headeRow = finalTable.SelectSingleNode("//head-row");
                    headeRow = CombineTableLine(xmlDoc, rows[i], headeRow, page, true);
                }
                else
                {
                    XmlNode lastRow = finalTable.SelectNodes("//Row").Cast<XmlNode>().Last();
                    XmlNode rowNode = xmlDoc.CreateElement("Row");

                    rowNode = CombineTableLine(xmlDoc, rows[i], rowNode, page, false);
                    lastRow.ParentNode.InsertAfter(rowNode, lastRow);
                }
            }
            finalTable.SelectNodes("//Row").Cast<XmlNode>().First().ParentNode.RemoveChild(finalTable.SelectNodes("//Row").Cast<XmlNode>().First());

            string dirPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table";
            string xmlDirPath = dirPath + "//TableXmls";

            if (!File.Exists(xmlDirPath))
                Directory.CreateDirectory(xmlDirPath);

            string tableSavingPath = xmlDirPath + "//" + "Table_" + page + "_" + tableId + ".xml";
            //xmlDoc.InnerXml = finalTable.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");

            try
            {
                xmlDoc.InnerXml = finalTable.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");

                if (File.Exists(tableSavingPath))
                    File.Delete(tableSavingPath);
            }
            catch (Exception)
            {

            }

            XmlNode finalUpdatedXml = GetUpdatedXml(finalTable, finalTableHtml, xmlDoc, page);

            if (finalUpdatedXml == null)
            {
                xmlDoc.InnerXml = finalTable.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");
                xmlDoc.Save(tableSavingPath);
            }
            else
            {
                xmlDoc.InnerXml = finalUpdatedXml.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");
                xmlDoc.Save(tableSavingPath);
            }

            return xmlDoc;
        }

        //public XmlNode ConvertToFinalXml(XmlNode tableNode, string finalTableHtml, int page, int tableId, int rbtnTableBorderIndex, int rbtnHeadRowIndex)
        //{
        //    string tableBorderStatus = rbtnTableBorderIndex == 0 ? "on" : "off";
        //    string headerRowStatus = rbtnHeadRowIndex == 0 ? "on" : "off";

        //    string bookId = Convert.ToString(Session["MainBook"]);
        //    string mainDirectoryPath = Common.GetDirectoryPath();

        //    //string xmlText = "<table id=\"0\" border=\"off\" head-row=\"on\"><tbody ispreviewpassed=\"false\" page=\"2\"><header/>" +
        //    //                 "<head-row></head-row><Row></Row><caption/></tbody></table>";

        //    List<string> colWidthList = new List<string>();
        //    HtmlDocument doc = new HtmlDocument();
        //    doc.LoadHtml(finalTableHtml);
        //    int maxColumns = doc.DocumentNode.SelectNodes("//tr").Cast<HtmlNode>().Select(x => x.ChildNodes.Count).Max();

        //    //var tdStyleList = doc.DocumentNode.SelectNodes("//tr").Where(c => c.ChildNodes.Count == maxColumns).Take(1)
        //    //                                 .Select(x => x.ChildNodes[0].Attributes["style"] != null ? x.ChildNodes[0].Attributes["style"].Value : "")
        //    //                                 .ToList();

        //    var tdList = doc.DocumentNode.SelectNodes("//tr").Where(c => c.ChildNodes.Count == maxColumns).Take(1).ToList();
        //    if (tdList.Count > 0)
        //    {
        //        foreach (HtmlNode td in tdList[0].ChildNodes)
        //        {
        //            if (td.Attributes["style"] != null)
        //                colWidthList.Add(td.Attributes["style"].Value.Split(':')[2].Replace("%;", "").Trim());
        //        }
        //    }

        //    XmlDocument xmlDoc = new XmlDocument();
        //    XmlNode finalTable = xmlDoc.CreateElement("table");
        //    XmlAttribute idAttr = xmlDoc.CreateAttribute("id");
        //    idAttr.Value = Convert.ToString(tableId);
        //    XmlAttribute borderAttr = xmlDoc.CreateAttribute("border");
        //    borderAttr.Value = tableBorderStatus;
        //    XmlAttribute headrowAttr = xmlDoc.CreateAttribute("head-row");
        //    headrowAttr.Value = headerRowStatus;
        //    XmlAttribute pageAttr = xmlDoc.CreateAttribute("page");
        //    pageAttr.Value = Convert.ToString(page);

        //    finalTable.Attributes.Append(idAttr);
        //    finalTable.Attributes.Append(borderAttr);
        //    finalTable.Attributes.Append(headrowAttr);
        //    finalTable.Attributes.Append(pageAttr);

        //    XmlNode headerNode = xmlDoc.CreateElement("header");
        //    finalTable.AppendChild(headerNode);
        //    XmlNode headrowNode = xmlDoc.CreateElement("head-row");
        //    finalTable.AppendChild(headrowNode);
        //    XmlNode RowNode = xmlDoc.CreateElement("Row");
        //    finalTable.AppendChild(RowNode);
        //    XmlNode captionNode = xmlDoc.CreateElement("caption");
        //    finalTable.AppendChild(captionNode);
        //    XmlNode voiceDescriptionNode = xmlDoc.CreateElement("voice-description");
        //    finalTable.AppendChild(voiceDescriptionNode);

        //    XmlNode tableHeader = finalTable.SelectSingleNode("//header");
        //    XmlNode tableHeadercol = xmlDoc.CreateElement("col");

        //    XmlNode tableCaption = finalTable.SelectSingleNode("//caption");
        //    XmlNode tableCaptioncol = xmlDoc.CreateElement("col");

        //    //tableCaptioncol.InnerText = divCaptionRow.InnerText;
        //    //tableCaption.AppendChild(tableCaptioncol);

        //    StringBuilder colText = new StringBuilder();
        //    int tblHeaderRowCount = 0;

        //    var taleHeaderLines = tableNode.Cast<XmlNode>().TakeWhile(x => x.ChildNodes.Count == 1).ToList();

        //    tblHeaderRowCount = taleHeaderLines.Count;

        //    var colLine = GetRowLinesWithCoord(taleHeaderLines, page);
        //    tableHeadercol.InnerText = colLine.Trim();
        //    tableHeader.AppendChild(tableHeadercol);

        //    if (!string.IsNullOrEmpty(divCaptionRow.InnerText))
        //    {
        //        var taleCaptionLines = tableNode.Cast<XmlNode>().Skip(tblHeaderRowCount).Where(x => x.ChildNodes.Count == 1).ToList();
        //        var colCapLine = GetRowLinesWithCoord(taleCaptionLines, page);
        //        tableCaptioncol.InnerText = colCapLine.Trim();
        //        tableCaption.AppendChild(tableCaptioncol);
        //    }

        //    colText.Length = 0;

        //    double llx = 0;
        //    double lly = 0;
        //    double urx = 0;
        //    double ury = 0;
        //    string coord = string.Empty;
        //    string font = string.Empty;
        //    string fontSize = string.Empty;
        //    string fontType = string.Empty;
        //    string top = string.Empty;
        //    string height = string.Empty;
        //    string left = string.Empty;
        //    StringBuilder lineText = new StringBuilder();

        //    var rows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Where(x => x.ChildNodes.Count > 1).ToList();
        //    int colIndex = 0;

        //    for (int i = 0; i < rows.Count; i++)
        //    {
        //        if (i == 0 && headerRowStatus.Equals("on"))
        //        {
        //            XmlNode headeRow = finalTable.SelectSingleNode("//head-row");

        //            var columnsList = rows[i].SelectNodes("descendant::Cell").Cast<XmlNode>().ToList();
        //            foreach (XmlNode col in columnsList)
        //            {
        //                List<double> llyList = col.SelectNodes("descendant::Word").Cast<XmlNode>().Where(x => (x.ChildNodes != null &&
        //                                        x.ChildNodes[0].Attributes["lly"] != null)).
        //                                        Select(y => Convert.ToDouble(y.ChildNodes[0].Attributes["lly"].Value)).Distinct().ToList();

        //                foreach (double llyCoord in llyList)
        //                {
        //                    lineText.Length = 0;
        //                    llx = 0;
        //                    lly = 0;
        //                    urx = 0;
        //                    ury = 0;
        //                    coord = string.Empty;
        //                    font = string.Empty;
        //                    fontSize = string.Empty;
        //                    fontType = string.Empty;
        //                    top = string.Empty;
        //                    height = string.Empty;
        //                    left = string.Empty;

        //                    var colWordList = col.SelectNodes("descendant::Word").Cast<XmlNode>().Where(x => (x.ChildNodes != null &&
        //                                        x.ChildNodes[0].Attributes["lly"] != null && Convert.ToDouble(x.ChildNodes[0].Attributes["lly"].Value).Equals(Convert.ToDouble(llyCoord)))).ToList();

        //                    for (int j = 0; j < colWordList.Count; j++)
        //                    {
        //                        if (colWordList[j].ChildNodes.Count > 1)
        //                        {
        //                            if (j == 0)
        //                            {
        //                                llx = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["llx"].Value);
        //                                lly = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["lly"].Value);
        //                                ury = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["ury"].Value);

        //                                top = colWordList[j].ChildNodes[0].Attributes["lly"].Value;
        //                                left = colWordList[j].ChildNodes[0].Attributes["llx"].Value;

        //                                font = colWordList[j].ChildNodes[0].Attributes["font"] != null ? colWordList[j].ChildNodes[0].Attributes["font"].Value : "emptyFont";
        //                                fontSize = colWordList[j].ChildNodes[0].Attributes["fontsize"] != null ? colWordList[j].ChildNodes[0].Attributes["fontsize"].Value : "emptyFontSize";

        //                                fontType = colWordList[j].ChildNodes[0].Attributes["fonttype"] != null ? colWordList[j].ChildNodes[0].Attributes["fonttype"].Value : "Embeded";

        //                                height = colWordList[j].ChildNodes[0].Attributes["height"] != null ? colWordList[j].ChildNodes[0].Attributes["height"].Value : "emptyHeight";
        //                            }

        //                            if (j == colWordList.Count - 1)
        //                                urx = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["urx"].Value);

        //                            if (!string.IsNullOrEmpty(colWordList[j].ChildNodes[1].InnerText.Trim()))
        //                            {
        //                                lineText.Append(colWordList[j].ChildNodes[1].InnerText.Trim() + " ");
        //                            }
        //                        }
        //                    }

        //                    coord = llx + ":" + lly + ":" + urx + ":" + ury;
        //                    string newLn = "<ln coord=\"" + coord + "\" " + "page=\"" + page + "\" height=\"" + height +
        //                                   "\" left=\"" + left + "\" top=\"" +
        //                                   top + "\" font=\"" + font + "\" fontsize=\"" + fontSize + "\" fonttype=\"" +
        //                                   fontType + "\" >";

        //                    string columnLine = newLn + Convert.ToString(lineText) + "</ln>";
        //                    colText.Append(columnLine);
        //                }
        //                XmlNode headercol = xmlDoc.CreateElement("head-col");
        //                XmlAttribute headerWidth = xmlDoc.CreateAttribute("width");

        //                if (colWidthList.Count > 0 && colIndex < colWidthList.Count)
        //                {
        //                    headerWidth.Value = colWidthList[colIndex];
        //                    colIndex++;
        //                    headercol.Attributes.Append(headerWidth);
        //                    headercol.InnerText = Convert.ToString(colText).Trim();
        //                    headeRow.AppendChild(headercol);
        //                    colText.Length = 0;
        //                }
        //            }
        //        }
        //        else if (i == 0 && headerRowStatus.Equals("off"))
        //        {
        //            XmlNode headeRow = finalTable.SelectSingleNode("//head-row");

        //            var columnsList = rows[i].SelectNodes("descendant::Cell").Cast<XmlNode>().ToList();
        //            foreach (XmlNode col in columnsList)
        //            {
        //                List<double> llyList = col.SelectNodes("descendant::Word").Cast<XmlNode>().Where(x => (x.ChildNodes != null &&
        //                                        x.ChildNodes[0].Attributes["lly"] != null)).
        //                                        Select(y => Convert.ToDouble(y.ChildNodes[0].Attributes["lly"].Value)).Distinct().ToList();

        //                foreach (double llyCoord in llyList)
        //                {
        //                    lineText.Length = 0;
        //                    llx = 0;
        //                    lly = 0;
        //                    urx = 0;
        //                    ury = 0;
        //                    coord = string.Empty;
        //                    font = string.Empty;
        //                    fontSize = string.Empty;
        //                    fontType = string.Empty;
        //                    top = string.Empty;
        //                    height = string.Empty;
        //                    left = string.Empty;

        //                    var colWordList = col.SelectNodes("descendant::Word").Cast<XmlNode>().Where(x => (x.ChildNodes != null &&
        //                                        x.ChildNodes[0].Attributes["lly"] != null && Convert.ToDouble(x.ChildNodes[0].Attributes["lly"].Value).Equals(Convert.ToDouble(llyCoord)))).ToList();

        //                    for (int j = 0; j < colWordList.Count; j++)
        //                    {
        //                        if (colWordList[j].ChildNodes.Count > 1)
        //                        {
        //                            if (j == 0)
        //                            {
        //                                llx = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["llx"].Value);
        //                                lly = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["lly"].Value);
        //                                ury = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["ury"].Value);

        //                                top = colWordList[j].ChildNodes[0].Attributes["lly"].Value;
        //                                left = colWordList[j].ChildNodes[0].Attributes["llx"].Value;

        //                                font = colWordList[j].ChildNodes[0].Attributes["font"] != null ? colWordList[j].ChildNodes[0].Attributes["font"].Value : "emptyFont";
        //                                fontSize = colWordList[j].ChildNodes[0].Attributes["fontsize"] != null ? colWordList[j].ChildNodes[0].Attributes["fontsize"].Value : "emptyFontSize";

        //                                fontType = colWordList[j].ChildNodes[0].Attributes["fonttype"] != null ? colWordList[j].ChildNodes[0].Attributes["fonttype"].Value : "Embeded";

        //                                height = colWordList[j].ChildNodes[0].Attributes["height"] != null ? colWordList[j].ChildNodes[0].Attributes["height"].Value : "emptyHeight";
        //                            }

        //                            if (j == colWordList.Count - 1)
        //                                urx = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["urx"].Value);

        //                            if (!string.IsNullOrEmpty(colWordList[j].ChildNodes[1].InnerText.Trim()))
        //                            {
        //                                lineText.Append(colWordList[j].ChildNodes[1].InnerText.Trim() + " ");
        //                            }
        //                        }
        //                    }

        //                    coord = llx + ":" + lly + ":" + urx + ":" + ury;
        //                    string newLn = "<ln coord=\"" + coord + "\" " + "page=\"" + page + "\" height=\"" + height +
        //                                   "\" left=\"" + left + "\" top=\"" +
        //                                   top + "\" font=\"" + font + "\" fontsize=\"" + fontSize + "\" fonttype=\"" +
        //                                   fontType + "\" >";

        //                    string columnLine = newLn + Convert.ToString(lineText) + "</ln>";
        //                    colText.Append(columnLine);
        //                }
        //                XmlNode headercol = xmlDoc.CreateElement("head-col");
        //                XmlAttribute headerWidth = xmlDoc.CreateAttribute("width");

        //                if (colWidthList.Count > 0 && colIndex < colWidthList.Count)
        //                {
        //                    headerWidth.Value = colWidthList[colIndex];
        //                    colIndex++;
        //                    headercol.Attributes.Append(headerWidth);
        //                    headercol.InnerText = Convert.ToString(colText).Trim();
        //                    headeRow.AppendChild(headercol);
        //                    colText.Length = 0;
        //                }
        //            }
        //        }
        //        else
        //        {
        //            XmlNode lastRow = finalTable.SelectNodes("//Row").Cast<XmlNode>().Last();
        //            XmlNode rowNode = xmlDoc.CreateElement("Row");

        //            rowNode = CombineTableLine(xmlDoc, rows[i], rowNode, page);
        //            lastRow.ParentNode.InsertAfter(rowNode, lastRow);

        //            //var columnsList = rows[i].SelectNodes("descendant::Cell").Cast<XmlNode>().ToList();
        //            //foreach (XmlNode col in columnsList)
        //            //{
        //            //    List<double> llyList = col.SelectNodes("descendant::Word").Cast<XmlNode>().Where(x => (x.ChildNodes != null &&
        //            //                            x.ChildNodes[0].Attributes["lly"] != null)).
        //            //                            Select(y => Convert.ToDouble(y.ChildNodes[0].Attributes["lly"].Value)).Distinct().ToList();

        //            //    foreach (double llyCoord in llyList)
        //            //    {
        //            //        lineText.Length = 0;
        //            //        llx = 0;
        //            //        lly = 0;
        //            //        urx = 0;
        //            //        ury = 0;
        //            //        coord = string.Empty;
        //            //        font = string.Empty;
        //            //        fontSize = string.Empty;
        //            //        fontType = string.Empty;
        //            //        top = string.Empty;
        //            //        height = string.Empty;
        //            //        left = string.Empty;

        //            //        var colWordList = col.SelectNodes("descendant::Word").Cast<XmlNode>().Where(x => (x.ChildNodes != null &&
        //            //                            x.ChildNodes[0].Attributes["lly"] != null && Convert.ToDouble(x.ChildNodes[0].Attributes["lly"].Value).Equals(Convert.ToDouble(llyCoord)))).ToList();

        //            //        for (int j = 0; j < colWordList.Count; j++)
        //            //        {
        //            //            if (colWordList[j].ChildNodes.Count > 1)
        //            //            {
        //            //                if (j == 0)
        //            //                {
        //            //                    llx = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["llx"].Value);
        //            //                    lly = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["lly"].Value);
        //            //                    ury = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["ury"].Value);

        //            //                    top = colWordList[j].ChildNodes[0].Attributes["lly"].Value;
        //            //                    left = colWordList[j].ChildNodes[0].Attributes["llx"].Value;

        //            //                    font = colWordList[j].ChildNodes[0].Attributes["font"] != null ? colWordList[j].ChildNodes[0].Attributes["font"].Value : "emptyFont";
        //            //                    fontSize = colWordList[j].ChildNodes[0].Attributes["fontsize"] != null ? colWordList[j].ChildNodes[0].Attributes["fontsize"].Value : "emptySize";

        //            //                    fontType = colWordList[j].ChildNodes[0].Attributes["fonttype"] != null ? colWordList[j].ChildNodes[0].Attributes["fonttype"].Value : "Embeded";

        //            //                    height = colWordList[j].ChildNodes[0].Attributes["height"] != null ? colWordList[j].ChildNodes[0].Attributes["height"].Value : "emptyHeight";
        //            //                }

        //            //                if (j == colWordList.Count - 1)
        //            //                    urx = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["urx"].Value);

        //            //                if (!string.IsNullOrEmpty(colWordList[j].ChildNodes[1].InnerText.Trim()))
        //            //                {
        //            //                    lineText.Append(colWordList[j].ChildNodes[1].InnerText.Trim() + " ");
        //            //                }
        //            //            }
        //            //        }

        //            //        coord = llx + ":" + lly + ":" + urx + ":" + ury;
        //            //        string newLn = "<ln coord=\"" + coord + "\" " + "page=\"" + page + "\" height=\"" + height +
        //            //                       "\" left=\"" + left + "\" top=\"" +
        //            //                       top + "\" font=\"" + font + "\" fontsize=\"" + fontSize + "\" fonttype=\"" +
        //            //                       fontType + "\" >";

        //            //        string columnLine = newLn + Convert.ToString(lineText) + "</ln>";
        //            //        colText.Append(columnLine);
        //            //    }
        //            //    XmlNode column = xmlDoc.CreateElement("col");
        //            //    column.InnerText = Convert.ToString(colText).Trim();
        //            //    rowNode.AppendChild(column);
        //            //    colText.Length = 0;
        //            //}
        //            //lastRow.ParentNode.InsertAfter(rowNode, lastRow);
        //        }
        //    }
        //    finalTable.SelectNodes("//Row").Cast<XmlNode>().First().ParentNode.RemoveChild(finalTable.SelectNodes("//Row").Cast<XmlNode>().First());

        //    string dirPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table";
        //    string xmlDirPath = dirPath + "//TableXmls";

        //    if (!File.Exists(xmlDirPath))
        //        Directory.CreateDirectory(xmlDirPath);

        //    string tableSavingPath = xmlDirPath + "//" + "Table_" + page + "_" + tableId + ".xml";
        //    //xmlDoc.InnerXml = finalTable.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");

        //    try
        //    {
        //        if (File.Exists(tableSavingPath))
        //            File.Delete(tableSavingPath);
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    XmlNode finalUpdatedXml = UpdateXmlByNewRowCol(finalTable, finalTableHtml, xmlDoc, page);

        //    if (finalUpdatedXml == null)
        //    {
        //        xmlDoc.InnerXml = finalTable.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");
        //        xmlDoc.Save(tableSavingPath);
        //    }
        //    else
        //    {
        //        xmlDoc.InnerXml = finalUpdatedXml.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");
        //        xmlDoc.Save(tableSavingPath);
        //    }

        //    return xmlDoc;
        //}

        public XmlNode CombineTableLine(XmlDocument xmlDoc, XmlNode rows, XmlNode newRowNode, int page, bool isHeadRow)
        {
            double llx = 0;
            double lly = 0;
            double urx = 0;
            double ury = 0;
            string coord = string.Empty;
            string font = string.Empty;
            string fontSize = string.Empty;
            string fontType = string.Empty;
            string top = string.Empty;
            string height = string.Empty;
            string left = string.Empty;
            StringBuilder lineText = new StringBuilder();
            StringBuilder colText = new StringBuilder();
            string columnName = string.Empty;

            var columnsList = rows.SelectNodes("descendant::Cell").Cast<XmlNode>().ToList();
            foreach (XmlNode col in columnsList)
            {
                List<double> llyList = col.SelectNodes("descendant::Word").Cast<XmlNode>().Where(x => (x.ChildNodes != null &&
                                        x.ChildNodes[0].Attributes["lly"] != null)).
                                        Select(y => Convert.ToDouble(y.ChildNodes[0].Attributes["lly"].Value)).Distinct().ToList();

                foreach (double llyCoord in llyList)
                {
                    lineText.Length = 0;
                    llx = 0;
                    lly = 0;
                    urx = 0;
                    ury = 0;
                    coord = string.Empty;
                    font = string.Empty;
                    fontSize = string.Empty;
                    fontType = string.Empty;
                    top = string.Empty;
                    height = string.Empty;
                    left = string.Empty;

                    var colWordList = col.SelectNodes("descendant::Word").Cast<XmlNode>().Where(x => (x.ChildNodes != null &&
                                        x.ChildNodes[0].Attributes["lly"] != null && Convert.ToDouble(x.ChildNodes[0].Attributes["lly"].Value).Equals(Convert.ToDouble(llyCoord)))).ToList();

                    for (int j = 0; j < colWordList.Count; j++)
                    {
                        if (colWordList[j].ChildNodes.Count > 1)
                        {
                            if (j == 0)
                            {
                                llx = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["llx"].Value);
                                lly = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["lly"].Value);
                                ury = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["ury"].Value);

                                top = colWordList[j].ChildNodes[0].Attributes["lly"].Value;
                                left = colWordList[j].ChildNodes[0].Attributes["llx"].Value;

                                font = colWordList[j].ChildNodes[0].Attributes["font"] != null ? colWordList[j].ChildNodes[0].Attributes["font"].Value : "emptyFont";
                                fontSize = colWordList[j].ChildNodes[0].Attributes["fontsize"] != null ? colWordList[j].ChildNodes[0].Attributes["fontsize"].Value : "emptySize";

                                fontType = colWordList[j].ChildNodes[0].Attributes["fonttype"] != null ? colWordList[j].ChildNodes[0].Attributes["fonttype"].Value : "Embeded";

                                height = colWordList[j].ChildNodes[0].Attributes["height"] != null ? colWordList[j].ChildNodes[0].Attributes["height"].Value : "emptyHeight";
                            }

                            if (j == colWordList.Count - 1)
                                urx = Convert.ToDouble(colWordList[j].ChildNodes[0].Attributes["urx"].Value);

                            if (!string.IsNullOrEmpty(colWordList[j].ChildNodes[1].InnerText.Trim()))
                            {
                                lineText.Append(colWordList[j].ChildNodes[1].InnerText.Trim() + " ");
                            }
                        }
                    }

                    coord = llx + ":" + lly + ":" + urx + ":" + ury;
                    string newLn = "<ln coord=\"" + coord + "\" " + "page=\"" + page + "\" height=\"" + height +
                                   "\" left=\"" + left + "\" top=\"" +
                                   top + "\" font=\"" + font + "\" fontsize=\"" + fontSize + "\" fonttype=\"" +
                                   fontType + "\" >";

                    string columnLine = newLn + Convert.ToString(lineText) + "</ln>";
                    colText.Append(columnLine);
                }

                if (isHeadRow)
                    columnName = "head-col";
                else
                    columnName = "col";

                XmlNode column = xmlDoc.CreateElement(columnName);
                column.InnerText = Convert.ToString(colText).Trim();
                newRowNode.AppendChild(column);
                colText.Length = 0;
            }
            return newRowNode;
        }

        public string GetRowLinesWithCoord(List<XmlNode> taleHeaderLines, int page)
        {
            double llx = 0;
            double lly = 0;
            double urx = 0;
            double ury = 0;
            string coord = string.Empty;
            string font = string.Empty;
            string fontSize = string.Empty;
            string fontType = string.Empty;
            string top = string.Empty;
            string height = string.Empty;
            string left = string.Empty;
            StringBuilder lineText = new StringBuilder();
            StringBuilder colText = new StringBuilder();

            foreach (XmlNode row in taleHeaderLines)
            {
                var columnsList = row.SelectNodes("descendant::Cell").Cast<XmlNode>().ToList();
                foreach (XmlNode col in columnsList)
                {
                    List<double> llyList = col.SelectNodes("descendant::Word").Cast<XmlNode>().Where(x => (x.ChildNodes != null &&
                                            x.ChildNodes[0].Attributes["lly"] != null)).Select(y => Convert.ToDouble(y.ChildNodes[0].Attributes["lly"].Value))
                            .Distinct()
                            .ToList();

                    foreach (double llyCoord in llyList)
                    {
                        lineText.Length = 0;
                        llx = 0;
                        lly = 0;
                        urx = 0;
                        ury = 0;
                        coord = string.Empty;
                        font = string.Empty;
                        fontSize = string.Empty;
                        fontType = string.Empty;
                        top = string.Empty;
                        height = string.Empty;
                        left = string.Empty;

                        var colWordList =
                            col.SelectNodes("descendant::Word").Cast<XmlNode>().Where(x => (x.ChildNodes != null
                                                                                            &&
                                                                                            x.ChildNodes[0].Attributes[
                                                                                                "lly"] != null &&
                                                                                            Convert.ToDouble(
                                                                                                x.ChildNodes[0]
                                                                                                    .Attributes["lly"]
                                                                                                    .Value)
                                                                                                .Equals(
                                                                                                    Convert.ToDouble(
                                                                                                        llyCoord))))
                                .ToList();

                        for (int i = 0; i < colWordList.Count; i++)
                        {
                            if (colWordList[i].ChildNodes.Count > 1)
                            {
                                if (i == 0)
                                {
                                    llx = Convert.ToDouble(colWordList[i].ChildNodes[0].Attributes["llx"].Value);
                                    lly = Convert.ToDouble(colWordList[i].ChildNodes[0].Attributes["lly"].Value);
                                    ury = Convert.ToDouble(colWordList[i].ChildNodes[0].Attributes["ury"].Value);

                                    top = colWordList[i].ChildNodes[0].Attributes["lly"].Value;
                                    left = colWordList[i].ChildNodes[0].Attributes["llx"].Value;

                                    font = colWordList[i].ChildNodes[0].Attributes["font"] != null ? colWordList[i].ChildNodes[0].Attributes["font"].Value : "emptyFont";
                                    fontSize = colWordList[i].ChildNodes[0].Attributes["fontsize"].Value != null ? colWordList[i].ChildNodes[0].Attributes["fontsize"].Value : "emptySize";
                                    fontType = colWordList[i].ChildNodes[0].Attributes["fonttype"] != null ? colWordList[i].ChildNodes[0].Attributes["fonttype"].Value : "Embeded";
                                    height = colWordList[i].ChildNodes[0].Attributes["height"] != null ? colWordList[i].ChildNodes[0].Attributes["height"].Value : "emptyHeight";

                                }

                                if (i == colWordList.Count - 1)
                                    urx = Convert.ToDouble(colWordList[i].ChildNodes[0].Attributes["urx"].Value);

                                if (!string.IsNullOrEmpty(colWordList[i].ChildNodes[1].InnerText.Trim()))
                                {
                                    lineText.Append(colWordList[i].ChildNodes[1].InnerText.Trim() + " ");
                                }
                            }
                        }

                        coord = llx + ":" + lly + ":" + urx + ":" + ury;
                        string newLn = "<ln coord=\"" + coord + "\" " + "page=\"" + page + "\" height=\"" + height +
                                       "\" left=\"" + left + "\" top=\"" + top +
                                       "\" font=\"" + font + "\" fontsize=\"" + fontSize + "\" fonttype=\"" + fontType +
                                       "\" >";

                        string columnLine = newLn + Convert.ToString(lineText) + "</ln>";
                        colText.Append(columnLine);
                    }
                }
            }

            return Convert.ToString(colText).Trim();
        }

        public void InsertMergedRows(XmlNode finalTable, List<HtmlNode> mergedRows)
        {
            XmlNode firstMatchedRow = null;
            List<XmlNode> matchedNodes = new List<XmlNode>();
            List<string> mergedLines = new List<string>();
            bool isMatch = false;
            int totalMergedRow = mergedRows.Count;
            Dictionary<string, int> dictMergedRows = new Dictionary<string, int>();

            foreach (var row in mergedRows)
            {
                var colSplittedText = row.InnerText.Split(new string[] { "\r\n" }, StringSplitOptions.None).ToList();

                if (colSplittedText.Count > 0)
                {
                    var xmlNodeList = finalTable.SelectNodes("//Row | //head-row").Cast<XmlNode>().ToList();

                    for (int i = 0; i < colSplittedText.Count; i++)
                    {
                        isMatch = false;

                        for (int r = 0; r < xmlNodeList.Count; r++)
                        {
                            for (int c = 0; c < xmlNodeList[r].ChildNodes.Count; c++)
                            {
                                if (xmlNodeList[r].ChildNodes[c].InnerText.Trim().Equals(colSplittedText[i]))
                                {
                                    if (i == 0)
                                        firstMatchedRow = xmlNodeList[r];

                                    matchedNodes.Add(xmlNodeList[r]);
                                    isMatch = true;
                                    break;
                                }
                            }
                            if (isMatch) break;
                        }
                    }
                }

                int maxCols = 0;

                foreach (XmlNode rowNode in matchedNodes)
                {
                    maxCols = rowNode.ChildNodes.Count;

                    for (int i = 0; i < maxCols; i++)
                    {
                        if (!dictMergedRows.ContainsKey(rowNode.ChildNodes[i].InnerXml) && !dictMergedRows.ContainsValue(i + 1)) //to check
                            dictMergedRows.Add(rowNode.ChildNodes[i].InnerXml, i + 1);
                    }
                }

                if (firstMatchedRow != null)
                {
                    for (int i = 0; i < firstMatchedRow.ChildNodes.Count; i++)
                    {
                        firstMatchedRow.ChildNodes[i].InnerXml = string.Join(" ", dictMergedRows.Where(x => x.Value == i + 1)
                                                                       .Select(y => Convert.ToString(y.Key)).ToList().ToArray());

                        firstMatchedRow.ChildNodes[i].InnerXml.Replace("&lt;", "<").Replace("&gt;", ">");
                    }

                    int rowToDelete = matchedNodes.Count - 1;

                    for (int i = 0; i < rowToDelete; i++)
                    {
                        if (firstMatchedRow.NextSibling != null)
                            firstMatchedRow.ParentNode.RemoveChild(firstMatchedRow.NextSibling);
                    }
                }
            }

            var finalT = finalTable;
        }

        //public double GetHorizontalLineDiff(List<XmlNode> rowsList)
        //{
        //    //XmlNodeList qq = rowsList.ToList();

        //    if (rowsList[2].ChildNodes[0].ChildNodes[0] == null || rowsList[1].ChildNodes[0].ChildNodes[0] == null) return 0;

        //    double horizontalLineDiff = 0;

        //    if (rowsList.Count > 2)
        //    {
        //        double firstLineLlx = 0;
        //        double secondLineLlx = 0;

        //        if (rowsList[2].ChildNodes[0].ChildNodes[0] != null && rowsList[1].ChildNodes[0].ChildNodes[0] != null)
        //        {
        //            var secondLinecoord = rowsList[2].ChildNodes[0].ChildNodes[0].InnerText.Split(':').ToList();
        //            if (secondLinecoord.Count > 2) 
        //                secondLineLlx = Convert.ToDouble(secondLinecoord[2]);
        //        }
        //        if (rowsList[2].ChildNodes[0].ChildNodes[0] != null && rowsList[1].ChildNodes[0].ChildNodes[0] != null)
        //        {
        //            var firstLinecoord = rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" left=\"" }, StringSplitOptions.None).ToList();
        //            if (firstLinecoord.Count > 1)
        //            {
        //                var firstLineTemp = firstLinecoord[1].Split(new string[] { "\" top=\"" }, StringSplitOptions.None).ToList();
        //                if (firstLineTemp.Count > 0) 
        //                    firstLineLlx = Convert.ToDouble(firstLineTemp[0]);
        //            }
        //        }
        //        horizontalLineDiff = Math.Abs(secondLineLlx - firstLineLlx);
        //    }
        //    return horizontalLineDiff;
        //}

        public double GetHorizontalLineDiff(List<XmlNode> rowsList)
        {
            //XmlNodeList qq = rowsList.ToList();

            if (rowsList[2].ChildNodes[0].ChildNodes[0] == null || rowsList[1].ChildNodes[0].ChildNodes[0] == null) return 0;

            double horizontalLineDiff = 0;

            if (rowsList.Count > 2)
            {
                double firstLineLlx = 0;
                double secondLineLlx = 0;

                if (rowsList[2].ChildNodes[0].ChildNodes[0] != null && rowsList[1].ChildNodes[0].ChildNodes[0] != null)
                {
                    var secondLinecoord = rowsList[2].ChildNodes[0].ChildNodes[0].InnerText.Split(':').ToList();
                    if (secondLinecoord.Count > 2)
                        secondLineLlx = Convert.ToDouble(secondLinecoord[2]);
                }
                if (rowsList[2].ChildNodes[0].ChildNodes[0] != null && rowsList[1].ChildNodes[0].ChildNodes[0] != null)
                {
                    var firstLinecoord = rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" left=\"" }, StringSplitOptions.None).ToList();
                    if (firstLinecoord.Count > 1)
                    {
                        var firstLineTemp = firstLinecoord[1].Split(new string[] { "\" top=\"" }, StringSplitOptions.None).ToList();
                        if (firstLineTemp.Count > 0)
                            firstLineLlx = Convert.ToDouble(firstLineTemp[0]);
                    }
                }
                horizontalLineDiff = Math.Abs(secondLineLlx - firstLineLlx);
            }
            return horizontalLineDiff;
        }

        public XmlNode GetUpdatedXml(XmlNode finalTable, string finalTableHtml, XmlDocument xmlDoc, int page)
        {
            //trList[1].InnerText.Split(new string[] { "\r\n" }, StringSplitOptions.None);

            try
            {
                finalTable.InnerXml = finalTable.InnerXml.Replace("&lt;", "<").Replace("&gt;", ">");
            }
            catch (Exception)
            {
                return null;
            }

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(finalTableHtml);

            List<string> innerTextList = new List<string>();
            List<string> finalTextList = null;
            bool containsMergedRows = false;

            List<XmlNode> matchedNodes = new List<XmlNode>();
            List<HtmlNode> html5ViewerTrList = doc.DocumentNode.SelectNodes("//tr").ToList();
            List<XmlNode> rowsList = finalTable.SelectNodes("//Row | //head-row").Cast<XmlNode>().ToList();

            List<HtmlNode> mergedRows = doc.DocumentNode.SelectNodes("//tr").Cast<HtmlNode>().Where(x => x.Attributes["style"] != null &&
                                                                                 x.Attributes["style"].Value.Equals("white-space: pre")).ToList();
            if (mergedRows.Count > 0)
            {
                containsMergedRows = true;
                InsertMergedRows(finalTable, mergedRows);
            }

            double llx = 0;
            double lly = 0;
            double urx = 0;
            double ury = 0;

            double verticalLineDiff = 6;
            double horizontalLineDiff = 6;

            //int maxColumns = finalTable.SelectNodes("//Row | //head-row").Cast<XmlNode>().ToList().Select(x=>x.ChildNodes.Count).Max();
            //var maxColRow = finalTable.SelectNodes("//Row | //head-row").Cast<XmlNode>().Where(x => x.ChildNodes.Count == maxColumns).Take(1).ToList();

            //var left1 = maxColRow.Where(x => x.ChildNodes.Count > 1 && x.ChildNodes[0].ChildNodes != null && x.ChildNodes[0].ChildNodes.Count > 0)
            //                  .Select(y => Convert.ToDouble(y.ChildNodes[0].ChildNodes[0].Attributes["left"].Value));

            //var left2 = maxColRow.Where(x => x.ChildNodes.Count > 1 && x.ChildNodes[0].ChildNodes != null && x.ChildNodes[0].ChildNodes.Count > 1)
            //                  .Select(y => Convert.ToDouble(y.ChildNodes[0].ChildNodes[1].Attributes["left"].Value));

            //if (maxColRow.Count > 0)
            //{
            //    foreach (XmlNode col in maxColRow)
            //    {

            //    }
            //}

            //var left = finalTable.SelectNodes("descendant::ln/@left");

            string coord = string.Empty;
            string font = string.Empty;
            string fontSize = string.Empty;
            string fontType = string.Empty;
            string top = string.Empty;
            string height = string.Empty;
            string left = string.Empty;
            List<string> coordList = new List<string>();

            if (html5ViewerTrList.Count > 0 && rowsList.Count > 0)
            {
                //Case 1 when rows of html5 viewer's table and xml's table are equal
                if (html5ViewerTrList.Count == rowsList.Count)
                {
                    for (int i = 0; i < rowsList.Count; i++)
                    {
                        //if (i == 1 && rowsList[i].ChildNodes.Count > 1)
                        //{
                        //    if (trList[0].ChildNodes.Count > rowsList[0].ChildNodes.Count)
                        //    {
                        //        if (rowsList[2].ChildNodes[0].ChildNodes[0] != null &&
                        //            rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" left=\"" },
                        //                            StringSplitOptions.None).ToList().Count > 0)
                        //        {
                        //            horizontalLineDiff = Math.Abs(Convert.ToDouble(rowsList[2].ChildNodes[0].ChildNodes[0].InnerText.Split(':')[2]) -
                        //                    Convert.ToDouble(rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" left=\"" },
                        //                            StringSplitOptions.None)[1].Split(new string[] { "\" top=\"" }, StringSplitOptions.None)[0]));
                        //        }
                        //    }
                        //}

                        ////if (i == 1 && rowsList[i].ChildNodes.Count > 1)
                        ////{
                        ////    horizontalLineDiff = GetHorizontalLineDiff(rowsList);
                        ////}

                        for (int j = 0; j < rowsList[i].ChildNodes.Count; j++)
                        {
                            rowsList[i].ChildNodes[j].InnerXml = rowsList[i].ChildNodes[j].InnerXml.Replace("&lt;", "<").Replace("&gt;", ">");

                            if (!string.IsNullOrEmpty(rowsList[i].ChildNodes[j].InnerXml))
                            {
                                string tempInnerxml = rowsList[i].ChildNodes[j].InnerXml.Split(new string[] { "fonttype=\"Embeded\">" }, StringSplitOptions.None)[0] +
                               " fonttype=\"Embeded\">" + (html5ViewerTrList[i].ChildNodes[j] != null ? html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() : "") + "</ln>";

                                rowsList[i].ChildNodes[j].InnerText = (html5ViewerTrList[i].ChildNodes[j] != null ? html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() : "");
                                rowsList[i].ChildNodes[j].InnerXml = tempInnerxml;
                            }
                        }
                    }

                    //When new columns are added in html5 viewer's table 
                    if (html5ViewerTrList[0].ChildNodes.Count > rowsList[0].ChildNodes.Count)
                    {
                        int newColumnCount = html5ViewerTrList[0].ChildNodes.Count - rowsList[0].ChildNodes.Count;

                        for (int i = 0; i < html5ViewerTrList.Count; i++)
                        {
                            for (int j = 0; j < html5ViewerTrList[i].ChildNodes.Count; j++)
                            {
                                if (j >= html5ViewerTrList[i].ChildNodes.Count - newColumnCount)
                                {
                                    XmlNode column = xmlDoc.CreateElement("col");

                                    if (rowsList[rowsList.Count - 1].ChildNodes.Count > 0 & !string.IsNullOrEmpty(rowsList[i].ChildNodes[0].InnerXml))
                                    {
                                        llx = Convert.ToDouble(rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { ":" }, StringSplitOptions.None)[0]
                                                .Split(new string[] { "<ln coord=\"" }, StringSplitOptions.None)[1]) +
                                              horizontalLineDiff;

                                        lly = Convert.ToDouble(rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { ":" },
                                                    StringSplitOptions.None)[1]);

                                        urx = Convert.ToDouble(rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { ":" },
                                                    StringSplitOptions.None)[2]) + horizontalLineDiff;

                                        ury = Convert.ToDouble(rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { "\" page=\"" },
                                                    StringSplitOptions.None)[0]
                                                    .Split(new string[] { ":" }, StringSplitOptions.None)[3]);

                                        top = rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { "\" top=\"" }, StringSplitOptions.None)[1]
                                                        .Split(new string[] { "\" font=\"" }, StringSplitOptions.None)[0];

                                        font = rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { "\" font=\"" }, StringSplitOptions.None)[1]
                                                    .Split(new string[] { "\" fontsize=\"" }, StringSplitOptions.None)[0];

                                        fontSize = rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { "\" fontsize=\"" }, StringSplitOptions.None)[1]
                                                    .Split(new string[] { "\" fonttype=\"" }, StringSplitOptions.None)[0];

                                        height = rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { "\" height=\"" }, StringSplitOptions.None)[1]
                                                    .Split(new string[] { "\" left=\"" }, StringSplitOptions.None)[0];

                                        left = rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { "\" left=\"" }, StringSplitOptions.None)[1]
                                                    .Split(new string[] { "\" top=\"" }, StringSplitOptions.None)[0];
                                    }

                                    coord = llx + ":" + lly + ":" + urx + ":" + ury;
                                    string newLn = "<ln coord=\"" + coord + "\" " + "page=\"" + page + "\" height=\"" +
                                                   height + "\" left=\"" + left + "\" top=\"" +
                                                   top + "\" font=\"" + font + "\" fontsize=\"" + fontSize +
                                                   "\" fonttype=\"" + fontType + "\" >";

                                    column.InnerText = newLn + html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() + "</ln>";
                                    rowsList[i].AppendChild(column);
                                }
                            }
                        }
                    }
                }//end case 1

                //Case 2 when rows of html5 viewer's table and xml's table are equal
                else if (html5ViewerTrList.Count < rowsList.Count && !containsMergedRows)
                {
                    for (int i = 0; i < rowsList.Count; i++)
                    {
                        if (i < html5ViewerTrList.Count)
                        {
                            for (int j = 0; j < rowsList[i].ChildNodes.Count; j++)
                            {
                                rowsList[i].ChildNodes[j].InnerXml = rowsList[i].ChildNodes[j].InnerXml.Replace("&lt;", "<").Replace("&gt;", ">");

                                string tempInnerxml = rowsList[i].ChildNodes[j].InnerXml.Split(new string[] { "fonttype=\"Embeded\">" }, StringSplitOptions.None)[0] + " fonttype=\"Embeded\">" + (html5ViewerTrList[i].ChildNodes[j] != null ? html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() : "") + "</ln>";

                                rowsList[i].ChildNodes[j].InnerText = (html5ViewerTrList[i].ChildNodes[j] != null ? html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() : "");
                                rowsList[i].ChildNodes[j].InnerXml = tempInnerxml;
                            }
                        }
                    }
                }
                else if (html5ViewerTrList.Count > rowsList.Count && !containsMergedRows)
                {
                    for (int i = 0; i < rowsList.Count; i++)
                    {
                        if (i == 1 && rowsList[i].ChildNodes.Count > 1)
                        {
                            //To do
                            //verticalLineDiff = Math.Abs(Convert.ToDouble(rowsList[2].ChildNodes[0].ChildNodes[0].InnerText.Split(':')[1]) -
                            //                            Convert.ToDouble(rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(':')[1]));

                            //top = rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { ":" }, StringSplitOptions.None)[1];
                            //font = rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" font=\"" },
                            //        StringSplitOptions.None)[1].Split(new string[] { "\" fontsize=\"" }, StringSplitOptions.None)[0];
                            //fontSize = rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" fontsize=\"" }, StringSplitOptions.None)[1]
                            //                      .Split(new string[] { "\" fonttype=\"" }, StringSplitOptions.None)[0];
                            //fontType = rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" fonttype=\"" }, StringSplitOptions.None)[1]
                            //                      .Split(new string[] { "\" >" }, StringSplitOptions.None)[0];
                            //height = rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" height=\"" }, StringSplitOptions.None)[1]
                            //                    .Split(new string[] { "\" left=\"" }, StringSplitOptions.None)[0];
                            //left = rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" left=\"" }, StringSplitOptions.None)[1]
                            //            .Split(new string[] { "\" top=\"" }, StringSplitOptions.None)[0];

                            verticalLineDiff = 3;

                            top = "";
                            font = "";
                            fontSize = "";
                            fontType = "";
                            height = "";
                            left = "";

                        }

                        for (int j = 0; j < rowsList[i].ChildNodes.Count; j++)
                        {
                            rowsList[i].ChildNodes[j].InnerXml = rowsList[i].ChildNodes[j].InnerXml.Replace("&lt;", "<").Replace("&gt;", ">");

                            string tempInnerxml = rowsList[i].ChildNodes[j].InnerXml.Split(new string[] { "fonttype=\"Embeded\">" },
                                    StringSplitOptions.None)[0] + " fonttype=\"Embeded\">" + (html5ViewerTrList[i].ChildNodes[j] != null ? html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() : "") + "</ln>";

                            rowsList[i].ChildNodes[j].InnerText = (html5ViewerTrList[i].ChildNodes[j] != null
                                ? html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() : ""); rowsList[i].ChildNodes[j].InnerXml = tempInnerxml;
                        }
                    }

                    var newRowsList = doc.DocumentNode.SelectNodes("//tr").Skip(rowsList.Count - 1).ToList();

                    for (int i = 0; i < newRowsList.Count; i++)
                    {
                        XmlNode newRow = xmlDoc.CreateElement("Row");

                        for (int j = 0; j < newRowsList[i].ChildNodes.Count; j++)
                        {
                            XmlNode column = xmlDoc.CreateElement("col");

                            if (rowsList[rowsList.Count - 1].ChildNodes.Count > 0)
                            {
                                llx = Convert.ToDouble(rowsList[rowsList.Count - 1].ChildNodes[0].InnerXml.Split(new string[] { ":" }, StringSplitOptions.None)[0]
                                                  .Split(new string[] { "<ln coord=\"" }, StringSplitOptions.None)[1]);

                                lly = Convert.ToDouble(rowsList[rowsList.Count - 1].ChildNodes[0].InnerXml.Split(new string[] { ":" }, StringSplitOptions.None)[1]) + verticalLineDiff;

                                urx = Convert.ToDouble(rowsList[rowsList.Count - 1].ChildNodes[0].InnerXml.Split(new string[] { ":" }, StringSplitOptions.None)[2]);

                                ury = Convert.ToDouble(rowsList[rowsList.Count - 1].ChildNodes[0].InnerXml.Split(new string[] { "\" page=\"" }, StringSplitOptions.None)[0]
                                                  .Split(new string[] { ":" }, StringSplitOptions.None)[3]) + verticalLineDiff;
                            }

                            coord = llx + ":" + lly + ":" + urx + ":" + ury;
                            string newLn = "<ln coord=\"" + coord + "\" " + "page=\"" + page + "\" height=\"" + height + "\" left=\"" + left + "\" top=\"" +
                                           top + "\" font=\"" + font + "\" fontsize=\"" + fontSize + "\" fonttype=\"" + fontType + "\" >";

                            column.InnerText = newLn + newRowsList[i].ChildNodes[j].InnerText.Trim() + "</ln>";
                            newRow.AppendChild(column);
                        }
                        XmlNode lastRow = finalTable.SelectNodes("//Row").Count > 0 ? finalTable.SelectNodes("//Row").Cast<XmlNode>().Last() : null;

                        if (lastRow != null)
                            lastRow.ParentNode.InsertAfter(newRow, lastRow);
                    }
                }
            }

            return finalTable;
        }

        public bool HasBoldFont(XmlNode headerRow)
        {
            if (headerRow == null) return false;

            if (headerRow.FirstChild != null)
            {
                if (headerRow.FirstChild.ChildNodes[0].ChildNodes.Count > 0)
                {
                    if (headerRow.FirstChild.ChildNodes[0].ChildNodes[0].ChildNodes.Count > 0)
                    {
                        if (headerRow.FirstChild.ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["font"] != null)
                        {
                            if (headerRow.FirstChild.ChildNodes[0].ChildNodes[0].ChildNodes[0].Attributes["font"].Value.ToLower().Contains("bold"))
                                return true;
                        }
                    }
                }
            }
            return false;
        }

        //Mark table's button click from dialog


        ////old
        ////Mark table's button click from dialog
        //protected void btnMarkTableLines_Click(object sender, EventArgs e)
        //{
        //    string tableLines = hfMarkedTableText.Value;
        //    var tableXml = MarkTableInXml(tableLines);

        //    if (tableXml == null) return;

        //    StringBuilder tableHeader = new StringBuilder();
        //    StringBuilder tableCaption = new StringBuilder();

        //    StringBuilder tableBody = new StringBuilder();

        //    bool containsBoldFont = false;
        //    int index = 0;

        //    var rows = tableXml.SelectNodes("Row");

        //    if (rows == null || rows.Count < 1) return;

        //    for (int i = 0; i < rows.Count; i++)
        //    {
        //        if (i == 0)
        //        {
        //            if (rows[i].ChildNodes != null)
        //            {
        //                while (rows[i].ChildNodes.Count == 1)
        //                {
        //                    var tableHeaderLine =
        //                        rows[i].ChildNodes[0].SelectNodes("descendant::Text")
        //                            .Cast<XmlNode>()
        //                            .Select(y => y.InnerText)
        //                            .ToList();
        //                    tableHeader.Append(String.Join(" ", tableHeaderLine.ToArray()) + " ");
        //                    if (i < rows.Count)
        //                    {
        //                        i++;
        //                    }
        //                }
        //            }
        //            containsBoldFont = HasBoldFont(rows[i]);
        //        }
        //        else if (i == rows.Count - 1)
        //        {
        //            index = i;

        //            if (rows[index].ChildNodes != null)
        //            {
        //                while (rows[index].ChildNodes.Count == 1)
        //                {
        //                    if (i > 0)
        //                    {
        //                        index--;
        //                    }
        //                }
        //            }
        //            index++;

        //            for (int j = index; j < rows.Count; j++)
        //            {
        //                var tableCaptionLine = rows[j].ChildNodes[0].SelectNodes("descendant::Text").Cast<XmlNode>().Select(y => y.InnerText).ToList();
        //                tableCaption.Append(String.Join(" ", tableCaptionLine.ToArray()));
        //            }
        //        }
        //    }

        //    //By default table border is on
        //    rbtnlTableBorder.SelectedIndex = 0;

        //    if (containsBoldFont) rbtnlHeaderRow.SelectedIndex = 0;
        //    else rbtnlHeaderRow.SelectedIndex = 1;

        //    if (!String.IsNullOrEmpty(Convert.ToString(tableHeader)))
        //    {
        //        rbtnlTableHeader.SelectedIndex = 0;
        //        btnManualHeader.Attributes.Add("style", "display:none");
        //    }
        //    else
        //    {
        //        rbtnlTableHeader.SelectedIndex = 1;
        //        btnManualHeader.Attributes.Add("style", "display:inline-block; position:relative; bottom:9px");
        //    }

        //    if (!String.IsNullOrEmpty(Convert.ToString(tableCaption)))
        //    {
        //        rbtnlTableCaption.SelectedIndex = 0;
        //        btnManualCaption.Attributes.Add("style", "display:none");
        //    }
        //    else
        //    {
        //        rbtnlTableCaption.SelectedIndex = 1;
        //        btnManualCaption.Attributes.Add("style", "display:inline-block; position:relative; bottom:9px");
        //        //btnManualCaption.Attributes.Add("position", "relative");
        //        //btnManualCaption.Attributes.Add("bottom", "5px");
        //    }

        //    divHeaderRow.InnerText = Convert.ToString(tableHeader);
        //    divCaptionRow.InnerText = Convert.ToString(tableCaption);

        //    divTableBody.InnerText = Convert.ToString(tableBody);

        //    ClientScript.RegisterStartupScript(this.GetType(), "showTableDialog", "ShowMarkedTableDialog();", true);
        //}



        //public List<XmlNode> MarkTableInXml(string tableLines)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(tableLines)) return null;

        //        if (!string.IsNullOrEmpty(tableLines))
        //        {
        //            TableDetection tblDetectionObj = new TableDetection();
        //            tblDetectionObj.TempXmlDoc = null;
        //            tblDetectionObj.TableXml = null;

        //            var tables = tblDetectionObj.SaveTablesInXml(tableLines, cbxIgnoreAlgo.Checked, false);
        //            return tables;
        //        }

        //        return null;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //public XmlNode MarkTableInXml(string tableLines, string selectionType)
        //{
        //    try
        //    {
        //        if (string.IsNullOrEmpty(tableLines)) return null;

        //        if (!string.IsNullOrEmpty(tableLines))
        //        {
        //            TableDetection tblDetectionObj = new TableDetection();
        //            tblDetectionObj.TempXmlDoc = null;
        //            tblDetectionObj.TableXml = null;

        //            var tables = tblDetectionObj.SaveTablesInXml(tableLines, cbxIgnoreAlgo.Checked, false, selectionType);
        //            return tables;
        //        }

        //        return null;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        private string DrawColumnWidthTable(List<XmlNode> bodyLinesList)
        {
            try
            {
                StringBuilder tableStringBuilder = null;

                if (bodyLinesList != null)
                {
                    tableStringBuilder = new StringBuilder();

                    int maxColumns = bodyLinesList.Select(x => x.ChildNodes.Count).Max();
                    int rows = bodyLinesList.Count;
                    int counter = 0;

                    double widthPer = Math.Round((Convert.ToDouble(86 / maxColumns)), 2);
                    double tableColWidth = Math.Round((Convert.ToDouble(100 / maxColumns)), 2);

                    for (int row = 0; row < 2; row++)
                    {
                        tableStringBuilder.Append("<tr>");
                        var columnsList = bodyLinesList[row].SelectNodes("descendant::Cell");

                        if (columnsList.Count < maxColumns)
                            counter = maxColumns;
                        else
                            counter = columnsList.Count;

                        for (int col = 0; col <= counter; col++)
                        {
                            if (row == 0)
                            {
                                if (col == 0)
                                {
                                    tableStringBuilder.Append("<td style='border: 1px solid black; width:14%;'>");
                                    tableStringBuilder.Append("Columns");
                                    tableStringBuilder.Append("</td>");
                                }
                                else
                                {
                                    tableStringBuilder.Append("<td style='border: 1px solid black; width:" + widthPer + "%;'>");
                                    tableStringBuilder.Append("Col " + col);
                                    tableStringBuilder.Append("</td>");
                                }
                            }
                            else if (row == 1)
                            {
                                if (col == 0)
                                {
                                    tableStringBuilder.Append("<td style='border: 1px solid black; width:14%;'>");
                                    tableStringBuilder.Append("Width");
                                    tableStringBuilder.Append("</td>");
                                }
                                else
                                {
                                    tableStringBuilder.Append("<td style='border: 1px solid black;background-color: #f0f0f0; width:" + widthPer + "%;' contenteditable='true'>");
                                    tableStringBuilder.Append(tableColWidth);
                                    tableStringBuilder.Append("</td>");
                                }
                            }
                        }

                        tableStringBuilder.Append("</tr>");
                    }
                }
                return Convert.ToString(tableStringBuilder);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string DrawTableBody(List<XmlNode> bodyLinesList)
        {
            try
            {
                StringBuilder tableStringBuilder = null;

                if (bodyLinesList != null)
                {
                    string columnLine = "";
                    tableStringBuilder = new StringBuilder();

                    int maxColumns = bodyLinesList.Select(x => x.ChildNodes.Count).Max();
                    int rows = bodyLinesList.Count;

                    //double widthPer = (Convert.ToDouble(Convert.ToString(columns) + ".0") / rows) * 100;
                    double widthPer = Math.Round((Convert.ToDouble(100 / maxColumns)), 2);

                    for (int row = 0; row < rows; row++)
                    {
                        tableStringBuilder.Append("<tr class='selectedRow'>");
                        var columnsList = bodyLinesList[row].SelectNodes("descendant::Cell");

                        for (int col = 0; col < maxColumns; col++)
                        {
                            if (col < columnsList.Count)
                            {
                                tableStringBuilder.Append("<td style='border: 1px solid black; width:" + widthPer + "%;'>");
                                columnLine = combineColumnLines(columnsList[col]);
                                tableStringBuilder.Append(columnLine);
                                tableStringBuilder.Append("</td>");
                            }
                            else
                            {
                                tableStringBuilder.Append("<td style='border: 1px solid black; width:" + widthPer + "%;'>");
                                columnLine = "";
                                tableStringBuilder.Append(columnLine);
                                tableStringBuilder.Append("</td>");
                            }
                        }
                        tableStringBuilder.Append("</tr>");
                    }
                }
                return Convert.ToString(tableStringBuilder);
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string DrawTableBodyAutoXml(List<XmlNode> bodyLinesList)
        {
            try
            {
                StringBuilder tableStringBuilder = null;

                if (bodyLinesList != null)
                {
                    string columnLine = "";
                    tableStringBuilder = new StringBuilder();

                    int maxColumns = bodyLinesList.Select(x => x.ChildNodes.Count).Max();
                    int rows = bodyLinesList.Count;

                    //double widthPer = (Convert.ToDouble(Convert.ToString(columns) + ".0") / rows) * 100;
                    double widthPer = Math.Round((Convert.ToDouble(100 / maxColumns)), 2);

                    for (int row = 0; row < rows; row++)
                    {
                        tableStringBuilder.Append("<tr class='selectedRow'>");
                        var columnsList = bodyLinesList[row].SelectNodes("descendant::col");

                        for (int col = 0; col < maxColumns; col++)
                        {
                            if (col < columnsList.Count)
                            {
                                tableStringBuilder.Append("<td style='border: 1px solid black; width:" + widthPer + "%;'>");
                                columnLine = combineColumnLinesAutoXml(columnsList[col]);
                                tableStringBuilder.Append(columnLine + "</td>");
                            }
                            else
                            {
                                tableStringBuilder.Append("<td style='border: 1px solid black; width:" + widthPer + "%;'>");
                                columnLine = "";
                                tableStringBuilder.Append(columnLine + "</td>");
                            }
                        }
                        tableStringBuilder.Append("</tr>");
                    }
                }
                return Convert.ToString(tableStringBuilder);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<string> GetLinesFromAutoDetecXml(XmlNode bodyLinesList)
        {
            if (bodyLinesList == null) return null;

            List<string> tableLinesList = new List<string>();
            try
            {
                StringBuilder tableRow = new StringBuilder();

                var llyList = bodyLinesList.SelectNodes("descendant::Box/@lly").Cast<XmlNode>().Select(x => x.Value).Distinct().ToList();

                if (llyList.Count > 0)
                {
                    for (int i = 0; i < llyList.Count; i++)
                    {
                        var rowWords = bodyLinesList.SelectNodes("descendant::Box[@lly ='" + llyList[i] + "']/../Text").Cast<XmlNode>().ToList();

                        if (rowWords.Count > 0)
                        {
                            for (int j = 0; j < rowWords.Count; j++)
                            {
                                tableRow.Append(rowWords[j].InnerText.Trim() + " ");
                            }

                            tableLinesList.Add(Convert.ToString(tableRow).Trim());
                            tableRow.Length = 0;
                        }
                    }
                }

                return tableLinesList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public List<string> GetLinesFromAutoDetecXml2(XmlNode bodyLinesList)
        {
            if (bodyLinesList == null) return null;

            List<string> tableLinesList = new List<string>();
            try
            {
                StringBuilder tableRow = new StringBuilder();

                var llyList = bodyLinesList.SelectNodes("descendant::ln/@top").Cast<XmlNode>().Select(x => x.Value).Distinct().ToList();

                if (llyList.Count > 0)
                {
                    for (int i = 0; i < llyList.Count; i++)
                    {
                        var rowWords = bodyLinesList.SelectNodes("descendant::ln[@top ='" + llyList[i] + "']").Cast<XmlNode>().ToList();

                        if (rowWords.Count > 0)
                        {
                            for (int j = 0; j < rowWords.Count; j++)
                            {
                                tableRow.Append(rowWords[j].InnerText.Trim() + " ");
                            }

                            tableLinesList.Add(Convert.ToString(tableRow).Trim());
                            tableRow.Length = 0;
                        }
                    }
                }

                return tableLinesList;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private string combineColumnLines(XmlNode tableColumnLines)
        {
            if (tableColumnLines == null) return "";

            StringBuilder tableBody = new StringBuilder();

            foreach (XmlNode row in tableColumnLines)
            {
                var tableBodyLines = row.SelectNodes("descendant::Text").Cast<XmlNode>().Select(y => y.InnerText).ToList();
                tableBody.Append(String.Join(" ", tableBodyLines.ToArray()) + " ");
            }

            return Convert.ToString(tableBody);
        }

        private string combineColumnLinesAutoXml(XmlNode tableColumnLines)
        {
            if (tableColumnLines == null) return "";

            StringBuilder tableBody = new StringBuilder();

            foreach (XmlNode ln in tableColumnLines)
            {
                tableBody.Append(ln.InnerText + " ");
            }

            return Convert.ToString(tableBody);
        }

        //private string DrawTableInRichTbx(BOL.Table tableLines)
        //{
        //    try
        //    {
        //        StringBuilder tableStringBuilder = null;

        //        if (tableLines != null)
        //        {
        //            string columnLine = "";
        //            tableStringBuilder = new StringBuilder();

        //            int columns = tableLines.ColumnCount;
        //            int rows = tableLines.Rows.Count;
        //            double widthPer = (Convert.ToDouble(Convert.ToString(columns) + ".0") / rows) * 100;

        //            tableStringBuilder.Append("<table border='1' cellpadding='1'>");

        //            for (int row = 0; row < rows; row++)
        //            {
        //                tableStringBuilder.Append("<tr>");

        //                for (int col = 1; col <= columns; col++)
        //                {
        //                    tableStringBuilder.Append("<td style='width:" + widthPer + "%;'>");
        //                    columnLine = combineColumnLines(tableLines.Rows[row].Lines, col);
        //                    tableStringBuilder.Append(columnLine);
        //                    tableStringBuilder.Append("</td>");
        //                }
        //                tableStringBuilder.Append("</tr>");
        //            }
        //            tableStringBuilder.Append("</table>");
        //        }
        //        return Convert.ToString(tableStringBuilder);
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //private string combineColumnLines(List<Line> tableColumnLines, int column)
        //{
        //    if (tableColumnLines.Count == 0) return "";

        //    StringBuilder columnText = new StringBuilder();

        //    foreach (var line in tableColumnLines)
        //    {
        //        if (line.ColumnNum == column)
        //            columnText.Append(line.LineText + " ");
        //    }
        //    return Convert.ToString(columnText);
        //}

        #region Web Methods

        //[WebMethod]
        //public static bool MarkTable(string text)
        //{
        //    bool status = false;

        //    try
        //    {
        //        if (string.IsNullOrEmpty(text)) return status;

        //        var temp = text.Split(new string[] { "/~/" }, StringSplitOptions.None).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

        //        if ((temp != null) && (temp.Count > 2))
        //        {
        //            //var tableLines = temp[0].Split('\n').Select(x => x.Trim()).Distinct().ToList();
        //            var tableLines = temp[0];
        //            bool isIgnorAlgoChecked = Convert.ToString(temp[1]) == "" ? false : Convert.ToBoolean(temp[1]);

        //            //HttpContext.Current.Session["isIgnorAlgoChecked"] = Convert.ToString(temp[1]) == "" ? false : Convert.ToBoolean(temp[1]);

        //            TableDetection tblDetectionObj = new TableDetection();
        //            var tables = tblDetectionObj.SaveTablesInXml(tableLines, isIgnorAlgoChecked);

        //            if (tables != null && tables.Count > 0)
        //            {
        //                tblDetectionObj.CreateXls(tables);
        //                //tblDetectionObj.SaveAsXml(tables);

        //                string mainDirectoryPath = Common.GetDirectoryPath();
        //                string bookId = Convert.ToString(HttpContext.Current.Session["MainBook"]);
        //                string sourcePdfPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\" + bookId + "_actual.tetml";
        //                string outputHighlightedPdfPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\" + bookId + "_actual_Manual_Highlighted.pdf";

        //                List<string> abbrCoord = tblDetectionObj.GetCoordinates(sourcePdfPath, tables);
        //                if (abbrCoord != null)
        //                {
        //                    if (abbrCoord.Count > 0)
        //                    {
        //                        if (tblDetectionObj.HighLightTables(sourcePdfPath.Replace(".tetml", ".pdf"),
        //                            outputHighlightedPdfPath, BaseColor.GREEN, abbrCoord))
        //                        {
        //                            List<string> tableContainingPages = (List<string>)HttpContext.Current.Session["TablePages"];
        //                            int currentPage = Convert.ToInt32(HttpContext.Current.Session["CurrentPage"]);
        //                            string pageNum = tableContainingPages.ElementAt(currentPage - 1);

        //                            string pdfPreviewDirPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\PdfPreview";
        //                            string outputManualHighlightedPdfPath = pdfPreviewDirPath + "\\" + pageNum + "_Manual_Highlighted.pdf";

        //                            tblDetectionObj.ExtractPage(outputHighlightedPdfPath, outputManualHighlightedPdfPath, pageNum);
        //                            //HttpContext.Current.Session["isIgnorAlgoChecked"] = "false";
        //                        }
        //                    }
        //                }

        //                status = true;

        //                //HttpContext.Current.Session["isNextClicked"] = "False";
        //            }
        //        }

        //        return status;
        //    }
        //    catch (Exception)
        //    {
        //        return status;
        //    }
        //}

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
        public static bool MarkTableHeaderCaption(string text)
        {
            bool status = false;

            try
            {
                if (string.IsNullOrEmpty(text)) return status;

                var temp = text.Split(new string[] { "/~/" }, StringSplitOptions.None).Where(x => !string.IsNullOrWhiteSpace(x)).ToList();

                if ((temp != null) && (temp.Count > 1))
                {
                    string tableLines = temp[0];
                    string rowType = temp[1];

                    TableDetection tblDetectionObj = new TableDetection();
                    status = tblDetectionObj.SaveHeaderCaptionInXml(tableLines, rowType);

                    //if (tableXml != null)
                    //{
                    //    tblDetectionObj.CreateXls(tableXml);
                    //    //tblDetectionObj.SaveAsXml(tables);

                    //    string mainDirectoryPath = Common.GetDirectoryPath();
                    //    string bookId = Convert.ToString(HttpContext.Current.Session["MainBook"]);
                    //    string sourcePdfPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\" + bookId + "_actual.tetml";
                    //    string outputHighlightedPdfPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\" + bookId + "_actual_Manual_Highlighted.pdf";

                    //    List<string> abbrCoord = tblDetectionObj.GetCoordinates(sourcePdfPath, tableXml);
                    //    if (abbrCoord != null)
                    //    {
                    //        if (abbrCoord.Count > 0)
                    //        {
                    //            if (tblDetectionObj.HighLightTables(sourcePdfPath.Replace(".tetml", ".pdf"),
                    //                outputHighlightedPdfPath, BaseColor.GREEN, abbrCoord))
                    //            {
                    //                List<string> tableContainingPages = (List<string>)HttpContext.Current.Session["TablePages"];
                    //                int currentPage = Convert.ToInt32(HttpContext.Current.Session["CurrentPage"]);
                    //                string pageNum = tableContainingPages.ElementAt(currentPage - 1);

                    //                string pdfPreviewDirPath = mainDirectoryPath + "\\" + bookId + "\\DetectedTables\\PdfPreview";
                    //                string outputManualHighlightedPdfPath = pdfPreviewDirPath + "\\" + pageNum + "_Manual_Highlighted.pdf";

                    //                tblDetectionObj.ExtractPage(outputHighlightedPdfPath, outputManualHighlightedPdfPath, pageNum);
                    //                //HttpContext.Current.Session["isIgnorAlgoChecked"] = "false";
                    //            }
                    //        }
                    //    }

                    //    status = true;

                    //    //HttpContext.Current.Session["isNextClicked"] = "False";
                    //}
                }

                return status;
            }
            catch (Exception)
            {
                return status;
            }
        }

        #endregion
    }
}
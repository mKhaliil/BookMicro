using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI.WebControls;
using System.Xml;
using EO.Web;
using HtmlAgilityPack;
using iTextSharp.text;
using iTextSharp.text.pdf;
using OfficeOpenXml;
//using OfficeOpenXml.FormulaParsing.Excel.Functions.RefAndLookup;
using Outsourcing_System.PdfCompare_Classes;
using System.Globalization;
using System.Drawing;
using Telerik.Charting.Styles;

namespace Outsourcing_System.CommonClasses
{
    public class TableDetection
    {
        public XmlDocument TempXmlDoc
        {
            get
            {
                if (HttpContext.Current.Session["TempXmlDoc"] != null)
                {
                    return ((XmlDocument)HttpContext.Current.Session["TempXmlDoc"]);
                }
                return null;
            }
            set { HttpContext.Current.Session["TempXmlDoc"] = value; }
        }

        public XmlNode TableXml
        {
            get
            {
                if (HttpContext.Current.Session["TableXml"] != null)
                {
                    return ((XmlNode)HttpContext.Current.Session["TableXml"]);
                }
                return null;
            }
            set { HttpContext.Current.Session["TableXml"] = value; }
        }

        public XmlNodeList TableHeader
        {
            get
            {
                if (HttpContext.Current.Session["TableHeader"] != null)
                {
                    return ((XmlNodeList)HttpContext.Current.Session["TableHeader"]);
                }
                return null;
            }
            set { HttpContext.Current.Session["TableHeader"] = value; }
        }

        public XmlNodeList TableCaption
        {
            get
            {
                if (HttpContext.Current.Session["TableCaption"] != null)
                {
                    return ((XmlNodeList)HttpContext.Current.Session["TableCaption"]);
                }
                return null;
            }
            set { HttpContext.Current.Session["TableCaption"] = value; }
        }

        public XmlNode SeletectedHeaderCapXml
        {
            get
            {
                if (HttpContext.Current.Session["SeletectedHeaderCapXml"] != null)
                {
                    return ((XmlNode)HttpContext.Current.Session["SeletectedHeaderCapXml"]);
                }
                return null;
            }
            set { HttpContext.Current.Session["SeletectedHeaderCapXml"] = value; }
        }


        #region Algorithm 1

        #endregion

        #region Algorithm 2


        #endregion

        #region Algorithm 3

        #endregion

        #region Algorithm 4


        #endregion

        public List<string> GetTableStartingLine(List<string> pdfJsLinesList)
        {
            if (pdfJsLinesList == null || pdfJsLinesList.Count == 0) return null;

            List<string> firstLineWords = new List<string>();

            for (int i = 0; i < pdfJsLinesList.Count; i++)
            {
                List<string> pdfJsLineWordsList = Regex.Split(pdfJsLinesList[i], @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

                if (pdfJsLineWordsList.Count >= 3)
                {
                    //firstLineWords.Add(pdfJsLineWordsList[0].Trim());
                    //firstLineWords.Add(pdfJsLineWordsList[1].Trim());
                    //firstLineWords.Add(pdfJsLineWordsList[2].Trim());

                    foreach (var word in pdfJsLineWordsList)
                    {
                        firstLineWords.Add(word.Trim() + " ");
                    }
                    break;
                }
                else if (pdfJsLineWordsList.Count == 2)
                {
                    firstLineWords.Add(pdfJsLineWordsList[0].Trim());
                    firstLineWords.Add(pdfJsLineWordsList[1].Trim());

                    if (i + 1 < pdfJsLinesList.Count)
                    {
                        List<string> pdfJsNextLineWordsList = Regex.Split(pdfJsLinesList[i + 1], @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

                        if (pdfJsNextLineWordsList.Count >= 1)
                        {
                            firstLineWords.Add(pdfJsNextLineWordsList[0].Trim());
                            break;
                        }
                    }
                }
                else if (pdfJsLineWordsList.Count == 1)
                {
                    firstLineWords.Add(pdfJsLineWordsList[0]);

                    if (i + 1 < pdfJsLinesList.Count)
                    {
                        List<string> pdfJsNextLineWordsList = Regex.Split(pdfJsLinesList[i + 1], @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

                        if (pdfJsNextLineWordsList.Count > 1)
                        {
                            firstLineWords.Add(pdfJsNextLineWordsList[0].Trim());
                            firstLineWords.Add(pdfJsNextLineWordsList[1].Trim());
                            break;
                        }
                        else if (pdfJsNextLineWordsList.Count == 1)
                        {
                            firstLineWords.Add(pdfJsNextLineWordsList[0].Trim());

                            if (i + 2 < pdfJsLinesList.Count)
                            {
                                List<string> pdfJsNextLineWord = Regex.Split(pdfJsLinesList[i + 2], @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
                                if (pdfJsNextLineWord.Count >= 1)
                                {
                                    firstLineWords.Add(pdfJsNextLineWord[0].Trim());
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            return firstLineWords;
        }

        public List<string> GetTableEndLine(List<string> pdfJsLinesList)
        {
            if (pdfJsLinesList == null || pdfJsLinesList.Count == 0) return null;

            List<string> endLineWords = new List<string>();

            for (int i = pdfJsLinesList.Count - 1; i >= 0; i--)
            {
                List<string> pdfJsLineWordsList = Regex.Split(pdfJsLinesList[i], @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

                if (pdfJsLineWordsList.Count >= 3)
                {
                    //endLineWords.Add(pdfJsLineWordsList[pdfJsLineWordsList.Count - 3].Trim());
                    //endLineWords.Add(pdfJsLineWordsList[pdfJsLineWordsList.Count - 2].Trim());
                    //endLineWords.Add(pdfJsLineWordsList[pdfJsLineWordsList.Count - 1].Trim());

                    foreach (var word in pdfJsLineWordsList)
                    {
                        endLineWords.Add(word.Trim() + " ");
                    }

                    break;
                }
                else if (pdfJsLineWordsList.Count == 2)
                {
                    endLineWords.Add(pdfJsLineWordsList[pdfJsLineWordsList.Count - 2].Trim());
                    endLineWords.Add(pdfJsLineWordsList[pdfJsLineWordsList.Count - 1].Trim());

                    if (i - 1 >= 0)
                    {
                        List<string> pdfJsNextLineWordsList = Regex.Split(pdfJsLinesList[i - 1], @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

                        if (pdfJsNextLineWordsList.Count >= 1)
                        {
                            endLineWords.Add(pdfJsNextLineWordsList[pdfJsNextLineWordsList.Count - 1].Trim());
                            break;
                        }
                    }
                }
                else if (pdfJsLineWordsList.Count == 1)
                {
                    endLineWords.Add(pdfJsLineWordsList[pdfJsLineWordsList.Count - 1]);

                    if (i - 1 >= 0)
                    {
                        List<string> pdfJsNextLineWordsList = Regex.Split(pdfJsLinesList[i - 1], @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

                        //if (pdfJsNextLineWordsList.Count > 1)
                        //{
                        //    endLineWords.Add(pdfJsNextLineWordsList[pdfJsNextLineWordsList.Count - 1].Trim());
                        //    endLineWords.Add(pdfJsNextLineWordsList[pdfJsNextLineWordsList.Count - 2].Trim());
                        //    break;
                        //}
                        //else if (pdfJsNextLineWordsList.Count == 1)
                        //{
                        //    endLineWords.Add(pdfJsNextLineWordsList[pdfJsNextLineWordsList.Count - 1].Trim());

                        //    if (i - 2 >= 0)
                        //    {
                        //        List<string> pdfJsNextLineWord = Regex.Split(pdfJsLinesList[i - 2], @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
                        //        if (pdfJsNextLineWord.Count >= 1)
                        //        {
                        //            endLineWords.Add(pdfJsNextLineWord[pdfJsNextLineWordsList.Count - 1].Trim());
                        //            break;
                        //        }
                        //    }
                        //}

                        if (pdfJsNextLineWordsList.Count == 1)
                        {
                            endLineWords.Add(pdfJsNextLineWordsList[pdfJsNextLineWordsList.Count - 1].Trim());

                            if (i - 2 >= 0)
                            {
                                List<string> pdfJsNextLineWord = Regex.Split(pdfJsLinesList[i - 2], @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
                                if (pdfJsNextLineWord.Count >= 1)
                                {
                                    //endLineWords.Add(pdfJsNextLineWord[pdfJsNextLineWordsList.Count - 1].Trim());
                                    //break;
                                    foreach (var word in pdfJsNextLineWord)
                                    {
                                        endLineWords.Add(word.Trim() + " ");
                                    }

                                    break;
                                }
                            }
                        }

                        else if (pdfJsNextLineWordsList.Count > 1)
                        {
                            foreach (var word in pdfJsNextLineWordsList)
                            {
                                endLineWords.Add(word.Trim() + " ");
                            }

                            break;
                        }
                    }
                }
            }

            return endLineWords;
        }

        public int GetCurrentPageNum()
        {
            int currentPage = Convert.ToInt32(HttpContext.Current.Session["CurrentPage"]);
            List<string> tableContainingPages = (List<string>)HttpContext.Current.Session["TablePages"];

            return Convert.ToInt32(tableContainingPages.ElementAt(currentPage - 1));
        }

        public List<string> GetPdfJsSelectedLines(string linesList, int pageNumber)
        {
            List<string> pdfJsLinesList = new List<string>();
            StringBuilder sbPdfJsLine = new StringBuilder();
            Common comObj = new Common();

            List<PdfJsLine> pdfJsLines = comObj.GetCurrentPdfJsPageLines(linesList, pageNumber);

            if (pdfJsLines != null)
            {
                if (pdfJsLines.Count > 0)
                {
                    List<double> pdfJsCoordList = pdfJsLines.Select(x => Math.Truncate(Convert.ToDouble(x.Top))).Distinct().OrderBy(x => x).ToList();

                    foreach (var topCoord in pdfJsCoordList)
                    {
                        var matchedLines = pdfJsLines.Where(x => Math.Truncate(Convert.ToDouble(x.Top)).Equals(topCoord)).ToList();
                        foreach (var line in matchedLines)
                        {
                            sbPdfJsLine.Append(line.Text.Trim() + " ");
                        }
                        pdfJsLinesList.Add(Convert.ToString(sbPdfJsLine));
                        sbPdfJsLine.Length = 0;
                    }
                }
            }

            return pdfJsLinesList;
        }

        public List<string> GetAutoSelectedLines(string linesList, string pageNumber)
        {
            List<string> pdfJsLinesList = new List<string>();
            StringBuilder sbPdfJsLine = new StringBuilder();
            Common comObj = new Common();

            List<PdfJsLine> pdfJsLines = comObj.GetCurrentPdfJsPageLines(linesList, Convert.ToInt32(pageNumber));

            if (pdfJsLines != null)
            {
                if (pdfJsLines.Count > 0)
                {
                    List<double> pdfJsCoordList = pdfJsLines.Select(x => Math.Truncate(Convert.ToDouble(x.Top))).Distinct().OrderBy(x => x).ToList();

                    foreach (var topCoord in pdfJsCoordList)
                    {
                        var matchedLines = pdfJsLines.Where(x => Math.Truncate(Convert.ToDouble(x.Top)).Equals(topCoord)).ToList();
                        foreach (var line in matchedLines)
                        {
                            sbPdfJsLine.Append(line.Text.Trim() + " ");
                        }
                        pdfJsLinesList.Add(Convert.ToString(sbPdfJsLine));
                        sbPdfJsLine.Length = 0;
                    }
                }
            }

            return pdfJsLinesList;
        }

        //Working prev backup
        //public List<XmlNode> SaveTablesInXml(string linesList, bool isIgnorAlgoChecked)
        //{
        //    if (string.IsNullOrEmpty(linesList)) return null;

        //    string pageNumber = GetCurrentPageNum();
        //    List<string> pdfJsLinesList = GetPdfJsSelectedLines(linesList, pageNumber);

        //    if (pdfJsLinesList == null || pdfJsLinesList.Count == 0) return null;

        //    string temmXml = "";
        //    int pageNum = Convert.ToInt32(HttpContext.Current.Session["ActualPdfPage"]);

        //    if (isIgnorAlgoChecked)
        //        temmXml = Convert.ToString(HttpContext.Current.Session["tempXmlPath_actual"]);
        //    else
        //        temmXml = Convert.ToString(HttpContext.Current.Session["tempXmlPath"]);

        //    List<XmlNode> tables = new List<XmlNode>();
        //    XmlNode table = null;
        //    XmlDocument tblDocument = null;
        //    int pageno = 0;

        //    //XElement xmlEle = XElement.Load(temmXml);
        //    List<XmlNode> tableLines = new List<XmlNode>();
        //    XmlDocument xmlDoc = new XmlDocument();
        //    xmlDoc.Load(temmXml);

        //    int normalX = 0;
        //    int normalXIndent = 0;
        //    double lineHeight = 0;

        //    NormalAndIndentX(xmlDoc, ref normalX, ref normalXIndent);

        //    XmlNodeList pageNode = xmlDoc.SelectNodes("//Page[@number=" + pageNum + "]");

        //    if (pageNode != null)
        //    {
        //        if (pageNode.Count > 0)
        //        {
        //            if (pageNode[0].Attributes["height"] != null)
        //            {
        //                lineHeight = Convert.ToDouble(pageNode[0].Attributes["height"].Value);
        //            }
        //        }
        //    }

        //    //XmlNodeList listLines = xmlDoc.SelectNodes("//Line");
        //    XmlNodeList listLines = xmlDoc.SelectNodes("//Page[@number=" + pageNum + "]//Line");
        //    List<XmlNode> LinesPassed = new List<XmlNode>();
        //    bool isProperTable = false;
        //    int ActivePage = 0;
        //    int noOftables = 1;
        //    int tableStartLine = 0;
        //    bool isTableEndLine = false;

        //    int AverageVeticalSpace = Convert.ToInt32(HttpContext.Current.Session["AverageVeticalSpace"]);
        //    int AverageSpace = Convert.ToInt32(HttpContext.Current.Session["AverageSpace"]);

        //    List<string> tblStartLineWords = GetTableStartingLine(pdfJsLinesList);
        //    List<string> tblEndLineWords = GetTableEndLine(pdfJsLinesList);

        //    for (int i = 0; i < listLines.Count; i++)
        //    {
        //        if (!LinesPassed.Contains(listLines[i]))
        //        {
        //            //If current Line is a table line
        //            if (isTableStartingLine(listLines[i], tblStartLineWords))
        //            {
        //                tableStartLine++;

        //                ////if next node is not null and is table line
        //                //if (listLines[i].NextSibling != null && (listLines[i].NextSibling.Name.Equals("Line") && (isMatchedLine(listLines[i].NextSibling, linesList))))
        //                ////if (listLines[i].NextSibling != null && (listLines[i].NextSibling.Name.Equals("Line") && (tableStartLine < linesList.Count)))
        //                //{
        //                XmlNode currentNode = listLines[i];
        //                bool firstLine = true;
        //                double Linediffrence = 0;
        //                pageno = Convert.ToInt32(listLines[i].SelectSingleNode("ancestor::Page").Attributes["number"].Value);
        //                //Create table and add rows till the end of abnormal rows...
        //                do
        //                {
        //                    tableLines.Add(currentNode);
        //                    //if first abnormal row then start creating a new table
        //                    if (firstLine)
        //                    {
        //                        isProperTable = false;
        //                        string OutputXml1 = "<?xml version=\"1.0\"?><Table page='" + pageno + "'></Table>";
        //                        tblDocument = new XmlDocument();
        //                        tblDocument.LoadXml(OutputXml1);
        //                        table = tblDocument.SelectSingleNode("//Table");
        //                    }
        //                    XmlNodeList words = currentNode.SelectNodes("Word");
        //                    XmlNode RowNode = tblDocument.CreateElement("Row");
        //                    XmlNode CellNode = tblDocument.CreateElement("Cell");
        //                    XmlNode ParaNode = tblDocument.CreateElement("Para");
        //                    for (int j = 0; j < words.Count; j++)
        //                    {
        //                        int llx = (int)Convert.ToDouble(words[j].Attributes["x1"].Value);
        //                        int lly = (int)Convert.ToDouble(words[j].Attributes["y1"].Value);
        //                        int ury = (int)Convert.ToDouble(words[j].Attributes["y"].Value);
        //                        int urx = (int)Convert.ToDouble(words[j].Attributes["x"].Value);

        //                        if (j > 0)
        //                        {
        //                            double diffrence = Convert.ToDouble(words[j].Attributes["x1"].Value) - Convert.ToDouble(words[j - 1].Attributes["x"].Value);
        //                            if (diffrence / AverageSpace >= 3)
        //                            {
        //                                CellNode = tblDocument.CreateElement("Cell");
        //                                ParaNode = tblDocument.CreateElement("Para");
        //                            }
        //                        }
        //                        XmlNode WordNode = tblDocument.CreateElement("Word");
        //                        XmlNode TextNode = tblDocument.CreateElement("Text");
        //                        XmlNode BoxNode = tblDocument.CreateElement("Box");
        //                        XmlAttribute lowerx = tblDocument.CreateAttribute("llx");
        //                        XmlAttribute uperrx = tblDocument.CreateAttribute("urx");
        //                        XmlAttribute lowery = tblDocument.CreateAttribute("lly");
        //                        XmlAttribute uperry = tblDocument.CreateAttribute("ury");

        //                        XmlAttribute fontsize = tblDocument.CreateAttribute("fontsize");
        //                        XmlAttribute font = tblDocument.CreateAttribute("font");
        //                        XmlAttribute fonttype = tblDocument.CreateAttribute("fonttype");
        //                        XmlAttribute height = tblDocument.CreateAttribute("height");

        //                        lowerx.Value = words[j].Attributes["x1"].Value;
        //                        uperrx.Value = words[j].Attributes["x"].Value;
        //                        lowery.Value = words[j].Attributes["y1"].Value;
        //                        uperry.Value = words[j].Attributes["y"].Value;
        //                        BoxNode.Attributes.Append(lowerx);
        //                        BoxNode.Attributes.Append(uperrx);
        //                        BoxNode.Attributes.Append(lowery);
        //                        BoxNode.Attributes.Append(uperry);

        //                        fontsize.Value = words[j].Attributes["fontsize"].Value;
        //                        font.Value = words[j].Attributes["font"].Value;
        //                        fonttype.Value = words[j].Attributes["fonttype"].Value;
        //                        height.Value = Convert.ToString(lineHeight);
        //                        BoxNode.Attributes.Append(fontsize);
        //                        BoxNode.Attributes.Append(font);
        //                        BoxNode.Attributes.Append(fonttype);
        //                        BoxNode.Attributes.Append(height);
        //                        //CellNode.Attributes.Append(lowerx);

        //                        TextNode.InnerText = words[j].InnerText;
        //                        WordNode.AppendChild(BoxNode);
        //                        WordNode.AppendChild(TextNode);
        //                        ParaNode.AppendChild(WordNode);
        //                        CellNode.AppendChild(ParaNode);
        //                        RowNode.AppendChild(CellNode);
        //                    }
        //                    LinesPassed.Add(currentNode);
        //                    table.AppendChild(RowNode);
        //                    firstLine = false;

        //                    i += 1;

        //                    currentNode = listLines[i] != null ? listLines[i] : null;

        //                    if (isTableStartingLine(currentNode, tblEndLineWords))
        //                    {
        //                        isProperTable = true;
        //                        isTableEndLine = true;

        //                        tableLines.Add(currentNode);
        //                        XmlNodeList wordsEndLine = currentNode.SelectNodes("Word");
        //                        XmlNode RowNodeEndLine = tblDocument.CreateElement("Row");
        //                        XmlNode CellNodeEndLine = tblDocument.CreateElement("Cell");
        //                        XmlNode ParaNodeEndLine = tblDocument.CreateElement("Para");
        //                        for (int j = 0; j < wordsEndLine.Count; j++)
        //                        {
        //                            int llx = (int)Convert.ToDouble(wordsEndLine[j].Attributes["x1"].Value);
        //                            int lly = (int)Convert.ToDouble(wordsEndLine[j].Attributes["y1"].Value);
        //                            int ury = (int)Convert.ToDouble(wordsEndLine[j].Attributes["y"].Value);
        //                            int urx = (int)Convert.ToDouble(wordsEndLine[j].Attributes["x"].Value);

        //                            if (j > 0)
        //                            {
        //                                double diffrence = Convert.ToDouble(wordsEndLine[j].Attributes["x1"].Value) -
        //                                    Convert.ToDouble(wordsEndLine[j - 1].Attributes["x"].Value);

        //                                if (diffrence / AverageSpace >= 3)
        //                                {
        //                                    CellNodeEndLine = tblDocument.CreateElement("Cell");
        //                                    ParaNodeEndLine = tblDocument.CreateElement("Para");
        //                                }
        //                            }
        //                            XmlNode WordNode = tblDocument.CreateElement("Word");
        //                            XmlNode TextNode = tblDocument.CreateElement("Text");
        //                            XmlNode BoxNode = tblDocument.CreateElement("Box");
        //                            XmlAttribute lowerx = tblDocument.CreateAttribute("llx");
        //                            XmlAttribute uperrx = tblDocument.CreateAttribute("urx");
        //                            XmlAttribute lowery = tblDocument.CreateAttribute("lly");
        //                            XmlAttribute uperry = tblDocument.CreateAttribute("ury");

        //                            XmlAttribute fontsize = tblDocument.CreateAttribute("fontsize");
        //                            XmlAttribute font = tblDocument.CreateAttribute("font");
        //                            XmlAttribute fonttype = tblDocument.CreateAttribute("fonttype");
        //                            XmlAttribute height = tblDocument.CreateAttribute("height");

        //                            lowerx.Value = wordsEndLine[j].Attributes["x1"].Value;
        //                            uperrx.Value = wordsEndLine[j].Attributes["x"].Value;
        //                            lowery.Value = wordsEndLine[j].Attributes["y1"].Value;
        //                            uperry.Value = wordsEndLine[j].Attributes["y"].Value;
        //                            BoxNode.Attributes.Append(lowerx);
        //                            BoxNode.Attributes.Append(uperrx);
        //                            BoxNode.Attributes.Append(lowery);
        //                            BoxNode.Attributes.Append(uperry);

        //                            fontsize.Value = wordsEndLine[j].Attributes["fontsize"].Value;
        //                            font.Value = wordsEndLine[j].Attributes["font"].Value;
        //                            fonttype.Value = wordsEndLine[j].Attributes["fonttype"].Value;
        //                            height.Value = Convert.ToString(lineHeight);
        //                            BoxNode.Attributes.Append(fontsize);
        //                            BoxNode.Attributes.Append(font);
        //                            BoxNode.Attributes.Append(fonttype);
        //                            BoxNode.Attributes.Append(height);
        //                            //CellNode.Attributes.Append(lowerx);

        //                            TextNode.InnerText = wordsEndLine[j].InnerText;
        //                            WordNode.AppendChild(BoxNode);
        //                            WordNode.AppendChild(TextNode);
        //                            ParaNodeEndLine.AppendChild(WordNode);
        //                            CellNodeEndLine.AppendChild(ParaNodeEndLine);
        //                            RowNodeEndLine.AppendChild(CellNodeEndLine);
        //                        }
        //                        LinesPassed.Add(currentNode);
        //                        table.AppendChild(RowNodeEndLine);
        //                    }
        //                } while (currentNode != null && (currentNode.Name.Equals("Line") && (!isTableEndLine)));
        //                //} while (currentNode != null && (currentNode.Name.Equals("Line") && (tableStartLine < linesList.Count)));

        //                if (table != null)
        //                {
        //                    if (isProperTable)
        //                    {
        //                        tables.Add(table);
        //                        XmlNode dummyTable = xmlDoc.CreateElement("Table");
        //                        XmlAttribute tableId = xmlDoc.CreateAttribute("id");
        //                        noOftables = pageno.Equals(ActivePage) ? noOftables + 1 : 1;
        //                        ActivePage = pageno;
        //                        tableId.Value = pageno + "_" + noOftables + "_Manual";
        //                        dummyTable.Attributes.Append(tableId);
        //                        dummyTable.InnerText = "Manual Table Insertion";
        //                        for (int k = 0; k < tableLines.Count; k++)
        //                        {
        //                            if (k == 0) tableLines[k].ParentNode.InsertBefore(dummyTable, tableLines[k]);
        //                            tableLines[k].ParentNode.RemoveChild(tableLines[k]);
        //                        }
        //                        tableLines = new List<XmlNode>();
        //                    }
        //                }
        //                //}
        //            }
        //        }
        //    }

        //    if (tables.Count > 0)
        //        xmlDoc.Save(temmXml);

        //    return tables;
        //}

        public void SaveInFinalXml(List<string> pdfJsLinesList, int page, int tableId, string rowType)
        {
            if (pdfJsLinesList == null) return;

            //<table id="2436" border="off" head-row="off">
            // <header>
            // </header>
            // <voice-description>
            // </voice-description>
            // <head-row>
            //   <head-col width="35">
            //   </head-col>
            //   <head-col width="65">
            //   </head-col>
            // </head-row>
            // <row>
            //  <col>Abayya </col>
            //  <col>a robe worn over a long skirt or loose trousers and shirt </col>
            // </row>
            // <caption>
            // </caption>
            //</table>

            string bookId = Convert.ToString(HttpContext.Current.Session["mainbook"]);
            string mainDirectoryPath = Common.GetDirectoryPath();
            string tableXmlPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table\\TableXmls\\Table_" + page + "_" + tableId + ".xml";

            if (!File.Exists(tableXmlPath)) return;

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(tableXmlPath);

            StringBuilder comnpleteLine = new StringBuilder();

            if (rowType.Equals("tableHeader"))
            {
                GetTableHeader(pdfJsLinesList, xDoc, "tableHeader");

                //XmlNode tableHeader = xDoc.SelectSingleNode("//header/descendant::ln");

                //table/caption/col/*[normalize-space() = 'Source: European Parliament, Directorate General for Internal Policies.']

                //foreach (var line in pdfJsLinesList)
                //{
                //    comnpleteLine.Append(line + " ");
                //}
                //tableHeader.InnerText = Convert.ToString(comnpleteLine);
                //comnpleteLine.Length = 0;
            }
            else
            {
                GetTableHeader(pdfJsLinesList, xDoc, "tableCaption");

                //XmlNode tableCaption = xDoc.SelectSingleNode("//caption/descendant::ln");
                //foreach (var line in pdfJsLinesList)
                //{
                //    comnpleteLine.Append(line + " ");
                //}
                //tableCaption.InnerText = Convert.ToString(comnpleteLine);
                //comnpleteLine.Length = 0;
            }

            xDoc.Save(tableXmlPath);
        }

        public void GetTableHeader(List<string> pdfJsLinesList, XmlDocument tableDoc, string rowType)
        {
            if (pdfJsLinesList != null && pdfJsLinesList.Count > 0)
            {
                List<XmlNode> matchedLinesList = new List<XmlNode>();

                var tableAllLines = tableDoc.SelectNodes("//ln").Cast<XmlNode>().Where(x => isHeaderStartingLine(x.InnerText.Trim(), pdfJsLinesList)).ToList();

                if (tableAllLines.Count > 0)
                {
                    matchedLinesList.Add(tableAllLines[0]);
                    SeletectedHeaderCapXml = tableAllLines[0];
                }

                if (rowType.Equals("tableHeader"))
                {
                    var tableHeaderLines = tableDoc.SelectNodes("//header/col/ln");

                    if (tableHeaderLines != null && tableHeaderLines.Count > 0)
                    {
                        SeletectedHeaderCapXml = tableHeaderLines[0];
                        tableHeaderLines[0].ParentNode.RemoveAll();
                    }

                    var headerNode = tableDoc.SelectSingleNode("//header/col");

                    foreach (XmlNode line in matchedLinesList)
                    {
                        headerNode.AppendChild(line);
                    }

                    var tableCaptionLines = tableDoc.SelectNodes("//caption/col");

                    if (tableCaptionLines != null && tableCaptionLines.Count > 0)
                    {
                        tableCaptionLines[0].AppendChild(SeletectedHeaderCapXml);
                    }
                }
                else
                {
                    var tableCaptionLines = tableDoc.SelectNodes("//caption/col/ln");

                    if (tableCaptionLines != null && tableCaptionLines.Count > 0)
                    {
                        SeletectedHeaderCapXml = tableCaptionLines[0];
                        tableCaptionLines[0].ParentNode.RemoveAll();
                    }
                    //else
                    //{
                    //    if (TableCaption!=null)
                    //        matchedLinesList = TableCaption.Cast<XmlNode>().ToList();
                    //}

                    var captionNode = tableDoc.SelectSingleNode("//caption/col");

                    foreach (XmlNode line in matchedLinesList)
                    {
                        captionNode.AppendChild(line);
                    }

                    var tableHeaderLines = tableDoc.SelectNodes("//header/col");

                    if (tableHeaderLines != null && tableHeaderLines.Count > 0)
                    {
                        tableHeaderLines[0].AppendChild(SeletectedHeaderCapXml);
                    }
                }
            }
        }

        private bool isHeaderStartingLine(string xmlLineInnerText, List<string> pdfJsLineWords)
        {
            if (string.IsNullOrEmpty(xmlLineInnerText) || pdfJsLineWords == null) return false;

            string pdfJsFirstLine = string.Join(" ", pdfJsLineWords.ToArray());

            List<string> pdfJsLineTempList = Regex.Split(pdfJsFirstLine, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
            List<string> xmlLineTempList = Regex.Split(xmlLineInnerText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

            if (pdfJsLineTempList.Count > 0 && xmlLineTempList.Count > 0)
            {
                int matchingPer = GetMatchingPercentage(pdfJsLineTempList[0].Trim(), xmlLineTempList[0].Trim());
                if (matchingPer < 50)
                    return false;
            }

            if (pdfJsFirstLine.Trim().Equals(xmlLineInnerText.Trim()))
                return true;

            string pdfJsText = RemoveWhiteSpace(RemoveSpecialChars(pdfJsFirstLine));
            string xmlText = RemoveWhiteSpace(RemoveSpecialChars(xmlLineInnerText));

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

        //public void GetTableHeader(List<string> pdfJsLinesList, XmlDocument tableDoc)
        //{
        //    if (pdfJsLinesList != null && pdfJsLinesList.Count > 0)
        //    {
        //        List<XmlNode> matchedLinesList = new List<XmlNode>();
        //        //foreach (string line in pdfJsLinesList)
        //        //{
        //        //var tableAllLines = tableDoc.SelectSingleNode("//ln").Cast<XmlNode>().Where(x => x.InnerText.Trim().Equals(line.Trim())).ToList();

        //        var tableAllLines = tableDoc.SelectNodes("//ln").Cast<XmlNode>().Where(x => isHeaderStartingLine(x.InnerText.Trim(), pdfJsLinesList)).ToList();

        //        if (tableAllLines.Count > 0)
        //            matchedLinesList.Add(tableAllLines[0]);
        //        //}

        //        var tableHeaderLines = tableDoc.SelectNodes("//header/col/ln");

        //        if (tableHeaderLines != null && tableHeaderLines.Count > 0)
        //        {
        //            tableHeaderLines[0].ParentNode.RemoveAll();
        //        }

        //        var headerNode = tableDoc.SelectSingleNode("//header/col");

        //        foreach (XmlNode line in matchedLinesList)
        //        {
        //            headerNode.AppendChild(line);
        //        }

        //        //foreach (XmlNode line in tableHeaderLines)
        //        //{
        //        //    //line.InnerXml = list[0].InnerXml;
        //        //    //var tableAllLines = tableDoc.SelectSingleNode("//ln").Cast<XmlNode>().Where(x => x.InnerText.Trim().Equals(line.Trim())).ToList();
        //        //    //list.AddRange(tableAllLines);
        //        //}
        //    }
        //}



        //old
        //public void SaveInFinalXml(List<string> pdfJsLinesList, int page, int tableId, string rowType)
        //{
        //    if (pdfJsLinesList == null) return;

        //    //<table id="2436" border="off" head-row="off">
        //    // <header>
        //    // </header>
        //    // <voice-description>
        //    // </voice-description>
        //    // <head-row>
        //    //   <head-col width="35">
        //    //   </head-col>
        //    //   <head-col width="65">
        //    //   </head-col>
        //    // </head-row>
        //    // <row>
        //    //  <col>Abayya </col>
        //    //  <col>a robe worn over a long skirt or loose trousers and shirt </col>
        //    // </row>
        //    // <caption>
        //    // </caption>
        //    //</table>

        //    string bookId = Convert.ToString(HttpContext.Current.Session["mainbook"]);
        //    string mainDirectoryPath = Common.GetDirectoryPath();
        //    string tableXmlPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table\\TableXmls\\Table_" + page + "_" + tableId + ".xml";

        //    if (!File.Exists(tableXmlPath)) return;

        //    XmlDocument xDoc = new XmlDocument();
        //    xDoc.Load(tableXmlPath);

        //    StringBuilder comnpleteLine = new StringBuilder();

        //    if (rowType.Equals("tableHeader"))
        //    {
        //        XmlNode tableHeader = xDoc.SelectSingleNode("//header/descendant::ln");

        //        //foreach (var line in pdfJsLinesList)
        //        //{
        //        //    comnpleteLine.Append(line + " ");
        //        //}
        //        //tableHeader.InnerText = Convert.ToString(comnpleteLine);
        //        //comnpleteLine.Length = 0;
        //    }
        //    else
        //    {
        //        XmlNode tableCaption = xDoc.SelectSingleNode("//caption/descendant::ln");
        //        foreach (var line in pdfJsLinesList)
        //        {
        //            comnpleteLine.Append(line + " ");
        //        }
        //        tableCaption.InnerText = Convert.ToString(comnpleteLine);
        //        comnpleteLine.Length = 0;
        //    }

        //    xDoc.Save(tableXmlPath);
        //}

        public bool SaveHeaderCaptionInXml(string linesList, string rowType)
        {
            if (string.IsNullOrEmpty(linesList)) return false;

            int pageNumber = GetCurrentPageNum();
            List<string> pdfJsLinesList = GetPdfJsSelectedLines(linesList, pageNumber);

            if (pdfJsLinesList == null || pdfJsLinesList.Count == 0) return false;

            SaveInFinalXml(pdfJsLinesList, pageNumber, 1, rowType);

            return false;
        }

        public void SetTableTags(List<XmlNode> tables, XmlDocument xmlDoc)
        {
            if (tables == null || tables.Count == 0) return;

            List<int> columnsList = new List<int>();

            foreach (XmlNode table in tables)
            {
                var rows = table.SelectNodes("Row");

                for (int i = 0; i < rows.Count; i++)
                {
                    if (rows[i].ChildNodes.Count == 1)
                    {

                    }
                }

                //var colList = columnsList.Distinct().OrderBy(x => x).ToList();

                //if (colList.Count > 0)
                //{

                //}
                //XmlNode TableHeader = xmlDoc.CreateElement("Row");


            }
        }

        private string GetImageCoords(XmlNode table, int page)
        {
            if (table == null) return "";

            double left = 0;
            double bottom = 0;
            string llx = "";
            string lly = "";
            string urx = "";
            string ury = "";
            string coordinates = "";

            string imgLly = "";

            string bookId = Convert.ToString(HttpContext.Current.Session["MainBook"]);
            string mainDirectoryPath = Common.GetDirectoryPath();
            string pdfDirectoryPath = mainDirectoryPath + "\\" + bookId;
            string pdfPath = pdfDirectoryPath + "\\DetectedTables\\" + bookId + "_actual.pdf";

            XmlNodeList lineList = table.SelectNodes("Row/Cell/Para/Word/Box");
            if (lineList.Count > 0)
            {
                //var rightValues = lineList.Cast<XmlNode>()
                //    .Select(node => Convert.ToDouble(node.Attributes["urx"].Value))
                //    .ToList();
                //urx = rightValues.Max().ToString();
                //var leftValues = lineList.Cast<XmlNode>()
                //    .Select(node => Convert.ToDouble(node.Attributes["llx"].Value))
                //    .ToList();
                //llx = leftValues.Min().ToString();
                //var rightYValues = lineList.Cast<XmlNode>()
                //   .Select(node => Convert.ToDouble(node.Attributes["ury"].Value))
                //   .ToList();
                //ury = rightYValues.Max().ToString();
                //var leftYValues = lineList.Cast<XmlNode>()
                //   .Select(node => Convert.ToDouble(node.Attributes["lly"].Value))
                //   .ToList();
                //lly = leftYValues.Min().ToString();

                var rightValues = lineList.Cast<XmlNode>()
                    .Select(node => Convert.ToDouble(node.Attributes["urx"].Value))
                    .ToList();
                urx = rightValues.Max().ToString();

                var leftValues = lineList.Cast<XmlNode>()
                    .Select(node => Convert.ToDouble(node.Attributes["llx"].Value))
                    .ToList();
                llx = leftValues.Min().ToString();

                var topValues = lineList.Cast<XmlNode>()
                   .Select(node => Convert.ToDouble(node.Attributes["ury"].Value))
                   .ToList();
                ury = topValues.Max().ToString();

                var bottomValues = lineList.Cast<XmlNode>()
                   .Select(node => Convert.ToDouble(node.Attributes["lly"].Value))
                   .ToList();
                lly = bottomValues.Min().ToString();
                imgLly = bottomValues.Max().ToString();
            }
            double width = Convert.ToDouble(urx) - Convert.ToDouble(llx);
            double height = Convert.ToDouble(ury) - Convert.ToDouble(lly);
            string croppedMargins = getCroppedMargins(page, pdfPath);
            List<string> tempValues = croppedMargins.Split(' ').ToList();

            if (tempValues != null)
            {
                if (tempValues.Count > 0)
                {
                    left = Math.Abs(Convert.ToDouble(tempValues[3]));
                    bottom = Math.Abs(Convert.ToDouble(tempValues[1]));
                }
            }
            coordinates = (Convert.ToDouble(llx) + left) + ":" + (Convert.ToDouble(imgLly) + bottom) + ":" + height + ":" + width;
            return coordinates;
        }

        private string getCroppedMargins(int pageNum, string mainPdfPath)
        {
            PdfReader reader = new PdfReader(File.ReadAllBytes(mainPdfPath));
            iTextSharp.text.Rectangle cropbox = reader.GetCropBox(pageNum);
            var box = reader.GetPageSizeWithRotation(pageNum);

            double top = (box.Top - cropbox.Top);
            double bottom = cropbox.Bottom;
            double right = (box.Right - cropbox.Right);
            double left = cropbox.Left;
            return Math.Round(top, 2) + " " + Math.Round(bottom, 2) + " " + Math.Round(right, 2) + " " + Math.Round(left, 2);
        }

        public bool SaveTablesInFinalXml(XmlNodeList tableColLinesList, List<string> tblStartLineWords, List<string> tblEndLineWords, string page, string xmlFilePath)
        {
            bool status = false;
            XmlDocument tblDocument = null;
            bool firstLine = true;
            XmlNode tableXml = null;

            if (!File.Exists(xmlFilePath)) return status;

            try
            {
                string bookId = Convert.ToString(HttpContext.Current.Session["MainBook"]);
                string mainDirectoryPath = Common.GetDirectoryPath();

                GlobalVar objGlobal = new GlobalVar();
                objGlobal.XMLPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\TaggingUntagged\\" + bookId + "-1.rhyw";
                objGlobal.LoadXml();

                List<XmlNode> tableLines = new List<XmlNode>();
                List<XmlNode> LinesPassed = new List<XmlNode>();
                List<string> tablLinesUry = new List<string>();
                bool isProperTable = false;
                bool isTableEnd = false;

                var xmlLines = objGlobal.PBPDocument.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes.Count > 0 &&
                                                                                              x.Attributes["page"] != null &&
                                                                                              x.Attributes["page"].Value == page).ToList();
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(xmlFilePath);
                XmlNode table = xDoc.SelectSingleNode("//table");


                //XmlNode dummyTable = objGlobal.PBPDocument.CreateElement("Table");
                
                XmlNode dummyTable = objGlobal.PBPDocument.CreateElement("table");

                XmlAttribute pageNum = objGlobal.PBPDocument.CreateAttribute("page");
                pageNum.Value = page;
                dummyTable.Attributes.Append(pageNum);

                var tableXmlDoc = CombineColumnWords(xDoc, table);

                if (tableXmlDoc == null || table == null)
                {
                    status = false;
                    return status;
                }

                tableXml = tableXmlDoc.SelectSingleNode("//table");
                dummyTable.InnerXml = tableXml.InnerXml;

                if (xmlLines == null || xmlLines.Count == 0) return false;

                for (int i = 0; i < xmlLines.Count; i++)
                {
                    if (!LinesPassed.Contains(xmlLines[i]))
                    {
                        //If current Line is a table line
                        if (isMatchedFinalXmlLine(xmlLines[i], tblStartLineWords, "starting"))
                        {
                            XmlNode currentNode = xmlLines[i];
                            do
                            {
                                tablLinesUry.Add(xmlLines[i].Attributes["top"].Value);
                                tableLines.Add(currentNode);
                                LinesPassed.Add(currentNode);

                                if (i < xmlLines.Count - 1)
                                    i += 1;

                                else if (i == xmlLines.Count - 1)
                                    isTableEnd = true;

                                currentNode = xmlLines[i] != null ? xmlLines[i] : null;

                                if (isMatchedFinalXmlLine(currentNode, tblEndLineWords, "ending"))
                                {
                                    tablLinesUry.Add(xmlLines[i].Attributes["top"].Value);
                                    tableLines.Add(currentNode);
                                    LinesPassed.Add(currentNode);
                                    isTableEnd = true;
                                }
                            } while (currentNode != null && (currentNode.Name.Equals("ln") && (!isTableEnd)));
                        }
                    }
                    if (isTableEnd) break;
                }

                foreach (XmlNode lineInTblCol in tableColLinesList)
                {
                    List<XmlNode> matchedTblLine = xmlLines.Where(x => x.InnerText.Trim().Equals(lineInTblCol.InnerText.Trim())).ToList();

                    if (matchedTblLine.Count > 0)
                    {
                        if (!tableLines.Contains(matchedTblLine[0]) && tablLinesUry.Contains(matchedTblLine[0].Attributes["top"].Value))
                        {
                            tableLines.AddRange(matchedTblLine);
                        }
                    }
                }

                for (int k = 0; k < tableLines.Count; k++)
                {
                    if (k == 0)
                    {
                        if (tableLines[k].PreviousSibling != null)
                        {
                            string paraName = tableLines[k].PreviousSibling.ParentNode.Name;
                            XmlNode previousLinesPara = objGlobal.PBPDocument.CreateElement(paraName);

                            XmlNode currentNode = tableLines[k].PreviousSibling;

                            while (currentNode != null)
                            {
                                previousLinesPara.AppendChild(currentNode);
                                currentNode = currentNode.PreviousSibling;
                            }
                            tableLines[k].ParentNode.ParentNode.InsertBefore(previousLinesPara, tableLines[k].ParentNode);
                        }
                        //if (tableLines[tableLines.Count - 1].ParentNode.ParentNode.NextSibling != null)
                        //{
                        //    string paraName = tableLines[tableLines.Count - 1].ParentNode.ParentNode.Name;
                        //    XmlNode nextLinesPara = objGlobal.PBPDocument.CreateElement(paraName);

                        //    XmlNode currentNode = tableLines[tableLines.Count - 1].ParentNode.ParentNode.NextSibling;

                        //    while (currentNode != null)
                        //    {
                        //        nextLinesPara.AppendChild(currentNode);
                        //        currentNode = currentNode.NextSibling;
                        //    }
                        //    tableLines[k].ParentNode.ParentNode.InsertBefore(tableLines[k].ParentNode, nextLinesPara);
                        //}

                        tableLines[k].ParentNode.ParentNode.InsertBefore(dummyTable, tableLines[k].ParentNode);
                    }

                    //var lineText = tableLines[k].InnerText;
                    //var childToRemove = tableLines[k].ParentNode.OuterXml;
                    //var testParent = tableLines[k].ParentNode.ParentNode;

                    if (tableLines[k].ParentNode != null)
                    {
                        if (tableLines[k].ParentNode.ParentNode != null)
                        {
                            if (tableLines[k].ParentNode.ParentNode.Name.Equals("body"))
                            {
                                tableLines[k].ParentNode.ParentNode.RemoveChild(tableLines[k].ParentNode);
                            }
                        }
                    }
                }

                objGlobal.SaveXml();
                status = true;

                return status;
            }
            catch (Exception ex)
            {
                return status;
            }
        }

        public bool SaveTablesInFinalXml11(List<string> linesList, List<string> tblStartLineWords, List<string> tblEndLineWords, string page)
        {
            if (linesList == null || linesList.Count == 0) return false;

            bool status = false;

            try
            {
                string bookId = Convert.ToString(HttpContext.Current.Session["MainBook"]);
                string mainDirectoryPath = Common.GetDirectoryPath();

                GlobalVar objGlobal = new GlobalVar();
                objGlobal.XMLPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\TaggingUntagged\\" + bookId + "-1.rhyw";
                objGlobal.LoadXml();

                List<XmlNode> tables = new List<XmlNode>();
                XmlNode table = null;
                XmlDocument tblDocument = null;
                int pageno = 0;
                List<XmlNode> tableLines = new List<XmlNode>();
                List<XmlNode> LinesPassed = new List<XmlNode>();
                bool isProperTable = false;
                int ActivePage = 0;
                int noOftables = 1;
                int tableStartLine = 0;

                var xmlLines = objGlobal.PBPDocument.SelectNodes("//ln").Cast<XmlNode>().Where(x => x.Attributes.Count > 0 &&
                                                                                              x.Attributes["page"] != null &&
                                                                                              x.Attributes["page"].Value == page).ToList();

                if (xmlLines == null || xmlLines.Count == 0) return false;

                for (int i = 0; i < xmlLines.Count; i++)
                {
                    if (!LinesPassed.Contains(xmlLines[i]))
                    {
                        //If current Line is a table line
                        if (isMatchedFinalXmlLine(xmlLines[i], tblStartLineWords, "starting"))
                        {
                            tableStartLine++;

                            XmlNode currentNode = xmlLines[i];
                            bool firstLine = true;
                            double Linediffrence = 0;
                            //pageno = Convert.ToInt32(xmlLines[i].SelectSingleNode("ancestor::Page").Attributes["number"].Value);
                            //Create table and add rows till the end of abnormal rows...
                            do
                            {
                                tableLines.Add(currentNode);
                                //if first abnormal row then start creating a new table
                                if (firstLine)
                                {
                                    isProperTable = false;
                                    string OutputXml1 = "<?xml version=\"1.0\"?><Table page='" + page + "'></Table>";
                                    tblDocument = new XmlDocument();
                                    tblDocument.LoadXml(OutputXml1);
                                    table = tblDocument.SelectSingleNode("//Table");
                                }

                                if (isProperTable)
                                {
                                    tables.Add(table);
                                    for (int k = 0; k < tableLines.Count; k++)
                                    {
                                        if (k == 0) tableLines[k].ParentNode.InsertBefore(table, tableLines[k]);
                                        tableLines[k].ParentNode.RemoveChild(tableLines[k]);
                                    }
                                    tableLines = new List<XmlNode>();

                                    isProperTable = false;
                                    string OutputXml = "<?xml version=\"1.0\"?><Table page='" + page + "'></Table>";
                                    tblDocument = new XmlDocument();
                                    tblDocument.LoadXml(OutputXml);
                                    table = tblDocument.SelectSingleNode("//Table");
                                }

                                XmlNodeList words = currentNode.SelectNodes("Word");
                                XmlNode RowNode = tblDocument.CreateElement("Row");
                                XmlNode CellNode = tblDocument.CreateElement("Cell");
                                XmlNode ParaNode = tblDocument.CreateElement("Para");
                                for (int j = 0; j < words.Count; j++)
                                {
                                    int llx = (int)Convert.ToDouble(words[j].Attributes["x1"].Value);
                                    int lly = (int)Convert.ToDouble(words[j].Attributes["y1"].Value);
                                    int ury = (int)Convert.ToDouble(words[j].Attributes["y"].Value);
                                    int urx = (int)Convert.ToDouble(words[j].Attributes["x"].Value);

                                    //if (j > 0)
                                    //{
                                    //    double diffrence = Convert.ToDouble(words[j].Attributes["x1"].Value) - Convert.ToDouble(words[j - 1].Attributes["x"].Value);
                                    //    if (diffrence / AverageSpace >= 3)
                                    //    {
                                    //        CellNode = tblDocument.CreateElement("Cell");
                                    //        ParaNode = tblDocument.CreateElement("Para");
                                    //    }
                                    //}
                                    XmlNode WordNode = tblDocument.CreateElement("Word");
                                    XmlNode TextNode = tblDocument.CreateElement("Text");
                                    XmlNode BoxNode = tblDocument.CreateElement("Box");
                                    XmlAttribute lowerx = tblDocument.CreateAttribute("llx");
                                    XmlAttribute uperrx = tblDocument.CreateAttribute("urx");
                                    XmlAttribute lowery = tblDocument.CreateAttribute("lly");
                                    XmlAttribute uperry = tblDocument.CreateAttribute("ury");
                                    lowerx.Value = words[j].Attributes["x1"].Value;
                                    uperrx.Value = words[j].Attributes["x"].Value;
                                    lowery.Value = words[j].Attributes["y1"].Value;
                                    uperry.Value = words[j].Attributes["y"].Value;
                                    BoxNode.Attributes.Append(lowerx);
                                    BoxNode.Attributes.Append(uperrx);
                                    BoxNode.Attributes.Append(lowery);
                                    BoxNode.Attributes.Append(uperry);

                                    TextNode.InnerText = words[j].InnerText;
                                    WordNode.AppendChild(BoxNode);
                                    WordNode.AppendChild(TextNode);
                                    ParaNode.AppendChild(WordNode);
                                    CellNode.AppendChild(ParaNode);
                                    RowNode.AppendChild(CellNode);
                                }
                                LinesPassed.Add(currentNode);
                                table.AppendChild(RowNode);
                                firstLine = false;
                                currentNode = currentNode.NextSibling != null ? currentNode.NextSibling : null;
                                if (isMatchedFinalXmlLine(currentNode, tblEndLineWords, "ending"))
                                {
                                    isProperTable = true;

                                    tableLines.Add(currentNode);
                                    XmlNodeList wordsEndLine = currentNode.SelectNodes("Word");
                                    XmlNode RowNodeEndLine = tblDocument.CreateElement("Row");
                                    XmlNode CellNodeEndLine = tblDocument.CreateElement("Cell");
                                    XmlNode ParaNodeEndLine = tblDocument.CreateElement("Para");
                                    for (int j = 0; j < wordsEndLine.Count; j++)
                                    {
                                        int llx = (int)Convert.ToDouble(wordsEndLine[j].Attributes["x1"].Value);
                                        int lly = (int)Convert.ToDouble(wordsEndLine[j].Attributes["y1"].Value);
                                        int ury = (int)Convert.ToDouble(wordsEndLine[j].Attributes["y"].Value);
                                        int urx = (int)Convert.ToDouble(wordsEndLine[j].Attributes["x"].Value);

                                        //if (j > 0)
                                        //{
                                        //    double diffrence = Convert.ToDouble(wordsEndLine[j].Attributes["x1"].Value) -
                                        //        Convert.ToDouble(wordsEndLine[j - 1].Attributes["x"].Value);

                                        //    if (diffrence / AverageSpace >= 3)
                                        //    {
                                        //        CellNodeEndLine = tblDocument.CreateElement("Cell");
                                        //        ParaNodeEndLine = tblDocument.CreateElement("Para");
                                        //    }
                                        //}
                                        XmlNode WordNode = tblDocument.CreateElement("Word");
                                        XmlNode TextNode = tblDocument.CreateElement("Text");
                                        XmlNode BoxNode = tblDocument.CreateElement("Box");
                                        XmlAttribute lowerx = tblDocument.CreateAttribute("llx");
                                        XmlAttribute uperrx = tblDocument.CreateAttribute("urx");
                                        XmlAttribute lowery = tblDocument.CreateAttribute("lly");
                                        XmlAttribute uperry = tblDocument.CreateAttribute("ury");
                                        lowerx.Value = wordsEndLine[j].Attributes["x1"].Value;
                                        uperrx.Value = wordsEndLine[j].Attributes["x"].Value;
                                        lowery.Value = wordsEndLine[j].Attributes["y1"].Value;
                                        uperry.Value = wordsEndLine[j].Attributes["y"].Value;
                                        BoxNode.Attributes.Append(lowerx);
                                        BoxNode.Attributes.Append(uperrx);
                                        BoxNode.Attributes.Append(lowery);
                                        BoxNode.Attributes.Append(uperry);

                                        TextNode.InnerText = wordsEndLine[j].InnerText;
                                        WordNode.AppendChild(BoxNode);
                                        WordNode.AppendChild(TextNode);
                                        ParaNodeEndLine.AppendChild(WordNode);
                                        CellNodeEndLine.AppendChild(ParaNodeEndLine);
                                        RowNodeEndLine.AppendChild(CellNodeEndLine);
                                    }
                                    LinesPassed.Add(currentNode);
                                    table.AppendChild(RowNodeEndLine);
                                }
                            } while (currentNode != null && (currentNode.Name.Equals("Line") && (!isMatchedFinalXmlLine(currentNode, tblEndLineWords, "ending"))));

                            if (table != null)
                            {
                                if (isProperTable)
                                {
                                    ////tables.Add(table);
                                    ////XmlNode dummyTable = xmlDoc.CreateElement("Table");
                                    ////XmlAttribute tableId = xmlDoc.CreateAttribute("id");
                                    ////noOftables = pageno.Equals(ActivePage) ? noOftables + 1 : 1;
                                    ////ActivePage = pageno;
                                    ////tableId.Value = pageno + "_" + noOftables + "_Manual";
                                    ////dummyTable.Attributes.Append(tableId);
                                    ////dummyTable.InnerText = "Manual Table Insertion";

                                    for (int k = 0; k < tableLines.Count; k++)
                                    {
                                        if (k == 0) tableLines[k].ParentNode.InsertBefore(table, tableLines[k]);
                                        tableLines[k].ParentNode.RemoveChild(tableLines[k]);
                                    }
                                    tableLines = new List<XmlNode>();
                                }
                            }
                        }
                    }
                }
                objGlobal.SaveXml();
                status = true;

                return status;
            }
            catch (Exception)
            {
                return status;
            }
        }

        public int AvgWordSpace(string tempFilePath)
        {
            if (!File.Exists(tempFilePath)) return 0;

            int AverageSpace = 0;

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(tempFilePath);
            XmlNodeList pages = xmlDoc.SelectNodes("//Page");
            int checkPages = 1;
            List<double> chracterwidhts = new List<double>();
            foreach (XmlNode page in pages)
            {
                XmlNodeList words = page.SelectNodes("descendant::Word");
                if (words != null)
                {
                    var Spaces = words.Cast<XmlNode>().Select(node => node.NextSibling == null ? 0
                            : Convert.ToDouble(node.NextSibling.Attributes["x1"].Value) >
                              Convert.ToDouble(node.Attributes["x"].Value)
                                ? Convert.ToDouble(node.NextSibling.Attributes["x1"].Value) -
                                  Convert.ToDouble(node.Attributes["x"].Value)
                                : 0).ToList();
                    Spaces.RemoveAll(equaltoZerro);
                    if (Spaces.Count > 0)
                    {
                        AverageSpace = Convert.ToInt32(Spaces.Average());

                        while (ContainsGreaterValue(Spaces, AverageSpace))
                        {
                            Spaces.RemoveAll(greaterthanAverage);

                            if (Spaces.Count > 0 && Spaces.Average() >= 1)
                                AverageSpace = Convert.ToInt32(Spaces.Average());
                        }
                        if (Spaces.Count > 0 && Spaces.Average() >= 1)
                            AverageSpace = Convert.ToInt32(Spaces.Average());
                        checkPages++;
                    }
                    if (checkPages > 6) break;
                }
            }

            return AverageSpace;
        }

        public int AvgLineSpace(string tempFilePath)
        {
            try
            {
                int AverageVeticalSpace = 0;
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(tempFilePath);
                XmlNodeList pages = xmlDoc.SelectNodes("//Page");
                int checkPages = 1;
                List<double> chracterwidhts = new List<double>();
                foreach (XmlNode page in pages)
                {
                    XmlNodeList lines = page.SelectNodes("descendant::Line");

                    if (lines != null)
                    {
                        var tempSpaces = lines.Cast<XmlNode>().Select(node => node.NextSibling).ToList();

                        var Spaces = lines.Cast<XmlNode>().Select(node => node.NextSibling == null ? 0 :
                                Convert.ToDouble(node.Attributes["y1"].Value) > Convert.ToDouble(node.NextSibling.Attributes["y"].Value) ?
                                Convert.ToDouble(node.Attributes["y1"].Value) - Convert.ToDouble(node.NextSibling.Attributes["y"].Value) : 0).ToList();
                        Spaces.RemoveAll(equaltoZerro);
                        if (Spaces.Count > 0)
                        {
                            AverageVeticalSpace = Convert.ToInt32(Spaces.Average());

                            while (ContainsGreaterLineDistance(Spaces, AverageVeticalSpace))
                            {
                                Spaces.RemoveAll(greaterthanNormalDistance);
                                if (Spaces.Count > 0)
                                    AverageVeticalSpace = Convert.ToInt32(Spaces.Average());
                            }
                            if (Spaces.Count > 0)
                                AverageVeticalSpace = Convert.ToInt32(Spaces.Average());
                        }
                        checkPages++;
                        if (checkPages > 6) break;
                    }
                }

                return AverageVeticalSpace;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        private static bool equaltoZerro(double val)
        {
            return val.Equals(0);
        }

        private static bool greaterthanAverage(double val)
        {
            int AverageSpace = Convert.ToInt32(HttpContext.Current.Session["AverageSpace"]);

            //if (AverageSpace < 1) return false;

            return Math.Round(val) > AverageSpace;
        }

        public bool ContainsGreaterValue(List<double> listValues, int AverageSpace)
        {
            int result = listValues.FindIndex(value => Math.Round(value) / AverageSpace > 3);
            return result >= 0 ? true : false;
        }

        public bool ContainsGreaterLineDistance(List<double> listValues, int AverageVeticalSpace)
        {
            int result = listValues.FindIndex(value => Math.Round(value) / AverageVeticalSpace > 2);
            return result >= 0 ? true : false;
        }

        private static bool greaterthanNormalDistance(double val)
        {
            int AverageVeticalSpace = Convert.ToInt32(HttpContext.Current.Session["AverageVeticalSpace"]);
            return Math.Round(val) > AverageVeticalSpace;
        }

        public void NormalAndIndentX(XmlDocument XmlDoc, ref int NormalX, ref int NormalIndentX)
        {
            XmlNodeList pages = XmlDoc.SelectNodes("//Page");
            //int checkPages = 1;                
            //foreach (XmlNode page in pages)
            //{
            //    XmlNodeList words = page.SelectNodes("descendant::Line");
            //    List<Double> x1Values = words.Cast<XmlNode>()
            //        .Select(node => Convert.ToDouble(node.Attributes["x1"].Value)).ToList();
            //    x1Values.RemoveAll(equaltoZerro);

            //   //


            var code = XmlDoc.SelectNodes("descendant::Line").Cast<XmlNode>().GroupBy(z => z.Attributes["x1"] == null ? "" : z.Attributes["x1"].Value).Select(group => new
            {
                value = group.Key,
                Count = group.Count()
            })
        .OrderByDescending(z => z.Count).FirstOrDefault();

            if (code != null)
                NormalX = Convert.ToInt32(Convert.ToDouble(code.value));
            ////


            //if (x1Values.Count > 0)
            //{
            //    NormalX = Convert.ToInt32(x1Values.Average());
            //    while (isGreaterthanNormalX(x1Values))
            //    {
            //        x1Values.RemoveAll(greaterthanNormalX);
            //        NormalX = Convert.ToInt32(x1Values.Average());
            //    }
            //    NormalX = Convert.ToInt32(x1Values.Average());
            //}
            //checkPages++;
            //if (checkPages > 6) break;
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

        private bool isTableStartingLine(XmlNode xmlLine, List<string> pdfJsLineWords)
        {
            if (xmlLine == null || pdfJsLineWords == null) return false;

            StringBuilder sbXmlLine = new StringBuilder();

            XmlNodeList xmlLineWord = xmlLine.SelectNodes("descendant::Word");

            for (int i = 0; i < xmlLineWord.Count; i++)
            {
                sbXmlLine.Append(xmlLineWord[i].InnerText + " ");
            }

            string pdfJsFirstLine = string.Join(" ", pdfJsLineWords.ToArray());

            List<string> pdfJsLineTempList = Regex.Split(pdfJsFirstLine, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
            List<string> xmlLineTempList = Regex.Split(Convert.ToString(sbXmlLine), @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

            if (pdfJsLineTempList.Count > 0 && xmlLineTempList.Count > 0)
            {
                int matchingPer = GetMatchingPercentage(pdfJsLineTempList[0].Trim(), xmlLineTempList[0].Trim());
                if (matchingPer < 50)
                    return false;
            }

            if (pdfJsFirstLine.Trim().Equals(Convert.ToString(sbXmlLine).Trim()))
                return true;

            string pdfJsText = RemoveWhiteSpace(RemoveSpecialChars(pdfJsFirstLine));
            string xmlText = RemoveWhiteSpace(RemoveSpecialChars(Convert.ToString(sbXmlLine)));

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

        //private bool isTableEndingLine(XmlNode xmlLine, List<string> pdfJsLineWords)
        //{
        //    if (xmlLine == null || pdfJsLineWords == null) return false;

        //    StringBuilder sbXmlCurrentLine = new StringBuilder();
        //    StringBuilder sbXmlPrevLine = new StringBuilder();

        //    XmlNodeList xmlCurrentLineWord = xmlLine.SelectNodes("descendant::Word");
        //    if (xmlCurrentLineWord != null)
        //    {
        //        foreach (XmlNode currentLineWord in xmlCurrentLineWord)
        //        {
        //            sbXmlCurrentLine.Append(currentLineWord.InnerText + " ");
        //        }
        //    }

        //    if (xmlLine.PreviousSibling != null)
        //    {
        //        XmlNodeList xmlPrevLineWord = xmlLine.PreviousSibling.SelectNodes("descendant::Word");

        //        if (xmlPrevLineWord != null)
        //        {
        //            foreach (XmlNode prevLineWord in xmlPrevLineWord)
        //            {
        //                sbXmlPrevLine.Append(prevLineWord.InnerText + " ");
        //            }
        //        }
        //    }

        //    string pdfJsFirstLine = string.Join(" ", pdfJsLineWords.ToArray());

        //    List<string> pdfJsLineTempList = Regex.Split(pdfJsFirstLine, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
        //    List<string> xmlLineTempList = Regex.Split(Convert.ToString(sbXmlCurrentLine), @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

        //    if (pdfJsLineTempList.Count > 0 && xmlLineTempList.Count > 0)
        //    {
        //        int matchingPer = GetMatchingPercentage(pdfJsLineTempList[0].Trim(), xmlLineTempList[0].Trim());
        //        if (matchingPer < 50)
        //            return false;
        //    }

        //    if (pdfJsFirstLine.Trim().Equals(Convert.ToString(sbXmlCurrentLine).Trim()))
        //        return true;

        //    string pdfJsText = RemoveWhiteSpace(RemoveSpecialChars(pdfJsFirstLine));
        //    string xmlText = RemoveWhiteSpace(RemoveSpecialChars(Convert.ToString(sbXmlCurrentLine)));

        //    string finalPdfJsText = "";
        //    string finalxmlText = "";

        //    if (pdfJsText.Length == xmlText.Length)
        //    {
        //        if (pdfJsText != xmlText)
        //        {
        //            int matchingPer = GetMatchingPercentage(pdfJsText, xmlText);
        //            if (matchingPer >= 80)
        //                return true;
        //        }
        //        else
        //            return true;
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
        //        return true;
        //    }
        //    else
        //    {
        //        if (pdfJsLineTempList.Count > 3 && xmlLineTempList.Count > 3)
        //        {
        //            if (pdfJsLineTempList[0].Trim().Equals(xmlLineTempList[0].Trim()))
        //            {
        //                if (pdfJsLineTempList[1].Trim().Equals(xmlLineTempList[1].Trim()))
        //                {
        //                    if (pdfJsLineTempList[2].Trim().Equals(xmlLineTempList[2].Trim()))
        //                    {
        //                        return true;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //    return false;
        //}

        //old one
        ////private bool isMatchedLine(XmlNode xmlLine, List<string> pdfJsLineWords, string linePosition)
        ////{
        ////    if (xmlLine == null || pdfJsLineWords == null) return false;

        ////    XmlNodeList xmlLineWord = xmlLine.SelectNodes("descendant::Word");
        ////    StringBuilder sbWords = new StringBuilder();

        ////    if (linePosition.Equals("starting"))
        ////    {
        ////        for (int i = 0; i < xmlLineWord.Count; i++)
        ////        {
        ////            sbWords.Append(xmlLineWord[i].InnerText + " ");
        ////            if (i == pdfJsLineWords.Count - 1) break;
        ////        }
        ////    }
        ////    else
        ////    {
        ////        if (pdfJsLineWords.Count <= xmlLineWord.Count)
        ////        {
        ////            if (pdfJsLineWords.Count == 1)
        ////                sbWords.Append(xmlLineWord[xmlLineWord.Count - 1].InnerText + " ");

        ////            else if (pdfJsLineWords.Count == 2)
        ////                sbWords.Append(xmlLineWord[xmlLineWord.Count - 2].InnerText + " " + xmlLineWord[xmlLineWord.Count - 1].InnerText + " ");

        ////            else if (pdfJsLineWords.Count >= 3)
        ////                sbWords.Append(xmlLineWord[xmlLineWord.Count - 3].InnerText + " " + xmlLineWord[xmlLineWord.Count - 2].InnerText + " " +
        ////                               xmlLineWord[xmlLineWord.Count - 1].InnerText + " ");
        ////        }
        ////        else if (xmlLineWord.Count <= pdfJsLineWords.Count)
        ////        {
        ////            if (xmlLineWord.Count == 1)
        ////                sbWords.Append(xmlLineWord[xmlLineWord.Count - 1].InnerText + " ");

        ////            else if (xmlLineWord.Count == 2)
        ////                sbWords.Append(xmlLineWord[xmlLineWord.Count - 2].InnerText + " " + xmlLineWord[xmlLineWord.Count - 1].InnerText + " ");

        ////            else if (xmlLineWord.Count >= 3)
        ////                sbWords.Append(xmlLineWord[xmlLineWord.Count - 3].InnerText + " " + xmlLineWord[xmlLineWord.Count - 2].InnerText + " " +
        ////                               xmlLineWord[xmlLineWord.Count - 1].InnerText + " ");
        ////        }
        ////    }

        ////    string ocrLine = string.Join(" ", pdfJsLineWords.ToArray());

        ////    string pdfJsText = RemoveWhiteSpace(ocrLine.Trim());
        ////    string xmlText = RemoveWhiteSpace(Convert.ToString(sbWords).Trim());

        ////    string finalPdfJsText = "";
        ////    string finalxmlText = "";

        ////    if (pdfJsText.Length == xmlText.Length)
        ////    {
        ////        finalPdfJsText = pdfJsText;
        ////        finalxmlText = xmlText;
        ////    }
        ////    else if (pdfJsText.Length > xmlText.Length)
        ////    {
        ////        if (linePosition.Equals("starting"))
        ////        {
        ////            finalPdfJsText = pdfJsText.Substring(0, xmlText.Length);
        ////            finalxmlText = xmlText;
        ////        }
        ////        else
        ////        {
        ////            finalPdfJsText = pdfJsText.Substring(pdfJsText.Length - xmlText.Length, xmlText.Length);
        ////            finalxmlText = xmlText;
        ////        }
        ////    }
        ////    else if (xmlText.Length > pdfJsText.Length)
        ////    {
        ////        if (linePosition.Equals("starting"))
        ////        {
        ////            finalPdfJsText = pdfJsText;
        ////            finalxmlText = xmlText.Substring(0, pdfJsText.Length);
        ////        }
        ////        else
        ////        {
        ////            finalPdfJsText = pdfJsText;
        ////            finalxmlText = xmlText.Substring(xmlText.Length - pdfJsText.Length, pdfJsText.Length);
        ////        }
        ////    }

        ////    if (string.IsNullOrEmpty(finalPdfJsText) || string.IsNullOrEmpty(finalxmlText)) return false;

        ////    if (finalPdfJsText.ToLower().Equals(finalxmlText.ToLower()))
        ////        return true;

        ////    return false;
        ////}

        private bool isMatchedFinalXmlLine(XmlNode xmlLine, List<string> pdfJsLineWords, string linePosition)
        {
            if (xmlLine == null || pdfJsLineWords == null) return false;

            List<string> xmlLineWord = Regex.Split(xmlLine.InnerText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
            if (xmlLineWord == null || xmlLineWord.Count == 0) return false;

            StringBuilder sbWords = new StringBuilder();

            if (linePosition.Equals("starting"))
            {
                for (int i = 0; i < xmlLineWord.Count; i++)
                {
                    sbWords.Append(xmlLineWord[i] + " ");
                    if (i == pdfJsLineWords.Count - 1) break;
                }
            }
            else
            {
                if (pdfJsLineWords.Count <= xmlLineWord.Count)
                {
                    if (pdfJsLineWords.Count == 1)
                        sbWords.Append(xmlLineWord[xmlLineWord.Count - 1] + " ");

                    else if (pdfJsLineWords.Count == 2)
                        sbWords.Append(xmlLineWord[xmlLineWord.Count - 2] + " " + xmlLineWord[xmlLineWord.Count - 1] + " ");

                    else if (pdfJsLineWords.Count >= 3)
                        sbWords.Append(xmlLineWord[xmlLineWord.Count - 3] + " " + xmlLineWord[xmlLineWord.Count - 2] + " " +
                                       xmlLineWord[xmlLineWord.Count - 1] + " ");
                }
                else if (xmlLineWord.Count <= pdfJsLineWords.Count)
                {
                    if (xmlLineWord.Count == 1)
                        sbWords.Append(xmlLineWord[xmlLineWord.Count - 1] + " ");

                    else if (xmlLineWord.Count == 2)
                        sbWords.Append(xmlLineWord[xmlLineWord.Count - 2] + " " + xmlLineWord[xmlLineWord.Count - 1] + " ");

                    else if (xmlLineWord.Count >= 3)
                        sbWords.Append(xmlLineWord[xmlLineWord.Count - 3] + " " + xmlLineWord[xmlLineWord.Count - 2] + " " +
                                       xmlLineWord[xmlLineWord.Count - 1] + " ");
                }
            }

            string ocrLine = string.Join(" ", pdfJsLineWords.ToArray());

            string pdfJsText = RemoveWhiteSpace(ocrLine.Trim());
            string xmlText = RemoveWhiteSpace(Convert.ToString(sbWords).Trim());

            string finalPdfJsText = "";
            string finalxmlText = "";

            if (pdfJsText.Length == xmlText.Length)
            {
                finalPdfJsText = pdfJsText;
                finalxmlText = xmlText;
            }
            else if (pdfJsText.Length > xmlText.Length)
            {
                if (linePosition.Equals("starting"))
                {
                    finalPdfJsText = pdfJsText.Substring(0, xmlText.Length);
                    finalxmlText = xmlText;
                }
                else
                {
                    finalPdfJsText = pdfJsText.Substring(pdfJsText.Length - xmlText.Length, xmlText.Length);
                    finalxmlText = xmlText;
                }
            }
            else if (xmlText.Length > pdfJsText.Length)
            {
                if (linePosition.Equals("starting"))
                {
                    finalPdfJsText = pdfJsText;
                    finalxmlText = xmlText.Substring(0, pdfJsText.Length);
                }
                else
                {
                    finalPdfJsText = pdfJsText;
                    finalxmlText = xmlText.Substring(xmlText.Length - pdfJsText.Length, pdfJsText.Length);
                }
            }

            if (string.IsNullOrEmpty(finalPdfJsText) || string.IsNullOrEmpty(finalxmlText)) return false;

            if (finalPdfJsText.ToLower().Equals(finalxmlText.ToLower()))
                return true;

            return false;
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

        //public void CreateXls(XmlNode table)
        //{
        //    if (table == null) return;

        //    string bookId = Convert.ToString(HttpContext.Current.Session["MainBook"]);

        //    int noOftables = 1;

        //    string mainDirectoryPath = Common.GetDirectoryPath();

        //    string directoryPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table\\";
        //    string pdfPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + ".pdf";

        //    if (!Directory.Exists(directoryPath + "\\FinalTableTasks"))
        //        Directory.CreateDirectory(directoryPath + "\\FinalTableTasks");

        //    if (!Directory.Exists(directoryPath + "\\TableXmls"))
        //        Directory.CreateDirectory(directoryPath + "\\TableXmls");

        //    //for (int i = 0; i < tables.Count; i++)
        //    //{
        //    //    if (tables[i] != null)
        //    //    {
        //            //int pageno = Convert.ToInt32(table.Attributes["page"].Value);
        //            //noOftables = i + 1;

        //    string pageno = GetCurrentPageNum();

        //            //ExportToXml(table, directoryPath + "\\TableXmls\\Table_" + pageno + "_" + noOftables + ".xml", Convert.ToInt32(pageno));

        //            ExporttoExcel(table, directoryPath + "\\FinalTableTasks\\Table_" + pageno + "_" + noOftables + ".xlsx");
        //            if (noOftables == 1)
        //                ExtractPage(pdfPath, directoryPath + "\\FinalTableTasks\\Table_" + pageno + "_" + noOftables + ".pdf", pageno);
        //    //    }
        //    //}
        //}

        public void CreateXls(XmlNode table)
        {
            if (table == null) return;

            string bookId = Convert.ToString(HttpContext.Current.Session["MainBook"]);

            int noOftables = 1;

            string mainDirectoryPath = Common.GetDirectoryPath();

            string directoryPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table\\";
            string pdfPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + ".pdf";

            if (!Directory.Exists(directoryPath + "\\FinalTableTasks"))
                Directory.CreateDirectory(directoryPath + "\\FinalTableTasks");

            if (!Directory.Exists(directoryPath + "\\TableXmls"))
                Directory.CreateDirectory(directoryPath + "\\TableXmls");

            //for (int i = 0; i < tables.Count; i++)
            //{
            //    if (tables[i] != null)
            //    {
            //int pageno = Convert.ToInt32(table.Attributes["page"].Value);
            //noOftables = i + 1;

            int pageno = GetCurrentPageNum();

            //ExportToXml(table, directoryPath + "\\TableXmls\\Table_" + pageno + "_" + noOftables + ".xml", Convert.ToInt32(pageno));

            ExporttoExcel(table, directoryPath + "\\FinalTableTasks\\Table_" + pageno + "_" + noOftables + ".xlsx");
            if (noOftables == 1)
                ExtractPage(pdfPath, directoryPath + "\\FinalTableTasks\\Table_" + pageno + "_" + noOftables + ".pdf", pageno);
            //    }
            //}
        }

        public void SaveAsXml(XmlNode table)
        {
            if (table == null) return;

            string bookId = Convert.ToString(HttpContext.Current.Session["MainBook"]);

            int noOftables = 1;

            string mainDirectoryPath = Common.GetDirectoryPath();

            string directoryPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table\\";
            string pdfPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + ".pdf";

            if (!Directory.Exists(directoryPath + "\\FinalTableTasks"))
                Directory.CreateDirectory(directoryPath + "\\FinalTableTasks");

            if (!Directory.Exists(directoryPath + "\\TableXmls"))
                Directory.CreateDirectory(directoryPath + "\\TableXmls");

            //for (int i = 0; i < tables.Count; i++)
            //{
            //    if (tables[i] != null)
            //    {
            int pageno = Convert.ToInt32(table.Attributes["page"].Value);
            //noOftables = i + 1;

            ExportToXml(table, directoryPath + "\\TableXmls\\Table_" + pageno + "_" + noOftables + ".xml", pageno);
            //    }
            //}
        }

        public void ExportToXml(XmlNode table, string fileName, int pageno)
        {
            try
            {
                string OutputXml = "<?xml version=\"1.0\"?><Table page='" + pageno + "'></Table>";
                XmlDocument tblDocument = new XmlDocument();
                tblDocument.LoadXml(OutputXml);
                //XmlNode tableXml = tblDocument.SelectSingleNode("//Table");
                //tableXml.InnerXml = table.InnerXml;
                //CombineColumnWords(tblDocument, table);
                XmlNode tableXml = tblDocument.SelectSingleNode("//Table");
                tableXml.InnerXml = table.InnerXml;
                tblDocument.Save(fileName);
            }
            catch (Exception)
            {

            }
        }

        public XmlDocument CombineColumnWords(XmlDocument doc, XmlNode table)
        {
            try
            {
                if (table == null) return null;

                StringBuilder columnLineText = new StringBuilder();
                XmlNode combineLine = null;
                List<XmlNode> columnLines = new List<XmlNode>();

                double llxTemp = 0;
                double llyTemp = 0;
                double urxTemp = 0;
                double uryTemp = 0;

                double fontSize = 0;
                string fontName = "";
                string fontType = "";
                double height = 0;
                double left = 0;
                double top = 0;

                double prevLineUry = 0;
                int sameUryLineCount = 0;

                double llx = 0;
                double lly = 0;
                double urx = 0;
                double ury = 0;
                int page = 0;

                foreach (XmlNode row in table)
                {
                    prevLineUry = 0;

                    if (row.Name.Equals("head-row") || row.Name.Equals("Row"))
                    {
                        var columns = row.Cast<XmlNode>().Where(x => x.ChildNodes.Count > 1).ToList();

                        if (columns != null)
                        {
                            for (int col = 0; col < columns.Count; col++)
                            {
                                for (int ln = 0; ln < columns[col].ChildNodes.Count; ln++)
                                {
                                    var coord = columns[col].ChildNodes[ln].Attributes["coord"].Value;

                                    llxTemp = Convert.ToDouble(coord.Split(':')[0]);
                                    llyTemp = Convert.ToDouble(coord.Split(':')[1]);
                                    urxTemp = Convert.ToDouble(coord.Split(':')[2]);
                                    uryTemp = Convert.ToDouble(coord.Split(':')[3]);

                                    if (ln == 0)
                                    {
                                        llx = llxTemp;
                                        lly = llyTemp;
                                        fontSize = Convert.ToDouble(columns[col].ChildNodes[ln].Attributes["fontsize"].Value);
                                        fontName = Convert.ToString(columns[col].ChildNodes[ln].Attributes["font"].Value);
                                        fontType = Convert.ToString(columns[col].ChildNodes[ln].Attributes["fonttype"].Value);
                                        height = Convert.ToDouble(columns[col].ChildNodes[ln].Attributes["height"].Value);
                                        page = Convert.ToInt32(columns[col].ChildNodes[ln].Attributes["page"].Value);
                                        left = Convert.ToDouble(columns[col].ChildNodes[ln].Attributes["left"].Value);
                                        top = Convert.ToDouble(columns[col].ChildNodes[ln].Attributes["top"].Value);
                                    }
                                    else if (ln == columns[col].ChildNodes.Count - 1)
                                    {
                                        urx = urxTemp;
                                        ury = uryTemp;
                                    }

                                    if (prevLineUry == uryTemp || prevLineUry == 0)
                                    {
                                        sameUryLineCount++;
                                        columnLineText.Append(columns[col].ChildNodes[ln].InnerText + " ");
                                        columnLines.Add(columns[col].ChildNodes[ln]);
                                    }
                                    prevLineUry = uryTemp;
                                }

                                if (sameUryLineCount > 1)
                                {
                                    XmlAttribute coord = doc.CreateAttribute("coord");
                                    XmlAttribute font_size = doc.CreateAttribute("fontsize");
                                    XmlAttribute font_Name = doc.CreateAttribute("font");
                                    XmlAttribute font_Type = doc.CreateAttribute("fonttype");
                                    XmlAttribute lineHeight = doc.CreateAttribute("height");
                                    XmlAttribute pageNum = doc.CreateAttribute("page");
                                    XmlAttribute leftAttr = doc.CreateAttribute("left");
                                    XmlAttribute topAttr = doc.CreateAttribute("top");

                                    coord.Value = Convert.ToString(llx) + ":" + Convert.ToString(lly) + ":" +
                                                  Convert.ToString(urx) + ":" + Convert.ToString(ury);

                                    font_size.Value = Convert.ToString(fontSize);
                                    font_Name.Value = fontName;
                                    font_Type.Value = fontType;
                                    lineHeight.Value = Convert.ToString(height);
                                    pageNum.Value = Convert.ToString(page);
                                    leftAttr.Value = Convert.ToString(left);
                                    topAttr.Value = Convert.ToString(top);

                                    combineLine = doc.CreateElement("ln");
                                    combineLine.Attributes.Append(coord);
                                    combineLine.Attributes.Append(pageNum);
                                    combineLine.Attributes.Append(lineHeight);
                                    combineLine.Attributes.Append(leftAttr);
                                    combineLine.Attributes.Append(topAttr);
                                    combineLine.Attributes.Append(font_Name);
                                    combineLine.Attributes.Append(font_size);
                                    combineLine.Attributes.Append(font_Type);
                                    combineLine.InnerText = Convert.ToString(columnLineText);
                                }

                                for (int k = 0; k < columnLines.Count; k++)
                                {
                                    if (combineLine != null)
                                    {
                                        if (k == 0) columnLines[k].ParentNode.InsertBefore(combineLine, columnLines[k]);
                                        columnLines[k].ParentNode.RemoveChild(columnLines[k]);
                                    }
                                }
                                columnLines = new List<XmlNode>();
                                columnLineText.Clear();
                            }
                        }
                    }
                }

                return doc;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void ExtractPage(string sourcePdfPath, string outputPdfPath, int pageNumber)
        {
            PdfReader reader = null;
            Document document = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage = null;

            try
            {
                // Intialize a new PdfReader instance with the contents of the source Pdf file:
                reader = new PdfReader(sourcePdfPath);

                // Capture the correct size and orientation for the page:
                document = new Document(reader.GetPageSizeWithRotation(Convert.ToInt32(pageNumber)));

                // Initialize an instance of the PdfCopyClass with the source 
                // document and an output file stream:
                pdfCopyProvider = new PdfCopy(document,
                    new System.IO.FileStream(outputPdfPath, System.IO.FileMode.Create));

                document.Open();

                // Extract the desired page number:
                importedPage = pdfCopyProvider.GetImportedPage(reader, Convert.ToInt32(pageNumber));
                pdfCopyProvider.AddPage(importedPage);
                document.Close();
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ExporttoExcel(XmlNode table, string fileName)
        {
            //Common commObj = new Common();
            //string dirctoryPath = commObj.GetFilesDirectoryPath();

            //if (!Directory.Exists(dirctoryPath + "\\TableTasks"))
            //{
            //    Directory.CreateDirectory(dirctoryPath + "\\TableTasks");
            //}
            FileInfo objInfo = new FileInfo(fileName);
            if (!File.Exists(fileName))
            {
                using (ExcelPackage p = new ExcelPackage(objInfo))
                {

                    //Create a sheet
                    p.Workbook.Worksheets.Add("Sample WorkSheet");
                    ExcelWorksheet ws = p.Workbook.Worksheets[1];
                    ws.Name = "Sample Worksheet"; //Setting Sheet's name

                    int colIndex = 1;
                    int rowIndex = 2;
                    XmlNodeList rows = table.SelectNodes("Row");
                    foreach (XmlNode row in rows) //Creating Headings
                    {
                        colIndex = 1;
                        XmlNodeList cells = row.SelectNodes("descendant::Cell");
                        foreach (XmlNode cell in cells)
                        {
                            var innerCell = ws.Cells[rowIndex, colIndex];
                            XmlNodeList words = cell.SelectNodes("descendant::Text");
                            string sentence = "";
                            foreach (XmlNode word in words)
                            {
                                sentence = sentence + word.InnerText + " ";
                            }
                            if (sentence.Length > 0)
                            {
                                innerCell.Value = sentence.Remove(sentence.Length - 1, 1);
                            }
                            colIndex++;
                        }
                        rowIndex++;
                    }
                    p.Save();
                }
            }
        }

        ////old method from first xml
        //public List<String> GetCoordinates(string tetmlPath, XmlNode lsttable)
        //{
        //    try
        //    {
        //        List<string> lstcoordiants = new List<string>();
        //        string llx = "";
        //        string lly = "";
        //        string urx = "";
        //        string ury = "";
        //        string pageno = "";
        //        string coordinates = "";
        //        double left = 0;
        //        double bottom = 0;
        //        string pdfPath = tetmlPath.Replace(".tetml", ".pdf");

        //        if (lsttable == null)
        //        {
        //            XmlDocument tetDoc = new XmlDocument();
        //            StreamReader sr = new StreamReader(tetmlPath);
        //            string xmlText = sr.ReadToEnd();
        //            sr.Close();
        //            string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
        //            tetDoc.LoadXml(documentXML);
        //            XmlNodeList pages = tetDoc.SelectNodes("//Page");

        //            foreach (XmlNode page in pages)
        //            {
        //                string pageText = "";
        //                pageno = page.Attributes["number"].Value;
        //                XmlNodeList tables = page.SelectNodes("Content/Table");
        //                if (tables.Count > 0)
        //                {
        //                    for (int i = 0; i < tables.Count; i++)
        //                    {
        //                        XmlNodeList lineList = tables[i].SelectNodes("Row/Cell/Para/Word/Box");
        //                        if (lineList.Count > 0)
        //                        {
        //                            //Convert xmlNodeList to list<string>
        //                            var rightValues = lineList.Cast<XmlNode>()
        //                                .Select(node => Convert.ToDouble(node.Attributes["urx"].Value))
        //                                .ToList();
        //                            urx = rightValues.Max().ToString();
        //                            var leftValues = lineList.Cast<XmlNode>()
        //                                .Select(node => Convert.ToDouble(node.Attributes["llx"].Value))
        //                                .ToList();
        //                            llx = leftValues.Min().ToString();
        //                            var rightYValues = lineList.Cast<XmlNode>()
        //                               .Select(node => Convert.ToDouble(node.Attributes["ury"].Value))
        //                               .ToList();
        //                            ury = rightYValues.Max().ToString();
        //                            var leftYValues = lineList.Cast<XmlNode>()
        //                               .Select(node => Convert.ToDouble(node.Attributes["lly"].Value))
        //                               .ToList();
        //                            lly = leftYValues.Min().ToString();
        //                        }
        //                        double width = Convert.ToDouble(urx) - Convert.ToDouble(llx);
        //                        double height = Convert.ToDouble(ury) - Convert.ToDouble(lly);
        //                        //coordinates = llx + " " + lly + " " + height + " " + width + " " + pageno;

        //                        string croppedMargins = GetCroppedMargins(Convert.ToInt32(pageno), pdfPath);
        //                        List<string> tempValues = croppedMargins.Split(' ').ToList();

        //                        if (tempValues != null)
        //                        {
        //                            if (tempValues.Count > 0)
        //                            {
        //                                left = Convert.ToDouble(tempValues[3]);
        //                                bottom = Convert.ToDouble(tempValues[1]);
        //                            }
        //                        }
        //                        //coordinates = (Convert.ToDouble(llx) + left) + " " + (Convert.ToDouble(lly) + bottom) + " " + 
        //                        //              (height + left) + " " + (width + bottom) + " " + pageno;

        //                        coordinates = (Convert.ToDouble(llx) + left) + " " + (Convert.ToDouble(lly) + bottom) + " " + height + " " + width + " " + pageno;

        //                        lstcoordiants.Add(coordinates);
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            //for (int i = 0; i < lsttable.Count; i++)
        //            //{
        //                if (lsttable != null)
        //                {
        //                    XmlNodeList lineList = lsttable.SelectNodes("Row/Cell/Para/Word/Box");
        //                    if (lineList.Count > 0)
        //                    {
        //                        //Convert xmlNodeList to list<string>
        //                        var rightValues = lineList.Cast<XmlNode>()
        //                            .Select(node => Convert.ToDouble(node.Attributes["urx"].Value))
        //                            .ToList();
        //                        urx = rightValues.Max().ToString();
        //                        var leftValues = lineList.Cast<XmlNode>()
        //                            .Select(node => Convert.ToDouble(node.Attributes["llx"].Value))
        //                            .ToList();
        //                        llx = leftValues.Min().ToString();
        //                        var rightYValues = lineList.Cast<XmlNode>()
        //                           .Select(node => Convert.ToDouble(node.Attributes["ury"].Value))
        //                           .ToList();
        //                        ury = rightYValues.Max().ToString();
        //                        var leftYValues = lineList.Cast<XmlNode>()
        //                           .Select(node => Convert.ToDouble(node.Attributes["lly"].Value))
        //                           .ToList();
        //                        lly = leftYValues.Min().ToString();
        //                    }
        //                    double width = Convert.ToDouble(urx) - Convert.ToDouble(llx);
        //                    double height = Convert.ToDouble(ury) - Convert.ToDouble(lly);
        //                    //coordinates = llx + " " + lly + " " + height + " " + width + " " + lsttable[i].Attributes["page"].Value;
        //                    string croppedMargins = GetCroppedMargins(Convert.ToInt32(lsttable.Attributes["page"].Value), pdfPath);
        //                    List<string> tempValues = croppedMargins.Split(' ').ToList();

        //                    if (tempValues != null)
        //                    {
        //                        if (tempValues.Count > 0)
        //                        {
        //                            left = Math.Abs(Convert.ToDouble(tempValues[3]));
        //                            bottom = Math.Abs(Convert.ToDouble(tempValues[1]));
        //                        }
        //                    }
        //                    //coordinates = (Convert.ToDouble(llx) + left) + " " + (Convert.ToDouble(lly) + bottom) + " " + (height + left) + " " + (width + bottom) + " " + lsttable[i].Attributes["page"].Value;
        //                    coordinates = (Convert.ToDouble(llx) + left) + " " + (Convert.ToDouble(lly) + bottom) + " " + height + " " + width + " " + lsttable.Attributes["page"].Value;

        //                    lstcoordiants.Add(coordinates);
        //                }
        //        }
        //        //}
        //        return lstcoordiants;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        public List<String> GetCoordinates(string tetmlPath, XmlNode lsttable, int page, string tableRowType)
        {
            if (lsttable == null) return null;

            try
            {
                List<string> lstcoordiants = null;
                string llx = string.Empty;
                string lly = string.Empty;
                string urx = string.Empty;
                string ury = string.Empty;
                string coordinates = string.Empty;
                double left = 0;
                double bottom = 0;
                string pdfPath = tetmlPath.Replace(".tetml", ".pdf");

                XmlNodeList lineList = null;

                //if (tableRowType.Equals("tableHeader"))
                //    lineList = lsttable.SelectNodes("//header/descendant::ln");

                //else if (tableRowType.Equals("headerRow"))
                //    lineList = lsttable.SelectNodes("//head-row/descendant::ln");

                //else if (tableRowType.Equals("tableCaption"))
                //    lineList = lsttable.SelectNodes("//caption/descendant::ln");

                //else
                //    lineList = lsttable.SelectNodes("//Row/descendant::ln");

                if (tableRowType.Equals("tableHeader"))
                    lineList = lsttable.SelectNodes("descendant::header/col/ln");

                else if (tableRowType.Equals("headerRow"))
                    lineList = lsttable.SelectNodes("descendant::head-row/head-col/ln");

                else if (tableRowType.Equals("tableCaption"))
                    lineList = lsttable.SelectNodes("descendant::caption/col/ln");

                else
                    lineList = lsttable.SelectNodes("descendant::Row/col/ln");


                if (lineList.Count > 0)
                {
                    lstcoordiants = new List<string>();

                    //var rightValues = lineList.Cast<XmlNode>()
                    //    .Select(node => Convert.ToDouble(node.Attributes["urx"].Value))
                    //    .ToList();
                    //urx = rightValues.Max().ToString();
                    //var leftValues = lineList.Cast<XmlNode>()
                    //    .Select(node => Convert.ToDouble(node.Attributes["llx"].Value))
                    //    .ToList();
                    //llx = leftValues.Min().ToString();
                    //var rightYValues = lineList.Cast<XmlNode>()
                    //    .Select(node => Convert.ToDouble(node.Attributes["ury"].Value))
                    //    .ToList();
                    //ury = rightYValues.Max().ToString();
                    //var leftYValues = lineList.Cast<XmlNode>()
                    //    .Select(node => Convert.ToDouble(node.Attributes["lly"].Value))
                    //    .ToList();
                    //lly = leftYValues.Min().ToString();

                    var leftValues = lineList.Cast<XmlNode>().Select(node => Convert.ToDouble(node.Attributes["coord"].Value.Split(':')[0])).ToList();
                    llx = leftValues.Min().ToString();

                    var rightValues = lineList.Cast<XmlNode>().Select(node => Convert.ToDouble(node.Attributes["coord"].Value.Split(':')[2])).ToList();
                    urx = rightValues.Max().ToString();

                    var leftYValues = lineList.Cast<XmlNode>().Select(node => Convert.ToDouble(node.Attributes["coord"].Value.Split(':')[1])).ToList();
                    lly = leftYValues.Min().ToString();

                    var rightYValues = lineList.Cast<XmlNode>().Select(node => Convert.ToDouble(node.Attributes["coord"].Value.Split(':')[3])).ToList();
                    ury = rightYValues.Max().ToString();

                    double width = Convert.ToDouble(urx) - Convert.ToDouble(llx);
                    double height = Convert.ToDouble(ury) - Convert.ToDouble(lly);
                    string croppedMargins = getCroppedMargins(Convert.ToInt32(page), pdfPath);
                    List<string> tempValues = croppedMargins.Split(' ').ToList();

                    if (tempValues != null)
                    {
                        if (tempValues.Count > 0)
                        {
                            left = Math.Abs(Convert.ToDouble(tempValues[3]));
                            bottom = Math.Abs(Convert.ToDouble(tempValues[1]));
                        }
                    }

                    coordinates = (Convert.ToDouble(llx) + left) + " " + (Convert.ToDouble(lly) + bottom) + " " + height +
                                  " " + width + " " + page;

                    lstcoordiants.Add(coordinates);

                    return lstcoordiants;
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetCroppedMargins(int pageNum, string mainPdfPath)
        {
            PdfReader reader = new PdfReader(File.ReadAllBytes(mainPdfPath));
            iTextSharp.text.Rectangle cropbox = reader.GetCropBox(pageNum);
            var box = reader.GetPageSizeWithRotation(pageNum);

            double top = (box.Top - cropbox.Top);
            double bottom = cropbox.Bottom;
            double right = (box.Right - cropbox.Right);
            double left = cropbox.Left;
            return Math.Round(top, 2) + " " + Math.Round(bottom, 2) + " " + Math.Round(right, 2) + " " + Math.Round(left, 2);
        }

        public bool HighLightTables(string inFilePath, string outputFilePath, BaseColor color, List<string> lstcoordinates)
        {
            bool IsHighlighted = false;

            try
            {
                using (Stream inputPdfStream = new FileStream(inFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                using (Stream outputPdfStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
                {
                    //Opens the unmodified PDF for reading
                    PdfReader reader = new PdfReader(inputPdfStream);

                    var stamper = new PdfStamper(reader, outputPdfStream) { FormFlattening = true, FreeTextFlattening = true };
                    foreach (string coordinates in lstcoordinates)
                    {
                        string[] dimensions = coordinates.Split(' ');
                        float x = 0;
                        float y = 0;
                        float.TryParse(dimensions[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                        float.TryParse(dimensions[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                        int width = Convert.ToInt32(Math.Round(Convert.ToDouble(dimensions[3]))) == 0 ? 1 : Convert.ToInt32(Math.Round(Convert.ToDouble(dimensions[3])));
                        int height = Convert.ToInt32(Math.Round(Convert.ToDouble(dimensions[2]))) == 0 ? 1 : Convert.ToInt32(Math.Round(Convert.ToDouble(dimensions[2])));
                        iTextSharp.text.Image objImage1 = iTextSharp.text.Image.GetInstance(new Bitmap(width, height), color);

                        objImage1.SetAbsolutePosition(x, y - 2);
                        PdfGState _state = new PdfGState()
                        {
                            FillOpacity = 0.7F,
                            StrokeOpacity = 0.7F
                        };
                        stamper.GetOverContent(Convert.ToInt32(dimensions[4])).SetGState(_state);
                        stamper.GetOverContent(Convert.ToInt32(dimensions[4])).AddImage(objImage1, true);
                    }
                    stamper.Close();
                }
                lstcoordinates.Clear();
                IsHighlighted = true;

                File.Delete(inFilePath);
                File.Copy(outputFilePath, inFilePath);
                //File.Delete(outputFilePath);

                return IsHighlighted;
            }
            catch (Exception ex)
            {
                return IsHighlighted;
            }
        }

        #region Mark text from table JqueryPopup

        //public XmlNode MarkTableInXml(List<string> tableLinesList, int pageNum, bool isIgnoreAlgo, bool saveAsImg)
        //{
        //    try
        //    {
        //        //if (string.IsNullOrEmpty(tableLines)) return null;

        //        //if (!string.IsNullOrEmpty(tableLines))
        //        //{
        //        //    TempXmlDoc = null;
        //        //    TableXml = null;

        //        //    var tables = SaveTablesInXml(tableLines, isIgnoreAlgo, false, selectionType);
        //        //    return tables;
        //        //}

        //        if (tableLinesList == null || tableLinesList.Count == 0) return null;

        //        //string pageNumber = GetCurrentPageNum();

        //        return SaveTablesInTempXml(tableLinesList, pageNum, isIgnoreAlgo, saveAsImg);
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        //public XmlNode SaveTablesInXml(string linesList, bool isIgnorAlgoChecked, bool saveAsImg, string selectionType)
        //{
        //    if (string.IsNullOrEmpty(linesList)) return null;

        //    int pageNumber = GetCurrentPageNum();
        //    List<string> pdfJsLinesList = new List<string>();

        //    if (selectionType.Equals("auto"))
        //        pdfJsLinesList = GetAutoSelectedLines(linesList, pageNumber);
        //    else
        //        pdfJsLinesList = GetPdfJsSelectedLines(linesList, pageNumber);

        //    if (pdfJsLinesList == null || pdfJsLinesList.Count == 0) return null;

        //    return SaveTablesInTempXml(pdfJsLinesList, isIgnorAlgoChecked, pageNumber, saveAsImg);
        //}

        //public XmlNode MoveTablesInXml(List<string> linesList, int pageNum, bool isIgnorAlgoChecked)
        //{
        //    return SaveTablesInTempXml(linesList, isIgnorAlgoChecked, pageNum, false);
        //}

        //<table id="321" border="off" head-row="off">
        //  <header> </header>
        //  <voice-description />
        //  <head-row>
        //    <head-col width="20"> </head-col>
        //    <head-col width="60"> </head-col>
        //  </head-row>
        //  <row>
        //    <col></col> 
        //    <col></col>
        //  </row>
        // <caption></caption>
        //</table>

        public XmlNode SaveTablesInTempXml(List<string> pdfJsLinesList, int pageNumber, bool isIgnorAlgoChecked, bool saveAsImg)
        {
            if (pdfJsLinesList == null || pdfJsLinesList.Count == 0) return null;

            string temmXml = "";
            int pageNum = Convert.ToInt32(HttpContext.Current.Session["ActualPdfPage"]);

            ////if (isIgnorAlgoChecked)
            ////    temmXml = Convert.ToString(HttpContext.Current.Session["tempXmlPath_actual"]);
            ////else
            ////    temmXml = Convert.ToString(HttpContext.Current.Session["tempXmlPath"]);

            //if (isIgnorAlgoChecked)
            temmXml = Convert.ToString(HttpContext.Current.Session["tempXmlPath_actual"]);
            //else
            //    temmXml = Convert.ToString(HttpContext.Current.Session["tempXmlPath"]);

            try
            {


                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(temmXml);

                int normalX = 0;
                int normalXIndent = 0;
                double lineHeight = 0;

                NormalAndIndentX(xmlDoc, ref normalX, ref normalXIndent);

                XmlNodeList pageNode = xmlDoc.SelectNodes("//Page[@number=" + pageNum + "]");

                if (pageNode != null)
                {
                    if (pageNode.Count > 0)
                    {
                        if (pageNode[0].Attributes["height"] != null)
                        {
                            lineHeight = Convert.ToDouble(pageNode[0].Attributes["height"].Value);
                        }
                    }
                }

                //XmlNodeList listLines = xmlDoc.SelectNodes("//Line");
                XmlNodeList listLines = xmlDoc.SelectNodes("//Page[@number=" + pageNum + "]//Line");
                List<XmlNode> LinesPassed = new List<XmlNode>();
                bool isProperTable = false;
                int ActivePage = 0;
                int noOftables = 1;
                int tableStartLine = 0;
                bool isTableEndLine = false;

                int AverageVeticalSpace = Convert.ToInt32(HttpContext.Current.Session["AverageVeticalSpace"]);
                int AverageSpace = Convert.ToInt32(HttpContext.Current.Session["AverageSpace"]);

                List<string> tblStartLineWords = GetTableStartingLine(pdfJsLinesList);
                List<string> tblEndLineWords = GetTableEndLine(pdfJsLinesList);

                List<XmlNode> tableLines = new List<XmlNode>();
                XmlDocument tblDocument = null;
                XmlNode table = null;
                int pageno = 0;

                for (int i = 0; i < listLines.Count; i++)
                {
                    if (!LinesPassed.Contains(listLines[i]))
                    {
                        //If current Line is a table line
                        if (isTableStartingLine(listLines[i], tblStartLineWords))
                        {
                            tableStartLine++;

                            ////if next node is not null and is table line
                            //if (listLines[i].NextSibling != null && (listLines[i].NextSibling.Name.Equals("Line") && (isMatchedLine(listLines[i].NextSibling, linesList))))
                            ////if (listLines[i].NextSibling != null && (listLines[i].NextSibling.Name.Equals("Line") && (tableStartLine < linesList.Count)))
                            //{
                            XmlNode currentNode = listLines[i];
                            bool firstLine = true;
                            pageno = Convert.ToInt32(listLines[i].SelectSingleNode("ancestor::Page").Attributes["number"].Value);
                            //Create table and add rows till the end of abnormal rows...
                            do
                            {
                                tableLines.Add(currentNode);
                                //if first abnormal row then start creating a new table
                                if (firstLine)
                                {
                                    isProperTable = false;
                                    string OutputXml1 = "<?xml version=\"1.0\"?><Table page='" + pageno + "'></Table>";
                                    tblDocument = new XmlDocument();
                                    tblDocument.LoadXml(OutputXml1);
                                    table = tblDocument.SelectSingleNode("//Table");
                                }

                                XmlNode RowNode = tblDocument.CreateElement("Row");
                                XmlNode CellNode = tblDocument.CreateElement("Cell");
                                XmlNode ParaNode = tblDocument.CreateElement("Para");
                                XmlNodeList words = currentNode.SelectNodes("Word");

                                for (int j = 0; j < words.Count; j++)
                                {
                                    int llx = (int)Convert.ToDouble(words[j].Attributes["x1"].Value);
                                    int lly = (int)Convert.ToDouble(words[j].Attributes["y1"].Value);
                                    int ury = (int)Convert.ToDouble(words[j].Attributes["y"].Value);
                                    int urx = (int)Convert.ToDouble(words[j].Attributes["x"].Value);

                                    if (j > 0)
                                    {
                                        double diffrence = Convert.ToDouble(words[j].Attributes["x1"].Value) - Convert.ToDouble(words[j - 1].Attributes["x"].Value);
                                        if (diffrence / AverageSpace >= 3)
                                        {
                                            CellNode = tblDocument.CreateElement("Cell");
                                            ParaNode = tblDocument.CreateElement("Para");
                                        }
                                    }
                                    XmlNode WordNode = tblDocument.CreateElement("Word");
                                    XmlNode TextNode = tblDocument.CreateElement("Text");
                                    XmlNode BoxNode = tblDocument.CreateElement("Box");
                                    XmlAttribute lowerx = tblDocument.CreateAttribute("llx");
                                    XmlAttribute uperrx = tblDocument.CreateAttribute("urx");
                                    XmlAttribute lowery = tblDocument.CreateAttribute("lly");
                                    XmlAttribute uperry = tblDocument.CreateAttribute("ury");

                                    XmlAttribute fontsize = tblDocument.CreateAttribute("fontsize");
                                    XmlAttribute font = tblDocument.CreateAttribute("font");
                                    XmlAttribute fonttype = tblDocument.CreateAttribute("fonttype");
                                    XmlAttribute height = tblDocument.CreateAttribute("height");

                                    lowerx.Value = words[j].Attributes["x1"].Value;
                                    uperrx.Value = words[j].Attributes["x"].Value;
                                    lowery.Value = words[j].Attributes["y1"].Value;
                                    uperry.Value = words[j].Attributes["y"].Value;
                                    BoxNode.Attributes.Append(lowerx);
                                    BoxNode.Attributes.Append(uperrx);
                                    BoxNode.Attributes.Append(lowery);
                                    BoxNode.Attributes.Append(uperry);

                                    fontsize.Value = words[j].Attributes["fontsize"].Value;
                                    font.Value = words[j].Attributes["font"].Value;
                                    fonttype.Value = words[j].Attributes["fonttype"].Value;
                                    height.Value = Convert.ToString(lineHeight);
                                    BoxNode.Attributes.Append(fontsize);
                                    BoxNode.Attributes.Append(font);
                                    BoxNode.Attributes.Append(fonttype);
                                    BoxNode.Attributes.Append(height);
                                    //CellNode.Attributes.Append(lowerx);

                                    TextNode.InnerText = words[j].InnerText;
                                    WordNode.AppendChild(BoxNode);
                                    WordNode.AppendChild(TextNode);
                                    ParaNode.AppendChild(WordNode);
                                    CellNode.AppendChild(ParaNode);
                                    RowNode.AppendChild(CellNode);
                                }
                                LinesPassed.Add(currentNode);
                                table.AppendChild(RowNode);
                                firstLine = false;

                                i += 1;

                                currentNode = listLines[i] != null ? listLines[i] : null;

                                if (isTableStartingLine(currentNode, tblEndLineWords))
                                {
                                    isProperTable = true;
                                    isTableEndLine = true;

                                    tableLines.Add(currentNode);
                                    XmlNodeList wordsEndLine = currentNode.SelectNodes("Word");
                                    XmlNode RowNodeEndLine = tblDocument.CreateElement("Row");
                                    XmlNode CellNodeEndLine = tblDocument.CreateElement("Cell");
                                    XmlNode ParaNodeEndLine = tblDocument.CreateElement("Para");
                                    for (int j = 0; j < wordsEndLine.Count; j++)
                                    {
                                        int llx = (int)Convert.ToDouble(wordsEndLine[j].Attributes["x1"].Value);
                                        int lly = (int)Convert.ToDouble(wordsEndLine[j].Attributes["y1"].Value);
                                        int ury = (int)Convert.ToDouble(wordsEndLine[j].Attributes["y"].Value);
                                        int urx = (int)Convert.ToDouble(wordsEndLine[j].Attributes["x"].Value);

                                        if (j > 0)
                                        {
                                            double diffrence = Convert.ToDouble(wordsEndLine[j].Attributes["x1"].Value) -
                                                Convert.ToDouble(wordsEndLine[j - 1].Attributes["x"].Value);

                                            if (diffrence / AverageSpace >= 3)
                                            {
                                                CellNodeEndLine = tblDocument.CreateElement("Cell");
                                                ParaNodeEndLine = tblDocument.CreateElement("Para");
                                            }
                                        }
                                        XmlNode WordNode = tblDocument.CreateElement("Word");
                                        XmlNode TextNode = tblDocument.CreateElement("Text");
                                        XmlNode BoxNode = tblDocument.CreateElement("Box");
                                        XmlAttribute lowerx = tblDocument.CreateAttribute("llx");
                                        XmlAttribute uperrx = tblDocument.CreateAttribute("urx");
                                        XmlAttribute lowery = tblDocument.CreateAttribute("lly");
                                        XmlAttribute uperry = tblDocument.CreateAttribute("ury");

                                        XmlAttribute fontsize = tblDocument.CreateAttribute("fontsize");
                                        XmlAttribute font = tblDocument.CreateAttribute("font");
                                        XmlAttribute fonttype = tblDocument.CreateAttribute("fonttype");
                                        XmlAttribute height = tblDocument.CreateAttribute("height");

                                        lowerx.Value = wordsEndLine[j].Attributes["x1"].Value;
                                        uperrx.Value = wordsEndLine[j].Attributes["x"].Value;
                                        lowery.Value = wordsEndLine[j].Attributes["y1"].Value;
                                        uperry.Value = wordsEndLine[j].Attributes["y"].Value;
                                        BoxNode.Attributes.Append(lowerx);
                                        BoxNode.Attributes.Append(uperrx);
                                        BoxNode.Attributes.Append(lowery);
                                        BoxNode.Attributes.Append(uperry);

                                        fontsize.Value = wordsEndLine[j].Attributes["fontsize"].Value;
                                        font.Value = wordsEndLine[j].Attributes["font"].Value;
                                        fonttype.Value = wordsEndLine[j].Attributes["fonttype"].Value;
                                        height.Value = Convert.ToString(lineHeight);
                                        BoxNode.Attributes.Append(fontsize);
                                        BoxNode.Attributes.Append(font);
                                        BoxNode.Attributes.Append(fonttype);
                                        BoxNode.Attributes.Append(height);
                                        //CellNode.Attributes.Append(lowerx);

                                        TextNode.InnerText = wordsEndLine[j].InnerText;
                                        WordNode.AppendChild(BoxNode);
                                        WordNode.AppendChild(TextNode);
                                        ParaNodeEndLine.AppendChild(WordNode);
                                        CellNodeEndLine.AppendChild(ParaNodeEndLine);
                                        RowNodeEndLine.AppendChild(CellNodeEndLine);
                                    }
                                    LinesPassed.Add(currentNode);
                                    table.AppendChild(RowNodeEndLine);
                                }
                            } while (currentNode != null && (currentNode.Name.Equals("Line") && (!isTableEndLine)));
                            //} while (currentNode != null && (currentNode.Name.Equals("Line") && (tableStartLine < linesList.Count)));

                            if (table != null)
                            {
                                if (isProperTable)
                                {
                                    //tables.Add(table);

                                    if (saveAsImg)
                                    {
                                        XmlNode dummyImage = xmlDoc.CreateElement("image");
                                        XmlAttribute imgId = xmlDoc.CreateAttribute("id");
                                        XmlAttribute imgUrl = xmlDoc.CreateAttribute("image-url");
                                        XmlAttribute height = xmlDoc.CreateAttribute("height");
                                        XmlAttribute coord = xmlDoc.CreateAttribute("coord");
                                        XmlAttribute page = xmlDoc.CreateAttribute("page");

                                        noOftables = pageno.Equals(ActivePage) ? noOftables + 1 : 1;
                                        ActivePage = pageno;

                                        imgId.Value = pageno + "_" + noOftables;
                                        imgUrl.Value = @"Resources\image" + noOftables + ".jpg";
                                        height.Value = Convert.ToString(lineHeight);
                                        coord.Value = GetImageCoords(table, pageno);
                                        page.Value = Convert.ToString(pageno);

                                        dummyImage.Attributes.Append(imgId);
                                        dummyImage.Attributes.Append(imgUrl);
                                        dummyImage.Attributes.Append(height);
                                        dummyImage.Attributes.Append(coord);
                                        dummyImage.Attributes.Append(page);

                                        for (int k = 0; k < tableLines.Count; k++)
                                        {
                                            //Change for demo
                                            //if (k == 0) tableLines[k].ParentNode.InsertBefore(dummyImage, tableLines[k]);
                                            //tableLines[k].ParentNode.RemoveChild(tableLines[k]);
                                        }
                                        tableLines = new List<XmlNode>();
                                    }
                                    else
                                    {
                                        XmlNode dummyTable = xmlDoc.CreateElement("Table");
                                        XmlAttribute tableId = xmlDoc.CreateAttribute("id");
                                        noOftables = pageno.Equals(ActivePage) ? noOftables + 1 : 1;
                                        ActivePage = pageno;
                                        tableId.Value = pageno + "_" + noOftables + "_Manual";
                                        dummyTable.Attributes.Append(tableId);
                                        dummyTable.InnerText = "Manual Table Insertion";
                                        for (int k = 0; k < tableLines.Count; k++)
                                        {
                                            //Change for demo
                                            //if (k == 0) tableLines[k].ParentNode.InsertBefore(dummyTable, tableLines[k]);
                                            //tableLines[k].ParentNode.RemoveChild(tableLines[k]);
                                        }
                                        tableLines = new List<XmlNode>();
                                    }
                                }
                            }
                            //}
                        }
                    }
                }

                //if (tables.Count > 0)
                //    xmlDoc.Save(temmXml);

                if (table != null)
                {
                    //SetTableTags(tables);

                    if (saveAsImg)
                        xmlDoc.Save(temmXml);

                    else
                    {
                        TableXml = table;
                        TempXmlDoc = xmlDoc;
                    }
                }
                return table;
            }
            catch (Exception)
            {
                return null;
            }
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

        #endregion

        #region Conversion to Final Xml

        public XmlNode ConvertToFinalXml(XmlNode tableNode, string finalTableHtml, int page, int tableId, int rbtnTableBorderIndex, int rbtnHeadRowIndex,
                                         string divHeaderRowText, string divCaptionRowText, int headerSelectType, int captionSelectType)
        {
            if (string.IsNullOrEmpty(finalTableHtml) || tableNode == null) return null;

            string tableBorderStatus = rbtnTableBorderIndex == 0 ? "on" : "off";
            string headerRowStatus = rbtnHeadRowIndex == 0 ? "on" : "off";

            string bookId = Convert.ToString(HttpContext.Current.Session["MainBook"]);
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
            int tblCaptionRowCount = 0;

            var taleHeaderLines = tableNode.Cast<XmlNode>().TakeWhile(x => x.ChildNodes.Count == 1).ToList();

            Process1 obj = new Process1();

            if (!string.IsNullOrEmpty(divHeaderRowText))
            {
                //Converting header row to para
                if (headerSelectType == 0)
                {
                    if (!string.IsNullOrEmpty(divHeaderRowText) && taleHeaderLines.Count > 0)
                    {
                        obj.ClearHeaderRowText();

                        foreach (XmlNode headerLine in taleHeaderLines)
                        {
                            tableNode.RemoveChild(headerLine);
                        }
                    }
                }
                //Adding header rows to start of table body
                else if (headerSelectType == 1)
                {
                    if (!string.IsNullOrEmpty(divHeaderRowText) && taleHeaderLines.Count > 0)
                    {
                        obj.ClearHeaderRowText();

                        var tblBodyRows =
                            tableNode.SelectNodes("//Row").Cast<XmlNode>().Where(x => x.ChildNodes.Count > 1).ToList();

                        if (tblBodyRows.Count > 0)
                        {
                            for (int i = taleHeaderLines.Count - 1; i >= 0; i--)
                            {
                                tblBodyRows[0].ParentNode.InsertBefore(taleHeaderLines[i], tblBodyRows[0]);
                            }
                        }
                    }
                }
                //Header rows are correctly detected
                else if (headerSelectType == 2)
                {
                    if (!string.IsNullOrEmpty(divHeaderRowText) && taleHeaderLines.Count > 0)
                    {
                        tblHeaderRowCount = taleHeaderLines.Count;

                        var colLine = GetRowLinesWithCoord(taleHeaderLines, page);
                        tableHeadercol.InnerText = colLine.Trim();
                        tableHeader.AppendChild(tableHeadercol);
                    }
                }
                //end
            }

            if (!string.IsNullOrEmpty(divCaptionRowText))
            {
                var taleCaptionLines = tableNode.Cast<XmlNode>().Reverse().TakeWhile(x => x.ChildNodes.Count == 1).ToList();
                ////var colCapLine = GetRowLinesWithCoord(taleCaptionLines, page);
                ////tableCaptioncol.InnerText = colCapLine.Trim();
                ////tableCaption.AppendChild(tableCaptioncol);

                //Converting caption rows to para
                if (captionSelectType == 0)
                {
                    if (!string.IsNullOrEmpty(divCaptionRowText) && taleCaptionLines.Count > 0)
                    {
                        obj.ClearCaptionRowText();

                        foreach (XmlNode captionLine in taleCaptionLines)
                        {
                            tableNode.RemoveChild(captionLine);
                        }
                    }
                }

                //Adding caption rows to end of table body
                else if (captionSelectType == 1)
                {
                    if (!string.IsNullOrEmpty(divCaptionRowText) && taleCaptionLines.Count > 0)
                    {
                        obj.ClearCaptionRowText();

                        var tblBodyLastRow = tableNode.SelectNodes("//Row").Cast<XmlNode>().Last();

                        if (tblBodyLastRow != null)
                        {
                            for (int i = taleCaptionLines.Count - 1; i >= 0; i--)
                            {
                                tblBodyLastRow.ParentNode.InsertBefore(taleCaptionLines[i], tblBodyLastRow);
                            }
                        }
                    }
                }
                //Caption rows are correctly detected
                else if (captionSelectType == 2)
                {
                    if (!string.IsNullOrEmpty(divCaptionRowText) && taleCaptionLines.Count > 0)
                    {
                        tblCaptionRowCount = taleCaptionLines.Count;

                        var colLine = GetRowLinesWithCoord(taleCaptionLines, page);
                        tableCaptioncol.InnerText = colLine.Trim();
                        tableCaption.AppendChild(tableCaptioncol);
                    }
                }
                //end
            }

            colText.Length = 0;

            bool isInitialXml = false;

            List<string> llyList = new List<string>();
            llyList = tableNode.SelectNodes("descendant::Box/@lly").Cast<XmlNode>().Select(x => x.Value).Distinct().ToList();

            if (llyList.Count > 0)
                isInitialXml = true;

            //var rows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Skip(tblHeaderRowCount).Where(x => x.ChildNodes.Count >= 1).ToList();

            ////var bodyRows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Skip(tblHeaderRowCount).ToList();
            ////int bodyRowsCount = bodyRows.Count;
            ////var captionRows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Reverse().TakeWhile(x => x.ChildNodes.Count == 1).ToList();
            ////var rows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Take(bodyRowsCount - captionRows.Count).ToList();

            var allBodyRows = tableNode.SelectNodes("//Row").Cast<XmlNode>().ToList();
            var rowsWithoutHeader = tableNode.SelectNodes("//Row").Cast<XmlNode>().Skip(tblHeaderRowCount).ToList();
            var rowsWithoutCaption = tableNode.SelectNodes("//Row").Cast<XmlNode>().Reverse().Skip(tblCaptionRowCount).ToList();

            var rows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Skip(tblHeaderRowCount).Take(rowsWithoutHeader.Count - tblCaptionRowCount).ToList();

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

                        XmlNode lastRow = finalTable.SelectNodes("//Row").Cast<XmlNode>().Last();
                        XmlNode rowNode = xmlDoc.CreateElement("Row");

                        if (isInitialXml)
                            rowNode = CombineTableLine(xmlDoc, rows[i], rowNode, page, false);
                        else
                            rowNode.InnerXml = rows[i].InnerXml;

                        lastRow.ParentNode.InsertAfter(rowNode, lastRow);
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

            ////XmlNode finalUpdatedXml = UpdateXmlByNewRowCol(finalTable, finalTableHtml, xmlDoc, page);

            ////if (finalUpdatedXml == null)
            ////{
                xmlDoc.InnerXml = finalTable.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");
                xmlDoc.Save(tableSavingPath);
            ////}
            ////else
            ////{
            ////    xmlDoc.InnerXml = finalUpdatedXml.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");
            ////    xmlDoc.Save(tableSavingPath);
            ////}

            return xmlDoc;
        }

        public XmlNode ConvertToFinalXmlUpdate(XmlNode tableNode, string finalTableHtml, int page, int tableId, int rbtnTableBorderIndex, int rbtnHeadRowIndex,
                                         string divHeaderRowText, string divCaptionRowText, int headerSelectType, int captionSelectType)
        {
            if (string.IsNullOrEmpty(finalTableHtml) || tableNode == null) return null;

            string tableBorderStatus = rbtnTableBorderIndex == 0 ? "on" : "off";
            string headerRowStatus = rbtnHeadRowIndex == 0 ? "on" : "off";

            string bookId = Convert.ToString(HttpContext.Current.Session["MainBook"]);
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
            int tblCaptionRowCount = 0;

            //var taleHeaderLines = tableNode.Cast<XmlNode>().TakeWhile(x => x.ChildNodes.Count == 1).ToList();

            //var taleHeaderLines =  tableNode.SelectNodes("//header").Cast<XmlNode>().ToList();

            XmlNode tableHeaderNode = tableNode.SelectSingleNode("//header");

            Process1 obj = new Process1();

            if (!string.IsNullOrEmpty(divHeaderRowText))
            {
                //Converting header row to para
                if (headerSelectType == 0)
                {
                    if (!string.IsNullOrEmpty(divHeaderRowText) && tableHeaderNode != null)
                    {
                        obj.ClearHeaderRowText();

                        tableHeaderNode.RemoveAll();

                        //foreach (XmlNode headerLine in taleHeaderLines)
                        //{
                        //    tableNode.RemoveChild(headerLine);
                        //}
                    }
                }
                //Adding header rows to start of table body
                else if (headerSelectType == 1)
                {
                    if (!string.IsNullOrEmpty(divHeaderRowText) && tableHeaderNode != null)
                    {
                        obj.ClearHeaderRowText();

                        var tableHeaderLines = tableHeaderNode.SelectNodes("descendant::ln").Cast<XmlNode>().ToList();

                        var tblBodyRows =
                            tableNode.SelectNodes("//Row").Cast<XmlNode>().Where(x => x.ChildNodes.Count > 1).ToList();

                        if (tblBodyRows.Count > 0)
                        {
                            for (int i = tableHeaderLines.Count - 1; i >= 0; i--)
                            {
                                tblBodyRows[0].ParentNode.InsertBefore(tableHeaderLines[i], tblBodyRows[0]);
                            }
                        }
                    }
                }
                //Header rows are correctly detected
                else if (headerSelectType == 2)
                {
                    if (!string.IsNullOrEmpty(divHeaderRowText) && tableHeaderNode != null)
                    {
                        var tableHeaderLines = tableHeaderNode.SelectNodes("descendant::ln").Cast<XmlNode>().ToList();
                        tblHeaderRowCount = tableHeaderLines.Count;

                        foreach (XmlNode headerLine in tableHeaderLines)
                        {
                            colText.Append(headerLine.OuterXml.Trim() + " ");
                        }
                        tableHeadercol.InnerText = Convert.ToString(colText);
                        tableHeader.AppendChild(tableHeadercol);

                        //var colLine = GetRowLinesWithCoord(tableHeaderLines, page);
                        //tableHeadercol.InnerText = colLine.Trim();
                        //tableHeader.AppendChild(tableHeadercol);
                    }
                }
                //end
            }

            if (!string.IsNullOrEmpty(divCaptionRowText))
            {
                //var taleCaptionLines = tableNode.Cast<XmlNode>().Reverse().TakeWhile(x => x.ChildNodes.Count == 1).ToList();

                XmlNode taleCaptionNode = tableNode.SelectSingleNode("//caption");

                //Converting caption rows to para
                if (captionSelectType == 0)
                {
                    if (!string.IsNullOrEmpty(divCaptionRowText) && taleCaptionNode != null)
                    {
                        obj.ClearCaptionRowText();

                        taleCaptionNode.RemoveAll();

                        //foreach (XmlNode captionLine in taleCaptionLines)
                        //{
                        //    captionLine.RemoveChild(captionLine.ChildNodes[0]);
                        //}
                    }
                }

                //Adding caption rows to end of table body
                else if (captionSelectType == 1)
                {
                    if (!string.IsNullOrEmpty(divCaptionRowText) && taleCaptionNode != null)
                    {
                        obj.ClearCaptionRowText();

                        var taleCaptionLines = taleCaptionNode.SelectNodes("descendant::ln").Cast<XmlNode>().ToList();
                        var tblBodyLastRow = tableNode.SelectNodes("//Row").Cast<XmlNode>().Last();

                        if (tblBodyLastRow != null)
                        {
                            for (int i = taleCaptionLines.Count - 1; i >= 0; i--)
                            {
                                tblBodyLastRow.ParentNode.InsertBefore(taleCaptionLines[i], tblBodyLastRow);
                            }
                        }
                    }
                }
                //Caption rows are correctly detected
                else if (captionSelectType == 2)
                {
                    if (!string.IsNullOrEmpty(divCaptionRowText) && taleCaptionNode != null)
                    {
                        var taleCaptionLines = taleCaptionNode.SelectNodes("descendant::ln").Cast<XmlNode>().ToList();
                        tblCaptionRowCount = taleCaptionLines.Count;

                        //var colLine = GetRowLinesWithCoord(taleCaptionLines, page);
                        //tableCaptioncol.InnerText = colLine.Trim();
                        //tableCaption.AppendChild(tableCaptioncol);

                        colText.Length = 0;

                        foreach (XmlNode captionLine in taleCaptionLines)
                        {
                            colText.Append(captionLine.OuterXml.Trim() + " ");
                        }
                        tableCaptioncol.InnerText = Convert.ToString(colText);
                        tableCaption.AppendChild(tableCaptioncol);
                    }
                }
                //end
            }

            colText.Length = 0;

            //bool isInitialXml = false;

            //List<string> llyList = new List<string>();
            //llyList = tableNode.SelectNodes("descendant::Box/@lly").Cast<XmlNode>().Select(x => x.Value).Distinct().ToList();

            //if (llyList.Count > 0)
            //    isInitialXml = true;

            ////var rows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Skip(tblHeaderRowCount).Where(x => x.ChildNodes.Count >= 1).ToList();

            //////var bodyRows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Skip(tblHeaderRowCount).ToList();
            //////int bodyRowsCount = bodyRows.Count;
            //////var captionRows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Reverse().TakeWhile(x => x.ChildNodes.Count == 1).ToList();
            //////var rows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Take(bodyRowsCount - captionRows.Count).ToList();

            //var allBodyRows = tableNode.SelectNodes("//Row").Cast<XmlNode>().ToList();
            //var rowsWithoutHeader = tableNode.SelectNodes("//Row").Cast<XmlNode>().Skip(tblHeaderRowCount).ToList();
            //var rowsWithoutCaption = tableNode.SelectNodes("//Row").Cast<XmlNode>().Reverse().Skip(tblCaptionRowCount).ToList();

            var rows = tableNode.SelectNodes("//Row").Cast<XmlNode>().ToList();

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

                        XmlNode lastRow = finalTable.SelectNodes("//Row").Cast<XmlNode>().Last();
                        XmlNode rowNode = xmlDoc.CreateElement("Row");

                        //if (isInitialXml)
                        //    rowNode = CombineTableLine(xmlDoc, rows[i], rowNode, page, false);
                        //else
                            rowNode.InnerXml = rows[i].InnerXml;

                        lastRow.ParentNode.InsertAfter(rowNode, lastRow);
                    }
                }
                else if (i == 0 && headerRowStatus.Equals("on"))
                {
                    XmlNode headeRow = finalTable.SelectSingleNode("//head-row");
                    //headeRow = CombineTableLine(xmlDoc, rows[i], headeRow, page, true);
                    headeRow.InnerXml = rows[i].InnerXml;
                }
                else
                {
                    XmlNode lastRow = finalTable.SelectNodes("//Row").Cast<XmlNode>().Last();
                    XmlNode rowNode = xmlDoc.CreateElement("Row");

                    //rowNode = CombineTableLine(xmlDoc, rows[i], rowNode, page, false);
                    rowNode.InnerXml = rows[i].InnerXml;
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

            ////XmlNode finalUpdatedXml = UpdateXmlByNewRowCol(finalTable, finalTableHtml, xmlDoc, page);

            ////if (finalUpdatedXml == null)
            ////{
            xmlDoc.InnerXml = finalTable.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");
            xmlDoc.Save(tableSavingPath);
            ////}
            ////else
            ////{
            ////    xmlDoc.InnerXml = finalUpdatedXml.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");
            ////    xmlDoc.Save(tableSavingPath);
            ////}

            return xmlDoc;
        }


        //backup
        //public XmlNode ConvertToFinalXmlUpdate(XmlNode tableNode, string finalTableHtml, int page, int tableId, int rbtnTableBorderIndex, int rbtnHeadRowIndex,
        //                                 string divHeaderRowText, string divCaptionRowText, int headerSelectType, int captionSelectType)
        //{
        //    if (string.IsNullOrEmpty(finalTableHtml) || tableNode == null) return null;

        //    string tableBorderStatus = rbtnTableBorderIndex == 0 ? "on" : "off";
        //    string headerRowStatus = rbtnHeadRowIndex == 0 ? "on" : "off";

        //    string bookId = Convert.ToString(HttpContext.Current.Session["MainBook"]);
        //    string mainDirectoryPath = Common.GetDirectoryPath();

        //    //string xmlText = "<table id=\"0\" border=\"off\" head-row=\"on\"><tbody ispreviewpassed=\"false\" page=\"2\"><header/>" +
        //    //                 "<head-row></head-row><Row></Row><caption/></tbody></table>";

        //    List<string> colWidthList = new List<string>();
        //    HtmlDocument doc = new HtmlDocument();
        //    doc.LoadHtml(finalTableHtml);
        //    int maxColumns = doc.DocumentNode.SelectNodes("//tr").Cast<HtmlNode>().Select(x => x.ChildNodes.Count).Max();

        //    var tdList = doc.DocumentNode.SelectNodes("//tr").Where(c => c.ChildNodes.Count == maxColumns).Take(1).ToList();
        //    if (tdList.Count > 0)
        //    {
        //        foreach (HtmlNode td in tdList[0].ChildNodes)
        //        {
        //            if (td.Attributes["style"] != null)
        //                colWidthList.Add(td.Attributes["style"].Value.Split(':')[2].Replace("%;", "").Trim());
        //        }
        //    }

        //    //XmlDocument xmlDoc = new XmlDocument();
        //    //XmlNode finalTable = xmlDoc.CreateElement("table");
        //    //XmlAttribute idAttr = xmlDoc.CreateAttribute("id");
        //    //idAttr.Value = Convert.ToString(tableId);
        //    //XmlAttribute borderAttr = xmlDoc.CreateAttribute("border");
        //    //borderAttr.Value = tableBorderStatus;
        //    //XmlAttribute headrowAttr = xmlDoc.CreateAttribute("head-row");
        //    //headrowAttr.Value = headerRowStatus;
        //    //XmlAttribute pageAttr = xmlDoc.CreateAttribute("page");
        //    //pageAttr.Value = Convert.ToString(page);

        //    //finalTable.Attributes.Append(idAttr);
        //    //finalTable.Attributes.Append(borderAttr);
        //    //finalTable.Attributes.Append(headrowAttr);
        //    //finalTable.Attributes.Append(pageAttr);

        //    //XmlNode headerNode = xmlDoc.CreateElement("header");
        //    //finalTable.AppendChild(headerNode);
        //    //XmlNode headrowNode = xmlDoc.CreateElement("head-row");
        //    //finalTable.AppendChild(headrowNode);
        //    //XmlNode RowNode = xmlDoc.CreateElement("Row");
        //    //finalTable.AppendChild(RowNode);
        //    //XmlNode captionNode = xmlDoc.CreateElement("caption");
        //    //finalTable.AppendChild(captionNode);
        //    //XmlNode voiceDescriptionNode = xmlDoc.CreateElement("voice-description");
        //    //finalTable.AppendChild(voiceDescriptionNode);


        //    //////XmlNodeList tableHeaderLines = tableNode.SelectNodes("//header/descendant::ln");
        //    //////XmlNodeList tableCaptionLines = tableNode.SelectNodes("//caption/descendant::ln");


        //    XmlNodeList tableHeaderLines = tableNode.SelectNodes("//header/descendant::ln");
        //    XmlNodeList tableCaptionLines = tableNode.SelectNodes("//caption/descendant::ln");

        //    //tableCaptioncol.InnerText = divCaptionRow.InnerText;
        //    //tableCaption.AppendChild(tableCaptioncol);

        //    StringBuilder colText = new StringBuilder();

        //    int tblHeaderRowCount = 0;
        //    int tblCaptionRowCount = 0;

        //    //var taleHeaderLines = tableNode.Cast<XmlNode>().TakeWhile(x => x.ChildNodes.Count == 1).ToList();

        //    Process1 obj = new Process1();

        //    if (!string.IsNullOrEmpty(divHeaderRowText))
        //    {
        //        //Converting header row to para
        //        if (headerSelectType == 0)
        //        {
        //            if (!string.IsNullOrEmpty(divHeaderRowText) && tableHeaderLines.Count > 0)
        //            {
        //                obj.ClearHeaderRowText();

        //                foreach (XmlNode headerLine in tableHeaderLines)
        //                {
        //                    headerLine.ParentNode.ParentNode.RemoveChild(headerLine.ParentNode);
        //                }
        //            }
        //        }
        //        //Adding header rows to start of table body
        //        else if (headerSelectType == 1)
        //        {
        //            if (!string.IsNullOrEmpty(divHeaderRowText) && tableHeaderLines.Count > 0)
        //            {
        //                obj.ClearHeaderRowText();

        //                var tblBodyRows = tableNode.SelectNodes("//Row");

        //                if (tblBodyRows.Count > 0)
        //                {
        //                    for (int i = tableHeaderLines.Count - 1; i >= 0; i--)
        //                    {
        //                        tblBodyRows[0].ParentNode.InsertBefore(tableHeaderLines[i], tblBodyRows[0]);
        //                    }
        //                }
        //            }
        //        }
        //        //Header rows are correctly detected
        //        else if (headerSelectType == 2)
        //        {
        //            if (!string.IsNullOrEmpty(divHeaderRowText) && tableHeaderLines.Count > 0)
        //            {
        //                tblHeaderRowCount = tableHeaderLines.Count;

        //                //var colLine = GetRowLinesWithCoord(tableHeaderLines, page);
        //                //tableHeadercol.InnerText = colLine.Trim();
        //                //tableHeader.AppendChild(tableHeadercol);
        //            }
        //        }
        //        //end
        //    }

        //    if (!string.IsNullOrEmpty(divCaptionRowText))
        //    {
        //        //Converting caption rows to para
        //        if (captionSelectType == 0)
        //        {
        //            if (!string.IsNullOrEmpty(divCaptionRowText) && tableCaptionLines.Count > 0)
        //            {
        //                obj.ClearCaptionRowText();

        //                foreach (XmlNode captionLine in tableCaptionLines)
        //                {
        //                    captionLine.ParentNode.ParentNode.RemoveChild(captionLine.ParentNode);
        //                }
        //            }
        //        }

        //        //Adding caption rows to end of table body
        //        else if (captionSelectType == 1)
        //        {
        //            if (!string.IsNullOrEmpty(divCaptionRowText) && tableCaptionLines.Count > 0)
        //            {
        //                obj.ClearCaptionRowText();

        //                var tblBodyLastRow = tableNode.SelectNodes("//Row").Cast<XmlNode>().Last();

        //                if (tblBodyLastRow != null)
        //                {
        //                    for (int i = tableCaptionLines.Count - 1; i >= 0; i--)
        //                    {
        //                        tblBodyLastRow.ParentNode.InsertBefore(tableCaptionLines[i], tblBodyLastRow);
        //                    }
        //                }
        //            }
        //        }
        //        //Caption rows are correctly detected
        //        else if (captionSelectType == 2)
        //        {
        //            if (!string.IsNullOrEmpty(divCaptionRowText) && tableCaptionLines.Count > 0)
        //            {
        //                tblCaptionRowCount = tableCaptionLines.Count;

        //                //var colLine = GetRowLinesWithCoord(tableCaptionLines, page);
        //                //tableCaptioncol.InnerText = colLine.Trim();
        //                //tableCaption.AppendChild(tableCaptioncol);
        //            }
        //        }
        //        //end
        //    }

        //    colText.Length = 0;

        //    bool isInitialXml = false;

        //    List<string> llyList = new List<string>();
        //    llyList = tableNode.SelectNodes("descendant::Box/@lly").Cast<XmlNode>().Select(x => x.Value).Distinct().ToList();

        //    if (llyList.Count > 0)
        //        isInitialXml = true;

        //    //var rows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Skip(tblHeaderRowCount).Where(x => x.ChildNodes.Count >= 1).ToList();

        //    ////var bodyRows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Skip(tblHeaderRowCount).ToList();
        //    ////int bodyRowsCount = bodyRows.Count;
        //    ////var captionRows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Reverse().TakeWhile(x => x.ChildNodes.Count == 1).ToList();
        //    ////var rows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Take(bodyRowsCount - captionRows.Count).ToList();

        //    var allBodyRows = tableNode.SelectNodes("//Row").Cast<XmlNode>().ToList();
        //    var rowsWithoutHeader = tableNode.SelectNodes("//Row").Cast<XmlNode>().Skip(tblHeaderRowCount).ToList();
        //    var rowsWithoutCaption = tableNode.SelectNodes("//Row").Cast<XmlNode>().Reverse().Skip(tblCaptionRowCount).ToList();

        //    var rows = tableNode.SelectNodes("//Row").Cast<XmlNode>().Skip(tblHeaderRowCount).Take(rowsWithoutHeader.Count - tblCaptionRowCount).ToList();

        //    int colIndex = 0;

        //    for (int i = 0; i < rows.Count; i++)
        //    {
        //        if (i == 0 && headerRowStatus.Equals("off"))
        //        {
        //            XmlNode headeRow = tableNode.SelectSingleNode("//head-row");
        //            XmlNode headercol = null;
        //            XmlAttribute headerWidth = null;

        //            if (colWidthList.Count > 0 && colIndex < colWidthList.Count)
        //            {
        //                foreach (string colWidth in colWidthList)
        //                {
        //                    headercol = xmlDoc.CreateElement("head-col");
        //                    headerWidth = xmlDoc.CreateAttribute("width");

        //                    headerWidth.Value = colWidth;
        //                    headercol.Attributes.Append(headerWidth);
        //                    headercol.InnerText = "";
        //                    headeRow.AppendChild(headercol);
        //                }

        //                XmlNode lastRow = tableNode.SelectNodes("//Row").Cast<XmlNode>().Last();
        //                XmlNode rowNode = xmlDoc.CreateElement("Row");

        //                if (isInitialXml)
        //                    rowNode = CombineTableLine(xmlDoc, rows[i], rowNode, page, false);
        //                else
        //                    rowNode.InnerXml = rows[i].InnerXml;

        //                lastRow.ParentNode.InsertAfter(rowNode, lastRow);
        //            }
        //        }
        //        else if (i == 0 && headerRowStatus.Equals("on"))
        //        {
        //            XmlNode headeRow = tableNode.SelectSingleNode("//head-row");
        //            headeRow = CombineTableLine(xmlDoc, rows[i], headeRow, page, true);
        //        }
        //        else
        //        {
        //            XmlNode lastRow = tableNode.SelectNodes("//Row").Cast<XmlNode>().Last();
        //            XmlNode rowNode = xmlDoc.CreateElement("Row");

        //            rowNode = CombineTableLine(xmlDoc, rows[i], rowNode, page, false);
        //            lastRow.ParentNode.InsertAfter(rowNode, lastRow);
        //        }
        //    }
        //    tableNode.SelectNodes("//Row").Cast<XmlNode>().First().ParentNode.RemoveChild(tableNode.SelectNodes("//Row").Cast<XmlNode>().First());

        //    string dirPath = mainDirectoryPath + "\\" + bookId + "\\" + bookId + "-1\\Table";
        //    string xmlDirPath = dirPath + "//TableXmls";

        //    if (!File.Exists(xmlDirPath))
        //        Directory.CreateDirectory(xmlDirPath);

        //    string tableSavingPath = xmlDirPath + "//" + "Table_" + page + "_" + tableId + ".xml";
        //    //xmlDoc.InnerXml = finalTable.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");

        //    try
        //    {
        //        xmlDoc.InnerXml = tableNode.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");

        //        if (File.Exists(tableSavingPath))
        //            File.Delete(tableSavingPath);
        //    }
        //    catch (Exception)
        //    {

        //    }

        //    ////XmlNode finalUpdatedXml = UpdateXmlByNewRowCol(finalTable, finalTableHtml, xmlDoc, page);

        //    ////if (finalUpdatedXml == null)
        //    ////{
        //    xmlDoc.InnerXml = tableNode.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");
        //    xmlDoc.Save(tableSavingPath);
        //    ////}
        //    ////else
        //    ////{
        //    ////    xmlDoc.InnerXml = finalUpdatedXml.OuterXml.Replace("&lt;", "<").Replace("&gt;", ">");
        //    ////    xmlDoc.Save(tableSavingPath);
        //    ////}

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

        public XmlNode CombineTableLineFinalXml(XmlDocument xmlDoc, XmlNode rows, XmlNode newRowNode, int page, bool isHeadRow)
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

            var columnsList = rows.SelectNodes("descendant::col").Cast<XmlNode>().ToList();
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

        //public XmlNode UpdateXmlByNewRowCol(XmlNode finalTable, string html5ViewerTableHtml, XmlDocument xmlDoc, int page)
        //{
        //    //trList[1].InnerText.Split(new string[] { "\r\n" }, StringSplitOptions.None);

        //    try
        //    {
        //        finalTable.InnerXml = finalTable.InnerXml.Replace("&lt;", "<").Replace("&gt;", ">");


        //        HtmlDocument doc = new HtmlDocument();
        //        doc.LoadHtml(html5ViewerTableHtml);

        //        List<string> innerTextList = new List<string>();
        //        List<string> finalTextList = null;
        //        bool containsMergedRows = false;

        //        List<XmlNode> matchedNodes = new List<XmlNode>();
        //        List<HtmlNode> html5ViewerTrList = doc.DocumentNode.SelectNodes("//tr").ToList();
        //        List<XmlNode> rowsList = finalTable.SelectNodes("//Row | //head-row").Cast<XmlNode>().ToList();

        //        List<HtmlNode> mergedRows = doc.DocumentNode.SelectNodes("//tr").Cast<HtmlNode>().Where(x => x.Attributes["style"] != null &&
        //                                                                             x.Attributes["style"].Value.Equals("white-space: pre")).ToList();
        //        if (mergedRows.Count > 0)
        //        {
        //            containsMergedRows = true;
        //            InsertMergedRows(finalTable, mergedRows);
        //        }

        //        double llx = 0;
        //        double lly = 0;
        //        double urx = 0;
        //        double ury = 0;

        //        double verticalLineDiff = 6;
        //        double horizontalLineDiff = 6;

        //        //int maxColumns = finalTable.SelectNodes("//Row | //head-row").Cast<XmlNode>().ToList().Select(x=>x.ChildNodes.Count).Max();
        //        //var maxColRow = finalTable.SelectNodes("//Row | //head-row").Cast<XmlNode>().Where(x => x.ChildNodes.Count == maxColumns).Take(1).ToList();

        //        //var left1 = maxColRow.Where(x => x.ChildNodes.Count > 1 && x.ChildNodes[0].ChildNodes != null && x.ChildNodes[0].ChildNodes.Count > 0)
        //        //                  .Select(y => Convert.ToDouble(y.ChildNodes[0].ChildNodes[0].Attributes["left"].Value));

        //        //var left2 = maxColRow.Where(x => x.ChildNodes.Count > 1 && x.ChildNodes[0].ChildNodes != null && x.ChildNodes[0].ChildNodes.Count > 1)
        //        //                  .Select(y => Convert.ToDouble(y.ChildNodes[0].ChildNodes[1].Attributes["left"].Value));

        //        //if (maxColRow.Count > 0)
        //        //{
        //        //    foreach (XmlNode col in maxColRow)
        //        //    {

        //        //    }
        //        //}

        //        //var left = finalTable.SelectNodes("descendant::ln/@left");

        //        string coord = string.Empty;
        //        string font = string.Empty;
        //        string fontSize = string.Empty;
        //        string fontType = string.Empty;
        //        string top = string.Empty;
        //        string height = string.Empty;
        //        string left = string.Empty;
        //        List<string> coordList = new List<string>();

        //        if (html5ViewerTrList.Count > 0 && rowsList.Count > 0)
        //        {
        //            //Case 1 when rows of html5 viewer's table and xml's table are equal
        //            if (html5ViewerTrList.Count == rowsList.Count)
        //            {
        //                for (int i = 0; i < rowsList.Count; i++)
        //                {
        //                    //if (i == 1 && rowsList[i].ChildNodes.Count > 1)
        //                    //{
        //                    //    if (trList[0].ChildNodes.Count > rowsList[0].ChildNodes.Count)
        //                    //    {
        //                    //        if (rowsList[2].ChildNodes[0].ChildNodes[0] != null &&
        //                    //            rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" left=\"" },
        //                    //                            StringSplitOptions.None).ToList().Count > 0)
        //                    //        {
        //                    //            horizontalLineDiff = Math.Abs(Convert.ToDouble(rowsList[2].ChildNodes[0].ChildNodes[0].InnerText.Split(':')[2]) -
        //                    //                    Convert.ToDouble(rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" left=\"" },
        //                    //                            StringSplitOptions.None)[1].Split(new string[] { "\" top=\"" }, StringSplitOptions.None)[0]));
        //                    //        }
        //                    //    }
        //                    //}

        //                    ////if (i == 1 && rowsList[i].ChildNodes.Count > 1)
        //                    ////{
        //                    ////    horizontalLineDiff = GetHorizontalLineDiff(rowsList);
        //                    ////}

        //                    for (int j = 0; j < rowsList[i].ChildNodes.Count; j++)
        //                    {
        //                        rowsList[i].ChildNodes[j].InnerXml = rowsList[i].ChildNodes[j].InnerXml.Replace("&lt;", "<").Replace("&gt;", ">");

        //                        if (!string.IsNullOrEmpty(rowsList[i].ChildNodes[j].InnerXml))
        //                        {
        //                            string tempInnerxml = rowsList[i].ChildNodes[j].InnerXml.Split(new string[] { "fonttype=\"Embeded\">" }, StringSplitOptions.None)[0] +
        //                           " fonttype=\"Embeded\">" + (html5ViewerTrList[i].ChildNodes[j] != null ? html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() : "") + "</ln>";

        //                            rowsList[i].ChildNodes[j].InnerText = (html5ViewerTrList[i].ChildNodes[j] != null ? html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() : "");
        //                            rowsList[i].ChildNodes[j].InnerXml = tempInnerxml;
        //                        }
        //                    }
        //                }

        //                //When new columns are added in html5 viewer's table 
        //                if (html5ViewerTrList[0].ChildNodes.Count > rowsList[0].ChildNodes.Count)
        //                {
        //                    int newColumnCount = html5ViewerTrList[0].ChildNodes.Count - rowsList[0].ChildNodes.Count;

        //                    for (int i = 0; i < html5ViewerTrList.Count; i++)
        //                    {
        //                        for (int j = 0; j < html5ViewerTrList[i].ChildNodes.Count; j++)
        //                        {
        //                            if (j >= html5ViewerTrList[i].ChildNodes.Count - newColumnCount)
        //                            {
        //                                XmlNode column = xmlDoc.CreateElement("col");

        //                                if (rowsList[rowsList.Count - 1].ChildNodes.Count > 0 & !string.IsNullOrEmpty(rowsList[i].ChildNodes[0].InnerXml))
        //                                {
        //                                    llx = Convert.ToDouble(rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { ":" }, StringSplitOptions.None)[0]
        //                                            .Split(new string[] { "<ln coord=\"" }, StringSplitOptions.None)[1]) +
        //                                          horizontalLineDiff;

        //                                    lly = Convert.ToDouble(rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { ":" },
        //                                                StringSplitOptions.None)[1]);

        //                                    urx = Convert.ToDouble(rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { ":" },
        //                                                StringSplitOptions.None)[2]) + horizontalLineDiff;

        //                                    ury = Convert.ToDouble(rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { "\" page=\"" },
        //                                                StringSplitOptions.None)[0]
        //                                                .Split(new string[] { ":" }, StringSplitOptions.None)[3]);

        //                                    top = rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { "\" top=\"" }, StringSplitOptions.None)[1]
        //                                                    .Split(new string[] { "\" font=\"" }, StringSplitOptions.None)[0];

        //                                    font = rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { "\" font=\"" }, StringSplitOptions.None)[1]
        //                                                .Split(new string[] { "\" fontsize=\"" }, StringSplitOptions.None)[0];

        //                                    fontSize = rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { "\" fontsize=\"" }, StringSplitOptions.None)[1]
        //                                                .Split(new string[] { "\" fonttype=\"" }, StringSplitOptions.None)[0];

        //                                    height = rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { "\" height=\"" }, StringSplitOptions.None)[1]
        //                                                .Split(new string[] { "\" left=\"" }, StringSplitOptions.None)[0];

        //                                    left = rowsList[i].ChildNodes[0].InnerXml.Split(new string[] { "\" left=\"" }, StringSplitOptions.None)[1]
        //                                                .Split(new string[] { "\" top=\"" }, StringSplitOptions.None)[0];
        //                                }

        //                                coord = llx + ":" + lly + ":" + urx + ":" + ury;
        //                                string newLn = "<ln coord=\"" + coord + "\" " + "page=\"" + page + "\" height=\"" +
        //                                               height + "\" left=\"" + left + "\" top=\"" +
        //                                               top + "\" font=\"" + font + "\" fontsize=\"" + fontSize +
        //                                               "\" fonttype=\"" + fontType + "\" >";

        //                                column.InnerText = newLn + html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() + "</ln>";
        //                                rowsList[i].AppendChild(column);
        //                            }
        //                        }
        //                    }
        //                }
        //            }//end case 1

        //            //Case 2 when rows of html5 viewer's table and xml's table are equal
        //            else if (html5ViewerTrList.Count < rowsList.Count && !containsMergedRows)
        //            {
        //                for (int i = 0; i < rowsList.Count; i++)
        //                {
        //                    if (i < html5ViewerTrList.Count)
        //                    {
        //                        for (int j = 0; j < rowsList[i].ChildNodes.Count; j++)
        //                        {
        //                            rowsList[i].ChildNodes[j].InnerXml = rowsList[i].ChildNodes[j].InnerXml.Replace("&lt;", "<").Replace("&gt;", ">");

        //                            string tempInnerxml = rowsList[i].ChildNodes[j].InnerXml.Split(new string[] { "fonttype=\"Embeded\">" }, StringSplitOptions.None)[0] + " fonttype=\"Embeded\">" + (html5ViewerTrList[i].ChildNodes[j] != null ? html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() : "") + "</ln>";

        //                            rowsList[i].ChildNodes[j].InnerText = (html5ViewerTrList[i].ChildNodes[j] != null ? html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() : "");
        //                            rowsList[i].ChildNodes[j].InnerXml = tempInnerxml;
        //                        }
        //                    }
        //                }
        //            }
        //            //Case 3 when rows of html5 viewer's table and xml's table are not equal
        //            else if (html5ViewerTrList.Count > rowsList.Count && !containsMergedRows)
        //            {
        //                for (int i = 0; i < rowsList.Count; i++)
        //                {
        //                    if (i == 1 && rowsList[i].ChildNodes.Count > 1)
        //                    {
        //                        //To do
        //                        //verticalLineDiff = Math.Abs(Convert.ToDouble(rowsList[2].ChildNodes[0].ChildNodes[0].InnerText.Split(':')[1]) -
        //                        //                            Convert.ToDouble(rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(':')[1]));

        //                        //top = rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { ":" }, StringSplitOptions.None)[1];
        //                        //font = rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" font=\"" },
        //                        //        StringSplitOptions.None)[1].Split(new string[] { "\" fontsize=\"" }, StringSplitOptions.None)[0];
        //                        //fontSize = rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" fontsize=\"" }, StringSplitOptions.None)[1]
        //                        //                      .Split(new string[] { "\" fonttype=\"" }, StringSplitOptions.None)[0];
        //                        //fontType = rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" fonttype=\"" }, StringSplitOptions.None)[1]
        //                        //                      .Split(new string[] { "\" >" }, StringSplitOptions.None)[0];
        //                        //height = rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" height=\"" }, StringSplitOptions.None)[1]
        //                        //                    .Split(new string[] { "\" left=\"" }, StringSplitOptions.None)[0];
        //                        //left = rowsList[1].ChildNodes[0].ChildNodes[0].InnerText.Split(new string[] { "\" left=\"" }, StringSplitOptions.None)[1]
        //                        //            .Split(new string[] { "\" top=\"" }, StringSplitOptions.None)[0];

        //                        //verticalLineDiff = 3;

        //                        //top = "";
        //                        //font = "";
        //                        //fontSize = "";
        //                        //fontType = "";
        //                        //height = "";
        //                        //left = "";

        //                        verticalLineDiff = 3;

        //                        top = "";
        //                        font = "";
        //                        fontSize = "";
        //                        fontType = "";
        //                        height = "";
        //                        left = "";
        //                    }

        //                    for (int j = 0; j < rowsList[i].ChildNodes.Count; j++)
        //                    {
        //                        rowsList[i].ChildNodes[j].InnerXml = rowsList[i].ChildNodes[j].InnerXml.Replace("&lt;", "<").Replace("&gt;", ">");

        //                        string tempInnerxml = rowsList[i].ChildNodes[j].InnerXml.Split(new string[] { "fonttype=\"Embeded\">" },
        //                                StringSplitOptions.None)[0] + " fonttype=\"Embeded\">" + (html5ViewerTrList[i].ChildNodes[j] != null ? html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() : "") + "</ln>";

        //                        rowsList[i].ChildNodes[j].InnerText = (html5ViewerTrList[i].ChildNodes[j] != null                                     ? html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() : ""); 

        //                        rowsList[i].ChildNodes[j].InnerXml = tempInnerxml;
        //                    }
        //                }

        //                var newRowsList = doc.DocumentNode.SelectNodes("//tr").Skip(rowsList.Count - 1).ToList();

        //                for (int i = 0; i < newRowsList.Count; i++)
        //                {
        //                    XmlNode newRow = xmlDoc.CreateElement("Row");

        //                    for (int j = 0; j < newRowsList[i].ChildNodes.Count; j++)
        //                    {
        //                        XmlNode column = xmlDoc.CreateElement("col");

        //                        if (rowsList[rowsList.Count - 1].ChildNodes.Count > 0)
        //                        {
        //                            llx = Convert.ToDouble(rowsList[rowsList.Count - 1].ChildNodes[0].InnerXml.Split(new string[] { ":" }, StringSplitOptions.None)[0]
        //                                              .Split(new string[] { "<ln coord=\"" }, StringSplitOptions.None)[1]);

        //                            lly = Convert.ToDouble(rowsList[rowsList.Count - 1].ChildNodes[0].InnerXml.Split(new string[] { ":" }, StringSplitOptions.None)[1]) + verticalLineDiff;

        //                            urx = Convert.ToDouble(rowsList[rowsList.Count - 1].ChildNodes[0].InnerXml.Split(new string[] { ":" }, StringSplitOptions.None)[2]);

        //                            ury = Convert.ToDouble(rowsList[rowsList.Count - 1].ChildNodes[0].InnerXml.Split(new string[] { "\" page=\"" }, StringSplitOptions.None)[0]
        //                                              .Split(new string[] { ":" }, StringSplitOptions.None)[3]) + verticalLineDiff;
        //                        }

        //                        coord = llx + ":" + lly + ":" + urx + ":" + ury;
        //                        string newLn = "<ln coord=\"" + coord + "\" " + "page=\"" + page + "\" height=\"" + height + "\" left=\"" + left + "\" top=\"" +
        //                                       top + "\" font=\"" + font + "\" fontsize=\"" + fontSize + "\" fonttype=\"" + fontType + "\" >";

        //                        column.InnerText = newLn + newRowsList[i].ChildNodes[j].InnerText.Trim() + "</ln>";
        //                        newRow.AppendChild(column);
        //                    }
        //                    XmlNode lastRow = finalTable.SelectNodes("//Row").Count > 0 ? finalTable.SelectNodes("//Row").Cast<XmlNode>().Last() : null;

        //                    if (lastRow != null)
        //                        lastRow.ParentNode.InsertAfter(newRow, lastRow);
        //                }
        //            }
        //        }

        //        return finalTable;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //    }
        //}

        public XmlNode UpdateXmlByNewRowCol(XmlNode finalTable, string html5ViewerTableHtml, XmlDocument xmlDoc, int page)
        {
            if (finalTable == null || string.IsNullOrEmpty(html5ViewerTableHtml) || xmlDoc == null || page < 1) return null;

            try
            {
                bool containsMergedRows = false;

                finalTable.InnerXml = finalTable.InnerXml.Replace("&lt;", "<").Replace("&gt;", ">");

                HtmlDocument doc = new HtmlDocument();
                doc.LoadHtml(html5ViewerTableHtml);

                List<HtmlNode> html5ViewerTrList = doc.DocumentNode.SelectNodes("//tr").ToList();
                //List<XmlNode> rowsList = finalTable.SelectNodes("//Row | //head-row").Cast<XmlNode>().ToList();

                List<XmlNode> rowsList = finalTable.SelectNodes("//Row | //head-row[descendant::head-col/ln]").Cast<XmlNode>().ToList();

                if ((html5ViewerTrList == null || html5ViewerTrList.Count == 0) || (rowsList == null || rowsList.Count == 0)) return null;

                List<HtmlNode> mergedRows = doc.DocumentNode.SelectNodes("//tr").Cast<HtmlNode>().Where(x => x.Attributes["style"] != null &&
                                                                                     x.Attributes["style"].Value.Equals("white-space: pre")).ToList();

                if (mergedRows == null) return null;

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
                string coord = string.Empty;

                #region  Case 1 when rows of html5 viewer's table and xml's table are equal

                if (html5ViewerTrList.Count == rowsList.Count)
                {
                    if ((html5ViewerTrList[0].ChildNodes != null && html5ViewerTrList[0].ChildNodes.Count > 0) &&
                        (rowsList[0].ChildNodes != null && rowsList[0].ChildNodes.Count > 0))
                    {
                        //When html5 viewer and xml column are equal 
                        if (html5ViewerTrList[0].ChildNodes.Count == rowsList[0].ChildNodes.Count)
                        {
                            for (int i = 0; i < rowsList.Count; i++)
                            {
                                for (int j = 0; j < rowsList[i].ChildNodes.Count; j++)
                                {
                                    if (!string.IsNullOrEmpty(rowsList[i].ChildNodes[j].InnerXml))
                                    {
                                        string innerText = rowsList[i].ChildNodes[j].InnerText.Trim();
                                        string innerXml = rowsList[i].ChildNodes[j].InnerXml;

                                        rowsList[i].ChildNodes[j].InnerXml = innerXml.Replace(innerText, html5ViewerTrList[i].ChildNodes[j].InnerText.Trim());
                                    }
                                }
                            }
                        }
                        //When html5 viewer table's columns are greater then xml table's column
                        if (html5ViewerTrList[0].ChildNodes.Count > rowsList[0].ChildNodes.Count)
                        {
                            TetmlLine line = GetLineParameters(rowsList[0]);
                            horizontalLineDiff = GetHorizontalDiff(rowsList);

                            int newColumnCount = html5ViewerTrList[0].ChildNodes.Count - rowsList[0].ChildNodes.Count;

                            for (int i = 0; i < html5ViewerTrList.Count; i++)
                            {
                                for (int j = 0; j < html5ViewerTrList[i].ChildNodes.Count; j++)
                                {
                                    if (j >= html5ViewerTrList[i].ChildNodes.Count - newColumnCount)
                                    {
                                        XmlNode column = xmlDoc.CreateElement("col");

                                        if (rowsList[i].ChildNodes.Count > 0 & !string.IsNullOrEmpty(rowsList[i].ChildNodes[0].InnerXml))
                                        {
                                            TetmlLine lastLineCol = GetColLineCoord(rowsList[i].ChildNodes[0]);

                                            if (rowsList[rowsList.Count - 1].ChildNodes.Count > 0)
                                            {
                                                llx = Convert.ToDouble(lastLineCol.Llx) + horizontalLineDiff;
                                                lly = Convert.ToDouble(lastLineCol.Lly);
                                                urx = Convert.ToDouble(lastLineCol.Urx) + horizontalLineDiff;
                                                ury = Convert.ToDouble(lastLineCol.Ury);
                                            }
                                        }

                                        coord = llx + ":" + lly + ":" + urx + ":" + ury;
                                        string newLn = "<ln coord=\"" + coord + "\" " + "page=\"" + page + "\" height=\"" +
                                                       line.Height + "\" left=\"" + line.Left + "\" top=\"" +
                                                       line.Top + "\" font=\"" + line.Font + "\" fontsize=\"" + line.FontSize +
                                                       "\" fonttype=\"" + line.FontType + "\" >";

                                        column.InnerText = newLn + html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() + "</ln>";
                                        rowsList[i].AppendChild(column);
                                    }
                                }
                            }
                        }
                        //When html5 viewer table's columns are less then xml table's column
                        if (html5ViewerTrList[0].ChildNodes.Count < rowsList[0].ChildNodes.Count)
                        {
                            TetmlLine line = GetLineParameters(rowsList[0]);

                            for (int i = 0; i < html5ViewerTrList.Count; i++)
                            {
                                for (int j = 0; j < html5ViewerTrList[i].ChildNodes.Count; j++)
                                {
                                    if (j <= rowsList[0].ChildNodes.Count)
                                    {
                                        XmlNode column = xmlDoc.CreateElement("col");

                                        if (rowsList[i].ChildNodes.Count > 0 &
                                            !string.IsNullOrEmpty(rowsList[i].ChildNodes[0].InnerXml))
                                        {
                                            TetmlLine lastLineCol = GetColLineCoord(rowsList[i].ChildNodes[0]);

                                            if (rowsList[rowsList.Count - 1].ChildNodes.Count > 0)
                                            {
                                                llx = Convert.ToDouble(lastLineCol.Llx) + horizontalLineDiff;
                                                lly = Convert.ToDouble(lastLineCol.Lly);
                                                urx = Convert.ToDouble(lastLineCol.Urx) + horizontalLineDiff;
                                                ury = Convert.ToDouble(lastLineCol.Ury);
                                            }
                                        }

                                        coord = llx + ":" + lly + ":" + urx + ":" + ury;
                                        string newLn = "<ln coord=\"" + coord + "\" " + "page=\"" + page +
                                                       "\" height=\"" +
                                                       line.Height + "\" left=\"" + line.Left + "\" top=\"" +
                                                       line.Top + "\" font=\"" + line.Font + "\" fontsize=\"" +
                                                       line.FontSize +
                                                       "\" fonttype=\"" + line.FontType + "\" >";

                                        column.InnerText = newLn +
                                                           html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() +
                                                           "</ln>";
                                        rowsList[i].AppendChild(column);
                                    }
                                    else
                                    {
                                        rowsList[i].ParentNode.RemoveChild(rowsList[i]);
                                    }
                                }
                            }
                        }
                    }//end null child check
                }//end equal row check

                #endregion end case 1

                #region  Case 2 when rows of html5 viewer's table are less then xml's table

                else if (html5ViewerTrList.Count < rowsList.Count && !containsMergedRows)
                {
                    if ((html5ViewerTrList[0].ChildNodes != null && html5ViewerTrList[0].ChildNodes.Count > 0) &&
                        (rowsList[0].ChildNodes != null && rowsList[0].ChildNodes.Count > 0))
                    {
                        //When html5 viewer table columns and xml table columns are equal 
                        if (html5ViewerTrList[0].ChildNodes.Count == rowsList[0].ChildNodes.Count)
                        {
                            for (int i = 0; i < rowsList.Count; i++)
                            {
                                for (int j = 0; j < rowsList[i].ChildNodes.Count; j++)
                                {
                                    if (!string.IsNullOrEmpty(rowsList[i].ChildNodes[j].InnerXml))
                                    {
                                        string innerText = rowsList[i].ChildNodes[j].InnerText;
                                        string innerXml = rowsList[i].ChildNodes[j].InnerXml;

                                        rowsList[i].ChildNodes[j].InnerXml = innerXml.Replace(innerText, html5ViewerTrList[i].ChildNodes[j].InnerText.Trim());
                                    }
                                }
                            }
                        }
                        //When html5 viewer table columns are greater then xml table columns
                        else if (html5ViewerTrList[0].ChildNodes.Count > rowsList[0].ChildNodes.Count)
                        {
                            TetmlLine line = GetLineParameters(rowsList[0]);
                            horizontalLineDiff = GetHorizontalDiff(rowsList);

                            int newColumnCount = html5ViewerTrList[0].ChildNodes.Count - rowsList[0].ChildNodes.Count;

                            for (int i = 0; i < html5ViewerTrList.Count; i++)
                            {
                                for (int j = 0; j < html5ViewerTrList[i].ChildNodes.Count; j++)
                                {
                                    if (j >= html5ViewerTrList[i].ChildNodes.Count - newColumnCount)
                                    {
                                        XmlNode column = xmlDoc.CreateElement("col");

                                        if (rowsList[i].ChildNodes.Count > 0 & !string.IsNullOrEmpty(rowsList[i].ChildNodes[0].InnerXml))
                                        {
                                            TetmlLine lastLineCol = GetColLineCoord(rowsList[i].ChildNodes[0]);

                                            if (rowsList[rowsList.Count - 1].ChildNodes.Count > 0)
                                            {
                                                llx = Convert.ToDouble(lastLineCol.Llx) + horizontalLineDiff;
                                                lly = Convert.ToDouble(lastLineCol.Lly);
                                                urx = Convert.ToDouble(lastLineCol.Urx) + horizontalLineDiff;
                                                ury = Convert.ToDouble(lastLineCol.Ury);
                                            }
                                        }

                                        coord = llx + ":" + lly + ":" + urx + ":" + ury;
                                        string newLn = "<ln coord=\"" + coord + "\" " + "page=\"" + page + "\" height=\"" +
                                                       line.Height + "\" left=\"" + line.Left + "\" top=\"" +
                                                       line.Top + "\" font=\"" + line.Font + "\" fontsize=\"" + line.FontSize +
                                                       "\" fonttype=\"" + line.FontType + "\" >";

                                        column.InnerText = newLn + html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() + "</ln>";
                                        rowsList[i].AppendChild(column);
                                    }
                                }
                            }
                        }
                        //When html5 viewer table columns are less then xml table columns
                        else if (html5ViewerTrList[0].ChildNodes.Count < rowsList[0].ChildNodes.Count)
                        {
                            TetmlLine line = GetLineParameters(rowsList[0]);

                            for (int i = 0; i < html5ViewerTrList.Count; i++)
                            {
                                for (int j = 0; j < html5ViewerTrList[i].ChildNodes.Count; j++)
                                {
                                    if (j <= rowsList[0].ChildNodes.Count)
                                    {
                                        XmlNode column = xmlDoc.CreateElement("col");

                                        if (rowsList[i].ChildNodes.Count > 0 && !string.IsNullOrEmpty(rowsList[i].ChildNodes[0].InnerXml))
                                        {
                                            TetmlLine lastLineCol = GetColLineCoord(rowsList[i].ChildNodes[0]);

                                            if (rowsList[rowsList.Count - 1].ChildNodes.Count > 0)
                                            {
                                                llx = Convert.ToDouble(lastLineCol.Llx) + horizontalLineDiff;
                                                lly = Convert.ToDouble(lastLineCol.Lly);
                                                urx = Convert.ToDouble(lastLineCol.Urx) + horizontalLineDiff;
                                                ury = Convert.ToDouble(lastLineCol.Ury);
                                            }
                                        }

                                        coord = llx + ":" + lly + ":" + urx + ":" + ury;
                                        string newLn = "<ln coord=\"" + coord + "\" " + "page=\"" + page +
                                                       "\" height=\"" +
                                                       line.Height + "\" left=\"" + line.Left + "\" top=\"" +
                                                       line.Top + "\" font=\"" + line.Font + "\" fontsize=\"" +
                                                       line.FontSize +
                                                       "\" fonttype=\"" + line.FontType + "\" >";

                                        column.InnerText = newLn +
                                                           html5ViewerTrList[i].ChildNodes[j].InnerText.Trim() +
                                                           "</ln>";
                                        rowsList[i].AppendChild(column);
                                    }
                                    else
                                    {
                                        rowsList[i].ParentNode.RemoveChild(rowsList[i]);
                                    }
                                }
                            }
                        }
                    }

                    //for (int i = 0; i < rowsList.Count; i++)
                    //{
                    //    if (i < html5ViewerTrList.Count)
                    //    {
                    //        for (int j = 0; j < rowsList[i].ChildNodes.Count; j++)
                    //        {
                    //            if (!string.IsNullOrEmpty(rowsList[i].ChildNodes[j].InnerXml))
                    //            {
                    //                string innerText = rowsList[i].ChildNodes[j].InnerText;
                    //                string innerXml = rowsList[i].ChildNodes[j].InnerXml;

                    //                rowsList[i].ChildNodes[j].InnerXml = innerXml.Replace(innerText, html5ViewerTrList[i].ChildNodes[j].InnerText.Trim());
                    //            }
                    //        }
                    //    }
                    //}
                }

                #endregion

                #region Case 3 when rows of html5 viewer's table are greater then xml's table

                else if (html5ViewerTrList.Count > rowsList.Count && !containsMergedRows)
                {
                    TetmlLine line = null;

                    //When html5 viewer and xml column are equal 

                    //Update inner text of first equal rows and assign coordinates
                    for (int i = 0; i < rowsList.Count; i++)
                    {
                        if (i == 1 && rowsList[i].ChildNodes != null)
                        {
                            if (rowsList[i].ChildNodes.Count > 1)
                            {
                                line = GetLineParameters(rowsList[i]);

                                if (rowsList.Count > 1)
                                    verticalLineDiff = GetVerticalDiff(rowsList);
                            }
                        }

                        if (rowsList[i].ChildNodes != null)
                        {
                            for (int j = 0; j < rowsList[i].ChildNodes.Count; j++)
                            {
                                if (!string.IsNullOrEmpty(rowsList[i].ChildNodes[j].InnerXml))
                                {
                                    string innerText = rowsList[i].ChildNodes[j].InnerText;
                                    string innerXml = rowsList[i].ChildNodes[j].InnerXml;
                                    //string html5Text = html5ViewerTrList[i].ChildNodes[j].InnerText.Trim();

                                    rowsList[i].ChildNodes[j].InnerXml = innerXml.Replace(innerText, html5ViewerTrList[i].ChildNodes[j].InnerText.Trim());
                                }
                            }
                        }
                    }
                    //end

                    //Add new rows and assign coordinates
                    var newRowsList = doc.DocumentNode.SelectNodes("//tr").Skip(rowsList.Count).ToList();

                    if (newRowsList.Count > 0)
                    {
                        for (int i = 0; i < newRowsList.Count; i++)
                        {
                            XmlNode lastTableRow = finalTable.SelectNodes("//Row").Count > 0 ? finalTable.SelectNodes("//Row").Cast<XmlNode>().Last() : null;

                            if (lastTableRow != null && lastTableRow.ChildNodes != null && lastTableRow.ChildNodes.Count > 0)
                            {
                                if (newRowsList[i].ChildNodes.Count == lastTableRow.ChildNodes.Count)
                                {
                                    XmlNode newRow = xmlDoc.CreateElement("Row");

                                    for (int j = 0; j < newRowsList[i].ChildNodes.Count; j++)
                                    {
                                        XmlNode column = xmlDoc.CreateElement("col");

                                        TetmlLine lastLineCol = GetColLineCoord(lastTableRow.ChildNodes[j]);

                                        if (rowsList[rowsList.Count - 1].ChildNodes.Count > 0)
                                        {
                                            llx = Convert.ToDouble(lastLineCol.Llx);
                                            lly = Convert.ToDouble(lastLineCol.Lly) + verticalLineDiff;
                                            urx = Convert.ToDouble(lastLineCol.Urx);
                                            ury = Convert.ToDouble(lastLineCol.Ury) + verticalLineDiff;
                                        }

                                        coord = llx + ":" + lly + ":" + urx + ":" + ury;
                                        string newLn = "<ln coord=\"" + coord + "\" " + "page=\"" + page + "\" height=\"" +
                                                       line.Height + "\" left=\"" + line.Left +
                                                       "\" top=\"" + line.Top + "\" font=\"" + line.Font + "\" fontsize=\"" +
                                                       line.FontSize + "\" fonttype=\"" +
                                                       line.FontType + "\" >";

                                        column.InnerText = newLn + newRowsList[i].ChildNodes[j].InnerText.Trim() + "</ln>";
                                        newRow.AppendChild(column);
                                    }

                                    XmlNode lastRow = finalTable.SelectNodes("//Row").Count > 0 ? finalTable.SelectNodes("//Row").Cast<XmlNode>().Last() : null;

                                    if (lastRow != null)
                                        lastRow.ParentNode.InsertAfter(newRow, lastRow);
                                }
                            }
                        }
                    }//end new rows addition
                }

                #endregion

                return finalTable;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public TetmlLine GetLineParameters(XmlNode rowsList)
        {
            if (rowsList.ChildNodes[0].ChildNodes != null)
            {
                if (rowsList.ChildNodes[0].ChildNodes.Count > 0)
                {
                    TetmlLine ln = new TetmlLine();

                    ln.Height = rowsList.ChildNodes[0].ChildNodes[0].Attributes["height"] == null ? 0 :
                                                        Convert.ToDouble(rowsList.ChildNodes[0].ChildNodes[0].Attributes["height"].Value);

                    ln.Left = rowsList.ChildNodes[0].ChildNodes[0].Attributes["left"] == null ? 0 :
                                                        Convert.ToDouble(rowsList.ChildNodes[0].ChildNodes[0].Attributes["left"].Value);

                    ln.Top = rowsList.ChildNodes[0].ChildNodes[0].Attributes["top"] == null ? 0 :
                                                        Convert.ToDouble(rowsList.ChildNodes[0].ChildNodes[0].Attributes["top"].Value);

                    ln.Font = rowsList.ChildNodes[0].ChildNodes[0].Attributes["font"] == null ? "" :
                                                       Convert.ToString(rowsList.ChildNodes[0].ChildNodes[0].Attributes["font"].Value);

                    ln.FontSize = rowsList.ChildNodes[0].ChildNodes[0].Attributes["fontsize"] == null ? "" :
                                                        Convert.ToString(rowsList.ChildNodes[0].ChildNodes[0].Attributes["fontsize"].Value);

                    ln.FontType = rowsList.ChildNodes[0].ChildNodes[0].Attributes["fonttype"] == null ? "" :
                                                        Convert.ToString(rowsList.ChildNodes[0].ChildNodes[0].Attributes["fonttype"].Value);

                    return ln;
                }
            }

            return null;
        }

        public double GetHorizontalDiff(List<XmlNode> rowsList)
        {
            int maxColumns = rowsList.Cast<XmlNode>().Where(x => x.ChildNodes != null).Select(x => Convert.ToInt32(x.ChildNodes.Count)).Max();

            if (maxColumns > 1)
            {
                var maxColRowList = rowsList.Cast<XmlNode>().Where(x => x.ChildNodes.Count == maxColumns).ToList();

                if (maxColRowList.Count > 0)
                {
                    if (maxColRowList[0].ChildNodes[1].ChildNodes != null && maxColRowList[0].ChildNodes[0].ChildNodes != null)
                    {
                        if (maxColRowList[0].ChildNodes[1].ChildNodes.Count > 0 && maxColRowList[0].ChildNodes[0].ChildNodes.Count > 0)
                        {
                            double nextColumnLlx = maxColRowList[0].ChildNodes[1].ChildNodes[0].Attributes["left"] == null ? 0 :
                                                           Convert.ToDouble(maxColRowList[0].ChildNodes[1].ChildNodes[0].Attributes["left"].Value);
                            double firstColumnLlx = maxColRowList[0].ChildNodes[0].ChildNodes[0].Attributes["left"] == null ? 0 :
                                                                  Convert.ToDouble(maxColRowList[0].ChildNodes[0].ChildNodes[0].Attributes["left"].Value);

                            return Math.Abs(nextColumnLlx - firstColumnLlx);
                        }
                    }
                }
            }

            return 0;
        }

        public double GetVerticalDiff(List<XmlNode> rowsList)
        {
            int maxColumns = rowsList.Cast<XmlNode>().Where(x => x.ChildNodes != null).Select(x => Convert.ToInt32(x.ChildNodes.Count)).Max();

            if (maxColumns > 1)
            {
                var maxColRowList = rowsList.Cast<XmlNode>().Where(x => x.ChildNodes.Count == maxColumns && !x.Name.Equals("head-row")).ToList();

                if (maxColRowList.Count > 0)
                {
                    if (maxColRowList[0].ChildNodes[1].ChildNodes != null && maxColRowList[0].ChildNodes[0].ChildNodes != null)
                    {
                        if (maxColRowList[0].ChildNodes[1].ChildNodes.Count > 0 && maxColRowList[0].ChildNodes[0].ChildNodes.Count > 0)
                        {
                            double nextColumnLlx = maxColRowList[0].ChildNodes[1].ChildNodes[0].Attributes["left"] == null ? 0 :
                                                           Convert.ToDouble(maxColRowList[0].ChildNodes[1].ChildNodes[0].Attributes["left"].Value);
                            double firstColumnLlx = maxColRowList[0].ChildNodes[0].ChildNodes[0].Attributes["left"] == null ? 0 :
                                                                  Convert.ToDouble(maxColRowList[0].ChildNodes[0].ChildNodes[0].Attributes["left"].Value);

                            return Math.Abs(nextColumnLlx - firstColumnLlx);
                        }
                    }
                }
            }

            return 0;
        }

        public TetmlLine GetColLineCoord(XmlNode column)
        {
            if (column != null)
            {
                if (column.ChildNodes != null)
                {
                    if (column.ChildNodes.Count > 0)
                    {
                        TetmlLine ln = new TetmlLine();

                        if (column.ChildNodes[0].Attributes["coord"] != null)
                        {
                            if (column.ChildNodes[0].Attributes["coord"].Value.Split(':').Length > 3)
                            {
                                ln.Llx = column.ChildNodes[0].Attributes["coord"].Value.Split(':')[0];
                                ln.Lly = column.ChildNodes[0].Attributes["coord"].Value.Split(':')[1];
                                ln.Urx = column.ChildNodes[0].Attributes["coord"].Value.Split(':')[2];
                                ln.Ury = column.ChildNodes[0].Attributes["coord"].Value.Split(':')[3];
                            }
                            else
                            {
                                ln.Llx = "0";
                                ln.Lly = "0";
                                ln.Urx = "0";
                                ln.Ury = "0";
                            }
                        }
                        ln.Height = column.ChildNodes[0].Attributes["height"] == null ? 0 : Convert.ToDouble(column.ChildNodes[0].Attributes["height"].Value);
                        ln.Left = column.ChildNodes[0].Attributes["left"] == null ? 0 : Convert.ToDouble(column.ChildNodes[0].Attributes["left"].Value);
                        ln.Top = column.ChildNodes[0].Attributes["top"] == null ? 0 : Convert.ToDouble(column.ChildNodes[0].Attributes["top"].Value);
                        ln.Font = column.ChildNodes[0].Attributes["font"] == null ? "" : Convert.ToString(column.ChildNodes[0].Attributes["font"].Value);
                        ln.FontSize = column.ChildNodes[0].Attributes["fontsize"] == null ? "" : Convert.ToString(column.ChildNodes[0].Attributes["fontsize"].Value);
                        ln.FontType = column.ChildNodes[0].Attributes["fonttype"] == null ? "" : Convert.ToString(column.ChildNodes[0].Attributes["fonttype"].Value);
                        return ln;
                    }
                }
            }

            return null;
        }

        //public TetmlLine GetColLineCoord(XmlNode table)
        //{
        //    XmlNode lastRow = table.SelectNodes("//Row").Count > 0 ? table.SelectNodes("//Row").Cast<XmlNode>().Last() : null;

        //    if (lastRow != null)
        //    {
        //        if (lastRow.ChildNodes != null)
        //        {
        //            if (lastRow.ChildNodes.Count > 0)
        //            {
        //                if (lastRow.ChildNodes[0].ChildNodes != null)
        //                {
        //                    if (lastRow.ChildNodes[0].ChildNodes.Count > 0)
        //                    {
        //                        TetmlLine ln = new TetmlLine();

        //                        if (lastRow.ChildNodes[0].ChildNodes[0].Attributes["coord"] != null)
        //                        {
        //                            if (lastRow.ChildNodes[0].ChildNodes[0].Attributes["coord"].Value.Split(':').Length > 3)
        //                            {
        //                                ln.Llx = lastRow.ChildNodes[0].ChildNodes[0].Attributes["coord"].Value.Split(':')[0];
        //                                ln.Lly = lastRow.ChildNodes[0].ChildNodes[0].Attributes["coord"].Value.Split(':')[1];
        //                                ln.Urx = lastRow.ChildNodes[0].ChildNodes[0].Attributes["coord"].Value.Split(':')[2];
        //                                ln.Ury = lastRow.ChildNodes[0].ChildNodes[0].Attributes["coord"].Value.Split(':')[3];
        //                            }
        //                            else
        //                            {
        //                                ln.Llx = "0";
        //                                ln.Lly = "0";
        //                                ln.Urx = "0";
        //                                ln.Ury = "0";
        //                            }
        //                        }

        //                        ln.Height = lastRow.ChildNodes[0].ChildNodes[0].Attributes["height"] == null ? 0 :
        //                                    Convert.ToDouble(lastRow.ChildNodes[0].ChildNodes[0].Attributes["height"].Value);

        //                        ln.Left = lastRow.ChildNodes[0].ChildNodes[0].Attributes["left"] == null ? 0 :
        //                                  Convert.ToDouble(lastRow.ChildNodes[0].ChildNodes[0].Attributes["left"].Value);

        //                        ln.Top = lastRow.ChildNodes[0].ChildNodes[0].Attributes["top"] == null ? 0 :
        //                                 Convert.ToDouble(lastRow.ChildNodes[0].ChildNodes[0].Attributes["top"].Value);

        //                        ln.Font = lastRow.ChildNodes[0].ChildNodes[0].Attributes["font"] == null ? "" :
        //                                  Convert.ToString(lastRow.ChildNodes[0].ChildNodes[0].Attributes["font"].Value);

        //                        ln.FontSize = lastRow.ChildNodes[0].ChildNodes[0].Attributes["fontsize"] == null ? "" :
        //                                      Convert.ToString(lastRow.ChildNodes[0].ChildNodes[0].Attributes["fontsize"].Value);

        //                        ln.FontType = lastRow.ChildNodes[0].ChildNodes[0].Attributes["fonttype"] == null ? "" :
        //                                      Convert.ToString(lastRow.ChildNodes[0].ChildNodes[0].Attributes["fonttype"].Value);
        //                        return ln;
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    return null;
        //}

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

        public XmlNode InsertEmptyColumns(XmlNode tableXml)
        {
            if (tableXml == null) return null;

            int maxColumns = tableXml.SelectNodes("//head-row | //Row").Cast<XmlNode>().Select(y => y.ChildNodes.Count).Max();
            var maxColumnsRow = tableXml.SelectNodes("//head-row | //Row").Cast<XmlNode>().Where(x => x.ChildNodes.Count == maxColumns).ToList();

            List<TableColumns> colCoordList = new List<TableColumns>();
            List<TableColumns> allColsHorizontalCoord = new List<TableColumns>();

            int columnNum = 0;

            if (maxColumnsRow != null)
            {
                foreach (XmlNode row in maxColumnsRow)
                {
                    columnNum = 0;

                    foreach (XmlNode cell in row.ChildNodes)
                    {
                        columnNum++;

                        var coordinates = cell.SelectNodes("descendant::Box");

                        if (coordinates != null && coordinates.Count > 0)
                        {
                            foreach (XmlNode coords in coordinates)
                            {
                                colCoordList.Add(new TableColumns
                                {
                                    ColumnNum = columnNum,
                                    Llx = coords.Attributes["llx"] != null ? Convert.ToDouble(coords.Attributes["llx"].Value) : 0,
                                    Urx = coords.Attributes["urx"] != null ? Convert.ToDouble(coords.Attributes["urx"].Value) : 0
                                });
                            }
                        }
                    }
                }
            }

            if (colCoordList != null)
            {
                for (int i = 1; i <= maxColumns; i++)
                {
                    var llx = colCoordList.Where(x => x.ColumnNum == i).GroupBy(x => Convert.ToDouble(x.Llx))
                                          .Select(group => new { value = group.Key, Count = group.Count() })
                                          .OrderBy(y => Convert.ToDouble(y.value)).FirstOrDefault();

                    var urx = colCoordList.Where(x => x.ColumnNum == i).GroupBy(x => Convert.ToDouble(x.Urx))
                                          .Select(group => new { value = group.Key, Count = group.Count() })
                                          .OrderByDescending(y => Convert.ToDouble(y.value)).FirstOrDefault();

                    allColsHorizontalCoord.Add(new TableColumns
                    {
                        ColumnNum = i,
                        Llx = llx.value,
                        Urx = urx.value
                    });
                }
            }

            XmlDocument tblDocument = new XmlDocument();
            tblDocument.LoadXml(tableXml.OuterXml);

            //var tblRows = tblDocument.SelectNodes("//head-row | //Row").Cast<XmlNode>().Where(x => x.ChildNodes.Count >= 1 &&
            //                                                                                  x.ChildNodes.Count < maxColumns).ToList();

            int totalRows = tblDocument.SelectNodes("//head-row | //Row").Cast<XmlNode>().Count();

            var captionRows = tblDocument.SelectNodes("//Row").Cast<XmlNode>().Reverse().TakeWhile(x => x.ChildNodes.Count == 1).ToList();

            var tblHeaderRows = tblDocument.SelectNodes("//head-row | //Row").Cast<XmlNode>().TakeWhile(x => x.ChildNodes.Count == 1).ToList();

            //var tblBodyRowsTest = tblDocument.SelectNodes("//head-row | //Row").Cast<XmlNode>().Skip(tblHeaderRows.Count).Reverse()
            //                                 .SkipWhile(x => x.ChildNodes!=null && x.ChildNodes.Count == 1 && Convert.ToDouble(x.ChildNodes[0].Attributes["llx"].Value) <= Convert.ToDouble(allColsHorizontalCoord[0].Llx)).ToList();

            var tblBodyRows = tblDocument.SelectNodes("//head-row | //Row").Cast<XmlNode>().Skip(tblHeaderRows.Count).Reverse().Skip(captionRows.Count).ToList();

            var tblRows = tblBodyRows.Where(x => x.ChildNodes.Count >= 1 && x.ChildNodes.Count < maxColumns).ToList();

            XmlNode Row = null;
            XmlNode CellNode = null;
            XmlNode ParaNode = null;

            int colPos = 0;

            foreach (XmlNode row in tblRows)
            {
                colPos = 0;

                Row = tblDocument.CreateElement("Row");

                var cellList = row.SelectNodes("descendant::Cell").Cast<XmlNode>().ToList();

                List<int> actualColPos = GetColActualPos(cellList, allColsHorizontalCoord);

                if (actualColPos != null)
                {
                    List<int> missingColPos = Enumerable.Range(1, maxColumns).Except(actualColPos).ToList();

                    Dictionary<int, string> dicAllColumns = new Dictionary<int, string>();

                    foreach (int column in actualColPos)
                    {
                        dicAllColumns.Add(column, "P");
                    }

                    foreach (int column in missingColPos)
                    {
                        dicAllColumns.Add(column, "A");
                    }

                    var sortedColumns = dicAllColumns.OrderBy(x => x.Key);

                    foreach (var dicRow in sortedColumns)
                    {
                        if (dicRow.Value.EndsWith("A"))
                        {
                            CellNode = tblDocument.CreateElement("Cell");
                            ParaNode = tblDocument.CreateElement("Para");
                            CellNode.AppendChild(ParaNode);
                            Row.AppendChild(CellNode);
                        }

                        else
                        {
                            CellNode = tblDocument.CreateElement("Cell");
                            CellNode.InnerXml = cellList[colPos].InnerXml;
                            Row.AppendChild(CellNode);
                            colPos++;
                        }
                    }

                    row.InnerXml = Row.InnerXml;
                }
            }

            return tblDocument.SelectSingleNode("//Table");
        }

        public XmlNode InsertEmptyColumnsInFinalXml(XmlNode tableXml)
        {
            if (tableXml == null) return null;

            int maxColumns = tableXml.SelectNodes("//head-row | //Row").Cast<XmlNode>().Select(y => y.ChildNodes.Count).Max();
            var maxColumnsRow = tableXml.SelectNodes("//head-row | //Row").Cast<XmlNode>().Where(x => x.ChildNodes.Count == maxColumns).ToList();

            List<TableColumns> colCoordList = new List<TableColumns>();
            List<TableColumns> allColsHorizontalCoord = new List<TableColumns>();

            int columnNum = 0;

            if (maxColumnsRow != null)
            {
                foreach (XmlNode row in maxColumnsRow)
                {
                    columnNum = 0;

                    foreach (XmlNode cell in row.ChildNodes)
                    {
                        columnNum++;

                        var coordinates = cell.SelectNodes("descendant::Box");

                        if (coordinates != null && coordinates.Count > 0)
                        {
                            foreach (XmlNode coords in coordinates)
                            {
                                colCoordList.Add(new TableColumns
                                {
                                    ColumnNum = columnNum,
                                    Llx = coords.Attributes["llx"] != null ? Convert.ToDouble(coords.Attributes["llx"].Value) : 0,
                                    Urx = coords.Attributes["urx"] != null ? Convert.ToDouble(coords.Attributes["urx"].Value) : 0
                                });
                            }
                        }
                    }
                }
            }

            if (colCoordList != null)
            {
                for (int i = 1; i <= maxColumns; i++)
                {
                    var llx = colCoordList.Where(x => x.ColumnNum == i).GroupBy(x => Convert.ToDouble(x.Llx))
                                          .Select(group => new { value = group.Key, Count = group.Count() })
                                          .OrderBy(y => Convert.ToDouble(y.value)).FirstOrDefault();

                    var urx = colCoordList.Where(x => x.ColumnNum == i).GroupBy(x => Convert.ToDouble(x.Urx))
                                          .Select(group => new { value = group.Key, Count = group.Count() })
                                          .OrderByDescending(y => Convert.ToDouble(y.value)).FirstOrDefault();

                    allColsHorizontalCoord.Add(new TableColumns
                    {
                        ColumnNum = i,
                        Llx = llx.value,
                        Urx = urx.value
                    });
                }
            }

            XmlDocument tblDocument = new XmlDocument();
            tblDocument.LoadXml(tableXml.OuterXml);

            //var tblRows = tblDocument.SelectNodes("//head-row | //Row").Cast<XmlNode>().Where(x => x.ChildNodes.Count >= 1 &&
            //                                                                                  x.ChildNodes.Count < maxColumns).ToList();

            int totalRows = tblDocument.SelectNodes("//head-row | //Row").Cast<XmlNode>().Count();

            var captionRows = tblDocument.SelectNodes("//Row").Cast<XmlNode>().Reverse().TakeWhile(x => x.ChildNodes.Count == 1).ToList();

            var tblHeaderRows = tblDocument.SelectNodes("//head-row | //Row").Cast<XmlNode>().TakeWhile(x => x.ChildNodes.Count == 1).ToList();

            //var tblBodyRowsTest = tblDocument.SelectNodes("//head-row | //Row").Cast<XmlNode>().Skip(tblHeaderRows.Count).Reverse()
            //                                 .SkipWhile(x => x.ChildNodes!=null && x.ChildNodes.Count == 1 && Convert.ToDouble(x.ChildNodes[0].Attributes["llx"].Value) <= Convert.ToDouble(allColsHorizontalCoord[0].Llx)).ToList();

            var tblBodyRows = tblDocument.SelectNodes("//head-row | //Row").Cast<XmlNode>().Skip(tblHeaderRows.Count).Reverse().Skip(captionRows.Count).ToList();

            var tblRows = tblBodyRows.Where(x => x.ChildNodes.Count >= 1 && x.ChildNodes.Count < maxColumns).ToList();

            XmlNode Row = null;
            XmlNode CellNode = null;
            XmlNode ParaNode = null;

            int colPos = 0;

            foreach (XmlNode row in tblRows)
            {
                colPos = 0;

                Row = tblDocument.CreateElement("Row");

                var cellList = row.SelectNodes("descendant::Cell").Cast<XmlNode>().ToList();

                List<int> actualColPos = GetColActualPos(cellList, allColsHorizontalCoord);

                if (actualColPos != null)
                {
                    List<int> missingColPos = Enumerable.Range(1, maxColumns).Except(actualColPos).ToList();

                    Dictionary<int, string> dicAllColumns = new Dictionary<int, string>();

                    foreach (int column in actualColPos)
                    {
                        dicAllColumns.Add(column, "P");
                    }

                    foreach (int column in missingColPos)
                    {
                        dicAllColumns.Add(column, "A");
                    }

                    var sortedColumns = dicAllColumns.OrderBy(x => x.Key);

                    foreach (var dicRow in sortedColumns)
                    {
                        if (dicRow.Value.EndsWith("A"))
                        {
                            CellNode = tblDocument.CreateElement("Cell");
                            ParaNode = tblDocument.CreateElement("Para");
                            CellNode.AppendChild(ParaNode);
                            Row.AppendChild(CellNode);
                        }

                        else
                        {
                            CellNode = tblDocument.CreateElement("Cell");
                            CellNode.InnerXml = cellList[colPos].InnerXml;
                            Row.AppendChild(CellNode);
                            colPos++;
                        }
                    }

                    row.InnerXml = Row.InnerXml;
                }
            }

            return tblDocument.SelectSingleNode("//Table");
        }

        public List<int> GetColActualPos(List<XmlNode> cellNodes, List<TableColumns> allColsHorizontalCoord)
        {
            if ((cellNodes == null || cellNodes.Count == 0) || (allColsHorizontalCoord == null || allColsHorizontalCoord.Count == 0)) return null;

            List<int> actualColPosList = new List<int>();

            double llx = 0;
            double urx = 0;

            for (int i = 0; i < cellNodes.Count; i++)
            {
                var coordList = cellNodes[i].SelectNodes("descendant::Box").Cast<XmlNode>().ToList();

                if (coordList.Count > 0)
                {
                    llx = coordList[0].Attributes["llx"] != null ? Convert.ToDouble(coordList[0].Attributes["llx"].Value) : 0;
                    urx = coordList[0].Attributes["urx"] != null ? Convert.ToDouble(coordList[0].Attributes["urx"].Value) : 0;
                }

                if (llx > 0 && urx > 0)
                {
                    //var matchedColList = allColsHorizontalCoord.Where(x => (xmlLlx >= x.Llx && xmlLlx <= x.Urx)).ToList();
                    //var matchedColList = allColsHorizontalCoord.Where(x => IsColMatchedByCoord(llx, urx, x)).ToList();

                    //if (matchedColList.Count == 1)
                    //    actualColPosList.Add(matchedColList[0].ColumnNum);

                    //else if (matchedColList.Count > 1)
                    //    actualColPosList.Add(matchedColList.Min(x=>x.ColumnNum));

                    actualColPosList.Add(GetMatchedColLoc(llx, urx, allColsHorizontalCoord));
                }
            }

            if (actualColPosList.Count > 0) return actualColPosList.Distinct().ToList();

            //if (actualColPosList.Count > 0) return actualColPosList;

            return null;
        }

        public int GetMatchedColLoc(double llx, double urx, List<TableColumns> allColsCoords)
        {
            if (allColsCoords == null || allColsCoords.Count == 0) return 0;

            for (int i = 0; i < allColsCoords.Count; i++)
            {
                if (Math.Floor(allColsCoords[i].Llx) >= Math.Floor(llx) && Math.Floor(allColsCoords[i].Urx) >= Math.Floor(urx))
                    return allColsCoords[i].ColumnNum;

                if (Math.Floor(allColsCoords[i].Llx) >= Math.Floor(llx) && Math.Floor(allColsCoords[i].Urx) <= Math.Floor(urx))
                    return allColsCoords[i].ColumnNum;

                if (Math.Floor(allColsCoords[i].Llx) >= Math.Floor(llx) && Math.Floor(allColsCoords[i].Urx) >= Math.Floor(urx))
                    return allColsCoords[i].ColumnNum;
            }

            return 0;
        }

        public List<XmlNode> GetLineWithCoordFromTblXml(List<string> pdfJsLinesList, XmlDocument tableDoc)
        {
            List<XmlNode> matchedLinesList = null;

            if (pdfJsLinesList != null && pdfJsLinesList.Count > 0)
            {
                matchedLinesList = new List<XmlNode>();

                var tableAllLines = tableDoc.SelectNodes("//ln").Cast<XmlNode>().Where(x => isHeaderStartingLine(x.InnerText.Trim(), pdfJsLinesList)).ToList();

                if (tableAllLines.Count > 0)
                {
                    matchedLinesList.Add(tableAllLines[0]);
                }

                if (matchedLinesList.Count > 0)
                {
                    return matchedLinesList;
                }
            }

            return null;
        }

        #endregion

        //public bool GetMatchedColLoc(double llx, double urx, TableColumns allCols)
        //{
        //    if ((Math.Floor(allCols.Llx) >= Math.Floor(llx) && Math.Floor(allCols.Urx) <= Math.Floor(urx) ||
        //        (Math.Floor(allCols.Llx) >= Math.Floor(llx) && Math.Floor(allCols.Urx) >= Math.Floor(urx)))) return true;
        //        //(Math.Floor(allCols.Llx) <= Math.Floor(llx) && Math.Floor(allCols.Urx) >= Math.Floor(urx))


        //    return false;
        //}

        private bool isMatchedLine(string xmlLineInnerText, string pdfJsFirstLine)
        {
            if (string.IsNullOrEmpty(xmlLineInnerText) || string.IsNullOrEmpty(pdfJsFirstLine)) return false;

            List<string> pdfJsLineTempList = Regex.Split(pdfJsFirstLine, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
            List<string> xmlLineTempList = Regex.Split(xmlLineInnerText, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

            if (pdfJsLineTempList.Count > 0 && xmlLineTempList.Count > 0)
            {
                int matchingPer = GetMatchingPercentage(pdfJsLineTempList[0].Trim(), xmlLineTempList[0].Trim());
                if (matchingPer < 50)
                    return false;
            }

            if (pdfJsFirstLine.Trim().Equals(xmlLineInnerText.Trim()))
                return true;

            string pdfJsText = RemoveWhiteSpace(RemoveSpecialChars(pdfJsFirstLine));
            string xmlText = RemoveWhiteSpace(RemoveSpecialChars(xmlLineInnerText));

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
    }
}
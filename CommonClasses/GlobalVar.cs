using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Collections.Generic;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Collections;
using System.Text;
namespace Outsourcing_System
{
    public class GlobalVar
    {
        public GlobalVar()
        {

        }
        public GlobalVar(XmlDocument objXMLDoc)
        {
            this.PBPDocument = objXMLDoc;
        }


        //Khalil code adde here on 31-05-2013

        private string xMLPath;
        //public static string XMLPath;
        private string pDFPath;
        private string rhywPath;

        public static string ProjectFolderPath;
        private System.Xml.XmlDocument pBPDocument;
        private int firstXmlPageNo = 0;
        ImageIndex objImageIndex = new ImageIndex();

        public string XMLPath
        {
            get { return xMLPath; }
            set { xMLPath = value; }
        }

        public string RhywPath
        {
            get { return rhywPath; }
            set { rhywPath = value; }
        }

        public string PDFPath
        {
            get { return pDFPath; }
            set { pDFPath = value; }
        }

        public System.Xml.XmlDocument PBPDocument
        {
            get { return pBPDocument; }
            set { pBPDocument = value; }
        }

        static System.Xml.XmlDocument PBPDocument1
        {
            get
            {
                return SiteSession.xmlDoc;
            }
        }
        /// <summary>
        /// Gets the maximum page number in the xml document
        /// </summary>
        /// <returns></returns>
        public static int GetMaxPageNo()
        {//aamir
            //XPathNavigator navigator = GlobalVar.PBPDocument.CreateNavigator();

            //string res = (string)navigator.Evaluate("max(//ln/@page)");


            //XmlNode lnNode = GlobalVar.PBPDocument.SelectSingleNode("//ln[@page=max(//ln/@page)]");
            //if (lnNode != null)
            //{
            //    try
            //    {
            //        return int.Parse(lnNode.Attributes["page"].Value);
            //    }
            //    catch
            //    {
            //        return -1;
            //    }
            //}
            //else
            return -1;
        }

        //khalil update end here......


        public int GetPageCountFromTetml(string tetmlPath)
        {
            StreamReader sr = new StreamReader(tetmlPath);
            string tetmlContent = sr.ReadToEnd();
            sr.Dispose();
            Match mtch = Regex.Match(tetmlContent, "(?<=<Document.*?pageCount=\").*?(?=\")");
            string pc = mtch.Value;

            //XmlDocument xmlDOc = new XmlDocument();
            //xmlDOc.Load(tetmlPath);
            //string pc = xmlDOc.SelectSingleNode("//Document[@pageCount]").Value;

            return int.Parse(pc);
        }

        //To make a complete page xml :: GlobalVar
        public XmlDocument PreProcess(XmlDocument xmlDoc)
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

        //To make a complete page xml :: GlobalVar
        static XmlDocument PreProcess1(XmlDocument xmlDoc)
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

        //=============================================================================
        //Getting Content of the Page
        //=============================================================================    
        public DataSet ConvertXMLToDataSet(string xmlData)
        {
            StringReader stream = null;
            XmlTextReader reader = null;
            try
            {
                DataSet xmlDS = new DataSet();
                stream = new StringReader(xmlData);
                // Load the XmlTextReader from the stream
                reader = new XmlTextReader(stream);
                xmlDS.ReadXml(reader);
                return xmlDS;
            }
            catch
            {
                return null;
            }
            finally
            {
                if (reader != null) reader.Close();
            }
        }

        //old method
        //public XmlDocument GetPageXmlDoc(string num)
        //{
        //    //DataSet ds = ConvertXMLToDataSet(this.pBPDocument.InnerXml);
        //    //firstXmlPageNo = int.Parse(GlobalVar.PBPDocument.SelectSingleNode("//ln").Attributes["page"].Value.ToString());

        //    XmlNodeList pageContents = this.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/..");
        //    List<XmlNode> FinalNodes = new List<XmlNode>();
        //    for (int i = 0; i < pageContents.Count; i++)
        //    {
        //        if (pageContents[i].ParentNode.Name.Equals("box"))
        //        {
        //            FinalNodes.Add(pageContents[i].ParentNode);
        //            while (pageContents[i].NextSibling != null)
        //            {
        //                i++;
        //            }
        //        }
        //        else
        //        {
        //            FinalNodes.Add(pageContents[i]);
        //        }
        //    }
        //    //XmlNodeList pageContents = GlobalVar.PBPDocument.SelectNodes("//*[@page=\"" + (int.Parse(num) - 1 + firstXmlPageNo) + "\"]/..");
        //    XmlDocument tmpPageXml = new XmlDocument();
        //    if (FinalNodes.Count == 0)
        //    {
        //        return PreProcess(null);
        //    }
        //    else
        //    {
        //        XmlNode rootElement = tmpPageXml.CreateElement("body");
        //        tmpPageXml.AppendChild(rootElement);
        //        foreach (XmlNode xmlNode in FinalNodes)
        //        {
        //            XmlNode tempNode = xmlNode.ParentNode.Name.Equals("box") ? xmlNode.ParentNode : xmlNode;
        //            XmlNode validNode = this.PBPDocument.CreateElement(tempNode.Name);
        //            foreach (XmlAttribute att in tempNode.Attributes)
        //            {
        //                ((XmlElement)validNode).SetAttribute(att.Name, att.Value);
        //            }
        //            //if (xmlNode.Attributes["image-url"] != null)
        //            //{
        //            //    ((XmlElement)validNode).SetAttribute("image-url", xmlNode.Attributes["image-url"].Value);
        //            //}
        //            XmlNodeList contentChilds = tempNode.ChildNodes;
        //            foreach (XmlNode content in contentChilds)
        //            {
        //                //if (content.Attributes["page"] != null && content.Attributes["page"].Value == (int.Parse(num) - 1 + firstXmlPageNo).ToString())
        //                if (content.Name.Equals("upara") || (content.Name.Equals("box-title")))
        //                {
        //                    validNode.InnerXml += content.OuterXml;
        //                }
        //                else if (content.Attributes["page"] != null && content.Attributes["page"].Value == num)
        //                {
        //                    validNode.InnerXml += content.OuterXml;
        //                }
        //            }
        //            rootElement.InnerXml += validNode.OuterXml;
        //        }
        //        return PreProcess(tmpPageXml);
        //    }
        //}

        //=============================================================================
        //Getting Content of the Page
        //=============================================================================        


        //To Do
        public XmlDocument GetPageXmlDoc(string num)
        {
            XmlNodeList pageContents = this.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/..");
            List<XmlNode> FinalNodes = new List<XmlNode>();
            for (int i = 0; i < pageContents.Count; i++)
            {
                if (!pageContents[i].Name.Equals("body"))
                {
                    if (pageContents[i].Name.Equals("head-col") || pageContents[i].Name.Equals("col"))
                    {
                        if (pageContents[i].ParentNode.ParentNode.Name.Equals("Table"))
                        {
                            FinalNodes.Add(pageContents[i].ParentNode.ParentNode);
                            while (pageContents[i].Name.Equals("head-col") || pageContents[i].Name.Equals("col"))
                            {
                                if (i < pageContents.Count - 1)
                                    i++;

                                if (i == pageContents.Count - 1)
                                    break;
                            }
                            break;
                        }
                    }
                    if (pageContents[i].ParentNode.Name.Equals("box"))
                    {
                        FinalNodes.Add(pageContents[i].ParentNode);
                        while (pageContents[i].NextSibling != null)
                        {
                            i++;
                        }
                    }
                    else
                    {
                        FinalNodes.Add(pageContents[i]);
                    }
                }
            }
            //XmlNodeList pageContents = GlobalVar.PBPDocument.SelectNodes("//*[@page=\"" + (int.Parse(num) - 1 + firstXmlPageNo) + "\"]/..");
            XmlDocument tmpPageXml = new XmlDocument();
            if (FinalNodes.Count == 0)
            {
                return PreProcess(null);
            }
            else
            {
                XmlNode rootElement = tmpPageXml.CreateElement("body");
                tmpPageXml.AppendChild(rootElement);
                foreach (XmlNode xmlNode in FinalNodes)
                {
                    XmlNode tempNode = xmlNode.ParentNode.Name.Equals("box") ? xmlNode.ParentNode : xmlNode;
                    XmlNode validNode = this.PBPDocument.CreateElement(tempNode.Name);
                    foreach (XmlAttribute att in tempNode.Attributes)
                    {
                        ((XmlElement)validNode).SetAttribute(att.Name, att.Value);
                    }
                    //if (xmlNode.Attributes["image-url"] != null)
                    //{
                    //    ((XmlElement)validNode).SetAttribute("image-url", xmlNode.Attributes["image-url"].Value);
                    //}
                    XmlNodeList contentChilds = tempNode.ChildNodes;
                    foreach (XmlNode content in contentChilds)
                    {
                        //if (content.Attributes["page"] != null && content.Attributes["page"].Value == (int.Parse(num) - 1 + firstXmlPageNo).ToString())
                        if (content.Name.Equals("upara") || (content.Name.Equals("box-title")))
                        {
                            validNode.InnerXml += content.OuterXml;
                        }
                        else if (content.Attributes["page"] != null && content.Attributes["page"].Value == num)
                        {
                            validNode.InnerXml += content.OuterXml;
                        }
                    }
                    rootElement.InnerXml += validNode.OuterXml;
                }
                return PreProcess(tmpPageXml);
            }
        }

        public static XmlDocument GetPageXmlDoc1(string num)
        {
            //firstXmlPageNo = int.Parse(GlobalVar.PBPDocument.SelectSingleNode("//ln").Attributes["page"].Value.ToString());
            XmlNodeList pageContents = GlobalVar.PBPDocument1.SelectNodes("//*[@page=\"" + num + "\"]/..");
            //XmlNodeList pageContents = GlobalVar.PBPDocument.SelectNodes("//*[@page=\"" + (int.Parse(num) - 1 + firstXmlPageNo) + "\"]/..");
            XmlDocument tmpPageXml = new XmlDocument();
            if (pageContents.Count == 0)
            {
                return PreProcess1(null);
            }
            else
            {
                XmlNode rootElement = tmpPageXml.CreateElement("body");
                tmpPageXml.AppendChild(rootElement);
                foreach (XmlNode xmlNode in pageContents)
                {
                    XmlNode validNode = GlobalVar.PBPDocument1.CreateElement(xmlNode.Name);
                    foreach (XmlAttribute att in xmlNode.Attributes)
                    {
                        ((XmlElement)validNode).SetAttribute(att.Name, att.Value);
                    }
                    //if (xmlNode.Attributes["image-url"] != null)
                    //{
                    //    ((XmlElement)validNode).SetAttribute("image-url", xmlNode.Attributes["image-url"].Value);
                    //}
                    XmlNodeList contentChilds = xmlNode.ChildNodes;
                    foreach (XmlNode content in contentChilds)
                    {
                        //if (content.Attributes["page"] != null && content.Attributes["page"].Value == (int.Parse(num) - 1 + firstXmlPageNo).ToString())
                        if (content.Attributes["page"] != null && content.Attributes["page"].Value == num)
                        {
                            validNode.InnerXml += content.OuterXml;
                        }
                    }
                    rootElement.InnerXml += validNode.OuterXml;
                }
                return PreProcess1(tmpPageXml);
            }
        }

        //=============================================================================
        //Creating Tree
        //=============================================================================
        //public XmlDocument BuildXMLTree(int currPage)
        //{
        //    XmlDocument treeDoc = new XmlDocument();
        //    string xmlStructure = "<?xml version='1.0'?>\n<pbp-book />";
        //    treeDoc.LoadXml(xmlStructure);
        //    XmlNode rootNode = treeDoc.SelectSingleNode("//pbp-book");
        //    XmlNodeList xmlPageChilds = GetPageChilds(currPage.ToString());

        //    foreach (XmlNode pCurrentNode in xmlPageChilds)
        //    {
        //        XmlElement newNode = treeDoc.CreateElement(pCurrentNode.Name);

        //        newNode.SetAttribute("outerxml", pCurrentNode.SelectSingleNode(".//ln").OuterXml);
        //        if (newNode.Name.Equals("section-title"))
        //        {
        //            newNode.SetAttribute("displaytext", pCurrentNode.ParentNode.ParentNode.Attributes["type"].Value);
        //        }
        //        LineTree(pCurrentNode, newNode, currPage);
        //        rootNode.AppendChild(newNode);
        //    }
        //    return treeDoc;
        //}

        //public XmlDocument BuildXMLTree(int currPage)
        //{
        //    XmlDocument treeDoc = new XmlDocument();
        //    string xmlStructure = "<?xml version='1.0'?>\n<pbp-book />";
        //    treeDoc.LoadXml(xmlStructure);
        //    XmlNode rootNode = treeDoc.SelectSingleNode("//pbp-book");
        //    XmlNodeList xmlPageChilds = GetPageChilds(currPage.ToString());

        //    foreach (XmlNode pCurrentNode in xmlPageChilds)
        //    {
        //        XmlElement newNode = treeDoc.CreateElement(pCurrentNode.Name);

        //        newNode.SetAttribute("outerxml", pCurrentNode.SelectSingleNode(".//ln").OuterXml);
        //        if (newNode.Name.Equals("section-title"))
        //        {
        //            newNode.SetAttribute("displaytext", pCurrentNode.ParentNode.ParentNode.Attributes["type"].Value);
        //        }
        //        else if ((newNode.Name.Equals("spara")))
        //        {
        //            string type = pCurrentNode.Attributes["type"].Value;
        //            string lineorPara = pCurrentNode.ChildNodes[0].Name;
        //            string hAlign = pCurrentNode.Attributes["h-align"] == null ? "" : pCurrentNode.Attributes["h-align"].Value;
        //            newNode.SetAttribute("displaytext", pCurrentNode.Name + "( " + type + " " + lineorPara + " " + hAlign + " )");
        //        }
        //        else
        //        {
        //            newNode.SetAttribute("displaytext", pCurrentNode.Name);
        //        }

        //        LineTree(pCurrentNode, newNode, currPage);
        //        rootNode.AppendChild(newNode);
        //    }
        //    return treeDoc;
        //}

        public XmlDocument BuildXMLTree(int currPage)
        {
            XmlDocument treeDoc = new XmlDocument();
            string xmlStructure = "<?xml version='1.0'?>\n<pbp-book />";
            treeDoc.LoadXml(xmlStructure);
            XmlNode rootNode = treeDoc.SelectSingleNode("//pbp-book");
            XmlNodeList xmlPageChilds = GetPageChilds(currPage.ToString());

            foreach (XmlNode pCurrentNode in xmlPageChilds)
            {
                XmlElement newNode = treeDoc.CreateElement(pCurrentNode.Name);

                if (!pCurrentNode.Name.Equals("Table"))
                {
                    newNode.SetAttribute("outerxml", pCurrentNode.SelectSingleNode(".//ln").OuterXml);
                    if (newNode.Name.Equals("section-title"))
                    {
                        newNode.SetAttribute("displaytext", pCurrentNode.ParentNode.ParentNode.Attributes["type"].Value);
                    }
                    LineTree(pCurrentNode, newNode, currPage);
                    rootNode.AppendChild(newNode);
                }
                else
                {
                    newNode.SetAttribute("outerxml", pCurrentNode.SelectSingleNode("//Row/col").OuterXml);
                    LineTableTree(pCurrentNode, newNode, currPage);
                    rootNode.AppendChild(newNode);
                }
            }
            return treeDoc;
        }

        public XmlDocument BuildXMLChapterTree()
        {
            XmlDocument treeDoc = new XmlDocument();
            string xmlStructure = "<?xml version='1.0'?>\n<pbp-book />";
            treeDoc.LoadXml(xmlStructure);

            XmlNode rootNode = treeDoc.SelectSingleNode("//pbp-book");
            //XmlNodeList xmlPageChilds = GetPageChilds(currPage.ToString());

            //XmlNodeList xmlPageChilds = PBPDocument.SelectNodes("//post-section/descendant::section-title/ln[text()]");

            //XmlNodeList xmlPageChilds = PBPDocument.SelectNodes("descendant::section-title/ln[text()]");

            PBPDocument = new XmlDocument();

            PBPDocument.Load(@"F:\33.xml");

            XmlNodeList xmlChapterNodeList = PBPDocument.SelectNodes("//post-section/descendant::section[@type='level1']");

            foreach (XmlNode pCurrentNode in xmlChapterNodeList)
            {
                XmlElement newNode = treeDoc.CreateElement(pCurrentNode.Name);

                //if (!pCurrentNode.Name.Equals("Table"))
                //{
                    newNode.SetAttribute("outerxml", pCurrentNode.OuterXml);
                    //if (newNode.Name.Equals("section-title"))
                    //{
                    //    newNode.SetAttribute("displaytext", pCurrentNode.ParentNode.ParentNode.Attributes["type"].Value);
                    //}
                    //LineTree(pCurrentNode, newNode, currPage);

                    newNode.SetAttribute("displaytext", pCurrentNode.InnerText);
                    rootNode.AppendChild(newNode);
                //}
                //else
                //{
                //    newNode.SetAttribute("outerxml", pCurrentNode.SelectSingleNode("//Row/col").OuterXml);
                //    LineTableTree(pCurrentNode, newNode, currPage);
                //    rootNode.AppendChild(newNode);
                //}
            }
            return treeDoc;
        }


        //commented by aamir
        //public XmlDocument BuildXMLTree(int currPage)
        //{

        //    XmlDocument treeDoc = new XmlDocument();
        //    string xmlStructure = "<?xml version='1.0'?>\n<pbp-book />";
        //    treeDoc.LoadXml(xmlStructure);
        //    XmlNode rootNode = treeDoc.SelectSingleNode("//pbp-book");
        //    XmlNodeList xmlPageChilds = GetPageChilds(currPage.ToString());
        //    List<XmlNode> FinalNodes = new List<XmlNode>();
        //    for (int i = 0; i < xmlPageChilds.Count; i++)
        //    {
        //        if (xmlPageChilds[i].ParentNode.Name.Equals("box"))
        //        {
        //            FinalNodes.Add(xmlPageChilds[i].ParentNode);
        //            while (xmlPageChilds[i].NextSibling != null)
        //            {
        //                i++;
        //            }
        //        }

        //        else
        //        {
        //            FinalNodes.Add(xmlPageChilds[i]);
        //        }
        //    }
        //    foreach (XmlNode pChildNode in FinalNodes)
        //    {
        //        XmlElement parentNode = treeDoc.CreateElement(pChildNode.Name);
        //        parentNode.SetAttribute("outerxml", pChildNode.SelectSingleNode(".//ln").OuterXml);
        //        if (pChildNode.Name.Equals("section"))
        //        {
        //            parentNode.SetAttribute("outerxml", pChildNode.SelectSingleNode("//descendant::section-title").InnerText);
        //        }
        //        if (pChildNode.Name.Equals("box"))
        //        {
        //            XmlNodeList uparas = pChildNode.SelectNodes("descendant::upara");
        //            foreach (XmlNode uparaNode in uparas)
        //            {
        //                XmlElement childNode = treeDoc.CreateElement(uparaNode.Name);
        //                if (uparaNode.SelectSingleNode(".//ln") != null)
        //                {
        //                    childNode.SetAttribute("outerxml", uparaNode.SelectSingleNode(".//ln").OuterXml);
        //                    LineTree(uparaNode, childNode, currPage);
        //                    parentNode.AppendChild(childNode);
        //                }
        //            }
        //        }
        //        else
        //        {

        //            LineTree(pChildNode, parentNode, currPage);
        //        }
        //        rootNode.AppendChild(parentNode);
        //    }
        //    XmlNodeList boxNodes = treeDoc.SelectNodes("//box");
        //    //if (boxNodes.Count > 0)
        //    //{
        //    //    for (int i = 0; i < boxNodes.Count; i++)
        //    //    {
        //    //        for (int j = 0; j < boxNodes[i].ChildNodes.Count; )
        //    //        {
        //    //            boxNodes[i].RemoveChild(boxNodes[i].ChildNodes[j]);
        //    //        }
        //    //        XmlNode uparaNode = boxNodes[i].NextSibling;
        //    //        boxNodes[i].AppendChild(uparaNode);
        //    //    }
        //    //}
        //    return treeDoc;
        //}

        public void LineTree(XmlNode pCurrentNode, XmlNode newNode, int currPage)
        {
            XmlNodeList lnChilds = pCurrentNode.SelectNodes("descendant::ln|descendant::break");
            foreach (XmlNode paraNode in lnChilds)
            {
                XmlElement tmpNode = null;
                if (paraNode.Attributes["page"] != null || paraNode.Name == "break")
                {
                    if (paraNode.Name == "break" || paraNode.Attributes["page"].Value == currPage.ToString())
                    {
                        string lineNodeText = getTrimmedText(paraNode.InnerText);

                        if (paraNode.Name == "ln")
                        {
                            tmpNode = newNode.OwnerDocument.CreateElement(paraNode.Name);
                            tmpNode.SetAttribute("displaytext", lineNodeText);
                            tmpNode.SetAttribute("outerxml", paraNode.OuterXml);
                            newNode.AppendChild(tmpNode);
                        }
                        else if (paraNode.Name == "break")
                        {
                            tmpNode = newNode.OwnerDocument.CreateElement(paraNode.Name);
                            newNode.AppendChild(tmpNode);
                        }
                    }
                }
            }
        }

        public void LineTableTree(XmlNode pCurrentNode, XmlNode newNode, int currPage)
        {
            XmlNodeList headRow = pCurrentNode.SelectNodes("//Table[@page='" + currPage + "']//head-row");
            XmlNodeList row = pCurrentNode.SelectNodes("//Table[@page='" + currPage + "']//Row");

            StringBuilder tableRow = new StringBuilder();
            foreach (XmlNode paraNode in headRow)
            {
                tableRow.Length = 0;

                foreach (XmlNode col in paraNode)
                {
                    tableRow.Append(col.InnerText + " ");
                }

                XmlElement tmpNode = null;
                string lineNodeText = getTrimmedText(Convert.ToString(tableRow));
                tmpNode = newNode.OwnerDocument.CreateElement(paraNode.Name);
                tmpNode.SetAttribute("displaytext", lineNodeText);
                tmpNode.SetAttribute("outerxml", paraNode.OuterXml);
                newNode.AppendChild(tmpNode);
            }

            foreach (XmlNode paraNode in row)
            {
                tableRow.Length = 0;

                foreach (XmlNode col in paraNode)
                {
                    tableRow.Append(col.InnerText + " ");
                }

                XmlElement tmpNode = null;
                string lineNodeText = getTrimmedText(Convert.ToString(tableRow));
                tmpNode = newNode.OwnerDocument.CreateElement(paraNode.Name);
                tmpNode.SetAttribute("displaytext", lineNodeText);
                tmpNode.SetAttribute("outerxml", paraNode.OuterXml);
                newNode.AppendChild(tmpNode);
            }
        }

        public string getTrimmedText(string text)
        {
            string[] words = text.Split(' ');
            if (words.Length > 6)
            {
                string trimmedText = words[0] + " " + words[1] + "......." + words[words.Length - 2] + " " + words[words.Length - 1];
                return trimmedText;
            } return text;
        }

        public XmlNodeList GetPageChilds(string num)
        {
            XmlNodeList pageContents;
            //pageContents = this.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::section[@type=\"chapter\"]");
            //if (pageContents.Count > 0)
            //{
            //    pageContents = this.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::section[@type=\"level1\"]");
            //}
            //else if (pageContents.Count > 0)
            //{
            //    pageContents = this.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::section[@type=\"level2\"]");
            //}
            //else if (pageContents.Count > 0)
            //{
            //    pageContents = this.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::section[@type=\"level3\"]");
            //}
            //else if (pageContents.Count > 0)
            //{
            //    pageContents = this.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::section[@type=\"level4\"]");
            //}
            //else if (pageContents.Count > 0)
            //{
            //    pageContents = this.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::section[@type=\"level5\"]");
            //}
            //else
            //{
            pageContents = this.PBPDocument.SelectNodes("//*[@page=\"" + num +
                            "\"]/ancestor::upara|//*[@page=\"" + num +
                            "\"]/ancestor::spara|//*[@page=\"" + num +
                            "\"]/ancestor::npara|//*[@page=\"" + num +
                            "\"]/ancestor::table|//*[@page=\"" + num +
                            "\"]/ancestor::image|//*[@page=\"" + num +
                            "\"]/ancestor::section-title|//*[@page=\"" + num +
                            "\"]/ancestor::emphasis|//Table[@page=" + num + "]")
                            ;
            //}
            return pageContents;
        }

        //=============================================================================
        //Load XML
        //=============================================================================  

        public XmlDocument GetXml(string path)
        {

            Stream xmlStream = null;
            try
            {
                xmlStream = objImageIndex.GetHeader(path, true);
            }
            catch
            {

            }
            StreamReader reader = new StreamReader(xmlStream);
            byte[] bytes1 = new byte[xmlStream.Length];
            xmlStream.Position = 0;
            xmlStream.Read(bytes1, 0, (int)xmlStream.Length);
            string text = System.Text.Encoding.Unicode.GetString(bytes1);

            XmlDocument docXML = new XmlDocument();
            docXML.LoadXml(text);
            reader.Dispose();
            return docXML;

        }

        public bool IsValidXml(string xml)
        {
            try
            {
                XDocument.Parse(xml);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void LoadXml()
        {

            Stream xmlStream = null;
            try
            {
                xmlStream = objImageIndex.GetHeader(this.XMLPath, true);
            }
            catch
            {

            }
            using (StreamReader reader = new StreamReader(xmlStream))
            {
                byte[] bytes1 = new byte[xmlStream.Length];
                xmlStream.Position = 0;
                xmlStream.Read(bytes1, 0, (int)xmlStream.Length);

                //string text = System.Text.Encoding.Unicode.GetString(bytes1);

                string text = "";

                if (IsValidXml(Encoding.Unicode.GetString(bytes1)))
                    text = Encoding.Unicode.GetString(bytes1);
                else
                    text = Encoding.UTF8.GetString(bytes1);

                reader.Dispose();
                xmlStream.Dispose();
                try
                {
                    if (this.PBPDocument == null)
                    {
                        this.PBPDocument = new XmlDocument();
                    }
                    this.PBPDocument.LoadXml(text);
                }
                catch
                {
                    //MessageBox.Show("Cannot load xml file");
                }
            }
        }
        public void SaveXml(string xml, string path)
        {
            objImageIndex.SetHeader(xml, path);
        }

        public void SaveXml()
        {
            objImageIndex.SetHeader(this.PBPDocument.OuterXml, this.XMLPath);
        }

        public void SaveRhyw()
        {
            objImageIndex.SetHeader(this.PBPDocument.OuterXml, this.RhywPath);
        }


        ////////////////////////PdfCompare//////////////////////

        ////To make a complete page xml :: GlobalVar
        ////static XmlDocument PreProcess(XmlDocument xmlDoc)
        ////{
        ////    XmlDocument newXmlDoc = new XmlDocument();
        ////    XmlNode rootNode = newXmlDoc.CreateElement("pbp-book");
        ////    newXmlDoc.AppendChild(rootNode);

        ////    string necessaryElements = "<pbp-meta><pbp-info tag-operator=\"pakistan\" tag-date=\"2009-01-01\" file-name=\"XXXXXX\" schema-name=\"PBPBook_P02.xsd\" publication-status=\"NOT FOR PUBLICATION\" book-type=\"OTHER\" copyright-status=\"IN COPYRIGHT\" book-title=\"XXXXX\" schema-rev=\"p02\"/><doc-track></doc-track><bookrep-info><author-id>1</author-id><book-summary></book-summary><author-info></author-info></bookrep-info></pbp-meta><pbp-front><cover><image-model><front image-url=\"\"/><spine image-url=\"\"/><back image-url=\"\"/></image-model></cover><BISAC><BISAC-item><BISAC-text></BISAC-text><BISAC-code></BISAC-code></BISAC-item></BISAC><ISBN></ISBN><title-block><book-title><main-title>XX</main-title><running-header></running-header></book-title><author><full-name>FullName</full-name><prenominal /><first-name>FirstName</first-name><last-name>XX</last-name></author></title-block><book-notices></book-notices></pbp-front>";
        ////    rootNode.InnerXml = necessaryElements;
        ////    string innerXML = "";
        ////    if (xmlDoc == null)
        ////    {
        ////        innerXML = "<body><upara><ln coord=\"237.65:568.72:388.55:586.72\" page=\"1\" height=\"792\" left=\"237.65\" top=\"568.72\" font=\"BemboStd\" fontsize=\"18\" error=\"0\" ispreviewpassed=\"true\" isUserSigned=\"0\" isEditted=\"false\">Sorry! The page is blank</ln></upara></body>";
        ////    }
        ////    else
        ////    {
        ////        innerXML = xmlDoc.InnerXml;
        ////    }
        ////    rootNode.InnerXml += "<pbp-body>" + innerXML + "</pbp-body>";
        ////    return newXmlDoc;
        ////}

        ///// <summary>
        ///// Cleans the inner XML of the document and returns only the inner XML of the body node
        ///// </summary>
        ///// <param name="xmlDoc"></param>
        ///// <returns></returns>
        //public static String GetBodyTagsAsText(string xmlFilePath)
        //{
        //    XmlDocument xmlDoc = new XmlDocument();
        //    try
        //    {
        //        xmlDoc.Load(xmlFilePath);
        //    }
        //    catch
        //    {

        //    }
        //    string innerXml = xmlDoc.InnerXml;
        //    string bodyInnerXml = Regex.Match(innerXml, "(?<=<body>).*?(?=</body>)", RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.IgnorePatternWhitespace | RegexOptions.Compiled).ToString();
        //    return bodyInnerXml;
        //}

        ///// <summary>
        ///// Gets the maximum page number in the xml document
        ///// </summary>
        ///// <returns></returns>
        //public static int GetMaxPageNo()
        //{
        //    XPathNavigator navigator = GlobalVar.PBPDocument.CreateNavigator();

        //    string res = (string)navigator.Evaluate("max(//ln/@page)");


        //    XmlNode lnNode = GlobalVar.PBPDocument.SelectSingleNode("//ln[@page=max(//ln/@page)]");
        //    if (lnNode != null)
        //    {
        //        try
        //        {
        //            return int.Parse(lnNode.Attributes["page"].Value);
        //        }
        //        catch
        //        {
        //            return -1;
        //        }
        //    }
        //    else
        //        return -1;
        //}


        //private static XmlNodeList Merge(XmlNodeList lstA, XmlNodeList lstB)
        //{
        //    string tagNameInA = "";
        //    string tagNameInB = "";

        //    XmlDocument temp = new XmlDocument();
        //    string strTemp = "&lt;root>";
        //    IEnumerator ienum = lstA.GetEnumerator();
        //    while (ienum.MoveNext())
        //    {
        //        XmlNode title = (XmlNode)ienum.Current;
        //        // Console.WriteLine(title.InnerText);
        //        tagNameInA = title.Name;
        //        strTemp += "&lt;" + title.Name + "&gt;";
        //        strTemp += title.InnerText;
        //        strTemp += "&lt;/" + title.Name + "&gt;";

        //    }


        //    IEnumerator ienum2 = lstB.GetEnumerator();
        //    while (ienum2.MoveNext())
        //    {

        //        XmlNode title = (XmlNode)ienum2.Current;
        //        tagNameInB = title.Name;
        //        // Console.WriteLine(title.InnerText);
        //        strTemp += "&lt;" + title.Name + "&gt;";
        //        strTemp += title.InnerText;
        //        strTemp += "&lt;/" + title.Name + "&gt;";

        //    }

        //    strTemp += "&lt;/root>";

        //    temp.LoadXml(strTemp);

        //    XmlNodeList res = temp.SelectNodes("//" + tagNameInA + "|//" + tagNameInB + "");
        //    return res;

        //}

        ////=============================================================================
        ////Getting Content of the Page
        ////=============================================================================        
        //public static XmlDocument GetPageXmlDoc(string num)
        //{
        //    //XmlNodeList pageContents = GlobalVar.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/..");
        //    XmlNodeList pageContents = GlobalVar.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::upara|" +
        //                                                                 "//*[@page=\"" + num + "\"]/ancestor::spara|" +
        //                                                                 "//*[@page=\"" + num + "\"]/ancestor::npara|" +
        //                                                                 "//*[@page=\"" + num + "\"]/ancestor::image|" +
        //                                                                 "//*[@page=\"" + num + "\"]/ancestor::section-title|" +
        //                                                                 "//*[@page=\"" + num + "\"]/ancestor::prefix|" +
        //                                                                 "//*[@page=\"" + num + "\"]/ancestor::table"
        //                                                                 );
        //    int counter = 0;

        //    XmlDocument tmpPageXml = new XmlDocument();
        //    if (pageContents.Count == 0)
        //    {
        //        return PreProcess(null);
        //    }
        //    else
        //    {
        //        XmlNode rootElement = tmpPageXml.CreateElement("body");
        //        tmpPageXml.AppendChild(rootElement);
        //        XmlNode validNode = null;
        //        XmlNode validNode_Parent = null;

        //        foreach (XmlNode xmlNode in pageContents)
        //        {
        //            counter = 0;

        //            if ((xmlNode.Name.Equals("section-title")) && (xmlNode.ParentNode.Name.Equals("head")))
        //            {
        //                validNode_Parent = GlobalVar.PBPDocument.CreateElement(xmlNode.ParentNode.ParentNode.Name);

        //                foreach (XmlAttribute att in xmlNode.ParentNode.ParentNode.Attributes)
        //                {
        //                    ((XmlElement)validNode_Parent).SetAttribute(att.Name, att.Value);
        //                }
        //            }
        //            else
        //            {
        //                validNode_Parent = null;
        //            }

        //            validNode = GlobalVar.PBPDocument.CreateElement(xmlNode.Name);
        //            foreach (XmlAttribute att in xmlNode.Attributes)
        //            {
        //                ((XmlElement)validNode).SetAttribute(att.Name, att.Value);
        //            }

        //            XmlNodeList contentChilds = xmlNode.ChildNodes;
        //            foreach (XmlNode content in contentChilds)
        //            {
        //                if (xmlNode.Name.Equals("spara"))
        //                {
        //                    foreach (XmlNode ch in content)
        //                    {
        //                        if (counter < 1)
        //                        {
        //                            //Create a new attribute in first line of every spara for changing its color to blue
        //                            XmlAttribute attr = GlobalVar.PBPDocument.CreateAttribute("colorChange");
        //                            attr.Value = "1";
        //                            ch.Attributes.Append(attr);
        //                            counter++;
        //                        }

        //                        if (ch.Attributes["page"] != null && ch.Attributes["page"].Value == num)
        //                        {
        //                            validNode.InnerXml += content.OuterXml;
        //                        }
        //                    }
        //                }

        //                else if (xmlNode.Name.Equals("image"))
        //                {
        //                    if (content.Name.Equals("caption"))
        //                    {
        //                        if (content.FirstChild.Attributes["page"].Value != null && content.FirstChild.Attributes["page"].Value == num)
        //                        {
        //                            validNode.InnerXml += content.OuterXml;
        //                        }
        //                    }
        //                    //If there is no caption tag in image
        //                    else
        //                    {
        //                        if (content.Attributes["page"] != null && content.Attributes["page"].Value == num)
        //                        {
        //                            validNode.InnerXml += content.OuterXml;
        //                        }
        //                    }
        //                }

        //                else
        //                {
        //                    if (content.Attributes["page"] != null && content.Attributes["page"].Value == num)
        //                    {
        //                        validNode.InnerXml += content.OuterXml;
        //                    }
        //                }
        //            }

        //            if (validNode_Parent != null)
        //                rootElement.InnerXml += validNode_Parent.OuterXml + validNode.OuterXml;

        //            else
        //                rootElement.InnerXml += validNode.OuterXml;
        //        }
        //        return PreProcess(tmpPageXml);
        //    }
        //}


        //////=============================================================================
        //////Getting Content of the Page
        //////=============================================================================        


        ////public static XmlDocument GetPageXmlDoc(string num)
        ////{
        ////    //firstXmlPageNo = int.Parse(GlobalVar.PBPDocument.SelectSingleNode("//ln").Attributes["page"].Value.ToString());
        ////    XmlNodeList pageContents = GlobalVar.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/..");
        ////    //XmlNodeList pageContents = GlobalVar.PBPDocument.SelectNodes("//*[@page=\"" + (int.Parse(num) - 1 + firstXmlPageNo) + "\"]/..");
        ////    XmlDocument tmpPageXml = new XmlDocument();
        ////    if (pageContents.Count == 0)
        ////    {
        ////        return PreProcess(null);
        ////    }
        ////    else
        ////    {

        ////        XmlNode rootElement = tmpPageXml.CreateElement("body");
        ////        tmpPageXml.AppendChild(rootElement);
        ////        foreach (XmlNode xmlNode in pageContents)
        ////        {
        ////            XmlNode validNode = GlobalVar.PBPDocument.CreateElement(xmlNode.Name);
        ////            foreach (XmlAttribute att in xmlNode.Attributes)
        ////            {
        ////                ((XmlElement)validNode).SetAttribute(att.Name, att.Value);
        ////            }
        ////            //if (xmlNode.Attributes["image-url"] != null)
        ////            //{
        ////            //    ((XmlElement)validNode).SetAttribute("image-url", xmlNode.Attributes["image-url"].Value);
        ////            //}
        ////            XmlNodeList contentChilds = xmlNode.ChildNodes;
        ////            foreach (XmlNode content in contentChilds)
        ////            {
        ////                //if (content.Attributes["page"] != null && content.Attributes["page"].Value == (int.Parse(num) - 1 + firstXmlPageNo).ToString())
        ////                if (content.Attributes["page"] != null && content.Attributes["page"].Value == num)
        ////                {
        ////                    validNode.InnerXml += content.OuterXml;
        ////                }
        ////            }
        ////            rootElement.InnerXml += validNode.OuterXml;
        ////        }
        ////        return PreProcess(tmpPageXml);
        ////    }
        ////}



        ////=============================================================================
        ////Creating Tree
        ////=============================================================================
        ////public static XmlDocument BuildXMLTree(int currPage)
        ////{
        ////    XmlDocument treeDoc = new XmlDocument();
        ////    string xmlStructure = "<?xml version='1.0'?>\n<pbp-book />";
        ////    treeDoc.LoadXml(xmlStructure);
        ////    XmlNode rootNode = treeDoc.SelectSingleNode("//pbp-book");
        ////    XmlNodeList xmlPageChilds = GlobalVar.GetPageChilds(currPage.ToString());

        ////    foreach (XmlNode pChildNode in xmlPageChilds)
        ////    {
        ////        XmlElement parentNode = treeDoc.CreateElement(pChildNode.Name);
        ////        parentNode.SetAttribute("outerxml", pChildNode.SelectSingleNode(".//ln").OuterXml);

        ////        LineTree(pChildNode, parentNode, currPage);
        ////        rootNode.AppendChild(parentNode);
        ////    }
        ////    return treeDoc;
        ////}

        ////public static void LineTree(XmlNode pChildNode, XmlNode parentNode, int currPage)
        ////{
        ////    XmlNodeList lnChilds = pChildNode.SelectNodes("descendant::ln|descendant::break");
        ////    foreach (XmlNode paraNode in lnChilds)
        ////    {
        ////        XmlElement tmpNode = null;
        ////        if (paraNode.Attributes["page"] != null || paraNode.Name == "break")
        ////        {
        ////            if (paraNode.Name == "break" || paraNode.Attributes["page"].Value == currPage.ToString())
        ////            {
        ////                string lineNodeText = getTrimmedText(paraNode.InnerText);

        ////                if (paraNode.Name == "ln")
        ////                {
        ////                    tmpNode = parentNode.OwnerDocument.CreateElement(paraNode.Name);
        ////                    tmpNode.SetAttribute("displaytext", lineNodeText);
        ////                    tmpNode.SetAttribute("outerxml", paraNode.OuterXml);
        ////                    parentNode.AppendChild(tmpNode);
        ////                }
        ////                else if (paraNode.Name == "break")
        ////                {
        ////                    tmpNode = parentNode.OwnerDocument.CreateElement(paraNode.Name);
        ////                    parentNode.AppendChild(tmpNode);
        ////                }
        ////            }
        ////        }
        ////    }
        ////}
        ////public static string getTrimmedText(string text)
        ////{
        ////    string[] words = text.Split(' ');
        ////    if (words.Length > 6)
        ////    {
        ////        string trimmedText = words[0] + " " + words[1] + "......." + words[words.Length - 2] + " " + words[words.Length - 1];
        ////        return trimmedText;
        ////    } return text;
        ////}
        //////public static XmlNodeList GetPageChilds(string num)
        //////{
        //////    XmlNodeList pageContents = GlobalVar.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::box-title|//*[@page=\"" + num + "\"]/ancestor::upara|//*[@page=\"" + num + "\"]/ancestor::spara|//*[@page=\"" + num + "\"]/ancestor::npara|//*[@page=\"" + num + "\"]/ancestor::section-title|//*[@page=\"" + num + "\"]/ancestor::table|//*[@page=\"" + num + "\"]/ancestor::image");
        //////    return pageContents;
        //////}

        //public static bool SaveXml(string XmlSavePath)
        //{
        //    try
        //    {
        //        GlobalVar.PBPDocument.Save(XmlSavePath);
        //        return true;
        //    }
        //    catch (Exception exp)
        //    {
        //        throw exp;
        //    }
        //}

        ////public static void LoadXml()
        ////{
        ////    Stream xmlStream = null;
        ////    try
        ////    {
        ////        xmlStream = objImageIndex.GetHeader(GlobalVar.XMLPath, true);
        ////    }
        ////    catch
        ////    {

        ////    }
        ////    StreamReader reader = new StreamReader(xmlStream);
        ////    byte[] bytes1 = new byte[xmlStream.Length];
        ////    xmlStream.Position = 0;
        ////    xmlStream.Read(bytes1, 0, (int)xmlStream.Length);
        ////    string text = System.Text.Encoding.Unicode.GetString(bytes1);
        ////    try
        ////    {
        ////        GlobalVar.PBPDocument.LoadXml(text);
        ////    }
        ////    catch
        ////    {
        ////        //MessageBox.Show("Cannot load xml file");
        ////    }
        ////}

        ////public static void SaveXml()
        ////{
        ////    objImageIndex.SetHeader(PBPDocument.OuterXml, GlobalVar.XMLPath);
        ////}

        ////internal static void InsertError(Word wrd1, Word wrd2, string SourcePDFPath, string ProducedPDFPath, string XMLPath)
        ////{
        ////    //if (SiteSession.ErrorList == null)

        ////    {
        ////        SiteSession.ErrorList = new System.Collections.ArrayList();
        ////    }
        ////    MisMatchError wrdError = new MisMatchError();
        ////    wrdError.list1Word = wrd1;
        ////    wrdError.list2Word = wrd2;
        ////    wrdError.pdfSourcePath = SourcePDFPath;
        ////    wrdError.pdfProducedPath = ProducedPDFPath;
        ////    wrdError.xmlPath = XMLPath;
        ////    SiteSession.ErrorList.Add(wrdError);
        ////}

        ////internal static void ArrangeErrorListAsc()
        ////{
        ////    ArrayList errorList = SiteSession.ErrorList;
        ////    Hashtable ht = new Hashtable();
        ////    SortedList sr = new SortedList();

        ////    for (int i = 0; i < errorList.Count; i++)
        ////    {
        ////        MisMatchError wrdError = (MisMatchError)errorList[i];
        ////        int currPNum = ((Word)wrdError.list1Word).PageNumber;
        ////        //ht.Add(currPNum, wrdError);
        ////        sr.Add(currPNum, wrdError);
        ////    }
        ////    ArrayList arrSorted =  (ArrayList)sr.GetValueList();
        ////    SiteSession.ErrorList = arrSorted;
        ////}

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <returns>XMLFile Path</returns>
        //public static String MergeAllPages()
        //{
        //    ArrayList arrSepPages = SiteSession.AllSeperatePages;
        //    String CompleteBody = "";
        //    if (SiteSession.AllSeperatePages.Count > 0)
        //    {
        //        for (int i = 0; i < arrSepPages.Count; i++)
        //        {
        //            string xmlPath = ((SeperatePages)arrSepPages[i]).XMLPath;
        //            CompleteBody += GetBodyTagsAsText(xmlPath);
        //        }
        //    }

        //    String FinalXMLString = "";
        //    string necessaryElements = "<pbp-book><pbp-meta><pbp-info tag-operator=\"pakistan\" tag-date=\"2009-01-01\" file-name=\"XXXXXX\" schema-name=\"PBPBook_P02.xsd\" publication-status=\"NOT FOR PUBLICATION\" book-type=\"OTHER\" copyright-status=\"IN COPYRIGHT\" book-title=\"XXXXX\" schema-rev=\"p02\"/><doc-track></doc-track><bookrep-info><author-id>1</author-id><book-summary></book-summary><author-info></author-info></bookrep-info></pbp-meta><pbp-front><cover><image-model><front image-url=\"\"/><spine image-url=\"\"/><back image-url=\"\"/></image-model></cover><BISAC><BISAC-item><BISAC-text></BISAC-text><BISAC-code></BISAC-code></BISAC-item></BISAC><ISBN></ISBN><title-block><book-title><main-title>XX</main-title><running-header></running-header></book-title><author><full-name>FullName</full-name><prenominal /><first-name>FirstName</first-name><last-name>XX</last-name></author></title-block><book-notices></book-notices></pbp-front></pbp-book>";
        //    FinalXMLString = necessaryElements;

        //    FinalXMLString += "<body>" + CompleteBody + "</body>";
        //    string dirPath = ConfigurationManager.AppSettings["PDFDirPhyPath"];

        //    Random rnd = new Random();
        //    int rndVal = rnd.Next(10000); ;
        //    string filePath = dirPath + "\\_" + rndVal + ".xml";
        //    StreamWriter sw = new StreamWriter(filePath);
        //    sw.Write(FinalXMLString);
        //    sw.Close();
        //    return filePath;
        //}
    }
}

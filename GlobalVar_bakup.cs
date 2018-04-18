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
namespace Outsourcing_System
{
    public class GlobalVar_bakup
    {
        public GlobalVar_bakup()
        {

        }
        public GlobalVar_bakup(XmlDocument objXMLDoc)
        {
            this.PBPDocument = objXMLDoc;
        }


        //Khalil code adde here on 31-05-2013

        private string xMLPath;
        private string pDFPath;
        public static string ProjectFolderPath;
        private System.Xml.XmlDocument pBPDocument;
        private int firstXmlPageNo = 0;
        ImageIndex objImageIndex = new ImageIndex();

        public string XMLPath
        {
            get { return xMLPath; }
            set { xMLPath = value; }
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

        //khalil update end here......


        public int GetPageCountFromTetml(string tetmlPath)
        {
            StreamReader sr = new StreamReader(tetmlPath);
            string tetmlContent = sr.ReadToEnd();
            sr.Close();
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

        public XmlDocument GetPageXmlDoc(string num)
        {
            //firstXmlPageNo = int.Parse(GlobalVar.PBPDocument.SelectSingleNode("//ln").Attributes["page"].Value.ToString());
            XmlNodeList pageContents = this.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/..");
            List<XmlNode> FinalNodes = new List<XmlNode>();
            for (int i = 0; i < pageContents.Count; i++)
            {
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

        //=============================================================================
        //Creating Tree
        //=============================================================================

        private XmlDocument readtextIntoXml(string path)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml(File.ReadAllText(path));
            return xmldoc;
        }

        public XmlDocument BuildXMLTree(int currPage)
        {

            XmlDocument treeDoc = new XmlDocument();
            string xmlStructure = "<?xml version='1.0'?>\n<pbp-book />";
            treeDoc.LoadXml(xmlStructure);
            XmlNode rootNode = treeDoc.SelectSingleNode("//pbp-book");
            XmlNodeList xmlPageChilds = GetPageChilds(currPage.ToString());
            List<XmlNode> FinalNodes = new List<XmlNode>();
            for (int i = 0; i < xmlPageChilds.Count; i++)
            {
                if (xmlPageChilds[i].ParentNode.Name.Equals("box"))
                {
                    FinalNodes.Add(xmlPageChilds[i].ParentNode);
                    while (xmlPageChilds[i].NextSibling != null)
                    {
                        i++;
                    }
                }

                else
                {
                    FinalNodes.Add(xmlPageChilds[i]);
                }
            }
            foreach (XmlNode pChildNode in FinalNodes)
            {
                XmlElement parentNode = treeDoc.CreateElement(pChildNode.Name);
                parentNode.SetAttribute("outerxml", pChildNode.SelectSingleNode(".//ln").OuterXml);
                if (pChildNode.Name.Equals("section"))
                {
                    parentNode.SetAttribute("outerxml", pChildNode.SelectSingleNode("//descendant::section-title").InnerText);
                }
                if (pChildNode.Name.Equals("box"))
                {
                    XmlNodeList uparas = pChildNode.SelectNodes("descendant::upara");
                    foreach (XmlNode uparaNode in uparas)
                    {
                        XmlElement childNode = treeDoc.CreateElement(uparaNode.Name);
                        if (uparaNode.SelectSingleNode(".//ln") != null)
                        {
                            childNode.SetAttribute("outerxml", uparaNode.SelectSingleNode(".//ln").OuterXml);
                            LineTree(uparaNode, childNode, currPage);
                            parentNode.AppendChild(childNode);
                        }
                    }
                }
                else
                {

                    LineTree(pChildNode, parentNode, currPage);
                }
                rootNode.AppendChild(parentNode);
            }
            XmlNodeList boxNodes = treeDoc.SelectNodes("//box");
            //if (boxNodes.Count > 0)
            //{
            //    for (int i = 0; i < boxNodes.Count; i++)
            //    {
            //        for (int j = 0; j < boxNodes[i].ChildNodes.Count; )
            //        {
            //            boxNodes[i].RemoveChild(boxNodes[i].ChildNodes[j]);
            //        }
            //        XmlNode uparaNode = boxNodes[i].NextSibling;
            //        boxNodes[i].AppendChild(uparaNode);
            //    }
            //}
            return treeDoc;
        }
        //public XmlDocument BuildXMLTree(int currPage)
        //{
        //    try
        //    {
        //        XmlNodeList xmlPageChilds = GetPageChilds(currPage.ToString());

        //        //Testing by khalil 

        //        #region |Khalil code for Menu herarichy|

        //        XmlDocument tempDoc = new XmlDocument();
        //        string tempStructure = "<?xml version='1.0'?>\n<pbp-book />";
        //        tempDoc.LoadXml(tempStructure);
        //        XmlNode temprootNode = tempDoc.SelectSingleNode("//pbp-book");
        //        foreach (XmlNode item in xmlPageChilds)
        //        {
        //            if (item.Name.Equals("section") || item.Name.Equals("upara") || item.Name.Equals("box"))
        //            {
        //                XmlNode importedNode = temprootNode.OwnerDocument.ImportNode(item, true);
        //                if (item.SelectSingleNode("ancestor::section[1]") != null)
        //                {
        //                    string section = item.SelectSingleNode("ancestor::section[1]").Attributes["type"].Value;
        //                    XmlElement uparaEleme = (XmlElement)importedNode;
        //                    if (!section.Equals(""))
        //                    {

        //                        uparaEleme.SetAttribute("Section", section);
        //                    }
        //                    temprootNode.AppendChild(uparaEleme);
        //                }
        //                else
        //                {
        //                    XmlElement uparaEleme = (XmlElement)importedNode;
        //                    uparaEleme.SetAttribute("Section", "null");
        //                    temprootNode.AppendChild(uparaEleme);
        //                }
        //            }
        //        }



        //        XmlNodeList lstSections = tempDoc.SelectNodes("//section");

        //        foreach (XmlNode objNode in lstSections)
        //        {
        //            XmlNode objsectNode = objNode.SelectSingleNode("ancestor::section");
        //            if (objsectNode != null)
        //            {
        //                objNode.ParentNode.RemoveChild(objNode);
        //            }
        //        }
        //        lstSections = tempDoc.SelectNodes("//section");
        //        //removes all Upara, boxes, and sections from Section Node....
        //        foreach (XmlNode objNode in lstSections)
        //        {
        //            XmlNode objtempNode = objNode.SelectSingleNode("descendant::body[1]");

        //            if (objtempNode != null)
        //            {
        //                objtempNode.RemoveAll();
        //            }

        //        }

        //        XmlNodeList lstotherNodes = tempDoc.SelectNodes("//upara|//box");

        //        //Moves Upara and Boxes to correspondent Levels.
        //        foreach (XmlNode otherNode in lstotherNodes)
        //        {
        //            foreach (XmlNode item in lstSections)
        //            {
        //                if (otherNode.Attributes["Section"].Value.Equals(item.Attributes["type"].Value))
        //                {
        //                    item.SelectSingleNode("descendant::body[1]").AppendChild(otherNode);
        //                }
        //            }
        //        }


        //        //Moves Section other than chapter to their Correspondent Section.
        //        foreach (XmlNode secNode in lstSections)
        //        {
        //            if (secNode.Attributes["Section"].Value != "null")
        //            {
        //                foreach (XmlNode temSection in lstSections)
        //                {
        //                    if (temSection.Attributes["type"].Value.Equals(secNode.Attributes["Section"].Value))
        //                    {
        //                        temSection.AppendChild(secNode);
        //                    }
        //                }
        //            }
        //        }
        //        XmlNodeList allNodes = tempDoc.SelectNodes("//*[@Section]");
        //        foreach (XmlNode node in allNodes)
        //        {
        //            node.Attributes.Remove(node.Attributes["Section"]);
        //        }

        //        tempDoc.Save(@"C:\Documents and Settings\mkhalil\Desktop\desFolder\tempDoc.xml");
        //        #endregion

        //        //End Testing by khalil
        //        xmlPageChilds = tempDoc.SelectNodes("//section");
        //        if (xmlPageChilds.Count == 0)
        //        {
        //            xmlPageChilds = tempDoc.SelectNodes("//upara|//spara|//box|//image");
        //        }
        //        XmlDocument treeDoc = new XmlDocument();
        //        string xmlStructure = "<?xml version='1.0'?>\n<pbp-book />";
        //        treeDoc.LoadXml(xmlStructure);
        //        XmlNode rootNode = treeDoc.SelectSingleNode("//pbp-book");

        //        //khalil Test Code Part 2
        //        XmlNodeList lstPageNodes = tempDoc.SelectNodes("//*");
        //        foreach (XmlNode contentNode in lstPageNodes)
        //        {
        //            if (contentNode.Name.Equals("section") || contentNode.Name.Equals("upara") || contentNode.Name.Equals("box"))
        //            {
        //                XmlNode tNode = rootNode.OwnerDocument.ImportNode(contentNode, true);
        //                XmlElement parentNode = (XmlElement)tNode;
        //                if (contentNode.Name.Equals("section"))
        //                {
        //                    if (contentNode.SelectSingleNode("ancestor::section[1]") != null)
        //                    {
        //                        parentNode.SetAttribute("Section", contentNode.SelectSingleNode("ancestor::section[1]").Attributes["type"].Value);
        //                    }
        //                    else
        //                    {
        //                        parentNode.SetAttribute("Section", "null");
        //                    }

        //                    parentNode.SetAttribute("displaytext", contentNode.Attributes["type"].Value);
        //                }
        //                else
        //                {

        //                    if (contentNode.SelectSingleNode("ancestor::section[1]") != null)
        //                    {
        //                        parentNode.RemoveAll();
        //                        parentNode.SetAttribute("Section", contentNode.SelectSingleNode("ancestor::section[1]").Attributes["type"].Value);
        //                    }

        //                    LineTree(contentNode, parentNode, currPage);
        //                }
        //                rootNode.AppendChild(parentNode);
        //            }
        //        }

        //        XmlNodeList lsttreedocSections = treeDoc.SelectNodes("//section");

        //        foreach (XmlNode objNode in lsttreedocSections)
        //        {
        //            XmlNode objsectNode = objNode.SelectSingleNode("ancestor::section");
        //            if (objsectNode != null)
        //            {
        //                objNode.ParentNode.RemoveChild(objNode);
        //            }
        //        }
        //        lsttreedocSections = treeDoc.SelectNodes("//section");
        //        //removes all Upara, boxes, and sections from Section Node....
        //        foreach (XmlNode objNode in lsttreedocSections)
        //        {
        //            XmlNode sectionTitle = objNode.SelectSingleNode("descendant::section-title");

        //            while (objNode.ChildNodes.Count>0)
        //            {
        //                objNode.RemoveChild(objNode.ChildNodes[0]);
        //            }
        //            objNode.AppendChild(sectionTitle);

        //            //XmlNode objtempNode = objNode.SelectSingleNode("descendant::body[1]");

        //            //if (objtempNode != null)
        //            //{
        //            //    objtempNode.RemoveAll();
        //            //}

        //        }

        //        XmlNodeList lsttreedocotherNodes = treeDoc.SelectNodes("//upara|//box");

        //        //Moves Upara and Boxes to correspondent Levels.
        //        foreach (XmlNode otherNode in lsttreedocotherNodes)
        //        {
        //            foreach (XmlNode item in lsttreedocSections)
        //            {
        //                if (otherNode.Attributes["Section"].Value.Equals(item.Attributes["type"].Value))
        //                {
        //                    item.AppendChild(otherNode);
        //                }
        //            }
        //        }


        //        //Moves Section other than chapter to their Correspondent Section.
        //        foreach (XmlNode secNode in lsttreedocSections)
        //        {
        //            if (secNode.Attributes["Section"].Value != "null")
        //            {
        //                foreach (XmlNode temSection in lsttreedocSections)
        //                {
        //                    if (temSection.Attributes["type"].Value.Equals(secNode.Attributes["Section"].Value))
        //                    {
        //                        temSection.AppendChild(secNode);
        //                    }
        //                }
        //            }
        //        }
        //        XmlNodeList alltreedocNodes = treeDoc.SelectNodes("//*[@Section]");
        //        foreach (XmlNode node in alltreedocNodes)
        //        {
        //            node.Attributes.Remove(node.Attributes["Section"]);
        //        }



        //        //Khalil test code part 2

        //        #region |Commnetd on 04-10-2013 |

        //        ////List<XmlNode> FinalNodes = new List<XmlNode>();
        //        ////for (int i = 0; i < xmlPageChilds.Count; i++)
        //        ////{
        //        ////    if (xmlPageChilds[i].ParentNode.Name.Equals("box"))
        //        ////    {
        //        ////        FinalNodes.Add(xmlPageChilds[i].ParentNode);
        //        ////        while (xmlPageChilds[i].NextSibling != null)
        //        ////        {
        //        ////            i++;
        //        ////        }
        //        ////    }

        //        ////    else
        //        ////    {
        //        ////        FinalNodes.Add(xmlPageChilds[i]);
        //        ////    }
        //        ////}

        //        ////foreach (XmlNode pChildNode in FinalNodes)
        //        ////{
        //        ////    XmlElement parentNode = treeDoc.CreateElement(pChildNode.Name);
        //        ////    parentNode.SetAttribute("outerxml", pChildNode.SelectSingleNode(".//ln").OuterXml);
        //        ////    if (pChildNode.Name.Equals("section"))
        //        ////    {
        //        ////        parentNode.SetAttribute("displaytext", pChildNode.Attributes["type"].Value);
        //        ////        XmlElement sectionTitle = treeDoc.CreateElement(pChildNode.SelectSingleNode("descendant::section-title").Name);
        //        ////        sectionTitle.SetAttribute("displaytext", getTrimmedText(pChildNode.SelectSingleNode("descendant::section-title").InnerText));
        //        ////        parentNode.AppendChild(sectionTitle);
        //        ////        if (pChildNode.SelectSingleNode("descendant::section[1]") != null)
        //        ////        {

        //        ////        }


        //        ////    }
        //        ////    else if (pChildNode.Name.Equals("box"))
        //        ////    {
        //        ////        XmlNodeList uparas = pChildNode.SelectNodes("descendant::upara");
        //        ////        foreach (XmlNode uparaNode in uparas)
        //        ////        {
        //        ////            XmlElement childNode = treeDoc.CreateElement(uparaNode.Name);
        //        ////            if (uparaNode.SelectSingleNode(".//ln") != null)
        //        ////            {
        //        ////                childNode.SetAttribute("outerxml", uparaNode.SelectSingleNode(".//ln").OuterXml);
        //        ////                LineTree(uparaNode, childNode, currPage);
        //        ////                parentNode.AppendChild(childNode);
        //        ////            }
        //        ////        }
        //        ////    }
        //        ////    else
        //        ////    {

        //        ////        LineTree(pChildNode, parentNode, currPage);
        //        ////    }
        //        ////    rootNode.AppendChild(parentNode);
        //        ////}
        //        ////XmlNodeList boxNodes = treeDoc.SelectNodes("//box");
        //        //if (boxNodes.Count > 0)
        //        //{
        //        //    for (int i = 0; i < boxNodes.Count; i++)
        //        //    {
        //        //        for (int j = 0; j < boxNodes[i].ChildNodes.Count; )
        //        //        {
        //        //            boxNodes[i].RemoveChild(boxNodes[i].ChildNodes[j]);
        //        //        }
        //        //        XmlNode uparaNode = boxNodes[i].NextSibling;
        //        //        boxNodes[i].AppendChild(uparaNode);
        //        //    }
        //        //}
        //        #endregion

        //        treeDoc.Save(@"C:\Documents and Settings\mkhalil\Desktop\desFolder\treeDoc.xml");

        //        return treeDoc;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}
        private void sectionConverter(XmlDocument doc, XmlElement parenNode, XmlNode sectionNode, int currPage)
        {
            XmlNodeList uparas = sectionNode.SelectNodes("descendant::upara");
            foreach (XmlNode uparaNode in uparas)
            {
                XmlElement childNode = doc.CreateElement(uparaNode.Name);
                if (uparaNode.SelectSingleNode(".//ln") != null)
                {
                    childNode.SetAttribute("outerxml", uparaNode.SelectSingleNode(".//ln").OuterXml);
                    LineTree(uparaNode, childNode, currPage);
                    parenNode.AppendChild(childNode);
                }
            }

        }
        public void LineTree(XmlNode pChildNode, XmlNode parentNode, int currPage)
        {
            XmlNodeList lnChilds = pChildNode.SelectNodes("descendant::ln|descendant::break");
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
                            tmpNode = parentNode.OwnerDocument.CreateElement(paraNode.Name);
                            tmpNode.SetAttribute("displaytext", lineNodeText);
                            tmpNode.SetAttribute("outerxml", paraNode.OuterXml);
                            parentNode.AppendChild(tmpNode);
                        }
                        else if (paraNode.Name == "break")
                        {
                            tmpNode = parentNode.OwnerDocument.CreateElement(paraNode.Name);
                            tmpNode.SetAttribute("displaytext", paraNode.Name);
                            tmpNode.SetAttribute("outerxml", paraNode.OuterXml);
                            parentNode.AppendChild(tmpNode);
                            //tmpNode = parentNode.OwnerDocument.CreateElement(paraNode.Name);
                            //for (int i = 0; i < paraNode.Attributes.Count; i++)
                            //{
                            //    tmpNode.SetAttribute(paraNode.Attributes[i].Name, paraNode.Attributes[i].Value);
                            //}
                            //parentNode.AppendChild(tmpNode);
                        }
                    }
                }
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
        public XmlNodeList GetPageChilds(XmlNode MainNode, string num)
        {
            XmlNodeList pageContents;
            pageContents = this.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::upara|//*[@page=\"" + num + "\"]/ancestor::spara|//*[@page=\"" + num + "\"]/ancestor::npara|//*[@page=\"" + num + "\"]/ancestor::table|//*[@page=\"" + num + "\"]/ancestor::image");
            return pageContents;
        }

        public XmlNodeList GetPageChilds(string num)
        {
            XmlNodeList pageContents;
            //Befor Change by Khalil on 03-10-2013
            // pageContents = this.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::section-title|//*[@page=\"" + num + "\"]/ancestor::upara|//*[@page=\"" + num + "\"]/ancestor::spara|//*[@page=\"" + num + "\"]/ancestor::npara|//*[@page=\"" + num + "\"]/ancestor::table|//*[@page=\"" + num + "\"]/ancestor::image");
            //After Change by Khalil on 03-10-2013
            pageContents = this.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::*");


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

            return docXML;

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
            StreamReader reader = new StreamReader(xmlStream);
            byte[] bytes1 = new byte[xmlStream.Length];
            xmlStream.Position = 0;
            xmlStream.Read(bytes1, 0, (int)xmlStream.Length);
            string text = System.Text.Encoding.Unicode.GetString(bytes1);
            try
            {
                this.PBPDocument.LoadXml(text);
            }
            catch
            {
                //MessageBox.Show("Cannot load xml file");
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
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;


  public  class SchemaElements
    {
       
        public SchemaElements()
        {
            
        }       
        /// <summary>
        /// Takes type and id as parameter and creates a section node of passed type
        /// </summary>
        /// <param name="strType">Section Type</param>
        /// <param name="strID">Section Id</param>
        /// <param name="XmlDoc"></param>
        /// <returns>Section Node</returns>
        public XmlElement CreateSection(string strType, string strID, XmlDocument XmlDoc)
        {
            XmlElement NodeSection = XmlDoc.CreateElement("section");
            XmlAttribute AttribType = XmlDoc.CreateAttribute("type");
            AttribType.Value = strType;
            XmlAttribute AttribId = XmlDoc.CreateAttribute("id");
            AttribId.Value = strID;
            NodeSection.Attributes.Append(AttribId);
            NodeSection.Attributes.Append(AttribType);
            return NodeSection;
        }
        public XmlElement CreatePreSection(string strType, string strID, XmlDocument XmlDoc)
        {
            XmlElement NodeSection = XmlDoc.CreateElement("pre-section");
            XmlAttribute AttribType = XmlDoc.CreateAttribute("type");
            AttribType.Value = strType;
            XmlAttribute AttribId = XmlDoc.CreateAttribute("id");
            AttribId.Value = strID;
            NodeSection.Attributes.Append(AttribId);
            NodeSection.Attributes.Append(AttribType);
            return NodeSection;
        }
        public XmlElement CreatePostSection(string strType, string strID, XmlDocument XmlDoc)
        {
            XmlElement NodeSection = XmlDoc.CreateElement("post-section");
            XmlAttribute AttribType = XmlDoc.CreateAttribute("type");
            AttribType.Value = strType;
            XmlAttribute AttribId = XmlDoc.CreateAttribute("id");
            AttribId.Value = strID;
            NodeSection.Attributes.Append(AttribId);
            NodeSection.Attributes.Append(AttribType);
            return NodeSection;
        }
        /// <summary>
        /// Takes type and id as parameter and creates a section node of passed type
        /// </summary>
        /// <param name="strType">Section Type</param>
        /// <param name="strID">Section Id</param>
        /// <param name="XmlDoc"></param>
        /// <returns>Section Node</returns>
        public XmlElement CreateSection(string nodeName,string strType, string strID, XmlDocument XmlDoc)
        {
            XmlElement NodeSection = XmlDoc.CreateElement(nodeName);
            XmlAttribute AttribType = XmlDoc.CreateAttribute("type");
            AttribType.Value = strType;
            XmlAttribute AttribId = XmlDoc.CreateAttribute("id");
            AttribId.Value = strID;
            NodeSection.Attributes.Append(AttribId);
            NodeSection.Attributes.Append(AttribType);
            return NodeSection;
        }
        /// <summary>
        /// creates a head node 
        /// </summary>
        /// <param name="strTitle">Section Title</param>
        /// <param name="strPrefix">Prefix</param>
        /// <param name="XmlDoc"></param>
        /// <returns>head node</returns>
        public XmlElement CreateHeadNode(string strTitle, string strPrefix, XmlDocument XmlDoc)
        {            
            string lnValue = Regex.Match(strTitle, "<ln.*?>").Value.ToString();
            string val = Regex.Replace(strTitle, "<[ ]?/?[ ]?ln.*?>","");
            if (lnValue!="")
            {
                strTitle = lnValue + val + "</ln>";
            }
            if (strPrefix != "" && strTitle != "")
            {
                string ln1 = Regex.Match(strTitle, "<ln.*?>").Value;
                string ln2 = Regex.Match(strPrefix, "<ln.*?>").Value;
                if (ln1.Trim() == ln2.Trim())
                {
                    strPrefix = Regex.Replace(strPrefix, "<ln.*?>", "<PREFIX>");
                    strPrefix = Regex.Replace(strPrefix, "</ln>", "</PREFIX>");
                    strTitle = Regex.Replace(strTitle, "<ln.*?>", ln1+strPrefix);
                    strPrefix = "";
                }
            }
            XmlElement NodeHead = XmlDoc.CreateElement("head");
            XmlElement NodeTitle = XmlDoc.CreateElement("section-title");
            NodeTitle.InnerText = strTitle.Replace("&", "amp;"); ;
            XmlElement NodePrefix = XmlDoc.CreateElement("prefix");
            NodePrefix.InnerXml = strPrefix;            
            XmlElement NodeAuthor = XmlDoc.CreateElement("author");
            XmlElement NodeRunningHeader = XmlDoc.CreateElement("running-header");            
            XmlElement NodeBrailleHeader = XmlDoc.CreateElement("braille-header");         
            XmlElement NodeSecNum = XmlDoc.CreateElement("section-num");
            NodeSecNum.InnerText = "1";//static;
            NodeHead.AppendChild(NodeSecNum);
            NodeHead.AppendChild(NodePrefix);
            NodeHead.AppendChild(NodeTitle);
            NodeHead.AppendChild(NodeAuthor);
            NodeHead.AppendChild(NodeRunningHeader);
            NodeHead.AppendChild(NodeBrailleHeader);
            return NodeHead;
        }
        public XmlElement CreateEmphasisTag(string InnerText, string strType, XmlDocument XmlDoc)
        {
            XmlElement NodeEmphasis = XmlDoc.CreateElement("emphasis");
            XmlAttribute AttribType = XmlDoc.CreateAttribute("type");
            AttribType.Value = strType;
            NodeEmphasis.Attributes.Append(AttribType);
            NodeEmphasis.InnerText = InnerText;
            return NodeEmphasis;
        }
        public XmlElement CreateUpara(string innerXml, string Id, XmlDocument XmlDoc,string txtindent,string paddingbottom)
        {
            XmlElement NodeUpara = XmlDoc.CreateElement("upara");
            XmlAttribute AttribId = XmlDoc.CreateAttribute("id");
            XmlAttribute txtIndent = XmlDoc.CreateAttribute("text-indent");
            XmlAttribute paddingBottom = XmlDoc.CreateAttribute("padding-bottom");
            paddingBottom.Value = paddingbottom;
            txtIndent.Value = txtindent;
            AttribId.Value = Id;
            XmlAttribute Attribpnum = XmlDoc.CreateAttribute("pnum");
            Attribpnum.Value = Id;
            NodeUpara.Attributes.Append(AttribId);
            NodeUpara.Attributes.Append(Attribpnum);
            NodeUpara.Attributes.Append(txtIndent);
            NodeUpara.Attributes.Append(paddingBottom);

            MatchCollection lnNodeList = Regex.Matches(innerXml, "<ln.*?>.*?</ln>");
            foreach (Match lnNode in lnNodeList)
            {
                if (lnNode.Value.Trim() != "" && !lnNode.Value.Contains("ispreviewpassed=\"false\" />"))
                {
                    string text = Regex.Replace(lnNode.Value, "</?ln.*?>", "");
                    MatchCollection empCol = Regex.Matches(text, "<.*?>|<[a-zA-Z \\[\\]]+|[a-zA-Z \\]\\[]+>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    for (int m = 0; m < empCol.Count; m++)
                    {
                        if (!empCol[m].Value.Contains("emphasis") && !empCol[m].Value.Contains("break"))
                        {
                            string newVal = empCol[m].Value.Replace("<", "&lt;").Replace(">", "&gt;");
                            innerXml = innerXml.Replace(empCol[m].Value, newVal);
                        }
                    }
                }
            }
            innerXml = Regex.Replace(innerXml, "<([0-9])", "&lt;$1").Replace("&", "&amp;").Replace("&amp;lt;", "&lt;");
            NodeUpara.InnerXml = innerXml;
            
            return NodeUpara;
        }
        public XmlElement CreateSpara(string Id, XmlDocument xmlDoc, string type, string txtindent, string paddingbottom)
        {
            XmlElement NodeSpara = xmlDoc.CreateElement("spara");
            XmlAttribute AttribType = xmlDoc.CreateAttribute("type");
            XmlAttribute AttribId = xmlDoc.CreateAttribute("id");
            XmlAttribute Attribpnum = xmlDoc.CreateAttribute("pnum");
            XmlAttribute txtIndent = xmlDoc.CreateAttribute("text-indent");
            XmlAttribute paddingBottom = xmlDoc.CreateAttribute("padding-bottom");
            paddingBottom.Value = paddingbottom;
            txtIndent.Value = txtindent;
            Attribpnum.Value = Id;
            AttribId.Value = Id;
            AttribType.Value = type;
            NodeSpara.Attributes.Append(AttribType);
            NodeSpara.Attributes.Append(AttribId);
            NodeSpara.Attributes.Append(Attribpnum);
            NodeSpara.Attributes.Append(txtIndent);
            NodeSpara.Attributes.Append(paddingBottom);
            return NodeSpara;
        }
        public XmlElement CreatePara(XmlDocument xmlDoc,string innerXml)
        {
            XmlElement NodePara = xmlDoc.CreateElement("para");
            NodePara.InnerText = innerXml;
            string temp = NodePara.InnerXml;
            temp = Regex.Replace(temp, "&lt;", "<");
            temp = Regex.Replace(temp, "&gt;", ">");
            NodePara.InnerXml = temp;

            //NodePara.InnerText = innerXml;
            //string temp = NodePara.InnerText;
            //temp = Regex.Replace(temp, "&lt;", "<");
            //temp = Regex.Replace(temp, "&gt;", ">");
            //NodePara.InnerText = temp;

            return NodePara;
        }
        public XmlElement CreateBreakTag(string strType, string strNum, int strID, XmlDocument xmlDoc)
        {
            XmlElement NodeBreak = xmlDoc.CreateElement("break");
            XmlAttribute AttribType = xmlDoc.CreateAttribute("type");
            AttribType.Value = strType;
            XmlAttribute AttribNum = xmlDoc.CreateAttribute("num");
            AttribNum.Value = strNum;
            XmlAttribute AttribID = xmlDoc.CreateAttribute("id");
            AttribID.Value = strID.ToString();
            //AttribID.Value = strNum;
            NodeBreak.Attributes.Append(AttribType);
            NodeBreak.Attributes.Append(AttribNum);
            NodeBreak.Attributes.Append(AttribID);
            return NodeBreak;
        }
        public XmlElement CreateImageTag(string strID, string Url,XmlDocument xmlDoc,string caption)
        {
            XmlElement NodeImage = xmlDoc.CreateElement("image");
            XmlAttribute AttribId = xmlDoc.CreateAttribute("id");
            XmlAttribute AttribUrl = xmlDoc.CreateAttribute("image-url");
            AttribId.Value = strID;
            AttribUrl.Value = Url;
            NodeImage.Attributes.Append(AttribId);
            NodeImage.Attributes.Append(AttribUrl);
            if (caption.Trim() != string.Empty)
            {
                XmlElement NodeCation = xmlDoc.CreateElement("caption");
                NodeCation.InnerText = caption;
                NodeImage.AppendChild(NodeCation);
            }
            return NodeImage;
        }
        public XmlElement CreateTableNode(XmlDocument xmlDoc,string ID)
        {
            XmlElement NodeTable = xmlDoc.CreateElement("table");
            XmlAttribute AttribHeadRow = xmlDoc.CreateAttribute("head-row");
            AttribHeadRow.Value = "off";
            XmlAttribute AttribID = xmlDoc.CreateAttribute("id");
            AttribID.Value = ID;
            XmlAttribute Attribborder=xmlDoc.CreateAttribute("border");
            Attribborder.Value = "on";
            NodeTable.Attributes.Append(AttribHeadRow);
            NodeTable.Attributes.Append(AttribID);
            NodeTable.Attributes.Append(Attribborder);
            XmlElement NodeHeadRow=xmlDoc.CreateElement("head-row");
            XmlElement NodeHeadcol=xmlDoc.CreateElement("head-col");
            XmlAttribute Attribwidth=xmlDoc.CreateAttribute("width");
            Attribwidth.Value = "0";
            NodeHeadcol.Attributes.Append(Attribwidth);
            NodeHeadRow.AppendChild(NodeHeadcol);
            XmlElement NodeRow=xmlDoc.CreateElement("row");
            XmlElement NodeCol = xmlDoc.CreateElement("col");
            NodeCol.InnerText = "dummy Table Change it with original";
            NodeRow.AppendChild(NodeCol);
            NodeTable.AppendChild(NodeHeadRow);
            NodeTable.AppendChild(NodeRow);
            return NodeTable;
        }

    }


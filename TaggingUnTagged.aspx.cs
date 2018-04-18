﻿using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using BookMicroBeta;

namespace Outsourcing_System
{
    public partial class TaggingUnTagged : System.Web.UI.Page
    {
        
        GlobalVar objGlobal = new GlobalVar();
        MyDBClass objMyDBClass = new MyDBClass();
        ConversionClass objConversionClass = new ConversionClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.Title = "Outsourcing System :: Tagging UnTagged Elements";
            this.lblMessage.Text = "";
            if (Session["objUser"] == null)
            {
                Response.Redirect("BookMicro.aspx");
            }
            if (!Page.IsPostBack)
            {
                LoadPDF();
            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //Load PDF
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region void LoadPDF()
        public void LoadPDF()
        {
            string bookID = Request.QueryString["bid"] != null ? Request.QueryString["bid"].ToString() : "";
            if (bookID != "")
            {
                //GlobalVar.XMLPath = Server.MapPath("Files/" + bookID.Split(new char[]{'-'})[0] + "/"+ bookID + "/TaggingUntagged/" + bookID + ".rhyw");
                objGlobal.XMLPath = objMyDBClass.MainDirPhyPath + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID + "/TaggingUntagged/" + bookID + ".rhyw"; //Shoaib here update in Files Directory
                Session["XMLPath"] = objGlobal.XMLPath;
                //GlobalVar.PDFPath = Server.MapPath("Files/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID.Split(new char[] { '-' })[0] + ".pdf");
                objGlobal.PDFPath = objMyDBClass.MainDirPhyPath + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID.Split(new char[] { '-' })[0] + ".pdf"; //Shoaib here update in Files Directory




                //GlobalVar.PDFPath = Server.MapPath("Files/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID + "/TaggingUntagged/" + bookID + ".pdf");
                objGlobal.PBPDocument = new XmlDocument();
                //XmlDocument temXML= GlobalVar.GetXml( MyDBClass.MainDirPhyPath + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID + "/TaggingUntagged/" + bookID + ".rhyw");
                objGlobal.LoadXml();
                Session["PBPDocument"] = objGlobal.PBPDocument;
                XmlNodeList untaggedList = objGlobal.PBPDocument.SelectNodes("//untagged");                
                Session["tempXML"] = objGlobal.PBPDocument;
                Session["rhywPath"] = objMyDBClass.MainDirPhyPath + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID + "/TaggingUntagged/" + bookID + ".rhyw";
                Session["Nodes"] = untaggedList;
                Session["untaggedIndex"] = "0";
                ShowPDF(Session["untaggedIndex"].ToString());
            }
            else
            {
                this.lblMessage.Text = "Sorry! Missing Book Path";
            }
        }
        #endregion

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //Show / Navigate PDF
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region void ShowPDF(string untaggedIndex)
        public void ShowPDF(string untaggedIndex)
        {
            try
            {
                this.cmbTag.SelectedIndex = 0;
                this.pnlSpara.Visible = false;
                this.pnlNpara.Visible = false;
                this.pnlSection.Visible = false;
                this.pnlFootNote.Visible = false;
                this.pnlText.Visible = false;
                this.pnlLevelSubtype.Visible = false;
                this.pnlEmphasis.Visible = false;

                string pdfPage = "";
                string elementType = "";
                XmlNodeList unTaggedElementList = (Session["Nodes"] as XmlNodeList);
                this.UntaggedRemaining.Text = "Remaining Untagged Elements :: " + unTaggedElementList.Count;
                if (int.Parse(Session["untaggedIndex"].ToString()) == unTaggedElementList.Count)
                {
                    this.lblMessage.Text = "Ajustment of Untagged Element completed successfully";
                    this.elemType.Text = "";
                    this.btnFinish.Visible = true;
                }
                else
                {
                    XmlNode unTaggedElement = (unTaggedElementList[int.Parse(untaggedIndex)] as XmlNode);
                    if (unTaggedElement.Attributes["type"].Value == "footnote" && unTaggedElement.Attributes["ftype"].Value == "link")
                    {
                        this.UnTaggedText.Text = unTaggedElement.InnerText.Replace("<", "&lt;").Replace(">", "&gt;");
                        pdfPage = unTaggedElement.ParentNode.Attributes["page"].Value;
                        elementType = "Foot Note of type Link";
                    }
                    else
                    {
                        if (unTaggedElement.Attributes["type"].Value == "footnote")
                        {
                            elementType = "Foot Note of type Text";
                        }
                        else
                        {
                            elementType = "Element of type " + unTaggedElement.Attributes["type"].Value;
                        }
                        XmlNode singleLine = unTaggedElement.SelectSingleNode(".//ln");
                        this.UnTaggedText.Text = singleLine.InnerXml.Replace("<", "&lt;").Replace(">", "&gt;"); ;
                        pdfPage = singleLine.Attributes["page"].Value;
                    }
                    this.elemType.Text = elementType;
                    Session["untaggedIndex"] = int.Parse(Session["untaggedIndex"].ToString()) + 1;

                    //string newPDFName = Server.MapPath("~/Files/" + Request.QueryString["bid"].ToString().Split(new char[] { '-' })[0] + "/" + Request.QueryString["bid"].ToString() + "/TaggingUntagged/Page" + pdfPage + ".pdf");
                    string newPDFName = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString().Split(new char[] { '-' })[0] + "/" + Request.QueryString["bid"].ToString() + "/TaggingUntagged/Page" + pdfPage + ".pdf";
                    if (!File.Exists(newPDFName))
                    {
                        objConversionClass.ExtractPages(objGlobal.PDFPath, newPDFName, int.Parse(pdfPage), int.Parse(pdfPage));
                    }
                    //PDFViewerTarget.FilePath = "Files/" + Request.QueryString["bid"].ToString().Split(new char[] { '-' })[0] + "/" + Request.QueryString["bid"].ToString() + "/TaggingUntagged/Page" + pdfPage + ".pdf#toolbar=0";
                    PDFViewerTarget.FilePath = Session["MainDirectory"].ToString() + "/" + Request.QueryString["bid"].ToString().Split(new char[] { '-' })[0] + "/" + Request.QueryString["bid"].ToString() + "/TaggingUntagged/Page" + pdfPage + ".pdf#toolbar=0";

                    //PDFViewerTarget.FilePath = ("Files/" + Request.QueryString["bid"].ToString().Split(new char[] { '-' })[0] + "/" + Request.QueryString["bid"].ToString() + "/TaggingUntagged/" + Request.QueryString["bid"].ToString() + ".pdf") + "#toolbar=0&amp;page=" + pdfPage;                   
                }
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }
        #endregion

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //Submitt and Navigate to Next
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        protected void btnSubmit_Click(object sender, ImageClickEventArgs e)
        {
            switch (cmbTag.SelectedValue)
            {
                case "upara":
                    {
                        XmlNodeList unTaggedElementList = (Session["Nodes"] as XmlNodeList);
                        XmlNode unTaggedElement = (unTaggedElementList[int.Parse(Session["untaggedIndex"].ToString()) - 1] as XmlNode);

                        uparaHandler(unTaggedElement, "");
                        break;
                    }
                case "spara": { sparaHandler(); break; }
                case "npara": { nparaHandler(); break; }
                case "pre-section": { presectionHandler(); break; }
                case "post-section": { postsectionHandler(); break; }
                case "section":
                    {
                        XmlNodeList unTaggedElementList = (Session["Nodes"] as XmlNodeList);
                        XmlNode unTaggedElement = (unTaggedElementList[int.Parse(Session["untaggedIndex"].ToString()) - 1] as XmlNode);
                        sectionHandler(unTaggedElement, false);
                        break;
                    }
                case "footnote": { footnoteHandler(); break; }
                case "table": { tableHandler(); break; }
                case "caption": { captionHandler(); break; }
                case "emphasized": { emphasizedHandler(); break; }
                case "skip": { skipHandler(); break; }
                case "text":
                    {
                        XmlNodeList unTaggedElementList = (Session["Nodes"] as XmlNodeList);
                        XmlNode unTaggedElement = (unTaggedElementList[int.Parse(Session["untaggedIndex"].ToString()) - 1] as XmlNode);

                        textHandler(unTaggedElement);
                        break;
                    }
            }
            //ShowPDF(Session["untaggedIndex"].ToString());
            LoadPDF();
        }


        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //Handlers of Different Tags
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        #region Handlers of Different Tags
        public void presectionHandler()
        {
            XmlNodeList unTaggedElementList = (Session["Nodes"] as XmlNodeList);
            XmlNode unTaggedElement = (unTaggedElementList[int.Parse(Session["untaggedIndex"].ToString()) - 1] as XmlNode);
            XmlNode NodeSection = CreateSectionNode(unTaggedElement, "pre-section", "other", unTaggedElement.InnerXml, "");
            XmlNode parentNode = unTaggedElement.ParentNode.ParentNode;
            if (parentNode.Name == "pre-section")
            {
                parentNode.ParentNode.InsertAfter(NodeSection, parentNode);
            }
            else
            {
                parentNode = unTaggedElement.OwnerDocument.SelectSingleNode("//pbp-front");
                parentNode.AppendChild(NodeSection);
            }
            unTaggedElement.ParentNode.RemoveChild(unTaggedElement);
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            objGlobal.SaveXml();
            objGlobal.PBPDocument = null;
        }

        public void postsectionHandler()
        {
            XmlNodeList unTaggedElementList = (Session["Nodes"] as XmlNodeList);
            XmlNode unTaggedElement = (unTaggedElementList[int.Parse(Session["untaggedIndex"].ToString()) - 1] as XmlNode);
            XmlNode NodeSection = CreateSectionNode(unTaggedElement, "post-section", "other", unTaggedElement.InnerXml, "");
            XmlNode parentNode = unTaggedElement.ParentNode.ParentNode;
            if (parentNode.Name == "post-section")
            {
                parentNode.ParentNode.InsertAfter(NodeSection, parentNode);
            }
            else
            {
                parentNode = unTaggedElement.OwnerDocument.SelectSingleNode("//pbp-end");
                parentNode.AppendChild(NodeSection);
            }
            unTaggedElement.ParentNode.RemoveChild(unTaggedElement);
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            objGlobal.SaveXml();
            objGlobal.PBPDocument = null;
        }

        public void uparaHandler(XmlNode unTaggedElement, string text)
        {
            try
            {
                XmlNodeList nodeList = unTaggedElement.ChildNodes;

                XmlElement temp = unTaggedElement.OwnerDocument.CreateElement("upara");
                temp.SetAttribute("id", "0");
                temp.SetAttribute("pnum", "0");
            Pick:
                foreach (XmlNode nod in nodeList)
                {
                    //if (text != "")
                    //{
                    //    nod.InnerText = text;
                    //}
                    temp.AppendChild(nod);
                    goto Pick;
                }
                unTaggedElement.ParentNode.InsertAfter(temp, unTaggedElement);
                unTaggedElement.ParentNode.RemoveChild(unTaggedElement);
                if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
                {
                    objGlobal.XMLPath = Session["XMLPath"].ToString();
                }
                if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
                {
                    objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                }
                objGlobal.SaveXml();
                Session["PBPDocument"] = objGlobal.PBPDocument;
                //GlobalVar.PBPDocument = null;
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = "UPara Handler :: " + ex.Message;
            }
        }

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //Spara Handler
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public void sparaHandler()
        {
            try
            {
                XmlNodeList unTaggedElementList = (Session["Nodes"] as XmlNodeList);
                XmlNode unTaggedElement = (unTaggedElementList[int.Parse(Session["untaggedIndex"].ToString()) - 1] as XmlNode);
                XmlNodeList nodeList = unTaggedElement.SelectNodes(".//ln");
                string SparaType = ddSparaType.SelectedValue;
                string SparaAlign = "";
                if (SparaType == "other")
                {
                    SparaAlign = ddOtherAlign.SelectedValue;
                }
                string SparaSubtype = ddStanzaType.SelectedValue;
                string isStanza = chkStanza.Checked == true ? "yes" : "no";
                if (isStanza == "yes")
                {
                    XmlElement temp = null;
                    temp = unTaggedElement.OwnerDocument.CreateElement("spara");
                    temp.SetAttribute("id", "0");
                    temp.SetAttribute("pnum", "0");
                    temp.SetAttribute("type", SparaType);
                    if (SparaAlign != "")
                    {
                        temp.SetAttribute("h-align", SparaAlign);
                    }
                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        if (nodeList[i].InnerXml != "")
                        {
                            XmlElement subElem = null;
                            subElem = unTaggedElement.OwnerDocument.CreateElement(SparaSubtype);
                            subElem.AppendChild(nodeList[i]);
                            temp.AppendChild(subElem);
                        }
                    }
                    unTaggedElement.ParentNode.InsertAfter(temp, unTaggedElement);
                }
                else
                {
                    XmlNode refNode = unTaggedElement;
                    for (int i = 0; i < nodeList.Count; i++)
                    {
                        XmlElement temp = null;
                        temp = unTaggedElement.OwnerDocument.CreateElement("spara");
                        temp.SetAttribute("id", "0");
                        temp.SetAttribute("pnum", "0");
                        temp.SetAttribute("type", SparaType);
                        if (SparaAlign != "")
                        {
                            temp.SetAttribute("h-align", SparaAlign);
                        }
                        if (nodeList[i].InnerXml != "")
                        {
                            XmlElement subElem = null;
                            subElem = unTaggedElement.OwnerDocument.CreateElement(SparaSubtype);
                            subElem.AppendChild(nodeList[i]);
                            temp.AppendChild(subElem);
                        }
                        unTaggedElement.ParentNode.InsertAfter(temp, refNode);
                        refNode = temp;
                    }
                }
                unTaggedElement.ParentNode.RemoveChild(unTaggedElement);
                if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
                {
                    objGlobal.XMLPath = Session["XMLPath"].ToString();
                }
                if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
                {
                    objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                }
                objGlobal.SaveXml();
                objGlobal.PBPDocument = null;
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = "SPara Handler :: " + ex.Message;
            }
        }

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //Npara Handler
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public void nparaHandler()
        {
            try
            {
                XmlNodeList unTaggedElementList = (Session["Nodes"] as XmlNodeList);
                XmlNode unTaggedElement = (unTaggedElementList[int.Parse(Session["untaggedIndex"].ToString()) - 1] as XmlNode);
                XmlNodeList nodeList = unTaggedElement.SelectNodes(".//ln");
                if (nodeList.Count > 0)
                {
                    XmlElement nElem = unTaggedElement.OwnerDocument.CreateElement("npara");
                    nElem.SetAttribute("id", "0");
                    nElem.SetAttribute("pnum", "0");
                    nElem.SetAttribute("text-indent", unTaggedElement.Attributes["text-indent"] != null ? unTaggedElement.Attributes["text-indent"].Value : "0");
                    nElem.SetAttribute("padding-bottom", unTaggedElement.Attributes["padding-bottom"] != null ? unTaggedElement.Attributes["padding-bottom"].Value : "0");
                    bool isok = true;
                    if (chkHaseNumber.Checked == true)
                    {
                        string val = nodeList[0].InnerXml.Split(new char[] { ' ' })[0];
                        if (val != "")
                        {
                            string newVal = "<num>" + val + "</num>";
                            if (nodeList[0].InnerXml != "" && nodeList[0].InnerXml.Substring(0, val.Length) == val)
                            {
                                nodeList[0].InnerXml = nodeList[0].InnerXml.Remove(0, val.Length);
                                nodeList[0].InnerXml = newVal + nodeList[0].InnerXml;
                            }
                        }
                    }
                    else if (ddStartOption1.SelectedValue != "" && ddStartOption2.SelectedValue != "")
                    {
                        nodeList[0].InnerXml = "<num>" + ddStartOption1.SelectedValue + ddStartOption2.SelectedValue + "<num>" + nodeList[0].InnerXml;
                    }
                    else
                    {
                        isok = false;
                    }
                    if (isok == false)
                    {
                        Session["untaggedIndex"] = int.Parse(Session["untaggedIndex"].ToString()) - 1;
                        this.pnlNpara.Visible = false;
                        this.cmbTag.SelectedIndex = 0;
                        this.lblMessage.Text = "Option(s) for NPara is not selected";
                        this.UpdatePanel1.Update();
                    }
                    else
                    {
                        foreach (XmlNode lnNode in nodeList)
                        {
                            nElem.AppendChild(lnNode);
                        }
                        unTaggedElement.ParentNode.InsertAfter(nElem, unTaggedElement);
                        unTaggedElement.ParentNode.RemoveChild(unTaggedElement);
                        if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
                        {
                            objGlobal.XMLPath = Session["XMLPath"].ToString();
                        }
                        if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
                        {
                            objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                        }
                        objGlobal.SaveXml();
                        objGlobal.PBPDocument = null;
                    }
                }
                else
                {
                    Session["untaggedIndex"] = int.Parse(Session["untaggedIndex"].ToString()) - 1;
                    this.pnlNpara.Visible = false;
                    this.cmbTag.SelectedIndex = 0;
                    this.lblMessage.Text = "Unable to convert into Npara";
                    this.UpdatePanel1.Update();
                }
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = "NPara Handler :: " + ex.Message;
            }
        }
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //Section Handler
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        public void sectionHandler(XmlNode unTaggedElement, bool isApplyAll)
        {
            try
            {
                this.lblMessage.Text = "";
                string levelType = ddLevels.SelectedValue;
                string subType = ddLevelSubtype.SelectedValue;
                XmlNode tempElemToDel = null;
                string prefix = "", title = "";
                bool isBoth = false;
                if (subType == "prefix")
                {
                    if (unTaggedElement.NextSibling != null && unTaggedElement.NextSibling.Name == "untagged" && (unTaggedElement.NextSibling.Attributes["type"].Value.Contains("level") || unTaggedElement.NextSibling.Attributes["type"].Value.Contains("part") || unTaggedElement.NextSibling.Attributes["type"].Value.Contains("chapter") || unTaggedElement.NextSibling.Attributes["type"].Value.Contains("section")))
                    {
                        prefix = unTaggedElement.InnerXml;
                        title = unTaggedElement.NextSibling.InnerXml;
                        tempElemToDel = unTaggedElement.NextSibling;
                        isBoth = true;
                    }
                    else if (unTaggedElement.PreviousSibling == null && unTaggedElement.ParentNode.Name == "body" && (unTaggedElement.ParentNode.ParentNode.Name == "section" || unTaggedElement.ParentNode.ParentNode.Name == "pre-section" || unTaggedElement.ParentNode.ParentNode.Name == "post-section"))
                    {
                        unTaggedElement.ParentNode.ParentNode.SelectSingleNode("//prefix").InnerXml = unTaggedElement.InnerXml;
                    }
                    else if (unTaggedElement.NextSibling == null && unTaggedElement.ParentNode.Name == "body")
                    {
                        XmlNode prefixParentNode = FindNextSibling(unTaggedElement.ParentNode);
                        prefixParentNode.SelectSingleNode("//prefix").InnerXml = unTaggedElement.InnerXml;
                    }
                }
                else if (subType == "title")
                {
                    if (unTaggedElement.PreviousSibling == null && unTaggedElement.ParentNode.Name == "body" && unTaggedElement.ParentNode.ParentNode.SelectSingleNode("//head/section-title").InnerXml == "" && (unTaggedElement.ParentNode.ParentNode.Name == "section" || unTaggedElement.ParentNode.ParentNode.Name == "pre-section" || unTaggedElement.ParentNode.ParentNode.Name == "post-section"))
                    {
                        unTaggedElement.ParentNode.ParentNode.SelectSingleNode("//section-title").InnerXml = unTaggedElement.InnerXml;
                    }
                    else
                    {
                        if (this.chkCaps.Checked == true && unTaggedElement.SelectSingleNode(".//ln") != null)
                        {
                            unTaggedElement.SelectSingleNode(".//ln").InnerText = unTaggedElement.SelectSingleNode(".//ln").InnerText.ToUpper();
                        }
                        title = unTaggedElement.InnerXml;
                    }
                }
                else if (subType == "continue")
                {
                    XmlNodeList xmlNodeList = unTaggedElement.SelectSingleNode(".//ancestor::section|.//ancestor::pre-section|.//ancestor::post-section").SelectNodes(".//head/section-title/ln");
                    xmlNodeList[xmlNodeList.Count - 1].InnerXml = xmlNodeList[xmlNodeList.Count - 1].InnerXml + " " + unTaggedElement.InnerText;
                }
                XmlNode sectionNode=unTaggedElement.SelectSingleNode(".//ancestor::section");
                if (title != "" && levelType.Equals(sectionNode.Attributes["type"].Value))
                {
                    sectionNode.SelectSingleNode(".//descendant::section-title").AppendChild(unTaggedElement.ChildNodes[0]);
                    unTaggedElement.ParentNode.RemoveChild(unTaggedElement);
                }
                else if (title != "" || prefix != "")
                {
                    XmlNode NodeSection = CreateSectionNode(unTaggedElement, "section", levelType, title, prefix);
                    //Parent
                    XmlNode parentNode = unTaggedElement.ParentNode.ParentNode == null ? unTaggedElement.ParentNode : unTaggedElement.ParentNode.ParentNode;
                    if (parentNode.Name == "pre-section" || parentNode.Name == "post-section")
                    {
                        parentNode.InsertAfter(NodeSection, unTaggedElement.ParentNode);
                    }
                    else if (parentNode.Attributes["type"] != null && parentNode.Attributes["type"].Value == levelType)
                    {
                        parentNode.ParentNode.InsertAfter(NodeSection, parentNode);
                    }
                    else
                    {
                        if (parentNode.Name == "pbp-body" && levelType == "chapter")
                        {
                            if (unTaggedElement.ParentNode.NextSibling != null && (!unTaggedElement.ParentNode.NextSibling.Name.Equals("section")))
                            {
                                parentNode.InsertAfter(NodeSection, unTaggedElement.ParentNode);
                            }
                            else
                            {
                                unTaggedElement.ParentNode.ParentNode.AppendChild(NodeSection);
                            }
                        }
                        else
                        {
                            parentNode = NodeComparison(parentNode, levelType);
                            XmlNode insertAfter = ReturnLevel(unTaggedElement.ParentNode.Name, levelType, unTaggedElement.ParentNode);
                            if (insertAfter != null && (insertAfter.Name.Equals("pbp-body")))
                            {
                                insertAfter.AppendChild(NodeSection);
                            }
                            else
                            {
                                parentNode.InsertAfter(NodeSection, insertAfter);
                            }
                        }
                    }
                }
                if (unTaggedElement.ParentNode != null)
                {
                    if (isBoth == true && tempElemToDel != null)
                    {
                        tempElemToDel.ParentNode.RemoveChild(tempElemToDel);
                    }
                    unTaggedElement.ParentNode.RemoveChild(unTaggedElement);
                }
                if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
                {
                    objGlobal.XMLPath = Session["XMLPath"].ToString();
                }
                if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
                {
                    objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                }
                objGlobal.SaveXml();
                if (isApplyAll == false)
                {
                    objGlobal.PBPDocument = null;
                }
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = "You may selected wrong level<br />" + ex.Message;
            }
        }
        public XmlNode FindNextSibling(XmlNode currentNode)
        {
            XmlNode retNode = currentNode;
            if (retNode.NextSibling == null)
            {
                retNode = retNode.ParentNode;
            }
            if (retNode.NextSibling == null)
            {
                retNode = retNode.ParentNode;
            }
            if (retNode.NextSibling == null)
            {
                retNode = retNode.ParentNode;
            }
            if (retNode.NextSibling == null)
            {
                retNode = retNode.ParentNode;
            }

            if (retNode.NextSibling != null && retNode.NextSibling.Name == "section" || retNode.NextSibling.Name == "pre-section" || retNode.NextSibling.Name == "post-section")
            {
                retNode = retNode.NextSibling;
            }
            else if (retNode.NextSibling != null && retNode.NextSibling.Name == "section" || retNode.NextSibling.Name == "pre-section" || retNode.NextSibling.Name == "post-section")
            {
                retNode = retNode.NextSibling;
            }
            else if (retNode.NextSibling != null && retNode.NextSibling.Name == "section" || retNode.NextSibling.Name == "pre-section" || retNode.NextSibling.Name == "post-section")
            {
                retNode = retNode.NextSibling;
            }
            else if (retNode.NextSibling != null && retNode.NextSibling.Name == "section" || retNode.NextSibling.Name == "pre-section" || retNode.NextSibling.Name == "post-section")
            {
                retNode = retNode.NextSibling;
            }

            return retNode;
        }
        public XmlNode CreateSectionNode(XmlNode unTaggedElement, string elemType, string sectionType, string title, string prefix)
        {
            //Section                    
            XmlElement NodeSection = unTaggedElement.OwnerDocument.CreateElement(elemType);
            XmlAttribute AttribType = unTaggedElement.OwnerDocument.CreateAttribute("type");
            AttribType.Value = sectionType;
            XmlAttribute AttribId = unTaggedElement.OwnerDocument.CreateAttribute("id");
            AttribId.Value = "0";
            NodeSection.Attributes.Append(AttribId);
            NodeSection.Attributes.Append(AttribType);

            //Head
            XmlElement NodeHead = unTaggedElement.OwnerDocument.CreateElement("head");
            XmlElement NodeTitle = unTaggedElement.OwnerDocument.CreateElement("section-title");
            NodeTitle.InnerXml = title;
            XmlElement NodePrefix = unTaggedElement.OwnerDocument.CreateElement("prefix");
            NodePrefix.InnerXml = prefix;
            XmlElement NodeAuthor = unTaggedElement.OwnerDocument.CreateElement("author");
            XmlElement NodeRunningHeader = unTaggedElement.OwnerDocument.CreateElement("running-header");
            XmlElement NodeBrailleHeader = unTaggedElement.OwnerDocument.CreateElement("braille-header");
            XmlElement NodeSecNum = unTaggedElement.OwnerDocument.CreateElement("section-num");
            NodeSecNum.InnerText = "1";
            NodeHead.AppendChild(NodeSecNum);
            NodeHead.AppendChild(NodePrefix);
            NodeHead.AppendChild(NodeTitle);
            NodeHead.AppendChild(NodeAuthor);
            NodeHead.AppendChild(NodeRunningHeader);
            NodeHead.AppendChild(NodeBrailleHeader);

            NodeSection.AppendChild(NodeHead);

            //Body
            XmlElement elemBody = unTaggedElement.OwnerDocument.CreateElement("body");
            elemBody.SetAttribute("id", "0");

            if (sectionType != "book" && (unTaggedElement.NextSibling != null && (unTaggedElement.NextSibling.Name.Equals("untagged"))))
            {
                elemBody.AppendChild(unTaggedElement.NextSibling);
            }

            else if (sectionType == "book" && (unTaggedElement.NextSibling != null))
            {
                while (unTaggedElement.NextSibling != null)
                {
                    XmlNode currentlinNode = unTaggedElement.FirstChild;
                    XmlNode nextLineNode = unTaggedElement.NextSibling.FirstChild;
                    if (currentlinNode.Attributes["fontsize"] != null && nextLineNode.Attributes["fontsize"] != null && (!currentlinNode.Attributes["fontsize"].Value.Equals(nextLineNode.Attributes["fontsize"].Value)))
                    {
                        elemBody.AppendChild(unTaggedElement.NextSibling);
                    }
                    else
                    {
                        break;
                    }
                }

            }
            if (sectionType != "book")
            {
                while (unTaggedElement.NextSibling != null && (!unTaggedElement.NextSibling.Name.Equals("untagged")))
                {
                    elemBody.AppendChild(unTaggedElement.NextSibling);
                }
            }
            NodeSection.AppendChild(elemBody);
            return NodeSection;
        }

        public XmlNode ReturnLevel(string currNode, string newNode, XmlNode SourceNode)
        {
            XmlNode retNode = null;
            //Change done by Khalil as PBP-Body does not have attribute of "type"
            if (currNode == "body" && (SourceNode.ParentNode.Attributes["type"] != null))
            {
                if (SourceNode.ParentNode.Attributes["type"].Value == "book" && newNode == "chapter")
                {
                    retNode = SourceNode;
                }
                if (SourceNode.ParentNode.Attributes["type"].Value == "chapter" && newNode == "level1")
                {
                    retNode = SourceNode;
                }
                else if (SourceNode.ParentNode.Attributes["type"].Value == "level1" && newNode == "level2")
                {
                    retNode = SourceNode;
                }
                else if (SourceNode.ParentNode.Attributes["type"].Value == "level2" && newNode == "level3")
                {
                    retNode = SourceNode;
                }
                else if (SourceNode.ParentNode.Attributes["type"].Value == "chapter" && newNode == "chapter")
                {
                    retNode = SourceNode.ParentNode;
                }
                else if (SourceNode.ParentNode.Attributes["type"].Value == "level1" && newNode == "level1")
                {
                    retNode = SourceNode.ParentNode;
                }
                else if (SourceNode.ParentNode.Attributes["type"].Value == "level2" && newNode == "level2")
                {
                    retNode = SourceNode.ParentNode;
                }
                else if (SourceNode.ParentNode.Attributes["type"].Value == "level3" && newNode == "level3")
                {
                    retNode = SourceNode.ParentNode;
                }
                else if (SourceNode.ParentNode.Attributes["type"].Value == "level2" && newNode == "level1")
                {
                    retNode = SourceNode.ParentNode.ParentNode;
                }
                else if (SourceNode.ParentNode.Attributes["type"].Value == "level3" && newNode == "level2")
                {
                    retNode = SourceNode.ParentNode.ParentNode;
                }
            }
            else
            {
                retNode = SourceNode.ParentNode;
            }
            return retNode;
        }

        #region NodeComparison(string newNodeName)
        public XmlNode NodeComparison(XmlNode currentNode, string newNodeName)
        {
            XmlNode tempNode = null;
        jump:
            if (currentNode.Name == "section")
            {
                string sType = currentNode.Attributes["type"].Value;
                int res = comparison(sType, newNodeName);
                if (res == 0)
                {
                    tempNode = currentNode.ParentNode;
                }
                else if (res == 1)
                {
                    tempNode = currentNode;
                }
                else
                {
                    currentNode = currentNode.ParentNode;
                    goto jump;
                }
            }
            return tempNode;
        }
        public int comparison(string currNode, string newNode)
        {
            int left = 0, right = 0;
            int returnValue = -1;

            string presection = "pre-section";
            string postsection = "post-section";
            string book = "book";
            string part = "part";
            string chapter = "chapter";
            string level1 = "level1";
            string level2 = "level2";
            string level3 = "level3";
            string level4 = "level4";

            if (currNode == book)
            {
                left = 6;
            }
            else if (currNode == part)
            {
                left = 6;
            }
            else if (currNode == chapter)
            {
                left = 5;
            }
            else if (currNode == presection)
            {
                left = 5;
            }
            else if (currNode == postsection)
            {
                left = 5;
            }
            else if (currNode == level1)
            {
                left = 4;
            }
            else if (currNode == level2)
            {
                left = 3;
            }
            else if (currNode == level3)
            {
                left = 2;
            }
            else if (currNode == level4)
            {
                left = 1;
            }


            if (newNode == book)
            {
                right = 6;
            }
            else if (newNode == part)
            {
                right = 6;
            }
            else if (newNode == chapter)
            {
                right = 5;
            }
            else if (newNode == presection)
            {
                right = 5;
            }
            else if (newNode == postsection)
            {
                right = 5;
            }
            else if (newNode == level1)
            {
                right = 4;
            }
            else if (newNode == level2)
            {
                right = 3;
            }
            else if (newNode == level3)
            {
                right = 2;
            }
            else if (newNode == level4)
            {
                right = 1;
            }

            if (left == 6 && right == 4)    //True for Level1 after part or chapter
            {
                returnValue = 3;
            }
            else if (left - right == 1) //Going right level for entry
            {
                returnValue = 1;
            }
            else if (left == right) //Both have same parent
            {
                returnValue = 0;
            }
            else if (left - right > 1) //Invalid Entery level
            {
                returnValue = 3;
            }
            else if (left < right) //Find suitable parent for new entery
            {
                returnValue = 2;
            }
            return returnValue;
        }
        #endregion

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //Skip / Remove Handler
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public void skipHandler()
        {
            XmlNodeList unTaggedElementList = (Session["Nodes"] as XmlNodeList);
            XmlNode unTaggedElement = (unTaggedElementList[int.Parse(Session["untaggedIndex"].ToString()) - 1] as XmlNode);
            unTaggedElement.ParentNode.RemoveChild(unTaggedElement);
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            objGlobal.SaveXml();
            objGlobal.PBPDocument = null;
        }

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //Foot Note Handler
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        public void footnoteHandler()
        {
            XmlNodeList unTaggedElementList = (Session["Nodes"] as XmlNodeList);
            XmlNode unTaggedElement = (unTaggedElementList[int.Parse(Session["untaggedIndex"].ToString()) - 1] as XmlNode);
            unTaggedElement.ParentNode.InnerXml = unTaggedElement.ParentNode.InnerXml.Replace(unTaggedElement.OuterXml, "<footnote id=\"1\">" + this.txtFootNoteDef.Text + "</footnote>");
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            objGlobal.SaveXml();
            objGlobal.PBPDocument = null;
        }

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //Text Definition Method
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        protected void btnTextApplyAll_Click(object sender, EventArgs e)
        {
            XmlNodeList unTaggedElementList = (Session["Nodes"] as XmlNodeList);
            XmlNode unTaggedElement = (unTaggedElementList[int.Parse(Session["untaggedIndex"].ToString()) - 1] as XmlNode);
            XmlNodeList unTaggedSameElemList;
            if (unTaggedElement.SelectSingleNode("ln") != null)
            {
                string fontSize = unTaggedElement.SelectSingleNode("ln").Attributes["fontsize"].Value;
                string fontName = unTaggedElement.SelectSingleNode("ln").Attributes["font"].Value;
                unTaggedSameElemList = unTaggedElement.OwnerDocument.SelectNodes(".//ln[@font=\"" + fontName + "\" and @fontsize=\"" + fontSize + "\"]/ancestor::untagged");
            }
            else
            {
                string type = unTaggedElement.Attributes["type"].Value;
                string xpath = ".//untagged[@type=\"" + type + "\"";
                if (unTaggedElement.Attributes["ftype"] != null)
                {
                    xpath += " and @ftype=\"" + unTaggedElement.Attributes["ftype"].Value + "\"]";
                }
                unTaggedSameElemList = unTaggedElement.OwnerDocument.SelectNodes(xpath);
            }

            foreach (XmlNode node in unTaggedSameElemList)
            {
                textHandler(node);
            }
            Response.Redirect("TaggingUnTagged.aspx?bid=" + Request.QueryString["bid"].ToString());
        }
        public void textHandler(XmlNode unTaggedElement)
        {
            string text = "";
            if (ddTextType.SelectedValue == "superscript" || ddTextType.SelectedValue == "subscript")
            {
                text = " ^" + unTaggedElement.InnerText;
            }
            else if (ddTextType.SelectedValue == "text" || ddTextType.SelectedValue == "continue")
            {
                text = unTaggedElement.InnerText;
            }
            if (text != "")
            {
                if (unTaggedElement.ParentNode != null && (unTaggedElement.ParentNode.Name != "ln"))
                {
                    uparaHandler(unTaggedElement, "");
                }
                else if (unTaggedElement.ParentNode != null && (unTaggedElement.ParentNode.Name == "ln"))
                {
                    unTaggedElement.ParentNode.InnerXml = unTaggedElement.ParentNode.InnerXml.Replace(unTaggedElement.OuterXml, text);
                    if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
                    {
                        objGlobal.XMLPath = Session["XMLPath"].ToString();
                    }
                    if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
                    {
                        objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                    }
                    objGlobal.SaveXml();
                    Session["PBPDocument"] = objGlobal.PBPDocument;
                    //GlobalVar.PBPDocument = null;
                }
            }
            else
            {
                this.lblMessage.Text = "Please select any option for Text";
                LoadPDF();
            }
        }

        public void tableHandler()
        {

        }

        public void captionHandler()
        {

        }

        public void emphasizedHandler()
        {
            XmlNodeList unTaggedElementList = (Session["Nodes"] as XmlNodeList);
            XmlNode unTaggedElement = (unTaggedElementList[int.Parse(Session["untaggedIndex"].ToString()) - 1] as XmlNode);
            string emphasis = "<emphasis type=\"" + rdoEmphasis.SelectedValue + "\">";
            string tag = emphasis + unTaggedElement.InnerXml + "</emphasis>";
            XmlNode parentNode = unTaggedElement.ParentNode;
            if (parentNode.Name == "body")
            {
                if (unTaggedElement.PreviousSibling.Name == "upara" || unTaggedElement.PreviousSibling.Name == "spara" || unTaggedElement.PreviousSibling.Name == "npara")
                {
                    parentNode = unTaggedElement.PreviousSibling;
                    parentNode.InnerXml = parentNode.InnerXml + " " + tag;
                }
                else if (unTaggedElement.NextSibling.Name == "upara" || unTaggedElement.NextSibling.Name == "spara" || unTaggedElement.NextSibling.Name == "npara")
                {
                    parentNode = unTaggedElement.NextSibling;
                    parentNode.InnerXml = tag + " " + parentNode.InnerXml;
                }
            }
            unTaggedElement.ParentNode.RemoveChild(unTaggedElement);
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            objGlobal.SaveXml();
            objGlobal.PBPDocument = null;
        }
        #endregion


        protected void lnkUserPanle_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserPanel.aspx");
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Server.Transfer("BookMicro.aspx");
        }

        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
        //Show and Hide Panels and Options
        //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

        protected void ddLevelSubtype_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.ddLevelSubtype.Visible = true;
            this.ddLevelSubtype.SelectedIndex = 0;
            this.UpdatePanel1.Update();
        }
        protected void cmbTag_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pnlSpara.Visible = false;
            this.pnlNpara.Visible = false;
            this.pnlSection.Visible = false;
            this.pnlFootNote.Visible = false;
            this.pnlText.Visible = false;
            this.pnlLevelSubtype.Visible = false;
            this.pnlHelp.Visible = false;

            switch (cmbTag.SelectedValue)
            {
                case "upara": { break; }
                case "spara":
                    {
                        this.pnlSpara.Visible = true;
                        this.chkStanza.Checked = false;
                        this.ddSparaType.SelectedIndex = 0;
                        break;
                    }
                case "npara":
                    {
                        this.pnlNpara.Visible = true;
                        this.chkHaseNumber.Checked = false;
                        this.ddStartOption1.SelectedIndex = 0;
                        this.ddStartOption2.SelectedIndex = 0;
                        break;
                    }
                case "section":
                    {
                        this.pnlSection.Visible = true;
                        this.pnlLevelSubtype.Visible = false;
                        this.ddLevelSubtype.SelectedIndex = 0;
                        this.ddLevels.SelectedIndex = 0;
                        this.chkCaps.Checked = false;
                        break;
                    }
                case "pre-section": { break; }
                case "post-section": { break; }
                case "footnote":
                    {
                        this.pnlFootNote.Visible = true;
                        this.txtFootNoteDef.Text = "";
                        break;
                    }
                case "text":
                    {
                        this.pnlText.Visible = true;
                        this.ddTextType.SelectedIndex = 0;
                        break;
                    }
                case "table": { break; }
                case "caption": { break; }
                case "emphasized":
                    {
                        this.pnlEmphasis.Visible = true;
                        break;
                    }
                case "skip": { break; }
            }
            this.UpdatePanel1.Update();
        }

        //Visible Other Spara Aligment Option
        protected void ddSparaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddSparaType.SelectedValue == "other")
            {
                ddOtherAlign.Visible = true;
            }
            else
            {
                ddOtherAlign.Visible = false;
            }
            this.UpdatePanel1.Update();
        }


        protected void btnApplyAll_Click(object sender, EventArgs e)
        {
            XmlNodeList unTaggedElementList = (Session["Nodes"] as XmlNodeList);
            XmlNode unTaggedElement = (unTaggedElementList[int.Parse(Session["untaggedIndex"].ToString()) - 1] as XmlNode);

            string text_indent = unTaggedElement.Attributes["text-indent"].Value;
            string fontSize = unTaggedElement.SelectSingleNode("ln").Attributes["fontsize"].Value;
            string fontName = unTaggedElement.SelectSingleNode("ln").Attributes["font"].Value;

            XmlNodeList unTaggedSameElemList = unTaggedElement.OwnerDocument.SelectNodes(".//ln[@font=\"" + fontName + "\" and @fontsize=\"" + fontSize + "\"]/ancestor::untagged");
            foreach (XmlNode node in unTaggedSameElemList)
            {
                //if (node.Attributes["text-indent"].Value == text_indent)
                //{
                sectionHandler(node, true);
                //}
            }
            Response.Redirect("TaggingUnTagged.aspx?bid=" + Request.QueryString["bid"].ToString());
        }

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["tempXML"] != null)
                {
                    XmlDocument temXML = Session["tempXML"] as XmlDocument;
                    XmlNodeList untaggedList = temXML.SelectNodes("//untagged");
                    XmlNodeList pageList = temXML.SelectNodes("//break");
                    foreach (XmlNode item in pageList)
                    {
                        if (item.Attributes["type"].Value.Equals("page"))
                        {
                            item.Attributes["num"].Value = item.Attributes["id"].Value;
                        }
                    }
                    if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
                    {
                        objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                    }
                    temXML.SelectSingleNode("//pbp-body").RemoveChild(objGlobal.PBPDocument.SelectSingleNode("//pbp-body").ChildNodes[0]);
                    if (Session["rhywPath"] != null)
                    {
                        objGlobal.SaveXml(temXML.OuterXml, Session["rhywPath"].ToString());
                    }
                    if (untaggedList.Count > 0)
                    {
                        this.btnFinish.Visible = false;
                        LoadPDF();
                    }
                    else
                    {
                        string querySel = "Select BID,UploadedBy from BOOK Where BIdentityNo='" + Request.QueryString["bid"].ToString() + "'";
                        DataSet dsBookInfo = objMyDBClass.GetDataSet(querySel);
                        string bookID = dsBookInfo.Tables[0].Rows[0]["BID"].ToString();
                        string admin = dsBookInfo.Tables[0].Rows[0]["UploadedBy"].ToString();

                        string queryUpdate = "Update ACTIVITY Set Status='Pending Confirmation', CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "' Where UID=" + (Session["objUser"] as UserClass).UserID + " AND BID=" + bookID + " AND Task='TaggingUntagged'";
                        int upRes = objMyDBClass.ExecuteCommand(queryUpdate);
                        if (upRes > 0)
                        {
                            //string dirPath = Server.MapPath("Files/" + Request.QueryString["bid"].ToString().Split(new char[] { '-' })[0] + "/" + Request.QueryString["bid"].ToString() + "/Comparison");
                            string dirPath = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString().Split(new char[] { '-' })[0] + "/" + Request.QueryString["bid"].ToString() + "/Comparison";

                            if (!Directory.Exists(dirPath))
                            {
                                Directory.CreateDirectory(dirPath);
                            }
                            this.btnFinish.Visible = false;
                            Response.Redirect("UserPanel.aspx");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = ex.Message;
            }
        }

        protected void ddLevels_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.pnlLevelSubtype.Visible = true;
        }

        protected void btnHelp_Click(object sender, EventArgs e)
        {
            string bookID = Request.QueryString["bid"] != null ? Request.QueryString["bid"].ToString() : "";
            //GlobalVar.XMLPath = Server.MapPath("Files/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID + "/TaggingUntagged/" + bookID + ".rhyw");
            objGlobal.XMLPath = objMyDBClass.MainDirPhyPath + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID + "/TaggingUntagged/" + bookID + ".rhyw";
            Session["XMLPath"] = objGlobal.XMLPath;
            objGlobal.PBPDocument = new XmlDocument();
            objGlobal.LoadXml();
            Session["PBPDocument"] = objGlobal.PBPDocument;
            XmlNodeList unTaggedElementList = (Session["Nodes"] as XmlNodeList);
            XmlNode unTaggedElement = (unTaggedElementList[int.Parse(Session["untaggedIndex"].ToString()) - 1] as XmlNode);

            string table = "<table cellspacing=\"3px\">";
            if (unTaggedElement.SelectSingleNode("//ln") != null)
            {
                table += "<tr><td align=\"right\" valign=\"top\"><b>Current Node Size : <b></td><td align=\"left\" class=\"bbw\">" + unTaggedElement.SelectSingleNode("//ln").Attributes["fontsize"].Value + "</td></tr>";
            }
            XmlNode bookNode = objGlobal.PBPDocument.SelectSingleNode("//section[@type=\"book\"]//section-title/ln");
            if (bookNode != null)
            {
                table += "<tr><td align=\"right\" valign=\"top\"><b>Book Node Size : <b></td><td align=\"left\" class=\"bbw\">" + bookNode.Attributes["fontsize"].Value + "</td></tr>";
            }
            XmlNode partNode = objGlobal.PBPDocument.SelectSingleNode("//section[@type=\"part\"]//section-title/ln");
            if (partNode != null)
            {
                table += "<tr><td align=\"right\" valign=\"top\"><b>Part Node Size : <b></td><td align=\"left\" class=\"bbw\">" + partNode.Attributes["fontsize"].Value + "</td></tr>";
            }
            XmlNode chpaterNode = objGlobal.PBPDocument.SelectSingleNode("//section[@type=\"chapter\"]//section-title/ln");
            if (chpaterNode != null)
            {
                table += "<tr><td align=\"right\" valign=\"top\"><b>Chapter Node Size : <b></td><td align=\"left\" class=\"bbw\">" + chpaterNode.Attributes["fontsize"].Value + "</td></tr>";
            }
            XmlNode level1Node = objGlobal.PBPDocument.SelectSingleNode("//section[@type=\"level1\"]//section-title/ln");
            if (level1Node != null)
            {
                table += "<tr><td align=\"right\" valign=\"top\"><b>Level 1 Node Size : <b></td><td align=\"left\" class=\"bbw\">" + level1Node.Attributes["fontsize"].Value + "</td></tr>";
            }
            XmlNode level2Node = objGlobal.PBPDocument.SelectSingleNode("//section[@type=\"level2\"]//section-title/ln");
            if (level2Node != null)
            {
                table += "<tr><td align=\"right\" valign=\"top\"><b>Level 2 Node Size : <b></td><td align=\"left\" class=\"bbw\">" + level2Node.Attributes["fontsize"].Value + "</td></tr>";
            }
            XmlNode level3Node = objGlobal.PBPDocument.SelectSingleNode("//section[@type=\"level3\"]//section-title/ln");
            if (level3Node != null)
            {
                table += "<tr><td align=\"right\" valign=\"top\"><b>Level 3 Node Size : <b></td><td align=\"left\" class=\"bbw\">" + level3Node.Attributes["fontsize"].Value + "</td></tr>";
            }
            XmlNode level4Node = objGlobal.PBPDocument.SelectSingleNode("//section[@type=\"level4\"]//section-title/ln");
            if (level4Node != null)
            {
                table += "<tr><td align=\"right\" valign=\"top\"><b>Level 4 Node Size : <b></td><td align=\"left\" class=\"bbw\">" + level4Node.Attributes["fontsize"].Value + "</td></tr>";
            }
            XmlNode preSectionNode = objGlobal.PBPDocument.SelectSingleNode("//pre-section//section-title/ln");
            if (preSectionNode != null)
            {
                table += "<tr><td align=\"right\" valign=\"top\"><b>Pre-Section Node Size : <b></td><td align=\"left\" class=\"bbw\">" + preSectionNode.Attributes["fontsize"].Value + "</td></tr>";
            }
            XmlNode postSectionNode = objGlobal.PBPDocument.SelectSingleNode("//pre-section//section-title/ln");
            if (preSectionNode != null)
            {
                table += "<tr><td align=\"right\" valign=\"top\"><b>Post-Section Node Size : <b></td><td align=\"left\" class=\"bbw\">" + postSectionNode.Attributes["fontsize"].Value + "</td></tr>";
            }
            table += "</table>";
            this.pnlHelpText.InnerHtml = table;
            this.pnlHelp.Visible = true;
            this.UpdatePanel1.Update();
        }
    }
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                         
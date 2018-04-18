using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using iTextSharp.text;
using System.Collections;
using Microsoft.Ajax.Utilities;
using Outsourcing_System.CommonClasses;
using iTextSharp.text.pdf;
using System.Text.RegularExpressions;
using System.Globalization;
using Outsourcing_System.PdfCompare_Classes;

namespace Outsourcing_System
{
    public partial class ComplexBitsMapping : Page
    {
        #region |Fields and Properties|

        private GlobalVar objGlobal = new GlobalVar();
        private GlobalVar objGlobalStrTool = new GlobalVar();
        public MyDBClass objMyDBClass = new MyDBClass();
        private string XSLFilePath;
        private ArrayList FList;
        private ArrayList UFontList;
        private ArrayList AText;
        private ArrayList UTexts;
        private int FontsRemaing;
        private ConversionClass objConversionClass = new ConversionClass();

        public List<string> BoxBgColors
        {
            get
            {
                if (Session["BoxBgColors"] != null)
                    return (List<string>)Session["BoxBgColors"];

                return null;
            }
            set
            {
                Session["BoxBgColors"] = value;
            }
        }

        public List<int> BoxConvertedPagesList
        {
            get
            {
                if (Session["BoxConvertedPagesList"] != null)
                    return (List<int>)Session["BoxConvertedPagesList"];

                return null;
            }
            set
            {
                Session["BoxConvertedPagesList"] = value;
            }
        }

        public List<int> BoxContainingPages
        {
            get
            {
                if (Session["BoxContainingPages"] != null)
                    return (List<int>)Session["BoxContainingPages"];

                return null;
            }
            set
            {
                Session["BoxContainingPages"] = value;
            }
        }

        public List<XmlNode> SelectedXmlParaNodes
        {
            get
            {
                if (Session["SelectedXmlParaNodes"] != null)
                    return (List<XmlNode>)Session["SelectedXmlParaNodes"];

                return null;
            }
            set
            {
                Session["SelectedXmlParaNodes"] = value;
            }
        }

        public List<XmlNode> BeforeComplexBitMappingList
        {
            get
            {
                if (Session["BeforeComplexBitMappingList"] != null)
                    return (List<XmlNode>)Session["BeforeComplexBitMappingList"];

                return null;
            }
            set
            {
                Session["BeforeComplexBitMappingList"] = value;
            }
        }

        public PdfFootNote DetectedFootNotes
        {
            get
            {
                if (Session["DetectedFootNotes"] != null)
                    return (PdfFootNote)Session["DetectedFootNotes"];

                return null;
            }
            set
            {
                Session["DetectedFootNotes"] = value;
            }
        }

        public string CurrentParaTypeForMapping
        {
            get
            {
                if (Session["CurrentParaTypeForMapping"] != null)
                    return (string)Session["CurrentParaTypeForMapping"];

                return null;
            }
            set
            {
                Session["CurrentParaTypeForMapping"] = value;
            }
        }

        public string SelectedTreeViewSection
        {
            get
            {
                if (Session["SelectedTreeViewSection"] != null)
                    return (string)Session["SelectedTreeViewSection"];

                return null;
            }
            set
            {
                Session["SelectedTreeViewSection"] = value;
            }
        }

        public PdfIndentation PdfIndentDetails
        {
            get
            {
                if (Session["PdfIndentDetails"] != null)
                    return (PdfIndentation)Session["PdfIndentDetails"];

                return null;
            }
            set
            {
                Session["PdfIndentDetails"] = value;
            }
        }

        #endregion

        #region |Page Events|

        protected void Page_Load(object sender, EventArgs e)
        {
            Page.MaintainScrollPositionOnPostBack = true;

            //string ddlParaTypeSelIndex = Request["__EVENTARGUMENT"];
            //string val = Request.Params.Get("__EVENTTARGET"); // AddNewEmployeeType
            //if (!string.IsNullOrEmpty(ddlParaTypeSelIndex))
            //{
            //    if (ddlParaTypeSelIndex == "10")
            //    {
            //        CurrentParaTypeForMapping = "endnote";

            //        Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "ShowEndNoteSelDialog()", true);
            //        populateTreeView();
            //        //showEndNoteParas();
            //        //isFootNoteSelected = true;
            //    }
            //}

            //cbxSplitted.Checked = false;
            ////cbxMergeNextLines.Checked = false;
            ////cbxMergePrevLines.Checked = false;
            ////cbxDiffType.Checked = false;

            if (!Page.IsPostBack)
            {
                //BoxContainingPages = null;

                //ShowComplexBitByOrder();
                ////populateDivSparas(ddlParaType.SelectedValue);

                if (BoxContainingPages == null)
                {
                    SetTotaPages();
                    BoxConvertedPagesList = new List<int>();
                    List<int> boxContainingPages = GetBoxContainingPage();
                    //if (boxContainingPages == null || boxContainingPages.Count == 0) return;
                    BoxContainingPages = boxContainingPages;
                }

                if (string.IsNullOrEmpty(CurrentParaTypeForMapping))
                {
                    CurrentParaTypeForMapping = "box";
                    ddlParaType.SelectedValue = CurrentParaTypeForMapping;
                    //GetAbnormalPara();
                    //DisplaySelectedAbnormalPara();

                    SetPdfPageIndentations();
                    ShowComplexBitByOrder();
                }
                else
                {
                    ddlParaType.SelectedValue = CurrentParaTypeForMapping;
                    //GetAbnormalPara();
                    //DisplaySelectedAbnormalPara();

                    ShowComplexBitByOrder();
                }
            }
        }

        protected void btnConvert_Click(object sender, EventArgs e)
        {
            string msgText = "";
            int resultStatusId;
            ResultStatus status = null;

            try
            {
                if (ddlParaType.SelectedValue.Equals("footnote"))
                {
                    DetectedFootNotes = null;
                    convertToFootNote(cbxApplyAll.Checked);
                }
                else if (ddlParaType.SelectedValue.Equals("endnote"))
                {
                    DetectedFootNotes = null;
                    CurrentParaTypeForMapping = ddlParaType.SelectedValue;

                    if (cbxEndOfChap.Checked)
                    {
                        ConvertToChapEndNotes();
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "ShowEndNoteSelDialog()", true);
                        populateTreeView();
                    }

                    //showEndNoteParas();
                    //isFootNoteSelected = true;
                }
                else
                {
                    if (ddlParaType.SelectedValue.Equals("spara"))
                    {
                        DetectedFootNotes = null;

                        if (cbxLParaConv.SelectedValue.Equals("Split"))
                        {   //To do
                            SplitSPara(cbxApplyAll.Checked);
                            cbxLParaConv.ClearSelection();
                        }
                        else if (cbxLParaConv.SelectedValue.Equals("MergePrevious"))
                        {
                            //To do
                            MergeSPara(cbxApplyAll.Checked, "MergePrevious");
                            cbxLParaConv.ClearSelection();
                        }
                        else if (cbxLParaConv.SelectedValue.Equals("MergeNext"))
                        {
                            //To do
                            MergeSPara(cbxApplyAll.Checked, "MergeNext");
                            cbxLParaConv.ClearSelection();
                        }
                        else
                        {
                            status = ConvertToSOrUPara(cbxApplyAll.Checked, "spara");
                        }
                    }
                    else if (ddlParaType.SelectedValue.Equals("upara"))
                    {
                        if (cbxLParaConv.SelectedValue.Equals("Split"))
                        {
                            SplitUPara(cbxApplyAll.Checked);
                            cbxLParaConv.ClearSelection();
                        }
                        else if (cbxLParaConv.SelectedValue.Equals("MergePrevious"))
                        {
                            //To Do
                            //SplitUPara();
                            cbxLParaConv.ClearSelection();
                        }
                        else
                        {
                            status = ConvertToSOrUPara(cbxApplyAll.Checked, "upara");
                        }
                    }
                    else if (ddlParaType.SelectedValue.Equals("level1"))
                    {
                        AddSection("level1", 4);
                    }
                    else if (ddlParaType.SelectedValue.Equals("level2"))
                    {
                        AddSection("level2", 3);
                    }
                    else if (ddlParaType.SelectedValue.Equals("level3"))
                    {
                        AddSection("level3", 2);
                    }
                    else if (ddlParaType.SelectedValue.Equals("level4"))
                    {
                        AddSection("level4", 1);
                    }
                    else if (ddlParaType.SelectedValue.Equals("chapter"))
                    {
                        AddSection("chapter", 5);
                    }
                    else if (ddlParaType.SelectedValue.Equals("npara"))
                    {
                        //convertToNpara(cbxApplyAll.Checked, "");

                        DetectedFootNotes = null;

                        if (cbxLParaConv.SelectedValue.Equals("Split"))
                        {
                            SplitNPara(cbxApplyAll.Checked);
                            cbxLParaConv.ClearSelection();
                        }
                        else if (cbxLParaConv.SelectedValue.Equals("MergePrevious"))
                        {
                            //To Do
                            cbxLParaConv.ClearSelection();
                        }
                        else if (cbxLParaConv.SelectedValue.Equals("MergeNext"))
                        {
                            //To Do
                            cbxLParaConv.ClearSelection();
                        }
                        else
                        {
                            status = convertToNpara(cbxApplyAll.Checked, "npara");
                        }
                    }
                    else if (ddlParaType.SelectedValue.Equals("box"))
                    {
                        convertToBox(cbxApplyAll.Checked);
                    }
                }

                CurrentParaTypeForMapping = ddlParaType.SelectedValue;
                //GetAbnormalPara();
                //DisplaySelectedAbnormalPara();

                if (status != null && !string.IsNullOrEmpty(status.Message))
                {
                    if (status.Status.Equals("success"))
                        ShowMessage(MessageTypes.Success, status.Message);
                    else if (status.Status.Equals("error"))
                        ShowMessage(MessageTypes.Error, status.Message);
                }

                chkStanza.Checked = false;

                ShowComplexBitByOrder();
            }
            catch (Exception ex)
            {

            }
        }

        protected void btnUndoMapping_Click(object sender, EventArgs e)
        {
            objGlobal.XMLPath = Session["XMLPath"].ToString();

            if (!File.Exists(objGlobal.XMLPath)) return;

            objGlobal.LoadXml();

            string bookID = Request.QueryString["bid"] != null ? Request.QueryString["bid"].ToString() : "";
            if (!string.IsNullOrEmpty(bookID))
            {
                objGlobal.PDFPath = objMyDBClass.MainDirPhyPath + "/" + bookID.Split(new char[] { '-' })[0] + "/" +
                                    bookID.Split(new char[] { '-' })[0] + ".pdf";
            }

            StringBuilder sbParaText = new StringBuilder();

            string paraType = string.Empty;

            if (BeforeComplexBitMappingList != null && BeforeComplexBitMappingList.Count > 0)
            {
                //int pageNum = 0;

                XmlNode originalNode = null;

                foreach (XmlNode para in BeforeComplexBitMappingList)
                {
                    if (para.ChildNodes != null && para.ChildNodes.Count > 0 &&
                        para.ChildNodes[0].Attributes != null &&
                        para.ChildNodes[0].Attributes["page"] != null)
                    {
                        List<XmlNode> matchedLinesList = objGlobal.PBPDocument.SelectNodes("//descendant::ln").Cast<XmlNode>().Where(x => x.Attributes != null && x.Attributes["page"] != null && x.Attributes["page"].Value.Equals(para.ChildNodes[0].Attributes["page"].Value)
                       && x.Attributes["coord"].Value.Equals(para.ChildNodes[0].Attributes["coord"].Value)).ToList();

                        if (matchedLinesList != null && matchedLinesList.Count > 0)
                        {
                            foreach (XmlNode matchedLine in matchedLinesList)
                            {
                                if (matchedLine.ParentNode.Name.Equals("upara"))
                                {
                                    if (para.Attributes["abnormalLeft"] != null)
                                    {
                                        XmlAttribute abnLeft = objGlobal.PBPDocument.CreateAttribute("abnormalLeft");
                                        abnLeft.Value = para.Attributes["abnormalLeft"].Value;
                                        matchedLine.ParentNode.Attributes.Append(abnLeft);
                                    }

                                    if (para.Attributes["abnormalRight"] != null)
                                    {
                                        XmlAttribute abnRight = objGlobal.PBPDocument.CreateAttribute("abnormalRight");
                                        abnRight.Value = para.Attributes["abnormalRight"].Value;
                                        matchedLine.ParentNode.Attributes.Append(abnRight);
                                    }

                                    if (para.Attributes["pType"] != null)
                                    {
                                        XmlAttribute pType = objGlobal.PBPDocument.CreateAttribute("pType");
                                        pType.Value = para.Attributes["pType"].Value;
                                        matchedLine.ParentNode.Attributes.Append(pType);
                                    }

                                    originalNode = para;

                                    //paraType = matchedLine.ParentNode.Name;
                                }
                                else if (matchedLine.ParentNode.Name.Equals("para") || matchedLine.ParentNode.Name.Equals("line"))
                                {
                                    originalNode = objGlobal.PBPDocument.CreateElement("upara");

                                    if (para.Attributes != null)
                                    {
                                        for (int i = 0; i < para.Attributes.Count; i++)
                                        {
                                            ((XmlElement)originalNode).SetAttribute(para.Attributes[i].Name, para.Attributes[i].Value);
                                        }
                                    }

                                    originalNode.InnerXml = para.InnerXml;

                                    if (matchedLine.ParentNode != null)
                                    {
                                        matchedLine.ParentNode.ParentNode.ParentNode.InsertBefore(originalNode, matchedLine.ParentNode.ParentNode);
                                        matchedLine.ParentNode.ParentNode.ParentNode.RemoveChild(matchedLine.ParentNode.ParentNode);
                                        objGlobal.SaveXml();
                                    }

                                    //paraType = matchedLine.ParentNode.ParentNode.Name;
                                }
                                else if (matchedLine.ParentNode.Name.Equals("npara"))
                                {
                                    originalNode = objGlobal.PBPDocument.CreateElement("upara");

                                    if (para.Attributes != null)
                                    {
                                        for (int i = 0; i < para.Attributes.Count; i++)
                                        {
                                            ((XmlElement)originalNode).SetAttribute(para.Attributes[i].Name, para.Attributes[i].Value);
                                        }
                                    }

                                    originalNode.InnerXml = para.InnerXml;

                                    if (matchedLine.ParentNode != null)
                                    {
                                        matchedLine.ParentNode.ParentNode.InsertBefore(originalNode, matchedLine.ParentNode);
                                        matchedLine.ParentNode.ParentNode.RemoveChild(matchedLine.ParentNode);
                                        objGlobal.SaveXml();
                                    }

                                    //for (int j = 0; j < paraToConvert.Count; j++)
                                    //{
                                    //    paraToConvert[j].ParentNode.RemoveChild(paraToConvert[j]);
                                    //    objGlobal.SaveXml();
                                    //}

                                    //paraType = matchedLine.ParentNode.Name;
                                }
                            }

                            //if (pageNum == 0)
                            //{
                            //    pageNum = Convert.ToInt32(para.ChildNodes[0].Attributes["page"].Value);

                            //    for (int i = 0; i < matchedLinesList[0].ParentNode.ChildNodes.Count; i++)
                            //    {
                            //        sbParaText.Append(matchedLinesList[0].ParentNode.ChildNodes[i].InnerText.Trim() + "</br>");
                            //    }
                            //}
                        }
                    }
                    //sbParaText.Append("</br>");
                }

                BeforeComplexBitMappingList = null;
                btnUndoMapping.Visible = false;

                //DisplayPreviousSelection(BeforeComplexBitMappingList);

                //XmlNodeList allParas = objGlobal.PBPDocument.SelectNodes("//upara");

                //if (allParas == null || allParas.Count == 0) return;

                List<XmlNode> abnormalParasList = new List<XmlNode>();

                abnormalParasList.Add(originalNode);

                if (originalNode != null)
                {
                    SelectedXmlParaNodes = abnormalParasList;
                    DisplayBoxAndNParas(ddlParaType.SelectedValue, abnormalParasList);
                }


                ////XmlNodeList allParas = objGlobal.PBPDocument.SelectNodes("//upara");

                ////if (allParas == null || allParas.Count == 0) return;

                ////List<XmlNode> abnormalParasList = null;
                ////abnormalParasList = GetSPara(allParas);

                ////DisplayBoxAndNParas(ddlParaType.SelectedValue, abnormalParasList);

                //if (pageNum > 0)
                //    ShowPDF(pageNum);

                //divParaText.InnerHtml = Convert.ToString(sbParaText);

                if (ddlParaType.SelectedValue.Equals("spara"))
                    Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "ShowSParaOptions()", true);
            }
        }

        protected void ddlParaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlParaType.SelectedValue.Equals("endnote"))
            {
                CurrentParaTypeForMapping = ddlParaType.SelectedValue;

                Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "ShowEndNoteSelDialog()", true);
                populateTreeView();
                //showEndNoteParas();
                //isFootNoteSelected = true;
            }
        }

        protected void tvChapters_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                string NodeText = tvChapters.SelectedNode.Text;
                NodeText = NodeText.Replace("<div style=\" background:red; font-weight:bold;\">", "")
                                    .Replace("</div>", "").Replace("<font style='background-color:#ffff42'>", "")
                                    .Replace("</font>", "");

                //if (!string.IsNullOrEmpty(tvChapters.SelectedNode.Text))
                //    SelectedTreeViewSection = tvChapters.SelectedNode.Value;

                //btnSaveEndNotePage_Click(this, null);

                int page = 0;

                if (!string.IsNullOrEmpty(tvChapters.SelectedNode.Value))
                {
                    string pageNum = tvChapters.SelectedNode.Value.Split(new string[] { "page=" }, StringSplitOptions.None).ToList()[1]
                                                            .Split(new string[] { "height=" }, StringSplitOptions.None).ToList()[0].Replace("\"", ""); ;

                    page = Convert.ToInt32(pageNum);
                }

                ConvertToEndNote(tvChapters.SelectedNode.Text, page);
                ShowComplexBitByOrder();
            }
            catch (Exception ex)
            {
            }
        }

        protected void btnFinishCompMapping_Click(object sender, EventArgs e)
        {
            //Should be uncomment while deploying
            CompleteCompBitMapTask();

            if (!string.IsNullOrEmpty(Convert.ToString(Session["BID"])) && !string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
            {
                int inResult = objMyDBClass.CreateTask(Convert.ToString(Session["BID"]), "Unassigned", "MistakeInjection", Convert.ToString(Session["LoginId"]));
            }

            Response.Redirect("AdminPanel.aspx");
        }

        protected void btnSaveEndNotePage_Click(object sender, EventArgs e)
        {
            int pageNumber;

            if (int.TryParse(tbxOtherPage.Text.Trim(), out pageNumber))
            {
                ConvertToEndNoteInSection(pageNumber);
            }
        }

        #endregion

        #region |Public methods|

        #region UPara

        public List<XmlNode> GetUPara(XmlNodeList allParas)
        {
            if (allParas == null || allParas.Count == 0) return null;

            List<XmlNode> abnomalParas = new List<XmlNode>();
            int pageNum = 0;

            foreach (XmlNode abnormalNode in allParas)
            {
                if (abnormalNode != null &&
                    abnormalNode.Attributes != null &&
                    abnormalNode.Attributes["normalX"] != null &&
                    abnormalNode.Attributes["pageNum"] != null &&
                    abnormalNode.ChildNodes.Count > 0 &&
                    !abnormalNode.ChildNodes[0].Name.Equals("#text"))
                {
                    pageNum = Convert.ToInt32(abnormalNode.Attributes["pageNum"].Value);
                    break;
                }
            }

            foreach (XmlNode abnormalNode in allParas)
            {
                if (abnormalNode != null &&
                    abnormalNode.Attributes != null &&
                    abnormalNode.Attributes["normalX"] != null &&
                    abnormalNode.Attributes["pageNum"] != null &&
                    Convert.ToInt32(abnormalNode.Attributes["pageNum"].Value) == pageNum)
                {
                    abnomalParas.Add(abnormalNode);
                }

                if (abnomalParas.Count > 0) break;
            }

            return abnomalParas;
        }

        public ResultStatus ConvertToSOrUPara(bool isApplyAll, string paraType)
        {
            string msgText = "";
            ResultStatus status = null;

            try
            {
                LoadPdfXml();

                List<XmlNode> paraToConvert = new List<XmlNode>();
                List<XmlNode> allMatchingNodes = new List<XmlNode>();
                XmlNode abnormalPara = null;
                double fontSize = 0;
                string fontName = string.Empty;
                double xIndent = 0;
                int pageCount = 0;
                int page = 0;

                if (objGlobal != null && objGlobal.PBPDocument != null)
                {
                    if (DetectedFootNotes != null)
                    {
                        //Remove footnote attributes from tempXml 
                        abnormalPara = RemoveFootNote(DetectedFootNotes);
                        DetectedFootNotes = null;
                        return null;
                    }
                    else if (SelectedXmlParaNodes != null && SelectedXmlParaNodes.Count > 0)
                        abnormalPara = SelectedXmlParaNodes[0];
                    else
                        abnormalPara = objGlobal.PBPDocument.SelectSingleNode("//*[@normalX]");

                    if (abnormalPara != null &&
                        abnormalPara.Attributes != null &&
                        abnormalPara.Attributes["abnormalLeft"] != null &&
                        abnormalPara.Attributes["abnormalRight"] != null &&
                        abnormalPara.Attributes["normalIndX"] != null &&
                        abnormalPara.Attributes["normalTop"] != null &&
                        abnormalPara.Attributes["normalBottom"] != null &&
                        abnormalPara.Attributes["normalY"] != null &&
                        abnormalPara.Attributes["pageType"] != null &&
                        abnormalPara.Attributes["font"] != null &&
                        abnormalPara.Attributes["fontSize"] != null &&
                        abnormalPara.Attributes["pageNum"] != null &&
                        abnormalPara.ChildNodes.Count > 0 &&
                        abnormalPara.ChildNodes[0].Attributes != null &&
                        abnormalPara.ChildNodes[0].Attributes["fontsize"] != null)
                    {
                        page = Convert.ToInt32(abnormalPara.Attributes["pageNum"].Value);

                        if (page == 0) return null;

                        fontSize = Convert.ToDouble(abnormalPara.Attributes["fontSize"].Value);
                        fontName = Convert.ToString(abnormalPara.Attributes["font"].Value);
                        xIndent = Convert.ToDouble(abnormalPara.Attributes["abnormalLeft"].Value);

                        if (isApplyAll)
                        {
                            //allMatchingNodes = objGlobal.PBPDocument.SelectNodes("//upara[@abnormalLeft=" + abnormalPara.Attributes["abnormalLeft"].Value +
                            //                                     " and not(@pType)]").Cast<XmlNode>().ToList();

                            allMatchingNodes =
                                objGlobal.PBPDocument.SelectNodes("//upara[@normalX='" + abnormalPara.Attributes["normalX"].Value +
                                                                  "' and @normalIndX='" + abnormalPara.Attributes["normalIndX"].Value +
                                //"' and @normalTop='" + abnormalPara.Attributes["normalTop"].Value +
                                //"' and @normalBottom='" + abnormalPara.Attributes["normalBottom"].Value +
                                //"' and @normalY='" + abnormalPara.Attributes["normalY"].Value +
                                                                  "' and @font='" + abnormalPara.Attributes["font"].Value +
                                                                  "' and @fontSize='" + abnormalPara.Attributes["fontSize"].Value +
                                                                  "' and @pageType='" + abnormalPara.Attributes["pageType"].Value +
                                                                  "' and not(@pType)]").Cast<XmlNode>().ToList();

                            if (allMatchingNodes.Count > 0)
                            {
                                double valueMargin = 2;

                                foreach (XmlNode uPara in allMatchingNodes)
                                {
                                    if (paraType.Equals("upara"))
                                    {
                                        //var uParaNormalIndXList = uPara.SelectNodes("descendant::ln").Cast<XmlNode>().Where(x => x.Attributes != null &&
                                        //                                  x.Attributes["left"] != null &&
                                        //                                  Math.Abs(Convert.ToDouble(x.Attributes["left"].Value) - Convert.ToDouble(abnormalPara.Attributes["normalIndX"].Value)) < valueMargin).ToList();

                                        //if (uParaNormalIndXList.Count == 1 || uPara.ChildNodes.Count == 1)
                                        //{
                                        //paraToConvert.Add(uPara);
                                        //}

                                        if (abnormalPara.ChildNodes.Count == 1 && uPara.ChildNodes.Count == 1)
                                        {
                                            paraToConvert.Add(uPara);
                                        }
                                        else if (abnormalPara.ChildNodes.Count > 1)
                                        {
                                            paraToConvert.Add(uPara);
                                        }
                                    }
                                    else
                                        if (ddlSparaType.SelectedValue.Equals("other") &&
                                           ddlSparaSubType.SelectedValue.Equals("line") &&
                                           (ddlSparaOrientation.SelectedValue.Equals("right") ||
                                           ddlSparaOrientation.SelectedValue.Equals("left") ||
                                           ddlSparaOrientation.SelectedValue.Equals("center")))
                                        {
                                            var sParaNormalXList = uPara.SelectNodes("descendant::ln").Cast<XmlNode>().Where(x => x.Attributes != null &&
                                                                               x.Attributes["left"] != null &&
                                                                               Math.Abs(Convert.ToDouble(x.Attributes["left"].Value) - Convert.ToDouble(abnormalPara.Attributes["normalX"].Value)) < valueMargin).ToList();

                                            if (sParaNormalXList.Count <= 1)
                                            {
                                                paraToConvert.Add(uPara);
                                            }
                                        }
                                        else
                                        {
                                            paraToConvert.Add(uPara);
                                        }
                                }
                                //paraToConvert = allMatchingNodes;
                            }
                        }
                        else
                        {
                            //To Do
                            //if (SelectedXmlParaNodes != null && SelectedXmlParaNodes.Count > 0 &&
                            //    SelectedXmlParaNodes[0].Attributes["normalX"] != null)
                            //    abnormalPara = SelectedXmlParaNodes[0];
                            //else if (SelectedXmlParaNodes != null && SelectedXmlParaNodes.Count > 0)
                            //{
                            //    //Remove footnote attributes from tempxml and finalxml
                            //    abnormalPara = GetFootNootParas(SelectedXmlParaNodes);
                            //}
                            //else
                            //    abnormalPara = objGlobal.PBPDocument.SelectSingleNode("//*[@normalX][1]");

                            //allMatchingNodes =
                            //   objGlobal.PBPDocument.SelectNodes("//upara[@abnormalLeft=" + abnormalPara.Attributes["abnormalLeft"].Value +
                            //                                     " and not(@pType)]/ln[@page=" + page + "]/..")
                            //       .Cast<XmlNode>()
                            //       .ToList();

                            allMatchingNodes = objGlobal.PBPDocument.SelectNodes("//upara[@normalX='" + abnormalPara.Attributes["normalX"].Value +
                               "' and @normalIndX='" + abnormalPara.Attributes["normalIndX"].Value +
                                //"' and @normalTop='" + abnormalPara.Attributes["normalTop"].Value +
                                //"' and @normalBottom='" + abnormalPara.Attributes["normalBottom"].Value +
                                //"' and @normalY='" + abnormalPara.Attributes["normalY"].Value +
                               "' and @font='" + abnormalPara.Attributes["font"].Value +
                               "' and @fontSize='" + abnormalPara.Attributes["fontSize"].Value +
                               "']/ln[@page='" + page + "']/..").Cast<XmlNode>().ToList();

                            if (allMatchingNodes.Count > 0)
                            {
                                double valueMargin = 2;

                                foreach (XmlNode uPara in allMatchingNodes)
                                {
                                    if (paraType.Equals("upara"))
                                    {
                                        //var uParaNormalIndXList = uPara.SelectNodes("descendant::ln").Cast<XmlNode>().Where(x => x.Attributes != null &&
                                        //                                  x.Attributes["left"] != null &&
                                        //                                  Math.Abs(Convert.ToDouble(x.Attributes["left"].Value) - Convert.ToDouble(abnormalPara.Attributes["normalIndX"].Value)) < valueMargin).ToList();

                                        //if (uParaNormalIndXList.Count == 1 || uPara.ChildNodes.Count == 1)
                                        //{
                                        //paraToConvert.Add(uPara);
                                        //}

                                        if (abnormalPara.ChildNodes.Count == 1 && uPara.ChildNodes.Count == 1)
                                        {
                                            paraToConvert.Add(uPara);
                                        }
                                        else if (abnormalPara.ChildNodes.Count > 1)
                                        {
                                            paraToConvert.Add(uPara);
                                        }
                                    }
                                    else

                                        if (ddlSparaType.SelectedValue.Equals("other") &&
                                           ddlSparaSubType.SelectedValue.Equals("line") &&
                                           (ddlSparaOrientation.SelectedValue.Equals("right") ||
                                           ddlSparaOrientation.SelectedValue.Equals("left") ||
                                           ddlSparaOrientation.SelectedValue.Equals("center")))
                                        {
                                            var sParaNormalXList = uPara.SelectNodes("descendant::ln").Cast<XmlNode>().Where(x => x.Attributes != null &&
                                                                               x.Attributes["left"] != null &&
                                                                               Math.Abs(Convert.ToDouble(x.Attributes["left"].Value) - Convert.ToDouble(abnormalPara.Attributes["normalX"].Value)) < valueMargin).ToList();

                                            if (sParaNormalXList.Count <= 1)
                                            {
                                                paraToConvert.Add(uPara);
                                            }
                                        }
                                        else
                                        {
                                            paraToConvert.Add(uPara);
                                        }
                                }
                                //paraToConvert = allMatchingNodes;
                            }
                        }
                    }
                }

                if (paraToConvert.Count > 0)
                {
                    BeforeComplexBitMappingList = paraToConvert.Select(x => x.Clone()).ToList();

                    if (paraType.Equals("upara"))
                    {
                        foreach (XmlNode para in paraToConvert)
                        {
                            //if (((XmlElement)para).HasAttribute("abnormalLeft"))
                            //    para.Attributes.RemoveNamedItem("abnormalLeft");

                            //if (((XmlElement)para).HasAttribute("abnormalRight"))
                            //    para.Attributes.RemoveNamedItem("abnormalRight");

                            //if (((XmlElement)para).HasAttribute("pType"))
                            //    para.Attributes.RemoveNamedItem("pType");

                            //if (((XmlElement)para).HasAttribute("normalX"))
                            //    para.Attributes.RemoveNamedItem("normalX");

                            //if (((XmlElement)para).HasAttribute("normalIndX"))
                            //    para.Attributes.RemoveNamedItem("normalIndX");

                            //if (((XmlElement)para).HasAttribute("pageNum"))
                            //    para.Attributes.RemoveNamedItem("pageNum");

                            //if (((XmlElement)para).HasAttribute("pageType"))
                            //    para.Attributes.RemoveNamedItem("pageType");

                            //if (((XmlElement)para).HasAttribute("conversion-Operations"))
                            //    para.Attributes.RemoveNamedItem("conversion-Operations");

                            if (((XmlElement)para).HasAttribute("normalX"))
                                para.Attributes.RemoveNamedItem("normalX");

                            if (((XmlElement)para).HasAttribute("normalIndX"))
                                para.Attributes.RemoveNamedItem("normalIndX");

                            if (((XmlElement)para).HasAttribute("normalTop"))
                                para.Attributes.RemoveNamedItem("normalTop");

                            if (((XmlElement)para).HasAttribute("normalBottom"))
                                para.Attributes.RemoveNamedItem("normalBottom");

                            if (((XmlElement)para).HasAttribute("normalY"))
                                para.Attributes.RemoveNamedItem("normalY");

                            if (((XmlElement)para).HasAttribute("pageNum"))
                                para.Attributes.RemoveNamedItem("pageNum");

                            if (((XmlElement)para).HasAttribute("pageType"))
                                para.Attributes.RemoveNamedItem("pageType");

                            if (((XmlElement)para).HasAttribute("font"))
                                para.Attributes.RemoveNamedItem("font");

                            if (((XmlElement)para).HasAttribute("fontSize"))
                                para.Attributes.RemoveNamedItem("fontSize");

                            if (((XmlElement)para).HasAttribute("abnormalLeft"))
                                para.Attributes.RemoveNamedItem("abnormalLeft");

                            if (((XmlElement)para).HasAttribute("abnormalRight"))
                                para.Attributes.RemoveNamedItem("abnormalRight");

                            if (((XmlElement)para).HasAttribute("pType"))
                                para.Attributes.RemoveNamedItem("pType");

                            if (((XmlElement)para).HasAttribute("conversion-Operations"))
                                para.Attributes.RemoveNamedItem("conversion-Operations");
                        }
                    }
                    else if (paraType.Equals("spara"))
                    {
                        CreateSParaXml(paraToConvert, abnormalPara);
                    }

                    objGlobal.SaveXml();

                    pageCount = paraToConvert.Where(x => x.ChildNodes.Count > 0 &&
                                        x.ChildNodes[0].Attributes != null &&
                                        x.ChildNodes[0].Attributes["page"] != null).Select(x => Convert.ToInt32(x.ChildNodes[0].Attributes["page"].Value))
                                         .Distinct().Count();

                    if (paraToConvert.Count == 1)
                    {
                        msgText = paraToConvert.Count + " para in page " + page + " is successfully converted into " +
                                  paraType +
                                  " with indentation = " + xIndent + ", FontName = " + fontName + " and fontsize = " +
                                  fontSize;
                    }
                    else if (paraToConvert.Count > 1 && pageCount == 1)
                    {
                        msgText = paraToConvert.Count + " paras in page " + page + " is successfully converted into " +
                                  paraType +
                                  " with indentation = " + xIndent + ", FontName = " + fontName + " and fontsize = " +
                                  fontSize;
                    }
                    else if (paraToConvert.Count > 1 && pageCount > 1)
                    {
                        msgText = paraToConvert.Count + " paras in " + pageCount + " pages are successfully converted into " +
                                  paraType +
                                  " with indentation = " + xIndent + ", FontName = " + fontName + " and fontsize = " +
                                  fontSize;
                    }

                    status = new ResultStatus();
                    status.Status = "success";
                    status.Message = msgText;
                }
                else
                    divParaText.InnerText = "";

                return status;
            }
            catch (Exception)
            {
                status = new ResultStatus();
                status.Status = "error";
                status.Message = "Some error has occured while converting upara.";
                return status;
            }
        }

        public void SplitUPara(bool isApplyAll)
        {
            XmlNode abnormalPara = null;

            if (SelectedXmlParaNodes != null && SelectedXmlParaNodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(SelectedXmlParaNodes[0].InnerText))
                {
                    LoadPdfXml();

                    int page = Convert.ToInt32(SelectedXmlParaNodes[0].ChildNodes[0].Attributes["page"].Value);
                    double llx = Convert.ToDouble(SelectedXmlParaNodes[0].ChildNodes[0].Attributes["left"].Value);
                    double lly = Convert.ToDouble(SelectedXmlParaNodes[0].ChildNodes[0].Attributes["top"].Value);

                    XmlNode matchedUpara =
                        objGlobal.PBPDocument.SelectSingleNode("//upara/ln[@page='" + page + "' and @left= '" + llx +
                                                               "' and @top='" + lly + "']/..");

                    if (matchedUpara != null)
                    {
                        abnormalPara = matchedUpara;
                    }
                }
                //abnormalPara = SelectedXmlParaNodes[0];
            }
            else
                abnormalPara = objGlobal.PBPDocument.SelectSingleNode("//*[@abnormalLeft][1]");

            Indentation paraIndValues = GetParaCoords(abnormalPara);

            if (objGlobal != null &&
                objGlobal.PBPDocument != null &&
                abnormalPara != null &&
                paraIndValues != null &&
                abnormalPara.Attributes != null &&
                abnormalPara.Attributes["normalX"] != null &&
                abnormalPara.Attributes["normalIndX"] != null &&
                abnormalPara.Attributes["normalTop"] != null &&
                abnormalPara.Attributes["normalBottom"] != null &&
                abnormalPara.Attributes["normalY"] != null &&
                abnormalPara.Attributes["pageNum"] != null &&
                abnormalPara.Attributes["pageType"] != null &&
                abnormalPara.Attributes["font"] != null &&
                abnormalPara.Attributes["fontSize"] != null &&
                abnormalPara.ChildNodes.Count > 0)
            {
                if (isApplyAll)
                {
                    List<XmlNode> allMatchingNodes =
                        objGlobal.PBPDocument.SelectNodes("//upara[@pageType='" + abnormalPara.Attributes["pageType"].Value +
                                                          "' and @normalX='" + abnormalPara.Attributes["normalX"].Value +
                                                          "' and @normalIndX='" + abnormalPara.Attributes["normalIndX"].Value +
                        //"' and @normalTop='" + abnormalPara.Attributes["normalTop"].Value +
                        //"' and @normalBottom='" + abnormalPara.Attributes["normalBottom"].Value +
                        //"' and @normalY='" + abnormalPara.Attributes["normalY"].Value +
                                                          "' and @font='" + abnormalPara.Attributes["font"].Value +
                                                          "' and @fontSize='" + abnormalPara.Attributes["fontSize"].Value + "']")
                            .Cast<XmlNode>()
                            .ToList();
                    if (allMatchingNodes.Count > 0)
                    {
                        foreach (XmlNode upara in allMatchingNodes)
                        {
                            var splittedParas = UParaSplitting(upara, objGlobal.PBPDocument, paraIndValues);
                        }
                    }
                }
                else
                {
                    var splittedParas = UParaSplitting(abnormalPara, objGlobal.PBPDocument, paraIndValues);
                }

                objGlobal.SaveXml();
            }
        }

        public List<XmlNode> UParaSplitting(XmlNode para, XmlDocument xmlDoc, Indentation pageIndent)
        {
            double paraX1Val = 0;
            double paraFontSize = 0;
            double paraEndXValPrev = 0;
            double paraEndXVal = 0;

            bool isNormalPara = true;

            double normalFontSize = pageIndent.FontSize;
            string normalFontName = pageIndent.FontName;
            double normalIndentX = pageIndent.NormalIndentX;
            double normalX = pageIndent.NormalX;
            double normalEndX = pageIndent.Endx;
            int page = pageIndent.Page;

            double xIndentXValMargin = 3;
            double endXValMargin = 3;

            List<XmlNode> newParaList = new List<XmlNode>();

            if (para != null && para.SelectNodes("//ln").Count > 0)
            {
                if (IsContainsIndentX(para, normalX, normalIndentX, normalFontSize, ""))
                {
                    XmlElement xparaElem = SubSplitting(xmlDoc, para);
                    //Splitting
                    XmlNodeList lineList = para.SelectNodes("descendant::ln");
                    if (lineList.Count > 0)
                    {
                        newParaList.Add(xparaElem);
                        for (int m = 0; m < lineList.Count; m++)
                        {
                            double x1Val = Math.Floor(double.Parse(lineList[m].Attributes["left"].Value.ToString()));
                            double fontSize = double.Parse(lineList[m].Attributes["fontsize"].Value.ToString());
                            string fontName = lineList[m].Attributes["font"].Value;

                            //if (x1Val == NormalIndentX)
                            if (Math.Abs(x1Val - normalIndentX) < xIndentXValMargin && normalFontSize.Equals(fontSize) && IsSameFontName(fontName, normalFontName))
                            {
                                xparaElem = SubSplitting(xmlDoc, para);
                                xparaElem.AppendChild(lineList[m]);
                                newParaList.Add(xparaElem);
                            }
                            //else if (x1Val == NormalX)
                            else if (Math.Abs(x1Val - normalX) < xIndentXValMargin && normalFontSize.Equals(fontSize) && IsSameFontName(fontName, normalFontName))
                            {
                                if (xparaElem != null)
                                {
                                    xparaElem.AppendChild(lineList[m]);
                                }
                            }
                            //else if (x1Val < NormalIndentX && x1Val != NormalX)
                            else if (x1Val + xIndentXValMargin < normalIndentX && normalFontSize.Equals(fontSize) && IsSameFontName(fontName, normalFontName))
                            {
                                XmlAttribute mergError = xmlDoc.CreateAttribute("MergeError");
                                mergError.Value = "1";
                                lineList[m].Attributes.Append(mergError);
                                if (xparaElem != null)
                                {
                                    xparaElem.AppendChild(lineList[m]);
                                }
                            }
                            //else if (x1Val > NormalIndentX)//) && (x1Val - NormalIndentX > 1) original
                            else if (x1Val + xIndentXValMargin > normalIndentX && normalFontSize.Equals(fontSize) && IsSameFontName(fontName, normalFontName))
                            {
                                xparaElem = SubSplitting(xmlDoc, para);
                                if (lineList[m].Attributes["MergeError"] == null)
                                {
                                    XmlAttribute splitError = xmlDoc.CreateAttribute("SplitError");
                                    splitError.Value = "1";
                                    lineList[m].Attributes.Append(splitError);
                                }
                                xparaElem.AppendChild(lineList[m]);
                                newParaList.Add(xparaElem);
                            }
                            else
                            {
                                if (xparaElem != null)
                                {
                                    xparaElem.AppendChild(lineList[m]);
                                }
                            }
                        }
                        if (newParaList.Count > 0)
                        {
                            foreach (XmlElement elem in newParaList)
                            {
                                if (!string.IsNullOrEmpty(elem.InnerText))
                                    para.ParentNode.InsertBefore(elem, para);
                            }
                            para.ParentNode.RemoveChild(para);
                        }
                    }
                }
            }
            return newParaList;
        }

        #endregion

        #region SPara

        public List<XmlNode> GetSPara(XmlNodeList allParas)
        {
            if (allParas == null || allParas.Count == 0) return null;

            List<XmlNode> abnomalParas = new List<XmlNode>();
            int pageNum = 0;

            foreach (XmlNode abnormalNode in allParas)
            {
                if (abnormalNode != null &&
                    abnormalNode.Attributes != null &&
                    abnormalNode.Attributes["normalX"] != null &&
                    abnormalNode.Attributes["pageNum"] != null &&
                    abnormalNode.ChildNodes.Count > 0 &&
                    !abnormalNode.ChildNodes[0].Name.Equals("#text") &&
                    abnormalNode.Attributes["pType"] == null)
                {
                    pageNum = Convert.ToInt32(abnormalNode.Attributes["pageNum"].Value);
                    break;
                }
            }

            foreach (XmlNode abnormalNode in allParas)
            {
                if (abnormalNode != null &&
                    abnormalNode.Attributes != null &&
                    abnormalNode.Attributes["normalX"] != null &&
                    abnormalNode.Attributes["pageNum"] != null &&
                    abnormalNode.Attributes["pType"] == null &&
                    Convert.ToInt32(abnormalNode.Attributes["pageNum"].Value) == pageNum)
                {
                    abnomalParas.Add(abnormalNode);
                }

                if (abnomalParas.Count > 0) break;
            }

            return abnomalParas;
        }

        public void SplitSPara(bool isApplyAll)
        {
            XmlNode abnormalPara = null;

            if (objGlobal.PBPDocument == null)
                LoadPdfXml();

            if (SelectedXmlParaNodes != null && SelectedXmlParaNodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(SelectedXmlParaNodes[0].InnerText))
                {
                    //LoadPdfXml();

                    int page = Convert.ToInt32(SelectedXmlParaNodes[0].ChildNodes[0].Attributes["page"].Value);
                    double llx = Convert.ToDouble(SelectedXmlParaNodes[0].ChildNodes[0].Attributes["left"].Value);
                    double lly = Convert.ToDouble(SelectedXmlParaNodes[0].ChildNodes[0].Attributes["top"].Value);

                    XmlNode matchedUpara =
                        objGlobal.PBPDocument.SelectSingleNode("//upara/ln[@page='" + page + "' and @left= '" + llx +
                                                               "' and @top='" + lly + "']/..");

                    if (matchedUpara != null)
                    {
                        abnormalPara = matchedUpara;
                    }
                    //abnormalPara = SelectedXmlParaNodes[0];
                }
            }
            else
                abnormalPara = objGlobal.PBPDocument.SelectSingleNode("//*[@abnormalLeft][1]");

            //Indentation paraIndValues = GetParaCoords(abnormalPara);

            Indentation paraIndValues = null;

            paraIndValues = GetParaCoords(abnormalPara);

            if (paraIndValues == null || paraIndValues.NormalX == 0 || paraIndValues.NormalIndentX == 0)
            {
                if ((ddlSparaType.SelectedValue.Equals("other") &&
                     ddlSparaSubType.SelectedValue.Equals("line") &&
                     ddlSparaOrientation.SelectedValue.Equals("left")) ||
                    (ddlSparaType.SelectedValue.Equals("other") &&
                     ddlSparaSubType.SelectedValue.Equals("line") &&
                     ddlSparaOrientation.SelectedValue.Equals("right")) ||
                    (ddlSparaType.SelectedValue.Equals("other") &&
                     ddlSparaSubType.SelectedValue.Equals("line") &&
                     ddlSparaOrientation.SelectedValue.Equals("center")))
                {
                    paraIndValues = GetParaAttrFromLine(objGlobal.PBPDocument, abnormalPara, "otherline");
                }
                else
                {
                    paraIndValues = GetParaAttrFromLine(objGlobal.PBPDocument, abnormalPara, "quote");
                }
            }

            if (objGlobal != null &&
                objGlobal.PBPDocument != null &&
                abnormalPara != null &&
                paraIndValues != null &&
                abnormalPara.Attributes != null &&
                abnormalPara.Attributes["normalX"] != null &&
                abnormalPara.Attributes["normalIndX"] != null &&
                abnormalPara.Attributes["normalTop"] != null &&
                abnormalPara.Attributes["normalBottom"] != null &&
                abnormalPara.Attributes["normalY"] != null &&
                abnormalPara.Attributes["pageNum"] != null &&
                abnormalPara.Attributes["pageType"] != null &&
                abnormalPara.Attributes["font"] != null &&
                abnormalPara.Attributes["fontSize"] != null &&
                abnormalPara.ChildNodes.Count > 0)
            {
                if (isApplyAll)
                {
                    List<XmlNode> allMatchingNodes =
                        objGlobal.PBPDocument.SelectNodes("//upara[@pageType='" + abnormalPara.Attributes["pageType"].Value +
                                                          "' and @normalX='" + abnormalPara.Attributes["normalX"].Value +
                                                          "' and @normalIndX='" + abnormalPara.Attributes["normalIndX"].Value +
                        //"' and @normalTop='" + abnormalPara.Attributes["normalTop"].Value +
                        //"' and @normalBottom='" + abnormalPara.Attributes["normalBottom"].Value +
                        //"' and @normalY='" + abnormalPara.Attributes["normalY"].Value +
                                                          "' and @font='" + abnormalPara.Attributes["font"].Value +
                                                          "' and @fontSize='" + abnormalPara.Attributes["fontSize"].Value + "']")
                            .Cast<XmlNode>()
                            .ToList();
                    if (allMatchingNodes.Count > 0)
                    {
                        if ((ddlSparaType.SelectedValue.Equals("letter") && ddlSparaSubType.SelectedValue.Equals("para")) ||
                        (ddlSparaType.SelectedValue.Equals("quotation") && ddlSparaSubType.SelectedValue.Equals("para")) ||
                        (ddlSparaType.SelectedValue.Equals("quotation") && ddlSparaSubType.SelectedValue.Equals("line")) ||
                        (ddlSparaType.SelectedValue.Equals("poem") && ddlSparaSubType.SelectedValue.Equals("line")))
                        {
                            foreach (XmlNode upara in allMatchingNodes)
                            {
                                var splittedParas = SParaSplitting(upara, objGlobal.PBPDocument, paraIndValues, "quote");
                            }
                        }
                        else if ((ddlSparaType.SelectedValue.Equals("other") &&
                                  ddlSparaSubType.SelectedValue.Equals("line") &&
                                  ddlSparaOrientation.SelectedValue.Equals("left")) ||
                                  (ddlSparaType.SelectedValue.Equals("other") &&
                                  ddlSparaSubType.SelectedValue.Equals("line") &&
                                  ddlSparaOrientation.SelectedValue.Equals("right")) ||
                                  (ddlSparaType.SelectedValue.Equals("other") &&
                                  ddlSparaSubType.SelectedValue.Equals("line") &&
                                  ddlSparaOrientation.SelectedValue.Equals("center")))
                        {
                            foreach (XmlNode upara in allMatchingNodes)
                            {
                                var splittedParas = SParaSplitting(upara, objGlobal.PBPDocument, paraIndValues, "otherline");
                            }
                        }
                    }
                }
                else
                {
                    if ((ddlSparaType.SelectedValue.Equals("letter") && ddlSparaSubType.SelectedValue.Equals("para")) ||
                         (ddlSparaType.SelectedValue.Equals("quotation") && ddlSparaSubType.SelectedValue.Equals("para")) ||
                         (ddlSparaType.SelectedValue.Equals("quotation") && ddlSparaSubType.SelectedValue.Equals("line")) ||
                         (ddlSparaType.SelectedValue.Equals("poem") && ddlSparaSubType.SelectedValue.Equals("line")))
                    {
                        var splittedParas = SParaSplitting(abnormalPara, objGlobal.PBPDocument, paraIndValues, "quote");
                    }
                    else if ((ddlSparaType.SelectedValue.Equals("other") &&
                              ddlSparaSubType.SelectedValue.Equals("line") &&
                              ddlSparaOrientation.SelectedValue.Equals("left")) ||
                              (ddlSparaType.SelectedValue.Equals("other") &&
                              ddlSparaSubType.SelectedValue.Equals("line") &&
                              ddlSparaOrientation.SelectedValue.Equals("right")) ||
                              (ddlSparaType.SelectedValue.Equals("other") &&
                              ddlSparaSubType.SelectedValue.Equals("line") &&
                              ddlSparaOrientation.SelectedValue.Equals("center")))
                    {
                        var splittedParas = SParaSplitting(abnormalPara, objGlobal.PBPDocument, paraIndValues, "otherline");
                    }
                }
                objGlobal.SaveXml();
            }
        }

        public void MergeSPara(bool isApplyAll, string mergingType)
        {
            XmlNode abnormalPara = null;

            if (objGlobal.PBPDocument == null)
            {
                LoadPdfXml();
            }

            if (SelectedXmlParaNodes != null && SelectedXmlParaNodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(SelectedXmlParaNodes[0].InnerText))
                {
                    int page = Convert.ToInt32(SelectedXmlParaNodes[0].ChildNodes[0].Attributes["page"].Value);
                    double llx = Convert.ToDouble(SelectedXmlParaNodes[0].ChildNodes[0].Attributes["left"].Value);
                    double lly = Convert.ToDouble(SelectedXmlParaNodes[0].ChildNodes[0].Attributes["top"].Value);

                    XmlNode matchedUpara =
                        objGlobal.PBPDocument.SelectSingleNode("//upara/ln[@page='" + page + "' and @left= '" + llx +
                                                               "' and @top='" + lly + "']/..");
                    if (matchedUpara != null)
                    {
                        abnormalPara = matchedUpara;
                    }
                    //abnormalPara = SelectedXmlParaNodes[0];
                }
            }
            else
                abnormalPara = objGlobal.PBPDocument.SelectSingleNode("//*[@abnormalLeft][1]");

            Indentation paraIndValues = GetParaCoords(abnormalPara);

            if (paraIndValues == null || paraIndValues.NormalX == 0 || paraIndValues.NormalIndentX == 0)
            {
                if ((ddlSparaType.SelectedValue.Equals("other") &&
                     ddlSparaSubType.SelectedValue.Equals("line") &&
                     ddlSparaOrientation.SelectedValue.Equals("left")) ||
                    (ddlSparaType.SelectedValue.Equals("other") &&
                     ddlSparaSubType.SelectedValue.Equals("line") &&
                     ddlSparaOrientation.SelectedValue.Equals("right")) ||
                    (ddlSparaType.SelectedValue.Equals("other") &&
                     ddlSparaSubType.SelectedValue.Equals("line") &&
                     ddlSparaOrientation.SelectedValue.Equals("center")))
                {
                    paraIndValues = GetParaAttrFromLine(objGlobal.PBPDocument, abnormalPara, "otherline");
                }
                else
                {
                    paraIndValues = GetParaAttrFromLine(objGlobal.PBPDocument, abnormalPara, "quote");
                }
            }

            if (objGlobal != null &&
                objGlobal.PBPDocument != null &&
                abnormalPara != null &&
                paraIndValues != null &&
                abnormalPara.Attributes != null &&
                abnormalPara.Attributes["normalX"] != null &&
                abnormalPara.Attributes["normalIndX"] != null &&
                abnormalPara.Attributes["normalTop"] != null &&
                abnormalPara.Attributes["normalBottom"] != null &&
                abnormalPara.Attributes["normalY"] != null &&
                abnormalPara.Attributes["pageNum"] != null &&
                abnormalPara.Attributes["pageType"] != null &&
                abnormalPara.Attributes["font"] != null &&
                abnormalPara.Attributes["fontSize"] != null &&
                abnormalPara.ChildNodes.Count > 0)
            {
                if (isApplyAll)
                {
                   List<XmlNode> allMatchingNodes = objGlobal.PBPDocument.SelectNodes("//upara[@pageType='" + abnormalPara.Attributes["pageType"].Value +
                                                          "' and @normalX='" + abnormalPara.Attributes["normalX"].Value +
                                                          "' and @normalIndX='" + abnormalPara.Attributes["normalIndX"].Value +
                        //"' and @normalTop='" + abnormalPara.Attributes["normalTop"].Value +
                        //"' and @normalBottom='" + abnormalPara.Attributes["normalBottom"].Value +
                        //"' and @normalY='" + abnormalPara.Attributes["normalY"].Value +
                                                          "' and @font='" + abnormalPara.Attributes["font"].Value +
                                                          "' and @fontSize='" + abnormalPara.Attributes["fontSize"].Value + "']")
                            .Cast<XmlNode>()
                            .ToList();

                    if (allMatchingNodes.Count > 0)
                    {
                        foreach (XmlNode upara in allMatchingNodes)
                        {
                            if ((ddlSparaType.SelectedValue.Equals("quotation") && ddlSparaSubType.SelectedValue.Equals("para")) ||
                                (ddlSparaType.SelectedValue.Equals("quotation") && ddlSparaSubType.SelectedValue.Equals("line")) ||
                                (ddlSparaType.SelectedValue.Equals("letter") && ddlSparaSubType.SelectedValue.Equals("para")) ||
                                (ddlSparaType.SelectedValue.Equals("poem") && ddlSparaSubType.SelectedValue.Equals("line")))
                            {
                                if (mergingType.Equals("MergePrevious"))
                                {
                                    MergingAllSParaTypes(upara, objGlobal.PBPDocument, paraIndValues, mergingType, "quote");
                                    Indentation mergedParaIndValues = GetSParaCoords(objGlobal.PBPDocument, upara);

                                    if (mergedParaIndValues != null)
                                    {
                                        var splittedParas1 = SParaSplitting(upara, objGlobal.PBPDocument, mergedParaIndValues, "quote");
                                    }
                                }
                                else
                                {
                                    MergingAllSParaTypes(upara, objGlobal.PBPDocument, paraIndValues, mergingType, "quote");
                                    Indentation mergedParaIndValues = GetSParaCoords(objGlobal.PBPDocument, upara);

                                    if (mergedParaIndValues != null)
                                    {
                                        var splittedParas1 = SParaSplitting(upara, objGlobal.PBPDocument, mergedParaIndValues, "quote");
                                    }
                                }
                            }
                            else if ((ddlSparaType.SelectedValue.Equals("other") &&
                                      ddlSparaSubType.SelectedValue.Equals("line") &&
                                      ddlSparaOrientation.SelectedValue.Equals("left")) ||
                                      (ddlSparaType.SelectedValue.Equals("other") &&
                                      ddlSparaSubType.SelectedValue.Equals("line") &&
                                      ddlSparaOrientation.SelectedValue.Equals("right")) ||
                                      (ddlSparaType.SelectedValue.Equals("other") &&
                                      ddlSparaSubType.SelectedValue.Equals("line") &&
                                      ddlSparaOrientation.SelectedValue.Equals("center")))
                            {
                                if (mergingType.Equals("MergePrevious"))
                                {
                                    MergingAllSParaTypes(upara, objGlobal.PBPDocument, paraIndValues, mergingType, "otherline");
                                    Indentation mergedParaIndValues = GetSParaCoords(objGlobal.PBPDocument, upara);

                                    if (mergedParaIndValues != null)
                                    {
                                        var splittedParas1 = SParaSplitting(upara, objGlobal.PBPDocument, mergedParaIndValues, "otherline");
                                    }
                                }
                                else
                                {
                                    MergingAllSParaTypes(upara, objGlobal.PBPDocument, paraIndValues, mergingType, "otherline");
                                    Indentation mergedParaIndValues = GetSParaCoords(objGlobal.PBPDocument, upara);

                                    if (mergedParaIndValues != null)
                                    {
                                        var splittedParas1 = SParaSplitting(upara, objGlobal.PBPDocument,
                                            mergedParaIndValues, "otherline");
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    if ((ddlSparaType.SelectedValue.Equals("quotation") && ddlSparaSubType.SelectedValue.Equals("para")) ||
                     (ddlSparaType.SelectedValue.Equals("quotation") && ddlSparaSubType.SelectedValue.Equals("line")) ||
                     (ddlSparaType.SelectedValue.Equals("letter") && ddlSparaSubType.SelectedValue.Equals("para")) ||
                     (ddlSparaType.SelectedValue.Equals("poem") && ddlSparaSubType.SelectedValue.Equals("line")))
                    {
                        if (mergingType.Equals("MergePrevious"))
                        {
                            MergingAllSParaTypes(abnormalPara, objGlobal.PBPDocument, paraIndValues, mergingType, "quote");
                            Indentation mergedParaIndValues = GetSParaCoords(objGlobal.PBPDocument, abnormalPara);

                            if (mergedParaIndValues != null)
                            {
                                var splittedParas1 = SParaSplitting(abnormalPara, objGlobal.PBPDocument, mergedParaIndValues, "quote");
                            }
                        }
                        else
                        {
                            MergingAllSParaTypes(abnormalPara, objGlobal.PBPDocument, paraIndValues, mergingType, "quote");
                            Indentation mergedParaIndValues = GetSParaCoords(objGlobal.PBPDocument, abnormalPara);

                            if (mergedParaIndValues != null)
                            {
                                var splittedParas1 = SParaSplitting(abnormalPara, objGlobal.PBPDocument, mergedParaIndValues, "quote");
                            }
                        }
                    }
                    else if ((ddlSparaType.SelectedValue.Equals("other") &&
                              ddlSparaSubType.SelectedValue.Equals("line") &&
                              ddlSparaOrientation.SelectedValue.Equals("left")) ||
                              (ddlSparaType.SelectedValue.Equals("other") &&
                              ddlSparaSubType.SelectedValue.Equals("line") &&
                              ddlSparaOrientation.SelectedValue.Equals("right")) ||
                              (ddlSparaType.SelectedValue.Equals("other") &&
                              ddlSparaSubType.SelectedValue.Equals("line") &&
                              ddlSparaOrientation.SelectedValue.Equals("center")))
                    {
                        if (mergingType.Equals("MergePrevious"))
                        {
                            MergingAllSParaTypes(abnormalPara, objGlobal.PBPDocument, paraIndValues, mergingType, "otherline");
                            Indentation mergedParaIndValues = GetSParaCoords(objGlobal.PBPDocument, abnormalPara);

                            if (mergedParaIndValues != null)
                            {
                                var splittedParas1 = SParaSplitting(abnormalPara, objGlobal.PBPDocument, mergedParaIndValues, "otherline");
                            }
                        }
                        else
                        {
                            MergingAllSParaTypes(abnormalPara, objGlobal.PBPDocument, paraIndValues, mergingType, "otherline");
                            Indentation mergedParaIndValues = GetSParaCoords(objGlobal.PBPDocument, abnormalPara);

                            if (mergedParaIndValues != null)
                            {
                                var splittedParas1 = SParaSplitting(abnormalPara, objGlobal.PBPDocument,
                                    mergedParaIndValues, "otherline");
                            }
                        }
                    }
                }
                objGlobal.SaveXml();
            }
        }

        public XmlNode MergingAllSParaTypes(XmlNode abnormalPara, XmlDocument xmlDoc, Indentation pageIndent, string mergingType, string paraType)
        {
            Indentation paraToMergeIndValues = null;
            //double y = 0;

            double y = GetPageNormalY(xmlDoc, abnormalPara);

            List<XmlNode> splittedParaList = new List<XmlNode>();
            //splittedParaList.Add(abnormalPara);

            double xIndentXValMargin = 2;
            double normalYValMargin = 2;
            double trancYVal = 0;

            double normalFontSize = pageIndent.FontSize;
            double normalIndentX = 0;
            double normalX = 0;

            double leftVal = 0;
            //double normalEndX = pageIndent.Endx;

            if (mergingType.Equals("MergePrevious"))
            {
                if (abnormalPara.PreviousSibling == null ||
                    abnormalPara.PreviousSibling.ChildNodes.Count == 0 ||
                    (abnormalPara.PreviousSibling.ChildNodes.Count == 1 && abnormalPara.PreviousSibling.ChildNodes[0].Name.Equals("break"))) return null;

                //XmlNode firstLine = abnormalPara.PreviousSibling.SelectSingleNode("descendant::ln/@left");

                List<double> prevParaAllLeftVal = abnormalPara.PreviousSibling.SelectNodes("descendant::ln/@left").Cast<XmlNode>()
                                                              .Select(x => Convert.ToDouble(x.Value)).ToList();

                var allLeftVal = prevParaAllLeftVal.GroupBy(x => x).Select(group => new { value = @group.Key, Count = @group.Count() })
            .OrderByDescending(x => x.value).ToList();

                if (allLeftVal.Count > 1)
                {
                    leftVal = allLeftVal[0].value;
                }

                if (leftVal > 0)
                {
                    //double leftVal = Convert.ToDouble(firstLine.Value);

                    if (paraType.Equals("otherline"))
                    {
                        if (Math.Abs(leftVal - pageIndent.NormalX) < 2 || Math.Abs(leftVal - pageIndent.NormalIndentX) < 2)
                        {
                            normalX = leftVal;
                        }
                        else if (leftVal < pageIndent.NormalX || leftVal < pageIndent.NormalIndentX)
                        {
                            normalX = leftVal;
                        }
                    }
                    else if (paraType.Equals("quote"))
                    {
                        if (leftVal >= pageIndent.NormalX || leftVal >= pageIndent.NormalIndentX)
                        {
                            normalX = leftVal;
                        }
                    }

                    splittedParaList.Add(abnormalPara.PreviousSibling);
                }
            }
            else if (mergingType.Equals("MergeNext"))
            {
                if (abnormalPara.NextSibling == null ||
                  abnormalPara.NextSibling.ChildNodes.Count == 0 ||
                  (abnormalPara.NextSibling.ChildNodes.Count == 1 && abnormalPara.NextSibling.ChildNodes[0].Name.Equals("break"))) return null;

                XmlNode firstLine = abnormalPara.NextSibling.SelectSingleNode("descendant::ln/@left");
                if (firstLine != null)
                {
                    leftVal = Convert.ToDouble(firstLine.Value);

                    if (paraType.Equals("otherline"))
                    {
                        if (leftVal > pageIndent.NormalX || leftVal > pageIndent.NormalIndentX)
                        {
                            normalX = leftVal;
                        }
                    }
                    else if (paraType.Equals("quote"))
                    {
                        if (leftVal >= pageIndent.NormalX || leftVal >= pageIndent.NormalIndentX)
                        {
                            normalX = leftVal;
                        }
                        else if (leftVal <= pageIndent.NormalIndentX)
                        {
                            normalX = leftVal;
                        }
                    }

                    splittedParaList.Add(abnormalPara.NextSibling);
                }
            }

            List<XmlNode> paraList = new List<XmlNode>();

            if (splittedParaList.Count > 0)
            {
                foreach (XmlNode node in splittedParaList)
                {
                    if (!string.IsNullOrEmpty(node.InnerText))
                    {
                        if (mergingType.Equals("MergeNext"))
                        {
                            XmlNode pageNode = abnormalPara.NextSibling.SelectSingleNode("descendant::ln/@page");
                            XmlNode leftNode = abnormalPara.NextSibling.SelectSingleNode("descendant::ln/@left");
                            XmlNode topNode = abnormalPara.NextSibling.SelectSingleNode("descendant::ln/@top");

                            if (pageNode != null && leftNode != null && topNode != null)
                            {
                                XmlNode matchedUpara = null;

                                if (abnormalPara.NextSibling.Name.Equals("spara"))
                                {
                                    matchedUpara =
                                        objGlobal.PBPDocument.SelectSingleNode("descendant::ln[@page='" + pageNode.Value +
                                                                               "' and @left= '" +
                                                                               leftNode.Value + "' and @top='" +
                                                                               topNode.Value + "']/..");
                                }
                                else if (abnormalPara.NextSibling.Name.Equals("upara"))
                                {
                                    matchedUpara =
                                        objGlobal.PBPDocument.SelectSingleNode("//upara/ln[@page='" + pageNode.Value +
                                                                               "' and @left= '" +
                                                                               leftNode.Value + "' and @top='" +
                                                                               topNode.Value + "']/..");
                                }

                                if (matchedUpara != null)
                                {
                                    if (abnormalPara.NextSibling.Name.Equals("upara"))
                                    {
                                        paraList.Add(abnormalPara);
                                        paraList.Add(matchedUpara);
                                    }
                                    if (abnormalPara.NextSibling.Name.Equals("spara"))
                                    {
                                        paraList.Add(abnormalPara);
                                        paraList.Add(matchedUpara.ParentNode);
                                    }
                                }

                            }
                        }
                        else if (mergingType.Equals("MergePrevious"))
                        {
                            XmlNode pageNode = abnormalPara.PreviousSibling.SelectSingleNode("descendant::ln/@page");
                            XmlNode leftNode = abnormalPara.PreviousSibling.SelectSingleNode("descendant::ln/@left");
                            XmlNode topNode = abnormalPara.PreviousSibling.SelectSingleNode("descendant::ln/@top");

                            if (pageNode != null && leftNode != null && topNode != null)
                            {
                                XmlNode matchedUpara = null;

                                if (abnormalPara.PreviousSibling.Name.Equals("spara"))
                                {
                                    matchedUpara =
                                        objGlobal.PBPDocument.SelectSingleNode("descendant::ln[@page='" + pageNode.Value +
                                                                               "' and @left= '" +
                                                                               leftNode.Value + "' and @top='" +
                                                                               topNode.Value + "']/..");
                                }
                                else if (abnormalPara.PreviousSibling.Name.Equals("upara"))
                                {
                                    matchedUpara =
                                        objGlobal.PBPDocument.SelectSingleNode("//upara/ln[@page='" + pageNode.Value +
                                                                               "' and @left= '" +
                                                                               leftNode.Value + "' and @top='" +
                                                                               topNode.Value + "']/..");
                                }

                                if (matchedUpara != null)
                                {
                                    if (abnormalPara.PreviousSibling.Name.Equals("upara"))
                                    {
                                        paraList.Add(matchedUpara);
                                        paraList.Add(abnormalPara);
                                    }
                                    if (abnormalPara.PreviousSibling.Name.Equals("spara"))
                                    {
                                        paraList.Add(matchedUpara.ParentNode);
                                        paraList.Add(abnormalPara);
                                    }
                                }
                            }
                        }
                    }
                }
            }

        jump:

            for (int r = 1; r < paraList.Count; r++)
            {
                if (!string.IsNullOrEmpty(paraList[r].InnerText))
                {
                    XmlNode firstLineNode = paraList[r].SelectSingleNode("descendant::ln");
                    if (firstLineNode != null)
                    {
                        double x1Val = Math.Floor(double.Parse(firstLineNode.Attributes["left"].Value.ToString()));
                        double fontSize = double.Parse(firstLineNode.Attributes["fontsize"].Value.ToString());
                        string font = firstLineNode.Attributes["font"].Value;

                        double y1Val = double.Parse(firstLineNode.Attributes["top"].Value.ToString());
                        double yVal = 0, diffy = 0;
                        for (int j = r - 1; j >= 0; j--)
                        {
                            XmlNodeList lineList = paraList[j].SelectNodes("descendant::ln");
                            if (lineList.Count == 0)
                            {
                                continue;
                            }
                            XmlNode lastNode = lineList[lineList.Count - 1];
                            yVal = double.Parse(lastNode.Attributes["top"].Value.ToString());
                            if (yVal != 0)
                            {
                                diffy = Math.Abs(yVal - y1Val);
                                break;
                            }
                        }

                        if (x1Val < normalX + xIndentXValMargin && normalFontSize.Equals(fontSize))
                        {
                            //if (Math.Floor(diffy) > Math.Floor(y))

                            trancYVal = ((y - Math.Truncate(y) > 0.90) ? Math.Ceiling(y) : Math.Floor(y));

                            if (Math.Floor(diffy) > (trancYVal + normalYValMargin))
                            {
                                if (firstLineNode.Attributes["SplitError"] == null)
                                {
                                    XmlAttribute splitError = xmlDoc.CreateAttribute("SplitError");
                                    splitError.Value = "1";
                                    firstLineNode.Attributes.Append(splitError);
                                }
                            }
                            else
                            {
                                if (paraList[r - 1].SelectSingleNode("descendant::ln") != null)
                                {
                                    double prevParafontSize = Convert.ToDouble(paraList[r - 1].SelectSingleNode("descendant::ln").Attributes["fontsize"].Value);
                                    string prevParafont = Convert.ToString(paraList[r - 1].SelectSingleNode("descendant::ln").Attributes["font"].Value);

                                    if (IsSameFontName(font, prevParafont) && prevParafontSize.Equals(fontSize))
                                    {
                                        foreach (XmlNode lnNode in paraList[r].SelectNodes("descendant::ln"))
                                        {
                                            paraList[r - 1].AppendChild(lnNode);
                                        }
                                        paraList[r].ParentNode.RemoveChild(paraList[r]);
                                        goto jump;
                                    }
                                }
                            }
                        }
                        //else if (x1Val != NormalIndentX)
                        else if (x1Val != normalIndentX + xIndentXValMargin && normalFontSize.Equals(fontSize))
                        {
                            if (firstLineNode.Attributes["MergeError"] == null &&
                                firstLineNode.Attributes["SplitError"] == null)
                            {
                                XmlAttribute splitError = xmlDoc.CreateAttribute("SplitError");
                                splitError.Value = "1";
                                firstLineNode.Attributes.Append(splitError);
                            }
                        }
                    }
                }
            }//end for loop

            return null;
        }

        public List<XmlNode> SParaSplitting(XmlNode para, XmlDocument xmlDoc, Indentation pageIndent, string paraType)
        {
            double paraX1Val = 0;
            double paraFontSize = 0;
            double paraEndXValPrev = 0;
            double paraEndXVal = 0;

            bool isNormalPara = true;

            //double normalFontSize = pageIndent.FontSize;
            //string normalFontName = pageIndent.FontName;
            //double normalIndentX = pageIndent.NormalIndentX;
            //double normalX = pageIndent.NormalX;
            //double normalEndX = pageIndent.Endx;
            //int page = pageIndent.Page;

            double normalFontSize = pageIndent.FontSize;
            string normalFontName = pageIndent.FontName;
            double normalIndentX = 0;
            double normalX = 0;
            double normalEndX = pageIndent.Endx;
            int page = pageIndent.Page;

            if (paraType.Equals("quote"))
            {
                normalX = pageIndent.NormalX;
                normalIndentX = pageIndent.NormalIndentX;
            }
            else if (paraType.Equals("otherline"))
            {
                normalX = pageIndent.NormalIndentX;
                normalIndentX = pageIndent.NormalX;
            }

            double xIndentXValMargin = 3;
            double endXValMargin = 3;
            bool isIndXFound = false;

            List<XmlNode> newParaList = new List<XmlNode>();

            if (para != null && para.SelectNodes("//ln").Count > 0)
            {
                if (IsContainsIndentX(para, normalX, normalIndentX, normalFontSize, paraType))
                {
                    XmlElement xparaElem = SubSplittingSPara(xmlDoc, para, paraType);

                    //Splitting
                    XmlNodeList lineList = para.SelectNodes("descendant::ln");
                    if (lineList.Count > 0)
                    {
                        newParaList.Add(xparaElem);
                        for (int m = 0; m < lineList.Count; m++)
                        {
                            double x1Val = Math.Floor(double.Parse(lineList[m].Attributes["left"].Value.ToString()));
                            double fontSize = double.Parse(lineList[m].Attributes["fontsize"].Value.ToString());
                            string fontName = lineList[m].Attributes["font"].Value;

                            if (Math.Abs(x1Val - normalIndentX) < xIndentXValMargin &&
                                normalFontSize.Equals(fontSize) &&
                                IsSameFontName(fontName, normalFontName) &&
                                ((paraType.Equals("quote") && !isIndXFound) || (paraType.Equals("otherline"))))
                            {
                                xparaElem = SubSplittingSPara(xmlDoc, para, paraType);
                                xparaElem.AppendChild(lineList[m]);
                                newParaList.Add(xparaElem);
                                isIndXFound = true;
                            }

                            else if (Math.Abs(x1Val - normalX) < xIndentXValMargin && normalFontSize.Equals(fontSize) && IsSameFontName(fontName, normalFontName))
                            {
                                if (xparaElem != null)
                                {
                                    xparaElem.AppendChild(lineList[m]);
                                }
                            }

                            else if (x1Val + xIndentXValMargin < normalIndentX && normalFontSize.Equals(fontSize) && IsSameFontName(fontName, normalFontName))
                            {
                                XmlAttribute mergError = xmlDoc.CreateAttribute("MergeError");
                                mergError.Value = "1";
                                lineList[m].Attributes.Append(mergError);
                                if (xparaElem != null)
                                {
                                    xparaElem.AppendChild(lineList[m]);
                                }
                            }
                            else if (x1Val + xIndentXValMargin > normalIndentX && normalFontSize.Equals(fontSize) && IsSameFontName(fontName, normalFontName) &&
                                    ((paraType.Equals("quote") && !isIndXFound) || (paraType.Equals("otherline"))))
                            {
                                xparaElem = SubSplittingSPara(xmlDoc, para, paraType);
                                if (lineList[m].Attributes["MergeError"] == null)
                                {
                                    XmlAttribute splitError = xmlDoc.CreateAttribute("SplitError");
                                    splitError.Value = "1";
                                    lineList[m].Attributes.Append(splitError);
                                }
                                xparaElem.AppendChild(lineList[m]);
                                newParaList.Add(xparaElem);
                                isIndXFound = true;
                            }
                            else
                            {
                                if (xparaElem != null)
                                {
                                    xparaElem.AppendChild(lineList[m]);
                                }
                            }
                        }//end for loop
                        if (newParaList.Count > 0)
                        {
                            int count = 0;

                            foreach (XmlElement elem in newParaList)
                            {
                                if (!string.IsNullOrEmpty(elem.InnerText))
                                {
                                    if (count == 0)
                                    {
                                        count++;
                                        XmlNode sPara = CreateSParaXmlAfterSplit(elem);
                                        para.ParentNode.InsertBefore(sPara, para);
                                    }
                                    else
                                    {
                                        para.ParentNode.InsertBefore(elem, para);
                                    }
                                }
                            }
                            para.ParentNode.RemoveChild(para);
                        }
                    }
                }
            }

            return newParaList;
        }

        public XmlElement SubSplittingSPara(XmlDocument xmlDoc, XmlNode para, string paraType)
        {
            XmlElement xparaElem = xmlDoc.CreateElement(para.Name);

            if (para != null &&
                para.ChildNodes.Count > 0 &&
                para.ChildNodes[0].Attributes != null &&
                para.ChildNodes[0].Attributes["left"] != null &&
                para.ChildNodes[0].Attributes["font"] != null &&
                para.ChildNodes[0].Attributes["fontsize"] != null)
            {

                Indentation paraIndValues = GetParaAttrFromLine(xmlDoc, para, paraType);

                if (paraIndValues != null && para.ParentNode != null)
                {
                    XmlAttribute abnormalLeft = xmlDoc.CreateAttribute("abnormalLeft");
                    XmlAttribute abnormalRight = xmlDoc.CreateAttribute("abnormalRight");

                    XmlAttribute normalLeft = xmlDoc.CreateAttribute("normalX");
                    XmlAttribute normalIndentLeft = xmlDoc.CreateAttribute("normalIndX");
                    XmlAttribute normalTop = xmlDoc.CreateAttribute("normalTop");
                    XmlAttribute normalBottom = xmlDoc.CreateAttribute("normalBottom");

                    XmlAttribute normalY = xmlDoc.CreateAttribute("normalY");

                    XmlAttribute pageNum = xmlDoc.CreateAttribute("pageNum");
                    XmlAttribute pageType = xmlDoc.CreateAttribute("pageType");

                    XmlAttribute font = xmlDoc.CreateAttribute("font");
                    XmlAttribute fontSize = xmlDoc.CreateAttribute("fontSize");

                    var pageNumberList = para.SelectNodes("ln")
                        .Cast<XmlNode>()
                        .Select(node => Convert.ToInt32(node.Attributes["page"].Value)).Distinct().ToList();
                    if (pageNumberList.Count > 0)
                    {
                        pageNum.Value = Convert.ToString(pageNumberList[0]);
                        if (pageNumberList[0] % 2 == 0)
                        {
                            pageType.Value = "even";
                        }
                        else
                        {
                            pageType.Value = "odd";
                        }

                        abnormalLeft.Value = Convert.ToString(paraIndValues.NormalX);
                        abnormalRight.Value = Convert.ToString(paraIndValues.Endx);

                        normalLeft.Value = Convert.ToString(paraIndValues.NormalX);
                        normalIndentLeft.Value = Convert.ToString(paraIndValues.NormalIndentX);
                        normalTop.Value = Convert.ToString(paraIndValues.PrevParaY);
                        normalBottom.Value = Convert.ToString(paraIndValues.NextParaY);
                        normalY.Value = Convert.ToString(paraIndValues.NormalY);
                    }
                    xparaElem.Attributes.Append(abnormalLeft);
                    xparaElem.Attributes.Append(abnormalRight);
                    xparaElem.Attributes.Append(normalLeft);
                    xparaElem.Attributes.Append(normalIndentLeft);
                    xparaElem.Attributes.Append(normalTop);
                    xparaElem.Attributes.Append(normalBottom);
                    xparaElem.Attributes.Append(normalY);

                    xparaElem.Attributes.Append(pageNum);
                    xparaElem.Attributes.Append(pageType);

                    XmlNode firstLineNode = para.SelectSingleNode("ln");
                    if (firstLineNode != null &&
                        firstLineNode.Attributes != null &&
                        firstLineNode.Attributes["font"] != null &&
                        firstLineNode.Attributes["fontsize"] != null)
                    {
                        font.Value = firstLineNode.Attributes["font"].Value;
                        fontSize.Value = firstLineNode.Attributes["fontsize"].Value;
                        xparaElem.Attributes.Append(font);
                        xparaElem.Attributes.Append(fontSize);
                    }
                }
            }
            return xparaElem;
        }

        public Indentation GetParaAttrFromLine(XmlDocument mainXML, XmlNode line, string paraType)
        {
            Indentation pageIndent = null;

            if (mainXML == null || string.IsNullOrEmpty(mainXML.InnerText) || line == null || line.ParentNode == null)
                return null;

            XmlNode uParaNode = null;

            //if (paraType.Equals("otherline"))
            //{
            uParaNode = line;
            //}
            //else
            //{
            //    uParaNode = line.ParentNode;
            //}
            //XmlNode uParaNode = line.ParentNode;

            List<double> leftValueList = uParaNode.SelectNodes("descendant::ln/@left").Cast<XmlNode>().Select(x => Convert.ToDouble(x.Value)).ToList();

            if (leftValueList.Count > 0)
            {
                double valueMargin = 10;
                //double indentXvalueMargin = 22;
                double normalleft = 0;
                double normalleftnormalX = 0;
                double normalX = 0;
                double normalIndentX = 0;

                var allLeftValues = leftValueList.GroupBy(x => x)
                        .Select(group => new { value = @group.Key, Count = @group.Count() })
                        .OrderByDescending(x => x.Count).ToList();

                if (allLeftValues.Count > 0)
                {
                    if (allLeftValues.Count == 1)
                    {
                        if (paraType.Equals("otherline") && allLeftValues[0].Count > 1)
                        {
                            normalX = allLeftValues[0].value;
                            normalleft = allLeftValues[0].value;
                        }
                        else
                        {
                            normalIndentX = allLeftValues[0].value;
                            normalleft = allLeftValues[0].value;
                        }
                    }
                    else
                    {
                        for (int i = 0; i < allLeftValues.Count; i++)
                        {
                            if (i == 0)
                            {
                                normalX = allLeftValues[i].value;
                                normalleft = allLeftValues[i].value;
                            }
                            //if (i + 1 < allLeftValues.Count && (Math.Abs(normalX - allLeftValues[i + 1].value) > valueMargin) &&
                            //    (Math.Abs(normalX - allLeftValues[i + 1].value) < indentXvalueMargin))
                            //{
                            //    normalIndentX = allLeftValues[i + 1].value;
                            //    break;
                            //}

                            if (i + 1 < allLeftValues.Count && (Math.Abs(normalX - allLeftValues[i + 1].value) > valueMargin))
                            {
                                normalIndentX = allLeftValues[i + 1].value;
                                break;
                            }
                        }
                    }
                }

                double temp = 0;

                //if (paraType.Equals("otherline"))
                //{
                //    if (normalX < normalIndentX && normalX != 0 && normalIndentX != 0)
                //    {
                //        temp = normalX;
                //        normalX = normalIndentX;
                //        normalIndentX = temp;
                //    }
                //}
                //else
                //{
                if (normalX > normalIndentX && normalX != 0 && normalIndentX != 0)
                {
                    temp = normalX;
                    normalX = normalIndentX;
                    normalIndentX = temp;
                }
                //}

                double normalY = 0;

                List<double> llyValueList = uParaNode.SelectNodes("descendant::ln/@top").Cast<XmlNode>().Select(x => Convert.ToDouble(x.Value)).ToList();

                if (llyValueList.Count > 0)
                {
                    var verticalDiffereances =
                           llyValueList.Take(llyValueList.Count - 1).Select((v, i) => llyValueList[i + 1] - v)
                               .Where(x => Math.Abs(x) > 0).ToList();

                    if (verticalDiffereances.Count > 0)
                    {
                        var allDifferenceByValue = verticalDiffereances.GroupBy(x => Math.Floor(Math.Abs(x)))
                            .Select(g => new { Value = g.Key, Count = g.Count() })
                            .OrderByDescending(x => x.Count).ToList();

                        if (allDifferenceByValue.Count > 0)
                            normalY = Math.Floor(allDifferenceByValue[0].Value);
                    }
                }
                //normalX = Math.Floor(normalX);
                //normalIndentX = Math.Floor(normalIndentX);

                var fontName = uParaNode.SelectSingleNode("descendant::ln[@left='" + normalleft + "']/@font");
                var fontSize = uParaNode.SelectSingleNode("descendant::ln[@left='" + normalleft + "']/@fontsize");
                var pageNum = uParaNode.SelectSingleNode("descendant::ln[@left='" + normalleft + "']/@page");

                pageIndent = new Indentation();

                if (fontName != null && fontSize != null)
                {
                    pageIndent.FontName = fontName.Value;
                    pageIndent.FontSize = Convert.ToDouble(fontSize.Value);

                    //pageIndent.NormalX = normalX;
                    pageIndent.NormalX = Convert.ToString(normalX).Contains(".")
                          ? Convert.ToDouble(Convert.ToString(normalX).Remove(Convert.ToString(normalX).IndexOf('.'),
                          Convert.ToString(normalX).Length - Convert.ToString(normalX).IndexOf('.'))) : Convert.ToDouble(normalX);

                    //pageIndent.NormalIndentX = normalIndentX;
                    pageIndent.NormalIndentX = Convert.ToString(normalIndentX).Contains(".")
                          ? Convert.ToDouble(Convert.ToString(normalIndentX).Remove(Convert.ToString(normalIndentX).IndexOf('.'),
                          Convert.ToString(normalIndentX).Length - Convert.ToString(normalIndentX).IndexOf('.'))) : Convert.ToDouble(normalIndentX);
                    pageIndent.Page = Convert.ToInt32(pageNum.Value);

                    //pageIndent.NormalY = normalY;

                    pageIndent.NormalY = Convert.ToString(normalY).Contains(".")
                          ? Convert.ToDouble(Convert.ToString(normalY).Remove(Convert.ToString(normalY).IndexOf('.'),
                          Convert.ToString(normalY).Length - Convert.ToString(normalY).IndexOf('.'))) : Convert.ToDouble(normalY);
                }

                XmlNode prevParaNode = line.ParentNode.PreviousSibling;
                if (prevParaNode != null && prevParaNode.ChildNodes.Count > 0)
                {
                    List<double> llyPrevParaList =
                        prevParaNode.SelectNodes("descendant::ln/@top")
                            .Cast<XmlNode>()
                            .Select(x => Convert.ToDouble(x.Value))
                            .ToList();
                    if (llyPrevParaList.Count > 0)
                    {
                        //pageIndent.PrevParaY = Math.Abs(Convert.ToDouble(llyPrevParaList[llyPrevParaList.Count - 1]) - Convert.ToDouble(llyValueList[0]));

                        double prevParaDist = Math.Abs(Convert.ToDouble(llyPrevParaList[llyPrevParaList.Count - 1]) - Convert.ToDouble(llyValueList[0]));

                        pageIndent.PrevParaY = Convert.ToString(prevParaDist).Contains(".")
                          ? Convert.ToDouble(Convert.ToString(prevParaDist).Remove(Convert.ToString(prevParaDist).IndexOf('.'),
                          Convert.ToString(prevParaDist).Length - Convert.ToString(prevParaDist).IndexOf('.'))) : Convert.ToDouble(prevParaDist);
                    }
                }
                else
                    pageIndent.PrevParaY = 0;

                XmlNode nextParaNode = line.ParentNode.NextSibling;
                if (nextParaNode != null && nextParaNode.ChildNodes.Count > 0)
                {
                    List<double> llyNextParaList =
                        nextParaNode.SelectNodes("descendant::ln/@top")
                            .Cast<XmlNode>()
                            .Select(x => Convert.ToDouble(x.Value))
                            .ToList();
                    if (llyNextParaList.Count > 0)
                    {
                        //pageIndent.NextParaY = Math.Abs(Convert.ToDouble(llyValueList[llyValueList.Count - 1]) - Convert.ToDouble(llyNextParaList[0]));

                        double nextParaDist = Math.Abs(Convert.ToDouble(llyValueList[llyValueList.Count - 1]) - Convert.ToDouble(llyNextParaList[0]));

                        pageIndent.NextParaY = Convert.ToString(nextParaDist).Contains(".")
                          ? Convert.ToDouble(Convert.ToString(nextParaDist).Remove(Convert.ToString(nextParaDist).IndexOf('.'),
                          Convert.ToString(nextParaDist).Length - Convert.ToString(nextParaDist).IndexOf('.'))) : Convert.ToDouble(nextParaDist);
                    }
                }
                else
                    pageIndent.NextParaY = 0;
            }//end leftValueList

            List<string> paraLeft = uParaNode.SelectNodes("descendant::ln/@coord").Cast<XmlNode>().Select(x => x.Value.Split(':')[0]).ToList();
            List<string> paraRight = uParaNode.SelectNodes("descendant::ln/@coord").Cast<XmlNode>().Select(x => x.Value.Split(':')[2]).ToList();

            //var leftVal = paraLeft.GroupBy(x => x)
            //            .Select(g => new { Value = g.Key, Count = g.Count() })
            //            .OrderBy(x => x.Count).ToList();

            var RightVal = paraRight.GroupBy(x => x)
                .Select(g => new { Value = g.Key, Count = g.Count() })
                .OrderBy(x => x.Count).ToList();

            //string LeftValue = leftVal[0].Value.Contains(".")
            //    ? leftVal[0].Value
            //        .Remove(leftVal[0].Value.IndexOf('.'), leftVal[0].Value.Length - leftVal[0].Value
            //            .IndexOf('.'))
            //    : leftVal[0].Value;

            string RightValue = RightVal[0].Value.Contains(".")
                ? RightVal[0].Value
                    .Remove(RightVal[0].Value.IndexOf('.'), RightVal[0].Value.Length - RightVal[0].Value
                        .IndexOf('.'))
                : RightVal[0].Value;

            pageIndent.Endx = Convert.ToDouble(RightValue);

            return pageIndent;
        }

        public Indentation GetParaCoords(XmlNode abnormalPara)
        {
            Indentation pageIndent = null;

            if (abnormalPara != null &&
                abnormalPara.Attributes != null &&
                abnormalPara.Attributes["normalX"] != null &&
                abnormalPara.Attributes["normalIndX"] != null &&
                abnormalPara.Attributes["normalTop"] != null &&
                abnormalPara.Attributes["normalBottom"] != null &&
                abnormalPara.Attributes["pageNum"] != null &&
                abnormalPara.Attributes["pageType"] != null &&
                abnormalPara.Attributes["font"] != null &&
                abnormalPara.Attributes["fontSize"] != null)
            {

                pageIndent = new Indentation();

                pageIndent.NormalX = Convert.ToDouble(abnormalPara.Attributes["normalX"].Value);
                pageIndent.NormalIndentX = Convert.ToDouble(abnormalPara.Attributes["normalIndX"].Value);
                pageIndent.PrevParaY = Convert.ToDouble(abnormalPara.Attributes["normalTop"].Value);
                pageIndent.NextParaY = Convert.ToDouble(abnormalPara.Attributes["normalBottom"].Value);

                pageIndent.NormalY = Convert.ToDouble(abnormalPara.Attributes["normalY"].Value);

                pageIndent.FontName = abnormalPara.Attributes["font"].Value;
                pageIndent.FontSize = Convert.ToDouble(abnormalPara.Attributes["fontSize"].Value);

                pageIndent.Page = Convert.ToInt32(abnormalPara.Attributes["pageNum"].Value);
                pageIndent.PageType = abnormalPara.Attributes["pageType"].Value;

                pageIndent.Endx = Convert.ToDouble(abnormalPara.Attributes["abnormalRight"].Value);
            }

            return pageIndent;
        }

        public Indentation GetSParaCoords(XmlDocument mainXML, XmlNode line)
        {
            Indentation pageIndent = null;

            //if (line.ParentNode == null)
            //{
            //    int page = Convert.ToInt32(line.ChildNodes[0].Attributes["page"].Value);
            //    double llx = Convert.ToDouble(line.ChildNodes[0].Attributes["left"].Value);
            //    double lly = Convert.ToDouble(line.ChildNodes[0].Attributes["top"].Value);

            //    XmlNode matchedUpara = objGlobal.PBPDocument.SelectSingleNode("//upara/ln[@page='" + page + "' and @left= '" + llx + "' and @top='" + lly + "']/..");
            //    if (matchedUpara != null)
            //    {
            //        line = matchedUpara;
            //    }
            //}

            if (mainXML == null || string.IsNullOrEmpty(mainXML.InnerText) || line == null || line.ChildNodes.Count > 0 || line.ParentNode == null)
                return null;

            XmlNode uParaNode = line;

            List<double> leftValueList = uParaNode.SelectNodes("descendant::ln/@left").Cast<XmlNode>().Select(x => Convert.ToDouble(x.Value)).ToList();

            if (leftValueList.Count > 0)
            {
                double valueMargin = 10;
                //double indentXvalueMargin = 22;

                double normalleft = 0;
                double normalX = 0;
                double normalIndentX = 0;

                var allLeftValues = leftValueList.GroupBy(x => x)
                        .Select(group => new { value = @group.Key, Count = @group.Count() })
                        .OrderByDescending(x => x.Count).ToList();

                if (allLeftValues.Count > 0)
                {
                    if (allLeftValues.Count == 1)
                    {
                        normalIndentX = allLeftValues[0].value;
                        normalleft = allLeftValues[0].value;
                    }
                    else
                    {
                        for (int i = 0; i < allLeftValues.Count; i++)
                        {
                            if (i == 0)
                            {
                                normalX = allLeftValues[i].value;
                                normalleft = allLeftValues[0].value;
                            }
                            //if (i + 1 < allLeftValues.Count && (Math.Abs(normalX - allLeftValues[i + 1].value) > valueMargin) &&
                            //    (Math.Abs(normalX - allLeftValues[i + 1].value) < indentXvalueMargin))
                            //{
                            //    normalIndentX = allLeftValues[i + 1].value;
                            //    break;
                            //}

                            if (i + 1 < allLeftValues.Count && (Math.Abs(normalX - allLeftValues[i + 1].value) > valueMargin))
                            {
                                normalIndentX = allLeftValues[i + 1].value;
                                break;
                            }
                        }
                    }
                }

                double temp = 0;

                if (normalX > normalIndentX && normalX != 0 && normalIndentX != 0)
                {
                    temp = normalX;
                    normalX = normalIndentX;
                    normalIndentX = temp;
                }

                double normalY = 0;

                List<double> llyValueList = uParaNode.SelectNodes("descendant::ln/@top").Cast<XmlNode>().Select(x => Convert.ToDouble(x.Value)).ToList();

                if (llyValueList.Count > 0)
                {
                    var verticalDiffereances =
                           llyValueList.Take(llyValueList.Count - 1).Select((v, i) => llyValueList[i + 1] - v)
                               .Where(x => Math.Abs(x) > 0).ToList();

                    if (verticalDiffereances.Count > 0)
                    {
                        var allDifferenceByValue = verticalDiffereances.GroupBy(x => Math.Floor(Math.Abs(x)))
                            .Select(g => new { Value = g.Key, Count = g.Count() })
                            .OrderByDescending(x => x.Count).ToList();

                        if (allDifferenceByValue.Count > 0)
                            normalY = Math.Floor(allDifferenceByValue[0].Value);
                    }
                }
                //normalX = Math.Floor(normalX);
                //normalIndentX = Math.Floor(normalIndentX);

                var fontName = uParaNode.SelectSingleNode("descendant::ln[@left='" + normalleft + "']/@font");
                var fontSize = uParaNode.SelectSingleNode("descendant::ln[@left='" + normalleft + "']/@fontsize");
                var pageNum = uParaNode.SelectSingleNode("descendant::ln[@left='" + normalleft + "']/@page");

                pageIndent = new Indentation();

                if (fontName != null && fontSize != null)
                {
                    pageIndent.FontName = fontName.Value;
                    pageIndent.FontSize = Convert.ToDouble(fontSize.Value);

                    //pageIndent.NormalX = normalX;
                    pageIndent.NormalX = Convert.ToString(normalX).Contains(".")
                          ? Convert.ToDouble(Convert.ToString(normalX).Remove(Convert.ToString(normalX).IndexOf('.'),
                          Convert.ToString(normalX).Length - Convert.ToString(normalX).IndexOf('.'))) : Convert.ToDouble(normalX);

                    //pageIndent.NormalIndentX = normalIndentX;
                    pageIndent.NormalIndentX = Convert.ToString(normalIndentX).Contains(".")
                          ? Convert.ToDouble(Convert.ToString(normalIndentX).Remove(Convert.ToString(normalIndentX).IndexOf('.'),
                          Convert.ToString(normalIndentX).Length - Convert.ToString(normalIndentX).IndexOf('.'))) : Convert.ToDouble(normalIndentX);
                    pageIndent.Page = Convert.ToInt32(pageNum.Value);

                    //pageIndent.NormalY = normalY;

                    pageIndent.NormalY = Convert.ToString(normalY).Contains(".")
                          ? Convert.ToDouble(Convert.ToString(normalY).Remove(Convert.ToString(normalY).IndexOf('.'),
                          Convert.ToString(normalY).Length - Convert.ToString(normalY).IndexOf('.'))) : Convert.ToDouble(normalY);
                }

                XmlNode prevParaNode = line.ParentNode.PreviousSibling;
                if (prevParaNode != null && prevParaNode.ChildNodes.Count > 0)
                {
                    List<double> llyPrevParaList =
                        prevParaNode.SelectNodes("descendant::ln/@top")
                            .Cast<XmlNode>()
                            .Select(x => Convert.ToDouble(x.Value))
                            .ToList();
                    if (llyPrevParaList.Count > 0)
                    {
                        //pageIndent.PrevParaY = Math.Abs(Convert.ToDouble(llyPrevParaList[llyPrevParaList.Count - 1]) - Convert.ToDouble(llyValueList[0]));

                        double prevParaDist = Math.Abs(Convert.ToDouble(llyPrevParaList[llyPrevParaList.Count - 1]) - Convert.ToDouble(llyValueList[0]));

                        pageIndent.PrevParaY = Convert.ToString(prevParaDist).Contains(".")
                          ? Convert.ToDouble(Convert.ToString(prevParaDist).Remove(Convert.ToString(prevParaDist).IndexOf('.'),
                          Convert.ToString(prevParaDist).Length - Convert.ToString(prevParaDist).IndexOf('.'))) : Convert.ToDouble(prevParaDist);
                    }
                }
                else
                    pageIndent.PrevParaY = 0;

                XmlNode nextParaNode = line.ParentNode.NextSibling;
                if (nextParaNode != null && nextParaNode.ChildNodes.Count > 0)
                {
                    List<double> llyNextParaList =
                        nextParaNode.SelectNodes("descendant::ln/@top")
                            .Cast<XmlNode>()
                            .Select(x => Convert.ToDouble(x.Value))
                            .ToList();
                    if (llyNextParaList.Count > 0)
                    {
                        //pageIndent.NextParaY = Math.Abs(Convert.ToDouble(llyValueList[llyValueList.Count - 1]) - Convert.ToDouble(llyNextParaList[0]));

                        double nextParaDist = Math.Abs(Convert.ToDouble(llyValueList[llyValueList.Count - 1]) - Convert.ToDouble(llyNextParaList[0]));

                        pageIndent.NextParaY = Convert.ToString(nextParaDist).Contains(".")
                          ? Convert.ToDouble(Convert.ToString(nextParaDist).Remove(Convert.ToString(nextParaDist).IndexOf('.'),
                          Convert.ToString(nextParaDist).Length - Convert.ToString(nextParaDist).IndexOf('.'))) : Convert.ToDouble(nextParaDist);
                    }
                }
                else
                    pageIndent.NextParaY = 0;
            }//end leftValueList


            return pageIndent;
        }

        public double GetPageNormalY(XmlDocument mainXML, XmlNode line)
        {
            if (mainXML == null || string.IsNullOrEmpty(mainXML.InnerText) || line == null || line.ParentNode == null)
                return 0;

            double normalY = 0;

            var pageList = line.SelectNodes("descendant::ln/@page").Cast<XmlNode>().Select(x => Convert.ToInt32(x.Value)).ToList();

            int page = pageList[0];

            List<double> llyValueList = mainXML.SelectNodes("//ln[@page='" + page + "']/@top").Cast<XmlNode>().Select(x => Convert.ToDouble(x.Value)).ToList();

            if (llyValueList.Count > 0)
            {
                var verticalDiffereances =
                       llyValueList.Take(llyValueList.Count - 1).Select((v, i) => llyValueList[i + 1] - v)
                           .Where(x => Math.Abs(x) > 0).ToList();

                if (verticalDiffereances.Count > 0)
                {
                    var allDifferenceByValue = verticalDiffereances.GroupBy(x => Math.Floor(Math.Abs(x)))
                        .Select(g => new { Value = g.Key, Count = g.Count() })
                        .OrderByDescending(x => x.Count).ToList();

                    if (allDifferenceByValue.Count > 0)
                        normalY = Math.Floor(allDifferenceByValue[0].Value);
                }
            }

            return normalY;
        }

        private XmlNode CreateSParaXmlAfterSplit(XmlNode paraToConvert)
        {
            try
            {
                XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("spara");
                string origNodeXml = paraToConvert.InnerXml;
                origNodeXml = Regex.Replace(origNodeXml, "</?num.*?>", "");
                origNodeXml = Regex.Replace(origNodeXml, "</?para.*?>", "");
                origNodeXml = Regex.Replace(origNodeXml, "</?line.*?>", "");

                ((XmlElement)convertedNode).SetAttribute("h-align", ddlSparaOrientation.SelectedValue);
                ((XmlElement)convertedNode).SetAttribute("bgcolor", ddlSparaBackground.SelectedValue);
                ((XmlElement)convertedNode).SetAttribute("type", ddlSparaType.SelectedValue);

                ((XmlElement)convertedNode).SetAttribute("id", "0");
                ((XmlElement)convertedNode).SetAttribute("pnum", "0");
                ((XmlElement)convertedNode).SetAttribute("text-indent", "0");
                ((XmlElement)convertedNode).SetAttribute("padding-bottom", "0");

                // Assigning line or para
                string xmlChild = ddlSparaSubType.SelectedValue;
                string xml = "";
                if (chkStanza.Enabled == true && chkStanza.Checked == true)
                {
                    xml = origNodeXml.Replace("<ln", "<" + xmlChild + "><ln")
                        .Replace("</ln>", "</ln></" + xmlChild + ">");
                    xml = Regex.Replace(xml, "(</ln>)(</line>|</para>)(<break.*?>)", "$3$1$2");
                }
                else
                {
                    xml = "<" + xmlChild + ">" + origNodeXml + "</" + xmlChild + ">";
                }
                convertedNode.InnerXml = xml;
                if (convertedNode.Attributes["type"].Value.ToString() != "other")
                {
                    XmlAttribute attAlign = convertedNode.Attributes["h-align"];
                    convertedNode.Attributes.Remove(attAlign);
                }
                return convertedNode;
            }
            catch (Exception)
            {
                return null;
            }
        }

        #endregion

        #region NPara

        public List<XmlNode> GetNPara(XmlNodeList allParas)
        {
            if (allParas == null || allParas.Count == 0) return null;

            List<XmlNode> abnomalParas = new List<XmlNode>();
            int pageNum = 0;
            double nParaLlxValue = 0;
            double leftValueOffset = 2;

            foreach (XmlNode abnormalNode in allParas)
            {
                if (abnormalNode != null &&
                    abnormalNode.ChildNodes.Count > 0 &&
                    !abnormalNode.ChildNodes[0].Name.Equals("#text") &&
                    abnormalNode.ChildNodes[0].Attributes != null &&
                    abnormalNode.ChildNodes[0].Attributes["page"] != null &&
                    abnormalNode.ChildNodes[0].Attributes["left"] != null &&
                    abnormalNode.Attributes != null &&
                    abnormalNode.Attributes["abnormalLeft"] != null &&
                    abnormalNode.Attributes["pType"] != null &&
                    abnormalNode.Attributes["pType"].Value.Equals("nPara"))
                {
                    pageNum = Convert.ToInt32(abnormalNode.ChildNodes[0].Attributes["page"].Value);
                    nParaLlxValue = Convert.ToDouble(abnormalNode.ChildNodes[0].Attributes["left"].Value);
                    break;
                }
            }

            if (pageNum > 0)
            {
                List<XmlNode> currentPageUParaList = allParas.Cast<XmlNode>().Where(x => x.Attributes != null &&
                                                                                         x.ChildNodes.Count > 0 &&
                                                                                         x.ChildNodes[0].Attributes != null &&
                                                                                         x.ChildNodes[0].Attributes["page"] != null &&
                                                                                         Convert.ToInt32(x.ChildNodes[0].Attributes["page"].Value)
                                                                                        .Equals(pageNum)).ToList();

                if (currentPageUParaList != null && currentPageUParaList.Count > 0)
                {
                    for (int i = 0; i < currentPageUParaList.Count; i++)
                    {
                        if (currentPageUParaList[i] != null &&
                            currentPageUParaList[i].Attributes != null &&
                            currentPageUParaList[i].Attributes["abnormalLeft"] != null &&
                            currentPageUParaList[i].Attributes["pType"] != null &&
                            currentPageUParaList[i].Attributes["pType"].Value.Equals("nPara") &&
                            currentPageUParaList[i].ChildNodes != null &&
                            currentPageUParaList[i].ChildNodes.Count > 0 &&
                            currentPageUParaList[i].ChildNodes[0].Attributes != null &&
                            currentPageUParaList[i].ChildNodes[0].Attributes["left"] != null &&
                            Math.Abs(Convert.ToDouble(currentPageUParaList[i].ChildNodes[0].Attributes["left"].Value) - nParaLlxValue) < leftValueOffset)
                        {
                            abnomalParas.Add(currentPageUParaList[i]);
                        }

                        if (abnomalParas.Count > 0) break;
                    }
                }

                return abnomalParas;
            }
            else return null;
        }

        public ResultStatus convertToNpara(bool isApplyAll, string paraType)
        {
            string msgText = "";
            ResultStatus status = null;

            try
            {
                LoadPdfXml();

                List<XmlNode> paraToConvert = new List<XmlNode>();
                List<XmlNode> allMatchingNodes = new List<XmlNode>();
                XmlNode abnormalPara = null;
                double fontSize = 0;
                string fontName = string.Empty;
                double xIndent = 0;
                int pageCount = 0;
                int page = 0;
                string nParaStartWord = "";
                List<string> nParaWordList = new List<string>();

                //if (Convert.ToString(allWords[0].Trim().ToCharArray().ElementAt(0)).Equals("•")

                if (objGlobal != null && objGlobal.PBPDocument != null)
                {
                    if (SelectedXmlParaNodes != null && SelectedXmlParaNodes.Count > 0)
                        abnormalPara = SelectedXmlParaNodes[0];
                    else
                        abnormalPara = objGlobal.PBPDocument.SelectSingleNode("//*[@normalX and @pType='nPara']");

                    if (abnormalPara != null &&
                        abnormalPara.Attributes != null &&
                        abnormalPara.Attributes["abnormalLeft"] != null &&
                        abnormalPara.Attributes["abnormalRight"] != null &&
                        abnormalPara.Attributes["normalIndX"] != null &&
                        abnormalPara.Attributes["normalTop"] != null &&
                        abnormalPara.Attributes["normalBottom"] != null &&
                        abnormalPara.Attributes["normalY"] != null &&
                        abnormalPara.Attributes["pageType"] != null &&
                        abnormalPara.Attributes["font"] != null &&
                        abnormalPara.Attributes["fontSize"] != null &&
                        abnormalPara.Attributes["pageNum"] != null &&
                        //abnormalPara.Attributes["pType"] != null &&
                        abnormalPara.ChildNodes.Count > 0 &&
                        abnormalPara.ChildNodes[0].Attributes != null &&
                        abnormalPara.ChildNodes[0].Attributes["fontsize"] != null)
                    {
                        page = Convert.ToInt32(abnormalPara.Attributes["pageNum"].Value);

                        if (page == 0) return null;

                        fontSize = Convert.ToDouble(abnormalPara.Attributes["fontSize"].Value);
                        fontName = Convert.ToString(abnormalPara.Attributes["font"].Value);
                        xIndent = Convert.ToDouble(abnormalPara.Attributes["abnormalLeft"].Value);

                        nParaWordList = Regex.Split(abnormalPara.InnerText.Trim(), @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
                        if (nParaWordList.Count > 0)
                        {
                            nParaStartWord = nParaWordList[0];
                        }

                        if (isApplyAll)
                        {
                            allMatchingNodes = objGlobal.PBPDocument.SelectNodes("//upara[@normalX='" + abnormalPara.Attributes["normalX"].Value +
                                                                  "' and @normalIndX='" + abnormalPara.Attributes["normalIndX"].Value +
                                //"' and @normalTop='" + abnormalPara.Attributes["normalTop"].Value +
                                //"' and @normalBottom='" + abnormalPara.Attributes["normalBottom"].Value +
                                //"' and @normalY='" + abnormalPara.Attributes["normalY"].Value +
                                                                  "' and @font='" + abnormalPara.Attributes["font"].Value +
                                                                  "' and @fontSize='" + abnormalPara.Attributes["fontSize"].Value +
                                                                  "' and @pageType='" + abnormalPara.Attributes["pageType"].Value +
                                                                  "' and @pType='nPara']").Cast<XmlNode>().ToList();

                            if (allMatchingNodes.Count > 0)
                            {
                                double valueMargin = 2;

                                foreach (XmlNode uPara in allMatchingNodes)
                                {
                                    nParaWordList = Regex.Split(uPara.InnerText.Trim(), @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
                                    if (nParaWordList.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(nParaStartWord) &&
                                            !string.IsNullOrEmpty(nParaWordList[0]) &&
                                            nParaWordList[0].Equals(nParaStartWord))
                                        {
                                            paraToConvert.Add(uPara);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            allMatchingNodes = objGlobal.PBPDocument.SelectNodes("//upara[@normalX='" + abnormalPara.Attributes["normalX"].Value +
                               "' and @normalIndX='" + abnormalPara.Attributes["normalIndX"].Value +
                                //"' and @normalTop='" + abnormalPara.Attributes["normalTop"].Value +
                                //"' and @normalBottom='" + abnormalPara.Attributes["normalBottom"].Value +
                                //"' and @normalY='" + abnormalPara.Attributes["normalY"].Value +
                               "' and @font='" + abnormalPara.Attributes["font"].Value +
                               "' and @fontSize='" + abnormalPara.Attributes["fontSize"].Value +
                               "']/ln[@page='" + page + "']/..").Cast<XmlNode>().ToList();

                            if (allMatchingNodes.Count > 0)
                            {
                                double valueMargin = 2;

                                foreach (XmlNode uPara in allMatchingNodes)
                                {
                                    nParaWordList = Regex.Split(uPara.InnerText.Trim(), @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
                                    if (nParaWordList.Count > 0)
                                    {
                                        if (!string.IsNullOrEmpty(nParaStartWord) &&
                                            !string.IsNullOrEmpty(nParaWordList[0]) &&
                                            nParaWordList[0].Equals(nParaStartWord))
                                        {
                                            paraToConvert.Add(uPara);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                if (paraToConvert.Count > 0)
                {
                    BeforeComplexBitMappingList = paraToConvert.Select(x => x.Clone()).ToList();
                    CreateNParaXml(paraToConvert);

                    objGlobal.SaveXml();

                    pageCount = paraToConvert.Where(x => x.ChildNodes.Count > 0 &&
                                        x.ChildNodes[0].Attributes != null &&
                                        x.ChildNodes[0].Attributes["page"] != null).Select(x => Convert.ToInt32(x.ChildNodes[0].Attributes["page"].Value))
                                         .Distinct().Count();

                    if (paraToConvert.Count == 1)
                    {
                        msgText = paraToConvert.Count + " para in page " + page + " is successfully converted into " +
                                  paraType +
                                  " with indentation = " + xIndent + ", FontName = " + fontName + " and fontsize = " +
                                  fontSize;
                    }
                    else if (paraToConvert.Count > 1 && pageCount == 1)
                    {
                        msgText = paraToConvert.Count + " paras in page " + page + " is successfully converted into " +
                                  paraType +
                                  " with indentation = " + xIndent + ", FontName = " + fontName + " and fontsize = " +
                                  fontSize;
                    }
                    else if (paraToConvert.Count > 1 && pageCount > 1)
                    {
                        msgText = paraToConvert.Count + " paras in " + pageCount + " pages are successfully converted into " +
                                  paraType +
                                  " with indentation = " + xIndent + ", FontName = " + fontName + " and fontsize = " +
                                  fontSize;
                    }

                    status = new ResultStatus();
                    status.Status = "success";
                    status.Message = msgText;
                }
                else
                    divParaText.InnerText = "";

                return status;
            }
            catch (Exception)
            {
                status = new ResultStatus();
                status.Status = "error";
                status.Message = "Some error has occured while converting upara.";
                return status;
            }
        }

        public void SplitNPara(bool isApplyAll)
        {
            XmlNode abnormalPara = null;

            if (SelectedXmlParaNodes != null && SelectedXmlParaNodes.Count > 0)
            {
                if (!string.IsNullOrEmpty(SelectedXmlParaNodes[0].InnerText))
                {
                    LoadPdfXml();

                    int page = Convert.ToInt32(SelectedXmlParaNodes[0].ChildNodes[0].Attributes["page"].Value);
                    double llx = Convert.ToDouble(SelectedXmlParaNodes[0].ChildNodes[0].Attributes["left"].Value);
                    double lly = Convert.ToDouble(SelectedXmlParaNodes[0].ChildNodes[0].Attributes["top"].Value);

                    XmlNode matchedUpara =
                        objGlobal.PBPDocument.SelectSingleNode("//upara/ln[@page='" + page + "' and @left= '" + llx +
                                                               "' and @top='" + lly + "']/..");

                    if (matchedUpara != null)
                    {
                        abnormalPara = matchedUpara;
                    }
                }
                //abnormalPara = SelectedXmlParaNodes[0];
            }
            else
                abnormalPara = objGlobal.PBPDocument.SelectSingleNode("//*[@abnormalLeft][1]");

            Indentation paraIndValues = GetParaCoords(abnormalPara);

            if (objGlobal != null &&
                objGlobal.PBPDocument != null &&
                abnormalPara != null &&
                paraIndValues != null &&
                abnormalPara.Attributes != null &&
                abnormalPara.Attributes["normalX"] != null &&
                abnormalPara.Attributes["normalIndX"] != null &&
                abnormalPara.Attributes["normalTop"] != null &&
                abnormalPara.Attributes["normalBottom"] != null &&
                abnormalPara.Attributes["normalY"] != null &&
                abnormalPara.Attributes["pageNum"] != null &&
                abnormalPara.Attributes["pageType"] != null &&
                abnormalPara.Attributes["font"] != null &&
                abnormalPara.Attributes["fontSize"] != null &&
                abnormalPara.ChildNodes.Count > 0)
            {
                if (isApplyAll)
                {
                    List<XmlNode> allMatchingNodes =
                        objGlobal.PBPDocument.SelectNodes("//upara[@pageType='" + abnormalPara.Attributes["pageType"].Value +
                                                          "' and @normalX='" + abnormalPara.Attributes["normalX"].Value +
                                                          "' and @normalIndX='" + abnormalPara.Attributes["normalIndX"].Value +
                        //"' and @normalTop='" + abnormalPara.Attributes["normalTop"].Value +
                        //"' and @normalBottom='" + abnormalPara.Attributes["normalBottom"].Value +
                        //"' and @normalY='" + abnormalPara.Attributes["normalY"].Value +
                                                          "' and @font='" + abnormalPara.Attributes["font"].Value +
                                                          "' and @fontSize='" + abnormalPara.Attributes["fontSize"].Value + "']")
                            .Cast<XmlNode>()
                            .ToList();
                    if (allMatchingNodes.Count > 0)
                    {
                        foreach (XmlNode upara in allMatchingNodes)
                        {
                            var splittedParas = NParaSplitting(upara, objGlobal.PBPDocument, paraIndValues);
                        }
                    }
                }
                else
                {
                    var splittedParas = NParaSplitting(abnormalPara, objGlobal.PBPDocument, paraIndValues);
                }

                objGlobal.SaveXml();
            }
        }

        public List<XmlNode> NParaSplitting(XmlNode para, XmlDocument xmlDoc, Indentation pageIndent)
        {
            double paraX1Val = 0;
            double paraFontSize = 0;
            double paraEndXValPrev = 0;
            double paraEndXVal = 0;

            bool isNormalPara = true;

            double normalFontSize = pageIndent.FontSize;
            string normalFontName = pageIndent.FontName;
            double normalIndentX = pageIndent.NormalIndentX;
            double normalX = pageIndent.NormalX;
            double normalEndX = pageIndent.Endx;
            int page = pageIndent.Page;

            double xIndentXValMargin = 3;
            double endXValMargin = 3;

            string nParaStartWord = "";
            double x1ValFirstLine = 0; 

            if (para != null && para.SelectNodes("descendant::ln").Count > 0)
            {
                //x1ValFirstLine = Math.Floor(double.Parse(para.SelectNodes("descendant::ln")[0].Attributes["left"].Value.ToString()));
                var nParaWordList = Regex.Split(para.SelectNodes("descendant::ln")[0].InnerText.Trim(), @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
                if (nParaWordList.Count > 0)
                {
                    nParaStartWord = nParaWordList[0].Trim();
                }
            }

            List<XmlNode> newParaList = new List<XmlNode>();

            if (para != null && para.SelectNodes("//ln").Count > 0)
            {
                XmlElement xparaElem = SubSplitting(xmlDoc, para);
                XmlNodeList lineList = para.SelectNodes("descendant::ln");
                if (lineList.Count > 0)
                {
                    newParaList.Add(xparaElem);
                    for (int m = 0; m < lineList.Count; m++)
                    {
                        double x1Val = Math.Floor(double.Parse(lineList[m].Attributes["left"].Value.ToString()));
                        double fontSize = double.Parse(lineList[m].Attributes["fontsize"].Value.ToString());
                        string fontName = lineList[m].Attributes["font"].Value;

                        List<string> wordsList = Regex.Split(lineList[m].InnerText.Trim(), @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

                        if (wordsList.Count > 0)
                        {
                            if (nParaStartWord.Equals(wordsList[0].Trim()) && normalFontSize.Equals(fontSize) && IsSameFontName(fontName, normalFontName))
                            {
                                xparaElem = SubSplittingNPara(xmlDoc, para, "npara");
                                xparaElem.AppendChild(lineList[m]);
                                newParaList.Add(xparaElem);
                            }
                            else if (normalFontSize.Equals(fontSize) && IsSameFontName(fontName, normalFontName))
                            {
                                if (xparaElem != null)
                                {
                                    xparaElem.AppendChild(lineList[m]);
                                }
                            }
                            else
                            {
                                if (xparaElem != null)
                                {
                                    xparaElem.AppendChild(lineList[m]);
                                }
                            }
                        }
                    }
                    if (newParaList.Count > 0)
                    {
                        foreach (XmlElement elem in newParaList)
                        {
                            if (!string.IsNullOrEmpty(elem.InnerText))
                                para.ParentNode.InsertBefore(elem, para);
                        }
                        para.ParentNode.RemoveChild(para);
                    }
                }
            }
            return newParaList;
        }

        public XmlElement SubSplittingNPara(XmlDocument xmlDoc, XmlNode para, string paraType)
        {
            XmlElement xparaElem = xmlDoc.CreateElement(para.Name);

            if (para != null &&
                para.ChildNodes.Count > 0 &&
                para.ChildNodes[0].Attributes != null &&
                para.ChildNodes[0].Attributes["left"] != null &&
                para.ChildNodes[0].Attributes["font"] != null &&
                para.ChildNodes[0].Attributes["fontsize"] != null)
            {

                Indentation paraIndValues = GetParaAttrFromLine(xmlDoc, para, paraType);

                if (paraIndValues != null && para.ParentNode != null)
                {
                    XmlAttribute abnormalLeft = xmlDoc.CreateAttribute("abnormalLeft");
                    XmlAttribute abnormalRight = xmlDoc.CreateAttribute("abnormalRight");

                    XmlAttribute normalLeft = xmlDoc.CreateAttribute("normalX");
                    XmlAttribute normalIndentLeft = xmlDoc.CreateAttribute("normalIndX");
                    XmlAttribute normalTop = xmlDoc.CreateAttribute("normalTop");
                    XmlAttribute normalBottom = xmlDoc.CreateAttribute("normalBottom");

                    XmlAttribute normalY = xmlDoc.CreateAttribute("normalY");

                    XmlAttribute pageNum = xmlDoc.CreateAttribute("pageNum");
                    XmlAttribute pageType = xmlDoc.CreateAttribute("pageType");

                    XmlAttribute font = xmlDoc.CreateAttribute("font");
                    XmlAttribute fontSize = xmlDoc.CreateAttribute("fontSize");

                    var pageNumberList = para.SelectNodes("ln")
                        .Cast<XmlNode>()
                        .Select(node => Convert.ToInt32(node.Attributes["page"].Value)).Distinct().ToList();
                    if (pageNumberList.Count > 0)
                    {
                        pageNum.Value = Convert.ToString(pageNumberList[0]);
                        if (pageNumberList[0] % 2 == 0)
                        {
                            pageType.Value = "even";
                        }
                        else
                        {
                            pageType.Value = "odd";
                        }

                        abnormalLeft.Value = Convert.ToString(paraIndValues.NormalX);
                        abnormalRight.Value = Convert.ToString(paraIndValues.Endx);

                        normalLeft.Value = Convert.ToString(paraIndValues.NormalX);
                        normalIndentLeft.Value = Convert.ToString(paraIndValues.NormalIndentX);
                        normalTop.Value = Convert.ToString(paraIndValues.PrevParaY);
                        normalBottom.Value = Convert.ToString(paraIndValues.NextParaY);
                        normalY.Value = Convert.ToString(paraIndValues.NormalY);
                    }
                    xparaElem.Attributes.Append(abnormalLeft);
                    xparaElem.Attributes.Append(abnormalRight);
                    xparaElem.Attributes.Append(normalLeft);
                    xparaElem.Attributes.Append(normalIndentLeft);
                    xparaElem.Attributes.Append(normalTop);
                    xparaElem.Attributes.Append(normalBottom);
                    xparaElem.Attributes.Append(normalY);

                    xparaElem.Attributes.Append(pageNum);
                    xparaElem.Attributes.Append(pageType);

                    XmlNode firstLineNode = para.SelectSingleNode("ln");
                    if (firstLineNode != null &&
                        firstLineNode.Attributes != null &&
                        firstLineNode.Attributes["font"] != null &&
                        firstLineNode.Attributes["fontsize"] != null)
                    {
                        font.Value = firstLineNode.Attributes["font"].Value;
                        fontSize.Value = firstLineNode.Attributes["fontsize"].Value;
                        xparaElem.Attributes.Append(font);
                        xparaElem.Attributes.Append(fontSize);
                    }
                }
            }
            return xparaElem;
        }

        #endregion

        #region FootNotes

        private void convertToFootNote(bool isApplyAll)
        {
            try
            {
                if (isApplyAll)
                {
                    convertToFootNoteAll();
                }
                else
                {
                    double yCoordOffset = 0.09;
                    double pageHeight = 0;
                    //string supScriptId = string.Empty;

                    //List<string> supScriptIdsList = null;

                    XmlNodeList footNoteLines = null;

                    List<XmlNode> allFootNoteLines = new List<XmlNode>();

                    StringBuilder supScriptLineText = new StringBuilder();
                    string footNoteLinesText = string.Empty;
                    //List<string> footNoteLinesText = null;

                    LoadPdfXml();

                    XmlNodeList mainXmlLines = objGlobal.PBPDocument.SelectNodes("//ln");

                    string tempXmlPath = GetTempXmlPath();

                    if (string.IsNullOrEmpty(tempXmlPath) || !File.Exists(tempXmlPath)) return;

                    XmlDocument temp_FootNotesXmlDoc = new XmlDocument();
                    temp_FootNotesXmlDoc.Load(tempXmlPath);

                    XmlNode firstSupScriptLine = temp_FootNotesXmlDoc.SelectSingleNode("//*[Word[@wordtype=\"supScript\"]/..]");

                    int supScriptPage = 0;

                    if (firstSupScriptLine != null &&
                        firstSupScriptLine.ParentNode != null &&
                        firstSupScriptLine.ParentNode.ParentNode != null &&
                        firstSupScriptLine.ParentNode.ParentNode.Attributes != null &&
                        firstSupScriptLine.ParentNode.ParentNode.Attributes["number"] != null)
                    {
                        supScriptPage = Convert.ToInt32(firstSupScriptLine.ParentNode.ParentNode.Attributes["number"].Value);
                    }

                    if (supScriptPage > 0)
                    {
                        List<XmlNode> supScriptWordsList = GetSupScriptWordsFromLine(firstSupScriptLine);

                        if (supScriptWordsList != null && supScriptWordsList.Count > 0)
                        {
                            //supScriptIdsList = new List<string>();
                            //footNoteLinesText = new List<string>();

                            for (int i = 0; i < supScriptWordsList.Count; i++)
                            {
                                //}
                                //foreach (XmlNode supScriptWord in supScriptWordsList)
                                //{
                                if (supScriptWordsList[i].Attributes != null && supScriptWordsList[i].Attributes["id"] != null &&
                                    !string.IsNullOrEmpty(supScriptWordsList[i].Attributes["id"].Value))
                                {
                                    //supScriptId = supScriptWord.Attributes["id"].Value;

                                    //supScriptIdsList.Add(supScriptWordsList[i].Attributes["id"].Value);

                                    //if (!string.IsNullOrEmpty(supScriptId))
                                    //{
                                    footNoteLines = temp_FootNotesXmlDoc.SelectNodes("//Page[@number='" + supScriptPage + "' or @number='" + (supScriptPage + 1) +
                                                                         "']/Para/Line[@linetype='footnote' and @id='" +
                                                                         supScriptWordsList[i].Attributes["id"].Value + "']");

                                    //footNoteLines = temp_FootNotesXmlDoc.SelectNodes("//Page[@number='" + (supScriptPage + 1) +
                                    //                                  "']/Para/Line[@linetype='footnote' and @id='" + supScriptId + "']");

                                    if (footNoteLines != null && footNoteLines.Count > 0)
                                    {
                                        //XmlNode pageHeightNode = temp_FootNotesXmlDoc.SelectSingleNode("//Page[@number=" + supScriptPage + "]/@height");
                                        //if (pageHeightNode != null && !string.IsNullOrEmpty(pageHeightNode.Value))
                                        //{
                                        //    pageHeight = Convert.ToDouble(pageHeightNode.Value);
                                        //    footNoteLinesText = GetFootNoteLinesText(footNoteLines, supScriptPage, pageHeight);


                                        allFootNoteLines.AddRange(footNoteLines.Cast<XmlNode>().ToList());

                                        //supScriptLineText.Append(SetFNoteTextInWordNode(firstSupScriptLine, footNoteLinesText,
                                        //    supScriptWordsList[i].Attributes["id"].Value));

                                        footNoteLinesText = GetFootNoteLinesText(footNoteLines);
                                        SetFNoteTextInWordNode(firstSupScriptLine, footNoteLinesText, supScriptWordsList[i].Attributes["id"].Value, temp_FootNotesXmlDoc);

                                        //if (i == supScriptWordsList.Count - 1)
                                        //{
                                        //    supScriptLineText.Append(GetFinalSupScriptLine(firstSupScriptLine, footNoteLinesText,
                                        //           supScriptWordsList[i].Attributes["id"].Value));
                                        //}

                                        //}
                                    }
                                    //}
                                }
                            }

                            //foreach (string supScriptId in supScriptIdsList)
                            //{
                            // supScriptLineText.Append(SetFNoteTextInWordNode(firstSupScriptLine, footNoteLinesText, supScriptIdsList));
                            //}                

                            //foreach (string VARIABLE in supScriptIdsList)
                            //{

                            //}

                            InsertFootNotesInSupScriptLine(mainXmlLines, firstSupScriptLine, supScriptPage, yCoordOffset, supScriptWordsList);

                            //var tt = tempXmlPath.Replace("Temp_FootNotes", "");

                            //objGlobal.RhywPath = tempXmlPath.Replace("Temp_FootNotes", "");
                            //objGlobal.SaveRhyw();

                            SetIdInFootNoteLines(allFootNoteLines, mainXmlLines, yCoordOffset, supScriptPage);

                            //var tt = tempXmlPath.Replace("Temp_FootNotes", "");
                            //objGlobal.RhywPath = tempXmlPath.Replace("Temp_FootNotes", "");
                            //objGlobal.SaveRhyw();

                            //RemoveFootNotesFromBottom(allFootNoteLines, mainXmlLines, yCoordOffset, supScriptPage);

                            temp_FootNotesXmlDoc.Save(tempXmlPath);
                            objGlobal.SaveXml();
                        }
                        else
                        {
                            divParaText.InnerText = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public XmlNode RemoveFootNote(PdfFootNote detFootnoteDetails)
        {
            if (detFootnoteDetails != null && DetectedFootNotes.SupScriptXmlLine != null &&
                detFootnoteDetails.SupScriptWord != null && detFootnoteDetails.Page > 0)
            {
                int page = detFootnoteDetails.Page;
                string supScriptId = detFootnoteDetails.SupScriptWord;

                string tempXmlPath = GetTempXmlPath();

                if (string.IsNullOrEmpty(tempXmlPath) || !File.Exists(tempXmlPath) || page < 1 || string.IsNullOrEmpty(supScriptId)) return null;

                XmlDocument temp_FootNotesXmlDoc = new XmlDocument();
                temp_FootNotesXmlDoc.Load(tempXmlPath);

                XmlNode supScriptLine = temp_FootNotesXmlDoc.SelectSingleNode("//Page[@number='" + page +
                                                          "']/descendant::Word[@wordtype='supScript' and @id='"
                                                          + supScriptId + "']");

                if (supScriptLine != null)
                {
                    if (((XmlElement)supScriptLine).HasAttribute("wordtype"))
                    {
                        ((XmlElement)supScriptLine).RemoveAttribute("wordtype");
                    }

                    if (((XmlElement)supScriptLine).HasAttribute("id"))
                    {
                        ((XmlElement)supScriptLine).RemoveAttribute("id");
                    }

                    temp_FootNotesXmlDoc.Save(tempXmlPath);
                }
            }
            return null;
        }

        private void RemoveFootNoteTags()
        {
            try
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["XMLPath"])))
                    return;

                string strucToolRhywPath = Convert.ToString(Session["XMLPath"]).Replace(".rhyw", "_structoolcopy.rhyw");

                if (File.Exists(strucToolRhywPath))
                    return;

                populateSessions();
                objGlobal.XMLPath = Convert.ToString(Session["XMLPath"]);

                if (!File.Exists(objGlobal.XMLPath)) return;

                objGlobal.LoadXml();

                XmlNode mainXmlCopy = objGlobal.PBPDocument.Clone();

                var footNoteNodes = objGlobal.PBPDocument.SelectNodes("//footnote");

                if (footNoteNodes != null && footNoteNodes.Count > 0)
                {
                    foreach (XmlNode footNote in footNoteNodes)
                    {
                        if (footNote != null && footNote.ParentNode != null)
                        {
                            footNote.ParentNode.RemoveChild(footNote);
                        }
                    }

                    objGlobal.SaveXml();
                }

                var supScriptLines = mainXmlCopy.SelectNodes("//*[@wordtype='supScript']");

                if (supScriptLines != null && supScriptLines.Count > 0)
                {
                    foreach (XmlNode supScrLn in supScriptLines)
                    {
                        if (supScrLn != null && supScrLn.ParentNode != null)
                        {
                            supScrLn.ParentNode.RemoveChild(supScrLn);
                        }
                    }
                }

                var footNoteLines = mainXmlCopy.SelectNodes("//*[@footNoteId]");

                if (footNoteLines != null && footNoteLines.Count > 0)
                {
                    foreach (XmlNode footNoteLn in footNoteLines)
                    {
                        if (footNoteLn != null && footNoteLn.ParentNode != null)
                        {
                            footNoteLn.ParentNode.RemoveChild(footNoteLn);
                        }
                    }
                }

                var uParaLines = mainXmlCopy.SelectNodes("//upara");

                if (uParaLines != null && uParaLines.Count > 0)
                {
                    foreach (XmlNode uPara in uParaLines)
                    {
                        if (uPara != null && ((uPara.ChildNodes.Count == 0 && string.IsNullOrEmpty(uPara.InnerText.Trim())) ||
                                              (uPara.ChildNodes.Count == 1 && uPara.ChildNodes[0].Name.Equals("break"))))
                        {
                            uPara.ParentNode.RemoveChild(uPara);
                        }
                    }
                }

                objGlobalStrTool.XMLPath = strucToolRhywPath;
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(mainXmlCopy.InnerXml);
                objGlobalStrTool.PBPDocument = doc;
                objGlobalStrTool.SaveXml();
            }
            catch (Exception ex)
            {

            }
        }

        public PdfFootNote GetFootNotePara(XmlNodeList allParas)
        {
            string tempXmlPath = GetTempXmlPath();

            if (string.IsNullOrEmpty(tempXmlPath) || !File.Exists(tempXmlPath)) return null;

            PdfFootNote footNote = new PdfFootNote();
            double yCoordOffset = 0.09;

            try
            {
                XmlDocument tetmlXmlDoc = new XmlDocument();
                tetmlXmlDoc.Load(tempXmlPath);

                //XmlNode supScriptLine = tetmlXmlDoc.SelectSingleNode("//*[Word[@wordtype=\"supScript\"]/..][1]");

                XmlNode supScriptLine = tetmlXmlDoc.SelectSingleNode("//*[Word[@wordtype=\"supScript\" and @type!=\"end-node\"]/..]");
                XmlNode pageNumber = tetmlXmlDoc.SelectSingleNode("//*[Word[@wordtype='supScript' and @type!=\"end-node\"]/..]/ancestor::Page/@number");

                footNote.SupScriptXmlLine = supScriptLine;

                StringBuilder lineText = new StringBuilder();
                string supScriptId = "";
                int supScriptCounter = 0;

                if (supScriptLine != null && pageNumber != null)
                {
                    if (!string.IsNullOrEmpty(pageNumber.InnerText))
                        footNote.Page = Convert.ToInt32(pageNumber.InnerText);

                    lineText.Append("</br>");

                    XmlNodeList superScriptWrd = supScriptLine.SelectNodes("descendant::Word");

                    if (superScriptWrd != null && superScriptWrd.Count > 0)
                    {
                        foreach (XmlNode word in superScriptWrd)
                        {
                            if (word != null)
                            {
                                if (word.Attributes != null && word.Attributes["wordtype"] != null &&
                                    word.Attributes["id"] != null && supScriptCounter != 1)
                                {
                                    supScriptId = word.Attributes["id"].Value;
                                    supScriptCounter++;
                                }

                                lineText.Append(word.InnerText.Trim() + " ");
                            }
                        }
                    }

                    footNote.SupScriptWord = supScriptId;

                    XmlNodeList mainXmlLines = objGlobal.PBPDocument.SelectNodes("//ln");

                    //Setting footnote tag in subScrtipt line 
                    //List<XmlNode> subScriptLine = mainXmlLines.Cast<XmlNode>()
                    //    .Where(x => x.Attributes["page"] != null && Convert.ToInt32(x.Attributes["page"].Value).Equals(footNote.Page) && x.InnerText.Trim().Equals(footNote.SupScriptXmlLine.InnerText.Trim())).ToList();

                    if (footNote.Page > 0)
                    {
                        List<XmlNode> pageLine =
                            mainXmlLines.Cast<XmlNode>()
                                .Where(x => (x.Attributes["page"] != null && (x.Attributes["page"].Value)
                                    .Equals(Convert.ToString(footNote.Page)))).ToList();

                        if (pageLine.Count > 0)
                        {
                            List<XmlNode> subScriptLine = pageLine.Where(x => (x.Attributes["top"] != null &&
                                                                               x.Attributes["page"] != null &&
                                                                               footNote.SupScriptXmlLine != null &&
                                                                               footNote.SupScriptXmlLine.Attributes[
                                                                                   "supScrLowerY"] != null &&
                                                                               footNote.SupScriptXmlLine.Attributes[
                                                                                   "supScrUpperY"] != null &&
                                                                               (
                                                                                   (
                                                                                       Math.Floor(
                                                                                           Convert.ToDouble(
                                                                                               x.Attributes["top"].Value))
                                                                                           .Equals(
                                                                                               Math.Floor(
                                                                                                   Convert.ToDouble(
                                                                                                       footNote
                                                                                                           .SupScriptXmlLine
                                                                                                           .Attributes[
                                                                                                               "supScrLowerY"
                                                                                                           ].Value))))

                                                                                   ||

                                                                                   (
                                                                                       Math.Floor(
                                                                                           Convert.ToDouble(
                                                                                               x.Attributes["top"].Value))
                                                                                           .Equals(
                                                                                               Math.Floor(
                                                                                                   Convert.ToDouble(
                                                                                                       footNote
                                                                                                           .SupScriptXmlLine
                                                                                                           .Attributes[
                                                                                                               "supScrLowerY"
                                                                                                           ].Value) +
                                                                                                   yCoordOffset)))

                                                                                   ||

                                                                                   (
                                                                                       Math.Floor(
                                                                                           Convert.ToDouble(
                                                                                               x.Attributes["top"].Value))
                                                                                           .Equals(
                                                                                               Math.Floor(
                                                                                                   Convert.ToDouble(
                                                                                                       footNote
                                                                                                           .SupScriptXmlLine
                                                                                                           .Attributes[
                                                                                                               "supScrUpperY"
                                                                                                           ].Value))))

                                                                                   ||

                                                                                   (
                                                                                       Math.Floor(
                                                                                           Convert.ToDouble(
                                                                                               x.Attributes["top"].Value))
                                                                                           .Equals(
                                                                                               Math.Floor(
                                                                                                   Convert.ToDouble(
                                                                                                       footNote
                                                                                                           .SupScriptXmlLine
                                                                                                           .Attributes[
                                                                                                               "supScrUpperY"
                                                                                                           ].Value) +
                                                                                                   yCoordOffset)))

                                                                                   ||

                                                                                   (Math.Floor(
                                                                                       Convert.ToDouble(
                                                                                           x.Attributes["top"].Value))
                                                                                       .Equals(
                                                                                           Math.Floor(
                                                                                               Convert.ToDouble(
                                                                                                   footNote.SupScriptXmlLine
                                                                                                       .Attributes["y1"]
                                                                                                       .Value))))
                                                                                   )

                                //&&

                                //(footNote.SupScriptXmlLine.ParentNode.ParentNode.Attributes["number"].Value.Equals
                                //    (x.Attributes["page"].Value))

                                )).ToList();

                            if (subScriptLine.Count > 0 && subScriptLine[0].ParentNode != null)
                            {
                                if (SelectedXmlParaNodes == null)
                                    SelectedXmlParaNodes = new List<XmlNode>();

                                SelectedXmlParaNodes.Add(subScriptLine[0].ParentNode);
                            }
                        }
                        lineText.Append("</br></br>");

                        //XmlNodeList footNoteLines = tetmlXmlDoc.SelectNodes("//*[@linetype='footnote' and @id='" + supScriptId + "']");

                        int pageNum = Convert.ToInt32(pageNumber.Value);

                        XmlNodeList footNoteLines =
                            tetmlXmlDoc.SelectNodes("//Page[@number='" + pageNum +
                                                    "']/Para/Line[@linetype='footnote' and @id='"
                                                    + supScriptId + "']");

                        XmlNodeList contFootNoteLines =
                            tetmlXmlDoc.SelectNodes("//Page[@number='" + (pageNum + 1) +
                                                    "']/Para/Line[@linetype='footnote' and @id='"
                                                    + supScriptId + "']");

                        List<XmlNode> footNotesComplete = null;

                        if (footNoteLines != null && footNoteLines.Count > 0 && !string.IsNullOrEmpty(supScriptId))
                        {
                            footNotesComplete = new List<XmlNode>();

                            footNotesComplete.AddRange(footNoteLines.Cast<XmlNode>().ToList());

                            footNote.FootNoteXmlLines = footNotesComplete;

                            foreach (XmlNode ln in footNoteLines)
                            {
                                //Setting footnote tag for conversion to upara. 
                                List<XmlNode> ftNoteLine = mainXmlLines.Cast<XmlNode>()
                                    .Where(x => (x.Attributes["top"] != null &&
                                                 ln.Attributes["y1"] != null &&
                                                 x.Attributes["page"] != null) &&
                                                Convert.ToInt32(x.Attributes["page"].Value)
                                                    .Equals(footNote.Page) &&
                                                ((ln.Attributes["supScrLowerY"] != null &&
                                                  Math.Floor(Convert.ToDouble(x.Attributes["top"].Value))
                                                      .Equals(
                                                          Math.Floor(Convert.ToDouble(ln.Attributes["supScrLowerY"].Value)) +
                                                          yCoordOffset)) ||
                                                 (Math.Floor(Convert.ToDouble(x.Attributes["top"].Value))
                                                     .Equals(Math.Floor(Convert.ToDouble(ln.Attributes["y1"].Value))))))
                                    .ToList();


                                if (ftNoteLine.Count > 0 && SelectedXmlParaNodes != null && !SelectedXmlParaNodes.Contains(ftNoteLine[0].ParentNode))
                                {
                                    SelectedXmlParaNodes.Add(ftNoteLine[0].ParentNode);
                                }

                                XmlNodeList footNoteWord = ln.SelectNodes("descendant::Word");

                                if (footNoteWord != null && footNoteWord.Count > 0)
                                {
                                    foreach (XmlNode word in footNoteWord)
                                    {
                                        lineText.Append(word.InnerText.Trim() + " ");
                                    }
                                }
                            }

                            if (contFootNoteLines != null && contFootNoteLines.Count > 0 && !string.IsNullOrEmpty(supScriptId))
                            {
                                //footNotesComplete.AddRange(contFootNoteLines.Cast<XmlNode>().ToList());

                                foreach (XmlNode ln in contFootNoteLines)
                                {
                                    //Setting footnote tag for conversion to upara 
                                    List<XmlNode> ftNoteLine = mainXmlLines.Cast<XmlNode>()
                                        .Where(x => (x.Attributes["top"] != null &&
                                                     ln.Attributes["y1"] != null &&
                                                     x.Attributes["page"] != null) &&
                                                    Convert.ToInt32(x.Attributes["page"].Value)
                                                        .Equals(footNote.Page) &&
                                                    ((ln.Attributes["supScrLowerY"] != null &&
                                                      Math.Floor(Convert.ToDouble(x.Attributes["top"].Value))
                                                          .Equals(
                                                              Math.Floor(Convert.ToDouble(ln.Attributes["supScrLowerY"].Value)) +
                                                              yCoordOffset)) ||
                                                     (Math.Floor(Convert.ToDouble(x.Attributes["top"].Value))
                                                         .Equals(Math.Floor(Convert.ToDouble(ln.Attributes["y1"].Value))))))
                                        .ToList();


                                    if (ftNoteLine.Count > 0 && SelectedXmlParaNodes != null && !SelectedXmlParaNodes.Contains(ftNoteLine[0].ParentNode))
                                    {
                                        SelectedXmlParaNodes.Add(ftNoteLine[0].ParentNode);
                                    }

                                    XmlNodeList footNoteWord = ln.SelectNodes("descendant::Word");

                                    if (footNoteWord != null && footNoteWord.Count > 0)
                                    {
                                        foreach (XmlNode word in footNoteWord)
                                        {
                                            lineText.Append(word.InnerText.Trim() + " ");
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            lineText.Length = 0;
                        }

                        if (!string.IsNullOrEmpty(Convert.ToString(lineText)))
                        {
                            footNote.FootNoteText = Convert.ToString(lineText);
                            //divParaText.InnerHtml = "<font style='color:#4682b4'><sup>" + Convert.ToString(lineText) + "</sup></font>";
                            divParaText.InnerHtml = Convert.ToString(lineText);
                        }
                    }
                }

                //if (footNote.SupScriptWord == null) return null;

                return footNote;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<String> GetFootNoteParaCoordinates(string selectedParaType, PdfFootNote footNote, string pdfPath)
        {
            if (footNote == null) return null;

            try
            {
                List<string> lstcoordiants = new List<string>();
                double llx = 0;
                double lly = 0;
                double urx = 0;
                double ury = 0;
                double left = 0;
                double bottom = 0;

                List<double> rightValues = new List<double>();
                List<double> leftValues = new List<double>();
                List<double> topValues = new List<double>();
                List<double> bottomValues = new List<double>();

                XmlNode para = footNote.SupScriptXmlLine;

                if (selectedParaType.Equals("endnote") && para != null)
                {

                    leftValues.Add(para.Attributes["x1"] != null ? Convert.ToDouble(para.Attributes["x1"].Value) : 0);

                    bottomValues.Add(para.Attributes["y1"] != null ? Convert.ToDouble(para.Attributes["y1"].Value) : 0);

                    rightValues.Add(para.Attributes["x"] != null ? Convert.ToDouble(para.Attributes["x"].Value) : 0);

                    topValues.Add(para.Attributes["y"] != null ? Convert.ToDouble(para.Attributes["y"].Value) : 0);
                }
                else
                {
                    leftValues.AddRange(para.SelectSingleNode("descendant::Word[@wordtype='supScript']/@x1").Cast<XmlNode>()
                        .Where(x => !string.IsNullOrEmpty(x.Value))
                        .Select(x => Convert.ToDouble(x.Value)));

                    bottomValues.AddRange(para.SelectSingleNode("descendant::Word[@wordtype='supScript']/@y1")
                        .Cast<XmlNode>()
                        .Where(x => !string.IsNullOrEmpty(x.Value))
                        .Select(x => Convert.ToDouble(x.Value)));

                    rightValues.AddRange(para.SelectSingleNode("descendant::Word[@wordtype='supScript']/@x").Cast<XmlNode>()
                        .Where(x => !string.IsNullOrEmpty(x.Value))
                        .Select(x => Convert.ToDouble(x.Value)));

                    topValues.AddRange(para.SelectSingleNode("descendant::Word[@wordtype='supScript']/@y").Cast<XmlNode>()
                        .Where(x => !string.IsNullOrEmpty(x.Value))
                        .Select(x => Convert.ToDouble(x.Value)));
                }
                llx = leftValues.Min();
                lly = bottomValues.Min();
                urx = rightValues.Max();
                ury = topValues.Max();

                double width = Convert.ToDouble(urx) - Convert.ToDouble(llx);
                double height = Convert.ToDouble(ury) - Convert.ToDouble(lly);
                string croppedMargins = getCroppedMargins(pdfPath);

                if (!string.IsNullOrEmpty(croppedMargins))
                {
                    List<string> tempValues = croppedMargins.Split(' ').ToList();

                    if (tempValues != null)
                    {
                        if (tempValues.Count > 0)
                        {
                            left = Math.Abs(Convert.ToDouble(tempValues[3]));
                            bottom = Math.Abs(Convert.ToDouble(tempValues[1]));
                        }
                    }
                }

                lstcoordiants.Add((Convert.ToDouble(llx) + left) + " " + (Convert.ToDouble(lly) + bottom) + " " + height + " " + width + " " + footNote.Page);

                if (footNote.FootNoteXmlLines != null)
                {
                    rightValues = new List<double>();
                    leftValues = new List<double>();
                    topValues = new List<double>();
                    bottomValues = new List<double>();

                    //XmlNodeList footNoteWordsList = footNote.FootNoteXmlLines;

                    List<XmlNode> footNoteWordsList = footNote.FootNoteXmlLines;

                    if (footNoteWordsList != null)
                    {
                        foreach (XmlNode footNoteWord in footNoteWordsList)
                        {
                            leftValues.AddRange(footNoteWord.SelectNodes("descendant::Word/@x1").Cast<XmlNode>()
                                .Where(x => !string.IsNullOrEmpty(x.Value))
                                .Select(x => Convert.ToDouble(x.Value)));

                            bottomValues.AddRange(footNoteWord.SelectNodes("descendant::Word/@y1").Cast<XmlNode>()
                                .Where(x => !string.IsNullOrEmpty(x.Value))
                                .Select(x => Convert.ToDouble(x.Value)));

                            rightValues.AddRange(footNoteWord.SelectNodes("descendant::Word/@x").Cast<XmlNode>()
                                .Where(x => !string.IsNullOrEmpty(x.Value))
                                .Select(x => Convert.ToDouble(x.Value)));

                            topValues.AddRange(footNoteWord.SelectNodes("descendant::Word/@y").Cast<XmlNode>()
                                .Where(x => !string.IsNullOrEmpty(x.Value))
                                .Select(x => Convert.ToDouble(x.Value)));

                        }
                    }

                    llx = leftValues.Min();
                    lly = bottomValues.Min();
                    urx = rightValues.Max();
                    ury = topValues.Max();

                    width = Convert.ToDouble(urx) - Convert.ToDouble(llx);
                    height = Convert.ToDouble(ury) - Convert.ToDouble(lly);
                    croppedMargins = getCroppedMargins(pdfPath);

                    if (!string.IsNullOrEmpty(croppedMargins))
                    {
                        List<string> tempValues = croppedMargins.Split(' ').ToList();

                        if (tempValues != null)
                        {
                            if (tempValues.Count > 0)
                            {
                                left = Math.Abs(Convert.ToDouble(tempValues[3]));
                                bottom = Math.Abs(Convert.ToDouble(tempValues[1]));
                            }
                        }
                    }

                    lstcoordiants.Add((Convert.ToDouble(llx) + left) + " " + (Convert.ToDouble(lly) + bottom) + " " +
                                      height + " " + width + " " + footNote.Page);
                }
                return lstcoordiants;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public string GetTempXmlPath()
        {
            return objMyDBClass.MainDirPhyPath + "/" + Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0] +
                   "/DetectedFootNotes/Temp_FootNotes.xml";
        }

        public string CleanInvalidXmlChars(string fileText)
        {
            //Only 5 special characters in XML 
            //&lt; (<) , &amp; (&) , &gt; (>) , &quot; (") , &apos; (')

            return fileText.Replace("&", "&amp;")
                           .Replace("<", "&lt;")
                           .Replace(">", "&gt;")
                           .Replace("\"", "&quot;")
                           .Replace("'", "&apos;");
        }

        private void convertToFootNoteAll()
        {
            try
            {
                double yCoordOffset = 0.15;
                int supScriptPage = 0;
                double fontSize = 0;

                double urx = 0;
                double ury = 0;

                double ulx = 0;
                double uly = 0;
                double lry = 0;
                double lrx = 0;
                //double pageHeight = 0;

                string supScriptId = string.Empty;
                XmlNodeList footNoteLines = null;

                List<XmlNode> allFootNoteLines = null;

                StringBuilder supScriptLineText = new StringBuilder();
                string footNoteLinesText = string.Empty;

                LoadPdfXml();

                XmlNodeList mainXmlLines = objGlobal.PBPDocument.SelectNodes("//ln");

                string tempXmlPath = GetTempXmlPath();

                if (string.IsNullOrEmpty(tempXmlPath) || !File.Exists(tempXmlPath)) return;

                XmlDocument temp_FootNotesXmlDoc = new XmlDocument();
                temp_FootNotesXmlDoc.Load(tempXmlPath);

                //Get a superscript line from start page of the pdf.
                XmlNode firstSupScriptLine = temp_FootNotesXmlDoc.SelectSingleNode("//*[Word[@wordtype=\"supScript\"]/..]");

                if (firstSupScriptLine != null)
                {
                    XmlNode supScriptWord = GetSupScriptWordNode(firstSupScriptLine);

                    if (supScriptWord != null)
                    {
                        fontSize = Convert.ToDouble(supScriptWord.Attributes["fontsize"].Value);

                        if (fontSize > 0)
                        {
                            XmlNodeList sameFontSizeSupScrptLines = null;

                            ulx = supScriptWord.Attributes["ulx"] != null ? Convert.ToDouble(supScriptWord.Attributes["ulx"].Value) : 0;
                            uly = supScriptWord.Attributes["uly"] != null ? Convert.ToDouble(supScriptWord.Attributes["uly"].Value) : 0;

                            lry = supScriptWord.Attributes["lry"] != null ? Convert.ToDouble(supScriptWord.Attributes["lry"].Value) : 0;
                            lrx = supScriptWord.Attributes["lrx"] != null ? Convert.ToDouble(supScriptWord.Attributes["lrx"].Value) : 0;
                            urx = supScriptWord.Attributes["x"] != null ? Convert.ToDouble(supScriptWord.Attributes["x"].Value) : 0;
                            ury = supScriptWord.Attributes["y"] != null ? Convert.ToDouble(supScriptWord.Attributes["y"].Value) : 0;

                            //Ask footNote again if
                            //(1) Font Size changes
                            //(2) If superscript is merged with another word and uly is greator then ury
                            //(3) If superscript is merged with another word and uly is smaller then ury

                            if (urx.Equals(lrx) &&
                                lrx > 0 &&
                                ulx > 0 &&
                                uly > 0 &&
                                (uly > ury))
                            {
                                sameFontSizeSupScrptLines = temp_FootNotesXmlDoc.SelectNodes("//*[Word[@wordtype='supScript' and @fontsize=" +
                                                                        fontSize + " and @uly and @uly > @y]/..]");
                            }
                            else if (urx.Equals(lrx) &&
                                 lrx > 0 &&
                                 ulx > 0 &&
                                 uly > 0 &&
                                (uly < ury))
                            {
                                sameFontSizeSupScrptLines = temp_FootNotesXmlDoc.SelectNodes("//*[Word[@wordtype='supScript' and @fontsize=" +
                                                                        fontSize + " and @uly and @uly < @y]/..]");
                            }
                            else
                            {
                                //Get all same font size superscripts lines from pdf
                                sameFontSizeSupScrptLines = temp_FootNotesXmlDoc.SelectNodes("//*[Word[@wordtype='supScript' and @fontsize=" +
                                                                        fontSize + " and not(@uly) and not(@ulx)]/..]");
                            }

                            if (sameFontSizeSupScrptLines != null && sameFontSizeSupScrptLines.Count > 0)
                            {
                                foreach (XmlNode supScrptLine in sameFontSizeSupScrptLines)
                                {
                                    allFootNoteLines = new List<XmlNode>();

                                    supScriptLineText.Length = 0;

                                    if (supScrptLine != null && supScrptLine.ParentNode != null)
                                    {
                                        var pageXmlNode = supScrptLine.SelectSingleNode("ancestor::Page/@number").Cast<XmlNode>()
                                                                      .Where(x => Convert.ToInt32(x.Value) > 0).ToList();

                                        if (pageXmlNode.Count > 0)
                                        {
                                            supScriptPage = Convert.ToInt32(pageXmlNode[0].Value);
                                        }
                                    }

                                    //Get superScript word from line
                                    List<XmlNode> supScriptWordsList = GetSupScriptWordsFromLine(supScrptLine);

                                    if (supScriptWordsList != null && supScriptWordsList.Count > 0)
                                    {
                                        foreach (XmlNode supWrd in supScriptWordsList)
                                        {
                                            if (supWrd.Attributes != null && supWrd.Attributes["id"] != null)
                                            {
                                                supScriptId = supWrd.Attributes["id"].Value;

                                                if (!string.IsNullOrEmpty(supScriptId))
                                                {
                                                    //Get all bottom footnote lines from superScript word
                                                    //footNoteLines = temp_FootNotesXmlDoc.SelectNodes("//Page[@number='" + supScriptPage +
                                                    //                                     "']/Para/Line[@linetype='footnote' and @id='" + supScriptId + "']");

                                                    footNoteLines = temp_FootNotesXmlDoc.SelectNodes("//Page[@number=" + supScriptPage + " or @number=" + (supScriptPage + 1) + "]/Para/Line[@linetype='footnote' and @id='" + supScriptId + "']");

                                                    if (footNoteLines != null && footNoteLines.Count > 0)
                                                    {
                                                        //XmlNode pageHeightNode =
                                                        //    temp_FootNotesXmlDoc.SelectSingleNode("//Page[@number=" +
                                                        //                                          supScriptPage +
                                                        //                                          "]/@height");

                                                        //if (pageHeightNode != null &&
                                                        //    !string.IsNullOrEmpty(pageHeightNode.Value))
                                                        //{
                                                        //pageHeight = Convert.ToDouble(pageHeightNode.Value);
                                                        footNoteLinesText = GetFootNoteLinesText(footNoteLines);
                                                        //supScriptLineText.Append(SetFNoteTextInWordNode(supScrptLine, footNoteLinesText));
                                                        SetFNoteTextInWordNode(supScrptLine, footNoteLinesText, supWrd.Attributes["id"].Value, temp_FootNotesXmlDoc);
                                                        allFootNoteLines.AddRange(footNoteLines.Cast<XmlNode>().ToList());
                                                        //}
                                                    }
                                                }
                                            }
                                        }

                                        InsertFootNotesInSupScriptLine(mainXmlLines, supScrptLine, supScriptPage, yCoordOffset, supScriptWordsList);
                                        SetIdInFootNoteLines(allFootNoteLines, mainXmlLines, yCoordOffset, supScriptPage);

                                        // To Do
                                        //InsertFootNotesInSupScriptLine(mainXmlLines, supScrptLine, supScriptPage, supScriptLineText, yCoordOffset, supScriptId);
                                        //RemoveFootNotesFromBottom(allFootNoteLines, mainXmlLines, yCoordOffset, supScriptPage);
                                    }
                                }//end foreach superScipt words

                                temp_FootNotesXmlDoc.Save(tempXmlPath);
                                objGlobal.SaveXml();
                            }
                        }
                    }
                }
                else
                {
                    divParaText.InnerText = "";
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void InsertFootNotesInSupScriptLine(XmlNodeList mainXmlLines, XmlNode supScrptLine, int supScriptPage, double yCoordOffset,
                                                    List<XmlNode> supScriptWordsList)
        {
            List<XmlNode> subScriptLineInMainXml = null;
            double currentLinefontSize = 0;
            double prevLinefontSize = 0;
            string currentSecType = "";
            string prevSecType = "";

            bool isSpecialFootNote = false;

            List<XmlNode> pageLines = mainXmlLines.Cast<XmlNode>().Where(x => (x.Attributes["page"] != null &&
                                                                   Convert.ToInt32(x.Attributes["page"].Value).Equals(supScriptPage))).ToList();
            if (supScrptLine.PreviousSibling != null &&
                supScrptLine.PreviousSibling.ChildNodes.Count > 0 &&
                supScrptLine.PreviousSibling.ChildNodes[0].Attributes != null &&
                supScrptLine.PreviousSibling.ChildNodes[0].Attributes["type"] != null &&
                (supScrptLine.PreviousSibling.ChildNodes[0].Attributes["type"].Value.Equals("level1") ||
                 supScrptLine.PreviousSibling.ChildNodes[0].Attributes["type"].Value.Equals("level2") ||
                 supScrptLine.PreviousSibling.ChildNodes[0].Attributes["type"].Value.Equals("level3") ||
                 supScrptLine.PreviousSibling.ChildNodes[0].Attributes["type"].Value.Equals("level4") ||
                 supScrptLine.PreviousSibling.ChildNodes[0].Attributes["type"].Value.Equals("chapter")) &&
                supScrptLine.ChildNodes.Count > 0 &&
                supScrptLine.ChildNodes[0].Attributes != null &&
                supScrptLine.ChildNodes[0].Attributes["type"] != null &&
                (supScrptLine.ChildNodes[0].Attributes["type"].Value.Equals("level1") ||
                 supScrptLine.ChildNodes[0].Attributes["type"].Value.Equals("level2") ||
                 supScrptLine.ChildNodes[0].Attributes["type"].Value.Equals("level3") ||
                 supScrptLine.ChildNodes[0].Attributes["type"].Value.Equals("level4") ||
                 supScrptLine.ChildNodes[0].Attributes["type"].Value.Equals("chapter"))
                )
            {
                currentLinefontSize = Convert.ToDouble(supScrptLine.ChildNodes[0].Attributes["fontsize"].Value);
                currentSecType = supScrptLine.ChildNodes[0].Attributes["type"].Value;

                XmlNode prevLine = supScrptLine.PreviousSibling;
                XmlNode secSingleLine = null;

                if (prevLine != null && prevLine.ChildNodes.Count > 0 && prevLine.ChildNodes[0].Attributes != null &&
                    prevLine.ChildNodes[0].Attributes["fontsize"] != null)
                {
                    prevLinefontSize = Convert.ToDouble(prevLine.ChildNodes[0].Attributes["fontsize"].Value);
                    prevSecType = prevLine.ChildNodes[0].Attributes["type"].Value;

                    while (currentLinefontSize.Equals(prevLinefontSize) && currentSecType.Equals(prevSecType) && prevLine != null)
                    {
                        secSingleLine = prevLine;

                        prevLine = prevLine.PreviousSibling;

                        if (prevLine != null && prevLine.ChildNodes.Count > 0 &&
                            prevLine.ChildNodes[0].Attributes != null &&
                            prevLine.ChildNodes[0].Attributes["fontsize"] != null &&
                            prevLine.ChildNodes[0].Attributes["type"] != null &&
                            prevLine.ChildNodes[0].Attributes["type"].Value.Equals(currentSecType))
                        {
                            prevLinefontSize = Convert.ToDouble(prevLine.ChildNodes[0].Attributes["fontsize"].Value);
                        }
                    }
                }

                if (secSingleLine != null && secSingleLine.ChildNodes.Count > 0)
                {
                    subScriptLineInMainXml = pageLines.Where(x => (x.Attributes["top"] != null &&
                                                                                       secSingleLine.Attributes != null &&
                                                                                       secSingleLine.Attributes["y1"] !=
                                                                                       null &&
                                                                                       x.Attributes["page"] != null) &&

                                                                                      Convert.ToInt32(
                                                                                          x.Attributes["page"].Value)
                                                                                          .Equals(supScriptPage)

                                                                                      &&

                                                                                      (
                                                                                          (Math.Abs(
                                                                                              Math.Floor(
                                                                                                  Convert.ToDouble(
                                                                                                      x.Attributes["top"
                                                                                                          ].Value)) -
                                                                                              Math.Floor(
                                                                                                  Convert.ToDouble(
                                                                                                      secSingleLine
                                                                                                          .Attributes[
                                                                                                              "y1"]
                                                                                                          .Value))) <
                                                                                           yCoordOffset)

                                                                                          ||

                                                                                          (secSingleLine.Attributes[
                                                                                              "supScrLowerY"] != null &&
                                                                                           Math.Abs(
                                                                                               Math.Floor(
                                                                                                   Convert.ToDouble(
                                                                                                       x.Attributes[
                                                                                                           "top"].Value)) -
                                                                                               Math.Floor(
                                                                                                   Convert.ToDouble(
                                                                                                       secSingleLine
                                                                                                           .Attributes[
                                                                                                               "supScrLowerY"
                                                                                                           ].Value))) <
                                                                                           yCoordOffset)

                                                                                          ||

                                                                                          (secSingleLine.Attributes[
                                                                                              "supScrUpperY"] != null &&
                                                                                           Math.Abs(
                                                                                               Math.Floor(
                                                                                                   Convert.ToDouble(
                                                                                                       x.Attributes[
                                                                                                           "top"].Value)) -
                                                                                               Math.Floor(
                                                                                                   Convert.ToDouble(
                                                                                                       secSingleLine
                                                                                                           .Attributes[
                                                                                                               "supScrUpperY"
                                                                                                           ].Value))) <
                                                                                           yCoordOffset)

                                                                                          ||

                                                                                          (secSingleLine.Attributes[
                                                                                              "supScrUpperY"] != null &&
                                                                                           x.Attributes["coord"].Value
                                                                                               .Split(':').Length > 2 &&
                                                                                           Math.Abs(
                                                                                               Math.Floor(
                                                                                                   Convert.ToDouble(
                                                                                                       x.Attributes[
                                                                                                           "coord"]
                                                                                                           .Value.Split(
                                                                                                               ':')[3])) -
                                                                                               Math.Floor(
                                                                                                   Convert.ToDouble(
                                                                                                       secSingleLine
                                                                                                           .Attributes[
                                                                                                               "supScrUpperY"
                                                                                                           ].Value))) <
                                                                                           yCoordOffset)

                                                                                          ||

                                                                                          (secSingleLine.Attributes[
                                                                                              "supScrLowerY"] != null &&
                                                                                           x.Attributes["coord"].Value
                                                                                               .Split(':').Length > 2 &&
                                                                                           Math.Abs(
                                                                                               Math.Floor(
                                                                                                   Convert.ToDouble(
                                                                                                       x.Attributes[
                                                                                                           "coord"]
                                                                                                           .Value.Split(
                                                                                                               ':')[3])) -
                                                                                               Math.Floor(
                                                                                                   Convert.ToDouble(
                                                                                                       secSingleLine
                                                                                                           .Attributes[
                                                                                                               "supScrLowerY"
                                                                                                           ].Value))) <
                                                                                           yCoordOffset)
                                                                                          )
                        ).ToList();

                    //if (subScriptLineInMainXml.Count > 0)
                    //    isSpecialFootNote = true;
                }
            }

            else
            {
                //Setting footnote tag in subScrtipt line 
                subScriptLineInMainXml = pageLines.Where(x => (x.Attributes["top"] != null &&
                                                               supScrptLine.Attributes != null &&
                                                               supScrptLine.Attributes["y1"] != null &&
                                                               x.Attributes["page"] != null) &&
                    //supScrptLine.Attributes["supScrLowerY"] != null &&
                    //supScrptLine.Attributes["supScrUpperY"] != null &&
                                                              Convert.ToInt32(x.Attributes["page"].Value)
                                                                  .Equals(supScriptPage)

                                                              &&

                                                              (
                                                                  (Math.Abs(
                                                                      Math.Floor(
                                                                          Convert.ToDouble(x.Attributes["top"].Value)) -
                                                                      Math.Floor(
                                                                          Convert.ToDouble(
                                                                              supScrptLine.Attributes["y1"].Value))) <
                                                                   yCoordOffset)

                                                                  ||

                                                                  (supScrptLine.Attributes["supScrLowerY"] != null &&
                                                                   Math.Abs(
                                                                       Math.Floor(
                                                                           Convert.ToDouble(x.Attributes["top"].Value)) -
                                                                       Math.Floor(
                                                                           Convert.ToDouble(
                                                                               supScrptLine.Attributes["supScrLowerY"]
                                                                                   .Value))) < yCoordOffset)

                                                                      // ||

                                                                      //(Math.Floor(Convert.ToDouble(x.Attributes["top"].Value)) - 
                    // Math.Floor(Convert.ToDouble(supScrptLine.Attributes["supScrLowerY"].Value)) < yCoordOffset)

                                                                      //||

                                                                      //(Math.Floor(Convert.ToDouble(x.Attributes["top"].Value)) - 
                    // Math.Floor(Convert.ToDouble(supScrptLine.Attributes["supScrUpperY"].Value)) < yCoordOffset)

                                                                  ||

                                                                  (supScrptLine.Attributes["supScrUpperY"] != null &&
                                                                   Math.Abs(
                                                                       Math.Floor(
                                                                           Convert.ToDouble(x.Attributes["top"].Value)) -
                                                                       Math.Floor(
                                                                           Convert.ToDouble(
                                                                               supScrptLine.Attributes["supScrUpperY"]
                                                                                   .Value))) < yCoordOffset)

                                                                  ||

                                                                  (supScrptLine.Attributes["supScrUpperY"] != null &&
                                                                   x.Attributes["coord"].Value.Split(':').Length > 2 &&
                                                                   Math.Abs(
                                                                       Math.Floor(
                                                                           Convert.ToDouble(
                                                                               x.Attributes["coord"].Value.Split(':')[3])) -
                                                                       Math.Floor(
                                                                           Convert.ToDouble(
                                                                               supScrptLine.Attributes["supScrUpperY"]
                                                                                   .Value))) < yCoordOffset)

                                                                  ||

                                                                  (supScrptLine.Attributes["supScrLowerY"] != null &&
                                                                   x.Attributes["coord"].Value.Split(':').Length > 2 &&
                                                                   Math.Abs(
                                                                       Math.Floor(
                                                                           Convert.ToDouble(
                                                                               x.Attributes["coord"].Value.Split(':')[3])) -
                                                                       Math.Floor(
                                                                           Convert.ToDouble(
                                                                               supScrptLine.Attributes["supScrLowerY"]
                                                                                   .Value))) < yCoordOffset)
                                                                  )
                    ).ToList();
            }

            if (subScriptLineInMainXml != null &&
                subScriptLineInMainXml.Count > 0 &&
                supScrptLine.ChildNodes.Count > 0)
            {
                string finalText = GetSupScrLineWithFN(supScrptLine, supScriptPage, subScriptLineInMainXml);
                foreach (XmlNode supScrtLine in subScriptLineInMainXml)
                {
                    supScrtLine.InnerXml = finalText.Trim().Replace("&", "&amp;");
                    //supScrtLine.ParentNode.InsertAfter(supScrtLine, supScrtLine);
                }

                //To do sup script
                InsertSupScriptWord(supScriptPage, subScriptLineInMainXml, supScriptWordsList);

                //Remove attributes from Temp_FootNotes.xml
                var supWordList = supScrptLine.ChildNodes.Cast<XmlNode>().Where(x => (x.Attributes["wordtype"] != null && x.Attributes["id"] != null)).ToList();
                foreach (XmlNode scWord in supWordList)
                {
                    ((XmlElement)scWord).RemoveAttribute("wordtype");
                    ((XmlElement)scWord).RemoveAttribute("id");
                }
            }//end if
        }

        private void InsertSupScriptWord(int supScriptPage, List<XmlNode> subScriptLineInMainXml, List<XmlNode> supScriptWordsList)
        {
            if (subScriptLineInMainXml.Count > 0 && supScriptWordsList.Count > 0)
            {
                foreach (XmlNode supScrptWord in supScriptWordsList)
                {
                    if (supScrptWord.Attributes != null &&
                        supScrptWord.Attributes["x1"] != null &&
                        supScrptWord.Attributes["x"] != null &&
                        supScrptWord.Attributes["y"] != null &&
                        supScrptWord.Attributes["fontsize"] != null &&
                        supScrptWord.Attributes["font"] != null &&
                        supScrptWord.Attributes["fonttype"] != null &&
                        supScrptWord.Attributes["wordtype"] != null &&
                        supScrptWord.Attributes["id"] != null)
                    {
                        XmlElement ln = objGlobal.PBPDocument.CreateElement("ln");

                        ln.SetAttribute("coord", supScrptWord.Attributes["x1"].Value + ":" +
                                                 supScrptWord.Attributes["y1"].Value + ":" +
                                                 supScrptWord.Attributes["x"].Value + ":" +
                                                 supScrptWord.Attributes["y"].Value);
                        ln.SetAttribute("page", Convert.ToString(supScriptPage));
                        ln.SetAttribute("height", "0");
                        ln.SetAttribute("left", supScrptWord.Attributes["x1"].Value);
                        ln.SetAttribute("top", supScrptWord.Attributes["y1"].Value);
                        ln.SetAttribute("font", supScrptWord.Attributes["font"].Value);
                        ln.SetAttribute("fontsize", supScrptWord.Attributes["fontsize"].Value);
                        ln.SetAttribute("error", "0");
                        ln.SetAttribute("isUserSigned", "0");
                        ln.SetAttribute("isEditted", "false");
                        ln.SetAttribute("ispreviewpassed", "false");
                        ln.SetAttribute("wordtype", "supScript");
                        ln.InnerText = supScrptWord.Attributes["id"].Value.Trim();
                        subScriptLineInMainXml[0].ParentNode.InsertAfter(ln, subScriptLineInMainXml[0]);
                    }
                }
            }
        }

        private string GetSupScrLineWithFN(XmlNode supScrptLine, int supScriptPage, List<XmlNode> subScriptLineInMainXml)
        {
            XmlNodeList supScriptWordsInLine = supScrptLine.SelectNodes("descendant::Word");

            if (supScriptWordsInLine != null && supScriptWordsInLine.Count > 0)
            {
                bool isEmphasisType = false;
                bool isFirstEmphasisWord = true;

                StringBuilder finalFootNoteLine = new StringBuilder();

                for (int i = 0; i < supScriptWordsInLine.Count; i++)
                {
                    if (supScriptWordsInLine[i].Attributes != null &&
                            supScriptWordsInLine[i].Attributes["type"] != null &&
                            supScriptWordsInLine[i].Attributes["type"].Value.Equals("italic") ||
                            supScriptWordsInLine[i].Attributes["type"].Value.Equals("bold") ||
                            supScriptWordsInLine[i].Attributes["type"].Value.Equals("bold-italic"))
                    {
                        isEmphasisType = true;
                    }
                    else
                    {
                        isEmphasisType = false;
                    }
                    
                    if (supScriptWordsInLine[i] != null && supScriptWordsInLine[i].Attributes != null &&
                        supScriptWordsInLine[i].Attributes["wordtype"] != null &&
                        supScriptWordsInLine[i].Attributes["id"] != null)
                    {
                        if (supScriptWordsInLine[i].Attributes["specialFootNote"] != null &&
                            subScriptLineInMainXml[0].ParentNode != null &&
                            subScriptLineInMainXml[0].ParentNode.Attributes != null &&
                            subScriptLineInMainXml[0].ParentNode.Attributes["id"] != null &&
                            subScriptLineInMainXml[0].ParentNode.Attributes["pnum"] != null)
                        {
                            XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("upara");

                            ((XmlElement)convertedNode).SetAttribute("id",
                                subScriptLineInMainXml[0].ParentNode.Attributes["id"].Value);
                            ((XmlElement)convertedNode).SetAttribute("pnum",
                                subScriptLineInMainXml[0].ParentNode.Attributes["pnum"].Value);
                            //((XmlElement)convertedNode).SetAttribute("text-indent", subScriptLineInMainXml[0].ParentNode.Attributes["text-indent"].Value);
                            //((XmlElement)convertedNode).SetAttribute("padding-bottom", subScriptLineInMainXml[0].ParentNode.Attributes["padding-bottom"].Value);

                            XmlElement ln = objGlobal.PBPDocument.CreateElement("ln");

                            ln.SetAttribute("coord", "0:0:0:0");
                            ln.SetAttribute("page", Convert.ToString(supScriptPage));
                            ln.SetAttribute("height", "0");
                            ln.SetAttribute("font", "");
                            ln.SetAttribute("fontsize", "0");
                            ln.SetAttribute("error", "0");
                            ln.SetAttribute("isUserSigned", "0");
                            ln.SetAttribute("isEditted", "false");
                            ln.SetAttribute("ispreviewpassed", "false");

                            ln.InnerText = "[* " + supScriptWordsInLine[i].InnerText.Trim() + "]";

                            convertedNode.AppendChild(ln);

                            //convertedNode.InnerText = "[* " + supScriptLineText + "]";

                            if (subScriptLineInMainXml[0].ParentNode != null &&
                                subScriptLineInMainXml[0].ParentNode.ParentNode != null)
                            {
                                subScriptLineInMainXml[0].ParentNode.ParentNode.InsertAfter(convertedNode,
                                    subScriptLineInMainXml[0].ParentNode);
                                //objGlobal.SaveXml();
                            }

                            //foreach (XmlNode supScrtLine in subScriptLineInMainXml)
                            //{
                            //    supScrtLine.InnerXml = supScrtLine.InnerXml.Replace(word.Attributes["id"].Value, "*");
                            //}

                            finalFootNoteLine.Append("*" + " ");
                        }
                        else if (supScriptWordsInLine[i].Attributes["specialFootNoteInSec"] != null &&
                                 subScriptLineInMainXml[0].ParentNode != null &&
                                 (subScriptLineInMainXml[0].ParentNode.Name.Equals("section-title") ||
                                 subScriptLineInMainXml[0].ParentNode.Name.Equals("prefix")))
                        {
                            XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("upara");

                            ((XmlElement)convertedNode).SetAttribute("id", "0");
                            ((XmlElement)convertedNode).SetAttribute("pnum", "0");

                            convertedNode.InnerText = "[* " + supScriptWordsInLine[i].InnerText.Trim() + "]";

                            if (subScriptLineInMainXml[0].ParentNode != null &&
                                subScriptLineInMainXml[0].ParentNode.ParentNode != null &&
                                subScriptLineInMainXml[0].ParentNode.ParentNode.ParentNode != null &&
                                subScriptLineInMainXml[0].ParentNode.ParentNode.ParentNode.Name.Equals("section") &&
                                subScriptLineInMainXml[0].ParentNode.ParentNode.NextSibling != null &&
                                subScriptLineInMainXml[0].ParentNode.ParentNode.NextSibling.FirstChild != null &&
                                subScriptLineInMainXml[0].ParentNode.ParentNode.NextSibling.FirstChild.ParentNode != null &&
                                subScriptLineInMainXml[0].ParentNode.ParentNode.NextSibling.Name.Equals("body"))
                            {
                                subScriptLineInMainXml[0].ParentNode.ParentNode.NextSibling.FirstChild.ParentNode.InsertBefore(
                                    convertedNode,
                                    subScriptLineInMainXml[0].ParentNode.ParentNode.NextSibling.FirstChild);
                            }

                            //foreach (XmlNode supScrtLine in subScriptLineInMainXml)
                            //{
                            //    supScrtLine.InnerXml = supScrtLine.InnerXml.Replace(word.Attributes["id"].Value, "*");
                            //}

                            finalFootNoteLine.Append("*" + " ");
                        }
                        //else if (!isEmphasisType &&
                        //     i - 1 >= 0 &&
                        //   (supScriptWordsInLine[i - 1].Attributes["type"].Value.Equals("italic") ||
                        //   supScriptWordsInLine[i - 1].Attributes["type"].Value.Equals("bold") ||
                        //   supScriptWordsInLine[i - 1].Attributes["type"].Value.Equals("bold-italic")))
                        //{
                        //    finalFootNoteLine.Append(supScriptWordsInLine[i].InnerText.Trim() + "</emphasis>" + " ");
                        //}
                        else
                        {
                            //foreach (XmlNode supScrtLine in subScriptLineInMainXml)
                            //{
                            //    supScrtLine.InnerXml = word.InnerText.Trim().Replace("&", "&amp;");
                            //}

                            if (!isEmphasisType &&
                             i - 1 >= 0 &&
                           (supScriptWordsInLine[i - 1].Attributes["type"].Value.Equals("italic") ||
                           supScriptWordsInLine[i - 1].Attributes["type"].Value.Equals("bold") ||
                           supScriptWordsInLine[i - 1].Attributes["type"].Value.Equals("bold-italic")))
                            {
                                finalFootNoteLine.Append("</emphasis>" + " ");
                            }

                            finalFootNoteLine.Append(supScriptWordsInLine[i].InnerText.Trim() + " ");
                        }
                    }
                    else
                    {
                       if (isEmphasisType)
                        {
                            if (isFirstEmphasisWord)
                            {
                                finalFootNoteLine.Append("<emphasis type=\"italic\">" + supScriptWordsInLine[i].InnerText.Trim() + " ");
                                isFirstEmphasisWord = false;
                            }
                            else if (!isFirstEmphasisWord && i == supScriptWordsInLine.Count - 1)
                            {
                                finalFootNoteLine.Append(supScriptWordsInLine[i].InnerText.Trim() + "</emphasis>" + " ");
                            }
                            else
                            {
                                finalFootNoteLine.Append(supScriptWordsInLine[i].InnerText.Trim() + " ");
                            }
                        }
                        else if (!isEmphasisType &&
                                  i - 1 >= 0 &&
                                (supScriptWordsInLine[i - 1].Attributes["type"].Value.Equals("italic") ||
                                supScriptWordsInLine[i - 1].Attributes["type"].Value.Equals("bold") ||
                                supScriptWordsInLine[i - 1].Attributes["type"].Value.Equals("bold-italic")))
                        {
                            finalFootNoteLine.Append(supScriptWordsInLine[i].InnerText.Trim() + "</emphasis>" + " ");
                            isFirstEmphasisWord = true;
                        }
                        else
                        {
                            finalFootNoteLine.Append(supScriptWordsInLine[i].InnerText.Trim() + " ");
                        }
                    }
                } //end foreach

                return Convert.ToString(finalFootNoteLine);
            }

            return null;
        }

        public bool ContainsUrl(string footNoteText)
        {
            //string mystring = "My text and url http://www.google.com The end.";

            Regex urlRx = new Regex(@"(?<url>(http:[/][/]|www.)([a-z]|[A-Z]|[0-9]|[/.]|[~])*)", RegexOptions.IgnoreCase);

            MatchCollection matches = urlRx.Matches(footNoteText);

            if (matches.Count > 0)
                return true;

            //foreach (Match match in matches)
            //{
            //    var url = match.Groups["url"].Value;
            //    //footNoteText = footNoteText.Replace(url, string.Format("<a href=\"{0}\">{0}</a>", url));
            //}

            return false;
        }

        public bool IsSectionTypeLine(XmlNode supScrptLine)
        {
            if (supScrptLine != null &&
                supScrptLine.ChildNodes.Count > 0 &&
                supScrptLine.ChildNodes[0].Attributes != null &&
                supScrptLine.ChildNodes[0].Attributes["type"] != null &&
                (supScrptLine.ChildNodes[0].Attributes["type"].Value.Equals("level1") ||
                supScrptLine.ChildNodes[0].Attributes["type"].Value.Equals("level2") ||
                supScrptLine.ChildNodes[0].Attributes["type"].Value.Equals("level3") ||
                supScrptLine.ChildNodes[0].Attributes["type"].Value.Equals("level4") ||
                supScrptLine.ChildNodes[0].Attributes["type"].Value.Equals("chapter")))
            {
                return true;
            }

            return false;
        }

        private void SetIdInFootNoteLines(List<XmlNode> footNoteLines, XmlNodeList mainXmlLines, double yCoordOffset, int supScriptPage)
        {
            List<XmlNode> pageLines = new List<XmlNode>();

            if (footNoteLines.Count > 0 && footNoteLines[footNoteLines.Count - 1].ParentNode != null &&
                                       footNoteLines[footNoteLines.Count - 1].ParentNode.ParentNode != null &&
                                       footNoteLines[footNoteLines.Count - 1].ParentNode.ParentNode.Attributes != null &&
                                       footNoteLines[footNoteLines.Count - 1].ParentNode.ParentNode.Attributes["number"] != null &&
                                      Convert.ToInt32(footNoteLines[footNoteLines.Count - 1].ParentNode.ParentNode.Attributes["number"].Value) > supScriptPage)
            {
                pageLines = mainXmlLines.Cast<XmlNode>().Where(x => (x.Attributes["page"] != null &&
                                                                 Convert.ToInt32(x.Attributes["page"].Value).Equals(supScriptPage) ||
                                                                 Convert.ToInt32(x.Attributes["page"].Value).Equals(supScriptPage + 1))).ToList();
            }
            else
            {
                pageLines = mainXmlLines.Cast<XmlNode>().Where(x => (x.Attributes["page"] != null &&
                                                                  Convert.ToInt32(x.Attributes["page"].Value).Equals(supScriptPage))).ToList();
            }

            int counter = 0;

            foreach (XmlNode fNoteLine in footNoteLines)
            {
                if (fNoteLine.Attributes != null &&
                    fNoteLine.Attributes["y1"] != null &&
                    fNoteLine.Attributes["id"] != null &&
                    fNoteLine.ParentNode != null &&
                    fNoteLine.ParentNode.ParentNode != null &&
                    fNoteLine.ParentNode.ParentNode.Attributes != null &&
                    fNoteLine.ParentNode.ParentNode.Attributes["number"] != null)
                {
                    List<XmlNode> mainXmlLine = pageLines.Where(x =>
                                                            (
                                                                x.Attributes["top"] != null &&
                                                                x.Attributes["page"] != null &&
                                                                (
                                                                    (Math.Floor(Convert.ToDouble(x.Attributes["top"].Value))
                                                                    .Equals(Math.Floor(Convert.ToDouble(fNoteLine.Attributes["y1"].Value))))

                                                                    ||

                                                                    (Math.Floor(Convert.ToDouble(x.Attributes["top"].Value))
                                                                    .Equals(Math.Floor(Convert.ToDouble(fNoteLine.Attributes["y1"].Value) + yCoordOffset)))
                                                                )
                                                            )

                                                        ).ToList();

                    if (mainXmlLine.Count > 0)
                    {
                        foreach (XmlNode fNLine in mainXmlLine)
                        {
                            if (fNLine != null &&
                                fNLine.ParentNode != null &&
                                fNLine.ParentNode.Name.Equals("upara"))
                            {
                                if (counter == 0)
                                {
                                    XmlAttribute attrpType = objGlobal.PBPDocument.CreateAttribute("lType");
                                    attrpType.Value = "footNote";
                                    fNLine.Attributes.Append(attrpType);
                                }

                                XmlAttribute attrFid = objGlobal.PBPDocument.CreateAttribute("footNoteId");
                                attrFid.Value = fNoteLine.Attributes["id"].Value;
                                fNLine.Attributes.Append(attrFid);

                                counter++;

                                //if (((XmlElement)fNLine.ParentNode).HasAttribute("abnormalLeft"))
                                //    fNLine.ParentNode.Attributes.RemoveNamedItem("abnormalLeft");

                                //if (((XmlElement)fNLine.ParentNode).HasAttribute("abnormalRight"))
                                //    fNLine.ParentNode.Attributes.RemoveNamedItem("abnormalRight");

                                //if (((XmlElement)fNLine.ParentNode).HasAttribute("pType"))
                                //    fNLine.ParentNode.Attributes.RemoveNamedItem("pType");

                                //if (((XmlElement)fNLine.ParentNode).HasAttribute("normalX"))
                                //    fNLine.ParentNode.Attributes.RemoveNamedItem("normalX");

                                //if (((XmlElement)fNLine.ParentNode).HasAttribute("normalIndX"))
                                //    fNLine.ParentNode.Attributes.RemoveNamedItem("normalIndX");

                                //if (((XmlElement)fNLine.ParentNode).HasAttribute("pageNum"))
                                //    fNLine.ParentNode.Attributes.RemoveNamedItem("pageNum");

                                //if (((XmlElement)fNLine.ParentNode).HasAttribute("pageType"))
                                //    fNLine.ParentNode.Attributes.RemoveNamedItem("pageType");

                                //if (((XmlElement)fNLine.ParentNode).HasAttribute("conversion-Operations"))
                                //    fNLine.ParentNode.Attributes.RemoveNamedItem("conversion-Operations");

                                if (((XmlElement)fNLine.ParentNode).HasAttribute("abnormalLeft"))
                                    fNLine.ParentNode.Attributes.RemoveNamedItem("abnormalLeft");

                                if (((XmlElement)fNLine.ParentNode).HasAttribute("abnormalRight"))
                                    fNLine.ParentNode.Attributes.RemoveNamedItem("abnormalRight");

                                if (((XmlElement)fNLine.ParentNode).HasAttribute("normalX"))
                                    fNLine.ParentNode.Attributes.RemoveNamedItem("normalX");

                                if (((XmlElement)fNLine.ParentNode).HasAttribute("normalIndX"))
                                    fNLine.ParentNode.Attributes.RemoveNamedItem("normalIndX");

                                if (((XmlElement)fNLine.ParentNode).HasAttribute("normalTop"))
                                    fNLine.ParentNode.Attributes.RemoveNamedItem("normalTop");

                                if (((XmlElement)fNLine.ParentNode).HasAttribute("normalBottom"))
                                    fNLine.ParentNode.Attributes.RemoveNamedItem("normalBottom");

                                if (((XmlElement)fNLine.ParentNode).HasAttribute("normalY"))
                                    fNLine.ParentNode.Attributes.RemoveNamedItem("normalY");

                                if (((XmlElement)fNLine.ParentNode).HasAttribute("pageNum"))
                                    fNLine.ParentNode.Attributes.RemoveNamedItem("pageNum");

                                if (((XmlElement)fNLine.ParentNode).HasAttribute("pageType"))
                                    fNLine.ParentNode.Attributes.RemoveNamedItem("pageType");

                                if (((XmlElement)fNLine.ParentNode).HasAttribute("font"))
                                    fNLine.ParentNode.Attributes.RemoveNamedItem("font");

                                if (((XmlElement)fNLine.ParentNode).HasAttribute("fontSize"))
                                    fNLine.ParentNode.Attributes.RemoveNamedItem("fontSize");

                                if (((XmlElement)fNLine.ParentNode).HasAttribute("conversion-Operations"))
                                    fNLine.ParentNode.Attributes.RemoveNamedItem("conversion-Operations");
                            }
                        }
                    }
                }
            }//end foreach temp_Footnotes
        }

        private string GetFootNoteLinesText(XmlNodeList footNoteLines)
        {
            StringBuilder footNoteLinesText = new StringBuilder();

            foreach (XmlNode ln in footNoteLines)
            {
                XmlNodeList footNoteWord = ln.SelectNodes("descendant::Word");

                if (footNoteWord != null && footNoteWord.Count > 0)
                {
                    foreach (XmlNode word in footNoteWord)
                    {
                        footNoteLinesText.Append(word.InnerText.Trim() + " ");
                    }
                }
            }

            return Convert.ToString(footNoteLinesText);
        }

        public XmlNode GetSupScriptWordNode(XmlNode firstSupScriptLine)
        {
            if (firstSupScriptLine != null)
            {
                XmlNodeList superScriptWrd = firstSupScriptLine.SelectNodes("descendant::Word");

                if (superScriptWrd != null && superScriptWrd.Count > 0)
                {
                    foreach (XmlNode word in superScriptWrd)
                    {
                        //if (word.Attributes["wordtype"] != null && word.Attributes["id"] != null && word.InnerText.Trim().Length == 1) return word;

                        if (word != null)
                        {
                            if (word.Attributes != null && word.Attributes["wordtype"] != null && word.Attributes["id"] != null) return word;
                        }
                    }
                }
            }
            return null;
        }

        public List<XmlNode> GetSupScriptWordsFromLine(XmlNode firstSupScriptLine)
        {
            List<XmlNode> subScripWordList = null;

            if (firstSupScriptLine != null)
            {
                subScripWordList = new List<XmlNode>();

                XmlNodeList superScriptWrd = firstSupScriptLine.SelectNodes("descendant::Word");

                if (superScriptWrd != null && superScriptWrd.Count > 0)
                {
                    foreach (XmlNode word in superScriptWrd)
                    {
                        //if (word.Attributes["wordtype"] != null && word.Attributes["id"] != null && word.InnerText.Trim().Length == 1) return word;

                        if (word != null)
                        {
                            if (word.Attributes != null &&
                                word.Attributes["wordtype"] != null &&
                                word.Attributes["id"] != null)
                            {
                                subScripWordList.Add(word);
                            }
                        }
                    }
                }
            }
            return subScripWordList;
        }

        public bool SetFNoteTextInWordNode(XmlNode supScrptLine, string footNoteLinesText, string supScriptId, XmlDocument temp_FootNotesXmlDoc)
        {
            StringBuilder supScriptLineText = null;
            bool isSpecialFN = false;
            bool isSpecialFNInSec = false;
            bool status = true;

            try
            {
                if (ContainsUrl(Convert.ToString(footNoteLinesText)))
                    isSpecialFN = true;

                if (IsSectionTypeLine(supScrptLine))
                    isSpecialFNInSec = true;

                if (supScrptLine != null)
                {
                    supScriptLineText = new StringBuilder();
                    XmlNodeList supScriptWord = supScrptLine.SelectNodes("descendant::Word");

                    if (supScriptWord != null && supScriptWord.Count > 0)
                    {
                        foreach (XmlNode word in supScriptWord)
                        {
                            if (word != null && word.Attributes != null &&
                                word.Attributes["wordtype"] != null &&
                                word.Attributes["id"] != null &&
                                word.Attributes["id"].Value.Equals(supScriptId))
                            {
                                var textList = footNoteLinesText.Split(' ').ToList();
                                if (textList.Count > 0)
                                {
                                    textList.RemoveAt(0);

                                    string finalText = string.Join(" ", textList.ToArray());

                                    if (isSpecialFN)
                                    {
                                        word.InnerText = finalText;

                                        XmlAttribute newAttr = temp_FootNotesXmlDoc.CreateAttribute("specialFootNote");
                                        newAttr.Value = "true";
                                        word.Attributes.Append(newAttr);
                                        break;
                                    }
                                    else if (isSpecialFNInSec)
                                    {
                                        word.InnerText = finalText;

                                        XmlAttribute newAttr = temp_FootNotesXmlDoc.CreateAttribute("specialFootNoteInSec");
                                        newAttr.Value = "true";
                                        word.Attributes.Append(newAttr);
                                        break;
                                    }
                                    else
                                    {
                                        supScriptLineText.Append("<footnote id='" + supScriptId + "'>" + HttpUtility.HtmlEncode(finalText) + "</footnote>");
                                        word.InnerText = Convert.ToString(supScriptLineText);
                                        break;
                                    }
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
            return status;
        }

        #endregion

        #region Box

        public List<XmlNode> GetBoxPara(XmlNodeList allParas)
        {
            // return null;

            if (allParas == null || allParas.Count == 0) return null;

            List<XmlNode> abnomalParas = new List<XmlNode>();
            bool isBoxPara = false;
            int counter = 0;

            try
            {
                if (BoxContainingPages == null || BoxContainingPages.Count == 0) return null;

                foreach (int page in BoxContainingPages)
                {
                    //if (!BoxConvertedPagesList.Contains(page))
                    //{
                    //    BoxConvertedPagesList.Add(page);

                    var boxParaText = GetBoxText(page);

                    if (boxParaText != null || boxParaText.Count > 0)
                    {
                        List<XmlNode> pageAllParaList = allParas.Cast<XmlNode>().Where(x => x.ChildNodes != null &&
                                                                                            x.ChildNodes.Count > 0 &&
                                                                                            x.ChildNodes[0].Attributes[
                                                                                                "page"] != null &&
                                                                                            Convert.ToInt32(
                                                                                                x.ChildNodes[0]
                                                                                                    .Attributes["page"]
                                                                                                    .Value) == page)
                            .ToList();

                        foreach (XmlNode abnormalNode in pageAllParaList)
                        {
                            isBoxPara = false;

                            if (abnormalNode.ParentNode != null &&
                                !abnormalNode.ParentNode.Name.Equals("box") &&
                                abnormalNode.ChildNodes != null &&
                                abnormalNode.ChildNodes.Count > 0 &&
                                abnormalNode.ChildNodes[0].Attributes["page"] != null &&
                                abnormalNode.ChildNodes[0].Attributes["page"].Value == Convert.ToString(page))
                            {
                                if (abnormalNode.ChildNodes.Count > 0)
                                {
                                    if (BoxContainingPages.Contains(page))
                                    {
                                        foreach (SvgBox boxLine in boxParaText)
                                        {
                                            List<string> boxLines =
                                                boxLine.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None)
                                                    .ToList();

                                            if (boxLines.Count > 0)
                                            {
                                                for (int i = 0; i < boxLines.Count; i++)
                                                {
                                                    if (IsBoxStartingLine(boxLines[i].Trim(), abnormalNode))
                                                    {
                                                        isBoxPara = true;
                                                        break;
                                                    }
                                                }
                                            }
                                        }

                                        if (isBoxPara)
                                        {
                                            counter++;

                                            if (counter == 1)
                                            {
                                                if (abnormalNode.ChildNodes != null &&
                                                    abnormalNode.ChildNodes.Count > 0 &&
                                                    abnormalNode.PreviousSibling != null &&
                                                    abnormalNode.PreviousSibling.ChildNodes != null &&
                                                    abnormalNode.PreviousSibling.ChildNodes.Count == 1 &&
                                                    (Math.Abs(Convert.ToDouble(
                                                        abnormalNode.PreviousSibling.ChildNodes[0].Attributes["left"]
                                                            .Value)
                                                              -
                                                              Convert.ToDouble(
                                                                  abnormalNode.ChildNodes[0].Attributes["left"].Value)) <
                                                     0.05) &&
                                                    abnormalNode.PreviousSibling.ChildNodes[0].Attributes["font"].Value
                                                        .Equals(abnormalNode.ChildNodes[0].Attributes["font"].Value) &&
                                                    Convert.ToDouble(
                                                        abnormalNode.PreviousSibling.ChildNodes[0].Attributes["fontsize"
                                                            ].Value)
                                                    >=
                                                    Convert.ToDouble(
                                                        abnormalNode.ChildNodes[0].Attributes["fontsize"].Value))
                                                {
                                                    abnomalParas.Add(abnormalNode.PreviousSibling);
                                                }

                                                abnomalParas.Add(abnormalNode);
                                            }
                                            else
                                                abnomalParas.Add(abnormalNode);
                                        }
                                    }
                                }
                            }
                        }

                        if (abnomalParas != null && abnomalParas.Count > 0) break;
                    }
                    //}
                }

                return abnomalParas;

            }
            catch (Exception)
            {
                return null;
            }
        }

        private void convertToBox(bool isApplyAll)
        {
            try
            {
                LoadPdfXml();

                string normalBgColor = GetPdfNormalBgColor();

                var boxBgColor = GetBoxBgColor(normalBgColor);

                if (BoxBgColors == null) BoxBgColors = new List<string>();

                BoxBgColors.Add(boxBgColor);

                List<XmlNode> allAbnParas = new List<XmlNode>();
                List<XmlNode> paraToConvert = new List<XmlNode>();

                //List<int> boxContainingPages = GetBoxContainingPage();
                //if (boxContainingPages == null || boxContainingPages.Count == 0) return;
                //BoxContainingPages = boxContainingPages;
                //BoxConvertedPagesList = BoxContainingPages;

                var allParas = objGlobal.PBPDocument.SelectNodes("//upara");

                //////paraToConvert = GetBoxPara(allParas);

                //if (SelectedXmlParaNodes != null && SelectedXmlParaNodes.Count > 0)
                //    paraToConvert = SelectedXmlParaNodes;

                ////if ((paraToConvert == null || paraToConvert.Count == 0) &&
                ////    (SelectedXmlParaNodes != null && SelectedXmlParaNodes.Count > 0) &&
                ////    (SelectedXmlParaNodes[0].ChildNodes != null && SelectedXmlParaNodes[0].ChildNodes.Count > 0))
                /// 

                ////if ((SelectedXmlParaNodes != null && SelectedXmlParaNodes.Count > 0) &&
                ////(SelectedXmlParaNodes[0].ChildNodes != null && SelectedXmlParaNodes[0].ChildNodes.Count > 0))
                ////{
                ////    paraToConvert = new List<XmlNode>();

                ////    int page = Convert.ToInt32(SelectedXmlParaNodes[0].ChildNodes[0].Attributes["page"].Value);
                ////    string coord = SelectedXmlParaNodes[0].ChildNodes[0].Attributes["coord"].Value;

                ////    List<XmlNode> pageAllParaList = allParas.Cast<XmlNode>().Where(x => x.ChildNodes != null &&
                ////                                                                          x.ChildNodes.Count > 0 &&
                ////                                                                          x.ChildNodes[0].Attributes[
                ////                                                                              "page"] != null &&
                ////                                                                          Convert.ToInt32(
                ////                                                                              x.ChildNodes[0]
                ////                                                                                  .Attributes["page"]
                ////                                                                                  .Value) == page &&
                ////                                                                          x.ChildNodes[0].Attributes[
                ////                                                                              "coord"] != null &&
                ////                                                                              x.ChildNodes[0]
                ////                                                                                  .Attributes["coord"]
                ////                                                                                  .Value == coord).ToList();

                ////    if (pageAllParaList.Count > 0)
                ////        paraToConvert = pageAllParaList;
                ////}

                if ((SelectedXmlParaNodes != null && SelectedXmlParaNodes.Count > 0) &&
                 (SelectedXmlParaNodes[0].ChildNodes != null && SelectedXmlParaNodes[0].ChildNodes.Count > 0))
                {
                    paraToConvert = new List<XmlNode>();

                    foreach (XmlNode box in SelectedXmlParaNodes)
                    {
                        if (box.ChildNodes.Count > 0)
                        {
                            int page = Convert.ToInt32(box.ChildNodes[0].Attributes["page"].Value);
                            string coord = box.ChildNodes[0].Attributes["coord"].Value;

                            List<XmlNode> pageAllParaList = allParas.Cast<XmlNode>().Where(x => x.ChildNodes != null &&
                                                                                                x.ChildNodes.Count > 0 &&
                                                                                                x.ChildNodes[0]
                                                                                                    .Attributes[
                                                                                                        "page"] != null &&
                                                                                                Convert.ToInt32(
                                                                                                    x.ChildNodes[0]
                                                                                                        .Attributes[
                                                                                                            "page"]
                                                                                                        .Value) == page &&
                                                                                                x.ChildNodes[0]
                                                                                                    .Attributes[
                                                                                                        "coord"] != null &&
                                                                                                x.ChildNodes[0]
                                                                                                    .Attributes["coord"]
                                                                                                    .Value == coord)
                                .ToList();

                            if (pageAllParaList.Count > 0)
                                paraToConvert.AddRange(pageAllParaList);
                        }
                    }
                }

                if (paraToConvert != null && paraToConvert.Count > 0)
                {
                    //if (isApplyAll)
                    //{
                    //    while (paraToConvert != null && paraToConvert.Count > 0)
                    //    {
                    //        CreateBoxXml(paraToConvert);
                    //        paraToConvert = GetBoxPara(allParas);
                    //    }
                    //}
                    //else
                    //{
                    ////CreateBoxXml(paraToConvert);
                    //}


                    if (isApplyAll)
                    {
                        string abnLeft = paraToConvert[0].Attributes["abnormalLeft"].Value;

                        int page = paraToConvert[0].SelectSingleNode("//upara[@abnormalLeft='" + abnLeft + "']/descendant::ln/@page") != null
                                ? Convert.ToInt32(paraToConvert[0].SelectSingleNode("//upara[@abnormalLeft='" + abnLeft + "']/descendant::ln/@page").Value)
                                : 0;

                        double fontSize = paraToConvert[0].SelectSingleNode("//upara[@abnormalLeft='" + abnLeft + "']/descendant::ln/@fontsize") != null
                                ? Convert.ToDouble(paraToConvert[0].SelectSingleNode("//upara[@abnormalLeft='" + abnLeft +
                                                                    "']/descendant::ln/@fontsize").Value)
                                : 0;

                        string font = paraToConvert[0].SelectSingleNode("//upara[@abnormalLeft='" + abnLeft + "']/descendant::ln/@font") != null
                                ? Convert.ToString(paraToConvert[0].SelectSingleNode("//upara[@abnormalLeft='" + abnLeft +
                                                                    "']/descendant::ln/@font").Value)
                                : "0";

                        if (page == 0) return;

                        bool isEven = page % 2 == 0 ? true : false;

                        var allMatchingNodes = objGlobal.PBPDocument.SelectNodes("//upara[@abnormalLeft='" + abnLeft + "']/descendant::ln[@font='" + font
                                                                    + "' and @fontsize='" + fontSize + "']/..").Cast<XmlNode>().ToList();
                        if (isEven)
                        {
                            foreach (XmlNode para in allMatchingNodes)
                            {
                                if (para.ChildNodes != null && para.ChildNodes.Count > 0)
                                {
                                    if (Convert.ToInt32(para.ChildNodes[0].Attributes["page"].Value) % 2 == 0)
                                        allAbnParas.Add(para);
                                }
                            }
                        }
                        else
                        {
                            foreach (XmlNode para in allMatchingNodes)
                            {
                                if (para.ChildNodes != null && para.ChildNodes.Count > 0)
                                {
                                    if (Convert.ToInt32(para.ChildNodes[0].Attributes["page"].Value) % 2 != 0)
                                        allAbnParas.Add(para);
                                }
                            }
                        }

                        BeforeComplexBitMappingList = allAbnParas.Select(x => x.Clone()).ToList();

                        CreateBoxXml(allAbnParas);
                    }
                    else
                    {
                        BeforeComplexBitMappingList = paraToConvert.Select(x => x.Clone()).ToList();

                        CreateBoxXml(paraToConvert);
                    }
                }
                else
                {

                    divParaText.InnerText = "";
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static bool IsAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9\s,]*$");
            return rg.IsMatch(strToCheck);
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

        public string GetPdfNormalBgColor()
        {
            try
            {
                string bookId = Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0];
                string pdfSvgXml = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/" + Convert.ToString(Request.QueryString["bid"]) + "_Svg.xml";

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(pdfSvgXml);

                List<ColorTypes> allColors = GetAllColors();

                //*[@page='2']/descendant::path

                //XmlNodeList pdfColorsList = xDoc.SelectNodes("//@fill");
                int page = 0;

                //XmlNodeList pageList = xDoc.SelectNodes("//@page");
                //XmlNodeList pdfColorsList = xDoc.SelectNodes("//*[@page='" +page + "']/descendant::path");

                //path[@fill!='none']
                //path[@stroke]

                //XmlNodeList boxBgColorList = xDoc.SelectNodes("//path[@fill!='none']/@fill");
                XmlNodeList pdfNormalBgColorList = xDoc.SelectNodes("//path[@stroke]/@stroke");

                var allColorTypes = pdfNormalBgColorList.Cast<XmlNode>().GroupBy(x => x.Value).Select(group => new { value = group.Key, Count = group.Count() });

                Dictionary<string, int> colorsList = new Dictionary<string, int>();

                foreach (var col in allColorTypes)
                {
                    if (!col.value.Equals("none"))
                    {
                        var matchedColor = allColors.Where(x =>
                                (x.Red == Convert.ToInt32(col.value.Replace("rgb(", "").Replace(")", "").Split(',')[0])) &&
                                (x.Green == Convert.ToInt32(col.value.Replace("rgb(", "").Replace(")", "").Split(',')[1])) &&
                                (x.Blue == Convert.ToInt32(col.value.Replace("rgb(", "").Replace(")", "").Split(',')[2]))).ToList();

                        if (matchedColor.Count > 0)
                        {
                            //foreach (ColorTypes matchColor in matchedColor)
                            //{
                            //    colorsList.Add(matchColor.ColorName, col.Count);
                            //}

                            colorsList.Add(matchedColor[0].ColorName, col.Count);
                        }
                    }
                }

                //var allBgColors = colorsList.GroupBy(x => x.Key).Select(group => new { value = group.Key, Count = group.Count() });
                //var result = colorsList.GroupBy(x => x.Key).Select(group => new { value = group.Key, Count = group.Count() }).FirstOrDefault();

                var result = colorsList.OrderByDescending(x => x.Key).FirstOrDefault();

                return result.Key;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public string GetBoxBgColor(string normalBgColor)
        {
            try
            {
                string bookId = Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0];
                string pdfSvgXml = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/" + Convert.ToString(Request.QueryString["bid"]) + "_Svg.xml";

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(pdfSvgXml);

                List<ColorTypes> allColors = GetAllColors();

                XmlNodeList boxBgColorList = xDoc.SelectNodes("//path[@fill!='none']/@fill");

                List<string> colorsList = new List<string>();

                foreach (XmlNode col in boxBgColorList)
                {
                    var matchedColor = allColors.Where(
                        x => (x.Red == Convert.ToInt32(col.Value.Replace("rgb(", "").Replace(")", "").Split(',')[0])) &&
                             (x.Green == Convert.ToInt32(col.Value.Replace("rgb(", "").Replace(")", "").Split(',')[1])) &&
                             (x.Blue == Convert.ToInt32(col.Value.Replace("rgb(", "").Replace(")", "").Split(',')[2])))
                        .ToList();

                    foreach (ColorTypes matchColor in matchedColor)
                    {
                        colorsList.Add(matchColor.ColorName);
                    }
                }

                var result = colorsList.GroupBy(x => x).Select(group => new { value = group.Key, Count = group.Count() }).FirstOrDefault();

                return result.value;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        public List<int> GetBoxContainingPage()
        {
            try
            {
                //var list = GetNoBgColorBoxPages();

                string bookId = Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0];
                string boxDirPath = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/";

                string pdfSvgXml = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/" +
                                              Convert.ToString(Request.QueryString["bid"]) + "_Svg.xml";

                if (!File.Exists(pdfSvgXml)) return null;

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(pdfSvgXml);

                List<int> boxContPages = new List<int>();

                //XmlNodeList colorBgPages = xDoc.SelectNodes("//path[@fill!='none']/../../../../@page");

                XmlNodeList colorBgPages = xDoc.SelectNodes("//path[@fill!='none' and @fill!='rgb(255,255,255)' and fill='rgb(0,0,0)']/../../../../@page");

                if (colorBgPages != null && colorBgPages.Count > 0)
                {
                    foreach (XmlNode page in colorBgPages)
                    {
                        if (!string.IsNullOrEmpty(page.Value))
                            boxContPages.Add(Convert.ToInt32(page.Value));
                    }
                }

                return boxContPages.Distinct().OrderBy(x => x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<int> GetNoBgColorBoxPages()
        {
            try
            {
                string bookId = Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0];
                string boxDirPath = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/";

                string pdfSvgXml = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/" +
                                              Convert.ToString(Request.QueryString["bid"]) + "_Svg.xml";

                if (!File.Exists(pdfSvgXml)) return null;

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(pdfSvgXml);

                List<int> boxContPages = new List<int>();

                List<double> yCoordList = new List<double>();

                //XmlNodeList colorBgPages = xDoc.SelectNodes("//path[@fill='none']");

                XmlNodeList noColorBgPages = xDoc.SelectNodes("//path[@fill='none']/ancestor::div[@page]");

                if (noColorBgPages != null && noColorBgPages.Count > 0)
                {
                    foreach (XmlNode div in noColorBgPages)
                    {
                        //var boxNodes = div.SelectNodes("descendant::path[@fill='none' and @stroke!='none' and @stroke!='rgb(255,255,255)']/@d")
                        //                  .Cast<XmlNode>().Where(x => x.InnerText.Trim().Split(' ').Length == 6).ToList();

                        var gNodes = div.SelectNodes("//g/descendant::path[@fill='none' and @stroke!='none' and @stroke!='rgb(255,255,255)']/..")
                                          .Cast<XmlNode>().Where(x => x.InnerText.Trim().Split(' ').Length == 6).ToList();

                        if (gNodes.Count > 0)
                        {
                            var list = gNodes.Where(
                                x =>
                                    x.ChildNodes.Count > 0 && x.ChildNodes[0].Name.Equals("path") &&
                                    x.ChildNodes[0].Attributes.Count > 0 &&
                                    x.ChildNodes[0].Attributes["d"] != null && x.ChildNodes[0].Attributes["d"].Value.Split(' ').Length == 6)
                                .Select(y => y.Attributes["d"].Value)
                                .ToList();

                            //foreach (XmlNode node in gNodes)
                            for (int i = 0; i < gNodes.Count; i++)
                            {
                                if (gNodes[i].Attributes != null &&
                                    gNodes[i].Attributes["transform"] != null &&
                                    gNodes[i].ChildNodes != null &&
                                    gNodes[i].ChildNodes.Count > 0 &&
                                    gNodes[i].ChildNodes[0].Name.Equals("path") &&
                                    gNodes[i].ChildNodes[0].Attributes != null &&
                                    gNodes[i].ChildNodes[0].Attributes.Count > 0 &&
                                    gNodes[i].ChildNodes[0].Attributes["d"] != null &&
                                    gNodes[i].ChildNodes[0].Attributes["d"].Value.Split(' ').Length == 6)
                                {
                                    if (i + 1 < gNodes.Count)
                                    {
                                        if (gNodes[i + 1].Attributes != null &&
                                            gNodes[i + 1].Attributes["transform"] != null &&
                                            gNodes[i + 1].ChildNodes != null &&
                                            gNodes[i + 1].ChildNodes.Count > 0 &&
                                            gNodes[i + 1].ChildNodes[0].Name.Equals("path") &&
                                            gNodes[i + 1].ChildNodes[0].Attributes != null &&
                                            gNodes[i + 1].ChildNodes[0].Attributes.Count > 0 &&
                                            gNodes[i + 1].ChildNodes[0].Attributes["d"] != null &&
                                            gNodes[i + 1].ChildNodes[0].Attributes["d"].Value.Split(' ').Length == 6)
                                        {

                                        }
                                    }
                                    //string yValue = gNodes[i].Attributes["transform"].Value.Replace("translate(", "").Replace(")", "").Split(' ')[1];

                                    //yCoordList.Add(yValue);
                                }
                            }
                        }
                    }
                }

                return boxContPages.Distinct().OrderBy(x => x).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public List<SvgBox> GetBoxText_(int page)
        {
            try
            {
                string bookId = Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0];
                string pdfSvgXml = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/" + Convert.ToString(Request.QueryString["bid"]) + "_Svg.xml";

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(pdfSvgXml);

                List<SvgBox> pathCoordList = new List<SvgBox>();
                List<SvgBox> boxList = new List<SvgBox>();

                XmlNode bgColorPage = xDoc.SelectSingleNode("//*[path[@fill!='none']/../../../../@page='" + page + "']/ancestor::div[@page='" + page + "']");

                if (bgColorPage == null) return null;

                XmlNodeList pathList = bgColorPage.SelectNodes("descendant::path[@fill!='none']/@d");

                if (pathList == null || pathList.Count == 0) return null;

                foreach (XmlNode path in pathList)
                {
                    List<string> coordList = Regex.Split(path.Value.Trim(), @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

                    if (coordList.Count > 10)
                    {
                        pathCoordList.Add(new SvgBox
                        {
                            TopYCoord = Convert.ToDouble(coordList[2]),
                            BottomYCoord = Convert.ToDouble(coordList[11])
                        });
                    }
                }

                double topYCoord = 0;
                double bottomYCoord = 0;

                for (int i = 0; i < pathCoordList.Count; i++)
                {
                    if (pathCoordList[i].TopYCoord > pathCoordList[i].BottomYCoord)
                    {
                        topYCoord = pathCoordList[i].TopYCoord;

                        while ((i + 1) < pathCoordList.Count && pathCoordList[i + 1].TopYCoord > pathCoordList[i].BottomYCoord)
                        {
                            i++;
                            bottomYCoord = pathCoordList[i].BottomYCoord;
                        }

                        boxList.Add(new SvgBox { TopYCoord = topYCoord, BottomYCoord = bottomYCoord });

                        topYCoord = 0;
                        bottomYCoord = 0;
                    }
                }

                XmlNodeList textList = bgColorPage.SelectNodes("descendant::text");
                StringBuilder sbBoxText = new StringBuilder();

                if (textList != null && textList.Count > 0)
                {
                    foreach (var box in boxList)
                    {
                        var boxText = textList.Cast<XmlNode>().Where(x => x.Attributes["transform"] != null &&
                                                                          x.Attributes["transform"].Value.Split(' ').Length > 4 &&
                                Convert.ToDouble(x.Attributes["transform"].Value.Split(' ')[5].Replace(")", "")) < box.TopYCoord &&
                                Convert.ToDouble(x.Attributes["transform"].Value.Split(' ')[5].Replace(")", "")) > box.BottomYCoord).ToList();

                        if (boxText.Count > 0)
                        {
                            foreach (XmlNode textNode in boxText)
                            {
                                foreach (XmlNode tspanNode in textNode.ChildNodes)
                                {
                                    sbBoxText.Append(tspanNode.InnerText.Trim() + Environment.NewLine);
                                }
                            }
                        }

                        box.Text = Convert.ToString(sbBoxText);
                        sbBoxText.Length = 0;
                    }
                }

                return boxList.Where(x => !string.IsNullOrEmpty(x.Text)).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool ContainsBgColor(List<XmlNode> abnormalPara)
        {
            try
            {
                string bookId = Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0];
                string pdfSvgXml = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/" + Convert.ToString(Request.QueryString["bid"]) + "_Svg.xml";

                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(pdfSvgXml);

                List<ColorTypes> allColors = GetAllColors();

                XmlNodeList pdfColorsList = xDoc.SelectNodes("//@fill");

                List<string> colorsList = new List<string>();

                foreach (XmlNode col in pdfColorsList)
                {
                    var matchedColor = allColors.Where(
                        x => (x.Red == Convert.ToInt32(col.Value.Replace("rgb(", "").Replace(")", "").Split(',')[0])) &&
                             (x.Green == Convert.ToInt32(col.Value.Replace("rgb(", "").Replace(")", "").Split(',')[1])) &&
                             (x.Blue == Convert.ToInt32(col.Value.Replace("rgb(", "").Replace(")", "").Split(',')[2])))
                        .ToList();

                    foreach (ColorTypes matchColor in matchedColor)
                    {
                        colorsList.Add(matchColor.ColorName);
                    }
                }

                var result = colorsList.GroupBy(x => x).Select(group => new { value = group.Key, Count = group.Count() }).FirstOrDefault();

                return false;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void GetSvgPdfList(string pdfSvgXml, int page)
        {
            try
            {
                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(pdfSvgXml);

                XmlNodeList textBlocksList1 = xDoc.SelectNodes("//descendant::text");

                XmlNodeList textBlocksList = xDoc.SelectNodes("//*[@page='" + "1']/descendant::text");

                XmlNodeList linesList = xDoc.SelectNodes("//*[@page='1']/descendant::text/tspan");

                GetAllColors();

                List<PdfSvgEntities> list = new List<PdfSvgEntities>();
            }
            catch (Exception ex)
            {

            }
        }

        public List<ColorTypes> GetAllColors()
        {
            Color color;
            List<ColorTypes> listOfColours = new List<ColorTypes>();
            List<Color> list = new List<Color>();

            foreach (string ColorName in Enum.GetNames(typeof(KnownColor)))
            {
                color = Color.FromName(ColorName);

                //if (!color.IsSystemColor && !color.Name.Equals("Transparent"))
                //{
                //    if (color.R == 216 && color.G == 216 && color.B == 218)
                //    {

                //    }
                //}

                if (!color.IsSystemColor && !color.Name.Equals("Transparent"))
                    listOfColours.Add(new ColorTypes { Red = color.R, Green = color.G, Blue = color.B, ColorName = color.Name });
            }

            listOfColours.Add(new ColorTypes { Red = 216, Green = 216, Blue = 218, ColorName = "LightGray" });

            return listOfColours;
        }

        public List<SvgBox> GetBoxText(int page)
        {
            try
            {
                string bookId = Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0];
                string pdfSvgXml = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/" + Convert.ToString(Request.QueryString["bid"]) + "_Svg.xml";

                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(pdfSvgXml);

                List<SvgBox> pathCoordList = new List<SvgBox>();
                List<SvgBox> boxList = new List<SvgBox>();

                XmlNode bgColorPage = xDoc.SelectSingleNode("//*[path[@fill!='none']/../../../../@page='" + page + "']/ancestor::div[@page='" + page + "']");

                if (bgColorPage == null) return null;

                XmlNodeList pathList = bgColorPage.SelectNodes("descendant::path[@fill!='none']/@d");

                if (pathList == null || pathList.Count == 0) return null;

                foreach (XmlNode path in pathList)
                {
                    List<string> coordList = Regex.Split(path.Value.Trim(), @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

                    if (coordList.Count == 13)
                    {
                        pathCoordList.Add(new SvgBox
                        {
                            TopYCoord = Convert.ToDouble(coordList[2]),
                            BottomYCoord = Convert.ToDouble(coordList[11])
                        });
                    }
                }

                double topYCoord = 0;
                double bottomYCoord = 0;

                for (int i = 0; i < pathCoordList.Count; i++)
                {
                    if (pathCoordList[i].TopYCoord > pathCoordList[i].BottomYCoord)
                    {
                        topYCoord = pathCoordList[i].TopYCoord;

                        if (pathCoordList.Count == 1)
                        {
                            bottomYCoord = pathCoordList[i].BottomYCoord;
                        }
                        else
                        {
                            while ((i + 1) < pathCoordList.Count && pathCoordList[i + 1].TopYCoord > pathCoordList[i].BottomYCoord)
                            {
                                i++;
                                bottomYCoord = pathCoordList[i].BottomYCoord;
                            }
                        }

                        boxList.Add(new SvgBox { TopYCoord = topYCoord, BottomYCoord = bottomYCoord });

                        topYCoord = 0;
                        bottomYCoord = 0;
                    }
                }

                XmlNodeList textList = bgColorPage.SelectNodes("descendant::text");
                StringBuilder sbBoxText = new StringBuilder();

                if (textList != null && textList.Count > 0)
                {
                    foreach (var box in boxList)
                    {
                        var boxText = textList.Cast<XmlNode>().Where(x => x.Attributes["transform"] != null &&
                                                                          x.Attributes["transform"].Value.Split(' ').Length > 4 &&
                                Convert.ToDouble(x.Attributes["transform"].Value.Split(' ')[5].Replace(")", "")) < box.TopYCoord &&
                                Convert.ToDouble(x.Attributes["transform"].Value.Split(' ')[5].Replace(")", "")) > box.BottomYCoord).ToList();

                        if (boxText.Count > 0)
                        {
                            foreach (XmlNode textNode in boxText)
                            {
                                foreach (XmlNode tspanNode in textNode.ChildNodes)
                                {
                                    sbBoxText.Append(tspanNode.InnerText.Trim() + Environment.NewLine);
                                }
                            }
                        }

                        box.Text = Convert.ToString(sbBoxText);
                        sbBoxText.Length = 0;
                    }
                }

                return boxList.Where(x => !string.IsNullOrEmpty(x.Text)).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void LoadPdfXml()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["XMLPath"])))
            {
                objGlobal.XMLPath = Convert.ToString(Session["XMLPath"]);
                objGlobal.LoadXml();
            }
        }
        public List<XmlNode> GetBoxTextByMatching(List<SvgBox> svgBoxLines, List<XmlNode> abnXmlNodes)
        {

            //IsBoxStartingLine(svgBoxLines, abnXmlNodes);

            return null;
        }

        public bool IsBoxStartingLine(string pdfJsSvgBoxLine, XmlNode xmlLine)
        {
            if (xmlLine == null || pdfJsSvgBoxLine == null) return false;

            StringBuilder sbXmlLine = new StringBuilder();

            XmlNodeList xmlLinesList = xmlLine.SelectNodes("descendant::ln");

            for (int i = 0; i < xmlLinesList.Count; i++)
            {
                sbXmlLine.Append(xmlLinesList[i].InnerText + " ");
            }

            string pdfJsFirstLine = Regex.Replace(pdfJsSvgBoxLine.Trim(), "[^A-Za-z0-9 ]", "");
            string xmlNodeLine = Regex.Replace(Convert.ToString(sbXmlLine).Trim(), "[^A-Za-z0-9 ]", "");

            if (string.IsNullOrEmpty(pdfJsFirstLine) || string.IsNullOrEmpty(xmlNodeLine)) return false;

            List<string> pdfJsLineTempList = Regex.Split(pdfJsFirstLine, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
            List<string> xmlLineTempList = Regex.Split(xmlNodeLine, @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

            if (pdfJsLineTempList.Count > 0 && xmlLineTempList.Count > 0)
            {
                int matchingPer = GetMatchingPercentage(pdfJsLineTempList[0].Trim(), xmlLineTempList[0].Trim());
                if (matchingPer < 50)
                    return false;
            }

            if (pdfJsFirstLine.Trim().Equals(xmlNodeLine.Trim()))
                return true;

            string pdfJsText = RemoveWhiteSpace(RemoveSpecialChars(pdfJsFirstLine));
            string xmlText = RemoveWhiteSpace(RemoveSpecialChars(xmlNodeLine));

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

        public string RemoveSpecialChars(string word)
        {
            return word.Replace(",", "").Replace(",", "").Replace("’", "").Replace("‘", "").Replace(",", "").Replace("***", "").Trim().ToLower();
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

        #endregion

        #region endNote

        private void ConvertToChapEndNotes()
        {
            int page = 0;
            string tempXmlPath = GetTempXmlPath();

            if (string.IsNullOrEmpty(tempXmlPath) || !File.Exists(tempXmlPath)) return;

            try
            {
                XmlDocument tetmlXmlDoc = new XmlDocument();
                tetmlXmlDoc.Load(tempXmlPath);

                int startPage = 0;
                int endPage = 0;

                XmlNode superScriptWrd = tetmlXmlDoc.SelectSingleNode("//Word[@type='end-node']");

                if (superScriptWrd != null &&
                    superScriptWrd.Attributes != null &&
                    superScriptWrd.Attributes["type"] != null)
                {
                    var pageNode = superScriptWrd.SelectSingleNode("//Word[@type='end-node']/ancestor::Page[@number]/@number");

                    if (pageNode != null)
                        page = Convert.ToInt32(pageNode.Value);

                    if (page > 0)
                    {
                        //populateSessions();
                        objGlobal.XMLPath = Session["XMLPath"].ToString();

                        if (!File.Exists(objGlobal.XMLPath)) return;

                        objGlobal.LoadXml();

                        startPage = page;

                        XmlNodeList sectioNodeList = objGlobal.PBPDocument.SelectNodes("//ln[@page='" + page +
                                                                                       "']/ancestor::section[@type='chapter']/descendant::section");

                        if (sectioNodeList != null && sectioNodeList.Count > 0)
                        {
                            List<XmlNode> sectionLineParaList = sectioNodeList[sectioNodeList.Count - 1].SelectNodes("descendant::upara").Cast<XmlNode>().ToList();

                            if (sectionLineParaList.Count > 0)
                            {
                                //XmlNodeList chapEndNodeList = GetChapEndNodesList(superScriptWrd, sectionLineParaList);

                                //if (chapEndNodeList == null || chapEndNodeList.Count == 0)
                                //{
                                //    //To Do show error msg
                                //    return;
                                //}


                                ContinuingEndNodesMerging(sectionLineParaList, objGlobal.PBPDocument);

                                XmlNodeList sectioNodeList1 = objGlobal.PBPDocument.SelectNodes("//ln[@page='" + page +
                                                                                      "']/ancestor::section[@type='chapter']/descendant::section");
                                List<XmlNode> sectionLineParaList1 = sectioNodeList1[sectioNodeList.Count - 1].SelectNodes("descendant::upara").Cast<XmlNode>()
                                    .ToList();

                                if (sectionLineParaList1.Count == 0) return;

                                CreateNParaXml(sectionLineParaList1);

                                //List<XmlNode> paraToConvert = new List<XmlNode>();

                                //foreach (XmlNode paraNode in sectionLineParaList)
                                //{
                                //    paraToConvert.Add(paraNode);
                                //}

                                //CreateNParaXml(paraToConvert);

                                List<int> pageList = objGlobal.PBPDocument.SelectNodes("//ln[@page='" + page +
                                                                      "']/ancestor::section[@type='chapter']/descendant::section/descendant::ln[@page]/@page")
                                                                      .Cast<XmlNode>().Select(x => Convert.ToInt32(x.Value))
                                                                      .Distinct().OrderByDescending(x => x).ToList();

                                XmlNodeList nParaEndNoteLines = objGlobal.PBPDocument.SelectNodes("//ln[@page='" + page +
                                                                      "']/ancestor::section[@type='chapter']/descendant::section/descendant::num");

                                if (nParaEndNoteLines != null && nParaEndNoteLines.Count > 0 && pageList != null && pageList.Count > 0)
                                {
                                    foreach (XmlNode nPara in nParaEndNoteLines)
                                    {
                                        if (nPara != null)
                                        {
                                            nPara.InnerText = "[" + nPara.InnerText.Trim().Replace(".", "") + "]";
                                        }
                                    }

                                    endPage = pageList[0];
                                }

                                objGlobal.SaveXml();

                                //XmlNode superScriptNode = null;

                                ////Removing tag from Temp xml from each chapter
                                //for (int index = startPage; index < endPage; index++)
                                //{
                                //    superScriptNode = tetmlXmlDoc.SelectSingleNode("//Word[@type='end-node']/ancestor::Page[@number='" + index +
                                //                                                   "']/descendant::Word[@type='end-node']");

                                //    if (superScriptNode != null &&
                                //        superScriptNode.Attributes != null &&
                                //        superScriptNode.Attributes["type"] != null &&
                                //        superScriptNode.Attributes["type"].Value.Equals("end-node"))
                                //    {
                                //        superScriptNode.Attributes["type"].Value = "upara";
                                //    }
                                //}

                                XmlNodeList superScriptNodes = null;

                                //Removing tag from Temp xml from each chapter
                                for (int index = startPage; index < endPage; index++)
                                {
                                    superScriptNodes = tetmlXmlDoc.SelectNodes("//Word[@type='end-node']/ancestor::Page[@number='" + index +
                                                                                   "']/descendant::Word[@type='end-node']");

                                    if (superScriptNodes != null && superScriptNodes.Count > 0)
                                    {
                                        foreach (XmlNode superScriptNode in superScriptNodes)
                                        {
                                            if (superScriptNode != null &&
                                                superScriptNode.Attributes != null &&
                                                superScriptNode.Attributes["type"] != null &&
                                                superScriptNode.Attributes["type"].Value.Equals("end-node"))
                                            {
                                                superScriptNode.Attributes["type"].Value = "upara";
                                            }
                                        }
                                    }
                                }

                                tetmlXmlDoc.Save(tempXmlPath);
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {

            }
        }

        public PdfFootNote GetEndNotePara(XmlNodeList allParas)
        {
            string tempXmlPath = GetTempXmlPath();

            if (string.IsNullOrEmpty(tempXmlPath) || !File.Exists(tempXmlPath)) return null;

            PdfFootNote footNote = new PdfFootNote();

            StringBuilder lineText = new StringBuilder();

            try
            {
                XmlDocument tetmlXmlDoc = new XmlDocument();
                tetmlXmlDoc.Load(tempXmlPath);

                XmlNode superScriptWrd = tetmlXmlDoc.SelectSingleNode("//Word[@type='end-node']");

                footNote.SupScriptXmlLine = superScriptWrd;

                XmlNode pageNumber = tetmlXmlDoc.SelectSingleNode("//Word[@type='end-node']/ancestor::Page[@number]/@number");

                if (superScriptWrd != null && pageNumber != null)
                {
                    if (!string.IsNullOrEmpty(pageNumber.InnerText))
                        footNote.Page = Convert.ToInt32(pageNumber.InnerText);

                    lineText.Append("</br>");

                    lineText.Append("<font color=green><sup>" + superScriptWrd.InnerText.Trim() + "</sup></font>");
                    footNote.SupScriptWord = superScriptWrd.InnerText.Trim();
                }

                footNote.FootNoteText = Convert.ToString(lineText);

                return footNote;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void DisplayFootAndEndNote(string selectedParaType, XmlNodeList allParas, PdfFootNote footNote)
        {
            if (allParas != null && allParas.Count > 0 && footNote != null && !string.IsNullOrEmpty(footNote.FootNoteText) && footNote.Page > 0)
            {
                if (!string.IsNullOrEmpty(selectedParaType) && !string.IsNullOrEmpty(footNote.FootNoteText))
                {
                    divParaText.InnerHtml = footNote.FootNoteText;
                }
                else
                {
                    divParaText.InnerText = "";
                }

                string sourcePagePath = objMyDBClass.MainDirPhyPath + "/" +
                                        Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0] + "/" +
                                        Convert.ToString(Request.QueryString["bid"]) + "/TaggingUntagged/Page" + footNote.Page + ".pdf";

                if (File.Exists(sourcePagePath))
                    File.Delete(sourcePagePath);

                if (!File.Exists(sourcePagePath))
                    objConversionClass.ExtractPages(objGlobal.PDFPath, sourcePagePath, footNote.Page, footNote.Page);

                string outPutFilePath = sourcePagePath.Replace(".pdf", "_Highlighted.pdf");

                if (File.Exists(outPutFilePath))
                    File.Delete(outPutFilePath);

                List<string> lstcoordiants = GetFootNoteParaCoordinates(selectedParaType, footNote, sourcePagePath);

                if (lstcoordiants != null && lstcoordiants.Count > 0)
                    HighLightSelectedParas(sourcePagePath, outPutFilePath, BaseColor.ORANGE, lstcoordiants);

                ShowPDF(footNote.Page);
            }
        }

        public XmlNode InsertNumTagInEndNode(XmlNode lines)
        {
            try
            {
                var linesList = lines.SelectNodes("descendant::ln").Cast<XmlNode>().ToList();

                if (linesList.Count > 0)
                {
                    if (linesList[0].InnerText.Trim().Length > 0 && char.IsNumber(linesList[0].InnerText.Trim().ToCharArray().ElementAt(0)))
                    {
                        List<string> allWords = Regex.Split(linesList[0].InnerText.Trim(), @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

                        if (allWords[0].Replace(".", "").All(char.IsDigit))
                        {
                            List<string> allXmlWords = Regex.Split(linesList[0].InnerXml.Trim(), @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();

                            if (allXmlWords.Count > 0)
                            {
                                string numPart = HttpUtility.HtmlDecode("<num>" + allWords[0].Replace(".", "") + "</num>");

                                allXmlWords[0] = numPart;

                                string endNodeLine = string.Join(" ", allXmlWords.ToArray());

                                linesList[0].InnerXml = endNodeLine;
                            }
                        }

                        //string numPart = allWords[0].Replace(".", "").All(char.IsDigit)

                        //linesList[0].InnerXml = HttpUtility.HtmlDecode("<num>" + numPart + "</num>" + linesList[0].InnerXml.Trim().Replace(numPart, ""));
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void ConvertToEndNoteInSection(int page)
        {
            if (page == 0) return;

            string treeViewText = "";

            bool isApplyAll = cbxApplyAll.Checked;

            try
            {
                populateSessions();
                objGlobal.XMLPath = Session["XMLPath"].ToString();

                if (!File.Exists(objGlobal.XMLPath)) return;

                objGlobal.LoadXml();


                List<string> secList = objGlobal.PBPDocument.SelectNodes("//ln[@page='" + page + "']/parent::section-title/ln/text()").Cast<XmlNode>()
                                          .Where(x => !string.IsNullOrEmpty(x.Value)).Select(y => y.Value).ToList();

                if (secList.Count > 0)
                    treeViewText = secList[0];

                if (!string.IsNullOrEmpty(treeViewText))
                {


                    //List<XmlNode> uParaList = objGlobal.PBPDocument.SelectNodes("//ln[@page='" + page + "' and text()= '" + treeViewText +
                    //                     "']/parent::section-title/ancestor::pre-section/descendant::upara|ancestor::post-section/descendant::upara|ancestor::pbp-body/descendant::upara").Cast<XmlNode>()
                    //                     .Where(x => x.ChildNodes != null &&
                    //                                 x.ChildNodes.Count > 0 &&
                    //                                 !string.IsNullOrEmpty(x.ChildNodes[0].InnerText.Trim()) &&
                    //                         char.IsDigit(x.ChildNodes[0].InnerText.Trim().ToCharArray().ElementAt(0))).ToList();

                    List<XmlNode> uParaList =
                        objGlobal.PBPDocument.SelectNodes("//ln[@page='" + page + "' and text()= '" + treeViewText +
                                                          "']/parent::section-title/ancestor::pre-section/descendant::upara|ancestor::post-section/descendant::upara|ancestor::pbp-body/descendant::upara")
                            .Cast<XmlNode>()
                            .Where(x => x.ChildNodes != null &&
                                        x.ChildNodes.Count > 0 &&
                                        !string.IsNullOrEmpty(x.ChildNodes[0].InnerText.Trim())).ToList();

                    if (uParaList.Count == 0) return;

                    //uParaList = ArrangeUParasByEndNodes(uParaList);

                    List<XmlNode> paraToConvert = new List<XmlNode>();
                    paraToConvert = uParaList;

                    CreateNParaXmlInEndNode(paraToConvert);

                    //List<XmlNode> numParaList = objGlobal.PBPDocument.SelectNodes("//ln[text()= '" + treeViewText +
                    //                         "']/ancestor::post-section/descendant::npara/descendant::num").Cast<XmlNode>().ToList();

                    List<XmlNode> numParaList =
                        objGlobal.PBPDocument.SelectNodes("//ln[@page='" + page + "' and text()= '" + treeViewText +
                                                          "']/parent::section-title/ancestor::pre-section/descendant::npara/descendant::num|ancestor::post-section/descendant::npara/descendant::num|ancestor::pbp-body/descendant::npara/descendant::num")
                            .Cast<XmlNode>()
                            .ToList();

                    foreach (XmlNode nPara in numParaList)
                    {
                        if (!string.IsNullOrEmpty(nPara.InnerText.Trim()))
                        {
                            nPara.InnerText = "[" + nPara.InnerText.Trim() + "]";
                        }
                    }

                    objGlobal.SaveXml();

                    string tempXmlPath = GetTempXmlPath();

                    if (string.IsNullOrEmpty(tempXmlPath) || !File.Exists(tempXmlPath)) return;

                    XmlDocument temp_FootNotesXmlDoc = new XmlDocument();
                    temp_FootNotesXmlDoc.Load(tempXmlPath);

                    XmlNodeList endNodeWordList = temp_FootNotesXmlDoc.SelectNodes("//Word[@type='end-node']");

                    if (endNodeWordList != null && endNodeWordList.Count > 0)
                    {
                        foreach (XmlNode endNodeWord in endNodeWordList)
                        {
                            endNodeWord.Attributes["type"].Value = "upara";
                        }

                        temp_FootNotesXmlDoc.Save(tempXmlPath);
                    }

                    ShowComplexBitByOrder();
                }
                else
                {
                    ShowComplexBitByOrder();

                    //To Do show error msg
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ConvertToEndNote(string treeViewText, int page)
        {
            if (string.IsNullOrEmpty(treeViewText) || page == 0) return;

            bool isApplyAll = cbxApplyAll.Checked;

            try
            {
                populateSessions();
                objGlobal.XMLPath = Session["XMLPath"].ToString();

                if (!File.Exists(objGlobal.XMLPath)) return;

                objGlobal.LoadXml();

                //List<XmlNode> sectionNode = objGlobal.PBPDocument.SelectNodes("//ln[text()= '" + treeViewText +
                //                           "']/ancestor::section/descendant::npara").Cast<XmlNode>()
                //                                    .Where(x => x.ChildNodes != null &&
                //                                                x.ChildNodes.Count > 0 &&
                //                                                x.ChildNodes[0].Attributes != null &&
                //                                                x.ChildNodes[0].Attributes.Count > 0 &&
                //                                                x.ChildNodes[0].Attributes["page"] != null &&
                //                                                Convert.ToInt32(x.ChildNodes[0].Attributes["page"].Value).Equals(page)).ToList();

                //ln[text()= 'Notes']/ancestor::post-section/descendant::ln

                //List<XmlNode> uParaList = objGlobal.PBPDocument.SelectNodes("//ln[text()= '" + treeViewText +
                //                          "']/ancestor::post-section/descendant::upara").Cast<XmlNode>()
                //                          .Where(x => x.ChildNodes != null &&
                //                                      x.ChildNodes.Count > 0 &&
                //                                      !string.IsNullOrEmpty(x.ChildNodes[0].InnerText.Trim()) &&
                //                              char.IsDigit(x.ChildNodes[0].InnerText.Trim().ToCharArray().ElementAt(0))).ToList();


                List<XmlNode> uParaList = objGlobal.PBPDocument.SelectNodes("//ln[text()= '" + treeViewText +
                                         "']/ancestor::post-section/descendant::upara").Cast<XmlNode>()
                                         .Where(x => x.ChildNodes != null &&
                                                     x.ChildNodes.Count > 0 &&
                                                     !string.IsNullOrEmpty(x.ChildNodes[0].InnerText.Trim())).ToList();

                if (uParaList.Count == 0) return;

                ContinuingEndNodesMerging(uParaList, objGlobal.PBPDocument);

                List<XmlNode> uParaList1 = objGlobal.PBPDocument.SelectNodes("//ln[text()= '" + treeViewText +
                                          "']/ancestor::post-section/descendant::upara").Cast<XmlNode>()
                                          .Where(x => x.ChildNodes != null &&
                                                      x.ChildNodes.Count > 0 &&
                                                      !string.IsNullOrEmpty(x.ChildNodes[0].InnerText.Trim()) &&
                                              char.IsDigit(x.ChildNodes[0].InnerText.Trim().ToCharArray().ElementAt(0))).ToList();

                List<XmlNode> paraToConvert = new List<XmlNode>();
                paraToConvert = uParaList1;

                CreateNParaXmlInEndNode(paraToConvert);

                List<XmlNode> numParaList = objGlobal.PBPDocument.SelectNodes("//ln[text()= '" + treeViewText +
                                         "']/ancestor::post-section/descendant::npara/descendant::num").Cast<XmlNode>().ToList();

                foreach (XmlNode nPara in numParaList)
                {
                    if (!string.IsNullOrEmpty(nPara.InnerText.Trim()))
                    {
                        nPara.InnerText = "[" + nPara.InnerText.Trim().Replace(".", "") + "]";
                    }
                }

                objGlobal.SaveXml();

                ////string nextNParaStartingChar = "";

                ////for (int i = 0; i < nParaList.Count; i++)
                ////{
                ////    XmlNode currentLine = nParaList[i];

                ////            if ((i + 1) < nParaList.Count)
                ////                nextNParaStartingChar = GetNParaStartChar(nParaList[i + 1].InnerText.Trim());

                ////            XmlNode nextLine = nParaList[i].NextSibling;

                ////     //else if (currentLine.NextSibling != null || (currentLine.ParentNode != null && currentLine.ParentNode.NextSibling != null &&
                ////     //                (currentLine.ParentNode.NextSibling.Name.Equals("upara"))))
                ////     //       {



                ////    if (currentLine.NextSibling != null ||
                ////        (currentLine.ParentNode != null && currentLine.ParentNode.NextSibling != null &&
                ////         (currentLine.ParentNode.NextSibling.Name.Equals("upara")) &&
                ////         !currentLine.ParentNode.NextSibling.InnerText.Trim().ToCharArray().ElementAt(0).ToString().Contains(nextNParaStartingChar)))
                ////    {
                ////                            XmlNodeList lineNodes = currentLine.ParentNode.NextSibling.SelectNodes("descendant::ln");
                ////        {

                ////        }

                ////        while (!nextLine.InnerText.Trim().ToCharArray().ElementAt(0).ToString().Contains(nextNParaStartingChar))
                ////        {

                ////            currentLine.ParentNode.AppendChild(nextLine);

                ////            currentLine.ParentNode.NextSibling.ParentNode.RemoveChild(currentLine.ParentNode.NextSibling);

                ////            nextLine = nParaList[i].NextSibling;
                ////        }
                ////    }
                ////}

                ////objGlobal.SaveXml();

                string tempXmlPath = GetTempXmlPath();

                if (string.IsNullOrEmpty(tempXmlPath) || !File.Exists(tempXmlPath)) return;

                XmlDocument temp_FootNotesXmlDoc = new XmlDocument();
                temp_FootNotesXmlDoc.Load(tempXmlPath);

                XmlNodeList endNodeWordList = temp_FootNotesXmlDoc.SelectNodes("//Word[@type='end-node']");

                if (endNodeWordList != null && endNodeWordList.Count > 0)
                {
                    foreach (XmlNode endNodeWord in endNodeWordList)
                    {
                        endNodeWord.Attributes["type"].Value = "upara";
                        //temp_FootNotesXmlDoc.Save(tempXmlPath);
                    }

                    temp_FootNotesXmlDoc.Save(tempXmlPath);
                }

                divParaText.InnerText = "";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public EndNodeCoord GetEndNodeValues(List<XmlNode> uParaList, int page)
        {
            EndNodeCoord endNodeObj = null;
            double leftVal = 0;
            double marginValue = 5;
            bool endNodeFound = false;

            try
            {
                if (page > 0)
                {
                    List<XmlNode> pageAllNodes =
                        uParaList.Where(x => x.ChildNodes.Cast<XmlNode>().Any(y => y.Attributes != null &&
                                                                                   y.Attributes["page"] != null &&
                                                                                   y.Attributes["left"] != null &&
                                                                                   Convert.ToInt32(
                                                                                       y.Attributes["page"].Value)
                                                                                       .Equals(page))).ToList();

                    foreach (XmlNode node in pageAllNodes)
                    {
                        if (node.ChildNodes.Count > 0 &&
                            node.ChildNodes[0].Attributes != null &&
                            node.ChildNodes[0].Attributes["page"] != null)
                        {
                            List<string> allWords =
                                Regex.Split(node.ChildNodes[0].InnerText.Trim(), @"\s+")
                                    .Where(x => !string.IsNullOrEmpty(x))
                                    .ToList();

                            if (allWords.Count > 0)
                            {
                                if (allWords[0].Replace(".", "").All(char.IsDigit))
                                {
                                    leftVal = Convert.ToDouble(node.ChildNodes[0].Attributes["left"].Value);

                                    endNodeObj = new EndNodeCoord();
                                    endNodeObj.NormalX =
                                        Convert.ToDouble(node.ChildNodes[0].Attributes["left"].Value);
                                    endNodeObj.NormalIndentX = node.ChildNodes.Count > 1
                                        ? Convert.ToDouble(node.ChildNodes[1].Attributes["left"].Value)
                                        : 0;
                                    endNodeObj.NormalFontSize =
                                        Convert.ToDouble(node.ChildNodes[0].Attributes["fontsize"].Value);
                                    endNodeObj.NormalFontName =
                                        Convert.ToString(node.ChildNodes[0].Attributes["font"].Value);
                                    endNodeObj.StartNumber = Convert.ToInt32(allWords[0].Replace(".", "").Replace("-", ""));
                                    endNodeObj.Page = page;
                                    endNodeObj.PageType = page % 2 == 0 ? "even" : "odd";

                                    endNodeFound = true;
                                }
                            }

                            List<XmlNode> pageNodes = pageAllNodes.Where(x => x.ChildNodes.Cast<XmlNode>().Any(y => y.Attributes != null &&
                                                                                              y.Attributes["page"] != null &&
                                                                                              y.Attributes["left"] != null &&
                                                                                              Convert.ToInt32(
                                                                                                  y.Attributes["page"].Value)
                                                                                                  .Equals(page)))
                                    .Where(z => z.ChildNodes.Count > 0 &&
                                                z.ChildNodes[0].Attributes != null &&
                                                z.ChildNodes[0].Attributes["left"] != null &&
                                                Math.Abs(Convert.ToDouble(z.ChildNodes[0].Attributes["left"].Value) -
                                                         leftVal) <
                                                marginValue).ToList();
                            if (pageNodes.Count > 1 &&
                                pageNodes[pageNodes.Count - 1].ChildNodes.Count > 0 &&
                                pageNodes[pageNodes.Count - 1].ChildNodes[0].Attributes != null &&
                                endNodeObj != null)
                            {
                                List<string> allEndNodeWords =
                                    Regex.Split(pageNodes[pageNodes.Count - 1].ChildNodes[0].InnerText.Trim(), @"\s+")
                                        .Where(x => !string.IsNullOrEmpty(x))
                                        .ToList();

                                if (allEndNodeWords.Count > 0)
                                {
                                    if (allEndNodeWords[0].Replace(".", "").All(char.IsDigit))
                                    {
                                        endNodeObj.EndNumber = Convert.ToInt32(allEndNodeWords[0].Replace(".", ""));
                                    }
                                }
                            }
                        }
                        if (endNodeFound)
                            break;
                    }//end foreach
                }

                return endNodeObj;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void ContinuingEndNodesMerging(List<XmlNode> uParaList, XmlDocument xmlDoc)
        {
            if (uParaList.Count > 1)
            {
                if (uParaList[0].ChildNodes.Count > 0 &&
                uParaList[0].ChildNodes[0].Attributes != null &&
                uParaList[0].ChildNodes[0].Attributes["page"] != null &&

                uParaList[uParaList.Count - 1].ChildNodes.Count > 0 &&
                uParaList[uParaList.Count - 1].ChildNodes[0].Attributes != null &&
                uParaList[uParaList.Count - 1].ChildNodes[0].Attributes["page"] != null)
                {
                    int startPage = Convert.ToInt32(uParaList[0].ChildNodes[0].Attributes["page"].Value);
                    int endPage = Convert.ToInt32(uParaList[uParaList.Count - 1].ChildNodes[0].Attributes["page"].Value);

                    if (endPage > startPage)
                    {
                        EndNodeCoord firstPageCoord = GetEndNodeValues(uParaList, startPage);
                        EndNodeCoord secondPageCoord = GetEndNodeValues(uParaList, startPage + 1);
                        MergeNode(xmlDoc, uParaList, firstPageCoord, secondPageCoord, endPage);
                    }
                    else if (endPage == startPage)
                    {
                        EndNodeCoord startPageCoord = GetEndNodeValues(uParaList, startPage);

                    }
                }
            }
            else if (uParaList.Count == 1)
            {
                if (uParaList[0].ChildNodes.Count > 0 &&
               uParaList[0].ChildNodes[0].Attributes != null &&
               uParaList[0].ChildNodes[0].Attributes["page"] != null)
                {
                    EndNodeCoord startPageCoord = GetEndNodeValues(uParaList, Convert.ToInt32(uParaList[0].ChildNodes[0].Attributes["page"].Value));
                }
            }
        }

        //// Load a TreeView control from an XML file.
        //private void LoadTreeViewFromXmlFile(string filename, TreeView trv)
        //{
        //    // Load the XML document.
        //    XmlDocument xml_doc = new XmlDocument();
        //    xml_doc.Load(filename);

        //    // Add the root node's children to the TreeView.
        //    trv.Nodes.Clear();
        //    AddTreeViewChildNodes(trv.Nodes, xml_doc.DocumentElement);
        //}



        public void ShowEndNoteOtherPDF(int page)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Request.QueryString["bid"]))) return;

            try
            {
                string newPDFName = objMyDBClass.MainDirPhyPath + "/" + Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0] + "/" +
                                    Convert.ToString(Request.QueryString["bid"]) + "/TaggingUntagged/Page" + page + ".pdf";

                if (!File.Exists(newPDFName))
                    objConversionClass.ExtractPages(objGlobal.PDFPath, newPDFName, page, page);

                ////aamir temporary change
                //PDFViewerTarget.FilePath = Session["MainDirectory"].ToString() + "/" +
                //                           Request.QueryString["bid"].ToString().Split(new char[] { '-' })[0] + "/" +
                //                           Request.QueryString["bid"].ToString() + "/TaggingUntagged/Page" + pageno +
                //                           ".pdf#toolbar=0";
                ShowPdf1.FilePath = "DisplayPdf.ashx?bid=" + Convert.ToString(Request.QueryString["bid"]) + "&page=" + page;
            }

            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        protected void btnViewPage_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxOtherPage.Text))
            {
                int num;

                if (int.TryParse(tbxOtherPage.Text, out num))
                    ShowEndNoteOtherPDF(Convert.ToInt32(tbxOtherPage.Text));

                Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "ShowEndNoteOtherPageDialog()", true);
            }
            else
                Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "ShowEndNoteSelDialog()", true);
        }

        private void populateTreeView()
        {
            LoadPdfXml();

            if (objGlobal.PBPDocument == null) return;

            //XmlNodeList postSecLinesList = objGlobal.PBPDocument.SelectNodes("descendant::section-title/ln[text()]");

            XmlNodeList postSecLinesList = objGlobal.PBPDocument.SelectNodes("//section-title/ancestor::post-section/descendant::section-title/ln[text()]");

            //XmlNodeList postSecLinesList = objGlobal.PBPDocument.SelectNodes("//post-section/descendant::section[@type='level1']");

            var rootNode = objGlobal.PBPDocument.DocumentElement;

            if (rootNode != null)
            {
                TreeNode mainNode = new TreeNode();
                mainNode.Text = rootNode.Name;

                TreeNode postSecNode = new TreeNode();
                postSecNode.Text = "post-section";

                mainNode.ChildNodes.Add(postSecNode);

                if (postSecLinesList != null && postSecLinesList.Count > 0)
                {
                    foreach (XmlNode node in postSecLinesList)
                    {
                        postSecNode.ChildNodes.Add(new TreeNode
                        {
                            Text = node.InnerText,
                            Value = node.OuterXml
                        });
                    }

                    tvChapters.Nodes.Add(mainNode);

                    tvChapters.ExpandAll();
                }
            }
        }

        private void populateTreeViewTest()
        {
            var rootNode = new TreeNode();

            XmlDocument xmlDoc = new XmlDocument();

            xmlDoc.Load(Server.MapPath("EmployeesDemo.xml"));

            string str = "";
            foreach (XmlNode xmlNode in xmlDoc.DocumentElement)
            {
                rootNode.Text = xmlNode.Name;
                if (xmlNode.HasChildNodes)
                {   //Employee items
                    foreach (XmlNode childnode in xmlNode.ChildNodes)
                    {
                        var nodeEmployee = new TreeNode();
                        nodeEmployee.Text = childnode.Name;

                        //Get Qualifications
                        if (childnode.HasChildNodes)
                        {
                            foreach (XmlNode childnode2 in childnode.ChildNodes)
                            {
                                var nodeQualifications = new TreeNode();
                                nodeQualifications.Text = childnode2.Name;

                                //Get Qualification
                                if (childnode2.HasChildNodes)
                                {
                                    foreach (XmlNode childnode3 in childnode2.ChildNodes)
                                    {
                                        var nodeQualification = new TreeNode();
                                        nodeQualification.Text = childnode3.Name;
                                        nodeQualifications.ChildNodes.Add(nodeQualification);
                                    }
                                }

                                nodeEmployee.ChildNodes.Add(nodeQualifications);
                            }
                        }

                        rootNode.ChildNodes.Add(nodeEmployee);
                    }
                }
                string nodename = xmlNode.Name;
                str += nodename + "<br/>";
            }

            tvChapters.Nodes.Add(rootNode);
        }

        public void populateTreeView1()
        {
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            try
            {
                //string xmlTreeFile = objGlobal.XMLPath.Replace(".rhyw", ".xml");

                string xmlTreeFile = @"F:\33.xml";

                // //XmlDataChapters.XPath = "pbp-book/box|//section-title|//section|pbp-book/upara|//spara|//npara|//Table|//image";

                //objGlobal.PBPDocument.Save(xmlTreeFile);

                //XmlDataChapters.DataFile = xmlTreeFile;
                //var yy = XmlDataChapters.GetXmlDocument();
                //XmlDataChapters.XPath = "//section-title/ln[text()]";
                //tvChapters.DataSource = XmlDataChapters;
                //tvChapters.DataBind();
                //tvChapters.ExpandAll();

                XmlDocument xmlTreeDoc = objGlobal.BuildXMLChapterTree();
                //xmlTreeDoc.Save(xmlTreeFile);
                tvChapters.DataSource = null;
                tvChapters.DataBind();

                //XmlDataChapters.XPath = "//post-section/descendant::section[@type='level1']";
                //XmlDataChapters.DataFile = xmlTreeFile;

                tvChapters.DataSource = objGlobal.PBPDocument;
                tvChapters.DataBind();
                tvChapters.ExpandAll();

            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        protected void tvChapters_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            try
            {
                var value = e.Node.DataItem;


                if (e.Node.Value.Contains("NotEmbeded"))
                {
                    e.Node.Text = "<div style=\" background:red; font-weight:bold;\">" + e.Node.Text + "</div>";
                }

                if (e.Node.Value.Contains("correction=\"edit,\""))
                {
                    e.Node.Text = "<font style='background-color:#ffff42'>" + e.Node.Text + "</font>";
                }

                //if ((e.Node.Value.Split(',')[0].Contains("edit")) || (e.Node.Value.Split(',')[1].Contains("edit")))
                //{
                //    e.Node.Text = "<font style='background-color:#ffff42'>" + e.Node.Text + "</font>";
                //}
            }
            catch (Exception ex)
            {
                //ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry some error has occured");
                //showMessage(ex.Message);
            }
        }

        #endregion

        #region Create Xmls

        private void CreateSParaXml(List<XmlNode> paraToConvert, XmlNode abnormalPara)
        {
            try
            {
                foreach (XmlNode item in paraToConvert)
                {
                    XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("spara");
                    string origNodeXml = item.InnerXml;
                    origNodeXml = Regex.Replace(origNodeXml, "</?num.*?>", "");
                    origNodeXml = Regex.Replace(origNodeXml, "</?para.*?>", "");
                    origNodeXml = Regex.Replace(origNodeXml, "</?line.*?>", "");

                    ((XmlElement)convertedNode).SetAttribute("h-align", ddlSparaOrientation.SelectedValue);
                    ((XmlElement)convertedNode).SetAttribute("bgcolor", ddlSparaBackground.SelectedValue);
                    ((XmlElement)convertedNode).SetAttribute("type", ddlSparaType.SelectedValue);

                    // Assigning line or para
                    string xmlChild = ddlSparaSubType.SelectedValue;
                    string xml = "";
                    if (chkStanza.Enabled == true && chkStanza.Checked == true)
                    {
                        xml = origNodeXml.Replace("<ln", "<" + xmlChild + "><ln")
                            .Replace("</ln>", "</ln></" + xmlChild + ">");
                        xml = Regex.Replace(xml, "(</ln>)(</line>|</para>)(<break.*?>)", "$3$1$2");
                    }
                    else
                    {
                        xml = "<" + xmlChild + ">" + origNodeXml + "</" + xmlChild + ">";
                    }
                    convertedNode.InnerXml = xml;
                    if (convertedNode.Attributes["type"].Value.ToString() != "other")
                    {
                        XmlAttribute attAlign = convertedNode.Attributes["h-align"];
                        convertedNode.Attributes.Remove(attAlign);
                    }

                    #region Copy original node attributes

                    //if (abnormalPara != null)
                    //{
                    //    foreach (XmlAttribute attr in abnormalPara.Attributes)
                    //    {
                    //        if (attr.Name == "id" | attr.Name == "pnum" | attr.Name == "text-indent" |
                    //            attr.Name == "padding-bottom" | attr.Name == "conversion-Operations")
                    //            ((XmlElement)convertedNode).SetAttribute(attr.Name, attr.Value);
                    //    }
                    //    if (convertedNode.Attributes["conversion-Operations"].Value != "")
                    //    {
                    //        convertedNode.Attributes["conversion-Operations"].Value =
                    //            convertedNode.Attributes["conversion-Operations"].Value + ",converted";
                    //    }
                    //    else
                    //    {
                    //        convertedNode.Attributes["conversion-Operations"].Value = "converted";
                    //    }
                    //}

                    if (abnormalPara != null)
                    {
                        foreach (XmlAttribute attr in abnormalPara.Attributes)
                        {
                            if (attr.Name == "id" | attr.Name == "pnum" | attr.Name == "text-indent" |
                                attr.Name == "padding-bottom")
                                ((XmlElement)convertedNode).SetAttribute(attr.Name, attr.Value);
                        }
                    }

                    #endregion

                    item.ParentNode.InsertBefore(convertedNode, item);
                    //objGlobal.SaveXml();
                }

                //objGlobal.SaveXml();

                foreach (XmlNode item in paraToConvert)
                {
                    item.ParentNode.RemoveChild(item);
                    //objGlobal.SaveXml();
                }
                objGlobal.SaveXml();
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void CreateNParaXml(List<XmlNode> paraToConvert)
        {
            try
            {
                if (paraToConvert != null)
                {
                    if (paraToConvert.Count > 0)
                    {
                        for (int i = 0; i < paraToConvert.Count; i++)
                        {
                            XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("npara");

                            if (((XmlElement)convertedNode).HasAttribute("id"))
                            {
                                ((XmlElement)convertedNode).SetAttribute("id", paraToConvert[i].Attributes["id"].Value);
                            }
                            else
                            {
                                ((XmlElement)convertedNode).SetAttribute("id", "0");
                            }

                            if (((XmlElement)convertedNode).HasAttribute("pnum"))
                            {
                                ((XmlElement)convertedNode).SetAttribute("pnum", paraToConvert[i].Attributes["pnum"].Value);
                            }
                            else
                            {
                                ((XmlElement)convertedNode).SetAttribute("pnum", "0");
                            }

                            //((XmlElement)convertedNode).SetAttribute("id", paraToConvert[i].Attributes["id"].Value);
                            //((XmlElement)convertedNode).SetAttribute("pnum", paraToConvert[i].Attributes["pnum"].Value);
                            //((XmlElement)convertedNode).SetAttribute("text-indent", paraToConvert[i].Attributes["text-indent"].Value);
                            //((XmlElement)convertedNode).SetAttribute("padding-bottom", paraToConvert[i].Attributes["padding-bottom"].Value);

                            var nParaLines = InsertNumTag(paraToConvert[i]);

                            convertedNode.InnerXml = paraToConvert[i].InnerXml;

                            if (paraToConvert[i].ParentNode != null)
                            {
                                paraToConvert[i].ParentNode.InsertBefore(convertedNode, paraToConvert[i]);
                                //objGlobal.SaveXml();
                            }
                            else
                            {

                            }
                        }

                        for (int j = 0; j < paraToConvert.Count; j++)
                        {
                            paraToConvert[j].ParentNode.RemoveChild(paraToConvert[j]);
                            //objGlobal.SaveXml();
                        }

                        objGlobal.SaveXml();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        public XmlNode InsertNumTag(XmlNode lines)
        {
            try
            {
                var linesList = lines.SelectNodes("descendant::ln").Cast<XmlNode>().ToList();

                foreach (XmlNode line in linesList)
                {
                    if (line.InnerText.Trim().Length > 0 && (char.IsNumber(line.InnerText.Trim().ToCharArray().ElementAt(0)) ||
                        Convert.ToString(line.InnerText.Trim().ToCharArray().ElementAt(0)).Equals("•")))
                    {
                        List<string> allWords = Regex.Split(line.InnerText.Trim(), @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
                        string numPart = allWords[0];

                        //line.InnerXml = "<num>" + numPart + "</num>" + CleanInvalidXmlChars(line.InnerText.Trim().Remove(0, allWords[0].Count()));

                        //HttpUtility.HtmlDecode(supScriptLineText).
                        //fileText.Replace("&", "&amp;")
                        //   .Replace("<", "&lt;")
                        //   .Replace(">", "&gt;")
                        //   .Replace("\"", "&quot;")
                        //   .Replace("'", "&apos;");

                        //var t1 = line.InnerXml.Trim().Replace(numPart, "<num>" + numPart + "</num>")
                        //                                                          .Replace("&", "&amp;").Replace("'", "&apos;");

                        //string finalText = CleanInvalidXmlChars(HttpUtility.HtmlDecode(line.InnerXml.Trim().Replace(numPart, "<num>" + numPart + "</num>")));

                        ////line.InnerXml = HttpUtility.HtmlDecode(line.InnerXml.Trim().Replace(numPart, "<num>" + numPart + "</num>"));

                        line.InnerXml = HttpUtility.HtmlDecode("<num>" + numPart + "</num>" + line.InnerXml.Trim().Replace(numPart, ""));
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void CreateBoxXml(List<XmlNode> paraToConvert)
        {
            try
            {
                int page = 0;
                if (paraToConvert != null && paraToConvert.Count > 0)
                {
                    int prevPage = 0;
                    XmlNode boxElem = null;

                    for (int i = 0; i < paraToConvert.Count; i++)
                    {
                        page = paraToConvert[i].ChildNodes.Count > 0
                            ? Convert.ToInt32(paraToConvert[i].ChildNodes[0].Attributes["page"].Value)
                            : 0;

                        if (page != prevPage)
                        {
                            boxElem = objGlobal.PBPDocument.CreateElement("box");

                            ((XmlElement)boxElem).SetAttribute("id", "0");
                            ((XmlElement)boxElem).SetAttribute("bgcolor", "gray");
                            ((XmlElement)boxElem).SetAttribute("border", "off");

                            XmlNode boxTitleElem = objGlobal.PBPDocument.CreateElement("box-title");
                            boxElem.PrependChild(boxTitleElem);

                            XmlElement ln = objGlobal.PBPDocument.CreateElement("ln");

                            if (paraToConvert.Count > 1 &&
                                paraToConvert[0].ChildNodes.Count > 0 &&
                                paraToConvert[1].ChildNodes.Count > 0 &&
                                paraToConvert[0].ChildNodes[0].Attributes["fontsize"] != null &&
                                paraToConvert[1].ChildNodes[0].Attributes["fontsize"] != null &&
                                Convert.ToDouble(paraToConvert[0].ChildNodes[0].Attributes["fontsize"].Value) >
                                Convert.ToDouble(paraToConvert[1].ChildNodes[0].Attributes["fontsize"].Value))
                            {
                                ln.SetAttribute("coord", paraToConvert[0].ChildNodes[0].Attributes["coord"].Value);
                                ln.SetAttribute("page", Convert.ToString(page));
                                ln.SetAttribute("height", paraToConvert[0].ChildNodes[0].Attributes["height"].Value);
                                ln.SetAttribute("left", paraToConvert[0].ChildNodes[0].Attributes["left"].Value);
                                ln.SetAttribute("top", paraToConvert[0].ChildNodes[0].Attributes["top"].Value);
                                ln.SetAttribute("font", paraToConvert[0].ChildNodes[0].Attributes["font"].Value);
                                ln.SetAttribute("fontsize",
                                    paraToConvert[0].ChildNodes[0].Attributes["fontsize"].Value);
                                ln.SetAttribute("error", paraToConvert[0].ChildNodes[0].Attributes["error"].Value);
                                ln.SetAttribute("ispreviewpassed",
                                    paraToConvert[0].ChildNodes[0].Attributes["ispreviewpassed"].Value);
                                ln.SetAttribute("isUserSigned",
                                    paraToConvert[0].ChildNodes[0].Attributes["isUserSigned"].Value);
                                ln.SetAttribute("isEditted",
                                    paraToConvert[0].ChildNodes[0].Attributes["isEditted"].Value);
                                ln.InnerText = paraToConvert[0].ChildNodes[0].InnerText;
                            }
                            else
                            {
                                ln.SetAttribute("coord", "0:0:0:0");
                                ln.SetAttribute("page", Convert.ToString(page));
                                ln.SetAttribute("height", "0");
                                ln.SetAttribute("left", "0");
                                ln.SetAttribute("top", "0");
                                ln.SetAttribute("font", "Arial");
                                ln.SetAttribute("fontsize", "12");
                                ln.SetAttribute("error", "0");
                                ln.SetAttribute("ispreviewpassed", "true");
                                ln.SetAttribute("isUserSigned", "1");
                                ln.SetAttribute("isEditted", "true");
                            }

                            boxTitleElem.AppendChild(ln);

                            paraToConvert[i].Attributes.RemoveNamedItem("abnormalLeft");
                            paraToConvert[i].Attributes.RemoveNamedItem("abnormalRight");
                            paraToConvert[i].Attributes.RemoveNamedItem("pType");

                            XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("upara");
                            ((XmlElement)convertedNode).SetAttribute("id", paraToConvert[i].Attributes["id"].Value);
                            ((XmlElement)convertedNode).SetAttribute("pnum", paraToConvert[i].Attributes["pnum"].Value);
                            convertedNode.InnerXml = paraToConvert[i].InnerXml;

                            boxElem.AppendChild(convertedNode);

                            paraToConvert[i].ParentNode.InsertBefore(boxElem, paraToConvert[i]);
                            objGlobal.SaveXml();
                            prevPage = page;
                        }
                        else
                        {
                            paraToConvert[i].Attributes.RemoveNamedItem("abnormalLeft");
                            paraToConvert[i].Attributes.RemoveNamedItem("abnormalRight");
                            paraToConvert[i].Attributes.RemoveNamedItem("pType");

                            XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("upara");
                            ((XmlElement)convertedNode).SetAttribute("id", paraToConvert[i].Attributes["id"].Value);
                            ((XmlElement)convertedNode).SetAttribute("pnum", paraToConvert[i].Attributes["pnum"].Value);
                            convertedNode.InnerXml = paraToConvert[i].InnerXml;

                            boxElem.AppendChild(convertedNode);

                            paraToConvert[i].ParentNode.InsertBefore(boxElem, paraToConvert[i]);
                            //objGlobal.SaveXml();
                        }
                    }

                    objGlobal.SaveXml();

                    for (int j = 0; j < paraToConvert.Count; j++)
                    {
                        paraToConvert[j].ParentNode.RemoveChild(paraToConvert[j]);
                        //objGlobal.SaveXml();
                    }
                    objGlobal.SaveXml();
                }

                //var tt = objGlobal.PBPDocument;
            }
            catch (Exception ex)
            {

            }
        }

        private void CreateNParaXmlInEndNode(List<XmlNode> paraToConvert)
        {
            try
            {
                if (paraToConvert != null)
                {
                    if (paraToConvert.Count > 0)
                    {
                        for (int i = 0; i < paraToConvert.Count; i++)
                        {
                            XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("npara");

                            ((XmlElement)convertedNode).SetAttribute("id", paraToConvert[i].Attributes["id"].Value);
                            ((XmlElement)convertedNode).SetAttribute("pnum", paraToConvert[i].Attributes["pnum"].Value);
                            //((XmlElement)convertedNode).SetAttribute("text-indent", paraToConvert[i].Attributes["text-indent"].Value);
                            //((XmlElement)convertedNode).SetAttribute("padding-bottom", paraToConvert[i].Attributes["padding-bottom"].Value);

                            var nParaLines = InsertNumTagInEndNode(paraToConvert[i]);

                            convertedNode.InnerXml = paraToConvert[i].InnerXml;

                            if (paraToConvert[i].ParentNode != null)
                            {
                                paraToConvert[i].ParentNode.InsertBefore(convertedNode, paraToConvert[i]);
                                //objGlobal.SaveXml();
                            }
                            else
                            {

                            }
                        }

                        for (int j = 0; j < paraToConvert.Count; j++)
                        {
                            paraToConvert[j].ParentNode.RemoveChild(paraToConvert[j]);
                            //objGlobal.SaveXml();
                        }

                        objGlobal.SaveXml();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        #endregion

        #region Common Methods

        public void ShowComplexBitByOrder()
        {
            //Order of processing

            //FootNotes
            //EndNotes
            //Box 
            //SPara
            //UPara
            //NPara

            populateSessions();
            objGlobal.XMLPath = Session["XMLPath"].ToString();
            if (!File.Exists(objGlobal.XMLPath)) return;
            objGlobal.LoadXml();

            if (BeforeComplexBitMappingList == null)
                btnUndoMapping.Visible = false;
            else
                btnUndoMapping.Visible = true;

            if (objGlobal.PBPDocument == null || (objGlobal.PBPDocument != null && string.IsNullOrEmpty(objGlobal.PBPDocument.InnerText.Trim()))) return;

            XmlNodeList allParas = objGlobal.PBPDocument.SelectNodes("//upara");

            if (allParas == null || allParas.Count == 0) return;

            double normalFontSize = 0;

            List<XmlNode> abnormalParasList = null;

            PdfFootNote footNote = GetFootNotePara(allParas);

            if (footNote == null || string.IsNullOrEmpty(footNote.FootNoteText))
            {
                PdfFootNote endNote = GetEndNotePara(allParas);

                if (endNote == null || string.IsNullOrEmpty(endNote.FootNoteText))
                {
                    abnormalParasList = GetBoxPara(allParas);

                    if (abnormalParasList == null || abnormalParasList.Count == 0)
                    {
                        abnormalParasList = GetSPara(allParas);

                        if (abnormalParasList == null || abnormalParasList.Count == 0)
                        {
                            abnormalParasList = GetNPara(allParas);

                            if (abnormalParasList == null || abnormalParasList.Count == 0)
                            {
                                abnormalParasList = GetUPara(allParas);

                                if (abnormalParasList == null || abnormalParasList.Count == 0)
                                {
                                    RemoveFootNoteTags();

                                    string strucToolRhywPath = Convert.ToString(Session["XMLPath"]).Replace(".rhyw", "_structoolcopy.rhyw");
                                    string rhywPath = Convert.ToString(Session["XMLPath"]);

                                    if (File.Exists(strucToolRhywPath))
                                        InsertPageBreaks(strucToolRhywPath);

                                    if (File.Exists(rhywPath))
                                        InsertPageBreaks(rhywPath);

                                    //To Do
                                    //InsertInlineMarkup();
                                    //RemoveExtraTags();

                                    //objGlobal.PBPDocument.Save("");

                                    BoxContainingPages = null;
                                    CurrentParaTypeForMapping = null;

                                    btnFinishCompMapping.Visible = true;
                                    divParaText.InnerText = "";

                                    if (!string.IsNullOrEmpty(Convert.ToString(Session["BID"])) && !string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
                                    {
                                        string finalFootNoteXml = objGlobal.PBPDocument.InnerXml.Replace("&lt;", "<").Replace("&gt;", ">");

                                        int inResult = objMyDBClass.CreateTask(Convert.ToString(Session["BID"]), "Unassigned", "MistakeInjection",
                                            Convert.ToString(Session["LoginId"]));
                                    }

                                    string applicationPath = "AdminPanel.aspx";
                                    //ucShowMessage1.ShowMessage(MessageTypes.Success,
                                    //    "Mapped Successfully! Click <a style='color:#4F8A10;text-decoration: underline;' href='" +
                                    //    applicationPath + "'>Finish</a> to continue");

                                    string msgText = "Mapped Successfully! Click <a style='color:#4F8A10;text-decoration: underline;' href='" +
                                        applicationPath + "'>Finish</a> to continue";

                                    ShowMessage(MessageTypes.Success, msgText);
                                }
                                else
                                {
                                    //ddlParaType.SelectedValue = "upara";
                                    //DisplayBoxAndNParas(ddlParaType.SelectedValue, abnormalParasList);

                                    string paraType = GetParaTypeFromLineAttr(abnormalParasList);
                                    //string xIndentXType = GetParaTypeFromLineAttr(abnormalParasList);

                                    if (!string.IsNullOrEmpty(paraType))
                                    {
                                        if (paraType.Equals("upara"))
                                        {
                                            ddlParaType.SelectedValue = "upara";
                                            DisplayBoxAndNParas(ddlParaType.SelectedValue, abnormalParasList);
                                        }
                                        else if (paraType.Equals("spara"))
                                        {
                                            Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "ShowSParaOptions()", true);
                                            ddlParaType.SelectedValue = "spara";
                                            DisplayBoxAndNParas(ddlParaType.SelectedValue, abnormalParasList);
                                        }
                                    }
                                    else
                                    {
                                        ddlParaType.SelectedValue = "upara";
                                        DisplayBoxAndNParas(ddlParaType.SelectedValue, abnormalParasList);
                                    }
                                }
                            }
                            else
                            {
                                ddlParaType.SelectedValue = "npara";
                                DisplayBoxAndNParas(ddlParaType.SelectedValue, abnormalParasList);
                            }
                        }
                        else
                        {
                            string paraType = GetParaTypeFromLineAttr(abnormalParasList);

                            if (!string.IsNullOrEmpty(paraType))
                            {
                                if (paraType.Equals("upara"))
                                {
                                    ddlParaType.SelectedValue = "upara";
                                    DisplayBoxAndNParas(ddlParaType.SelectedValue, abnormalParasList);
                                }
                                else if (paraType.Equals("spara"))
                                {
                                    Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "ShowSParaOptions()", true);
                                    ddlParaType.SelectedValue = "spara";
                                    DisplayBoxAndNParas(ddlParaType.SelectedValue, abnormalParasList);
                                }
                            }
                            else
                            {
                                Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "ShowSParaOptions()", true);
                                ddlParaType.SelectedValue = "spara";
                                DisplayBoxAndNParas(ddlParaType.SelectedValue, abnormalParasList);
                            }
                        }
                    }
                    else
                    {
                        ddlParaType.SelectedValue = "box";
                        DisplayBoxAndNParas(ddlParaType.SelectedValue, abnormalParasList);
                    }
                }
                else
                {
                    ddlParaType.SelectedValue = "endnote";
                    DisplayFootAndEndNote(ddlParaType.SelectedValue, allParas, endNote);
                }
            }
            else
            {
                ddlParaType.SelectedValue = "footnote";
                DetectedFootNotes = footNote;
                DisplayFootAndEndNote(ddlParaType.SelectedValue, allParas, footNote);
            }

            CurrentParaTypeForMapping = ddlParaType.SelectedValue;

            if (abnormalParasList != null && abnormalParasList.Count > 0)
                SelectedXmlParaNodes = abnormalParasList;
        }

        private void populateSessions()
        {
            string bookID = Request.QueryString["bid"] != null ? Request.QueryString["bid"].ToString() : "";
            if (bookID != "")
            {
                objGlobal.XMLPath = objMyDBClass.MainDirPhyPath + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID +
                                    "/TaggingUntagged/" + bookID + ".rhyw"; //Shoaib here update in Files Directory
                Session["XMLPath"] = objGlobal.XMLPath;
                objGlobal.PDFPath = objMyDBClass.MainDirPhyPath + "/" + bookID.Split(new char[] { '-' })[0] + "/" +
                                    bookID.Split(new char[] { '-' })[0] + ".pdf"; //Shoaib here update in Files Directory
                objGlobal.PBPDocument = new XmlDocument();
                Session["tempXML"] = objGlobal.PBPDocument;
                Session["rhywPath"] = objMyDBClass.MainDirPhyPath + "/" + bookID.Split(new char[] { '-' })[0] + "/" +
                                      bookID + "/TaggingUntagged/" + bookID + ".rhyw";
            }
        }

        public void ShowPDF(int page)
        {
            if (string.IsNullOrEmpty(Convert.ToString(Request.QueryString["bid"]))) return;

            try
            {
                lblPage.Text = Convert.ToString(page);

                if (!string.IsNullOrEmpty(Convert.ToString(Session["totalPages"])))
                    lblTotalPages.Text = Convert.ToString(Session["totalPages"]);

                string newPDFName = objMyDBClass.MainDirPhyPath + "/" + Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0] + "/" +
                                    Convert.ToString(Request.QueryString["bid"]) + "/TaggingUntagged/Page" + page + ".pdf";

                if (!File.Exists(newPDFName))
                    objConversionClass.ExtractPages(objGlobal.PDFPath, newPDFName, page, page);


                ////aamir temporary change
                //PDFViewerTarget.FilePath = Session["MainDirectory"].ToString() + "/" +
                //                           Request.QueryString["bid"].ToString().Split(new char[] { '-' })[0] + "/" +
                //                           Request.QueryString["bid"].ToString() + "/TaggingUntagged/Page" + pageno +
                //                           ".pdf#toolbar=0";
                PDFViewerTarget.FilePath = "DisplayPdf.ashx?bid=" + Convert.ToString(Request.QueryString["bid"]) + "&page=" + page;
            }

            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        public void ClearComplexBitSession()
        {
            BoxBgColors = null;
            BoxConvertedPagesList = null;
            BoxContainingPages = null;
            SelectedXmlParaNodes = null;
            BeforeComplexBitMappingList = null;
            CurrentParaTypeForMapping = null;
            SelectedTreeViewSection = null;
        }

        public void SetPdfPageIndentations()
        {
            populateSessions();
            objGlobal.XMLPath = Session["XMLPath"].ToString();

            if (!File.Exists(objGlobal.XMLPath)) return;
            objGlobal.LoadXml();

            if (objGlobal.PBPDocument == null || (objGlobal.PBPDocument != null && string.IsNullOrEmpty(objGlobal.PBPDocument.InnerText.Trim()))) return;

            PdfIndentation pdfInd = new PdfIndentation();

            NormalXIndentXCompletePdf(objGlobal.PBPDocument, pdfInd, "even");
            NormalXIndentXCompletePdf(objGlobal.PBPDocument, pdfInd, "odd");

            if (pdfInd.NormalXEvenPages > 0 &&
                pdfInd.NormalIndentXEvenPages > 0 &&
                pdfInd.NormalFontSizeEvenPages > 0 &&
                pdfInd.NormalXOddPages > 0 &&
                pdfInd.NormalIndentXOddPages > 0 &&
                pdfInd.NormalFontSizeOddPages > 0)
            {
                PdfIndentDetails = pdfInd;
            }
        }

        public PdfIndentation NormalXIndentXCompletePdf(XmlNode mainDoc, PdfIndentation pdfInd, string pageType)
        {
            if (mainDoc == null || (mainDoc != null && string.IsNullOrEmpty(mainDoc.InnerText.Trim()))) return null;

            double normalFontSize = 0;
            List<int> pagesList = null;

            if (pageType.Equals("even"))
            {
                pagesList = mainDoc.SelectNodes("//@page").Cast<XmlNode>().Where(x => Convert.ToInt32(x.Value) % 2 == 0)
                      .Select(y => Convert.ToInt32(y.Value)).Distinct().ToList();

                normalFontSize = GetNormalFontSize(pagesList);
            }
            else
            {
                pagesList = mainDoc.SelectNodes("//@page").Cast<XmlNode>().Where(x => Convert.ToInt32(x.Value) % 2 != 0)
                                                                              .Select(y => Convert.ToInt32(y.Value)).Distinct().ToList();

                normalFontSize = GetNormalFontSize(pagesList);
            }

            if (pagesList.Count == 0 || normalFontSize == 0) return null;

            double normalX = 0;
            double normalEndX = 0;
            double normalIndentX = 0;
            double normalY = 0;

            //double normalFontSize = 0;
            //double normalLlxCoord = 0;
            //double normalUrxCoord = 0;

            double valueMargin = 10;
            double indentXvalueMargin = 25;

            List<double> pdfFontSizeList = new List<double>();
            List<double> pdfLlxCoordList = new List<double>();
            List<double> pdfUrxCoordList = new List<double>();

            foreach (int pageNum in pagesList)
            {
                if (pageNum >= 130) break;

                //pdfFontSizeList.AddRange(objGlobal.PBPDocument.SelectNodes("//ln[@page='" + pageNum + "' and font ='" + normalFontSize + "']/@fontsize")
                //                                  .Cast<XmlNode>().Select(x => Convert.ToDouble(x.Value)).ToList());

                pdfLlxCoordList.AddRange(objGlobal.PBPDocument.SelectNodes("//ln[@page=" + pageNum + " and @fontsize =" + normalFontSize + "]/@left")
                                                  .Cast<XmlNode>().Select(x => Convert.ToDouble(x.Value)).ToList());

                pdfUrxCoordList.AddRange(objGlobal.PBPDocument.SelectNodes("//ln[@page=" + pageNum + " and @fontsize =" + normalFontSize + "]/@coord")
                                                  .Cast<XmlNode>().Select(x => Convert.ToDouble(x.Value.Split(':')[2])).ToList());
            }

            if (pdfLlxCoordList.Count > 0 && pdfUrxCoordList.Count > 0)
            {
                //var allFontSizes = pdfFontSizeList.GroupBy(x => x).Select(group => new { value = group.Key, Count = group.Count() })
                //                              .OrderByDescending(x => x.Count).ToList();

                //if (allFontSizes.Count > 0)
                //    normalFontSize = allFontSizes[0].value;

                var allLlxCoords = pdfLlxCoordList.GroupBy(x => x).Select(group => new { value = group.Key, Count = group.Count() })
                                             .OrderByDescending(x => x.Count).ToList();

                //if (allLlxCoords.Count > 0)
                //    normalLlxCoord = allLlxCoords[0].value;


                if (allLlxCoords.Count > 0)
                {
                    normalX = allLlxCoords[0].value;
                }

                var indentXList = allLlxCoords.Where(x => !x.Equals(normalX)).OrderByDescending(y => y.Count).ToList();

                if (indentXList.Count > 0)
                {
                    for (int i = 0; i < indentXList.Count; i++)
                    {
                        if (Math.Abs(normalX - indentXList[i].value) > valueMargin && Math.Abs(normalX - indentXList[i].value) < indentXvalueMargin)
                        {
                            normalIndentX = indentXList[i].value;
                            break;
                        }
                    }
                }

                var allUrxCoords = pdfUrxCoordList.GroupBy(x => x).Select(group => new { value = group.Key, Count = group.Count() })
                                            .OrderByDescending(x => x.Count).ToList();

                if (allUrxCoords.Count > 0)
                    normalEndX = allUrxCoords[0].value;
            }

            double temp = 0;

            if (normalX > normalIndentX)
            {
                temp = normalX;
                normalX = normalIndentX;
                normalIndentX = temp;
            }

            normalX = Math.Floor(normalX);
            normalIndentX = Math.Floor(normalIndentX);

            if (pageType.Equals("even"))
            {
                pdfInd.NormalIndentXEvenPages = normalIndentX;
                pdfInd.NormalXEvenPages = normalX;
                pdfInd.NormalEndxEvenPages = normalEndX;
                pdfInd.NormalYEvenPages = normalY;
                pdfInd.NormalFontSizeEvenPages = normalFontSize;
            }
            else
            {
                pdfInd.NormalIndentXOddPages = normalIndentX;
                pdfInd.NormalXOddPages = normalX;
                pdfInd.NormalEndXOddPages = normalEndX;
                pdfInd.NormalYOddPages = normalY;
                pdfInd.NormalFontSizeOddPages = normalFontSize;
            }

            return pdfInd;
        }

        private double GetNormalFontSize(List<int> pagesList)
        {
            List<double> normalFontSizeList = new List<double>();
            double normalFontSize = 0;

            foreach (int pageNum in pagesList)
            {
                if (pageNum >= 130) break;

                normalFontSizeList.AddRange(
                    objGlobal.PBPDocument.SelectNodes("//ln[@page='" + pageNum + "']/@fontsize").Cast<XmlNode>()
                        .Select(x => Convert.ToDouble(x.Value)).ToList());
            }

            if (normalFontSizeList.Count > 0)
            {
                var allFontSizes =
                    normalFontSizeList.GroupBy(x => x).Select(group => new { value = @group.Key, Count = @group.Count() })
                        .OrderByDescending(x => x.Count).ToList();

                if (allFontSizes.Count > 0)
                    normalFontSize = allFontSizes[0].value;
            }
            return normalFontSize;
        }

        public void SetTotaPages()
        {
            try
            {
                string pdfPath = objMyDBClass.MainDirPhyPath + "/" + Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0] + "/" +
                                       Convert.ToString(Request.QueryString["bid"]) + "/TaggingUntagged/" + Convert.ToString(Request.QueryString["bid"]) + ".pdf";

                PdfReader srcPdf = new PdfReader(pdfPath);
                int srcTotalPages = srcPdf.NumberOfPages;
                srcPdf.Close();

                Session["totalPages"] = srcTotalPages;
            }
            catch (Exception)
            {
                Session["totalPages"] = "0";
            }
        }

        public bool IsContainsIndentX(XmlNode para, double normalX, double normalIndentX, double normalFontSize, string paraType)
        {
            if (para == null || normalIndentX == 0.0 || normalFontSize == 0.0)
                return false;

            double marginVal = 5.0;

            if (paraType.Equals("otherline"))
            {
                normalIndentX = normalX;
            }

            //var lineList = para.SelectNodes("Line").Cast<XmlNode>().Where(x => x.Attributes != null &&
            //                                                                    x.Attributes["x1"] != null &&
            //                                                                    x.Attributes["fontsize"] != null && 
            //                                                                    Math.Abs(Convert.ToDouble(x.Attributes["x1"].Value) - normalIndentX) < marginVal &&
            //                                                                    Convert.ToDouble(x.Attributes["fontsize"].Value).Equals(normalFontSize)).ToList();

            var lineList = para.SelectNodes("descendant::ln").Cast<XmlNode>().Where(x => x.Attributes != null &&
                                                                              x.Attributes["left"] != null &&
                                                                              x.Attributes["fontsize"] != null &&
                                                                              Math.Abs(Convert.ToDouble(x.Attributes["left"].Value) - normalIndentX) < marginVal &&
                                                                              Convert.ToDouble(x.Attributes["fontsize"].Value).Equals(normalFontSize)).ToList();

            if (lineList.Count > 0)
                return true;

            return false;
        }

        public XmlElement SubSplitting(XmlDocument xmlDoc, XmlNode para)
        {
            XmlElement xparaElem = xmlDoc.CreateElement(para.Name);

            if (para != null && para.Attributes != null)
            {
                if (para.Attributes["id"] != null)
                {
                    XmlAttribute idAttr = xmlDoc.CreateAttribute("id");
                    idAttr.Value = para.Attributes["id"].Value;
                    xparaElem.Attributes.Append(idAttr);
                }

                if (para.Attributes["pnum"] != null)
                {
                    XmlAttribute pnumAttr = xmlDoc.CreateAttribute("pnum");
                    pnumAttr.Value = para.Attributes["pnum"].Value;
                    xparaElem.Attributes.Append(pnumAttr);
                }
            }
            return xparaElem;
        }

        public void CompleteCompBitMapTask()
        {
            if (!string.IsNullOrEmpty(Request.QueryString["bid"]))
            {
                string bookId = Request.QueryString["bid"].Split(new char[] { '-' })[0];
                string bid = objMyDBClass.ExecuteSelectCom("select bid from book where mainbook like '" + bookId + "%'");
                string queryUpdate = "Update ACTIVITY Set Status='Approved' Where BID=" + bid + " AND Task='ComplexBitsMapping' AND Status='Working'";

                objMyDBClass.ExecuteCommand(queryUpdate);
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

        public string GetParaTypeFromLineAttr(List<XmlNode> abnormalParasList)
        {
            if (abnormalParasList != null &&
                abnormalParasList.Count > 0 &&
                abnormalParasList[0].ChildNodes.Count > 0 &&
                PdfIndentDetails != null)
            {
                XmlNode line = abnormalParasList[0].ChildNodes[0];

                if (abnormalParasList[0].ChildNodes.Count == 1)
                {
                    if (line.Attributes != null &&
                   line.Attributes["fontsize"] != null &&
                   line.Attributes["left"] != null &&
                   line.Attributes["page"] != null)
                    {
                        if (
                                (Convert.ToInt32(line.Attributes["page"].Value) % 2 == 0 &&
                                Convert.ToDouble(line.Attributes["fontsize"].Value).Equals(PdfIndentDetails.NormalFontSizeEvenPages) &&
                                Math.Abs(Convert.ToDouble(line.Attributes["left"].Value) - PdfIndentDetails.NormalIndentXEvenPages) < 3)

                                ||

                                (Convert.ToInt32(line.Attributes["page"].Value) % 2 != 0 &&
                                 Convert.ToDouble(line.Attributes["fontsize"].Value).Equals(PdfIndentDetails.NormalFontSizeOddPages) &&
                                 Math.Abs(Convert.ToDouble(line.Attributes["left"].Value) - PdfIndentDetails.NormalIndentXOddPages) < 3)
                            )
                        {
                            return "upara";
                        }
                    }
                }
                else if (abnormalParasList[0].ChildNodes.Count > 1)
                {
                    XmlNode secondLine = abnormalParasList[0].ChildNodes[1];

                    if (line.Attributes != null &&
                   line.Attributes["fontsize"] != null &&
                   line.Attributes["left"] != null &&
                   line.Attributes["page"] != null &&
                   secondLine.Attributes != null &&
                   secondLine.Attributes["fontsize"] != null &&
                   secondLine.Attributes["left"] != null &&
                   secondLine.Attributes["page"] != null)
                    {
                        if (
                                (Convert.ToInt32(line.Attributes["page"].Value) % 2 == 0 &&
                                Convert.ToDouble(line.Attributes["fontsize"].Value).Equals(PdfIndentDetails.NormalFontSizeEvenPages) &&
                                Math.Abs(Convert.ToDouble(line.Attributes["left"].Value) - PdfIndentDetails.NormalXEvenPages) < 3 &&
                                Math.Abs(Convert.ToDouble(secondLine.Attributes["left"].Value) - PdfIndentDetails.NormalIndentXEvenPages) < 3 &&
                                Math.Abs(Convert.ToDouble(line.Attributes["coord"].Value.Split(':')[2]) - PdfIndentDetails.NormalEndxEvenPages) < 3)

                                ||

                                (Convert.ToInt32(line.Attributes["page"].Value) % 2 != 0 &&
                                 Convert.ToDouble(line.Attributes["fontsize"].Value).Equals(PdfIndentDetails.NormalFontSizeOddPages) &&
                                 Math.Abs(Convert.ToDouble(line.Attributes["left"].Value) - PdfIndentDetails.NormalXOddPages) < 3 &&
                                 Math.Abs(Convert.ToDouble(secondLine.Attributes["left"].Value) - PdfIndentDetails.NormalIndentXOddPages) < 3 &&
                                 Math.Abs(Convert.ToDouble(line.Attributes["coord"].Value.Split(':')[2]) - PdfIndentDetails.NormalIndentXOddPages) < 3)
                            )
                        {
                            return "upara";
                        }
                    }
                }
            }
            return "spara";
        }

        private void InsertPageBreaks(string xmlPath)
        {
            try
            {
                string bookId = Request.QueryString["bid"].Split(new char[] { '-' })[0];
                BookPageBreak pageInfo = null;
                int bodyStartPage = 0;
                int bodyEndPage = 0;

                if (!string.IsNullOrEmpty(bookId))
                {
                    string bid = objMyDBClass.ExecuteSelectCom("select bid from book where mainbook like '" + bookId + "%'");

                    if (!string.IsNullOrEmpty(bid))
                    {
                        MyDBClass obj = new MyDBClass();
                        pageInfo = obj.GetBookPageBreak(bid);
                    }
                }

                if (pageInfo != null)
                {
                    bodyStartPage = pageInfo.BodyStart;
                    bodyEndPage = pageInfo.BodyEnd;

                    populateSessions();
                    // objGlobal.XMLPath = Session["XMLPath"].ToString();

                    objGlobal.XMLPath = xmlPath;

                    if (!File.Exists(objGlobal.XMLPath)) return;

                    objGlobal.LoadXml();

                    int pageBreakOffset = pageInfo.PageBreak - bodyStartPage;

                    int currentPage = 1;
                    //int id = bodyStartPage;

                    var allParas = objGlobal.PBPDocument.SelectNodes("//ln/ancestor::upara|//ln/ancestor::npara|//ln/ancestor::spara|//ln/ancestor::image");

                    if (allParas != null && allParas.Count > 0)
                    {
                        for (int p = 0; p < allParas.Count; p++)
                        {
                            var pagesInParaList = allParas[p].SelectNodes("descendant::ln").Cast<XmlNode>()
                                                        .Where(x => x.Attributes != null && x.Attributes["page"] != null)
                                                        .Select(y => Convert.ToInt32(y.Attributes["page"].Value)).Distinct().ToList();

                            if (pagesInParaList.Count > 0)
                            {
                                currentPage = pagesInParaList[0];

                                if (currentPage < bodyStartPage)
                                    DeletePageBreaks(allParas[p], pageInfo.PageBreak, bodyStartPage, bodyEndPage);
                            }

                            //Case 1 - When para ends on same page
                            if (p + 1 < allParas.Count && IsNotContinuePara(allParas[p], allParas[p + 1], currentPage, bodyStartPage, bodyEndPage))
                            {
                                var paraLines = allParas[p].SelectNodes("descendant::ln");

                                if (paraLines != null && paraLines.Count > 0)
                                {
                                    XmlNode pageBreak = CreatePageBreakTag(currentPage, pageBreakOffset);

                                    if (pageBreak != null && paraLines[0].ParentNode != null && paraLines[0].ParentNode.Name.Equals("image"))
                                    {
                                        if (paraLines[0].ChildNodes.Count > 0 &&
                                            paraLines[0].ChildNodes[0].Name.Equals("break"))
                                        {
                                            paraLines[0].ChildNodes[0].ParentNode.RemoveChild(paraLines[0].ChildNodes[0]);
                                        }
                                        paraLines[0].AppendChild(pageBreak);
                                    }
                                    else
                                    {
                                        if (paraLines[paraLines.Count - 1].ParentNode != null)
                                        {
                                            if (paraLines[paraLines.Count - 1].NextSibling != null &&
                                                paraLines[paraLines.Count - 1].NextSibling.Name.Equals("break") &&
                                                paraLines[paraLines.Count - 1].ParentNode != null)
                                            {
                                                paraLines[paraLines.Count - 1].ParentNode.RemoveChild(paraLines[paraLines.Count - 1].NextSibling);
                                            }

                                            paraLines[paraLines.Count - 1].ParentNode.InsertAfter(pageBreak,
                                                paraLines[paraLines.Count - 1]);
                                        }
                                    }

                                    //id++;
                                    currentPage++;
                                }
                            }//end case 1

                            //Case 2 - When para continues on two pages
                            var lines = allParas[p].SelectNodes("descendant::ln");

                            if (lines != null && lines.Count > 0)
                            {
                                for (int i = 0; i < lines.Count; i++)
                                {
                                    currentPage = Convert.ToInt32(lines[i].Attributes["page"].Value);

                                    if ((i + 1 < lines.Count &&
                                         lines[i + 1].Attributes != null &&
                                         lines[i + 1].Attributes["page"] != null &&
                                         currentPage >= bodyStartPage &&
                                         currentPage <= bodyEndPage &&
                                         Convert.ToInt32(lines[i + 1].Attributes["page"].Value) > currentPage) ||
                                         (p == allParas.Count - 1 && i == lines.Count - 1 && currentPage <= bodyEndPage))
                                    {
                                        XmlNode pageBreak = CreatePageBreakTag(currentPage, pageBreakOffset);

                                        if (pageBreak != null && lines[i].ParentNode != null)
                                        {
                                            if (lines[i].NextSibling != null &&
                                                lines[i].NextSibling.Name.Equals("break") &&
                                                lines[i].ParentNode != null)
                                            {
                                                lines[i].ParentNode.RemoveChild(lines[i].NextSibling);
                                            }

                                            lines[i].ParentNode.InsertAfter(pageBreak, lines[i]);
                                        }

                                        //id++;
                                        currentPage++;
                                        break;
                                    }
                                }
                            }//end case 2
                        }//end for loop para

                        objGlobal.SaveXml();
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private XmlNode CreatePageBreakTag(int currentPage, int pageBreakOffset)
        {
            XmlNode pageBreak = objGlobal.PBPDocument.CreateElement("break");

            XmlAttribute typeAttr = objGlobal.PBPDocument.CreateAttribute("type");
            typeAttr.Value = "page";

            XmlAttribute numAttr = objGlobal.PBPDocument.CreateAttribute("num");
            numAttr.Value = Convert.ToString(currentPage + pageBreakOffset);

            XmlAttribute idAttr = objGlobal.PBPDocument.CreateAttribute("id");
            idAttr.Value = Convert.ToString(currentPage);

            pageBreak.Attributes.Append(typeAttr);
            pageBreak.Attributes.Append(numAttr);
            pageBreak.Attributes.Append(idAttr);

            return pageBreak;
        }

        public bool IsNotContinuePara(XmlNode currentPara, XmlNode nextPara, int currentPage, int bodyStartPage, int bodyEndPage)
        {
            if (nextPara != null && currentPage > 0)
            {
                var currentPageLinesList = currentPara.SelectNodes("descendant::ln").Cast<XmlNode>().Where(x => x.Attributes != null && x.Attributes["page"] != null)
                                               .Select(y => Convert.ToInt32(y.Attributes["page"].Value)).Where(z => z > currentPage).ToList();

                var nextPageLinesList = nextPara.SelectNodes("descendant::ln").Cast<XmlNode>().Where(x => x.Attributes != null && x.Attributes["page"] != null)
                                                .Select(y => Convert.ToInt32(y.Attributes["page"].Value)).ToList();

                if (currentPageLinesList.Count == 0 &&
                    nextPageLinesList.Count > 0 &&
                    nextPageLinesList[0] > currentPage &&
                    currentPage >= bodyStartPage &&
                    currentPage <= bodyEndPage)
                    return true;
            }

            return false;
        }

        public void DeletePageBreaks(XmlNode currentPara, int pageBreak, int bodyStartPage, int bodyEndPage)
        {
            if (currentPara != null && pageBreak > 0 && bodyStartPage > 0 && bodyEndPage > 0)
            {
                XmlNode pageBreakNode = currentPara.SelectSingleNode("descendant::break");

                if (pageBreakNode != null && pageBreakNode.ParentNode != null)
                    pageBreakNode.ParentNode.RemoveChild(pageBreakNode);
            }
        }

        public void InsertInlineMarkup(string rhywPath)
        {
            //try
            //{
            //    objGlobarVar.XMLPath = rhywPath;
            //    objGlobarVar.LoadXml();
            //    MatchCollection keyCol;
            //    keyCol = Regex.Matches(objGlobarVar.PBPDocument.InnerXml, @"[\w-]{20,100}");
            //    for (int i = 0; i < keyCol.Count; i++)
            //    {
            //        if (keyCol[i].Value != "noNamespaceSchemaLocation")
            //        {
            //            objGlobarVar.PBPDocument.InnerXml = objGlobarVar.PBPDocument.InnerXml.Replace(keyCol[i].Value, "<inline-markup type=\"hyphenated-word\">" + keyCol[i].Value + "</inline-markup>");
            //        }
            //    }

            //    XmlNodeList inlineNodes = objGlobarVar.PBPDocument.SelectNodes("//inline-markup");
            //    foreach (XmlNode inlineNode in inlineNodes)
            //    {
            //        if (inlineNode.Attributes["type"].Value == "url")
            //        {
            //            keyCol = Regex.Matches(objGlobarVar.PBPDocument.InnerXml, "<inline-markup type=\"url\">" + inlineNode.InnerXml + @"</inline-markup>");
            //            for (int i = 0; i < keyCol.Count; i++)
            //            {

            //                string innerUrl = keyCol[i].Value.Replace("<inline-markup type=\"url\">", "").Replace("</inline-markup>", "");
            //                string txttoPrepend = "";
            //                string txttoAppend = "";
            //                while (innerUrl.Substring(innerUrl.Length - 1) == " ")
            //                {
            //                    txttoPrepend = innerUrl.Substring(innerUrl.Length - 1) + txttoPrepend;
            //                    innerUrl = innerUrl.Remove(innerUrl.Length - 1, 1);
            //                }
            //                if (char.IsPunctuation(innerUrl.Substring(innerUrl.Length - 1).ToCharArray()[0]))
            //                {
            //                    txttoPrepend = innerUrl.Substring(innerUrl.Length - 1) + txttoPrepend;
            //                    innerUrl = innerUrl.Remove(innerUrl.Length - 1, 1);
            //                }
            //                if (innerUrl.Contains("&lt;"))
            //                {
            //                    txttoAppend = "&lt;";
            //                }
            //                if (innerUrl.Contains("&gt;"))
            //                {
            //                    txttoPrepend = "&gt;" + txttoPrepend;
            //                }

            //                objGlobarVar.PBPDocument.InnerXml = objGlobarVar.PBPDocument.InnerXml.Replace(keyCol[i].Value, txttoAppend + "<inline-markup type=\"url\">" + innerUrl.Replace(" ", "") + "</inline-markup>" + txttoPrepend);
            //            }
            //        }
            //    }
            //    objGlobarVar.SaveXml();
            //}
            //catch (Exception ex)
            //{

            //    throw ex;
            //}
        }

        public void DisplayBoxAndNParas(string selectedParaType, List<XmlNode> abnormalParaList)
        {
            int pageNum = 0;

            StringBuilder sbParaText = new StringBuilder();

            if (abnormalParaList != null && abnormalParaList.Count > 0)
            {
                XmlNode line = abnormalParaList[0].ChildNodes[0];

                if (line.Attributes != null && line.Attributes["page"] != null)
                {
                    pageNum = Convert.ToInt32(line.Attributes["page"].Value);

                    string sourcePagePath = objMyDBClass.MainDirPhyPath + "/" +
                                        Request.QueryString["bid"].ToString().Split(new char[] { '-' })[0] + "/" +
                                        Request.QueryString["bid"].ToString() + "/TaggingUntagged/Page" + pageNum + ".pdf";

                    if (File.Exists(sourcePagePath))
                        File.Delete(sourcePagePath);

                    if (!File.Exists(sourcePagePath))
                        objConversionClass.ExtractPages(objGlobal.PDFPath, sourcePagePath, pageNum, pageNum);

                    //string sourceTetmlPath = "";
                    string outPutFilePath = sourcePagePath.Replace(".pdf", "_Highlighted.pdf");

                    if (File.Exists(outPutFilePath))
                        File.Delete(outPutFilePath);

                    List<string> lstcoordiants = GetParaCoordinates(abnormalParaList, Convert.ToInt32(line.Attributes["page"].Value), sourcePagePath);

                    if (lstcoordiants != null && lstcoordiants.Count > 0)
                        HighLightSelectedParas(sourcePagePath, outPutFilePath, BaseColor.ORANGE, lstcoordiants);
                }
                ShowPDF(pageNum);

                //foreach (XmlNode abnNode in abnormalParaList)
                //{
                //    foreach (XmlNode ln in abnNode.ChildNodes)
                //    {
                //        sbParaText.Append(ln.InnerText.Trim() + "</br>");
                //    }
                //}

                if (abnormalParaList != null && abnormalParaList.Count > 0)
                {
                    for (int i = 0; i < abnormalParaList.Count; i++)
                    {
                        if (i > 0) sbParaText.Append("</br>");

                        for (int j = 0; j < abnormalParaList[i].ChildNodes.Count; j++)
                        {
                            //sbParaText.Append(abnormalParaList[i].ChildNodes[j].InnerText.Trim() + "</br>");
                            sbParaText.Append(Server.HtmlEncode(abnormalParaList[i].ChildNodes[j].InnerText.Trim()) + "</br>");
                        }
                    }
                }

                //divParaText.InnerHtml = "<font style='color:#4682b4'><sup>" + Convert.ToString(sbParaText) + "</sup></font>";
                divParaText.InnerHtml = Convert.ToString(sbParaText);
                //sbParaText.Length = 0;
            }
            else
            {
                divParaText.InnerText = "";

                string bookID = Request.QueryString["bid"] != null ? Request.QueryString["bid"].ToString() : "";
                bookID = bookID.Split(new char[] { '-' })[0];
                string dbBookId =
                    objMyDBClass.ExecuteSelectCom("select bid from book where mainbook like '" + bookID + "%'");
                string queryUpdate;
                if (dbBookId != "0")
                {
                    queryUpdate = "Update ACTIVITY Set Status='Approved' Where BID=" + dbBookId + " AND Task='TaggingUntagged' AND Status='Working'";
                }
                else
                {
                    queryUpdate = "Update ACTIVITY Set Status='Approved' Where BID=" + bookID + " AND Task='TaggingUntagged' AND Status='Working'";
                }
                objMyDBClass.ExecuteCommand(queryUpdate);
                AutoMapService.AutoMappService autoMapSvc = new AutoMapService.AutoMappService();
                autoMapSvc.AllowAutoRedirect = true;
            }
        }

        private List<String> GetParaCoordinates(List<XmlNode> abnormalParas, int page, string pdfPath)
        {
            if (abnormalParas == null || abnormalParas.Count == 0) return null;

            try
            {
                List<string> lstcoordiants = new List<string>();
                double llx = 0;
                double lly = 0;
                double urx = 0;
                double ury = 0;
                double left = 0;
                double bottom = 0;

                List<double> rightValues = new List<double>();
                List<double> leftValues = new List<double>();
                List<double> topValues = new List<double>();
                List<double> bottomValues = new List<double>();

                //foreach (XmlNode para in abnormalParas)
                //{
                //    leftValues.AddRange(para.SelectNodes("descendant::ln[@coord]/@coord").Cast<XmlNode>()
                //                             .Where(x => !string.IsNullOrEmpty(x.Value) && x.Value.Split(':').Length > 3)
                //                             .Select(x => Convert.ToDouble(x.Value.Split(':')[0])));

                //    bottomValues.AddRange(para.SelectNodes("descendant::ln[@coord]/@coord").Cast<XmlNode>()
                //                             .Where(x => !string.IsNullOrEmpty(x.Value) && x.Value.Split(':').Length > 3)
                //                             .Select(x => Convert.ToDouble(x.Value.Split(':')[1])));

                //    rightValues.AddRange(para.SelectNodes("descendant::ln[@coord]/@coord").Cast<XmlNode>()
                //                             .Where(x => !string.IsNullOrEmpty(x.Value) && x.Value.Split(':').Length > 3)
                //                             .Select(x => Convert.ToDouble(x.Value.Split(':')[2])));

                //    topValues.AddRange(para.SelectNodes("descendant::ln[@coord]/@coord").Cast<XmlNode>()
                //                             .Where(x => !string.IsNullOrEmpty(x.Value) && x.Value.Split(':').Length > 3)
                //                             .Select(x => Convert.ToDouble(x.Value.Split(':')[3])));
                //}

                foreach (XmlNode para in abnormalParas)
                {
                    leftValues.AddRange(para.SelectNodes("descendant::ln[@page='" + page + "']/@coord").Cast<XmlNode>()
                                             .Where(x => !string.IsNullOrEmpty(x.Value) && x.Value.Split(':').Length > 3)
                                             .Select(x => Convert.ToDouble(x.Value.Split(':')[0])));

                    bottomValues.AddRange(para.SelectNodes("descendant::ln[@page='" + page + "']/@coord").Cast<XmlNode>()
                                             .Where(x => !string.IsNullOrEmpty(x.Value) && x.Value.Split(':').Length > 3)
                                             .Select(x => Convert.ToDouble(x.Value.Split(':')[1])));

                    rightValues.AddRange(para.SelectNodes("descendant::ln[@page='" + page + "']/@coord").Cast<XmlNode>()
                                             .Where(x => !string.IsNullOrEmpty(x.Value) && x.Value.Split(':').Length > 3)
                                             .Select(x => Convert.ToDouble(x.Value.Split(':')[2])));

                    topValues.AddRange(para.SelectNodes("descendant::ln[@page='" + page + "']/@coord").Cast<XmlNode>()
                                             .Where(x => !string.IsNullOrEmpty(x.Value) && x.Value.Split(':').Length > 3)
                                             .Select(x => Convert.ToDouble(x.Value.Split(':')[3])));
                }

                llx = leftValues.Min();
                lly = bottomValues.Min();
                urx = rightValues.Max();
                ury = topValues.Max();

                double width = Convert.ToDouble(urx) - Convert.ToDouble(llx);
                double height = Convert.ToDouble(ury) - Convert.ToDouble(lly);
                string croppedMargins = getCroppedMargins(pdfPath);

                if (!string.IsNullOrEmpty(croppedMargins))
                {
                    List<string> tempValues = croppedMargins.Split(' ').ToList();

                    if (tempValues != null)
                    {
                        if (tempValues.Count > 0)
                        {
                            left = Math.Abs(Convert.ToDouble(tempValues[3]));
                            bottom = Math.Abs(Convert.ToDouble(tempValues[1]));
                        }
                    }
                }

                lstcoordiants.Add((Convert.ToDouble(llx) + left) + " " + (Convert.ToDouble(lly) + bottom) + " " + height + " " + width + " " + page);

                return lstcoordiants;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private string getCroppedMargins(string mainPdfPath)
        {
            try
            {
                PdfReader reader = new PdfReader(File.ReadAllBytes(mainPdfPath));
                iTextSharp.text.Rectangle cropbox = reader.GetCropBox(1);
                var box = reader.GetPageSizeWithRotation(1);

                double top = (box.Top - cropbox.Top);
                double bottom = cropbox.Bottom;
                double right = (box.Right - cropbox.Right);
                double left = cropbox.Left;
                return Math.Round(top, 2) + " " + Math.Round(bottom, 2) + " " + Math.Round(right, 2) + " " + Math.Round(left, 2);
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private string HighLightSelectedParas(string inFilePath, string outputFilePath, BaseColor color, List<string> lstcoordinates)
        {
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

                        //var yy = dimensions[4];
                        //var tt = Convert.ToInt32(dimensions[4]);

                        //stamper.GetOverContent(Convert.ToInt32(dimensions[4])).SetGState(_state);
                        //stamper.GetOverContent(Convert.ToInt32(dimensions[4])).AddImage(objImage1, true);

                        stamper.GetOverContent(1).SetGState(_state);
                        stamper.GetOverContent(1).AddImage(objImage1, true);
                    }
                    stamper.Close();
                }
                lstcoordinates.Clear();
                File.Delete(inFilePath);
                File.Copy(outputFilePath, inFilePath);
                File.Delete(outputFilePath);
                return outputFilePath;
            }
            catch (Exception)
            {
                return null;
            }
        }

        private XmlNode GetSection(XmlNode node)
        {

            while (!node.Name.Equals("section"))
            {
                if (node.ParentNode == null)
                {
                    return node;
                }
                node = node.ParentNode;

            }
            return node;

        }

        private bool CheckLevelSequence(string first, string second)
        {
            int a = 0, b = 0;

            switch (first)
            {
                case "level1":
                    a = 1;
                    break;
                case "level2":
                    a = 2;
                    break;
                case "level3":
                    a = 3;
                    break;
                case "level4":
                    a = 4;
                    break;
                default:
                    break;
            }
            switch (second)
            {
                case "level1":
                    b = 1;
                    break;
                case "level2":
                    b = 2;
                    break;
                case "level3":
                    b = 3;
                    break;
                case "level4":
                    b = 4;
                    break;
                default:
                    break;
            }
            if (a > b) return false;
            else return true;
        }

        private XmlNode CheckSectionType(XmlNode node)
        {

            while (!node.Name.Equals("pbp-front"))
            {
                if (node.ParentNode == null)
                {
                    return node;
                }
                else if (node.Name.Equals("pbp-end"))
                {
                    return node;
                }
                node = node.ParentNode;

            }

            return node;
        }

        public void AddSection(string type, int newlevel)
        {
            LoadPdfXml();

            XmlNode abnormalPara = objGlobal.PBPDocument.SelectSingleNode("(//*[@abnormalLeft])[1]");
            XmlNodeList ParaToConvert =
                objGlobal.PBPDocument.SelectNodes("//upara[@abnormalLeft='" + abnormalPara.Attributes["abnormalLeft"].Value +
                                                  "']");
            SchemaElements objSchemaElem = new SchemaElements();
            foreach (XmlNode currentNode in ParaToConvert)
            {
                string PreOrPostSection = CheckSectionType(currentNode).Name;
                XmlElement section;

                if (PreOrPostSection.Equals("pbp-front"))
                {
                    if (type.Equals("other"))
                    {
                        section = objSchemaElem.CreatePreSection("pre-section", "1", objGlobal.PBPDocument);
                    }
                    else
                    {
                        section = objSchemaElem.CreateSection(type, "1", objGlobal.PBPDocument);
                    }
                }
                else if (PreOrPostSection.Equals("pbp-end"))
                {
                    if (type.Equals("other"))
                    {
                        section = objSchemaElem.CreatePostSection("post-section", "1", objGlobal.PBPDocument);
                    }
                    else
                    {
                        section = objSchemaElem.CreateSection(type, "1", objGlobal.PBPDocument);
                    }
                }

                else
                {
                    section = objSchemaElem.CreateSection(type, "1", objGlobal.PBPDocument);
                }
                #region |To skip split step in Level insertion|
                //if (currentNode == null || currentNode.Name == "ln")
                //{
                //    TreeNode tn = treeView1.SelectedNode;
                //    if (tn.Name.Contains("line"))
                //    {
                //        XmlNode lnNode = (XmlNode)tn.Tag;
                //        XmlNode lnParentNode = lnNode.SelectSingleNode("ancestor::upara|ancestor::spara|ancestor::npara");

                //        if (lnParentNode == null)
                //            return;
                //        XmlNode newNodeToInsert = lnParentNode.Clone();
                //        XmlNodeList newChildNodes = lnNode.SelectNodes(". | following-sibling::*");
                //        if (newChildNodes.Count > 0)
                //        {
                //            if (newNodeToInsert.Attributes["text-indent"] != null)
                //            {
                //                newNodeToInsert.Attributes["text-indent"].Value = newChildNodes[0].Attributes["coord"].Value.ToString().Split(new char[] { ':' })[0];
                //            }
                //        }
                //        TreeNode ParentNode = (TreeNode)tn.Parent.Clone();
                //        ParentNode.Nodes.Clear();
                //        int currIndex = treeView1.SelectedNode.Index;
                //        for (int j = 0; j < treeView1.SelectedNode.Parent.Nodes.Count; j++)
                //        {
                //            if (j < currIndex)
                //            {
                //                ParentNode.Nodes.Add(treeView1.SelectedNode.Parent.Nodes[j].Clone() as TreeNode);
                //            }
                //            else
                //            {
                //                break;
                //            }
                //        }
                //        treeView1.Nodes[0].Nodes.Insert(tn.Parent.Index + 1, ParentNode);
                //        treeView1.Nodes.Remove(tn.Parent);
                //        if (lnParentNode.Name == "spara")
                //        {
                //            XmlNode sparaChild = newNodeToInsert.FirstChild;
                //            sparaChild.RemoveAll();

                //            for (int i = 0; i < newChildNodes.Count; i++)
                //            {
                //                sparaChild.AppendChild(newChildNodes[i]);
                //            }
                //        }
                //        else
                //        {
                //            newNodeToInsert.RemoveAll();
                //            ///Select the current and the following sibling nodes
                //            for (int i = 0; i < newChildNodes.Count; i++)
                //            {
                //                newNodeToInsert.AppendChild(newChildNodes[i]);
                //            }
                //        }
                //        ///Copying attributes
                //        foreach (XmlAttribute attr in lnParentNode.Attributes)
                //        {
                //            ((XmlElement)newNodeToInsert).SetAttribute(attr.Name, attr.Value);
                //        }
                //        lnParentNode.ParentNode.InsertAfter(newNodeToInsert, lnParentNode);
                //        TreeNode newTreeNode = new TreeNode(lnParentNode.Name);
                //        XmlNodeList destChildNodes = newNodeToInsert.ChildNodes;
                //        if (newNodeToInsert.Name == "spara")
                //        {
                //            destChildNodes = newNodeToInsert.FirstChild.ChildNodes;
                //        }
                //        foreach (XmlNode pChildNode in destChildNodes)
                //        {
                //            string lineNodeText = getTrimmedText(pChildNode.InnerText);
                //            TreeNode tnPageChildNode = new TreeNode(lineNodeText);
                //            tnPageChildNode.Tag = pChildNode;
                //            tnPageChildNode.Name = "line";
                //            newTreeNode.Nodes.Add(tnPageChildNode);
                //        }
                //        newTreeNode.Tag = newNodeToInsert;
                //        treeView1.Nodes[0].Nodes.Insert(ParentNode.Index + 1, newTreeNode);
                //        treeView1.SelectedNode = newTreeNode;
                //        treeView1.ExpandAll();
                //        if (tn.Parent.Text.Contains("cont.."))
                //        {
                //            this.showXMLTree(this.currentPage);
                //        }
                //    }
                //    currentNode = treeView1.SelectedNode.Tag as XmlNode;
                //    PreOrPostSection = CheckSectionType(currentNode).Name;
                //}
                #endregion

                if (currentNode != null && currentNode.Name == "upara")
                {
                    XmlElement upara = objGlobal.PBPDocument.CreateElement("upara");
                    upara.SetAttribute("id", "0");
                    upara.SetAttribute("pnum", "0");
                    XmlElement ln = objGlobal.PBPDocument.CreateElement("ln");
                    ln.SetAttribute("coord", "0:0:0:0");
                    ln.SetAttribute("page", currentNode.Attributes["pnum"].Value);
                    ln.SetAttribute("height", "0");
                    ln.SetAttribute("left", "0");
                    ln.SetAttribute("top", "0");
                    ln.SetAttribute("font", "Arial");
                    ln.SetAttribute("fontsize", "12");
                    ln.SetAttribute("error", "0");
                    ln.SetAttribute("ispreviewpassed", "true");
                    ln.SetAttribute("isUserSigned", "1");
                    ln.SetAttribute("isEditted", "true");

                    upara.AppendChild(ln);

                    currentNode.ParentNode.InsertAfter(upara, currentNode);

                    XmlElement head = objSchemaElem.CreateHeadNode(currentNode.InnerXml, "", objGlobal.PBPDocument);
                    XmlDocument xmlFile = objGlobal.PBPDocument;
                    string objOuterXml = xmlFile.OuterXml;
                    section.AppendChild(head);
                    XmlElement body = objGlobal.PBPDocument.CreateElement("body");
                    body.SetAttribute("id", "1");
                    XmlNode currentParentNode = currentNode.ParentNode;
                    if (currentNode.ParentNode.Name.Equals("body"))
                    {
                        while (currentNode.NextSibling != null)
                        {
                            body.AppendChild(currentNode.NextSibling);
                        }
                    }

                    string tempInnerXml = section.InnerXml;
                    ////   tempInnerXml = tempInnerXml.Replace("/&gt;&lt;/ln&gt;", "/>");
                    tempInnerXml = tempInnerXml.Replace("&lt;", "<");
                    tempInnerXml = tempInnerXml.Replace("&gt;", ">");
                    section.InnerXml = tempInnerXml;
                    section.AppendChild(body);
                    XmlNodeList lstInnerNodes = section.SelectNodes("descendant::ln");
                    foreach (XmlNode item in lstInnerNodes)
                    {
                        item.InnerText = item.InnerText.Replace("<", "&lt;").Replace(">", "&gt;");
                    }

                    if (GetSection(currentNode).Attributes != null)
                    {
                        string SectionType = GetSection(currentNode).Attributes["type"].Value;

                        if (type.Equals(SectionType))
                        {
                            if (GetSection(currentNode).ParentNode != null)
                            {
                                GetSection(currentNode).ParentNode.InsertAfter(section, GetSection(currentNode));
                                currentNode.ParentNode.RemoveChild(currentNode);
                                objGlobal.SaveXml();
                            }
                        }

                        else
                        {
                            XmlNode existingNode = GetSection(currentNode);

                            if (existingNode != null)
                            {
                                if (existingNode.Attributes["type"].Value.Equals(type))
                                {
                                    existingNode.ParentNode.InsertAfter(section, existingNode);
                                    currentNode.ParentNode.RemoveChild(currentNode);
                                    objGlobal.SaveXml();
                                }
                                else
                                {
                                    if (!CheckLevelSequence(existingNode.Attributes["type"].Value, type))
                                    {
                                        bool parentSameLevel = false;
                                        while (existingNode.ParentNode.Attributes["type"].Value.StartsWith("level"))
                                        {
                                            if (parentSameLevel == true)
                                            {
                                                break;
                                            }
                                            parentSameLevel = existingNode.ParentNode.Attributes["type"].Value.Equals(type);
                                            existingNode = existingNode.ParentNode;
                                        }

                                        if (!parentSameLevel)
                                        {
                                            existingNode = GetSection(currentNode);
                                            existingNode.ParentNode.InsertAfter(section, existingNode);
                                            currentNode.ParentNode.RemoveChild(currentNode);
                                            objGlobal.SaveXml();
                                        }
                                        else
                                        {
                                            existingNode.ParentNode.InsertAfter(section, existingNode);
                                            currentNode.ParentNode.RemoveChild(currentNode);
                                            objGlobal.SaveXml();
                                        }
                                    }
                                    else
                                    {
                                        currentParentNode.ParentNode.InsertAfter(section, currentParentNode);
                                        currentNode.ParentNode.RemoveChild(currentNode);
                                        objGlobal.SaveXml();
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (!currentParentNode.ParentNode.Name.Equals("section") && (section.Name.Equals("section")))
                        {
                            currentParentNode.ParentNode.InsertAfter(section, currentParentNode);
                            currentNode.ParentNode.RemoveChild(currentNode);
                            objGlobal.SaveXml();
                        }
                        else
                        {
                            currentParentNode.ParentNode.ParentNode.InsertAfter(section, currentParentNode.ParentNode);
                            currentNode.ParentNode.RemoveChild(currentNode);
                            objGlobal.SaveXml();
                        }
                    }
                }
            }
        }

        public bool IsSameFontName(string fontName, string uParaFontName)
        {
            List<string> currentLineWordList = Regex.Split(fontName, "-").Where(x => !string.IsNullOrEmpty(x)).ToList();
            List<string> nextLineWordList = Regex.Split(uParaFontName, "-").Where(x => !string.IsNullOrEmpty(x)).ToList();

            if (currentLineWordList.Count > 0 && nextLineWordList.Count > 0 && currentLineWordList[0].Equals(nextLineWordList[0]))
            {
                return true;
            }

            return false;
        }

        public void MergeNode(XmlDocument xmlDoc, List<XmlNode> uParaList, EndNodeCoord firstPageCoord, EndNodeCoord secondPageCoord, int endPage)
        {
            if (firstPageCoord != null && secondPageCoord != null)
            {
                double normalX = 0;
                double normalIndentX = 0;
                string fontName = "";
                double fontSize = 0;

                double normalXSecondPage = 0;
                double normalIndentXSecondPage = 0;
                string fontNameSecondPage = "";
                double fontSizeSecondPage = 0;

                double marginValue = 4;

                for (int p = secondPageCoord.Page; p <= endPage; p++)
                {
                    if (p % 2 == 0)
                    {
                        normalX = firstPageCoord.PageType.Equals("even") ? firstPageCoord.NormalX : secondPageCoord.NormalX;
                        normalIndentX = firstPageCoord.PageType.Equals("even") ? firstPageCoord.NormalIndentX : secondPageCoord.NormalIndentX;
                        fontSize = firstPageCoord.PageType.Equals("even") ? firstPageCoord.NormalFontSize : secondPageCoord.NormalFontSize;
                        fontName = firstPageCoord.PageType.Equals("even") ? firstPageCoord.NormalFontName : secondPageCoord.NormalFontName;
                    }
                    else
                    {
                        normalX = firstPageCoord.PageType.Equals("odd") ? firstPageCoord.NormalX : secondPageCoord.NormalX;
                        normalIndentX = firstPageCoord.PageType.Equals("odd") ? firstPageCoord.NormalIndentX : secondPageCoord.NormalIndentX;
                        fontSize = firstPageCoord.PageType.Equals("odd") ? firstPageCoord.NormalFontSize : secondPageCoord.NormalFontSize;
                        fontName = firstPageCoord.PageType.Equals("odd") ? firstPageCoord.NormalFontName : secondPageCoord.NormalFontName;
                    }

                    List<XmlNode> pageNodes =
                        uParaList.Where(
                            x =>
                                x.ChildNodes.Cast<XmlNode>()
                                    .Any(
                                        y =>
                                            y.Attributes != null && y.Attributes["page"] != null &&
                                            Convert.ToInt32(y.Attributes["page"].Value).Equals(p)))
                            .ToList();
                    try
                    {
                        XmlNode firstLine = null;

                        if (pageNodes.Count > 0)
                        {
                            firstLine = pageNodes[0].ChildNodes[0];
                        }

                        if (firstLine != null)
                        {
                            if ((Math.Abs(Convert.ToDouble(firstLine.Attributes["left"].Value) - normalIndentX) <= marginValue &&
                                 Convert.ToDouble(firstLine.Attributes["fontsize"].Value).Equals(fontSize)) &&
                                IsSameFontName(firstLine.Attributes["font"].Value, fontName) && normalIndentX != 0)
                            {
                                List<XmlNode> prevPageNodes =
                                    uParaList.Where(
                                        x =>
                                            x.ChildNodes.Cast<XmlNode>()
                                                .Any(
                                                    y =>
                                                        y.Attributes != null && y.Attributes["page"] != null &&
                                                        Convert.ToInt32(y.Attributes["page"].Value)
                                                            .Equals(p - 1))).ToList();

                                if (prevPageNodes.Count > 0)
                                {
                                    XmlNode preLastPara = prevPageNodes[prevPageNodes.Count - 1];

                                    if (preLastPara != null)
                                    {
                                        XmlNode lastLine = preLastPara.ChildNodes[preLastPara.ChildNodes.Count - 1].Name.Equals("break") ?
                                            preLastPara.ChildNodes[preLastPara.ChildNodes.Count - 2] : preLastPara.ChildNodes[preLastPara.ChildNodes.Count - 1];

                                        if (lastLine != null &&
                                            lastLine.Attributes != null &&
                                            lastLine.Attributes["left"] != null &&
                                            lastLine.Attributes["fontsize"] != null &&
                                            lastLine.Attributes["font"] != null)
                                        {
                                            if ((p - 1) % 2 == 0)
                                            {
                                                normalXSecondPage = firstPageCoord.PageType.Equals("even")
                                                    ? firstPageCoord.NormalX
                                                    : secondPageCoord.NormalX;
                                                normalIndentXSecondPage = firstPageCoord.PageType.Equals("even")
                                                    ? firstPageCoord.NormalIndentX
                                                    : secondPageCoord.NormalIndentX;
                                                fontSizeSecondPage = firstPageCoord.PageType.Equals("even")
                                                    ? firstPageCoord.NormalFontSize
                                                    : secondPageCoord.NormalFontSize;
                                                fontNameSecondPage = firstPageCoord.PageType.Equals("even")
                                                    ? firstPageCoord.NormalFontName
                                                    : secondPageCoord.NormalFontName;
                                            }
                                            else
                                            {
                                                normalXSecondPage = firstPageCoord.PageType.Equals("odd")
                                                    ? firstPageCoord.NormalX
                                                    : secondPageCoord.NormalX;
                                                normalIndentXSecondPage = firstPageCoord.PageType.Equals("odd")
                                                    ? firstPageCoord.NormalIndentX
                                                    : secondPageCoord.NormalIndentX;
                                                fontSizeSecondPage = firstPageCoord.PageType.Equals("odd")
                                                    ? firstPageCoord.NormalFontSize
                                                    : secondPageCoord.NormalFontSize;
                                                fontNameSecondPage = firstPageCoord.PageType.Equals("odd")
                                                    ? firstPageCoord.NormalFontName
                                                    : secondPageCoord.NormalFontName;
                                            }

                                            if ((Math.Abs(Convert.ToDouble(lastLine.Attributes["left"].Value) - normalIndentXSecondPage) <= marginValue ||
                                                Math.Abs(Convert.ToDouble(lastLine.Attributes["left"].Value) - normalXSecondPage) <= marginValue) &&
                                             Convert.ToDouble(lastLine.Attributes["fontsize"].Value).Equals(fontSize) &&
                                            IsSameFontName(lastLine.Attributes["font"].Value, fontName) && normalIndentXSecondPage != 0 && normalXSecondPage != 0)
                                            {
                                                XmlNode nextPara = pageNodes[0];

                                                if (nextPara != null && nextPara.ChildNodes.Count > 0 &&
                                                    nextPara.ParentNode != null)
                                                {
                                                    foreach (XmlNode ln in nextPara.SelectNodes("ln"))
                                                    {
                                                        XmlAttribute att = xmlDoc.CreateAttribute("NextPage");
                                                        att.Value = "1";
                                                        ln.Attributes.Append(att);
                                                        preLastPara.AppendChild(ln);
                                                    }
                                                    nextPara.ParentNode.RemoveChild(nextPara);
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception)
                    {

                    }
                }
            }
        }

        #endregion

        #endregion
    }
}
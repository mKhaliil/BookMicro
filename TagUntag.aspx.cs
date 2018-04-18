using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;

using System.Text;
using System.Collections;
using System.Xml;
using System.IO;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Threading;
using System.Diagnostics;
using System.Text.RegularExpressions;
using BookMicroBeta;
using HtmlAgilityPack;
using Microsoft.Ajax.Utilities;
using Outsourcing_System.readableEnglishService;
using iTextSharp.text.pdf;
using Outsourcing_System.CommonClasses;
using iTextSharp.text;
using System.Globalization;
//using Photoshop;

namespace Outsourcing_System
{
    public partial class TagUntag : System.Web.UI.Page
    {
        #region |Fields and Properties|

        private GlobalVar objGlobal = new GlobalVar();
        public MyDBClass objMyDBClass = new MyDBClass();
        private string XSLFilePath;
        private ArrayList FList;
        private ArrayList UFontList;
        private ArrayList AText;
        private ArrayList UTexts;
        private int FontsRemaing;

        #endregion

        public string WordCoordinates
        {
            get { return Convert.ToString(ViewState["WordCoordinates"]); }
            set { ViewState["WordCoordinates"] = value; }
        }

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


        private ConversionClass objConversionClass = new ConversionClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            //if (!IsPostBack)
            //{
            //    string bookId = Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0];

            //    string pdfPath = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + Convert.ToString(Request.QueryString["bid"]) +
            //                     "/TaggingUntagged/" + Convert.ToString(Request.QueryString["bid"]) + ".pdf";

            //    if (!string.IsNullOrEmpty(pdfPath) && File.Exists(pdfPath))
            //        Session["PdfForSVGPath"] = pdfPath;

            //    string boxDirPath = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/";

            //    if (!Directory.Exists(boxDirPath))
            //    {
            //        if (CreateDirectory(boxDirPath))
            //        {
            //            string pdfSvgXml = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/" + Convert.ToString(Request.QueryString["bid"]) + "_Svg.xml";

            //            while (!File.Exists(pdfSvgXml))
            //            {

            //            }

            //            Thread.Sleep(10000);

            //            string hdnValue = hfSvgContent.Value;
            //        }
            //    }
            //}


            //Session["PdfForSVGPath"] = @"F:\1.pdf";

            //var rr = hfScreenResolution.Value;

            //GetScreenResolution();

            //var rr = hfScreenResolution.Value;

            Page.MaintainScrollPositionOnPostBack = true;
            //((AdminMaster)this.Page.Master).SetLogIn = true;
            //((AdminMaster)this.Page.Master).SetMenuLocation = "0px";

            //((AdminMaster)this.Page.Master).ShowLogOutButton();

            this.Title = "Outsourcing System :: Tagging UnTagged Elements";
            this.lblMessage.Text = "";

            if (string.IsNullOrEmpty(Convert.ToString(Session["objUser"])))
                Response.Redirect("BookMicro.aspx");

            // FetchData();
            populateSessions();
            if (!Page.IsPostBack)
            {
                pupulateFontsFields();
                populateEditedFonts();
            }

            if (!string.IsNullOrEmpty(Convert.ToString(Session["MainBook"])))
            {
                //bool isParaSelected = objMyDBClass.GetParaIndentationStatus(Convert.ToString(Session["MainBook"]));

                //if (!isParaSelected)
                //{
                //    Response.Redirect("ParaSelection.aspx", true);
                //}
                //else
                //{
                if (!string.IsNullOrEmpty(lblPageno.Text.Trim()))
                    ShowPDF(lblPageno.Text);

                if (lblRemainingFonts.Text.Equals("0"))
                {
                    //populateDivSparas();
                }
                //}
            }
        }

        public void GetParaSelection()
        {

        }

        public void GetScreenResolution()
        {
            Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "GetResolution()", true);
        }

        protected void dlistMappedFonts_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            LinkButton lnkFontName = (LinkButton)e.Item.FindControl("lnkFontName");
            lnkFontName.Text = e.Item.DataItem.ToString().Replace(",", " ");
            lnkFontName.CommandArgument = e.Item.DataItem.ToString();
            lnkFontName.CommandName = "UpdateFont";
        }

        #region void LoadPDF()

        public void LoadPDF(string pageno)
        {
            ShowPDF(pageno);

        }

        #endregion

        #region void ShowPDF(string untaggedIndex)

        public void ShowPDF(string pageno)
        {
            bool isHighlight = true;
            try
            {
                #region |Undo|

                //string elementType = "";
                //List<XmlNode> unTaggedElementList = (Session["Nodes"] as List<XmlNode>);
                //this.lblRemainingFonts.Text = unTaggedElementList.Count.ToString();
                //if (int.Parse(Session["untaggedIndex"].ToString()) == unTaggedElementList.Count)
                //{
                //    this.lblMessage.Text = "Ajustment of Untagged Element completed successfully";

                //    this.btnFinish.Visible = true;
                //}
                //else
                //{
                //    XmlNode unTaggedElement = (unTaggedElementList[int.Parse(untaggedIndex)] as XmlNode);
                //    txtFontType.Text = unTaggedElement.SelectSingleNode(".//ln").Attributes["font"].Value + " " + unTaggedElement.SelectSingleNode(".//ln").Attributes["fontsize"].Value;
                //    lblPageno.Text = unTaggedElement.SelectSingleNode(".//ln").Attributes["page"].Value;
                //    if (unTaggedElement.Attributes["type"].Value == "footnote" && unTaggedElement.Attributes["ftype"].Value == "link")
                //    {
                //        this.txtActualText.Text = unTaggedElement.InnerText.Replace("<", "&lt;").Replace(">", "&gt;");
                //        pdfPage = unTaggedElement.ParentNode.Attributes["page"].Value;
                //        elementType = "Foot Note of type Link";
                //    }
                //    else
                //    {
                //        if (unTaggedElement.Attributes["type"].Value == "footnote")
                //        {
                //            elementType = "Foot Note of type Text";
                //        }
                //        else
                //        {
                //            elementType = "Element of type " + unTaggedElement.Attributes["type"].Value;
                //        }
                //        XmlNode singleLine = unTaggedElement.SelectSingleNode(".//ln");
                //        this.txtActualText.Text = singleLine.InnerXml.Replace("<", "&lt;").Replace(">", "&gt;"); ;
                //        pdfPage = singleLine.Attributes["page"].Value;
                //    }
                //    //this.elemType.Text = elementType;
                //    Session["untaggedIndex"] = int.Parse(Session["untaggedIndex"].ToString()) + 1;

                #endregion

                string newPDFName = objMyDBClass.MainDirPhyPath + "/" +
                                    Request.QueryString["bid"].ToString().Split(new char[] { '-' })[0] + "/" +
                                    Request.QueryString["bid"].ToString() + "/TaggingUntagged/Page" + pageno + ".pdf";

                try
                {
                    if (File.Exists(newPDFName))
                        File.Delete(newPDFName);
                }
                catch (Exception)
                {
                    isHighlight = false;
                }

                if (!File.Exists(newPDFName))
                {
                    objConversionClass.ExtractPages(objGlobal.PDFPath, newPDFName, int.Parse(pageno), int.Parse(pageno));
                }

                ////aamir temporary change
                //PDFViewerTarget.FilePath = Session["MainDirectory"].ToString() + "/" +
                //                           Request.QueryString["bid"].ToString().Split(new char[] { '-' })[0] + "/" +
                //                           Request.QueryString["bid"].ToString() + "/TaggingUntagged/Page" + pageno +
                //                           ".pdf#toolbar=0";

                if (isHighlight && File.Exists(newPDFName) && !string.IsNullOrEmpty(WordCoordinates))
                {
                    string coords = GetWordCoordinates(WordCoordinates, newPDFName, pageno);

                    if (!string.IsNullOrEmpty(coords))
                    {
                        List<string> coordList = new List<string>();
                        coordList.Add(coords);
                        HighLightSelectedParas(newPDFName, newPDFName.Replace(".pdf", "_highlighted.pdf"), BaseColor.ORANGE, coordList);
                    }
                }
                PDFViewerTarget.FilePath = "DisplayPdf.ashx?bid=" + Convert.ToString(Request.QueryString["bid"]) + "&page=" + pageno;
            }

            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
            }
        }

        private string GetWordCoordinates(string coordValues, string pdfPath, string page)
        {
            if (string.IsNullOrEmpty(coordValues)) return null;

            string coordinates = "";

            try
            {
                List<string> coordValList = coordValues.Split(',').ToList();

                if (coordValList.Count > 0)
                {
                    double left = 0;
                    double bottom = 0;

                    double llx = Convert.ToDouble(coordValList[0]);
                    double lly = Convert.ToDouble(coordValList[1]);
                    double urx = Convert.ToDouble(coordValList[2]);
                    double ury = Convert.ToDouble(coordValList[3]);

                    double width = Convert.ToDouble(urx) - Convert.ToDouble(llx);
                    double height = Convert.ToDouble(ury) - Convert.ToDouble(lly);
                    string croppedMargins = getCroppedMargins(pdfPath);

                    if (!string.IsNullOrEmpty(croppedMargins))
                    {
                        List<string> tempValues = croppedMargins.Split(' ').ToList();

                        if (tempValues.Count > 0)
                        {
                            if (tempValues.Count > 0)
                            {
                                left = Math.Abs(Convert.ToDouble(tempValues[3]));
                                bottom = Math.Abs(Convert.ToDouble(tempValues[1]));
                            }
                        }
                    }

                    coordinates = (Convert.ToDouble(llx) + left) + " " + (Convert.ToDouble(lly) + bottom) + " " + height + " " + width + " " + page;
                }
                return coordinates;
            }
            catch (Exception ex)
            {
                return null;
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

        #endregion

        protected void btnAssign_Click1(object sender, EventArgs e)
        {
            try
            {
                List<string> listofFonts = new List<string>();
                if (File.Exists(getFontsFilePath()))
                {
                    //StreamReader sr = new StreamReader(getFontsFilePath());
                    //string fileData = sr.ReadToEnd().Replace("\r\n", "$");
                    //string[] fonts = fileData.Remove(fileData.Length - 1, 1).Split('$');
                    //sr.Dispose();

                    string[] fonts = File.ReadAllLines(getFontsFilePath(), Encoding.UTF8);

                    for (int i = 0; i < fonts.Length; i++)
                    {

                        if (fonts[i] != "")
                        {
                            string[] fontDetail = fonts[i].Split('@');
                            if (txtFontType.Text.Equals(fontDetail[1] + " " + fontDetail[2]))
                            {
                                fonts[i] = fontDetail[0] + "@" + fontDetail[1] + "@" + fontDetail[2] + "@" + fontDetail[3] +
                                           "@" + fontDetail[4] + "@assigned@" + ddlPreSection.SelectedItem.Text + "@" +
                                           ddlBody.SelectedItem.Text + "@" + ddlPostSection.SelectedItem.Text;

                                if (fontDetail[5].Contains("CID"))
                                {
                                    fonts[i] = fonts[i] + "@NotEmbeded";
                                }
                                else
                                {
                                    fonts[i] = fonts[i] + "@Embeded";
                                }
                                if (chkCaps.Checked)
                                {
                                    fonts[i] = fonts[i] + "@CAPS$";
                                }
                                fonts[i] = fonts[i].Replace(",", "").Replace("|", ",");
                                listofFonts.Add(fonts[i]);
                            }
                            else
                            {
                                listofFonts.Add(fonts[i]);
                            }
                        }
                    }
                    if (listofFonts.Count > 0)
                    {
                        using (StreamWriter sw = new StreamWriter(getFontsFilePath()))
                        {
                            foreach (string item in listofFonts)
                            {
                                sw.WriteLine(item);
                            }
                            sw.Flush();
                            sw.Close();
                        }
                    }
                    pupulateFontsFields();
                    populateEditedFonts();
                    ShowPDF(lblPageno.Text);
                    chkCaps.Checked = false;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        protected void ddlParaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            populateDivSparas(ddlParaType.SelectedValue);
            if (ddlParaType.SelectedValue.Equals("spara"))
            {
                sparaOptions.Visible = true;
            }
            else
            {
                sparaOptions.Visible = false;
            }
        }

        //protected void ddlPreSection_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlPreSection.SelectedItem.Text.Equals("upara") || ddlPreSection.SelectedItem.Text.Equals("italic") ||
        //        ddlPreSection.SelectedItem.Text.Equals("bold") || ddlPreSection.SelectedItem.Text.Equals("end-node") ||
        //        ddlPreSection.SelectedItem.Text.Equals("bold-italic") ||
        //        ddlPreSection.SelectedItem.Text.Equals("section-break"))
        //    {
        //        ddlBody.SelectedIndex = ddlBody.Items.IndexOf(ddlPreSection.SelectedItem);
        //        ddlPostSection.SelectedIndex = ddlPostSection.Items.IndexOf(ddlPreSection.SelectedItem);
        //    }
        //}

        protected void ddlPreSection_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlPreSection.SelectedItem.Text.Equals("upara") || ddlPreSection.SelectedItem.Text.Equals("italic") ||
                ddlPreSection.SelectedItem.Text.Equals("bold") || ddlPreSection.SelectedItem.Text.Equals("end-node") ||
                ddlPreSection.SelectedItem.Text.Equals("level1") || ddlPreSection.SelectedItem.Text.Equals("level2") ||
                ddlPreSection.SelectedItem.Text.Equals("level3") || ddlPreSection.SelectedItem.Text.Equals("level4") ||
                ddlPreSection.SelectedItem.Text.Equals("bold-italic") || ddlPreSection.SelectedItem.Text.Equals("chap-prefix") ||
                ddlPreSection.SelectedItem.Text.Equals("section-break"))
            {
                ddlBody.SelectedIndex = ddlBody.Items.IndexOf(ddlPreSection.SelectedItem);
                ddlPostSection.SelectedIndex = ddlPostSection.Items.IndexOf(ddlPreSection.SelectedItem);
            }

            else if (ddlPreSection.SelectedItem.Text.Equals("pre-section"))
            {
                ddlBody.SelectedIndex = ddlBody.Items.IndexOf(ddlBody.Items[2]);
                ddlPostSection.SelectedIndex = ddlPostSection.Items.IndexOf(ddlPostSection.Items[0]);
            }
        }

        #region Tagging Un Tagging

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

        public void italiBoldHandler(XmlNode unTaggedElement, string type)
        {
            try
            {
                XmlNodeList nodeList = unTaggedElement.ChildNodes;
                XmlElement tempUparaNode = unTaggedElement.OwnerDocument.CreateElement("upara");
                tempUparaNode.SetAttribute("id", "0");
                tempUparaNode.SetAttribute("pnum", "0");
            Pick:
                foreach (XmlNode lnNode in nodeList)
                {
                    XmlElement tempEmphasis = unTaggedElement.OwnerDocument.CreateElement("emphasis");
                    tempEmphasis.SetAttribute("type", type);
                    tempEmphasis.InnerText = lnNode.InnerText;
                    lnNode.InnerText = "";
                    lnNode.AppendChild(tempEmphasis);
                    tempUparaNode.AppendChild(lnNode);
                    goto Pick;
                }



                unTaggedElement.ParentNode.InsertAfter(tempUparaNode, unTaggedElement);
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

        #endregion

        public void MainOperation(XmlNode node, string operation)
        {
            switch (operation)
            {
                case "book":
                    {
                        sectionHandler(node, operation);
                        break;
                    }
                case "chapter":
                    {
                        sectionHandler(node, operation);
                        break;
                    }
                case "chap-prefix":
                    {
                        sectionHandler(node, operation);
                        break;
                    }

                case "pre-section":
                    {
                        presectionHandler(node);
                        break;
                    }
                case "post-section":
                    {
                        postsectionHandler(node);
                        break;
                    }
                case "upara":
                    {
                        uparaHandler(node, "");
                        break;
                    }
                case "spara":
                    {

                        break;
                    }

            }
        }

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

            if (left == 6 && right == 4) //True for Level1 after part or chapter
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

            //if (sectionType != "book" && (unTaggedElement.NextSibling != null && (unTaggedElement.NextSibling.Name.Equals("untagged"))))
            //{
            //    elemBody.AppendChild(unTaggedElement.NextSibling);
            //}

            if (sectionType == "book" && (unTaggedElement.NextSibling != null))
            {
                while (unTaggedElement.NextSibling != null)
                {
                    XmlNode currentlinNode = unTaggedElement.FirstChild;
                    XmlNode nextLineNode = unTaggedElement.NextSibling.FirstChild;
                    if (currentlinNode.Attributes["fontsize"] != null && nextLineNode.Attributes["fontsize"] != null &&
                        (!currentlinNode.Attributes["fontsize"].Value.Equals(nextLineNode.Attributes["fontsize"].Value)))
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
                if (unTaggedElement.NextSibling != null)
                {
                    XmlNode nextlineNode = unTaggedElement.NextSibling.SelectSingleNode(".//ln");
                    XmlNode currentLineNode = unTaggedElement.SelectSingleNode(".//ln");
                    while (unTaggedElement.NextSibling != null && (nextlineNode != null && currentLineNode != null))
                    {
                        if (unTaggedElement.NextSibling.Name.Equals("untagged") &&
                            (currentLineNode.Attributes["fontsize"].Value.Equals(
                                nextlineNode.Attributes["fontsize"].Value)))
                        {
                            break;
                        }
                        elemBody.AppendChild(unTaggedElement.NextSibling);
                        if (unTaggedElement.NextSibling != null)
                        {
                            nextlineNode = unTaggedElement.NextSibling.SelectSingleNode(".//ln");
                        }
                    }
                }
            }
            NodeSection.AppendChild(elemBody);
            return NodeSection;
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

            if (retNode.NextSibling != null && retNode.NextSibling.Name == "section" ||
                retNode.NextSibling.Name == "pre-section" || retNode.NextSibling.Name == "post-section")
            {
                retNode = retNode.NextSibling;
            }
            else if (retNode.NextSibling != null && retNode.NextSibling.Name == "section" ||
                     retNode.NextSibling.Name == "pre-section" || retNode.NextSibling.Name == "post-section")
            {
                retNode = retNode.NextSibling;
            }
            else if (retNode.NextSibling != null && retNode.NextSibling.Name == "section" ||
                     retNode.NextSibling.Name == "pre-section" || retNode.NextSibling.Name == "post-section")
            {
                retNode = retNode.NextSibling;
            }
            else if (retNode.NextSibling != null && retNode.NextSibling.Name == "section" ||
                     retNode.NextSibling.Name == "pre-section" || retNode.NextSibling.Name == "post-section")
            {
                retNode = retNode.NextSibling;
            }

            return retNode;
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            //SaveSvgAsXml();
            btnAssign.Enabled = true;
            // btnFinish.Enabled = true;
        }

        public void presectionHandler(XmlNode unTaggedElement)
        {
            XmlNode NodeSection = CreateSectionNode(unTaggedElement, "pre-section", "other", unTaggedElement.InnerXml,
                "");
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
        }

        public void sectionBreakHandler(XmlNode unTaggedElement)
        {
            string txt = "<section-break value=\"" + unTaggedElement.InnerText + "\">" + unTaggedElement.InnerText +
                         "</section-break>";

            XmlNode lnNode = unTaggedElement.SelectSingleNode(".//ln");
            lnNode.InnerText = txt;
            lnNode.InnerText = lnNode.InnerText.Replace("&lt;", "<");
            lnNode.InnerText = lnNode.InnerText.Replace("&gt;", ">");
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            XmlElement uparaNode = objGlobal.PBPDocument.CreateElement("upara");
            foreach (XmlAttribute att in unTaggedElement.Attributes)
            {
                uparaNode.SetAttribute(att.Name, att.Value);
            }
            uparaNode.AppendChild(lnNode);
            unTaggedElement.ParentNode.InsertBefore(uparaNode, unTaggedElement);
            unTaggedElement.ParentNode.RemoveChild(unTaggedElement);

            objGlobal.SaveXml();

        }

        public void postsectionHandler(XmlNode unTaggedElement)
        {
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
        }

        public void sectionHandler(XmlNode unTaggedElement, string sectionType)
        {
            try
            {
                this.lblMessage.Text = "";
                XmlNode tempElemToDel = null;
                string prefix = "", title = "";
                bool isBoth = false;

                switch (sectionType)
                {
                    case "book-prefix":
                        {
                            if (unTaggedElement.NextSibling != null && unTaggedElement.NextSibling.Name == "untagged" &&
                                (unTaggedElement.NextSibling.Attributes["type"].Value.Contains("level") ||
                                 unTaggedElement.NextSibling.Attributes["type"].Value.Contains("part") ||
                                 unTaggedElement.NextSibling.Attributes["type"].Value.Contains("chapter") ||
                                 unTaggedElement.NextSibling.Attributes["type"].Value.Contains("section")))
                            {
                                prefix = unTaggedElement.InnerXml;
                                title = unTaggedElement.NextSibling.InnerXml;
                                tempElemToDel = unTaggedElement.NextSibling;
                                isBoth = true;
                            }
                            else if (unTaggedElement.PreviousSibling == null && unTaggedElement.ParentNode.Name == "body" &&
                                     (unTaggedElement.ParentNode.ParentNode.Name == "section" ||
                                      unTaggedElement.ParentNode.ParentNode.Name == "pre-section" ||
                                      unTaggedElement.ParentNode.ParentNode.Name == "post-section"))
                            {
                                unTaggedElement.ParentNode.ParentNode.SelectSingleNode("//prefix").InnerXml =
                                    unTaggedElement.InnerXml;
                            }
                            else if (unTaggedElement.NextSibling == null && unTaggedElement.ParentNode.Name == "body")
                            {
                                XmlNode prefixParentNode = FindNextSibling(unTaggedElement.ParentNode);
                                prefixParentNode.SelectSingleNode("//prefix").InnerXml = unTaggedElement.InnerXml;
                                unTaggedElement.ParentNode.RemoveChild(unTaggedElement);
                            }
                            break;
                        }
                    case "chap-prefix":
                        {
                            if (unTaggedElement.NextSibling != null && unTaggedElement.NextSibling.Name == "untagged" &&
                                (unTaggedElement.NextSibling.Attributes["type"].Value.Contains("level") ||
                                 unTaggedElement.NextSibling.Attributes["type"].Value.Contains("part") ||
                                 unTaggedElement.NextSibling.Attributes["type"].Value.Contains("chapter") ||
                                 unTaggedElement.NextSibling.Attributes["type"].Value.Contains("section")))
                            {
                                prefix = unTaggedElement.InnerXml;
                                title = unTaggedElement.NextSibling.InnerXml;
                                tempElemToDel = unTaggedElement.NextSibling;
                                isBoth = true;
                            }
                            else if (unTaggedElement.PreviousSibling == null && unTaggedElement.ParentNode.Name == "body" &&
                                     (unTaggedElement.ParentNode.ParentNode.Name == "section" ||
                                      unTaggedElement.ParentNode.ParentNode.Name == "pre-section" ||
                                      unTaggedElement.ParentNode.ParentNode.Name == "post-section"))
                            {
                                unTaggedElement.ParentNode.ParentNode.SelectSingleNode("//prefix").InnerXml =
                                    unTaggedElement.InnerXml;
                            }
                            else if (unTaggedElement.NextSibling == null && unTaggedElement.ParentNode.Name == "body")
                            {
                                XmlNode prefixParentNode = FindNextSibling(unTaggedElement.ParentNode);
                                prefixParentNode.SelectSingleNode("//prefix").InnerXml = unTaggedElement.InnerXml;
                                unTaggedElement.ParentNode.RemoveChild(unTaggedElement);
                            }
                            break;
                        }
                    case "chapter":
                        {
                            if (unTaggedElement.PreviousSibling == null && unTaggedElement.ParentNode.Name == "body" &&
                                unTaggedElement.ParentNode.ParentNode.SelectSingleNode("//head/section-title") != null &&
                                (unTaggedElement.ParentNode.ParentNode.SelectSingleNode("//head/section-title").InnerXml == "" &&
                                 (unTaggedElement.ParentNode.ParentNode.Name == "section" ||
                                  unTaggedElement.ParentNode.ParentNode.Name == "pre-section" ||
                                  unTaggedElement.ParentNode.ParentNode.Name == "post-section")))
                            {
                                unTaggedElement.ParentNode.ParentNode.SelectSingleNode("//section-title").InnerXml = unTaggedElement.InnerXml;
                            }
                            else
                            {


                                title = unTaggedElement.InnerXml;
                            }
                            break;
                        }
                    case "book":
                        {
                            if (unTaggedElement.PreviousSibling == null && unTaggedElement.ParentNode.Name == "body" &&
                                unTaggedElement.ParentNode.ParentNode.SelectSingleNode("//head/section-title").InnerXml ==
                                "" &&
                                (unTaggedElement.ParentNode.ParentNode.Name == "section" ||
                                 unTaggedElement.ParentNode.ParentNode.Name == "pre-section" ||
                                 unTaggedElement.ParentNode.ParentNode.Name == "post-section"))
                            {
                                unTaggedElement.ParentNode.ParentNode.SelectSingleNode("//section-title").InnerXml =
                                    unTaggedElement.InnerXml;
                            }
                            else
                            {

                                title = unTaggedElement.InnerXml;
                            }
                            break;
                        }
                }

                if (!sectionType.Contains("prefix"))
                {
                    XmlNode NodeSection = CreateSectionNode(unTaggedElement, "section", sectionType, title, prefix);

                    XmlNode parentNode = unTaggedElement.ParentNode.ParentNode == null
                        ? unTaggedElement.ParentNode
                        : unTaggedElement.ParentNode.ParentNode;
                    if (parentNode.Name == "pre-section" || parentNode.Name == "post-section")
                    {
                        parentNode.InsertAfter(NodeSection, unTaggedElement.ParentNode);
                    }
                    else if (parentNode.Attributes["type"] != null && parentNode.Attributes["type"].Value == sectionType)
                    {
                        parentNode.ParentNode.InsertAfter(NodeSection, parentNode);
                    }
                    else
                    {
                        if (parentNode.Name == "pbp-body" && sectionType.Contains("chap"))
                        {
                            if (unTaggedElement.ParentNode.NextSibling != null &&
                                (!unTaggedElement.ParentNode.NextSibling.Name.Equals("section")))
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
                            parentNode = NodeComparison(parentNode, sectionType);
                            XmlNode insertAfter = ReturnLevel(unTaggedElement.ParentNode.Name, sectionType,
                                unTaggedElement.ParentNode);
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
                    if (unTaggedElement.ParentNode != null)
                    {
                        unTaggedElement.ParentNode.RemoveChild(unTaggedElement);
                    }
                }
                SaveXML();
            }
            catch (Exception ex)
            {
                this.lblMessage.Text = "You may selected wrong level<br />" + ex.Message;
            }
        }

        public void SaveXML()
        {
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
        }

        #region void FetchData()

        private void FetchData()
        {
            string OutputFile = objMyDBClass.MainDirPhyPath + @"\temp1.xml";
            XslCompiledTransform xsl = new XslCompiledTransform();
            //HardCoded by Khalil...
            string XmlFilePath = Session["XMLPath"].ToString().Replace(".xml", ".tetml");
            xsl.Load(@"C:\XSL\nsxsl.xsl");
            xsl.Transform(XmlFilePath, OutputFile);

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(OutputFile);
            XmlNode Fonts = xmlDoc.DocumentElement.SelectSingleNode(".//Pages/Resources/Fonts");
            XmlNodeList FontList = Fonts.ChildNodes;

            foreach (XmlNode Font in FontList)
            {
                string Xpath = "//Glyph[@font=\'" + Font.Attributes["id"].Value + "\']";
                XmlNodeList sizeList = xmlDoc.DocumentElement.SelectNodes(Xpath);
                ArrayList DistinctFonts = new ArrayList();
                foreach (XmlNode size in sizeList)
                {
                    if (DistinctFonts.Contains(size.Attributes["size"].Value))
                        continue;
                    else
                    {
                        string tempSize = size.Attributes["size"].Value.ToString();
                        DistinctFonts.Add(tempSize);
                        XmlNodeList sameFontList =
                            size.ParentNode.SelectNodes("Glyph[@font=\"" + size.Attributes["font"].Value +
                                                        "\" and @size=\"" + tempSize + "\"]");
                        string strtem = "";
                        foreach (XmlNode nod in sameFontList)
                        {
                            strtem += nod.InnerXml;
                        }
                        strtem += " Page No:";
                        //string strtem = size.ParentNode.ParentNode.FirstChild.InnerText + " Page No:";
                        try
                        {
                            if (size.ParentNode.ParentNode.ParentNode.Name == "Para")
                            {
                                strtem += size.SelectSingleNode("ancestor::Page").Attributes["number"].Value;
                                // strtem += size.ParentNode.ParentNode.ParentNode.ParentNode.ParentNode.Attributes["number"].Value;
                            }
                            else
                            {
                                strtem += size.SelectSingleNode("ancestor::Page").Attributes["number"].Value;
                                //strtem += size.ParentNode.ParentNode.ParentNode.ParentNode.Attributes["number"].Value;
                            }
                        }
                        catch
                        {
                            strtem += "{Page No Error}";
                        }
                        AText.Add(strtem);
                    }
                }

                foreach (object o in DistinctFonts)
                {
                    string FontSize = o.ToString().Trim();
                    string fontName = Font.Attributes["name"].Value.Trim();
                    fontName = fontName.Replace(",", "-");
                    string Value = Font.Attributes["id"].Value.Trim() + "," + fontName + "," + FontSize;
                    FList.Add(Value);
                }
            }
            int i = 0;
            foreach (object o in FList)
            {
                string val = o.ToString();
                string[] vals = val.Split(',');
                string UniqueFont = vals[1].ToString() + "," + vals[2].ToString();
                if (!UFontList.Contains(UniqueFont))
                {
                    UFontList.Add(UniqueFont);
                    UTexts.Add(AText[i]);
                }
                i++;
            }

            FontsRemaing = UFontList.Count;
            lblRemainingFonts.Text = FontsRemaing.ToString();
        }

        #endregion

        protected void btnFinish_Click(object sender, EventArgs e)
        {
            //Session["SvgXmlCreated"] = "";
            string screenHeight = "";
            string screenWidth = "";

            string tempValues = hfScreenResolution.Value;
            if ((tempValues != "") && (tempValues.Length > 0))
            {
                screenHeight = tempValues.Split(',')[0];
                screenWidth = tempValues.Split(',')[1];
            }

            if (File.Exists(getFontsFilePath()))
            {
                string presectionRang = txtPreSectionStart.Text + ">" + txtPreSectionEnd.Text;
                string bodysectionRange = txtBodyStart.Text + ">" + txtBodyEnd.Text;
                string postsectionRange = txtPostSectionStart.Text + ">" + txtPostSectionEnd.Text;

                if (File.Exists(getFontsFilePath()))
                {
                    //StreamReader sr = new StreamReader(getFontsFilePath());
                    //string fileData = sr.ReadToEnd().Replace("\r\n", "$");
                    //string[] fonts = fileData.Remove(fileData.Length - 1, 1).Split('$');
                    //sr.Dispose();

                    string[] fonts = File.ReadAllLines(getFontsFilePath(), Encoding.UTF8);

                    List<FontDetails> fDetails = new List<FontDetails>();

                    for (int i = 0; i < fonts.Length; i++)
                    {
                        FontDetails objFont = new FontDetails();
                        string[] arrAssignedFonts = fonts[i].Split('@');
                        objFont.Font = arrAssignedFonts[0];
                        objFont.Fontname = arrAssignedFonts[1];
                        objFont.Fontsize = arrAssignedFonts[2];
                        objFont.Text = arrAssignedFonts[3];
                        objFont.Presection = arrAssignedFonts[6];
                        objFont.Body = arrAssignedFonts[7];
                        objFont.Postsection = arrAssignedFonts[8];
                        objFont.FontType = arrAssignedFonts[9];
                        if (arrAssignedFonts.Length > 10)
                        {
                            objFont.Iscaps = true;
                        }
                        else
                        {
                            objFont.Iscaps = false;
                        }

                        fDetails.Add(objFont);
                    }
                    List<FinalFonts> finalFonts = new List<FinalFonts>();
                    foreach (FontDetails font in fDetails)
                    {
                        for (int i = 1; i < 4; i++)
                        {
                            FinalFonts objFinalFont = new FinalFonts();
                            switch (i.ToString())
                            {
                                case "1":
                                    objFinalFont.Font = font.Font + font.Fontsize;
                                    objFinalFont.Fontname = font.Fontname;
                                    objFinalFont.Fontsize = font.Fontsize;
                                    objFinalFont.Type = font.Presection;
                                    objFinalFont.Pagerange = presectionRang;
                                    objFinalFont.Sectiontype = "pre-section";
                                    objFinalFont.FontType = font.FontType;
                                    objFinalFont.Iscaps = font.Iscaps;
                                    break;

                                case "2":
                                    objFinalFont.Font = font.Font + font.Fontsize;
                                    objFinalFont.Fontname = font.Fontname;
                                    objFinalFont.Fontsize = font.Fontsize;
                                    objFinalFont.Type = font.Body;
                                    objFinalFont.Pagerange = bodysectionRange;
                                    objFinalFont.Sectiontype = "body";
                                    objFinalFont.FontType = font.FontType;
                                    objFinalFont.Iscaps = font.Iscaps;
                                    break;
                                case "3":
                                    objFinalFont.Font = font.Font + font.Fontsize;
                                    objFinalFont.Fontname = font.Fontname;
                                    objFinalFont.Fontsize = font.Fontsize;
                                    objFinalFont.Type = font.Postsection;
                                    objFinalFont.Pagerange = postsectionRange;
                                    objFinalFont.Sectiontype = "post-section";
                                    objFinalFont.FontType = font.FontType;
                                    objFinalFont.Iscaps = font.Iscaps;
                                    break;
                                default:
                                    break;
                            }
                            finalFonts.Add(objFinalFont);
                        }
                    }
                    StringBuilder strFinalFont = new StringBuilder();
                    foreach (FinalFonts fFont in finalFonts)
                    {
                        if (fFont.Iscaps)
                        {
                            fFont.Fontname = "CAPS$" + fFont.Fontname;
                        }
                        string str = fFont.Font + "," + fFont.Fontname + "," + fFont.Fontsize + "," + fFont.Type + "," +
                                     fFont.Pagerange + "," + fFont.Sectiontype + "," + fFont.FontType;

                        strFinalFont.AppendLine(str);
                    }
                    File.WriteAllText(getFontsFilePath().Replace("UnAssignedFonts.txt", "FinalFonts.csv"),
                        strFinalFont.ToString());
                }

                string bookId = Convert.ToString(Request.QueryString["bid"]).Replace("-1", "");
                string queryBID = "Select BID From book Where Book.MainBook= '" + bookId + "'";
                string BID = objMyDBClass.GetID(queryBID);

                if (!string.IsNullOrEmpty(BID))
                {
                    string pageBreakStart = txtPageBreak.Text;
                    string bodyStart = txtBodyStart.Text;
                    string bodyEnd = txtBodyEnd.Text;

                    if (!string.IsNullOrEmpty(pageBreakStart) && !string.IsNullOrEmpty(bodyStart) &&
                     !string.IsNullOrEmpty(bodyEnd))
                    {
                        string queryUpdatePageBreak = "Update BookPageBreak Set BodyStart=" + bodyStart + ", BodyEnd=" + bodyEnd +
                                                      ", PageBreak=" + pageBreakStart + " Where BId='" + BID + "'";

                        int upResPageBreak = objMyDBClass.ExecuteCommand(queryUpdatePageBreak);
                    }
                }

                AutoMapService.AutoMappService autoMapSvc = new AutoMapService.AutoMappService();
                autoMapSvc.AllowAutoRedirect = true;
                autoMapSvc.AutoProcessAllAsync((Session["objUser"] as UserClass).UserName, "", getPdfPath(),
                    chkIndex.Checked, string.IsNullOrEmpty(txtindexStart.Text) ? 0 : int.Parse(txtindexStart.Text),
                    string.IsNullOrEmpty(txtindexEnd.Text) ? 0 : int.Parse(txtindexEnd.Text), chkImage.Checked,
                    chkTable.Checked, cbxAlgo1.Checked, cbxalgo2.Checked, cbxalgo3.Checked, cbxNPara.Checked, cbxNParaAlgo1.Checked, cbxNParaAlgo2.Checked, cbxSPara.Checked, cbxFootNotes.Checked);

                string rhywFilePath = getRhywFilePath();

                //stop waiting for tagginuntag completion
                while (!File.Exists(rhywFilePath))
                {

                }

                Thread.Sleep(2000);
                autoMapSvc.CancelAsync(null);
                autoMapSvc.Dispose();

                ////MyDBClass objMyDBClass = new MyDBClass();
                ////string filesSavingPath = objMyDBClass.MainDirPhyPath + "/";

                ////AutoTableDetection.AutoTableDetection autoTblSvc = new AutoTableDetection.AutoTableDetection();
                ////autoTblSvc.AllowAutoRedirect = true;
                ////autoTblSvc.AutoDetectTablesAsync(bookId, filesSavingPath, cbxAlgo1.Checked, cbxalgo2.Checked, cbxalgo3.Checked);
                ////Thread.Sleep(2000);
                ////autoTblSvc.CancelAsync(null);
                ////autoTblSvc.Dispose();

                ////if (screenHeight != "")
                ////{
                ////    // setImageDimensions_ByPhotoshop(screenHeight, screenWidth);
                ////}

                //objMyDBClass.CalculateTaskTime()
                //CheckBookPageBreak

                string bookDetails = GetBookComplexity(bookId);
                var temp = bookDetails.Split(',');

                if ((temp != null) && (temp.Length > 0))
                {
                    string complexity = Convert.ToString(temp[1]);
                    string pageCount = Convert.ToString(temp[0]);

                    string queryUpdate = "Update Book Set Complexity='" + complexity + "', PageCount=" + pageCount +
                                         " Where MainBook='" + bookId + "'";
                    int upRes = objMyDBClass.ExecuteCommand(queryUpdate);
                }

                if (!string.IsNullOrEmpty(BID))
                {
                    string queryUpdateStatus = "Update Activity Set Status='In Process' Where BID='" + BID + "' and task='TaggingUntagged'";
                    int status = objMyDBClass.ExecuteCommand(queryUpdateStatus);

                    //string pageBreakStart = txtPageBreak.Text;
                    //string bodyStart = txtBodyStart.Text;
                    //string bodyEnd = txtBodyEnd.Text;

                    //if (!string.IsNullOrEmpty(pageBreakStart) && !string.IsNullOrEmpty(bodyStart) &&
                    // !string.IsNullOrEmpty(bodyEnd))
                    //{
                    //    string queryUpdatePageBreak = "Update BookPageBreak Set BodyStart=" + bodyStart + ", BodyEnd=" + bodyEnd +
                    //                                  ", PageBreak=" + pageBreakStart + " Where BId='" + BID + "'";

                    //    int upResPageBreak = objMyDBClass.ExecuteCommand(queryUpdatePageBreak);
                    //}
                }

                //Set Priority of book
                if (ddlTasKPriority.SelectedIndex > 0)
                {
                    int priority = ddlTasKPriority.SelectedIndex;
                    string querySetPriority = "Update Activity Set Priority=" + priority + " Where BID='" + BID + "'";
                    int res = objMyDBClass.ExecuteCommand(querySetPriority);
                }
                //end

                string queryBookExists = "Select BookId From UserRankCanPerform Where BookId = '" + bookId.Trim() + "'";
                string bookIdCheck = objMyDBClass.GetID(queryBookExists);

                if (string.IsNullOrEmpty(bookIdCheck))
                {
                    //Set allowed users for error detection of book
                    if (cbxUserRanks.Items.Count > 0)
                    {
                        List<string> selectedValues = cbxUserRanks.Items.Cast<System.Web.UI.WebControls.ListItem>().Where(li => li.Selected).Select(li => li.Value).ToList();

                        if (selectedValues != null)
                        {
                            string sqlText = "";

                            foreach (var item in selectedValues)
                            {
                                if (item.Equals("Trainee Editor"))
                                    sqlText += "INSERT INTO UserRankCanPerform(BookId, RankId) VALUES ( '" + bookId.Trim() + "',' " + "1" + "'); ";

                                if (item.Equals("Junior Editor"))
                                    sqlText += "INSERT INTO UserRankCanPerform(BookId, RankId) VALUES ( '" + bookId.Trim() + "',' " + "2" + "'); ";

                                if (item.Equals("Editor"))
                                    sqlText += "INSERT INTO UserRankCanPerform(BookId, RankId) VALUES ( '" + bookId.Trim() + "',' " + "3" + "'); ";

                                if (item.Equals("Senior Editor"))
                                    sqlText += "INSERT INTO UserRankCanPerform(BookId, RankId) VALUES ( '" + bookId.Trim() + "',' " + "4" + "'); ";

                                if (item.Equals("Expert Editor"))
                                    sqlText += "INSERT INTO UserRankCanPerform(BookId, RankId) VALUES ( '" + bookId.Trim() + "',' " + "5" + "'); ";
                            }

                            if (!string.IsNullOrEmpty(sqlText))
                            {
                                MyDBClass dbObj = new MyDBClass();
                                SqlConnection connection = new SqlConnection(dbObj.ConnectionString());
                                SqlCommand command = new SqlCommand(sqlText, connection);
                                command.CommandType = CommandType.Text;
                                connection.Open();
                                int recordsInserted = command.ExecuteNonQuery();
                                connection.Close();
                            }
                        }
                    }
                }
                //end 

                string bookID = Request.QueryString["bid"] != null ? Request.QueryString["bid"].ToString() : "";
                bookID = bookID.Split(new char[] { '-' })[0];
                string bid = objMyDBClass.ExecuteSelectCom("select bid from book where mainbook like '" + bookID + "%'");

                //Commented on 2017-04-05

                //string completeQuery = "";
                //if (bid != "0")
                //{
                //    completeQuery = "Update ACTIVITY Set Status='Approved' Where BID=" + bid + " AND Task='TaggingUntagged' AND Status='Working'";
                //}
                //else
                //{
                //    completeQuery = "Update ACTIVITY Set Status='Approved' Where BID=" + bookID + " AND Task='TaggingUntagged' AND Status='Working'";
                //}
                //objMyDBClass.ExecuteCommand(completeQuery);


                //WorkMeter wmObj = new WorkMeter();
                //string taskTypeId = "29";
                //wmObj.StopTask(true, "complete", taskTypeId, bookId);

                if (!string.IsNullOrEmpty(bid) && !string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
                {
                    string completeQuery = "Update ACTIVITY Set Status='Approved' Where BID=" + bid + " AND Task='TaggingUntagged' AND Status='In Process'";
                    objMyDBClass.ExecuteCommand(completeQuery);

                    string pdfPath = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" +
                                     Convert.ToString(Request.QueryString["bid"]) + "/TaggingUntagged/" +
                                     Convert.ToString(Request.QueryString["bid"]) + ".pdf";

                    if (!string.IsNullOrEmpty(pdfPath) && File.Exists(pdfPath))
                        Session["PdfForSVGPath"] = pdfPath;

                    ////int result = objMyDBClass.CreateTask(bid, "Working", "ComplexBitsMapping", Convert.ToString(Session["LoginId"]));

                    //int result = objMyDBClass.CreateTask(bid, "Unassigned", "Table", Convert.ToString(Session["LoginId"]));

                    //Response.Redirect("AdminPanel.aspx", true);

                    if (IsImageDetected())
                        objMyDBClass.CreateTask(bid, "Unassigned", "Image", Convert.ToString(Session["LoginId"]));
                    if (IsTableDetected())
                        objMyDBClass.CreateTask(bid, "Unassigned", "Table", Convert.ToString(Session["LoginId"]));
                    else
                        objMyDBClass.CreateTask(bid, "Working", "ComplexBitsMapping", Convert.ToString(Session["LoginId"]));
                }

                //string boxDirPath = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/";

                //if (!Directory.Exists(boxDirPath))
                //{
                //    if (CreateDirectory(boxDirPath))
                //    {
                //        string pdfSvgXml = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/" + Convert.ToString(Request.QueryString["bid"]) +                          "_Svg.xml";

                //        while (!File.Exists(pdfSvgXml))
                //        {

                //        }

                //        Thread.Sleep(10000);

                //        string hdnValue = hfSvgContent.Value;
                //    }
                //}

                //Thread.Sleep(5000);

                //if(cbxNParaAlgo2.Checked)
                ////populateDivSparas(ddlParaType.SelectedValue); //Commented by Aamir Ghafoor on 2016-10-13

                //else 
                //    Response.Redirect("AdminPanel.aspx", true);
            }
        }

        public bool IsTableDetected()
        {
            string tablesDirctoryPath = objMyDBClass.MainDirPhyPath + Convert.ToString(Request.QueryString["bid"].Replace("-1", "")) + "\\DetectedTables\\TableXmls";

            if (!Directory.Exists(tablesDirctoryPath)) return false;

            if (Directory.GetFiles(tablesDirctoryPath, "*.xml").Length == 0)
                return false;

            return true;
        }

        public bool IsImageDetected()
        {
            string tablesDirctoryPath = objMyDBClass.MainDirPhyPath + Convert.ToString(Request.QueryString["bid"].Replace("-1", "")) +
                Convert.ToString(Request.QueryString["bid"]) + "\\TaggingUntagged\\" + Convert.ToString(Request.QueryString["bid"]) + ".rhyw";

            if (File.Exists(tablesDirctoryPath) && objGlobal != null)
            {
                objGlobal.XMLPath = tablesDirctoryPath;
                objGlobal.LoadXml();
            }

            if (objGlobal.PBPDocument != null)
            {
                var imageNodes = objGlobal.PBPDocument.SelectNodes("//image").Cast<XmlNode>().ToList();

                if (imageNodes.Count < 2) return false;
            }

            return true;
        }

        protected void btnSaveSvgXml_Click(object sender, EventArgs e)
        {
            SaveSvgAsXml();
            Response.Redirect("AdminPanel.aspx", true);
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

        public void GetSVGXMLFromPDFJs(string pdfPath)
        {
            pdfPath = "WebHandlers/GetPdfInSVG.ashx";
            Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "GetXMLFromPDFJs(" + pdfPath + ")", true);
        }

        public string GetBookComplexity(string bookId)
        {
            string Servicedata = "";
            string totalPages = "";
            string complexity = "";

            try
            {
                Service1Client client = new Service1Client();
                Servicedata = client.GetConversionFileDetailsForWebService(Convert.ToInt64(bookId), "test");
                client.Close();
            }
            catch (Exception ex)
            {
                Servicedata = "";
            }

            if (Servicedata == "")
            {
                string inputFile = objMyDBClass.MainDirPhyPath + "/" +
                                   Convert.ToString(Request.QueryString["bid"]).Replace("-1", "") + "/" +
                                   Convert.ToString(Request.QueryString["bid"]).Replace("-1", ".pdf");
                PdfReader inputPdf = new PdfReader(inputFile);
                totalPages = Convert.ToString(inputPdf.NumberOfPages);
                complexity = "Simple";
            }
            else
            {
                var result = Servicedata.Split('~');

                var temp_complexity = result[1].Split(':');
                var temp_totalPages = result[2].Split(':');

                totalPages = temp_totalPages[1];
                complexity = temp_complexity[1];
            }

            return totalPages + "," + complexity;
        }

        public void CreateSpellCheckTask(string aid, string action, string userId)
        {
            int inResult = 0;
            string queryBookID = "Select BID From Activity Where AID=" + aid;
            string bookID = objMyDBClass.GetID(queryBookID);
            inResult = objMyDBClass.CreateTask(bookID, "Unassigned", "SpellCheck", userId);
            Response.Redirect("AdminPanel.aspx");
        }

        private void preFixHandler(XmlNode node)
        {
            string txt = node.InnerText;
            if (node.NextSibling != null)
            {
                XmlNode nodePrefix = node.NextSibling.SelectSingleNode("//prefix");
                nodePrefix.InnerText = txt;
            }
            node.ParentNode.RemoveChild(node);
        }

        protected void chkIndex_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox cbox = (CheckBox)sender;
            if (cbox.Checked)
            {
                txtindexStart.Enabled = true;
                txtindexEnd.Enabled = true;
            }
            else
            {
                txtindexStart.Enabled = false;
                txtindexEnd.Enabled = false;

            }
        }

        #region |Section Handling|

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

        public void AddSection(XmlNode node, string type)
        {
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            SchemaElements objSchemaElem = new SchemaElements();


            XmlNode currentNode = node;
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

        protected void dlistMappedFonts_ItemCommand(object source, DataListCommandEventArgs e)
        {
            if (e.CommandName.Equals("UpdateFont"))
            {
                string[] FontInfo = e.CommandArgument.ToString().Split(',');
                txtFontType.Text = FontInfo[0] + " " + FontInfo[1];
                txtActualText.Text = FontInfo[2];
                lblPageno.Text = FontInfo[3];

                ShowPDF(lblPageno.Text);
            }
        }

        private List<string> getUniqueNodes(List<XmlNode> nodes)
        {
            List<string> fonts = new List<string>();
            foreach (XmlNode item in nodes)
            {
                XmlNode lineNode = item.SelectSingleNode(".//ln");
                if (lineNode != null)
                {
                    string fontInfo = lineNode.Attributes["font"].Value + " " + lineNode.Attributes["fontsize"].Value +
                                      " " + lineNode.InnerText;
                    for (int i = 0; i < fonts.Count; i++)
                    {
                        if (!fonts[i].Equals(fontInfo))
                        {
                            fonts.Add(fontInfo);
                        }
                    }
                }
            }
            return fonts;
        }

        private void populateEditedFonts()
        {
            if (File.Exists(getFontsFilePath()))
            {
                List<string> editedFonts = new List<string>();
                //StreamReader sr = new StreamReader(getFontsFilePath());
                //string fileData = sr.ReadToEnd().Replace("\r\n", "$");
                //string[] fonts = fileData.Remove(fileData.Length - 1, 1).Split('$');
                //sr.Dispose();

                string[] fonts = File.ReadAllLines(getFontsFilePath(), Encoding.UTF8);

                for (int i = 0; i < fonts.Length; i++)
                {
                    if (fonts[i].Contains("assigned"))
                    {
                        string[] fontdetail = fonts[i].Split('@');
                        fonts[i] = fontdetail[1] + "," + fontdetail[2] + "," + fontdetail[3] + "," + fontdetail[4];
                        editedFonts.Add(fonts[i]);
                    }
                }
                dlistMappedFonts.DataSource = editedFonts;
                dlistMappedFonts.DataBind();
            }

        }

        private string getRhywFilePath()
        {
            string bookID = Request.QueryString["bid"] != null ? Request.QueryString["bid"].ToString() : "";
            string filePath = objMyDBClass.MainDirPhyPath + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID +
                              "//TaggingUntagged//" + bookID + ".rhyw";
            return filePath;
        }

        private string getPdfPath()
        {
            string bookID = Request.QueryString["bid"] != null ? Request.QueryString["bid"].ToString() : "";
            string filePath = objMyDBClass.MainDirPhyPath + "/" + bookID.Split(new char[] { '-' })[0] + "/" +
                              bookID.Split(new char[] { '-' })[0] + ".pdf";
            return filePath;
        }

        private string getFontsFilePath()
        {
            string bookID = Request.QueryString["bid"] != null ? Request.QueryString["bid"].ToString() : "";
            string filePath = objMyDBClass.MainDirPhyPath + "/" + bookID.Split(new char[] { '-' })[0] +
                              "/UnAssignedFonts.txt";
            return filePath;
        }

        private void pupulateFontsFields()
        {
            if (File.Exists(getFontsFilePath()))
            {
                ////StreamReader sr = new StreamReader(getFontsFilePath());
                ////string fileData = sr.ReadToEnd().Replace("\r\n", "$");
                ////string[] fonts = fileData.Remove(fileData.Length - 1, 1).Split('$');
                //int remainingFonts = fonts.Length;
                //sr.Dispose();

                string[] fonts = File.ReadAllLines(getFontsFilePath(), Encoding.UTF8);
                int remainingFonts = fonts.Length;

                for (int i = 0; i < fonts.Length; i++)
                {
                    string font = fonts[i];
                    if (!font.Equals(""))
                    {
                        if (font.Contains("assigned"))
                        {
                            txtActualText.Text = "";
                            txtFontType.Text = "";
                            lblPageno.Text = "";
                            remainingFonts = remainingFonts - 1;
                            btnFinish.Enabled = true;
                            continue;
                        }
                        else
                        {
                            string[] fontDetail = font.Split('@');
                            txtActualText.Text = fontDetail[3];

                            //Added on 2017-09-26 for highlighting of word by Aamir Ghafoor
                            if (fontDetail.Length > 6)
                                WordCoordinates = fontDetail[6];

                            txtFontType.Text = fontDetail[1] + " " + fontDetail[2];
                            lblPageno.Text = fontDetail[4];
                            this.lblMessage.Text = "";
                            btnFinish.Enabled = false;
                            break;
                        }
                    }
                }
                lblRemainingFonts.Text = remainingFonts.ToString();
            }
        }

        private void Step1(string[] Fonts)
        {
            try
            {
                if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
                {
                    objGlobal.XMLPath = Session["XMLPath"].ToString();
                }
                if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
                {
                    objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                }
                objGlobal.LoadXml();
                Session["PBPDocument"] = objGlobal.PBPDocument;
                XmlNodeList untaggedList = objGlobal.PBPDocument.SelectNodes("//untagged");
                for (int i = 0; i < Fonts.Length; i++)
                {

                    string[] FontDetail = Fonts[i].Split(',');
                    List<XmlNode> matchedElements = new List<XmlNode>();
                    foreach (XmlNode node in untaggedList)
                    {
                        if (node.SelectSingleNode(".//ln") != null)
                        {
                            XmlNodeList lines = node.SelectNodes(".//ln");
                            foreach (XmlNode ln in lines)
                            {
                                string FontInfo = ln.Attributes["font"].Value + " " + ln.Attributes["fontsize"].Value;
                                if (FontInfo.Equals(FontDetail[0] + " " + FontDetail[1]))
                                {
                                    if (Fonts[i].Contains("caps"))
                                    {
                                        ln.InnerText = ln.InnerText.ToUpper();
                                    }
                                    matchedElements.Add(node);
                                }
                            }
                        }
                    }
                    foreach (XmlNode node in matchedElements)
                    {
                        string BoodSection = "";
                        int CurrentpageNo = Convert.ToInt32(node.SelectSingleNode(".//ln").Attributes["page"].Value);
                        int PreSectionStart = Convert.ToInt32(txtPreSectionStart.Text);
                        int PreSectionEnd = Convert.ToInt32(txtPreSectionEnd.Text);
                        int BodyStart = Convert.ToInt32(txtBodyStart.Text);
                        int BodyEnd = Convert.ToInt32(txtBodyEnd.Text);
                        int PostSectionStart = Convert.ToInt32(txtPostSectionStart.Text);
                        int PostSectionEnd = Convert.ToInt32(txtPostSectionEnd.Text);
                        if (CurrentpageNo <= PreSectionEnd && (CurrentpageNo >= PreSectionStart))
                        {
                            BoodSection = "Pre-Section";
                        }
                        else if (CurrentpageNo <= BodyEnd && (CurrentpageNo >= BodyStart))
                        {
                            BoodSection = "Body";
                        }
                        else if (CurrentpageNo <= PostSectionEnd && (CurrentpageNo >= PostSectionStart))
                        {
                            BoodSection = "Post-Section";
                        }
                        //if (Fonts[i].Contains("caps"))
                        //{
                        //    node.InnerText = node.InnerText.ToUpper();
                        //}
                        switch (BoodSection)
                        {
                            case "Pre-Section":
                                {
                                    node.Attributes["type"].Value = FontDetail[5] + " done";
                                    SaveXML();
                                    //MainOperation(node, ddlPreSection.SelectedValue);
                                    break;
                                }
                            case "Body":
                                {
                                    node.Attributes["type"].Value = FontDetail[6] + " done";
                                    SaveXML();
                                    //MainOperation(node, ddlBody.SelectedValue);
                                    break;
                                }
                            case "Post-Section":
                                {
                                    node.Attributes["type"].Value = FontDetail[7] + " done";
                                    SaveXML();
                                    //MainOperation(node, ddlPostSection.SelectedValue);
                                    break;
                                }
                        }
                    }

                }
            }
            catch (Exception)
            {

                throw;
            }
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

        private void ConvertPdfToImage(string pdfPath)
        {
            PDF2JPG_Sofnix.Convert cnv = new PDF2JPG_Sofnix.Convert();
            bool output = cnv.PDF2ImageFromFile(pdfPath, "");
        }

        public string GetTempXmlPath()
        {
            return objMyDBClass.MainDirPhyPath + "/" + Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0] +
                   "/DetectedFootNotes/Temp_FootNotes.xml";
        }

        //private void convertToBox(bool isApplyAll)
        //{
        //    try
        //    {
        //        //ConvertPdfToImage("");

        //        LoadPdfXml();

        //        XmlNode abnormalPara = null;
        //        List<XmlNode> paraToConvert = new List<XmlNode>();

        //        int page = 0;

        //        if (isApplyAll)
        //        {
        //            abnormalPara = objGlobal.PBPDocument.SelectSingleNode("//*[@abnormalLeft][1]");

        //            if (abnormalPara != null)
        //            {
        //                page = Convert.ToInt32(abnormalPara.SelectSingleNode("descendant::ln/@page").Value);

        //                bool isEven = page % 2 == 0 ? true : false;

        //                var allMatchingNodes = objGlobal.PBPDocument.SelectNodes("//upara[@abnormalLeft='" + abnormalPara.Attributes["abnormalLeft"].Value + "']").Cast<XmlNode>().ToList();
        //                if (isEven)
        //                {
        //                    foreach (XmlNode para in allMatchingNodes)
        //                    {
        //                        if (para.ChildNodes != null && para.ChildNodes.Count > 0)
        //                        {
        //                            if (Convert.ToInt32(para.ChildNodes[0].Attributes["page"].Value) % 2 == 0)
        //                                paraToConvert.Add(para);
        //                        }
        //                    }
        //                }
        //                else
        //                {
        //                    foreach (XmlNode para in allMatchingNodes)
        //                    {
        //                        if (para.ChildNodes != null && para.ChildNodes.Count > 0)
        //                        {
        //                            if (Convert.ToInt32(para.ChildNodes[0].Attributes["page"].Value) % 2 != 0)
        //                                paraToConvert.Add(para);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        else
        //        {
        //            XmlNodeList allParas = objGlobal.PBPDocument.SelectNodes("//upara");

        //            foreach (XmlNode abnormalNode in allParas)
        //            {
        //                if (abnormalNode.Attributes["abnormalLeft"] != null)
        //                    paraToConvert.Add(abnormalNode);

        //                else if (paraToConvert.Count > 0) break;
        //            }
        //        }

        //        if (paraToConvert != null)
        //        {
        //            if (paraToConvert.Count > 0)
        //            {
        //                XmlNode boxElem = null;

        //                page = paraToConvert[0].ChildNodes.Count > 0 ? Convert.ToInt32(paraToConvert[0].ChildNodes[0].Attributes["page"].Value) : 0;

        //                for (int i = 0; i < paraToConvert.Count; i++)
        //                {
        //                    if (i == 0)
        //                    {
        //                        boxElem = objGlobal.PBPDocument.CreateElement("box");

        //                        ((XmlElement)boxElem).SetAttribute("id", "0");
        //                        ((XmlElement)boxElem).SetAttribute("bgcolor", "gray");
        //                        ((XmlElement)boxElem).SetAttribute("border", "off");

        //                        XmlNode boxTitleElem = objGlobal.PBPDocument.CreateElement("box-title");
        //                        boxElem.PrependChild(boxTitleElem);

        //                        XmlElement ln = objGlobal.PBPDocument.CreateElement("ln");

        //                        if (paraToConvert.Count > 1 &&
        //                            paraToConvert[0].ChildNodes.Count > 0 &&
        //                            paraToConvert[1].ChildNodes.Count > 0 &&
        //                            paraToConvert[0].ChildNodes[0].Attributes["fontsize"] != null &&
        //                            paraToConvert[1].ChildNodes[0].Attributes["fontsize"] != null &&
        //                            Convert.ToDouble(paraToConvert[0].ChildNodes[0].Attributes["fontsize"].Value) >
        //                            Convert.ToDouble(paraToConvert[1].ChildNodes[0].Attributes["fontsize"].Value))
        //                        {
        //                            ln.SetAttribute("coord", paraToConvert[0].ChildNodes[0].Attributes["coord"].Value);
        //                            ln.SetAttribute("page", Convert.ToString(page));
        //                            ln.SetAttribute("height", paraToConvert[0].ChildNodes[0].Attributes["height"].Value);
        //                            ln.SetAttribute("left", paraToConvert[0].ChildNodes[0].Attributes["left"].Value);
        //                            ln.SetAttribute("top", paraToConvert[0].ChildNodes[0].Attributes["top"].Value);
        //                            ln.SetAttribute("font", paraToConvert[0].ChildNodes[0].Attributes["font"].Value);
        //                            ln.SetAttribute("fontsize", paraToConvert[0].ChildNodes[0].Attributes["fontsize"].Value);
        //                            ln.SetAttribute("error", paraToConvert[0].ChildNodes[0].Attributes["error"].Value);
        //                            ln.SetAttribute("ispreviewpassed", paraToConvert[0].ChildNodes[0].Attributes["ispreviewpassed"].Value);
        //                            ln.SetAttribute("isUserSigned", paraToConvert[0].ChildNodes[0].Attributes["isUserSigned"].Value);
        //                            ln.SetAttribute("isEditted", paraToConvert[0].ChildNodes[0].Attributes["isEditted"].Value);
        //                            ln.InnerText = paraToConvert[0].ChildNodes[0].InnerText;
        //                        }
        //                        else
        //                        {
        //                            ln.SetAttribute("coord", "0:0:0:0");
        //                            ln.SetAttribute("page", Convert.ToString(page));
        //                            ln.SetAttribute("height", "0");
        //                            ln.SetAttribute("left", "0");
        //                            ln.SetAttribute("top", "0");
        //                            ln.SetAttribute("font", "Arial");
        //                            ln.SetAttribute("fontsize", "12");
        //                            ln.SetAttribute("error", "0");
        //                            ln.SetAttribute("ispreviewpassed", "true");
        //                            ln.SetAttribute("isUserSigned", "1");
        //                            ln.SetAttribute("isEditted", "true");
        //                        }

        //                        boxTitleElem.AppendChild(ln);

        //                        XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("upara");

        //                        convertedNode.InnerXml = paraToConvert[i].InnerXml;

        //                        convertedNode.Attributes.RemoveNamedItem("abnormal");

        //                        boxElem.AppendChild(convertedNode);

        //                        paraToConvert[i].ParentNode.InsertBefore(boxElem, paraToConvert[i]);
        //                        objGlobal.SaveXml();
        //                    }
        //                    else
        //                    {
        //                        XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("upara");

        //                        convertedNode.InnerXml = paraToConvert[i].InnerXml;

        //                        convertedNode.Attributes.RemoveNamedItem("abnormalLeft");
        //                        convertedNode.Attributes.RemoveNamedItem("abnormalRight");

        //                        boxElem.AppendChild(convertedNode);

        //                        paraToConvert[i].ParentNode.InsertBefore(boxElem, paraToConvert[i]);
        //                        objGlobal.SaveXml();
        //                    }
        //                }
        //                for (int j = 0; j < paraToConvert.Count; j++)
        //                {
        //                    paraToConvert[j].ParentNode.RemoveChild(paraToConvert[j]);
        //                    objGlobal.SaveXml();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        private void convertToUpara(bool isApplyAll)
        {
            try
            {
                LoadPdfXml();

                XmlNode abnormalPara = null;
                List<XmlNode> paraToConvert = new List<XmlNode>();

                //if (!isApplyAll)
                //{
                //abnormalPara = objGlobal.PBPDocument.SelectSingleNode("//*[@abnormalLeft and not(@pType='nPara')][1]");

                abnormalPara = objGlobal.PBPDocument.SelectSingleNode("//*[@abnormalLeft][1]");

                if (abnormalPara != null)
                {
                    int page = Convert.ToInt32(abnormalPara.SelectSingleNode("descendant::ln/@page").Value);

                    bool isEven = page % 2 == 0 ? true : false;

                    //var allMatchingNodes = objGlobal.PBPDocument.SelectNodes("//upara[@abnormalLeft='" + abnormalPara.Attributes["abnormalLeft"].Value +
                    //                                                      "' and not(@pType='nPara')]").Cast<XmlNode>().ToList();

                    var allMatchingNodes = objGlobal.PBPDocument.SelectNodes("//upara[@abnormalLeft='" + abnormalPara.Attributes["abnormalLeft"].Value +
                                                                          "']").Cast<XmlNode>().ToList();

                    if (isEven)
                    {
                        foreach (XmlNode para in allMatchingNodes)
                        {
                            if (para.ChildNodes != null && para.ChildNodes.Count > 0)
                            {
                                if (Convert.ToInt32(para.ChildNodes[0].Attributes["page"].Value) % 2 == 0)
                                    paraToConvert.Add(para);
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
                                    paraToConvert.Add(para);
                            }
                        }
                    }
                }
                //}
                //else
                //{
                //    //XmlNodeList allParas = objGlobal.PBPDocument.SelectNodes("//upara");

                //    XmlNodeList allParas = objGlobal.PBPDocument.SelectNodes("//*[@abnormalLeft and not(@pType='nPara')]");

                //    foreach (XmlNode abnormalNode in allParas)
                //    {
                //        if (abnormalNode.Attributes["abnormalLeft"] != null)
                //            paraToConvert.Add(abnormalNode);
                //    }
                //}

                if (abnormalPara == null || abnormalPara.Attributes["abnormalLeft"] == null) return;

                if (paraToConvert != null)
                {
                    if (paraToConvert.Count > 0)
                    {
                        foreach (XmlNode para in paraToConvert)
                        {
                            //if (para.Attributes["pType"] != null && !para.Attributes["pType"].Value.Equals("nPara"))
                            //{
                            para.Attributes.RemoveNamedItem("abnormalLeft");
                            para.Attributes.RemoveNamedItem("abnormalRight");
                            objGlobal.SaveXml();
                            //}
                        }
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void convertToNpara(bool isApplyAll)
        {
            try
            {
                LoadPdfXml();

                XmlNode abnormalPara = null;
                List<XmlNode> paraToConvert = new List<XmlNode>();

                //if (!isApplyAll)
                //{
                abnormalPara = objGlobal.PBPDocument.SelectSingleNode("//*[@abnormalLeft and @pType='nPara'][1]");

                if (abnormalPara != null)
                {
                    int page = Convert.ToInt32(abnormalPara.SelectSingleNode("descendant::ln/@page").Value);

                    bool isEven = page % 2 == 0 ? true : false;

                    var allMatchingNodes = objGlobal.PBPDocument.SelectNodes("//upara[@abnormalLeft='" + abnormalPara.Attributes["abnormalLeft"].Value +
                                                                          "' and @pType='nPara']").Cast<XmlNode>().ToList();
                    if (isEven)
                    {
                        foreach (XmlNode para in allMatchingNodes)
                        {
                            if (para.ChildNodes != null && para.ChildNodes.Count > 0)
                            {
                                if (Convert.ToInt32(para.ChildNodes[0].Attributes["page"].Value) % 2 == 0)
                                    paraToConvert.Add(para);
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
                                    paraToConvert.Add(para);
                            }
                        }
                    }
                }
                //}
                //else
                //{
                //    XmlNodeList allParas = objGlobal.PBPDocument.SelectNodes("//upara");

                //    foreach (XmlNode abnormalNode in allParas)
                //    {
                //        if (abnormalNode.Attributes["abnormalLeft"] != null &&
                //            abnormalNode.Attributes["pType"] != null &&
                //            abnormalNode.Attributes["pType"].Value.Equals("nPara") &&
                //            (char.IsNumber(abnormalNode.InnerText.Trim().ToCharArray().ElementAt(0)) ||
                //            Convert.ToString(abnormalNode.InnerText.Trim().ToCharArray().ElementAt(0)).Equals("•")))
                //            paraToConvert.Add(abnormalNode);

                //        //else if (paraToConvert.Count > 0) break;
                //    }
                //}

                if (paraToConvert != null)
                {
                    if (paraToConvert.Count > 0)
                    {
                        for (int i = 0; i < paraToConvert.Count; i++)
                        {
                            XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("npara");

                            ((XmlElement)convertedNode).SetAttribute("id", paraToConvert[i].Attributes["id"].Value);
                            ((XmlElement)convertedNode).SetAttribute("pnum", paraToConvert[i].Attributes["pnum"].Value);
                            ((XmlElement)convertedNode).SetAttribute("text-indent", paraToConvert[i].Attributes["text-indent"].Value);

                            var nParaLines = InsertNumTag(paraToConvert[i]);

                            convertedNode.InnerXml = paraToConvert[i].InnerXml;

                            paraToConvert[i].ParentNode.InsertBefore(convertedNode, paraToConvert[i]);
                            objGlobal.SaveXml();
                        }

                        for (int j = 0; j < paraToConvert.Count; j++)
                        {
                            paraToConvert[j].ParentNode.RemoveChild(paraToConvert[j]);
                            objGlobal.SaveXml();
                        }

                        //foreach (XmlNode para in paraToConvert)
                        //{
                        //    para.Attributes.RemoveNamedItem("abnormalLeft");
                        //    para.Attributes.RemoveNamedItem("abnormalRight");
                        //    para.Attributes.RemoveNamedItem("pType");
                        //    objGlobal.SaveXml();
                        //}
                    }
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        private void convertSpara(bool isApplyAll)
        {
            try
            {
                LoadPdfXml();

                XmlNode abnormalPara = null;
                List<XmlNode> paraToConvert = new List<XmlNode>();

                //if (isApplyAll)
                //{
                abnormalPara = objGlobal.PBPDocument.SelectSingleNode("//*[@abnormalLeft and @pType='sPara'][1]");

                if (abnormalPara != null)
                {
                    int page = Convert.ToInt32(abnormalPara.SelectSingleNode("descendant::ln/@page").Value);

                    bool isEven = page % 2 == 0 ? true : false;

                    var allMatchingNodes = objGlobal.PBPDocument.SelectNodes("//upara[@abnormalLeft='" + abnormalPara.Attributes["abnormalLeft"].Value +
                                                                          "' and @pType='sPara']").Cast<XmlNode>().ToList();
                    if (isEven)
                    {
                        foreach (XmlNode para in allMatchingNodes)
                        {
                            if (para.ChildNodes != null && para.ChildNodes.Count > 0)
                            {
                                if (Convert.ToInt32(para.ChildNodes[0].Attributes["page"].Value) % 2 == 0)
                                    paraToConvert.Add(para);
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
                                    paraToConvert.Add(para);
                            }
                        }
                    }
                }
                //}
                //else
                //{
                //    XmlNodeList allParas = objGlobal.PBPDocument.SelectNodes("//upara");

                //    //foreach (XmlNode abnormalNode in allParas)
                //    //{
                //    //    if (abnormalNode.Attributes["abnormalLeft"] != null &&
                //    //        abnormalNode.Attributes["pType"] != null &&
                //    //        abnormalNode.Attributes["pType"].Value.Equals("sPara"))

                //    //        paraToConvert.Add(abnormalNode);

                //    //    else if (paraToConvert.Count > 0) break;
                //    //}

                //    foreach (XmlNode abnormalNode in allParas)
                //    {
                //        if (abnormalNode.Attributes["abnormalLeft"] != null && abnormalNode.Attributes["pType"] != null)
                //            paraToConvert.Add(abnormalNode);

                //        //else if (paraToConvert.Count > 0) break;
                //    }
                //}

                //XmlNode abnormalPara = objGlobal.PBPDocument.SelectSingleNode("(//*[@abnormalLeft])[1]");
                //XmlNodeList ParaToConvert = objGlobal.PBPDocument.SelectNodes("//upara[@abnormalLeft='" + abnormalPara.Attributes["abnormalLeft"].Value + "']");

                if (paraToConvert != null)
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

                        if (abnormalPara != null)
                        {
                            foreach (XmlAttribute attr in abnormalPara.Attributes)
                            {
                                if (attr.Name == "id" | attr.Name == "pnum" | attr.Name == "text-indent" |
                                    attr.Name == "padding-bottom" | attr.Name == "conversion-Operations")
                                    ((XmlElement)convertedNode).SetAttribute(attr.Name, attr.Value);

                            }
                            if (convertedNode.Attributes["conversion-Operations"].Value != "")
                            {
                                convertedNode.Attributes["conversion-Operations"].Value =
                                    convertedNode.Attributes["conversion-Operations"].Value + ",converted";
                            }
                            else
                            {
                                convertedNode.Attributes["conversion-Operations"].Value = "converted";
                            }
                        }

                        #endregion

                        item.ParentNode.InsertBefore(convertedNode, item);
                        objGlobal.SaveXml();
                    }
                    foreach (XmlNode item in paraToConvert)
                    {
                        item.ParentNode.RemoveChild(item);
                        objGlobal.SaveXml();
                    }
                }

            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public XmlNode InsertNumTag(XmlNode lines)
        {
            var linesList = lines.SelectNodes("descendant::ln").Cast<XmlNode>().ToList();

            foreach (XmlNode line in linesList)
            {
                if (char.IsNumber(line.InnerText.Trim().ToCharArray().ElementAt(0)))
                {
                    List<string> allWords = Regex.Split(line.InnerText.Trim(), @"\s+").Where(x => !string.IsNullOrEmpty(x)).ToList();
                    string numPart = allWords[0];
                    line.InnerXml = "<num>" + numPart + "</num>" + line.InnerText.Trim().Remove(0, allWords[0].Count());
                }
            }
            return null;
        }

        private void LoadPdfXml()
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["XMLPath"])))
            {
                objGlobal.XMLPath = Convert.ToString(Session["XMLPath"]);
                objGlobal.LoadXml();
            }
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

        private void setImageDimensions_ByPhotoshop(string height, string width)
        {
            string imageFolderPath = objMyDBClass.MainDirPhyPath + "/" +
                                     Convert.ToString(Request.QueryString["bid"]).Replace("-1", "") + "/" +
                                     Convert.ToString(Request.QueryString["bid"]) + "/Image";

            if (!Directory.Exists(imageFolderPath))
                return;

            if (Directory.GetFiles(imageFolderPath).Length == 0)
                return;
            #region |Commented|
            //    Photoshop.Application appRef = default(Photoshop.Application);
            //    Photoshop.Document currentDoc = default(Photoshop.Document);

            //    double originalWidth;
            //    double originalHeight;
            //    double targetHeight;
            //    double targetWidth;
            //    double dpi;

            //    try
            //    {
            //        int minx, miny, maxx, maxy;
            //        minx = miny = int.MaxValue;
            //        maxx = maxy = int.MinValue;

            //        minx = 0;
            //        miny = 0;
            //        maxx = Convert.ToInt32(width);
            //        maxy = Convert.ToInt32(height);

            //        System.Drawing.Rectangle tempRect = new System.Drawing.Rectangle(1, 0, maxx, maxy);
            //        appRef = new Photoshop.Application();

            //        String[] files = Directory.GetFiles(imageFolderPath, "*.tif");

            //        foreach (string fl in files)
            //        {
            //            currentDoc = appRef.Open(fl);
            //            currentDoc.ChangeMode(PsChangeMode.psConvertToCMYK);
            //            appRef.Preferences.RulerUnits = Photoshop.PsUnits.psPixels;

            //            originalWidth = currentDoc.Width;
            //            originalHeight = currentDoc.Height;
            //            double percentwidth = originalWidth / originalHeight * 100;
            //            double percentHieght = originalHeight / originalWidth * 100;
            //            dpi = currentDoc.Resolution;
            //            targetWidth = (originalWidth * 300) / dpi;
            //            targetHeight = (originalHeight * 300) / dpi;
            //            if (targetHeight > 2066)
            //            {
            //                targetHeight = 2066;
            //                targetWidth = 2066 / percentHieght;
            //                targetWidth = targetWidth * 100;
            //            }
            //            else if (targetWidth > 1239)
            //            {
            //                targetWidth = 1239;
            //                targetHeight = 1239 / percentwidth;
            //                targetHeight = targetHeight * 100;
            //            }
            //            else
            //            {
            //                targetHeight = (originalHeight);
            //                targetWidth = (originalWidth);
            //            }
            //            Photoshop.JPEGSaveOptions jpeg = new Photoshop.JPEGSaveOptions();
            //            jpeg.Quality = 8;
            //            currentDoc.ResizeImage(targetWidth, targetHeight, 300, PsResampleMethod.psBicubic);
            //            currentDoc.SaveAs(imageFolderPath + "\\" + Path.GetFileName(fl), jpeg);
            //            currentDoc.Close(PsSaveOptions.psDoNotSaveChanges);
            //        }

            //        var oldImages = Directory.GetFiles(imageFolderPath, "*.tif");

            //        foreach (var img in oldImages)
            //        {
            //            File.Delete(img);
            //        }
            //    }

            //    catch (Exception ex)
            //    {

            //    }
            //    finally
            //    {

            //    }
            //}
            #endregion

        #endregion

        }

        //public string GetPdfNormalBgColor()
        //{
        //    try
        //    {
        //        string bookId = Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0];
        //        string pdfSvgXml = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/" + Convert.ToString(Request.QueryString["bid"]) + "_Svg.xml";

        //        XmlDocument xDoc = new XmlDocument();
        //        xDoc.Load(pdfSvgXml);

        //        List<ColorTypes> allColors = GetAllColors();

        //        XmlNodeList pdfColorsList = xDoc.SelectNodes("//@fill");

        //        List<string> colorsList = new List<string>();

        //        foreach (XmlNode col in pdfColorsList)
        //        {
        //            if (!col.Value.Equals("none"))
        //            {
        //                var matchedColor = allColors.Where(x =>
        //                        (x.Red == Convert.ToInt32(col.Value.Replace("rgb(", "").Replace(")", "").Split(',')[0])) &&
        //                        (x.Green == Convert.ToInt32(col.Value.Replace("rgb(", "").Replace(")", "").Split(',')[1])) &&
        //                        (x.Blue == Convert.ToInt32(col.Value.Replace("rgb(", "").Replace(")", "").Split(',')[2]))).ToList();

        //                foreach (ColorTypes matchColor in matchedColor)
        //                {
        //                    colorsList.Add(matchColor.ColorName);
        //                }
        //            }
        //        }

        //        var result = colorsList.GroupBy(x => x).Select(group => new { value = group.Key, Count = group.Count() }).FirstOrDefault();

        //        return result.value;
        //    }
        //    catch (Exception ex)
        //    {
        //        return "";
        //    }
        //}

        //protected void btnSaveSvgXml_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        string hdnValue = hfSvgContent.Value;

        //        if (!string.IsNullOrEmpty(hdnValue))
        //        {
        //            string bookId = Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0];
        //            string boxDirPath = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/";

        //            if (!Directory.Exists(boxDirPath))
        //            {
        //                if (CreateDirectory(boxDirPath))
        //                {
        //                    string pdfSvgXml = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/" +
        //                                       Convert.ToString(Request.QueryString["bid"]) + "_Svg.xml";

        //                    hdnValue = hdnValue.Replace("svg:svg", "svg")
        //                        .Replace("svg:defs", "defs")
        //                        .Replace("svg:clipPath", "clipPath")
        //                        .Replace("svg:rect", "rect")
        //                        .Replace("svg:g", "g")
        //                        .Replace("svg:path", "path")
        //                        .Replace("svg:tspan", "tspan")
        //                        .Replace("svg:text", "text");

        //                    File.WriteAllText(pdfSvgXml, hdnValue);

        //                    hfSvgContent.Value = "";
        //                    Session["PdfForSVGPath"] = "";
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

        private void populateDivSparas(string selectedParaType)
        {
            divFontsAssignment.Visible = false;
            divSparaAssignment.Visible = true;
            populateSessions();
            objGlobal.XMLPath = Session["XMLPath"].ToString();

            if (!File.Exists(objGlobal.XMLPath)) return;

            objGlobal.LoadXml();

            XmlNodeList allParas = null;
            List<XmlNode> firstAbnomalParas = null;
            bool isFootNoteSelected = false;
            int pageNum = 0;

            StringBuilder sbParaText = new StringBuilder();

            allParas = objGlobal.PBPDocument.SelectNodes("//upara");

            if (allParas != null && allParas.Count > 0)
                firstAbnomalParas = new List<XmlNode>();

            if (selectedParaType.Equals("footnote"))
            {
                showFootNoteParas();
                isFootNoteSelected = true;
            }
            else if (selectedParaType.Equals("endnote"))
            {
                Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "ShowEndNoteSelDialog()", true);
                populateTreeView();
                showEndNoteParas();
                isFootNoteSelected = true;
            }
            else if (selectedParaType.Equals("box"))
            {
                firstAbnomalParas = showBoxParas(allParas);
            }
            else if (selectedParaType.Equals("npara"))
            {
                firstAbnomalParas = new List<XmlNode>();

                foreach (XmlNode abnormalNode in allParas)
                {
                    if (abnormalNode.ChildNodes != null &&
                        abnormalNode.ChildNodes.Count > 0 &&
                        abnormalNode.ChildNodes[0].Attributes["page"] != null &&
                        abnormalNode.Attributes["abnormalLeft"] != null &&
                        abnormalNode.Attributes["pType"] != null &&
                        abnormalNode.Attributes["pType"].Value.Equals("nPara"))
                    {
                        pageNum = Convert.ToInt32(abnormalNode.ChildNodes[0].Attributes["page"].Value);
                        break;
                    }
                }

                foreach (XmlNode abnormalNode in allParas)
                {
                    if (abnormalNode.Attributes["abnormalLeft"] != null &&
                      abnormalNode.Attributes["pType"] != null &&
                      abnormalNode.Attributes["pType"].Value.Equals("nPara") &&
                      abnormalNode.ChildNodes != null &&
                      abnormalNode.ChildNodes.Count > 0 &&
                      abnormalNode.ChildNodes[0].Attributes["page"] != null &&
                       Convert.ToInt32(abnormalNode.ChildNodes[0].Attributes["page"].Value) == pageNum)
                        firstAbnomalParas.Add(abnormalNode);

                    else if (firstAbnomalParas.Count > 0) break;
                }
            }
            else
            {
                foreach (XmlNode abnormalNode in allParas)
                {
                    if (abnormalNode.Attributes["abnormalLeft"] != null)
                    {
                        if (abnormalNode.ChildNodes != null && abnormalNode.ChildNodes.Count > 0)
                        {
                            firstAbnomalParas.Add(abnormalNode);
                        }
                    }

                    else if (firstAbnomalParas.Count > 0) break;
                }
            }

            if (firstAbnomalParas != null && firstAbnomalParas.Count > 0)
            {
                XmlNode line = firstAbnomalParas[0].ChildNodes[0];

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

                    List<string> lstcoordiants = GetParaCoordinates(firstAbnomalParas, Convert.ToInt32(line.Attributes["page"].Value), sourcePagePath);

                    if (lstcoordiants != null && lstcoordiants.Count > 0)
                        HighLightDetectedTables(sourcePagePath, outPutFilePath, BaseColor.ORANGE, lstcoordiants);
                }
                ShowPDF(line.Attributes["page"].Value);

                foreach (XmlNode abnNode in firstAbnomalParas)
                {
                    foreach (XmlNode ln in abnNode.ChildNodes)
                    {
                        sbParaText.Append(ln.InnerText.Trim() + "</br>");
                    }
                }

                //divParaText.InnerHtml = "<font style='color:#4682b4'><sup>" + Convert.ToString(sbParaText) + "</sup></font>";
                divParaText.InnerHtml = Convert.ToString(sbParaText);
                sbParaText.Length = 0;
            }
            else
            {
                if (!isFootNoteSelected)
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

            //if (string.IsNullOrEmpty(Convert.ToString(Session["SvgXmlCreated"])))
            //{
            //    Thread.Sleep(10000);
            //    Session["SvgXmlCreated"] = "svgCreated";
            //}
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

                foreach (XmlNode para in abnormalParas)
                {
                    leftValues.AddRange(para.SelectNodes("descendant::ln[@coord]/@coord").Cast<XmlNode>()
                                             .Where(x => !string.IsNullOrEmpty(x.Value) && x.Value.Split(':').Length > 3)
                                             .Select(x => Convert.ToDouble(x.Value.Split(':')[0])));

                    bottomValues.AddRange(para.SelectNodes("descendant::ln[@coord]/@coord").Cast<XmlNode>()
                                             .Where(x => !string.IsNullOrEmpty(x.Value) && x.Value.Split(':').Length > 3)
                                             .Select(x => Convert.ToDouble(x.Value.Split(':')[1])));

                    rightValues.AddRange(para.SelectNodes("descendant::ln[@coord]/@coord").Cast<XmlNode>()
                                             .Where(x => !string.IsNullOrEmpty(x.Value) && x.Value.Split(':').Length > 3)
                                             .Select(x => Convert.ToDouble(x.Value.Split(':')[2])));

                    topValues.AddRange(para.SelectNodes("descendant::ln[@coord]/@coord").Cast<XmlNode>()
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

        //new one
        private string HighLightDetectedTables(string inFilePath, string outputFilePath, BaseColor color, List<string> lstcoordinates)
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

        protected void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                //SaveSvgAsXml();

                objGlobal.XMLPath = Session["XMLPath"].ToString();
                objGlobal.LoadXml();

                if (ddlParaType.SelectedValue.Equals("footnote"))
                {
                    convertToFootNote(cbxApplyAll.Checked);
                }
                else if (ddlParaType.SelectedValue.Equals("endnote"))
                {
                    convertToEndNote(cbxApplyAll.Checked);
                }
                else
                {
                    XmlNode abnormalPara = objGlobal.PBPDocument.SelectSingleNode("(//*[@abnormalLeft])[1]");

                    if (abnormalPara == null) return;

                    if (ddlParaType.SelectedValue.Equals("spara"))
                    {
                        convertSpara(cbxApplyAll.Checked);
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
                        convertToNpara(cbxApplyAll.Checked);
                    }
                    else if (ddlParaType.SelectedValue.Equals("box"))
                    {
                        //SaveSvgAsXml();
                        convertToBox(cbxApplyAll.Checked);
                    }
                    else if (ddlParaType.SelectedValue.Equals("upara"))
                    {
                        convertToUpara(cbxApplyAll.Checked);
                    }
                }

                populateDivSparas(ddlParaType.SelectedValue);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Box

        //private List<XmlNode> showBoxParas(XmlNodeList allParas)
        //{
        //    if (allParas == null || allParas.Count == 0) return null;

        //    List<XmlNode> firstAbnomalParas = new List<XmlNode>();
        //    StringBuilder sbParaText = new StringBuilder();
        //    bool isBoxPara = false;
        //    int page = 0;

        //    //Get next box containing page number
        //    foreach (XmlNode abnormalNode in allParas)
        //    {
        //        if (abnormalNode.ParentNode.Name.Equals("box")) isBoxPara = true;

        //        if (isBoxPara && !abnormalNode.ParentNode.Name.Equals("box") &&
        //            abnormalNode.ChildNodes != null &&
        //            abnormalNode.ChildNodes.Count > 0 &&
        //            abnormalNode.ChildNodes[0].Attributes["page"] != null)
        //        {
        //            page = Convert.ToInt32(abnormalNode.ChildNodes[0].Attributes["page"].Value);
        //            break;
        //        }
        //    }

        //    if (page == 0 && allParas[0].ChildNodes != null && allParas[0].ChildNodes.Count > 0 &&
        //        allParas[0].ChildNodes[0].Attributes["page"] != null)
        //    {
        //        page = Convert.ToInt32(allParas[0].ChildNodes[0].Attributes["page"].Value);
        //    }

        //    var notConvToBoxPara = allParas.Cast<XmlNode>().Where(x => x.ParentNode != null && !x.ParentNode.Name.Equals("box")).ToList();

        //    if (notConvToBoxPara.Count > 0)
        //    {

        //    //}

        //    //if (allParas[0].ChildNodes != null && allParas[0].ChildNodes.Count > 0 &&
        //    //    allParas[0].ChildNodes[0].Attributes["page"] != null)
        //    //{

        //        //int page = Convert.ToInt32(notConvToBoxPara[0].ChildNodes[0].Attributes["page"].Value);

        //        List<int> boxContainingPages = GetBoxContainingPage();

        //        if (boxContainingPages == null || boxContainingPages.Count == 0) return null;

        //        var boxParaText = GetBoxText(page);

        //        if (boxParaText == null || boxParaText.Count == 0) return null;

        //        foreach (XmlNode abnormalNode in notConvToBoxPara)
        //        {
        //            isBoxPara = false;

        //            if (!abnormalNode.ParentNode.Name.Equals("box") && abnormalNode.ChildNodes[0].Attributes["page"].Value == Convert.ToString(page))
        //            {
        //                if (abnormalNode.ChildNodes.Count > 0)
        //                {
        //                    if (boxContainingPages.Contains(page))
        //                    {
        //                            foreach (SvgBox boxLine in boxParaText)
        //                            {
        //                                List<string> boxLines = boxLine.Text.Split(new string[] {"\r\n", "\n"}, StringSplitOptions.None).ToList();

        //                                if (boxLines.Count > 0)
        //                                {
        //                                    for (int i = 0; i < boxLines.Count; i++)
        //                                    {
        //                                        if (IsBoxStartingLine(boxLines[i].Trim(), abnormalNode))
        //                                        {
        //                                            isBoxPara = true;
        //                                            break;
        //                                        }
        //                                    }
        //                                }
        //                            }

        //                        if (isBoxPara)
        //                        {
        //                            firstAbnomalParas.Add(abnormalNode);
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    if (firstAbnomalParas != null && firstAbnomalParas.Count > 0)
        //    {
        //        XmlNode line = firstAbnomalParas[0].ChildNodes[0];
        //        ShowPDF(line.Attributes["page"].Value);

        //        foreach (XmlNode abnNode in firstAbnomalParas)
        //        {
        //            foreach (XmlNode ln in abnNode.ChildNodes)
        //            {
        //                sbParaText.Append(ln.InnerText.Trim() + "</br>");
        //            }
        //        }

        //        divParaText.InnerHtml = Convert.ToString(sbParaText);
        //        sbParaText.Length = 0;
        //    }
        //    else
        //    {
        //        divParaText.InnerText = "";
        //    }

        //    return null;
        //}

        private List<XmlNode> showBoxParas(XmlNodeList allParas)
        {
            if (allParas == null || allParas.Count == 0) return null;

            List<XmlNode> firstAbnomalParas = new List<XmlNode>();
            StringBuilder sbParaText = new StringBuilder();
            bool isBoxPara = false;

            List<int> boxContainingPages = GetBoxContainingPage();

            if (boxContainingPages == null || boxContainingPages.Count == 0) return null;

            foreach (int page in boxContainingPages)
            {
                var boxParaText = GetBoxText(page);

                if (boxParaText != null || boxParaText.Count > 0)
                {
                    foreach (XmlNode abnormalNode in allParas)
                    {
                        isBoxPara = false;

                        if (!abnormalNode.ParentNode.Name.Equals("box") && abnormalNode.ChildNodes[0].Attributes["page"].Value == Convert.ToString(page))
                        {
                            if (abnormalNode.ChildNodes.Count > 0)
                            {
                                if (boxContainingPages.Contains(page))
                                {
                                    foreach (SvgBox boxLine in boxParaText)
                                    {
                                        List<string> boxLines = boxLine.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

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

                                    if (isBoxPara) firstAbnomalParas.Add(abnormalNode);
                                }
                            }
                        }
                    }

                    if (firstAbnomalParas != null && firstAbnomalParas.Count > 0) break;
                }
            }

            //if (firstAbnomalParas != null && firstAbnomalParas.Count > 0)
            //{
            //    XmlNode line = firstAbnomalParas[0].ChildNodes[0];
            //    ShowPDF(line.Attributes["page"].Value);

            //    foreach (XmlNode abnNode in firstAbnomalParas)
            //    {
            //        foreach (XmlNode ln in abnNode.ChildNodes)
            //        {
            //            sbParaText.Append(ln.InnerText.Trim() + "</br>");
            //        }
            //    }

            //    //divParaText.InnerHtml = Convert.ToString(sbParaText);

            //    divParaText.InnerHtml = "<font style='color:#4682b4'><sup>" + Convert.ToString(sbParaText) + "</sup></font>";

            //    sbParaText.Length = 0;
            //}
            //else
            //{
            //    divParaText.InnerText = "";
            //}

            return firstAbnomalParas;
        }

        public static bool IsAlphaNumeric(string strToCheck)
        {
            Regex rg = new Regex(@"^[a-zA-Z0-9\s,]*$");
            return rg.IsMatch(strToCheck);
        }

        public void SaveSvgAsXml()
        {
            try
            {
                string hdnValue = hfSvgContent.Value;

                if (!string.IsNullOrEmpty(hdnValue))
                {
                    string bookId = Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0];
                    string boxDirPath = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/";

                    if (!Directory.Exists(boxDirPath))
                    {
                        if (CreateDirectory(boxDirPath))
                        {
                            string pdfSvgXml = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/" +
                                               Convert.ToString(Request.QueryString["bid"]) + "_Svg.xml";

                            hdnValue = hdnValue.Replace("svg:svg", "svg")
                                .Replace("svg:defs", "defs")
                                .Replace("svg:clipPath", "clipPath")
                                .Replace("svg:rect", "rect")
                                .Replace("svg:g", "g")
                                .Replace("svg:path", "path")
                                .Replace("svg:tspan", "tspan")
                                .Replace("svg:text", "text");

                            File.WriteAllText(pdfSvgXml, hdnValue);

                            hfSvgContent.Value = "";
                            Session["PdfForSVGPath"] = "";
                        }
                    }
                }
            }
            catch (Exception ex)
            {

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
                string bookId = Convert.ToString(Request.QueryString["bid"]).Split(new char[] { '-' })[0];
                string boxDirPath = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/";

                string pdfSvgXml = objMyDBClass.MainDirPhyPath + "/" + bookId + "/" + "/DetectedBox/" +
                                              Convert.ToString(Request.QueryString["bid"]) + "_Svg.xml";


                XmlDocument xDoc = new XmlDocument();
                xDoc.Load(pdfSvgXml);

                List<int> boxContPages = new List<int>();

                XmlNodeList colorBgPages = xDoc.SelectNodes("//path[@fill!='none']/../../../../@page");

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

        //private void convertToBox(bool isApplyAll)
        //{
        //    try
        //    {
        //        LoadPdfXml();

        //        string normalBgColor = GetPdfNormalBgColor();

        //        var boxBgColor = GetBoxBgColor(normalBgColor);

        //        if (BoxBgColors == null) BoxBgColors = new List<string>();

        //        BoxBgColors.Add(boxBgColor);

        //        XmlNode abnormalPara = null;
        //        List<XmlNode> paraToConvert = new List<XmlNode>();

        //        int page = 0;

        //        List<int> boxContainingPages = GetBoxContainingPage();

        //        //if (!isApplyAll)
        //        //{
        //        //abnormalPara = objGlobal.PBPDocument.SelectSingleNode("//*[@abnormalLeft][1]");

        //        var abnormalAllParas = objGlobal.PBPDocument.SelectNodes("//upara");

        //        foreach (XmlNode abnormalNode in abnormalAllParas)
        //        {
        //            if (abnormalNode.Attributes["abnormalLeft"] != null &&
        //                abnormalNode.ChildNodes != null &&
        //                abnormalNode.ChildNodes[0].Attributes["page"] != null &&
        //                boxContainingPages.Contains(Convert.ToInt32(abnormalNode.ChildNodes[0].Attributes["page"].Value)))
        //            {
        //                if (abnormalNode.ChildNodes != null && abnormalNode.ChildNodes.Count > 0)
        //                {
        //                    abnormalPara = abnormalNode;
        //                    break;
        //                }
        //            }
        //        }

        //        if (abnormalPara != null)
        //        {
        //            page = Convert.ToInt32(abnormalPara.SelectSingleNode("descendant::ln/@page").Value);

        //            bool isEven = page % 2 == 0 ? true : false;

        //            var allMatchingNodes = objGlobal.PBPDocument.SelectNodes("//upara[@abnormalLeft='" +
        //                                   abnormalPara.Attributes["abnormalLeft"].Value + "']").Cast<XmlNode>().ToList();
        //            if (isEven)
        //            {
        //                foreach (XmlNode para in allMatchingNodes)
        //                {
        //                    if (para.ChildNodes != null && para.ChildNodes.Count > 0)
        //                    {
        //                        if ((Convert.ToInt32(para.ChildNodes[0].Attributes["page"].Value) % 2 == 0) &&
        //                            boxContainingPages.Contains(Convert.ToInt32(para.ChildNodes[0].Attributes["page"].Value)))
        //                            paraToConvert.Add(para);
        //                    }
        //                }
        //            }
        //            else
        //            {
        //                foreach (XmlNode para in allMatchingNodes)
        //                {
        //                    if (para.ChildNodes != null && para.ChildNodes.Count > 0)
        //                    {
        //                        if ((Convert.ToInt32(para.ChildNodes[0].Attributes["page"].Value) % 2 != 0) &&
        //                            boxContainingPages.Contains(Convert.ToInt32(para.ChildNodes[0].Attributes["page"].Value)))
        //                            paraToConvert.Add(para);
        //                    }
        //                }
        //            }
        //        }
        //        //}
        //        //else //Apply all is true
        //        //{
        //        //    XmlNodeList allParas = objGlobal.PBPDocument.SelectNodes("//upara");

        //        //    foreach (XmlNode abnormalNode in allParas)
        //        //    {
        //        //        if (abnormalNode.Attributes["abnormalLeft"] != null)
        //        //            paraToConvert.Add(abnormalNode);

        //        //        else if (paraToConvert.Count > 0) break;
        //        //    }
        //        //}

        //        if (paraToConvert != null)
        //        {
        //            int prevPage = 0;

        //            if (paraToConvert.Count > 0)
        //            {
        //                XmlNode boxElem = null;

        //                for (int i = 0; i < paraToConvert.Count; i++)
        //                {
        //                    page = paraToConvert[i].ChildNodes.Count > 0 ? Convert.ToInt32(paraToConvert[i].ChildNodes[0].Attributes["page"].Value) : 0;

        //                    if (boxContainingPages.Contains(page))
        //                    {
        //                        var boxParaText = GetBoxText(page);
        //                        //To do (match by box para text)

        //                        var result = GetBoxTextByMatching(boxParaText, paraToConvert);

        //                        if (page != prevPage)
        //                        {
        //                            boxElem = objGlobal.PBPDocument.CreateElement("box");

        //                            ((XmlElement)boxElem).SetAttribute("id", "0");
        //                            ((XmlElement)boxElem).SetAttribute("bgcolor", "gray");
        //                            ((XmlElement)boxElem).SetAttribute("border", "off");

        //                            XmlNode boxTitleElem = objGlobal.PBPDocument.CreateElement("box-title");
        //                            boxElem.PrependChild(boxTitleElem);

        //                            XmlElement ln = objGlobal.PBPDocument.CreateElement("ln");

        //                            if (paraToConvert.Count > 1 &&
        //                                paraToConvert[0].ChildNodes.Count > 0 &&
        //                                paraToConvert[1].ChildNodes.Count > 0 &&
        //                                paraToConvert[0].ChildNodes[0].Attributes["fontsize"] != null &&
        //                                paraToConvert[1].ChildNodes[0].Attributes["fontsize"] != null &&
        //                                Convert.ToDouble(paraToConvert[0].ChildNodes[0].Attributes["fontsize"].Value) >
        //                                Convert.ToDouble(paraToConvert[1].ChildNodes[0].Attributes["fontsize"].Value))
        //                            {
        //                                ln.SetAttribute("coord", paraToConvert[0].ChildNodes[0].Attributes["coord"].Value);
        //                                ln.SetAttribute("page", Convert.ToString(page));
        //                                ln.SetAttribute("height", paraToConvert[0].ChildNodes[0].Attributes["height"].Value);
        //                                ln.SetAttribute("left", paraToConvert[0].ChildNodes[0].Attributes["left"].Value);
        //                                ln.SetAttribute("top", paraToConvert[0].ChildNodes[0].Attributes["top"].Value);
        //                                ln.SetAttribute("font", paraToConvert[0].ChildNodes[0].Attributes["font"].Value);
        //                                ln.SetAttribute("fontsize",
        //                                    paraToConvert[0].ChildNodes[0].Attributes["fontsize"].Value);
        //                                ln.SetAttribute("error", paraToConvert[0].ChildNodes[0].Attributes["error"].Value);
        //                                ln.SetAttribute("ispreviewpassed",
        //                                    paraToConvert[0].ChildNodes[0].Attributes["ispreviewpassed"].Value);
        //                                ln.SetAttribute("isUserSigned",
        //                                    paraToConvert[0].ChildNodes[0].Attributes["isUserSigned"].Value);
        //                                ln.SetAttribute("isEditted",
        //                                    paraToConvert[0].ChildNodes[0].Attributes["isEditted"].Value);
        //                                ln.InnerText = paraToConvert[0].ChildNodes[0].InnerText;
        //                            }
        //                            else
        //                            {
        //                                ln.SetAttribute("coord", "0:0:0:0");
        //                                ln.SetAttribute("page", Convert.ToString(page));
        //                                ln.SetAttribute("height", "0");
        //                                ln.SetAttribute("left", "0");
        //                                ln.SetAttribute("top", "0");
        //                                ln.SetAttribute("font", "Arial");
        //                                ln.SetAttribute("fontsize", "12");
        //                                ln.SetAttribute("error", "0");
        //                                ln.SetAttribute("ispreviewpassed", "true");
        //                                ln.SetAttribute("isUserSigned", "1");
        //                                ln.SetAttribute("isEditted", "true");
        //                            }

        //                            boxTitleElem.AppendChild(ln);

        //                            XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("upara");

        //                            convertedNode.InnerXml = paraToConvert[i].InnerXml;

        //                            convertedNode.Attributes.RemoveNamedItem("abnormal");

        //                            boxElem.AppendChild(convertedNode);

        //                            paraToConvert[i].ParentNode.InsertBefore(boxElem, paraToConvert[i]);
        //                            objGlobal.SaveXml();
        //                            prevPage = page;
        //                        }
        //                        else
        //                        {
        //                            XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("upara");

        //                            convertedNode.InnerXml = paraToConvert[i].InnerXml;

        //                            convertedNode.Attributes.RemoveNamedItem("abnormalLeft");
        //                            convertedNode.Attributes.RemoveNamedItem("abnormalRight");

        //                            boxElem.AppendChild(convertedNode);

        //                            paraToConvert[i].ParentNode.InsertBefore(boxElem, paraToConvert[i]);
        //                            objGlobal.SaveXml();
        //                        }
        //                    }
        //                }
        //                for (int j = 0; j < paraToConvert.Count; j++)
        //                {
        //                    paraToConvert[j].ParentNode.RemoveChild(paraToConvert[j]);
        //                    objGlobal.SaveXml();
        //                }
        //            }
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

        private void convertToBox(bool isApplyAll)
        {
            try
            {
                LoadPdfXml();

                string normalBgColor = GetPdfNormalBgColor();

                var boxBgColor = GetBoxBgColor(normalBgColor);

                if (BoxBgColors == null) BoxBgColors = new List<string>();

                BoxBgColors.Add(boxBgColor);

                List<XmlNode> paraToConvert = new List<XmlNode>();
                bool isBoxPara = false;
                int page = 0;

                List<int> boxContainingPages = GetBoxContainingPage();
                if (boxContainingPages == null || boxContainingPages.Count == 0) return;

                var allParas = objGlobal.PBPDocument.SelectNodes("//upara");

                foreach (int pageNum in boxContainingPages)
                {
                    var boxParaText = GetBoxText(pageNum);

                    if (boxParaText != null || boxParaText.Count > 0)
                    {
                        foreach (XmlNode abnormalNode in allParas)
                        {
                            isBoxPara = false;

                            if (!abnormalNode.ParentNode.Name.Equals("box") && abnormalNode.ChildNodes[0].Attributes["page"].Value == Convert.ToString(pageNum))
                            {
                                if (abnormalNode.ChildNodes.Count > 0)
                                {
                                    if (boxContainingPages.Contains(pageNum))
                                    {
                                        foreach (SvgBox boxLine in boxParaText)
                                        {
                                            List<string> boxLines = boxLine.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

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

                                        if (isBoxPara) paraToConvert.Add(abnormalNode);
                                    }
                                }
                            }
                        }

                        if (paraToConvert != null && paraToConvert.Count > 0) break;
                    }
                }

                if (paraToConvert != null && paraToConvert.Count > 0)
                {
                    int prevPage = 0;
                    XmlNode boxElem = null;

                    for (int i = 0; i < paraToConvert.Count; i++)
                    {
                        page = paraToConvert[i].ChildNodes.Count > 0 ? Convert.ToInt32(paraToConvert[i].ChildNodes[0].Attributes["page"].Value) : 0;

                        if (boxContainingPages.Contains(page))
                        {
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

                                XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("upara");

                                convertedNode.InnerXml = paraToConvert[i].InnerXml;

                                convertedNode.Attributes.RemoveNamedItem("abnormal");

                                boxElem.AppendChild(convertedNode);

                                paraToConvert[i].ParentNode.InsertBefore(boxElem, paraToConvert[i]);
                                objGlobal.SaveXml();
                                prevPage = page;
                            }
                            else
                            {
                                XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("upara");

                                convertedNode.InnerXml = paraToConvert[i].InnerXml;

                                convertedNode.Attributes.RemoveNamedItem("abnormalLeft");
                                convertedNode.Attributes.RemoveNamedItem("abnormalRight");

                                boxElem.AppendChild(convertedNode);

                                paraToConvert[i].ParentNode.InsertBefore(boxElem, paraToConvert[i]);
                                objGlobal.SaveXml();
                            }
                        }
                    }
                    for (int j = 0; j < paraToConvert.Count; j++)
                    {
                        paraToConvert[j].ParentNode.RemoveChild(paraToConvert[j]);
                        objGlobal.SaveXml();
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

        //private void convertToBox(bool isApplyAll)
        //{
        //    try
        //    {
        //        LoadPdfXml();

        //        string normalBgColor = GetPdfNormalBgColor();

        //        var boxBgColor = GetBoxBgColor(normalBgColor);

        //        if (BoxBgColors == null) BoxBgColors = new List<string>();

        //        BoxBgColors.Add(boxBgColor);

        //        List<XmlNode> paraToConvert = new List<XmlNode>();
        //        int page = 0;
        //        bool isBoxPara = false;

        //        List<int> boxContainingPages = GetBoxContainingPage();
        //        if (boxContainingPages == null || boxContainingPages.Count == 0) return;

        //        var abnormalAllParas = objGlobal.PBPDocument.SelectNodes("//upara");

        //        //if (abnormalAllParas[0].ChildNodes != null && abnormalAllParas[0].ChildNodes.Count > 0 &&
        //        //    abnormalAllParas[0].ChildNodes[0].Attributes["page"] != null)
        //        //{

        //        foreach (XmlNode abnormalNode in abnormalAllParas)
        //        {
        //            if (abnormalNode.ParentNode.Name.Equals("box")) isBoxPara = true;

        //            if (isBoxPara && !abnormalNode.ParentNode.Name.Equals("box") &&
        //                abnormalNode.ChildNodes != null &&
        //                abnormalNode.ChildNodes.Count > 0 &&
        //                abnormalNode.ChildNodes[0].Attributes["page"] != null)
        //            {
        //                page = Convert.ToInt32(abnormalNode.ChildNodes[0].Attributes["page"].Value);
        //                break;
        //            }
        //        }

        //        if (page == 0 && abnormalAllParas[0].ChildNodes != null && abnormalAllParas[0].ChildNodes.Count > 0 &&
        //        abnormalAllParas[0].ChildNodes[0].Attributes["page"] != null)
        //        {
        //            page = Convert.ToInt32(abnormalAllParas[0].ChildNodes[0].Attributes["page"].Value);
        //        }

        //        var notConvToBoxPara = abnormalAllParas.Cast<XmlNode>().Where(x => x.ParentNode != null && !x.ParentNode.Name.Equals("box")).ToList();

        //        if (notConvToBoxPara.Count > 0)
        //        {
        //            //page = Convert.ToInt32(notConvToBoxPara[0].ChildNodes[0].Attributes["page"].Value);

        //            var boxParaText = GetBoxText(page);

        //            if (boxParaText == null || boxParaText.Count == 0) return;

        //            foreach (XmlNode abnormalNode in notConvToBoxPara)
        //            {
        //                isBoxPara = false;

        //                if (!abnormalNode.ParentNode.Name.Equals("box") && abnormalNode.ChildNodes[0].Attributes["page"].Value == Convert.ToString(page))
        //                {
        //                    if (abnormalNode.ChildNodes.Count > 0)
        //                    {
        //                        if (boxContainingPages.Contains(page))
        //                        {
        //                            foreach (SvgBox boxLine in boxParaText)
        //                            {
        //                                List<string> boxLines = boxLine.Text.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None).ToList();

        //                                if (boxLines.Count > 0)
        //                                {
        //                                    for (int i = 0; i < boxLines.Count; i++)
        //                                    {
        //                                        if (IsBoxStartingLine(boxLines[i].Trim(), abnormalNode))
        //                                        {
        //                                            isBoxPara = true;
        //                                            break;
        //                                        }
        //                                    }
        //                                }
        //                            }

        //                            if (isBoxPara)
        //                            {
        //                                paraToConvert.Add(abnormalNode);
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        //foreach (XmlNode abnormalNode in abnormalAllParas)
        //        //{
        //        //    isBoxPara = false;

        //        //    if (!abnormalNode.ParentNode.Name.Equals("box"))
        //        //    {
        //        //        if (abnormalNode.ChildNodes.Count > 0)
        //        //        {
        //        //            page = Convert.ToInt32(abnormalNode.ChildNodes[0].Attributes["page"].Value);

        //        //            if (boxContainingPages.Contains(page))
        //        //            {
        //        //                var boxParaText = GetBoxText(page);

        //        //                if (boxParaText != null && boxParaText.Count > 0)
        //        //                {
        //        //                    foreach (SvgBox boxLine in boxParaText)
        //        //                    {
        //        //                        List<string> boxLines =
        //        //                            boxLine.Text.Split(new string[] {"\r\n", "\n"}, StringSplitOptions.None)
        //        //                                .ToList();

        //        //                        if (boxLines.Count > 0)
        //        //                        {
        //        //                            for (int i = 0; i < boxLines.Count; i++)
        //        //                            {
        //        //                                if (IsBoxStartingLine(boxLines[i].Trim(), abnormalNode))
        //        //                                {
        //        //                                    isBoxPara = true;
        //        //                                    break;
        //        //                                }
        //        //                            }
        //        //                        }
        //        //                    }
        //        //                }

        //        //                if (isBoxPara)
        //        //                {
        //        //                    paraToConvert.Add(abnormalNode);
        //        //                }
        //        //            }
        //        //        }
        //        //    }
        //        //}

        //        if (paraToConvert != null)
        //        {
        //            int prevPage = 0;

        //            if (paraToConvert.Count > 0)
        //            {
        //                XmlNode boxElem = null;

        //                for (int i = 0; i < paraToConvert.Count; i++)
        //                {
        //                    page = paraToConvert[i].ChildNodes.Count > 0 ? Convert.ToInt32(paraToConvert[i].ChildNodes[0].Attributes["page"].Value) : 0;

        //                    if (boxContainingPages.Contains(page))
        //                    {
        //                        var boxParaText = GetBoxText(page);
        //                        //To do (match by box para text)

        //                        var result = GetBoxTextByMatching(boxParaText, paraToConvert);

        //                        if (page != prevPage)
        //                        {
        //                            boxElem = objGlobal.PBPDocument.CreateElement("box");

        //                            ((XmlElement)boxElem).SetAttribute("id", "0");
        //                            ((XmlElement)boxElem).SetAttribute("bgcolor", "gray");
        //                            ((XmlElement)boxElem).SetAttribute("border", "off");

        //                            XmlNode boxTitleElem = objGlobal.PBPDocument.CreateElement("box-title");
        //                            boxElem.PrependChild(boxTitleElem);

        //                            XmlElement ln = objGlobal.PBPDocument.CreateElement("ln");

        //                            if (paraToConvert.Count > 1 &&
        //                                paraToConvert[0].ChildNodes.Count > 0 &&
        //                                paraToConvert[1].ChildNodes.Count > 0 &&
        //                                paraToConvert[0].ChildNodes[0].Attributes["fontsize"] != null &&
        //                                paraToConvert[1].ChildNodes[0].Attributes["fontsize"] != null &&
        //                                Convert.ToDouble(paraToConvert[0].ChildNodes[0].Attributes["fontsize"].Value) >
        //                                Convert.ToDouble(paraToConvert[1].ChildNodes[0].Attributes["fontsize"].Value))
        //                            {
        //                                ln.SetAttribute("coord", paraToConvert[0].ChildNodes[0].Attributes["coord"].Value);
        //                                ln.SetAttribute("page", Convert.ToString(page));
        //                                ln.SetAttribute("height", paraToConvert[0].ChildNodes[0].Attributes["height"].Value);
        //                                ln.SetAttribute("left", paraToConvert[0].ChildNodes[0].Attributes["left"].Value);
        //                                ln.SetAttribute("top", paraToConvert[0].ChildNodes[0].Attributes["top"].Value);
        //                                ln.SetAttribute("font", paraToConvert[0].ChildNodes[0].Attributes["font"].Value);
        //                                ln.SetAttribute("fontsize",
        //                                    paraToConvert[0].ChildNodes[0].Attributes["fontsize"].Value);
        //                                ln.SetAttribute("error", paraToConvert[0].ChildNodes[0].Attributes["error"].Value);
        //                                ln.SetAttribute("ispreviewpassed",
        //                                    paraToConvert[0].ChildNodes[0].Attributes["ispreviewpassed"].Value);
        //                                ln.SetAttribute("isUserSigned",
        //                                    paraToConvert[0].ChildNodes[0].Attributes["isUserSigned"].Value);
        //                                ln.SetAttribute("isEditted",
        //                                    paraToConvert[0].ChildNodes[0].Attributes["isEditted"].Value);
        //                                ln.InnerText = paraToConvert[0].ChildNodes[0].InnerText;
        //                            }
        //                            else
        //                            {
        //                                ln.SetAttribute("coord", "0:0:0:0");
        //                                ln.SetAttribute("page", Convert.ToString(page));
        //                                ln.SetAttribute("height", "0");
        //                                ln.SetAttribute("left", "0");
        //                                ln.SetAttribute("top", "0");
        //                                ln.SetAttribute("font", "Arial");
        //                                ln.SetAttribute("fontsize", "12");
        //                                ln.SetAttribute("error", "0");
        //                                ln.SetAttribute("ispreviewpassed", "true");
        //                                ln.SetAttribute("isUserSigned", "1");
        //                                ln.SetAttribute("isEditted", "true");
        //                            }

        //                            boxTitleElem.AppendChild(ln);

        //                            XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("upara");

        //                            convertedNode.InnerXml = paraToConvert[i].InnerXml;

        //                            convertedNode.Attributes.RemoveNamedItem("abnormal");

        //                            boxElem.AppendChild(convertedNode);

        //                            paraToConvert[i].ParentNode.InsertBefore(boxElem, paraToConvert[i]);
        //                            objGlobal.SaveXml();
        //                            prevPage = page;
        //                        }
        //                        else
        //                        {
        //                            XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("upara");

        //                            convertedNode.InnerXml = paraToConvert[i].InnerXml;

        //                            convertedNode.Attributes.RemoveNamedItem("abnormalLeft");
        //                            convertedNode.Attributes.RemoveNamedItem("abnormalRight");

        //                            boxElem.AppendChild(convertedNode);

        //                            paraToConvert[i].ParentNode.InsertBefore(boxElem, paraToConvert[i]);
        //                            objGlobal.SaveXml();
        //                        }
        //                    }
        //                }
        //                for (int j = 0; j < paraToConvert.Count; j++)
        //                {
        //                    paraToConvert[j].ParentNode.RemoveChild(paraToConvert[j]);
        //                    objGlobal.SaveXml();
        //                }
        //            }
        //        }
        //        else
        //        {
        //            divParaText.InnerText = "";
        //        }
        //    }
        //    catch (Exception)
        //    {

        //        throw;
        //    }
        //}

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

        #region FootNotes

        private void showFootNoteParas()
        {
            string tempXmlPath = GetTempXmlPath();

            if (string.IsNullOrEmpty(tempXmlPath) || !File.Exists(tempXmlPath)) return;

            try
            {
                XmlDocument tetmlXmlDoc = new XmlDocument();
                tetmlXmlDoc.Load(tempXmlPath);

                XmlNode supScriptLine = tetmlXmlDoc.SelectSingleNode("//*[Word[@wordtype=\"supScript\"]/..][1]");

                StringBuilder lineText = new StringBuilder();
                string supScriptId = "";

                if (supScriptLine != null)
                {
                    lineText.Append("</br>");

                    XmlNodeList superScriptWrd = supScriptLine.SelectNodes("descendant::Word");

                    if (superScriptWrd != null && superScriptWrd.Count > 0)
                    {
                        foreach (XmlNode word in superScriptWrd)
                        {
                            if (word.Attributes["wordtype"] != null && word.Attributes["id"] != null &&
                                word.InnerText.Trim().Length == 1)
                            {
                                lineText.Append("<font color=green><sup>" + word.InnerText.Trim() + "</sup></font>");
                                supScriptId = word.Attributes["id"].Value;
                            }
                            else
                            {
                                lineText.Append(word.InnerText.Trim() + " ");
                            }
                        }
                    }

                    lineText.Append("</br></br>");


                    XmlNodeList footNoteLines =
                        tetmlXmlDoc.SelectNodes("//*[@linetype='footnote' and @id='" + supScriptId + "']");

                    if (footNoteLines != null && !string.IsNullOrEmpty(supScriptId))
                    {
                        foreach (XmlNode ln in footNoteLines)
                        {
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

                    divParaText.InnerHtml = "<font style='color:#4682b4'><sup>" + Convert.ToString(lineText) + "</sup></font>";
                    //divParaText.InnerHtml = Convert.ToString(lineText);

                    lineText.Length = 0;
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

        //private void convertToFootNote(bool isApplyAll)
        //{
        //    try
        //    {
        //        if (isApplyAll)
        //        {
        //            convertToFootNoteAll();
        //        }
        //        else
        //        {
        //            double fontSize = 0;
        //            string supScriptId = string.Empty;
        //            XmlNodeList footNoteLines = null;

        //            string supScriptLineText = string.Empty;
        //            string footNoteLinesText = string.Empty;

        //            LoadPdfXml();

        //            XmlNodeList pageLines = objGlobal.PBPDocument.SelectNodes("//ln");

        //            string tempXmlPath = GetTempXmlPath();

        //            if (string.IsNullOrEmpty(tempXmlPath) || !File.Exists(tempXmlPath)) return;

        //            XmlDocument tetmlXmlDoc = new XmlDocument();
        //            tetmlXmlDoc.Load(tempXmlPath);

        //            XmlNode firstSupScriptLine = tetmlXmlDoc.SelectSingleNode("//*[Word[@wordtype=\"supScript\"]/..][1]");

        //            if (firstSupScriptLine != null)
        //            {
        //                XmlNode supScriptWord = GetSupScriptWordsFromLine(firstSupScriptLine);

        //                if (supScriptWord != null)
        //                {
        //                    fontSize = Convert.ToDouble(supScriptWord.Attributes["fontsize"].Value);

        //                    if (fontSize > 0)
        //                    {
        //                        XmlNodeList sameFontSizeSupScrptLines = tetmlXmlDoc.SelectNodes("//*[Word[@wordtype='supScript' and @fontsize='" +
        //                                                                            fontSize + "']/..]");

        //                        if (sameFontSizeSupScrptLines != null && sameFontSizeSupScrptLines.Count > 0)
        //                        {
        //                            foreach (XmlNode supScrptLine in sameFontSizeSupScrptLines)
        //                            {
        //                                supScriptWord = GetSupScriptWordsFromLine(supScrptLine);
        //                                supScriptId = supScriptWord.Attributes["id"].Value;

        //                                if (!string.IsNullOrEmpty(supScriptId))
        //                                {
        //                                    footNoteLines = tetmlXmlDoc.SelectNodes("//*[@linetype='footnote' and @id='" + supScriptId + "']");

        //                                    if (footNoteLines != null && footNoteLines.Count > 0)
        //                                    {
        //                                        footNoteLinesText = GetFootNoteLinesText(footNoteLines);
        //                                        supScriptLineText = SetFNoteTextInWordNode(supScrptLine, footNoteLinesText);

        //                                        //Removing bottom footnote lines from main xml
        //                                        foreach (XmlNode fNoteLine in footNoteLines)
        //                                        {
        //                                            List<XmlNode> mainXmlLine = pageLines.Cast<XmlNode>().Where(x => (x.Attributes["top"] != null &&
        //                                                                                      fNoteLine.Attributes["y1"] != null) &&
        //                                                                                     Convert.ToDouble(x.Attributes["top"].Value).Equals
        //                                                                                    (Convert.ToDouble(fNoteLine.Attributes["y1"].Value))).ToList();

        //                                            if (mainXmlLine != null && mainXmlLine.Count > 0)
        //                                            {
        //                                                foreach (XmlNode fNLine in mainXmlLine)
        //                                                {
        //                                                    fNLine.ParentNode.RemoveChild(fNLine);
        //                                                }
        //                                            }
        //                                        }
        //                                        //end


        //                                        //Setting footnote tag in subScrtipt line 
        //                                        List<XmlNode> mainXml = pageLines.Cast<XmlNode>().Where(x => (x.Attributes["top"] != null &&
        //                                                                        supScrptLine.Attributes["y1"] != null) &&
        //                                                                                 Convert.ToDouble(x.Attributes["top"].Value).Equals
        //                                                                                (Convert.ToDouble(supScrptLine.Attributes["y1"].Value))).ToList();

        //                                        if (mainXml != null && mainXml.Count > 0)
        //                                        {
        //                                            foreach (XmlNode supScrtLine in mainXml)
        //                                            {
        //                                                supScrtLine.InnerXml = supScriptLineText;
        //                                            }
        //                                        }
        //                                        //end

        //                                        ((XmlElement)supScriptWord).RemoveAttribute("wordtype");
        //                                        ((XmlElement)supScriptWord).RemoveAttribute("id");

        //                                        tetmlXmlDoc.Save(tempXmlPath);
        //                                        objGlobal.SaveXml();
        //                                    }
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            } //end supScriptLine
        //            else
        //            {
        //                divParaText.InnerText = "";
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //}

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
                    double fontSize = 0;
                    string supScriptId = string.Empty;
                    XmlNodeList footNoteLines = null;

                    string supScriptLineText = string.Empty;
                    string footNoteLinesText = string.Empty;

                    LoadPdfXml();

                    XmlNodeList pageLines = objGlobal.PBPDocument.SelectNodes("//ln");

                    string tempXmlPath = GetTempXmlPath();

                    if (string.IsNullOrEmpty(tempXmlPath) || !File.Exists(tempXmlPath)) return;

                    XmlDocument tetmlXmlDoc = new XmlDocument();
                    tetmlXmlDoc.Load(tempXmlPath);

                    XmlNode firstSupScriptLine = tetmlXmlDoc.SelectSingleNode("//*[Word[@wordtype=\"supScript\"]/..][1]");

                    if (firstSupScriptLine != null)
                    {
                        XmlNode supScriptWord = GetSupScriptWordNode(firstSupScriptLine);

                        if (supScriptWord != null)
                        {
                            fontSize = Convert.ToDouble(supScriptWord.Attributes["fontsize"].Value);

                            if (fontSize > 0)
                            {
                                //Get all same font size superscript and convert to footnote
                                XmlNodeList sameFontSizeSupScrptLines = tetmlXmlDoc.SelectNodes("//*[Word[@wordtype='supScript' and @fontsize='" +
                                                                                    fontSize + "']/..]");

                                if (sameFontSizeSupScrptLines != null && sameFontSizeSupScrptLines.Count > 0)
                                {
                                    foreach (XmlNode supScrptLine in sameFontSizeSupScrptLines)
                                    {
                                        supScriptWord = GetSupScriptWordNode(supScrptLine);
                                        supScriptId = supScriptWord.Attributes["id"].Value;

                                        if (!string.IsNullOrEmpty(supScriptId))
                                        {
                                            footNoteLines = tetmlXmlDoc.SelectNodes("//*[@linetype='footnote' and @id='" + supScriptId + "']");

                                            if (footNoteLines != null && footNoteLines.Count > 0)
                                            {
                                                footNoteLinesText = GetFootNoteLinesText(footNoteLines);
                                                supScriptLineText = GetSupScriptWordNodeText(supScrptLine, footNoteLinesText);

                                                //Removing bottom footnote lines from main xml
                                                foreach (XmlNode fNoteLine in footNoteLines)
                                                {
                                                    List<XmlNode> mainXmlLine = pageLines.Cast<XmlNode>().Where(x => (x.Attributes["top"] != null &&
                                                                                              fNoteLine.Attributes["y1"] != null) &&
                                                                                             Convert.ToDouble(x.Attributes["top"].Value).Equals
                                                                                            (Convert.ToDouble(fNoteLine.Attributes["y1"].Value))).ToList();

                                                    if (mainXmlLine != null && mainXmlLine.Count > 0)
                                                    {
                                                        foreach (XmlNode fNLine in mainXmlLine)
                                                        {
                                                            fNLine.ParentNode.RemoveChild(fNLine);
                                                        }
                                                    }
                                                }
                                                //end


                                                //Setting footnote tag in subScrtipt line 
                                                List<XmlNode> mainXml = pageLines.Cast<XmlNode>().Where(x => (x.Attributes["top"] != null &&
                                                                                supScrptLine.Attributes["y1"] != null) &&
                                                                                         Convert.ToDouble(x.Attributes["top"].Value).Equals
                                                                                        (Convert.ToDouble(supScrptLine.Attributes["y1"].Value))).ToList();

                                                if (mainXml != null && mainXml.Count > 0)
                                                {
                                                    foreach (XmlNode supScrtLine in mainXml)
                                                    {
                                                        supScrtLine.InnerXml = supScriptLineText;
                                                    }

                                                    //Removing tage from Temp xml
                                                    ((XmlElement)supScriptWord).RemoveAttribute("wordtype");
                                                    ((XmlElement)supScriptWord).RemoveAttribute("id");
                                                    tetmlXmlDoc.Save(tempXmlPath);
                                                }
                                                //end

                                                //((XmlElement)supScriptWord).RemoveAttribute("wordtype");
                                                //((XmlElement)supScriptWord).RemoveAttribute("id");

                                                //tetmlXmlDoc.Save(tempXmlPath);
                                                objGlobal.SaveXml();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    } //end supScriptLine
                    else
                    {
                        divParaText.InnerText = "";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void convertToFootNoteAll()
        {
            try
            {
                string supScriptId = string.Empty;
                XmlNodeList footNoteLines = null;

                string supScriptLineText = string.Empty;
                string footNoteLinesText = string.Empty;

                LoadPdfXml();

                XmlNodeList pageLines = objGlobal.PBPDocument.SelectNodes("//ln");

                string tempXmlPath = GetTempXmlPath();

                if (string.IsNullOrEmpty(tempXmlPath) || !File.Exists(tempXmlPath)) return;

                XmlDocument tetmlXmlDoc = new XmlDocument();
                tetmlXmlDoc.Load(tempXmlPath);

                XmlNode supScriptWord = null;

                XmlNode supScriptAllLines = tetmlXmlDoc.SelectSingleNode(" //*[Word[@wordtype='supScript']/..]");

                if (supScriptAllLines != null)
                {
                    foreach (XmlNode supScrptLine in supScriptAllLines)
                    {
                        supScriptWord = GetSupScriptWordNode(supScrptLine);

                        if (supScriptWord != null)
                        {
                            supScriptId = supScriptWord.Attributes["id"].Value;

                            if (!string.IsNullOrEmpty(supScriptId))
                            {
                                footNoteLines =
                                    tetmlXmlDoc.SelectNodes("//*[@linetype='footnote' and @id='" + supScriptId + "']");

                                if (footNoteLines != null && footNoteLines.Count > 0)
                                {
                                    footNoteLinesText = GetFootNoteLinesText(footNoteLines);
                                    supScriptLineText = GetSupScriptWordNodeText(supScrptLine, footNoteLinesText);

                                    foreach (XmlNode fNoteLine in footNoteLines)
                                    {
                                        List<XmlNode> mainXmlLine =
                                            pageLines.Cast<XmlNode>().Where(x => (x.Attributes["top"] != null &&
                                                                                  fNoteLine.Attributes["y1"] != null) &&
                                                                                 Convert.ToDouble(x.Attributes["top"].Value)
                                                                                     .Equals
                                                                                     (Convert.ToDouble(
                                                                                         fNoteLine.Attributes["y1"].Value)))
                                                .ToList();

                                        if (mainXmlLine != null && mainXmlLine.Count > 0)
                                        {
                                            foreach (XmlNode fNLine in mainXmlLine)
                                            {
                                                fNLine.ParentNode.RemoveChild(fNLine);
                                            }
                                        }
                                    }

                                    List<XmlNode> mainXml =
                                        pageLines.Cast<XmlNode>().Where(x => (x.Attributes["top"] != null &&
                                                                              supScrptLine.Attributes["y1"] != null) &&
                                                                             Convert.ToDouble(x.Attributes["top"].Value)
                                                                                 .Equals
                                                                                 (Convert.ToDouble(
                                                                                     supScrptLine.Attributes["y1"].Value)))
                                            .ToList();

                                    if (mainXml != null && mainXml.Count > 0)
                                    {
                                        foreach (XmlNode supScrtLine in mainXml)
                                        {
                                            supScrtLine.InnerXml = Convert.ToString(supScriptLineText);
                                        }
                                    }

                                    ((XmlElement)supScriptWord).RemoveAttribute("wordtype");
                                    ((XmlElement)supScriptWord).RemoveAttribute("id");

                                    tetmlXmlDoc.Save(tempXmlPath);
                                    objGlobal.SaveXml();
                                }
                            }
                        }
                    }
                }//end supScriptLine
                else
                {
                    divParaText.InnerText = "";
                }
            }
            catch (Exception ex)
            {
            }
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
                        if (word.Attributes["wordtype"] != null && word.Attributes["id"] != null && word.InnerText.Trim().Length == 1) return word;
                    }
                }
            }
            return null;
        }

        public string GetSupScriptWordNodeText(XmlNode supScrptLine, string footNoteLinesText)
        {
            StringBuilder supScriptLineText = null;

            if (supScrptLine != null)
            {
                supScriptLineText = new StringBuilder();
                XmlNodeList supScriptWord = supScrptLine.SelectNodes("descendant::Word");

                if (supScriptWord != null && supScriptWord.Count > 0)
                {
                    foreach (XmlNode word in supScriptWord)
                    {
                        if (word.Attributes["wordtype"] != null && word.Attributes["id"] != null &&
                            word.InnerText.Trim().Length == 1)
                        {
                            supScriptLineText.Append("<footnote id='1'>" + footNoteLinesText + "</footnote>");
                        }
                        else
                        {
                            supScriptLineText.Append(word.InnerText.Trim() + " ");
                        }
                    }
                }

                return Convert.ToString(supScriptLineText);
            }
            return null;
        }

        #endregion

        #region endNote

        private void convertToEndNote(bool isApplyAll)
        {
            try
            {
                if (isApplyAll)
                {
                    convertToFootNoteAll();
                }
                else
                {
                    double fontSize = 0;
                    string supScriptId = string.Empty;
                    XmlNodeList footNoteLines = null;

                    string supScriptLineText = string.Empty;
                    string footNoteLinesText = string.Empty;

                    LoadPdfXml();

                    XmlNodeList pageLines = objGlobal.PBPDocument.SelectNodes("//ln");

                    string tempXmlPath = GetTempXmlPath();

                    if (string.IsNullOrEmpty(tempXmlPath) || !File.Exists(tempXmlPath)) return;

                    XmlDocument tetmlXmlDoc = new XmlDocument();
                    tetmlXmlDoc.Load(tempXmlPath);

                    XmlNode firstSupScriptLine = tetmlXmlDoc.SelectSingleNode("//*[Word[@wordtype=\"supScript\"]/..][1]");

                    if (firstSupScriptLine != null)
                    {
                        XmlNode supScriptWord = GetSupScriptWordNode(firstSupScriptLine);

                        if (supScriptWord != null)
                        {
                            fontSize = Convert.ToDouble(supScriptWord.Attributes["fontsize"].Value);

                            if (fontSize > 0)
                            {
                                //Get all same font size superscript and convert to footnote
                                XmlNodeList sameFontSizeSupScrptLines = tetmlXmlDoc.SelectNodes("//*[Word[@wordtype='supScript' and @fontsize='" +
                                                                                    fontSize + "']/..]");

                                if (sameFontSizeSupScrptLines != null && sameFontSizeSupScrptLines.Count > 0)
                                {
                                    foreach (XmlNode supScrptLine in sameFontSizeSupScrptLines)
                                    {
                                        supScriptWord = GetSupScriptWordNode(supScrptLine);
                                        supScriptId = supScriptWord.Attributes["id"].Value;

                                        if (!string.IsNullOrEmpty(supScriptId))
                                        {
                                            footNoteLines = tetmlXmlDoc.SelectNodes("//*[@linetype='footnote' and @id='" + supScriptId + "']");

                                            if (footNoteLines != null && footNoteLines.Count > 0)
                                            {
                                                footNoteLinesText = GetFootNoteLinesText(footNoteLines);
                                                supScriptLineText = GetSupScriptWordNodeText(supScrptLine, footNoteLinesText);

                                                //Removing bottom footnote lines from main xml
                                                foreach (XmlNode fNoteLine in footNoteLines)
                                                {
                                                    List<XmlNode> mainXmlLine = pageLines.Cast<XmlNode>().Where(x => (x.Attributes["top"] != null &&
                                                                                              fNoteLine.Attributes["y1"] != null) &&
                                                                                             Convert.ToDouble(x.Attributes["top"].Value).Equals
                                                                                            (Convert.ToDouble(fNoteLine.Attributes["y1"].Value))).ToList();

                                                    if (mainXmlLine != null && mainXmlLine.Count > 0)
                                                    {
                                                        foreach (XmlNode fNLine in mainXmlLine)
                                                        {
                                                            fNLine.ParentNode.RemoveChild(fNLine);
                                                        }
                                                    }
                                                }
                                                //end


                                                //Setting footnote tag in subScrtipt line 
                                                List<XmlNode> mainXml = pageLines.Cast<XmlNode>().Where(x => (x.Attributes["top"] != null &&
                                                                                supScrptLine.Attributes["y1"] != null) &&
                                                                                         Convert.ToDouble(x.Attributes["top"].Value).Equals
                                                                                        (Convert.ToDouble(supScrptLine.Attributes["y1"].Value))).ToList();

                                                if (mainXml != null && mainXml.Count > 0)
                                                {
                                                    foreach (XmlNode supScrtLine in mainXml)
                                                    {
                                                        supScrtLine.InnerXml = supScriptLineText;
                                                    }

                                                    //Removing tage from Temp xml
                                                    ((XmlElement)supScriptWord).RemoveAttribute("wordtype");
                                                    ((XmlElement)supScriptWord).RemoveAttribute("id");
                                                    tetmlXmlDoc.Save(tempXmlPath);
                                                }
                                                //end

                                                //((XmlElement)supScriptWord).RemoveAttribute("wordtype");
                                                //((XmlElement)supScriptWord).RemoveAttribute("id");

                                                //tetmlXmlDoc.Save(tempXmlPath);
                                                objGlobal.SaveXml();
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    } //end supScriptLine
                    else
                    {
                        divParaText.InnerText = "";
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void showEndNoteParas()
        {
            string tempXmlPath = GetTempXmlPath();

            if (string.IsNullOrEmpty(tempXmlPath) || !File.Exists(tempXmlPath)) return;

            try
            {
                XmlDocument tetmlXmlDoc = new XmlDocument();
                tetmlXmlDoc.Load(tempXmlPath);

                XmlNode supScriptLine = tetmlXmlDoc.SelectSingleNode("//*[Word[@wordtype=\"supScriptEndNote\"]/..][1]");

                //XmlNode supScriptLine = tetmlXmlDoc.SelectSingleNode("//post-section/descendant::section-title/ln[text()]");

                StringBuilder lineText = new StringBuilder();
                string supScriptId = "";

                if (supScriptLine != null)
                {
                    lineText.Append("</br>");

                    XmlNodeList superScriptWrd = supScriptLine.SelectNodes("descendant::Word");

                    if (superScriptWrd != null && superScriptWrd.Count > 0)
                    {
                        foreach (XmlNode word in superScriptWrd)
                        {
                            if (word.Attributes["wordtype"] != null && word.Attributes["id"] != null &&
                                word.InnerText.Trim().Length == 1)
                            {
                                lineText.Append("<font color=green><sup>" + word.InnerText.Trim() + "</sup></font>");
                                supScriptId = word.Attributes["id"].Value;
                            }
                            else
                            {
                                lineText.Append(word.InnerText.Trim() + " ");
                            }
                        }
                    }

                    //lineText.Append("</br></br>");


                    //XmlNodeList footNoteLines =
                    //    tetmlXmlDoc.SelectNodes("//*[@linetype='footnote' and @id='" + supScriptId + "']");

                    //if (footNoteLines != null && !string.IsNullOrEmpty(supScriptId))
                    //{
                    //    foreach (XmlNode ln in footNoteLines)
                    //    {
                    //        XmlNodeList footNoteWord = ln.SelectNodes("descendant::Word");

                    //        if (footNoteWord != null && footNoteWord.Count > 0)
                    //        {
                    //            foreach (XmlNode word in footNoteWord)
                    //            {
                    //                lineText.Append(word.InnerText.Trim() + " ");
                    //            }
                    //        }
                    //    }
                    //}

                    divParaText.InnerHtml = Convert.ToString(lineText);

                    lineText.Length = 0;
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

        protected void btnEndNotePage_Click(object sender, EventArgs e)
        {

        }

        private void populateTreeView()
        {
            if (objGlobal.PBPDocument == null) return;

            //objGlobal.PBPDocument = new XmlDocument();

            //objGlobal.PBPDocument.Load(@"F:\33.xml");

            XmlNodeList postSecLinesList = objGlobal.PBPDocument.SelectNodes("//post-section/descendant::section-title/ln[text()]");

            //XmlNodeList postSecLinesList = objGlobal.PBPDocument.SelectNodes("//post-section/descendant::section[@type='level1']");

            var rootNode = objGlobal.PBPDocument.DocumentElement;

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

        protected void tvChapters_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                string NodeText = tvChapters.SelectedNode.Text;
                NodeText = NodeText.Replace("<div style=\" background:red; font-weight:bold;\">", "")
                                    .Replace("</div>", "").Replace("<font style='background-color:#ffff42'>", "")
                                    .Replace("</font>", "");


                //XmlDocument lnDoc = new XmlDocument();
                //lnDoc.LoadXml("<book>" + tvChapters.SelectedValue + "</book>");
                //lnNode = lnDoc.SelectSingleNode("//ln");

                //nodeVal = lnNode.Attributes["outerxml"] != null ? lnNode.Attributes["outerxml"].Value.Replace("&lt;", "<").Replace("&gt;", ">") : "";
                //parentName = TreeView1.SelectedNode.Parent.Text.Replace("<font style='background-color:#ffff42'>", "").Replace("</font>", "");
                //lnNode = GetTargetXmlNode(nodeVal, parentName);
                //this.innerText = lnNode.InnerText;
                //hfNodeText.Value = lnNode.InnerText;

            }
            catch (Exception ex)
            {
            }
        }
    }
}


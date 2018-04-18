using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Xml;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System.Diagnostics;
using System.Globalization;
using System.Drawing;
using System.Text.RegularExpressions;
using System.Web.Script.Services;


namespace Outsourcing_System
{
    public partial class ComparisonTask : System.Web.UI.Page
    {
        #region |Fields and Properties|

        public string innerText = "";
        int pdfPageCount = 0;

        GlobalVar objGlobal = new GlobalVar();
        MyDBClass objMyDBClass = new MyDBClass();
        ConversionClass objConversionClass = new ConversionClass();

        #endregion


        protected void Page_Load(object sender, EventArgs e)
        {
            ShowErrorMessage("");
            if (Session["PDFPAgeCount"] == null)
            {
                DecideFolderPath("");
                string tetmlFilePath = System.IO.Directory.GetParent(GlobalVar.ProjectFolderPath) + "\\" + Request.QueryString["bid"].ToString() + ".tetml";
                int pC = objGlobal.GetPageCountFromTetml(tetmlFilePath);
                Session["PDFPAgeCount"] = pdfPageCount = pC;

            }
            else
                pdfPageCount = int.Parse(Session["PDFPAgeCount"].ToString());
            this.Page.Title = "Outsourcing System :: Book Preview";
            string xmlFile = "";
            if (Request.QueryString["username"] != null)
            {
                xmlFile = objMyDBClass.MainDirPhyPath + "/Tests/" + Request.QueryString["username"].ToString() + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".rhyw";
            }
            else
            {

                xmlFile = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".rhyw";
            }
            //if (File.Exists(xmlFile))
            //{
            //    try
            //    {
            //        ShowPDF(chkRegenrate.Checked);
            //    }
            //    catch
            //    {

            //    }

            //}
            if (!Page.IsPostBack)
            {
                try
                {
                    DecideFolderPath("");
                    lblTotalPages.Text = pdfPageCount.ToString();
                    if (Session["pno"] == null)
                    {
                        Session["pno"] = "1";
                    }
                    this.txtCurrentPage.Text = Session["pno"].ToString();


                }
                catch (Exception ex)
                {

                }
                lblMistakesCount.Text = "Total Mistakes : 0";
            }
            if (Session["SourcePagePath"] != null)
            {
                PDFViewerSource.FilePath = Session["SourcePagePath"].ToString();
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            XmlNode lineNode = objGlobal.PBPDocument.SelectSingleNode("//ln[@page='" + txtCurrentPage.Text + "' and contains(.,'" + txtSelectedLineHidden.Text + "')]");
            if (lineNode != null)
            {
                lineNode.InnerText = lineNode.InnerText.Replace(txtSelectedWordHidden.Text, txtSelectedText.Text);
                //InsertMistakesInXML(lineNode, objGlobal.PBPDocument, "Mistake", Operations.Editing.ToString());
                objGlobal.SaveXml();
            }
        }
        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            if (btnGenerate.Text == "Spell Check")
            {
                string bookID = Request.QueryString["bid"].ToString();
                Response.Redirect("SpellChecker.aspx?bid=" + bookID + "&TotalPages=" + lblTotalPages.Text, false);
            }
            else if (btnGenerate.Text == "Done")
            {

                Response.Redirect("OnlineTestPage.aspx", false);
            }
            else
            {
                if (Session["pno"] == null)
                {
                    Session["pno"] = txtCurrentPage.Text;
                }

                Session["pno"] = txtCurrentPage.Text.Trim();
                ShowPDF(chkRegenrate.Checked);
            }
        }

        protected void lnkSpit_Click(object sender, EventArgs e)
        {
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            txtSelectedLineHidden.Text = txtSelectedLineHidden.Text.Replace("&nbsp;", " ");
            XmlNode targetNode = objGlobal.PBPDocument.SelectSingleNode("//ln[@page='" + txtCurrentPage.Text + "' and contains(.,'" + txtSelectedLineHidden.Text + "')]");

            string pageno = targetNode.Attributes["page"].Value;
            XmlNode nextSibling = null;
            XmlNode tempNode = null;
            XmlNode parentNode = targetNode.ParentNode;
            XmlNode grandParentNode = parentNode.ParentNode;
            if (grandParentNode.Name.Equals("spara"))
            {

                XmlElement paraElem = targetNode.OwnerDocument.CreateElement(targetNode.ParentNode.Name);
                if (targetNode.ParentNode.NextSibling == null)
                {
                    nextSibling = targetNode.NextSibling != null ? targetNode.NextSibling : null;
                    paraElem.AppendChild(targetNode);
                    while (nextSibling != null)
                    {
                        tempNode = nextSibling.NextSibling;
                        paraElem.AppendChild(nextSibling);
                        nextSibling = tempNode;
                    }
                    XmlElement sParaElem = targetNode.OwnerDocument.CreateElement(grandParentNode.Name);
                    foreach (XmlAttribute att in grandParentNode.Attributes)
                    {
                        sParaElem.SetAttribute(att.Name, att.Value);
                    }
                    sParaElem.AppendChild(paraElem);
                    grandParentNode.ParentNode.InsertAfter(sParaElem, grandParentNode);
                }
                else
                {
                    XmlElement sParaElem = targetNode.OwnerDocument.CreateElement(grandParentNode.Name);
                    nextSibling = targetNode.ParentNode.NextSibling != null ? targetNode.ParentNode.NextSibling : null;
                    sParaElem.AppendChild(targetNode.ParentNode);
                    while (nextSibling != null)
                    {
                        tempNode = nextSibling.NextSibling;
                        sParaElem.AppendChild(nextSibling);
                        nextSibling = tempNode;
                    }

                    foreach (XmlAttribute att in grandParentNode.Attributes)
                    {
                        sParaElem.SetAttribute(att.Name, att.Value);
                    }
                    grandParentNode.ParentNode.InsertAfter(sParaElem, grandParentNode);

                }
            }
            else
            {
                XmlNode currentNode = targetNode.ParentNode;
                XmlElement newElem = targetNode.OwnerDocument.CreateElement(currentNode.Name);

                foreach (XmlAttribute att in targetNode.ParentNode.Attributes)
                {
                    newElem.SetAttribute(att.Name, att.Value);
                    if (att.Name.Equals("conversion-Operations"))
                    {
                        newElem.Attributes["conversion-Operations"].Value = "";
                    }
                }
                if (currentNode.Attributes["conversion-Operations"].Value != "")
                {
                    currentNode.Attributes["conversion-Operations"].Value = currentNode.Attributes["conversion-Operations"].Value + ",splited";
                }
                else
                {
                    currentNode.Attributes["conversion-Operations"].Value = "splited";
                }
                nextSibling = targetNode.NextSibling != null ? targetNode.NextSibling : null;
                newElem.AppendChild(targetNode);
                while (nextSibling != null)
                {
                    tempNode = nextSibling.NextSibling;
                    newElem.AppendChild(nextSibling);
                    nextSibling = tempNode;
                }
                grandParentNode.InsertAfter(newElem, parentNode);
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

        }
        protected void lnkMerge_Click(object sender, EventArgs e)
        {
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            txtSelectedLineHidden.Text = txtSelectedLineHidden.Text.Replace("&nbsp;", " ");

            string[] Lines = txtSelectedLineHidden.Text.Split(new string[] { "<br/>" }, StringSplitOptions.None);
            bool sameTypePara = true;
            string NodeType = "";
            List<XmlNode> paras = new List<XmlNode>();
            for (int i = 0; i < Lines.Length; i++)
            {
                XmlNode lineNode = objGlobal.PBPDocument.SelectSingleNode("//ln[@page='" + txtCurrentPage.Text + "' and contains(.,'" + Lines[i] + "')]");
                if (lineNode != null)
                {
                    if (i == 0)
                    {
                        while (lineNode.PreviousSibling != null)
                        {
                            lineNode = lineNode.PreviousSibling;
                        }
                    }
                    XmlNode paraNode = lineNode.SelectSingleNode("ancestor::upara|ancestor::spara|ancestor::npara");
                    //This is to store unique paras
                    if (lineNode.PreviousSibling == null && (!paras.Contains(paraNode)))
                    {
                        paras.Add(paraNode);
                    }
                    //This is to get first para Type 
                    if (i == 0)
                    {
                        NodeType = paraNode == null ? "" : paraNode.Name;
                    }
                    else
                    {
                        if (!NodeType.Equals(paraNode.Name))
                        {
                            sameTypePara = false;
                        }
                    }

                }
            }
            if (sameTypePara)
            {
                XmlNode mainNode = paras[0] as XmlNode;


                for (int g = 1; g < paras.Count; g++)
                {
                    XmlNodeList ChildNodes = null;
                    XmlNode currNode = (paras[g] as XmlNode);
                ReLoad:
                    ChildNodes = currNode.ChildNodes;
                    foreach (XmlNode tmpNode in ChildNodes)
                    {
                        mainNode.AppendChild(tmpNode);
                        goto ReLoad;
                    }
                    currNode.ParentNode.RemoveChild(currNode);
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
            }
        }
        [System.Web.Services.WebMethod]
        public static string MergeOperation(string CurrentPage, string SelectedLineHidden)
        {
            ComparisonService.ComparisonTasksClient svc = new ComparisonService.ComparisonTasksClient();
            svc.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential("win 7", "pakistan", "58.65.163.243:");
            svc.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            try
            {
                svc.Open();
                string result = "";
                if (HttpContext.Current.Session["XMLPath"] != null)
                {
                    result = svc.MergeOperation(CurrentPage, SelectedLineHidden, HttpContext.Current.Session["XMLPath"].ToString());
                }
                return result;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (svc.State.Equals(System.ServiceModel.CommunicationState.Opened))
                {
                    svc.Close();
                }
            }
        }
        [System.Web.Services.WebMethod]
        public static string SplitOperation(string CurrentPage, string SelectedLineHidden)
        {
            ComparisonService.ComparisonTasksClient svc = new ComparisonService.ComparisonTasksClient();
            svc.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential("win 7", "pakistan", "58.65.163.243:");
            svc.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            try
            {
                svc.Open();
                string result = "";
                if (HttpContext.Current.Session["XMLPath"] != null)
                {
                    result = svc.SplitOperation(CurrentPage, SelectedLineHidden, HttpContext.Current.Session["XMLPath"].ToString());
                }
                return result;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (svc.State.Equals(System.ServiceModel.CommunicationState.Opened))
                {
                    svc.Close();
                }
            }
        }
        [System.Web.Services.WebMethod]        
        public static string AddSectionOperation(string CurrentPage, string SelectedLineHidden,string sectionType,int SectionLevel)
        {
            ComparisonService.ComparisonTasksClient svc = new ComparisonService.ComparisonTasksClient();
            svc.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential("win 7", "pakistan", "58.65.163.243:");
            svc.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            try
            {
                svc.Open();
                string result = "";
                if (HttpContext.Current.Session["XMLPath"] != null)
                {
                    result = svc.AddSection(sectionType, SectionLevel, CurrentPage, SelectedLineHidden, HttpContext.Current.Session["XMLPath"].ToString());
                }
                return result;

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            finally
            {
                if (svc.State.Equals(System.ServiceModel.CommunicationState.Opened))
                {
                    svc.Close();
                }
            }
        }
        [System.Web.Services.WebMethod]
        public static string EditOperation(string CurrentPage, string SelectedLineHidden, string SelectedWordHidden, string UpdatedText)
        {
            ComparisonService.ComparisonTasksClient svc = new ComparisonService.ComparisonTasksClient();
            svc.ClientCredentials.Windows.ClientCredential = new System.Net.NetworkCredential("win 7", "pakistan", "58.65.163.243:");
            svc.ClientCredentials.Windows.AllowedImpersonationLevel = System.Security.Principal.TokenImpersonationLevel.Impersonation;
            try
            {
                svc.Open();
                string result = "";
                if (HttpContext.Current.Session["XMLPath"] != null)
                {
                   result= svc.SaveText(CurrentPage, SelectedLineHidden, SelectedWordHidden, UpdatedText, HttpContext.Current.Session["XMLPath"].ToString());
                }
                return result;

            }
            catch (Exception ex)
            {                
                return ex.Message;
            }
            finally
            {
                if (svc.State.Equals(System.ServiceModel.CommunicationState.Opened))
                {
                    svc.Close();
                }
            }
        }
        [System.Web.Services.WebMethod]
        public static string ConvertParaType(string CurrentPage, string SelectedLineHidden, bool uparatype, bool saparatype, bool nparatype, string SparaOrientation, string SparaBackground, string SparaType, string SparaSubType, bool chkStanza, bool HasNumbers, string HasStartNo, string sign)
        {
            ComparisonService.ComparisonTasksClient svc = new ComparisonService.ComparisonTasksClient();
            try
            {
                svc.Open();                
                string result="";
                if (HttpContext.Current.Session["XMLPath"] != null)
                {
                    result = svc.CovertParas(CurrentPage, SelectedLineHidden, uparatype, saparatype, nparatype, SparaOrientation, SparaBackground, SparaType, SparaSubType, chkStanza, HasNumbers, HasStartNo, sign, HttpContext.Current.Session["XMLPath"].ToString());
                }
                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (svc.State.Equals(System.ServiceModel.CommunicationState.Opened))
                {
                    svc.Close();
                }
            }
        }
        public void btnConvert_Click(object sender, EventArgs e)
        {            
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            txtSelectedLineHidden.Text = txtSelectedLineHidden.Text.Replace("&nbsp;", " ");

            string[] Lines = txtSelectedLineHidden.Text.Split(new string[] { "<br/>" }, StringSplitOptions.None);
            bool sameTypePara = true;
            string NodeType = "";
            List<XmlNode> paras = new List<XmlNode>();
            for (int i = 0; i < Lines.Length; i++)
            {
                XmlNode lineNode = objGlobal.PBPDocument.SelectSingleNode("//ln[@page='" + txtCurrentPage.Text + "' and contains(.,'" + Lines[i] + "')]");
                if (lineNode != null)
                {
                    if (i == 0)
                    {
                        while (lineNode.PreviousSibling != null)
                        {
                            lineNode = lineNode.PreviousSibling;
                        }
                    }
                    XmlNode paraNode = lineNode.SelectSingleNode("ancestor::upara|ancestor::spara|ancestor::npara");
                    //This is to store unique paras
                    if (lineNode.PreviousSibling == null && (!paras.Contains(paraNode)))
                    {
                        paras.Add(paraNode);
                    }
                    //This is to get first para Type 
                    if (i == 0)
                    {
                        NodeType = paraNode == null ? "" : paraNode.Name;
                    }
                    else
                    {
                        if (!NodeType.Equals(paraNode.Name))
                        {
                            sameTypePara = false;
                        }
                    }

                }
            }
            if (sameTypePara)
            {
                for (int i = 0; i < paras.Count; i++)
                {
                    if (rbSpara.Checked)
                    {
                        convertSpara(paras[i]);
                    }
                    else if (rbNpara.Checked)
                    {
                        ConvertNpara(paras[i]);
                    }
                    else if (rbUpara.Checked)
                    {
                        convertUpara(paras[i]);
                    }
                }
            }
            else
            {
                ShowErrorMessage("Selected Paras are not of same type..");
            }
        }

        protected void btnOk_Click(object sender, EventArgs e)
        {
            ShowErrorMessage("");
        }
        protected void btnNext_Click(object sender, EventArgs e)
        {
            if (Session["pno"] != null)
            {

                if (Request.QueryString["username"] != null && ((int.Parse(Session["pno"].ToString()) + 1) >= pdfPageCount))
                {
                    this.btnGenerate.Text = "Done";
                }
                else if ((int.Parse(Session["pno"].ToString()) + 1) >= pdfPageCount)
                {
                    this.btnGenerate.Text = "Spell Check";
                }

                Session["pno"] = (int.Parse(Session["pno"].ToString())) < pdfPageCount ? ((int.Parse(Session["pno"].ToString()) + 1).ToString()) : (pdfPageCount.ToString());
                txtCurrentPage.Text = Session["pno"].ToString();
                ShowPDF(false);

            }
        }
        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            if (Session["pno"] != null)
            {
                Session["pno"] = (int.Parse(Session["pno"].ToString())) > 1 ? ((int.Parse(Session["pno"].ToString()) - 1).ToString()) : ("1");
                txtCurrentPage.Text = Session["pno"].ToString();
                ShowPDF(false);

            }
        }

        protected void lnklevel1_Click(object sender, EventArgs e)
        {
            try
            {
                AddSection("level1", 4);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }

        }
        protected void lnklevel2_Click(object sender, EventArgs e)
        {
            try
            {
                AddSection("level2", 3);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }
        protected void lnklevel3_Click(object sender, EventArgs e)
        {
            try
            {
                AddSection("level3", 2);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        protected void lnklevel4_Click(object sender, EventArgs e)
        {

            try
            {
                AddSection("level4", 1);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        public static void DisablePageCaching()
        {
            //Used for disabling page caching
            HttpContext.Current.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            HttpContext.Current.Response.Cache.SetValidUntilExpires(false);
            HttpContext.Current.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache);
            HttpContext.Current.Response.Cache.SetNoStore();
        }
        private string HighLightMisSpelled(string path, string word)
        {

            //Path to where you want the file to output 
            string[] splited = path.Split('/');
            String FileName = splited[splited.Length - 1];
            string outputFilePath = path.Replace(".pdf", "_Highlighted.pdf");
            string inputFilePath = path;
            //Path to where the pdf you want to modify is

            using (Stream inputPdfStream = new FileStream(inputFilePath, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (Stream outputPdfStream = new FileStream(outputFilePath, FileMode.Create, FileAccess.Write, FileShare.ReadWrite))
            {
                //Opens the unmodified PDF for reading
                PdfReader reader = new PdfReader(inputPdfStream);
                //Creates a stamper to put an image on the original pdf
                reader.GetPageContent(4);
                for (int i = 1; i <= reader.NumberOfPages; i++)
                {
                    string pageText = PdfTextExtractor.GetTextFromPage(reader, i).Replace(" ", "");
                    if (pageText.Contains(word.Replace(" ", "")))
                    {
                        List<string> lstcoordinates = getCoordinates(path, word);
                        var stamper = new PdfStamper(reader, outputPdfStream) { FormFlattening = true, FreeTextFlattening = true };
                        foreach (string coordinates in lstcoordinates)
                        {
                            string[] dimensions = coordinates.Split(' ');
                            float x = 0;
                            float y = 0;
                            float.TryParse(dimensions[0], NumberStyles.Any, CultureInfo.InvariantCulture, out x);
                            float.TryParse(dimensions[1], NumberStyles.Any, CultureInfo.InvariantCulture, out y);
                            iTextSharp.text.Image objImage1 = iTextSharp.text.Image.GetInstance(new Bitmap(Convert.ToInt32(Math.Round(Convert.ToDouble(dimensions[2]))), 1), BaseColor.RED);
                            objImage1.SetAbsolutePosition(x, y - 2);
                            stamper.GetOverContent(i).AddImage(objImage1, true);
                        }
                        stamper.Close();
                    }

                }


            }
            File.Copy(outputFilePath, path, true);
            File.Delete(outputFilePath);
            return path;



        }
        private List<string> getCoordinates(string inputFilePath, string text)
        {
            try
            {
                string tetmlPath = inputFilePath.Replace(".pdf", ".tetml");
                string llx = "";
                string lly = "";
                string urx = "";
                string ury = "";
                string[] lineWords = text.Split(' ');
                string coordinates = "";
                string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=true font=true}} contentanalysis={nopunctuationbreaks} clippingarea={cropbox}\" -o \"" + tetmlPath + "\" \"" + inputFilePath + "\"";
                //string Img_Conversion_bat = @"D:\work\tet.exe";
                string Img_Conversion_bat = @"C:\XSL\tet.exe";
                Process pConvertTetml = new Process();
                pConvertTetml.StartInfo.UseShellExecute = false;
                pConvertTetml.StartInfo.RedirectStandardError = true;
                pConvertTetml.StartInfo.RedirectStandardOutput = true;
                pConvertTetml.StartInfo.CreateNoWindow = true;
                pConvertTetml.StartInfo.Arguments = strParameter;
                pConvertTetml.StartInfo.FileName = Img_Conversion_bat;
                pConvertTetml.Start();
                pConvertTetml.WaitForExit();

                XmlDocument tetDoc = new XmlDocument();
                StreamReader sr = new StreamReader(tetmlPath);
                string xmlText = sr.ReadToEnd();
                sr.Close();
                string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<Document.*?</Document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
                tetDoc.LoadXml(documentXML);
                XmlNodeList pages = tetDoc.SelectNodes("//Page");
                List<string> lstcoordiants = new List<string>();

                foreach (XmlNode page in pages)
                {
                    string pageText = "";
                    XmlNodeList words = page.SelectNodes("//Word");
                    for (int j = 0; j < lineWords.Length; j++)
                    {

                        for (int i = 0; i < words.Count; i++)
                        {
                            XmlDocument temDoc = new XmlDocument();
                            temDoc.LoadXml(words[i].OuterXml);
                            XmlNode txtNode = temDoc.SelectSingleNode("//Text");
                            if (lineWords[j].Equals(txtNode.InnerText.Replace(" ", "")))
                            {
                                temDoc = new XmlDocument();
                                temDoc.LoadXml(words[i + 1].OuterXml);
                                txtNode = temDoc.SelectSingleNode("//Text");
                                if (lineWords[j + 1].Equals(txtNode.InnerText.Replace(" ", "")))
                                {
                                    temDoc = new XmlDocument();
                                    temDoc.LoadXml(words[i + 2].OuterXml);
                                    txtNode = temDoc.SelectSingleNode("//Text");
                                    if (lineWords[j + 2].Equals(txtNode.InnerText.Replace(" ", "")))
                                    {
                                        XmlNode boxNode = words[i].ChildNodes[1];
                                        llx = boxNode.Attributes["llx"].Value;
                                        lly = boxNode.Attributes["lly"].Value;


                                        j = lineWords.Length - 1;

                                        while (words[i].ChildNodes[1].Attributes["lly"].Value.Equals(lly))
                                        {
                                            XmlNode boxlastNode = words[i].ChildNodes[1];
                                            urx = boxlastNode.Attributes["urx"].Value;
                                            ury = boxlastNode.Attributes["ury"].Value;
                                            i++;
                                        }
                                        double width = Convert.ToDouble(urx) - Convert.ToDouble(llx);
                                        coordinates = llx + " " + lly + " " + width;
                                        lstcoordiants.Add(coordinates);
                                        break;
                                        break;


                                    }
                                }
                            }
                        }
                    }
                }
                File.Delete(tetmlPath);
                return lstcoordiants;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void ShowPDF(bool Regenrate)
        {
            try
            {
                string bookID = Request.QueryString["bid"].ToString();
                string status = "";
            Restart:
                string[] Files = Directory.GetFiles(Server.MapPath("PdftoHtml5Scripts"));
                for (int i = 0; i < Files.Length; i++)
                {

                    if (System.IO.Path.GetExtension(Files[i]).ToLower().Equals(".pdf"))
                    {
                        File.Delete(Files[i]);
                        goto Restart;
                    }
                }
                if (Session["pno"] != null)
                {
                    status = GeneratePreview(Session["pno"].ToString(), Regenrate);
                }
                else
                {
                    status = GeneratePreview(txtCurrentPage.Text, Regenrate);
                }
                if (status != "")
                {
                    string FileName = objMyDBClass.MainDirPhyPath + "/" + bookID + "/Comparison/Page" + Session["pno"].ToString() + ".pdf";
                    if (txtCurrentPage.Text != "")
                    {
                        XmlDocument pageXml = new XmlDocument();
                        StreamReader sr = new StreamReader(getPageXmlPath(txtCurrentPage.Text));
                        pageXml.LoadXml(sr.ReadToEnd());
                        sr.Close();
                        string pdfPhysicalPath = System.Configuration.ConfigurationManager.AppSettings["MainDirPhyPath"] + "/" + bookID + "/Comparison/Page" + Session["pno"].ToString() + ".pdf";
                        if (pageXml != null)
                        {
                            XmlNodeList linesNode = pageXml.SelectNodes(@"//ln[@Mistake]");
                            foreach (XmlNode node in linesNode)
                            {
                                node.InnerText = node.InnerText.Replace("\r\n", "");
                                pdfPhysicalPath = HighLightMisSpelled(pdfPhysicalPath, node.InnerText);
                            }
                        }
                    }
                    if (File.Exists(FileName))
                    {
                        File.Copy(FileName, Server.MapPath("PdftoHtml5Scripts") + "/Page" + Session["pno"].ToString() + ".pdf");
                    }
                    FileLoadPath.Value = "PdftoHtml5Scripts/Page" + Session["pno"].ToString() + ".pdf";
                }

                Session["SourcePagePath"] = System.Configuration.ConfigurationManager.AppSettings["MainDirectory"] + "/" + bookID + "/Comparison/Page" + Session["pno"].ToString() + "-1.pdf";

                PDFViewerSource.FilePath = Session["SourcePagePath"].ToString();

                //this.txtCurrentPage.Text = Session["pno"].ToString();
            }
            catch (Exception ex)
            {
                //  lblMessage.Text = ex.Message;
            }

        }
        private void loadLatestXml(string username)
        {

            string xmlFile = "";
            if (username != "")
            {
                xmlFile = objMyDBClass.MainDirPhyPath + "/Tests/" + username + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".rhyw";
            }
            else
            {
                xmlFile = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".rhyw";
            }
            objGlobal.XMLPath = xmlFile;
            Session["XMLPath"] = objGlobal.XMLPath;
            objGlobal.PBPDocument = new System.Xml.XmlDocument();
            objGlobal.LoadXml();
            Session["PBPDocument"] = objGlobal.PBPDocument;
        }

        public string getPageXmlPath(string pageno)
        {
            if (Session["ProjectFolderPath"] != null)
            {
                return Session["ProjectFolderPath"].ToString() + "/Page" + pageno + ".xml";
            }
            else
            {
                return GlobalVar.ProjectFolderPath + "/Page" + pageno + ".xml";
            }
        }
        public void LoadTree(string page)
        {
            loadLatestXml("");
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            XmlDocument xmlDoc = objGlobal.GetPageXmlDoc(page);
            try
            {
                string xmlFile = getPageXmlPath(page);

                if (File.Exists(xmlFile))
                {
                    File.Delete(xmlFile);//Deleting the old xml file
                }
                xmlDoc.Save(xmlFile);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                xmlDoc = null;
            }
        }


        private void InsertMistakesInXML(XmlNode line, XmlDocument xmldoc, string attributeName, string attributeValue)
        {
            bool check = true;
            if (line.Attributes[attributeName] != null)
            {
                var temp = line.Attributes[attributeName].Value.Split(',');
                if (temp[0] != "")
                {
                    foreach (var item in temp)
                    {
                        if (item != "")
                        {
                            if (item == attributeValue.Replace(",", ""))
                            {
                                check = false;
                            }
                        }
                    }

                    if (check)
                        line.Attributes[attributeName].Value = line.Attributes[attributeName].Value + attributeValue;
                }


            }
            else
            {

                XmlAttribute newAttr = xmldoc.CreateAttribute(attributeName);
                newAttr.Value = attributeValue;
                XmlElement lineElement = (XmlElement)line;
                lineElement.SetAttributeNode(newAttr);


            }
        }

        public string GeneratePreview(string page, bool Regenerate)
        {
            LoadTree(page);
            string pdfPageFile;
            string pdfFile;
            if (Session["ProjectFolderPath"] != null)
            {
                pdfPageFile = Session["ProjectFolderPath"].ToString() + "/Page" + page + "-1.pdf";
                pdfFile = Session["ProjectFolderPath"].ToString() + "/Page" + page + ".pdf";
            }
            else
            {
                pdfPageFile = GlobalVar.ProjectFolderPath + "/Page" + page + "-1.pdf";
                pdfFile = GlobalVar.ProjectFolderPath + "/Page" + page + ".pdf";
            }

            try
            {
                if (!File.Exists(pdfPageFile))
                {
                    //ConversionClass.ExtractPages(Server.MapPath("~/" + PdfViewer1.File), pdfPageFile, int.Parse(page), int.Parse(page));
                    string filePath = "";
                    if (Request.QueryString["username"] != null)
                    {
                        filePath = objMyDBClass.MainDirPhyPath + "/Tests/" + Request.QueryString["username"].ToString() + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".pdf";
                    }
                    else
                    {
                        string relPdfPath = Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".pdf";
                        filePath = objMyDBClass.MainDirPhyPath + "/" + relPdfPath;
                    }
                    objConversionClass.ExtractPages(filePath, pdfPageFile, int.Parse(page), int.Parse(page));
                }

                if (!File.Exists(pdfFile))
                {
                    ImageValidator.ImageValidationService imgValidator = new ImageValidator.ImageValidationService();
                    try
                    {
                        pdfFile = imgValidator.GenearatePDFPreview(pdfFile.Replace(".pdf", ".xml"), pdfFile);
                    }
                    finally
                    {
                        imgValidator.Dispose();
                    }
                }
                else if (Regenerate)
                {
                    File.Delete(pdfFile);
                    ImageValidator.ImageValidationService imgValidator = new ImageValidator.ImageValidationService();
                    try
                    {
                        pdfFile = imgValidator.GenearatePDFPreview(pdfFile.Replace(".pdf", ".xml"), pdfFile);
                    }
                    finally
                    {
                        imgValidator.Dispose();
                    }
                }
            }
            catch (Exception)
            {
                pdfFile = "";
            }
            return pdfFile;
        }
        public void DecideFolderPath(string username)
        {
            if (username != "")
            {
                GlobalVar.ProjectFolderPath = objMyDBClass.MainDirPhyPath + "/Tests/" + username + "/"+Request.QueryString["bid"].ToString() +"/Comparison";
                Session["ProjectFolderPath"] = objMyDBClass.MainDirPhyPath + "/Tests/" + username + "/" + Request.QueryString["bid"].ToString() + "/Comparison";
            }
            else
            {
                GlobalVar.ProjectFolderPath = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString() + "/Comparison";
                Session["ProjectFolderPath"] = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString() + "/Comparison";
            }

        }
        private void convertSpara(XmlNode origNodyCopy)
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
                XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("spara");
                string origNodeXml = origNodyCopy.InnerXml;
                origNodeXml = Regex.Replace(origNodeXml, "</?num.*?>", "");
                origNodeXml = Regex.Replace(origNodeXml, "</?para.*?>", "");
                origNodeXml = Regex.Replace(origNodeXml, "</?line.*?>", "");

                //if (origNodyCopy.Attributes["correction"] != null)
                //{
                //    ((XmlElement)convertedNode).SetAttribute("correction", "");
                //}
                ((XmlElement)convertedNode).SetAttribute("h-align", ddlSparaOrientation.SelectedValue);
                ((XmlElement)convertedNode).SetAttribute("bgcolor", ddlSparaBackground.SelectedValue);
                ((XmlElement)convertedNode).SetAttribute("type", ddlSparaType.SelectedValue);
                /// Assigning line or para
                string xmlChild = ddlSparaSubType.SelectedValue;
                string xml = "";
                if (chkStanza.Enabled == true && chkStanza.Checked == true)
                {
                    xml = origNodeXml.Replace("<ln", "<" + xmlChild + "><ln").Replace("</ln>", "</ln></" + xmlChild + ">");
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

                foreach (XmlAttribute attr in origNodyCopy.Attributes)
                {
                    if (attr.Name == "id" | attr.Name == "pnum" | attr.Name == "text-indent" | attr.Name == "padding-bottom" | attr.Name == "conversion-Operations" | attr.Name == "correction")
                        ((XmlElement)convertedNode).SetAttribute(attr.Name, attr.Value);

                }

                #endregion
                origNodyCopy.ParentNode.InsertBefore(convertedNode, origNodyCopy);
                XmlNode temp_origNode = origNodyCopy;
                origNodyCopy.ParentNode.RemoveChild(origNodyCopy);
                objGlobal.SaveXml();

                //InsertConversion_MistakesInXML(convertedNode, temp_origNode);


                //objGlobal.PBPDocument.Save(Convert.ToString(Session["xmlPath"]));

                //StringBuilder type = new StringBuilder();

                //for (int i = 0; i < convertedNode.ChildNodes[0].ChildNodes.Count; i++)
                //{
                //    if (!(ddlSparaType.SelectedValue.Trim().Equals("other")))
                //        type.Append("spara:" + ddlSparaType.SelectedValue.Trim() + ":" + ddlSparaSubType.SelectedValue.Trim() + ",");
                //    else
                //        type.Append("spara:" + ddlSparaType.SelectedValue.Trim() + ":" + ddlSparaSubType.SelectedValue.Trim() + ":" + ddlSparaOrientation.SelectedValue.Trim() + ",");

                //    InsertMistakesInXML(Convert.ToString(Session["xmlPath"]).Replace(".rhyw", ".xml"), convertedNode.ChildNodes[0].ChildNodes[i].Attributes["coord"].Value, convertedNode.ChildNodes[0].ChildNodes[i].Attributes["page"].Value, "conversion",Convert.ToString(type), "");
                //}
            }
            catch (Exception ex)
            {

                //  showMessage(ex.Message);
            }
        }

        private void convertUpara(XmlNode origNodyCopy)
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
                XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("upara");

                //XmlNodeList unnecessecaryChilds = origNodyCopy.SelectNodes("//num | //line | //para");
                string origNodeXml = origNodyCopy.InnerXml;
                origNodeXml = Regex.Replace(origNodeXml, "</?num.*?>", "");
                origNodeXml = Regex.Replace(origNodeXml, "</?para.*?>", "");
                origNodeXml = Regex.Replace(origNodeXml, "</?line.*?>", "");
                convertedNode.InnerXml = origNodeXml;

                foreach (XmlAttribute attr in origNodyCopy.Attributes)
                {
                    if (attr.Name == "id" | attr.Name == "pnum" | attr.Name == "text-indent" | attr.Name == "padding-bottom" | attr.Name == "conversion-Operations" | attr.Name == "correction")
                        ((XmlElement)convertedNode).SetAttribute(attr.Name, attr.Value);
                }

                origNodyCopy.ParentNode.InsertBefore(convertedNode, origNodyCopy);
                XmlNode temp_origNode = origNodyCopy;
                origNodyCopy.ParentNode.RemoveChild(origNodyCopy);
                objGlobal.SaveXml();
                //objGlobal.SaveXml();
                //var aa = this.objGlobal.PBPDocument.OuterXml;

                //var tt = origNodyCopy.ChildNodes[i].ChildNodes[0].Attributes["coord"].Value;

                //origNodyCopy.ChildNodes[0].ChildNodes[0].Attributes["coord"].Value;origNodyCopy.ChildNodes.Count convertedNode.ChildNodes[1].Attributes["coord"].Value

                //for (int i = 0; i < convertedNode.ChildNodes.Count; i++)
                //{
                //InsertMistakesInXML(Convert.ToString(Session["xmlPath"]).Replace(".rhyw", ".xml"), convertedNode.ChildNodes[0].Attributes["coord"].Value, convertedNode.ChildNodes[0].Attributes["page"].Value, "conversion", "upara:::", "");
                //  InsertConversion_MistakesInXML(convertedNode, temp_origNode);
                //}              

                //if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
                //{
                //    objGlobal.XMLPath = Session["XMLPath"].ToString();
                //}
                //if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
                //{
                //    objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                //}

                //ShowPDF();
                //LoadTree(Convert.ToString(Session["pno"]));
                //return convertedNode;
            }
            catch (Exception ex)
            {

                //showMessage(ex.Message);
                //return null;
            }
        }

        private void ConvertNpara(XmlNode origNodyCopy)
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
                XmlNode convertedNode = objGlobal.PBPDocument.CreateElement("npara");
                string origNodeXml = origNodyCopy.InnerXml;
                origNodeXml = Regex.Replace(origNodeXml, "</?num.*?>", "");
                origNodeXml = Regex.Replace(origNodeXml, "</?para.*?>", "");
                origNodeXml = Regex.Replace(origNodeXml, "</?line.*?>", "");

                ///Checking extra npara parameters
                if (chkHasNumbers.Checked)
                {
                    XmlNodeList lnNodes = origNodyCopy.SelectNodes("descendant::ln|descendant::break");
                    int iterat = 0;
                    foreach (XmlNode lnNode in lnNodes)
                    {
                        string xml = lnNode.InnerXml;
                        if (iterat == 0)
                        {
                            ++iterat;
                            string num = xml.Split(' ')[0];
                            int r = 0;
                            string empTag = "";
                            if (Regex.Match(num, "<emphasis").Success)
                            {
                                r = 1;
                                empTag = Regex.Match(xml, "<emphasis.*?>").Value.ToString();
                                xml = Regex.Replace(xml, "<emphasis.*?>", "");
                                num = xml.Split(' ')[0];
                            }
                            if (Regex.Match(num, "</emphasis>").Success)
                            {
                                r = 2;
                                num = Regex.Replace(num, "</emphasis>", "");
                            }

                            string sentence = xml.Substring(xml.IndexOf(' '));
                            if (r == 0 || r == 2)
                            {
                                xml = "<num>" + num + "</num>" + sentence;
                            }
                            else if (r == 1)
                            {
                                xml = "<num>" + num + "</num>" + empTag + sentence;
                            }
                        }
                        lnNode.InnerXml = xml;
                        convertedNode.InnerXml += lnNode.OuterXml;
                    }
                }
                else
                {
                    XmlNodeList lnNodes = origNodyCopy.SelectNodes("descendant::ln|descendant::break");
                    if (ddlHasStartNo.SelectedValue.Equals("1"))
                    {
                        int num = 1;
                        if (Regex.Match(ddlHasStartNo.SelectedValue, "[0-9]+").Success)
                        {
                            num = int.Parse(ddlHasStartNo.SelectedValue);
                        }
                        foreach (XmlNode lnNode in lnNodes)
                        {
                            string xml = lnNode.InnerXml;
                            xml = "<num>" + num++ + ddlSign.SelectedValue + "</num>" + xml;
                            lnNode.InnerXml = xml;
                            convertedNode.InnerXml += lnNode.OuterXml;
                        }
                    }
                    else if (ddlHasStartNo.SelectedValue.Equals("a"))
                    {
                        char num = 'a';
                        foreach (XmlNode lnNode in lnNodes)
                        {
                            string xml = lnNode.InnerXml;
                            xml = "<num>" + num++ + ddlSign.SelectedValue + "</num>" + xml;
                            lnNode.InnerXml = xml;
                            convertedNode.InnerXml += lnNode.OuterXml;
                        }
                    }
                    else if (ddlHasStartNo.SelectedValue.Equals("i"))
                    {
                        string[] romans = { "i", "ii", "iii", "iv", "v", "vi", "vii", "viii", "ix", "x", "xi", "xii", "xiii", "xiv", "xv", "xvi", "xvii", "xviii", "xix", "xxi", "xxii", "xxiii", "xxiv", "xxv", "xxvi", "xxvii", "xxviii", "xxix", "xxx" };
                        for (int i = 0; i < lnNodes.Count; i++)//each (XmlNode lnNode in lnNodes)
                        {
                            XmlNode lnNode = lnNodes[i];
                            string xml = lnNode.InnerXml;
                            xml = "<num>" + romans[i] + ddlSign.SelectedValue + "</num>" + xml;
                            lnNode.InnerXml = xml;
                            convertedNode.InnerXml += lnNode.OuterXml;
                        }
                    }
                }

                foreach (XmlAttribute attr in origNodyCopy.Attributes)
                {
                    if (attr.Name == "id" | attr.Name == "pnum" | attr.Name == "text-indent" | attr.Name == "padding-bottom" | attr.Name == "conversion-Operations" | attr.Name == "correction")
                        ((XmlElement)convertedNode).SetAttribute(attr.Name, attr.Value);

                }

                origNodyCopy.ParentNode.InsertBefore(convertedNode, origNodyCopy);
                XmlNode temp_origNode = origNodyCopy;
                origNodyCopy.ParentNode.RemoveChild(origNodyCopy);
                objGlobal.PBPDocument.Save(Convert.ToString(Session["xmlPath"]));
                //objGlobal.SaveXml();

                //InsertMistakesInXML(Convert.ToString(Session["xmlPath"]).Replace(".rhyw", ".xml"), convertedNode.ChildNodes[0].Attributes["coord"].Value, convertedNode.ChildNodes[0].Attributes["page"].Value, "conversion", "spara:quotation:para:", "");
                //InsertConversion_MistakesInXML(convertedNode, temp_origNode);
            }
            catch (Exception ex)
            {
                //showMessage(ex.Message);
            }
        }
        public void AddSection(string type, int newlevel)
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

            txtSelectedLineHidden.Text = txtSelectedLineHidden.Text.Replace("&nbsp;", " ");
            XmlNode lineNode = objGlobal.PBPDocument.SelectSingleNode("//ln[@page='" + txtCurrentPage.Text + "' and contains(.,'" + txtSelectedLineHidden.Text + "')]");
            XmlNode currentNode = lineNode.SelectSingleNode("ancestor::upara|ancestor::spara|ancestor::npara");
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
                string nodeName = currentNode.Name;

                bool canAdd = true;
                if (currentNode != null && (currentNode.Name == "table" || currentNode.Name == "upara" || currentNode.Name == "upara(cont..)" || currentNode.Name == "spara" || currentNode.Name == "npara" || currentNode.Name == "box" || currentNode.Name == "image" || currentNode.Name == "level1" || currentNode.Name == "level2" || currentNode.Name == "level3" || currentNode.Name == "level4" || currentNode.Name == "chapter" || currentNode.Name == "part" || currentNode.Name == "box" || currentNode.Name == "page" || currentNode.Name == "pre-section" || currentNode.Name == "post-section"))
                {
                    XmlElement upara = objGlobal.PBPDocument.CreateElement("upara");
                    upara.SetAttribute("id", "0");
                    upara.SetAttribute("pnum", "0");

                    XmlElement ln = objGlobal.PBPDocument.CreateElement("ln");
                    ln.SetAttribute("coord", "0:0:0:0");
                    ln.SetAttribute("page", txtCurrentPage.Text);
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
                    XmlNode tempNode = currentNode;
                    if (currentNode.Name == "level1" || currentNode.Name == "level2" || currentNode.Name == "level3" || currentNode.Name == "level4" || currentNode.Name == "chapter" || currentNode.Name == "part" || currentNode.Name == "pre-section" || currentNode.Name == "post-section")
                    {
                        XmlNode bodyNode = tempNode.ParentNode.ParentNode.SelectSingleNode("body");
                        if (bodyNode == null)
                        {
                            XmlElement bod = objGlobal.PBPDocument.CreateElement("body");
                            bod.SetAttribute("id", "0");
                            tempNode.ParentNode.ParentNode.InsertAfter(bod, tempNode.ParentNode);
                        }
                        bodyNode = tempNode.ParentNode.ParentNode.SelectSingleNode("body");
                        bodyNode.PrependChild(upara);
                    }
                    else if (currentNode.Name == "page")
                    {
                        if (tempNode.Name == "level1" || tempNode.Name == "level2" || tempNode.Name == "level3" || tempNode.Name == "level4" || tempNode.Name == "chapter" || tempNode.Name == "part")
                        {
                            lblMessage.Text = "Defficult to Add upara here.";
                            ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "showMessageDiv();", true);
                            canAdd = false;
                        }
                        else
                        {
                            tempNode.ParentNode.InsertBefore(upara, tempNode);
                        }
                    }
                    else
                    {
                        tempNode.ParentNode.InsertAfter(upara, tempNode);
                    }
                    if (canAdd == true)
                    {

                    }
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
                            LoadTree(txtCurrentPage.Text);
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
                                LoadTree(txtCurrentPage.Text);
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
                                        LoadTree(txtCurrentPage.Text);
                                    }
                                    else
                                    {
                                        existingNode.ParentNode.InsertAfter(section, existingNode);
                                        currentNode.ParentNode.RemoveChild(currentNode);
                                        objGlobal.SaveXml();
                                        LoadTree(txtCurrentPage.Text);
                                    }
                                }
                                else
                                {
                                    currentParentNode.ParentNode.InsertAfter(section, currentParentNode);
                                    currentNode.ParentNode.RemoveChild(currentNode);
                                    objGlobal.SaveXml();
                                    LoadTree(txtCurrentPage.Text);
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
                        LoadTree(txtCurrentPage.Text);
                    }
                    else
                    {
                        currentParentNode.ParentNode.ParentNode.InsertAfter(section, currentParentNode.ParentNode);
                        currentNode.ParentNode.RemoveChild(currentNode);
                        objGlobal.SaveXml();
                        LoadTree(txtCurrentPage.Text);
                    }
                }

            }

            else
            {
                ShowErrorMessage("Right Click on Upara to add Section.");
            }

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
        public void ShowErrorMessage(string textMessage)
        {

            divMessageBox.Visible = true;
            lblMessage.Text = textMessage;
            if (textMessage.Equals(""))
            {
                divMessageBox.Visible = false;
            }
        }
    }

}

#region|Previou Logic|

//        #region |Fields and Properties|

//        public string innerText = "";
//        int pdfPageCount = 0;
//        GlobalVar objGlobal = new GlobalVar();
//        MyDBClass objMyDBClass = new MyDBClass();
//        ConversionClass objConversionClass = new ConversionClass();

//        #endregion


//        protected void Page_Load(object sender, EventArgs e)
//        {

//            if (Session["objUser"] == null)
//            {
//                Server.Transfer("Login.aspx");
//            }

//            if (Session["PDFPAgeCount"] == null)
//            {
//                DecideFolderPath("");
//                string tetmlFilePath = System.IO.Directory.GetParent(GlobalVar.ProjectFolderPath) + "\\" + Request.QueryString["bid"].ToString() + ".tetml";
//                int pC = objGlobal.GetPageCountFromTetml(tetmlFilePath);
//                Session["PDFPAgeCount"] = pdfPageCount = pC;
//            }
//            else
//                pdfPageCount = int.Parse(Session["PDFPAgeCount"].ToString());
//            this.Page.Title = "Outsourcing System :: Book Preview";
//            string xmlFile = "";
//            if (Request.QueryString["username"] != null)
//            {
//                xmlFile = objMyDBClass.MainDirPhyPath + "/Tests/" + Request.QueryString["username"].ToString() + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".rhyw";
//            }
//            else
//            {

//                xmlFile = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".rhyw";
//            }
//            if (!Page.IsPostBack)
//            {
//                try
//                {


//                    DecideFolderPath("");
//                    lblTotalPages.Text = pdfPageCount.ToString();
//                    this.txtCurrentPage.Text = Session["pno"] == null ? "1" : Session["pno"].ToString();
//                    if (File.Exists(xmlFile))
//                    {
//                        try
//                        {

//                        }
//                        catch
//                        {
//                        }

//                    }
//                    else
//                    {
//                    }
//                }
//                catch (Exception ex)
//                {

//                }
//            }

//        }
//        protected void btnGenerate_Click(object sender, EventArgs e)
//        {
//            if (btnGenerate.Text == "Spell Check")
//            {
//                string bookID = Request.QueryString["bid"].ToString();
//                Response.Redirect("SpellChecker.aspx?bid=" + bookID + "&TotalPages=" + lblTotalPages.Text, false);
//            }
//            else if (btnGenerate.Text == "Done")
//            {

//                Response.Redirect("OnlineTest.aspx", false);
//            }
//            else
//            {
//                if (Session["pno"] == null)
//                {
//                    Session["pno"] = txtCurrentPage.Text;
//                }

//                Session["pno"] = this.txtCurrentPage.Text.Trim();
//                ShowPDF();
//            }
//        }
//        protected void btnNext_Click(object sender, EventArgs e)
//        {
//            if (Session["pno"] != null)
//            {

//                if (Request.QueryString["username"] != null && ((int.Parse(Session["pno"].ToString()) + 1) >= pdfPageCount))
//                {
//                    this.btnGenerate.Text = "Done";
//                }
//                else if ((int.Parse(Session["pno"].ToString()) + 1) >= pdfPageCount)
//                {
//                    this.btnGenerate.Text = "Spell Check";
//                }

//                Session["pno"] = (int.Parse(Session["pno"].ToString())) < pdfPageCount ? ((int.Parse(Session["pno"].ToString()) + 1).ToString()) : (pdfPageCount.ToString());
//                ShowPDF();
//            }
//        }

//        public void ShowPDF()
//        {
//            try
//            {
//                string bookID = Request.QueryString["bid"].ToString();
//                string status = "";
//            Restart:
//                string[] Files = Directory.GetFiles(Server.MapPath("PdftoHtml5Scripts"));
//                for (int i = 0; i < Files.Length; i++)
//                {

//                    if (Path.GetExtension(Files[i]).ToLower().Equals(".pdf"))
//                    {
//                        File.Delete(Files[i]);
//                        goto Restart;
//                    }
//                }
//                if (Session["pno"] != null)
//                {
//                    status = GeneratePreview(Session["pno"].ToString());
//                }
//                else
//                {
//                    status = GeneratePreview(txtCurrentPage.Text);
//                }
//                if (status != "")
//                {
//                    string FileName = objMyDBClass.MainDirPhyPath + "/" + bookID + "/Comparison/Page" + Session["pno"].ToString() + "-1.pdf";
//                    if (File.Exists(FileName))
//                    {
//                        File.Copy(FileName, Server.MapPath("PdftoHtml5Scripts") + "/Page" + Session["pno"].ToString() + ".pdf");
//                    }
//                    FileLoadPath.Value = "PdftoHtml5Scripts/Page" + Session["pno"].ToString() + ".pdf";
//                }
//                PDFViewerSource.FilePath = Session["MainDirectory"].ToString() + "/" + bookID + "/Comparison/Page" + Session["pno"].ToString() + "-1.pdf#toolbar=0";

//                this.txtCurrentPage.Text = Session["pno"].ToString();
//            }
//            catch (Exception ex)
//            {
//                //  lblMessage.Text = ex.Message;
//            }

//        }
//        public string GeneratePreview(string page)
//        {


//            string pdfPageFile;
//            string pdfFile;
//            if (Session["ProjectFolderPath"] != null)
//            {
//                pdfPageFile = Session["ProjectFolderPath"].ToString() + "/Page" + page + "-1.pdf";
//                pdfFile = Session["ProjectFolderPath"].ToString() + "/Page" + page + ".pdf";
//            }
//            else
//            {
//                pdfPageFile = GlobalVar.ProjectFolderPath + "/Page" + page + "-1.pdf";
//                pdfFile = GlobalVar.ProjectFolderPath + "/Page" + page + ".pdf";
//            }

//            try
//            {
//                if (!File.Exists(pdfPageFile))
//                {
//                    //ConversionClass.ExtractPages(Server.MapPath("~/" + PdfViewer1.File), pdfPageFile, int.Parse(page), int.Parse(page));
//                    string filePath = "";
//                    if (Request.QueryString["username"] != null)
//                    {
//                        filePath = objMyDBClass.MainDirPhyPath + "/Tests/" + Request.QueryString["username"].ToString() + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".pdf";
//                    }
//                    else
//                    {
//                        string relPdfPath = Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".pdf";
//                        filePath = objMyDBClass.MainDirPhyPath + "/" + relPdfPath;
//                    }
//                    objConversionClass.ExtractPages(filePath, pdfPageFile, int.Parse(page), int.Parse(page));
//                }

//                if (!File.Exists(pdfFile))
//                {
//                    Outsourcing_System.ImageValidator.ImageValidationService imgValidator = new Outsourcing_System.ImageValidator.ImageValidationService();
//                    try
//                    {
//                        pdfFile = imgValidator.GenearatePDFPreview(pdfFile.Replace(".pdf", ".xml"), pdfFile);
//                    }
//                    finally
//                    {
//                        imgValidator.Dispose();
//                    }
//                }
//            }
//            catch (Exception)
//            {
//                pdfFile = "";
//            }
//            return pdfFile;
//        }
//        public void DecideFolderPath(string username)
//        {
//            if (username != "")
//            {
//                GlobalVar.ProjectFolderPath = objMyDBClass.MainDirPhyPath + "/Tests/" + username + "/" + Request.QueryString["bid"].ToString() + "/Comparison";
//                Session["ProjectFolderPath"] = objMyDBClass.MainDirPhyPath + "/Tests/" + username + "/" + Request.QueryString["bid"].ToString() + "/Comparison";
//            }
//            else
//            {
//                GlobalVar.ProjectFolderPath = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString() + "/Comparison";
//                Session["ProjectFolderPath"] = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString() + "/Comparison";
//            }

//        }
//    }
//}
#endregion 
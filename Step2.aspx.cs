using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.Text.RegularExpressions;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Web.Services;

namespace Outsourcing_System
{
    public partial class Step2 : System.Web.UI.Page
    {
        #region |Fields and Properties|

        public string innerText = "";
        int pdfPageCount = 0;
        GlobalVar objGlobal = new GlobalVar();
        MyDBClass objMyDBClass = new MyDBClass();
        ConversionClass objConversionClass = new ConversionClass();

        //public string generate_Timer
        //{
        //    get
        //    {
        //        return (Convert.ToString(ViewState["generate_Timer"]) == null) ? "" : Convert.ToString(ViewState["generate_Timer"]);
        //    }

        //    set
        //    {
        //        ViewState["generate_Timer"] = value;
        //    }
        //}

        #endregion

        #region |Events|

        protected void Page_Load(object sender, EventArgs e)
        {
           // Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "hidediv()", true);

            showMessage("");
            if (!IsPostBack)
            {
                if (Convert.ToString(Session["TimeUpdator_StartTime"]) == "")
                {
                    timehdnmin.Value = "20";
                    timehdnsec.Value = "0";

                    Session["TimeUpdator_StartTime"] = DateTime.Now;
                    Session["TimeUpdator_EndTime"] = DateTime.Now.AddMinutes(20);
                }
                else
                {
                    System.TimeSpan difference = Convert.ToDateTime(Session["TimeUpdator_EndTime"]).Subtract(DateTime.Now);
                    timehdnmin.Value = Convert.ToString(difference.Minutes);
                    timehdnsec.Value = Convert.ToString(difference.Seconds);
                }
            }
            if (rbNpara.Checked)
            {
                sparaFields(false);
            }
            else if (rbSpara.Checked)
            {
                NparaFields(false);
            }
            else
            {
                sparaFields(false);
                NparaFields(false);
            }

            if (Convert.ToString(Request.QueryString["username"]) == null)
            {
                Response.Redirect("Bookmicro.aspx", true);
            }

            //else if (Session["objUser"] == null)
            //{
            //    Server.Transfer("Login.aspx");
            //}
            lblMessage.Text = "";
            if (Session["PDFPAgeCount"] == null)
            {
                //DecideFolderPath(Request.QueryString["username"].ToString());
                DecideFolderPath(Convert.ToString(Session["email"]));
                string tetmlFilePath = System.IO.Directory.GetParent(GlobalVar.ProjectFolderPath) + "\\" + Request.QueryString["bid"].ToString() + ".tetml";
                int pC = objGlobal.GetPageCountFromTetml(tetmlFilePath);
                Session["PDFPAgeCount"] = pdfPageCount = pC;
            }
            else
                pdfPageCount = int.Parse(Session["PDFPAgeCount"].ToString());
            //this.Page.Title = "Outsourcing System :: Book Preview";
            string xmlFile = "";
            if (Convert.ToString(Session["email"]) != "")
            {
                xmlFile = objMyDBClass.MainDirPhyPath + "/Tests/" + Convert.ToString(Session["email"]) + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".rhyw";
            }
            else
            {

                xmlFile = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".rhyw";
            }
            if (!Page.IsPostBack)
            {
                try
                {

                    //btnGenerate_Click(btnGenerate, null);
                    //((EditorMaster)this.Page.Master).hideTimer();

                    if (Convert.ToString(Session["email"]) != "")
                    {
                        PdfViewer1.File = "Files/Tests/" + Convert.ToString(Session["email"]) + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".pdf";
                    }
                    else
                    {
                        PdfViewer1.File = "Files/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".pdf";
                    }
                    getOperationlog(Request.QueryString["bid"].ToString(), Convert.ToString(Session["email"]));
                    DecideFolderPath(Convert.ToString(Session["email"]));
                    lblTotalPages.Text = pdfPageCount.ToString();
                    btnGenerate_Click(btnGenerate, null);
                    this.txtCurrentPage.Text = Session["pno"] == null ? "1" : Session["pno"].ToString();
                    if (File.Exists(xmlFile))
                    {
                        try
                        {
                            ArrayList tableList = new ArrayList();
                            string queryTableList = "select T.TableList from TableList T inner join Activity A on T.AID=A.AID Where A.BID=" + Request.QueryString["bid"].ToString();
                            string dsTableList = objMyDBClass.GetID(queryTableList);
                            if (dsTableList != "")
                            {
                                string[] tables = dsTableList.Split(new char[] { ',' });
                                foreach (string st in tables)
                                {
                                    tableList.Add(st);
                                }
                                string qAid = "select A.AID from Activity A  inner join TableList T on A.AID=T.AID Where A.BID=" + Request.QueryString["bid"].ToString();
                                string aid = objMyDBClass.GetID(qAid);
                                string qUpdateTableCount = "Update Activity Set [Count]=" + tableList.Count + " Where AID=" + aid;
                                objMyDBClass.ExecuteCommand(qUpdateTableCount);

                                Session["tableList"] = tableList;
                            }
                        }
                        catch
                        {
                        }

                    }
                    else
                    {
                        this.txtCurrentPage.Text = "0";
                        this.lblMessage.Text = "File(s) Not Found";
                    }
                }
                catch (Exception ex)
                {
                    this.lblMessage.Text = ex.Message;
                }
            }

            ////Adde by Aamir Ghafoor on 22-04-2014
            //if (Session["ShowTimer"] == "true")
            //{
            //    ((EditorMaster)this.Page.Master).showTimer();
            //}

            loadLatestXml(Convert.ToString(Session["email"]));
            InvalidFontPages();

            //((EditorMaster)this.Page.Master).showTimer();
        }

        public string GeneratePreview(string page)
        {

            // updateLogFile(Request.QueryString["bid"].ToString(), page);
            LoadTree(page);
            //string pdfPageFile = GlobalVar.ProjectFolderPath + "\\Page" + page + "-1.pdf";
            //string pdfFile = GlobalVar.ProjectFolderPath + "\\Page" + page + ".pdf";
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
                    if (Convert.ToString(Session["email"]) != "")
                    {
                        filePath = objMyDBClass.MainDirPhyPath + "/Tests/" + Convert.ToString(Session["email"]) + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".pdf";
                    }
                    else
                    {
                        string relPdfPath = Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".pdf";
                        filePath = objMyDBClass.MainDirPhyPath + "/" + relPdfPath;
                    }
                    objConversionClass.ExtractPages(filePath, pdfPageFile, int.Parse(page), int.Parse(page));
                }

                //if (!File.Exists(pdfFile))
                //{
                Outsourcing_System.ImageValidator.ImageValidationService imgValidator = new Outsourcing_System.ImageValidator.ImageValidationService();
                try
                {
                    pdfFile = imgValidator.GenearatePDFPreview(pdfFile.Replace(".pdf", ".xml"), pdfFile);
                }
                finally
                {
                    imgValidator.Dispose();
                }
                // }
            }
            catch (Exception)
            {
                pdfFile = "";
            }
            return pdfFile;
        }

        public void LoadTree(string page)
        {
            loadLatestXml(Convert.ToString(Session["email"]));
            if (Session["InvalidFontPages"] != null)
            {
                List<int> pageNos = (List<int>)Session["InvalidFontPages"];
                if (pageNos.Contains(int.Parse(page)))
                {
                    TreeView1.Style["Background-Color"] = "Yellow";
                }
                else
                {
                    TreeView1.Style["Background-Color"] = "White";
                }
            }

            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            XmlDocument xmlDoc = objGlobal.GetPageXmlDoc(page);
            try
            {
                //string xmlFile = GlobalVar.ProjectFolderPath + "\\Page" + page + ".xml";
                //string xmlTreeFile = GlobalVar.ProjectFolderPath + "\\Page" + page + "-1.xml";
                string xmlFile;
                string xmlTreeFile;
                if (Session["ProjectFolderPath"] != null)
                {
                    xmlFile = Session["ProjectFolderPath"].ToString() + "/Page" + page + ".xml";
                    xmlTreeFile = Session["ProjectFolderPath"].ToString() + "/Page" + page + "-1.xml";
                }
                else
                {
                    xmlFile = GlobalVar.ProjectFolderPath + "/Page" + page + ".xml";
                    xmlTreeFile = GlobalVar.ProjectFolderPath + "/Page" + page + "-1.xml";
                }
                if (File.Exists(xmlFile))
                {
                    File.Delete(xmlFile);//Deleting the old xml file
                }
                xmlDoc.Save(xmlFile);
                if (File.Exists(xmlTreeFile))
                {
                    File.Delete(xmlTreeFile);//Deleting the old xml file
                }
                XmlDocument xmlTreeDoc = objGlobal.BuildXMLTree(int.Parse(page));
                xmlTreeDoc.Save(xmlTreeFile);
                TreeView1.DataSource = null;
                TreeView1.DataBind();

                XmlDataSource1.XPath = "pbp-book/box|//section-title|//section|pbp-book/upara|//spara|//npara|//table|//image";
                XmlDataSource1.DataFile = xmlTreeFile;
                TreeView1.DataSource = XmlDataSource1;
                TreeView1.DataBind();
                //PopulateTree(xmlTreeDoc);
                TreeView1.ExpandAll();
                // ShowPDF();

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

        private void PopulateTree(XmlDocument objXmlDoc)
        {
            XmlNode RootNode = objXmlDoc.SelectSingleNode("//pbp-book");
            XmlNodeList ParentNodes = RootNode.ChildNodes;
            foreach (XmlNode node in ParentNodes)
            {
                if (node.Name.Equals("section"))
                {
                    string sectionTitle = node.SelectSingleNode("//descendant::section-title").InnerXml;
                    TreeNode tn = new TreeNode(node.Attributes["type"].Value, sectionTitle);
                    SectionHandler(tn, node);
                    TreeView1.Nodes.Add(tn);
                }
                else if (node.Name.Equals("upara"))
                {
                    TreeNode tn = new TreeNode(node.Name, node.InnerXml);
                    UparaHandler(tn, node);
                    TreeView1.Nodes.Add(tn);
                }
            }
        }


        private TreeNode UparaHandler(TreeNode tn, XmlNode subNode)
        {
            XmlNodeList uparachldNodes = subNode.SelectNodes("descendant::*");
            foreach (XmlNode uparasubNode in uparachldNodes)
            {
                if (uparasubNode.Name.Equals("ln"))
                {
                    TreeNode childtn = new TreeNode(uparasubNode.Attributes["displaytext"].Value, uparasubNode.Attributes["outerxml"].Value);
                    tn.ChildNodes.Add(childtn);
                }
            }
            return tn;
        }
        private TreeNode SectionHandler(TreeNode tn, XmlNode node)
        {
            XmlNodeList chldNodes = node.ChildNodes;
            foreach (XmlNode subNode in chldNodes)
            {
                if (subNode.Name.Equals("section"))
                {
                    string sectionTitle = node.SelectSingleNode("descendant::section-title").InnerXml;
                    TreeNode sectionNode = new TreeNode(node.Attributes["type"].Value, sectionTitle);
                    tn.ChildNodes.Add(sectionNode);
                    SectionHandler(sectionNode, subNode);
                }
                else if (subNode.Name.Equals("upara"))
                {
                    TreeNode uparaNode = new TreeNode(subNode.Name, subNode.InnerXml);
                    UparaHandler(uparaNode, subNode);
                    tn.ChildNodes.Add(uparaNode);
                }
            }
            return tn;
        }
        public void ShowPDF()
        {
            try
            {
                string bookID = Request.QueryString["bid"].ToString();
                //PDFViewerSource.FilePath = "Files/" + bookID+ "/"+ bookID + ".pdf#toolbar=0&amp;page=" + Session["pno"].ToString();
                string status = "";
                if (Session["pno"] != null)
                {
                    status = GeneratePreview(Session["pno"].ToString());
                }
                else
                {
                    status = GeneratePreview(txtCurrentPage.Text);
                }
                if (status != "")
                {
                    //PDFViewerTarget.FilePath = "Files/" + bookID + "/Comparison/Page" + Session["pno"].ToString() + ".pdf#toolbar=0";
                    if (Convert.ToString(Session["email"]) != "")
                    {
                        DecideFolderPath(Convert.ToString(Session["email"]));
                        PDFViewerTarget.FilePath = objMyDBClass.MainDirectory + "/Tests/" + Convert.ToString(Session["email"]) + "/" + Request.QueryString["bid"].ToString() + "/Comparison/Page" + Session["pno"].ToString() + ".pdf#toolbar=0";
                    }
                    else
                    {
                        PDFViewerTarget.FilePath = objMyDBClass.MainDirectory + "/" + bookID + "/Comparison/Page" + Session["pno"].ToString() + ".pdf#toolbar=0";
                    }
                }
                //PDFViewerSource.FilePath = "Files/" + bookID + "/Comparison/Page" + Session["pno"].ToString() + "-1.pdf#toolbar=0";
                if (Convert.ToString(Session["email"]) != "")
                {

                    PDFViewerSource.FilePath = objMyDBClass.MainDirectory + "/Tests/" + Convert.ToString(Session["email"]) + "/" + Request.QueryString["bid"].ToString() + "/Comparison/Page" + Session["pno"].ToString() + "-1.pdf#toolbar=0";
                }
                else
                {
                    PDFViewerSource.FilePath = objMyDBClass.MainDirectory + "/" + bookID + "/Comparison/Page" + Session["pno"].ToString() + "-1.pdf#toolbar=0";
                }
                this.txtCurrentPage.Text = Session["pno"].ToString();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //this.RadEditor1.Text = "";
            //this.pnlEdit.Visible = false;
            //this./.Update();
            // this.uplSource.Update();

            //this.uplTree.Update();            
        }

        public void btnFirst_Click(object sender, EventArgs e)
        {
            try
            {
                Timer();
                Session["pno"] = "1";
                ShowPDF();
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
            }
        }



        protected void btnCancleImages_Click(object sender, EventArgs e)
        {

        }

        protected void btnPrevious_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["pno"] != null)
                {
                    updateLogFile(Request.QueryString["bid"].ToString(), Session["pno"].ToString());
                    Session["pno"] = ((int.Parse(Session["pno"].ToString()) - 1) > 0) ? (int.Parse(Session["pno"].ToString()) - 1).ToString() : "1";
                    ShowPDF();
                    Timer();
                }
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
            }
        }

        protected void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["pno"] != null)
                {
                    //if ((int.Parse(Session["pno"].ToString()) + 1) >= PdfViewer1.PdfTotalPages)
                    Timer();
                    updateLogFile(Request.QueryString["bid"].ToString(), Session["pno"].ToString());
                    if ((int.Parse(Session["pno"].ToString()) + 1) >= pdfPageCount)
                    {
                        this.btnGenerate.Text = "Finish Test";
                        this.btnGenerate.BackColor = System.Drawing.Color.LightGray;
                    }
                    //Session["pno"] = (int.Parse(Session["pno"].ToString())) < PdfViewer1.PdfTotalPages ? ((int.Parse(Session["pno"].ToString()) + 1).ToString()) : (PdfViewer1.PdfTotalPages.ToString());
                    Session["pno"] = (int.Parse(Session["pno"].ToString())) < pdfPageCount ? ((int.Parse(Session["pno"].ToString()) + 1).ToString()) : (pdfPageCount.ToString());
                    ShowPDF();
                }
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
            }
        }

        protected void btnLast_Click(object sender, EventArgs e)
        {
            try
            {
                Timer();
                //Session["pno"] = PdfViewer1.PdfTotalPages.ToString();
                Session["pno"] = pdfPageCount.ToString();
                ShowPDF();
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
            }
        }
        private string GenerateTestReport()
        {
            loadLatestXml(Convert.ToString(Session["email"]));
            XmlNodeList correctedNodes = objGlobal.PBPDocument.SelectNodes(@"//*[@correction]");
            int total = objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineTestUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"]));
            foreach (XmlNode node in correctedNodes)
            {
                if (node.Attributes["correction"].Value.Equals(""))
                {
                    node.Attributes.Remove(node.Attributes["correction"]);
                }
            }
            correctedNodes = objGlobal.PBPDocument.SelectNodes(@"//*[@correction]");
            StringBuilder sr = new StringBuilder();

            if (total > 0)
            {
                int remainingOccurences = total - correctedNodes.Count;
                if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
                {
                    objGlobal.XMLPath = Session["XMLPath"].ToString();
                }
                if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
                {
                    objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                }
                objGlobal.SaveXml();
               
                // sr.Append("You Have Corrected " + Convert.ToString(totalOccurences - remainingOccurences) + " out of toalt " + remainingOccurences + " Mistakes");
                sr.Append(remainingOccurences + ", " + total);
            }
            else
            {
                sr.Append("0, " + total);
            }

            return sr.ToString();

        }
        private void InvalidFontPages()
        {
            loadLatestXml(Convert.ToString(Session["email"]));
            XmlNodeList pages = objGlobal.PBPDocument.SelectNodes(@"//ln[@fonttype='NotEmbeded']/@page");
            System.Collections.Generic.List<int> PageNos = new System.Collections.Generic.List<int>();
            foreach (XmlNode ln in pages)
            {
                if (!PageNos.Contains(int.Parse(ln.Value)))
                {
                    PageNos.Add(int.Parse(ln.Value));
                }
            }
            Session["InvalidFontPages"] = PageNos;
            //XPathNavigator nav = objGlobal.PBPDocument.DocumentElement.CreateNavigator();
            //XPathExpression expr = nav.Compile(@"my:distinct-values()");

            //XmlNamespaceManager manager = new XmlNamespaceManager(objGlobal.PBPDocument.NameTable);
            //manager.AddNamespace("ms", "urn:schemas-microsoft-com:xslt");
            //expr.SetContext(manager);

            //var abc = nav.Evaluate(expr);

        }

        [WebMethod]
        public static string GetTestResult()
        {
            Step2 obj = new Step2();

            string temp = obj.GenerateTestReport();
            var result = temp.Split(',');
            double obtained = Convert.ToDouble(result[0]);
            int total = Convert.ToInt32(result[1]);

            HttpContext.Current.Session.Remove("TimeUpdator_StartTime");
            HttpContext.Current.Session.Remove("TimeUpdator_EndTime");

            double percentage = 0.0;

            if (result[0] != "")
            {
                percentage = Convert.ToDouble(obtained / total) * 100;

                HttpContext.Current.Session["Result"] = obtained;

                //if (percentage >= 60)
                //    HttpContext.Current.Response.Redirect("Passed.aspx", false);
                //else
                //    HttpContext.Current.Response.Redirect("Step3.aspx", false);
            }

            return Convert.ToString(percentage);
       }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (btnGenerate.Text == "Finish Test")
                {
                    string temp = GenerateTestReport();
                    var result = temp.Split(',');
                    double obtained = Convert.ToDouble(result[0]);
                    int total = Convert.ToInt32(result[1]);

                    Session.Remove("TimeUpdator_StartTime"); 
                    Session.Remove("TimeUpdator_EndTime");

                    //HttpResponse.RemoveOutputCacheItem("/Outsourcing System/Step2.aspx");

                    //double total = Convert.ToDouble(Session["TotalMarks"].ToString());
                    double percentage = 0.0;

                    if (result[0] != "")
                    {
                        percentage = Convert.ToDouble(obtained / total) * 100;

                        Session["Result"] = obtained;

                        if (percentage >= 80)
                            Response.Redirect("Passed.aspx", false);
                        //Response.Redirect("Passed.aspx?username=" + Convert.ToString(Session["OnlineTestUser"]) + "&bid=" + Convert.ToString(Session["TestName"]), false);
                        //Response.Redirect("Step2.aspx?username=" + username + "&bid=" + rndNumber, false);

                        else
                            Response.Redirect("Failed.aspx", false);
                    }
                }
                else
                {
                    if (Session["pno"] == null)
                    {
                        Session["pno"] = txtCurrentPage.Text;
                    }
                    updateLogFile(Request.QueryString["bid"].ToString(), Session["pno"].ToString());
                    Session["pno"] = this.txtCurrentPage.Text.Trim();
                    ShowPDF();
                    Timer();
                    //showTimer();

                    //Added by Aamir Ghafoor on 22-04-2014
                    //((EditorMaster)this.Page.Master).showTimer();
                    Session["hideTimer"] = "false";
                    // ((EditorMaster)this.Page.Master).Timer();
                    //end
                }
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
            }
        }






        protected void btnMessageOk_Click(object sender, EventArgs e)
        {
            // ShowPDF();
            LoadTree(txtCurrentPage.Text.Trim());
        }

        protected void btnAddSection_Click(object sender, EventArgs e)
        {

        }

        protected void lnkUserPanle_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminPanel.aspx", false);
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Server.Transfer("BookMicro.aspx");
        }

        protected void btnAddTable_Click(object sender, EventArgs e)
        {
            TreeNodeCollection selNodeCol = TreeView1.CheckedNodes;
            if (selNodeCol.Count == 0)
            {
                this.lblMessage.Text = "Select any node";
            }
            else
            {
                string val = selNodeCol[0].Value;


                TableProcessing(val, "insert");
            }
        }

        protected void btnDelTable_Click(object sender, EventArgs e)
        {
            TreeNodeCollection selNodeCol = TreeView1.CheckedNodes;
            if (selNodeCol.Count == 0)
            {
                this.lblMessage.Text = "Select any node";
            }
            else
            {
                string val = selNodeCol[0].Value;
                if (!selNodeCol[0].Text.Equals("upara"))
                {
                    if (selNodeCol[0].Text.Equals("box-title"))
                    {
                        val = selNodeCol[0].ChildNodes[0].Value;
                        TableProcessing(val, "delete");
                    }
                    else if (selNodeCol[0].Text.Equals("break"))
                    {
                        DeleteTag(selNodeCol[0].Value, "");
                    }
                    else if (selNodeCol[0].Text.Contains("......"))
                    {
                        DeleteTag(selNodeCol[0].Value, "");
                    }
                    else
                    {
                        DeleteTag(selNodeCol[0].Value, "");
                    }

                }
                else
                {
                    TableProcessing(val, "delete");
                }

            }
        }

        //Merging Paras  

        protected void imgMergAll_Click(object sender, ImageClickEventArgs e)
        {
            XmlDocument lnDoc = new XmlDocument();
            ArrayList uparas = new ArrayList();
            bool isSameType = true;
            int merging = 0;
            string pageno = "0";
            foreach (TreeNode treeNode in TreeView1.Nodes)
            {
                if (treeNode.Text.ToLower() != "table" && treeNode.Text.ToLower() != "image" && treeNode.Text.ToLower() != "box" && treeNode.Text.ToLower() != "chapter")
                {
                    string nodeVal = treeNode.Value;

                    lnDoc.LoadXml("<book>" + nodeVal + "</book>");
                    XmlNode lnNode = lnDoc.SelectSingleNode("//ln");
                    pageno = lnNode.Attributes["page"].Value;
                    string xPathLine = "//ln[@coord='" + lnNode.Attributes["coord"].Value + "' and @page='" + lnNode.Attributes["page"].Value + "' and @height='" + lnNode.Attributes["height"].Value + "' and @left='" + lnNode.Attributes["left"].Value + "' and @top='" + lnNode.Attributes["top"].Value + "' and @font='" + lnNode.Attributes["font"].Value + "' and  @fontsize='" + lnNode.Attributes["fontsize"].Value + "']";
                    string xPath = xPathLine + "/ancestor::upara|" + xPathLine + "/ancestor::spara|" + xPathLine + "/ancestor::npara";
                    if (objGlobal.PBPDocument == null)
                    {
                        objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                    }
                    uparas.Add(objGlobal.PBPDocument.SelectSingleNode(xPath));
                }
                else
                {
                    isSameType = false;
                    break;
                }
            }
            if (uparas.Count > 1)
            {
                XmlNode mainNode = uparas[0] as XmlNode;

                for (int g = 1; g < uparas.Count; g++)
                {
                    XmlNodeList ChildNodes = null;
                    XmlNode currNode = (uparas[g] as XmlNode);
                ReLoad:
                    ChildNodes = currNode.ChildNodes;
                    foreach (XmlNode tmpNode in ChildNodes)
                    {
                        if (tmpNode.Attributes["SplitError"] != null)
                        {
                            tmpNode.Attributes["SplitError"].Value = "0";
                        }
                        mainNode.AppendChild(tmpNode);
                        goto ReLoad;
                    }
                    merging++;
                    currNode.ParentNode.RemoveChild(currNode);

                    if (mainNode.Attributes["correction"] != null && (mainNode.Attributes["correction"].Value != ""))
                    {
                        string[] operations = mainNode.Attributes["correction"].Value.Split(',');
                        if (operations.Contains("merge"))
                        {
                            mainNode.Attributes["correction"].Value = mainNode.Attributes["correction"].Value.Replace("merge,", "");
                        }

                    }
                }
            }
            lnDoc = null;
            if (isSameType == true)
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
                if (merging > 0)
                {
                    ////Editing Loging Logic by Khail
                    //ConversionLog objconLog = AlreadyExistinLog(pageno);
                    //List<ConversionLog> lstConversionLog = getConversionLog();
                    //if (objconLog != null)
                    //{

                    //    lstConversionLog.Where(d => d.Pageno == pageno).First().Merging = (Convert.ToInt32(objconLog.Merging == null || objconLog.Merging.Equals("") ? "0" : objconLog.Merging) + merging).ToString();
                    //}
                    //else
                    //{
                    //    objconLog = new ConversionLog();
                    //    objconLog.Pageno = pageno;
                    //    objconLog.Merging = merging.ToString();
                    //    lstConversionLog.Add(objconLog);
                    //}
                    //Session["lstConversionLog"] = lstConversionLog;
                    ////Editing Loging Logic Ends here by Khail
                }
                ShowPDF();
            }
            else
            {
                this.lblMessage.Text = "Kindly select same type for merging";
            }


        }
        protected void btnMerge_Click(object sender, ImageClickEventArgs e)
        {
            try
            {
                TreeNodeCollection selNodeCol = TreeView1.CheckedNodes;
                string pageno = "0";
                if (selNodeCol.Count < 2)
                {
                    this.lblMessage.Text = "Select at lease two nodes";
                }
                else
                {
                    XmlDocument lnDoc = new XmlDocument();
                    ArrayList uparas = new ArrayList();
                    bool isSameType = true;
                    int megingOccurence = 0;
                    foreach (TreeNode treeNode in selNodeCol)
                    {
                        if (treeNode.Text.ToLower() != "table" && treeNode.Text.ToLower() != "image" && treeNode.Text.ToLower() != "box")
                        {
                            string nodeVal = treeNode.Value;

                            lnDoc.LoadXml("<book>" + nodeVal + "</book>");
                            XmlNode lnNode = lnDoc.SelectSingleNode("//ln");

                            string xPathLine = "//ln[@coord='" + lnNode.Attributes["coord"].Value + "' and @page='" + lnNode.Attributes["page"].Value + "' and @height='" + lnNode.Attributes["height"].Value + "' and @left='" + lnNode.Attributes["left"].Value + "' and @top='" + lnNode.Attributes["top"].Value + "' and @font='" + lnNode.Attributes["font"].Value + "' and  @fontsize='" + lnNode.Attributes["fontsize"].Value + "']";
                            string xPath = xPathLine + "/ancestor::upara|" + xPathLine + "/ancestor::spara|" + xPathLine + "/ancestor::npara";
                            if (objGlobal.PBPDocument == null)
                            {
                                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                            }
                            uparas.Add(objGlobal.PBPDocument.SelectSingleNode(xPath));
                            pageno = lnNode.Attributes["page"].Value;
                        }
                        else
                        {
                            isSameType = false;
                            break;
                        }
                    }
                    if (uparas.Count > 1)
                    {
                        XmlNode mainNode = uparas[0] as XmlNode;
                        for (int g = 1; g < uparas.Count; g++)
                        {
                            XmlNodeList ChildNodes = null;
                            XmlNode currNode = (uparas[g] as XmlNode);
                        ReLoad:
                            ChildNodes = currNode.ChildNodes;
                            foreach (XmlNode tmpNode in ChildNodes)
                            {
                                if (tmpNode.Attributes["SplitError"] != null)
                                {
                                    tmpNode.Attributes["SplitError"].Value = "0";
                                }
                                mainNode.AppendChild(tmpNode);
                                goto ReLoad;
                            }
                            currNode.ParentNode.RemoveChild(currNode);
                            megingOccurence++;

                            if (mainNode.Attributes["correction"] != null && (mainNode.Attributes["correction"].Value != ""))
                            {
                                string[] operations = mainNode.Attributes["correction"].Value.Split(',');
                                if (operations.Contains("merge"))
                                {
                                    mainNode.Attributes["correction"].Value = mainNode.Attributes["correction"].Value.Replace("merge,", "");
                                }

                            }
                        }
                    }
                    lnDoc = null;
                    if (isSameType == true)
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
                        ////Editing Loging Logic by Khail
                        //ConversionLog objconLog = AlreadyExistinLog(pageno);
                        //List<ConversionLog> lstConversionLog = getConversionLog();
                        //if (objconLog != null)
                        //{

                        //    lstConversionLog.Where(d => d.Pageno == pageno).First().Merging = (Convert.ToInt32(objconLog.Merging == null || objconLog.Merging.Equals("") ? "0" : objconLog.Merging) + megingOccurence).ToString(); ;
                        //}
                        //else
                        //{
                        //    objconLog = new ConversionLog();
                        //    objconLog.Pageno = pageno;
                        //    objconLog.Merging = megingOccurence.ToString();
                        //    lstConversionLog.Add(objconLog);
                        //}
                        //Session["lstConversionLog"] = lstConversionLog;
                        ////Editing Loging Logic Ends here by Khail

                    }
                    else
                    {
                        this.lblMessage.Text = "Kindly select same type for merging";
                    }
                    LoadTree(txtCurrentPage.Text.Trim());
                    //ShowPDF();
                }
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
            }
        }

        //Getting Selected Node for Capitalize, Edit and Splitting        
        protected void TreeView1_SelectedNodeChanged(object sender, EventArgs e)
        {
            try
            {
                string NodeText = TreeView1.SelectedNode.Text;
                NodeText = NodeText.Replace("<div style=\" background:red; font-weight:bold;\">", "").Replace("</div>", "");
                //ScriptManager.RegisterStartupScript(this.Page, GetType(), "General", "ShowGeneralBox();", true);level1
                
                //ScriptManager.RegisterStartupScript(this.Page, GetType(), "General", "ShowLoadingGif();", true);
                //ScriptManager.RegisterStartupScript(this.Page, GetType(), "nn", "ShowLoadingGif();", true);

                if (!NodeText.Equals("chapter") && !NodeText.Equals("upara") && !NodeText.Equals("spara") && !NodeText.Equals("npara") && !NodeText.Equals("level1") && !NodeText.Equals("level2") && !NodeText.Equals("level3") && !NodeText.Equals("level4"))
                {
                    string nodeVal = "";
                    string parentName = "";
                    XmlNode lnNode = null;
                    if (NodeText.ToLower().Equals("upara"))
                    {
                        XmlDocument lnDoc = new XmlDocument();
                        lnDoc.LoadXml("<book>" + TreeView1.SelectedValue + "</book>");
                        lnNode = lnDoc.SelectSingleNode("//ln");

                        nodeVal = lnNode.Attributes["outerxml"] != null ? lnNode.Attributes["outerxml"].Value.Replace("&lt;", "<").Replace("&gt;", ">") : "";
                        parentName = TreeView1.SelectedNode.Parent.Text;
                        lnNode = GetTargetXmlNode(nodeVal, parentName);
                        this.innerText = lnNode.InnerText;
                        hfNodeText.Value = lnNode.InnerText;
                    }
                    else
                    {
                        nodeVal = TreeView1.SelectedValue;
                        parentName = TreeView1.SelectedNode.Parent.Text.Replace("<div style=\" background:red; font-weight:bold;\">", "").Replace("</div>", "");
                        lnNode = GetTargetXmlNode(nodeVal, parentName);
                        this.innerText = lnNode.InnerText;
                        hfNodeText.Value = lnNode.InnerText;
                    }
                    if (parentName == "image")
                    {
                        this.txtImageURL.Text = Path.GetFileName(lnNode.SelectSingleNode("ancestor::image").Attributes["image-url"].Value);
                        XmlNode captionNode = lnNode.SelectSingleNode("ancestor::image").SelectSingleNode(".//caption");
                        this.txtImageCaption.Text = captionNode == null ? "" : captionNode.InnerText;
                        ScriptManager.RegisterStartupScript(this.Page, GetType(), "ImagePostBack", "ImageBoxPostBack();", true);
                    }
                    else
                    {
                        string xml = objConversionClass.Xml2HtmlBoldItalic(lnNode.InnerXml);
                        xml = xml.Replace("&amp;", "&");
                        this.RadEditor1.Content = xml;
                        ScriptManager.RegisterStartupScript(this.Page, GetType(), "serverChanged", "treeViewPostBack();", true);
                    }
                    //ScriptManager.RegisterStartupScript(this.Page, GetType(), "serverChanged", "alert('i am server');", true);
                    Session["TreeNodeVal"] = TreeView1.SelectedValue + "₪" + parentName;
                    TreeView1.SelectedNode.Selected = false;
                    txtFootNote.Text = "";
                }
                else
                {

                    XmlDocument lnDoc = new XmlDocument();
                    lnDoc.LoadXml("<book>" + TreeView1.SelectedValue + "</book>");
                    XmlNode lnNode = lnDoc.SelectSingleNode("//ln");

                    string nodeVal = lnNode.Attributes["outerxml"] != null ? lnNode.Attributes["outerxml"].Value.Replace("&lt;", "<").Replace("&gt;", ">") : "";
                    string parentName = NodeText;

                    this.innerText = lnNode.InnerText;
                    if (NodeText.EndsWith("para") || NodeText.Contains("level"))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, GetType(), "paraConvert", "showRootNodeActions();", true);
                    }
                    else if (parentName == "image")
                    {
                        lnNode = GetTargetXmlNode(nodeVal, parentName);
                        this.txtImageURL.Text = Path.GetFileName(lnNode.SelectSingleNode("ancestor::image").Attributes["image-url"].Value);
                        XmlNode captionNode = lnNode.SelectSingleNode("ancestor::image").SelectSingleNode(".//caption");
                        this.txtImageCaption.Text = captionNode == null ? "" : captionNode.InnerText;
                        ScriptManager.RegisterStartupScript(this.Page, GetType(), "ImagePostBack", "ImageBoxPostBack();", true);
                    }
                    else if (parentName.EndsWith("para"))
                    {
                        ScriptManager.RegisterStartupScript(this.Page, GetType(), "paraConvert", "showRootNodeActions();", true);
                    }
                    else
                    {
                        string xml = objConversionClass.Xml2HtmlBoldItalic(lnNode.InnerXml);
                        lnNode = GetTargetXmlNode(nodeVal, parentName);
                        xml = xml.Replace("&amp;", "&");
                        this.RadEditor1.Content = xml;
                        ScriptManager.RegisterStartupScript(this.Page, GetType(), "serverChanged", "treeViewPostBack();", true);
                    }
                    // ScriptManager.RegisterStartupScript(this.Page, GetType(), "serverChanged", "alert('i am server');", true);
                    Session["TreeNodeVal"] = TreeView1.SelectedValue + "₪" + parentName;
                    TreeView1.SelectedNode.Selected = false;
                    txtFootNote.Text = "";
                }
                //ShowPDF();

            }
            catch (Exception ex)
            {
                //ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
                showMessage(ex.Message);
                //this.lblMessage.Text = ex.Message;
            }
        }

        //Add Image        
        protected void btnAddImage_Click(object sender, ImageClickEventArgs e)
        {
            TreeNodeCollection selNodeCol = TreeView1.CheckedNodes;
            if (selNodeCol.Count == 0)
            {
                this.lblMessage.Text = "Select any node";
            }
            else
            {
                string val = selNodeCol[0].Value;
                TableProcessing(val, "image");
            }
        }

        //Capitalize Line        
        protected void btnCaps_Click(object sender, EventArgs e)
        {
            string nodeVal = TreeView1.SelectedValue;
            XmlNode targetNode = GetTargetXmlNode(nodeVal, TreeView1.SelectedNode.Parent.Text);

            string innerTagXml = targetNode.InnerXml.ToUpper();
            MatchCollection tagCol = Regex.Matches(innerTagXml, "<.*?>");

            foreach (Match tag in tagCol)
            {
                innerTagXml = innerTagXml.Replace(tag.Value, tag.Value.ToLower());
            }
            targetNode.InnerXml = innerTagXml;
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            objGlobal.SaveXml();
            this.RadEditor1.Text = "";
            LoadTree(txtCurrentPage.Text.Trim());
            //ShowPDF();
        }

        //Edit and Save Paras        
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string[] nodeVal = Session["TreeNodeVal"].ToString().Split(new char[] { '₪' });
                XmlNode targetNode = GetTargetXmlNode(nodeVal[0], nodeVal[1]);
                string pageno = targetNode.Attributes["page"].Value;
                string inputText;

                if (RadEditor1.Text.Trim() == "")
                {
                    inputText = "";
                }
                else
                {
                    string TextSpantoFont = RadEditor1.Content;

                    MatchCollection tagColgreen = Regex.Matches(TextSpantoFont, "<span style=\"color: green;\">[<>./\\sa-zA-Z0-9]*</span>");

                    foreach (Match tag in tagColgreen)
                    {
                        string innerText = tag.Value.Replace("<span style=\"color: green;\">", "").Replace("</span>", "");
                        TextSpantoFont = TextSpantoFont.Replace(tag.Value, "<FONT color=\"green\">" + innerText + "</FONT>");
                    }
                    MatchCollection tagColblue = Regex.Matches(TextSpantoFont, "<span style=\"color: blue;\">[<>./\\sa-zA-Z0-9]*</span>");

                    foreach (Match tag in tagColblue)
                    {
                        string innerText = tag.Value.Replace("<span style=\"color: blue;\">", "").Replace("</span>", "");
                        TextSpantoFont = TextSpantoFont.Replace(tag.Value, "<FONT color=\"blue\">" + innerText + "</FONT>");
                    }
                    inputText = objConversionClass.filterHtml(TextSpantoFont);
                }
                if (!hfNodeText.Value.Equals(RadEditor1.Text))
                {
                    if (targetNode.Attributes["correction"] != null && (targetNode.Attributes["correction"].Value != ""))
                    {
                        string[] operations = targetNode.Attributes["correction"].Value.Split(',');
                        if (operations.Contains("edit"))
                        {
                            targetNode.Attributes["correction"].Value = targetNode.Attributes["correction"].Value.Replace("edit,", "");
                        }

                    }
                    ////Editing Loging Logic by Khail
                    //targetNode.Attributes["isEditted"].Value = "true";
                    //ConversionLog objconLog = AlreadyExistinLog(pageno);
                    //List<ConversionLog> lstConversionLog = getConversionLog();
                    //if (objconLog != null)
                    //{

                    //    lstConversionLog.Where(d => d.Pageno == pageno).First().Textediting = (Convert.ToInt32(objconLog.Textediting == null || objconLog.Textediting.Equals("") ? "0" : objconLog.Textediting) + 1).ToString(); ;
                    //}
                    //else
                    //{
                    //    objconLog = new ConversionLog();
                    //    objconLog.Pageno = pageno;
                    //    objconLog.Textediting = "1";
                    //    lstConversionLog.Add(objconLog);
                    //}
                    //Session["lstConversionLog"] = lstConversionLog;
                    ////Editing Loging Logic Ends here by Khail
                }
                targetNode.InnerXml = objConversionClass.Html2XmlBoldItalic(objConversionClass.Html2XmlCommonConversion(inputText));
                if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
                {
                    objGlobal.XMLPath = Session["XMLPath"].ToString();
                }
                if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
                {
                    objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                }
                objGlobal.SaveXml();



                this.RadEditor1.Content = "";
                ShowPDF();
                txtFootNote.Text = "";
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
            }
        }

        protected void imgClose_Click(object sender, EventArgs e)
        {
            this.RadEditor1.Content = "";
            //ShowPDF();
            txtFootNote.Text = "";
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
        }

        //Splitting Paras 
        protected void iimgSplitAll_Click(object sender, EventArgs e)
        {
            try
            {
                int splitting = 0;
                string pageno = "0";
                for (int i = 0; i < TreeView1.Nodes.Count; i++)
                {
                    for (int j = 0; j < TreeView1.Nodes[i].ChildNodes.Count; j++)
                    {

                        if (TreeView1.Nodes[i].ChildNodes[j].Value != "break")
                        {
                            XmlNode targetNode = GetTargetXmlNode(TreeView1.Nodes[i].ChildNodes[j].Value, TreeView1.Nodes[i].ChildNodes[j].Parent.Text);
                            pageno = targetNode.Attributes["page"].Value;
                            XmlNode nextSibling = null;
                            XmlNode tempNode = null;
                            XmlNode parentNode = targetNode.ParentNode;
                            XmlNode grandParentNode = parentNode.ParentNode;

                            XmlNode currentNode = targetNode.ParentNode;
                            XmlElement newElem = targetNode.OwnerDocument.CreateElement(currentNode.Name);

                            foreach (XmlAttribute att in targetNode.ParentNode.Attributes)
                            {
                                newElem.SetAttribute(att.Name, att.Value);

                            }


                            if (targetNode.Attributes["correction"] != null && (targetNode.Attributes["correction"].Value != ""))
                            {
                                string[] operations = targetNode.Attributes["correction"].Value.Split(',');
                                if (operations.Contains("split"))
                                {
                                    targetNode.Attributes["correction"].Value = targetNode.Attributes["correction"].Value.Replace("split,", "");
                                }

                            }
                            nextSibling = targetNode.NextSibling != null ? targetNode.NextSibling : null;
                            if (targetNode.Attributes["MergeError"] != null)
                            {
                                targetNode.Attributes["MergeError"].Value = "0";
                            }
                            newElem.AppendChild(targetNode);
                            while (nextSibling != null)
                            {
                                if (nextSibling.Attributes["MergeError"] != null)
                                {
                                    nextSibling.Attributes["MergeError"].Value = "0";
                                }
                                tempNode = nextSibling.NextSibling;
                                newElem.AppendChild(nextSibling);
                                nextSibling = tempNode;
                            }
                            grandParentNode.InsertAfter(newElem, parentNode);
                            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
                            {
                                objGlobal.XMLPath = Session["XMLPath"].ToString();
                            }
                            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
                            {
                                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                            }
                            objGlobal.SaveXml();
                            splitting++;


                        }
                    }
                }
                if (splitting > 0)
                {
                    ////Editing Loging Logic by Khail
                    //ConversionLog objconLog = AlreadyExistinLog(pageno);
                    //List<ConversionLog> lstConversionLog = getConversionLog();
                    //if (objconLog != null)
                    //{

                    //    lstConversionLog.Where(d => d.Pageno == pageno).First().Splitting = (Convert.ToInt32(objconLog.Splitting == null || objconLog.Splitting.Equals("") ? "0" : objconLog.Splitting) + splitting).ToString(); ;
                    //}
                    //else
                    //{
                    //    objconLog = new ConversionLog();
                    //    objconLog.Pageno = pageno;
                    //    objconLog.Splitting = splitting.ToString();
                    //    lstConversionLog.Add(objconLog);
                    //}
                    //Session["lstConversionLog"] = lstConversionLog;
                    ////Editing Loging Logic Ends here by Khail
                }
                ShowPDF();
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        protected void btnSplit_Click(object sender, EventArgs e)
        {
            try
            {
                string[] nodeVal = Session["TreeNodeVal"].ToString().Split(new char[] { '₪' });
                XmlNode targetNode = GetTargetXmlNode(nodeVal[0], nodeVal[1]);
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

                    }

                    if (targetNode.Attributes["correction"] != null && (targetNode.Attributes["correction"].Value != ""))
                    {
                        string[] operations = targetNode.Attributes["correction"].Value.Split(',');
                        if (operations.Contains("split"))
                        {
                            targetNode.Attributes["correction"].Value = targetNode.Attributes["correction"].Value.Replace("split,", "");
                        }

                    }
                    nextSibling = targetNode.NextSibling != null ? targetNode.NextSibling : null;
                    if (targetNode.Attributes["MergeError"] != null)
                    {
                        targetNode.Attributes["MergeError"].Value = "0";
                    }
                    newElem.AppendChild(targetNode);
                    while (nextSibling != null)
                    {
                        if (nextSibling.Attributes["MergeError"] != null)
                        {
                            nextSibling.Attributes["MergeError"].Value = "0";
                        }
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
                ////Editing Loging Logic by Khail
                //ConversionLog objconLog = AlreadyExistinLog(pageno);
                //List<ConversionLog> lstConversionLog = getConversionLog();
                //if (objconLog != null)
                //{

                //    lstConversionLog.Where(d => d.Pageno == pageno).First().Splitting = (Convert.ToInt32(objconLog.Splitting == null || objconLog.Splitting.Equals("") ? "0" : objconLog.Splitting) + 1).ToString(); ;
                //}
                //else
                //{
                //    objconLog = new ConversionLog();
                //    objconLog.Pageno = pageno;
                //    objconLog.Splitting = "1";
                //    lstConversionLog.Add(objconLog);
                //}
                //Session["lstConversionLog"] = lstConversionLog;
                //Editing Loging Logic Ends here by Khail
                this.RadEditor1.Content = "";
                LoadTree(txtCurrentPage.Text.Trim());
                //ShowPDF();
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
            }
            catch (Exception ex)
            {
                showMessage(ex.Message);
            }
        }

        protected void btnBoxSave_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
                TreeNodeCollection selNodeCol = TreeView1.CheckedNodes;
                if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
                {
                    objGlobal.XMLPath = Session["XMLPath"].ToString();
                }
                if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
                {
                    objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                }
                if (selNodeCol.Count < 1)
                {
                    this.lblMessage.Text = "Select at lease one node";
                }
                else
                {
                    XmlDocument lnDoc = new XmlDocument();
                    ArrayList uparas = new ArrayList();
                    foreach (TreeNode treeNode in selNodeCol)
                    {
                        string nodeVal = treeNode.Value;

                        lnDoc.LoadXml("<book>" + nodeVal + "</book>");
                        XmlNode lnNode = lnDoc.SelectSingleNode("//ln");

                        string xPathLine = "//ln[@coord='" + lnNode.Attributes["coord"].Value + "' and @page='" + lnNode.Attributes["page"].Value + "' and @height='" + lnNode.Attributes["height"].Value + "' and @left='" + lnNode.Attributes["left"].Value + "' and @top='" + lnNode.Attributes["top"].Value + "' and @font='" + lnNode.Attributes["font"].Value + "' and  @fontsize='" + lnNode.Attributes["fontsize"].Value + "']";
                        string xPath = xPathLine + "/ancestor::upara|" + xPathLine + "/ancestor::spara|" + xPathLine + "/ancestor::npara";
                        uparas.Add(objGlobal.PBPDocument.SelectSingleNode(xPath));
                    }
                    if (uparas.Count > 0)
                    {
                        XmlElement box = (uparas[0] as XmlNode).OwnerDocument.CreateElement("box");
                        box.SetAttribute("id", "0");
                        box.SetAttribute("bgcolor", chhBackColor.Checked ? "gray" : "none");
                        box.SetAttribute("border", chkBoxBorder.Checked ? "on" : "off");
                        XmlElement boxtitle = (uparas[0] as XmlNode).OwnerDocument.CreateElement("box-title");
                        boxtitle.AppendChild((uparas[0] as XmlNode).SelectSingleNode(".//ln").Clone());
                        boxtitle.SelectSingleNode(".//ln").InnerXml = txtBoxTitle.Text.Trim();
                        box.AppendChild(boxtitle);
                        XmlNode insertBefore = (uparas[0] as XmlNode);
                        foreach (XmlNode tmpNode in uparas)
                        {
                            box.AppendChild(tmpNode.Clone());
                        }
                        insertBefore.ParentNode.InsertBefore(box, insertBefore);
                        foreach (XmlNode tmpNode in uparas)
                        {
                            tmpNode.ParentNode.RemoveChild(tmpNode);
                        }
                    }

                    objGlobal.SaveXml();
                    ShowPDF();
                }
            }
            catch (Exception ex)
            {
                showMessage(ex.Message);
            }
        }

        protected void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
                TreeNodeCollection selNodeCol = TreeView1.CheckedNodes;
                bool SameNodes = true;
                for (int i = 0; i < selNodeCol.Count; i++)
                {
                    if (!selNodeCol[0].Text.Equals(selNodeCol[i].Text))
                    {
                        SameNodes = false;
                    }

                }
                if (!SameNodes)
                {
                    lblMessage.Text = "Please select Nodes of same para type.";
                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "showMessageDiv();", true);
                }
                else
                {
                    for (int i = 0; i < selNodeCol.Count; i++)
                    {
                        string nodeVal = selNodeCol[i].Value;
                        string parentName = selNodeCol[i].Text;
                        XmlNode lnNode = GetTargetXmlNode(nodeVal, parentName);
                        string pageno = lnNode.Attributes["page"].Value;
                        XmlNode nodetoConvert = lnNode.ParentNode.Name.ToLower().Equals("line") | lnNode.ParentNode.Name.ToLower().Equals("para") ? lnNode.ParentNode.ParentNode : lnNode.ParentNode;
                        if (rbSpara.Checked)
                        {
                            convertSpara(nodetoConvert);
                        }
                        else if (rbNpara.Checked)
                        {
                            ConvertNpara(nodetoConvert);
                        }
                        else if (rbUpara.Checked)
                        {
                            convertUpara(nodetoConvert);
                        }
                        ////Editing Loging Logic by Khail
                        //ConversionLog objconLog = AlreadyExistinLog(pageno);
                        //List<ConversionLog> lstConversionLog = getConversionLog();
                        //if (objconLog != null)
                        //{

                        //    lstConversionLog.Where(d => d.Pageno == pageno).First().Merging = (Convert.ToInt32(objconLog.Merging == null || objconLog.Merging.Equals("") ? "0" : objconLog.Merging) + 1).ToString();
                        //}
                        //else
                        //{
                        //    objconLog = new ConversionLog();
                        //    objconLog.Pageno = pageno;
                        //    objconLog.Merging = "1";
                        //    lstConversionLog.Add(objconLog);
                        //}
                        //Session["lstConversionLog"] = lstConversionLog;
                        ////Editing Loging Logic Ends here by Khail
                    }
                    LoadTree(txtCurrentPage.Text.Trim());
                    //ShowPDF();
                }
            }
            catch (Exception ex)
            {
                showMessage(ex.Message);
            }
        }

        protected void btnAddBox_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "serverChanged", "showBoxDiv();", true);
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
        }

        protected void btnYesTable_Click(object sender, EventArgs e)
        {
            if (Session["nodeVal"] == null)
            {
                this.lblMessage.Text = "Select any node";
            }
            else
            {
                TableProcessing(Session["nodeVal"].ToString(), "extra");
            }
        }

        protected void btnImageSave_Click(object sender, EventArgs e)
        {
            string imageName = File1.PostedFile.FileName;
            //imageName = fileUpload1.FileName;
            if (imageName != "" && Path.GetExtension(imageName) == ".jpg" && txtImageURL.Text != "")
            {
                string tmpPath = "Resources\\" + txtImageURL.Text;
                //string savePath = Server.MapPath("~/Files\\" + Request.QueryString["bid"].ToString());
                string savePath = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString();
                File1.PostedFile.SaveAs(savePath + "\\" + tmpPath);
                TableProcessing(Session["TreeNodeVal"].ToString(), "image_mod");
            }
            else
            {
                TableProcessing(Session["TreeNodeVal"].ToString(), "image_mod");
            }
        }

        protected void btnSpecialChrac_Click(object sender, EventArgs e)
        {
            string selectedText = txtSelectedText.Text;
            string newInerHtml = RadEditor1.Content;
            // newInerHtml=newInerHtml.Replace(selectedText,"<FONT color=blue>" + selectedText + "</FONT>");
            newInerHtml = newInerHtml.Replace(selectedText, "<special-chars>" + selectedText + "</special-chars>");
            RadEditor1.Content = newInerHtml;
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "treeViewPostBack();", true);

        }

        protected void btnHyperLink_Click(object sender, EventArgs e)
        {
            string selectedText = txtSelectedText.Text;
            string newInerHtml = RadEditor1.Content;
            // newInerHtml=newInerHtml.Replace(selectedText,"<FONT color=blue>" + selectedText + "</FONT>");
            newInerHtml = newInerHtml.Replace(selectedText, "<a href='" + selectedText + "'>" + selectedText + "</a>");
            RadEditor1.Content = newInerHtml;
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "treeViewPostBack();", true);
        }

        protected void btnPageBreak_Click(object sender, EventArgs e)
        {

            string newInerHtml = RadEditor1.Content;
            if (newInerHtml.Contains(@"<break type='page'"))
            {
                Match value = Regex.Match(newInerHtml, "<break type='page' num='\\d' id='\\d'/>");
                RadEditor1.Content = RadEditor1.Content.Replace(value.Value, "");
            }
            else
            {
                RadEditor1.Content = newInerHtml + "<break type='page' num='" + txtPageBreakNo.Text + "' id='4'/>";
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "treeViewPostBack();", true);
            }

        }

        protected void btnFootNote_Click(object sender, EventArgs e)
        {
            int FootNoteno = 1;
            MatchCollection matches = Regex.Matches(RadEditor1.Content, @"(<sup>|<SUP>)\d(</sup>|</SUP>)");
            if (matches.Count > 0)
            {
                string pageNo = matches[matches.Count - 1].Value.Replace("<sup>", "").Replace("<SUP>", "").Replace("</sup>", "").Replace("</SUP>", "");
                FootNoteno = Convert.ToInt32(pageNo) + 1;
            }
            string selectedText = txtSelectedText.Text;
            string newInerHtml = RadEditor1.Content;
            string cleanedSelectedText = selectedText.Replace("<STRONG>", "").Replace("</STRONG>", "");

            RadEditor1.Content = newInerHtml.Replace(selectedText, cleanedSelectedText + "<FONT color=green><SUP>" + FootNoteno + "</SUP></FONT>");
            RadEditor1.Content = RadEditor1.Content + " " + "<BR><FONT color=green><SUB>" + txtFootNote.Text + "</SUB></FONT>";
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "treeViewPostBack();", true);

        }

        protected void TreeView1_TreeNodePopulate(object sender, TreeNodeEventArgs e)
        {

        }

        protected void ddlSparaType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "ImagePostBack", "showConvertPara();", true);
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
            if (ddlSparaType.SelectedValue.Equals("other"))
            {
                ddlSparaOrientation.Enabled = true;
                ddlSparaBackground.Enabled = true;
            }
            else
            {
                ddlSparaOrientation.Enabled = false;
                ddlSparaBackground.Enabled = false;
            }
        }

        protected void rbSpara_CheckedChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "ImagePostBack", "showConvertPara();", true);
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
            NparaFields(false);
            sparaFields(true);
        }

        protected void rbUpara_CheckedChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "ImagePostBack", "showConvertPara();", true);
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
            NparaFields(false);
            sparaFields(false);
        }

        protected void rbNpara_CheckedChanged(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "ImagePostBack", "showConvertPara();", true);
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
            sparaFields(false);
            NparaFields(true);
        }

        protected void lnkConvert_Click(object sender, EventArgs e)
        {
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "paraConvert", "showConvertPara();", true);
            ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
        }

        protected void lnkAddUpara_Click(object sender, EventArgs e)
        {
            try
            {

                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
                TreeNodeCollection selNodeCol = TreeView1.CheckedNodes;
                string nodeVal = selNodeCol[0].Value;
                string parentName = selNodeCol[0].Text;
                XmlNode lnNode = GetTargetXmlNode(nodeVal, parentName);
                XmlNode tempNode = lnNode.ParentNode;
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
                ln.SetAttribute("isEditted", "false");
                ln.InnerXml = "..........";
                upara.AppendChild(ln);

                tempNode.ParentNode.InsertAfter(upara, tempNode);
                ////Editing Loging Logic by Khail
                //ConversionLog objconLog = AlreadyExistinLog(txtCurrentPage.Text);
                //List<ConversionLog> lstConversionLog = getConversionLog();
                //if (objconLog != null)
                //{

                //    lstConversionLog.Where(d => d.Pageno == txtCurrentPage.Text).First().Paraaddition = (Convert.ToInt32(objconLog.Paraaddition == null || objconLog.Paraaddition.Equals("") ? "0" : objconLog.Paraaddition) + 1).ToString();
                //}
                //else
                //{
                //    objconLog = new ConversionLog();
                //    objconLog.Pageno = txtCurrentPage.Text;
                //    objconLog.Paraaddition = "1";
                //    lstConversionLog.Add(objconLog);
                //}
                //Session["lstConversionLog"] = lstConversionLog;
                ////Editing Loging Logic Ends here by Khail
                objGlobal.SaveXml();
                ShowPDF();
            }
            catch (Exception ex)
            {
                showMessage(ex.Message);
            }
        }

        protected void lnkAddSection_Click(object sender, EventArgs e)
        {

        }

        protected void lnklevel1_Click(object sender, EventArgs e)
        {
            try
            {
                AddSection("level1", 4);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "showMessageDiv();", true);
                showMessage(ex.Message);
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
                lblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "showMessageDiv();", true);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
                showMessage(ex.Message);
            }
        }

        protected void lnklevel3_Click(object sender, EventArgs e)
        {
            try
            {
                AddSection("level3", 2);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "showMessageDiv();", true);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
                showMessage(ex.Message);
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
                lblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "showMessageDiv();", true);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
                showMessage(ex.Message);
            }
        }

        protected void lnkChapter_Click(object sender, EventArgs e)
        {

            try
            {
                AddSection("chapter", 5);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "showMessageDiv();", true);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
                showMessage(ex.Message);
            }
        }

        protected void lnkPart_Click(object sender, EventArgs e)
        {
            try
            {
                AddSection("part", 6);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "showMessageDiv();", true);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
                showMessage(ex.Message);
            }
        }

        protected void lnkOther_Click(object sender, EventArgs e)
        {
            try
            {
                AddSection("other", 7);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
            }
            catch (Exception ex)
            {
                lblMessage.Text = ex.Message;
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "showMessageDiv();", true);
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "createcssmenu();", true);
                showMessage(ex.Message);
            }
        }

        #endregion

        #region |Methods|

        #region |comments about convertSpara|
        /*Created on 10-09-2013 by Khalil
         * para to convert is taken as parameter from treenode
         * from selected treenode ln node is available
         * from ln node its parent para node is selected 
         * and this para node is taken as parameter.
         */
        #endregion
        //public void showTimer()
        //{
        //    //trTimer.Visible = true;
        //    //Page.ClientScript.RegisterStartupScript(GetType(), "pScript1", "show()", true);
        //    Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "Timer()", true);
        //}

        private void showMessage(string message)
        {
            if (message != "")
            {
                DivError.Visible = true;
                lblError.Text = message;
            }
            else
            {
                DivError.Visible = false;
            }
        }

        public void Timer()
        {
            //if (timehdnmin.Value == "20")
            //{
            Page.ClientScript.RegisterStartupScript(GetType(), "pScript", "Timer()", true);
            //}
        }
        public void DecideFolderPath(string email)
        {
            if (email != "")
            {
                GlobalVar.ProjectFolderPath = objMyDBClass.MainDirPhyPath + "/Tests/" + email + "/" + Request.QueryString["bid"].ToString() + "/Comparison";
                Session["ProjectFolderPath"] = objMyDBClass.MainDirPhyPath + "/Tests/" + email + "/" + Request.QueryString["bid"].ToString() + "/Comparison";
                objMyDBClass.MainDirectory = ConfigurationManager.AppSettings["MainDirectory"].ToString();
            }
            else
            {
                GlobalVar.ProjectFolderPath = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString() + "/Comparison";
                Session["ProjectFolderPath"] = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString() + "/Comparison";
            }

        }

        public void updateLogFile(string BookId, string pageno)
        {
            List<ConversionLog> lstConversion = (List<ConversionLog>)Session["lstConversionLog"];

            StringBuilder strbuilder = new StringBuilder();
            strbuilder.AppendLine("Page No,Merging,Splitting,Text Editing,Para Conversions,Table Inserted,Box Inserted,Image Inserted,Node Deleted,Para Addition,Section Addition");
            foreach (ConversionLog item in lstConversion)
            {
                strbuilder.AppendLine(item.Pageno + "," + item.Merging + "," + item.Splitting + "," + item.Textediting + "," + item.Paraconversions + "," + item.Tableinserted + "," + item.Boxinserted + "," + item.Imageinserted + "," + item.Nodedeleted + "," + item.Paraaddition + "," + item.Sectionaddition);
            }
            if (Convert.ToString(Session["email"]) != "")
            {
                File.WriteAllText(objMyDBClass.MainDirPhyPath + "/Tests/" + Convert.ToString(Session["email"]) + "/" + Request.QueryString["bid"].ToString() + "/Comparison/ListOfConversionEditing.csv", strbuilder.ToString());
            }
            else
            {
                File.WriteAllText(objMyDBClass.MainDirPhyPath + "/" + BookId + "/Comparison/ListOfConversionEditing.csv", strbuilder.ToString());
            }
        }

        public List<ConversionLog> getConversionLog()
        {
            List<ConversionLog> lstConversionLog = new List<ConversionLog>();
            if (Session["lstConversionLog"] != null)
            {
                lstConversionLog = (List<ConversionLog>)Session["lstConversionLog"];
            }
            return lstConversionLog;
        }
        public ConversionLog AlreadyExistinLog(string pageno)
        {
            List<ConversionLog> lstConversionLog = getConversionLog();
            if (lstConversionLog.Count > 0)
            {
                var objConversionLog = from n in lstConversionLog where (n.Pageno.Equals(pageno)) select n;
                foreach (var item in objConversionLog)
                {
                    if (item != null)
                        return (ConversionLog)item;
                }

                return null;
            }
            return null;
        }

        public void getOperationlog(string BookId, string email)
        {
            List<ConversionLog> lstConversionLog = new List<ConversionLog>();
            string DirPath = "";
            if (email != "")
            {
                DirPath = objMyDBClass.MainDirPhyPath + "/Tests/" + email + "/" + BookId;
            }
            else
            {
                DirPath = objMyDBClass.MainDirPhyPath + "/" + BookId;
            }
            if (File.Exists(DirPath + "/Comparison/ListOfConversionEditing.csv"))
            {
                StreamReader sr = new StreamReader(DirPath + "/Comparison/ListOfConversionEditing.csv");
                string fileData = sr.ReadToEnd().Replace("\r\n", "$");
                sr.Dispose();
                string[] Operations = fileData.Remove(fileData.Length - 1, 1).Split('$');
                for (int i = 1; i < Operations.Length; i++)
                {
                    ConversionLog objConLog = new ConversionLog();
                    string[] operationLog = Operations[i].Split(',');
                    objConLog.Pageno = operationLog[0];
                    objConLog.Merging = operationLog[1];
                    objConLog.Splitting = operationLog[2];
                    objConLog.Textediting = operationLog[3];
                    objConLog.Paraconversions = operationLog[4];
                    objConLog.Tableinserted = operationLog[5];
                    objConLog.Boxinserted = operationLog[6];
                    objConLog.Imageinserted = operationLog[7];
                    objConLog.Nodedeleted = operationLog[8];
                    objConLog.Paraaddition = operationLog[9];
                    objConLog.Sectionaddition = operationLog[10];
                    lstConversionLog.Add(objConLog);
                }
            }
            Session["lstConversionLog"] = lstConversionLog;
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

                if (origNodyCopy.Attributes["correction"] != null)
                {
                    ((XmlElement)convertedNode).SetAttribute("correction", "");
                }
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
                    if (attr.Name == "id" | attr.Name == "pnum" | attr.Name == "text-indent" | attr.Name == "padding-bottom" | attr.Name == "conversion-Operations")
                        ((XmlElement)convertedNode).SetAttribute(attr.Name, attr.Value);

                }

                #endregion
                origNodyCopy.ParentNode.InsertBefore(convertedNode, origNodyCopy);
                origNodyCopy.ParentNode.RemoveChild(origNodyCopy);
                objGlobal.SaveXml();
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
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
                    if (attr.Name == "id" | attr.Name == "pnum" | attr.Name == "text-indent" | attr.Name == "padding-bottom" | attr.Name == "conversion-Operations")
                        ((XmlElement)convertedNode).SetAttribute(attr.Name, attr.Value);

                }

                origNodyCopy.ParentNode.InsertBefore(convertedNode, origNodyCopy);
                origNodyCopy.ParentNode.RemoveChild(origNodyCopy);
                objGlobal.SaveXml();
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
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
                    if (attr.Name == "id" | attr.Name == "pnum" | attr.Name == "text-indent" | attr.Name == "padding-bottom" | attr.Name == "conversion-Operations")
                        ((XmlElement)convertedNode).SetAttribute(attr.Name, attr.Value);

                }


                origNodyCopy.ParentNode.InsertBefore(convertedNode, origNodyCopy);
                origNodyCopy.ParentNode.RemoveChild(origNodyCopy);
                objGlobal.SaveXml();
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
            }
        }


        private void NparaFields(bool val)
        {
            chkHasNumbers.Enabled = val;
            ddlHasStartNo.Enabled = val;
            ddlSign.Enabled = val;
        }

        private void sparaFields(bool val)
        {
            ddlSparaType.Enabled = val;
            ddlSparaSubType.Enabled = val;

            chkStanza.Enabled = val;
        }

        //Getting Original Node from PBP Document        
        public XmlNode GetTargetXmlNode(string nodeVal, string parentName)
        {
            XmlDocument lnDoc = new XmlDocument();
            lnDoc.LoadXml("<book>" + nodeVal + "</book>");
            XmlNode lnNode = lnDoc.SelectSingleNode("//ln");
            loadLatestXml(Convert.ToString(Session["email"]));
            string xPathLine = "//ln[@coord='" + lnNode.Attributes["coord"].Value + "' and @page='" + lnNode.Attributes["page"].Value + "' and @height='" + lnNode.Attributes["height"].Value + "' and @left='" + lnNode.Attributes["left"].Value + "' and @top='" + lnNode.Attributes["top"].Value + "' and @font='" + lnNode.Attributes["font"].Value + "' and  @fontsize='" + lnNode.Attributes["fontsize"].Value + "']";
            XmlNode targetNode = null;

            XmlNodeList lsNodes = objGlobal.PBPDocument.SelectNodes(xPathLine);
            if (parentName.Equals("image"))
            {
                foreach (XmlNode tmpNode in lsNodes)
                {

                    if (tmpNode.SelectSingleNode("ancestor::image") != null && lnNode.InnerText == tmpNode.InnerText)
                    {
                        targetNode = tmpNode;
                        break;
                    }

                }
            }
            else
            {
                foreach (XmlNode tmpNode in lsNodes)
                {
                    XmlNode tempParentNode = tmpNode.SelectSingleNode("ancestor::spara|ancestor::upara|ancestor::npara|ancestor::section-title");
                    if (tempParentNode != null && (tempParentNode.Name == parentName && lnNode.InnerText == tmpNode.InnerText))
                    {
                        targetNode = tmpNode;
                        break;
                    }
                }
            }
            return targetNode;
        }

        //Adding and Deleting Table        
        public void DeleteTag(string nodeVal, string info)
        {
            try
            {
                string xPathLine;
                loadLatestXml(Convert.ToString(Session["email"]));
                XmlDocument lnDoc = new XmlDocument();
                lnDoc.LoadXml("<book>" + nodeVal + "</book>");
                string pageno = lnDoc.Attributes["page"].Value;
                if (nodeVal.Contains("break"))
                {

                    XmlNode breakNode = lnDoc.SelectSingleNode("//break");
                    xPathLine = "//break[@type='" + breakNode.Attributes["type"].Value + "' and @id='" + breakNode.Attributes["id"].Value + "']";
                    XmlNode tempNode = objGlobal.PBPDocument.SelectSingleNode(xPathLine);
                    tempNode.ParentNode.RemoveChild(tempNode);
                }
                else
                {
                    XmlNode lnNode = lnDoc.SelectSingleNode("//ln");

                    xPathLine = "//ln[@coord='" + lnNode.Attributes["coord"].Value + "' and @page='" + lnNode.Attributes["page"].Value + "' and @height='" + lnNode.Attributes["height"].Value + "' and @left='" + lnNode.Attributes["left"].Value + "' and @top='" + lnNode.Attributes["top"].Value + "' and @font='" + lnNode.Attributes["font"].Value + "' and  @fontsize='" + lnNode.Attributes["fontsize"].Value + "']";
                    XmlNode tgNode = null;
                    if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
                    {
                        objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                    }
                    XmlNodeList targetList = objGlobal.PBPDocument.SelectNodes(xPathLine);
                    foreach (XmlNode tmpNode in targetList)
                    {
                        if (tmpNode.InnerXml.Contains(nodeVal.Split(new char[] { '₪' })[0]))
                        {
                            tgNode = tmpNode;
                        }
                        else if (tmpNode.ParentNode.Name.Contains("para"))
                        {
                            tgNode = tmpNode;
                        }
                        else if (tmpNode.SelectSingleNode("ancestor::image") != null)
                        {
                            tgNode = tmpNode.SelectSingleNode("ancestor::image");
                        }
                        else if (tmpNode.ParentNode.Name.Equals("section-title"))
                        {
                            tgNode = tmpNode.SelectSingleNode("ancestor::section");
                        }
                    }
                    XmlNode body = tgNode.SelectSingleNode("//body");
                    if (body.InnerText.Equals(""))
                    {
                        tgNode.ParentNode.RemoveChild(tgNode);
                    }
                    else
                    {
                        lblMessage.Text = "Section Contain Text.";
                    }
                }
                ////Editing Loging Logic by Khail
                //ConversionLog objconLog = AlreadyExistinLog(pageno);
                //List<ConversionLog> lstConversionLog = getConversionLog();
                //if (objconLog != null)
                //{

                //    lstConversionLog.Where(d => d.Pageno == pageno).First().Nodedeleted = (Convert.ToInt32(objconLog.Nodedeleted == null || objconLog.Nodedeleted.Equals("") ? "0" : objconLog.Nodedeleted) + 1).ToString();
                //}
                //else
                //{
                //    objconLog = new ConversionLog();
                //    objconLog.Pageno = pageno;
                //    objconLog.Nodedeleted = "1";
                //    lstConversionLog.Add(objconLog);
                //}
                //Session["lstConversionLog"] = lstConversionLog;
                ////Editing Loging Logic Ends here by Khail
                objGlobal.SaveXml();
                LoadTree(txtCurrentPage.Text.Trim());
                //ShowPDF();
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
            }
        }

        public void TableProcessing(string nodeVal, string operation)
        {
            XmlDocument lnDoc = new XmlDocument();
            lnDoc.LoadXml("<book>" + nodeVal + "</book>");
            XmlNode lnNode = lnDoc.SelectSingleNode("//ln");
            loadLatestXml(Convert.ToString(Session["email"]));
            XmlNodeList tableList = objGlobal.PBPDocument.SelectNodes("//ln[@page='" + lnNode.Attributes["page"].Value + "']/ancestor::table");
            string tableText = "Table_" + lnNode.Attributes["page"].Value + "_" + (tableList.Count == 0 ? 1 : tableList.Count);
            lnNode.InnerXml = tableText;
            lnDoc = null;
            string pageno = lnNode.Attributes["page"].Value;
            string xPathLine = "//ln[@coord='" + lnNode.Attributes["coord"].Value + "' and @page='" + lnNode.Attributes["page"].Value + "' and @height='" + lnNode.Attributes["height"].Value + "' and @left='" + lnNode.Attributes["left"].Value + "' and @top='" + lnNode.Attributes["top"].Value + "' and @font='" + lnNode.Attributes["font"].Value + "' and  @fontsize='" + lnNode.Attributes["fontsize"].Value + "']";
            string xPath = xPathLine + "/ancestor::upara|" + xPathLine + "/ancestor::spara|" + xPathLine + "/ancestor::npara|" + xPathLine + "/ancestor::image|" + xPathLine + "/ancestor::table";

            XmlNodeList targetList = objGlobal.PBPDocument.SelectNodes(xPath);
            XmlNode tgNode = null;
            foreach (XmlNode tmpNode in targetList)
            {
                if (tmpNode.InnerXml.Contains(nodeVal.Split(new char[] { '₪' })[0]))
                {
                    tgNode = tmpNode;
                }
                else if (tmpNode.ParentNode.Name.Equals("box"))
                {
                    tgNode = tmpNode.ParentNode;
                }
            }

            if (operation == "insert")
            {
                if (Session["tableList"] == null || !(Session["tableList"] as ArrayList).Contains(tableText))
                {
                    Session["nodeVal"] = nodeVal;
                    ScriptManager.RegisterStartupScript(this.Page, GetType(), "tableConfirmation", "TableConfirmationPostBack();", true);
                }
                else
                {
                    XmlElement tableElem = objGlobal.PBPDocument.CreateElement("table");
                    tableElem.SetAttribute("id", tableText);
                    tableElem.InnerXml = lnNode.OuterXml;
                    tgNode.ParentNode.InsertBefore(tableElem, tgNode);
                    Session.Remove("nodeVal");
                }
            }
            else if (operation == "extra")
            {
                XmlElement tableElem = objGlobal.PBPDocument.CreateElement("table");
                tableElem.SetAttribute("id", tableText);
                tableElem.InnerXml = lnNode.OuterXml;
                tgNode.ParentNode.InsertBefore(tableElem, tgNode);
                Session.Remove("nodeVal");
                string queryGetExistExTables = "Select Extra from TableList where AID=(Select AID from Activity where BID=" + Request.QueryString["bid"].ToString() + ")";
                string existTables = objMyDBClass.GetID(queryGetExistExTables) + "," + tableText;
                string upDateTable = "Update TableList Set Extra='" + existTables.TrimStart(new char[] { ',' }) + "' Where AID=(Select AID from Activity where BID=" + Request.QueryString["bid"].ToString() + ")";
                objMyDBClass.ExecuteCommand(upDateTable);
            }
            else if (operation == "delete")
            {
                tgNode.ParentNode.RemoveChild(tgNode);

                ////Editing Loging Logic by Khail
                //ConversionLog objconLog = AlreadyExistinLog(pageno);
                //List<ConversionLog> lstConversionLog = getConversionLog();
                //if (objconLog != null)
                //{

                //    lstConversionLog.Where(d => d.Pageno == pageno).First().Nodedeleted = (Convert.ToInt32(objconLog.Nodedeleted == null || objconLog.Nodedeleted.Equals("") ? "0" : objconLog.Nodedeleted) + 1).ToString();
                //}
                //else
                //{
                //    objconLog = new ConversionLog();
                //    objconLog.Pageno = pageno;
                //    objconLog.Nodedeleted = "1";
                //    lstConversionLog.Add(objconLog);
                //}
                //Session["lstConversionLog"] = lstConversionLog;
                ////Editing Loging Logic Ends here by Khail
            }
            else if (operation == "image")
            {
                int imageCount = objGlobal.PBPDocument.SelectNodes("//ln[@page='" + lnNode.Attributes["page"].Value + "']/ancestor::image").Count;
                string imageText = "Image_" + lnNode.Attributes["page"].Value + "_" + imageCount;
                lnNode.InnerXml = imageText;
                XmlElement imageElem = objGlobal.PBPDocument.CreateElement("image");
                XmlElement elemCap = tgNode.OwnerDocument.CreateElement("caption");
                XmlElement elemvDes = tgNode.OwnerDocument.CreateElement("voice-description");
                XmlElement elemLine = tgNode.OwnerDocument.CreateElement("ln");
                for (int i = 0; i < lnNode.Attributes.Count; i++)
                {
                    elemLine.SetAttribute(lnNode.Attributes[i].Name, lnNode.Attributes[i].Value);
                }
                imageElem.AppendChild(elemCap);
                imageElem.AppendChild(elemvDes);
                elemLine.InnerText = imageText;
                elemCap.AppendChild(elemLine);
                imageElem.SetAttribute("id", imageText);
                imageElem.SetAttribute("image-url", "Resources\\" + imageText + ".jpg");
                tgNode.ParentNode.InsertBefore(imageElem, tgNode);
            }
            else if (operation == "image_mod")
            {
                tgNode.Attributes["image-url"].Value = this.txtImageURL.Text;
                if (this.txtImageCaption.Text != "" && tgNode.SelectSingleNode(".//caption") == null)
                {
                    XmlElement elemCap = tgNode.OwnerDocument.CreateElement("caption");
                    tgNode.AppendChild(elemCap);
                }
                if (tgNode.SelectSingleNode(".//caption/ln") == null)
                {
                    lnNode.InnerText = "";
                    lnNode.InnerText = this.txtImageCaption.Text;
                    tgNode.SelectSingleNode(".//caption").AppendChild(lnNode);
                }
                else
                {
                    tgNode.SelectSingleNode(".//caption/ln").InnerText = this.txtImageCaption.Text;
                }
            }
            if (tableList.Count > 0)
            {
                string[] tableName = null;
                for (int i = 0; i < tableList.Count; i++)
                {
                    tableName = tableList[i].Attributes["id"].Value.ToString().Split(new char[] { '_' });
                    tableText = tableName[0] + "_" + tableName[1] + "_" + (i + 1);
                    tableList[i].Attributes["id"].Value = tableText;
                    tableList[i].SelectSingleNode(".//ln").InnerXml = tableText;
                }
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
            LoadTree(txtCurrentPage.Text.Trim());
            //ShowPDF();
        }

        public void CookieMaintinance(string Comments)
        {
            string bookID = Request.QueryString["bid"].ToString();
            if (Request.Cookies[bookID] == null)
            {
                HttpCookie httpCookie = new HttpCookie(bookID);
                httpCookie.Value = Comments;
                Response.Cookies.Add(httpCookie);
            }
            else
            {
                Response.Cookies[bookID].Value = Request.Cookies[bookID].Value + Comments;
            }
            // this.pnlIssue.Visible = false;

            //btnNext_Click(null, null);

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
            string nodeVal = TreeView1.CheckedNodes[0].Value;
            string parentName = TreeView1.CheckedNodes[0].Text;

            XmlNode currentNode = GetTargetXmlNode(nodeVal, parentName).ParentNode; ;
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
                if (TreeView1.CheckedNodes.Count > 0)
                {

                    TreeNode tn = TreeView1.CheckedNodes[0];
                    bool canAdd = true;
                    if (tn != null && (tn.Text == "table" || tn.Text == "upara" || tn.Text == "upara(cont..)" || tn.Text == "spara" || tn.Text == "npara" || tn.Text == "box" || tn.Text == "image" || tn.Text == "level1" || tn.Text == "level2" || tn.Text == "level3" || tn.Text == "level4" || tn.Text == "chapter" || tn.Text == "part" || tn.Text == "box" || tn.Text == "page" || tn.Text == "pre-section" || tn.Text == "post-section"))
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
                        if (tn.Text == "level1" || tn.Text == "level2" || tn.Text == "level3" || tn.Text == "level4" || tn.Text == "chapter" || tn.Text == "part" || tn.Text == "pre-section" || tn.Text == "post-section")
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
                        else if (tn.Text == "page")
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
                            //TreeNode uparaTNode = new TreeNode("upara");
                            //uparaTNode.Tag = upara;
                            //treeView1.Nodes[0].Nodes.Insert(treeView1.SelectedNode.Index + 1, uparaTNode);
                            //UpdateTreeNode(uparaTNode, true);
                            //treeView1.SelectedNode = uparaTNode;
                            //treeView1.ExpandAll();
                            //currentNode = treeView1.SelectedNode.Tag as XmlNode;
                            //PreOrPostSection = CheckSectionType(currentNode).Name;
                        }
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
                            LoadTree(txtCurrentPage.Text.Trim());
                            //ShowPDF();
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
                                LoadTree(txtCurrentPage.Text.Trim());
                                //ShowPDF();
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
                                        LoadTree(txtCurrentPage.Text.Trim());
                                        //ShowPDF();
                                    }
                                    else
                                    {
                                        existingNode.ParentNode.InsertAfter(section, existingNode);
                                        currentNode.ParentNode.RemoveChild(currentNode);
                                        objGlobal.SaveXml();
                                        LoadTree(txtCurrentPage.Text.Trim());
                                        //ShowPDF();
                                    }
                                }
                                else
                                {
                                    currentParentNode.ParentNode.InsertAfter(section, currentParentNode);
                                    currentNode.ParentNode.RemoveChild(currentNode);
                                    objGlobal.SaveXml();
                                    LoadTree(txtCurrentPage.Text.Trim());
                                    //ShowPDF();
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
                        LoadTree(txtCurrentPage.Text.Trim());
                        //ShowPDF();
                    }
                    else
                    {
                        currentParentNode.ParentNode.ParentNode.InsertAfter(section, currentParentNode.ParentNode);
                        currentNode.ParentNode.RemoveChild(currentNode);
                        objGlobal.SaveXml();
                        LoadTree(txtCurrentPage.Text.Trim());
                        //ShowPDF();
                    }
                }
                //section.ParentNode.InnerXml = section.ParentNode.InnerXml.Replace(section.OuterXml, "</body>" + section.OuterXml + "<body id=\"1\">");

                //GlobalVar.PBPDocument.LoadXml(objOuterXml);


                //section.AppendChild(head);
                //XmlElement body = GlobalVar.PBPDocument.CreateElement("body");
                //body.SetAttribute("id", "1");
                //section.AppendChild(body);
                //currentNode.ParentNode.InsertBefore(section, currentNode);
                //currentNode.ParentNode.RemoveChild(currentNode);
                //showXMLTree(currentPage);
            }

            else
            {
                lblMessage.Text = "Right Click on Upara to add Section.";
                ScriptManager.RegisterStartupScript(this.Page, GetType(), "MessageBox", "showMessageDiv();", true);
            }

        }
        private void loadLatestXml(string email)
        {
            try
            {
                string xmlFile = "";
                if (email != "")
                {
                    //xmlFile = objMyDBClass.MainDirPhyPath + "/Tests/" + email + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".rhyw";
                    xmlFile = objMyDBClass.MainDirPhyPath + "/Tests/" + email + "/" + Convert.ToString(Session["TestName"]) + "/" + Convert.ToString(Session["TestName"]) + ".rhyw";
                }
                else
                {
                    //xmlFile = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".rhyw";
                    xmlFile = objMyDBClass.MainDirPhyPath + "/" + Convert.ToString(Session["TestName"]) + "/" + Convert.ToString(Session["TestName"]) + ".rhyw";
                }
                objGlobal.XMLPath = xmlFile;
                Session["XMLPath"] = objGlobal.XMLPath;
                objGlobal.PBPDocument = new System.Xml.XmlDocument();
                objGlobal.LoadXml();
                Session["PBPDocument"] = objGlobal.PBPDocument;
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
            }
        }

        #endregion

        protected void TreeView1_TreeNodeDataBound(object sender, TreeNodeEventArgs e)
        {
            try
            {
                if (e.Node.Value.Contains("NotEmbeded"))
                {
                    e.Node.Text = "<div style=\" background:red; font-weight:bold;\">" + e.Node.Text + "</div>";
                }
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
            }
        }




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text.RegularExpressions;
using System.IO;
using System.Drawing;
using System.Collections;

namespace Outsourcing_System
{
    public partial class Finalize : System.Web.UI.Page
    {

        #region |InlineMarkUp Section|

        #region |Fields and Propertis|
        XmlDocument XmlDoc = new XmlDocument();
        GlobalVar objGlobal = new GlobalVar();
        MyDBClass objMyDBClass = new MyDBClass();
        CommonClass objCommonClass = new CommonClass();
        #endregion


        #region |Events|

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                loadLatestXml();
            }
            XmlDoc = (XmlDocument)Session["PBPDocument"];

        }
        protected void btnInlineMarkUp_Click(object sender, EventArgs e)
        {
            divInlineMarkUp.Visible = true;
            divPageBreak.Visible = false;
            mainProcess();
        }
        protected void treeMarkUps_SelectedNodeChanged(object sender, EventArgs e)
        {
            showNodeinDiv(0);
        }
        protected void btnCreateInline_Click(object sender, EventArgs e)
        {

            string selectedText = hFSelectedText.Value;
            string url = Regex.Match(selectedText, "<a[\\w\\W]*</a>").Value;
            if (url != "")
            {
                url = Regex.Replace(url, "<a[\\w\\W]*\">", "").Replace("</a>", "");
            }
            int indexofSelectedNode = treeMarkUps.Nodes.IndexOf(treeMarkUps.SelectedNode);
            TreeNode selectedNode = treeMarkUps.SelectedNode;
            XmlNode lnNode = null;
            XmlNodeList inlineNodes = XmlDoc.SelectNodes(@"//inline-markup");
            foreach (XmlNode node in inlineNodes)
            {
                if (node.InnerText.Contains(url))
                {
                    lnNode = node.ParentNode;
                    break;
                }
            }

            if (lnNode.NextSibling != null && (lnNode.InnerXml.Contains(lnNode.NextSibling.InnerXml)))
            {

                if (lnNode.NextSibling.NextSibling != null && (lnNode.InnerXml.Contains(lnNode.NextSibling.NextSibling.InnerXml)))
                {
                    lnNode.ParentNode.RemoveChild(lnNode.NextSibling.NextSibling);
                }
                lnNode.ParentNode.RemoveChild(lnNode.NextSibling);
            }
            //string innerXml = Regex.Replace(lnNode.InnerXml, "</?inline-markup.*?>", "");

            string innerXml = lnNode.InnerXml;
            innerXml = innerXml.Replace("<inline-markup type=\"url\">", "").Replace("</inline-markup>", "");
            selectedText = Regex.Replace(Regex.Replace(selectedText, "<a[\\w\\W]*\">", ""), "</a>", "");
            innerXml = innerXml.Replace(selectedText, "<inline-markup type=\"url\">" + selectedText + "</inline-markup>");

            lnNode.InnerXml = innerXml;
            treeMarkUps.SelectedNode.Value = lnNode.InnerXml;
            Session["PBPDocument"] = XmlDoc;
            saveChanges();
            mainProcess();
            showNodeinDiv(indexofSelectedNode);
        }
        protected void btnFind_Click(object sender, EventArgs e)
        {
            treeMarkUps.Nodes.Clear();
            if (txtSearch.Text != "")
            {
                XmlNodeList lnNodeList = XmlDoc.SelectNodes("//ln");
                TreeNode rootNode = new TreeNode("Main Book");
                foreach (XmlNode ln in lnNodeList)
                {
                    string searchingText = txtSearch.Text.Trim().Replace(".", "\\.").Replace("?", "\\?");
                    MatchCollection txtSearchCol = Regex.Matches(ln.InnerXml, searchingText);
                    if (txtSearchCol.Count > 0)
                    {
                        TreeNode templnNode = new TreeNode("Page No : " + ln.Attributes["page"].Value);
                        templnNode.Value = ln.InnerXml;
                        MatchCollection txtInlineMarkupCol = Regex.Matches(ln.InnerXml, "<inline-markup.*?>");
                        if (txtInlineMarkupCol.Count == txtSearchCol.Count)
                        {
                            templnNode.Text = "<font color='Navy'> " + templnNode.Text + "</font>";
                        }
                        else
                        {
                            templnNode.Text = "<font color='Orange'> " + templnNode.Text + "</font>";
                        }
                        rootNode.ChildNodes.Add(templnNode);
                    }
                }
                if (rootNode.ChildNodes.Count > 0)
                {
                    treeMarkUps.Nodes.Add(rootNode);
                    treeMarkUps.ExpandAll();
                }
                else
                {
                    CommonClass.ShowMessage("Sorry! No URL found");
                }
            }
            else
            {
                CommonClass.ShowMessage("Please Enter URL to search");
            }
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            txtSearch.Text = "";
            treeMarkUps.Nodes.Clear();
            divText.InnerText = "";
        }

        #endregion


        #region |Methods|

        private void loadLatestXml()
        {
            string xmlFile = objMyDBClass.MainDirPhyPath + "/" + Request.QueryString["bid"].ToString() + "/" + Request.QueryString["bid"].ToString() + ".rhyw";
            objGlobal.XMLPath = xmlFile;
            Session["XMLPath"] = objGlobal.XMLPath;
            objGlobal.PBPDocument = new System.Xml.XmlDocument();
            objGlobal.LoadXml();
            Session["PBPDocument"] = objGlobal.PBPDocument;
            XmlDoc = objGlobal.PBPDocument;
        }
        private void mainProcess()
        {
            treeMarkUps.Nodes.Clear();
            TreeNode rootNode = new TreeNode("Main Book");

            string pattern = "(KKK)?((http|ftp|file)://)?([a-z0-9-_.]*)?(www\\.|[-.a-z0-9]*@)[-a-z0-9 ]*\\.([\\./a-zA-Z0-9-_?=])*(QQQ)?";
            Regex url = new Regex(pattern, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            string Tempxml = XmlDoc.OuterXml;
            MatchCollection tempcole = Regex.Matches(Tempxml, pattern);
            for (int i = 0; i < tempcole.Count; i++)
            {
                Tempxml = Tempxml.Replace(tempcole[i].Value, "<inline-markup type=\"url\">" + tempcole[i].Value + "</inline-markup>");

            }

            XmlNodeList lnNodeList = XmlDoc.SelectNodes("//ln");

            foreach (XmlNode ln in lnNodeList)
            {
                ln.InnerXml = Regex.Replace(ln.InnerXml, "</?A.*?>", "");
                if (Regex.Match(ln.InnerXml, pattern).Success)
                {
                    ln.InnerXml = Regex.Replace(ln.InnerXml, "<A[ ]href=.*?>", "<inline-markup type=\"url\">");
                    ln.InnerXml = Regex.Replace(ln.InnerXml, "</A>", "</inline-markup>");
                    string NodeText = "";
                    MatchCollection matches = url.Matches(ln.InnerXml);
                    for (int i = 0; i < matches.Count; i++)
                    {
                        NodeText = matches[i].Value;

                        XmlNode inlineMarkup = ln.SelectSingleNode("inline-markup");
                        if (inlineMarkup != null && (!ln.InnerXml.Contains("<inline-markup type=\"url\">" + NodeText + "</inline-markup>")))
                        {
                            ln.InnerXml = ln.InnerXml.Replace(NodeText, "<inline-markup type=\"url\">" + NodeText + "</inline-markup>");
                            Session["PBPDocument"] = XmlDoc;
                            saveChanges();
                        }
                        else if (inlineMarkup == null)
                        {
                            ln.InnerXml = ln.InnerXml.Replace(NodeText, "<inline-markup type=\"url\">" + NodeText + "</inline-markup>");
                            Session["PBPDocument"] = XmlDoc;
                            saveChanges();
                        }

                    }

                    TreeNode templnNode = new TreeNode(NodeText);
                    ////
                    if (ln.NextSibling != null)
                    {
                        if (!ln.InnerXml.Contains(ln.NextSibling.InnerXml))
                        {
                            ln.InnerXml = ln.InnerXml + ln.NextSibling.InnerXml;
                            if (ln.NextSibling.NextSibling != null)
                            {
                                if (!ln.InnerXml.Contains(ln.NextSibling.NextSibling.InnerXml))
                                {
                                    ln.InnerXml = ln.InnerXml + ln.NextSibling.NextSibling.InnerXml;
                                }
                            }
                        }
                    }
                    ////

                    templnNode.Value = ln.InnerXml;
                    MatchCollection txtInlineMarkupCol = Regex.Matches(ln.InnerXml, "<inline-markup.*?>");
                    if (txtInlineMarkupCol.Count > 0)
                    {
                        templnNode.Text = "<font color='Navy'> " + templnNode.Text + "</font>";
                    }
                    else
                    {
                        templnNode.Text = "<font color='orange'> " + templnNode.Text + "</font>";
                    }
                    if (templnNode.Text.Length > 35)
                    {
                        templnNode.Text = templnNode.Text.Remove(30, templnNode.Text.Length - 30);
                        templnNode.Text = templnNode.Text + "....";
                    }
                    treeMarkUps.Nodes.Add(templnNode);
                }
            }

            if (treeMarkUps.Nodes.Count > 0)
            {
                treeMarkUps.Nodes.AddAt(0, rootNode);
                treeMarkUps.ExpandAll();
            }
        }
        private void saveChanges()
        {
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();                 
            }
            if (Session["PBPDocument"] != null)
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            if (File.Exists(objGlobal.XMLPath))
            {
                File.Copy(objGlobal.XMLPath, objGlobal.XMLPath.Replace(".rhyw", "Compared.rhyw"));
            }
            objGlobal.SaveXml();
        }
        private void showNodeinDiv(int index)
        {
            if (index == 0)
            {
                TreeNode selectedNode = treeMarkUps.SelectedNode;
                string pattren = @"<inline-markup[\w\W]*</[\w\W]*>";
                MatchCollection mathces = Regex.Matches(selectedNode.Value, pattren);
                divText.InnerHtml = selectedNode.Value;
                foreach (Match item in mathces)
                {
                    string value = item.Value.Replace("<inline-markup type=\"url\">", "").Replace("</inline-markup>", "");
                    divText.InnerHtml = divText.InnerHtml.Replace(item.Value, "<a style=\"color:blue;\">" + value + "</a>");
                }
            }
            else
            {
                TreeNode selectedNode = treeMarkUps.Nodes[index];
                string pattren = @"<inline-markup[\w\W]*</[\w\W]*>";
                MatchCollection mathces = Regex.Matches(selectedNode.Value, pattren);
                divText.InnerHtml = selectedNode.Value;
                foreach (Match item in mathces)
                {
                    string value = item.Value.Replace("<inline-markup type=\"url\">", "").Replace("</inline-markup>", "");
                    divText.InnerHtml = divText.InnerHtml.Replace(item.Value, "<a style=\"color:blue;\">" + value + "</a>");
                }
            }
        }

        #endregion

        #endregion

        #region |Page Break Section|

        #region |Fields and Properties|
        int startNumeber = 1;
        #endregion

        #region |Events|
        protected void btnPageBreaks_Click(object sender, EventArgs e)
        {
            divPageBreak.Visible = true;
            divInlineMarkUp.Visible = false;
            BuildPageBreakTree();
            Session["PBPDocument"] = XmlDoc;
        }

        protected void btnSaveChanges_Click(object sender, EventArgs e)
        {
            Session["PBPDocument"] = XmlDoc;
            saveChanges();
        }
        protected void btnPageBreakCancel_Click(object sender, EventArgs e)
        {
            divFinalize.Visible = false;
            divInlineMarkUp.Visible = false;
            divPageBreak.Visible = false;
        }
        protected void btnDeletePageBreak_Click(object sender, EventArgs e)
        {
            bool updated = false;
            loadLatestXml();
            foreach (TreeNode node in treePageBreaks.Nodes[0].ChildNodes)
            {
                if (node.Checked)
                {
                    updated = deletePageBreak(node);
                }
            }

            if (updated)
            {
                BuildPageBreakTree();
            }
        }
        protected void btnAddPageBreaks_Click(object sender, EventArgs e)
        {
            bool updated = false;
            loadLatestXml();
            foreach (TreeNode tNode in treePageBreaks.Nodes[0].ChildNodes)
            {
                if (tNode.Checked)
                {
                    AddPageBreak(tNode);
                    updated = true;
                }
            }
            if (updated)
            {
                BuildPageBreakTree();
            }
        }
        #endregion

        #region |Methods|
        public void orederPageBreaks(int startPageNo, XmlNodeList breakList)
        {
            for (int i = 0; i < breakList.Count; i++)
            {
                XmlNode node = breakList[i];
                breakList[i].Attributes["num"].Value = startNumeber.ToString();
                startNumeber = startNumeber + 1;
            }
        }
        public void BuildPageBreakTree()
        {
        reBuild:
            treePageBreaks.Nodes.Clear();
            TreeNode rootNode = new TreeNode("Page Breaks");
            XmlNodeList breakList = XmlDoc.SelectNodes("//section[@type=\"chapter\"]//break[@type=\"page\"]|//section[@type=\"part\"]//break[@type=\"page\"]|//section[@type=\"book\"]//break[@type=\"page\"]|//post-section//break[@type=\"page\"]");
            int startpoint = 1;
            if (breakList.Count > 0)
            {
                startpoint = int.Parse(breakList[0].Attributes["num"].Value.ToString());
                startNumeber = startpoint;
            }
            for (int i = 0; i < breakList.Count; i++)
            {
                TreeNode tempTNode = new TreeNode();
                int pageNo = int.Parse(breakList[i].Attributes["num"].Value.ToString());
                if (i + 1 <= breakList.Count - 1 && pageNo > int.Parse(breakList[i + 1].Attributes["num"].Value.ToString()))
                {
                    XmlNode lnNode = breakList[i].SelectSingleNode("preceding-sibling::ln");
                    divPageBreakResult.InnerHtml = divPageBreakResult.InnerHtml + "All Misorders are Resolved.</br>";
                    orederPageBreaks(startpoint, breakList);
                    goto reBuild;
                }
                if (i + 1 <= breakList.Count - 1 && pageNo == int.Parse(breakList[i + 1].Attributes["num"].Value.ToString()))
                {
                    XmlNode lnNode = breakList[i].SelectSingleNode("preceding-sibling::ln");
                    divPageBreakResult.InnerHtml = divPageBreakResult.InnerHtml + "A duplicate Page Break " + breakList[i + 1].Attributes["num"].Value.ToString() + " is removed at Page # " + breakList[i + 1].Attributes["num"].Value.ToString() + "</br>";
                    breakList[i + 1].ParentNode.RemoveChild(breakList[i + 1]);
                    goto reBuild;
                }
               
                string nodeTitle = "";
                if (pageNo == (i + startpoint))
                {
                    nodeTitle = pageNo.ToString();
                    tempTNode.Text = nodeTitle;
                    tempTNode.Value = breakList[i].OuterXml;
                    rootNode.ChildNodes.Add(tempTNode);
                }
                else
                {
                    nodeTitle = (i + startpoint).ToString();
                    tempTNode.Text = nodeTitle + " - Missed";
                    tempTNode.Text = "<font color='Orange'> " + tempTNode.Text + "</font>";
                    rootNode.ChildNodes.Add(tempTNode);
                    startpoint = startpoint + 1;
                    i--;
                }

            }
            treePageBreaks.Nodes.Add(rootNode);
            treePageBreaks.ExpandAll();
        }

        private bool deletePageBreak(TreeNode sNode)
        {

            int NodeIndex = treePageBreaks.Nodes[0].ChildNodes.IndexOf(sNode);
            XmlNodeList PageBreaks = XmlDoc.SelectNodes("//break");

            foreach (XmlNode node in PageBreaks)
            {
                if (node.OuterXml.Equals(sNode.Value))
                {
                    node.ParentNode.RemoveChild(node);
                    Session["PBPDocument"] = XmlDoc;
                    saveChanges();
                    return true;
                }
            }
            return false;


        }
        private void AddPageBreak(TreeNode sNode)
        {
            int NodeIndex = treePageBreaks.Nodes[0].ChildNodes.IndexOf(sNode);
            XmlNodeList PageBreaks = XmlDoc.SelectNodes("//break");
            TreeNode preTNode = treePageBreaks.Nodes[0].ChildNodes[NodeIndex - 1];
            TreeNode postTNode = treePageBreaks.Nodes[0].ChildNodes[NodeIndex + 1];
            if (preTNode.Value != null)
            {
                if (preTNode.Value != null)
                {
                    XmlNode prevXNode = null;
                    if (preTNode != null)
                    {
                        foreach (XmlNode node in PageBreaks)
                        {
                            if (node.OuterXml.Equals(preTNode.Value))
                            {
                                prevXNode = node;
                                break;
                            }
                        }
                    }
                    XmlNode postXNode = null;
                    if (postTNode != null)
                    {
                        foreach (XmlNode node in PageBreaks)
                        {
                            if (node.OuterXml.Equals(postTNode.Value))
                            {
                                postXNode = node;
                                break;
                            }
                        }
                    }

                    int prevPage = prevXNode != null ? int.Parse(prevXNode.Attributes["num"].Value.ToString()) : 0;
                    int postPage = postXNode != null ? int.Parse(postXNode.Attributes["num"].Value.ToString()) : 0;
                    if (prevXNode.NextSibling == null)
                    {
                        if (postPage - prevPage >= 2 || postPage - prevPage < 0)
                        {
                            XmlElement breakNode = XmlDoc.CreateElement("break");
                            breakNode.SetAttribute("type", "page");
                            breakNode.SetAttribute("num", (prevPage + 1).ToString());
                            breakNode.SetAttribute("id", prevXNode.Attributes["id"].Value.ToString());
                            sNode.Text = sNode.Text.Replace(" - Missed", "");
                            sNode.Text = "<font color='Navy'> " + sNode.Text + "</font>";
                            sNode.Value = breakNode.OuterXml;
                            prevXNode.ParentNode.InsertAfter(breakNode, prevXNode);
                            objGlobal.PBPDocument = XmlDoc;
                            saveChanges();
                        }
                    }
                    else
                    {
                        XmlNodeList followingNodes = prevXNode.SelectNodes("following-sibling::ln|following-sibling::break");

                        XmlElement breakNode = XmlDoc.CreateElement("break");
                        breakNode.SetAttribute("type", "page");
                        breakNode.SetAttribute("num", (prevPage + 1).ToString());
                        breakNode.SetAttribute("id", prevXNode.Attributes["id"].Value.ToString());

                        bool isAdd = false;
                        for (int j = 0; j < followingNodes.Count; j++)
                        {
                            if (followingNodes[j].Name == "ln" || followingNodes[j].Name == "break")
                            {
                                int number = followingNodes[j].Attributes["page"] != null ? int.Parse(followingNodes[j].Attributes["page"].Value.ToString()) : 0;
                                if (int.Parse(prevXNode.Attributes["id"].Value.ToString()) + 1 != number)
                                {
                                    isAdd = true;
                                    sNode.Text = sNode.Text.Replace(" - Missed", "");
                                    sNode.Text = "<font color='Navy'> " + sNode.Text + "</font>";
                                    sNode.Value = breakNode.OuterXml;
                                    prevXNode.ParentNode.InsertBefore(breakNode, followingNodes[j]);
                                    objGlobal.PBPDocument = XmlDoc;
                                    saveChanges();
                                }
                                if (isAdd == false && j == followingNodes.Count - 1)
                                {
                                    sNode.Text = sNode.Text.Replace(" - Missed", "");
                                    sNode.Text = "<font color='Navy'> " + sNode.Text + "</font>";
                                    sNode.Value = breakNode.OuterXml;
                                    prevXNode.ParentNode.InsertAfter(breakNode, followingNodes[j]);
                                    objGlobal.PBPDocument = XmlDoc;
                                    saveChanges();
                                }
                            }
                        }

                    }
                }
            }
        }

        #endregion

        #endregion

        #region |Finalize Section|

        #region |Fields and Properties|

        #endregion

        #region |Events|
        protected void btnMessageOk_Click(object sender, EventArgs e)
        {

        }

        protected void btnFinalize_Click(object sender, EventArgs e)
        {
            divFinalize.Visible = true;
            divPageBreak.Visible = false;
            divInlineMarkUp.Visible = false;

        }

        protected void btnFinalizedProcess_Click(object sender, EventArgs e)
        {
            FinalizationProcess();
            updateComparisontoPending();
            divFinalizedResult.InnerHtml = divFinalizedResult.InnerHtml + "<span style=\" font-size:13pt; font-family:Tahoma; font-style:normal;color:green\">Finalization is Complete</span>";
        }
        protected void btnFinalizedOk_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminPanel.aspx", false);

        }
        #endregion



        #region |Methods|

        private void RemoveLogginTags()
        {
 
        }

        private void updateComparisontoPending()
        {
            string bookID = Request.QueryString["bid"].ToString();
            string totalPages = Request.QueryString["TotalPages"].ToString();
            if (Response.Cookies[bookID] != null)
            {
                string val = Request.Cookies[bookID].Value;
                string querySel = "Select AID from Activity Where BID=" + bookID + " and Task='Comparison'";
                string aid = objMyDBClass.GetID(querySel);
                string qUpCountComparison = "Update Activity Set [Count]=" + totalPages + " Where AID=" + aid;
                objMyDBClass.ExecuteCommand(qUpCountComparison);

                if (val != null && val != "")
                {
                    int count = 0;
                    //string FileName = Server.MapPath("~/Files/" + bookID + "/IssueLog.txt");
                    string FileName = objMyDBClass.MainDirPhyPath + "/" + bookID + "/IssueLog.txt";
                    string[] issues = val.Split(new char[] { '^' });
                    if (!File.Exists(FileName))
                    {
                        TextWriter tr = new StreamWriter(FileName);
                        tr.Close();
                    }
                    foreach (string issue in issues)
                    {
                        TextWriter tr = new StreamWriter(FileName, true);
                        tr.WriteLine(issue);
                        tr.Close();
                        ++count;
                    }
                    try
                    {
                        objMyDBClass.LogEntry(bookID, "Comparison", "Comparison completed. Waiting for approval", "In Progress", "insert");
                        string queryUpdate = "Update ACTIVITY Set Status='Pending Confirmation', CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "' Where AID=" + aid;
                        int upRes = objMyDBClass.ExecuteCommand(queryUpdate);
                        if (upRes > 0)
                        {
                            //string dirPath = Server.MapPath("Files/" + bookID + "/ErrorAdjustment");
                            string dirPath = objMyDBClass.MainDirPhyPath + "/" + bookID + "/ErrorAdjustment";
                            if (!Directory.Exists(dirPath))
                            {
                                Directory.CreateDirectory(dirPath);
                            }
                            Session.Remove("pno");
                            Response.Cookies[bookID].Expires = DateTime.Now.AddDays(-1);
                            //Response.Redirect("UserPanel.aspx", false);
                        }
                    }
                    catch (Exception ex)
                    {
                        this.lblMessage.Text = ex.Message;
                    }
                }
                else
                {
                    this.lblMessage.Text = "No issue found in the file";
                    string queryUpdate = "Update ACTIVITY Set Status='Pending Confirmation', CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "' Where AID=" + aid;
                    int upRes = objMyDBClass.ExecuteCommand(queryUpdate);
                    if (upRes > 0)
                    {
                        Session.Remove("pno");
                        Response.Cookies[bookID].Expires = DateTime.Now.AddDays(-1);
                        //   Response.Redirect("UserPanel.aspx", false);
                    }
                }
            }
        }
        private void FinalizationProcess()
        {
            loadLatestXml();
            XmlDoc.LoadXml(CommonAdjustment(XmlDoc.OuterXml));
            XmlNodeList paraList = XmlDoc.SelectNodes("//upara|//spara/line|//spara/para|//npara|//ln");
            foreach (XmlNode para in paraList)
            {
                if (para.Attributes["conversion-Operations"] != null)
                {
                    para.Attributes.Remove(para.Attributes["conversion-Operations"]);
                }                
            }
            RemovingBlankEmphasis(paraList);
            InsertingBlankSpaceInEmphasis(paraList);
            CheckingMultipleNumTagsInNpara(paraList);
            checkEmptyParas(paraList);
            RemovingBlackSpaceInEmphasis(paraList);
            RemovingBlankSpaceAfterEmphasis(paraList);
            XmlNode bookNoticesImageNode = XmlDoc.SelectSingleNode("//pbp-front/book-notices/body/image");
            checkbookNoticesImageMissing(bookNoticesImageNode);//, string path);
            XmlNode frontImageNode = XmlDoc.SelectSingleNode("//pbp-front/cover/image-model/front");
            checkFrontImageUrl(frontImageNode);

            //Section and Body Adjustment

            //completeFile = completeFile.Replace("KKK", "&lt;").Replace("QQQ", ">");

            XmlNodeList sectionListt = XmlDoc.SelectNodes("//section");
            foreach (XmlNode tempSection in sectionListt)
            {
                XmlNode bodyNode = tempSection.SelectSingleNode("body");
                if (bodyNode != null && bodyNode.ChildNodes.Count == 0 && tempSection.ParentNode.Name == "body")
                {
                    while (tempSection.NextSibling != null && tempSection.NextSibling.Name != "section" && tempSection.NextSibling.Name != "pre-section" && tempSection.NextSibling.Name != "post-section")
                    {
                        bodyNode.AppendChild(tempSection.NextSibling);
                    }
                }
            }
            sectionListt = XmlDoc.SelectNodes("//section|//pre-section|//post-section");
            foreach (XmlNode tempSection in sectionListt)
            {
                string currType = tempSection.Attributes["type"].Value.ToString();
                string nextSiblingType = "";
                if (tempSection.NextSibling != null && tempSection.NextSibling.Name == "section")
                {
                    nextSiblingType = tempSection.NextSibling.Attributes["type"].Value.ToString();
                    if ((currType == "part" || currType == "book") && nextSiblingType == "chapter")
                    {
                        tempSection.AppendChild(tempSection.NextSibling);
                    }
                    if ((currType == "chapter" || currType == "pre-section" || currType == "post-section") && nextSiblingType == "level1")
                    {
                        tempSection.AppendChild(tempSection.NextSibling);
                    }
                    if (currType == "level1" && nextSiblingType == "level2")
                    {
                        tempSection.AppendChild(tempSection.NextSibling);
                    }
                    if (currType == "level2" && nextSiblingType == "level3")
                    {
                        tempSection.AppendChild(tempSection.NextSibling);
                    }
                    if (currType == "level3" && nextSiblingType == "level4")
                    {
                        tempSection.AppendChild(tempSection.NextSibling);
                    }
                }
            }

            XmlNodeList headList = XmlDoc.SelectNodes("//head");
            foreach (XmlNode headNode in headList)
            {
                XmlNode prefix = headNode.SelectSingleNode("prefix");
                XmlNode PREFIX = headNode.SelectSingleNode(".//PREFIX");
                if (PREFIX != null)
                {
                    prefix.InnerXml = PREFIX.InnerXml;
                    XmlNodeList PREFIXList = headNode.SelectNodes(".//PREFIX");
                    foreach (XmlNode tempPrefix in PREFIXList)
                    {
                        tempPrefix.ParentNode.RemoveChild(tempPrefix);
                    }
                }
                XmlNode sectTitle = headNode.SelectSingleNode("section-title");
                if (sectTitle != null)
                {
                    sectTitle.InnerText = sectTitle.InnerText.Trim();
                }
                XmlNode brailleHeader = headNode.SelectSingleNode("braille-header");
                if (brailleHeader != null)
                {
                    brailleHeader.InnerText = brailleHeader.InnerText.Trim();
                }
                XmlNode runningHeader = headNode.SelectSingleNode("running-header");
                if (runningHeader != null)
                {
                    runningHeader.InnerText = runningHeader.InnerText.Trim();
                }
            }
            //Deleting Inner Inline Markup
            XmlNodeList inlineList = XmlDoc.SelectNodes("//inline-markup");
            foreach (XmlNode inNode in inlineList)
            {
                inNode.InnerText = Regex.Replace(inNode.InnerXml, "</?inline-markup.*?>", "");
            }

            //Separating Pre and Post Section
            XmlNodeList prepostSectionList = XmlDoc.SelectNodes("//pre-section");
            if (prepostSectionList.Count > 0)
            {
                XmlNode pbpFront = XmlDoc.SelectSingleNode("//pbp-front");
                foreach (XmlNode tempNode in prepostSectionList)
                {
                    //Adding SigBlock 
                    XmlNode sigBlok = tempNode.SelectSingleNode("sigblock");
                    if (sigBlok == null)
                    {
                        XmlElement sigBlock = XmlDoc.CreateElement("sigblock");
                        XmlElement name = XmlDoc.CreateElement("name");
                        sigBlock.AppendChild(name);
                        tempNode.AppendChild(sigBlock);
                    }
                    pbpFront.AppendChild(tempNode);
                }
            }
            prepostSectionList = XmlDoc.SelectNodes("//post-section");
            if (prepostSectionList.Count > 0)
            {
                XmlNode pbpEnd = XmlDoc.SelectSingleNode("//pbp-end");
                foreach (XmlNode tempNode in prepostSectionList)
                {
                    //Adding SigBlock 
                    XmlNode sigBlok = tempNode.SelectSingleNode("sigblock");
                    if (sigBlok == null)
                    {
                        XmlElement sigBlock = XmlDoc.CreateElement("sigblock");
                        XmlElement name = XmlDoc.CreateElement("name");
                        sigBlock.AppendChild(name);
                        tempNode.AppendChild(sigBlock);
                    }
                    pbpEnd.AppendChild(tempNode);
                }
            }
            //Separating Section Break
            XmlNodeList sectionBreakList = XmlDoc.SelectNodes("//section-break");
            foreach (XmlNode sectNode in sectionBreakList)
            {
                XmlElement uparaElem = XmlDoc.CreateElement("upara");
                uparaElem.SetAttribute("id", "0");
                uparaElem.SetAttribute("pnum", "0");
                uparaElem.InnerText = sectNode.InnerText;
                sectNode.ParentNode.ParentNode.InsertAfter(uparaElem, sectNode.ParentNode);
                sectNode.ParentNode.RemoveChild(sectNode);
            }
            //**************************************************************************
            //Emphasis url Problem solution
            EmphasisCorrection(XmlDoc);
            OtherCorrection(XmlDoc);
            //**************************************************************************

            objGlobal.PBPDocument = XmlDoc;
            saveChanges();
            //try { XmlDoc.LoadXml(CommonClass.LoadEncryptedXmlFile(CommonClass.xmlFilePath)); }catch { CommonClass.ShowMessage("Cannot Load file"); }
            XmlNode versionNode = XmlDoc.SelectSingleNode("//Version");
            if (versionNode != null)
            {
                versionNode.InnerText = "2";
                if (versionNode.InnerText == "2")
                {
                    //To set the Validation-passed
                    XmlNode validationPassedNode = XmlDoc.SelectSingleNode("//Validation-passed");
                    validationPassedNode.InnerText = "2";
                }
            }

            //To set the content-correction
            XmlNode contentCorrectPassed = XmlDoc.SelectSingleNode("//content-correction");
            if (contentCorrectPassed != null)
            {
                string isPassed = contentCorrectPassed.Attributes["isPassed"].Value;
                if (isPassed == "false")
                {
                    bool isTrue = true;
                    XmlNodeList errorPassedList = contentCorrectPassed.SelectNodes("Error-not-Passed");
                    foreach (XmlNode error in errorPassedList)
                    {
                        int count = int.Parse(error.Attributes["count"].Value);
                        int passed = int.Parse(error.Attributes["passed"].Value);
                        if (count != passed)
                        {
                            isTrue = false;
                            break;
                        }
                    }
                    if (isTrue == true)
                    {
                        contentCorrectPassed.Attributes["isPassed"].Value = "true";
                    }
                }
            }

            //To set the Preview-passed 
            XmlNodeList lnTmpNodeList = XmlDoc.SelectNodes("//ln");
            bool isPreviewPassed = true;
            foreach (XmlNode lnNode in lnTmpNodeList)
            {
                if (lnNode.Attributes["ispreviewpassed"].Value == "false")
                {
                    isPreviewPassed = false;
                    break;
                }
            }
            if (isPreviewPassed == true)
            {
                XmlNode previewPassed = XmlDoc.SelectSingleNode("//Preview-passed");
                if (previewPassed != null)
                {
                    previewPassed.InnerText = "1";
                }
            }

            //To Summarize up
            XmlNode runningBraille = XmlDoc.SelectSingleNode("//running-braille-passed");
            XmlNode inlineMarkup = XmlDoc.SelectSingleNode("//inline-markup-passed");
            XmlNode valueAddPasses = XmlDoc.SelectSingleNode("//ValueAdd-passed");
            if (contentCorrectPassed != null)
            {
                if (contentCorrectPassed.Attributes["isPassed"].Value == "true" && inlineMarkup.InnerText == "1" && runningBraille.Attributes["pass"].Value == "true")
                {
                    valueAddPasses.Attributes["isPassed"].Value = "true";
                }
            }

            //XmlDoc.Save(CommonClass.xmlFilePath);
            objGlobal.PBPDocument = XmlDoc;
            saveChanges();
        }
        public void OtherCorrection(XmlDocument XMLDoc)
        {
        jump:
            XmlNodeList SecTitleNodeList = XMLDoc.SelectNodes("//section-title");
            foreach (XmlNode numNode1 in SecTitleNodeList)
            {
                if (numNode1.InnerXml.Contains("&lt;PREFIX&gt;") && numNode1.InnerXml.Contains("&lt;/PREFIX&gt;"))
                {
                    string prefix = Regex.Match(numNode1.InnerXml, "&lt;PREFIX&gt;.*?&lt;/PREFIX&gt;").Value;
                    numNode1.InnerXml = numNode1.InnerXml.Replace("&lt;PREFIX&gt;", "");
                    numNode1.InnerXml = numNode1.InnerXml.Replace("&lt;/PREFIX&gt;", "");
                    //numNode1.InnerXml = numNode1.InnerXml.Replace(prefix, "");
                }
            }
            //Removing Emphasis from <num>
            XmlNodeList numNodeList1 = XMLDoc.SelectNodes("//num");
            foreach (XmlNode numNode1 in numNodeList1)
            {
                if (Regex.IsMatch(numNode1.InnerXml, "<emphasis.*?>.*?</emphasis>"))
                {
                    string st1 = Regex.Match(numNode1.InnerXml, "<emphasis.*?>.*?</emphasis>").Value;
                    string st2 = Regex.Replace(st1, "</?emphasis.*?>", "");
                    numNode1.InnerXml = numNode1.InnerXml.Replace(st1, st2);
                    goto jump;
                }
            }
            //Removing Emphasis from inline-markup if inline markup come inside
            XmlNodeList empNodeList1 = XMLDoc.SelectNodes("//emphasis");
            foreach (XmlNode empNode1 in empNodeList1)
            {
                if (Regex.IsMatch(empNode1.InnerXml, "<inline-markup.*?>.*?</inline-markup>"))
                {
                    string st1 = Regex.Replace(empNode1.OuterXml, "</?emphasis.*?>", "");
                    empNode1.ParentNode.InnerXml = empNode1.ParentNode.InnerXml.Replace(empNode1.OuterXml, st1);
                    goto jump;
                }
            }
            //Converting <A> to <inline-markup>
            XmlNodeList anchorList = XMLDoc.SelectNodes("//A");
            foreach (XmlNode anNode1 in anchorList)
            {
                string st1 = Regex.Replace(anNode1.ParentNode.InnerXml, "<A.*?>", "<inline-markup type=\"url\">");
                st1 = Regex.Replace(st1, "</A>", "</inline-markup>");
                anNode1.ParentNode.InnerXml = st1;
            }
            //Removing Line inside para
            XmlNodeList paraList = XMLDoc.SelectNodes("//para/line");
            foreach (XmlNode paraNode in paraList)
            {
                string st = Regex.Replace(paraNode.ParentNode.InnerXml, "</?line>", "");
                paraNode.ParentNode.InnerXml = st;
            }
            //Removing para inside line
            XmlNodeList lineList = XMLDoc.SelectNodes("//line/para");
            foreach (XmlNode lineNode in lineList)
            {
                string st = Regex.Replace(lineNode.ParentNode.InnerXml, "</?para>", "");
                lineNode.ParentNode.InnerXml = st;
            }
            //Removing body inside box
            XmlNodeList bodyList = XMLDoc.SelectNodes("//box/body");
            foreach (XmlNode bodyNode in bodyList)
            {
                string st = Regex.Replace(bodyNode.ParentNode.InnerXml, "</?body>", "");
                bodyNode.ParentNode.InnerXml = st;
            }
            //Removing blank bodies
            XmlNodeList blankList = XMLDoc.SelectNodes("//body");
            foreach (XmlNode bodyNode in blankList)
            {
                if (bodyNode.ChildNodes.Count == 0 && bodyNode.InnerXml == "")
                {
                    string st = Regex.Replace(bodyNode.ParentNode.InnerXml, "<body id=\"1\"/>", "");
                    bodyNode.ParentNode.InnerXml = st;
                }
            }

        }
        public void EmphasisCorrection(XmlDocument XMLDoc)
        {
        jump:
            XmlNodeList empNodeList1 = XMLDoc.SelectNodes("//emphasis");
            foreach (XmlNode empNode1 in empNodeList1)
            {
                XmlNodeList empNodeList2 = empNode1.SelectNodes("emphasis");
                foreach (XmlNode empNode2 in empNodeList2)
                {
                    if (empNode1.Attributes["type"].Value == empNode2.Attributes["type"].Value)
                    {
                        empNode1.InnerXml = empNode1.InnerXml.Replace(empNode2.OuterXml, empNode2.InnerText);
                        goto jump;
                    }
                    else if (empNode1.Attributes["type"].Value != empNode2.Attributes["type"].Value)
                    {
                        string CompleteOuterXml = empNode1.ParentNode.InnerXml;
                        string oldText = empNode2.OuterXml;
                        string newText = oldText.Replace(empNode2.OuterXml, "</emphasis>" + empNode2.OuterXml + "<emphasis type=\"" + empNode1.Attributes["type"].Value + "\">");//.Replace("</emphasis>", " "));
                        newText = newText.Replace(empNode2.Attributes["type"].Value, "bold-italic");
                        CompleteOuterXml = CompleteOuterXml.Replace(oldText, newText);
                        empNode1.ParentNode.InnerXml = CompleteOuterXml;
                        goto jump;
                    }
                }
            }
        }


        /////////////////////////////////////////////





        //Method***************Common Cleaning Method*****************
        #region string CommonAdjustment(string completeFile)
        public string CommonAdjustment(string completeFile)
        {
            //completeFile = completeFile.Replace("&lt;", "<").Replace("&gt;", ">"); //Shoaib here: Removing this line, it causes error
            //////////////////////////////////////////////////////////
            //XmlDocument xmlD = new XmlDocument();
            //try
            //{
            //    xmlD.LoadXml(completeFile);
            //}
            //catch { }
            //xmlD.Clone();
            //////////////////////////////////////////////////////////
            completeFile = completeFile.Replace("&shy;", "");
            completeFile = Regex.Replace(completeFile, "<([0-9]+)", "&lt;$1").Replace("&", "&amp;").Replace("&amp;lt;", "&lt;");
            completeFile = Regex.Replace(completeFile, "(&amp;)+gt;", ">");
            completeFile = Regex.Replace(completeFile, "(&amp;)+lt;", "<");
            WriteInformation("Invalid Charecters Removed", Color.Red);

            MatchCollection lnNodeList = Regex.Matches(completeFile, "<ln[^/>]+?/>");
            foreach (Match lnNode in lnNodeList)
            {
                string text = lnNode.Value;
                completeFile = completeFile.Replace(lnNode.Value, "");
            }
            WriteInformation("Blank Lines Removed", Color.Green);

            lnNodeList = Regex.Matches(completeFile, "<ln.*?>.*?</ln>");
            foreach (Match lnNode in lnNodeList)
            {
                if (lnNode.Value.Trim() != "")
                {
                    string text = Regex.Replace(lnNode.Value, "</?ln.*?>", "");
                    MatchCollection empCol = Regex.Matches(text, "<.*?>|<[a-zA-Z \\[\\]]+|[a-zA-Z \\]\\[]+>", RegexOptions.Singleline | RegexOptions.IgnoreCase);
                    for (int m = 0; m < empCol.Count; m++)
                    {
                        if (!empCol[m].Value.Contains("emphasis") && !empCol[m].Value.Contains("break") && !empCol[m].Value.Contains("PREFIX") && !empCol[m].Value.Contains("num") && !empCol[m].Value.Contains("inline-markup") && !empCol[m].Value.Contains("untagged") && !empCol[m].Value.Contains("sub") && !empCol[m].Value.Contains("sup") && !empCol[m].Value.Contains("special-chars") && !empCol[m].Value.Contains("footnote"))
                        {
                            string newVal = empCol[m].Value.Replace("<", "&lt;").Replace(">", "&gt;");
                            completeFile = completeFile.Replace(empCol[m].Value, newVal);
                        }
                    }
                }
            }
            WriteInformation("Extra < or > Adjusted", Color.Green);

            completeFile = Regex.Replace(completeFile, "[ ]*<ln.*?>", "", RegexOptions.Singleline);
            completeFile = Regex.Replace(completeFile, "(\r\n)[ ]+<ln.*?>", "", RegexOptions.Singleline);
            completeFile = Regex.Replace(completeFile, "</[ ]?ln>(\r\n)[ ]+", "");
            completeFile = Regex.Replace(completeFile, "</[ ]?ln>+", " ");
            completeFile = Regex.Replace(completeFile, "(?<=</[ ]?emphasis>)(\r\n)[ ]+(?=<emphasis.*?)", " ");
            completeFile = Regex.Replace(completeFile, "(</[ ]?emphasis>)(<emphasis.*?>)", "$1 $2");
            completeFile = Regex.Replace(completeFile, "<process-attributes>.*?</process-attributes>", "", RegexOptions.Singleline);
            completeFile = Regex.Replace(completeFile, "type=\"pre-section\"", "type=\"other\"", RegexOptions.Singleline);
            completeFile = Regex.Replace(completeFile, "type=\"post-section\"", "type=\"other\"", RegexOptions.Singleline);
            completeFile = Regex.Replace(completeFile, "</?tbody.*?>", "", RegexOptions.Singleline);
            completeFile = Regex.Replace(completeFile, "&amp;&amp;", "&amp;", RegexOptions.Singleline);
            completeFile = Regex.Replace(completeFile, "&amp;amp;", "&amp;", RegexOptions.Singleline);
            WriteInformation("Extra Attributes are Removed", Color.Chocolate);

            completeFile = Regex.Replace(completeFile, "[ ][ ]+", " ", RegexOptions.Singleline);
            WriteInformation("Extra Spaces are Removed", Color.YellowGreen);

            completeFile = Regex.Replace(completeFile, "— ", "—", RegexOptions.Singleline);
            WriteInformation("Space are Removed after — ", Color.Pink);

            completeFile = Regex.Replace(completeFile, " —", "—", RegexOptions.Singleline);
            WriteInformation("Space are Removed before — ", Color.Pink);

            completeFile = Regex.Replace(completeFile, " -", "-", RegexOptions.Singleline);
            WriteInformation("Space are Removed before - ", Color.Purple);

            completeFile = Regex.Replace(completeFile, ">>", ">", RegexOptions.Singleline);
            completeFile = Regex.Replace(completeFile, "<<", "<", RegexOptions.Singleline);
            completeFile = Regex.Replace(completeFile, "([ ][a-zA-Z0-9]+)-[ ]?(<break.*?>)", "$2$1", RegexOptions.Singleline);
            WriteInformation("Merged Hyphenated Word at the End of Page", Color.Blue);

            //completeFile = Regex.Replace(completeFile, "([ ]?)(</[ ]?emphasis>)([ ]?)([”|\\.|;|:|,|’|!|?|'|\"|\\]|)])", "$4$2");
            //WriteInformation("Entered Punctuation after the Emphases to it", Color.Black);

            //completeFile = Regex.Replace(completeFile, "(“)(<emphasis.*?>)", "$2$1");
            //WriteInformation("Entered the Opening Quote into the Emphases", Color.Black);

            MatchCollection mCol = Regex.Matches(completeFile, "<emphasis.*?>.*?</emphasis>", RegexOptions.Singleline);
            foreach (Match m in mCol)
            {
                string value = m.Value;
                MatchCollection mCol2 = Regex.Matches(value, "(<emphasis.*?>)([ ]?)([;”\\.,’!?'\"\\])])");
                if (mCol2.Count > 0)
                {
                    string sentence = Regex.Replace(value, "(<emphasis.*?>)([ ]?)([;”\\.,’!?'\"\\])])", "$3$1");
                    completeFile = completeFile.Replace(value, sentence);
                }
            }
            WriteInformation("Out the Closing Punctuation From Emphasis", Color.Maroon);
            //Replace the words of Content Correction
            if (File.Exists(objGlobal.XMLPath.Replace(".rhyw", ".xml")))
            {
                XmlDocument tempXmlIssues = new XmlDocument();
                tempXmlIssues.Load(objGlobal.XMLPath.Replace(".rhyw", ".xml"));
                XmlNodeList issueList = tempXmlIssues.SelectNodes("//issue");
                foreach (XmlNode nod in issueList)
                {
                    string match = nod.SelectSingleNode("match").InnerText;
                    string repl = nod.SelectSingleNode("replace").InnerText;
                    completeFile = Regex.Replace(completeFile, match, repl);
                }
                tempXmlIssues = null;
            }
            WriteInformation("Content Correction Replacement Completed", Color.DarkOrange);
            //+++++++++++++++++++++++++++++++++++
            //Merging Emphasis
            completeFile = completeFile.Replace("<www", "www");
            completeFile = completeFile.Replace("<http", "http");
            completeFile = completeFile.Replace("< <", "<");
            completeFile = Regex.Replace(completeFile, "(<)([ ]*)([0-9]+)", "&lt;$2$3");
            //completeFile = Regex.Replace(completeFile, "(&lt;inline-markup type=\"url\">", "(<inline-markup type=\"url\">");

            XmlDocument xmlEmp = new XmlDocument();
            xmlEmp.LoadXml(completeFile);
        Emp:
            XmlNodeList joinEmphasisCol = xmlEmp.SelectNodes("//emphasis");
            foreach (XmlNode emphNode in joinEmphasisCol)
            {
                if (emphNode.NextSibling != null && emphNode.NextSibling.Name == "emphasis" && emphNode.Attributes["type"].Value == emphNode.NextSibling.Attributes["type"].Value)
                {
                    emphNode.InnerXml = emphNode.InnerXml + " " + emphNode.NextSibling.InnerXml;
                    emphNode.NextSibling.ParentNode.RemoveChild(emphNode.NextSibling);
                    goto Emp;
                }
            }
            WriteInformation("Emphasis are Merged", Color.DarkOrange);
            return xmlEmp.OuterXml;
        }
        #endregion

        //Method***************Writing Info to RichTextBox*****************
        #region WriteInformation(string information,Color colorName)
        public void WriteInformation(string information, Color colorName)
        {
            divFinalizedResult.InnerHtml = divFinalizedResult.InnerHtml + "<span style=\" font-size:8; font-family:Tahoma; font-style:normal;color:" + colorName.ToString().Replace("Color [", "").Replace("]", "") + "\">" + information + "</span></br>";
        }
        #endregion

        //Method*******************RemovingBlankEmphasis*******************
        #region RemovingBlankEmphasis(XmlNodeList paraList)
        public void RemovingBlankEmphasis(XmlNodeList paraList)
        {
            string descrip = "";
            int iTemp = 0;

            for (int i = 0; i < paraList.Count; i++)
            {
                iTemp = i;
                XmlNodeList emphasisNodesList = paraList[i].SelectNodes("descendant::emphasis");
                if (emphasisNodesList.Count > 0)
                {
                    for (int j = 0; j < emphasisNodesList.Count; j++)
                    {
                        if (emphasisNodesList[j].InnerText == string.Empty)
                        {
                            emphasisNodesList[j].ParentNode.RemoveChild(emphasisNodesList[j]);
                        }
                    }
                }
                descrip = "Blank Emphasises are removed";
            }
            if (descrip != "")
            {
                WriteInformation(descrip, Color.Chocolate);
            }
        }
        #endregion

        //Method***************InsertingBlankSpaceAtEndOfEmphasisInnerText********
        #region InsertingBlackSpaceInEmphasis(XmlNodeList paraList)
        public void InsertingBlankSpaceInEmphasis(XmlNodeList paraList)
        {
            string descrip = "";
            string strEmphasisInnerText = string.Empty;
            string strEmphasisOuterXml = string.Empty;
            //string strTemp = string.Empty;
            XmlNodeList emphasisNodesList;

            for (int i = 0; i < paraList.Count; i++)
            {
                emphasisNodesList = paraList[i].SelectNodes("./emphasis");

                for (int j = 0; j < emphasisNodesList.Count; j++)
                {
                    //strTemp = paraList[i].OuterXml;
                    strEmphasisOuterXml = emphasisNodesList[j].OuterXml;
                    strEmphasisInnerText = emphasisNodesList[j].InnerText;
                    if (strEmphasisInnerText[strEmphasisInnerText.Length - 1] != ' ')
                    {
                        strEmphasisInnerText += " ";
                        emphasisNodesList[j].InnerText = strEmphasisInnerText;
                        descrip = "Blank space in emphasis is inserted";
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            if (descrip != "")
            {
                WriteInformation(descrip, Color.Chocolate);
            }
        }
        #endregion

        //Method**************************checkFrontImageUrl**********************
        #region checkFrontImageUrl(XmlNode frontImageNode)
        private void checkFrontImageUrl(XmlNode frontImageNode)
        {
            if (frontImageNode != null)
            {
                string strFrontUrl = frontImageNode.Attributes["image-url"].Value.ToString();
                if (strFrontUrl == "Resources\\Front")
                {
                    divFinalizedResult.InnerHtml = divFinalizedResult.InnerHtml + "<span style=\" font-size:8; font-family:Tahoma; font-style:normal; color:" + Color.Green + "\">Front Image is OK.</span>";
                }
                else
                {
                    frontImageNode.Attributes["image-url"].Value = "Resources\\Front";
                    WriteInformation("Front image name changed from " + strFrontUrl + " to " + frontImageNode.Attributes["image-url"].Value.ToString() + " !!", Color.Chocolate);
                }
            }
            else
            {
                WriteInformation("No front image exists", Color.Chocolate);
            }
        }
        #endregion

        //Method*********************CheckingMultipleNumTagsInNpara***************
        #region CheckingMultipleNumTagsInNpara(XmlNodeList paraList)
        public void CheckingMultipleNumTagsInNpara(XmlNodeList paraList)
        {
            string descrip = "";
            ArrayList nparaErrorsList = new ArrayList();
            string strNparaId = string.Empty;
            bool bNparaExists = false;
            for (int i = 0; i < paraList.Count; i++)
            {
                if (paraList[i].Name == "npara")
                {
                    bNparaExists = true;
                    XmlNodeList nparaChildList = paraList[i].SelectNodes("./num");
                    foreach (XmlNode numNode in nparaChildList)
                    {
                        if (nparaChildList.Count > 1)
                        {
                            descrip = "Multiple num tags in npara with id " + paraList[i].Attributes["id"].Value.ToString();
                            nparaErrorsList.Add(descrip);
                            break;
                        }
                        else if (numNode.SelectNodes("./num").Count > 0)
                        {
                            descrip = "Nestled num tags in napara with id " + paraList[i].Attributes["id"].Value.ToString();
                            nparaErrorsList.Add(descrip);
                            break;
                        }
                        else
                        {
                            descrip = "No error found in nparas";
                        }
                    }
                }
                else
                {
                    continue;
                }
            }

            if (!bNparaExists)
            {
                descrip = "No npara exists in this book";
            }

            if (nparaErrorsList.Count > 0)
            {
                for (int i = 0; i < nparaErrorsList.Count; i++)
                {
                    WriteInformation(nparaErrorsList[i].ToString(), Color.Chocolate);
                }
            }
            else
            {
                WriteInformation(descrip, Color.Chocolate);
            }
        }
        #endregion

        //Method**************************checkForEmptyParas**********************
        #region checkEmptyParas(XmlNodeList parasList)
        private void checkEmptyParas(XmlNodeList parasList)
        {
            string descrip = string.Empty;
            ArrayList errorParaList = new ArrayList();
            for (int i = 0; i < parasList.Count; i++)
            {
                if (parasList[i].Name == "upara")
                {
                    if (parasList[i].ChildNodes.Count == 0)
                    {
                        descrip = parasList[i].Name + " with id = " + parasList[i].Attributes["id"].Value.ToString() + " is empty";
                        errorParaList.Add(descrip);
                    }
                }
                else if (parasList[i].ParentNode.Name == "spara")
                {
                    if (parasList[i].ParentNode.InnerText == string.Empty)
                    {
                        descrip = parasList[i].ParentNode.Name + " with id = " + parasList[i].ParentNode.Attributes["id"].Value.ToString() + " is empty";
                        errorParaList.Add(descrip);
                    }
                }
            }

            if (errorParaList.Count > 0)
            {
                for (int i = 0; i < errorParaList.Count; i++)
                {
                    WriteInformation(errorParaList[i].ToString(), Color.Red);
                }
            }
            else
            {
                descrip = "None of the sparas or uparas is empty";
                WriteInformation(descrip, Color.Chocolate);
            }
        }
        #endregion

        //Method***********RemovingBlankSpaceAtStartOfEmphasisInnerText***********
        #region RemovingBlackSpaceInEmphasis(XmlNodeList paraList)
        public void RemovingBlackSpaceInEmphasis(XmlNodeList paraList)
        {
            string descrip = "";
            string strEmphasisInnerText = string.Empty;
            XmlNodeList emphasisNodesList;
            for (int i = 0; i < paraList.Count; i++)
            {
                emphasisNodesList = paraList[i].SelectNodes("./emphasis");
                for (int j = 0; j < emphasisNodesList.Count; j++)
                {
                    if (emphasisNodesList[j].InnerText.StartsWith(" "))
                    {
                        emphasisNodesList[j].InnerText = emphasisNodesList[j].InnerText.TrimStart(' ');
                        descrip = "Spaces at start of emphasis tags removed";
                    }
                    else
                    {
                        continue;
                    }
                }
            }

            if (descrip != "")
            {
                WriteInformation(descrip, Color.Chocolate);
            }
        }
        #endregion

        //Method**************************checkBookNoticesImageMissing******************
        #region checkbookNoticesImageMissing(XmlNode bookNoticeImageNode)
        private void checkbookNoticesImageMissing(XmlNode bookNoticeImageNode)
        {
            if (bookNoticeImageNode != null)
            {
                string strBookNoticesUrl = bookNoticeImageNode.Attributes["image-url"].Value.ToString();

                string path = Path.GetDirectoryName(CommonClass.xmlFilePath);
                string bookNoticesImage = path + "\\" + strBookNoticesUrl;

                if (System.IO.File.Exists(bookNoticesImage))
                //if (System.IO.File.Exists(strBookNoticesUrl) || System.IO.File.Exists(strBookNoticesUrl))
                {
                    WriteInformation("BookNotices Image exists.", Color.Green);
                }
                else
                {
                    WriteInformation("BookNotices image not exists or spelling mistake in title image name!!", Color.Red);
                }
            }
            else
            {
                WriteInformation("BookNotices url not exists in xml!!", Color.Red);
            }
        }
        #endregion

        //Method***************RemovingBlankSpaceAfterEmphasisTag*****************
        #region RemovingBlankSpaceAfterEmphasis(XmlNodeList paraList)
        public void RemovingBlankSpaceAfterEmphasis(XmlNodeList paraList)
        {
            string descrip = "";
            string strParaInnerXml = string.Empty;

            for (int i = 0; i < paraList.Count; i++)
            {
                strParaInnerXml = paraList[i].InnerXml;
                paraList[i].InnerXml = Regex.Replace(paraList[i].InnerXml, "(</emphasis>)[ ]+", "$1");
                if (strParaInnerXml != paraList[i].InnerXml)
                {
                    descrip = "Blank spaces after emphasis tag removed";
                }
            }

            if (descrip != "")
            {
                WriteInformation(descrip, Color.Chocolate);
            }
        }

        #endregion
        /////////////////////////////////////////////

        #endregion

        #endregion

    }

}

                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                       
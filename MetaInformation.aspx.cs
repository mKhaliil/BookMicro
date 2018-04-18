using System;
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
using System.Diagnostics;
using System.IO;
using System.Data.OleDb;
using iTextSharp.text.pdf;
using iTextSharp.text;
using iTextSharp.text.pdf.parser;
using Outsourcing_System.UserControls;

namespace Outsourcing_System
{
    public partial class MetaInformation : System.Web.UI.Page
    {
        //======================BISAC
        string conString = "Provider=Microsoft.jet.oledb.4.0;Data Source=C:\\Data Conversion must include\\BISAC.mdb";
        OleDbConnection con;
        OleDbDataReader dr;
        OleDbCommand cmd;
        GlobalVar objGlobal = new GlobalVar();
        MyDBClass objMyDBClass = new MyDBClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            // this.Title = "Outsourcing System :: Audit Tool Specified Errors";            
            this.lblMessage.Text = "";
            if (!Page.IsPostBack)
            {               
                if (Session["rhywPath"] == null)
                {
                    if (Session["BID"] != null)
                    {
                        string mainBook = Session["MainBook"].ToString();
                        if (File.Exists((objMyDBClass.MainDirPhyPath + mainBook + "\\" + mainBook + "-Final.rhyw")))
                        {
                            Session["rhywPath"] = objMyDBClass.MainDirPhyPath + mainBook + "\\" + mainBook + "-Final.rhyw";
                        }
                        else
                        {
                            Session["rhywPath"] = objMyDBClass.MainDirPhyPath + mainBook + "\\" + mainBook + "-1\\TaggingUntagged\\" + mainBook + "-1.rhyw";
                        }
                        if (File.Exists((objMyDBClass.MainDirPhyPath + mainBook + "\\" + mainBook + ".pdf")))
                        {
                            Session["pdfPath"] = objMyDBClass.MainDirPhyPath + mainBook + "\\" + mainBook + ".pdf";
                        }
                    }
                }

                objGlobal.XMLPath = Session["rhywPath"].ToString();
                Session["XMLPath"] = objGlobal.XMLPath;
                //GlobalVar.XMLPath = Server.MapPath("Files/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID + "/TaggingUntagged/" + bookID + ".rhyw");
                objGlobal.PBPDocument = new XmlDocument();
                Session["PBPDocument"] = objGlobal.PBPDocument;
                objGlobal.LoadXml();
                HandleMetaInformation();
            }
        }


        protected void ddInformation_SelectedIndexChanged(object sender, EventArgs e)
        {
            pnlMeta.Visible = false;
            pnlTitle.Visible = false;
            pnlBookrepInfo.Visible = false;
            pnlBisac.Visible = false;

            this.txtMetaSName.Text = "";
            this.txtMetaSRev.Text = "";
            this.txtMetaFName.Text = "";
            this.txtMetaTDate.Text = "";
            this.cboMetaTOperator.SelectedIndex = 0;
            this.txtMetaBTitle.Text = "";
            this.cboMetaBType.SelectedIndex = 0;
            this.cboMetaPStatus.SelectedIndex = 0;
            this.cboMetaCStatus.SelectedIndex = 0;

            this.txtBMTitle.Text = "";
            this.txtBSTitle.Text = "";
            this.txtBRHeader.Text = "";

            //this.author1.firstName = "";
            //this.txtAFName.Text = "";
           // this.author1.fullName = "";
            //this.txtAFulName.Text = "";
            //this.author1.prenominal = "";
            //this.txtAPrenominal.Text = "";
           // this.author1.lastName = "";
            //this.txtALName.Text = "";
            //this.txtAFulName.Focus();

            this.txtTFName.Text = "";
            this.txtTFulName.Text = "";
            this.txtTLName.Text = "";
            this.txtTPrenominal.Text = "";
            this.txtTFulName.Focus();

            this.txtInfoAID.Text = "";
            this.txtBookSummary.Text = "";
            this.txtAuthorInfo.Text = "";

            string selValue = this.ddInformation.SelectedValue;
            switch (selValue)
            {
                case "meta":
                    {
                        pnlMeta.Visible = true;
                        HandleMetaInformation();
                        break;
                    }
                case "title":
                    {
                        pnlTitle.Visible = true;
                        HandleTitleInformation();
                        break;
                    }
                case "bookrepinfo":
                    {
                        pnlBookrepInfo.Visible = true;
                        HandleBookrepInfo();
                        break;
                    }

                case "bisac":
                    {
                        pnlBisac.Visible = true;
                        HandleBisac();
                        break;
                    }
            }
            // this.UpdatePanel1.Update();
        }
        //+++++++++++++++++++++++
        // Meta Information
        //+++++++++++++++++++++++

        #region HandleMetaInformation() and btnSaveMeta_Click(object sender, EventArgs e)
        private void HandleMetaInformation()
        {
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            XmlNode info = objGlobal.PBPDocument.SelectSingleNode("//pbp-info");
            this.txtMetaSName.Text = info.Attributes["schema-name"].Value;
            this.txtMetaSRev.Text = info.Attributes["schema-rev"].Value;
            this.txtMetaFName.Text = info.Attributes["file-name"].Value;
            this.txtMetaTDate.Text = info.Attributes["tag-date"].Value;
            this.cboMetaTOperator.SelectedIndex = this.cboMetaTOperator.Items.IndexOf(this.cboMetaTOperator.Items.FindByValue(info.Attributes["tag-operator"].Value));
            this.txtMetaBTitle.Text = info.Attributes["book-title"].Value;
            this.cboMetaBType.SelectedIndex = this.cboMetaTOperator.Items.IndexOf(this.cboMetaTOperator.Items.FindByValue(info.Attributes["book-type"].Value));
            this.cboMetaPStatus.SelectedIndex = this.cboMetaTOperator.Items.IndexOf(this.cboMetaTOperator.Items.FindByValue(info.Attributes["publication-status"].Value));
            this.cboMetaCStatus.SelectedIndex = this.cboMetaTOperator.Items.IndexOf(this.cboMetaTOperator.Items.FindByValue(info.Attributes["copyright-status"].Value));

        }
        protected void btnSubmitMeta_Click(object sender, EventArgs e)
        {
            string BID = Session["BID"].ToString();
            string queryUpdate = "Update ACTIVITY Set Status='Approved',CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "' Where Task='Meta' AND BID=" + BID;
            string uid = Convert.ToString(Session["LoginId"]);
            CreateErroDetectionTask(BID, uid);
            objMyDBClass.ExecuteCommand(queryUpdate);
            objMyDBClass.LogEntry(BID, "Meta", "Meta Information is added successfully", "Completed", "update");

            Response.Redirect(string.Format("OnlineTestUser.aspx?UserId={0}", Convert.ToString(Session["LoginId"])), true);
        }

        protected void btnSaveMeta_Click(object sender, EventArgs e)
        {
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            XmlNode info = objGlobal.PBPDocument.SelectSingleNode("//pbp-info");
            info.Attributes["schema-name"].Value = this.txtMetaSName.Text;
            info.Attributes["schema-rev"].Value = this.txtMetaSRev.Text;
            info.Attributes["file-name"].Value = this.txtMetaFName.Text;
            info.Attributes["tag-date"].Value = this.txtMetaTDate.Text;
            info.Attributes["tag-operator"].Value = this.cboMetaTOperator.SelectedValue.ToString();
            info.Attributes["book-title"].Value = this.txtMetaBTitle.Text;
            info.Attributes["book-type"].Value = this.cboMetaBType.SelectedValue.ToString();
            info.Attributes["publication-status"].Value = this.cboMetaPStatus.SelectedValue.ToString();
            info.Attributes["copyright-status"].Value = this.cboMetaCStatus.SelectedValue.ToString();
            objGlobal.SaveXml();
            objMyDBClass.LogEntry(System.IO.Path.GetFileNameWithoutExtension(Session["rhywPath"].ToString()), "Meta Information", "Meta basic attributes are saved", "In Progress", "update");
            this.lblMessage.Text = "Meta Information Saved Successfully";
        }
        #endregion

        //+++++++++++++++++++++++
        // Title Information
        //+++++++++++++++++++++++

        #region HandleTitleInformation() and btnSaveTitle_Click(object sender, EventArgs e)
        private void HandleTitleInformation()
        {
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            XmlNode author = objGlobal.PBPDocument.SelectSingleNode("//author");
            XmlNode fullname = author.SelectSingleNode(".//full-name");
            // this.txtAFulName.Text = fullname.InnerXml;
            XmlNode prenominal = author.SelectSingleNode(".//prenominal");
            // this.txtAPrenominal.Text = prenominal.InnerXml;
            XmlNode firstname = author.SelectSingleNode(".//first-name");
            // this.txtAFName.Text = firstname.InnerXml;
            XmlNode lastname = author.SelectSingleNode(".//last-name");
            //   this.txtALName.Text = lastname.InnerXml;

            XmlNode translator = objGlobal.PBPDocument.SelectSingleNode("//translator");
            if (translator != null)
            {
                fullname = translator.SelectSingleNode(".//full-name");
                this.txtTFulName.Text = fullname.InnerXml;
                prenominal = translator.SelectSingleNode(".//prenominal");
                this.txtTPrenominal.Text = prenominal.InnerXml;
                firstname = translator.SelectSingleNode(".//first-name");
                this.txtTFName.Text = firstname.InnerXml;
                lastname = translator.SelectSingleNode(".//last-name");
                this.txtTLName.Text = lastname.InnerXml;
            }

            XmlNode bookTitle = objGlobal.PBPDocument.SelectSingleNode("//book-title");
            this.txtBMTitle.Text = bookTitle.SelectSingleNode("main-title").InnerXml;
            this.txtBSTitle.Text = bookTitle.SelectSingleNode("sub-title").InnerXml;
            this.txtBRHeader.Text = bookTitle.SelectSingleNode("running-header").InnerXml;
        }

        protected void btnAuthor_Click(object sender, EventArgs e)
        {
            if(ViewState["Authors"]!=null)
            {
                ViewState["Authors"]=Convert.ToInt32(ViewState["Authors"].ToString()) + 1;
            }
            else
            {
            ViewState["Authors"] = 1;
            }
            ucAuthor uc = (ucAuthor)Page.LoadControl("~/UserControls/ucAuthor.ascx");
            uc.ID = ViewState["Authors"].ToString();
            authorsPlace.Controls.Add(uc);
            this.UpdatePanel1.Update();
        }
        protected void btnSaveTitle_Click(object sender, EventArgs e)
        {
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            XmlNode author = objGlobal.PBPDocument.SelectSingleNode("//title-block//author");
            XmlNode afullname = author.SelectSingleNode(".//full-name");
            //  afullname.InnerXml = this.txtAFulName.Text;
            XmlNode aprenominal = author.SelectSingleNode(".//prenominal");
            //  aprenominal.InnerXml = this.txtAPrenominal.Text;
            XmlNode afirstname = author.SelectSingleNode(".//first-name");
            //  afirstname.InnerXml = this.txtAFName.Text;
            XmlNode alastname = author.SelectSingleNode(".//last-name");
            //  alastname.InnerXml = this.txtALName.Text;

            XmlNode translator = objGlobal.PBPDocument.SelectSingleNode("//title-block//translator");
            if (translator != null)
            {
                XmlNode tfullname = translator.SelectSingleNode(".//full-name");
                this.txtTFulName.Text = tfullname.InnerXml;
                XmlNode tprenominal = translator.SelectSingleNode(".//prenominal");
                this.txtTPrenominal.Text = tprenominal.InnerXml;
                XmlNode tfirstname = translator.SelectSingleNode(".//first-name");
                this.txtTFName.Text = tfirstname.InnerXml;
                XmlNode tlastname = translator.SelectSingleNode(".//last-name");
                this.txtTLName.Text = tlastname.InnerXml;
            }
            else if (this.txtTFulName.Text != "" && this.txtTFName.Text != "")
            {
                XmlElement newTranslator = objGlobal.PBPDocument.CreateElement("translator");

                XmlElement fullname = objGlobal.PBPDocument.CreateElement("full-name");
                fullname.InnerXml = this.txtTFulName.Text;
                XmlElement prenominal = objGlobal.PBPDocument.CreateElement("prenominal");
                prenominal.InnerXml = this.txtTPrenominal.Text;
                XmlElement firstname = objGlobal.PBPDocument.CreateElement("first-name");
                firstname.InnerXml = this.txtTFName.Text;
                XmlElement lastname = objGlobal.PBPDocument.CreateElement("last-name");
                lastname.InnerXml = this.txtTLName.Text;
                newTranslator.AppendChild(fullname);
                newTranslator.AppendChild(prenominal);
                newTranslator.AppendChild(firstname);
                newTranslator.AppendChild(lastname);

                author = objGlobal.PBPDocument.SelectSingleNode("//title-block//author");
                author.ParentNode.InsertAfter(newTranslator, author);
            }

            XmlNode bookTitle = objGlobal.PBPDocument.SelectSingleNode("//title-block//book-title");
            bookTitle.SelectSingleNode("main-title").InnerXml = this.txtBMTitle.Text;
            bookTitle.SelectSingleNode("sub-title").InnerXml = this.txtBSTitle.Text;
            bookTitle.SelectSingleNode("running-header").InnerXml = this.txtBRHeader.Text;
            objGlobal.SaveXml();
            objMyDBClass.LogEntry(System.IO.Path.GetFileNameWithoutExtension(Session["rhywPath"].ToString()), "Meta Information", "Meta title information are saved", "In Progress", "update");
            this.lblMessage.Text = "Title Information Saved Successfully";
        }
        #endregion

        //+++++++++++++++++++++++
        // Book Rep Information
        //+++++++++++++++++++++++

        #region HandleBookrepInfo() and  btnBookrepInfoSave_Click(object sender, EventArgs e)
        private void HandleBookrepInfo()
        {
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            this.txtInfoAID.Text = objGlobal.PBPDocument.SelectSingleNode("//bookrep-info/author-id").InnerXml.ToString();
            this.txtBookSummary.Text = objGlobal.PBPDocument.SelectSingleNode("//bookrep-info/book-summary").InnerXml;
            this.txtAuthorInfo.Text = objGlobal.PBPDocument.SelectSingleNode("//bookrep-info/author-info").InnerXml;
        }

        protected void btnBookrepInfoSave_Click(object sender, EventArgs e)
        {
            XmlCDataSection bookSummary = objGlobal.PBPDocument.CreateCDataSection("<html><head></head><body>" + txtAuthorInfo.Text + "</body></html>");
            XmlCDataSection authorInfo = objGlobal.PBPDocument.CreateCDataSection("<html><head></head><body>" + txtAuthorInfo.Text + "</body></html>");

            objGlobal.PBPDocument.SelectSingleNode("//bookrep-info/author-id").InnerXml = this.txtInfoAID.Text;
            XmlNode binfo = objGlobal.PBPDocument.SelectSingleNode("//bookrep-info/author-info");
            binfo.InnerXml = "";
            binfo.AppendChild(authorInfo);

            XmlNode bsummary = objGlobal.PBPDocument.SelectSingleNode("//bookrep-info/book-summary");
            bsummary.InnerXml = "";
            bsummary.AppendChild(bookSummary);
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            objGlobal.SaveXml();
            objMyDBClass.LogEntry(System.IO.Path.GetFileNameWithoutExtension(Session["rhywPath"].ToString()), "Meta Information", "Meta bookrep information are saved", "In Progress", "update");
            this.lblMessage.Text = "Bookrep Information Saved Successfully";
        }
        #endregion

        //+++++++++++++++++++++++
        // Book Rep Information
        //+++++++++++++++++++++++

        private void HandleBisac()
        {
            this.cboBisacMainCat.Items.Clear();
            this.cboBisacSubCat.Items.Clear();
            this.txtBisacSubCode.Text = "";
            this.lstBISAC.Items.Clear();
            con = new OleDbConnection(conString);
            con.Open();
            cmd = new OleDbCommand("SELECT * FROM tblBISACSubjectHeadings", con);
            dr = cmd.ExecuteReader();
            while (dr.Read())
            {
                this.cboBisacMainCat.Items.Add(dr.GetString(2));
            }
            this.cboBisacMainCat.SelectedIndex = 0;
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            XmlNodeList list = objGlobal.PBPDocument.SelectNodes("//BISAC-item");
            for (int i = 0; i < list.Count; i++)
            {
                string text1 = list[i].SelectSingleNode("BISAC-text").InnerXml;
                string text2 = list[i].SelectSingleNode("BISAC-code").InnerXml;
                if (text1 != "" && text2 != "")
                {
                    this.lstBISAC.Items.Add(text1 + "-" + text2);
                }
            }
            con.Close();
        }

        #region cboBisacMainCat_SelectedIndexChanged(object sender, EventArgs e)
        protected void cboBisacMainCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            con = new OleDbConnection(conString);
            con.Open();
            int heading_id = cboBisacMainCat.SelectedIndex + 1;
            cmd = new OleDbCommand("SELECT * FROM tblBISACSubject where BISACSubjectHeadings_ID = " + heading_id, con);
            OleDbDataReader dr = cmd.ExecuteReader();
            cboBisacSubCat.Items.Clear();
            while (dr.Read())
            {
                this.cboBisacSubCat.Items.Add(dr.GetString(2));
            }
            this.cboBisacSubCat.SelectedIndex = 0;
            con.Close();
            //  this.UpdatePanel1.Update();
        }
        #endregion

        #region cboBisacSubCat_SelectedIndexChanged(object sender, EventArgs e)
        protected void cboBisacSubCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.txtBisacSubCode.Text = getSubjectCode();
            // this.UpdatePanel1.Update();
        }
        #endregion

        #region getSubjectCode()
        protected string getSubjectCode()
        {
            string strBISAC_code = "";
            try
            {
                string headingIndex = "" + (this.cboBisacMainCat.SelectedIndex + 1);
                string subHeadingIndex = "" + (this.cboBisacSubCat.SelectedIndex + 1);

                if (this.cboBisacSubCat.SelectedItem.ToString().Trim().Equals("") || this.cboBisacSubCat.SelectedItem.ToString().Trim().Equals(""))
                {
                    //this.btnBISACAdd.Enabled = false;
                }
                else
                {
                    //this.btnBISACAdd.Enabled = true;
                    con = new OleDbConnection(conString);
                    con.Open();

                    cmd = new OleDbCommand("SELECT * FROM tblBISACSubject where BISACSubjectHeadings_ID = " + headingIndex, con);
                    dr = cmd.ExecuteReader();
                    int count = 1;
                    while (dr.Read())
                    {
                        if (count.ToString().Equals(subHeadingIndex))
                        {
                            strBISAC_code = dr.GetString(1);
                        }
                        count++;
                    }
                    con.Close();
                }
            }
            catch (OleDbException exp)
            {
                this.lblMessage.Text = exp.Message;
            }
            return strBISAC_code;
        }
        #endregion

        protected void btnBISACAdd_Click(object sender, EventArgs e)
        {
            string toAdd = "";
            if (this.cboBisacSubCat.SelectedIndex != -1)
            {
                toAdd = this.cboBisacMainCat.SelectedItem.ToString() + "/" + this.cboBisacSubCat.SelectedItem.ToString() + "-" + this.txtBisacSubCode.Text;
                this.lstBISAC.Items.Add(toAdd);
            }
            // this.UpdatePanel1.Update();
        }

        protected void btnBISACRemove_Click(object sender, EventArgs e)
        {
            this.lstBISAC.Items.Remove(this.lstBISAC.SelectedItem);
            //  this.UpdatePanel1.Update();
        }
        protected void btnSaveBISAC_Click(object sender, EventArgs e)
        {
            if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
            {
                objGlobal.XMLPath = Session["XMLPath"].ToString();
            }
            if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
            {
                objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
            }
            if (this.lstBISAC.Items.Count <= 0)
            {
                this.lblMessage.Text = "At least one item should be in list";
            }
            else
            {
                string depCat = "";
                string strBISAC_text = "";
                string strBISAC_code = "";
                XmlNode BISAC_item = null;

                XmlNode mainBISAC = objGlobal.PBPDocument.SelectSingleNode("//BISAC");
                mainBISAC.RemoveAll();

                for (int listIndex = 0; listIndex < this.lstBISAC.Items.Count; listIndex++)
                {
                    depCat = this.lstBISAC.Items[listIndex].ToString();
                    strBISAC_text = depCat.Substring(0, depCat.IndexOf("-"));
                    strBISAC_code = depCat.Substring(depCat.IndexOf("-") + 1, depCat.Length - depCat.IndexOf("-") - 1);
                    strBISAC_text = strBISAC_text.Trim();
                    strBISAC_code = strBISAC_code.Trim();

                    BISAC_item = objGlobal.PBPDocument.CreateElement("BISAC-item");
                    XmlNode BISAC_text = objGlobal.PBPDocument.CreateElement("BISAC-text");
                    XmlNode BISAC_code = objGlobal.PBPDocument.CreateElement("BISAC-code"); ;
                    BISAC_text.InnerText = strBISAC_text;
                    BISAC_code.InnerText = strBISAC_code;
                    BISAC_item.AppendChild(BISAC_text);
                    BISAC_item.AppendChild(BISAC_code);
                    mainBISAC.AppendChild(BISAC_item);
                }
                objMyDBClass.LogEntry(System.IO.Path.GetFileNameWithoutExtension(Session["rhywPath"].ToString()), "Meta Information", "Meta BISAC is added. Book is Completed. Allah Hafiz", "Completed", "update");
                objGlobal.SaveXml();
                insertVolumeBreakInfo();
                Response.Redirect("AdminPanel.aspx", false);
            }
        }
        private void insertVolumeBreakInfo()
        {
            string pdfPath = Session["pdfPath"].ToString();
            PdfReader inputPdf = new PdfReader(pdfPath);
            int Pages = inputPdf.NumberOfPages;
            int bID = Convert.ToInt32(Session["BID"].ToString());
            string insertRecQuery = "Insert Into VOLUME_BREAK_INFO Values('" + bID + "','" + Pages + "')";
            int insRes = objMyDBClass.ExecuteCommand(insertRecQuery);
        }
        public void CreateErroDetectionTask(string bookID, string userId)
        {
            objMyDBClass.CreateTask(bookID, "Unassigned", "ErrorDetection", userId);
        }
    }
}                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                            
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outsourcing_System
{
    public partial class SpellChecker : System.Web.UI.Page
    {
        GlobalVar objGlobal = new GlobalVar();
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["Finalized"] != null)
            {

            }
            else
            {
                loadLatestXml("");
                SpellTextBox1.Text = objGlobal.PBPDocument.OuterXml;
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

        protected void SpellButton1_SpellCheckCompleteEvent(object sender, EventArgs e)
        {

        }
    }
}
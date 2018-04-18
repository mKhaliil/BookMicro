using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outsourcing_System.MasterPages;

namespace Outsourcing_System
{
    public partial class IndexingTutorials : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["LoginId"] == null)
                {
                    Response.Redirect("Bookmicro.aspx", true);
                }

               //((OnlineTestRegisterdMaster)this.Page.Master).SetLogOut = true;
                //((UserMaster)this.Page.Master).ShowLogOutButton();
                //((UserMaster)this.Page.Master).SetMenuLocation = "-120px";
            }
        }
        protected void lbtnTestTraining_Click(object sender, EventArgs e)
        {
            Response.Redirect("Training.aspx", false);
        }
        protected void lnlIndexTool_Click(object sender, EventArgs e)
        {
            string filePath = @"C:\E-Index_BookMicro.zip";
            Context.Response.Clear();
            Context.Response.ContentType = "application/x-zip-compressed";
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=IndexTool");
            Context.Response.WriteFile(filePath);
            //Response.Redirect("ImageTest.aspx");
            Context.Response.End();
        }
    }
}
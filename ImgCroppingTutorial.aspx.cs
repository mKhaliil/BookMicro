using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outsourcing_System.MasterPages;

namespace Outsourcing_System
{
    public partial class ImgTutorialDetaisl : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               //((OnlineTestRegisterdMaster)this.Page.Master).SetLogOut = true;
                //((UserMaster)this.Page.Master).ShowLogOutButton();
                //((UserMaster)this.Page.Master).SetMenuLocation = "-120px";
                mvTrainingVideos.ActiveViewIndex = 0;
                lbtnUrdu.Attributes.Add("Style", "color:#0099ff");
                lbtnEnglish.Attributes.Add("Style", "color:#CCCCCC");
                lbtnUrdu.Enabled = true;
                lbtnEnglish.Enabled = false;
            }
        }

        protected void lbtnTestTraining_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImageTutorials.aspx", false);
        }
        protected void lbtnUrdu_Click(object sender, EventArgs e)
        {
            mvTrainingVideos.ActiveViewIndex = 1;
            lbtnUrdu.Attributes.Add("Style", "color:#CCCCCC");
            lbtnEnglish.Attributes.Add("Style", "color:#0099ff");
            lbtnEnglish.Enabled = true;
            lbtnUrdu.Enabled = false;
        }
        protected void lblEnglish_Click(object sender, EventArgs e)
        {
            mvTrainingVideos.ActiveViewIndex = 0;
            lbtnUrdu.Attributes.Add("Style", "color:#0099ff");
            lbtnEnglish.Attributes.Add("Style", "color:#CCCCCC");
            lbtnUrdu.Enabled = true;
            lbtnEnglish.Enabled = false;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outsourcing_System.MasterPages;

namespace Outsourcing_System
{
    public partial class ImageTutorials : System.Web.UI.Page
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

        protected void ibtnImgCropping_Text_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImgCroppingTutorial.aspx#tutorials", false);
        }

        protected void ibtnImgCropping_Video_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImgCroppingTutorial.aspx#video", false);
        }

        protected void ibtnBookNotices_Text_Click(object sender, EventArgs e)
        {
            Response.Redirect("Booknotices.aspx#tutorials", false);
        }

        protected void ibtnBookNotices_Video_Click(object sender, EventArgs e)
        {
            Response.Redirect("Booknotices.aspx#video", false);
        }

        protected void ibtnTitleCropping_Text_Click(object sender, EventArgs e)
        {
            Response.Redirect("TitlePageCroppingTutorial.aspx#tutorials", false);
        }

        protected void ibtnTitleCropping_Video_Click(object sender, EventArgs e)
        {
            Response.Redirect("TitlePageCroppingTutorial.aspx#video", false);
        }
    }
}
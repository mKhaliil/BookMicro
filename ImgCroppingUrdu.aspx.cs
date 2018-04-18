using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outsourcing_System.MasterPages;

namespace Outsourcing_System
{
    public partial class ImgCroppingUrdu : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //((UserMaster)this.Page.Master).SetLogOut = true;
            }
        }

        protected void lbtnTestTraining_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImageTutorials.aspx", false);
        }
    }
}
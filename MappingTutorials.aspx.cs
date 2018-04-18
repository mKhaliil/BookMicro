using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outsourcing_System
{
    public partial class MappingTutorials : System.Web.UI.Page
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
                ((OnlineTestMasterPage)this.Page.Master).SetLogOut = true;
                ((OnlineTestMasterPage)this.Page.Master).SetMenuLocation = "-120px";
            }
        }

        protected void lbtnTestTraining_Click(object sender, EventArgs e)
        {
            Response.Redirect("Training.aspx", false);
        }

        protected void ibtnComplexBits_Text_Click(object sender, EventArgs e)
        {
            Response.Redirect("ComplexBits.aspx#tutorials", false);
        }

        protected void ibtnComplexBits_Video_Click(object sender, EventArgs e)
        {
            Response.Redirect("ComplexBits.aspx#video", false);
        }

        protected void ibtnBookHierarchy_Text_Click(object sender, EventArgs e)
        {
            Response.Redirect("BookHierarchy.aspx#tutorials", false);
        }

        protected void ibtnBookHierarchy_Video_Click(object sender, EventArgs e)
        {
            Response.Redirect("BookHierarchy.aspx#video", false);
        }

        protected void ibtnBookComparison1_Text_Click(object sender, EventArgs e)
        {
            Response.Redirect("BookComparison1.aspx#tutorials", false);
        }

        protected void ibtnBookComparison1_Video_Click(object sender, EventArgs e)
        {
            Response.Redirect("BookComparison1.aspx#video", false);
        }
        protected void ibtnBookComparison2_Text_Click(object sender, EventArgs e)
        {
            Response.Redirect("BookComparison2.aspx#tutorials", false);
        }

        protected void ibtnBookComparison2_Video_Click(object sender, EventArgs e)
        {
            Response.Redirect("BookComparison2.aspx#video", false);
        }
    }
}
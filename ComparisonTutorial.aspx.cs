using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outsourcing_System
{
    public partial class ComparisonTutorial : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Convert.ToString(Session["LoginId"]) != "")
                {
                    string countryName = Convert.ToString(Session["CountryName"]);

                    if (countryName != "")
                    {
                        if (countryName.Equals("pakistan"))
                        {
                            mvTrainingVideos.ActiveViewIndex = 1;
                            lbtnUrdu.Attributes.Add("Style", "color:#CCCCCC");
                            lbtnEnglish.Attributes.Add("Style", "color:#0099ff");
                            lbtnUrdu.Enabled = false;
                            lbtnEnglish.Enabled = true;
                        }
                        else if (countryName.Equals("other"))
                        {
                            mvTrainingVideos.ActiveViewIndex = 0;
                            lbtnUrdu.Attributes.Add("Style", "color:#0099ff");
                            lbtnEnglish.Attributes.Add("Style", "color:#CCCCCC");
                            lbtnUrdu.Enabled = true;
                            lbtnEnglish.Enabled = false;
                        }
                    }
                }
                else
                {
                    Response.Redirect("Bookmicro.aspx", true);
                }
            }
        }

        protected void lbtnTestTraining_Click(object sender, EventArgs e)
        {
            Response.Redirect("Training.aspx", false);
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
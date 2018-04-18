using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outsourcing_System
{
    public partial class QuizType : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            mvQuiz.ActiveViewIndex = 0;
        }

        protected void btnSplitting_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ComparisonPreProcess.aspx?uid={0}&bid={1}&type={2}&email={3}&quiztype={4}", Convert.ToString(Session["LoginId"]), "", "onepagetest", Convert.ToString(Session["email"]), "Splitting"), true);
        }

        protected void btnMerging_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ComparisonPreProcess.aspx?uid={0}&bid={1}&type={2}&email={3}&quiztype={4}", Convert.ToString(Session["LoginId"]), "", "onepagetest", Convert.ToString(Session["email"]), "Merging"), true);
        }

        protected void btnSpace_Click(object sender, EventArgs e)
        {
            Response.Redirect(string.Format("ComparisonPreProcess.aspx?uid={0}&bid={1}&type={2}&email={3}&quiztype={4}", Convert.ToString(Session["LoginId"]), "", "onepagetest", Convert.ToString(Session["email"]), "Space"), true);
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            mvQuiz.ActiveViewIndex = 0;
        }

        protected void btnGeneralQuiz_Click(object sender, EventArgs e)
        {
            mvQuiz.ActiveViewIndex = 0;
        }

        protected void btnBeginner_Click(object sender, EventArgs e)
        {
            mvQuiz.ActiveViewIndex = 1;
        }

        protected void btnIntermediate_Click(object sender, EventArgs e)
        {
            mvQuiz.ActiveViewIndex = 1;
        }

        protected void btnExpert_Click(object sender, EventArgs e)
        {
            mvQuiz.ActiveViewIndex = 1;
        }
    }
}
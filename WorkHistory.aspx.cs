using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outsourcing_System
{
    public partial class WorkHistory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack) BindArchiveTask();
        }

        private void BindArchiveTask()
        {
            MyDBClass objMyDBClass = new MyDBClass();
            List<OnlineTest> list_Completedtasks = objMyDBClass.GetCompletedTasks_ByUser(Convert.ToString(Session["LoginId"]));
            int count = 0;

            if ((list_Completedtasks != null) && (list_Completedtasks.Count > 0))
            {
                if (list_Completedtasks.Count > 0)
                {
                    if (list_Completedtasks.Count > 20)
                    {
                        list_Completedtasks.RemoveRange(20, list_Completedtasks.Count - 20);
                    }

                    gvArchiveTasks.DataSource = list_Completedtasks;
                    gvArchiveTasks.DataBind();
                }
            }
        }

        protected void gvArchiveTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblMark = (Label)e.Row.FindControl("lblTask");
                Label FeedBackDate = (Label)e.Row.FindControl("lblFeedBackDate");
                Label lblComments = (Label)e.Row.FindControl("lblComments");

                string BookId = Convert.ToString(gvArchiveTasks.DataKeys[e.Row.RowIndex].Values[0]);
                string TestType = Convert.ToString(gvArchiveTasks.DataKeys[e.Row.RowIndex].Values[1]);
                string TimelyDelivery = Convert.ToString(gvArchiveTasks.DataKeys[e.Row.RowIndex].Values[2]);
                string Quality = Convert.ToString(gvArchiveTasks.DataKeys[e.Row.RowIndex].Values[3]);
                string Responsiveness = Convert.ToString(gvArchiveTasks.DataKeys[e.Row.RowIndex].Values[4]);
                //string Responsiveness = Convert.ToString(gvCompletedTasks.DataKeys[e.Row.RowIndex].Values[4]);

                lblMark.Text = Convert.ToString(":: ") + BookId + " - " + TestType;

                if (!string.IsNullOrEmpty(FeedBackDate.Text)) FeedBackDate.Text = Convert.ToDateTime(FeedBackDate.Text).ToString("Y");

                //if (!string.IsNullOrEmpty(lblComments.Text)) lblComments.Text = lblComments.Text;
                //FeedBackDate.Text = FeedBackDate.Text;

                //e.Row.ForeColor = System.Drawing.Color.Gray;   
            }
        }
    }
}
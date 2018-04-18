using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Outsourcing_System
{
    public partial class ManageTasks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindTaskPriorities();
            }
        }

        private void BindTaskPriorities()
        {
            MyDBClass objMyDBClass = new MyDBClass();
            List<OnlineTest> list_tasks = objMyDBClass.GetTaskPriorities(Convert.ToString(Session["LoginId"]));
            gvTaskPriorities.DataSource = list_tasks;
            gvTaskPriorities.DataBind();
        }

        protected void gvTaskPriorities_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            GridView grid = sender as GridView;
            GridViewRow row = grid.Rows[Convert.ToInt32(e.CommandArgument)];
            Label lblPriority = (Label)row.FindControl("lblPriority");
            DropDownList ddlPriority = (DropDownList)row.FindControl("ddlPriority");

            string bid = Convert.ToString(gvTaskPriorities.DataKeys[Convert.ToInt32(e.CommandArgument)].Values[0]);

            if (e.CommandName.Equals("editBtnClick"))
            {
                lblPriority.Visible = false;
                ddlPriority.Visible = true;
            }

            if (e.CommandName.Equals("saveBtnClick"))
            {
                lblPriority.Visible = true;
                ddlPriority.Visible = false;

                int index = ddlPriority.SelectedIndex;
                SavePriority(index, bid);
                BindTaskPriorities();
            }
        }

        protected void gvTaskPriorities_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");
                Label lblPriority = (Label)e.Row.FindControl("lblPriority");
               
                lblSrNo.Text = Convert.ToString(e.Row.RowIndex + 1) + ".";

                if (lblPriority.Text.Trim().Equals("0"))
                {
                    lblPriority.Text = "Normal";
                }

                else if (lblPriority.Text.Trim().Equals("1"))
                {
                    lblPriority.Text = "High";
                }

                else if (lblPriority.Text.Trim().Equals("2"))
                {
                    lblPriority.Text = "Urgent";
                }
            }
        }

        public void SavePriority(int index, string bid)
        {
            MyDBClass objMyDBClass = new MyDBClass();
            string queryInsert = "Update ACTIVITY Set Priority=" + index + "where bid =" + bid;
            int inResult = objMyDBClass.ExecuteCommand(queryInsert);
        }
    }
}
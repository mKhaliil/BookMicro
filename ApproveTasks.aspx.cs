using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outsourcing_System.PdfCompare_Classes;

namespace Outsourcing_System
{
    public partial class ApproveTasks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if (!string.IsNullOrEmpty(Convert.ToString(Session["LoginId"]))) BindApprovedTasks();
                BindApprovedTasks();
                BindInProgressTasks();
            }
        }

        private void BindApprovedTasks()
        {
            MyDBClass objMyDBClass = new MyDBClass();
            //List<OnlineTest> approvedTasksList = objMyDBClass.GetApprovedTasks(Convert.ToString(Session["LoginId"]));
            List<OnlineTest> approvedTasksList = objMyDBClass.GetTasksByStatus("ErrorDetection", "Approved");

            gvApprovedTask.DataSource = approvedTasksList;
            gvApprovedTask.DataBind();
        }

        private void BindInProgressTasks()
        {
            MyDBClass objMyDBClass = new MyDBClass();
            //List<OnlineTest> approvedTasksList = objMyDBClass.GetApprovedTasks(Convert.ToString(Session["LoginId"]));
            List<OnlineTest> approvedTasksList = objMyDBClass.GetTasksByStatus("ErrorDetection", "Working");

            gvInProgressTasks.DataSource = approvedTasksList;
            gvInProgressTasks.DataBind();
        }

        protected void gvWithdrawAmountReq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");

                lblSrNo.Text = Convert.ToString(e.Row.RowIndex + 1) + ".";

                //string completionDate = Convert.ToString(gvApprovedTask.DataKeys[e.Row.RowIndex].Values[0]);
                //string startDate = Convert.ToString(gvApprovedTask.DataKeys[e.Row.RowIndex].Values[1]);

                //if (!string.IsNullOrEmpty(completionDate))
                //    lblCompletionDate.Text = Convert.ToDateTime(completionDate).ToString("dd-MM-yyyy");

                //if (!string.IsNullOrEmpty(startDate))
                //    lblStartDate.Text = Convert.ToDateTime(startDate).ToString("dd-MM-yyyy");
            }
        }

        protected void gvApprovedTask_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");
                Label lblCompletionDate = (Label)e.Row.FindControl("lblCompletionDate");
                Label lblStartDate = (Label)e.Row.FindControl("lblStartDate");
                Label lblPriority = (Label)e.Row.FindControl("lblPriority");

                lblSrNo.Text = Convert.ToString(e.Row.RowIndex + 1) + ".";

                string completionDate = Convert.ToString(gvApprovedTask.DataKeys[e.Row.RowIndex].Values[0]);
                string startDate = Convert.ToString(gvApprovedTask.DataKeys[e.Row.RowIndex].Values[1]);
                string priority = Convert.ToString(gvApprovedTask.DataKeys[e.Row.RowIndex].Values[3]);

                if (!string.IsNullOrEmpty(completionDate))
                    lblCompletionDate.Text = Convert.ToDateTime(completionDate).ToString("dd-MM-yyyy");

                if (!string.IsNullOrEmpty(startDate))
                    lblStartDate.Text = Convert.ToDateTime(startDate).ToString("dd-MM-yyyy");

                if (!string.IsNullOrEmpty(priority))
                {
                    if (priority.Equals("1")) lblPriority.Text = "Normal";
                    else if (priority.Equals("0")) lblPriority.Text = "Normal";
                }
            }
        }

        protected void gvInProgressTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");
                Label lblAmount = (Label)e.Row.FindControl("lblAmount");
                Label lblStartingDate = (Label)e.Row.FindControl("lblStartingDate");
                Label lblDeadLine = (Label)e.Row.FindControl("lblDeadLine");
                Label lblPriority = (Label)e.Row.FindControl("lblPriority");

                lblSrNo.Text = Convert.ToString(e.Row.RowIndex + 1) + ".";

                string startingDate = Convert.ToString(gvInProgressTasks.DataKeys[e.Row.RowIndex].Values[0]);
                string deadLine = Convert.ToString(gvInProgressTasks.DataKeys[e.Row.RowIndex].Values[1]);
                string priority = Convert.ToString(gvInProgressTasks.DataKeys[e.Row.RowIndex].Values[2]);

                if (!string.IsNullOrEmpty(startingDate))
                    lblStartingDate.Text = Convert.ToDateTime(startingDate).ToString("dd-MM-yyyy");

                if (!string.IsNullOrEmpty(deadLine))
                    lblDeadLine.Text = Convert.ToDateTime(deadLine).ToString("dd-MM-yyyy");

                if (!string.IsNullOrEmpty(priority))
                {
                    if (priority.Equals("1")) lblPriority.Text = "Normal";
                    else if (priority.Equals("0")) lblPriority.Text = "Normal";
                }
            }
        }

        protected void btnReleaseAmount_Click(object sender, EventArgs e)
        {
            int index = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
            //string fullName = (string)this.gvPassedUsers.DataKeys[index].Values[2];
            //string email = (string)this.gvPassedUsers.DataKeys[index].Values[3];
            //string userName = (string)this.gvPassedUsers.DataKeys[index].Values[4];
            //string password = (string)this.gvPassedUsers.DataKeys[index].Values[5];

            //MyDBClass objMyDBClass = new MyDBClass();
            //string value = objMyDBClass.VerifyUser(fullName, email);

            //if (Convert.ToInt32(value) > 0)
            //{
            //    bool result = sendEmail(userName, password, email);

            //    if (result)
            //    {
            //        showSuccessMessage(fullName + " is successfully verified and email is sent.");
            //        BindPassedTestUsers();
            //    }
            //    else if (!result)
            //    {
            //        showMessage(fullName + " is successfully verified but there is some error while sending email.");
            //    }
            //}

            //else if (Convert.ToInt32(value) <= 0)
            //{
            //    showMessage("There is some error while verification of " + fullName);
            //}
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            string TransationId = tbxTransationId.Text;
            string trasactionAmount = tbxAmount.Text;
            int ddlTransactionTypeId = ddlTransactionType.SelectedIndex;
            string transactionType = string.Empty;

            if (ddlTransactionTypeId == 0) transactionType = ddlTransactionType.SelectedValue;
            if (ddlTransactionTypeId == 1) transactionType = tbxOther.Value;

            string aId = string.Empty;

            if (!string.IsNullOrEmpty(hfAId.Value)) aId = hfAId.Value;

            string userId = Convert.ToString(Session["LoginId"]);
            string status = "send";

            MyDBClass objMyDBClass = new MyDBClass();
            string value = objMyDBClass.ReleaseAmounts(userId, aId, status, trasactionAmount, transactionType, TransationId);

            if (string.IsNullOrEmpty(value))
                ucShowMessage1.ShowMessage(MessageTypes.Error, "Some error has occured and amount is not released.");
            else
            {
                ucShowMessage1.ShowMessage(MessageTypes.Success, trasactionAmount + " is released successfully.");
            }
        }

        protected void btnExportApprovedTasks_Click(object sender, EventArgs e)
        {
            HideHeader(gvApprovedTask, "Edit News");
            //HideHeader(gvApprovedTask, "Delete News");
            PrepareGridViewForExport(gvApprovedTask);
            ExportGridView(gvApprovedTask);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
        }

        protected void btnExportWorkingTasks_Click(object sender, EventArgs e)
        {
            HideHeader(gvInProgressTasks, "Edit News");
            //HideHeader(gvInProgressTasks, "Delete News");
            PrepareGridViewForExport(gvInProgressTasks);
            ExportGridView(gvInProgressTasks);
        }

        private void HideHeader(GridView gridView, string headerColumn)
        {
            try
            {
                for (int i = 0; i < gridView.HeaderRow.Cells.Count; i++)
                {
                    if (gridView.HeaderRow.Cells[i].Text.Equals(headerColumn))
                    {
                        gridView.HeaderRow.Cells[i].Visible = false;
                    }
                }
            }
            catch
            {
                throw;
            }
        }
        private void PrepareGridViewForExport(Control gv)
        {
            LinkButton lb = new LinkButton();
            Literal l = new Literal();
            string name = String.Empty;
            for (int i = 0; i < gv.Controls.Count; i++)
            {
                if (gv.Controls[i].GetType() == typeof(LinkButton))
                {
                    // l.Text = (gv.Controls[i] as LinkButton).Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    //  gv.Controls.AddAt(i, l);
                }
                else if (gv.Controls[i].GetType() == typeof(HyperLink))
                {
                    // l.Text = (gv.Controls[i] as HyperLink).Text;
                    //l.Text = "Details";
                    gv.Controls.Remove(gv.Controls[i]);
                    // gv.Controls.AddAt(i, l);
                }

                else if (gv.Controls[i].GetType() == typeof(DropDownList))
                {
                    //l.Text = (gv.Controls[i] as DropDownList).SelectedItem.Text;
                    gv.Controls.Remove(gv.Controls[i]);
                    // gv.Controls.AddAt(i, l);
                }

                else if (gv.Controls[i].GetType() == typeof(CheckBox))
                {
                    //l.Text = (gv.Controls[i] as CheckBox).Checked ? "True" : "False";
                    gv.Controls.Remove(gv.Controls[i]);
                    // gv.Controls.AddAt(i, l);
                }
                if (gv.Controls[i].HasControls())
                {
                    PrepareGridViewForExport(gv.Controls[i]);
                }
            }

        }
        private void ExportGridView(GridView grid)
        {

            string attachment = "attachment; filename=Data.xls";
            Response.ClearContent();
            Response.AddHeader("content-disposition", attachment);
            Response.ContentType = "application/ms-excel";
            StringWriter sw = new StringWriter();
            HtmlTextWriter htw = new HtmlTextWriter(sw);
            grid.RenderControl(htw);
            Response.Write(sw.ToString());
            Response.End();
        }
    }
}
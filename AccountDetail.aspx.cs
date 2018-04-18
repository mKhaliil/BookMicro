using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using BookMicroBeta;
using Outsourcing_System.PdfCompare_Classes;

namespace Outsourcing_System
{
    public partial class AccountDetail : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["objUser"] == null)
            {
                Response.Redirect("Login.aspx");
            }
            if (!Page.IsPostBack)
            {
                BindWithdrawRequests();
                BindApprovedRequests();
            }
            //ShowRecord();
        }

        //public void ShowRecord()
        //{
        //    if (!string.IsNullOrEmpty((Session["objUser"] as UserClass).UserID))
        //    {
        //        string qDetail ="SELECT A.DID,A.UID,A.Deposit, A.Withdraw,A.Description,A.Balance, A.Date FROM AccountDetail A inner Join [User] U on A.UID=U.UID" +
        //            " Where A.UID=" + (Session["objUser"] as UserClass).UserID;

        //        SqlDataSource1.SelectCommand = qDetail;
        //        this.GridView1.DataSourceID = SqlDataSource1.ID;

        //        string qBalance = "Select TotalAmount From AccountInformation Where UID=" +
        //                          (Session["objUser"] as UserClass).UserID;
        //        string balance = objMyDBClass.GetID(qBalance);
        //        string htmlVal = "Available Balance :&nbsp;<b>Rs " + Math.Round(double.Parse(balance), 2) + "</b>";
        //        this.balance.InnerHtml = htmlVal;
        //    }
        //}

        private void BindWithdrawRequests()
        {
            MyDBClass objMyDBClass = new MyDBClass();
            List<Transaction> approvedTasksList = objMyDBClass.GetWithdrawAmountRequests("pending");

            gvWithdrawAmountReq.DataSource = approvedTasksList;
            gvWithdrawAmountReq.DataBind();
        }

        private void BindApprovedRequests()
        {
            MyDBClass objMyDBClass = new MyDBClass();
            List<Transaction> approvedTasksList = objMyDBClass.GetWithdrawAmountRequests("send");

            gvApprovedRequests.DataSource = approvedTasksList;
            gvApprovedRequests.DataBind();
        }

        protected void gvWithdrawAmountReq_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");
                Label lblWithdrawDate = (Label)e.Row.FindControl("lblWithdrawDate");
                Label lblWithdrawAmount = (Label)e.Row.FindControl("lblWithdrawAmount");

                lblSrNo.Text = Convert.ToString(e.Row.RowIndex + 1) + ".";

                string withdrawDate = Convert.ToString(gvWithdrawAmountReq.DataKeys[e.Row.RowIndex].Values[1]);
                string withdrawAmount = Convert.ToString(gvWithdrawAmountReq.DataKeys[e.Row.RowIndex].Values[2]);

                if (!string.IsNullOrEmpty(withdrawDate))
                    lblWithdrawDate.Text = Convert.ToDateTime(withdrawDate).ToString("dd-MM-yyyy");

                if (!string.IsNullOrEmpty(withdrawAmount))
                    lblWithdrawAmount.Text = "AU $ " + Convert.ToString(Math.Round(Convert.ToDouble(withdrawAmount), 2));
            }
        }

        protected void gvApprovedRequests_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");
                Label lblWithdrawDate = (Label)e.Row.FindControl("lblWithdrawDate");
                Label lblWithdrawAmount = (Label)e.Row.FindControl("lblWithdrawAmount");
                Label lblTransactionDate = (Label)e.Row.FindControl("lblTransactionDate");
                Label lblTransactionAmount = (Label)e.Row.FindControl("lblTransactionAmount");

                lblSrNo.Text = Convert.ToString(e.Row.RowIndex + 1) + ".";

                string withdrawDate = Convert.ToString(gvApprovedRequests.DataKeys[e.Row.RowIndex].Values[0]);
                string withdrawAmount = Convert.ToString(gvApprovedRequests.DataKeys[e.Row.RowIndex].Values[1]);
                string transactionAmount = Convert.ToString(gvApprovedRequests.DataKeys[e.Row.RowIndex].Values[2]);
                string transactionDate = Convert.ToString(gvApprovedRequests.DataKeys[e.Row.RowIndex].Values[3]);

                if (!string.IsNullOrEmpty(withdrawDate))
                    lblWithdrawDate.Text = Convert.ToDateTime(withdrawDate).ToString("dd-MM-yyyy");

                if (!string.IsNullOrEmpty(withdrawAmount))
                    lblWithdrawAmount.Text = "AU $ " + Convert.ToString(Math.Round(Convert.ToDouble(withdrawAmount), 2));

                if (!string.IsNullOrEmpty(transactionDate))
                    lblTransactionDate.Text = Convert.ToDateTime(transactionDate).ToString("dd-MM-yyyy");

                if (!string.IsNullOrEmpty(transactionAmount))
                    lblTransactionAmount.Text = "AU $ " + Convert.ToString(Math.Round(Convert.ToDouble(transactionAmount), 2));
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
            string bonusAmount = tbxBonus.Text;
            int ddlTransactionTypeId = ddlTransactionType.SelectedIndex;
            string transactionType = string.Empty;

            if (ddlTransactionTypeId == 0) transactionType = ddlTransactionType.SelectedValue;
            if (ddlTransactionTypeId == 1) transactionType = tbxOther.Value;

            string wId = string.Empty;

            if (!string.IsNullOrEmpty(hfAId.Value)) wId = hfAId.Value;

            string userId = Convert.ToString(Session["LoginId"]);
            string status = "send";

            if (string.IsNullOrEmpty(trasactionAmount)) return;

            if (!string.IsNullOrEmpty(bonusAmount))
            {
                trasactionAmount = Convert.ToString(Convert.ToDouble(trasactionAmount) + Convert.ToDouble(bonusAmount));
            }

            MyDBClass objMyDBClass = new MyDBClass();
            string value = objMyDBClass.ReleaseAmounts(userId, wId, status, trasactionAmount, transactionType, TransationId);

            if (string.IsNullOrEmpty(value) || value == "-100")
                ucShowMessage1.ShowMessage(MessageTypes.Error, "Some error has occured and amount is not released.");
            else
            {
                ucShowMessage1.ShowMessage(MessageTypes.Success, "AU $ " + trasactionAmount + " is released successfully.");

                string queryUID = "Select UserId From Transactions_New where WId=" + wId;
                string UID = objMyDBClass.GetID(queryUID);

                string queryEmail = "Select email From [User] where Uid=" + UID;
                string email = objMyDBClass.GetID(queryEmail);

                sendEmail(email, "AU $ " + trasactionAmount + " is released successfully.");
                BindWithdrawRequests();
                BindApprovedRequests();
                Response.Redirect(Request.Url.AbsoluteUri);
            }
        }

        private bool sendEmail(string email, string message)
        {
            try
            {
                string From = "bookmicro123@gmail.com";
                string Password = "@sad@123";
                string To = email;
                //string To = "aamirghafoor2005@gmail.com";
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(From);
                mailMessage.To.Add(To);
                mailMessage.Bcc.Add("asad@readhowyouwant.com");
                mailMessage.Bcc.Add("rajaasad@gmail.com");
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "Payment Released";

                string applicationPath = Convert.ToString(ConfigurationManager.AppSettings["MainServerPath"]);
                mailMessage.Body = message + "<br/><br/>" +
                                   "Thanks," + "<br/> " + "Bookmicro Accounts Department";


                //"AU $ " + trasactionAmount + " is released successfully."


                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(From, Password);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
                //lblErrorMsg.Text = ex.Message;
                //lblErrorMsg.Visible = true;
                //divCanvas.Visible = false;
                //showMessage(ex.Message);
            }
        }

        protected void btnExportApprovedTasks_Click(object sender, EventArgs e)
        {
            HideHeader(gvApprovedRequests, "Edit News");
            //HideHeader(gvApprovedRequests, "Delete News");
            PrepareGridViewForExport(gvApprovedRequests);
            ExportGridView(gvApprovedRequests);
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
               server control at run time. */
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
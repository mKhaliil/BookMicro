using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using Outsourcing_System.PdfCompare_Classes;
using Outsourcing_System.ServiceReference1;
using System.Data;

namespace Outsourcing_System
{
    public partial class TestUserAccountDetails : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Convert.ToString(Session["LoginId"]) != "")
                {
                    ShowRecord();
                }
                else
                {
                    Response.Redirect("Bookmicro.aspx", true);
                }
            }
        }

        //public double GetMappingTaskSalary(string bookId, int taskCount, string taskName, int pageCount, DateTime startTime, DateTime endTime)
        //{
        //    string Servicedata = "";

        //    Service1Client client = new Service1Client();
        //    Servicedata = client.GetConversionFileDetailsForWebService(Convert.ToInt64(bookId), "test");

        //    client.Close();

        //    var result = Servicedata.Split('~');

        //    string totalPages = null;
        //    string complexity = null;
        //    bool scanned = false;
        //    bool IsTextChangeIssue = false;
        //    string RFPPushDate = null;
        //    string Status = null;

        //    var temp_complexity = result[1].Split(':');
        //    var temp_totalPages = result[2].Split(':');
        //    var temp_scanned = result[3].Split(':');
        //    var temp_IsTextChangeIssue = result[10].Split(':');
        //    var temp_RFPPushDate = result[11].Split(':');
        //    var status_temp = result[12].Split(':');

        //    totalPages = temp_totalPages[1];
        //    complexity = temp_complexity[1];
        //    scanned = Convert.ToBoolean(temp_scanned[1] == "False" ? 0 : 1);
        //    IsTextChangeIssue = Convert.ToBoolean(temp_IsTextChangeIssue[1] == "False" ? 0 : 1);
        //    RFPPushDate = temp_RFPPushDate[1];
        //    Status = status_temp[1];

        //    string dbDetail = objMyDBClass.getTaskThoughput("BookMicro", "conversion", complexity);
        //    double productivityHours = 0.0;

        //    if (dbDetail != "")
        //    {
        //        string[] splitedoutput = dbDetail.Split(' ');
        //        double expectedTime = Convert.ToDouble(splitedoutput[0]);
        //        double expectedOut = Convert.ToDouble(splitedoutput[1]) / 60;

        //        double bookunittime = (pageCount * Convert.ToDouble(splitedoutput[2])) / pageCount;
        //        double expectedHours = (pageCount * expectedOut) / 60 + bookunittime;

        //        double timeUnitPerPage = 0;
        //        double processePagesUnitTime = 0;

        //        if (Convert.ToDouble(splitedoutput[2]) != 0)
        //        {
        //            timeUnitPerPage = Convert.ToDouble(splitedoutput[2]) / pageCount;
        //            processePagesUnitTime = timeUnitPerPage * pageCount;
        //        }
        //        TimeSpan ts = endTime - startTime;

        //        double expetedPages = (ts.Minutes - processePagesUnitTime) * expectedTime;
        //        double result2 = pageCount - expetedPages;
        //        productivityHours = expectedHours - ts.Minutes;
        //    }

        //    double salary = objMyDBClass.GetUser_Salary("97");

        //    double workingDays = 22.0;
        //    double dailyRequiredTime = 6.0;

        //    double oneDaySalary = salary / workingDays;
        //    double oneHourSalary = oneDaySalary / dailyRequiredTime;

        //    //double taskSalary = ((productivityHours) / 60) / oneHourSalary;

        //    double taskSalary = (oneHourSalary / 60) * (productivityHours / 60);

        //    return taskSalary;
        //}

        public double GetSalary(string bookId, int taskCount, string taskName)
        {
            //string bookId = "30220";
            string Servicedata = "";
            string[] result = null;
            double taskSalary = 0;

            try
            {
                Service1Client client = new Service1Client();
                Servicedata = client.GetConversionFileDetailsForWebService(Convert.ToInt64(bookId), "test");

                client.Close();

                result = Servicedata.Split('~');
            }
            catch (Exception ex)
            {
                result[0] = "";
                return taskSalary;
            }

            string totalPages = null;
            string complexity = null;
            bool scanned = false;
            bool IsTextChangeIssue = false;
            string RFPPushDate = null;
            string Status = null;

            var temp_complexity = result[1].Split(':');
            var temp_totalPages = result[2].Split(':');
            var temp_scanned = result[3].Split(':');
            var temp_IsTextChangeIssue = result[10].Split(':');
            var temp_RFPPushDate = result[11].Split(':');
            var status_temp = result[12].Split(':');

            totalPages = temp_totalPages[1];
            complexity = temp_complexity[1];
            scanned = Convert.ToBoolean(temp_scanned[1] == "False" ? 0 : 1);
            IsTextChangeIssue = Convert.ToBoolean(temp_IsTextChangeIssue[1] == "False" ? 0 : 1);
            RFPPushDate = temp_RFPPushDate[1];
            Status = status_temp[1];

            string onetaskTime = objMyDBClass.GetTaskTime("BookMicro", taskName, complexity);
            double salary = objMyDBClass.GetUser_Salary("97");

            double workingDays = 22.0;
            double dailyRequiredTime = 6.0;

            double oneDaySalary = salary / workingDays;
            double oneHourSalary = oneDaySalary / dailyRequiredTime;

            //double taskSalary = ((Convert.ToDouble(onetaskTime) * taskCount) / 60) / oneHourSalary;

            taskSalary = (oneHourSalary / 60) * ((Convert.ToDouble(onetaskTime) * taskCount) / 60);

            return taskSalary;
        }

        public void ShowRecord()
        {
            string qDetail = "SELECT A.DID,A.UID,A.Deposit, A.Withdraw,A.Description,A.Balance, A.Date FROM AccountDetail A inner Join [User] U on A.UID=U.UID Where A.UID=" + Convert.ToString(Session["LoginId"]);//(Session["objUser"] as UserClass).UserID;

            SqlDataSource1.SelectCommand = qDetail;
            this.GridView1.DataSourceID = SqlDataSource1.ID;

            string qBalance = "Select TotalAmount From AccountInformation Where UID=" + Convert.ToString(Session["LoginId"]); // (Session["objUser"] as UserClass).UserID;
            string balance = objMyDBClass.GetID(qBalance);

            if (balance == "") return;
            string htmlVal = "Available Balance :&nbsp;<b>AU $ " + Math.Round(Convert.ToDouble(balance), 2) + "</b>";
            this.balance.InnerHtml = htmlVal;
            Session["Balance"] = Math.Round(double.Parse(balance), 2);

            string pendingAmountQuery = " Select sum(Expected) as expected From AccountDetail Where UID=" + Convert.ToString(Session["LoginId"]);// +" and date=" + ""; // (Session["objUser"] as UserClass).UserID;
            string pendingAmount = objMyDBClass.GetID(pendingAmountQuery);
            //string pending = "Pending Amount :&nbsp;<b>Rs " + Math.Round(double.Parse(pendingAmount == "" ? "0" : pendingAmount), 2) + "</b>";
            //this.pending.InnerHtml = pending;
        }

        protected void btnWithdraw_Click(object sender, EventArgs e)
        {
            divWithdrawAmount.Visible = true;
            lblBalance.Text = Convert.ToString(Session["Balance"]);
        }

        protected void btnWithdrawAmount_Click(object sender, EventArgs e)
        {
            string withdrawThreshould = ConfigurationManager.AppSettings["WithdrawThreshould"];
            divWithdrawAmount.Visible = false;

            string withdrawAmount = "";
            string balance = Convert.ToString(Session["Balance"]);

            if (string.IsNullOrEmpty(withdrawThreshould) || string.IsNullOrEmpty(balance) || balance == "0")
            {
                if (Convert.ToDouble(balance) < Convert.ToDouble(withdrawThreshould))
                {
                    ucShowMessage1.ShowMessage(MessageTypes.Error, "Amount can't be withdrawn because you don't have amount in your account.");
                    return;
                }
            }

            //string queryPayableEarning = "Select A.TotalAmount A From AccountInformation where A.UID=" + Convert.ToString(Session["LoginId"]);
            //DataSet dsPayableEarning = objMyDBClass.GetDataSet(queryPayableEarning);
            //DataRow dr = dsPayableEarning.Tables[0].Rows[0];
            //string userID = dr["UID"].ToString();
            //string task = dr["Task"].ToString();
            //double oldAmount = double.Parse(Session["accountpanel"].ToString().Split(new char[] { '@' })[1]);

            if (rbtnlAmount.SelectedIndex == 0)
                withdrawAmount = balance;

            else
                withdrawAmount = tbxWithdrawAmount.Text == "" ? "0" : tbxWithdrawAmount.Text;

            if ((Convert.ToDouble(balance) >= Convert.ToDouble(withdrawThreshould)))
            {
                ////Acount Detail
                //string qAccountDetail = "Insert into AccountDetail(UID,Deposit,Withdraw,Balance,Description,[Date]) Values(" + 
                //                        Convert.ToString(Session["LoginId"]) + "," + (double.Parse(txtPayableAmount.Text.Trim()) +
                //                        double.Parse(txtBonus.Text.Trim())) + withdrawAmount + totalAmount + ",'Amount against the task " + 
                //                        task.ToUpper() + " is withdrwn'," + DateTime.Now.Date.GetDateTimeFormats('d')[5] + ")";

                //objMyDBClass.ExecuteCommand(qAccountDetail);
                //End Account Detail    

                if (Convert.ToDouble(withdrawAmount) > Convert.ToDouble(balance))
                {
                    //showMessage("Withdraw amount must be less then available balance.");
                    ucShowMessage1.ShowMessage(MessageTypes.Error, "Withdraw amount must be less then available balance.");
                    return;
                }

                string res = objMyDBClass.InsertWithdrawAmount(Convert.ToString(Session["LoginId"]), withdrawAmount);
                string msg = "Request for withdraw amount AU $ " + withdrawAmount + " is made successfully.";

                if (Convert.ToInt32(res) > 2 && sendEmail(Convert.ToString(Session["email"]), msg))
                {
                    //showSuccessMessage("Request for withdraw amount Rs " + withdrawAmount + " is successfully made.");
                    ucShowMessage1.ShowMessage(MessageTypes.Success, "Request for withdraw amount AU $ " + withdrawAmount + " is made successfully.");
                    ;
                    ShowRecord();
                    Response.Redirect(Request.Url.AbsoluteUri);
                }
            }

            else
            {
                //showMessage("Amount less then 1000 can not be withdrawn.");
                ucShowMessage1.ShowMessage(MessageTypes.Error, "Amount less then 1000 can not be withdrawn.");
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            divWithdrawAmount.Visible = false;
        }

        protected void rbtnlAmount_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rbtnlAmount.SelectedIndex == 0)
                tbxWithdrawAmount.Visible = false;

            else
                tbxWithdrawAmount.Visible = true;
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("BookMicro.aspx");
        }

        protected void GridView1_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[2].Text.Equals("0.00"))
                    e.Row.Cells[2].Text = "";

                if (e.Row.Cells[3].Text.Equals("0.00"))
                    e.Row.Cells[3].Text = "";

                if (e.Row.Cells[4].Text.Equals("0.00"))
                    e.Row.Cells[4].Text = "";
            }
        }

        //protected void ddlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    if (ddlTransactionType.SelectedIndex == 1)
        //        tbxOther.Visible = true;

        //    else
        //        tbxOther.Visible = false;
        //}

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
                mailMessage.Subject = "Withdraw Amount Request";

                string applicationPath = Convert.ToString(ConfigurationManager.AppSettings["MainServerPath"]);
                //mailMessage.Body = "Congratulations! You have passed the test and now assigned a role of \"Trainee Editor at BookMicro\".<br/> <br/> " +
                //                    "Please click <a href='" + applicationPath + "/UserDetails.aspx?uid=" + uid + "&email=" + email + "'>here</a> to complete your profile. <br/> <br/>" +
                //                    "Note: Tests given after training are paid, if you qualify the test in first two attempts. On qualification of any training level, amount of passed test will be added to your account. To withdraw the amount, you need to successfully complete atleast 5 tasks in the same category.<br/><br/><br/>" +
                //                    "Welcome to BookMicro Community and wish you best of Luck!<br/><br/>BookMicro Team will contact you soon <br/><br/>BookMicro Hiring Team";

                //"Request for withdraw amount AU $ " + withdrawAmount + " is made successfully.

                mailMessage.Body = message + "<br/><br/>" +
                                   "As soon as your payment is released. You will be notified via email.<br/><br/><br/>" +
                                   "Thanks," + "<br/>" +
                                   "Bookmicro Accounts Department";

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
    }
}
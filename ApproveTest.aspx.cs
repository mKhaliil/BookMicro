using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Net.Mail;
using System.Net;
using System.IO;

namespace Outsourcing_System
{
    public partial class ApproveTest : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //int UserID = Convert.ToInt32(Request.QueryString.Get("UserId"));

                //if (UserID != 0)
                //{
                //    //((OnlineTestRegisterdMaster)this.Page.Master).SetLogOut = true;
                //    ((OnlineTestMasterPage)this.Page.Master).SetLogOut = true;
                //    ((OnlineTestMasterPage)this.Page.Master).SetMenuLocation = "-120px";
                //    Session["LoginId"] = UserID;
                //}

                if (Session["LoginId"] == null)
                {
                    Response.Redirect("Bookmicro.aspx", false);
                    return;
                }

                BindFailedTestUsers();
                BindPassedTestUsers();
                BindPendingTests();
                //BindAccountPanel();

                hfAmountSelected.Value = "";
            }
        }

        protected void btnFailTest_Click(object sender, EventArgs e)
        {
            int index = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
            string testId = (string)this.gvPendingTests.DataKeys[index].Values[0];
            MyDBClass objMyDBClass = new MyDBClass();
            string value = objMyDBClass.UpdateTestStatus(Convert.ToInt32(testId), "Failed");
            BindPendingTests();
        }
        protected void btnApproveTest_Click(object sender, EventArgs e)
        {
            int index = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
            string testId = (string)this.gvPendingTests.DataKeys[index].Values[0];
            MyDBClass objMyDBClass = new MyDBClass();
            string value = objMyDBClass.UpdateTestStatus(Convert.ToInt32(testId), "Passed");
            BindPendingTests();
        }


        protected void btnDownloadTest_Click(object sender, EventArgs e)
        {
            //int index = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
            //Label testName = (Label)gvPendingTests.Rows[index].FindControl("lblTestName");
            //Label testType = (Label)gvPendingTests.Rows[index].FindControl("lblTestTypw");
            //Label userEmail = (Label)gvPendingTests.Rows[index].FindControl("lblUser");
            //ZipUtility zip = new ZipUtility();
            ////string originalTestPdf = "Z:\\Files\\Tests\\Original\\" + testType.Text + "/" + testName.Text; //commented by aamir
            ////string userTestFolder = @"Z:\Files\Tests\" + userEmail.Text + "/" + testType.Text + "Tests/" + testName.Text.Replace(".pdf", "");
            ////string newFolder = "Z:/DownloadTestToCheck";

            //string originalTestPdf = "D:\\Office Data\\Files\\Tests\\Original\\" + testType.Text + "/" + testName.Text;
            //string userTestFolder = @"D:\\Office Data\\Files\Tests\" + userEmail.Text + "/" + testType.Text + "Tests/" + testName.Text.Replace(".pdf", "");
            //string newFolder = "D:\\Office Data\\Files\\DownloadTestToCheck";

            int index = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
            Label testName = (Label)gvPendingTests.Rows[index].FindControl("lblTestName");
            Label testType = (Label)gvPendingTests.Rows[index].FindControl("lblTestTypw");
            Label userEmail = (Label)gvPendingTests.Rows[index].FindControl("lblUser");
            ZipUtility zip = new ZipUtility();
            //string originalTestPdf = "Z:\\Files\\Tests\\Original\\" + testType.Text + "/" + testName.Text; //commented by aamir
            //string userTestFolder = @"Z:\Files\Tests\" + userEmail.Text + "/" + testType.Text + "Tests/" + testName.Text.Replace(".pdf", "");
            //string newFolder = "Z:/DownloadTestToCheck";

            string originalTestPdf = "D:\\Files\\Tests\\Original\\" + testType.Text.Replace("s", "") + "/" + testName.Text;
            string userTestFolder = @"D:\\Files\Tests\" + userEmail.Text + "/" + testType.Text.Replace("s", "") + "Tests/" + testName.Text.Replace(".pdf", "");
            string newFolder = "D:\\Files\\DownloadTestToCheck";

            if (Directory.Exists(newFolder))
            {
                Directory.Delete(newFolder, true);
                File.Delete(newFolder + ".zip");

            }
            if (!Directory.Exists(newFolder))
            {
                Directory.CreateDirectory(newFolder);
            }
            DirectoryInfo dInfo = CopyTo(new DirectoryInfo(userTestFolder), newFolder);

            if (dInfo == null)
            {
                return;
            }

            File.Copy(originalTestPdf, newFolder + "/OrignalTest.pdf");
            zip.setdirToZip(newFolder);
            // zip.ZipPath = newFolder;

            //if (formatsFolder[j] != "LS")
            //{
            //    zip.ZipPath = txtFolder.Text + "\\" + formatsFolder[j] + "\\" + withoutExtension + formatsFolder[j];
            //}
            //else
            //{
            //    zip.ZipPath = txtFolder.Text + "\\" + formatsFolder[j] + "\\" + withoutExtension;
            //}
            zip.ZIPDirectory(newFolder);
            Context.Response.Clear();
            Context.Response.ContentType = "application/x-zip-compressed";
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=TestDetails.zip");
            Context.Response.WriteFile(newFolder + ".zip");
            //Response.Redirect("ImageTest.aspx");

            Context.Response.End();
        }
        public DirectoryInfo CopyTo(DirectoryInfo sourceDir, string destinationPath, bool overwrite = false)
        {
            var sourcePath = sourceDir.FullName;

            var destination = new DirectoryInfo(destinationPath);

            destination.Create();

            if (!Directory.Exists(sourcePath))
            {
                return null;
            }

            foreach (var sourceSubDirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(sourceSubDirPath.Replace(sourcePath, destinationPath));

            foreach (var file in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
                File.Copy(file, file.Replace(sourcePath, destinationPath), overwrite);

            return destination;
        }

        //private void BindAccountPanel()
        //{
        //    try
        //    {
        //        MyDBClass objMyDBClass = new MyDBClass();
        //        List<Transaction> list_Transactions = objMyDBClass.GetWithdrawAmounts();

        //        if ((list_Transactions != null) && (list_Transactions.Count > 0))
        //        {
        //            gvAccountPanel.DataSource = list_Transactions;
        //            gvAccountPanel.DataBind();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        showMessage(ex.Message);
        //    }
        //}
        private void BindPendingTests()
        {

            try
            {
                MyDBClass objMyDBClass = new MyDBClass();
                List<TestDetail> list_tasks = objMyDBClass.GetPendingTests();
                gvPendingTests.DataSource = list_tasks;
                gvPendingTests.DataBind();
            }
            catch (Exception ex)
            {
                showMessage(ex.Message);
            }
        }
        private void BindFailedTestUsers()
        {
            try
            {
                MyDBClass objMyDBClass = new MyDBClass();
                List<TestUser> list_tasks = objMyDBClass.GetFailedTestUsers();

                List<TestUser> list_final = new List<TestUser>();

                if ((list_tasks != null) && (list_tasks.Count > 0))
                {
                    foreach (var item in list_tasks)
                    {
                        if (!CheckDublicateUsers(list_final, item))
                        {
                            list_final.Add(item);
                        }
                    }
                    gvFailedUsers.DataSource = list_final;
                    gvFailedUsers.DataBind();
                }
            }
            catch (Exception ex)
            {
                showMessage(ex.Message);
            }
        }

        private void BindPassedTestUsers()
        {
            try
            {
                MyDBClass objMyDBClass = new MyDBClass();
                List<TestUser> list_tasks = objMyDBClass.GetPassedTestUsers();

                List<TestUser> list_final = new List<TestUser>();

                if ((list_tasks != null) && (list_tasks.Count > 0))
                {
                    foreach (var item in list_tasks)
                    {
                        if (!CheckDublicateUsers(list_final, item))
                        {
                            list_final.Add(item);
                        }
                        else
                        {
                            GetMax_PassedTestScore(list_final, item);
                        }
                    }
                    gvPassedUsers.DataSource = list_final;
                    gvPassedUsers.DataBind();
                }
            }
            catch (Exception ex)
            {
                showMessage(ex.Message);
            }
        }

        public bool CheckDublicateUsers(List<TestUser> list_final, TestUser user)
        {
            foreach (var item in list_final)
            {
                if ((item.FullName == user.FullName) && (item.Email == user.Email))
                {
                    return true;
                }
            }
            return false;
        }

        public List<TestUser> GetMax_PassedTestScore(List<TestUser> list_final, TestUser user)
        {
            List<string> list_TestResult = new List<string>();

            foreach (var item in list_final)
            {
                if ((item.FullName.Trim() == user.FullName.Trim()) && (item.Email.Trim() == user.Email.Trim()))
                {
                    if ((user.ObtainedScore.Trim() != "") && (user.TotalScore.Trim() != "") && (item.ObtainedScore.Trim() != "") && (item.TotalScore.Trim() != ""))
                    {
                        if ((Convert.ToDouble(user.ObtainedScore.Trim()) / Convert.ToDouble(user.TotalScore.Trim())) > (Convert.ToDouble(item.ObtainedScore.Trim()) / Convert.ToDouble(item.TotalScore.Trim())))
                            list_TestResult.Add(user.TestName.Trim() + "," + user.ObtainedScore.Trim() + "," + user.TotalScore.Trim() + "," + user.TestDate.Trim());

                    }
                }
            }

            if ((list_TestResult != null) && (list_TestResult.Count > 0))
            {
                for (int i = 0; i < list_final.Count; i++)
                {
                    if ((list_final[i].FullName.Trim() == user.FullName.Trim()) && (list_final[i].Email.Trim() == user.Email.Trim()))
                    {
                        list_final[i].TestName = list_TestResult[0].Split(',')[0];
                        list_final[i].ObtainedScore = list_TestResult[0].Split(',')[1];
                        list_final[i].TotalScore = list_TestResult[0].Split(',')[2];
                        list_final[i].TestDate = list_TestResult[0].Split(',')[3];
                    }
                }
            }

            return list_final;
        }


        private void showMessage(string message)
        {
            if (message != "")
            {
                DivError.Visible = true;
                lblError.Text = message;
            }
            else
            {
                DivError.Visible = false;
            }
        }

        protected void gvAccountPanel_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");
                Label Status = (Label)e.Row.FindControl("lblStatus");
                Label lblTransactionDate = (Label)e.Row.FindControl("lblTransactionDate");
                Button btnReleaseAmount = (Button)e.Row.FindControl("btnReleaseAmount");

                lblSrNo.Text = Convert.ToString(e.Row.RowIndex + 1) + ".";

                if (Status.Text.Equals("send"))
                {
                    btnReleaseAmount.Text = "Done";
                    btnReleaseAmount.Enabled = false;
                }

                if (lblTransactionDate.Text.Trim().Equals("1/1/0001 12:00:00 AM"))
                {
                    lblTransactionDate.Text = "";
                }
            }
        }

        protected void gvFailedUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPassedScore = (Label)e.Row.FindControl("lblPassedScore");
                Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");
                lblSrNo.Text = Convert.ToString(e.Row.RowIndex + 1) + ".";

                string TotalScore = gvFailedUsers.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string ObtainedScor = gvFailedUsers.DataKeys[e.Row.RowIndex].Values[1].ToString();

                double percentage = 0.0;

                if ((TotalScore != "") && (ObtainedScor != ""))
                {
                    percentage = (Convert.ToDouble(ObtainedScor) / Convert.ToDouble(TotalScore)) * 100;

                    lblPassedScore.Text = Convert.ToString(Math.Round(percentage, 2));
                }
            }
        }

        protected void gvPassedUsers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblPassedScore = (Label)e.Row.FindControl("lblPassedScore");
                Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                Button btnApproveProfile = (Button)e.Row.FindControl("btnApproveProfile");
                lblSrNo.Text = Convert.ToString(e.Row.RowIndex + 1) + ".";

                string TotalScore = gvPassedUsers.DataKeys[e.Row.RowIndex].Values[0].ToString();
                string ObtainedScor = gvPassedUsers.DataKeys[e.Row.RowIndex].Values[1].ToString();
                string verification = gvPassedUsers.DataKeys[e.Row.RowIndex].Values[6].ToString();

                if (verification != "")
                {
                    if (verification.Trim().Equals("1"))
                    {
                        btnApproveProfile.Enabled = false;
                        btnApproveProfile.Text = "Approved";
                        btnApproveProfile.BackColor = Color.LawnGreen;
                        //btnApproveProfile.Attributes.Add("onmouseover", "this.style.cursor='pointer'"); 
                        //btnApproveProfile.Attributes["class"] = "hidden";
                    }
                }

                double percentage = 0.0;

                if ((TotalScore != "") && (ObtainedScor != ""))
                {
                    percentage = (Convert.ToDouble(ObtainedScor) / Convert.ToDouble(TotalScore)) * 100;

                    lblPassedScore.Text = Convert.ToString(Math.Round(percentage, 2));
                }
            }
        }

        protected void lbtnView_Click(object sender, EventArgs e)
        {
            int index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
            divViewUserDetails.Visible = true;

            string name = (string)this.gvPassedUsers.DataKeys[index].Values[2];
            string email = (string)this.gvPassedUsers.DataKeys[index].Values[3];

            LoadUserProfiole(name, email);
        }

        //protected void btnReleaseAmount_Click(object sender, EventArgs e)
        //{
        //    int index = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
        //    divReleaseAmount.Visible = true;

        //    string userId = (string)this.gvAccountPanel.DataKeys[index].Values[0];
        //    DateTime withdrawDate = (DateTime)this.gvAccountPanel.DataKeys[index].Values[1];
        //    string status = (string)this.gvAccountPanel.DataKeys[index].Values[2];
        //    string rowId = (string)this.gvAccountPanel.DataKeys[index].Values[3];

        //    hfAmountSelected.Value = userId + "," + rowId + "," + status;

        //    BindAccountPanel();
        //}

        private void LoadUserProfiole(string name, string email)
        {
            MyDBClass objMyDBClass = new MyDBClass();
            TestUser user = objMyDBClass.GetUserProfile_ForAdmin(name, email);

            if (user != null)
            {
                lblFullName.Text = user.FullName;
                lblEmail.Text = user.Email;
                lblCnicNo.Text = user.CNICNO;
                lblAccountNumber.Text = user.AccountNum;
                tbxMobileNumber.Text = user.MobileNumber;
                lblEducation.Text = user.Education;
                imgUserProfile.ImageUrl = user.Picture;

                lblDescription.Text = user.Description;
                tbxExperience.Text = user.Experience;
            }
        }

        protected void btnApproveProfile_Click(object sender, EventArgs e)
        {
            int index = ((GridViewRow)((Button)sender).Parent.Parent).RowIndex;
            string fullName = (string)this.gvPassedUsers.DataKeys[index].Values[2];
            string email = (string)this.gvPassedUsers.DataKeys[index].Values[3];
            string userName = (string)this.gvPassedUsers.DataKeys[index].Values[4];
            string password = (string)this.gvPassedUsers.DataKeys[index].Values[5];

            MyDBClass objMyDBClass = new MyDBClass();
            string value = objMyDBClass.VerifyUser(fullName, email);

            if (Convert.ToInt32(value) > 0)
            {
                bool result = sendEmail(userName, password, email);

                if (result)
                {
                    showSuccessMessage(fullName + " is successfully verified and email is sent.");
                    BindPassedTestUsers();
                }
                else if (!result)
                {
                    showMessage(fullName + " is successfully verified but there is some error while sending email.");
                }
            }

            else if (Convert.ToInt32(value) <= 0)
            {
                showMessage("There is some error while verification of " + fullName);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            divViewUserDetails.Visible = false;
        }

        protected void btnDone_Click(object sender, EventArgs e)
        {
            divReleaseAmount.Visible = false;

            string TransationId = tbxTransationId.Text;
            string trasactionAmount = tbxAmount.Text;
            int ddlTransactionTypeId = ddlTransactionType.SelectedIndex;

            if (hfAmountSelected.Value != "")
            {
                string userId = hfAmountSelected.Value.Split(',')[0];
                string rowId = hfAmountSelected.Value.Split(',')[1];
                string status = hfAmountSelected.Value.Split(',')[2];

                MyDBClass objMyDBClass = new MyDBClass();
                string value = objMyDBClass.ReleaseAmounts(userId, rowId, status, trasactionAmount, Convert.ToString(ddlTransactionTypeId), TransationId);

                //BindAccountPanel();
                //Acount Detail
                //string qAccountDetail = "Insert into AccountDetail(UID,Deposit,Withdraw,Balance,Description,[Date]) Values(" + userId + "," + (double.Parse(txtPayableAmount.Text.Trim()) + double.Parse(txtBonus.Text.Trim() == "" ? "0" : txtBonus.Text)) + ",0.00," + totalAmount + ",'Amount against the task " + task.ToUpper() + " is deposited'," + DateTime.Now.Date.GetDateTimeFormats('d')[5] + ")";
                //objMyDBClass.ExecuteCommand(qAccountDetail);
            }
        }

        protected void btnClosedivReleaseAmount_Click(object sender, EventArgs e)
        {
            divReleaseAmount.Visible = false;
        }

        private void showSuccessMessage(string message)
        {
            if (message != "")
            {
                divSuccess.Visible = true;
                lblSuccess.Text = message;
                lblSuccess.ForeColor = Color.Blue;
            }
            else
            {
                divSuccess.Visible = false;
            }
        }

        private bool sendEmail(string userName, string password, string userEmail)
        {
            try
            {
                string From = "bookmicro123@gmail.com";
                string Password = "@sad@123";
                string To = userEmail;

                MyDBClass objMyDBClass = new MyDBClass();

                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(From);
                mailMessage.To.Add(To);
                mailMessage.Bcc.Add("asad@readhowyouwant.com");
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "Congratulations!";

                mailMessage.Body = "Congratulations! You have passed the test and now assigned a role of \"Trainee Editor at BookMicro\".<br/> <br/> " + "Your email is " + userEmail +
                                    " and password is " + password + "<br/><br/>BookMicro Hiring Team";
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.Credentials = new NetworkCredential(From, Password);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        protected void ddlTransactionType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlTransactionType.SelectedIndex == 1)
                tbxOther.Visible = true;

            else
                tbxOther.Visible = false;
        }
    }
}
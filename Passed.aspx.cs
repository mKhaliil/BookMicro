using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;
using System.Net;
using System.Text;
using Outsourcing_System.MasterPages;

namespace Outsourcing_System
{
    public partial class Passed : System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{
        //    if (!IsPostBack)
        //    {
        //        //((OnlineTestMasterPage)this.Page.Master).SetLogIn = false;
        //        //((OnlineTestMasterPage)this.Page.Master).SetMenuLocation = "-120px";

        //        ((UserMaster)this.Page.Master).HideLogInButton();

        //        string totalMarks = "";
        //        string obtained = "";
        //        string userName = "";
        //        string email = "";
        //        string testName = "";

        //        string testType = Convert.ToString(Request.QueryString["type"]);
        //        double per = Convert.ToDouble(Request.QueryString["p"]);

        //        if (testType != null)
        //        {
        //            if (testType.Equals("test"))
        //            {
        //                GetComparisonTestResult(per);
        //            }
        //        }
        //        else
        //        {
        //            if (Session["Result"] == null)
        //            {
        //                Response.Redirect("Bookmicro.aspx", true);
        //            }

        //            if (Session["Result"].ToString() != "")
        //            {
        //                double percentage = 0.0;
        //                obtained = Convert.ToString(Session["Result"]);

        //                if (obtained == "") return;
        //                if (obtained != "")
        //                {
        //                    double totalMistakes = Convert.ToDouble(Session["TotalMarks"]);
        //                    percentage = (Convert.ToDouble(obtained) / totalMistakes) * 100;
        //                    if (Convert.ToDouble(percentage) < 80) Response.Redirect("BookMicro.aspx", true);
        //                }

        //                MyDBClass objMyDBClass = new MyDBClass();

        //                if (Session["OnlineTestUser"] != null)
        //                {
        //                    totalMarks = Convert.ToString(objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineTestUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"])));
        //                }
        //                else if (Session["OnlineUser"] != null)
        //                {
        //                    totalMarks = Convert.ToString(objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"])));
        //                }
        //                percentage = Convert.ToDouble(obtained.Trim()) / Convert.ToDouble(totalMarks.ToString().Trim()) * 100;

        //                lbl.Attributes.Add("data-percent", Convert.ToString(percentage));

        //                if ((Convert.ToString(Session["TestName"]) != null) && (Convert.ToString(Session["TestName"]) != ""))
        //                {
        //                    testName = Convert.ToString(Session["TestName"]);
        //                }
        //                if ((Convert.ToString(Session["OnlineTestUser"]) != null) && (Convert.ToString(Session["OnlineTestUser"]) != ""))
        //                {
        //                    userName = Convert.ToString(Session["OnlineTestUser"]);
        //                }
        //                else
        //                {
        //                    userName = Session["OnlineUser"].ToString();
        //                }
        //                if ((Convert.ToString(Session["email"]) != null) && (Convert.ToString(Session["email"]) != ""))
        //                {
        //                    email = Convert.ToString(Session["email"]);
        //                }

        //                string rows = objMyDBClass.SaveResult(userName, email, testName, obtained, "passed");

        //                string rows1 = objMyDBClass.MovePassedResult(userName, email);

        //                string userId = CreateUserLogin(email);
        //                Session["LoginId"] = userId;
        //                sendEmail(obtained, email, userId);
        //            }
        //        }
        //    }
        //}

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //((OnlineTestMasterPage)this.Page.Master).SetLogIn = false;
                //((OnlineTestMasterPage)this.Page.Master).SetMenuLocation = "-120px";

                ((UserMaster)this.Page.Master).HideLogInButton();

                string totalMarks = "";
                string obtained = "";
                string userName = "";
                string email = "";
                string testName = "";

                string testType = Convert.ToString(Request.QueryString["type"]);
                double per = Convert.ToDouble(Request.QueryString["p"]);

                if (testType == null)
                {
                    if (string.IsNullOrEmpty(Convert.ToString(Session["Result"])) || string.IsNullOrEmpty(Convert.ToString(Session["TotalMarks"])))
                        Response.Redirect("Bookmicro.aspx", true);

                        h4UpgradedTestStatus.Attributes.Add("style", "display:none");
                        divGoToUserPage.Attributes.Add("style", "display:none");
                        h4UpgradedSampleTestStatus.Attributes.Add("style", "display:none");

                        h4TestStatus.Attributes.Add("style", "display:block");
                        divProfileLink.Attributes.Add("style", "display:block");

                        double percentage = 0.0;
                        obtained = Convert.ToString(Session["Result"]);

                        if (obtained == "") return;
                        if (obtained != "")
                        {
                            double totalMistakes = Convert.ToDouble(Session["TotalMarks"]);
                            percentage = (Convert.ToDouble(obtained) / totalMistakes) * 100;
                            if (Convert.ToDouble(percentage) < 80) Response.Redirect("BookMicro.aspx", true);
                        }

                        MyDBClass objMyDBClass = new MyDBClass();

                        if (Session["OnlineTestUser"] != null)
                        {
                            totalMarks = Convert.ToString(objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineTestUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"])));
                        }
                        else if (Session["OnlineUser"] != null)
                        {
                            totalMarks = Convert.ToString(objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"])));
                        }
                        percentage = Convert.ToDouble(obtained.Trim()) / Convert.ToDouble(totalMarks.ToString().Trim()) * 100;

                        lbl.Attributes.Add("data-percent", Convert.ToString(percentage));

                        if ((Convert.ToString(Session["TestName"]) != null) && (Convert.ToString(Session["TestName"]) != ""))
                        {
                            testName = Convert.ToString(Session["TestName"]);
                        }
                        if ((Convert.ToString(Session["OnlineTestUser"]) != null) && (Convert.ToString(Session["OnlineTestUser"]) != ""))
                        {
                            userName = Convert.ToString(Session["OnlineTestUser"]);
                        }
                        else
                        {
                            userName = Session["OnlineUser"].ToString();
                        }
                        if ((Convert.ToString(Session["email"]) != null) && (Convert.ToString(Session["email"]) != ""))
                        {
                            email = Convert.ToString(Session["email"]);
                        }

                        string rows = objMyDBClass.SaveResult(userName, email, testName, obtained, "passed");

                        string rows1 = objMyDBClass.MovePassedResult(userName, email);

                        string userId = CreateUserLogin(email);
                        Session["LoginId"] = userId;
                        sendEmail(obtained, email, userId);
                }
                if (testType != null)
                {
                    if (testType.Equals("test"))
                    {
                        h4UpgradedTestStatus.Attributes.Add("style", "display:none");
                        divGoToUserPage.Attributes.Add("style", "display:none");
                        h4UpgradedSampleTestStatus.Attributes.Add("style", "display:none");

                        h4TestStatus.Attributes.Add("style", "display:block");
                        divProfileLink.Attributes.Add("style", "display:block");

                        GetComparisonTestResult(per);
                    }
                    else if (testType.Equals("CompUpgradedStartTest"))
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(Session["Result"])) || string.IsNullOrEmpty(Convert.ToString(Session["TotalMarks"])))
                            Response.Redirect("Bookmicro.aspx", true);

                        h4UpgradedTestStatus.Attributes.Add("style", "display:block");
                        divGoToUserPage.Attributes.Add("style", "display:block");
                        h4UpgradedSampleTestStatus.Attributes.Add("style", "display:none");

                        h4TestStatus.Attributes.Add("style", "display:none");
                        divProfileLink.Attributes.Add("style", "display:none");

                            double percentage = 0.0;
                            obtained = Convert.ToString(Session["Result"]);

                            if (obtained == "") return;
                            if (obtained != "")
                            {
                                double totalMistakes = Convert.ToDouble(Session["TotalMarks"]);
                                percentage = (Convert.ToDouble(obtained) / totalMistakes) * 100;
                                if (Convert.ToDouble(percentage) < 80) Response.Redirect("BookMicro.aspx", true);
                            }

                            MyDBClass objMyDBClass = new MyDBClass();

                            if (Session["OnlineTestUser"] != null)
                            {
                                totalMarks = Convert.ToString(objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineTestUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"])));
                            }
                            else if (Session["OnlineUser"] != null)
                            {
                                totalMarks = Convert.ToString(objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"])));
                            }
                            percentage = Convert.ToDouble(obtained.Trim()) / Convert.ToDouble(totalMarks.ToString().Trim()) * 100;

                            lbl.Attributes.Add("data-percent", Convert.ToString(percentage));

                            if ((Convert.ToString(Session["TestName"]) != null) && (Convert.ToString(Session["TestName"]) != ""))
                            {
                                testName = Convert.ToString(Session["TestName"]);
                            }
                            if ((Convert.ToString(Session["OnlineTestUser"]) != null) && (Convert.ToString(Session["OnlineTestUser"]) != ""))
                            {
                                userName = Convert.ToString(Session["OnlineTestUser"]);
                            }
                            else
                            {
                                userName = Session["OnlineUser"].ToString();
                            }
                            if ((Convert.ToString(Session["email"]) != null) && (Convert.ToString(Session["email"]) != ""))
                            {
                                email = Convert.ToString(Session["email"]);
                            }

                            string rows = objMyDBClass.SaveResult(userName, email, testName, obtained, "passed");

                            string rows1 = objMyDBClass.MovePassedResult(userName, email);

                            string userId = objMyDBClass.GetUserIdByEmail(email);
                            if (!string.IsNullOrEmpty(userId))
                            {
                                objMyDBClass.SetComparisonRights(Convert.ToInt32(userId), "ErrorDetectionComplexBits", "passed", "ErrorDetectionComplexBits");
                                objMyDBClass.SetUserIsActiveStatus(userId);
                            }

                            //string userId = CreateUserLogin(email);
                            //Session["LoginId"] = userId;
                            //sendEmail(obtained, email, userId);
                    }
                    else if (testType.Equals("CompUpgradedSampleTest"))
                    {
                        if (string.IsNullOrEmpty(Convert.ToString(Session["Result"])) || string.IsNullOrEmpty(Convert.ToString(Session["TotalMarks"])))
                            Response.Redirect("Bookmicro.aspx", true);

                        h4UpgradedTestStatus.Attributes.Add("style", "display:none");
                        divGoToUserPage.Attributes.Add("style", "display:block");
                        h4UpgradedSampleTestStatus.Attributes.Add("style", "display:block");

                        h4TestStatus.Attributes.Add("style", "display:none");
                        divProfileLink.Attributes.Add("style", "display:none");

                        double percentage = 0.0;
                        obtained = Convert.ToString(Session["Result"]);

                        if (obtained == "") return;
                        if (obtained != "")
                        {
                            double totalMistakes = Convert.ToDouble(Session["TotalMarks"]);
                            percentage = (Convert.ToDouble(obtained) / totalMistakes) * 100;
                            if (Convert.ToDouble(percentage) < 80) Response.Redirect("BookMicro.aspx", true);
                        }

                        MyDBClass objMyDBClass = new MyDBClass();

                        if (Session["OnlineTestUser"] != null)
                        {
                            totalMarks = Convert.ToString(objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineTestUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"])));
                        }
                        else if (Session["OnlineUser"] != null)
                        {
                            totalMarks = Convert.ToString(objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"])));
                        }
                        percentage = Convert.ToDouble(obtained.Trim()) / Convert.ToDouble(totalMarks.ToString().Trim()) * 100;
                        lbl.Attributes.Add("data-percent", Convert.ToString(percentage));
                    }
                }
            }
        }

        public String CreateUserLogin(string email)
        {
            MyDBClass objMyDBClass = new MyDBClass();
            string fullName = Convert.ToString(Session["OnlineTestUser"]);
            string uid = objMyDBClass.SavePassedUserLogin(fullName, email);
            return uid;
        }

        public DirectoryInfo CopyTo(DirectoryInfo sourceDir, string destinationPath, bool overwrite = false)
        {
            var sourcePath = sourceDir.FullName;

            var destination = new DirectoryInfo(destinationPath);

            destination.Create();

            foreach (var sourceSubDirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(sourceSubDirPath.Replace(sourcePath, destinationPath));

            foreach (var file in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
                File.Copy(file, file.Replace(sourcePath, destinationPath), overwrite);

            return destination;
        }

        protected void imgbtnLogin_Click(object sender, System.EventArgs e)
        {
            MyDBClass objMyDBClass = new MyDBClass();

            Session.Clear();

            if (Convert.ToString(Session["LoginId"]) == "")
            {
                if (cbxRememberMe.Checked) //If user checks keep me signed in
                {
                    Response.Cookies["email"].Expires = DateTime.Now.AddDays(1);
                    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(1);
                }
                //else //If next time user don't want to save password
                //{
                //    if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
                //    {
                //        Response.Cookies["UserName"].Expires = DateTime.Now;
                //        Response.Cookies["Password"].Expires = DateTime.Now;
                //        tbxLogin.Text = "";
                //        tbxPassword.Text = "";
                //    }
                //}

                TestUser user_Details = objMyDBClass.Validate_User(Convert.ToString(tbxEmail.Text), Convert.ToString(tbxPassword.Text));

                if (user_Details == null)
                {
                    showMessage("The email or password you entered is incorrect. Please try again (make sure your caps lock is off).");
                    return;
                }

                Response.Cookies["email"].Value = tbxEmail.Text.Trim();
                Response.Cookies["Password"].Value = tbxPassword.Text.Trim();

                if (user_Details != null)
                {
                    if ((user_Details.UserId != null) && (user_Details.UserId != ""))
                    {
                        Session["LoginId"] = user_Details.UserId;
                        Session["UserDetail"] = user_Details;
                        Session["Email"] = tbxEmail.Text.Trim();

                        //Moving xsl files from C to UserDirectory if not exists 
                        try
                        {
                            int requireUpdation = objMyDBClass.CheckOperationalFiles_Updation();

                            int insertedRow = 0;

                            string userDir = Common.GetDirectoryPath() + "User Files/" + tbxEmail.Text.Trim() + "/XSL";

                            if (!Directory.Exists(userDir))
                            {
                                insertedRow = objMyDBClass.InsertOperationalFiles(user_Details.UserId);
                            }

                            string orignalDir = Common.GetXSLSourcePath();

                            if (insertedRow > 0)
                            {
                                if (!Directory.Exists(userDir))
                                    Directory.CreateDirectory(userDir);

                                DirectoryInfo dInfo = CopyTo(new DirectoryInfo(orignalDir), userDir, true);
                            }
                            else if (requireUpdation > 0)
                            {
                                if (Directory.Exists(userDir))
                                    Directory.Delete(userDir, true);

                                Directory.CreateDirectory(userDir);

                                DirectoryInfo dInfo = CopyTo(new DirectoryInfo(orignalDir), userDir, true);
                            }
                        }
                        catch (Exception)
                        {
                            showMessage("Some error occurs while moving operational files to user directory.");
                            return;
                        }
                        //end moving files

                        if ((user_Details.UserType.ToLower().Trim() == "1") || (user_Details.UserType.ToLower().Trim() == "2"))
                        {
                            Response.Redirect("OnlineTestUser.aspx");
                        }
                        else if (user_Details.UserType.ToLower().Trim() == "7")
                        {
                            Response.Redirect("OnlineTestAdmin.aspx?UserId=" + user_Details.UserId);
                        }

                        //Admin user create tagging/untagging tasks
                        else if (user_Details.UserType.ToLower().Trim() == "5")
                        {
                            string id = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxEmail.Text.Trim()));
                            string pass = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxPassword.Text.Trim()));
                            string type = HttpUtility.UrlEncode(CommonClass.Encrypt(user_Details.UserType.Trim()));

                            Response.Redirect(string.Format("AdminPanel.aspx?id={1}&p={2}&t={3}", Request.Url.Host, id, pass, type), true);
                        }

                        //teamlead user perform mapping
                        else if (user_Details.UserType.ToLower().Trim() == "6")
                        {
                            string id = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxEmail.Text.Trim()));
                            string pass = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxPassword.Text.Trim()));
                            string type = HttpUtility.UrlEncode(CommonClass.Encrypt(user_Details.UserType.Trim()));

                            Response.Redirect(string.Format("AdminPanel.aspx?id={1}&p={2}&t={3}", Request.Url.Host, id, pass, type), true);
                        }
                    }
                }
                else
                {
                    Response.Redirect("BookMicro.aspx");
                }
            }
        }
        public void GetComparisonTestResult(double percentage)
        {
            //divClickHere.Visible = false;
            lbl.Attributes.Add("data-percent", Convert.ToString(percentage));
        }

        private void sendEmail(string obtained, string email, string uid)
        {
            try
            {
                string From = "bookmicro123@gmail.com";
                string Password = "@sad@123";
                string To = email;
                //string uid =Convert.ToString(Session["LoginId"]);
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(From);
                mailMessage.To.Add(To);
                //mailMessage.Bcc.Add("asad@readhowyouwant.com");
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "Congratulations!";
               
                string applicationPath = Convert.ToString(ConfigurationManager.AppSettings["MainServerPath"]);
                //mailMessage.Body = "Congratulations! You have passed the test and now assigned a role of \"Trainee Editor at BookMicro\".<br/> <br/> " +
                //                    "Please click <a href='" + applicationPath + "/UserDetails.aspx?uid=" + uid + "&email=" + email + "'>here</a> to complete your profile. <br/> <br/>" +
                //                    "Note: Tests given after training are paid, if you qualify the test in first two attempts. On qualification of any training level, amount of passed test will be added to your account. To withdraw the amount, you need to successfully complete atleast 5 tasks in the same category.<br/><br/><br/>" +
                //                    "Welcome to BookMicro Community and wish you best of Luck!<br/><br/>BookMicro Team will contact you soon <br/><br/>BookMicro Hiring Team";

                mailMessage.Body = "Congratulations! You have passed the test.<br/> <br/> " +
                                 "Please click <a href='" + applicationPath + "/UserDetails.aspx?uid=" + uid + "&email=" + email + "'>here</a> to complete your profile. <br/> <br/>" +
                                 "You can get tasks after login using your credentials. In case of any issue please contact <br/>" +
                                 "skype: bookmicro.support.<br/><br/><br/>" +
                                 "Welcome to BookMicro!";

                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(From, Password);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
                //lblErrorMsg.Text = ex.Message;
                //lblErrorMsg.Visible = true;
                //divCanvas.Visible = false;
                showMessage(ex.Message);
            }
        }

        private void showMessage(string message)
        {
            if (message != "")
            {
                DivError.Visible = true;
                divText.InnerText = message;
            }
            else
            {
                DivError.Visible = false;
            }
        }

        protected void lbtnCompleteProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserDetails.aspx", true);
        }

        protected void lbtnGoToProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("OnlineTestUser.aspx", true);
        }
    }
}
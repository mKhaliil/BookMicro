using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Drawing;
using Outsourcing_System.MasterPages;
using Outsourcing_System.PdfCompare_Classes;

namespace Outsourcing_System
{
    public partial class UserDetails : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //if ((Session["OnlineTestUser"] == null) && (Session["email"] == null))
                //{
                //    Response.Redirect("Bookmicro.aspx", true);
                //}

                //if (Convert.ToString(Session["LoginId"]) == "") Response.Redirect("Bookmicro.aspx", true);

                //GetUserEditProfile();

                //string userId = Convert.ToString(Request.QueryString["uid"]);

                //User enter profile details for the first time
                if (!string.IsNullOrEmpty(Convert.ToString(Session["OnlineTestUser"])) && !string.IsNullOrEmpty(Convert.ToString(Session["email"])))
                {
                    tbxFullName.Text = Convert.ToString(Session["OnlineTestUser"]);
                    tbxEmail.Text = Convert.ToString(Session["email"]);
                    tbxProfileName.Text = "";
                    //tbxPassword.Text = "1234";
                    //tbxConfirmPassword.Text = "1234";
                }

                if (!string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
                {
                    if (objMyDBClass.GetUserIsActiveStatus(Convert.ToString(Session["LoginId"])))
                        GetUserEditProfile();
                }
            }
        }

        public void GetUserEditProfile()
        {
            var user = objMyDBClass.GetUserEdit_Profile(Convert.ToString(Session["LoginId"]));

            if (user != null)
            {
                if (!string.IsNullOrEmpty(user.FullName) && !string.IsNullOrEmpty(user.Email))
                {
                    tbxProfileName.Text = user.ProfileName;
                    tbxPassword.Attributes.Add("value", user.Password);
                    tbxAccountNumber.Text = user.AccountNum;
                    tbxMobileNumber.Text = user.MobileNumber;
                    ddlEducation.SelectedValue = user.Education == "" ? "Please Select" : user.Education;
                    ddlGender.SelectedValue = user.Gender == null ? "Please Select" : user.Gender;
                    tbxExperience.Text = user.Experience;
                    tbxDescription.Text = user.Description;
                    tbxIdCardNumber.Text = user.IdCardNum;
                    tbxFullName.Text = user.FullName;
                    tbxEmail.Text = user.Email;
                    tbxIdCardNumber.Text = user.CNICNO;
                    btnSave.Text = "Update";
                    tbxCountryOfResidence.Text = user.Country;
                    tbxPaypalNumber.Text = user.PaypalNum;

                    tbxAccountTitle.Text = user.AccountTitle;
                    tbxAccountType.Text = user.AccountType;
                    tbxBankName.Text = user.BankName;
                    tbxBranch.Text = user.Branch;
                    tbxBankCode.Text = user.BankCode;
                    tbxCity.Text = user.City;
                    tbxCountry.Text = user.Country;

                    if (btnSave.Text == "Update")
                    {
                        trPassword.Visible = false;
                        trConfirmPasword.Visible = false;
                    }
                }
            }
            else btnSave.Text = "Save";
        }

        public void SaveUserProfile()
        {
            string email = "";
            string imgPath = "";
            email = tbxEmail.Text;
            if (fuImage.HasFile)
            {
                try
                {
                    string savePath = null;

                    savePath = Server.MapPath("~/UserImages/") + fuImage.FileName;
                    imgPath = "~/UserImages/" + fuImage.FileName;
                    fuImage.SaveAs(savePath);
                }
                catch (Exception ex)
                {
                    (Master as UserMaster).ShowMessage(MessageTypes.Error,
                        "Sorry! Some error has occured while uploading image.");
                }
            }

            TestUser user = new TestUser
            {
                ProfileName = tbxProfileName.Text.Trim(),
                Email = Convert.ToString(email),
                Password = tbxPassword.Text.Trim(),
                FullName = Convert.ToString(Session["OnlineTestUser"]),
                Gender = Convert.ToString(ddlGender.SelectedValue),
                IdCardNum = tbxIdCardNumber.Text.Trim(),
                MobileNumber = tbxMobileNumber.Text.Trim(),
                Education = Convert.ToString(ddlEducation.SelectedValue),
                Experience = Convert.ToString(tbxExperience.Text),
                Description = Convert.ToString(tbxDescription.Text),
                ImagPath = Convert.ToString(imgPath),

                BankName = Convert.ToString(tbxBankName.Text),
                AccountNum = Convert.ToString(tbxAccountNumber.Text),
                AccountTitle = Convert.ToString(tbxAccountTitle.Text),
                AccountType = Convert.ToString(tbxAccountType.Text),
                Branch = Convert.ToString(tbxBranch.Text),
                BankCode = Convert.ToString(tbxBankCode.Text),
                City = Convert.ToString(tbxCity.Text),
                Country = Convert.ToString(tbxCountry.Text),
                CountryOfResidence = Convert.ToString(tbxCountryOfResidence.Text),
                PaypalNum = Convert.ToString(tbxPaypalNumber.Text)
            };

            string value = objMyDBClass.SavePassedUser(user);

            if (Convert.ToInt32(value) == -100)
            {
                (Master as UserMaster).ShowMessage(MessageTypes.Error, "Your details can't be saved beacause you have not passed the test.");
            }

            else if (Convert.ToInt32(value) == -101)
            {
                (Master as UserMaster).ShowMessage(MessageTypes.Error, "Sorry! Some error has occured while saving user details.");
            }

            if (Convert.ToInt32(value) >= 0)
            {
                string userId = objMyDBClass.GetUserIdByEmail(email);
                if (!string.IsNullOrEmpty(userId))
                {
                    objMyDBClass.SetComparisonRights(Convert.ToInt32(userId), "ErrorDetection", "passed", "ErrorDetection");
                    objMyDBClass.SetUserIsActiveStatus(userId);
                }
               
                //ClearFields();

                Session.Clear();

                //ucShowMessage1.ShowMessage(MessageTypes.Success,
                //    "Your Profile is submitted successfully. " +
                //    "<br/><br/> In case of any issue, contact us on our facebook page.<br/><br/>" +
                //    "Facebook Page link is https://www.facebook.com/pages/BookMicro/117027968480309");
                //Session.Clear();

                Session["LoginId"] = userId;
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "showProfileCreatedDialog();", true);

                int passwordLength = tbxPassword.Text.Trim().Length;

                if (passwordLength > 1)
                {
                    StringBuilder shape = new StringBuilder(tbxPassword.Text.Trim().Length - 1);

                    for (int i = 0; i < passwordLength - 2; i++)
                    {
                        shape.Append("*");
                    }

                    string passwordFirstCharacter = tbxPassword.Text.Trim()[0].ToString();
                    string encodedPassword = passwordFirstCharacter + Convert.ToString(shape);
                    sendEmail(email, encodedPassword);
                }
            }
        }

        private void sendEmail(string email, string password)
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
                mailMessage.Subject = "Welcome to BookMicro!";

                mailMessage.Body = "Welcome to BookMicro!<br/> <br/> " +
                                   "Your profile is created. You can login with the following credentials:<br/> <br/>" +
                                   "Email: " + email + "Password: " + password + "<br/> <br/>" +
                                   "After login, you can assign task from open tasks in your panel. In case of any issues please contact <br/>" +
                                   "skype: bookmicro.support";
                                 
                SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);

                smtpClient.UseDefaultCredentials = false;
                smtpClient.Credentials = new NetworkCredential(From, Password);
                smtpClient.EnableSsl = true;
                smtpClient.Send(mailMessage);
            }
            catch (Exception ex)
            {
               
            }
        }

        public void UpdateUserProfile()
        {
            string imgPath = null;
            string savePath = null;

            HttpPostedFile file = fuImage.PostedFile;

            if (fuImage.HasFile)
            {
                try
                {
                    savePath = Server.MapPath("~/UserImages/") + fuImage.FileName;
                    imgPath = "~/UserImages/" + fuImage.FileName;
                    fuImage.SaveAs(savePath);
                }
                catch (Exception ex)
                {
                    ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured.");
                }
            }

            int effectedRows = objMyDBClass.UpdateUserProfile(Convert.ToString(Session["LoginId"]), Convert.ToString(tbxProfileName.Text),
                                Convert.ToString(tbxFullName.Text), Convert.ToString(tbxAccountNumber.Text), Convert.ToString(tbxIdCardNumber.Text),
                                Convert.ToString(tbxMobileNumber.Text), Convert.ToString(ddlEducation.SelectedValue),
                                Convert.ToString(tbxExperience.Text), Convert.ToString(tbxDescription.Text), imgPath,
                                Convert.ToString(ddlGender.SelectedValue), Convert.ToString(tbxPaypalNumber.Text), Convert.ToString(tbxCountryOfResidence.Text),
                                Convert.ToString(tbxBankName.Text), Convert.ToString(tbxAccountNumber.Text), Convert.ToString(tbxAccountTitle.Text),
                                Convert.ToString(tbxAccountType.Text), Convert.ToString(tbxBranch.Text), Convert.ToString(tbxBankCode.Text),
                                Convert.ToString(tbxCity.Text), Convert.ToString(tbxCountry.Text));

            if (effectedRows >= 0) ucShowMessage1.ShowMessage(MessageTypes.Success, "Your profile is updated successfully.");
            else ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured while updating profile");
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

                TestUser user_Details = objMyDBClass.Validate_User(Convert.ToString(tbxPopupEmail.Text), Convert.ToString(tbxPopupPassword.Text));

                if (user_Details == null)
                {
                    ucShowMessage1.ShowMessage(MessageTypes.Success,
                        "The email or password you entered is incorrect. Please try again (make sure your caps lock is off).");
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
                            ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured.");
                            //showMessage("Some error occurs while moving operational files to user directory.");
                            return;
                        }
                        //end moving files

                        if ((user_Details.UserType.ToLower().Trim() == "1") || (user_Details.UserType.ToLower().Trim() == "2"))
                        {
                            Response.Redirect("OnlineTestUser.aspx", true);
                        }
                        else if (user_Details.UserType.ToLower().Trim() == "7")
                        {
                            Response.Redirect("OnlineTestAdmin.aspx", true);
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

        public void ClearFields()
        {
            tbxProfileName.Text = "";
            tbxPassword.Text = "";
            tbxFullName.Text = "";
            tbxIdCardNumber.Text = "";
            tbxEmail.Text = "";
            tbxMobileNumber.Text = "";
            ddlEducation.SelectedIndex = 0;
            ddlGender.SelectedIndex = 0;
            tbxAccountNumber.Text = "";
            tbxExperience.Text = "";
            tbxDescription.Text = "";
            tbxCountryOfResidence.Text = "";
            tbxPaypalNumber.Text = "";
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(tbxFullName.Text) || String.IsNullOrEmpty(tbxEmail.Text))
            {
                ucShowMessage1.ShowMessage(MessageTypes.Error, "You session has been expired.");
                return;
            }

            if (btnSave.Text.Trim().Equals("Save")) SaveUserProfile();
            else UpdateUserProfile();
        }

        protected void lbtnAccountDetails_Click(object sender, EventArgs e)
        {
            //divAccountInformation.Visible = true;
        }

        protected void btnAddBankDetails_Click(object sender, EventArgs e)
        {
            if (tbxIdCardNumber.Text != "")
            {
                string value = objMyDBClass.Save_BankDetails(Convert.ToString(tbxAccountTitle.Text), Convert.ToString(tbxAccountType.Text), Convert.ToString(tbxBankName.Text), Convert.ToString(tbxBranch.Text), Convert.ToString(tbxBankCode.Text), Convert.ToString(tbxCity.Text), Convert.ToString(tbxCountry.Text));

                if (Convert.ToInt32(value) == -101)
                {
                    ucShowMessage1.ShowMessage(MessageTypes.Error,
                         "You have already saved your details please login into your account and then edit profile from account.");
                }
                else if (Convert.ToInt32(value) == -100)
                {
                    ucShowMessage1.ShowMessage(MessageTypes.Error,
                         "Your details can't be saved beacause you have not passed the test.");
                }

                if (Convert.ToInt32(value) >= 0)
                {
                    ClearFields();
                    ucShowMessage1.ShowMessage(MessageTypes.Success,
                        "Congratulations! Your Profile is successfully submitted. BookMicro Team will verify your profile and then send you" +
                        "your login and password.<br/><br/> In case of any issue, contact us on our facebook page.<br/><br/>" +
                        "Facebook Page link is https://www.facebook.com/pages/BookMicro/117027968480309");
                    Session.Clear();
                }
            }
        }
    }
}
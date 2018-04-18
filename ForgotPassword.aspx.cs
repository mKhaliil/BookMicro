using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outsourcing_System.MasterPages;
using Outsourcing_System.PdfCompare_Classes;
using System.Text.RegularExpressions;

namespace Outsourcing_System
{
    public partial class ForgotPassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnSendPassword_Click(object sender, EventArgs e)
        {
            if (tbxEmail.Text.Trim() != "")
            {
                MyDBClass obj = new MyDBClass();
                string userId = obj.GetUserIdByEmail(tbxEmail.Text.Trim());

                if (!string.IsNullOrEmpty(userId))
                {
                    string newPassword = GeneratePassword();
                    int result = ResetPassword(userId, tbxEmail.Text.Trim(), newPassword);
                    if (result > 0)
                    {
                        bool emailStatus = sendEmail(userId, tbxEmail.Text.Trim(), newPassword);
                        if (emailStatus) ucShowMessage1.ShowMessage(MessageTypes.Success, "New password is successfully sent to your email.");
                        else ShowErrorMessage();
                    }
                    else ShowErrorMessage();
                }
                else ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Email does't exists in bookmicro.");
            }
            else ShowErrorMessage();
        }

        private void ShowErrorMessage()
        {
            ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error occurs while sending email.");
        }

        public int ResetPassword(string uid, string email, string newPassword)
        {
            MyDBClass obj = new MyDBClass();
            int effectedRows = obj.ResetPassword(uid, email, newPassword);
            return effectedRows;
        }

        public string GeneratePassword()
        {
            int passwordLength = 6;
            int nonAlphaNumericCharacters = 1;

            string newPassword = Membership.GeneratePassword(passwordLength, nonAlphaNumericCharacters); 
            Random rnd = new Random(); 
            newPassword = Regex.Replace(newPassword, @"[^a-zA-Z0-9]", m => rnd.Next(0, 10).ToString());

            return newPassword;
        }

        private bool sendEmail(string uid, string email, string newPassword)
        {
            try
            {
                string From = "bookmicro123@gmail.com";
                string Password = "@sad@123";
                string To = email;
                MailMessage mailMessage = new MailMessage();
                mailMessage.From = new MailAddress(From);
                mailMessage.To.Add(To);
                mailMessage.IsBodyHtml = true;
                mailMessage.Subject = "Password Reset";

                string applicationPath = Convert.ToString(ConfigurationManager.AppSettings["MainServerPath"]);
                mailMessage.Body = "Your password has been reset.Your new password is " + newPassword + "<br/> <br/> " +
                                    "Please click <a href='" + applicationPath + "/OnlineTestUser.aspx?UserId=" + uid + "'>here</a> to login into your profile with new password. <br/> <br/>" +
                                    "BookMicro Hiring Team";

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
            }
        }
    }
}
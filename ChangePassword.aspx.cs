using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Outsourcing_System.MasterPages;
using Outsourcing_System.PdfCompare_Classes;

namespace Outsourcing_System
{
    public partial class ChangePassword : System.Web.UI.Page
    {
        MyDBClass obj = new MyDBClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                //Request for password change came from email without logging in
                if (Request.QueryString["uid"] != null && Request.QueryString["key"] != null)
                {
                    string key = GetResetPasswordKey(Convert.ToString(Request.QueryString["uid"]));
                    if (!string.IsNullOrEmpty(key) && key.Equals(Convert.ToString(Request.QueryString["key"])))
                    {
                        ClearResetPasswordKey(Convert.ToString(Request.QueryString["uid"]), Convert.ToString(Request.QueryString["key"]));
                        Session["LoginId"] = Convert.ToString(Request.QueryString["uid"]);
                    }
                }
                //Normal password change
                else if (Convert.ToString(Session["ResetPasswordKey"]) != "")
                {
                    string key = Convert.ToString(Session["ResetPasswordKey"]);
                    ClearResetPasswordKey(Convert.ToString(Session["LoginId"]), key);
                }

                if (!string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
                {
                    var user = GetUserDetails(Convert.ToString(Session["LoginId"]));
                    if (user != null) ShowLoginDetails(user.Email, user.Password);
                }
            }
        }

        public void ShowLoginDetails(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return;

            tbxEmail.Text = email;
            //tbxOldPassword.Text = password;
            tbxOldPassword.Attributes.Add("value", password);
        }

        public TestUser GetUserDetails(string userId)
        {
            return obj.GetUserLoginDetails(userId);
        }

        public string GetResetPasswordKey(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return "";
            return obj.GetResetPasswordKey(userId);
        }

        public void ClearResetPasswordKey(string userId, string key)
        {
            if (string.IsNullOrEmpty(userId) && string.IsNullOrEmpty(key)) return;

            int status = Convert.ToInt32(obj.ClearResetPasswordKey(userId, key));
            if (status > 0) Session["ResetPasswordKey"] = "";
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxEmail.Text) && !string.IsNullOrEmpty(tbxOldPassword.Text) &&
                !string.IsNullOrEmpty(tbxNewPassword.Text) && !string.IsNullOrEmpty(tbxConNewPassword.Text))
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
                {
                    int effectedRow = obj.ChangePassword(tbxEmail.Text.Trim(), Convert.ToString(Session["LoginId"]),
                        Convert.ToString(tbxOldPassword.Text.Trim()), Convert.ToString(tbxNewPassword.Text.Trim()));

                    if (effectedRow > 0) ucShowMessage1.ShowMessage(MessageTypes.Success, "Password changed successfully.");
                    else ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! There is some error password can't changed.");
                }
            }
        }
    }
}
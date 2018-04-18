using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Outsourcing_System.UserControls
{
    public partial class ucOnlineTestTasks : System.Web.UI.UserControl
    {
        MyDBClass objMyDBClass = new MyDBClass();

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public int SetEditView
        {
            set
            {
                //mvUserProfile.ActiveViewIndex = value;
            }
        }

        public bool EditProfile
        {
            set { divEditProfile.Visible = value; }
        }
        public bool NormalProfile
        {
            set { divNormalProfile.Visible = value; }
        }

        public bool ChangePassword
        {
            set { divChangePassword.Visible = value; }
        }

        public bool EditBankDetails
        {
            set { divEditBankDetails.Visible = value; }
        }

        public void btnChangePaswordbtnEdit_Click(object sender, EventArgs e)
        {
            //mvUserProfile.ActiveViewIndex = 0;

            EditProfile = false;
            NormalProfile = true;

            string imgPath = null;
            string savePath = null;

            HttpPostedFile file = fuProfileImage.PostedFile;

            if (fuProfileImage.HasFile)
            {
                try
                {
                    savePath = Server.MapPath("~/UserImages/") + fuProfileImage.FileName;
                    imgPath = "~/UserImages/" + fuProfileImage.FileName;
                    fuProfileImage.SaveAs(savePath);
                }
                catch (Exception ex)
                {

                }
            }

            //var user = objMyDBClass.UpdateUserProfile(Convert.ToString(Session["LoginId"]), Convert.ToString(tbxProfileName.Text), Convert.ToString(tbxFullName.Text), Convert.ToString(tbxAccountNumber.Text), Convert.ToString(tbxMobileNumber.Text), Convert.ToString(ddlEducation.SelectedValue), Convert.ToString(tbxExperience.Text), Convert.ToString(tbxDescription.Text), imgPath);
            //GetUserProfile();
        }

        public void btnClose_Click(object sender, EventArgs e)
        {
            //mvUserProfile.ActiveViewIndex = 0;
            EditProfile = false;
            NormalProfile = true;
            ChangePassword = false;
        }
        public void btnChangePasword_Click(object sender, EventArgs e)
        {
            //mvUserProfile.ActiveViewIndex = 0;
            EditProfile = false;
            ChangePassword = false;
            NormalProfile = true;
        }
        public string UserName { get; set; }
        public void GetUserProfile()
        {
            List<TestUser> list = objMyDBClass.GetUserProfile(Convert.ToString(Session["LoginId"]));

            int imgTask_Test = 0;
            int tableTask_Test = 0;
            int indexTask_Test = 0;
            int mappingTask_Test = 0;

            if ((list != null) && (list.Count > 0))
            {
                lblUserName.Text = list[0].FullName;
                lblDescription.Text = list[0].Description;
                imgUserProfile.ImageUrl = list[0].Picture;

                foreach (var item in list)
                {
                    if (item.TestType.Trim().ToLower().Equals("tables"))
                    {
                        tableTask_Test++;
                    }
                    else if (item.TestType.Trim().ToLower().Equals("images"))
                    {
                        imgTask_Test++;
                    }
                    else if (item.TestType.Trim().ToLower().Equals("index"))
                    {
                        indexTask_Test++;
                    }
                    else if (item.TestType.Trim().ToLower().Equals("comparison"))
                    {
                        mappingTask_Test++;
                    }
                }

                if (tableTask_Test > 0)
                {
                    lblTables.Text = "Tables";
                    divTablesTest.Visible = true;
                }

                if (imgTask_Test > 0)
                {
                    lblImages.Text = "Images";
                    divImagesTest.Visible = true;
                }

                if (indexTask_Test > 0)
                {
                    lblIndex.Text = "Index";
                    divIndexTest.Visible = true;
                }
                if (mappingTask_Test > 0)
                {
                    lblMapping.Text = "Mapping";
                    divMappingTest.Visible = true;
                }
            }
            UserName = lblUserName.Text;
        }

        public void GetUserEditProfile()
        {
            var user = objMyDBClass.GetUserEdit_Profile(Convert.ToString(Session["LoginId"]));

            if (user != null)
            {
                if (user.FullName != "" && user.FullName != null)
                {
                    tbxProfileName.Text = user.ProfileName;
                    tbxAccountNumber.Text = user.AccountNum;
                    tbxMobileNumber.Text = user.MobileNumber;
                    ddlEducation.SelectedValue = user.Education == "" ? "Please Select" : user.Education;
                    tbxExperience.Text = user.Experience;
                    tbxDescription.Text = user.Description;
                    imgUserProfile.ImageUrl = user.Picture;

                    tbxFullName.Text = user.FullName;
                    tbxEmail.Text = user.Email;
                    tbxCnicNo.Text = user.CNICNO;
                }
            }
        }

        public void btnEdit_Click(object sender, EventArgs e)
        {
            //mvUserProfile.ActiveViewIndex = 0;

            EditProfile = false;
            NormalProfile = true;

            string imgPath = null;
            string savePath = null;

            HttpPostedFile file = fuProfileImage.PostedFile;

            if (fuProfileImage.HasFile)
            {
                try
                {
                    savePath = Server.MapPath("~/UserImages/") + fuProfileImage.FileName;
                    imgPath = "~/UserImages/" + fuProfileImage.FileName;
                    fuProfileImage.SaveAs(savePath);
                }
                catch (Exception ex)
                {

                }
            }

            //var user = objMyDBClass.UpdateUserProfile(Convert.ToString(Session["LoginId"]), Convert.ToString(tbxProfileName.Text), Convert.ToString(tbxFullName.Text), Convert.ToString(tbxAccountNumber.Text), Convert.ToString(tbxMobileNumber.Text), Convert.ToString(ddlEducation.SelectedValue), Convert.ToString(tbxExperience.Text), Convert.ToString(tbxDescription.Text), imgPath);
            //GetUserProfile();
        }

        protected void lbtnEditBankDetails_Click(object sender, EventArgs e)
        {
            //divAccountInformation.Visible = true;

            BankDetails bankInfo = objMyDBClass.GetUserEdit_BankDetails(Convert.ToString(Session["LoginId"]));

            if (bankInfo != null)
            {
                tbxAccountTitle.Text = bankInfo.AccountTitle;
                tbxAccountType.Text = bankInfo.AccountType;
                tbxBankName.Text = bankInfo.Bank;
                tbxBranch.Text = bankInfo.Branch;
                tbxBankCode.Text = bankInfo.BankCode;
                tbxCity.Text = bankInfo.City;
                tbxCountry.Text = bankInfo.Country;
            }

            if ((tbxAccountTitle.Text.Trim() == "") && (tbxAccountType.Text.Trim() == "") && (tbxBankName.Text.Trim() == "") &&
               (tbxBranch.Text.Trim() == "") && (tbxBankCode.Text.Trim() == "") && (tbxCity.Text.Trim() == "") && (tbxCountry.Text.Trim() == ""))
                btnEditBankDetails.Text = "Add";

            //else
            //    btnEditBankDetails.Text = "Edit";
        }

        protected void btnEditBankDetails_Click(object sender, EventArgs e)
        {
            divNormalProfile.Visible = true;

            string value = objMyDBClass.Edit_BankDetails(Convert.ToString(Session["LoginId"]), Convert.ToString(tbxAccountNumber.Text), Convert.ToString(tbxAccountTitle.Text),
                               Convert.ToString(tbxAccountType.Text), Convert.ToString(tbxBankName.Text), Convert.ToString(tbxBranch.Text),
                               Convert.ToString(tbxBankCode.Text), Convert.ToString(tbxCity.Text), Convert.ToString(tbxCountry.Text));
        }

        protected void btnChangePassword_Click(object sender, EventArgs e)
        {
            //divNormalProfile.Visible = true;

            //string value = objMyDBClass.ChangePassword(Convert.ToString(Session["LoginId"]), Convert.ToString(tbxOldPassword.Text), 
            //                                           Convert.ToString(tbxNewPassword.Text));
            //divChangePassword.Visible = false;
        } 
        
        protected void btnCancelBankDetails_Click(object sender, EventArgs e)
        {
            divNormalProfile.Visible = true;
        }
        protected void imgSetting_Click(object sender, EventArgs e)
        {

        }
    }
}
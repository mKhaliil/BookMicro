using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Drawing;
using System.Web.Services;
using System.Text;
using EO.Web.Internal;
using Microsoft.Ajax.Utilities;

namespace Outsourcing_System
{
    public partial class ManageUser : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["LoginId"] == null)
            {
                Response.Redirect("Bookmicro.aspx", true);
            }

            else if (!Page.IsPostBack)
            {
                BindDdUserCategorySelection();
                BindDdUserCategory();
                ProcessControl1.LoadProcesses();
            }
        }

        public void BindDdUsersName(string userCategory)
        {
            if (userCategory == "")
                return;

            int userCategoryId = 0;

            if (userCategory.Equals("Junior"))
                userCategoryId = 1;

            if (userCategory.Equals("Senior"))
                userCategoryId = 2;

            if (userCategory.Equals("Admin"))
                userCategoryId = 5;

            if (userCategory.Equals("TeamLead"))
                userCategoryId = 6;

            if (userCategory.Equals("PowerUser"))
                userCategoryId = 7;

            string queryUser = "Select * from [User] Where UType='" + userCategoryId + "' Order By UserName";
            DataSet dsUser = objMyDBClass.GetDataSet(queryUser);
            ddUser.DataSource = dsUser.Tables[0];
            ddUser.DataTextField = "UserName";
            ddUser.DataValueField = "UID";
            ddUser.DataBind();

            ListItem liSelect = new ListItem("Select Name", "-1");
            ddUser.Items.Insert(0, liSelect);
        }

        public void BindDdUserCategorySelection()
        {
            string qCategory = "Select * From UserCategory";
            DataSet dsCategory = objMyDBClass.GetDataSet(qCategory);
            ddUserCategory.DataTextField = "Category";
            ddUserCategory.DataValueField = "CID";
            ddUserCategory.DataSource = dsCategory;
            ddUserCategory.DataBind();

            ListItem liSelect = new ListItem("Select Category", "-1");
            ddUserCategory.Items.Insert(0, liSelect);
        }

        public void BindDdUserCategory()
        {
            string qCategory = "Select * From UserCategory";
            DataSet dsCategory = objMyDBClass.GetDataSet(qCategory);
            ddCategory.DataTextField = "Category";
            ddCategory.DataValueField = "CID";
            ddCategory.DataSource = dsCategory;
            ddCategory.DataBind();
        }

        protected void btnUpdateUser_Click(object sender, EventArgs e)
        {
            if (txtFullName.Text == "" || txtPassword.Text == "" || txtEmail.Text == "")
            {
                ((AdminMaster)this.Master).ShowMessageBox("Please Fill Out All the Fields", "error");
                return;
                //Response.Write("<script type=\"text/javascript\">alert('Please Fill Out All the Fields');</script>");
            }
            else if (txtPassword.Text != txtConfirmPassword.Text)
            {
                ((AdminMaster)this.Master).ShowMessageBox("Password and Confirm Password mismatched", "error");
                return;
                //Response.Write("<script type=\"text/javascript\">alert('Password and Confirm Password mismatched');</script>");
            }
            else
            {

                int userCategoryId = 0;
                string userCategory = Convert.ToString(ddCategory.SelectedItem);

                if (userCategory.Equals("Junior"))
                    userCategoryId = 1;

                if (userCategory.Equals("Senior"))
                    userCategoryId = 2;

                if (userCategory.Equals("Admin"))
                    userCategoryId = 5;

                if (userCategory.Equals("TeamLead"))
                    userCategoryId = 6;

                if (userCategory.Equals("PowerUser"))
                    userCategoryId = 7;

                string query = "Update [User] set utype=" + userCategoryId + ", UName='" + this.txtFullName.Text + "',Password='" + this.txtConfirmPassword.Text + "',Email='" + this.txtEmail.Text + "',IsActive='" + this.ddUserStatus.SelectedItem.Value + "' Where UID=" + this.ddUser.SelectedItem.Value;
                int result = objMyDBClass.ExecuteCommand(query);
                if (result > 0)
                {
                    string queryDel = "Delete from UserCanPerform Where UID=" + this.ddUser.SelectedItem.Value;
                    result = objMyDBClass.ExecuteCommand(queryDel);

                    string processIDs = ProcessControl1.getSelectedIValues();

                    if (processIDs != "")
                    {
                        foreach (string val in processIDs.Split(new char[] {':'}))
                        {
                            string queryUserCanPerform = "Insert into [UserCanPerform](PID,UID) Values(" + val + "," +
                                                         this.ddUser.SelectedItem.Value + ")";
                            result = objMyDBClass.ExecuteCommand(queryUserCanPerform);
                        }
                    }

                    ((AdminMaster)this.Master).ShowMessageBox("User Record Updated Successfully", "Succ");
                    //Response.Write("<script type=\"text/javascript\">alert('User Record Updated Succssfully');</script>");
                }
            }
        }

        protected void btnDeleteUser_Click(object sender, EventArgs e)
        {
            if (txtFullName.Text == "" || txtPassword.Text == "" || txtEmail.Text == "")
            {
                ((AdminMaster)this.Master).ShowMessageBox("Please Fill Out All the Fields", "error");
            }
            else if (txtPassword.Text != txtConfirmPassword.Text)
            {
                ((AdminMaster)this.Master).ShowMessageBox("Password and Confirm Password mismatched", "error");
            }
            else
            {
                string query = " delete from [User] Where UID=" + this.ddUser.SelectedItem.Value;
                int result = objMyDBClass.ExecuteCommand(query);
                if (result > 0)
                {
                    string queryDel = "Delete from UserCanPerform Where UID=" + this.ddUser.SelectedItem.Value;
                    result = objMyDBClass.ExecuteCommand(queryDel);

                    ((AdminMaster)this.Master).ShowMessageBox("User Record deleted Successfully", "Succ");
                }
            }
        }

        protected void btnAdminPanel_Click(object sender, EventArgs e)
        {
            Server.Transfer("AdminPanel.aspx");
        }

        protected void ddUserCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddUserCategory.SelectedValue == "-1")
            {
                ddUser.SelectedIndex = 0;
                ddUser.Enabled = false;
                ResetUserFields();
            }
            else
            {
                ddUser.Enabled = true;

                BindDdUsersName(Convert.ToString(ddUserCategory.SelectedItem));
                //GetUserDetails_ByUserId(Convert.ToInt32(ddUser.SelectedItem.Value));
            }
        }

        protected void ddUser_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddUser.SelectedValue == "-1")
            {
                ResetUserFields();
            }
            else
            {
                GetUserDetails_ByUserId(Convert.ToInt32(ddUser.SelectedItem.Value));
            }
        }

        protected void btnGetNewUser_Click(object sender, EventArgs e)
        {
            //pnlReportsButton.Visible = false;
            BindLeftListBox();
        }

        protected void btnGetOldUser_Click(object sender, EventArgs e)
        {
            //pnlReportsButton.Visible = false;
            //BindRightListBox();
        }

        protected void btnMappEmail_Click(object sender, EventArgs e)
        {

        }

        protected void btnCreateAccount_Click(object sender, EventArgs e)
        {
            string leftSelectedItems = Request.Form[lbxLeft.UniqueID];

            if (leftSelectedItems == null)
                return;

            var temp = leftSelectedItems.Split(',');

            temp = temp.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

            StringBuilder sb = null;

            if (temp.Length > 0)
            {
                sb = new StringBuilder();

                foreach (var email in temp)
                {
                    sb.Append("'" + email + "',");
                }
            }

            string emails = sb.ToString().Remove(sb.Length - 1, 1);

            List<WokmeterUser> userDetails = getUserDetails_ByEmail(emails);

            MyDBClass obj = new MyDBClass();
            int userCount = 0;
            int result = 0;

            if ((userDetails != null) && (userDetails.Count > 0))
            {
                foreach (var user in userDetails)
                {
                    result = obj.InsertNewBookMicroUser(user.Login, user.Password, "13", user.UserName, user.Email);

                    if (result > 0)
                        userCount++;
                }

                lbxLeft.Items.Clear();

                if (userCount == 1)
                    ((AdminMaster)this.Master).ShowMessageBox("Account of " + userCount + " user is successfully created in workmeter.", "Succ");

                else if (userCount > 1)
                    ((AdminMaster)this.Master).ShowMessageBox("Account of " + userCount + " users are successfully created in workmeter.", "Succ");

                else
                    ((AdminMaster)this.Master).ShowMessageBox("Account of " + userDetails.Count + " users can't be created due to some error.", "error");
            }
        }

        protected void BindLeftListBox()
        {
            List<string> list_BookMicro = getBookMicroUsers_List();
            list_BookMicro = list_BookMicro.Where(x => (!string.IsNullOrEmpty(x))).ToList();

            List<string> list_WorkMeter = getWorkMeterUsers_List();
            list_WorkMeter = list_WorkMeter.Where(x => (!string.IsNullOrEmpty(x))).ToList();

            //List<string> ThirdList = list_BookMicro.Except(list_WorkMeter).ToList();

            //lbxLeft.DataSource = ThirdList;
            //lbxLeft.DataBind();

            List<string> ThirdList = list_BookMicro.Except(list_WorkMeter).ToList();

            StringBuilder sb = new StringBuilder();

            foreach (var email in ThirdList)
            {
                sb.Append("'" + email + "',");
            }

            string emails = sb.ToString().Remove(sb.Length - 1, 1);
            var res = getUserFullName_ByEmail(emails);
            lbxLeft.DataSource = res;
            lbxLeft.DataBind();

            //SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString());
            //con.Open();
            //SqlDataAdapter objDa = new SqlDataAdapter("select UserName,email,uid from [User] order by [User].uid", con);
            //DataSet objDs = new DataSet();
            //objDa.Fill(objDs, "[User]");

            //List<CreateAccount> userEmailList_BookMicro = new List<CreateAccount>();
            //foreach (DataRow myRow in objDs.Tables["[User]"].Rows)
            //{
            //    userEmailList_BookMicro.Add(new CreateAccount
            //    {
            //        FullName = Convert.ToString(myRow["UserName"]),
            //        Email = Convert.ToString(myRow["email"]),
            //        Id = Convert.ToString(myRow["uid"])
            //    });
            //}

            //objDa.Dispose();
            //objDs.Clear();
            //con.Close();

            //SqlConnection con_Wm = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
            //con.Open();
            //SqlDataAdapter objDa_Wm = new SqlDataAdapter("select email from [tblUserLogin]", con_Wm);
            //DataSet objDs_Wm = new DataSet();
            //objDa_Wm.Fill(objDs_Wm, "[tblUserLogin]");

            //List<CreateAccount> userEmailList_WorkMeter = new List<CreateAccount>();
            //foreach (DataRow myRow in objDs.Tables["[User]"].Rows)
            //{
            //    userEmailList_WorkMeter.Add(new CreateAccount
            //    {
            //        Email = Convert.ToString(myRow["email"])
            //    });
            //}

            //objDa_Wm.Dispose();
            //objDs_Wm.Clear();
            //con_Wm.Close();

            //foreach (var item in userEmailList_BookMicro)
            //{

            //}
            ////var ThirdList = userEmailList_BookMicro.Select(x => x.Email.ToString())
            ////               .Except(userEmailList_WorkMeter.Select(x => x.Email)).ToList();

            ////List<CreateAccount> ThirdList = userEmailList_BookMicro.Except(userEmailList_WorkMeter).ToList();

            //lbxLeft.DataSource = ThirdList;
            //lbxLeft.DataTextField = "Email";
            //lbxLeft.DataValueField = "Id";
            //lbxLeft.DataBind();
        }

        //protected void BindRightListBox()
        //{
        //    List<string> list_BookMicro = getBookMicroUsers_List();
        //    list_BookMicro = list_BookMicro.Where(x => (!string.IsNullOrEmpty(x))).ToList();

        //    List<string> list_WorkMeter = getWorkMeterUsers_List();
        //    list_WorkMeter = list_WorkMeter.Where(x => (!string.IsNullOrEmpty(x))).ToList();

        //    List<string> ThirdList = list_WorkMeter.Except(list_BookMicro).ToList();

        //    StringBuilder sb = new StringBuilder();

        //    foreach (var email in ThirdList)
        //    {
        //        sb.Append("'" + email + "',");
        //    }

        //    string emails = sb.ToString().Remove(sb.Length - 1, 1);
        //    var finalList = getUserFullName_ByEmail(emails);
        //    lbxRight.DataSource = finalList;
        //    lbxRight.DataBind();

        //    //SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString());
        //    //con.Open();
        //    //SqlDataAdapter objDa = new SqlDataAdapter("select UserName,uid from [User] order by [User].uid", con);
        //    //DataSet objDs = new DataSet();
        //    //objDa.Fill(objDs, "[User]");

        //    //lbxRight.DataSource = objDs.Tables["[User]"];
        //    //lbxRight.DataTextField = objDs.Tables["[User]"].Columns["UserName"].ColumnName.ToString();
        //    //lbxRight.DataValueField = objDs.Tables["[User]"].Columns["uid"].ColumnName.ToString();
        //    //lbxRight.DataBind();
        //    //objDa.Dispose();
        //    //objDs.Clear();
        //    //con.Close();
        //}

        private List<WokmeterUser> getUserDetails_ByEmail(string emailList)
        {
            try
            {
                List<WokmeterUser> userNamesList = null;
                string query = "select UName, Password, UserName, Email from [User] where UserName in (" + emailList + ") order by [User].uid";
                SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString());
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                using (con)
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        userNamesList = new List<WokmeterUser>();

                        while (dr.Read())
                        {
                            userNamesList.Add(new WokmeterUser
                            {
                                UserName = Convert.ToString(dr["UserName"]),
                                Password = Convert.ToString(dr["Password"]),
                                Login = Convert.ToString(dr["UName"]),
                                Email = Convert.ToString(dr["Email"])
                            });
                        }
                    }
                }

                userNamesList.TrimExcess();

                return userNamesList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<String> getUserFullName_ByEmail(string emailList)
        {
            try
            {
                List<String> userNamesList = null;
                string query = "select UserName from [User] where email in (" + emailList + ") order by [User].uid";
                SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString());
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                using (con)
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        userNamesList = new List<String>();

                        while (dr.Read())
                        {
                            userNamesList.Add(Convert.ToString(dr["UserName"]));
                        }
                    }
                }

                userNamesList.TrimExcess();

                return userNamesList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<String> getBookMicroUsers_List()
        {
            try
            {
                List<String> userEmailList = null;
                string query = "select email from [User] order by [User].uid";
                SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString());
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                using (con)
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        userEmailList = new List<String>();

                        while (dr.Read())
                        {
                            userEmailList.Add(Convert.ToString(dr["email"]));
                        }
                    }
                }

                userEmailList.TrimExcess();

                return userEmailList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private List<String> getWorkMeterUsers_List()
        {
            try
            {
                List<String> userEmailList = null;
                string query = "select email from [tblUserLogin]";
                SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
                SqlCommand cmd = new SqlCommand(query, con);
                cmd.CommandType = CommandType.Text;
                con.Open();

                using (con)
                {
                    SqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        userEmailList = new List<String>();

                        while (dr.Read())
                        {
                            userEmailList.Add(Convert.ToString(dr["email"]));
                        }
                    }
                }

                userEmailList.TrimExcess();

                return userEmailList;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void GetUserDetails_ByUserId(int userId)
        {
            ddCategory.Enabled = true;
            ddUserStatus.Enabled = true;
            string query = "Select * from [User] where UID=" + userId;
            DataSet ds = objMyDBClass.GetDataSet(query);
            DataRow dr = ds.Tables[0].Rows[0];
            this.txtFullName.Text = dr["UName"].ToString();
            this.txtPassword.Text = dr["Password"].ToString();
            this.txtConfirmPassword.Text = dr["Password"].ToString();
            this.txtEmail.Text = dr["Email"].ToString();
            this.ddUserStatus.SelectedValue = dr["isActive"].ToString();
            this.ddCategory.SelectedIndex = this.ddCategory.Items.IndexOf(this.ddCategory.Items.FindByValue(dr["UType"].ToString()));

            string queryProcess = "SELECT UserCanPerform.UID, Process.PID, Process.PName FROM UserCanPerform INNER JOIN ";
            queryProcess += "Process ON Process.PID = UserCanPerform.PID WHERE UserCanPerform.UID =" + this.ddUser.SelectedValue;
            DataSet dsProcess = objMyDBClass.GetDataSet(queryProcess);
            ProcessControl1.UncheckedSelectedBoxes();
            for (int i = 0; i < dsProcess.Tables[0].Rows.Count; i++)
            {
                ProcessControl1.setSelectedBoxes(dsProcess, dsProcess.Tables[0].Rows[i]["PName"].ToString(), "");
            }
        }

        public void ResetUserFields()
        {
            this.txtConfirmPassword.Text = "";
            this.txtPassword.Text = "";
            this.txtFullName.Text = "";
            this.txtEmail.Text = "";
            this.ddUserStatus.SelectedIndex = -1;
            ProcessControl1.UncheckedSelectedBoxes();
            ddCategory.Enabled = false;
            ddUserStatus.Enabled = false;
        }

        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Server.Transfer("Login.aspx");
        }

        [WebMethod]
        public static string GetPassedTests(string name)
        {
            WebForm5 obj = new WebForm5();
            string testTypes = obj.GetClearedTests(name);
            return testTypes;
        }

        public string GetClearedTests(string name)
        {
            StringBuilder sbTestTypes = new StringBuilder();
            List<string> testList = objMyDBClass.GetEditor_ClearedTestsList(name);

            testList = testList.Distinct().ToList();

            Session["AutoComplete_UserName"] = name;
            if ((testList != null) && (testList.Count > 0))
            {
                foreach (var test in testList)
                {
                    sbTestTypes.Append(test + ",");
                }
            }

            if (sbTestTypes.Length > 0)
                sbTestTypes.Remove(sbTestTypes.Length - 1, 1);

            return sbTestTypes.ToString();
        }

        [WebMethod]
        public static void GetTests_ByCheckBox(string passedTests)
        {
            WebForm5 obj = new WebForm5();
            obj.SaveClearedTests(passedTests);
        }

        public void SaveClearedTests(string passedTests)
        {
            string[] clearedTests = null;
            string returnVal = "";
            String userName = Convert.ToString(Session["AutoComplete_UserName"]);

            string[] TestTypes = { "Comparison", "Image", "Table", "Index" };

            if (passedTests != "")
            {
                clearedTests = passedTests.Split(',');
            }

            clearedTests = clearedTests.Where(x => (!string.IsNullOrEmpty(x))).Distinct().ToArray();

            //Get failed tests
            TestTypes = TestTypes.Where(x => (!clearedTests.Contains(x))).ToArray();

            //Clear checked passed tests
            if (clearedTests != null && clearedTests.Length > 0)
            {
                foreach (var passedTest in clearedTests)
                {

                    returnVal = objMyDBClass.ClearTests(userName, passedTest, "passed", passedTest);
                }
            }

            //Fail unChecked tests
            if (TestTypes != null && TestTypes.Length > 0)
            {
                foreach (var failedTest in TestTypes)
                {
                    returnVal = objMyDBClass.ClearTests(userName, failedTest, "failed", failedTest);
                }
            }

            if (Convert.ToInt32(returnVal) == -100)
            {
                showMessage("User Name does not exists.");
            }
            else if (Convert.ToInt32(returnVal) == -101)
            {
                showSuccessMessage("User clears the selected tests.");
            }

            else if (Convert.ToInt32(returnVal) == -102)
            {
                showSuccessMessage("User details are updated.");
            }
        }

        //Save users details in db
        protected void btnSave_Click(object sender, EventArgs e)
        {
            string returnVal = objMyDBClass.InsertOnlineTest(tbxFullName.Text, tbxEmail.Text, "1001", "", 100, "Comparison");
            if (Convert.ToInt32(returnVal) == 0)
            {
                returnVal = objMyDBClass.SaveResult(tbxFullName.Text, tbxEmail.Text, "1001", "100", "passed");
                objMyDBClass.MovePassedResult(tbxFullName.Text, tbxEmail.Text);
            }

            if (Convert.ToInt32(returnVal) > 0)
            {
                //if (tbxIdCardNumber.Text != "")
                //{
                //    string value = objMyDBClass.SavePassedUser(Convert.ToString(tbxProfileName.Text),
                //        Convert.ToString(tbxPassword.Text), Convert.ToString(tbxFullName.Text),
                //        Convert.ToString(tbxEmail.Text), Convert.ToString(tbxIdCardNumber.Text),
                //        Convert.ToString(tbxMobileNumber.Text),
                //        Convert.ToString(ddlEducation.SelectedValue), "", Convert.ToString(tbxExperience.Text), "", "", "", "", "", "", "", "", "");

                //    if (Convert.ToInt32(value) == -101)
                //    {
                //        showMessage(
                //            "You have already saved your details please login into your account and then edit profile from account.");
                //    }
                //    else if (Convert.ToInt32(value) == -100)
                //    {
                //        showMessage("Your details can't be saved beacause you have not passed the test.");
                //    }

                //    if (Convert.ToInt32(value) >= 0)
                //    {
                //        showSuccessMessage("New user is successfully created.");
                //    }
                //}

                TestUser user = new TestUser
                {
                    ProfileName = tbxProfileName.Text.Trim(),
                    Email = Convert.ToString(tbxEmail.Text.Trim()),
                    Password = tbxPassword.Text.Trim(),
                    FullName = Convert.ToString(tbxFullName.Text.Trim()),
                    IdCardNum = tbxIdCardNumber.Text.Trim(),
                    MobileNumber = tbxMobileNumber.Text.Trim(),
                    Education = Convert.ToString(ddlEducation.SelectedValue),
                    Experience = Convert.ToString(tbxExperience.Text),

                    Description = "",
                    ImagPath = "",
                    BankName = "",
                    AccountNum = "",
                    AccountTitle = "",
                    AccountType = "",
                    Branch = "",
                    BankCode = "",
                    City = "",
                    Country = ""
                };

                string value = objMyDBClass.SavePassedUser(user);
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
    }

    public class WokmeterUser
    {
        public String Login { get; set; }
        public String Password { get; set; }
        public String UserName { get; set; }
        public String Email { get; set; }
    }
}
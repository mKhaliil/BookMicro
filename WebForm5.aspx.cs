using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Web.Services;
using System.Drawing;

namespace Outsourcing_System
{
    public partial class WebForm5 : System.Web.UI.Page
    {
        private MyDBClass objMyDBClass = new MyDBClass();

        protected void Page_Load(object sender, EventArgs e)
        {

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

            ////Fail unChecked tests
            //foreach (var failedTest in TestTypes)
            //{
            //    if (clearedTests.Length == 0)
            //    {
            //        foreach (var test in TestTypes)
            //        {
            //            returnVal = objMyDBClass.ClearTests(userName, failedTest, "failed", failedTest);
            //        }
            //    }

            //    else
            //    {
            //        foreach (var test in clearedTests)
            //        {
            //            if (!(failedTest.Equals(test)))
            //            {
            //                returnVal = objMyDBClass.ClearTests(userName, failedTest, "failed", failedTest);
            //            }
            //        }
            //    }
            //}

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
                ////if (tbxIdCardNumber.Text != "")
                ////{
                ////    string value = objMyDBClass.SavePassedUser(Convert.ToString(tbxProfileName.Text),
                ////        Convert.ToString(tbxPassword.Text), Convert.ToString(tbxFullName.Text),
                ////        Convert.ToString(tbxEmail.Text), Convert.ToString(tbxIdCardNumber.Text),
                ////        Convert.ToString(tbxMobileNumber.Text),
                ////        Convert.ToString(ddlEducation.SelectedValue), "", Convert.ToString(tbxExperience.Text), "", "", "", "", "", "", "", "", "");

                ////    if (Convert.ToInt32(value) == -101)
                ////    {
                ////        showMessage(
                ////            "You have already saved your details please login into your account and then edit profile from account.");
                ////    }
                ////    else if (Convert.ToInt32(value) == -100)
                ////    {
                ////        showMessage("Your details can't be saved beacause you have not passed the test.");
                ////    }

                ////    if (Convert.ToInt32(value) >= 0)
                ////    {
                ////        showSuccessMessage("New user is successfully created.");
                ////    }
                ////}
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
}
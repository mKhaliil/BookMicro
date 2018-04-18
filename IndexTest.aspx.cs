using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using Outsourcing_System.MasterPages;

namespace Outsourcing_System
{
    public partial class IndexTest : System.Web.UI.Page
    {

        #region |Fields and Properties|


        GlobalVar objGlobal = new GlobalVar();
        MyDBClass objMyDBClass = new MyDBClass();
        TestUser currentUser = new TestUser();

        #endregion

        #region |Events|

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                //((OnlineTestRegisterdMaster)this.Page.Master).SetLogOut = true;
                //((UserMaster)this.Page.Master).ShowLogOutButton();
                //((UserMaster)this.Page.Master).SetMenuLocation = "-120px";
                bindGrid();
            }

        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            if (fZipUpload.HasFile)
            {

                if (Session["UserDetail"] != null)
                {
                    currentUser = (TestUser)Session["UserDetail"];
                    string userid = currentUser.UserId;
                    List<TestDetail> testDetails = objMyDBClass.getTestDetails(userid, "Index");

                    string userDir_Path = objMyDBClass.MainDirPhyPath + "\\Tests\\" + currentUser.Email + "\\IndexTests\\" + testDetails[0].testName.Replace(".pdf", "") + "//";
                    if (!Directory.Exists(userDir_Path))
                    {
                        Directory.CreateDirectory(userDir_Path);
                    }

                    fZipUpload.SaveAs(userDir_Path + fZipUpload.FileName);
                    objMyDBClass.updateOnlineTestDetails("Index", currentUser.UserId, "Pending Confirmation", System.DateTime.Now.ToString());

                }

                Response.Redirect("OnlineTestUser.aspx");

            }

        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        #endregion

        protected void lnlIndexTool_Click(object sender, EventArgs e)
        {
            string filePath = @"C:\E-Index_BookMicro.zip";
            Context.Response.Clear();
            Context.Response.ContentType = "application/x-zip-compressed";
            Context.Response.AddHeader("Content-Disposition", "attachment; filename=E-Index_BookMicro.zip");
            Context.Response.WriteFile(filePath);
            //Response.Redirect("ImageTest.aspx");
            Context.Response.End();
        }

        protected void lnkAssignedTest_Click(object sender, EventArgs e)
        {

            if (Session["UserDetail"] != null)
            {
                currentUser = (TestUser)Session["UserDetail"];

                string userid = currentUser.UserId;
                List<TestDetail> testDetails = objMyDBClass.getTestDetails(userid, "Index");

                string filePath = "D://Files/Tests/Original/Index//";
                //string filePath = "D://Office Data/Files/Tests/Original/Index//";
                Context.Response.Clear();
                Context.Response.ContentType = "application/pdf";
                Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + testDetails[0].testName);
                Context.Response.WriteFile(filePath + testDetails[0].testName);
                //Response.Redirect("ImageTest.aspx");
                Context.Response.End();
            }
        }


        protected void lnkTest_Click(object sender, EventArgs e)
        {
            List<int> alreadyExitedTests = new List<int>();
            if (Session["UserDetail"] != null)
            {
                currentUser = (TestUser)Session["UserDetail"];

                string userid = currentUser.UserId;
                List<TestDetail> testDetails = objMyDBClass.getTestDetails(userid, "Index");
                if (testDetails != null && (testDetails.Count > 0))
                {
                    foreach (TestDetail test in testDetails)
                    {

                        int number = Convert.ToInt32(test.testName.Replace(".pdf", ""));
                        alreadyExitedTests.Add(number);
                    }
                }
                if (alreadyExitedTests.Count > 0)
                {
                    showMessage("You Test is in process Mode. Please upload your in process test first.");
                }
                else
                {
                    string fileName = GetTestFile(alreadyExitedTests);

                    if (Session["UserDetail"] != null)
                    {
                        currentUser = (TestUser)Session["UserDetail"];
                    }
                    if (fileName.Equals("No Test"))
                    {
                    }
                    else
                    {
                        objMyDBClass.insertOnlineTestDetails("Index", currentUser.UserId, fileName, "In Process", System.DateTime.Now.ToString());
                        lnkTest.Visible = false;
                        ConfirmationPnl.Visible = true;

                        string filePath = "D://Files/Tests/Original/Index//"; 
                        //string filePath = "D://Office Data/Files/Tests/Original/Index//";
                        Context.Response.Clear();
                        Context.Response.ContentType = "application/pdf";
                        Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                        Context.Response.WriteFile(filePath + fileName);
                        //Response.Redirect("ImageTest.aspx");
                        Context.Response.End();
                    }
                }
            }
            else
            {
                Response.Redirect("BookMicro.aspx");
            }

        }
        private void bindGrid()
        {
            if (Session["UserDetail"] != null)
            {
                currentUser = (TestUser)Session["UserDetail"];
            }
            else
            {
                Response.Redirect("Bookmicro.aspx", true);
            }

            string userid = currentUser.UserId;
            List<TestDetail> testDetails = objMyDBClass.getTestDetails(userid, "Index");
            if (testDetails != null && (testDetails.Count > 0))
            {
                foreach (TestDetail test in testDetails)
                {
                    if (test.status.Equals("In Process"))
                    {
                        lnkTest.Visible = false;
                    }
                    else if (test.status.Contains("Pending"))
                    {
                        showMessage("Your test is submited and in process on BookMicro side. ");
                        lnkTest.Visible = false;
                        ConfirmationPnl.Visible = false;
                    }
                    else if (test.status.Equals("Passed"))
                    {
                        lnkTest.Visible = false;
                        ConfirmationPnl.Visible = false;
                        showMessage("You have already passed this test.");
                    }
                }
            }
            else
            {
                ConfirmationPnl.Visible = false;
            }

        }
        private string GetTestFile(List<int> alreadyExisted)
        {

            Random rnd = new Random();
            int rndNumber = 0;

            rndNumber = rnd.Next(1, 11);
            int numbersTested = 0;
            while (alreadyExisted.Contains(rndNumber))
            {
                if (numbersTested > 11)
                {
                    return "No Test";
                }
                rndNumber = rnd.Next(1, 11);
                numbersTested = numbersTested + 1;
            }
            return rndNumber + ".pdf";

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
    }
}

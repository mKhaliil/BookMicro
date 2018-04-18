using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Xml;
using Org.BouncyCastle.Asn1.Ocsp;
using System.Drawing;
using Outsourcing_System.MasterPages;

namespace Outsourcing_System
{
    public partial class Training : System.Web.UI.Page
    {
        #region |Fields and Properties|
        GlobalVar objGlobal = new GlobalVar();
        MyDBClass obj = new MyDBClass();
        TestUser currentUser = new TestUser();
        #endregion
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    if (Convert.ToString(Session["LoginId"]) != "")
                    {
                        //((UserMaster)this.Page.Master).ShowLogOutButton();
                        //((UserMaster)this.Page.Master).SetMenuLocation = "-120px";

                        string countryName = Convert.ToString(Session["CountryName"]);

                        if (countryName != "")
                        {
                            //if (countryName.Equals("pakistan"))
                            //{
                            //    lblComparisonTaskReward.Text = "Rs. 500";
                            //    lblTableTaskReward.Text = "Rs. 300";
                            //    lblImageTaskReward.Text = "Rs. 200";
                            //    lblIndexTaskReward.Text = "Rs. 500";
                            //    lblMappingTaskReward.Text = "Rs. 3500";
                            //}
                            //else if (countryName.Equals("other"))
                            //{
                                //lblComparisonTaskReward.Text = "$ 5";
                                //lblTableTaskReward.Text = "$ 3";
                                //lblImageTaskReward.Text = "$ 2";
                                //lblIndexTaskReward.Text = "$ 5";
                                //lblMappingTaskReward.Text = "$ 35";
                            //}
                        }

                        //lblComparisonTaskReward.Text = "$ 5";
                        //lblTableTaskReward.Text = "$ 3";
                        //lblImageTaskReward.Text = "$ 2";
                        //lblIndexTaskReward.Text = "$ 5";
                        //lblMappingTaskReward.Text = "$ 35";
                    }
                    else
                    {
                        Response.Redirect("Bookmicro.aspx", true);
                    }
                }
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
            }
        }

        protected void lnkTest_Click(object sender, EventArgs e)
        {
            try
            {
                string filePath = "D://Files/Tests/Original/Sample Book.pdf";
                //string filePath = "D://Office Data/Files/Tests/Original/";
                string fileName = "Sample.pdf";
                Context.Response.Clear();
                Context.Response.ContentType = "application/pdf";
                Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + fileName);
                Context.Response.WriteFile(filePath + fileName);
                //HttpContext.Current.ApplicationInstance.CompleteRequest();
                Context.Response.End();
                //Response.Redirect("Trainig.aspx");
            }
            catch (Exception ex)
            {
                showMessage(ex.Message);
            }
        }

        //protected void lbtnUrdu_Click(object sender, EventArgs e)
        //{
        //    mvTrainingVideos.ActiveViewIndex = 1;
        //    lbtnUrdu.Attributes.Add("Style", "color:#CCCCCC");
        //    lbtnEnglish.Attributes.Add("Style", "color:#0099ff");
        //    lbtnEnglish.Enabled = true;
        //}
        //protected void lblEnglish_Click(object sender, EventArgs e)
        //{
        //    mvTrainingVideos.ActiveViewIndex = 0;
        //    lbtnUrdu.Attributes.Add("Style", "color:#0099ff");
        //    lbtnEnglish.Attributes.Add("Style", "color:#CCCCCC");
        //    lbtnUrdu.Enabled = true;
        //}

        protected void ibtnImagesTraining_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImageTutorials.aspx", false);
        }

        protected void ibtnCompTask_Click(object sender, EventArgs e)
        {
            MyDBClass objMyDBClass = new MyDBClass();
            List<TestDetail> testDetails = objMyDBClass.getTestDetails(Convert.ToString(Session["LoginId"]), "ErrorDetection");
            if (testDetails != null && (testDetails.Count > 0))
            {
                int index_Passed = testDetails.FindIndex((x => x.status.Equals("Passed"))) == -1 ? 0 : testDetails.FindIndex((x => x.status.Equals("Passed")));

                if (testDetails[index_Passed].status.Equals("Passed"))
                {
                    OnlineTestMasterPage ParentMasterPage = (OnlineTestMasterPage)Page.Master;
                    ParentMasterPage.ShowMessageBox("You have already passed this test.", "Info");
                }
                else
                {
                    Response.Redirect(string.Format("ComparisonPreProcess.aspx?uid={1}&bid={2}&type={3}&email={4}", Request.Url.Host, Convert.ToString(Session["LoginId"]), "", "test", Convert.ToString(Session["email"])), true);
                }
            }
            else
            {
                Response.Redirect(string.Format("ComparisonPreProcess.aspx?uid={1}&bid={2}&type={3}&email={4}", Request.Url.Host, Convert.ToString(Session["LoginId"]), "", "test", Convert.ToString(Session["email"])), true);
            }
        }

        protected void ibtnQuiz_Click(object sender, EventArgs e)
        {
            //Response.Redirect(string.Format("ComparisonPreProcess.aspx?uid={1}&bid={2}&type={3}&email={4}", Request.Url.Host, Convert.ToString(Session["LoginId"]), "", "onepagetest", Convert.ToString(Session["email"])), true);
            Response.Redirect("QuizType.aspx", true);
        }

        protected void ibtnCompTaskTraining_Click(object sender, EventArgs e)
        {
            Response.Redirect("ComparisonTutorial.aspx", false);
        }

        protected void ibtnMappingTraining_Click(object sender, EventArgs e)
        {
            Response.Redirect("MappingTutorials.aspx", false);
        }
        protected void ibtnIndexingTraining_Click(object sender, EventArgs e)
        {
            Response.Redirect("IndexingTutorials.aspx", false);
        }

        protected void ibtnTableTraining_Click(object sender, EventArgs e)
        {
            Response.Redirect("TableTutorials.aspx", false);
        }

        protected void btnGoToProfile_Click(object sender, EventArgs e)
        {
            try
            {
                int userId = 0;
                if ((Convert.ToString(Session["LoginId"]) != null) && (Convert.ToString(Session["LoginId"]) != ""))
                {
                    userId = Convert.ToInt32(Session["LoginId"]);
                    Response.Redirect("OnlineTestUser.aspx");
                }
                else
                {
                    showMessage("Your session is expired.");
                }
            }
            catch (Exception ex)
            {

                showMessage(ex.Message);
            }
        }
        private void showSuccessMessage(string message)
        {
            try
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
            catch (Exception ex)
            {

                showMessage(ex.Message);
            }
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
        protected void ibtnTable_Click(object sender, EventArgs e)
        {
            if (obj.TableTest == "0")
            {
                showSuccessMessage("Please first complete training.");
            }
            else
            {
                ibtnTable.Enabled = true;
            }
        }

        protected void ibtnIndexing_Click(object sender, EventArgs e)
        {
            if (obj.IndexingTest == "0")
            {
                showSuccessMessage("Please first complete training.");
            }
            else
            {
                ibtnIndexing.Enabled = true;
            }
        }

        protected void ibtnImages_Click(object sender, EventArgs e)
        {
            if (obj.ImagesTest == "0")
            {
                showSuccessMessage("Please first complete training.");
            }
            else
            {
                ibtnImages.Enabled = true;
            }
        }

        protected void ibtnMapping_Click(object sender, EventArgs e)
        {
            try
            {
                if (obj.MappingTest == "0")
                {
                    showSuccessMessage("You can't perform this test. Please first complete some tasks.");
                }
                //else
                //{
                //    ibtnIndexing.Enabled = true;
                //}

                else if (Session["UserDetail"] != null)
                {
                    currentUser = (TestUser)Session["UserDetail"];

                    currentUser = (TestUser)Session["UserDetail"];
                    string userDetails = obj.CheckTestStatus(currentUser.FullName, currentUser.Email, "Mapping");

                    if ((userDetails != null) && (userDetails != ""))
                    {
                        showMessage("Test can't be started because you have already passed the test.");
                        //imgbtnStartTest.Enabled = false;
                        return;
                    }
                    //end

                    //Passed users cannot give tests of different types in next 24 hours
                    var testDates = obj.GetTestDate(currentUser.FullName, currentUser.Email);

                    if ((testDates != null) && (testDates.Count > 0))
                    {
                        if (DateTime.Now <= Convert.ToDateTime(testDates[0]).AddHours(24))
                        {
                            showMessage("Test can't be started because only one attempt per day is allowed.");
                            return;
                        }
                    }
                    //end

                    if (((currentUser.FullName != null) && (currentUser.FullName != "")) && ((currentUser.Email != null) && (currentUser.Email != "")))
                    {
                        Session["OnlineUser"] = currentUser.FullName;
                        Session["email"] = currentUser.Email;

                        string path = obj.MainDirPhyPath + @"\\Tests\\Original\\";

                        string ipAddress = GetIPAddress();

                        //Resriction on using same IP Address for different email addresses
                        List<TestUser> list = obj.GetUserDetails_ByIPAdress(currentUser.FullName, currentUser.Email, ipAddress);
                        bool emailCheck = false;

                        if ((list != null) && (list.Count > 0))
                        {
                            foreach (var item in list)
                            {
                                if ((item.Email != currentUser.Email))
                                {
                                    emailCheck = true;
                                }
                            }

                            if (emailCheck)
                            {
                                string value1 = obj.InsertOnlineTest_IPAddress(currentUser.FullName, currentUser.Email, ipAddress);
                            }
                        }
                        //end

                        List<string> emailId_List = obj.GetEmailId_ByName(currentUser.FullName);
                        bool check = false;

                        if ((emailId_List != null) && (emailId_List.Count > 0))
                        {
                            foreach (var item in emailId_List)
                            {
                                if (item.Trim().Equals(currentUser.Email))
                                {
                                    check = true;
                                    break;
                                }
                            }
                        }
                        int rndNumber = 0;
                        //if there is already a test in process then same test should be reassigned.
                        int InProcessTestName = obj.GetInProcessTestName(currentUser.FullName, currentUser.Email);
                        if (InProcessTestName != 0)
                        {
                            string orignalDir = obj.MainDirPhyPath + "\\Tests\\Original\\Mapping\\" + InProcessTestName.ToString();
                            string userDir_Path = obj.MainDirPhyPath + "\\Tests\\" + currentUser.Email;
                            string userDir = obj.MainDirPhyPath + "\\Tests\\" + currentUser.Email + "/MappingTests/" + InProcessTestName;
                            if (!Directory.Exists(userDir))
                            {
                                Directory.CreateDirectory(userDir);
                                DirectoryInfo dInfo = CopyTo(new DirectoryInfo(orignalDir), userDir);
                                File.Delete(userDir + "\\" + InProcessTestName + ".rhyw");
                                File.Copy(userDir + "\\" + InProcessTestName + "-1" + "\\TaggingUntagged\\" + InProcessTestName + "-1.rhyw", userDir + "\\" + InProcessTestName + ".rhyw");
                            }

                            Session["TotalMarks"] = GetTotalMistakes(currentUser.Email, Convert.ToString(InProcessTestName));
                            Session["TestName"] = Convert.ToString(InProcessTestName);
                            Session["email"] = currentUser.Email;
                            Session["OnlineUser"] = currentUser.FullName;
                            Session["orignalDir"] = orignalDir + "/" + InProcessTestName.ToString() + ".rhyw";
                            //string value = obj.InsertOnlineTest(currentUser.FullName, currentUser.Email, Convert.ToString(InProcessTestName), GetIPAddress(), Convert.ToInt32(Session["TotalMarks"]), "Mapping");
                            Response.Redirect("MappingTest.aspx?username=" + currentUser.FullName + "&bid=" + InProcessTestName, false);
                        }
                        else
                        {
                            Random rnd = new Random();
                            rndNumber = rnd.Next(101, 107);
                            if (check)
                            {
                                int oldTestName = obj.GetTestName(currentUser.FullName, currentUser.Email);

                                oldTestName++;

                                if (oldTestName >= 108)
                                    rndNumber = 101;

                                else
                                    rndNumber = oldTestName;
                            }



                            string orignalDir = obj.MainDirPhyPath + "\\Tests\\Original\\Mapping\\" + rndNumber.ToString();
                            string userDir_Path = obj.MainDirPhyPath + "\\Tests\\" + currentUser.Email;
                            string userDir = obj.MainDirPhyPath + "\\Tests\\" + currentUser.Email + "/MappingTests\\" + rndNumber;

                            if (Directory.Exists(userDir))
                            {
                                Directory.Delete(userDir, true);
                            }
                            if (!Directory.Exists(userDir))
                            {
                                Directory.CreateDirectory(userDir);
                                DirectoryInfo dInfo = CopyTo(new DirectoryInfo(orignalDir), userDir);
                                File.Delete(userDir + "\\" + rndNumber + ".rhyw");
                                File.Copy(userDir + "\\" + rndNumber + "-1" + "\\TaggingUntagged\\" + rndNumber + "-1.rhyw", userDir + "\\" + rndNumber + ".rhyw");
                            }

                            Session["TotalMarks"] = GetTotalMistakes(currentUser.Email, Convert.ToString(rndNumber));
                            Session["TestName"] = Convert.ToString(rndNumber);
                            Session["email"] = currentUser.Email;
                            Session["OnlineUser"] = currentUser.FullName;
                            Session["orignalDir"] = orignalDir + "/" + rndNumber.ToString() + ".rhyw";
                            string value = obj.InsertOnlineTest(currentUser.FullName, currentUser.Email, Convert.ToString(rndNumber), GetIPAddress(), Convert.ToInt32(Session["TotalMarks"]), "Mapping");
                            Response.Redirect("MappingTest.aspx?username=" + currentUser.FullName + "&bid=" + rndNumber, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                showMessage(ex.Message);
            }
        }
        public DirectoryInfo CopyTo(DirectoryInfo sourceDir, string destinationPath, bool overwrite = false)
        {
            try
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
            catch (Exception ex)
            {

                throw ex;
            }

        }
        private int GetTotalMistakes(string email, string rndNumber)
        {
            try
            {
                loadLatestXml_New(email, rndNumber);
                XmlNodeList correctedNodes = objGlobal.PBPDocument.SelectNodes(@"//ln[@conversion]");
                XmlNodeList inBoxLines = objGlobal.PBPDocument.SelectNodes(@"//ln[@inbox]");
                XmlNodeList editingLeftNodes = objGlobal.PBPDocument.SelectNodes(@"//*[@correction]");
                int totalOccurences = correctedNodes.Count + inBoxLines.Count + editingLeftNodes.Count;
                return totalOccurences;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void loadLatestXml_New(string email, string rndNumber)
        {
            try
            {
                string xmlFile = "";
                if (email != "")
                {
                    xmlFile = obj.MainDirPhyPath + "/Tests/" + email + "/MappingTests/" + rndNumber + "/" + rndNumber + ".rhyw";
                }
                else
                {
                    xmlFile = obj.MainDirPhyPath + "/" + email + "/MappingTests/" + rndNumber + ".rhyw";
                }
                objGlobal.XMLPath = xmlFile;
                Session["XMLPath"] = objGlobal.XMLPath;
                objGlobal.PBPDocument = new System.Xml.XmlDocument();
                objGlobal.LoadXml();
                Session["PBPDocument"] = objGlobal.PBPDocument;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        public string GetIPAddress()
        {
            //var aa = Request.ServerVariables["REMOTE_ADDR"].ToString();
            //var bb = Request.ServerVariables["http_user_agent"].ToString();
            //var cc = Request.ServerVariables["request_method"].ToString();
            //var server_name = Request.ServerVariables["server_name"].ToString();
            //var dd = Request.ServerVariables["server_port"].ToString();
            //var ee = Request.ServerVariables["server_software"].ToString();
            //var ff= Request.ServerVariables["REMOTE_HOST"].ToString();

            string ipaddress;

            try
            {
                ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (ipaddress == "" || ipaddress == null)
                    ipaddress = Request.ServerVariables["REMOTE_ADDR"];

                return ipaddress;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Collections;
using System.IO;
using System.Xml.Linq;
using EO.Web;
using System.Xml;
using System.Data;
//using System.Activities.Statements;
using System.Threading.Tasks;
using Ionic.Zip;
using iTextSharp.text.pdf;
using System.Diagnostics;
using iTextSharp.text;
using System.Threading;
using Outsourcing_System;
using System.Data.SqlClient;
using Outsourcing_System.readableEnglishService;

namespace Outsourcing_System
{
    public partial class web_ComparisonPreProcess : System.Web.UI.Page
    {
        //private long counter = 0;
        private MyDBClass objMyDBClass = new MyDBClass();

        public string imgResourceFolderPath
        {
            get { return Convert.ToString(ViewState["imgResourceFolderPath"]); }
            set { ViewState["imgResourceFolderPath"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                string userId = Convert.ToString(Session["LoginId"]);
                string bookId = Convert.ToString(Session["BID"]);
                string email = Convert.ToString(Session["email"]);
                string TaskType = Convert.ToString(Request.QueryString["type"]);
                string quizType = Convert.ToString(Request.QueryString["quiztype"]);
                //string TaskType = "onepagetest";

                ////For restricting user to go back to previous page after test/task is started
                //Session["ErrorDetection_Started"] = "1";
                ////end

                if (bookId != "")
                {
                    string queryBookID = "select MainBook from book where BID=" + bookId;
                    bookId = objMyDBClass.GetID(queryBookID);
                    Session["BookId"] = bookId;
                }

                if ((quizType != "") && (quizType != null))
                {
                    Session["quizType"] = quizType;
                }

                Session["UserId"] = userId;
                Session["ComparisonTestUser_Email"] = email;

                if (TaskType != null)
                {
                    if (TaskType.Equals("onepagetest"))
                    {
                        Session["ComparisonTask"] = "onepagetest";
                        StartTest(email, userId, bookId, "onepagetest");
                    }
                    else if (TaskType.Equals("test"))
                    {
                        Session["ComparisonTask"] = "test";
                        StartTest(email, userId, bookId);
                    }
                    else if (TaskType.Equals("task"))
                    {
                        Session["ComparisonTask"] = "task";
                        StartTask(bookId, userId);
                    }
                    else if (TaskType.Equals("comparisonEntryTest"))
                    {
                        divTestInstr.Visible = true;
                        Session["ComparisonTask"] = "comparisonEntryTest";
                        StartComparisonEntryTest(email, "comparisonEntryTest");
                    }
                    else if (TaskType.Equals("CompUpgradedSampleTest"))
                    {
                        divTestInstr.Visible = true;
                        Session["ComparisonTask"] = "CompUpgradedSampleTest";
                        StartComparisonEntryTest(email, "CompUpgradedSampleTest");
                    }
                    else if (TaskType.Equals("CompUpgradedStartTest"))
                    {
                        divTestInstr.Visible = true;
                        Session["ComparisonTask"] = "CompUpgradedStartTest";
                        StartComparisonEntryTest(email, "CompUpgradedStartTest");
                    }
                    //CreateFolderHierarchy(userId, bookId, "");
                }
            }
        }

        public double CalculateTaskCost(string tool, string task, string complexity, int PageCount)
        {
            MyDBClass dbObj = new MyDBClass();
            string OnePageTime_InSeconds = dbObj.GetTaskThroughputInSec(tool, task, complexity);

            if (string.IsNullOrEmpty(OnePageTime_InSeconds)) return 0;

            double salary = 0;

            string userRank = Convert.ToString(Session["UserRank"]);
            if (!string.IsNullOrEmpty(userRank))
            {
                string configKeyName = string.Empty;

                if (userRank.Trim().Equals("Trainee Editor")) configKeyName = "TraineerEditorSalary";
                else if (userRank.Trim().Equals("Junior Editor")) configKeyName = "JuniorEditorSalary";
                else if (userRank.Trim().Equals("Editor")) configKeyName = "EditorSalary";
                else if (userRank.Trim().Equals("Senior Editor")) configKeyName = "SeniorEditorSalary";
                else if (userRank.Trim().Equals("Expert Editor")) configKeyName = "ExpertEditorSalary";

                salary = ConfigurationManager.AppSettings[configKeyName] == "" ? 0 : Convert.ToDouble(ConfigurationManager.AppSettings[configKeyName]);
            }

            if (salary > 0)
            {
                double workingDays = ConfigurationManager.AppSettings["WorkingDays"] == "" ? 0 : Convert.ToDouble(ConfigurationManager.AppSettings["WorkingDays"]); ;
                double dailyRequiredTime = ConfigurationManager.AppSettings["DailyRequiredTime"] == "" ? 0 :
                                            Convert.ToDouble(ConfigurationManager.AppSettings["DailyRequiredTime"]);

                double oneDaySalary = salary / workingDays;
                double oneHourSalary = oneDaySalary / dailyRequiredTime;

                double taskSalary = (oneHourSalary / 60) * ((Convert.ToDouble(OnePageTime_InSeconds) * PageCount) / 60);

                double currency = Math.Round(taskSalary / Convert.ToDouble(ConfigurationManager.AppSettings["AUD"]), 2);

                return currency;
            }

            return 0;
        }

        public int GetTotalInjectedMistakes(string bookId, string userId)
        {
            Common commObj = new Common();

            string path = ConfigurationManager.AppSettings["MainDirPhyPath"] + "\\" + bookId + "\\" + bookId + "-1\\Comparison\\Comparison-1\\" +
                          userId + "\\" + bookId + "-1.rhyw";

            if (!File.Exists(path)) return 0;

            commObj.LoadXml(path);
            XmlDocument doc = commObj.xmlDoc;

            int totalErrors = doc.SelectNodes(@"//*[@correction]").Count + doc.SelectNodes(@"//*[@conversion]").Count;

            return totalErrors;
        }

        private void LoadInProgressTaskPaths()
        {
            string pDirPath = Common.GetDirectoryPath();

            string srcFilePath = "";
            string rhywFilePath = "";

            if (Convert.ToString(Session["ComparisonTask"]) != "")
            {
                if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
                {
                    Session["SrcPDF"] = Convert.ToString(Session["BookId"]) + ".pdf";
                    Session["rhywFile"] = Convert.ToString(Session["BookId"]) + ".rhyw";

                    srcFilePath = Common.GetTestFiles_SavingPath() + "/" + Convert.ToString(Session["BookId"]) + ".pdf";
                    rhywFilePath = Common.GetTestFiles_SavingPath() + "/" + Convert.ToString(Session["BookId"]) +
                                   ".rhyw";
                }
                else if (Convert.ToString(Session["ComparisonTask"]).Equals("task"))
                {
                    Session["comparisonType"] = "1";
                    Session["SrcPDF"] = Convert.ToString(Session["BookId"]) + "-1.pdf";
                    Session["rhywFile"] = Convert.ToString(Session["BookId"]) + "-1.rhyw";

                    srcFilePath = Common.GetTaskFiles_SavingPath() + Convert.ToString(Session["SrcPDF"]);
                    rhywFilePath = Common.GetTaskFiles_SavingPath() + Convert.ToString(Session["rhywFile"]);
                }
            }

            PdfReader srcPdf = new PdfReader(srcFilePath);
            int srcTotalPages = srcPdf.NumberOfPages;
            srcPdf.Close();

            Session["srcTotalPages"] = srcTotalPages;
            int counter = 0;

            Common commObj = new Common();
            commObj.LoadXml(rhywFilePath);
            commObj.xmlDoc.Save(rhywFilePath.Replace(".rhyw", ".xml"));
            Session["MainXMLFilePath"] = rhywFilePath.Replace(".rhyw", ".xml");

            if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
            {
                int totalMistakes = commObj.xmlDoc.SelectNodes(@"//ln[@PDFmistake]").Count;
                Session["ComparisonTestTotalMistakes"] = totalMistakes;
            }

            Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPathCoord"];

            //Set default values of variables used in xsls
            SetXls_Variables();

            ////////////////////////////////////////////////////////////////////////////

            if (Convert.ToString(Session["ComparisonTask"]).Equals("task"))
            {
                //Find normalx and normalIndent from whole pdf file

                RHYWManipulation obj = new RHYWManipulation();



                //Get normal font from whole pdf
                string normalFont = obj.GetNormalFont(commObj.xmlDoc);
                Session["normalFont"] = normalFont;

                string level1 = obj.GetLevelFontSize("level1", commObj.xmlDoc);
                string level2 = obj.GetLevelFontSize("level2", commObj.xmlDoc);
                string level3 = obj.GetLevelFontSize("level3", commObj.xmlDoc);
                string level4 = obj.GetLevelFontSize("level4", commObj.xmlDoc);

                Session["level1"] = level1;
                Session["level2"] = level2;
                Session["level3"] = level3;
                Session["level4"] = level4;

                double normalx_EvenPages = 0;
                double normalIndent_EvenPages = 0;
                obj.NormalAndIndentX_EvenPages(commObj.xmlDoc, ref normalx_EvenPages, ref normalIndent_EvenPages);

                double normalx_OddPages = 0;
                double normalIndent_OddPages = 0;
                obj.NormalAndIndentX_OddPages(commObj.xmlDoc, ref normalx_OddPages, ref normalIndent_OddPages);

                if (normalx_OddPages == normalx_EvenPages && normalIndent_OddPages == normalIndent_EvenPages)
                {
                    double normalx = 0;
                    double normalIndent = 0;

                    //obj.NormalAndIndentX_Old(Common.xmlDoc, ref normalx, ref normalIndent);
                    //normalx = 0;
                    //normalIndent = 0;

                    obj.NormalAndIndentX(commObj.xmlDoc, ref normalx, ref normalIndent);
                    Session["normalx"] = normalx;
                    Session["normalIndent"] = normalIndent;
                    Session["EvenOddLeftDifference"] = "false";
                }
                else
                {
                    Session["normalx_EvenPages"] = normalx_EvenPages;
                    Session["normalIndent_EvenPages"] = normalIndent_EvenPages;
                    Session["normalx_OddPages"] = normalx_OddPages;
                    Session["normalIndent_OddPages"] = normalIndent_OddPages;
                    Session["EvenOddLeftDifference"] = "true";
                }

                try
                {
                    //Log in task start time in workmeter on loading existing errordetection task
                    string userId = Convert.ToString(Session["LoginId"]);
                    string bookId = Convert.ToString(Session["MainBook"]);
                    string TaskId = getTaskId(bookId);

                    if (TaskId != "")
                    {
                        SqlConnection objConnection = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
                        objConnection.Open();
                        string updaterQuery = "update tblTaskDetails set StartTime='" + DateTime.Now +
                                              "',EndTime='',Current_Status='working' where TaskId=" + TaskId;
                        SqlCommand cmdUpdate = new SqlCommand(updaterQuery, objConnection);
                        int res = cmdUpdate.ExecuteNonQuery();
                        objConnection.Close();

                        handleLog(TaskId, "(checked In) ", userId);
                    }
                    //end
                }
                catch (Exception)
                {
                }
            }
            ////////////////////////////////////////////////////////////////////////////

            if (srcTotalPages > 0)
                Session["MainCurrPage"] = "1";

            //For old comparison viewer
            //Response.Redirect("Comparison.aspx", true);

            Response.Redirect("ErrorDetection.aspx", true);

        }

        //public void CreateFolderHierarchy(string userId, string bookId, string path)
        //{
        //    AutoMapService.AutoMappService autoMapSvc = new AutoMapService.AutoMappService();
        //    autoMapSvc.AllowAutoRedirect = true;
        //    autoMapSvc.CreateTaggingUntaggingAsync(userId, bookId, path);
        //    System.Threading.Thread.Sleep(1000 * 12);
        //    autoMapSvc.CancelAsync(null);
        //    autoMapSvc.Dispose();
        //}

        public bool CheckTaskCompletionStatus(string bookId, string userId)
        {
            var temp = MyDBClass.CheckTaskCompleteness(bookId, userId);

            var taskDetails = temp.Split(',');

            taskDetails = taskDetails.Where(x => (!string.IsNullOrEmpty(x))).ToArray();

            if (taskDetails.Length == 0)
                return true;

            //If status == 0 then comparison 1 task is not completed
            if ((taskDetails != null) && (taskDetails.Length > 0))
            {
                if (taskDetails[0].Equals("0"))
                    return false;
            }

            return true;
        }

        private void StartTest(string email, string userId, string bookId, string testType = "")
        {
            string quizType = Convert.ToString(Session["quizType"]);
            string ComparisonTask = Convert.ToString(Session["ComparisonTask"]);

            if ((email == "") || (userId == ""))
                return;

            int index_Passed = 0;
            int index_InProcess = 0;

            if (ComparisonTask.Equals("test"))
            {
                //Resume old task if not completed 
                List<TestDetail> testDetails = objMyDBClass.getTestDetails(Convert.ToString(Session["UserId"]), "ErrorDetection");

                if (testDetails != null && (testDetails.Count > 0))
                {
                    index_Passed = testDetails.FindIndex((x => x.status.Equals("Passed"))) == -1
                       ? 0
                       : testDetails.FindIndex((x => x.status.Equals("Passed")));
                    index_InProcess = testDetails.FindIndex((x => x.status.Equals("In Process"))) == -1
                       ? 0
                       : testDetails.FindIndex((x => x.status.Equals("In Process")));
                    var indexes_Failed =
                        Enumerable.Range(0, testDetails.Count).Where(i => testDetails[i].status == "Failed").ToList();

                    if (indexes_Failed != null && (indexes_Failed.Count > 0))
                    {
                        //Delete failed test folders of user
                        bool IsFileLocked = false;

                        string delDirPath = Common.GetDirectoryPath() + "\\Tests\\" +
                                            Convert.ToString(HttpContext.Current.Session["ComparisonTestUser_Email"]) +
                                            "/ComparisonTests/";

                        foreach (var failedTestIndex in indexes_Failed)
                        {
                            IsFileLocked = DeleteDirectories(delDirPath + testDetails[failedTestIndex].testName);
                        }
                    }

                    //if (testDetails[index_Passed].status.Equals("Passed"))
                    //{
                    //    OnlineTestMasterPage ParentMasterPage = (OnlineTestMasterPage)Page.Master;
                    //    ParentMasterPage.ShowMessageBox("You have already passed this test.", "Info");
                    //}

                    if (testDetails[index_InProcess].status.Equals("In Process"))
                    {
                        Session["BookId"] = testDetails[index_InProcess].testName;
                        LoadInProgressTaskPaths();
                    }
                }

                //if (testDetails[index_Passed].status.Equals("Passed"))
                //{
                //    OnlineTestMasterPage ParentMasterPage = (OnlineTestMasterPage)Page.Master;
                //    ParentMasterPage.ShowMessageBox("You have already passed this test.", "Info");
                //}

                if (testDetails[index_InProcess].status.Equals("In Process"))
                {
                    Session["BookId"] = testDetails[index_InProcess].testName;
                    LoadInProgressTaskPaths();
                }
            }

            int testName = 0;
            string fileName = "";
            string destFile = "";
            string targetPath = "";
            int minTestNumber = 100;
            int maxTestNumber = 103;

            Random rnd = new Random();
            int rndNumber = rnd.Next(minTestNumber, maxTestNumber);
            //testName = Convert.ToString(rndNumber);

            if (testType.Equals("onepagetest"))
            {
                Session["TimeUpdator_StartTime"] = "";
                Session["TimeUpdator_EndTime"] = "";

                if (quizType.Equals("Splitting"))
                {
                    testName = 112;
                }
                else if (quizType.Equals("Merging"))
                {
                    testName = 113;
                }
                else if (quizType.Equals("Space"))
                {
                    testName = 114;
                }

                Session["BookId"] = testName;
            }
            //Comparison tests
            else
            {
                testName = rndNumber;
                Session["BookId"] = testName;
            }

            string orignalDir = Common.GetTestFiles_InputFilesPath() + testName;
            string userDir_Path = Common.GetTestFiles_SavingPath();

            //If user's quiz test files are locked and then use another quiz
            if (quizType != "")
            {
                bool IsFileLocked = DeleteDirectories(userDir_Path);
                if (IsFileLocked)
                {
                    if (quizType.Equals("Splitting"))
                    {
                        testName = 115;
                    }
                    else if (quizType.Equals("Merging"))
                    {
                        testName = 116;
                    }
                    else if (quizType.Equals("Space"))
                    {
                        testName = 117;
                    }

                    Session["BookId"] = testName;
                    orignalDir = Common.GetTestFiles_InputFilesPath() + testName;
                    userDir_Path = Common.GetTestFiles_SavingPath();
                }
            }
            //end

            //Create test directory and save test files in it.
            Directory.CreateDirectory(userDir_Path);

            targetPath = userDir_Path;

            if (Directory.Exists(orignalDir))
            {
                string[] files = Directory.GetFiles(orignalDir);

                foreach (string s in files)
                {
                    fileName = Path.GetFileName(s);
                    destFile = Path.Combine(targetPath, fileName);

                    if (destFile.Contains(".pdf"))
                        Session["SrcPDF"] = Path.GetFileName(destFile);

                    else if (destFile.Contains(".rhyw"))
                        Session["rhywFile"] = Path.GetFileName(destFile);

                    System.IO.File.Copy(s, destFile);
                }
            }
        }

        private void StartComparisonEntryTest(string email, string testType)
        {
            if ((email == "") || (testType == ""))
                return;

            if (testType.Equals("comparisonEntryTest"))
            {
                Session["TimeUpdator_StartTime"] = "";
                Session["TimeUpdator_EndTime"] = "";
            }
        }

        public static bool DeleteDirectories(string userDir_Path)
        {
            try
            {
                if (Directory.Exists(userDir_Path))
                    Directory.Delete(userDir_Path, true);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return true;
            }

            //file is not locked
            return false;
        }

        public void StartTask(string bookId, string userId)
        {
            //Resume old task if not completed
            bool taskCompletionStatus = CheckTaskCompletionStatus(bookId, userId);

            if (!taskCompletionStatus)
            {
                LoadInProgressTaskPaths();
            }
            //Start new task
            else
            {
                string sourcePath = Common.GetTaskFiles_InputFilesPath();
                string savePath = Common.GetDirectoryPath() + "\\";
                string srcPdfPath = savePath + bookId + "\\" + bookId + ".pdf";
                string email = Convert.ToString(Session["email"]);

                string compPath1 = "";
                string compPath2 = "";
                string comparison = "";
                String extractPath = "";
                string srcFilePath = "";
                string rhywFilePath = "";

                comparison = "1";
                Session["comparisonType"] = comparison;

                compPath1 = savePath + bookId + "\\" + bookId + "-1\\Comparison\\Comparison-1";
                extractPath = compPath1 + "\\" + userId;

                if (!Directory.Exists(extractPath))
                {
                    Directory.CreateDirectory(extractPath);
                }

                string fileName = "";
                string destFile = "";

                string targetPath = extractPath;

                if (Directory.Exists(sourcePath))
                {
                    string[] files = Directory.GetFiles(sourcePath);

                    foreach (string s in files)
                    {
                        fileName = Path.GetFileName(s);

                        if (fileName.Contains("-1.pdf") || (fileName.Contains("-1.rhyw")))
                        {
                            destFile = Path.Combine(targetPath, fileName);
                            if (!File.Exists(destFile)) File.Copy(s, destFile);

                            if (fileName.Contains("-1.pdf"))
                                Session["SrcPDF"] = Path.GetFileName(fileName);

                            else if (fileName.Contains("-1.rhyw"))
                                Session["rhywFile"] = Path.GetFileName(fileName);
                        }
                    }
                }

                string imagesFolderPath = Common.GetTaskFiles_ImagesPath();

                //if (File.Exists(imagesZipPath))
                //{
                //    string imgResourceFolderPath = extractPath + "\\Resources";

                //    byte[] imgBytes = ReadFile(imagesZipPath);

                //    if ((imgBytes != null) && (imgBytes.Length > 0))
                //    {
                //        using (ZipFile zip = ZipFile.Read(new MemoryStream(imgBytes)))
                //        {
                //            zip.ExtractAll(imgResourceFolderPath, ExtractExistingFileAction.DoNotOverwrite);
                //        }
                //    }
                //}

                string resourceFolderPath = extractPath + "\\Resources";

                //Create test directory and save test files in it.
                Directory.CreateDirectory(resourceFolderPath);

                if (Directory.Exists(imagesFolderPath))
                {
                    string[] files = Directory.GetFiles(imagesFolderPath);

                    foreach (string s in files)
                    {
                        fileName = Path.GetFileName(s);
                        destFile = Path.Combine(resourceFolderPath, fileName);
                        if (!File.Exists(destFile)) File.Copy(s, destFile);
                    }

                    imgResourceFolderPath = resourceFolderPath;
                }

                //int totalMistakes = GetTotalInsertedMistakes(bookId, userId);

                //if (totalMistakes > 0)
                //    objMyDBClass.SaveInsertedMistakesCount(bookId, userId, totalMistakes);

                //string tool = "BookMicro";
                //string task = "conversion";
                //string complexity = objMyDBClass.GetTaskComplexity(bookId);

                //if (string.IsNullOrEmpty(complexity)) return;

                //double taskCost = CalculateTaskCost(tool, task, complexity);

                //if (taskCost > 0)
                //    objMyDBClass.SaveTaskAmount(bookId, "ErrorDetection", userId, taskCost);
            }
        }

        #region WorkMeter Time Insertion

        public void InsertTaskInWorkMeter(string srcPdfPath, string bookId, string email)
        {
            try
            {
                //string currentUserId = Convert.ToString(Session["LoginId"]);
                //string message = checkAlreadyExists(bookId.Trim(), currentUserId, "27");

                //if (!message.Equals(string.Empty))
                //{

                //}

                string totalPages = "";
                string complexity = "";
                string[] result = null;

                if (bookId != "")
                {
                    if (bookId.Contains("-1"))
                        bookId = bookId.Replace("-1", "");

                    try
                    {
                        Service1Client client = new Service1Client();

                        var data = client.GetConversionFileDetailsForWebService(Convert.ToInt64(bookId.Trim()), "test");
                        client.Close();

                        result = data.Split('~');
                    }
                    catch (Exception ex)
                    {
                        result = new string[1];
                        result[0] = "";
                    }

                    //If book id not exists in its
                    if ((result[0] == ""))
                    {
                        //this.lblMessage.Text = "Book Id does not exists in ITS.";
                        complexity = "Simple";
                        PdfReader inputPdf = new PdfReader(srcPdfPath);
                        int pageCount = inputPdf.NumberOfPages;
                        totalPages = Convert.ToString(pageCount);
                    }
                    else
                    {
                        complexity = result[1].Split(':')[1];
                        totalPages = result[2].Split(':')[1];
                        bool scanned = Convert.ToBoolean(result[3].Split(':')[1] == "False" ? 0 : 1);
                        bool IsTextChangeIssue = Convert.ToBoolean(result[10].Split(':')[1] == "False" ? 0 : 1);
                        string Status = result[12].Split(':')[1];
                    }
                }

                SqlConnection objConnection = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
                objConnection.Open();

                string strMaxTaskId = "select max(TaskId)+1 as TaskId from tblTaskSheet";
                SqlCommand objCmdMax = new SqlCommand(strMaxTaskId, objConnection);
                objCmdMax.CommandType = CommandType.Text;
                SqlDataReader objRsMax = objCmdMax.ExecuteReader();
                string strTaskId = "1";
                if (objRsMax.Read())
                {
                    if (objRsMax["TaskId"].ToString() != "")
                    {
                        strTaskId = objRsMax["TaskId"].ToString();
                    }
                }
                objRsMax.Close();

                //Get fullname from bookmicro db
                SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString());
                con.Open();
                string queryGetName = "SELECT UserName FROM db_TaskOut_Final_1.dbo.[user] bmUserTable WHERE bmUserTable.Email='" + email.Trim() + "'";
                SqlCommand cmd = new SqlCommand(queryGetName, con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                string userName = "";

                if (dr.Read())
                {
                    if (dr["UserName"].ToString() != "")
                    {
                        userName = dr["UserName"].ToString();
                    }
                }
                dr.Close();
                con.Close();
                //end

                //Get userId from workmeter db after inserting user name
                string userId = Convert.ToString(getUserId_ByEmail(email, userName, "27"));
                //////////////////////////////////////

                /* To calculate time difference */
                string startTime = DateTime.Now.ToString();
                string strTask = "Insert into tblTaskSheet(TaskId,UserId,TaskDate) values(" + strTaskId + "," +
                                 userId + ",'" + System.DateTime.Now.ToShortDateString() +
                                 "')";
                SqlCommand objCmdTask = new SqlCommand(strTask, objConnection);
                objCmdTask.CommandType = CommandType.Text;
                int rowAffected = objCmdTask.ExecuteNonQuery();
                if (rowAffected != 0)
                {
                    string strTaskDetails =
                        "Insert into tblTaskDetails(TaskId,CatId,BookId,StartTime,EndTime,CalculatedTime,Comments,Current_Status,Target,Achived,Complexity,Tool_Used) values(" +
                        strTaskId + "," + "27" + ",'" + bookId + "','" + startTime +
                        "','','','" + "" + "','working','" + totalPages.Trim() + "','" +
                        totalPages.Trim() + "','" + complexity + "','" + "BookMicro" + "')";

                    SqlCommand objCmdDetails = new SqlCommand(strTaskDetails, objConnection);
                    objCmdDetails.CommandType = CommandType.Text;
                    int rowAffected1 = objCmdDetails.ExecuteNonQuery();
                    objCmdDetails = null;
                }
                objCmdMax = null;
                objCmdTask = null;
                objRsMax = null;
                objConnection.Close();

                TaskInfoManuplation(Convert.ToInt32(userId), Convert.ToInt32(bookId), 0, DateTime.Now.ToShortDateString(), 27, "insert");
                handleLog(strTaskId, "(checked In) ", userId);
            }

            catch (Exception ex)
            {
                //this.lblMessage.ForeColor = Color.Red;
                //this.lblMessage.Text = ex.Message + ex.Source + ex.InnerException;
            }

        }

        //private int getUserId_WorkMeter(string email)
        //{
        //    SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString());
        //    try
        //    {
        //        string strQuery = "GetWorkMeterUserId";
        //        SqlCommand cmd = new SqlCommand(strQuery, con);
        //        cmd.CommandType = CommandType.StoredProcedure;
        //        cmd.Parameters.AddWithValue("@Email", email.Trim());

        //        con.Open();
        //        SqlDataReader dr = cmd.ExecuteReader();
        //        int userId = 0;
        //        using (con)
        //        {
        //            if (dr.HasRows)
        //            {
        //                while (dr.Read())
        //                {
        //                    userId = Convert.ToInt32(dr["UserId"]);
        //                }
        //            }
        //        }

        //        return userId;
        //    }
        //    catch
        //    {
        //        return 0;
        //    }
        //    finally
        //    {
        //        if (con.State == ConnectionState.Open)
        //        {
        //            con.Close();
        //        }
        //    }
        //}

        private int getUserId_ByEmail(string email, string userName, string taskTypeId)
        {
            SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
            try
            {
                string strQuery = "GetWorkMeterUserId_ByEmail";
                SqlCommand cmd = new SqlCommand(strQuery, con);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Email", email.Trim());
                cmd.Parameters.AddWithValue("@FullName", userName.Trim());
                cmd.Parameters.AddWithValue("@StatusId", taskTypeId.Trim() == "27" ? "2" : "5");

                con.Open();
                SqlDataReader dr = cmd.ExecuteReader();
                int userId = 0;
                using (con)
                {
                    if (dr.HasRows)
                    {
                        while (dr.Read())
                        {
                            userId = Convert.ToInt32(dr["UserId"]);
                        }
                    }
                }

                return userId;
            }
            catch
            {
                return 0;
            }
            finally
            {
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }
        }

        private void TaskInfoManuplation(int userid, int bookid, double timespent, string date, int catid, string action)
        {
            SqlConnection objconection = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
            objconection.Open();

            string query = action.Equals("insert") ? "SP_INSERT_TASK_INFO" : "SP_UPDATE_TASK_INFO";
            SqlCommand objCmd = new SqlCommand(query, objconection);
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("@USERID", SqlDbType.Int);
            objCmd.Parameters["@USERID"].Value = userid;

            objCmd.Parameters.Add("@BOOKID", SqlDbType.Int);
            objCmd.Parameters["@BOOKID"].Value = bookid;
            objCmd.Parameters.Add("@TIMESPENT", SqlDbType.Float);
            objCmd.Parameters["@TIMESPENT"].Value = timespent;
            objCmd.Parameters.Add("@DATE", SqlDbType.VarChar);
            objCmd.Parameters["@DATE"].Value = date;
            objCmd.Parameters.Add("@CATID", SqlDbType.Int);
            objCmd.Parameters["@CATID"].Value = catid;
            objCmd.ExecuteNonQuery();
            objconection.Close();
        }

        public void handleLog(string TaskId, string message, string userId)
        {
            string LogDetail = getLogofDay(Convert.ToInt32(TaskId), DateTime.Now);
            if (LogDetail.Equals(""))
            {
                TaskLogManuplation(Convert.ToInt32(TaskId), DateTime.Now.ToShortTimeString() + message, 1,
                    Convert.ToInt32(userId), DateTime.Now.Date, "insert");
            }
            else
            {
                string[] splittedDetail = LogDetail.Split('?');
                string tasklog = splittedDetail[0] + " " + DateTime.Now.ToShortTimeString() + message;
                int countLog = Convert.ToInt32(splittedDetail[1]);
                TaskLogManuplation(Convert.ToInt32(TaskId), tasklog, countLog + 1, Convert.ToInt32(userId),
                    DateTime.Now.Date, "update");
            }
        }

        public string getLogofDay(int taskid, DateTime date)
        {
            SqlConnection objconection = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
            string query = @"select * from TBL_TASK_LOGS where TASK_ID=" + taskid +
                           " AND CONVERT(DATE,TBL_TASK_LOGS.DATE)='" + date.ToShortDateString() + "'";
            objconection.Open();

            SqlCommand objCmd = new SqlCommand(query, objconection);
            SqlDataReader objRs = objCmd.ExecuteReader();
            if (objRs.Read())
            {
                string role = objRs[2].ToString() + "?" + objRs[3].ToString();
                if (!role.Equals(""))
                {
                    objconection.Close();
                    return role;
                }
            }

            objconection.Close();
            return "";
        }

        private void TaskLogManuplation(int taskid, string taskLog, int countLog, int userID, DateTime date,
            string action)
        {
            SqlConnection objconection = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
            objconection.Open();

            string query = action.Equals("insert") ? "SP_INSERT_TASK_LOGS" : "SP_UPDATE_TASK_LOGS";
            SqlCommand objCmd = new SqlCommand(query, objconection);
            objCmd.CommandType = CommandType.StoredProcedure;

            objCmd.Parameters.Add("@TASK_ID", SqlDbType.Int);
            objCmd.Parameters["@TASK_ID"].Value = taskid;

            objCmd.Parameters.Add("@TASK_LOG", SqlDbType.VarChar);
            objCmd.Parameters["@TASK_LOG"].Value = taskLog;

            objCmd.Parameters.Add("@LOG_COUNT", SqlDbType.Int);
            objCmd.Parameters["@LOG_COUNT"].Value = countLog;

            objCmd.Parameters.Add("@USERID", SqlDbType.Int);
            objCmd.Parameters["@USERID"].Value = userID;

            objCmd.Parameters.Add("@DATE", SqlDbType.Date);
            objCmd.Parameters["@DATE"].Value = date.ToShortDateString();

            objCmd.ExecuteNonQuery();
            objconection.Close();
        }

        public string StopTask(bool complete, string status)
        {
            try
            {
                string bookId = Convert.ToString(Session["BookId"]);
                string email = Convert.ToString(Session["email"]);

                SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString());
                con.Open();
                string queryGetName = "SELECT UserName FROM db_TaskOut_Final_1.dbo.[user] bmUserTable WHERE bmUserTable.Email='" + email.Trim() + "'";
                SqlCommand cmd = new SqlCommand(queryGetName, con);
                cmd.CommandType = CommandType.Text;
                SqlDataReader dr = cmd.ExecuteReader();
                string userName = "";

                if (dr.Read())
                {
                    if (dr["UserName"].ToString() != "")
                    {
                        userName = dr["UserName"].ToString();
                    }
                }
                dr.Close();
                con.Close();

                string userId = Convert.ToString(getUserId_ByEmail(email, userName, "27"));
                string TaskId = getTaskId(bookId);

                if (TaskId == "")
                    return null;

                SqlConnection objConnection = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
                objConnection.Open();
                string strQueryStartDate = "select tblTaskDetails.CatId,tblTaskDetails.StartTime,tblTaskDetails.EndTime,tblTaskDetails.Achived,tblTaskDetails.Target,tblTaskDetails.Tool_Used,tblTaskDetails.CalculatedTime,tblTaskDetails.Complexity, tblTasksCategory.CatName from tblTaskDetails inner join dbo.tblTasksCategory on tblTaskDetails.CatId=dbo.tblTasksCategory.Catid where tblTaskDetails.TaskId=" + TaskId;
                SqlCommand objCmdMax = new SqlCommand(strQueryStartDate, objConnection);
                objCmdMax.CommandType = CommandType.Text;
                SqlDataReader objRsMax = objCmdMax.ExecuteReader();
                string startDate = "";
                string endTime = ""; //Addded by Aamir Ghafoor on 2013-12-28
                double calculatedTime = 0;
                string category = "";
                string complexity = "";
                string toolUsed = "";
                double expectedHours = 0;
                double expetedPages = 0;
                double result = 0;
                int processedPages = 0;
                int targetPages = 0;
                int catId = 0;

                if (objRsMax.Read())
                {
                    if (objRsMax["StartTime"].ToString() != "")
                    {
                        startDate = objRsMax["StartTime"].ToString();
                        endTime = objRsMax["EndTime"].ToString(); //Addded by Aamir Ghafoor on 2013-12-28
                        calculatedTime = Convert.ToDouble(objRsMax["CalculatedTime"].ToString().Equals("") ? null : objRsMax["CalculatedTime"].ToString());
                        category = objRsMax["CatName"].ToString();
                        complexity = objRsMax["Complexity"].ToString();
                        processedPages = Convert.ToInt32(objRsMax["Achived"].ToString().Equals("") ? null : objRsMax["Achived"].ToString());
                        targetPages = Convert.ToInt32(objRsMax["Target"].ToString().Equals("") ? null : objRsMax["Target"].ToString());
                        toolUsed = objRsMax["Tool_Used"].ToString();
                        catId = Convert.ToInt32(objRsMax["catId"]);
                    }
                }
                objRsMax.Close();
                objConnection.Close();

                DateTime endDate;
                bool check = false;

                endDate = DateTime.Now;

                string compTime = Convert.ToString(Session["TimeSpent_ComparisonTask"]);
                int hr = Convert.ToInt32(compTime.Split(':')[0]);
                int mn = Convert.ToInt32(compTime.Split(':')[1]);

                if (!calculatedTime.Equals(""))
                {
                    double hours = 0;
                    double minutes = 0;
                    if (status.Equals("pause"))
                    {
                        //hours = hours + endDate.Subtract(Convert.ToDateTime(startDate)).Hours - hr;
                        //minutes = minutes + endDate.Subtract(Convert.ToDateTime(startDate)).Minutes - mn;

                        hours = hr;
                        minutes = mn;
                    }
                    minutes = minutes + (hours * 60);
                    hours = .5 / 30 * minutes;
                    calculatedTime = calculatedTime + hours;
                }
                else
                {
                    //double hours = endDate.Subtract(Convert.ToDateTime(startDate)).Hours - hr;
                    //double minutes = endDate.Subtract(Convert.ToDateTime(startDate)).Minutes - mn;

                    double hours = hr;
                    double minutes = mn;
                    minutes = minutes + (hours * 60);
                    hours = .5 / 30 * minutes;
                    calculatedTime = hours;
                }
                string strQuery = "";

                string dbDetail = getTaskThoughput(category, complexity, toolUsed);
                double timeWorked = calculatedTime;

                double productivityHours = 0;

                if (dbDetail != "")
                {
                    string[] splitedoutput = dbDetail.Split(' ');
                    double expectedTime = Convert.ToDouble(splitedoutput[0]);
                    double expectedOut = Convert.ToDouble(splitedoutput[1]) / 60;

                    double bookunittime = (processedPages * Convert.ToDouble(splitedoutput[2])) / targetPages;
                    expectedHours = (processedPages * expectedOut) / 60 + bookunittime;

                    double timeUnitPerPage = 0;
                    double processePagesUnitTime = 0;
                    if (Convert.ToDouble(splitedoutput[2]) != 0)
                    {
                        timeUnitPerPage = Convert.ToDouble(splitedoutput[2]) / processedPages;
                        processePagesUnitTime = timeUnitPerPage * processedPages;
                    }
                    expetedPages = (timeWorked - processePagesUnitTime) * expectedTime;
                    result = processedPages - expetedPages;
                    productivityHours = expectedHours - timeWorked;
                }

                //if (endTime == "") //Addded by Aamir Ghafoor on 2013-12-28
                //{

                if (!complete)
                {
                    strQuery = "update tblTaskDetails set Expected_Pages='" + Math.Round(expetedPages, 2).ToString() +
                               "',Expected_Hours='" + Math.Round(expectedHours, 2).ToString() + "',Result='" +
                               Math.Round(result, 2).ToString() + "',Productivity_Hours='" +
                               Math.Round(productivityHours, 2).ToString() + "', EndTime='" + endDate +
                               "',CalculatedTime='" + Math.Round(calculatedTime, 2).ToString() + "'where TaskId=" +
                               TaskId;

                    //////If task is not completed then update its start time
                    ////objConnection.Open();
                    ////string updaterQuery = "update tblTaskDetails set StartTime='" + DateTime.Now + "',EndTime='',Current_Status='working' where TaskId=" + TaskId;
                    ////SqlCommand cmdUpdate = new SqlCommand(updaterQuery, objConnection);
                    ////int res = cmdUpdate.ExecuteNonQuery();
                    ////objConnection.Close();

                    //if (res > 0)
                    //{
                    //    //string temp = getLogofDay(Convert.ToInt32(TaskId), DateTime.Now);
                    //    handleLog(TaskId, "(checked In) ", userId);
                    //}
                    //end
                }
                else
                {
                    strQuery = "update tblTaskDetails set Expected_Pages='" + Math.Round(expetedPages, 2).ToString() +
                               "',Expected_Hours='" + Math.Round(expectedHours, 2).ToString() + "',Result='" +
                               Math.Round(result, 2).ToString() + "',Productivity_Hours='" +
                               Math.Round(productivityHours, 2).ToString() + "', EndTime='" + endDate +
                               "',CalculatedTime='" + Math.Round(calculatedTime, 2).ToString() + "',End_Date='" +
                               endDate + "',Current_Status='complete' where TaskId=" + TaskId;
                }

                objConnection.Open();
                SqlCommand objCmd = new SqlCommand(strQuery, objConnection);
                objCmd.CommandType = CommandType.Text;
                int rowAffected = objCmd.ExecuteNonQuery();
                objConnection.Close();

                if (rowAffected > 0)
                {
                    //this.lblMessage.ForeColor = Color.Blue;
                    //lblMessage.Style["color"] = "Blue";
                    //this.lblMessage.Text = "Task Successfully Updated.";
                }

                if (status.Equals("pause"))
                {
                    string comparisonTime = Convert.ToString(Session["TimeSpent_ComparisonTask"]);
                    int hr1 = Convert.ToInt32(comparisonTime.Split(':')[0]);
                    int mn1 = Convert.ToInt32(comparisonTime.Split(':')[1]);

                    if (!taskExistonSameDate(Convert.ToInt32(bookId), endDate.ToShortDateString(), Convert.ToInt32(userId), 27))
                    {
                        double hours = 0;
                        double minutes = 0;
                        double timeSpent;

                        //hours = endDate.Subtract(Convert.ToDateTime(startDate)).Hours - hr1;
                        //minutes = endDate.Subtract(Convert.ToDateTime(startDate)).Minutes - mn1;

                        hours = hr1;
                        minutes = mn1;

                        minutes = minutes + (hours * 60);
                        hours = .5 / 30 * minutes;
                        timeSpent = hours;
                        if ((userId != null) || (userId != ""))
                        {
                            TaskInfoManuplation(Convert.ToInt32(userId), Convert.ToInt32(bookId), timeSpent,
                                endDate.ToShortDateString(), 27, "insert");
                        }
                    }
                    else
                    {
                        string previoustime = getTimeofDay(Convert.ToInt32(bookId), endDate.ToShortDateString(),
                            Convert.ToInt32(userId), 27);
                        double timeSpent = Convert.ToDouble(previoustime.Equals("") ? null : previoustime);
                        double hours = 0;
                        double minutes = 0;

                        //hours = endDate.Subtract(Convert.ToDateTime(startDate)).Hours - hr1;
                        //minutes = endDate.Subtract(Convert.ToDateTime(startDate)).Minutes - mn1;

                        hours = hr1;
                        minutes = mn1;

                        minutes = minutes + (hours * 60);
                        hours = .5 / 30 * minutes;
                        timeSpent = timeSpent + hours;
                        if ((userId != null) || (userId != ""))
                        {
                            TaskInfoManuplation(Convert.ToInt32(userId), Convert.ToInt32(bookId), timeSpent,
                                endDate.ToShortDateString(), 27, "update");
                        }
                    }

                    //if (Convert.ToString(Session["BtnFinishClicked"]).Equals("1"))
                    //{
                    //    handleLog(TaskId.ToString(), "(checked out) ", userId);
                    //}

                    handleLog(TaskId.ToString(), "(checked out) ", userId);
                }
                //}
                else
                {
                    return null;
                }

                return Convert.ToString(timeWorked);
            }
            catch (Exception ex)
            {
                //this.lblMessage.ForeColor = Color.Red;
                //this.lblMessage.Text = ex.Message + ex.Source + ex.InnerException;

                return null;
            }
            finally
            {
            }
        }

        public string getTaskId(string bookId)
        {
            try
            {
                SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
                con.Open();

                string strMaxTaskId = "select TaskId from tblTaskDetails where BookId='" + bookId + "' and CatId = 27";
                SqlCommand objCmdMax = new SqlCommand(strMaxTaskId, con);
                objCmdMax.CommandType = CommandType.Text;
                SqlDataReader objRsMax = objCmdMax.ExecuteReader();
                string strTaskId = "";
                if (objRsMax.Read())
                {
                    if (objRsMax["TaskId"].ToString() != "")
                    {
                        strTaskId = Convert.ToString(objRsMax["TaskId"]);
                    }
                }
                objRsMax.Close();
                con.Close();

                return strTaskId;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        private string getTimeofDay(int pbookid, string date, int userid, int Catid)
        {
            SqlConnection objconection = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
            string query = @"select * from TBL_DATEWISE_INFO where Book_Id='" + pbookid + "' and DATE like '" + date + "' and USER_ID=" + userid + "and CATEGORY_ID=" + Catid;
            objconection.Open();

            SqlCommand objCmd = new SqlCommand(query, objconection);
            SqlDataReader objRs = objCmd.ExecuteReader();
            if (objRs.Read())
            {
                string role = objRs[3].ToString();
                //objCn.Close();
                if (!role.Equals(""))
                {
                    return role;
                }
            }
            objconection.Close();
            return "";

        }

        private bool taskExistonSameDate(int pbookid, string date, int userid, int Catid)
        {
            SqlConnection objconection = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
            string query = @"select * from TBL_DATEWISE_INFO where Book_Id='" + pbookid + "' and DATE like '" + date + "' and USER_ID=" + userid + "and CATEGORY_ID=" + Catid;
            objconection.Open();

            SqlCommand objCmd = new SqlCommand(query, objconection);
            SqlDataReader objRs = objCmd.ExecuteReader();
            if (objRs.Read())
            {
                string role = objRs[0].ToString();
                //objCn.Close();
                if (!role.Equals(""))
                {
                    return true;
                }
            }
            objconection.Close();
            return false;

        }

        private string getTaskThoughput(string category, string complexity, string toolUsed)
        {
            string query = @"select * from TBL_THROUGHPUT where task_name='" + category + "' and complexity='" + complexity + "' and TOOL_NAME='" + toolUsed + "'";
            SqlConnection objconection = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
            objconection.Open();
            SqlCommand objCmd = new SqlCommand(query, objconection);
            SqlDataReader objRs = objCmd.ExecuteReader();
            if (objRs.Read())
            {
                string expectedPages = objRs["EXPECTED_PER_HOUR"].ToString();
                string expectedTime = objRs["IN_SECONDS"].ToString();
                string bookUnitTime = objRs["BOOK_UNIT_TIME"].ToString();
                objconection.Close();
                objRs.Close();
                string result = expectedPages + " " + expectedTime + " " + bookUnitTime;
                return result;
            }
            else
            {
                objconection.Close();
                return "";
            }
        }

        #endregion

        public static byte[] ReadFile(string filePath)
        {
            byte[] buffer;
            FileStream fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            try
            {
                int length = (int)fileStream.Length; // get file length
                buffer = new byte[length]; // create buffer
                int count; // actual number of bytes read
                int sum = 0; // total number of bytes read

                // read until Read method returns 0 (end of the stream has been reached)
                while ((count = fileStream.Read(buffer, sum, length - sum)) > 0)
                    sum += count; // sum is a buffer offset for next reading
            }
            finally
            {
                fileStream.Close();
            }
            return buffer;
        }

        private void CompareSourcePDFWithRHYW(EO.Web.ProgressTaskEventArgs e)
        {
            string srcFilePath = "";
            string rhywFilePath = "";

            if (Convert.ToString(Session["ComparisonTask"]) != "")
            {
                if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
                {
                    srcFilePath = Common.GetTestFiles_SavingPath() + "/" + Convert.ToString(Session["SrcPDF"]);
                    rhywFilePath = Common.GetTestFiles_SavingPath() + "/" + Convert.ToString(Session["rhywFile"]);
                }
                else if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
                {
                    srcFilePath = Common.GetOnePageTestFiles_SavingPath() + "/" + Convert.ToString(Session["SrcPDF"]);
                    rhywFilePath = Common.GetOnePageTestFiles_SavingPath() + "/" + Convert.ToString(Session["rhywFile"]);
                }
                else if (Convert.ToString(Session["ComparisonTask"]).Equals("task"))
                {
                    srcFilePath = Common.GetTaskFiles_SavingPath() + Convert.ToString(Session["SrcPDF"]);
                    rhywFilePath = Common.GetTaskFiles_SavingPath() + Convert.ToString(Session["rhywFile"]);
                }
                else if (Convert.ToString(Session["ComparisonTask"]).Equals("comparisonEntryTest"))
                {
                    srcFilePath = Common.GetComparisonEntryTestFiles_SavingPath() + Convert.ToString(Session["SrcPDF"]);
                    rhywFilePath = Common.GetComparisonEntryTestFiles_SavingPath() + Convert.ToString(Session["rhywFile"]);
                }
                else if (Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedSampleTest"))
                {
                    srcFilePath = Common.GetComparisonEntryTestFiles_SavingPath() + Convert.ToString(Session["SrcPDF"]);
                    rhywFilePath = Common.GetComparisonEntryTestFiles_SavingPath() + Convert.ToString(Session["rhywFile"]);
                }
                else if (Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedStartTest"))
                {
                    srcFilePath = Common.GetComparisonEntryTestFiles_SavingPath() + Convert.ToString(Session["SrcPDF"]);
                    rhywFilePath = Common.GetComparisonEntryTestFiles_SavingPath() + Convert.ToString(Session["rhywFile"]);
                }
            }

            if (!File.Exists(srcFilePath))
                return;

            PdfReader srcPdf = new PdfReader(srcFilePath);
            int srcTotalPages = srcPdf.NumberOfPages;
            srcPdf.Close();

            Session["srcTotalPages"] = srcTotalPages;
            //static volatile int needsToBeThreadSafe = 0;
            int counter = 0;

            Common commObj = new Common();

            commObj.LoadXml(rhywFilePath);
            string xmlPath = rhywFilePath.Remove(rhywFilePath.Length - 5) + ".xml";
            commObj.xmlDoc.Save(xmlPath);
            Session["MainXMLFilePath"] = xmlPath;

            //////Resize all images according to width and height in src pdf
            ////PDFManipulation pdfMan = new PDFManipulation("");
            ////pdfMan.GetAllImages(imgResourceFolderPath);

            Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPathCoord"];
            //Set default values of variables used in xsls
            SetXls_Variables();

            if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
            {
                objMyDBClass.insertOnlineTestDetails("ErrorDetection", Convert.ToString(Session["UserId"]),
                    Convert.ToString(Session["BookId"]), "In Process", System.DateTime.Now.ToString());
            }
            else if (Convert.ToString(Session["ComparisonTask"]).Equals("task"))
            {
                ////////////////////////////////////////////////////////////////////////////

                //Find normalx and normalIndent from whole pdf file
                RHYWManipulation obj = new RHYWManipulation();

                //Get normal font from whole pdf
                string normalFont = obj.GetNormalFont(commObj.xmlDoc);
                Session["normalFont"] = normalFont;

                string level1 = obj.GetLevelFontSize("level1", commObj.xmlDoc);
                string level2 = obj.GetLevelFontSize("level2", commObj.xmlDoc);
                string level3 = obj.GetLevelFontSize("level3", commObj.xmlDoc);
                string level4 = obj.GetLevelFontSize("level4", commObj.xmlDoc);

                Session["level1"] = level1;
                Session["level2"] = level2;
                Session["level3"] = level3;
                Session["level4"] = level4;
                ////////////////

                double normalx_EvenPages = 0;
                double normalIndent_EvenPages = 0;
                obj.NormalAndIndentX_EvenPages(commObj.xmlDoc, ref normalx_EvenPages, ref normalIndent_EvenPages);

                double normalx_OddPages = 0;
                double normalIndent_OddPages = 0;
                obj.NormalAndIndentX_OddPages(commObj.xmlDoc, ref normalx_OddPages, ref normalIndent_OddPages);

                if (normalx_OddPages == normalx_EvenPages && normalIndent_OddPages == normalIndent_EvenPages)
                {
                    double normalx = 0;
                    double normalIndent = 0;

                    //obj.NormalAndIndentX_Old(Common.xmlDoc, ref normalx, ref normalIndent);
                    //normalx = 0;
                    //normalIndent = 0;

                    obj.NormalAndIndentX(commObj.xmlDoc, ref normalx, ref normalIndent);
                    Session["normalx"] = normalx;
                    Session["normalIndent"] = normalIndent;
                    Session["EvenOddLeftDifference"] = "false";
                }
                else
                {
                    Session["normalx_EvenPages"] = normalx_EvenPages;
                    Session["normalIndent_EvenPages"] = normalIndent_EvenPages;
                    Session["normalx_OddPages"] = normalx_OddPages;
                    Session["normalIndent_OddPages"] = normalIndent_OddPages;
                    Session["EvenOddLeftDifference"] = "true";
                }

                ///////////////
                //double normalx = 0;
                //double normalIndent = 0;

                ////obj.NormalAndIndentX_Old(GlobalVar.PBPDocument, ref normalx, ref normalIndent);

                //obj.NormalAndIndentX(Common.xmlDoc, ref normalx, ref normalIndent);

                //Session["normalx"] = normalx;
                //Session["normalIndent"] = normalIndent;

                //double normalx_EvenPages = 0;
                //double normalIndent_EvenPages = 0;

                //obj.NormalAndIndentX_EvenPages(Common.xmlDoc, ref normalx_EvenPages, ref normalIndent_EvenPages);

                //Session["normalx_EvenPages"] = normalx_EvenPages;
                //Session["normalIndent_EvenPages"] = normalIndent_EvenPages;

                ////////////////////////////////////////////////////////////////////////////
            }

            if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
            {
                StreamReader sr = new StreamReader(Convert.ToString(Session["MainXMLFilePath"]));
                string xmlFile = sr.ReadToEnd();
                sr.Close();

                XmlDocument xmlDocOrigXml = new XmlDocument();
                xmlDocOrigXml.LoadXml(xmlFile);

                int totalMistakes = xmlDocOrigXml.SelectNodes(@"//ln[@PDFmistake!='']").Count;
                Session["ComparisonTestTotalMistakes"] = totalMistakes;

                //string ipAddress = GetIPAddress();

                //MyDBClass objMyDBClass = new MyDBClass();
                //string querySel = "Select distinct(Name) from Archive where Email='" + Convert.ToString(Session["ComparisonTestUser_Email"]) + "'";
                //DataSet dsBookInfo = objMyDBClass.GetDataSet(querySel);
                //string userName = dsBookInfo.Tables[0].Rows[0]["Name"].ToString();

                //SaveComparisonTest(userName, Convert.ToString(Session["ComparisonTestUser_Email"]), Convert.ToString(Session["SrcPDF"]), ipAddress, totalMistakes, "ErrorDetection");
            }
            else if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
            {
                StreamReader sr = new StreamReader(Convert.ToString(Session["MainXMLFilePath"]));
                string xmlFile = sr.ReadToEnd();
                sr.Close();

                XmlDocument xmlDocOrigXml = new XmlDocument();
                xmlDocOrigXml.LoadXml(xmlFile);

                int totalMistakes = xmlDocOrigXml.SelectNodes(@"//ln[@PDFmistake!='']").Count;
                Session["ComparisonTestTotalMistakes"] = totalMistakes;
            }

            //Serial Execution 
            for (int i = 1; i <= srcTotalPages; i++)
            {
                string srcPdfPagePath = ExtractPage(i);
                string prdPdfPagePath = GetProducePdf(i);

                counter++;

                #region For Progress Bar

                float percnt = ((float)(counter / (float)srcTotalPages) * 100f);
                e.UpdateProgress(Convert.ToInt32(percnt), "Currently Processing: " + counter + " of " + srcTotalPages + " Pages");

                #endregion
            }

            //Parallel Execution
            //for (int i = 1; i < srcTotalPages + 1; i++)
            //{
            //    string srcPdfPagePath = ExtractPage(i);
            //    string prdPdfPagePath = GetProducePdf(i);
            //}
            //System.Threading.Tasks.Parallel.For(1, srcTotalPages + 1, i =>
            //{
            //    //var tt = Convert.ToString(Session["ComparisonTask"]);
            //    //var tt1 = Convert.ToString(Session["ComparisonTestUser_Email"]);

            //    string srcPdfPagePath = ExtractPage(i);
            //    string prdPdfPagePath = GetProducePdf(i);

            //    //needsToBeThreadSafe++;
            //    //counter++;

            //    //web_ComparisonPreProcess.IncrementCounter();

            //    #region For Progress Bar

            //    //float percnt = ((float)(i / (float)srcTotalPages) * 100f);
            //    //e.UpdateProgress(Convert.ToInt32(percnt), "Currently Processing: " + IncrementCounter() + " of " + srcTotalPages + " Pages");
            //    //Increment();

            //    //float percnt = ((float)IncrementCounter());
            //    //e.UpdateProgress(Convert.ToInt32(Value), "");

            //    #endregion
            //}
            //    );
            //for (int i = 1; i <= srcTotalPages; i++)
            //{
            //    float percnt = ((float)(i / (float)srcTotalPages) * 100f);
            //    e.UpdateProgress(Convert.ToInt32(percnt),
            //        "Currently Processing: " + i + " of " + srcTotalPages + " Pages");
            //}

            string comparisonTask = Convert.ToString(Session["ComparisonTask"]);

            if (comparisonTask != "")
            {
                if (comparisonTask.Equals("task"))
                {
                    string bookId = Convert.ToString(Session["BookId"]);
                    string userId = Convert.ToString(Session["LoginId"]);
                    string email = Convert.ToString(Session["email"]);

                    MyDBClass db = new MyDBClass();
                    if ((!string.IsNullOrEmpty(bookId)) && (!string.IsNullOrEmpty(userId)))
                    {
                        int totalInjectedMistakes = GetTotalInjectedMistakes(bookId, userId);

                        //if (totalInjectedMistakes > 0)
                        //    objMyDBClass.SaveInsertedMistakesCount(bookId, userId, totalInjectedMistakes);

                        string tool = "BookMicro";
                        string task = "conversion";
                        string complexity = objMyDBClass.GetTaskComplexity(bookId);

                        if (string.IsNullOrEmpty(complexity)) return;

                        double taskCost = CalculateTaskCost(tool, task, complexity, srcTotalPages);

                        if (taskCost > 0)
                        {
                            string bid = GetBidFromBookId(bookId);
                            if (!string.IsNullOrEmpty(bid)) objMyDBClass.SaveTaskAmount(bid, "ErrorDetection", userId, taskCost);
                        }

                        //0 status is initial status, 1 means competed
                        db.InsertQaTask(bookId + "-1", userId, totalInjectedMistakes, "0");
                        InsertTaskInWorkMeter(srcFilePath, bookId, email); //commented by aamir 2016-03-10
                    }
                }
            }

            if (srcTotalPages > 0)
                Session["MainCurrPage"] = "1";
        }

        private string GetBidFromBookId(string bookId)
        {
            SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString());
            con.Open();
            string queryGetName = "select a.BID from Activity a inner join Book b on a.BID = b.BID WHERE b.MainBook='" + bookId.Trim() + "'";
            SqlCommand cmd = new SqlCommand(queryGetName, con);
            cmd.CommandType = CommandType.Text;
            SqlDataReader dr = cmd.ExecuteReader();
            string bid = "";

            if (dr.Read())
            {
                if (dr["BID"].ToString() != "")
                {
                    bid = dr["BID"].ToString();
                }
            }
            dr.Close();
            con.Close();

            return bid;
        }

        public void SetXls_Variables()
        {
            string xslPath = "";
            string xslCoordPath = "";

            if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]) != "")
            {
                if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("test"))
                {
                    xslCoordPath = Common.GetXSLCoordDirectoryPath();
                }
                else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("onepagetest"))
                {
                    xslCoordPath = Common.GetXSLCoordDirectoryPath();
                }
                else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("comparisonEntryTest") ||
                    Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("CompUpgradedSampleTest") ||
                    Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("CompUpgradedStartTest"))
                {
                    xslCoordPath = Common.GetXSLDirectoryPath_StartTest();
                }
                else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
                {
                    xslCoordPath = Common.GetXSLCoordDirectoryPath();
                }
            }

            string marginTop = "xsl:variable[@name=\"margin-top\"]";
            string marginBottom = "xsl:variable[@name=\"margin-bottom\"]";
            string marginRight = "xsl:variable[@name=\"margin-right\"]";
            string marginLeft = "xsl:variable[@name=\"margin-left\"]";
            string pageWidth = "xsl:variable[@name=\"doc-page-width\"]";
            string pageHeight = "xsl:variable[@name=\"doc-page-height\"]";
            //string tableTopMargin = "xsl:variable[@name=\"tableTopMargin\"]";
            //string topPageMargin = "xsl:variable[@name=\"topPageMargin\"]";
            //string imgTopMargin = "xsl:variable[@name=\"imageMarginTop\"]";
            double top = 0;
            double bottom = 0;
            double right = 0;
            double left = 0;
            double width = 0;
            double height = 0;
            double tableMargin = 0;
            //double topMargin = 0;
            //double imgTop = 0;
            int page = 1;

            string mainXml = Convert.ToString(Session["MainXMLFilePath"]).Replace(".xml", ".pdf");
            PdfReader pdfReader = new PdfReader(mainXml);

            var pdfPage = pdfReader.GetPageSize(page);

            //1 Inch = 72 Points [Postscript]

            //1 Point = 0.01388888889 Inch

            //1 PostScript point = 0.352777778 millimeters
            //units in mm
            width = pdfPage.Width * 0.352777778;
            height = pdfPage.Height * 0.352777778;

            iTextSharp.text.Rectangle cropbox = pdfReader.GetCropBox(page);
            var box = pdfReader.GetPageSizeWithRotation(page);

            top = Math.Round((box.Top - cropbox.Top) * 0.352777778, 3);
            bottom = Math.Round(cropbox.Bottom * 0.352777778, 3);
            right = Math.Round((box.Right - cropbox.Right) * 0.352777778, 3);
            left = Math.Round(cropbox.Left * 0.352777778, 3);

            XmlDocument doc = new XmlDocument();
            doc.Load(xslCoordPath);
            XmlNode root = doc.DocumentElement;
            XmlNamespaceManager nsmgr = new XmlNamespaceManager(doc.NameTable);
            nsmgr.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

            root.SelectSingleNode(marginTop, nsmgr).Attributes["select"].Value = Convert.ToString(top);
            root.SelectSingleNode(marginBottom, nsmgr).Attributes["select"].Value = Convert.ToString(bottom);
            root.SelectSingleNode(marginRight, nsmgr).Attributes["select"].Value = Convert.ToString(right);
            root.SelectSingleNode(marginLeft, nsmgr).Attributes["select"].Value = Convert.ToString(left);
            root.SelectSingleNode(pageWidth, nsmgr).Attributes["select"].Value = Convert.ToString(width);
            root.SelectSingleNode(pageHeight, nsmgr).Attributes["select"].Value = Convert.ToString(height);
            //root.SelectSingleNode(tableTopMargin, nsmgr).Attributes["select"].Value = Convert.ToString(tableMargin);
            //root.SelectSingleNode(topPageMargin, nsmgr).Attributes["select"].Value = Convert.ToString(topMargin);
            //root.SelectSingleNode(imgTopMargin, nsmgr).Attributes["select"].Value = Convert.ToString(imgTop);

            doc.Save(xslCoordPath);
        }

        public string ExtractPage(int pageNum)
        {
            string inputFilePath = "";
            string outputFile = "";

            if (Convert.ToString(Session["ComparisonTask"]) != "")
            {
                if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
                {
                    inputFilePath = Common.GetDirectoryPath() + "\\Tests\\" +
                                    Convert.ToString(Session["ComparisonTestUser_Email"]) +
                                    "/ComparisonTests/" +
                                    Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"])) + "/" +
                                    Convert.ToString(Session["SrcPDF"]);

                    outputFile = Common.GetDirectoryPath() + "\\Tests\\" +
                                 Convert.ToString(Session["ComparisonTestUser_Email"]) +
                                 "/ComparisonTests/" +
                                 Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"])) + "/" +
                                 "/" + pageNum + ".pdf";
                }
                else if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest") )
                {
                    inputFilePath = Common.GetDirectoryPath() + "\\Tests\\" +
                                    Convert.ToString(Session["ComparisonTestUser_Email"]) +
                                    "/ComparisonTests/" +
                                    Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"])) + "/" +
                                    Convert.ToString(Session["SrcPDF"]);

                    outputFile = Common.GetDirectoryPath() + "\\Tests\\" +
                                 Convert.ToString(Session["ComparisonTestUser_Email"]) +
                                 "/ComparisonTests/" +
                                 Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"])) + "/" +
                                 "/" + pageNum + ".pdf";
                }
                else if (Convert.ToString(Session["ComparisonTask"]).Equals("task"))
                {
                    inputFilePath = Common.GetDirectoryPath() + Convert.ToString(Session["BookId"]) + "/" +
                                    Convert.ToString(Session["BookId"]) +
                                    "-1/Comparison/Comparison-" + Convert.ToString(Session["comparisonType"]) + "/" +
                                    Convert.ToString(Session["userId"]) +
                                    "/" + Convert.ToString(Session["SrcPDF"]);

                    outputFile = Common.GetDirectoryPath() + Convert.ToString(Session["BookId"]) + "/" +
                                 Convert.ToString(Session["BookId"]) +
                                 "-1/Comparison/Comparison-" + Convert.ToString(Session["comparisonType"]) + "/" +
                                 Convert.ToString(Session["userId"]) +
                                 "/" + "\\" + pageNum + ".pdf";

                }

                else if (Convert.ToString(Session["ComparisonTask"]).Equals("comparisonEntryTest") ||
                    Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedSampleTest") ||
                    Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedStartTest"))
                {
                    inputFilePath = Common.GetDirectoryPath() + "\\Tests\\" + Convert.ToString(Session["ComparisonTestUser_Email"]) +
                                   "/ComparisonTests/" + Convert.ToString(Session["TestName"]) + "/" +
                                    Convert.ToString(Session["TestName"]) + "-1/Comparison/" + Convert.ToString(Session["SrcPDF"]);

                    outputFile = Common.GetDirectoryPath() + "\\Tests\\" + Convert.ToString(Session["ComparisonTestUser_Email"]) +
                                 "/ComparisonTests/" + Convert.ToString(Session["TestName"]) + "/" +
                                    Convert.ToString(Session["TestName"]) + "-1/Comparison/" + pageNum + ".pdf";

                }
            }

            if (!File.Exists(outputFile))
            {
                ExtractPages(inputFilePath, outputFile, pageNum, pageNum);
                Createtetml(outputFile);
            }
            return outputFile;
        }

        //private void ExtractPages(string inputFile, string outputFile, int start, int end)
        //{
        //    // get input document
        //    PdfReader inputPdf = new PdfReader(inputFile);
        //    // retrieve the total number of pages
        //    int pageCount = inputPdf.NumberOfPages;
        //    if (end < start || end > pageCount)
        //    {
        //        end = pageCount;
        //    }

        //    //var pgSize = new iTextSharp.text.Rectangle(myWidth, myHeight);
        //    //var doc = new iTextSharp.text.Document(pgSize, leftMargin, rightMargin, topMargin, bottomMargin);

        //    // load the input document
        //    Document inputDoc = new Document(inputPdf.GetPageSizeWithRotation(1));

        //    // create the filestream
        //    using (FileStream fs = new FileStream(outputFile, FileMode.Create))
        //    {
        //        // create the output writer
        //        PdfWriter outputWriter = PdfWriter.GetInstance(inputDoc, fs);
        //        inputDoc.Open();
        //        PdfContentByte cb1 = outputWriter.DirectContent;

        //        // copy pages from input to output document
        //        for (int i = start; i <= end; i++)
        //        {
        //            inputDoc.SetPageSize(inputPdf.GetPageSizeWithRotation(i));
        //            inputDoc.NewPage();

        //            PdfImportedPage page = outputWriter.GetImportedPage(inputPdf, i);
        //            int rotation = inputPdf.GetPageRotation(i);

        //            if (rotation == 90 || rotation == 270)
        //            {
        //                cb1.AddTemplate(page, 0, -1f, 1f, 0, 0, inputPdf.GetPageSizeWithRotation(i).Height);
        //            }
        //            else
        //            {
        //                cb1.AddTemplate(page, 1f, 0, 0, 1f, 0, 0);
        //            }
        //        }
        //        inputDoc.Close();
        //    }
        //}

        private void ExtractPages(string inputFile, string outputFile, int start, int end)
        {
            PdfReader reader = null;
            Document document = null;
            PdfCopy pdfCopyProvider = null;
            PdfImportedPage importedPage = null;

            try
            {
                using (Stream outPutStream = new FileStream(outputFile, FileMode.Create))
                {
                    // Intialize a new PdfReader instance with the contents of the source Pdf file:
                    reader = new PdfReader(inputFile);

                    // Capture the correct size and orientation for the page:
                    document = new Document(reader.GetPageSizeWithRotation(1));

                    // Initialize an instance of the PdfCopyClass with the source 
                    // document and an output file stream:
                    //pdfCopyProvider = new PdfCopy(document, new FileStream(outputFile, FileMode.Create));

                    pdfCopyProvider = new PdfCopy(document, outPutStream);
                    document.Open();

                    // Extract the desired page number:
                    importedPage = pdfCopyProvider.GetImportedPage(reader, start);
                    pdfCopyProvider.AddPage(importedPage);

                    document.Close();
                    pdfCopyProvider.Close();
                    importedPage.ClosePath();
                    reader.Close();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string Createtetml(string filePath)
        {
            if (filePath == null)
                return null;

            if (File.Exists(filePath.Replace("pdf", "tetml")))
            {
                File.Delete(filePath.Replace("pdf", "tetml"));
            }

            //WriteLog("Generating tetml File............ Please Wait");
            //WriteLog("This Will Take Time Depending upon PDF Pages");
            string DirectoryPath = Directory.GetParent(filePath).ToString();
            string wordTETMLPath = DirectoryPath + "\\" + Path.GetFileNameWithoutExtension(filePath) + ".tetml";
            //tetFile = XmlFile;
            //string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks}\" -o \"" + XmlFile + "\" \"" + PDFFilePath + "\"";
            string strParameter =
                "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks} clippingarea={cropbox}\" -o \"" +
                wordTETMLPath + "\" \"" + filePath + "\"";
            //string Img_Conversion_bat = @"D:\work\tet.exe";

            //string email = "";
            //if (Convert.ToString(HttpContext.Current.Session["Email"]) != "")
            //{
            //    email = Convert.ToString(HttpContext.Current.Session["Email"]);
            //}
            //else
            //{
            //    return "";
            //}

            string Img_Conversion_bat = System.Configuration.ConfigurationSettings.AppSettings["TetPath"].ToString();

            Process pConvertTetml = new Process();
            pConvertTetml.StartInfo.UseShellExecute = false;
            pConvertTetml.StartInfo.RedirectStandardError = true;
            pConvertTetml.StartInfo.RedirectStandardOutput = true;
            pConvertTetml.StartInfo.CreateNoWindow = true;
            pConvertTetml.StartInfo.Arguments = strParameter;
            pConvertTetml.StartInfo.FileName = Img_Conversion_bat;
            pConvertTetml.Start();
            pConvertTetml.WaitForExit();
            return wordTETMLPath;
        }

        public string GetProducePdf(int PageNum)
        {
            string pageXMLSavedPath = "";

            XmlDocument pageXML = GetPageXmlDoc(PageNum.ToString());

            //pageXMLSavedPath = Common.GetDirectoryPath() + Convert.ToString(Session["BookId"]) + "/" + Convert.ToString(Session["BookId"]) +
            //       "-1/Comparison/Comparison-" + Convert.ToString(Session["comparisonType"]) + "/" + Convert.ToString(Session["userId"]) + "/" + "\\Produced_" + PageNum + ".xml";

            //if (Convert.ToString(Session["ComparisonTask"]) != "")
            //{
            //    if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
            //    {
            //        inputFilePath = Common.GetDirectoryPath() + "\\Tests\\" + Convert.ToString(Session["ComparisonTestUser_Email"]) +
            //                        "/ComparisonTests/" + Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"])) + "/" +
            //                        Convert.ToString(Session["SrcPDF"]);

            //        outputFile = Common.GetDirectoryPath() + "\\Tests\\" + Convert.ToString(Session["ComparisonTestUser_Email"]) +
            //                        "/ComparisonTests/" + Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"])) + "/" +
            //                        "/" + pageNum + ".pdf";
            //    }
            //    else if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
            //    {
            //        //inputFilePath = Common.GetOnePageTestFiles_SavingPath() + Convert.ToString(Session["SrcPDF"]);
            //        //outputFile = Common.GetOnePageTestFiles_SavingPath() + Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"])) + "/" + pageNum + ".pdf"; 
            //    }
            //    else if (Convert.ToString(Session["ComparisonTask"]).Equals("task"))
            //    {
            //        inputFilePath = Common.GetDirectoryPath() + Convert.ToString(Session["BookId"]) + "/" + Convert.ToString(Session["BookId"]) +
            //                        "-1/Comparison/Comparison-" + Convert.ToString(Session["comparisonType"]) + "/" + Convert.ToString(Session["userId"]) +
            //                        "/" + Convert.ToString(Session["SrcPDF"]);

            //        outputFile = Common.GetDirectoryPath() + Convert.ToString(Session["BookId"]) + "/" + Convert.ToString(Session["BookId"]) +
            //                     "-1/Comparison/Comparison-" + Convert.ToString(Session["comparisonType"]) + "/" + Convert.ToString(Session["userId"]) +
            //                     "/" + "\\" + pageNum + ".pdf";

            //    }
            //}

            if (Convert.ToString(Session["ComparisonTask"]) != "")
            {
                if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
                {
                    pageXMLSavedPath = Common.GetDirectoryPath() + "\\Tests\\" +
                                       Convert.ToString(Session["ComparisonTestUser_Email"]) +
                                       "/ComparisonTests/" +
                                       Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"])) + "/" +
                                       "\\Produced_" + PageNum + ".xml";
                }
                else if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
                {
                    pageXMLSavedPath = Common.GetDirectoryPath() + "\\Tests\\" +
                                       Convert.ToString(Session["ComparisonTestUser_Email"]) +
                                       "/ComparisonTests/" +
                                       Path.GetFileNameWithoutExtension(Convert.ToString(Session["SrcPDF"])) + "/" +
                                       "\\Produced_" + PageNum + ".xml";
                }
                else if (Convert.ToString(Session["ComparisonTask"]).Equals("task"))
                {
                    //pageXMLSavedPath = @"C:\Users\Aamir\Desktop\New folder\output" + "\\Produced_" + PageNum + ".xml";

                    pageXMLSavedPath = Common.GetDirectoryPath() + Convert.ToString(Session["BookId"]) + "/" +
                                       Convert.ToString(Session["BookId"]) +
                                       "-1/Comparison/Comparison-" + Convert.ToString(Session["comparisonType"]) + "/" +
                                       Convert.ToString(Session["userId"]) +
                                       "/" + "\\Produced_" + PageNum + ".xml";
                }

                if (Convert.ToString(Session["ComparisonTask"]).Equals("comparisonEntryTest") ||
                    Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedSampleTest") ||
                    Convert.ToString(Session["ComparisonTask"]).Equals("CompUpgradedStartTest"))
                {
                    pageXMLSavedPath = Common.GetDirectoryPath() + "\\Tests\\" + Convert.ToString(Session["ComparisonTestUser_Email"]) +
                                       "/ComparisonTests/" + Convert.ToString(Session["TestName"]) + "/" +
                                       Convert.ToString(Session["TestName"]) + "-1/Comparison/" +
                                       "\\Produced_" + PageNum + ".xml";
                }
            }

            pageXML.Save(pageXMLSavedPath);

            string prodFilePath = pageXMLSavedPath.TrimEnd(".xml".ToCharArray()) + ".pdf";
            //try
            //{
            //    string result = GenearatePDFPreview(pageXMLSavedPath, prodFilePath);

            //    if (!result.Equals("Successfull"))
            //    {
            //        prodFilePath = "";
            //    }

            //    else if (result.Equals("Successfull"))
            //    {
            //        Createtetml(prodFilePath);
            //    }
            //}
            //finally
            //{
            //    //imgValidator.Dispose();
            //}
            return prodFilePath;
        }

        public string GenearatePDFPreview(string srcXMLFile, string targetPDFPath)
        {
            string xslsPath = System.Configuration.ConfigurationManager.AppSettings["XSLPath"];
            //return ShowPdfPreview(srcXMLFile, targetPDFPath, "c:\\XEP\\xep.bat", @"C:\XSL\XSLS\PBPBook.xsl");
            return ShowPdfPreview(srcXMLFile, targetPDFPath, "c:\\XEP\\xep.bat", xslsPath);
        }

        private string ShowPdfPreview(string xmlfile, string PdfFile, string xepfile, string xslfile)
        {
            string retMessage = "";
            try
            {
                if (Convert.ToString(Session["ComparisonTask"]) != "")
                {
                    if (Convert.ToString(Session["ComparisonTask"]).Equals("test"))
                    {
                        Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPathCoord"];
                    }
                    else if (Convert.ToString(Session["ComparisonTask"]).Equals("onepagetest"))
                    {
                        Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPathCoord"];
                    }
                    else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("task"))
                    {
                        Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPathCoord"];
                    }
                    else if (Convert.ToString(HttpContext.Current.Session["ComparisonTask"]).Equals("comparisonEntryTest"))
                    {
                        Session["setDefaultXSL"] = ConfigurationManager.AppSettings["XSLPathCoord"];
                    }
                }

                if (Convert.ToString(Session["setDefaultXSL"]) != "")
                {
                    xslfile = Convert.ToString(Session["setDefaultXSL"]);
                }

                string cmdStr = "-xml " + "\"" + xmlfile + "\"" + " -xsl " + "\"" + xslfile + "\"" + " -out " + "\"" +
                                PdfFile + "\"";

                if (File.Exists(PdfFile))
                {
                    File.Delete(PdfFile);
                }
                Process pPdfPreview = new Process();
                Process pPdfPreviewInPDF = new Process();

                //tells operating system not to use a shell;
                pPdfPreview.StartInfo.UseShellExecute = false;
                //allow me to capture stdout, i.e. results
                pPdfPreview.StartInfo.RedirectStandardOutput = true;
                //#my command arguments, i.e. what site to ping
                pPdfPreview.StartInfo.Arguments = cmdStr;
                //#the command to invoke under MSDOS
                pPdfPreview.StartInfo.FileName = xepfile;
                //#do not show MSDOS window
                pPdfPreview.StartInfo.CreateNoWindow = true;
                //#do it!
                bool bStarted = pPdfPreview.Start();
                while (!pPdfPreview.HasExited)
                {
                    //Application.DoEvents();
                    //System.Diagnostics.Debug.Write(".");
                }
                pPdfPreview.WaitForExit();
                // Check if PDF size is greater than zero
                // IF-ELSE Block Added
                FileInfo PdfFileInfo = new FileInfo(PdfFile);
                if (File.Exists(PdfFile) && PdfFileInfo.Length > 0)
                {
                    retMessage = "Successfull";
                }
                else
                {
                    retMessage = "Command= " + cmdStr + " File Name=" + PdfFile + "  No PDF File found";
                }
            }
            catch (Exception ex)
            {
                retMessage = ex.Message;
            }
            return retMessage;
        }

        public XmlDocument GetPageXmlDoc(string num)
        {
            //XmlNodeList pageContents = GlobalVar.PBPDocument.SelectNodes("//*[@page=\"" + num + "\"]/..");
            XmlNodeList pageContents = xmlDoc.SelectNodes("//*[@page=\"" + num + "\"]/ancestor::upara|" +
                                                          "//*[@page=\"" + num + "\"]/ancestor::spara|" +
                                                          "//*[@page=\"" + num + "\"]/ancestor::npara|" +
                                                          "//*[@page=\"" + num + "\"]/ancestor::image|" +
                                                          "//*[@page=\"" + num + "\"]/ancestor::section-title|" +
                                                          "//*[@page=\"" + num + "\"]/ancestor::prefix|" +
                                                          "//*[@page=\"" + num + "\"]/ancestor::table"
                );
            int counter = 0;

            XmlDocument tmpPageXml = new XmlDocument();
            if (pageContents.Count == 0)
            {
                return PreProcess(null);
            }
            else
            {
                XmlNode rootElement = tmpPageXml.CreateElement("body");
                tmpPageXml.AppendChild(rootElement);
                XmlNode validNode = null;
                XmlNode validNode_Parent = null;

                foreach (XmlNode xmlNode in pageContents)
                {
                    counter = 0;

                    if ((xmlNode.Name.Equals("section-title")) && (xmlNode.ParentNode.Name.Equals("head")))
                    {
                        validNode_Parent = xmlDoc.CreateElement(xmlNode.ParentNode.ParentNode.Name);

                        foreach (XmlAttribute att in xmlNode.ParentNode.ParentNode.Attributes)
                        {
                            ((XmlElement)validNode_Parent).SetAttribute(att.Name, att.Value);
                        }
                    }
                    else
                    {
                        validNode_Parent = null;
                    }

                    validNode = xmlDoc.CreateElement(xmlNode.Name);
                    foreach (XmlAttribute att in xmlNode.Attributes)
                    {
                        ((XmlElement)validNode).SetAttribute(att.Name, att.Value);
                    }

                    XmlNodeList contentChilds = xmlNode.ChildNodes;
                    foreach (XmlNode content in contentChilds)
                    {
                        if (xmlNode.Name.Equals("spara"))
                        {
                            foreach (XmlNode ch in content)
                            {
                                if (counter < 1)
                                {
                                    //Create a new attribute in first line of every spara for changing its color to blue
                                    XmlAttribute attr = xmlDoc.CreateAttribute("colorChange");
                                    attr.Value = "1";
                                    ch.Attributes.Append(attr);
                                    counter++;
                                }

                                if (ch.Attributes["page"] != null && ch.Attributes["page"].Value == num)
                                {
                                    validNode.InnerXml += content.OuterXml;
                                }
                            }
                        }

                        else if (xmlNode.Name.Equals("image"))
                        {
                            if (content.Name.Equals("caption"))
                            {
                                if (content.FirstChild.Attributes["page"].Value != null &&
                                    content.FirstChild.Attributes["page"].Value == num)
                                {
                                    validNode.InnerXml += content.OuterXml;
                                }
                            }
                            //If there is no caption tag in image
                            else
                            {
                                if (content.Attributes["page"] != null && content.Attributes["page"].Value == num)
                                {
                                    validNode.InnerXml += content.OuterXml;
                                }
                            }
                        }

                        else
                        {
                            if (content.Attributes["page"] != null && content.Attributes["page"].Value == num)
                            {
                                validNode.InnerXml += content.OuterXml;
                            }
                        }
                    }

                    if (validNode_Parent != null)
                        rootElement.InnerXml += validNode_Parent.OuterXml + validNode.OuterXml;

                    else
                        rootElement.InnerXml += validNode.OuterXml;
                }
                return PreProcess(tmpPageXml);
            }
        }

        public XmlDocument PreProcess(XmlDocument xmlDoc)
        {
            XmlDocument newXmlDoc = new XmlDocument();
            XmlNode rootNode = newXmlDoc.CreateElement("pbp-book");
            newXmlDoc.AppendChild(rootNode);

            string necessaryElements =
                "<pbp-meta><pbp-info tag-operator=\"pakistan\" tag-date=\"2009-01-01\" file-name=\"XXXXXX\" schema-name=\"PBPBook_P02.xsd\" publication-status=\"NOT FOR PUBLICATION\" book-type=\"OTHER\" copyright-status=\"IN COPYRIGHT\" book-title=\"XXXXX\" schema-rev=\"p02\"/><doc-track></doc-track><bookrep-info><author-id>1</author-id><book-summary></book-summary><author-info></author-info></bookrep-info></pbp-meta><pbp-front><cover><image-model><front image-url=\"\"/><spine image-url=\"\"/><back image-url=\"\"/></image-model></cover><BISAC><BISAC-item><BISAC-text></BISAC-text><BISAC-code></BISAC-code></BISAC-item></BISAC><ISBN></ISBN><title-block><book-title><main-title>XX</main-title><running-header></running-header></book-title><author><full-name>FullName</full-name><prenominal /><first-name>FirstName</first-name><last-name>XX</last-name></author></title-block><book-notices></book-notices></pbp-front>";
            rootNode.InnerXml = necessaryElements;
            string innerXML = "";
            if (xmlDoc == null)
            {
                innerXML =
                    "<body><upara><ln coord=\"237.65:568.72:388.55:586.72\" page=\"1\" height=\"792\" left=\"237.65\" top=\"568.72\" font=\"BemboStd\" fontsize=\"18\" error=\"0\" ispreviewpassed=\"true\" isUserSigned=\"0\" isEditted=\"false\">Sorry! The page is blank</ln></upara></body>";
            }
            else
            {
                innerXML = xmlDoc.InnerXml;
            }
            rootNode.InnerXml += "<pbp-body>" + innerXML + "</pbp-body>";
            return newXmlDoc;
        }

        public XmlDocument xmlDoc
        {
            get
            {
                if (Session["xmlDoc"] != null)
                {
                    return ((XmlDocument)Session["xmlDoc"]);
                }
                return null;
            }
            set { Session["xmlDoc"] = value; }
        }

        //public void LoadXml(string xmlPath)
        //{
        //    Stream xmlStream = null;
        //    try
        //    {
        //        xmlStream = PdfCompareImageIndex.GetHeader(xmlPath, true);
        //    }
        //    catch
        //    {

        //    }
        //    StreamReader reader = new StreamReader(xmlStream);
        //    byte[] bytes1 = new byte[xmlStream.Length];
        //    xmlStream.Position = 0;
        //    xmlStream.Read(bytes1, 0, (int)xmlStream.Length);
        //    string text = System.Text.Encoding.Unicode.GetString(bytes1);
        //    try
        //    {
        //        xmlDoc = new XmlDocument();
        //        xmlDoc.LoadXml(text);
        //    }
        //    catch
        //    {
        //        //MessageBox.Show("Cannot load xml file");
        //    }
        //}

        //public XmlDocument xmlDoc
        //{
        //    get
        //    {
        //        if (Session["xmlDoc"] != null)
        //        {
        //            return ((XmlDocument)Session["xmlDoc"]);
        //        }
        //        return null;
        //    }
        //    set
        //    {
        //        Session["xmlDoc"] = value;
        //    }
        //}

        public static int MainCurrPage
        {
            get
            {
                if (HttpContext.Current.Session["MainCurrPage"] != null)
                {
                    return int.Parse(HttpContext.Current.Session["MainCurrPage"].ToString());
                }
                return -1;
            }
            set { HttpContext.Current.Session["MainCurrPage"] = value; }
        }

        //private void LoadResumeTaskPaths()
        //{
        //    string pDirPath = Common.GetTestFiles_SavingPath();

        //    String pdfName = Request.QueryString["PDF"];
        //    String rhywName = Request.QueryString["RHYW"];
        //    String userId = Request.QueryString["uid"];
        //    string bookId = pdfName;
        //    string testType = Request.QueryString["type"];

        //    Session["pdfName"] = pdfName;
        //    Session["userId"] = userId;

        //    string srcFilePath = "";
        //    string rhywFilePath = "";

        //    if ((pdfName != "") && (rhywName != ""))
        //    {
        //        srcFilePath = pDirPath + pdfName + ".pdf";
        //        rhywFilePath = pDirPath + rhywName + ".rhyw";
        //        //if (testType.Equals("test"))
        //        //{
        //        //    srcFilePath = Convert.ToString(Session["testSrcFilePath"]);
        //        //    rhywFilePath = Convert.ToString(Session["testRhywFilePath"]);
        //        //}
        //        //else if (testType.Equals("1"))
        //        //{
        //        //    srcFilePath = pDirPath + bookId + "\\" + bookId + "-1\\Comparison\\Comparison-1" + "\\" + userId + "\\" + pdfName;
        //        //    rhywFilePath = pDirPath + bookId + "\\" + bookId + "-1\\Comparison\\Comparison-1" + "\\" + userId + "\\" + rhywName;
        //        //}
        //        //else
        //        //{
        //        //    srcFilePath = pDirPath + bookId + "\\" + bookId + "-1\\Comparison\\Comparison-2" + "\\" + userId + "\\" + pdfName;
        //        //    rhywFilePath = pDirPath + bookId + "\\" + bookId + "-1\\Comparison\\Comparison-2" + "\\" + userId + "\\" + rhywName;
        //        //}
        //    }

        //    if (srcFilePath != "" && rhywFilePath != "")
        //    {
        //        SiteSession.PDFManFileObj = new PDFManipulation(srcFilePath);
        //        SiteSession.RHYWManFileObj = new RHYWManipulation(rhywFilePath);
        //        PDFCmpSesssion.XMLFilePath = rhywFilePath.Replace(".rhyw", ".xml");

        //        Session["MainXMLFilePath"] = PDFCmpSesssion.XMLFilePath;

        //        SiteSession.MainXMLFilePath_PDF = PDFCmpSesssion.XMLFilePath;

        //        SiteSession.RHYWManFileObj.SaveCompleteXMLFile(PDFCmpSesssion.XMLFilePath);

        //        int srcTotalPages = SiteSession.PDFManFileObj.TotalPages;
        //        SiteSession.MainCurrPage = 1;
        //    }

        //    Response.Redirect("Comparison.aspx", true);
        //}

        public void SaveComparisonTest(string username, string email, string testName, string ipAddress, int totalMarks,
            string testType)
        {
            MyDBClass objMyDBClass = new MyDBClass();
            string effectedRows = objMyDBClass.InsertOnlineTest(username, email, testName, ipAddress, totalMarks,
                testType);
        }

        public string GetIPAddress()
        {
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

        private string GetTotalInjectedMistakes()
        {
            StreamReader strreader = new StreamReader(PDFCmpSesssion.XMLFilePath);
            string xmlInnerText = strreader.ReadToEnd();
            strreader.Close();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlInnerText);

            string totalMistakes =
                Convert.ToString(xmlDoc.SelectNodes(@"//*[@correction]").Count + xmlDoc.SelectNodes(@"//missing").Count +
                                 xmlDoc.SelectNodes(@"//*[@conversion]").Count);

            return totalMistakes;
        }

        private void FindErrorCallBack(IAsyncResult result)
        {
            int currProcessedPages = (int)Session["CurrProcessedPages"];
            Session["CurrProcessedPages"] = currProcessedPages++;
        }

        protected void ProgressBar1_RunTask(object sender, EO.Web.ProgressTaskEventArgs e)
        {
            btnProceed.Enabled = false;
            CompareSourcePDFWithRHYW(e);
        }

        protected void btnProceed_Click(object sender, EventArgs e)
        {
            //Response.Redirect("PDFComparison.aspx");
            //ProgressBar1_RunTask(sender,e);
            //ProgressBar1.Visible = true;
        }

        [System.Web.Services.WebMethod()]
        public static string test1(int cateogryID)
        {
            return "a";
        }
    }
}
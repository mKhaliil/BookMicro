using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.DynamicData;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Data;
using System.Xml;
using System.Configuration;
using System.IO;
using Outsourcing_System.CommonClasses;
using Outsourcing_System.MasterPages;
using Outsourcing_System.PdfCompare_Classes;

namespace Outsourcing_System
{
    public partial class OnlineTestUser : System.Web.UI.Page
    {
        public void ClearMissingImgSessions()
        {
            Session.Remove("MissingImagesNames");
            Session.Remove("ImagePdfTotalPageCount");
            Session.Remove("ImagePdfPages");
            Session.Remove("CurrentPage");
            Session.Remove("ActualPdfPage");
            Session.Remove("misingImgPdfPath");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                TableDetection tblDetectionObj = new TableDetection();
                tblDetectionObj.TableHeader = null;
                tblDetectionObj.TableCaption = null;

                string bookId = Convert.ToString(Request.QueryString.Get("bid"));
                string comparisonType = Convert.ToString(Request.QueryString.Get("ct"));
             
                if (Convert.ToString(Session["LoginId"]) == "") Response.Redirect("Bookmicro.aspx", true);
                string userId = Convert.ToString(Session["LoginId"]);

                bool testStatus = false;

                divUpgradeLevel.Attributes.Add("style", "display:none");
                divBooksLegend.Attributes.Add("style", "display:none");

                if (objMyDBClass.CheckComplexBitsTestStatus(userId, "ErrorDetectionComplexBits"))
                {
                    testStatus = true;
                    Session["levelUpgraded"] = "true";
                }
                else
                {
                    Session["levelUpgraded"] = "false";
                }

                int userRank = GetUserRank(userId, testStatus);

                //If user is a trainee editor or he has not passed ErrorDetectionComplexBits then hide upgrade level
                if (!testStatus || userRank < 2)
                {
                    divUpgradeLevel.Attributes.Add("style", "display:block");
                    divBooksLegend.Attributes.Add("style", "display:none");
                    //"width:61%; height:20px; color:#0066cc; font-weight:bold; margin-left:auto; margin-right:auto;

                }

                else if (testStatus)
                {
                    divUpgradeLevel.Attributes.Add("style", "display:none");
                    //divBooksLegend.Attributes.Add("style", "display:block");

                    divBooksLegend.Attributes.Add("style", "display:block; width:61%; height:20px; color:#0066cc; font-weight:bold; margin-left:auto; margin-right:auto");
                }

                ////If user is a trainee editor or he has not passed ErrorDetectionComplexBits then hide upgrade level
                //if (!testStatus || userRank == 1)
                //    divUpgradeLevel.Attributes.Add("style", "display:none");
                //else
                //    divUpgradeLevel.Attributes.Add("style", "display:block");

                //UpdateUserRank(userId, userRank);
                GetUserProfile(userId);

                if (!string.IsNullOrEmpty(comparisonType) && comparisonType.Equals("table"))
                {
                    ucShowMessage1.ShowMessage(MessageTypes.Success, "Table Task is completed successfully.");
                }
                else if (!string.IsNullOrEmpty(comparisonType) && comparisonType.Equals("image"))
                {
                    ClearMissingImgSessions();
                    ucShowMessage1.ShowMessage(MessageTypes.Success, "Image Task is completed successfully.");
                }

                if (bookId != null)
                {
                    GetRemainingDetectedErrors(bookId, userId, comparisonType);
                }

                if (userId != "")
                {
                    Session["LoginId"] = userId;
                }
                else
                {
                    Response.Redirect("Bookmicro.aspx", true);
                }

                BindAvailableTaskGrid();
                BindMyTaskGrid();
                CountPassedTests();
                CountCompletedTasks();
                CountProgressTasks();
                //GetUserRank();
                ProcessControl1.LoadProcesses();
            }
        }

        public void GetUserProfile(string userId)
        {
            List<TestUser> list = objMyDBClass.GetUserProfile(userId);

            int imgTask_Test = 0;
            int tableTask_Test = 0;
            int indexTask_Test = 0;
            int mappingTask_Test = 0;

            if ((list != null) && (list.Count > 0))
            {
                lblProfileName.Text = list[0].FullName;
                lblDescription.Text = list[0].Description;

                if (!string.IsNullOrEmpty(imgUserProfile.ImageUrl))
                {
                    if (list[0].Gender.Equals("male")) imgUserProfile.ImageUrl = "~/img/male.png";
                    else if (list[0].Gender.Equals("female")) imgUserProfile.ImageUrl = "~/img/female.png";
                }
                else imgUserProfile.ImageUrl = list[0].Picture;

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
                    //lblMapping.Text = "Mapping";
                    lblMapping.Text = "Error Detection";
                    divMappingTest.Visible = true;
                }
            }
        }
        //public void ShowTaskStatus()
        //{


        //}

        public void GetRemainingDetectedErrors(string bookId, string userId, string comparisonType)
        {
            string msg = "";
            int startPage = 0;
            int endPage = 0;
            bool status = false;

            Common commObj = new Common();

            string path = ConfigurationManager.AppSettings["MainDirPhyPath"] + "\\" + bookId + "\\" + bookId + "-1\\Comparison\\Comparison-" +
                          comparisonType + "\\" + userId + "\\" + bookId + "-1.rhyw";
            commObj.LoadXml(path);
            XmlDocument doc = commObj.xmlDoc;

            int totalPages = doc.SelectNodes(@"//ln").Cast<XmlNode>().Select(node => Convert.ToInt32(node.Attributes["page"].Value)).Distinct().ToList().Max();
            int totalErrors = doc.SelectNodes(@"//*[@correction]").Count + doc.SelectNodes(@"//*[@conversion]").Count;
            int resolvedErrors = doc.SelectNodes(@"//*[@correction='']").Count + doc.SelectNodes(@"//*[@conversion='']").Count;
            int remainingErrors = doc.SelectNodes(@"//*[@correction!='']").Count + doc.SelectNodes(@"//*[@conversion!='']").Count;

            int leftMistakeCount = totalErrors - resolvedErrors;
            double passingWithFullPayment = Convert.ToDouble((80 * totalErrors) / 100);

            ////Log out task end time
            //PdfCompareMyDBClass obj = new PdfCompareMyDBClass();
            //string TaskId = obj.getTaskId(bookId);
            //if (TaskId != "")
            //    obj.handleLog(TaskId, "(checked out) ", userId);
            ////end

            if (totalErrors > 0)
            {
                //User detects all mistakes
                //if ((leftMistakeCount == 0) || (leftMistakeCount >= passingWithFullPayment))
                if (leftMistakeCount == 0)
                {
                    if (!string.IsNullOrEmpty(Convert.ToString(Session["srcTotalPages"])) || !string.IsNullOrEmpty(Convert.ToString(Session["BookId"])))
                    {
                        MyDBClass dbObj = new MyDBClass();
                        int pageViewedCount = dbObj.GetPageViewedCount(Convert.ToString(Session["BookId"]));
                        if (pageViewedCount < Convert.ToInt32(Session["srcTotalPages"]))
                        {
                            int remainingPages = Convert.ToInt32(Session["srcTotalPages"]) - pageViewedCount;

                            if (remainingPages == 1)
                            {
                                ucShowMessage1.ShowMessage(MessageTypes.Error,
                                    "Please try again! Your task is not completed because you don't view 1 page.");
                                return;
                            }
                            else
                            {
                                ucShowMessage1.ShowMessage(MessageTypes.Error, "Please try again! Your task is not completed because you don't view " +
                                    remainingPages + " pages.");
                                return;
                            }
                        }
                    }

                    string bId = Convert.ToString(Session["BookId"]) + "-1";
                    MyDBClass db = new MyDBClass();

                    //Complete error detection task and set status to 1 in QaComparisonTask table.
                    string st = db.CompleteErrorDetectionTask(bId, Convert.ToInt32(Session["LoginId"]));

                    //Change status of task to Pending Confirmation for approval from admin user after checking it.
                    Common.finishTask(bId, Convert.ToString(Session["LoginId"]));

                    //Save produced .rhyw file at outer folder
                    string rhywFilePath = ConfigurationManager.AppSettings["MainDirPhyPath"] + "/" + bId.Replace("-1", "") + "/" + bId.Replace("-1", ".rhyw");

                    string newXmlPath = Common.GetDirectoryPath() + Convert.ToString(Session["MainBook"]) + "/" + Convert.ToString(Session["MainBook"]) +
                                   "-1/TaggingUntagged" + "/" + Convert.ToString(Session["MainBook"]) + "-1_actual.rhyw";

                    if (!File.Exists(newXmlPath)) return;

                    commObj.LoadXml(newXmlPath);
                    Session["xmlDoc"] = commObj.xmlDoc;

                    GlobalVar objGlobal = new GlobalVar();
                    objGlobal.PBPDocument = Session["xmlDoc"] as XmlDocument;
                    objGlobal.XMLPath = rhywFilePath;
                    objGlobal.SaveXml();

                    ucShowMessage1.ShowMessage(MessageTypes.Success, "Your Task is submitted successfully and is under review. We will notify you in due course. You can start next task now!");

                    //web_ComparisonPreProcess cp = new web_ComparisonPreProcess();
                    //string currentStatus = "pause";
                    //string timeWorked = cp.StopTask(true, currentStatus);

                    //////UserMaster ParentMasterPage = (UserMaster)Page.Master;
                    //////ParentMasterPage.ShowMessageBox("Success! Your task is completed", "Succ");
                    CreateQaInspectionTask(bId, Convert.ToString(Session["LoginId"]));

                    ////AutoMapService.AutoMappService objAutoMapService = new AutoMapService.AutoMappService();
                    ////try
                    ////{
                    ////    objAutoMapService.ManualCorrections(rhywFilePath);
                    ////    System.Threading.Thread.Sleep(5000);
                    ////    objAutoMapService.Dispose();
                    ////}
                    ////catch (Exception ex)
                    ////{
                    ////    throw ex;
                    ////}
                    ////finally
                    ////{
                    ////    objAutoMapService.Dispose();
                    ////}
                }
                else
                {
                    //For reading correction tag in lines
                    List<int> list_Pages = doc.SelectNodes(@"//*[@correction!='' or @conversion!='']").Cast<XmlNode>().
                        Select(node => (node.Attributes["page"]) == null ? 0 : Convert.ToInt32(node.Attributes["page"].Value)).
                        Distinct().Where(x => x > 0).ToList();

                    List<int> list_PagesWithParaMistakes = null;

                    //For reading correction tag from para (for split errors)
                    var tempPages = doc.SelectNodes(@"//*[@correction!='' or @conversion!='']");
                    if (tempPages != null)
                    {
                        if (tempPages.Count > 0)
                        {
                            list_PagesWithParaMistakes = new List<int>();

                            foreach (XmlNode node in tempPages)
                            {
                                if (node.ChildNodes != null && node.ChildNodes.Count > 0)
                                {
                                    if (node.Name.Trim().Equals("spara"))
                                    {
                                        XmlNode line = node.SelectSingleNode("descendant::ln");
                                        if (line != null && line.Attributes.Count > 0 && line.Attributes["page"] != null)
                                        {
                                            list_PagesWithParaMistakes.Add(Convert.ToInt32(line.Attributes["page"].Value));
                                        }
                                    }
                                    else
                                    {
                                        foreach (XmlNode childNode in node.ChildNodes)
                                        {
                                            if (childNode.Attributes != null && childNode.Attributes.Count > 0 && !childNode.Name.ToLower().Equals("break"))
                                                if (childNode.Attributes["page"] != null)
                                                {
                                                    list_PagesWithParaMistakes.Add(Convert.ToInt32(childNode.Attributes["page"].Value));
                                                }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    List<int> list_MissingPages = new List<int>();
                    list_MissingPages.AddRange(list_Pages);
                    list_MissingPages.AddRange(list_PagesWithParaMistakes);

                    startPage = list_MissingPages.Count < 1 ? 1 : list_MissingPages.Distinct().Min();

                    int errorPageNumberRange = 50;
                    int errorPageOffset = totalPages - (startPage + errorPageNumberRange);

                    if (totalPages == 1)
                    {
                        msg = "Page 1.";
                    }

                    else if (totalPages < 50)
                    {
                        msg = "Page 1 to " + totalPages + ".";
                    }

                    else if (totalPages >= 50)
                    {
                        if (errorPageOffset >= 0)
                        {
                            msg = "Page " + startPage + " to " + (startPage + errorPageNumberRange) + ".";
                        }
                        else
                        {
                            msg = "Page " + (startPage + errorPageOffset + 1) + " to " + (startPage + errorPageOffset + errorPageNumberRange) + ".";
                        }
                    }

                    if (leftMistakeCount <= 1)
                        ucShowMessage1.ShowMessage(MessageTypes.Error, "Please try again! Your task is not completed because you have left " + leftMistakeCount +
                                                   " mistake which is in " + msg);
                    else
                        ucShowMessage1.ShowMessage(MessageTypes.Error, "Please try again! Your task is not completed because you have left " + leftMistakeCount +
                                                    " mistakes which are in " + msg);
                }
            }
            //If no mistake is injected in mistake injection task
            else
            {
                if (!string.IsNullOrEmpty(Convert.ToString(Session["srcTotalPages"])) || !string.IsNullOrEmpty(Convert.ToString(Session["BookId"])))
                {
                    MyDBClass dbObj = new MyDBClass();
                    int pageViewedCount = dbObj.GetPageViewedCount(Convert.ToString(Session["BookId"]));
                    if (pageViewedCount < Convert.ToInt32(Session["srcTotalPages"]))
                    {
                        int remainingPages = Convert.ToInt32(Session["srcTotalPages"]) - pageViewedCount;

                        if (remainingPages == 1)
                        {
                            ucShowMessage1.ShowMessage(MessageTypes.Error,
                                "Please try again! Your task is not completed because you don't view 1 page.");
                            return;
                        }
                        else
                        {
                            ucShowMessage1.ShowMessage(MessageTypes.Error, "Please try again! Your task is not completed because you don't view " +
                                remainingPages + " pages.");
                            return;
                        }
                    }
                }
            }
        }

        public void CreateQaInspectionTask(string bid, string userId)
        {
            MyDBClass objMyDBClass = new MyDBClass();
            string querySel = "Select BID from BOOK Where BIdentityNo='" + bid + "'";
            DataSet dsBookInfo = objMyDBClass.GetDataSet(querySel);
            string bookID = dsBookInfo.Tables[0].Rows[0]["BID"].ToString();

            int inResult = objMyDBClass.CreateTask(bookID, "Unassigned", "QaInspection", userId);
        }

        private void BindAvailableTaskGrid()
        {
            MyDBClass objMyDBClass = new MyDBClass();

            List<OnlineTest> list_tasks = objMyDBClass.GetUserAvailableTasks(Convert.ToString(Session["LoginId"]));

            ////Remove dublicate record from the list
            //var distinct_list = list_tasks.GroupBy(x => x.BookId).Select(x => x.First()).ToList();

            //if ((list_tasks != null) && (list_tasks.Count > 0))
            //{

            if (list_tasks == null) return;

             string userRole = Convert.ToString(Session["UserRole"]);
            string userRank = Convert.ToString(Session["UserRank"]);

            if (!string.IsNullOrEmpty(userRank) && !string.IsNullOrEmpty(userRole))
            {
                string configKeyName = string.Empty;

                if (userRank.Trim().Equals("Trainee Editor") && !userRole.Trim().Equals("2"))
                {
                    var temp = ConfigurationManager.AppSettings["TraineerEditorTasks"] == "" ? "" : Convert.ToString(ConfigurationManager.AppSettings["TraineerEditorTasks"]); ;

                    if (!string.IsNullOrEmpty(temp))
                    {
                        List<string> initailTaskList = temp.Split(',').ToList();

                        if (initailTaskList != null)
                        {
                            if (initailTaskList.Count > 0 && (list_tasks != null) && (list_tasks.Count > 0))
                                list_tasks.RemoveAll(x => !initailTaskList.Contains(x.BookId.Trim()));
                        }
                    }
                }
                else
                {
                    var tempOther = ConfigurationManager.AppSettings["TraineerEditorTasks"] == "" ? "" : Convert.ToString(ConfigurationManager.AppSettings["TraineerEditorTasks"]); ;
                    List<string> initailTaskListOther = tempOther.Split(',').ToList();

                    if (initailTaskListOther != null)
                    {
                        if (initailTaskListOther.Count > 0)
                            list_tasks.RemoveAll(x => initailTaskListOther.Contains(x.BookId.Trim()));
                    }
                }
            }

            //string task = "";

            //list_tasks.RemoveAll(x => (x.BookId.Trim() !=  task.Trim()));

            gvTaskManager.DataSource = list_tasks;
            gvTaskManager.DataBind();
            //}

            if ((list_tasks != null) && (list_tasks.Count > 0)) lblAsterikOpenTasks.Text = "* Approximate bonus amount for time and quality";
        }

        private void BindMyTaskGrid()
        {
            MyDBClass objMyDBClass = new MyDBClass();
            List<OnlineTest> list_tasks = objMyDBClass.GetMyTasks(Convert.ToString(Session["LoginId"]));

            //if ((list_tasks != null) && (list_tasks.Count > 0))
            //{
            gvMyTasks.DataSource = list_tasks;
            gvMyTasks.DataBind();
            //}

            if ((list_tasks != null) && (list_tasks.Count > 0)) lblAsterikMyTasks.Text = "* Approximate bonus amount for time and quality";
        }

        //public void GetUserRank()
        //{
        //    MyDBClass objMyDBClass = new MyDBClass();
        //    int rank = objMyDBClass.GetUserRank(Convert.ToString(Session["LoginId"]));

        //    lblRank.Text = Convert.ToString(rank);
        //}

        public int GetUserRank(string userId, bool testStatus)
        {
            MyDBClass objMyDBClass = new MyDBClass();
            int approvedTask = objMyDBClass.GetApprovedTasksCount(userId, "ErrorDetection");
            List<UserRank> userRanksList = objMyDBClass.GetUserRanksByTask("ErrorDetection");
            int userRankId = 0;

            if (approvedTask >= 0 && userRanksList != null)
            {
                if (userRanksList.Count == 5)
                {
                    for (int i = 0; i < userRanksList.Count; i++)
                    {
                        if (i == 0) userRanksList[i].MaxApprovedTasks = 0;
                        else 
                        {
                            userRanksList[i].MaxApprovedTasks = getMaxTasks(userRanksList, i);
                            userRanksList[i].MinApprovedTasks = getMinTasks(userRanksList, i);
                        }
                    }

                    //                min   max  Total Tasks in  rank , Required For next rank 
                    //Trainee Editor = 0  -  0

                    //Junior Editor  = 1  - 19  =  20 , 21

                    //Editor         = 21 - 79  =  100 , 101

                    //Senior Editor  = 101 - 200 =  200 , 201

                    //Expert Editor  = 201 + ....

                    var currentRank = userRanksList.Where(x => (approvedTask >= x.MinApprovedTasks && approvedTask <= x.MaxApprovedTasks)).ToList();

                    if (currentRank.Count > 0)
                    {
                        if (testStatus)
                        {
                          var nextRank = userRanksList.Where(x => Convert.ToInt32(x.RankId) > Convert.ToInt32(currentRank[0].RankId)).ToList();

                          lblJobRank.Text = nextRank[0].RankName;
                          userRankId = Convert.ToInt32(nextRank[0].RankId);

                          if (nextRank[0].RankName.Trim().ToLower().Equals("trainee editor") || nextRank[0].RankName.Trim().ToLower().Equals("junior editor"))
                              btnTestTraining.Visible = false;

                          if (!nextRank[0].RankName.Trim().Equals("Expert Editor"))
                          {
                              int remainingTaskCount = (nextRank[0].MaxApprovedTasks - approvedTask) + 1;

                              if (remainingTaskCount == 1)
                                  lblNextRank.Text = (nextRank[0].MaxApprovedTasks - approvedTask) + 1 + " more Task to change Rank";

                              else if (remainingTaskCount > 1)
                                  lblNextRank.Text = (nextRank[0].MaxApprovedTasks - approvedTask) + 1 + " more Tasks to change Rank";
                          }

                          lblRank.Text = Convert.ToString("0");
                          Session["UserRank"] = nextRank[0].RankName; 
                        }
                        else
                        {
                            lblJobRank.Text = currentRank[0].RankName;
                            userRankId = Convert.ToInt32(currentRank[0].RankId);

                            if (currentRank[0].RankName.Trim().ToLower().Equals("trainee editor") || currentRank[0].RankName.Trim().ToLower().Equals("junior editor"))
                                btnTestTraining.Visible = false;

                            if (!currentRank[0].RankName.Trim().Equals("Expert Editor"))
                            {
                                int remainingTaskCount = (currentRank[0].MaxApprovedTasks - approvedTask) + 1;

                                if (remainingTaskCount == 1)
                                    lblNextRank.Text = (currentRank[0].MaxApprovedTasks - approvedTask) + 1 + " more Task to change Rank";

                                else if (remainingTaskCount > 1)
                                    lblNextRank.Text = (currentRank[0].MaxApprovedTasks - approvedTask) + 1 + " more Tasks to change Rank";
                            }

                            lblRank.Text = Convert.ToString("0");

                            Session["UserRank"] = currentRank[0].RankName; 
                        }
                       

                        //string colour = "";

                        //if (currentRank[0].RankName.Trim().Equals("Trainee Editor")) colour = "#ff0000;";

                        //else if (currentRank[0].RankName.Trim().Equals("Junior Editor")) colour = "#ff0000;";

                        //else if (currentRank[0].RankName.Trim().Equals("Editor")) colour = "#ff0000;";

                        //else if (currentRank[0].RankName.Trim().Equals("Senior Editor")) colour = "#ff0000;";

                        //else if (currentRank[0].RankName.Trim().Equals("Expert Editor")) colour = "#ff0000;";

                        //star.Attributes.Add("style", "width: 0;height: 40px;border-left: 20px solid " + colour + "border-right: 20px solid #ff0000;" +
                        //    "border-bottom: 15px solid transparent;position: relative;margin:0 auto;");

                        //star.Attributes.Add("style", "#star span{ color:#FFFF00; font-size:32px; text-align:left; margin-left:-8px;}");
                        //star.Attributes.Add("style", "#star:after {width: 30px;height: 35px;border-top: 1px dashed rgba(255,255,255,0.4);" + 
                        //    "border-bottom: 1px solid transparent;border-left: 1px dashed rgba(255,255,255,0.4);border-right: 1px dashed rgba(255,255,255,0.4);position: absolute;content: '';top: 4px;left: -16px;}");
                    }
                }
            }

            return userRankId;
        }
        
        private int getMaxTasks(List<UserRank> userRanksList, int i)
        {
            if (i == userRanksList.Count - 1) return 10000;
            else i++;
            
            if (userRanksList != null)
            {
                int maxTasks = 0;

                if (userRanksList.Count > 0)
                {
                    while (i > 0)
                    {
                        maxTasks += userRanksList[i].RequiredTasks;
                        i--;
                    }
                }
                return maxTasks - 1;
            }
            return 0;
        }

        private int getMinTasks(List<UserRank> userRanksList, int i)
        {
            if (i == userRanksList.Count - 1) return 10000;

            if (userRanksList != null)
            {
                int maxTasks = 0;

                if (userRanksList.Count > 0)
                {
                    while (i > 0)
                    {
                        maxTasks += userRanksList[i].RequiredTasks;
                        i--;
                    }
                }
                return maxTasks;
            }
            return 0;
        }
        public void UpdateUserRank(string userId, int rankId)
        {
            MyDBClass objMyDBClass = new MyDBClass();
            int status = objMyDBClass.UpdateUserRank(userId, rankId);
        }

        //public void CalculateApprovedTasks(int rank, int approvedTaskCount, int minTasksRequired)
        //{
        //    int tasksToChangeRank = 0; 

        //    if (approvedTaskCount == 0)
        //    {
        //        lblJobRank.Text = "Trainee Editor";
        //        star.Attributes.Add("style", "width: 0;height: 40px;border-left: 20px solid #ff0000;border-right: 20px solid #ff0000;" +
        //                                     "border-bottom: 15px solid transparent;position: relative;margin:0 auto;");
        //    }
        //    else if (approvedTaskCount >= 1 && approvedTaskCount < 20)
        //    {
        //        lblJobRank.Text = "Junior Editor";
        //        star.Attributes.Add("style", "width: 0;height: 40px;border-left: 20px solid #ff0000;border-right: 20px solid #ff0000;" +
        //                                    "border-bottom: 15px solid transparent;position: relative;margin:0 auto;");
        //    }
        //    else if (approvedTaskCount >= 20 && approvedTaskCount < 80)
        //    {
        //        lblJobRank.Text = "Editor";
        //        star.Attributes.Add("style", "width: 0;height: 40px;border-left: 20px solid #ff0000;border-right: 20px solid #ff0000;" +
        //                                    "border-bottom: 15px solid transparent;position: relative;margin:0 auto;");
        //    }
        //    else if (approvedTaskCount >= 80 && approvedTaskCount < 100)
        //    {
        //        lblJobRank.Text = "Senior Editor";
        //        star.Attributes.Add("style", "width: 0;height: 40px;border-left: 20px solid #ff0000;border-right: 20px solid #ff0000;" +
        //                                    "border-bottom: 15px solid transparent;position: relative;margin:0 auto;");
        //    }
        //    else if (approvedTaskCount >= 100)
        //    {
        //        lblJobRank.Text = "Expert Editor";
        //        star.Attributes.Add("style", "width: 0;height: 40px;border-left: 20px solid #ff0000;border-right: 20px solid #ff0000;" +
        //                                    "border-bottom: 15px solid transparent;position: relative;margin:0 auto;");
        //    }

        //    tasksToChangeRank = minTasksRequired - approvedTaskCount;
        //    lblNextRank.Text = Convert.ToString(tasksToChangeRank) + " more Tasks to change Rank.";
        //    lblRank.Text = Convert.ToString(rank);
        //}

        public XmlDocument LoadXmlDocument(string xmlPath)
        {
            if ((xmlPath == "") || (xmlPath == null))
                return null;

            StreamReader strreader = new StreamReader(xmlPath);
            string xmlInnerText = strreader.ReadToEnd();
            strreader.Close();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlInnerText);

            return xmlDoc;
        }

        public void CountPassedTests()
        {
            MyDBClass objMyDBClass = new MyDBClass();
            List<TestUser> tests = objMyDBClass.GetPassedTests_Count_ByUser(Convert.ToString(Session["LoginId"]));

            int imgTask_Test = 0;
            int tableTask_Test = 0;
            int indexTask_Test = 0;
            int mappingTask_Test = 0;

            double mappingPercentage = 0.0;
            double imagePercentage = 0.0;
            double indexPercentage = 0.0;
            double tablePercentage = 0.0;

            if ((tests != null) && (tests.Count > 0))
            {
                foreach (var item in tests)
                {
                    if (item.TestType.Trim().ToLower().Equals("tables"))
                    {
                        tableTask_Test++;
                        tablePercentage = (Convert.ToDouble(item.ObtainedScore) / Convert.ToDouble(item.TotalScore)) * 100;
                    }
                    else if (item.TestType.Trim().ToLower().Equals("images"))
                    {
                        imgTask_Test++;
                        imagePercentage = (Convert.ToDouble(item.ObtainedScore) / Convert.ToDouble(item.TotalScore)) * 100;
                    }
                    else if (item.TestType.Trim().ToLower().Equals("index"))
                    {
                        indexTask_Test++;
                        indexPercentage = (Convert.ToDouble(item.ObtainedScore) / Convert.ToDouble(item.TotalScore)) * 100;
                    }
                    else if (item.TestType.Trim().ToLower().Equals("comparison"))
                    {
                        mappingTask_Test++;
                        mappingPercentage = (Convert.ToDouble(item.ObtainedScore) / Convert.ToDouble(item.TotalScore)) * 100;
                    }
                }

                if (tableTask_Test > 0)
                {
                    tdTableTasks.Visible = true;
                    tbline1.Visible = true;
                    tbline2.Visible = true;
                }

                if (imgTask_Test > 0)
                {
                    tdImageTasks.Visible = true;
                    tbline2.Visible = true;
                    tbline3.Visible = true;
                }

                if (indexTask_Test > 0)
                {
                    tdIndexTasks.Visible = true;
                    tbline3.Visible = true;
                    tbline4.Visible = true;
                }

                if (mappingTask_Test > 0)
                {
                    tdMappingTask.Visible = true;
                    tbline4.Visible = true;
                    tbline5.Visible = true;
                }

                int passedTestCount = tests.Where(x => x.TestType.Equals("Comparison")).ToList().Count;
                lblTestTaken.Text = "Tests (" + passedTestCount + ")"; 
                //lblTestTaken.Text = "Tests (" + tests.Count + ")";
                lblTableTasks.Text = Convert.ToString(Math.Round(tablePercentage, 2)) + "%";
                lblImageTasks.Text = Convert.ToString(Math.Round(imagePercentage, 2)) + "%";
                lblIndexTasks.Text = Convert.ToString(Math.Round(indexPercentage, 2)) + "%";
                lblMappingTasks.Text = Convert.ToString(Math.Round(mappingPercentage, 2)) + "%";
            }
        }

        public void CountCompletedTasks()
        {
            MyDBClass objMyDBClass = new MyDBClass();
            List<OnlineTest> list_Completedtasks = objMyDBClass.GetCompletedTasks_ByUser(Convert.ToString(Session["LoginId"]));
            int count = 0;

            if ((list_Completedtasks != null) && (list_Completedtasks.Count > 0))
            {
                count = list_Completedtasks.Count;
                BindCompletedTaskGrid(list_Completedtasks);
            }

            lblTasksCompleted.Text = "Tasks Completed: " + count;
        }

        public void CountProgressTasks()
        {
            MyDBClass objMyDBClass = new MyDBClass();
            int count = objMyDBClass.GetProgressTasks_ByUser(Convert.ToString(Session["LoginId"]));
            lblTasksInProgress.Text = "Tasks In Progress: " + count;
        }

        private void BindCompletedTaskGrid(List<OnlineTest> completedTasks)
        {
            //if (completedTasks.Count > 0)
            //{
            //    if (completedTasks.Count > 20)
            //    {
            //        completedTasks.RemoveRange(20, completedTasks.Count - 20);
            //    }

            //    gvCompletedTasks.DataSource = completedTasks;
            //    gvCompletedTasks.DataBind();
            //}
        }

        public void imgbtnEditProfile_Click(object sender, EventArgs e)
        {
            //ucOnlineTestTasks1.SetEditView = 1;
            ////ucOnlineTestTasks1.NormalProfile = false;
            ////ucOnlineTestTasks1.EditProfile = true;
            ////ucOnlineTestTasks1.GetUserEditProfile();
        }

        protected void btnCancel_Click(object sender, System.EventArgs e)
        {
            //divEditProfile.Visible = false;
        }

        //protected void imgAssignMe_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        if (Session["LoginId"] != null)
        //        {
        //            ProcessControl1.UncheckedSelectedBoxes();
        //            MyDBClass objMyDBClass = new MyDBClass();
        //            int index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
        //            string Id = (string)this.gvTaskManager.DataKeys[index].Values[0];
        //            Label taskType = (Label)gvTaskManager.Rows[index].FindControl("lblIssueCategory");
        //            string bookID = Id;
        //            Session["bid"] = bookID;
        //            string userid = Convert.ToString(Session["LoginId"]);
        //            string queryActivity = " select * from activity where [UID]=" + userid + " and Task like '" + taskType.Text + "' and Status='working'";
        //            DataSet dsActivity = objMyDBClass.GetDataSet(queryActivity);
        //            if (dsActivity.Tables[0].Rows.Count == 0)
        //            {
        //                string pName = taskType != null ? taskType.Text : "";
        //                queryActivity = "Select Task,Status from Activity where BID=" + bookID + " and Task='" + pName + "'";
        //                dsActivity = objMyDBClass.GetDataSet(queryActivity);
        //                ProcessControl1.setSelectedBoxes(dsActivity, pName, "AssignTask");
        //                TaskDetails();
        //                AssignMe.Visible = true;

        //                objMyDBClass.ChangeTask_Status(Id, "");

        //                //BindTaskGrid();
        //                CountProgressTasks();

        //            }
        //            else
        //            {
        //                showMessage("You have already a task of same category in process.");
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        showMessage(ex.Message);
        //    }

        //}

        protected void imgAssignMe_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["LoginId"] != null)
                {
                    //ProcessControl1.UncheckedSelectedBoxes();
                    MyDBClass objMyDBClass = new MyDBClass();
                    int index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
                    string Id = (string)this.gvTaskManager.DataKeys[index].Values[0];
                    Label taskType = (Label)gvTaskManager.Rows[index].FindControl("lblIssueCategory");
                    string bookID = Id;
                    Session["bid"] = bookID;

                    if (Convert.ToString(Session["UserRank"]).Equals("Trainee Editor"))
                    {
                        if (!bookID.Equals(""))
                        {
                            ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry you are not allowed to assign this book to yourself.");
                            return;
                        }
                    }

                    string userid = Convert.ToString(Session["LoginId"]);
                    string queryActivity = " select * from activity where [UID]=" + userid + " and Task like '" + taskType.Text + "' and Status='working'";
                    DataSet dsActivity = objMyDBClass.GetDataSet(queryActivity);
                    if (dsActivity.Tables[0].Rows.Count == 0)
                    {
                        //Check if current bookid is assigned to someone else in BookMicro db
                        string currentUserId = Convert.ToString(Session["LoginId"]);
                        Label lblTestName = (Label)gvTaskManager.Rows[index].FindControl("lblTestName");
                        string mainBook = lblTestName.Text;

                        string queryBookId = "select Activity.uid from activity inner join book on Activity.BID = Book.BID where Book.MainBook=" + mainBook + "and activity.Task = 'ErrorDetection' and activity.Status='working'";

                        string uid = objMyDBClass.GetID(queryBookId);

                        if ((!uid.Trim().Equals("")) && (!uid.Trim().Equals("0")) && (!uid.Trim().Equals(currentUserId.Trim())))
                        {
                            ucShowMessage1.ShowMessage(MessageTypes.Error, "Someone has already picked this task please choose another task.");
                        }
                        //end

                        //Check if same task of high priority exists
                        string taskName = Convert.ToString(gvTaskManager.DataKeys[index].Values[2]);
                        string aid = Convert.ToString(gvTaskManager.DataKeys[index].Values[6]);
                        string priority = Convert.ToString(gvTaskManager.DataKeys[index].Values[7]);

                        string queryPriority = "select top (1) bid from activity where Priority >=" + priority + " and task='" + taskName + "' and aid < " + aid + " and UID=42 and status ='Unassigned'";

                        string priorityCheck = objMyDBClass.GetID(queryPriority);

                        //if (!priorityCheck.Trim().Equals("")) aamir
                        //{
                        //    showMessage("Task can't be assigned because another task with high priority already exists please choose another.");
                        //}
                        //end

                        //else
                        //{
                        string pName = taskType != null ? taskType.Text : "";
                        queryActivity = "Select Task,Status from Activity where BID=" + bookID + " and Task='" + pName + "'";
                        dsActivity = objMyDBClass.GetDataSet(queryActivity);
                        int effectedRows = objMyDBClass.UpdateTaskStatus(bookID, pName, 0,
                                                                         Convert.ToInt32(Session["LoginId"]), DateTime.Now.Date.GetDateTimeFormats('d')[5],
                                                                          "Working", "");
                        //}
                        CountProgressTasks();
                        BindMyTaskGrid();
                        BindAvailableTaskGrid();
                    }
                    else
                    {
                        ucShowMessage1.ShowMessage(MessageTypes.Error, "You have already a task of same category in process.");
                    }
                }
            }
            catch (Exception ex)
            {
                ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured while assigning task.");
                //showMessage(ex.Message);
            }
        }

        //private void showMessage(string message)
        //{
        //    if (message != "")
        //    {
        //        DivError.Visible = true;
        //        lblError.Text = message;
        //    }
        //    else
        //    {
        //        DivError.Visible = false;
        //    }
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

        protected void lbtnUpgradeLevel_Click(object sender, EventArgs e)
        {
            Response.Redirect("BookMicroUpgraded.aspx", true);
        }

        protected void lbtnSubmitTask_Click(object sender, EventArgs e)
        {
            MyDBClass objMyDBClass = new MyDBClass();
            int index = ((GridViewRow)((ImageButton)sender).Parent.Parent).RowIndex;
            string aid = (string)this.gvMyTasks.DataKeys[index].Values[0];
            string bid = (string)this.gvMyTasks.DataKeys[index].Values[1];
            string task = (string)this.gvMyTasks.DataKeys[index].Values[2];
            string status = (string)this.gvMyTasks.DataKeys[index].Values[3];
            Label mainBook = (Label)gvMyTasks.Rows[index].FindControl("lblTestName");
            UserMaster ParentMasterPage = (UserMaster)Page.Master;
            Session["BID"] = bid;
            Session["MainBook"] = mainBook.Text;
            Session["AID"] = aid;

            Session["xmlPath_MistakeInsertion"] = "";
            Session["xmlPath_MistakeInsertion"] = "";
            Session["pdfFile"] = "";

            if (!(status.Equals("Approved") || status.Equals("Pending Confirmation")))
            {
                if (task.Trim().Equals("MistakeInjection"))
                {
                    Response.Redirect("MistakesInsertion.aspx", true);
                }
                else if (task.Trim().Equals("ErrorDetection"))
                {
                    string resultMsg = CheckTaskAlreadyExistance(mainBook.Text.Trim(), bid.Trim());

                    if (resultMsg.Equals(""))
                    {
                        Response.Redirect("ComparisonPreProcess.aspx?type=task", true);
                    }
                    else
                    {
                        //////((UserMaster)this.Master).ShowMessageBox(resultMsg, "error");
                    }
                }

                else if (task.Trim().Equals("QaInspection") || task.Trim().ToLower().Equals("image"))
                {
                    Response.Redirect("SubmitTask.aspx", true);
                }

                //else if ((task.Trim().ToLower().Equals("image")) || (task.Trim().ToLower().Equals("index")) ||
                //         (task.Trim().ToLower().Equals("table")))

                else if (task.Trim().ToLower().Equals("table"))
                {
                    Session.Remove("CurrentPage");
                    Session.Remove("ActualPdfPage");

                    Response.Redirect("Process1.aspx", true);
                }
                else if (task.Trim().ToLower().Equals("spellcheck"))
                {
                    string rhywFilePath = objMyDBClass.MainDirPhyPath + "/" + mainBook.Text + "/" + mainBook.Text + "-1/TaggingUntagged/" + mainBook.Text + "-1.rhyw";

                    //Check if rhyw file is under process and not produced yet.
                    if (!File.Exists(rhywFilePath))
                    {
                        //////ParentMasterPage.ShowMessageBox("Warning! SpellCheck Task can't be started because rhyw file is not produced yet. Please wait and try again.", "Info");
                    }
                    else
                    {
                        Response.Redirect("SpellChecker.aspx", true);
                    }
                }
                else if (task.Trim().ToLower().Equals("meta"))
                {
                    Response.Redirect("MetaInformation.aspx", true);
                }
            }
        }

        public string CheckTaskAlreadyExistance(string mainBook, string bid)
        {
            try
            {
                string returnMsg = "";
                string srcFilePath = Common.GetDirectoryPath() + mainBook + "/" + mainBook + "-1/Comparison/Comparison-1/" +
                                        Convert.ToString(Session["LoginId"]) + "/" + mainBook + "-1.pdf";
                //In case of task resume.
                bool taskCompletionStatus = CheckTaskCompletionStatus(mainBook, Convert.ToString(Session["LoginId"]));
                if (!taskCompletionStatus)
                {
                    //Check if pdf file is present at specific location 
                    if (!File.Exists(srcFilePath))
                    {
                        //((UserMaster)this.Master).ShowMessageBox("Error! Comparison Task can't be resumed because source pdf is not present. ", "error");
                        returnMsg = "Error! Comparison Task can't be resumed because source pdf is not present. ";
                    }

                    return returnMsg;
                }
                //In case of a new task
                else
                {
                    //Check if current bookid is assigned to someone else in BookMicro db
                    string currentUserId = Convert.ToString(Session["LoginId"]);
                    string queryActivity = "select Activity.uid from activity inner join book on Activity.BID = Book.BID where Book.MainBook=" + mainBook + "and activity.Task = 'ErrorDetection' and activity.Status='working'";

                    string uid = objMyDBClass.GetID(queryActivity);

                    if ((!uid.Trim().Equals("")) && (!uid.Trim().Equals(currentUserId.Trim())))
                    {
                        returnMsg = "Error! Someone has already picked this task please choose another task.";
                    }
                    ////Check if book id is already inserted into workmeter
                    //else
                    //{
                    //    MyDBClass db = new MyDBClass();
                    //    string userId_OtherUser = db.checkAlreadyExists(mainBook, currentUserId, "27");

                    //    if ((!userId_OtherUser.Trim().Equals("")))
                    //    {
                    //        returnMsg = "Error! This task can't be started again because time is alreay logged against this book.";
                    //    }
                    //}
                    ////end
                    return returnMsg;
                }
            }
            catch (Exception ex)
            {
                return "sorry! Some error occurs in error detection task.";
            }
        }

        protected void btnAccountDetails_Click(object sender, EventArgs e)
        {
            Response.Redirect("TestUserAccountDetails.aspx", true);
        }

        protected void lbtnChangePassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ChangePassword.aspx", true);
            //ucOnlineTestTasks1.ChangePassword = true;
            //ucOnlineTestTasks1.NormalProfile = false;
            //ucOnlineTestTasks1.EditProfile = false;
            //ucOnlineTestTasks1.EditBankDetails = false;
            //ucOnlineTestTasks1.GetUserEditProfile();
        }
        protected void lbtnEditProfile_Click(object sender, EventArgs e)
        {
            Response.Redirect("UserDetails.aspx", true);

            //ucOnlineTestTasks1.EditProfile = true;
            //ucOnlineTestTasks1.NormalProfile = false;
            //ucOnlineTestTasks1.ChangePassword = false;
            //ucOnlineTestTasks1.EditBankDetails = false;
            //ucOnlineTestTasks1.GetUserEditProfile();
        }
        protected void lbtnEditBankDetails_Click(object sender, EventArgs e)
        {
            //ucOnlineTestTasks1.EditProfile = false;
            //ucOnlineTestTasks1.NormalProfile = false;
            //ucOnlineTestTasks1.ChangePassword = false;
            //ucOnlineTestTasks1.EditBankDetails = true;
            //ucOnlineTestTasks1.GetUserEditProfile();
        }

        protected void gvTaskManager_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");
                Label lblAmount = (Label)e.Row.FindControl("lblAmount");
                Label lblTaskTime = (Label)e.Row.FindControl("lblTaskTime");
                Label lblBonus = (Label)e.Row.FindControl("lblBonus");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                ImageButton ImgSubmitTask = (ImageButton)e.Row.FindControl("ImgSubmitTask");

                lblSrNo.Text = Convert.ToString(e.Row.RowIndex + 1) + ".";

                if (lblStatus.Text.Trim().ToLower().Equals("Pending Confirmation"))
                {
                    ImgSubmitTask.Enabled = false;
                }

                string complexity = Convert.ToString(gvTaskManager.DataKeys[e.Row.RowIndex].Values[3]);

                if (!string.IsNullOrEmpty(complexity) && complexity.ToLower().Trim().Equals("complex"))
                {
                    e.Row.BackColor = System.Drawing.Color.Turquoise;
                }

                string taskType = Convert.ToString(this.gvTaskManager.DataKeys[e.Row.RowIndex].Values[2]);
                double salary = 0;
                if ((taskType.Equals("Image")) || (taskType.Equals("Index")) || (taskType.Equals("Table")) || (taskType.Equals("ErrorDetection")))
                {
                    string Complexity = Convert.ToString(this.gvTaskManager.DataKeys[e.Row.RowIndex].Values[3]);
                    double complexBookTime = 0;
                    
                    if (Complexity.ToLower().Equals("complex"))
                    {
                        complexBookTime = ConfigurationManager.AppSettings["ComplexBookTime"] == "" ? 0 :
                                                Convert.ToDouble(ConfigurationManager.AppSettings["ComplexBookTime"]);
                    }

                    int PageCount = Convert.ToInt32(this.gvTaskManager.DataKeys[e.Row.RowIndex].Values[4]);
                    string OnePageTime_InSeconds = Convert.ToString(this.gvTaskManager.DataKeys[e.Row.RowIndex].Values[5]);

                    string userRank = Convert.ToString(Session["UserRank"]);
                    if (!string.IsNullOrEmpty(userRank))
                    {
                        string configKeyName = string.Empty;

                        if (userRank.Trim().Equals("Trainee Editor")) configKeyName = "TraineerEditorSalary";
                        else if (userRank.Trim().Equals("Junior Editor")) configKeyName = "JuniorEditorSalary";
                        else if (userRank.Trim().Equals("Editor")) configKeyName = "EditorSalary";
                        else if (userRank.Trim().Equals("Senior Editor")) configKeyName = "SeniorEditorSalary";
                        else if (userRank.Trim().Equals("Expert Editor")) configKeyName = "ExpertEditorSalary";

                        //salary = ConfigurationManager.AppSettings[configKeyName] == "" ? 0 :
                        //                   Convert.ToDouble(ConfigurationManager.AppSettings[configKeyName]);

                        if (Complexity.ToLower().Equals("complex") && taskType.Equals("ErrorDetection") && Convert.ToString(Session["levelUpgraded"]).Equals("true"))
                        {
                            salary = Convert.ToDouble(ConfigurationManager.AppSettings[configKeyName]) +
                                     Convert.ToDouble(ConfigurationManager.AppSettings["ComplexBookAmount"]);
                        }
                        else
                        {
                            salary = ConfigurationManager.AppSettings[configKeyName] == "" ? 0 :
                                    Convert.ToDouble(ConfigurationManager.AppSettings[configKeyName]);
                        }
                    }

                    //double salary = ConfigurationManager.AppSettings["MonthlySalary"] == "" ? 0 : Convert.ToDouble(ConfigurationManager.AppSettings["MonthlySalary"]);
                    double workingDays = ConfigurationManager.AppSettings["WorkingDays"] == "" ? 0 : Convert.ToDouble(ConfigurationManager.AppSettings["WorkingDays"]); ;
                    double dailyRequiredTime = ConfigurationManager.AppSettings["DailyRequiredTime"] == "" ? 0 :
                        Convert.ToDouble(ConfigurationManager.AppSettings["DailyRequiredTime"]);

                    if (salary > 0)
                    {
                        double oneDaySalary = salary / workingDays;
                        double oneHourSalary = oneDaySalary / dailyRequiredTime;

                        double taskSalary = (oneHourSalary / 60) * ((Convert.ToDouble(OnePageTime_InSeconds) * PageCount) / 60);

                        if (Complexity.ToLower().Equals("complex") && !taskType.Equals("ErrorDetection"))
                            taskSalary = taskSalary * complexBookTime;

                        string countryName = Convert.ToString(Session["CountryName"]);
                        string currency = "";
                        double amount = 0;
                        double bonus = 0;

                        if (countryName != "")
                        {
                            if (countryName.Equals("pakistan"))
                            {
                                //currency = "Rs. " + Convert.ToString(Math.Round(taskSalary, 2));
                                currency = "Rs. " + Convert.ToString(Math.Round(taskSalary, 2));
                            }
                            else if (countryName.Equals("other"))
                            {
                                //currency = "$ " + Convert.ToString(Math.Round(taskSalary, 2) / 100);
                                amount = Math.Round(
                                    taskSalary / Convert.ToDouble(ConfigurationManager.AppSettings["AUD"]), 2);
                                currency = "AU $ " +
                                           Convert.ToString(
                                               Math.Round(
                                                   taskSalary / Convert.ToDouble(ConfigurationManager.AppSettings["AUD"]),
                                                   2));
                            }
                        }

                        lblAmount.Text = currency;

                        if (amount > 0)
                        {
                            if (amount < 1)
                                lblBonus.Text = "";

                            //else
                            //{
                            //    bonus = Math.Round(amount / 4, 2);

                            //    if (bonus < 1) lblBonus.Text = "AU $ 1*";
                            //    if (bonus >= 1) lblBonus.Text = "AU $ " + Convert.ToString(bonus) + "*";
                            //}
                            if (Complexity.ToLower().Equals("complex") &&
                                   taskType.Equals("ErrorDetection") &&
                                   Convert.ToString(Session["levelUpgraded"]).Equals("true"))
                            {
                                //bonus for complex task is 37.5%
                                //bonus = 37.5 * Math.Round(amount, 2) / 100;

                                //if (bonus < 1) lblBonus.Text = "AU $ 1.5*";
                                //if (bonus >= 1) lblBonus.Text = "AU $ " + Convert.ToString(bonus) + "*";

                                bonus = Math.Round(amount / 4, 2);

                                if (bonus < 1) lblBonus.Text = "AU $ 1*";
                                if (bonus >= 1) lblBonus.Text = "AU $ " + Convert.ToString(bonus) + "*";
                            }
                            else
                            {
                                bonus = Math.Round(amount / 4, 2);

                                if (bonus < 1) lblBonus.Text = "AU $ 1*";
                                if (bonus >= 1) lblBonus.Text = "AU $ " + Convert.ToString(bonus) + "*";
                            }

                        }
                        else
                        {
                            lblBonus.Text = "";
                        }
                        //lblTaskTime.Text = Convert.ToString(Convert.ToDouble(OnePageTime_InSeconds) * PageCount / 60 / 60);
                    }
                }
                else
                {
                    lblAmount.Text = "";
                    lblTaskTime.Text = "";
                    lblBonus.Text = "";
                }
            }
        }

        protected void gvMyTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");
                Label lblAmount = (Label)e.Row.FindControl("lblAmount");
                Label lblTaskTime = (Label)e.Row.FindControl("lblTaskTime");
                Label lblBonus = (Label)e.Row.FindControl("lblBonus");
                Label lblStatus = (Label)e.Row.FindControl("lblStatus");
                ImageButton ImgSubmitTask = (ImageButton)e.Row.FindControl("ImgSubmitTask");

                lblSrNo.Text = Convert.ToString(e.Row.RowIndex + 1) + ".";

                if (lblStatus.Text.Trim().ToLower().Equals("Pending Confirmation"))
                {
                    ImgSubmitTask.Enabled = false;
                }

                string complexity = Convert.ToString(gvMyTasks.DataKeys[e.Row.RowIndex].Values[5]);

                if (!string.IsNullOrEmpty(complexity) && complexity.ToLower().Trim().Equals("complex"))
                {
                    e.Row.BackColor = System.Drawing.Color.Turquoise;
                }

                string taskType = Convert.ToString(this.gvMyTasks.DataKeys[e.Row.RowIndex].Values[2]);
                double salary = 0;

                if ((taskType.Equals("Image")) || (taskType.Equals("Index")) || (taskType.Equals("Table")) || (taskType.Equals("ErrorDetection")))
                {
                    string Complexity = Convert.ToString(this.gvMyTasks.DataKeys[e.Row.RowIndex].Values[5]);
                    int PageCount = Convert.ToInt32(this.gvMyTasks.DataKeys[e.Row.RowIndex].Values[6]);
                    string OnePageTime_InSeconds = Convert.ToString(this.gvMyTasks.DataKeys[e.Row.RowIndex].Values[7]);

                    double complexBookTime = 0;

                    if (Complexity.ToLower().Equals("complex"))
                    {
                        complexBookTime = ConfigurationManager.AppSettings["ComplexBookTime"] == "" ? 0 :
                                                Convert.ToDouble(ConfigurationManager.AppSettings["ComplexBookTime"]);
                    }

                    string userRank = Convert.ToString(Session["UserRank"]);
                    if (!string.IsNullOrEmpty(userRank))
                    {
                        string configKeyName = string.Empty;

                        if (userRank.Trim().Equals("Trainee Editor")) configKeyName = "TraineerEditorSalary";
                        else if (userRank.Trim().Equals("Junior Editor")) configKeyName = "JuniorEditorSalary";
                        else if (userRank.Trim().Equals("Editor")) configKeyName = "EditorSalary";
                        else if (userRank.Trim().Equals("Senior Editor")) configKeyName = "SeniorEditorSalary";
                        else if (userRank.Trim().Equals("Expert Editor")) configKeyName = "ExpertEditorSalary";

                        if (Complexity.ToLower().Equals("complex") && taskType.Equals("ErrorDetection") && Convert.ToString(Session["levelUpgraded"]).Equals("true"))
                        {
                            salary = Convert.ToDouble(ConfigurationManager.AppSettings[configKeyName]) +
                                     Convert.ToDouble(ConfigurationManager.AppSettings["ComplexBookAmount"]);
                        }
                        else
                        {
                            salary = ConfigurationManager.AppSettings[configKeyName] == "" ? 0 :
                                    Convert.ToDouble(ConfigurationManager.AppSettings[configKeyName]);
                        }
                    }

                    //double salary = ConfigurationManager.AppSettings["MonthlySalary"] == "" ? 0 : Convert.ToDouble(ConfigurationManager.AppSettings["MonthlySalary"]);
                    double workingDays = ConfigurationManager.AppSettings["WorkingDays"] == "" ? 0 : Convert.ToDouble(ConfigurationManager.AppSettings["WorkingDays"]); ;
                    double dailyRequiredTime = ConfigurationManager.AppSettings["DailyRequiredTime"] == "" ? 0 :
                        Convert.ToDouble(ConfigurationManager.AppSettings["DailyRequiredTime"]);

                    if (salary > 0)
                    {
                        double oneDaySalary = salary / workingDays;
                        double oneHourSalary = oneDaySalary / dailyRequiredTime;

                        double taskSalary = (oneHourSalary / 60) * ((Convert.ToDouble(OnePageTime_InSeconds) * PageCount) / 60);

                        if (Complexity.ToLower().Equals("complex") && !taskType.Equals("ErrorDetection"))
                            taskSalary = taskSalary * complexBookTime;

                        string countryName = Convert.ToString(Session["CountryName"]);
                        string currency = "";
                        double amount = 0;
                        double bonus = 0;

                        if (countryName != "")
                        {
                            if (countryName.Equals("pakistan"))
                            {
                                //currency = "Rs. " + Convert.ToString(Math.Round(taskSalary, 2));
                                currency = "Rs. " + Convert.ToString(Math.Round(taskSalary, 2));
                            }
                            else if (countryName.Equals("other"))
                            {
                                //currency = "$ " + Convert.ToString(Math.Round(taskSalary, 2) / 100);
                                currency = "AU $ " +
                                           Convert.ToString(
                                               Math.Round(
                                                   taskSalary / Convert.ToDouble(ConfigurationManager.AppSettings["AUD"]),
                                                   2));
                                amount = Math.Round(
                                    taskSalary / Convert.ToDouble(ConfigurationManager.AppSettings["AUD"]), 2);
                            }
                        }

                        lblAmount.Text = currency;
                        if (amount > 0)
                        {
                            if (amount < 1)
                                lblBonus.Text = "";

                            else
                            {
                                if (Complexity.ToLower().Equals("complex") && 
                                    taskType.Equals("ErrorDetection") &&
                                    Convert.ToString(Session["levelUpgraded"]).Equals("true"))
                                {
                                    //bonus = 37.5 * Math.Round(amount, 2)/100;

                                    //if (bonus < 1) lblBonus.Text = "AU $ 1.5*";
                                    //if (bonus >= 1) lblBonus.Text = "AU $ " + Convert.ToString(bonus) + "*";

                                    bonus = Math.Round(amount / 4, 2);

                                    if (bonus < 1) lblBonus.Text = "AU $ 1*";
                                    if (bonus >= 1) lblBonus.Text = "AU $ " + Convert.ToString(bonus) + "*";
                                }
                                else
                                {
                                    bonus = Math.Round(amount / 4, 2);

                                    if (bonus < 1) lblBonus.Text = "AU $ 1*";
                                    if (bonus >= 1) lblBonus.Text = "AU $ " + Convert.ToString(bonus) + "*";
                                }
                                
                            }
                        }
                        else
                        {
                            lblBonus.Text = "";
                        }
                        //lblTaskTime.Text = Convert.ToString(Convert.ToDouble(OnePageTime_InSeconds) * PageCount / 60 / 60);
                    }
                }
                else
                {
                    lblAmount.Text = "";
                    lblTaskTime.Text = "";
                    lblBonus.Text = "";
                }
            }
        }

        protected void gvArchiveTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label lblSrNo = (Label)e.Row.FindControl("lblSrNo");
                Label lblAmount = (Label)e.Row.FindControl("lblAmount");
                Label lblBonus = (Label)e.Row.FindControl("lblBonus");
                //Label lblTaskTime = (Label)e.Row.FindControl("lblTaskTime");

                lblSrNo.Text = Convert.ToString(e.Row.RowIndex + 1) + ".";

                if (!string.IsNullOrEmpty(lblAmount.Text)) lblAmount.Text = "AU $ " + lblAmount.Text;
                if (!string.IsNullOrEmpty(lblBonus.Text))
                {
                    if (!lblBonus.Text.Equals("0"))
                        lblBonus.Text = "AU $ " + lblBonus.Text;
                    else
                        lblBonus.Text = " ";
                }
            }
        }

        protected void gvCompletedTasks_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                LinkButton lblMark = (LinkButton)e.Row.FindControl("lbtnTask");
                Label FeedBackDate = (Label)e.Row.FindControl("lblFeedBackDate");
                Label lblComments = (Label)e.Row.FindControl("lblComments");

                string BookId = Convert.ToString(gvCompletedTasks.DataKeys[e.Row.RowIndex].Values[0]);
                string TestType = Convert.ToString(gvCompletedTasks.DataKeys[e.Row.RowIndex].Values[1]);
                string TimelyDelivery = Convert.ToString(gvCompletedTasks.DataKeys[e.Row.RowIndex].Values[2]);
                string Quality = Convert.ToString(gvCompletedTasks.DataKeys[e.Row.RowIndex].Values[3]);
                string Responsiveness = Convert.ToString(gvCompletedTasks.DataKeys[e.Row.RowIndex].Values[4]);
                string Comments = Convert.ToString(gvCompletedTasks.DataKeys[e.Row.RowIndex].Values[5]);

                lblMark.Text = Convert.ToString(":: ") + BookId + " - " + TestType;
                if (!string.IsNullOrEmpty(Comments))
                {
                    if (Comments.Length > 20) lblComments.Text = Comments.Substring(0, 20) + " . . .";
                    else lblComments.Text = Comments;
                }

                if (!string.IsNullOrEmpty(FeedBackDate.Text)) FeedBackDate.Text = Convert.ToDateTime(FeedBackDate.Text).ToString("Y");

                //e.Row.ForeColor = System.Drawing.Color.Gray;   
            }
        }

        protected void lnkDownloadgvTaskManager_Click(object sender, EventArgs e)
        {
            try
            {
                MyDBClass objMyDBClass = new MyDBClass();
                int index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
                string Id = (string)this.gvTaskManager.DataKeys[index].Values[0];
                Label taskType = (Label)gvTaskManager.Rows[index].FindControl("lblIssueCategory");
                string bookID = Id;
                string querySelBook = "Select MainBook from BOOK Where BID=" + bookID;
                string MainBook = objMyDBClass.GetID(querySelBook);

                string TaskName = Convert.ToString(gvTaskManager.DataKeys[index].Values[2]);
                string path = "";

                Context.Response.Clear();

                if (TaskName.ToLower().Trim().Equals("table"))
                {
                    path = System.Configuration.ConfigurationManager.AppSettings["MainDirPhyPath"] + "/" + MainBook +
                           "/" + MainBook + "-1/Table/Table.zip";
                    Context.Response.ContentType = "application/x-zip-compressed";
                }

                else if (TaskName.ToLower().Trim().Equals("image"))
                {
                    path = System.Configuration.ConfigurationManager.AppSettings["MainDirPhyPath"] + "/" + MainBook +
                          "/" + MainBook + ".pdf";
                    Context.Response.ContentType = "application/pdf";
                }

                else if (TaskName.ToLower().Trim().Equals("index"))
                {
                    //path = System.Configuration.ConfigurationManager.AppSettings["MainDirPhyPath"] + "/" + MainBook +
                    //       "/" + MainBook + "-1/Index/Index.zip";
                    path = System.Configuration.ConfigurationManager.AppSettings["MainDirPhyPath"] + "/" + MainBook +
                          "/" + MainBook + "-1/Index/" + MainBook + "-1" + ".pdf";
                    Context.Response.ContentType = "application/pdf";
                }

                else if (TaskName.ToLower().Trim().Equals("qainspection"))
                {
                    path = System.Configuration.ConfigurationManager.AppSettings["MainDirPhyPath"] + "/" + MainBook + "/" + MainBook + ".rhyw";
                    Context.Response.ContentType = "application/octet-stream";

                    string link = "<a class='bbw' href='" + path + "'>RHYW</a>";
                }

                if (TaskName.ToLower().Trim().Equals("qainspection"))
                {
                    Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + TaskName + ".rhyw");
                }
                else
                {
                    Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + TaskName);
                }

                Context.Response.WriteFile(path);
                Context.Response.End();
            }
            catch (Exception ex)
            {

                //showMessage(ex.Message);
            }
        }

        protected void lnkDownloadgvMyTasks_Click(object sender, EventArgs e)
        {
            try
            {
                MyDBClass objMyDBClass = new MyDBClass();
                int index = ((GridViewRow)((LinkButton)sender).Parent.Parent).RowIndex;
                string bookID = (string)this.gvMyTasks.DataKeys[index].Values[4];
                Label taskType = (Label)gvMyTasks.Rows[index].FindControl("lblIssueCategory");

                string TaskName = Convert.ToString(gvMyTasks.DataKeys[index].Values[2]);
                string path = "";

                if (TaskName.ToLower().Trim().Equals("table"))
                {
                    path = System.Configuration.ConfigurationManager.AppSettings["MainDirPhyPath"] + "/" + bookID +
                           "/" + bookID + "-1/Table/Table.zip";
                }

                else if (TaskName.ToLower().Trim().Equals("image"))
                {
                    path = System.Configuration.ConfigurationManager.AppSettings["MainDirPhyPath"] + "/" + bookID +
                          "/" + bookID + "-1/Image/Image.zip";
                }

                else if (TaskName.ToLower().Trim().Equals("index"))
                {
                    path = System.Configuration.ConfigurationManager.AppSettings["MainDirPhyPath"] + "/" + bookID +
                           "/" + bookID + "-1/Index/Index.zip";
                }

                else if (TaskName.ToLower().Trim().Equals("qainspection"))
                {
                    path = System.Configuration.ConfigurationManager.AppSettings["MainDirPhyPath"] + "/" + bookID + "/" + bookID + ".rhyw";
                }

                Context.Response.Clear();

                if (TaskName.ToLower().Trim().Equals("qainspection"))
                {
                    Context.Response.ContentType = "application/octet-stream";
                    Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + TaskName + ".rhyw");
                }
                else
                {
                    Context.Response.ContentType = "application/x-zip-compressed";
                    Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + TaskName);
                }

                Context.Response.WriteFile(path);
                Context.Response.End();
            }
            catch (Exception ex)
            {

                //showMessage(ex.Message);
            }
        }

        #region |Task Assign Events Details|


        MyDBClass objMyDBClass = new MyDBClass();



        protected void lnkLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Response.Redirect("BookMicro.aspx");
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            AssignMe.Visible = false;
        }

        protected void lbtnTask_Click(object sender, EventArgs e)
        {
            Response.Redirect("WorkHistory.aspx", true);
        }

        protected void btnAssign_Click(object sender, EventArgs e)
        {
            string bookID = Request.QueryString["bid"] != null ? Request.QueryString["bid"].ToString() : "";
            if (bookID == "" && (Session["bid"] != null))
            {
                bookID = Session["bid"].ToString();
            }

            string process = ProcessControl1.getSelectedItems().Replace(":Qa", "");


            //Mail
            //objMyDBClass.SendMail(process.ToUpper(), Convert.ToString(Session["LoginId"]), (Session["objUser"] as UserClass));
            //End Mail

            //////string queryInsert = "Update ACTIVITY Set Cost=" + this.lblRate.Text.Split(new char[] { ' ' })[3] + ",UID=" + Convert.ToString(Session["LoginId"]) + ",AssigmentDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "',DeadLine=Null,Status='Working',Comments='" + this.txtComments.Text + "' Where BID=" + bookID + "and task like '" + process + "%'";
            //////int inResult = objMyDBClass.ExecuteCommand(queryInsert);
            /**********************/

            //string qTaskPayment = "Select C.[" + process + "] From UserCategory C inner join [User] U on C.CID=U.CID Where U.UID=" + UID.Value;
            //string taskPayment = objMyDBClass.GetID(qTaskPayment);

            //string earningQuery = "Insert into Earnings(UID,AID,ActualEarning,PayableEarning,Bonus,Paid) Values(" + UID.Value + "," + Request.QueryString["aid"].ToString() + "," + taskPayment + "," + taskPayment + ",0.00,'N')";
            //objMyDBClass.ExecuteCommand(earningQuery);
            /**********************/


            //Update status of a task
            MyDBClass objMyDBClass = new MyDBClass();
            int effectedRows = objMyDBClass.UpdateTaskStatus(bookID, process, Convert.ToDouble(this.lblRate.Text.Split(new char[] { ' ' })[3]),
                                                                        Convert.ToInt32(Session["LoginId"]), DateTime.Now.Date.GetDateTimeFormats('d')[5],
                                                                         "Working", "");

            ////if (effectedRows > 0)
            ////{
            //    string querySelBook = "Select MainBook from BOOK Where BID=" + bookID;
            //    string MainBook = objMyDBClass.GetID(querySelBook);


            //    //process = process.First().ToString().ToUpper() + process.Remove(0, 1).ToLower();
            //    //string path = System.Configuration.ConfigurationManager.AppSettings["MainDirPhyPath"] + "/" + MainBook + "/" + MainBook + "-1" +
            //    //              "/" + process + "/" + MainBook + ".pdf";

            //    string path = System.Configuration.ConfigurationManager.AppSettings["MainDirPhyPath"] + "/" + MainBook +
            //                  "/" + process + "/" + MainBook + ".pdf";

            //    Context.Response.Clear();
            //    Context.Response.ContentType = "application/pdf";
            //    Context.Response.AddHeader("Content-Disposition", "attachment; filename=" + MainBook + ".pdf");
            //    Context.Response.WriteFile(path);
            //    //Response.Redirect("ImageTest.aspx");
            //    Context.Response.End();
            ////}

            AssignMe.Visible = false;

            BindAvailableTaskGrid();
            BindMyTaskGrid();
        }

        protected void TaskDetails()
        {
            string task = "Comparison";
            string uQuery = "SELECT UserCategory.[" + task + "] FROM [User] INNER JOIN UserCategory ON [User].CID = UserCategory.CID WHERE [User].UID=90";
            string rate = objMyDBClass.GetID(uQuery);
            string qCount = "Select [Count] From Activity Where AID=2623";
            double count = double.Parse(objMyDBClass.GetID(qCount));
            count = (count == 0 ? 1 : count);
            this.lblRate.Text = "Estimated Amount : " + rate + " x " + count + "=" + (double.Parse(rate) * count) + " <sup><font color='red'>*</font></sup>";
            this.instruction.Visible = true;
        }

        protected void btnTestTraining_Click(object sender, EventArgs e)
        {
            Response.Redirect("Training.aspx", false);
        }

        protected void lbtnArchiveTasks_Click(object sender, EventArgs e)
        {
            pnlUserTasks.Visible = false;
            BindArchiveTasks();
            pnlArchive.Visible = true;
        }

        private void BindArchiveTasks()
        {
            MyDBClass objMyDBClass = new MyDBClass();
            List<OnlineTest> list_tasks = objMyDBClass.GetArchiveTasks(Convert.ToString(Session["LoginId"]));
            gvArchiveTasks.DataSource = list_tasks;
            gvArchiveTasks.DataBind();
        }

        #endregion
    }
}
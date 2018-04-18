using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

using System.Drawing;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using Outsourcing_System.MasterPages;
using BookMicroBeta;
using Outsourcing_System;
using System.Data.SqlClient;
using Outsourcing_System.PdfCompare_Classes;

public partial class AdminPanel : System.Web.UI.Page
{
    MyDBClass objMyDBClass = new MyDBClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        //((AdminMaster)this.Page.Master).SetLogOut = true;
        //((AdminMaster)this.Page.Master).SetMenuLocation = "-20px";

        //((AdminMaster)this.Page.Master).ShowLogOutButton();

        if (Session["objUser"] == null)
        {
            //if (!Page.IsPostBack)
            //{
            //if (Request.QueryString["id"] != null)
            //{
            string email = CommonClass.Decrypt(HttpUtility.UrlDecode(Request.QueryString["id"]));
            string password = CommonClass.Decrypt(HttpUtility.UrlDecode(Request.QueryString["p"]));
            string userType = CommonClass.Decrypt(HttpUtility.UrlDecode(Request.QueryString["t"]));

            GetLoginvariables(email, password, userType);

            MyDBClass objMyDBClass = new MyDBClass();
            if (Request.Url.OriginalString.Contains("http://192.168.0.200:91/"))
            {
                objMyDBClass.MainDirectory = ConfigurationManager.AppSettings["MainDirectory"].ToString();
            }
            else
            {
                objMyDBClass.MainDirectory = ConfigurationManager.AppSettings["LiveMainDirectory"].ToString();
            }
            Session["MainDirectory"] = objMyDBClass.MainDirectory;

            //}
            //}
            //this.Title = "Outsourcing System :: Admin Panel";
            this.lblMessage.Text = "";
            //this.Master.ShowMessageBox("", "NoMessage");
            //if (Session["objUser"] == null || (Session["objUser"] as UserClass).UserType != "admin")
            //{
            //    //Response.Redirect("Login.aspx");
            //}
            //else 
            ////if (Request.QueryString["act"] != null)
            ////{
            ////    string action = Request.QueryString["act"].ToString();
            ////    string aid = Request.QueryString["aid"].ToString();

            ////    //=====================================================
            ////    if (action == "rtd")
            ////    {
            ////        this.pnlComments.Visible = true;
            ////        this.uplComments.Update();
            ////    }
            ////    else if (action == "cfm" && Session["accountpanel"] == null || !Session["accountpanel"].ToString().StartsWith("visible"))
            ////    {
            ////        string qAmount = "Select Cost,[Count] from Activity where AID=" + aid;
            ////        DataSet dsAmount = objMyDBClass.GetDataSet(qAmount);
            ////        DataRow dr = dsAmount.Tables[0].Rows[0];
            ////        double amount = double.Parse(dr["Cost"].ToString());
            ////        int count = int.Parse(dr["Count"].ToString());
            ////        amount = count * amount;
            ////        this.txtPayableAmount.Text = amount.ToString();
            ////        this.txtBonus.Text = "0.00";
            ////        pnlAmount.Visible = true;
            ////        this.uplAccount.Update();
            ////        Session["accountpanel"] = "visible@" + amount;
            ////    }
            ////    //=====================================================
            ////}
        }
        if (!Page.IsPostBack)
        {
            if (Request.QueryString["act"] != null)
            {
                string action = Request.QueryString["act"].ToString();
                string aid = Request.QueryString["aid"].ToString();

                //=====================================================
                if (action == "rtd")
                {
                    this.pnlComments.Visible = true;
                    this.uplComments.Update();
                }
                //else if (action == "cfm" && Session["accountpanel"] == null || !Session["accountpanel"].ToString().StartsWith("visible"))
                else if (action == "cfm")
                {
                    //string qAmount = "Select Cost,[Count] from Activity where AID=" + aid;
                    string qAmount = "Select Cost, CompletionDate from Activity where AID=" + aid;
                    DataSet dsAmount = objMyDBClass.GetDataSet(qAmount);
                    DataRow dr = dsAmount.Tables[0].Rows[0];
                    double amount = double.Parse(dr["Cost"].ToString());
                    string completionDate = Convert.ToString(dr["CompletionDate"]);
                    //int count = int.Parse(dr["Count"].ToString());
                    //amount = count * amount;
                    //txtPayableAmount.Text = "AU $ " + Convert.ToString(amount);
                    txtPayableAmount.Text = Convert.ToString(amount);

                    if (amount > 0)
                    {
                        string qBookTable = "Select b.Complexity, b.PageCount, b.BIdentityNo, a.UID from Book b inner join Activity a on b.BID = a.BID where a.AID=" + aid;
                        DataSet dsBookTable = objMyDBClass.GetDataSet(qBookTable);
                        DataRow drBookTable = dsBookTable.Tables[0].Rows[0];
                        string complexity = Convert.ToString(drBookTable["Complexity"]);
                        string bookId = Convert.ToString(drBookTable["BIdentityNo"]);
                        Session["BIdentityNo"] = bookId;
                        string userId = Convert.ToString(drBookTable["UID"]);
                        double pageCount = Convert.ToDouble(drBookTable["PageCount"]);

                        double bonus = CalculateBonus(complexity, pageCount, amount, completionDate, bookId, userId);
                        //if (bonus > 0) txtBonus.Text = "AU $ " + bonus;
                        if (bonus > 0) txtBonus.Text = bonus + "";
                    }

                    pnlAmount.Visible = true;
                    this.uplAccount.Update();
                    Session["accountpanel"] = "visible@" + amount;
                }
                //=====================================================
            }

            string query = "Select * from Process";
            DataSet ds = objMyDBClass.GetDataSet(query);
            //if (ds.Tables.Count > 0)
            //{
            //    if(ds.Tables[0].Rows.Count>0)
            //    {
            this.ddProcess.DataSource = ds.Tables[0];
            this.ddProcess.DataTextField = "PName";
            this.ddProcess.DataValueField = "PID";
            this.ddProcess.DataBind();

            this.ddProcess.Items.Add("All");
            this.ddProcess.SelectedIndex = this.ddProcess.Items.Count - 1;
            ShowDataInGridView();
            //    }
            //}
        }
    }

    private double CalculateBonus(string complexity, double pageCount, double amount, string completionDate, string bookId, string userId)
    {
        if (!string.IsNullOrEmpty(complexity) && pageCount > 0 && amount > 0)
        {
            if (IsBonusValid(complexity, pageCount, completionDate, bookId, userId))
            {
                if (amount / 4 < 1) return 1;
                if (amount / 4 >= 1) return amount / 4;
            }
        }

        return 0;
    }
    private bool IsBonusValid(string complexity, double pageCount, string completionDate, string bookId, string userId)
    {
        double dailyRequiredTime = 6.0;

        string temp = objMyDBClass.CalculateTaskTime(complexity);
        string[] splitedoutput = temp.Split(',');
        double onePageTimeInSec = Convert.ToDouble(splitedoutput[0]);
        double bookUnitTime = Convert.ToDouble(splitedoutput[1]);

        //Formula from workmeter expectedHours For a book = (processedPages * onePageTimeInSec) / (sec in 1 hour) + bookunittime;
        double expectedBookCompHours = ((pageCount * onePageTimeInSec) / 3600) + bookUnitTime;
        double bonusTime = Math.Round((expectedBookCompHours + 2) / dailyRequiredTime, 2);

        string taskStartingDate = objMyDBClass.GetTaskStartingTime(bookId, userId);

        if (!string.IsNullOrEmpty(taskStartingDate) && !string.IsNullOrEmpty(completionDate))
        {
            var test = ((Convert.ToDateTime(completionDate) - Convert.ToDateTime(taskStartingDate)).TotalDays < bonusTime);

            return ((Convert.ToDateTime(completionDate) - Convert.ToDateTime(taskStartingDate)).TotalDays < bonusTime);
        }

        return false;
    }

    public void GetLoginvariables(string email, string password, string userType)
    {
        string queryPayableEarning = "Select UserName,Password From [User] where Email='" + email + "' and Password='" + password + "' and UType='" + userType + "' AND IsActive='1'";
        DataSet dsPayableEarning = objMyDBClass.GetDataSet(queryPayableEarning);
        string userName = "";
        string userPassword = "";

        if (dsPayableEarning.Tables[0].Rows.Count > 0)
        {
            userName = dsPayableEarning.Tables[0].Rows[0]["UserName"].ToString();
            userPassword = dsPayableEarning.Tables[0].Rows[0]["Password"].ToString();
        }

        string query = "Select * from [User] where UserName='" + userName.Trim() + "' AND Password='" + userPassword.Trim() + "' AND UType='" + userType + "' AND IsActive='1'";
        DataSet ds = objMyDBClass.GetDataSet(query);
        if (ds.Tables[0].Rows.Count > 0)
        {
            UserClass objUser = new UserClass();
            objUser.UserID = ds.Tables[0].Rows[0]["UID"].ToString();
            objUser.UserName = userName.Trim();
            objUser.UserType = ds.Tables[0].Rows[0]["UType"].ToString();
            objUser.UserFullName = ds.Tables[0].Rows[0]["UNAME"].ToString();
            objUser.UserEmail = ds.Tables[0].Rows[0]["Email"].ToString();
            Session["objUser"] = objUser;
            Session["DBObj"] = objMyDBClass;
        }
    }

    public string GetNullValue(string count)
    {
        if (string.IsNullOrEmpty(count)) return "";

        string retVal = count;
        if (int.Parse(count) == 0)
        {
            retVal = "N/A";
        }
        return retVal;
    }
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    //Create Comparison Task after the completion of tagging untagging on all parts then 
    //merge it. Comparison and Error Ajustment will work on merged file
    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++

    protected void FinalizationProcess()
    {
        string queryBID = "Select BID From Activity where AID=" + Request.QueryString["aid"].ToString();
        string BID = objMyDBClass.GetID(queryBID);

        if (BID != "")
        {
            //Calling Service
            Outsourcing_System.AutoMapService.AutoMappService autoMapSvc = new Outsourcing_System.AutoMapService.AutoMappService();
            Outsourcing_System.Index_Service.IndexService indexSvc = new Outsourcing_System.Index_Service.IndexService();

            //string mainFile = Server.MapPath("~/Files/" + BID + "/" + BID);
            string mainFile = objMyDBClass.MainDirPhyPath + "/" + BID + "/" + BID;

            string status = autoMapSvc.InsertVolumeBreaks(mainFile + ".rhyw");
            //string xlsFile = Server.MapPath("~/Files/" + BID + "/" + BID + "-3/Index/" + BID + "-3.xls");
            string xlsFile = objMyDBClass.MainDirPhyPath + "/" + BID + "/" + BID + "-3/Index/" + BID + "-3.xls";

            if (File.Exists(xlsFile) && status == "Successfull")
            {
                //Calling Service
                objMyDBClass.LogEntry(BID, "Index", "We are going to attach Index", "In Progress", "update");
                status = indexSvc.AttachRHYWIndex(mainFile + ".rhyw", xlsFile);
                objMyDBClass.LogEntry(BID, "Index", "Index is attached successfully", "Completed", "update");
            }
            if (status == "Successfull")
            {
                objMyDBClass.LogEntry(BID, "Finalizing", "Finalizing the Main Book", "In Progress", "insert");
                try
                {
                    //Calling Service 
                    objMyDBClass.LogEntry(BID, "Finalizing", "Main Book is finalized successfully", "Completed", "update");
                    status = autoMapSvc.FinalizeBook(mainFile + ".rhyw");
                    this.lblMessage.Text = "Book # " + BID + " Completed Successfully";
                    //Mail
                    objMyDBClass.SendMail(BID, (Session["objUser"] as UserClass).UserID, (Session["objUser"] as UserClass));
                    Session["rhywPath"] = mainFile + "-Final.rhyw";
                }
                catch
                {
                    objMyDBClass.LogEntry(BID, "Error", "Unable to finalize the book due to error in xml", "Completed", "insert");
                    Session["rhywPath"] = mainFile + ".rhyw";
                }
                finally
                {
                    string queryUID = "Select UID From Activity where BID=" + BID;
                    string UID = objMyDBClass.GetID(queryUID);

                    string queryInsert = "Insert into ACTIVITY(UID,BID,AssignedBy,Status,Task) VALUES(" + UID + "," + BID + ",'" + (Session["objUser"] as UserClass).UserName + "','Working','Meta')";
                    int inResult = objMyDBClass.ExecuteCommand(queryInsert);
                    objMyDBClass.LogEntry(BID, "Meta", "Task is Assigned", "In Progress", "insert");
                    this.lblMessage.Text = "Congratulation! Book is completed successfully. Meta is assigned";
                }
            }
        }
    }

    //+++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
    protected void lnkApproved_Click(object sender, EventArgs e)
    {
        Response.Redirect("ApprovedTask.aspx");
    }
    //protected void lnkLogout_Click(object sender, EventArgs e)
    //{
    //    Session.Clear();
    //    Response.Redirect("Login.aspx");
    //}
    protected void lnkAddUser_Click(object sender, EventArgs e)
    {
        Response.Redirect("Signup.aspx");
    }

    protected void lnkMessage_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/MessageBoard.aspx?act=inbox");
    }

    //Creating Link for the patrh of source file uploaded by user
    #region string DownloadLink(string bookID, string Process,string completionDate)
    public string DownloadLink(string bookID, string Process, string completionDate, string status)
    {
        string ext = "", directory = "";
        string filePath = "";

        if (status.ToLower() == "unassigned")
        {
            directory = "TaggingUntagged";
            ext = ".pdf";
            if (Process.ToLower() == "index")
            {
                directory = "Index";
            }
        }
        else
        {
            if (status.ToLower() == "pending confirmation" || status.ToLower() == "approved")
            {
                if (Process.ToLower() == "image" || Process.ToLower() == "table")
                {
                    directory = Process;
                    ext = ".zip";
                }
                else if (Process.ToLower() == "tagginguntagged" || Process.ToLower() == "complexbitsmapping")
                {
                    directory = Process;
                    ext = ".rhyw";
                }
                else if (Process.ToLower() == "index")
                {
                    directory = Process;
                    ext = ".xls";
                }
            }
            else
            {
                if (Process.ToLower() == "tagginguntagged")
                {
                    directory = Process;
                    ext = ".rhyw";
                }
                else if (Process.ToLower() == "erroradjustment" && status.ToLower() == "unassigned")
                {
                    directory = "TaggingUntagged";
                    ext = ".rhyw";
                }
                //else if (Process.ToLower() == "comparison")
                //{
                //    directory = Process;
                //    ext = "_1.pdf";
                //}
                else if (Process.ToLower() == "table" || Process.ToLower() == "image")
                {
                    directory = "TaggingUntagged";
                    ext = ".pdf";
                }

            }
        }
        string link = "";
        if (status.ToLower() == "pending confirmation" || status.ToLower() == "approved" || status.ToLower() == "working")
        {
            if (Process.ToLower() == "table" || Process.ToLower() == "image")
            {
                Process = "TaggingUntagged";
            }
            if (Process.ToLower() == "comparison" || Process.ToLower() == "erroradjustment" || Process.ToLower() == "meta" )
            {
                link += "<a class='bbw' href='" + Session["MainDirectory"].ToString() + "/" + bookID + "/" + bookID + ".rhyw'>RHYW</a>";
            }
            if (Process.ToLower() == "complexbitsmapping" && status.ToLower() == "approved")
            {
                //link += "<a class='bbw' href='" + Session["MainDirectory"].ToString() + "/" + bookID + "/" + bookID + ".rhyw'>RHYW</a>";

                //filePath = Convert.ToString(Session["MainDirectory"]) + "/" + bookID.Split(new char[] { '-' })[0] + "/" +
                //               bookID + "/TaggingUntagged/" + bookID + ".rhyw";
                //if (File.Exists(filePath))
                //{ 
                    //link += "<a class='bbw' href='" + Session["MainDirectory"].ToString() + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID
                    //    + "/TaggingUntagged/" + bookID + ".rhyw'>RHYW</a>";
                link += "<a class='bbw' href='" + Session["MainDirectory"].ToString() + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID
                        + "/TaggingUntagged/" + bookID + "_structoolcopy.rhyw'>RHYW</a>";
                //}
            }
            else
            {
                link += "<a class='bbw' href='" + Session["MainDirectory"].ToString() + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID + "/" + Process + "/" + bookID + ".pdf'>PDF</a>";
            }
        }
        if (Process.ToLower() == "comparison" || Process.ToLower() == "erroradjustment" || Process.ToLower() == "meta")
        {
            link += "&nbsp;<a class='bbw' href='" + Session["MainDirectory"].ToString() + "/" + bookID + "/" + bookID + ".pdf'>PDF</a>";
        }
        else if (Process.ToLower() == "table")
        {
            //link += "&nbsp;<a class='bbw' href='" + Session["MainDirectory"].ToString() + "/" + bookID.Split(new char[] { '-' })[0] + "/Tables.zip'>Zip</a>";
            link += "&nbsp;<a class='bbw' href='" + Session["MainDirectory"].ToString() + "/" + bookID.Split(new char[] { '-' })[0] + "/Tables.zip'></a>";
        }
        else
        {
            //link += "&nbsp;<a class='bbw' href='" + Session["MainDirectory"].ToString() + "/" + bookID.Split(new char[] { '-' })[0] + "/" + bookID + "/" + directory + "/" + bookID + ext + "'>" + ext.ToUpper().Replace(".", "") + "</a>";
        }
        return link;
    }
    #endregion

    //Creating link for Approving / Rejecting Pending Tasks
    #region string LinkCreation(string status,string AID, string type)
    public string LinkCreation(string task, string status, string AID, string type)
    {
        string link = "";
        if (type == "edit")
        {
            link = "<a class='link' href='UpdateStatus.aspx?aid=" + AID + "&task=" + task + "&status=" + status + "'>" + status + "</a>";
        }
        else if (status.StartsWith("Pending"))
        {
            if (type == "cfm")
            {
                link = "<a class='link' href='AdminPanel.aspx?act=cfm&aid=" + AID + "&task=" + task + "'>Approve</a>";
            }
            else
            {
                link = "<a class='link' href='AdminPanel.aspx?act=rtd&aid=" + AID + "&task=" + task + "'>Reject</a>";
            }
        }
        return link;
    }
    #endregion

    //Creating Link for Assigning Task
    #region string AssignTaskLink(string status, string BID, string task,string aid)
    public string AssignTaskLink(string status, string BID, string task, string aid, string pages)
    {
        string link = "";
        int volumeBreakPages = Convert.ToInt32(objMyDBClass.VolumBreakPages);
        int bookPages = pages.Equals("") ? 0 : Convert.ToInt32(pages);
        if (status == "Unassigned")
        {
            //link = "<a class='link' href='AssignTask.aspx?aid=" + aid + "&bid=" + BID + "&pname=" + task + "'>Assign Task</a>";
            link = "<a class='link' href=''></a>";
        }
        else if (status.Equals("Approved") && (task.Equals("Meta") && (bookPages > volumeBreakPages)))
        {
            link = "<a class='link' href='AddVolumeBreaks.aspx?aid=" + aid + "&bid=" + BID + "&pname=" + task + "'>Add Voume Breaks</a>";
        }
        else if (status == "Working" && !task.Equals("ComplexBitsMapping") && !task.Equals("Table") && !task.Equals("ErrorDetection"))
        {
            //link = "<a class='link' href='AssignTask.aspx?aid=" + aid + "&bid=" + BID + "&pname=" + task + "&act=upa'>Edit Task</a>";

            //if (!string.IsNullOrEmpty(Convert.ToString(Session["MainBook"])))
            //{
                Session["AID"] = aid;

                string queryBookID = "select Book.BIdentityNo from Activity inner join Book on Activity.BID = Book.BID where Activity.BID = " + BID;
                string bookID = objMyDBClass.GetID(queryBookID);
                Session["MainBook"] = bookID.Replace("-1", "");

                bool isParaSelected = objMyDBClass.GetParaIndentationStatus(Convert.ToString(Session["MainBook"]));

                if (!isParaSelected)
                {
                    //Response.Redirect("ParaSelection.aspx", true);

                    link = "<a class=\"link\" href='ParaSelection.aspx?aid=" + aid + "&bid=" + bookID + "'>Submit Task</a>";
                }
                else
                {
                    //string queryBookID = "select Book.BIdentityNo from Activity inner join Book on Activity.BID = Book.BID where Activity.BID = " + BID;
                    //string bookID = objMyDBClass.GetID(queryBookID);
                    link = "<a class=\"link\" href='TagUntag.aspx?aid=" + aid + "&bid=" + bookID + "'>Submit Task</a>";
                }
            //}
        }
        else if (status == "Working" && task.Equals("ComplexBitsMapping"))
        {
            Session["BID"] = BID;
            string queryBookID = "select Book.BIdentityNo from Activity inner join Book on Activity.BID = Book.BID where Activity.BID = " + BID;
            string bookID = objMyDBClass.GetID(queryBookID);
            
            ComplexBitsMapping obj = new ComplexBitsMapping();
            obj.ClearComplexBitSession();

            link = "<a class=\"link\" href='ComplexBitsMapping.aspx?aid=" + aid + "&bid=" + bookID + "'>Submit Task</a>";
        }
        //else if (status == "Working" && task.Equals("ErrorDetection"))
        //{
        //    Session["BID"] = BID;
        //    string queryBookID = "select Book.BIdentityNo from Activity inner join Book on Activity.BID = Book.BID where Activity.BID = " + BID;
        //    string bookID = objMyDBClass.GetID(queryBookID);

        //    Session["MainBook"] = bookID;
        //    Session["AID"] = aid;

        //    Session["xmlPath_MistakeInsertion"] = "";
        //    Session["xmlPath_MistakeInsertion"] = "";
        //    Session["pdfFile"] = "";

        //    //Response.Redirect("ComparisonPreProcess.aspx?type=task", true);

        //    link = "<a class=\"link\" href='ComparisonPreProcess.aspx?aid=" + aid + "&bid=" + bookID + "'>Submit Task</a>";
        //}

        return link;
    }
    #endregion

    //Showing Data in GridView / Repeator
    #region void ShowDataInGridView()
    public void ShowDataInGridView()
    {
        try
        {
            string query1 = "Select distinct Book.PageCount,Book.PageViewed,Book.BID,Book.BIdentityNo,VOLUME_BREAK_INFO.PAGES,Book.BTitle,Book.UploadedBy,Book.[UpDate],Activity.AID, Activity.Status,Activity.CompletionDate,Activity.Cost,Activity.[Count],Activity.Deadline,Activity.Task,[User].UName as AssignedTo from Book INNER Join Activity on Book.BID=Activity.BID INNER JOIN [User] on Activity.UID=[User].UID left outer Join VOLUME_BREAK_INFO on Book.BID=VOLUME_BREAK_INFO.BID Where Book.BStatus='incomplete'";
            string query2 = "Select distinct Book.PageCount,Book.PageViewed,Activity.BID,cast(Activity.BID as varchar(100)) as BIdentityNo,VOLUME_BREAK_INFO.PAGES, Book.BTitle,Book.UploadedBy,Book.[UpDate],Activity.AID, Activity.Status,Activity.CompletionDate,Activity.Cost,Activity.[Count],Activity.Deadline,Activity.Task,[User].UName as AssignedTo from Book INNER Join Activity on " +
                //"Book.MainBook="+
                "cast(Book.MainBook as bigint)=" +
                //"cast(Activity.BID as varchar(100))"+
                "Activity.BID " +
                "left outer Join VOLUME_BREAK_INFO on Book.BID=VOLUME_BREAK_INFO.BID INNER JOIN [User] on Activity.UID=[User].UID Where Book.BStatus='Complete'";

            //string query = "Select Book.BID,Book.BIdentityNo, Book.BTitle,Book.UploadedBy,Book.[UpDate],Activity.AID, Activity.Status,Activity.CompletionDate,Activity.Deadline,Activity.Task,[User].UName as AssignedTo from Book INNER Join Activity on Book.BID=Activity.BID INNER JOIN [User] on Activity.UID=[User].UID Where Book.BStatus='incomplete' AND Activity.Status<>'Approved'  AND Activity.AssignedBy='" + (Session["objUser"] as UserClass).UserName + "' ";
            string task = this.ddProcess.SelectedItem.Text;
            string status = this.ddStatus.SelectedItem.Text;
            string bookID = this.txtSearch.Text.Trim();
            string condition = "";
            if (Session["condition"] != null)
            {
                this.ddStatus.SelectedIndex = this.ddStatus.Items.IndexOf((ListItem)this.ddStatus.Items.FindByValue(Session["condition"].ToString()));
                status = Session["condition"].ToString();
                Session["condition"] = null;
            }

            if (task == "All" && status == "All" && bookID == "")
            {

            }

            else if (task == "All" && status == "Complete" && bookID == "")
            {

            }
            else if (task == "All" && status == "All" && bookID != "")
            {
                condition += "AND Book.BIdentityNo like '" + bookID + "%' ";
            }
            else if (task == "All" && status != "All" && bookID == "")
            {
                condition += "AND Activity.Status='" + status + "' ";
            }
            else if (task == "All" && status != "All" && bookID != "")
            {
                condition += "AND Activity.Status='" + status + "' AND Book.BIdentityNo like '" + bookID + "%' ";
            }
            else if (status == "All" && task != "All" && bookID == "")
            {
                condition += "AND Activity.Task='" + task + "' ";
            }
            else if (status == "All" && task != "All" && bookID != "")
            {
                condition += "AND Activity.Task='" + task + "' AND Book.BIdentityNo like '" + bookID + "%' ";
            }
            else if (status != "All" && task != "All" && bookID == "")
            {
                condition += "AND Activity.Task='" + task + "' AND Activity.Status='" + status + "' ";
            }
            else if (status != "All" && task != "All" && bookID != "")
            {
                condition += "AND Activity.Task='" + task + "' AND Activity.Status='" + status + "'AND Book.BIdentityNo like '" + bookID + "%' ";
            }
            else
            {
                condition += "and Activity.Status='Pending Confirmation'";
            }
            string query;
            if (task == "All" && status == "Complete" && bookID == "")
            {
                query = "Select distinct Book.PageCount,Book.PageViewed,Activity.BID,cast(Activity.BID as varchar(100)) as BIdentityNo,VOLUME_BREAK_INFO.PAGES," +
                    " Book.BTitle,Book.UploadedBy,Book.[UpDate],Activity.AID,Activity.Status,Activity.CompletionDate,Activity.Cost," +
                    "Activity.[Count],Activity.Deadline,Activity.Task,[User].UName as AssignedTo from Book INNER Join Activity on " +
                    "Book.MainBook=cast(Activity.BID as varchar(100)) INNER JOIN [User] on Activity.UID=[User].UID left outer Join VOLUME_BREAK_INFO " +
                    "on Book.BID=VOLUME_BREAK_INFO.BID Where Book.BStatus='Complete' AND Activity.Status like'Approved' and Activity.Task like 'Meta' ";
                //+
                //"  AND Activity.AssignedBy='" + Convert.ToString(Session["LoginId"]) + "' ";
            }
            else if (task == "All" && status == "Complete" && bookID != "")
            {
                query = "Select distinct Book.PageCount,Book.PageViewed,Activity.BID,cast(Activity.BID as varchar(100)) as BIdentityNo,VOLUME_BREAK_INFO.PAGES," +
                    " Book.BTitle,Book.UploadedBy,Book.[UpDate],Activity.AID,Activity.Status,Activity.CompletionDate,Activity.Cost," +
                    "Activity.[Count],Activity.Deadline,Activity.Task,[User].UName as AssignedTo from Book INNER Join Activity on " +
                    "Book.MainBook=cast(Activity.BID as varchar(100)) INNER JOIN [User] on Activity.UID=[User].UID left outer Join VOLUME_BREAK_INFO " +
                    "on Book.BID=VOLUME_BREAK_INFO.BID Where Book.BStatus='Complete' AND Activity.Status like'Approved' and Activity.Task like 'Meta' " +

                    //"  AND Activity.AssignedBy='" + Convert.ToString(Session["LoginId"]) + "' " 

                        " AND Book.BIdentityNo like '" + bookID + "%' ";
            }
            else
            {
                query = query1 + condition + " union " + query2 + condition;
            }

            DataSet ds = objMyDBClass.GetDataSet(query);

            //Added by Aamir Ghafoor on 2015-04-16
            //If dataset rows are greator then 0 then calculate time spent of books from workmeter.
            if (ds.Tables[0].Rows.Count > 0)
            {
                string bookId = "";

                ds.Tables[0].Columns.Add("TimeSpent", typeof(string));

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    bookId = Convert.ToString(dr["BIdentityNo"]);

                    if (bookId != "")
                    {
                        double timeSpent = getTimeSpent(bookId.Replace("-1", ""));

                        if (timeSpent > 0)
                            dr["TimeSpent"] = Convert.ToString(timeSpent);

                        else
                            dr["TimeSpent"] = Convert.ToString("");
                    }
                }
            }
            //end

            if (ds.Tables.Count > 0)
            {
                if (ds.Tables[0].Rows.Count >= 0)
                {
                    this.Repeater1.DataSource = ds.Tables[0];
                    this.Repeater1.DataBind();
                }
            }
            if (ds.Tables[0].Rows.Count <= 0)
            {
                ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! No Record Found.");
            }
        }
        catch (Exception)
        {
            ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured.");
        }
    }
    #endregion

    public double getTimeSpent(string bookId)
    {
        //SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
        //con.Open();

        //string strMaxTaskId = "select CalculatedTime from tblTaskDetails where BookId='" + bookId + "' and CatId = 27";
        //SqlCommand objCmdMax = new SqlCommand(strMaxTaskId, con);
        //objCmdMax.CommandType = CommandType.Text;
        //SqlDataReader objRsMax = objCmdMax.ExecuteReader();
        string CalculatedTime = "";
        //if (objRsMax.Read())
        //{
        //    if (objRsMax["CalculatedTime"].ToString() != "")
        //    {
        //        CalculatedTime = Convert.ToString(objRsMax["CalculatedTime"]);
        //    }
        //}
        //objRsMax.Close();
        //con.Close();

        return Convert.ToDouble(CalculatedTime == "" ? "0" : CalculatedTime);
    }

    public void btnSearch_Click(object sender, ImageClickEventArgs e)
    {

        //this.Master.ShowMessageBox("Hello Hello bole ke Mera aaajoo baajoo dool ke....", "error");
        this.txtComments.Text = "";
        this.pnlComments.Visible = false;
        this.uplComments.Update();
        pnlAmount.Visible = false;
        uplAccount.Update();
        Session.Remove("accountpanel");
        ShowDataInGridView();
    }
    protected void btnAddComments_Click(object sender, EventArgs e)
    {
        string aid = Request.QueryString["aid"].ToString();
        string queryUp = "Update Activity set Comments='" + this.txtComments.Text + "' Where AID=" + aid;
        objMyDBClass.ExecuteCommand(queryUp);
        string qUp = "Update Activity set CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "', Status='Rejected' Where AID=" + aid;
        objMyDBClass.ExecuteCommand(qUp);

        this.txtComments.Text = "";
        this.pnlComments.Visible = false;
        this.uplComments.Update();
        Response.Redirect("AdminPanel.aspx", false);

    }

    ////Create New Task after approving a task by admin---Old method commented by Aamir Ghafoor on 2016-03-21
    //protected void btnUpdateAmount_Click(object sender, EventArgs e)
    //{
    //    string aid = Request.QueryString["aid"].ToString();
    //    string action = Request.QueryString["act"].ToString();
    //    int inResult = 0;
    //    int inResult2 = 0;

    //    if (action == "cfm" && Request.QueryString["task"] != null && (Request.QueryString["task"].ToLower() != "mistakeinjection"))
    //    {
    //        string qApprove = "Update Activity set CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "', Status='Approved' Where AID=" + aid;
    //        objMyDBClass.ExecuteCommand(qApprove);
    //    }

    //    string queryBookID = "Select BID From Activity Where AID=" + aid;
    //    string bookID = objMyDBClass.GetID(queryBookID);

    //    //======================================================Create New Tasks==============================================================//

    //    //Create SpellCheck Task 
    //    if (action == "cfm" && Request.QueryString["task"] != null && (Request.QueryString["task"].ToLower() == "tagginguntagged" ||
    //        Request.QueryString["task"].ToLower() == "table" || Request.QueryString["task"].ToLower() == "image" || Request.QueryString["task"].ToLower() == "index"))
    //    {
    //        inResult = objMyDBClass.CreateTask(bookID, "Unassigned", "SpellCheck", (Session["objUser"] as UserClass).UserName);
    //    }

    //    //Create MistakeInjection Task
    //    else if (action == "cfm" && Request.QueryString["task"] != null && Request.QueryString["task"].ToLower() == "spellcheck")
    //    {
    //        inResult = objMyDBClass.CreateTask(bookID, "Unassigned", "MistakeInjection", (Session["objUser"] as UserClass).UserName);
    //    }

    //    //Create ErrorDetection Task
    //    else if (action == "cfm" && Request.QueryString["task"] != null && Request.QueryString["task"].ToLower() == "mistakeinjection")
    //    {
    //        //Check approved status of image, table and index tasks. If all 3 (if created) are approved then create ErrorDetection task           
    //        var temp = objMyDBClass.GetTasks_StatusByBookId(bookID);

    //        if ((temp != null) && (temp.Count > 0))
    //        {
    //            if (CheckTasks_Completion(temp))
    //            {
    //                //Create tasks for comparison-1 and comparison-2
    //                inResult = objMyDBClass.CreateTask(bookID, "Unassigned", "ErrorDetection", (Session["objUser"] as UserClass).UserName);
    //                inResult2 = objMyDBClass.CreateTask(bookID, "Unassigned", "ErrorDetection", (Session["objUser"] as UserClass).UserName);
    //            }
    //        }
    //    }

    //    //Create ErrorAdjustment Task
    //    else if (action == "cfm" && Request.QueryString["task"] != null && (Request.QueryString["task"].ToLower() == "ErrorDetection"))
    //    {
    //        string queryBID = "Select BID From Activity where AID=" + Request.QueryString["aid"].ToString();
    //        string BID = objMyDBClass.GetID(queryBID);

    //        //Get book id of book uploaded in webcompare for ErrorDetection
    //        string query_BookId = "select book.mainbook from activity inner join Book on Activity.BID = Book.BID where activity.bid = " + BID;
    //        string bookId = objMyDBClass.GetID(query_BookId);
    //        string pdfFilePath_User1 = "";
    //        string pdfFilePath_User2 = "";

    //        var temp = objMyDBClass.GetMistakeXmlStatus(bookId + "-1");

    //        if ((temp != null) && (temp.Count > 0))
    //        {
    //            string qApprove = "Update Activity set CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "', Status='Approved' Where AID=" + aid;
    //            objMyDBClass.ExecuteCommand(qApprove);

    //            string comparison = "";

    //            if (temp.Count == 1)
    //            {
    //                return;
    //            }

    //            if (temp.Count == 2)
    //            {
    //                if (temp[1].Split(',')[1].Equals("2"))
    //                {
    //                    pdfFilePath_User1 = objMyDBClass.WebCompareDirPhyPath + "/" + bookId + "/" + bookId + "-1/Comparison/Comparison-" + temp[0].Split(',')[1] + "/" + temp[0].Split(',')[0] + "/" + bookId + "-1.xml";
    //                    pdfFilePath_User2 = objMyDBClass.WebCompareDirPhyPath + "/" + bookId + "/" + bookId + "-1/Comparison/Comparison-" + temp[1].Split(',')[1] + "/" + temp[1].Split(',')[0] + "/" + bookId + "-1.xml";

    //                    //Check task approval of other editor
    //                    string queryApproval = "Select Activity.status From Activity inner join book on activity.BID = Book.BID" +
    //                                           " where Activity.task='MistakeInjection' and Book.MainBook='" + bookId +
    //                                           "' and Activity.AID <> " + Convert.ToString(Request.QueryString["aid"]);

    //                    string OtherEditor_Status = objMyDBClass.GetID(queryApproval);

    //                    //Calculate efficiency when both editor tasks's are approved by the admin
    //                    if (OtherEditor_Status.Trim().Equals("Approved"))
    //                    {
    //                        double efficiency = Calculate_CumulativeEfficiency(bookId, pdfFilePath_User1, pdfFilePath_User2);

    //                        if (efficiency == 100)
    //                        {
    //                            //    inResult = objMyDBClass.CreateTask(BID, "Unassigned", "QaInspection", (Session["objUser"] as UserClass).UserName);
    //                            //}
    //                            //else
    //                            //{
    //                            inResult = objMyDBClass.CreateTask(BID, "Unassigned", "ErrorAdjustment", (Session["objUser"] as UserClass).UserName);
    //                        }

    //                        return;
    //                    }
    //                    else
    //                    {
    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //    }

    //    else if (action == "cfm" && Request.QueryString["task"] != null && Request.QueryString["task"].ToLower() == "comparison")
    //    {
    //        objMyDBClass.LogEntry(bookID, "Comparison", "Comparison is approved. Error Adjustment task is assigned", "Completed", "update");

    //        string queryUID = "Select UID From Activity where BID=" + bookID;
    //        string UID = objMyDBClass.GetID(queryUID);

    //        string queryInsert = "Insert into ACTIVITY(UID,BID,AssignedBy,Status,Task) VALUES(" + UID + "," + bookID + ",'" + (Session["objUser"] as UserClass).UserName + "','Working','ErrorAdjustment')";
    //        inResult = objMyDBClass.ExecuteCommand(queryInsert);
    //        if (inResult > 0)
    //        {
    //            //string FileName = Server.MapPath("~/Files/" + bookID + "/IssueLog.txt");
    //            string FileName = objMyDBClass.MainDirPhyPath + "/" + bookID + "/IssueLog.txt";
    //            if (!File.Exists(FileName))
    //            {
    //                File.WriteAllText(FileName, "");
    //            }
    //            string val = File.ReadAllText(FileName);
    //            string[] issues = val.Split(new char[] { '^' });
    //            string qUpError = "Update Activity Set [Count]=" + issues.LongLength + " Where BID=" + bookID + " and Task='ErrorAdjustment'";
    //            objMyDBClass.ExecuteCommand(qUpError);
    //            objMyDBClass.LogEntry(bookID, "ErrorAdjustment", "Comparison is approved. Error Adjustment task is assigned", "In Progress", "insert");
    //        }
    //    }
    //    else if (action == "cfm" && Request.QueryString["task"] != null && Request.QueryString["task"].ToLower() == "erroradjustment")
    //    {
    //        objMyDBClass.LogEntry(bookID, "ErrorAdjustment", "Error Adjustment is approved. Going towards Finalizing / Volume Break / Meta", "Completed", "update");

    //        FinalizationProcess();
    //    }
    //    else if (action == "cfm" && Request.QueryString["task"] != null && Request.QueryString["task"].ToLower() == "meta")
    //    {
    //        objMyDBClass.LogEntry(bookID, "Meta", "Inserted Meta Information is approved", "Completed", "update");
    //        string queryUpdateMainBook = "Update MainBook Set Status='Complete' Where MainBook='" + bookID + "'";
    //        objMyDBClass.ExecuteCommand(queryUpdateMainBook);
    //        Response.Redirect("CompleteBooks.aspx?bid=" + bookID);
    //    }
    //    //===============================================End Create New Tasks=====================================================//

    //    string queryPayableEarning = "Select E.UID,A.TotalAmount,E.Task From Activity E Inner Join AccountInformation A on E.UID=A.UID where E.AID=" + aid;
    //    DataSet dsPayableEarning = objMyDBClass.GetDataSet(queryPayableEarning);

    //    if (dsPayableEarning.Tables[0].Rows.Count == 0)
    //        return;

    //    DataRow dr = dsPayableEarning.Tables[0].Rows[0];

    //    string userID = dr["UID"].ToString();
    //    string task = dr["Task"].ToString();
    //    double oldAmount = double.Parse(Session["accountpanel"].ToString().Split(new char[] { '@' })[1]);
    //    //if (oldAmount != double.Parse(txtPayableAmount.Text.Trim()))
    //    if (oldAmount > double.Parse(txtPayableAmount.Text.Trim()))//Shoaib here, condition changed, to avoid a dispute if the new amount is greater than the old amount
    //    {
    //        string qDispute = "Insert into Dispute(UID,AID,AcAmount,PpAmount,Bonus,Remarks,Status,DDate) Values(" + userID + "," + aid + "," + oldAmount + "," + txtPayableAmount.Text.Trim() + "," + this.txtBonus.Text.Trim() + ",'" + txtRemarks.Text.Trim() + "','User','" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "')";
    //        objMyDBClass.ExecuteCommand(qDispute);

    //        //string qUpdateActivity = "Update Activity Set Remarks='" + txtRemarks.Text.Trim() + "' Where AID="+aid;
    //        string qUpdateActivity = "Update Activity Set Comments='" + txtRemarks.Text.Trim() + "' Where AID=" + aid;//Shoaib here
    //        objMyDBClass.ExecuteCommand(qUpdateActivity);
    //    }
    //    else //if oldAmount is equal to or less than the new amount
    //    {
    //        string totalAmount = dr["TotalAmount"].ToString() == "" ? "0" : dr["TotalAmount"].ToString();
    //        totalAmount = (double.Parse(totalAmount) + double.Parse(txtPayableAmount.Text.Trim()) + double.Parse(txtBonus.Text.Trim())).ToString();

    //        //Acount Detail
    //        string qAccountDetail = "Insert into AccountDetail(UID,Deposit,Withdraw,Balance,Description,[Date]) Values(" + userID + "," + (double.Parse(txtPayableAmount.Text.Trim()) + double.Parse(txtBonus.Text.Trim())) + ",0.00," + totalAmount + ",'Amount against the task " + task.ToUpper() + " is deposited'," + DateTime.Now.Date.GetDateTimeFormats('d')[5] + ")";
    //        objMyDBClass.ExecuteCommand(qAccountDetail);
    //        //End Account Detail     

    //        string queryUpdateAccount = "Update AccountInformation Set TotalAmount=" + totalAmount + " Where UID=" + userID;
    //        objMyDBClass.ExecuteCommand(queryUpdateAccount);
    //    }
    //    //Adding data to Earning Table
    //    string qEarning = "Insert into Earnings(UID,AID,ActualEarning,PayableEarning,Bonus,Paid,Remarks) values(" + userID + "," + aid + "," + oldAmount + "," + txtPayableAmount.Text.Trim() + "," + txtBonus.Text.Trim() + ",'Y','" + this.txtComments.Text + "')";
    //    objMyDBClass.ExecuteCommand(qEarning);
    //    //End Earning Table
    //    pnlAmount.Visible = false;
    //    uplAccount.Update();
    //    Session.Remove("accountpanel");
    //    Response.Redirect("AdminPanel.aspx");
    //}

    //Create New Task after approving a task by admin

    protected void btnUpdateAmount_Click(object sender, EventArgs e)
    {
        string aid = Request.QueryString["aid"].ToString();
        string action = Request.QueryString["act"].ToString();
        int inResult = 0;
        int inResult2 = 0;

        if (action == "cfm" && Request.QueryString["task"] != null && (Request.QueryString["task"].ToLower() != "mistakeinjection"))
        {
            string qApprove = "Update Activity set CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "', Status='Approved' Where AID=" + aid;
            objMyDBClass.ExecuteCommand(qApprove);
        }

        string queryBookID = "Select BID From Activity Where AID=" + aid;
        string bookID = objMyDBClass.GetID(queryBookID);

        //======================================================Create New Tasks==============================================================//

        //Create SpellCheck Task 
        if (action == "cfm" && Request.QueryString["task"] != null && (Request.QueryString["task"].ToLower() == "tagginguntagged" ||
            Request.QueryString["task"].ToLower() == "table" || Request.QueryString["task"].ToLower() == "image" || Request.QueryString["task"].ToLower() == "index"))
        {
            inResult = objMyDBClass.CreateTask(bookID, "Unassigned", "SpellCheck", (Session["objUser"] as UserClass).UserName);
        }

        //Create MistakeInjection Task
        else if (action == "cfm" && Request.QueryString["task"] != null && Request.QueryString["task"].ToLower() == "spellcheck")
        {
            inResult = objMyDBClass.CreateTask(bookID, "Unassigned", "MistakeInjection", (Session["objUser"] as UserClass).UserName);
        }

        //Create ErrorDetection Task
        else if (action == "cfm" && Request.QueryString["task"] != null && Request.QueryString["task"].ToLower() == "mistakeinjection")
        {
            //Check approved status of image, table and index tasks. If all 3 (if created) are approved then create ErrorDetection task           
            var temp = objMyDBClass.GetTasks_StatusByBookId(bookID);

            if ((temp != null) && (temp.Count > 0))
            {
                if (CheckTasks_Completion(temp))
                {
                    //Create tasks for comparison-1 and comparison-2
                    inResult = objMyDBClass.CreateTask(bookID, "Unassigned", "ErrorDetection", (Session["objUser"] as UserClass).UserName);
                    inResult2 = objMyDBClass.CreateTask(bookID, "Unassigned", "ErrorDetection", (Session["objUser"] as UserClass).UserName);
                }
            }
        }

        //Create ErrorAdjustment Task
        else if (action == "cfm" && Request.QueryString["task"] != null && (Request.QueryString["task"].ToLower() == "ErrorDetection"))
        {
            string queryBID = "Select BID From Activity where AID=" + Request.QueryString["aid"].ToString();
            string BID = objMyDBClass.GetID(queryBID);

            //Get book id of book uploaded in webcompare for ErrorDetection
            string query_BookId = "select book.mainbook from activity inner join Book on Activity.BID = Book.BID where activity.bid = " + BID;
            string bookId = objMyDBClass.GetID(query_BookId);
            string pdfFilePath_User1 = "";
            string pdfFilePath_User2 = "";

            var temp = objMyDBClass.GetMistakeXmlStatus(bookId + "-1");

            if ((temp != null) && (temp.Count > 0))
            {
                string qApprove = "Update Activity set CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "', Status='Approved' Where AID=" + aid;
                objMyDBClass.ExecuteCommand(qApprove);

                string comparison = "";

                if (temp.Count == 1)
                {
                    return;
                }

                if (temp.Count == 2)
                {
                    if (temp[1].Split(',')[1].Equals("2"))
                    {
                        pdfFilePath_User1 = objMyDBClass.WebCompareDirPhyPath + "/" + bookId + "/" + bookId + "-1/Comparison/Comparison-" + temp[0].Split(',')[1] + "/" + temp[0].Split(',')[0] + "/" + bookId + "-1.xml";
                        pdfFilePath_User2 = objMyDBClass.WebCompareDirPhyPath + "/" + bookId + "/" + bookId + "-1/Comparison/Comparison-" + temp[1].Split(',')[1] + "/" + temp[1].Split(',')[0] + "/" + bookId + "-1.xml";

                        //Check task approval of other editor
                        string queryApproval = "Select Activity.status From Activity inner join book on activity.BID = Book.BID" +
                                               " where Activity.task='MistakeInjection' and Book.MainBook='" + bookId +
                                               "' and Activity.AID <> " + Convert.ToString(Request.QueryString["aid"]);

                        string OtherEditor_Status = objMyDBClass.GetID(queryApproval);

                        //Calculate efficiency when both editor tasks's are approved by the admin
                        if (OtherEditor_Status.Trim().Equals("Approved"))
                        {
                            double efficiency = Calculate_CumulativeEfficiency(bookId, pdfFilePath_User1, pdfFilePath_User2);

                            if (efficiency == 100)
                            {
                                //    inResult = objMyDBClass.CreateTask(BID, "Unassigned", "QaInspection", (Session["objUser"] as UserClass).UserName);
                                //}
                                //else
                                //{
                                inResult = objMyDBClass.CreateTask(BID, "Unassigned", "ErrorAdjustment", (Session["objUser"] as UserClass).UserName);
                            }

                            return;
                        }
                        else
                        {
                            return;
                        }
                    }
                }
            }
        }

        else if (action == "cfm" && Request.QueryString["task"] != null && Request.QueryString["task"].ToLower() == "comparison")
        {
            objMyDBClass.LogEntry(bookID, "Comparison", "Comparison is approved. Error Adjustment task is assigned", "Completed", "update");

            string queryUID = "Select UID From Activity where BID=" + bookID;
            string UID = objMyDBClass.GetID(queryUID);

            string queryInsert = "Insert into ACTIVITY(UID,BID,AssignedBy,Status,Task) VALUES(" + UID + "," + bookID + ",'" + (Session["objUser"] as UserClass).UserName + "','Working','ErrorAdjustment')";
            inResult = objMyDBClass.ExecuteCommand(queryInsert);
            if (inResult > 0)
            {
                //string FileName = Server.MapPath("~/Files/" + bookID + "/IssueLog.txt");
                string FileName = objMyDBClass.MainDirPhyPath + "/" + bookID + "/IssueLog.txt";
                if (!File.Exists(FileName))
                {
                    File.WriteAllText(FileName, "");
                }
                string val = File.ReadAllText(FileName);
                string[] issues = val.Split(new char[] { '^' });
                string qUpError = "Update Activity Set [Count]=" + issues.LongLength + " Where BID=" + bookID + " and Task='ErrorAdjustment'";
                objMyDBClass.ExecuteCommand(qUpError);
                objMyDBClass.LogEntry(bookID, "ErrorAdjustment", "Comparison is approved. Error Adjustment task is assigned", "In Progress", "insert");
            }
        }
        else if (action == "cfm" && Request.QueryString["task"] != null && Request.QueryString["task"].ToLower() == "erroradjustment")
        {
            objMyDBClass.LogEntry(bookID, "ErrorAdjustment", "Error Adjustment is approved. Going towards Finalizing / Volume Break / Meta", "Completed", "update");

            FinalizationProcess();
        }
        else if (action == "cfm" && Request.QueryString["task"] != null && Request.QueryString["task"].ToLower() == "meta")
        {
            objMyDBClass.LogEntry(bookID, "Meta", "Inserted Meta Information is approved", "Completed", "update");
            string queryUpdateMainBook = "Update MainBook Set Status='Complete' Where MainBook='" + bookID + "'";
            objMyDBClass.ExecuteCommand(queryUpdateMainBook);
            Response.Redirect("CompleteBooks.aspx?bid=" + bookID);
        }
        //===============================================End Create New Tasks=====================================================//

        double bonus = 0;

        string queryPayableEarning = "Select E.UID,A.TotalAmount,E.Task From Activity E Inner Join AccountInformation A on E.UID=A.UID where E.AID=" + aid;
        DataSet dsPayableEarning = objMyDBClass.GetDataSet(queryPayableEarning);

        if (dsPayableEarning.Tables[0].Rows.Count == 0)
            return;

        DataRow dr = dsPayableEarning.Tables[0].Rows[0];

        string userID = dr["UID"].ToString();
        string task = dr["Task"].ToString();
        double oldAmount = double.Parse(Session["accountpanel"].ToString().Split(new char[] { '@' })[1]);
        //if (oldAmount != double.Parse(txtPayableAmount.Text.Trim()))
        if (oldAmount > double.Parse(txtPayableAmount.Text.Trim()))//Shoaib here, condition changed, to avoid a dispute if the new amount is greater than the old amount
        {
            string qDispute = "Insert into Dispute(UID,AID,AcAmount,PpAmount,Bonus,Remarks,Status,DDate) Values(" + userID + "," + aid + "," + oldAmount + "," + txtPayableAmount.Text.Trim() + "," + this.txtBonus.Text.Trim() + ",'" + txtRemarks.Text.Trim() + "','User','" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "')";
            objMyDBClass.ExecuteCommand(qDispute);

            //string qUpdateActivity = "Update Activity Set Remarks='" + txtRemarks.Text.Trim() + "' Where AID="+aid;
            string qUpdateActivity = "Update Activity Set Comments='" + txtRemarks.Text.Trim() + "' Where AID=" + aid;//Shoaib here
            objMyDBClass.ExecuteCommand(qUpdateActivity);
        }
        else //if oldAmount is equal to or less than the new amount
        {
            string totalAmount = dr["TotalAmount"].ToString() == "" ? "0" : dr["TotalAmount"].ToString();

            if (string.IsNullOrEmpty(txtBonus.Text.Replace("AU $ ", ""))) bonus = 0;
            else bonus = Convert.ToDouble(txtBonus.Text.Trim().Replace("AU $", ""));

            if (cbxBonux.Checked) bonus = 0;

            totalAmount = (double.Parse(totalAmount) + double.Parse(txtPayableAmount.Text.Trim()) + bonus).ToString();

            //Acount Detail
            string qAccountDetail = "Insert into AccountDetail(UID,Deposit,Withdraw,Balance,Description,[Date]) Values(" + userID + "," +
                (double.Parse(txtPayableAmount.Text.Trim()) + bonus) + ",0.00," + totalAmount + ",'Amount against the task " +
               Convert.ToString(Session["BIdentityNo"]).Replace("-1", "") + " is Deposited'," + "GETDATE()" + ")";
            objMyDBClass.ExecuteCommand(qAccountDetail);
            //End Account Detail     

            string queryUpdateAccount = "Update AccountInformation Set TotalAmount=" + totalAmount + " Where UID=" + userID;
            objMyDBClass.ExecuteCommand(queryUpdateAccount);
        }

        if (string.IsNullOrEmpty(txtBonus.Text.Replace("AU $", ""))) bonus = 0;
        else bonus = Convert.ToDouble(txtBonus.Text.Trim().Replace("AU $", ""));

        if (cbxBonux.Checked) bonus = 0;

        //Adding data to Earning Table
        string qEarning = "Insert into Earnings(UID,AID,ActualEarning,PayableEarning,Bonus,Paid,Remarks) values(" + userID + "," + aid + "," + oldAmount +
                          "," + txtPayableAmount.Text.Trim() + "," + bonus + ",'Y','" + this.txtComments.Text + "')";
        objMyDBClass.ExecuteCommand(qEarning);
        //End Earning Table
        pnlAmount.Visible = false;
        uplAccount.Update();
        Session.Remove("accountpanel");
        //Response.Redirect("AdminPanel.aspx");

        ucShowMessage1.ShowMessage(MessageTypes.Success, " Task is approved successfully.");
        //Response.Redirect(Request.Url.AbsoluteUri);
    }

    public bool CheckTasks_Completion(List<string> list_Tasks)
    {
        if (list_Tasks.Count == 1)
            return true;

        bool check = true;

        foreach (string task in list_Tasks)
        {
            if ((task.Split(',')[1].Equals("Image")) || (task.Split(',')[1].Equals("Table")) || (task.Split(',')[1].Equals("Index")))
            {
                if (!(task.Split(',')[2].Equals("Approved")))
                {
                    check = false;
                    break;
                }
            }
        }

        return check;
    }

    private double Calculate_CumulativeEfficiency(string bookId, string xmlOne, string xmlTwo)
    {
        //Get injected mistake from xml 1
        XmlDocument xmlDoc = LoadXmlDocument(xmlOne);
        int injected_mistakes = xmlDoc.SelectNodes(@"//ln[@autoInjection!='']").Count;

        //Get mistake from xml 1
        XmlDocument xmlDoc_Xml1 = LoadXmlDocument(xmlOne);
        int mistakes_Xml1 = xmlDoc_Xml1.SelectNodes(@"//ln[@pdfMistake!='']").Count;

        //Get mistake from xml 2
        XmlDocument xmlDoc_Xml2 = LoadXmlDocument(xmlTwo);
        int mistakes_Xml2 = xmlDoc_Xml2.SelectNodes(@"//ln[@pdfMistake!='']").Count;

        if (injected_mistakes > 0)
        {
            return Convert.ToDouble((mistakes_Xml1 + mistakes_Xml2) / (2 * injected_mistakes));
        }

        return 0;
    }

    private XmlDocument LoadXmlDocument(string xmlPath)
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
    protected void CreateComparisonTask()
    {
        string queryBID = "Select BID From Activity where AID=" + Request.QueryString["aid"].ToString();
        string BID = objMyDBClass.GetID(queryBID);
        string queryMainBook = "Select MainBook from Book Where BID=" + BID;
        string mainBook = objMyDBClass.GetID(queryMainBook);
        if (mainBook != "")
        {
            string queryBookSubIDs = "Select BID From Book Where MainBook='" + mainBook + "'";
            DataSet dsBookSubIDs = objMyDBClass.GetDataSet(queryBookSubIDs);
            bool isUntagged = true;
            foreach (DataRow drBook in dsBookSubIDs.Tables[0].Rows)
            {
                string queryAllProcessRecord = "Select * from ACTIVITY Where BID=" + drBook["BID"].ToString();
                DataSet dsAllProcessRecord = objMyDBClass.GetDataSet(queryAllProcessRecord);
                foreach (DataRow drActivity in dsAllProcessRecord.Tables[0].Rows)
                {
                    if (drActivity["Status"].ToString().ToLower() != "approved")// && drActivity["Task"].ToString().ToLower() == "tagginguntagged")// && dr["Task"].ToString() != "Index" && dr["Task"].ToString() != "Image")
                    {
                        isUntagged = false;
                        break;
                    }
                }
            }
            if (isUntagged == true)
            {
                foreach (DataRow drBook in dsBookSubIDs.Tables[0].Rows)
                {
                    string queryUpdatePartStatus = "Update Book Set BStatus='Complete' Where BID=" + drBook["BID"].ToString();
                    objMyDBClass.ExecuteCommand(queryUpdatePartStatus);
                }

                objMyDBClass.LogEntry(mainBook, "TaggingUntagged", "TaggingUntagged on Partitions completed successfully", "Completed", "insert");

                Outsourcing_System.AutoMapService.AutoMappService autoMapSvc = new Outsourcing_System.AutoMapService.AutoMappService();
                try
                {
                    //string mainFile = Server.MapPath("~/Files/" + mainBook + "/" + mainBook);
                    string mainFile = objMyDBClass.MainDirPhyPath + "/" + mainBook + "/" + mainBook;

                    //Calling Service
                    objMyDBClass.LogEntry(mainBook, "Merging", "Merging all the partition", "In Progress", "insert");
                    string status = autoMapSvc.MergMethod(mainFile + ".pdf");
                    //if (status == "Successfull")
                    //{
                    objMyDBClass.LogEntry(mainBook, "Merging", "Partitions are merged successfully", "Completed", "update");

                    int inResult = objMyDBClass.CreateTask(mainBook, "Unassigned", "Comparison", (Session["objUser"] as UserClass).UserName);
                    if (inResult > 0)
                    {
                        //string compDirPath = Server.MapPath("~/Files/" + mainBook + "/Comparison");
                        string compDirPath = objMyDBClass.MainDirPhyPath + "/" + mainBook + "/Comparison";
                        if (!Directory.Exists(compDirPath))
                        {
                            Directory.CreateDirectory(compDirPath);
                        }

                        if (!File.Exists(compDirPath))
                        {
                            File.WriteAllText(compDirPath + "\\ListOfConversionEditing.csv", "Page No,Merging,Splitting,Text Editing,Para Conversions,Table Inserted,Box Inserted,Image Inserted,Node Deleted,Para Addition,Section Addition");
                        }
                        objMyDBClass.SendMail("Comparison", (Session["objUser"] as UserClass).UserName, (Session["objUser"] as UserClass));
                    }
                    //}
                }
                finally
                {
                    autoMapSvc.Dispose();
                }
            }
        }
    }

    protected void lnkLogout_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("http://localhost:30074/BookMicro.aspx");
    }

    protected void btnsynDb_Click(object sender, EventArgs e)
    {
        bool status = false;

        var cloudWmData = objMyDBClass.GetCloudWorkMeterData();

        if (cloudWmData != null && cloudWmData.Count > 0)
        {
            List<Outsourcing_System.WorkMeterDbSyncService.WorkMeterEntities> tempList = new List<Outsourcing_System.WorkMeterDbSyncService.WorkMeterEntities>();
            Outsourcing_System.WorkMeterDbSyncService.WorkMeterEntities wmRow = null;

            List<Outsourcing_System.WorkMeterDbSyncService.DateWiseInfo> wmDailyTaskList = new List<Outsourcing_System.WorkMeterDbSyncService.DateWiseInfo>();

            foreach (var item in cloudWmData)
            {
                wmRow = new Outsourcing_System.WorkMeterDbSyncService.WorkMeterEntities();

                wmRow.FullName = item.FullName;
                wmRow.Email = item.Email;
                wmRow.TaskCreationDate = item.TaskCreationDate;
                wmRow.BookId = item.BookId;
                wmRow.CatId = item.CatId;
                wmRow.CalculatedTime = item.CalculatedTime;
                wmRow.StartTime = item.StartTime;
                wmRow.EndTime = item.EndTime;
                wmRow.End_Date = item.End_Date;
                wmRow.Complexity = item.Complexity;
                wmRow.Comments = item.Comments;
                wmRow.Achived = item.Achived;
                wmRow.Target = item.Target;
                wmRow.Current_Status = item.Current_Status;
                wmRow.Result = item.Result;
                wmRow.Expected_Hours = item.Expected_Hours;
                wmRow.Expected_Pages = item.Expected_Hours;
                wmRow.Productivity_Hours = item.Productivity_Hours;
                wmRow.Tool_Used = item.Tool_Used;

                foreach (var value in item.DailyTimeSpent)
                {
                    wmDailyTaskList.Add(new Outsourcing_System.WorkMeterDbSyncService.DateWiseInfo
                    {
                        BookId = value.BookId,
                        CategoryId = value.CategoryId,
                        TaskDate = value.TaskDate,
                        TimeSpent = value.TimeSpent
                    });
                }

                wmRow.DailyTimeSpent = wmDailyTaskList.ToArray();

                tempList.Add(wmRow);
            }
            try
            {
                Outsourcing_System.WorkMeterDbSyncService.SyncronizeDb obj = new Outsourcing_System.WorkMeterDbSyncService.SyncronizeDb();
                status = obj.InsertRecordsInWorkMeterDb(tempList.ToArray());

                if (status)
                {
                   List<string> completedbooksList = cloudWmData.Where(x => x.Current_Status.Equals("complete") && x.CatId.Equals("27"))
                                                                .Select(y => y.BookId).ToList();

                   //objMyDBClass.DeleteTasksInCloudWorkMeter(completedbooksList);
                }
            }
            catch (Exception)
            {
                ucShowMessage1.ShowMessage(MessageTypes.Error, "WorkMeter db is not available now.");
            }
            if (status)
                ucShowMessage1.ShowMessage(MessageTypes.Success, "Data is successfully inserted in local workmeter db.");

            else
                ucShowMessage1.ShowMessage(MessageTypes.Error, "Some Error has occured while inserting Data in local db.");
        }
        else
            ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! No data available in cloud's workmeter db.");
    }
}                                                                        
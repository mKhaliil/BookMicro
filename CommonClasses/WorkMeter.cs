using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using iTextSharp.text.pdf;
using Outsourcing_System.ServiceReference1;

namespace Outsourcing_System.CommonClasses
{
    public class WorkMeter
    {
        MyDBClass objMyDBClass = new MyDBClass();

        public void InsertTaskInWorkMeter(string taskTypeId, string srcPdfPath, string bookId)
        {
            try
            {
                string email = Convert.ToString(HttpContext.Current.Session["Email"]);

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
                string userId = Convert.ToString(getUserId_ByEmail(email, userName, taskTypeId));
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
                        strTaskId + "," + taskTypeId + ",'" + bookId + "','" + startTime +
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

                TaskInfoManuplation(Convert.ToInt32(userId), Convert.ToInt32(bookId), 0, DateTime.Now.ToShortDateString(), Convert.ToInt32(taskTypeId), "insert");
                handleLog(strTaskId, "(checked In) ", userId);
            }

            catch (Exception ex)
            {
                //this.lblMessage.ForeColor = Color.Red;
                //this.lblMessage.Text = ex.Message + ex.Source + ex.InnerException;
            }
        }

        public string StopTask(bool complete, string status, string taskTypeId, string bookId)
        {
            try
            {
                string email = Convert.ToString(HttpContext.Current.Session["email"]);

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

                string userId = Convert.ToString(getUserId_ByEmail(email, userName, taskTypeId));
                string TaskId = getTaskId(bookId, taskTypeId);

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

                //string compTime = Convert.ToString(HttpContext.Current.Session["TimeSpent_ComparisonTask"]);
                //int hr = Convert.ToInt32(compTime.Split(':')[0]);
                //int mn = Convert.ToInt32(compTime.Split(':')[1]);

                double hours = 0;
                double minutes = 0;

                if (calculatedTime > 0)
                {
                    if (status.Equals("pause"))
                    {
                        hours = hours + endDate.Subtract(Convert.ToDateTime(startDate)).Hours;
                        minutes = minutes + endDate.Subtract(Convert.ToDateTime(startDate)).Minutes;

                        //hours = hr;
                        //minutes = mn;
                    }
                    minutes = minutes + (hours * 60);
                    hours = .5 / 30 * minutes;
                    calculatedTime = calculatedTime + hours;
                }
                else
                {
                    hours = endDate.Subtract(Convert.ToDateTime(startDate)).Hours;
                    minutes = endDate.Subtract(Convert.ToDateTime(startDate)).Minutes;

                    //double hours = hr;
                    //double minutes = mn;
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
                               "',CalculatedTime='" + Math.Round(calculatedTime, 2).ToString() + "'where TaskId=" + TaskId;

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

                if ((userId != null) || (userId != ""))
                {
                    TaskInfoManuplation(Convert.ToInt32(userId), Convert.ToInt32(bookId), Math.Round(calculatedTime, 2),
                        endDate.ToShortDateString(), Convert.ToInt32(taskTypeId), "update");
                }

                return Convert.ToString(calculatedTime);
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

        private void TaskLogManuplation(int taskid, string taskLog, int countLog, int userID, DateTime date, string action)
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

        public string getTaskId(string bookId, string taskTypeId)
        {
            try
            {
                SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
                con.Open();

                string strMaxTaskId = "select TaskId from tblTaskDetails where BookId='" + bookId + "' and CatId = " + taskTypeId;
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
    }
}
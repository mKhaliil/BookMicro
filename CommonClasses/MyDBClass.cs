using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
//using Microsoft.Office.Interop.Word;
using System.Collections.Generic;
using BookMicroBeta;
//using Outsourcing_System.CommonClasses;
using Outsourcing_System.CommonClasses;

public class MyDBClass
{
    public MyDBClass()
    {

    }

    public string MainDirectory = "";


    public string WebCompareDirPhyPath
    {
        get
        {
            return System.Configuration.ConfigurationManager.AppSettings["WebCompareDirPhyPath"];
        }
    }

    //public string PDFDirPhyPath
    //{
    //    get
    //    {
    //        return System.Configuration.ConfigurationManager.AppSettings["PDFDirPhyPath"];
    //    }
    //}

    public string MainDirPhyPath
    {
        get
        {
            return System.Configuration.ConfigurationManager.AppSettings["MainDirPhyPath"];
        }
    }

    public string VolumBreakPages
    {
        get
        {
            return System.Configuration.ConfigurationManager.AppSettings["VolumBreakPages"];
        }
    }
    public string ConnectionString()
    {
        string constring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
        return constring;
    }
    public string ConnectionString_Workmeter()
    {
        string constring = ConfigurationManager.ConnectionStrings["ConnectionString_Workmeter"].ConnectionString;
        return constring;
    }

    public string ConnectionString_OutCopy()
    {
        string constring = ConfigurationManager.ConnectionStrings["ConnectionString_OutCopy"].ConnectionString;
        return constring;
    }
    public DataSet GetDataSet(string Query)
    {
        SqlConnection con = null;
        try
        {
            con = new SqlConnection(ConnectionString());
            con.Open();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.SelectCommand.ExecuteNonQuery();
            DataSet ds = new DataSet();
            da.Fill(ds, "temp");
            con.Close();
            return ds;
        }
        finally
        {
            con.Close();
        }
    }
    public string ExecuteSelectCom(string Query)
    {
        SqlConnection con = null;
        try
        {
            con = new SqlConnection(ConnectionString());
            con.Open();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.SelectCommand.ExecuteNonQuery();
            DataSet ds = new DataSet();
            da.Fill(ds, "temp");
            con.Close();
            return ds.Tables[0].Rows[0][0].ToString();
        }
        catch (Exception excep)
        {
            return "0";
        }
        finally
        {
            con.Close();
        }
    }
    public int ExecuteCommand(string Query)
    {
        SqlConnection con = null;
        try
        {
            con = new SqlConnection(ConnectionString());
            con.Open();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            int result = da.SelectCommand.ExecuteNonQuery();
            return result;
        }
        finally
        {
            con.Close();
        }
    }

    public string GetID(string Query)
    {
        SqlConnection con = null;
        try
        {
            con = new SqlConnection(ConnectionString());
            con.Open();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.SelectCommand.ExecuteNonQuery();
            DataSet ds = new DataSet();
            da.Fill(ds, "temp");
            con.Close();
            return ds.Tables[0].Rows[0][0].ToString();
        }
        catch (Exception excep)
        {
            return "";
        }
        finally
        {
            con.Close();
        }
    }


    /*
     * Function for getting all Processes from Process table
     */
    public DataSet getAllProcesses()
    {
        SqlConnection con = null;
        DataSet ds = new DataSet();

        string Query = "SELECT * FROM Process";
        try
        {
            con = new SqlConnection(ConnectionString());
            con.Open();
            SqlCommand cmd = new SqlCommand(Query, con);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.SelectCommand.ExecuteNonQuery();
            da.Fill(ds, "temp");
            con.Close();
            //return ds.Tables[0].Rows[0][0].ToString();
        }
        finally
        {
            con.Close();
        }

        return ds;
    }
    public int SendMail(string process, string toUser, UserClass fromUser)
    {
        string subject = "New Task *" + process + "* Uploaded";
        string body = "Dear " + toUser + ", <br/><br/>Task has been uploaded for your confirmation" + @"<br/> Thanks,<br/>" + fromUser.UserFullName;
        DateTime dt = DateTime.Now;
        string date = dt.Year + "-" + dt.Month + "-" + dt.Day;
        string queryMail = "insert into mail(Subject,Message,Dat,Reciever,Sender,Stat,DelFrom,DelTo) values('" + subject + "','" + body + "','" + date + "','" + toUser + "','" + fromUser.UserID + "','No','Null','Null')";
        queryMail = queryMail.Replace("\r\n", "<br />");
        int res = ExecuteCommand(queryMail);
        return res;
    }

    public void LogEntry(string MainBook, string Process, string detail, string Status, string action)
    {
        DateTime dt = DateTime.Now;
        string time = dt.Hour + ":" + dt.Minute;
        string queryLog = "";
        if (action == "insert")
        {
            queryLog = "Insert into LogBook(MainBook,Process,detail,Status,StartTime) values('" + MainBook + "','" + Process + "','" + detail + "','" + Status + "','" + time + "')";
            queryLog = queryLog.Replace("\r\n", "<br />");
        }
        else if (action == "update")
        {
            queryLog = "Update LogBook set Status='" + Status + "',detail='" + detail + "' Where MainBook='" + MainBook + "' and Process='" + Process + "'";
            queryLog = queryLog.Replace("\r\n", "<br />");

        }
        int res = ExecuteCommand(queryLog);
    }

    #region UpdateMailStatus(string MID,string UserName)
    public void UpdateMailStatus(string MID, string UserName)
    {
        string queryUpdate = "UPDATE MAIL SET Stat='Yes' WHERE Reciever='" + UserName + "' AND MailID=" + MID;
        ExecuteCommand(queryUpdate);
    }
    #endregion

    public long InsertDisputeHistory(long DisputeID, float ProposedAmount, float ProposedBonus, long LoggedBy_ID, string Comments)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            SqlCommand cmd = new SqlCommand("AddDisputeHistory", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DID", DisputeID);
            cmd.Parameters.AddWithValue("@ProposedAmount", ProposedAmount);
            cmd.Parameters.AddWithValue("@ProposedBonus", ProposedBonus);
            cmd.Parameters.AddWithValue("@LoggedBy", LoggedBy_ID);
            cmd.Parameters.AddWithValue("@Comments", Comments);
            long dh_ID = new long();
            cmd.Parameters.AddWithValue("@DisputHistory_IDOUT", dh_ID).Direction = ParameterDirection.Output;
            dh_ID = (long)cmd.Parameters["@DisputHistory_IDOUT"].Value;

            sqlCon.Open();
            int rowsUpdated = cmd.ExecuteNonQuery();
            return rowsUpdated;
        }
        catch
        {
            return -1;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
        return -1;
    }
    public long InsertDisputeHistory(long DisputeID, long LoggedBy_ID, string Comments)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            SqlCommand cmd = new SqlCommand("AddDisputeHistory", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@DID", DisputeID);
            cmd.Parameters.AddWithValue("@LoggedBy", LoggedBy_ID);
            cmd.Parameters.AddWithValue("@Comments", Comments);
            long dh_ID = new long();
            cmd.Parameters.AddWithValue("@DisputHistory_IDOUT", dh_ID).Direction = ParameterDirection.Output;
            dh_ID = (long)cmd.Parameters["@DisputHistory_IDOUT"].Value;

            sqlCon.Open();
            int rowsUpdated = cmd.ExecuteNonQuery();
            return rowsUpdated;
        }
        catch
        {
            return -1;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public int CreateTask(string BID, string Status, string Task, string Admin)
    {
        string queryUnnowID = "Select * from [User] where UserName='unknown'";
        string UnKnownID = GetID(queryUnnowID);

        //commented by aamir on 2017-04-05
        //string queryPriority = "Select top (1) priority from ACTIVITY where BID=" + BID;
        //int priority = Convert.ToInt32(GetID(queryPriority));
        //priority = priority < 1 ? 1 : priority;

        int priority = 1;
        //string queryInsert = "Insert into ACTIVITY(UID,BID,AssignedBy,Status,Task,priority) VALUES(" + Admin + "," + BID + ",'" + Admin + "','" + Status 
        //                        + "','" + Task + "'," + priority + ")";

        string queryInsert = "";

        if (Task.Equals("Table") || Task.Equals("MistakeInjection") || Task.Equals("ErrorDetection") || Task.Equals("Image"))
            queryInsert = "Insert into ACTIVITY(UID,BID,AssignedBy,Status,Task,priority) VALUES(" + "42"+ "," + BID + ",'" + Admin + "','" + Status
                                + "','" + Task + "'," + priority + ")";
        else
            queryInsert = "Insert into ACTIVITY(UID,BID,AssignedBy,Status,Task,priority) VALUES(" + Admin + "," + BID + ",'" + Admin + "','" + Status 
                                + "','" + Task + "'," + priority + ")";

        int inResult = ExecuteCommand(queryInsert);
        return inResult;
    }

    public string ClearTests(string userName, string testType, string status, string testName)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            SqlCommand cmd = new SqlCommand("ClearOnlineTests", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name", userName.Trim());
            cmd.Parameters.AddWithValue("@TestType", testType.Trim());
            cmd.Parameters.AddWithValue("@status", status.Trim());
            cmd.Parameters.AddWithValue("@testName", testName.Trim());
            cmd.Parameters.AddWithValue("@start_time", DateTime.Now);
            cmd.Parameters.AddWithValue("@end_Time", DateTime.Now);
            sqlCon.Open();

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            string result = Convert.ToString(returnParameter.Value);

            return Convert.ToString(result);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public List<string> GetMistakeXmlStatus(string bookId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        string strQuery = "GetMistakeXmlStatus";
        SqlCommand cmd = new SqlCommand(strQuery, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BookId", bookId.Trim());

        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        List<string> list = null;

        using (con)
        {
            if (dr.HasRows)
            {
                list = new List<string>();

                while (dr.Read())
                {
                    list.Add(Convert.ToString(dr["UserId"]) + "," + Convert.ToString(dr["TaskStatus"]));
                }
            }
        }

        return list;
    }

    public List<string> GetTasks_StatusByBookId(string bookId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        string strQuery = "GetTasks_StatusByBookId";
        SqlCommand cmd = new SqlCommand(strQuery, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@BookId", bookId.Trim());

        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        List<string> list = null;

        using (con)
        {
            if (dr.HasRows)
            {
                list = new List<string>();
                while (dr.Read())
                {
                    list.Add(Convert.ToString(dr["UID"]) + "," + Convert.ToString(dr["Task"]) + "," + Convert.ToString(dr["Status"]));
                }
            }
        }

        return list;
    }

    //Aamir Methods 

    public string TableTest
    {
        get
        {
            return System.Configuration.ConfigurationManager.AppSettings["TableTest"];
        }
    }
    public string IndexingTest
    {
        get
        {
            return System.Configuration.ConfigurationManager.AppSettings["IndexingTest"];
        }
    }
    public string ImagesTest
    {
        get
        {
            return System.Configuration.ConfigurationManager.AppSettings["ImagesTest"];
        }
    }
    public string MappingTest
    {
        get
        {
            return System.Configuration.ConfigurationManager.AppSettings["MappingTest"];
        }
    }

    public string OnlineUsers()
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "SP_COUNT_ACTIVE_USER";
            string totalUsers = "";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();

            using (con)
            {
                totalUsers = Convert.ToString(cmd.ExecuteScalar());
            }

            return totalUsers;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string CheckTestStatus(string name, string email, string testType)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetTestStatus";
            string TestName = "";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name.Trim());
            cmd.Parameters.AddWithValue("@Email", email.Trim());
            cmd.Parameters.AddWithValue("@testtype", testType.Trim());

            con.Open();

            using (con)
            {
                TestName = Convert.ToString(cmd.ExecuteScalar());
            }

            return TestName;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string InsertOnlineUsers(string userName)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("SP_INSERT_ACTIVE_USER", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@NAME", userName);
            con.Open();
            rows = cmd.ExecuteNonQuery();
            return Convert.ToString(rows);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    #region UpdateMailStatus(string MID,string UserName)

    #endregion


    public string InsertOnlineTest(string name, string email, string testName, string ipAdress, int totalmarks, string testType)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("AddOnlineTest", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name.Trim());
            cmd.Parameters.AddWithValue("@Email", email.Trim());
            cmd.Parameters.AddWithValue("@TestName", testName.Trim());
            cmd.Parameters.AddWithValue("@IPAdress", ipAdress.Trim());
            cmd.Parameters.AddWithValue("@totalmarks", totalmarks);
            cmd.Parameters.AddWithValue("@TestType", testType.Trim());
            cmd.Parameters.AddWithValue("@TestStartTime", DateTime.Now);

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            return Convert.ToString(result);

            //sqlCon.Open();
            //rows = cmd.ExecuteNonQuery();
            //return Convert.ToString(rows);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public List<TestUser> GetUserDetails_ByIPAdress(string name, string email, string ipAddress)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "CheckIpAddress";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name.Trim());
            cmd.Parameters.AddWithValue("@Email", email.Trim());
            cmd.Parameters.AddWithValue("@IPAddress", ipAddress.Trim());
            List<TestUser> list = null;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            using (con)
            {
                if (dr.HasRows)
                {
                    list = new List<TestUser>();
                    while (dr.Read())
                    {
                        list.Add(new TestUser
                        {
                            FullName = Convert.ToString(dr["name"]),
                            Email = Convert.ToString(dr["email"]),
                            TestDate = Convert.ToString(dr["TestStartTime"]),
                            Status = Convert.ToString(dr["status"])
                        });
                    }
                }
            }

            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public List<string> GetEditor_ClearedTestsList(string name)
    {
        string userId = GetPassedUserId(name);

        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetUserMainDetails";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());

            List<string> list = null;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            using (con)
            {
                if (dr.HasRows)
                {
                    list = new List<string>();
                    while (dr.Read())
                    {
                        list.Add(Convert.ToString(dr["TestType"]));
                    }
                }
            }

            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string GetPassedUserId(string name)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetUserId_ByName";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name.Trim());

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            string userId = "";

            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        userId = Convert.ToString(dr["uid"]);
                    }
                }
            }

            return userId;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public int insertOnlineTestDetails(string testtype, string uid, string testName, string status, string startTime)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("INSERT_ONLINE_TEST_DETAILS", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TESTTYPE", testtype);
            cmd.Parameters.AddWithValue("@UID", uid);
            cmd.Parameters.AddWithValue("@STATUS", status);
            cmd.Parameters.AddWithValue("@TESTNAME", testName);
            cmd.Parameters.AddWithValue("@STARTTIME", startTime);

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            return rows;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public int updateOnlineTestDetails(string testtype, string uid, string status, string endtime)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("UPDATE_ONLINE_TEST_STATUS", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TESTTYPE", testtype);
            cmd.Parameters.AddWithValue("@UID", uid);
            cmd.Parameters.AddWithValue("@STATUS", status);
            cmd.Parameters.AddWithValue("@ENDTIME", endtime);

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            return rows;
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }
    public string InsertOnlineTest_IPAddress(string name, string email, string ipAdress)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("InsertOnlineTest_IPAddress", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name.Trim());
            cmd.Parameters.AddWithValue("@Email", email.Trim());
            cmd.Parameters.AddWithValue("@IPAdress", ipAdress.Trim());
            cmd.Parameters.AddWithValue("@TestStartTime", DateTime.Now);

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            return Convert.ToString(result);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public int GetInProcessTestName(string name, string email)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "Get_INProcess_TestName";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name.Trim());
            cmd.Parameters.AddWithValue("@Email", email.Trim());

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            string TestName = "";
            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        TestName = Convert.ToString(dr["TestName"]);
                    }
                }
            }

            return Convert.ToInt32(TestName);
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

    public string GetUserIdByEmail(string email)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetUserIdByEmail";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email.Trim());

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            string userId = "";
            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        userId = Convert.ToString(dr["uid"]);
                    }
                }
            }

            return userId;
        }
        catch
        {
            return "";
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string GetEmailByUserId(string uid)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetEmailByUserId";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", uid.Trim());

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            string email = "";
            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        email = Convert.ToString(dr["email"]);
                    }
                }
            }

            return email;
        }
        catch
        {
            return "";
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string CalculateTaskTime(string complexity)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "CalculateTaskTime";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Complexity", complexity.Trim());

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            string onePageTimeInSec = "";
            string bookUnitTime = "";

            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        onePageTimeInSec = Convert.ToString(dr["onePageTimeInSec"]);
                        bookUnitTime = Convert.ToString(dr["BOOK_UNIT_TIME"]);
                    }
                }
            }

            return onePageTimeInSec + "," + bookUnitTime;
        }
        catch
        {
            return "";
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string GetTaskStartingTime(string bookId, string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetTaskStartingTime";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId.Trim());
            cmd.Parameters.AddWithValue("@UserId", userId.Trim());

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            string taskStartingDate = "";
            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        taskStartingDate = Convert.ToString(dr["TaskStartDate"]);
                    }
                }
            }

            return taskStartingDate;
        }
        catch
        {
            return "";
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public int GetTestName(string name, string email)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetTestName";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name.Trim());
            cmd.Parameters.AddWithValue("@Email", email.Trim());

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            string TestName = "";
            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        TestName = Convert.ToString(dr["TestName"]);
                    }
                }
            }

            return Convert.ToInt32(TestName);
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

    public List<string> GetEmailId_ByName(string name)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetEmailId_ByName";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name.Trim());
            List<string> list = null;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            using (con)
            {
                if (dr.HasRows)
                {
                    list = new List<string>();
                    while (dr.Read())
                    {
                        list.Add(Convert.ToString(dr["Email"]));
                    }
                }
            }

            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string SaveResult(string name, string email, string testName, string obtainedMarks, string status)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("SaveOnlineTestResult", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name", name.Trim());
            cmd.Parameters.AddWithValue("@email", email.Trim());
            cmd.Parameters.AddWithValue("@obtainedMarks", obtainedMarks.Trim());
            cmd.Parameters.AddWithValue("@status", status.Trim());
            cmd.Parameters.AddWithValue("@testName", testName.Trim());
            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            return Convert.ToString(rows);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public string MovePassedResult(string name, string email)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("MovePassedTestData", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name", name.Trim());
            cmd.Parameters.AddWithValue("@email", email.Trim());
            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            var result = returnParameter.Value;

            return Convert.ToString(result);


        }
        catch
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    //public string SavePassedResultOld(string profileName, string password, string name, string email, string idCardNum, string mobileNum, string education, string accountNumber, string experience, string description, string imageName, string AccountTitle, string AccountType, string BankName, string Branch, string BankCode, string City, string Country)
    //{
    //    SqlConnection sqlCon = new SqlConnection(ConnectionString());
    //    try
    //    {
    //        int rows = 0;
    //        SqlCommand cmd = new SqlCommand("SavePassedResult", sqlCon);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.Parameters.AddWithValue("@profileName", profileName.Trim());
    //        cmd.Parameters.AddWithValue("@password", password.Trim());
    //        cmd.Parameters.AddWithValue("@fullname", name.Trim());
    //        cmd.Parameters.AddWithValue("@email", email.Trim());
    //        cmd.Parameters.AddWithValue("@idCardNum", idCardNum.Trim());
    //        cmd.Parameters.AddWithValue("@mobileNum", mobileNum.Trim());
    //        cmd.Parameters.AddWithValue("@education", education.Trim() == "Please Select" ? "" : education.Trim());
    //        cmd.Parameters.AddWithValue("@accountNumber", accountNumber.Trim());
    //        cmd.Parameters.AddWithValue("@experience", experience.Trim());
    //        cmd.Parameters.AddWithValue("@imagePath", imageName.Trim());
    //        cmd.Parameters.AddWithValue("@description", description.Trim());
    //        cmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now.Date);

    //        cmd.Parameters.AddWithValue("@AccountTitle", AccountTitle.Trim());
    //        cmd.Parameters.AddWithValue("@AccountType", AccountType.Trim());
    //        cmd.Parameters.AddWithValue("@BankName", BankName.Trim());
    //        cmd.Parameters.AddWithValue("@Branch", Branch.Trim());
    //        cmd.Parameters.AddWithValue("@BankCode", BankCode.Trim());
    //        cmd.Parameters.AddWithValue("@City", City.Trim());
    //        cmd.Parameters.AddWithValue("@Country", Country.Trim());
    //        //cmd.Parameters.AddWithValue("@userId", 0);
    //        //cmd.Parameters["@UserId"].Direction = ParameterDirection.Output;

    //        var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
    //        returnParameter.Direction = ParameterDirection.ReturnValue;

    //        sqlCon.Open();
    //        rows = cmd.ExecuteNonQuery();
    //        var result = returnParameter.Value;
    //        //int userId = Convert.ToInt32(cmd.Parameters["@UserId"].Value);

    //        //return Convert.ToString(result + "," + userId);
    //        return Convert.ToString(result);
    //    }
    //    catch (Exception ex)
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        if (sqlCon.State == ConnectionState.Open)
    //        {
    //            sqlCon.Close();
    //        }
    //    }
    //}

    public string SavePassedUser(TestUser user)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("SavePassedResult", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@profileName", user.ProfileName);
            cmd.Parameters.AddWithValue("@email", user.Email);
            cmd.Parameters.AddWithValue("@password", user.Password);
            cmd.Parameters.AddWithValue("@fullname", user.FullName);
            cmd.Parameters.AddWithValue("@gender", user.Gender);
            cmd.Parameters.AddWithValue("@idCardNum", user.IdCardNum);
            cmd.Parameters.AddWithValue("@mobileNum", user.MobileNumber);
            cmd.Parameters.AddWithValue("@education", user.Education == "Please Select" ? "" : user.Education);
            cmd.Parameters.AddWithValue("@experience", user.Experience);
            cmd.Parameters.AddWithValue("@imagePath", user.ImagPath.Trim());
            cmd.Parameters.AddWithValue("@description", user.Description);
            cmd.Parameters.AddWithValue("@CurrentDate", DateTime.Now.Date);

            cmd.Parameters.AddWithValue("@accountNumber", user.AccountNum);
            cmd.Parameters.AddWithValue("@AccountTitle", user.AccountTitle.Trim());
            cmd.Parameters.AddWithValue("@AccountType", user.AccountType.Trim());
            cmd.Parameters.AddWithValue("@BankName", user.BankName.Trim());
            cmd.Parameters.AddWithValue("@Branch", user.Branch.Trim());
            cmd.Parameters.AddWithValue("@BankCode", user.BankCode.Trim());
            cmd.Parameters.AddWithValue("@City", user.City.Trim());
            cmd.Parameters.AddWithValue("@Country", user.Country.Trim());
            cmd.Parameters.AddWithValue("@CountryOfResidence", user.CountryOfResidence.Trim());
            cmd.Parameters.AddWithValue("@PaypalNum", user.PaypalNum.Trim());
            //cmd.Parameters.AddWithValue("@userId", 0);
            //cmd.Parameters["@UserId"].Direction = ParameterDirection.Output;

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            //int userId = Convert.ToInt32(cmd.Parameters["@UserId"].Value);

            //return Convert.ToString(result + "," + userId);
            return Convert.ToString(result);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }


    public string SavePassedUserLogin(string fullName, string email)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("CreatePassedUserLogin", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Fullname", fullName.Trim());
            cmd.Parameters.AddWithValue("@Email", email.Trim());

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            //int userId = Convert.ToInt32(cmd.Parameters["@UserId"].Value);

            //return Convert.ToString(result + "," + userId);
            return Convert.ToString(result);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public string Save_BankDetails(string AccountTitle, string AccountType, string BankName, string Branch, string BankCode, string City, string Country)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("SaveBankDetails", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@AccountTitle", AccountTitle.Trim());
            cmd.Parameters.AddWithValue("@AccountType", AccountType.Trim());
            cmd.Parameters.AddWithValue("@BankName", BankName.Trim());
            cmd.Parameters.AddWithValue("@Branch", Branch.Trim());
            cmd.Parameters.AddWithValue("@BankCode", BankCode.Trim());
            cmd.Parameters.AddWithValue("@City", City.Trim());
            cmd.Parameters.AddWithValue("@Country", Country.Trim());

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();

            return Convert.ToString(rows);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public string Edit_BankDetails(string uid, string AccountNumber, string AccountTitle, string AccountType, string BankName, string Branch, string BankCode, string City, string Country)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("EditBankDetails", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", uid.Trim());
            cmd.Parameters.AddWithValue("@AccountNumber", AccountNumber.Trim());
            cmd.Parameters.AddWithValue("@AccountTitle", AccountTitle.Trim());
            cmd.Parameters.AddWithValue("@AccountType", AccountType.Trim());
            cmd.Parameters.AddWithValue("@BankName", BankName.Trim());
            cmd.Parameters.AddWithValue("@Branch", Branch.Trim());
            cmd.Parameters.AddWithValue("@BankCode", BankCode.Trim());
            cmd.Parameters.AddWithValue("@City", City.Trim());
            cmd.Parameters.AddWithValue("@Country", Country.Trim());

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();

            return Convert.ToString(rows);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public int ChangePassword(string email, string uid, string oldPassword, string newPassword)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("ChangePassword", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email.Trim());
            cmd.Parameters.AddWithValue("@UserId", uid.Trim());
            cmd.Parameters.AddWithValue("@OldPassword", oldPassword.Trim());
            cmd.Parameters.AddWithValue("@NewPassword", newPassword.Trim());

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();

            return Convert.ToInt32(rows);
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public string ClearResetPasswordKey(string uid, string key)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("ClearResetPasswordKey", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", uid.Trim());
            cmd.Parameters.AddWithValue("@ResetPasswordKey", key.Trim());
            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();

            return Convert.ToString(rows);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public int ResetPassword(string uid, string email, string newPassword)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("ResetPassword", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", uid.Trim());
            cmd.Parameters.AddWithValue("@Email", email.Trim());
            cmd.Parameters.AddWithValue("@NewPassword", newPassword.Trim());

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();

            return rows;
        }
        catch (Exception ex)
        {
            return -1;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public int InsertResetPasswordKey(string uid, string key)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("InsertResetPasswordKey", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", uid.Trim());
            cmd.Parameters.AddWithValue("@ResetPasswordKey", key.Trim());

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();

            return rows;
        }
        catch (Exception ex)
        {
            return -1;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public string GetResetPasswordKey(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string key = string.Empty;
            string strQuery = "GetResetPasswordKey";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandTimeout = 120;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId.Trim());
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        key = Convert.ToString(dr["ResetPasswordKey"]);
                    }
                }
            }

            return key;
        }
        catch (Exception ex)
        {
            return "";
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public int GetTotalScore(string name, string email, string testName)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            int totalScore = 0;
            string strQuery = "GetTotalScore";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandTimeout = 120;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name.Trim());
            cmd.Parameters.AddWithValue("@Email", email.Trim());
            cmd.Parameters.AddWithValue("@TestName", testName.Trim());
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        totalScore = Convert.ToInt32(dr["TotalScore"]);
                    }
                }
            }

            return totalScore;
        }
        catch (Exception ex)
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

    public string CheckTestStatus(string name, string email)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetTestStatus";
            string userName = "";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name.Trim());
            cmd.Parameters.AddWithValue("@Email", email.Trim());

            con.Open();

            using (con)
            {
                userName = Convert.ToString(cmd.ExecuteScalar());
            }

            return userName;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public List<string> GetTestDate(string name, string email)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetTestDate";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name.Trim());
            cmd.Parameters.AddWithValue("@Email", email.Trim());
            //cmd.Parameters.AddWithValue("@IPAddress", ipAddress.Trim());
            List<string> list = null;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            using (con)
            {
                if (dr.HasRows)
                {
                    list = new List<string>();
                    while (dr.Read())
                    {
                        list.Add(Convert.ToString(dr["TestStartTime"]));
                    }
                }
            }

            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    //public string GetTestDate(string name, string email, string ipAddress)
    //{
    //    SqlConnection con = new SqlConnection(ConnectionString());
    //    try
    //    {
    //        string strQuery = "GetTestDate";
    //        string testDate = "";
    //        SqlCommand cmd = new SqlCommand(strQuery, con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.Parameters.AddWithValue("@Name", name.Trim());
    //        cmd.Parameters.AddWithValue("@Email", email.Trim());
    //        cmd.Parameters.AddWithValue("@IPAddress", ipAddress.Trim());

    //        con.Open();

    //        using (con)
    //        {
    //            userName = Convert.ToString(cmd.ExecuteScalar());
    //        }

    //        return userName;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        if (con.State == ConnectionState.Open)
    //        {
    //            con.Close();
    //        }
    //    }
    //}

    public string CheckIpAddress(string name, string email)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "CheckIpAddress";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name.Trim());
            cmd.Parameters.AddWithValue("@Email", email.Trim());

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            string IpAdress = "";
            string Date = "";
            string TestName = "";

            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        IpAdress = Convert.ToString(dr["IPAdress"]);
                        Date = Convert.ToString(dr["TestStartTime"]);
                        TestName = Convert.ToString(dr["TestName"]);
                    }
                }
            }

            return IpAdress + "," + Date + ',' + TestName;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string GetPassedUserDetails(string Id)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetTestName";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", Id.Trim());

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            string Name = "";
            string Email = "";

            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        Name = Convert.ToString(dr["Name"]);
                        Email = Convert.ToString(dr["Email"]);
                    }
                }
            }

            return Name + "," + Email;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public List<TestUser> GetUserProfile(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetUserMainDetails";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = null;
            List<TestUser> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<TestUser>();

                    while (dr.Read())
                    {
                        list.Add(new TestUser
                        {
                            FullName = Convert.ToString(dr["UName"]),
                            Description = Convert.ToString(dr["Description"]),
                            TestType = Convert.ToString(dr["TestType"]),
                            Picture = Convert.ToString(dr["Picture"]),
                            Gender = Convert.ToString(dr["gender"])
                        });
                    }
                }
            }
            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public TestUser GetUserEdit_Profile(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetUserDetails";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = null;
            TestUser user = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        user = new TestUser
                        {
                            ProfileName = Convert.ToString(dr["UName"]),
                            FullName = Convert.ToString(dr["UserName"]),
                            Password = Convert.ToString(dr["Password"]),
                            Description = Convert.ToString(dr["Description"]),
                            Education = Convert.ToString(dr["Education"]),
                            Experience = Convert.ToString(dr["Experience"]),
                            IdCardNum = Convert.ToString(dr["IdCardNum"]),
                            MobileNumber = Convert.ToString(dr["MobileNumber"]),
                            Picture = Convert.ToString(dr["Picture"]),
                            Email = Convert.ToString(dr["Email"]),
                            CNICNO = Convert.IsDBNull(dr["IdCardNum"]) ? "" : Convert.ToString(dr["IdCardNum"]),
                            AccountNum = Convert.IsDBNull(dr["AccountNumber"]) ? "" : Convert.ToString(dr["AccountNumber"]),
                            Gender = Convert.ToString(dr["Gender"]),
                            PaypalNum = Convert.ToString(dr["PaypalAccountNum"]),
                            CountryOfResidence = Convert.ToString(dr["Country"]),
                            AccountTitle = Convert.IsDBNull(dr["AccountTitle"]) ? "" : Convert.ToString(dr["AccountTitle"]),

                            AccountType = Convert.IsDBNull(dr["AccountType"]) ? "" : Convert.ToString(dr["AccountType"]),
                            BankName = Convert.IsDBNull(dr["Bank"]) ? "" : Convert.ToString(dr["Bank"]),
                            Branch = Convert.IsDBNull(dr["Branch"]) ? "" : Convert.ToString(dr["Branch"]),
                            BankCode = Convert.IsDBNull(dr["BankCode"]) ? "" : Convert.ToString(dr["BankCode"]),
                            City = Convert.IsDBNull(dr["City"]) ? "" : Convert.ToString(dr["City"]),
                            Country = Convert.IsDBNull(dr["Country"]) ? "" : Convert.ToString(dr["Country"]),

                        };
                    }
                }
            }
            return user;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public BankDetails GetUserEdit_BankDetails(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetUserBankDetails";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = null;
            BankDetails user = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        user = new BankDetails
                        {
                            AccountNo = Convert.ToString(dr["AccountNo"]),
                            AccountTitle = Convert.ToString(dr["AccountTitle"]),
                            AccountType = Convert.ToString(dr["AccountType"]),
                            Bank = Convert.ToString(dr["Bank"]),
                            Branch = Convert.ToString(dr["Branch"]),
                            BankCode = Convert.ToString(dr["BankCode"]),
                            City = Convert.ToString(dr["City"]),
                            Country = Convert.ToString(dr["Country"]),
                            TotalAmount = Convert.ToString(dr["TotalAmount"]),
                            UnpaidAmount = Convert.ToString(dr["UnpaidAmount"])
                        };
                    }
                }
            }
            return user;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public TestUser GetUserProfile_ForAdmin(string name, string email)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetUserProfile";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name", name.Trim());
            cmd.Parameters.AddWithValue("@email", email.Trim());

            con.Open();
            SqlDataReader dr = null;
            TestUser user = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        user = new TestUser
                        {
                            ProfileName = Convert.ToString(dr["UName"]),
                            FullName = Convert.ToString(dr["UserName"]),
                            Description = Convert.ToString(dr["Description"]),
                            Education = Convert.ToString(dr["Education"]),
                            Experience = Convert.ToString(dr["Experience"]),
                            IdCardNum = Convert.ToString(dr["IdCardNum"]),
                            MobileNumber = Convert.ToString(dr["MobileNumber"]),
                            Picture = Convert.ToString(dr["Picture"]),
                            Email = Convert.ToString(dr["Email"]),
                            CNICNO = Convert.ToString(dr["IdCardNum"]),
                            AccountNum = Convert.ToString(dr["AccountNumber"])
                        };
                    }
                }
            }
            return user;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public int UpdateUserProfile(string userId, string profileName, string fullName, string accountNumber, string idCardNumber, string mobileNumber,
        string education, string experience, string description, string imagePath, string gender, string paypalAccount, string countryOfResidence,
        string bankName, string bankAccountNumber, string accountTitle, string accountType, string branch, string bankCode, string city, string country)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("UpdateUserDetails", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());
            cmd.Parameters.AddWithValue("@profileName", profileName.Trim());
            cmd.Parameters.AddWithValue("@fullName", fullName.Trim());
            cmd.Parameters.AddWithValue("@accountNumber", accountNumber.Trim());
            cmd.Parameters.AddWithValue("@idCardNumber", idCardNumber.Trim());
            cmd.Parameters.AddWithValue("@mobileNumber", mobileNumber.Trim());
            cmd.Parameters.AddWithValue("@education", education.Trim());
            cmd.Parameters.AddWithValue("@experience", experience.Trim());
            cmd.Parameters.AddWithValue("@description", description.Trim());
            cmd.Parameters.AddWithValue("@imagePath", imagePath);
            cmd.Parameters.AddWithValue("@gender", gender);
            cmd.Parameters.AddWithValue("@PaypalNum", paypalAccount.Trim());
            cmd.Parameters.AddWithValue("@CountryOfResidence", countryOfResidence.Trim());

            cmd.Parameters.AddWithValue("@bankName", bankName.Trim());
            cmd.Parameters.AddWithValue("@accountTitle", accountTitle.Trim());
            cmd.Parameters.AddWithValue("@accountType", accountType.Trim());
            cmd.Parameters.AddWithValue("@branch", branch.Trim());
            cmd.Parameters.AddWithValue("@bankCode", bankCode.Trim());
            cmd.Parameters.AddWithValue("@city", city.Trim());
            cmd.Parameters.AddWithValue("@country", country.Trim());

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            con.Open();
            rows = cmd.ExecuteNonQuery();
            int result = Convert.ToInt32(returnParameter.Value);
            return Convert.ToInt32(result);
        }
        catch
        {
            return -1;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public List<OnlineTest> GetUserAvailableTasks(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetAvailableTasks";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = null;
            List<OnlineTest> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<OnlineTest>();

                    while (dr.Read())
                    {
                        list.Add(new OnlineTest
                        {
                            Description = Convert.ToString(dr["Comments"]),
                            TaskName = Convert.ToString(dr["Task"]),
                            TaskStatus = Convert.ToString(dr["Status"]),
                            AID = Convert.ToString(dr["AID"]),
                            DueDate = Convert.ToString(dr["DeadLine"]),
                            BID = Convert.ToString(dr["BID"]),
                            Complexity = Convert.ToString(dr["Complexity"]),
                            PageCount = Convert.ToString(dr["PageCount"]),
                            OnePageTime_InSeconds = Convert.ToString(dr["IN_SECONDS"]),
                            BookId = Convert.ToString(dr["MainBook"]),
                            Priority = Convert.ToString(dr["Priority"])
                        });
                    }
                }
            }
            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    //public List<OnlineTest> GetApprovedTasks(string taskType)
    //{
    //    SqlConnection con = new SqlConnection(ConnectionString());
    //    try
    //    {
    //        string strQuery = "GetApprovedTasks";
    //        SqlCommand cmd = new SqlCommand(strQuery, con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.Parameters.AddWithValue("@TaskType", taskType.Trim());

    //        con.Open();
    //        SqlDataReader dr = null;
    //        List<OnlineTest> list = null;

    //        using (con)
    //        {
    //            dr = cmd.ExecuteReader();
    //            if (dr.HasRows)
    //            {
    //                list = new List<OnlineTest>();

    //                while (dr.Read())
    //                {
    //                    list.Add(new OnlineTest
    //                    {
    //                        BID = Convert.ToString(dr["BID"]),
    //                        TaskAssigmentDate = Convert.IsDBNull(dr["AssigmentDate"]) ? "" : Convert.ToString(dr["AssigmentDate"]),
    //                        TaskDeadLine = Convert.IsDBNull(dr["DeadLine"]) ? "" : Convert.ToString(dr["DeadLine"]),


    //                        TaskName = Convert.ToString(dr["Task"]),
    //                        TaskCompletionDate = Convert.IsDBNull(dr["CompletionDate"]) ? "" : Convert.ToString(dr["CompletionDate"]),

    //                        Description = Convert.IsDBNull(dr["Comments"]) ? "" : Convert.ToString(dr["Comments"]),
    //                        Priority = Convert.IsDBNull(dr["Priority"]) ? "" : Convert.ToString(dr["Priority"]),
    //                        TaskStatus = Convert.IsDBNull(dr["Status"]) ? "" : Convert.ToString(dr["Status"]),
    //                        TaskCost = Convert.IsDBNull(dr["Cost"]) ? 0 : Convert.ToDouble(dr["Cost"]),

    //                        AID = Convert.ToString(dr["AID"]),
    //                        Complexity = Convert.ToString(dr["Complexity"]),
    //                        PageCount = Convert.IsDBNull(dr["PageCount"]) ? "" : Convert.ToString(dr["PageCount"]),
    //                        PageViewed = Convert.IsDBNull(dr["PageViewed"]) ? 0 : Convert.ToInt32(dr["PageViewed"]),
    //                        FinishTaskCount = Convert.IsDBNull(dr["FinishTaskCount"]) ? 0 : Convert.ToInt32(dr["FinishTaskCount"]),

    //                        FullName = Convert.IsDBNull(dr["UserName"]) ? "" : Convert.ToString(dr["UserName"])
    //                    });
    //                }
    //            }
    //        }
    //        return list;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        if (con.State == ConnectionState.Open)
    //        {
    //            con.Close();
    //        }
    //    }
    //}

    public List<OnlineTest> GetTasksByStatus(string taskType, string taskStatus)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetTasksByStatus";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TaskStatus", taskStatus.Trim());
            cmd.Parameters.AddWithValue("@TaskType", taskType.Trim());

            con.Open();
            SqlDataReader dr = null;
            List<OnlineTest> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<OnlineTest>();

                    while (dr.Read())
                    {
                        list.Add(new OnlineTest
                        {
                            BookId = Convert.ToString(dr["MainBook"]),
                            TaskAssigmentDate = Convert.IsDBNull(dr["AssigmentDate"]) ? "" : Convert.ToString(dr["AssigmentDate"]),
                            TaskDeadLine = Convert.IsDBNull(dr["DeadLine"]) ? "" : Convert.ToString(dr["DeadLine"]),


                            TaskName = Convert.ToString(dr["Task"]),
                            TaskCompletionDate = Convert.IsDBNull(dr["CompletionDate"]) ? "" : Convert.ToString(dr["CompletionDate"]),

                            Description = Convert.IsDBNull(dr["Comments"]) ? "" : Convert.ToString(dr["Comments"]),
                            Priority = Convert.IsDBNull(dr["Priority"]) ? "" : Convert.ToString(dr["Priority"]),
                            TaskStatus = Convert.IsDBNull(dr["Status"]) ? "" : Convert.ToString(dr["Status"]),
                            TaskCost = Convert.IsDBNull(dr["Cost"]) ? 0 : Convert.ToDouble(dr["Cost"]),

                            AID = Convert.ToString(dr["AID"]),
                            Complexity = Convert.ToString(dr["Complexity"]),
                            PageCount = Convert.IsDBNull(dr["PageCount"]) ? "" : Convert.ToString(dr["PageCount"]),
                            PageViewed = Convert.IsDBNull(dr["PageViewed"]) ? 0 : Convert.ToInt32(dr["PageViewed"]),

                            DetectedInjectedMistakes = Convert.IsDBNull(dr["DetectedInjectedMistakes"]) ? 0 : Convert.ToInt32(dr["DetectedInjectedMistakes"]),
                            TotalInjectedMistakes = Convert.IsDBNull(dr["TotalInjectedMistakes"]) ? 0 : Convert.ToInt32(dr["TotalInjectedMistakes"]),
                            DetectedOtherMistakes = Convert.IsDBNull(dr["OtherDetectedMistakes"]) ? 0 : Convert.ToInt32(dr["OtherDetectedMistakes"]),

                            FinishTaskCount = Convert.IsDBNull(dr["FinishTaskCount"]) ? 0 : Convert.ToInt32(dr["FinishTaskCount"]),

                            FullName = Convert.IsDBNull(dr["UserName"]) ? "" : Convert.ToString(dr["UserName"])
                        });
                    }
                }
            }
            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    //public List<Transaction> GetWithdrawAmountRequests(string taskStatus)
    //{
    //    SqlConnection con = new SqlConnection(ConnectionString());
    //    try
    //    {
    //        string strQuery = "GetWithdrawAmountRequests";
    //        SqlCommand cmd = new SqlCommand(strQuery, con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.Parameters.AddWithValue("@Status", taskStatus.Trim());
    //        con.Open();
    //        SqlDataReader dr = null;
    //        List<Transaction> list = null;

    //        using (con)
    //        {
    //            dr = cmd.ExecuteReader();
    //            if (dr.HasRows)
    //            {
    //                list = new List<Transaction>();

    //                while (dr.Read())
    //                {
    //                    list.Add(new Transaction
    //                    {
    //                        WithdrawId = Convert.IsDBNull(dr["WId"]) ? "" : Convert.ToString(dr["WId"]),
    //                        UserId = Convert.IsDBNull(dr["UserId"]) ? "" : Convert.ToString(dr["UserId"]),
    //                        UserName = Convert.IsDBNull(dr["userName"]) ? "" : Convert.ToString(dr["userName"]),
    //                        WithdrawAmount = Convert.IsDBNull(dr["WithdrawAmount"]) ? "" : Convert.ToString(dr["WithdrawAmount"]),
    //                        WithdrawDate = Convert.IsDBNull(dr["WithdrawDate"]) ? DateTime.MinValue : Convert.ToDateTime(dr["WithdrawDate"]),
    //                        Status = Convert.IsDBNull(dr["Status"]) ? "" : Convert.ToString(dr["Status"])
    //                    });
    //                }
    //            }
    //        }
    //        return list;
    //    }
    //    catch
    //    {
    //        return null;
    //    }
    //    finally
    //    {
    //        if (con.State == ConnectionState.Open)
    //        {
    //            con.Close();
    //        }
    //    }
    //}

    public List<Transaction> GetWithdrawAmountRequests(string taskStatus)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetWithdrawAmountRequests";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Status", taskStatus.Trim());
            con.Open();
            SqlDataReader dr = null;
            List<Transaction> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<Transaction>();

                    while (dr.Read())
                    {
                        list.Add(new Transaction
                        {
                            UserName = Convert.IsDBNull(dr["userName"]) ? "" : Convert.ToString(dr["userName"]),
                            WithdrawId = Convert.IsDBNull(dr["WId"]) ? "" : Convert.ToString(dr["WId"]),
                            UserId = Convert.IsDBNull(dr["UserId"]) ? "" : Convert.ToString(dr["UserId"]),
                            WithdrawAmount = Convert.IsDBNull(dr["WithdrawAmount"]) ? "" : Convert.ToString(dr["WithdrawAmount"]),
                            WithdrawDate = Convert.IsDBNull(dr["WithdrawDate"]) ? DateTime.MinValue : Convert.ToDateTime(dr["WithdrawDate"]),
                            Status = Convert.IsDBNull(dr["Status"]) ? "" : Convert.ToString(dr["Status"]),
                            TransactionAmount = Convert.IsDBNull(dr["TransactionAmount"]) ? "" : Convert.ToString(dr["TransactionAmount"]),
                            TransactionDate = Convert.IsDBNull(dr["TransactionDate"]) ? DateTime.MinValue : Convert.ToDateTime(dr["TransactionDate"]),
                            TransactionId = Convert.IsDBNull(dr["TransactionId"]) ? "" : Convert.ToString(dr["TransactionId"]),
                            TransactionType = Convert.IsDBNull(dr["TransactionType"]) ? "" : Convert.ToString(dr["TransactionType"])
                        });
                    }
                }
            }
            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public List<OnlineTest> GetArchiveTasks(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetArchiveTasks";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = null;
            List<OnlineTest> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<OnlineTest>();

                    while (dr.Read())
                    {
                        list.Add(new OnlineTest
                        {
                            Description = dr["Comments"] == DBNull.Value ? "" : Convert.ToString(dr["Comments"]),
                            TaskName = dr["Task"] == DBNull.Value ? "" : Convert.ToString(dr["Task"]),
                            TaskStatus = dr["Status"] == DBNull.Value ? "" : Convert.ToString(dr["Status"]),
                            AID = dr["AID"] == DBNull.Value ? "" : Convert.ToString(dr["AID"]),
                            DueDate = dr["DeadLine"] == DBNull.Value ? "" : Convert.ToString(dr["DeadLine"]),
                            BID = dr["BID"] == DBNull.Value ? "" : Convert.ToString(dr["BID"]),
                            Complexity = dr["Complexity"] == DBNull.Value ? "" : Convert.ToString(dr["Complexity"]),
                            PageCount = dr["PageCount"] == DBNull.Value ? "" : Convert.ToString(dr["PageCount"]),
                            OnePageTime_InSeconds = dr["IN_SECONDS"] == DBNull.Value ? "" : Convert.ToString(dr["IN_SECONDS"]),
                            BookId = dr["MainBook"] == DBNull.Value ? "" : Convert.ToString(dr["MainBook"]),
                            PayableEarning = dr["PayableEarning"] == DBNull.Value ? 0 : Convert.ToDouble(dr["PayableEarning"]),
                            Bonus = dr["Bonus"] == DBNull.Value ? 0 : Convert.ToDouble(dr["Bonus"])
                        });
                    }
                }
            }
            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public List<OnlineTest> GetTaskPriorities(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetTaskPriorities";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader dr = null;
            List<OnlineTest> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<OnlineTest>();

                    while (dr.Read())
                    {
                        list.Add(new OnlineTest
                        {
                            BID = Convert.ToString(dr["bid"]),
                            BookId = Convert.ToString(dr["MainBook"]),
                            TaskStatus = Convert.ToString(dr["Status"]),
                            Priority = Convert.ToString(dr["Priority"]),
                            TaskType = Convert.ToString(dr["Task"])
                        });
                    }
                }
            }
            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public List<OnlineTest> GetMyTasks(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetMyTasks";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = null;
            List<OnlineTest> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<OnlineTest>();

                    while (dr.Read())
                    {
                        list.Add(new OnlineTest
                        {
                            Description = Convert.ToString(dr["Comments"]),
                            TaskName = Convert.ToString(dr["Task"]),
                            TaskStatus = Convert.ToString(dr["Status"]),
                            AID = Convert.ToString(dr["AID"]),
                            DueDate = Convert.ToString(dr["DeadLine"]),
                            BookId = Convert.ToString(dr["MainBook"]),
                            Complexity = Convert.ToString(dr["Complexity"]),
                            PageCount = Convert.ToString(dr["PageCount"]),
                            OnePageTime_InSeconds = Convert.ToString(dr["IN_SECONDS"]),
                            BID = Convert.ToString(dr["BID"])
                        });
                    }
                }
            }
            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public List<TestUser> GetPassedTests_Count_ByUser(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetPassedTestCount";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = null;
            List<TestUser> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<TestUser>();

                    while (dr.Read())
                    {
                        list.Add(new TestUser
                        {
                            TestType = Convert.ToString(dr["TestType"]),
                            TotalScore = Convert.ToString(dr["TotalScore"]),
                            ObtainedScore = Convert.ToString(dr["ObtainedScore"])
                        });
                    }
                }
            }
            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public int GetFinishTaskClickedCount(string bookId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetFinishTaskClickedCount";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId.Trim());

            con.Open();
            SqlDataReader dr = null;
            int count = 0;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        count = Convert.ToInt32(dr["FinishBtnClickCount"]);
                    }
                }
            }
            return count;
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



    public List<OnlineTest> GetCompletedTasks_ByUser(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetCompletedTasksCount";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = null;
            List<OnlineTest> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<OnlineTest>();

                    while (dr.Read())
                    {
                        list.Add(new OnlineTest
                        {
                            TaskType = Convert.ToString(dr["Task"]),
                            BookId = Convert.ToString(dr["BID"])
                            //,
                            //TimelyDelivery = Convert.ToInt32(dr["TimelyDelivery"]),
                            //Quality = Convert.ToInt32(dr["Quality"]),
                            //Responsiveness = Convert.ToInt32(dr["Responsiveness"]),
                            //FeedBackDate = Convert.ToDateTime(dr["FeedBackDate"]),
                            //Rating = Convert.ToInt32(dr["Rating"]),
                            //Comments = Convert.ToString(dr["comments"])
                        });
                    }
                }
            }
            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public int GetProgressTasks_ByUser(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetInProgressTasksCount";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            int count = 0;
            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        count = Convert.ToInt32(dr["totalTasks"]);
                    }
                }
            }

            return count;
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

    public int GetUserRank(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetUserRank";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());
            int userName = 0;
            con.Open();

            using (con)
            {
                userName = Convert.ToInt32(cmd.ExecuteScalar());
            }

            return userName;
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

    public void ChangeTask_Status(string id)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            SqlCommand cmd = new SqlCommand("ChangeTask_Status", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Id", id);

            sqlCon.Open();
            cmd.ExecuteNonQuery();

        }
        catch (Exception ex)
        {

        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();

            }
        }
    }
    public string checkAlreadyExists(string bookId, string userId, string category)
    {
        try
        {
            string query = "select tblTaskDetails.Taskid,tbluserlogin.UserId from tblTaskDetails inner join tblTaskSheet on tblTaskDetails.TaskId=tblTaskSheet.TaskId inner join tbluserlogin on tblUserLogin.UserId=tblTaskSheet.UserId where tblTaskDetails.BookId='" + bookId + "' and tblTaskDetails.CatId=" + category;
            SqlConnection objConnection = new SqlConnection(ConnectionString_Workmeter());
            objConnection.Open();

            SqlCommand objCmd = new SqlCommand(query, objConnection);
            SqlDataReader objRs = objCmd.ExecuteReader();
            if (objRs.Read())
            {
                //string tasks = "Task id " + objRs[0].ToString() + " against user name " + objRs[1].ToString().ToUpper() + " already assigned.";
                string tasks = Convert.ToString(objRs[1]);
                objConnection.Close();
                objRs.Close();
                return tasks;
            }
            else
            {
                objConnection.Close();
                return "";
            }
        }
        catch (Exception ex)
        {

            throw ex;
        }

    }
    public TestUser Validate_User(string email, string password)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "Validate_User";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Email", email.Trim());
            cmd.Parameters.AddWithValue("@Password", password.Trim());

            con.Open();
            SqlDataReader dr = null;
            TestUser user = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        user = new TestUser
                        {
                            TestType = dr["TEST_TYPE"] == null ? "" : Convert.ToString(dr["TEST_TYPE"]),
                            FullName = Convert.ToString(dr["UserName"]),
                            UserId = Convert.ToString(dr["UID"]),
                            UserType = Convert.ToString(dr["UType"]),
                            Email = Convert.ToString(dr["Email"])
                        };
                    }
                }
            }
            return user;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public TestUser GetUserLoginDetails(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetUserLoginDetails";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId.Trim());
            con.Open();
            SqlDataReader dr = null;
            TestUser user = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        user = new TestUser
                        {
                            Email = Convert.ToString(dr["Email"]),
                            Password = Convert.ToString(dr["Password"]),

                        };
                    }
                }
            }
            return user;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public List<TestUser> GetFailedTestUsers()
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetFailedTestUsers";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = null;
            List<TestUser> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<TestUser>();

                    while (dr.Read())
                    {
                        list.Add(new TestUser
                        {
                            FullName = Convert.ToString(dr["Name"]),
                            Email = Convert.ToString(dr["Email"]),
                            TotalScore = Convert.ToString(dr["TotalScore"]),
                            ObtainedScore = Convert.ToString(dr["ObtainedScore"]),
                            Status = Convert.ToString(dr["Status"]),
                            TestDate = Convert.ToString(dr["TestStartTime"]),
                            //TestType = Convert.ToString(dr["TestType"]),
                            IPAddress = Convert.ToString(dr["IPAdress"]),
                            TestAttempts = Convert.ToString(dr["TestAttempts"])
                        });
                    }
                }
            }
            return list;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
    public List<TestDetail> getTestDetails(string uid, string testtype)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GET_TESTDETAIL_USERWISE";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UID", uid);
            cmd.Parameters.AddWithValue("@TESTTYPE", testtype);


            con.Open();
            SqlDataReader dr = null;
            List<TestDetail> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<TestDetail>();

                    while (dr.Read())
                    {
                        list.Add(new TestDetail
                        {
                            status = Convert.ToString(dr["STATUS"]),
                            testtype = Convert.ToString(dr["TEST_TYPE"]),
                            uid = Convert.ToString(dr["UID"]),
                            id = Convert.ToString(dr["ID"]),
                            testName = Convert.ToString(dr["TEST_NAME"])

                        });
                    }
                }
            }
            return list;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
    public List<TestUser> GetPassedTestUsers()
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetPassedTestUsers";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = null;
            List<TestUser> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<TestUser>();

                    while (dr.Read())
                    {
                        list.Add(new TestUser
                        {
                            FullName = Convert.ToString(dr["FullName"]).Trim(),
                            Email = Convert.ToString(dr["Email"]).Trim(),
                            TotalScore = Convert.ToString(dr["TotalScore"]).Trim(),
                            ObtainedScore = Convert.ToString(dr["ObtainedScore"]).Trim(),
                            Status = Convert.ToString(dr["Status"]).Trim(),
                            TestDate = Convert.ToString(dr["TestStartTime"]).Trim(),
                            TestName = Convert.ToString(dr["testname"]).Trim(),
                            IPAddress = Convert.ToString(dr["IPAdress"]).Trim(),
                            TestAttempts = Convert.ToString(dr["TestAttempts"]).Trim(),
                            ProfileName = Convert.ToString(dr["UserName"]).Trim(),
                            Password = Convert.ToString(dr["password"]).Trim(),
                            Verification = Convert.ToString(dr["verification"]).Trim()
                        });
                    }
                }
            }
            return list;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string VerifyUser(string name, string email)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("Verify_TestUser", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@name", name.Trim());
            cmd.Parameters.AddWithValue("@email", email.Trim());
            con.Open();
            rows = cmd.ExecuteNonQuery();
            return Convert.ToString(rows);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string GetPassedUserNameByUserId(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetFullNameByUserId";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId.Trim());

            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            string fullName = "";

            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        fullName = Convert.ToString(dr["UserName"]);
                    }
                }
            }

            return fullName;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string GetTaskTime(string toolName, string taskName, string complexity)
    {
        if (taskName.Trim().ToLower().Equals("images"))
        {
            taskName = taskName.Trim().ToLower().Remove(taskName.Length - 1, 1);
        }

        else if (taskName.Trim().ToLower().Equals("tables"))
        {
            taskName = taskName.Trim().ToLower().Remove(taskName.Length - 1, 1);
        }

        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetUser_TaskTime";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TOOL_NAME", toolName.Trim());
            cmd.Parameters.AddWithValue("@TASK_NAME", taskName.Trim());
            cmd.Parameters.AddWithValue("@COMPLEXITY", complexity.Trim().ToLower());

            con.Open();
            SqlDataReader dr = null;
            string amount = "";

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        amount = Convert.ToString(dr["task_TimeInSec"]);
                    }
                }
            }
            return amount;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public int GetUser_Salary(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetUser_Salary";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = null;
            int salary = 0;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {

                    while (dr.Read())
                    {
                        salary = Convert.ToInt32(dr["Salary"]);
                    }
                }
            }
            return salary;
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

    //public string getTaskThoughput(string toolUsed, string category, string complexity)
    //{
    //    string strQuery = "GetThroughput";
    //    SqlConnection con = new SqlConnection(ConnectionString());
    //    con.Open();
    //    SqlCommand cmd = new SqlCommand(strQuery, con);
    //    cmd.CommandType = CommandType.StoredProcedure;
    //    cmd.Parameters.AddWithValue("@TOOL_NAME", toolUsed.Trim());
    //    cmd.Parameters.AddWithValue("@TASK_NAME", category.Trim());
    //    cmd.Parameters.AddWithValue("@COMPLEXITY", complexity.Trim());

    //    SqlDataReader objRs = cmd.ExecuteReader();
    //    if (objRs.Read())
    //    {
    //        string expectedPages = objRs["EXPECTED_PER_HOUR"].ToString();
    //        string expectedTime = objRs["IN_SECONDS"].ToString();
    //        string bookUnitTime = objRs["BOOK_UNIT_TIME"].ToString();
    //        con.Close();
    //        objRs.Close();
    //        string result = expectedPages + " " + expectedTime + " " + bookUnitTime;
    //        return result;
    //    }
    //    else
    //    {
    //        con.Close();
    //        return "";
    //    }
    //}



    public string InsertTaskExpectedSalary(string userId, string description, string expected, string deposit, string withdraw, string balance)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("InsertTaskExpectedSalary", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());
            cmd.Parameters.AddWithValue("@TaskDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@Description", description.Trim());
            cmd.Parameters.AddWithValue("@Expected", expected.Trim());
            cmd.Parameters.AddWithValue("@Deposit", deposit.Trim());
            cmd.Parameters.AddWithValue("@Withdraw", withdraw.Trim());
            cmd.Parameters.AddWithValue("@Balance", balance.Trim());

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            //return Convert.ToString(result);

            //sqlCon.Open();
            //rows = cmd.ExecuteNonQuery();
            return Convert.ToString(rows);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public string InsertPendingAmount(string userId, double expected, string task, string activityID)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("InsertPendingAmount", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@uid", userId.Trim());
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@expected", expected);
            cmd.Parameters.AddWithValue("@task", task.Trim());
            cmd.Parameters.AddWithValue("@AID", activityID.Trim());

            //var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            //returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            //var result = returnParameter.Value;
            return Convert.ToString(rows);

            //sqlCon.Open();
            //rows = cmd.ExecuteNonQuery();
            //return Convert.ToString(rows);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public List<Transaction> GetWithdrawAmounts()
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetWithdrawAmounts";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = null;
            List<Transaction> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<Transaction>();

                    while (dr.Read())
                    {
                        list.Add(new Transaction
                        {
                            UserId = dr["UserId"] == DBNull.Value ? "" : Convert.ToString(dr["UserId"]),
                            UserName = dr["UserName"] == DBNull.Value ? "" : Convert.ToString(dr["UserName"]),
                            WithdrawAmount = dr["WithdrawAmount"] == DBNull.Value ? "" : Convert.ToString(dr["WithdrawAmount"]),
                            WithdrawDate = dr["WithdrawDate"] == DBNull.Value ? System.DateTime.MinValue : Convert.ToDateTime(dr["WithdrawDate"]),
                            TransactionAmount = dr["TransactionAmount"] == DBNull.Value ? "" : Convert.ToString(dr["TransactionAmount"]),
                            TransactionDate = dr["TransactionDate"] == DBNull.Value ? System.DateTime.MinValue : Convert.ToDateTime(dr["TransactionDate"]),
                            //TransactionType = dr["TransactionType"] == DBNull.Value ? "" : Convert.ToString(dr["TransactionType"]),
                            TransactionType = Convert.ToString(dr["TransactionType"]) == "0" ? "Bank Account" : "Other",
                            TransactionId = dr["TransactionId"] == DBNull.Value ? "" : Convert.ToString(dr["TransactionId"]),
                            Status = dr["Status"] == DBNull.Value ? "" : Convert.ToString(dr["Status"]),
                            RowId = dr["Id"] == DBNull.Value ? "" : Convert.ToString(dr["Id"])
                        });
                    }
                }
            }
            return list;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string ReleaseAmounts(string userId, string rowId, string status, string transactionAmount, string transactionType, string transactionRef)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("ReleaseAmounts", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TransactionBy", userId.Trim());
            cmd.Parameters.AddWithValue("@Status", status.Trim());
            cmd.Parameters.AddWithValue("@WId", rowId.Trim());

            cmd.Parameters.AddWithValue("@TransactionAmount", transactionAmount.Trim());
            cmd.Parameters.AddWithValue("@TransactionType", transactionType);
            cmd.Parameters.AddWithValue("@TransactionRef", transactionRef.Trim());
            cmd.Parameters.AddWithValue("@TransactionDate", DateTime.Now);
            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            con.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;

            return Convert.ToString(result);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string InsertWithdrawAmount(string userId, string withdraw)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("InsertWithdrawAmount", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@uid", userId.Trim());
            cmd.Parameters.AddWithValue("@date", DateTime.Now);
            cmd.Parameters.AddWithValue("@withdraw", withdraw.Trim());

            //var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            //returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            //var result = returnParameter.Value;
            return Convert.ToString(rows);

            //sqlCon.Open();
            //rows = cmd.ExecuteNonQuery();
            //return Convert.ToString(rows);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public string InsertApprovedAmount(string UID, string Deposit, string Withdraw, string Balance, string Description)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("InsertApprovedAmount", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@uid", UID.Trim());
            cmd.Parameters.AddWithValue("@Deposit", Deposit.Trim());
            cmd.Parameters.AddWithValue("@Withdraw", Withdraw.Trim());
            cmd.Parameters.AddWithValue("@Balance", Balance.Trim());
            cmd.Parameters.AddWithValue("@Description", Description.Trim());
            cmd.Parameters.AddWithValue("@date", DateTime.Now);

            //var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            //returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            //var result = returnParameter.Value;
            return Convert.ToString(rows);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public List<string> GetEditor_NamesList()
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetEditorNames";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;

            List<string> list = null;
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            using (con)
            {
                if (dr.HasRows)
                {
                    list = new List<string>();
                    while (dr.Read())
                    {
                        list.Add(Convert.ToString(dr["UserName"]));
                    }
                }
            }

            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public int UpdateTaskStatus(string bookId, string task, double Cost, int uid, string assigDate, string status, string comments)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("Update_TaskStatus", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Cost", Cost);
            cmd.Parameters.AddWithValue("@UID", uid);
            cmd.Parameters.AddWithValue("@AssigmentDate", assigDate);
            cmd.Parameters.AddWithValue("@Status", status.Trim());
            cmd.Parameters.AddWithValue("@Comments", comments.Trim());
            cmd.Parameters.AddWithValue("@bookId", bookId.Trim());
            cmd.Parameters.AddWithValue("@task", task.Trim());
            con.Open();
            rows = cmd.ExecuteNonQuery();
            return rows;
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

    public string UpdateTestStatus(int testid, string status)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("UPDATE_ONLINETESTS_STATUS", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TESTID", testid);
            cmd.Parameters.AddWithValue("@STATUS", status.Trim());
            con.Open();
            rows = cmd.ExecuteNonQuery();
            return Convert.ToString(rows);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public List<TestDetail> GetPendingTests()
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GET_PENDING_ONLINETESTS";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = null;
            List<TestDetail> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<TestDetail>();

                    while (dr.Read())
                    {
                        list.Add(new TestDetail
                        {
                            id = Convert.ToString(dr["ID"]),
                            uid = Convert.ToString(dr["Email"]),
                            testName = Convert.ToString(dr["TEST_NAME"]),
                            testtype = Convert.ToString(dr["TEST_TYPE"]),
                            startTime = Convert.ToString(dr["START_TIME"]),
                            endTiem = Convert.ToString(dr["END_TIME"]),
                            status = Convert.ToString(dr["STATUS"])


                        });
                    }
                }
            }
            return list;
        }
        catch (Exception ex)
        {
            throw ex;
            //return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    //Aamir methods ends here




    ///////////////////////////////////////////////////////////////////////////////
    /// 
    //public string ConnectionString()
    //{
    //    string constring = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
    //    return constring;
    //}

    public string InsertQaMistakes(string BookId, int UserId, int ErrorTypeId, int ErrorNum, DateTime ErrorDate, int ErrorPage, string Comments)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("InsertQaMistakes", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", BookId.Trim());
            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Parameters.AddWithValue("@ErrorTypeId", ErrorTypeId);
            cmd.Parameters.AddWithValue("@ErrorNum", ErrorNum);
            cmd.Parameters.AddWithValue("@ErrorDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@ErrorPage", ErrorPage);
            cmd.Parameters.AddWithValue("@Comments", Comments.Trim());

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            return Convert.ToString(result);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public string InsertQaTask(string bookId, string userId, int totalInjectedMistakes, string status)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("InsertQaTask", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId.Trim());
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@TaskStartDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@TotalInjectedMistakes", totalInjectedMistakes);
            cmd.Parameters.AddWithValue("@status", status);

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            return Convert.ToString(result);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    ////public DataSet GetDataSet(string Query)
    ////{
    ////    SqlConnection con = null;
    ////    try
    ////    {
    ////        con = new SqlConnection(ConnectionString());
    ////        con.Open();
    ////        SqlCommand cmd = new SqlCommand(Query, con);
    ////        SqlDataAdapter da = new SqlDataAdapter(cmd);
    ////        da.SelectCommand.ExecuteNonQuery();
    ////        DataSet ds = new DataSet();
    ////        da.Fill(ds, "temp");
    ////        con.Close();
    ////        return ds;
    ////    }
    ////    finally
    ////    {
    ////        con.Close();
    ////    }
    ////}

    ////public int ExecuteCommand(string Query)
    ////{
    ////    SqlConnection con = null;

    ////    try
    ////    {
    ////        con = new SqlConnection(ConnectionString());
    ////        con.Open();
    ////        SqlCommand cmd = new SqlCommand(Query, con);
    ////        SqlDataAdapter da = new SqlDataAdapter(cmd);
    ////        int result = da.SelectCommand.ExecuteNonQuery();
    ////        return result;
    ////    }
    ////    finally
    ////    {
    ////        con.Close();
    ////    }
    ////}

    public string UpdateMistakeCount(string BookId, int UserId, int detectedInjMistakes, int otherDetectedMistakes)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("UpdateMistakeCount", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", BookId.Trim());
            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Parameters.AddWithValue("@DetectedInjMistakes", detectedInjMistakes);
            cmd.Parameters.AddWithValue("@OtherDetectedMistakes", otherDetectedMistakes);

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            return Convert.ToString(result);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public string CompleteErrorDetectionTask(string BookId, int UserId)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("CompleteErrorDetectionTask", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", BookId.Trim());
            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Parameters.AddWithValue("@Status", "1");
            cmd.Parameters.AddWithValue("@TaskEndDate", DateTime.Now);

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            return Convert.ToString(result);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public int GetXmlComparison_UserId(string bookId, string status)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        string strQuery = "GetXmlComparison_UserId";
        SqlCommand cmd = new SqlCommand(strQuery, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@Status", status.Trim());
        cmd.Parameters.AddWithValue("@BookId", bookId.Trim());

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

    public List<string> GetQaMistake_Comments(string userId, string bookId, int page)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        string strQuery = "GetQaMistakeComments";
        SqlCommand cmd = new SqlCommand(strQuery, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@userId", userId.Trim());
        cmd.Parameters.AddWithValue("@BookId", bookId.Trim());
        cmd.Parameters.AddWithValue("@PageNum", page);

        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        List<string> Comments = null;

        using (con)
        {
            if (dr.HasRows)
            {
                Comments = new List<string>();

                while (dr.Read())
                {
                    Comments.Add(Convert.ToString(dr["QaMistakesId"]) + "," + Convert.ToString(dr["Comments"]));
                }
            }

            if (Comments != null)
                Comments.TrimExcess();
        }

        return Comments;
    }

    public string ShowComment_ByError(string userId, string bookId, int page, int qaMistakeId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        string strQuery = "GetComment_ByError";
        SqlCommand cmd = new SqlCommand(strQuery, con);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@userId", userId.Trim());
        cmd.Parameters.AddWithValue("@BookId", bookId.Trim());
        cmd.Parameters.AddWithValue("@PageNum", page);
        cmd.Parameters.AddWithValue("@QaMistakeId", qaMistakeId);

        con.Open();
        SqlDataReader dr = cmd.ExecuteReader();
        string comment = "";

        using (con)
        {
            if (dr.HasRows)
            {
                while (dr.Read())
                {
                    comment = Convert.ToString(dr["Comments"]);
                }
            }
        }

        return comment.Trim();
    }

    ////public string InsertOnlineTest(string name, string email, string testName, string ipAdress, int totalmarks, string testType)
    ////{
    ////    SqlConnection sqlCon = new SqlConnection(ConnectionString());
    ////    try
    ////    {
    ////        int rows = 0;
    ////        SqlCommand cmd = new SqlCommand("AddOnlineTest", sqlCon);
    ////        cmd.CommandType = CommandType.StoredProcedure;
    ////        cmd.Parameters.AddWithValue("@Name", name.Trim());
    ////        cmd.Parameters.AddWithValue("@Email", email.Trim());
    ////        cmd.Parameters.AddWithValue("@TestName", testName.Trim());
    ////        cmd.Parameters.AddWithValue("@IPAdress", ipAdress.Trim());
    ////        cmd.Parameters.AddWithValue("@totalmarks", totalmarks);
    ////        cmd.Parameters.AddWithValue("@TestType", testType.Trim());
    ////        cmd.Parameters.AddWithValue("@TestStartTime", DateTime.Now);

    ////        var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
    ////        returnParameter.Direction = ParameterDirection.ReturnValue;

    ////        sqlCon.Open();
    ////        rows = cmd.ExecuteNonQuery();
    ////        var result = returnParameter.Value;
    ////        return Convert.ToString(result);
    ////    }
    ////    catch (Exception ex)
    ////    {
    ////        return null;
    ////    }
    ////    finally
    ////    {
    ////        if (sqlCon.State == ConnectionState.Open)
    ////        {
    ////            sqlCon.Close();
    ////        }
    ////    }
    ////}

    public string ClearComparisonTest(string testType, string userId, string status, string testName)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("ClearComparisonTest", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TestType", testType.Trim());
            cmd.Parameters.AddWithValue("@UserId", userId.Trim());
            cmd.Parameters.AddWithValue("@Status", status.Trim());
            cmd.Parameters.AddWithValue("@TestName", testName.Trim());
            cmd.Parameters.AddWithValue("@TestStartTime", DateTime.Now);
            cmd.Parameters.AddWithValue("@TestEndTime", DateTime.Now);

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            return Convert.ToString(result);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    ////public string SaveResult(string name, string email, string testName, string obtainedMarks, string status)
    ////{
    ////    SqlConnection sqlCon = new SqlConnection(ConnectionString());
    ////    try
    ////    {
    ////        int rows = 0;
    ////        SqlCommand cmd = new SqlCommand("SaveOnlineTestResult", sqlCon);
    ////        cmd.CommandType = CommandType.StoredProcedure;
    ////        cmd.Parameters.AddWithValue("@name", name.Trim());
    ////        cmd.Parameters.AddWithValue("@email", email.Trim());
    ////        cmd.Parameters.AddWithValue("@obtainedMarks", obtainedMarks.Trim());
    ////        cmd.Parameters.AddWithValue("@status", status.Trim());
    ////        cmd.Parameters.AddWithValue("@testName", testName.Trim());
    ////        sqlCon.Open();
    ////        rows = cmd.ExecuteNonQuery();
    ////        return Convert.ToString(rows);
    ////    }
    ////    catch
    ////    {
    ////        return null;
    ////    }
    ////    finally
    ////    {
    ////        if (sqlCon.State == ConnectionState.Open)
    ////        {
    ////            sqlCon.Close();
    ////        }
    ////    }
    ////}

    ////public string MovePassedResult(string name, string email)
    ////{
    ////    SqlConnection sqlCon = new SqlConnection(ConnectionString());
    ////    try
    ////    {
    ////        int rows = 0;
    ////        SqlCommand cmd = new SqlCommand("MovePassedTestData", sqlCon);
    ////        cmd.CommandType = CommandType.StoredProcedure;
    ////        cmd.Parameters.AddWithValue("@name", name.Trim());
    ////        cmd.Parameters.AddWithValue("@email", email.Trim());
    ////        sqlCon.Open();
    ////        rows = cmd.ExecuteNonQuery();

    ////        var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
    ////        returnParameter.Direction = ParameterDirection.ReturnValue;

    ////        var result = returnParameter.Value;

    ////        return Convert.ToString(result);


    ////    }
    ////    catch
    ////    {
    ////        return null;
    ////    }
    ////    finally
    ////    {
    ////        if (sqlCon.State == ConnectionState.Open)
    ////        {
    ////            sqlCon.Close();
    ////        }
    ////    }
    ////}

    public static String CheckTaskCompleteness(string bookId, string userId)
    {
        if (bookId == null)
            return null;

        MyDBClass objMyDBClass = new MyDBClass();
        string querySel = "Select TaskStatus from QaComparisonTask Where BookId='" + bookId + "-1' and UserId=" + userId;
        DataSet dsSel = objMyDBClass.GetDataSet(querySel);
        String taskStatus = "";
        string totalPages = "";

        if (dsSel.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsSel.Tables[0].Rows[0];
            taskStatus = Convert.ToString(dr["TaskStatus"]);
            // totalPages = Convert.ToString(dr["totalPages"]);
        }

        return taskStatus + "," + totalPages;
    }
    public List<SpellError> getSpellErrors(string bookId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "select * from SPELL_MISTAKES where BOOK_ID='" + bookId + "'";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();
            List<SpellError> listErrors = new List<SpellError>();
            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SpellError objError = new SpellError();
                        objError.Word = Convert.ToString(dr["WORD"]);
                        objError.PageNo = Convert.ToString(dr["PAGE_NO"]);
                        objError.Occurences = Convert.ToInt32(dr["OCCURENCES"]);
                        listErrors.Add(objError);
                    }
                }
            }
            return listErrors;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
    public SpellError getSpellError(string word, string bookId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "select * from SPELL_MISTAKES where BOOK_ID='" + bookId + "' and WORD='" + word + "'";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        SpellError objError = new SpellError();
                        objError.Word = Convert.ToString(dr["WORD"]);
                        objError.PageNo = Convert.ToString(dr["PAGE_NO"]);
                        objError.Occurences = Convert.ToInt32(dr["OCCURENCES"]);
                        return objError;
                    }
                }
            }
            return null;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }
    public string SaveComparisonTimeSpent(int hour, int minute, string userId)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("SaveComparisonTimeSpent", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@hour", hour);
            cmd.Parameters.AddWithValue("@minute", minute);
            cmd.Parameters.AddWithValue("@userId", userId.Trim());
            cmd.Parameters.AddWithValue("@taskDate", DateTime.Now);
            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            return Convert.ToString(rows);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public string GetComparisonTimeByBookId(string bookId)
    {
        string query = "GetComparisonTimeByBookId";

        try
        {
            MyDBClass objMyDBClass = new MyDBClass();
            SqlConnection con = new SqlConnection(objMyDBClass.ConnectionString_Workmeter());
            SqlCommand cmd = new SqlCommand(query, con);

            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@bookId", bookId);
            cmd.Parameters.AddWithValue("@catId", "18");

            con.Open();

            string CalculatedTime = "";

            using (con)
            {
                SqlDataReader dr = cmd.ExecuteReader();

                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        CalculatedTime = Convert.ToString(dr["CalculatedTime"]);
                    }
                }
            }

            return CalculatedTime;
        }
        catch (Exception ex)
        {
            return "";
        }
    }

    public string InsertExceptionInDb(string ErrorMessage, string ErrorSource, string StackTrace, string TargetSite)
    {
        string bookId = "";

        if (HttpContext.Current.Session != null)
        {
            if (HttpContext.Current.Session.Count > 0)
            {
                bookId = Convert.ToString(HttpContext.Current.Session["BookId"]).Trim();
            }
        }
        else
        {
            bookId = "";
        }

        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("InsertErrorLog", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ErrorMessage", ErrorMessage.Trim());
            cmd.Parameters.AddWithValue("@ErrorSource", ErrorSource.Trim());
            cmd.Parameters.AddWithValue("@StackTrace", StackTrace.Trim());
            cmd.Parameters.AddWithValue("@TargetSite", TargetSite.Trim());
            cmd.Parameters.AddWithValue("@ErrorDate", DateTime.Now);
            //cmd.Parameters.AddWithValue("@MainBookId", HttpContext.Current.Session["BookId"] == null ? "" : Convert.ToString(HttpContext.Current.Session["BookId"]).Trim());
            cmd.Parameters.AddWithValue("@MainBookId", bookId);
            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            return Convert.ToString(result);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public string SetComparisonRights(int userId, string testType, string status, string testName)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            SqlCommand cmd = new SqlCommand("AssignErrorDetectionRights", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@TestType", testType.Trim());
            cmd.Parameters.AddWithValue("@status", status.Trim());
            cmd.Parameters.AddWithValue("@testName", testName.Trim());
            cmd.Parameters.AddWithValue("@start_time", DateTime.Now);
            cmd.Parameters.AddWithValue("@end_Time", DateTime.Now);
            sqlCon.Open();

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            string result = Convert.ToString(returnParameter.Value);

            return Convert.ToString(result);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public int InsertNewBookMicroUser(string loginName, string password, string statusId, string fullName, string email)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString_Workmeter());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("SP_AddBookMicroUser", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@LoginName", loginName.Trim());
            cmd.Parameters.AddWithValue("@Password", password.Trim());
            cmd.Parameters.AddWithValue("@StatusId", statusId.Trim());
            cmd.Parameters.AddWithValue("@FullName", fullName.Trim());
            cmd.Parameters.AddWithValue("@Email", email.Trim());

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();

            return rows;
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public int InsertOperationalFiles(string userId)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("InsertOperationalFiles", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());
            sqlCon.Open();

            SqlParameter returnParameter = cmd.Parameters.Add("RetVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();

            int id = (int)returnParameter.Value;

            return id;
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public int InsertOperationalFiles_StartTest(string email)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("InsertOperationalFiles_StartTest", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@email", email);
            sqlCon.Open();

            SqlParameter returnParameter = cmd.Parameters.Add("RetVal", SqlDbType.VarChar);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();

            int id = (int)returnParameter.Value;

            return id;
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }


    public int CheckOperationalFiles_Updation()
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("CheckOperationalFiles_Updation", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            sqlCon.Open();

            SqlParameter returnParameter = cmd.Parameters.Add("RetVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();

            int id = (int)returnParameter.Value;

            return id;
        }
        catch (Exception ex)
        {
            return 0;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public int GetPageViewedCount(string bookId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetPageViewedCount";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId.Trim());

            con.Open();
            SqlDataReader dr = null;
            int count = 0;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        count = Convert.ToInt32(dr["pageViewedCount"]);
                    }
                }
            }
            return count;
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

    public string InsertPageViewedCount(string bookId, int pageViewedCount)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("InsertPageViewedCount", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId.Trim());
            cmd.Parameters.AddWithValue("@PageViewed", pageViewedCount);

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            return Convert.ToString(result);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public string InsertFinishTaskClickCount(string bookId, int finishTaskCount)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("InsertFinishTaskClickCount", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId.Trim());
            cmd.Parameters.AddWithValue("@FinishTaskCount", finishTaskCount);

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            return Convert.ToString(result);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public string SaveInsertedMistakesCount(string bookId, string userId, int totalMisakes)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("SaveInsertedMistakesCount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId.Trim());
            cmd.Parameters.AddWithValue("@UserId", userId.Trim());
            cmd.Parameters.AddWithValue("@TotalMisakes", totalMisakes);
            con.Open();
            rows = cmd.ExecuteNonQuery();
            return Convert.ToString(rows);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string SaveTaskAmount(string bId, string taskType, string userId, double cost)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("SaveTaskAmount", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BId", bId.Trim());
            cmd.Parameters.AddWithValue("@TaskType", taskType.Trim());
            cmd.Parameters.AddWithValue("@UserId", userId);
            cmd.Parameters.AddWithValue("@Cost", cost);
            con.Open();
            rows = cmd.ExecuteNonQuery();
            return Convert.ToString(rows);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string GetTaskThroughputInSec(string tool, string task, string complexity)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string query = "GetTaskThroughputInSec";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Tool", tool.Trim());
            cmd.Parameters.AddWithValue("@Task", task.Trim());
            cmd.Parameters.AddWithValue("@Complexity", complexity.Trim());

            con.Open();
            SqlDataReader dr = null;
            string onePageTimeInSec = string.Empty;

            using (con)
            {
                cmd.ExecuteNonQuery();
            }
            return onePageTimeInSec;
        }
        catch
        {
            return "";
        }
    }

    public string GetTaskComplexity(string bookId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string query = "GetTaskComplexity";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId.Trim());


            con.Open();
            SqlDataReader dr = null;
            string complexity = string.Empty;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        complexity = Convert.ToString(dr["Complexity"]);
                    }
                }
            }
            return complexity;
        }
        catch
        {
            return "";
        }
    }

    public int GetApprovedTasksCount(string userId, string taskType)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            int approvedTaskCount = 0;
            string strQuery = "GetApprovedTasksCount";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandTimeout = 120;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId.Trim());
            cmd.Parameters.AddWithValue("@TaskType", taskType.Trim());
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        approvedTaskCount = Convert.ToInt32(dr["approvedTaskCount"]);
                    }
                }
            }

            return approvedTaskCount;
        }
        catch (Exception ex)
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

    public int GetMinApprovedTasks(string rank, string taskType, int approvedTasks)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            int minApprovedTask = -1;
            string strQuery = "GetMinApprovedTasks";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandTimeout = 120;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Rank", rank.Trim());
            cmd.Parameters.AddWithValue("@TaskType", taskType.Trim());
            cmd.Parameters.AddWithValue("@ApprovedTasks", approvedTasks);
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        minApprovedTask = Convert.ToInt32(dr["minApprovedTask"]);
                    }
                }
            }

            return minApprovedTask;
        }
        catch (Exception ex)
        {
            return -1;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public List<UserRank> GetUserRanksByTask(string taskType)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetUserRanksByTask";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@TaskType", taskType.Trim());

            con.Open();
            SqlDataReader dr = null;
            List<UserRank> list = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    list = new List<UserRank>();

                    while (dr.Read())
                    {
                        list.Add(new UserRank
                        {
                            TaskType = Convert.ToString(dr["TaskType"]),
                            RankName = Convert.ToString(dr["RankName"]),
                            RankId = Convert.ToString(dr["Id"]),
                            RequiredTasks = Convert.ToInt32(dr["minApprovedTask"])
                        });
                    }
                }
            }
            return list;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public int UpdateUserRank(string userId, int rankId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("UpdateUserRank", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId.Trim());
            cmd.Parameters.AddWithValue("@RankId", rankId);

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            con.Open();
            rows = cmd.ExecuteNonQuery();
            int result = Convert.ToInt32(returnParameter.Value);
            return Convert.ToInt32(result);
        }
        catch
        {
            return -1;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public bool GetUserIsActiveStatus(string userId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string query = "GetUserIsActiveStatus";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId.Trim());

            con.Open();
            SqlDataReader dr = null;
            bool isActive = false;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        isActive = Convert.ToString(dr["IsActive"]) == "1" ? true : false;
                    }
                }
            }
            return isActive;
        }
        catch
        {
            return false;
        }
    }

    public string SetUserIsActiveStatus(string userId)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            SqlCommand cmd = new SqlCommand("SetUserIsActiveStatus", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@userId", userId);
            sqlCon.Open();

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            cmd.ExecuteNonQuery();
            string result = Convert.ToString(returnParameter.Value);

            return Convert.ToString(result);
        }
        catch
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public List<WorkMeterEntities> GetCloudWorkMeterData()
    {
        SqlConnection con = new SqlConnection(ConnectionString_Workmeter());
        try
        {
            string strQuery = "GetCloudWorkMeterData";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;

            con.Open();
            SqlDataReader dr = null;
            List<WorkMeterEntities> recordsList = null;
            List<DateWiseInfo> dailyTimeSpentList = null;
            //List<WorkMeterEntities> finalList = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    recordsList = new List<WorkMeterEntities>();

                    while (dr.Read())
                    {
                        recordsList.Add(new WorkMeterEntities
                        {
                            FullName = Convert.ToString(dr["FullName"]).Trim(),
                            Email = Convert.ToString(dr["Email"]).Trim(),
                            TaskCreationDate = Convert.ToString(dr["TaskStartingDate"]).Trim(),
                            CatId = Convert.ToString(dr["CatId"]).Trim(),
                            BookId = Convert.ToString(dr["BookId"]).Trim(),
                            StartTime = Convert.ToString(dr["StartTime"]).Trim(),
                            EndTime = Convert.ToString(dr["EndTime"]).Trim(),
                            CalculatedTime = Convert.ToString(dr["CalculatedTime"]).Trim(),
                            Current_Status = Convert.ToString(dr["Current_Status"]).Trim(),
                            Comments = Convert.ToString(dr["Comments"]).Trim(),
                            Target = Convert.ToString(dr["Target"]).Trim(),
                            Achived = Convert.ToString(dr["Achived"]).Trim(),
                            Complexity = Convert.ToString(dr["Complexity"]).Trim(),
                            End_Date = Convert.ToString(dr["End_Date"]).Trim(),
                            Expected_Pages = Convert.ToString(dr["Expected_Pages"]).Trim(),
                            Expected_Hours = Convert.ToString(dr["Expected_Hours"]).Trim(),
                            Result = Convert.ToString(dr["Result"]).Trim(),
                            Productivity_Hours = Convert.ToString(dr["Productivity_Hours"]).Trim(),
                            Tool_Used = Convert.ToString(dr["Tool_Used"]).Trim()
                        });
                    }

                    dr.NextResult();

                    if (dr.HasRows)
                    {
                        dailyTimeSpentList = new List<DateWiseInfo>();

                        while (dr.Read())
                        {
                            dailyTimeSpentList.Add(new DateWiseInfo
                            {
                                FullName = Convert.ToString(dr["FullName"]).Trim(),
                                Email = Convert.ToString(dr["Email"]).Trim(),
                                BookId = Convert.ToString(dr["BOOK_ID"]).Trim(),
                                CategoryId = Convert.ToString(dr["CATEGORY_ID"]).Trim(),
                                TaskDate = Convert.ToString(dr["taskDate"]).Trim(),
                                TimeSpent = Convert.ToString(dr["TIME_SPENT"]).Trim()
                            });
                        }
                    }

                    if (recordsList.Count > 0 && dailyTimeSpentList.Count > 0)
                    {
                        foreach (var item in recordsList)
                        {
                            item.DailyTimeSpent = dailyTimeSpentList.Where(x => x.BookId.Equals(item.BookId)).ToList();
                        }
                    }
                }
            }
            return recordsList;
        }
        catch (Exception ex)
        {
            return null;
        }
    }

    public void DeleteTasksInCloudWorkMeter(List<string> completedbooksList)
    {
        if (completedbooksList.Count == 0) return;

        try
        {
            StringBuilder deleteQuery = new StringBuilder();
            deleteQuery.Append("Delete from tblTaskDetails where catid = 27 and current_status='complete' and bookId in (");

            foreach (string bookId in completedbooksList)
            {
                deleteQuery.Append("'" + bookId + "',");
            }

            deleteQuery = deleteQuery.Remove(deleteQuery.Length - 1, 1);

            deleteQuery.Append(")");

            SqlConnection con = new SqlConnection(ConnectionString_Workmeter());
            SqlCommand cmd = new SqlCommand();
            cmd.CommandText = Convert.ToString(deleteQuery);
            cmd.Connection = con;
            con.Open();

            using (con)
            {
                cmd.ExecuteNonQuery();
            }
        }
        catch
        {
            return;
        }
    }

    public BookPageBreak GetBookPageBreak(string bId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetBookPageBreak";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BId", bId);

            con.Open();
            SqlDataReader dr = null;
            BookPageBreak pageDetail = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    pageDetail = new BookPageBreak();

                    while (dr.Read())
                    {
                        pageDetail.BodyStart = Convert.ToInt32(dr["BodyStart"]);
                        pageDetail.BodyEnd = Convert.ToInt32(dr["BodyEnd"]);
                        pageDetail.PageBreak = Convert.ToInt32(dr["PageBreak"]);
                    }
                }
            }
            return pageDetail;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public bool CheckComplexBitsTestStatus(string userId, string taskType)
    {
        bool status = false;
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string testStatus = string.Empty;
            string strQuery = "CheckComplexBitsTestStatus";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandTimeout = 120;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@UserId", userId.Trim());
            cmd.Parameters.AddWithValue("@TaskType", taskType.Trim());
            con.Open();
            SqlDataReader dr = cmd.ExecuteReader();

            using (con)
            {
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        testStatus = Convert.ToString(dr["STATUS"]);
                    }
                }
            }

            if (!string.IsNullOrEmpty(testStatus))
            {
               status = testStatus == "passed" ? true : false;
            }

            return status;
        }
        catch (Exception ex)
        {
            return status;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    public string SaveIndentationInDb(string bookId, string pageType, double xVal, double xIndentVal, double endXVal, double fontSize, string fontName,                                       string paraType, int pageNum)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("SaveIndentationInDb", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId.Trim());
            cmd.Parameters.AddWithValue("@PageType", pageType.Trim());
            cmd.Parameters.AddWithValue("@XVal", xVal);
            cmd.Parameters.AddWithValue("@XIndentVal", xIndentVal);
            cmd.Parameters.AddWithValue("@EndXVal", endXVal);
            cmd.Parameters.AddWithValue("@FontSize", fontSize);
            cmd.Parameters.AddWithValue("@FontName", fontName.Trim());
            cmd.Parameters.AddWithValue("@ParaType", paraType.Trim());
            cmd.Parameters.AddWithValue("@PageNum", pageNum);

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            sqlCon.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            sqlCon.Close();
            return Convert.ToString(result);
        }
        catch (Exception ex)
        {
            return null;
        }
        finally
        {
            if (sqlCon.State == ConnectionState.Open)
            {
                sqlCon.Close();
            }
        }
    }

    public bool GetParaIndentationStatus(string bookId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string query = "GetParaIndentationDetails";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId.Trim());

            con.Open();
            SqlDataReader dr = null;
            bool isActive = false;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        isActive = Convert.ToBoolean(dr["IsParaSelected"]);
                    }
                }
            }
            return isActive;
        }
        catch
        {
            return false;
        }
    }

    public int UpdateParaIndentationStatus(string bookId, bool isParaSelected)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            string query = "UpdateParaIndentationDetails";
            SqlCommand cmd = new SqlCommand(query, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId.Trim());
            cmd.Parameters.AddWithValue("@IsParaSelected", isParaSelected);

            var returnParameter = cmd.Parameters.Add("@ReturnVal", SqlDbType.Int);
            returnParameter.Direction = ParameterDirection.ReturnValue;

            con.Open();
            rows = cmd.ExecuteNonQuery();
            var result = returnParameter.Value;
            con.Close();
            return rows;
        }
        catch
        {
            return 0;
        }
    }

    public List<PdfPara> GetParaSelectedPageNum(string bookId)
    {
        SqlConnection con = new SqlConnection(ConnectionString());
        try
        {
            string strQuery = "GetParaSelectedPage";
            SqlCommand cmd = new SqlCommand(strQuery, con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", bookId.Trim());

            con.Open();
            SqlDataReader dr = null;
            List<PdfPara> paraObjList = null;

            using (con)
            {
                dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    paraObjList = new List<PdfPara>();

                    while (dr.Read())
                    {
                        paraObjList.Add(new PdfPara
                        {
                            PageType = Convert.ToString(dr["PageType"]),
                            ParaType = Convert.ToString(dr["ParaType"])
                        });
                    }
                }
            }
            return paraObjList;
        }
        catch
        {
            return null;
        }
        finally
        {
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
        }
    }

    //public bool GetPageBreakStatus(string bookId)
    //{
    //    SqlConnection con = new SqlConnection(ConnectionString());
    //    try
    //    {
    //        string query = "CheckBookPageBreak";
    //        SqlCommand cmd = new SqlCommand(query, con);
    //        cmd.CommandType = CommandType.StoredProcedure;
    //        cmd.Parameters.AddWithValue("@BookId", bookId.Trim());

    //        con.Open();
    //        SqlDataReader dr = null;
    //        bool isActive = false;

    //        using (con)
    //        {
    //            dr = cmd.ExecuteReader();
    //            if (dr.HasRows)
    //            {
    //                while (dr.Read())
    //                {
    //                    isActive = Convert.ToBoolean(dr["IsParaSelected"]);
    //                }
    //            }
    //        }
    //        return isActive;
    //    }
    //    catch
    //    {
    //        return false;
    //    }
    //}
}

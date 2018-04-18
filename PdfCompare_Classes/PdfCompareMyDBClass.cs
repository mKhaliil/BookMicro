using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;


public class PdfCompareMyDBClass
{
    public PdfCompareMyDBClass()
    {

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

    public string InsertQaTask(string BookId, int UserId, int Mistakes, string status)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("InsertQaTask", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", BookId.Trim());
            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Parameters.AddWithValue("@TaskDate", DateTime.Now);
            cmd.Parameters.AddWithValue("@Mistakes", Mistakes);
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

    public string UpdateMistakeCount(string BookId, int UserId, int Mistakes)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("UpdateMistakeCount", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", BookId.Trim());
            cmd.Parameters.AddWithValue("@UserId", UserId);
            cmd.Parameters.AddWithValue("@Mistakes", Mistakes);

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

    public string CompleteMistakeInj_Task(string BookId, int UserId)
    {
        SqlConnection sqlCon = new SqlConnection(ConnectionString());
        try
        {
            int rows = 0;
            SqlCommand cmd = new SqlCommand("CompleteMistakeInjection_Task", sqlCon);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@BookId", BookId.Trim());
            cmd.Parameters.AddWithValue("@UserId", UserId);

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

    public static String CheckTaskCompleteness(string bookId, string userId)
    {
        if (bookId == null)
            return null;

        MyDBClass objMyDBClass = new MyDBClass();
        string querySel = "Select TaskStatus, totalPages from QaComparisonTask Where BookId='" + bookId + "-1' and UserId=" + userId;
        DataSet dsSel = objMyDBClass.GetDataSet(querySel);
        String taskStatus = "";
        string totalPages = "";

        if (dsSel.Tables[0].Rows.Count > 0)
        {
            DataRow dr = dsSel.Tables[0].Rows[0];
            taskStatus = Convert.ToString(dr["TaskStatus"]);
            totalPages = Convert.ToString(dr["totalPages"]);
        }

        return taskStatus + "," + totalPages;
    }

    public string getTaskId(string bookId)
    {
        try
        {
            SqlConnection con = new SqlConnection(ConnectionString_Workmeter());
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
        try
        {
            SqlConnection objconection = new SqlConnection(ConnectionString_Workmeter());
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
        catch (Exception)
        {
            return "";
        }
    }

    private void TaskLogManuplation(int taskid, string taskLog, int countLog, int userID, DateTime date,
        string action)
    {
        SqlConnection objconection = new SqlConnection(ConnectionString_Workmeter());
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
}

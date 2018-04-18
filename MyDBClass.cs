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
using Outsourcing_System;

public class MyDBClass
{
    public MyDBClass()
    {

    }
    
    public string MainDirectory = "";

    public  string MainDirPhyPath
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

    public  string GetID(string Query)
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

    public  void LogEntry(string MainBook, string Process, string detail, string Status, string action)
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
    public  void UpdateMailStatus(string MID, string UserName)
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

    public  int CreateTask(string BID, string Status, string Task, string Admin)
    {
        string queryUnnowID = "Select * from [User] where UserName='unknown'";
        string UnKnownID = GetID(queryUnnowID);
        string queryInsert = "Insert into ACTIVITY(UID,BID,AssignedBy,Status,Task) VALUES(" + UnKnownID + "," + BID + ",'" + Admin + "','" + Status + "','" + Task + "')";
        int inResult = ExecuteCommand(queryInsert);
        return inResult;
    }
}

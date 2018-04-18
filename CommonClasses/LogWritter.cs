using System;
using System.Collections.Generic;
using System.IO;
using System.Web;

/// <summary>
/// Summary description for LogWritter
/// </summary>

public class LogWritter
{
    public LogWritter()
    {
        //
        // TODO: Add constructor logic here
        //
    }

    public static bool WriteLineInLog(string content)
    {
        StreamWriter SW = null;
        try
        {
            object objLogFilePath = System.Configuration.ConfigurationSettings.AppSettings["LogFilePath"];
            string logFilePath = "";
            if (objLogFilePath == null)
            {
                logFilePath = HttpContext.Current.Server.MapPath(".") + "\\AutoMappService_Log.txt";
            }
            else
            {
                if (logFilePath == "")
                    logFilePath = HttpContext.Current.Server.MapPath(".") + "\\AutoMappService_Log.txt";
                else
                    logFilePath = objLogFilePath.ToString();
            }

            SW = File.AppendText(logFilePath);
            SW.WriteLine(content);
            return true;

        }
        catch
        {
            return false;
        }
        finally
        {
            if (SW != null)
            {
                SW.Close();
            }
        }
    }
}

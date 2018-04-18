using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Remoting.Contexts;
using System.Web.Configuration;
using System.IO;

namespace Outsourcing_System
{
    /// <summary>
    /// Summary description for DisplayPdf
    /// </summary>
    public class DisplayPdf : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        MyDBClass objMyDBClass = new MyDBClass();

        public void ProcessRequest(HttpContext context)
        {
            ////int testName = Convert.ToInt32(context.Request.QueryString["TestName"]);
            ////string testName = Convert.ToString(context.Request.QueryString["TestName"]).Split('/')[0];
            //string testPath = Convert.ToString(context.Request.QueryString["TestName"]);
            //int pageNum = Convert.ToInt32(context.Request.QueryString["page"]);

            //byte[] buffer;
            ////using (System.IO.FileStream fileStream = new System.IO.FileStream(objMyDBClass.MainDirPhyPath + "\\" + testPath + "/Page" + pageNum + ".pdf", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            //using (System.IO.FileStream fileStream = new System.IO.FileStream(objMyDBClass.MainDirPhyPath + "\\" + testPath + "/Page" + pageNum + ".pdf", System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            //using (System.IO.BinaryReader reader = new System.IO.BinaryReader(fileStream))
            //{
            //    buffer = reader.ReadBytes((int)reader.BaseStream.Length);
            //}
            //context.Response.ContentType = "application/pdf";
            //context.Response.AddHeader("Content-Length", buffer.Length.ToString());
            //context.Response.AppendHeader("content-disposition", "inline");
            //context.Response.BinaryWrite(buffer);
            //context.Response.End();


            string path = "";
            string bid = Convert.ToString(context.Request.QueryString["bid"]);
            int pageNum = Convert.ToInt32(context.Request.QueryString["page"]);
            string taskType = Convert.ToString(context.Request.QueryString["type"]);

            if (taskType != null)
            {
                if (taskType.Equals("sc"))
                {
                    path = WebConfigurationManager.AppSettings["MainDirPhyPath"] + "/" + bid + "/SpellCheck/Page" +
                           pageNum + "_Highlighted.pdf";
                }
                else if (taskType.Equals("mi"))
                {
                    path = WebConfigurationManager.AppSettings["MainDirPhyPath"] + bid + "/" + bid +
                           "-1/TaggingUntagged/Page" + pageNum + ".pdf";
                }
            }
            else
            {
                path = WebConfigurationManager.AppSettings["MainDirPhyPath"] + bid.Replace("-1", "") + "/" + bid +
                       "/TaggingUntagged/Page" + pageNum + ".pdf";
            }

            if (File.Exists(path))
            {
                byte[] buffer;
                using (
                    System.IO.FileStream fileStream = new System.IO.FileStream(path, System.IO.FileMode.Open,
                        System.IO.FileAccess.Read, System.IO.FileShare.Read))
                using (System.IO.BinaryReader reader = new System.IO.BinaryReader(fileStream))
                {
                    buffer = reader.ReadBytes((int) reader.BaseStream.Length);
                }
                context.Response.ContentType = "application/pdf";
                context.Response.AddHeader("Content-Length", buffer.Length.ToString());
                context.Response.AppendHeader("content-disposition", "inline");
                context.Response.BinaryWrite(buffer);
                context.Response.End();
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
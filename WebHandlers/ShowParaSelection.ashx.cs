using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Outsourcing_System.WebHandlers
{
    /// <summary>
    /// Summary description for ShowParaSelection
    /// </summary>
    public class ShowParaSelection : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {

            string pdfPath = Convert.ToString(HttpContext.Current.Session["paraSelectionPdfPath"]);

            if (File.Exists(pdfPath))
            {
                byte[] buffer;
                using (FileStream fileStream = new FileStream(pdfPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (BinaryReader reader = new BinaryReader(fileStream))
                    {
                        buffer = reader.ReadBytes((int)reader.BaseStream.Length);
                    }
                }
                context.Response.ContentType = "application/pdf";
                context.Response.AddHeader("Content-Length", buffer.Length.ToString());
                context.Response.AppendHeader("content-disposition", "inline");
                context.Response.BinaryWrite(buffer);
                context.Response.End();
            }
            else
            {
                return;
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
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Outsourcing_System.WebHandlers
{
    /// <summary>
    /// Summary description for GetPdfInSVG
    /// </summary>
    public class GetPdfInSVG : IHttpHandler, System.Web.SessionState.IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["PdfForSVGPath"])))
            {
                string pdfPath = Convert.ToString(HttpContext.Current.Session["PdfForSVGPath"]);

                if (File.Exists(pdfPath))
                {
                    byte[] buffer;
                    using (FileStream fileStream = new FileStream(pdfPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        using (BinaryReader reader = new BinaryReader(fileStream))
                        {
                            buffer = reader.ReadBytes((int) reader.BaseStream.Length);
                        }
                    }
                    context.Response.ContentType = "application/pdf";
                    context.Response.AddHeader("Content-Length", Convert.ToString(buffer.Length));
                    context.Response.AppendHeader("content-disposition", "inline");
                    context.Response.BinaryWrite(buffer);
                    context.Response.End();
                }
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
<%@ WebHandler Language="C#" Class="showPdf" %>

using System;
using System.IO;
using System.Net;
using System.Web;

public class showPdf : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    public void ProcessRequest(HttpContext context)
    {
        int currentPage = 0;
        int subPrdPdfSubPage = 0;
        string pdfType = Convert.ToString(context.Request.QueryString["pdfType"]);
        string type = Convert.ToString(context.Request.QueryString["type"]);
        string imgFilePath = string.Empty;

        string path = "";
        string producePdfType = string.Empty;
        
        if (type == null)
        {
            currentPage = Convert.ToInt32(HttpContext.Current.Session["Handler_Page"]);
            pdfType = Convert.ToString(HttpContext.Current.Session["Handler_pdfType"]);
            type = Convert.ToString(HttpContext.Current.Session["Handler_type"]);

            producePdfType = Convert.ToString(HttpContext.Current.Session["ProducePdfType"]);
            subPrdPdfSubPage = Convert.ToInt32(HttpContext.Current.Session["Handler_PrdPdfSubPage"]);
        }

        if (type.Equals("pdf") && (producePdfType.Equals("Pdf") || string.IsNullOrEmpty(producePdfType)))
        {
            currentPage = Convert.ToInt32(HttpContext.Current.Session["Handler_Page"]);
            path = Common.GetPdfPath(currentPage, "prd");

            if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["UndoHighlighting"])))
            {
                if (Convert.ToString(HttpContext.Current.Session["UndoHighlighting"]).Equals("true"))
                {
                    path = path.Replace("_Annotated", "").Replace("_Stamped", "");
                }
            }
        }
        else if (type.Equals("subPdf") || producePdfType.Equals("SubPdf"))
        {
            currentPage = Convert.ToInt32(HttpContext.Current.Session["Handler_Page"]);
            path = Common.GetPdfPath(currentPage, "prd");
            
            
            string outPutPdfPath = Path.GetDirectoryName(path);

            if (path.Contains("_Stamped"))
                path = outPutPdfPath + "\\Produced_" + currentPage + "_" + subPrdPdfSubPage + "_Stamped.pdf";
            else 
                path = outPutPdfPath + "\\Produced_" + currentPage + "_" + subPrdPdfSubPage + ".pdf";
        }

        else if (type.Equals("img"))
        {
            currentPage = Convert.ToInt32(context.Request.QueryString["Page"]);

            imgFilePath = Common.GetPdfPath(currentPage, "src").Replace(".pdf", ".pdf.jpeg");

            if (!string.IsNullOrEmpty(Convert.ToString(HttpContext.Current.Session["UndoHighlighting"])))
            {
                if (Convert.ToString(HttpContext.Current.Session["UndoHighlighting"]).Equals("true"))
                {
                    imgFilePath = imgFilePath.Replace("_Annotated", "").Replace("_Stamped", "");
                }
            }
            
            if (File.Exists(imgFilePath)) 
                path = imgFilePath;

            //if (File.Exists(Common.GetPdfPath(currentPage, "src").Replace(".pdf", "_Stamped.pdf.jpeg")))
            //{
            //    path = Common.GetPdfPath(currentPage, "src").Replace(".pdf", "_Stamped.pdf.jpeg");
            //}
            //else
            //{
            //    path = Common.GetPdfPath(currentPage, "src").Replace(".pdf", ".pdf.jpeg");
            //}
        }

        if (File.Exists(path))
        {
            byte[] buffer;
            using (System.IO.FileStream fileStream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
            {
                using (System.IO.BinaryReader reader = new System.IO.BinaryReader(fileStream))
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

//Works With old Comparison.aspx viewer

//public class showPdf : IHttpHandler, System.Web.SessionState.IRequiresSessionState
//{
//    public void ProcessRequest(HttpContext context)
//    {
//        int currentPage = Convert.ToInt32(context.Request.QueryString["Page"]);
//        string pdfType = Convert.ToString(context.Request.QueryString["pdfType"]);
//        string type = Convert.ToString(context.Request.QueryString["type"]);

//        string path = "";

//        if ((currentPage < 1) || (pdfType == ""))
//            return;

//        //string srcPdfPath = Common.GetPdfPath(currentPage, "src");
//        //string prdPdfPath = Common.GetPdfPath(currentPage, "prd");

//        if (type.Equals("pdf"))
//        {
//            path = Common.GetPdfPath(currentPage, "prd"); 
//        }

//        else if (type.Equals("img"))
//        {
//            if (File.Exists(Common.GetPdfPath(currentPage, "src").Replace(".pdf", "_Stamped.pdf.jpeg")))
//            {
//                path = Common.GetPdfPath(currentPage, "src").Replace(".pdf", "_Stamped.pdf.jpeg");
//            }
//            else
//            {
//                path = Common.GetPdfPath(currentPage, "src").Replace(".pdf", ".pdf.jpeg");
//            }
//        }

//        if (File.Exists(path))
//        {
//            byte[] buffer;
//            using (System.IO.FileStream fileStream = new System.IO.FileStream(path, System.IO.FileMode.Open, System.IO.FileAccess.Read, System.IO.FileShare.Read))
//            {
//                using (System.IO.BinaryReader reader = new System.IO.BinaryReader(fileStream))
//                {
//                    buffer = reader.ReadBytes((int)reader.BaseStream.Length);
//                }
//            }
//            context.Response.ContentType = "application/pdf";
//            context.Response.AddHeader("Content-Length", buffer.Length.ToString());
//            context.Response.AppendHeader("content-disposition", "inline");
//            context.Response.BinaryWrite(buffer);
//            context.Response.End();
//        }
//        else
//        {
//            return;
//        }
//    }

//    public bool IsReusable
//    {
//        get
//        {
//            return false;
//        }
//    }

//}
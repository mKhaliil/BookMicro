using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
//using WebSupergoo.ABCocr3;
using System.Text;

namespace Outsourcing_System
{
    public partial class OCRDemonstration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnDownload_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            string output = "Output";
            sb.Append(output);
            sb.Append("\r\n");
            sb.Append(txtContent.Text);
            string text = sb.ToString();

            Response.Clear();
            Response.ClearHeaders();

            Response.AddHeader("Content-Length", text.Length.ToString());
            Response.ContentType = "text/plain";
            Response.AppendHeader("content-disposition", "attachment;filename=\"output.txt\"");

            Response.Write(text);
            Response.End();
        }

        //protected void btnOCR_Click(object sender, EventArgs e)
        //{
        //    if (FileUpload1.HasFile)
        //    {
        //        FileUpload1.SaveAs(@"D:\Files\OCR\" + FileUpload1.FileName);
        //        img.ImageUrl =Server.MapPath("/Files/OCR/" + FileUpload1.FileName);


        //        txtContent.Text = performOCR(@"D:\Files\OCR\" + FileUpload1.FileName);
        //    }
        //}

        //private string performOCR(string FilePath)
        //{
        //    string extension = Path.GetExtension(FilePath);
        //    System.Text.StringBuilder stBuilder = new System.Text.StringBuilder();
        //    OCROperations objOcrOperations = new OCROperations();
        //    System.Drawing.Bitmap bmap = null;
        //    Ocr objocr = new Ocr();
        //    bmap = new System.Drawing.Bitmap(FilePath);
        //    objocr.SetBitmap(bmap);

        //    for (int i = 0; i < objocr.Page.Words.Count; i++)
        //    {
        //        stBuilder.Append(objocr.Page.Words[i].Text + " ");
        //    }

        //    stBuilder = stBuilder.Replace("|<", "k");
        //    File.WriteAllText(FilePath.Replace(extension, ".txt"), stBuilder.ToString());
        //    return stBuilder.ToString();
        //}
    }
}

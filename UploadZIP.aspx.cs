using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Xml.Linq;
using System.IO;
using BookMicroBeta;

namespace Outsourcing_System
{
    public partial class UploadZIP : System.Web.UI.Page
    {
        MyDBClass objMyDBClass = new MyDBClass();
        protected void Page_Load(object sender, EventArgs e)
        {
            this.lblMessage.Text = "";
            this.Title = "Outsourcing System :: Upload New Task";
            if (Session["objUser"] == null)
            {
                Response.Redirect("BookMicro.aspx");
            }     
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            if (File1.PostedFile.FileName != "" && File2.PostedFile.FileName!="")
            {
                try
                {
                    string ZipInputPath = Server.MapPath("~/Files/temp" + (Session["objUser"] as UserClass).UserName);
                    string fileName = ZipInputPath + "\\temp" + (Session["objUser"] as UserClass).UserName + ".zip";
                    string csvFileName = ZipInputPath + "\\temp" + (Session["objUser"] as UserClass).UserName + ".csv";
                    if (!Directory.Exists(ZipInputPath))
                    {
                        Directory.CreateDirectory(ZipInputPath);
                    }
                    File1.PostedFile.SaveAs(fileName);
                    File2.PostedFile.SaveAs(csvFileName);
                    if (File.Exists(fileName))
                    {
                        try
                        {
                            ZipUtility zip = new ZipUtility();
                            zip.setunzipPath(ZipInputPath);
                            zip.ExtractZipFile(fileName);
                        }
                        catch
                        {}
                        finally
                        {
                            if (File.Exists(fileName))
                            {
                                File.Delete(fileName);
                                DirectoryInfo info = new DirectoryInfo(ZipInputPath);
                                FileInfo[] files = info.GetFiles("*.pdf");

                                if (files.Length > 0)
                                {
                                    TextReader tr = new StreamReader(csvFileName);
                                    string csvLine = tr.ReadLine();
                                    while (csvLine != null)
                                    {                                        
                                        string bookID = csvLine.Split(new char[] { ',' })[0].Trim();
                                        string PDFName = csvLine.Split(new char[] { ',' })[1].Trim();
                                        string bookTitle = csvLine.Split(new char[] { ',' })[2].Trim();

                                        string subDirName = Server.MapPath("~/Files/") + bookID;
                                        string pdfFileName = subDirName + "\\" + bookID+".pdf";
                                        if (!Directory.Exists(subDirName))
                                        {
                                            Directory.CreateDirectory(subDirName);
                                        }
                                        File.Copy(ZipInputPath + "\\" + PDFName, pdfFileName);
                                        //++++++++++++++++++++++++++++++++++++++++++++++++++++
                                        string existRecQuery = "SELECT * FROM MainBook Where MainBook='" + bookID + "'";
                                        DataSet ds = objMyDBClass.GetDataSet(existRecQuery);
                                        if (ds.Tables[0].Rows.Count > 0)
                                        {
                                            this.lblMessage.Text = "Book with same ID already exist";
                                        }
                                        else
                                        {
                                            string insertRecQuery = "Insert Into MainBook Values('" + bookID + "','" + bookTitle + "','incomplete','" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "')";
                                            int insRes = objMyDBClass.ExecuteCommand(insertRecQuery);
                                            if (insRes > 0)
                                            {
                                                //autoMapSvc.AutoMappMethodAsync((Session["objUser"] as UserClass).UserName, bookTitle, pdfFileName);
                                                Outsourcing_System.AutoMapService.AutoMappService autoMapSvc = new Outsourcing_System.AutoMapService.AutoMappService();
                                                autoMapSvc.AllowAutoRedirect = true;
                                                autoMapSvc.CreateTaggingUntaggingAsync((Session["objUser"] as UserClass).UserName, bookTitle, pdfFileName);
                                                System.Threading.Thread.Sleep(1000 * 60);
                                                autoMapSvc.CancelAsync(null);
                                                autoMapSvc.Dispose();
                                            }
                                        }
                                        //++++++++++++++++++++++++++++++++++++++++++++++++++++
                                        csvLine = tr.ReadLine();
                                    }
                                    tr.Close();
                                    Directory.Delete(ZipInputPath, true);
                                }
                            }
                        }
                    }
                }
                catch(Exception ex)
                {
                    this.lblMessage.Text = ex.Message;
                }
            }
        }
    }
}

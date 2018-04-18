using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Outsourcing_System;
using System.IO;
using BookMicroBeta;

public partial class CompleteBooks : System.Web.UI.Page
{
    MyDBClass objMyDBClass = new MyDBClass();
    protected void Page_Load(object sender, EventArgs e)
    {        
        this.Title = "Outsourcing System :: Completed Books";
        this.lblMessage.Text = "";
        if (Session["objUser"] == null || (Session["objUser"] as UserClass).UserType != "admin")
        {
            Response.Redirect("BookMicro.aspx");
        }        
        if (Request.QueryString["act"] != null)
        {         
            
            Outsourcing_System.AutoMapService.AutoMappService autoMapSvc1 = new Outsourcing_System.AutoMapService.AutoMappService();
            try
            {
                string mainBook = Request.QueryString["book"].ToString();
                string filePath = "Files/" + mainBook + "/" + mainBook + ".pdf";

                if (Request.QueryString["act"].ToString() == "merge")
                {
                    autoMapSvc1.MergMethod(Server.MapPath(filePath));
                    ShowDataInGridView(mainBook);
                }
                else if (Request.QueryString["act"].ToString() == "finalize")
                {
                    filePath = filePath.Replace(".pdf", ".rhyw");
                    autoMapSvc1.FinalizeBook(Server.MapPath(filePath));
                    ShowDataInGridView(mainBook);
                }
                else if (Request.QueryString["act"].ToString() == "zip")
                {
                    try
                    {
                        ZipUtility zip = new ZipUtility();
                        string zipFile = "Files/" + mainBook + "/" + mainBook + ".zip";
                        if (File.Exists(Server.MapPath("~/" + zipFile)))
                        {
                            File.Delete(zipFile);
                        }
                        zip.setdirToZip(Server.MapPath("~/Files/" + mainBook));
                        zip.ZIPDirectory(Server.MapPath("~/Files"));
                        ShowDataInGridView(mainBook);
                    }
                    catch (Exception ex)
                    {
                        this.lblMessage.Text = ex.Message ;
                    }
                }
            }
            finally
            {
                autoMapSvc1.Dispose();
            }
        }
        //if (!Page.IsPostBack)
        //{
        //    ShowDataInGridView("");
        //}
       
    }
    public string InnerContent(string mainBook, string status,string type)
    {
        if (status == "incomplete")
        {
            FinalizingBook(mainBook);
        }
        string lastCol = "<a href='LogStatus.aspx?book=" + mainBook + "&act=view'>View Log</a> ";
        string table = "<table width='100%' class='bbw'>";
        table += "<tr><td class='bbw' align='left' width='60%'>";

        string rhywFile = "Files/" + mainBook + "/" + mainBook + ".rhyw";
        string finalizedFile = "Files/" + mainBook + "/" + mainBook + "-Final.rhyw";
        string pdfFile = "Files/" + mainBook + "/" + mainBook + ".pdf";
        string zipFile = "Files/"  + mainBook + ".zip";

        if (!File.Exists(Server.MapPath("~/" + pdfFile)))
        {
            table += "PDF not found :: ";
        }
        else
        {
            //table += "<a href='" + pdfFile + "'>PDF</a> ";
        }
        if (!File.Exists(Server.MapPath("~/" + rhywFile)))
        {
            table += "Not Merged :: ";
            lastCol += "<a href=CompleteBooks.aspx?book=" + mainBook + "&act=merge>Merge</a> ";
        }
        else
        {
            //table += "<a href='" + rhywFile + "'>RHYW</a> ";
        }
        if (!File.Exists(Server.MapPath("~/" + finalizedFile)))
        {
            table += "Not Finalized :: ";
            lastCol += "<a href=CompleteBooks.aspx?book=" + mainBook + "&act=finalize>Finalize</a> ";
        }
        else
        {
            //table += "<a href='" + finalizedFile + "'>Finalized</a> ";
        }

        for (int j = 1; j < 4; j++)
        {
            string imageZip = "Files/" + mainBook + "/" + mainBook + "-" + j + "/Image/" + mainBook  +"-" + j + ".zip";
            if (File.Exists(Server.MapPath("~/" + imageZip)))
            {
                //table += "<a href='" + imageZip + "'>Images-"+j+"</a> ";
            }
        }
        for (int j = 1; j < 4; j++)
        {
            string tableZip = "Files/" + mainBook + "/" + mainBook + "-" + j + "/Table/" + mainBook + "-" + j + ".zip";
            if (File.Exists(Server.MapPath("~/" + tableZip)))
            {
                //table += "<a href='" + tableZip + "'>Tables-" + j + "</a> ";
            }
        }
        if (!File.Exists(Server.MapPath("~/" + zipFile)))
        {
            table += "Not Zipped :: ";
            lastCol += " <a href=CompleteBooks.aspx?book=" + mainBook + "&act=zip>Zip</a> ";
        }
        else
        {
            table += "<a href='" + zipFile + "'>Complete Zip</a> ";
        }

        table += "</td></tr></table>";
        if (type == "last")
        {
            table = lastCol;
        }
        return table;
    }

    protected void FinalizingBook(string mainBook)
    {
        if (mainBook != "")
        {
            string queryCheckMainBooks = "Select * from Book Where MainBook='" + mainBook + "' AND UploadedBy='admin'";
            DataSet dsMainProcessed = objMyDBClass.GetDataSet(queryCheckMainBooks);
            bool isAllMainProcessed = true;
            foreach (DataRow dr in dsMainProcessed.Tables[0].Rows)
            {
                if (dr["BStatus"].ToString().ToLower() != "complete")
                {
                    isAllMainProcessed = false;
                    break;
                }
            }
            if (isAllMainProcessed == true && dsMainProcessed.Tables[0].Rows.Count>0)
            {
                Outsourcing_System.AutoMapService.AutoMappService autoMapSvc = new Outsourcing_System.AutoMapService.AutoMappService();
                Outsourcing_System.Index_Service.IndexService indexSvc = new Outsourcing_System.Index_Service.IndexService();

                try
                {
                    string mainFile = Server.MapPath("~/Files/" + mainBook + "/" + mainBook);
                    string status = autoMapSvc.MergMethod(mainFile + ".pdf");

                    if (status == "Successfull")
                    {
                        status = autoMapSvc.InsertVolumeBreaks(mainFile + ".rhyw");
                        string xlsFile = Server.MapPath("~/Files/" + mainBook + "/" + mainBook + "-3/Index/" + mainBook + "-3.xls");

                        if (File.Exists(xlsFile) && status == "Successfull")
                        {
                            status = indexSvc.AttachRHYWIndex(mainFile + ".rhyw", xlsFile);
                        }
                        if (status == "Successfull")
                        {
                            //Upate the Main Book Table
                            string queryUpdateMainBook = "Update MainBook Set Status='Complete' Where MainBook='" + mainBook + "'";
                            int resBook = objMyDBClass.ExecuteCommand(queryUpdateMainBook);
                            if (resBook > 0)
                            {
                                status = autoMapSvc.FinalizeBook(mainFile + ".rhyw");
                            }
                        }
                    }
                }
                finally
                {
                    autoMapSvc.Dispose();
                    indexSvc.Dispose();
                }
            }
        }
    }
    public void ShowDataInGridView(string BookID)
    {
        string query = "Select MainBook,Status from MainBook ";
        if (BookID.Trim() != "")
        {
            query += "Where MainBook='" + BookID + "' ";
        }
        DataSet ds = objMyDBClass.GetDataSet(query);
        this.GridView1.DataSource = ds.Tables[0];
        this.GridView1.DataBind();
        if (ds.Tables[0].Rows.Count <= 0)
        {
            this.Master.ShowMessageBox("Sorry! No Record Found","Error");
        }
    }
    protected void btnShow_Click(object sender, EventArgs e)
    {
       ShowDataInGridView(txtBookID.Text.Trim());
    }
    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex; 
    }
   
}

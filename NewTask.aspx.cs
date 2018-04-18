using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using BookMicroBeta;
//using WebSupergoo.ABCocr3;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Outsourcing_System.CommonClasses;
using Outsourcing_System.MasterPages;
using Outsourcing_System;
using Outsourcing_System.PdfCompare_Classes;
using Outsourcing_System.ServiceReference1;

public partial class NewTask : System.Web.UI.Page
{
    MyDBClass objMyDBClass = new MyDBClass();
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.Title = "Outsourcing System :: Upload New Task";
        //if (Session["objUser"] == null || (Session["objUser"] as UserClass).UserType != "admin")
        //{
        //    Response.Redirect("Login.aspx");
        //}

        if (!IsPostBack)
        {
            //((AdminMaster)this.Page.Master).ShowLogOutButton();
            //((AdminMaster)this.Page.Master).SetMenuLocation = "-20px";
        }
    }
    protected void btnBatch_Click(object sender, EventArgs e)
    {
        Response.Redirect("UploadZIP.aspx");
    }
    //private void MissingFontsValidating()
    //{
    //    XmlDocument xmldoc = new XmlDocument();
    //    StreamReader sr = new StreamReader("D:\\Files\\194\\194-1\\TaggingUntagged\\194-1.xml");
    //    xmldoc.LoadXml(sr.ReadToEnd());
    //    XmlNodeList uparas = xmldoc.SelectNodes("//upara");
    //    string directory = Path.GetDirectoryName("D:\\Files\\194\\194.pdf");
    //    CommonClass objCommon = new CommonClass();

    //    foreach (XmlNode upara in uparas)
    //    {

    //        if (upara.OuterXml.Contains("NotEmbeded"))
    //        {
    //            string NotEmbededDirectory = "D:\\Files\\194\\NotEmbededPages";
    //            if (!Directory.Exists(NotEmbededDirectory))
    //            {
    //                Directory.CreateDirectory(NotEmbededDirectory);
    //            }
    //            string pageno = upara.ChildNodes[0].Attributes["page"].Value;
    //            if (!File.Exists(NotEmbededDirectory + "\\Page-1.txt"))
    //            {

    //                System.Drawing.Bitmap bmap = new System.Drawing.Bitmap(NotEmbededDirectory + "\\Page-1.tiff");
    //                Ocr objocr = new Ocr();
    //                objocr.SetBitmap(bmap);
    //                System.Text.StringBuilder stBuilder = new System.Text.StringBuilder();
    //                int yCoord = 0;

    //                for (int i = 0; i < objocr.Page.Words.Count; i++)
    //                {
    //                    if (yCoord == 0)
    //                    {
    //                        yCoord = objocr.Page.Words[i].Bounds.Bottom;
    //                    }
    //                    if (objocr.Page.Words[i].Bounds.Bottom == yCoord || (objocr.Page.Words[i].Bounds.Top <= yCoord))
    //                    {
    //                        stBuilder.Append(objocr.Page.Words[i].Text + " ");
    //                    }
    //                    else
    //                    {
    //                        yCoord = objocr.Page.Words[i].Bounds.Bottom;

    //                        stBuilder.Append(" </br>" + objocr.Page.Words[i].Text);
    //                    }
    //                }

    //                stBuilder = stBuilder.Replace("|<", "k");
    //                File.WriteAllText(NotEmbededDirectory + "\\Page-1.txt", stBuilder.ToString());
    //                stBuilder = stBuilder.Replace("-", "");
    //                XmlNodeList nodeLines = xmldoc.SelectNodes("//ln[@page='1']");

    //                for (int i = 0; i < nodeLines.Count; i++)
    //                {
    //                    if (i != 0)
    //                    {
    //                        XmlNode currentNode = nodeLines[i];
    //                        XmlNode prevLine = currentNode;
    //                        XmlNode nextLine = currentNode;
    //                        XmlNode MainNode = nodeLines[i];
    //                        int j = i;
    //                        while (currentNode.Attributes["fonttype"].Value.Equals("NotEmbeded"))
    //                        {
    //                            prevLine = nodeLines[j];
    //                            currentNode = nodeLines[j--];
    //                        }
    //                        currentNode = nodeLines[i];
    //                        j = i;
    //                        while (currentNode.Attributes["fonttype"].Value.Equals("NotEmbeded"))
    //                        {
    //                            nextLine = nodeLines[j];
    //                            currentNode = nodeLines[j++];
    //                            i++;
    //                        }

    //                        if (MainNode.Attributes["fonttype"].Value.Equals("NotEmbeded"))
    //                        {
    //                            string[] PrevLinewords = prevLine.InnerText.Split(' ');
    //                            string[] NextLinewords = nextLine.InnerText.Split(' ');
    //                            string preLine = "";
    //                            string postLine = "";
    //                            for (int k = 1; k < PrevLinewords.Length; k++)
    //                            {
    //                                if (k == 4) break;
    //                                preLine = PrevLinewords[PrevLinewords.Length - k] + " " + preLine;
    //                            }
    //                            preLine = preLine.Remove(preLine.Length - 1);
    //                            for (int k = 0; k < NextLinewords.Length; k++)
    //                            {
    //                                if (k == 3) break;
    //                                postLine = postLine + NextLinewords[k] + " ";
    //                            }

    //                            postLine = postLine.Remove(postLine.Length - 1);
    //                            string RegexPattren = preLine + "[\\w\\W\\S\\s\\s\\D':;\"<>,.?]*" + postLine;
    //                            Match Matches = Regex.Match(stBuilder.ToString(), RegexPattren);
    //                            MainNode.InnerText = Matches.Value.Replace(preLine, "").Replace(postLine, "");

    //                            XmlAttribute AttribTextManuplated = xmldoc.CreateAttribute("TextManuplated");
    //                            MainNode.Attributes.Append(AttribTextManuplated);

    //                        }
    //                    }
    //                }
    //                nodeLines = xmldoc.SelectNodes("//ln[@page=\"1\" and @ fonttype=\"NotEmbeded\"]");
    //                for (int i = 0; i < nodeLines.Count; i++)
    //                {
    //                    if (nodeLines[i].Attributes["TextManuplated"] == null)
    //                    {
    //                        nodeLines[i].ParentNode.RemoveChild(nodeLines[i]);
    //                    }
    //                }

    //                XmlNodeList uparaList = xmldoc.SelectNodes("//upara");
    //                for (int i = 0; i < uparaList.Count; i++)
    //                {
    //                    if (uparaList[i].ChildNodes.Count == 0)
    //                    {
    //                        uparaList[i].ParentNode.RemoveChild(uparaList[i]);
    //                    }
    //                }
    //            }
    //        }
    //    }


    //}


    //protected void Submit_Click(object sender, EventArgs e)
    //{
    //    if (string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
    //        Response.Redirect("Bookmicro.aspx", true);

    //    try
    //    {
    //        string dirName = this.txtBookID.Text;
    //        string bookDir = objMyDBClass.MainDirPhyPath + "\\" + dirName;
    //        string savingPath = "";
    //        string serviceDirectoryPath = "";
    //        bool isPdfUploaded = true;

    //        if (!Directory.Exists(bookDir))
    //        {
    //            try
    //            {
    //                Directory.CreateDirectory(bookDir);
    //            }
    //            catch (Exception exp)
    //            {
    //                ucShowMessage1.ShowMessage(MessageTypes.Error, "Some error has occured while uploading book.");
    //                LogWritter.WriteLineInLog("Exception in creating directory, msg: " + exp.Message);
    //            }
    //        }
    //        else
    //        {
    //            ucShowMessage1.ShowMessage(MessageTypes.Error, "Book with same ID already exist");
    //            return;
    //        }

    //        string fileName = this.txtBookID.Text;
    //        HttpFileCollection hfc = Request.Files;

    //        if (hfc.Count == 2 && this.txtBookID.Text != "" && this.txtBookTitle.Text != "")
    //        {
    //            for (int i = 0; i <= hfc.Count - 1; i++)
    //            {
    //                HttpPostedFile hpf = hfc[i];

    //                if (hpf.FileName != "")
    //                {
    //                    string extension = Path.GetExtension(hpf.FileName);

    //                    if ((extension == ".pdf") && (!(Path.GetFileNameWithoutExtension(hpf.FileName).ToLower().Trim().Contains("actual"))))
    //                    {
    //                        savingPath = bookDir + "\\" + dirName + extension;
    //                        serviceDirectoryPath = savingPath;
    //                    }
    //                    else
    //                    {
    //                        savingPath = bookDir + "\\" + dirName + "_actual" + extension;
    //                    }

    //                    if (savingPath != "")
    //                    {
    //                        try
    //                        {
    //                            hpf.SaveAs(savingPath);
    //                        }
    //                        catch (Exception)
    //                        {
    //                            isPdfUploaded = false;
    //                        }
    //                    }
    //                    else
    //                    {
    //                        ucShowMessage1.ShowMessage(MessageTypes.Error, "Please Upload .PDF File.");
    //                        Directory.Delete(bookDir);
    //                        return;
    //                    }
    //                }
    //            }
    //        }
    //        else
    //        {
    //            //((AdminMaster)this.Master).ShowMessageBox("Please Fill Out All Fields", "Info");
    //            ucShowMessage1.ShowMessage(MessageTypes.Error, "Please select 2 PDF files.");
    //            Directory.Delete(bookDir);
    //            return;
    //        }

    //        if (isPdfUploaded)
    //        {
    //            WorkMeter wmObj = new WorkMeter();
    //            string inceptionTaskId = "29";
    //            wmObj.InsertTaskInWorkMeter(inceptionTaskId, savingPath, txtBookID.Text.Trim());
    //        }

    //        if (savingPath != "")
    //        {
    //            string existRecQuery = "SELECT * FROM MainBook Where MainBook='" + this.txtBookID.Text + "'";
    //            DataSet ds = objMyDBClass.GetDataSet(existRecQuery);
    //            if (ds.Tables[0].Rows.Count > 0)
    //            {
    //                //((AdminMaster)this.Master).ShowMessageBox("Book with same ID already exist", "error");
    //                ucShowMessage1.ShowMessage(MessageTypes.Error, "Book with same ID already exist");
    //                Directory.Delete(bookDir);
    //            }
    //            else
    //            {
    //                Response.Write("<script>alert('In Async Call for mapping - It should display message on page i.e. Please Check back your tasks for assignment in <b><i>30</i></b> minutes');</script>");
    //                string insertRecQuery = "Insert Into MainBook Values('" + this.txtBookID.Text + "','" + txtBookTitle.Text + "','incomplete','" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "')";
    //                int insRes = objMyDBClass.ExecuteCommand(insertRecQuery);
    //                if (insRes > 0)
    //                {
    //                    objMyDBClass.LogEntry(this.txtBookID.Text, "AutoMapp", "Main entry completed. Calling AutoMapp Web Service", "In Progress", "insert");

    //                    //Session["MainBook"] = txtBookID.Text.Trim();

    //                    Outsourcing_System.AutoMapService.AutoMappService autoMapSvc = new Outsourcing_System.AutoMapService.AutoMappService();
    //                    autoMapSvc.AllowAutoRedirect = true;

    //                    var id = (Session["objUser"] as UserClass).UserID;

    //                    autoMapSvc.CreateTaggingUntaggingAsync((Session["objUser"] as UserClass).UserID, txtBookTitle.Text, serviceDirectoryPath);
    //                    System.Threading.Thread.Sleep(1000 * 12);
    //                    autoMapSvc.CancelAsync(null);
    //                    autoMapSvc.Dispose();

    //                    Response.Redirect("AdminPanel.aspx", false);
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        ucShowMessage1.ShowMessage(MessageTypes.Error, "Some error has occured while uploading book.");
    //    }
    //}

    protected void Submit_Click(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
            Response.Redirect("Bookmicro.aspx", true);

        if (!fuPdf.HasFile)
        {
            ucShowMessage1.ShowMessage(MessageTypes.Error, "Please select an editted PDF file.");
            return;
        }
        else if (string.IsNullOrEmpty(txtBookID.Text.Trim()))
        {
            ucShowMessage1.ShowMessage(MessageTypes.Error, "Please enter book Id.");
            return;
        }
        else if (string.IsNullOrEmpty(txtBookTitle.Text.Trim()))
        {
            ucShowMessage1.ShowMessage(MessageTypes.Error, "Please enter book title.");
            return;
        }

        string dirName = this.txtBookID.Text;
        string bookDir = objMyDBClass.MainDirPhyPath + "\\" + dirName;
        string savingPath = "";
        string serviceDirectoryPath = "";
        //bool isPdfUploaded = true;

        if (!Directory.Exists(bookDir))
        {
            try
            {
                Directory.CreateDirectory(bookDir);
            }
            catch (Exception exp)
            {
                ucShowMessage1.ShowMessage(MessageTypes.Error, "Some error has occured while uploading book.");
                LogWritter.WriteLineInLog("Exception in creating directory, msg: " + exp.Message);
            }
        }
        else
        {
            ucShowMessage1.ShowMessage(MessageTypes.Error, "Book with same ID already exist");
            return;
        }

        if (fuPdf.HasFile && !string.IsNullOrEmpty(fuPdf.FileName) && Path.GetExtension(fuPdf.FileName) == ".pdf")
        {
            savingPath = bookDir + "\\" + dirName + Path.GetExtension(fuPdf.FileName);
            serviceDirectoryPath = savingPath;

            if (!string.IsNullOrEmpty(savingPath))
            {
                try
                {
                    fuPdf.SaveAs(savingPath);
                }
                catch (Exception)
                {
                    //isPdfUploaded = false;
                    ucShowMessage1.ShowMessage(MessageTypes.Error, "Some error has occured while uploading book.");
                }

            }
        }
        try
        {

            //if (isPdfUploaded)
            //{
            //    WorkMeter wmObj = new WorkMeter();
            //    string inceptionTaskId = "29";
            //    wmObj.InsertTaskInWorkMeter(inceptionTaskId, savingPath, txtBookID.Text.Trim());
            //}

            if (savingPath != "")
            {
                Session["MainBook"] = txtBookID.Text.Trim();
                string existRecQuery = "SELECT * FROM MainBook Where MainBook='" + txtBookID.Text + "'";
                DataSet ds = objMyDBClass.GetDataSet(existRecQuery);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    //((AdminMaster)this.Master).ShowMessageBox("Book with same ID already exist", "error");
                    ucShowMessage1.ShowMessage(MessageTypes.Error, "Book with same ID already exist");
                    Directory.Delete(bookDir);
                }
                else
                {
                    Response.Write("<script>alert('In Async Call for mapping - It should display message on page i.e. Please Check back your tasks for assignment in <b><i>30</i></b> minutes');</script>");
                    string insertRecQuery = "Insert Into MainBook Values('" + txtBookID.Text + "','" + txtBookTitle.Text + "','incomplete','" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "')";
                    int insRes = objMyDBClass.ExecuteCommand(insertRecQuery);
                    if (insRes > 0)
                    {
                        objMyDBClass.LogEntry(this.txtBookID.Text, "AutoMapp", "Main entry completed. Calling AutoMapp Web Service", "In Progress", "insert");

                        //Session["MainBook"] = txtBookID.Text.Trim();

                        Outsourcing_System.AutoMapService.AutoMappService autoMapSvc = new Outsourcing_System.AutoMapService.AutoMappService();
                        autoMapSvc.AllowAutoRedirect = true;

                        var id = (Session["objUser"] as UserClass).UserID;

                        autoMapSvc.CreateTaggingUntaggingAsync((Session["objUser"] as UserClass).UserID, txtBookTitle.Text, serviceDirectoryPath);
                        System.Threading.Thread.Sleep(1000 * 12);
                        autoMapSvc.CancelAsync(null);
                        autoMapSvc.Dispose();

                        //Response.Redirect("AdminPanel.aspx", false);

                        Response.Redirect("AdminPanel.aspx", false);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            ucShowMessage1.ShowMessage(MessageTypes.Error, "Some error has occured while uploading book.");
        }
    }

    protected void btnReset_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdminPanel.aspx");
    }


    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("BookMicro.aspx");
    }



}

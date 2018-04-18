using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using Outsourcing_System;
using System.Text.RegularExpressions;
using System.Data.OleDb;
using BookMicroBeta;
using System.Collections.Generic;
using System.Xml;
using Outsourcing_System.PdfCompare_Classes;


public partial class SubmitTask : System.Web.UI.Page
{
    MyDBClass objMyDBClass = new MyDBClass();
    GlobalVar objGlobal = new GlobalVar();

    protected void Page_Load(object sender, EventArgs e)
    {
        //ZipUtility zip = new ZipUtility();

        //int tables = zip.CountTableSheets(@"D:\OnlineTasks\24588\tables\24588.zip");       
        //int images = zip.CountImages(@"D:\OnlineTasks\12826_Images\images\12826.zip");




        //this.Title = "Outsourcing System :: Submit Assigned Task";
        //this.lblMessage.Text = "";
        //this.pnlDelConfirmation.Visible = false;
        //this.pnlExtraImgConf.Visible = false;
        if (Session["LoginId"] != null)
        {
            string query = "Select * from [User] where UID=" + Session["LoginId"].ToString();

            DataSet ds = objMyDBClass.GetDataSet(query);
            if (ds.Tables[0].Rows.Count > 0)
            {
                UserClass objUser = new UserClass();
                objUser.UserID = ds.Tables[0].Rows[0]["UID"].ToString();
                objUser.UserName = ds.Tables[0].Rows[0]["UName"].ToString();
                objUser.UserType = ds.Tables[0].Rows[0]["UType"].ToString();
                objUser.UserFullName = ds.Tables[0].Rows[0]["UNAME"].ToString();
                objUser.UserEmail = ds.Tables[0].Rows[0]["Email"].ToString();
                Session["objUser"] = objUser;
            }
            query = "Select * from Activity where aid='" + Session["AID"].ToString() + "' AND UID=" + (Session["objUser"] as UserClass).UserID;
            ds = objMyDBClass.GetDataSet(query);
            this.txtProcess.Text = ds.Tables[0].Rows[0]["Task"].ToString();

            btnSubmit0.Visible = true;

            this.txtComments.Text = ds.Tables[0].Rows[0]["Comments"].ToString();
        }
        else if (Session["objUser"] == null)
        {


            Response.Redirect("BookMicro.aspx");

        }
        else
        {
            string activityID = Session["AID"].ToString();
            string query = "Select * from Activity where AID=" + activityID + " AND UID=" + (Session["objUser"] as UserClass).UserID;
            DataSet ds = objMyDBClass.GetDataSet(query);
            this.txtProcess.Text = ds.Tables[0].Rows[0]["Task"].ToString();

            btnSubmit0.Visible = true;

            this.txtComments.Text = ds.Tables[0].Rows[0]["Comments"].ToString();
        }
    }

    //2017-06-06 old method
    //protected void btnSubmit_Click(object sender, EventArgs e)
    //{
    //    if (fuPdf.PostedFile.FileName != "")
    //    {
    //        string activityID = Session["AID"].ToString();
    //        string bookID = Session["BID"] + "-1";
    //        string fileName = fuPdf.PostedFile.FileName;
    //        string extension = Path.GetExtension(fuPdf.PostedFile.FileName);
    //        string process = this.txtProcess.Text;
    //        //string uploadPath = Server.MapPath("~/" + objMyDBClass.MainDirectory + "\\" + bookID.Split(new char[] { '-' })[0] + "\\" + bookID + "\\" + this.txtProcess.Text + "\\" + bookID + extension);
    //        string dirPath = System.Configuration.ConfigurationManager.AppSettings["MainDirectory"];
    //        string MainBook = Session["MainBook"].ToString();
    //        string uploadPath = objMyDBClass.MainDirPhyPath + "\\" + MainBook + "\\" + MainBook + "-1\\" + this.txtProcess.Text + "\\" + MainBook + extension;
    //        if (process.ToLower().Contains("image") || process.ToLower().Contains("table"))
    //        {
    //            if (extension != ".zip")
    //            {
    //                uploadPath = "";
    //                // this.Master.ShowMessageBox("Please Upload .ZIP File", "Info");
    //            }
    //            else
    //            {
    //                if (process.ToLower().Contains("table"))
    //                {
    //                    objGlobal.XMLPath = objMyDBClass.MainDirPhyPath + "\\" + MainBook + "\\" + MainBook + "-1\\TaggingUntagged\\" + MainBook + "-1.rhyw";
    //                    objGlobal.LoadXml();

    //                    XmlNodeList tablesList = objGlobal.PBPDocument.SelectNodes("//ln[text() = 'Dummy Table']");
    //                    int dummyTableCount = 0;

    //                    if (tablesList != null)
    //                    {
    //                        if (tablesList.Count > 0)
    //                            dummyTableCount = tablesList.Count;
    //                    }

    //                    //string producedFiles = objMyDBClass.MainDirPhyPath + MainBook + "\\" + MainBook + "-1\\" + this.txtProcess.Text + "\\";
    //                    //string[] existingXslFiles = Directory.GetFiles(producedFiles, "*.xlsx");
    //                    fuPdf.PostedFile.SaveAs(uploadPath);
    //                    string uploadedDirectory = unzip(uploadPath);
    //                    string[] uploadedXslFiles = Directory.GetFiles(uploadedDirectory, "*.xlsx");

    //                    if (dummyTableCount == uploadedXslFiles.Length)
    //                    {
    //                        var listPages = SortPages(uploadedXslFiles);

    //                        if (listPages != null)
    //                        {
    //                            if (listPages.Count > 0)
    //                            {
    //                                foreach (var file in listPages)
    //                                {
    //                                    ExceltoXml(file, uploadedDirectory);
    //                                }
    //                            }
    //                        }
    //                    }
    //                    else
    //                    {
    //                        //Show msg
    //                    }
    //                }
    //            }
    //        }
    //        else if (process.ToLower() == "index")
    //        {
    //            if (extension == ".xlsx")
    //            {
    //                uploadPath = "";
    //                //this.Master.ShowMessageBox("Office 2007 Files not supported, Please Upload .xls Files", "error");
    //            }
    //            else if (extension != ".xls")
    //            {
    //                uploadPath = "";
    //                //this.Master.ShowMessageBox("Please Upload .xls File", "error");
    //            }
    //        }
    //        else if (process.ToLower() == "erroradjustment")
    //        {
    //            if (extension != ".rhyw")
    //            {
    //                uploadPath = "";
    //                //this.Master.ShowMessageBox("Please Upload .RHYW File", "error");
    //            }
    //            //uploadPath = Server.MapPath("~/" + objMyDBClass.MainDirectory + "\\" + bookID + "\\" + bookID + extension);
    //            uploadPath = objMyDBClass.MainDirPhyPath + "\\" + bookID + "\\" + bookID + extension;
    //        }
    //        else if (process.ToLower() == "tagginguntagged")
    //        {
    //            if (extension != ".rhyw")
    //            {
    //                uploadPath = "";
    //                //this.Master.ShowMessageBox("Please Upload .RHYW File", "error");
    //            }
    //        }
    //        if (uploadPath != "")
    //        {
    //            if (File.Exists(uploadPath))    //deleting any prevoius .zip file
    //            {
    //                try
    //                {
    //                    File.Delete(uploadPath);
    //                }
    //                catch (Exception exp)
    //                {
    //                    LogWritter.WriteLineInLog("Exception while deleting zip" + exp.Message);
    //                }
    //            }
    //            string extractedDir = uploadPath.TrimEnd(Path.GetExtension(uploadPath).ToCharArray());
    //            //string extractedDir = Directory.GetParent(uploadPath).ToString();
    //            //uploadPath.TrimEnd(Path.GetExtension(uploadPath).ToCharArray());
    //            if (Directory.Exists(extractedDir))
    //            {
    //                string ImgFolder = Directory.GetParent(extractedDir).ToString();
    //                string[] existingJPGFiles = Directory.GetFiles(ImgFolder, "*.jpg");
    //                foreach (string jpgFile in existingJPGFiles)
    //                {
    //                    try
    //                    {
    //                        File.Delete(jpgFile);
    //                    }
    //                    catch (Exception exp)
    //                    {
    //                        LogWritter.WriteLineInLog("Exception while deleting jpg: " + exp.Message);
    //                    }
    //                }
    //                try
    //                {
    //                    Directory.Delete(extractedDir, true);
    //                }
    //                catch (Exception exp)
    //                {
    //                    LogWritter.WriteLineInLog("Exception while deleting directory: " + exp.Message);
    //                }
    //            }
    //            //else
    //            //{
    //            //    ShowMessage("Task does not exist currently, Server Error");
    //            //    LogWritter.WriteLineInLog("Directory Does not exist cannot upload file: " + extractedDir);
    //            //    return;
    //            //}
    //            try
    //            {
    //                fuPdf.PostedFile.SaveAs(uploadPath);
    //            }
    //            catch (Exception exp)
    //            {
    //                LogWritter.WriteLineInLog("Exception while saving zip File: " + uploadPath + " Msg: " + exp.Message);
    //                //this.Master.ShowMessageBox("Error while uploading, please check Zip and retry later", "error");
    //                return;
    //            }
    //            string uploadStatus = "";
    //            string[] retVal = new string[] { "" };
    //            if (process.ToLower() == "index" && extension == ".xls")//Index
    //            {
    //                Outsourcing_System.Index_Service.IndexService indexServic = new Outsourcing_System.Index_Service.IndexService();
    //                uploadStatus = indexServic.ValidateIndexFile(uploadPath);
    //                retVal = uploadStatus.Split(';');
    //                string serviceMsg = "";
    //                if (retVal.Length > 1)
    //                {
    //                    serviceMsg = retVal[1];
    //                }
    //                if (retVal.Length > 1 && retVal[1] == "Successfull")
    //                {
    //                    int entryCount;
    //                    int.TryParse(retVal[0], out entryCount);
    //                    serviceMsg = retVal[1];
    //                    CompleteTaskIndex(activityID, process, entryCount);
    //                }
    //                if (serviceMsg != "")
    //                {
    //                    //this.Master.ShowMessageBox(serviceMsg, "error");
    //                }
    //                else//Error
    //                {
    //                    //this.Master.ShowMessageBox(uploadStatus, "Info");
    //                }
    //            }
    //            else if (process.ToLower().Contains("image") && extension == ".zip")//Image
    //            {

    //                Outsourcing_System.ImageValidator.ImageValidationService imgValidator = new Outsourcing_System.ImageValidator.ImageValidationService();
    //                try
    //                {
    //                    uploadStatus = imgValidator.ValidateImagesAgainstList(uploadPath, long.Parse(activityID));

    //                    LogWritter.WriteLineInLog("Status returned from webservice: " + uploadStatus);
    //                    retVal = uploadStatus.Split(';');
    //                    if (retVal.Length > 1)
    //                    {
    //                        if (retVal[0] == "1")
    //                        {
    //                            //Success
    //                            uploadStatus = "successfull";
    //                        }
    //                        else if (retVal[0] == "2")
    //                        {
    //                            //Warning Extra Images
    //                            uploadStatus = retVal[1];
    //                            //this.Master.ShowMessageBox(uploadStatus, "Info");
    //                        }
    //                        else if (retVal[0] == "3")
    //                        {
    //                            //Error Images not valid
    //                            uploadStatus = retVal[1];
    //                            ScriptManager.RegisterStartupScript(this, this.GetType(), "popup", "alert('" + uploadStatus + "');", true);
    //                            //ShowMessage(uploadStatus);
    //                        }
    //                        if (retVal[0].Contains("1"))//Successfull
    //                        {
    //                            CompleteTaskImage(activityID, process);
    //                        }
    //                        else if (retVal[0].Contains("2"))//Warning
    //                        {
    //                            this.pnlExtraImgConf.Visible = true;
    //                        }
    //                        else//Error
    //                        {
    //                            //this.Master.ShowMessageBox(uploadStatus, "Info");
    //                        }
    //                    }
    //                    else //TODO: handle service not responding error code here
    //                        uploadStatus = "error in service";
    //                }
    //                catch
    //                {
    //                    //this.Master.ShowMessageBox("Images are uploaded successfully, but not extracted due to security issues. Contact back with admin", "Info");
    //                    uploadStatus = "successfull";
    //                }
    //                finally
    //                {
    //                    imgValidator.Dispose();
    //                }
    //            }
    //            else if (process.ToLower().Contains("table") && extension == ".zip")
    //            {
    //                //Outsourcing_System.ImageValidator.ImageValidationService imgValidator = new Outsourcing_System.ImageValidator.ImageValidationService();

    //                //Outsourcing_System.TableValidation.Service tblValidator = new Outsourcing_System.TableValidation.Service();

    //                try
    //                {
    //                    //uploadStatus = imgValidator.ValidateTables(uploadPath, activityID);
    //                    uploadStatus = "Successfull;Successfull";// tblValidator.ValidateTablesZipFile(uploadPath);
    //                    retVal = uploadStatus.Split(';');
    //                    string tblSerMsg = "";
    //                    if (retVal.Length > 1)
    //                    {
    //                        tblSerMsg = retVal[1];
    //                    }
    //                    if (retVal.Length > 1 && tblSerMsg.Contains("Successfull"))
    //                    {
    //                        //int entryCount;
    //                        //int.TryParse(retVal[0], out entryCount);
    //                        //tblSerMsg = retVal[1];
    //                        //int successfulCount = Regex.Matches(tblSerMsg, "Successfull").Count;

    //                        //if (entryCount == successfulCount)
    //                        //{
    //                        //CompleteTaskTable(activityID, process, 0);
    //                        //}
    //                        //else if (tblSerMsg != "")
    //                        //{
    //                        // //   ShowMessage(tblSerMsg);
    //                        //}
    //                        //else
    //                        //{
    //                        // //   ShowMessage("Cannot Save no table found");
    //                        //    return;
    //                        //}
    //                        //Tables validated
    //                    }
    //                    else if (tblSerMsg == "")
    //                    {
    //                        // ShowMessage("The zip contains no valid tables, re-check the zip and try again");
    //                    }
    //                    else //Error in Tables
    //                    {
    //                        //ShowMessage(tblSerMsg);
    //                    }
    //                }
    //                catch
    //                {
    //                    //this.lblMessage.Text = "Table are uploaded successfully, but not validated";
    //                    uploadStatus = "successfull";
    //                }
    //                finally
    //                {
    //                    // tblValidator.Dispose();
    //                }
    //            }
    //            else
    //            {
    //                uploadStatus = "successfull";
    //            }
    //            //if (retVal[0].Contains("1"))//Successfull
    //            //{
    //            //    CompleteTask(activityID, process);
    //            //}
    //            //else if (retVal[0].Contains("2"))//Warning
    //            //{
    //            //    this.pnlExtraImgConf.Visible = true;
    //            //}
    //            //else//Error
    //            //{
    //            //    ShowMessage(uploadStatus);
    //            //}
    //        }
    //    }
    //    else
    //    {
    //        //this.Master.ShowMessageBox("Please Select Process Relevant File to Upload", "error");
    //    }
    //}

    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(fuPdf.PostedFile.FileName))
            {
                string extension = Path.GetExtension(fuPdf.PostedFile.FileName);
                string process = txtProcess.Text;
                string mainBook = Convert.ToString(Session["MainBook"]);
                string uploadPath = objMyDBClass.MainDirPhyPath + "\\" + mainBook + "\\" + mainBook + "-1\\" + txtProcess.Text;
                string uploadedZipPath = objMyDBClass.MainDirPhyPath + "\\" + mainBook + "\\" + mainBook + "-1\\" + txtProcess.Text + "\\" + mainBook + extension;
                if (process.ToLower().Contains("image"))
                {
                    if (extension != ".zip")
                    {
                        uploadPath = "";
                        ucShowMessage1.ShowMessage(MessageTypes.Info, "Please Upload .ZIP File");
                    }
                    else
                    {
                        if (Directory.Exists(uploadPath))
                        {
                            if (File.Exists(uploadedZipPath))
                                DeletePreviousZip(uploadedZipPath);
                        }
                        else
                        {
                            ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured while uploading zip.");
                            return;
                        }

                        fuPdf.PostedFile.SaveAs(uploadedZipPath);

                        objGlobal.XMLPath = objMyDBClass.MainDirPhyPath + "\\" + mainBook + "\\" + mainBook + "-1\\TaggingUntagged\\" + mainBook + "-1.rhyw";
                        objGlobal.LoadXml();

                        XmlNodeList imagesPlaceHoldersList = objGlobal.PBPDocument.SelectNodes("//image");

                        if (imagesPlaceHoldersList != null && imagesPlaceHoldersList.Count > 1)
                        {
                            string uploadedDirectory = unzip(uploadedZipPath);

                            if (!string.IsNullOrEmpty(uploadedDirectory))
                            {
                                List<string> uploadedImageFiles = Directory.GetFiles(uploadedDirectory, "*.jpg").ToList()
                                                                    .Select(x => Path.GetFileNameWithoutExtension(x)).ToList();

                                if (uploadedImageFiles.Count > 0)
                                {
                                    if (IsZipImgsEqualToXml(imagesPlaceHoldersList, uploadedImageFiles))
                                    {
                                        ucShowMessage1.ShowMessage(MessageTypes.Success, "Images are uploaded successfully.");
                                    }
                                    else
                                        Response.Redirect("PdfImageUpload.aspx", true);
                                }
                                else
                                    ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured while uploading zip.");
                            }
                            else
                                ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured while uploading zip.");
                        }
                        else
                            ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured while uploading zip.");

                    }
                }
            }
            else
            {
                ucShowMessage1.ShowMessage(MessageTypes.Error, "Please Select Process Relevant File to Upload.");
                //this.Master.ShowMessageBox("Please Select Process Relevant File to Upload", "error");
            }
        }
        catch (Exception ex)
        {

        }
    }

    public bool IsZipImgsEqualToXml(XmlNodeList imagesPlaceHoldersList, List<string> uploadedImageFiles)
    {
        if ((imagesPlaceHoldersList.Count - 1) == uploadedImageFiles.Count)
        {
            List<string> xmlImageNameList = imagesPlaceHoldersList.Cast<XmlNode>()
                    .Where(x => x.Attributes != null && x.Attributes["image-url"] != null && !x.Attributes["image-url"].Value.Contains("BookNotices"))
                    .Select(y => Path.GetFileNameWithoutExtension(y.Attributes["image-url"].Value.Replace("Resources\\", ""))).ToList();

            var missingImageInZip = xmlImageNameList.Except(uploadedImageFiles).ToList();

            var missingImageInXml = uploadedImageFiles.Except(xmlImageNameList).ToList();

            List<string> missingImageNameList = new List<string>();

            missingImageNameList.AddRange(missingImageInZip);
            missingImageNameList.AddRange(missingImageInXml);

            if (missingImageNameList.Count > 0)
                return false;
        }
        else
            return false;

        return true;
    }

    private void DeletePreviousZip(string uploadedZipPath)
    {
        if (!string.IsNullOrEmpty(uploadedZipPath) && File.Exists(uploadedZipPath))
        {
            try
            {
                File.Delete(uploadedZipPath);
            }
            catch (Exception exp)
            {
                LogWritter.WriteLineInLog("Exception while deleting zip" + exp.Message);
            }

            string extractedDir = uploadedZipPath.TrimEnd(Path.GetExtension(uploadedZipPath).ToCharArray());
            //string extractedDir = Directory.GetParent(uploadPath).ToString();
            //uploadPath.TrimEnd(Path.GetExtension(uploadPath).ToCharArray());
            if (Directory.Exists(extractedDir))
            {
                //string ImgFolder = Directory.GetParent(extractedDir).ToString();
                string[] existingJPGFiles = Directory.GetFiles(extractedDir, "*.jpg");
                foreach (string jpgFile in existingJPGFiles)
                {
                    try
                    {
                        File.Delete(jpgFile);
                    }
                    catch (Exception exp)
                    {
                        LogWritter.WriteLineInLog("Exception while deleting jpg: " + exp.Message);
                    }
                }
                try
                {
                    Directory.Delete(extractedDir, true);
                }
                catch (Exception exp)
                {
                    LogWritter.WriteLineInLog("Exception while deleting directory: " + exp.Message);
                }
            }
            else
            {
                //ShowMessage("Task does not exist currently, Server Error");
                LogWritter.WriteLineInLog("Directory Does not exist cannot upload file: " + extractedDir);
                return;
            }
        }
    }


    private List<XlsxFile> SortPages(string[] uploadedXslFiles)
    {
        List<XlsxFile> tableXlsxFiles = new List<XlsxFile>();

        foreach (var file in uploadedXslFiles)
        {
            var fileNamesList = Path.GetFileNameWithoutExtension(file).Split('_').ToList();

            if (fileNamesList != null)
            {
                if (fileNamesList.Count > 0)
                {
                    tableXlsxFiles.Add(
                        new XlsxFile
                        {
                            PageNum = Convert.ToInt32(fileNamesList[1]),
                            TableNum = Convert.ToInt32(Path.GetFileNameWithoutExtension(fileNamesList[2])),
                            Extension = Path.GetExtension(file),
                            Name = Path.GetFileNameWithoutExtension(file)
                        });
                }
            }
        }

        if (tableXlsxFiles != null)
        {
            if (tableXlsxFiles.Count > 0)
            {
                return tableXlsxFiles.GroupBy(o => new { o.PageNum, o.TableNum })
                    .Select(o => o.FirstOrDefault()).OrderBy(x => x.PageNum)
                    .ToList();
            }
        }

        return null;
    }

    #region ReadExcelFile

    public static DataTable ReadExcelFile(String filePath,
                                          String sheetName,
                                          String selectFields,
                                          String tableName,
                                          Boolean fileIncludesHeaders)
    {
        DataSet dataSet = null;
        DataTable dtReturn = null;
        string connectionString = string.Empty;
        string commandText = string.Empty;

        // Indicates the Excel file with header or not.
        string headerYesNo = string.Empty;
        string fileExtension = string.Empty;
        try
        {
            //if (fileIncludesHeaders == true)
            //{
            //    // Set YES if excel WithHeader is TRUE.
            //    headerYesNo = "YES";
            //}
            //else
            //{
            // Set NO if excel WithHeader is FALSE.
            headerYesNo = "NO";
            //}

            // Gets file extension for checking.
            fileExtension = Path.GetExtension(filePath);

            switch (fileExtension.ToUpper())
            {
                case ".XLS":

                    //Take Connection For Microsoft Excel File.
                    connectionString = string.Format(@"Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties='Excel 8.0;IMEX=2.0;HDR={1}'",
                                    filePath,
                                    headerYesNo);

                    break;

                case ".XLSX":

                    //Take Connection For Microsoft Excel File.
                    connectionString = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;IMEX=2.0;HDR={1}'",
                                    filePath,
                                    headerYesNo);

                    break;

                default:

                    throw new Exception("File is invalid.");
            }

            commandText = string.Format("SELECT {0} FROM [{1}$]",
                                        selectFields,
                                        sheetName);

            dataSet = new DataSet();
            using (OleDbConnection dbConnection = new OleDbConnection(connectionString))
            {
                OleDbCommand dbCommand = new OleDbCommand(commandText, dbConnection);
                dbCommand.CommandType = CommandType.Text;
                OleDbDataAdapter dbDataAdapter = new OleDbDataAdapter(dbCommand);
                dbDataAdapter.Fill(dataSet);
            }

            if (dataSet != null &&
                dataSet.Tables.Count > 0)
            {
                dataSet.Tables[0].TableName = tableName;

                // Sets reference of data table.
                dtReturn = dataSet.Tables[tableName];
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return dtReturn;
    }

    #endregion

    private void CompleteTaskImage(string activityID, string process)
    {
        string queryUpdate = "Update ACTIVITY Set Status='Pending Confirmation',Comments='" + this.txtComments.Text + "', CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "' WHERE AID=" + activityID;
        int inResult = objMyDBClass.ExecuteCommand(queryUpdate);
        if (inResult > 0)
        {
            //Mail
            string querySelAdmin = "Select AssignedBy From Activity WHERE AID=" + activityID;
            string userAdmin = objMyDBClass.GetID(querySelAdmin);
            objMyDBClass.SendMail(process, userAdmin, (Session["objUser"] as UserClass));
            //End Mail
            Session["condition"] = "Working";
            Response.Redirect("AdminPanel.aspx", false);
            //}
        }
    }

    private void CompleteTaskIndex(string activityID, string process, int indexEntryCount)
    {
        string queryUpdate = "Update ACTIVITY Set Status='Pending Confirmation',Comments='" + this.txtComments.Text + "', CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "', Count=" + indexEntryCount + " WHERE AID=" + activityID;
        int inResult = objMyDBClass.ExecuteCommand(queryUpdate);
        if (inResult > 0)
        {
            //Mail
            string querySelAdmin = "Select AssignedBy From Activity WHERE AID=" + activityID;
            string userAdmin = objMyDBClass.GetID(querySelAdmin);
            objMyDBClass.SendMail(process, userAdmin, (Session["objUser"] as UserClass));
            //End Mail
            Session["condition"] = "Working";
            Response.Redirect("http://175.41.132.19/BookMicroTest/BookMicro.aspx", true);
            // Response.Redirect("UserPanel.aspx");
            //}
        }
    }

    private void CompleteTaskTable(string activityID, string process, int tablesCount)
    {
        string queryUpdate = "Update ACTIVITY Set Status='Approved',Comments='" + this.txtComments.Text + "', CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "', Count=" + tablesCount + " WHERE AID=" + activityID;
        int inResult = objMyDBClass.ExecuteCommand(queryUpdate);
        if (inResult > 0)
        {
            //Mail
            string querySelAdmin = "Select AssignedBy From Activity WHERE AID=" + activityID;
            string userAdmin = objMyDBClass.GetID(querySelAdmin);
            //objMyDBClass.SendMail(process, userAdmin, (Session["objUser"] as UserClass));
            ////End Mail
            //Session["condition"] = "Working";
            //Response.Redirect("http://175.41.132.19/BookMicroTest/BookMicro.aspx", true);
            // Response.Redirect("UserPanel.aspx");
            //}
        }
    }

    //public void ShowMessage(string Message)
    //{
    //    this.lblMessage.Text = Message;



    //    //ScriptManager.RegisterStartupScript(this.Page, GetType(), "ShowLightBoxMessage", "ShowLightBoxMessage('" + Message + "');", true);
    //    //ClientScript.RegisterStartupScript(GetType(), "ShowLightBoxMessage", "alert('helo');", true);
    //    //Response.Redirect("SubmitTask.aspx?Prm=1");
    //}

    protected void LinkButton2_Click(object sender, EventArgs e)
    {
        Response.Redirect("AdminPanel.aspx", false);
    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Session.Clear();
        Response.Redirect("BookMicro.aspx");
    }

    protected void btnSubmit0_Click(object sender, ImageClickEventArgs e)
    {
        pnlDelConfirmation.Visible = true;
    }

    protected void btnUpload_Click(object sender, EventArgs e)
    {
        lblMessage.Text = "this is a test messge from server";
        //ClientScript.RegisterClientScriptBlock(GetType(), "test", "<script>function alertMe(){alert('hello im alerted');}</script>");
        //ClientScript.RegisterStartupScript(GetType(), "test", "<script>alert('hello im here');aClick();alert('hello im here again');</script>");
    }

    protected void btnYes_Click(object sender, EventArgs e)
    {
        string bookID = Session["BID"].ToString();
        string activityID = Session["AID"].ToString();
        string process = this.txtProcess.Text;
        string queryUpdate = "Update ACTIVITY Set Status='Pending Confirmation',Comments='" + this.txtComments.Text + "', CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "' WHERE AID=" + activityID;
        int inResult = objMyDBClass.ExecuteCommand(queryUpdate);
        if (inResult > 0)
        {
            //Mail
            string querySelAdmin = "Select AssignedBy From Activity WHERE AID=" + activityID;
            string userAdmin = objMyDBClass.GetID(querySelAdmin);
            objMyDBClass.SendMail(process, userAdmin, (Session["objUser"] as UserClass));
            //End Mail
            Session["condition"] = "Working";
            Response.Redirect("AdminPanel.aspx", false);
        }
        //this.Master.ShowMessageBox("TAsk Submitteed", "Info");
    }
    protected void btnYesImg_Click(object sender, EventArgs e)
    {
        string activityID = Session["AID"].ToString();
        string process = this.txtProcess.Text;
        string queryUpdate = "Update ACTIVITY Set Status='Pending Confirmation',Comments='" + this.txtComments.Text + "', CompletionDate='" + DateTime.Now.Date.GetDateTimeFormats('d')[5] + "' WHERE AID=" + activityID;
        int inResult = objMyDBClass.ExecuteCommand(queryUpdate);
        if (inResult > 0)
        {
            //Mail
            string querySelAdmin = "Select AssignedBy From Activity WHERE AID=" + activityID;
            string userAdmin = objMyDBClass.GetID(querySelAdmin);
            objMyDBClass.SendMail(process, userAdmin, (Session["objUser"] as UserClass));
            //End Mail
            Session["condition"] = "Working";
            Response.Redirect("AdminPanel.aspx", false);
        }
    }

    protected void btnNo_Click(object sender, EventArgs e)
    {
        pnlDelConfirmation.Visible = false;
    }

    protected void Button2_Click(object sender, EventArgs e)
    {
        pnlExtraImgConf.Visible = false;
    }

    private void ExceltoXml(XlsxFile excelFile, string uploadedDirectory)
    {
        try
        {
            List<List<String>> rows = ReadXlsxFile(excelFile.Name, excelFile.Extension, uploadedDirectory);

            if (rows == null) return;

            //string xmlText = "<table id=\"0\" border=\"off\" head-row=\"on\"><tbody ispreviewpassed=\"false\" page=\"2\"><header/>" +
            //                 "<head-row></head-row><Row></Row><caption/></tbody></table>";

            XmlNode table = objGlobal.PBPDocument.CreateElement("table");
            XmlAttribute idAttr = objGlobal.PBPDocument.CreateAttribute("id");
            idAttr.Value = Convert.ToString(excelFile.TableNum);
            XmlAttribute borderAttr = objGlobal.PBPDocument.CreateAttribute("border");
            borderAttr.Value = "off";
            XmlAttribute headrowAttr = objGlobal.PBPDocument.CreateAttribute("head-row");
            headrowAttr.Value = "off";
            table.Attributes.Append(idAttr);
            table.Attributes.Append(borderAttr);
            table.Attributes.Append(headrowAttr);
            //XmlNode tbodyNode = objGlobal.PBPDocument.CreateElement("tbody");
            //XmlAttribute ispreviewpassedAttr = objGlobal.PBPDocument.CreateAttribute("ispreviewpassed");
            //XmlAttribute pageAttr = objGlobal.PBPDocument.CreateAttribute("page");
            //tbodyNode.Attributes.Append(ispreviewpassedAttr);
            //pageAttr.Value = PageNum;
            //tbodyNode.Attributes.Append(pageAttr);
            XmlNode headerNode = objGlobal.PBPDocument.CreateElement("header");
            table.AppendChild(headerNode);
            XmlNode voiceDescriptionNode = objGlobal.PBPDocument.CreateElement("voice-description");
            table.AppendChild(voiceDescriptionNode);
            XmlNode headrowNode = objGlobal.PBPDocument.CreateElement("head-row");
            table.AppendChild(headrowNode);
            XmlNode RowNode = objGlobal.PBPDocument.CreateElement("Row");
            table.AppendChild(RowNode);
            XmlNode captionNode = objGlobal.PBPDocument.CreateElement("caption");
            table.AppendChild(captionNode);
            //table.AppendChild(tbodyNode);

            for (int i = 0; i < rows.Count; i++)
            {
                if (i == 0)
                {
                    XmlNode headeRow = table.SelectSingleNode("//head-row");

                    for (int j = 0; j < rows[i].Count; j++)
                    {
                        XmlNode headercol = objGlobal.PBPDocument.CreateElement("head-col");
                        XmlAttribute headerWidth = objGlobal.PBPDocument.CreateAttribute("width");
                        headercol.Attributes.Append(headerWidth);
                        headercol.InnerText = rows[i][j];
                        headeRow.AppendChild(headercol);
                    }
                }
                else
                {
                    XmlNode lastRow = table.SelectNodes("//Row").Cast<XmlNode>().Last();
                    XmlNode rowNode = objGlobal.PBPDocument.CreateElement("Row");
                    for (int j = 0; j < rows[i].Count; j++)
                    {
                        XmlNode column = objGlobal.PBPDocument.CreateElement("col");
                        column.InnerText = rows[i][j];
                        rowNode.AppendChild(column);
                    }
                    lastRow.ParentNode.InsertAfter(rowNode, lastRow);
                }
            }
            table.SelectNodes("//Row").Cast<XmlNode>().First().ParentNode.RemoveChild(table.SelectNodes("//Row").Cast<XmlNode>().First());

            string bookId = Session["MainBook"].ToString();
            string dirPath = objMyDBClass.MainDirPhyPath + "\\" + bookId + "\\" + bookId + "-1\\" + this.txtProcess.Text;
            string xmlDirPath = dirPath + "//TableXmls";
            if (!File.Exists(xmlDirPath))
                Directory.CreateDirectory(xmlDirPath);
            string tableSavingPath = xmlDirPath + "//" + Path.GetFileNameWithoutExtension(excelFile.Name) + ".xml";
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(table.OuterXml);
            xmlDoc.Save(tableSavingPath);

            //XmlNode dummyTable = objGlobal.PBPDocument.SelectSingleNode("//table[@id='" + Path.GetFileNameWithoutExtension(excelFile).Replace("Table_","") + "']");
            //if (dummyTable != null)
            //{
            //    dummyTable.ParentNode.InsertAfter(table, dummyTable);
            //    dummyTable.ParentNode.RemoveChild(dummyTable);
            //    objGlobal.SaveXml();
            //}       


            XmlNodeList listLines = objGlobal.PBPDocument.SelectNodes("//*[@page=" + excelFile.PageNum + "]");

            if (listLines != null)
            {
                if (listLines.Count > 0)
                {
                    for (int i = 0; i < listLines.Count; i++)
                    {
                        if (listLines[i].InnerText.Equals("Dummy Table"))
                        {
                            //XmlNode dummyTable = xmlDoc.CreateElement("Table");
                            //dummyTable.InnerXml = xmlDoc.InnerXml;
                            listLines[i].ParentNode.ParentNode.InsertBefore(table, listLines[i].ParentNode);
                            listLines[i].ParentNode.ParentNode.RemoveChild(listLines[i].ParentNode);
                        }
                    }
                    objGlobal.SaveXml();
                }
            }
        }
        catch (Exception)
        {

        }
    }

    private string unzip(string zipFilePath)
    {
        ZipUtility zip = new ZipUtility();
        string unZipPath = zipFilePath.Replace(".zip", "");
        if (!Directory.Exists(unZipPath))
        {
            Directory.CreateDirectory(unZipPath);
        }
        zip.setunzipPath(unZipPath);
        zip.ExtractZipFile(zipFilePath);
        return unZipPath;
    }

    public List<List<String>> ReadXlsxFile(string xlsFileName, string ext, string uploadedDirectory)
    {
        try
        {
            string conStr = string.Empty;
            List<List<String>> rows = null;
            List<string> row = null;
            int columns = 0;

            if (ext.Trim().ToLower().Equals(".xls"))
                conStr = @"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + uploadedDirectory + "\\" + xlsFileName + ext + ";Extended Properties='Excel 8.0;IMEX=2.0;HDR=NO'";

            if (ext.Trim().ToLower().Equals(".xlsx"))
                conStr = @"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + uploadedDirectory + "\\" + xlsFileName + ext + ";Extended Properties='Excel 12.0;IMEX=2.0;HDR=NO'";

            string queryString = "SELECT * FROM [sheet$]";
            OleDbConnection con = new OleDbConnection(conStr);
            OleDbCommand cmd = new OleDbCommand(queryString, con);

            con.Open();
            using (con)
            {
                OleDbDataReader dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    rows = new List<List<string>>();
                    while (dr.Read())
                    {
                        if (columns == 0) columns = dr.FieldCount;
                        row = new List<string>();
                        for (int i = 0; i < columns; i++)
                        {
                            row.Add(Convert.ToString(dr[i]));
                        }
                        rows.Add(row);
                    }
                }
            }
            return rows;
        }
        catch (Exception)
        {
            return null;
        }
    }
}
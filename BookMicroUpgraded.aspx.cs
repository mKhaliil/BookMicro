using Outsourcing_System.PdfCompare_Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace Outsourcing_System
{
    public partial class BookMicroUpgraded : System.Web.UI.Page
    {
        #region |Fields and Properties|

        GlobalVar objGlobal = new GlobalVar();
        MyDBClass objMyDBClass = new MyDBClass();

        #endregion

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (string.IsNullOrEmpty(Convert.ToString(Session["LoginId"])))
                {
                    Response.Redirect("BookMicro.aspx");
                }

                //if (Request.Cookies["Email"] != null && Request.Cookies["Password"] != null)
                //{
                //    tbxEmail.Text = Request.Cookies["Email"].Value;
                //    tbxPassword.Attributes["value"] = Request.Cookies["Password"].Value;
                //    cbxRememberMe.Checked = true;
                //}
                //else
                //{
                //    tbxEmail.Text = "";
                //    tbxPassword.Text = "";
                //    tbxPassword.Attributes["value"] = "";
                //    cbxRememberMe.Checked = false;
                //}

                Page.MaintainScrollPositionOnPostBack = true;

                if (Convert.ToString(Session["CountryName"]).Equals("pakistan"))
                {
                    mvTrainingVideos.ActiveViewIndex = 1;
                    lbtnUrdu.Attributes.Add("Style", "color:#CCCCCC");
                    lbtnEnglish.Attributes.Add("Style", "color:#0099ff");
                    lbtnUrdu.Enabled = false;
                    lbtnEnglish.Enabled = true;
                }
                else if (Convert.ToString(Session["CountryName"]).Equals("other"))
                {
                    mvTrainingVideos.ActiveViewIndex = 1;
                    lbtnUrdu.Attributes.Add("Style", "color:#0099ff");
                    lbtnEnglish.Attributes.Add("Style", "color:#CCCCCC");
                    //lbtnUrdu.Enabled = true;

                    lbtnUrdu.Enabled = false;
                    lbtnEnglish.Enabled = false;
                }
            }
        }

        protected void lbtnSampletest_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["OnlineTestUser"])) && !string.IsNullOrEmpty(Convert.ToString(Session["Email"])))
            {
                SampleTest(Convert.ToString(Session["OnlineTestUser"]), Convert.ToString(Session["Email"]));
            }
        }

        protected void lbtnStartTest_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(Convert.ToString(Session["OnlineTestUser"])) && !string.IsNullOrEmpty(Convert.ToString(Session["Email"])))
            {
                StartTest(Convert.ToString(Session["OnlineTestUser"]), Convert.ToString(Session["Email"]));
            }
        }

        public void SampleTest(string username, string email)
        {
            StringBuilder notDeletedTest = new StringBuilder();
            string[] temp = null;
            string[] allTestNumbers = { "124", "125", "126", "127", "128", "129", "130","131", "132", "133", "134" };

            int newTestNumber = 0;
            string testType = "CompUpgradedSampleTest";
            int totalOnlineUsers = Convert.ToInt32(objMyDBClass.OnlineUsers());
            if (totalOnlineUsers < 500)
            {
                ////Passed users cannot give tests of same type again
                //string userDetails = objMyDBClass.CheckTestStatus(username, email, testType);

                //if ((userDetails != null) && (userDetails != ""))
                //{
                //    ucShowMessage1.ShowMessage(MessageTypes.Error, "Test can't be started because you have already passed the test.");
                //    //imgbtnStartTest.Enabled = false;
                //    return;
                //}
                ////end

                ////Passed users cannot give tests of different types in next 24 hours
                //var testDates = objMyDBClass.GetTestDate(username, email);

                //if ((testDates != null) && (testDates.Count > 0))
                //{
                //    if (DateTime.Now <= Convert.ToDateTime(testDates[0]).AddHours(24))
                //    {
                //        ucShowMessage1.ShowMessage(MessageTypes.Error, "Test can't be started because only one attempt per day is allowed.");
                //        return;
                //    }
                //}
                ////end

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email))
                {
                    objMyDBClass.InsertOnlineUsers(email);
                    Session["OnlineTestUser"] = username;
                    Session["email"] = email;

                    //string path = objMyDBClass.MainDirPhyPath + @"\\Tests\\Original\\";
                    string ipAddress = GetIPAddress();

                    ////Resriction on using same IP Address for different email addresses
                    //List<TestUser> list = objMyDBClass.GetUserDetails_ByIPAdress(username, email, ipAddress);
                    //bool emailCheck = false;

                    //if ((list != null) && (list.Count > 0))
                    //{
                    //    foreach (var item in list)
                    //    {
                    //        if ((item.Email != email))
                    //        {
                    //            emailCheck = true;
                    //        }
                    //    }

                    //    if (emailCheck)
                    //    {
                    //        string value1 = objMyDBClass.InsertOnlineTest_IPAddress(username, email, ipAddress);
                    //    }
                    //}
                    ////end

                    List<string> emailId_List = objMyDBClass.GetEmailId_ByName(username);
                    bool check = false;

                    if ((emailId_List != null) && (emailId_List.Count > 0))
                    {
                        foreach (var item in emailId_List)
                        {
                            if (item.Trim().Equals(email))
                            {
                                check = true;
                                break;
                            }
                        }
                    }

                    Random rnd = new Random();
                    int rndNumber = 0;

                    rndNumber = rnd.Next(124, 134);

                    //If user has previously take a test then select next test number
                    if (check)
                    {
                        int oldTestName = objMyDBClass.GetTestName(username, email);

                        if (oldTestName == rndNumber)
                        {
                            oldTestName++;

                            if (oldTestName >= 134)
                                rndNumber = 124;

                            else
                                rndNumber = oldTestName;
                        }
                    }
                    //end


                    string userDir_Path = objMyDBClass.MainDirPhyPath + "\\Tests\\" + email;

                    //If user directory not exists then create it
                    if (!Directory.Exists(userDir_Path))
                    {
                        Directory.CreateDirectory(userDir_Path);
                    }

                    //Delete all previous test in comparisonTest Folder.If any test can't be deleted and current test number is 
                    //same as that test then select some different test
                    string userCompTestsDir = objMyDBClass.MainDirPhyPath + "\\Tests\\" + email + "/ComparisonTests/";

                    DirectoryInfo di = new DirectoryInfo(userCompTestsDir);

                    //If old test directories exists
                    if (di.Exists)
                    {
                        DirectoryInfo[] oldTests = di.GetDirectories();

                        foreach (var test in oldTests)
                        {
                            notDeletedTest.Append(DeleteDirectories(test.FullName) + ",");
                        }

                        temp = notDeletedTest.ToString().Split(',');

                        if ((temp != null) && (temp.Length > 0))
                        {
                            temp = temp.Where(x => (!string.IsNullOrEmpty(x))).ToArray();
                        }

                        bool contains = temp.Any(s => s.Contains(Convert.ToString(rndNumber)));
                        if (contains)
                        {
                            var unMatched = (from p in allTestNumbers
                                             where !(from test in temp
                                                     select test).Contains(p)
                                             select p);

                            newTestNumber = Convert.ToInt32(unMatched.First());
                        }
                        else
                        {
                            newTestNumber = rndNumber;
                        }
                    }
                    //No old test directory
                    else
                    {
                        newTestNumber = rndNumber;
                    }

                    string userDir = objMyDBClass.MainDirPhyPath + "\\Tests\\" + email + "/ComparisonTests/" + newTestNumber;
                    string orignalDir = Common.GetComparisonEntryTestFiles_InputFilesPath() + "\\" + Convert.ToString(newTestNumber);

                    Directory.CreateDirectory(userDir);
                    DirectoryInfo dInfo = CopyTo(new DirectoryInfo(orignalDir), userDir);
                    //File.Delete(userDir + "\\" + newTestNumber + ".rhyw");
                    //File.Copy(userDir + "\\" + rndNumber + "-1" + "\\TaggingUntagged\\" + rndNumber + "-1.rhyw", userDir + "\\" + rndNumber + ".rhyw");
                    File.Copy(userDir + "\\" + newTestNumber + "-1" + "\\TaggingUntagged\\" + newTestNumber + "-1.rhyw", userDir + "\\" + newTestNumber + "-1" + "\\Comparison\\" + newTestNumber + "-1.rhyw");
                    File.Copy(userDir + "\\" + newTestNumber + "-1" + "\\TaggingUntagged\\" + newTestNumber + "-1.pdf", userDir + "\\" + newTestNumber + "-1" + "\\Comparison\\" + newTestNumber + "-1.pdf");

                    Session["TotalMarks"] = GetTotalMistakes(email, Convert.ToString(newTestNumber));
                    Session["TestName"] = Convert.ToString(newTestNumber);

                    Session["rhywFile"] = newTestNumber + "-1.rhyw";
                    Session["SrcPDF"] = newTestNumber + "-1.pdf";
                    Session["MainXMLFilePath"] = userDir + "\\" + newTestNumber + "-1" + "\\Comparison\\" + newTestNumber + "-1.rhyw";

                    string value = objMyDBClass.InsertOnlineTest(username, email, Convert.ToString(newTestNumber), ipAddress, Convert.ToInt32(Session["TotalMarks"]), testType);

                    if (Convert.ToInt32(value) >= 0)
                    {
                        //Moving xsl files from C to UserDirectory if not exists 
                        try
                        {
                            int requireUpdation = objMyDBClass.CheckOperationalFiles_Updation();

                            int insertedRow = 0;

                            string userDir_StartTest = Common.GetDirectoryPath() + "User Files/Tests/" + email.Trim() + "/XSL";

                            if (!Directory.Exists(userDir_StartTest))
                            {
                                insertedRow = objMyDBClass.InsertOperationalFiles_StartTest(email.Trim());
                            }

                            string orignalXSLFiles = Common.GetXSLSourcePath();

                            if (insertedRow > 0)
                            {
                                if (!Directory.Exists(userDir_StartTest))
                                    Directory.CreateDirectory(userDir_StartTest);

                                DirectoryInfo dInfo_StartTest = CopyTo(new DirectoryInfo(orignalXSLFiles), userDir_StartTest, true);
                            }
                            else if (requireUpdation > 0)
                            {
                                if (Directory.Exists(userDir_StartTest))
                                    Directory.Delete(userDir_StartTest, true);

                                Directory.CreateDirectory(userDir_StartTest);

                                DirectoryInfo dInfo_StartTest = CopyTo(new DirectoryInfo(orignalXSLFiles), userDir_StartTest, true);
                            }
                        }
                        catch (Exception)
                        {
                            ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured.");
                            return;
                        }
                        //end moving files

                        //Response.Redirect("Step2.aspx?username=" + username + "&bid=" + rndNumber, false);
                        Response.Redirect("ComparisonPreProcess.aspx?type=CompUpgradedSampleTest", true);
                    }
                    else if (Convert.ToInt32(value) < 0)
                    {
                        ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured.");
                    }
                }
            }
            else
            {
                ucShowMessage1.ShowMessage(MessageTypes.Error, "There is big traffic, please try later.");
            }
        }

        public void StartTest(string username, string email)
        {
            StringBuilder notDeletedTest = new StringBuilder();
            string[] temp = null;
            string[] allTestNumbers = { "135", "136", "137", "138", "139"};

            int newTestNumber = 0;
            string testType = "CompUpgradedStartTest";
            int totalOnlineUsers = Convert.ToInt32(objMyDBClass.OnlineUsers());
            if (totalOnlineUsers < 500)
            {
                //Passed users cannot give tests of same type again
                string userDetails = objMyDBClass.CheckTestStatus(username, email, testType);

                if (!string.IsNullOrEmpty(userDetails))
                {
                    ucShowMessage1.ShowMessage(MessageTypes.Error, "Test can't be started because you have already passed the test.");
                    //imgbtnStartTest.Enabled = false;
                    return;
                }
                //end

                //Passed users cannot give tests of different types in next 24 hours
                var testDates = objMyDBClass.GetTestDate(username, email);

                if ((testDates != null) && (testDates.Count > 0))
                {
                    if (DateTime.Now <= Convert.ToDateTime(testDates[0]).AddHours(24))
                    {
                        ucShowMessage1.ShowMessage(MessageTypes.Error, "Test can't be started because only one attempt per day is allowed.");
                        return;
                    }
                }
                //end

                if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(email))
                {
                    objMyDBClass.InsertOnlineUsers(email);
                    Session["OnlineTestUser"] = username;
                    Session["email"] = email;

                    string path = objMyDBClass.MainDirPhyPath + @"\\Tests\\Original\\";
                    string ipAddress = GetIPAddress();

                    //Resriction on using same IP Address for different email addresses
                    List<TestUser> list = objMyDBClass.GetUserDetails_ByIPAdress(username, email, ipAddress);
                    bool emailCheck = false;

                    if ((list != null) && (list.Count > 0))
                    {
                        foreach (var item in list)
                        {
                            if ((item.Email != email))
                            {
                                emailCheck = true;
                            }
                        }

                        if (emailCheck)
                        {
                            string value1 = objMyDBClass.InsertOnlineTest_IPAddress(username, email, ipAddress);
                        }
                    }
                    //end

                    List<string> emailId_List = objMyDBClass.GetEmailId_ByName(username);
                    bool check = false;

                    if ((emailId_List != null) && (emailId_List.Count > 0))
                    {
                        foreach (var item in emailId_List)
                        {
                            if (item.Trim().Equals(email))
                            {
                                check = true;
                                break;
                            }
                        }
                    }

                    Random rnd = new Random();
                    int rndNumber = 0;

                    rndNumber = rnd.Next(135, 139);

                    //If user has previously take a test then select next test number
                    if (check)
                    {
                        int oldTestName = objMyDBClass.GetTestName(username, email);

                        if (oldTestName == rndNumber)
                        {
                            oldTestName++;

                            if (oldTestName >= 139)
                                rndNumber = 135;

                            else
                                rndNumber = oldTestName;
                        }
                    }
                    //end


                    string userDir_Path = objMyDBClass.MainDirPhyPath + "\\Tests\\" + email;

                    //If user directory not exists then create it
                    if (!Directory.Exists(userDir_Path))
                    {
                        Directory.CreateDirectory(userDir_Path);
                    }

                    //Delete all previous test in comparisonTest Folder.If any test can't be deleted and current test number is 
                    //same as that test then select some different test
                    string userCompTestsDir = objMyDBClass.MainDirPhyPath + "\\Tests\\" + email + "/ComparisonTests/";

                    DirectoryInfo di = new DirectoryInfo(userCompTestsDir);

                    //If old test directories exists
                    if (di.Exists)
                    {
                        DirectoryInfo[] oldTests = di.GetDirectories();

                        foreach (var test in oldTests)
                        {
                            notDeletedTest.Append(DeleteDirectories(test.FullName) + ",");
                        }

                        temp = notDeletedTest.ToString().Split(',');

                        if ((temp != null) && (temp.Length > 0))
                        {
                            temp = temp.Where(x => (!string.IsNullOrEmpty(x))).ToArray();
                        }

                        bool contains = temp.Any(s => s.Contains(Convert.ToString(rndNumber)));
                        if (contains)
                        {
                            var unMatched = (from p in allTestNumbers
                                             where !(from test in temp
                                                     select test).Contains(p)
                                             select p);

                            newTestNumber = Convert.ToInt32(unMatched.First());
                        }
                        else
                        {
                            newTestNumber = rndNumber;
                        }
                    }
                    //No old test directory
                    else
                    {
                        newTestNumber = rndNumber;
                    }

                    string userDir = objMyDBClass.MainDirPhyPath + "\\Tests\\" + email + "/ComparisonTests/" + newTestNumber;
                    string orignalDir = Common.GetComparisonEntryTestFiles_InputFilesPath() + "\\" + Convert.ToString(newTestNumber);

                    Directory.CreateDirectory(userDir);
                    DirectoryInfo dInfo = CopyTo(new DirectoryInfo(orignalDir), userDir);
                    //File.Delete(userDir + "\\" + newTestNumber + ".rhyw");
                    //File.Copy(userDir + "\\" + rndNumber + "-1" + "\\TaggingUntagged\\" + rndNumber + "-1.rhyw", userDir + "\\" + rndNumber + ".rhyw");
                    File.Copy(userDir + "\\" + newTestNumber + "-1" + "\\TaggingUntagged\\" + newTestNumber + "-1.rhyw", userDir + "\\" + newTestNumber + "-1" + "\\Comparison\\" + newTestNumber + "-1.rhyw");
                    File.Copy(userDir + "\\" + newTestNumber + "-1" + "\\TaggingUntagged\\" + newTestNumber + "-1.pdf", userDir + "\\" + newTestNumber + "-1" + "\\Comparison\\" + newTestNumber + "-1.pdf");

                    Session["TotalMarks"] = GetTotalMistakes(email, Convert.ToString(newTestNumber));
                    Session["TestName"] = Convert.ToString(newTestNumber);

                    Session["rhywFile"] = newTestNumber + "-1.rhyw";
                    Session["SrcPDF"] = newTestNumber + "-1.pdf";
                    Session["MainXMLFilePath"] = userDir + "\\" + newTestNumber + "-1" + "\\Comparison\\" + newTestNumber + "-1.rhyw";

                    string value = objMyDBClass.InsertOnlineTest(username, email, Convert.ToString(newTestNumber), ipAddress, Convert.ToInt32(Session["TotalMarks"]), testType);

                    if (Convert.ToInt32(value) >= 0)
                    {
                        //Moving xsl files from C to UserDirectory if not exists 
                        try
                        {
                            int requireUpdation = objMyDBClass.CheckOperationalFiles_Updation();

                            int insertedRow = 0;

                            string userDir_StartTest = Common.GetDirectoryPath() + "User Files/Tests/" + email.Trim() + "/XSL";

                            if (!Directory.Exists(userDir_StartTest))
                            {
                                insertedRow = objMyDBClass.InsertOperationalFiles_StartTest(email.Trim());
                            }

                            string orignalXSLFiles = Common.GetXSLSourcePath();

                            if (insertedRow > 0)
                            {
                                if (!Directory.Exists(userDir_StartTest))
                                    Directory.CreateDirectory(userDir_StartTest);

                                DirectoryInfo dInfo_StartTest = CopyTo(new DirectoryInfo(orignalXSLFiles), userDir_StartTest, true);
                            }
                            else if (requireUpdation > 0)
                            {
                                if (Directory.Exists(userDir_StartTest))
                                    Directory.Delete(userDir_StartTest, true);

                                Directory.CreateDirectory(userDir_StartTest);

                                DirectoryInfo dInfo_StartTest = CopyTo(new DirectoryInfo(orignalXSLFiles), userDir_StartTest, true);
                            }
                        }
                        catch (Exception)
                        {
                            ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured.");
                            return;
                        }
                        //end moving files

                        //Response.Redirect("Step2.aspx?username=" + username + "&bid=" + rndNumber, false);
                        Response.Redirect("ComparisonPreProcess.aspx?type=CompUpgradedStartTest", true);
                    }
                    else if (Convert.ToInt32(value) < 0)
                    {
                        ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured.");
                    }
                }
            }
            else
            {
                ucShowMessage1.ShowMessage(MessageTypes.Error, "There is big traffic, please try later.");
            }
        }

        public XmlDocument LoadXmlDocument(string xmlPath)
        {
            if ((xmlPath == "") || (xmlPath == null) || (!File.Exists(xmlPath)))
                return null;

            StreamReader strreader = new StreamReader(xmlPath);
            string xmlInnerText = strreader.ReadToEnd();
            strreader.Close();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlInnerText);

            return xmlDoc;
        }

        private XmlDocument LoadTetmlXmlDocument(string tetFilePath)
        {
            XmlDocument tetDoc = new XmlDocument();
            try
            {
                StreamReader sr = new StreamReader(tetFilePath);
                string xmlText = sr.ReadToEnd();
                sr.Close();
                string documentXML = Regex.Match(xmlText, "<Document.*?</Document>", RegexOptions.Singleline).ToString();
                tetDoc.LoadXml(documentXML);
            }
            catch
            {
                return null;
            }
            return tetDoc;
        }

        private void ExtractImages(string pdfPath)
        {
            string dirPath = Directory.GetParent(pdfPath).ToString();

            string bookId = System.IO.Path.GetFileNameWithoutExtension(pdfPath);
            string outfilebase = dirPath + "\\" + bookId + "-1\\Image\\";

            if (!Directory.Exists(outfilebase))
                Directory.CreateDirectory(outfilebase);

            string strParameter = "--targetdir " + outfilebase + " --image " + pdfPath;
            //string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} contentanalysis={nopunctuationbreaks}\" -o \"" + XmlFile + "\" \"" + PDFFilePath + "\"";
            //string Img_Conversion_bat = @"D:\work\tet.exe";
            string Img_Conversion_bat = "C:\\XSL\\tet.exe";
            Process pConvertTetml = new Process();
            pConvertTetml.StartInfo.UseShellExecute = false;
            pConvertTetml.StartInfo.RedirectStandardError = true;
            pConvertTetml.StartInfo.RedirectStandardOutput = true;
            pConvertTetml.StartInfo.CreateNoWindow = true;
            pConvertTetml.StartInfo.Arguments = strParameter;
            pConvertTetml.StartInfo.FileName = Img_Conversion_bat;
            pConvertTetml.Start();
            pConvertTetml.WaitForExit();
        }

        //public void Createtetml(string tetFile, string pdfPath)
        //{
        //    string tetmlExePath = @"C:\XSLBookMicro\tet.exe";

        //    string strParameter = "-m word --pageopt \"tetml={glyphdetails={geometry=false font=true}} " +
        //        "contentanalysis={nopunctuationbreaks}\" -o \"" +
        //        tetFile.Replace(".pdf", ".tetml") + "\" \"" + pdfPath + "\"";
        //    Process pConvertTetml = new Process();
        //    pConvertTetml.StartInfo.UseShellExecute = false;
        //    pConvertTetml.StartInfo.RedirectStandardError = true;
        //    pConvertTetml.StartInfo.RedirectStandardOutput = true;
        //    pConvertTetml.StartInfo.CreateNoWindow = true;
        //    pConvertTetml.StartInfo.Arguments = strParameter;
        //    pConvertTetml.StartInfo.FileName = tetmlExePath;
        //    pConvertTetml.Start();
        //    pConvertTetml.WaitForExit();
        //}

        public void ReadTetml()
        {

        }

        #region |Events|

        #region Reading Lines From OCR XML

        private List<Para> GetLinesByPara_OcrXml(string xmlPath)
        {
            int pageNumber = 0;
            int lineNum = 0;
            int wordNum = 0;
            int para = 0;

            List<CharParams> list_Chars_CurrentLine = null;
            List<CharParams> list_Chars_NextLine = null;
            List<CharParams> list_Chars_PrevLine = null;

            List<OcrLine> list_Lines = null;
            List<OcrWord> list_LineWords = null;
            List<Para> list_Paras = new List<Para>();

            OcrLine ln = null;

            double top = 0;
            double bottom = 0;

            try
            {
                XmlDocument xmlDoc = LoadXml(xmlPath);
                XmlNodeList pages = xmlDoc.SelectNodes("//page");

                foreach (XmlNode page in pages)
                {
                    //list_Lines.Clear();
                    para = 0;
                    lineNum = 0;

                    pageNumber++;

                    XmlNodeList paras = page.SelectNodes("descendant::par");

                    if (paras != null)
                    {
                        if (paras.Count > 0)
                        {
                            //Iterate through all paras
                            for (int i = 0; i < paras.Count; i++)
                            {
                                para++;

                                list_Lines = new List<OcrLine>();

                                //Iterate through lines in a para
                                for (int line = 0; line < paras[i].ChildNodes.Count; line++)
                                {
                                    list_LineWords = new List<OcrWord>();
                                    ln = new OcrLine();
                                    lineNum++;

                                    list_Chars_CurrentLine = GetCharParamsOfLine(paras[i].ChildNodes[line]);
                                    list_LineWords = GetWordsFromLine(paras[i].ChildNodes[line], pageNumber, lineNum);

                                    if ((line + 1) < paras[i].ChildNodes.Count)
                                    {
                                        list_Chars_NextLine = GetCharParamsOfLine(paras[i].ChildNodes[line + 1]);
                                        GetCharParams_Bottom(list_Chars_CurrentLine, list_Chars_NextLine, out bottom);
                                    }
                                    else
                                    {
                                        list_Chars_NextLine = new List<CharParams>();
                                    }

                                    if ((i == 0) && (line == 0))
                                    {
                                        ln.DistanceTopLine = 0;
                                        ln.DistanceBottomLine = Math.Abs(bottom);
                                    }
                                    else if ((i == paras.Count - 1) && (line == paras[i].ChildNodes.Count - 1))
                                    {
                                        ln.DistanceTopLine = Math.Abs(top);
                                        ln.DistanceBottomLine = 0;
                                    }
                                    else
                                    {
                                        ln.DistanceTopLine = Math.Abs(top);
                                        ln.DistanceBottomLine = Math.Abs(bottom);
                                    }

                                    ln.Left = Convert.ToDouble(paras[i].ChildNodes[line].Attributes["l"].Value);
                                    ln.Right = Convert.ToDouble(paras[i].ChildNodes[line].Attributes["r"].Value);
                                    ln.Top = Convert.ToDouble(paras[i].ChildNodes[line].Attributes["t"].Value);
                                    ln.Bottom = Convert.ToDouble(paras[i].ChildNodes[line].Attributes["b"].Value);
                                    ln.Characters = list_Chars_CurrentLine;
                                    ln.Words = list_LineWords;
                                    ln.Line = GetLineFromChars(paras[i].ChildNodes[line]);
                                    ln.Para = para;
                                    ln.ConatinsEmphasisWords = false;

                                    list_Chars_PrevLine = list_Chars_CurrentLine;
                                    GetCharParams_Top(list_Chars_CurrentLine, list_Chars_PrevLine, out top);

                                    ln.LineWidth = Convert.ToDouble(paras[i].ChildNodes[line].Attributes["r"].Value) - Convert.ToDouble(paras[i].ChildNodes[line].Attributes["l"].Value);
                                    ln.LineNumber = lineNum;

                                    if (paras[i].ParentNode.ParentNode.ParentNode.ParentNode.Attributes.Count > 0)
                                    {
                                        if (paras[i].ParentNode.ParentNode.ParentNode.ParentNode.Attributes["blockType"].Value.Equals("Table"))
                                        {
                                            ln.Type = "table";
                                            ln.TableColumns = paras[i].ParentNode.ParentNode.ParentNode.ChildNodes.Count;
                                        }
                                    }

                                    list_Lines.Add(ln);
                                }
                                //end line iteration

                                list_Paras.Add(new Para
                                {
                                    Line = list_Lines,
                                    Page = pageNumber,
                                    PageHeight = Convert.ToDouble(page.Attributes["height"].Value),
                                    PageWidth = Convert.ToDouble(page.Attributes["width"].Value)
                                });
                            }//end all para iteration
                        }
                    }
                }

                return list_Paras;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public XmlDocument LoadXml(string xmlPath)
        {
            XmlDocument tetDoc = new XmlDocument();
            StreamReader sr = new StreamReader(xmlPath);
            string xmlText = sr.ReadToEnd();
            sr.Close();
            string documentXML = System.Text.RegularExpressions.Regex.Match(xmlText, "<document.*?</document>", System.Text.RegularExpressions.RegexOptions.Singleline).ToString();
            string finalDocument = documentXML.Replace(" xmlns=\"http://www.abbyy.com/FineReader_xml/FineReader10-schema-v1.xml\" version=\"1.0\" producer=\"ABBYY FineReader Engine 11\" languages=\"\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:schemaLocation=\"http://www.abbyy.com/FineReader_xml/FineReader10-schema-v1.xml http://www.abbyy.com/FineReader_xml/FineReader10-schema-v1.xml\"", "");
            tetDoc.LoadXml(finalDocument);
            return tetDoc;
        }

        public List<CharParams> GetCharParamsOfLine(XmlNode line)
        {
            List<CharParams> wordChars = new List<CharParams>();
            XmlNodeList charParam = line.SelectNodes("descendant::charParams");

            foreach (XmlNode chParams in charParam)
            {
                if (!(chParams.InnerText.Equals("")))
                {
                    wordChars.Add(new CharParams
                    {
                        Char = chParams.InnerText,
                        Top = Convert.ToDouble(chParams.Attributes["t"].Value),
                        Bottom = Convert.ToDouble(chParams.Attributes["b"].Value),
                        Left = Convert.ToDouble(chParams.Attributes["l"].Value),
                        Right = Convert.ToDouble(chParams.Attributes["r"].Value),
                        Height = (Convert.ToDouble(chParams.Attributes["b"].Value) - Convert.ToDouble(chParams.Attributes["t"].Value))
                    });
                }
            }

            return wordChars;
        }

        public void GetCharParams_Bottom(List<CharParams> list_Chars_CurrentLine, List<CharParams> list_Chars_NextLine, out double bottom)
        {
            bottom = 0;
            bool endLoop = false;
            bool charMatched = false;
            string[] matchingChars = { "a", "e", "o", "n", "s" };
            string selectedChar = null;

            if ((list_Chars_CurrentLine.Count > 0) && (list_Chars_NextLine.Count > 0))
            {
                //foreach (var character in matchingChars)
                //{
                //    if ((list_Chars_CurrentLine.Any(x => (x.Char == character))) && (list_Chars_NextLine.Any(x => (x.Char == character))))
                //    {
                //        selectedChar = character;
                //        charMatched = true;
                //        break;
                //    }
                //}

                //if (charMatched)
                //{
                //    var currentLine = list_Chars_CurrentLine.Where(x => x.Char == selectedChar).ToList();
                //    var nextLine = list_Chars_NextLine.Where(x => x.Char == selectedChar).ToList();

                //    bottom = Math.Abs(currentLine[0].Bottom - nextLine[0].Top);
                //}

                //else
                //{
                foreach (var currentLinechar in list_Chars_CurrentLine)
                {
                    //Only match alphabets
                    if (Regex.IsMatch(currentLinechar.Char, @"^[a-zA-Z]+$"))
                    {
                        if (list_Chars_NextLine.Any(x => (x.Char == currentLinechar.Char)))
                        {
                            foreach (var nextLineChar in list_Chars_NextLine)
                            {
                                if (currentLinechar.Char == nextLineChar.Char)
                                {
                                    bottom = Math.Abs(currentLinechar.Bottom - nextLineChar.Top);
                                    endLoop = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (endLoop)
                        break;
                }
                //}
            }
        }

        public void GetCharParams_Top(List<CharParams> list_Chars_CurrentLine, List<CharParams> list_Chars_PrevLine, out double top)
        {
            top = 0;
            bool endLoop = false;
            bool charMatched = false;
            string[] matchingChars = { "a", "e", "o", "n", "s" };
            string selectedChar = null;


            if ((list_Chars_CurrentLine.Count > 0) && (list_Chars_PrevLine.Count > 0))
            {
                //foreach (var character in matchingChars)
                //{
                //    if ((list_Chars_CurrentLine.Any(x => (x.Char == character))) && (list_Chars_PrevLine.Any(x => (x.Char == character))))
                //    {
                //        selectedChar = character;
                //        charMatched = true;
                //        break;
                //    }
                //}

                //if (charMatched)
                //{
                //    var currentLine = list_Chars_CurrentLine.Where(x => x.Char == selectedChar).ToList();
                //    var nextLine = list_Chars_PrevLine.Where(x => x.Char == selectedChar).ToList();

                //    top = Math.Abs(currentLine[0].Top - nextLine[0].Bottom);
                //}
                //else
                //{
                foreach (var currentLinechar in list_Chars_CurrentLine)
                {
                    //Only match alphabets
                    if (Regex.IsMatch(currentLinechar.Char, @"^[a-zA-Z]+$"))
                    {
                        if (list_Chars_PrevLine.Any(x => (x.Char == currentLinechar.Char)))
                        {
                            foreach (var prevLineChar in list_Chars_PrevLine)
                            {
                                if (currentLinechar.Char == prevLineChar.Char)
                                {
                                    top = Math.Abs(currentLinechar.Top - prevLineChar.Bottom);
                                    endLoop = true;
                                    break;
                                }
                            }
                        }
                    }

                    if (endLoop)
                        break;
                }
                //}
            }
        }

        public string GetLineFromChars(XmlNode line)
        {
            StringBuilder sb = new StringBuilder();
            XmlNodeList charParam = line.SelectNodes("descendant::charParams");

            foreach (XmlNode chParams in charParam)
            {
                if (chParams.InnerText.Equals(""))
                {
                    sb.Append(chParams.InnerText + " ");
                }
                else
                {
                    sb.Append(chParams.InnerText);
                }
            }

            return Convert.ToString(sb);
        }

        public List<OcrWord> GetWordsFromLine(XmlNode line, int pageNum, int lineNum)
        {
            List<OcrWord> lineWords = new List<OcrWord>();
            List<CharParams> wordChars = new List<CharParams>();
            XmlNodeList charParam = line.SelectNodes("descendant::charParams");
            StringBuilder characters = new StringBuilder();

            foreach (XmlNode chParams in charParam)
            {
                if (!chParams.InnerText.Equals(""))
                {
                    characters.Append(chParams.InnerText);
                }
                else
                {
                    lineWords.Add(new OcrWord
                    {
                        Word = Convert.ToString(characters),
                        Page = pageNum,
                        LineNumber = lineNum
                    });

                    characters.Length = 0;
                }
            }

            return lineWords;
        }

        #endregion

        private string GetOcrXmlLines(string pageno)
        {
            StringBuilder stBuilder = new StringBuilder();

            //string extractedPdfPath = "Page-" + pageno + ".pdf";
            //byte[] pdfBytes = File.ReadAllBytes(extractedPdfPath);
            //StartOcrByService(pdfBytes, "Page-" + pageno, NotEmbededDirectory, "");
            string xmlPath = @"D:/Cid Book1/2/Page-1.xml";

            if (File.Exists(xmlPath))
            {
                //Get paras list from whole ocr xml
                List<Para> list_Lines_ByPara = GetLinesByPara_OcrXml(xmlPath);

                if (list_Lines_ByPara != null)
                {
                    if (list_Lines_ByPara.Count > 0)
                    {
                        foreach (var para in list_Lines_ByPara)
                        {
                            foreach (var line in para.Line)
                            {
                                stBuilder.Append(" </br>" + line.Line + " ");
                            }
                        }
                    }
                }
            }
            return Convert.ToString(stBuilder);
        }

        protected void lbtnUrdu_Click(object sender, EventArgs e)
        {
            mvTrainingVideos.ActiveViewIndex = 0;
            lbtnUrdu.Attributes.Add("Style", "color:#CCCCCC");
            lbtnEnglish.Attributes.Add("Style", "color:#0099ff");
            lbtnEnglish.Enabled = true;
            lbtnUrdu.Enabled = false;
        }

        protected void lblEnglish_Click(object sender, EventArgs e)
        {
            mvTrainingVideos.ActiveViewIndex = 0;
            lbtnUrdu.Attributes.Add("Style", "color:#0099ff");
            lbtnEnglish.Attributes.Add("Style", "color:#CCCCCC");
            lbtnUrdu.Enabled = true;
            lbtnEnglish.Enabled = false;
        }

        protected void lbtnForgotPassword_Click(object sender, EventArgs e)
        {
            Response.Redirect("ForgotPassword.aspx");
        }

        public string GetIPAddress()
        {
            //var aa = Request.ServerVariables["REMOTE_ADDR"].ToString();
            //var bb = Request.ServerVariables["http_user_agent"].ToString();
            //var cc = Request.ServerVariables["request_method"].ToString();
            //var server_name = Request.ServerVariables["server_name"].ToString();
            //var dd = Request.ServerVariables["server_port"].ToString();
            //var ee = Request.ServerVariables["server_software"].ToString();
            //var ff= Request.ServerVariables["REMOTE_HOST"].ToString();

            string ipaddress;

            try
            {
                ipaddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

                if (ipaddress == "" || ipaddress == null)
                    ipaddress = Request.ServerVariables["REMOTE_ADDR"];

                return ipaddress;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        private void loadLatestXml_New(string email, string rndNumber)
        {

            string xmlFile = "";
            if (email != "")
            {
                //xmlFile = objMyDBClass.MainDirPhyPath + "/Tests/" + email + "/" + rndNumber + "/" + rndNumber + ".rhyw";
                xmlFile = objMyDBClass.MainDirPhyPath + "\\Tests\\" + email + "/ComparisonTests/" + rndNumber + "/" + rndNumber + "-1/Comparison/" + rndNumber + "-1.rhyw";
            }
            else
            {
                xmlFile = objMyDBClass.MainDirPhyPath + "/" + email + "/" + rndNumber + ".rhyw";
            }
            objGlobal.XMLPath = xmlFile;
            Session["XMLPath"] = objGlobal.XMLPath;
            objGlobal.PBPDocument = new System.Xml.XmlDocument();
            objGlobal.LoadXml();
            Session["PBPDocument"] = objGlobal.PBPDocument;
        }

        private int GetTotalMistakes(string email, string rndNumber)
        {
            loadLatestXml_New(email, rndNumber);
            XmlNodeList correctedNodes = objGlobal.PBPDocument.SelectNodes(@"//*[@PDFmistake]");
            int totalOccurences = correctedNodes.Count;
            return totalOccurences;
        }

        

        public static string DeleteDirectories(string userDir_Path)
        {
            try
            {
                if (Directory.Exists(userDir_Path))
                    Directory.Delete(userDir_Path, true);
            }
            catch (IOException)
            {
                //the file is unavailable because it is:
                //still being written to
                //or being processed by another thread
                //or does not exist (has already been processed)
                return System.IO.Path.GetFileName(userDir_Path);
            }

            //file is deleted
            return "";
        }

        public DirectoryInfo CopyTo(DirectoryInfo sourceDir, string destinationPath, bool overwrite = false)
        {
            var sourcePath = sourceDir.FullName;

            var destination = new DirectoryInfo(destinationPath);

            destination.Create();

            foreach (var sourceSubDirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
                Directory.CreateDirectory(sourceSubDirPath.Replace(sourcePath, destinationPath));

            foreach (var file in Directory.GetFiles(sourcePath, "*", SearchOption.AllDirectories))
                File.Copy(file, file.Replace(sourcePath, destinationPath), overwrite);

            return destination;
        }

        //protected void imgbtnLogin_Click(object sender, System.EventArgs e)
        //{
        //    MyDBClass objMyDBClass = new MyDBClass();

        //    Session.Clear();

        //    Session["CountryName"] = "other";

        //    if (Convert.ToString(Session["LoginId"]) == "")
        //    {
        //        if (cbxRememberMe.Checked) //If user checks Stay signed in
        //        {
        //            Response.Cookies["email"].Expires = DateTime.Now.AddDays(30);
        //            Response.Cookies["Password"].Expires = DateTime.Now.AddDays(30);
        //        }
        //        else //If next time user don't want to save password
        //        {
        //            Response.Cookies["email"].Expires = DateTime.Now.AddDays(-1);
        //            Response.Cookies["Password"].Expires = DateTime.Now.AddDays(-1);
        //        }

        //        TestUser user_Details = objMyDBClass.Validate_User(Convert.ToString(tbxEmail.Text), Convert.ToString(tbxPassword.Text));

        //        if (user_Details == null)
        //        {
        //            //showMessage("The email or password you entered is incorrect. Please try again (make sure your caps lock is off).");
        //            ucShowMessage1.ShowMessage(MessageTypes.Error, "The email or password you entered is incorrect. Please try again (make sure your caps lock is off).");
        //            return;
        //        }

        //        Response.Cookies["email"].Value = tbxEmail.Text.Trim();
        //        Response.Cookies["Password"].Value = tbxPassword.Text.Trim();

        //        if (user_Details != null)
        //        {
        //            if ((user_Details.UserId != null) && (user_Details.UserId != ""))
        //            {
        //                Session["LoginId"] = user_Details.UserId;
        //                Session["UserDetail"] = user_Details;
        //                Session["Email"] = tbxEmail.Text.Trim();
        //                Session["UserRole"] = user_Details.UserType.ToLower().Trim();

        //                if (!objMyDBClass.GetUserIsActiveStatus(user_Details.UserId))
        //                {
        //                    Session["OnlineTestUser"] = user_Details.FullName;
        //                    Session["email"] = tbxEmail.Text.Trim();

        //                    Response.Redirect("UserDetails.aspx", true);
        //                }

        //                //Moving xsl files from C to UserDirectory if not exists 
        //                try
        //                {
        //                    int requireUpdation = objMyDBClass.CheckOperationalFiles_Updation();

        //                    int insertedRow = 0;

        //                    string userDir = Common.GetDirectoryPath() + "User Files/" + tbxEmail.Text.Trim() + "/XSL";

        //                    if (!Directory.Exists(userDir))
        //                    {
        //                        insertedRow = objMyDBClass.InsertOperationalFiles(user_Details.UserId);
        //                    }

        //                    string orignalDir = Common.GetXSLSourcePath();

        //                    if (insertedRow > 0)
        //                    {
        //                        if (!Directory.Exists(userDir))
        //                            Directory.CreateDirectory(userDir);

        //                        DirectoryInfo dInfo = CopyTo(new DirectoryInfo(orignalDir), userDir, true);
        //                    }
        //                    else if (requireUpdation > 0)
        //                    {
        //                        if (Directory.Exists(userDir))
        //                            Directory.Delete(userDir, true);

        //                        Directory.CreateDirectory(userDir);

        //                        DirectoryInfo dInfo = CopyTo(new DirectoryInfo(orignalDir), userDir, true);
        //                    }
        //                }
        //                catch (Exception)
        //                {
        //                    ucShowMessage1.ShowMessage(MessageTypes.Error, "Sorry! Some error has occured.");
        //                    return;
        //                }
        //                //end moving files

        //                if ((user_Details.UserType.ToLower().Trim() == "1") || (user_Details.UserType.ToLower().Trim() == "2"))
        //                {
        //                    Response.Redirect("OnlineTestUser.aspx", true);
        //                }
        //                //else if (user_Details.UserType.ToLower().Trim() == "7")
        //                //{
        //                //    Response.Redirect("OnlineTestAdmin.aspx?UserId=" + user_Details.UserId);
        //                //}

        //                //Admin user create tagging/untagging tasks
        //                else if (user_Details.UserType.ToLower().Trim() == "5")
        //                {
        //                    string id = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxEmail.Text.Trim()));
        //                    string pass = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxPassword.Text.Trim()));
        //                    string type = HttpUtility.UrlEncode(CommonClass.Encrypt(user_Details.UserType.Trim()));

        //                    Response.Redirect(string.Format("AdminPanel.aspx?id={1}&p={2}&t={3}", Request.Url.Host, id, pass, type), true);
        //                }

        //                //teamlead user perform mapping
        //                else if (user_Details.UserType.ToLower().Trim() == "6" || user_Details.UserType.ToLower().Trim() == "7")
        //                {
        //                    string id = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxEmail.Text.Trim()));
        //                    string pass = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxPassword.Text.Trim()));
        //                    string type = HttpUtility.UrlEncode(CommonClass.Encrypt(user_Details.UserType.Trim()));

        //                    Response.Redirect(string.Format("AdminPanel.aspx?id={1}&p={2}&t={3}", Request.Url.Host, id, pass, type), true);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            Response.Redirect("BookMicro.aspx");
        //        }
        //    }
        //}


        protected void lbtnDownloadPdf_Click(object sender, EventArgs e)
        {
            Response.ContentType = "Application/pdf";
            Response.AppendHeader("Content-Disposition", "attachment; filename=help.pdf");
            Response.TransmitFile(Server.MapPath("~/Document/book micro.pdf"));
            Response.End();
        }

        #endregion
       
    }
}
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;
using System.Text;
using Outsourcing_System.MasterPages;

namespace Outsourcing_System
{
    public partial class Failed : System.Web.UI.Page
    {
        GlobalVar objGlobal = new GlobalVar();
        MyDBClass objMyDBClass = new MyDBClass();
        ConversionClass objConversionClass = new ConversionClass();

        protected void Page_Load(object sender, EventArgs e)
        {
            ((UserMaster)this.Page.Master).HideLogInButton();

            string userName = "";
            string email = "";
            string totalMarks = "1";
            string obtained = "";
            string testName = "";

            if (!Page.IsPostBack)
            {
                string testType = Convert.ToString(Request.QueryString["type"]);
                double per = Convert.ToDouble(Request.QueryString["p"]);
                string userId = Convert.ToString(Request.QueryString["userId"]);
                string tName = Convert.ToString(Request.QueryString["t"]);

                if (testType != null)
                {
                    if (testType.Equals("test"))
                    {
                        GetComparisonTestResult(per, userId, tName);
                    }
                }
                else
                {
                    if (Session["Result"] == null)
                    {
                        Response.Redirect("Bookmicro.aspx", true);
                    }

                    //If user click finish test
                    if (Convert.ToString(Session["Result"]) != "")
                    {
                        obtained = Convert.ToString(Session["Result"]);
                    }

                    //If time is up
                    else
                    {
                        string temp = GenerateTestReport();
                        var result = temp.Split(',');
                        obtained = Convert.ToString(result[0]);

                    }

                    MyDBClass objMyDBClass = new MyDBClass();
                    if (Session["OnlineTestUser"] != null)
                    {

                        totalMarks = Convert.ToString(objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineTestUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"])));
                    }
                    else if (Session["OnlineUser"] != null)
                    {
                        totalMarks = Convert.ToString(objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"])));
                    }

                    double percentage = Convert.ToDouble(obtained.Trim()) / Convert.ToDouble(totalMarks.Trim()) * 100;

                    lbl.Attributes.Add("data-percent", Convert.ToString(percentage));

                    if ((Convert.ToString(Session["OnlineTestUser"]) != null) && (Convert.ToString(Session["OnlineTestUser"]) != ""))
                    {
                        userName = Convert.ToString(Session["OnlineTestUser"]);
                    }
                    else
                    {
                        userName = Session["OnlineUser"].ToString();
                    }
                    if (Session["email"].ToString() != "")
                    {
                        email = Convert.ToString(Session["email"]);
                    }
                    if (Session["TestName"].ToString() != "")
                    {
                        testName = Convert.ToString(Session["TestName"]);
                    }

                    string value = objMyDBClass.SaveResult(userName, email, testName, obtained, "failed");
                    //This session clear was causing log out of register user, i.e. when someone atemps mapping test on fail it was logout. while it should be redirected to onlinetestuser page.
                    // Session.Clear();
                }
            }
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

        protected void imgbtnLogin_Click(object sender, System.EventArgs e)
        {
            MyDBClass objMyDBClass = new MyDBClass();

            Session.Clear();

            if (Convert.ToString(Session["LoginId"]) == "")
            {
                if (cbxRememberMe.Checked) //If user checks keep me signed in
                {
                    Response.Cookies["email"].Expires = DateTime.Now.AddDays(1);
                    Response.Cookies["Password"].Expires = DateTime.Now.AddDays(1);
                }
                //else //If next time user don't want to save password
                //{
                //    if (Request.Cookies["UserName"] != null && Request.Cookies["Password"] != null)
                //    {
                //        Response.Cookies["UserName"].Expires = DateTime.Now;
                //        Response.Cookies["Password"].Expires = DateTime.Now;
                //        tbxLogin.Text = "";
                //        tbxPassword.Text = "";
                //    }
                //}

                TestUser user_Details = objMyDBClass.Validate_User(Convert.ToString(tbxEmail.Text), Convert.ToString(tbxPassword.Text));

                if (user_Details == null)
                {
                    showMessage("The email or password you entered is incorrect. Please try again (make sure your caps lock is off).");
                    return;
                }

                Response.Cookies["email"].Value = tbxEmail.Text.Trim();
                Response.Cookies["Password"].Value = tbxPassword.Text.Trim();

                if (user_Details != null)
                {
                    if ((user_Details.UserId != null) && (user_Details.UserId != ""))
                    {
                        Session["LoginId"] = user_Details.UserId;
                        Session["UserDetail"] = user_Details;
                        Session["Email"] = tbxEmail.Text.Trim();

                        //Moving xsl files from C to UserDirectory if not exists 
                        try
                        {
                            int requireUpdation = objMyDBClass.CheckOperationalFiles_Updation();

                            int insertedRow = 0;

                            string userDir = Common.GetDirectoryPath() + "User Files/" + tbxEmail.Text.Trim() + "/XSL";

                            if (!Directory.Exists(userDir))
                            {
                                insertedRow = objMyDBClass.InsertOperationalFiles(user_Details.UserId);
                            }

                            string orignalDir = Common.GetXSLSourcePath();

                            if (insertedRow > 0)
                            {
                                if (!Directory.Exists(userDir))
                                    Directory.CreateDirectory(userDir);

                                DirectoryInfo dInfo = CopyTo(new DirectoryInfo(orignalDir), userDir, true);
                            }
                            else if (requireUpdation > 0)
                            {
                                if (Directory.Exists(userDir))
                                    Directory.Delete(userDir, true);

                                Directory.CreateDirectory(userDir);

                                DirectoryInfo dInfo = CopyTo(new DirectoryInfo(orignalDir), userDir, true);
                            }
                        }
                        catch (Exception)
                        {
                            showMessage("Some error occurs while moving operational files to user directory.");
                            return;
                        }
                        //end moving files

                        if ((user_Details.UserType.ToLower().Trim() == "1") || (user_Details.UserType.ToLower().Trim() == "2"))
                        {
                            Response.Redirect("OnlineTestUser.aspx");
                        }
                        else if (user_Details.UserType.ToLower().Trim() == "7")
                        {
                            Response.Redirect("OnlineTestAdmin.aspx?UserId=" + user_Details.UserId);
                        }

                        //Admin user create tagging/untagging tasks
                        else if (user_Details.UserType.ToLower().Trim() == "5")
                        {
                            string id = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxEmail.Text.Trim()));
                            string pass = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxPassword.Text.Trim()));
                            string type = HttpUtility.UrlEncode(CommonClass.Encrypt(user_Details.UserType.Trim()));

                            Response.Redirect(string.Format("AdminPanel.aspx?id={1}&p={2}&t={3}", Request.Url.Host, id, pass, type), true);
                        }

                        //teamlead user perform mapping
                        else if (user_Details.UserType.ToLower().Trim() == "6")
                        {
                            string id = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxEmail.Text.Trim()));
                            string pass = HttpUtility.UrlEncode(CommonClass.Encrypt(tbxPassword.Text.Trim()));
                            string type = HttpUtility.UrlEncode(CommonClass.Encrypt(user_Details.UserType.Trim()));

                            Response.Redirect(string.Format("AdminPanel.aspx?id={1}&p={2}&t={3}", Request.Url.Host, id, pass, type), true);
                        }
                    }
                }
                else
                {
                    Response.Redirect("BookMicro.aspx");
                }
            }
        }

        private void showMessage(string message)
        {
            //if (message != "")
            //{
            //    DivError.Visible = true;
            //    lblError.Text = message;
            //}
            //else
            //{
            //    DivError.Visible = false;
            //}
        }
        public void GetComparisonTestResult(double percentage, string userId, string testName)
        {
            lbl.Attributes.Add("data-percent", Convert.ToString(percentage));

            //string value = objMyDBClass.SaveResult(userName, email, testName, obtained, "failed");
        }

        private string GenerateTestReport()
        {
            loadLatestXml(Convert.ToString(Session["email"]));
            int total = objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"]));
            XmlNodeList effectedNodes = objGlobal.PBPDocument.SelectNodes(@"//ln[@conversion]");
            foreach (XmlNode node in effectedNodes)
            {
                string[] paraInfo = node.Attributes["conversion"].Value.Split(':');
                XmlNode paraNode = node.SelectSingleNode(@"ancestor::upara|ancestor::spara|ancestor::npara");
                switch (paraNode.Name)
                {
                    case "spara":
                        string lineorPara = paraNode.ChildNodes[0].Name;
                        string hAlign = paraNode.Attributes["h-align"] == null ? "" : paraNode.Attributes["h-align"].Value;
                        string type = paraNode.Attributes["type"] == null ? "" : paraNode.Attributes["type"].Value;

                        if (paraInfo[0].Equals("spara") && paraInfo[1].Equals(type) && paraInfo[2].Equals(lineorPara))
                        {
                            if (!paraInfo[1].Equals("other"))
                            {
                                node.Attributes.RemoveNamedItem("conversion");
                            }
                            else if (paraInfo[3].Equals(hAlign))
                            {
                                node.Attributes.RemoveNamedItem("conversion");
                            }
                        }
                        break;
                    case "npara":
                        if (paraInfo[0].Equals("npara"))
                        {
                            node.Attributes.RemoveNamedItem("conversion");
                        }
                        break;
                    case "upara":
                        if (paraInfo[0].Equals("upara"))
                        {
                            node.Attributes.RemoveNamedItem("conversion");
                        }
                        break;
                    default:
                        break;
                }
            }
            XmlNodeList inBoxLines = objGlobal.PBPDocument.SelectNodes(@"//ln[@inbox]");
            foreach (XmlNode node in effectedNodes)
            {
                XmlNode boxNode = node.SelectSingleNode(@"ancestor::box");
                if (boxNode != null)
                {
                    node.Attributes.RemoveNamedItem("inbox");
                }
            }

            XmlNodeList editingLeftNodes = objGlobal.PBPDocument.SelectNodes(@"//*[@correction]");
            foreach (XmlNode node in editingLeftNodes)
            {
                if (node.Attributes["correction"].Value.Equals(""))
                {
                    node.Attributes.Remove(node.Attributes["correction"]);
                }
            }
            editingLeftNodes = objGlobal.PBPDocument.SelectNodes(@"//*[@correction]");

            XmlNodeList notConvertedNodes = objGlobal.PBPDocument.SelectNodes(@"//ln[@conversion]");
            XmlNodeList notInbox = objGlobal.PBPDocument.SelectNodes(@"//ln[@inbox]");

            StringBuilder sr = new StringBuilder();

            if (total > 0)
            {
                int corrected = total - notConvertedNodes.Count - notInbox.Count - editingLeftNodes.Count;
                if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
                {
                    objGlobal.XMLPath = Session["XMLPath"].ToString();
                }
                if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
                {
                    objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
                }
                objGlobal.SaveXml();
                // sr.Append("You Have Corrected " + Convert.ToString(totalOccurences - remainingOccurences) + " out of toalt " + remainingOccurences + " Mistakes");
                sr.Append(corrected + ", " + total);
            }
            else
            {
                sr.Append("0, " + total);
            }

            return sr.ToString();
        }
        private void loadLatestXml(string email)
        {

            string xmlFile = "";
            if (email != "")
            {
                xmlFile = objMyDBClass.MainDirPhyPath + "/Tests/" + email + "/" + Convert.ToString(Session["TestName"]) + "/" + Convert.ToString(Session["TestName"]) + ".rhyw";
            }
            else
            {
                xmlFile = objMyDBClass.MainDirPhyPath + "/" + Convert.ToString(Session["TestName"]) + "/" + Convert.ToString(Session["TestName"]) + ".rhyw";
            }
            objGlobal.XMLPath = xmlFile;
            Session["XMLPath"] = objGlobal.XMLPath;
            objGlobal.PBPDocument = new System.Xml.XmlDocument();
            objGlobal.LoadXml();
            Session["PBPDocument"] = objGlobal.PBPDocument;
        }
    }
}
















//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.Web.UI;
//using System.Web.UI.WebControls;
//using System.Xml;
//using System.Text;
//using Outsourcing_System.MasterPages;

//namespace Outsourcing_System
//{
//    public partial class Step3 : System.Web.UI.Page
//    {
//        GlobalVar objGlobal = new GlobalVar();
//        MyDBClass objMyDBClass = new MyDBClass();
//        ConversionClass objConversionClass = new ConversionClass();

//        protected void Page_Load(object sender, EventArgs e)
//        {
//            string userName = "";
//            string email = "";
//            string totalMarks = "1";
//            string obtained = "";
//            string testName = "";

//            if (!Page.IsPostBack)
//            {

//                //((UserMaster)this.Page.Master).HideLogInButton();
//                //((UserMaster)this.Page.Master).SetMenuLocation = "-120px";

//                string testType = Convert.ToString(Request.QueryString["type"]);
//                double per = Convert.ToDouble(Request.QueryString["p"]);
//                string userId = Convert.ToString(Request.QueryString["userId"]);
//                string tName = Convert.ToString(Request.QueryString["t"]);

//                if (testType != null)
//                {
//                    if (testType.Equals("test"))
//                    {
//                        GetComparisonTestResult(per, userId, tName);
//                    }
//                }
//                else
//                {
//                    if (Session["Result"] == null)
//                    {
//                        Response.Redirect("Bookmicro.aspx", true);
//                    }

//                    //If user click finish test
//                    if (Convert.ToString(Session["Result"]) != "")
//                    {
//                        obtained = Convert.ToString(Session["Result"]);
//                    }

//                    //If time is up
//                    else
//                    {
//                        string temp = GenerateTestReport();
//                        var result = temp.Split(',');
//                        obtained = Convert.ToString(result[0]);
//                    }

//                    MyDBClass objMyDBClass = new MyDBClass();
//                    if (Session["OnlineTestUser"] != null)
//                    {
//                        totalMarks = Convert.ToString(objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineTestUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"])));
//                    }
//                    else if (Session["OnlineUser"] != null)
//                    {
//                        totalMarks = Convert.ToString(objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"])));
//                    }

//                    double percentage = Convert.ToDouble(obtained.Trim()) / Convert.ToDouble(totalMarks.Trim()) * 100;

//                    lbl.Attributes.Add("data-percent", Convert.ToString(percentage));

//                    if ((Convert.ToString(Session["OnlineTestUser"]) != null) && (Convert.ToString(Session["OnlineTestUser"]) != ""))
//                    {
//                        userName = Convert.ToString(Session["OnlineTestUser"]);
//                    }
//                    else
//                    {
//                        userName = Session["OnlineUser"].ToString();
//                    }
//                    if (Session["email"].ToString() != "")
//                    {
//                        email = Convert.ToString(Session["email"]);
//                    }
//                    if (Session["TestName"].ToString() != "")
//                    {
//                        testName = Convert.ToString(Session["TestName"]);
//                    }

//                    string value = objMyDBClass.SaveResult(userName, email, testName, obtained, "failed");
//                    //This session clear was causing log out of register user, ie when someone atemps mapping test on fail it was logout. while it should be redirected to onlinetestuser page.
//                   // Session.Clear();
//                }
//            }
//        }

       

//        public void GetComparisonTestResult(double percentage, string userId, string testName)
//        {
//            lbl.Attributes.Add("data-percent", Convert.ToString(percentage));

//            //string value = objMyDBClass.SaveResult(userName, email, testName, obtained, "failed");
//        }

//        private string GenerateTestReport()
//        {
//            loadLatestXml(Convert.ToString(Session["email"]));
//            int total = objMyDBClass.GetTotalScore(Convert.ToString(Session["OnlineUser"]), Convert.ToString(Session["email"]), Convert.ToString(Session["TestName"]));
//            XmlNodeList effectedNodes = objGlobal.PBPDocument.SelectNodes(@"//ln[@conversion]");
//            foreach (XmlNode node in effectedNodes)
//            {
//                string[] paraInfo = node.Attributes["conversion"].Value.Split(':');
//                XmlNode paraNode = node.SelectSingleNode(@"ancestor::upara|ancestor::spara|ancestor::npara");
//                switch (paraNode.Name)
//                {
//                    case "spara":
//                        string lineorPara = paraNode.ChildNodes[0].Name;
//                        string hAlign = paraNode.Attributes["h-align"] == null ? "" : paraNode.Attributes["h-align"].Value;
//                        string type = paraNode.Attributes["type"] == null ? "" : paraNode.Attributes["type"].Value;

//                        if (paraInfo[0].Equals("spara") && paraInfo[1].Equals(type) && paraInfo[2].Equals(lineorPara))
//                        {
//                            if (!paraInfo[1].Equals("other"))
//                            {
//                                node.Attributes.RemoveNamedItem("conversion");
//                            }
//                            else if (paraInfo[3].Equals(hAlign))
//                            {
//                                node.Attributes.RemoveNamedItem("conversion");
//                            }
//                        }
//                        break;
//                    case "npara":
//                        if (paraInfo[0].Equals("npara"))
//                        {
//                            node.Attributes.RemoveNamedItem("conversion");
//                        }
//                        break;
//                    case "upara":
//                        if (paraInfo[0].Equals("upara"))
//                        {
//                            node.Attributes.RemoveNamedItem("conversion");
//                        }
//                        break;
//                    default:
//                        break;
//                }
//            }
//            XmlNodeList inBoxLines = objGlobal.PBPDocument.SelectNodes(@"//ln[@inbox]");
//            foreach (XmlNode node in effectedNodes)
//            {
//                XmlNode boxNode = node.SelectSingleNode(@"ancestor::box");
//                if (boxNode != null)
//                {
//                    node.Attributes.RemoveNamedItem("inbox");
//                }
//            }

//            XmlNodeList editingLeftNodes = objGlobal.PBPDocument.SelectNodes(@"//*[@correction]");
//            foreach (XmlNode node in editingLeftNodes)
//            {
//                if (node.Attributes["correction"].Value.Equals(""))
//                {
//                    node.Attributes.Remove(node.Attributes["correction"]);
//                }
//            }
//            editingLeftNodes = objGlobal.PBPDocument.SelectNodes(@"//*[@correction]");

//            XmlNodeList notConvertedNodes = objGlobal.PBPDocument.SelectNodes(@"//ln[@conversion]");
//            XmlNodeList notInbox = objGlobal.PBPDocument.SelectNodes(@"//ln[@inbox]");

//            StringBuilder sr = new StringBuilder();

//            if (total > 0)
//            {
//                int corrected = total - notConvertedNodes.Count - notInbox.Count - editingLeftNodes.Count;
//                if (objGlobal.XMLPath == null && (Session["XMLPath"] != null))
//                {
//                    objGlobal.XMLPath = Session["XMLPath"].ToString();
//                }
//                if (objGlobal.PBPDocument == null && (Session["PBPDocument"] != null))
//                {
//                    objGlobal.PBPDocument = Session["PBPDocument"] as XmlDocument;
//                }
//                objGlobal.SaveXml();
//                // sr.Append("You Have Corrected " + Convert.ToString(totalOccurences - remainingOccurences) + " out of toalt " + remainingOccurences + " Mistakes");
//                sr.Append(corrected + ", " + total);
//            }
//            else
//            {
//                sr.Append("0, " + total);
//            }

//            return sr.ToString();
//        }
//        private void loadLatestXml(string email)
//        {

//            string xmlFile = "";
//            if (email != "")
//            {
//                xmlFile = objMyDBClass.MainDirPhyPath + "/Tests/" + email + "/" + Convert.ToString(Session["TestName"]) + "/" + Convert.ToString(Session["TestName"]) + ".rhyw";
//            }
//            else
//            {
//                xmlFile = objMyDBClass.MainDirPhyPath + "/" + Convert.ToString(Session["TestName"]) + "/" + Convert.ToString(Session["TestName"]) + ".rhyw";
//            }
//            objGlobal.XMLPath = xmlFile;
//            Session["XMLPath"] = objGlobal.XMLPath;
//            objGlobal.PBPDocument = new System.Xml.XmlDocument();
//            objGlobal.LoadXml();
//            Session["PBPDocument"] = objGlobal.PBPDocument;
//        }
//    }
//}
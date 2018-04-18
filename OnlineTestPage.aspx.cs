using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;

namespace Outsourcing_System
{
    public partial class OnlineTestPage : System.Web.UI.Page
    {
        #region |Fields and Properties|


        GlobalVar objGlobal = new GlobalVar();
        MyDBClass objMyDBClass = new MyDBClass();

        #endregion
        #region |Events|

        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void btnTest_Click(object sender, EventArgs e)
        {
            string username = txtName.Text.Trim();
            int rndNumber = 224;
            string orignalDir = objMyDBClass.MainDirPhyPath + "\\Tests\\Original\\" + rndNumber.ToString();
            string userDir = objMyDBClass.MainDirPhyPath + "\\Tests\\" + username + "/" + rndNumber;
            if (!Directory.Exists(userDir))
            {
                Directory.CreateDirectory(userDir);
                DirectoryInfo dInfo = CopyTo(new DirectoryInfo(orignalDir), userDir);
                File.Delete(userDir + "\\" + rndNumber + ".rhyw");
                File.Copy(userDir + "\\" + rndNumber + "-1" + "\\TaggingUntagged\\" + rndNumber + "-1.rhyw", userDir + "\\" + rndNumber + ".rhyw");
            }

           


            Response.Redirect("Step2.aspx?username=" + username + "&bid=" + rndNumber, false);

            //ShowPDF();
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


        #endregion
    }
}
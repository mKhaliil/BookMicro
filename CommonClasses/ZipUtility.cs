using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
using System.Data.OleDb;

namespace Outsourcing_System
{

    public class ZipUtility
    {
        private string XML_File;
        private string Book_Resources;
        private string dirToZip;
        private string unzipPath;

        public ZipUtility()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        //getters
        public string getXML_File()
        {
            return this.XML_File;
        }

        //setters
        public void setdirToZip(string prmDirToZip)
        {
            this.dirToZip = prmDirToZip;
        }

        public void setunzipPath(string prmunzipPath)
        {
            this.unzipPath = prmunzipPath;
        }

        //
        public int CountTableSheets(string ZipFilePath)
        {
            string[] XmlBookSources = new string[2];
            string TempLocation = ZipFilePath.Replace(Path.GetExtension(ZipFilePath),"");
            int tables = 0;
            try
            {
                System.IO.Stream ZipStream = CreateIOStreamFromZip(ZipFilePath);
                
                Zip saZipAgent = new Zip();
                if (saZipAgent.extractAndSaveFile(TempLocation+"/", ZipStream))
                {
                    string path = ""; 
                    bool excelFound=false;
                    
                    if (Directory.GetFiles(TempLocation,"*.xlsx").Length > 0)
                    {
                        path =  Directory.GetFiles(TempLocation, "*.xlsx")[0];
                        excelFound=true;
                    }
                    else if (Directory.GetFiles(TempLocation, "*.xls").Length > 0)
                    {
                        path =  Directory.GetFiles(TempLocation, "*.xls")[0];
                        excelFound=true;
                    }
                    if(excelFound)
                    {
                    string[] sheets = GetExcelSheetNames(path);
                    tables = sheets.Length;
                    }

                }
                if (Directory.Exists(TempLocation))
                {
                    Directory.Delete(TempLocation, true);
                }

            }
            catch (System.Exception E)
            {
                throw new Exception(E.Message);
            }
            return tables;
        }
        public int CountImages(string ZipFilePath)
        {
            string[] XmlBookSources = new string[2];

            try
            {
                System.IO.Stream ZipStream = CreateIOStreamFromZip(ZipFilePath);
                Zip saZipAgent = new Zip();
                return saZipAgent.extractNoofImages(this.unzipPath, ZipStream);

            }
            catch (System.Exception E)
            {
                throw new Exception(E.Message);
            }
            
        }
        public void ExtractZipFile(string ZipFilePath)
        {
            string[] XmlBookSources = new string[2];

            try
            {
                System.IO.Stream ZipStream = CreateIOStreamFromZip(ZipFilePath);

                Zip saZipAgent = new Zip();
                saZipAgent.extractFile(ZipStream, this.unzipPath, false);
                XML_File = saZipAgent.BookXMLFile;
                Book_Resources = saZipAgent.BookResources;
            }
            catch (System.Exception E)
            {
                //throw new Exception(E.Message);
            }
        }

        private System.IO.Stream CreateIOStreamFromZip(string FilePath)
        {
            System.IO.Stream IoStream = null;

            try
            {
                System.IO.FileStream IoFileStream = new System.IO.FileStream(FilePath, System.IO.FileMode.Open);
                IoStream = (System.IO.Stream)IoFileStream;
            }
            catch (System.Exception E)
            {
                throw new Exception(E.Message);
            }

            return IoStream;
        }

        public void ZIPDirectory(string path)
        {
            string[] filesList = System.IO.Directory.GetFiles(this.dirToZip);
            if (filesList.Length == 0)
            {
                throw new Exception("No files to zip at location " + this.dirToZip);
            }
            // Now create a new zip stream
            string strOutputFileName = this.dirToZip + ".zip";
            ZipOutputStream zippedDTB = new ZipOutputStream(File.Create(strOutputFileName)); // create a temp zip file at this path

            zippedDTB.SetLevel(9); // 9- Best Compression

            System.IO.FileStream inputFile = null;
            for (int fileNumber = 0; fileNumber < filesList.Length; fileNumber++)
            {
                if (Path.GetExtension(filesList[fileNumber]) == ".rhyw" || Path.GetExtension(filesList[fileNumber]) == ".pdf")
                {
                    if (Path.GetExtension(filesList[fileNumber]) == ".pdf")
                    {
                        string mainBook = Path.GetFileNameWithoutExtension(filesList[fileNumber]);
                        for (int j = 1; j < 4; j++)
                        {
                            string tableZip = "\\" + mainBook + "\\" + mainBook + "-" + j + "\\Table\\" + mainBook + "-" + j + ".zip";
                            if (File.Exists(path + tableZip))
                            {
                                TableIndecies(zippedDTB, path + tableZip);
                            }
                        }
                        for (int j = 1; j < 4; j++)
                        {
                            string indexZip = "\\" + mainBook + "\\" + mainBook + "-" + j + "\\Index\\" + mainBook + "-" + j + ".xls";
                            if (File.Exists(path + indexZip))
                            {
                                TableIndecies(zippedDTB, path + indexZip);
                            }
                        }
                    }
                    inputFile = File.OpenRead(filesList[fileNumber]);
                    byte[] fileData = new byte[inputFile.Length];
                    string fileName = filesList[fileNumber].Substring(filesList[fileNumber].LastIndexOf("\\") + 1);
                    ZipEntry entry = new ZipEntry(fileName);
                    zippedDTB.PutNextEntry(entry);
                    inputFile.Read(fileData, 0, fileData.Length);
                    zippedDTB.Write(fileData, 0, fileData.Length);
                    inputFile.Close(); inputFile = null;
                }
            }

            //Adds sub directories
            string[] dirsList = Directory.GetDirectories(this.dirToZip);
            foreach (string dir in dirsList)
            {
                if (dir.EndsWith("Resources"))
                {
                    AddDirToZip(zippedDTB, dir);
                }
            }

            zippedDTB.Finish();
            zippedDTB.Close();

        }
        public void TableIndecies(ZipOutputStream zippedDTB, string subFile)
        {
            FileStream inputFile = null;
            try
            {
                inputFile = File.OpenRead(subFile);
                byte[] fileData = new byte[inputFile.Length];
                string fileName = subFile.Substring(subFile.LastIndexOf("\\") + 1);
                ZipEntry entry = new ZipEntry(fileName);
                zippedDTB.PutNextEntry(entry);
                inputFile.Read(fileData, 0, fileData.Length);
                zippedDTB.Write(fileData, 0, fileData.Length);
            }
            finally
            {
                inputFile.Close(); inputFile = null;
            }
        }

        private bool AddDirToZip(ZipOutputStream zipFile, string path)
        {
            // Add a sub directory with in this zip folder and add files to that directory
            string[] filesList = System.IO.Directory.GetFiles(path);
            string directoryName = path.Substring(path.LastIndexOf("\\") + 1);
            foreach (string file in filesList)
            {
                System.IO.FileStream inputFile = File.OpenRead(file);
                byte[] fileData = new byte[inputFile.Length];
                string fileName = file.Substring(file.LastIndexOf("\\") + 1);
                ZipEntry entry = new ZipEntry(directoryName + "/" + fileName);
                zipFile.PutNextEntry(entry);
                inputFile.Read(fileData, 0, fileData.Length);
                zipFile.Write(fileData, 0, fileData.Length);
                inputFile.Close(); inputFile = null;

            }
            return true;
        }
        private String[] GetExcelSheetNames(string excelFile)
        {
            OleDbConnection objConn = null;
            System.Data.DataTable dt = null;

            try
            {
                // Connection String. Change the excel file to the file you
                // will search.
                String connString = "Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + excelFile + ";Extended Properties=Excel 12.0;";
                // Create connection object by using the preceding connection string.
                objConn = new OleDbConnection(connString);
                // Open connection with the database.
                objConn.Open();
                // Get the data table containg the schema guid.
                dt = objConn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

                if (dt == null)
                {
                    return null;
                }

                String[] excelSheets = new String[dt.Rows.Count];
                int i = 0;

                // Add the sheet name to the string array.
                foreach (DataRow row in dt.Rows)
                {
                    excelSheets[i] = row["TABLE_NAME"].ToString();
                    i++;
                }

                // Loop through all of the sheets if you want too...
                for (int j = 0; j < excelSheets.Length; j++)
                {
                    // Query each excel sheet.
                }

                return excelSheets;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                // Clean up.
                if (objConn != null)
                {
                    objConn.Close();
                    objConn.Dispose();
                }
                if (dt != null)
                {
                    dt.Dispose();
                }
            }
        }
    }
}

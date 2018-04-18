using System;
using System.IO;
using ICSharpCode.SharpZipLib.Zip;
// Need these to enable logging by ZIP DLL
//using Microsoft.ApplicationBlocks.Logging.Schema;
//using Microsoft.EnterpriseInstrumentation;


namespace Outsourcing_System
{
    /// <summary>
    /// Summary description for Class1.
    /// </summary>
    public class Zip
    {
        private string xmlFile;
        private string ResourceFolderPath;
        private int LogEventNumber;

        public Zip()
        {
            LogEventNumber = 0;
            xmlFile = "";
            ResourceFolderPath = "";
        }

        /// <summary>
        /// It will be extracting the input file at teh specifield location
        /// </summary>
        /// <param name="zipFile">The input zip file stream</param>
        /// <param name="location">The output location. The extracted files will be placed in this location</param>
        /// <param name="createNew">Creates a new directory. If one exists over writes it</param>
        /// <returns></returns>
        public bool extractFile(Stream zipFile, string location, bool createNew)
        {
            //AuditEvent(LogLevel.Informational, "Target Location = \"" + location + "\" AND Create New " + createNew.ToString() );
            // Create a new directory overwritting existing one
            if (createNew)
            {
                try
                {
                    //					AuditEvent (LogLevel.Informational, "Trying to create \"" + location + "\"");
                    if (Directory.Exists(location))
                    {
                        //						AuditEvent (LogLevel.Informational, "Found and Trying to Delete \"" + location + "\"");
                        Directory.Delete(location, true);
                        //						if (Directory.Exists(location))
                        //							AuditEvent (LogLevel.Error, "Could not Delete \"" + location + "\"");
                        //						else
                        //							AuditEvent (LogLevel.Informational, "Completed Delete \"" + location + "\"");
                    }
                    DirectoryInfo LocInfo = Directory.CreateDirectory(location);
                    //if (LocInfo == null) 
                    //AuditEvent (LogLevel.Error, "Could not Re-Create \"" + location + "\"");
                }
                catch (System.Exception ex)
                {
                    //AuditEvent (LogLevel.Error, ex.Message);
                    throw ex;
                }
            }
            // use the existing or created directory

            if (!extractAndSaveFile(location, zipFile))
                return false;

            // Get the book xml file from the specified folder
            if (Directory.GetFiles(location).Length == 1)
            {// There is only one file in zip folder
                xmlFile = Directory.GetFiles(location)[0];
            }
            else
            {// There are more than one files. Only use the xml file
                xmlFile = Directory.GetFiles(location, "*.xml")[0];
            }

            // Get the folder in the location just created assume this is resources folder
            if (Directory.GetDirectories(location).Length != 0)
            {
                ResourceFolderPath = Directory.GetDirectories(location)[0];
            }

            return true;
        }


        /// <summary>
        /// This function zips the contents of specified folder, and (if mentioned)
        /// sub folders 
        /// </summary>
        /// <param name="location">The folder to zip</param>
        /// <param name="outputFileName">The fully qualified name of the output Zip file to be created</param>
        /// <param name="IncludeSubDirectories">True: if sub directories are required to
        /// be included in zip. Otherwise False.</param>
        /// <returns>True: if files are zipped. Otherwise false</returns>
        public bool zipFile(string location, string outputFileName, bool IncludeSubDirectories)
        {
            string[] filesList = System.IO.Directory.GetFiles(location);
            if (filesList.Length == 0)
            {
                throw new Exception("No files to zip at location " + location);
            }
            // Now create a new zip stream
            ZipOutputStream zippedDTB = new ZipOutputStream(File.Create(outputFileName)); // create a temp zip file at this path

            zippedDTB.SetLevel(9); // 9- Best Compression

            System.IO.FileStream inputFile = null;
            //for (int fileNumber = 0; fileNumber < filesList.Length; fileNumber++)
            //{
            //    inputFile = File.OpenRead(filesList[fileNumber]);
            //    byte[] fileData = new byte[inputFile.Length];
            //    string fileName = filesList[fileNumber].Substring(filesList[fileNumber].LastIndexOf("\\") + 1);
            //    ZipEntry entry = new ZipEntry(fileName);
            //    zippedDTB.PutNextEntry(entry);
            //    inputFile.Read(fileData, 0, fileData.Length);
            //    zippedDTB.Write(fileData, 0, fileData.Length);
            //    inputFile.Close(); inputFile = null;
            //}

            // If the sub directories are requried to be included 
            if (IncludeSubDirectories)
            {
                string[] dirsList = Directory.GetDirectories(location);//,"*",SearchOption.AllDirectories) ;
                foreach (string dir in dirsList)
                {
                    AddDirToZip(zippedDTB, dir);
                }
            }

            for (int fileNumber = 0; fileNumber < filesList.Length; fileNumber++)
            {
                inputFile = File.OpenRead(filesList[fileNumber]);
                byte[] fileData = new byte[inputFile.Length];
                string fileName = filesList[fileNumber].Substring(filesList[fileNumber].LastIndexOf("\\") + 1);
                ZipEntry entry = new ZipEntry(fileName);
                entry.Offset = 1;
                zippedDTB.PutNextEntry(entry);
                inputFile.Read(fileData, 0, fileData.Length);
                zippedDTB.Write(fileData, 0, fileData.Length);
                inputFile.Close(); inputFile = null;
            }
            zippedDTB.Finish(); zippedDTB.Close();

            return true;
        }


        /// <summary>
        /// A property to get the file path for the extracted xml file
        /// </summary>
        public string BookXMLFile
        {
            get
            {
                return xmlFile;
            }
        }


        /// <summary>
        /// A property to get the file path for the extracted xml file
        /// </summary>
        public string BookResources
        {
            get
            {
                return ResourceFolderPath;
            }
        }


        #region Helper Functions

        /// <summary>
        /// It extracts the zip file and saves contents to the location specified.
        /// </summary>
        /// <param name="extractPath"></param>
        /// <param name="zipFile"></param>
        /// <returns></returns>
        public int extractNoofImages(string extractPath, System.IO.Stream zipFile)
        {
            ZipInputStream StreamIn = null;
            ZipEntry ZippedItem = null;
            int NoofImages = 0;
            try
            {
                
                StreamIn = new ZipInputStream(zipFile);
                
                while ((ZippedItem = StreamIn.GetNextEntry()) != null)
                {
                    if (ZippedItem.Name.ToLower().Contains(".jpg") | ZippedItem.Name.ToLower().Contains(".tiff"))
                    {
                        NoofImages = NoofImages + 1;
                    }
                }
            }
            catch (System.Exception Ex)
            {
                throw new Exception("extractAndSaveFile threw exception " + Ex.Message);
            }
            if (StreamIn != null) StreamIn.Close();

            return NoofImages;
        }
        public bool extractAndSaveFile(string extractPath, System.IO.Stream zipFile)
        {
            // Instantiating Zip Input Stream Object and Zip Entry Object with NULL
            ZipInputStream StreamIn = null;
            ZipEntry ZippedItem = null;

            // Instantiating Compressed Item Name and Extract Name for the Item
            string ZipFileName = "";
            string ExtractFileName = "";

            int nIndexofLastBackSlash = extractPath.LastIndexOf("\\");
            string strBookId = extractPath.Substring(nIndexofLastBackSlash + 1);

            try
            {
                // Typecasting System.IO.Stream object to ZipInputStream object
                StreamIn = new ZipInputStream(zipFile);

                // while-ing through the Entries in the ZipInputStream
                while ((ZippedItem = StreamIn.GetNextEntry()) != null)
                {
                    ZipFileName = ZippedItem.Name.Replace(strBookId + @"\", "");
                    ZipFileName = ZippedItem.Name.Replace(strBookId + @"/", "");
                    //ZipFileName = System.Text.RegularExpressions.Regex.Replace (ZippedItem.Name.Trim(), @"/$", "");
                    ZipFileName = System.Text.RegularExpressions.Regex.Replace(ZipFileName.Trim(), @"/$", "");

                    //ZipFileName = System.Text.RegularExpressions.Regex.Replace (ZippedItem.Name.Trim(), @"^\\{1,}", "");
                    ZipFileName = System.Text.RegularExpressions.Regex.Replace(ZipFileName.Trim(), @"^\\{1,}", "");

                    if (System.Text.RegularExpressions.Regex.Matches(ZipFileName, "\\\\").Count > 0)
                    {
                        //ZipFileName = ZipFileName.Substring (ZipFileName.IndexOf ("\\"));
                        ZipFileName = Path.GetFileName(ZipFileName);
                    }
                    //else
                    ZipFileName = "\\" + ZipFileName;

                    ZipFileName = ZipFileName.Replace("/", "\\");
                    ExtractFileName = extractPath + ZipFileName;

                    #region Create Folders for Extraction
                    if (ZippedItem.IsDirectory)
                    {
                        CreateFolder(ExtractFileName);
                    }
                    else
                    {
                        if (System.Text.RegularExpressions.Regex.Matches(ZipFileName, "\\\\").Count > 0)
                            CreateFolder(ZipFileName.Split('\\'), extractPath);
                    }
                    #endregion

                    #region Extract Files
                    if (!ZippedItem.IsDirectory && Directory.Exists(ExtractFileName.Substring(0, ExtractFileName.LastIndexOf("\\"))))
                    {
                        FileStream FileWriter = null;
                        try
                        {
                            FileWriter = File.Create(ExtractFileName);
                            int ChunkSize = 2048;
                            byte[] ChunkData = new byte[ChunkSize];
                            #region Write to Disk
                            while (true)
                            {
                                ChunkSize = StreamIn.Read(ChunkData, 0, ChunkData.Length);
                                if (ChunkSize > 0)
                                    FileWriter.Write(ChunkData, 0, ChunkSize);
                                else
                                    break;
                            }
                            #endregion
                            FileWriter.Close();
                        }
                        catch (System.Exception Ex)
                        {
                            throw new Exception("[extractAndSaveFile/extractFile] \"" + ExtractFileName + "\" -> " + Ex.Message);
                        }
                        if (!File.Exists(ExtractFileName))
                        {
                            throw new Exception("[extractAndSaveFile/extractFile] \"" + ExtractFileName + "\" -> Failed. Unknown Reason");
                        }
                    }
                    #endregion
                }
            }
            catch (System.Exception Ex)
            {
                throw new Exception("extractAndSaveFile threw exception " + Ex.Message);
            }
            if (StreamIn != null) StreamIn.Close();

            return true;
        }

        /// <summary>
        /// Creates a folder with the Folder Path given
        /// </summary>
        /// <param name="FolderPath">string: Folder Path</param>
        private void CreateFolder(string FolderPath)
        {
            try
            {
                //				AuditEvent (LogLevel.Informational, "[CreateFolder] Trying to Create Folder \"" + FolderPath + "\"");
                if (!Directory.Exists(FolderPath))
                {
                    Directory.CreateDirectory(FolderPath);
                    //AuditEvent (LogLevel.Informational, "[CreateFolder] Folder Created!");
                }
                //				else
                //					AuditEvent (LogLevel.Informational, "[CreateFolder] Folder already Exists!");
            }
            catch (System.Exception Ex)
            {
                throw new Exception("[CreateFolder] \"" + FolderPath + "\" Generated Exception: " + Ex.Message);
            }
        }

        /// <summary>
        /// Creates a folder and sub folders with the Folders' Paths given in the Extract In Folder
        /// </summary>
        /// <param name="FolderPath">string: Folder Path</param>
        /// <param name="ExtractIn">string: Extract In Folder Path</param>
        private void CreateFolder(string[] Folders, string ExtractIn)
        {
            string FolderToExtract = ExtractIn;

            for (int index = 0; index < Folders.Length - 1; index++)
            {
                FolderToExtract += "\\" + Folders[index];
                CreateFolder(FolderToExtract);
            }
        }


        /// <summary>
        /// Adds one directory to the zip file
        /// </summary>
        /// <param name="zipFile">The zip</param>
        /// <param name="path">The input directory path</param>
        /// <returns></returns>
        private bool AddDirToZip(ZipOutputStream zipFile, string path)
        {// Add a sub directory with in this zip folder and add files to that directory
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
            string[] di = Directory.GetDirectories(path);
            foreach (string d in di)
            {
                string subDir = d.Substring(d.LastIndexOf("\\") + 1);
                AddDirToZip(zipFile, d, directoryName + "\\" + subDir);
            }
            return true;
        }

        private bool AddDirToZip(ZipOutputStream zipFile, string path, string parentDirectory)
        {// Add a sub directory with in this zip folder and add files to that directory
            string[] filesList = System.IO.Directory.GetFiles(path);
            //string directoryName = path.Substring(path.LastIndexOf("\\") + 1);
            string directoryName = parentDirectory;
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
            string[] di = Directory.GetDirectories(path);
            foreach (string d in di)
            {
                string subDir = d.Substring(d.LastIndexOf("\\") + 1);
                AddDirToZip(zipFile, d, directoryName + "\\" + subDir);
            }
            return true;
        }

        #endregion

        /// <summary>
        /// Raise the Audit/Error Message Event
        /// </summary>
        /// <param name="lglvlEventType">LogLevel</param>
        /// <param name="strDescription">string</param>
        //		public void AuditEvent ( LogLevel lglvlEventType, string strDescription ) 
        //		{
        //			System.Diagnostics.Debug.WriteLine (lglvlEventType.ToString() + "\t" + strDescription);
        //			LogEventNumber += 1;
        //			switch ( lglvlEventType ) 
        //			{
        //				case LogLevel.Informational : 
        //				{
        //					#region Audit Message Event
        //					AuditMessageEvent objAuditMsgEvent = new AuditMessageEvent();
        //					objAuditMsgEvent.Message = LogEventNumber.ToString() + "[ZipServiceAgent] " + strDescription;
        //					objAuditMsgEvent.EventPublishLogLevel = (int)LogLevel.Always;
        //					EventSource.Application.Raise ( objAuditMsgEvent );
        //					objAuditMsgEvent = null;
        //					break;
        //					#endregion
        //				}
        //				case LogLevel.Error : 
        //				{
        //					#region Error Message Event
        //					ErrorMessageEvent objErrorMsgEvent = new ErrorMessageEvent ();
        //					objErrorMsgEvent.Message = LogEventNumber.ToString() + "[ZipServiceAgent] " + strDescription;
        //					objErrorMsgEvent.EventPublishLogLevel = (int) LogLevel.Always;
        //					EventSource.Application.Raise ( objErrorMsgEvent );
        //					objErrorMsgEvent = null;
        //					break;
        //					#endregion
        //				}
        //			}
        //			
        //		}

    }
}


using System;
using System.IO;
namespace Outsourcing_System
{
    public class FileBackup
    {
        string lastBackupFile;
        int maxBackupFiles;
        string backupFolderPath;
        DirectoryInfo di;
        int currentFileNum;
        /// <summary>
        /// The name of the file, whose backup has to be taken
        /// </summary>
        string originalFile;

        /// <summary>
        /// The full path of the file, associated with this instance, whose backup has to be taken
        /// </summary>
        public string OriginalFile
        {
            get
            {
                return this.originalFile;
            }
        }
        public FileBackup(string fileName)
        {
            try
            {
                maxBackupFiles = int.Parse(System.Configuration.ConfigurationSettings.AppSettings["MaxBackupFiles"]);
            }
            catch
            {
                maxBackupFiles = 10;
            }
            if (!File.Exists(fileName))
            {
                throw new FileNotFoundException();
            }
            else
            {
                this.originalFile = fileName;
            }
            string parentDirectory = Directory.GetParent(fileName).ToString();
            this.backupFolderPath = parentDirectory + "\\Backup";
            try
            {
                di = new DirectoryInfo(backupFolderPath);
            }
            catch
            {
                //MessageBox.Show("Cannot create backup directory");
                //Ask user to specify a backup folder here
            }
            if (!di.Exists)
            {
                try
                {
                    di.Create();
                }
                catch (UnauthorizedAccessException uae)
                {
                    //MessageBox.Show(uae.Message);
                }
                catch
                {
                    //MessageBox.Show("Cannot create backup directory");
                    //Ask user to specify a backup folder here
                }
            }
            else
            {
                FileInfo[] rhywFiles = di.GetFiles("*.rhyw");
                if (rhywFiles.Length < this.maxBackupFiles)
                {
                    int fileNo = GetMostRecentBackupFileNum(di.FullName);
                }
            }
        }

        /// <summary>
        /// Makes the backup of the file in the OriginalFile property of this instance
        /// </summary>
        public void Backup()
        {
            if (!Directory.Exists(di.FullName))
            {
                try
                {
                    di.Create();
                }
                catch (UnauthorizedAccessException uae)
                {
                    //MessageBox.Show(uae.Message);
                }
                catch
                {
                    //MessageBox.Show("Cannot create backup directory");
                    //Ask user to specify a backup folder here
                }
            }
            FileInfo[] rhywFiles = di.GetFiles("*.rhyw");
            if (rhywFiles.Length < this.maxBackupFiles)
            {
                this.currentFileNum = GetMostRecentBackupFileNum(di.FullName);
                this.currentFileNum++;
                this.writeFile();
            }
            else
            {
                int oldestFileNum = GetOldestFileFileNum(di.FullName);
                //this.currentFileNum = GetOldestFileFileNum(di.FullName);
                //
                if (this.writeFile() == true)//Only increment If file has been written
                {
                    this.currentFileNum++;
                    this.DeleteFileWithNum(oldestFileNum);
                }
            }
        }

        private void DeleteFileWithNum(int oldestFileNum)
        {
            string xmlFileName = this.originalFile.Substring(this.originalFile.LastIndexOf('\\'));
            string fileExtension = xmlFileName.Substring(xmlFileName.LastIndexOf('.'));
            xmlFileName = xmlFileName.TrimEnd(fileExtension.ToCharArray());
            string fileToDelete = this.backupFolderPath + xmlFileName + "-" + oldestFileNum + fileExtension;
            try
            {
                System.IO.File.Delete(fileToDelete);
            }
            catch
            {
                ;
            }
        }

        /// <summary>
        /// Writes the backup file in the backup folder, if the backup folder does not exist, creates the folder
        /// in the book directory
        /// </summary>
        /// <returns>true: if file is written, false: otherwise</returns>
        private bool writeFile()
        {
            string xmlFileName = this.originalFile.Substring(this.originalFile.LastIndexOf('\\'));
            string fileExtension = xmlFileName.Substring(xmlFileName.LastIndexOf('.'));
            xmlFileName = xmlFileName.TrimEnd(fileExtension.ToCharArray());
            string backupFilePath = this.backupFolderPath + xmlFileName + "-" + this.currentFileNum + fileExtension;

            string tempFile = this.backupFolderPath + xmlFileName + " temp";

            File.Copy(this.originalFile, tempFile, true);

            string lastBackupFile = this.GetMostRecentBackupFileName();
            ///If the newly created file is equal to the last backed up file, set the file num to the previous
            ///file number, so that it is overwritten the next time
            if (lastBackupFile != null)
            {
                if (File.Exists(lastBackupFile) && !FilesEqual(tempFile, lastBackupFile))
                {
                    File.Copy(tempFile, backupFilePath, true);
                    File.Delete(tempFile);
                    return true;
                }
                else
                {
                    File.Delete(tempFile);
                    return false;
                }
            }
            else
            {
                File.Copy(tempFile, backupFilePath, true);
                File.Delete(tempFile);
                return true;
            }
        }


        /// <summary>
        /// Returns true if files are equal, false otherwise
        /// </summary>
        /// <param name="firstFile"></param>
        /// <param name="secondFile"></param>
        /// <returns></returns>
        private bool FilesEqual(string firstFile, string secondFile)
        {
            bool isEqual;
            int i = 0, j = 0;
            FileStream f1 = null;
            FileStream f2 = null;
            try
            {
                f1 = new FileStream(firstFile, FileMode.Open, FileAccess.Read);
                f2 = new FileStream(secondFile, FileMode.Open, FileAccess.Read);
                do
                {
                    i = f1.ReadByte();
                    j = f2.ReadByte();
                    if (i != j) break;
                } while (i != -1 && j != -1);
            }
            catch (IOException)
            {
                ;
            }
            if (i != j)
            {
                isEqual = false;
            }
            else
            {
                isEqual = true;
            }
            f1.Close();
            f2.Close();
            return isEqual;
        }
        private int GetMostRecentBackupFileNum(string backupDirectoryPath)
        {
            DirectoryInfo di = new DirectoryInfo(backupDirectoryPath);
            FileInfo[] backupFiles = di.GetFiles("*.rhyw");
            if (backupFiles.Length == 0)
                return 0;
            //string fileNum = backupFiles[backupFiles.Length - 1].ToString();
            string fileName = GetMostRecentBackupFileName();
            //////
            //////
            string fileNum = fileName.TrimEnd(".rhyw".ToCharArray());
            //int dotIndex = fileNum.LastIndexOf('.');
            fileNum = fileNum.Substring(fileNum.LastIndexOf('-') + 1);
            try
            {
                return int.Parse(fileNum);
            }
            catch
            {
                return 1;
            }
        }

        private string GetMostRecentBackupFileName()
        {
            FileInfo[] backupFiles = di.GetFiles("*.rhyw");
            if (backupFiles.Length == 0)
                return null;
            DateTime dt = new DateTime();
            dt = backupFiles[0].LastWriteTime;
            string fileName = backupFiles[0].FullName;
            for (int i = 1; i < backupFiles.Length; i++)
            {
                DateTime fileDt = backupFiles[i].LastWriteTime;
                if (fileDt > dt)
                {
                    dt = fileDt;
                    fileName = backupFiles[i].FullName;
                }
            }
            return fileName;
        }
        private int GetOldestFileFileNum(string backupDirectoryPath)
        {
            DirectoryInfo di = new DirectoryInfo(backupDirectoryPath);
            FileInfo[] backupFiles = di.GetFiles("*.rhyw");

            DateTime dt = new DateTime();
            dt = backupFiles[0].LastWriteTime;
            int fileNum = GetFileVersion(backupFiles[0].FullName);
            for (int i = 1; i < backupFiles.Length; i++)
            {
                DateTime fileDt = backupFiles[i].LastWriteTime;
                if (fileDt < dt)
                {
                    dt = fileDt;
                    fileNum = GetFileVersion(backupFiles[i].FullName);
                }
            }
            return fileNum;
        }

        private int GetFileVersion(string fileNameToReturn)
        {
            string fileNum = fileNameToReturn;
            //fileNameToReturn = backupFiles[i].FullName;
            //string fileNum = backupFiles[backupFiles.Length - 1].ToString();
            fileNum = fileNum.TrimEnd(".rhyw".ToCharArray());
            //int dotIndex = fileNum.LastIndexOf('.');
            fileNum = fileNum.Substring(fileNum.LastIndexOf('-') + 1);
            try
            {
                return int.Parse(fileNum);
            }
            catch
            {
                return 1;
            }
        }
    }
}

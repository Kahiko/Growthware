using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;

namespace GrowthWare.WebSupport.Utilities
{
    /// <summary>
    /// The FileUtility is a utility class used to help with file and directory management.
    /// </summary>
    public static class FileUtility
    {
        static String s_Space = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

        /// <summary>
        /// Retruns the parent full name.
        /// </summary>
        /// <param name="path">string</param>
        /// <returns>string</returns>
        public static string GetParent(string path)
        {
            string mRetVal = null;
            DirectoryInfo mDirInfo = new DirectoryInfo(path);
            mRetVal = mDirInfo.Parent.FullName.ToString();
            return mRetVal;
        }

        /// <summary>
        /// Returns a table of files and directories.
        /// </summary>
        /// <param name="path">string</param>
        /// <param name="directoryProfile">MDirectoryProfile</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDirectoryTableData(string path, MDirectoryProfile directoryProfile)
        {
            return GetDirectoryTableData(path, directoryProfile, false);
        }

        /// <summary>
        /// Returns a table of files and/or directories.
        /// </summary>
        /// <param name="path">string</param>
        /// <param name="directoryProfile">MDirectoryProfile</param>
        /// <param name="filesOnly">bool</param>
        /// <param name="columnName">name of the column to sort on</param>
        /// <param name="sortOrder">the sort direction "ASC" or "DESC"</param>
        /// <returns>DataTable</returns>
        public static DataTable GetDirectoryTableData(string path, MDirectoryProfile directoryProfile, bool filesOnly, string columnName, string sortOrder)
        {
            if (directoryProfile == null)
            {
                throw new ArgumentNullException("directoryProfile", "directoryProfile can not be null reference (Nothing in Visual Basic)");
            }
            DataTable mRetTable = getDataTable();

            DataRow mRow = mRetTable.NewRow();
            StringBuilder mStringBuilder = new StringBuilder(4096);
            string[] mDirs = null;
            char mDirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;
            WindowsImpersonationContext mImpersonatedUser = null;
            if (directoryProfile.Impersonate)
            {
                mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword);
            }


            mRow["Name"] = mStringBuilder.ToString();
            mStringBuilder = new StringBuilder();
            // Clear the string builder
            if (!filesOnly)
            {
                try
                {
                    mDirs = Directory.GetDirectories(path);
                    foreach (string mDirectory in mDirs)
                    {
                        string mDirName = System.IO.Path.GetFileName(mDirectory);
                        mStringBuilder = new StringBuilder();
                        // Clear the string builder
                        mRow = mRetTable.NewRow();
                        // Create a new row
                        // Populate the string for the new row
                        mStringBuilder.Append(mDirName);
                        // Populate the cell in the row
                        mRow["Name"] = mStringBuilder.ToString();

                        mStringBuilder = new StringBuilder();
                        // Clear the string builder
                        mRow["ShortFileName"] = mStringBuilder.ToString();

                        mStringBuilder = new StringBuilder();
                        // Clear the string builder
                        mRow["Extension"] = mStringBuilder.ToString();

                        mStringBuilder = new StringBuilder();
                        // Clear the string builder
                        // Populate the cell in the row
                        mRow["Delete"] = mStringBuilder.ToString();
                        mStringBuilder = new StringBuilder();
                        // Clear the string builder
                        mStringBuilder.Append("Folder");
                        // Populate the cell in the row
                        mRow["Type"] = mStringBuilder.ToString();
                        mStringBuilder = new StringBuilder();
                        // Clear the string builder
                        mStringBuilder.Append("N/A");
                        // Populate the cell in the row
                        mRow["Size"] = mStringBuilder.ToString();
                        mStringBuilder = new StringBuilder();
                        // Clear the string builder
                        mStringBuilder.Append(Directory.GetLastWriteTime(path + mDirectorySeparatorChar.ToString(CultureInfo.InvariantCulture) + mDirName).ToString());
                        // Populate the cell in the row
                        mRow["Modified"] = mStringBuilder.ToString();
                        mStringBuilder = new StringBuilder();
                        mStringBuilder.Append(mDirectorySeparatorChar.ToString(CultureInfo.InvariantCulture) + mDirName + "\\");
                        mRow["FullName"] = mStringBuilder.ToString();
                        mRetTable.Rows.Add(mRow);
                        // Add the row to the table
                    }
                }
                catch (IOException ex)
                {
                    if (mRetTable != null) mRetTable.Dispose();
                    Logger mLog = Logger.Instance();
                    mLog.Error(ex);
                    throw;
                }

            }
            // Add all of the directories to the table
            try
            {
                DirectoryInfo dirInfo = new DirectoryInfo(path);
                FileInfo[] files = null;
                files = dirInfo.GetFiles();
                //FileInfo mFileInfo = null;
                if (mRetTable == null)
                {

                }
                foreach (FileInfo mFileInfo in files)
                {
                    mRow = mRetTable.NewRow();
                    string mFilename = mFileInfo.Name;
                    string mShortFileName = mFileInfo.Name;
                    mShortFileName = mFilename.Remove(mFilename.Length - mFileInfo.Extension.Length, mFileInfo.Extension.Length);

                    mStringBuilder = new StringBuilder();
                    mStringBuilder.Append(mFilename);
                    mRow["Name"] = mFilename.ToString();

                    mStringBuilder = new StringBuilder();
                    mStringBuilder.Append("File");
                    mRow["Type"] = mStringBuilder.ToString();

                    mStringBuilder = new StringBuilder();
                    mStringBuilder.Append(mShortFileName);
                    mRow["shortFileName"] = mStringBuilder.ToString();

                    mStringBuilder = new StringBuilder();
                    string fileExtension = mFileInfo.Extension;
                    mStringBuilder.Append(fileExtension);
                    mRow["Extension"] = mStringBuilder.ToString();

                    mStringBuilder = new StringBuilder();
                    mStringBuilder.Append(mFileInfo.Length.ToFileSize());
                    mRow["Size"] = mStringBuilder.ToString();
                    mStringBuilder = new StringBuilder();
                    mStringBuilder.Append(File.GetLastWriteTime(path + mDirectorySeparatorChar.ToString() + mFileInfo.Name).ToString());
                    mRow["Modified"] = mStringBuilder.ToString();
                    mStringBuilder = new StringBuilder();
                    mStringBuilder.Append(mFileInfo.FullName);
                    mRow["FullName"] = mStringBuilder.ToString();

                    mRetTable.Rows.Add(mRow);
                }
            }
            catch (IOException ex)
            {
                if (mRetTable != null) mRetTable.Dispose();
                Logger mLog = Logger.Instance();
                mLog.Error(ex);
                throw;
            }
            finally
            {
                if (directoryProfile.Impersonate)
                {
                    // Stop impersonating the user.
                    if ((mImpersonatedUser != null))
                    {
                        mImpersonatedUser.Undo();
                    }
                }
            }            // Return the table object as the data source
            SortTable mSorter = new Framework.Common.SortTable();
            String mColName = columnName;
            mSorter.Sort(mRetTable, mColName, sortOrder);
            return mRetTable;
        }

        /// <summary>
        /// Returns a table of files and/or directories.
        /// </summary>
        /// <param name="path">string</param>
        /// <param name="directoryProfile">MDirectoryProfile</param>
        /// <param name="filesOnly">bool</param>
        /// <returns>DataTable sorted by the "Name" column ascending</returns>
        public static DataTable GetDirectoryTableData(string path, MDirectoryProfile directoryProfile, bool filesOnly) 
        { 
            return GetDirectoryTableData(path, directoryProfile, filesOnly, "Name", "ASC");
        }

        private static DataTable getDataTable()
        {
            DataTable mTempRetTable = null;
            DataTable mRetTable = null;
            mTempRetTable = new DataTable("MyTable");
            try
            {
                mTempRetTable.Locale = CultureInfo.InvariantCulture;
                // Add the column header
                mTempRetTable.Columns.Add("Name", System.Type.GetType("System.String"));
                mTempRetTable.Columns.Add("ShortFileName", System.Type.GetType("System.String"));
                mTempRetTable.Columns.Add("Extension", System.Type.GetType("System.String"));
                mTempRetTable.Columns.Add("Delete", System.Type.GetType("System.String"));
                mTempRetTable.Columns.Add("Type", System.Type.GetType("System.String"));
                mTempRetTable.Columns.Add("Size", System.Type.GetType("System.String"));
                mTempRetTable.Columns.Add("Modified", System.Type.GetType("System.String"));
                mTempRetTable.Columns.Add("FullName", System.Type.GetType("System.String"));
                mTempRetTable.Columns["FullName"].ReadOnly = true;
                mRetTable = mTempRetTable;
            }
            catch (NullReferenceException)
            {
                throw;
            }
            finally 
            {
                if (mTempRetTable != null) mTempRetTable.Dispose();
            }
            return mRetTable;
        }

        /// <summary>
        /// Up loads file from an HtmlInputFile to the directory specified in the MDirectoryProfile object.
        /// </summary>
        /// <param name="uploadFile">HtmlInputFile</param>
        /// <param name="currentDirectory">string</param>
        /// <param name="directoryProfile">MDirectoryProfile</param>
        /// <returns>string</returns>
        public static string DoUpload(HttpPostedFile uploadFile, string currentDirectory, MDirectoryProfile directoryProfile)
        {
            return DoUpload(null, uploadFile, currentDirectory, directoryProfile);
        }

        /// <summary>
        /// Up loads file from an HtmlInputFile to the directory specified in the MDirectoryProfile object with a specific file name.
        /// </summary>
        /// <param name="fileName">string</param>
        /// <param name="uploadFile">HtmlInputFile</param>
        /// <param name="currentDirectory">string</param>
        /// <param name="directoryProfile">MDirectoryProfile</param>
        /// <returns>string</returns>
        public static string DoUpload(string fileName, HttpPostedFile uploadFile, string currentDirectory, MDirectoryProfile directoryProfile)
        {
            if (directoryProfile == null)
            {
                throw new ArgumentNullException("directoryProfile", "directoryProfile can not be null reference (Nothing in Visual Basic)");
            }
            string mRetVal = "Upload successfull";
            char mDirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;
            WindowsImpersonationContext mImpersonatedUser = null;
            if ((uploadFile != null))
            {
                try
                {
                    if (directoryProfile.Impersonate)
                    {
                        mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword);
                    }
                    string mFilename = uploadFile.FileName;
                    if (fileName != null)
                    {
                        mFilename = fileName;
                    }
                    System.IO.Path.GetFileName(uploadFile.FileName);
                    uploadFile.SaveAs(currentDirectory + mDirectorySeparatorChar.ToString() + mFilename);
                }
                catch (IOException ex)
                {
                    Logger mLog = Logger.Instance();
                    mLog.Error(ex);
                    mRetVal = "Failed uploading file";
                }
                finally
                {
                    if (directoryProfile.Impersonate)
                    {
                        // Stop impersonating the user.
                        if ((mImpersonatedUser != null))
                        {
                            mImpersonatedUser.Undo();
                        }
                    }
                }
            }
            else
            {
                mRetVal = "fileToUpload cannot be a null reference (Nothing in Visual Basic)! or Nothing.";
            }
            return mRetVal;
        }

        /// <summary>
        /// Creates the directory specified in the MDirectoryProfile given the currentDirecty.
        /// </summary>
        /// <param name="currentDirectory">string</param>
        /// <param name="newDirectoryName">string</param>
        /// <param name="directoryProfile">MDirectoryProfile</param>
        /// <returns>string</returns>
        public static string CreateDirectory(string currentDirectory, string newDirectoryName, MDirectoryProfile directoryProfile)
        {
            if (directoryProfile == null)
            {
                throw new ArgumentNullException("directoryProfile", "directoryProfile can not be null reference (Nothing in Visual Basic)");
            }
            string mRetVal = null;
            mRetVal = "Successfully created the new directory!";
            WindowsImpersonationContext impersonatedUser = null;
            try
            {
                if (directoryProfile.Impersonate)
                {
                    impersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword);
                }
                Directory.CreateDirectory(currentDirectory + "\\" + newDirectoryName);
            }
            catch (IOException ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Error(ex);
                mRetVal = "Directory was not created!";
            }
            finally
            {
                if (directoryProfile.Impersonate)
                {
                    // Stop impersonating the user.
                    if ((impersonatedUser != null))
                    {
                        impersonatedUser.Undo();
                    }
                }
            }
            return mRetVal;
        }

        /// <summary>
        /// Deletes a directory specified in the MDirectoryProfile in the current directory.
        /// </summary>
        /// <param name="currentDirectoryName">string</param>
        /// <param name="directoryProfile">MDirectoryProfile</param>
        /// <returns></returns>
        public static string DeleteDirectory(string currentDirectoryName, MDirectoryProfile directoryProfile)
        {
            if (directoryProfile == null)
            {
                throw new ArgumentNullException("directoryProfile", "directoryProfile can not be null reference (Nothing in Visual Basic)");
            }
            string mRetVal = null;
            WindowsImpersonationContext mImpersonatedUser = null;
            mRetVal = "Successfully deleted the directory(s)";
            try
            {
                if (directoryProfile.Impersonate)
                {
                    mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword);
                }
                Directory.Delete(currentDirectoryName);
            }
            catch (IOException ex)
            {
                mRetVal = ex.Message.ToString();
            }
            finally
            {
                if (directoryProfile.Impersonate)
                {
                    // Stop impersonating the user.
                    if ((mImpersonatedUser != null))
                    {
                        mImpersonatedUser.Undo();
                    }
                }
            }
            return mRetVal;
        }

        /// <summary>
        /// Deletes a file in the directory specified in the MDirectoryProfile object.
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="directoryProfile"></param>
        /// <returns>string</returns>
        public static string DeleteFile(string fileName, MDirectoryProfile directoryProfile)
        {
            if (directoryProfile == null)
            {
                throw new ArgumentNullException("directoryProfile", "directoryProfile can not be null reference (Nothing in Visual Basic)");
            }
            string mRetVal = null;
            mRetVal = "Successfully deleted the file(s)";
            WindowsImpersonationContext mImpersonatedUser = null;
            try
            {
                if (directoryProfile.Impersonate)
                {
                    mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword);
                }
                File.Delete(fileName);
            }
            catch (IOException ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Error(ex);
                mRetVal = ex.Message.ToString();
            }
            finally
            {
                if (directoryProfile.Impersonate)
                {
                    // Stop impersonating the user.
                    if ((mImpersonatedUser != null))
                    {
                        mImpersonatedUser.Undo();
                    }
                }
            }
            return mRetVal;
        }

        /// <summary>
        /// Renames a file from the "source" to the "destination"
        /// </summary>
        /// <param name="sourceFileName">string</param>
        /// <param name="destinationFileName">string</param>
        /// <param name="directoryProfile">MDirectoryProfile</param>
        /// <returns>string</returns>
        /// <remarks>The MDirectoryProfile object is used for impersonation if necessary.</remarks>
        public static string RenameFile(string sourceFileName, string destinationFileName, MDirectoryProfile directoryProfile)
        {
            if (directoryProfile == null)
            {
                throw new ArgumentNullException("directoryProfile", "directoryProfile can not be null reference (Nothing in Visual Basic)");
            }
            string mRetVal = null;
            mRetVal = "Successfully renamed the file!";
            WindowsImpersonationContext mImpersonatedUser = null;
            try
            {
                if (directoryProfile.Impersonate)
                {
                    mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword);
                }
                File.Move(sourceFileName, destinationFileName);
            }
            catch (IOException ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Error(ex);
                mRetVal = ex.Message.ToString();
            }
            finally
            {
                if (directoryProfile.Impersonate)
                {
                    // Stop impersonating the user.
                    if ((mImpersonatedUser != null))
                    {
                        mImpersonatedUser.Undo();
                    }
                }
            }
            return mRetVal;
        }

        /// <summary>
        /// Renames a directory from the "source" to the "destination"
        /// </summary>
        /// <param name="sourceDirectoryName">string</param>
        /// <param name="destinationDirectoryName">string</param>
        /// <param name="directoryProfile">MDirectoryProfile</param>
        /// <returns>string</returns>
        /// <remarks>The MDirectoryProfile object is used for impersonation if necessary.</remarks>
        public static string RenameDirectory(string sourceDirectoryName, string destinationDirectoryName, MDirectoryProfile directoryProfile)
        {
            string mRetVal = null;
            WindowsImpersonationContext mImpersonatedUser = null;
            mRetVal = "Successfully renamed the directory!";
            if (directoryProfile != null)
            {
                try
                {
                    if (directoryProfile.Impersonate)
                    {
                        mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword);
                    }
                    Directory.Move(sourceDirectoryName, destinationDirectoryName);
                }
                catch (IOException ex)
                {
                    Logger mLog = Logger.Instance();
                    mLog.Error(ex);
                    mRetVal = ex.Message.ToString();
                }
                finally
                {
                    if (directoryProfile.Impersonate)
                    {
                        // Stop impersonating the user.
                        if ((mImpersonatedUser != null))
                        {
                            mImpersonatedUser.Undo();
                        }
                    }
                }
            }
            else
            {
                throw new ArgumentNullException("directoryProfile","directoryProfile can not be null reference (Nothing in Visual Basic)");
            }
            return mRetVal;
        }

        /// <summary>
        /// Gets the line count.
        /// </summary>
        /// <param name="theDirectory">The dir.</param>
        /// <param name="level">An int representing the level.</param>
        /// <param name="outputBuilder">The string builder.</param>
        /// <param name="excludeList">The exclude list.</param>
        /// <param name="directoryLineCount">The directory line count.</param>
        /// <param name="totalLinesOfCode">The total lines of code.</param>
        /// <param name="fileArray">The file array.</param>
        /// <returns>System.String.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static string GetLineCount(DirectoryInfo theDirectory, int level, StringBuilder outputBuilder, List<String> excludeList, int directoryLineCount, int totalLinesOfCode, String[] fileArray)
        {
            if (theDirectory == null) throw new ArgumentNullException("theDirectory", "theDirectory cannot be a null reference (Nothing in Visual Basic)!");
            if (outputBuilder == null) throw new ArgumentNullException("outputBuilder", "outputBuilder cannot be a null reference (Nothing in Visual Basic)!");
            DirectoryInfo[] subDirectories = null;
            try
            {
                subDirectories = theDirectory.GetDirectories();
                int x = 0;
                int numDirectories = subDirectories.Length - 1;
                if (directoryLineCount > 0)
                {
                    totalLinesOfCode += totalLinesOfCode;
                    outputBuilder.AppendLine("<br>Lines of code for " + theDirectory.Name + " " + directoryLineCount);
                    outputBuilder.AppendLine("<br>Lines of so far " + totalLinesOfCode);
                    directoryLineCount = 0;
                }
                for (x = 0; x <= numDirectories; x++)
                {
                    if (subDirectories[x].Name.Trim().ToUpper(CultureInfo.InvariantCulture) != "BIN" && subDirectories[x].Name.Trim().ToUpper(CultureInfo.InvariantCulture) != "DEBUG" && subDirectories[x].Name.Trim().ToUpper(CultureInfo.InvariantCulture) != "RELEASE")
                    {
                        CountDirectory(subDirectories[x], outputBuilder, excludeList, fileArray, ref directoryLineCount);
                        if (directoryLineCount > 0)
                        {
                            totalLinesOfCode += directoryLineCount;
                            outputBuilder.AppendLine("<br>Lines of code for " + subDirectories[x].Name + " " + directoryLineCount);
                            outputBuilder.AppendLine("<br>Lines of so far " + totalLinesOfCode);
                            directoryLineCount = 0;
                        }
                    }
                    checked
                    {
                        level = level + 1;
                    }
                    GetLineCount(subDirectories[x], level, outputBuilder, excludeList, directoryLineCount, totalLinesOfCode, fileArray);
                }
            }
            catch (NullReferenceException)
            {
                outputBuilder.AppendLine("Directory not found");
            }
            return outputBuilder.ToString();
        }


        /// <summary>
        /// Counts the directory.
        /// </summary>
        /// <param name="theDirectory">The directory.</param>
        /// <param name="outputBuilder">The string builder.</param>
        /// <param name="excludeList">The exclude list.</param>
        /// <param name="fileArray">The file array.</param>
        /// <param name="directoryLineCount">The directory line count.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public static void CountDirectory(DirectoryInfo theDirectory, StringBuilder outputBuilder, List<String> excludeList, String[] fileArray, ref int directoryLineCount)
        {
            if (theDirectory == null) throw new ArgumentNullException("theDirectory", "theDirectory cannot be a null reference (Nothing in Visual Basic)!");
            if (outputBuilder == null) throw new ArgumentNullException("outputBuilder", "outputBuilder cannot be a null reference (Nothing in Visual Basic)!");
            if (excludeList == null) throw new ArgumentNullException("excludeList", "excludeList cannot be a null reference (Nothing in Visual Basic)!");
            if (fileArray == null) throw new ArgumentNullException("fileArray", "fileArray cannot be a null reference (Nothing in Visual Basic)!");

            Boolean writeDirectory = true;
            int FileLineCount = 0;
            foreach (String sFileType in fileArray)
            {
                // this loops files
                Boolean countFile = true;
                foreach (FileInfo directoryFile in theDirectory.GetFiles(sFileType.Trim()))
                {
                    countFile = true;
                    foreach (var item in excludeList)
                    {
                        if (directoryFile.Name.ToUpper(CultureInfo.InvariantCulture).IndexOf(item, StringComparison.OrdinalIgnoreCase) > -1)
                        {
                            countFile = false;
                            break;
                        }
                    }
                    if (countFile)
                    {
                        using (StreamReader sr = File.OpenText(directoryFile.FullName))
                        {
                            //loop until the end
                            while (sr.Peek() > -1)
                            {
                                String myString = sr.ReadLine();
                                if ((!myString.Trim().StartsWith("'", StringComparison.OrdinalIgnoreCase) || !myString.Trim().StartsWith("//", StringComparison.OrdinalIgnoreCase)) & myString.Trim().Length != 0)
                                {
                                    FileLineCount += 1;
                                }
                            }                            
                        }
                        if (FileLineCount > 0)
                        {
                            if (writeDirectory)
                            {
                                outputBuilder.AppendLine("<br>" + theDirectory.FullName);
                                writeDirectory = false;
                            }
                            outputBuilder.AppendLine("<br>" + s_Space + directoryFile.Name + " " + FileLineCount);
                        }
                        if (FileLineCount > 0)
                        {
                            directoryLineCount += FileLineCount;
                        }
                    }
                    FileLineCount = 0;
                }
            }
        }

        /// <summary>
        /// To the size of the file.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>System.String.</returns>
        public static string ToFileSize(this int source)
        {
            return ToFileSize(Convert.ToInt64(source));
        }

        /// <summary>
        /// To the size of the file.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <returns>System.String.</returns>
        public static string ToFileSize(this long source)
        {
            const int byteConversion = 1024;
            double bytes = Convert.ToDouble(source);

            if (bytes >= Math.Pow(byteConversion, 3)) //GB Range
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 3), 2), " GB");
            }
            else if (bytes >= Math.Pow(byteConversion, 2)) //MB Range
            {
                return string.Concat(Math.Round(bytes / Math.Pow(byteConversion, 2), 2), " MB");
            }
            else if (bytes >= byteConversion) //KB Range
            {
                return string.Concat(Math.Round(bytes / byteConversion, 2), " KB");
            }
            else //Bytes
            {
                return string.Concat(bytes, " Bytes");
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using GrowthWare.Framework.Models;

namespace GrowthWare.Framework;

/// <summary>
/// The FileUtility is a utility class used to help with file and directory management.
/// </summary>
public static class FileUtility
{
    static String s_Space = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";

    public static string GetLineCount(DirectoryInfo theDirectory, int level, StringBuilder outputBuilder, List<String> excludeList, int directoryLineCount, ref int totalLinesOfCode, String[] fileArray)
    {
        if (theDirectory == null) throw new ArgumentNullException(nameof(theDirectory), "theDirectory cannot be a null reference (Nothing in Visual Basic)!");
        if (outputBuilder == null) throw new ArgumentNullException(nameof(outputBuilder), "outputBuilder cannot be a null reference (Nothing in Visual Basic)!");
        DirectoryInfo[] subDirectories = null;
        try
        {
            // subDirectories = theDirectory.GetDirectories();
            List<string> mExcludeDirectories = new List<string>();
            mExcludeDirectories.Add("node_modules");
            mExcludeDirectories.Add(".vscode");
            mExcludeDirectories.Add(".angular");
            mExcludeDirectories.Add("bin");
            mExcludeDirectories.Add("obj");
            subDirectories = theDirectory.GetDirectories().Where(d => !mExcludeDirectories.Contains(d.Name)).ToArray();
            int x = 0;
            int numDirectories = subDirectories.Length - 1;
            if (directoryLineCount > 0)
            {
                totalLinesOfCode += totalLinesOfCode;
                outputBuilder.AppendLine("Lines of code for " + theDirectory.Name + " " + directoryLineCount);
                outputBuilder.AppendLine("Lines of so far " + totalLinesOfCode);
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
                        outputBuilder.AppendLine("Lines of code for " + subDirectories[x].Name + " " + directoryLineCount);
                        outputBuilder.AppendLine("Lines of so far " + totalLinesOfCode);
                        directoryLineCount = 0;
                    }
                }
                checked
                {
                    level = level + 1;
                }
                GetLineCount(subDirectories[x], level, outputBuilder, excludeList, directoryLineCount, ref totalLinesOfCode, fileArray);
            }
        }
        catch (NullReferenceException)
        {
            outputBuilder.AppendLine("Directory not found");
        }
        return outputBuilder.ToString();
    }

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
    /// Returns true if the file is in use.
    /// </summary>
    /// <param name="file"></param>
    /// <returns>bool</returns>
    public static bool IsFileInUse(FileInfo file)
    {
        bool mRetVal = false;
        try
        {
            using var stream = file.Open(FileMode.Open, FileAccess.Read, FileShare.None);
        }
        catch (IOException)
        {
            mRetVal = true;
        }
        return mRetVal;
    }

    /// <summary>
    /// Deletes old files
    /// </summary>
    /// <param name="theDirectory"></param>
    /// <param name="daysToKeep"></param>
    public static void DeleteOlderFiles(string filePath, int daysToKeep)
    {
        int mCounter = 0;
        int mPosSep = 0;
        string[] mAFiles = null;
        string mFile = null;
        if (daysToKeep > 0)
        {
            daysToKeep = daysToKeep * -1;
            System.DateTime mRetentionDate = System.DateTime.Now.AddDays(daysToKeep);
            if (System.IO.Directory.Exists(filePath))
            {
                mAFiles = System.IO.Directory.GetFiles(filePath);
                for (mCounter = 0; mCounter <= mAFiles.GetUpperBound(0); mCounter++)
                {
                    // Get the position of the trailing separator.
                    mPosSep = mAFiles[mCounter].LastIndexOf(Path.DirectorySeparatorChar.ToString(), StringComparison.OrdinalIgnoreCase);
                    mFile = mAFiles[mCounter].Substring((mPosSep + 1), mAFiles[mCounter].Length - (mPosSep + 1));
                    mFile = filePath + mFile;
                    if (File.GetCreationTime(mFile) < mRetentionDate)
                    {
                        File.Delete(mFile);
                    }
                }
            }
        }
    }

    public static void CountDirectory(DirectoryInfo theDirectory, StringBuilder outputBuilder, List<String> excludeList, String[] fileArray, ref int directoryLineCount)
    {
        if (theDirectory == null) throw new ArgumentNullException(nameof(theDirectory), "theDirectory cannot be a null reference (Nothing in Visual Basic)!");
        if (outputBuilder == null) throw new ArgumentNullException(nameof(outputBuilder), "outputBuilder cannot be a null reference (Nothing in Visual Basic)!");
        if (excludeList == null) throw new ArgumentNullException(nameof(excludeList), "excludeList cannot be a null reference (Nothing in Visual Basic)!");
        if (fileArray == null) throw new ArgumentNullException(nameof(fileArray), "fileArray cannot be a null reference (Nothing in Visual Basic)!");

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
                            outputBuilder.AppendLine(theDirectory.FullName);
                            writeDirectory = false;
                        }
                        outputBuilder.AppendLine(s_Space + directoryFile.Name + " " + FileLineCount);
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

    public static string CreateDirectory(string currentDirectory, string newDirectoryName, MDirectoryProfile directoryProfile)
    {
        if (directoryProfile == null)
        {
            throw new ArgumentNullException(nameof(directoryProfile), "directoryProfile can not be null reference (Nothing in Visual Basic)");
        }
        string mRetVal = null;
        mRetVal = "Successfully created the new directory!";
        /**
            * TODO: This is legacy code that I'm not sure we should do.  Doing so will not work for all OS's.
            * WindowsImpersonationContext is specific to windows and I haven't researched it for other OS's
            */
        // WindowsImpersonationContext impersonatedUser = null;
        // try
        // {
        //     if (directoryProfile.Impersonate)
        //     {
        //         impersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.ImpersonateAccount, directoryProfile.ImpersonatePassword);
        //     }
        //     Directory.CreateDirectory(currentDirectory + "\\" + newDirectoryName);
        // }
        // catch (IOException ex)
        // {
        //     Logger mLog = Logger.Instance();
        //     mLog.Error(ex);
        //     mRetVal = "Directory was not created!";
        // }
        // finally
        // {
        //     if (directoryProfile.Impersonate)
        //     {
        //         // Stop impersonating the user.
        //         if ((impersonatedUser != null))
        //         {
        //             impersonatedUser.Undo();
        //         }
        //     }
        // }
        return mRetVal;
    }

    public static string CreateDirectory(string directoryToCreate)
    {
        string mRetVal = "Successfully created the new directory!";
        try
        {
            Directory.CreateDirectory(directoryToCreate);
        }
        catch (System.Exception ex)
        {
            Logger mLog = Logger.Instance();
            mLog.Error(ex);
            mRetVal = "Directory was not created!";
        }
        return mRetVal;
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
    public static DataTable GetDirectory(string path, bool filesOnly, string columnName, string sortOrder)
    {
        DataTable mRetTable = getDataTable();

        DataRow mRow = mRetTable.NewRow();
        StringBuilder mStringBuilder = new StringBuilder(4096);
        string[] mDirs = null;
        char mDirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;

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
            catch (IOException)
            {
                if (mRetTable != null) mRetTable.Dispose();
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
        catch (IOException)
        {
            if (mRetTable != null) mRetTable.Dispose();
            throw;
        }
        SortTable mSorter = new SortTable();
        String mColName = columnName;
        mSorter.Sort(mRetTable, mColName, sortOrder);
        return mRetTable;
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

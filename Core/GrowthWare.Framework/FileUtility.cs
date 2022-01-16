using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;

namespace GrowthWare.Framework
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
                catch (IOException ex)
                {
                    if (mRetTable != null) mRetTable.Dispose();
                    throw ex;
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
                throw ex;
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
}

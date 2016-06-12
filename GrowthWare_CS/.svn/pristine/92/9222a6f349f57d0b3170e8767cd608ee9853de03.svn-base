using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.UI.HtmlControls;
using GrowthWare.Framework.Model.Profiles;

namespace GrowthWare.Framework.Web.Utilities
{
	/// <summary>
	/// The FileUtility is a utility class used to help with file and directory management.
	/// </summary>
	public static class FileUtility
	{
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
		/// <returns>DataTable</returns>
		public static DataTable GetDirectoryTableData(string path, MDirectoryProfile directoryProfile, bool filesOnly)
		{
			if (directoryProfile == null)
			{
				throw new ArgumentNullException("directoryProfile", "Can not be null reference (Nothing in Visual Basic)");
			}
			DataTable mRetTable = null;
			if(directoryProfile != null)
			{
				mRetTable = new DataTable("MyTable");
				DataRow mRow = mRetTable.NewRow();
				StringBuilder mStringBuilder = new StringBuilder(4096);
				string[] mDirs = null;
				int mKiloBytes = 1024;
				int mMegaBytes = mKiloBytes * 1024;
				int mGigaBytes = mMegaBytes * 1024;
				char mDirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;
				WindowsImpersonationContext mImpersonatedUser = null;
				if(directoryProfile.Impersonate)
				{
					mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.Impersonate_Account, directoryProfile.Impersonate_PWD);
				}

				// Add the column header

				mRetTable.Columns.Add("Name", System.Type.GetType("System.String"));
				mRetTable.Columns.Add("ShortFileName", System.Type.GetType("System.String"));
				mRetTable.Columns.Add("Extension", System.Type.GetType("System.String"));
				mRetTable.Columns.Add("Delete", System.Type.GetType("System.String"));
				mRetTable.Columns.Add("Type", System.Type.GetType("System.String"));
				mRetTable.Columns.Add("Size", System.Type.GetType("System.String"));
				mRetTable.Columns.Add("Modified", System.Type.GetType("System.String"));
				mRetTable.Columns.Add("FullName", System.Type.GetType("System.String"));
				mRetTable.Columns["FullName"].ReadOnly = true;

				mRow["Name"] = mStringBuilder.ToString();
				mStringBuilder = new StringBuilder();
				// Clear the string builder
				if(!filesOnly)
				{
					try
					{
						mDirs = Directory.GetDirectories(path);
						foreach(string mDirectory in mDirs)
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
							mStringBuilder.Append(Directory.GetLastWriteTime(path + mDirectorySeparatorChar.ToString() + mDirName).ToString());
							// Populate the cell in the row
							mRow["Modified"] = mStringBuilder.ToString();
							mStringBuilder = new StringBuilder();
							mStringBuilder.Append(mDirectorySeparatorChar.ToString() + mDirName + "\\");
							mRow["FullName"] = mStringBuilder.ToString();
							mRetTable.Rows.Add(mRow);
							// Add the row to the table
						}
					}
					catch (IOException ex)
					{
						LogUtility mLog = LogUtility.GetInstance();
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
					foreach(FileInfo mFileInfo in files)
					{
						mRow = mRetTable.NewRow();
						string mFilename = mFileInfo.Name;
						string mShortFileName = mFileInfo.Name;
						mShortFileName = mFilename.Remove(mFilename.Length - mFileInfo.Extension.Length, mFileInfo.Extension.Length);
						string mFileSize = null;
						mStringBuilder = new StringBuilder();
						mStringBuilder.Append(mFilename);
						mRow["Name"] = mStringBuilder.ToString();
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
						// Show size in either KB or MB.  GB files will cause a problem
						if((mFileInfo.Length / mKiloBytes) <= mKiloBytes)
						{
							mFileSize = (mFileInfo.Length / mKiloBytes).ToString(CultureInfo.CurrentCulture);
							//sb.Append(String.FormatNumber(sFileSize, 1,TriState.False ,TriState.True , TriState.True, TriState.False) + " KB");
							mStringBuilder.Append(String.Format(CultureInfo.CurrentCulture, mFileSize, "#.#") + " KB");
						}
						else if((mFileInfo.Length / mMegaBytes) <= mMegaBytes)
						{
							mFileSize = (mFileInfo.Length / mMegaBytes).ToString(CultureInfo.CurrentCulture);
							//sb.Append(String.FormatNumber(sFileSize, 1, TriState.False, TriState.True, TriState.True, TriState.False) + " MB");
							mStringBuilder.Append(String.Format(CultureInfo.CurrentCulture, mFileSize, "#.#") + " MB");
						}
						else
						{
							mFileSize = (mFileInfo.Length / mGigaBytes).ToString(CultureInfo.CurrentCulture);
							//sb.Append(String.FormatNumber(sFileSize, 1, TriState.False, TriState.True, TriState.True, TriState.False) + " GB");
							mStringBuilder.Append(String.Format(CultureInfo.CurrentCulture, mFileSize, "#.#") + " GB");
						}

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
					LogUtility mLog = LogUtility.GetInstance();
					mLog.Error(ex);
					throw;
				}
				finally
				{
					if(directoryProfile.Impersonate)
					{
						// Stop impersonating the user.
						if((mImpersonatedUser != null))
						{
							mImpersonatedUser.Undo();
						}
					}
				}
			}
			else
			{
				mRetTable.Dispose();
				throw new ArgumentNullException("directoryProfile", "Can not be null reference (Nothing in Visual Basic)");
			}
			// Return the table object as the data source
			return mRetTable;
		}

		/// <summary>
		/// Up loads file from an HtmlInputFile to the directory specified in the MDirectoryProfile object.
		/// </summary>
		/// <param name="uploadFile">HtmlInputFile</param>
		/// <param name="currentDir">string</param>
		/// <param name="directoryProfile">MDirectoryProfile</param>
		/// <returns>string</returns>
		public static string DoUpload(HtmlInputFile uploadFile, string currentDir, MDirectoryProfile directoryProfile)
		{
			if (directoryProfile == null)
			{
				throw new ArgumentNullException("directoryProfile", "Can not be null reference (Nothing in Visual Basic)");
			}
			string mRetVal = "Upload successfull";
			char mDirectorySeparatorChar = System.IO.Path.DirectorySeparatorChar;
			WindowsImpersonationContext mImpersonatedUser = null;
			if(uploadFile != null)
			{
				if((uploadFile.PostedFile != null))
				{
					try
					{
						if(directoryProfile.Impersonate)
						{
							mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.Impersonate_Account, directoryProfile.Impersonate_PWD);
						}

						HttpPostedFile mPostedFile = uploadFile.PostedFile;
						string mFilename = System.IO.Path.GetFileName(mPostedFile.FileName);
						//string mContentType = mPostedFile.ContentType;
						//int mContentLength = mPostedFile.ContentLength;

						mPostedFile.SaveAs(currentDir + mDirectorySeparatorChar.ToString() + mFilename);
					}
					catch (IOException ex)
					{
						LogUtility mLog = LogUtility.GetInstance();
						mLog.Error(ex);
						mRetVal = "Failed uploading file";
					}
					finally
					{
						if(directoryProfile.Impersonate)
						{
							// Stop impersonating the user.
							if((mImpersonatedUser != null))
							{
								mImpersonatedUser.Undo();
							}
						}
					}
				}
			}
			else
			{
				mRetVal = "fileToUpload can not be null or Nothing.";
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
				throw new ArgumentNullException("directoryProfile", "can not be null reference (Nothing in Visual Basic)");
			}
			string mRetVal = null;
			mRetVal = "Successfully created the new directory!";
			WindowsImpersonationContext impersonatedUser = null;
			try {
				if (directoryProfile.Impersonate) {
					impersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.Impersonate_Account, directoryProfile.Impersonate_PWD);
				}
				Directory.CreateDirectory(currentDirectory + "\\" + newDirectoryName);
			}
			catch (IOException ex) 
			{
				LogUtility mLog = LogUtility.GetInstance();
				mLog.Error(ex);
				mRetVal = "Directory was not created!";
			}
			finally 
			{
				if (directoryProfile.Impersonate) {
					// Stop impersonating the user.
					if ((impersonatedUser != null)) {
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
				throw new ArgumentNullException("directoryProfile", "Can not be null reference (Nothing in Visual Basic)");
			}
			string mRetVal = null;
			WindowsImpersonationContext mImpersonatedUser = null;
			mRetVal = "Successfully deleted the directory(s)";
			try {
				if (directoryProfile.Impersonate) {
					mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.Impersonate_Account, directoryProfile.Impersonate_PWD);
				}
				Directory.Delete(currentDirectoryName);
			}
			catch (IOException ex)
			{
				mRetVal = ex.Message.ToString();
			}
			finally {
				if (directoryProfile.Impersonate) {
					// Stop impersonating the user.
					if ((mImpersonatedUser != null)) {
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
				throw new ArgumentNullException("directoryProfile", "Can not be null reference (Nothing in Visual Basic)");
			}
			string mRetVal = null;
			mRetVal = "Successfully deleted the file(s)";
			WindowsImpersonationContext mImpersonatedUser = null;
			try {
				if (directoryProfile.Impersonate) {
					mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.Impersonate_Account, directoryProfile.Impersonate_PWD);
				}
				File.Delete(fileName);
			}
			catch (IOException ex)
			{
				LogUtility mLog = LogUtility.GetInstance();
				mLog.Error(ex);
				mRetVal = ex.Message.ToString();
			}
			finally {
				if (directoryProfile.Impersonate) {
					// Stop impersonating the user.
					if ((mImpersonatedUser != null)) {
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
				throw new ArgumentNullException("directoryProfile", "Can not be null reference (Nothing in Visual Basic)");
			}
			string mRetVal = null;
			mRetVal = "Successfully renamed the file!";
			WindowsImpersonationContext mImpersonatedUser = null;
			try {
				if (directoryProfile.Impersonate) {
					mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.Impersonate_Account, directoryProfile.Impersonate_PWD);
				}
				File.Move(sourceFileName, destinationFileName);
			}
			catch (IOException ex)
			{
				LogUtility mLog = LogUtility.GetInstance();
				mLog.Error(ex);
				mRetVal = ex.Message.ToString();
			}
			finally {
				if (directoryProfile.Impersonate) {
					// Stop impersonating the user.
					if ((mImpersonatedUser != null)) {
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
			if(directoryProfile != null)
			{
				try
				{
					if(directoryProfile.Impersonate)
					{
						mImpersonatedUser = WebImpersonate.ImpersonateNow(directoryProfile.Impersonate_Account, directoryProfile.Impersonate_PWD);
					}
					Directory.Move(sourceDirectoryName, destinationDirectoryName);
				}
				catch (IOException ex)
				{
					LogUtility mLog = LogUtility.GetInstance();
					mLog.Error(ex);
					mRetVal = ex.Message.ToString();
				}
				finally
				{
					if(directoryProfile.Impersonate)
					{
						// Stop impersonating the user.
						if((mImpersonatedUser != null))
						{
							mImpersonatedUser.Undo();
						}
					}
				}
			}
			else
			{
				throw new ArgumentNullException("directoryProfile");
			}
			return mRetVal;
		}
	}
}

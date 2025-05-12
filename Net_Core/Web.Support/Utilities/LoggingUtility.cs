using System.Collections.Generic;
using System.Data;
using System.IO;
using System.IO.Compression;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.Web.Support.Utilities;

public static class LoggingUtility
{
    private static BLogger m_BusinessLogic = null;

    private static Logger m_Logger = Logger.Instance();

    public static async Task CreateSystemLogsZipAsync(string zipFilePath)
    {
        using var mZipArchive = ZipFile.Open(zipFilePath, ZipArchiveMode.Create);
        try
        {
            // 1. First, add the database logs as CSV
            var mCsvDatabaseLogs = mZipArchive.CreateEntry("ZGWSystem_Logging.csv");
            using var mCsvStream = mCsvDatabaseLogs.Open();
            using var mStreamWriter = new StreamWriter(mCsvStream);

            // Write CSV headers
            await mStreamWriter.WriteLineAsync(DBLogColumns.GetCommaSeparatedString());

            // Get database records (using async streaming)
            await foreach (var mRecord in LoggingUtility.GetDBLogRecordsForExportAsync())
            {
                List<string> mColumnValues = [];
                for (int i = 0; i < DBLogColumns.GetList().Count; i++)
                {
                    mColumnValues.Add(mRecord.GetValue(i).ToString());
                }
                string mLine = string.Join(",", mColumnValues);
                await mStreamWriter.WriteLineAsync(mLine);
            }
            mStreamWriter.Flush();
            mStreamWriter.Close();

            // 2. Add any existing log files from the system
            var logDirectory = m_Logger.LogFilePath;
            if (Directory.Exists(logDirectory))
            {
                foreach (string fileName in Directory.GetFiles(logDirectory))
                {
                    // Get the relative path to maintain directory structure
                    FileInfo mFileInfo = new(fileName);
                    string relativePath = Path.GetRelativePath(logDirectory, fileName);
                    if (!FileUtility.IsFileInUse(mFileInfo))
                    {
                        mZipArchive.CreateEntryFromFile(fileName, $"Logs/{relativePath}");
                    }
                    else
                    {
                        ZipArchiveEntry entry = mZipArchive.CreateEntry($"Logs/{relativePath}");
                        using (FileStream fileStream = new (fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                        {
                            using (Stream entryStream = entry.Open())
                            {
                                fileStream.CopyTo(entryStream);
                            }
                        }
                    }
                }
            }
        }
        catch (System.Exception ex)
        {
            m_Logger.Error(ex);
            mZipArchive?.Dispose();
            if (File.Exists(zipFilePath))
            {
                File.Delete(zipFilePath);
            }
            throw;
        }
    }


    /// <summary>
    /// Returns the business logic object used to access the database.
    /// </summary>
    /// <returns></returns>
    private static async Task<BLogger> getBusinessLogic()
    {
        if (m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
        {
            m_BusinessLogic = new(await SecurityEntityUtility.CurrentProfile());
        }
        return m_BusinessLogic;
    }

    public static async IAsyncEnumerable<IDataRecord> GetDBLogRecordsForExportAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        BLogger mBusinessLogic = await getBusinessLogic();
        await foreach (var record in mBusinessLogic.GetDBLogRecordsForExportAsync(cancellationToken))
        {
            yield return record;
        }
    }

    public static async Task<MLoggingProfile> GetProfile(int logSeqId)
    {
        BLogger mBusinessLogic = await getBusinessLogic();
        return await mBusinessLogic.GetLoggingProfile(logSeqId);
    }
    
    public static async Task Save(MLoggingProfile profile)
    {
        BLogger mBusinessLogic = await getBusinessLogic();
        await mBusinessLogic.Save(profile);
    }
}
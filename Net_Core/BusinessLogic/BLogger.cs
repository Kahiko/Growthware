using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace GrowthWare.BusinessLogic;

public class BLogger : AbstractBusinessLogic
{

    #region Member Fields
    private ILogging m_Logging;
    #endregion

    #region Constructors
    public BLogger(MSecurityEntity securityEntityProfile)
    {
        if (securityEntityProfile == null) throw new ArgumentNullException(nameof(securityEntityProfile), "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!");
        if (m_Logging == null)
        {
            this.m_Logging = (ILogging)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DLogging", securityEntityProfile.ConnectionString);
            if (this.m_Logging == null)
            {
                throw new InvalidOperationException("Failed to create an instance of DLogging.");
            }
        }
    }
    #endregion

    /// <summary>
    /// Asynchronously retrieves log records for export.
    /// </summary>
    /// <param name="cancellationToken"></param>
    /// <returns>IAsyncEnumerable<IDataRecord></returns>
    public async IAsyncEnumerable<IDataRecord> GetDBLogRecordsForExportAsync([EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        if (DatabaseIsOnline())
        {
            await foreach (var record in m_Logging.GetLogs(cancellationToken))
            {
                yield return record;
            }
        }
    }

    /// <summary>
    /// Retrieves the logging profile for a given log sequence ID.
    /// </summary>
    /// <param name="logSeqId">The log sequence ID.</param>
    /// <returns>The logging profile if the database is online, otherwise null.</returns>
    public async Task<MLoggingProfile> GetLoggingProfile(int logSeqId)
    {
        if (DatabaseIsOnline())
        {
            return await this.m_Logging.GetLog(logSeqId);
        }
        return null;
    }

    /// <summary>
    /// Saves the profile information to the Database.
    /// </summary>
    /// <param name="profile"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public async Task Save(MLoggingProfile profile)
    {
        if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!");
        if (DatabaseIsOnline())
        {
            await m_Logging.Save(profile);
        }
    }
}

using GrowthWare.Framework.Models;
using GrowthWare.DataAccess.Interfaces.Base;
using System.Data;
using System.Collections.Generic;
using System.Threading;

namespace GrowthWare.DataAccess.Interfaces
{
    /// <summary>
    /// The base interface for Database interaction code
    /// </summary>
    public interface ILogging : IDBInteraction
    {

        /// <summary>
        /// Returns a populated MLoggingProfile object given the logSeqId
        /// </summary>
        /// <param name="logSeqId">int</param>
        /// <returns>MLoggingProfile</returns>
        MLoggingProfile GetLog(int logSeqId);

        /// <summary>
        /// Returns data from the Logging table
        /// </summary>
        /// <param name="cancellationToken"></param>
        /// <returns>IAsyncEnumerable<IDataRecord></returns>
        IAsyncEnumerable<IDataRecord> GetLogs(CancellationToken cancellationToken = default);

        /// <summary>
        /// Saves this instance.
        /// </summary>
        void Save(MLoggingProfile profile);
    }

}
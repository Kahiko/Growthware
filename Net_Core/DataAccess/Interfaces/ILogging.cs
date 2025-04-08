using GrowthWare.Framework.Models;
using GrowthWare.DataAccess.Interfaces.Base;
using System;
using System.Data;
using System.Threading.Tasks;

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
        Task<MLoggingProfile> GetLog(int logSeqId);

        /// <summary>
        /// Saves this instance.
        /// </summary>
        Task Save(MLoggingProfile profile);
    }

}
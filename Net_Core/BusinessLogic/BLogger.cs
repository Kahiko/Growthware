using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using System;

namespace GrowthWare.BusinessLogic
{
    public class BLogger : AbstractBusinessLogic
    {

        private ILogging m_Logging;

        public BLogger(string dataAccessLayerAssemblyName, string dataAccessLayerNamespace, string connectionString)
        {
            this.m_Logging = (ILogging)ObjectFactory.Create(dataAccessLayerAssemblyName, dataAccessLayerNamespace, "DLogging");
            this.m_Logging.ConnectionString = connectionString;
        }

        /// <summary>
        /// Retrieves the logging profile for a given log sequence ID.
        /// </summary>
        /// <param name="logSeqId">The log sequence ID.</param>
        /// <returns>The logging profile if the database is online, otherwise null.</returns>
        public MLoggingProfile GetLoggingProfile(int logSeqId)
        {
            if (DatabaseIsOnline())
            {
                return this.m_Logging.GetLog(logSeqId);
            }
            return null;
        }

        /// <summary>
        /// Saves the profile information to the Database.
        /// </summary>
        /// <param name="profile"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public void Save(MLoggingProfile profile)
        {
            if (profile == null) throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!");
            if (DatabaseIsOnline())
            {
                m_Logging.Save(profile);
            }
        }
    }
}

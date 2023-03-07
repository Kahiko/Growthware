using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using System;

namespace GrowthWare.BusinessLogic
{
    public class BLogger
    {

        private ILogging m_Logging;

        public BLogger(string dataAccessLayerAssemblyName, string dataAccessLayerNamespace, string connectionString)
        {
            this.m_Logging = (ILogging)ObjectFactory.Create(dataAccessLayerAssemblyName, dataAccessLayerNamespace, "DLogging");
            this.m_Logging.ConnectionString = connectionString;
        }

        public MLoggingProfile GetLoggingProfile(int logSeqId)
        {
            return this.m_Logging.GetLog(logSeqId);
        }

        public void Save(MLoggingProfile profile)
        {
            m_Logging.Save(profile);
        }
    }
}

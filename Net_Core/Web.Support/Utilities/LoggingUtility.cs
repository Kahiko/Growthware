using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.Web.Support.Utilities;
public static class LoggingUtility
{
    private static BLogger m_BusinessLogic = null;
    /// <summary>
    /// Returns the business logic object used to access the database.
    /// </summary>
    /// <returns></returns>
    private static BLogger getBusinessLogic()
    {
        if(m_BusinessLogic == null || ConfigSettings.CentralManagement == true)
        {
            m_BusinessLogic = new(ConfigSettings.DataAccessLayerAssemblyName, ConfigSettings.DataAccessLayerNamespace, ConfigSettings.ConnectionString);
        }
        return m_BusinessLogic;
    }

    public static MLoggingProfile GetProfile(int logSeqId)
    {
        BLogger mBLogger = getBusinessLogic();
        return mBLogger.GetLoggingProfile(logSeqId);
    }
    public static void Save(MLoggingProfile profile)
    {
        BLogger mBLogger = getBusinessLogic();
        mBLogger.Save(profile);
    }
}
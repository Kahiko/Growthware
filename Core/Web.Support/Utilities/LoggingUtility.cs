using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.Web.Support.Utilities;
public static class LoggingUtility
{

    public static MLoggingProfile GetProfile(int logSeqId)
    {
        BLogger mBLogger = new BLogger(ConfigSettings.DataAccessLayerAssemblyName, ConfigSettings.DataAccessLayerNamespace, ConfigSettings.ConnectionString);
        return mBLogger.GetLoggingProfile(logSeqId);
    }
    public static void Save(MLoggingProfile profile)
    {
        BLogger mBLogger = new BLogger(ConfigSettings.DataAccessLayerAssemblyName, ConfigSettings.DataAccessLayerNamespace, ConfigSettings.ConnectionString);
        mBLogger.Save(profile);
    }
}
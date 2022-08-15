
using GrowthWare.Framework;
using GrowthWare.Framework.Models;

namespace GrowthWare.WebSupport.Utilities;

public static class SecurityEntityUtility {


    public static MSecurityEntity CurrentProfile(){
        MSecurityEntity mRetVal = new MSecurityEntity();
        mRetVal.ConnectionString = ConfigSettings.ConnectionString;
        mRetVal.DataAccessLayer = ConfigSettings.DataAccessLayer;
        mRetVal.DataAccessLayerAssemblyName = ConfigSettings.DataAccessLayerAssemblyName;
        mRetVal.DataAccessLayerNamespace = ConfigSettings.DataAccessLayerNamespace;
        mRetVal.Id = 1;
        return mRetVal;
    }
}
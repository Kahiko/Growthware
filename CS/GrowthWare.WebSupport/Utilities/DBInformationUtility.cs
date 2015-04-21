using GrowthWare.Framework.BusinessData.BusinessLogicLayer;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.WebSupport.Utilities
{
    /// <summary>
    /// DBInformationUtility serves as the focal point for any web application needing to utilize the GrowthWare framework.
    /// Web needs such as caching are handled here.
    /// </summary>
    public static class DBInformationUtility
    {
        /// <summary>
        /// New instance of the class
        /// </summary>
        /// <returns>MDBInformation</returns>
        public static MDBInformation DBInformation()
        {
            BDBInformation mBll = new BDBInformation(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            return mBll.GetProfile;
        }
        /// <summary>
        /// Updated the information in the data store
        /// </summary>
        /// <param name="profile">MDBInformation</param>
        /// <returns>bool or exception</returns>
        public static bool UpdateProfile(MDBInformation profile) 
        {
            bool mRetVal = false;
            BDBInformation mBll = new BDBInformation(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            mRetVal = mBll.UpdateProfile(profile);
            return mRetVal;
        }
    }
}

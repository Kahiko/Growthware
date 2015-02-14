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
    public class DBInformationUtility
    {

        public static MDBInformation DBInformation()
        {
            BDBInformation mBll = new BDBInformation(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            return mBll.GetProfile();
        }

        public static bool UpdateProfile(MDBInformation profile) 
        {
            bool mRetVal = false;
            BDBInformation mBll = new BDBInformation(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
            mRetVal = mBll.UpdateProfile(profile);
            return mRetVal;
        }
    }
}

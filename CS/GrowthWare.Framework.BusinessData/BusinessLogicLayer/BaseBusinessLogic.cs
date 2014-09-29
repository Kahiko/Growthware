using GrowthWare.Framework.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.BusinessData.BusinessLogicLayer
{
    public abstract class BaseBusinessLogic
    {
        protected virtual bool isDataBaseOnline()
        {
            if (ConfigSettings.DBStatus.ToUpper(CultureInfo.InvariantCulture) == "ONLINE")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}

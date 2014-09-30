using GrowthWare.Framework.Common;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.BusinessData.BusinessLogicLayer
{
    /// <summary>
    /// Abstract (must inherit) class to add common code
    /// </summary>
    public abstract class BaseBusinessLogic
    {
        /// <summary>
        /// IsDatabaseOnline return bool based on ConfigSettings.DBStatus
        /// </summary>
        /// <returns>bool</returns>
        protected virtual bool IsDatabaseOnline()
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

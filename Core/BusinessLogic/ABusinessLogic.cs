using GrowthWare.Framework;
using System.Globalization;

namespace GrowthWare.BusinessLogic
{
    /// <summary>
    /// Abstract (must inherit) class to add common code
    /// </summary>
    public abstract class AbstractBusinessLogic
    {
        /// <summary>
        /// IsDatabaseOnline return bool based on ConfigSettings.DBStatus
        /// </summary>
        /// <returns>bool</returns>
        protected virtual bool DatabaseIsOnline()
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

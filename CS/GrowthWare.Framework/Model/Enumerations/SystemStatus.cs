using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.Model.Enumerations
{
    /// Enumeration of system status
    /// </summary>
    /// <remarks>
    /// Values match ZF_SYSTEM_STATUS in the database
    /// </remarks>
    public enum SystemStatus
    {
        /// <summary>
        /// The active
        /// </summary>
        Active = 1,
        /// <summary>
        /// The change password
        /// </summary>
        ChangePassword = 4,
        /// <summary>
        /// The disabled
        /// </summary>
        Disabled = 3,
        /// <summary>
        /// The inactive
        /// </summary>
        Inactive = 2,
        /// <summary>
        /// Not used
        /// </summary>
        None = 0
    }
}

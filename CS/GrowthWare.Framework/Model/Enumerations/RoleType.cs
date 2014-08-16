using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.Model.Enumerations
{
    /// <summary>
    /// Enumerates all role types.
    /// </summary>
    /// <remarks>
    /// Closely coupled with table ZF_PERMISSIONS.
    /// </remarks>
    [Serializable(), CLSCompliant(true)]
    public enum RoleType
    {
        AddRole = 3,
        DeleteRole = 4,
        EditRole = 2,
        ViewRole = 1,
        None = 0
    }
}

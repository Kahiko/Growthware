using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.Model.Profiles.Interfaces
{
    /// <summary>
    /// Ensures the basic properties are avalible to all Profile model objects.
    /// </summary>
    /// <remarks>
    /// If it is decided to use entities in the future then
    /// this interface should be used for the save, delete, and getitem methods.
    ///  </remarks>
    [CLSCompliant(true)]
    public interface IMSecurityInfo
    {
        string[] AddRoles
        {
            get;
        }
        string[] DeleteRoles
        {
            get;
        }
        string[] EditRoles
        {
            get;
        }
        string[] ViewRoles
        {
            get;
        }
    }
}

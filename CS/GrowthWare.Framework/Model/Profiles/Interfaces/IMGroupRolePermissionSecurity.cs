using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public interface IMGroupRolePermissionSecurity
    {
        /// <summary>
        /// Add groups that are directly assigned
        /// </summary>
        Collection<string> AddGroups
        {
            get;
        }

        /// <summary>
        /// Delete groups that are directly assigned
        /// </summary>
        Collection<string> DeleteGroups
        {
            get;
        }

        /// <summary>
        /// Edit groups that are directly assigned
        /// </summary>
        Collection<string> EditGroups
        {
            get;
        }

        /// <summary>
        /// View groups that are directly assigned
        /// </summary>
        Collection<string> ViewGroups
        {
            get;
        }

        /// <summary>
        /// Add roles that are directly assigned
        /// </summary>
        Collection<string> AssignedAddRoles
        {
            get;
        }

        /// <summary>
        /// Delete roles that are directly assigned
        /// </summary>
        Collection<string> AssignedDeleteRoles
        {
            get;
        }

        /// <summary>
        /// Edit roles that are directly assigned
        /// </summary>
        Collection<string> AssignedEditRoles
        {
            get;
        }

        /// <summary>
        /// View roles that are directly assigned
        /// </summary>
        Collection<string> AssignedViewRoles
        {
            get;
        }

        /// <summary>
        /// Add roles that are derived from roles assigned to groups
        /// </summary>
        Collection<string> DerivedAddRoles
        {
            get;
        }

        /// <summary>
        /// Delete roles that are derived from roles assigned to groups
        /// </summary>
        Collection<string> DerivedDeleteRoles
        {
            get;
        }

        /// <summary>
        /// Edit roles that are derived from roles assigned to groups
        /// </summary>
        Collection<string> DerivedEditRoles
        {
            get;
        }

        /// <summary>
        /// View roles that are derived from roles assigned to groups
        /// </summary>
        Collection<string> DerivedViewRoles
        {
            get;
        }
    }
}

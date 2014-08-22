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
        Collection<string> AddGroups
        {
            get;
        }
        Collection<string> DeleteGroups
        {
            get;
        }
        Collection<string> EditGroups
        {
            get;
        }
        Collection<string> ViewGroups
        {
            get;
        }
         Collection<string> AssignedAddRoles
        {
            get;
        }
        Collection<string> AssignedDeleteRoles
        {
            get;
        }
        Collection<string> AssignedEditRoles
        {
            get;
        }
        Collection<string> AssignedViewRoles
        {
            get;
        }
        Collection<string> DerivedAddRoles
        {
            get;
        }
        Collection<string> DerivedDeleteRoles
        {
            get;
        }
        Collection<string> DerivedEditRoles
        {
            get;
        }
        Collection<string> DerivedViewRoles
        {
            get;
        }
    }
}

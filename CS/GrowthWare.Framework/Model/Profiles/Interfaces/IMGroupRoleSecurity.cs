using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace GrowthWare.Framework.Model.Profiles.Interfaces
{
    public interface IMGroupRoleSecurity
    {
        Collection<String> AssignedRoles
        {
            get;
        }

        Collection<String> DerivedRoles
        {
            get;
        }

        Collection<String> Groups
        {
            get;
        }
    }
}

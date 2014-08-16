using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.Model.Profiles.Base
{
    /// <summary>
    /// The MBaseSecurity is a abstract class that when inherited will add 4 types of roles
    /// to your class/object.  After you have inherited the class pass a data row to the SecurityInit sub 
    /// to populate the roles.
    /// </summary>
    /// <remarks>
    /// Currently there are 4 permission roles and they are Add, Edit, View, Delete.  
    /// If you would like to extend this class do so by inheriting this class and adding your
    /// own types of roles say Moderate if your writing some sort of formum type of class.
    /// Any of the objects you create should now inherit your class and will now have all of the 
    /// roles from this class as well as the ones for yours.
    ///</remarks>
    [Serializable(), CLSCompliant(true)]
    public abstract class MBaseSecurity : MProfile, IMSecurityInfo
    {
        private string[] m_AddRoles;
        private string[] m_DeleteRoles;
        private string[] m_EditRoles;
        private string[] m_ViewRoles;
        private string m_PermissionColumn = "PERMISSIONS_SEQ_ID";

        private string m_RoleColumn = "ROLE";
        /// <summary>
        /// Init orverloads and calles mybase.init to will populate the Add, Delete, Edit, and View role properties.
        /// </summary>
        /// <param name="dataRowSecurity">A data row that must contain two columns ("PERMISSIONS_SEQ_ID","ROLE")</param>
        /// <param name="dataRowMain">A data row that contains base information</param>
        /// <remarks></remarks>
        protected void Init(DataRow dataRowMain, DataRow[] dataRowSecurity)
        {
            base.Initialize(dataRowMain);
            m_AddRoles = SplitRoles(dataRowSecurity, RoleType.AddRole);
            m_DeleteRoles = SplitRoles(dataRowSecurity, RoleType.DeleteRole);
            m_EditRoles = SplitRoles(dataRowSecurity, RoleType.EditRole);
            m_ViewRoles = SplitRoles(dataRowSecurity, RoleType.ViewRole);
        }

        public string[] AddRoles
        {
            get { return m_AddRoles; }
        }

        public string[] DeleteRoles
        {
            get { return m_DeleteRoles; }
        }

        public string[] EditRoles
        {
            get { return m_EditRoles; }
        }

        public string[] ViewRoles
        {
            get { return m_ViewRoles; }
        }

        public string PermissionColumn
        {
            get { return m_PermissionColumn; }
            set { m_PermissionColumn = value.Trim(); }
        }

        public string RoleColumn
        {
            get { return m_RoleColumn; }
            set { m_RoleColumn = value.Trim(); }
        }

        protected string[] SplitRoles(DataRow[] allRoles, RoleType moduleRoleType)
        {
            ArrayList colRoles = new ArrayList();
            DataRow row = null;
            foreach (DataRow row_loopVariable in allRoles)
            {
                row = row_loopVariable;
                if (!Convert.IsDBNull(row[m_PermissionColumn]))
                {
                    if ((RoleType)row[m_PermissionColumn] == moduleRoleType)
                    {
                        colRoles.Add(row[m_RoleColumn]);
                    }
                }
            }
            return (string[])colRoles.ToArray(typeof(string));
        }
    }
}

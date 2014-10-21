using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base;
using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Data;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
{
    interface IDGroups : IDDBInteraction
    {
        /// <summary>
        /// Gets a subset of information from the database 
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        DataTable Search(MSearchCriteria searchCriteria);

        /// <summary>
        /// Sets or gets the SecurityEntitySeqID
        /// </summary>
        int SecurityEntitySeqID { get; set; }

        /// <summary>
        /// GroupProfile
        /// </summary>
        MGroupProfile Profile { get; set; }

        /// <summary>
        /// GroupRoles
        /// </summary>
        MGroupRoles GroupRolesProfile { get; set; }

        /// <summary>
        /// Returns a DataTable of Group roles
        /// </summary>
        /// <returns>DataTable</returns>
        DataTable GetGroupRoles();

        /// <summary>
        /// Updates the Groups roles
        /// </summary>
        /// <returns>bool</returns>
        bool UpdateGroupRoles();

        /// <summary>
        /// Get's all of the groups for a given Security Entity
        /// </summary>
        /// <returns>DataTable</returns>
        DataTable GetGroupsBySecurityEntity();

        /// <summary>
        /// Adds a group to a Security Entity
        /// </summary>
        /// <returns>int</returns>
        void AddGroup();

        /// <summary>
        /// Returns a data row necessary to populate MGroupProfile
        /// </summary>
        /// <returns>DataRow</returns>
        DataRow GetProfileData();

        /// <summary>
        /// Deletes a group in a given Security Entity
        /// </summary>
        /// <returns>bool</returns>
        bool DeleteGroup();

        /// <summary>
        /// Saves this instance.
        /// </summary>
        void Save();
    }
}

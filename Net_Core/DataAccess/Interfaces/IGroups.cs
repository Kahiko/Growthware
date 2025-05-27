using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System.Data;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.Interfaces;

public interface IGroups : IDBInteraction
{

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
    Task<DataTable> GroupRoles();

    /// <summary>
    /// Updates the Groups roles
    /// </summary>
    /// <returns>bool</returns>
    Task<bool> UpdateGroupRoles();

    /// <summary>
    /// Get's all of the groups for a given Security Entity
    /// </summary>
    /// <returns>DataTable</returns>
    Task<DataTable> GroupsBySecurityEntity(int securityEntityId);

    /// <summary>
    /// Returns a data row necessary to populate MGroupProfile
    /// </summary>
    /// <returns>DataRow</returns>
    Task<DataRow> ProfileData(int securityEntityId);

    /// <summary>
    /// Deletes a group in a given Security Entity
    /// </summary>
    /// <returns>bool</returns>
    Task<bool> DeleteGroup();

    /// <summary>
    /// Saves this instance.
    /// </summary>
    Task<int> Save();
}
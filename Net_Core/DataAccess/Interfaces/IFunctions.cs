
using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using System.Data;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.Interfaces
{
    public interface IFunction : IDBInteraction
    {
        Task<DataTable> MenuTypes();

        /// <summary>
        /// Used by all methods and must be set to send parameters to the data store
        /// </summary>
        MFunctionProfile Profile { get; set; }

        /// <summary>
        /// Used by all methods and must be set to send parameters to the data store
        /// </summary>
        int SecurityEntitySeqId { get; set; }

        /// <summary>
        /// Copies the function security from the source to the target deleteing the target in the process
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        Task CopyFunctionSecurity(int source, int target, int added_Updated_By);

        /// <summary>
        /// Deletes a funciton
        /// </summary>
        Task Delete(int functionSeqId);

        /// <summary>
        /// Retrieves Function information
        /// </summary>
        /// <returns>DataRow</returns>
        Task<DataRow> GetFunction();

        /// <summary>
        /// Returns all functions associated with a given SecurityEntitySeqID.
        /// </summary>
        /// <returns>DataSet</returns>
        /// <remarks>Does not caculate security for accounts.</remarks>
        DataSet GetFunctions();

        Task<DataTable> FunctionTypes();

        Task<DataTable> GetMenuOrder(MFunctionProfile Profile);

        /// <summary>
        /// Inserts or updates account information
        /// </summary>
        /// <returns>int</returns>
        Task<int> Save();

        /// <summary>
        /// Save groups by passing a string or comma seporated groups to the database.
        /// </summary>
        Task SaveGroups(PermissionType permission);

        /// <summary>
        /// Save roles by passing a string or comma seporated roles to the database.
        /// </summary>
        Task SaveRoles(PermissionType permission);

        /// <summary>
        /// Updates the menu order.
        /// </summary>
        /// <param name="commaSeparated_Ids">A comma separated list of ids</param>
        /// <param name="profile">The profile.</param>
        Task UpdateMenuOrder(string commaSeparated_Ids, MFunctionProfile profile);
    }
}

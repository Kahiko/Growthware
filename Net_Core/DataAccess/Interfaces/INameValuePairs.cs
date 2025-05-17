using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System.Data;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.Interfaces
{
    public interface INameValuePairs : IDBInteraction
    {
        /// <summary>
        /// Gets or sets the name value pair profile.
        /// </summary>
        /// <value>The name value pair profile.</value>
        MNameValuePair NameValuePairProfile { get; set; }

        /// <summary>
        /// Gets or sets the S e_ SE q_ ID.
        /// </summary>
        /// <value>The S e_ SE q_ ID.</value>
        int SecurityEntitySeqId { get; set; }

        /// <summary>
        /// Gets or sets the account ID.
        /// </summary>
        /// <value>The account ID.</value>
        int AccountId { get; set; }

        /// <summary>
        /// Gets or sets the primary key.
        /// </summary>
        /// <value>The primary key.</value>
        int PrimaryKey { get; set; }

        /// <summary>
        /// Gets or sets the detail profile.
        /// </summary>
        /// <value>The detail profile.</value>
        MNameValuePairDetail DetailProfile { get; set; }

        /// <summary>
        /// Deletes the NVP detail.
        /// </summary>
        /// <param name="profile">The profile.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise</returns>
        Task<bool> DeleteNVPDetail(MNameValuePairDetail profile);

        /// <summary>
        /// Gets the NVP detail.
        /// </summary>
        /// <returns>DataRow.</returns>
        Task<DataRow> NameValuePairDetail();

        /// <summary>
        /// Gets the NVP details.
        /// </summary>
        /// <param name="nameValuePairSeqDetailId">The NVP seq det ID.</param>
        /// <param name="nameValuePairSeqId">The NVP seq ID.</param>
        /// <returns>DataRow.</returns>
        Task<DataRow> NameValuePairDetails(int nameValuePairSeqDetailId, int nameValuePairSeqId);

        /// <summary>
        /// Gets all NVP detail.
        /// </summary>
        /// <returns>DataTable.</returns>
        Task<DataTable> AllNameValuePairDetail();

        /// <summary>
        /// Gets all NVP detail.
        /// </summary>
        /// <param name="nameValuePairSeqId">The NVP seq ID.</param>
        /// <returns>DataTable.</returns>
        Task<DataTable> GetAllNVPDetail(int nameValuePairSeqId);

        /// <summary>
        /// Gets the groups.
        /// </summary>
        /// <param name="nameValuePairSeqId">The name value pair seq ID.</param>
        /// <returns>DataTable.</returns>
        Task<DataTable> GetGroups(int nameValuePairSeqId);

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="nameValuePairSeqId">The name value pair seq ID.</param>
        /// <returns>DataTable.</returns>
        Task<DataTable> GetRoles(int nameValuePairSeqId);

        /// <summary>
        /// Saves the NVP detail.
        /// </summary>
        /// <param name="profile">The profile.</param>
        Task<DataRow> SaveNVPDetail(MNameValuePairDetail profile);

        /// <summary>
        /// Updates the groups.
        /// </summary>
        /// <param name="NVP_ID">The NV p_ ID.</param>
        /// <param name="SecurityEntityID">The security entity ID.</param>
        /// <param name="commaSeparatedGroups">The comma separated groups.</param>
        /// <param name="nameValuePairProfile">MNameValuePair</param>
        Task UpdateGroups(int NVP_ID, int SecurityEntityID, string commaSeparatedGroups, MNameValuePair nameValuePairProfile);

        /// <summary>
        /// Updates the roles.
        /// </summary>
        /// <param name="nameValuePairId">The NV p_ ID.</param>
        /// <param name="SecurityEntityID">The security entity ID.</param>
        /// <param name="commaSeparatedRoles">The comma separated roles.</param>
        /// <param name="nameValuePairProfile">MNameValuePair</param>
        Task UpdateRoles(int nameValuePairId, int SecurityEntityID, string commaSeparatedRoles, MNameValuePair nameValuePairProfile);

        /// <summary>
        /// Gets all NVP.
        /// </summary>
        /// <returns>DataTable.</returns>
        Task<DataTable> GetAllNVP();

        /// <summary>
        /// Gets the NVP.
        /// </summary>
        /// <returns>DataRow.</returns>
        Task<DataRow> NameValuePair();

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns>System.DataRow</returns>
        Task<DataRow> Save();
    }
}

using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System.Data;

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
        bool DeleteNVPDetail(MNameValuePairDetail profile);

        /// <summary>
        /// Gets the NVP detail.
        /// </summary>
        /// <returns>DataRow.</returns>
        DataRow NameValuePairDetail();

        /// <summary>
        /// Gets the NVP details.
        /// </summary>
        /// <param name="nameValuePairSeqDetailId">The NVP seq det ID.</param>
        /// <param name="nameValuePairSeqId">The NVP seq ID.</param>
        /// <returns>DataRow.</returns>
        DataRow NameValuePairDetails(int nameValuePairSeqDetailId, int nameValuePairSeqId);

        /// <summary>
        /// Gets all NVP detail.
        /// </summary>
        /// <returns>DataTable.</returns>
        DataTable AllNameValuePairDetail();

        /// <summary>
        /// Gets all NVP detail.
        /// </summary>
        /// <param name="nameValuePairSeqId">The NVP seq ID.</param>
        /// <returns>DataTable.</returns>
        DataTable GetAllNVPDetail(int nameValuePairSeqId);

        /// <summary>
        /// Gets the groups.
        /// </summary>
        /// <param name="nameValuePairSeqId">The name value pair seq ID.</param>
        /// <returns>DataTable.</returns>
        DataTable GetGroups(int nameValuePairSeqId);

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="nameValuePairSeqId">The name value pair seq ID.</param>
        /// <returns>DataTable.</returns>
        DataTable GetRoles(int nameValuePairSeqId);

        /// <summary>
        /// Saves the NVP detail.
        /// </summary>
        /// <param name="profile">The profile.</param>
        DataRow SaveNVPDetail(MNameValuePairDetail profile);

        /// <summary>
        /// Updates the groups.
        /// </summary>
        /// <param name="NVP_ID">The NV p_ ID.</param>
        /// <param name="SecurityEntityID">The security entity ID.</param>
        /// <param name="commaSeparatedGroups">The comma separated groups.</param>
        /// <param name="nameValuePairProfile">MNameValuePair</param>
        void UpdateGroups(int NVP_ID, int SecurityEntityID, string commaSeparatedGroups, MNameValuePair nameValuePairProfile);

        /// <summary>
        /// Updates the roles.
        /// </summary>
        /// <param name="nameValuePairId">The NV p_ ID.</param>
        /// <param name="SecurityEntityID">The security entity ID.</param>
        /// <param name="commaSeparatedRoles">The comma separated roles.</param>
        /// <param name="nameValuePairProfile">MNameValuePair</param>
        void UpdateRoles(int nameValuePairId, int SecurityEntityID, string commaSeparatedRoles, MNameValuePair nameValuePairProfile);

        /// <summary>
        /// Gets all NVP.
        /// </summary>
        /// <returns>DataTable.</returns>
        DataTable GetAllNVP();

        /// <summary>
        /// Gets the NVP.
        /// </summary>
        /// <returns>DataRow.</returns>
        DataRow NameValuePair();

        /// <summary>
        /// Saves this instance.
        /// </summary>
        /// <returns>System.DataRow</returns>
        DataRow Save();

    }
}

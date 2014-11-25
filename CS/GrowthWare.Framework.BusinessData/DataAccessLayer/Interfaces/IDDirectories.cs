using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base;
using GrowthWare.Framework.Model.Profiles;
using System.Data;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
{
    /// <summary>
    /// Interface IDDirectories
    /// </summary>
    interface IDDirectories : IDDBInteraction
    {
        		/// <summary>
		/// Gets the directories.
		/// </summary>
		/// <returns>DataTable.</returns>
		DataTable Directories();

		/// <summary>
		/// Saves the specified profile.
		/// </summary>
		/// <param name="profile">The profile.</param>
		void Save(ref MDirectoryProfile profile);

		/// <summary>
		/// Gets or sets the security entity seq ID.
		/// </summary>
		/// <value>The security entity seq ID.</value>
		int SecurityEntitySeqId { get; set; }
    }
}

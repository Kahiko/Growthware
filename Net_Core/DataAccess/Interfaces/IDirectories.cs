
using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System.Data;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.Interfaces;

/// <summary>
/// Interface IDDirectories
/// </summary>
public interface IDirectories : IDBInteraction
{
	/// <summary>
	/// Gets the directories.
	/// </summary>
	/// <returns>DataTable.</returns>
	Task<DataTable> Directories();

	/// <summary>
	/// Saves the specified profile.
	/// </summary>
	/// <param name="profile">The profile.</param>
	Task Save(MDirectoryProfile profile);

	/// <summary>
	/// Gets or sets the security entity seq ID.
	/// </summary>
	/// <value>The security entity seq ID.</value>
	int SecurityEntitySeqId { get; set; }
}
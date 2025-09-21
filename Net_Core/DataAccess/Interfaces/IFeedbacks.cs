using System.Data;
using System.Threading.Tasks;
using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;

namespace GrowthWare.DataAccess.Interfaces;

/// <summary>
/// The interface the "DFeedbacks" class will implement.
/// A "DFeedbacks" will exist for each Database technology supported
/// </summary>
public interface IFeedbacks : IDBInteraction
{
	/// <summary>
	/// Used by all methods and must be set to send parameters to the datastore
	/// </summary>
	MFeedback Profile { get; set; }

	/// <summary>
	/// Retrieves the current feedback using the information provided in the Profile property.
	/// </summary>
	/// <returns>DataRow</returns>
	Task<DataRow> GetFeedback();

	/// <summary>
	/// Saves the current feedback using the information provided in the Profile property.
	/// </summary>
	Task<DataRow> SaveFeedback();
}
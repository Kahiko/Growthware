using GrowthWare.DataAccess.Interfaces.Base;

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
		string Profile {get; set;}

		/// <summary>
		/// Used by all methods and must be set to send parameters to the datastore
		/// </summary>
		int SecurityEntitySeqId { get; set; }
}
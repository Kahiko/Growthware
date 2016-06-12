using System;

namespace GrowthWare.Framework.ModelObjects.Base.Interfaces
{
	/// <summary>
	/// Ensures the basic properties are avalible to all Profile model objects.
	/// </summary>
	/// <remarks>
	/// If it is decided to use entities in the future then
	/// this interface should be used for the save, delete, and getitem methods.
	///  </remarks>
	public interface IMProfile
	{
		/// <summary>
		/// Account ID used to add
		/// </summary>
		int AddedBy
		{
			get;
			set;
		}

		/// <summary>
		/// Date the row was added.
		/// </summary>
		DateTime AddedDate
		{
			get;
			set;
		}

		/// <summary>
		/// Unique numeric identifier
		/// </summary>
		int Id
		{
			get;
			set;
		}

		/// <summary>
		/// String representation normaly unique
		/// </summary>
		string Name
		{
			get;
			set;
		}

		/// <summary>
		/// Account ID used to update
		/// </summary>
		int UpdatedBy
		{
			get;
			set;
		}

		/// <summary>
		/// The date lasted updated
		/// </summary>
		DateTime UpdatedDate
		{
			get;
			set;
		}
	}
}

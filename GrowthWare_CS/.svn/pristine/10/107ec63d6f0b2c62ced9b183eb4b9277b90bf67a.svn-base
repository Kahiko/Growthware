using System.Data;
using GrowthWare.Framework.DataAccessLayer.Interfaces.Base;
using GrowthWare.Framework.Model.Profiles;
using System.Collections;

namespace GrowthWare.Framework.DataAccessLayer.Interfaces
{
	interface IDClientChoices : IDDBInteraction
	{
		/// <summary>
		/// Retrieves a row of data given the account
		/// </summary>
		/// <param name="account">String</param>
		/// <returns>DataRow</returns>
		/// <remarks></remarks>
		DataRow GetChoices(ref string account);

		/// <summary>
		/// Save the client choices
		/// </summary>
		/// <param name="clientChoicesStateHashTable">Hashtable</param>
		/// <remarks></remarks>
		void Save(ref Hashtable clientChoicesStateHashTable);
	}
}

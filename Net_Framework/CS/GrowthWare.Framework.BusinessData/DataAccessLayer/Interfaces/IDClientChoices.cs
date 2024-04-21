using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base;
using System.Collections;
using System.Data;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
{
    interface IDClientChoices : IDDBInteraction
    {
        /// <summary>
        /// Retrieves a row of data given the account
        /// </summary>
        /// <param name="account">String</param>
        /// <returns>DataRow</returns>
        /// <remarks></remarks>
        DataRow GetChoices(string account);

        /// <summary>
        /// Save the client choices
        /// </summary>
        /// <param name="clientChoicesStateHashtable">Hashtable</param>
        /// <remarks></remarks>
        void Save(Hashtable clientChoicesStateHashtable);
    }
}

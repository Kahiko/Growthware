﻿using GrowthWare.DataAccess.Interfaces.Base;
using System.Collections;
using System.Data;

namespace GrowthWare.DataAccess.Interfaces
{
    public interface IClientChoices : IDBInteraction
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

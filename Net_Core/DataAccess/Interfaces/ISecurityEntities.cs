﻿using GrowthWare.DataAccess.Interfaces.Base;
using GrowthWare.Framework.Models;
using System.Data;

namespace GrowthWare.DataAccess.Interfaces
{
    public interface ISecurityEntities : IDBInteraction
    {
        /// <summary>
        /// Retrieves all Security Entities as a data table.
        /// </summary>
        /// <returns>DataTable</returns>
        /// <remarks></remarks>
        DataTable GetSecurityEntities();

        /// <summary>
        /// Retrieves security entities for a given account.
        /// </summary>
        /// <param name="account">String</param>
        /// <param name="SecurityEntityID">int or Integer</param>
        /// <param name="isSecurityEntityAdministrator">Boolean or bool</param>
        /// <returns>Datatable</returns>
        /// <remarks></remarks>
        DataTable GetSecurityEntities(string account, int SecurityEntityID, bool isSecurityEntityAdministrator);

        /// <summary>
        /// Saves security entity information to the datastore.
        /// </summary>
        /// <param name="profile">MSecurityEntity</param>
        /// <remarks></remarks>
        int Save(MSecurityEntity profile);

        /// <summary>
        /// Gets the valid security entities.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="SecurityEntityID">The security entity id.</param>
        /// <param name="isSystemAdmin">if set to <c>true</c> [is system admin].</param>
        /// <returns>DataTable.</returns>
        DataTable GetValidSecurityEntities(string account, int SecurityEntityID, bool isSystemAdmin);
    }
}

using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base;
using GrowthWare.Framework.Model.Profiles;
using System.Data;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
{
    interface IDSecurityEntity : IDDBInteraction
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
        /// <param name="securityEntityID">int or Integer</param>
        /// <param name="isSecurityEntityAdministrator">Boolean or bool</param>
        /// <returns>Datatable</returns>
        /// <remarks></remarks>
        DataTable GetSecurityEntities(string account, int securityEntityID, bool isSecurityEntityAdministrator);


        DataTable Search(MSearchCriteria searchCriteria);

        /// <summary>
        /// Saves security entity information to the datastore.
        /// </summary>
        /// <param name="profile">MSecurityEntityProfile</param>
        /// <remarks></remarks>
        int Save(MSecurityEntityProfile profile);

        /// <summary>
        /// Gets the valid security entities.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="securityEntityId">The security entity id.</param>
        /// <param name="isSystemAdmin">if set to <c>true</c> [is system admin].</param>
        /// <returns>DataTable.</returns>
        DataTable GetValidSecurityEntities(string account, int securityEntityId, bool isSystemAdmin);
    }
}

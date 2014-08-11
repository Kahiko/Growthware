using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base;
using GrowthWare.Framework.Model.Enumerations;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
{
    /// <summary>
    /// IDAccount defines the contract for any
    /// class implementing the interface.
    /// </summary>
    interface IDAccount : IDDBInteraction
	{
		/// <summary>
		/// Used by all methds and must be set to send parameters to the datastore
		/// </summary>
		MAccountProfile Profile {get; set;}

		/// <summary>
		/// Used by all methds and must be set to send parameters to the datastore
		/// </summary>
		int SecurityEntitySeqId { get; set; }

		/// <summary>
		/// Deletes an account
		/// </summary>
		void Delete();

		/// <summary>
		/// Retrieves Account information
		/// </summary>
		/// <returns>DataRow</returns>
		DataRow GetAccount { get; }

		/// <summary>
		/// Returns all accounts associated with a given SecurityEntitySeqID.
		/// </summary>
		/// <returns>DataTable</returns>
		/// <remarks>Does not caculate security for accounts.</remarks>
		DataTable GetAccounts { get; }

		/// <summary>
		/// Returns all roles associated with a given SecurityEntitySeqID.
		/// </summary>
		/// <returns>DataTable</returns>
		DataTable Groups();

		/// <summary>
		/// Retrieves menu data for a given account and MenuType
		/// </summary>
		/// <param name="account">String</param>
		/// <param name="menuType">MenuType</param>
		/// <returns>DataTable</returns>
		/// <remarks></remarks>
		DataTable GetMenu(string account, MenuType menuType);

		/// <summary>
		/// Returns all groups associated with a given SecurityEntitySeqID.
		/// </summary>
		/// <returns>DataTable</returns>
		DataTable Roles();

		/// <summary>
		/// Returns all roles either direct association or by association via
		/// groups.
		/// </summary>
		/// <returns>DataTable</returns>
		DataTable Security();

		/// <summary>
		/// Gets a subset of information from the database 
		/// </summary>
		/// <param name="searchCriteria"></param>
		/// <returns></returns>
		DataTable Search(MSearchCriteria searchCriteria);

		/// <summary>
		/// Inserts or updates account information
		/// </summary>
		/// <returns>int</returns>
		int Save();

		/// <summary>
		/// Save groups by passing a string or comma seporated groups to the database.
		/// </summary>
		void SaveGroups();

		/// <summary>
		/// Save roles by passing a string or comma seporated rolse to the database.
		/// </summary>
		void SaveRoles();
    }
}

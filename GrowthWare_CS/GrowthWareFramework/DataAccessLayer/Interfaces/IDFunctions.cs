using GrowthWare.Framework.DataAccessLayer.Interfaces.Base;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.Framework.Model.Enumerations;
using System.Data;

namespace GrowthWare.Framework.DataAccessLayer.Interfaces
{
	interface IDFunctions : IDDBInteraction
	{
		/// <summary>
		/// Used by all methds and must be set to send parameters to the datastore
		/// </summary>
		MFunctionProfile Profile { get; set; }

		/// <summary>
		/// Used by all methds and must be set to send parameters to the datastore
		/// </summary>
		int SecurityEntitySeqID{get;set;}

		/// <summary>
		/// Deletes an account
		/// </summary>
		void Delete();

		/// <summary>
		/// Retrieves Function information
		/// </summary>
		/// <returns>DataRow</returns>
		DataRow GetFunction {get;}
		/// <summary>
		/// Returns all functions associated with a given SecurityEntitySeqID.
		/// </summary>
		/// <returns>DataSet</returns>
		/// <remarks>Does not caculate security for accounts.</remarks>
		DataSet GetFunctions { get; }

		// /// <summary>
		// /// Returns all groups associated with a given SecurityEntitySeqID and permission.
		// /// </summary>
		// /// <param name="permission">PermissionTypes</param>
		// /// <returns>DataTable</returns>
		//DataTable GetGroups(PermissionTypes permission);

		// /// <summary>
		// /// Returns all roles associated with a given SecurityEntitySeqID and permission.
		// /// </summary>
		// /// <param name="permission">PermissionTypes</param>
		// /// <returns>DataTable</returns>
		//DataTable GetRoles(PermissionTypes permission);

		// /// <summary>
		// /// Returns all roles for all permissions all functions
		// /// </summary>
		// /// <returns></returns>
		// /// <remarks></remarks>
		//DataTable GetAllRoles();

		// /// <summary>
		// /// Returns all groups for all permissions all functions
		// /// </summary>
		// /// <returns></returns>
		// /// <remarks></remarks>
		//DataTable GetAllGroups();

		// /// <summary>
		// /// Returns all roles either direct association or by association via
		// /// groups.
		// /// </summary>
		// /// <returns>DataTable</returns>
		//DataTable GetSecurity();

		/// <summary>
		/// Inserts or updates account information
		/// </summary>
		/// <returns>int</returns>
		int Save();

		/// <summary>
		/// Save groups by passing a string or comma seporated groups to the database.
		/// </summary>
		void SaveGroups(PermissionType permission);

		/// <summary>
		/// Save roles by passing a string or comma seporated roles to the database.
		/// </summary>
		void SaveRoles(PermissionType permission);

	}
}

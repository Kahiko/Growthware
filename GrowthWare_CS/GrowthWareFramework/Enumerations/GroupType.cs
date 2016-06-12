
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
namespace GrowthWare.Framework.Enumerations
{
	/// <summary>
	/// Enumerates all role types.
	/// </summary>
	/// <remarks>
	/// Closely coupled with table ZFC_FUNCTION_TYPES.
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
	public enum GroupType
	{
		AddRole = 3,
		DeleteRole = 4,
		EditRole = 2,
		ViewRole = 1
	}
}
//RoleType 

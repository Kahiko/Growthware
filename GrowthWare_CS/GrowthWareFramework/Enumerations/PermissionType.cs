
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
namespace GrowthWare.Framework.Enumerations
{
	/// <summary>
	/// Enumeration of permission Types
	/// </summary>
	/// <remarks>
	/// Values match ZF_PERMISSIONS in the database
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
	public enum PermissionType
	{
		Add = 3,
		Delete = 4,
		Edit = 2,
		View = 1
	}
}

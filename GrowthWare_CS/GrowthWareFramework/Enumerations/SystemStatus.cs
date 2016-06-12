
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
namespace GrowthWare.Framework.Enumerations
{
	/// <summary>
	/// Enumeration of system status
	/// </summary>
	/// <remarks>
	/// Values match ZF_SYSTEM_STATUS in the database
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
	public enum SystemStatus
	{
		Active = 1,
		ChangePassword = 4,
		Disabled = 3,
		Inactive = 2
	}
}

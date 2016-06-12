
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
namespace GrowthWare.Framework.Model.Enumerations
{
	/// <summary>
	/// Enumeration of menu types
	/// </summary>
	/// <remarks>
	/// Values match ZF_NAVIGATION_TYPE in the database
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
	public enum MenuType
	{
		Hierarchical = 3,
		Horizontal = 1,
		Vertical = 2
	}
}

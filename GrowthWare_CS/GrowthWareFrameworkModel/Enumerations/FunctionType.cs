
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
namespace GrowthWare.Framework.Enumerations
{
	/// <summary>
	/// Enumeration of menu types
	/// </summary>
	/// <remarks>
	/// Values match ZF_FUNCTION_TYPES in the database
	/// </remarks>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
	public enum FunctionType
	{
		Modules = 1,
		Security = 2,
		Menu_Item = 3,
		HTML_Content = 4,
		Content = 5
	}
}

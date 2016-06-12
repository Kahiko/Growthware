
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Web;
namespace GrowthWare.Framework.Enumerations
{
	/// <summary>
	/// Enumeration of allow html
	/// </summary>
	/// <remarks>
	/// 
	/// </remarks>
	public enum AllowHtml
	{
		/// <summary>
		/// No Html Edit is allowed
		/// </summary>
		/// <remarks></remarks>
		None = 0,
		/// <summary>
		/// Limited Html Edit is allowed
		/// </summary>
		/// <remarks></remarks>
		Limited = 1,
		/// <summary>
		/// Full Html Edit is allowed
		/// </summary>
		/// <remarks></remarks>
		Full = 2
	}
}

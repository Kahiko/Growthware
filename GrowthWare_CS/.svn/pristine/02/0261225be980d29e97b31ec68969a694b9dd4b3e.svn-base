using System;
using System.Reflection;

namespace GrowthWare.Framework.Common
{
	/// <summary>
	/// The FactoryObject will create an instance of an object from any 
	/// Assembley give the assembly name, namespace, and the object/class name.
	/// </summary>
	/// <remarks>
	/// None
	/// </remarks>
	public sealed class FactoryObject
	{
		private FactoryObject()	{}

		/// <summary>
		/// Creates an instance of an object.
		/// </summary>
		/// <param name="theAssemblyName">
		/// The name of the assembley (DLL).  Must be 
		/// included in your solution in order to find the file.
		/// </param>
		/// <param name="theNamespace">
		/// The name space where the class is located.
		/// </param>
		/// <param name="theClassName">
		/// The name of the class you need an instance of.
		/// </param>
		/// <returns>An object</returns>
		/// <remarks></remarks>
		public static object Create(string theAssemblyName, string theNamespace, string theClassName)
		{
			if ((theAssemblyName == null))
			{
				throw new ArgumentNullException("theAssemblyName", "theAssemblyName cannot be a null reference (Nothing in Visual Basic)");
			}
			if (theNamespace == null)
			{
				throw new ArgumentNullException("theNamespace", "theNamespace cannot be a null reference (Nothing in Visual Basic)");
			}
			if (theClassName == null)
			{
				throw new ArgumentNullException("theClassName", "theClassName cannot be a null reference (Nothing in Visual Basic)");
			}
			object mReturnObject = null;
			try
			{
				Assembly TheAssembly = System.Reflection.Assembly.Load(theAssemblyName);
				if (theNamespace.Length > 0)
				{
					mReturnObject = TheAssembly.CreateInstance(theNamespace + "." + theClassName, true);
				}
				else
				{
					mReturnObject = TheAssembly.CreateInstance(theClassName, true);
				}
				if (mReturnObject == null)
				{
					string exMessage = "FactoryObject :: Create() theAssemblyName: " + theAssemblyName + " theNamespace: " + theNamespace + " theClassName: " + theClassName + Environment.NewLine;
					Exception factoryEx = new Exception(exMessage);
					System.Diagnostics.Trace.WriteLine(factoryEx.ToString());
					throw factoryEx;
				}
			}
			catch
			{
				throw;
			}
			return mReturnObject;
		}
	}
}

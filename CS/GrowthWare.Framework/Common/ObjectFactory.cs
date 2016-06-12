using System;
using System.Reflection;

namespace GrowthWare.Framework.Common
{
    /// <summary>
    /// The ObjectFactory will create an instance of an object from any 
	/// Assembley give the assembly name, namespace, and the object/class name.
	/// </summary>
	/// <remarks>
	/// None
	/// </remarks>
    public sealed class ObjectFactory
    {
        private ObjectFactory() { }

		/// <summary>
		/// Creates an instance of an object.
		/// </summary>
		/// <param name="assemblyName">
		/// The name of the assembley (DLL).  Must be 
		/// included in your solution in order to find the file.
		/// </param>
		/// <param name="theNamespace">
		/// The name space where the class is located.
		/// </param>
		/// <param name="className">
		/// The name of the class you need an instance of.
		/// </param>
		/// <returns>An object</returns>
		/// <remarks></remarks>
		public static object Create(string assemblyName, string theNamespace, string className)
		{
			if ((assemblyName == null))
			{
                throw new ArgumentNullException("assemblyName", "assemblyName cannot be a null reference (Nothing in Visual Basic)!");
			}
			if (theNamespace == null)
			{
				throw new ArgumentNullException("theNamespace", "theNamespace cannot be a null reference (Nothing in Visual Basic)!");
			}
			if (className == null)
			{
                throw new ArgumentNullException("className", "className cannot be a null reference (Nothing in Visual Basic)!");
			}
			object mReturnObject = null;
			try
			{
				Assembly mAssembly = System.Reflection.Assembly.Load(assemblyName);
				if (theNamespace.Length > 0)
				{
					mReturnObject = mAssembly.CreateInstance(theNamespace + "." + className, true);
				}
				else
				{
					mReturnObject = mAssembly.CreateInstance(className, true);
				}
				if (mReturnObject == null)
				{
                    string exMessage = string.Concat("Object ", theNamespace, ".", className, " could not be created from assembly ", assemblyName, System.Environment.NewLine);
                    Exception factoryEx = new ObjectFactoryException(exMessage);
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

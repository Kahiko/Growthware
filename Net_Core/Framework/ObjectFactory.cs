using System;
using System.Linq;
using System.Reflection;

namespace GrowthWare.Framework
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
		/// The name space where the class is located (can be empty if the object is not in a namespace)
		/// </param>
		/// <param name="className">
		/// The name of the class you need an instance of.
		/// </param>
		/// <param name="constructorArgs">
		// Optional constructor arguments.
		// </param>
		/// <returns>An object of the className from the statied assembly</returns>
		/// <remarks></remarks>
		public static object Create(string assemblyName, string theNamespace, string className, params object[] constructorArgs)
		{
			if ((assemblyName == null))
			{
                throw new ArgumentNullException(nameof(assemblyName), "assemblyName cannot be a null reference (Nothing in Visual Basic)!");
			}
			if (theNamespace == null)
			{
				throw new ArgumentNullException(nameof(theNamespace), "theNamespace cannot be a null reference (Nothing in Visual Basic)!");
			}
			if (className == null)
			{
                throw new ArgumentNullException(nameof(className), "className cannot be a null reference (Nothing in Visual Basic)!");
			}

			object mReturnObject = null;

			try
			{
				Assembly mAssembly = System.Reflection.Assembly.Load(assemblyName);
                // If no constructor arguments are provided, use the parameterless constructor
                if (constructorArgs.Length == 0)
                {
					// Allow for empty namespace
					mReturnObject = mAssembly.CreateInstance(theNamespace + "." + className) ?? mAssembly.CreateInstance(className, true);
                }
				else
				{
					Type mType = mAssembly.GetType(theNamespace + "." + className) ?? mAssembly.GetType(className, true);
					if (mType == null)
					{
						string mMsg = $"Type '{theNamespace}.{className}' not found in assembly '{assemblyName}'.";
						if (string.IsNullOrWhiteSpace(theNamespace))
						{
							mMsg = $"Type '{className}' not found in assembly '{assemblyName}'.";
						}

						throw new ArgumentException(mMsg);
					}

					// Check for a matching constructor
					var mConstructorInfo = mType.GetConstructor(constructorArgs.Select(arg => arg?.GetType() ?? typeof(object)).ToArray());
					
					if (mConstructorInfo != null)
					{
						// Create an instance using the assembly's CreateInstance method
						mReturnObject = mAssembly.CreateInstance(theNamespace + "." + className, true, BindingFlags.CreateInstance, null, constructorArgs, null, null);
					}
				}

				if (mReturnObject == null)
				{
					string exMessage = $"Object '{theNamespace}.{className}' could not be created from assembly '{assemblyName}'.";
                    ObjectFactoryException factoryEx = new(exMessage);
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

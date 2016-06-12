using System;
using System.Collections;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.DataAccessLayer.Interfaces;
using GrowthWare.Framework.Model.Profiles;

namespace GrowthWare.Framework.BusinessLogicLayer
{
	/// <summary>
	/// Process business logic for functions
	/// </summary>
	/// <remarks>
	/// <![CDATA[
	/// MSecurityEntityProfile can be found in the GrowthWare.Framework.ModelObjects namespace.  
	/// 
	/// The following properties are necessary for correct business logic operation.
	/// .ConnctionString
	/// .DALName
	/// .DALNameSpace
	/// ]]>
	/// </remarks>
	/// <example> This sample shows how to create an instance of the class.
	/// <code language="VB.NET">
	/// <![CDATA[
	/// Dim myBll as new BClientChoices(mySecurityEntityProfile, ConfigSettings.CentralManagement)
	/// ]]>
	/// </code>
	/// </example>
	public class BClientChoices
	{
		private static IDClientChoices m_DClientChoices;

		/// <summary>
		/// Private BClientChoices() to ensure only new instances with passed parameters is used.
		/// </summary>
		/// <remarks></remarks>
		private BClientChoices()
		{
		}

		/// <summary>
		/// Parameters are need to pass along to the factory for correct connection to the desired datastore.
		/// </summary>
		/// <param name="SecurityEntityProfile">The Security Entity profile used to obtain the DAL name, DAL name space, and the Connection String</param>
		/// <param name="CentralManagement">Boolean value indicating if the system is being used to manage multiple database instances.</param>
		/// <remarks></remarks>
		/// <example> This sample shows how to create an instance of the class.
		/// <code language="VB.NET">
		/// <![CDATA[
		/// MSecurityEntityProfile mSecurityEntityProfile = MSecurityEntityProfile = New MSecurityEntityProfile();
		/// mSecurityEntityProfile.ID = ConfigSettings.DefaultSecurityEntityID;
		/// mSecurityEntityProfile.DAL = ConfigSettings.DAL;
		/// mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL);
		/// mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL);
		/// mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString;
		/// 
		/// BClientChoices mBClientChoices = New BClientChoices(mSecurityEntityProfile, ConfigSettings.CentralManagement);
		/// ]]>
		/// </code>
		/// <code language="C#">
		/// <![CDATA[
		/// Dim mSecurityEntityProfile As MSecurityEntityProfile = New MSecurityEntityProfile()
		/// mSecurityEntityProfile.ID = ConfigSettings.DefaultSecurityEntityID
		/// mSecurityEntityProfile.DAL = ConfigSettings.DAL
		/// mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL)
		/// mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL)
		/// mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString
		/// 
		/// Dim mBClientChoices As BClientChoices = New BClientChoices(mSecurityEntityProfile, ConfigSettings.CentralManagement)
		/// ]]>
		/// </code>
		/// </example>
		public BClientChoices(ref MSecurityEntityProfile SecurityEntityProfile, bool CentralManagement)
		{
			if(SecurityEntityProfile == null) 
			{
				throw new ArgumentException("The securityEntityProfile and not be null!");
			}
			if(CentralManagement)
			{
				if (m_DClientChoices == null)
				{
					m_DClientChoices = (IDClientChoices)FactoryObject.Create(SecurityEntityProfile.DALAssemblyName, SecurityEntityProfile.DALNamespace, "DClientChoices");
				}
			}
			else
			{
				m_DClientChoices = (IDClientChoices)FactoryObject.Create(SecurityEntityProfile.DALAssemblyName, SecurityEntityProfile.DALNamespace, "DClientChoices");
			}

			m_DClientChoices.ConnectionString = SecurityEntityProfile.ConnectionString;
		}

		/// <summary>
		/// Retrieves a data row from the data store and populates a MClientChoicesState object.
		/// </summary>
		/// <param name="account">The desired account in which to base the MClientChoicesState model object</param>
		/// <returns>A populated MClientChoicesState</returns>
		/// <remarks>None.</remarks>
		public MClientChoicesState GetClientChoicesState(string account)
		{
			return new MClientChoicesState(m_DClientChoices.GetChoices(ref account));
		}

		/// <summary>
		/// Saves the choices a client may have made during usage of the application.
		/// </summary>
		/// <param name="clientChoicesState">A populated MClientChoicesState object.</param>
		/// <remarks>MClientChoicesState can be found in the GrowthWare.Framework.ModelObjects namespace.</remarks>
		public void Save(ref MClientChoicesState clientChoicesState)
		{
			Hashtable mChoices = clientChoicesState.ChoicesHashtable;
			m_DClientChoices.Save(ref mChoices);
		}
	}
}

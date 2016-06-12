using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.DataAccessLayer;
using GrowthWare.Framework.DataAccessLayer.Interfaces;
using GrowthWare.Framework.Model.Profiles;
using GrowthWare.Framework.Model.Enumerations;

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
	/// Dim myBll as new BFunctions(mSecurityEntityProfile, ConfigSettings.CentralManagement)
	/// ]]>
	/// </code>
	/// </example>
	public class BFunctions
	{
		private static IDFunctions m_DFunctions;

		/// <summary>
		/// Private BFunctions() to ensure only new instances with passed parameters is used.
		/// </summary>
		/// <remarks></remarks>
		private BFunctions()
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
		/// BFunctions mBFunction = New BFunctions(mSecurityEntityProfile, ConfigSettings.CentralManagement);
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
		/// Dim mBFunction As BFunctions = New BFunctions(mSecurityEntityProfile, ConfigSettings.CentralManagement)
		/// ]]>
		/// </code>
		/// </example>
		public BFunctions(MSecurityEntityProfile SecurityEntityProfile, bool CentralManagement)
		{
			if(SecurityEntityProfile == null) 
			{
				throw new ArgumentException("The securityEntityProfile and not be null!");
			}
			if(CentralManagement)
			{
				if (m_DFunctions == null)
				{
					m_DFunctions = (IDFunctions)FactoryObject.Create(SecurityEntityProfile.DALAssemblyName, SecurityEntityProfile.DALNamespace, "DFunctions");
				}
			}
			else
			{
				m_DFunctions = (IDFunctions)FactoryObject.Create(SecurityEntityProfile.DALAssemblyName, SecurityEntityProfile.DALNamespace, "DFunctions");
			}

			m_DFunctions.ConnectionString = SecurityEntityProfile.ConnectionString;
			m_DFunctions.SecurityEntitySeqID = SecurityEntityProfile.Id;
		}

		/// <summary>
		/// Returns a collection of MFunctionProfile objects for the given
		/// security entity.
		/// </summary>
		/// <param name="securityEntitySeqID">Integer</param>
		/// <returns>Collection(of MFunctionProfile)</returns>
		/// <remarks></remarks>
		public Collection<MFunctionProfile> GetFunctions(int securityEntitySeqID) 
		{
			Collection<MFunctionProfile> mRetVal = new Collection<MFunctionProfile>();
			DataSet mDSFunctions = null;
			try
			{
				m_DFunctions.Profile = new MFunctionProfile();
				m_DFunctions.SecurityEntitySeqID = securityEntitySeqID; 
				mDSFunctions = m_DFunctions.GetFunctions;
				bool mHasAssingedRoles = false;
				bool mHasGroups = false;
				if(mDSFunctions.Tables[1].Rows.Count > 0) mHasAssingedRoles = true;
				if(mDSFunctions.Tables[2].Rows.Count > 0)mHasGroups = true;
				DataRow[] mGroups = null;
				DataRow[] mAssignedRoles = null;
				DataRow[] mDerivedRoles = null;
				
				foreach (DataRow item in mDSFunctions.Tables["Functions"].Rows)
				{
					mDerivedRoles = item.GetChildRows("DerivedRoles");
					mAssignedRoles = null;
					if(mHasAssingedRoles) mAssignedRoles = item.GetChildRows("AssignedRoles");
					mGroups = null;
					if(mHasGroups) mGroups = item.GetChildRows("Groups");
					MFunctionProfile mProfile = new MFunctionProfile(item, mDerivedRoles, mAssignedRoles, mGroups);
					mRetVal.Add(mProfile);
				}
			}
			catch (Exception)
			{
				throw;
			}
			return mRetVal;
		}

		/// <summary>
		/// Save Function information to the database
		/// </summary>
		/// <param name="profile">MFunctionProfile</param>
		/// <param name="saveGroups">bool</param>
		/// <param name="saveRoles">bool</param>
		/// <returns>int</returns>
		public int Save(ref MFunctionProfile profile, bool saveGroups, bool saveRoles) 
		{
			profile.Id = m_DFunctions.Save();
			if (saveGroups) 
			{
				m_DFunctions.SaveGroups(PermissionType.Add);
				m_DFunctions.SaveGroups(PermissionType.Delete);
				m_DFunctions.SaveGroups(PermissionType.Edit);
				m_DFunctions.SaveGroups(PermissionType.View);
			}
			if (saveRoles) 
			{
				m_DFunctions.SaveRoles(PermissionType.Add);
				m_DFunctions.SaveRoles(PermissionType.Delete);
				m_DFunctions.SaveRoles(PermissionType.Edit);
				m_DFunctions.SaveRoles(PermissionType.View);
			}
			m_DFunctions.Profile = profile;
			return profile.Id;
			
		}
	}
}

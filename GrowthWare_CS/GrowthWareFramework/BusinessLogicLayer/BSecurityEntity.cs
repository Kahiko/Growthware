using System;
using System.Collections.ObjectModel;
using System.Data;
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
	/// MSecurityEntityProfile mSecurityEntityProfile = MSecurityEntityProfile = New MSecurityEntityProfile();
	/// mSecurityEntityProfile.ID = ConfigSettings.DefaultSecurityEntityID;
	/// mSecurityEntityProfile.DAL = ConfigSettings.DAL;
	/// mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL);
	/// mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL);
	/// mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString;
	/// 
	/// Dim myBll as new BSecurityEntity(mySecurityEntityProfile, ConfigSettings.CentralManagement)
	/// ]]>
	/// </code>
	/// <code language="C#">
	/// <![CDATA[
	/// MSecurityEntityProfile mSecurityEntityProfile = new MSecurityEntityProfile();
	/// mSecurityEntityProfile.ID = ConfigSettings.DefaultSecurityEntityID;
	/// mSecurityEntityProfile.DAL = ConfigSettings.DAL;
	/// mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL);
	/// mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL);
	/// mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString;
	/// 
	/// BSecurityEntity mBSecurityEntity = New BSecurityEntity(mSecurityEntityProfile, ConfigSettings.CentralManagement);
	/// ]]>
	/// </code>	/// </example>
	public class BSecurityEntity
	{
		private static IDSecurityEntity m_DSecurityEntity;

		/// <summary>
		/// Private constructor to ensure only new instances with passed parameters is used.
		/// </summary>
		/// <remarks></remarks>
		private BSecurityEntity() { }

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
		/// Dim mBClientChoices As BClientChoices = New BClientChoices(mSecurityEntityProfile, ConfigSettings.CentralManagement)
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
		/// BClientChoices mBClientChoices = new BClientChoices(mSecurityEntityProfile, ConfigSettings.CentralManagement);
		/// ]]>
		/// </code>
		/// </example>
		public BSecurityEntity(MSecurityEntityProfile SecurityEntityProfile, bool CentralManagement)
		{
			if(SecurityEntityProfile == null) 
			{
				throw new ArgumentException("The securityEntityProfile and not be null!");
			}
			if(CentralManagement)
			{
				if (m_DSecurityEntity == null)
				{
					m_DSecurityEntity = (IDSecurityEntity)FactoryObject.Create(SecurityEntityProfile.DALAssemblyName, SecurityEntityProfile.DALNamespace, "DSecurityEntity");
				}
			}
			else
			{
				m_DSecurityEntity = (IDSecurityEntity)FactoryObject.Create(SecurityEntityProfile.DALAssemblyName, SecurityEntityProfile.DALNamespace, "DSecurityEntity");
			}

			m_DSecurityEntity.ConnectionString = SecurityEntityProfile.ConnectionString;
		}

		/// <summary>
		/// Returns a collection of MSecurityEntityProfile objects for the given.
		/// </summary>
		/// <returns>
		///		Collection of MSecurityEntityProfile
		///	</returns>
		/// <remarks></remarks>
		public Collection<MSecurityEntityProfile> GetSecurityEntities(){
			Collection<MSecurityEntityProfile> mRetVal = new Collection<MSecurityEntityProfile>();
			DataTable mDataTable = null;
			try
			{
				mDataTable = m_DSecurityEntity.GetSecurityEntities();
				foreach (DataRow item in mDataTable.Rows)
				{
					MSecurityEntityProfile mProfile = new MSecurityEntityProfile(item);
					mRetVal.Add(mProfile);
				}
			}
			catch(Exception)
			{
				throw;
			}
			finally
			{
				if(mDataTable != null)
				{
					mDataTable.Dispose();
				}
			}
			return mRetVal;
		}

		/// <summary>
		/// Save Function information to the database
		/// </summary>
		/// <param name="profile">MSecurityEntityProfile</param>
		/// <returns>Integer</returns>
		public int Save(ref MSecurityEntityProfile profile){
			profile.Id = -1;
			m_DSecurityEntity.Save(ref profile);
			return profile.Id;
		}
	}
}

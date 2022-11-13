using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace GrowthWare.BusinessLogic
{
    /// <summary>
    /// Process business logic for Directories
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// MSecurityEntity can be found in the GrowthWare.Framework.Model.Profiles namespace.  
    /// 
    /// The following properties are necessary for correct business logic operation.
    /// .ConnectionString
    /// .DALName
    /// .DALNameSpace
    /// ]]>
    /// </remarks>
    public class BDirectories : AbstractBusinessLogic
    {
        private IDirectories m_DDirectories;

        /// <summary>
        /// Private BDirectories() to ensure only new instances with passed parameters is used.
        /// </summary>
        /// <remarks></remarks>
        private BDirectories()
        {
        }

        /// <summary>
        /// Parameters are need to pass along to the factory for correct connection to the desired datastore.
        /// </summary>
        /// <param name="securityEntityProfile">The Security Entity profile used to obtain the DAL name, DAL name space, and the Connection String</param>
        /// <param name="centralManagement">Boolean value indicating if the system is being used to manage multiple database instances.</param>
        /// <remarks></remarks>
        /// <example> This sample shows how to create an instance of the class.
        /// <code language="VB.NET">
        /// <![CDATA[
        /// MSecurityEntity MSecurityEntity = MSecurityEntity = New MSecurityEntity();
        /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID;
        /// MSecurityEntity.DAL = ConfigSettings.DAL;
        /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL);
        /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL);
        /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString;
        /// 
        /// BDirectories mBAccount = BDirectories = New BDirectories(MSecurityEntity, ConfigSettings.CentralManagement);
        /// ]]>
        /// </code>
        /// <code language="C#">
        /// <![CDATA[
        /// Dim MSecurityEntity As MSecurityEntity = New MSecurityEntity()
        /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID
        /// MSecurityEntity.DAL = ConfigSettings.DAL
        /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL)
        /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL)
        /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString
        /// 
        /// Dim mBAccount As BDirectories = New BDirectories(MSecurityEntity, ConfigSettings.CentralManagement)
        /// ]]>
        /// </code>
        /// </example>
        public BDirectories(MSecurityEntity securityEntityProfile, bool centralManagement)
        {
            if (securityEntityProfile == null)
            {
                throw new ArgumentException("The securityEntityProfile and not be null!");
            }
            if (centralManagement)
            {
                if (m_DDirectories == null)
                {
                    m_DDirectories = (IDirectories)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DDirectories");
                }
            }
            else
            {
                m_DDirectories = (IDirectories)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DDirectories");
            }

            m_DDirectories.ConnectionString = securityEntityProfile.ConnectionString;
            m_DDirectories.SecurityEntitySeqId = securityEntityProfile.Id;
        }

        /// <summary>
        /// Gets the directories.
        /// </summary>
        /// <returns>Collection{MDirectoryProfile}.</returns>
        public Collection<MDirectoryProfile> Directories()
        {
            Collection<MDirectoryProfile> mRetVal = new Collection<MDirectoryProfile>();
            if (DatabaseIsOnline()) 
            {
                DataTable mDataTable = m_DDirectories.Directories();
                foreach (DataRow mDataRow in mDataTable.Rows)
                {
                    MDirectoryProfile mProfile = new MDirectoryProfile(mDataRow);
                    mRetVal.Add(mProfile);
                }
            }
            return mRetVal;
        }

        /// <summary>
        /// Saves the specified profile.
        /// </summary>
        /// <param name="profile">The profile.</param>
        public void Save(MDirectoryProfile profile)
        {
            if(DatabaseIsOnline()) m_DDirectories.Save(profile);
        }
    }
}

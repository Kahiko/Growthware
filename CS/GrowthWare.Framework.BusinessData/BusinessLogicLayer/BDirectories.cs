using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.ObjectModel;
using System.Data;

namespace GrowthWare.Framework.BusinessData.BusinessLogicLayer
{
    public class BDirectories : BaseBusinessLogic
    {
        private IDDirectories m_DDirectories;

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
        /// MSecurityEntityProfile mSecurityEntityProfile = MSecurityEntityProfile = New MSecurityEntityProfile();
        /// mSecurityEntityProfile.ID = ConfigSettings.DefaultSecurityEntityID;
        /// mSecurityEntityProfile.DAL = ConfigSettings.DAL;
        /// mSecurityEntityProfile.DAL_Namespace = ConfigSettings.DAL_NameSpace(mSecurityEntityProfile.DAL);
        /// mSecurityEntityProfile.DAL_Name = ConfigSettings.DAL_AssemblyName(mSecurityEntityProfile.DAL);
        /// mSecurityEntityProfile.ConnectionString = ConfigSettings.ConnectionString;
        /// 
        /// BDirectories mBAccount = BDirectories = New BDirectories(mSecurityEntityProfile, ConfigSettings.CentralManagement);
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
        /// Dim mBAccount As BDirectories = New BDirectories(mSecurityEntityProfile, ConfigSettings.CentralManagement)
        /// ]]>
        /// </code>
        /// </example>
        public BDirectories(MSecurityEntityProfile securityEntityProfile, bool centralManagement)
        {
            if (securityEntityProfile == null)
            {
                throw new ArgumentException("The securityEntityProfile and not be null!");
            }
            if (centralManagement)
            {
                if (m_DDirectories == null)
                {
                    m_DDirectories = (IDDirectories)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DDirectories");
                }
            }
            else
            {
                m_DDirectories = (IDDirectories)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DDirectories");
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
            if (IsDatabaseOnline()) 
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
        public void Save(ref MDirectoryProfile profile)
        {
            if(IsDatabaseOnline()) m_DDirectories.Save(ref profile);
        }
    }
}

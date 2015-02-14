using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces;
using GrowthWare.Framework.Common;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.BusinessData.BusinessLogicLayer
{
    public class BDBInformation : BaseBusinessLogic
    {
        private static IDBInformation m_DDBInformation;

        /// <summary>
        /// Private BDBInformation() to ensure only new instances with passed parameters is used.
        /// </summary>
        /// <remarks></remarks>
        private BDBInformation() { }

        /// <summary>
        /// Parameters are need to pass along to the factory for correct connection to the desired data store.
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
        /// BDBInformation mBAccount = BDBInformation = New BDBInformation(mSecurityEntityProfile, ConfigSettings.CentralManagement);
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
        /// Dim mBDBInformation As BDBInformation = New BDBInformation(mSecurityEntityProfile, ConfigSettings.CentralManagement)
        /// ]]>
        /// </code>
        /// </example>
        public BDBInformation(MSecurityEntityProfile securityEntityProfile, bool centralManagement)
        {
            if (!centralManagement)
            {
                if (m_DDBInformation == null)
                {
                    m_DDBInformation = (IDBInformation)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DDBInformation");
                    m_DDBInformation.ConnectionString = securityEntityProfile.ConnectionString;
                }
            }
            else
            {
                m_DDBInformation = (IDBInformation)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DDBInformation");
                m_DDBInformation.ConnectionString = securityEntityProfile.ConnectionString;
            }
        }

        /// <summary>
        /// Return a populated MDBInformation profile.
        /// </summary>
        /// <returns>MDBInformation</returns>
        /// <remarks></remarks>
        public MDBInformation GetProfile()
        {
            return new MDBInformation(m_DDBInformation.GetProfile());
        }

        /// <summary>
        /// Updates the ZF_INFORMATION table using the properties of a given profile.
        /// </summary>
        /// <param name="profile">MDBInformation</param>
        /// <returns>Boolean</returns>
        /// <remarks></remarks>
        public bool UpdateProfile(MDBInformation profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile can not be null (Nothing in Visual Basic)");
            bool mRetVal = false;
            m_DDBInformation.Profile = profile;
            if (DatabaseIsOnline()) 
            {
                mRetVal = m_DDBInformation.UpdateProfile();
            }
            return mRetVal;
        }
    }
}

using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces;
using GrowthWare.Framework.Common;
using System.Globalization;

namespace GrowthWare.Framework.BusinessData.BusinessLogicLayer
{
    /// <summary>
    /// Process business logic for functions
    /// </summary>
    /// <remarks>
    /// <![CDATA[
    /// MSecurityEntityProfile can be found in the GrowthWare.Framework.Model.Profiles namespace.  
    /// 
    /// The following properties are necessary for correct business logic operation.
    /// .ConnectionString
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
    public class BSecurityEntity : BaseBusinessLogic
    {
        private IDSecurityEntity m_DSecurityEntity;

        /// <summary>
        /// Private constructor to ensure only new instances with passed parameters is used.
        /// </summary>
        /// <remarks></remarks>
        private BSecurityEntity() { }

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
        public BSecurityEntity(MSecurityEntityProfile securityEntityProfile, bool centralManagement)
        {
            if (securityEntityProfile == null)
            {
                throw new ArgumentNullException("securityEntityProfile", "The securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!!");
            }
            if (centralManagement)
            {
                if (m_DSecurityEntity == null)
                {
                    m_DSecurityEntity = (IDSecurityEntity)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DSecurityEntity");
                }
            }
            else
            {
                m_DSecurityEntity = (IDSecurityEntity)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DSecurityEntity");
            }

            m_DSecurityEntity.ConnectionString = securityEntityProfile.ConnectionString;
        }

        /// <summary>
        /// Returns a collection of MSecurityEntityProfile objects for the given.
        /// </summary>
        /// <returns>
        ///		Collection of MSecurityEntityProfile
        ///	</returns>
        /// <remarks></remarks>
        public Collection<MSecurityEntityProfile> SecurityEntities()
        {
            Collection<MSecurityEntityProfile> mRetVal = new Collection<MSecurityEntityProfile>();
            DataTable mDataTable = null;
            try
            {
                if (ConfigSettings.DBStatus.ToUpper(CultureInfo.InvariantCulture) == "ONLINE")
                {
                    mDataTable = m_DSecurityEntity.GetSecurityEntities();
                    foreach (DataRow item in mDataTable.Rows)
                    {
                        MSecurityEntityProfile mProfile = new MSecurityEntityProfile(item);
                        mRetVal.Add(mProfile);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (mDataTable != null)
                {
                    mDataTable.Dispose();
                }
            }
            return mRetVal;
        }

        /// <summary>
        /// Gets the valid security entities.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="securityEntityId">The security entity id.</param>
        /// <param name="isSystemAdmin">if set to <c>true</c> [is system admin].</param>
        /// <returns>DataTable.</returns>
        public DataTable GetValidSecurityEntities(string account, int securityEntityId, bool isSystemAdmin)
        {
            DataTable mRetVal = null;
            if (ConfigSettings.DBStatus.ToUpper(CultureInfo.InvariantCulture) == "ONLINE")
            { 
                mRetVal = m_DSecurityEntity.GetValidSecurityEntities(account, securityEntityId, isSystemAdmin);
            }
            return mRetVal;
        }

        /// <summary>
        /// Searches security enties using the specified search criteria.
        /// </summary>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>DataTable.</returns>
        public DataTable Search(MSearchCriteria searchCriteria)
        {
            DataTable mRetVal = null;
            if (ConfigSettings.DBStatus.ToUpper(CultureInfo.InvariantCulture) == "ONLINE") 
            {
                mRetVal = m_DSecurityEntity.Search(searchCriteria);
            }
            return mRetVal;
        }

        /// <summary>
        /// Save Function information to the database
        /// </summary>
        /// <param name="profile">MSecurityEntityProfile</param>
        /// <returns>Integer</returns>
        public int Save(MSecurityEntityProfile profile)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!");
            profile.Id = profile.Id;
            if (ConfigSettings.DBStatus.ToUpper(CultureInfo.InvariantCulture) == "ONLINE") m_DSecurityEntity.Save(profile);
            return profile.Id;
        }
    }
}

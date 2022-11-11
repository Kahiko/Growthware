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
    /// Process business logic for accounts
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
    /// <example> This sample shows how to create an instance of the class.
    /// <code language="VB.NET">
    /// <![CDATA[
    /// Dim myBll as new BAccounts(mySecurityEntityProfile)
    /// ]]>
    /// </code>
    /// </example>
    public class BAccounts : BaseBusinessLogic
    {
        private IAccount m_DAccounts;

        /// <summary>
        /// Private BAccounts() to ensure only new instances with passed parameters is used.
        /// </summary>
        /// <remarks></remarks>
        private BAccounts()
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
        /// BAccounts mBAccount = BAccounts = New BAccounts(MSecurityEntity, ConfigSettings.CentralManagement);
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
        /// Dim mBAccount As BAccounts = New BAccounts(MSecurityEntity, ConfigSettings.CentralManagement)
        /// ]]>
        /// </code>
        /// </example>
        public BAccounts(MSecurityEntity securityEntityProfile, bool centralManagement)
        {
            if (securityEntityProfile == null) throw new ArgumentNullException("securityEntityProfile", "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!");
            if (!centralManagement)
            {
                if (m_DAccounts == null)
                {
                    m_DAccounts = (IAccount)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DAccounts");
                }
            }
            else
            {
                m_DAccounts = (IAccount)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DAccounts");
            }

            m_DAccounts.ConnectionString = securityEntityProfile.ConnectionString;
            m_DAccounts.SecurityEntitySeqId = securityEntityProfile.Id;
        }

        /// <summary>
        /// Deletes a record from the database.
        /// </summary>
        /// <param name="accountId">int</param>
        /// <remarks>Not to be used lightly.  In most cases any history associated with the account will be lost.</remarks>
        /// <example> This sample shows how to create an instance of the class.
        /// <code language="VB.NET">
        /// <![CDATA[
        /// Dim mBll as new BAccounts(mySecurityEntityProfile)
        /// mBll.Delete(1)
        /// ]]>
        /// </code>
        /// <code language="C#">
        /// <![CDATA[
        /// BAccounts mBll = new BAccounts(mySecurityEntityProfile);
        /// mBll.Delete(1);
        /// ]]>
        /// </code>
        /// </example>
        public void Delete(int accountId)
        {
            m_DAccounts.Profile = new MAccountProfile();
            m_DAccounts.Profile.Id = accountId;
            if (DatabaseIsOnline()) m_DAccounts.Delete();
        }

        /// <summary>
        /// Returns Account model based given the acccount name 
        /// </summary>
        /// <param name="account">String</param>
        /// <returns>MAccountProfile</returns>
        /// <remarks></remarks>
        /// <example> This sample shows how to create an instance of the class.
        /// <code language="VB.NET">
        /// <![CDATA[
        /// Dim mBll as new BAccounts(mySecurityEntityProfile)
        /// Dim mMAccountProfile as MAccountProfile = mbill.GetAccountProfile("Tester")
        /// ]]>
        /// </code>
        /// <code language="C#">
        /// <![CDATA[
        /// BAccounts mBll = new BAccounts(mySecurityEntityProfile);
        /// MAccountProfile mMAccountProfile = mbill.GetAccountProfile("Tester");
        /// ]]>
        /// </code>
        /// </example>
        public MAccountProfile GetProfile(string account)
        {
            MAccountProfile mRetVal = null;
            if (DatabaseIsOnline()) 
            {
                m_DAccounts.Profile = new MAccountProfile();
                m_DAccounts.Profile.Account = account;
                DataTable mAssignedRoles = m_DAccounts.Roles();
                DataTable mAssignedGroups = m_DAccounts.Groups();
                DataTable mRoles = m_DAccounts.Security();
                mRetVal = new MAccountProfile(m_DAccounts.GetAccount, mAssignedRoles, mAssignedGroups, mRoles);
            }
            return mRetVal;
        }

        public MAccountProfile GetProfileByRefreshToken(string token)
        {
            MAccountProfile mRetVal = null;
            if (DatabaseIsOnline()) 
            {
                string mAccount = string.Empty;
                string mColumnName = "Account";
                m_DAccounts.Profile = new MAccountProfile();
                m_DAccounts.Profile.Token = token;
                DataRow mDataRow = m_DAccounts.GetAccountByRefreshToken;
                // we will need the "Account" in order to get the correct roles and groups
                if (mDataRow != null && mDataRow.Table.Columns.Contains(mColumnName) && !(Convert.IsDBNull(mDataRow[mColumnName])))
                {
                    mAccount = mDataRow[mColumnName].ToString().Trim();
                } 
                else 
                {
                    throw new BusinessLogicLayerException("token does not exist, unable to get account");
                }
                m_DAccounts.Profile.Account = mAccount;
                DataTable mAssignedRoles = m_DAccounts.Roles();
                DataTable mAssignedGroups = m_DAccounts.Groups();
                DataTable mRoles = m_DAccounts.Security();
                mRetVal = new MAccountProfile(mDataRow, mAssignedRoles, mAssignedGroups, mRoles);
            }
            return mRetVal;

        }

        /// <summary>
        /// Returns a collection of MAccountProfiles without any role information
        /// </summary>
        /// <param name="profile">An instance of MAccountProfile</param>
        /// <returns></returns>
        public Collection<MAccountProfile> GetAccounts(MAccountProfile profile)
        {
            Collection<MAccountProfile> mRetList = new Collection<MAccountProfile>();
            DataTable mDataTable = null;
            try
            {
                m_DAccounts.Profile = profile;
                if (DatabaseIsOnline()) mDataTable = m_DAccounts.GetAccounts;
                if (mDataTable != null) 
                {
                    foreach (DataRow item in mDataTable.Rows)
                    {
                        mRetList.Add(new MAccountProfile(item));
                    }
                }
                return mRetList;
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
        }

        /// <summary>
        /// Retrieves menu data for a given account and MenuType
        /// </summary>
        /// <param name="account">String</param>
        /// <param name="menuType">MenuType</param>
        /// <returns>DataTable</returns>
        /// <remarks></remarks>
        public DataTable GetMenu(String account, MenuType menuType)
        {
            DataTable mRetVal = null;
            if (DatabaseIsOnline()) 
            {
                mRetVal = m_DAccounts.GetMenu(account, menuType);
            }
            return mRetVal;
        }

        /// <summary>
        /// Check the database to see if the refreshToken matches any existing Tokens
        /// already in the data store.
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns>bool</returns>
        public bool RefreshTokenExists(string refreshToken)
        {
            return m_DAccounts.RefreshTokenExists(refreshToken);
        }

        /// <summary>
        /// Performs either insert or update of an MAcountProfile, and re-populates the MAccountProfile with DB information.
        /// </summary>
        /// <param name="profile">MAccountProfile</param>
        /// <param name="saveRoles">MAccountProfile</param>
        /// <param name="saveGroups">MAccountProfile</param>
        /// <remarks>
        /// Updates the model object with information from the database<br></br>
        /// For example if you are creating a new account the ID will be sent into<br></br>
        /// this method as -1, after the call to this method the ID will from the database
        /// </remarks>
        /// <example> This sample shows how to create an instance of the class.
        /// <code language="VB.NET">
        /// <![CDATA[
        /// Dim mMAccountProfile as new MAccountProfile()
        /// mMAccountProfile.Account = "Account"
        /// mMAccountProfile.Password = CryptoUtility.Encrypt("my password", ConfigSettings.EncryptionType)
        /// mMAccountProfile.UpdatedBy = 1
        /// mMAccountProfile.UpdagedDate = Date.Now
        /// Dim mBll as new BAccounts(mySecurityEntityProfile)
        /// Dim mSaveRoles As Boolean = False;
        /// Dim mSaveGroups As Boolean = False;
        /// mMAccountProfile = mbill.SaveAccount(mMAccountProfile, mSaveRoles, mSaveGroups)
        /// ]]>
        /// </code>
        /// <code language="C#">
        /// MAccountProfile mMAccountProfile = new mMAccountProfile();
        /// mMAccountProfile.Account = "Account";
        /// mMAccountProfile.Password = CryptoUtility.Encrypt("my password", ConfigSettings.EncryptionType);
        /// mMAccountProfile.UpdatedBy = 1;
        /// mMAccountProfile.UpdagedDate = Date.Now();
        /// BAccounts mBll = new BAccounts(mySecurityEntityProfile);
        /// bool mSaveRoles = false;
        /// bool mSaveGroups = true;
        /// mMAccountProfile = mbill.SaveAccount(ref mMAccountProfile, mSaveRoles, mSaveGroups);
        /// </code>
        /// </example>
        public void Save(MAccountProfile profile, bool saveRoles, bool saveGroups)
        {
            if (profile == null) throw new ArgumentNullException("profile", "profile cannot be a null reference (Nothing in Visual Basic)!");
            m_DAccounts.Profile = profile;
            if (DatabaseIsOnline()) 
            {
                profile.Id = m_DAccounts.Save();
                if (saveGroups)
                {
                    m_DAccounts.SaveGroups();
                }
                if (saveRoles)
                {
                    m_DAccounts.SaveRoles();
                }
                profile = new MAccountProfile(m_DAccounts.GetAccount, m_DAccounts.Roles(), m_DAccounts.Groups(), m_DAccounts.Security());
            }
        }
    }
}

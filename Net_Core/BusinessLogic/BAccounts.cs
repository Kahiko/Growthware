using GrowthWare.DataAccess.Interfaces;
using GrowthWare.Framework;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Threading.Tasks;

namespace GrowthWare.BusinessLogic;

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
public class BAccounts : AbstractBusinessLogic
{

    #region Member Fields
    private IAccount m_DAccounts;
    #endregion

    #region Constructors
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
    /// <code language="C#">
    /// <![CDATA[
    /// MSecurityEntity MSecurityEntity = MSecurityEntity = New MSecurityEntity();
    /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID;
    /// MSecurityEntity.DAL = ConfigSettings.DAL;
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL);
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL);
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString;
    /// 
    /// BAccounts mBAccount = BAccounts = New BAccounts(MSecurityEntity);
    /// ]]>
    /// </code>
    /// <code language="VB.NET">
    /// <![CDATA[
    /// Dim MSecurityEntity As MSecurityEntity = New MSecurityEntity()
    /// MSecurityEntity.ID = ConfigSettings.DefaultSecurityEntityID
    /// MSecurityEntity.DAL = ConfigSettings.DAL
    /// MSecurityEntity.DAL_Namespace = ConfigSettings.DAL_NameSpace(MSecurityEntity.DAL)
    /// MSecurityEntity.DAL_Name = ConfigSettings.DAL_AssemblyName(MSecurityEntity.DAL)
    /// MSecurityEntity.ConnectionString = ConfigSettings.ConnectionString
    /// 
    /// Dim mBAccount As BAccounts = New BAccounts(MSecurityEntity)
    /// ]]>
    /// </code>
    /// </example>
    public BAccounts(MSecurityEntity securityEntityProfile)
    {
        if (securityEntityProfile == null) throw new ArgumentNullException(nameof(securityEntityProfile), "securityEntityProfile cannot be a null reference (Nothing in Visual Basic)!");
        if (m_DAccounts == null)
        {
            this.m_DAccounts = (IAccount)ObjectFactory.Create(securityEntityProfile.DataAccessLayerAssemblyName, securityEntityProfile.DataAccessLayerNamespace, "DAccounts", securityEntityProfile.ConnectionString, securityEntityProfile.Id);
            if (this.m_DAccounts == null)
            {
                throw new InvalidOperationException("Failed to create an instance of DAccounts with or without parameters.");
            }
        }
    }
    #endregion

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
    /// Returns Account model based given the account name 
    /// </summary>
    /// <param name="account">String</param>
    /// <returns>MAccountProfile</returns>
    /// <remarks></remarks>
    /// <example> This sample shows how to create an instance of the class.
    /// <code language="VB.NET">
    /// <![CDATA[
    /// Dim mBll as new BAccounts(mySecurityEntityProfile)
    /// Dim mMAccountProfile as MAccountProfile = mBill.GetAccountProfile("Tester")
    /// ]]>
    /// </code>
    /// <code language="C#">
    /// <![CDATA[
    /// BAccounts mBll = new BAccounts(mySecurityEntityProfile);
    /// MAccountProfile mMAccountProfile = mBill.GetAccountProfile("Tester");
    /// ]]>
    /// </code>
    /// </example>
    public async Task<MAccountProfile> GetProfile(string account, int securityEntitySeqId)
    {
        MAccountProfile mRetVal = null;
        if (DatabaseIsOnline())
        {
            if (account != null && !String.IsNullOrWhiteSpace(account))
            {
                m_DAccounts.Profile = new MAccountProfile();
                m_DAccounts.Profile.Account = account;
                DataSet mAccountData = await m_DAccounts.GetAccount(securityEntitySeqId);
                DataRow mAccountRow = mAccountData.Tables[(int)AccountTables.AccountDetails].Rows[0];
                DataTable mRefreshTokens = mAccountData.Tables[(int)AccountTables.RefreshTokens];
                DataTable mAssignedRoles = mAccountData.Tables[(int)AccountTables.AssignedRoles];
                DataTable mAssignedGroups = mAccountData.Tables[(int)AccountTables.AssignedGroups];
                DataTable mDerivedRoles = mAccountData.Tables[(int)AccountTables.DerivedRoles];
                mRetVal = new MAccountProfile(mAccountRow, mRefreshTokens, mAssignedRoles, mAssignedGroups, mDerivedRoles);
            }
            else
            {
                throw new ArgumentException("account can not be null or empty", account);
            }
        }
        return mRetVal;
    }

    public async Task<MAccountProfile> GetProfileByRefreshToken(string token)
    {
        MAccountProfile mRetVal = null;
        if (DatabaseIsOnline())
        {
            string mColumnName = "ACCT";
            m_DAccounts.Profile = new MAccountProfile
            {
                Token = token
            };
            DataSet mAccountData = await m_DAccounts.GetAccountByRefreshToken();
            if (mAccountData.Tables.Count > 0 && mAccountData.Tables[0].Rows.Count > 0)
            {
                DataRow mDataRow = mAccountData.Tables[0].Rows[0];
                m_DAccounts.Profile.Account = mDataRow[mColumnName].ToString().Trim();
                DataTable mRefreshTokens = mAccountData.Tables[(int)AccountTables.RefreshTokens];
                DataTable mAssignedRoles = mAccountData.Tables[(int)AccountTables.AssignedRoles];
                DataTable mAssignedGroups = mAccountData.Tables[(int)AccountTables.AssignedGroups];
                DataTable mDerivedRoles = mAccountData.Tables[(int)AccountTables.DerivedRoles];
                mRetVal = new MAccountProfile(mDataRow, mRefreshTokens, mAssignedRoles, mAssignedGroups, mDerivedRoles);
                return mRetVal;
            }
            throw new BusinessLogicLayerException("token does not exist, unable to get account");
        }
        return mRetVal;
    }

    public async Task<MAccountProfile> GetProfileByResetToken(string token)
    {
        MAccountProfile mRetVal = null;
        if (DatabaseIsOnline())
        {
            string mColumnName = "ACCT";
            m_DAccounts.Profile = new MAccountProfile
            {
                ResetToken = token
            };
            DataSet mAccountData = await m_DAccounts.GetAccountByResetToken();
            if (mAccountData.Tables.Count > 0 && mAccountData.Tables[0].Rows.Count > 0)
            {
                DataRow mDataRow = mAccountData.Tables[0].Rows[0];
                m_DAccounts.Profile.Account = mDataRow[mColumnName].ToString().Trim();
                DataTable mRefreshTokens = mAccountData.Tables[(int)AccountTables.RefreshTokens];
                DataTable mAssignedRoles = mAccountData.Tables[(int)AccountTables.AssignedRoles];
                DataTable mAssignedGroups = mAccountData.Tables[(int)AccountTables.AssignedGroups];
                DataTable mDerivedRoles = mAccountData.Tables[(int)AccountTables.DerivedRoles];
                mRetVal = new MAccountProfile(mDataRow, mRefreshTokens, mAssignedRoles, mAssignedGroups, mDerivedRoles);
                return mRetVal;
            }
            throw new BusinessLogicLayerException("Invalid token");
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns a MAccountProfile based on a verification token
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<MAccountProfile> GetProfileByVerificationToken(string token)
    {
        MAccountProfile mRetVal = null;
        if (DatabaseIsOnline())
        {
            string mColumnName = "ACCT";
            m_DAccounts.Profile = new MAccountProfile
            {
                VerificationToken = token
            };
            DataSet mAccountData = await m_DAccounts.GetAccountByVerificationToken();
            if (mAccountData.Tables.Count > 0 && mAccountData.Tables[0].Rows.Count > 0)
            {
                DataRow mDataRow = mAccountData.Tables[0].Rows[0];
                m_DAccounts.Profile.Account = mDataRow[mColumnName].ToString().Trim();
                DataTable mRefreshTokens = mAccountData.Tables[(int)AccountTables.RefreshTokens];
                DataTable mAssignedRoles = mAccountData.Tables[(int)AccountTables.AssignedRoles];
                DataTable mAssignedGroups = mAccountData.Tables[(int)AccountTables.AssignedGroups];
                DataTable mDerivedRoles = mAccountData.Tables[(int)AccountTables.DerivedRoles];
                mRetVal = new MAccountProfile(mDataRow, mRefreshTokens, mAssignedRoles, mAssignedGroups, mDerivedRoles);
                return mRetVal;
            }
            throw new BusinessLogicLayerException("Invalid token");
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns a collection of MAccountProfiles without any role information
    /// </summary>
    /// <param name="profile">An instance of MAccountProfile</param>
    /// <returns></returns>
    public async Task<Collection<MAccountProfile>> GetAccounts(MAccountProfile profile)
    {
        Collection<MAccountProfile> mRetList = [];
        DataTable mDataTable = null;
        try
        {
            m_DAccounts.Profile = profile;
            if (DatabaseIsOnline()) mDataTable = await m_DAccounts.GetAccounts();
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
    /// <param name="securityEntitySeqId">int</param>
    /// <returns>DataTable</returns>
    /// <remarks></remarks>
    public async Task<DataTable> GetMenu(String account, MenuType menuType, int securityEntitySeqId)
    {
        DataTable mRetVal = null;
        if (DatabaseIsOnline())
        {
            mRetVal = await m_DAccounts.GetMenu(account, menuType, securityEntitySeqId);
        }
        return mRetVal;
    }

    /// <summary>
    /// Check the database to see if the refreshToken matches any existing Tokens
    /// already in the data store.
    /// </summary>
    /// <param name="refreshToken"></param>
    /// <returns>bool</returns>
    public async Task<bool> RefreshTokenExists(string refreshToken)
    {
        return await m_DAccounts.RefreshTokenExists(refreshToken);
    }

    /// <summary>
    /// Check the database to see if the resetToken matches any existing Tokens
    /// </summary>
    /// <param name="resetToken"></param>
    /// <returns></returns>
    public async Task<bool> ResetTokenExists(string resetToken)
    {
        return await m_DAccounts.ResetTokenExists(resetToken);
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
    /// mMAccountProfile.UpdatedDate = Date.Now
    /// Dim mBll as new BAccounts(mySecurityEntityProfile)
    /// Dim mSaveRoles As Boolean = False;
    /// Dim mSaveGroups As Boolean = False;
    /// mMAccountProfile = mBll.SaveAccount(mMAccountProfile, mSaveRoles, mSaveGroups)
    /// ]]>
    /// </code>
    /// <code language="C#">
    /// MAccountProfile mMAccountProfile = new mMAccountProfile();
    /// mMAccountProfile.Account = "Account";
    /// mMAccountProfile.Password = CryptoUtility.Encrypt("my password", ConfigSettings.EncryptionType);
    /// mMAccountProfile.UpdatedBy = 1;
    /// mMAccountProfile.UpdatedDate = Date.Now();
    /// BAccounts mBll = new BAccounts(mySecurityEntityProfile);
    /// bool mSaveRoles = false;
    /// bool mSaveRefreshTokens = true
    /// bool mSaveGroups = true;
    /// mMAccountProfile = mBill.SaveAccount(ref mMAccountProfile, saveRefreshTokens, mSaveRoles, mSaveGroups);
    /// </code>
    /// </example>
    public async Task Save(MAccountProfile profile, bool saveRefreshTokens, bool saveRoles, bool saveGroups, int securityEntitySeqId)
    {
        m_DAccounts.Profile = profile ?? throw new ArgumentNullException(nameof(profile), "profile cannot be a null reference (Nothing in Visual Basic)!");
        m_DAccounts.SecurityEntitySeqId = securityEntitySeqId;
        if (DatabaseIsOnline())
        {
            profile.Id = await m_DAccounts.Save();
            if (saveGroups)
            {
                await m_DAccounts.SaveGroups();
            }
            if (saveRefreshTokens)
            {
                await m_DAccounts.SaveRefreshTokens();
            }
            if (saveRoles)
            {
                await m_DAccounts.SaveRoles();
            }
            DataSet mAccountData = await m_DAccounts.GetAccount(securityEntitySeqId);
            DataRow mAccountRow = mAccountData.Tables[(int)AccountTables.AccountDetails].Rows[0];
            DataTable mRefreshTokens = mAccountData.Tables[(int)AccountTables.RefreshTokens];
            DataTable mAssignedRoles = mAccountData.Tables[(int)AccountTables.AssignedRoles];
            DataTable mAssignedGroups = mAccountData.Tables[(int)AccountTables.AssignedGroups];
            DataTable mDerivedRoles = mAccountData.Tables[(int)AccountTables.DerivedRoles];
            profile = new MAccountProfile(mAccountRow, mRefreshTokens, mAssignedRoles, mAssignedGroups, mDerivedRoles);
        }
    }

    /// <summary>
    /// Check the database to see if the verificationToken matches any existing Tokens
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public async Task<bool> VerificationTokenExists(string token)
    {
        return await m_DAccounts.VerificationTokenExists(token);
    }
}

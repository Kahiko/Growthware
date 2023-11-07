using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Utilities;

namespace GrowthWare.Web.Support.Services;

public class ClientChoicesService : IClientChoicesService
{
    private static string m_AnonymousAccount = "Anonymous";

    private CacheController m_CacheController = CacheController.Instance();

    [CLSCompliant(false)]
    public ClientChoicesService()
    {
    }

    /// <summary>
    /// Adds or updates a value in the cache or session.
    /// </summary>
    /// <param name="name">The name of the value to add or update.</param>
    /// <param name="value">The value to add or update.</param>
    private void addOrUpdateCacheOrSession(string name, object value, string forAccount)
    {
        if (forAccount.ToLowerInvariant() != m_AnonymousAccount.ToLowerInvariant())
        {
            SessionController.AddToSession(name, value);
            return;
        }
        this.m_CacheController.AddToCache(m_AnonymousAccount, value);
    }

    /// <summary>
    /// Gets the state of the client choices.
    /// </summary>
    /// <param name="account">The account.</param>
    /// <param name="fromDB">if set to <c>true</c> [from database].</param>
    /// <returns>MClientChoicesState.</returns>
    public MClientChoicesState GetClientChoicesState(string account, bool fromDB)
    {
        if (string.IsNullOrEmpty(account)) throw new ArgumentNullException("account", "account cannot be a null reference (Nothing in VB) or empty!");
        MSecurityEntity mSecurityEntity = SecurityEntityUtility.DefaultProfile();
        MClientChoicesState mRetVal = this.getFromCacheOrSession<MClientChoicesState>(MClientChoices.SessionName, account); ;
        if (mRetVal == null || fromDB)
        {
            BClientChoices mBClientChoices = new BClientChoices(mSecurityEntity, ConfigSettings.CentralManagement);
            mRetVal = mBClientChoices.GetClientChoicesState(account);
            this.addOrUpdateCacheOrSession(MClientChoices.SessionName, mRetVal, account);
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns the client choices given the account
    /// </summary>
    /// <param name="account">String</param>
    /// <returns>MClientChoicesState</returns>
    public MClientChoicesState GetClientChoicesState(String account)
    {
        return GetClientChoicesState(account, false);
    }

    /// <summary>
    /// Retrieves an object of type `T` from either the cache or the session, based on the given `name`.
    /// </summary>
    /// <typeparam name="T">The type of the object being retrieved.</typeparam>
    /// <param name="name">The name of the value to retrieved.</param>
    /// <returns></returns>
    private T getFromCacheOrSession<T>(string name, string forAccount)
    {
        if (forAccount.ToLowerInvariant() != m_AnonymousAccount.ToLowerInvariant())
        {
            return SessionController.GetFromSession<T>(name);
        }
        return this.m_CacheController.GetFromCache<T>(m_AnonymousAccount);
    }

    /// <summary>
    /// Save the client choices to the database.
    /// </summary>
    /// <param name="clientChoicesState">MClientChoicesState</param>
    /// <param name="updateContext">bool</param>
    /// <remarks></remarks>
    public void Save(MClientChoicesState clientChoicesState, bool updateContext)
    {
        if (clientChoicesState == null) throw new ArgumentNullException("clientChoicesState", "clientChoicesState cannot be a null reference (Nothing in Visual Basic)! (Nothing in VB)!");
        MSecurityEntity mSecurityEntityProfile = SecurityEntityUtility.DefaultProfile();
        BClientChoices mBClientChoices = new BClientChoices(mSecurityEntityProfile, ConfigSettings.CentralManagement);
        mBClientChoices.Save(clientChoicesState);
        if (updateContext)
        {
            this.addOrUpdateCacheOrSession(MClientChoices.SessionName, clientChoicesState, clientChoicesState.AccountName);
        }
    }

    /// <summary>
    /// Save the client choices to the database.
    /// </summary>
    /// <param name="clientChoicesState">MClientChoicesState</param>
    /// <remarks></remarks>
    public void Save(MClientChoicesState clientChoicesState)
    {
        Save(clientChoicesState, true);
    }

    /// <summary>
    /// Gets the selected security entity.
    /// </summary>
    /// <returns>System.Int32.</returns>
    public int SelectedSecurityEntity()
    {
        MClientChoicesState myClientChoicesState = this.m_CacheController.GetFromCache<MClientChoicesState>(MClientChoices.SessionName);
        if ((myClientChoicesState != null))
        {
            return int.Parse(myClientChoicesState[MClientChoices.SecurityEntityID], CultureInfo.InvariantCulture);
        }
        else
        {
            return ConfigSettings.DefaultSecurityEntityID;
        }
    }
}
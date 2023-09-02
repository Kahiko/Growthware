using Microsoft.AspNetCore.Http;
using System;
using System.Data;
using System.Text.Json;
using System.Globalization;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Utilities;

namespace GrowthWare.Web.Support.Services;

public class ClientChoicesService : IClientChoicesService
{
    private static string m_AnonymousAccount = "Anonymous";

    private static MClientChoicesState m_AnonymousClientChoicesState;

    private static IHttpContextAccessor m_HttpContextAccessor = null;

    [CLSCompliant(false)]
    public ClientChoicesService(IHttpContextAccessor httpContextAccessor)
    {
        m_HttpContextAccessor = httpContextAccessor;
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
        BClientChoices mBClientChoices = new BClientChoices(mSecurityEntity, ConfigSettings.CentralManagement);
        if (fromDB) return mBClientChoices.GetClientChoicesState(account);
        MClientChoicesState mRetVal = null;
        if (account.Trim().ToLower(CultureInfo.CurrentCulture) != m_AnonymousAccount.ToLower(CultureInfo.CurrentCulture))
        {
            if (m_HttpContextAccessor.HttpContext.Session != null && m_HttpContextAccessor.HttpContext.Session.GetString(MClientChoices.SessionName) != null) 
            {
                string mJsonString = m_HttpContextAccessor.HttpContext.Session.GetString(MClientChoices.SessionName);
                mRetVal = new MClientChoicesState(mJsonString);
                if (mRetVal == null || (mRetVal.AccountName.Trim().ToUpper(CultureInfo.InvariantCulture) != account.Trim().ToUpper(CultureInfo.InvariantCulture)))
                {
                    mRetVal = mBClientChoices.GetClientChoicesState(account);
                    mJsonString = JsonSerializer.Serialize(mRetVal);
                    m_HttpContextAccessor.HttpContext.Session.SetString(MClientChoices.SessionName, mJsonString);
                }
            }
            else
            {
                mRetVal = mBClientChoices.GetClientChoicesState(account);
                string mJsonString = JsonSerializer.Serialize(mRetVal);
                m_HttpContextAccessor.HttpContext.Session.SetString(MClientChoices.SessionName, mJsonString);
            }
        }
        else
        {
            if (m_AnonymousClientChoicesState == null) 
            {
                m_AnonymousClientChoicesState = mBClientChoices.GetClientChoicesState(m_AnonymousAccount);
            }
            mRetVal = m_AnonymousClientChoicesState;
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
            if (m_HttpContextAccessor.HttpContext.Session != null)
            {
                string mJsonString = JsonSerializer.Serialize(clientChoicesState);
                m_HttpContextAccessor.HttpContext.Session.SetString(MClientChoices.SessionName, mJsonString);
            }
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
        if (m_HttpContextAccessor != null && m_HttpContextAccessor.HttpContext != null && m_HttpContextAccessor.HttpContext.Session != null)
        {
            string mJsonString = m_HttpContextAccessor.HttpContext.Session.GetString(MClientChoices.SessionName);
            MClientChoicesState myClientChoicesState = new MClientChoicesState(mJsonString);        
            if ((myClientChoicesState != null))
            {
                return int.Parse(myClientChoicesState[MClientChoices.SecurityEntityID], CultureInfo.InvariantCulture);
            }
            else
            {
                return ConfigSettings.DefaultSecurityEntityID;
            }
        }
        else
        {
            return ConfigSettings.DefaultSecurityEntityID;
        }
    }
}
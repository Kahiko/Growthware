using Microsoft.AspNetCore.Http;
using System;
using System.Globalization;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Utilities;
using System.Data;
using System.Text.Json;

namespace GrowthWare.Web.Support.Services;

public class ClientChoicesService : IClientChoicesService
{
    private string m_AnonymousAccount = "Anonymous";

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
        DataTable mDataTable = this.getDataTableWithEmptyRow();
        string mJsonString = this.getFromCacheOrSession<string>(MClientChoices.SessionName, account);
        if (mJsonString == null || fromDB)
        {
            BClientChoices mBClientChoices = new BClientChoices(SecurityEntityUtility.DefaultProfile(), ConfigSettings.CentralManagement);
            mJsonString = JsonSerializer.Serialize(mBClientChoices.GetDataRow(account).ItemArray);
            this.addOrUpdateCacheOrSession(MClientChoices.SessionName, mJsonString, account);
        }
        this.populateDataRow(ref mDataTable, mJsonString);
        MClientChoicesState mRetVal = new(mDataTable.Rows[0]);
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
    /// Creates a new DataTable with an empty row.
    /// </summary>
    /// <returns>The newly created DataTable with an empty row.</returns>
    private DataTable getDataTableWithEmptyRow()
    {
        DataTable mRetVal = new DataTable();
        mRetVal.Columns.Add("ACCT");
        mRetVal.Columns.Add("SecurityEntityID");
        mRetVal.Columns.Add("SecurityEntityName");
        mRetVal.Columns.Add("BackColor");
        mRetVal.Columns.Add("LeftColor");
        mRetVal.Columns.Add("HeadColor");
        mRetVal.Columns.Add("HeaderForeColor");
        mRetVal.Columns.Add("SubHeadColor");
        mRetVal.Columns.Add("RowBackColor");
        mRetVal.Columns.Add("AlternatingRowBackColor");
        mRetVal.Columns.Add("ColorScheme");
        mRetVal.Columns.Add("FavoriteAction");
        mRetVal.Columns.Add("recordsPerPage");
        mRetVal.Rows.Add(mRetVal.NewRow());
        return mRetVal;
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
    /// Populates a DataTable with data from a JSON string.
    /// </summary>
    /// <param name="yourDataTable">The DataTable to populate.</param>
    /// <param name="jasonData">The JSON string containing the data.</param>
    private void populateDataRow(ref DataTable yourDataTable, string jasonData)
    {
        string mJsonString = jasonData;
        // Remove unnecessary characters from the JSON string
        mJsonString = mJsonString.Replace("[", "").Replace("]", "").Replace("\"", "");
        // Split the JSON string into an array
        string[] mJsonStringArray = mJsonString.Split(',');
        // Iterate over the array and assign values to the DataTable
        for (int i = 0; i < mJsonStringArray.Length; i++)
        {
            yourDataTable.Rows[0][i] = mJsonStringArray[i];
        }
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
}
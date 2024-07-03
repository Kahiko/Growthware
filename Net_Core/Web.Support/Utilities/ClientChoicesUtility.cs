using System;
using System.Data;
using System.Text.Json;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Helpers;

namespace GrowthWare.Web.Support.Utilities;

public static class ClientChoicesUtility
{
    private static CacheHelper m_CacheController = CacheHelper.Instance();

#region "Cache/Session Methods"
    /// <summary>
    /// Adds or updates a value in the cache or session.
    /// </summary>
    /// <param name="forAccount">The name of the value to add or update.</param>
    /// <param name="value">The string value to add or update.</param>
    /// <remarks>
    /// The MClientChoicesState object does not deserialize correctly b/c it does not have
    /// concrete properties, so, we store a serialize DataRow.ItemArray.
    /// </remarks>
    private static void addOrUpdateCacheOrSession(string forAccount, string value)
    {
        if (forAccount.ToLowerInvariant() != ConfigSettings.Anonymous.ToLowerInvariant())
        {
            SessionHelper.AddToSession(MClientChoices.SessionName, value);
            return;
        }
        SessionHelper.AddToSession(MClientChoices.SessionName, value);
        m_CacheController.AddToCache(MClientChoices.AnonymousClientChoicesState, value);
    }

    /// <summary>
    /// Retrieves a value from either the cache or the session, based on the provided parameters.
    /// </summary>
    /// <param name="forAccount">The account for which the value is being retrieved.</param>
    /// <param name="name">The name of the value to retrieve.</param>
    /// <returns>The retrieved value.</returns>
    private static MClientChoicesState getFromCacheOrSession(string forAccount)
    {
        string mJsonString = SessionHelper.GetFromSession<string>(MClientChoices.SessionName) ?? m_CacheController.GetFromCache<string>(MClientChoices.AnonymousClientChoicesState);
        if(mJsonString != null && mJsonString.Length > 0) 
        {
            return convertToClientChoicesState(mJsonString);
        }
        return null;
    }
#endregion

#region "Helper Methods"
    private static MClientChoicesState convertToClientChoicesState(string jsonData)
    {
        /*
         *  1.) Make a copy of jsonData (As a matter of habbit we don't want to change the orginal parameter even if it is imutable)
         *  2.) Create a DataTable with the client choices "columns" in the correct order and add a new row
         *  3.) return MAccountProfile from the DataTable populated DataRow
         */
        string mJsonString = jsonData;
        DataTable mDataTable = new DataTable();
        mDataTable.Columns.Add("ACCT");
        mDataTable.Columns.Add("SecurityEntityID");
        mDataTable.Columns.Add("SecurityEntityName");
        mDataTable.Columns.Add("BackColor");
        mDataTable.Columns.Add("LeftColor");
        mDataTable.Columns.Add("HeadColor");
        mDataTable.Columns.Add("HeaderForeColor");
        mDataTable.Columns.Add("SubHeadColor");
        mDataTable.Columns.Add("RowBackColor");
        mDataTable.Columns.Add("AlternatingRowBackColor");
        mDataTable.Columns.Add("ColorScheme");
        mDataTable.Columns.Add("FavoriteAction");
        mDataTable.Columns.Add("recordsPerPage");
        mDataTable.Rows.Add(mDataTable.NewRow());
        // Remove unnecessary characters from the JSON string
        mJsonString = mJsonString.Replace("[", "").Replace("]", "").Replace("\"", "");
        // Split the JSON string into an array
        string[] mJsonStringArray = mJsonString.Split(',');
        // Iterate over the array and assign values to the DataTable DataRow
        for (int i = 0; i < mJsonStringArray.Length; i++)
        {
            mDataTable.Rows[0][i] = mJsonStringArray[i];
        }
        MClientChoicesState mRetVal = new(mDataTable.Rows[0]);
        return mRetVal;
    }
#endregion

    public static MClientChoicesState AnonymousState
    {
        get
        {
            MClientChoicesState mRetVal = getFromCacheOrSession(MClientChoices.AnonymousClientChoicesState);
            if (mRetVal == null)
            {
                string mJsonString = JsonSerializer.Serialize(getFromDB(ConfigSettings.Anonymous).ItemArray);
                addOrUpdateCacheOrSession(ConfigSettings.Anonymous, mJsonString);
                mRetVal = getFromCacheOrSession(MClientChoices.AnonymousClientChoicesState);
            }
            return mRetVal;
        }
    }

    public static void ClearSession()
    {
        SessionHelper.RemoveFromSession(MClientChoices.SessionName);
    }

    public static MClientChoicesState CurrentState
    {
        get
        {
            MClientChoicesState mRetVal = getFromCacheOrSession("not") ?? getFromCacheOrSession(MClientChoices.AnonymousClientChoicesState);
            if (mRetVal == null)
            {
                string mJsonString = JsonSerializer.Serialize(getFromDB(ConfigSettings.Anonymous).ItemArray);
                addOrUpdateCacheOrSession(ConfigSettings.Anonymous, mJsonString);
                mRetVal = getFromCacheOrSession("not") ?? getFromCacheOrSession(MClientChoices.AnonymousClientChoicesState);
            }
            if(ConfigSettings.SecurityEntityFromUrl)
            {
                mRetVal[MClientChoices.SecurityEntityID] = SecurityEntityUtility.CurrentProfile.Id.ToString();
                mRetVal[MClientChoices.SecurityEntityName] = SecurityEntityUtility.CurrentProfile.Name;
            }            
            return mRetVal;
        }
    }

    /// <summary>
    /// Retrieves the client choices state for the specified account and updates Cache/Session.
    /// </summary>
    /// <param name="forAccount">The account for which to retrieve the client choices state.</param>
    /// <returns>The client choices state for the specified account.</returns>
    private static MClientChoicesState getClientChoicesState(string forAccount)
    {
        DataRow mDataRow = getFromDB(forAccount);
        string mJsonString = JsonSerializer.Serialize(mDataRow.ItemArray);
        addOrUpdateCacheOrSession(forAccount, mJsonString);
        MClientChoicesState mRetVal = getFromCacheOrSession(forAccount);
        if(ConfigSettings.SecurityEntityFromUrl)
        {
            mRetVal[MClientChoices.SecurityEntityID] = SecurityEntityUtility.CurrentProfile.Id.ToString();
            mRetVal[MClientChoices.SecurityEntityName] = SecurityEntityUtility.CurrentProfile.Name;
        }
        return mRetVal;
    }

    /// <summary>
    /// Gets the client choices state for the specified account from the database.
    /// </summary>
    /// <param name="forAccount"></param>
    /// <returns></returns>
    private static DataRow getFromDB(string forAccount)
    {
        BClientChoices mBusinessLayer = new(SecurityEntityUtility.DefaultProfile(), ConfigSettings.CentralManagement);
        return mBusinessLayer.GetDataRow(forAccount);
    }

    /// <summary>
    /// Save the client choices to the database and updates Cache/Session.
    /// </summary>
    /// <param name="clientChoicesState">MClientChoicesState</param>
    /// <param name="updateContext">bool</param>
    /// <remarks>Calls getClientChoicesState to ensure the Session/Cache matches for the given account.</remarks>
    public static void Save(MClientChoicesState clientChoicesState)
    {
        if (clientChoicesState == null) throw new ArgumentNullException("clientChoicesState", "clientChoicesState cannot be a null reference (Nothing in Visual Basic)! (Nothing in VB)!");
        BClientChoices mBusinessLayer = new BClientChoices(SecurityEntityUtility.DefaultProfile(), ConfigSettings.CentralManagement);
        mBusinessLayer.Save(clientChoicesState);
        getClientChoicesState(clientChoicesState.AccountName);
    }

    /// <summary>
    /// Ensures the Session/Cache matches for the given account.
    /// </summary>
    /// <param name="forAccount"></param>
    public static void SynchronizeContext(string forAccount)
    {
        if(CurrentState.AccountName.ToLowerInvariant() != forAccount.ToLowerInvariant())
        {
            MClientChoicesState mClientChoicesState = getClientChoicesState(forAccount);            
        }
    }
}
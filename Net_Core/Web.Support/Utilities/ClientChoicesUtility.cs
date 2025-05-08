using System;
using System.Data;
using System.Text.Json;
using System.Threading.Tasks;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Web.Support.Helpers;

namespace GrowthWare.Web.Support.Utilities;

public static class ClientChoicesUtility
{
    private static BClientChoices m_BClientChoices = null;
    private static CacheHelper m_CacheHelper = CacheHelper.Instance();

    /// <summary>
    /// Returns the client choices state for the anonymous account from cache.
    /// </summary>
    /// <returns>MClientChoicesState</returns>
    public static MClientChoicesState AnonymousState()
    {
        string mJsonString = m_CacheHelper.GetFromCache<string>(MClientChoices.AnonymousClientChoicesState);
        if(mJsonString == null || string.IsNullOrWhiteSpace(mJsonString))
        {
            mJsonString = JsonSerializer.Serialize(getFromDB(ConfigSettings.Anonymous).ItemArray);
            m_CacheHelper.AddToCache(MClientChoices.AnonymousClientChoicesState, mJsonString);
        }
        MClientChoicesState mRetVal = stringToClientChoicesSTate(mJsonString);
        return mRetVal;
    }

    static BClientChoices BusinessLogic()
    {
        m_BClientChoices ??= new(SecurityEntityUtility.DefaultProfile(), ConfigSettings.CentralManagement);
        return m_BClientChoices;
    }

    /// <summary>
    /// Returns the current client choices state.
    /// </summary>
    /// <returns></returns>
    public static MClientChoicesState CurrentState()
    {
        string mJsonString = SessionHelper.GetFromSession<string>(MClientChoices.SessionName);
        if(mJsonString == null || string.IsNullOrWhiteSpace(mJsonString))
        {
            return AnonymousState();
        }
        MClientChoicesState mRetVal = stringToClientChoicesSTate(mJsonString);
        return mRetVal;
    }

    /// <summary>
    /// Gets the client choices state for the specified account from the database.
    /// </summary>
    /// <param name="forAccount"></param>
    /// <returns></returns>
    private static DataRow getFromDB(string forAccount)
    {
        return BusinessLogic().GetDataRow(forAccount);
    }

    /// <summary>
    /// Save the client choices to the database and updates Cache/Session.
    /// </summary>
    /// <param name="clientChoicesState">MClientChoicesState</param>
    /// <param name="updateContext">bool</param>
    /// <remarks>Calls getClientChoicesState to ensure the Session/Cache matches for the given account.</remarks>
    public static async Task Save(MClientChoicesState clientChoicesState)
    {
        if (clientChoicesState == null) throw new ArgumentNullException(nameof(clientChoicesState), "clientChoicesState cannot be a null reference (Nothing in Visual Basic)! (Nothing in VB)!");
        await BusinessLogic().Save(clientChoicesState);
    }

    /// <summary>
    /// Converts a JSON string to a MClientChoicesState object
    /// </summary>
    /// <param name="jsonData"></param>
    /// <returns></returns>
    private static MClientChoicesState stringToClientChoicesSTate(string jsonData)
    {
        /*
         *  1.) Make a copy of jsonData (As a matter of habbit we don't want to change the orginal parameter even if it is imutable)
         *  2.) Create a DataTable with the client choices "columns" in the correct order and add a new row
         *  3.) return MAccountProfile from the DataTable populated DataRow
         */
        string mJsonString = jsonData;
        DataTable mDataTable = new();
        mDataTable.Columns.Add("Account");
        mDataTable.Columns.Add("SecurityEntityID");
        mDataTable.Columns.Add("SecurityEntityName");
        mDataTable.Columns.Add("FavoriteAction");
        mDataTable.Columns.Add("RecordsPerPage");

        mDataTable.Columns.Add("ColorScheme");
        mDataTable.Columns.Add("EvenRow");
        mDataTable.Columns.Add("EvenFont");
        mDataTable.Columns.Add("OddRow");
        mDataTable.Columns.Add("OddFont");
        mDataTable.Columns.Add("HeaderRow");
        mDataTable.Columns.Add("HeaderFont");
        mDataTable.Columns.Add("Background");

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

    /// <summary>
    /// Ensures the Session/Cache matches for the given account.
    /// </summary>
    /// <param name="forAccount"></param>
    public static void SynchronizeContext(string forAccount)
    {
        if(!forAccount.Equals(CurrentState().Account, StringComparison.InvariantCultureIgnoreCase))
        {
            string mJsonString = JsonSerializer.Serialize(getFromDB(forAccount).ItemArray);
            SessionHelper.AddToSession(MClientChoices.SessionName, mJsonString);
        }
    }    
}
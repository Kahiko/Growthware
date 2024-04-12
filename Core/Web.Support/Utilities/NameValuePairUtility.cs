using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using GrowthWare.Framework.Models.UI;

namespace GrowthWare.Web.Support.Utilities;

public static class NameValuePairUtility
{
    // TODO: this will work for now but will need to be added to a caching sub system

    private static Dictionary<int, List<UIKeyValuePair>> m_NameValuePairCache = new Dictionary<int, List<UIKeyValuePair>>();

    /// <summary>
    /// Returns a List of UIKeyValuePair ({key: 1, value: "string"}) representing name value pairs from the database
    /// </summary>
    /// <param name="nameValuePairSeqId"></param>
    /// <returns>List<UIKeyValuePair></returns>
    private static List<UIKeyValuePair> getNameValuePairs(int nameValuePairSeqId)
    {
        if (!m_NameValuePairCache.ContainsKey(nameValuePairSeqId))
        {
            DataTable mDataTable = new DataTable();
            getNameValuePairDetails(ref mDataTable, nameValuePairSeqId);
            List<UIKeyValuePair> mPairs = null;
            mPairs = mDataTable.AsEnumerable().Select(item => new UIKeyValuePair
            {
                Key = int.Parse(item["NVP_SEQ_DET_ID"].ToString()),
                Value = item["NVP_DET_VALUE"].ToString()
            }).ToList();
            m_NameValuePairCache.Add(nameValuePairSeqId, mPairs);
        }
        List<UIKeyValuePair> mRetVal = m_NameValuePairCache[nameValuePairSeqId];
        return mRetVal;
    }

    /// <summary>
    /// Method to get all children for a name value pair (Security is ignored).  Created to support add/edit name value pair details
    /// </summary>
    /// <param name="nameValuePairSeqId"></param>
    /// <returns></returns>
    public static string GetAllChildrenForParent(int nameValuePairSeqId)
    {
        DataTable mDataTable = new DataTable();
        getNameValuePairDetails(ref mDataTable, nameValuePairSeqId);
        DataHelper.AddTotalRowsField(ref mDataTable);
        string mRetVal = DataHelper.GetJsonStringFromTable(ref mDataTable);
        return mRetVal;
    }

    /// <summary>
    /// Returns a List of UIKeyValuePair ({key: 1, value: "string"}) representing link behaviors
    /// </summary>
    /// <returns>List<UIKeyValuePair></returns>
    public static List<UIKeyValuePair> GetLinkBehaviors()
    {
        int mNVPSeqId = 3; // From .[ZGWSystem].[Name_Value_Pairs]
        List<UIKeyValuePair> mLinkBehaviorTypes = getNameValuePairs(mNVPSeqId);
        return mLinkBehaviorTypes;
    }

    /// <summary>
    /// Returns a List of UIKeyValuePair ({key: 1, value: "string"}) representing navigation types
    /// </summary>
    /// <returns>List<UIKeyValuePair></returns>
    public static List<UIKeyValuePair> GetNavigationTypes()
    {
        int mNVPSeqId = 1; // From .[ZGWSystem].[Name_Value_Pairs]
        List<UIKeyValuePair> mLinkBehaviorTypes = getNameValuePairs(mNVPSeqId);
        return mLinkBehaviorTypes;
    }

    public static List<MNameValuePair> GetMNameValuePairs()
    {
        List<MNameValuePair> mRetVal = new List<MNameValuePair>();
        DataTable mDataTable = new DataTable();
        getNameValuePairs(ref mDataTable);
        mRetVal = mDataTable.AsEnumerable().Select(item => new MNameValuePair(item)).ToList();
        return mRetVal;
    }

    private static void getNameValuePairs(ref DataTable yourDataTable)
    {
        BNameValuePairs mNameValuePairDetails = new BNameValuePairs(SecurityEntityUtility.CurrentProfile());
        yourDataTable = mNameValuePairDetails.GetAllNameValuePair();
    }

    private static void getNameValuePairDetails(ref DataTable yourDataTable, int nameValuePairSeqId)
    {
        DataView mDataView = null;
        DataTable mDataTable = null;
        try
        {
            getNameValuePairDetails(ref mDataTable);
            mDataTable.Locale = CultureInfo.InvariantCulture;
            mDataTable.DefaultView.RowFilter = string.Empty;
            mDataView = new DataView(mDataTable);
            yourDataTable = mDataView.Table.Clone();
            mDataView.RowFilter = "NVP_SEQ_ID = " + nameValuePairSeqId;
            yourDataTable = mDataView.ToTable();
        }
        finally
        {
            if (mDataView != null)
            {
                mDataView.Dispose();
            }
            if (mDataTable != null)
            {
                mDataTable.Dispose();
            }
        }
    }

    private static void getNameValuePairDetails(ref DataTable yourDataTable)
    {
        BNameValuePairs mNameValuePairDetails = new BNameValuePairs(SecurityEntityUtility.CurrentProfile());
        yourDataTable = mNameValuePairDetails.GetAllNameValuePairDetail();
    }
}
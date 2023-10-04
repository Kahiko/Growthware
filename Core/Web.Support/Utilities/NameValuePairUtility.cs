using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models.UI;

namespace GrowthWare.Web.Support.Utilities;

public static class NameValuePairUtility
{
    private static List<UIKeyValuePair> m_NavigationTypes = null;
    
    public static List<UIKeyValuePair> GetNavigationTypes()
    {
        if(m_NavigationTypes == null) 
        {
            DataTable mDataTable = new DataTable();
            int mNVPSeqId = 1; // From .[ZGWSystem].[Name_Value_Pairs]
            getNameValuePairDetails(ref mDataTable, mNVPSeqId);
            m_NavigationTypes = mDataTable.AsEnumerable().Select(item => new UIKeyValuePair {
                Key = int.Parse(item["NVP_SEQ_DET_ID"].ToString()) ,
                Value = item["NVP_DET_VALUE"].ToString()
            }).ToList();

        }
        return m_NavigationTypes;
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
            foreach (DataRowView mDataRowView in mDataView)
            {
                yourDataTable.ImportRow(mDataRowView.Row);
            }
        }
        finally
        {
            if(mDataView != null)
            {
                mDataView.Dispose();
            }
            if(mDataTable != null)
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
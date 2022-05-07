using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using System.Data;
using System.Text;

namespace GrowthWare.WebSupport.Utilities;
public static class SearchUtility
{
    private static BSearch m_BSearch = null;

    public static string GetSearchResults(MSearchCriteria searchCriteria) {
        string mRetVal = string.Empty;
        DataTable mDataTable = null;
        m_BSearch = new BSearch(SecurityEntityUtility.CurrentProfile(), ConfigSettings.CentralManagement);
        mDataTable = m_BSearch.GetSearchResults(searchCriteria);
            var mStringBuilder = new StringBuilder();
            if (mDataTable.Rows.Count > 0)
            {
                mStringBuilder.Append("[");
                for (int i = 0; i < mDataTable.Rows.Count; i++)
                {
                    mStringBuilder.Append("{");
                    for (int j = 0; j < mDataTable.Columns.Count; j++)
                    {
                        if (j < mDataTable.Columns.Count - 1)
                        {
                            mStringBuilder.Append("\"" + mDataTable.Columns[j].ColumnName.ToString() + "\":" + "\"" + mDataTable.Rows[i][j].ToString() + "\",");
                        }
                        else if (j == mDataTable.Columns.Count - 1)
                        {
                            mStringBuilder.Append("\"" + mDataTable.Columns[j].ColumnName.ToString() + "\":" + "\"" + mDataTable.Rows[i][j].ToString() + "\"");
                        }
                    }
                    if (i == mDataTable.Rows.Count - 1)
                    {
                        mStringBuilder.Append("}");
                    }
                    else
                    {
                        mStringBuilder.Append("},");
                    }
                }
                mStringBuilder.Append("]");
            }

        return mRetVal;
    }
}
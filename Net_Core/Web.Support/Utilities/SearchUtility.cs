using GrowthWare.BusinessLogic;
using GrowthWare.Framework;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Text;

namespace GrowthWare.Web.Support.Utilities;
public static class SearchUtility
{

    public static Tuple<string, string> GetOrderByAndWhere(string columns, string[] searchColumns, string[] sortColumnInfo, string searchText)
    {
        string mWhereClause = "";
        String mOrderByClause = "";
        foreach (var item in sortColumnInfo)
        {
            string[] mColumnParts = item.Split("=");
            if (mColumnParts != null && mColumnParts.Length == 2)
            {
                if (columns.Contains(mColumnParts[0], StringComparison.OrdinalIgnoreCase))
                {
                    // [Account] ASC,
                    mOrderByClause += ", " + mColumnParts[0] + " " + mColumnParts[1] + Environment.NewLine;
                }
            }
        }
        foreach (var item in searchColumns)
        {
            if(item.Length > 0)
            {
                // [Account] LIKE '%abc%' OR
                mWhereClause += "AND " + item + " LIKE '%" + searchText + "%'" + Environment.NewLine;
            }
        }
        if(mWhereClause.Length > 0)
        {
            mWhereClause = mWhereClause.Substring(4, mWhereClause.Length - 4);
        } 
        else 
        {
            mWhereClause = "1=1";
        }
        if(mOrderByClause.Length > 0)
        {
            mOrderByClause = mOrderByClause.Substring(2, mOrderByClause.Length - 2);
        }
        else
        {
            mOrderByClause = "2 asc";
        }
        return new Tuple<string, string>(mOrderByClause, mWhereClause);
    }

    public static string GetSearchResults(MSearchCriteria searchCriteria, string constantWhere = "1=1")
    {
        string mRetVal = string.Empty;
        DataTable mDataTable = null;
        BSearch mBSearch = new BSearch(SecurityEntityUtility.CurrentProfile);
        searchCriteria.WhereClause = constantWhere + " AND " + searchCriteria.WhereClause;
        mDataTable = mBSearch.GetSearchResults(searchCriteria);
        mRetVal = DataHelper.GetJsonStringFromTable(ref mDataTable);
        return mRetVal;
    }
}
using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GrowthWare.DataAccess.SQLServer;

/// <summary>
/// Performs all data store interaction to SQL Server.
/// </summary>
public class DSearch : AbstractDBInteraction, ISearch
{

#region Member Fields
    private int m_SecurityEntityID;
#endregion

#region Constructors
    public DSearch(string connectionString, int securityEntitySeqId): base() 
    {
        this.ConnectionString = connectionString;
        this.m_SecurityEntityID = securityEntitySeqId;
    }
#endregion

    int ISearch.SecurityEntitySeqID
    {
        get { return m_SecurityEntityID; }
        set { m_SecurityEntityID = value; }
    }

    async Task<DataTable> ISearch.GetSearchResults(MSearchCriteria searchCriteria)
    {
        if (searchCriteria == null) throw new ArgumentNullException(nameof(searchCriteria), "searchCriteria cannot be a null reference (Nothing in Visual Basic)!");
        string mStoredProcedure = "[ZGWSystem].[Get_Paginated_Data]";
        string mOrderByClause = Regex.Replace(searchCriteria.OrderByClause, @"<[^>]+>|&nbsp;", "").Trim();
        mOrderByClause = mOrderByClause.Replace("\r\n", "");
        string mWhereClause = Regex.Replace(searchCriteria.WhereClause, @"<[^>]+>|&nbsp;", "").Trim();
        DataTable mRetVal;
        SqlParameter[] mParameters = [
            new("@P_Columns", searchCriteria.Columns),
            new("@P_OrderByClause", mOrderByClause),
            new("@P_PageSize", searchCriteria.PageSize),
            new("@P_SelectedPage", searchCriteria.SelectedPage),
            new("@P_TableOrView", searchCriteria.TableOrView),
            new("@P_WhereClause", mWhereClause)
        ];
        mRetVal = await base.GetDataTableAsync(mStoredProcedure, mParameters);
        return mRetVal;
    }
}

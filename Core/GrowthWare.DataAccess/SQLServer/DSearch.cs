using GrowthWare.DataAccess.Interfaces;
using GrowthWare.DataAccess.SQLServer.Base;
using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace GrowthWare.DataAccess.SQLServer
{
    /// <summary>
    /// Performs all data store interaction to SQL Server.
    /// </summary>
    public class DSearch : ADBInteraction, ISearch
    {
        private int m_SecurityEntityID;

        int ISearch.SecurityEntitySeqID
        {
            get { return m_SecurityEntityID; }
            set { m_SecurityEntityID = value; }
        }

        DataTable ISearch.GetSearchResults(MSearchCriteria searchCriteria)
        {
            if (searchCriteria == null) throw new ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!");
            string mStoredProcedure = "ZGWSystem.Get_Paginated_Data";
            string mOrderByClause = Regex.Replace(searchCriteria.OrderByClause, @"<[^>]+>|&nbsp;", "").Trim();
            mOrderByClause = mOrderByClause.Replace("\r\n", "");
            string mWhereClause = Regex.Replace(searchCriteria.WhereClause, @"<[^>]+>|&nbsp;", "").Trim();
            DataTable mRetVal;
            SqlParameter[] mParameters =
             {
              new SqlParameter("@P_Columns", searchCriteria.Columns),
              new SqlParameter("@P_OrderByClause", mOrderByClause),
              new SqlParameter("@P_PageSize", searchCriteria.PageSize),
              new SqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
              new SqlParameter("@P_TableOrView", searchCriteria.TableOrView),
              new SqlParameter("@P_WhereClause", mWhereClause)
             };
            mRetVal = base.GetDataTable(mStoredProcedure, mParameters);
            return mRetVal;
        }
    }
}


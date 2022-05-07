using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Data.SqlClient;

namespace GrowthWare.DataAccess.SQLServer.Base
{
    /// <summary>
    /// Performs all data store interaction to SQL Server.
    /// </summary>
    public abstract class DSearch : DDBInteraction
    {
        protected virtual DataTable Search(MSearchCriteria searchCriteria, string tableOrView)
        {
            if (searchCriteria == null) throw new ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in Visual Basic)!");
            string mStoredProcedure = "ZGWSystem.Get_Paginated_Data";
            DataTable mRetVal;
            SqlParameter[] mParameters =
             {
              new SqlParameter("@P_Columns", searchCriteria.Columns),
              new SqlParameter("@P_OrderByColumn", searchCriteria.OrderByColumn),
              new SqlParameter("@P_OrderByDirection", searchCriteria.OrderByDirection),
              new SqlParameter("@P_PageSize", searchCriteria.PageSize),
              new SqlParameter("@P_SelectedPage", searchCriteria.SelectedPage),
              new SqlParameter("@P_TableOrView", tableOrView),
              new SqlParameter("@P_WhereClause", searchCriteria.WhereClause)
             };
            mRetVal = base.GetDataTable(mStoredProcedure, mParameters);
            return mRetVal;
        }
    }
}

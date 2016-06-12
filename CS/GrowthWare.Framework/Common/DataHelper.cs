using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.Common
{
    /// <summary>
    /// Class DataHelper is a helper class for System.Data objects
    /// </summary>
    public static class DataHelper
    {
        /// <summary>
        /// Gets the total name of the row column.
        /// </summary>
        /// <value>The total name of the row column.</value>
        public static String TotalRowColumnName { get { return "TotalRows"; } }
        /// <summary>
        /// Gets the name of the row number column.
        /// </summary>
        /// <value>The name of the row number column.</value>
        public static String RowNumberColumnName { get { return "RowNumber"; } }

        /// <summary>
        /// Adds the auto increment field named using the RowNumberColumnName property.
        /// </summary>
        /// <param name="table">The table.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public static void AddAutoIncrementField(ref DataTable table)
        {
            if (table == null) throw new ArgumentNullException("table", "table cannot be a null reference (Nothing in VB) or empty!");
            AddAutoIncrementField(ref table, RowNumberColumnName);
        }

        /// <summary>
        /// Adds the auto increment field.
        /// </summary>
        /// <param name="table">The table.</param>
        /// <param name="columnName">Name of the column.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public static void AddAutoIncrementField(ref DataTable table, string columnName)
        {
            if (table == null) throw new ArgumentNullException("table", "table cannot be a null reference (Nothing in VB) or empty!");
            DataColumn mColumn = null;
            try
            {
                if (!table.Columns.Contains(columnName))
                {
                    mColumn = new DataColumn(columnName, Type.GetType("System.Int32"));
                    mColumn.AutoIncrement = true;
                    mColumn.AutoIncrementSeed = 1;
                    mColumn.AutoIncrementStep = 1;
                    table.Columns.Add(mColumn);
                    int intCtr = 0;
                    foreach (DataRow mRow in table.Rows)
                    {
                        intCtr += 1;
                        mRow[columnName] = intCtr;
                    }
                    mColumn.ReadOnly = true;
                }
            }
            catch (Exception ex)
            {
                Logger mLog = Logger.Instance();
                mLog.Error(ex);
                throw;
            }
            finally 
            {
                if (mColumn != null) mColumn.Dispose();
            }
        }

        /// <summary>
        /// Adds the total rows field.
        /// </summary>
        /// <param name="table">The table.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public static void AddTotalRowsField(ref DataTable table)
        {
            if (table == null) throw new ArgumentNullException("table", "table cannot be a null reference (Nothing in VB) or empty!");
            string mColumnName = TotalRowColumnName;
            if (!table.Columns.Contains(mColumnName))
            {
                DataColumn mColumn = new DataColumn(mColumnName, Type.GetType("System.Int32"));
                table.Columns.Add(mColumn);
                int mRowCount = table.Rows.Count;
                foreach (DataRow mRow in table.Rows)
                {
                    mRow[mColumnName] = mRowCount;
                }
                mColumn.ReadOnly = true;
            }

        }

        /// <summary>
        /// Gets the page of data.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="sort">DataView.Sort</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>DataTable.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public static DataTable GetPageOfData(ref DataTable dataTable, string sort, MSearchCriteria searchCriteria)
        {
            if (dataTable == null) throw new ArgumentNullException("dataTable", "dataTable cannot be a null reference (Nothing in VB) or empty!");
            if (searchCriteria == null) throw new ArgumentNullException("searchCriteria", "searchCriteria cannot be a null reference (Nothing in VB) or empty!");
            // create a dataview object
            DataView mSortingDataView = dataTable.DefaultView;
            // apply any sorting using the searchCriteria
            if (sort == null)
            {
                mSortingDataView.Sort = searchCriteria.OrderByColumn + " " + searchCriteria.OrderByDirection;
            }
            else
            {
                mSortingDataView.Sort = sort;
            }

            // apply filtering
            if (searchCriteria.WhereClause.Trim() != "1 = 1")
            {
                mSortingDataView.RowFilter = searchCriteria.WhereClause;
            }
            DataTable mTempTable = mSortingDataView.Table.Clone();
            foreach (DataRowView item in mSortingDataView)
            {
                mTempTable.ImportRow(item.Row);
            }
            // add the total rows field
            AddTotalRowsField(ref mTempTable);
            // add the rownumber field
            AddAutoIncrementField(ref mTempTable);
            mSortingDataView = mTempTable.DefaultView;
            // apply paging data filter logic
            int mStartingRow = 1;
            if (searchCriteria.SelectedPage > 1)
            {
                mStartingRow = searchCriteria.PageSize * (searchCriteria.SelectedPage - 1);
            }
            int mEndingRow = mStartingRow + searchCriteria.PageSize;
            mSortingDataView.RowFilter = "RowNumber >= " + mStartingRow.ToString(CultureInfo.InvariantCulture) + " and RowNumber <= " + mEndingRow.ToString(CultureInfo.InvariantCulture);
            DataTable mRetTable = mSortingDataView.Table.Clone();
            foreach (DataRowView item in mSortingDataView)
            {
                mRetTable.ImportRow(item.Row);
            }
            return mRetTable;
        }

        /// <summary>
        /// Gets the page of data.
        /// </summary>
        /// <param name="dataTable">The data table.</param>
        /// <param name="searchCriteria">The search criteria.</param>
        /// <returns>DataTable.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public static DataTable GetPageOfData(ref DataTable dataTable, MSearchCriteria searchCriteria)
        {
            return GetPageOfData(ref dataTable, null, searchCriteria);
        }

        /// <summary>
        /// Gets a table given a DataView.
        /// </summary>
        /// <param name="dataView">The data view.</param>
        /// <returns>DataTable.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", MessageId = "0#")]
        public static DataTable GetTable(ref DataView dataView)
        {
            if (dataView == null) throw new ArgumentNullException("dataView", "table cannot be a null reference (Nothing in VB) or empty!");
            DataTable mRetVal = dataView.Table.Clone();
            foreach (DataRowView item in dataView)
            {
                mRetVal.ImportRow(item.Row);
            }
            return mRetVal;
        }
    }
}

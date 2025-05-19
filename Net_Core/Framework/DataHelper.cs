using GrowthWare.Framework.Models;
using System;
using System.Data;
using System.Globalization;
using System.Text;

namespace GrowthWare.Framework;

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
    public static void AddAutoIncrementField(ref DataTable table)
    {
        if (table == null) throw new ArgumentNullException(nameof(table), "table cannot be a null reference (Nothing in VB) or empty!");
        AddAutoIncrementField(ref table, RowNumberColumnName);
    }

    /// <summary>
    /// Adds the auto increment field.
    /// </summary>
    /// <param name="table">The table.</param>
    /// <param name="columnName">Name of the column.</param>
    public static void AddAutoIncrementField(ref DataTable table, string columnName)
    {
        if (table == null) throw new ArgumentNullException(nameof(table), "table cannot be a null reference (Nothing in VB) or empty!");
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
        catch (Exception)
        {
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
    public static void AddTotalRowsField(ref DataTable table)
    {
        if (table == null) throw new ArgumentNullException(nameof(table), "table cannot be a null reference (Nothing in VB) or empty!");
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
    /// Returns a JSON string from the given table.
    /// </summary>
    /// <param name="dataTable"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    public static string GetJsonStringFromTable(ref DataTable dataTable)
    {
        if (dataTable == null) throw new ArgumentNullException(nameof(dataTable), "table cannot be a null reference (Nothing in VB) or empty!");
        string mRetVal = string.Empty;
        var mStringBuilder = new StringBuilder();
        if (dataTable.Rows.Count > 0)
        {
            mStringBuilder.Append("[");
            for (int i = 0; i < dataTable.Rows.Count; i++)
            {
                mStringBuilder.Append("{");
                for (int j = 0; j < dataTable.Columns.Count; j++)
                {
                    if (j < dataTable.Columns.Count - 1)
                    {
                        mStringBuilder.Append("\"" + dataTable.Columns[j].ColumnName.ToString() + "\":" + "\"" + dataTable.Rows[i][j].ToString().Replace("\"", "\\\"") + "\",");
                    }
                    else if (j == dataTable.Columns.Count - 1)
                    {
                        mStringBuilder.Append("\"" + dataTable.Columns[j].ColumnName.ToString() + "\":" + "\"" + dataTable.Rows[i][j].ToString().Replace("\"", "\\\"") + "\"");
                    }
                }
                if (i == dataTable.Rows.Count - 1)
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
        mRetVal = mStringBuilder.ToString();
        mStringBuilder.Clear();
        mStringBuilder.Capacity = 0;
        return mRetVal;
    }

    /// <summary>
    /// Gets the page of data.  Was created for instances when the data source
    /// does not paginate the data and it becomes necessary to do the pagination
    /// separately.
    /// </summary>
    /// <param name="dataTable">The data table.</param>
    /// <param name="sort">DataView.Sort</param>
    /// <param name="searchCriteria">The search criteria.</param>
    /// <returns>DataTable.</returns>
    public static DataTable GetPageOfData(ref DataTable dataTable, string sort, MSearchCriteria searchCriteria)
    {
        if (dataTable == null) throw new ArgumentNullException(nameof(dataTable), "dataTable cannot be a null reference (Nothing in VB) or empty!");
        if (searchCriteria == null) throw new ArgumentNullException(nameof(searchCriteria), "searchCriteria cannot be a null reference (Nothing in VB) or empty!");
        // create a dataview object
        DataView mSortingDataView = dataTable.DefaultView;
        // apply any sorting using the searchCriteria
        if (sort == null)
        {
            mSortingDataView.Sort = searchCriteria.OrderByClause;
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
    public static DataTable GetPageOfData(ref DataTable dataTable, MSearchCriteria searchCriteria)
    {
        return GetPageOfData(ref dataTable, null, searchCriteria);
    }

    /// <summary>
    /// Gets a table given a DataView.
    /// </summary>
    /// <param name="dataView">The data view.</param>
    /// <returns>DataTable.</returns>
    public static DataTable GetTable(ref DataView dataView)
    {
        if (dataView == null) throw new ArgumentNullException(nameof(dataView), "table cannot be a null reference (Nothing in VB) or empty!");
        DataTable mRetVal = dataView.Table.Clone();
        foreach (DataRowView item in dataView)
        {
            mRetVal.ImportRow(item.Row);
        }
        return mRetVal;
    }

    /// <summary>
    /// Sorts the table based on the column and direction.
    /// </summary>
    /// <param name="dataView">A reverance to your data table</param>
    /// <param name="string">The column to sort on</param>
    /// <param name="direction">The direction to sort on ASC or DESC</param>
    /// <returns>Updates your dataTable</returns>
    /// <remarks>This not generic enough, should expand on it later</remarks>
    public static void SortTable(ref DataTable dataTable, string column, string direction)
    {
        // TODO: Added multiple column sort support
        DataView mDataView = dataTable.DefaultView;
        NaturalComparer mNaturalComparer = new NaturalComparer();
        DataTable mNewDataTable = null;
        if (dataTable != null && dataTable.Rows != null && dataTable.Rows.Count > 0)
        {
            if (direction.Equals("ASC", StringComparison.InvariantCultureIgnoreCase))
            {
                mNewDataTable = mDataView.Table.AsEnumerable().OrderBy(x => x.Field<string>(column), mNaturalComparer).CopyToDataTable();
            }
            else
            {
                mNewDataTable = mDataView.Table.AsEnumerable().OrderByDescending(x => x.Field<string>(column), mNaturalComparer).CopyToDataTable();
            }
            dataTable = mNewDataTable.Copy();
        }
    }
}

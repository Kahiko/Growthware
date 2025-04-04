using System;
using System.Data;
using System.Globalization;

namespace GrowthWare.Framework;

public static class DataRowHelper
{
    /** 
      * The code here was pulled out of an AbstractDatabaseFunctions old class
      * that was replaced by ADatabaseTable class.  I felt that these methods could
      * be usefull outside of the conext of a database table class, so separated them.
      */

    /// <summary>
    /// Returns a boolean given the DataRow and Column name for either bit or int values.
    /// </summary>
    /// <param name="dataRow">The dataRow.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <returns>Boolean.</returns>
    /// <remarks>Integer or int values not equal to 0 are considered true</remarks>
    public static Boolean GetBool(DataRow dataRow, String columnName)
    {
        bool mRetVal = false;
        if (RowHasValue(dataRow, columnName))
        {
            string mRowValue = dataRow[columnName].ToString();
            if (String.Equals(mRowValue, "1") || string.Equals(mRowValue, "true", StringComparison.OrdinalIgnoreCase))
            {
                mRetVal = true;
            }
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns a DateTime given the a DataRow and Column name and the default value.
    /// </summary>
    /// <param name="dataRow">DataRow</param>
    /// <param name="columnName">String</param>
    /// <param name="defaultDateTime">DateTime</param>
    /// <returns>DateTime</returns>
    /// <remarks></remarks>
    public static DateTime GetDateTime(DataRow dataRow, String columnName, DateTime defaultDateTime)
    {
        DateTime mRetVal = defaultDateTime;
        if (RowHasValue(dataRow, columnName))
        {
            mRetVal = DateTime.Parse(dataRow[columnName].ToString(), CultureInfo.CurrentCulture);
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns an int given the DataRow and Column name
    /// </summary>
    /// <param name="dataRow"></param>
    /// <param name="columnName"></param>
    /// <returns>-1 if no value was found</returns>
    public static Int32 GetInt(DataRow dataRow, String columnName)
    {
        int mRetVal = -1;
        if (RowHasValue(dataRow, columnName))
        {
            mRetVal = int.Parse(dataRow[columnName].ToString(), CultureInfo.InvariantCulture);
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns a string given the DataRow and Column name
    /// </summary>
    /// <param name="dataRow"></param>
    /// <param name="columnName"></param>
    /// <returns>string value or empty</returns>
    public static String GetString(DataRow dataRow, String columnName)
    {
        String mRetVal = string.Empty;
        if (RowHasValue(dataRow, columnName))
        {
            mRetVal = dataRow[columnName].ToString().Trim();
        }
        return mRetVal;
    }

    /// <summary>
    /// Determines if a DataRow has a value for a given column name. Checks if the dataRow is not null, if the column exists in the Table, and if the value is not DBNull.
    /// </summary>
    /// <param name="dataRow">DataRow</param>
    /// <param name="columnName">name of the column</param>
    /// <returns>true if the row has a value for the given column</returns>
    /// <remarks></remarks>
    public static bool RowHasValue(DataRow dataRow, String columnName)
    {
        return dataRow != null && dataRow.Table.Columns.Contains(columnName) && !(Convert.IsDBNull(dataRow[columnName]));
    }
}


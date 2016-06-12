using System;
using System.Data;
using System.Globalization;

namespace GrowthWare.Framework.Model.Profiles.Base
{
    /// <summary>
    /// Class MDBFunctions servers as the base class for profiles.
    /// Code only no properties.
    /// Inherit from MProfile if you need the base properties as well.
    /// </summary>
    public abstract class MDatabaseFunctions
    {
        /// <summary>
        ///Returns a boolean given the DataRow and Column name for either bit or int values.
        /// </summary>
        /// <param name="dataRow">The datarow.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>Boolean.</returns>
        /// <remarks>Integer or int values not equal to 0 are considered true</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bool"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected Boolean GetBool(DataRow dataRow, String columnName)
        {
            bool mRetVal = false;
            if (dataRow != null && dataRow.Table.Columns.Contains(columnName) && !(Convert.IsDBNull(dataRow[columnName])))
            {
                if (dataRow[columnName].ToString() == "1" || dataRow[columnName].ToString().ToUpper(CultureInfo.InvariantCulture) == "TRUE")
                {
                    mRetVal = true;
                }
            }
            return mRetVal;
        }

        /// <summary>
        /// Returns a DateTime given the a DataRow and Column name and the defaul value.
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <param name="columnName">String</param>
        /// <param name="defaultDateTime">DateTime</param>
        /// <returns>DateTime</returns>
        /// <remarks></remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected DateTime GetDateTime(DataRow dataRow, String columnName, DateTime defaultDateTime)
        {
            DateTime mRetVal = defaultDateTime;
            if (dataRow != null && dataRow.Table.Columns.Contains(columnName) && !(Convert.IsDBNull(dataRow[columnName])))
            {
                mRetVal = DateTime.Parse(dataRow[columnName].ToString(), CultureInfo.CurrentCulture);
            }
            return mRetVal;
        }

        /// <summary>
        /// Returns a int given the a DataRow and Column name.
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <param name="columnName">String</param>
        /// <returns>int</returns>
        /// <remarks></remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "int"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected Int32 GetInt(DataRow dataRow, String columnName)
        {
            int mRetVal = -1;
            if (dataRow != null && dataRow.Table.Columns.Contains(columnName) && !(Convert.IsDBNull(dataRow[columnName])))
            {
                mRetVal = int.Parse(dataRow[columnName].ToString(), CultureInfo.InvariantCulture);
            }
            return mRetVal;
        }

        /// <summary>
        /// Returns a String given the a DataRow and Column name.
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        /// <param name="columnName">String</param>
        /// <returns>String</returns>
        /// <remarks></remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected String GetString(DataRow dataRow, String columnName)
        {
            String mRetVal = string.Empty;
            if (dataRow != null && dataRow.Table.Columns.Contains(columnName) && !(Convert.IsDBNull(dataRow[columnName])))
            {
                mRetVal = dataRow[columnName].ToString().Trim();
            }
            return mRetVal;
        }

    }
}

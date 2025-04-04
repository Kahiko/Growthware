using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Reflection;
using GrowthWare.Framework.Interfaces;

namespace GrowthWare.Framework.Models.Base
{
    /// <summary>
    /// Class MDBFunctions servers as the base class for profiles.
    /// Code only no properties.
    /// Inherit from MProfile if you need the base properties as well.
    /// </summary>
    [Obsolete("Obsolete replaced by ADatabaseTable", false)]
    public abstract class AbstractDatabaseFunctions : IDatabaseFunctions
    {
        // Only needs to be set if you intend on using BulkInsert
        protected string m_ForeignKeyName = string.Empty;

        protected bool m_ForeignKeyIsNumber = false;

        protected string m_PrimaryKeyName = string.Empty;

        protected string m_TableName = string.Empty;

        protected HashSet<Type> m_NumTypes = new HashSet<Type>
        {
            typeof(int),  typeof(double),  typeof(decimal),
            typeof(long), typeof(short),   typeof(sbyte),
            typeof(byte), typeof(ulong),   typeof(ushort),
            typeof(uint), typeof(float)// ,   typeof(BigInteger)
        };

        /// <summary>
        ///Returns a boolean given the DataRow and Column name for either bit or int values.
        /// </summary>
        /// <param name="dataRow">The dataRow.</param>
        /// <param name="columnName">Name of the column.</param>
        /// <returns>Boolean.</returns>
        /// <remarks>Integer or int values not equal to 0 are considered true</remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1720:IdentifiersShouldNotContainTypeNames", MessageId = "bool"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected Boolean GetBool(DataRow dataRow, String columnName)
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
        /// Returns the columns including the primary key
        /// </summary>
        /// <returns></returns>
        string IDatabaseFunctions.GetColumnNames()
        {
            return this.GetColumnNames(true);
        }

        protected string GetColumnNames(bool usePrimaryKey)
        {
            string mRetVal = string.Empty;
            PropertyInfo[] mPropertyInfo = this.GetType().GetProperties();
            foreach (PropertyInfo mPropertyItem in mPropertyInfo)
            {
                if (!string.Equals(mPropertyItem.Name, m_PrimaryKeyName, StringComparison.OrdinalIgnoreCase))
                {
                    mRetVal += "[" + mPropertyItem.Name + "] ,";
                }
                else if (!string.Equals(mPropertyItem.Name, m_PrimaryKeyName, StringComparison.OrdinalIgnoreCase) || usePrimaryKey)
                {
                    mRetVal += "[" + mPropertyItem.Name + "] ,";
                }
            }
            mRetVal = mRetVal.Substring(0, mRetVal.Length - 2);
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
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        protected DateTime GetDateTime(DataRow dataRow, String columnName, DateTime defaultDateTime)
        {
            DateTime mRetVal = defaultDateTime;
            if (RowHasValue(dataRow, columnName))
            {
                mRetVal = DateTime.Parse(dataRow[columnName].ToString(), CultureInfo.CurrentCulture);
            }
            return mRetVal;
        }

        DataTable IDatabaseFunctions.GetEmptyTable(string tableName, bool includePrimaryKey)
        {
            DataTable mTempRetTable = null;
            DataTable mRetTable = null;
            mTempRetTable = new DataTable(tableName);
            string mPrimaryKeyName = this.m_PrimaryKeyName.Replace("[","").Replace("]","");
            try
            {
                mTempRetTable.Locale = CultureInfo.InvariantCulture;
                PropertyInfo[] mPropertyInfo = this.GetType().GetProperties();
                foreach (PropertyInfo mPropertyItem in mPropertyInfo)
                {
                    if (mPrimaryKeyName.ToLowerInvariant() != mPropertyItem.Name.ToLowerInvariant())
                    {
                        var mPropertyType = mPropertyItem.PropertyType;
                        if (mPropertyType.IsGenericType && mPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            mPropertyType = mPropertyType.GetGenericArguments()[0];
                        }
                        if(mPropertyItem.Name.ToLowerInvariant() != mPrimaryKeyName.ToLowerInvariant()) 
                        {
                            mTempRetTable.Columns.Add(mPropertyItem.Name, mPropertyType);
                        } 
                        else if (includePrimaryKey) 
                        {
                            mTempRetTable.Columns.Add(mPropertyItem.Name, mPropertyType);
                        }
                    }
                }
                mRetTable = mTempRetTable;
            }
            catch (NullReferenceException)
            {
                throw;
            }
            finally
            {
                if (mTempRetTable != null) mTempRetTable.Dispose();
            }
            return mRetTable;
        }

        string IDatabaseFunctions.GetInsert(bool usePrimaryKey)
        {
            // this is just in case the deriving class did not set the value.
            if (string.IsNullOrEmpty(m_PrimaryKeyName))
            {
                throw new NullReferenceException("The m_PrimaryKeyName can not be null");
            }
            if (string.IsNullOrEmpty(m_TableName))
            {
                throw new NullReferenceException("The m_TableName can not be null");
            }
            string mRetVal = this.getInsertTableFragment(usePrimaryKey);
            mRetVal += " VALUES " + this.getInsertDataFragment(usePrimaryKey);
            return mRetVal;
        }

        private string getInsertTableFragment(bool usePrimaryKey)
        {
            // this is just in case the deriving class did not set the value.
            if (string.IsNullOrEmpty(m_PrimaryKeyName))
            {
                throw new NullReferenceException("The m_PrimaryKeyName can not be null");
            }
            if (string.IsNullOrEmpty(m_TableName))
            {
                throw new NullReferenceException("The m_TableName can not be null");
            }
            string mRetVal = "INSERT INTO {0} (";
            mRetVal += this.GetColumnNames(usePrimaryKey);
            mRetVal = string.Format(mRetVal, m_TableName);
            PropertyInfo[] mPropertyInfo = this.GetType().GetProperties();
            mRetVal += ")";
            return mRetVal;
        }

        private string getInsertDataFragment(bool usePrimaryKey)
        {
            string mRetVal = "(";
            PropertyInfo[] mPropertyInfo = this.GetType().GetProperties();
            foreach (PropertyInfo mPropertyItem in mPropertyInfo)
            {
                if (!string.Equals(mPropertyItem.Name, m_PrimaryKeyName, StringComparison.OrdinalIgnoreCase))
                {
                    if (IsBoolean(mPropertyItem.PropertyType))
                    {
                        if (mPropertyItem.GetValue(this, null) != null)
                        {
                            mRetVal += (Convert.ToUInt32(mPropertyItem.GetValue(this, null))).ToString() + " ,";
                        }
                        else
                        {
                            mRetVal += "'Property was null' ,";
                        }
                    }
                    else if (!IsNumeric(mPropertyItem.PropertyType))
                    {
                        if (mPropertyItem.GetValue(this, null) != null)
                        {
                            mRetVal += "'" + mPropertyItem.GetValue(this, null).ToString().Replace("'", "''") + "' ,";
                        }
                        else
                        {
                            mRetVal += "'Property was null' ,";
                        }
                    }
                    else
                    {
                        if (mPropertyItem.GetValue(this, null) != null)
                        {
                            mRetVal += mPropertyItem.GetValue(this, null).ToString() + " ,";
                        }
                        else
                        {
                            mRetVal += "'Property was null' ,";
                        }
                    }
                }
                else if (!string.Equals(mPropertyItem.Name, m_PrimaryKeyName, StringComparison.OrdinalIgnoreCase) || usePrimaryKey)
                {
                    if (IsBoolean(mPropertyItem.PropertyType))
                    {
                        if (mPropertyItem.GetValue(this, null) != null)
                        {
                            mRetVal += (Convert.ToUInt32(mPropertyItem.GetValue(this, null))).ToString() + " ,";
                        }
                        else
                        {
                            mRetVal += "'Property was null' ,";
                        }
                    }
                    else if (!IsNumeric(mPropertyItem.PropertyType))
                    {
                        if (mPropertyItem.GetValue(this, null) != null)
                        {
                            mRetVal += "'" + mPropertyItem.GetValue(this, null).ToString() + "' ,";
                        }
                        else
                        {
                            mRetVal += "'Property was null' ,";
                        }
                    }
                    else
                    {
                        if (mPropertyItem.GetValue(this, null) != null)
                        {
                            mRetVal += mPropertyItem.GetValue(this, null).ToString() + " ,";
                        }
                        else
                        {
                            mRetVal += "'Property was null' ,";
                        }
                    }
                }
            }
            mRetVal = mRetVal.Substring(0, mRetVal.Length - 1);
            mRetVal += ")";
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
            if (RowHasValue(dataRow, columnName))
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
            if (RowHasValue(dataRow, columnName))
            {
                mRetVal = dataRow[columnName].ToString().Trim();
            }
            return mRetVal;
        }

        string IDatabaseFunctions.GetForeignKeyName()
        {
            return this.m_ForeignKeyName;
        }

        string IDatabaseFunctions.GetPrimaryKeyName()
        {
            return this.m_PrimaryKeyName;
        }

        string IDatabaseFunctions.GetTableName()
        {
            return this.m_TableName;
        }

        /// <summary>
        /// Helper to determine if the type is numeric
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>        
        protected virtual bool IsNumeric(Type type)
        {
            bool mIsNumeric = false;

            if (type != null)
            {
                mIsNumeric = m_NumTypes.Contains(type);
            }

            return mIsNumeric;
        }

        /// <summary>
        /// Helper to determine if the type is numeric
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>        
        protected virtual bool IsBoolean(Type type)
        {
            bool mIsBoolean = false;

            if (type != null)
            {
                mIsBoolean = type == typeof(bool);
            }

            return mIsBoolean;
        }

        bool IDatabaseFunctions.IsForeignKeyNumber()
        {
            return this.m_ForeignKeyIsNumber;
        }

        protected static bool RowHasValue(DataRow dataRow, String columnName)
        {
            return dataRow != null && dataRow.Table.Columns.Contains(columnName) && !(Convert.IsDBNull(dataRow[columnName]));
        }
    }
}

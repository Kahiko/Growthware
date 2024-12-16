using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using GrowthWare.Framework.Interfaces;

namespace GrowthWare.Framework.Models.Base;

[AttributeUsage(AttributeTargets.Property)]
public class IgnorePropertyAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class PrimaryKeyAttribute : Attribute { }

[AttributeUsage(AttributeTargets.Property)]
public class ColumnNameAttribute : Attribute
{
    public string Name { get; }

    public ColumnNameAttribute(string name)
    {
        Name = name;
    }
}

/// <summary>
/// A Base class to contain common properties and methods for a database table
/// </summary>
public abstract class ADatabaseTable : IDatabaseTable
{

    #region Private Fields
        private bool m_DisposedValue;

        protected string m_ForeignKeyName = string.Empty;

        protected bool m_IsForeignKeyNumeric = true;

        protected static HashSet<Type> m_NumTypes = new HashSet<Type>
            {
                typeof(int),  typeof(double),  typeof(decimal),
                typeof(long), typeof(short),   typeof(sbyte),
                typeof(byte), typeof(ulong),   typeof(ushort),
                typeof(uint), typeof(float)// ,   typeof(BigInteger)
            };

        protected static PropertyInfo[] m_PropertyInfoArray = null;
        
        static StringBuilder m_StringBuilder = new();

        protected static string m_TableName = string.Empty;

        protected static bool m_UseBrackets = true;
    #endregion

    /// <summary>
    /// Implements IDispose
    /// </summary>
    /// <param name="disposing">Boolean</param>
    /// <remarks></remarks>
    protected virtual void Dispose(bool disposing)
    {
        // Check to see if Dispose has already been called.
        if (!m_DisposedValue)
        {
            if (disposing)
            {
                // Dispose managed resources if you have any.
                m_StringBuilder = null;
            }
            // TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            // TODO: set large fields to null.

        }
        m_DisposedValue = true;
    }

    /// <summary>
    /// Implements Dispose
    /// </summary>
    /// <remarks></remarks>
    public void Dispose()
    {
        //Do not change this code.  Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    [IgnorePropertyAttribute]
    public string ForeignKeyName
    {
        get {return m_ForeignKeyName;}        
    }

    [IgnorePropertyAttribute]
    public bool IsForeignKeyNumeric
    {
        get
        {
            return m_IsForeignKeyNumeric;
        }
    }

    [IgnorePropertyAttribute]
    public string TableName
    { 
        get
        {
            if (string.IsNullOrWhiteSpace(m_TableName))
            {
                throw new InvalidOperationException($"The deriving class must set the m_TableName field.");
            }
            return m_TableName;
        }
    }

    [IgnorePropertyAttribute]
    public bool UseBrackets
    {
        get
        {
            return m_UseBrackets;
        }
    }

    /// <summary>
    /// Helper method to format the values for SQL (handle strings, nulls, etc.)
    /// </summary>
    /// <param name="value"></param>
    /// <returns>string</returns>
    private static string formatValue(object value)
    {
        if (value == null) return "NULL";
        if (value is string || value is DateTime)
        {
            return $"'{value.ToString().Replace("'", "''")}'"; // Escape single quotes
        }
        return value.ToString(); // For numbers or other types
    }

    /// <summary>
    /// Static method to generate a DELETE statement;
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="keyColumn"></param>
    /// <returns></returns>
    public static string GenerateDeleteWithParameters<T>(string keyColumn) where T : ADatabaseTable
    {
        string mWhereClause = $"WHERE [{keyColumn}] = @{keyColumn}";
        if (!m_UseBrackets)
        { 
            mWhereClause = mWhereClause.Replace("[", "").Replace("]", "");
        }
        string mRetVal = $"DELETE FROM {m_TableName} {mWhereClause};";
        return mRetVal;
    }

    /// <summary>
    /// Generates a DELETE statement specifying the keyColumn, the keyColumn's is derived from the propeerty
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="keyColumn"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public string GenerateDeleteWithValues<T>(string keyColumn) where T : ADatabaseTable
    {
        string mKeyValue = getPropertyValue<T>(keyColumn);
        if (string.IsNullOrWhiteSpace(mKeyValue)) 
        {
            throw new InvalidOperationException($"The keyColumn {keyColumn} has no value and cannot be deleted.");
        }
        return GenerateDeleteWithValues<T>(keyColumn, mKeyValue);
    }

    /// <summary>
    /// Generates a DELETE statement specifying the keyColumn and keyValue
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="keyColumn"></param>
    /// <param name="keyValue"></param>
    /// <returns></returns>
    /// <remarks>keyValue should be single quoted if it is a string</remarks>
    public string GenerateDeleteWithValues<T>(string keyColumn, string keyValue) where T : ADatabaseTable
    {
        string mWhereClause = $"WHERE [{keyColumn}] = {keyValue}";
        if (!m_UseBrackets)
        { 
            mWhereClause = mWhereClause.Replace("[", "").Replace("]", "");
        }
        string mRetVal = $"DELETE FROM {m_TableName} {mWhereClause};";
        return mRetVal;
    }

    /// <summary>
    /// Static method to generate an INSERT statement with parameters
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="includePrimaryKey">Specifies whether to include the primary key.</param>
    /// <returns></returns>
    public static string GenerateInsertWithParameters<T>(bool includePrimaryKey = false) where T : ADatabaseTable
    {
        string mColumnNames = getColumnNames<T>(includePrimaryKey);
        foreach (PropertyInfo mPropertyItem in getProperties<T>(includePrimaryKey))
        {
            m_StringBuilder.Append("@" + getColumnName(mPropertyItem) + ", ");
        }
        string mParameterNames = m_StringBuilder.ToString().Substring(0, m_StringBuilder.ToString().Length - 2);
        m_StringBuilder.Clear();
        return $"INSERT INTO {m_TableName} ({mColumnNames}) VALUES ({mParameterNames});";
    }

    /// <summary>
    /// Generates an INSERT SQL statement for the current instance with actual values from the properties.
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="includePrimaryKey">Specifies whether to include the primary key.</param>
    /// <returns>A string representing the INSERT SQL statement with column names and their corresponding values.</returns>
    public string GenerateInsertWithValues<T>(bool includePrimaryKey = false) where T : ADatabaseTable
    {
        string mColumnNames = getColumnNames<T>(includePrimaryKey);
        string mValues = this.getPropertyValues<T>(includePrimaryKey);
        return $"INSERT INTO {m_TableName} ({mColumnNames}) VALUES ({mValues});";
    }

    /// <summary>
    /// Static method to generate an UPDATE statement using parameters
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="keyColumn"></param>
    /// <returns></returns>
    public static string GenerateUpdateWithParameters<T>(string keyColumn) where T : ADatabaseTable
    {
        var mSetClauses = string.Join(", ", getProperties<T>(false).Select(p => $"[{getColumnName(p)}] = @{getColumnName(p)}"));
        if (!m_UseBrackets) 
        { 
            mSetClauses = mSetClauses.Replace("[", "").Replace("]", ""); 
        }
        return $"UPDATE {m_TableName} SET {mSetClauses} WHERE {keyColumn} = @{keyColumn};";
    }

    /// <summary>
    /// Generates an UPDATE SQL statement for the current instance with actual values from the properties
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="keyColumn"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public string GenerateUpdateWithValues<T>(string keyColumn) where T : ADatabaseTable
    {
        string mKeyValue = getPropertyValue<T>(keyColumn);
        if (string.IsNullOrWhiteSpace(mKeyValue)) 
        {
            throw new InvalidOperationException($"The keyColumn {keyColumn} has no value and cannot be updated.");
        }
        return GenerateUpdateWithValues<T>(keyColumn, mKeyValue);
    }

    /// <summary>
    /// Generates an UPDATE SQL statement for the current instance with actual values from the properties specifing the keyColumn and it's value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keyColumn"></param>
    /// <param name="keyValue"></param>
    /// <returns>string</returns>
    /// <remarks>keyValue should be single quoted if it is a string</remarks>
    public string GenerateUpdateWithValues<T>(string keyColumn, string keyValue) where T : ADatabaseTable
    {
        m_StringBuilder.Append($"UPDATE {m_TableName} SET ");
        foreach (PropertyInfo mPropertyItem in getProperties<T>(false))
        {
            if (m_UseBrackets) 
            {
                m_StringBuilder.Append($"[{getColumnName(mPropertyItem)}] = {formatValue(mPropertyItem.GetValue(this))}, ");
            } else 
            {
                m_StringBuilder.Append($"{getColumnName(mPropertyItem)} = {formatValue(mPropertyItem.GetValue(this))}, ");
            }
        }
        string mRetVal = m_StringBuilder.ToString().Substring(0, m_StringBuilder.ToString().Length - 2);
        mRetVal += $" WHERE {keyColumn} = {keyValue};";
        m_StringBuilder.Clear();
        return mRetVal;
    }

    /// <summary>
    ///Returns a boolean given the DataRow and Column name for either bit or int values.
    /// </summary>
    /// <param name="dataRow">The dataRow.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <returns>Boolean.</returns>
    /// <remarks>Integer or int values not equal to 0 are considered true</remarks>
    protected static Boolean GetBool(DataRow dataRow, String columnName)
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
    /// Helper method to get the column name from the attribute or property name
    /// </summary>
    /// <param name="property"></param>
    /// <returns></returns>
    private static string getColumnName(PropertyInfo property)
    {
        var attribute = property.GetCustomAttribute<ColumnNameAttribute>();
        return attribute?.Name ?? property.Name; // Use the attribute name if available, otherwise use the property name
    }

    /// <summary>
    /// Static method to get all the column names either from the attribute or property name if the attribute is not present
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string GetColumnNames<T>() where T : ADatabaseTable
    {
        return getColumnNames<T>(true);
    }

    /// <summary>
    /// Helper method to get all the column names either from the attribute or property name if the attribute is not present
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="properties"></param>
    /// <returns></returns>
    private static string getColumnNames<T>(bool includePrimaryKey) where T : ADatabaseTable
    {
        foreach (PropertyInfo mPropertyItem in getProperties<T>(includePrimaryKey))
        {
            if (m_UseBrackets) 
            {
                m_StringBuilder.Append("[" + getColumnName(mPropertyItem) + "], ");
            } else 
            {
                m_StringBuilder.Append(getColumnName(mPropertyItem) + ", ");                
            }            
        }
        string mRetVal = m_StringBuilder.ToString().Substring(0, m_StringBuilder.ToString().Length - 2);
        m_StringBuilder.Clear();
        return mRetVal;
    }

    /// <summary>
    /// Helper method to get the property's value given it's name
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    private string getPropertyValue<T>(string propertyName) where T : ADatabaseTable
    {
        PropertyInfo mPropertyItem = getProperties<T>(true).FirstOrDefault(p => p.Name == propertyName);
        string mRetVal = formatValue(mPropertyItem.GetValue(this));
        if (string.IsNullOrWhiteSpace(mRetVal)) 
        {
            throw new InvalidOperationException($"The keyColumn {propertyName} has no value and cannot be updated.");
        }
        return mRetVal;
    }

    /// <summary>
    /// Helper method to get the property values
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="properties"></param>
    /// <returns>string</returns>
    private string getPropertyValues<T>(bool includePrimaryKey) where T : ADatabaseTable
    {
        foreach (PropertyInfo mPropertyItem in getProperties<T>(includePrimaryKey))
        {
            var value = mPropertyItem.GetValue(this, null);
            m_StringBuilder.Append(formatValue(value) + " ,");
        }
        string mRetVal = m_StringBuilder.ToString().Substring(0, m_StringBuilder.ToString().Length - 2);
        m_StringBuilder.Clear();
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
    protected static DateTime GetDateTime(DataRow dataRow, String columnName, DateTime defaultDateTime)
    {
        DateTime mRetVal = defaultDateTime;
        if (dataRow != null && dataRow.Table.Columns.Contains(columnName) && !(Convert.IsDBNull(dataRow[columnName])))
        {
            mRetVal = DateTime.Parse(dataRow[columnName].ToString(), CultureInfo.CurrentCulture);
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns an empty DataTable based on the T properties
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="tableName">Name of the table</param>
    /// <param name="includePrimaryKey">Specifies whether to include the primary key.</param>
    /// <returns>DataTable</returns>
    /// <exception cref="NullReferenceException"></exception>
    public static DataTable GetEmptyTable<T>(string tableName, bool includePrimaryKey) where T : ADatabaseTable
    {
        DataTable mTempRetTable = null;
        DataTable mRetTable = null;
        mTempRetTable = new DataTable(tableName);
        string mPrimaryKeyName = GetPrimaryKeyName<T>();
        if (includePrimaryKey && string.IsNullOrWhiteSpace(mPrimaryKeyName))
        {
            throw new NullReferenceException("The includePrimaryKey prameter is true and PrimaryKeyAttribute has not been set for any property.");
        }
        try
        {
            mTempRetTable.Locale = CultureInfo.InvariantCulture;
            foreach (PropertyInfo mPropertyItem in getProperties<T>(includePrimaryKey))
            {
                var mPropertyType = mPropertyItem.PropertyType;
                if (mPropertyType.IsGenericType && mPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                {
                    mPropertyType = mPropertyType.GetGenericArguments()[0];
                }
                string mPropertyName = getColumnName(mPropertyItem);
                if (includePrimaryKey && string.Equals(mPropertyName, mPrimaryKeyName, StringComparison.OrdinalIgnoreCase))
                {
                    mTempRetTable.Columns.Add(mPropertyName, mPropertyType);
                }
                if (!string.Equals(mPropertyName, mPrimaryKeyName, StringComparison.OrdinalIgnoreCase))
                {
                    mTempRetTable.Columns.Add(mPropertyName, mPropertyType);
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

    /// <summary>
    /// Returns an int given the DataRow and Column name
    /// </summary>
    /// <param name="dataRow"></param>
    /// <param name="columnName"></param>
    /// <returns>-1 if no value was found</returns>
    protected static Int32 GetInt(DataRow dataRow, String columnName)
    {
        int mRetVal = -1;
        if (dataRow != null && dataRow.Table.Columns.Contains(columnName) && !(Convert.IsDBNull(dataRow[columnName])))
        {
            mRetVal = int.Parse(dataRow[columnName].ToString(), CultureInfo.InvariantCulture);
        }
        return mRetVal;
    }

    static string GetPrimaryKeyName<T>() where T : ADatabaseTable
    {
        PropertyInfo mPrimaryKeyProperty = getProperties<T>(true).Where(p => p.IsDefined(typeof(PrimaryKeyAttribute), false)).FirstOrDefault();
        string mRetVal = string.Empty;
        if (mPrimaryKeyProperty != null)
        {
            mRetVal = getColumnName(mPrimaryKeyProperty).Replace("[", "").Replace("]", "");
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns an array of PropertyInfo
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="includePrimaryKey"></param>
    /// <returns></returns>
    private static PropertyInfo[] getProperties<T>(bool includePrimaryKey) where T : ADatabaseTable
    {
        if (m_PropertyInfoArray == null)
        {
            Type mType = typeof(T);
            m_PropertyInfoArray = mType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).ToArray();
        }
        PropertyInfo[] mPropertyInfoArray = m_PropertyInfoArray.Where(p => 
                p.CanRead &&
                !p.IsDefined(typeof(IgnorePropertyAttribute), false) &&
                (includePrimaryKey || !p.IsDefined(typeof(PrimaryKeyAttribute), false))
        ).ToArray();
        return mPropertyInfoArray;
    }

    /// <summary>
    /// Returns a string given the DataRow and Column name
    /// </summary>
    /// <param name="dataRow"></param>
    /// <param name="columnName"></param>
    /// <returns>string value or empty</returns>
    protected static String GetString(DataRow dataRow, String columnName)
    {
        String mRetVal = string.Empty;
        if (dataRow != null && dataRow.Table.Columns.Contains(columnName) && !(Convert.IsDBNull(dataRow[columnName])))
        {
            mRetVal = dataRow[columnName].ToString().Trim();
        }
        return mRetVal;
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
    /// An abstract method to setup the class
    /// </summary>
    /// <remarks>Set the following in the child class:
    ///     <br/>m_ForeignKeyName - Only used for bulk inserts
    ///     <br/>m_IsForeignKeyNumeric - Only used for bulk inserts (Defaults to true)
    ///     <br/>m_TableName - The name of the table the class represents
    ///     <br/>m_UseBrackets - Whether to use brackets or not in the column names (Defaults to true)
    /// </remarks>
    protected abstract void SetupClass();
}

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
public class DBIgnoreProperty : Attribute { } // [DBIgnoreProperty]

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DBPrimaryKey : Attribute { } // [DBPrimaryKey]

[AttributeUsage(AttributeTargets.Property)]
public class DBColumnName : Attribute // [DBColumnName("The_Column_Name")]
{
    public string Name { get; }

    public DBColumnName(string name)
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

    private bool m_IsSetDefaultSystemDateTime = false;

    private DateTime m_DefaultSystemDateTime;

    private const string m_DeleteStatementTemplate = "DELETE FROM {0} WHERE [{1}] = {2};";

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

    protected PropertyInfo[] m_PropertyInfoArray = null;

    // The StringBuilder should be cleared after every use!
    static StringBuilder m_StringBuilder = new();

    //   I don't think this is going to work well it sort of opens the code up
    // for massive errors, if this isn't set correctly in the deriving class then an
    // unexpected result will occur.  I've even seen where it has been set but the incorrect
    // value is being return from the property TableName.
    //   OK this happens when a collection of the objects are pulled from
    // cache the constructurer is not being called in the process so the SetupClass()
    // never gets callled make the m_TableName return something from a previous value
    protected static string m_TableName = string.Empty;
#endregion

#region Public Properties
    // Default System DateTime of 1/1/1753 12:00:00 AM
    [DBIgnoreProperty]
    public DateTime DefaultSystemDateTime
    {
        get
        {
            if (!m_IsSetDefaultSystemDateTime)
            {
                m_IsSetDefaultSystemDateTime = true;
                m_DefaultSystemDateTime = new(1753, 1, 1, 0, 0, 0); // 1/1/1753 12:00:00 AM
            }
            return (DateTime)m_DefaultSystemDateTime;
        }
    }

    // The name of the foreign key used when performing bulk insert.
    [DBIgnoreProperty]
    public string ForeignKeyName
    {
        get { return m_ForeignKeyName; }
    }

    // Whether the foreign key is numeric only used in bulk inserts
    [DBIgnoreProperty]
    public bool IsForeignKeyNumeric
    {
        get
        {
            return m_IsForeignKeyNumeric;
        }
    }

    // The name of the database table
    [DBIgnoreProperty]
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
#endregion

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
    /// Helper method to format the values for SQL (handle strings, nulls, etc.)
    /// </summary>
    /// <param name="value"></param>
    /// <returns>string</returns>
    private static string formatValue(object value)
    {
        if (value == null) return "NULL";
        switch (value)
        {
            case string:
            case DateTime:
                return $"'{value.ToString().Replace("'", "''")}'"; // Escape single quotes
            case bool:
                return (bool)value ? "1" : "0"; // DB values are 0 or 1
            default:
                return value.ToString(); // For numbers or other types
        }
    }

    /// <summary>
    /// Static method to generate a parameterized DELETE statement;
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="keyColumn"></param>
    /// <returns>A parameterized SQL DELETE statement for the specified key column and value.</returns>
    public static string GenerateDeleteWithParameters(string keyColumn, bool useBrackets)
    {
        string mWhereClause = $"WHERE [{keyColumn}] = @{keyColumn}";
        if (!useBrackets)
        {
            mWhereClause = handleBrackets(mWhereClause, useBrackets);
        }
        string mRetVal = $"DELETE FROM {m_TableName} {mWhereClause};";
        return mRetVal;
    }

    /// <summary>
    /// Generates a DELETE statement specifying the keyColumn, the keyColumn's value is derived from the propeerty
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="keyColumn"></param>
    /// <returns>A SQL DELETE statement for the specified key column.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public string GenerateDeleteWithValues<T>(string keyColumn, bool useBrackets) where T : ADatabaseTable
    {
        if (string.IsNullOrEmpty(keyColumn))
        {
            throw new ArgumentException("keyColumn cannot be null or empty", nameof(keyColumn));
        }
        string mKeyValue = GetPropertyValue<T>(keyColumn);
        if (string.IsNullOrWhiteSpace(mKeyValue))
        {
            throw new InvalidOperationException($"The keyColumn {keyColumn} has no value and cannot be deleted.");
        }
        return GenerateDeleteWithValues(keyColumn, mKeyValue, useBrackets);
    }

    /// <summary>
    /// Generates a DELETE statement for the specified key column.
    /// </summary>
    /// <param name="keyColumn">The name of the key column.</param>
    /// <param name="keyValue">The value of the key column. If it is a string, it should be single quoted.</param>
    /// <param name="useBrackets">Whether to use brackets in the WHERE clause.</param>
    /// <returns>A SQL DELETE statement for the specified key column and value.</returns>
    public static string GenerateDeleteWithValues(string keyColumn, string keyValue, bool useBrackets)
    {
        string mRetVal = string.Format(m_DeleteStatementTemplate, m_TableName, keyColumn, keyValue);
        if (!useBrackets)
        {
            mRetVal = handleBrackets(mRetVal, useBrackets);
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
    public static DataTable GenerateEmptyTable<T>(string tableName, bool includePrimaryKey) where T : ADatabaseTable
    {
        DataTable mTempRetTable = null;
        DataTable mRetTable = null;
        mTempRetTable = new DataTable(tableName);
        string mPrimaryKeyName = GetPrimaryKeyName<T>();
        if (includePrimaryKey && string.IsNullOrWhiteSpace(mPrimaryKeyName))
        {
            throw new NullReferenceException("The includePrimaryKey prameter is true and PrimaryKey has not been set for any property.");
        }
        try
        {
            mTempRetTable.Locale = CultureInfo.InvariantCulture;
            foreach (PropertyInfo mPropertyItem in getPropertiesFromType<T>())
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
    /// Static method to generate an INSERT statement with parameters
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="includePrimaryKey">Specifies whether to include the primary key.</param>
    /// <returns></returns>
    public static string GenerateInsertWithParameters<T>(bool useBrackets, bool includePrimaryKey = false) where T : ADatabaseTable
    {
        PropertyInfo[] mPropertiesArray = getPropertiesFromType<T>().Where((propertyInfo) =>
            (includePrimaryKey || !propertyInfo.IsDefined(typeof(DBPrimaryKey), false))
        ).ToArray();
        string mColumnNames = getColumnNames<T>(mPropertiesArray, useBrackets);
        foreach (PropertyInfo mPropertyItem in mPropertiesArray)
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
    public string GenerateInsertWithValues<T>(bool useBrackets, bool includePrimaryKey = false) where T : ADatabaseTable
    {
        PropertyInfo[] mPropertiesArray = getPropertiesFromField<T>(includePrimaryKey);
        string mColumnNames = getColumnNames<T>(mPropertiesArray, useBrackets);
        foreach (PropertyInfo mPropertyItem in mPropertiesArray)
        {
            var value = mPropertyItem.GetValue(this, null);
            m_StringBuilder.Append(formatValue(value) + " ,");
        }
        string mValues = m_StringBuilder.ToString().Substring(0, m_StringBuilder.ToString().Length - 2);
        m_StringBuilder.Clear();
        return $"INSERT INTO {m_TableName} ({mColumnNames}) VALUES ({mValues});";
    }

    /// <summary>
    /// Static method to generate an UPDATE statement using parameters
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="keyColumn"></param>
    /// <returns></returns>
    public static string GenerateUpdateWithParameters<T>(string keyColumn, bool useBrackets) where T : ADatabaseTable
    {
        PropertyInfo[] mPropertiesArray = getPropertiesFromType<T>().Where(propertyInfo =>
            !propertyInfo.IsDefined(typeof(DBPrimaryKey), false)
        ).ToArray();
        var mSetClauses = string.Join(", ", mPropertiesArray.Select(p => $"[{getColumnName(p)}] = @{getColumnName(p)}"));
        if (!useBrackets)
        {
            mSetClauses = handleBrackets(mSetClauses, useBrackets);
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
    public string GenerateUpdateWithValues<T>(string keyColumn, bool useBrackets) where T : ADatabaseTable
    {
        string mKeyValue = GetPropertyValue<T>(keyColumn);
        if (string.IsNullOrWhiteSpace(mKeyValue))
        {
            throw new InvalidOperationException($"The keyColumn {keyColumn} has no value and cannot be updated.");
        }
        return GenerateUpdateWithValues<T>(keyColumn, mKeyValue, useBrackets);
    }

    /// <summary>
    /// Generates an UPDATE SQL statement for the current instance with actual values from the properties specifing the keyColumn and it's value
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="keyColumn"></param>
    /// <param name="keyValue"></param>
    /// <returns>string</returns>
    /// <remarks>keyValue should be single quoted if it is a string</remarks>
    public string GenerateUpdateWithValues<T>(string keyColumn, string keyValue, bool useBrackets) where T : ADatabaseTable
    {
        m_StringBuilder.Append($"UPDATE {m_TableName} SET ");
        foreach (PropertyInfo mPropertyItem in getPropertiesFromField<T>(false))
        {
            if (useBrackets)
            {
                m_StringBuilder.Append($"[{getColumnName(mPropertyItem)}] = {formatValue(mPropertyItem.GetValue(this))}, ");
            }
            else
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
    /// Returns a boolean given the DataRow and Column name for either bit or int values.
    /// </summary>
    /// <param name="dataRow">The dataRow. If null return value is false</param>
    /// <param name="columnName">Name of the column.</param>
    /// <returns>Boolean.</returns>
    /// <remarks>
    ///     Integer or int values not equal to 0 are considered true
    /// </remarks>
#pragma warning disable CA1822 // Mark members as static
    protected Boolean GetBool(DataRow dataRow, String columnName)
    {
        /*
         * Not marked as static to give clarity of where the code exists when 
         * reading it in the deriving class (eg. base.GetBool(dataRow, columnName))
         * it is clear that the code resides in the abstract class.
         */
        if (string.IsNullOrEmpty(columnName))
        {
            throw new ArgumentNullException(nameof(columnName), "columnName cannot be a null reference (Nothing in Visual Basic) or empty!");
        }
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
        if (property == null)
        {
            throw new ArgumentNullException(nameof(property), "property cannot be a null reference (Nothing in Visual Basic)");
        }
        var attribute = property.GetCustomAttribute<DBColumnName>();
        return attribute?.Name ?? property.Name; // Use the attribute name if available, otherwise use the property name
    }

    /// <summary>
    /// Static method to get all the column names either from the attribute or property name if the attribute is not present
    /// </summary>
    /// <param name="useBrackets">If true, the column names will be enclosed in brackets.</param>
    /// <typeparam name="T"></typeparam>
    /// <returns>An array of column names.</returns>
    public string GetColumnNames<T>(bool useBrackets) where T : ADatabaseTable
    {
        PropertyInfo[] mPropertiesArray = getPropertiesFromField<T>(true);
        return getColumnNames<T>(mPropertiesArray, useBrackets);
    }

    /// <summary>
    /// Helper method to get all the column names either from the attribute or property name if the attribute is not present
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="propertiesArray"></param>
    /// <returns></returns>
    private static string getColumnNames<T>(PropertyInfo[] propertiesArray, bool useBrackets) where T : ADatabaseTable
    {
        foreach (PropertyInfo mPropertyItem in propertiesArray)
        {
            if (useBrackets)
            {
                m_StringBuilder.Append("[" + getColumnName(mPropertyItem) + "], ");
            }
            else
            {
                m_StringBuilder.Append(getColumnName(mPropertyItem) + ", ");
            }
        }
        string mRetVal = m_StringBuilder.ToString().Substring(0, m_StringBuilder.ToString().Length - 2);
        m_StringBuilder.Clear();
        return mRetVal;
    }

    /// <summary>
    /// Helper method to get a property from either the attribute or the property name
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    private PropertyInfo getProperty<T>(string propertyName, bool includePrimaryKey) where T : ADatabaseTable
    {
        PropertyInfo mRetVal = null;
        PropertyInfo[] mPropertiesArray = getPropertiesFromField<T>(includePrimaryKey);
        mRetVal = mPropertiesArray.FirstOrDefault(propertyInfo =>
            propertyInfo.Name == propertyName ||
            propertyInfo.GetCustomAttribute<DBColumnName>()?.Name == propertyName
        );
        return mRetVal;
    }

    /// <summary>
    /// Helper method to get the property's value given it's name
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    /// <exception cref="InvalidOperationException"></exception>
    public string GetPropertyValue<T>(string propertyName) where T : ADatabaseTable
    {
        PropertyInfo mPropertyItem = getProperty<T>(propertyName, true);
        if (mPropertyItem == null)
        {
            throw new InvalidOperationException($"The Property {propertyName} was not found.");
        }
        string mRetVal = formatValue(mPropertyItem.GetValue(this));
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
#pragma warning disable CA1822 // Mark members as static
    protected DateTime GetDateTime(DataRow dataRow, String columnName, DateTime defaultDateTime)
    {
        /*
         * Not marked as static to give clarity of where the code exists when 
         * reading it in the deriving class (eg. base.GetDateTime(dataRow, columnName, defaultDateTime))
         * it is clear that the code resides in the abstract class.
         */
        DateTime mRetVal = defaultDateTime;
        if (dataRow != null && dataRow.Table.Columns.Contains(columnName) && !(Convert.IsDBNull(dataRow[columnName])))
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
#pragma warning disable CA1822 // Mark members as static
    protected Int32 GetInt(DataRow dataRow, String columnName)
    {
        /*
         * Not marked as static to give clarity of where the code exists when 
         * reading it in the deriving class (eg. base.GetInt(dataRow, columnName))
         * it is clear that the code resides in the abstract class.
         */
        int mRetVal = -1;
        if (dataRow != null && dataRow.Table.Columns.Contains(columnName) && !(Convert.IsDBNull(dataRow[columnName])))
        {
            mRetVal = int.Parse(dataRow[columnName].ToString(), CultureInfo.InvariantCulture);
        }
        return mRetVal;
    }

    /// <summary>
    /// Return the value of the primary key
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static string GetPrimaryKeyName<T>() where T : ADatabaseTable
    {
        PropertyInfo mPrimaryKeyProperty = getPropertiesFromType<T>().FirstOrDefault(propertyInfo =>
            propertyInfo.IsDefined(typeof(DBPrimaryKey), false)
        );
        string mRetVal = string.Empty;
        if (mPrimaryKeyProperty != null)
        {
            mRetVal = handleBrackets(getColumnName(mPrimaryKeyProperty), true);
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns an array of PropertyInfo for the specified type
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <returns></returns>
    private static PropertyInfo[] getPropertiesFromType<T>() where T : ADatabaseTable
    {
        Type mType = typeof(T);
        return mType.GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where((propertyInfo) =>
            propertyInfo.CanRead &&
            !propertyInfo.IsDefined(typeof(DBIgnoreProperty), false)
        ).ToArray();
    }

    /// <summary>
    /// Returns an array of PropertyInfo for the specified type from a static memeber
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="includePrimaryKey"></param>
    /// <returns></returns>
    /// 
    private PropertyInfo[] getPropertiesFromField<T>(bool includePrimaryKey) where T : ADatabaseTable
    {
        if (m_PropertyInfoArray == null)
        {
            m_PropertyInfoArray = getPropertiesFromType<T>().ToArray();
        }
        PropertyInfo[] mPropertyInfoArray = m_PropertyInfoArray.Where(p =>
                p.CanRead &&
                !p.IsDefined(typeof(DBIgnoreProperty), false) &&
                (includePrimaryKey || !p.IsDefined(typeof(DBPrimaryKey), false))
        ).ToArray();
        return mPropertyInfoArray;
    }

    /// <summary>
    /// Returns a string given the DataRow and Column name
    /// </summary>
    /// <param name="dataRow"></param>
    /// <param name="columnName"></param>
    /// <returns>string value or empty</returns>
    protected String GetString(DataRow dataRow, String columnName)
    {
        /*
         * Not marked as static to give clarity of where the code exists when 
         * reading it in the deriving class (eg. base.GetString(dataRow, columnName))
         * it is clear that the code resides in the abstract class.
         */
        String mRetVal = string.Empty;
        if (dataRow != null && dataRow.Table.Columns.Contains(columnName) && !(Convert.IsDBNull(dataRow[columnName])))
        {
            mRetVal = dataRow[columnName].ToString().Trim();
        }
        return mRetVal;
    }

    /// <summary>
    /// Removes brackets from the DELETE statement if the useBrackets flag is false.
    /// </summary>
    /// <param name="deleteStatement">The SQL DELETE statement.</param>
    /// <param name="useBrackets">Indicates whether to retain brackets in the statement.</param>
    /// <returns>The modified DELETE statement with brackets removed if useBrackets is false.</returns>
    private static string handleBrackets(string deleteStatement, bool useBrackets)
    {
        // strings are inmutable so no need to create a copy.
        if (!useBrackets)
        {
            deleteStatement = deleteStatement.Replace("[", "").Replace("]", "");
        }
        return deleteStatement;
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
    /// </remarks>
    protected abstract void SetupClass();
}

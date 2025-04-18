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
public class DBIgnorePropertyAttribute : Attribute { } // [DBIgnoreProperty]

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public class DBPrimaryKeyAttribute : Attribute { } // [DBPrimaryKey]

[AttributeUsage(AttributeTargets.Property)]
public class DBColumnNameAttribute : Attribute // [DBColumnName("The_Column_Name")]
{
    public string Name { get; }

    public DBColumnNameAttribute(string name)
    {
        Name = name;
    }
}

/// <summary>
/// A Base class to contain common properties and methods for a database table
/// </summary>
public abstract class ADatabaseTable : IDatabaseTable
{

    #region Member Fields

    private DateTime? m_DefaultSystemDateTime = null;

    private bool m_DisposedValue;

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

    private const string DELETE_STATEMENT_TEMPLATE = "DELETE FROM {0} WHERE [{1}] = @{2};";

    private const string INSERT_STATEMENT_TEMPLATE = "INSERT INTO {0} ({1}) VALUES ({2});";

    #endregion

    #region Public Properties

    /// <summary>
    /// The default system date for Growthware
    /// </summary>
    [DBIgnoreProperty]
    public DateTime DefaultSystemDateTime
    {
        get
        {
            if (m_DefaultSystemDateTime == null)
            {
                m_DefaultSystemDateTime = new(1753, 1, 1, 0, 0, 0); // 1/1/1753 12:00:00 AM
            }
            return m_DefaultSystemDateTime.Value;
        }
    }

    /// <summary>
    /// The foreign key name used during bulk insert
    /// </summary>
    [DBIgnoreProperty]
    public abstract string ForeignKeyName { get; }

    /// <summary>
    /// Indicates the foreign key is numeric and is used during bulk insert
    /// </summary>
    [DBIgnoreProperty]
    public abstract bool IsForeignKeyNumeric { get; }

    [DBIgnoreProperty]
    public string PrimaryKeyName
    {
        get
        {
            PropertyInfo[] mPropertiesArray = this.GetType().GetProperties();
            return getPrimaryKeyName(mPropertiesArray);
        }
    }

    /// <summary>
    /// The table name in the database
    /// </summary>
    [DBIgnoreProperty]
    public abstract string TableName { get; }

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
    /// Generates a parameterized SQL DELETE statement given the name of the primary key.
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="primaryKeyName">The name of the key column used in the WHERE clause of the DELETE statement.</param>
    /// <param name="useBrackets">Indicates whether to include brackets around table and column names in the SQL statement.</param>
    /// <returns>A SQL DELETE statement for the specified key column.</returns>
    /// <exception cref="Exception">Thrown when the delete statement generation fails for the specified table type.</exception>
    public static string GenerateDeleteUsingParameter<T>(string primaryKeyName, bool useBrackets) where T : ADatabaseTable, new()
    {
        try
        {
            string mTableName = (string)(new T()).TableName;
            // DELETE FROM {0} WHERE [{1}] = @{2};
            string mRetVal = string.Format(DELETE_STATEMENT_TEMPLATE, mTableName, primaryKeyName, primaryKeyName);
            mRetVal = handleBrackets(mRetVal, useBrackets);
            return mRetVal;
        }
        catch (Exception ex)
        {
            throw new Exception($"Failed to generate delete statement for table {typeof(T).Name}", ex);
        }
    }

    /// <summary>
    /// Generates a DELETE statement specifying the keyColumn, the keyColumn's value is derived from the propeerty
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="keyColumn"></param>
    /// <returns>A SQL DELETE statement for the specified key column.</returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="InvalidOperationException"></exception>
    public string GenerateDeleteUsingValues<T>(string keyColumn, bool useBrackets) where T : ADatabaseTable, new()
    {
        if (string.IsNullOrEmpty(keyColumn))
        {
            throw new ArgumentException("keyColumn cannot be null or empty", nameof(keyColumn));
        }
        string mKeyValue = getPropertyValue<T>(keyColumn);
        if (string.IsNullOrWhiteSpace(mKeyValue))
        {
            throw new InvalidOperationException($"The keyColumn {keyColumn} has no value and cannot be deleted.");
        }
        return GenerateDeleteUsingValues<T>(keyColumn, mKeyValue, useBrackets);
    }

    /// <summary>
    /// Generates a DELETE statement for the specified key column.
    /// </summary>
    /// <param name="keyColumn">The name of the key column.</param>
    /// <param name="keyValue">The value of the key column. If it is a string, it should be single quoted.</param>
    /// <param name="useBrackets">Whether to use brackets in the WHERE clause.</param>
    /// <returns>A SQL DELETE statement for the specified key column and value.</returns>
    public string GenerateDeleteUsingValues<T>(string keyColumn, string keyValue, bool useBrackets) where T : ADatabaseTable, new()
    {
        // DELETE FROM {0} WHERE [{1}] = @{2};
        string mTableName = (string)(new T()).TableName;
        string mRetVal = string.Format(DELETE_STATEMENT_TEMPLATE, mTableName, keyColumn, keyValue);
        mRetVal = mRetVal.Replace("@", "");
        if (!useBrackets)
        {
            mRetVal = handleBrackets(mRetVal, useBrackets);
        }
        return mRetVal;
    }

    /// <summary>
    /// Generates an SQL INSERT statement for the specified table and set of properties.
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="useBrackets">Indicates whether to include brackets around table and column names in the SQL statement.</param>
    /// <param name="includePrimaryKey">Indicates whether to include the primary key in the generated SQL statement.</param>
    /// <returns>A SQL INSERT statement for the specified table and set of properties.</returns>
    public static string GenerateInsertUsingParameters<T>(bool useBrackets, bool includePrimaryKey = false) where T : ADatabaseTable, new()
    {
        PropertyInfo[] mPropertiesArray = getProperties<T>(includePrimaryKey);
        string mTableName = (string)(new T()).TableName;
        bool mFirstLoop = true;
        m_StringBuilder.Clear();
        m_StringBuilder.Append("INSERT INTO ").Append(mTableName.Trim()).AppendLine(" (");
        foreach (PropertyInfo mPropertyItem in mPropertiesArray)
        {
            if (!mFirstLoop)
            {
                m_StringBuilder.Append("	,[").Append(getColumnName(mPropertyItem)).AppendLine("]");
            }
            else
            {
                mFirstLoop = false;
                m_StringBuilder.Append("	 [").Append(getColumnName(mPropertyItem)).AppendLine("]");
            }
        }
        m_StringBuilder.AppendLine(") VALUES (");
        mFirstLoop = true;
        foreach (PropertyInfo mPropertyItem in mPropertiesArray)
        {
            if (!mFirstLoop)
            {
                m_StringBuilder.Append("	,@").AppendLine(getColumnName(mPropertyItem));
            }
            else
            {
                mFirstLoop = false;
                m_StringBuilder.Append("	 @").AppendLine(getColumnName(mPropertyItem));
            }
        }
        m_StringBuilder.AppendLine(");");
        string mRetVal = handleBrackets(m_StringBuilder.ToString(), useBrackets);
        m_StringBuilder.Clear();
        return mRetVal;
    }

    /// <summary>
    /// Generates an SQL INSERT statement for the specified table and set of properties with values.
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="useBrackets">Indicates whether to include brackets around table and column names in the SQL statement.</param>
    /// <param name="includePrimaryKey">Indicates whether to include the primary key in the generated SQL statement.</param>
    /// <returns>A SQL INSERT statement for the specified table and set of properties with values.</returns>
    public string GenerateInsertUsingValues<T>(bool useBrackets, bool includePrimaryKey = false) where T : ADatabaseTable, new()
    {
        PropertyInfo[] mPropertiesArray = getProperties<T>(includePrimaryKey);
        string mTableName = (string)(new T()).TableName;
        bool mFirstLoop = true;
        m_StringBuilder.Clear();
        m_StringBuilder.Append("INSERT INTO ").Append(mTableName.Trim()).AppendLine(" (");
        foreach (PropertyInfo mPropertyItem in mPropertiesArray)
        {
            if (!mFirstLoop)
            {
                m_StringBuilder.Append("	, [").Append(getColumnName(mPropertyItem)).AppendLine("]");
            }
            else
            {
                mFirstLoop = false;
                m_StringBuilder.Append("	  [").Append(getColumnName(mPropertyItem)).AppendLine("]");
            }
        }
        m_StringBuilder.AppendLine(") VALUES (");
        mFirstLoop = true;
        foreach (PropertyInfo mPropertyItem in mPropertiesArray)
        {
            if (!mFirstLoop)
            {
                m_StringBuilder.Append("	,  ").AppendLine(getPropertyValue(mPropertyItem));
            }
            else
            {
                mFirstLoop = false;
                m_StringBuilder.Append("	   ").AppendLine(getPropertyValue(mPropertyItem));
            }
        }
        m_StringBuilder.AppendLine(");");
        string mRetVal = handleBrackets(m_StringBuilder.ToString(), useBrackets);
        m_StringBuilder.Clear();
        mRetVal = handleBrackets(mRetVal, useBrackets);
        return mRetVal;
    }

    /// <summary>
    /// Generates an SQL UPDATE statement for the specified table and set of properties using parameterised values.
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="useBrackets">Indicates whether to include brackets around table and column names in the SQL statement.</param>
    /// <param name="includePrimaryKey">Indicates whether to include the primary key in the generated SQL statement.</param>
    /// <returns>A SQL UPDATE statement for the specified table and set of properties using parameterised values.</returns>
    public static string GenerateUpdateUsingParameters<T>(bool useBrackets) where T : ADatabaseTable, new()
    {
        PropertyInfo[] mPropertiesArray = getProperties<T>(false);
        string mTableName = (string)(new T()).TableName;
        m_StringBuilder.Clear();
        m_StringBuilder.Append("UPDATE ").AppendLine(mTableName);
        bool mFirstLoop = true;
        string mPrimaryKeyName = getPrimaryKeyName(getProperties<T>(true));
        foreach (PropertyInfo mPropertyItem in mPropertiesArray)
        {
            if (!mFirstLoop)
            {
                m_StringBuilder.Append("      ,[").Append(getColumnName(mPropertyItem)).Append("] = ").Append('@').AppendLine(getColumnName(mPropertyItem));
            }
            else
            {
                mFirstLoop = false;
                m_StringBuilder.Append("   SET [").Append(getColumnName(mPropertyItem)).Append("] = ").Append('@').AppendLine(getColumnName(mPropertyItem));
            }
        }
        m_StringBuilder.Append("WHERE ").Append(mPrimaryKeyName).Append(" = @").AppendLine(mPrimaryKeyName);
        string mRetVal = m_StringBuilder.ToString();
        m_StringBuilder.Clear();
        mRetVal = handleBrackets(mRetVal, useBrackets);
        return mRetVal;
    }

    /// <summary>
    /// Generates an SQL UPDATE statement for the specified table and set of properties using actual values.
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="useBrackets">Indicates whether to include brackets around table and column names in the SQL statement.</param>
    /// <param name="includePrimaryKey">Indicates whether to include the primary key in the generated SQL statement.</param>
    /// <returns>A SQL UPDATE statement for the specified table and set of properties using actual values.</returns>
    public string GenerateUpdateUsingValues<T>(bool useBrackets) where T : ADatabaseTable, new()
    {
        PropertyInfo[] mPropertiesArray = getProperties<T>(false);
        string mTableName = (string)(new T()).TableName;
        m_StringBuilder.Clear();
        m_StringBuilder.Append("UPDATE ").AppendLine(mTableName);
        bool mFirstLoop = true;
        PropertyInfo[] mAllProperties = getProperties<T>(true);
        PropertyInfo mPrimaryKeyProperty = mAllProperties.Where(propertyInfo => propertyInfo.IsDefined(typeof(DBPrimaryKeyAttribute), false)).First();
        string mPrimaryKeyName = getPrimaryKeyName(mAllProperties);
        string mPrimaryKeyValue = getPropertyValue(mPrimaryKeyProperty);
        foreach (PropertyInfo mPropertyItem in mPropertiesArray)
        {
            if (!mFirstLoop)
            {
                m_StringBuilder.Append("      ,[").Append(getColumnName(mPropertyItem)).Append("]").Append(" = ").AppendLine(getPropertyValue(mPropertyItem));
            }
            else
            {
                mFirstLoop = false;
                m_StringBuilder.Append("   SET [").Append(getColumnName(mPropertyItem)).Append("]").Append(" = ").AppendLine(getPropertyValue(mPropertyItem));
            }
        }
        m_StringBuilder.Append("WHERE [").Append(mPrimaryKeyName).Append("] = ").AppendLine(mPrimaryKeyValue);
        string mRetVal = m_StringBuilder.ToString();
        m_StringBuilder.Clear();
        mRetVal = handleBrackets(mRetVal, useBrackets);
        return mRetVal;
    }

    /// <summary>
    /// Returns a boolean given the DataRow and Column name for either bit or int values.
    /// </summary>
    /// <param name="dataRow">The dataRow.</param>
    /// <param name="columnName">Name of the column.</param>
    /// <returns>Boolean.</returns>
    /// <remarks>Integer or int values not equal to 0 are considered true</remarks>
    protected bool GetBool(DataRow dataRow, string columnName)
    {
        return DataRowHelper.GetBool(dataRow, columnName);
    }

    /// <summary>
    /// Gets the "column name" associated with the given property.
    /// The column name is the value of the <see cref="DBColumnName"/> attribute if it exists, otherwise it is the name of the property.
    /// </summary>
    /// <param name="propertyInfo">The PropertyInfo object to get the column name from.</param>
    /// <returns>The column name associated with the given property.</returns>
    /// <exception cref="ArgumentNullException">propertyInfo is null.</exception>
    private static string getColumnName(PropertyInfo propertyInfo)
    {
        if (propertyInfo == null)
        {
            throw new ArgumentNullException(nameof(propertyInfo), "propertyInfo cannot be a null reference (Nothing in Visual Basic)");
        }
        var attribute = propertyInfo.GetCustomAttribute<DBColumnNameAttribute>();
        return attribute?.Name ?? propertyInfo.Name; // Use the attribute name if available, otherwise use the property name
    }

    /// <summary>
    /// Returns the column names from the given array of PropertyInfo objects in a form suitable for an SQL INSERT statement.
    /// </summary>
    /// <param name="propertyInfoArray">The array of PropertyInfo objects to get the column names from.</param>
    /// <param name="useBrackets">Indicates whether to include brackets around column names in the SQL statement.</param>
    /// <returns>A string containing the column names, separated by commas, and optionally enclosed in brackets.</returns>
    /// <exception cref="ArgumentNullException">propertyInfoArray is null.</exception>
    private static string getColumnNames(PropertyInfo[] propertyInfoArray, bool useBrackets)
    {
        if (propertyInfoArray == null)
        {
            throw new ArgumentNullException(nameof(propertyInfoArray), "propertyInfoArray cannot be a null reference (Nothing in Visual Basic)");
        }
        m_StringBuilder.Clear();
        string mColumnNames = string.Join("], [", propertyInfoArray.Select(p => getColumnName(p)));
        m_StringBuilder.Append("[").Append(mColumnNames).Append("]");
        mColumnNames = m_StringBuilder.ToString();
        m_StringBuilder.Clear();
        mColumnNames = handleBrackets(mColumnNames, useBrackets);
        return mColumnNames;
    }

    public static string GetColumnNames<T>(bool useBrackets, bool includePrimaryKey = false) where T : ADatabaseTable, new()
    {
        PropertyInfo[] mPropertiesArray = getProperties<T>(includePrimaryKey);
        return getColumnNames(mPropertiesArray, useBrackets);
    }

    /// <summary>
    /// Returns a DateTime given the a DataRow and Column name and the default value.
    /// </summary>
    /// <param name="dataRow">DataRow</param>
    /// <param name="columnName">String</param>
    /// <param name="defaultDateTime">DateTime</param>
    /// <returns>DateTime</returns>
    /// <remarks></remarks>
    protected DateTime GetDateTime(DataRow dataRow, String columnName, DateTime defaultDateTime)
    {
        return DataRowHelper.GetDateTime(dataRow, columnName, defaultDateTime);
    }

    /// <summary>
    /// Returns a DataTable with the same schema as the table represented by the given type,
    /// but with no rows. The table name is set to the tableName parameter.
    /// </summary>
    /// <typeparam name="T">The object type in wich the column names and data types will be derived.</typeparam>
    /// <param name="tableName">Used to populate the emptyTable.TableName property of the returned DataTable.</param>
    /// <param name="includePrimaryKey">Indicates whether to include the primary key in the generated table.</param>
    /// <returns>A DataTable with the same schema as the table represented by the given type, but with no rows.</returns>
    /// <exception cref="ArgumentNullException">tableName is null.</exception>
    public DataTable GetEmptyTable(string tableName, bool includePrimaryKey, string[] columnNamesInOrder)
    {
        DataTable mTempRetTable = null;
        DataTable mRetTable = null;
        mTempRetTable = new DataTable(tableName);
        string mPrimaryKeyName = handleBrackets(this.PrimaryKeyName, false);
        try
        {
            mTempRetTable.Locale = CultureInfo.InvariantCulture;
            // PropertyInfo[] mPropertyInfo = this.GetType().GetProperties();
            PropertyInfo[] mPropertiesArray = this.GetType().GetProperties().Where((propertyInfo) =>
                propertyInfo.CanRead
                && propertyInfo.IsDefined(typeof(DBIgnorePropertyAttribute), false) == false
                || propertyInfo.IsDefined(typeof(DBPrimaryKeyAttribute), false) == includePrimaryKey
            ).ToArray();
            foreach (string mColumnName in columnNamesInOrder)
            {
                // Find the PropertyInfo that matches the column name
                var mPropertyItem = mPropertiesArray.FirstOrDefault(p => string.Equals(p.Name, mColumnName, StringComparison.OrdinalIgnoreCase));
                if (mPrimaryKeyName.ToLowerInvariant() != mPropertyItem.Name.ToLowerInvariant())
                {
                    Type mPropertyType = mPropertyItem.PropertyType;
                    if (mPropertyType.IsGenericType && mPropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                    {
                        mPropertyType = mPropertyType.GetGenericArguments()[0];
                    }

                    // Create a DataColumn object
                    DataColumn mColumn = null;
                    switch (Type.GetTypeCode(mPropertyType))
                    {
                        case TypeCode.String:
                            mColumn = new DataColumn(mPropertyItem.Name, typeof(string));
                            break;
                        case TypeCode.DateTime:
                            mColumn = new DataColumn(mPropertyItem.Name, typeof(DateTime));
                            break;
                        case TypeCode.Int32:
                        case TypeCode.Int64:
                        case TypeCode.Double:
                        case TypeCode.Decimal:
                            mColumn = new DataColumn(mPropertyItem.Name, typeof(decimal)); // Use decimal for all numeric types
                            break;
                        case TypeCode.Boolean:
                            mColumn = new DataColumn(mPropertyItem.Name, typeof(int)); // Use NUMBER (0 or 1) for boolean
                            break;
                        default:
                            mColumn = new DataColumn(mPropertyItem.Name, typeof(object));
                            break;
                    }

                    // Add the column to the DataTable
                    if (mColumn != null)
                    {
                        mTempRetTable.Columns.Add(mColumn);
                    }
                }
                else if (includePrimaryKey)
                {
                    mTempRetTable.Columns.Add(new DataColumn(mPropertyItem.Name, mPropertyItem.PropertyType));
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
    /// Gets the name of the property that has the DBPrimaryKey attribute.
    /// </summary>
    /// <param name="propertyInfoArray">The array of PropertyInfo objects to search for the given property name.</param>
    /// <typeparam name="T">The type of the object to get the primary key column name of.</typeparam>
    /// <returns>The name of the property or an empty string.</returns>
    /// <remarks>
    /// Because a data table can only have 1 primary key, this method returns the first property that has the DBPrimaryKey attribute.
    /// Note: this method does not handle compound primary key.
    /// </remarks>
    private static string getPrimaryKeyName(PropertyInfo[] propertyInfoArray)
    {
        // TODO: add support for compound primary key
        PropertyInfo mPrimaryKeyProperty = propertyInfoArray.Where(propertyInfo => propertyInfo.IsDefined(typeof(DBPrimaryKeyAttribute), false)).FirstOrDefault();
        string mRetVal = string.Empty;
        if (mPrimaryKeyProperty != null)
        {
            mRetVal = handleBrackets(getColumnName(mPrimaryKeyProperty), true);
        }
        else
        {
            throw new NullReferenceException("No primary key found.");
        }
        return mRetVal;
    }


    /// <summary>
    /// Returns an int given the DataRow and Column name
    /// </summary>
    /// <param name="dataRow"></param>
    /// <param name="columnName"></param>
    /// <returns>-1 if no value was found</returns>
    protected int GetInt(DataRow dataRow, string columnName)
    {
        return DataRowHelper.GetInt(dataRow, columnName);
    }

    /// <summary>
    /// Gets an array of PropertyInfo objects with the BindingFlasts of Public, Instance, and Static.
    /// </summary>
    /// <typeparam name="T">The type of the object.</typeparam>
    /// <returns>PropertyInfo array.</returns>
    private static PropertyInfo[] getProperties<T>() where T : ADatabaseTable
    {
        PropertyInfo[] mRetVal = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        return mRetVal;
    }

    private static PropertyInfo[] getProperties<T>(bool includePrimaryKey) where T : ADatabaseTable, new()
    {
        PropertyInfo[] mRetVal = getProperties<T>().Where(propertyInfo => 
            propertyInfo.CanRead &&
            !Attribute.IsDefined(propertyInfo, typeof(DBIgnorePropertyAttribute)) &&
            (includePrimaryKey || !Attribute.IsDefined(propertyInfo, typeof(DBPrimaryKeyAttribute)))
        ).ToArray();

        return mRetVal;
    }

    /// <summary>
    /// Helper method to get the property's value given it's name
    /// </summary>
    /// <typeparam name="T">The type of the database table, must inherit from ADatabaseTable.</typeparam>
    /// <param name="propertyName"></param>
    /// <returns>A formatted string using the formatValue method</returns>
    /// <exception cref="InvalidOperationException"></exception>
    private string getPropertyValue<T>(string propertyName) where T : ADatabaseTable
    {
        PropertyInfo mPropertyItem = getProperty<T>(propertyName, getProperties<T>());
        if (mPropertyItem == null)
        {
            throw new InvalidOperationException($"The Property {propertyName} was not found.");
        }
        string mRetVal = formatValue(mPropertyItem.GetValue(this));
        return mRetVal;
    }

    /// <summary>
    /// Helper method to get the property's value given it's PropertyInfo
    /// </summary>
    /// <param name="propertyInfo">The PropertyInfo object of the property to get the value from.</param>
    /// <returns>A formatted string using the formatValue method</returns>
    /// <exception cref="ArgumentNullException">propertyInfo is null.</exception>
    private string getPropertyValue(PropertyInfo propertyInfo)
    {
        if (propertyInfo == null)
        {
            throw new ArgumentNullException(nameof(propertyInfo), "propertyInfo cannot be a null reference (Nothing in Visual Basic)");
        }
        string mRetVal = formatValue(propertyInfo.GetValue(this));
        return mRetVal;
    }

    /// <summary>
    /// Gets a PropertyInfo object for the given property name from the given property infos.
    /// </summary>
    /// <param name="propertyName">The name of the property to get the PropertyInfo for.</param>
    /// <param name="propertyInfoArray">The array of PropertyInfo objects to search for the given property name.</param>
    /// <returns>The PropertyInfo object for the given property name.</returns>
    /// <exception cref="ArgumentNullException">propertyInfoArray is null.</exception>
    /// <exception cref="ArgumentException">The property name was not found in the given property infos.</exception>
    private static PropertyInfo getProperty<T>(string propertyName, PropertyInfo[] propertyInfoArray) where T : ADatabaseTable
    {
        if (string.IsNullOrEmpty(propertyName))
        {
            throw new ArgumentNullException(nameof(propertyName), "propertyName cannot be a null reference (Nothing in Visual Basic)");
        }
        if (propertyInfoArray == null)
        {
            throw new ArgumentNullException(nameof(propertyInfoArray), "propertyInfoArray cannot be a null reference (Nothing in Visual Basic)");
        }
        PropertyInfo mRetVal = propertyInfoArray.Where(item => item.Name == propertyName).First();
        if (mRetVal == null)
        {
            throw new ArgumentException($"The property name {propertyName} was not found.");
        }
        return mRetVal;
    }

    /// <summary>
    /// Returns a string given the DataRow and Column name
    /// </summary>
    /// <param name="dataRow"></param>
    /// <param name="columnName"></param>
    /// <returns>string value or empty</returns>
    protected string GetString(DataRow dataRow, string columnName)
    {
        return DataRowHelper.GetString(dataRow, columnName);
    }

    /// <summary>
    /// Removes brackets from the DELETE statement if the useBrackets flag is false.
    /// </summary>
    /// <param name="sqlStatement">The SQL statement.</param>
    /// <param name="useBrackets">Indicates whether to retain brackets in the statement.</param>
    /// <returns>The modified SQL statement with brackets removed if useBrackets is false.</returns>
    protected static string handleBrackets(string sqlStatement, bool useBrackets)
    {
        // strings are inmutable so no need to create a copy.
        if (!useBrackets)
        {
            sqlStatement = sqlStatement.Replace("[", "").Replace("]", "");
        }
        return sqlStatement;
    }

    /// <summary>
    /// Use this to setup any default values for your class.
    /// </summary>
    protected abstract void setDefaults();
}

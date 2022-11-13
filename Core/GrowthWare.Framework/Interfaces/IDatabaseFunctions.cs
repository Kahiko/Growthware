using System;
using System.Data;

namespace GrowthWare.Framework.Interfaces;

/// <summary>
/// Ensures the basic properties are availble to all Profile model objects.
/// </summary>
/// <remarks>
/// No common properties exist at the time of this writing.
/// Leaving the structure in place for future needs
///  </remarks>    
public interface IDatabaseFunctions
{
        /// <summary>
        /// Returns a string of all the column name IE properties
        /// </summary>
        /// <returns></returns>
		string GetColumnNames();
		
        /// <summary>
        /// Will generate a DataTable from the object properties
        /// </summary>
        /// <param name="tableName">string</param>
        /// <returns>DataTable</returns>
        DataTable GetEmptyTable(string tableName);

        /// <summary>
        /// Will generate an insert statement from the object
        /// </summary>
        /// <param name="usePrimaryKey">
        ///     bool indicating if the primary key should be used
        ///     in the insert statement.  Set to to false if your
        ///     tables primary key is auto increment.  Set to true
        ///     if your tables primary key is a guid.
        /// </param>
        /// <returns>
        /// string: Complete insert statement for the object:
        ///     INSERT INTO TableName 
        ///          ([col1], [col2]) VALUES
        ///         ,(data1, 'data2')
        /// 
        /// Note: Formating done for example only.
        /// </returns>
        /// <remarks>
        /// All properties must be populated first if not this will may have adverse effects.
        /// </remarks>		
        string GetInsert(bool usePrimaryKey);
		
        /// <summary>
        /// A method to return m_PrimaryKeyName
        /// </summary>
        /// <remarks>
        ///     Avoids adding a propery that would be then
        ///     need to be accounted for in any of the reflection 
        ///     loop logic dealing with properties.
        /// </remarks>
        /// <returns>string</returns>
		string GetPrimaryKeyName();
		
        /// <summary>
        /// A method to return m_TableName
        /// </summary>
        /// <remarks>
        ///     Avoids adding a propery that would be then
        ///     need to be accounted for in any of the reflection 
        ///     loop logic dealing with properties.
        /// </remarks>
        /// <returns>string</returns>
        string GetTableName();

}

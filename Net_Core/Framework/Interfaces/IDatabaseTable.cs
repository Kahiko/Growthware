using System;
using System.Data;

namespace GrowthWare.Framework.Interfaces;

/// <summary>
/// Ensures the basic properties are availble to all model objects that represent a database table.
/// </summary>
public interface IDatabaseTable : IDisposable
{

    /// <summary>
    /// The name of the foreign key used when performing bulk insert.
    /// </summary>
    string ForeignKeyName { get; }

    /// <summary>
    /// Whether the foreign key is numeric only used in bulk inserts
    /// </summary>
    bool IsForeignKeyNumeric { get; }

    /// <summary>
    /// The name of the database table
    /// </summary>
    string TableName { get; }

    /// <summary>
    /// Whether to use brackets around the column names
    /// </summary>
    bool UseBrackets { get; }
    
}
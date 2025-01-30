using System;
using System.Data;
using GrowthWare.Framework.Models.Base;

namespace GrowthWare.Framework.Interfaces;

/// <summary>
/// Ensures the basic properties are availble to all model objects that represent a database table.
/// </summary>
public interface IDatabaseTable : IDisposable
{

    /// <summary>
    /// The default system date time of new(1753, 1, 1, 0, 0, 0)
    /// </summary>
    DateTime DefaultSystemDateTime { get; }

    /// <summary>
    /// The name of the foreign key used when performing bulk insert.
    /// </summary>
    string ForeignKeyName { get; }

    /// <summary>
    /// Whether the foreign key is numeric only used in bulk inserts
    /// </summary>
    bool IsForeignKeyNumeric { get; }
}
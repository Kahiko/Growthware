using System;

namespace GrowthWare.DataAccess.Interfaces.Base;
/// <summary>
/// The base interface for Database interaction code
/// </summary>
public interface IDBInteraction
{
    /// <summary>
    /// Gets or sets the connection string.
    /// </summary>
    String ConnectionString { get; set; }

    /// <summary>
    /// Returns the connection string without the database name.
    /// </summary>
    /// <returns></returns>
    string ConnectionStringWithoutDatabaseName { get; }

    /// <summary>
    /// Gets or sets the name of the database.
    /// </summary>
    string DatabaseName { get; set; }

}

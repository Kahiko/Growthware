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
}

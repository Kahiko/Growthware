using System;

namespace GrowthWare.DataAccess.Interfaces.Base;
/// <summary>
/// The base interface for Database interaction code
/// </summary>
public interface IDBInteraction
{
    /**
      * TODO: Currently a SqlConnection is created in earch of the following methods:
      *   ExecuteScalar
      *   ExecuteNonQuery
      *   GetDataSet
      * It might be worth revising it to create one and reuse it and removing the neede for
      * m_ConnectionString.
      */

    /// <summary>
    /// Gets or sets the connection string.
    /// </summary>
    String ConnectionString { get; set; }
}

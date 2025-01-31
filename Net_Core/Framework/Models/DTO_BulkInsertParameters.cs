using System;
using System.Collections.Generic;
using System.Data;

namespace GrowthWare.Framework.Models;

public class DTO_BulkInsert_Parameters
{
    /// <summary>
    /// The name of the destination table
    /// </summary>
    public string DestinationTableName{ get; set; }

    /// <summary>
    /// Indicates if records in the destination table should be deleted
    /// </summary>
    public bool DoDelete{ get; set; }

    /// <summary>
    /// The name of the foreign key
    /// </summary>
    public string ForeignKeyName{ get; set; }

    /// <summary>
    /// Indicates if the primary key should be included
    /// </summary>
    public bool IncludePrimaryKey{ get; set; }

    /// <summary>
    /// The list of profiles
    /// </summary>
    public IEnumerable<Object> ListOfProfiles{ get; set; }

    /// <summary>
    /// The number of profiles in ListOfProfiles
    /// </summary>
    public int NumberOfProfiles{ get; set; }

    /// <summary>
    /// The name of the temporary table
    /// </summary>
    public string TempTableName{ get; set; }

    /// <summary>
    /// The name of the primary key
    /// </summary>
    public string PrimaryKeyName{ get; set; }
}
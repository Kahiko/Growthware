using System;

namespace GrowthWare.Framework.Model.Profiles.Interfaces
{
    /// <summary>
    /// Ensures the basic properties are avalible to all Profile model objects.
    /// </summary>
    /// <remarks>
    /// If it is decided to use entities in the future then
    /// this interface should be used for the save, delete, and getitem methods.
    ///  </remarks>
    public interface IMProfile
    {
        /// <summary>
        /// Account ID used to add
        /// </summary>
        int AddedBy
        {
            get;
            set;
        }

        /// <summary>
        /// Date the row was added.
        /// </summary>
        DateTime AddedDate
        {
            get;
            set;
        }

        /// <summary>
        /// Unique numeric identifier
        /// </summary>
        int Id
        {
            get;
            set;
        }

        /// <summary>
        /// Column Name for the Id property from the DB ... IE Function_Seq_ID for a "function" profile.
        /// </summary>
        string IdColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// String representation normaly unique
        /// </summary>
        string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Column Name for the Name property from the DB ... IE Name for a "function" profile.
        /// </summary>
        string NameColumnName
        {
            get;
            set;
        }

        /// <summary>
        /// Account ID used to update
        /// </summary>
        int UpdatedBy
        {
            get;
            set;
        }

        /// <summary>
        /// The date lasted updated
        /// </summary>
        DateTime UpdatedDate
        {
            get;
            set;
        }
    }
}

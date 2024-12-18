using System;
using System.Data;
using GrowthWare.Framework.Interfaces;

namespace GrowthWare.Framework.Models.Base;

/// <summary>
/// Base class for profile objects adding AddedBy, AddedDate, UpdatedBy, UpdatedDate.
/// </summary>
[Serializable()]
public abstract class AAddedUpdated : ADatabaseTable, IAddedUpdated, IDatabaseTable
{
    #region Public Properties
        /// <summary>
        /// Account ID used to add
        /// </summary>
        [ColumnName("Added_By")]
        public int AddedBy { get; set; }

        /// <summary>
        /// Date the row was added.
        /// </summary>
        [ColumnName("Added_Date")]
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// Account ID used to update
        /// </summary>
        [ColumnName("Updated_By")]
        public int UpdatedBy { get; set; }

        /// <summary>
        /// The date lasted updated
        /// </summary>
        [ColumnName("Updated_Date")]
        public DateTime UpdatedDate { get; set; }
    #endregion

    #region Protected Methods
        /// <summary>
        /// Initializes properties given a DataRow
        /// </summary>
        /// <param name="dataRow">datarow</param>
        /// <remarks>
        /// Sets AddedBy, AddedDate, UpdatedBy, UpdatedDate
        /// </remarks>
        protected virtual void Initialize(DataRow dataRow)
        {
            
            this.AddedBy = base.GetInt(dataRow, "Added_By");
            this.AddedDate = base.GetDateTime(dataRow, "Added_Date", DateTime.Now);
            this.UpdatedBy = base.GetInt(dataRow, "Updated_By");
            this.UpdatedDate = base.GetDateTime(dataRow, "Updated_Date", DateTime.Now);
        }
    #endregion
}
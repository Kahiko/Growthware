using System;
using System.Data;
using GrowthWare.Framework.Interfaces;

namespace GrowthWare.Framework.Models.Base
{
    /// <summary>
    /// Base class for profile objects.
    /// </summary>
    [Serializable()]
    [Obsolete("Please use AAddedUpdated instead.  It will be necessary to implement Id but Name is optional.", false)]
    public abstract class AbstractBaseModel : AbstractDatabaseFunctions, IBaseModel
    {

        int m_Id = -1;

#region Public Properties
        /// <summary>
        /// Account ID used to add
        /// </summary>
        public int AddedBy { get; set; }

        /// <summary>
        /// Date the row was added.
        /// </summary>
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// Unique numeric identifier
        /// </summary>
        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }
        /// <summary>
        /// Column Name for the Id property from the DB ... IE Function_Seq_ID for a "function" profile.
        /// </summary>
        public string IdColumnName { get; set; }
        /// <summary>
        /// String representation normaly unique
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Column Name for the Name property from the DB ... IE Name for a "function" profile.
        /// </summary>
        public string NameColumnName { get; set; }
        /// <summary>
        /// Account ID used to update
        /// </summary>
        public int UpdatedBy { get; set; }
        /// <summary>
        /// The date lasted updated
        /// </summary>
        public DateTime UpdatedDate { get; set; }
#endregion

#region Protected Methods
        /// <summary>
        /// Initializes values given a DataRow
        /// </summary>
        /// <param name="dataRow">datarow</param>
        /// <remarks>
        /// Sets AddedBy, AddedDate, UpdatedBy, UpdatedDate
        /// Only sets Id and Name if properties IdColumnName or NameColumnName is not null
        /// </remarks>
        protected virtual void Initialize(DataRow dataRow)
        {
            this.AddedBy = base.GetInt(dataRow, "Added_By");
            this.AddedDate = base.GetDateTime(dataRow, "Added_Date", DateTime.Now);
            this.UpdatedBy = base.GetInt(dataRow, "Updated_By");
            this.UpdatedDate = base.GetDateTime(dataRow, "Updated_Date", DateTime.Now);
            if (!String.IsNullOrEmpty(this.NameColumnName)) 
            {
                this.Name = base.GetString(dataRow, this.NameColumnName);
            }
            if (!String.IsNullOrEmpty(this.IdColumnName)) 
            {
                this.Id = base.GetInt(dataRow, this.IdColumnName);
            }
        }

#endregion
    }
}

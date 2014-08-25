using GrowthWare.Framework.Model.Profiles.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.Model.Profiles.Base
{
    /// <summary>
    /// Base class for profile objects.
    /// </summary>
    [Serializable()]
    public abstract class MProfile : MDatabaseFunctions, IMProfile
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

        public int Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        public string IdColumnName { get; set; }

        public string Name { get; set; }

        public string NameColumnName { get; set; }

        public int UpdatedBy { get; set; }

        public DateTime UpdatedDate { get; set; }
        #endregion

        #region Protected Methods
        /// <summary>
        /// Initializes values given a DataRow
        /// </summary>
        /// <param name="dataRow">datarow</param>
        /// <remarks>
        /// Only sets Id and Name if IdColumnName or NameColumnName is not null
        /// </remarks>
        internal virtual void Initialize(DataRow dataRow)
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

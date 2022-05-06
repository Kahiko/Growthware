using System;
using System.Data;

namespace GrowthWare.Framework.Models
{
    /// <summary>
    /// Properties that represent the table [ZGWOptional].[States]
    /// </summary>
    [Serializable(), CLSCompliant(true)]
    public class MState : MBaseModel
    {
#region "Public Methods"
        public MState()
        {

        }

        public MState(DataRow detailRow)
        {
            base.NameColumnName = "State";
            base.IdColumnName = "State";
            this.Description = this.GetString(detailRow, "Description");
            this.State = this.GetString(detailRow, "State");
            this.StatusId = this.GetInt(detailRow, "StatusSeqId");
            base.Initialize(detailRow);
        }
#endregion

#region "Public Properties"
        public string Description{get; set;}
        public string State{get; set;}
        public int StatusId{get; set;}
#endregion

    }
}
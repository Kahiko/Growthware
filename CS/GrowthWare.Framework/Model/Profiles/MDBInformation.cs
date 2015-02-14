using GrowthWare.Framework.Model.Profiles.Base;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.Model.Profiles
{
    /// <summary>
    /// Base properties an DB Information Profile
    /// </summary>
    /// <remarks>
    /// Corresponds to table ZGWSystem.Database_Information and 
    /// Store procedures: 
    /// ZGWSystem.Set_DataBase_Information, ZGWSystem.Get_Database_Information
    /// </remarks>
    [Serializable(), CLSCompliant(true)]
    public class MDBInformation : MProfile
    {
#region "Member Properties"
        private int mInformationSeqId = 1;
        private string mVersion = string.Empty;
        private int mEnableInheritance = 1;
#endregion

#region "Public Properties"
        public int Information_SEQ_ID
        {
            get { return mInformationSeqId; }
            set { mInformationSeqId = value; }
        }

        public string Version
        {
            get { return mVersion.Trim(); }
            set { mVersion = value.Trim(); }
        }

        public int EnableInheritance
        {
            get { return mEnableInheritance; }
            set { mEnableInheritance = value; }
        }
#endregion

#region "Private Methods"
        protected new void Initialize(ref DataRow dataRow)
        {
            base.NameColumnName = "VERSION";
            base.IdColumnName = "Information_SEQ_ID";
            base.Initialize(dataRow);
            mInformationSeqId = base.GetInt(dataRow, "Information_SEQ_ID");
            mVersion = base.GetString(dataRow, "VERSION");
            mEnableInheritance = base.GetInt(dataRow, "ENABLE_INHERITANCE");
        }
#endregion

#region "Public Methods"

        /// <summary>
        /// Initializes a new instance of the <see cref="MDBInformation"/> class.
        /// </summary>
        public MDBInformation()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDBInformation"/> class.
        /// </summary>
        /// <param name="dataRow">The dataRow.</param>
        public MDBInformation(DataRow dataRow)
        {
            this.Initialize(ref dataRow);
        }
#endregion
    }
}

using System;
using System.Data;

namespace GrowthWare.Framework.Models
{
    /// <summary>
    /// Base properties an DB Information Profile
    /// </summary>
    /// <remarks>
    /// Corresponds to table ZGWSystem.Database_Information and 
    /// Store procedures: 
    /// ZGWSystem.Set_DataBase_Information, ZGWSystem.Get_Database_Information
    /// </remarks>
    public class MDBInformation : MBaseModel
    {
#region "Member Properties"
        private int mInformationSeqId = 1;
        private string mVersion = string.Empty;
        private int mEnableInheritance = 1;
#endregion

#region "Public Properties"
        /// <summary>
        /// The informatino Sequence Identifier
        /// </summary>
        public int InformationSeqId
        {
            get { return mInformationSeqId; }
            set { mInformationSeqId = value; }
        }
        /// <summary>
        /// The version of the database
        /// </summary>
        public string Version
        {
            get { return mVersion.Trim(); }
            set 
            {
                if (value != null) mVersion = value.Trim(); 
            }
        }
        /// <summary>
        /// Determins if the database should use Inheritance when 
        /// calculating roles and or groups.
        /// </summary>
        public int EnableInheritance
        {
            get { return mEnableInheritance; }
            set { mEnableInheritance = value; }
        }
#endregion

#region "Private Methods"
        /// <summary>
        /// Populates direct properties as well as passing the DataRow to the abstract class
        /// for the population of the base properties.
        /// </summary>
        /// <param name="dataRow">DataRow</param>
        protected new void Initialize(DataRow dataRow)
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
            this.Initialize(dataRow);
        }
#endregion
    }
}

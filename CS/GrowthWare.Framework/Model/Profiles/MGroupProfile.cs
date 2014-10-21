using GrowthWare.Framework.Model.Profiles.Base;
using GrowthWare.Framework.Model.Profiles.Interfaces;
using System;
using System.Data;

namespace GrowthWare.Framework.Model.Profiles
{
    [Serializable(), CLSCompliant(true)]
    public class MGroupProfile : MProfile
    {
        #region "Member Properties"
        private string mDescription = string.Empty;
        #endregion
        private int mSecurityEntityId = 1;

        #region "Protected Methods"
        /// <summary>
        /// Initializes the specified DataRow.
        /// </summary>
        /// <param name="dataRow">The DataRow.</param>
        protected new void Initialize(DataRow dataRow)
        {
            base.NameColumnName = "NAME";
            base.IdColumnName = "GROUP_SEQ_ID";
            base.Initialize(dataRow);
            mDescription = base.GetString(dataRow, "DESCRIPTION");
        }
        #endregion

        #region "Public Methods"
        /// <summary>
        /// Will return a message profile with the default value's
        /// </summary>
        /// <remarks></remarks>
        public MGroupProfile()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MGroupProfile" /> class.
        /// </summary>
        /// <param name="profileDataRow">The profile data row.</param>
        public MGroupProfile(DataRow profileDataRow)
        {
            this.Initialize(profileDataRow);
        }
        #endregion

        #region "Public Properties"
        /// <summary>
        /// Gets or sets the security entity ID.
        /// </summary>
        /// <value>The security entity ID.</value>
        public int SecurityEntityId
        {
            get { return mSecurityEntityId; }
            set { mSecurityEntityId = value; }
        }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>The description.</value>
        public string Description
        {
            get { return mDescription; }
            set { if(value != null) mDescription = value.Trim(); }
        }
        #endregion
    }
}

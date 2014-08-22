using GrowthWare.Framework.Model.Profiles.Base;
using GrowthWare.Framework.Model.Profiles.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace GrowthWare.Framework.Model.Profiles
{
    /// <summary>
    /// Class MFunctionProfile
    /// </summary>
    [Serializable(), CLSCompliant(true)]
    public class MFunctionProfile : MGroupRolePermissionSecurity, IMProfile
    {
        #region Member Objects
        private int m_NavigationTypeSeqId = 2;
        //private int m_ALLOW_HTML_INPUT = 1;
        //private int m_ALLOW_COMMENT_HTML_INPUT = 1;
        private int m_FunctionTypeSeqId = 1;
        private int m_ParentmFunctionSeqId = 1;
        private int m_LinkBehavior = 1;
        #endregion

        #region Public Methods
        /// <summary>
        /// Will return a Function profile with the default vaules
        /// </summary>
        public MFunctionProfile()
        {
        }

        /// <summary>
        /// Will return a fully populated Function profile.
        /// </summary>
        /// <param name="profileDatarow">A data row containing the Function information</param>
        /// <param name="derivedRoles">A data row containing all of the derived roles</param>
        /// <param name="assignedRoles">A data row containing all of the assigned roles</param>
        /// <param name="groups">A data row containing all of the assigned groups</param>
        /// <remarks></remarks>
        public MFunctionProfile(DataRow profileDatarow, DataRow[] derivedRoles, DataRow[] assignedRoles, DataRow[] groups)
        {
            this.Initialize(profileDatarow, derivedRoles, assignedRoles, groups);
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Initializes the specified profile with the given DataRow.
        /// </summary>
        /// <param name="profileDataRow">The profile DataRow.</param>
        /// <param name="derivedRoles">The derived roles.</param>
        /// <param name="assignedRoles">The assigned roles.</param>
        /// <param name="groups">The groups.</param>
        private new void Initialize(DataRow profileDataRow, DataRow[] derivedRoles, DataRow[] assignedRoles, DataRow[] groups)
        {
            base.NameColumnName = "ACTION";
            base.IdColumnName = "FUNCTION_SEQ_ID";
            m_FunctionTypeSeqId = base.GetInt(profileDataRow, "FUNCTION_TYPE_SEQ_ID");
            Name = base.GetString(profileDataRow, "NAME");
            Description = base.GetString(profileDataRow, "DESCRIPTION");
            Notes = base.GetString(profileDataRow, "NOTES");
            Source = base.GetString(profileDataRow, "SOURCE");
            EnableViewState = base.GetBool(profileDataRow, "ENABLE_VIEW_STATE");
            EnableNotifications = base.GetBool(profileDataRow, "ENABLE_NOTIFICATIONS");
            RedirectOnTimeout = base.GetBool(profileDataRow, "REDIRECT_ON_TIMEOUT");
            IsNavigable = base.GetBool(profileDataRow, "IS_NAV");
            LinkBehavior = base.GetInt(profileDataRow, "Link_Behavior");
            NoUI = base.GetBool(profileDataRow, "No_UI");
            m_NavigationTypeSeqId = base.GetInt(profileDataRow, "NAVIGATION_NVP_SEQ_DET_ID");
            m_ParentmFunctionSeqId = base.GetInt(profileDataRow, "PARENT_Function_Seq_ID");
            Action = base.GetString(profileDataRow, "ACTION");
            // need to set the the base class name with the action.
            // the names can repeate but the action is unique and lower case.
            base.Name = Action.ToString();
            MetaKeywords = base.GetString(profileDataRow, "META_KEY_WORDS");
        }
        #endregion

        #region Public Properties
        /// <summary>
        /// Represents the Action to be take within the system.
        /// </summary>
        /// <remarks>This is a unique value</remarks>
        public string Action { get; set; }

        /// <summary>
        /// Used as description of the profile
        /// </summary>
        /// <remarks>Designed to be used in any search options</remarks>
        public string Description { get; set; }

        /// <summary>
        /// Indicates to the system if the "page's" view state should be enabled.
        /// </summary>
        /// <remarks>Legacy usage</remarks>
        public bool EnableViewState { get; set; }

        /// <summary>
        /// Intended to be used to send notifications when this profile is "used" by the client
        /// </summary>
        public bool EnableNotifications { get; set; }

        /// <summary>
        /// Use to determin if a function is a navigation function
        /// </summary>
        /// <remarks>
        /// Should be replaced by LinkBehavior
        /// </remarks>
        public bool IsNavigable { get; set; }

        /// <summary>
        /// Represents the link behavior of a function.
        /// </summary>
        /// <returns>Integer</returns>
        /// <remarks>
        /// Data stored in ZGWSecurity.Functions and related to ZGWCoreWeb.Link_Behaviors
        /// </remarks>
        public int LinkBehavior
        {
            get { return m_LinkBehavior; }
            set { m_LinkBehavior = value; }
        }

        /// <summary>
        /// Represents the type of function Module,Security, Menu Item etc
        /// </summary>
        /// <value>Integer/int</value>
        /// <returns>Integer/int</returns>
        /// <remarks>
        /// Data stored in ZGWSecurity.Functions related to ZGWSecurity.Function_Types
        /// </remarks>
        public int FunctionTypeSeqId
        {
            get { return m_FunctionTypeSeqId; }
            set { m_FunctionTypeSeqId = value; }
        }

        /// <summary>
        /// Gets or sets the meta key words.
        /// </summary>
        /// <value>The meta key words.</value>
        public string MetaKeywords { get; set; }

        /// <summary>
        /// String representation normaly unique
        /// </summary>
        /// <value>The name.</value>
        public new string Name { get; set; }

        /// <summary>
        /// Gets or sets the navigation type seq id.
        /// </summary>
        /// <value>The navigation type seq id.</value>
        public int NavigationTypeSeqId
        {
            get { return m_NavigationTypeSeqId; }
            set { m_NavigationTypeSeqId = value; }
        }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>The notes.</value>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [no UI].
        /// </summary>
        /// <value><c>true</c> if [no UI]; otherwise, <c>false</c>.</value>
        public bool NoUI { get; set; }

        /// <summary>
        /// Gets or sets the parent ID.
        /// </summary>
        /// <value>The parent ID.</value>
        public int ParentId
        {
            get { return m_ParentmFunctionSeqId; }
            set { m_ParentmFunctionSeqId = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [redirect on timeout].
        /// </summary>
        /// <value><c>true</c> if [redirect on timeout]; otherwise, <c>false</c>.</value>
        public bool RedirectOnTimeout { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Models.Base;
using GrowthWare.Framework.Models.UI;

namespace GrowthWare.Framework.Models;

/// <summary>
/// Class MFunctionProfile
/// </summary>
[Serializable(), CLSCompliant(true)]
public class MFunctionProfile : AbstractGroupRolePermissionSecurity
{
    #region Member Objects
        private int m_NavigationTypeSeqId = 2;
        //private int m_ALLOW_HTML_INPUT = 1;
        //private int m_ALLOW_COMMENT_HTML_INPUT = 1;
        private int m_FunctionTypeSeqId = 1;
        private int m_ParentmFunctionSeqId = 1;
        private int m_LinkBehavior = 1;
    #endregion

    #region Public Constructors
        /// <summary>
        /// Will return a Function profile with the default vaules
        /// </summary>
        public MFunctionProfile()
        {
            this.SetupClass();
        }

        /// <summary>
        /// Will return a fully populated Function profile from data.
        /// </summary>
        /// <param name="profileDataRow">A data row containing the Function information</param>
        /// <param name="derivedRoles">A data row containing all of the derived roles</param>
        /// <param name="assignedRoles">A data row containing all of the assigned roles</param>
        /// <param name="groups">A data row containing all of the assigned groups</param>
        /// <remarks></remarks>
        public MFunctionProfile(DataRow profileDataRow, DataRow[] derivedRoles, DataRow[] assignedRoles, DataRow[] groups)
        {
            this.SetupClass();
            this.Initialize(profileDataRow, derivedRoles, assignedRoles, groups);
        }

        /// <summary>
        /// Will return a fully populated Function profile from another Function profile
        /// </summary>
        /// <param name="functionProfile"></param>
        public MFunctionProfile(MFunctionProfile functionProfile)
        {
            this.SetupClass();
            this.Action = functionProfile.Action;
            this.AddedBy = functionProfile.AddedBy;
            this.AddedDate = functionProfile.AddedDate;
            this.Controller = functionProfile.Controller;
            this.Description = functionProfile.Description;
            this.EnableNotifications = functionProfile.EnableNotifications;
            this.EnableViewState = functionProfile.EnableViewState;
            this.FunctionTypeSeqId = functionProfile.FunctionTypeSeqId;
            this.Id = functionProfile.Id;
            // this.IdColumnName = functionProfile.IdColumnName;
            this.IsNavigable = functionProfile.IsNavigable;
            this.LinkBehavior = functionProfile.LinkBehavior;
            this.MetaKeywords = functionProfile.MetaKeywords;
            this.Name = functionProfile.Name;
            // this.NameColumnName = functionProfile.NameColumnName;
            this.NavigationTypeSeqId = functionProfile.NavigationTypeSeqId;
            this.Notes = functionProfile.Notes;
            this.NoUI = functionProfile.NoUI;
            this.ParentId = functionProfile.ParentId;
            this.PermissionColumn = functionProfile.PermissionColumn;
            this.RedirectOnTimeout = functionProfile.RedirectOnTimeout;
            this.RoleColumn = functionProfile.RoleColumn;
            this.Source = functionProfile.Source;
            this.UpdatedBy = functionProfile.UpdatedBy;
            this.UpdatedDate = functionProfile.UpdatedDate;
        }

        /// <summary>
        /// Will return a populated Function profile from a UIFunctionProfile
        /// </summary>
        /// <param name="uIFunctionProfile"></param>
        public MFunctionProfile(UIFunctionProfile uIFunctionProfile)
        {
            this.SetupClass();
            this.Action = uIFunctionProfile.Action;
            this.Controller = uIFunctionProfile.Controller;
            this.Description = uIFunctionProfile.Description;
            this.EnableNotifications = uIFunctionProfile.EnableNotifications;
            this.EnableViewState = uIFunctionProfile.EnableViewState;
            this.FunctionTypeSeqId = uIFunctionProfile.FunctionTypeSeqId;
            this.Id = uIFunctionProfile.Id;
            this.IsNavigable = uIFunctionProfile.IsNavigable;
            this.LinkBehavior = uIFunctionProfile.LinkBehavior;
            this.MetaKeywords = uIFunctionProfile.MetaKeywords;
            this.Name = uIFunctionProfile.Name;
            this.NavigationTypeSeqId = uIFunctionProfile.NavigationTypeSeqId;
            this.NoUI = uIFunctionProfile.NoUI;
            this.Notes = uIFunctionProfile.Notes;
            this.ParentId = uIFunctionProfile.ParentId;
            this.RedirectOnTimeout = uIFunctionProfile.RedirectOnTimeout;
            this.Source = uIFunctionProfile.Source;

            string mRoles = string.Join(",", uIFunctionProfile.AssignedViewRoles);
            this.SetAssignedRoles(mRoles, PermissionType.View);
            mRoles = string.Join(",", uIFunctionProfile.AssignedAddRoles);
            this.SetAssignedRoles(mRoles, PermissionType.Add);
            mRoles = string.Join(",", uIFunctionProfile.AssignedEditRoles);
            this.SetAssignedRoles(mRoles, PermissionType.Edit);
            mRoles = string.Join(",", uIFunctionProfile.AssignedDeleteRoles);
            this.SetAssignedRoles(mRoles, PermissionType.Delete);

            string mGroups = string.Join(",", uIFunctionProfile.ViewGroups);
            this.SetGroups(mGroups, PermissionType.View);
            mGroups = string.Join(",", uIFunctionProfile.AddGroups);
            this.SetGroups(mGroups, PermissionType.Add);
            mGroups = string.Join(",", uIFunctionProfile.EditGroups);
            this.SetGroups(mGroups, PermissionType.Edit);
            mGroups = string.Join(",", uIFunctionProfile.DeleteGroups);
            this.SetGroups(mGroups, PermissionType.Delete);
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
        /// Intended to be used to send notifications when this profile is "used" by the client
        /// </summary>
        [DBColumnName("Enable_Notifications")]
        public bool EnableNotifications { get; set; }

        /// <summary>
        /// Indicates to the system if the "page's" view state should be enabled.
        /// </summary>
        /// <remarks>Legacy usage</remarks>
        [DBColumnName("Enable_View_State")]
        public bool EnableViewState { get; set; }

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

        [DBPrimaryKey]
        [DBColumnName("FunctionSeqId")]
        public int Id { get; set; }

        /// <summary>
        /// Use to determin if a function is a navigation function
        /// </summary>
        /// <remarks>
        /// Should be replaced by LinkBehavior
        /// </remarks>
        [DBColumnName("Is_Nav")]
        public bool IsNavigable { get; set; }

        /// <summary>
        /// Represents the link behavior of a function.
        /// </summary>
        /// <returns>Integer</returns>
        /// <remarks>
        /// Data stored in ZGWSecurity.Functions and related to ZGWCoreWeb.Link_Behaviors
        /// </remarks>
        [DBColumnName("Link_Behavior")]
        public int LinkBehavior
        {
            get { return m_LinkBehavior; }
            set { m_LinkBehavior = value; }
        }

        /// <summary>
        /// Gets or sets the meta key words.
        /// </summary>
        /// <value>The meta key words.</value>
        [DBColumnName("Meta_Key_Words")]
        public string MetaKeywords { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the navigation type seq id.
        /// </summary>
        /// <value>The navigation type seq id.</value>
        [DBColumnName("Navigation_Types_NVP_DetailSeqId")]
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
        [DBColumnName("No_UI")]
        public bool NoUI { get; set; }

        /// <summary>
        /// Gets or sets the parent ID.
        /// </summary>
        /// <value>The parent ID.</value>
        [DBColumnName("ParentSeqId")]
        public int ParentId
        {
            get { return m_ParentmFunctionSeqId; }
            set { m_ParentmFunctionSeqId = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether [redirect on timeout].
        /// </summary>
        /// <value><c>true</c> if [redirect on timeout]; otherwise, <c>false</c>.</value>
        [DBColumnName("Redirect_On_Timeout")]
        public bool RedirectOnTimeout { get; set; }

        public string Resolve { get; set; }

        [DBColumnName("Sort_Order")]
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>The source.</value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the controller.
        /// </summary>
        /// <value>The controller.</value>
        public string Controller { get; set; }

    #endregion

    protected override void SetupClass()
    {
        base.m_ForeignKeyName = "NOT_USED";
        m_TableName = "[ZGWSecurity].[Functions]";
        this.Id = -1;
    }

    /// <summary>
    /// Initializes the specified profile with the given DataRow.
    /// </summary>
    /// <param name="profileDataRow">The profile DataRow.</param>
    /// <param name="derivedRoles">The derived roles.</param>
    /// <param name="assignedRoles">The assigned roles.</param>
    /// <param name="groups">The groups.</param>
    internal new void Initialize(DataRow profileDataRow, DataRow[] derivedRoles, DataRow[] assignedRoles, DataRow[] groups)
    {
        m_FunctionTypeSeqId = base.GetInt(profileDataRow, "FUNCTION_TYPE_SEQ_ID");
        Description = base.GetString(profileDataRow, "DESCRIPTION");
        Notes = base.GetString(profileDataRow, "NOTES");
        Source = base.GetString(profileDataRow, "SOURCE");
        Controller = base.GetString(profileDataRow, "Controller");
        EnableViewState = base.GetBool(profileDataRow, "ENABLE_VIEW_STATE");
        EnableNotifications = base.GetBool(profileDataRow, "ENABLE_NOTIFICATIONS");
        RedirectOnTimeout = base.GetBool(profileDataRow, "REDIRECT_ON_TIMEOUT");
        Id = base.GetInt(profileDataRow, "FUNCTION_SEQ_ID");
        IsNavigable = base.GetBool(profileDataRow, "IS_NAV");
        LinkBehavior = base.GetInt(profileDataRow, "Link_Behavior");
        NoUI = base.GetBool(profileDataRow, "No_UI");
        m_NavigationTypeSeqId = base.GetInt(profileDataRow, "NAVIGATION_NVP_SEQ_DET_ID");
        m_ParentmFunctionSeqId = base.GetInt(profileDataRow, "PARENT_Function_Seq_ID");
        Action = base.GetString(profileDataRow, "ACTION");
        Name = base.GetString(profileDataRow, "Name");
        MetaKeywords = base.GetString(profileDataRow, "META_KEY_WORDS");
        base.Initialize(profileDataRow, derivedRoles, assignedRoles, groups);
    }

}

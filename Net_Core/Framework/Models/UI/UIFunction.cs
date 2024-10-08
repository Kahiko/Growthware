using System.Collections.Generic;
using System.Collections.ObjectModel;
using GrowthWare.Framework.Enumerations;

namespace GrowthWare.Framework.Models.UI;
public class UIFunctionProfile
{
    public UIFunctionProfile()
    {
        this.init();
    }

    public UIFunctionProfile(MFunctionProfile functionProfile)
    {
        this.init();
        this.Action = functionProfile.Action;
        this.Description = functionProfile.Description;
        this.EnableViewState = functionProfile.EnableViewState;
        this.EnableNotifications = functionProfile.EnableNotifications;
        this.FunctionMenuOrders = new List<UIFunctionMenuOrder>();
        this.Id = functionProfile.Id;
        this.IsNavigable = functionProfile.IsNavigable;
        this.LinkBehavior = functionProfile.LinkBehavior;
        this.FunctionTypeSeqId = functionProfile.FunctionTypeSeqId;
        this.MetaKeywords = functionProfile.MetaKeywords;
        this.Name = functionProfile.Name;
        this.NavigationTypeSeqId = functionProfile.NavigationTypeSeqId;
        this.Notes = functionProfile.Notes;
        this.NoUI = functionProfile.NoUI;
        this.ParentId = functionProfile.ParentId;
        this.RedirectOnTimeout = functionProfile.RedirectOnTimeout;
        this.Source = functionProfile.Source;
        this.Controller = functionProfile.Controller;
        string mRoles = functionProfile.GetCommaSeparatedAssignedRoles(PermissionType.Add);
        this.AssignedViewRoles = functionProfile.AssignedViewRoles;
        this.AssignedAddRoles = functionProfile.AssignedAddRoles;
        this.AssignedEditRoles = functionProfile.AssignedEditRoles;
        this.AssignedDeleteRoles = functionProfile.AssignedDeleteRoles;

        this.ViewGroups = functionProfile.ViewGroups;
        this.AddGroups = functionProfile.AddGroups;
        this.EditGroups = functionProfile.EditGroups;
        this.DeleteGroups = functionProfile.DeleteGroups;

        this.DerivedViewRoles = functionProfile.DerivedViewRoles;
        this.DerivedAddRoles = functionProfile.DerivedAddRoles;
        this.DerivedEditRoles = functionProfile.DerivedEditRoles;
        this.DerivedDeleteRoles = functionProfile.DerivedDeleteRoles;
    }

    private void init() 
    { 
        this.Id = -1;
        this.FunctionTypeSeqId = 1;
        this.Groups = new string[]{};
        this.LinkBehavior = 1;
        this.NavigationTypeSeqId = 2;
        this.ParentId = 1;
        this.Roles = new string[]{};
    }

    public string[] Groups;
    public string[] Roles;
    public bool CanSaveRoles{get; set;}
    public bool CanSaveGroups{get; set;}

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

    public MDirectoryProfile DirectoryData { get; set; }

    /// <summary>
    /// Indicates to the system if the "page's" view state should be enabled.
    /// </summary>
    /// <remarks>Legacy usage</remarks>
    public bool EnableViewState { get; set; }

    /// <summary>
    /// Intended to be used to send notifications when this profile is "used" by the client
    /// </summary>
    public bool EnableNotifications { get; set; }

    public List<UIFunctionMenuOrder> FunctionMenuOrders { get; set; }

    public int Id{get; set;}

    public Collection<string> AssignedViewRoles{get; set;}
    public Collection<string> AssignedAddRoles{get; set;}
    public Collection<string> AssignedEditRoles{get; set;}
    public Collection<string> AssignedDeleteRoles{get; set;}

    public Collection<string> DerivedViewRoles{get; set;}
    public Collection<string> DerivedAddRoles{get; set;}
    public Collection<string> DerivedEditRoles{get; set;}
    public Collection<string> DerivedDeleteRoles{get; set;}

    public Collection<string> ViewGroups{get; set;}
    public Collection<string> AddGroups{get; set;}
    public Collection<string> EditGroups{get; set;}
    public Collection<string> DeleteGroups{get; set;}

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
    public int LinkBehavior { get; set; }

    /// <summary>
    /// Represents the type of function Module,Security, Menu Item etc
    /// </summary>
    /// <value>Integer/int</value>
    /// <returns>Integer/int</returns>
    /// <remarks>
    /// Data stored in ZGWSecurity.Functions related to ZGWSecurity.Function_Types
    /// </remarks>
    public int FunctionTypeSeqId { get; set; }

    /// <summary>
    /// Gets or sets the meta key words.
    /// </summary>
    /// <value>The meta key words.</value>
    public string MetaKeywords { get; set; }

    public string Name{get; set;}

    /// <summary>
    /// Gets or sets the navigation type seq id.
    /// </summary>
    /// <value>The navigation type seq id.</value>
    public int NavigationTypeSeqId { get; set; }

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
    public int ParentId { get; set; }

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

    /// <summary>
    /// Gets or sets the controller.
    /// </summary>
    /// <value>The controller.</value>
    public string Controller { get; set; }

}
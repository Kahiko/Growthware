using GrowthWare.Framework.Enumerations;
using GrowthWare.Framework.Interfaces;
using GrowthWare.Framework.Models.Base;
using System;
using System.Data;
using System.Collections.ObjectModel;
using System.Globalization;

namespace GrowthWare.Framework.Models;

/// <summary>
/// The MBaseSecurity is a abstract class that when inherited will add 4 types of roles
/// to your class/object.  After you have inherited the class pass a data row to the SecurityInit sub 
/// to populate the roles.
/// </summary>
/// <remarks>
/// Currently there are 4 permission roles and they are Add, Edit, View, Delete.  
/// If you would like to extend this class do so by inheriting this class and adding your
/// own types of roles say Moderate if your writing some sort of formum type of class.
/// Any of the objects you create should now inherit your class and will now have all of the 
/// roles from this class as well as the ones for yours.
///</remarks>
[Serializable(), CLSCompliant(true)]
public abstract class AbstractGroupRolePermissionSecurity : AAddedUpdated, IGroupRolePermissionSecurity
{
    private Collection<string> m_AssignedAddRoles = new Collection<string>();
    private Collection<string> m_AssignedDeleteRoles = new Collection<string>();
    private Collection<string> m_AssignedEditRoles = new Collection<string>();
    private Collection<string> m_AssignedViewRoles = new Collection<string>();

    private Collection<string> m_DerivedAddRoles = new Collection<string>();
    private Collection<string> m_DerivedDeleteRoles = new Collection<string>();
    private Collection<string> m_DerivedEditRoles = new Collection<string>();
    private Collection<string> m_DerivedViewRoles = new Collection<string>();

    private Collection<string> m_AddGroups = new Collection<string>();
    private Collection<string> m_DeleteGroups = new Collection<string>();
    private Collection<string> m_EditGroups = new Collection<string>();
    private Collection<string> m_ViewGroups = new Collection<string>();
    private string m_PermissionColumn = "PERMISSIONS_SEQ_ID";

    private string m_RoleColumn = "ROLE";
    private string m_GroupColumn = "Group";

    /// <summary>
    /// Retruns a comma separated string given a Collection strings
    /// </summary>
    /// <param name="collectionOfStrings">Collecion</param>
    /// <returns>comma seportated string.</returns>
    private static string getCommaSeportatedString(Collection<string> collectionOfStrings)
    {
        String mRetValue = String.Empty;
        if (collectionOfStrings != null)
        {
            if (collectionOfStrings.Count > 0)
            {
                foreach (var item in collectionOfStrings)
                {
                    mRetValue += item.ToString() + ",";
                }
            }
        }
        if (mRetValue.Length > 0)
        {
            mRetValue = mRetValue.Substring(0, mRetValue.Length - 1);
        }
        return mRetValue;
    }

    /// <summary>
    /// Initialize overloads and calls base.Initialize to will populate the Add, Delete, Edit, and View role properties.
    /// </summary>
    /// <param name="profileDataRow">The detail row.</param>
    /// <param name="derivedRoles">The derived roles.</param>
    /// <param name="assignedRoles">The assigned roles.</param>
    /// <param name="groups">The groups.</param>
    internal virtual void Initialize(DataRow profileDataRow, DataRow[] derivedRoles, DataRow[] assignedRoles, DataRow[] groups)
    {
        base.Initialize(profileDataRow);
        setRolesOrGroups(ref m_DerivedAddRoles, derivedRoles, PermissionType.Add, m_RoleColumn);
        setRolesOrGroups(ref m_DerivedDeleteRoles, derivedRoles, PermissionType.Delete, m_RoleColumn);
        setRolesOrGroups(ref m_DerivedEditRoles, derivedRoles, PermissionType.Edit, m_RoleColumn);
        setRolesOrGroups(ref m_DerivedViewRoles, derivedRoles, PermissionType.View, m_RoleColumn);
        if (assignedRoles != null)
        {
            setRolesOrGroups(ref m_AssignedAddRoles, assignedRoles, PermissionType.Add, m_RoleColumn);
            setRolesOrGroups(ref m_AssignedDeleteRoles, assignedRoles, PermissionType.Delete, m_RoleColumn);
            setRolesOrGroups(ref m_AssignedEditRoles, assignedRoles, PermissionType.Edit, m_RoleColumn);
            setRolesOrGroups(ref m_AssignedViewRoles, assignedRoles, PermissionType.View, m_RoleColumn);
        }
        if (groups != null)
        {
            setRolesOrGroups(ref m_AddGroups, groups, PermissionType.Add, m_GroupColumn);
            setRolesOrGroups(ref m_DeleteGroups, groups, PermissionType.Delete, m_GroupColumn);
            setRolesOrGroups(ref m_EditGroups, groups, PermissionType.Edit, m_GroupColumn);
            setRolesOrGroups(ref m_ViewGroups, groups, PermissionType.View, m_GroupColumn);
        }

    }

    /// <summary>
    /// Return assigned roles associated with the "Add" permission.
    /// </summary>
    [DBIgnoreProperty]
    public Collection<string> AssignedAddRoles
    {
        get { return m_AssignedAddRoles; }
    }

    /// <summary>
    /// Return roles associated with the "Add" permission.
    /// </summary>
    [DBIgnoreProperty]
    public Collection<string> DerivedAddRoles
    {
        get { return m_DerivedAddRoles; }
    }

    /// <summary>
    /// Return assigned roles associated with the "Delete" permission.
    /// </summary>
    [DBIgnoreProperty]
    public Collection<string> AssignedDeleteRoles
    {
        get { return m_AssignedDeleteRoles; }
    }

    /// <summary>
    /// Return roles associated with the "Delete" permission.
    /// </summary>
    [DBIgnoreProperty]
    public Collection<string> DerivedDeleteRoles
    {
        get { return m_DerivedDeleteRoles; }
    }

    /// <summary>
    /// Return roles associated with the "Edit" permission.
    /// </summary>
    [DBIgnoreProperty]
    public Collection<string> AssignedEditRoles
    {
        get { return m_AssignedEditRoles; }
    }

    /// <summary>
    /// Return roles associated with the "Edit" permission.
    /// </summary>
    [DBIgnoreProperty]
    public Collection<string> DerivedEditRoles
    {
        get { return m_DerivedEditRoles; }
    }

    /// <summary>
    /// Return assigned roles associated with the "View" permission.
    /// </summary>
    [DBIgnoreProperty]
    public Collection<string> AssignedViewRoles
    {
        get { return m_AssignedViewRoles; }
    }

    /// <summary>
    /// Return roles associated with the "View" permission.
    /// </summary>
    [DBIgnoreProperty]
    public Collection<string> DerivedViewRoles
    {
        get { return m_DerivedViewRoles; }
    }

    /// <summary>
    /// Return groups associated with the "Add" permission.
    /// </summary>
    [DBIgnoreProperty]
    public Collection<string> AddGroups
    {
        get { return m_AddGroups; }
    }

    /// <summary>
    /// Return groups associated with the "Delete" permission.
    /// </summary>
    [DBIgnoreProperty]
    public Collection<string> DeleteGroups
    {
        get { return m_DeleteGroups; }
    }

    /// <summary>
    /// Return groups associated with the "Edit" permission.
    /// </summary>
    [DBIgnoreProperty]
    public Collection<string> EditGroups
    {
        get { return m_EditGroups; }
    }

    /// <summary>
    /// Return groups associated with the "View" permission.
    /// </summary>
    [DBIgnoreProperty]
    public Collection<string> ViewGroups
    {
        get { return m_ViewGroups; }
    }

    /// <summary>
    /// Converts the collection of AssignedGroups to a comma separated string.
    /// </summary>
    /// <returns>String</returns>
    public string GetCommaSeparatedGroups(PermissionType permission)
    {
        switch (permission)
        {
            case PermissionType.Add:
                return getCommaSeportatedString(m_AddGroups);
            case PermissionType.Delete:
                return getCommaSeportatedString(m_DeleteGroups);
            case PermissionType.Edit:
                return getCommaSeportatedString(m_EditGroups);
            case PermissionType.View:
                return getCommaSeportatedString(m_ViewGroups);
            default:
                return null;
        }
    }

    /// <summary>
    /// Converts the collection of AssignedGroups to a comma separated string.
    /// </summary>
    /// <returns>String</returns>
    public string GetCommaSeparatedAssignedRoles(PermissionType permission)
    {
        switch (permission)
        {
            case PermissionType.Add:
                return getCommaSeportatedString(m_AssignedAddRoles);
            case PermissionType.Delete:
                return getCommaSeportatedString(m_AssignedDeleteRoles);
            case PermissionType.Edit:
                return getCommaSeportatedString(m_AssignedEditRoles);
            case PermissionType.View:
                return getCommaSeportatedString(m_AssignedViewRoles);
            default:
                return null;
        }
    }

    /// <summary>
    /// Represents the permission column name.
    /// </summary>
    [DBIgnoreProperty]
    public string PermissionColumn
    {
        get { return m_PermissionColumn; }
        set
        {
            m_PermissionColumn = value;
            if (!String.IsNullOrEmpty(m_PermissionColumn)) m_PermissionColumn = m_PermissionColumn.Trim();
        }
    }

    /// <summary>
    /// Represents the role column name.
    /// </summary>
    [DBIgnoreProperty]
    public string RoleColumn
    {
        get { return m_RoleColumn; }
        set
        {
            m_RoleColumn = value;
            if (!String.IsNullOrEmpty(m_RoleColumn)) m_RoleColumn = m_RoleColumn.Trim();
        }
    }

    /// <summary>
    /// Sets the groups.
    /// </summary>
    /// <param name="commaSeparatedGroups">The comma separated groups.</param>
    /// <param name="permission">The permission.</param>
    public void SetGroups(string commaSeparatedGroups, PermissionType permission)
    {
        switch (permission)
        {
            case PermissionType.Add:
                setRolesOrGroups(ref m_AddGroups, commaSeparatedGroups);
                break;
            case PermissionType.Delete:
                setRolesOrGroups(ref m_DeleteGroups, commaSeparatedGroups);
                break;
            case PermissionType.Edit:
                setRolesOrGroups(ref m_EditGroups, commaSeparatedGroups);
                break;
            case PermissionType.View:
                setRolesOrGroups(ref m_ViewGroups, commaSeparatedGroups);
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// Sets the assigned roles.
    /// </summary>
    /// <param name="commaSeparatedRoles">The comma separated roles.</param>
    /// <param name="permission">The permission.</param>
    public void SetAssignedRoles(string commaSeparatedRoles, PermissionType permission)
    {
        if (!string.IsNullOrEmpty(commaSeparatedRoles))
        {
            switch (permission)
            {
                case PermissionType.Add:
                    setRolesOrGroups(ref m_AssignedAddRoles, commaSeparatedRoles);
                    break;
                case PermissionType.Delete:
                    setRolesOrGroups(ref m_AssignedDeleteRoles, commaSeparatedRoles);
                    break;
                case PermissionType.Edit:
                    setRolesOrGroups(ref m_AssignedEditRoles, commaSeparatedRoles);
                    break;
                case PermissionType.View:
                    setRolesOrGroups(ref m_AssignedViewRoles, commaSeparatedRoles);
                    break;
                default:
                    break;
            }
        }
    }

    /// <summary>
    /// Populates the given permissions roles.
    /// </summary>
    /// <param name="refCollection">reference to the role or group colletion</param>
    /// <param name="roleOrGroups">An array of rows for the role or group</param>
    /// <param name="permissionType">the type of role or group (View, Add, Edit, Delete)</param>
    /// <param name="dataColumnName">Name of the column containg the data... will be different for roles and groups.</param>
    /// <remarks></remarks>
    private void setRolesOrGroups(ref Collection<String> refCollection, DataRow[] roleOrGroups, PermissionType permissionType, String dataColumnName)
    {
        refCollection = new Collection<String>();
        foreach (DataRow row in roleOrGroups)
        {
            if (!Convert.IsDBNull(row[this.m_PermissionColumn]))
            {
                if (!Convert.IsDBNull(row[dataColumnName]))
                {
                    if (int.Parse(row[m_PermissionColumn].ToString(), CultureInfo.InvariantCulture) == (int)permissionType)
                    {
                        refCollection.Add(row[dataColumnName].ToString());
                    }
                }
            }
        }
    }

    /// <summary>
    /// Sets the roles or groups.
    /// </summary>
    /// <param name="stringCollectionObject">The string collection object.</param>
    /// <param name="commaSeparatedString">The comma separated string.</param>
    private static void setRolesOrGroups(ref Collection<String> stringCollectionObject, string commaSeparatedString)
    {
        string[] mRoles = commaSeparatedString.Split(',');
        stringCollectionObject = new Collection<string>();
        foreach (object mRole in mRoles)
        {
            stringCollectionObject.Add(mRole.ToString());
        }

    }
}

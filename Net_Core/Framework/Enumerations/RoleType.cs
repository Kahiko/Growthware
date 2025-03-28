using System;

namespace GrowthWare.Framework.Enumerations;

/// <summary>
/// Enumerates all role types.
/// </summary>
/// <remarks>
/// Closely coupled with table ZF_PERMISSIONS.
/// </remarks>
[Serializable(), CLSCompliant(true)]
public enum RoleType
{
    /// <summary>
    /// AddRole = 3
    /// </summary>
    AddRole = 3,

    /// <summary>
    /// DeleteRole = 4
    /// </summary>
    DeleteRole = 4,

    /// <summary>
    /// EditRole = 2
    /// </summary>
    EditRole = 2,

    /// <summary>
    /// ViewRole = 1
    /// </summary>
    ViewRole = 1,

    /// <summary>
    /// None = 0
    /// </summary>
    None = 0
}


namespace GrowthWare.Framework.Enumerations;

/// <summary>
/// Enumeration of menu types
/// </summary>
/// <remarks>Values match ZF_NAVIGATION_TYPE in the database</remarks>
[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
public enum MenuType
{
    /// <summary>
    /// Represents the Hierarchical value (3)
    /// </summary>
    Hierarchical = 3,
    /// <summary>
    /// Represents the Horizontal value (1)
    /// </summary>
    Horizontal = 1,
    /// <summary>
    /// Represents the Vertical value (2)
    /// </summary>
    Vertical = 2
}

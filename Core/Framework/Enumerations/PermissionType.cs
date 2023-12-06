
namespace GrowthWare.Framework.Enumerations
{
    /// <summary>
    /// Enumeration of permission Types
    /// </summary>
    /// <remarks>
    /// Values match ZF_PERMISSIONS in the database
    /// </remarks>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1008:EnumsShouldHaveZeroValue")]
    public enum PermissionType
    {
        /// <summary>
        /// Represents the Add value (3)
        /// </summary>
        Add = 3,
        /// <summary>
        /// Represents the Delete value (4)
        /// </summary>
        Delete = 4,
        /// <summary>
        /// Represents the Edit value (2)
        /// </summary>
        Edit = 2,
        /// <summary>
        /// Represents the View value (1)
        /// </summary>
        View = 1,
        /// <summary>
        /// Represents the View value (0)
        /// </summary>
        None = 0
    }
}


namespace GrowthWare.Framework.Enumerations
{
    /// <summary>
    /// Enumeration of Encryption Types
    /// </summary>
    /// <remarks>
    /// Used for indicating En/Decryption type.
    /// </remarks>
    public enum EncryptionType
    {
        /// <summary>
		/// Indicates Triple DES encryption
		/// </summary>
		Aes = 4,
        /// <summary>
		/// Indicates Triple DES encryption
		/// </summary>
		TripleDes = 1,
		/// <summary>
		/// Indicates DES encryption
		/// </summary>
		Des = 2,
		/// <summary>
		/// Indicates no encryption
		/// </summary>
		None = 0
    }
}

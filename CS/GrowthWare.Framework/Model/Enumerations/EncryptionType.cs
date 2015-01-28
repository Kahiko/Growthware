using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.Model.Enumerations
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

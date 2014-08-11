using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.Common
{
    /// <summary>
    /// Created to distinguish errors created in the CryptoUtility class.
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    public class CryptoUtilityException : Exception
    {
        		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1032:ImplementStandardExceptionConstructors")]
		public CryptoUtilityException(){}

		/// <summary>
		/// Calls base method
		/// </summary>
		/// <param name="message">string</param>
		public CryptoUtilityException(string message):base(message)
		{
			
		}

		/// <summary>
		/// Calls base method
		/// </summary>
		/// <param name="message">string</param>
		/// <param name="innerException">Exception</param>
		public CryptoUtilityException(string message, Exception innerException):base(message, innerException)
		{
		
		}

		/// <summary>
		/// Calls base method
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
        protected CryptoUtilityException(SerializationInfo info, StreamingContext context)
            : base(info, context)
		{
			// Implement type-specific serialization constructor logic.
		}
    }
}

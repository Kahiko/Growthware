using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.WebSupport
{
    /// <summary>
    /// Created to distinguish errors created in the GrowthWare.WebSupport project.
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    public class WebSupportException : Exception
    {
        /// <summary>
        /// Helps seporate any errors that occure within Web Support 
        /// </summary>
		public WebSupportException(){}

		/// <summary>
		/// Calls base method
		/// </summary>
		/// <param name="message">string</param>
		public WebSupportException(string message):base(message)
		{
			
		}

		/// <summary>
		/// Calls base method
		/// </summary>
		/// <param name="message">string</param>
		/// <param name="innerException">Exception</param>
		public WebSupportException(string message, Exception innerException):base(message, innerException)
		{
		
		}

		/// <summary>
		/// Calls base method
		/// </summary>
		/// <param name="info"></param>
		/// <param name="context"></param>
        protected WebSupportException(SerializationInfo info, StreamingContext context)
            : base(info, context)
		{
			// Implement type-specific serialization constructor logic.
		}

    }
}

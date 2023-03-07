using System;
using System.Runtime.Serialization;

namespace GrowthWare.Web.Support
{
    /// <summary>
    /// Created to distinguish errors created in the data access layer.
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    public class WebSupportException : Exception
    {
        /// <summary>
        /// Creates a new instance of the WebSupportException class
        /// </summary>
        public WebSupportException() { }

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
		public WebSupportException(string message, Exception innerException): base(message, innerException)
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

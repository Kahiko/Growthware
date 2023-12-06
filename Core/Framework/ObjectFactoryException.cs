using System;
using System.Runtime.Serialization;

namespace GrowthWare.Framework
{
    /// <summary>
    /// Created to distinguish errors created in the data access layer.
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    public class ObjectFactoryException : Exception
    {
        /// <summary>
        /// Creates a new instance of the ObjectFactoryException class
        /// </summary>
        public ObjectFactoryException() { }

		/// <summary>
		/// Calls base method
		/// </summary>
		/// <param name="message">string</param>
		public ObjectFactoryException(string message):base(message)
		{
			
		}

		/// <summary>
		/// Calls base method
		/// </summary>
		/// <param name="message">string</param>
		/// <param name="innerException">Exception</param>
		public ObjectFactoryException(string message, Exception innerException):base(message, innerException)
		{
		
		}
    }
}

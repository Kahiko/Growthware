using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GrowthWare.DataAccess
{
    /// <summary>
    /// Created to distinguish errors created in the data access layer.
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    public class DataAccessLayerException : Exception
    {
        /// <summary>
        /// Creates a new instance of the DataAccessLayerException class
        /// </summary>
        public DataAccessLayerException() { }

        /// <summary>
        /// Calls base method
        /// </summary>
        /// <param name="message">string</param>
        public DataAccessLayerException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Calls base method
        /// </summary>
        /// <param name="message">string</param>
        /// <param name="innerException">Exception</param>
        public DataAccessLayerException(string message, Exception innerException)
            : base(message, innerException)
        {

        }
    }
}

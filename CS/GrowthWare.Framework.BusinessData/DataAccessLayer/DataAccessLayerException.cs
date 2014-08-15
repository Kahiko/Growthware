using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer
{
    /// <summary>
    /// Created to distinguish errors created in the data access layer.
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    public class DataAccessLayerException : Exception
    {
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

        /// <summary>
        /// Calls base method
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected DataAccessLayerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Implement type-specific serialization constructor logic.
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.WebSupport
{
    /// <summary>
    /// Created to distinguish errors created in the WebImpersonate.
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    public class CustomWebControlException : Exception
    {
        public CustomWebControlException() { }

        /// <summary>
        /// Calls base method
        /// </summary>
        /// <param name="message">string</param>
        public CustomWebControlException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Calls base method
        /// </summary>
        /// <param name="message">string</param>
        /// <param name="innerException">Exception</param>
        public CustomWebControlException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// Calls base method
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected CustomWebControlException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Implement type-specific serialization constructor logic.
        }
    }
}

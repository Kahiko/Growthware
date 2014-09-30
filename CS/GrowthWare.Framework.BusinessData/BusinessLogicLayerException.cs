using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.BusinessData
{
    /// <summary>
    /// Created to distinguish errors created in the Business LogicLayer Exceptions.
    /// </summary>
    /// <remarks></remarks>
    [Serializable()]
    public class BusinessLogicLayerException : Exception
    {
        /// <summary>
        /// Calls base method
        /// </summary>
        public BusinessLogicLayerException() { }

        /// <summary>
        /// Calls base method
        /// </summary>
        /// <param name="message">string</param>
        public BusinessLogicLayerException(string message)
            : base(message)
        {

        }

        /// <summary>
        /// Calls base method
        /// </summary>
        /// <param name="message">string</param>
        /// <param name="innerException">Exception</param>
        public BusinessLogicLayerException(string message, Exception innerException)
            : base(message, innerException)
        {

        }

        /// <summary>
        /// Calls base method
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected BusinessLogicLayerException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // Implement type-specific serialization constructor logic.
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base
{
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "DDB")]
    public interface IDDBInteraction
    {
        /// <summary>
        /// Sets or Gets the connection string information.
        /// </summary>
        /// <value>String</value>
        /// <returns>String</returns>
        /// <remarks>Can not be blank</remarks>
        String ConnectionString { get; set; }
    }
}

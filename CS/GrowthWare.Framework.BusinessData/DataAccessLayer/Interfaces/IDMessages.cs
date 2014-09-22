using GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces.Base;
using GrowthWare.Framework.Model.Profiles;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GrowthWare.Framework.BusinessData.DataAccessLayer.Interfaces
{
    public interface IDMessages : IDDBInteraction
    {
        /// <summary>
        /// Gets a subset of information from the database 
        /// </summary>
        /// <param name="searchCriteria"></param>
        /// <returns></returns>
        DataTable Search(MSearchCriteria searchCriteria);

        /// <summary>
        /// Gets or sets the profile.
        /// </summary>
        /// <value>The profile.</value>
        MMessageProfile Profile { get; set; }

        /// <summary>
        /// Gets or sets the security entity seq ID.
        /// </summary>
        /// <value>The security entity seq ID.</value>
        int SecurityEntitySeqId { get; set; }

        /// <summary>
        /// Gets all messages.
        /// </summary>
        /// <returns>DataTable.</returns>
        DataTable Messages();

        /// <summary>
        /// Gets the message.
        /// </summary>
        /// <returns>DataRow.</returns>
        DataRow Message(int messageSeqId);

        /// <summary>
        /// Saves this instance.
        /// </summary>
        void Save();
    }
}
